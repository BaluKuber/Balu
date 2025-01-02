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

public partial class Origination_S3G_OrgApplicationGLAssetDetails : ApplyThemeForProject
{
    #region Initialization

    /// <summary>
    /// To Initialize Objects and Variables
    /// </summary>
    /// 
    int intCompanyId = 0;
    int intUserId = 0;
    string strLocation_ID = "0";
    string strConstitution_ID = "0";
    string strProduct_ID = "0";
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
    Dictionary<string, string> ProcParam;

    string strNewWin = " window.showModalDialog('../Origination/S3GOrgApplicationGLAssetDetails.aspx?qsMaster=Yes&qsRowID=";
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

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        strDateFormat = objS3GSession.ProDateFormatRW;
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
        //Code end

        if (Session["Constitution_ID"] != null)
        strConstitution_ID = Session["Constitution_ID"].ToString();
        if (Session["Product_ID"] != null)
        strProduct_ID = Session["Product_ID"].ToString();
        if (Session["Location_ID"] != null)
        strLocation_ID = Session["Location_ID"].ToString();

        if (Request.QueryString["qsMode"] != null)
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

        txtUnitCount.SetDecimalPrefixSuffix(3, 0, true, "Unit Count");
        HdnGPSDecimal.Value = objS3GSession.ProGpsSuffixRW.ToString();
        txtGrossWeight.SetDecimalPrefixSuffix(5, 4, true, false, "Gross Weight");
        txtNetWeight.SetDecimalPrefixSuffix(5, 4, true, false, "Net Weight");

        if (!IsPostBack)
        {
            FunProLoadCombos();

            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);

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

    protected void btnOK_Click(object sender, EventArgs e)
    {

        DataTable dtAssetDetails = new DataTable();
        if (Session["PricingAssetDetails"] == null)
        {
            dtAssetDetails.Columns.Add("SlNo");
            dtAssetDetails.Columns.Add("Asset_ID");
            dtAssetDetails.Columns.Add("Asset_Code");
            dtAssetDetails.Columns.Add("ValuationDate");
            dtAssetDetails.Columns.Add("MarketValue");
            dtAssetDetails.Columns.Add("Noof_Units");
            dtAssetDetails.Columns.Add("Purity");
            dtAssetDetails.Columns.Add("PurityID");
            dtAssetDetails.Columns.Add("Measurement");
            dtAssetDetails.Columns.Add("MeasurementID");
            dtAssetDetails.Columns.Add("GrossWeight");
            dtAssetDetails.Columns.Add("NetWeight");
            dtAssetDetails.Columns.Add("GrossWeightDisp");
            dtAssetDetails.Columns.Add("NetWeightDisp");
            dtAssetDetails.Columns.Add("PureGoldMass");
            dtAssetDetails.Columns.Add("PureGoldValue");
            dtAssetDetails.Columns.Add("MaxLoan", typeof(decimal));
            dtAssetDetails.Columns.Add("FinanceAmount", typeof(decimal));
            dtAssetDetails.Columns.Add("Rate", typeof(decimal));
        }
        else
        {
            dtAssetDetails = (DataTable)Session["PricingAssetDetails"];
        }

        decimal decLTV = 0;
        decLTV = (Convert.ToDecimal(txtMaxLoan.Text) / Convert.ToDecimal(txtPureGoldValue.Text)) * 100;

        ProcParam = new Dictionary<string, string>();
        ProcParam.Add("@Asset_Code", ddlAssetCodeList.SelectedValue);
        ProcParam.Add("@Company_ID", intCompanyId.ToString());
        ProcParam.Add("@Constitution_ID", strConstitution_ID);
        ProcParam.Add("@Product_ID", strProduct_ID);
        ProcParam.Add("@Location_Code", strLocation_ID);
        ProcParam.Add("@LTV", decLTV.ToString());

        DataTable dTbl = Utility.GetDefaultData("S3G_ORG_GetGLRateMatrixForApp", ProcParam);

        if (dTbl != null && dTbl.Rows.Count > 0)
        {

        }
        else
        {
            Utility.FunShowAlertMsg(this, "LTV rate not defined in Gold Loan Matrix");
            return;
        }

        if (intSerialNo == 0)
        {
            DataRow Dr = dtAssetDetails.NewRow();

            Dr["SlNo"] = txtSlNo.Text;
            Dr["Asset_ID"] = ddlAssetCodeList.SelectedValue;
            Dr["Asset_Code"] = ddlAssetCodeList.SelectedItem.Text;
            Dr["ValuationDate"] = txtValuationDate.Text;
            Dr["MarketValue"] = txtMarketValue.Text;
            Dr["Noof_Units"] = txtUnitCount.Text;
            Dr["Purity"] = ddlPurity.SelectedItem.Text;
            Dr["PurityID"] = ddlPurity.SelectedValue;
            Dr["Measurement"] = txtMeasurement.Text;
            Dr["MeasurementID"] = lblMeasurementID.Text;
            Dr["GrossWeight"] = txtGrossWeight.Text;
            Dr["NetWeight"] = txtNetWeight.Text;
            Dr["GrossWeightDisp"] = txtGrossWeight.Text + " " + txtMeasurement.Text;
            Dr["NetWeightDisp"] = txtNetWeight.Text + " " + txtMeasurement.Text;
            Dr["PureGoldMass"] = txtPureGoldMass.Text;
            Dr["PureGoldValue"] = txtPureGoldValue.Text;
            Dr["MaxLoan"] = lblMaxLoanValue.Text;
            Dr["FinanceAmount"] = txtMaxLoan.Text;
            Dr["Rate"] = dTbl.Rows[0][0].ToString();

            dtAssetDetails.Rows.Add(Dr);
        }
        else
        {
            DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + intSerialNo.ToString());

            drAsset[0]["SlNo"] = txtSlNo.Text;
            drAsset[0]["Asset_ID"] = ddlAssetCodeList.SelectedValue;
            drAsset[0]["Asset_Code"] = ddlAssetCodeList.SelectedItem.Text;
            drAsset[0]["ValuationDate"] = txtValuationDate.Text;
            drAsset[0]["MarketValue"] = txtMarketValue.Text;
            drAsset[0]["Noof_Units"] = txtUnitCount.Text;
            drAsset[0]["Purity"] = ddlPurity.SelectedItem.Text;
            drAsset[0]["PurityID"] = ddlPurity.SelectedValue;
            drAsset[0]["Measurement"] = txtMeasurement.Text;
            drAsset[0]["MeasurementID"] = lblMeasurementID.Text;
            drAsset[0]["GrossWeight"] = txtGrossWeight.Text;
            drAsset[0]["NetWeight"] = txtNetWeight.Text;
            drAsset[0]["GrossWeightDisp"] = txtGrossWeight.Text + " " + txtMeasurement.Text;
            drAsset[0]["NetWeightDisp"] = txtNetWeight.Text + " " + txtMeasurement.Text;
            drAsset[0]["PureGoldMass"] = txtPureGoldMass.Text;
            drAsset[0]["PureGoldValue"] = txtPureGoldValue.Text;
            drAsset[0]["MaxLoan"] = lblMaxLoanValue.Text;
            drAsset[0]["FinanceAmount"] = txtMaxLoan.Text;
            drAsset[0]["Rate"] = dTbl.Rows[0][0].ToString();

            drAsset[0].AcceptChanges();
        }

