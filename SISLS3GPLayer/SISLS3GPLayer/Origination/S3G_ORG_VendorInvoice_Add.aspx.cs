#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Vendor Invoice
/// Created By			: Chandrasekar K
/// Created Date		: 30-Oct-2014
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
public partial class Origination_S3G_ORG_VendorInvoice_Add : ApplyThemeForProject
{
    #region Intialization
    Dictionary<string, string> dictParam = null;
    string strDateFormat;
    int intErrCode;
    int intPI_Hdr_ID;
    int intUserId;
    int intCompanyId;
    string _DINo;
    string strMode;
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
    static string strPageName = "Vendor Invoice";
    DataTable dtPO = new DataTable();
    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    int intTotalQty = 0;
    decimal dcmTotalAmount = 0;
    decimal dcmTotalTax = 0;
    decimal dcmTotal = 0;

    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGPIVITransLander.aspx?Code=VNINV';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_VendorInvoice_Add.aspx?qsMode=C';";

    SerializationMode SerMode = SerializationMode.Binary;
    LoanAdminMgtServicesClient objLoanAdmin_MasterClient;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjPOService;

    LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable ObjS3G_GetAssetDataTable = new LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable();
    LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable ObjS3G_GetCustDataTable = new LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable();
    public static Origination_S3G_ORG_VendorInvoice_Add obj_Page;

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

    //added by vinodha m to get page number for asset grid  starts

    public int ProAssGrdPageSizeRW
    {
        get;
        set;
    }

