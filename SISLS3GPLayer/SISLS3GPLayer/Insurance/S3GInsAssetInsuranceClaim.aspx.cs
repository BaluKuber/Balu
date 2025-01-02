using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Text;
using System.Globalization;
using S3GBusEntity;

public partial class Insurance_S3GInsAssetInsuranceClaim : ApplyThemeForProject
{
    #region [Common Variable declaration]

    DataTable dtClaimDetails;
    int intCompanyId, intUserID = 0;
    int intICNId = 0;
    static int intCustomer = 0;
    //decimal dcmTotalPremium = 0;
    Dictionary<string, string> Procparam = null;
    string strICNId = "0";
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    StringBuilder strICNBuilder = new StringBuilder();
    public static Insurance_S3GInsAssetInsuranceClaim obj_Page;

    StringBuilder strPolicyBuilder = new StringBuilder();
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Insurance/S3GInsAssetInsuranceClaim.aspx?qsMode=C";
    string strRedirectPageAdd = "window.location.href='../Insurance/S3GInsAssetInsuranceClaim.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../Insurance/S3GInsTranslander.aspx?Code=AINC';";
    string strPageName = "Asset Insurance Claim";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    #endregion

    #region [Page Load Event]

    protected void Page_Load(object sender, EventArgs e)
    {
        S3GSession ObjS3GSession = null;
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            //Date Format
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            //End
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            if (intCompanyId == 0)
                intCompanyId = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                intICNId = Convert.ToInt32(fromTicket.Name);
            }

            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            obj_Page = this;
            if (!IsPostBack)
            {
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                strMode = Request.QueryString["qsMode"].ToString();
                if (strMode == "Q")
                {
                    FunPriDisableControls(-1);
                    ucCustomerCodeLov.ButtonEnabled = false;
                }
                if (strMode == "M")
                {
                    FunPriDisableControls(1);
                }
                if (strMode != "Q" && strMode != "M")
                {
                    FunPriLoadLOV();
                    txtICNDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);
                    FunPriDisableControls(0);
                    FunPriBindPolicyDetailsDLL("Add");
                    //FunPriLoadDependentLOV();
                    chkActive.Checked = true;
                }
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Attributes.Add("onfocus", "fnLoadCustomer();");
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
            GvHistorytxtRemarks();
        }

        catch (Exception ex)
        {
            cvAssetInsuranceClaim.ErrorMessage = "Unable to load the claim details";
            cvAssetInsuranceClaim.IsValid = false;
        }
        finally
        {
            ObjS3GSession = null;
        }
    }

    #endregion

    #region  Policy Details Grid

    private void FunPriBindPolicyDetailsDLL(string Mode)
    {
        try
        {
            if (Mode == "Add")
            {
                DataTable ObjDT = new DataTable();
                ObjDT.Columns.Add("AssetClaimNo");
                ObjDT.Columns.Add("ClaimDate");
                ObjDT.Columns.Add("AlertDate");
                ObjDT.Columns.Add("MachineNo");
                ObjDT.Columns.Add("AssetDescription");
                ObjDT.Columns.Add("InsuredBy");
                ObjDT.Columns.Add("Ins_Company_Name");
                ObjDT.Columns.Add("PolicyNo");
                ObjDT.Columns.Add("PolicySetupDate");
                ObjDT.Columns.Add("PolicyExpiryDate");
                ObjDT.Columns.Add("InsuredAmount");
                ObjDT.Columns.Add("ClaimAmount");
                ObjDT.Columns.Add("StatusId");
                ObjDT.Columns.Add("Status");
                ObjDT.Columns.Add("Remarks");

                DataRow drClaim = ObjDT.NewRow();
                drClaim["AssetClaimNo"] = "";
                drClaim["ClaimDate"] = "";
                drClaim["AlertDate"] = "";
                drClaim["MachineNo"] = "";
                drClaim["AssetDescription"] = "";
                drClaim["InsuredBy"] = "";
                drClaim["Ins_Company_Name"] = "";
                drClaim["PolicyNo"] = "";
                drClaim["PolicySetupDate"] = "";
                drClaim["PolicyExpiryDate"] = "";
                drClaim["InsuredAmount"] = "";
                drClaim["ClaimAmount"] = "";
                drClaim["StatusId"] = "";
                drClaim["Status"] = "";
                drClaim["Remarks"] = "";

                ObjDT.Rows.Add(drClaim);

                gvPolicyDetails.DataSource = ObjDT;
                gvPolicyDetails.DataBind();

                ObjDT.Rows.Clear();
                ViewState["dtClaimDetails"] = ObjDT;

                //gvPolicyDetails.Rows[0].Cells.Clear();
                //gvPolicyDetails.Rows[0].Visible = false;
                //FunPriGenerateNewPolicyRow();
                gvPolicyDetails.FooterRow.Visible = false;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    #endregion

    #region [To populate LOV]

    private void FunPriLoadLOV()
    {
        try
        {
            strMode = Request.QueryString["qsMode"].ToString();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            //Procparam.Add("@OPTION", "6");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@User_Id", Convert.ToString(intUserID));
            Procparam.Add("@Program_ID", Convert.ToString(141));
            Procparam.Add("@FilterOption", "'FL','HP','LN','TE','TL'");
            if (intICNId == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            ddlLOB.BindDataTable("S3G_Get_LOB_LIST", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            Procparam.Clear();
            if (strMode != "C")
            {
                //Procparam.Add("@OPTION", "4");
                //Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                //Procparam.Add("@User_Id", Convert.ToString(intUserID));
                //Procparam.Add("@Program_ID", Convert.ToString(141));
                //if (intICNId == 0)
                //{
                //    Procparam.Add("@Is_Active", "1");
                //}
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
                Procparam.Clear();
            }
            //Procparam.Add("@OPTION", "2");
            //ddlPayType.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            //Procparam.Clear();
            //Procparam.Add("@OPTION", "3");
            //ddlInsuranceDoneBy.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Program_Id", "141");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    #endregion

    #region [Load LOV]

    private void FunPriLoadDependentLOV()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "13");
            Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
            if (hdnCustomerID.Value != "")
            {
                Procparam.Add("@CUSTOMER_ID", hdnCustomerID.Value);
            }
            if (ddlLOB.SelectedIndex > 0)
            {
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            }
            if (Convert.ToInt32( ddlBranch.SelectedValue) > 0)
            {
                Procparam.Add("@LOCATION_ID", Convert.ToString(ddlBranch.SelectedValue));
            }
            ddlMLA.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "PANUM", "PANUM" });


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region [UserAuthorization]
    ////This is used to implement User Authorization
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                chkActive.Enabled = false;
                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                FunPriLoadLOV();
                FunPriLoadClaimDetails();
                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    ddlBranch.Enabled = false;
                    ddlMLA.ClearDropDownList();
                    ddlSLA.ClearDropDownList();
                }
                rfvSubAccount.Enabled = false;

                if (ddlSLA.SelectedItem.Text == "--Select--")
                {
                   lblSLA.CssClass = "styleDisplayLabel";
                }
                else
                {
                    lblSLA.CssClass = "styleReqFieldLabel";
                }
                ucCustomerCodeLov.ButtonEnabled = false;

                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                btnClear.Enabled = false;
                break;

            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                FunPriLoadLOV();
                FunPriLoadClaimDetails();
                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    ddlBranch.Enabled = false;
                    ddlMLA.ClearDropDownList();
                    ddlSLA.ClearDropDownList();
                }
                ucCustomerCodeLov.ButtonEnabled = false;

                //gvPolicyDetails.Columns[13].Visible =
                gvPolicyDetails.FooterRow.Visible = false;

                btnSave.Enabled = false;
                chkActive.Enabled = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                btnClear.Enabled = false;
                break;
        }
    }

    private void FunPriLoadClaimDetails()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@CompanyId", intCompanyId.ToString());
        Procparam.Add("@InsClaimId", intICNId.ToString());
        DataSet dsAINC = Utility.GetDataset("S3G_INS_getClaimDetails", Procparam);
        if (dsAINC != null)
        {
            if (dsAINC.Tables[0] != null && dsAINC.Tables[0].Rows.Count > 0)
            {
                txtICNNO.Text = dsAINC.Tables[0].Rows[0]["ICN_No"].ToString();
                txtICNDate.Text = dsAINC.Tables[0].Rows[0]["ICN_Date"].ToString();
                ddlLOB.SelectedValue = dsAINC.Tables[0].Rows[0]["Lob_Id"].ToString();
                ddlBranch.SelectedValue = dsAINC.Tables[0].Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = dsAINC.Tables[0].Rows[0]["Location_Name"].ToString();
                FunPriLoadDependentLOV();
                ddlMLA.SelectedValue = dsAINC.Tables[0].Rows[0]["Panum"].ToString();
                FunPriLoadSLA();
                ddlSLA.SelectedValue = dsAINC.Tables[0].Rows[0]["Sanum"].ToString();
                chkActive.Checked = Convert.ToBoolean(dsAINC.Tables[0].Rows[0]["Is_Active"]);
                hdnCustomerID.Value = dsAINC.Tables[0].Rows[0]["Customer_ID"].ToString(); 
            }            
            FunPubQueryExistCustomerList(Convert.ToInt32(hdnCustomerID.Value));
            //ViewState["DtCheckPolicyDetails"] = dsAINC.Tables[3];
            if (dsAINC.Tables[1].Rows.Count == 0)
            {
                FunPriBindPolicyDetailsDLL("Add");
            }
            else
            {
                if (dsAINC.Tables[1] != null && dsAINC.Tables[1].Rows.Count > 0)
                {
                    ViewState["dtClaimDetails"] = dsAINC.Tables[1];
                    gvPolicyDetails.DataSource = dsAINC.Tables[1];
                    gvPolicyDetails.DataBind();

                    if (dsAINC.Tables[1].Rows[0]["StatusId"].ToString() == "2")
                    {
                        btnSave.Enabled = false;
                        btnClear.Enabled = false;
                        chkActive.Enabled = false;
                        //gvPolicyDetails.Enabled = false;
                    }
                }
               
            }
            //FunPriGenerateNewPolicyRow();
            if (dsAINC.Tables[2]!=null && dsAINC.Tables[2].Rows.Count>0)
            {
                gvHistory.DataSource = dsAINC.Tables[2];
                gvHistory.DataBind();
                gvPolicyDetails.FooterRow.Visible = false;
            }
            
        }
    }

    private bool FunPriValidateAccountDetails()
    {
        dtClaimDetails = (DataTable)ViewState["AccountDetails"];

        if (ViewState["AccountDetails"] == null)
        {
            strAlert = strAlert.Replace("__ALERT__", "Enter atleast one Account Details By Select a Customer");
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            return false;
        }
        if (dtClaimDetails.Rows[0]["PrimeAccountNo"].Equals("0"))
        {
            strAlert = strAlert.Replace("__ALERT__", "Enter a Account Details ");
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            return false;
        }
        return true;
    }

    #endregion

    #region [ButtonEvents]

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            
            
            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCID != null && hdnCID.Value != "")
            {
                hdnCustomerID.Value = "";
                S3GCustomerAddress1.ClearCustomerDetails();
                FunPubQueryExistCustomerList(Convert.ToInt32(hdnCID.Value));
                hdnCustomerID.Value = hdnCID.Value.ToString();
                FunPriLoadDependentLOV();
                ddlLOB.SelectedValue = "0";
                ddlBranch.SelectedValue = "0";
                ddlSLA.Items.Clear();
                gvHistory.DataSource = null;
                gvHistory.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../Insurance/S3GInsTranslander.aspx?Code=AINC");
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadLOV();
            //ddlBranch.Items.Clear();
            txtICNNO.Text = string.Empty;
            ucCustomerCodeLov.FunPubClearControlValue();
            S3GCustomerAddress1.ClearCustomerDetails();
            ddlMLA.SelectedValue = "0";
            ddlBranch.Clear();
            //ddlMLA.ClearDropDownList();
            ddlSLA.Items.Clear();
            //gvHistory.Visible = false;
            gvHistory.DataSource = null;
            gvHistory.DataBind();

            if (gvPolicyDetails.Rows.Count > 0)
            {
                TextBox lblAssetClaimNumber = (TextBox)gvPolicyDetails.Rows[0].FindControl("lblAssetClaimNumber");
                Label lblAssetDescriptionEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblAssetDescriptionEdit");
                Label lblInsuredByEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblInsuredByEdit");
                Label lblPolicyNumberEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblPolicyNumberEdit");
                Label lblInsuredAmountEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblInsuredAmountEdit");
                TextBox lblClaimAmount = (TextBox)gvPolicyDetails.Rows[0].FindControl("lblClaimAmount");
                TextBox lblRemarks = (TextBox)gvPolicyDetails.Rows[0].FindControl("lblRemarks");
                DropDownList ddlStatusEdit = (DropDownList)gvPolicyDetails.Rows[0].FindControl("ddlStatusEdit");
                DropDownList ddlMachineNoEdit = (DropDownList)gvPolicyDetails.Rows[0].FindControl("ddlMachineNoEdit");
                Label lblPolicySetupDateEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblPolicySetupDateEdit");
                Label lblPolicyExpiryDateEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblPolicyExpiryDateEdit");
                TextBox lblAssetClaimDate = (TextBox)gvPolicyDetails.Rows[0].FindControl("lblAssetClaimDate");
                Label lblInsurerNameEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblInsurerNameEdit");
                TextBox txtAssetAlertDate = (TextBox)gvPolicyDetails.Rows[0].FindControl("txtAssetAlertDate");

                lblAssetClaimNumber.Text = "";
                lblAssetDescriptionEdit.Text = "";
                lblInsuredByEdit.Text = "";
                lblPolicyNumberEdit.Text = "";
                lblInsuredAmountEdit.Text = "";
                lblClaimAmount.Text = "";
                lblRemarks.Text = "";
                lblPolicySetupDateEdit.Text = "";
                lblPolicyExpiryDateEdit.Text = "";
                lblAssetClaimDate.Text = "";
                ddlStatusEdit.SelectedValue = "0";
                ddlMachineNoEdit.SelectedValue = "0";
                lblInsurerNameEdit.Text = "";
                txtAssetAlertDate.Text = "";
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        InsuranceMgtServicesReference.InsuranceMgtServicesClient objInsuranceClient = new InsuranceMgtServicesReference.InsuranceMgtServicesClient();

        try
        {

            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            if (txtName != null)
            {
                if (txtName.Text == "")
                {
                    strAlert = strAlert.Replace("__ALERT__", "Select a Customer");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    return;
                }
            }
            if (gvPolicyDetails.Rows.Count > 0)
            {
                Label lblInsuredAmt = gvPolicyDetails.Rows[0].FindControl("lblInsuredAmountEdit") as Label;
                TextBox lblClaimAmount = gvPolicyDetails.Rows[0].FindControl("lblClaimAmount") as TextBox;
                DropDownList ddlMachineNoEdit = gvPolicyDetails.Rows[0].FindControl("ddlMachineNoEdit") as DropDownList;
                TextBox lblAssetClaimNumber = gvPolicyDetails.Rows[0].FindControl("lblAssetClaimNumber") as TextBox;

                if (lblAssetClaimNumber != null)
                {
                    if (lblAssetClaimNumber.Text != "")
                    {
                        Procparam = new Dictionary<string, string>();
                        Procparam.Add("@Option", "22");
                        Procparam.Add("@ClaimNo", lblAssetClaimNumber.Text);
                        if (txtICNNO.Text.Trim() != "")
                        {
                            Procparam.Add("@ICNNo", txtICNNO.Text.Trim());
                        }
                        DataTable dtClaimNoExist = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);
                        if (dtClaimNoExist.Rows.Count > 0)
                        {
                            if (dtClaimNoExist.Rows[0][0].ToString() != "0")
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Asset Claim No already Exists");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                                return;
                            }
                        }
                    }
                }

                //Checking for Claim Amount should be less than or equal to Insured Amount
                if (lblInsuredAmt != null && lblClaimAmount != null)
                {
                    if (!string.IsNullOrEmpty(lblInsuredAmt.Text) && !string.IsNullOrEmpty(lblClaimAmount.Text))
                    {
                        if (Convert.ToDouble(lblInsuredAmt.Text) <= Convert.ToDouble(lblClaimAmount.Text))
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Claim Amount should be less than Insured Amount");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                            return;
                        }

                    }
                }
                //Checking for Claim Amount should be less than or equal to Insured Amount
                if (ddlMachineNoEdit != null)
                {
                    if (ddlMachineNoEdit.SelectedValue != "0")
                    {
                        if (ViewState["History"] != null)
                        {
                            DataSet dsHistroy = ViewState["History"] as DataSet;
                            DataRow[] drOpenedStatus = dsHistroy.Tables[0].Select("[Regn No or Serial No] = '" + ddlMachineNoEdit.SelectedItem.Text + "' and Status = 'Open'");
                            if (drOpenedStatus.Length > 0)
                            {
                                //DropDownList ddlStatusEdit = gvPolicyDetails.Rows[0].FindControl("ddlStatusEdit") as DropDownList;
                                //if (ddlStatusEdit.SelectedValue == "1") //For Closed status, user can put another claim.
                                //{
                                strAlert = strAlert.Replace("__ALERT__", "Reg No or Serial No already Exist in Opened Status");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                                return;
                                //}
                            }
                        }
                        //Checking for Accident Date Duplicate
                        TextBox txtAsseAlertDate = gvPolicyDetails.Rows[0].FindControl("txtAssetAlertDate") as TextBox;
                        Procparam = new Dictionary<string, string>();
                        Procparam.Add("@Option", "23");
                        Procparam.Add("@MachineNo", ddlMachineNoEdit.SelectedItem.Text);
                        Procparam.Add("@CompanyId", intCompanyId.ToString());
                        Procparam.Add("@AlertDate", Utility.StringToDate(txtAsseAlertDate.Text).ToString());
                        if (txtICNNO.Text.Trim() == "")
                        {
                            Procparam.Add("@ICNNo", "0");
                        }
                        else
                        {
                            Procparam.Add("@ICNNo", txtICNNO.Text.Trim());
                        }
                        DataSet dtClaimAmount = Utility.GetDataset("S3G_INS_LOADLOV", Procparam);
                        if (dtClaimAmount.Tables.Count > 0)
                        {
                            if (int.Parse(dtClaimAmount.Tables[0].Rows[0][0].ToString()) > 0)
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Alert Date should be greater than the Previous Claim Closed Date");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                                return;
                            }
                        }

                        Procparam = new Dictionary<string, string>();
                        Procparam.Add("@Option", "24");
                        Procparam.Add("@MachineNo", ddlMachineNoEdit.SelectedItem.Text);
                        Procparam.Add("@CompanyId", intCompanyId.ToString());
                        Procparam.Add("@AlertDate", Utility.StringToDate(txtAsseAlertDate.Text).ToString());
                        if (txtICNNO.Text.Trim() == "")
                        {
                            Procparam.Add("@ICNNo", "0");
                        }
                        else
                        {
                            Procparam.Add("@ICNNo", txtICNNO.Text.Trim());
                        }
                        dtClaimAmount = null;
                        dtClaimAmount = Utility.GetDataset("S3G_INS_LOADLOV", Procparam);
                        if (dtClaimAmount.Tables.Count > 0)
                        {
                            if (int.Parse(dtClaimAmount.Tables[0].Rows[0][0].ToString()) > 0)
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Alert Date alreay Exists");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                                return;
                            }
                        }
                        //Checking for Accident Date Duplicate
                    }
                }
                Label lblPolicyNo = gvPolicyDetails.Rows[0].FindControl("lblPolicyNumberEdit") as Label;
                TextBox txtAssetAlertDate = gvPolicyDetails.Rows[0].FindControl("txtAssetAlertDate") as TextBox;
                if (lblPolicyNo.Text != "")
                {

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Option", "19");
                    Procparam.Add("@PolicyNo", lblPolicyNo.Text);
                    Procparam.Add("@CompanyId", intCompanyId.ToString());
                    Procparam.Add("@AlertDate", Utility.StringToDate(txtAssetAlertDate.Text).ToString());
                    if (txtICNNO.Text.Trim() != "")
                    {
                        Procparam.Add("@ICNNo", txtICNNO.Text.Trim());
                    }
                    DataSet dtClaimAmount = Utility.GetDataset("S3G_INS_LOADLOV", Procparam);
                    if (dtClaimAmount.Tables.Count > 0)
                    {
                        double dcmClaimAmount = 0.0;
                        double dcmInsuranceAmount = 0.0;
                        if (dtClaimAmount.Tables[0].Rows.Count > 0)
                        {
                            dcmClaimAmount = Convert.ToDouble(dtClaimAmount.Tables[0].Rows[0][0]) + Convert.ToDouble(lblClaimAmount.Text);
                        }
                        if (dtClaimAmount.Tables[1].Rows.Count > 0)
                        {
                            dcmInsuranceAmount = Convert.ToDouble(dtClaimAmount.Tables[1].Rows[0][0]);
                        }
                        if (dcmClaimAmount >= dcmInsuranceAmount)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Claim Amount should be less than the Insured Amount for selected Policy Number");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                            return;
                        }
                    }

                }

                TextBox lblAssetClaimDate = gvPolicyDetails.Rows[0].FindControl("lblAssetClaimDate") as TextBox;

                Label lblPolicyExpiryDateEdit = gvPolicyDetails.Rows[0].FindControl("lblPolicyExpiryDateEdit") as Label;
                Label lblPolicySetupDateEdit = gvPolicyDetails.Rows[0].FindControl("lblPolicySetupDateEdit") as Label;
                if (Utility.StringToDate(lblAssetClaimDate.Text) < Utility.StringToDate(txtAssetAlertDate.Text))
                {
                    strAlert = strAlert.Replace("__ALERT__", "Claim Date should be greater or equal to Alert Date");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    return;
                }
                if (txtAssetAlertDate != null && lblPolicyExpiryDateEdit != null && lblPolicySetupDateEdit != null)
                {
                    if (!string.IsNullOrEmpty(lblPolicySetupDateEdit.Text) && !string.IsNullOrEmpty(txtAssetAlertDate.Text) && !string.IsNullOrEmpty(lblPolicyExpiryDateEdit.Text))
                    {
                        if ((Utility.StringToDate(lblPolicySetupDateEdit.Text) > Utility.StringToDate(txtAssetAlertDate.Text)) || (Utility.StringToDate(lblPolicyExpiryDateEdit.Text) < Utility.StringToDate(txtAssetAlertDate.Text)))
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Alert Date should be between Policy Setup Date and Policy Expiry Date");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                            return;
                        }

                    }
                }
            }
            int intResult = 0;

            S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimDataTable objAssetInsuranceClaimTable = new S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimDataTable();
            S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimRow objAssetInsuranceClaimRow = objAssetInsuranceClaimTable.NewS3G_INS_AssetInsuranceClaimRow();
            objAssetInsuranceClaimRow.Company_ID = intCompanyId;
            objAssetInsuranceClaimRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objAssetInsuranceClaimRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objAssetInsuranceClaimRow.Ins_Claim_ID = intICNId;
            objAssetInsuranceClaimRow.ICN_Date = Utility.StringToDate(txtICNDate.Text);
            objAssetInsuranceClaimRow.PANum = ddlMLA.SelectedItem.Text;
            objAssetInsuranceClaimRow.Is_Active = chkActive.Checked;
            objAssetInsuranceClaimRow.SANum = (ddlSLA.SelectedIndex == 0) ? ddlMLA.SelectedItem.Text + "DUMMY" : ddlSLA.SelectedItem.Text;
            if (intCustomer > 0)
                objAssetInsuranceClaimRow.Customer_ID = intCustomer;

            else
                objAssetInsuranceClaimRow.Customer_ID = Convert.ToInt32(hdnCustomerID.Value);
            objAssetInsuranceClaimRow.UserId = intUserID.ToString();
            objAssetInsuranceClaimRow.ICN_No = txtICNNO.Text;
            objAssetInsuranceClaimRow.XmlPolicyClaimDetails = gvPolicyDetails.FunPubFormXml(true);
            objAssetInsuranceClaimTable.AddS3G_INS_AssetInsuranceClaimRow(objAssetInsuranceClaimRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] objByteAssetInsuranceClaimTable = ClsPubSerialize.Serialize(objAssetInsuranceClaimTable, SerMode);
            string strICNNO = "";
            if (intICNId == 0)
            {
                intResult = objInsuranceClient.FunPubCreateAssetInsuranceClaim(out strICNNO, SerMode, objByteAssetInsuranceClaimTable);
            }
            else
            {
                intResult = objInsuranceClient.FunPubUpdateAssetInsuranceClaim(SerMode, objByteAssetInsuranceClaimTable);
            }
            if (intResult == 0)
            {
                //Added by Bhuvana T on 19/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                if (intICNId == 0)
                {
                    strAlert = "ICN NO " + strICNNO + " created successfully";
                    strAlert += @"\n\nWould you like to create one more Claim?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
                else
                {
                    string strModAlert = " Insurance Asset Claim Details " + strICNNO + " updated successfully";
                    strAlert = strAlert.Replace("__ALERT__", strModAlert);
                }

            }
            else if (intResult == -1)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                strRedirectPageView = "";
            }
            else if (intResult == -2)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                strRedirectPageView = "";
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Error in creating AINC");
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            cvAssetInsuranceClaim.ErrorMessage = "Unable to insert/ modify the claim details";
            cvAssetInsuranceClaim.IsValid = false;
        }
        finally
        {
            objInsuranceClient.Close();
        }
    }
   
    #endregion

    protected void btnAddPolicyClaim_OnClick(object sender, EventArgs e)
    {
        try
        {
            dtClaimDetails = (DataTable)ViewState["dtClaimDetails"];

            TextBox txtAssetClaimNumber = gvPolicyDetails.FooterRow.FindControl("txtAssetClaimNumber") as TextBox;
            TextBox txtAssetClaimDate = gvPolicyDetails.FooterRow.FindControl("txtAssetClaimDate") as TextBox;
            DropDownList ddlMachineNo = gvPolicyDetails.FooterRow.FindControl("ddlMachineNo") as DropDownList;
            Label lblAssetDescription = gvPolicyDetails.FooterRow.FindControl("lblAssetDescription") as Label;
            Label lblInsuredBy = gvPolicyDetails.FooterRow.FindControl("lblInsuredBy") as Label;
            Label lblPolicyNumber = gvPolicyDetails.FooterRow.FindControl("lblPolicyNumber") as Label;
            Label lblPolicySetupDate = gvPolicyDetails.FooterRow.FindControl("lblPolicySetupDate") as Label;
            Label lblPolicyExpiryDate = gvPolicyDetails.FooterRow.FindControl("lblPolicyExpiryDate") as Label;
            Label lblInsuredAmount = gvPolicyDetails.FooterRow.FindControl("lblInsuredAmount") as Label;
            TextBox txtClaimAmount = gvPolicyDetails.FooterRow.FindControl("txtClaimAmount") as TextBox;
            DropDownList ddlStatus = gvPolicyDetails.FooterRow.FindControl("ddlStatus") as DropDownList;
            TextBox txtRemarks = gvPolicyDetails.FooterRow.FindControl("txtRemarks") as TextBox;


            DataRow drPolicy = dtClaimDetails.NewRow();
            drPolicy["AssetClaimNo"] = txtAssetClaimNumber.Text;
            drPolicy["ClaimDate"] = txtAssetClaimDate.Text;
            drPolicy["MachineNo"] = ddlMachineNo.SelectedItem.Text;
            drPolicy["AssetDescription"] = lblAssetDescription.Text;
            drPolicy["PolicyNo"] = lblPolicyNumber.Text;
            drPolicy["PolicySetupDate"] = lblPolicySetupDate.Text;
            drPolicy["PolicyExpiryDate"] = lblPolicyExpiryDate.Text;
            drPolicy["InsuredAmount"] = lblInsuredAmount.Text;
            drPolicy["ClaimAmount"] = txtClaimAmount.Text;
            drPolicy["StatusId"] = ddlStatus.SelectedValue;
            drPolicy["Status"] = ddlStatus.SelectedItem.Text;
            drPolicy["Remarks"] = txtRemarks.Text;
            dtClaimDetails.Rows.Add(drPolicy);

            gvPolicyDetails.DataSource = dtClaimDetails;
            gvPolicyDetails.DataBind();

            ViewState["dtClaimDetails"] = dtClaimDetails;
            FunPriGenerateNewPolicyRow();


        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void FunPriGenerateNewPolicyRow()
    {
        try
        {
            FunPriLoadMachineNo();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #region [CustomerAddress]

    public static string SetCustomerAddress(string Address1, string Address2, string City, string State, string Country, string Pincode)
    {
        try
        {
            string strAddress = "";
            if (Address1.ToString() != "") strAddress += Address1.ToString() + System.Environment.NewLine;
            if (Address2.ToString() != "") strAddress += Address2.ToString() + System.Environment.NewLine;
            if (City.ToString() != "") strAddress += City.ToString() + System.Environment.NewLine;
            if (State.ToString() != "") strAddress += State.ToString() + System.Environment.NewLine;
            if (Country.ToString() != "") strAddress += Country.ToString() + System.Environment.NewLine;
            if (Pincode.ToString() != "") strAddress += Pincode.ToString();
            return strAddress;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region [CustomerDetails]
    private void FunPubQueryExistCustomerList(int CustomerID)
    {
        string strCustomerAddress = "";

        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Customer_Id", CustomerID.ToString());
            DataTable dtCustomerDetails = Utility.GetDefaultData("S3G_CLN_GetCustomerDetails", Procparam);
            if (dtCustomerDetails.Rows.Count > 0)
            {
                if (intICNId > 0)
                {
                    TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    txtName.Text = dtCustomerDetails.Rows[0]["Title"].ToString() + ' ' + dtCustomerDetails.Rows[0]["Customer_Name"].ToString();
                    txtName.Enabled = false;
                    txtName.ToolTip = txtName.Text;
                    Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
                    btnGetLOV.Visible = false;
                }
                strCustomerAddress = SetCustomerAddress(dtCustomerDetails.Rows[0]["comm_address1"].ToString(), dtCustomerDetails.Rows[0]["comm_address2"].ToString(), dtCustomerDetails.Rows[0]["comm_city"].ToString(), dtCustomerDetails.Rows[0]["comm_state"].ToString(), dtCustomerDetails.Rows[0]["comm_country"].ToString(), dtCustomerDetails.Rows[0]["comm_pincode"].ToString());
                S3GCustomerAddress1.SetCustomerDetails(dtCustomerDetails.Rows[0]["Customer_Code"].ToString(),
                    strCustomerAddress, dtCustomerDetails.Rows[0]["Customer_Name"].ToString(),
                    dtCustomerDetails.Rows[0]["Comm_Telephone"].ToString(),
                    dtCustomerDetails.Rows[0]["comm_mobile"].ToString(),
                    dtCustomerDetails.Rows[0]["comm_email"].ToString(),
                    dtCustomerDetails.Rows[0]["Comm_Website"].ToString());

            }
        }
        catch (Exception ex)
        {
            //    ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }
    #endregion

    #region [ClearCustomerDetails]

    private void FunPriClearCusdetails()
    {
        try
        {
            S3GCustomerAddress1.ClearCustomerDetails();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region [DropdownlistEvents]

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlMLA.Items.Clear();
        ddlSLA.Items.Clear();
        FunProClearControls(true);
        FunPriLoadDependentLOV();
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadDependentLOV();
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        Procparam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Program_ID", Convert.ToString(141));
        Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        if (intICNId == 0)
            Procparam.Add("@Is_Active", "1");
        //ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
        Procparam = null;
        ddlBranch.SelectedValue = "0";
        ddlMLA.Items.Clear();
        ddlSLA.Items.Clear();

        FunProClearControls(true);

    }

    protected void FunProClearControls(bool ClearCustomer)
    {
        if (ClearCustomer)
        {
            hdnCustomerID.Value = "";
            S3GCustomerAddress1.ClearCustomerDetails();
            ucCustomerCodeLov.FunPubClearControlValue();
        }        
        FunPriBindPolicyDetailsDLL("Add");

        gvHistory.DataSource = null;
        gvHistory.DataBind();
    }
    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlMLA.SelectedValue != "0")
        {
            FunPriLoadSLA();
            FunPriLoadLobBranch();

            if (ddlSLA.Items.Count > 1)
            {
                lblSLA.CssClass = "styleReqFieldLabel";
                rfvSubAccount.Enabled = true;

            }
            else
            {
                lblSLA.CssClass = "styleDisplayLabel";
                rfvSubAccount.Enabled = false;
            }

            FunPriLoadMachineNo();
            FunPriLoadCustomerDetails();

            if (ddlSLA.Items.Count == 1)
            {
              LoadHistoryDetailsInCreate();
            }
        
        }
        else
        {
            ddlSLA.Items.Clear();
            FunProClearControls(true);
        }

        
    }
    private void FunPriLoadLobBranch()
    {
        {
            Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "17");

            Procparam.Add("@companyId", intCompanyId.ToString());

            if (ddlMLA.SelectedValue != "0")
            {
                Procparam.Add("@Panum", ddlMLA.SelectedItem.Text);
            }

            DataTable dtMachineDetails = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);
            ddlLOB.SelectedValue = dtMachineDetails.Rows[0]["Lob_Id"].ToString();
            ddlBranch.SelectedValue = dtMachineDetails.Rows[0]["Location_ID"].ToString();

        }
    }
    private void FunPriLoadCustomerDetails()
    {
        Procparam = new Dictionary<string, string>();

        Procparam.Add("@Option", "9");
        if (ddlMLA.SelectedValue != "0")
        {
            Procparam.Add("@Panum", ddlMLA.SelectedItem.Text);
        }

        DataTable dtMachineDetails = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);

        if (dtMachineDetails.Rows.Count > 0)
        {
            hdnCustomerID.Value = dtMachineDetails.Rows[0]["Customer_ID"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(dtMachineDetails.Rows[0]["Customer_Code"].ToString(),
                       dtMachineDetails.Rows[0]["comm_Address1"].ToString() + "\n" +
                dtMachineDetails.Rows[0]["comm_Address2"].ToString() + "\n" +
               dtMachineDetails.Rows[0]["comm_city"].ToString() + "\n" +
               dtMachineDetails.Rows[0]["comm_state"].ToString() + "\n" +
               dtMachineDetails.Rows[0]["comm_country"].ToString() + "\n" +
               dtMachineDetails.Rows[0]["comm_pincode"].ToString(), dtMachineDetails.Rows[0]["Customer_Name"].ToString(), dtMachineDetails.Rows[0]["Comm_mobile"].ToString(),
                dtMachineDetails.Rows[0]["Comm_telephone"].ToString(),
                dtMachineDetails.Rows[0]["comm_email"].ToString(), dtMachineDetails.Rows[0]["comm_website"].ToString());
            TextBox txtName = ucCustomerCodeLov.FindControl("txtName") as TextBox;
            txtName.Text = dtMachineDetails.Rows[0]["Customer_Name"].ToString();
        }
        else
        {
            S3GCustomerAddress1.ClearCustomerDetails();
            
            hdnCustomerID.Value = "";
        }
    }



    protected void gvPolicyDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtClaimDetails = (DataTable)ViewState["dtClaimDetails"];
            if (dtClaimDetails.Rows.Count > 0)
            {
                dtClaimDetails.Rows.RemoveAt(e.RowIndex);

                if (dtClaimDetails.Rows.Count == 0)
                {
                    FunPriBindPolicyDetailsDLL("Add");
                }
                else
                {
                    gvPolicyDetails.DataSource = dtClaimDetails;
                    gvPolicyDetails.DataBind();
                    FunPriGenerateNewPolicyRow();
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvAssetInsuranceClaim.ErrorMessage = "Due to Data Problem,Unable to Remove Claim";
            cvAssetInsuranceClaim.IsValid = false;

        }
    }

    protected void gvPolicyDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{

                
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtClaimAmt = (TextBox)e.Row.FindControl("lblClaimAmount");
                txtClaimAmt.SetDecimalPrefixSuffix(10, 4, true,false, "Claim Amount");

                AjaxControlToolkit.CalendarExtender calAssetClaimDateEdit = e.Row.FindControl("calAssetClaimDateEdit") as AjaxControlToolkit.CalendarExtender;
                calAssetClaimDateEdit.Format = strDateFormat;
                

                AjaxControlToolkit.CalendarExtender calAssetAlertDateEdit = e.Row.FindControl("calAssetAlertDateEdit") as AjaxControlToolkit.CalendarExtender;
                calAssetAlertDateEdit.Format = strDateFormat;
                

                DropDownList ddlMachineNo = e.Row.FindControl("ddlMachineNoEdit") as DropDownList;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@OPTION", "7");
                if (intICNId == 0)
                {
                    if (ddlMLA.SelectedIndex != -1 && ddlMLA.SelectedIndex != 0)
                    {
                        Procparam.Add("@Panum", ddlMLA.SelectedItem.Text);
                        Procparam.Add("@Sanum", (ddlSLA.SelectedIndex == 0) ? ddlMLA.SelectedItem.Text + "DUMMY" : ddlSLA.SelectedItem.Text);
                    }
                }
                else
                {
                    if (ddlMLA.SelectedIndex != -1 && ddlMLA.SelectedIndex != 0)
                    {
                        Procparam.Add("@Panum", ddlMLA.SelectedItem.Text);
                        Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedItem.Text + "DUMMY" : ddlSLA.SelectedItem.Text);
                    }
                }
                ddlMachineNo.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Machine_No", "Machine_No" });
                DropDownList ddlStatus = e.Row.FindControl("ddlStatusEdit") as DropDownList;
                Procparam.Clear();
                Procparam.Add("@OPTION", "12");
                ddlStatus.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
                TextBox txtAssetClaimDate = e.Row.FindControl("lblAssetClaimDate") as TextBox;
                //txtAssetClaimDate.Attributes.Add("readonly", "true");
                TextBox txtAlertDate = (TextBox)e.Row.FindControl("txtAssetAlertDate");
                //txtAssetClaimDate.Attributes.Add("readonly", "true");
                if (txtAssetClaimDate != null)
                {
                    txtAssetClaimDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtAssetClaimDate.ClientID + "','" + strDateFormat + "',true,  false);");
                }
                if (PageMode == PageModes.Query)
                {
                    txtAssetClaimDate.Attributes.Add("readonly", "true");
                    txtAssetClaimDate.Attributes.Remove("onblur");
                }

                //TextBox txtAlertDate = (TextBox)e.Row.FindControl("txtAssetAlertDate");
                //txtAlertDate.Attributes.Add("readonly", "true");
                if (txtAlertDate != null)
                {
                    txtAlertDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtAlertDate.ClientID + "','" + strDateFormat + "',true,  false);");
                }
                if (PageMode == PageModes.Query)
                {
                    txtAlertDate.Attributes.Add("readonly", "true");
                    txtAlertDate.Attributes.Remove("onblur");
                }
                
                DataTable dtPolicy = (DataTable)ViewState["dtClaimDetails"];
                if (dtPolicy != null && dtPolicy.Rows.Count > 0)
                {
                    ddlMachineNo.SelectedValue = dtPolicy.Rows[e.Row.RowIndex]["MachineNo"].ToString();
                    ddlStatus.SelectedValue = dtPolicy.Rows[e.Row.RowIndex]["StatusId"].ToString();
                    Label lblInsuredBy = e.Row.FindControl("lblInsuredByEdit") as Label;
                    Label lblPolicyNumber = e.Row.FindControl("lblPolicyNumberEdit") as Label;
                    Label lblPolicySetupDate = e.Row.FindControl("lblPolicySetupDateEdit") as Label;
                    Label lblPolicyExpiryDate = e.Row.FindControl("lblPolicyExpiryDateEdit") as Label;
                    Label lblInsuredAmount = e.Row.FindControl("lblInsuredAmountEdit") as Label;
                    Label lblInsurerName = e.Row.FindControl("lblInsurerNameEdit") as Label;
                    TextBox txtAssetAlertDate = e.Row.FindControl("txtAssetAlertDate") as TextBox;
                    TextBox lblAssetClaimDate = e.Row.FindControl("lblAssetClaimDate") as TextBox;
                    txtAssetAlertDate.Attributes.Add("readonly","true");
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@OPTION", "8");
                    Procparam.Add("@CompanyId", intCompanyId.ToString());
                    Procparam.Add("@MachineNo", ddlMachineNo.SelectedItem.Text);
                    Procparam.Add("@AlertDate", Utility.StringToDate(txtAssetAlertDate.Text).ToString());
                    DataSet dsMachineDetails = Utility.GetDataset("S3G_INS_LOADLOV", Procparam);

                    if (dsMachineDetails != null)
                    {
                        if (dsMachineDetails.Tables[0] !=null && dsMachineDetails.Tables[0].Rows.Count > 0)
                        {
                            lblPolicyNumber.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_No"].ToString();
                            lblPolicySetupDate.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_From_Date"].ToString();
                            lblPolicyExpiryDate.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_To_Date"].ToString();
                            lblInsuredAmount.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_Value"].ToString();
                        }
                        if (dsMachineDetails.Tables[1]!=null && dsMachineDetails.Tables[1].Rows.Count > 0)
                        {
                            lblInsuredBy.Text = dsMachineDetails.Tables[1].Rows[0]["InsuredBy"].ToString();
                            lblInsurerName.Text = dsMachineDetails.Tables[1].Rows[0]["entity_name"].ToString();
                        }
                        if (strMode == "Q" || ddlStatus.SelectedValue == "2")
                        {
                            TextBox txtAssetClaimNumber = e.Row.FindControl("lblAssetClaimNumber") as TextBox;
                            AjaxControlToolkit.CalendarExtender calAssetClaimDate = e.Row.FindControl("calAssetClaimDateEdit") as AjaxControlToolkit.CalendarExtender;
                            TextBox txtClaimAmount = e.Row.FindControl("lblClaimAmount") as TextBox;
                            
                            TextBox txtRemarks = e.Row.FindControl("lblRemarks") as TextBox;
                            ddlMachineNo.ClearDropDownList();
                            ddlStatus.ClearDropDownList();
                            lblAssetClaimDate.ReadOnly = txtAssetAlertDate.ReadOnly = txtAssetClaimNumber.ReadOnly = txtClaimAmount.ReadOnly = txtRemarks.ReadOnly = true;
                            calAssetAlertDateEdit.Enabled = false;
                            calAssetClaimDateEdit.Enabled = false;

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            cvAssetInsuranceClaim.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvAssetInsuranceClaim.IsValid = false;
        }
    }
    protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    TextBox txtRemarks = new TextBox();
        //    txtRemarks.ID = "txtRemarks";
        //    txtRemarks.TextMode = TextBoxMode.MultiLine;
        //    if (e.Row.Cells.Count == 15)
        //    {

        //        txtRemarks.Text = e.Row.Cells[14].Text;

        //        e.Row.Cells[14].Controls.Add(txtRemarks);

        //    }
        //    else if (e.Row.Cells.Count > 15)
        //    {
        //        txtRemarks.Text = e.Row.Cells[15].Text;
        //        e.Row.Cells[15].Controls.Add(txtRemarks);
        //    }
        //    txtRemarks.Attributes.Add("onchange", "wraptext(" + txtRemarks.ClientID + ",60)");
        //    txtRemarks.ReadOnly = true;
        //}

    }

    private void FunPriLoadMachineNo()
    {
        DropDownList ddlMachineNo = gvPolicyDetails.Rows[0].FindControl("ddlMachineNoEdit") as DropDownList;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@OPTION", "7");
        if (ddlMLA.SelectedIndex != -1)
        {
            Procparam.Add("@Panum", ddlMLA.SelectedItem.Text);
            Procparam.Add("@Sanum", (ddlSLA.SelectedIndex == 0) ? ddlMLA.SelectedItem.Text + "DUMMY" : ddlSLA.SelectedItem.Text);
        }
        ddlMachineNo.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Machine_No", "Machine_No" });

    }

    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSLA.SelectedIndex > 0)
        {
            FunPriLoadMachineNo();
            LoadHistoryDetailsInCreate();
        }
        //else
        //{
        //    FunProClearControls(false);
        //}

    }

    protected void ddlMachineNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlMachineNo = (DropDownList)sender;
        GridViewRow gvRow = (GridViewRow)ddlMachineNo.Parent.Parent;
        Label lblAssetDescription = gvRow.FindControl("lblAssetDescriptionEdit") as Label;
        Label lblInsuredBy = gvRow.FindControl("lblInsuredByEdit") as Label;
        Label lblInsurerName = gvRow.FindControl("lblInsurerNameEdit") as Label;
        Label lblPolicyNumber = gvRow.FindControl("lblPolicyNumberEdit") as Label;
        Label lblPolicySetupDate = gvRow.FindControl("lblPolicySetupDateEdit") as Label;
        Label lblPolicyExpiryDate = gvRow.FindControl("lblPolicyExpiryDateEdit") as Label;
        Label lblInsuredAmount = gvRow.FindControl("lblInsuredAmountEdit") as Label;
        TextBox txtAssetAlertDate = gvRow.FindControl("txtAssetAlertDate") as TextBox;

        if (txtAssetAlertDate.Text == "")
        {
            ClearClaimGridDetails();
            strAlert = strAlert.Replace("__ALERT__", "Select the Alert Date");
            strRedirectPageView = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            return;
        }

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@OPTION", "8");
        Procparam.Add("@CompanyId", intCompanyId.ToString());
        Procparam.Add("@MachineNo", ddlMachineNo.SelectedItem.Text);
        Procparam.Add("@AlertDate", Utility.StringToDate(txtAssetAlertDate.Text).ToString());
        DataSet dsMachineDetails = Utility.GetDataset("S3G_INS_LOADLOV", Procparam);
        if (dsMachineDetails != null)
        {
            if (dsMachineDetails.Tables[0].Rows.Count > 0)
            {
                lblAssetDescription.Text = dsMachineDetails.Tables[1].Rows[0]["AssetDescription"].ToString();
                lblPolicyNumber.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_No"].ToString();
                lblPolicySetupDate.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_From_Date"].ToString();
                lblPolicyExpiryDate.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_To_Date"].ToString();
                lblInsuredAmount.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_Value"].ToString();
                lblInsuredBy.Text = dsMachineDetails.Tables[1].Rows[0]["InsuredBy"].ToString();
                //lblInsurerName.Text = dsMachineDetails.Tables[1].Rows[0]["Ins_Company_Name"].ToString();
                lblInsurerName.Text = dsMachineDetails.Tables[1].Rows[0]["entity_name"].ToString();
                
            }
            else
            {
                ClearClaimGridDetails();
                strAlert = strAlert.Replace("__ALERT__", "No Policy Details Available");
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            

        }
    }

    private void FunPriLoadMachineDetails()
    {

    }
    private void FunPriLoadSLA()
    {
        if (ddlMLA.SelectedIndex != -1)
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@OPTION", "21");
            Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
            Procparam.Add("@PANUM", Convert.ToString(ddlMLA.SelectedItem.Text));
            ddlSLA.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "SANUM", "SANUM" });
        }

    }

    
    private void LoadHistoryDetailsInCreate()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        Procparam.Add("@LOCATION_ID", ddlBranch.SelectedValue);
        Procparam.Add("@PANUM", ddlMLA.SelectedValue);
        if (ddlSLA.SelectedValue != "0")
        {
            Procparam.Add("@SANUM", ddlSLA.SelectedValue);
        }
        DataSet dsAINSE = Utility.GetDataset("S3G_INS_GetInsuranceClaimHistoryDetails", Procparam);
        ViewState["History"] = dsAINSE;
        if (dsAINSE.Tables[0].Rows.Count > 0)
        {
            gvHistory.DataSource = dsAINSE.Tables[0];
        }
        else
        {
            gvHistory.DataSource = null;
        }
        gvHistory.DataBind();
        GvHistorytxtRemarks();

    }
    private void ClearClaimGridDetails()
    {
        Label lblAssetDescriptionEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblAssetDescriptionEdit");
        Label lblInsuredByEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblInsuredByEdit");
        Label lblPolicyNumberEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblPolicyNumberEdit");
        Label lblInsuredAmountEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblInsuredAmountEdit");
        TextBox lblClaimAmount = (TextBox)gvPolicyDetails.Rows[0].FindControl("lblClaimAmount");
        TextBox lblRemarks = (TextBox)gvPolicyDetails.Rows[0].FindControl("lblRemarks");
        DropDownList ddlStatusEdit = (DropDownList)gvPolicyDetails.Rows[0].FindControl("ddlStatusEdit");
        DropDownList ddlMachineNoEdit = (DropDownList)gvPolicyDetails.Rows[0].FindControl("ddlMachineNoEdit");
        Label lblPolicySetupDateEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblPolicySetupDateEdit");
        Label lblPolicyExpiryDateEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblPolicyExpiryDateEdit");
        Label lblInsurerNameEdit = (Label)gvPolicyDetails.Rows[0].FindControl("lblInsurerNameEdit");

        lblAssetDescriptionEdit.Text = "";
        lblInsuredByEdit.Text = "";
        lblPolicyNumberEdit.Text = "";
        lblInsuredAmountEdit.Text = "";
        lblClaimAmount.Text = "";
        lblRemarks.Text = "";
        lblPolicySetupDateEdit.Text = "";
        lblPolicyExpiryDateEdit.Text = "";
        ddlStatusEdit.SelectedValue = "0";
        ddlMachineNoEdit.SelectedValue = "0";
        lblInsurerNameEdit.Text = "";

    }

    protected void txtAssetAlertDate_TextChanged(object sender, EventArgs e)
    {
        TextBox txtAssetAlertDate = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txtAssetAlertDate.Parent.Parent;
        Label lblAssetDescription = gvRow.FindControl("lblAssetDescriptionEdit") as Label;
        Label lblInsuredBy = gvRow.FindControl("lblInsuredByEdit") as Label;
        Label lblInsurerName = gvRow.FindControl("lblInsurerNameEdit") as Label;
        Label lblPolicyNumber = gvRow.FindControl("lblPolicyNumberEdit") as Label;
        Label lblPolicySetupDate = gvRow.FindControl("lblPolicySetupDateEdit") as Label;
        Label lblPolicyExpiryDate = gvRow.FindControl("lblPolicyExpiryDateEdit") as Label;
        Label lblInsuredAmount = gvRow.FindControl("lblInsuredAmountEdit") as Label;
        DropDownList ddlMachineNo = gvRow.FindControl("ddlMachineNoEdit") as DropDownList;
        if (ddlMachineNo.SelectedIndex == 0)
        {
            return;
        }

        if (txtAssetAlertDate.Text == "")
        {
            ClearClaimGridDetails();
            strAlert = strAlert.Replace("__ALERT__", "Select the Alert Date");
            strRedirectPageView = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            return;
        }

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@OPTION", "8");
        Procparam.Add("@CompanyId", intCompanyId.ToString());
        Procparam.Add("@MachineNo", ddlMachineNo.SelectedItem.Text);
        Procparam.Add("@AlertDate", Utility.StringToDate(txtAssetAlertDate.Text).ToString());
        DataSet dsMachineDetails = Utility.GetDataset("S3G_INS_LOADLOV", Procparam);
        if (dsMachineDetails != null)
        {
            if (dsMachineDetails.Tables[0].Rows.Count > 0)
            {
                lblAssetDescription.Text = dsMachineDetails.Tables[1].Rows[0]["AssetDescription"].ToString();
                lblPolicyNumber.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_No"].ToString();
                lblPolicySetupDate.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_From_Date"].ToString();
                lblPolicyExpiryDate.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_To_Date"].ToString();
                lblInsuredAmount.Text = dsMachineDetails.Tables[0].Rows[0]["Policy_Value"].ToString();
                lblInsuredBy.Text = dsMachineDetails.Tables[1].Rows[0]["InsuredBy"].ToString();
                //lblInsurerName.Text = dsMachineDetails.Tables[1].Rows[0]["Ins_Company_Name"].ToString();
                lblInsurerName.Text = dsMachineDetails.Tables[1].Rows[0]["entity_name"].ToString();

            }
            else
            {
                ClearClaimGridDetails();
                strAlert = strAlert.Replace("__ALERT__", "No Policy Details Available");
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }


        }
    }

    private void GvHistorytxtRemarks()
    {
        if (gvHistory.Rows.Count > 0)
        {
            foreach (GridViewRow item in gvHistory.Rows)
            {
                item.Cells[0].Visible = false;
                if (item.RowType == DataControlRowType.DataRow)
                {

                    if (item.RowType == DataControlRowType.DataRow)
                    {
                        TextBox txtRemarks = new TextBox();
                        txtRemarks.ID = "txtRemarks";
                        txtRemarks.TextMode = TextBoxMode.MultiLine;
                        if (item.Cells.Count == 16)
                        {

                            txtRemarks.Text = item.Cells[15].Text.Replace("&nbsp;", "");

                            item.Cells[15].Controls.Add(txtRemarks);

                        }
                        else if (item.Cells.Count > 16)
                        {
                            txtRemarks.Text = item.Cells[16].Text.Replace("&nbsp;", "");
                            item.Cells[16].Controls.Add(txtRemarks);
                        }
                        //txtRemarks.Attributes.Add("onchange", "wraptext(" + txtRemarks.ClientID + ",60)");
                        txtRemarks.ReadOnly = true;
                    }
                }
            }
        }

    }


    #endregion

}

