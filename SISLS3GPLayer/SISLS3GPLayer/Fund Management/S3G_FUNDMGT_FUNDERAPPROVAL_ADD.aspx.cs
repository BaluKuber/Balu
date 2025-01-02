using System;
using System.Web.Security;
using System.Globalization;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using S3GBusEntity;
using System.Configuration;
using System.Text;


public partial class Fund_Management_S3G_FUNDMGT_FUNDERAPPROVAL_ADD : ApplyThemeForProject
{
    #region Initialization
    int intCompanyID = 0;
    int intApproval_ID = 0;
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
    FunderMgtServiceReference.FundMgtServiceClient ObjApproval;
    S3GBusEntity.FundManagement.FundMgtServices.S3G_FUNDMGT_FunderApprovalDataTable ObjApproval_DataTable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_FUNDMGT_FunderApprovalDataTable();
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    string strPageName = "Funder Limit Approval";
    public static Fund_Management_S3G_FUNDMGT_FUNDERAPPROVAL_ADD obj_Page;
    #endregion

    #region EVENTS

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ProgramCode = "288";
            obj_Page = this;
            if (Request.QueryString["qsFunderApprovalId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsFunderApprovalId"));
                intApproval_ID = Convert.ToInt32(fromTicket.Name);
            }
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            Label lblCustomerCode = (Label)S3GCustomerPermAddress.FindControl("lblCustomerCode");
            Label lblCustomerName = (Label)S3GCustomerPermAddress.FindControl("lblCustomerName");
            lblCustomerCode.Text = "Funder Code";
            lblCustomerName.Text = "Funder Name";

            if (!IsPostBack)
            {
                btnRevoke.Enabled = false;
                PaymentReqID.Visible = false;
                ViewState["strCreate"] = "0";
                if (intApproval_ID > 0)
                {
                    FunPriEnableDisableControls();
                    FunPubViewApprovedDetails();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                }
                else
                {
                    //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    lblHeading.Text = "Funder Limit Approval";
                    btnSave.Enabled = btnClear.Enabled = btnRevoke.Enabled = btnGo.Enabled = false;
                }
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
            DataTable dtSanction = new DataTable();
            dtSanction.Columns.Add("IsChecked");
            dtSanction.Columns.Add("SanctionNumber");
            foreach (GridViewRow gv in gvSanctionDtl.Rows)
            {
                CheckBox chkSelect = (CheckBox)gv.FindControl("chkSelect");
                Label lblSanctionNo = (Label)gv.FindControl("lblSanctionNo");
                if (chkSelect.Checked == true)
                {
                    DataRow dr = dtSanction.NewRow();
                    dr["IsChecked"] = 1;
                    dr["SanctionNumber"] = Convert.ToString(lblSanctionNo.Text);
                    dtSanction.Rows.Add(dr);
                }
            }

            if (dtSanction.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast One Sanction Details");
                return;
            }

            Label lblApprovalSNO = new Label();
            Label lblFunderApproval_ID = new Label();
            TextBox txtRemarks = new TextBox();
            TextBox txtPassword = new TextBox();
            DropDownList ddlAction = new DropDownList();
            Label lblApprovalDate = new Label();
            foreach (GridViewRow grv in grvApprovalDetails.Rows)
            {
                lblApprovalSNO = (Label)grv.FindControl("lblApprovalSNO");
                lblFunderApproval_ID = (Label)grv.FindControl("lblFunderApproval_ID");
                ddlAction = (DropDownList)grv.FindControl("ddlAction");
                txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                txtPassword = (TextBox)grv.FindControl("txtPassword");
                lblApprovalDate = (Label)grv.FindControl("lblApprovalDate");
            }
            S3GBusEntity.FundManagement.FundMgtServices.S3G_FUNDMGT_FunderApprovalRow ObjRow;

            ObjRow = ObjApproval_DataTable.NewS3G_FUNDMGT_FunderApprovalRow();
            ObjRow.Funder_ID = Convert.ToInt32(ddlFunderCode.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(lblFunderApproval_ID.Text))
                ObjRow.FunderApproval_ID = Convert.ToInt32(lblFunderApproval_ID.Text);
            else
                ObjRow.FunderApproval_ID = 0;
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
            //ObjRow.XML_Sanction_Dtl = Utility.FunPubFormXml(gvSanctionDtl, true, false);
            ObjRow.XML_Sanction_Dtl = FunPriGenerateSanctionXMLDtl(dtSanction);
            ObjRow.Customer_ID = Convert.ToInt32(ddlLessee.SelectedValue);

            ObjApproval_DataTable.AddS3G_FUNDMGT_FunderApprovalRow(ObjRow);
            ObjApproval = new FunderMgtServiceReference.FundMgtServiceClient();
            string strApprovalNumber = "";
            Int64 intApproval_No = 0;
            int intErrorCode = ObjApproval.FunPubInsertApproval(out strApprovalNumber, out intApproval_No, SerMode, ClsPubSerialize.Serialize(ObjApproval_DataTable, SerMode));
            if (intErrorCode == 0)
            {
                string strAlert = "";

                if (intApproval_No > 0)
                {
                    DataTable WFFP = new DataTable();
                    // By Palani Kumar.A for Customer Name remove after Concatenation
                    string strBusinessNo = string.Empty;
                    if (!string.IsNullOrEmpty(ddlFunderCode.SelectedText.ToString()))
                    {
                        strBusinessNo = ddlFunderCode.SelectedText.Substring(0, ddlFunderCode.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();
                    }
                    else
                    {
                        strBusinessNo = ddlFunderCode.SelectedText.ToString();
                    }
                }
                btnSave.Enabled = false;

                if (ddlAction.SelectedValue == "4")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Funder Limit Approved successfully.');window.location.href='../Fund Management/S3G_FUNDMGT_FunderApproval_View.aspx';", true);
                }
                else if (ddlAction.SelectedValue == "3")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Funder Limit Rejected successfully.');window.location.href='../Fund Management/S3G_FUNDMGT_FunderApproval_View.aspx';", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('User does not have access to Approve');", true);
                return;
            }
            else if (intErrorCode == 50)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Access Denied.');", true);
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
            if (Convert.ToInt32(ddlFunderCode.SelectedValue) > 0 && Convert.ToInt32(ddlLessee.SelectedValue) > 0)
            {
                FormsAuthenticationTicket Funder = new FormsAuthenticationTicket(ddlFunderCode.SelectedValue.ToString(), false, 0);
                FormsAuthenticationTicket Lessee = new FormsAuthenticationTicket(ddlLessee.SelectedValue.ToString(), false, 0);
                hdnID.Value = "../Fund Management/S3G_ORG_Funder_Master.aspx?qsFunderId=" + FormsAuthentication.Encrypt(Funder) + "&qsCustomerId=" + FormsAuthentication.Encrypt(Lessee) + "&qsMode=Q&Popup=yes";
                if (hdnID.Value.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Funder Code.');", true);
                    return;
                }
                //Response.Write(hdnID.Value.ToString());
            }
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
            ddlFunderCode.Clear();
            ddlLessee.Clear();
            btnGo.Enabled = false;
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
                Response.Redirect("~/Fund Management/S3G_FUNDMGT_FunderApproval_View.aspx");
        }

        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }


    protected void ddlFunderCode_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            ddlLessee.Clear();
            if (ddlFunderCode.SelectedValue != "0")
            {
                FunPubClear();
                FunPriShowCustomerDetails();
                ddlFunderCode.ToolTip = ddlFunderCode.SelectedText.ToString();
            }
            else
            {
                FunPubClear();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }


    protected void ddlLessee_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            grvApprovalDetails.DataSource = dsApplicationNumberDetails.Tables.Add();  //make empty grid
            grvApprovalDetails.DataBind();

            gvSanctionDtl.DataSource = null;
            gvSanctionDtl.DataBind();

            pnlSanctionDtls.Visible = false;
            btnSave.Enabled = btnRevoke.Enabled = btnClear.Enabled = false;

            btnGo.Enabled = PaymentReqID.Visible = true;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
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

    #endregion

    #region METHODS

    public void FunPubViewApprovedDetails()
    {
        try
        {
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@OPTION", "2");
            dictDropdownListParam.Add("@Funder_Approval_ID", Convert.ToString(intApproval_ID));
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            DataSet dsApproval = Utility.GetDataset("S3G_ORG_GetFunderDtls_Approval", dictDropdownListParam);
            dsApplicationNumberDetails = dsApproval;
            if (dsApproval != null)
            {
                txtStatus.Text = Convert.ToString(dsApproval.Tables[0].Rows[0]["Status"]);
                txtCreationDate.Text = Convert.ToString(dsApproval.Tables[0].Rows[0]["Creation_Date"]);
                S3GCustomerPermAddress.SetCustomerDetails(dsApproval.Tables[1].Rows[0], true);

                ddlFunderCode.SelectedValue = Convert.ToString(dsApproval.Tables[0].Rows[0]["Funder_ID"]);
                ddlFunderCode.SelectedText = Convert.ToString(dsApproval.Tables[0].Rows[0]["Funder_Code"]);

                ddlLessee.SelectedText = Convert.ToString(dsApproval.Tables[0].Rows[0]["Customer_Name"]);
                ddlLessee.SelectedValue = Convert.ToString(dsApproval.Tables[0].Rows[0]["Customer_ID"]);

                grvApprovalDetails.DataSource = dsApproval.Tables[2];
                grvApprovalDetails.DataBind();

                gvSanctionDtl.DataSource = dsApproval.Tables[4];
                gvSanctionDtl.DataBind();

                gvSanctionDtl.Columns[gvSanctionDtl.Columns.Count - 1].Visible = false;

                PaymentReqID.Visible = pnlSanctionDtls.Visible = true;
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
            ddlFunderCode.Enabled = ddlLessee.Enabled = btnSave.Enabled = btnClear.Enabled = btnGo.Enabled = false;
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
            ddlFunderCode.Clear();
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
            ddlFunderCode.Clear();
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
            if (ddlFunderCode.SelectedValue != "0")
            {
                uinfo = new UserInfo();
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@User_ID", Convert.ToString(uinfo.ProUserIdRW));
                dictDropdownListParam.Add("@Funder_ID", Convert.ToString(ddlFunderCode.SelectedValue));
                dictDropdownListParam.Add("@Customer_ID", Convert.ToString(ddlLessee.SelectedValue));
                dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictDropdownListParam.Add("@OPTION", "1");
                dsApplicationNumberDetails = Utility.GetDataset("S3G_ORG_GetFunderDtls_Approval ", dictDropdownListParam);


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

                if (dsApplicationNumberDetails.Tables[4].Rows.Count > 0)
                {
                    pnlSanctionDtls.Visible = true;
                    gvSanctionDtl.DataSource = dsApplicationNumberDetails.Tables[4];
                    gvSanctionDtl.DataBind();
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

            gvSanctionDtl.DataSource = null;
            gvSanctionDtl.DataBind();

            PaymentReqID.Visible = pnlSanctionDtls.Visible = false;
            btnSave.Enabled = btnRevoke.Enabled = btnClear.Enabled = false;
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
            dictDropdownListParam.Add("@Funder_ID", ddlFunderCode.SelectedValue.ToString());
            dictDropdownListParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictDropdownListParam.Add("@OPTION", "1");
            DataSet dsFunderDtls = Utility.GetDataset("S3G_ORG_GetFunderDtls_Approval ", dictDropdownListParam);
            if (dsFunderDtls.Tables[0].Rows.Count > 0)
            {
                txtStatus.Text = dsFunderDtls.Tables[0].Rows[0]["Name"].ToString();
                txtCreationDate.Text = Convert.ToString(dsFunderDtls.Tables[0].Rows[0]["Creation_Date"]);

                S3GCustomerPermAddress.SetCustomerDetails(dsFunderDtls.Tables[1].Rows[0], true);

                txtStatus.ReadOnly = txtCreationDate.ReadOnly = btnGo.Enabled = true;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private string FunPriGenerateSanctionXMLDtl(DataTable dtSanctionDtl)
    {
        String strXML = string.Empty;
        StringBuilder strSanctionDtl = new StringBuilder();
        try
        {
            strSanctionDtl.Append("<Root>");
            foreach (DataRow drow in dtSanctionDtl.Rows)
            {
                strSanctionDtl.Append("<Details ");
                strSanctionDtl.Append(" SANCTIONNUMBER = '" + Convert.ToString(drow["SanctionNumber"]) + "'");
                strSanctionDtl.Append(" ISCHECKED = '" + Convert.ToString(drow["IsChecked"]) + "'");
                strSanctionDtl.Append(" />");
            }
            strSanctionDtl.Append("</Root>");
            strXML = Convert.ToString(strSanctionDtl);
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return strXML;
    }

    #endregion

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetFunderCode(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetFunderNumber_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLesseeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Option", "8");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@User_ID", Convert.ToString(obj_Page.intUserID));
        if (Convert.ToString(obj_Page.ddlFunderCode.SelectedText) != "")
        {
            Procparam.Add("@Funder_ID", Convert.ToString(obj_Page.ddlFunderCode.SelectedValue));
        }
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));

        return suggetions.ToArray();
    }
}