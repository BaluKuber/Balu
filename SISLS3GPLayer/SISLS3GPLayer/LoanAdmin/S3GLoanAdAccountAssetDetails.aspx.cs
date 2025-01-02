#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Asset Details
/// Reason              : Asset Details For Application Processiong
/// Created By  		: Thangam M
/// Created Date        : 29-Oct-2010
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

public partial class LoanAd_S3G_LoanAdAccountAssetDetails : ApplyThemeForProject
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
    public static LoanAd_S3G_LoanAdAccountAssetDetails obj_Page;
    static string strPageName = "Account Asset Details";
    string strNewWin = " window.showModalDialog('../LoanAdmin/S3GLoanAdAccountAssetDetails.aspx?qsMaster=Yes&qsRowID=";
    string NewWinAttributes = "', 'Asset Details', 'dialogwidth:700px;dialogHeight:400px;');";
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
        if (Request.QueryString["qsMode"] != null)
        {
            btnOK.Enabled = false;
        }

        if (Request.QueryString["qsRowID"] != null)                               //When click Add for First Row//
        {
            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);
        }
        txtRequiredFromDate_CalendarExtender.Format = strDateFormat;
        
        txtTotalAssetValue.Attributes.Add("readonly", "readonly");
        txtRequiredFromDate.Attributes.Add("readonly", "readonly");
        txtUnitValue.SetDecimalPrefixSuffix(10, 2, true);
        txtTotalAssetValue.SetDecimalPrefixSuffix(10, 2, true);
        txtMarginPercentage.SetDecimalPrefixSuffix(3, 2, true);
        txtMarginAmountAsset.SetDecimalPrefixSuffix(10, 2, true);
        txtPaymentPercentage.SetDecimalPrefixSuffix(3, 2, true);
        if (Request.QueryString["qsMaster"].ToString() == "Yes")
        {
            if (rdnlAssetType.SelectedIndex == 1)
            {
                rfvFinanceAmountAsset.Enabled =
                rfvCapitalPortion.Enabled = false;
                lblCapitalPortion.CssClass = lblNonCapitalPortion.CssClass =
            lblFinanceAmountAsset0.CssClass = "styleDisplayLabel";
            }
            else
            {
                rfvFinanceAmountAsset.Enabled =
                rfvCapitalPortion.Enabled = true;
                lblCapitalPortion.CssClass = lblNonCapitalPortion.CssClass = lblFinanceAmountAsset0.CssClass = "styleReqFieldLabel";
            }
            //rfvEntityNameList.Enabled = 
                rfvPayTo.Enabled = false;

            lblPayTo.CssClass = lblCustomerName.CssClass = "styleDisplayLabel";
        }
        else
        {
            rfvFinanceAmountAsset.Enabled =
            rfvCapitalPortion.Enabled = 
            //rfvEntityNameList.Enabled = 
            rfvPayTo.Enabled = true;
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
                txtUnitCount.Text = "1";
                txtUnitCount.Enabled = false;
            }
            else
            {
                FunToggleLeaseAssetCode(false);
            }
            FunPriLoadAssetDetails((DataTable)Session["ApplicationAssetDetails"]);
        }
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
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

    protected new void Page_PreInit(object sender, EventArgs e)
    {
        UserInfo ObjUserInfo = new UserInfo();
        this.Page.Theme = ObjUserInfo.ProUserThemeRW;
    }

    #endregion

    #region Page Events

    protected void rdnlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBookDepreciationPerc.Text = "";
        txtBlockDepreciationPerc.Text = "";

        if (rdnlAssetType.SelectedValue == "1")
        {
            FunToggleLableVisble(true);
            lblRequiredFromDate.CssClass = "styleReqFieldLabel";
            rfvRequiredFromDate.Enabled = true;
            
        }
        else
        {
            FunToggleLableVisble(false);
            lblRequiredFromDate.CssClass = "styleDisplayLabel";
            rfvRequiredFromDate.Enabled = false;
            
        }
    }

    protected void ddlPayTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayTo.SelectedItem.ToString().ToLower() == "entity")
        {
            FunToggleEntityControls(true);
        }
        else
        {
            FunToggleEntityControls(false);
        }
    }
    protected void ddlNewLeaseAssetNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRequiredFromDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
        //Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //if (ddlNewLeaseAssetNo.SelectedIndex > 0)
        //{
        //    dictParam.Add("@OPTION", "4");
        //    dictParam.Add("@COMPANYID", intCompanyId.ToString());
        //    dictParam.Add("@LeaseAssetNo", ddlNewLeaseAssetNo.SelectedValue);
        //    DataTable DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
        //    txtTotalAssetValue.Text = txtUnitValue.Text = DtRate.Rows[0]["WDV"].ToString();
        //    txtUnitValue.ReadOnly = true;
        //}
        //else
        //{
        //    txtTotalAssetValue.Text = txtUnitValue.Text = "";
        //    txtUnitValue.ReadOnly = false;
        //}
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["qsMaster"].ToString() == "No")
        {
            if (Convert.ToDouble(txtFinanceAmountAsset.Text) > Convert.ToDouble(txtTotalAssetValue.Text))
            {
                cvApplicationAsset.ErrorMessage = @"Please correct the following validation(s): </br></br>" + "     Finance Amount cannot exceed Total Asset Value";
                cvApplicationAsset.IsValid = false;
                return;
            }
        }
        if (Convert.ToDouble(txtCapitalPortion.Text) > Convert.ToDouble(txtFinanceAmountAsset.Text))
        {
            cvApplicationAsset.ErrorMessage = @"Please correct the following validation(s): </br></br>" + "     Capital Amount cannot exceed Finance Amount";
            cvApplicationAsset.IsValid = false;
            return;
        }

        if (Convert.ToDouble(txtNonCapitalPortion.Text) > Convert.ToDouble(txtFinanceAmountAsset.Text))
        {
            cvApplicationAsset.Text = @"Please correct the following validation(s): </br></br>" + "        Non Capital Amount cannot exceed Finance Amount";
            cvApplicationAsset.IsValid = false;
            return;
        }
        if (Convert.ToDouble(txtNonCapitalPortion.Text) + Convert.ToDouble(txtCapitalPortion.Text) != Convert.ToDouble(txtFinanceAmountAsset.Text))
        {
            cvApplicationAsset.Text = @"Please correct the following validation(s): </br></br>" + "        The sum of Capital and Non Capital Amounts should be equal to Finance Amount.";
            cvApplicationAsset.IsValid = false;
            return;
        }
        if (ddlNewLeaseAssetNo.Visible)
        {
            DataRow[] DrRate = (((DataTable)ViewState["NewLeaseAssetNo"]).Select("Lease_Asset_No = '" + ddlNewLeaseAssetNo.SelectedValue.ToString() + "'"));
            if (DrRate.Count() > 0)
            {
                if (Convert.ToString(DrRate[0]["AVAILABLE_DATE"]) != string.Empty)
                {
                    if (Utility.StringToDate(txtRequiredFromDate.Text) <= Utility.StringToDate(Convert.ToString(DrRate[0]["AVAILABLE_DATE"])))
                    {
                        cvApplicationAsset.Text = @"Please correct the following validation(s): </br><br/>" + "     The Asset is not available on the selected date";
                        cvApplicationAsset.IsValid = false;
                        return;
                    }
                }
            }
        }
        DataTable dtAssetDetails = (DataTable)Session["ApplicationAssetDetails"];
        DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + intSerialNo.ToString());
        drAsset[0]["LeaseType"] = rdnlAssetType.SelectedValue;
        drAsset[0]["Asset_ID"] = ddlAssetCodeList.SelectedValue;
        drAsset[0]["Asset_Code"] = ddlAssetCodeList.SelectedItem.Text.Split('-')[0].ToString();
        if (Request.QueryString["qsMaster"] != null)
        {
            if (Request.QueryString["qsMaster"].ToString() == "Yes")
            {
                if (ddlNewLeaseAssetNo.Visible)
                {
                    drAsset[0]["Lease_Asset_No"] = ddlNewLeaseAssetNo.SelectedItem.Text;
                }
                else
                {
                    drAsset[0]["Lease_Asset_No"] = ddlLeaseAssetNo.SelectedItem.Text;
                }

            }
            else
            {
                drAsset[0]["Lease_Asset_No"] = "";
            }
        }
            
       
        
        drAsset[0]["Noof_Units"] = txtUnitCount.Text;
        drAsset[0]["Finance_Amount"] = txtFinanceAmountAsset.Text;
        if (!string.IsNullOrEmpty(txtRequiredFromDate.Text))
        {
            drAsset[0]["Required_FromDate"] = Utility.StringToDate(txtRequiredFromDate.Text);
        }
        else
        {
            drAsset[0]["Required_FromDate"] = DBNull.Value;
        }
        drAsset[0]["Unit_Value"] = txtUnitValue.Text;
        if (!string.IsNullOrEmpty(txtMarginPercentage.Text))
            drAsset[0]["Margin_Percentage"] = txtMarginPercentage.Text;
        drAsset[0]["TotalAssetValue"] = Convert.ToDecimal(txtUnitValue.Text) * Convert.ToDecimal(txtUnitCount.Text);
        drAsset[0]["Book_depreciation_Percentage"] = string.IsNullOrEmpty(txtBlockDepreciationPerc.Text) ? "0" : txtBlockDepreciationPerc.Text;
        drAsset[0]["Margin_Amount"] = string.IsNullOrEmpty(txtMarginAmountAsset.Text) ? 0 : Convert.ToDecimal(txtMarginAmountAsset.Text);
        drAsset[0]["Block_depreciation_Percentage"] = string.IsNullOrEmpty(txtBlockDepreciationPerc.Text) ? "0" : txtBookDepreciationPerc.Text;
        drAsset[0]["NonCapital_Portion"] = txtNonCapitalPortion.Text;
        drAsset[0]["Capital_Portion"] = txtCapitalPortion.Text;
        if (!string.IsNullOrEmpty(txtPaymentPercentage.Text))
            drAsset[0]["Payment_Percentage"] = txtPaymentPercentage.Text;
        if (ddlPayTo.SelectedValue == "--Select--")
        {
            drAsset[0]["Pay_To_ID"] = DBNull.Value;
            drAsset[0]["Entity_ID"] = DBNull.Value;
            drAsset[0]["Entity_Code"] = DBNull.Value;
        }
        else
        {
            drAsset[0]["Pay_To_ID"] = ddlPayTo.SelectedValue.ToString();

            if (ddlPayTo.SelectedItem.ToString().ToUpper() == "CUSTOMER")
            {
                drAsset[0]["Entity_ID"] = Session["AccountAssetCustomer"].ToString().Substring(0, Session["AccountAssetCustomer"].ToString().Remove(Session["AccountAssetCustomer"].ToString().IndexOf(";")).Length);
            }
            else
            {
                drAsset[0]["Entity_ID"] = ddlEntityNameList.SelectedValue.ToString();
                drAsset[0]["Entity_Code"] = ddlEntityNameList.SelectedText.ToString();
            }
        }
        drAsset[0].AcceptChanges();
        Session["ApplicationAssetDetails"] = dtAssetDetails;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
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

       try
       {

           S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
           ObjStatus.Option = 1;
           ObjStatus.Param1 = S3G_Statu_Lookup.PAY_TO.ToString();
           Utility.FillDLL(ddlPayTo, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

           DataTable DtRate = new DataTable();
           Dictionary<string, string> dictParam = new Dictionary<string, string>();

           dictParam.Add("@OPTION", "2");
           dictParam.Add("@COMPANYID", intCompanyId.ToString());
           DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
           ddlAssetCodeList.DataSource = DtRate;
           ddlAssetCodeList.DataTextField = "Asset_Code";
           ddlAssetCodeList.DataValueField = "Asset_ID";
           ddlAssetCodeList.DataBind();
           ddlAssetCodeList.Items.Insert(0, new ListItem("--Select--", "0"));

           ViewState["RateDt1"] = DtRate;

           dictParam = new Dictionary<string, string>();

           dictParam.Add("@OPTION", "1");
           dictParam.Add("@COMPANYID", intCompanyId.ToString());
           DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
           ddlLeaseAssetNo.DataSource = DtRate;
           ddlLeaseAssetNo.DataTextField = "Lease_Asset_No";
           ddlLeaseAssetNo.DataValueField = "Lease_Asset_No";
           ddlLeaseAssetNo.DataBind();
           ddlLeaseAssetNo.Items.Insert(0, new ListItem("--Select--", "0"));
           ViewState["RateDt2"] = DtRate;
           dictParam.Clear();

           dictParam.Clear();
           dictParam.Add("@Option", "11");
           dictParam.Add("@Company_ID", intCompanyId.ToString());
           dictParam.Add("@ID", "1");
           //ddlEntityNameList.BindDataTable(SPNames.S3G_ORG_GetPricing_List, dictParam, true, new string[] { "Entity_ID", "Entity_Code", "Entity_Name" });
           //ObjStatus.Option = 38;
           //ObjStatus.Param1 = intCompanyId.ToString();
           //Utility.FillDLL(ddlEntityNameList, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus), true);
       }
       catch (Exception ex)
      {
             ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
           throw ex;
           
       }
      finally
      {
          ObjCustomerService.Close();
      }
    }
    private void FunPriLoadNewLAN()
    {
        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        dictParam.Add("@OPTION", "3");
        dictParam.Add("@COMPANYID", intCompanyId.ToString());
        dictParam.Add("@ASSETId", ddlAssetCodeList.SelectedValue);
        DataTable DtRate = Utility.GetDataset("S3G_ORG_GETAPPLICATIONASSET", dictParam).Tables[0];
        ddlNewLeaseAssetNo.DataSource = DtRate;
        ddlNewLeaseAssetNo.DataTextField = "Lease_Asset_No";
        ddlNewLeaseAssetNo.DataValueField = "Lease_Asset_No";
        ddlNewLeaseAssetNo.DataBind();
        ddlNewLeaseAssetNo.Items.Insert(0, new ListItem("--Select--", "0"));
        ViewState["NewLeaseAssetNo"] = DtRate;
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
        
    }

    protected void FunToggleEntityControls(bool CanShow)
    {
        ddlEntityNameList.Visible = CanShow;
        lblEntityNameList.Visible = CanShow;
        //rfvEntityNameList.Enabled = CanShow;
        ddlEntityNameList.IsMandatory = CanShow;

        txtCustomerName.Visible = !CanShow;
        lblCustomerName.Visible = !CanShow;
    }
    private void FunPriLoadAssetDetails(DataTable dtAssetDetails)
    {
        DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + intSerialNo.ToString());
        if (drAsset.Length == 0)
        {
            txtSlNo.Text = Convert.ToString(dtAssetDetails.Rows.Count + 1);
            return;
        }
        txtSlNo.Text = (intSerialNo).ToString();
        rdnlAssetType.SelectedValue = drAsset[0]["LeaseType"].ToString();
        rdnlAssetType.Enabled = false;
        FunProLoadCombos();
        if (rdnlAssetType.SelectedValue == "1")
        {
            ddlAssetCodeList.SelectedValue = drAsset[0]["Asset_ID"].ToString();
            ddlLeaseAssetNo.SelectedValue = drAsset[0]["Lease_Asset_No"].ToString();
            ddlAssetCodeList.ClearDropDownList();
            FunToggleLableVisble(true);
            txtRequiredFromDate.Text = DateTime.Parse(drAsset[0]["Required_FromDate"].ToString(), CultureInfo.CurrentCulture).ToString(objS3GSession.ProDateFormatRW);
            ddlLeaseAssetNo.ClearDropDownList();
            ddlNewLeaseAssetNo.Visible = lblNewLeaseAssetNo.Visible = false;
            rfvNewLeaseAssetNo.Enabled = false;
        }
        else
        {
            ddlAssetCodeList.SelectedValue = drAsset[0]["Asset_ID"].ToString();
            ddlAssetCodeList.ToolTip = ddlAssetCodeList.SelectedItem.Text;
            FunToggleLableVisble(false);
            if (drAsset[0]["Required_FromDate"] != null)
            {
                if (drAsset[0]["Required_FromDate"].ToString() != "")
                {
                    txtRequiredFromDate.Text = DateTime.Parse(drAsset[0]["Required_FromDate"].ToString(), CultureInfo.CurrentCulture).ToString(objS3GSession.ProDateFormatRW);
                }
                else
                {
                    txtRequiredFromDate.Text = string.Empty;
                }
            }
            ddlAssetCodeList.ClearDropDownList();
            if (Request.QueryString["qsMaster"] != null)
            {
                if (Request.QueryString["qsMaster"].ToString() == "Yes")
                {
                    FunPriLoadNewLAN();
                    ddlNewLeaseAssetNo.Visible = lblNewLeaseAssetNo.Visible = true;
                    rfvNewLeaseAssetNo.Enabled = true;
                    txtUnitCount.Enabled = false;
                    ddlNewLeaseAssetNo.SelectedValue = drAsset[0]["Lease_Asset_No"].ToString();
                }
                else
                {
                    ddlNewLeaseAssetNo.Visible = lblNewLeaseAssetNo.Visible = false;
                    rfvNewLeaseAssetNo.Enabled = false;
                    txtUnitCount.Enabled = true;
                }
            }
            else
            {
                ddlNewLeaseAssetNo.Visible = lblNewLeaseAssetNo.Visible = false;
                rfvNewLeaseAssetNo.Enabled = false;
            }
            
        }
        txtUnitCount.Text = drAsset[0]["Noof_Units"].ToString();
        txtFinanceAmountAsset.Text = drAsset[0]["Finance_Amount"].ToString();
        txtCapitalPortion.Text = drAsset[0]["Capital_Portion"].ToString();
        txtNonCapitalPortion.Text = drAsset[0]["NonCapital_Portion"].ToString();
        txtUnitValue.Text = drAsset[0]["Unit_Value"].ToString();
        txtUnitValue.Enabled = false;
        txtBookDepreciationPerc.Text = drAsset[0]["Book_depreciation_Percentage"].ToString();
        txtBlockDepreciationPerc.Text = drAsset[0]["Block_depreciation_Percentage"].ToString();
        if (drAsset[0]["Margin_Amount"].ToString() == "0.00" || drAsset[0]["Margin_Amount"].ToString() == "0" || drAsset[0]["Margin_Amount"].ToString() == "")
        {
            txtMarginAmountAsset.Text = "";
        }
        else
        {
            txtMarginAmountAsset.Text = drAsset[0]["Margin_Amount"].ToString();
        }
        if (drAsset[0]["Margin_Percentage"].ToString() == "0.00" || drAsset[0]["Margin_Percentage"].ToString() == "" || drAsset[0]["Margin_Percentage"].ToString() == "0")
        {
            txtMarginPercentage.Text = "";
        }
        else
        {
            txtMarginPercentage.Text = drAsset[0]["Margin_Percentage"].ToString();
        }
        txtMarginPercentage.Enabled = false;
        txtMarginAmountAsset.Enabled = false;
        txtPaymentPercentage.Text = Convert.ToString(drAsset[0]["Payment_Percentage"]);
        txtTotalAssetValue.Text = Convert.ToString(Convert.ToInt32(txtUnitCount.Text) * Convert.ToDouble(txtUnitValue.Text));
        ddlPayTo.SelectedValue = drAsset[0]["Pay_To_ID"].ToString();
        ddlPayTo.ClearDropDownList();
        if (ddlPayTo.SelectedItem.Text.ToUpper() == "ENTITY")
        {
            FunToggleEntityControls(true);
            txtCustomerName.Text = "";
            ddlEntityNameList.SelectedValue = drAsset[0]["Entity_ID"].ToString();
            ddlEntityNameList.SelectedText = drAsset[0]["Entity_Code"].ToString();
            if (Request.QueryString["qsMaster"].ToString() == "Yes")
            {
                //ddlEntityNameList.ClearDropDownList();
                ddlEntityNameList.ReadOnly = true;
            }

        }
        else if (ddlPayTo.SelectedItem.Text.ToUpper() == "CUSTOMER")
        {
            FunToggleEntityControls(false);
            if (Session["AccountAssetCustomer"] != null)
            {
                txtCustomerName.Text = Session["AccountAssetCustomer"].ToString().Substring(Session["AccountAssetCustomer"].ToString().IndexOf(";") + 1);
            }
        }
        if (Request.QueryString["qsMode"] != null)
        {
            rdnlAssetType.Enabled = false;
            ddlAssetCodeList.ClearDropDownList();
            ddlLeaseAssetNo.ClearDropDownList();
            ddlPayTo.ClearDropDownList();
            
            txtUnitCount.ReadOnly = txtUnitValue.ReadOnly = txtTotalAssetValue.ReadOnly = txtMarginAmountAsset.ReadOnly =
            txtMarginPercentage.ReadOnly = txtBlockDepreciationPerc.ReadOnly = txtBookDepreciationPerc.ReadOnly =
            txtFinanceAmountAsset.ReadOnly = txtCapitalPortion.ReadOnly = txtNonCapitalPortion.ReadOnly = txtPaymentPercentage.ReadOnly =
            txtSlNo.ReadOnly = true;
            //ddlEntityNameList.ClearDropDownList();
            ddlEntityNameList.ReadOnly = true;
            if (ddlNewLeaseAssetNo.Visible)
            {
                ddlNewLeaseAssetNo.ClearDropDownList();
            }
        }
        if (Request.QueryString["qsLOB"] != null)
        {
            rdnlAssetType.Enabled = false;
            ddlAssetCodeList.ClearDropDownList();
            ddlLeaseAssetNo.ClearDropDownList();
            ddlPayTo.ClearDropDownList();

            txtUnitCount.ReadOnly = txtUnitValue.ReadOnly = txtTotalAssetValue.ReadOnly = txtMarginAmountAsset.ReadOnly =
            txtMarginPercentage.ReadOnly = txtBlockDepreciationPerc.ReadOnly = txtBookDepreciationPerc.ReadOnly =
            txtFinanceAmountAsset.ReadOnly = txtCapitalPortion.ReadOnly = txtNonCapitalPortion.ReadOnly = txtPaymentPercentage.ReadOnly =
            txtSlNo.ReadOnly = true;
            //ddlEntityNameList.ClearDropDownList();
            ddlEntityNameList.ReadOnly = true;
            
        }
       
    }
    #endregion
}

