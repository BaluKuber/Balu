#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Legal & Repossession
/// Screen Name			: Repossession
/// Created By			: Manikandan R
/// Created Date		: 21-MAY-2011
/// Purpose	            : This module is used to repossess the asset
///<Program Summary>
#endregion
using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Collections;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;

public partial class LegalRepossession_S3GLegalRepossessionRepossession_Add : ApplyThemeForProject
{
    # region Intialization
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> Procparam = null;
    Dictionary<string, string> ProcparamGuarantor = null;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    StringBuilder strAssetBuilder = new StringBuilder();
    string strRepo = string.Empty;
    int intRepossession_ID = 0;
    int intAsset_ID = 0;
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLegalRepossessionRepossession_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GRP';";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    int IsAdd = 0;
    string strRedirectPage = "~/LegalRepossession/S3GLoanAdTransLander.aspx?Code=GRP";
    //string strNumberFormat = string.Empty;

    //User Authorization

    public static LegalRepossession_S3GLegalRepossessionRepossession_Add obj_Page;
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    int intErrorCode = 0;

    LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient objClient;
    S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionDataTable objLRDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;

    //Code end

    #endregion
    #region Page Load Events
    // Changed by Bhuvana.
    // I have changed the queryname and master page file. and in Cancel.
    protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {

            if (Request.QueryString["IsPopup"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to Initialise the Controls in Vendor Invoice");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end
        //Date Format
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        //strNumberFormat = Funsetsuffix();
        //End
        obj_Page = this;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        btnLedgerView.Enabled = false;
        if (intCompanyID == 0)
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
        if (intUserID == 0)
            intUserID = ObjUserInfo.ProUserIdRW;
        txtRepoDate.Text = DateTime.Now.ToString(strDateFormat);
        txtCurrentMarket.SetDecimalPrefixSuffix(10, 4, true, "Current Market Value");
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
            strRepo = fromTicket.Name;
            IsAdd = 1;

            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if (fromTicket != null)
            {
                intRepossession_ID = Convert.ToInt32(fromTicket.Name);
                strMode = Request.QueryString.Get("qsMode");
                tcRepossession.ActiveTabIndex = 0;
                //btnViewAccount.Enabled = false;
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }
        txtRepossessionDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtRepossessionDate.ClientID + "','" + strDateFormat + "',true,  false);");
        if (!IsPostBack)
        {
            tcRepossession.ActiveTabIndex = 0;
            //txtRepossessionDate.Attributes.Add("readonly", "readonly");
            CalendarExtenderRepossessionDate.Format = strDateFormat;
            FunToolTip();
            PopulateLOBList();
            if (Request.QueryString.Get("qsMode") != "C")
            {
                PopulateBranchList();
            }
            FunPriLoadDoneBy();
            if (strMode == "M")
                PageMode = PageModes.Modify;


            // ChkActive.Enabled = false;
            //ChkActive.Checked = true;
            FunPriDisableControls(0);
            if ((intRepossession_ID > 0) && (strMode == "M"))
            {
                PageMode = PageModes.Modify;
                PopulateGarageDetails();
                FunGetRepossionDetails(intRepossession_ID);
                PopulateLRNumber();
                FunPriDisableControls(1);
            }
            if ((intRepossession_ID > 0) && (strMode == "Q"))
            {
                PageMode = PageModes.Modify;
                PopulateGarageDetails();
                //PopulateAgentDetails();
                FunGetRepossionDetails(intRepossession_ID);
                PopulateLRNumber();
                FunPriDisableControls(2);
            }

            if (PageMode == PageModes.Create)
            {
                btnViewAccount.Enabled = false;
            }


        }
    }

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
                btnViewAccount.Enabled = false;
                // ChkActive.Checked = true;
                //ChkActive.Enabled = false;
                tcRepossession.ActiveTabIndex = 0;
                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                tcRepossession.ActiveTabIndex = 0;
                btnClear.Enabled = false;
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                // imgDateofActivation.Visible = false;
                ddlLOB.ClearDropDownList();
                ddlBranch.Enabled = false;
                //ddlBranch.ClearDropDownList();
                ddlLRNNumber.ClearDropDownList();
                btnViewAccount.Enabled = true;
                //if ((ViewState["Release"]==null))
                //{
                //    rdoNo.Enabled = false; 
                //}
                //if ((ViewState["Release"].ToString() != "0"))//modified by Ponnurajesh on 23/mar/2012 for Oracle conversion
                //if (Convert.ToString (ViewState ["Release"])!="0") 

                if (ViewState["Release"].ToString() != "0")
                {
                    ddlRepossessionDoneBy.ClearDropDownList();
                    ddlAgentCode.ClearDropDownList();
                    ddlGarageCode.ClearDropDownList();
                    rdoNo.Enabled = false;
                    rdoYes.Enabled = false;
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    txtAssetInventory.ReadOnly = true;
                    txtCondition.ReadOnly = true;
                    txtCurrentMarket.ReadOnly = true;
                    txtRemarks.ReadOnly = true;
                    txtPlace.ReadOnly = true;
                    txtRepossessionDate.ReadOnly = true;
                    txtRepoExp.ReadOnly = true;
                    // CalendarExtenderRepossessionDate.Enabled = false;
                }
                break;
            case 2:
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);

                }
                btnViewAccount.Enabled = true;
                imgDateofActivation.Visible = false;
                tcRepossession.ActiveTabIndex = 0;
                ddlLOB.ClearDropDownList();
                ddlBranch.Enabled = false;
                //ddlBranch.ClearDropDownList();
                ddlLRNNumber.ClearDropDownList();
                ddlRepossessionDoneBy.ClearDropDownList();
                ddlAgentCode.ClearDropDownList();
                ddlGarageCode.ClearDropDownList();
                txtAssetInventory.ReadOnly = true;
                txtCondition.ReadOnly = true;
                txtPlace.ReadOnly = true;
                rdoNo.Enabled = false;
                rdoYes.Enabled = false;
                txtCurrentMarket.ReadOnly = true;
                txtRemarks.ReadOnly = true;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                txtRepoExp.ReadOnly = true;
                //  ChkActive.Enabled = false;
                CalendarExtenderRepossessionDate.Enabled = false;
                txtRepossessionDate.ReadOnly = true;
                break;
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
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Program_Id", "141");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    #region To Get Tool Tips
    private void FunToolTip()
    {

        ddlLOB.ToolTip = lblLOB.Text;
        ddlBranch.ToolTip = lblBranch.Text;
        ddlLRNNumber.ToolTip = lblLRNNumber.Text;
        gvAssetDetails.ToolTip = "Asset Details";
        txtRepossessionDate.ToolTip = lblRepossessionDate.Text;
        txtRepoDate.ToolTip = lblRepoDate.Text;
        txtPlace.ToolTip = lblPlace.Text;
        txtPANum.ToolTip = lblPANum.Text;
        txtSANum.ToolTip = lblSANum.Text;
        rdoYes.ToolTip = "Select Yes";
        rdoNo.ToolTip = "Select No";
        txtCondition.ToolTip = lblCondition.Text;
        ddlRepossessionDoneBy.ToolTip = lblRepossessionDoneBy.Text;
        txtInsuranceDate.ToolTip = lblInsuranceDate.Text;
        txtCurrentMarket.ToolTip = lblCurrentMarketValue.Text;
        //txtDocsObtained.ToolTip = lblDocumentObtained.Text;
        txtTenure.ToolTip = lblTenure.Text;
        txtAccountDate.ToolTip = lblAccountDate.Text;
        txtAmtFinanced.ToolTip = lblAmtFinanced.Text;
        txtRemarks.ToolTip = lblRemarks.Text;
        txtRepoExp.ToolTip = lblRepoExp.Text;
        txtGarageIn.ToolTip = lblGarageIn.Text;
        ddlAgentCode.ToolTip = lblEntity.Text;
        ddlGarageCode.ToolTip = lblGarageCode.Text;
        txtAssetInventory.ToolTip = lblInventoryDetails.Text;
    }
    #endregion
    #endregion
    # region To Fill Drop Down List
    # region TO Get Line of Business
    private void PopulateLOBList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_Id", Convert.ToString(intUserID));
            Procparam.Add("@FilterOption", "'HP','LN','OL','FL'");
            Procparam.Add("@Program_ID", "154");
            if (PageMode == PageModes.Create)
                Procparam.Add("@Is_UserLobActive", "1");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.Items.RemoveAt(0);
                ddlLOB.SelectedIndex = 0;

                //PopulatePANum();
            }

        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Unable to Load Line of Business";
            cvRepossession.IsValid = false;
            return;
        }
    }
    #endregion
    #region TO Get Branch List
    private void PopulateBranchList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Program_ID", "154");
            if (ddlLOB.SelectedIndex > 0)
            {
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            }
            if (PageMode == PageModes.Create)
                Procparam.Add("@Is_Active", "1");

            ddlBranch.Clear();
            //ddlBranch.BindDataTable("S3G_Get_Branch_List", Procparam, new string[] { "Location_ID", "Location" });
        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Unable to Load Location";
            cvRepossession.IsValid = false;
            return;
        }



    }
    private void FunPriClear()
    {
        try
        {
            //ddlLRNNumber.ClearDropDownList();
            btnViewAccount.Enabled = false;
            txtPANum.Text = "";
            txtSANum.Text = "";
            txtTenure.Text = "";
            txtAccountDate.Text = "";
            txtAmtFinanced.Text = "";
            gvAssetDetails.DataSource = null;
            gvAssetDetails.DataBind();
            S3GCustomerAddress1.ClearCustomerDetails();
            ddlGarageCode.Items.Clear();
            //ddlRepossessionDoneBy.Items.Clear();
            ddlAgentCode.Items.Clear();
            S3GGarageAddress.ClearCustomerDetails();
            gvGuarDetails.DataSource = null;
            gvGuarDetails.DataBind();
            txtInsuranceDate.Text = "";
            txtPlace.Text = "";
            //txtRepoDate.Text = "";
            txtRepossessionDate.Text = "";
            //txtDocsObtained.Text = "";
            //ddlGarageCode.ClearDropDownList();
            //ddlAgentCode.ClearDropDownList();
            ddlRepossessionDoneBy.SelectedValue = "0";
            txtRepoExp.Text = "";
            txtCondition.Text = "";
            txtCurrentMarket.Text = "";
            txtGarageIn.Text = "";
            txtRemarks.Text = "";
            txtAssetInventory.Text = "";
            gvGuarDetails.DataSource = null;
            gvGuarDetails.DataBind();
            lblGuarDetails.Visible = false;
            tcRepossession.ActiveTabIndex = 0;

        }

        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Unable to Clear Details";
            cvRepossession.IsValid = false;
            return;
        }


    }

    private void FunClearRepossessionDetails()
    {
        ddlRepossessionDoneBy.SelectedValue = "0";
        txtRepoExp.Text = "";
        txtCondition.Text = "";
        txtCurrentMarket.Text = "";
        txtGarageIn.Text = "";
        txtRemarks.Text = "";
        txtAssetInventory.Text = "";
        txtPlace.Text = "";
        gvGuarDetails.DataSource = null;
        gvGuarDetails.DataBind();
        ddlGarageCode.SelectedIndex = -1;
        ddlAgentCode.Items.Clear();
        S3GGarageAddress.ClearCustomerDetails();
        txtRepossessionDate.Text = "";

    }
    private void FunPriGetInsurancePolicy(int AssetNumber)
    {

        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@PANum", Convert.ToString(txtPANum.Text));
            Procparam.Add("@SANum", Convert.ToString(txtSANum.Text));
            Procparam.Add("@Asset_Number", Convert.ToString(intAsset_ID));

            DataTable dtInsuraceDetails = Utility.GetDefaultData("S3G_LR_GetInsuranceDetails", Procparam);
            if ((dtInsuraceDetails.Rows.Count) > 0)
            {
                DataRow DrInsurance = dtInsuraceDetails.Rows[0];
                txtInsuranceDate.Text = DateTime.Parse(DrInsurance["Policy_To_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                hdnInsurance_ID.Text = dtInsuraceDetails.Rows[0]["Account_Ins_ID"].ToString();
            }

        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = ex.Message;
            cvRepossession.IsValid = false;
            return;
        }
    }

    private void PopulateLRNumber()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            if ((PageMode == PageModes.Modify) || (PageMode == PageModes.Query))
            {
                Procparam.Add("@IS_Modify", Convert.ToString("1"));
            }

            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
            {
                Procparam.Add("@IS_OL", Convert.ToString("1"));
            }
            else
            {
                Procparam.Add("@IS_OL", Convert.ToString("0"));
            }
            ddlLRNNumber.BindDataTable("S3G_LR_GetLRNumber", Procparam, new string[] { "LRN_ID", "LRN_NO" });
        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Unable to Load LRN Number";
            cvRepossession.IsValid = false;
            return;
        }
    }
    private void PopulateAgentDetails()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Agent", Convert.ToString(ddlRepossessionDoneBy.SelectedItem.Text));
            if (PageMode != PageModes.Create)
            {
                Procparam.Add("@Mode", Convert.ToString(1));
                Procparam.Add("@Repossession_ID", Convert.ToString(intRepossession_ID));
            }

            ddlAgentCode.BindDataTable("S3G_LR_GetAgentDetails", Procparam, new string[] { "Agent_ID", "Agent_NO" });
        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Unable to Load Agent/Employee Details";
            cvRepossession.IsValid = false;
            return;
        }
    }

    private void FunGetGuarageDetails(int Guarage_ID)
    {
        try
        {
            if (Convert.ToInt32(ddlGarageCode.SelectedValue) > 0)
            {
                string strGarageAddress = "";
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictParam.Add("@Garage_ID", Convert.ToString(ddlGarageCode.SelectedValue));
                DataTable dtGarage = Utility.GetDefaultData("S3G_LR_GetGarageDetails", dictParam);
                strGarageAddress = Utility.SetVendorAddress(dtGarage.Rows[0]);
                S3GGarageAddress.SetCustomerDetails("", strGarageAddress, "",
                 dtGarage.Rows[0]["PhoneNumber"].ToString(),
                dtGarage.Rows[0]["Mobile"].ToString(),
                dtGarage.Rows[0]["EMail"].ToString(),
                dtGarage.Rows[0]["Website"].ToString());

                if (PageMode == PageModes.Create)
                    txtGarageIn.Text = DateTime.Now.ToString(strDateFormat); //dtGarage.Rows[0]["Garage_Code"].ToString();
            }
        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Unable to get Garage Details";
            cvRepossession.IsValid = false;
            return;
        }

    }


    private void PopulateGarageDetails()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (PageMode != PageModes.Create)
            {
                Procparam.Add("@Repossession_ID", Convert.ToString(intRepossession_ID));
            }
            ddlGarageCode.BindDataTable("S3G_LR_GetGarageDetails", Procparam, new string[] { "Garage_ID", "Garage_Code" });
        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Unable to load Garage Details";
            cvRepossession.IsValid = false;
            return;
        }
    }

    protected void FunProFillgrid(DataTable dtAssetDetails)
    {
        try
        {
            int temp = 1;
            gvAssetDetails.DataSource = ViewState["AssetDetails"] = dtAssetDetails;
            gvAssetDetails.DataBind();
            if (ddlLRNNumber.SelectedIndex > 0)
            {
                foreach (GridViewRow grvData1 in gvAssetDetails.Rows)
                {
                    Label lblEngineNumber = (Label)grvData1.FindControl("lblEngineNumber");
                    Label lblSerialNumber = (Label)grvData1.FindControl("lblSerialNumber");
                    Label lblAsset_Type = (Label)grvData1.FindControl("lblAsset_Type");
                    CheckBox CbAssets = ((CheckBox)grvData1.FindControl("CbAssets"));
                    if (CbAssets.Enabled == true)
                    {
                        temp = 0;
                        break;
                    }
                    else
                    {
                        temp = 1;

                    }

                }
                if (temp == 1)
                {
                    Utility.FunShowAlertMsg(this, "Asset has to be Identified");
                    //return;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Load Grid");
        }
    }
    #endregion
    #endregion


    # region Drop Down Selected Value Change Event
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tbRepossessionDetails.Enabled = false;
            ddlLRNNumber.Items.Clear();
            FunPriClear();
            ddlLOB.Focus();
            if(ddlLOB.SelectedIndex > 0)
            PopulateBranchList();
            //ddlBranch.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = ex.Message;
            cvRepossession.IsValid = false;
            return;
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tbRepossessionDetails.Enabled = false;
            ddlLRNNumber.Items.Clear();
            FunPriClear();
            PopulateLRNumber();
            ddlBranch.Focus();
        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = ex.Message;
            cvRepossession.IsValid = false;
            return;
        }
    }

    protected void ddlLRNNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            tbRepossessionDetails.Enabled = false;
            btnViewAccount.Enabled = false;
            FunPriClear();
            DataSet dsAccountDetails;

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@LRN_ID", Convert.ToString(ddlLRNNumber.SelectedValue));
            dsAccountDetails = Utility.GetDataset("S3G_LR_GetRepossessionDetails", Procparam);
            if (dsAccountDetails.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountDetails = dsAccountDetails.Tables[0];
                txtPANum.Text = dtAccountDetails.Rows[0]["PANum"].ToString();
                txtSANum.Text = dtAccountDetails.Rows[0]["SANum"].ToString();
                txtAmtFinanced.Text = dtAccountDetails.Rows[0]["Finance_Amount"].ToString();
                txtTenure.Text = dtAccountDetails.Rows[0]["Tenure"].ToString();
                txtAccountDate.Text = DateTime.Parse(dtAccountDetails.Rows[0]["Creation_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                lblCustID.Text = dtAccountDetails.Rows[0]["Customer_ID"].ToString();
                S3GCustomerAddress1.SetCustomerDetails(dtAccountDetails.Rows[0], true);
            }


            // To load guarantor details
            if (ProcparamGuarantor != null)
                ProcparamGuarantor.Clear();
            else

                ProcparamGuarantor = new Dictionary<string, string>();
            ProcparamGuarantor.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            ProcparamGuarantor.Add("@LRN_ID", ddlLRNNumber.SelectedValue.ToString());
            DataSet Dst = Utility.GetDataset("S3G_LR_GetLRNNumDetails", ProcparamGuarantor);
            DataTable dtGuarDetails = Dst.Tables[1];


            if (dtGuarDetails.Rows.Count < 1)
            {
                lblGuarDetails.Visible = true;
                lblGuarDetails.Font.Size = 12;
                gvGuarDetails.DataSource = null;
                gvGuarDetails.DataBind();


            }
            else
            {
                lblGuarDetails.Visible = false;
                gvGuarDetails.DataSource = dtGuarDetails;
                gvGuarDetails.DataBind();
            }
            rdoYes.Checked = true;
            FunProFillgrid(dsAccountDetails.Tables[1]);
            PopulateGarageDetails();
            if (ddlLRNNumber.SelectedIndex > 0)
                btnViewAccount.Enabled = true;

        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Unable to Load Repossession Details";
            cvRepossession.IsValid = false;
            return;
        }

    }

    private void FunPriLoadDoneBy()
    {
        ListItem lstSelect = new ListItem("--Select--", "0");
        ListItem lstEmployee = new ListItem("Employee", "1");
        ListItem lstAgent = new ListItem("Agent", "2");
        ddlRepossessionDoneBy.Items.Add(lstSelect);
        ddlRepossessionDoneBy.Items.Add(lstEmployee);
        ddlRepossessionDoneBy.Items.Add(lstAgent);
        ddlRepossessionDoneBy.SelectedIndex = 0;
    }

    protected void ddlRepossessionDoneBy_SelectedIndexChanged(object sender, EventArgs e)
    {

        //ddlAgentCode.ClearDropDownList();
        if (ddlRepossessionDoneBy.SelectedIndex > 0)
            PopulateAgentDetails();
        else
            ddlAgentCode.Items.Clear();

    }


    protected void ddlGarageCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlGarageCode.ClearDropDownList();
        S3GGarageAddress.ClearCustomerDetails();
        FunGetGuarageDetails(Convert.ToInt32(ddlGarageCode.SelectedValue));

    }

    #endregion
    # region Asset Selection
    protected void CbAssets_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAsset = null;
            CheckBox CbAssets = null;
            //GridViewRow grvData;
            txtInsuranceDate.Text = "";

            foreach (GridViewRow grvData in gvAssetDetails.Rows)
            {
                CbAssets = ((CheckBox)grvData.FindControl("CbAssets"));
                if (CbAssets.Checked)
                {
                    intAsset_ID = Convert.ToInt32(((Label)grvData.FindControl("lblAssetID")).Text);

                    //Procparam = new Dictionary<string, string>();
                    //Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                    //Procparam.Add("@Asset_Number", (intAsset_ID).ToString());
                    //if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
                    //    Procparam.Add("@LOB_CODE", "OL");
                    //else
                    //    Procparam.Add("@LOB_CODE", "NOT");
                    //DataTable dtAssetIden = Utility.GetDefaultData("S3G_LR_GetAssetIdenDetails", Procparam);
                    //if (dtAssetIden.Rows[0][0].ToString() == "0")
                    //{
                    //    tbRepossessionDetails.Enabled = false;
                    //    CbAssets.Checked = CbAssets.Enabled = false;
                    //    Utility.FunShowAlertMsg(this, "The Selected Asset not yet Identified");
                    //    break;
                    //}
                    //else
                    //{
                    FunClearRepossessionDetails();
                    tbRepossessionDetails.Enabled = true;
                    FunPriGetInsurancePolicy(intAsset_ID);
                    //}



                }
                else
                {

                    CbAssets.Enabled = false;
                    //intAsset_ID = 0;
                }

            }

            if (intAsset_ID == 0)
            {
                foreach (GridViewRow grvData1 in gvAssetDetails.Rows)
                {
                    Label lblEngineNumber = (Label)grvData1.FindControl("lblEngineNumber");
                    Label lblSerialNumber = (Label)grvData1.FindControl("lblSerialNumber");
                    Label lblAsset_Type = (Label)grvData1.FindControl("lblAsset_Type");
                    CbAssets = ((CheckBox)grvData1.FindControl("CbAssets"));
                    if (string.IsNullOrEmpty(lblEngineNumber.Text) && string.IsNullOrEmpty(lblSerialNumber.Text))
                    {
                        //CbAssets.Checked = false;
                        CbAssets.Enabled = false;
                        //Utility.FunShowAlertMsg(this, "The Asset Doesn't Identified");
                        //tbRepossessionDetails.Enabled = false;
                    }
                    else if ((string.IsNullOrEmpty(lblEngineNumber.Text)) && ((lblAsset_Type.Text == "93") || (lblAsset_Type.Text == "94")))
                    {
                        //CbAssets.Checked = false;
                        CbAssets.Enabled = false;
                        //Utility.FunShowAlertMsg(this, "The Asset Doesn't Identified");
                        //tbRepossessionDetails.Enabled = false;
                    }
                    else if ((string.IsNullOrEmpty(lblSerialNumber.Text)) && ((lblAsset_Type.Text == "205") || (lblAsset_Type.Text == "206") || (lblAsset_Type.Text == "207") || (lblAsset_Type.Text == "208") || (lblAsset_Type.Text == "209") || (lblAsset_Type.Text == "210") || (lblAsset_Type.Text == "211") || (lblAsset_Type.Text == "212") || (lblAsset_Type.Text == "213") || (lblAsset_Type.Text == "214")))
                    {
                        //CbAssets.Checked = false;
                        CbAssets.Enabled = false;
                        //Utility.FunShowAlertMsg(this, "The Asset Doesn't Identified");
                        //tbRepossessionDetails.Enabled = false;
                    }
                    else
                    {
                        CbAssets.Enabled = true;
                        tbRepossessionDetails.Enabled = false;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = ex.Message;
            cvRepossession.IsValid = false;
            return;
        }

    }
    #endregion

    #region Commented gvAssetDetails_RowDataBound
    protected void gvAssetDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEngineNumber = (Label)e.Row.FindControl("lblEngineNumber");
            Label lblSerialNumber = (Label)e.Row.FindControl("lblSerialNumber");
            Label lblAsset_Type = (Label)e.Row.FindControl("lblAsset_Type");

            CheckBox CbAssets = (CheckBox)e.Row.FindControl("CbAssets");

            if (string.IsNullOrEmpty(lblEngineNumber.Text) && string.IsNullOrEmpty(lblSerialNumber.Text))
            {
                CbAssets.Enabled = false;
            }
            else if ((string.IsNullOrEmpty(lblEngineNumber.Text)) && ((lblAsset_Type.Text == "93") || (lblAsset_Type.Text == "94")))
            {
                CbAssets.Enabled = false;
            }
            else if ((string.IsNullOrEmpty(lblSerialNumber.Text)) && ((lblAsset_Type.Text == "205") || (lblAsset_Type.Text == "206") || (lblAsset_Type.Text == "207") || (lblAsset_Type.Text == "208") || (lblAsset_Type.Text == "209") || (lblAsset_Type.Text == "210") || (lblAsset_Type.Text == "211") || (lblAsset_Type.Text == "212") || (lblAsset_Type.Text == "213") || (lblAsset_Type.Text == "214")))
            {
                CbAssets.Enabled = false;
            }
            if (!CbAssets.Enabled)
            {
                //if (string.IsNullOrEmpty(lblEngineNumber.Text) && string.IsNullOrEmpty(lblSerialNumber.Text))
                //{
                //    e.Row.Attributes.Add("onclick", " return alert('Asset need to Identified');");

                //}
                if ((string.IsNullOrEmpty(lblEngineNumber.Text)) && ((lblAsset_Type.Text == "93") || (lblAsset_Type.Text == "94")))
                {
                    e.Row.Attributes.Add("onclick", "return alert('Vehicle Registration Number of the Asset is not Identified');");
                }
                else if ((string.IsNullOrEmpty(lblSerialNumber.Text)) && ((lblAsset_Type.Text == "205") || (lblAsset_Type.Text == "206") || (lblAsset_Type.Text == "207") || (lblAsset_Type.Text == "208") || (lblAsset_Type.Text == "209") || (lblAsset_Type.Text == "210") || (lblAsset_Type.Text == "211") || (lblAsset_Type.Text == "212") || (lblAsset_Type.Text == "213") || (lblAsset_Type.Text == "214")))
                {
                    e.Row.Attributes.Add("onclick", "return alert('Serial Number of the Asset is not Identified');");
                }

            }

        }
    }

    #endregion

    #region Button Events
    #region Save Option
    protected void btnSave_Click(object sender, EventArgs e)
    {
        objClient = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();
        try
        {
            if (ddlRepossessionDoneBy.SelectedIndex == -1)
            {
                cvRepossession.ErrorMessage = "Select Repossession Done By";
                cvRepossession.IsValid = false;
                return;
                //Utility.FunShowAlertMsg(this, "Select Repossession Done By");
                //return;

            }
            objLRDataTable = new S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionDataTable();
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionRow objRow;
            objRow = objLRDataTable.NewS3G_LR_RepossessionRow();

            objRow.Company_ID = Convert.ToInt32(intCompanyID);
            objRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objRow.LRN_ID = Convert.ToInt32(ddlLRNNumber.SelectedValue);
            objRow.LRN_No = Convert.ToString(ddlLRNNumber.SelectedItem.Text.Substring(0, ddlLRNNumber.SelectedItem.Text.LastIndexOf("-"))).Trim();
            objRow.PANum = txtPANum.Text;
            objRow.SANum = txtSANum.Text;
            objRow.Repossession_Docket_Date = Utility.StringToDate(txtRepoDate.Text.Trim());
            //objRow.IS_Active = Convert.ToBoolean(ChkActive.Checked);
            objRow.Customer_ID = Convert.ToInt32(lblCustID.Text.Trim());
            objRow.Repossession_Date = Utility.StringToDate(txtRepossessionDate.Text.Trim());
            objRow.Repossession_Place = txtPlace.Text.Trim();
            objRow.Asset_Condition = txtCondition.Text.Trim();
            if (rdoYes.Checked)
                objRow.Informed_To_Police = Convert.ToString(true);
            else
                objRow.Informed_To_Police = Convert.ToString(false);

            objRow.Repossession_By = Convert.ToString(ddlRepossessionDoneBy.SelectedItem.Text);
            objRow.Garage_ID = Convert.ToInt32(ddlGarageCode.SelectedValue);
            objRow.Garage_In = Utility.StringToDate(txtRepoDate.Text.Trim());
            objRow.Repossesser_ID = Convert.ToInt32(ddlAgentCode.SelectedValue);
            objRow.Current_Market_Value = Convert.ToDecimal(txtCurrentMarket.Text);
            objRow.Asset_Inventory_Details = txtAssetInventory.Text.Trim();
            objRow.Repossession_Expences = txtRepoExp.Text.Trim();
            objRow.Remarks = txtRemarks.Text.Trim();
            objRow.Created_By = Convert.ToInt32(intUserID);
            objRow.Created_On = Utility.StringToDate(txtRepoDate.Text.Trim());
            objRow.Repossession_ID = intRepossession_ID;



            int count = 0;
            foreach (GridViewRow grv in gvAssetDetails.Rows)
            {
                if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                {
                    count++;

                }

            }
            if (count == 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Asset");
                tcRepossession.ActiveTabIndex = 0;
                return;
            }
            if (txtCurrentMarket.Text != "")
            {
                decimal tmpAssetCost = 0;
                foreach (GridViewRow grv in gvAssetDetails.Rows)
                {
                    if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                    {
                        Label lblAssetCost = grv.FindControl("lblAssetCost") as Label;
                        tmpAssetCost = Convert.ToDecimal(lblAssetCost.Text);

                    }

                }
                if ((Convert.ToDecimal(txtCurrentMarket.Text)) >= (Convert.ToDecimal(tmpAssetCost)))
                {
                    Utility.FunShowAlertMsg(this, "Enter the Current Market Value less than the Asset Value(" + tmpAssetCost + ")");
                    txtCurrentMarket.Text = "";
                    tcRepossession.ActiveTabIndex = 1;
                    txtCurrentMarket.Focus();
                    return;
                }
            }
            foreach (GridViewRow grv in gvAssetDetails.Rows)
            {
                if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                {
                    Label lblAssetNumber = grv.FindControl("lblAssetID") as Label;
                    objRow.Asset_Number = Convert.ToInt32(lblAssetNumber.Text);
                    Label lblAssetCode = grv.FindControl("lblAssetCode") as Label;
                    objRow.Asset_Code = Convert.ToString(lblAssetCode.Text);

                }
            }
            objLRDataTable.AddS3G_LR_RepossessionRow(objRow);
            intErrorCode = objClient.FunCreateRepossesion(out strRepo, SerMode, ClsPubSerialize.Serialize(objLRDataTable, SerMode));

            if (intErrorCode == -1)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                return;
            }
            else if (intErrorCode == -2)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                return;
            }
            else if (intErrorCode == 3)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Selected asset has already Repossessed');" + strRedirectPageView, true);
            }

            if (intErrorCode == 50)
            {
                cvRepossession.ErrorMessage = "Error in Saving";
                cvRepossession.IsValid = false;
                return;
            }



            if (strMode == "M")
            {
                //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Repossession Details updated successfully');" + strRedirectPageView, true);

            }

            else
            {
                //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                strAlert = "Repossession " + strRepo + " added successfully";
                strAlert += @"\n\nWould you like to add one more record?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }


            //return;
        }


        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = ex.Message;
            cvRepossession.IsValid = false;
            return;

        }
        finally
        {
            if (objClient != null)
                objClient.Close();
        }

    }
    #endregion
    #region Clear Event
    protected void btnClear_Click(object sender, EventArgs e)
    {

        ddlLOB.SelectedIndex = -1;
        //ddlBranch.SelectedIndex = -1;
        //ddlBranch.Items.Clear();
        ddlLRNNumber.Items.Clear();
        //ddlLRNNumber.ClearDropDownList();

        FunPriClear();

    }
    #endregion
    // Changed by Bhuvana.Here i have included to close the particular Popup menu.
    #region Cancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["IsPopup"] != null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
        }
        else
        {
            Response.Redirect("~/LegalRepossession/S3GLRTransLander.aspx?Code=GRP");
        }
    }
    #endregion
    #endregion


    #region Get Repossession Details

    private void FunGetRepossionDetails(int repoid)
    {
        try
        {
            tbRepossessionDetails.Enabled = true;
            if (Procparam != null)
                Procparam.Clear();
            else

                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Repossession_ID", intRepossession_ID.ToString());

            DataSet dsRepossession = Utility.GetDataset("S3G_LR_GetRepossessionDetailsForModification", Procparam);

            if (dsRepossession.Tables[0].Rows.Count > 0)
            {

                DataRow dtRow = dsRepossession.Tables[0].Rows[0];

                ViewState["Release"] = dtRow["Release_Status"].ToString();
                ddlLOB.SelectedValue = dtRow["LOB_ID"].ToString();
                ddlBranch.SelectedValue = dtRow["Location_ID"].ToString();
                ddlBranch.SelectedText = dtRow["Location_Name"].ToString();
                ddlLRNNumber.SelectedValue = dtRow["LRN_ID"].ToString();
                txtPANum.Text = dtRow["PANum"].ToString();
                txtSANum.Text = dtRow["SANum"].ToString();
                txtRepoNo.Text = dtRow["Repossession_Docket_No"].ToString();
                lblCustID.Text = dtRow["Customer_ID"].ToString();
                S3GCustomerAddress1.SetCustomerDetails(dtRow, true);
                txtRepossessionDate.Text = DateTime.Parse(dtRow["Repossession_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtRepoDate.Text = DateTime.Parse(dtRow["Repossession_Docket_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtAccountDate.Text = DateTime.Parse(dtRow["Creation_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtAmtFinanced.Text = dtRow["Finance_Amount"].ToString();
                txtTenure.Text = dtRow["Tenure"].ToString();
                txtPlace.Text = dtRow["Repossession_Place"].ToString();
                txtCondition.Text = dtRow["Asset_Condition"].ToString();
                //ChkActive.Enabled = true;
                //ChkActive.Checked = Convert.ToBoolean(dtRow["IS_Active"].ToString());
                //ddlRepossessionDoneBy.SelectedItem.Text = dtRow["Repossession_By"].ToString();
                ddlRepossessionDoneBy.SelectedIndex = ddlRepossessionDoneBy.Items.IndexOf(ddlRepossessionDoneBy.Items.FindByText(dtRow["Repossession_By"].ToString()));
                PopulateAgentDetails();
                ddlAgentCode.SelectedValue = dtRow["Repossesser_ID"].ToString();
                ddlGarageCode.SelectedValue = dtRow["Garage_ID"].ToString();
                S3GGarageAddress.SetCustomerDetails(dtRow["Garage_Code"].ToString(), dtRow["Garage_Address1"].ToString() + "\n" + dtRow["Garage_Address2"].ToString() + "\n" + dtRow["Garage_State"].ToString() + "\n" + dtRow["Garage_Pin"].ToString() + "\n" + dtRow["Garage_Country"].ToString(), dtRow["Garage_Code"].ToString(), dtRow["Garage_Telephone"].ToString(), dtRow["Garage_Mobile"].ToString(), dtRow["Garage_Email_ID"].ToString(), dtRow["Garage_Web_Site"].ToString());
                txtGarageIn.Text = DateTime.Parse(dtRow["Garage_In"].ToString()).ToString(strDateFormat);
                if (bool.Parse(dtRow["Informed_To_Police"].ToString()))
                    rdoYes.Checked = true;
                else
                    rdoNo.Checked = true;
                txtRepoExp.Text = dtRow["Repossession_Expences"].ToString();
                //txtInsuranceDate.Text = DateTime.Parse(dtRow["Repossession_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtAssetInventory.Text = dtRow["Asset_Inventory_Details"].ToString();
                txtCurrentMarket.Text = dtRow["Current_Market_Value"].ToString();
                txtRemarks.Text = dtRow["Remarks"].ToString();
                CalendarExtenderRepossessionDate.Enabled = true;

                if (dtRow["Release_Status"].ToString() != "0")
                {
                    btnSave.Enabled = false;
                    txtAssetInventory.ReadOnly = true;
                    txtCondition.ReadOnly = true;
                    txtCurrentMarket.ReadOnly = true;
                    txtRemarks.ReadOnly = true;
                    txtPlace.ReadOnly = true;
                    txtRepossessionDate.ReadOnly = true;
                    //txtRepossessionDate.Enabled = false;
                    txtAssetInventory.ReadOnly = true;
                    txtCondition.ReadOnly = true;
                    txtRepoExp.ReadOnly = true;
                    rdoNo.Enabled = false;
                    rdoYes.Enabled = false;
                    //ChkActive.Enabled = false;
                    CalendarExtenderRepossessionDate.Enabled = false;
                    imgDateofActivation.Visible = false;
                    txtRepoDate.ReadOnly = true;

                }

                if (dsRepossession.Tables[2].Rows.Count < 1)
                {
                    lblGuarDetails.Visible = true;
                    lblGuarDetails.Font.Size = 12;
                    gvGuarDetails.DataSource = null;
                    gvGuarDetails.DataBind();


                }
                else
                {
                    lblGuarDetails.Visible = false;
                    gvGuarDetails.DataSource = dsRepossession.Tables[2];
                    gvGuarDetails.DataBind();
                }
            }

            DataTable dtAssetDetails1 = dsRepossession.Tables[1];
            gvAssetDetails.DataSource = ViewState["AssetDetails"] = dtAssetDetails1;
            gvAssetDetails.DataBind();

            foreach (GridViewRow grvData in gvAssetDetails.Rows)
            {
                CheckBox CbAssets = null;
                CbAssets = ((CheckBox)grvData.FindControl("CbAssets"));
                CbAssets.Checked = true;
                CbAssets.Enabled = false;

            }
            if (dsRepossession.Tables[3].Rows.Count > 0)
            {
                DataRow dtIns = dsRepossession.Tables[3].Rows[0];
                txtInsuranceDate.Text = DateTime.Parse(dtIns["Policy_To_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            }
        }

        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = "Due to Data Problem the Repossession Can't be Loaded";
            cvRepossession.IsValid = false;
            return;

        }
    }

    /// <summary>
    /// To set the Suffix to total
    /// </summary>
    /// <returns></returns>
    //private string Funsetsuffix()
    //{

    //    int suffix = 1;
    //    S3GSession ObjS3GSession = new S3GSession();
    //    suffix = ObjS3GSession.ProGpsSuffixRW;
    //    string strformat = "0.";
    //    for (int i = 1; i <= suffix; i++)
    //    {
    //        strformat += "0";
    //    }
    //    return strformat;
    //}
    #endregion


    protected void btnViewAccount_Click(object sender, EventArgs e)
    {
        try
        {
            string s = "";
            if (txtSANum.Text == "")
                s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=" + txtPANum.Text + "&qsSANum=" + txtPANum.Text + "DUMMY";

            else
                s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=" + txtPANum.Text + "&qsSANum=" + txtSANum.Text;

            // s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=2011-2012/PRIME/10001&qsSANum=2011-2012/WCSAN/100";

            string strScipt = "window.open('" + s + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SOA", strScipt, true);
        }
        catch (Exception ex)
        {
            cvRepossession.ErrorMessage = ex.Message;
            cvRepossession.IsValid = false;
            return;
        }
    }

    protected void txtRepossessionDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtRepossessionDate.Text))
        {
            if (ddlLRNNumber.Items.Count > 0)
            {
                if (Convert.ToDateTime(Utility.StringToDate(txtRepossessionDate.Text).ToString("dd/MMM/yyyy")) <
                    Convert.ToDateTime(Utility.StringToDate(txtAccountDate.Text).ToString("dd/MMM/yyyy")))
                {
                    Utility.FunShowAlertMsg(this, "Repossession date should be greater than the Account Creation Date (" + txtAccountDate.Text + ")");
                    txtRepossessionDate.Text = string.Empty;
                }
            }
        }
    }





}
