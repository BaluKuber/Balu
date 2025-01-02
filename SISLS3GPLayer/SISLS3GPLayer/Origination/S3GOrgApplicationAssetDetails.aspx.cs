#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Asset Details
/// Reason              : Asset Details For Application Processing
/// Created By  		: Thangam M
/// Created Date        : 29-Oct-2010
/// Modified By  		: Prabhu K
/// Modified on         : 30-Nov-2011
/// Reason              : For OL Line of Business, Some fields are only required. Capital & Non-Capital calculated automatically.
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using S3GBusEntity;
using System.Globalization;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Configuration;
#endregion

public partial class Origination_S3G_OrgApplicationAssetDetails : ApplyThemeForProject
{
    #region Initialization

    /// <summary>
    /// To Initialize Objects and Variables
    /// </summary>
    /// 
    int intCompanyId = 0;
    int intUserId = 0;
    int intBranchId = 2;
    int intSerialNo = 0;
    int intErrorCode = 0;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    UserInfo ObjUserInfo;
    S3GSession objS3GSession;
    //User Authorization
    string strMode = string.Empty;
    string strDateFormat = "";
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    string[] ErrorList = new string[3];
    public static Origination_S3G_OrgApplicationAssetDetails obj_Page;
    string strNewWin = " window.showModalDialog('../Origination/S3GOrgApplicationAssetDetails.aspx?qsMaster=Yes&qsRowID=";
    string NewWinAttributes = "', 'Asset Details', 'location:no;toolbar:no;menubar:no;dialogwidth:700px;dialogHeight:400px;');";
    //Code end
    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        objS3GSession = new S3GSession();
        obj_Page = this;
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        strDateFormat = objS3GSession.ProDateFormatRW;
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
        //Code end
       

        if(Request.QueryString["qsMode"] != null)                               
        {
            btnOK.Enabled = false;
        }

