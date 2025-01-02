/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// <Program Summary>
/// Module Name               : Loan Admin
/// Screen Name               : Debit Credit Note
/// Created By                : Sathish R 
/// Created Date              : 6-Oct-2014
/// Purpose                   : Debit Credit Note Reqirement 
/// <Program Summary>
#region Header
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
using S3GBusEntity;
using System.Collections.Generic;
using System.Text;
//using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using S3GBusEntity.LoanAdmin;
using QRCoder;
using System.Drawing;
#endregion

public partial class S3GDebitCreditNote_Add : ApplyThemeForProject
{

    #region Initialisatin
    DataTable dtGLSLDetails = null;
    DataTable dtApprovalDetails;
    DataTable dtDimDetails;
    DataTable dtReceiving = null;
    int intCompanyId;
    HiddenField hdn_AccNature;
    string strDC_NO = string.Empty;
    DataSet ds;
    int intUserId;
    StringBuilder sbReceivingXML;
    public string strDateFormat;
    int intDebit_ID = 0;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bDelete = false;
    bool bQuery = false;
    string strMode = string.Empty;
    List<string> Lstparams = new List<string>();
    string DCNote_HeaderId = string.Empty;
    S3GSession ObjSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    string strDCNote_ID = string.Empty;
    string EntType = String.Empty;
    string strConnectionName;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GDebitCreditNote_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GDebitCreditNote_View.aspx?Code=DCN';";
    string strRedirectPage = "../LoanAdmin/S3GDebitCreditNote_View.aspx?Code=DCN";
    string strKey = "Insert";
    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjMJVClient;
    LoanAdminAccMgtServices.S3G_LOANAD_CRDRDTDataTable ObjMJVClientDataTable = null;
    int intErrCode = 0;
    static string strPageName = "Debit Credit Note";

    // Added By kalaivanan

    public static int? Funder_ID = null;
    public static int? Entity_ID = null;
    public static int? Customer_iD = null;
    public static int? Type_ID = null;
    //





