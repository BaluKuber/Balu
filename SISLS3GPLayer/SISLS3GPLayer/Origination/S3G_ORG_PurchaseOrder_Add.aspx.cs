#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Purchase Order
/// Created By			: Chandrasekar K
/// Created Date		: 15-Oct-2014
/// <Program Summary>
#endregion

#region Name Spaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
//using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;
using System.Collections;

#endregion

#region Delivery Instruction / LPO
public partial class Origination_S3G_ORG_PurchaseOrder_Add : ApplyThemeForProject
{
    #region Intialization
    Dictionary<string, string> dictParam = null;
    string strDateFormat;
    int intErrCode;
    int intPO_Hdr_ID;
    int intUserId;
    int intCompanyId;
    string _DINo;
    string strMode;
    string strPO_Type;
    int i;
    string s;
    bool status;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    string strKey = "Insert";
    static string strPageName = "DeliveryInstruction / LPO";
    DataTable dtPO = new DataTable();
    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=PURORD';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_PurchaseOrder_Add.aspx?qsMode=C';";
    string strExceptionEmail = "";
    string filePath_zip = "";
    Dictionary<string, string> Procparam_PO_PDF = new Dictionary<string, string>();
    SerializationMode SerMode = SerializationMode.Binary;
    LoanAdminMgtServicesClient objLoanAdmin_MasterClient;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjPOService;
    UserInfo ObjUserInfo = new UserInfo();
    LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable ObjS3G_GetAssetDataTable = new LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable();
    LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable ObjS3G_GetCustDataTable = new LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable();
    public static Origination_S3G_ORG_PurchaseOrder_Add obj_Page;

    public int ProPageNumRW                                                     // to retain the current page size and number
    {
        get;
        set;
    }
    public int ProPageSizeRW
    {
        get;
        set;
    }

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {

        ProgramCode = "278";
        obj_Page = this;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            //ADDED BY VINODHA M FOR (TO LOCK UNIQUE RECORD WHEN MORE THAN ONE USER TRY TO MODIFY) STARTS
            string[] strFromTicket = fromTicket.Name.Split('~');
            intPO_Hdr_ID = Convert.ToInt32(strFromTicket[0].ToString());
            //ADDED BY VINODHA M FOR (TO LOCK UNIQUE RECORD WHEN MORE THAN ONE USER TRY TO MODIFY) ENDS
            strMode = Request.QueryString["qsMode"];
        }
        Request.QueryString["qsMode"].ToString();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bQuery = ObjUserInfo.ProViewRW;
        lblCompanyName.Text = ObjUserInfo.ProCompanyNameRW.ToUpper();

        txtDate.Attributes.Add("readonly", "readonly");
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        txtDate.Text = DateTime.Today.ToString(strDateFormat);
        txtDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

