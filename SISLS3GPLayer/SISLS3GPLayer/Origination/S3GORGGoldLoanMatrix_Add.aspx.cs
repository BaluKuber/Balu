//Program summary
/// Module Name     :   Origination
/// Screen Name     :   Asset Matrix
/// Created By      :   Chandrashekar.S
/// Created Date    :   04-May-2012
/// Purpose         :   To view Asset Matrix details
//Program summary
using System;
using System.Web.UI;
using System.Data;
using System.Text;
using S3GBusEntity.LoanAdmin;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Resources;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using S3GBusEntity.Origination;

public partial class Origination_S3GORGGoldLoanMatrix_Add : ApplyThemeForProject
{
    #region initialization
    SerializationMode SerMode = SerializationMode.Binary;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    OrgMasterMgtServices.S3G_ORG_GoldLoanMatrixDataTable ObjGoldLoanMatrixDataTable = null;
    PagingValues ObjPaging = new PagingValues();
    int intErrCode = 0;
    int intMatrixID = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int intRowId = 0;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    //Code end    
    Dictionary<string, string> Procparam = null;
    Dictionary<string, string> Procparam1 = null;
    string strRedirectPageView = "window.location.href='../Origination/S3gOrgGoldLoanMatrix_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3gOrgGoldLoanMatrix_Add.aspx?qsMode=C';";
    string strRedirectPage = "~/Origination/S3gOrgGoldLoanMatrix_View.aspx";
    string strAlert = "alert('__ALERT__');";
    string strMJVNumber = string.Empty;
    string strDateFormat = string.Empty;
    DataTable dtGLMtr = new DataTable();
    #endregion