        Session["PricingAssetDetails"] = dtAssetDetails;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
    }

    protected void ddlAssetCodeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProGetGLMatrix();
        FunProCalculateFinanceAmount();
        ddlAssetCodeList.ToolTip = ddlAssetCodeList.SelectedItem.Text;
        ddlAssetCodeList.Focus();
    }

    protected void ddlPurity_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProGetGLMatrix();
        FunProCalculateFinanceAmount();
        ddlPurity.Focus();
    }

    protected void FunProGetGLMatrix()
    {
        txtMeasurement.Text = lblMeasurementID.Text = txtMaxLoan.Text = 
        lblMaxLoanValue.Text = txtMarketValue.Text = txtValuationDate.Text = string.Empty;
        if (ddlPurity.SelectedValue != "0" && ddlAssetCodeList.SelectedValue != "0")
        {
            ProcParam = new Dictionary<string, string>();
            ProcParam.Add("@Asset_Code", ddlAssetCodeList.SelectedValue);
            ProcParam.Add("@Company_ID", intCompanyId.ToString());
            ProcParam.Add("@Constitution_ID", strConstitution_ID);
            ProcParam.Add("@Product_ID", strProduct_ID);
            ProcParam.Add("@Location_Code", strLocation_ID);
            ProcParam.Add("@Purity_ID", ddlPurity.SelectedValue);

            DataSet dSet = Utility.GetDataset("S3G_ORG_GetGLMatrixForApp", ProcParam);
            if (dSet.Tables[1].Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Market Value not defined");
                return;
            }

            if (dSet.Tables[0].Rows.Count > 0)
            {
                txtMeasurement.Text = dSet.Tables[0].Rows[0]["Measurement"].ToString();
                lblMeasurementID.Text = dSet.Tables[0].Rows[0]["Unit_ID"].ToString();
                lblMaxLoanValue.Text = dSet.Tables[0].Rows[0]["MaxLoan"].ToString();
                txtValuationDate.Text = dSet.Tables[1].Rows[0]["ValuationDate"].ToString();
                txtMarketValue.Text = dSet.Tables[1].Rows[0]["MarketValue"].ToString();
                Session["Asset_Tenure"] = dSet.Tables[0].Rows[0]["Tenure"].ToString();
                Session["Asset_Tenure_Type"] = dSet.Tables[0].Rows[0]["Tenure_Type"].ToString();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Gold Loan Matrix not defined");
                return;
            }
        }
    }

    protected void FunProCalculateFinanceAmount()
    {
        decimal decActualGrams = 0;
        decimal decActLoanVal = 0;
        if (!string.IsNullOrEmpty(txtUnitCount.Text) && ddlAssetCodeList.SelectedValue != "0" && ddlPurity.SelectedValue != "0"
            && !string.IsNullOrEmpty(lblMeasurementID.Text) && !string.IsNullOrEmpty(txtMarketValue.Text)
            && !string.IsNullOrEmpty(txtNetWeight.Text) && !string.IsNullOrEmpty(lblMaxLoanValue.Text))
        {
            if (lblMeasurementID.Text == "1")  // Grams
            {
                decActualGrams = Convert.ToDecimal(txtNetWeight.Text);
                decActLoanVal = Convert.ToDecimal(lblMaxLoanValue.Text);
            }
            else if (lblMeasurementID.Text == "2")   // Tola
            {
                decActualGrams = Convert.ToDecimal(txtNetWeight.Text) * Convert.ToDecimal("11.6638125");
                decActLoanVal = Convert.ToDecimal(lblMaxLoanValue.Text) / Convert.ToDecimal("11.6638125");
            }
            else if (lblMeasurementID.Text == "3")  // Ounce
            {
                decActualGrams = Convert.ToDecimal(txtNetWeight.Text) * Convert.ToDecimal("28.3495");
                decActLoanVal = Convert.ToDecimal(lblMaxLoanValue.Text) / Convert.ToDecimal("28.3495");
            }
            else // Sovereign 
            {
                decActualGrams = Convert.ToDecimal(txtNetWeight.Text) * Convert.ToDecimal("7.9881");
                decActLoanVal = Convert.ToDecimal(lblMaxLoanValue.Text) / Convert.ToDecimal("7.9881");
            }

            int intKaratRate = Convert.ToInt32(ddlPurity.SelectedItem.Text.Substring(0, 2));
            txtPureGoldMass.Text = ((intKaratRate * decActualGrams) / 24).ToString("#0.####");
            txtPureGoldValue.Text = (Convert.ToDecimal(txtPureGoldMass.Text) * Convert.ToDecimal(txtMarketValue.Text)).ToString("#0.##");

            txtMaxLoan.Text = Math.Round((decActLoanVal * Convert.ToDecimal(txtPureGoldMass.Text))).ToString();

        }
        else
        {
            txtPureGoldMass.Text = txtPureGoldValue.Text = txtMaxLoan.Text = string.Empty;
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

        DataTable DtRate = new DataTable();
        Dictionary<string, string> dictParam = new Dictionary<string, string>();

        dictParam.Add("@OPTION", "1");
        dictParam.Add("@COMPANYID", intCompanyId.ToString());
        DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];

        ViewState["RateDt2"] = DtRate;

        FunProLoadAssetValue("NEW");
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

        dictParam = new Dictionary<string, string>();
        dictParam.Clear();
        dictParam.Add("@Option", "1");
        dictParam.Add("@Param1", "Gold_Purity");

        ddlPurity.BindDataTable("S3G_ORG_GetGlobalLookUp", dictParam, new string[] { "Value", "Name" });
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

        txtValuationDate.Text = drAsset[0]["ValuationDate"].ToString();
        txtMarketValue.Text = drAsset[0]["MarketValue"].ToString();
        ddlAssetCodeList.SelectedValue = drAsset[0]["Asset_ID"].ToString();
        ddlPurity.SelectedValue = drAsset[0]["PurityID"].ToString();
        txtMeasurement.Text = drAsset[0]["Measurement"].ToString();
        lblMeasurementID.Text = drAsset[0]["MeasurementID"].ToString();
        txtUnitCount.Text = drAsset[0]["Noof_Units"].ToString();
        txtGrossWeight.Text = drAsset[0]["GrossWeight"].ToString();
        txtNetWeight.Text = drAsset[0]["NetWeight"].ToString();
        txtPureGoldMass.Text = drAsset[0]["PureGoldMass"].ToString();
        txtPureGoldValue.Text = drAsset[0]["PureGoldValue"].ToString();
        lblMaxLoanValue.Text = drAsset[0]["MaxLoan"].ToString();
        txtMaxLoan.Text = drAsset[0]["FinanceAmount"].ToString();

        if (Request.QueryString["qsMode"] != null)
        {
            ddlAssetCodeList.ClearDropDownList();
            ddlPurity.ClearDropDownList();
            txtValuationDate.ReadOnly = txtMarketValue.ReadOnly = txtUnitCount.ReadOnly = txtMeasurement.ReadOnly = txtMaxLoan.ReadOnly =
                txtGrossWeight.ReadOnly = txtNetWeight.ReadOnly = txtPureGoldMass.ReadOnly = txtPureGoldValue.ReadOnly = true;
        }

    }

    #endregion


    protected void txtNetWeight_TextChanged(object sender, EventArgs e)
    {
        FunProCalculateFinanceAmount();
    }
    
}