        if (Request.QueryString["qsRowID"] != null)                               //When click Add for First Row//
        {
            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);
        }
        else                                                                      //When click Add for Further Rows//  
        {
            DataTable dtAssetDetails = (DataTable)Session["PricingAssetDetails"];
            if (dtAssetDetails != null && dtAssetDetails.Rows.Count > 0)
            {
                txtSlNo.Text = Convert.ToString(dtAssetDetails.Rows.Count + 1);
            }
        }
        txtRequiredFromDate_CalendarExtender.Format = strDateFormat;
        txtTotalAssetValue.Attributes.Add("readonly", "readonly");
        //txtRequiredFromDate.Attributes.Add("readonly", "readonly");        
        txtUnitCount.SetDecimalPrefixSuffix(3, 0, true, "Unit Count");
        txtUnitValue.SetDecimalPrefixSuffix(10, 2, true,"Unit Value");
        txtMarginPercentage.SetDecimalPrefixSuffix(3, 2, false,"Margin %");
        txtMarginAmountAsset.SetDecimalPrefixSuffix(10, 2, false,"Margin Amount");
        txtFinanceAmountAsset.SetDecimalPrefixSuffix(10,0,true, "Finance Amount");
        txtCapitalPortion.SetDecimalPrefixSuffix(10,0,true, "Capital Portion");
        txtNonCapitalPortion.SetDecimalPrefixSuffix(10, 0, false, "Non Capital Portion");

        txtDiscountAmount.SetDecimalPrefixSuffix(10, 2, false, "Discount Amount");
        HdnGPSDecimal.Value = objS3GSession.ProGpsSuffixRW.ToString();
        
        if (Request.QueryString["qsMaster"].ToString() == "Yes")
        {
            if (rdnlAssetType.SelectedIndex == 1)
            {
                lblCapitalPortion.CssClass = lblNonCapitalPortion.CssClass =
            lblFinanceAmountAsset0.CssClass = "styleDisplayLabel";
            }
            else
            {
                lblCapitalPortion.CssClass = lblNonCapitalPortion.CssClass = lblFinanceAmountAsset0.CssClass = "styleReqFieldLabel";
            }
            rfvCustomerName.Enabled = rfvPayTo.Enabled = false;

            lblPayTo.CssClass = lblCustomerName.CssClass = "styleDisplayLabel";
        }
        else
        {
            rfvCustomerName.Enabled = rfvPayTo.Enabled = true;
            lblCapitalPortion.CssClass = lblNonCapitalPortion.CssClass =
           lblFinanceAmountAsset0.CssClass = lblPayTo.CssClass = lblCustomerName.CssClass = "styleReqFieldLabel";
        }
        if (!IsPostBack)
        {
            FunProLoadCombos();
            FunToggleLableVisble(false);
            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);
            if (Request.QueryString["qsMaster"].ToString() == "Yes")
            {
                FunToggleLeaseAssetCode(true);
                txtUnitCount.Enabled = false;
               
            }
            else
            {
                FunToggleLeaseAssetCode(false);
                
            }
            if (string.IsNullOrEmpty(txtSlNo.Text))
            
            {
                DataTable dtAssetDetails = (DataTable)Session["PricingAssetDetails"];
                if (dtAssetDetails != null && dtAssetDetails.Rows.Count > 0)
                {
                    FunPriLoadAssetDetails(dtAssetDetails);
                }
                else
                {
                    txtSlNo.Text = "1";
                }
            }
        }
        /* these code executes for .Net 3.0 and above only.*/
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");

    }

    protected new void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.QueryString["qsMaster"] != null)
        {
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
        }
        else
        {
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
        }
    }

    #endregion

    #region Page Events

    protected void rdnlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBookDepreciationPerc.Text = "";
        txtBlockDepreciationPerc.Text = "";
        txtCapitalPortion.Text = "";
        txtNonCapitalPortion.Text = "";
        txtUnitValue.Text = "";
        txtFinanceAmountAsset.Text = "";
        txtMarginAmountAsset.Text = "";
        txtMarginPercentage.Text = "";
        txtTotalAssetValue.Text = "";
        txtRequiredFromDate.Text = "";        
        if (rdnlAssetType.SelectedValue == "1")
        {
            FunProLoadAssetValue("OLD");
            FunToggleLableVisble(true);
            ddlLeaseAssetNo.SelectedIndex = 0;
            lblRequiredFromDate.Enabled = true;
            lblRequiredFromDate.CssClass = "styleReqFieldLabel";
            rfvRequiredFromDate.Enabled = true;
            txtRequiredFromDate_CalendarExtender.Enabled = true;
        }
        else
        {
            FunProLoadAssetValue("NEW");
            FunToggleLableVisble(false);
            ddlAssetCodeList.SelectedIndex = 0;
            lblRequiredFromDate.Enabled = false;
            lblRequiredFromDate.CssClass = "styleDisplayLabel";
            rfvRequiredFromDate.Enabled = false;
            txtRequiredFromDate_CalendarExtender.Enabled = false;
        }
    }

    protected void ddlPayTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayTo.SelectedItem.ToString().ToLower() == "entity")
        {
            FunToggleEntityControls(true);
        }
        else if (ddlPayTo.SelectedItem.ToString().ToLower() == "customer")
        {
            FunToggleEntityControls(false);
        }
        else
        {
            txtCustomerName.Text = "";

        }
        ddlPayTo.Focus();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (rdnlAssetType.SelectedIndex == 0 && Convert.ToDouble(txtFinanceAmountAsset.Text) > Convert.ToDouble(txtTotalAssetValue.Text))
        {
            cvApplicationAsset.ErrorMessage = @"Correct the following validation(s): </br></br>" + "     Finance Amount should be less than or equal to Total Asset Value";
            cvApplicationAsset.IsValid = false;
            return;
        }
        if (Request.QueryString["qsMaster"].ToString() == "No")
        {
            if (txtMarginAmountAsset.Text != "")
            {
                double douFinanceMarginAmount = Convert.ToDouble(txtFinanceAmountAsset.Text) + Convert.ToDouble(txtMarginAmountAsset.Text);
                if (douFinanceMarginAmount > Convert.ToDouble(txtTotalAssetValue.Text))
                {
                    cvApplicationAsset.ErrorMessage = @"Correct the following validation(s): </br></br>" + "    The sum of Finance Amount and Margin Amount should be less than or equal to Total Asset Value";
                    cvApplicationAsset.IsValid = false;
                    return;
                }
            }
        }
        if (Convert.ToDouble(txtCapitalPortion.Text) > Convert.ToDouble(txtFinanceAmountAsset.Text))
        {
            cvApplicationAsset.ErrorMessage = @"Correct the following validation(s): </br></br>" + "     Capital Amount should be less than or equal to Finance Amount";
            cvApplicationAsset.IsValid = false;
            return;
        }
        
        if (Convert.ToDouble(txtNonCapitalPortion.Text) > Convert.ToDouble(txtFinanceAmountAsset.Text))
        {
            cvApplicationAsset.Text = @"Correct the following validation(s): </br></br>" + "        Non Capital Amount should be less than or equal to Finance Amount";
            cvApplicationAsset.IsValid = false;
            return;
        }
        if (Convert.ToDouble(txtNonCapitalPortion.Text) + Convert.ToDouble(txtCapitalPortion.Text) != Convert.ToDouble(txtFinanceAmountAsset.Text))
        {
            cvApplicationAsset.Text = @"Correct the following validation(s): </br></br>" +  "        The sum of Capital and Non Capital Amounts should be equal to Finance Amount.";
            cvApplicationAsset.IsValid = false;
            return;
        }
        if (ddlLeaseAssetNo.Visible)
        {
            if (Request.QueryString["FromPricing"]== null)
            {
                DataRow[] DrRate = (((DataTable)ViewState["RateDt2"]).Select("LEASE_ASSET_NO = '" + ddlLeaseAssetNo.SelectedValue + "'"));
                if (DrRate.Count() > 0)
                {
                    if (Convert.ToString(DrRate[0]["AVAILABLE_DATE"]) != string.Empty)
                    {
                        //condition =< modified as >=for bug fixing - kuppu
                        if (Utility.StringToDate(txtRequiredFromDate.Text) >= Utility.StringToDate(Convert.ToString(DrRate[0]["AVAILABLE_DATE"])))
                        {
                            cvApplicationAsset.Text = @"Correct the following validation(s): </br><br/>" + "     The Asset is not available on the selected date";
                            cvApplicationAsset.IsValid = false;
                            return;
                        }
                    }
                }
            }
        }
        DataTable dtAssetDetails = new DataTable();
        if (Session["PricingAssetDetails"] == null)
        {
            dtAssetDetails.Columns.Add("LeaseType");
            dtAssetDetails.Columns.Add("Required_FromDate");
            dtAssetDetails.Columns.Add("SlNo");
            dtAssetDetails.Columns.Add("Asset_ID");
            dtAssetDetails.Columns.Add("Asset_Code");
            dtAssetDetails.Columns.Add("Unit_Value");
            dtAssetDetails.Columns.Add("Noof_Units");
            dtAssetDetails.Columns.Add("Margin_Percentage");
            dtAssetDetails.Columns.Add("TotalAssetValue");
            dtAssetDetails.Columns.Add("Book_depreciation_Percentage");
            dtAssetDetails.Columns.Add("Margin_Amount");
            dtAssetDetails.Columns.Add("Block_depreciation_Percentage");
            dtAssetDetails.Columns.Add("Finance_Amount");
            dtAssetDetails.Columns.Add("NonCapital_Portion");
            dtAssetDetails.Columns.Add("Capital_Portion");
            dtAssetDetails.Columns.Add("Payment_Percentage");
            dtAssetDetails.Columns.Add("Pay_To_ID");
            dtAssetDetails.Columns.Add("Entity_ID");
            dtAssetDetails.Columns.Add("Entity_Code");
            dtAssetDetails.Columns.Add("Proforma_Id");
            dtAssetDetails.Columns["Finance_Amount"].DataType = typeof(decimal);
            dtAssetDetails.Columns["Margin_Amount"].DataType = typeof(decimal);
            dtAssetDetails.Columns["TotalAssetValue"].DataType = typeof(decimal);
            dtAssetDetails.Columns.Add("Lease_Asset_No");
            //new column added 
            dtAssetDetails.Columns.Add("Discount_Absorbed");
            dtAssetDetails.Columns.Add("Discount_Amount");
        }
        else
        {
            dtAssetDetails = (DataTable)Session["PricingAssetDetails"];
        }

        if (intSerialNo == 0)
        {
           
            DataRow Dr = dtAssetDetails.NewRow();
            Dr["LeaseType"] = rdnlAssetType.SelectedValue.ToString();
            Dr["SlNo"] = dtAssetDetails.Rows.Count + 1;
            Dr["Asset_Code"] = ddlAssetCodeList.SelectedItem.Text;
            Dr["Asset_ID"] = ddlAssetCodeList.SelectedValue;
            if (rdnlAssetType.SelectedValue == "0")
            {
                Dr["Lease_Asset_No"] = "";
            }
            else
            {
                Dr["Lease_Asset_No"] = ddlLeaseAssetNo.SelectedValue;
            }
            //if (ddlAssetCodeList.Visible)
            //{
            //    Dr["Asset_Code"] = ddlAssetCodeList.SelectedItem.Text;
            //    Dr["Asset_ID"] = ddlAssetCodeList.SelectedValue;
            //}
            //else
            //{
            //    DataRow[] drLAN = ((DataTable)ViewState["RateDt2"]).Select("Asset_Code = '" + ddlLeaseAssetNo.SelectedItem.Text + "'");
            //    if (drLAN.Length > 0)
            //    {
            //        Dr["Asset_Code"] = ddlLeaseAssetNo.SelectedItem.Text;
            //        Dr["Asset_ID"] = drLAN[0]["LEASE_ASSET_ID"];
            //    }
            //}
            Dr["Noof_Units"] = txtUnitCount.Text;
            Dr["Finance_Amount"] = txtFinanceAmountAsset.Text;
            if (!string.IsNullOrEmpty(txtRequiredFromDate.Text))
            {
                Dr["Required_FromDate"] = Utility.StringToDate(txtRequiredFromDate.Text);
            }
            else
            {
                //Dr["Required_FromDate"] = DBNull;
            }
            Dr["Unit_Value"] = txtUnitValue.Text;
            Dr["Margin_Percentage"] = string.IsNullOrEmpty(txtMarginPercentage.Text) ? "0" : txtMarginPercentage.Text;
            Dr["TotalAssetValue"] = Convert.ToDecimal(txtUnitValue.Text) * Convert.ToDecimal(txtUnitCount.Text);
            Dr["Book_depreciation_Percentage"] = string.IsNullOrEmpty(txtBookDepreciationPerc.Text) ? "0" : txtBookDepreciationPerc.Text;
            Dr["Margin_Amount"] = string.IsNullOrEmpty(txtMarginAmountAsset.Text) ? 0 : Convert.ToDecimal(txtMarginAmountAsset.Text);
            Dr["Block_depreciation_Percentage"] = string.IsNullOrEmpty(txtBlockDepreciationPerc.Text) ? "0" : txtBlockDepreciationPerc.Text;
            Dr["NonCapital_Portion"] = txtNonCapitalPortion.Text;
            Dr["Capital_Portion"] = txtCapitalPortion.Text;
            Dr["Payment_Percentage"] = txtPaymentPercentage.Text;
            /*Changed by Prabhu.K on 30-Nov-2011 - For OL , it is not required*/
            if (ddlPayTo.SelectedIndex == 0)
            {
                Dr["Pay_To_ID"] = Dr["Entity_ID"] = Dr["Entity_Code"] = "";
            }
            else
            {
                Dr["Pay_To_ID"] = ddlPayTo.SelectedValue;
                if (ddlPayTo.SelectedItem.ToString().ToUpper() == "CUSTOMER")
                {
                    Dr["Entity_ID"] = Session["AssetCustomer"].ToString().Substring(0, Session["AssetCustomer"].ToString().Remove(Session["AssetCustomer"].ToString().IndexOf(";")).Length);
                }
                else
                {
                    Dr["Entity_ID"] = ddlEntityNameList.SelectedValue;
                    Dr["Entity_Code"] = ddlEntityNameList.SelectedText;
                }
            }

              Dr["Discount_Amount"] = txtDiscountAmount.Text;
            if(chkDiscountAbsorbed.Checked)
                Dr["Discount_Absorbed"] = 1;
            else
                Dr["Discount_Absorbed"] = 0;
            


            dtAssetDetails.Rows.Add(Dr);
        }
        else
        {
            DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + intSerialNo.ToString());
            drAsset[0]["LeaseType"] = rdnlAssetType.SelectedValue.ToString();
            drAsset[0]["Asset_Code"] = ddlAssetCodeList.SelectedItem.Text;
            drAsset[0]["Asset_ID"] = ddlAssetCodeList.SelectedValue;
            if (rdnlAssetType.SelectedValue == "0")
            {
                drAsset[0]["Lease_Asset_No"] = "";
            }
            else
            {
                drAsset[0]["Lease_Asset_No"] = ddlLeaseAssetNo.SelectedItem.Text;
                
            }
            drAsset[0]["Noof_Units"] = txtUnitCount.Text;
            drAsset[0]["Finance_Amount"] = string.IsNullOrEmpty(txtFinanceAmountAsset.Text.Trim()) ? "0" : txtFinanceAmountAsset.Text.Trim();
            if (!string.IsNullOrEmpty(txtRequiredFromDate.Text))
            {
                drAsset[0]["Required_FromDate"] = Utility.StringToDate(txtRequiredFromDate.Text);
            }
            else
            {
                drAsset[0]["Required_FromDate"] = DBNull.Value;
            }
            drAsset[0]["Unit_Value"] = txtUnitValue.Text;
            if (txtMarginPercentage.Text != "")
            {
                drAsset[0]["Margin_Percentage"] = txtMarginPercentage.Text;
            }
            drAsset[0]["TotalAssetValue"] = Convert.ToDecimal(txtUnitValue.Text) * Convert.ToDecimal(txtUnitCount.Text);
            drAsset[0]["Book_depreciation_Percentage"] = string.IsNullOrEmpty(txtBlockDepreciationPerc.Text) ? "0" : txtBlockDepreciationPerc.Text;
            drAsset[0]["Margin_Amount"] = string.IsNullOrEmpty(txtMarginAmountAsset.Text) ? 0 :Convert.ToDecimal(txtMarginAmountAsset.Text);
            drAsset[0]["Block_depreciation_Percentage"] = string.IsNullOrEmpty(txtBlockDepreciationPerc.Text) ? "0" : txtBookDepreciationPerc.Text;
            drAsset[0]["NonCapital_Portion"] = txtNonCapitalPortion.Text;
            drAsset[0]["Capital_Portion"] = txtCapitalPortion.Text;
            if (txtPaymentPercentage.Text != "")
            {
                drAsset[0]["Payment_Percentage"] = txtPaymentPercentage.Text;
            }
            /*Changed by Prabhu.K on 30-Nov-2011 - For OL , it is not required*/
            if (ddlPayTo.SelectedIndex == 0)
            {
                drAsset[0]["Pay_To_ID"] = drAsset[0]["Entity_ID"] = drAsset[0]["Entity_Code"] = "";
            }
            else
            {
                drAsset[0]["Pay_To_ID"] = ddlPayTo.SelectedValue.ToString();

                if (ddlPayTo.SelectedItem.ToString().ToUpper() == "CUSTOMER")
                {
                    drAsset[0]["Entity_ID"] = Session["AssetCustomer"].ToString().Substring(0, Session["AssetCustomer"].ToString().Remove(Session["AssetCustomer"].ToString().IndexOf(";")).Length);
                }
                else
                {
                    drAsset[0]["Entity_ID"] = ddlEntityNameList.SelectedValue.ToString();
                    drAsset[0]["Entity_Code"] = ddlEntityNameList.SelectedText.ToString();
                }
            }

            drAsset[0]["Discount_Amount"] = txtDiscountAmount.Text;
            if (chkDiscountAbsorbed.Checked)
                drAsset[0]["Discount_Absorbed"] = 1;
            else
                drAsset[0]["Discount_Absorbed"] = 0;

            drAsset[0].AcceptChanges();
        }
        Session["PricingAssetDetails"] = dtAssetDetails;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
    }

    protected void ddlAssetCodeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBookDepreciationPerc.Text = "";
        txtBlockDepreciationPerc.Text = "";
        txtCapitalPortion.Text = "";
        txtNonCapitalPortion.Text = "";
        txtUnitValue.Text = "";
        txtFinanceAmountAsset.Text = "";
        txtMarginAmountAsset.Text = "";
        txtMarginPercentage.Text = "";
        txtTotalAssetValue.Text = "";
        //txtRequiredFromDate.Text = "";
        ViewState["AssetAvailDate"] = string.Empty;
        FunFillDepreciationRate((DataTable)ViewState["RateDt1"], ddlAssetCodeList);
        ddlAssetCodeList.ToolTip = ddlAssetCodeList.SelectedItem.Text;
        ddlAssetCodeList.Focus();
        if (rdnlAssetType.SelectedValue == "1")
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@OPTION", "3");
            dictParam.Add("@COMPANYID", intCompanyId.ToString());
            dictParam.Add("@AssetID", ddlAssetCodeList.SelectedValue);
            DataTable DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
            ViewState["RateDt2"] = DtRate;
            ddlLeaseAssetNo.DataSource = DtRate;
            ddlLeaseAssetNo.DataTextField = "LEASE_ASSET_NO";
            ddlLeaseAssetNo.DataValueField = "LEASE_ASSET_NO";
            ddlLeaseAssetNo.DataBind();
            ddlLeaseAssetNo.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        if (ddlAssetCodeList.SelectedValue == "0")
        {
            DataTable DtRate = new DataTable();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@OPTION", "1");
            dictParam.Add("@COMPANYID", intCompanyId.ToString());
            DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
            ddlLeaseAssetNo.DataSource = DtRate;
            ddlLeaseAssetNo.DataTextField = "LEASE_ASSET_NO";
            ddlLeaseAssetNo.DataValueField = "LEASE_ASSET_NO";
            ddlLeaseAssetNo.DataBind();
            ddlLeaseAssetNo.Items.Insert(0, new ListItem("--Select--", "0"));
        }

    }

    protected void ddlLeaseAssetNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        if (ddlLeaseAssetNo.SelectedIndex > 0)
        {
            dictParam.Add("@OPTION", "4");
            dictParam.Add("@COMPANYID", intCompanyId.ToString());
            dictParam.Add("@LeaseAssetNo", ddlLeaseAssetNo.SelectedValue);
            DataTable DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
            txtTotalAssetValue.Text = txtUnitValue.Text = DtRate.Rows[0]["WDV"].ToString();
            //Changed by Thangam M on 24/Oct/2012 to round off Finance amount
            if (!string.IsNullOrEmpty(DtRate.Rows[0]["WDV"].ToString()))
            {
                txtCapitalPortion.Text = txtFinanceAmountAsset.Text = Math.Round(Convert.ToDouble(DtRate.Rows[0]["WDV"].ToString())).ToString();
            }
            else
            {
                txtCapitalPortion.Text = txtFinanceAmountAsset.Text = string.Empty;
            }
            //End here
            ddlAssetCodeList.SelectedValue = DtRate.Rows[0]["Asset_Code"].ToString();
            txtUnitValue.ReadOnly = true;
        }
        else
        {
            txtTotalAssetValue.Text = txtUnitValue.Text = "";
            txtUnitValue.ReadOnly = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// Method to load DropDownList & Ajax combos
    /// </summary>
    /// 

    protected void FunProLoadCombos()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        ObjStatus.Option = 1;
        ObjStatus.Param1 = S3G_Statu_Lookup.PAY_TO.ToString();
        Utility.FillDLL(ddlPayTo, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

        DataTable DtRate = new DataTable();
        Dictionary<string, string> dictParam = new Dictionary<string, string>();

        FunProLoadAssetValue("NEW");

        //dictParam.Add("@OPTION", "2");
        //dictParam.Add("@COMPANYID", intCompanyId.ToString());
        //DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
        //ddlAssetCodeList.DataSource = DtRate;
        //ddlAssetCodeList.DataTextField = "Asset_Code";
        //ddlAssetCodeList.DataValueField = "Asset_ID";
        //ddlAssetCodeList.DataBind();
        //ddlAssetCodeList.Items.Insert(0, new ListItem("--Select--", "0"));

        //ViewState["RateDt1"] = DtRate;

        //dictParam = new Dictionary<string, string>();

        dictParam.Add("@OPTION", "1");
        dictParam.Add("@COMPANYID", intCompanyId.ToString());
        DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
        ddlLeaseAssetNo.DataSource = DtRate;
        ddlLeaseAssetNo.DataTextField = "LEASE_ASSET_NO";
        ddlLeaseAssetNo.DataValueField = "LEASE_ASSET_NO";
        ddlLeaseAssetNo.DataBind();
        ddlLeaseAssetNo.Items.Insert(0, new ListItem("--Select--", "0"));
        ViewState["RateDt2"] = DtRate;
        dictParam.Clear();
        dictParam.Add("@Option", "11");
        dictParam.Add("@Company_ID", intCompanyId.ToString());
        dictParam.Add("@ID", "1");
        //ddlEntityNameList.BindDataTable(SPNames.S3G_ORG_GetPricing_List, dictParam, true, new string[] { "Entity_ID", "Entity_Code", "Entity_Name" });

        //ObjStatus.Option = 1;
        //ObjStatus.Param1 = intCompanyId.ToString();
        //ObjStatus.Param2 
        //Utility.FillDLL(ddlEntityNameList, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus), true);
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Entity_Type", "8");
        Procparam.Add("@PrefixText", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));

        return suggetions.ToArray();

    }

    private void FunProLoadAssetValue(string StrAssetType)
    {
        DataTable DtRate = new DataTable();
        Dictionary<string, string> dictParam = new Dictionary<string, string>();

        if (StrAssetType.ToUpper() == "NEW")
            dictParam.Add("@OPTION", "2");
        else
            dictParam.Add("@OPTION", "6");

        dictParam.Add("@COMPANYID", intCompanyId.ToString());
        DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
        ddlAssetCodeList.DataSource = DtRate;
        ddlAssetCodeList.DataTextField = "Asset_Code";
        ddlAssetCodeList.DataValueField = "Asset_ID";
        ddlAssetCodeList.DataBind();
        ddlAssetCodeList.Items.Insert(0, new ListItem("--Select--", "0"));

        ViewState["RateDt1"] = DtRate;
    }

    public void FunFillDepreciationRate(DataTable RateDt, DropDownList ddlOption)
    {
        txtBookDepreciationPerc.Text = "";
        txtBlockDepreciationPerc.Text = "";

        if (Request.QueryString["qsMaster"] != null)
        {
            if (Request.QueryString["qsMaster"].ToString() == "Yes")
            {
                DataRow[] DrRate = (RateDt.Select("Asset_ID = '" + ddlOption.SelectedValue.ToString() + "'"));

                if (DrRate.Count() > 0)
                {
                    txtBookDepreciationPerc.Text = DrRate[0]["Book_Depreciation_Rate"].ToString();
                    txtBlockDepreciationPerc.Text = DrRate[0]["Block_Depreciation_Rate"].ToString();
                }
            }
        }
    }

    protected void FunToggleLeaseAssetCode(bool Canshow)
    {
        rdnlAssetType.Visible = Canshow;
    }

    protected void FunFillControls()
    {

    }

    public void FunToggleLableVisble(bool CanShow)
    {
        lblLeaseAssetNo.Visible = CanShow;
        ddlLeaseAssetNo.Visible = CanShow;
        rfvLeastAssetCodeNo.Enabled = CanShow;

        //lblAssetCodeList.Visible = !CanShow;
        //ddlAssetCodeList.Visible = !CanShow;
        //rfvAssetCodeList.Enabled = !CanShow;

        rfvRequiredFromDate.Enabled = CanShow;
        if (CanShow)
        {
            txtRequiredFromDate.ReadOnly = false;
        }
        else
        {
            txtRequiredFromDate.ReadOnly = true;
            txtRequiredFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtRequiredFromDate.ClientID + "','" + strDateFormat + "',false,true);");
        }
        txtRequiredFromDate_CalendarExtender.Enabled = CanShow;
    }

    protected void FunToggleEntityControls(bool CanShow)
    {
        txtCustomerName.Text = "";
        if (Session["AssetCustomer"] != null)
        {
            txtCustomerName.Text = Session["AssetCustomer"].ToString().Substring(Session["AssetCustomer"].ToString().IndexOf(";") + 1);
        }

        ddlEntityNameList.Visible = ddlEntityNameList.IsMandatory = CanShow;
        lblEntityNameList.Visible = CanShow;
        //rfvEntityNameList.Enabled = CanShow;

        txtCustomerName.Visible = !CanShow;
        lblCustomerName.Visible = !CanShow;
        rfvCustomerName.Enabled = !CanShow;
    }
    private void FunPriLoadAssetDetails(DataTable dtAssetDetails)
    {
        DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + intSerialNo.ToString());
        if (drAsset.Length == 0)
        {
            txtSlNo.Text = Convert.ToString(dtAssetDetails.Rows.Count + 1);
            return;
        }
        if (intSerialNo == 0)
        {
            txtSlNo.Text = (intSerialNo + 1).ToString();
        }
        else
        {
            txtSlNo.Text = (intSerialNo).ToString();
        }
        rdnlAssetType.SelectedValue = drAsset[0]["LeaseType"].ToString();
        ddlAssetCodeList.SelectedValue = drAsset[0]["Asset_ID"].ToString();
        if (rdnlAssetType.SelectedValue == "1")
        {
            ddlLeaseAssetNo.SelectedValue = drAsset[0]["Lease_Asset_No"].ToString();
            FunToggleLableVisble(true);
            txtRequiredFromDate.Text = DateTime.Parse(drAsset[0]["Required_FromDate"].ToString(), CultureInfo.CurrentCulture).ToString(objS3GSession.ProDateFormatRW);
        }
        else
        {
            FunToggleLableVisble(false);
            txtRequiredFromDate.Text = string.Empty;
        }
        
        txtUnitCount.Text = drAsset[0]["Noof_Units"].ToString();
        txtFinanceAmountAsset.Text = drAsset[0]["Finance_Amount"].ToString();
        txtCapitalPortion.Text = drAsset[0]["Capital_Portion"].ToString();
        txtNonCapitalPortion.Text = drAsset[0]["NonCapital_Portion"].ToString();
        txtUnitValue.Text = drAsset[0]["Unit_Value"].ToString();
        txtBookDepreciationPerc.Text = drAsset[0]["Book_depreciation_Percentage"].ToString();
        txtBlockDepreciationPerc.Text = drAsset[0]["Block_depreciation_Percentage"].ToString();
        txtMarginAmountAsset.Text = drAsset[0]["Margin_Amount"].ToString();
        txtMarginPercentage.Text = drAsset[0]["Margin_Percentage"].ToString();
        txtPaymentPercentage.Text = Convert.ToString(drAsset[0]["Payment_Percentage"]);
        txtTotalAssetValue.Text = Convert.ToString(Convert.ToInt32(txtUnitCount.Text) * Convert.ToDouble(txtUnitValue.Text));
        ddlPayTo.SelectedValue = drAsset[0]["Pay_To_ID"].ToString();

        txtDiscountAmount.Text = drAsset[0]["Discount_Amount"].ToString();

        if (drAsset[0]["Discount_Absorbed"].ToString() == "1")
            chkDiscountAbsorbed.Checked = true;

        if (ddlPayTo.SelectedItem.Text.ToUpper() == "ENTITY")
        {
            FunToggleEntityControls(true);
            ddlEntityNameList.SelectedValue = drAsset[0]["Entity_ID"].ToString();
            ddlEntityNameList.SelectedText = drAsset[0]["Entity_Code"].ToString();
        }
        else if (ddlPayTo.SelectedItem.Text.ToUpper() == "CUSTOMER")
        {
            FunToggleEntityControls(false);
            if (Session["AssetCustomer"] != null)
            {
                txtCustomerName.Text = Session["AssetCustomer"].ToString().Substring(Session["AssetCustomer"].ToString().IndexOf(";") + 1);
            }
        }
        if (Request.QueryString["qsMode"] != null)
        {
            rdnlAssetType.Enabled = false;
            ddlAssetCodeList.ClearDropDownList();
            ddlLeaseAssetNo.ClearDropDownList();
            ddlPayTo.ClearDropDownList();
            txtRequiredFromDate_CalendarExtender.Enabled = false;
            txtUnitCount.ReadOnly = txtUnitValue.ReadOnly = txtTotalAssetValue.ReadOnly = txtMarginAmountAsset.ReadOnly =
            txtMarginPercentage.ReadOnly = txtBlockDepreciationPerc.ReadOnly = txtBookDepreciationPerc.ReadOnly =
            txtFinanceAmountAsset.ReadOnly = txtCapitalPortion.ReadOnly = txtNonCapitalPortion.ReadOnly = txtPaymentPercentage.ReadOnly =
            txtSlNo.ReadOnly = txtDiscountAmount.ReadOnly = true;
            txtRequiredFromDate_CalendarExtender.Enabled = false;
            chkDiscountAbsorbed.Enabled = false;
            ddlEntityNameList.ReadOnly = true;
        }
        if (Request.QueryString["FromPricing"] != null)
        {
            rdnlAssetType.Enabled = false;
            ddlAssetCodeList.ClearDropDownList();
            ddlLeaseAssetNo.ClearDropDownList();
        }
       
    }
    protected void txtUnitValue_TextChanged(object sender, EventArgs e)
    {
        txtMarginPercentage.Focus();
        FunPriAssignMarginAmount();
        
    }
    
    protected void txtMarginPercentage_TextChanged(object sender, EventArgs e)
    {
       FunPriAssignMarginAmount();
       txtMarginPercentage.Focus();
    }

    protected void txtUnitCount_TextChanged(object sender, EventArgs e)
    {
        FunPriAssignMarginAmount();
        txtUnitCount.Focus();  
    }

    private string Funsetsuffix()
    {

        int suffix = 1;

        // S3GSession ObjS3GSession = new S3GSession();
        suffix = objS3GSession.ProGpsSuffixRW;
        // suffix = 0;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    private void FunPriAssignMarginAmount()
    {
        if (!string.IsNullOrEmpty(txtMarginPercentage.Text) && !string.IsNullOrEmpty(txtTotalAssetValue.Text))
        {
            decimal dcmTotalAssetValue = Convert.ToDecimal(txtTotalAssetValue.Text);
            decimal dcmMarginPercentage = Convert.ToDecimal(txtMarginPercentage.Text) / 100;
            txtMarginAmountAsset.Text = (dcmTotalAssetValue * dcmMarginPercentage).ToString(Funsetsuffix());

        }
        else
        {
            txtMarginAmountAsset.Text = "";
        }
    }
    #endregion
}