    #region PageLoad
    //transaction screen page init
    protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
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
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        CalendarExtender1.Format = strDateFormat;

        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            if (fromTicket != null)
            {
                intMatrixID =Convert.ToInt32(fromTicket.Name);
            }
        }

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        if (!IsPostBack)
        {
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            txtEffectiveDate.Attributes.Add("readonly", "readonly");
            
            if (strMode == "C")
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                FunPriInitializeFundingGrid(false);
                FunPriInitializeRateGrid(false);
                FunPriBindAssetCategory();
                FunProPopulateLocationList();
                FunPriBindConstitution();
                FunPriBindProduct();
                FunPubProGetUnit();
                FunPriLoadTenureType();
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlAssetDesc.Items.Insert(0, liSelect);
            }
            if (strMode == "M" || strMode == "Q")
            {
                btnGo.Enabled = false;
                CalendarExtender1.Enabled = false;
                FunPriBindModifyData();
                if (strMode == "M")
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    btnSave.Enabled = true;
                    btnClear.Enabled = false;
                }
                else
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = false;
                    btnClear.Enabled = btnSave.Enabled = false;
                    grvFundDetails.Columns[grvFundDetails.Columns.Count - 1].Visible = false;
                    grvRateDetails.Columns[grvRateDetails.Columns.Count - 1].Visible = false;
                }
            }
        }
    }
    #endregion

    #region Page Events
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            
            ObjGoldLoanMatrixDataTable = new OrgMasterMgtServices.S3G_ORG_GoldLoanMatrixDataTable();
            OrgMasterMgtServices.S3G_ORG_GoldLoanMatrixRow ObjGoldLoanMatrixRow;

            ObjGoldLoanMatrixRow = ObjGoldLoanMatrixDataTable.NewS3G_ORG_GoldLoanMatrixRow();
            ObjGoldLoanMatrixRow.GLMatrix_ID = intMatrixID;

            ObjGoldLoanMatrixRow.Asset_ID = Convert.ToInt32(ddlAssetDesc.SelectedValue);
            ObjGoldLoanMatrixRow.Company_ID = intCompanyID;
            ObjGoldLoanMatrixRow.Constitution_ID = Convert.ToInt32(ddlConstitutionName.SelectedValue);
            ObjGoldLoanMatrixRow.Product_ID = Convert.ToInt32(ddlProductName.SelectedValue);
            ObjGoldLoanMatrixRow.Location_Code = ddlLocation.SelectedValue;
            ObjGoldLoanMatrixRow.Unit_ID = Convert.ToInt32(ddlUnit.SelectedValue);
            ObjGoldLoanMatrixRow.Eff_Date = Utility.StringToDate(txtEffectiveDate.Text);

            ObjGoldLoanMatrixRow.Tenure = Convert.ToDecimal(txtTenure.Text);
          
            ObjGoldLoanMatrixRow.Tenure_Type = Convert.ToInt32(ddlTenureType.SelectedValue);
            
            ObjGoldLoanMatrixRow.Created_By = intUserID;
            ObjGoldLoanMatrixRow.Created_On = DateTime.Today;
            FunPriReAssignFundingValues();
            ObjGoldLoanMatrixRow.XMLFundingDtl = ((DataTable)ViewState["DtFunding"]).FunPubFormXml();
            ObjGoldLoanMatrixRow.XMLRateDtl = ((DataTable)ViewState["DtRate"]).FunPubFormXml();

            ObjGoldLoanMatrixDataTable.AddS3G_ORG_GoldLoanMatrixRow(ObjGoldLoanMatrixRow);

            intErrCode = ObjOrgMasterMgtServicesClient.FunPubCreateGoldLoanMatrix(SerMode, ClsPubSerialize.Serialize(ObjGoldLoanMatrixDataTable, SerMode));

            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intMatrixID != 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Gold Loan Matrix details updated successfully");
                }
                else
                {
                    strAlert = "Gold Loan Matrix Details added successfully.";
                    strAlert += @"\n\nWould you like to add one more Gold Loan Matrix?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
            }
            else if (intErrCode == 10)
            {
                strAlert = strAlert.Replace("__ALERT__", "Gold Loan Matrix already exists for the present date");
                strRedirectPageView = "";
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Error in saving Gold Loan Matrix");
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Save", strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            lblClassDesc.Text = "";
            lblAssetTypDesc.Text = "";
            lblAssetModelDesc.Text = "";
            lblAssetMakeDesc.Text = "";

            ddlAssetCategory.SelectedIndex = 0;
            ddlConstitutionName.SelectedIndex = 0;
            ddlLocation.SelectedIndex = 0;
            ddlProductName.SelectedIndex = 0;
            ddlAssetDesc.SelectedIndex = 0;
            ddlAssetDesc.ClearDropDownList();
            ddlUnit.SelectedValue = "0";
            txtEffectiveDate.Text = "";

            FunPriInitializeFundingGrid(false);
            FunPriInitializeRateGrid(false);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void ddlAssetDesc_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPubProGetAssetDetails(intCompanyID, int.Parse(ddlAssetDesc.SelectedValue));
        FunProResetGrids();
    }
    protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriBindAssetDesc(ddlAssetCategory.SelectedValue);
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    { }
    
    protected void grvFundDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtMaxAmount = (TextBox)e.Row.FindControl("txtMaxAmount");
            DropDownList ddlPurityFrom = (DropDownList)e.Row.FindControl("ddlPurityFrom");
            DropDownList ddlPurityTo = (DropDownList)e.Row.FindControl("ddlPurityTo");

            if (txtMaxAmount != null)
            {
                txtMaxAmount.SetDecimalPrefixSuffix(10, 2, true, "Max Amount value");
            }
            
            if (strMode != "Q")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Clear();
                Procparam.Add("@Option", "1");
                Procparam.Add("@Param1", "Gold_Purity");

                DataTable dtPurity = Utility.GetDefaultData("S3G_ORG_GetGlobalLookUp", Procparam);
                ddlPurityFrom.FillDataTable(dtPurity, "Value", "Name");
                ddlPurityTo.FillDataTable(dtPurity, "Value", "Name");
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFundingID = (Label)e.Row.FindControl("lblFundingID");
            CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            TextBox txtIMaxAmount = (TextBox)e.Row.FindControl("txtIMaxAmount");
            Label lblMaxAmount = (Label)e.Row.FindControl("lblMaxAmount");
            
            if (txtIMaxAmount != null)
            {
                txtIMaxAmount.SetDecimalPrefixSuffix(10, 2, true, "Max Amount value");
            }

            if (lblFundingID.Text != "0" && strMode == "M")
            {
                chkActive.Enabled = true;
                lnkDelete.Enabled = false;
                lnkDelete.OnClientClick = string.Empty;
            }

            if (strMode == "Q")
            {
                lblMaxAmount.Visible = true;
                txtIMaxAmount.Visible = false;
            }
        }
    }
    
    protected void grvFundDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunPriReAssignFundingValues();
        dtGLMtr = (DataTable)ViewState["DtFunding"];
        dtGLMtr.Rows.RemoveAt(e.RowIndex);
        if (dtGLMtr.Rows.Count == 0)
        {
            FunPriInitializeFundingGrid(true);
        }
        else
        {
            FunPriBindFundingGridDetails(dtGLMtr, true);
        }
    }
    #endregion
    #region Page Methods
    /// <summary>
    /// to bind LOB and Product details
    /// </summary>   
    private void FunPriBindAssetCategory()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Type", "Asset_Category");
            ddlAssetCategory.BindDataTable(SPNames.S3G_ORG_GetAssetCategory_List, Procparam, new string[] { "ID", "NAME" });
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadTenureType()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

        try
        {
            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.TENURE_TYPE.ToString();
            Utility.FillDLL(ddlTenureType, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriReAssignFundingValues()
    {
        dtGLMtr = (DataTable)ViewState["DtFunding"];

        foreach (GridViewRow GRow in grvFundDetails.Rows)
        {
            TextBox txtIMaxAmount = (TextBox)GRow.FindControl("txtIMaxAmount");
            if (!string.IsNullOrEmpty(txtIMaxAmount.Text))
            {
                dtGLMtr.Rows[GRow.RowIndex]["MaxAmount"] = txtIMaxAmount.Text;
            }
        }

        ViewState["DtFunding"] = dtGLMtr;
    }

    private void FunPriInitializeFundingGrid(bool blShowFooter)
    {

        dtGLMtr = new DataTable();
        DataRow dr;

        dtGLMtr.Columns.Add("FundingID");
        dtGLMtr.Columns.Add("PurityFrom");
        dtGLMtr.Columns.Add("PurityFromID");
        dtGLMtr.Columns.Add("PurityTo");
        dtGLMtr.Columns.Add("PurityToID");
        dtGLMtr.Columns.Add("MaxAmount");
        dtGLMtr.Columns.Add("Active");
        dr = dtGLMtr.NewRow();
        dr["Active"] = "True";
        dtGLMtr.Rows.Add(dr);

        FunPriBindFundingGridDetails(dtGLMtr, blShowFooter);

        dtGLMtr.Rows[0].Delete();
        ViewState["DtFunding"] = dtGLMtr;

        grvFundDetails.Rows[0].Visible = false;
    }

    private void FunPriBindFundingGridDetails(DataTable dtGLMtr, bool blShowFooter)
    {
        ViewState["DtFunding"] = dtGLMtr;
        grvFundDetails.DataSource = dtGLMtr;
        grvFundDetails.DataBind();

        grvFundDetails.FooterRow.Visible = blShowFooter;
    }


    private void FunPriInitializeRateGrid(bool blShowFooter)
    {

        DataTable dtRateFixing = new DataTable();
        DataRow dr;

        dtRateFixing.Columns.Add("Rate_ID");
        dtRateFixing.Columns.Add("FromAmt", typeof(decimal));
        dtRateFixing.Columns.Add("ToAmt", typeof(decimal));
        dtRateFixing.Columns.Add("Rate");
        dtRateFixing.Columns.Add("Active");
        dr = dtRateFixing.NewRow();
        dr["Active"] = "True";
        dtRateFixing.Rows.Add(dr);

        FunPriBindRateGridDetails(dtRateFixing, blShowFooter);

        dtRateFixing.Rows[0].Delete();
        ViewState["DtRate"] = dtRateFixing;

        grvRateDetails.Rows[0].Visible = false;
    }

    private void FunPriBindRateGridDetails(DataTable dtRate, bool blShowFooter)
    {
        ViewState["DtRate"] = dtRate;
        grvRateDetails.DataSource = dtRate;
        grvRateDetails.DataBind();

        grvRateDetails.FooterRow.Visible = blShowFooter;
    }

    #endregion

    protected void FunProPopulateLocationList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            if (PageMode == PageModes.Create)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Program_ID", "214");
            ddlLocation.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to get Location list");
        }
    }

    private void FunPriBindConstitution()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            ddlConstitutionName.BindDataTable("S3G_ORG_GetConstitutionDetails", Procparam, new string[] { "Constitution_Id", "Constitution_Name" });
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriBindProduct()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            ddlProductName.BindDataTable("S3G_Get_Product_List", Procparam, new string[] { "Product_Id", "Product_Name" });
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriBindAssetDesc(string intAssetTypeID)
    {
        if (intAssetTypeID != "0")
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
            Procparam.Add("@ASSETTYPE_ID", intAssetTypeID.ToString());
            if (PageMode == PageModes.Create)
            {
                Procparam.Add("@Is_Active", "1");
            }
            ddlAssetDesc.BindDataTable("S3G_ORG_GetAssetDescForType", Procparam, new string[] { "Asset_Id", "Asset_Description" });
        }
        else
        {
            ddlAssetDesc.SelectedValue = "0";
            ddlAssetDesc.ClearDropDownList();
        }

        FunProResetGrids();
    }

    protected void FunProResetGrids()
    {
        FunPriInitializeFundingGrid(false);
        FunPriInitializeRateGrid(false);
    }

    protected void FunPubProGetUnit()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Option", "1");
        Procparam.Add("@Param1", "Gold_Measurement");

        ddlUnit.BindDataTable("S3G_ORG_GetGlobalLookUp", Procparam, new string[] { "Value", "Name" });
    }

    protected void FunPubProGetAssetDetails(int intCompanyId, int intAssetId)
    {
        try
        {
            DataTable dtAssetDetails;
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
            Procparam.Add("@Asset_ID", intAssetId.ToString());//S3G_ORG_GetAssetDetails

            dtAssetDetails = Utility.GetDefaultData("S3G_ORG_GetAssetDetails", Procparam);

            if (dtAssetDetails.Rows.Count > 0)
            {
                lblClassDesc.Text = dtAssetDetails.Rows[0]["Class_Desc"].ToString();
                lblAssetMakeDesc.Text = dtAssetDetails.Rows[0]["Make_Desc"].ToString();
                lblAssetTypDesc.Text = dtAssetDetails.Rows[0]["Type_Desc"].ToString();
                lblAssetModelDesc.Text = dtAssetDetails.Rows[0]["Model_Desc"].ToString();
            }
            else
            {
                lblClassDesc.Text = "NIL";
                lblAssetMakeDesc.Text = "NIL";
                lblAssetTypDesc.Text = "NIL";
                lblAssetModelDesc.Text = "NIL";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Asset details");
        }
    }

    protected void grvFundDetails_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        dtGLMtr = (DataTable)ViewState["DtFunding"];
        DropDownList ddlPurityFrom = (DropDownList)grvFundDetails.FooterRow.FindControl("ddlPurityFrom");
        DropDownList ddlPurityTo = (DropDownList)grvFundDetails.FooterRow.FindControl("ddlPurityTo");
        TextBox txtMaxAmount = (TextBox)grvFundDetails.FooterRow.FindControl("txtMaxAmount");
        CheckBox chkActive1 = (CheckBox)grvFundDetails.FooterRow.FindControl("chkActive1");

        if (e.CommandName == "AddNew")
        {
            if (int.Parse(ddlPurityTo.SelectedValue) < int.Parse(ddlPurityFrom.SelectedValue))
            {
                Utility.FunShowAlertMsg(this.Page, "Purity To should be greater than are equal to Purity From");
                ddlPurityTo.Focus();
                return;
            }

            if (!FunProCheckOverlap(ddlPurityFrom.SelectedValue, ddlPurityTo.SelectedValue))
            {
                Utility.FunShowAlertMsg(this, "Purity order getting overlap");
                ddlPurityFrom.Focus();
                return;
            }

            FunPriReAssignFundingValues();

            dtGLMtr = (DataTable)ViewState["DtFunding"];
            DataRow dr = dtGLMtr.NewRow();

            dr["FundingID"] = "0";
            dr["PurityFrom"] = ddlPurityFrom.SelectedItem.Text;
            dr["PurityFromID"] = ddlPurityFrom.SelectedValue;
            dr["PurityTo"] = ddlPurityTo.SelectedItem.Text;
            dr["PurityToID"] = ddlPurityTo.SelectedValue;
            dr["MaxAmount"] = txtMaxAmount.Text;
            if (chkActive1.Checked)
            {
                dr["Active"] = "True";
            }
            else
            {
                dr["Active"] = "False";
            }

            dtGLMtr.Rows.Add(dr);

            FunPriBindFundingGridDetails(dtGLMtr, true);

            btnSave.Enabled = true;
        }
    }

    protected bool FunProCheckOverlap(string strPurityFrom, string strPurityTo)
    {
        DataTable dtPricingMtr = (DataTable)ViewState["DtFunding"];
        DataRow[] dRow = dtPricingMtr.Select("PurityFromID <= " + strPurityFrom + " and PurityToID >= " + strPurityFrom + " and Active = 'True'");
        if (dRow.Length > 0)
        {
            return false;
        }
        dRow = dtPricingMtr.Select("PurityFromID <= " + strPurityTo + " and PurityToID >= " + strPurityTo + " and Active = 'True'");
        if (dRow.Length > 0)
        {
            return false;
        }

        return true;
    }

    protected bool FunProCheckRateOverlap(string strFrom, string strTo)
    {
        DataTable dtPricingMtr = (DataTable)ViewState["DtRate"];
        DataRow[] dRow = dtPricingMtr.Select("FromAmt <= " + strFrom + " and ToAmt >= " + strFrom + " and Active = 'True'");
        if (dRow.Length > 0)
        {
            return false;
        }
        dRow = dtPricingMtr.Select("FromAmt <= " + strTo + " and ToAmt >= " + strTo + " and Active = 'True'");
        if (dRow.Length > 0)
        {
            return false;
        }

        return true;
    }

    private void FunPriBindModifyData()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@GLMatrix_ID", intMatrixID.ToString());
        DataSet dSet = Utility.GetDataset("S3G_ORG_GetgoldLoanMatrix", Procparam);
        DataTable ObjDataTable = dSet.Tables[0];
        if (ObjDataTable.Rows.Count > 0)
        {
            DataRow dRow = ObjDataTable.Rows[0];

            ddlAssetCategory.Items.Add(new ListItem(dRow["Asset_Type"].ToString(), dRow["AssetType_ID"].ToString()));
            ddlAssetCategory.SelectedValue = dRow["AssetType_ID"].ToString();

            ddlAssetDesc.Items.Add(new ListItem(dRow["Asset_Description"].ToString(), dRow["Asset_ID"].ToString()));
            ddlAssetDesc.SelectedValue = dRow["Asset_ID"].ToString();

            FunPubProGetAssetDetails(intCompanyID, int.Parse(ddlAssetDesc.SelectedValue));

            ddlLocation.Items.Add(new ListItem(dRow["Location"].ToString(), dRow["LocationID"].ToString()));
            ddlLocation.SelectedValue = dRow["LocationID"].ToString();

            ddlConstitutionName.Items.Add(new ListItem(dRow["Constitution_Name"].ToString(), dRow["Constitution_ID"].ToString()));
            ddlConstitutionName.SelectedValue = dRow["Constitution_ID"].ToString();

            ddlProductName.Items.Add(new ListItem(dRow["Product_Name"].ToString(), dRow["Product_ID"].ToString()));
            ddlProductName.SelectedValue = dRow["Product_ID"].ToString();

            ddlUnit.Items.Add(new ListItem(dRow["Unit"].ToString(), dRow["Unit_ID"].ToString()));
            ddlUnit.SelectedValue = dRow["Unit_ID"].ToString();

            txtEffectiveDate.Text = dRow["Eff_Date"].ToString();

            ddlTenureType.Items.Add(new ListItem(dRow["Tenure_Type"].ToString(), dRow["Tenure_ID"].ToString()));
            ddlTenureType.SelectedValue = dRow["Tenure_ID"].ToString();

            txtTenure.SetDecimalPrefixSuffix(3, 0, true, "Tenure");
            txtTenure.Text = dRow["Tenure"].ToString();

        }

        if (strMode == "M")
        {
            FunPriBindFundingGridDetails(dSet.Tables[1], true);
            FunPriBindRateGridDetails(dSet.Tables[2], true);
        }
        else
        {
            FunPriBindFundingGridDetails(dSet.Tables[1], false);
            FunPriBindRateGridDetails(dSet.Tables[2], false);
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (FunProCheckAvailability())
        {
            FunPriInitializeFundingGrid(true);
            FunPriInitializeRateGrid(true);
        }
        else
        {
            Utility.FunShowAlertMsg(this, "Matrix details exists for the given combination");
            return;
        }
    }

    protected bool FunProCheckAvailability()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Asset_ID", ddlAssetDesc.SelectedValue);
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Constitution_ID", ddlConstitutionName.SelectedValue);
        Procparam.Add("@Product_ID", ddlProductName.SelectedValue);
        Procparam.Add("@Location_Code", ddlLocation.SelectedValue);
        Procparam.Add("@Eff_Date", Utility.StringToDate(txtEffectiveDate.Text).ToString());
        Procparam.Add("@Tenure", txtTenure.Text.ToString());
        Procparam.Add("@Tenure_Type", ddlTenureType.SelectedValue);
       
           


        DataTable dtCheck = Utility.GetDefaultData("S3G_ORG_CheckForGoldLoanMatrix", Procparam);

        if (dtCheck != null && dtCheck.Rows.Count > 0)
        {
            if (dtCheck.Rows[0][0].ToString() == "1")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    protected void ddlConstitutionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProResetGrids();
    }

    protected void ddlProductName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProResetGrids();
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProResetGrids();
    }

    protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProResetGrids();
    }
    protected void ddlTenureType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProResetGrids();
    }

    protected void grvRateDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable DtRate = (DataTable)ViewState["DtRate"];
        TextBox txtFromAmt = (TextBox)grvRateDetails.FooterRow.FindControl("txtFromAmt");
        TextBox txtToAmt = (TextBox)grvRateDetails.FooterRow.FindControl("txtToAmt");
        TextBox txtRate = (TextBox)grvRateDetails.FooterRow.FindControl("txtRate");
        CheckBox chkActive1 = (CheckBox)grvRateDetails.FooterRow.FindControl("chkActive1");

        if (e.CommandName == "AddNew")
        {
            if (decimal.Parse(txtToAmt.Text) < decimal.Parse(txtFromAmt.Text))
            {
                Utility.FunShowAlertMsg(this.Page, "To amount should be greater than are equal to From amount");
                txtToAmt.Focus();
                return;
            }

            if (!FunProCheckRateOverlap(txtFromAmt.Text, txtToAmt.Text))
            {
                Utility.FunShowAlertMsg(this, "From and To amounts getting overlap");
                txtFromAmt.Focus();
                return;
            }

            DataRow dr = DtRate.NewRow();

            dr["Rate_ID"] = "0";
            dr["FromAmt"] = txtFromAmt.Text;
            dr["ToAmt"] = txtToAmt.Text;
            dr["Rate"] = txtRate.Text;
            if (chkActive1.Checked)
            {
                dr["Active"] = "True";
            }
            else
            {
                dr["Active"] = "False";
            }

            DtRate.Rows.Add(dr);

            FunPriBindRateGridDetails(DtRate, true);

            btnSave.Enabled = true;
        }
    }

    protected void grvRateDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable DtRate = (DataTable)ViewState["DtRate"];
        DtRate.Rows.RemoveAt(e.RowIndex);
        if (DtRate.Rows.Count == 0)
        {
            FunPriInitializeRateGrid(true);
        }
        else
        {
            FunPriBindRateGridDetails(DtRate, true);
        }
    }

    protected void grvRateDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtFromAmt = (TextBox)e.Row.FindControl("txtFromAmt");
            TextBox txtToAmt = (TextBox)e.Row.FindControl("txtToAmt");
            TextBox txtRate = (TextBox)e.Row.FindControl("txtRate");

            txtFromAmt.SetDecimalPrefixSuffix(10, 2, true, "From amount value");
            txtToAmt.SetDecimalPrefixSuffix(10, 2, true, "To amount value");
            txtRate.SetDecimalPrefixSuffix(3, 2, true, "Rate value");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblRate_ID = (Label)e.Row.FindControl("lblRate_ID");
            CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");

            if (lblRate_ID.Text != "0" && strMode == "M")
            {
                chkActive.Enabled = true;
                lnkDelete.Enabled = false;
                lnkDelete.OnClientClick = string.Empty;
            }
        }
    }

    protected void chkFundActive_OnCheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("grvFundDetails", ((CheckBox)sender).ClientID.ToString());
        CheckBox chkActive = (CheckBox)grvFundDetails.Rows[intRowIndex].FindControl("chkActive");
        Label lblPurityFromID = (Label)grvFundDetails.Rows[intRowIndex].FindControl("lblPurityFromID");
        Label lblPurityToID = (Label)grvFundDetails.Rows[intRowIndex].FindControl("lblPurityToID");

        DataTable DtFunding = (DataTable)ViewState["DtFunding"];

        if (chkActive.Checked)
        {
            DataRow[] dRow = DtFunding.Select("PurityFromID = " + lblPurityFromID.Text + " and PurityToID = " + lblPurityToID.Text + " and Active = 'True'");
            if (dRow.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Another active entry available for the givien Purity combination");
                chkActive.Checked = false;
                return;
            }

            DtFunding.Rows[intRowIndex]["Active"] = "True";
        }
        else
        {
            DtFunding.Rows[intRowIndex]["Active"] = "False";
        }

        ViewState["DtFunding"] = DtFunding;
    }

    protected void chkRateActive_OnCheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("grvRateDetails", ((CheckBox)sender).ClientID.ToString());
        CheckBox chkActive = (CheckBox)grvRateDetails.Rows[intRowIndex].FindControl("chkActive");
        Label lblFrom = (Label)grvRateDetails.Rows[intRowIndex].FindControl("lblFrom");
        Label lblTo = (Label)grvRateDetails.Rows[intRowIndex].FindControl("lblTo");

        DataTable DtRate = (DataTable)ViewState["DtRate"];

        if (chkActive.Checked)
        {
            DataRow[] dRow = DtRate.Select("FromAmt <= " + lblFrom.Text.Trim() + " and ToAmt >= " + lblTo.Text.Trim() + " and Active = 'True'");
            if (dRow.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Another active entry available for the givien Amount combination");
                chkActive.Checked = false;
                return;
            }

            DtRate.Rows[intRowIndex]["Active"] = "True";
        }
        else
        {
            DtRate.Rows[intRowIndex]["Active"] = "False";
        }

        ViewState["DtRate"] = DtRate;
    }

    protected void txtEffectiveDate_TextChanged(object sender, EventArgs e)
    {
        FunProResetGrids();
    }
}
