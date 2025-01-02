#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   System Admin
/// Screen Name         :   S3GSysAdminExchangeRateMaster_Add
/// Created By          :   Suresh P
/// Created Date        :   21-MAY-2010
/// Purpose             :   To Modify the ExchangeRate Master details
/// 
/// Last Updated By		:   Gunasekar.K
/// Last Updated Date   :   15-Oct-2010
/// Reason              :   To address the bug -1751
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Configuration;

#endregion

public partial class System_Admin_S3GSysAdminExchangeRateMaster_Add : ApplyThemeForProject
{
    #region Initialization
    int intExchangeRateID = 0;
    int intErrCode = 0;
    int intCompanyID;
    int intUserID;
    int inthdUserid;
    string strRedirectPageMC;
    string strDateFormat;
    string strMode = string.Empty;
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminExchangeRateMaster_View.aspx'";

    //string strMonth_Value = string.Empty;
    //string strMonth_Option = string.Empty;



    AccountMgtServicesReference.AccountMgtServicesClient ObjExchangeMasterMasterClient;
    AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable ObjS3G_ExchangeMasterDataTable = new AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable();
    #endregion

    /// <summary>
    /// Get Global Parameter Details for the given financialmonth and year
    /// </summary>
    /// <param name="strFinancialMonth"></param>
    /// <param name="strFinancialYear"></param>
    /// <returns></returns>
    private int FunPriGetGlobalParamDetails(string strFinancialMonth, string strFinancialYear)
    {
        int intMonth_Option = -1;
        try
        {
            CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable objS3G_SYSAD_GlobalParameterSetupDataTable = new CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable();
            SerializationMode SerMode = SerializationMode.Binary;

            CompanyMgtServicesReference.CompanyMgtServicesClient objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

            int intCompanyId = Convert.ToInt32(Session["Company_ID"]);

            byte[] byteGPSDetails = objCompanyMasterClient.FunPubQueryGPSDetails(intCompanyId, strFinancialYear, strFinancialMonth);
            objS3G_SYSAD_GlobalParameterSetupDataTable = (CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(byteGPSDetails, SerMode, typeof(CompanyMgtServices.S3G_SYSAD_GlobalParameterSetupDataTable));

            intMonth_Option = Convert.ToInt32(Convert.ToBoolean(objS3G_SYSAD_GlobalParameterSetupDataTable.Rows[0]["Month_Option"].ToString()));

            objCompanyMasterClient.Close();

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        return intMonth_Option;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void ddlExchangeCurrency_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //Response.Write(ddlExchangeCurrency.SelectedValue);
    //}

    /// <summary>
    /// Page Load Events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtDate.Attributes.Add("readonly", "readonly");
        txtDefaultCurrency.Attributes.Add("readonly", "readonly");
        //txtExchangeValue.Attributes.Add("onblur", "funChkDecimial(this,6,4);ChkIsZero(this);");
        //cexDate.Format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        txtExchangeValue.SetDecimalPrefixSuffix(6, 4, true, false, "Exchange Value");
        txtDefaultValue.SetDecimalPrefixSuffix(6, 0, true, false, "Default Value");
        //txtExchangeValue.Style.Add("text-align", "right");
        //txtExchangeValue.Attributes.Add("onkeyup", "extractNumber(this, event, " + 4 + ", " + false.ToString().ToLower() + ", " + false.ToString().ToLower() + ", " + false.ToString().ToLower() + ");");
        //txtExchangeValue.Attributes.Add("onkeypress", "return blockNonNumbers(this, event," + true.ToString().ToLower() + "," + false.ToString().ToLower() + ", " + false.ToString().ToLower() + ", " + false.ToString().ToLower() + ");");
        //txtExchangeValue.Attributes.Add("onblur", "return funChkDecimial(this, 6,4);");
          //  txtExchangeValue.Attributes.Add("onpaste", "return false;") ;  //disallow pasteing.  Seems to work on modern browsers
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        cexDate.Format = strDateFormat;
        cexDate.SelectedDate = DateTime.Today;


        txtDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDate.ClientID + "','" + strDateFormat + "',true,  false);");

        if (Request.QueryString["qsER"] != null)
        {

            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsER"));
            strMode = Request.QueryString.Get("qsMode");
            if (fromTicket != null)
            {
                intExchangeRateID = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        if (!IsPostBack)
        {

            FunPriLoadCurrency_LIST();
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if (((intExchangeRateID > 0)) && (strMode == "M"))
            {

                FunPriDisableControls(1);

            }
            else if (((intExchangeRateID > 0)) && (strMode == "Q"))
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriDisableControls(0);
            }
        }
    }

    /// <summary>
    /// Get Currency Exchange Rate Details
    /// </summary>
    private void FunPriGetExchangeRateDetails()
    {
        try
        {
            AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow ObjExchangeMasterRow;

            ObjExchangeMasterRow = ObjS3G_ExchangeMasterDataTable.NewS3G_SYSAD_ExchangeRateMasterRow();

            ObjExchangeMasterRow.Exchange_Rate_ID = intExchangeRateID;

            ObjS3G_ExchangeMasterDataTable.AddS3G_SYSAD_ExchangeRateMasterRow(ObjExchangeMasterRow);

            ObjExchangeMasterMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteExchangeRateDetails = ObjExchangeMasterMasterClient.FunPubQueryExchangeRate(SerMode, ClsPubSerialize.Serialize(ObjS3G_ExchangeMasterDataTable, SerMode));

            AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable dtExchangeMaster = (AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable)ClsPubSerialize.DeSerialize(byteExchangeRateDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable));
            AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow drExchangeMaster = dtExchangeMaster.Rows[0] as AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow;

            //txtDate.Text = DateTime.Parse(drExchangeMaster.Effective_Date.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToShortDateString();

            txtDate.Text = DateTime.Parse(drExchangeMaster.Effective_Date.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            //txtDate.Text = drExchangeMaster.Effective_Date.ToString();
            txtDefaultValue.Text = drExchangeMaster.Default_Value.ToString("0");
            ddlExchangeCurrency.SelectedValue = drExchangeMaster.Exchange_Currency_ID.ToString();
            //txtExchangeValue.Text = drExchangeMaster.Exchange_Value.ToString("0.0000");
            txtExchangeValue.Text = drExchangeMaster.Exchange_Value.ToString();
            FunSetPrecisionValue();
            hdnID.Value = drExchangeMaster.Created_By.ToString();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjExchangeMasterMasterClient.Close();
        }
    }

    /// <summary>
    /// Save the Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strGetValue = string.Empty;
        int intFin_Month, intFin_Year;
        string strFin_Year = string.Empty;

        DateTime dt = Utility.StringToDate(txtDate.Text);
        strGetValue = dt.Year.ToString();
        strGetValue += dt.Month.ToString().Length == 1 ? ("0".ToString() + dt.Month.ToString()) : dt.Month.ToString();

        intFin_Month = dt.Month;
        intFin_Year = dt.Year;

        if (intFin_Month < 4)
            strFin_Year = (intFin_Year - 1).ToString() + "-" + intFin_Year.ToString();
        else
            strFin_Year = intFin_Year.ToString() + "-" + (intFin_Year + 1).ToString();


        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminExchangeRateMaster_View.aspx';";
        string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminExchangeRateMaster_Add.aspx';";

        if (Convert.ToInt32(txtDefaultValue.Text) == 0)
        {
            //--Added by Guna on 15th-Oct-2010 to address the bug -1751
            strAlert = strAlert.Replace("__ALERT__", "Default Value should not be 0.");
            //strAlert = strAlert.Replace("__ALERT__", "Default Value not allow 0.");
            //--Ends Here
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            lblErrorMessage.Text = string.Empty;
            return;
        }

        if (Convert.ToDecimal(txtExchangeValue.Text) == 0)
        {
            //--Added by Guna on 15th-Oct-2010 to address the bug -1751
            strAlert = strAlert.Replace("__ALERT__", "Exchange Value should not be 0.");
            //strAlert = strAlert.Replace("__ALERT__", "Exchange Value not allow 0.");
            //--Ends Here

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            lblErrorMessage.Text = string.Empty;
            return;
        }
        if (intExchangeRateID == 0)
        {
            int strMonth_Option = FunPriGetGlobalParamDetails(strGetValue, strFin_Year);
            if (strMonth_Option == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "This date should be in an Open Month");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                lblErrorMessage.Text = string.Empty;
                return;
            }
        }

        try
        {
            //ObjUserInfo = new UserInfo();

            AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow ObjExchangeRateMasterRow;
            ObjExchangeRateMasterRow = ObjS3G_ExchangeMasterDataTable.NewS3G_SYSAD_ExchangeRateMasterRow();
            ObjExchangeRateMasterRow.Exchange_Rate_ID = intExchangeRateID;
            ObjExchangeRateMasterRow.Exchange_Currency_ID = Convert.ToInt32(ddlExchangeCurrency.SelectedValue);
            ObjExchangeRateMasterRow.Effective_Date = Utility.StringToDate(txtDate.Text);
            ObjExchangeRateMasterRow.Exchange_Value = Convert.ToDecimal(txtExchangeValue.Text);
            ObjExchangeRateMasterRow.Default_Value = Convert.ToDecimal(txtDefaultValue.Text);

            ObjExchangeRateMasterRow.Created_By = intUserID;
            //ObjUserInfo.ProUserIdRW;
            ObjExchangeRateMasterRow.Modified_By = intUserID;
            //ObjUserInfo.ProUserIdRW;
            ObjS3G_ExchangeMasterDataTable.AddS3G_SYSAD_ExchangeRateMasterRow(ObjExchangeRateMasterRow);


            SerializationMode SerMode = SerializationMode.Binary;
            ObjExchangeMasterMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
            if (intExchangeRateID > 0)
            {
                intErrCode = ObjExchangeMasterMasterClient.FunPubModifyExchangeRate(SerMode, ClsPubSerialize.Serialize(ObjS3G_ExchangeMasterDataTable, SerMode));
            }
            else
            {
                intErrCode = ObjExchangeMasterMasterClient.FunPubCreateExchangeRate(SerMode, ClsPubSerialize.Serialize(ObjS3G_ExchangeMasterDataTable, SerMode));
            }
            if (intErrCode == 0)
            {
                if (intExchangeRateID > 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Exchange Rate Master details updated successfully");
                }
                else
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strAlert = "Exchange Rate Master details added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
            }
            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Exchange Master already exists in the system. Kindly enter a new Exchange Master Code");
                strRedirectPageView = "";
            }
            else if (intErrCode == 3)
            {
                strAlert = strAlert.Replace("__ALERT__", "Exchange Master Name already exists in the system. Kindly enter a new Exchange Master Name");
                strRedirectPageView = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
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

    private void FunSetPrecisionValue()
    {
       DataTable ObjDt = (DataTable)ViewState["ExchangeVal"];
       DataRow[] ObjDRExchange = ObjDt.Select("CURRENCY_ID='" + ddlExchangeCurrency.SelectedValue + "'");
       txtExchangeValue.Attributes.Add("onkeyup", "funCutDecimal(this,'" + ObjDRExchange[0]["PRECISION"].ToString() + "')");
       S3GSession S3GSession = new S3GSession();
       int intGPSPrefix = S3GSession.ProGpsPrefixRW;
       /* Decimal places has to be set based on the selected 
        * Exchange country where defined in Currency Master */
       if (6 > intGPSPrefix)
           txtExchangeValue.Attributes.Add("onblur", "funChkDecimial(this,'" +
               intGPSPrefix.ToString() + "','" + ObjDRExchange[0]["PRECISION"].ToString() +
               "','Exchange Value',true);");
       else
           txtExchangeValue.Attributes.Add("onblur", "funChkDecimial(this,'6','" +
               ObjDRExchange[0]["PRECISION"].ToString() + "','Exchange Value',true);");

    }

    protected void ddlExchangeCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtExchangeValue.Text = "";
            FunSetPrecisionValue();
        }
        catch (Exception ex)
        {
            
        }
    }