    //added by vinodha m to get page number for asset grid ends

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {

        ProgramCode = "293";
        obj_Page = this;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            //ADDED BY VINODHA M FOR (TO LOCK UNIQUE RECORD WHEN MORE THAN ONE USER TRY TO MODIFY) STARTS   
            string[] strFromTicket = fromTicket.Name.Split('~');
            intPI_Hdr_ID = Convert.ToInt32(strFromTicket[0].ToString());
            //ADDED BY VINODHA M FOR (TO LOCK UNIQUE RECORD WHEN MORE THAN ONE USER TRY TO MODIFY) ENDS
        }

        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];

        UserInfo ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;

        txtDate.Attributes.Add("readonly", "readonly");
        S3GSession ObjS3GSession = new S3GSession();

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

        TextBox txtPageSize1 = (TextBox)ucPopUpPaging.FindControl("txtPageSize");
        if (txtPageSize1.Text != "")
            ProAssGrdPageSizeRW = Convert.ToInt32(txtPageSize1.Text);
        else
            ProAssGrdPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));
        PageAssignValue objPop = new PageAssignValue(this.AssignValuePop);
        ucPopUpPaging.callback = objPop;
        ucPopUpPaging.ProPageNumRW = ProPageNumRW;
        ucPopUpPaging.ProPageSizeRW = ProAssGrdPageSizeRW;

        ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
        TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
        txtUserName.Attributes.Add("onfocus", "fnLoadCustomer()");
        txtUserName.ToolTip = txtUserName.Text;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        CalendarExtenderPIDate.Format = strDateFormat;

        if (!IsPostBack)
        {
            FunPriLoadLOV();
            if (Request.QueryString["qsMode"] == "Q")
            {
                FunPriDisableControls(-1);
                Cache["PIDetails"] = gvDlivery.DataSource;
            }
            else if (Request.QueryString["qsMode"] == "M")
            {
                FunPriDisableControls(1);
            }
            else
            {
                FunPriDisableControls(0);
            }
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

        }
    }

    private void FunPriLoadLOV()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Clear();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            GSTEffectiveDate.Text = Utility.GetDefaultData("S3G_SYSAD_GSTEFFECTIVEDATE", dictParam).Rows[0]["GST_Effective_From"].ToString();
            //PremiumUser.Text = Utility.GetDefaultData("S3G_SYSAD_GSTPREMIUMUSER", dictParam).Rows[0]["premium_user"].ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            gvDlivery.EditIndex = -1;
            ProPageNumRW = intPageNum;              // To set the page Number
            ProPageSizeRW = intPageSize;            // To set the page size    
            FunPriGetTempPIGridDetails();                       // Binding the Landing grid
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void AssignValuePop(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;              // To set the page Number
            ProPageSizeRW = intPageSize;            // To set the page size    
            FunPriBindPopUpGrid();                       // Binding the Landing grid
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region Bind Grid

    protected void grvInvoices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvInvoices.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {
            cvDelivery.ErrorMessage = exp.Message;
            cvDelivery.IsValid = false;
        }
    }

    protected void gvInvoiceDet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPIQuantity = (Label)e.Row.FindControl("lblPIQuantity");
                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                Label lblTax_Amount = (Label)e.Row.FindControl("lblTax_Amount");
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");

                intTotalQty += lblPIQuantity.Text != "" ? Convert.ToInt32(lblPIQuantity.Text) : 0;
                dcmTotalAmount += lblAmount.Text != "" ? Convert.ToDecimal(lblAmount.Text) : 0;
                dcmTotalTax += lblTax_Amount.Text != "" ? Convert.ToDecimal(lblTax_Amount.Text) : 0;
                dcmTotal += lblTotal.Text != "" ? Convert.ToDecimal(lblTotal.Text) : 0;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotQuantity = (Label)e.Row.FindControl("lblTotQuantity");
                Label lblTotAmount = (Label)e.Row.FindControl("lblTotAmount");
                Label lblTotalTax_Amount = (Label)e.Row.FindControl("lblTotalTax_Amount");
                Label lblGTotal = (Label)e.Row.FindControl("lblGTotal");

                lblTotQuantity.Text = intTotalQty.ToString();
                lblTotAmount.Text = dcmTotalAmount.ToString();
                lblTotalTax_Amount.Text = dcmTotalTax.ToString();
                lblGTotal.Text = dcmTotal.ToString();
            }
        }
        catch (Exception exp)
        {
            cvDelivery.ErrorMessage = exp.Message;
            cvDelivery.IsValid = false;
        }
    }

    protected void gvDlivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

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
            DataView dtPO = new DataView();
            dtPO = (DataView)Cache["PIDetails"];

            //gvDlivery.DataSource = dtPO;
            //gvDlivery.DataBind();

            /*Parameter Added to get the current page number*/

            Label lblCurrentPage = (Label)ucCustomPaging.FindControl("lblCurrentPage");
            if (lblCurrentPage.Text != "")
                ProPageNumRW = Convert.ToInt32(lblCurrentPage.Text);

            FunPriGetTempPIGridDetails();

            GridViewRow grvRow = gvDlivery.Rows[intRowId];

            TextBox txtPIQuantity = (TextBox)grvRow.FindControl("txtPIQuantity");
            TextBox txtLBTRate = (TextBox)grvRow.FindControl("txtLBTRate");
            TextBox txtBill_Amount_FC = (TextBox)grvRow.FindControl("txtBill_Amount_FC");
            TextBox txtBill_Amount_INR = (TextBox)grvRow.FindControl("txtBill_Amount_INR");
            TextBox txtBase_Inv_Amt_Mat = (TextBox)grvRow.FindControl("txtBase_Inv_Amt_Mat");
            TextBox txtBase_Inv_Amt_Lab = (TextBox)grvRow.FindControl("txtBase_Inv_Amt_Lab");
            TextBox txtVAT = (TextBox)grvRow.FindControl("txtVAT");
            TextBox txtCST = (TextBox)grvRow.FindControl("txtCST");
            TextBox txtExcise_Duty = (TextBox)grvRow.FindControl("txtExcise_Duty");
            TextBox txtService_Tax = (TextBox)grvRow.FindControl("txtService_Tax");
            TextBox txtOthers = (TextBox)grvRow.FindControl("txtOthers");
            TextBox txtCrNoteAmount = (TextBox)grvRow.FindControl("txtCrNoteAmount");
            TextBox txtBaseAmtVAT = (TextBox)grvRow.FindControl("txtBaseAmtVAT");
            TextBox txtVAT1 = (TextBox)grvRow.FindControl("txtVAT1");
            TextBox txtOtherBillComponent = (TextBox)grvRow.FindControl("txtOtherBillComponent");
            TextBox txtTDSBaseValue = (TextBox)grvRow.FindControl("txtTDSBaseValue");
            TextBox txtWCTBaseValue = (TextBox)grvRow.FindControl("txtWCTBaseValue");
            TextBox txtCENVAT = (TextBox)grvRow.FindControl("txtCENVAT");
            TextBox txtRetention = (TextBox)grvRow.FindControl("txtRetention");
            TextBox txtBaseValue = (TextBox)grvRow.FindControl("txtBaseValue");

            AjaxControlToolkit.CalendarExtender CalendarExtenderWay_BillDate = (AjaxControlToolkit.CalendarExtender)grvRow.FindControl("CalendarExtenderWay_BillDate");
            TextBox txtWay_Bill_Issue_Date = (TextBox)grvRow.FindControl("txtWay_Bill_Issue_Date");

            CalendarExtenderWay_BillDate.Format = strDateFormat;
            txtWay_Bill_Issue_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtWay_Bill_Issue_Date.ClientID + "','" + strDateFormat + "',true,  false);");

            txtPIQuantity.SetDecimalPrefixSuffix(10, 0, true, false, "VI Quantity");
            txtLBTRate.SetDecimalPrefixSuffix(3, 2, false, false, "LBT Rate");
            txtVAT1.SetDecimalPrefixSuffix(3, 2, false, false, "VAT Percentage");

            //added by vinodha m for the impact of file upload 4 decimals starts

            txtBill_Amount_FC.SetUserDecimalPrefixSuffix(13, 4, false, "Total Bill Amount in FC");
            txtBill_Amount_INR.SetUserDecimalPrefixSuffix(13, 4, false, "Total Bill Amount in INR");
            txtBase_Inv_Amt_Mat.SetUserDecimalPrefixSuffix(13, 4, false, "Base Inv Amt(Excl Tax)-Material");
            txtBase_Inv_Amt_Lab.SetUserDecimalPrefixSuffix(13, 4, false, "Base Inv Amt(Excl Tax)-Labour");
            txtVAT.SetUserDecimalPrefixSuffix(13, 4, false, "VAT");
            txtCST.SetUserDecimalPrefixSuffix(13, 4, false, "CST");
            txtExcise_Duty.SetUserDecimalPrefixSuffix(13, 4, false, "Excise Duty");
            txtService_Tax.SetUserDecimalPrefixSuffix(13, 4, false, "Service Tax");
            txtOthers.SetUserDecimalPrefixSuffix(13, 4, false, "Others");
            txtCrNoteAmount.SetUserDecimalPrefixSuffix(13, 4, false, "Cr Note Amount");
            txtBaseAmtVAT.SetUserDecimalPrefixSuffix(13, 4, false, "Base Amt VAT");
            txtOtherBillComponent.SetUserDecimalPrefixSuffix(13, 4, false, "Other Bill Component");
            txtTDSBaseValue.SetUserDecimalPrefixSuffix(13, 4, false, "TDS Base Value");
            txtWCTBaseValue.SetUserDecimalPrefixSuffix(13, 4, false, "WCT Base Value");
            txtCENVAT.SetUserDecimalPrefixSuffix(13, 4, false, "CENVAT Amount Passed on to Cusomer");
            txtRetention.SetUserDecimalPrefixSuffix(13, 4, false, "Retention");
            txtBaseValue.SetUserDecimalPrefixSuffix(13, 4, false, "Base Value for Reverse Charge");

            //added by vinodha m for the impact of file upload 4 decimals ends

            //DropDownList ddlRegistration_Status = (DropDownList)grvRow.FindControl("ddlRegistration_Status");
            //DropDownList ddlConstitution = (DropDownList)grvRow.FindControl("ddlConstitution");

            //DropDownList ddlCurrencyCode = (DropDownList)grvRow.FindControl("ddlCurrencyCode");
            DropDownList ddlVendInvType = (DropDownList)grvRow.FindControl("ddlVendInvType");
            DropDownList ddlReverseCharge = (DropDownList)grvRow.FindControl("ddlReverseCharge");

            DropDownList ddlLBT = (DropDownList)grvRow.FindControl("ddlLBT");
            DropDownList ddlLBT_Circle_No = (DropDownList)grvRow.FindControl("ddlLBT_Circle_No");
            DropDownList ddlPurchaseTaxApp = (DropDownList)grvRow.FindControl("ddlPurchaseTaxApp");
            DropDownList ddlITC = (DropDownList)grvRow.FindControl("ddlITC");
            DropDownList ddlVATRebate = (DropDownList)grvRow.FindControl("ddlVATRebate");

            ddlLBT.SelectedValue = dtPO[intRowId]["LBT_Applicable"].ToString();
            ddlPurchaseTaxApp.SelectedValue = dtPO[intRowId]["Purchase_Tax_Applicable"].ToString();
            ddlITC.SelectedValue = dtPO[intRowId]["ITC"].ToString();
            ddlVATRebate.SelectedValue = dtPO[intRowId]["VAT_Rebate"].ToString();

            //dictParam = new Dictionary<string, string>();
            //dictParam.Add("@Is_Active", "1");
            //DataTable dtCurrency = Utility.GetDefaultData("S3G_SYS_Get_Currency_List", dictParam);
            //ddlCurrencyCode.BindDataTable(dtCurrency, "Currency_Code", "Currency_Code");
            //ddlCurrencyCode.SelectedValue = dtPO[intRowId]["Currency_Code"].ToString();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", intCompanyId.ToString());
            dictParam.Add("@User_Id", intUserId.ToString());
            DataSet dtReverseCharge = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", dictParam);
            ddlReverseCharge.BindDataTable(dtReverseCharge.Tables[1], "Name", "Name");
            ddlReverseCharge.SelectedValue = dtPO[intRowId]["Reverse_Charge_Type"].ToString();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@OptionValue", "5");
            DataTable dtVendInvType = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", dictParam);
            ddlVendInvType.BindDataTable(dtVendInvType, "Name", "Name");
            ddlVendInvType.SelectedValue = dtPO[intRowId]["Vend_Inv_Type"].ToString();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@OptionValue", "6");
            DataTable dtLBT = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", dictParam);
            ddlLBT_Circle_No.BindDataTable(dtLBT, "Value", "Name");
            ddlLBT_Circle_No.SelectedValue = dtPO[intRowId]["LBT_Circle_No_Value"].ToString();

            //dictParam = new Dictionary<string, string>();
            //dictParam.Add("@Company_Id", intCompanyId.ToString());
            //dictParam.Add("@User_Id", intUserId.ToString());
            //dictParam.Add("@Lookup_Desc", "Registration_Status");
            //DataTable Registration_Status = Utility.GetDefaultData("S3G_ORG_GetLookup_PL", dictParam);
            //ddlRegistration_Status.BindDataTable(Registration_Status, "Id", "Value");
            //ddlRegistration_Status.SelectedValue = dtPO[intRowId]["Registration_Status"].ToString();

            //dictParam = new Dictionary<string, string>();
            //dictParam.Add("@Company_Id", intCompanyId.ToString());
            //dictParam.Add("@LOB_ID", "3");
            //dictParam.Add("@Is_Active", "1");
            //DataTable Constitution = Utility.GetDefaultData("S3G_ORG_GetConstitution", dictParam);
            //ddlConstitution.BindDataTable(Constitution, "Constitution_ID", "Constitution_Name");
            //ddlConstitution.SelectedValue = dtPO[intRowId]["Constitution"].ToString();

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
                txtVAT.Enabled = txtCST.Enabled = txtExcise_Duty.Enabled = txtService_Tax.Enabled = txtOthers.Enabled = false;
                txtVAT.Text = txtCST.Text = txtExcise_Duty.Text = txtService_Tax.Text = txtOthers.Text = "0";

                txtHSNCodeIT.Enabled = txtSACCodeIT.Enabled = txtBillingStateIT.Enabled = txtBillingBranchIT.Enabled
                    = txtHSNSGSTIT.Enabled = txtHSNCGSTIT.Enabled = txtHSNIGSTIT.Enabled =
                    txtSACSGSTIT.Enabled = txtSACCGSTIT.Enabled = txtSACIGSTIT.Enabled = true;
            }
            else
            {
                txtHSNCodeIT.Enabled = txtSACCodeIT.Enabled = txtBillingStateIT.Enabled = txtBillingBranchIT.Enabled = txtHSNSGSTIT.Enabled
                    = txtHSNCGSTIT.Enabled = txtHSNIGSTIT.Enabled = txtSACSGSTIT.Enabled = txtSACCGSTIT.Enabled = txtSACIGSTIT.Enabled = false;

                txtHSNCodeIT.SelectedValue = txtSACCodeIT.SelectedValue = txtBillingStateIT.SelectedValue = txtBillingBranchIT.SelectedValue = "0";
                txtHSNSGSTIT.Text = txtHSNCGSTIT.Text = txtHSNIGSTIT.Text =
                      txtSACSGSTIT.Text = txtSACCGSTIT.Text = txtSACIGSTIT.Text = "0";
                txtVAT.Enabled = txtCST.Enabled = txtExcise_Duty.Enabled = txtService_Tax.Enabled = txtOthers.Enabled = true;
            }
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
            GridViewRow grvRow = gvDlivery.Rows[e.RowIndex];

            Label lblPI_det_ID = (Label)grvRow.FindControl("lblPI_dtl_ID");
            Label lblPO_DTL_ID = (Label)grvRow.FindControl("lblPO_dtl_ID");
            Label lblPO_HDR_ID = (Label)grvRow.FindControl("lblPO_HDR_ID");

            //DropDownList ddlRegistration_Status = (DropDownList)grvRow.FindControl("ddlRegistration_Status");
            //DropDownList ddlConstitution = (DropDownList)grvRow.FindControl("ddlConstitution");

            TextBox txtWay_Bill_No = (TextBox)grvRow.FindControl("txtWay_Bill_No");
            TextBox txtWay_Bill_Issue_Date = (TextBox)grvRow.FindControl("txtWay_Bill_Issue_Date");

            TextBox txtAsset_Serial_number = (TextBox)grvRow.FindControl("txtAsset_Serial_number");
            DropDownList ddlLBT = (DropDownList)grvRow.FindControl("ddlLBT");
            DropDownList ddlPurchaseTaxApp = (DropDownList)grvRow.FindControl("ddlPurchaseTaxApp");
            TextBox txtLBTRate = (TextBox)grvRow.FindControl("txtLBTRate");
            DropDownList ddlLBT_Circle_No = (DropDownList)grvRow.FindControl("ddlLBT_Circle_No");

            TextBox txtCurrencyCode = (TextBox)grvRow.FindControl("txtCurrencyCode");
            TextBox txtBill_Amount_FC = (TextBox)grvRow.FindControl("txtBill_Amount_FC");
            TextBox txtBill_Amount_INR = (TextBox)grvRow.FindControl("txtBill_Amount_INR");
            TextBox txtBase_Inv_Amt_Mat = (TextBox)grvRow.FindControl("txtBase_Inv_Amt_Mat");
            TextBox txtBase_Inv_Amt_Lab = (TextBox)grvRow.FindControl("txtBase_Inv_Amt_Lab");
            TextBox txtVAT = (TextBox)grvRow.FindControl("txtVAT");
            TextBox txtCST = (TextBox)grvRow.FindControl("txtCST");
            TextBox txtExcise_Duty = (TextBox)grvRow.FindControl("txtExcise_Duty");
            TextBox txtService_Tax = (TextBox)grvRow.FindControl("txtService_Tax");
            TextBox txtOthers = (TextBox)grvRow.FindControl("txtOthers");
            TextBox txtOtherBillComponent = (TextBox)grvRow.FindControl("txtOtherBillComponent");
            TextBox txtRetention = (TextBox)grvRow.FindControl("txtRetention");

            TextBox txtCrNoteNumber = (TextBox)grvRow.FindControl("txtCrNoteNumber");
            TextBox txtCrNoteAmount = (TextBox)grvRow.FindControl("txtCrNoteAmount");
            TextBox txtBaseAmtVAT = (TextBox)grvRow.FindControl("txtBaseAmtVAT");
            DropDownList ddlITC = (DropDownList)grvRow.FindControl("ddlITC");
            TextBox txtVAT1 = (TextBox)grvRow.FindControl("txtVAT1");
            DropDownList ddlVATRebate = (DropDownList)grvRow.FindControl("ddlVATRebate");
            TextBox txtTDSSection = (TextBox)grvRow.FindControl("txtTDSSection");
            TextBox txtTDSBaseValue = (TextBox)grvRow.FindControl("txtTDSBaseValue");
            TextBox txtWCTBaseValue = (TextBox)grvRow.FindControl("txtWCTBaseValue");
            TextBox txtCENVAT = (TextBox)grvRow.FindControl("txtCENVAT");
            DropDownList ddlReverseCharge = (DropDownList)grvRow.FindControl("ddlReverseCharge");
            TextBox txtBaseValue = (TextBox)grvRow.FindControl("txtBaseValue");
            DropDownList ddlVendInvType = (DropDownList)grvRow.FindControl("ddlVendInvType");
            TextBox txtRemarks = (TextBox)grvRow.FindControl("txtRemarks");
            TextBox txtPIQuantity = (TextBox)grvRow.FindControl("txtPIQuantity");

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
            Label lblAssetTypeID = (Label)grvRow.FindControl("lblAssetTypeID");

            if (ddlLBT.SelectedValue == "1")
            {
                if (ddlLBT_Circle_No.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select the LBT Circle No");
                    ddlLBT_Circle_No.Focus();
                    return;
                }

                if (txtLBTRate.Text == "")
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter the LBT Circle Rate");
                    txtLBTRate.Focus();
                    return;
                }
            }

            if (txtTDSSection.Text != "" && txtTDSBaseValue.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the TDS Base Value");
                txtTDSBaseValue.Focus();
                return;
            }

            if ((ddlVendInvType.SelectedValue == "Comp_Invoice_VAT" || ddlVendInvType.SelectedValue == "Comp_Invoice_CST") && txtWCTBaseValue.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the WCT Base Value");
                txtWCTBaseValue.Focus();
                return;
            }

            if (ddlReverseCharge.SelectedValue != "0" && txtBaseValue.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the Reverse Charge Base Value");
                txtBaseValue.Focus();
                return;
            }
            if (Utility.StringToDate(txtDate.Text) < Utility.StringToDate(GSTEffectiveDate.Text))
            {
                if (txtVAT.Text != "" && txtCST.Text != "")
                {
                    Utility.FunShowAlertMsg(this.Page, "Either VAT or CST Only Applicable");
                    txtCST.Focus();
                    return;
                }
            }
            decimal strmaterial, strlabour, strHSNSGST, strHSNCGST, strHSNIGST, strSACSGST, strSACCGST, strSACIGST;
            if (txtBase_Inv_Amt_Mat.Text.ToString() != "")
                strmaterial = Convert.ToDecimal(txtBase_Inv_Amt_Mat.Text.ToString());
            else
                strmaterial = Convert.ToDecimal("0");

            if (txtBase_Inv_Amt_Lab.Text.ToString() != "")
                strlabour = Convert.ToDecimal(txtBase_Inv_Amt_Lab.Text.ToString());
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
                //    dictParam.Add("@Type_ID", lblAssetTypeID.Text);
                //    if (txtHSNCodeIT.SelectedValue != Utility.GetDefaultData("S3G_ORGLPO_ASSETHSNID", dictParam).Rows[0]["ID"].ToString())
                //    {
                //        Utility.FunShowAlertMsg(this.Page, "Invalid Asset HSN Code");
                //        return;
                //    }
                //}
            }
            dtPO = new DataTable();
            dtPO.Columns.Add("VI_det_ID");
            dtPO.Columns.Add("PO_DTL_ID");
            dtPO.Columns.Add("PO_HDR_ID");
            dtPO.Columns.Add("VI_No");
            dtPO.Columns.Add("VI_Date");
            dtPO.Columns.Add("VI_Quantity");
            //dtPO.Columns.Add("Registration_Status");
            //dtPO.Columns.Add("Constitution");
            dtPO.Columns.Add("Way_Bill_No");
            dtPO.Columns.Add("Way_Bill_Issue_Date");
            dtPO.Columns.Add("Asset_Serial_number");
            dtPO.Columns.Add("LBT_Applicable");
            dtPO.Columns.Add("LBT_Circle_No");
            dtPO.Columns.Add("LBT_Circle_No_Value");
            dtPO.Columns.Add("LBT_rate");
            dtPO.Columns.Add("Purchase_Tax_Applicable");
            dtPO.Columns.Add("Currency_Code");
            dtPO.Columns.Add("Bill_Amount_USD");
            dtPO.Columns.Add("Bill_Amount_INR");
            dtPO.Columns.Add("Inv_Amt_Material");
            dtPO.Columns.Add("Inv_Amt_Labour");
            dtPO.Columns.Add("VAT");
            dtPO.Columns.Add("CST");
            dtPO.Columns.Add("Excise_Duty_CVD");
            dtPO.Columns.Add("Service_Tax_Amt");
            dtPO.Columns.Add("Others");
            dtPO.Columns.Add("Cr_Note_Number");
            dtPO.Columns.Add("Credit_Note_Amount");
            dtPO.Columns.Add("Base_Amt_VAT");
            dtPO.Columns.Add("ITC");
            dtPO.Columns.Add("VAT_Percentage");
            dtPO.Columns.Add("Other_Bill_Component");
            dtPO.Columns.Add("VAT_Rebate");
            dtPO.Columns.Add("TDS_Section");
            dtPO.Columns.Add("TDS_Base_Value");
            dtPO.Columns.Add("WCT_Base_Value");
            dtPO.Columns.Add("CENVAT_Amount");
            dtPO.Columns.Add("Retention");
            dtPO.Columns.Add("Reverse_Charge_Type");
            dtPO.Columns.Add("Base_Value");
            dtPO.Columns.Add("Vend_Inv_Type");
            dtPO.Columns.Add("Remarks");

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
            row["VI_det_ID"] = lblPI_det_ID.Text;
            row["PO_DTL_ID"] = lblPO_DTL_ID.Text;
            row["PO_HDR_ID"] = lblPO_HDR_ID.Text;
            row["VI_Date"] = Utility.StringToDate(txtPIDate.Text).ToString();
            row["VI_Quantity"] = txtPIQuantity.Text;
            //row["Registration_Status"] = ddlRegistration_Status.SelectedValue;
            //row["Constitution"] = ddlConstitution.SelectedValue;
            row["VI_No"] = txtPINo.Text;
            row["Way_Bill_No"] = txtWay_Bill_No.Text;
            row["Way_Bill_Issue_Date"] = Utility.StringToDate(txtWay_Bill_Issue_Date.Text).ToString();
            row["Asset_Serial_number"] = txtAsset_Serial_number.Text;
            row["LBT_Applicable"] = ddlLBT.SelectedValue;
            if (ddlLBT_Circle_No.SelectedValue != "0")
                row["LBT_Circle_No"] = ddlLBT_Circle_No.SelectedItem.Text;
            row["LBT_Circle_No_Value"] = ddlLBT_Circle_No.SelectedValue;
            row["LBT_rate"] = txtLBTRate.Text;
            row["Purchase_Tax_Applicable"] = ddlPurchaseTaxApp.SelectedValue;
            row["Currency_Code"] = txtCurrencyCode.Text;
            row["Bill_Amount_USD"] = txtBill_Amount_FC.Text;
            row["Bill_Amount_INR"] = txtBill_Amount_INR.Text;
            row["Inv_Amt_Material"] = txtBase_Inv_Amt_Mat.Text;
            row["Inv_Amt_Labour"] = txtBase_Inv_Amt_Lab.Text;
            row["VAT"] = txtVAT.Text;
            row["CST"] = txtCST.Text;
            row["Excise_Duty_CVD"] = txtExcise_Duty.Text;
            row["Service_Tax_Amt"] = txtService_Tax.Text;
            row["Others"] = txtOthers.Text;
            row["Cr_Note_Number"] = txtCrNoteNumber.Text;
            row["Credit_Note_Amount"] = txtCrNoteAmount.Text;
            row["Base_Amt_VAT"] = txtBaseAmtVAT.Text;
            row["ITC"] = ddlITC.SelectedValue;
            row["VAT_Percentage"] = txtVAT1.Text;
            row["Other_Bill_Component"] = txtOtherBillComponent.Text;
            row["VAT_Rebate"] = ddlVATRebate.SelectedValue;
            row["TDS_Section"] = txtTDSSection.Text;
            row["TDS_Base_Value"] = txtTDSBaseValue.Text;
            row["WCT_Base_Value"] = txtWCTBaseValue.Text;
            row["CENVAT_Amount"] = txtCENVAT.Text;
            row["Retention"] = txtRetention.Text;
            row["Reverse_Charge_Type"] = ddlReverseCharge.SelectedValue;
            row["Base_Value"] = txtBaseValue.Text;
            row["Vend_Inv_Type"] = ddlVendInvType.SelectedValue;
            row["Remarks"] = txtRemarks.Text;

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
            objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable ObjS3G_DeliveryInsDataTable = new LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable();
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow;
            ObjDeliveryInsRow = ObjS3G_DeliveryInsDataTable.NewS3G_LOANAD_InsertDeliveryInstructionRow();
            ObjDeliveryInsRow.DeliveryInstruction_ID = intPI_Hdr_ID;
            ObjDeliveryInsRow.Company_ID = intCompanyId;
            ObjDeliveryInsRow.Created_By = intUserId;
            ObjDeliveryInsRow.Created_On = DateTime.Now;
            ObjDeliveryInsRow.TXN_Id = 11;
            ObjDeliveryInsRow.XML_DeliveryDeltails = dtPO.FunPubFormXml();

            ObjS3G_DeliveryInsDataTable.AddS3G_LOANAD_InsertDeliveryInstructionRow(ObjDeliveryInsRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_DeliveryInsDataTable, SerMode);

            intErrCode = objLoanAdmin_MasterClient.FunPubUpdateVI(SerMode, ObjDeliveryInsDataTable);

            if (intErrCode == 10)
            {
                Utility.FunShowAlertMsg(this.Page, "VI Quantity exceeding PO Quantity");
                return;
            }

            if (intErrCode == 11)
            {
                Utility.FunShowAlertMsg(this.Page, "VI Date should be Greater Than or Equal to PO Date");
                return;
            }

            if (intErrCode == 12)
            {
                Utility.FunShowAlertMsg(this.Page, "VI Number Already Exists");
                return;
            }

            if (intErrCode == 13)
            {
                Utility.FunShowAlertMsg(this.Page, "Asset Serial Number Already Exists");
                return;
            }

            if (intErrCode == 14)
            {
                Utility.FunShowAlertMsg(this.Page, "VI Amount exceeding PO Amount");
                return;
            }

            if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Purchase Tax not defined in master");
                return;
            }

            if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "TDS Rate not defined in master");
                return;
            }

            if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "WCT Tax not defined in master");
                return;
            }

            if (intErrCode == 4)
            {
                Utility.FunShowAlertMsg(this.Page, "Reverse Charge Tax not defined in master");
                return;
            }

            else if (intErrCode == 20)
            {
                Utility.FunShowAlertMsg(this.Page, "SGST Tax not defined in master");
                return;
            }
            else if (intErrCode == 21)
            {
                Utility.FunShowAlertMsg(this.Page, "CGST Tax not defined in master");
                return;
            }
            else if (intErrCode == 22)
            {
                Utility.FunShowAlertMsg(this.Page, "IGST Tax not defined in master");
                return;
            }
            else if (intErrCode == 23)
            {
                Utility.FunShowAlertMsg(this.Page, "SAC SGST Tax not defined in master");
                return;
            }
            else if (intErrCode == 24)
            {
                Utility.FunShowAlertMsg(this.Page, "SAC CGST Tax not defined in master");
                return;
            }
            else if (intErrCode == 25)
            {
                Utility.FunShowAlertMsg(this.Page, "SAC IGST Tax not defined in master");
                return;
            }
            gvDlivery.EditIndex = -1;

            FunPriGetTempPIGridDetails();

            Cache["PIDetails"] = gvDlivery.DataSource;
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
            FunPriGetTempPIGridDetails();
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
        }
    }

    #endregion

    #region To Get Proforma Invoice in Query Mode

    private void FunPriGetPIDetails()
    {
        try
        {
            DataTable dtDeliveryDetails = new DataTable();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@VI_Hdr_ID", intPI_Hdr_ID.ToString());
            if (Session["PO_Number"] != null)
                dictParam.Add("@PO_Number", Session["PO_Number"].ToString());
            if (Session["VI_Number"] != null)
                dictParam.Add("@VI_Number", Session["VI_Number"].ToString());
            //added by vinodha m to delete the temp table details for the specific user if exists(starts)
            dictParam.Add("@User_ID", intUserId.ToString());
            //added by vinodha m to delete the temp table details for the specific user if exists(ends)
            dtDeliveryDetails = Utility.GetDefaultData("S3G_ORG_Get_VI_Details", dictParam);

            txtPINo.Text = dtDeliveryDetails.Rows[0]["VI_Number"].ToString();
            txtDate.Text = dtDeliveryDetails.Rows[0]["PO_Date"].ToString();
            txtPIDate.Text = dtDeliveryDetails.Rows[0]["VI_Date"].ToString();
            txtLoadSequenceNumber.Text = dtDeliveryDetails.Rows[0]["LSQ_Number"].ToString();

            ddlPONo.ReadOnly = true;
            ddlPONo.SelectedValue = dtDeliveryDetails.Rows[0]["PO_Hdr_ID"].ToString();
            ddlPONo.SelectedText = dtDeliveryDetails.Rows[0]["PO_Number"].ToString();
            txtPIStatus.Text = dtDeliveryDetails.Rows[0]["Status"].ToString();

            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");

            hdnCustomerId.Value = dtDeliveryDetails.Rows[0]["Customer_ID"].ToString();
            txtName.Text = txtCustomerCode.Text = dtDeliveryDetails.Rows[0]["Customer_Code"].ToString();
            txtVendorName.Text = dtDeliveryDetails.Rows[0]["Entity_Name"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(dtDeliveryDetails.Rows[0], true);
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

    private void FunPriBindGrid()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@PO_HDR_ID", ddlPONo.SelectedValue);
        dictParam.Add("@PO_Number", ddlPONo.SelectedText);
        dictParam.Add("@Option", "1");
        dictParam.Add("@Ids", FunPubGetXML());
        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProUser_ID = intUserId;

        gvDlivery.BindGridView("S3G_Org_Get_VI_Temp", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucCustomPaging.Visible = true;
        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucCustomPaging.setPageSize(ProPageSizeRW);

        Cache["PIDetails"] = gvDlivery.DataSource;
    }

    public string FunPubGetXML()
    {
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");
        foreach (GridViewRow grvRow in grvInvoices.Rows)
        {
            if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
            {
                strbXml.Append(" <Details PO_Dtl_ID='");
                strbXml.Append(((Label)grvRow.FindControl("lblPO_dtl_ID1")).Text);
                strbXml.Append("' /> ");
            }
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    private void FunPriBindPopUpGrid()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@PO_HDR_ID", ddlPONo.SelectedValue);
        dictParam.Add("@PO_Number", ddlPONo.SelectedText);
        if (ddlFilterType.SelectedValue != "0")
            dictParam.Add("@Asset_Category", ddlFilterType.SelectedValue);
        if (txtFilterValue.Text != "")
            dictParam.Add("@Search_Text", txtFilterValue.Text.Trim());

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProAssGrdPageSizeRW;

        grvInvoices.BindGridView("S3G_Org_Get_Asset_Details", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucPopUpPaging.Visible = true;
        ucPopUpPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucPopUpPaging.setPageSize(ProAssGrdPageSizeRW);

        if (bIsNewRow)
            grvInvoices.Rows[0].Visible = false;

    }

    private void FunPriGetTempPIGridDetails()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@PO_HDR_ID", ddlPONo.SelectedValue);
            dictParam.Add("@PO_Number", ddlPONo.SelectedText);

            int intTotalRecords = 0;
            bool bIsNewRow = false;

            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW = ucCustomPaging.ProPageSizeRW;
            ObjPaging.ProUser_ID = intUserId;

            gvDlivery.BindGridView("S3G_Org_Get_VI_Temp", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);
            Cache["PIDetails"] = gvDlivery.DataSource;

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            dictParam.Add("@VI_Number", txtPINo.Text);
            dictParam.Add("@Option", "1");
            DataTable dtInvoice = new DataTable();
            dtInvoice = Utility.GetDefaultData("S3G_Org_Get_VI_Invoice_Grid", dictParam);
            gvInvoiceDet.DataSource = dtInvoice;
            gvInvoiceDet.DataBind();

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

    private void FunPriGetPIGridDetails(int intOption)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@VI_Hdr_ID", intPI_Hdr_ID.ToString());
            dictParam.Add("@Option", intOption.ToString());
            if (Session["VI_Number"] != null)
                dictParam.Add("@VI_Number", Session["VI_Number"].ToString());
            if (Session["PO_Number"] != null)
                dictParam.Add("@PO_Number", Session["PO_Number"].ToString());
            int intTotalRecords = 0;
            bool bIsNewRow = false;

            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProUser_ID = intUserId;

            gvDlivery.BindGridView("S3G_Org_Get_VI_Paging", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            Cache["PIDetails"] = gvDlivery.DataSource;

            DataTable dtInvoice = new DataTable();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@VI_Hdr_ID", intPI_Hdr_ID.ToString());
            dtInvoice = Utility.GetDefaultData("S3G_Org_Get_VI_Invoice_Grid", dictParam);
            gvInvoiceDet.DataSource = dtInvoice;
            gvInvoiceDet.DataBind();
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

    protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@PO_HDR_ID", ddlPONo.SelectedValue);
            dictParam.Add("@PO_Number", ddlPONo.SelectedText);
            dictParam.Add("@User_Id", intUserId.ToString());

            DataTable dtPODate = new DataTable();
            dtPODate = Utility.GetDefaultData("S3G_ORG_Get_PO_Date", dictParam);

            txtDate.Text = dtPODate.Rows[0]["PO_Date"].ToString();
            txtVendorName.Text = dtPODate.Rows[0]["Entity_Name"].ToString();

            btnInvoices.Enabled = true;

            gvDlivery.DataSource = null;
            gvDlivery.DataBind();
            grvInvoices.DataSource = null;
            grvInvoices.DataBind();
            gvInvoiceDet.DataSource = null;
            gvInvoiceDet.DataBind();
            ucCustomPaging.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    #region Role Access Setup/Page Load
    private void FunPriDisableControls(int intModeID)
    {
        Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                btnDICancel.Enabled = false;
                ucCustomPaging.Visible = false;
                btnInvoices.Enabled = false;
                txtPIDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtPIDate.ClientID + "','" + strDateFormat + "',true,  false);");
                break;

            case 1:
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    Response.Redirect(strRedirectPageView);
                }

                btnClear.Enabled = false;

                btnCancel.Enabled = true;
                FunPriGetPIDetails();
                FunPriGetPIGridDetails(1);
                btnGetLOV.Enabled = false;
                txtPIDate.ReadOnly = txtPINo.ReadOnly = txtDate.ReadOnly = true;
                CalendarExtenderPIDate.Enabled = false;
                if (bClearList)
                {
                    //ddlVendorCode.ClearDropDownList();
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
                btnInvoices.Enabled = false;
                btnDICancel.Visible = false;
                btnCancel.Enabled = true;
                txtDate.ReadOnly = txtPINo.ReadOnly = txtPIDate.ReadOnly = true;
                FunPriGetPIDetails();
                FunPriGetPIGridDetails(1);
                btnGetLOV.Enabled = false;
                CalendarExtenderPIDate.Enabled = false;
                gvDlivery.Columns[73].Visible = false;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (gvInvoiceDet.Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Add atleast one PO Item");
            return;
        }

        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            string strLSQ_No = string.Empty;

            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable ObjS3G_DeliveryInsDataTable = new LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable();
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow;
            ObjDeliveryInsRow = ObjS3G_DeliveryInsDataTable.NewS3G_LOANAD_InsertDeliveryInstructionRow();
            ObjDeliveryInsRow.Company_ID = intCompanyId;

            if (intPI_Hdr_ID > 0)
                ObjDeliveryInsRow.DeliveryInstruction_ID = intPI_Hdr_ID;
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
            ObjDeliveryInsRow.DeliveryInstruction_No = txtPINo.Text;
            ObjDeliveryInsRow.DeliveryInstruction_Date = Utility.StringToDate(txtPIDate.Text);
            ObjDeliveryInsRow.Created_By = intUserId;
            ObjDeliveryInsRow.Created_On = DateTime.Now;
            ObjS3G_DeliveryInsDataTable.AddS3G_LOANAD_InsertDeliveryInstructionRow(ObjDeliveryInsRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_DeliveryInsDataTable, SerMode);

            intErrCode = objLoanAdmin_MasterClient.FunPubSaveVI(out strLSQ_No, SerMode, ObjDeliveryInsDataTable);

            if (intErrCode == 0)
            {
                if (intPI_Hdr_ID > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Vendor Invoice Updated Successfully");
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
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strLSQ_No, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }
                        ShowWFAlertMessage(strLSQ_No, ProgramCode, strAlert);
                        return;
                    }
                    else
                    {
                        strAlert = "Vendor Invoice " + strLSQ_No + " details added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    }
                }
            }
            else if (intErrCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined for Vendor Invoice");
                strRedirectPageView = "";
            }
            else if (intErrCode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined for Vendor Invoice");
                strRedirectPageView = "";
            }
            else if (intErrCode == 50)
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to save the record");
                return;
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

    protected void btnInvoices_Click(object sender, EventArgs e)
    {
        FunPriBindPopUpGrid();
        ModalPopupExtenderApprover.Show();
    }

    protected void btnGoInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindPopUpGrid();
        }
        catch (Exception ex)
        {
            cvPopUP.ErrorMessage = "Due to Data Problem, Unable to Load the Invoices.";
            cvPopUP.IsValid = false;
        }
    }

    protected void btnDEVModalMove_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGrid();
            ModalPopupExtenderApprover.Hide();
        }
        catch (Exception ex)
        {
            cv_TabMainPage.ErrorMessage = "Unable to Move invoices.";
            cv_TabMainPage.IsValid = false;
        }
    }

    protected void btnDEVModalCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderApprover.Hide();
    }

    #endregion

    #region Other Button

    #region Cancel Button
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        Session["VI_Number"] = null;
        Session["PO_Number"] = null;
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect("../Origination/S3GORGPIVITransLander.aspx?Code=VNINV");
    }
    #endregion

    #region Cancel Delivery Instruction
    protected void btnDICancel_Click(object sender, EventArgs e)
    {
        int ErrorCode = 0;
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesClient();
        try
        {
            int intResult = objLoanAdmin_MasterClient.FunPubCancelVI(out ErrorCode, intPI_Hdr_ID, txtPINo.Text, ddlPONo.SelectedText);
            if (intResult == 0)
            {
                string strRedi = "../Origination/S3GORGPIVITransLander.aspx?Code=VNINV";
                Utility.FunShowAlertMsg(this.Page, "Vendor Invoice Cancelled successfully", strRedi);
                Session["VI_Number"] = null;
                Session["PO_Number"] = null;
                return;
            }
            if (intResult == 5)
            {
                Utility.FunShowAlertMsg(this.Page, "Vendor Invoice already mapped with RS, Unable to cancel.");
                btnDICancel.Enabled = false;
            }
            if (intResult == 6)
            {
                Utility.FunShowAlertMsg(this.Page, "Payment made for the selected invoice, hence unable to cancel it.");
                btnDICancel.Enabled = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvDelivery.ErrorMessage = "Unable to cancel Vendor Invoice";
            cvDelivery.IsValid = false;
        }
        finally
        {
            objLoanAdmin_MasterClient.Close();
        }
    }
    #endregion

    #endregion

    #region Clear

    protected void btnClear_Click(object sender, EventArgs e)
    {
        S3GCustomerAddress1.ClearCustomerDetails();

        if (bCreate)
            btnSave.Enabled = true;
        Clear();
    }

    private void Clear()
    {
        S3GCustomerAddress1.ClearCustomerDetails();
        ucCustomerCodeLov.FunPubClearControlValue();
        gvDlivery.DataSource = null;
        gvDlivery.DataBind();
        gvInvoiceDet.DataSource = null;
        gvInvoiceDet.DataBind();
        ddlPONo.Clear();
        btnInvoices.Enabled = false;
        txtDate.Text = txtPINo.Text = txtPIDate.Text = txtVendorName.Text = String.Empty;
        ucCustomPaging.Visible = false;
    }

    #endregion


    #endregion
#endregion

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
        if (hdnCustomerId != null)
        {
            if (hdnCustomerId.Value != "")
            {
                FunPubQueryExistCustomerListEnquiryUpdation(Convert.ToInt32(hdnCustomerId.Value));
                ddlPONo.ReadOnly = false;
            }
        }
    }

    private void FunPubQueryExistCustomerListEnquiryUpdation(int CustomerID)
    {

        dictParam = new Dictionary<string, string>();
        dictParam.Add("@CustomerID", CustomerID.ToString());
        dictParam.Add("@CompanyID", intCompanyId.ToString());
        DataSet dsCustomer = Utility.GetDataset("S3G_Get_Exist_Customer_Details_Enquiry_Updation", dictParam);
        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        txtName.Text = txtCustomerCode.Text = dsCustomer.Tables[0].Rows[0]["Customer_Code"].ToString();
        S3GCustomerAddress1.SetCustomerDetails(dsCustomer.Tables[0].Rows[0]["Customer_Code"].ToString(),
                dsCustomer.Tables[0].Rows[0]["comm_Address1"].ToString() + "\n" +
         dsCustomer.Tables[0].Rows[0]["comm_Address2"].ToString() + "\n" +
        dsCustomer.Tables[0].Rows[0]["comm_city"].ToString() + "\n" +
        dsCustomer.Tables[0].Rows[0]["comm_state"].ToString() + "\n" +
        dsCustomer.Tables[0].Rows[0]["comm_country"].ToString() + "\n" +
        dsCustomer.Tables[0].Rows[0]["comm_pincode"].ToString(), dsCustomer.Tables[0].Rows[0]["Customer_Name"].ToString(), dsCustomer.Tables[0].Rows[0]["Comm_Telephone"].ToString(),
        dsCustomer.Tables[0].Rows[0]["Comm_mobile"].ToString(),
        dsCustomer.Tables[0].Rows[0]["comm_email"].ToString(), dsCustomer.Tables[0].Rows[0]["comm_website"].ToString());
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPurchaseOrderNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Customer_ID", ((HiddenField)obj_Page.ucCustomerCodeLov.FindControl("hdnID")).Value);
        Procparam.Add("@SearchText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_LoadPONo_AGT", Procparam));
        return suggestions.ToArray();
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

}

