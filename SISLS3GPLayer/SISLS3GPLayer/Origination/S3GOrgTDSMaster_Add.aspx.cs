#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: TDS Master
/// Created By			: Swarna S
/// Created Date		: 8-Oct-2014
/// 
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using ORG = S3GBusEntity.Origination;
using ORGSERVICE = OrgMasterMgtServicesReference;
using System.Collections;
using System.Globalization;
using System.Configuration;
using System.Web;
using System.Security.Permissions;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
#endregion

public partial class Origination_S3GOrgTDSMaster_Add: ApplyThemeForProject
{
    #region Initialization
    int intCompanyId = 0;
    int intUserId = 0;
    int intTDSId = 0;
    int intErrorCode = 0;
    string strTDSCode;
    ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterDataTable ObjTDSMasterDataTable;
    //ORG.OrgMasterMgtServices.S3G_ORG_EntityBankMappingDataTable ObjEntityBankMappingDataTable;
    ORGSERVICE.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgTDSMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgTDSMaster_Add.aspx';";
    string strRedirectPage = "~/Origination/S3GOrgTDSMaster_Add.aspx";
    string strDateFormat = string.Empty;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    public static Origination_S3GOrgTDSMaster_Add obj_Page;
    //Code end

    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            //Code end

            obj_Page = this;
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString["qsTDSId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsTDSId"));
                if (fromTicket != null)
                {
                    intTDSId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid TDS Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarValidupto.Format = strDateFormat;
            txtValidupto.Attributes.Add("onblur", "fnDoDate(this,'" + txtValidupto.ClientID + "','" + strDateFormat + "',false,  false);");
            if (!IsPostBack)
            {
                FunPriSetControlSettings();
               

                if (PageMode != PageModes.Query)
                {
                    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objTDSMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                    S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                    
                    ObjStatus.Param1 = S3G_Statu_Lookup.COMPANY_TYPE.ToString();
                    ObjStatus.Option = 501;
                    Utility.FillDLL(ddlCompanyType, objTDSMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

                    ObjStatus.Option = 2;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    Utility.FillDLL(ddlConstitutionName, objTDSMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

                    ObjStatus.Option = 503;
                    ObjStatus.Param1 = "Residential_Status";
                    Utility.FillDLL(ddlResidentialStatus, objTDSMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

                    Dictionary<string, string> dictParam = null;
                    dictParam = new Dictionary<string, string>();
                    dictParam.Add("@Is_Active", "1");
                    dictParam.Add("@LOB_ID", "3");
                    dictParam.Add("@Company_ID", intCompanyId.ToString());
                    dictParam.Add("@User_ID", intUserId.ToString());
                    DataTable dtAccountCard = Utility.GetDefaultData("S3G_ORG_GETGLACCOUNT", dictParam);
                    ddlGLCode.FillDataTable(dtAccountCard, "Account_Setup_ID", "GLAccountDesc");
                   
                }

                if (intTDSId > 0)
                {
                    bool blnTranExists;
                    FunPubProGetTDSDetails(intCompanyId, intTDSId, out blnTranExists);
                   
                    if (strMode == "M")
                    {
                        FunPriTDSControlStatus(1);
                    }
                    if (strMode == "Q")
                    {
                        FunPriTDSControlStatus(-1);
                    }
                }
                else
                {
                    FunPriTDSControlStatus(0);
                }

            }
        }
        catch (Exception ex)
        {
            cvTDS_Add.IsValid = false;
            cvTDS_Add.ErrorMessage = "Unable to load TDS due to data problem";
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    #endregion

    private void FunPriSetControlSettings()
    {
        rfvCompanyType.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCompanyType;
        rfvConsti.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvConstitutionName;
        rfvValidupto.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvValidupto;
        rfvTaxSection.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvTaxSection;
        rfvTaxDesc.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvTaxDesc;
        rfvGLAccount.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvGLAccount;
        rfvTax.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvTax;
        rfvEffRate.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvEffRate;
        cvTax.ErrorMessage = Resources.LocalizationResources.ORG_CUST_cvTax;
        cvSurcharge.ErrorMessage = Resources.LocalizationResources.ORG_CUST_cvSurcharge;
        cvCess.ErrorMessage = Resources.LocalizationResources.ORG_CUST_cvCess;
    }

    #region Page Events


    protected void txtTaxSection_OnTextChanged(object sender, EventArgs e)
    {
        FunPriLoadHistory(); 
    }

    protected void ddlResidentialStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (hdnRate.Value != "")
        {
            txtEffectiveRate.Text = hdnRate.Value;
        }
        FunPriLoadHistory(); 
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        FunPriSetControlSettings();
        if (txtValidupto.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "TDS Details", "alert('Select valid date range')');", true);
            return;
        }

        ClnReceiptMgtServicesReference.ClnReceiptMgtServicesClient objReceiptProcessingClient = new ClnReceiptMgtServicesReference.ClnReceiptMgtServicesClient();
        try
        {
            DataTable dtRecDocDate = objReceiptProcessingClient.FunPubCheckDocDate("S3G_CLN_CHECKRECEIPTDOCDATE", intCompanyId, Utility.StringToDate(txtValidupto.Text), "");

            if (dtRecDocDate != null)
            {
                if (dtRecDocDate.Rows.Count > 0)
                {
                    if (dtRecDocDate.Rows[0][0] == DBNull.Value)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Define the Month in MonthEnd Parameters");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        return;
                    }
                    else if (dtRecDocDate.Rows[0][0].ToString() == "True")
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Effective Date Should be Open Month Date");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            cvTDS_Add.ErrorMessage = "Enter a valid date format";
            cvTDS_Add.IsValid = false;
            cvTDS_Add.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
        }
       
        
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjTDSMasterDataTable = new ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterDataTable();
            ORG.OrgMasterMgtServices.S3G_ORG_TDS_MasterRow ObjTDSRow;
            ObjTDSRow = ObjTDSMasterDataTable.NewS3G_ORG_TDS_MasterRow();
            ObjTDSRow.Company_Type = Convert.ToInt32(ddlCompanyType.SelectedValue.Trim());
            ObjTDSRow.Tax_ID = Convert.ToInt32(intTDSId);
            ObjTDSRow.Constitution = Convert.ToInt32(ddlConstitutionName.SelectedValue.Trim());
            try
            {
                ObjTDSRow.Effective_From = Utility.StringToDate(txtValidupto.Text.Trim());
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "TDS Details", "alert('Enter a valid date')');", true);
                return;
            }

           
            ObjTDSRow.Tax_Law_Section = txtTaxSection.Text.Trim();
            ObjTDSRow.Tax_Description = txtTaxDescription.Text.Trim();
            ObjTDSRow.GL_Account = Convert.ToInt32(ddlGLCode.SelectedValue.Trim());
            if (ddlSLCode.SelectedValue != "0" || ddlSLCode.SelectedValue != "" || ddlSLCode.SelectedValue != "-1")
            {
                ObjTDSRow.SL_Account = Convert.ToInt32(ddlSLCode.SelectedValue.Trim());
            }
            else
            {
                ObjTDSRow.SL_Account = 0;
            }
            if (txtSurcharge.Text.Trim() != "")
            {
                ObjTDSRow.Surcharge = Convert.ToDecimal(txtSurcharge.Text.Trim());
            }
            else
            {
                ObjTDSRow.Surcharge = 0;
            }
            if (txtCess.Text.Trim() != "")
            {
                ObjTDSRow.Cess = Convert.ToDecimal(txtCess.Text.Trim());
            }
            else
            {
                ObjTDSRow.Cess = 0;
            }
            if (txtEdCess.Text.Trim() != "")
            {
                ObjTDSRow.EdCess = txtEdCess.Text.Trim();
            }
            else
            {
                ObjTDSRow.EdCess = "0";
            }
            ObjTDSRow.Tax = Convert.ToDecimal(txtTax.Text.Trim());
            if (hdnRate.Value != "")
            {
                txtEffectiveRate.Text = hdnRate.Value;
            }
            ObjTDSRow.RatePercentage = Convert.ToDecimal(txtEffectiveRate.Text.Trim());
            ObjTDSRow.Threshold_Level = Convert.ToDecimal(txtThresholdLimit.Text.Trim());
            ObjTDSRow.Company_ID = Convert.ToInt32(intCompanyId);
            ObjTDSRow.Res_Status = ddlResidentialStatus.SelectedValue;
            ObjTDSRow.PAN_Applicability = drpPANAppl.SelectedValue;
            if (chkActive.Checked == true)
            {
                ObjTDSRow.Is_active = true;
            }
            else
            {
                ObjTDSRow.Is_active = false;
            }
            if (chkGrossUp.Checked == true)
            {
                ObjTDSRow.Gross_up = true;
            }
            else
            {
                ObjTDSRow.Gross_up = false;
            }
            ObjTDSRow.Created_By = Convert.ToInt32(intUserId);
            S3GSession ObjS3GSession = new S3GSession();
            ObjTDSMasterDataTable.AddS3G_ORG_TDS_MasterRow(ObjTDSRow);

            if (ObjTDSMasterDataTable.Rows.Count > 0)
            {
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_ORG_TDS_DataTable = ClsPubSerialize.Serialize(ObjTDSMasterDataTable, SerMode);

                if (intTDSId > 0)
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubModifyTDSInt(SerMode, byteobjS3G_ORG_TDS_DataTable);
                }
                else
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateTDSInt(out strTDSCode, SerMode, byteobjS3G_ORG_TDS_DataTable);
                }

                switch (intErrorCode)
                {
                    case 0:
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        
                        if (intTDSId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "TDS details updated successfully");
                        }
                        else
                        {
                            txtTaxCode.Text = strTDSCode;
                            strAlert = "TDS code " + strTDSCode + " added successfully";
                            strAlert += @"\n\nWould you like to add one more TDS Record?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                        }
                        break;
                    case 1:
                        strAlert = strAlert.Replace("__ALERT__", "Service Tax Reg. No. already exists, Enter a new Service Tax Reg. No. Account Number");
                        strRedirectPageView = "";
                        break;
                    case 5:
                        strAlert = strAlert.Replace("__ALERT__", "Selected Combination already exists, Enter a different Combination");
                        strRedirectPageView = "";
                        break;
                    case -1:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        strRedirectPageView = "";
                        break;
                    case -2:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        strRedirectPageView = "";
                        break;
                    case -3:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                        strRedirectPageView = "";
                        break;
                    default:
                        if (intTDSId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating TDS details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding TDS details");
                        }
                        strRedirectPageView = "";
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            // if (ObjOrgMasterMgtServicesClient != null)
            ObjOrgMasterMgtServicesClient.Close();

        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtTaxCode.Text = "";
            ddlCompanyType.SelectedIndex = 0;
            ddlConstitutionName.SelectedIndex = 0;
            txtValidupto.Text = "";
            chkActive.Checked = false;
            txtTaxSection.Text = "";
            txtTaxDescription.Text = "";
            ddlGLCode.SelectedIndex = 0;
            ddlSLCode.SelectedIndex = 0;
            txtTax.Text = "";
            txtThresholdLimit.Text = "";
            txtSurcharge.Text = "";
            txtCess.Text = "";
            txtEdCess.Text = "";
            drpPANAppl.SelectedIndex = 0;
            txtEffectiveRate.Text = "";
            chkGrossUp.Checked = false;
            gvHistory.DataSource = null;
            gvHistory.DataBind();
            gvHistory.Visible = false;
            ViewState["BillingAddress"] = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvTDS_Add.IsValid = false;
            cvTDS_Add.ErrorMessage = "Unable to clear data";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3gOrgTDSMaster_View.aspx");
    }
    #endregion

    #region Page Methods

    protected void ddlCompanyType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadHistory();
    }

    private void FunPriLoadHistory()
    {

        DataSet dsGetHistory = new DataSet();
        if (ddlCompanyType.SelectedValue != "0" || ddlCompanyType.SelectedValue != "")
        {
            Dictionary<string, string> Procparam = null;
            Procparam = new Dictionary<string, string>();
            if (intTDSId > 0)
            {
                Procparam.Add("@TDS_ID", Convert.ToString(intTDSId));
            }
            else
            {
                Procparam.Add("@TDS_ID", "100");
            }
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Company_Type", ddlCompanyType.SelectedValue);
            Procparam.Add("@Tax_Section", txtTaxSection.Text.Trim());
            Procparam.Add("@Res_Status_ID", ddlResidentialStatus.SelectedValue);
            dsGetHistory = Utility.GetDataset("S3G_ORG_GetCompanyHistory_TDS", Procparam);
            gvHistory.DataSource = dsGetHistory;
            gvHistory.DataBind();
        }
        else
        {
            gvHistory.DataSource = "";
            gvHistory.DataBind();
        }
        gvHistory.Visible = true;
       
    }
    protected void ddlGLCode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriSetSLCodeList(ddlGLCode, ddlSLCode);
    }

    private void FunPriSetSLCodeList(DropDownList ddlGLCode, DropDownList ddlSLCode)
    {
        try
        {
            if (ddlGLCode.SelectedValue != "0")
            {
                Dictionary<string, string> dictParam = null;
                dictParam = new Dictionary<string, string>();
                if (PageMode != PageModes.Query)
                {
                    dictParam.Add("@Is_Active", "1");
                }
                dictParam.Add("@LOB_ID", "3");
                dictParam.Add("@Company_ID", intCompanyId.ToString());
                dictParam.Add("@User_ID", intUserId.ToString());
                dictParam.Add("@GL_Code", ddlGLCode.SelectedValue);
                ddlSLCode.BindDataTable("S3G_ORG_GETGLACCOUNT", dictParam, new string[] { "Account_Setup_ID", "SL_Account_Code" });
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, "TDS Master");
            throw ex;
        }
    }

    protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblGrossUp = (Label)e.Row.FindControl("lblGrossUp");
                Label lblActive = (Label)e.Row.FindControl("lblActive");
                CheckBox chkgGrossUp = (CheckBox)e.Row.FindControl("chkgGrossUp");
                CheckBox chkgActive = (CheckBox)e.Row.FindControl("chkgActive");
                Label lblEffDate = (Label)e.Row.FindControl("lblEffDate");
                if (lblEffDate.Text != "")
                {
                    lblEffDate.Text = DateTime.Parse(lblEffDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString("dd-MMM-yyyy");
                }
                if (lblGrossUp.Text == "1" || lblGrossUp.Text == "True")
                {
                    chkgGrossUp.Checked = true;
                }
                else
                {
                    chkgGrossUp.Checked = false;
                }
                if (lblActive.Text == "1" || lblActive.Text == "True")
                {
                    chkgActive.Checked = true;
                }
                else
                {
                    chkgActive.Checked = false;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "S3GORGENTITYMASTER.ASPX");
            throw new ApplicationException("Unable to Load the Bank Details");
        }
    }

    public void FunPubProGetTDSDetails(int intCompanyId, int intTDSId, out bool blnTranExists)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            DataSet dsTDSDetails;
            //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            byte[] byte_TDSDetails = ObjOrgMasterMgtServicesClient.FunPubQueryTDSDetails(out blnTranExists, intCompanyId, intTDSId);
            dsTDSDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_TDSDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsTDSDetails.Tables.Count > 0 && intCompanyId > 0)
            {
                DataTable dtTDSCode = dsTDSDetails.Tables[0];
                txtTaxCode.Text = dtTDSCode.Rows[0]["Tax_Code"].ToString();
                ListItem lst;
                if (PageMode == PageModes.Query)
                {
                    lst = new ListItem(dtTDSCode.Rows[0]["COMPNAME"].ToString(), dtTDSCode.Rows[0]["COMPID"].ToString());
                    ddlCompanyType.Items.Add(lst);

                    lst = new ListItem(dtTDSCode.Rows[0]["RESNAME"].ToString(), dtTDSCode.Rows[0]["RESID"].ToString());
                    ddlResidentialStatus.Items.Add(lst);

                    lst = new ListItem(dtTDSCode.Rows[0]["ConstitutionName"].ToString(), dtTDSCode.Rows[0]["Constitution"].ToString());
                    ddlConstitutionName.Items.Add(lst);

                    lst = new ListItem(dtTDSCode.Rows[0]["GLAccountDesc"].ToString(), dtTDSCode.Rows[0]["Account_Setup_ID"].ToString());
                    ddlGLCode.Items.Add(lst);
                }
                ddlResidentialStatus.SelectedValue = dtTDSCode.Rows[0]["RESID"].ToString();
                ddlCompanyType.SelectedValue = dtTDSCode.Rows[0]["COMPID"].ToString();
                ddlConstitutionName.SelectedValue = dtTDSCode.Rows[0]["Constitution"].ToString();
                ddlGLCode.SelectedValue = dtTDSCode.Rows[0]["GL_Account"].ToString();
                FunPriSetSLCodeList(ddlGLCode, ddlSLCode);
                ddlSLCode.SelectedValue = dtTDSCode.Rows[0]["SL_Account"].ToString();
               
                txtValidupto.Text = dtTDSCode.Rows[0]["Effective_From"].ToString();
                if (txtValidupto.Text != "")
                {
                    txtValidupto.Text = DateTime.Parse(txtValidupto.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                if (dtTDSCode.Rows[0]["Is_active"].ToString() == "True")
                {
                    chkActive.Checked = true;
                }
                else
                {
                    chkActive.Checked = false;
                }

                txtTaxSection.Text = dtTDSCode.Rows[0]["Tax_Law_Section"].ToString();
                txtTaxDescription.Text = dtTDSCode.Rows[0]["Tax_Description"].ToString();
                txtTax.Text = dtTDSCode.Rows[0]["Tax"].ToString();
                txtSurcharge.Text = dtTDSCode.Rows[0]["Surcharge"].ToString();
                txtThresholdLimit.Text = dtTDSCode.Rows[0]["Threshold_Level"].ToString();
                txtCess.Text = dtTDSCode.Rows[0]["Cess"].ToString();
                txtEdCess.Text = dtTDSCode.Rows[0]["EdCess"].ToString();
                txtEffectiveRate.Text = dtTDSCode.Rows[0]["RatePercentage"].ToString();
                drpPANAppl.SelectedValue = dtTDSCode.Rows[0]["PAN_Applicability"].ToString();
                FunPriLoadHistory();
                if (dtTDSCode.Rows[0]["Gross_up"].ToString() == "True")
                {
                    chkGrossUp.Checked = true;
                }
                else
                {
                    chkGrossUp.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load TDS details");
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();

        }
    }


    private void FunPriTDSControlStatus(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                btnClear.Enabled = true;
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                ddlGLCode.Enabled = true;
                ddlConstitutionName.Enabled = true;
                ddlCompanyType.Enabled = true;
                ddlSLCode.Enabled = true;
                break;

            case 1: //Modify
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
               
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                ddlGLCode.Enabled = true;
                ddlConstitutionName.Enabled = true;
                ddlCompanyType.Enabled = true;
                ddlSLCode.Enabled = true;
                break;
            case -1://Query
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                txtValidupto.ReadOnly = true;
                CalendarValidupto.Enabled = false;
                chkActive.Enabled = false;
                txtTaxSection.ReadOnly = true;
                txtTaxDescription.ReadOnly = true;
                txtTax.ReadOnly = true;
                txtThresholdLimit.ReadOnly = true;
                txtSurcharge.ReadOnly = true;
                txtCess.ReadOnly = true;
                txtEdCess.ReadOnly = true;
                chkGrossUp.Enabled = false;
                txtEffectiveRate.ReadOnly = true;
                gvHistory.Enabled = false;
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                ddlResidentialStatus.RemoveDropDownList();
                ddlCompanyType.RemoveDropDownList();
                ddlConstitutionName.RemoveDropDownList();
                ddlGLCode.RemoveDropDownList();
                ddlSLCode.RemoveDropDownList();
                drpPANAppl.RemoveDropDownList();
                break;
        }
    }

    #endregion
}
