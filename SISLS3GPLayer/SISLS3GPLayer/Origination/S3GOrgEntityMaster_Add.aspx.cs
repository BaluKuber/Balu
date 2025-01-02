#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Vendor Master
/// Created By			: Nataraj Y
/// Created Date		: 24-June-2010
/// Purpose	            : 
/// 
/// Modified By			: Nataraj Y
/// Modified Date		: 01-Dec-2010
/// Purpose	            : Bug Fixing - Round 3
/// 

/// Module Name     :   Origination
/// Screen Name     :   Vendor Master
/// Created By      :   Swarna S
/// Created Date    :   22-09-2014
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

public partial class Origination_S3GOrgEntityMaster : ApplyThemeForProject
{
    #region Initialization
    int intCompanyId = 0;
    int intUserId = 0;
    int intEntityId = 0;
    int intErrorCode = 0;
    string strEntityCode;
    ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable ObjEntityMasterDataTable;
    //ORG.OrgMasterMgtServices.S3G_ORG_EntityBankMappingDataTable ObjEntityBankMappingDataTable;
    ORGSERVICE.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    UserInfo ObjUserInfo;
    StringBuilder strbBnkDetails = new StringBuilder();
    StringBuilder strAddressDetails = new StringBuilder();
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgEntityMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgEntityMaster_Add.aspx';";
    string strRedirectPage = "~/Origination/S3GOrgEntityMaster_Add.aspx";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    public static Origination_S3GOrgEntityMaster obj_Page;
    string strGST;
    public string strDateFormat;
    //Code end

    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            //Code end
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            obj_Page = this;

            txtTaxNumber.Attributes.Add("maxlength", "10");

            //FunSetComboBoxAttributes(txtCity.TextBox, "City", "30");
            //FunSetComboBoxAttributes(txtState, "State", "60");
            FunSetComboBoxAttributes(txtCountry, "Country", "60");