    Dictionary<string, string> dictParam = null;
    StringBuilder sbBudget_XML = new StringBuilder();
    public static S3GDebitCreditNote_Add obj_Page;
    #endregion
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {



            obj_Page = this;
            FunPriPageLoad();
            if (ddlEntityType.SelectedValue == "1")//CUSTOMER
            {
                ucLov.strLOV_Code = "CMD";
            }
            else if (ddlEntityType.SelectedValue == "51")//ENTITY
            {
                ucLov.strLOV_Code = "ENVENDOR";
            }
            else if (ddlEntityType.SelectedValue == "13")//FUNDER
            {
                ucLov.strLOV_Code = "FLM";
            }
            else if (ddlEntityType.SelectedValue == "7") //Sundry Creditors
            {
                ucLov.strLOV_Code = "ENSUNDRY";
            }
            else if (ddlEntityType.SelectedValue == "50")//General
            {
                ucLov.strLOV_Code = "ENT";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    private void FunPriPageLoad()
    {
        try
        {

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjSession.ProDateFormatRW;
            CEDate.Format = strDateFormat;
            CalExtenderValuedate.Format = CalExtenderDueDate.Format = strDateFormat;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            strMode = "C";
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strMode = Request.QueryString.Get("qsMode");
                DCNote_HeaderId = fromTicket.Name;
            }
            if (Request.QueryString["Popup"] != null)
                btnBack.Enabled = false;
            if (!IsPostBack)
            {
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                btnCancel.Enabled = false;
                txtDate.Text = DateTime.Now.ToString(strDateFormat);
                txtDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDate.ClientID + "','" + strDateFormat + "',true,  false);");
                FunPriInsertReceiving(Lstparams);
                if (strMode == "C")
                {
                    txtCRDRValueDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCRDRValueDate.ClientID + "','" + strDateFormat + "',true,  false);");
                    txtDueDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDueDate.ClientID + "','" + strDateFormat + "',true,  false);");
                    FunPriGetLocaList();
                    FunPriLoadDocType();
                    FunPriLoadEntityType();
                    FunPubLoadLob();
                }
                if (Request.QueryString["qsMode"] == "Q")
                {
                    FunPubLoadLob();
                    FunPriLoadDocType();
                    FunPriLoadEntityType();
                    FunPriGetDCNote(strDCNote_ID);
                    FunPriDisableControls(-1);
                }
                else if (Request.QueryString["qsMode"] == "M")
                {
                    FunPubLoadLob();
                    FunPriLoadDocType();
                    FunPriLoadEntityType();
                    FunPriGetDCNote(strDCNote_ID);
                    FunPriDisableControls(1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
            }
            ucLov.strControlID = ucLov.ClientID;
            TextBox txtName = ucLov.FindControl("txtName") as TextBox;
            txtName.Attributes.Add("onfocus", "fnLoadEntity()");
            if (strMode == "M")
                txtName.Attributes.Remove("autopostback");
            if (ddlEntityType.SelectedIndex > 0)
                ucLov.ButtonEnabled = true;
            else
                ucLov.ButtonEnabled = false;
            if (ddlLocation.SelectedValue != "0")
                FunPriSetLOVCode();
            if (ddlLocation.SelectedValue != "0")
            {
                System.Web.HttpContext.Current.Session["Location_Id"] = intCompanyId.ToString();
            }
            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyId.ToString();
            if (ViewState["hdnEntityID"] != null)
            {
                System.Web.HttpContext.Current.Session["Customer_Id"] = ViewState["hdnEntityID"].ToString();
            }
            if (ddllob.SelectedValue != string.Empty)
                System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = ddllob.SelectedValue;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    #endregion
    # region Methods
    private void FunPriSetLOVCode()
    {
        try
        {
            if (ddlEntityType.SelectedIndex > 0)
            {
                if (ddlEntityType.SelectedValue == "1")//CUSTOMER
                {
                    ucLov.strLOV_Code = "CMD";
                }
                else if (ddlEntityType.SelectedValue == "51")//ENTITY
                {
                    ucLov.strLOV_Code = "ENVENDOR";
                }
                else if (ddlEntityType.SelectedValue == "13")//FUNDER
                {
                    ucLov.strLOV_Code = "FLM";
                }
                else if (ddlEntityType.SelectedValue == "7") //Sundry Creditors
                {
                    ucLov.strLOV_Code = "ENSUNDRY";
                }
                else if (ddlEntityType.SelectedValue == "50")//General
                {
                    ucLov.strLOV_Code = "ENT";
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    private void FunPriGetDCNote(string strDCNote_ID)
    {
        try
        {
            DataSet ds = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@DCNote_Tran_ID", DCNote_HeaderId);
            ds = Utility.GetDataset("S3G_DCN_GET_DCNOTE", dictParam);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddllob.SelectedValue = ds.Tables[0].Rows[0]["LOB_ID"].ToString();
                //ddllob.ClearDropDownList();
                ddlDocType.SelectedValue = ds.Tables[0].Rows[0]["Tran_Type"].ToString();
                //ddlDocType.ClearDropDownList();
                ddlEntityType.SelectedValue = ds.Tables[0].Rows[0]["ENTITY_TYPE"].ToString();
                //ddlEntityType.ClearDropDownList();
                ddlLocation.SelectedText = ds.Tables[0].Rows[0]["LocationCat_Description"].ToString();
                ddlLocation.SelectedValue = ds.Tables[0].Rows[0]["Location_ID"].ToString();
                //ListItem lstitem = new ListItem(ds.Tables[0].Rows[0]["Tran_Type_Desc"].ToString(), ds.Tables[0].Rows[0]["Tran_Type"].ToString());
                //ddlDocType.Items.Add(lstitem);
                txtDate.Text = ds.Tables[0].Rows[0]["Document_Date"].ToString();
                txtDocAmount.Text = ds.Tables[0].Rows[0]["Tran_Currency_Amount"].ToString();
                hfdcnstatus.Value = ds.Tables[0].Rows[0]["DOC_STAT"].ToString();
                txtNoteStatus.Text = ds.Tables[0].Rows[0]["Auth_Status"].ToString();
                txtDocInvoice.Text = ds.Tables[0].Rows[0]["Doc_Invoice_No"].ToString();
                txtNoteStatus.Text = ds.Tables[0].Rows[0]["Auth_Status"].ToString();
                taxDocInvStatus.Text = ds.Tables[0].Rows[0]["Invoice_Status"].ToString();
                txtCRDRValueDate.Text = ds.Tables[0].Rows[0]["VALUE_DATE"].ToString();
                txtDueDate.Text = ds.Tables[0].Rows[0]["Due_Date"].ToString();
                txtDocNumber.Text = ds.Tables[0].Rows[0]["DOCUMENT_NO"].ToString();
                txtMLASearch.SelectedText = ds.Tables[0].Rows[0]["ACCOUNT_CODE"].ToString();
                txtMLASearch.SelectedValue = ds.Tables[0].Rows[0]["pasa_id"].ToString();
                //FunPriLoadInvoiceNo();
                ddlInvoiceNo.SelectedValue = ds.Tables[0].Rows[0]["Invoice_No"].ToString();
                ddlInvoiceNo.SelectedText = ds.Tables[0].Rows[0]["Invoice_No"].ToString();

                txtRemarks.Text = ds.Tables[0].Rows[0]["REMARKS"].ToString();
                TextBox txtName = ucLov.FindControl("txtName") as TextBox;
                txtName.Text = ds.Tables[0].Rows[0]["ENTITY_NAME"].ToString();
                FunPriLoadEntityDtls(ds.Tables[0].Rows[0]["entity_id"].ToString());
                ViewState["DueTotal"] = ds.Tables[2].Rows[0]["Total_Due"].ToString();
                Button btnGetLOV = ucLov.FindControl("btnGetLOV") as Button;
                btnGetLOV.Enabled = false;
                (ucLov.FindControl("hdnID") as HiddenField).Value = ds.Tables[0].Rows[0]["entity_id"].ToString();

                ddlBillingState.SelectedText = ds.Tables[0].Rows[0]["Billing_State_Description"].ToString();
                ddlBillingState.SelectedValue = ds.Tables[0].Rows[0]["Billing_State_Id"].ToString();

                ddlDocSubType.SelectedValue = ds.Tables[0].Rows[0]["DocSubType"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                grvGLSLDetails.DataSource = ds.Tables[1];
                grvGLSLDetails.DataBind();
                ViewState["dtGLSLDetails"] = ds.Tables[1];
                ViewState["dsPrintDetails"] = ds;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
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
                // btnSave.Enabled = true;
                btn_print.Visible = false;
                break;
            case 1: // Modify Mode
                btn_print.Visible = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                if (bDelete)
                    btnCancel.Enabled = true;
                if (!bModify)
                {

                }
                btnSave.Enabled = true;
                //if (hfdcnstatus.Value != "1")
                // {
                Utility.ClearDropDownList(ddlDocType);
                Utility.ClearDropDownList(ddlDocSubType);

                ddlLocation.ReadOnly = true;
                Utility.ClearDropDownList(ddlEntityType);
                ucLov.ButtonEnabled = false;
                //txtMLASearch.ReadOnly = txtRemarks.ReadOnly = true;
                CalExtenderValuedate.Enabled = CalExtenderDueDate.Enabled = false;
                txtCRDRValueDate.ReadOnly = txtDueDate.ReadOnly = true;
                ddlInvoiceNo.ReadOnly = true;
                imgDate.Visible = false;
                imgInvoiceDate.Visible = imgDueDate.Visible = false;
                //commented and added WRF call id 22897
                ddlBillingState.ReadOnly = true;
                ddlBillingState.ReadOnly = false;
                //commented and added WRF call id 22897
                //}
                break;
            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                ddlLocation.ReadOnly = true;
                Utility.ClearDropDownList(ddlDocType);
                Utility.ClearDropDownList(ddlEntityType);
                Utility.ClearDropDownList(ddlDocSubType);
                txtCRDRValueDate.ReadOnly = txtDueDate.ReadOnly = true;
                ddlInvoiceNo.ReadOnly = true;
                ddlBillingState.ReadOnly = true;
                txtCode.Enabled = ucLov.ButtonEnabled = CEDate.Enabled = false;
                CalExtenderValuedate.Enabled = CalExtenderDueDate.Enabled = false;
                imgDate.Visible = false;
                txtDate.ReadOnly = true;
                grvGLSLDetails.Columns[grvGLSLDetails.Columns.Count - 1].Visible = false;
                grvGLSLDetails.FooterRow.Visible = false;
                txtMLASearch.ReadOnly = txtRemarks.ReadOnly = true;
                btnSave.Enabled = btnClear.Enabled = false;
                imgInvoiceDate.Visible = imgDueDate.Visible = false;
                imgDate.Visible = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage);
                }

                btn_print.Visible = true;
                break;
        }
    }

    private void FunPriGetLocaList()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            if (strMode == "C")
            {
                dictParam.Add("@Location_Active", "1");
                dictParam.Add("@Is_Operational", "1");
            }
            dictParam.Add("@Program_ID", "30");
            if (ddlLocation.SelectedValue != "0")
            {
                ddlEntityType.SelectedIndex = 0;
                TextBox txtName = ucLov.FindControl("txtName") as TextBox;
                txtName.Text = "";
                ucLov.ButtonEnabled = false;
                FunPriSetLOVCode();

                /* SEPTEMBER 2017 */
                //AjaxControlToolkit.ComboBox ddlglcode = grvGLSLDetails.FooterRow.FindControl("ddlFooterGLAccount") as AjaxControlToolkit.ComboBox;
                //FunPriLoadGLCode(ddlglcode);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadDocType()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", Convert.ToString(ObjUserInfo.ProCompanyIdRW));
            ddlDocType.BindDataTable("S3G_LAD_DCN_GET_DOCTYPE", Procparam, new string[] { "L_Id", "DESCRIPTION" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    private void FunPriLoadEntityDtls(string HdnId)
    {
        try
        {



            ViewState["hdnEntityID"] = HdnId;
            DataTable dtCustEntityDtls = new DataTable();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", intCompanyId.ToString());
            dictParam.Add("@TypeID", ddlEntityType.SelectedValue);//1-Customer, 2-Entity
            dictParam.Add("@ID", HdnId);
            dictParam.Add("@Cust_Address_ID", ddlBillingState.SelectedValue);
            dtCustEntityDtls = Utility.GetDefaultData("S3G_LAD_DCN_GET_ENTITY_DTLS", dictParam);
            if (dtCustEntityDtls.Rows.Count > 0)
            {
                ucFAAddressDetail.SetCustomerDetails(dtCustEntityDtls.Rows[0], true);
                Label lblCustomerCode = (Label)ucFAAddressDetail.FindControl("lblCustomerCode");
                lblCustomerCode.Text = "Customer Code/Name";
                TextBox txtName = (TextBox)ucLov.FindControl("txtName");
                txtName.Attributes.Add("width", "150");
                txtName.Text = txtCode.Text = dtCustEntityDtls.Rows[0]["CUSTOMER_CODE"].ToString();

                if (ddlEntityType.SelectedValue == "13")
                {
                    Funder_ID = Convert.ToInt32(dtCustEntityDtls.Rows[0]["FUNDER_ID"].ToString());
                }
                else if (ddlEntityType.SelectedValue == "1")
                {
                    Customer_iD = Convert.ToInt32(dtCustEntityDtls.Rows[0]["Customer_ID"].ToString());
                }
                else if (ddlEntityType.SelectedValue == "51")
                {
                    Entity_ID = Convert.ToInt32(dtCustEntityDtls.Rows[0]["Entity_ID"].ToString());
                }
                else if (ddlEntityType.SelectedValue == "50")
                {
                    Entity_ID = Convert.ToInt32(dtCustEntityDtls.Rows[0]["Entity_ID"].ToString());
                }

                Type_ID = Convert.ToInt16(ddlEntityType.SelectedValue);


                ViewState["Address"] = dtCustEntityDtls;
                ViewState["GL_Code"] = dtCustEntityDtls.Rows[0]["GL_Code"].ToString();
                ViewState["SL_Code"] = dtCustEntityDtls.Rows[0]["SL_Code"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadEntityType()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@LookupType_Code", "22");
            Procparam.Add("@IS_FROM_CRDRNOTE", "1");
            ddlEntityType.BindDataTable("S3G_LAD_DCN_GET_MJV_LOOKUP", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            dictParam = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    private void FunPriLoadEntityAddress(string StrEntityCode)
    {
        try
        {
            DataTable dt = new DataTable();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@Location_ID", ddlLocation.SelectedValue);
            dictParam.Add("@SL_Code", StrEntityCode);
            dt = Utility.GetDefaultData("FA_GET_ENTITY_CODE", dictParam);
            dictParam = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }


    private void FunPriLoadEditSLCode()
    {
        try
        {
            DataTable dt = new DataTable();
            UserControls_S3GAutoSuggest txtFooterGLAccount = (UserControls_S3GAutoSuggest)(grvGLSLDetails).FooterRow.FindControl("txtFooterGLAccount");
            UserControls_S3GAutoSuggest txtFooterSubGLAccount = (UserControls_S3GAutoSuggest)(grvGLSLDetails).FooterRow.FindControl("txtFooterSubGLAccount");
            HiddenField HdnId = ucLov.FindControl("hdnID") as HiddenField;
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@GL_Code", txtFooterGLAccount.SelectedValue);
            dictParam.Add("@Entity_ID", HdnId.Value.ToString());
            dt = Utility.GetDefaultData("S3G_GET_SL_DETAILS", dictParam);

            if (dt.Rows.Count > 0)
            {
                txtFooterSubGLAccount.SelectedValue = dt.Rows[0]["SL_Code"].ToString();
                txtFooterSubGLAccount.SelectedText = dt.Rows[0]["SL_Desc"].ToString();
            }
            else
            {
                txtFooterSubGLAccount.SelectedValue = "0";
                txtFooterSubGLAccount.SelectedText = "";

            }

            dictParam = null;
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
    private string FunPriReceivingXML()
    {
        try
        {
            sbReceivingXML = new StringBuilder();
            dtReceiving = (DataTable)ViewState["dtGLSLDetails"];
            sbReceivingXML.Append("<Root>");
            for (int dtRow = 0; dtRow < dtReceiving.Rows.Count; dtRow++)
            {
                sbReceivingXML.Append("<Details  Tran_Details_ID='" + dtReceiving.Rows[dtRow]["Tran_Details_ID"].ToString() + "' ");
                sbReceivingXML.Append("  GL_Code='" + dtReceiving.Rows[dtRow]["GL_Code"].ToString() + "' ");
                sbReceivingXML.Append("  SL_Code='" + dtReceiving.Rows[dtRow]["SL_Code"].ToString() + "' ");
                sbReceivingXML.Append("  Txn_Amount='" + dtReceiving.Rows[dtRow]["Amount"].ToString() + "' ");
                sbReceivingXML.Append("  Narration='" + dtReceiving.Rows[dtRow]["Narration"].ToString() + "' ");
                sbReceivingXML.Append("  CashFlow_flag_id='" + dtReceiving.Rows[dtRow]["CashFlow_Flag_Id"].ToString() + "' ");
                sbReceivingXML.Append("  ParentID ='" + dtReceiving.Rows[dtRow]["ParentID"].ToString() + "' ");
                sbReceivingXML.Append("  GSTRate ='" + dtReceiving.Rows[dtRow]["GSTRate"].ToString() + "' ");
                sbReceivingXML.Append("  Invoice_Id ='" + dtReceiving.Rows[dtRow]["Invoice_Id"].ToString() + "' ");
                sbReceivingXML.Append(" /> ");
            }

            sbReceivingXML.Append("</Root>");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
        return sbReceivingXML.ToString();
    }
    private void FunPriGetActivity()
    {
        try
        {

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            ddlActivity.BindDataTable("FA_SYS_GET_ACTIVITY", dictParam, new string[] { "ID", "DESCRIPTION" });
            if (ddlActivity != null && ddlActivity.Items.Count > 0 && ddlActivity.Items.Count == 2 && Convert.ToString(ddlActivity.Items[1].Value) == "-1")
            {
                ddlActivity.SelectedIndex = 1;
                ddlActivity.ClearDropDownList();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
    private void FunPriLoadDIM2(DropDownList ddlDim1, DropDownList ddlDim2)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@Program_ID", "30");
            dictParam.Add("@Is_Active", "1");
            if (ddlLocation.SelectedValue != "0")
                dictParam.Add("@location_id", ddlLocation.SelectedValue);
            if (ddlDim1.Items.Count > 0 && ddlDim1.SelectedItem.Text.Trim().ToLower() != "--select--")
                dictParam.Add("@DIM1_Code", ddlDim1.SelectedValue);
            ddlDim2.BindDataTable("FA_GET_DIM2", dictParam, new string[] { "Dim_Code", "Dim_Desc" });
            dictParam = null;
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriLoadCashFlow(AjaxControlToolkit.ComboBox ddlcashflow)
    {
        try
        {
            if (ddlcashflow.Items.Count > 0)
            {
                ddlcashflow.Items.Clear();
            }
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            ddlcashflow.BindDataTable("S3G_LAD_GET_CFList", Procparam, new string[] { "ID", "NAME" });
            if (ddlcashflow.Items.Count > 0)
            {
                ddlcashflow.Items.RemoveAt(0);
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlcashflow.Items.Insert(0, liSelect);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
    # endregion

    # region Grid Events

    protected void grvGLSLDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtAmount = (TextBox)e.Row.FindControl("txtFooterAmount");
                ImageButton imgbtn1 = (ImageButton)e.Row.FindControl("imgbtn");
                ImageButton imgbtn = (ImageButton)e.Row.FindControl("imgbtn1");
                Label lblTot = e.Row.FindControl("lblTot") as Label;
                TextBox txttotaldue = e.Row.FindControl("txttotaldue") as TextBox;
                if (ViewState["DueTotal"] != null)
                {
                    txttotaldue.Text = ViewState["DueTotal"].ToString();
                    txtDocAmount.Text = ViewState["DueTotal"].ToString();
                }
                txtAmount.SetDecimalPrefixSuffix(ObjSession.ProGpsPrefixRW, ObjSession.ProGpsSuffixRW, true, "Amount");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }


    protected void grvGLSLDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            // string strGLCode, strSLcode, stramount, strnaaration, strdim;
            if (e.CommandName == "Add")
            {
                HiddenField HdnId = ucLov.FindControl("hdnID") as HiddenField;
                if (HdnId.Value != string.Empty)
                {
                    ViewState["hdnEntityID"] = HdnId.Value;
                }
                DataRow dr;
                TextBox acc = ucFAAddressDetail.FindControl("txtHAccount") as TextBox;
                grvGLSLDetails.FooterRow.Visible = true;


                //if (ddlEntityType.SelectedValue != "50" && ddlEntityType.SelectedValue != "7") //General and Sundry Creditors
                //{
                //    if (txtMLASearch.SelectedValue == "-1" || txtMLASearch.SelectedValue == "0")
                //    {
                //        Utility.FunShowAlertMsg(this, "Select Rental Schedule No.");
                //        return;
                //    }
                //}

                UserControls_S3GAutoSuggest txtFooterCashflow = (UserControls_S3GAutoSuggest)grvGLSLDetails.FooterRow.FindControl("txtFooterCashflow");
                UserControls_S3GAutoSuggest txtFooterGLAccount = (UserControls_S3GAutoSuggest)grvGLSLDetails.FooterRow.FindControl("txtFooterGLAccount");

                if (txtFooterGLAccount.SelectedText == "")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select GL Account");
                    return;
                }


                if (txtFooterCashflow.SelectedText != "")
                {
                    DataTable dt = (DataTable)ViewState["dtGLSLDetails"];

                    int rowCount = dt.Select("CashFlow_Flag_Id = " + txtFooterCashflow.SelectedValue).Count();
                    if (rowCount > 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Selected Cashflow flag already added.");
                        return;
                    }

                }

                FunLoadSLCode();

                UserControls_S3GAutoSuggest txtFooterSubGLAccount = (UserControls_S3GAutoSuggest)grvGLSLDetails.FooterRow.FindControl("txtFooterSubGLAccount");

                if (txtFooterSubGLAccount.SelectedText == "")
                {
                    DataTable dt = (DataTable)ViewState["SLCODE"];
                    if (dt.Rows.Count > 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Select SL Code");
                        return;
                    }
                }

                if (txtFooterSubGLAccount.SelectedValue == "0" && txtFooterSubGLAccount.SelectedText != "")
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select valid Sub Account')", true);
                    return;

                }


                TextBox txtamount = grvGLSLDetails.FooterRow.FindControl("txtFooterAmount") as TextBox;
                TextBox txtnarration = grvGLSLDetails.FooterRow.FindControl("txtFooterNarration") as TextBox;

                if (txtamount.Text.ToString() == "")
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter the Amount");
                    return;
                }

                Lstparams.Add(txtFooterGLAccount.SelectedValue);
                Lstparams.Add(txtFooterGLAccount.SelectedText);
                Lstparams.Add(txtFooterSubGLAccount.SelectedValue);
                Lstparams.Add(txtFooterSubGLAccount.SelectedText);
                Lstparams.Add(Math.Round(Convert.ToDecimal(txtamount.Text)).ToString());
                Lstparams.Add(txtnarration.Text);
                Lstparams.Add(txtFooterCashflow.SelectedValue);
                Lstparams.Add(txtFooterCashflow.SelectedText);

                FunPriInsertReceiving(Lstparams);
                btnSave.Enabled = true;
                Pri_Arrive_Total();
            }
        }
        catch (Exception ex)
        {

            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void Pri_Arrive_Total()
    {
        TextBox txttotaldue = grvGLSLDetails.FooterRow.FindControl("txttotaldue") as TextBox;
        decimal decTemp = 0;
        for (int _Row = 0; _Row < grvGLSLDetails.Rows.Count; _Row++)
        {
            Label lblamount = grvGLSLDetails.Rows[_Row].FindControl("lblAmount") as Label;
            decTemp = decTemp + Convert.ToDecimal(lblamount.Text);
        }
        ViewState["DueTotal"] = decTemp;
        txtDocAmount.Text = Convert.ToString(decTemp);
        txttotaldue.Text = Convert.ToString(decTemp);
    }

    protected void grvGLSLDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Label lblamount = grvGLSLDetails.Rows[e.RowIndex].FindControl("lblAmount") as Label;
            Label parentID = grvGLSLDetails.Rows[e.RowIndex].FindControl("lblParentID") as Label;
            Label tranDetailsID = grvGLSLDetails.Rows[e.RowIndex].FindControl("lblTranDetailsID") as Label;

            /*
            if (ViewState["DueTotal"] != null)
            {
                ViewState["DueTotal"] = Convert.ToDecimal(ViewState["DueTotal"]) - Convert.ToDecimal(lblamount.Text);
            }
            */
            dtGLSLDetails = FunPriGetGLSlDetailsDataTable();

            //dtGLSLDetails.Rows.RemoveAt(e.RowIndex);



            if (parentID.Text == "0")
            {
                int ParentID = int.Parse(parentID.Text);
                int TranDetailsID = int.Parse(tranDetailsID.Text);


                DataRow[] drr = dtGLSLDetails.Select("ParentID = " + TranDetailsID);

                for (int i = 0; i < drr.Length; i++)
                {
                    ViewState["DueTotal"] = Convert.ToDecimal(ViewState["DueTotal"]) - Convert.ToDecimal(drr[i]["Amount"]);
                    drr[i].Delete();

                    dtGLSLDetails.AcceptChanges();
                }

                drr = dtGLSLDetails.Select("Tran_Details_ID = " + TranDetailsID);

                for (int i = 0; i < drr.Length; i++)
                {
                    ViewState["DueTotal"] = Convert.ToDecimal(ViewState["DueTotal"]) - Convert.ToDecimal(drr[i]["Amount"]);
                    drr[i].Delete();

                    dtGLSLDetails.AcceptChanges();
                }
            }
            else
            {
                int TranDetailsID = int.Parse(tranDetailsID.Text);
                DataRow[] drr = dtGLSLDetails.Select("Tran_Details_ID =" + TranDetailsID);

                for (int i = 0; i < drr.Length; i++)
                {
                    ViewState["DueTotal"] = Convert.ToDecimal(ViewState["DueTotal"]) - Convert.ToDecimal(drr[i]["Amount"]);
                    drr[i].Delete();

                    dtGLSLDetails.AcceptChanges();
                }
            }


            ViewState["dtGLSLDetails"] = dtGLSLDetails;

            dtGLSLDetails = FunPriGetGLSlDetailsDataTable();
            if (dtGLSLDetails.Rows.Count == 0)
            {
                FunPriInsertReceiving(Lstparams);
                btnSave.Enabled = false;
            }
            else
            {
                FunPubBindReceiving(dtGLSLDetails);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    # region Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddlInvoiceNo.SelectedText.Trim() == "" || ddlInvoiceNo.SelectedValue == "0")
            {

            }

            if (ddlInvoiceNo.SelectedText.Trim() != "" && ddlInvoiceNo.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Valid Invoice Number");
                return;
            }
            if (ddlBillingState.SelectedText.Trim() == "" || ddlBillingState.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Billing State");
                return;
            }

            if (ddlDocSubType.SelectedValue == "23" && txtRemarks.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Please Enter the Extension Period in Remarks");
                return;
            }

            if (Math.Round(Convert.ToDecimal(txtDocAmount.Text), 2) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Enter the valid Amount')", true);
                return;
            }

            string strHTML = string.Empty;
            if (ddlDocType.SelectedValue == "1")
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                    , Convert.ToInt32(ddlLocation.SelectedValue), 21, DCNote_HeaderId);
            else
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                    , Convert.ToInt32(ddlLocation.SelectedValue), 20, DCNote_HeaderId);

            if (strHTML == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined");
                return;
            }
            strHTML = null;

            HiddenField HdnId = ucLov.FindControl("hdnID") as HiddenField;
            if (HdnId.Value != string.Empty)
            {
                ViewState["hdnEntityID"] = HdnId.Value;
            }
            if (ViewState["hdnEntityID"] == null)
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Customer/Entity Name");
                return;
            }
            //if (ddlEntityType.SelectedValue != "50" && ddlEntityType.SelectedValue != "7")//General and Sundry Creditors
            //{
            //    if (txtMLASearch.SelectedValue == "-1" || txtMLASearch.SelectedValue == "0")
            //    {
            //        Utility.FunShowAlertMsg(this, "Select Rental Schedule No.");
            //        return;
            //    }
            //}
            dtGLSLDetails = (DataTable)ViewState["dtGLSLDetails"];
            if (Convert.ToInt32(dtGLSLDetails.Rows[0]["Tran_Details_ID"].ToString()) == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Enter atleast one row in Account Details Grid");
                return;
            }
            ObjMJVClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
            ObjMJVClientDataTable = new LoanAdminAccMgtServices.S3G_LOANAD_CRDRDTDataTable();
            LoanAdminAccMgtServices.S3G_LOANAD_CRDRDTRow objDCNoterow;
            string strDocNo = string.Empty;
            string strAlert = "alert('__ALERT__');";
            objDCNoterow = ObjMJVClientDataTable.NewS3G_LOANAD_CRDRDTRow();
            objDCNoterow.USRID = Convert.ToInt32(intUserId);
            if (ViewState["IS_DELETE"] != null)
            {
                objDCNoterow.IS_DELETE = Convert.ToInt32(ViewState["IS_DELETE"].ToString());
            }
            objDCNoterow.Mode = strMode;
            objDCNoterow.CompanyId = Convert.ToInt32(intCompanyId);
            objDCNoterow.Location_Id = Convert.ToInt32(ddlLocation.SelectedValue);
            objDCNoterow.Tran_Type = Convert.ToInt32(ddlDocType.SelectedValue);
            objDCNoterow.TranSubType = Convert.ToInt32(ddlDocSubType.SelectedValue);
            if (DCNote_HeaderId != string.Empty)
                objDCNoterow.DocumentNo = DCNote_HeaderId;//Use for Id
            else
                objDCNoterow.DocumentNo = "-1";
            objDCNoterow.Document_Date = Utility.StringToDate(Convert.ToString(txtDate.Text));
            objDCNoterow.Entity_Type = Convert.ToInt32(ddlEntityType.SelectedValue);
            objDCNoterow.Entity_Id = Convert.ToInt32(ViewState["hdnEntityID"].ToString());
            objDCNoterow.Entity_Code = txtCode.Text;
            objDCNoterow.Txn_Date = Utility.StringToDate(txtCRDRValueDate.Text);
            objDCNoterow.Billing_Address_ID = Convert.ToString(ddlBillingState.SelectedValue); // Added By kalaivanan
            objDCNoterow.TxnAmoun = Convert.ToDecimal(txtDocAmount.Text);
            string[] strPanum = txtMLASearch.SelectedText.Split('-');
            objDCNoterow.Account_Code = strPanum[0].ToString().Trim();
            objDCNoterow.Remarks = txtRemarks.Text;
            objDCNoterow.GL_Account = ViewState["GL_Code"].ToString();
            objDCNoterow.SL_Account = ViewState["SL_Code"].ToString();
            objDCNoterow.Lob_Id = ddllob.SelectedValue;
            objDCNoterow.XMlAccountDetails = FunPriReceivingXML();
            //if (ddlInvoiceNo.se)
            objDCNoterow.Invoice_No = ddlInvoiceNo.SelectedValue.ToString();

            objDCNoterow.Due_Date = Utility.StringToDate(txtDueDate.Text);

            ObjMJVClientDataTable.AddS3G_LOANAD_CRDRDTRow(objDCNoterow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDCNoteDataTable = ClsPubSerialize.Serialize(ObjMJVClientDataTable, SerMode);

            intErrCode = ObjMJVClient.FunPubInsertDebitCredit(out strDocNo, SerMode, ClsPubSerialize.Serialize(ObjMJVClientDataTable, SerMode));
            string strtranche = string.Empty;

            if (intErrCode == 0)
            {
                string Source_Path = "";
                string Traget_Path = "";
                string StrPath = string.Empty;
                string FileName = string.Empty;
                string strnewfile = "";
                string[] CRDRNO_OUT = strDocNo.Split('~');

                int count = CRDRNO_OUT.Count();
                if (count > 1)
                {
                    Source_Path = CRDRNO_OUT[1];
                    Traget_Path = CRDRNO_OUT[1];
                    strtranche = CRDRNO_OUT[3] + '_' + CRDRNO_OUT[4] + ".pdf";
                    strnewfile = CRDRNO_OUT[3] + '_' + CRDRNO_OUT[4] + ".pdf";
                    string[] strFilePath = Directory.GetFiles(Source_Path, "*" + strtranche);

                    if (strFilePath.Length > 0)
                    {
                        int i = 0;
                        foreach (string strRS in strFilePath)
                        {
                            FileInfo fl = new FileInfo(strRS);

                            if (fl.Exists == true)
                            {
                                if (!fl.Name.StartsWith("C_"))
                                {
                                    strtranche = fl.Name;
                                    Source_Path = strRS.Replace("\\", "/");
                                }
                                i = i + 1;
                            }
                        }
                        Traget_Path += "C" + "_" + CRDRNO_OUT[2] + "_" + strnewfile + "";
                        System.IO.File.Copy(Source_Path, Traget_Path, true);
                    }

                }
                if (strMode == "M")
                {
                    if (ddlDocType.SelectedValue.ToString() == "1")
                        //strAlert = "Debit Note " + strDocNo + " updated Successfully";
                        strAlert = "Debit Note " + CRDRNO_OUT[0] + " updated Successfully";
                    else
                        strAlert = "Debit Note " + CRDRNO_OUT[0] + " updated Successfully";

                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    btnSave.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                else
                {
                    if (ddlDocType.SelectedValue.ToString() == "1")
                        strAlert = "Credit Note " + CRDRNO_OUT[0] + " Added Successfully";
                    else
                        strAlert = "Debit Note " + CRDRNO_OUT[0] + " Added Successfully";

                    strAlert += @"\n" + " Woud you like to add detail Again?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    btnSave.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
            else if (intErrCode == -1)
            {
                Utility.FunShowAlertMsg(this, "Document Sequence Number not defined");
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this, "Transaction cannot be Cancelled");
                return;
            }
            else if (intErrCode == 3)
            {
                ViewState["IS_DELETE"] = null;
                if (ddlDocType.SelectedValue.ToString() == "1")
                    strAlert = strAlert.Replace("__ALERT__", "Credit Note Cancelled Successfully");
                else
                    strAlert = strAlert.Replace("__ALERT__", "Debit Note Cancelled Successfully");

                //strRedirectPageView = "";
                //btnSave.Enabled = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else if (intErrCode == 4)
            {
                Utility.FunShowAlertMsg(this, "Credit Note amount exceed the Old invoice amount.");
                return;
            }
            else if (intErrCode == 5)
            {
                Utility.FunShowAlertMsg(this, "Month is not open in GPS");
                return;
            }
            else if (intErrCode == 6)
            {
                Utility.FunShowAlertMsg(this, "Dr Note Date Should be greater than or equal to RS activation Date");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage);
        }
        catch (Exception objException)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtMLASearch.SelectedValue = "0";
            txtMLASearch.SelectedText = string.Empty;
            ddlInvoiceNo.SelectedValue = "0";
            ddlInvoiceNo.SelectedText = string.Empty;
            ddlBillingState.SelectedValue = "0";
            ddlBillingState.SelectedText = string.Empty;
            ucFAAddressDetail.ClearCustomerDetails();

            HiddenField HdnId = ucLov.FindControl("hdnID") as HiddenField;
            if (ViewState["hdnEntityID"] != null)
            {
                ViewState["hdnEntityID"] = null;
                //HdnId = null;
            }
            TextBox txtName = ucLov.FindControl("txtName") as TextBox;
            if (txtName.Text != string.Empty)
            {
                txtName.Text = "";
            }
            if (ddlEntityType.SelectedIndex > 0)
            {
                if (ddlEntityType.SelectedValue == "1")//CUSTOMER
                {
                    ucLov.strLOV_Code = "CMD";
                }
                else if (ddlEntityType.SelectedValue == "51")//ENTITY
                {
                    ucLov.strLOV_Code = "ENVENDOR";
                }
                else if (ddlEntityType.SelectedValue == "13")//FUNDER
                {
                    ucLov.strLOV_Code = "FLM";
                    System.Web.HttpContext.Current.Session["EntType"] = "13";
                }
                else if (ddlEntityType.SelectedValue == "7") //Sundry Creditors
                {
                    ucLov.strLOV_Code = "ENSUNDRY";
                }
                else if (ddlEntityType.SelectedValue == "50")//General
                {
                    ucLov.strLOV_Code = "ENT";
                }

            }

            if (ViewState["DueTotal"] != null)
                txtDocAmount.Text = ViewState["DueTotal"].ToString();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void btnCreateCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            HiddenField HdnId = ucLov.FindControl("hdnID") as HiddenField;

            if (ViewState["hdnEntityID"] != null)
            {
                System.Web.HttpContext.Current.Session["Customer_Id"] = HdnId.Value;
                if (ViewState["hdnEntityID"].ToString() == HdnId.Value)
                {
                    return;
                }
            }

            if (txtMLASearch.SelectedValue != string.Empty)
            {
                // HdnId.Value = null;
                txtMLASearch.SelectedValue = "0";
                txtMLASearch.SelectedText = string.Empty;

                ddlBillingState.SelectedValue = "-1";
                ddlBillingState.SelectedText = string.Empty;

                ViewState["DueTotal"] = null;
                txtDocAmount.Text = "";
            }


            if (strMode != "Q")
            {
                if (ddlLocation.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select Location");
                    ucLov.FunPubClearControlValue();
                    return;
                }
                if (ddlDocType.SelectedIndex > 0)
                {
                    if (ddlEntityType.SelectedValue == "1")
                    {
                        DataTable dt;
                        TextBox acc = ucFAAddressDetail.FindControl("txtHAccount") as TextBox;
                        dictParam = new Dictionary<string, string>();
                        dictParam.Add("@Company_ID", intCompanyId.ToString());

                        if (ddlDocType.SelectedValue == "1")
                        {
                            dictParam.Add("@ACC_LEG_ID", "2");
                        }
                        else
                        {
                            dictParam.Add("@ACC_LEG_ID", "1");
                        }

                        if (HdnId.Value != "" && HdnId.Value != null)
                        {
                            dictParam.Add("@id", HdnId.Value);
                        }
                    }
                }
                if (ddlEntityType.SelectedIndex > 0)
                {
                    if (HdnId.Value != "")
                    {
                        ViewState["hdnEntityID"] = HdnId.Value;
                        System.Web.HttpContext.Current.Session["Customer_Id"] = HdnId.Value;
                    }
                    if (ViewState["hdnEntityID"] != null)
                        FunPriLoadEntityDtls(ViewState["hdnEntityID"].ToString());
                }
                if (strMode == "C")
                {
                    ViewState["dtGLSLDetails"] = null;
                    ViewState["DueTotal"] = null;
                    FunPriInsertReceiving(Lstparams);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlLocation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlEntityType.SelectedIndex = 0;
            ddlDocType.SelectedIndex = 0;
            txtMLASearch.SelectedValue = "0";
            txtMLASearch.SelectedText = "";
            ddlInvoiceNo.SelectedValue = "0";
            ddlInvoiceNo.SelectedText = "";
            ddlBillingState.SelectedValue = "0";
            ddlBillingState.SelectedText = "";
            ucFAAddressDetail.ClearCustomerDetails();
            TextBox txtName = ucLov.FindControl("txtName") as TextBox;
            txtName.Text = "";
            ucLov.ButtonEnabled = false;
            if (ddlLocation.SelectedValue != "0")
                FunPriSetLOVCode();

            /* SEPTEMBER 2017 */
            //AjaxControlToolkit.ComboBox ddlglcode = grvGLSLDetails.FooterRow.FindControl("ddlFooterGLAccount") as AjaxControlToolkit.ComboBox;
            //FunPriLoadGLCode(ddlglcode);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void ddlDocType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtName = ucLov.FindControl("txtName") as TextBox;
            ddlBillingState.Clear();
            txtName.Text = "";
            ddlEntityType.SelectedValue = "0";

            //FunPriLoadEntityType();

            txtMLASearch.SelectedValue = "0";
            txtMLASearch.SelectedText = string.Empty;
            ddlInvoiceNo.SelectedValue = "0";
            ddlInvoiceNo.SelectedText = string.Empty;
            txtCode.Text = "";

            if (ddlDocType.SelectedValue == "1")
            {
                ddlDocSubType.SelectedIndex = 0;
                ddlDocSubType.Enabled = false;
            }
            else
            {
                ddlDocSubType.Enabled = true;
            }

            if (strMode == "C")
            {
                ViewState["dtGLSLDetails"] = null;
                FunPriInsertReceiving(Lstparams);
                dtGLSLDetails = (DataTable)ViewState["dtGLSLDetails"];

                /* SEPTEMBER 2017 */
                /*
                if (dtGLSLDetails.Rows.Count > 0 && Convert.ToString(dtGLSLDetails.Rows[0]["Tran_Details_ID"]) == "0")
                {
                    AjaxControlToolkit.ComboBox ddlglcode = grvGLSLDetails.FooterRow.FindControl("ddlFooterGLAccount") as AjaxControlToolkit.ComboBox;
                    FunPriLoadGLCode(ddlglcode);
                }*/
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void ddlGLCodeEdit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadEditSLCode();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void txtDate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (FunProValidateMonthLock())
            {
                Utility.FunShowAlertMsg(this.Page, "Month Locked For the Selected  Doc Date");
                return;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    # endregion

    #region Grid Datatable

    private void FunPriInsertReceiving(List<string> Lst)
    {
        try
        {
            DataRow dr;
            dtGLSLDetails = FunPriGetGLSlDetailsDataTable();
            if (dtGLSLDetails.Rows.Count < 1)
            {
                dr = dtGLSLDetails.NewRow();
                dr["Tran_Details_ID"] = "0";
                dr["GL_Code"] = "0";
                dr["SL_Code"] = "0";
                dr["Amount"] = "0";
                dr["Narration"] = "0";
                dr["CashFlow_Flag_Id"] = "0";
                dr["CashFlow_Flag_Name"] = "0";
                dr["ParentID"] = "0";
                dtGLSLDetails.Rows.Add(dr);
            }
            else
            {

                if (ddlLocation.SelectedText.ToString() == "")
                {
                    Utility.FunShowAlertMsg(this, "Select the valid Location.");
                    return;
                }

                if (ddlBillingState.SelectedText.ToString() == "")
                {
                    Utility.FunShowAlertMsg(this, "Select the valid Billing state.");
                    return;
                }

                DataTable dt = new DataTable();
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@CreditDebitType", ddlDocType.SelectedValue);
                dictParam.Add("@LocationID", ddlLocation.SelectedValue.ToString());
                dictParam.Add("@CashFlowFlagID", Lst[6].ToString());
                if (ddlBillingState.SelectedText.Contains(ddlLocation.SelectedText))
                    dictParam.Add("@Tax_Type", "13");
                else
                    dictParam.Add("@Tax_Type", "15");

                dt = Utility.GetDefaultData("S3G_GET_GLSLCODE", dictParam);

                if (Lst[6].ToString() != "500" && Lst[6].ToString() != "501" && Lst[6].ToString() != "502")
                {

                    if (dt.Rows[0]["Error_code"].ToString() != "0")
                    {
                        if (dt.Rows[0]["Error_code"].ToString() == "1")
                            Utility.FunShowAlertMsg(this, "Cashflow not defined for selected SGST.");
                        else if (dt.Rows[0]["Error_code"].ToString() == "2")
                            Utility.FunShowAlertMsg(this, "Cashflow not defined for selected CGST.");
                        else
                            Utility.FunShowAlertMsg(this, "Cashflow not defined for selected IGST.");

                        return;
                    }
                }

                dr = dtGLSLDetails.NewRow();

                int tranDetailsID = Convert.ToInt32(dtGLSLDetails.Rows[dtGLSLDetails.Rows.Count - 1]["Tran_Details_ID"]) + 1;

                //Code added by kalaivana 
                // getting this datatable dtGLSLDetails in reverse order finding last cashflow parent id
                string Tran_Details_ID = string.Empty;
                string CashFlow_Flag_Id = string.Empty;
                string parentid = "0";
                for (int i = dtGLSLDetails.Rows.Count - 1; i >= 0; i--)
                {
                    //string test = dtGLSLDetails.Columns["Tran_Details_ID"].ToString();
                    Tran_Details_ID = dtGLSLDetails.Rows[i]["Tran_Details_ID"].ToString();
                    CashFlow_Flag_Id = dtGLSLDetails.Rows[i]["CashFlow_Flag_Id"].ToString();

                    if (CashFlow_Flag_Id != "500" && CashFlow_Flag_Id != "501" && CashFlow_Flag_Id != "502")
                    {
                        parentid = Tran_Details_ID;
                        break;

                    }
                }


                dr["Tran_Details_ID"] = tranDetailsID;
                dr["GL_Code"] = Lst[0];
                dr["GL_Desc"] = Lst[1];
                dr["SL_Code"] = Lst[2];
                dr["SL_Desc"] = Lst[3];
                dr["Amount"] = Lst[4];
                dr["Narration"] = Lst[5];
                dr["CashFlow_Flag_Id"] = Lst[6];
                dr["CashFlow_Flag_Name"] = Lst[7];
                // dr["ParentID"] = "0";
                dr["ParentID"] = parentid;
                dtGLSLDetails.Rows.Add(dr);

                if ((Lst[7] != "" && Lst[6] != "") && (ddlEntityType.SelectedValue != "50" && ddlEntityType.SelectedValue != "7"))
                {
                    if (ddlBillingState.SelectedText.Contains(ddlLocation.SelectedText))
                    {
                        GSTAmountCalculation(13, Convert.ToDecimal(Lst[4]), tranDetailsID, Lst[6].ToString(), ddlBillingState.SelectedValue);
                        GSTAmountCalculation(14, Convert.ToDecimal(Lst[4]), tranDetailsID, Lst[6].ToString(), ddlBillingState.SelectedValue);
                    }
                    else
                    {
                        GSTAmountCalculation(15, Convert.ToDecimal(Lst[4]), tranDetailsID, Lst[6].ToString(), ddlBillingState.SelectedValue);
                    }
                }

            }
            if (dtGLSLDetails.Rows.Count > 1)
            {
                if (Convert.ToString(dtGLSLDetails.Rows[0]["Tran_Details_ID"]) == "0")
                {
                    dtGLSLDetails.Rows[0].Delete();
                }
            }
            ViewState["dtGLSLDetails"] = dtGLSLDetails;
            FunPriFillGrid();
            FunPubBindReceiving(dtGLSLDetails);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void GSTAmountCalculation(int taxTypeID, decimal amount, int parentID, string cashFlowID, string billingState)
    {
        try
        {
            DataRow dr;
            dtGLSLDetails = FunPriGetGLSlDetailsDataTable();
            if (dtGLSLDetails.Rows.Count < 1)
            {
                dr = dtGLSLDetails.NewRow();
                dr["Tran_Details_ID"] = "0";
                dr["Amount"] = "0";
                dr["Narration"] = "0";
                dtGLSLDetails.Rows.Add(dr);
            }
            else
            {

                dictParam = new Dictionary<string, string>();
                dictParam.Add("@TaxTypeID", taxTypeID.ToString());
                dictParam.Add("@StateID", ddlLocation.SelectedValue.ToString());
                dictParam.Add("@CASHFLOWID", cashFlowID);
                DataTable dt = new DataTable();
                dt = Utility.GetDefaultData("S3G_GET_GSTRATE", dictParam);
                decimal gstRate = 0;

                if (dt.Rows.Count > 0)
                    gstRate = Convert.ToDecimal(dt.Rows[0][0]);

                if (gstRate > 0)
                {
                    dictParam = new Dictionary<string, string>();
                    dictParam.Add("@CreditDebitType", ddlDocType.SelectedValue);
                    dictParam.Add("@LocationID", ddlLocation.SelectedValue.ToString());
                    dictParam.Add("@CashFlowFlagID", cashFlowID);

                    if (taxTypeID == 13)
                        dictParam.Add("@GSTCashFlowFlagID", "500");
                    else if (taxTypeID == 14)
                        dictParam.Add("@GSTCashFlowFlagID", "501");
                    else
                        dictParam.Add("@GSTCashFlowFlagID", "502");

                    dt = new DataTable();
                    dt = Utility.GetDefaultData("S3G_GET_GLSLCODE", dictParam);
                    dr = dtGLSLDetails.NewRow();

                    if (dt.Rows[0][0] == null)
                    {
                        Utility.FunShowAlertMsg(this, "GL Code not yet mapped for selected cashflow flag.");
                        return;
                    }
                    else
                    {
                        dr["GL_Code"] = dt.Rows[0][0];
                        dr["SL_Code"] = dt.Rows[0][1];
                        dr["GL_Desc"] = dt.Rows[0]["GL_Code_cr"];
                        dr["SL_Desc"] = dt.Rows[0]["SL_Code_cr"];
                        dr["CashFlow_Flag_Name"] = dt.Rows[0]["CashFlow_Description"];
                    }

                    if (taxTypeID == 13)
                        dr["CashFlow_Flag_Id"] = "500";
                    else if (taxTypeID == 14)
                        dr["CashFlow_Flag_Id"] = "501";
                    else
                        dr["CashFlow_Flag_Id"] = "502";

                    dr["Tran_Details_ID"] = Convert.ToInt32(dtGLSLDetails.Rows[dtGLSLDetails.Rows.Count - 1]["Tran_Details_ID"]) + 1;
                    dr["Amount"] = Math.Round(amount * gstRate / 100).ToString(Utility.SetSuffix());
                    //dr["Narration"] = "@" + gstRate.ToString(Utility.SetSuffix());// gstRate.ToString(ObjSession.ProGpsPrefixRW.ToString()); //(taxTypeID == 13 ? "CGST" : (taxTypeID == 14 ? "SGST" : "IGST"));
                    dr["ParentID"] = parentID;
                    dr["GSTRate"] = gstRate;
                    dr["Narration"] = dt.Rows[0]["CashFlow_Description"].ToString();
                    dtGLSLDetails.Rows.Add(dr);
                    ViewState["DueTotal"] = Convert.ToDecimal(ViewState["DueTotal"]) + Math.Round(amount * gstRate / 100);
                }
            }
            if (dtGLSLDetails.Rows.Count > 1)
            {
                if (Convert.ToString(dtGLSLDetails.Rows[0]["Tran_Details_ID"]) == "0")
                {
                    dtGLSLDetails.Rows[0].Delete();
                }
            }
            ViewState["dtGLSLDetails"] = dtGLSLDetails;
            FunPriFillGrid();
            FunPubBindReceiving(dtGLSLDetails);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private DataTable FunPriGetGLSlDetailsDataTable()
    {
        try
        {
            if (ViewState["dtGLSLDetails"] == null)
            {
                dtGLSLDetails = new DataTable();
                dtGLSLDetails.Columns.Add("Tran_Details_ID");
                dtGLSLDetails.Columns.Add("GL_Code");
                dtGLSLDetails.Columns.Add("GL_Desc");
                dtGLSLDetails.Columns.Add("SL_Code");
                dtGLSLDetails.Columns.Add("SL_Desc");
                dtGLSLDetails.Columns.Add("Amount");
                dtGLSLDetails.Columns.Add("Narration");
                dtGLSLDetails.Columns.Add("CashFlow_Flag_Id");
                dtGLSLDetails.Columns.Add("CashFlow_Flag_Name");
                dtGLSLDetails.Columns.Add("ParentID");
                dtGLSLDetails.Columns.Add("GSTRate");
                dtGLSLDetails.Columns.Add("Invoice_Id");

                ViewState["dtGLSLDetails"] = dtGLSLDetails;
            }
            dtGLSLDetails = (DataTable)ViewState["dtGLSLDetails"];
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
        return dtGLSLDetails;
    }

    private void FunPriFillGrid()
    {
        try
        {
            if (ViewState["dtGLSLDetails"] != null)
            {
                grvGLSLDetails.DataSource = (DataTable)ViewState["dtGLSLDetails"];
                grvGLSLDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPubBindReceiving(DataTable dtWorkflow)
    {
        try
        {
            grvGLSLDetails.DataSource = dtWorkflow;
            grvGLSLDetails.DataBind();
            Label lblTot = grvGLSLDetails.FooterRow.FindControl("lblTot") as Label;
            TextBox txttotaldue = grvGLSLDetails.FooterRow.FindControl("txttotaldue") as TextBox;
            if (dtWorkflow.Rows.Count > 0 && Convert.ToString(dtWorkflow.Rows[0]["Tran_Details_ID"]) == "0")
            {
                grvGLSLDetails.Rows[0].Visible = false;
                lblTot.Visible = txttotaldue.Visible = false;
            }
            else
            {
                lblTot.Visible = txttotaldue.Visible = true;
            }

            grvGLSLDetails.Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected bool FunProValidateMonthLock()
    {
        bool is_Lock = false;
        try
        {
            string strFinMonth = "";
            DateTime dt;
            dt = Utility.StringToDate(txtDate.Text);
            strFinMonth = dt.ToString("yyyy") + dt.ToString("MM");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Lock_Month", strFinMonth);
            if (Convert.ToInt32(ddlLocation.SelectedValue) > 1)
            {
                dictParam.Add("@Location_ID", ddlLocation.SelectedValue);
                DataTable dtIsLock = new DataTable();
                dtIsLock = Utility.GetDefaultData("S3G_LAD_DCN_Val_MonthLock", dictParam);
                is_Lock = Convert.ToBoolean(dtIsLock.Rows[0][0].ToString());
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {
        }
        return is_Lock;
    }
    # endregion

    private DataTable FunPriPmtHDR()
    {
        DataTable dt = new DataTable();
        try
        {
            DataRow drEmptyRow;
            dt.Columns.Add("activity");
            dt.Columns.Add("Company_id");
            dt.Columns.Add("Company_Name");
            dt.Columns.Add("company_address");
            dt.Columns.Add("Location");
            dt.Columns.Add("Username");
            dt.Columns.Add("Document_no");
            dt.Columns.Add("Document_date");
            dt.Columns.Add("batch");
            dt.Columns.Add("heading");
            drEmptyRow = dt.NewRow();
            drEmptyRow["activity"] = ddlActivity.SelectedValue;
            drEmptyRow["Company_id"] = ObjUserInfo.ProCompanyIdRW.ToString();
            drEmptyRow["Company_Name"] = ObjUserInfo.ProCompanyNameRW.ToString();
            drEmptyRow["company_address"] = FunGetCompAddress();
            drEmptyRow["Location"] = ddlLocation.SelectedText;
            drEmptyRow["Username"] = ObjUserInfo.ProUserNameRW.ToString();
            drEmptyRow["Document_no"] = txtDocNumber.Text;
            drEmptyRow["Document_date"] = txtDate.Text;
            drEmptyRow["batch"] = txtDocAmount.Text.Trim();
            if (ddlDocType.SelectedValue == "1")
                drEmptyRow["heading"] = "Credit Note";
            else
                drEmptyRow["heading"] = "Debit Note";
            dt.Rows.Add(drEmptyRow);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
        return dt;
    }
    private string FunGetCompAddress()
    {
        string strCompAddress = string.Empty;
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            DataSet DS = new DataSet();
            if (DS != null)
            {
                if (DS.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(DS.Tables[0].Rows[0]["Comu_Address"].ToString()))
                        strCompAddress = DS.Tables[0].Rows[0]["Comu_Address"].ToString();
                    if (strCompAddress.Contains("\r\n"))
                        strCompAddress = strCompAddress.Replace("\r\n", " ");
                }
            }
        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
        return strCompAddress;
    }
    private void FunPrintCoveringLetter()
    {
        try
        {
            string strHTML = string.Empty;
            if (ddlDocType.SelectedValue == "1")
            {
                if (Convert.ToInt32(DCNote_HeaderId) > 5064)
                {
                    if (ddlInvoiceNo.SelectedValue.Contains("/LAS/"))
                        strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                            , Convert.ToInt32(ddlLocation.SelectedValue), 65, DCNote_HeaderId);
                    else if (ddlInvoiceNo.SelectedValue.Contains("/BTN/"))
                        strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                            , Convert.ToInt32(ddlLocation.SelectedValue), 64, DCNote_HeaderId);
                    else
                        strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                            , Convert.ToInt32(ddlLocation.SelectedValue), 21, DCNote_HeaderId);
                }
                else
                {
                    strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                            , Convert.ToInt32(ddlLocation.SelectedValue), 72, DCNote_HeaderId);
                }
            }
            else if (ddlDocType.SelectedValue == "2" && ddlDocType.SelectedValue == "0")
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                    , Convert.ToInt32(ddlLocation.SelectedValue), 20, DCNote_HeaderId);
            else if (ddlDocType.SelectedValue == "2" && (ddlDocSubType.SelectedValue == "23" || ddlDocSubType.SelectedValue == "105"))
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                    , Convert.ToInt32(ddlLocation.SelectedValue), 12, DCNote_HeaderId);
            else if (ddlDocType.SelectedValue == "6")
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                   , Convert.ToInt32(ddlLocation.SelectedValue), 54, DCNote_HeaderId);
            else
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyId, Convert.ToInt32(ddllob.SelectedValue)
                    , Convert.ToInt32(ddlLocation.SelectedValue), 20, DCNote_HeaderId);

            if (strHTML == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined");
                return;
            }

            string FileName = PDFPageSetup.FunPubGetFileName(DCNote_HeaderId + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));

            if (ddlPrintType.SelectedValue == "P")
            {
                string FilePath = Server.MapPath(".") + "\\PDF Files\\";
                string DownFile = FilePath + FileName + ".pdf";
                SaveDocument(strHTML, DCNote_HeaderId, FilePath, FileName);
                if (!File.Exists(DownFile))
                {
                    Utility.FunShowAlertMsg(this, "File not exists");
                    return;
                }
                //Response.AppendHeader("content-disposition", "attachment; filename=DebitCreditNote.pdf");
                //Response.ContentType = "application/pdf";
                //Response.WriteFile(DownFile);


                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(DownFile, false, 0);
                string strScipt1 = "";
                if (DownFile.Contains("/File.pdf"))
                {
                    strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + DownFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                }
                else
                {
                    strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + DownFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);


            }
            else if (ddlPrintType.SelectedValue == "W")
            {
                string FilePath = Server.MapPath(".") + "\\PDF Files\\";
                SaveDocument(strHTML, DCNote_HeaderId, FilePath, FileName);
                string DownFile = FilePath + FileName + ".doc";
                if (!File.Exists(DownFile))
                {
                    Utility.FunShowAlertMsg(this, "File not exists");
                    return;
                }
                Response.AppendHeader("content-disposition", "attachment; filename=DebitCreditNote.doc");
                Response.ContentType = "application/vnd.ms-word";
                Response.WriteFile(DownFile);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void SaveDocument(string strHTML, string ReferenceNumber, string FilePath, string FileName)
    {
        try
        {
            int nDigi_Flag = 0;
            DataSet dsPrintDetails = new DataSet();

            if (dictParam != null)
                dictParam.Clear();
            else
                dictParam = new Dictionary<string, string>();

            dictParam.Add("@DCNoteHeaderId", ReferenceNumber);
            dsPrintDetails = Utility.GetDataset("S3G_LAD_GETPrint_DrCrnote", dictParam);

            GunfPubGetQrCode(dsPrintDetails.Tables[0].Rows[0]["QRCode"].ToString());

            string strQRCode = Server.MapPath(".") + "\\PDF Files\\DCQRCode.png";

            if (dsPrintDetails.Tables[0].Rows[0]["Digi_Sign_Enable"].ToString() == "1")
                nDigi_Flag = 1;

            DataRow[] ObjIGSTDR = dsPrintDetails.Tables[1].Select("InvTbl_IGST_Amount_Dbl > 0");


            if (ObjIGSTDR.Length > 0)
            {
                if (strHTML.Contains("~InvoiceTable~"))
                    strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

                if (strHTML.Contains("~InvoiceTable1~"))
                {
                    strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dsPrintDetails.Tables[1]/*Invoice Breakup*/);
                }
            }
            else
            {
                if (strHTML.Contains("~InvoiceTable1~"))
                    strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

                if (strHTML.Contains("~InvoiceTable~"))
                {
                    strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dsPrintDetails.Tables[1]/*Invoice Breakup*/);
                }
            }

            if (strHTML.Contains("~SACTable~"))
            {
                strHTML = PDFPageSetup.FunPubBindTable("~SACTable~", strHTML, dsPrintDetails.Tables[3]/*SAC*/);
            }

            strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsPrintDetails.Tables[2]/*HDR*/);

            strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsPrintDetails.Tables[0]/*HDR*/);

            List<string> listImageName = new List<string>();
            listImageName.Add("~CompanyLogo~");
            listImageName.Add("~InvoiceSignStamp~");
            listImageName.Add("~POSignStamp~");
            listImageName.Add("~DCQRCode~");
            List<string> listImagePath = new List<string>();

            //listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
            if (Convert.ToInt32(DCNote_HeaderId) > 15189)
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
            }
            else
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
            }

                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));
            listImagePath.Add(strQRCode);

            strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);

            string FooterNote = "";
            if (Convert.ToInt32(DCNote_HeaderId) > 15377)
            {
                FooterNote = "\nRegd. Office: Ground Floor, Block No B, 809, EGA Trade Centre, Poonamallee High Road, Kilpauk, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            }
            else
            {
                FooterNote = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069. ";
            }
            //PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);

            string SignedFile = "Signed" + intUserId.ToString();
            string DownFile = FilePath + FileName + ".pdf";

            if (nDigi_Flag == 1 && ddlPrintType.SelectedValue == "P")
            {
                FunPubSaveDocument(strHTML, FilePath, SignedFile, ddlPrintType.SelectedValue, FooterNote);
                S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                ObjPDFSign.DigiPDFSign(FilePath + SignedFile + ".pdf", DownFile, "RIGHT");
            }
            else
                FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue, FooterNote);


        }
        catch (Exception ex)
        {
            return;
        }
    }


    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, string FooterNote)
    {
        string strhtmlFile = FilePath + "\\" + FileName + ".html";
        string strwordFile = FilePath + "\\" + FileName + ".doc";
        string strpdfFile = FilePath + "\\" + FileName + ".pdf";
        object file = strhtmlFile;
        object oMissing = System.Reflection.Missing.Value;
        object readOnly = false;
        object oFalse = false;
        object fileFormat = null;

        Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
        Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            if (File.Exists(strwordFile) == true)
            {
                File.Delete(strwordFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            if (Utility.StringToDate(obj_Page.txtDate.Text) < Utility.StringToDate("30-Jun-2020"))
            {
                string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oDoc.ActiveWindow.Selection.Font.Size = 7;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(textDisc);
            }

            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" / ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            if (FooterNote != "")
            {
                oDoc.ActiveWindow.Selection.Font.Size = 9;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(FooterNote);
                oDoc.ActiveWindow.Selection.TypeText("      ");
            }
            else
            {
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
            }

            string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            if (DocumentType == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
            }

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);
        }
    }
    public static DataTable MergeTablesByIndex(DataTable t1, DataTable t2)
    {
        if (t1 == null || t2 == null) throw new ArgumentNullException("t1 or t2", "Both tables must not be null");

        DataTable t3 = t1.Clone();  // first add columns from table1
        foreach (DataColumn col in t2.Columns)
        {
            string newColumnName = col.ColumnName;
            int colNum = 1;
            while (t3.Columns.Contains(newColumnName))
            {
                newColumnName = string.Format("{0}_{1}", col.ColumnName, ++colNum);
            }
            t3.Columns.Add(newColumnName, col.DataType);
        }
        var mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(),
            (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
        foreach (object[] rowFields in mergedRows)
            t3.Rows.Add(rowFields);

        return t3;
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

    protected void btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            FunPrintCoveringLetter();

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected string FunProBudget_XML()
    {
        try
        {
            sbBudget_XML = new StringBuilder();
            sbBudget_XML.Append("<Root>");
            if (ddlDocType.SelectedValue == "2")
                sbBudget_XML = FunPro_XML((DataTable)ViewState["dtGLSLDetails"], 1);
            else
                sbBudget_XML = FunPro_XML((DataTable)ViewState["Address"], 2);
            sbBudget_XML.Append("</Root>");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
        return sbBudget_XML.ToString();
    }

    protected StringBuilder FunPro_XML(DataTable dtPaymentDetails, int intOption)
    {
        if (intOption == 1)
        {
            if (dtPaymentDetails.Rows.Count >= 1 && !string.IsNullOrEmpty(dtPaymentDetails.Rows[0]["Acc_Nature"].ToString()))
            {
                for (int dtRow = 0; dtRow < dtPaymentDetails.Rows.Count; dtRow++)
                {
                    if (dtPaymentDetails.Rows[dtRow]["Acc_Nature"].ToString() == "4")
                    {
                        sbBudget_XML.Append(" <Details GL_Code='" + dtPaymentDetails.Rows[dtRow]["GL_Code"].ToString() + "' ");
                        sbBudget_XML.Append("  GL_Desc='" + dtPaymentDetails.Rows[dtRow]["GL_Desc"].ToString() + "' ");
                        sbBudget_XML.Append("  SL_Code='" + dtPaymentDetails.Rows[dtRow]["SL_Code"].ToString() + "' ");
                        sbBudget_XML.Append("  SL_Desc='" + dtPaymentDetails.Rows[dtRow]["SL_Desc"].ToString() + "' ");
                        sbBudget_XML.Append("  Amount='" + dtPaymentDetails.Rows[dtRow]["Debit"].ToString() + "' ");
                        sbBudget_XML.Append(" /> ");
                    }
                }
            }
        }
        else if (intOption == 2)
        {
            if (dtPaymentDetails.Rows.Count > 0)
            {
                for (int dtRow = 0; dtRow < dtPaymentDetails.Rows.Count; dtRow++)
                {
                    if (dtPaymentDetails.Rows[dtRow]["Acc_Nature"].ToString() == "4")
                    {
                        sbBudget_XML.Append(" <Details GL_Code='" + dtPaymentDetails.Rows[dtRow]["GL_Code"].ToString() + "' ");
                        sbBudget_XML.Append("  GL_Desc='" + dtPaymentDetails.Rows[dtRow]["GL_Desc"].ToString() + "' ");
                        sbBudget_XML.Append("  SL_Code='" + dtPaymentDetails.Rows[dtRow]["Code"].ToString() + "' ");
                        sbBudget_XML.Append("  SL_Desc='" + dtPaymentDetails.Rows[dtRow]["SL_Desc"].ToString() + "' ");
                        sbBudget_XML.Append("  Amount='" + txtDocAmount.Text + "' ");

                        sbBudget_XML.Append(" /> ");
                    }
                }
            }
        }
        return sbBudget_XML;
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        UserInfo Ufo = new UserInfo();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(Ufo.ProCompanyIdRW));
        Procparam.Add("@User_ID", Convert.ToString(Ufo.ProUserIdRW));
        Procparam.Add("@Program_Id", "298");
        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
            Procparam.Add("@Lob_Id", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
        Procparam.Add("@PrefixText", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SA_DCN_GET_BRANCHLIST", Procparam));

        return suggetions.ToArray();
    }


    // public static List<string>BillingState(String prefixText)
    //{
    //    Dictionary<string, string> Procparam;
    //    Procparam = new Dictionary<string, string>();
    //    List<String> suggetions = new List<String>();
    //    DataTable dtCommon = new DataTable();
    //    DataSet Ds = new DataSet();
    //    UserInfo Ufo = new UserInfo();
    //    Procparam.Clear();


    //    Procparam.Add("@CUSTOMER_ID", Convert.ToString(Customer_iD));
    //    Procparam.Add("@FUNDER_ID", Convert.ToString(Funder_ID));
    //    Procparam.Add("@ENTITY_ID", Convert.ToString(Entity_ID));
    //    Procparam.Add("@Company_ID", Convert.ToString(Ufo.ProCompanyIdRW));
    //    Procparam.Add("@Type", Convert.ToString(Type_ID));
    //    Procparam.Add("@PREFIXTEXT", prefixText);



    //    //Procparam.Add("@User_ID", Convert.ToString(Ufo.ProUserIdRW));
    //    //Procparam.Add("@Program_Id", "298");
    //    //if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
    //    //    Procparam.Add("@Lob_Id", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
    //    //Procparam.Add("@PrefixText", prefixText);
    //    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SA_DCN_GET_BillingState", Procparam));
    //    return suggetions;
    //}


    [System.Web.Services.WebMethod]
    public static string[] GetBillingState(String prefixText, int count)
    {

        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        UserInfo Ufo = new UserInfo();
        Procparam.Clear();


        Procparam.Add("@CUSTOMER_ID", Convert.ToString(Customer_iD));
        Procparam.Add("@FUNDER_ID", Convert.ToString(Funder_ID));
        Procparam.Add("@ENTITY_ID", Convert.ToString(Entity_ID));
        Procparam.Add("@Company_ID", Convert.ToString(Ufo.ProCompanyIdRW));
        Procparam.Add("@Type", Convert.ToString(Type_ID));
        Procparam.Add("@PREFIXTEXT", prefixText);

        //Procparam.Add("@User_ID", Convert.ToString(Ufo.ProUserIdRW));
        //Procparam.Add("@Program_Id", "298");
        //if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
        //    Procparam.Add("@Lob_Id", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
        //Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SA_DCN_GET_BillingState", Procparam));
        return suggetions.ToArray();

    }


    protected void txtMLASearch_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            ddlInvoiceNo.SelectedValue = "0";
            ddlInvoiceNo.SelectedText = "";
            //FunPriLoadInvoiceNo();
            LoadBillingState();
            ViewState["DueTotal"] = null;

            ViewState["dtGLSLDetails"] = null;
            ViewState["DueTotal"] = null;


            FunPriInsertReceiving(Lstparams);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "ManualJournal");
        }
    }


    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        UserInfo Ufo = new UserInfo();
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@pasa_Ref_id", obj_Page.txtMLASearch.SelectedValue);
        Procparam.Add("@DOC_Type", obj_Page.ddlDocType.SelectedValue);
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_DCN_GET_INVDET", Procparam));
        //obj_Page.BindGrid_Invoice_Date(obj_Page.intCompanyId, Convert.ToInt16(obj_Page.txtMLASearch.SelectedValue), Convert.ToInt16(obj_Page.ddlDocType.SelectedValue), prefixText);
        return suggetions.ToArray();
    }


    //private void FunPriLoadInvoiceNo()
    //{
    //    try
    //    {
    //        ddlInvoiceNo.Items.Clear();
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
    //        Procparam.Add("@pasa_Ref_id", txtMLASearch.SelectedValue);
    //        Procparam.Add("@DOC_Type", ddlDocType.SelectedValue);
    //        ddlInvoiceNo.BindDataTable("S3G_DCN_GET_INVDET", Procparam, new string[] { "Invoice_No", "Invoice_No" });
    //        dictParam = null;
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //    }
    //}

    private void LoadBillingState()
    {
        try
        {

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@PASA_REF_ID", txtMLASearch.SelectedValue);

            ds = Utility.GetDataset("[S3G_GET_BILLINGSTATE]", Procparam);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBillingState.SelectedText = ds.Tables[0].Rows[0]["NAME"].ToString();
                ddlBillingState.SelectedValue = ds.Tables[0].Rows[0]["ID"].ToString();

                ddlBillingState.Enabled = false;
            }
            else
            {
                ddlBillingState.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetMLAList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] != null)
        {
            if (System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString() != "0")
                Procparam.Add("@LOB_ID", System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"].ToString());
        }
        Procparam.Add("@Company_ID", obj_Page.CompanyId.ToString());
        Procparam.Add("@Type", "1");
        Procparam.Add("@Is_Closed", "1");
        Procparam.Add("@CUSTOMER_ID", System.Web.HttpContext.Current.Session["Customer_Id"].ToString());
        Procparam.Add("@LOCATION_ID", obj_Page.ddlLocation.SelectedValue.ToString());
        Procparam.Add("@ParamPA_Status", "6,7,26,45,55");
        Procparam.Add("@ParamSA_Status", "6,7,26,45,55");
        Procparam.Add("@PrefixText", prefixText);
        if (System.Web.HttpContext.Current.Session["EntType"] != null)
            Procparam.Add("@ENT_TYPE", System.Web.HttpContext.Current.Session["EntType"].ToString());
        else
            Procparam.Add("@ENT_TYPE", "");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_DCN_GET_PANUM", Procparam));
        return suggetions.ToArray();

    }

    public void FunPubLoadLob()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@User_Id", intUserId.ToString());
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Consitution_Id", "");
        ddllob.BindDataTable("S3G_LAD_DCN_GetLOB", Procparam, new string[] { "LOB_ID", "LOB_Name" });

        if (ddllob.Items.Count > 0)
        {
            if (ddllob.Items.Count == 2)
                ddllob.SelectedIndex = 1;
            btnClear.Enabled = true;
            ddllob.Items.RemoveAt(0);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //txtDate.Clear();
            ViewState["dtGLSLDetails"] = null;
            FunPriInsertReceiving(Lstparams);
            btnSave.Enabled = false;
            ddllob.SelectedIndex = 0;
            ddlLocation.SelectedValue = "-1";
            ddlLocation.SelectedText = string.Empty;
            ddlDocType.SelectedIndex = 0;
            ddlDocSubType.SelectedIndex = 0;
            ddlEntityType.SelectedIndex = 0;
            txtDocAmount.Text = string.Empty;
            TextBox txtName = ucLov.FindControl("txtName") as TextBox;
            txtName.Text = string.Empty;
            txtMLASearch.SelectedValue = "0";
            txtMLASearch.SelectedText = string.Empty;
            //txtDate.Text = string.Empty;
            txtCRDRValueDate.Text = txtDueDate.Text = string.Empty;
            ucFAAddressDetail.ClearCustomerDetails();
            ddlBillingState.SelectedValue = "-1";
            ddlBillingState.SelectedText = string.Empty;

            ddlInvoiceNo.SelectedValue = "-1";
            ddlInvoiceNo.SelectedText = string.Empty;
            //btnClear.Enabled = false;

            ViewState["DueTotal"] = null;
            ViewState["SLCODE"] = null;
            ViewState["dtGLSLDetails"] = null;

            ddlBillingState.Enabled = true;
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["IS_DELETE"] = "1";
            btnSave_Click(sender, e);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Delete Credit Debit Note");
        }
        finally
        {

        }
    }

    #region UnWanted

    private void FunPriLoadGLCode(AjaxControlToolkit.ComboBox ddlglcode)
    {
        try
        {
            if (ddlglcode.Items.Count > 0)
            {
                ddlglcode.Items.Clear();
            }
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            ddlglcode.BindDataTable("S3G_LAD_DCN_GET_GL_LST", Procparam, new string[] { "ID", "NAME" });
            if (ddlglcode.Items.Count > 0)
            {
                ddlglcode.Items.RemoveAt(0);
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlglcode.Items.Insert(0, liSelect);
            }
            if (ddlglcode.SelectedValue != "0")
                FunPriLoadSLCode();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadSLCode()
    {
        try
        {
            /* SEPTEMBER 2017 */
            //AjaxControlToolkit.ComboBox ddglcode = grvGLSLDetails.FooterRow.FindControl("ddlFooterGLAccount") as AjaxControlToolkit.ComboBox;

            AjaxControlToolkit.ComboBox ddlslcode = grvGLSLDetails.FooterRow.FindControl("ddlFooterSubAccount") as AjaxControlToolkit.ComboBox;
            HiddenField hdn_AccNature = (HiddenField)(grvGLSLDetails).FooterRow.FindControl("hdn_AccNature") as HiddenField;
            ddlslcode.Enabled = true;
            if (ddlslcode.Items.Count > 0)
            {
                ddlslcode.Items.RemoveAt(0);
                ddlslcode.Items.Clear();
            }
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            //dictParam.Add("@GL_Code", ddglcode.SelectedValue);
            ds = Utility.GetDataset("[S3G_LAD_DCN_GET_SL_DTL]", dictParam);
            ddlslcode.DataValueField = "ID";
            ddlslcode.DataTextField = "NAME";
            ddlslcode.DataSource = ds.Tables[0];
            ViewState["SLCODE"] = ds.Tables[0];
            ddlslcode.DataBind();
            if (ddlslcode.Items.Count > 0)
            {
                ddlslcode.Items.RemoveAt(0);
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlslcode.Items.Insert(0, liSelect);
            }
            if (ddlslcode.Items.Count == 2)
                ddlslcode.SelectedIndex = 1;
            dictParam = null;
            //ddlslcode.ClearDropDownList();
            //if (ds.Tables[1].Rows.Count > 0)
            //{

            //    hdn_AccNature.Value = Convert.ToString(ds.Tables[1].Rows[0][1]);
            //}
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlFooterGLAccount_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadSLCode();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void lnk_Click(object sender, EventArgs e)
    {
        try
        {
            ImageButton lnk = (ImageButton)sender;
            GridViewRow grvRow = (GridViewRow)lnk.Parent.Parent;
            Label lbldim1 = (Label)grvRow.FindControl("lbldim1");
            Label lbldim2 = (Label)grvRow.FindControl("lbldim2");
            DropDownList ddlDim1 = (DropDownList)grvRow.FindControl("ddlDim1");
            DropDownList ddlDim2 = (DropDownList)grvRow.FindControl("ddlDim2");
            HiddenField hdn_Dim1 = (HiddenField)grvRow.FindControl("hdn_Dim1");
            HiddenField hdn_Dim2 = (HiddenField)grvRow.FindControl("hdn_Dim2");
            hdn_Dim1.Value = ddlDim1.SelectedValue;
            hdn_Dim2.Value = ddlDim2.SelectedValue;
            ImageButton imgbtn = (ImageButton)grvRow.FindControl("imgbtn");
            lbldim1.Visible = lbldim2.Visible = ddlDim1.Visible = ddlDim2.Visible = lnk.Visible = false;
            imgbtn.Visible = true;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void lnk_Click1(object sender, EventArgs e)
    {
        ImageButton lnk = (ImageButton)sender;

        GridViewRow grvRow = (GridViewRow)lnk.Parent.Parent;
        ImageButton imgbtn = (ImageButton)grvRow.FindControl("imgbtn1");
        Label lblDIM = (Label)grvGLSLDetails.HeaderRow.FindControl("lblDIM");
        lnk.Visible = false;
        imgbtn.Visible = lblDIM.Visible = true;

    }
    #endregion

    /* SEPTEMBER 2017 */

    protected void ddlBillingState_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["dtGLSLDetails"] = null;
            FunPriInsertReceiving(Lstparams);

            HiddenField HdnId = ucLov.FindControl("hdnID") as HiddenField;
            if (HdnId.Value != string.Empty)
            {
                FunPriLoadEntityDtls(HdnId.Value);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException, strPageName);
        }
    }


    [System.Web.Services.WebMethod]
    public static string[] GetCashflow(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_CASHFLOW_AGT", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetGLAccount(String prefixText, int count)
    {
        string companyId = System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString();

        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", companyId);
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_GLACCOUNT_AGT", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSubGLAccount(String prefixText, int count)
    {
        string companyId = System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString();

        UserControls_S3GAutoSuggest txtFooterGLAccount = (UserControls_S3GAutoSuggest)obj_Page.grvGLSLDetails.FooterRow.FindControl("txtFooterGLAccount");

        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", companyId);
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@GL_CODE", txtFooterGLAccount.SelectedValue);

        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_SUBGLACCOUNT_AGT", Procparam));
        return suggestions.ToArray();
    }

    private void FunLoadSLCode()
    {
        try
        {

            UserControls_S3GAutoSuggest txtFooterGLAccount = (UserControls_S3GAutoSuggest)obj_Page.grvGLSLDetails.FooterRow.FindControl("txtFooterGLAccount");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@GL_Code", txtFooterGLAccount.SelectedValue);
            ds = Utility.GetDataset("[S3G_GET_SUBGLACCOUNT_AGT]", dictParam);
            ViewState["SLCODE"] = ds.Tables[0];
            dictParam = null;

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    public void BindGrid_Invoice_Details()
    {


        ViewState["DueTotal"] = null;
        Dictionary<string, string> procparam = new Dictionary<string, string>();
        procparam.Add("@COMPANY_ID", obj_Page.intCompanyId.ToString());
        procparam.Add("@pasa_Ref_id", txtMLASearch.SelectedValue.ToString());
        procparam.Add("@Invoice_Number", ddlInvoiceNo.SelectedText.Trim());
        procparam.Add("@DOC_Type", ddlDocType.SelectedValue.ToString());

        DataSet ds = Utility.GetDataset("S3G_DCN_GET_Invoice_Det", procparam);
        grvGLSLDetails.DataSource = ds.Tables[0];
        ViewState["dtGLSLDetails"] = ds.Tables[0];
        grvGLSLDetails.Visible = true;
        grvGLSLDetails.DataBind();
        btnSave.Enabled = true;
        if (ds.Tables[1].Rows.Count > 0)
        {
            ddlBillingState.SelectedText = ds.Tables[1].Rows[0]["Bill_Address_GSTIN"].ToString();
            ddlBillingState.SelectedValue = ds.Tables[1].Rows[0]["Bill_Address_ID"].ToString();
        }
        else
        {
            ddlBillingState.SelectedText = "";
            ddlBillingState.SelectedValue = "0";
        }

        Int32 IntGrdRow = grvGLSLDetails.Rows.Count;

        for (Int32 I_Row = 0; I_Row < IntGrdRow; I_Row++)
        {
            Label Lblamount = grvGLSLDetails.Rows[I_Row].FindControl("lblAmount") as Label;
            decimal decTemp = 0;
            if (Lblamount.Text != "")
            {
                if (ViewState["DueTotal"] == null)
                    decTemp = Convert.ToDecimal(Lblamount.Text);
                else
                    decTemp = Convert.ToDecimal(ViewState["DueTotal"]) + Convert.ToDecimal(Lblamount.Text);
            }
            ViewState["DueTotal"] = decTemp;
        }
        TextBox txtamount = grvGLSLDetails.FooterRow.FindControl("txttotaldue") as TextBox;

        txtamount.Text = ViewState["DueTotal"].ToString();
        txtDocAmount.Text = ViewState["DueTotal"].ToString();
    }

    protected void ddlInvoiceNo_Item_Selected(object Sender, EventArgs e)
    {

        if (ddlDocType.SelectedValue.ToString() == "1" && ddlInvoiceNo.SelectedValue.ToString() != "")
        {
            BindGrid_Invoice_Details();

        }

        if (ddlInvoiceNo.SelectedValue.ToString() == "")
            ddlInvoiceNo.SelectedText = "";

    }


    protected void txtFooterCashflow_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            //int? cashflowid = 607;
            string cashflowid = string.Empty;
            string cashFlowDesc = string.Empty;

            if (cashflowid == "")
            {
                DataTable dt1 = new DataTable();
                UserControls_S3GAutoSuggest txtFooterCashflow = (UserControls_S3GAutoSuggest)(grvGLSLDetails).FooterRow.FindControl("txtFooterCashflow");
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Prefix", Convert.ToString(txtFooterCashflow.SelectedText));
                dt1 = Utility.GetDefaultData("S3G_ORG_GetCashflowID", dictParam);

                if (dt1.Rows.Count > 0)
                {
                    cashflowid = dt1.Rows[0]["CashFlow_ID"].ToString();
                    cashFlowDesc = dt1.Rows[0]["CashFlowFlag_Desc"].ToString();
                }

            }

            //txtFooterCashflow


            DataTable dt = new DataTable();
            UserControls_S3GAutoSuggest txtFooterGLAccount = (UserControls_S3GAutoSuggest)(grvGLSLDetails).FooterRow.FindControl("txtFooterGLAccount");
            UserControls_S3GAutoSuggest txtFooterSubGLAccount = (UserControls_S3GAutoSuggest)(grvGLSLDetails).FooterRow.FindControl("txtFooterSubGLAccount");

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@CashFlow_ID", Convert.ToString(cashflowid));
            dictParam.Add("@LocationID", null);
            dictParam.Add("@Program_ID", "298");
            dictParam.Add("@Type", Convert.ToString(ddlDocType.SelectedItem));
            dt = Utility.GetDefaultData("S3G_GET_CASHFLOW_GLSLCODE", dictParam);

            if (dt.Rows.Count > 0)
            {
                txtFooterGLAccount.SelectedValue = dt.Rows[0]["GL_Code"].ToString();
                txtFooterGLAccount.SelectedText = dt.Rows[0]["Account_Name"].ToString();
                txtFooterSubGLAccount.SelectedValue = dt.Rows[0]["SL_Code"].ToString();
                txtFooterSubGLAccount.SelectedText = dt.Rows[0]["SubAccount_Name"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }

    }
    private void GunfPubGetQrCode(string ReferenceNumber)
    {
        try
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ReferenceNumber, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);

            qrCodeImage.Save(Server.MapPath(".") + "\\PDF Files\\DCQRCode.png");
        }
        catch (Exception ex)
        {

        }
    }
}
