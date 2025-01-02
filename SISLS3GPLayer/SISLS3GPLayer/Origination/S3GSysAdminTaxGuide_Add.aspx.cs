///Module Name      :   System Admin
///Screen Name      :   S3GSysAdminTaxGuide_Add.aspx
///Created By       :   Vinodha.M
///Created Date     :   17-Sep-2014
///Purpose          :   To insert and update Tax Guide details

using System;
using System.ServiceModel;
using System.Web.UI;
using S3GBusEntity;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class S3GSysAdminTaxGuide_Add : ApplyThemeForProject
{
    #region Intialization

    AccountMgtServicesReference.AccountMgtServicesClient ObjTaxGuideMasterClient;
    AccountMgtServices.S3G_SYSAD_TaxGuideDataTable ObjS3G_SYSAD_TaxGuideDataTable = new AccountMgtServices.S3G_SYSAD_TaxGuideDataTable();
    string strDateFormat = string.Empty;
    Dictionary<string, string> Procparam = null;
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;
    int intTaxGuideId = 0;
    int intUserId = 0;
    int intCompanyID = 0;
    bool bClearList = false;
    string strMode = string.Empty;
    bool checkvalue = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;
    DataSet dsTaxDetails, dsInitTaxTypeDetails;
    DataSet dsInitLoadDetails;
    Dictionary<string, string> dictParam;
    string strProcName = null;
    string strXMLTaxAssetDet = null;
    string strxmlHSN = null;
    StringBuilder strbTaxAssetDet = new StringBuilder();
    StringBuilder strbHSNDet = new StringBuilder();
    DataTable dtTaxAsset = new DataTable();
    DataTable dtTaxAssetHistory = new DataTable();
    DataTable dtHSN = new DataTable();
    DataTable dtservices = new DataTable();
    DataTable dtTaxAssetGr;
    string strRedirectPage = "../Origination/S3GSysAdminTaxGuide_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GSysAdminTaxGuide_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3GSysAdminTaxGuide_View.aspx';";
    string state = String.Empty;
    string RevChargeZone = String.Empty;
    int taxclass = 0, taxtype = 0, Asset_Type_ID = 0;
    decimal AdditionalTax = 0, OtherTax = 0, Rate, totaltax;
    string Asset_Type;
    string Asset_Type_Name;
    //Asset_Category_ID = 0;

    public static S3GSysAdminTaxGuide_Add ojb_TransLander = null;

    PagingValues ObjPaging = new PagingValues();

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public int ProPageNumRW
    {
        get;
        set;
    }

    public int ProPageSizeRW
    {
        get;
        set;
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGrid();
    }
    #endregion

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ojb_TransLander = this;

            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender1.Format = strDateFormat;

            #region Paging Config

            ProPageNumRW = 1;
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

            #endregion

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

            txtRate.Attributes.Add("onblur", "funChkDecimial_OPC(this," + 3 + "," + 8 + ",'Rate %')");

            //txtTax.Attributes.Add("onblur", "funChkDecimial_OPC(this," + 3 + "," + 8 + ",'Tax %')");
            //txtsurcharge.Attributes.Add("onblur", "funChkDecimial_OPC(this," + 3 + "," + 8 + ",'Surcharge %')");
            //txtCess.Attributes.Add("onblur", "funChkDecimial_OPC(this," + 3 + "," + 8 + ",'Cess %')");
            //txtEduCess.Attributes.Add("onblur", "funChkDecimial_OPC(this," + 3 + "," + 8 + ",'Educational Cess %')");
            //txtAdditionalTax.Attributes.Add("onblur", "funChkDecimial_OPC(this," + 3 + "," + 8 + ",'Additional Tax %')");
            //txtOtherTaxRate.Attributes.Add("onblur", "funChkDecimial_OPC(this," + 3 + "," + 8 + ",'Other Tax Rate %')");

            txtTax.Attributes.Add("onblur", "return funChkDecimial(this,3,8,'" + "Tax %" + "',false);");
            txtsurcharge.Attributes.Add("onblur", "return funChkDecimial(this,3,8,'" + "Surcharge %" + "',false);");
            txtCess.Attributes.Add("onblur", "return funChkDecimial(this,3,8,'" + "Cess %" + "',false);");
            txtEduCess.Attributes.Add("onblur", "return funChkDecimial(this,3,8,'" + "Educational Cess %" + "',false);");
            txtAdditionalTax.Attributes.Add("onblur", "return funChkDecimial(this,3,8,'" + "Additional Tax %" + "',false);");
            txtOtherTaxRate.Attributes.Add("onblur", "return funChkDecimial(this,3,8,'" + "Other Tax Rate %" + "',false);");

            txttotaltax.Attributes.Add("ReadOnly", "ReadOnly");
            txtRate.Attributes.Add("ReadOnly", "ReadOnly");
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            if (Request.QueryString["qsTaxCodeId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsTaxCodeId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intTaxGuideId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            txtEffFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtEffFrom.ClientID + "','" + strDateFormat + "',false,  false);");
            /*Added by vinodha.m on sep3,2015 to calculate total tax when postback event is happening*/
            AdditionalTax = Convert.ToDecimal((txtAdditionalTax.Text != String.Empty ? txtAdditionalTax.Text : "0"));
            OtherTax = Convert.ToDecimal((txtOtherTaxRate.Text != String.Empty ? txtOtherTaxRate.Text : "0"));
            CalTotalTax(AdditionalTax, OtherTax);
            /*Added by vinodha.m on sep3,2015 to calculate total tax when postback event is happening*/

            if (!IsPostBack)
            {
                if (dtTaxAssetHistory.Rows.Count == 0)
                {
                    FunPriInsertTaxAssetHistoryDataTable();
                }
                else
                {
                    FunPubBindTaxAssetHistory(dtTaxAssetHistory);
                }
                loblist();
                FunPriBindLOBBranchTaxType();
                FunpriLoadGLCode();
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (((intTaxGuideId > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if (((intTaxGuideId > 0)) && (strMode == "Q"))
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
                if (dtTaxAsset.Rows.Count == 0)
                {
                    FunPriInsertTaxAssetDataTable("-1", "-1", "", "", "");
                }
                else
                {
                    FunPubBindTaxAsset(dtTaxAsset);
                }
                if (strMode == "Q" && (gvTaxAsset.FooterRow != null))
                {
                    gvTaxAsset.Columns[2].Visible = false;
                    gvTaxAsset.FooterRow.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    private void FunPriBindGrid()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            Procparam.Add("@TaxGuide_ID", Convert.ToString(intTaxGuideId));
            Procparam.Add("@service_Type", Convert.ToString(ddlServiceType.SelectedValue));
            if (rbpurchase.Checked == true)
                Procparam.Add("@Tax_Class", "1");//purchase
            /*Added by vinodha.m on Sep3,2015 to pass values based on Sales checked or not*/
            else if (rbsales.Checked == true)
                Procparam.Add("@Tax_Class", "2");//sales
            else
                Procparam.Add("@Tax_Class", "0");
            /*Added by vinodha.m on Sep3,2015 to pass values based on Sales checked or not*/

            if (ddlTaxType.SelectedValue != "0")
                Procparam.Add("@Tax_Type_ID", Convert.ToString(ddlTaxType.SelectedValue));
            else
                Procparam.Add("@Tax_Type_ID", Convert.ToString("0"));

            if (ddlleasestate.SelectedIndex > 0)
                Procparam.Add("@State_ID", Convert.ToString(ddlleasestate.SelectedValue));
            else
                Procparam.Add("@State_ID", Convert.ToString("0"));

            /*Added by vinodha.m on sep3,2015 to filter asset history grid through asset type as per the latest CR*/
            if (hfAssetTypeDesc.Value != String.Empty)
                Procparam.Add("@Asset_Type_Desc", hfAssetTypeDesc.Value);
            else
                Procparam.Add("@Asset_Type_Desc", "");
            /*Added by vinodha.m on sep3,2015 to filter asset history grid through asset type as per the latest CR*/

            if (ddlReverseChargeTypeorZone.SelectedIndex > 0)
                Procparam.Add("@RevChargeType_Zone_ID", Convert.ToString(ddlReverseChargeTypeorZone.SelectedValue));
            else
                Procparam.Add("@RevChargeType_Zone_ID", Convert.ToString("0"));

            //Paging Properties set
            if (ViewState["dtHSN"] != null)
            {
                dtHSN = (DataTable)ViewState["dtHSN"];
                Procparam.Add("@xmlService", dtHSN.FunPubFormXml());
            }
            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            //Paging Properties end

            //Paging Config            

            //This is to show grid header
            bool bIsNewRow = false;
            gvTaxAssetHistory.BindGridView("S3G_TaxGuide_GetAHPaging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                gvTaxAssetHistory.Rows[0].Visible = false;
            }

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            //objTaxGuideClient.Close();
        }
    }

    #region Page Events

    /// <summary>
    /// This is used to save TaxGuide details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (hdnSave.Value == "1")
        {
            Utility.FunShowAlertMsg(this.Page, "Some changes made: Kindly click GO before Save");
            return;
        }

        vsUserMgmt.Visible = true;

        if ((!rbAll.Checked) && (!rbpurchase.Checked) && (!rbsales.Checked))
        {
            Utility.FunShowAlertMsg(this.Page, "Select The Tax Class");
            return;
        }

        if (((rbAll.Checked || rbpurchase.Checked) || (rbsales.Checked)) && ddlTaxType.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this.Page, "Select The Tax Type");
            return;
        }

        if (!string.IsNullOrEmpty(txtAdditionalTax.Text) && txtAdditionalTax.Text != "0" && txtAdditionalTax.Text != "0.00" && string.IsNullOrEmpty(txtAdditionalTaxName.Text) && Convert.ToDecimal(txtAdditionalTax.Text) > 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Enter The Additional Tax Name");
            return;
        }

        if (!string.IsNullOrEmpty(txtAdditionalTax.Text) && txtAdditionalTax.Text != "0.00" && ddlAdditionalTaxBasedOn.SelectedValue == "0" && Convert.ToDecimal(txtAdditionalTax.Text) > 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Select The Additional Tax Based On");
            return;
        }

        if (ddlTaxType.SelectedValue != "0" && ddlReverseChargeTypeorZone.SelectedValue == "0")
        {
            //if (rbpurchase.Checked == true && ddlTaxType.SelectedValue.Equals("2"))//reverse charge type     
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Select The Reverse Charge Type");
            //    return;
            //}
            if (rbsales.Checked == true && ddlTaxType.SelectedValue == "4")//zone 
            {
                Utility.FunShowAlertMsg(this.Page, "Select The Zone");
                return;
            }
        }

        if (!string.IsNullOrEmpty(txtTax.Text))
        {
            FunCheckValue(" Tax ", txtTax);
            if (checkvalue)
            {
                txtTax.Text = String.Empty;
                txtTax.Focus();
                return;
            }
        }

        if (!string.IsNullOrEmpty(txtsurcharge.Text))
        {
            FunCheckValue(" Surcharge ", txtsurcharge);
            if (checkvalue)
            {
                txtsurcharge.Text = String.Empty;
                txtsurcharge.Focus();
                return;
            }
        }

        if (!string.IsNullOrEmpty(txtCess.Text))
        {
            FunCheckValue(" Cess ", txtCess);
            if (checkvalue == true)
            {
                txtCess.Text = String.Empty;
                txtCess.Focus();
                return;
            }
        }

        if (!string.IsNullOrEmpty(txtEduCess.Text))
        {
            FunCheckValue(" Edu Cess  ", txtEduCess);
            if (checkvalue == true)
            {
                txtEduCess.Text = String.Empty;
                txtEduCess.Focus();
                return;
            }
        }

        if (!string.IsNullOrEmpty(txtAdditionalTax.Text))
        {
            FunCheckValue(" Additional Tax ", txtAdditionalTax);
            if (checkvalue == true)
            {
                txtAdditionalTax.Text = String.Empty;
                txtAdditionalTax.Focus();
                return;
            }
        }

        if (!string.IsNullOrEmpty(txtOtherTaxRate.Text))
        {
            FunCheckValue(" Other Tax Rate ", txtOtherTaxRate);
            if (checkvalue == true)
            {
                txtOtherTaxRate.Text = String.Empty;
                txtOtherTaxRate.Focus();
                return;
            }
        }

        if (ddlOtherTaxName.SelectedValue != "0")
        {
            if (ddlOtherTaxBasedOn.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Other Tax Based on cannot be left empty");
                return;
            }
            if (txtOtherTaxRate.Text == String.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, "Other Tax Rate cannot be left empty");
                return;
            }
        }


        if (chkActive.Checked == true)
        {
            if (ViewState["Tax_AssetDetails"] != null)
            {
                DataTable DT = ViewState["Tax_AssetDetails"] as DataTable;
                if (DT.Rows.Count == 1)
                {
                    if (DT.Rows[0]["Asset_Serial_Number"].ToString() == "0")
                    {
                        //Utility.FunShowAlertMsg(this.Page, "Atleast One Row Should Be Added In Asset Type Wise Details");
                        //return;
                    }
                }
            }
        }

        if (ViewState["dtHSN"] != null && ((DataTable)ViewState["dtHSN"]).Rows.Count > 0)
        {
            if (String.IsNullOrEmpty(((DataTable)ViewState["dtHSN"]).Rows[0]["HSN_ID"].ToString()))
            {
                if (ddlServiceType.SelectedValue == "2")
                    Utility.FunShowAlertMsg(this.Page, "Atleast One Row Should Be Added In SAC Details");
                else
                    Utility.FunShowAlertMsg(this.Page, "Atleast One Row Should Be Added In HSN Details");
                return;
            }
        }
        else
        {
            if (ddlServiceType.SelectedValue == "2")
                Utility.FunShowAlertMsg(this.Page, "Atleast One Row Should Be Added In SAC Details");
            else
                Utility.FunShowAlertMsg(this.Page, "Atleast One Row Should Be Added In HSN Details");
            return;
        }

        string Tax_Code = "";
        ObjTaxGuideMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            AccountMgtServices.S3G_SYSAD_TaxGuideRow ObjTaxGuideRow;
            ObjTaxGuideRow = ObjS3G_SYSAD_TaxGuideDataTable.NewS3G_SYSAD_TaxGuideRow();

            ObjTaxGuideRow.Company_ID = intCompanyID;
            if (ddlLOB.SelectedValue != "0")
                ObjTaxGuideRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            
            if (rbAll.Checked == true)
                ObjTaxGuideRow.Tax_Class = 0;
            else if (rbpurchase.Checked == true)
                ObjTaxGuideRow.Tax_Class = 1;
            else if(rbsales.Checked == true)
                ObjTaxGuideRow.Tax_Class = 2;

            //ObjTaxGuideRow.Tax_Class = rbpurchase.Checked == true ? 1 : 2;

            if (ddlTaxType.SelectedValue != "0")
                ObjTaxGuideRow.Tax_Type_ID = Convert.ToInt32(ddlTaxType.SelectedValue);
            ObjTaxGuideRow.State_ID = Convert.ToInt32(ddlleasestate.SelectedValue);
            ObjTaxGuideRow.Effective_From = Utility.StringToDate(txtEffFrom.Text);
            ObjTaxGuideRow.Tax_Description = txtTaxDesc.Text;
            if (!string.IsNullOrEmpty(txtRate.Text))
                ObjTaxGuideRow.RatePercentage = Convert.ToDecimal(txtRate.Text);
            if (!string.IsNullOrEmpty(txtTax.Text))
                ObjTaxGuideRow.Tax = (txtTax.Text.Replace(".", "") == "") ? 0 : Convert.ToDecimal(txtTax.Text);
            if (!string.IsNullOrEmpty(txtsurcharge.Text))
                ObjTaxGuideRow.Surcharge = (txtsurcharge.Text.Replace(".", "") == "") ? 0 : Convert.ToDecimal(txtsurcharge.Text);
            if (!string.IsNullOrEmpty(txtCess.Text))
                ObjTaxGuideRow.Cess = (txtCess.Text.Replace(".", "") == "") ? 0 : Convert.ToDecimal(txtCess.Text);
            if (!string.IsNullOrEmpty(txtEduCess.Text))
                ObjTaxGuideRow.Educational_Cess = (txtEduCess.Text.Replace(".", "") == "") ? 0 : Convert.ToDecimal(txtEduCess.Text);
            if (!string.IsNullOrEmpty(txtAdditionalTax.Text))
            {
                ObjTaxGuideRow.Additional_tax = (txtAdditionalTax.Text.Replace(".", "") == "") ? 0 : Convert.ToDecimal(txtAdditionalTax.Text);
                ObjTaxGuideRow.Additional_tax_name = txtAdditionalTaxName.Text;
                if (ddlAdditionalTaxBasedOn.SelectedValue != String.Empty)
                    ObjTaxGuideRow.Additional_tax_On = Convert.ToInt32(ddlAdditionalTaxBasedOn.SelectedValue);
            }
            if (ddlOtherTaxName.SelectedValue != "0")
            {
                ObjTaxGuideRow.Other_Tax_ID = Convert.ToInt32(ddlOtherTaxName.SelectedValue);
                ObjTaxGuideRow.Other_Tax_Name = ddlOtherTaxName.SelectedItem.Text;
                ObjTaxGuideRow.Other_Tax_On = Convert.ToInt32(ddlOtherTaxBasedOn.SelectedValue);
                if (!string.IsNullOrEmpty(txtOtherTaxRate.Text))
                    ObjTaxGuideRow.Other_Tax_Rate = (txtOtherTaxRate.Text.Replace(".", "") == "") ? 0 : Convert.ToDecimal(txtOtherTaxRate.Text);
            }
            if (ddlTaxType.SelectedValue != "0")
            {
                if (rbpurchase.Checked == true && ddlTaxType.SelectedValue == "2" || rbsales.Checked == true && ddlTaxType.SelectedValue == "4")
                {
                    if (rbpurchase.Checked == true && ddlTaxType.SelectedValue == "2")//reverse charge tax                                
                        ObjTaxGuideRow.Reverse_charge_type = ddlReverseChargeTypeorZone.SelectedValue == "ALL" ? 0 : Convert.ToInt32(ddlReverseChargeTypeorZone.SelectedValue);
                    if (rbsales.Checked == true && ddlTaxType.SelectedValue == "4")//zone
                        ObjTaxGuideRow.Zone_ID = Convert.ToInt32(ddlReverseChargeTypeorZone.SelectedValue);
                }
            }
            ObjTaxGuideRow.Created_By = intUserId;
            ObjTaxGuideRow.Tax_Guide_ID = intTaxGuideId;
            ObjTaxGuideRow.Is_Active = chkActive.Checked;

            if (ViewState["Tax_AssetDetails"] != null)
            {
                FunPriGenerateTaxAssetXMLDet();
            }
            ObjTaxGuideRow.XMLAsset = strXMLTaxAssetDet;
            ObjTaxGuideRow.Abatement = Convert.ToDecimal(txtAbatement.Text.ToString());
            ObjTaxGuideRow.Service_Type = Convert.ToInt32(ddlServiceType.SelectedValue);
            if (ViewState["dtservices"] != null)
            {
                dtservices = (DataTable)ViewState["dtservices"];
                ObjTaxGuideRow.XML_Services = dtservices.FunPubFormXml();
            }

            if (ViewState["dtHSN"] != null)
            {
                FunPriGenerateHSNAssetXMLDet();
            }

            if (strxmlHSN != null)
                ObjTaxGuideRow.XMLHSN = strxmlHSN;
            else
                ObjTaxGuideRow.XMLHSN = "<Root></Root>";

            ObjS3G_SYSAD_TaxGuideDataTable.AddS3G_SYSAD_TaxGuideRow(ObjTaxGuideRow);

            intErrCode = ObjTaxGuideMasterClient.FunPubCreateTaxGuide(out Tax_Code, SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_TaxGuideDataTable, SerMode));

            if (intErrCode == 0)
            {
                btnSave.Enabled = false;
                txtTaxCode.Text = Tax_Code;
                strAlert = "Tax Guide " + Tax_Code + " added successfully";
                strAlert += @"\n\nWould you like to add one more tax guide?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 4)
            {
                btnSave.Enabled = false;
                txtTaxCode.Text = Tax_Code;
                strAlert = "Tax Guide Updated successfully";//" + Tax_Code + " // Modified By : Anbuvel.T,Date : 14-JAN-2015,Description : Bug fixing for CR_POCSL_026(Tax Guide Master History Maintanance)(due to Lead Suggestion)
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 5)
            {
                btnSave.Enabled = false;
                txtTaxCode.Text = Tax_Code;
                strAlert = "Tax Guide " + Tax_Code + " Deleted successfully";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Tax Guide already exist");
            }
            else if (intErrCode == 7)
            {
                Utility.FunShowAlertMsg(this.Page, "Define Tax Guide at Location Level");
            }
            else if (intErrCode == 8)
            {
                Utility.FunShowAlertMsg(this.Page, "Define Tax Guide at LOB Level");
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create TaxCode");
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "Effective from date cannot be less than date of incorporation of the company");
            }
            lblErrorMessage.Text = string.Empty;

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
            ObjTaxGuideMasterClient.Close();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        dtHSN = (DataTable)ViewState["dtHSN"];

        DataRow dr = dtHSN.NewRow();
        UserControls_S3GAutoSuggest txtHSNCode = grvHSNCOde.FooterRow.FindControl("txtHSNCode") as UserControls_S3GAutoSuggest;

        if (txtHSNCode.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this.Page, "Select a Code");
            return;
        }

        DataRow[] row = dtHSN.Select("HSN_ID=" + txtHSNCode.SelectedValue + "");

        if (row.Length > 0)
        {
            string strSerType = ddlServiceType.SelectedValue == "1" ? "HSN Code" : "SAC Code";
            Utility.FunShowAlertMsg(this.Page, strSerType + " Already Exists.");
            return;
        }

        dr["HSN_ID"] = txtHSNCode.SelectedValue;
        dr["HSN_Code"] = txtHSNCode.SelectedText;
        dr["Is_Checked"] = 1;

        dtHSN.Rows.Add(dr);

        grvHSNCOde.DataSource = dtHSN;
        grvHSNCOde.DataBind();

        ViewState["dtHSN"] = dtHSN;

        btnGo_Click(null, null);
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        dtHSN = (DataTable)ViewState["dtHSN"];

        GridViewRow gvr = (GridViewRow)((Button)sender).Parent.NamingContainer;

        dtHSN.Rows.RemoveAt(gvr.RowIndex);

        if (dtHSN.Rows.Count > 0)
        {

            grvHSNCOde.DataSource = dtHSN;
            grvHSNCOde.DataBind();

            ViewState["dtHSN"] = dtHSN;

            hdnSave.Value = "0";
            btnSave.Enabled = true;
            if (ddlServiceType.SelectedValue == "1")
            {
                FunPubBindTaxAssetGr();
                FunPriGenerateHSNAssetXMLDet();

            }
            else if (ddlServiceType.SelectedValue == "2")
            {
                FunPubBindServices();
            }
            FunPriBindGrid();
        }
        else
        {
            DataRow dr = dtHSN.NewRow();
            dr["Is_Checked"] = 1;
            dtHSN.Rows.Add(dr);
            grvHSNCOde.DataSource = dtHSN;
            grvHSNCOde.DataBind();
            grvHSNCOde.Rows[0].Visible = false;
            dtHSN.Clear();
            ViewState["dtHSN"] = dtHSN;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        dtHSN = (DataTable)ViewState["dtHSN"];

        //foreach (GridViewRow grvRow in grvHSNCOde.Rows)
        //{
        //    if (((CheckBox)grvRow.FindControl("chkSel")).Checked)
        //    {
        //        DataRow drAdd = dtHSN.NewRow();
        //        drAdd["HSN_ID"] = ((Label)grvRow.FindControl("lblHSNID")).Text;

        //        dtHSN.Rows.Add(drAdd);
        //        dtHSN.AcceptChanges();
        //    }
        //}

        ViewState["dtHSN"] = dtHSN;
        if (dtHSN.Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Select atleast one code");
            return;
        }
        hdnSave.Value = "0";
        btnSave.Enabled = true;
        if (ddlServiceType.SelectedValue == "1")
        {
            FunPubBindTaxAssetGr();
            FunPriGenerateHSNAssetXMLDet();

        }
        else if (ddlServiceType.SelectedValue == "2")
        {
            FunPubBindServices();
        }
        FunPriBindGrid();

    }

    private void FunPubBindTaxAssetGr()
    {
        try
        {
            dtHSN = (DataTable)ViewState["dtHSN"];
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@TaxCode_ID", intTaxGuideId.ToString());
            if (dtHSN != null)
                Procparam.Add("@Xml_HSN", dtHSN.FunPubFormXml());
            dtTaxAssetGr = Utility.GetDefaultData("S3G_Get_Taxguide_Asset_Details", Procparam);
            if (strMode == "Q")
            {

                DataRow[] advRow = dtTaxAssetGr.Select("Is_Checked = 0");
                foreach (DataRow dr in advRow)
                {
                    dr.Delete();
                }
                dtTaxAssetGr.AcceptChanges();
            }
            pnlAsset.Visible = true;
            if (dtTaxAssetGr.Rows.Count > 0)
            {
                gvTaxAsset.DataSource = dtTaxAssetGr;
                gvTaxAsset.DataBind();
                ViewState["Tax_AssetDetails"] = dtTaxAssetGr;
            }
            else
            {
                gvTaxAsset.EmptyDataText = "No records found";
                gvTaxAsset.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grvServices.DataSource = null;
            grvServices.DataBind();
            gvTaxAsset.DataSource = null;
            gvTaxAsset.DataBind();
            gvTaxAssetHistory.DataSource = null;
            gvTaxAssetHistory.DataBind();
            ViewState["Tax_AssetDetails"] = null;
            ViewState["dtservices"] = null;
            pnlservice.Visible = false;
            pnlAsset.Visible = false;
            //pnlHistory.Visible = false;
            if (ddlServiceType.SelectedValue != "0")
            {
                FunPubBindHSNGr();
            }
            if (ddlServiceType.SelectedValue == "0")
            {
                btnGo.Enabled = false;
            }
            else
            {
                btnGo.Enabled = true;
            }
            //btnSave.Enabled = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    private bool FunPriGenerateHSNAssetXMLDet()
    {
        try
        {
            string strHSNID = string.Empty;
            strbHSNDet = new StringBuilder();

            strbHSNDet.Append("<Root>");
            foreach (GridViewRow grvData in grvHSNCOde.Rows)
            {
                strHSNID = ((Label)grvData.FindControl("lblHSNID")).Text;
                strbHSNDet.Append(" <Details HSN_ID='" + strHSNID + "' /> ");
            }
            strbHSNDet.Append("</Root>");
            strxmlHSN = strbHSNDet.ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        return true;
    }

    private void FunPubBindServices()
    {
        try
        {
            dtservices = (DataTable)ViewState["dtservices"];
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@TaxCode_ID", intTaxGuideId.ToString());
            if (dtHSN != null)
                Procparam.Add("@Xml_HSN", dtHSN.FunPubFormXml());
            dtservices = Utility.GetDefaultData("S3G_Get_Taxguide_Services", Procparam);

            pnlservice.Visible = true;
            if (dtservices.Rows.Count > 0)
            {
                grvServices.DataSource = dtservices;
                grvServices.DataBind();
                ViewState["dtservices"] = dtservices;
            }
            else
            {
                grvServices.EmptyDataText = "No records found";
                grvServices.DataBind();
                ViewState["dtservices"] = null; ;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void FunPubBindHSNGr()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@service_Type", ddlServiceType.SelectedValue);
            dtHSN = Utility.GetDefaultData("S3G_Get_HSN_Tax_Details", Procparam);

            pnlHSN.Visible = true;
            if (dtHSN.Rows.Count > 0)
            {
                DataRow dr = dtHSN.NewRow();
                dtHSN.Clear();
                dr["Is_Checked"] = 1;
                dtHSN.Rows.Add(dr);

                grvHSNCOde.DataSource = dtHSN;
                grvHSNCOde.DataBind();
                grvHSNCOde.Rows[0].Visible = false;
                dtHSN.Clear();
                ViewState["dtHSN"] = dtHSN;
            }
            else
            {
                grvHSNCOde.EmptyDataText = "No records found";
                grvHSNCOde.DataBind();
            }
            if (ddlServiceType.SelectedValue == "1")
            {
                pnlHSN.GroupingText = "HSN Details";
            }
            else if (ddlServiceType.SelectedValue == "2")
            {
                pnlHSN.GroupingText = "SAC Details";
            }
        }

        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// This is used to redirect page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

    /// <summary>
    /// This is used to clear data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = ddlleasestate.SelectedIndex = 0;
            //ddlGLCode.SelectedIndex = 0;
            rbpurchase.Checked = rbsales.Checked = txtAdditionalTaxName.Enabled = false;
            if (ddlReverseChargeTypeorZone.SelectedIndex > 0)
            {
                ddlReverseChargeTypeorZone.SelectedIndex = 0;
            }
            ddlReverseChargeTypeorZone.Enabled = false;
            if (ddlReverseChargeTypeorZone.Items.Count > 0)
            {
                ddlReverseChargeTypeorZone.Items.RemoveAt(0);
                ddlReverseChargeTypeorZone.Items.Insert(0, "--Select--");
            }
            if (ddlAdditionalTaxBasedOn.SelectedIndex > 0)
            {
                ddlAdditionalTaxBasedOn.SelectedIndex = 0;
                ddlAdditionalTaxBasedOn.Enabled = false;
            }

            if (ddlOtherTaxBasedOn.SelectedIndex > 0)
            {
                ddlOtherTaxBasedOn.SelectedIndex = 0;
                ddlOtherTaxBasedOn.Enabled = false;
            }
            ddlOtherTaxName.SelectedIndex = 0;
            txtOtherTaxRate.Text = String.Empty;
            txtOtherTaxRate.Enabled = false;
            //if (ddlSLCode.SelectedIndex > 0)
            //{
            //    ddlSLCode.SelectedIndex = 0;
            //    ddlSLCode.Enabled = false;
            //}
            if (ddlTaxType.SelectedIndex > 0)
                ddlTaxType.SelectedIndex = 0;
            ddlTaxType.Enabled = false;
            txtTaxCode.Text = txtTaxDesc.Text = txtEffFrom.Text = txtTax.Text = txtRate.Text = txtsurcharge.Text = txtCess.Text = txtEduCess.Text =
            txtAdditionalTax.Text = txtAdditionalTaxName.Text = txttotaltax.Text = hfAssetTypeDesc.Value = String.Empty;
            chkActive.Checked = true;
            lbladditionaltaxbasedon.CssClass = lblAdditionalTaxName.CssClass = "styleDisplayLabel";
            ViewState["Tax_AssetDetails"] = null;
            FunPriInsertTaxAssetDataTable("-1", "-1", "", "", "");
            /*Added by vinodha.m on sep3,2015 to clear asset history grid details */
            FunPriBindGrid();
            /*Added by vinodha.m on sep3,2015 to clear asset history grid details */
            ddlServiceType.SelectedIndex = 0;
            grvServices.DataSource = null;
            grvServices.DataBind();
            gvTaxAsset.DataSource = null;
            gvTaxAsset.DataBind();
            gvTaxAssetHistory.DataSource = null;
            gvTaxAssetHistory.DataBind();
            ViewState["Tax_AssetDetails"] = null;
            ViewState["dtservices"] = null;
            ViewState["dtHSN"] = null;
            pnlservice.Visible = false;
            pnlAsset.Visible = false;
            pnlHSN.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            // ObjTaxGuideMasterClient.Close();
        }
    }

    #endregion

    #region Page Methods


    /// <summary>
    /// This is used to bind lob,product,module and program details in dropdownlist
    /// </summary>

    private void FunPriBindLOBBranchTaxType()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            dsInitLoadDetails = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", Procparam);
            //tax class - sales and tax type - ST ,Concessional ST
            //if (rbsales.Checked == true && ((ddlTaxType.SelectedValue == "5") || (ddlTaxType.SelectedValue == "7")))
            ddlleasestate.BindMemoDataTable("S3G_SYSAD_Get_StateDetails", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
            //else
            //ddlleasestate.BindDataTable("S3G_SYSAD_Get_StateDetails", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
            ddlReverseChargeTypeorZone.Enabled = ddlAdditionalTaxBasedOn.Enabled = txtAdditionalTaxName.Enabled
                = ddlTaxType.Enabled = ddlOtherTaxBasedOn.Enabled = false;
            ddlOtherTaxName.BindDataTable(dsInitLoadDetails.Tables[6], new string[] { "Value", "Name" });
            dsInitLoadDetails.Clear();

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "3");
            Procparam.Add("@Param1", "Service_Type");
            ddlServiceType.BindDataTable("S3G_Get_LookUp_TaxType", Procparam, new string[] { "ID", "Name" });

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

    private void loblist()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Is_Active", "1");
            if (intTaxGuideId == 0)
            {
                Procparam.Add("@User_ID", intUserId.ToString());
            }
            Procparam.Add("@Program_ID", "16");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.Items.RemoveAt(0);
            ddlLOB.SelectedValue = "3";
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunCheckValue(string Name, TextBox textvalue)
    {
        if (!string.IsNullOrEmpty(textvalue.Text))
        {
            decimal value = 0;
            value = Convert.ToDecimal(textvalue.Text);
            if (value > 100)
                checkvalue = true;
        }
    }

    private void FunpriLoadGLCode()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //if (ddlLOB.SelectedIndex > 0)
        //{
        //    Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        //}
        Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        //ddlGLCode.BindDataTable("S3G_SYSAD_GetGLCode", Procparam, new string[] { "Account_Setup_ID", "GLAccountDesc" });
    }
    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetTaxGuideDetails()
    {
        ObjTaxGuideMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            ObjS3G_SYSAD_TaxGuideDataTable = new AccountMgtServices.S3G_SYSAD_TaxGuideDataTable();
            AccountMgtServices.S3G_SYSAD_TaxGuideRow ObjTaxGuideRow;
            SerializationMode SerMode = SerializationMode.Binary;
            ObjTaxGuideRow = ObjS3G_SYSAD_TaxGuideDataTable.NewS3G_SYSAD_TaxGuideRow();
            ObjTaxGuideRow.Company_ID = intCompanyID;
            ObjTaxGuideRow.Tax_Code_ID = intTaxGuideId;

            ObjS3G_SYSAD_TaxGuideDataTable.AddS3G_SYSAD_TaxGuideRow(ObjTaxGuideRow);

            byte[] byteTaxGuideDetails = ObjTaxGuideMasterClient.FunPubQueryTaxGuide(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_TaxGuideDataTable, SerMode));

            ObjS3G_SYSAD_TaxGuideDataTable = new AccountMgtServices.S3G_SYSAD_TaxGuideDataTable();
            ObjS3G_SYSAD_TaxGuideDataTable = (AccountMgtServices.S3G_SYSAD_TaxGuideDataTable)ClsPubSerialize.DeSerialize(byteTaxGuideDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_TaxGuideDataTable));

            dsTaxDetails = new DataSet();
            strProcName = "S3G_Get_Taxguide_Asset_Details";
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@TaxCode_ID", intTaxGuideId.ToString());
            dsTaxDetails = Utility.GetTableValues(strProcName, Procparam);

            if (dsTaxDetails.Tables[0].Rows.Count > 0)
            {
                dtTaxAsset = dsTaxDetails.Tables[0].Copy();
                ViewState["Tax_AssetDetails"] = dtTaxAsset;
                pnlAsset.Visible = true;
                FunPubBindTaxAssetGr();
                //  hfAssetTypeDesc.Value = dtTaxAsset.Rows[0]["Asset_Type"].ToString();
                //hfAssetClassDesc.Value = dtTaxAsset.Rows[0]["Asset_Class"].ToString();

            }

            if (dsTaxDetails.Tables[2].Rows.Count > 0)//Other Tax
            {
                dtHSN = dsTaxDetails.Tables[2].Copy();
                ViewState["dtHSN"] = dtHSN;

                pnlHSN.Visible = true;

                if (dtHSN.Rows.Count > 0)
                {
                    grvHSNCOde.DataSource = dtHSN;
                    grvHSNCOde.DataBind();
                    ViewState["dtHSN"] = dtHSN;
                }
                else
                {
                    grvHSNCOde.EmptyDataText = "No records found";
                    grvHSNCOde.DataBind();
                }

            }
            if (dsTaxDetails.Tables[3].Rows.Count > 0)//Other Tax
            {
                dtservices = dsTaxDetails.Tables[3].Copy();
                ViewState["dtservices"] = dtservices;

                pnlservice.Visible = true;

                if (dtservices.Rows.Count > 0)
                {
                    grvServices.DataSource = dtservices;
                    grvServices.DataBind();
                    ViewState["dtservices"] = dtservices;
                }
                else
                {
                    grvServices.EmptyDataText = "No records found";
                    grvServices.DataBind();
                }

            }
            dsTaxDetails.Dispose();

            dsTaxDetails = new DataSet();
            strProcName = "S3G_Get_Taxguide_Details";
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@TaxGuide_ID", intTaxGuideId.ToString());
            dsTaxDetails = Utility.GetTableValues(strProcName, Procparam);

            loblist();
            ddlLOB.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["LOB_ID"].ToString();

            if (ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["TaxClass"].ToString() == "Purchase")
            {
                rbpurchase.Checked = true;
                string TaxClass = "Purchase";
                FunPubFillTaxTypeDetails(TaxClass);
                rbsales.Checked = false;
            }
            else
            {
                rbsales.Checked = true;
                string TaxClass = "Sales";
                FunPubFillTaxTypeDetails(TaxClass);
                rbpurchase.Checked = false;
            }
            txtTaxCode.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Tax_Code"].ToString();
            ddlTaxType.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Tax_Type_ID"].ToString();
            if (ddlTaxType.SelectedValue != "0")
                taxtype = Convert.ToInt32(ddlTaxType.SelectedValue);
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            //tax class - sales and tax type - ST ,Concessional ST
            //if (rbsales.Checked == true && ((ddlTaxType.SelectedValue == "5") || (ddlTaxType.SelectedValue == "7")))
            //{
                ddlleasestate.BindMemoDataTable("S3G_SYSAD_Get_StateDetails", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
                lblleasestate.CssClass = "styleDisplayLabel";
                rfvleasestate.Visible = false;
            /*}
            else if (rbpurchase.Checked == true && (ddlTaxType.SelectedValue == "2"))//tax class - purchase and tax type - reverse charge type
            {
                ddlleasestate.BindMemoDataTable("S3G_SYSAD_Get_StateDetails", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
                lblleasestate.CssClass = "styleDisplayLabel";
                rfvleasestate.Visible = false;
            }
            else
            {
                ddlleasestate.BindDataTable("S3G_SYSAD_Get_StateDetails", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
                lblleasestate.CssClass = "styleReqFieldLabel";
                rfvleasestate.Visible = true;
            }*/
            ddlleasestate.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["State_ID"].ToString();
            txtTaxDesc.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Tax_Description"].ToString();
            txtRate.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["RatePercentage"].ToString();
            txtTax.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Tax"].ToString();
            txtsurcharge.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Surcharge"].ToString();
            txtCess.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Cess"].ToString();
            txtEduCess.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Educational_Cess"].ToString();
            txtAdditionalTax.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Additional_tax"].ToString();
            if (txtAdditionalTax.Text != String.Empty)
                txtAdditionalTax_TextChanged(null, null);
            txtAdditionalTaxName.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Additional_tax_name"].ToString();
            ddlAdditionalTaxBasedOn.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Additional_tax_On"].ToString();
            if (!String.IsNullOrEmpty(txtAdditionalTax.Text))
            {
                AdditionalTax = Convert.ToDecimal(txtAdditionalTax.Text);
            }
            else
            {
                AdditionalTax = Convert.ToDecimal(0);
            }
            if (ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Other_Tax_ID"].ToString() != String.Empty)
            {
                ddlOtherTaxName.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Other_Tax_ID"].ToString();
                ddlOtherTaxName_SelectedIndexChanged(null, null);
            }
            ddlOtherTaxBasedOn.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Other_Tax_On"].ToString();
            txtOtherTaxRate.Text = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Other_Tax_Rate"].ToString();
            if (!String.IsNullOrEmpty(txtOtherTaxRate.Text))
            {
                OtherTax = Convert.ToDecimal(txtOtherTaxRate.Text);
            }
            else
            {
                OtherTax = Convert.ToDecimal(0);
            }
            CalTotalTax(AdditionalTax, OtherTax);
            //ddlGLCode.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Posting_GL_Code"].ToString();
            //ddlGLCode_SelectedIndexChanged(null, null);
            //ddlSLCode.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["SL_Code"].ToString();
            if (ddlTaxType.SelectedIndex > 0)
            {
                if (rbpurchase.Checked == true && (ddlTaxType.SelectedValue == "2" || ddlTaxType.SelectedValue == "14")
                    || rbsales.Checked == true && (ddlTaxType.SelectedValue == "4" || ddlTaxType.SelectedValue == "14"))
                {
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@User_ID", intUserId.ToString());
                    dsInitLoadDetails = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", Procparam);

                    if (rbpurchase.Checked == true && ddlTaxType.SelectedValue == "2")//2-Reverse Charge Type
                    {
                        lblReverseChargeTypeorZone.Text = "Reverse Charge Type";
                        ddlReverseChargeTypeorZone.Enabled = true;
                        ddlReverseChargeTypeorZone.BindDataTable(dsInitLoadDetails.Tables[1], new string[] { "Value", "Name" });
                        if (ddlReverseChargeTypeorZone.Items.Count > 0)
                        {
                            ddlReverseChargeTypeorZone.Items.RemoveAt(0);
                            ddlReverseChargeTypeorZone.Items.Insert(0, "ALL");
                        }
                    }
                    else if (rbsales.Checked == true && ddlTaxType.SelectedValue == "4")//4-Zone
                    {
                        lblReverseChargeTypeorZone.Text = "Zone";
                        ddlReverseChargeTypeorZone.Enabled = true;
                        ddlReverseChargeTypeorZone.BindDataTable(dsInitLoadDetails.Tables[2], new string[] { "Value", "Name" });
                    }
                }
                else
                {
                    lblReverseChargeTypeorZone.Text = "Reverse Charge Type/Zone";
                    ddlReverseChargeTypeorZone.Enabled = false;
                }
                if (rbpurchase.Checked == true && ddlTaxType.SelectedValue == "2")//reverse charge tax                                
                    ddlReverseChargeTypeorZone.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Reverse_charge_type"].ToString();
                if (rbsales.Checked == true && ddlTaxType.SelectedValue == "4")//zone
                    ddlReverseChargeTypeorZone.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Zone_ID"].ToString();
            }

            hdnID.Value = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["USRID"].ToString();

            string strTax = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Tax"].ToString();
            string strSur = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Surcharge"].ToString();
            string strCess = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Cess"].ToString();
            if ((strTax != "0.00") || (strSur != "0.00") || (strCess != "0.00"))
            {
                txtTax.Text = strTax;
                txtsurcharge.Text = strSur;
                txtCess.Text = strCess;
            }
            txtEffFrom.Text = DateTime.Parse(ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Effective_From"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            if (ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Is_Active"].ToString() == "True")
                chkActive.Checked = true;
            else
                chkActive.Checked = false;

            ddlServiceType.SelectedValue = ObjS3G_SYSAD_TaxGuideDataTable.Rows[0]["Service_Type"].ToString();

            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
            if (dsTaxDetails.Tables[2].Rows.Count > 0)
                hfAssetTypeDesc.Value = dsTaxDetails.Tables[2].Rows[0]["Asset_Type_Desc"].ToString();

            FunPriFillAssetHistory(taxclass, taxtype, ddlleasestate.SelectedValue, hfAssetTypeDesc.Value, ddlReverseChargeTypeorZone.SelectedValue);
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
            dsTaxDetails.Dispose();

            if (ddlServiceType.SelectedValue == "1")
            {
                pnlHSN.GroupingText = "HSN Details";
            }
            else if (ddlServiceType.SelectedValue == "2")
            {
                pnlHSN.GroupingText = "SAC Details";
            }
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
            ObjTaxGuideMasterClient.Close();
        }
    }

    /// <summary>
    /// This is to disable controls based on user level role id
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                chkActive.Enabled = ddlSLCode.Enabled = false;
                chkActive.Checked = true;
                txtOtherTaxRate.Enabled = ddlOtherTaxBasedOn.Enabled = false;
                pnlcopyProfile.Visible = true;
                FunGetAssetClass();
                rbAll.Checked = true;
                rbAll_CheckedChanged(null, null);
                break;

            case 1: // Modify Mode

                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                FunGetTaxGuideDetails();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                ddlLOB.ClearDropDownList();
                ddlLOB.Enabled = false;
                rbpurchase.Enabled = false;
                rbsales.Enabled = false;
                rbAll.Enabled = false;
                ddlleasestate.ClearDropDownList();
                ddlleasestate.Enabled = false;
                ddlTaxType.ClearDropDownList();
                ddlTaxType.Enabled = ddlSLCode.Enabled = false;
                chkActive.Enabled = true;
                //txtEffFrom.Enabled = false;
                //CalendarExtender1.Enabled = false;
                btnClear.Enabled = false;
                ddlGLCode.Enabled = false;
                txtRate.Enabled = txttotaltax.Enabled = false;
                ddlServiceType.Enabled = false;
                btnSave.Enabled = true;
                if (ddlServiceType.SelectedValue != "0")
                {
                    btnGo.Enabled = true;
                }
                else
                {
                    btnGo.Enabled = false;
                }
                pnlcopyProfile.Visible = false;
                if (ddlTaxType.SelectedValue == "13" || ddlTaxType.SelectedValue == "14" || ddlTaxType.SelectedValue == "15")
                    rfvServiceType.Enabled = true;
                else
                    rfvServiceType.Enabled = false;
                break;

            case -1:// Query Mode                
                FunGetTaxGuideDetails();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    rbpurchase.Enabled = false;
                    rbsales.Enabled = false;
                    rbAll.Enabled = false;
                    txtTaxCode.ReadOnly = true;
                    txtTaxCode.Enabled = true;
                    ddlleasestate.ClearDropDownList();
                    ddlTaxType.ClearDropDownList();
                    txtTaxDesc.ReadOnly = true;
                    if (ddlReverseChargeTypeorZone.Items.Count > 0)
                        ddlReverseChargeTypeorZone.ClearDropDownList();
                    CalendarExtender1.Enabled = false;
                    txtEffFrom.ReadOnly = true;
                    txtTax.ReadOnly = true;
                    //ddlGLCode.ClearDropDownList();
                    txtRate.ReadOnly = txttotaltax.ReadOnly = true;
                    txtsurcharge.ReadOnly = true;
                    txtCess.ReadOnly = true;
                    txtAdditionalTax.ReadOnly = true;
                    if (ddlAdditionalTaxBasedOn.Items.Count > 0)
                        ddlAdditionalTaxBasedOn.ClearDropDownList();
                    if (ddlOtherTaxBasedOn.Items.Count > 0)
                        ddlOtherTaxBasedOn.ClearDropDownList();
                    txtAdditionalTaxName.Enabled = false;
                    ddlAdditionalTaxBasedOn.Enabled = ddlSLCode.Enabled = false;
                    ddlOtherTaxName.Enabled = ddlOtherTaxBasedOn.Enabled = txtOtherTaxRate.Enabled = false;
                    chkActive.Enabled = false;
                    //ddlGLCode.ClearDropDownList();
                    gvTaxAsset.Enabled = false;
                }

                CalendarExtender1.Enabled = false;
                ddlServiceType.Enabled = false;
                txtEffFrom.Attributes.Remove("onblur");
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                chkActive.Enabled = false;
                txtTaxCode.Enabled = true;
                btnGo.Enabled = false;
                pnlcopyProfile.Visible = false;
                if (grvHSNCOde.FooterRow != null)
                    grvHSNCOde.FooterRow.Visible = grvHSNCOde.Columns[2].Visible = false;
                break;
        }
    }
    #endregion

    protected void rbpurchase_CheckedChanged(object sender, System.EventArgs e)
    {
        try
        {
            if (rbpurchase.Checked == true)
            {
                rbAll.Checked = rbsales.Checked = false;
                string TaxClass = "Purchase";
                FunPubFillTaxTypeDetails(TaxClass);
                if (ddlReverseChargeTypeorZone.Items.Count > 0)
                {
                    ddlReverseChargeTypeorZone.Items.RemoveAt(0);
                    ddlReverseChargeTypeorZone.Items.Insert(0, "--Select--");
                }
                ddlReverseChargeTypeorZone.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    protected void rbsales_CheckedChanged(object sender, System.EventArgs e)
    {
        try
        {
            if (rbsales.Checked == true)
            {
                rbAll.Checked = rbpurchase.Checked = false;
                string TaxClass = "Sales";
                FunPubFillTaxTypeDetails(TaxClass);
                if (ddlReverseChargeTypeorZone.Items.Count > 0)
                {
                    ddlReverseChargeTypeorZone.Items.RemoveAt(0);
                    ddlReverseChargeTypeorZone.Items.Insert(0, "--Select--");
                }
                ddlReverseChargeTypeorZone.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    protected void rbAll_CheckedChanged(object sender, System.EventArgs e)
    {
        try
        {
            if (rbAll.Checked == true)
            {
                rbsales.Checked = rbpurchase.Checked = false;
                string TaxClass = "Sales";
                FunPubFillTaxTypeDetails(TaxClass);
                if (ddlReverseChargeTypeorZone.Items.Count > 0)
                {
                    ddlReverseChargeTypeorZone.Items.RemoveAt(0);
                    ddlReverseChargeTypeorZone.Items.Insert(0, "--Select--");
                }
                ddlReverseChargeTypeorZone.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    protected void txtAdditionalTax_TextChanged(object sender, System.EventArgs e)
    {
        try
        {
            if (txtAdditionalTax.Text != String.Empty && Convert.ToDecimal(txtAdditionalTax.Text.Trim()) > 0)// Modified By : Anbuvel.T,Date : 14-JAN-2015,Description : Bug fixing for Additonal tax Zero Value(due to Lead Suggestion)
            {
                lbladditionaltaxbasedon.CssClass = lblAdditionalTaxName.CssClass = "styleReqFieldLabel";
                ddlAdditionalTaxBasedOn.ClearSelection();
                txtAdditionalTaxName.Text = String.Empty;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@User_ID", intUserId.ToString());
                dsInitLoadDetails = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", Procparam);
                ddlAdditionalTaxBasedOn.Enabled = txtAdditionalTaxName.Enabled = true;
                ddlAdditionalTaxBasedOn.BindDataTable(dsInitLoadDetails.Tables[3], new string[] { "Value", "Name" });
                AdditionalTax = Convert.ToDecimal(txtAdditionalTax.Text);
                CalTotalTax(AdditionalTax, OtherTax);
            }
            else
            {
                lbladditionaltaxbasedon.CssClass = lblAdditionalTaxName.CssClass = "styleDisplayLabel";
                if (ddlAdditionalTaxBasedOn.SelectedIndex > 0)
                    ddlAdditionalTaxBasedOn.SelectedIndex = 0;
                txtAdditionalTaxName.Text = String.Empty;
                ddlAdditionalTaxBasedOn.Enabled = txtAdditionalTaxName.Enabled = false;
                AdditionalTax = 0;
                CalTotalTax(AdditionalTax, OtherTax);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    protected void txtOtherTaxRate_TextChanged(object sender, System.EventArgs e)
    {
        try
        {
            if (txtOtherTaxRate.Text != String.Empty && Convert.ToDecimal(txtOtherTaxRate.Text.Trim()) > 0)
            {
                OtherTax = Convert.ToDecimal(txtOtherTaxRate.Text);
            }
            else
            {
                OtherTax = 0;
            }
            CalTotalTax(AdditionalTax, OtherTax);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    protected void ddlOtherTaxName_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        try
        {
            if (ddlOtherTaxName.SelectedValue != "0")
            {
                lblOtherTaxRate.CssClass = lblOtherTaxBasedOn.CssClass = "styleReqFieldLabel";
                txtOtherTaxRate.Enabled = ddlOtherTaxBasedOn.Enabled = true;
                ddlOtherTaxBasedOn.ClearSelection();
                txtOtherTaxRate.Text = String.Empty;

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@User_ID", intUserId.ToString());
                dsInitLoadDetails = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", Procparam);
                ddlOtherTaxBasedOn.BindDataTable(dsInitLoadDetails.Tables[3], new string[] { "Value", "Name" });
            }
            else
            {
                lblOtherTaxRate.CssClass = lblOtherTaxBasedOn.CssClass = "styleDisplayLabel";
                txtOtherTaxRate.Enabled = ddlOtherTaxBasedOn.Enabled = false;
                ddlOtherTaxBasedOn.ClearSelection();
                txtOtherTaxRate.Text = String.Empty;
                OtherTax = 0;
                CalTotalTax(AdditionalTax, OtherTax);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    protected void txtEffFrom_TextChanged(object sender, System.EventArgs e)
    {
        try
        {
            DateTime dt = DateTime.Today;
            if (txtEffFrom.Text != String.Empty)
            {
                if (Utility.StringToDate(txtEffFrom.Text) < dt)
                {
                    txtEffFrom.Text = String.Empty;
                    Utility.FunShowAlertMsg(this.Page, "Selected date cannot be less than system date");
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunPubFillTaxTypeDetails(string TaxClass)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@TaxClass", TaxClass);
            dsInitTaxTypeDetails = Utility.GetDataset("GetTaxTypeDetailsByTaxClass", dictParam);
            ddlTaxType.Enabled = true;
            ddlTaxType.BindDataTable(dsInitTaxTypeDetails.Tables[0], new string[] { "Value", "Name" });
            if (ddlTaxType.SelectedIndex > 0)
                taxtype = Convert.ToInt32(ddlTaxType.SelectedValue);
            if (ddlReverseChargeTypeorZone.SelectedValue != String.Empty)
                RevChargeZone = ddlReverseChargeTypeorZone.SelectedValue;
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
            FunPriFillAssetHistory(taxclass, taxtype, ddlleasestate.SelectedValue, hfAssetTypeDesc.Value, RevChargeZone);
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    private DataTable FunPubGetTaxAssetHistoryDataTable()
    {
        try
        {
            if (ViewState["Tax_AssetHistoryDetails"] == null)
            {
                dtTaxAssetHistory = new DataTable();
                //dtTaxAssetHistory.Columns.Add("Asset_Category_Desc");
                dtTaxAssetHistory.Columns.Add("Asset_Type_Desc");
                dtTaxAssetHistory.Columns.Add("Effective_From");
                dtTaxAssetHistory.Columns.Add("Effective_To");
                dtTaxAssetHistory.Columns.Add("RatePercentage");
                dtTaxAssetHistory.Columns.Add("Asset_Serial_Number");
                ViewState["Tax_AssetHistoryDetails"] = dtTaxAssetHistory;
            }
            dtTaxAssetHistory = (DataTable)ViewState["Tax_AssetHistoryDetails"];
            return dtTaxAssetHistory;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string Funsetsuffix()
    {
        int suffix = 8;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }


    private void CalTotalTax(decimal AdditionalTax, decimal OtherTax)
    {
        if (!String.IsNullOrEmpty(txtRate.Text))
            Rate = Convert.ToDecimal(txtRate.Text);
        if (ddlAdditionalTaxBasedOn.SelectedValue != String.Empty && ddlAdditionalTaxBasedOn.SelectedIndex > 0)
        {
            if (ddlAdditionalTaxBasedOn.SelectedValue == "1")//BASE RATE
            {
                AdditionalTax = ((AdditionalTax / 100) * Rate);
            }
        }
        /*Added by vinodha.m on sep3,2015 for else part to calculate total tax when additional tax is not given*/
        else
        {
            totaltax = Rate;
            AdditionalTax = 0;
        }
        /*Added by vinodha.m on sep3,2015 for else part to calculate total tax when additional tax is not given*/

        if (ddlOtherTaxBasedOn.SelectedValue != String.Empty && ddlOtherTaxBasedOn.SelectedIndex > 0)
        {
            if (ddlOtherTaxBasedOn.SelectedValue == "1")//BASE VALUE
            {
                OtherTax = ((OtherTax / 100) * Rate);
            }
        }
        /*Added by vinodha.m on sep3,2015 for else part to calculate total tax when additional tax is not given*/
        else
        {
            totaltax = Rate;
            OtherTax = 0;
        }
        /*Added by vinodha.m on sep3,2015 for else part to calculate total tax when additional tax is not given*/

        totaltax = Rate + AdditionalTax + OtherTax;
        txttotaltax.Text = totaltax.ToString(Funsetsuffix());
    }

    #region GridEvents

    protected void ddlTaxType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbpurchase.Checked == true && ddlTaxType.SelectedValue == "2" || rbsales.Checked == true && ddlTaxType.SelectedValue == "4")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@User_ID", intUserId.ToString());
                dsInitLoadDetails = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", Procparam);

                if (rbpurchase.Checked == true && ddlTaxType.SelectedValue == "2")//2-Reverse Charge Tax
                {
                    lblReverseChargeTypeorZone.Text = "Reverse Charge Type";
                    lblReverseChargeTypeorZone.CssClass = "styleReqFieldLabel";
                    ddlReverseChargeTypeorZone.Enabled = true;
                    ddlReverseChargeTypeorZone.BindDataTable(dsInitLoadDetails.Tables[1], new string[] { "Value", "Name" });
                    if (ddlReverseChargeTypeorZone.Items.Count > 0)
                    {
                        ddlReverseChargeTypeorZone.Items.RemoveAt(0);
                        ddlReverseChargeTypeorZone.Items.Insert(0, "ALL");
                    }
                }
                else if (rbsales.Checked == true && ddlTaxType.SelectedValue == "4")//4-Zone
                {
                    lblReverseChargeTypeorZone.Text = "Zone";
                    lblReverseChargeTypeorZone.CssClass = "styleReqFieldLabel";
                    ddlReverseChargeTypeorZone.Enabled = true;
                    ddlReverseChargeTypeorZone.BindDataTable(dsInitLoadDetails.Tables[2], new string[] { "Value", "Name" });
                }
            }
            else
            {
                lblReverseChargeTypeorZone.Text = "Reverse Charge Type/Zone";
                lblReverseChargeTypeorZone.CssClass = "styleDisplayLabel";
                ddlReverseChargeTypeorZone.Enabled = false;
                ddlReverseChargeTypeorZone.ClearSelection();
                if (ddlReverseChargeTypeorZone.Items.Count > 0)
                {
                    ddlReverseChargeTypeorZone.Items.RemoveAt(0);
                    ddlReverseChargeTypeorZone.Items.Insert(0, "--Select--");
                }
            }

            if (ddlTaxType.SelectedValue == "13" || ddlTaxType.SelectedValue == "14" || ddlTaxType.SelectedValue == "15")
                rfvServiceType.Enabled = true;
            else
                rfvServiceType.Enabled = false;

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            //tax class - sales and tax type - ST ,Concessional ST
            //if (rbsales.Checked == true && ((ddlTaxType.SelectedValue == "5") || (ddlTaxType.SelectedValue == "7")))
            //{
                ddlleasestate.BindMemoDataTable("S3G_SYSAD_Get_StateDetails", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
                lblleasestate.CssClass = "styleDisplayLabel";
                rfvleasestate.Visible = false;
            /*}
            else if (rbpurchase.Checked == true && ddlTaxType.SelectedValue == "2")
            {
                ddlleasestate.BindMemoDataTable("S3G_SYSAD_Get_StateDetails", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
                lblleasestate.CssClass = "styleDisplayLabel";
                rfvleasestate.Visible = false;
            }
            else
            {
                ddlleasestate.BindDataTable("S3G_SYSAD_Get_StateDetails", Procparam, new string[] { "Location_Category_ID", "LocationCat_Description" });
                lblleasestate.CssClass = "styleReqFieldLabel";
                rfvleasestate.Visible = true;
            }*/

            if (ddlTaxType.SelectedIndex > 0)
                taxtype = Convert.ToInt32(ddlTaxType.SelectedValue);
            state = ddlleasestate.SelectedValue;
            if (ddlReverseChargeTypeorZone.SelectedValue != String.Empty)
                RevChargeZone = ddlReverseChargeTypeorZone.SelectedValue;
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
            FunPriFillAssetHistory(taxclass, taxtype, state, hfAssetTypeDesc.Value, RevChargeZone);
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlleasestate_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTaxType.SelectedIndex > 0)
                taxtype = Convert.ToInt32(ddlTaxType.SelectedValue);
            state = ddlleasestate.SelectedValue;
            if (ddlReverseChargeTypeorZone.SelectedValue != String.Empty)
                RevChargeZone = ddlReverseChargeTypeorZone.SelectedValue;
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
            FunPriFillAssetHistory(taxclass, taxtype, state, hfAssetTypeDesc.Value, RevChargeZone);
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlAdditionalTaxBasedOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            AdditionalTax = Convert.ToDecimal(txtAdditionalTax.Text);
            CalTotalTax(AdditionalTax, OtherTax);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlOtherTaxBasedOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOtherTaxRate.Text != String.Empty)
            {
                OtherTax = Convert.ToDecimal(txtOtherTaxRate.Text);
                CalTotalTax(AdditionalTax, OtherTax);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlReverseChargeTypeorZone_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTaxType.SelectedIndex > 0)
                taxtype = Convert.ToInt32(ddlTaxType.SelectedValue);
            state = ddlleasestate.SelectedValue;
            if (ddlReverseChargeTypeorZone.SelectedValue != String.Empty)
                RevChargeZone = ddlReverseChargeTypeorZone.SelectedValue;
            /*Added by vinodha.m on Sep3,2015 Included Parameter Asset type desc to filter Asset History Grid */
            FunPriFillAssetHistory(taxclass, taxtype, state, hfAssetTypeDesc.Value, RevChargeZone);
            /*Added by vinodha.m on Sep3,2015 Included Parameter Asset type desc to filter Asset History Grid */
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunPriFillAssetHistory(int taxclass, int taxtype, string State, string Asset_Type_Desc, string RevChargeZone)
    {
        try
        {
            /*Added by vinodha.m on Sep3,2015 Included Parameter Asset type desc to filter Asset History Grid */
            if ((!(taxtype == 0) && (ddlleasestate.SelectedValue != String.Empty) && (hfAssetTypeDesc.Value != String.Empty)) || (ddlReverseChargeTypeorZone.SelectedValue != String.Empty))
            {
                FunPriBindGrid();
            }
            else
            {
                FunPriInsertTaxAssetHistoryDataTable();
            }
            /*Added by vinodha.m on Sep3,2015 Included Parameter Asset type desc to filter Asset History Grid */
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
        }
    }

    //protected void ddlAssetCategory_Grd_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DropDownList ddlAssetCategory_Grd;
    //        DropDownList ddlAssetType_Grd;
    //        if (gvTaxAsset.FooterRow.Visible)
    //        {
    //            ddlAssetCategory_Grd = (DropDownList)gvTaxAsset.FooterRow.FindControl("ddlAssetCategory");
    //            ddlAssetType_Grd = (DropDownList)gvTaxAsset.FooterRow.FindControl("ddlAssetType");
    //        }
    //        else
    //        {
    //            ddlAssetCategory_Grd = (DropDownList)sender;
    //            GridViewRow gvRow = (GridViewRow)ddlAssetCategory_Grd.Parent.Parent;
    //            ddlAssetType_Grd = (DropDownList)gvTaxAsset.Rows[gvRow.RowIndex].FindControl("ddlAssetType");
    //        }
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", intCompanyID.ToString());
    //        Procparam.Add("@Asset_Category_ID", ddlAssetCategory_Grd.SelectedValue);
    //        ddlAssetType_Grd.BindDataTable("S3G_SYSAD_GetAssetTypeByAssetCategory", Procparam, new string[] { "AT_ID", "AT_DESC" });
    //        ddlAssetType_Grd.Items.RemoveAt(0);
    //        if (ddlAssetCategory_Grd.SelectedValue == "0")
    //        {
    //            ddlAssetType_Grd.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--ALL--", "0")));
    //            ddlAssetType_Grd.Enabled = true;
    //        }
    //        else
    //        {
    //            if (ddlAssetType_Grd.Items.Count == 0)
    //                ddlAssetType_Grd.Enabled = false;
    //            else
    //            {
    //                ddlAssetType_Grd.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--ALL--", "0")));
    //                ddlAssetType_Grd.Enabled = true;
    //            }
    //        }
    //        Procparam = null;
    //        if (ddlAssetCategory_Grd.SelectedIndex > 0)
    //            Asset_Category_ID = Convert.ToInt32(ddlAssetCategory_Grd.SelectedValue);
    //        if (ddlTaxType.SelectedValue != String.Empty && ddlTaxType.SelectedValue != "0")
    //            taxtype = Convert.ToInt32(ddlTaxType.SelectedValue);
    //        FunPriFillAssetHistory(taxtype, ddlleasestate.SelectedValue, Asset_Category_ID);
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //    finally
    //    {
    //        //ObjTaxGuideMasterClient.Close();
    //    }
    //}

    protected void ddlAssetType_Grd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlAssetType_Grd;
            if (gvTaxAsset.FooterRow.Visible)
            {
                ddlAssetType_Grd = (DropDownList)gvTaxAsset.FooterRow.FindControl("ddlAssetType");
            }
            else
            {
                ddlAssetType_Grd = (DropDownList)sender;
            }
            //if (ddlAssetType_Grd.SelectedIndex > 0)
            //    Asset_Type_ID = Convert.ToInt32(ddlAssetType_Grd.SelectedValue);            

            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
            hfAssetTypeDesc.Value = String.Empty;
            hfAssetTypeDesc.Value = ddlAssetType_Grd.SelectedItem.Text;
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/

            if (ddlTaxType.SelectedValue != String.Empty && ddlTaxType.SelectedValue != "0")
                taxtype = Convert.ToInt32(ddlTaxType.SelectedValue);
            if (ddlReverseChargeTypeorZone.SelectedValue != String.Empty)
                RevChargeZone = ddlReverseChargeTypeorZone.SelectedValue;

            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
            FunPriFillAssetHistory(taxclass, taxtype, ddlleasestate.SelectedValue, hfAssetTypeDesc.Value, RevChargeZone);
            /*Added by vinodha.m on Sep3,2015 to filter asset history grid through asset type*/
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //ObjTaxGuideMasterClient.Close();
        }
    }

    protected void ddlGLCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlGLCode.SelectedValue != "0")
            //{
            //    Dictionary<string, string> dictParam = null;
            //    dictParam = new Dictionary<string, string>();
            //    dictParam.Add("@Is_Active", "1");
            //    dictParam.Add("@LOB_ID", "3");
            //    dictParam.Add("@Company_ID", intCompanyID.ToString());
            //    dictParam.Add("@User_ID", intUserId.ToString());
            //    dictParam.Add("@GL_Code", ddlGLCode.SelectedValue);
            //    ddlSLCode.BindDataTable("S3G_ORG_GETGLACCOUNT", dictParam, new string[] { "Account_Setup_ID", "SL_Account_Code" });
            //    if (ddlSLCode.Items.Count > 1)
            //    {
            //        ddlSLCode.Enabled = true;
            //        rfvSLCode.Visible = true;
            //    }
            //    else
            //    {
            //        ddlSLCode.Enabled = false;
            //        rfvSLCode.Visible = false;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, "TDS Master");
            throw ex;
        }
    }

    protected void gvTaxAsset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //DropDownList ddlAssetCategory = e.Row.FindControl("ddlAssetCategory") as DropDownList;
                DropDownList ddlAssetType = e.Row.FindControl("ddlAssetType") as DropDownList;

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@User_ID", intUserId.ToString());
                dsInitLoadDetails = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", Procparam);

                //ddlAssetCategory.BindDataTable(dsInitLoadDetails.Tables[4], new string[] { "AC_ID", "AC_DESC" });
                //ddlAssetCategory.Items.RemoveAt(0);
                //ddlAssetCategory.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--ALL--", "0")));
                //if (ddlAssetCategory.SelectedValue == "0")
                //{
                //    ddlAssetType.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--ALL--", "0")));
                //    ddlAssetType.Enabled = true;
                //}
                //else
                //{
                //    if (ddlAssetType.Items.Count == 0)
                //        ddlAssetType.Enabled = false;
                //    else
                //        ddlAssetType.Enabled = true;
                //}
                //ddlAssetCategory.Focus();

                ddlAssetType.BindDataTable(dsInitLoadDetails.Tables[5], new string[] { "AT_DESC", "AT_DESC" });
                ddlAssetType.Items.RemoveAt(0);
                ddlAssetType.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("ALL", "0")));
                ddlAssetType.Focus();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void gvTaxAsset_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //string strAssetCategoryID, strAssetCategory, strTaxAssetID, strFilterCheck;
            string strTaxAssetID, strFilterCheck;

            if (e.CommandName == "Add")
            {
                DropDownList ddlAssetType = gvTaxAsset.FooterRow.FindControl("ddlAssetType") as DropDownList;

                //DataTable dt =(DataTable)ViewState["Tax_AssetDetails"];
                //if (dt.Rows.Count >0)
                //{
                //    if (dt.Rows[0][0].ToString() != "0")
                //    {
                //        Utility.FunShowAlertMsg(this.Page, "You can map only one asset details...!");
                //        return;
                //    }
                //}

                if (ddlAssetType.SelectedIndex >= 0)
                {
                    strTaxAssetID = ddlAssetType.SelectedValue.ToString();

                    DataRow[] drCheck = null;
                    dtTaxAsset = FunPriGetTaxAssetDataTable();
                    string strFilterQuery = "Asset_Type='" + ddlAssetType.SelectedItem.Text + "'";

                    if (strTaxAssetID != string.Empty)
                    {
                        strFilterCheck = "Asset_Type='ALL'";
                        drCheck = dtTaxAsset.Select(strFilterCheck);
                        if (drCheck.Length > 0)
                        {
                            Utility.FunShowAlertMsg(this.Page, "All asset type already mapped..! you cannot map to specific type...!");
                            return;
                        }
                    }
                    else
                    {
                        strFilterCheck = "Asset_Type<>'ALL'";
                        drCheck = dtTaxAsset.Select(strFilterCheck);
                        if (drCheck.Length > 0)
                        {
                            Utility.FunShowAlertMsg(this.Page, "specific type added...! you cannot add All asset type...!");
                            return;
                        }
                    }

                    drCheck = dtTaxAsset.Select(strFilterQuery);
                    if (drCheck.Length > 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Specific record already exist in the grid...!");
                        return;
                    }
                    FunPriInsertTaxAssetDataTable("", ddlAssetType.SelectedValue, "", ddlAssetType.SelectedItem.Text, strTaxAssetID);
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Select The Asset Type");
                    return;
                }
                gvTaxAsset.FooterRow.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvTaxAsset_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtTaxAsset = FunPriGetTaxAssetDataTable();
            dtTaxAsset.Rows.RemoveAt(e.RowIndex);
            ViewState["Tax_AssetDetails"] = dtTaxAsset;
            dtTaxAsset = FunPriGetTaxAssetDataTable();

            if (dtTaxAsset.Rows.Count == 0)
            {
                FunPriInsertTaxAssetDataTable("-1", "-1", "", "", "");
            }
            else
            {
                FunPubBindTaxAsset(dtTaxAsset);
            }
            gvTaxAsset.FooterRow.Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvTaxAssetGr_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            switch (Asset_Type)
            {
                case "1":
                    Asset_Type_Name = "Asset Category";
                    break;
                case "2":
                    Asset_Type_Name = "Asset Class";
                    break;
                case "3":
                    Asset_Type_Name = "Asset Make";
                    break;
                case "4":
                    Asset_Type_Name = "Asset Type";
                    break;
                case "5":
                    Asset_Type_Name = "Asset Model";
                    break;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.ToolTip = Asset_Type_Name;
                //CheckBox CbAssets = (CheckBox)e.Row.FindControl("chkSel");
                //CbAssets.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + gvTaxAsset.ClientID + "','chkAll','chkSel');");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable FunPriGetTaxAssetDataTable()
    {
        try
        {
            if (ViewState["Tax_AssetDetails"] == null)
            {
                dtTaxAsset = new DataTable();
                dtTaxAsset.Columns.Add("Asset_Serial_Number");
                dtTaxAsset.Columns.Add("Tax_Guide_Detail_ID");
                dtTaxAsset.Columns.Add("Asset_Category_ID");
                dtTaxAsset.Columns.Add("Asset_Type_ID");
                dtTaxAsset.Columns.Add("Asset_Category");
                dtTaxAsset.Columns.Add("Asset_Type");
                ViewState["Tax_AssetDetails"] = dtTaxAsset;
            }
            dtTaxAsset = (DataTable)ViewState["Tax_AssetDetails"];
            return dtTaxAsset;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriInsertTaxAssetDataTable(string strAssetClass_ID, string strAssetType_ID, string strAssetClass, string strAssetType, string strTaxAsset_ID)
    {
        try
        {
            DataRow drEmptyRow;
            dtTaxAsset = FunPriGetTaxAssetDataTable();

            if (strAssetClass_ID == "-1")
            {
                if (dtTaxAsset.Rows.Count == 0)
                {
                    drEmptyRow = dtTaxAsset.NewRow();
                    drEmptyRow["Asset_Serial_Number"] = "0";
                    dtTaxAsset.Rows.Add(drEmptyRow);
                }
            }
            else
            {
                drEmptyRow = dtTaxAsset.NewRow();
                drEmptyRow["Asset_Serial_Number"] = Convert.ToInt32(dtTaxAsset.Rows[dtTaxAsset.Rows.Count - 1]["Asset_Serial_Number"]) + 1;
                //drEmptyRow["Tax_Guide_Detail_ID"] = 0;
                //drEmptyRow["Asset_Category_ID"] = strAssetClass_ID;
                // drEmptyRow["Asset_Type_ID"] = strAssetType_ID;
                //drEmptyRow["Asset_Category"] = strAssetClass;
                drEmptyRow["Asset_Type"] = strAssetType;
                dtTaxAsset.Rows.Add(drEmptyRow);
            }


            if (dtTaxAsset.Rows.Count > 1)
            {
                if (dtTaxAsset.Rows[0]["Asset_Serial_Number"].ToString() == "0")
                {
                    dtTaxAsset.Rows[0].Delete();
                }
            }

            ViewState["Tax_AssetDetails"] = dtTaxAsset;

            dtTaxAsset = FunPriGetTaxAssetDataTable();
            FunPubBindTaxAsset(dtTaxAsset);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriInsertTaxAssetHistoryDataTable()
    {
        try
        {
            DataRow drEmptyRow;
            dtTaxAssetHistory = FunPubGetTaxAssetHistoryDataTable();

            if (dtTaxAssetHistory.Rows.Count == 0)
            {
                drEmptyRow = dtTaxAssetHistory.NewRow();
                drEmptyRow["Asset_Serial_Number"] = "0";
                dtTaxAssetHistory.Rows.Add(drEmptyRow);
            }

            if (dtTaxAssetHistory.Rows.Count > 1)
            {
                if (dtTaxAssetHistory.Rows[0]["Asset_Serial_Number"] == "0")
                {
                    dtTaxAssetHistory.Rows[0].Delete();
                }
            }

            ViewState["Tax_AssetHistoryDetails"] = dtTaxAssetHistory;

            dtTaxAssetHistory = FunPubGetTaxAssetHistoryDataTable();
            FunPubBindTaxAssetHistory(dtTaxAssetHistory);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPubBindTaxAsset(DataTable dtTaxAsset)
    {
        try
        {
            gvTaxAsset.DataSource = dtTaxAsset;
            gvTaxAsset.DataBind();

            if (dtTaxAsset.Rows.Count > 0)
            {
                if (dtTaxAsset.Rows[0]["Asset_Serial_Number"].ToString() == "0")
                    gvTaxAsset.Rows[0].Visible = false;
                else
                    gvTaxAsset.FooterRow.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPubBindTaxAssetHistory(DataTable dtTaxAssetHistory)
    {
        try
        {
            gvTaxAssetHistory.DataSource = dtTaxAssetHistory;
            gvTaxAssetHistory.DataBind();
            if (dtTaxAssetHistory.Rows.Count > 0)
            {
                if (dtTaxAssetHistory.Rows[0]["Asset_Serial_Number"].ToString() == "0")
                {
                    gvTaxAssetHistory.Rows[0].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool FunPriGenerateTaxAssetXMLDet()
    {
        try
        {
            dtTaxAsset = (DataTable)ViewState["Tax_AssetDetails"];

            if (dtTaxAsset.Rows.Count == 1)
            {
                if (dtTaxAsset.Rows[0]["Asset_Serial_Number"].ToString() == "0" && PageMode == PageModes.Create)
                {
                    return false;
                }
            }
            strbTaxAssetDet.Append("<Root>");
            foreach (DataRow drow in dtTaxAsset.Rows)
            {
                strbTaxAssetDet.Append("<Details ");
                //strbTaxAssetDet.Append(" Tax_Guide_Detail_ID = '" + drow["Tax_Guide_Detail_ID"].ToString() + "'");
                //strbTaxAssetDet.Append(" Asset_Category_ID = '" + drow["Asset_Category_ID"].ToString() + "'");
                //strbTaxAssetDet.Append(" Asset_Type_ID = '" + drow["Asset_Type_ID"].ToString() + "'");
                strbTaxAssetDet.Append(" Asset_Type_Desc = '" + drow["Asset_Type"].ToString() + "'");
                strbTaxAssetDet.Append(" />");
            }
            strbTaxAssetDet.Append("</Root>");
            strXMLTaxAssetDet = strbTaxAssetDet.ToString();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region CopyProfile

    private void FunGetAssetClass()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@type", "1");
        ddlFromAsset.BindDataTable("S3G_Org_GetHSN_List", Procparam, new string[] { "Class_ID", "Class_Desc" });
        ddlToAsset.BindDataTable("S3G_Org_GetHSN_List", Procparam, new string[] { "Class_ID", "Class_Desc" });
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        if (ddlFromAsset.SelectedValue == ddlToAsset.SelectedValue)
        {
            Utility.FunShowAlertMsg(this, "From and To Asset cannot be Same");
            return;
        }

        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm.Add("@CompanyID", intCompanyID.ToString());
        Procparm.Add("@FromAsset", ddlFromAsset.SelectedValue);
        Procparm.Add("@ToAsset", ddlToAsset.SelectedValue);
        DataTable dt = Utility.GetDefaultData("S3G_InsertTaxGuideByCopyProfile", Procparm);
        if (dt.Rows[0][0].ToString() == "-1")
        {
            Utility.FunShowAlertMsg(this, "Tax not defined for selected 'From Asset'");
            return;
        }
        else if (dt.Rows[0][0].ToString() == "0")
        {
            Utility.FunShowAlertMsg(this, "Tax already defined for selected 'To Asset'");
            return;
        }
        else if (dt.Rows[0][0].ToString() == "1")
        {
            Utility.FunShowAlertMsg(this, "Tax details copied successfully from " + ddlFromAsset.SelectedItem.ToString() + "to" + ddlToAsset.SelectedItem.ToString());
            //Response.Redirect(strRedirectPage, false);
            //return;
        }


    }

    [System.Web.Services.WebMethod]
    public static string[] GetHSNCode(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(ojb_TransLander.intCompanyID));
        Procparam.Add("@Option", "4");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GETTAXGUIDE_TRANSVALUES", Procparam));


        return suggetions.ToArray();
    }

    #endregion

}