            //txtBankName.Attributes.Add("onkeypress", "wraptext(" + txtBankName.ClientID + ", 20);");
            //txtBranchName.Attributes.Add("onkeypress", "wraptext(" + txtBranchName.ClientID + ", 20);");
            //txtBranchName.Attributes.Add("onblur", "wraptext(" + txtBranchName.ClientID + ", 20);");
            // txtBankAddress.Attributes.Add("onkeypress", "wraptext(" + txtBankAddress.ClientID + ", 60);");
            //txtBankAddress.Attributes.Add("onblur", "wraptext(" + txtBankAddress.ClientID + ", 60);");
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString["qsEntityId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsEntityId"));
                if (fromTicket != null)
                {
                    intEntityId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Vendor Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }


            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            strGST = ClsPubConfigReader.FunPubReadConfig("Is_GST");//GST
            TxtSGSTRegDate.Attributes.Add("onblur", "fnDoDate(this,'" + TxtSGSTRegDate.ClientID + "','" + strDateFormat + "',true,  false);");
            TxtCGSTRegDate.Attributes.Add("onblur", "fnDoDate(this,'" + TxtCGSTRegDate.ClientID + "','" + strDateFormat + "',true,  false);");
            
            CalendarExtender2.Format = strDateFormat;
            CalendarExtender3.Format = strDateFormat;
            CalendarExtender3.OnClientDateSelectionChanged = "checkDate_NextSystemDate";

            if (!IsPostBack)
            {
                FunProIntializeData();
                if (PageMode != PageModes.Query)
                {
                    FunProLoadAddressCombos();

                    Dictionary<string, string> Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Option", "1");
                    ddlEntityType.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam, new string[] { "ENTITY_TYPE_ID", "ENTITY_TYPE_Name" });
                    Procparam.Clear();

                    Procparam.Add("@Option", "2");
                    ddlAccountType.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam, new string[] { "ID", "Name" });
                    Procparam.Clear();

                    //Dictionary<string, string> Procparam = new Dictionary<string, string>();
                    if (intCompanyId > 0)
                    {
                        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
                    }
                    DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetStateLookup", Procparam);
                    ddlAddState.BindDataTable("S3G_SYSAD_GetStateLookup", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
                    ddlComState.BindDataTable("S3G_SYSAD_GetStateLookup", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
                    ddlResState.BindDataTable("S3G_SYSAD_GetStateLookup", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });

                    Dictionary<string, string> Procparam1 = new Dictionary<string, string>();
                    Procparam1.Add("@Option", "3");
                    Procparam1.Add("@Company_ID", intCompanyId.ToString());
                    ddlGLPostingCode.BindDataTable(SPNames.S3G_ORG_GetEntity_List, Procparam1, new string[] { "GL_CODE", "Description" });
                    Procparam1.Clear();

                    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                    S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                    DataTable OrgCustomerMaster = new DataTable();
                    ObjStatus.Param1 = S3G_Statu_Lookup.COMPANY_TYPE.ToString();
                    ObjStatus.Option = 501;
                    Utility.FillDLL(ddlCompanyType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

                    ObjStatus.Option = 2;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    Utility.FillDLL(ddlConstitutionName, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

                    ObjStatus.Option = 505;
                    ObjStatus.Param1 = "Registration_Status";
                    Utility.FillDLL(ddlStateRegStatus, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));


                    ObjStatus.Param1 = "Residential_Status";
                    ObjStatus.Option = 503;
                    Utility.FillDLL(ddlResidentialStatus, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));
                    Utility.FillDLL(ddlVendorResidentialStatus, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

                    DataTable dtSource = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);
                    if (dtSource.Select("Category = 3").Length > 0)
                    {
                        dtSource = dtSource.Select("Category = 3").CopyToDataTable();
                    }
                    else
                    {
                        dtSource = FunProAddAddrColumns(dtSource);
                    }
                    //  txtCountry.FillDataTable(dtSource, "Name", "Name", false);
                    Procparam.Clear();
                }
                else if (PageMode == PageModes.Query)
                {
                    FunProLoadAddressCombos();
                }

                tcEntityMaster.ActiveTab = tbEntity;

                // Modified by R. Manikandan dated on sep/24/11
                //Procparam.Add("@Company_id", intCompanyId.ToString());
                //Procparam.Add("@Is_Active", "1");
                //Procparam.Add("@User_Id", intUserId.ToString());
                ////Procparam.Add("@Program_ID", "31");
                ////ddlBranch.BindDataTable("S3G_Get_Branch_List", Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });


                txtCity.TextBox.MaxLength = 30;
                //btnSave.Enabled = false;
                //S3GSession ObjS3GSession = new S3GSession();
                if (ObjS3GSession.ProPINCodeDigitsRW > 0)
                {
                    txtPINCode.MaxLength = ObjS3GSession.ProPINCodeDigitsRW;
                    //if (ObjS3GSession.ProPINCodeTypeRW.ToUpper() == "ALPHA NUMERIC")
                    //{
                    //    ftePIN.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                    //}
                    //else
                    //{
                    //    ftePIN.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                    //    //ftePIN.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                    //}
                }



                if (intEntityId > 0)
                {
                    bool blnTranExists;
                    FunPubProGetEntityDetails(intCompanyId, intEntityId, out blnTranExists);
                    if (blnTranExists)
                    {
                        ddlGLPostingCode.Enabled = false;
                        ddlEntityType.Enabled = false;
                    }
                    else
                    {
                        ddlGLPostingCode.Enabled = true;
                        ddlEntityType.Enabled = true;
                    }
                    if (strMode == "M")
                    {
                        FunPriEntityControlStatus(1);
                        txtCountry_TextChanged(null, null);
                    }
                    if (strMode == "Q")
                    {
                        FunPriEntityControlStatus(-1);
                        txtCountry_TextChanged(null, null);
                    }
                }
                else
                {
                    FunPriEntityControlStatus(0);
                }

            }
        }
        catch (Exception ex)
        {
            cvEntityMaster.IsValid = false;
            cvEntityMaster.ErrorMessage = "Unable to load Vendor due to data problem";
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    #endregion

    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
        }
    }

    protected void FunSetComboBoxAttributes(TextBox textBox, string Type, string maxLength)
    {
        if (textBox != null)
        {
            textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('false');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
            textBox.Attributes.Add("onpaste", "return false");
        }
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetCityList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Category", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup_AGT", Procparam), false);

        return suggetions.ToArray();
    }

    protected void FunProLoadAddressCombos()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);

            DataTable dtSource = new DataTable();
            //if (dtAddr.Select("Category = 1").Length > 0)
            //{
            //    dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
            //}
            //else
            //{
            //    dtSource = FunProAddAddrColumns(dtSource);
            //}
            //txtCity.FillDataTable(dtSource, "Name", "Name", false);

            //dtSource = new DataTable();
            //if (dtAddr.Select("Category = 2").Length > 0)
            //{
            //    dtSource = dtAddr.Select("Category = 2").CopyToDataTable();
            //}
            //else
            //{

            //    dtSource = FunProAddAddrColumns(dtSource);
            //}
            //txtState.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtCountry.FillDataTable(dtSource, "Name", "Name", false);


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        if (dt.Columns.Count == 0)
        {
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Category");
        }
        return dt;
    }

    #region Page Events



    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string PANExpression = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
        //if (!Regex.IsMatch(txtPAN.Text.Trim(), PANExpression))
        //{
        //    Utility.FunShowAlertMsg(this, "Permanent Tax Number should be in format of AAAAA9999A");
        //    tcEntityMaster.ActiveTab = tbEntity;
        //    txtPAN.Focus();
        //    return;
        //}

        if (txtEntityName.Text.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "Vendor Name is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtEntityName.Focus();
            return;
        }

        if (ddlGLPostingCode.SelectedValue == "")
        {
            Utility.FunShowAlertMsg(this, "Please select a posting code");
            tcEntityMaster.ActiveTab = tbEntity;
            ddlGLPostingCode.Focus();
            return;
        }

        //if(txtCIN.Text.Trim() !="")
        //{
        //    if (txtCIN.Text.Length != 21)
        //    {
        //        Utility.FunShowAlertMsg(this, "CIN length should be of 21 characters");
        //        tcEntityMaster.ActiveTab = tbEntity;
        //        txtCIN.Focus();
        //        return;
        //    }
        //}

        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjEntityMasterDataTable = new ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterDataTable();
            ORG.OrgMasterMgtServices.S3G_ORG_Entity_MasterRow ObjEntityRow;
            ObjEntityRow = ObjEntityMasterDataTable.NewS3G_ORG_Entity_MasterRow();
            ObjEntityRow.Entity_Master_ID = intEntityId;
            ObjEntityRow.Entity_Code = txtEntityCode.Text;
            ObjEntityRow.Entity_Type = Convert.ToInt32(ddlEntityType.SelectedItem.Value);
            ObjEntityRow.Entity_Name = txtEntityName.Text.Trim();
            ObjEntityRow.Company_Id = intCompanyId;
            ObjEntityRow.Service_Tax_Number = txtTaxNumber.Text.Trim();
            ObjEntityRow.Company_Type = ddlCompanyType.SelectedValue;
            ObjEntityRow.Reg_Status = "0";
            ObjEntityRow.Constitution = ddlConstitutionName.SelectedValue;
            ObjEntityRow.Service_Tax_Number = txtTaxNumber.Text.Trim();
            ObjEntityRow.PAN = txtPAN.Text.Trim();
            ObjEntityRow.CIN = txtCIN.Text.Trim();
            ObjEntityRow.Website = txtWebsite.Text.Trim();
            ObjEntityRow.ResidentialStatus = ddlVendorResidentialStatus.SelectedValue;
            ObjEntityRow.USRID = Convert.ToString(intUserId);
            ObjEntityRow.GL_Code = ddlGLPostingCode.SelectedItem.Value;
            ObjEntityRow.CGSTIN = txtCGSTin.Text.Trim();
            if (TxtCGSTRegDate.Text.Trim() != "")
                ObjEntityRow.CGST_Reg_Date = Utility.StringToDate(TxtCGSTRegDate.Text.ToString());
            ObjEntityRow.Composition = Convert.ToInt32(ddlComposition.SelectedValue);

            ObjEntityRow.MSME_Registered = Convert.ToInt32(ddlMSMERegistered.SelectedValue);
            ObjEntityRow.Vendor_Type = Convert.ToInt32(ddlType.SelectedValue);
            ObjEntityRow.Certificate_Received = Convert.ToInt32(ddlCertificateReceived.SelectedValue);

            ObjEntityRow.EInvoice = Convert.ToInt32(ddlEInvoice.SelectedValue);

            S3GSession ObjS3GSession = new S3GSession();

            DataTable dtaddress = new DataTable();
            dtaddress = (DataTable)ViewState["BillingAddress"];
            DataRow[] drRows1 = new DataRow[100];


            if (dtaddress != null)
            {
                if (dtaddress.Rows.Count > 0)
                {
                    strAddressDetails.Append("<Root>");
                    foreach (DataRow dtBankDetailsRow in dtaddress.Rows)
                    {
                        strAddressDetails.Append("<Details ");
                        strAddressDetails.Append(" Res_Status = '" + dtBankDetailsRow["RES_ADD"].ToString() + "'");
                        strAddressDetails.Append(" Reg_Status = '" + dtBankDetailsRow["REG_ADD"].ToString() + "'");
                        strAddressDetails.Append(" Address1 = '" + dtBankDetailsRow["Address1"].ToString() + "'");
                        strAddressDetails.Append(" Address2 = '" + dtBankDetailsRow["Address2"].ToString() + "'");
                        strAddressDetails.Append(" City ='" + dtBankDetailsRow["City"].ToString() + "'");
                        strAddressDetails.Append(" State = '" + dtBankDetailsRow["Location_Category_ID"].ToString() + "'");
                        /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
                        strAddressDetails.Append(" Loc_Code = '" + dtBankDetailsRow["Loc_Code"].ToString() + "'");
                        /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
                        strAddressDetails.Append(" Country ='" + dtBankDetailsRow["Country"].ToString() + "'");
                        strAddressDetails.Append(" Resident_State ='" + dtBankDetailsRow["Resident_State_ID"].ToString() + "'");
                        strAddressDetails.Append(" Pincode ='" + dtBankDetailsRow["Pincode"].ToString() + "'");
                        strAddressDetails.Append(" Telephone ='" + dtBankDetailsRow["Telephone"].ToString() + "'");
                        if (dtBankDetailsRow["Mobile"].ToString() != "")
                        {
                            strAddressDetails.Append(" Mobile = '" + dtBankDetailsRow["Mobile"].ToString() + "'");
                        }
                        else
                        {
                            strAddressDetails.Append(" Mobile = '0'");
                        }
                        strAddressDetails.Append(" Email ='" + dtBankDetailsRow["Email"].ToString() + "'");
                        strAddressDetails.Append(" VAT_Number = '" + dtBankDetailsRow["VAT_Number"].ToString() + "'");
                        strAddressDetails.Append(" CST_TIN ='" + dtBankDetailsRow["CST_TIN"].ToString() + "'");
                        strAddressDetails.Append(" SGSTIN ='" + dtBankDetailsRow["SGSTIN"].ToString() + "'");
                        strAddressDetails.Append(" SGST_Reg_Date ='" + dtBankDetailsRow["SGST_Reg_Date"].ToString() + "'");
                        strAddressDetails.Append(" Tax_Account_Number ='" + dtBankDetailsRow["Tax_Account_Number"].ToString() + "'/>");
                    }
                    strAddressDetails.Append("</Root>");
                }
            }
            //else
            //{
            //    strAlert = strAlert.Replace("__ALERT__", "Address details cannot be empty");
            //    strRedirectPageView = "";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            //    return;
            //}

            ObjEntityRow.XMLAddressDetails = strAddressDetails.ToString();


            DataTable dtSubmit = new DataTable();
            dtSubmit = (DataTable)ViewState["DetailsTable"];
            DataRow[] drRows = dtSubmit.Select("len(MICR_Code) <> '" + txtMICRCode.MaxLength + "'");
            //if (drRows.Length > 0)
            //{
            //    Utility.FunShowAlertMsg(this, "Mismatch between MICR Code(s) and Country");
            //    return;
            //}

            if (dtSubmit != null)
            {
                if (dtSubmit.Rows.Count > 0)
                {
                    strbBnkDetails.Append("<Root>");
                    foreach (DataRow dtBankDetailsRow in dtSubmit.Rows)
                    {
                        strbBnkDetails.Append("<Details ");
                        strbBnkDetails.Append(" Account_Number = '" + dtBankDetailsRow["Account_Number"].ToString() + "'");
                        strbBnkDetails.Append(" Account_Type = '" + dtBankDetailsRow["AccountType_ID"].ToString() + "'");
                        strbBnkDetails.Append(" Branch_Address ='" + dtBankDetailsRow["Branch_Address"].ToString() + "'");
                        strbBnkDetails.Append(" Bank_Name = '" + dtBankDetailsRow["Bank_Name"].ToString() + "'");
                        strbBnkDetails.Append(" Branch_Name ='" + dtBankDetailsRow["Branch_Name"].ToString() + "'");
                        strbBnkDetails.Append(" IFSC_Code ='" + dtBankDetailsRow["IFSC_Code"].ToString() + "'");
                        strbBnkDetails.Append(" State ='" + dtBankDetailsRow["Location_Category_ID"].ToString() + "'");
                        strbBnkDetails.Append(" MICR_Code = '" + dtBankDetailsRow["MICR_Code"].ToString() + "'");
                        strbBnkDetails.Append(" Is_Default_Account = '" + dtBankDetailsRow["Is_Default_Account"].ToString() + "'/>");
                    }
                    strbBnkDetails.Append("</Root>");
                }
                //else
                //{
                //    strAlert = strAlert.Replace("__ALERT__", "Bank details cannot be empty");
                //    strRedirectPageView = "";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                //    return;
                //}
            }
            //else
            //{
            //    strAlert = strAlert.Replace("__ALERT__", "Bank details cannot be empty");
            //    strRedirectPageView = "";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            //    return;
            //}

            ObjEntityRow.XMLBank_Details = strbBnkDetails.ToString();
            ObjEntityMasterDataTable.AddS3G_ORG_Entity_MasterRow(ObjEntityRow);

            if (ObjEntityMasterDataTable.Rows.Count > 0)
            {
                //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_ORG_Entity_DataTable = ClsPubSerialize.Serialize(ObjEntityMasterDataTable, SerMode);

                if (intEntityId > 0)
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubModifyEntityInt(SerMode, byteobjS3G_ORG_Entity_DataTable);
                }
                else
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateEntityInt(out strEntityCode, SerMode, byteobjS3G_ORG_Entity_DataTable);
                }

                switch (intErrorCode)
                {
                    case 0:
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        txtEntityCode.Text = strEntityCode;
                        if (intEntityId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Vendor details updated successfully");
                        }
                        else
                        {
                            strAlert = "Vendor code " + strEntityCode + " added successfully";
                            strAlert += @"\n\nWould you like to add one more Vendor?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                        }
                        break;
                    case 1:
                        strAlert = strAlert.Replace("__ALERT__", "Service Tax Reg. No. already exists, Enter a new Service Tax Reg. No. Account Number");
                        strRedirectPageView = "";
                        break;
                    case -1:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        strRedirectPageView = "";
                        break;
                    case -2:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                        strRedirectPageView = "";
                        break;
                    case -3://Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                        strAlert = strAlert.Replace("__ALERT__", Resources.ValidationMsgs._3.ToString());
                        strRedirectPageView = "";
                        break;
                    case -4://Added by Tamilselvan.S on 23/11/2011 for Adding validation for Duplication check
                        strAlert = strAlert.Replace("__ALERT__", "Vendor Details Already Exists");
                        strRedirectPageView = "";
                        break;
                    case -5://Added by Swarna for PAN Validation in OPC
                        strAlert = strAlert.Replace("__ALERT__", "PAN Number Already Exists for other Vendor");
                        strRedirectPageView = "";
                        break;
                    case -6://Added by Swarna for CIN Validation in OPC
                        strAlert = strAlert.Replace("__ALERT__", "CIN Number Already Exists for other Vendor");
                        strRedirectPageView = "";
                        break;
                    case -7://Added by Swarna for VAT_Number Validation in OPC
                        strAlert = strAlert.Replace("__ALERT__", "VAT-TIN Number Already Exists for other Vendor");
                        strRedirectPageView = "";
                        break;
                    case -8://Added by Swarna for CST_TIN Validation in OPC
                        strAlert = strAlert.Replace("__ALERT__", "CST-TIN Number Already Exists for other Vendor");
                        strRedirectPageView = "";
                        break;
                    case -9://Added by Swarna for Tax_Account_Number Validation in OPC
                        strAlert = strAlert.Replace("__ALERT__", "TAN Number Already Exists for other Vendor");
                        strRedirectPageView = "";
                        break;
                    case -10://Added by Swarna for Service Tax Number Validation in OPC
                        strAlert = strAlert.Replace("__ALERT__", "Service Tax Number Already Exists for other Vendor");
                        strRedirectPageView = "";
                        break;
                    case -11:
                        strAlert = strAlert.Replace("__ALERT__", "SGSTIN Already Exists for other Entity");
                        strRedirectPageView = "";
                        break;
                    /*Modified by vinodha.m on Sep5,2015 - commented to remove the below validation as per the client call*/
                    //case -11://Added by Swarna for Vendor Name - Validation in OPC
                    //    strAlert = strAlert.Replace("__ALERT__", "Vendor Name cannot be changed as it is defined in the transaction");
                    //    strRedirectPageView = "";
                    //    break;
                    /*Modified by vinodha.m on Sep5,2015 - commented to remove the below validation as per the client call*/
                    default:
                        if (intEntityId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Vendor details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Vendor details");
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

    protected void grvBankDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        //AddConfirmDelete(gv, e); 
        //AddConfirmDelete((GridView)sender,e);
        DataTable dtDelete;
        dtDelete = (DataTable)ViewState["DetailsTable"];
        dtDelete.Rows.RemoveAt(e.RowIndex);
        grvBankDetails.DataSource = dtDelete;
        grvBankDetails.DataBind();
        ViewState["DetailsTable"] = dtDelete;
        FunClearBankDetails();
    }

    protected void DeleteBankDetails_clik(object sender, EventArgs e)
    {
        string Confirm = "if(confirm('Do you want to remove the record?')){Yes}else {No}";
        if (Confirm.ToLower() == "yes")
        {
            string strFieldAtt = ((LinkButton)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvBankDetails_")).Replace("grvBankDetails_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;

            string strRefno = ((LinkButton)grvBankDetails.Rows[gRowIndex].FindControl("lnkbtnDelete")).Text;

            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["DetailsTable"];
            dtDelete.Rows.RemoveAt(gRowIndex);
            grvBankDetails.DataSource = dtDelete;
            grvBankDetails.DataBind();
        }
    }


    //protected void btnAddAddress_Click(object sender, EventArgs e)
    //{
    //    if (txtAddress.Text.Trim() == "")
    //    {
    //        Utility.FunShowAlertMsg(this, "Address is mandatory in Vendor Address");
    //        tcEntityMaster.ActiveTab = tbEntity;
    //        txtAddress.Focus();
    //        return;
    //    }


    //    if (txtCity.SelectedText.Trim() == "")
    //    {
    //        Utility.FunShowAlertMsg(this, "City is mandatory in Vendor Address");
    //        tcEntityMaster.ActiveTab = tbEntity;
    //        txtCity.Focus();
    //        return;
    //    }

    //    if (txtPerTIN.Text != "")
    //    {
    //        if (txtPerTIN.Text.Length != 15)
    //        {
    //            Utility.FunShowAlertMsg(this, "TIN Number should be of 15 characters in Billing Address");
    //            tcCustomerMaster.ActiveTab = tbAddress;
    //            txtPerTIN.Focus();
    //            return;
    //        }
    //    }

    //    if (txtState.SelectedValue.Trim() == "")
    //    {
    //        Utility.FunShowAlertMsg(this, "TAN Number should be of 15 characters in Billing Address");
    //        tcEntityMaster.ActiveTab = tbEntity;
    //        txtState.Focus();
    //        return;
    //    }

    //    DataRow drDetails;
    //    DataTable dtBankDetails = new DataTable();
    //    DataTable dtExist = new DataTable();

    //    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

    //    S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
    //    ObjStatus.Option = 1;
    //    ObjStatus.@Param1 = txtPerState.SelectedValue.Trim();
    //    DataTable ObjCustomerDetails = new DataTable();

    //    ObjCustomerDetails = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
    //    string CorporateTIN = "";
    //    string CorporateTAN = "";

    //    if (ObjCustomerDetails.Rows.Count > 0)
    //    {
    //        CorporateTIN = Convert.ToString(ObjCustomerDetails.Rows[0]["TIN"]);
    //        CorporateTAN = Convert.ToString(ObjCustomerDetails.Rows[0]["TAN"]);
    //    }

    //    if (CorporateTIN == txtPerTIN.Text.Trim())
    //    {
    //        Utility.FunShowAlertMsg(this, "TIN Number already defined for other state");
    //        tcCustomerMaster.ActiveTab = tbAddress;
    //        txtPerTIN.Focus();
    //        return;
    //    }

    //    if (CorporateTAN == txtPerTAN.Text.Trim())
    //    {
    //        Utility.FunShowAlertMsg(this, "TAN Number already defined for other state");
    //        tcCustomerMaster.ActiveTab = tbAddress;
    //        txtPerTAN.Focus();
    //        return;
    //    }


    //    if (ViewState["BillingAddress"] != null)
    //        dtBankDetails = (DataTable)ViewState["BillingAddress"];
    //    // Exist Pooling Row Based On Select

    //    //foreach (DataRow MyDataRow in dtBankDetails.Select("TIN='" + txtPerTIN.Text + "' And TAN ='" + txtPerTAN.Text + "'"))
    //    //    dtExist.ImportRow(MyDataRow);

    //    //DataRow[] rowsToUpdate = dtExist.Select("State <> '" + txtPerState.SelectedValue.Trim() + "'");
    //    //if (rowsToUpdate.Length > 0)
    //    //{
    //    //    Utility.FunShowAlertMsg(this, "Selected Combination already exists");
    //    //    return;
    //    //}


    //    if (ViewState["BillingAddress"] != null)
    //    {
    //        drDetails = dtBankDetails.NewRow();
    //        string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
    //        drDetails["Rowno"] = strBankMapId;
    //        drDetails["State"] = txtPerState.SelectedItem.ToString().Trim();
    //        drDetails["City"] = txtPerCity.SelectedText.Trim();
    //        drDetails["Address1"] = txtPerAddress1.Text.Trim();
    //        drDetails["Address2"] = txtPerAddress2.Text.Trim();
    //        drDetails["Pincode"] = txtPerPincode.Text.Trim();
    //        drDetails["Email"] = txtPerEmail.Text.Trim();
    //        drDetails["Contact_Name"] = txtPerContName.Text.Trim();
    //        drDetails["Contact_No"] = txtPerContactNo.Text.Trim();
    //        drDetails["TIN"] = txtPerTIN.Text.Trim();
    //        drDetails["TAN"] = txtPerTAN.Text.Trim();
    //    }
    //    else
    //    {
    //        dtBankDetails = new DataTable();
    //        dtBankDetails.Columns.Add("Rowno");
    //        dtBankDetails.Columns.Add("State");
    //        dtBankDetails.Columns.Add("City");
    //        dtBankDetails.Columns.Add("Address1");
    //        dtBankDetails.Columns.Add("Address2");
    //        dtBankDetails.Columns.Add("PINCODE");
    //        dtBankDetails.Columns.Add("Email");
    //        dtBankDetails.Columns.Add("Contact_Name");
    //        dtBankDetails.Columns.Add("Contact_No");
    //        dtBankDetails.Columns.Add("TIN");
    //        dtBankDetails.Columns.Add("TAN");

    //        drDetails = dtBankDetails.NewRow();
    //        string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
    //        drDetails["Rowno"] = strBankMapId;
    //        drDetails["State"] = txtPerState.SelectedItem.Text.Trim();
    //        drDetails["City"] = txtPerCity.SelectedText.Trim();
    //        drDetails["Address1"] = txtPerAddress1.Text.Trim();
    //        drDetails["Address2"] = txtPerAddress2.Text.Trim();
    //        drDetails["PINCODE"] = txtPerPincode.Text.Trim();
    //        drDetails["Email"] = txtPerEmail.Text.Trim();
    //        drDetails["Contact_Name"] = txtPerContName.Text.Trim();
    //        drDetails["Contact_No"] = txtPerContactNo.Text.Trim();
    //        drDetails["TIN"] = txtPerTIN.Text.Trim();
    //        drDetails["TAN"] = txtPerTAN.Text.Trim();

    //    }
    //    dtBankDetails.Rows.Add(drDetails);
    //    gvBAddress.DataSource = dtBankDetails;
    //    gvBAddress.DataBind();
    //    ViewState["BillingAddress"] = dtBankDetails;
    //    txtState.SelectedItem.Text = "";
    //    txtCity.Clear();
    //    txtAddress.Text = "";
    //    txtAddress2.Text = "";
    //    txtCountry.SelectedIndex = 0;
    //    txtPINCode.Text = "";
    //    txtTelephone.Text = "";
    //    txtMobile.Text = "";
    //    txtVEndorEmailId.Text = "";
    //    txtVATTIN.Text = "";
    //    txtCSTTIN.Text = "";
    //    txtTAN.Text = "";
    //}



    protected void gvBAddress_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlStateRegStatus.Visible = true;
            lblShowRegStatus.Visible = false;

            Label lblState = gvBAddress.SelectedRow.FindControl("lblState") as Label;
            Label lblResStatus = gvBAddress.SelectedRow.FindControl("lblResStatus") as Label;
            Label lblStaRegStatus = gvBAddress.SelectedRow.FindControl("lblStaRegStatus") as Label;
            Label lblResStatusCode = gvBAddress.SelectedRow.FindControl("lblResStatusCode") as Label;
            Label lblRegStatusCode = gvBAddress.SelectedRow.FindControl("lblRegStatusCode") as Label;
            Label lblStateCode = gvBAddress.SelectedRow.FindControl("lblStateCode") as Label;
            /*added by vinodha m on sep 8,2015 - cr in vendor,upload - POPLT*/
            Label lblLoc_Code = gvBAddress.SelectedRow.FindControl("lblLoc_Code") as Label;
            HiddenField gvhentityadrz_id = gvBAddress.SelectedRow.FindControl("gvhentityadrz_id") as HiddenField;
            /*added by vinodha m on sep 8,2015 - cr in vendor,upload - POPLT*/
            Label lblResiState = gvBAddress.SelectedRow.FindControl("lblResiState") as Label;
            Label lblResiStateID = gvBAddress.SelectedRow.FindControl("lblResiStateID") as Label;
            Label lblCity = gvBAddress.SelectedRow.FindControl("lblCity") as Label;
            Label lblAddress1 = gvBAddress.SelectedRow.FindControl("lblAddress1") as Label;
            Label lblAddress2 = gvBAddress.SelectedRow.FindControl("lblAddress2") as Label;
            Label lblCountry = gvBAddress.SelectedRow.FindControl("lblCountry") as Label;
            Label lblPIN = gvBAddress.SelectedRow.FindControl("lblPIN") as Label;
            Label lblEmailID = gvBAddress.SelectedRow.FindControl("lblEmailID") as Label;
            Label lblVATTIN = gvBAddress.SelectedRow.FindControl("lblVATTIN") as Label;
            Label lblMobNo = gvBAddress.SelectedRow.FindControl("lblMobNo") as Label;
            Label lblTelephoneNo = gvBAddress.SelectedRow.FindControl("lblTelephoneNo") as Label;
            Label lblTAN = gvBAddress.SelectedRow.FindControl("lblTAN") as Label;
            Label lblCSTTIN = gvBAddress.SelectedRow.FindControl("lblCSTTIN") as Label;
            Label lblRowNo = gvBAddress.SelectedRow.FindControl("lblRowNo") as Label;
            LinkButton lnkDelete = gvBAddress.SelectedRow.FindControl("lnkDelete") as LinkButton;
            Label lblSGSTIN = gvBAddress.SelectedRow.FindControl("lblSGSTIN") as Label;
            Label lblSGSTRegDate = gvBAddress.SelectedRow.FindControl("lblSGSTRegDate") as Label;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/

            for (int i = 0; i <= gvBAddress.Rows.Count - 1; i++)
            {
                if (i == gvBAddress.SelectedRow.RowIndex)
                {
                    lnkDelete.Enabled = false;

                    //break;
                }
                else
                {
                    LinkButton lnkDelete1 = gvBAddress.Rows[i].FindControl("lnkDelete") as LinkButton;
                    lnkDelete1.Enabled = true;
                }
            }

            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Option", "4");
            //Procparam.Add("@Param1", lblState.Text);
            //DataTable dt = Utility.GetDefaultData("S3G_ORG_CheckTINTAN", Procparam);
            //Procparam.Clear();

            //if (dt.Rows.Count > 0)
            //{
            //    ddlComState.SelectedValue = dt.Rows[0]["Location_Category_ID"].ToString();
            //}

            ddlComState.SelectedValue = lblStateCode.Text;
            if (lblResStatus.Text == "Non-Resident")
            {
                ddlResState.SelectedValue = lblResiStateID.Text;
            }
            else
            {
                ddlResState.SelectedValue = "0";
            }
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Option", "503");
            //Procparam.Add("@Param1", "Residential_Status");
            //Procparam.Add("@Param2", lblResStatus.Text);
            //DataTable dt1 = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);
            //Procparam.Clear();

            //if (dt1.Rows.Count > 0)
            //{
            //    ddlResidentialStatus.SelectedValue = dt1.Rows[0]["Value"].ToString();
            //}
            ddlResidentialStatus.SelectedValue = lblResStatusCode.Text;
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Option", "777");
            //Procparam.Add("@Param1", "Registration_Status");
            //Procparam.Add("@Param2", lblStaRegStatus.Text);
            //DataTable dt2 = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);
            //Procparam.Clear();

            //if (dt2.Rows.Count > 0)
            //{
            //    ddlStateRegStatus.SelectedValue = dt2.Rows[0]["Value"].ToString();
            //}


            ddlStateRegStatus.SelectedValue = lblRegStatusCode.Text;
            txtCity.SelectedText = lblCity.Text.Trim();
            /*added by vinodha m on sep 8,2015 - cr in vendor,upload - POPLT*/
            txtLoc_Code.Text = lblLoc_Code.Text;
            //hvEntityAdrz_ID.Value = gvhentityadrz_id.Value;
            /*added by vinodha m on sep 8,2015 - cr in vendor,upload - POPLT*/

            Dictionary<string, string> Procparam1 = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam1.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam1);

            DataTable dtSource = new DataTable();
            dtSource = new DataTable();
            if (dtAddr.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtCountry.ClearDropDownList();
            txtCountry.FillDataTable(dtSource, "Name", "Name", false);

            txtAddress.Text = lblAddress1.Text;
            txtAddress2.Text = lblAddress2.Text;
            txtPINCode.Text = lblPIN.Text;
            txtVEndorEmailId.Text = lblEmailID.Text;
            txtTelephone.Text = lblTelephoneNo.Text;
            txtMobile.Text = lblMobNo.Text;
            txtVATTIN.Text = lblVATTIN.Text;
            txtCSTTIN.Text = lblCSTTIN.Text;
            txtTAN.Text = lblTAN.Text;
            hentadrz_id.Value = gvhentityadrz_id.Value;
            hdnAddID.Value = Convert.ToString(gvBAddress.SelectedRow.RowIndex);
            txtSGSTin.Text = lblSGSTIN.Text.ToString();
            TxtSGSTRegDate.Text = lblSGSTRegDate.Text.ToString();
            if (strMode != "Q")
            {
                btnAddAddress.Enabled = false;
                btnModifyAddress.Enabled = true;
            }
            else
            {
                btnAddAddress.Enabled = false;
                btnModifyAddress.Enabled = false;
            }
            txtCountry.SelectedValue = lblCountry.Text.ToString();
            ddlResidentialStatus_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvEntityMaster.IsValid = false;
            cvEntityMaster.ErrorMessage = "Unable to retrive bank details";
        }
    }

    protected void ddlComState_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (PageMode == PageModes.Create)
        //{
        DataTable dtAddressValidate = new DataTable();
        if (ViewState["BillingAddress"] != null)
        {
            dtAddressValidate = (DataTable)ViewState["BillingAddress"];
            DataRow[] drow = dtAddressValidate.Select("Location_Category_ID = " + ddlComState.SelectedValue.ToString());
            if (drow.Length > 0)
            {
                ddlStateRegStatus.Visible = false;
                lblShowRegStatus.Visible = true;
                RequiredFieldValidator1.Enabled = false;
                lblShowRegStatus.Text = drow[0]["REGSTATUS"].ToString();
                if (lblShowRegStatus.Text == "Registered")
                {
                    lblShowRegID.Text = "1";
                }
                else if (lblShowRegStatus.Text == "Non-Registered")
                {
                    lblShowRegID.Text = "2";
                }
            }
            else
            {
                RequiredFieldValidator1.Enabled = true;
                ddlStateRegStatus.Visible = true;
                lblShowRegStatus.Visible = false;
                ddlStateRegStatus.SelectedIndex = 0;
            }
        }
        //}
    }

    protected void btnBnkModify_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtIFSC_Code.Text.Trim() != "")
            {
                if (txtIFSC_Code.Text.Trim().Length != 11)
                {
                    Utility.FunShowAlertMsg(this, "IFSC Code should be 11 digits");
                    return;
                }
            }

            DataRow drDetails;
            DataTable dtBankDetails = (DataTable)ViewState["DetailsTable"];
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            DataRow[] drDefault = dtBankDetails.Select("Is_Default_Account = 'True'");
            if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            {
                Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
                return;
            }

            DataView dvBankdetails = dtBankDetails.DefaultView;
            dvBankdetails.Sort = "Rowno";
            int rowindex = Convert.ToInt16(hdnBankId.Value);

            dvBankdetails[rowindex].Row["Account_type"] = ddlAccountType.SelectedItem.ToString();
            dvBankdetails[rowindex].Row["AccountType_ID"] = ddlAccountType.SelectedValue.ToString();
            dvBankdetails[rowindex].Row["Account_Number"] = txtAccountNumber.Text.Trim();
            dvBankdetails[rowindex].Row["Bank_Name"] = txtBankName.Text.Trim();
            dvBankdetails[rowindex].Row["Branch_Name"] = txtBranchName.Text.Trim();
            dvBankdetails[rowindex].Row["MICR_Code"] = txtMICRCode.Text.Trim();
            dvBankdetails[rowindex].Row["IFSC_code"] = txtIFSC_Code.Text.Trim();
            dvBankdetails[rowindex].Row["State"] = ddlAddState.SelectedItem.ToString();
            dvBankdetails[rowindex].Row["Location_Category_ID"] = ddlAddState.SelectedValue.ToString();
            dvBankdetails[rowindex].Row["Branch_Address"] = txtBankAddress.Text.Trim();
            if (chkDefaultAccount.Checked == true)
            {
                dvBankdetails[rowindex].Row["Is_Default_Account"] = 1;
            }
            else
            {
                dvBankdetails[rowindex].Row["Is_Default_Account"] = 0;
            }

            grvBankDetails.DataSource = dvBankdetails;
            grvBankDetails.DataBind();
            ViewState["DetailsTable"] = dvBankdetails.Table;
            FunClearBankDetails();
            btnBnkAdd.Enabled = true;
            btnBnkModify.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvEntityMaster.IsValid = false;
            cvEntityMaster.ErrorMessage = "Unable to modify bank details";
        }
    }

    protected void btnBnkAdd_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dtBankDetails = (DataTable)ViewState["DetailsTable"];
            //if (txtCountry.Text.Trim().ToLower() == "india")
            //{
            if (txtMICRCode.Text.Trim().Length != 9)
            {
                Utility.FunShowAlertMsg(this, "MICR Code should be 9 digits");
                return;
            }


            //}
            //else
            //{
            if (txtIFSC_Code.Text.Trim() != "")
            {
                if (txtIFSC_Code.Text.Trim().Length != 11)
                {
                    Utility.FunShowAlertMsg(this, "IFSC Code should be 11 digits");
                    return;
                }
            }

            DataRow drDetails;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            DataRow[] drDefault = dtBankDetails.Select("Is_Default_Account = 'True'");
            if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            {
                Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
                return;
            }

            if (dtBankDetails.Rows.Count < 10)
            {
                //grvBankDetails
                drDetails = dtBankDetails.NewRow();
                string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
                drDetails["Rowno"] = strBankMapId;
                drDetails["Account_type"] = ddlAccountType.SelectedItem.ToString();
                drDetails["AccountType_ID"] = ddlAccountType.SelectedValue.ToString();
                drDetails["Account_Number"] = txtAccountNumber.Text.Trim();
                drDetails["Bank_Name"] = txtBankName.Text.Trim();
                drDetails["Branch_Name"] = txtBranchName.Text.Trim();
                drDetails["MICR_Code"] = txtMICRCode.Text.Trim();
                drDetails["IFSC_code"] = txtIFSC_Code.Text.Trim();
                drDetails["State"] = ddlAddState.SelectedItem.ToString();
                drDetails["Location_Category_ID"] = ddlAddState.SelectedValue.ToString();
                if (chkDefaultAccount.Checked == true)
                {
                    drDetails["Is_Default_Account"] = 1;
                }
                else
                {
                    drDetails["Is_Default_Account"] = 0;
                }

                drDetails["Branch_Address"] = txtBankAddress.Text.Trim();
                dtBankDetails.Rows.Add(drDetails);
                grvBankDetails.DataSource = dtBankDetails;
                grvBankDetails.DataBind();
                //wraptext(grvBankDetails, 20);
                ViewState["DetailsTable"] = dtBankDetails;
                FunClearBankDetails();
                btnBnkAdd.Enabled = true;
                btnBnkModify.Enabled = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Class", "alert('Cannot add more than 10 Rows');", true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvEntityMaster.IsValid = false;
            cvEntityMaster.ErrorMessage = "Unable to add bank details";
        }
    }

    protected void txtCountry_TextChanged(object sender, EventArgs e)
    {
        // txtTaxNumber.Text = "";
        FunChangePANFormat(txtCountry.Text.Trim());

    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        tcEntityMaster.Tabs[1].Enabled = true;
        tcEntityMaster.ActiveTabIndex = 1;
    }

    protected void grvBankDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (intEntityId > 0)
            //{
            if (ddlAccountType.Visible)
            {
                Label lblAccountID = grvBankDetails.SelectedRow.FindControl("lblAccountID") as Label;
                Label lblRowNo = grvBankDetails.SelectedRow.FindControl("lblRowNo") as Label;
                Label lblAccountType = grvBankDetails.SelectedRow.FindControl("lblAccountType") as Label;
                Label lblAccountNumber = grvBankDetails.SelectedRow.FindControl("lblAccountNumber") as Label;
                Label lblBankName = grvBankDetails.SelectedRow.FindControl("lblBankName") as Label;
                Label lblBranchName = grvBankDetails.SelectedRow.FindControl("lblBranchName") as Label;
                Label lblBranchAddress = grvBankDetails.SelectedRow.FindControl("lblBranchAddress") as Label;
                Label lblState = grvBankDetails.SelectedRow.FindControl("lblState") as Label;
                Label lblMICRCode = grvBankDetails.SelectedRow.FindControl("lblMICRCode") as Label;
                Label lblIFSCCode = grvBankDetails.SelectedRow.FindControl("lblIFSCCode") as Label;
                Label lblDefAccount = grvBankDetails.SelectedRow.FindControl("lblDefAccount") as Label;
                LinkButton lnkbtnDelete = grvBankDetails.SelectedRow.FindControl("lnkbtnDelete") as LinkButton;


                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Option", "4");
                Procparam.Add("@Param1", lblState.Text.Trim());
                DataTable dt = Utility.GetDefaultData("S3G_ORG_CheckTINTAN", Procparam);
                Procparam.Clear();

                if (dt.Rows.Count > 0)
                {
                    ddlAddState.SelectedValue = dt.Rows[0]["Location_Category_ID"].ToString();
                }

                Procparam.Add("@Option", "5");
                Procparam.Add("@Param1", lblAccountType.Text.Trim());
                DataTable dt1 = Utility.GetDefaultData("S3G_ORG_CheckTINTAN", Procparam);
                Procparam.Clear();

                if (dt1.Rows.Count > 0)
                {
                    ddlAccountType.SelectedValue = dt1.Rows[0]["ID"].ToString();
                }
                if (lblDefAccount.Text == "1" || lblDefAccount.Text == "True")
                {
                    chkDefaultAccount.Checked = true;
                }
                else
                {
                    chkDefaultAccount.Checked = false;
                }


                txtAccountNumber.Text = lblAccountNumber.Text.Trim();
                txtBankName.Text = lblBankName.Text.Trim();
                txtBranchName.Text = lblBranchName.Text.Trim();
                txtMICRCode.Text = lblMICRCode.Text.Trim();
                hdnBankId.Value = Convert.ToString(grvBankDetails.SelectedRow.RowIndex);
                txtBankAddress.Text = lblBranchAddress.Text.Trim();
                txtIFSC_Code.Text = lblIFSCCode.Text.Trim();
                btnBnkModify.Enabled = true;
                btnBnkAdd.Enabled = false;

                for (int i = 0; i <= grvBankDetails.Rows.Count - 1; i++)
                {
                    if (i == grvBankDetails.SelectedRow.RowIndex)
                    {
                        lnkbtnDelete.Enabled = false;

                    }

                }
            }
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "NoEdit", "alert('Edit is not allowed in this Mode');", true);
            //    btnBnkAdd.Enabled = true;
            //}
        }
        catch (Exception ex)
        {
            if (ddlAccountType.Visible)
            {
                // ddlBranch.SelectedIndex = 0;
                btnBnkModify.Enabled = true;
                btnBnkAdd.Enabled = false;
            }
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {

        }
    }

    protected void grvBankDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDefAccount = (Label)e.Row.FindControl("lblDefAccount");
                CheckBox chkgvDefaultAccount = (CheckBox)e.Row.FindControl("chkgvDefaultAccount");
                if (lblDefAccount.Text == "1" || lblDefAccount.Text == "True")
                {
                    chkgvDefaultAccount.Checked = true;
                }
                else
                {
                    chkgvDefaultAccount.Checked = false;
                }
            }
            if (strMode != "Q")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    /* For Deleting purpose, Restrcit to add attribute to the Cell Remove linkbutton*/
                    for (int intCellIndex = 0; intCellIndex < e.Row.Cells.Count - 2; intCellIndex++)
                    {
                        e.Row.Cells[intCellIndex].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                        (this.grvBankDetails, "Select$" + e.Row.RowIndex);
                    }

                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";


                    //LinkButton LnkBtn = (LinkButton)e.Row.FindControl("lnkbtnDelete");
                    //LnkBtn.CommandArgument = e.Row.RowIndex.ToString();
                    //LnkBtn.Attributes.Add("onclick", "return confirm(\"Do you want to delete?\")");
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "S3GORGENTITYMASTER.ASPX");
            throw new ApplicationException("Unable to Load the Bank Details");
        }
    }


    protected void btnAddAddress_OnClick(object sender, EventArgs e)
    {
        if (txtAddress.Text.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "Address is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtAddress.Focus();
            return;
        }


        if (txtCity.SelectedText.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "City is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtCity.Focus();
            return;
        }

        if (ddlComState.SelectedValue.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "State is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            ddlComState.Focus();
            return;
        }

        ///*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        
        //        if (txtLoc_Code.Text.Trim() == "")
        //        {
        //            Utility.FunShowAlertMsg(this, "Location Code is mandatory");
        //            tcEntityMaster.ActiveTab = tbEntity;
        //            txtLoc_Code.Focus();
        //            return;
        //        }
        ///*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        

        if (txtCountry.SelectedValue.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "Country is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtCountry.Focus();
            return;
        }

        /* Swarna - related to mail dated 21st sep 2015*/

        if (ddlResState.Enabled && (ddlResState.SelectedValue == "0" || ddlResState.SelectedValue == "-1"))
        {
            Utility.FunShowAlertMsg(this, "Address State is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtCountry.Focus();
            return;
        }

        //if (txtPINCode.Text.Trim() == "")
        //{
        //    Utility.FunShowAlertMsg(this, "Pincode is mandatory");
        //    tcEntityMaster.ActiveTab = tbEntity;
        //    txtPINCode.Focus();
        //    return;
        //}

        //if (txtVATTIN.Text.Trim() == "")
        //{
        //    Utility.FunShowAlertMsg(this, "VAT-TIN is mandatory");
        //    tcEntityMaster.ActiveTab = tbEntity;
        //    txtVATTIN.Focus();
        //    return;
        //}
        //else
        //if (txtVATTIN.Text.Trim() != "")
        //{
        //    if (txtVATTIN.Text.Length != 20)
        //    {
        //        Utility.FunShowAlertMsg(this, "VAT-TIN should be of 20 characters");
        //        tcEntityMaster.ActiveTab = tbEntity;
        //        txtVATTIN.Focus();
        //        return;
        //    }
        //foreach (GridViewRow row in gvBAddress.Rows)
        //{
        if (txtSGSTin.Text.Trim() != "")
        {
            //    {
            //        Label lblVATTIN = (Label)row.Cells[9].FindControl("lblVATTIN");
            //        Label lblState = (Label)row.Cells[3].FindControl("lblState");
            //        if (txtVATTIN.Text.Trim() == lblVATTIN.Text.ToString())
            //        {
            //            if (ddlComState.SelectedItem.ToString() != lblState.Text.ToString())
            //            {
            //                Utility.FunShowAlertMsg(this, "VAT-TIN Number already defined for other state");
            //                tcEntityMaster.ActiveTab = tbEntity;
            //                txtVATTIN.Focus();
            //                return;
            //            }
            foreach (GridViewRow row in gvBAddress.Rows)
            {
                Label lblSGSTIN = (Label)row.Cells[9].FindControl("lblSGSTIN");
                Label lblState = (Label)row.Cells[3].FindControl("lblState");
                if (txtSGSTin.Text.Trim() == lblSGSTIN.Text.ToString())
                {
                    if (ddlComState.SelectedItem.ToString() != lblState.Text.ToString())
                    {
                        Utility.FunShowAlertMsg(this, "GSTIN already defined for other state");
                        tcEntityMaster.ActiveTab = tbEntity;
                        txtSGSTin.Focus();
                        return;
                    }
                }
            }
        }

        if (txtSGSTin.Text.Trim() == "" && ddlVendorResidentialStatus.SelectedValue == "1")
        {
            Utility.FunShowAlertMsg(this, "GSTIN is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtSGSTin.Focus();
            return;
        }


        //if (txtTAN.Text.Trim() != "")
        //{
        //    foreach (GridViewRow row in gvBAddress.Rows)
        //    {
        //        Label lblTAN = (Label)row.Cells[11].FindControl("lblTAN");
        //        Label lblState = (Label)row.Cells[3].FindControl("lblState");
        //        if (txtTAN.Text.Trim() == lblTAN.Text.ToString())
        //        {
        //            if (ddlComState.SelectedItem.ToString() != lblState.Text.ToString())
        //            {
        //                Utility.FunShowAlertMsg(this, "TAN Number already defined for other state");
        //                tcEntityMaster.ActiveTab = tbEntity;
        //                txtTAN.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}

        //if (txtCSTTIN.Text.Trim() == "")
        //{
        //    Utility.FunShowAlertMsg(this, "CST-TIN is mandatory");
        //    tcEntityMaster.ActiveTab = tbEntity;
        //    txtCSTTIN.Focus();
        //    return;
        //}
        //if (txtCSTTIN.Text.Trim() != "")
        //{
        //    if (txtCSTTIN.Text.Length != 20)
        //    {
        //        Utility.FunShowAlertMsg(this, "CST-TIN should be of 20 characters");
        //        tcEntityMaster.ActiveTab = tbEntity;
        //        txtCSTTIN.Focus();
        //        return;
        //    }
        //    foreach (GridViewRow row in gvBAddress.Rows)
        //    {
        //        Label lblCSTTIN = (Label)row.Cells[10].FindControl("lblCSTTIN");
        //        Label lblState = (Label)row.Cells[3].FindControl("lblState");
        //        if (txtCSTTIN.Text.Trim() == lblCSTTIN.Text.ToString())
        //        {
        //            if (ddlComState.SelectedItem.ToString() != lblState.Text.ToString())
        //            {
        //                Utility.FunShowAlertMsg(this, "CST-TIN Number already defined for other state");
        //                tcEntityMaster.ActiveTab = tbEntity;
        //                txtCSTTIN.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}

        var withoutSpecial = new string(txtCountry.Text.Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c)).ToArray());

        if (txtCountry.Text != withoutSpecial)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Special Characters Not allowed in Country field');", true);
            tcEntityMaster.ActiveTab = tbEntity;
            txtCountry.Focus();
            return;
        }

        ///*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/                
        //foreach (GridViewRow row in gvBAddress.Rows)
        //{            
        //    Label lblLoc_Code = (Label)row.FindControl("lblLoc_Code");
        //    if (txtLoc_Code.Text.Trim() == lblLoc_Code.Text.ToString())
        //    {
        //        Utility.FunShowAlertMsg(this, "Location Code already exists");
        //        txtLoc_Code.Focus();
        //        return;
        //    }
        //}
        ///*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        

        foreach (GridViewRow row in gvBAddress.Rows)
        {
            Label lblAddress1 = (Label)row.FindControl("lblAddress1");
            Label lblStateCode = (Label)row.FindControl("lblStateCode");
            Label lblCity = (Label)row.FindControl("lblCity");
            /*Included Address 1 to check duplication of vendor address based on req id 3548 on may 4,2016 by vinodha m*/
            if (txtAddress.Text.Trim() == lblAddress1.Text.ToString() && ddlComState.SelectedValue.Trim() == lblStateCode.Text.ToString() && txtCity.SelectedText.Trim() == lblCity.Text.ToString())
            {
                Utility.FunShowAlertMsg(this, "Vendor combination already exists");
                ddlComState.Focus();
                return;
            }
            /*Included Address 1 to check duplication of vendor address based on req id 3548 on may 4,2016 by vinodha m*/
        }



        DataRow drDetails;
        DataTable dtBankDetails = new DataTable();
        DataTable dtExist = new DataTable();

        if (ViewState["BillingAddress"] != null)
        {
            dtBankDetails = (DataTable)ViewState["BillingAddress"];
            drDetails = dtBankDetails.NewRow();
            string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
            drDetails["Rowno"] = strBankMapId;
            drDetails["RES_ADD"] = ddlResidentialStatus.SelectedValue.ToString();
            if (ddlStateRegStatus.Visible == true)
            {
                drDetails["REG_ADD"] = ddlStateRegStatus.SelectedValue.ToString();
                drDetails["REGSTATUS"] = ddlStateRegStatus.SelectedItem.ToString();
            }
            else
            {
                drDetails["REG_ADD"] = lblShowRegID.Text;
                drDetails["REGSTATUS"] = lblShowRegStatus.Text;
            }
            drDetails["NAME"] = ddlResidentialStatus.SelectedItem.ToString();
            drDetails["Address1"] = txtAddress.Text.Trim();
            drDetails["Address2"] = txtAddress2.Text.Trim();
            drDetails["City"] = txtCity.SelectedText.Trim();
            drDetails["State"] = ddlComState.SelectedItem.ToString();
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            drDetails["Loc_Code"] = txtLoc_Code.Text.ToString();
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            drDetails["Location_Category_ID"] = ddlComState.SelectedValue.ToString();
            drDetails["Country"] = txtCountry.SelectedItem.ToString();
            if (ddlResState.SelectedValue != "0" && ddlResState.SelectedValue != "-1")
            {
                drDetails["Resident_State"] = ddlResState.SelectedItem.ToString();
                drDetails["Resident_State_ID"] = ddlResState.SelectedValue.ToString();
            }
            else
            {
                drDetails["Resident_State"] = "";
                drDetails["Resident_State_ID"] = "0";
            }
            if (txtMobile.Text.Trim() != "")
            {
                drDetails["Mobile"] = txtMobile.Text.Trim();
            }
            else
            {
                drDetails["Mobile"] = 0;
            }
            drDetails["Pincode"] = txtPINCode.Text.Trim();
            drDetails["Telephone"] = txtTelephone.Text.Trim();
            drDetails["Email"] = txtVEndorEmailId.Text.Trim();
            drDetails["VAT_Number"] = txtVATTIN.Text.Trim();
            drDetails["CST_TIN"] = txtCSTTIN.Text.Trim();
            drDetails["Tax_Account_Number"] = txtTAN.Text.Trim();
            drDetails["SGSTIN"] = txtSGSTin.Text.Trim();
            drDetails["SGST_Reg_Date"] = TxtSGSTRegDate.Text.Trim();
        }
        else
        {
            dtBankDetails = new DataTable();
            dtBankDetails.Columns.Add("Rowno");
            dtBankDetails.Columns.Add("RES_ADD");
            dtBankDetails.Columns.Add("REG_ADD");
            dtBankDetails.Columns.Add("NAME");
            dtBankDetails.Columns.Add("REGSTATUS");
            dtBankDetails.Columns.Add("Address1");
            dtBankDetails.Columns.Add("Address2");
            dtBankDetails.Columns.Add("City");
            dtBankDetails.Columns.Add("State");
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            dtBankDetails.Columns.Add("Loc_Code");
            dtBankDetails.Columns.Add("Address_ID");
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            dtBankDetails.Columns.Add("Location_Category_ID");
            dtBankDetails.Columns.Add("Country");
            dtBankDetails.Columns.Add("Resident_State");
            dtBankDetails.Columns.Add("Resident_State_ID");
            dtBankDetails.Columns.Add("Pincode");
            dtBankDetails.Columns.Add("Telephone");
            dtBankDetails.Columns.Add("Mobile");
            dtBankDetails.Columns.Add("Email");
            dtBankDetails.Columns.Add("VAT_Number");
            dtBankDetails.Columns.Add("CST_TIN");
            dtBankDetails.Columns.Add("Tax_Account_Number");
            dtBankDetails.Columns.Add("SGSTIN");
            dtBankDetails.Columns.Add("SGST_Reg_Date");

            drDetails = dtBankDetails.NewRow();
            string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
            drDetails["Rowno"] = strBankMapId;
            drDetails["RES_ADD"] = ddlResidentialStatus.SelectedValue.ToString();
            if (ddlStateRegStatus.Visible == true)
            {
                drDetails["REG_ADD"] = ddlStateRegStatus.SelectedValue.ToString();
                drDetails["REGSTATUS"] = ddlStateRegStatus.SelectedItem.ToString();
            }
            else
            {
                drDetails["REG_ADD"] = lblShowRegID.Text;
                drDetails["REGSTATUS"] = lblShowRegStatus.Text;
            }
            drDetails["NAME"] = ddlResidentialStatus.SelectedItem.ToString();

            drDetails["Address1"] = txtAddress.Text.Trim();
            drDetails["Address2"] = txtAddress2.Text.Trim();
            drDetails["City"] = txtCity.SelectedText.Trim();
            drDetails["State"] = ddlComState.SelectedItem.ToString();
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            drDetails["Loc_Code"] = txtLoc_Code.Text;
            drDetails["Address_ID"] = String.Empty;
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            drDetails["Location_Category_ID"] = ddlComState.SelectedValue.ToString();
            drDetails["Country"] = txtCountry.SelectedItem.ToString();
            if (ddlResState.SelectedValue != "0" && ddlResState.SelectedValue != "-1")
            {
                drDetails["Resident_State"] = ddlResState.SelectedItem.ToString();
                drDetails["Resident_State_ID"] = ddlResState.SelectedValue.ToString();
            }
            else
            {
                drDetails["Resident_State"] = "";
                drDetails["Resident_State_ID"] = "0";
            }
            drDetails["Pincode"] = txtPINCode.Text.Trim();
            drDetails["Telephone"] = txtTelephone.Text.Trim();
            drDetails["Mobile"] = txtMobile.Text.Trim();
            drDetails["Email"] = txtVEndorEmailId.Text.Trim();
            drDetails["VAT_Number"] = txtVATTIN.Text.Trim();
            drDetails["CST_TIN"] = txtCSTTIN.Text.Trim();
            drDetails["Tax_Account_Number"] = txtTAN.Text.Trim();
            drDetails["SGSTIN"] = txtSGSTin.Text.Trim();
            drDetails["SGST_Reg_Date"] = TxtSGSTRegDate.Text.Trim();

        }
        dtBankDetails.Rows.Add(drDetails);
        gvBAddress.DataSource = dtBankDetails;
        gvBAddress.DataBind();
        ViewState["BillingAddress"] = dtBankDetails;

        RequiredFieldValidator1.Enabled = true;
        ddlStateRegStatus.Visible = true;
        lblShowRegStatus.Visible = false;

        txtAddress.Text = "";
        txtAddress2.Text = "";
        txtCity.Clear();
        ddlComState.SelectedIndex = 0;
        ddlResState.SelectedIndex = 0;
        /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
        txtLoc_Code.Text = "";
        /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
        txtCountry.SelectedIndex = 0;
        ddlResidentialStatus.SelectedIndex = 0;
        ddlStateRegStatus.SelectedIndex = 0;
        txtPINCode.Text = "";
        txtTelephone.Text = "";
        txtMobile.Text = "";
        txtVEndorEmailId.Text = "";
        txtVATTIN.Text = "";
        txtCSTTIN.Text = "";
        txtTAN.Text = "";
        txtSGSTin.Text = "";
        TxtSGSTRegDate.Text = "";
        try
        {
            txtCountry.SelectedItem.Text = "India";
        }
        catch (Exception ex)
        {
            txtCountry.SelectedIndex = 0;
        }
    }





    protected void btnModifyAddress_OnClick(object sender, EventArgs e)
    {
        if (txtAddress.Text.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "Address is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtAddress.Focus();
            return;
        }


        if (txtCity.SelectedText.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "City is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtCity.Focus();
            return;
        }

        if (ddlComState.SelectedValue.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "State is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            ddlComState.Focus();
            return;
        }

        if (txtCountry.SelectedValue.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "Country is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtCountry.Focus();
            return;
        }

        /* Swarna - related to mail dated 21st sep 2015*/

        if (ddlResState.Enabled && (ddlResState.SelectedValue == "0" || ddlResState.SelectedValue == "-1"))
        {
            Utility.FunShowAlertMsg(this, "Address State is mandatory");
            tcEntityMaster.ActiveTab = tbEntity;
            txtCountry.Focus();
            return;
        }

        //if (txtPINCode.Text.Trim() == "")
        //{
        //    Utility.FunShowAlertMsg(this, "Pincode is mandatory");
        //    tcEntityMaster.ActiveTab = tbEntity;
        //    txtPINCode.Focus();
        //    return;
        //}

        //if (txtVATTIN.Text.Trim() == "")
        //{
        //    Utility.FunShowAlertMsg(this, "VAT-TIN is mandatory");
        //    tcEntityMaster.ActiveTab = tbEntity;
        //    txtVATTIN.Focus();
        //    return;
        //}
        //else
        //if (txtVATTIN.Text.Trim() != "")
        //{
        //    if (txtVATTIN.Text.Length != 20)
        //    {
        //        Utility.FunShowAlertMsg(this, "VAT-TIN should be of 20 characters");
        //        tcEntityMaster.ActiveTab = tbEntity;
        //        txtVATTIN.Focus();
        //        return;
        //    }
            //foreach (GridViewRow row in gvBAddress.Rows)
            //{
        if (txtSGSTin.Text.Trim() != "")
        {
        //    {
        //        Label lblVATTIN = (Label)row.Cells[9].FindControl("lblVATTIN");
        //        Label lblState = (Label)row.Cells[3].FindControl("lblState");
        //        if (txtVATTIN.Text.Trim() == lblVATTIN.Text.ToString())
        //        {
        //            if (ddlComState.SelectedItem.ToString() != lblState.Text.ToString())
        //            {
        //                Utility.FunShowAlertMsg(this, "VAT-TIN Number already defined for other state");
        //                tcEntityMaster.ActiveTab = tbEntity;
        //                txtVATTIN.Focus();
        //                return;
        //            }
            foreach (GridViewRow row in gvBAddress.Rows)
            {
                Label lblSGSTIN = (Label)row.Cells[9].FindControl("lblSGSTIN");
                Label lblState = (Label)row.Cells[3].FindControl("lblState");
                if (txtSGSTin.Text.Trim() == lblSGSTIN.Text.ToString())
                {
                    if (ddlComState.SelectedItem.ToString() != lblState.Text.ToString())
                    {
                        Utility.FunShowAlertMsg(this, "GSTIN already defined for other state");
                        tcEntityMaster.ActiveTab = tbEntity;
                        txtSGSTin.Focus();
                        return;
                    }
                }
            }
        }

        //if (txtCSTTIN.Text.Trim() == "")
        //{
        //    Utility.FunShowAlertMsg(this, "CST-TIN is mandatory");
        //    tcEntityMaster.ActiveTab = tbEntity;
        //    txtCSTTIN.Focus();
        //    return;
        //}
        //else
        //if (txtCSTTIN.Text.Trim() != "")
        //{
        //    if (txtCSTTIN.Text.Length != 20)
        //    {
        //        Utility.FunShowAlertMsg(this, "CST-TIN should be of 20 characters");
        //        tcEntityMaster.ActiveTab = tbEntity;
        //        txtCSTTIN.Focus();
        //        return;
        //    }
        //    foreach (GridViewRow row in gvBAddress.Rows)
        //    {
        //        Label lblCSTTIN = (Label)row.Cells[10].FindControl("lblCSTTIN");
        //        Label lblState = (Label)row.Cells[3].FindControl("lblState");
        //        if (txtCSTTIN.Text.Trim() == lblCSTTIN.Text.ToString())
        //        {
        //            if (ddlComState.SelectedItem.ToString() != lblState.Text.ToString())
        //            {
        //                Utility.FunShowAlertMsg(this, "CST-TIN Number already defined for other state");
        //                tcEntityMaster.ActiveTab = tbEntity;
        //                txtCSTTIN.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}

        //if (txtTAN.Text.Trim() != "")
        //{
        //    foreach (GridViewRow row in gvBAddress.Rows)
        //    {
        //        Label lblTAN = (Label)row.Cells[11].FindControl("lblTAN");
        //        Label lblState = (Label)row.Cells[3].FindControl("lblState");
        //        if (txtTAN.Text.Trim() == lblTAN.Text.ToString())
        //        {
        //            if (ddlComState.SelectedItem.ToString() != lblState.Text.ToString())
        //            {
        //                Utility.FunShowAlertMsg(this, "TAN Number already defined for other state");
        //                tcEntityMaster.ActiveTab = tbEntity;
        //                txtTAN.Focus();
        //                return;
        //            }
        //        }
        //    }
        //}

        var withoutSpecial = new string(txtCountry.Text.Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c)).ToArray());

        if (txtCountry.Text != withoutSpecial)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Special Characters Not allowed in Country field');", true);
            tcEntityMaster.ActiveTab = tbEntity;
            txtCountry.Focus();
            return;
        }

        foreach (GridViewRow row in gvBAddress.Rows)
        {
            HiddenField gvhentityadrz_id = (HiddenField)row.FindControl("gvhentityadrz_id");
            Label lblAddress1 = (Label)row.FindControl("lblAddress1");
            Label lblStateCode = (Label)row.FindControl("lblStateCode");
            Label lblCity = (Label)row.FindControl("lblCity");
            if (hentadrz_id.Value != gvhentityadrz_id.Value && txtAddress.Text.Trim() == lblAddress1.Text.ToString() && ddlComState.SelectedValue.Trim() == lblStateCode.Text.ToString() && txtCity.SelectedText.Trim() == lblCity.Text.ToString())
            {
                Utility.FunShowAlertMsg(this, "Vendor combination already exists");
                ddlComState.Focus();
                hentadrz_id.Value = String.Empty;
                return;
            }
        }


        ///*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        
        //foreach (GridViewRow row in gvBAddress.Rows)
        //{
        //    Label lblLoc_Code = (Label)row.FindControl("lblLoc_Code");
        //    HiddenField gvhentityadrz_id = (HiddenField)row.FindControl("gvhentityadrz_id");
        //    if (hvEntityAdrz_ID.Value != gvhentityadrz_id.Value && txtLoc_Code.Text.Trim() == lblLoc_Code.Text.ToString())
        //    {
        //        Utility.FunShowAlertMsg(this, "Location Code already exists");
        //        txtLoc_Code.Focus();
        //        return;
        //    }
        //}
        ///*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        

        try
        {

            DataTable dtBankDetails = (DataTable)ViewState["BillingAddress"];
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            //DataRow[] drDefault = dtBankDetails.Select("IsDefaultAccount = 'True' and BankMapping_ID <> '" + hdnBankId.Value + "'");
            //if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            //{
            //    Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
            //    return;
            //}

            DataRow[] customerRow = dtBankDetails.Select("Location_Category_ID = " + ddlComState.SelectedValue.ToString());
            if (customerRow.Length > 0)
            {
                for (int i = 0; i < customerRow.Length; i++)
                {
                    customerRow[i]["REGSTATUS"] = ddlStateRegStatus.SelectedItem.ToString();
                    customerRow[i]["REG_ADD"] = ddlStateRegStatus.SelectedValue.ToString();
                }
            }

            dtBankDetails.AcceptChanges();


          
            DataView dvBankdetails = dtBankDetails.DefaultView;
            dvBankdetails.Sort = "Rowno";
            int rowindex = Convert.ToInt16(hdnAddID.Value);

            dvBankdetails[rowindex].Row["Location_Category_ID"] = ddlComState.SelectedValue.ToString();
            dvBankdetails[rowindex].Row["State"] = ddlComState.SelectedItem.ToString();
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        
            dvBankdetails[rowindex].Row["Loc_Code"] = txtLoc_Code.Text.ToString();
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        
            dvBankdetails[rowindex].Row["City"] = txtCity.SelectedText.Trim();
            dvBankdetails[rowindex].Row["RES_ADD"] = ddlResidentialStatus.SelectedValue.ToString();
            if (ddlStateRegStatus.Visible == true)
            {
                dvBankdetails[rowindex].Row["REG_ADD"] = ddlStateRegStatus.SelectedValue.ToString();
                dvBankdetails[rowindex].Row["REGSTATUS"] = ddlStateRegStatus.SelectedItem.ToString();
            }
            else
            {
                dvBankdetails[rowindex].Row["REG_ADD"] = lblShowRegID.Text;
                dvBankdetails[rowindex].Row["REGSTATUS"] = lblShowRegStatus.Text;
            }
            dvBankdetails[rowindex].Row["NAME"] = ddlResidentialStatus.SelectedItem.ToString();
           
            dvBankdetails[rowindex].Row["Address1"] = txtAddress.Text.Trim();
            dvBankdetails[rowindex].Row["Address2"] = txtAddress2.Text.Trim();
            dvBankdetails[rowindex].Row["Pincode"] = txtPINCode.Text.Trim();
            dvBankdetails[rowindex].Row["Email"] = txtVEndorEmailId.Text.Trim();
            dvBankdetails[rowindex].Row["Country"] = txtCountry.SelectedItem.ToString();
            
            if (ddlResState.SelectedValue != "0" && ddlResState.SelectedValue != "-1")
            {
                dvBankdetails[rowindex].Row["Resident_State"] = ddlResState.SelectedItem.ToString();
                dvBankdetails[rowindex].Row["Resident_State_ID"] = ddlResState.SelectedValue.ToString();
            }
            else
            {
                dvBankdetails[rowindex].Row["Resident_State"] = "";
                dvBankdetails[rowindex].Row["Resident_State_ID"] = "0";
            }
            dvBankdetails[rowindex].Row["Telephone"] = txtTelephone.Text.Trim();
            if (txtMobile.Text.Trim() == "")
            {
                dvBankdetails[rowindex].Row["Mobile"] = 0;
            }
            else
            {
                dvBankdetails[rowindex].Row["Mobile"] = txtMobile.Text.Trim();
            }

            dvBankdetails[rowindex].Row["VAT_Number"] = txtVATTIN.Text.Trim();
            dvBankdetails[rowindex].Row["CST_TIN"] = txtCSTTIN.Text.Trim();
            dvBankdetails[rowindex].Row["Tax_Account_Number"] = txtTAN.Text.Trim();
            dvBankdetails[rowindex].Row["SGSTIN"] = txtSGSTin.Text.Trim();
            dvBankdetails[rowindex].Row["SGST_Reg_Date"] = TxtSGSTRegDate.Text.Trim();

            gvBAddress.DataSource = dvBankdetails;
            gvBAddress.DataBind();
            ViewState["BillingAddress"] = dvBankdetails.Table;
            btnAddAddress.Enabled = true;
            btnModifyAddress.Enabled = false;

            RequiredFieldValidator1.Enabled = true;
            ddlStateRegStatus.Visible = true;
            lblShowRegStatus.Visible = false;

            ddlResidentialStatus.SelectedIndex = 0;
            ddlStateRegStatus.SelectedIndex = 0;
            txtAddress.Text = "";
            txtAddress2.Text = "";
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        
            txtLoc_Code.Text = "";
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/        
            txtCity.Clear();
            ddlComState.SelectedIndex = 0;
            ddlResState.SelectedIndex = 0;
            txtCountry.SelectedIndex = 0;
            txtPINCode.Text = "";
            txtTelephone.Text = "";
            //txtWebsite.Text = "";
            txtMobile.Text = "";
            txtVEndorEmailId.Text = "";
            txtVATTIN.Text = "";
            txtCSTTIN.Text = "";
            txtTAN.Text = "";
            txtSGSTin.Text = "";
            TxtSGSTRegDate.Text = "";
            try
            {
                txtCountry.SelectedItem.Text = "India";
            }
            catch (Exception ex)
            {
                txtCountry.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvEntityMaster.IsValid = false;
            cvEntityMaster.ErrorMessage = "Unable to modify address details";

        }
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtEntityCode.Text = "";
            ddlEntityType.SelectedIndex = 0;
            //ddlGLPostingCode.SelectedIndex = 0;
            txtEntityName.Text = "";
            ddlResidentialStatus.SelectedIndex = 0;
            ddlStateRegStatus.SelectedIndex = 0;
            txtAddress.Text = "";
            txtAddress2.Text = "";
            txtCity.Clear();
            txtCountry.SelectedIndex = 0;
            ddlComState.SelectedIndex = 0;
            ddlResState.SelectedIndex = 0;
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            txtLoc_Code.Text = "";
            /*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            txtPAN.Text = "";
            txtCIN.Text = "";
            txtVATTIN.Text = "";
            txtVEndorEmailId.Text = "";
            txtMobile.Text = "";
            txtPINCode.Text = "";
            txtTaxNumber.Text = "";
            txtTelephone.Text = "";
            txtWebsite.Text = "";
            ddlVendorResidentialStatus.SelectedIndex = 0;
            txtCSTTIN.Text = "";
            txtTAN.Text = "";
            tcEntityMaster.ActiveTabIndex = 0;
            gvBAddress.DataSource = null;
            gvBAddress.DataBind();
            ViewState["BillingAddress"] = null;
            grvBankDetails.DataSource = null;
            grvBankDetails.DataBind();
            ViewState["DetailsTable"] = null;
            FunProIntializeData();

            ddlComposition.SelectedIndex = 0;
            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = "";
            txtBankName.Text = "";
            txtBranchName.Text = "";
            txtMICRCode.Text = "";
            txtBankAddress.Text = "";
            txtMICRCode.Text = "";
            txtIFSC_Code.Text = "";
            ddlAddState.SelectedIndex = 0;
            txtCGSTin.Text = "";
            TxtCGSTRegDate.Text = "";
            TxtSGSTRegDate.Text = "";
            txtSGSTin.Text = "";

            ddlMSMERegistered.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            ddlCertificateReceived.SelectedIndex = 0;
            ddlEInvoice.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvEntityMaster.IsValid = false;
            cvEntityMaster.ErrorMessage = "Unable to clear data";
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3gOrgEntityMaster_View.aspx");
    }

    protected void btnBnkClear_Click(object sender, EventArgs e)
    {
        btnBnkAdd.Enabled = true;
        //ddlBranch.SelectedIndex = 0;
        ddlAccountType.SelectedIndex = 0;
        txtAccountNumber.Text = "";
        txtBankName.Text = "";
        txtBranchName.Text = "";
        txtMICRCode.Text = "";
        txtBankAddress.Text = "";
        hdnBankId.Value = "";
        btnBnkModify.Enabled = false;
        chkDefaultAccount.Checked = false;
        txtIFSC_Code.Text = "";
        ddlAddState.SelectedIndex = 0;
    }
    #endregion

    #region Page Methods
    /// <summary>
    /// Methos to get Vendor details
    /// </summary>
    /// <param name="intCompanyId">Company Id to whcich Vendor belongs</param>
    /// <param name="intEntityId">Vendor Id for which dat to be obtained</param>
    /// <param name="blnTranExists">Boolen to check trans exists for Enityt selected</param>
    public void FunPubProGetEntityDetails(int intCompanyId, int intEntityId, out bool blnTranExists)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Option", "4");
            //Procparam.Add("@Param1", Convert.ToString(intEntityId));
            //DataTable dt = Utility.GetDefaultData("S3G_ORG_CheckTINTAN", Procparam);
            //Procparam.Clear();

            //ViewState["BillingAddress"] = dt;
            //gvBAddress.DataSource = dt;
            //gvBAddress.DataBind();

            DataSet dsEntityDetails;
            //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            byte[] byte_EntityDetails = ObjOrgMasterMgtServicesClient.FunPubQueryEntityDetails(out blnTranExists, intCompanyId, intEntityId);
            dsEntityDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_EntityDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsEntityDetails.Tables.Count > 0 && intCompanyId > 0)
            {
                DataTable dtEntityCode = dsEntityDetails.Tables[0];
                txtEntityCode.Text = dtEntityCode.Rows[0]["Entity_Code"].ToString();
                ListItem lst;
                if (PageMode == PageModes.Query)
                {
                    lst = new ListItem(dtEntityCode.Rows[0]["Entity_Type_Name"].ToString(), dtEntityCode.Rows[0]["Entity_Type"].ToString());
                    ddlEntityType.Items.Add(lst);

                    lst = new ListItem(dtEntityCode.Rows[0]["ACCOUNT_CODE_DESC"].ToString(), dtEntityCode.Rows[0]["GL_Code"].ToString());
                    ddlGLPostingCode.Items.Add(lst);

                    lst = new ListItem(dtEntityCode.Rows[0]["COMPANYTYPE"].ToString(), dtEntityCode.Rows[0]["COMPANYTYPE"].ToString());
                    ddlCompanyType.Items.Add(lst);

                    //lst = new ListItem(dtEntityCode.Rows[0]["REG_NAME"].ToString(), dtEntityCode.Rows[0]["REG_VALUE"].ToString());
                    //ddlStateRegStatus.Items.Add(lst);

                    lst = new ListItem(dtEntityCode.Rows[0]["ConstitutionName"].ToString(), dtEntityCode.Rows[0]["Constitution_ID"].ToString());
                    ddlConstitutionName.Items.Add(lst);

                    lst = new ListItem(dtEntityCode.Rows[0]["RESIDENTIALTYPE"].ToString(), dtEntityCode.Rows[0]["RESIDENTIALTYPE"].ToString());
                    ddlVendorResidentialStatus.Items.Add(lst);
                }
                try
                {
                    ddlEntityType.SelectedValue = dtEntityCode.Rows[0]["Entity_Type"].ToString();
                }
                catch (Exception ex)
                {
                    ddlEntityType.SelectedIndex = 0;
                }

                ddlCompanyType.SelectedValue = dtEntityCode.Rows[0]["COMPTYPE"].ToString();
                //ddlRegStatus.SelectedValue = dtEntityCode.Rows[0]["REG_VALUE"].ToString();
                ddlConstitutionName.SelectedValue = dtEntityCode.Rows[0]["Constitution_ID"].ToString();
                ddlGLPostingCode.SelectedValue = dtEntityCode.Rows[0]["GL_Code"].ToString();

                if (ddlGLPostingCode.SelectedValue == "0")
                {
                    string str = dtEntityCode.Rows[0]["GL_Code"].ToString() + " - " + dtEntityCode.Rows[0]["ACCOUNT_CODE_DESC"].ToString();
                    ddlGLPostingCode.Items.Insert(1, new ListItem(str, dtEntityCode.Rows[0]["GL_Code"].ToString()));
                    ddlGLPostingCode.SelectedValue = dtEntityCode.Rows[0]["GL_Code"].ToString();
                }
                txtTaxNumber.Text = dtEntityCode.Rows[0]["Service_Tax_Number"].ToString();
                txtEntityName.Text = dtEntityCode.Rows[0]["Entity_Name"].ToString();
                txtPAN.Text = dtEntityCode.Rows[0]["PAN"].ToString();
                txtCIN.Text = dtEntityCode.Rows[0]["CIN"].ToString();
                hdnID.Value = dtEntityCode.Rows[0]["USRID"].ToString();
                txtWebsite.Text = dtEntityCode.Rows[0]["Website"].ToString();
                ddlVendorResidentialStatus.SelectedValue = dtEntityCode.Rows[0]["Res_Type"].ToString();
                txtCGSTin.Text = dtEntityCode.Rows[0]["CGSTIN"].ToString();
                TxtCGSTRegDate.Text = dtEntityCode.Rows[0]["CGST_Reg_Date"].ToString();
                ddlComposition.SelectedValue = dtEntityCode.Rows[0]["Composition"].ToString();

                ddlMSMERegistered.SelectedValue = dtEntityCode.Rows[0]["MSME_Registered"].ToString();
                ddlType.SelectedValue = dtEntityCode.Rows[0]["Vendor_Type"].ToString();
                ddlCertificateReceived.SelectedValue = dtEntityCode.Rows[0]["Certificate_Received"].ToString();

                ddlEInvoice.SelectedValue = dtEntityCode.Rows[0]["EInvoice"].ToString();

                if (ddlMSMERegistered.SelectedValue == "1")
                {
                    ddlType.Enabled = ddlCertificateReceived.Enabled = true;
                }
                else
                {
                    ddlType.SelectedIndex = ddlCertificateReceived.SelectedIndex = 0;
                    ddlType.Enabled = ddlCertificateReceived.Enabled = false;
                }

                DataTable dtBankDetails = dsEntityDetails.Tables[1];
                ViewState["DetailsTable"] = dtBankDetails;
                grvBankDetails.DataSource = dtBankDetails;
                grvBankDetails.DataBind();


                DataTable dtAddressDetails = dsEntityDetails.Tables[2];
                ViewState["BillingAddress"] = dtAddressDetails;
                gvBAddress.DataSource = dtAddressDetails;
                gvBAddress.DataBind();
                if (grvBankDetails.Rows.Count != 0)
                {
                    hdnBankId.Value = Convert.ToString(grvBankDetails.Rows[0].Cells[0].Text);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Vendor details");
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();

        }
    }
    /// <summary>
    /// Method to initialize then data datatable
    /// </summary>
    protected void FunProIntializeData()
    {
        try
        {
            DataTable dtBankViewDetails;
            dtBankViewDetails = new DataTable("BankDetails");
            dtBankViewDetails.Columns.Add("Rowno");
            dtBankViewDetails.Columns.Add("Account_type");
            dtBankViewDetails.Columns.Add("AccountType_ID");
            dtBankViewDetails.Columns.Add("Account_Number");
            dtBankViewDetails.Columns.Add("Bank_Name");
            dtBankViewDetails.Columns.Add("Branch_Name");
            dtBankViewDetails.Columns.Add("MICR_Code");
            dtBankViewDetails.Columns.Add("Branch_Address");
            dtBankViewDetails.Columns.Add("Account_Id");
            dtBankViewDetails.Columns.Add("State");
            dtBankViewDetails.Columns.Add("Location_Category_ID");
            dtBankViewDetails.Columns.Add("IFSC_code");
            dtBankViewDetails.Columns.Add("Is_Default_Account");
            ViewState["DetailsTable"] = dtBankViewDetails;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to Intialize data");
        }
    }


    protected void gvBAddress_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (strMode != "Q")
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    /* For Deleting purpose, Restrcit to add attribute to the Cell Remove linkbutton*/
                    for (int intCellIndex = 0; intCellIndex < e.Row.Cells.Count - 2; intCellIndex++)
                    {
                        e.Row.Cells[intCellIndex].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                        (this.gvBAddress, "Select$" + e.Row.RowIndex);
                    }
                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";

                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMobNo = (Label)e.Row.FindControl("lblMobNo");
                if (lblMobNo.Text == "" || lblMobNo.Text == "0")
                {
                    lblMobNo.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "S3GORGENTITYMASTER.ASPX");
            throw new ApplicationException("Unable to Load the Bank Details");
        }
    }

    protected void gvBAddress_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete = (DataTable)ViewState["BillingAddress"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            gvBAddress.DataSource = dtDelete;
            gvBAddress.DataBind();
            ViewState["BillingAddress"] = dtDelete;
            if (gvBAddress.Rows.Count == 0)
            {
                btnAddAddress.Enabled = true;
                btnModifyAddress.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvEntityMaster.IsValid = false;
            cvEntityMaster.ErrorMessage = "Unable to remove address details";
        }
    }

    private void FunPriEntityControlStatus(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode
                try
                {
                    txtCountry.SelectedValue = "India";
                }
                catch (Exception ex)
                {
                    txtCountry.SelectedIndex = 0;
                }

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                btnBnkModify.Enabled = false;
                btnBnkAdd.Enabled = true;
                btnModifyAddress.Enabled = false;
                btnAddAddress.Enabled = true;
                //txtEntityCode.Enabled = true;
                txtEntityName.Enabled = true;
                txtPAN.Enabled = true;
                txtCIN.Enabled = true;

                btnClear.Enabled = true;
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                if (ddlGLPostingCode.Items.Count > 0) ddlGLPostingCode.SelectedIndex = 1;
                break;

            case 1: //Modify
                try
                {
                    txtCountry.SelectedValue = "India";
                }
                catch (Exception ex)
                {
                    txtCountry.SelectedIndex = 0;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                tcEntityMaster.Tabs[1].Enabled = true;
                txtEntityCode.Enabled = false;
                //txtCountry.AutoPostBack = true;

                //Changed By Thangam M on 27/Feb/2012
                //txtEntityName.Enabled = false;
                //End here

                // btnModify.Visible = true;
                btnBnkModify.Enabled = false;  //modified as per UAT
                btnModifyAddress.Enabled = false;

                btnClear.Enabled = false;
                //btnSave.Enabled = true;
                // btnAdd.Enabled = false;
                //btnModify.Enabled = false;
                if (!bModify)
                {
                    btnSave.Enabled = false;
                    btnBnkModify.Enabled = false;
                }
                ddlEntityType.ClearDropDownList();
                ddlCompanyType.ClearDropDownList();
                //ddlRegStatus.ClearDropDownList();
                ddlConstitutionName.ClearDropDownList();
                ddlGLPostingCode.ClearDropDownList();
                break;
            case -1://Query

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                tcEntityMaster.Tabs[1].Enabled = true;
                txtEntityCode.ReadOnly = true;
                txtEntityName.ReadOnly = true;
                btnBnkModify.Visible = false;
                btnBnkClear.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                pnlBankDetails.Visible = false;
                txtEntityName.ReadOnly = true;
                ddlResidentialStatus.Enabled = false;
                ddlStateRegStatus.Enabled = false;
                txtAddress.ReadOnly = true;
                txtAddress2.ReadOnly = true;
                //txtCity.ReadOnly = true;
                //txtCity.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                //txtCity.ClearDropDownList();
                txtCity.ReadOnly = true;
                //txtCountry.ReadOnly = true;
                //txtCountry.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                //txtCountry.ClearDropDownList();
                txtCountry.Enabled = false;
                ddlComState.Enabled = false;
                txtPAN.ReadOnly = true;
                txtCIN.ReadOnly = true;
                txtVEndorEmailId.ReadOnly = true;
                txtMobile.ReadOnly = true;
                txtPINCode.ReadOnly = true;
                txtTAN.ReadOnly = true;
                txtCSTTIN.ReadOnly = true;
                txtVATTIN.ReadOnly = true;
                //ddlComState.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                //ddlComState.ClearDropDownList();
                btnAddAddress.Enabled = false;
                btnModifyAddress.Visible = false;
                txtTaxNumber.ReadOnly = true;
                txtPAN.ReadOnly = true;
                txtTelephone.ReadOnly = true;
                txtWebsite.ReadOnly = true;
                ddlVendorResidentialStatus.Enabled = false;
                txtAccountNumber.ReadOnly = true;
                txtBankName.ReadOnly = true;
                txtIFSC_Code.ReadOnly = true;
                txtBranchName.ReadOnly = true;
                chkDefaultAccount.Enabled = false;
                txtMICRCode.ReadOnly = true;
                txtBankAddress.ReadOnly = true;
                ddlEInvoice.Enabled = ddlComposition.Enabled = false;

                ddlMSMERegistered.Enabled = ddlType.Enabled = ddlCertificateReceived.Enabled = false;

                gvBAddress.Enabled = false;

                CalendarExtender3.Enabled = false;
                TxtSGSTRegDate.ReadOnly = txtSGSTin.ReadOnly = true;

                if (gvBAddress.Rows.Count != 0)
                {
                    foreach (GridViewRow drow in gvBAddress.Rows)
                    {
                        LinkButton lnkDelete = (LinkButton)drow.FindControl("lnkDelete");
                        lnkDelete.Enabled = false;
                    }
                }
                grvBankDetails.Enabled = false;
                if (gvBAddress.Rows.Count != 0)
                {
                    foreach (GridViewRow drow in grvBankDetails.Rows)
                    {
                        LinkButton lnkbtnDelete = (LinkButton)drow.FindControl("lnkbtnDelete");
                        lnkbtnDelete.Enabled = false;
                    }
                }

                if (bClearList)
                {
                    ddlEntityType.ClearDropDownList();
                    ddlGLPostingCode.ClearDropDownList();
                    ddlCompanyType.ClearDropDownList();
                    //ddlRegStatus.ClearDropDownList();
                    ddlConstitutionName.ClearDropDownList();
                }
                //grvBankDetails.Enabled = false;
                grvBankDetails.Columns[grvBankDetails.Columns.Count - 1].Visible = false;
                btnBnkAdd.Enabled = false;
                ddlGLPostingCode.ClearDropDownList();
                //Thangam M on 15/Nov/2012 for Trade Finance
                // chkTradeAdvance.Enabled = false;
                //End here
                try
                {
                    txtCountry.SelectedValue = "India";
                }
                catch (Exception ex)
                {
                    txtCountry.SelectedIndex = 0;
                }
                break;
        }
    }

    private void wraptext(GridView dtWrap, int intwraplength)
    {
        int intcolcount = 0;
        foreach (GridViewRow grvRow in dtWrap.Rows)
        {
            for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
            {
                if (grvRow.Cells[intcolcount].Text.Length > intwraplength)
                {
                    int intLoopcount = grvRow.Cells[intcolcount].Text.Length;
                    StringBuilder str = new StringBuilder();
                    StringBuilder str_copy = new StringBuilder();
                    str_copy = str;
                    str.Append(grvRow.Cells[intcolcount].Text);

                    for (int intLenCount = intwraplength; intLenCount < intLoopcount; intLenCount += intwraplength)
                    {

                        str_copy = str.Insert(intLenCount, "\r\n");

                    }
                    grvRow.Cells[intcolcount].Text = str_copy.ToString();
                }
            }
            intcolcount++;
        }
    }

    protected void FunClearBankDetails()
    {
        //ddlBranch.SelectedIndex = 0;
        ddlAccountType.SelectedIndex = 0;
        txtAccountNumber.Text = "";
        txtBankName.Text = "";
        txtBranchName.Text = "";
        txtMICRCode.Text = "";
        txtIFSC_Code.Text = "";
        ddlAddState.SelectedIndex = 0;
        txtBankAddress.Text = "";
        chkDefaultAccount.Checked = false;
        btnBnkAdd.Enabled = true;
        btnBnkModify.Enabled = false;
    }

    private void FunChangePANFormat(string strCountry)
    {
        txtTaxNumber.Attributes.Add("maxlength", "10");

        //Added by Thangam M on 24/Oct/2011 Based on OBS7-4 by Mr.S.Sudarsan

        //  revtxtPanNumber.Enabled = false;

        //End here

        //if (strCountry.ToLower() != "india")
        //{
        //    //mskexPanNumber.Enabled = false;
        //    //revtxtPanNumber.ValidationExpression = @"^[a-zA-Z_0-9](\w|\W)*";
        //    //revtxtPanNumber.ErrorMessage = "Enter a valid Income tax number";
        //    //revtxtPanNumber_Submit.ValidationExpression = @"^[a-zA-Z_0-9](\w|\W)*";
        //    //revtxtPanNumber_Submit.ErrorMessage = "Enter a valid Income tax number";
        //    //ftexMICRCode.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
        //   // FTBEtaxnumber.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
        //    txtMICRCode.MaxLength = 10;
        //}
        //else
        //{
        //    //mskexPanNumber.Enabled = true;
        //    //revtxtPanNumber.ValidationExpression = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
        //    //revtxtPanNumber.ErrorMessage = "Service Tax Regn Number should be of format AAAAA9999A";
        //    //revtxtPanNumber_Submit.ValidationExpression = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
        //    //revtxtPanNumber_Submit.ErrorMessage = "Service Tax Regn Number should be of format AAAAA9999A";
        //    //ftexMICRCode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
        //    //FTBEtaxnumber.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
        //    txtMICRCode.MaxLength = 9;
        //}
        txtCountry.Focus();
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
    #endregion


    protected void ddlResidentialStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlResidentialStatus.SelectedItem.ToString() == "Non-Resident")
        {
            lblResState.CssClass = "styleReqFieldLabel";
            ddlResState.Enabled = true;
        }
        else
        {
            lblResState.CssClass = "";
            ddlResState.Enabled = false;
            ddlResState.SelectedIndex = 0;
        }
    }

    protected void ddlMSMERegistered_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMSMERegistered.SelectedValue == "1")
        {
            ddlType.Enabled = ddlCertificateReceived.Enabled = true;
        }
        else
        {
            ddlType.SelectedIndex = ddlCertificateReceived.SelectedIndex = 0;
            ddlType.Enabled = ddlCertificateReceived.Enabled = false;
        }
    }
}
