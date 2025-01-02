#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Origination
/// Screen Name         :   S3GOrgEnquiryUpdation_Add
/// Created By          :   Venkatesan S
/// Created Date        :   21-MAY-2010
/// Purpose             :   To Create 
/// Last Updated By		:   Prabhu.K
/// Last Updated Date   :   05-Aug-2010
/// Reason              :   Modify the Page functionally.
/// 
/// 
/// Modified By         :   Thangam M
/// Modified Date       :   20-Sep-2010
/// Reason              :   Bug Fixing 
/// 

/// <Program Summary>
#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.Globalization;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.WebControls;
using S3GBusEntity;

#endregion

public partial class Origination_S3GOrgEnquiryUpdation_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyId = 0;
    int intUserId = 0;
    int intEnquiryUpdationId = 0;
    int intEnqNewCustomerId = 0;
    Decimal decRoundOff = 0;
    int intGPSPrefix = 0;
    int intGPSSuffix = 0;

    int intErrorCode, check = 0;
    Dictionary<string, string> Procparam = null;
    string strEntityCode;
    UserInfo ObjUserInfo;
    S3GSession objS3GSession;

    StringBuilder strbBnkDetails = new StringBuilder();
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgEnquiryUpdation_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgEnquiryUpdation_Add.aspx';";
    string strNewWin = "window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&NewCustomerID=0', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=620,resizable=yes,scrollbars=yes,top=50,left=50');return false;";

    //User Authorization
    string strMode = string.Empty;
    bool bClearList = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end


    EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable ObjS3G_SYSAD_GlobalParamPincodeMasterDataTable = new EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable();

    //EnquiryMgtServicesReference.EnquiryMgtServicesClient objCurrencyMasterClient;
    //EnquiryMgtServicesReference.EnquiryMgtServicesClient objConstitutionMasterClient;
    //EnquiryMgtServicesReference.EnquiryMgtServicesClient objAssetMasterClient;
    //EnquiryMgtServicesReference.EnquiryMgtServicesClient objCustomerMasterClient;
    EnquiryMgtServicesReference.EnquiryMgtServicesClient objGloabalParamPincodeClient;
    public static Origination_S3GOrgEnquiryUpdation_Add objPageClass = null;

    EnquiryService.S3G_ORG_ConstitutionMasterDataTable ObjS3G_ORG_ConstiutionMaster_ViewDataTable = new EnquiryService.S3G_ORG_ConstitutionMasterDataTable();
    EnquiryService.S3G_ORG_CustomerMasterDataTable ObjS3G_ORG_CustomerMaster_ViewDataTable = new EnquiryService.S3G_ORG_CustomerMasterDataTable();
    EnquiryService.S3G_ORG_AssetMasterDataTable ObjS3G_ORG_AssetMaster_ViewDataTable = new EnquiryService.S3G_ORG_AssetMasterDataTable();

    EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable ObjS3G_ORG_GlobalParamPincode_ViewDataTable = new EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable();
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            objPageClass = this;
            ObjUserInfo = new UserInfo();
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end
            lblErrorMessage.Text = "";
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            objS3GSession = new S3GSession();
            intGPSPrefix = objS3GSession.ProGpsPrefixRW;
            intGPSSuffix = objS3GSession.ProGpsSuffixRW;
            //FunPriToggleCustomerControls(true);      

            //-------Load Customer name in Pop up window
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
            txtUserName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtUserName.ToolTip = txtUserName.Text;
            strMode = Request.QueryString["qsMode"];
            if (PageMode != PageModes.Query)
            {
                GlobalParameters();//Changed by Tamilselvan.S on 26/11/2011 for Interest validation 
            }
            if (strMode == null)
            {
                if (check == 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Global Parameter is not defined');" + strRedirectPageView, true);
                    return;
                }
            }
            if (Request.QueryString["qsEnquiryUpdationId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsEnquiryUpdationId"));
                if (fromTicket != null)
                {
                    intEnquiryUpdationId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Enquiry Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                //                strMode = Request.QueryString["qsMode"];
            }
            if (Session["EnqNewCustomerId"] != null)
            {
                intEnqNewCustomerId = Convert.ToInt32(Utility.Load("EnqNewCustomerId", ""));
                if (intEnqNewCustomerId > 0)
                {
                    //----Modified By : Thangam M----
                    FunPubQueryCustomerCodeListEnquiryUpdation();
                    //-------

                    FunPubQueryExistCustomerListEnquiryUpdation(intEnqNewCustomerId);
                    //////////ddlCustomer.SelectedValue = intEnqNewCustomerId.ToString();
                    HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                    hdnCustomerId.Value = intEnqNewCustomerId.ToString();
                    //TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    //txtName.Text = intEnqNewCustomerId.ToString();

                    //Session["EnqNewCustomerId"] = null;
                }
            }
            //txtMargine.Attributes.Add("onblur", "ChkIsZero(this);EnableDisableResidual(this," + txtResidualValue.ClientID + ")");
            //txtResidualValue.Attributes.Add("onblur", "ChkIsZero(this);EnableDisableResidual(this," + txtMargine.ClientID + ")");
            // txtRemarks.Attributes.Add("onkeypress", "wraptext(" + txtRemarks.ClientID + ",20);");
            if (!IsPostBack)
            {
                //lnkCreate.Attributes.Add("onclick", strNewWin);
                txtResidualValue.Attributes.Add("onblur", "ChkIsZero(this,'Residual value')");
                txtMargine.Attributes.Add("onblur", "ChkIsZero(this,'Margin amount')");
                ftbeEnquiryDetails.ValidChars = ftbeEnquiryDetails.ValidChars + "\n\r";
                //if (strMode != "M" && strMode != "Q")
                ////FunPubQueryCustomerCodeListEnquiryUpdation();
                FunPubQueryAssetCodeListEnquiryUpdation();
                FunProLoadAddressCombos();
                FunPriLoadConstitution();
                //FunPriToggleCustomerControls(true);

                //FunPriTenureType();

                //<<Performance>>
                if (PageMode != PageModes.Query)
                {
                    FunLoadTenureType();
                    FunPubQueryCurrencyCodeListEnquiryUpdation();
                    FunPubQueryPinCodeGlobalParamListEnquiryUpdation();
                    FunPriLoadLOB(intUserId, intCompanyId);
                }

                //txtMargine.CheckGPSLength(Convert.ToInt32(txtMargine.Attributes["MaxC"]), txtMargine.MaxLength);
                txtMargine.CheckGPSLength(true, "Margin amount");
                //txtFacilityAmount.CheckGPSLength(Convert.ToInt32(txtMargine.Attributes["MaxC"]), txtFacilityAmount.MaxLength);
                txtFacilityAmount.CheckGPSLength(true, "Facility amount");
                //txtResidualValue.CheckGPSLength(Convert.ToInt32(txtMargine.Attributes["MaxC"]), txtResidualValue.MaxLength);
                txtResidualValue.CheckGPSLength(true, "Residual value");
                //txtTenure.CheckGPSLength(Convert.ToInt32(txtMargine.Attributes["MaxC"]), txtTenure.MaxLength);
                txtTenure.CheckGPSLength(true, "Tenure");
                int deciLen = 0;
                if (Convert.ToInt32(txtMargine.Attributes["DecC"]) > 4)
                    deciLen = 4;
                else
                    deciLen = Convert.ToInt32(txtMargine.Attributes["DecC"]);
                if (Convert.ToInt32(txtMargine.Attributes["MaxC"]) > 2)
                    txtInterestPercentage.Attributes.Add("onblur", "ChkIsZero(this,'Interest %');funChkDecimial(this," + 2 + "," + deciLen + ",'Interest %')");

                else
                    txtInterestPercentage.Attributes.Add("onblur", "ChkIsZero(this,'Interest %');funChkDecimial(this," + Convert.ToInt32(txtMargine.Attributes["MaxC"]) + "," + deciLen + ",'Interest %')");
                //User Authorization
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                txtCity.Visible = false;
                if (intEnquiryUpdationId > 0)
                {
                    if (strMode == "M")
                    { FunPriDisableControls(1); }
                    else
                    { FunPriDisableControls(-1); }
                }
                else
                {
                    FunPriDisableControls(0);
                }
                FunToolTip();
            }

            //Added By S.KAnnan,Start,
            //if (lblCreate.Visible)
            //    ddlNewCustomer.Style["Width"] = "162px";
            //else
            //    ddlNewCustomer.Style["Width"] = "200px";
            //Added By S.KAnnan,End.        
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region User defined functions

    private void FunToolTip()
    {
        try
        {
            //throw new NotImplementedException();
            txtEnqRefNo.ToolTip = lblEnqRefNo.Text;
            txtEnqDate.ToolTip = lblEnqDate.Text;
            ddlNewCustomer.ToolTip = lblNewCustomer.Text;
            //////ddlCustomer.ToolTip = lblCustomerReference.Text;        
            txtRemarks.ToolTip = lblRemarks.Text;
            txtResidualValue.ToolTip = lblResidualValue.Text;
            txtFacilityAmount.ToolTip = lblFacilityAmount.Text;
            txtInterestPercentage.ToolTip = lblInterestPercentage.Text;
            txtMargine.ToolTip = lblMargineAmount.Text;
            ddlAssetDetails.ToolTip = lblAssetDetails.Text;
            ddlCurrencyCode.ToolTip = lblCurrencyCode.Text;
            ddlFacilityType.ToolTip = lblFacilityType.Text;
            ddlInterestType.ToolTip = lblInterestType.Text;
            ddlTenureType.ToolTip = lblTenureType.Text;
            txtTenure.ToolTip = lblTenure.Text;
            btnSchedule.ToolTip = btnSchedule.Text;
            btnSave.ToolTip = btnSave.Text;
            btnClear.ToolTip = btnClear.Text;
            btnCancel.ToolTip = btnCancel.Text;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunLoadTenureType()
    {
        DataTable dtTable = new DataTable();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            ddlTenureType.BindDataTable(SPNames.S3G_ORG_GetEnquiryTenureType, Procparam, new string[] { "ID", "Name" });
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void GlobalParameters()
    {
        DataTable dtTable = new DataTable();
        DataTable dtTable1 = new DataTable();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());

            dtTable = Utility.GetDefaultData(SPNames.S3G_Get_GobalCompanyDetails, Procparam);
            dtTable1 = Utility.GetDefaultData(SPNames.S3G_ORG_GetGlobalParametermaster, Procparam);

            if (dtTable.Rows.Count > 0)
            {
                txtMargine.Attributes.Add("MaxC", dtTable.Rows[0]["Currency_Max_Digit"].ToString());
                txtMargine.Attributes.Add("DecC", dtTable.Rows[0]["Currency_Max_Dec_Digit"].ToString());
            }
            else
            {
                check = 1;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Define Global Parameters to proceed further');" + strRedirectPageAdd, true);               
            }
            if (dtTable1.Rows.Count > 0)
                txtMargine.Attributes.Add("Enquiry", dtTable1.Rows[0]["Enquiry_Number"].ToString());
            else
                // Issued fixed by Irsathameen on 27-12-2010 issue raised by Testing Team  
                check = 1;
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Define Global Parameters to proceed further');" + strRedirectPageView, true);            
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            if (!bQuery)
            {
                Response.Redirect(strRedirectPageView);
            }
            switch (intModeID)
            {
                case 0: // Create Mode
                    ddlCurrencyCode.SelectedIndex = 1;
                    FunPriToggleNewCustomerControls(true);
                    txtEnqDate.Text = DateTime.Now.Date.ToString(objS3GSession.ProDateFormatRW);
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    btnSave.Enabled = bCreate;
                    btnClear.Enabled = true;
                    //ddlNewCustomer.Focus();
                    break;

                case 1: // Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunPubQueryEnquiryUpdation(intEnquiryUpdationId);
                    ddlNewCustomer.Enabled = false;
                    btnClear.Enabled = false;
                    //lnkCreate.Visible = false;
                    btnSave.Enabled = bModify;
                    //lnkCreate.Attributes.Remove("onclick");

                    if (Procparam == null)
                        Procparam = new Dictionary<string, string>();
                    else
                        Procparam.Clear();

                    Procparam.Add("@Param1", intCompanyId.ToString());
                    Procparam.Add("@Param2", intEnquiryUpdationId.ToString());
                    Procparam.Add("@Option", "28");
                    DataTable Obj_Dt = new DataTable();
                    Obj_Dt = Utility.GetDataset("S3G_ORG_GetStatusLookUp", Procparam).Tables[0];
                    if (Obj_Dt.Rows.Count > 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Modification not possible. The Enquiry Number is assigned");
                        QueryView();
                        FunPubgetLOBName(intEnquiryUpdationId, 1);
                    }
                    else
                    {
                        if (ddlFacilityType.SelectedValue == "0")
                        {
                            FunPubgetLOBName(intEnquiryUpdationId, 2);
                            Utility.FunShowAlertMsg(this.Page, "The Facility Type : " + Session["LOBNAME"] + " is in deactive");
                        }

                    }
                    ddlAssetDetails.ToolTip = ddlAssetDetails.SelectedItem.Text;

                    if (ddlNewCustomer.SelectedValue == "1")
                    {
                        rfvMobile.Enabled = true;
                    }
                    else
                    {
                        rfvMobile.Enabled = false;
                    }

                    //////if (ddlCustomer.SelectedItem.Text != "--Select--")
                    //////{
                    //////    ddlCustomer.ToolTip = ddlCustomer.SelectedItem.Text;
                    //////}
                    break;
                case -1:// Query Mode
                    QueryView();
                    //FunPubgetLOBName(intEnquiryUpdationId, 1);
                    ddlAssetDetails.ToolTip = ddlAssetDetails.SelectedItem.Text;                    
                    //////if (ddlCustomer.SelectedItem.Text != "--Select--")
                    //////{
                    //////    ddlCustomer.ToolTip = ddlCustomer.SelectedItem.Text;
                    //////}

                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    public void QueryView()
    {
        try
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            FunPubQueryEnquiryUpdation(intEnquiryUpdationId);
            if (bClearList)
            {
                ddlNewCustomer.ClearDropDownList();
                //////ddlCustomer.RemoveDropDownList();
                ddlFacilityType.RemoveDropDownList();
                ddlInterestType.RemoveDropDownList();
                ddlTenureType.RemoveDropDownList();
                ddlCurrencyCode.RemoveDropDownList();
                ddlAssetDetails.RemoveDropDownList();
                ddlState.RemoveDropDownList(); 
                ddlCountry.RemoveDropDownList();
                ddlConstitutionId.RemoveDropDownList();
            }
            rdblAssetType.Enabled = false;
            //lblCreate.Visible = false;
            btnClear.Enabled = btnClear.Enabled = btnSave.Enabled =txtInterestPercentage.Enabled= false;
            txtTenure.ReadOnly = txtMargine.ReadOnly = txtResidualValue.ReadOnly =  txtFacilityAmount.ReadOnly = 
            txtRemarks.ReadOnly = true;
            //ddlNewCustomer.Width = 200;       
            //lnkCreate.Attributes.Remove("onclick");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPubQueryCustomerCodeListEnquiryUpdation()
    {
        //try
        //{
        //    if (strMode != "M" && strMode != "Q")
        //    {
        //        if (txtMargine.Attributes["Enquiry"].ToLower() == "true")
        //        {
        //            Dictionary<string, string> procparam = new Dictionary<string, string>();
        //            procparam.Add("@Company_ID", intCompanyId.ToString());
        //            ddlCustomer.BindDataTable("S3G_Get_Customer_Details_Enquiry_Updation", procparam, new string[] { "Customer_ID", "Customer" });
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Starting point of the process is not Enquiry');" + strRedirectPageView, true);
        //        }
        //    }
        //    else
        //    {
        //        Dictionary<string, string> procparam = new Dictionary<string, string>();
        //        procparam.Add("@Company_ID", intCompanyId.ToString());
        //        ddlCustomer.BindDataTable("S3G_Get_Customer_Details_Enquiry_Updation", procparam, new string[] { "Customer_ID", "Customer" });
        //    }
        //}
        //catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{
        //    lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        //}
        //catch (Exception ex)
        //{
        //    lblErrorMessage.Text = ex.Message;
        //}               
    }
    private void FunPubQueryPinCodeGlobalParamListEnquiryUpdation()
    {
        objGloabalParamPincodeClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            EnquiryService.S3G_SYSAD_GlobalParameterSetupRow ObjGlobalParamPincodeMasterRow;
            ObjGlobalParamPincodeMasterRow = ObjS3G_SYSAD_GlobalParamPincodeMasterDataTable.NewS3G_SYSAD_GlobalParameterSetupRow();
            ObjS3G_SYSAD_GlobalParamPincodeMasterDataTable.AddS3G_SYSAD_GlobalParameterSetupRow(ObjGlobalParamPincodeMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;
            ObjGlobalParamPincodeMasterRow.Company_ID = intCompanyId;
            byte[] byteGlobalParamPincodeDetails = objGloabalParamPincodeClient.FunPubQueryPinCodeGlobalParamListEnquiryUpdation(intCompanyId, SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_GlobalParamPincode_ViewDataTable, SerMode));
            ObjS3G_ORG_GlobalParamPincode_ViewDataTable = (EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(byteGlobalParamPincodeDetails, SerializationMode.Binary, typeof(EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable));
            //ddlAssetDetails.FillDataTable(ObjS3G_ORG_AssetMaster_ViewDataTable, ObjS3G_ORG_AssetMaster_ViewDataTable.IDColumn.ColumnName, ObjS3G_ORG_AssetMaster_ViewDataTable.AssetColumn.ColumnName);
            //objGloabalParamPincodeClient.Close();
            FunPriValidateGloabalParamPimcodeDetails(ObjS3G_ORG_GlobalParamPincode_ViewDataTable);
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objGloabalParamPincodeClient.Close();
        }
    }
    private void FunPriValidateGloabalParamPimcodeDetails(EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable ObjS3G_ORG_GlobalParamPincode)
    {
        if (ObjS3G_ORG_GlobalParamPincode.Rows[0]["Pincode_Field_Type"].ToString() == "Numeric")
        {
            //txtPinCode.Attributes.Add("onkeypress", "fnAllowNumbersOnly(false);");
            //txtPinCode.MaxLength = Convert.ToInt32(ObjS3G_ORG_GlobalParamPincode.Rows[0]["Pincode_Digits"]);
        }
        else
        {
            //txtPinCode.Attributes.Add("onkeypress", "fnCheckSpecialChars(false);");  
        }
    }
    private void FunPubQueryCurrencyCodeListEnquiryUpdation()
    {
        try
        {
            Dictionary<string, string> procparam = new Dictionary<string, string>();
            procparam.Add("@Company_ID", intCompanyId.ToString());
            ddlCurrencyCode.BindDataTable("S3G_Get_Currency_Details_Enquiry_Updation", procparam, new string[] { "Currency_ID", "Currency" });
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        { lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW; }
        catch (Exception ex)
        { lblErrorMessage.Text = ex.Message; }
    }
    private void FunPriGetDocumentNumberControlEnquiryNoDetails()
    {
        EnquiryMgtServicesReference.EnquiryMgtServicesClient ObjEnquiryUpdationClient;
        ObjEnquiryUpdationClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {


            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteLOBDetails = ObjEnquiryUpdationClient.FunPriGetDocumentNumberControlEnquiryNoDetails(intCompanyId, "Enq Upd", 0, 0);
            EnquiryService.S3G_ORG_DocumentNumberControlDataTable dtEnq = (EnquiryService.S3G_ORG_DocumentNumberControlDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, SerializationMode.Binary, typeof(EnquiryService.S3G_ORG_DocumentNumberControlDataTable));
            EnquiryService.S3G_ORG_DocumentNumberControlRow dtEnqRow = dtEnq.Rows[0] as EnquiryService.S3G_ORG_DocumentNumberControlRow;
            txtEnqRefNo.Text = dtEnqRow["EnquiryNo"].ToString();
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjEnquiryUpdationClient.Close();
        }
    }

    private void FunPubQueryAssetCodeListEnquiryUpdation()
    {
        try
        {
            Dictionary<string, string> procparam = new Dictionary<string, string>();
            procparam.Add("@Company_ID", intCompanyId.ToString());
            if (!string.IsNullOrEmpty(strMode))
                procparam.Add("@mode", strMode);
            if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")
                procparam.Add("@Existing", "1");
            ddlAssetDetails.BindDataTable("S3G_Get_Asset_Details_Enquiry_Updation", procparam, new string[] { "Asset_ID", "Asset" });

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FungetLARassetvalue()
    {
        try
        {
            txtFacilityAmount.Text = "";
            if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@LAR_Id", ddlAssetDetails.SelectedValue);
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")
                    Procparam.Add("@Existing", "1");
                DataTable dt = Utility.GetDefaultData("S3G_Get_Asset_Details_Enquiry_Updation", Procparam);
                if (dt.Rows.Count > 0)
                {
                    txtFacilityAmount.Text = dt.Rows[0]["Assetvalue"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriToggleNewCustomerControls(bool blnIsEnabled)
    {
        try
        {
            //ddlCustomer.SelectedIndex = 0;            
            //lblCreate.Enabled = blnIsEnabled;
            //////ddlCustomer.Enabled = !blnIsEnabled;
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Text = string.Empty;
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            hdnCustomerId.Value = string.Empty;
            ucCustomerCodeLov.Visible = !blnIsEnabled;
            txtCustomerCode.Visible = !blnIsEnabled;
            lblCustomerCode.Visible = !blnIsEnabled;
            //lblCustomerReference.Visible = !blnIsEnabled;
            //////rfvCustomerCode.Visible = !blnIsEnabled;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    private void FunPriToggleCustomerControls(bool blnIsEnabled)
    {
        try
        {
            //txtCustomerCode.Enabled=txtWebSite.Enabled = txtComAddress.Enabled = txtName.Enabled = txtConstitution.Enabled = txtAddress.Enabled = txtAddress2.Enabled = txtCity.Enabled = txtState.Enabled = txtCountry.Enabled = txtEmail.Enabled = txtMobile.Enabled = txtPinCode.Enabled = blnIsEnabled;          
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    private bool FunPriIsValid()
    {
        try
        {
            bool blnResult = true;
            if (!FunPriIsValidMargin())
            {
                blnResult = false;
                return blnResult;
            }
            #region Changes made after the introduction of the bug: If the user selects Loan as LOB, system is prompting the user to select the asset or type the remarks both the fields are optional.
            /* REVISION HISTORY*/
            /* Performed by : Srivatsan.s
             * Performed on : 05/11/2011
             * Description  : Please refer the Email sent by Narasimha Rao on 03 November 2011 12:01
             *                Subject: Observation Log
             * Changes Made : Added a condition to check if the LOB i.e the Facility Type is HP or FL. For all the other LOBs the message won't prompt.
            */
            #endregion
            if ((ddlFacilityType.SelectedItem.Text.StartsWith("FL") == true) || (ddlFacilityType.SelectedItem.Text.StartsWith("HP") == true))
            {
                if (ddlAssetDetails.SelectedIndex == 0)
                {
                    if (txtRemarks.Text == "")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Select the asset details/Enter the remarks");
                        ddlAssetDetails.Focus();
                        blnResult = false;
                    }
                }
            }
            return blnResult;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private bool FunPriIsValidMargin()
    {
        try
        {
            bool blnResult = true;
            if (txtMargine.Text != "" && txtFacilityAmount.Text != "")
            {
                if (Convert.ToDouble(txtMargine.Text) > Convert.ToDouble(txtFacilityAmount.Text))
                {
                    lblErrorMessage.Text = "Correct the following validation(s):" + "<br><br>" + "&nbsp;&nbsp;&nbsp;&nbsp; " + "Margin Amount should be less than Facility Amount";
                    txtMargine.Enabled = true;
                    txtResidualValue.Enabled = false;
                    txtMargine.Focus();
                    blnResult = false;
                }
            }
            return blnResult;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private decimal CalculateInstalmentAmount()
    {
        try
        {
            decimal FinanceAmount = 0;
            decimal InterestAmount = 0m;
            double IRR;
            decimal TotalReceive = 0;
            decimal InstalmentAmount = 0;
            int Tenure = Convert.ToInt32(txtTenure.Text);


            if (ddlInterestType.SelectedItem.Text.ToUpper() == "RATE")
            {
                //the following codes are modified by Prakash K , since the facility amount is considered as the Finance amount
                //modified date 08/02/2011

                //if (txtMargine.Text != "")
                //{ FinanceAmount = Convert.ToInt64(txtFacilityAmount.Text) - Convert.ToInt32(txtMargine.Text); }
                //else if (txtResidualValue.Text != "")
                //{ FinanceAmount = Convert.ToInt64(txtFacilityAmount.Text) - Convert.ToInt32(txtResidualValue.Text); }
                //else
                //{ 
                FinanceAmount = unchecked(Convert.ToInt64(txtFacilityAmount.Text));
                //}
            }
            else
            {
                //if (txtMargine.Text != "")
                //{
                //    FinanceAmount = Convert.ToInt32(txtMargine.Text) - Convert.ToInt64(txtFacilityAmount.Text);
                //}
                //else if (txtResidualValue.Text != "")
                //{
                //    FinanceAmount = Convert.ToInt32(txtResidualValue.Text) - Convert.ToInt64(txtFacilityAmount.Text);
                //}
                //else
                //{
                FinanceAmount = 0 - (Convert.ToInt64(txtFacilityAmount.Text));
                //}
            }
            if (ddlInterestType.SelectedItem.Text.ToUpper() == "RATE")
            {
                if (ddlTenureType.SelectedItem.Text.ToUpper() == "MONTHS")
                {
                    InterestAmount = (decimal)FinanceAmount * Tenure / 12 * Convert.ToDecimal(txtInterestPercentage.Text) / 100;
                }
                else if (ddlTenureType.SelectedItem.Text.ToUpper() == "WEEKS")
                {
                    InterestAmount = (decimal)FinanceAmount * Tenure / 52 * Convert.ToDecimal(txtInterestPercentage.Text) / 100;
                }
                else if (ddlTenureType.SelectedItem.Text.ToUpper() == "DAYS")
                {
                    InterestAmount = (decimal)FinanceAmount * Tenure / 365 * Convert.ToDecimal(txtInterestPercentage.Text) / 100;
                }
                TotalReceive = (decimal)FinanceAmount + InterestAmount;
                InstalmentAmount = (int)Math.Round(TotalReceive / Convert.ToInt32(txtTenure.Text.ToString().Trim()));
            }
            else
            {
                //txtInterestPercentage.Attributes.Add("Interest", (Convert.ToDecimal(txtInterestPercentage.Text) / 2).ToString());
                // double irr = Math.Round(Microsoft.VisualBasic.Financial.Pmt((14.4516) / (24 * 100), 24, -90000, 0, Microsoft.VisualBasic.DueDate.EndOfPeriod), 0);


                IRR = Math.Round(Microsoft.VisualBasic.Financial.Pmt(Convert.ToDouble(txtInterestPercentage.Text) / (12 * 100), Convert.ToDouble(Tenure), Convert.ToDouble(FinanceAmount), 0, Microsoft.VisualBasic.DueDate.EndOfPeriod), intGPSSuffix);
                InterestAmount = Math.Round((Convert.ToDecimal(IRR)), intGPSSuffix);
                InstalmentAmount = Convert.ToDecimal(InterestAmount);

                //if (ddlTenureType.SelectedItem.Text.ToUpper() == "MONTHS")
                //{
                //    InterestAmount = (decimal)FinanceAmount * Tenure / 12 * Convert.ToDecimal(txtInterestPercentage.Text) / 100;
                //}
                //else if (ddlTenureType.SelectedItem.Text.ToUpper() == "WEEKS")
                //{
                //    InterestAmount = (decimal)FinanceAmount * Tenure / 52 * Convert.ToDecimal(txtInterestPercentage.Text) / 100;
                //}
                //else if (ddlTenureType.SelectedItem.Text.ToUpper() == "DAYS")
                //{
                //    InterestAmount = (decimal)FinanceAmount * Tenure / 365 * Convert.ToDecimal(txtInterestPercentage.Text) / 100;
                //}
            }

            return InstalmentAmount;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private DateTime LastDate(DateTime dt)
    {
        try
        {
            DateTime dtTo = dt;
            dtTo = dt.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));

            return dtTo;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriLoadConstitution()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
        ddlConstitutionId.BindDataTable("S3G_ORG_GET_CONSTITUTION_NAME", Procparam, new string[] { "Constitution_ID", "Constitution" });
    }

    //////protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    //////{
    //////    if (ddlCustomer.SelectedIndex >0 )
    //////    {
    //////        int CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
    //////        FunPubQueryExistCustomerListEnquiryUpdation(CustomerID);
    //////        FunPriClearControlsforDropDown();
    //////        ddlCustomer.Focus();
    //////        ddlCurrencyCode.SelectedIndex = 1;
    //////        ddlCustomer.ToolTip = ddlCustomer.SelectedItem.Text;
    //////    }
    //////    else
    //////    {   
    //////        FunPriClearCusdetails();
    //////        ddlCurrencyCode.SelectedIndex = 1;
    //////        ddlCustomer.ToolTip = "Customer No";
    //////    }
    //////}
    private void FunPubQueryExistCustomerListEnquiryUpdation(int CustomerID)
    {
        EnquiryMgtServicesReference.EnquiryMgtServicesClient ObjEnquiryUpdationClient;
        ObjEnquiryUpdationClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            string strCustomerAddress = "";



            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteExistCusDetails = ObjEnquiryUpdationClient.FunPubQueryExistCustomerListEnquiryUpdation(CustomerID, intCompanyId);
            EnquiryService.S3G_ORG_ExistCustomerMasterDataTable dtEnqExistCus = (EnquiryService.S3G_ORG_ExistCustomerMasterDataTable)ClsPubSerialize.DeSerialize(byteExistCusDetails, SerializationMode.Binary, typeof(EnquiryService.S3G_ORG_ExistCustomerMasterDataTable));
            if (dtEnqExistCus != null && dtEnqExistCus.Rows.Count > 0)
            {
                EnquiryService.S3G_ORG_ExistCustomerMasterRow dtEnqExistCusRow = dtEnqExistCus.Rows[0] as EnquiryService.S3G_ORG_ExistCustomerMasterRow;
                //txtConstitution.Text = dtEnqExistCusRow["Constitution"].ToString();
                strCustomerAddress = SetCustomerAddress(dtEnqExistCusRow["comm_address1"].ToString(), dtEnqExistCusRow["comm_address2"].ToString(), dtEnqExistCusRow["comm_city"].ToString(), dtEnqExistCusRow["comm_state"].ToString(), dtEnqExistCusRow["comm_country"].ToString(), dtEnqExistCusRow["comm_pincode"].ToString());

                //Added By Thangam M on 16/Feb/2012 to fix bug id - 5460
                //S3GCustomerAddress1.SetCustomerDetails(dtEnqExistCusRow, true);

                //S3GCustomerAddress1.SetCustomerDetails(dtEnqExistCusRow["Customer_Code"].ToString(),
                //    strCustomerAddress, dtEnqExistCusRow["Customer_Name"].ToString(),
                //    dtEnqExistCusRow["comm_mobile"].ToString(),
                //    dtEnqExistCusRow["comm_email"].ToString(),
                //    dtEnqExistCusRow["Comm_Website"].ToString());
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjEnquiryUpdationClient.Close();
        }
    }
    private void FunPubQueryEnquiryUpdation(int EnquiryUpdationId)
    {
        EnquiryMgtServicesReference.EnquiryMgtServicesClient ObjEnquiryUpdationClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            string strCustomerAddress = "";


            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteExistCusDetails = ObjEnquiryUpdationClient.FunPubQueryEnquiryUpdation(EnquiryUpdationId, intCompanyId);
            EnquiryService.S3G_ORG_EnquiryUpdationDataTable dtEnquiryDetails = (EnquiryService.S3G_ORG_EnquiryUpdationDataTable)ClsPubSerialize.DeSerialize(byteExistCusDetails, SerializationMode.Binary, typeof(EnquiryService.S3G_ORG_EnquiryUpdationDataTable));
            EnquiryService.S3G_ORG_EnquiryUpdationRow drEnquirydetailsRow = dtEnquiryDetails.Rows[0] as EnquiryService.S3G_ORG_EnquiryUpdationRow;

            txtEnqRefNo.Text = Convert.ToString(drEnquirydetailsRow["Enquiry_No"]);
            txtEnqDate.Text = DateTime.Parse(Convert.ToString(drEnquirydetailsRow["Enquiry_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(objS3GSession.ProDateFormatRW);
            ddlNewCustomer.SelectedValue = (Convert.ToString(drEnquirydetailsRow["Is_NewCustomer"]).ToUpper() == "FALSE") ? "0" : "1";
            ////////ddlCustomer.SelectedValue = Convert.ToString(drEnquirydetailsRow["Customer_Id"]);
            if (ddlNewCustomer.SelectedValue == "0")
            {
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Text = Convert.ToString(drEnquirydetailsRow["Title"]) + ' ' + Convert.ToString(drEnquirydetailsRow["Customer_Name"]);
                txtName.Enabled = false;
                txtName.ToolTip = txtName.Text;
                ucCustomerCodeLov.Visible = true;
                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                hdnCustomerId.Value = Convert.ToString(drEnquirydetailsRow["Customer_Id"]);
                txtCustomerName.Visible = false;
            }
            else
            {
                txtCustomerName.Text = Convert.ToString(drEnquirydetailsRow["Customer_Name"]);
                ucCustomerCodeLov.Visible = false;
                txtCustomerName.Visible = true;

            }

            //txtName.Text =Convert.ToString(drEnquirydetailsRow["Title"])+ Convert.ToString(drEnquirydetailsRow["Customer_Name1"]);
            Utility.store("ModifyConstitutionId", Convert.ToInt32(drEnquirydetailsRow["Constitution_Id"]));
            txtCustomerCode.Text = Convert.ToString(drEnquirydetailsRow["Customer_Code"]);
            ddlConstitutionId.SelectedValue = Convert.ToString(drEnquirydetailsRow["Constitution_Id"]);
            //txtConstitution.Text = Convert.ToString(drEnquirydetailsRow["Constitution_name"]);
            //txtAddress.Text = Convert.ToString(drEnquirydetailsRow["comm_address1"]);
            //txtAddress2.Text = Convert.ToString(drEnquirydetailsRow["comm_address2"]);
            //txtCity.Text = Convert.ToString(drEnquirydetailsRow["comm_city"]);
            //txtState.Text = Convert.ToString(drEnquirydetailsRow["comm_state"]);
            //txtCountry.Text = Convert.ToString(drEnquirydetailsRow["comm_country"]);
            //txtPinCode.Text = Convert.ToString(drEnquirydetailsRow["comm_pincode"]);
            //txtMobile.Text = Convert.ToString(drEnquirydetailsRow["comm_mobile"]);
            //txtEmail.Text = Convert.ToString(drEnquirydetailsRow["comm_email"]);
            // Communication Address

            //strCustomerAddress = SetCustomerAddress(drEnquirydetailsRow["comm_address1"].ToString(), drEnquirydetailsRow["comm_address2"].ToString(), drEnquirydetailsRow["comm_city"].ToString(), drEnquirydetailsRow["comm_state"].ToString(), drEnquirydetailsRow["comm_country"].ToString(), drEnquirydetailsRow["comm_pincode"].ToString());
            strCustomerAddress = drEnquirydetailsRow["comm_address1"].ToString() + "\n" + drEnquirydetailsRow["comm_address2"].ToString();
            txtAddress.Text = strCustomerAddress;
            txtCity.Text = Convert.ToString(drEnquirydetailsRow["Comm_City"]);
            ucAutoSuggest.Visible = false;
            txtCity.Visible = true;
            ddlState.SelectedItem.Text = Convert.ToString(drEnquirydetailsRow["Comm_State"]);
            ListItem lstState = new ListItem(Convert.ToString(drEnquirydetailsRow["Comm_State"]));
            ddlState.Items.Remove(lstState);
            ddlState.Items.Add(lstState);
            ddlCountry.SelectedItem.Text = Convert.ToString(drEnquirydetailsRow["Comm_Country"]);
            ListItem lstCountry = new ListItem(Convert.ToString(drEnquirydetailsRow["Comm_Country"]));
            ddlCountry.Items.Remove(lstCountry);
            ddlCountry.Items.Add(lstCountry);
            //ddlState.SelectedItem.Text = Convert.ToString(drEnquirydetailsRow["Comm_State"]);
            //ddlCountry.SelectedItem.Text = Convert.ToString(drEnquirydetailsRow["Comm_Country"]);
            if (ddlNewCustomer.SelectedValue == "0")
            {
                txtPinCode.ReadOnly = txtEmail.ReadOnly = txtMobile.ReadOnly = true;
            }
            txtPinCode.Text = Convert.ToString(drEnquirydetailsRow["Comm_Pincode"]);
            txtEmail.Text = Convert.ToString(drEnquirydetailsRow["Comm_EMail"]);
            txtMobile.Text = Convert.ToString(drEnquirydetailsRow["Comm_Mobile"]);
            //Added By Thangam M on 16/Feb/2012 to fix bug id - 5460
            // S3GCustomerAddress1.SetCustomerDetails(drEnquirydetailsRow, true);

            //S3GCustomerAddress1.SetCustomerDetails(drEnquirydetailsRow["Customer_Code"].ToString(),
            //    strCustomerAddress,
            //    Convert.ToString(drEnquirydetailsRow["Title"]) + ' ' + Convert.ToString(drEnquirydetailsRow["Customer_Name1"]),
            //    drEnquirydetailsRow["comm_mobile"].ToString(),
            //    drEnquirydetailsRow["comm_email"].ToString(),
            //    drEnquirydetailsRow["Comm_Website"].ToString());

            if (drEnquirydetailsRow["Asset_Type"] != null && drEnquirydetailsRow["Asset_Type"].ToString() != string.Empty)
            {
                rdblAssetType.Visible = true;
                rdblAssetType.SelectedValue = drEnquirydetailsRow["Asset_Type"].ToString();
                FunPubQueryAssetCodeListEnquiryUpdation();
            }
            else
            {
                rdblAssetType.Visible = false;
            }
            //txtComAddress.Text = SetCustomerAddress(drEnquirydetailsRow["comm_address1"].ToString(), drEnquirydetailsRow["comm_address2"].ToString(), drEnquirydetailsRow["comm_city"].ToString(), drEnquirydetailsRow["comm_state"].ToString(), drEnquirydetailsRow["comm_country"].ToString(), drEnquirydetailsRow["comm_pincode"].ToString());
            //txtWebSite.Text = Convert.ToString(drEnquirydetailsRow["Comm_Website"]);
            //txtCustomerCode.Text = Convert.ToString(drEnquirydetailsRow["Customer_Code"]);


            ddlAssetDetails.SelectedValue = Convert.ToString(drEnquirydetailsRow["Asset_ID"]);

            //Added by Thangam M on 16/Feb/2012 to load inactive LOB's
            ListItem ListLOB = new ListItem(Convert.ToString(drEnquirydetailsRow["LOB_Name"]), Convert.ToString(drEnquirydetailsRow["Facility_Type"]), true);
            ListLOB.Selected = false;
            if (ddlFacilityType.Items.FindByValue(Convert.ToString(drEnquirydetailsRow["Facility_Type"])) == null)
            {
                ddlFacilityType.Items.Add(ListLOB);
            }

            ddlFacilityType.SelectedValue = Convert.ToString(drEnquirydetailsRow["Facility_Type"]);
            txtFacilityAmount.Text = Convert.ToString(drEnquirydetailsRow["Facility_Amount"]);

            //<<Performance>>
            if (PageMode == PageModes.Query)
            {
                ListItem ListCurr = new ListItem(Convert.ToString(drEnquirydetailsRow["Currency"]), Convert.ToString(drEnquirydetailsRow["Currency_ID"]), true);
                ddlCurrencyCode.Items.Add(ListCurr);

                ListItem ListTen = new ListItem(Convert.ToString(drEnquirydetailsRow["Tenure_Type"]), Convert.ToString(drEnquirydetailsRow["Tenure_Type_ID"]), true);
                ddlTenureType.Items.Add(ListTen);

                txtAddress.ReadOnly =
                txtMobile.ReadOnly =
                txtPinCode.ReadOnly =
                txtCustomerName.ReadOnly =
                txtEmail.ReadOnly =
                txtCity.ReadOnly = true;
            }

            ddlCurrencyCode.SelectedValue = Convert.ToString(drEnquirydetailsRow["Currency_ID"]);

            txtRemarks.Text = Convert.ToString(drEnquirydetailsRow["Remarks"]);
            txtTenure.Text = Convert.ToString(drEnquirydetailsRow["Tenure"]);
            ddlTenureType.SelectedValue = Convert.ToString(drEnquirydetailsRow["Tenure_Type_ID"]);
            ddlInterestType.SelectedValue = Convert.ToString(drEnquirydetailsRow["Interest_Type_ID"]);
            txtInterestPercentage.Text = Convert.ToString(drEnquirydetailsRow["Interest_Percentage"]);
            txtMargine.Text = Convert.ToString(drEnquirydetailsRow["Margin_Amount"]);
            txtResidualValue.Text = Convert.ToString(drEnquirydetailsRow["Residual_Value"]);
            if (ddlFacilityType.SelectedIndex > 0)
                FunPriDisableResidualvalue();
            //lblCreate.Enabled = false;
            //////ddlCustomer.Enabled = false;

            Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
            btnGetLOV.Visible = false;

            if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")
            {
                txtFacilityAmount.ReadOnly = true;
            }
            else
            {
                txtFacilityAmount.ReadOnly = false;
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            ObjEnquiryUpdationClient.Close();
        }
    }
    private void FunPubgetLOBName(int EnquiryUpdationId, int flag)
    {
        try
        {
            if (flag == 1)
            {
                Dictionary<string, string> dictLOB = new Dictionary<string, string>();
                dictLOB.Add("@Company_ID", Convert.ToString(intCompanyId));
                dictLOB.Add("@EnqID", Convert.ToString(EnquiryUpdationId));
                DataTable dtLOB = Utility.GetDefaultData("S3G_Org_GetEnquiryLOBName", dictLOB);
                if (dtLOB.Rows.Count >= 1)
                {
                    DataRow drLOB = dtLOB.Rows[0];
                    ddlFacilityType.SelectedValue = drLOB["LOB_ID"].ToString();
                    ddlFacilityType.SelectedItem.Text = drLOB["LOBNAME"].ToString();
                }
            }
            else if (flag == 2)
            {
                Dictionary<string, string> dictLOB = new Dictionary<string, string>();
                dictLOB.Add("@Company_ID", Convert.ToString(intCompanyId));
                dictLOB.Add("@EnqID", Convert.ToString(EnquiryUpdationId));
                DataTable dtLOB = Utility.GetDefaultData("S3G_Org_GetEnquiryLOBName", dictLOB);
                if (dtLOB.Rows.Count >= 1)
                {
                    DataRow drLOB = dtLOB.Rows[0];
                    Session["LOBNAME"] = drLOB["LOBNAME"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void FunPriClearControls()
    {
        try
        {
            FunPriClearCusdetails();
            txtEnqRefNo.Text = txtResidualValue.Text = txtInterestPercentage.Text = txtMargine.Text = txtTenure.Text = txtRemarks.Text = txtFacilityAmount.Text = string.Empty;
            if (ddlFacilityType.Items.Count > 0)
                ddlFacilityType.SelectedIndex = 0;
            if (ddlCurrencyCode.Items.Count > 0)
                ddlCurrencyCode.SelectedIndex = 0;
            if (ddlAssetDetails.Items.Count > 0)
                ddlAssetDetails.SelectedIndex = 0;
            if (ddlTenureType.Items.Count > 0)
                ddlTenureType.SelectedIndex = 0;
            if (ddlNewCustomer.Items.Count > 0)
                ddlNewCustomer.SelectedIndex = 0;
            if (ddlInterestType.Items.Count > 0)
                ddlInterestType.SelectedIndex = 0;
            //////if (ddlCustomer.Items.Count > 0)
            //////    ddlCustomer.SelectedIndex = 0;
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Text = string.Empty;
            ucCustomerCodeLov.Visible = false;
            //lblCustomerReference.Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriClearCusdetails()
    {
        try
        {
            //S3GCustomerAddress1.ClearCustomerDetails();
            //txtConstitution.Text = string.Empty;
            //txtComAddress.Text = txtWebSite.Text = txtCustomerCode.Text = txtEmail.Text = txtMobile.Text = txtPinCode.Text = txtCountry.Text = txtState.Text = txtCity.Text = txtAddress2.Text = txtConstitution.Text = txtAddress.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //private void FunPriClearControlsforDropDown()
    //{
    //    ddlFacilityType.SelectedIndex = 0;
    //    txtFacilityAmount.Text = "";
    //    ddlCurrencyCode.SelectedIndex = 0;
    //    ddlAssetDetails.SelectedIndex = 0;
    //    txtRemarks.Text = "";
    //    txtTenure.Text = "";
    //    ddlTenureType.SelectedIndex = 0;
    //    ddlInterestType.SelectedIndex = 0;
    //    txtInterestPercentage.Text = "";
    //    txtMargine.Text = "";
    //    txtResidualValue.Text = "";
    //}
    private void FunPriLoadLOB(int intUser_id, int intCompany_id)
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Company_ID", intCompany_id.ToString());
            Procparam.Add("@User_ID", intUser_id.ToString());
            Procparam.Add("@Program_ID", "18");
            ddlFacilityType.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //private void FunPriTenureType()
    //{
    //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //    Procparam.Clear();
    //    Procparam.Add("@Is_Active", "1");
    //    ddlTenureType.BindDataTable(SPNames.S3G_ORG_GetTenureType, Procparam, new string[] { "Value", "Name" });

    //}
    private void FunPriSaveEnquiryDetails()
    {
        EnquiryMgtServicesReference.EnquiryMgtServicesClient objEnquiryUpdate = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            string[] strSplitCusValues = new string[2];
            // EnquiryMgtServicesClient objEnquiryUpdate = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
            //EnquiryService.S3G_ORG_EnquiryUpdationRow ObjS3G_ORG_EnquiryUpdationRow = new EnquiryService.S3G_ORG_EnquiryUpdationRow();
            //ObjS3G_ORG_EnquiryUpdationRow = 

            EnquiryService.S3G_ORG_EnquiryUpdationDataTable objS3G_EnquiryReferenceTable = new EnquiryService.S3G_ORG_EnquiryUpdationDataTable();
            EnquiryService.S3G_ORG_EnquiryUpdationRow ObjS3G_ORG_EnquiryUpdationRow;
            // ObjS3G_ORG_EnquiryUpdationRow = new EnquiryService.S3G_ORG_EnquiryUpdationRow();
            ObjS3G_ORG_EnquiryUpdationRow = objS3G_EnquiryReferenceTable.NewS3G_ORG_EnquiryUpdationRow();


            ObjS3G_ORG_EnquiryUpdationRow.Enquiry_No = txtEnqRefNo.Text;
            ObjS3G_ORG_EnquiryUpdationRow.Enquiry_Date = Utility.StringToDate(txtEnqDate.Text);
            if (ddlNewCustomer.SelectedValue == "0")
            {
                ObjS3G_ORG_EnquiryUpdationRow.Is_NewCustomer = false;
                //////ObjS3G_ORG_EnquiryUpdationRow.Customer_ID = Convert.ToInt32(ddlCustomer.SelectedValue);
                UserControls_LOBMasterView ucLOVCustomer = new UserControls_LOBMasterView();
                if (ucLOVCustomer != null)
                {
                    HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                    if (hdnCustomerId != null)
                    {
                        if (hdnCustomerId.Value != "")
                        {
                            ObjS3G_ORG_EnquiryUpdationRow.Customer_ID = Convert.ToInt32(hdnCustomerId.Value);
                        }
                    }
                }

                ObjS3G_ORG_EnquiryUpdationRow.Constitution_ID = Convert.ToInt32(ddlConstitutionId.SelectedValue);
            }
            else
            {
                ObjS3G_ORG_EnquiryUpdationRow.Is_NewCustomer = true;
                if (intEnquiryUpdationId > 0)
                {
                    ObjS3G_ORG_EnquiryUpdationRow.Customer_ID = Convert.ToInt32(ViewState["ModifyCustomerId"]);
                }
                else
                {
                    //ObjS3G_ORG_EnquiryUpdationRow.Customer_ID = intEnqNewCustomerId;
                    if (intEnqNewCustomerId != 0)
                    {
                        ObjS3G_ORG_EnquiryUpdationRow.Customer_ID = intEnqNewCustomerId;
                    }
                }
                ObjS3G_ORG_EnquiryUpdationRow.Customer_Name = txtCustomerName.Text;                
                ObjS3G_ORG_EnquiryUpdationRow.Address = txtAddress.Text;
                ObjS3G_ORG_EnquiryUpdationRow.City = ucAutoSuggest.SelectedText;  
                ObjS3G_ORG_EnquiryUpdationRow.State = ddlState.SelectedItem.Text;
                ObjS3G_ORG_EnquiryUpdationRow.Country = ddlCountry.SelectedItem.Text;
                ObjS3G_ORG_EnquiryUpdationRow.PINCode_ZipCode = txtPinCode.Text;
                ObjS3G_ORG_EnquiryUpdationRow.Mobile = txtMobile.Text;
                ObjS3G_ORG_EnquiryUpdationRow.EMail = txtEmail.Text;
            }
            if (intEnquiryUpdationId != 0)
            {                
                ObjS3G_ORG_EnquiryUpdationRow.City = txtCity.Text.ToString();
                ObjS3G_ORG_EnquiryUpdationRow.EnquiryUpdation_ID = intEnquiryUpdationId;
                ObjS3G_ORG_EnquiryUpdationRow.Address = txtAddress.Text.ToString();
                ObjS3G_ORG_EnquiryUpdationRow.Address2 = "";               
            }
            ObjS3G_ORG_EnquiryUpdationRow.Facility_Type = Convert.ToInt32(ddlFacilityType.SelectedValue);
            ObjS3G_ORG_EnquiryUpdationRow.Facility_Amount = Convert.ToDecimal(txtFacilityAmount.Text);
            ObjS3G_ORG_EnquiryUpdationRow.Currency_ID = Convert.ToInt32(ddlCurrencyCode.SelectedValue);
            if (ddlAssetDetails.SelectedIndex > 0)
            {
                ObjS3G_ORG_EnquiryUpdationRow.Asset_ID = Convert.ToInt32(ddlAssetDetails.SelectedValue);
            }
            ObjS3G_ORG_EnquiryUpdationRow.Remarks = txtRemarks.Text;
            ObjS3G_ORG_EnquiryUpdationRow.Tenure = Convert.ToInt32(txtTenure.Text);
            ObjS3G_ORG_EnquiryUpdationRow.Constitution_ID = Convert.ToInt32(ddlConstitutionId.SelectedValue);
            ObjS3G_ORG_EnquiryUpdationRow.Tenure_Type_ID = Convert.ToInt32(ddlTenureType.SelectedValue);
            ObjS3G_ORG_EnquiryUpdationRow.Interest_Type_ID = Convert.ToInt32(ddlInterestType.SelectedValue);
            ObjS3G_ORG_EnquiryUpdationRow.Interest_Percentage = Convert.ToDecimal(txtInterestPercentage.Text);
            if (rdblAssetType.Visible)
                ObjS3G_ORG_EnquiryUpdationRow.Asset_Type = Convert.ToInt16(rdblAssetType.SelectedValue);
            if (!string.IsNullOrEmpty(txtMargine.Text.Trim()))
                ObjS3G_ORG_EnquiryUpdationRow.Margin_Amount = Convert.ToDecimal(txtMargine.Text);
            if (!string.IsNullOrEmpty(txtResidualValue.Text.Trim()))
                ObjS3G_ORG_EnquiryUpdationRow.Residual_Value = Convert.ToDecimal(txtResidualValue.Text);
            ObjS3G_ORG_EnquiryUpdationRow.Created_By = ObjUserInfo.ProUserIdRW;
            ObjS3G_ORG_EnquiryUpdationRow.Created_On = System.DateTime.Now;
            ObjS3G_ORG_EnquiryUpdationRow.Modified_On = System.DateTime.Now;
            ObjS3G_ORG_EnquiryUpdationRow.Modified_By = ObjUserInfo.ProUserIdRW;
            ObjS3G_ORG_EnquiryUpdationRow.Company_ID = intCompanyId;
            //ObjS3G_ORG_EnquiryUpdationTable.AddS3G_ORG_EnquiryUpdationRow(ObjS3G_ORG_EnquiryUpdationRow);
            objS3G_EnquiryReferenceTable.AddS3G_ORG_EnquiryUpdationRow(ObjS3G_ORG_EnquiryUpdationRow);
            // ObjS3G_CashflowMaster.AddS3G_ORG_CashflowMasterRow(ObjCashflowMasterRow);


            SerializationMode SMode = SerializationMode.Binary;
            if (intEnquiryUpdationId == 0)
            {
                //intErrorCode = objEnquiryUpdate.FunPubInsertEnquiryUpdation(out strEntityCode, SMode, ClsPubSerialize.Serialize(ObjS3G_ORG_EnquiryUpdationTable, SMode));
                intErrorCode = objEnquiryUpdate.FunPubInsertEnquiryUpdation(out strEntityCode, SMode, ClsPubSerialize.Serialize(objS3G_EnquiryReferenceTable, SMode));
                txtEnqRefNo.Text = strEntityCode;
            }
            else
            {
                intErrorCode = objEnquiryUpdate.FunPubModifyEnquiryUpdation(SMode, ClsPubSerialize.Serialize(objS3G_EnquiryReferenceTable, SMode));
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryUpdate.Close();
        }
    }
    private DataTable FunPriAddSerialNumber(DataTable dt)
    {
        try
        {
            DataColumn dc = new DataColumn("SerialNumber");
            dt.Columns.Add(dc);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i_serialNum = 0; i_serialNum < dt.Rows.Count; i_serialNum++)
                {
                    dt.Rows[i_serialNum]["SerialNumber"] = (i_serialNum + 1).ToString();
                }
            }
            return dt;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
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
    // Written by k irsath ameen on 25-01-2011 for UAT -ENQU_002
    private void FunPriDisableResidualvalue()
    {
        try
        {
            string LOBCode = FunPriGetLOBCode(ddlFacilityType.SelectedItem.Text);
            string[] LOBCodes1 = new string[] { "HP", "LN", "TL", "TE" };
            string[] LOBCodes2 = new string[] { "OL", "FL" };
            string[] LOBCodes3 = new string[] { "WC", "FT" };
            foreach (string Code in LOBCodes1)
            {
                if (Code.Contains(LOBCode.ToUpper().Trim()))
                {
                    txtResidualValue.Enabled = false;
                    //txtMargine.Attributes.Add("onblur", "ChkIsZero(this,'Margin amount')");
                    txtResidualValue.Text = string.Empty;
                    txtMargine.Enabled = true;
                    tcEnquiryUpdation.Tabs[1].Visible = true;
                    txtMargine.Focus();
                    break;
                }
            }
            foreach (string Code in LOBCodes2)
            {
                if (Code.Contains(LOBCode.ToUpper().Trim()))
                {
                    txtMargine.Enabled = false;
                    //txtMargine.Attributes.Add("onblur", "ChkIsZero(this,'Margin amount')");
                    txtMargine.Text = string.Empty;
                    txtResidualValue.Enabled = true;
                    tcEnquiryUpdation.Tabs[1].Visible = true;
                    txtResidualValue.Focus();
                    break;
                }
            }
            foreach (string Code in LOBCodes3)
            {
                if (Code.Contains(LOBCode.ToUpper().Trim()))
                {
                    txtResidualValue.Text = txtMargine.Text = string.Empty;
                    txtResidualValue.Enabled = txtMargine.Enabled = false;
                    //                    tcEnquiryUpdation.Tabs[1].Visible = false;
                    //Changed By Thangam on 27/Feb/2012
                    tbRepayment.Enabled = false;
                    break;
                }
            }

            //if (txtResidualValue.Enabled != false)
            //{
            //    if (ddlInterestType.SelectedIndex != 2)
            //    {
            //        txtResidualValue.Enabled = true;
            //        txtMargine.Attributes.Add("onblur", "ChkIsZero(this,'Margin amount');EnableDisableResidual(this," + txtResidualValue.ClientID + ")");
            //        txtResidualValue.Attributes.Add("onblur", "ChkIsZero(this,'Residual value');EnableDisableResidual(this," + txtMargine.ClientID + ")");
            //    }
            //    else
            //        txtResidualValue.Enabled = false;
            //}

            //if (ddlFacilityType.SelectedItem.Text.Contains("FT  -  Factoring") || ddlFacilityType.SelectedItem.Text.Contains("WC - Working Capital"))
            //    tcEnquiryUpdation.ActiveTabIndex = 0;
            //else
            //    tcEnquiryUpdation.ActiveTabIndex = 1;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
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

            //<<Performance>>
            //txtComCity.FillDataTable(dtSource, "Name", "Name", false);
            //txtPerCity.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 2").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 2").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            ddlState.BindDataTable(dtSource, new string[] { "ID", "NAME" });

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            ddlCountry.BindDataTable(dtSource, new string[] { "ID", "NAME" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("Category");

        return dt;
    }

    private void FunPriLoadCustomerAddress(int intCustomerID)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
        Procparam.Add("@CUSTOMER_ID", intCustomerID.ToString());
        DataTable dt = Utility.GetDefaultData("S3G_ORG_GET_CUSTOMER_ENQUIRY", Procparam);
        DataRow dtEnqExistCusRow = dt.Rows[0];
        //string strCustomerAddress = SetCustomerAddress(dtEnqExistCusRow["comm_address1"].ToString(), dtEnqExistCusRow["comm_address2"].ToString(), dtEnqExistCusRow["comm_city"].ToString(), dtEnqExistCusRow["comm_state"].ToString(), dtEnqExistCusRow["comm_country"].ToString(), dtEnqExistCusRow["comm_pincode"].ToString());
        string strCustomerAddress = dtEnqExistCusRow["comm_address1"].ToString() + "\n" + dtEnqExistCusRow["comm_address2"].ToString();
        txtCustomerName.Text = dtEnqExistCusRow["Customer_Name"].ToString();
        txtAddress.Text = strCustomerAddress;
        txtCity.Text = dtEnqExistCusRow["Comm_City"].ToString();
        ListItem lstState = new ListItem(dtEnqExistCusRow["Comm_State"].ToString());
        ddlState.Items.Add(lstState);
        ddlState.ClearDropDownList();
        ListItem lstCountry = new ListItem(dtEnqExistCusRow["Comm_Country"].ToString());
        ddlCountry.Items.Add(lstCountry);
        ddlCountry.ClearDropDownList();
        ListItem lstConstitution = new ListItem(dtEnqExistCusRow["Constitution_Name"].ToString(), dtEnqExistCusRow["Constitution_ID"].ToString());
        ddlConstitutionId.Items.Add(lstConstitution);
        ddlConstitutionId.ClearDropDownList();
        txtPinCode.Text = dtEnqExistCusRow["Comm_Pincode"].ToString();
        ucAutoSuggest.Visible = false;
        txtMobile.Text = dtEnqExistCusRow["Comm_Mobile"].ToString();
        txtEmail.Text = dtEnqExistCusRow["Comm_EMail"].ToString();
    }

    private string FunPriGetLOBCode(string LOBName)
    {
        try
        {
            string LOBCode = Convert.ToString(LOBName.Substring(0, LOBName.LastIndexOf("-")));
            return LOBCode.Trim();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    #region Button Events
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!FunPriIsValid())
            { return; }
            //if (txtMargine.Text == string.Empty && txtResidualValue.Text == string.Empty)
            //{
            //    if (txtResidualValue.Enabled)
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "Enter residual value");
            //        txtResidualValue.Focus();
            //        return;
            //    }
            //    else if (txtMargine.Enabled)
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "Enter margin amount");
            //        txtMargine.Focus();
            //        return;
            //    }
            //}
            if (txtFacilityAmount.Text == "" && txtFacilityAmount.Text == "0")
            {
                Utility.FunShowAlertMsg(this, "Facility amount cannot be zero or empty.");
                return;
            }

            if (txtResidualValue.Text != string.Empty)
            {
                if (Convert.ToDecimal(txtResidualValue.Text) >= Convert.ToDecimal(txtFacilityAmount.Text))
                {
                    Utility.FunShowAlertMsg(this, "Residual Value should be less than Facility Amount/Asset Value");
                    return;
                }
            }

            FunPriSaveEnquiryDetails();
            if (intErrorCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intEnquiryUpdationId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Enquiry details updated successfully");
                }
                else
                {
                    strAlert = "Enquiry Reference Number is " + strEntityCode;
                    strAlert += @"\n\nEnquiry details added successfully";
                    strAlert += @"\n\nWould you like to add one more Enquiry?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
            }
            else if (intErrorCode == -1)
            {

                txtEnqRefNo.Text = "";
                //Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                strRedirectPageView = "";
            }
            else if (intErrorCode == -2)
            {
                txtEnqRefNo.Text = "";
                // Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create EnquiryUpdation");
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                strRedirectPageView = "";
            }
            else
            {
                if (intEnquiryUpdationId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Error in updating Enquiry details");
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Error in adding Enquiry details");
                }
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            if (intEnquiryUpdationId > 0)
            {
                lblErrorMessage.Text = "Due to Enquiry Management Service problem,Unable to Modify Enquiry Details";
            }
            else
            {
                lblErrorMessage.Text = "Due to Enquiry Management Service problem,Unable to Create Enquiry Details";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            if (intEnquiryUpdationId > 0)
            {
                lblErrorMessage.Text = "Unable to Modify Enquiry Details";
            }
            else
            {
                lblErrorMessage.Text = "Unable to Create Enquiry Details";
            }
        }
        finally
        {
            Session["EnqNewCustomerId"] = null;
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    { }
    protected void btnSchedule_Click(object sender, EventArgs e)
    {
        //if (ddlFacilityType.SelectedItem.Text.Contains("FT  -  Factoring") || ddlFacilityType.SelectedItem.Text.Contains("WC - Working Capital"))
        //{
        //    tbRepayment.Enabled = false;
        //    return;
        //}

        #region  To get Round Off value from Glopbal parameter setup

        Dictionary<string, string> dictRoundOFF = new Dictionary<string, string>();
        dictRoundOFF.Add("@Company_ID", Convert.ToString(intCompanyId));
        DataTable dtRoundOff = Utility.GetDefaultData("S3g_ORG_GetGlobalIRRDetails", dictRoundOFF);
        if (dtRoundOff != null && dtRoundOff.Rows.Count > 0)
        {
            decRoundOff = Convert.ToDecimal(dtRoundOff.Rows[0]["Roundoff"].ToString());
        }

        #endregion

        CommonS3GBusLogic objCommonS3GBusLogic = new CommonS3GBusLogic();
        try
        {
            if (Page.IsValid)
            {
                if (!FunPriIsValidMargin())
                { return; }
                tcEnquiryUpdation.ActiveTabIndex = 1;
                //tbRepayment.Enabled = true;
                DataTable dtRepay = new DataTable();
                decimal Intamount = 0;
                decimal decTotalAmt = 0;

                if (ddlInterestType.SelectedItem.Text.ToUpper() == "RATE")
                {

                    string strFreQuency = string.Empty;
                    if (ddlTenureType.SelectedItem.Text.ToUpper() == "MONTHS")
                        strFreQuency = "4";
                    else if (ddlTenureType.SelectedItem.Text.ToUpper() == "DAYS")
                        strFreQuency = "0";
                    else if (ddlTenureType.SelectedItem.Text.ToUpper() == "WEEKS")
                        strFreQuency = "2";

                    bool blnIsRounded = false;

                    dtRepay = objCommonS3GBusLogic.FunPubCalculateRepaymentDetails(strFreQuency, int.Parse(txtTenure.Text),
                        ddlTenureType.SelectedItem.Text, decimal.Parse(txtFacilityAmount.Text),
                        decimal.Parse(txtInterestPercentage.Text), RepaymentType.EMI, null,
                        Convert.ToDateTime(DateTime.Now.ToShortDateString()), Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                        decRoundOff, out blnIsRounded, "advance");
                }
                else
                {
                    DataColumn dcInstalment = new DataColumn("InstallmentDate");
                    DataColumn dcAmount = new DataColumn("InstallmentAmount");
                    dtRepay.Columns.Add(dcInstalment);
                    dtRepay.Columns.Add(dcAmount);
                    DataRow drRepayRow;

                    string strInsDate = "";
                    DateTime dateMonthlyInsDate = DateTime.Now.Date;
                    DateTime locDateInsDate = dateMonthlyInsDate;
                    DateTime locDateInsDate1 = dateMonthlyInsDate;
                    if (ddlTenureType.SelectedItem.Text.ToUpper() == "MONTHS")
                    {
                        dateMonthlyInsDate = dateMonthlyInsDate.AddMonths(1);
                    }
                    else if (ddlTenureType.SelectedItem.Text.ToUpper() == "WEEKS")
                    {
                        dateMonthlyInsDate = dateMonthlyInsDate.AddDays(7);
                    }
                    else if (ddlTenureType.SelectedItem.Text.ToUpper() == "DAYS")
                    {
                        dateMonthlyInsDate = dateMonthlyInsDate.AddDays(1);
                    }
                    for (int i = 0; i < Convert.ToInt32(txtTenure.Text); i++)
                    {
                        drRepayRow = dtRepay.NewRow();
                        if (locDateInsDate.Day == 31)
                        {
                            DateTime loc = LastDate(dateMonthlyInsDate);
                            strInsDate = loc.ToString();//.ToString(objS3GSession.ProDateFormatRW);
                            dateMonthlyInsDate = dateMonthlyInsDate.AddMonths(1);
                        }
                        else
                        {
                            strInsDate = dateMonthlyInsDate.ToString();//.ToString(objS3GSession.ProDateFormatRW);
                            if (ddlTenureType.SelectedItem.Text.ToUpper() == "MONTHS")
                            {
                                if (dateMonthlyInsDate.Month != 2)
                                    dateMonthlyInsDate = dateMonthlyInsDate.AddMonths(1);
                                else
                                {
                                    dateMonthlyInsDate = locDateInsDate1.AddMonths(i + 2);
                                }
                            }
                            else if (ddlTenureType.SelectedItem.Text.ToUpper() == "WEEKS")
                            {
                                dateMonthlyInsDate = dateMonthlyInsDate.AddDays(7);
                            }
                            else if (ddlTenureType.SelectedItem.Text.ToUpper() == "DAYS")
                            {
                                dateMonthlyInsDate = dateMonthlyInsDate.AddDays(1);
                            }
                        }
                        drRepayRow[0] = strInsDate;
                        if (i == 0)
                        {
                            Intamount = CalculateInstalmentAmount();
                        }
                        drRepayRow[1] = Intamount.ToString();  //Convert.ToString(CalculateInstalmentAmount());
                        dtRepay.Rows.Add(drRepayRow);
                        decTotalAmt = decTotalAmt + Convert.ToDecimal(Intamount);
                    }

                    //dtRepay.Columns["InstallmentAmount"].DefaultValue = Math.Round(decPerInstallmentAmt, 0);
                    decimal decActualAmount = 0;
                    foreach (DataRow drRepaymentRow in dtRepay.Rows)
                    {
                        drRepaymentRow["InstallmentAmount"] = Math.Round((Convert.ToDecimal(drRepaymentRow["InstallmentAmount"].ToString()) / decRoundOff), 0) * decRoundOff;

                        //drRepaymentRow["Amount"] = Math.Round(decTotalAmt, 0);
                        decActualAmount = decActualAmount + Convert.ToDecimal(drRepaymentRow["InstallmentAmount"].ToString());
                    }


                    //if (returnType != RepaymentType.PMPL && returnType != RepaymentType.PMPM && returnType != RepaymentType.PMPT)
                    //{
                    //decimal decActualAmount = (decimal)dtRepay.Compute("Sum(Amount)", "Amount >=0 ");
                    decimal decbalamt;
                    if (decActualAmount < decTotalAmt)
                    {
                        //decbalamt = Convert.ToInt64(decTotalAmt) - decActualAmount;
                        decbalamt = decTotalAmt - decActualAmount;
                        dtRepay.Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dtRepay.Rows[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
                    }
                    else if (decActualAmount > decTotalAmt)
                    {
                        //decbalamt = decActualAmount - Convert.ToInt64(decTotalAmt);
                        decbalamt = decActualAmount - decTotalAmt;
                        dtRepay.Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dtRepay.Rows[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
                    }
                    //}
                }

                ViewState["dtRepay"] = GridView1.DataSource = FunPriAddSerialNumber(dtRepay);
                GridView1.DataBind();
            }
            else
            {
                tcEnquiryUpdation.ActiveTabIndex = 0;
            }
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = "Unable to show the RepaymentSchedule";
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
        finally
        {
            objCommonS3GBusLogic = null;
        }
        btnSchedule.Enabled = (tcEnquiryUpdation.ActiveTabIndex == 0) ? true : false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                strAlert = "window.close();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strAlert, true);
            }
            else
            {
                FunPriClearControls();
                Session["EnqNewCustomerId"] = null;
                Response.Redirect("~/Origination/S3gOrgEnquiryUpdation_View.aspx", false);
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #region  DateFormat
    public string FormatDate(string strDate)
    {
        return DateTime.Parse(strDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(objS3GSession.ProDateFormatRW);
    }
    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            DataTable dtTable = new DataTable();
            dtTable = null;
            GridView1.DataSource = dtTable;
            GridView1.PageIndex = 0;
            GridView1.DataBind();
            //lblCreate.Enabled = true;
            //lnkCreate.Visible = true;
            //lnkCreate.Attributes.Add("onclick", strNewWin);
            //////ddlCustomer.Enabled = false;
            ddlCurrencyCode.SelectedIndex = 1;
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = Resources.LocalizationResources.ClearError;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
        finally
        {
            Session["EnqNewCustomerId"] = null;
        }
    }
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            //S3GCustomerAddress1.ClearCustomerDetails();
            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCID != null && hdnCID.Value != "")
            {
                int CustomerID = Convert.ToInt32(hdnCID.Value);
                //FunPubQueryExistCustomerListEnquiryUpdation(CustomerID);
                FunPriLoadCustomerAddress(CustomerID);
                //FunPriClearControlsforDropDown();
                //ddlCurrencyCode.SelectedIndex = 1;
            }
            else
            {
                FunPriClearCusdetails();
                //ddlCurrencyCode.SelectedIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region Gridview Events
    // Added by S.Kannan - to make the "btnSchedule" disable in "Repayment Schedule" tab
    protected void tcEnquiryUpdation_ActiveTabChanged(object sender, EventArgs e)
    {
        try
        {
            if (tcEnquiryUpdation.ActiveTabIndex == 0)
            {
                //btnSchedule.Enabled = true;
                //tbRepayment.Enabled = false;
            }
            else
            {
                // This Lines added for fire the btnSchedule events for Repayment     
                GridView1.ClearGrid();
                if (!ddlFacilityType.SelectedItem.Text.Contains("FT  -  Factoring") && !ddlFacilityType.SelectedItem.Text.Contains("WC  -  Working Capital"))
                {
                    IPostBackEventHandler Repayment = (IPostBackEventHandler)this.btnSchedule;
                    Repayment.RaisePostBackEvent(string.Empty);
                }

                //btnSchedule_Click(sender, e);
                //btnSchedule.Enabled = false;

            }
            //btnSchedule.Enabled = (tcEnquiryUpdation.ActiveTabIndex == 0) ? true : false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            int newPageIndex = 1;
            if (ViewState["PreviousPageIndex"] == null)
                ViewState["PreviousPageIndex"] = 1;
            //else
            //{
            if (((int)ViewState["PreviousPageIndex"]) > (e.NewPageIndex + 1))
            {
                newPageIndex = e.NewPageIndex - 1;
            }
            else
            {
                newPageIndex = e.NewPageIndex + 1;
            }
            //}
            if (ViewState["dtRepay"] != null)
                GridView1.DataSource = (DataTable)ViewState["dtRepay"];
            GridView1.PageIndex = (newPageIndex - 1);
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }
    #endregion

    #region DropDown Events
    protected void ddlNewCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlNewCustomer.SelectedIndex == 1)
            {
                FunPriToggleNewCustomerControls(false);
                //lblCustomerCode.Visible = false;
                txtCustomerCode.Visible = false;
                //////rfvCustomerCode.Enabled = true;
                //lnkCreate.Visible = false;
                //lnkCreate.Attributes.Remove("onclick");
                //lnkCreate.Visible = false;
                //S3GCustomerAddress1.ClearCustomerDetails();
                //////ddlCustomer.SelectedIndex = 0;  
                ddlConstitutionId.Items.Clear();
                ddlState.Items.Clear();
                ddlCountry.Items.Clear();
                txtCustomerName.Text = string.Empty;
                txtCustomerName.Attributes.Add("readonly", "readonly");
                TextBox txtItemName = (TextBox)ucAutoSuggest.FindControl("txtItemName");
                txtItemName.Text = string.Empty;
                txtPinCode.Text = string.Empty;
                txtPinCode.Attributes.Add("readonly", "readonly");
                txtMobile.Text = string.Empty;
                txtMobile.Attributes.Add("readonly", "readonly");
                txtEmail.Text = string.Empty;
                txtEmail.Attributes.Add("readonly", "readonly");
                txtAddress.Text = string.Empty;
                txtAddress.Attributes.Add("readonly", "readonly");
                txtCity.Attributes.Add("readonly", "readonly");
                txtCity.Visible = true;
                txtCity.Text = string.Empty;
                ucAutoSuggest.Visible = false;
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Text = string.Empty;
                Session["EnqNewCustomerId"] = null;
                rfvCustomerName.Enabled = false;
                rfvAddress.Enabled = false;
                rfvState.Enabled = false;
                rfvMobile.Enabled = false;
                revEmailId.Enabled = false;
            }
            else
            {
                FunPriToggleNewCustomerControls(true);
                FunProLoadAddressCombos();
                FunPriLoadConstitution();
                txtCity.Visible = false;
                ucAutoSuggest.Visible = true;
                txtCustomerName.Attributes.Remove("readonly");
                txtPinCode.Attributes.Remove("readonly");
                txtMobile.Attributes.Remove("readonly");
                txtEmail.Attributes.Remove("readonly");
                txtAddress.Attributes.Remove("readonly");
                txtCustomerName.Text = string.Empty;
                TextBox txtItemName = (TextBox)ucAutoSuggest.FindControl("txtItemName");
                txtItemName.Text = string.Empty;
                txtPinCode.Text = string.Empty;
                txtMobile.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtAddress.Text = string.Empty;
                rfvCustomerName.Enabled = true;
                rfvAddress.Enabled = true;
                rfvState.Enabled = true;
                rfvMobile.Enabled = true;
                revEmailId.Enabled = false;
                //////rfvCustomerCode.Enabled = false;
                //lnkCreate.Visible = true;
                //S3GCustomerAddress1.ClearCustomerDetails();
                //////ddlCustomer.SelectedIndex = 0;           
                //lnkCreate.Attributes.Add("onclick", strNewWin);
                //lnkCreate.Attributes.Add("onclick", "window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&NewCustomerID=0', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;");
                //lnkCreate.Visible = true;

            }
            ddlNewCustomer.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void ddlAssetDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlAssetDetails.ToolTip = ddlAssetDetails.SelectedItem.Text;
        if (ddlAssetDetails.SelectedIndex > 0)
        {
            //if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")//For Existing Asset
            //{
            //   txtFacilityAmount.ReadOnly = true;
            //}
            //else // For New Asset
            //{
            //    txtFacilityAmount.ReadOnly = false;
            //}
            if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "1")//For Existing Asset
            {
                FungetLARassetvalue();
            }
            else if (rdblAssetType.Visible && rdblAssetType.SelectedValue == "0")
            {
                //txtFacilityAmount.Text = "";
            }


        }



    }
    //protected void ddlInterestType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try 
    //    {
    //        if (ddlInterestType.SelectedIndex == 1)
    //        {
    //            //txtResidualValue.Enabled = true;
    //            if (ddlFacilityType.SelectedIndex > 0)
    //                FunPriDisableResidualvalue();
    //            txtMargine.Focus();
    //            txtResidualValue.Text = string.Empty;                
    //            txtResidualValue.Attributes.Add("onblur", "ChkIsZero(this,'Residual value');EnableDisableResidual(this," + txtMargine.ClientID + ")");
    //            if (txtResidualValue.Enabled)
    //                txtMargine.Attributes.Add("onblur", "ChkIsZero(this,'Margin amount');EnableDisableResidual(this," + txtResidualValue.ClientID + ")");
    //        }
    //        else if (ddlInterestType.SelectedIndex == 2)
    //        {
    //            if (ddlFacilityType.SelectedIndex > 0)
    //                FunPriDisableResidualvalue();
    //            txtResidualValue.Text = string.Empty;                
    //            txtMargine.Focus();
    //            txtResidualValue.Enabled = false;
    //            txtMargine.Attributes.Add("onblur", "ChkIsZero(this,'Margin amount')");
    //        }
    //        else if (ddlInterestType.SelectedIndex == 0)
    //        {
    //            txtMargine.Text = string.Empty;
    //            txtResidualValue.Text = string.Empty;
    //        }
    //        if (strMode == "M")
    //        {
    //            txtMargine.Enabled = true;
    //            txtMargine.Focus();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        throw ex;
    //    }
    //}  
    protected void ddlFacilityType_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            //tcEnquiryUpdation.Tabs[1].Enabled = true;
            tbRepayment.Enabled = true;

            if (ddlFacilityType.SelectedIndex > 0)
                FunPriDisableResidualvalue();
            if (strMode != "M" && txtFacilityAmount.Text == string.Empty)
            { txtFacilityAmount.Focus(); }

            if (ddlFacilityType.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower() == "ol")
            {
                rdblAssetType.Visible = true;
                rfvAssetDetails.Enabled = true;
                lblAssetDetails.Attributes.Add("class", "styleReqFieldLabel");
                lblFacilityAmount.Text = "Asset Value";
            }
            else
            {
                rdblAssetType.Visible = false;
                rfvAssetDetails.Enabled = false;
                lblAssetDetails.Attributes.Add("class", "");
                lblFacilityAmount.Text = "Facility Amount";
            }

            txtFacilityAmount.Text = txtTenure.Text = txtInterestPercentage.Text =
                txtMargine.Text = txtResidualValue.Text = txtRemarks.Text = string.Empty;

            ddlCurrencyCode.SelectedIndex = 1;

            ddlTenureType.SelectedValue =
                ddlInterestType.SelectedValue = ddlAssetDetails.SelectedValue = "0";

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void rdblAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFacilityAmount.Text = "";
        if (rdblAssetType.SelectedValue == "1")
        {
            txtFacilityAmount.ReadOnly = true;
        }
        else
        {
            txtFacilityAmount.ReadOnly = false;
        }
        FunPubQueryAssetCodeListEnquiryUpdation();
    }
    #endregion
    #region Service Methods
    [System.Web.Services.WebMethod]
    public static string[] GetCityList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", objPageClass.intCompanyId.ToString());
        Procparam.Add("@Category", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup_AGT", Procparam), false);

        return suggetions.ToArray();
    }
    #endregion


}