    /// <summary>
    /// Clears all the record on confirmation 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        cexDate.SelectedDate = DateTime.Today;
        txtDefaultValue.Text = "1";
        ddlExchangeCurrency.SelectedValue = "0";
        txtExchangeValue.Text = "";
    }

    /// <summary>
    /// Redirect the Exchange Rate Master View Page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminExchangeRateMaster_View.aspx");
    }

    #region Page Methods
    /// <summary>
    /// Load All Currency
    /// </summary>
    private void FunPriLoadCurrency_LIST()
    {
        try
        {
            //ObjUserInfo = new UserInfo();
            Dictionary<string, string> dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", intCompanyID.ToString());
            if (intExchangeRateID == 0)
            {
                dictLOB.Add("@Is_Active", "1");
            }
            ddlExchangeCurrency.BindDataTable("S3G_Get_Currency_LIST", dictLOB, new string[] { "Currency_ID", "Currency_Code", "Currency_Name" });

            ViewState["ExchangeVal"] = (DataTable)((System.Web.UI.WebControls.BaseDataBoundControl)(ddlExchangeCurrency)).DataSource;

            dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", intCompanyID.ToString());
            DataTable dtDefault = Utility.GetDefaultData("S3G_Get_ExchangeRate_Default_Details", dictLOB);// as DataTable;
            if (dtDefault != null)
            {
                int intDefaultCurrency = Convert.ToInt32(dtDefault.Rows[0]["Currency_ID"]);
                txtDefaultCurrency.Text = ddlExchangeCurrency.Items.FindByValue(intDefaultCurrency.ToString()).Text;
                ddlExchangeCurrency.Items.Remove(ddlExchangeCurrency.Items.FindByValue(intDefaultCurrency.ToString()));
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion


    #region ValueDisable
    /// <summary>
    /// Disable Controls based on the User Roles
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
                ddlExchangeCurrency.Enabled = true;
                txtDate.Enabled = true;
                cexDate.Enabled = true;
                //txtDefaultValue.Enabled = true;
                btnClear.Enabled = true;
                txtDefaultValue.Text = "1";
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                break;

            case 1: // Modify Mode
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                FunPriGetExchangeRateDetails();
                ddlExchangeCurrency.Enabled = false;
                txtDate.Enabled = false;
                cexDate.Enabled = false;
                //txtDefaultValue.Enabled = false;
                btnClear.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                break;

            case -1:// Query Mode

                FunPriGetExchangeRateDetails();
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }

                if (bClearList)
                {
                    ddlExchangeCurrency.ClearDropDownList();
                }
                txtDate.ReadOnly = true;
                txtDate.Attributes.Remove("onblur");
                txtDefaultCurrency.ReadOnly = true;
                txtExchangeValue.ReadOnly = true;
                txtDefaultValue.ReadOnly = true;
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                cexDate.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                break;
        }

    }
    #endregion

}
