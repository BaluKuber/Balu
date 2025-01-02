#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Legal & Repossession
/// Screen Name			: Garage Master
/// Created By			: Srivatsan S
/// Created Date		: 21-April-2011
/// Purpose	            : This module is used to store and retrieve Garage details
///<Program Summary>
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
using LEGAL = S3GBusEntity.LegalRepossession;
using LEGALSERVICES = LegalAndRepossessionMgtServicesReference;
using System.Collections;
using System.Configuration;
#endregion

public partial class LegalRepossession_S3GLRGarageMaster_Add : ApplyThemeForProject
{

    #region Initialization
    enum AccountMode { ALL, CURRENT };
        
    int intCompanyId = 0;
    int intUserId = 0;
    int intGarageId = 0;
    int intErrorCode = 0;
    int intGarageOwnerID = 0;
    string strGarageCode=string.Empty;
    string strGarageOwnerCode = string.Empty;
    string strErrorMessage=string.Empty;
    bool blnTranExists = false;
    Dictionary<string, string> Procparam = null;
    LEGAL.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterDataTable ObjGarageMasterDataTable;
    LEGALSERVICES.LegalAndRepossessionMgtServicesClient ObjLegalMgtServicesClient;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    UserInfo ObjUserInfo;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRGarageMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRGarageMaster_Add.aspx';";
    string strRedirectPage = "~/LegalRepossession/S3GLRGarageMaster_Add.aspx";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    S3GSession ObjS3GSession;
    //Code end
    #endregion
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ObjS3GSession = new S3GSession();
             ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            //Code end

            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString["qsEntityId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsEntityId"));
                if (fromTicket != null)
                {
                    intGarageId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Garage Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
            //txtBox.Attributes.Add("onblur", "ChkIsZero(this)");
            this.TxtSqFeet.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, false, "Square Feet");
            this.TxtRntSqFeet.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, false, "Rent Per Square Feet");
            //this.TxtTotRent.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, false, "Rent Per Square Feet");
            this.TxtPlcyAmnt.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, false, "Policy Amount");

            this.TxtRntSqFeet.Attributes.Add("onkeyup", "CalcTotrent()");
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (!IsPostBack)
            {
                Session["Garage_Code"] = "";
                //FunProIntializeData();
                tcGarageMaster.ActiveTab = tpContact;
                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                ddlGarageOwnerCode.BindDataTable("S3G_LR_GetOwnerEntity", Procparam, new string[] { "Entity_ID", "Entity_Code" });
                Procparam.Clear();
                Procparam.Add("@LookupType_Code", "2");
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                ddlPymntFreq.BindDataTable(SPNames.S3G_LR_GetLookupTypeDescription, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
                //S3GSession ObjS3GSession = new S3GSession();
                if (ObjS3GSession.ProPINCodeDigitsRW > 0)
                {
                    TxtGrgPINCode.MaxLength = ObjS3GSession.ProPINCodeDigitsRW;
                    TxtCntctPINCode.MaxLength = ObjS3GSession.ProPINCodeDigitsRW;
                    if (ObjS3GSession.ProPINCodeTypeRW.ToUpper() == "ALPHA NUMERIC")
                    {
                        FTxtGrgPINCode.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                        TxtCntctPINCode_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                    }
                    else
                    {
                        FTxtGrgPINCode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                        TxtCntctPINCode_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                    }
                }

                if (intGarageId > 0)
                {
                    bool blnTranExists;
                    FunPubProGetGarageDetails(intCompanyId, intGarageId);
                    
                    if (strMode == "M")
                    {
                        FunPriEntityControlStatus(1);
                       
                    }
                    if (strMode == "Q")
                    {
                        FunPriEntityControlStatus(-1);
                    }
                }
                else
                {
                    FunPriEntityControlStatus(0);
                    //TxtGrgName.Focus();
                }

            }
        }
        catch (Exception ex)
        {
            cvGarageMaster.IsValid = false;
            cvGarageMaster.ErrorMessage = "Unable to load Entity due to data problem";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    #endregion
    public void FunPriBindGrid(int Mode)
    {
        LEGALSERVICES.LegalAndRepossessionMgtServicesClient ObjLegalMgtServicesClient = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {
            DataSet dsAccouontDetails;
           
            byte[] byte_AccountDetails = ObjLegalMgtServicesClient.FunGetAccount(intCompanyId, intGarageId, Mode);
            dsAccouontDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_AccountDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsAccouontDetails.Tables[0].Rows.Count != 0)
            {
                grvAccDtls.DataSource = dsAccouontDetails.Tables[0];
                grvAccDtls.DataBind();
            }
            else
            {
                grvAccDtls.EmptyDataText = "No Records Found!";
                grvAccDtls.DataBind();
            }
        }
        catch (Exception ex)
                
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Entity details");
        }
        
        finally
        {
            //if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }
    }
    protected void RdbInsCvr_SelectedIndexChanged(object sender, EventArgs e)
    
    {
        if (RdbInsCvr.SelectedIndex == 0)
        {
            TxtInsAddress1.Enabled = true;
            TxtInsAddress2.Enabled = true;
            TxtInsCity.Enabled = true;
            TxtInsCompany.Enabled = true;
            TxtInsCountry.Enabled = true;
            TxtInsEmailId.Enabled = true;
            TxtInsMobile.Enabled = true;
            TxtInsPINCode.Enabled = true;
            TxtInsState.Enabled = true;
            TxtInsTelephone.Enabled = true;
            TxtInsWebsite.Enabled = true;
            TxtPlcyAmnt.Enabled = true;
            TxtPolicyNo.Enabled = true;
            RqvTxtInsCompany.Enabled = true;
            RqvTxtPolicyNo.Enabled = true;
            rfvInsAddress1.Enabled = true;
            rfvInsEmailId.Enabled = true;
            rfvtxtInsCity.Enabled = true;
            rfvtxtInsCountry.Enabled = true;
            rfvtxtInsPINCode.Enabled = true;
            rfvtxtInsState.Enabled = true;
            rfvtxtInsTelephone.Enabled = true;
            revinsWebsite.Enabled = true;
            RqvPolicyAmnt.Enabled = true;
            TxtInsCompany.Focus();
        }
        else
        {

            TxtInsAddress1.Text ="";
            TxtInsAddress2.Text = "";
            TxtInsCity.Text = "";
            TxtInsCompany.Text = "";
            TxtInsCountry.Text = "";
            TxtInsEmailId.Text = "";
            TxtInsMobile.Text = "";
            TxtInsPINCode.Text = "";
            TxtInsState.Text = "";
            TxtInsTelephone.Text = "";
            TxtInsWebsite.Text = "";
            TxtPlcyAmnt.Text = "";
            TxtPolicyNo.Text = "";

            TxtInsAddress1.Enabled = false;
            TxtInsAddress2.Enabled = false;
            TxtInsCity.Enabled = false;
            TxtInsCompany.Enabled = false;
            TxtInsCountry.Enabled = false;
            TxtInsEmailId.Enabled = false;
            TxtInsMobile.Enabled = false;
            TxtInsPINCode.Enabled = false;
            TxtInsState.Enabled = false;
            TxtInsTelephone.Enabled = false;
            TxtInsWebsite.Enabled = false;
            TxtPlcyAmnt.Enabled = false;
            TxtPolicyNo.Enabled = false;
            RqvTxtInsCompany.Enabled = false;
            RqvTxtPolicyNo.Enabled = false;
            rfvInsAddress1.Enabled = false;
            rfvInsEmailId.Enabled = false;
            rfvtxtInsCity.Enabled = false;
            rfvtxtInsCountry.Enabled = false;
            rfvtxtInsPINCode.Enabled = false;
            rfvtxtInsState.Enabled = false;
            rfvtxtInsTelephone.Enabled = false;
            revinsWebsite.Enabled = false;
            RqvPolicyAmnt.Enabled = false;
        }

    }
    public void FunPubProGetGarageDetails(int intCompanyId, int intGarageId)
    {
        ObjLegalMgtServicesClient = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {
            DataSet dsGarageDetails;
            
            byte[] byte_GarageDetails = ObjLegalMgtServicesClient.FunGetGarageDetailsMod(intCompanyId, intGarageId);
            dsGarageDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_GarageDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsGarageDetails.Tables.Count > 0 && intCompanyId > 0)
            {
                DataTable dtGarageCode = dsGarageDetails.Tables[0];
                Session["Garage_Code"] = dtGarageCode.Rows[0]["Garage_Code"].ToString();
                TxtGrgCode.Text = dtGarageCode.Rows[0]["Garage_Code"].ToString();
                //TxtGrgName.Text = dtGarageCode.Rows[0]["Garage_Name"].ToString(); 
                ddlGarageOwnerCode.Items.FindByValue(dtGarageCode.Rows[0]["Garage_Owner_ID"].ToString()).Selected=true;
                ddlGarageOwnerCode_SelectedIndexChanged(this, new EventArgs());
                ddlPymntFreq.Items.FindByValue(dtGarageCode.Rows[0]["Frequency_Of_Payment_Type"].ToString()).Selected=true;
                
                //Filling Garage Owner Details
                //TxtGrgOwnerName.Text = dtGarageCode.Rows[0]["Garage_Owner_ID"].ToString();
                TxtGrgAddress1.Text = dtGarageCode.Rows[0]["Contact_Person_Address1"].ToString();
                TxtGrgAddress2.Text = dtGarageCode.Rows[0]["Contact_Person_Address2"].ToString();
                TxtGrgCity.Text = dtGarageCode.Rows[0]["Contact_Person_City"].ToString();
                TxtGrgCountry.Text = dtGarageCode.Rows[0]["Contact_Person_Country"].ToString();
                TxtGrgCpcty.Text = dtGarageCode.Rows[0]["No_Of_Assets_Garage"].ToString();
                TxtGrgEmailId.Text = dtGarageCode.Rows[0]["Contact_Person_Email_ID"].ToString();
                TxtGrgMobile.Text = dtGarageCode.Rows[0]["Contact_Person_Mobile"].ToString() == "0" ? "" : dtGarageCode.Rows[0]["Contact_Person_Mobile"].ToString();
                TxtGrgPINCode.Text = dtGarageCode.Rows[0]["Contact_Person_Pin"].ToString();
                TxtGrgState.Text = dtGarageCode.Rows[0]["Contact_Person_State"].ToString();
                TxtGrgTelephone.Text = dtGarageCode.Rows[0]["Contact_Person_Telephone"].ToString();
                TxtGrgWebsite.Text = dtGarageCode.Rows[0]["Contact_Person_Web_Site"].ToString();
                
                //Filling Contact Person Details
                TxtCntctAddress1.Text = dtGarageCode.Rows[0]["Garage_Address1"].ToString();
                TxtCntctAddress2.Text = dtGarageCode.Rows[0]["Garage_Address2"].ToString();
                TxtCntctCity.Text = dtGarageCode.Rows[0]["Garage_City"].ToString();
                TxtCntctCountry.Text = dtGarageCode.Rows[0]["Garage_Country"].ToString();
                TxtCntctEmailId.Text = dtGarageCode.Rows[0]["Garage_Email_ID"].ToString();
                TxtCntctMobile.Text = dtGarageCode.Rows[0]["Garage_Mobile"].ToString() == "0" ? "" : dtGarageCode.Rows[0]["Garage_Mobile"].ToString();
                TxtCntctName.Text = dtGarageCode.Rows[0]["Garage_Name"].ToString();
                TxtCntctPINCode.Text = dtGarageCode.Rows[0]["Garage_Pin"].ToString();
                TxtCntctState.Text = dtGarageCode.Rows[0]["Garage_State"].ToString();
                TxtCntctTelephone.Text = dtGarageCode.Rows[0]["Garage_Telephone"].ToString();
                TxtCntctWebsite.Text = dtGarageCode.Rows[0]["Garage_Web_Site"].ToString();
                ChkCntctAddress.Checked = Convert.ToBoolean(dtGarageCode.Rows[0]["Contact_Address_Same_As_Owners"]);

                //Filling Insurance Details
                TxtInsAddress1.Text = dtGarageCode.Rows[0]["Insurance_Company_Address1"].ToString();
                TxtInsAddress2.Text = dtGarageCode.Rows[0]["Insurance_Company_Address2"].ToString();
                TxtInsCity.Text = dtGarageCode.Rows[0]["Insurance_Company_City"].ToString();
                TxtInsCompany.Text = dtGarageCode.Rows[0]["Insurance_Company"].ToString();
                TxtInsCountry.Text = dtGarageCode.Rows[0]["Insurance_Company_Country"].ToString();
                TxtInsEmailId.Text = dtGarageCode.Rows[0]["Insurance_Company_Email_ID"].ToString();
                TxtInsMobile.Text = dtGarageCode.Rows[0]["Insurance_Company_Mobile"].ToString() == "0" ? "" : dtGarageCode.Rows[0]["Insurance_Company_Mobile"].ToString();
                TxtInsPINCode.Text = dtGarageCode.Rows[0]["Insurance_Company_Pin"].ToString();
                TxtInsState.Text = dtGarageCode.Rows[0]["Insurance_Company_State"].ToString();
                TxtInsTelephone.Text = dtGarageCode.Rows[0]["Insurance_Company_Telephone"].ToString();
                TxtInsWebsite.Text = dtGarageCode.Rows[0]["Insurance_Company_Web_Site"].ToString();
                
                //Filling Insurance and Policy details
                if (dtGarageCode.Rows[0]["Policy_Amount"].ToString() == "0.0000" || dtGarageCode.Rows[0]["Policy_Amount"].ToString() == "0")
                {
                    TxtPlcyAmnt.Text = "";
                }
                else 
                {
                    TxtPlcyAmnt.Text = ReturnFormattedDecimalValue(dtGarageCode.Rows[0]["Policy_Amount"].ToString());
                }
                 TxtPolicyNo.Text = dtGarageCode.Rows[0]["Policy_No"].ToString();
                TxtRemarks.Text = dtGarageCode.Rows[0]["Remarks"].ToString();

                //Filling GarageDetails
                TxtRntSqFeet.Text =ReturnFormattedDecimalValue(dtGarageCode.Rows[0]["Rent_Per_SqFeet"].ToString());
                TxtSqFeet.Text =ReturnFormattedDecimalValue(dtGarageCode.Rows[0]["Square_Feet"].ToString());
                decimal Rent_Per_SqFeet= Convert.ToDecimal(dtGarageCode.Rows[0]["Rent_Per_SqFeet"]);
                decimal Square_Feet= Convert.ToDecimal(dtGarageCode.Rows[0]["Square_Feet"]);
                TxtTotRent.Text =ReturnFormattedDecimalValue(Convert.ToString(Rent_Per_SqFeet* Square_Feet));
                txthiddenfield.Text = dtGarageCode.Rows[0]["Created_By"].ToString();
                RdbCvrPrk.ClearSelection();
                RdbCvrPrk.Items.FindByValue(dtGarageCode.Rows[0]["Is_Covered_Parking"].ToString()).Selected = true;
                RdbInsCvr.ClearSelection();
                RdbInsCvr.Items.FindByValue(dtGarageCode.Rows[0]["Is_Insurance_Cover"].ToString()).Selected=true;
                RdbInsCvr_SelectedIndexChanged(this, new EventArgs());
                ChkIsActive.Checked = Convert.ToBoolean(dtGarageCode.Rows[0]["Is_Active"].ToString());
                Session["GarageID"] = intGarageId;
            }
            FunPriBindGrid(Convert.ToInt32(AccountMode.CURRENT));
         }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Entity details");
        }
        finally
        {
            //if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }
    }

    public string ReturnFormattedDecimalValue(string DecimalVal)
    {
        bool IsGreater = false;
        string[] digits = new string[2];
        digits = DecimalVal.Split('.');
        if (Convert.ToInt32(digits[1].ToString()) > 0)
        {
            IsGreater = true;
        }

        if (IsGreater == true)
        {
            return DecimalVal;
        }
        else
        {
            return digits[0];
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjLegalMgtServicesClient = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {  
            ObjGarageMasterDataTable = new LEGAL.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterDataTable();
            LEGAL.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterRow ObjGarageDetailRow;
            ObjGarageDetailRow = ObjGarageMasterDataTable.NewS3G_LEGAL_Garage_MasterRow();

            //Garage ID and garage Owner Code...............
            ObjGarageDetailRow.Garage_ID = intGarageId;
            ObjGarageDetailRow.Garage_Code = Session["Garage_Code"].ToString();
            ObjGarageDetailRow.Garage_Owner_Code = TxtGrgOwnerName.Text.ToString().Trim();
            ObjGarageDetailRow.GL_Posting_Code = Convert.ToInt32(Session["GLCODE"].ToString());
            ObjGarageDetailRow.Garage_Name = TxtCntctName.Text.Trim();
            ObjGarageDetailRow.Garage_Owner_ID = Convert.ToInt32(ddlGarageOwnerCode.SelectedValue);
            ObjGarageDetailRow.Company_ID = intCompanyId;
            ObjGarageDetailRow.BranchID = 0;

            //Garage Contact Details................
            ObjGarageDetailRow.Garage_Address1 = TxtCntctAddress1.Text.Trim();
            ObjGarageDetailRow.Garage_Address2 = TxtCntctAddress2.Text.Trim();
            ObjGarageDetailRow.Garage_City = TxtCntctCity.Text.Trim();
            ObjGarageDetailRow.Garage_Country = TxtCntctCountry.Text.Trim();
            ObjGarageDetailRow.Garage_Email_ID = TxtCntctEmailId.Text.Trim();
            ObjGarageDetailRow.Garage_Mobile = TxtCntctMobile.Text.Trim() == string.Empty ? 0 : Convert.ToInt64(TxtCntctMobile.Text.Trim());
            ObjGarageDetailRow.Garage_Pin = TxtCntctPINCode.Text.Trim();
            ObjGarageDetailRow.Garage_State = TxtCntctState.Text.Trim();
            ObjGarageDetailRow.Garage_Telephone = TxtCntctTelephone.Text.Trim();
            ObjGarageDetailRow.Garage_Web_Site = TxtCntctWebsite.Text.Trim();

            //Contact Person Details..................

            ObjGarageDetailRow.Contact_Person_Name = "";
            ObjGarageDetailRow.Contact_Person_Address1 = TxtGrgAddress1.Text.Trim();
            ObjGarageDetailRow.Contact_Person_Address2 = TxtGrgAddress2.Text.Trim();
            ObjGarageDetailRow.Contact_Person_City = TxtGrgCity.Text.Trim();
            ObjGarageDetailRow.Contact_Person_Country = TxtGrgCountry.Text.Trim();
            ObjGarageDetailRow.Contact_Person_Email_ID = TxtGrgEmailId.Text.Trim();
            ObjGarageDetailRow.Contact_Person_Mobile = TxtGrgMobile.Text.Trim() == string.Empty ? 0 : Convert.ToInt64(TxtGrgMobile.Text.Trim());
            ObjGarageDetailRow.Contact_Person_Pin = TxtGrgPINCode.Text.Trim();
            ObjGarageDetailRow.Contact_Person_State = TxtGrgState.Text.Trim();
            ObjGarageDetailRow.Contact_Person_Telephone = TxtGrgTelephone.Text.Trim();
            ObjGarageDetailRow.Contact_Person_Web_Site = TxtGrgWebsite.Text.Trim();
            ObjGarageDetailRow.Contact_Address_Same_As_Owners = Convert.ToBoolean(ChkCntctAddress.Checked);

            //Garage Details

            ObjGarageDetailRow.Square_Feet = Convert.ToDecimal(TxtSqFeet.Text.Trim());
            ObjGarageDetailRow.Rent_Per_SqFeet = Convert.ToDecimal(TxtRntSqFeet.Text.Trim());
            ObjGarageDetailRow.Frequency_Of_Payment_Type = Convert.ToInt32(ddlPymntFreq.SelectedValue);
            ObjGarageDetailRow.Frequency_Of_Payment_Type_Code = 2;
            ObjGarageDetailRow.No_Of_Assets_Garage = Convert.ToInt32(TxtGrgCpcty.Text.Trim());
            ObjGarageDetailRow.Is_Covered_Parking = Convert.ToBoolean(RdbCvrPrk.SelectedValue);

            //Garage Insurance Details.............

            ObjGarageDetailRow.Is_Insurance_Cover = Convert.ToBoolean(RdbInsCvr.SelectedValue);
            ObjGarageDetailRow.Insurance_Company = TxtInsCompany.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_Address1 = TxtInsAddress1.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_Address2 = TxtInsAddress2.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_City = TxtInsCity.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_Country = TxtInsCountry.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_Email_ID = TxtInsEmailId.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_Mobile = TxtInsMobile.Text.Trim() == string.Empty ? 0 : Convert.ToInt64(TxtInsMobile.Text.Trim());
            ObjGarageDetailRow.Insurance_Company_Pin = TxtInsPINCode.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_State = TxtInsState.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_Telephone = TxtInsTelephone.Text.Trim();
            ObjGarageDetailRow.Insurance_Company_Web_Site = TxtInsWebsite.Text.Trim();
            ObjGarageDetailRow.Policy_Amount = TxtPlcyAmnt.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(TxtPlcyAmnt.Text.Trim());
            ObjGarageDetailRow.Policy_No = TxtPolicyNo.Text.Trim();
            ObjGarageDetailRow.Remarks = TxtRemarks.Text.Trim();
            ObjGarageDetailRow.Created_By = intUserId;
            ObjGarageDetailRow.Modified_By = intUserId;
            ObjGarageDetailRow.Is_Active = ChkIsActive.Checked == true ? true : false;


            ObjGarageMasterDataTable.AddS3G_LEGAL_Garage_MasterRow(ObjGarageDetailRow);

            if (ObjGarageMasterDataTable.Rows.Count > 0)
            {
               
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_LEGAL_Garage_DataTable = ClsPubSerialize.Serialize(ObjGarageMasterDataTable, SerMode);

                //if (intGarageId > 0)
                //{
                //    intErrorCode = ObjLegalMgtServicesClient.FunPubModifyEntityInt(SerMode, byteobjS3G_LEGAL_Garage_DataTable);
                //}
                //else
                //{
                //    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateEntityInt(out strGarageCode, SerMode, byteobjS3G_LEGAL_Garage_DataTable);
                //}
                intErrorCode = ObjLegalMgtServicesClient.FunPubCreateGarageMaster(out strErrorMessage, SerMode, byteobjS3G_LEGAL_Garage_DataTable);

                switch (intErrorCode)
                {
                    case 0:
                        TxtGrgCode.Text = strGarageCode = strErrorMessage;

                        if (intGarageId > 0)
                        {
                            //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = strAlert.Replace("__ALERT__", "Garage details updated sucessfully");
                        }
                        else
                        {
                            //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = "Garage code " + strGarageCode + " added successfully";
                            strAlert += @"\n\nWould you like to add one more Entity?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                        }
                        break;
                    case 1:
                        //strAlert = strAlert.Replace("__ALERT__", "Tax Account Number already exists, Enter a new Tax Account Number");
                        //strRedirectPageView = "";
                        break;
                    case -1:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        strRedirectPageView = "";
                        break;
                    case -2:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                        strRedirectPageView = "";
                        break;
                    default:
                        if (intGarageId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Garage details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Garage details");
                        }
                        strRedirectPageView = "";
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, "Unable to save Data");
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            //if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();

        }
    }
    public void FunPubClearGarageDetails()
    {
        ChkCntctAddress.Checked = false;
        TxtGrgOwnerName.Text = "";
        TxtGrgAddress1.Text = "";
        TxtGrgAddress2.Text = "";
        TxtGrgCity.Text = "";
        TxtGrgCountry.Text = "";
        TxtGrgEmailId.Text = "";
        TxtGrgMobile.Text = "";
        TxtGrgPINCode.Text = "";
        TxtGrgState.Text = "";
        TxtGrgTelephone.Text = "";
        TxtGrgWebsite.Text = "";
        TxtCntctName.Text = "";
        TxtCntctAddress1.Text = "";
        TxtCntctAddress2.Text = "";
        TxtCntctCity.Text = "";
        TxtCntctCountry.Text = "";
        TxtCntctEmailId.Text = "";
        TxtCntctMobile.Text = "";
        TxtCntctPINCode.Text = "";
        TxtCntctState.Text = "";
        TxtCntctTelephone.Text = "";
        TxtCntctWebsite.Text = "";
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            
            ddlGarageOwnerCode.SelectedIndex = 0;
            ddlPymntFreq.SelectedIndex = 0;
            RdbCvrPrk.ClearSelection();
            RdbInsCvr.ClearSelection();
            //TxtGrgName.Text = "";
            ChkCntctAddress.Checked = false;
            TxtGrgOwnerName.Text = "";
            TxtGrgAddress1.Text = "";
            TxtGrgAddress2.Text = "";
            TxtGrgCity.Text = "";
            TxtGrgCountry.Text = "";
            TxtGrgEmailId.Text = "";
            TxtGrgMobile.Text = "";
            TxtGrgPINCode.Text = "";
            TxtGrgState.Text = "";
            TxtGrgTelephone.Text = "";
            TxtGrgWebsite.Text = "";
            TxtCntctName.Text = "";
            TxtCntctAddress1.Text = "";
            TxtCntctAddress2.Text = "";
            TxtCntctCity.Text = "";
            TxtCntctCountry.Text = "";
            TxtCntctEmailId.Text = "";
            TxtCntctMobile.Text = "";
            TxtCntctPINCode.Text = "";
            TxtCntctState.Text = "";
            TxtCntctTelephone.Text = "";
            TxtCntctWebsite.Text = "";
            TxtPolicyNo.Text = "";
            TxtPlcyAmnt.Text = "";
            TxtRemarks.Text = "";
            TxtRntSqFeet.Text = "";
            TxtSqFeet.Text = "";
            TxtTotRent.Text = "";
            TxtInsAddress1.Text ="";
            TxtInsAddress2.Text = "";
            TxtInsCity.Text = "";
            TxtInsCompany.Text  ="";
            TxtInsCountry.Text = "";
            TxtInsEmailId.Text = "";
            TxtInsMobile.Text = "";
            TxtInsPINCode.Text = "";
            TxtInsState.Text = "";
            TxtInsTelephone.Text = "";
            TxtInsWebsite.Text = "";
            tcGarageMaster.ActiveTabIndex = 0;
                        
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvGarageMaster.IsValid = false;
            cvGarageMaster.ErrorMessage = "Unable to clear data";
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/LegalRepossession/S3GLRGarageMaster_View.aspx");
    }
    protected void btnProceedSave_Click(object sender, EventArgs e)
    {

    }
    protected void ChkCntctAddress_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkCntctAddress.Checked)
        {
            TxtCntctAddress1.Text = TxtGrgAddress1.Text;
            TxtCntctAddress2.Text = TxtGrgAddress2.Text;
            TxtCntctCity.Text = TxtGrgCity.Text;
            TxtCntctCountry.Text = TxtGrgCountry.Text;
            TxtCntctEmailId.Text = TxtGrgEmailId.Text;
            TxtCntctMobile.Text = TxtGrgMobile.Text;
            TxtCntctPINCode.Text = TxtGrgPINCode.Text;
            TxtCntctState.Text = TxtGrgState.Text;
            TxtCntctTelephone.Text = TxtGrgTelephone.Text;
            TxtCntctWebsite.Text = TxtGrgWebsite.Text;
        }
        else
        {
            TxtCntctAddress1.Text = "";
            TxtCntctAddress2.Text = "";
            TxtCntctCity.Text = "";
            TxtCntctCountry.Text = "";
            TxtCntctEmailId.Text = "";
            TxtCntctMobile.Text = "";
            TxtCntctPINCode.Text = "";
            TxtCntctState.Text = "";
            TxtCntctTelephone.Text = "";
            TxtCntctWebsite.Text = "";

        }
        TxtCntctName.Focus();
    }

    private void FunPriEntityControlStatus(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                //btnModify.Visible = false;
                //btnAdd.Visible = true;
                ChkIsActive.Checked = true;
                ChkIsActive.Enabled = false;
                ChkCntctAddress.Enabled = false;
                PnlAccDtls.Visible = false;
                ddlGarageOwnerCode.Enabled = true;
                RdbInsCvr.SelectedIndex = 1;
                btnClear.Enabled = true;
                btnSave.Enabled = true;
                //vs1.Visible = false;
                RdbInsCvr_SelectedIndexChanged(this, new EventArgs());
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                break;

            case 1: //Modify
                PnlAccDtls.Visible = true;
                ChkIsActive.Enabled = true;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                ddlGarageOwnerCode.Enabled = false;
                //TxtCntctName.ReadOnly = true;
                //TxtCntctAddress1.ReadOnly = true;
                //TxtCntctAddress2.ReadOnly = true;
                //TxtCntctCity.ReadOnly = true;
                //TxtCntctCountry.ReadOnly = true;
                //TxtCntctEmailId.ReadOnly = true;
                //TxtCntctMobile.ReadOnly = true;
                //TxtCntctPINCode.ReadOnly = true;
                //TxtCntctState.ReadOnly = true;
                //TxtCntctTelephone.ReadOnly = true;
                //TxtCntctWebsite.ReadOnly = true;
                RdbInsCvr_SelectedIndexChanged(this, new EventArgs());
                ChkCntctAddress.Visible = false;
                //ddlGarageOwnerCode.Enabled = false;
                //txtCountry.AutoPostBack = true;
                //txtEntityName.Enabled = false;
                // btnModify.Visible = true;
                //btnModify.Enabled = false;  //modified as per UAT

                btnClear.Enabled = false;
                //btnSave.Enabled = true;
                // btnAdd.Enabled = false;
                //btnModify.Enabled = false;
                if (!bModify)
                {
                    btnSave.Enabled = false;
                    //btnModify.Enabled = false;
                }

                break;
            case -1://Query

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                PnlAccDtls.Visible = true;
                ChkIsActive.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                ddlGarageOwnerCode.Enabled = false;
                ddlPymntFreq.Enabled = false;
                TxtGrgOwnerName.ReadOnly = true;
                TxtGrgAddress1.ReadOnly = true;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                RdbCvrPrk.Enabled=false;
                RdbInsCvr.Enabled = false;
                ChkCntctAddress.Visible = false;
                TxtGrgOwnerName.ReadOnly = true;
                TxtGrgAddress1.ReadOnly = true;
                TxtGrgAddress2.ReadOnly = true;
                TxtGrgCity.ReadOnly = true;
                TxtGrgCountry.ReadOnly = true;
                TxtGrgEmailId.ReadOnly = true;
                TxtGrgMobile.ReadOnly = true;
                TxtGrgPINCode.ReadOnly = true;
                TxtGrgState.ReadOnly = true;
                TxtGrgTelephone.ReadOnly = true;
                TxtGrgWebsite.ReadOnly = true;
                TxtCntctName.ReadOnly = true;
                TxtCntctAddress1.ReadOnly = true;
                TxtCntctAddress2.ReadOnly = true;
                TxtCntctCity.ReadOnly = true;
                TxtCntctCountry.ReadOnly = true;
                TxtCntctEmailId.ReadOnly = true;
                TxtCntctMobile.ReadOnly = true;
                TxtCntctPINCode.ReadOnly = true;
                TxtCntctState.ReadOnly = true;
                TxtCntctTelephone.ReadOnly = true;
                TxtCntctWebsite.ReadOnly = true;
                TxtPolicyNo.ReadOnly = true;
                TxtPlcyAmnt.ReadOnly = true;
                TxtRemarks.ReadOnly = true;
                TxtRntSqFeet.ReadOnly = true;
                TxtSqFeet.ReadOnly = true;
                TxtGrgCpcty.ReadOnly = true;
                TxtTotRent.ReadOnly = true;
                TxtInsAddress1.ReadOnly = true;
                TxtInsAddress2.ReadOnly = true;
                TxtInsCity.ReadOnly = true;
                TxtInsCompany.ReadOnly = true;
                TxtInsCountry.ReadOnly = true;
                TxtInsEmailId.ReadOnly = true;
                TxtInsMobile.ReadOnly = true;
                TxtInsPINCode.ReadOnly = true;
                TxtInsState.ReadOnly = true;
                TxtInsTelephone.ReadOnly = true;
                TxtInsWebsite.ReadOnly = true;
                break;
        }
    }
    public string FunWrapText(string strWarptext, int intWraplength)
    {
        string strWarppedtext = string.Empty;
        if (strWarptext.Length > 0)
        {
            int intcharlength = 1;
            foreach (char chr in strWarptext)
            {

                if ((intcharlength % intWraplength) == 0)
                {
                    if (intcharlength > 0)
                        strWarppedtext += chr.ToString() + System.Environment.NewLine;
                    else
                        strWarppedtext += chr.ToString();
                }
                else
                {
                    strWarppedtext += chr.ToString();
                }


                intcharlength++;
            }
        }
        return strWarppedtext;
    }


    protected void ddlGarageOwnerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {

            if (ddlGarageOwnerCode.SelectedIndex != 0)
            {
                FunPubClearGarageDetails();
                DataSet dsEntityDetails;

                byte[] byte_EntityDetails = ObjOrgMasterMgtServicesClient.FunPubQueryEntityDetails(out blnTranExists, intCompanyId, Convert.ToInt32(ddlGarageOwnerCode.SelectedValue));
                dsEntityDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_EntityDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
                if (dsEntityDetails.Tables.Count > 0 && intCompanyId > 0)
                {
                    DataTable dtEntityCode = dsEntityDetails.Tables[0];
                    TxtGrgOwnerName.Text = dtEntityCode.Rows[0]["Entity_Name"].ToString();
                    Session["GLCODE"] = dtEntityCode.Rows[0]["GL_Posting_Code"].ToString();
                    TxtGrgAddress1.Focus();

                    //DataSet dsEntityDetails = new DataSet();
                    //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                    byte_EntityDetails = ObjOrgMasterMgtServicesClient.FunPubQueryEntityDetails(out blnTranExists, intCompanyId, Convert.ToInt32(ddlGarageOwnerCode.SelectedValue));
                    dsEntityDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_EntityDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
                    if (dsEntityDetails.Tables.Count > 0 && intCompanyId > 0)
                    {
                        dtEntityCode = dsEntityDetails.Tables[0];
                        TxtGrgAddress1.Text = dtEntityCode.Rows[0]["Address"].ToString();
                        TxtGrgAddress2.Text = dtEntityCode.Rows[0]["Address2"].ToString();
                        TxtGrgCity.Text = dtEntityCode.Rows[0]["City"].ToString();
                        TxtGrgCountry.Text = dtEntityCode.Rows[0]["Country"].ToString();
                        TxtGrgEmailId.Text = dtEntityCode.Rows[0]["EMail"].ToString();

                        TxtGrgMobile.Text = dtEntityCode.Rows[0]["Mobile"].ToString();
                        TxtGrgPINCode.Text = dtEntityCode.Rows[0]["PinCode"].ToString();
                        TxtGrgState.Text = dtEntityCode.Rows[0]["State"].ToString();
                        TxtGrgTelephone.Text = dtEntityCode.Rows[0]["Telephone"].ToString();
                        TxtGrgWebsite.Text = dtEntityCode.Rows[0]["website"].ToString();

                    }
                }
                ChkCntctAddress.Enabled = true;
                //StringBuilder strMsg = new StringBuilder();
                //strMsg.Append("fnCheckPageValidators('Contact Details',false);");
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Display", strMsg.ToString(), true);   
            }
            else
            {
                FunPubClearGarageDetails();
                TxtGrgOwnerName.Text = "";
                ChkCntctAddress.Enabled = false;
                StringBuilder strMsg = new StringBuilder();
                strMsg.Append("fnCheckPageValidators('Contact Details',false);");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Display", strMsg.ToString(), true);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strErrorMessage);
            throw ex;
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();
        }
    
    }
    protected void btnAll_Click(object sender, EventArgs e)
    {
        FunPriBindGrid((int)AccountMode.ALL);

    }
    protected void btnCurrent_Click(object sender, EventArgs e)
    {
        FunPriBindGrid((int)AccountMode.CURRENT);
    }

    protected void ChkIsActive_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkIsActive.Checked == false)
        {
            DataSet DSAsset = new DataSet();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Garage_ID", Session["GarageID"].ToString());
            DSAsset = Utility.GetDataset(SPNames.S3G_LR_ChkAssetExistsinGarage, Procparam);
            if (Convert.ToInt32(DSAsset.Tables[0].Rows[0]["Assets"].ToString()) > 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Move the asset in the Garage to another location before de-activating");
                ChkIsActive.Checked = true;
                return;
            }

        }
    }
}