        ProPageNumRW = 1;                                                           // to set the default page number
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        if (txtPageSize.Text != "")
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        else
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));
        PageAssignValue obj = new PageAssignValue(this.AssignValue);
        ucCustomPaging.callback = obj;
        ucCustomPaging.ProPageNumRW = ProPageNumRW;
        ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

        ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
        TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
        txtUserName.Attributes.Add("onfocus", "fnLoadCustomer()");
        txtUserName.ToolTip = txtUserName.Text;
        if (Request.QueryString["POType"] == "1")
            strPO_Type = "1";
        else
            strPO_Type = "0";

        if (!IsPostBack)
        {

            if (Request.QueryString["qsMode"] == "Q")
            {
                FunPriDisableControls(-1);
            }
            else if (Request.QueryString["qsMode"] == "M")
            {
                FunPriDisableControls(1);
                Cache["PODetails"] = gvDlivery.DataSource;
            }
            else
            {
                FunPriDisableControls(0);
                FunPriGetCompanyInfo();
                FunPriDeleteGrid();
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "GetChildGridResize()", true);
            dictParam = new Dictionary<string, string>();
            dictParam.Clear();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            GSTEffectiveDate.Text = Utility.GetDefaultData("S3G_SYSAD_GSTEFFECTIVEDATE", dictParam).Rows[0]["GST_Effective_From"].ToString();


        }

        if (PageMode == PageModes.WorkFlow && !IsPostBack)
        {
            try
            {
                PreparePageForWFLoad();
            }
            catch (Exception ex)
            {

                Utility.FunShowAlertMsg(this, "Invalid data to load, access side menu");
            }
        }
    }

    private void PreparePageForWFLoad()
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();

            DataRow HeaderValues = GetHeaderDetailsFromDOCNo(WFSessionValues.PANUM, ProgramCode);
            FunPriGetCompanyInfo();

        }
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            gvDlivery.EditIndex = -1;
            ProPageNumRW = intPageNum;              // To set the page Number
            ProPageSizeRW = intPageSize;            // To set the page size    
            FunPriGetPuchaseGridDetails();                       // Binding the Landing grid
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region GetValues for DI

    #region Company Info


    private void FunPriGetCompanyInfo()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtVendorDetails = Utility.GetDefaultData("S3G_LOANAD_GetCompanyInfo", dictParam);
            DataRow dtRow = dtVendorDetails.Rows[0];
            lblCCity.Text = dtRow["City"].ToString();
            lblCZipcode.Text = dtRow["Zip_code"].ToString();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    #endregion

    #region GetVendor

    protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlAssetCategory = (DropDownList)gvDlivery.FooterRow.FindControl("ddlAssetCategory");
            DropDownList ddlAssetType = (DropDownList)gvDlivery.FooterRow.FindControl("ddlAssetType");

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", "2");
            dictParam.Add("@Parent_ID", ddlAssetCategory.SelectedValue);
            DataTable dtAssetType = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
            ddlAssetType.BindDataTable(dtAssetType, "Id", "Description");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    protected void ddlAssetCategoryhdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int intRowId = Utility.FunPubGetGridRowID("gvDlivery", ((DropDownList)sender).ClientID);
            DropDownList ddlAssetCategoryhdr = (DropDownList)gvDlivery.Rows[intRowId].FindControl("ddlAssetCategoryhdr");
            DropDownList ddlAssetTypehdr = (DropDownList)gvDlivery.Rows[intRowId].FindControl("ddlAssetTypehdr");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", "2");
            dictParam.Add("@Parent_ID", ddlAssetCategoryhdr.SelectedValue);
            DataTable dtAssetType = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
            ddlAssetTypehdr.BindDataTable(dtAssetType, "Id", "Description");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlAssetType = (DropDownList)gvDlivery.FooterRow.FindControl("ddlAssetType");
            DropDownList ddlAssetSubType = (DropDownList)gvDlivery.FooterRow.FindControl("ddlAssetSubType");

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", "3");
            dictParam.Add("@Parent_ID", ddlAssetType.SelectedValue);
            DataTable dtAssetType = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
            ddlAssetSubType.BindDataTable(dtAssetType, "Id", "Description");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    protected void ddlAssetTypehdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int intRowId = Utility.FunPubGetGridRowID("gvDlivery", ((DropDownList)sender).ClientID);
            DropDownList ddlAssetTypehdr = (DropDownList)gvDlivery.Rows[intRowId].FindControl("ddlAssetTypehdr");
            DropDownList ddlAssetSubTypehdr = (DropDownList)gvDlivery.Rows[intRowId].FindControl("ddlAssetSubTypehdr");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", "3");
            dictParam.Add("@Parent_ID", ddlAssetTypehdr.SelectedValue);
            DataTable dtAssetType = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
            ddlAssetSubTypehdr.BindDataTable(dtAssetType, "Id", "Description");
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    #endregion

    #region GetCustomerDetails
    private void FunPriGetCustomerDetails(string strMLAID)
    {
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsRow ObjCustMasterRow;
            ObjCustMasterRow = ObjS3G_GetCustDataTable.NewS3G_LOANAD_GetDICustomerDetailsRow();
            ObjCustMasterRow.Company_ID = intCompanyId;

            ObjS3G_GetCustDataTable.AddS3G_LOANAD_GetDICustomerDetailsRow(ObjCustMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteCustMasterDetails = objLoanAdmin_MasterClient.FunPubGetCustDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_GetCustDataTable, SerMode));
            LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable dtCustMaster = (LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable)ClsPubSerialize.DeSerialize(byteCustMasterDetails, SerializationMode.Binary, typeof(LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable));
            DataRow dtRow = dtCustMaster.Rows[0];
            lblCustID.Text = dtRow["Customer_ID"].ToString();


            //ViewState["dtCustMaster"] = dtCustMasterDetails;
            if (PageMode != PageModes.Create)
            {
                lblCuAddress1.Text = dtRow["Comm_Address1"].ToString();
                lblCuAddress2.Text = dtRow["Comm_Address2"].ToString();
                lblCuCity.Text = dtRow["Comm_City"].ToString();
                lblCuPincode.Text = dtRow["Comm_PINCode"].ToString();
                lblCuState.Text = dtRow["Comm_State"].ToString();
                lblCuCountry.Text = dtRow["Comm_Country"].ToString();
            }
            S3GCustomerAddress1.SetCustomerDetails(dtCustMaster.Rows[0], true);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            if (objLoanAdmin_MasterClient != null)
            {
                objLoanAdmin_MasterClient.Close();
            }
        }

    }
    #endregion

    #region GetXML
    protected string FunProFormXML()
    {
        StringBuilder strbuXML = new StringBuilder();
        strbuXML.Append("<Root>");
        foreach (GridViewRow grvData in gvDlivery.Rows)
        {

            if (((CheckBox)grvData.FindControl("CbAssets")).Checked)
            {
                //string strlblPRTID = ((Label)grvData.FindControl("lblDeliveryIns")).Text;
                string strAssetID = ((Label)grvData.FindControl("lblAssetID")).Text;
                string strModelDesc = ((TextBox)grvData.FindControl("txtModelDesc")).Text;
                string strQuly = ((TextBox)grvData.FindControl("txtQuantity")).Text;
                string strAssetValue = ((TextBox)grvData.FindControl("txtAssetValue")).Text;
                if (strAssetValue == "")
                    strAssetValue = ((TextBox)grvData.FindControl("txtAssetValue1")).Text;
                string strRemarks = ((TextBox)grvData.FindControl("txtRemarks")).Text;
                strbuXML.Append(" <Details  Asset_ID='" + strAssetID.ToString() + "' Model_description='" + strModelDesc.ToString() +
                 "' Asset_quantity='" + strQuly.ToString() + "' Asset_Value='" + strAssetValue.ToString() + "' Remarks ='" + strRemarks.ToString() + "'/>");
            }
        }
        strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }
    #endregion

    #endregion

    #region Bind Grid
    protected void gvDlivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal total = 0;
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlAssetCategory = (DropDownList)e.Row.FindControl("ddlAssetCategory");
                DropDownList ddlAssetType = (DropDownList)e.Row.FindControl("ddlAssetType");
                DropDownList ddlAssetSubType = (DropDownList)e.Row.FindControl("ddlAssetSubType");
                DropDownList ddlCustomer_State = (DropDownList)e.Row.FindControl("ddlCustomer_State");
                DropDownList ddlVendor_Branch = (DropDownList)e.Row.FindControl("ddlVendor_Branch");

                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Company_ID", intCompanyId.ToString());
                dictParam.Add("@Category_Type", "1");
                DataTable dtAssetCategory = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
                ddlAssetCategory.BindDataTable(dtAssetCategory, "Id", "Description");

                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlAssetType.Items.Insert(0, liSelect);
                ddlAssetSubType.Items.Insert(0, liSelect);

                //dictParam = new Dictionary<string, string>();
                //dictParam.Add("@Company_ID", intCompanyId.ToString());
                //dictParam.Add("@Category_Type", "2");
                //DataTable dtAssetType = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
                //ddlAssetType.BindDataTable(dtAssetType, "Id", "Description");

                //dictParam = new Dictionary<string, string>();
                //dictParam.Add("@Company_ID", intCompanyId.ToString());
                //dictParam.Add("@Category_Type", "3");
                //DataTable dtAssetSubType = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
                //ddlAssetSubType.BindDataTable(dtAssetSubType, "Id", "Description");

                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@ID", hdnCustomerId.Value);
                dictParam.Add("@Option", "1");
                DataTable dtState = Utility.GetDefaultData("S3G_ORG_GetCustomer_State", dictParam);
                ddlCustomer_State.BindDataTable(dtState, "State", "Name");

                dictParam = new Dictionary<string, string>();
                dictParam.Add("@ID", txtVendorName.SelectedValue);
                dictParam.Add("@Option", "2");
                DataTable dtVenState = Utility.GetDefaultData("S3G_ORG_GetCustomer_State", dictParam);
                ddlVendor_Branch.BindDataTable(dtVenState, "State", "Name");

                TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                TextBox txtPer_Unit_Price = (TextBox)e.Row.FindControl("txtPer_Unit_Price");
                TextBox txtBill_Amount_FC = (TextBox)e.Row.FindControl("txtBill_Amount_FC");
                TextBox txtBill_Amount_INR = (TextBox)e.Row.FindControl("txtBill_Amount_INR");
                TextBox txtBase_Inv_Amt_Mat = (TextBox)e.Row.FindControl("txtBase_Inv_Amt_Mat");
                TextBox txtBase_Inv_Amt_Lab = (TextBox)e.Row.FindControl("txtBase_Inv_Amt_Lab");
                TextBox txtVAT = (TextBox)e.Row.FindControl("txtVAT");
                TextBox txtCST = (TextBox)e.Row.FindControl("txtCST");
                TextBox txtExcise_Duty = (TextBox)e.Row.FindControl("txtExcise_Duty");
                TextBox txtService_Tax = (TextBox)e.Row.FindControl("txtService_Tax");
                TextBox txtOthers = (TextBox)e.Row.FindControl("txtOthers");
                TextBox txtTotal_Bill_Amount = (TextBox)e.Row.FindControl("txtTotal_Bill_Amount");
                TextBox txtRetention = (TextBox)e.Row.FindControl("txtRetention");
                TextBox txtASNo = (TextBox)e.Row.FindControl("txtASNo");

                txtQuantity.SetDecimalPrefixSuffix(13, 0, true, "Quantity");

                txtPer_Unit_Price.SetUserDecimalPrefixSuffix(13, 4, true, "Per Unit Price");
                txtBill_Amount_FC.SetUserDecimalPrefixSuffix(13, 4, true, "Total Bill Amount in FC");
                txtBill_Amount_INR.SetUserDecimalPrefixSuffix(13, 4, true, "Total Bill Amount in INR");
                txtBase_Inv_Amt_Mat.SetUserDecimalPrefixSuffix(13, 4, true, "Base Inv Amt(Excl Tax)-Material");
                txtBase_Inv_Amt_Lab.SetUserDecimalPrefixSuffix(13, 4, true, "Base Inv Amt(Excl Tax)-Labour");
                txtVAT.SetUserDecimalPrefixSuffix(13, 4, true, "VAT");
                txtCST.SetUserDecimalPrefixSuffix(13, 4, true, "CST");
                txtExcise_Duty.SetUserDecimalPrefixSuffix(13, 4, true, "Excise Duty");
                txtService_Tax.SetUserDecimalPrefixSuffix(13, 4, true, "Service Tax");
                txtOthers.SetUserDecimalPrefixSuffix(13, 4, true, "Others");
                txtRetention.SetUserDecimalPrefixSuffix(13, 4, true, "Retention");

                txtASNo.SetDecimalPrefixSuffix(5, 0, true, "AS No");

                UserControls_S3GAutoSuggest txtHSNCodeFT = (UserControls_S3GAutoSuggest)e.Row.FindControl("txtHSNCodeFT");
                UserControls_S3GAutoSuggest txtSACCodeFT = (UserControls_S3GAutoSuggest)e.Row.FindControl("txtSACCodeFT");
                TextBox txtHSNSGSTFT = (TextBox)e.Row.FindControl("txtHSNSGSTFT");
                TextBox txtHSNCGSTFT = (TextBox)e.Row.FindControl("txtHSNCGSTFT");
                TextBox txtHSNIGSTFT = (TextBox)e.Row.FindControl("txtHSNIGSTFT");
                TextBox txtSACSGSTFT = (TextBox)e.Row.FindControl("txtSACSGSTFT");
                TextBox txtSACCGSTFT = (TextBox)e.Row.FindControl("txtSACCGSTFT");
                TextBox txtSACIGSTFT = (TextBox)e.Row.FindControl("txtSACIGSTFT");
                UserControls_S3GAutoSuggest txtBillingStateFT = (UserControls_S3GAutoSuggest)e.Row.FindControl("txtBillingStateFT");
                UserControls_S3GAutoSuggest txtBillingBranchFT = (UserControls_S3GAutoSuggest)e.Row.FindControl("txtBillingBranchFT");
                TextBox txtServiceCode = (TextBox)e.Row.FindControl("txtServiceCode");

                txtHSNSGSTFT.SetUserDecimalPrefixSuffix(13, 4, true, "HSN SGST");
                txtHSNCGSTFT.SetUserDecimalPrefixSuffix(13, 4, true, "HSN CGST");
                txtHSNIGSTFT.SetUserDecimalPrefixSuffix(13, 4, true, "HSN IGST");
                txtSACSGSTFT.SetUserDecimalPrefixSuffix(13, 4, true, "SAC SGST");
                txtSACCGSTFT.SetUserDecimalPrefixSuffix(13, 4, true, "SAC CGST");
                txtSACIGSTFT.SetUserDecimalPrefixSuffix(13, 4, true, "SAC IGST");

                if (Utility.StringToDate(txtDate.Text) >= Utility.StringToDate(GSTEffectiveDate.Text))
                {
                    txtVAT.Enabled = txtCST.Enabled = txtExcise_Duty.Enabled = txtService_Tax.Enabled = txtOthers.Enabled = 
                        //txtServiceCode.Enabled = false;
                    txtHSNCodeFT.Enabled = txtSACCodeFT.Enabled = txtHSNSGSTFT.Enabled = txtHSNCGSTFT.Enabled = txtHSNIGSTFT.Enabled =
                        txtSACSGSTFT.Enabled = txtSACCGSTFT.Enabled = txtSACIGSTFT.Enabled = txtBillingStateFT.Enabled = txtBillingBranchFT.Enabled
                       = true;
                }
                else
                {
                    txtHSNCodeFT.Enabled = txtSACCodeFT.Enabled = txtHSNSGSTFT.Enabled = txtHSNCGSTFT.Enabled = txtHSNIGSTFT.Enabled =
                         txtSACSGSTFT.Enabled = txtSACCGSTFT.Enabled = txtSACIGSTFT.Enabled = txtBillingStateFT.Enabled = txtBillingBranchFT.Enabled
                        = false;
                    txtVAT.Enabled = txtCST.Enabled = txtExcise_Duty.Enabled = txtService_Tax.Enabled = txtOthers.Enabled = txtServiceCode.Enabled = true;
                }

            }
        }
        catch (Exception exp)
        {
            cvDelivery.ErrorMessage = exp.Message;
            cvDelivery.IsValid = false;
        }
    }

    protected void gvDlivery_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvDlivery.EditIndex = e.NewEditIndex;
            int intRowId = Convert.ToInt32(gvDlivery.DataKeys[e.NewEditIndex].Value.ToString()) - 1;
            rowindex.Value = intRowId.ToString();
            DataView dtPO = new DataView();
            dtPO = (DataView)Cache["PODetails"];

            //gvDlivery.DataSource = dtPO;
            //gvDlivery.DataBind();

            /*Parameter Added to get the current page number*/

            Label lblCurrentPage = (Label)ucCustomPaging.FindControl("lblCurrentPage");
            if (lblCurrentPage.Text != "")
                ProPageNumRW = Convert.ToInt32(lblCurrentPage.Text);

            FunPriGetPuchaseGridDetails();

            GridViewRow grvRow = gvDlivery.Rows[intRowId];

            DropDownList ddlAssetCategoryhdr = (DropDownList)grvRow.FindControl("ddlAssetCategoryhdr");
            DropDownList ddlAssetTypehdr = (DropDownList)grvRow.FindControl("ddlAssetTypehdr");
            DropDownList ddlAssetSubTypehdr = (DropDownList)grvRow.FindControl("ddlAssetSubTypehdr");
            DropDownList ddlCustomer_Statehdr = (DropDownList)grvRow.FindControl("ddlCustomer_Statehdr");
            DropDownList ddlVendor_Branchhdr = (DropDownList)grvRow.FindControl("ddlVendor_Branchhdr");

            TextBox txtAsset_Descirptionhdr = (TextBox)grvRow.FindControl("txtAsset_Descirptionhdr");
            TextBox txtCust_PO_Ref_Nohdr = (TextBox)grvRow.FindControl("txtCust_PO_Ref_Nohdr");
            TextBox txtQuot_Ref_Nohdr = (TextBox)grvRow.FindControl("txtQuot_Ref_Nohdr");
            //TextBox txtOPC_Billing_Addresshdr = (TextBox)grvRow.FindControl("txtOPC_Billing_Addresshdr");
            TextBox txtDelivery_Addresshdr = (TextBox)grvRow.FindControl("txtDelivery_Addresshdr");
            TextBox txtContact_Personhdr = (TextBox)grvRow.FindControl("txtContact_Personhdr");
            TextBox txtContact_Nohdr = (TextBox)grvRow.FindControl("txtContact_Nohdr");
            TextBox txtQuantityhdr = (TextBox)grvRow.FindControl("txtQuantityhdr");
            TextBox txtPer_Unit_Pricehdr = (TextBox)grvRow.FindControl("txtPer_Unit_Pricehdr");
            TextBox txtBill_Amount_FChdr = (TextBox)grvRow.FindControl("txtBill_Amount_FChdr");
            TextBox txtBill_Amount_INRhdr = (TextBox)grvRow.FindControl("txtBill_Amount_INRhdr");
            TextBox txtBase_Inv_Amt_Mathdr = (TextBox)grvRow.FindControl("txtBase_Inv_Amt_Mathdr");
            TextBox txtBase_Inv_Amt_Labhdr = (TextBox)grvRow.FindControl("txtBase_Inv_Amt_Labhdr");
            TextBox txtVAThdr = (TextBox)grvRow.FindControl("txtVAThdr");
            TextBox txtCSThdr = (TextBox)grvRow.FindControl("txtCSThdr");
            TextBox txtExcise_Dutyhdr = (TextBox)grvRow.FindControl("txtExcise_Dutyhdr");
            TextBox txtService_Taxhdr = (TextBox)grvRow.FindControl("txtService_Taxhdr");
            TextBox txtOthershdr = (TextBox)grvRow.FindControl("txtOthershdr");
            TextBox txtTotal_Bill_Amounthdr = (TextBox)grvRow.FindControl("txtTotal_Bill_Amounthdr");
            TextBox txtRetentionhdr = (TextBox)grvRow.FindControl("txtRetentionhdr");
            TextBox txtASNohdr = (TextBox)grvRow.FindControl("txtASNohdr");

            txtQuantityhdr.SetDecimalPrefixSuffix(13, 0, true, "Quantity");

            txtPer_Unit_Pricehdr.SetUserDecimalPrefixSuffix(13, 4, false, "Per Unit Price");
            txtBill_Amount_FChdr.SetUserDecimalPrefixSuffix(13, 4, false, "Total Bill Amount in FC");
            txtBill_Amount_INRhdr.SetUserDecimalPrefixSuffix(13, 4, false, "Total Bill Amount in INR");
            txtBase_Inv_Amt_Mathdr.SetUserDecimalPrefixSuffix(13, 4, false, "Base Inv Amt(Excl Tax)-Material");
            txtBase_Inv_Amt_Labhdr.SetUserDecimalPrefixSuffix(13, 4, false, "Base Inv Amt(Excl Tax)-Labour");
            txtVAThdr.SetUserDecimalPrefixSuffix(13, 4, false, "VAT");
            txtCSThdr.SetUserDecimalPrefixSuffix(13, 4, false, "CST");
            txtExcise_Dutyhdr.SetUserDecimalPrefixSuffix(13, 4, false, "Excise Duty");
            txtService_Taxhdr.SetUserDecimalPrefixSuffix(13, 4, false, "Service Tax");
            txtOthershdr.SetUserDecimalPrefixSuffix(13, 4, false, "Others");
            txtRetentionhdr.SetUserDecimalPrefixSuffix(13, 4, false, "Retention");

            txtASNohdr.SetDecimalPrefixSuffix(5, 0, false, false, "AS No");

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", "1");
            DataTable dtAssetCategory = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
            ddlAssetCategoryhdr.BindDataTable(dtAssetCategory, "Id", "Description");

            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@ID", hdnCustomerId.Value);
            dictParam.Add("@Option", "1");
            DataTable dtCusState = Utility.GetDefaultData("S3G_ORG_GetCustomer_State", dictParam);
            ddlCustomer_Statehdr.BindDataTable(dtCusState, "State", "Name");

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@ID", txtVendorName.SelectedValue);
            dictParam.Add("@Option", "2");
            DataTable dtVenState = Utility.GetDefaultData("S3G_ORG_GetCustomer_State", dictParam);
            ddlVendor_Branchhdr.BindDataTable(dtVenState, "State", "Name");

            ddlAssetCategoryhdr.SelectedValue = dtPO[intRowId]["Asset_Category_ID"].ToString();
            ddlCustomer_Statehdr.SelectedValue = dtPO[intRowId]["Delivery_State"].ToString();
            ddlVendor_Branchhdr.SelectedValue = dtPO[intRowId]["Entity_State"].ToString();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", "2");
            dictParam.Add("@Parent_ID", ddlAssetCategoryhdr.SelectedValue);
            DataTable dtAssetType = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
            ddlAssetTypehdr.BindDataTable(dtAssetType, "Id", "Description");
            ddlAssetTypehdr.SelectedValue = dtPO[intRowId]["Asset_Type_ID"].ToString();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", "3");
            dictParam.Add("@Parent_ID", ddlAssetTypehdr.SelectedValue);
            DataTable dtAssetSubType = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);
            ddlAssetSubTypehdr.BindDataTable(dtAssetSubType, "Id", "Description");
            ddlAssetSubTypehdr.SelectedValue = dtPO[intRowId]["Asset_Sub_Type_ID"].ToString();
            UserControls_S3GAutoSuggest txtHSNCodeIT = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtHSNCodeIT");
            UserControls_S3GAutoSuggest txtSACCodeIT = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtSACCodeIT");
            TextBox txtHSNSGSTIT = (TextBox)grvRow.FindControl("txtHSNSGSTIT");
            TextBox txtHSNCGSTIT = (TextBox)grvRow.FindControl("txtHSNCGSTIT");
            TextBox txtHSNIGSTIT = (TextBox)grvRow.FindControl("txtHSNIGSTIT");
            TextBox txtSACSGSTIT = (TextBox)grvRow.FindControl("txtSACSGSTIT");
            TextBox txtSACCGSTIT = (TextBox)grvRow.FindControl("txtSACCGSTIT");
            TextBox txtSACIGSTIT = (TextBox)grvRow.FindControl("txtSACIGSTIT");
            UserControls_S3GAutoSuggest txtBillingStateIT = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtBillingStateIT");
            UserControls_S3GAutoSuggest txtBillingBranchIT = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtBillingBranchIT");

            txtHSNSGSTIT.SetUserDecimalPrefixSuffix(13, 4, true, "HSN SGST");
            txtHSNCGSTIT.SetUserDecimalPrefixSuffix(13, 4, true, "HSN CGST");
            txtHSNIGSTIT.SetUserDecimalPrefixSuffix(13, 4, true, "HSN IGST");
            txtSACSGSTIT.SetUserDecimalPrefixSuffix(13, 4, true, "SAC SGST");
            txtSACCGSTIT.SetUserDecimalPrefixSuffix(13, 4, true, "SAC CGST");
            txtSACIGSTIT.SetUserDecimalPrefixSuffix(13, 4, true, "SAC IGST");

            if (Utility.StringToDate(txtDate.Text) >= Utility.StringToDate(GSTEffectiveDate.Text))
            {
                txtVAThdr.Enabled = txtCSThdr.Enabled = txtService_Taxhdr.Enabled = false;
                txtHSNCodeIT.Enabled = txtSACCodeIT.Enabled = txtHSNSGSTIT.Enabled = txtHSNCGSTIT.Enabled = txtHSNIGSTIT.Enabled =
                    txtSACSGSTIT.Enabled = txtSACCGSTIT.Enabled = txtSACIGSTIT.Enabled = txtBillingStateIT.Enabled = txtBillingBranchIT.Enabled =
                   txtExcise_Dutyhdr.Enabled = txtOthershdr.Enabled = true;
            }
            else
            {
                txtHSNCodeIT.Enabled = txtSACCodeIT.Enabled = txtHSNSGSTIT.Enabled = txtHSNCGSTIT.Enabled = txtHSNIGSTIT.Enabled =
                     txtSACSGSTIT.Enabled = txtSACCGSTIT.Enabled = txtSACIGSTIT.Enabled = txtBillingStateIT.Enabled = txtBillingBranchIT.Enabled
                    = false;
                txtVAThdr.Enabled = txtCSThdr.Enabled = txtExcise_Dutyhdr.Enabled = txtService_Taxhdr.Enabled = txtOthershdr.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvDlivery_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            string SlNo = ((Label)gvDlivery.Rows[e.RowIndex].FindControl("lblSlNo")).Text;

            ViewState["SlNo"] = SlNo;

            FunPriGetTempPuchaseGridDetails();

            Cache["PODetails"] = gvDlivery.DataSource;
            btnSave.Enabled = true;

            ViewState["SlNo"] = null;

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void gvDlivery_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {

            decimal strvat, strcst, strECD, strSTAmount, strothers, strmaterial, strlabour, strHSNSGST, strHSNCGST, strHSNIGST, strSACSGST, strSACCGST, strSACIGST;

            GridViewRow grvRow = gvDlivery.Rows[e.RowIndex];

            Label lblPO_dtl_ID = (Label)grvRow.FindControl("lblPO_dtl_ID");
            Label lblSlNo = (Label)grvRow.FindControl("lblSlNo");

            DropDownList ddlAssetCategoryhdr = (DropDownList)grvRow.FindControl("ddlAssetCategoryhdr");
            DropDownList ddlAssetTypehdr = (DropDownList)grvRow.FindControl("ddlAssetTypehdr");
            DropDownList ddlAssetSubTypehdr = (DropDownList)grvRow.FindControl("ddlAssetSubTypehdr");
            DropDownList ddlCustomer_Statehdr = (DropDownList)grvRow.FindControl("ddlCustomer_Statehdr");
            DropDownList ddlVendor_Branchhdr = (DropDownList)grvRow.FindControl("ddlVendor_Branchhdr");
            TextBox txtCust_PO_Ref_Nohdr = (TextBox)grvRow.FindControl("txtCust_PO_Ref_Nohdr");
            TextBox txtAsset_Descirptionhdr = (TextBox)grvRow.FindControl("txtAsset_Descirptionhdr");
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */
            //            TextBox txtLoc_CodeHdr = (TextBox)grvRow.FindControl("txtLoc_CodeHdr");
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */
            TextBox txtQuot_Ref_Nohdr = (TextBox)grvRow.FindControl("txtQuot_Ref_Nohdr");
            //TextBox txtOPC_Billing_Addresshdr = (TextBox)grvRow.FindControl("txtOPC_Billing_Addresshdr");
            TextBox txtDelivery_Addresshdr = (TextBox)grvRow.FindControl("txtDelivery_Addresshdr");
            TextBox txtContact_Personhdr = (TextBox)grvRow.FindControl("txtContact_Personhdr");
            TextBox txtContact_Nohdr = (TextBox)grvRow.FindControl("txtContact_Nohdr");
            TextBox txtContact_EmailIdhdr = (TextBox)grvRow.FindControl("txtContact_EmailIdhdr");
            TextBox txtQuantityhdr = (TextBox)grvRow.FindControl("txtQuantityhdr");
            TextBox txtPer_Unit_Pricehdr = (TextBox)grvRow.FindControl("txtPer_Unit_Pricehdr");
            TextBox txtBill_Amount_FChdr = (TextBox)grvRow.FindControl("txtBill_Amount_FChdr");
            TextBox txtBill_Amount_INRhdr = (TextBox)grvRow.FindControl("txtBill_Amount_INRhdr");
            TextBox txtBase_Inv_Amt_Mathdr = (TextBox)grvRow.FindControl("txtBase_Inv_Amt_Mathdr");
            TextBox txtBase_Inv_Amt_Labhdr = (TextBox)grvRow.FindControl("txtBase_Inv_Amt_Labhdr");
            TextBox txtVAThdr = (TextBox)grvRow.FindControl("txtVAThdr");
            TextBox txtCSThdr = (TextBox)grvRow.FindControl("txtCSThdr");
            TextBox txtExcise_Dutyhdr = (TextBox)grvRow.FindControl("txtExcise_Dutyhdr");
            TextBox txtService_Taxhdr = (TextBox)grvRow.FindControl("txtService_Taxhdr");
            TextBox txtOthershdr = (TextBox)grvRow.FindControl("txtOthershdr");
            TextBox txtTotal_Bill_Amounthdr = (TextBox)grvRow.FindControl("txtTotal_Bill_Amounthdr");
            TextBox txtRetentionhdr = (TextBox)grvRow.FindControl("txtRetentionhdr");
            TextBox txtASNohdr = (TextBox)grvRow.FindControl("txtASNohdr");
            TextBox txtModel_Namehdr = (TextBox)grvRow.FindControl("txtModel_Namehdr");
            TextBox txtManufacturer_Namehdr = (TextBox)grvRow.FindControl("txtManufacturer_Namehdr");

            UserControls_S3GAutoSuggest txtHSNCodeIT = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtHSNCodeIT");
            UserControls_S3GAutoSuggest txtSACCodeIT = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtSACCodeIT");
            TextBox txtHSNSGSTIT = (TextBox)grvRow.FindControl("txtHSNSGSTIT");
            TextBox txtHSNCGSTIT = (TextBox)grvRow.FindControl("txtHSNCGSTIT");
            TextBox txtHSNIGSTIT = (TextBox)grvRow.FindControl("txtHSNIGSTIT");
            TextBox txtSACSGSTIT = (TextBox)grvRow.FindControl("txtSACSGSTIT");
            TextBox txtSACCGSTIT = (TextBox)grvRow.FindControl("txtSACCGSTIT");
            TextBox txtSACIGSTIT = (TextBox)grvRow.FindControl("txtSACIGSTIT");
            UserControls_S3GAutoSuggest txtBillingStateIT = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtBillingStateIT");
            UserControls_S3GAutoSuggest txtBillingBranchIT = (UserControls_S3GAutoSuggest)grvRow.FindControl("txtBillingBranchIT");

            if (Utility.StringToDate(txtDate.Text) < Utility.StringToDate(GSTEffectiveDate.Text))
            {
                if (txtVAThdr.Text != "" && txtCSThdr.Text != "")
                {
                    if (Convert.ToDecimal(txtVAThdr.Text) > 0 && Convert.ToDecimal(txtCSThdr.Text) > 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Either VAT or CST Only Applicable");
                        txtCSThdr.Focus();
                        return;
                    }
                }
            }

            if (txtBase_Inv_Amt_Mathdr.Text.ToString() != "")
                strmaterial = Convert.ToDecimal(txtBase_Inv_Amt_Mathdr.Text.ToString());
            else
                strmaterial = Convert.ToDecimal("0");

            if (txtBase_Inv_Amt_Labhdr.Text.ToString() != "")
                strlabour = Convert.ToDecimal(txtBase_Inv_Amt_Labhdr.Text.ToString());
            else
                strlabour = Convert.ToDecimal("0");

            if (txtHSNSGSTIT.Text.ToString() != "")
                strHSNSGST = Convert.ToDecimal(txtHSNSGSTIT.Text.ToString());
            else
                strHSNSGST = Convert.ToDecimal("0");

            if (txtHSNCGSTIT.Text.ToString() != "")
                strHSNCGST = Convert.ToDecimal(txtHSNCGSTIT.Text.ToString());
            else
                strHSNCGST = Convert.ToDecimal("0");

            if (txtHSNIGSTIT.Text.ToString() != "")
                strHSNIGST = Convert.ToDecimal(txtHSNIGSTIT.Text.ToString());
            else
                strHSNIGST = Convert.ToDecimal("0");

            if (txtSACSGSTIT.Text.ToString() != "")
                strSACSGST = Convert.ToDecimal(txtSACSGSTIT.Text.ToString());
            else
                strSACSGST = Convert.ToDecimal("0");

            if (txtSACCGSTIT.Text.ToString() != "")
                strSACCGST = Convert.ToDecimal(txtSACCGSTIT.Text.ToString());
            else
                strSACCGST = Convert.ToDecimal("0");

            if (txtSACIGSTIT.Text.ToString() != "")
                strSACIGST = Convert.ToDecimal(txtSACIGSTIT.Text.ToString());
            else
                strSACIGST = Convert.ToDecimal("0");


            if (txtHSNCodeIT.SelectedText.Trim() == "")
            {
                txtHSNCodeIT.SelectedValue = "0";
            }

            if (txtSACCodeIT.SelectedText.Trim() == "")
            {
                txtSACCodeIT.SelectedValue = "0";
            }

            if (txtBillingStateIT.SelectedText.Trim() == "")
            {
                txtBillingStateIT.SelectedValue = "0";
            }

            if (txtBillingBranchIT.SelectedText.Trim() == "")
            {
                txtBillingBranchIT.SelectedValue = "0";
            }

            if (Utility.StringToDate(txtDate.Text) >= Utility.StringToDate(GSTEffectiveDate.Text))
            {

                if (txtHSNCodeIT.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select HSN Code");
                    return;
                }

                //if (strmaterial != Convert.ToDecimal("0") && txtHSNCodeIT.SelectedValue == "0")
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Select HSN Code,Since Material Amount Entered");
                //    return;
                //}

                if (strlabour != Convert.ToDecimal("0") && txtSACCodeIT.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select SAC Code,Since Labour Amount Entered");
                    return;
                }

                if (strmaterial != Convert.ToDecimal("0"))
                {
                    if ((strHSNCGST != Convert.ToDecimal("0") || strHSNSGST != Convert.ToDecimal("0")) && strHSNIGST != Convert.ToDecimal("0"))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Either HSN CGST / HSN SGST OR HSN IGST Applicable");
                        return;
                    }
                }

                if (strlabour != Convert.ToDecimal("0"))
                {
                    if ((strSACCGST != Convert.ToDecimal("0") || strSACSGST != Convert.ToDecimal("0")) && strSACIGST != Convert.ToDecimal("0"))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Either SAC CGST / SAC SGST OR SAC IGST Applicable");
                        return;
                    }
                }

                if (Utility.StringToDate(txtDate.Text) >= Utility.StringToDate(GSTEffectiveDate.Text))
                {
                    if (txtBillingStateIT.SelectedValue == "0")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Select Billing State");
                        return;
                    }
                }
                //if (strmaterial != Convert.ToDecimal("0"))
                //{
                //    Dictionary<string, string> dictParam = new Dictionary<string, string>();
                //    dictParam.Add("@Company_ID", intCompanyId.ToString());
                //    dictParam.Add("@Type_ID", ddlAssetTypehdr.SelectedValue);
                //    if (txtHSNCodeIT.SelectedValue != Utility.GetDefaultData("S3G_ORGLPO_ASSETHSNID", dictParam).Rows[0]["ID"].ToString())
                //    {
                //        Utility.FunShowAlertMsg(this.Page, "Invalid Asset HSN Code");
                //        return;
                //    }
                //}
            }
            objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();

            dtPO = new DataTable();
            dtPO.Columns.Add("Sl_No");
            dtPO.Columns.Add("PO_dtl_ID");
            dtPO.Columns.Add("Asset_Category_ID");
            dtPO.Columns.Add("Asset_Type_ID");
            dtPO.Columns.Add("Asset_Sub_Type_ID");
            dtPO.Columns.Add("Delivery_State");
            dtPO.Columns.Add("Entity_State");
            /*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */
            dtPO.Columns.Add("Loc_Code");
            /*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */
            dtPO.Columns.Add("Asset_Description");
            dtPO.Columns.Add("Customer_PO_Ref_No");
            dtPO.Columns.Add("Quotation_Ref_No");
            dtPO.Columns.Add("OPC_billing_address");
            dtPO.Columns.Add("Delivery_address");
            dtPO.Columns.Add("Contact_person");
            dtPO.Columns.Add("Contact_no");
            dtPO.Columns.Add("Contact_EmailId");
            dtPO.Columns.Add("Quantity");
            dtPO.Columns.Add("Unit_Price");
            dtPO.Columns.Add("Bill_Amount_USD");
            dtPO.Columns.Add("Bill_Amount_INR");
            dtPO.Columns.Add("Inv_Amt_Material");
            dtPO.Columns.Add("Inv_Amt_Labour");
            dtPO.Columns.Add("VAT");
            dtPO.Columns.Add("CST");
            dtPO.Columns.Add("Excise_Duty_CVD");
            dtPO.Columns.Add("Service_Tax_Amt");
            dtPO.Columns.Add("Other_Bill_Component");
            dtPO.Columns.Add("Total_Bill_Amount");
            dtPO.Columns.Add("Retention");
            dtPO.Columns.Add("AS_NO");
            dtPO.Columns.Add("Model_Name");
            dtPO.Columns.Add("Manufacturer_Name");

            dtPO.Columns.Add("HSN_Code");
            dtPO.Columns.Add("HSN_ID");
            dtPO.Columns.Add("SGST");
            dtPO.Columns.Add("CGST");
            dtPO.Columns.Add("IGST");
            dtPO.Columns.Add("SAC_Code");
            dtPO.Columns.Add("SAC_Id");
            dtPO.Columns.Add("SAC_SGST");
            dtPO.Columns.Add("SAC_CGST");
            dtPO.Columns.Add("SAC_IGST");
            dtPO.Columns.Add("Billing_Branch_State");
            dtPO.Columns.Add("Billing_State_Id");
            dtPO.Columns.Add("Billing_Branch_Id");

            DataRow row = dtPO.NewRow();
            row["Sl_No"] = lblSlNo.Text;
            row["PO_dtl_ID"] = lblPO_dtl_ID.Text;
            row["Asset_Category_ID"] = ddlAssetCategoryhdr.SelectedValue;
            row["Asset_Type_ID"] = ddlAssetTypehdr.SelectedValue;
            row["Asset_Sub_Type_ID"] = ddlAssetSubTypehdr.SelectedValue;
            row["Delivery_State"] = ddlCustomer_Statehdr.SelectedValue;
            row["Entity_State"] = ddlVendor_Branchhdr.SelectedValue;
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */
            //            row["Loc_Code"] = txtLoc_CodeHdr.Text;
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */
            row["Asset_Description"] = txtAsset_Descirptionhdr.Text;
            row["Customer_PO_Ref_No"] = txtCust_PO_Ref_Nohdr.Text;
            row["Quotation_Ref_No"] = txtQuot_Ref_Nohdr.Text;
            //row["OPC_billing_address"] = txtOPC_Billing_Addresshdr.Text;
            row["Delivery_address"] = txtDelivery_Addresshdr.Text;
            row["Contact_person"] = txtContact_Personhdr.Text;
            row["Contact_no"] = txtContact_Nohdr.Text;
            row["Contact_EmailId"] = txtContact_EmailIdhdr.Text;
            row["Quantity"] = txtQuantityhdr.Text;
            row["Unit_Price"] = txtPer_Unit_Pricehdr.Text;
            row["Bill_Amount_USD"] = txtBill_Amount_FChdr.Text;
            row["Bill_Amount_INR"] = txtBill_Amount_INRhdr.Text;
            row["Inv_Amt_Material"] = txtBase_Inv_Amt_Mathdr.Text;
            row["Inv_Amt_Labour"] = txtBase_Inv_Amt_Labhdr.Text;
            row["VAT"] = txtVAThdr.Text;
            row["CST"] = txtCSThdr.Text;
            row["Excise_Duty_CVD"] = txtExcise_Dutyhdr.Text;
            row["Service_Tax_Amt"] = txtService_Taxhdr.Text;
            row["Other_Bill_Component"] = txtOthershdr.Text;
            row["Total_Bill_Amount"] = txtTotal_Bill_Amounthdr.Text;
            row["Retention"] = txtRetentionhdr.Text;
            row["AS_NO"] = txtASNohdr.Text;
            row["Model_Name"] = txtModel_Namehdr.Text;
            row["Manufacturer_Name"] = txtManufacturer_Namehdr.Text;

            row["HSN_Code"] = txtHSNCodeIT.SelectedText;
            row["HSN_ID"] = txtHSNCodeIT.SelectedValue;
            row["SGST"] = txtHSNSGSTIT.Text;
            row["CGST"] = txtHSNCGSTIT.Text;
            row["IGST"] = txtHSNIGSTIT.Text;
            row["SAC_Code"] = txtSACCodeIT.SelectedText;
            row["SAC_Id"] = txtSACCodeIT.SelectedValue;
            row["SAC_SGST"] = txtSACSGSTIT.Text;
            row["SAC_CGST"] = txtSACCGSTIT.Text;
            row["SAC_IGST"] = txtSACIGSTIT.Text;
            row["Billing_Branch_State"] = txtBillingStateIT.SelectedText + " - " + txtBillingBranchIT.SelectedText;
            row["Billing_State_Id"] = txtBillingStateIT.SelectedValue;
            row["Billing_Branch_Id"] = txtBillingBranchIT.SelectedValue;


            dtPO.Rows.Add(row);

            string strDelivery_No = String.Empty;
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable ObjS3G_DeliveryInsDataTable = new LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable();
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow;
            ObjDeliveryInsRow = ObjS3G_DeliveryInsDataTable.NewS3G_LOANAD_InsertDeliveryInstructionRow();
            ObjDeliveryInsRow.Company_ID = intCompanyId;


            if (lblCustID.Text != "")
            {
                ObjDeliveryInsRow.Customer_ID = Convert.ToInt32(lblCustID.Text);
            }
            else
            {
                ObjDeliveryInsRow.Customer_ID = Convert.ToInt32("0");
            }

            if (intPO_Hdr_ID > 0)
                ObjDeliveryInsRow.DeliveryInstruction_ID = intPO_Hdr_ID;
            else
                ObjDeliveryInsRow.DeliveryInstruction_ID = 0;

            ObjDeliveryInsRow.Vendor_ID = Convert.ToInt32(txtVendorName.SelectedValue);
            ObjDeliveryInsRow.DeliveryInstruction_Date = Utility.StringToDate(txtDate.Text);
            ObjDeliveryInsRow.Created_By = intUserId;
            ObjDeliveryInsRow.Created_On = DateTime.Now;
            ObjDeliveryInsRow.TXN_Id = 11;  
            ObjDeliveryInsRow.XML_DeliveryDeltails = dtPO.FunPubFormXml();
            ObjDeliveryInsRow.PO_Type = Convert.ToInt32(strPO_Type);
            ObjS3G_DeliveryInsDataTable.AddS3G_LOANAD_InsertDeliveryInstructionRow(ObjDeliveryInsRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_DeliveryInsDataTable, SerMode);

            intErrCode = objLoanAdmin_MasterClient.FunPubUpdatePO(SerMode, ObjDeliveryInsDataTable);

            if (intErrCode == 10)
            {
                Utility.FunShowAlertMsg(this.Page, "Asset should not duplicate within Group");
                ddlAssetSubTypehdr.Focus();
                return;
            }
            ///*added by vinodha m on sep 9 , 2015 - CR in vendor,Upload - POPLT*/            
            //            if (intErrCode == 9)
            //            {
            //                Utility.FunShowAlertMsg(this.Page, "Invalid Location Code for the specific vendor details");
            //                txtLoc_CodeHdr.Focus();
            //                return;
            //            }
            ///*added by vinodha m on sep 9 , 2015 - CR in vendor,Upload - POPLT*/

            ///*added by vinodha m on sep 9 , 2015 - CR in vendor,Upload - POPLT*/
            //            if (intErrCode == 8)
            //            {
            //                Utility.FunShowAlertMsg(this.Page, "Location Code already Exists for the specific vendor details");
            //                txtLoc_CodeHdr.Focus();
            //                return;
            //            }
            ///*added by vinodha m on sep 9 , 2015 - CR in vendor,Upload - POPLT*/            

            gvDlivery.EditIndex = -1;

            FunPriGetTempPuchaseGridDetails();

            Cache["PODetails"] = gvDlivery.DataSource;
            btnSave.Enabled = true;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvDlivery_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvDlivery.EditIndex = -1;
            FunPriGetTempPuchaseGridDetails();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvDlivery_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddNew")
        {
            DropDownList ddlAssetCategory = (DropDownList)gvDlivery.FooterRow.FindControl("ddlAssetCategory");
            DropDownList ddlAssetType = (DropDownList)gvDlivery.FooterRow.FindControl("ddlAssetType");
            DropDownList ddlAssetSubType = (DropDownList)gvDlivery.FooterRow.FindControl("ddlAssetSubType");
            DropDownList ddlCustomer_State = (DropDownList)gvDlivery.FooterRow.FindControl("ddlCustomer_State");
            DropDownList ddlVendor_Branch = (DropDownList)gvDlivery.FooterRow.FindControl("ddlVendor_Branch");
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */            
            //            TextBox txtLoc_Code = (TextBox)gvDlivery.FooterRow.FindControl("txtLoc_Code");
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */            
            TextBox txtAsset_Descirption = (TextBox)gvDlivery.FooterRow.FindControl("txtAsset_Descirption");
            TextBox txtCust_PO_Ref_No = (TextBox)gvDlivery.FooterRow.FindControl("txtCust_PO_Ref_No");
            TextBox txtQuot_Ref_No = (TextBox)gvDlivery.FooterRow.FindControl("txtQuot_Ref_No");
            //TextBox txtOPC_Billing_Address = (TextBox)gvDlivery.FooterRow.FindControl("txtOPC_Billing_Address");
            TextBox txtDelivery_Address = (TextBox)gvDlivery.FooterRow.FindControl("txtDelivery_Address");
            TextBox txtContact_Person = (TextBox)gvDlivery.FooterRow.FindControl("txtContact_Person");
            TextBox txtContact_No = (TextBox)gvDlivery.FooterRow.FindControl("txtContact_No");
            TextBox txtContact_EmailId = (TextBox)gvDlivery.FooterRow.FindControl("txtContact_EmailId");
            TextBox txtQuantity = (TextBox)gvDlivery.FooterRow.FindControl("txtQuantity");
            TextBox txtPer_Unit_Price = (TextBox)gvDlivery.FooterRow.FindControl("txtPer_Unit_Price");
            TextBox txtBill_Amount_FC = (TextBox)gvDlivery.FooterRow.FindControl("txtBill_Amount_FC");
            TextBox txtBill_Amount_INR = (TextBox)gvDlivery.FooterRow.FindControl("txtBill_Amount_INR");
            TextBox txtBase_Inv_Amt_Mat = (TextBox)gvDlivery.FooterRow.FindControl("txtBase_Inv_Amt_Mat");
            TextBox txtBase_Inv_Amt_Lab = (TextBox)gvDlivery.FooterRow.FindControl("txtBase_Inv_Amt_Lab");
            TextBox txtVAT = (TextBox)gvDlivery.FooterRow.FindControl("txtVAT");
            TextBox txtCST = (TextBox)gvDlivery.FooterRow.FindControl("txtCST");
            TextBox txtExcise_Duty = (TextBox)gvDlivery.FooterRow.FindControl("txtExcise_Duty");
            TextBox txtService_Tax = (TextBox)gvDlivery.FooterRow.FindControl("txtService_Tax");
            TextBox txtOthers = (TextBox)gvDlivery.FooterRow.FindControl("txtOthers");
            TextBox txtTotal_Bill_Amount = (TextBox)gvDlivery.FooterRow.FindControl("txtTotal_Bill_Amount");
            TextBox txtRetention = (TextBox)gvDlivery.FooterRow.FindControl("txtRetention");
            TextBox txtASNo = (TextBox)gvDlivery.FooterRow.FindControl("txtASNo");
            TextBox txtModel_Name = (TextBox)gvDlivery.FooterRow.FindControl("txtModel_Name");
            TextBox txtManufacturer_Name = (TextBox)gvDlivery.FooterRow.FindControl("txtManufacturer_Name");

            UserControls_S3GAutoSuggest txtHSNCodeFT = (UserControls_S3GAutoSuggest)gvDlivery.FooterRow.FindControl("txtHSNCodeFT");
            UserControls_S3GAutoSuggest txtSACCodeFT = (UserControls_S3GAutoSuggest)gvDlivery.FooterRow.FindControl("txtSACCodeFT");
            TextBox txtHSNSGSTFT = (TextBox)gvDlivery.FooterRow.FindControl("txtHSNSGSTFT");
            TextBox txtHSNCGSTFT = (TextBox)gvDlivery.FooterRow.FindControl("txtHSNCGSTFT");
            TextBox txtHSNIGSTFT = (TextBox)gvDlivery.FooterRow.FindControl("txtHSNIGSTFT");
            TextBox txtSACSGSTFT = (TextBox)gvDlivery.FooterRow.FindControl("txtSACSGSTFT");
            TextBox txtSACCGSTFT = (TextBox)gvDlivery.FooterRow.FindControl("txtSACCGSTFT");
            TextBox txtSACIGSTFT = (TextBox)gvDlivery.FooterRow.FindControl("txtSACIGSTFT");
            UserControls_S3GAutoSuggest txtBillingStateFT = (UserControls_S3GAutoSuggest)gvDlivery.FooterRow.FindControl("txtBillingStateFT");
            UserControls_S3GAutoSuggest txtBillingBranchFT = (UserControls_S3GAutoSuggest)gvDlivery.FooterRow.FindControl("txtBillingBranchFT");

            if (Utility.StringToDate(txtDate.Text) < Utility.StringToDate(GSTEffectiveDate.Text))
            {
                if (txtVAT.Text != "" && txtCST.Text != "")
                {
                    if (Convert.ToDecimal(txtVAT.Text) > 0 && Convert.ToDecimal(txtCST.Text) > 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Either VAT or CST Only Applicable");
                        txtCST.Focus();
                        return;
                    }
                }
            }

            ///*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/
            //foreach (GridViewRow gvrow in gvDlivery.Rows)
            //{
            //    Label lblLoc_Code = (Label)gvrow.FindControl("lblLoc_Code");
            //    if (txtLoc_Code.Text.Trim() == lblLoc_Code.Text.ToString())
            //    {
            //        Utility.FunShowAlertMsg(this, "Location Code already exists");
            //        txtLoc_Code.Focus();
            //        return;
            //    }
            //}
            ///*added by vinodha m on sep 8,2015 - CR in vendor,upload - POPLT*/   
            ///
            decimal strvat, strcst, strECD, strSTAmount, strothers, strmaterial, strlabour, strHSNSGST, strHSNCGST, strHSNIGST, strSACSGST, strSACCGST, strSACIGST;
            if (txtBase_Inv_Amt_Mat.Text.ToString() != "")
                strmaterial = Convert.ToDecimal(txtBase_Inv_Amt_Mat.Text.ToString());
            else
                strmaterial = Convert.ToDecimal("0");

            if (txtBase_Inv_Amt_Lab.Text.ToString() != "")
                strlabour = Convert.ToDecimal(txtBase_Inv_Amt_Lab.Text.ToString());
            else
                strlabour = Convert.ToDecimal("0");
            if (txtHSNSGSTFT.Text.ToString() != "")
                strHSNSGST = Convert.ToDecimal(txtHSNSGSTFT.Text.ToString());
            else
                strHSNSGST = Convert.ToDecimal("0");

            if (txtHSNCGSTFT.Text.ToString() != "")
                strHSNCGST = Convert.ToDecimal(txtHSNCGSTFT.Text.ToString());
            else
                strHSNCGST = Convert.ToDecimal("0");

            if (txtHSNIGSTFT.Text.ToString() != "")
                strHSNIGST = Convert.ToDecimal(txtHSNIGSTFT.Text.ToString());
            else
                strHSNIGST = Convert.ToDecimal("0");

            if (txtSACSGSTFT.Text.ToString() != "")
                strSACSGST = Convert.ToDecimal(txtSACSGSTFT.Text.ToString());
            else
                strSACSGST = Convert.ToDecimal("0");

            if (txtSACCGSTFT.Text.ToString() != "")
                strSACCGST = Convert.ToDecimal(txtSACCGSTFT.Text.ToString());
            else
                strSACCGST = Convert.ToDecimal("0");

            if (txtSACIGSTFT.Text.ToString() != "")
                strSACIGST = Convert.ToDecimal(txtSACIGSTFT.Text.ToString());
            else
                strSACIGST = Convert.ToDecimal("0");


            if (txtHSNCodeFT.SelectedText.Trim() == "")
            {
                txtHSNCodeFT.SelectedValue = "0";
            }

            if (txtSACCodeFT.SelectedText.Trim() == "")
            {
                txtSACCodeFT.SelectedValue = "0";
            }

            if (txtBillingStateFT.SelectedText.Trim() == "")
            {
                txtBillingStateFT.SelectedValue = "0";
            }

            if (txtBillingBranchFT.SelectedText.Trim() == "")
            {
                txtBillingBranchFT.SelectedValue = "0";
            }

            if (Utility.StringToDate(txtDate.Text) >= Utility.StringToDate(GSTEffectiveDate.Text))
            {

                if (txtHSNCodeFT.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select HSN Code");
                    return;
                }

                //if (strmaterial != Convert.ToDecimal("0") && txtHSNCodeFT.SelectedValue == "0")
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Select HSN Code,Since Material Amount Entered");
                //    return;
                //}

                if (strlabour != Convert.ToDecimal("0") && txtSACCodeFT.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select SAC Code,Since Labour Amount Entered");
                    return;
                }

                if (strmaterial != Convert.ToDecimal("0"))
                {

                    if ((strHSNCGST != Convert.ToDecimal("0") || strHSNSGST != Convert.ToDecimal("0")) && strHSNIGST != Convert.ToDecimal("0"))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Either HSN CGST OR HSN SGST OR HSN IGST Applicable");
                        return;
                    }
                }

                if (strlabour != Convert.ToDecimal("0"))
                {
                    if ((strSACCGST != Convert.ToDecimal("0") || strSACSGST != Convert.ToDecimal("0")) && strSACIGST != Convert.ToDecimal("0"))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Either SAC CGST OR SAC SGST OR SAC IGST Applicable");
                        return;
                    }
                }

                if (Utility.StringToDate(txtDate.Text) >= Utility.StringToDate(GSTEffectiveDate.Text))
                {
                    if (txtBillingStateFT.SelectedValue == "0")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Select Billing State");
                        return;
                    }
                }
                //if (strmaterial != Convert.ToDecimal("0"))
                //{
                //    Dictionary<string, string> dictParam = new Dictionary<string, string>();
                //    dictParam.Add("@Company_ID", intCompanyId.ToString());
                //    dictParam.Add("@Type_ID", ddlAssetType.SelectedValue);
                //    if (txtHSNCodeFT.SelectedValue != Utility.GetDefaultData("S3G_ORGLPO_ASSETHSNID", dictParam).Rows[0]["ID"].ToString())
                //    {
                //        Utility.FunShowAlertMsg(this.Page, "Invalid Asset HSN Code");
                //        return;
                //    }
                //}
            }

            objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();

            dtPO = new DataTable();
            dtPO.Columns.Add("Sl_No");
            dtPO.Columns.Add("PO_dtl_ID");
            dtPO.Columns.Add("Asset_Category_ID");
            dtPO.Columns.Add("Asset_Type_ID");
            dtPO.Columns.Add("Asset_Sub_Type_ID");
            dtPO.Columns.Add("Delivery_State");
            dtPO.Columns.Add("Entity_State");
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */            
            //            dtPO.Columns.Add("Loc_Code");
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */            
            dtPO.Columns.Add("Asset_Description");
            dtPO.Columns.Add("Customer_PO_Ref_No");
            dtPO.Columns.Add("Quotation_Ref_No");
            dtPO.Columns.Add("OPC_billing_address");
            dtPO.Columns.Add("Delivery_address");
            dtPO.Columns.Add("Contact_person");
            dtPO.Columns.Add("Contact_no");
            dtPO.Columns.Add("Contact_EmailId");
            dtPO.Columns.Add("Quantity");
            dtPO.Columns.Add("Unit_Price");
            dtPO.Columns.Add("Bill_Amount_USD");
            dtPO.Columns.Add("Bill_Amount_INR");
            dtPO.Columns.Add("Inv_Amt_Material");
            dtPO.Columns.Add("Inv_Amt_Labour");
            dtPO.Columns.Add("VAT");
            dtPO.Columns.Add("CST");
            dtPO.Columns.Add("Excise_Duty_CVD");
            dtPO.Columns.Add("Service_Tax_Amt");
            dtPO.Columns.Add("Other_Bill_Component");
            dtPO.Columns.Add("Total_Bill_Amount");
            dtPO.Columns.Add("Retention");
            dtPO.Columns.Add("Payment_Terms");
            dtPO.Columns.Add("Delivery_Terms");
            dtPO.Columns.Add("Warranty_Terms");
            dtPO.Columns.Add("Notes1");
            dtPO.Columns.Add("Notes2");
            dtPO.Columns.Add("Others");
            dtPO.Columns.Add("AS_NO");
            dtPO.Columns.Add("Model_Name");
            dtPO.Columns.Add("Manufacturer_Name");

            dtPO.Columns.Add("HSN_Code");
            dtPO.Columns.Add("HSN_ID");
            dtPO.Columns.Add("SGST");
            dtPO.Columns.Add("CGST");
            dtPO.Columns.Add("IGST");
            dtPO.Columns.Add("SAC_Code");
            dtPO.Columns.Add("SAC_Id");
            dtPO.Columns.Add("SAC_SGST");
            dtPO.Columns.Add("SAC_CGST");
            dtPO.Columns.Add("SAC_IGST");
            dtPO.Columns.Add("Billing_Branch_State");
            dtPO.Columns.Add("Billing_State_Id");
            dtPO.Columns.Add("Billing_Branch_Id");

            DataRow row = dtPO.NewRow();
            row["Asset_Category_ID"] = ddlAssetCategory.SelectedValue;
            row["Asset_Type_ID"] = ddlAssetType.SelectedValue;
            row["Asset_Sub_Type_ID"] = ddlAssetSubType.SelectedValue;
            row["Delivery_State"] = ddlCustomer_State.SelectedValue;
            row["Entity_State"] = ddlVendor_Branch.SelectedValue;
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */            
            //            row["Loc_Code"] = txtLoc_Code.Text;
            ///*added by vinodha m on sep 8 ,2015 - CR of vendor,PO - POPLT */            
            row["Asset_Description"] = txtAsset_Descirption.Text;
            row["Customer_PO_Ref_No"] = txtCust_PO_Ref_No.Text;
            row["Quotation_Ref_No"] = txtQuot_Ref_No.Text;
            //row["OPC_billing_address"] = txtOPC_Billing_Address.Text;
            row["Delivery_address"] = txtDelivery_Address.Text;
            row["Contact_person"] = txtContact_Person.Text;
            row["Contact_no"] = txtContact_No.Text;
            row["Contact_EmailId"] = txtContact_EmailId.Text;
            row["Quantity"] = txtQuantity.Text;
            row["Unit_Price"] = txtPer_Unit_Price.Text;
            row["Bill_Amount_USD"] = txtBill_Amount_FC.Text;
            row["Bill_Amount_INR"] = txtBill_Amount_INR.Text;
            row["Inv_Amt_Material"] = txtBase_Inv_Amt_Mat.Text;
            row["Inv_Amt_Labour"] = txtBase_Inv_Amt_Lab.Text;
            row["VAT"] = txtVAT.Text;
            row["CST"] = txtCST.Text;
            row["Excise_Duty_CVD"] = txtExcise_Duty.Text;
            row["Service_Tax_Amt"] = txtService_Tax.Text;
            row["Other_Bill_Component"] = txtOthers.Text;
            row["Total_Bill_Amount"] = txtTotal_Bill_Amount.Text;
            row["Retention"] = txtRetention.Text;
            row["Payment_Terms"] = txtPaymentTerms.Text;
            row["Delivery_Terms"] = txtDeliveryTerms.Text;
            row["Warranty_Terms"] = txtWarrantyTerms.Text;
            row["Notes1"] = txtNotes1.Text;
            row["Notes2"] = txtNotes2.Text;
            row["Others"] = txtOthers1.Text;
            row["AS_NO"] = txtASNo.Text;
            row["Model_Name"] = txtModel_Name.Text;
            row["Manufacturer_Name"] = txtManufacturer_Name.Text;

            row["HSN_Code"] = txtHSNCodeFT.SelectedText;
            row["HSN_ID"] = txtHSNCodeFT.SelectedValue;
            row["SGST"] = txtHSNSGSTFT.Text;
            row["CGST"] = txtHSNCGSTFT.Text;
            row["IGST"] = txtHSNIGSTFT.Text;
            row["SAC_Code"] = txtSACCodeFT.SelectedText;
            row["SAC_Id"] = txtSACCodeFT.SelectedValue;
            row["SAC_SGST"] = txtSACSGSTFT.Text;
            row["SAC_CGST"] = txtSACCGSTFT.Text;
            row["SAC_IGST"] = txtSACIGSTFT.Text;
            row["Billing_Branch_State"] = txtBillingStateFT.SelectedText + " - " + txtBillingBranchFT.SelectedText;
            row["Billing_State_Id"] = txtBillingStateFT.SelectedValue;
            row["Billing_Branch_Id"] = txtBillingBranchFT.SelectedValue;

            dtPO.Rows.Add(row);

            string strDelivery_No = String.Empty;
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable ObjS3G_DeliveryInsDataTable = new LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable();
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow;
            ObjDeliveryInsRow = ObjS3G_DeliveryInsDataTable.NewS3G_LOANAD_InsertDeliveryInstructionRow();
            ObjDeliveryInsRow.Company_ID = intCompanyId;
            ObjDeliveryInsRow.DeliveryInstruction_ID = intPO_Hdr_ID;

            if (lblCustID.Text != "")
            {
                ObjDeliveryInsRow.Customer_ID = Convert.ToInt32(lblCustID.Text);
            }
            else
            {
                ObjDeliveryInsRow.Customer_ID = Convert.ToInt32("0");
            }

            ObjDeliveryInsRow.Vendor_ID = Convert.ToInt32(txtVendorName.SelectedValue);
            ObjDeliveryInsRow.DeliveryInstruction_Date = Utility.StringToDate(txtDate.Text);

            ObjDeliveryInsRow.DeliveryInstruction_Status_Code = 1;
            ObjDeliveryInsRow.DeliveryInstruction_Statustype_Code = 3;
            ObjDeliveryInsRow.IS_LPO = true;
            ObjDeliveryInsRow.Created_By = intUserId;
            ObjDeliveryInsRow.Created_On = DateTime.Now;
            ObjDeliveryInsRow.TXN_Id = 11;
            ObjDeliveryInsRow.XML_DeliveryDeltails = dtPO.FunPubFormXml();
            ObjDeliveryInsRow.PO_Type = Convert.ToInt32(strPO_Type);
            ObjS3G_DeliveryInsDataTable.AddS3G_LOANAD_InsertDeliveryInstructionRow(ObjDeliveryInsRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_DeliveryInsDataTable, SerMode);

            intErrCode = objLoanAdmin_MasterClient.FunPubUpdatePO(SerMode, ObjDeliveryInsDataTable);

            if (intErrCode == 10)
            {
                Utility.FunShowAlertMsg(this.Page, "Asset should not duplicate within Group");
                return;
            }

            ///*added by vinodha m on sep 9 , 2015 - CR in vendor,Upload - POPLT*/
            //if (intErrCode == 9)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Invalid Location Code for the specific vendor details");
            //    txtLoc_Code.Focus();
            //    return;
            //}
            ///*added by vinodha m on sep 9 , 2015 - CR in vendor,Upload - POPLT*/            

            gvDlivery.EditIndex = -1;

            FunPriGetTempPuchaseGridDetails();

            Cache["PODetails"] = gvDlivery.DataSource;
            btnSave.Enabled = true;
        }
    }

    #endregion

    #region To Get Puchase Order in Query Mode

    private void FunPriGetPuchaseOrderDetails()
    {
        //Changed By Shibu Unwanted DDL Load Events An Address Load Events are Removed
        try
        {
            DataSet dset = new DataSet();

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@PO_Hdr_ID", intPO_Hdr_ID.ToString());
            if (Session["VendorGroup"] != null)
                dictParam.Add("@Vendor_Group", Session["VendorGroup"].ToString());
            dictParam.Add("@User_ID", intUserId.ToString());

            if (strPO_Type == "1")
            {
                dset = Utility.GetDataset("S3G_ORG_Get_PurchaseOrder_Details_MPO", dictParam);
            }
            else
            {
                dset = Utility.GetDataset("S3G_ORG_Get_PurchaseOrder_Details", dictParam);
            }
            DataTable dtDeliveryDetails = dset.Tables[0].Copy();
            //DataTable dtAssetDetails = dset.Tables[1].Copy();

            txtLSQNo.Text = dtDeliveryDetails.Rows[0]["LSQ_Number"].ToString();
            txtLPONo.Text = dtDeliveryDetails.Rows[0]["PO_Number"].ToString();
            txtDate.Text = dtDeliveryDetails.Rows[0]["PO_Date"].ToString();
            txtPOStatus.Text = dtDeliveryDetails.Rows[0]["Status"].ToString();
            LPODate.Text = txtDate.Text;

            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");

            hdnCustomerId.Value = dtDeliveryDetails.Rows[0]["Customer_ID"].ToString();
            txtName.Text = txtCustomerCode.Text = dtDeliveryDetails.Rows[0]["Customer_Code"].ToString();

            S3GCustomerAddress1.SetCustomerDetails(dtDeliveryDetails.Rows[0], true);

            txtVendorName.ReadOnly = true;
            txtVendorName.SelectedValue = dtDeliveryDetails.Rows[0]["Entity_ID"].ToString();
            txtVendorName.SelectedText = dtDeliveryDetails.Rows[0]["Entity_Name"].ToString();

            txtPaymentTerms.Text = dtDeliveryDetails.Rows[0]["Payment_Terms"].ToString();
            txtDeliveryTerms.Text = dtDeliveryDetails.Rows[0]["Delivery_terms"].ToString();
            txtWarrantyTerms.Text = dtDeliveryDetails.Rows[0]["Warranty_terms"].ToString();
            txtNotes1.Text = dtDeliveryDetails.Rows[0]["Notes_1"].ToString();
            txtNotes2.Text = dtDeliveryDetails.Rows[0]["Notes_2"].ToString();
            txtOthers1.Text = dtDeliveryDetails.Rows[0]["Others"].ToString();
            txtCusCustomer_Name.Text = dtDeliveryDetails.Rows[0]["EndUseCustomer_Name"].ToString();
            txtCust_PO_Ref_No.Text = dtDeliveryDetails.Rows[0]["Customer_PO_Ref_No"].ToString();
            FunPriGetVendorBasedLocCode(Convert.ToInt32(txtVendorName.SelectedValue));
            ddlLoc_Code.SelectedValue = dtDeliveryDetails.Rows[0]["Loc_Code"].ToString();

            if (lblStatus.Text == "2")
            {
                btnDLGeneration.Enabled = false;
                btnDICancel.Enabled = false;
                btnEmail.Enabled = false;
                btnPrint.Enabled = false;
            }
            if (lblIsPrint.Text == "1" && lblStatus.Text != "2")
            {
                btnEmail.Enabled = true;
                btnPrint.Enabled = true;
            }
            else
            {
                btnEmail.Enabled = false;
                //btnPrint.Enabled = false;
            }
            if (dtDeliveryDetails.Rows[0]["Status_ID"].ToString() == "False")
            {
                pnlCancellationQry.Visible = true;
                txtCanReasonQry.Text = dtDeliveryDetails.Rows[0]["Cancellation_Reason"].ToString(); ;
            }
        }
        catch (FaultException<LoanAdminMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            cvDelivery.ErrorMessage = objFaultExp.Detail.ProReasonRW;
            cvDelivery.IsValid = false;
        }
        catch (Exception ex)
        {
            cvDelivery.ErrorMessage = ex.Message;
            cvDelivery.IsValid = false;
        }

    }
    private void FunPriBindEmptyGrid()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@PO_Hdr_ID", "0");
        dtPO = new DataTable();
        dtPO = Utility.GetDefaultData("S3G_Org_Get_PurOdr_Temp", dictParam);
        DataRow row = dtPO.NewRow();
        dtPO.Rows.Add(row);
        gvDlivery.DataSource = dtPO;
        gvDlivery.DataBind();
        gvDlivery.Rows[0].Visible = false;

        ucCustomPaging.Visible = false;
    }

    private void FunPriDeleteGrid()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@user_id", intUserId.ToString());
        DataTable dttemp = new DataTable();
        dttemp = Utility.GetDefaultData("S3G_Org_Delete_PO_Temp", dictParam);
        ucCustomPaging.Visible = false;
    }

    private string SetVendorAddressdetails(DataRow drCust)
    {
        string strAddress = "";
        if (drCust["Address"].ToString() != "") strAddress += drCust["Address"].ToString() + System.Environment.NewLine;
        if (drCust["Address2"].ToString() != "") strAddress += drCust["Address2"].ToString() + System.Environment.NewLine;
        if (drCust["EMCity"].ToString() != "") strAddress += drCust["EMCity"].ToString() + System.Environment.NewLine;
        if (drCust["State"].ToString() != "") strAddress += drCust["State"].ToString() + System.Environment.NewLine;
        if (drCust["Country"].ToString() != "") strAddress += drCust["Country"].ToString() + System.Environment.NewLine;
        if (drCust["Pincode"].ToString() != "") strAddress += drCust["Pincode"].ToString();
        return strAddress;
    }

    private void FunPriGetTempPuchaseGridDetails()
    {
        try
        {

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@PO_Hdr_ID", intPO_Hdr_ID.ToString());
            if (Session["VendorGroup"] != null)
                dictParam.Add("@Vendor_Group", Session["VendorGroup"].ToString());

            if (ViewState["SlNo"] != null)
                dictParam.Add("@SlNo", ViewState["SlNo"].ToString());

            int intTotalRecords = 0;
            bool bIsNewRow = false;

            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProUser_ID = intUserId;

            gvDlivery.BindGridView("S3G_Org_Get_PurOdr_Temp", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);

            if (gvDlivery.Rows.Count == 1)
                ((LinkButton)gvDlivery.Rows[0].FindControl("lnkDelete")).Enabled = false;

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    private void FunPriGetPuchaseGridDetails()
    {
        try
        {

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@PO_Hdr_ID", intPO_Hdr_ID.ToString());
            if (Session["VendorGroup"] != null)
                dictParam.Add("@Vendor_Group", Session["VendorGroup"].ToString());

            int intTotalRecords = 0;
            bool bIsNewRow = false;

            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProUser_ID = intUserId;

            gvDlivery.BindGridView("S3G_Org_Get_PurOdr_Paging", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);
            Cache["PODetails"] = gvDlivery.DataSource;

            if (gvDlivery.Rows.Count == 1 && gvDlivery.EditIndex == -1)
                ((LinkButton)gvDlivery.Rows[0].FindControl("lnkDelete")).Enabled = false;

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    protected void txtVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVendorName.SelectedValue != String.Empty && txtVendorName.SelectedValue != "0")
            {
                FunPriGetVendorBasedLocCode(Convert.ToInt32(txtVendorName.SelectedValue));
            }
            FunPriDeleteGrid();
            FunPriBindEmptyGrid();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    private void FunPriGetVendorBasedLocCode(int VendorCode)
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@VendorCode", VendorCode.ToString());
        ddlLoc_Code.BindDataTable("S3G_ORG_GETVENDORBASEDLOCODE", dictParam, new string[] { "ID", "Loc_Code" });
        if (ddlLoc_Code.Items.Count > 0)
            ddlLoc_Code.Enabled = true;
    }

    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    #region Role Access Setup/Page Load
    private void FunPriDisableControls(int intModeID)
    {
        Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                //FunPriGetDeliveryGridDetails();
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                txtLPONo.Enabled = false;
                txtLSQNo.Enabled = false;
                ddlLoc_Code.Enabled = false;
                btnDLGeneration.Enabled = false;
                btnPrint.Enabled = false;
                btnEmail.Enabled = false;
                btnDICancel.Enabled = false;
                txtPOStatus.Text = "Approved";
                ucCustomPaging.Visible = false;
                Session["VendorGroup"] = "1";
                break;

            case 1:
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    Response.Redirect(strRedirectPageView);
                }

                btnClear.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                if (bDelete)
                    btnDICancel.Enabled = true;
                else
                    btnDICancel.Enabled = false;
                txtLPONo.ReadOnly = true;
                txtLSQNo.Enabled = false;
                txtDate.ReadOnly = true;
                btnPrint.Enabled = false;
                FunPriGetPuchaseOrderDetails();
                FunPriGetPuchaseGridDetails();
                btnGetLOV.Enabled = false;
                //txtPaymentTerms.ReadOnly = true;
                //txtDeliveryTerms.ReadOnly = true;
                //txtWarrantyTerms.ReadOnly = true;
                //txtNotes1.ReadOnly = true;
                //txtNotes2.ReadOnly = true;
                //txtOthers1.ReadOnly = true;
                if (bClearList)
                {
                    //ddlVendorCode.ClearDropDownList();
                }

                if (txtPOStatus.Text == "Cancelled")
                {
                    gvDlivery.Columns[gvDlivery.Rows.Count - 1].Visible = false;
                    gvDlivery.FooterRow.Visible = false;
                    btnSave.Enabled = btnDICancel.Enabled = false;
                }
                if(strPO_Type=="1")
                {
                    btnDICancel.Enabled = false;
                }
                break;


            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                btnClear.Enabled = false;
                btnSave.Enabled = false;

                btnDICancel.Visible = false;
                btnCancel.Enabled = true;
                txtLPONo.ReadOnly = true;
                txtLSQNo.Enabled = false;
                ddlLoc_Code.Enabled = false;
                txtDate.ReadOnly = true;
                FunPriGetPuchaseOrderDetails();
                FunPriGetPuchaseGridDetails();
                ddlLoc_Code.ClearDropDownList();
                btnGetLOV.Enabled = false;
                txtPaymentTerms.ReadOnly = true;
                txtDeliveryTerms.ReadOnly = true;
                txtWarrantyTerms.ReadOnly = true;
                txtNotes1.ReadOnly = true;
                txtNotes2.ReadOnly = true;
                txtOthers1.ReadOnly = true;
                btnPrint.Enabled = true;
                gvDlivery.Columns[gvDlivery.Columns.Count - 1].Visible = false;
                gvDlivery.FooterRow.Visible = false;

                if (bClearList)
                {
                    //ddlVendorCode.ClearDropDownList();
                }
                break;
        }

    }
    #endregion

    #region Button Events
    #region Save

    private string FunPubDeleteTable(string strtblName, string strHTML)
    {
        try
        {
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);

            strHTML = strHTML.Replace(strTable, "");
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void SaveDocument(string strHTML, string ReferenceNumber, string FilePath, string FileName)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@PO_Number", txtLPONo.Text);
            DataSet dsHeader = new DataSet();
            if (strPO_Type == "1")
            {
                dsHeader = Utility.GetDataset("S3G_ORG_PurchaseOrder_Print_MPO", dictParam);
            }
            else
            {
                dsHeader = Utility.GetDataset("S3G_ORG_PurchaseOrder_Print", dictParam);
            }
            //dsHeader = Utility.GetDataset("S3G_ORG_PurchaseOrder_Print", dictParam);
            //dsHeader.Tables[0].Columns.Add("Grand_Total");

            //if (dsHeader.Tables[0].Rows.Count != 0)
            // dsHeader.Tables[0].Rows[0]["Grand_Total"] = Convert.ToDecimal((dsHeader.Tables[1].Compute("sum(Total1)", ""))).ToString(Funsetsuffix());

            if (dsHeader.Tables[1].Rows.Count == 0)
                if (strHTML.Contains("~PO_Header_Table~"))
                    strHTML = FunPubDeleteTable("~PO_Header_Table~", strHTML);

            if (dsHeader.Tables[2].Rows.Count == 0)
                if (strHTML.Contains("~PO_Annex1_Table~"))
                    strHTML = FunPubDeleteTable("~PO_Annex1_Table~", strHTML);

            if (dsHeader.Tables[3].Rows.Count == 0)
                if (strHTML.Contains("~PO_Annex2_Table~"))
                    strHTML = FunPubDeleteTable("~PO_Annex2_Table~", strHTML);

            if (dsHeader.Tables[1].Rows.Count > 0)
                if (strHTML.Contains("~PO_Header_Table~"))
                    strHTML = PDFPageSetup.FunPubBindTable("~PO_Header_Table~", strHTML, dsHeader.Tables[1]);

            if (dsHeader.Tables[2].Rows.Count > 0)
                if (strHTML.Contains("~PO_Annex1_Table~"))
                    strHTML = PDFPageSetup.FunPubBindTable("~PO_Annex1_Table~", strHTML, dsHeader.Tables[2]);

            if (dsHeader.Tables[3].Rows.Count > 0)
                if (strHTML.Contains("~PO_Annex2_Table~"))
                    strHTML = PDFPageSetup.FunPubBindTable("~PO_Annex2_Table~", strHTML, dsHeader.Tables[3]);

            if (dsHeader.Tables[0].Rows.Count > 0)
                strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);

            List<string> listImageName = new List<string>();
            listImageName.Add("~CompanyLogo~");
            listImageName.Add("~InvoiceSignStamp~");
            listImageName.Add("~POSignStamp~");
            List<string> listImagePath = new List<string>();
            if (Utility.StringToDate(LPODate.Text) >= Utility.StringToDate("06/06/2023"))
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
            }
            else
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
            }

            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

            strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
            PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);
        }
        catch (Exception ex)
        {
            cvDelivery.ErrorMessage = "Unable to Save : " + ex.ToString();
            cvDelivery.IsValid = false;
            return;
        }
    }


    private string FunPubGetTemplateContent(int CompanyID, int LobID, int LocationID, int TemplateTypeCode, string Reference_ID)
    {
        try
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyID.ToString());
            Procparam.Add("@Lob_Id", LobID.ToString());
            Procparam.Add("@Location_ID", LocationID.ToString());
            Procparam.Add("@Template_Type_Code", TemplateTypeCode.ToString());
            Procparam.Add("@Reference_ID", Reference_ID);
            dt = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
                return "<META content='text/html; charset=utf-8' http-equiv='Content-Type'> <TABLE><TR><TD>" + dt.Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE> </html>";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private string FunPubGetTemplateContent(int CompanyID, int LobID, string LocationCode, int TemplateTypeCode, string Reference_ID)
    {
        try
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyID.ToString());
            Procparam.Add("@Lob_Id", LobID.ToString());
            Procparam.Add("@location_code", LocationCode);
            Procparam.Add("@Template_Type_Code", TemplateTypeCode.ToString());
            Procparam.Add("@Reference_ID", Reference_ID);
            dt = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
                return "<META content='text/html; charset=utf-8' http-equiv='Content-Type'> <TABLE><TR><TD>" + dt.Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE> </html>";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strHTML = string.Empty;

        if (Utility.StringToDate(txtDate.Text) >= Utility.StringToDate(GSTEffectiveDate.Text))
            strHTML = FunPubGetTemplateContent(intCompanyId, 3, ddlLoc_Code.SelectedValue, 32, intPO_Hdr_ID.ToString());
        else
            strHTML = FunPubGetTemplateContent(intCompanyId, 3, ddlLoc_Code.SelectedValue, 10, intPO_Hdr_ID.ToString());
        if (strHTML == "")
        {
            Utility.FunShowAlertMsg(this, "Template Master not defined");
            return;
        }
        strHTML = null;
        string strPO_No = string.Empty;
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            string strDelivery_No = string.Empty;

            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable ObjS3G_DeliveryInsDataTable = new LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable();
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow;
            ObjDeliveryInsRow = ObjS3G_DeliveryInsDataTable.NewS3G_LOANAD_InsertDeliveryInstructionRow();
            ObjDeliveryInsRow.Company_ID = intCompanyId;

            if (intPO_Hdr_ID > 0)
                ObjDeliveryInsRow.DeliveryInstruction_ID = intPO_Hdr_ID;
            else
                ObjDeliveryInsRow.DeliveryInstruction_ID = 0;

            if (hdnCustomerId.Value != "")
            {
                ObjDeliveryInsRow.Customer_ID = Convert.ToInt32(hdnCustomerId.Value);
            }
            else
            {
                ObjDeliveryInsRow.Customer_ID = 0;
            }

            ObjDeliveryInsRow.Vendor_ID = Convert.ToInt32(txtVendorName.SelectedValue);
            ObjDeliveryInsRow.DeliveryInstruction_Date = Utility.StringToDate(txtDate.Text);

            ObjDeliveryInsRow.DeliveryInstruction_Status_Code = 1;
            ObjDeliveryInsRow.DeliveryInstruction_Statustype_Code = 3;
            ObjDeliveryInsRow.IS_LPO = true;
            ObjDeliveryInsRow.Created_By = intUserId;
            ObjDeliveryInsRow.Created_On = DateTime.Now;
            ObjDeliveryInsRow.TXN_Id = 11;
            ObjDeliveryInsRow.XML_DeliveryDeltails = dtPO.FunPubFormXml();
            ObjDeliveryInsRow.Payment_Terms = txtPaymentTerms.Text;
            ObjDeliveryInsRow.Delivery_Terms = txtDeliveryTerms.Text;
            ObjDeliveryInsRow.Warranty_Terms = txtWarrantyTerms.Text;
            ObjDeliveryInsRow.Notes1 = txtNotes1.Text;
            ObjDeliveryInsRow.Notes2 = txtNotes2.Text;
            ObjDeliveryInsRow.Others = txtOthers1.Text;
            ObjDeliveryInsRow.EndUseCustomer_Name = txtCusCustomer_Name.Text;
            ObjDeliveryInsRow.Customer_PO_Ref_No = txtCust_PO_Ref_No.Text;
            ObjDeliveryInsRow.Location_Code = ddlLoc_Code.SelectedValue;
            ObjDeliveryInsRow.PO_Type = Convert.ToInt32(strPO_Type);
            ObjS3G_DeliveryInsDataTable.AddS3G_LOANAD_InsertDeliveryInstructionRow(ObjDeliveryInsRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_DeliveryInsDataTable, SerMode);

            intErrCode = objLoanAdmin_MasterClient.FunPubSavePO(out strPO_No, SerMode, ObjDeliveryInsDataTable);

            if (intErrCode == 0)
            {
                if (intPO_Hdr_ID > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Purchase Order Updated Successfully");
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                }
                else
                {
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strDelivery_No, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }
                        ShowWFAlertMessage(strDelivery_No, ProgramCode, strAlert);
                        return;
                    }
                    else
                    {
                        strAlert = "Purchase Order " + strPO_No + " added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    }

                }
                Session["VendorGroup"] = 1;
            }
            else if (intErrCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined for Purchase Order");
                strRedirectPageView = "";
            }
            else if (intErrCode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined for Purchase Order");
                strRedirectPageView = "";
            }
            else if (intErrCode == 50)
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to save the record");
                return;
            }

            if (Request.QueryString["Popup"] != null)
            {
                strAlert += "window.close();";
                strRedirectPageView = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
            //objLoanAdmin_MasterClient.Close();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            cvDelivery.ErrorMessage = objFaultExp.Detail.ProReasonRW; ;
            cvDelivery.IsValid = false;
            return;
            //lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            cvDelivery.ErrorMessage = "Unable to Save";
            cvDelivery.IsValid = false;
            return;
            //lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            if (objLoanAdmin_MasterClient != null)
            {
                objLoanAdmin_MasterClient.Close();
            }
        }
    }
    #endregion

    #region Other Button
    #region Cancel Button
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FunPriDeleteGrid();
        // wf cancel
        if (Request.QueryString["Popup"] != null)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
        else
            Response.Redirect("../Origination/S3GORGTransLander.aspx?Code=PURORD");
        Session["VendorGroup"] = null;

    }
    #endregion

    #region Cancel Delivery Instruction
    protected void btnDICancel_Click(object sender, EventArgs e)
    {
        int ErrorCode = 0;
        int intVendorGroup = 0;
        string Flag;
        Flag = "C";
        string strReason_For_Cancellation = txtCancReason.Text;
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesClient();
        try
        {
            if (Session["VendorGroup"] != null)
                intVendorGroup = Convert.ToInt32(Session["VendorGroup"].ToString());
            int intResult = 0;

            //if (Convert.ToInt32(strPO_Type) == 1)
            //{
            //    intResult = objLoanAdmin_MasterClient.FunPubCancel_MPO(out ErrorCode, intPO_Hdr_ID, intVendorGroup, strReason_For_Cancellation);
            //}
            //else
            //{
            //    intResult = objLoanAdmin_MasterClient.FunPubCancelPO(out ErrorCode, intPO_Hdr_ID, intVendorGroup, strReason_For_Cancellation);
            //}

            intResult = objLoanAdmin_MasterClient.FunPubCancelPO(out ErrorCode, intPO_Hdr_ID, intVendorGroup, strReason_For_Cancellation);

            if (intResult == 0)
            {
                FunEmailGeneration(1);
                string strRedi = "";
                if (strExceptionEmail != "")
                {
                    strRedi = "../Origination/S3GORGTransLander.aspx?Code=PURORD";
                    Utility.FunShowAlertMsg(this.Page, "Purchase Order Cancelled successfully and unable to sent Email to " + strExceptionEmail, strRedi);
                    return;
                }
                else
                {
                    strRedi = "../Origination/S3GORGTransLander.aspx?Code=PURORD";
                    Utility.FunShowAlertMsg(this.Page, "Purchase Order Cancelled and Email sent successfully", strRedi);
                    return;
                }
            }
            if (intResult == 10)
            {
                Utility.FunShowAlertMsg(this.Page, "Proforma Invoice Processed, Unable to Cancel PO");
                btnDICancel.Enabled = false;
            }
            if (intResult == 11)
            {
                Utility.FunShowAlertMsg(this.Page, "Vendor Invoice Processed, Unable to Cancel PO");
                btnDICancel.Enabled = false;
            }
            if (intResult == 5)
            {
                Utility.FunShowAlertMsg(this.Page, "Purchase Order Cancelation Failed");
                btnDICancel.Enabled = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvDelivery.ErrorMessage = "Unable to cancel Delivery Instruction / LPO ";
            cvDelivery.IsValid = false;
        }
        finally
        {
            objLoanAdmin_MasterClient.Close();
        }
    }
    #endregion

    #region DL Generation Button Events
    protected void btnDLGeneration_Click(object sender, EventArgs e)
    {
        try
        {
            //     FunCrstalReportGeneration("D");
        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "LPO Generation PDF already open");
            //throw ex;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvDelivery.ErrorMessage = "Unable to Generate Delivery Instruction / LPO ";
            cvDelivery.IsValid = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //FunCrstalReportGeneration("P");
        try
        {
            FunPriGeneratePOFiles(0);
        }
        catch (Exception objException)
        {
        }

    }
    #endregion
    private void FunPriGeneratePOFiles(int IsCancelled)
    {

        String strHTML = String.Empty;
        if (Utility.StringToDate(txtDate.Text) >= Utility.StringToDate(GSTEffectiveDate.Text))
            strHTML = FunPubGetTemplateContent(intCompanyId, 3, ddlLoc_Code.SelectedValue, 32, intPO_Hdr_ID.ToString());
        else
            strHTML = FunPubGetTemplateContent(intCompanyId, 3, ddlLoc_Code.SelectedValue, 10, intPO_Hdr_ID.ToString());

        if (strHTML == "")
        {
            Utility.FunShowAlertMsg(this, "Template Master not defined");
            return;
        }

        string FileName = PDFPageSetup.FunPubGetFileName(txtLPONo.Text + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));

        if (ddlPrintType.SelectedValue == "P")
        {
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string DownFile = FilePath + FileName + ".pdf";
            SaveDocument(strHTML, txtLPONo.Text, FilePath, FileName);
            Procparam_PO_PDF.Add(txtLPONo.Text, DownFile);
            if (!File.Exists(DownFile))
            {
                Utility.FunShowAlertMsg(this, "File not exists");
                return;
            }
            if (IsCancelled == 0)
            {
                Response.AppendHeader("content-disposition", "attachment; filename=PurchaseOrder.pdf");
                Response.ContentType = "application/pdf";
                Response.WriteFile(DownFile);
            }
        }
        else if (ddlPrintType.SelectedValue == "W")
        {
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            SaveDocument(strHTML, txtLPONo.Text, FilePath, FileName);
            string DownFile = FilePath + FileName + ".doc";
            if (!File.Exists(DownFile))
            {
                Utility.FunShowAlertMsg(this, "File not exists");
                return;
            }
            Response.AppendHeader("content-disposition", "attachment; filename=PurchaseOrder.doc");
            Response.ContentType = "application/vnd.ms-word";
            Response.WriteFile(DownFile);
        }
    }

    #region TO Print Delivery Instruction
    private void FunPrint(String strFlag)
    {
        int ErrorCode = 0;
        //string Flag;
        //Flag = "P";
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesClient();
        try
        {

            int counts = 0;
            foreach (GridViewRow grv in gvDlivery.Rows)
            {
                if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                {
                    counts++;
                }

            }
            if (counts == 0)
            {
                cvDelivery.ErrorMessage = "Select atleast one Asset details";
                cvDelivery.IsValid = false;
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Asset_Description");
            dt.Columns.Add("Model_description");
            dt.Columns.Add("Asset_quantity");
            dt.Columns.Add("Asset_Value");
            dt.Columns.Add("Remarks");
            int intRowindex = 0;
            foreach (GridViewRow grv in gvDlivery.Rows)
            {
                if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                {
                    //      Label lalAssetCode = grv.FindControl("lblAssetCode") as Label;
                    Label lalAssetDesc = grv.FindControl("lblAssetDesc") as Label;
                    TextBox txtModel = grv.FindControl("txtModelDesc") as TextBox;
                    TextBox txtquly = grv.FindControl("txtQuantity") as TextBox;
                    TextBox txtvalue = grv.FindControl("txtAssetValue") as TextBox;
                    TextBox txtRemarks = grv.FindControl("txtRemarks") as TextBox;
                    dt.NewRow();
                    DataRow dr = dt.NewRow();
                    //    dr["Asset_Code"] = lalAssetCode.Text;
                    dr["Asset_Description"] = lalAssetDesc.Text;
                    dr["Model_description"] = txtModel.Text;
                    dr["Asset_quantity"] = txtquly.Text;
                    dr["Asset_Value"] = txtvalue.Text;
                    dr["Remarks"] = txtRemarks.Text;
                    dt.Rows.Add(dr);

                }

            }
            ViewState["dt"] = dt;
            intRowindex++;


            if (txtLPONo.Text != string.Empty)
            {



                String htmlText;

                int intResult = 0;

                DataSet dset = new DataSet();
                dset = (DataSet)ViewState["DSet"];

                if (strFlag == "P")
                {
                    dset.Tables[1].Rows[0]["Is_DIPrint"] = "1";
                    intResult = objLoanAdmin_MasterClient.FunCancelDeliveryIns(out ErrorCode,intPO_Hdr_ID, strFlag);
                }
                if (strFlag == "D")
                {

                    //Code modified for Modified Report - For both Oracle and SQL DB - Kuppusamy.B - 18-April-2012
                    if (dset.Tables[1].Rows[0]["Is_DIGeneration"] == "1")
                    {
                        dset.Tables[1].Rows[0]["Is_DIPrint"] = "1";
                    }


                    dset.Tables[1].Rows[0]["Is_DIGeneration"] = "1";

                    intResult = objLoanAdmin_MasterClient.FunCancelDeliveryIns(out ErrorCode, intPO_Hdr_ID, strFlag );
                }
                ViewState["DSet"] = dset;


                btnEmail.Enabled = true;
                btnPrint.Enabled = true;

            }
        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "DI/LPO Generation PDF already open");
            //throw ex;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvDelivery.ErrorMessage = "Unable to Generate Delivery Instruction / LPO ";
            cvDelivery.IsValid = false;
        }
        finally
        {
            if (objLoanAdmin_MasterClient != null)
            {
                objLoanAdmin_MasterClient.Close();

            }
        }
    }
    #region HTML Formation
    private string FunPriGetHtmlTable()
    {
        DataTable dt_Asset = new DataTable();
        dt_Asset = (DataTable)ViewState["dt"];
        string strHtml = string.Empty;
        // strHtml = "<table border=\"1\" >";// border=\"1px\" width=\"100%\">";
        for (int i_row = 0; i_row < dt_Asset.Rows.Count; i_row++)
        {
            strHtml += " <tr>";

            for (int i_column = 0; i_column < dt_Asset.Columns.Count; i_column++)
            {

                //for (int i = 0; i < dt_Asset.Rows.Count; i++)
                //{
                if ((dt_Asset.Columns[i_column].ColumnName == "Asset_quantity") || (dt_Asset.Columns[i_column].ColumnName == "Asset_Value"))
                {
                    strHtml += " <td align =\"right\" > ";
                }
                else
                {
                    strHtml += " <td> ";
                }
                strHtml += dt_Asset.Rows[i_row][i_column].ToString();

                strHtml += " </td> ";

            }
            strHtml += " </tr> ";
            //}

        }
        // strHtml += "</table>";
        dt_Asset.Clear();
        dt_Asset.Dispose();
        return strHtml;
    }
    #endregion

    #region Email

    //protected void FunEmailGeneration(string strFilePath)
    //{
    //    try
    //    {
    //        Dictionary<string, string> dictMail = new Dictionary<string, string>();
    //        //if (S3GVendorAddress.EmailID == string.Empty)
    //        //{
    //        //    Utility.FunShowAlertMsg(this.Page, " Unable to send the Email Due to Email Id is not Given");
    //        //    return;
    //        //}
    //        //dictMail.Add("FromMail", "manikandan.r@sundaraminfotech.in");
    //        //dictMail.Add("ToMail",  S3GVendorAddress.EmailID);
    //        //dictMail.Add("Subject", "Delivery Instruction/LPO");

    //        ArrayList arrMailAttachement = new ArrayList();
    //        arrMailAttachement.Add(strFilePath);

    //        StringBuilder strBody = new StringBuilder();
    //        strBody.Append("DI created sucessfully");

    //        try
    //        {
    //            Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
    //            //Utility.FunShowAlertMsg(this, "Mail sent successfully");
    //        }
    //        catch (Exception objException)
    //        {
    //            //if (objException.Message == "Mailbox unavailable. The server response was: 5.7.1 Unable to relay")
    //            if (objException.Message == "Error in :Mailbox unavailable. The server response was: 5.7.1 Client does not have permissions to send as this sender")
    //            {
    //                Utility.FunShowAlertMsg(this, "Mail not sent.");
    //            }
    //            return;
    //        }
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
    //        //Utility.FunShowAlertMsg(this, "Unable to Send Mail");
    //        if (objException.Message == "Error in :Mailbox unavailable. The server response was: 5.7.1 Client does not have permissions to send as this sender")
    //        {
    //            Utility.FunShowAlertMsg(this, "Mail not sent.");
    //        }
    //        return;
    //    }

    //}

    protected void btnEmail_Click(object sender, EventArgs e)
    {
        try
        {
            //   FunCrstalReportGeneration("E");

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            //Utility.FunShowAlertMsg(this, "Unable to Send Mail");
            if (objException.Message == "Mailbox unavailable. The server response was: 5.7.1 Unable to relay")
            {
                Utility.FunShowAlertMsg(this, "Mail not sent.");
            }
            return;
        }

    }
    #endregion

    #endregion

    #region Clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (bCreate)
            btnSave.Enabled = true;
        Clear();
    }

    private void Clear()
    {
        S3GCustomerAddress1.ClearCustomerDetails();
        ucCustomerCodeLov.FunPubClearControlValue();
        txtVendorName.Clear();
        ddlLoc_Code.SelectedIndex = 0;
        ddlLoc_Code.Enabled = false;
        txtNotes1.Text = txtNotes2.Text = txtPaymentTerms.Text = txtDeliveryTerms.Text = txtWarrantyTerms.Text = txtOthers1.Text = String.Empty;
        gvDlivery.DataSource = null;
        gvDlivery.DataBind();
        FunPriDeleteGrid();
    }
    #endregion


    #endregion
    #endregion

    #region CrstalReport Open
    //private void FunCrstalReportGeneration(string strFlag)
    //{
    //    try
    //    {
    //        Guid objGuid;
    //        objGuid = Guid.NewGuid();
    //        DataSet dset = new DataSet();

    //        dictParam = new Dictionary<string, string>();
    //        dictParam.Add("@PO_Hdr_ID", intPO_Hdr_ID.ToString());
    //        if (Session["VendorGroup"] != null)
    //            dictParam.Add("@Vendor_Group", Session["VendorGroup"].ToString());

    //        dset = Utility.GetDataset("S3G_ORG_Get_PurchaseOrder_Print", dictParam);

    //        ReportDocument rpd = new ReportDocument();
    //        //rpd.Close();
    //        //rpd.Dispose();

    //        //rpd = new ReportDocument();

    //        rpd.Load(Server.MapPath("PurchaseOrder.rpt"));

    //        rpd.SetDataSource(dset.Tables[0]);
    //        rpd.Subreports[0].SetDataSource(dset.Tables[1]);

    //        string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + txtLPONo.Text.Replace("/", "") + objGuid.ToString() + ".pdf";

    //        string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

    //        if (!(System.IO.Directory.Exists(strFolder)))
    //        {
    //            DirectoryInfo di = Directory.CreateDirectory(strFolder);

    //        }
    //        string strScipt = "";
    //        rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName); //ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "DeliveryInstruction");
    //        if (strFlag == "E")
    //        {
    //            FunEmailGeneration(strFileName);
    //            return;
    //        }
    //        else if (strFlag == "D")
    //        {
    //            strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=/LoanAdmin/PDF Files/DI/" + txtLPONo.Text.Replace("/", "") + objGuid.ToString() + ".pdf', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
    //        }

    //        else
    //        {
    //            strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
    //        }
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

    //        //if (strFlag == "D")
    //        //    FunPrint("D");
    //        //else
    //        //    FunPrint("P");

    //    }
    //    catch (System.IO.IOException ex)
    //    {
    //        Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print PO");
    //        //throw ex;
    //    }
    //    catch (Exception objException)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
    //        cvDelivery.ErrorMessage = "Unable to Generate Delivery Instruction / LPO ";
    //        cvDelivery.IsValid = false;
    //    }
    //}

    protected string GetAutoPrintJs()
    {
        var script = new StringBuilder();
        script.Append("var pp = getPrintParams();");
        script.Append("pp.interactive= pp.constants.interactionLevel.full;");
        script.Append("print(pp);"); return script.ToString();
    }
    #endregion
    protected void FunGetCompanyDetails(int Company_Id)
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", intCompanyId.ToString());

        DataTable dtCompany = Utility.GetDefaultData("S3G_LOANAD_GetCompany_DO", dictParam);

        S3GCustomerAddress1.SetCustomerDetails(dtCompany.Rows[0], true);


    }
    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        DataTable dtAssetCode;
        DataRow Drow;


        dtAssetCode = new DataTable("AssetDetails");
        if (ViewState["AssetDetails"] == null)
        {
            dtAssetCode.Columns.Add("Asset_ID");
            dtAssetCode.Columns.Add("Asset_Code");
            dtAssetCode.Columns.Add("Asset_Description");
            dtAssetCode.Columns.Add("Model_Description");
            dtAssetCode.Columns.Add("ASSET_COST");
            dtAssetCode.Columns.Add("Asset_quantity");
            dtAssetCode.Columns.Add("Asset_Value");
            dtAssetCode.Columns.Add("Remarks");
        }
        else
        {
            dtAssetCode = (DataTable)ViewState["AssetDetails"];
        }


        if (dtAssetCode.Rows.Count >= 0)
        {
            Drow = dtAssetCode.NewRow();
            if (dtAssetCode.Rows.Count > 0)
            {
                dtAssetCode = (DataTable)ViewState["AssetDetails"];
            }


            dtAssetCode.Rows.Add(Drow);
            ViewState["AssetDetails"] = dtAssetCode;
            gvDlivery.DataSource = dtAssetCode;
            gvDlivery.DataBind();

        }
        foreach (GridViewRow grv in gvDlivery.Rows)
        {

            TextBox txtAssetValue = grv.FindControl("txtAssetValue") as TextBox;
            TextBox txtDyAsset = grv.FindControl("txtDyAsset") as TextBox;
            TextBox txtQuantity = grv.FindControl("txtQuantity") as TextBox;

            txtAssetValue.ReadOnly = false;
            txtDyAsset.ReadOnly = false;
            txtDyAsset.Visible = false;

            txtQuantity.ReadOnly = false;

            txtAssetValue.Text = txtDyAsset.Text;

        }
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
        if (hdnCustomerId != null)
        {
            if (hdnCustomerId.Value != "")
            {
                Dictionary<string, string> procparam = new Dictionary<string, string>();
                procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
                procparam.Add("@User_ID", Convert.ToString(intUserId));
                procparam.Add("@Customer_ID", hdnCustomerId.Value);
                DataTable dt = new DataTable();
                dt = Utility.GetDefaultData("S3G_ORG_GetIs_POBlocked", procparam);

                if (dt.Rows[0]["Is_PO_Black"].ToString() == "1")
                {
                    Utility.FunShowAlertMsg(this.Page, "PO Blocked, Unable to Proceed!.");
                    S3GCustomerAddress1.ClearCustomerDetails();
                    ucCustomerCodeLov.FunPubClearControlValue();
                    return;
                }
                FunPubQueryExistCustomerListEnquiryUpdation(Convert.ToInt32(hdnCustomerId.Value));
            }
        }
    }

    private void FunPubQueryExistCustomerListEnquiryUpdation(int CustomerID)
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@CustomerID", CustomerID.ToString());
        dictParam.Add("@CompanyID", intCompanyId.ToString());
        dictParam.Add("@User_Id", intUserId.ToString());
        DataSet dsCustomer = Utility.GetDataset("S3G_Get_Exist_Customer_Details", dictParam);

        if (dsCustomer.Tables[0].Rows[0]["ErrorCode"].ToString() == "1")
        {
            Utility.FunShowAlertMsg(this.Page, "MRA is not available for the selected customer");
            S3GCustomerAddress1.ClearCustomerDetails();
            ucCustomerCodeLov.FunPubClearControlValue();
            return;
        }
        else if (dsCustomer.Tables[0].Rows[0]["ErrorCode"].ToString() == "2")
        {
            Utility.FunShowAlertMsg(this.Page, "MRA is not approved for the selected customer");
            S3GCustomerAddress1.ClearCustomerDetails();
            ucCustomerCodeLov.FunPubClearControlValue();
            return;
        }

        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        txtName.Text = txtCustomerCode.Text = dsCustomer.Tables[1].Rows[0]["Customer_Code"].ToString();
        S3GCustomerAddress1.SetCustomerDetails(dsCustomer.Tables[1].Rows[0]["Customer_Code"].ToString(),
                dsCustomer.Tables[1].Rows[0]["comm_Address1"].ToString() + "\n" +
         dsCustomer.Tables[1].Rows[0]["comm_Address2"].ToString() + "\n" +
        dsCustomer.Tables[1].Rows[0]["comm_city"].ToString() + "\n" +
        dsCustomer.Tables[1].Rows[0]["comm_state"].ToString() + "\n" +
        dsCustomer.Tables[1].Rows[0]["comm_country"].ToString() + "\n" +
        dsCustomer.Tables[1].Rows[0]["comm_pincode"].ToString(), dsCustomer.Tables[1].Rows[0]["Customer_Name"].ToString(), dsCustomer.Tables[1].Rows[0]["Comm_Telephone"].ToString(),
        dsCustomer.Tables[1].Rows[0]["Comm_mobile"].ToString(),
        dsCustomer.Tables[1].Rows[0]["comm_email"].ToString(), dsCustomer.Tables[1].Rows[0]["comm_website"].ToString());
    }

    [System.Web.Services.WebMethod]
    public static string[] GetHSNCode(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_HSN_AGT", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSACCode(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_SAC_AGT", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetState(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_Id", obj_Page.intCompanyId.ToString());
        Procparam.Add("@User_Id", obj_Page.intUserId.ToString());
        Procparam.Add("@Type", "1");
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORGLPO_GET_BRANCHLIST_AGT", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranch(String prefixText, int count)
    {
        UserControls_S3GAutoSuggest txtBillingStateFT = (UserControls_S3GAutoSuggest)obj_Page.gvDlivery.FooterRow.FindControl("txtBillingStateFT");
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_Id", obj_Page.intCompanyId.ToString());
        Procparam.Add("@User_Id", obj_Page.intUserId.ToString());
        Procparam.Add("@Type", "2");
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Parent_Id", txtBillingStateFT.SelectedValue);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORGLPO_GET_BRANCHLIST_AGT", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchIT(String prefixText, int count)
    {
        UserControls_S3GAutoSuggest txtBillingStateIT = (UserControls_S3GAutoSuggest)obj_Page.gvDlivery.Rows[Convert.ToInt32(obj_Page.rowindex.Value)].FindControl("txtBillingStateIT");
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_Id", obj_Page.intCompanyId.ToString());
        Procparam.Add("@User_Id", obj_Page.intUserId.ToString());
        Procparam.Add("@Type", "2");
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Parent_Id", txtBillingStateIT.SelectedValue);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORGLPO_GET_BRANCHLIST_AGT", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));
        return suggestions.ToArray();
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        int isEffective = 0;
        int isChangedEffective = 0;

        if (Utility.StringToDate(LPODate.Text) < Utility.StringToDate(GSTEffectiveDate.Text))
            isEffective = 0;
        else
            isEffective = 1;

        if (Utility.StringToDate(txtDate.Text) < Utility.StringToDate(GSTEffectiveDate.Text))
            isChangedEffective = 0;
        else
            isChangedEffective = 1;

        if (isEffective != isChangedEffective)
        {
            if (gvDlivery.Rows.Count > 0)
            {
                Utility.FunShowAlertMsg(this, "PO Number is inappropriate for the selected Date. Please Clear/Cancel");
                txtDate.Text = LPODate.Text;
            }
        }
        LPODate.Text = txtDate.Text;
    }

    private void FunEmailGeneration(int IsCancelled)
    {
        LoanAdminMgtServicesClient objLoanAdmin_MasterClient = new LoanAdminMgtServicesClient(); ;
        StringBuilder strPODetails = new StringBuilder();
        int intRowCt = 0;
        string strHTML_Email = "";
        string strHTML_Email_Base = "";
        string strPONos = "";
        StringBuilder strBody = new StringBuilder();
        ArrayList arrVendor = new ArrayList();
        DataSet dsEmail = new DataSet();
        try
        {

            //FunPriGeneratePOFiles(intCompanyId, 3, 0, 10, 0);
            FunPriGeneratePOFiles(IsCancelled);
            strPODetails.Append("<Root>");
            strPODetails.Append(
                       " <Details PO_Number = '" + txtLPONo.Text + "' />");
            strPODetails.Append("</Root>");
           arrVendor.Add(txtVendorName.SelectedValue);
            
            if (IsCancelled == 0)
            {
                strHTML_Email = PDFPageSetup.FunPubGetTemplateContent(1, 3, 0, 66, "0");
            }
            else
            {
                strHTML_Email = PDFPageSetup.FunPubGetTemplateContent(1, 3, 0, 67, "0");

            }
            if (strHTML_Email == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined for E-Mail");
                return;
            }

            List<string> listImageName = new List<string>();
            listImageName.Add("~CompanyLogoEmail~");
            List<string> listImagePath = new List<string>();
            listImagePath.Add("cid:CompanyLogoEmail");
            strHTML_Email = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML_Email);
            strHTML_Email_Base = strHTML_Email;

            ArrayList arrMailAttachement = new ArrayList();
            string strPOFile = "";
            string filePath_PO = Server.MapPath(".") + "\\PDF Files\\PO" + intUserId;
            string strLesseName = "";
            //Sending Mails for each Vendor Id Start
            for (int i = 0; i < arrVendor.Count; i++)
            {
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@XMLPODetail", strPODetails.ToString());
                dictParam.Add("@Vendor_ID", arrVendor[i].ToString());
                dsEmail = Utility.GetDataset("S3G_ORG_Get_PO_Email_Det", dictParam);
                arrMailAttachement.Clear();
                strBody.Clear();
                strPONos = "";
                strHTML_Email = strHTML_Email_Base;

                Dictionary<string, string> dictMail = new Dictionary<string, string>();
                if (dictMail.Count == 0)
                {
                    strLesseName = dsEmail.Tables[0].Rows[0]["Customer_Name"].ToString();
                    dictMail.Add("FromMail", dsEmail.Tables[0].Rows[0]["From_Email"].ToString());
                    dictMail.Add("ToCC", dsEmail.Tables[0].Rows[0]["CC"].ToString());
                    dictMail.Add("DisplayName", dsEmail.Tables[0].Rows[0]["Display_Name"].ToString());
                    dictMail.Add("ToMail", dsEmail.Tables[0].Rows[0]["Email_To"].ToString());
                    if (IsCancelled == 0)
                    {
                        dictMail.Add("Subject", dsEmail.Tables[0].Rows[0]["Subject_Name"].ToString());
                    }
                    else
                    {
                        dictMail.Add("Subject", "Cancelled - " + dsEmail.Tables[0].Rows[0]["Subject_Name"].ToString());
                    }
                    dictMail.Add("imgPath", Server.MapPath("../Images/TemplateImages/" + CompanyId + "/Company_Logo_Email_Img.jpg"));
                    filePath_zip = Server.MapPath(".") + "\\PDF Files\\PO_OPC_" + dsEmail.Tables[0].Rows[0]["Short_Name"].ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".zip";
                }

                if (Directory.Exists(filePath_PO))
                {
                    string[] str1 = Directory.GetFiles(filePath_PO);
                    foreach (string fileName in str1)
                    {
                        File.Delete(fileName);
                    }

                    Directory.Delete(filePath_PO);
                }

                if (!(System.IO.Directory.Exists(filePath_PO)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(filePath_PO);

                }
                foreach (DataRow rowPO in dsEmail.Tables[1].Rows)
                {

                    if (Procparam_PO_PDF.TryGetValue(rowPO["PO_Number"].ToString(), out strPOFile))
                    {
                        if (!(File.Exists(filePath_PO + "\\" + Path.GetFileName(strPOFile))))
                        {
                            File.Copy(strPOFile, filePath_PO + "\\" + Path.GetFileName(strPOFile));
                        }
                    }

                    if (strPONos == "")
                    {
                        strPONos = rowPO["PO_Number"].ToString();
                    }
                    else
                    {
                        strPONos = strPONos + "," + rowPO["PO_Number"].ToString();

                        //if (!(strPONos.Contains(rowPO["PO_Number"].ToString())))
                        //{
                        //    strPONos = strPONos + "," + rowPO["PO_Number"].ToString();

                        //}
                    }

                }

                //before creation of compressed folder,deleting it if exists
                //if (File.Exists(filePath_zip))
                //{
                //    File.Delete(filePath_zip);
                //}
                //if (IsCancelled == 0)
                //{
                if (!File.Exists(filePath_zip))
                {
                    //creating a zip file from one folder to another folder
                    ZipFile.CreateFromDirectory(filePath_PO, filePath_zip);
                    arrMailAttachement.Add(filePath_zip);
                    //Delete The pdf file which is created

                    string[] str = Directory.GetFiles(filePath_PO);
                    foreach (string fileName in str)
                    {
                        File.Delete(fileName);
                    }

                    if (Directory.Exists(filePath_PO))
                    {
                        Directory.Delete(filePath_PO);
                    }
                }

                if (IsCancelled == 0)
                {
                    if (dsEmail.Tables[1].Rows.Count > 0)
                        if (strHTML_Email.Contains("~PONos_Table~"))
                            strHTML_Email = PDFPageSetup.FunPubBindTable("~PONos_Table~", strHTML_Email, dsEmail.Tables[1]);
                    strHTML_Email = strHTML_Email.Replace("~User_Name~", ObjUserInfo.ProUserNameRW);
                }
                else
                {
                    strHTML_Email = strHTML_Email.Replace("~PO_Nos~", strPONos);
                    strHTML_Email = strHTML_Email.Replace("~Lessee_Name~", strLesseName);
                    strHTML_Email = strHTML_Email.Replace("~User_Name~", ObjUserInfo.ProUserNameRW);
                }
                strBody = strBody.Append(strHTML_Email);
                string strErrMsg = Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
                if (strErrMsg != "")
                {
                    if (strExceptionEmail == "")
                    {
                        strExceptionEmail = dsEmail.Tables[0].Rows[0]["Entity_Name"].ToString();
                    }
                    else
                    {
                        strExceptionEmail = strExceptionEmail + "," + dsEmail.Tables[0].Rows[0]["Entity_Name"].ToString();
                    }
                }
            }
            //Sending Mails for each Vendor Id End
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objLoanAdmin_MasterClient.Close();
        }
    }

}
#endregion

