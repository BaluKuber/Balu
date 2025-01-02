using System;
using S3GBusEntity;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text;
using System.Web.Security;
using System.Configuration;
using System.IO;
using System.Web;
using System.Collections;
using System.Diagnostics;

public partial class LoanAdmin_S3GLOANADOperatingLeaseDepreciation_Add : ApplyThemeForProject
{
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;

    string intSJVNo = string.Empty;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    protected string strDateFormat = string.Empty;
    string strDepreciation = string.Empty;
    int intDeprDays = 0;
    StringBuilder strDepreciationBuilder = new StringBuilder();
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "S3GLOANADOperatingLeaseDepreciation_Add.aspx";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=OLD';";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=OLD";
    public static LoanAdmin_S3GLOANADOperatingLeaseDepreciation_Add obj_Page;
    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient objDepreciation_Client;
    S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationDataTable objS3G_LOANAD_DepreciationTable = null;
    S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationRow objS3G_LOANAD_DepreciationDataRow = null;
    int intCompanyID, intUserID;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    const int ModuleId = 7;
    const int ParamVal1 = 21;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            InitializePageValues();
            if (!IsPostBack)
            {
                txtMonthYear.Attributes.Add("readonly","readonly");
                txtMonthYear.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.Year;
                FunCurrentMonthDate();
                if (PageMode == PageModes.Create)
                PopulateLOBBranchList();
                
                CheckWhetherDenominationDaysEnabled();
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (PageMode == PageModes.Query)
                {
                    FunDepreciatedAssetForViewing(intSJVNo);
                    FunPriDisableControls(-1);
                    //ShowSysJVRow();
                }
                else if (PageMode == PageModes.Modify)
                {
                    FunDepreciatedAssetForViewing(intSJVNo);
                    FunPriDisableControls(1);
                    //ShowSysJVRow();
                }
                else
                {
                    FunPriDisableControls(0);
                }
            }
        }
        finally
        {
            if (objDepreciation_Client != null)
                objDepreciation_Client.Close();
        }
    }

    private void ShowSysJVRow()
    {
        LblJVN1.Visible = true;
        LblJVN2.Visible = true;
        txtSysJVNumber.Visible = true;
        txtSysJVDate.Visible = true;
    }

    private void CheckWhetherDenominationDaysEnabled()
    {

        objDepreciation_Client = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
        try
        {
            int DDays = objDepreciation_Client.FunPubGetGlobalParameterVale(int.Parse(CompanyId));
            if (DDays == 0)
                Utility.FunShowAlertMsg(this, "Denomination days property not defined in global parameter");
            else
                DenominationDays = DDays;
        }
        finally
        {
            objDepreciation_Client.Close();
        }

    }
    public int DenominationDays
    {
        get
        {
            return int.Parse(ViewState["DenominationDays"].ToString());
        }
        set
        {
            ViewState["DenominationDays"] = value;
        }
    }

    private void InitializePageValues()
    {
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        FunPubSetIndex(1);
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = DateFormate; // ObjS3GSession.ProDateFormatRW;
        txtCurrentDate.Attributes.Add("readonly", "readonly");
        //CalendarExtender1.Format = strDateFormat;
        //txtCurrentDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCurrentDate.ClientID + "','" + strDateFormat + "',true,  false);");
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end
        intSJVNo = PageIdValue;
    }


    #region "LOB-Operating lease/Branch load"

    private void PopulateLOBBranchList()
    {
        try
        {
            DataTable dtTable = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@User_ID", UserId);
            Procparam.Add("@Company_ID", CompanyId);
            //ddlBranch.BindDataTable(SPNames.S3G_LOANAD_GetOperatingLeaseLOBBranch, Procparam, new string[] { "Branch_ID", "Branch_Code" });

           dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetOperatingLeaseLOBBranch, Procparam);
            //ddlBranch.BindDataTable(dtTable, new string[] { "Location_ID", "Location_Code" });
            //ddlBranch.AddItemToolTip();
            if (PageMode == PageModes.Create)
            {
                if (dtTable.Rows.Count > 1)
                {
                    hdnLOBID.Value = dtTable.Rows[1]["LOB_ID"].ToString();
                    txtLOB.Text = dtTable.Rows[1]["LOB_Code"].ToString();
                    txtLOB.Attributes.Add("LOB_ID", dtTable.Rows[1]["LOB_ID"].ToString());
                    DepreciationMethod();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Operating Lease Line of Business not mapped to the user');", true);
                    return;
                }
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region "Method used to calculate depreciation"

    private void DepreciationMethod()
    {
        //throw new NotImplementedException();
        if (PageMode == PageModes.Create)
        {
            DataTable dtTable = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", CompanyId);
            Procparam.Add("@LOB_ID", txtLOB.Attributes["LOB_ID"].ToString());

            dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetDepreciationMethod, Procparam);

            if (dtTable.Rows.Count > 0)
            {
                if (Convert.ToString(dtTable.Rows[0]["Depreciation_Method"]) == "1")
                    txtDepMethod.Text = "WDV";
                else if (Convert.ToString(dtTable.Rows[0]["Depreciation_Method"]) == "2")
                    txtDepMethod.Text = "SLM";
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Depreciation method not defined for the Line of Business yet');" + strRedirectPageView, true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Depreciation method not defined for the Line of Business yet');" + strRedirectPageView, true);
                return;
            }
        }
    }

    #endregion

    #region "Base value for depreciation calculation"

    private void GetGlobalParameterDetails()
    {
        DataTable dtTable = new DataTable();
        DataView dtView = new DataView();
        DataTable dtCopy = new DataTable();
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@Module_ID", ModuleId.ToString());
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@ParameterCode", ParamVal1.ToString());

        dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetBaseValue, Procparam);


        if (dtTable.Rows.Count > 0)
        {
            if (dtTable.Rows[0]["Parameter_Value"].ToString().Length > 0)
                ViewState["BaseValue"] = dtTable.Rows[0]["Parameter_Value"];
            else
                Utility.FunShowAlertMsg(this, "Base value is not defined in Global Parameter setup");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Define Base value for depreciation in Global Parameter for the selected Line of Business');", true);
            ddlBranch.Clear();
            txtCurrentDate.Text = string.Empty;
            return;
        }
        //}

    }

    #endregion

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        grvDepreciation.DataSource = null;
        grvDepreciation.DataBind();
        pnlAssetDetails.Visible = false;

        FunGetLastDate(Convert.ToInt32(ddlBranch.SelectedValue), int.Parse(CompanyId));
        GetGlobalParameterDetails();
        EnableCalculation(true);
        UpdButtons.Update();

        //ddlBranch.AddItemToolTip();
    }

    #region "Last calculated date when depreciation has been calculated"

    private void FunGetLastDate(int intBranchID, int intCompanyID)
    {
        DataTable dtTableDate = new DataTable();
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Location_ID", intBranchID.ToString());

        dtTableDate = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetDepreciationDate, Procparam);
        if (dtTableDate.Rows.Count > 0)
        {
            txtLastDate.Text = DateTime.Parse(dtTableDate.Rows[0]["Current_Calculated_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            ViewState["DepreciationID"] = dtTableDate.Rows[0]["Depreciation_ID"].ToString();
        }
        else
            txtLastDate.Text = string.Empty;
    }

    #endregion

    private void FunGetAssetDepreciationDetail(int intBranchID, int intCompanyID)
    {
        //try
        //{

        //    int RecordsCount;
        //    //ViewState["dtTable"] = GetDepreciationRecordsForBranch(intBranchID, intCompanyID, out RecordsCount);
        //    //Calculate values            
        //    if (RecordsCount == 0)
        //    {
        //        Utility.FunShowAlertMsg(this, "No Asset found for depreciation calculation", strRedirectPageAdd);                
        //        return;
        //    }
        //    //

        //    if (RecordsCount > 0)
        //    {
        //        pnlAssetDetails.Style.Add("Display", "block");
        //        grvDepreciation.DataSource = ViewState["dtTable"] as DataTable;
        //        grvDepreciation.DataBind();
        //        btnSave.Enabled = true;
        //    }
        //    else
        //    {
        //        Utility.FunShowAlertMsg(this, "Depreciation date needs to be in the same year and month as Asset Acquisition date");                
        //        return;
        //    }             
        //} 
        //catch (Exception ex)
        //{
        //   Utility.FunShowAlertMsg(this, ex.Message, strRedirectPageAdd);
        //   throw ex;

        //  // ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('"+ex.Message +"');" + strRedirectPageAdd, true);                         
        //}
    }

    private void FunGetDepreciation(int intBranchID, int intCompanyID)
    {
        DataTable dtTable = new DataTable();
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@Location_ID", intBranchID.ToString());
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@CalculationDate", Utility.StringToDate(txtCurrentDate.Text).ToString());
        Procparam.Add("@BaseValue", ViewState["BaseValue"].ToString());
        DateTime CurrentYearStart, CurrentYearEnd, NextYearStart, NextYearEnd;
        GetFinancialYearDates(out CurrentYearStart, out CurrentYearEnd, out NextYearStart, out NextYearEnd);
        //Procparam.Add("@finYear1Start", CurrentYearStart.ToString());
        Procparam.Add("@finYear1End", CurrentYearEnd.ToString());
        if (!string.IsNullOrEmpty(txtDepMethod.Text))
        {
            Procparam.Add("@LD_Method", txtDepMethod.Text.Trim());
        }

        try
        {

            dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAssetDepreciationDetail, Procparam);
            if (dtTable.Rows.Count > 0)
            {
                pnlAssetDetails.Visible = true;
                grvDepreciation.Visible = true;
                grvDepreciation.DataSource = dtTable;
                grvDepreciation.DataBind();
                btnSave.Enabled = true;
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Records available");
                btnSave.Enabled = false;
            }


            // RecordCount = dtTable.Rows.Count;
            //return dtTable;
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message.ToString());
            throw ex;

            //RecordCount = 0;
            //return null;
        }

    }

    int FinancialYearStartMonth;
    void GetFinancialYearDates(out DateTime CurrentYearStart, out DateTime CurrentYearEnd, out DateTime NextYearStart, out DateTime NextYearEnd)
    {
        FinancialYearStartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"].ToString());
        if (DateTime.Now.Month >= FinancialYearStartMonth)
        {
            CurrentYearStart = new DateTime(DateTime.Now.AddYears(-1).Year, FinancialYearStartMonth, 01);
            CurrentYearEnd = new DateTime(CurrentYearStart.AddMonths(12).Year, FinancialYearStartMonth - 1, DateTime.DaysInMonth(CurrentYearStart.AddMonths(12).Year, FinancialYearStartMonth - 1));
            NextYearStart = new DateTime(CurrentYearEnd.Year, FinancialYearStartMonth, 01);
            NextYearEnd = new DateTime(NextYearStart.AddMonths(12).Year, FinancialYearStartMonth - 1, DateTime.DaysInMonth(NextYearStart.AddMonths(12).Year, FinancialYearStartMonth - 1));
        }
        else
        {
            CurrentYearStart = new DateTime(DateTime.Now.Year, FinancialYearStartMonth, 01);
            CurrentYearEnd = new DateTime(CurrentYearStart.AddMonths(12).Year, FinancialYearStartMonth - 1, DateTime.DaysInMonth(CurrentYearStart.AddMonths(12).Year, FinancialYearStartMonth - 1));
            NextYearStart = new DateTime(CurrentYearEnd.Year, FinancialYearStartMonth, 01);
            NextYearEnd = new DateTime(NextYearStart.AddMonths(12).Year, FinancialYearStartMonth - 1, DateTime.DaysInMonth(NextYearStart.AddMonths(12).Year, FinancialYearStartMonth - 1));
        }
    }

    decimal DepreciationTotal, TotalAssets, DepreciationExisting, AssetsValue, CurrentValue;
    protected void grvDepreciation_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        //try
        //{
        //    // To hide Current Depreciation and Depreciation days in Query Mode
        //    if (PageMode == PageModes.Query || PageMode == PageModes.Modify)  
        //    {
        //        e.Row.Cells[6].Visible = false;
        //        e.Row.Cells[7].Visible = false;                
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lblAssetValue= e.Row.FindControl("lblValue") as Label;
        //            Label lblExistPercent= e.Row.FindControl("lblExistPercent") as Label;
        //            Label lblCurrentValue= e.Row.FindControl("lblValue") as Label;
        //            Label lblCurrentPercent = (Label)e.Row.FindControl("lblCurrentPercent");
        //            Label lblCurrentAssetValue = (Label)e.Row.FindControl("lblCurrentValue");
        //           // AccumalteGridSummary(decimal.Parse(lblCurrentPercent.Text), decimal.Parse(lblCurrentPercent.Text), 0, decimal.Parse(lblCurrentAssetValue.Text));

        //            AccumalteGridSummary(
        //                (decimal.Parse(lblCurrentPercent.Text)>0) ? decimal.Parse(lblCurrentPercent.Text) : 0,
        //                (decimal.Parse(lblCurrentPercent.Text)>0) ? decimal.Parse(lblCurrentPercent.Text) : 0,
        //                        0,
        //                (decimal.Parse(lblCurrentAssetValue.Text)>0) ? decimal.Parse(lblCurrentAssetValue.Text):0
        //                        );

        //            /*if (txtDepMethod.Text == "SLM")
        //            {
        //                e.Row.Cells[5].Text = ((Label)e.Row.FindControl("lblCurrentPercent")).Text;
        //            }
        //             */
        //        }               
        //    }

        //    if (e.Row.RowType == DataControlRowType.DataRow && PageMode==PageModes.Create )
        //    {
        //        Label lblAssetNo = (Label)e.Row.FindControl("lblAssetNo");
        //        Label lblAssetDesc = (Label)e.Row.FindControl("lblAssetDesc");
        //        Label lblAcqDate = (Label)e.Row.FindControl("lblAcqDate");
        //       // lblAcqDate.Text = DateTime.Parse(lblAcqDate.Text, CultureInfo.CurrentCulture).ToString(strDateFormat);
        //        Label lblDepPercent = (Label)e.Row.FindControl("lblDepPercent");
        //        Label lblValue = (Label)e.Row.FindControl("lblValue");
        //        Label lblExistPercent = (Label)e.Row.FindControl("lblExistPercent");
        //        Label lblCurrentPercent = (Label)e.Row.FindControl("lblCurrentPercent");
        //        Label lblCurrentValue = (Label)e.Row.FindControl("lblCurrentValue");
        //        Label lblDays = e.Row.FindControl("lblLastCalculated") as Label;
        //        intDeprDays = Convert.ToInt32(lblDays.Text);
        //        //lblAcqDate.Text = Utility.StringToDate(lblAcqDate.Text).ToString();
        //        decimal dec;
        //        decimal current;
        //        if (txtLastDate.Text == string.Empty && int.Parse(ddlBranch.SelectedValue)!=-1)
        //        {
        //            //CalculateDepreciationForFirstTime(lblAcqDate, lblDepPercent, lblValue, lblCurrentPercent, lblCurrentValue, out dec, out current);
        //            AccumalteGridSummary(0,decimal.Parse(lblValue.Text), dec, current);
        //        }
        //        else
        //        {
        //            CalculateDepreciation(lblDepPercent, lblValue, lblExistPercent, lblCurrentPercent, lblCurrentValue, out dec, out current);
        //            AccumalteGridSummary( (lblExistPercent.Text.Length>0)? decimal.Parse(lblExistPercent.Text):0,decimal.Parse(lblValue.Text), dec, current);
        //        }              
        //    }
        //if (e.Row.RowType == DataControlRowType.Footer)
        //{

        //    e.Row.Cells[0].Text = "Totals:";
        //    e.Row.Cells[1].Text = string.Format("Depreciation calculated for {0} asset(s)", TotalAssets);
        //    //e.Row.Cells[5].Text = DepreciationExisting.ToString();
        //    //e.Row.Cells[7].Text = DepreciationTotal.ToString();
        //    //    if (PageMode == PageModes.Query || PageMode == PageModes.Modify)  // Only show totals when at Create Mode
        //    //    {
        //    //        e.Row.Cells[8].Text = CurrentValue.ToString();
        //    //        e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
        //    //    }

        //    e.Row.Cells[1].HorizontalAlign =e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
        //    e.Row.Cells[5].HorizontalAlign = e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
        //    e.Row.Cells[7].HorizontalAlign = e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
        //    e.Row.Font.Bold = true;                
        //}
        //    else if (e.Row.RowType == DataControlRowType.Header)
        //    {
        //        if (txtDepMethod.Text == "SLM" && PageMode == PageModes.Query)
        //        {
        //            //e.Row.Cells[8].Text = "Total Depreciation";                    
        //            e.Row.Cells[5].Text = "Current Depreciation";
        //        }

        //    }
        //}
        //catch (Exception exp)
        //{
        //    throw exp;
        //}
    }

    private void AccumalteGridSummary(decimal EValue, decimal AValue, decimal dec, decimal current)
    {
        DepreciationExisting += EValue;
        DepreciationTotal += dec;
        AssetsValue += AValue;
        CurrentValue += current;
        TotalAssets += 1;
    }

    private void CalculateDepreciation(Label lblDepPercent, Label lblValue, Label lblExistPercent, Label lblCurrentPercent, Label lblCurrentValue, out decimal dec, out decimal current)
    {
        dec = 0;
        current = 0;
        decimal CurrentAssetValue = 0;



        // Declaration
        Decimal DepreciationPercentage = (Convert.ToDecimal(lblDepPercent.Text) / 100);

        if (txtDepMethod.Text == "SLM")
        {
            // Calculate the Current Asset Value by Subtracting the Current Depreciation Value and Existing Depreciation Value
            CurrentAssetValue = decimal.Parse(lblValue.Text) - ((lblExistPercent.Text.Length > 0) ? decimal.Parse(lblExistPercent.Text) : 0);

            if (lblValue.Text != string.Empty)
            {
                dec = Math.Round(Convert.ToDecimal(lblValue.Text) * intDeprDays / DenominationDays * DepreciationPercentage, 4);
                lblCurrentPercent.Text = dec.ToString();
                current = (dec + ((lblExistPercent.Text != string.Empty) ? Convert.ToDecimal(lblExistPercent.Text) : 0));
                // to show ZERO when current value goes below zero
                lblCurrentValue.Text = (Convert.ToDecimal(lblValue.Text) - current) > 0 ? (Convert.ToDecimal(lblValue.Text) - current).ToString() : "0"; //current.ToString();   // Show to total Depreciation Value by adding current and Existing
            }
        }
        else if (txtDepMethod.Text == "WDV")
        {
            DataTable dtTable = (DataTable)ViewState["dtTable"];
            // Calculate the Current Asset Value by Subtracting Asset Value - Current Depreciation Value
            CurrentAssetValue = decimal.Parse(lblValue.Text) - ((lblExistPercent.Text.Length > 0) ? decimal.Parse(lblExistPercent.Text) : 0);

            if (lblCurrentValue.Text != string.Empty)
                lblValue.Text = lblCurrentValue.Text;

            if (CurrentAssetValue > 0)
            {
                dec = Math.Round(Convert.ToDecimal(CurrentAssetValue) * intDeprDays / DenominationDays * DepreciationPercentage, 4);
                lblCurrentPercent.Text = dec.ToString();

                if (lblCurrentValue.Text != string.Empty)
                {
                    current = Convert.ToDecimal(lblCurrentPercent.Text); //((Convert.ToDecimal(lblCurrentPercent.Text)) + Convert.ToDecimal(lblExistPercent.Text));
                    lblCurrentValue.Text = (Convert.ToDecimal(CurrentAssetValue) - current).ToString();
                }
                else
                {
                    current = Convert.ToDecimal(CurrentAssetValue) - Convert.ToDecimal(lblCurrentPercent.Text);
                    lblValue.Text = CurrentAssetValue.ToString();
                    lblCurrentValue.Text = (current > 0) ? current.ToString() : "0"; // to show ZERO when current value goes below zero
                }
            }
        }
    }

    private void CalculateDepreciationForFirstTime(Label lblAcqDate, Label lblDepPercent, Label lblValue, Label lblCurrentPercent, Label lblCurrentValue, out decimal dec, out decimal current)
    {
        dec = 0;
        current = 0;
        if (CompareDates(lblAcqDate.Text, txtCurrentDate.Text))
        {
            Decimal DepreciationPercentage = (Convert.ToDecimal(lblDepPercent.Text) / 100);
            if (txtDepMethod.Text == "SLM")
            {
                decimal CurrentAssetValue = decimal.Parse(lblValue.Text) - ((lblCurrentPercent.Text.Length > 0) ? decimal.Parse(lblCurrentPercent.Text) : 0);

                dec = Math.Round(Convert.ToDecimal(lblValue.Text) * intDeprDays / DenominationDays * DepreciationPercentage, 4);
                lblCurrentPercent.Text = dec.ToString();

                // Current value calculation

                current = Convert.ToDecimal(lblValue.Text) - Convert.ToDecimal(lblCurrentPercent.Text);//Convert.ToDecimal(lblCurrentPercent.Text);
                lblCurrentValue.Text = (current > 0) ? current.ToString() : "0";
            }
            else if (txtDepMethod.Text == "WDV")
            {
                dec = Math.Round(Convert.ToDecimal(lblValue.Text) * intDeprDays / DenominationDays * DepreciationPercentage, 4);
                lblCurrentPercent.Text = dec.ToString();

                current = Convert.ToDecimal(lblValue.Text) - Convert.ToDecimal(lblCurrentPercent.Text);
                lblCurrentValue.Text = (current > 0) ? current.ToString() : "0";
            }
        }
    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    //base.VerifyRenderingInServerForm(control);
    //}

    #region "Schedule Now"
    protected void btnSave_Click(object sender, EventArgs e)
    {

        //if (SelectedLOB != 0 && SelectedLOB != -1) // if the selected option is not Select ALL or Nothing Selected
        //{
            //if (!LoadXMLValues(SelectedLOB))
            //{
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Please calculate the depreciation and click save.');", true);
            //    Utility.FunShowAlertMsg(this, "Please calculate the depreciation and click save.");
            //    return;
            //}
            //else
          
            FunGetLastDate(Convert.ToInt32(ddlBranch.SelectedValue), int.Parse(CompanyId));
            
            SaveDepreciationHeaderAndDetails(true, SelectedLOB);
        //}
        //else
        //{
        //    bool isDepreciationCalculated = false;
        //    ArrayList UnSavedBranches = new ArrayList();
        //    int DepreciationCalculatedBranches = 0;
        //    foreach (ListItem Branch in ddlBranch.Items)
        //    {
        //        if (int.Parse(Branch.Value) != -1 && int.Parse(Branch.Value) != 0) // if the selected option is not Select ALL or Nothing Selected
        //        {
        //            strDepreciationBuilder = new StringBuilder();
        //            if (LoadXMLValues(int.Parse(Branch.Value)))
        //            {
        //                isDepreciationCalculated = true;
        //                DepreciationCalculatedBranches++;
        //                if (!SaveDepreciationHeaderAndDetails(false, int.Parse(Branch.Value)))
        //                {
        //                    UnSavedBranches.Add(int.Parse(Branch.Value));
        //                }
        //            }
        //        }
        //    }
        //    if (UnSavedBranches.Count > 0)
        //    {
        //        string strMsg = " Document control number not defined for the following Locations." + "</br>";
        //        for (int Branches = 0; Branches < UnSavedBranches.Count; Branches++)
        //        {
        //            strMsg += "</br>" + (Branches + 1) + "." + ddlBranch.Items.FindByValue(UnSavedBranches[Branches].ToString()).Text;
        //        }
        //        lblErrorMsg.Text = strMsg;
        //        lblErrorMsg.Visible = true;
        //    }
        //    if (UnSavedBranches.Count != DepreciationCalculatedBranches)
        //    {
        //        strAlert = " Depreciation schduled successfully.";
        //        lblErrorMsg.Visible = false;
        //    }
        //    else
        //    {
        //        strAlert = " Document Sequence number not defined.";
        //    }

        //    Utility.FunShowAlertMsg(this, strAlert);
        //    return;


        //}
    }

    private bool SaveDepreciationHeaderAndDetails(bool showAlert, int branchId)
    {
        objDepreciation_Client = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
        bool isSaveSucess = true;
        try
        {
            objS3G_LOANAD_DepreciationTable = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationDataTable();
            objS3G_LOANAD_DepreciationDataRow = objS3G_LOANAD_DepreciationTable.NewS3G_LOANAD_LeaseAssetDepreciationRow();
            objS3G_LOANAD_DepreciationDataRow.Company_ID = int.Parse(CompanyId);
            objS3G_LOANAD_DepreciationDataRow.Depreciation_ID = strDepreciation;
            objS3G_LOANAD_DepreciationDataRow.LOB_ID = Convert.ToInt32(txtLOB.Attributes["LOB_ID"]);
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                objS3G_LOANAD_DepreciationDataRow.Branch_ID = branchId;
            }
            if (txtLastDate.Text.Trim().Length > 0)
                objS3G_LOANAD_DepreciationDataRow.Last_Calculated_Date = Utility.StringToDate(txtLastDate.Text);
            objS3G_LOANAD_DepreciationDataRow.Current_Calculated_Date = Utility.StringToDate(txtCurrentDate.Text);
            objS3G_LOANAD_DepreciationDataRow.Depreciation_Method = txtDepMethod.Text;
            objS3G_LOANAD_DepreciationDataRow.XMLValues = strDepreciationBuilder.ToString();
            objS3G_LOANAD_DepreciationDataRow.Created_By = int.Parse(UserId);
            DateTime CurrentYearStart, CurrentYearEnd, NextYearStart, NextYearEnd;
            GetFinancialYearDates(out CurrentYearStart, out CurrentYearEnd, out NextYearStart, out NextYearEnd);
            objS3G_LOANAD_DepreciationDataRow.Created_Date = CurrentYearEnd;

            objS3G_LOANAD_DepreciationTable.AddS3G_LOANAD_LeaseAssetDepreciationRow(objS3G_LOANAD_DepreciationDataRow);
            string strErrMsg = string.Empty;
            intErrCode = objDepreciation_Client.FunPubCreateAssetDepreciation(out strErrMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_DepreciationTable, ObjSerMode));
            if (intErrCode == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.LADP_Save);
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                //btnXL.Visible = true;
                //btnXL.Enabled = true;
                //btnSave.Enabled = false;
                if (showAlert)
                    Utility.FunShowAlertMsg(this, Resources.LocalizationResources.LADP_Save);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                isSaveSucess = true;
            }
            else if (intErrCode == -1)
            {
                string strAlert = Resources.LocalizationResources.DocNoNotDefined + " for the Location " + ddlBranch.SelectedText.ToString();
                if (showAlert)
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    Utility.FunShowAlertMsg(this, strAlert);
                isSaveSucess = false;
            }
            else if (intErrCode == -2)
            {
                string strAlert = Resources.LocalizationResources.DocNoExceeds + " for the Location " + ddlBranch.SelectedText.ToString();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                if (showAlert)
                    Utility.FunShowAlertMsg(this, strAlert);
                isSaveSucess = false;
            }
            else if (intErrCode == 5)
            {
                if (showAlert)
                    Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                isSaveSucess = false;
            }

            else if (intErrCode == 21)
            {
                if (showAlert)
                    Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                isSaveSucess = false;
            }
            else if (intErrCode == 26)
            {
                if (showAlert)
                    Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                isSaveSucess = false;
            }

            else if (intErrCode == 106)
            {
                if (showAlert)
                    Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                isSaveSucess = false;
            }

            else if (intErrCode == 106)
            {
                if (showAlert)
                    Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                isSaveSucess = false;
            }
            else if (intErrCode == 53)
            {
                if (showAlert)
                    Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                isSaveSucess = false;
            }
            else if (intErrCode > 100)
            {
                if (showAlert)
                    Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                isSaveSucess = false;
            }
           

            else
            {
                if (showAlert)
                    Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, "Error while saving Depreciation", false);
                isSaveSucess = false;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Operating Lease Depreciation");
            Utility.FunShowAlertMsg(this, "Error while saving  depreciation details", strRedirectPageAdd);
        }
        finally
        {
            objDepreciation_Client.Close();
        }
        return isSaveSucess;
    }

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    #region "Xcel Porting"

    protected void btnXL_Click(object sender, EventArgs e)
    {
        try
        {
            ExportToExcel();
        }
        catch (System.Threading.ThreadAbortException lException)
        {

        }
        finally
        {
            HttpContext.Current.Response.End();
        }
    }
    protected void ExportToExcel()
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            string attachment = "attachment; filename=DepreciationCalculation-" + txtCurrentDate.Text.ToString() + ".xls";
            Response.ContentType = "application/vnd.ms-excel"; // "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; 2007
            Response.AddHeader("content-disposition", attachment);
            Response.Charset = "";
            this.EnableViewState = false;
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            grvDepreciation.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    #endregion

    //protected void btnCalculate_Click(object sender, EventArgs e)
    //{
    //    pnlAssetDetails.Visible = true;
    //    //bool calculationFailed = false;
    //    if (ddlBranch.SelectedIndex > 0 && txtCurrentDate.Text.Trim() != string.Empty)
    //    {
    //        //FunGetDepreciation(ddlBranch.SelectedValue, CompanyId);
    //        FunGetDepreciation(Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(CompanyId));
    //    }

    //    //if (calculationFailed)
    //    //    EnableCalculation(true);
    //    //else
    //    //    EnableCalculation(false);

    //    //    UpdButtons.Update();        
    //}
    void EnableCalculation(bool EnableFlag)
    {
        //btnCalculate.Enabled = EnableFlag;
        //btnSave.Enabled = (btnCalculate.Enabled) ? false : true;
        //btnXL.Visible = (btnCalculate.Enabled) ? false : true;
        //lblErrorMsg.Visible = false;
    }
    //private bool CalculateDepreciation()
    //{
    //    bool isCalculationFailed = false;
    //    btnXL.Visible = false;
    //    if (txtLastDate.Text.Length > 0)
    //    {
    //        if (CompareDatesBeforeCalc(txtLastDate.Text, txtCurrentDate.Text))
    //        {
    //            if (CompareDates(txtLastDate.Text, txtCurrentDate.Text))
    //            {

    //                    FunGetAssetDepreciationDetail(Convert.ToInt32(ddlBranch.SelectedValue), int.Parse(CompanyId));

    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (SelectedLOB == -1)
    //        {
    //            DataSet DepereciationForAllBranches = new DataSet();
    //            int RecordsCount = 0;
    //            foreach (ListItem branchID in ddlBranch.Items)
    //            {
    //                if (branchID.Value != "0" && branchID.Value != "-1")
    //                {
    //                    DataTable BranchTable = GetDepreciationRecordsForBranch(int.Parse(branchID.Value), int.Parse(CompanyId), out RecordsCount);
    //                    if (BranchTable != null)
    //                    {
    //                        BranchTable.TableName = branchID.Value;
    //                        DepereciationForAllBranches.Tables.Add(BranchTable);
    //                    }
    //                    else
    //                    {
    //                        btnSave.Enabled = false;
    //                        return true;
    //                    }
    //                }
    //            }
    //            DepereciationForAllBranches.AcceptChanges();
    //            foreach (DataTable BranchValue in DepereciationForAllBranches.Tables)
    //            {
    //                DepereciationForAllBranches.Tables[0].Merge(BranchValue);
    //            }

    //            pnlAssetDetails.Style.Add("Display", "block");
    //            if (DepereciationForAllBranches != null && DepereciationForAllBranches.Tables.Count > 0 && DepereciationForAllBranches.Tables[0].Rows.Count > 0)
    //            {
    //                grvDepreciation.DataSource = DepereciationForAllBranches.Tables[0];
    //                grvDepreciation.DataBind();
    //                btnSave.Enabled = true;
    //            }
    //            else
    //                Utility.FunShowAlertMsg(this, "No Asset found for depreciation calculation.",strRedirectPageAdd);

    //        }
    //        else
    //            FunGetAssetDepreciationDetail(Convert.ToInt32(ddlBranch.SelectedValue), int.Parse(CompanyId));
    //    }
    //    return isCalculationFailed;
    //}
    public int SelectedLOB
    {
        get
        {
            return int.Parse(ddlBranch.SelectedValue);
        }
    }

    private bool CompareDates(string strLastDate, string strCurrentDate)
    {
        int? intDummyDeprDays;
        try
        {
            DateTime LastDate = new DateTime();
            DateTime CurrentDate = new DateTime();

            //DateTime.

            if (strLastDate != "")
            {
                LastDate = Utility.StringToDate(strLastDate);
                //Convert.ToDateTime((DateTime.Parse(strLastDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat)));
            }
            if (strCurrentDate != "")
            {
                CurrentDate = Utility.StringToDate(strCurrentDate);
                //Convert.ToDateTime((DateTime.Parse(strCurrentDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat)));
            }
            if (CurrentDate <= LastDate)
                return false;
            else
            {
                TimeSpan tmSpan = CurrentDate.Subtract(LastDate);
                //TimeSpan tmMonth = CurrentDate.sub
                if (txtLastDate.Text == string.Empty)
                    intDummyDeprDays = Convert.ToInt32(tmSpan.TotalDays) + 1;
                else
                    intDummyDeprDays = Convert.ToInt32(tmSpan.TotalDays);
                if (intDummyDeprDays > 31)
                {
                    int intCurrentYear = Convert.ToInt32(CurrentDate.Year);
                    int intLastYear = Convert.ToInt32(LastDate.Year);

                    //if ((intCurrentYear - intLastYear) == 0)
                    //{
                    int intCurrentMonth = Convert.ToInt32(CurrentDate.Month);
                    int intLastMonth = Convert.ToInt32(LastDate.Month);
                    if ((intCurrentMonth - intLastMonth) <= 9)
                    {
                        return true;
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Depreciation needs to be calculated for subsequent months');" + strRedirectPageAdd, true);
                        return false;
                    }
                    //}
                    //else
                    //{
                    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Depreciation needs to be calculated in the same year');" + strRedirectPageAdd, true);
                    //    return false;
                    //}
                }
                else
                {
                    if (txtLastDate.Text == string.Empty)
                    {
                        intDummyDeprDays = Convert.ToInt32(tmSpan.TotalDays) + 1;
                        return true;
                    }
                    else
                    {
                        intDummyDeprDays = Convert.ToInt32(tmSpan.TotalDays);
                        return true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region "Comparing dates when last calculated date is not acquisition date"

    private bool CompareDatesBeforeCalc(string strLastDate, string strCurrentDate)
    {
        try
        {
            DateTime LastDate = new DateTime();
            DateTime CurrentDate = new DateTime();
            if (strLastDate != "")
            {
                LastDate = Utility.StringToDate(strLastDate);
                //Convert.ToDateTime((DateTime.Parse(strLastDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat)));
            }
            if (strCurrentDate != "")
            {
                CurrentDate = Utility.StringToDate(strCurrentDate);

                //Convert.ToDateTime((DateTime.Parse(strCurrentDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat)));
            }
            if (CurrentDate <= LastDate)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Current date needs to be greater than Last depreciation calculated date');", true);
                txtCurrentDate.Text = "";
                txtCurrentDate.Focus();
                return false;

            }
            else
            {
                TimeSpan tmSpan = CurrentDate.Subtract(LastDate);
                //TimeSpan tmMonth = CurrentDate.sub
                if (txtLastDate.Text == string.Empty)
                    intDeprDays = Convert.ToInt32(tmSpan.TotalDays) + 1;
                else
                    intDeprDays = Convert.ToInt32(tmSpan.TotalDays);
                if (intDeprDays > 31)
                {
                    int intCurrentYear = Convert.ToInt32(CurrentDate.Year);
                    int intLastYear = Convert.ToInt32(LastDate.Year);
                    //if ((intCurrentYear - intLastYear) == 0)
                    //{
                    /*    int intCurrentMonth = Convert.ToInt32(CurrentDate.Month);
                        int intLastMonth = Convert.ToInt32(LastDate.Month);
                        if ((intCurrentMonth - intLastMonth) < 2)
                        {
                            return true;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Depreciation needs to be calculated for subsequent months');" + strRedirectPageAdd, true);
                            return false;
                        } */
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    protected void txtCurrentDate_TextChanged(object sender, EventArgs args)
    {
        //        if (Page.IsValid && ddlBranch.SelectedIndex>0)
        if (ddlBranch.SelectedValue !="0")
        {
            if ((txtCurrentDate.Text.Trim() != string.Empty))
            {
                txtSysJVDate.Text = txtCurrentDate.Text;
                FunGetLastDate(Convert.ToInt32(ddlBranch.SelectedValue), int.Parse(CompanyId));
                EnableCalculation(true);
                //UpdButtons.Update();
                //btnCalculate.Enabled = true;
                // CalculateDepreciation();
            }
        }
    }

    #region "viewing depreciated assets"

    private void FunDepreciatedAssetForViewing(string intSJVNo)
    {
        try
        {
            DataSet DepreciationDetails = new DataSet();
            objDepreciation_Client = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
            byte[] bytesDepreciatedDetails = objDepreciation_Client.FunGetOperatingDepreciationDetails(intSJVNo, int.Parse(CompanyId));
            DepreciationDetails = (DataSet)ClsPubSerialize.DeSerialize(bytesDepreciatedDetails, SerializationMode.Binary, typeof(DataTable));
            // Load Header Details from Header Table assumed as [0]                     
            //System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem(Convert.ToString(DepreciationDetails.Tables[0].Rows[0]["LocationName"]), Convert.ToString(DepreciationDetails.Tables[0].Rows[0]["Location_ID"]));

            if (DepreciationDetails.Tables[0].Rows[0]["Location_ID"].Equals(DBNull.Value))
            {
              
                ddlBranch.Clear();
            }
            else
            {
                ddlBranch.SelectedText = DepreciationDetails.Tables[0].Rows[0]["LocationName"].ToString();
                ddlBranch.SelectedValue = DepreciationDetails.Tables[0].Rows[0]["Location_ID"].ToString();
                ddlBranch.ToolTip = DepreciationDetails.Tables[0].Rows[0]["LocationName"].ToString();
                ddlBranch.ReadOnly = true;
               // ddlBranch.ClearDropDownList();
            }
             
           
            txtLOB.Text = DepreciationDetails.Tables[0].Rows[0]["LOBName"].ToString();
            txtLOB.Attributes.Add("LOB_ID", DepreciationDetails.Tables[0].Rows[0]["LOB_ID"].ToString());
            hdnLOBID.Value = DepreciationDetails.Tables[0].Rows[0]["LOB_ID"].ToString();

            txtLastDate.Text = (DepreciationDetails.Tables[0].Rows[0]["Last_Calculated_Date"].Equals(DBNull.Value)) ? "" : DateTime.Parse(DepreciationDetails.Tables[0].Rows[0]["Last_Calculated_Date"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
            txtCurrentDate.Text = DateTime.Parse(DepreciationDetails.Tables[0].Rows[0]["Current_Calculated_Date"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
            txtMonthYear.Text = Convert.ToDateTime(DepreciationDetails.Tables[0].Rows[0]["Current_Calculated_Date"].ToString()).ToString("MMM-yyyy");
            txtDepMethod.Text = DepreciationDetails.Tables[0].Rows[0]["Depreciation_Method"].ToString();
            //if (DepreciationDetails.Tables[0].Rows[0]["JV_Date"].ToString() != "")
            //    txtSysJVDate.Text = DateTime.Parse(DepreciationDetails.Tables[0].Rows[0]["JV_Date"].ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
            //txtSysJVNumber.Text = DepreciationDetails.Tables[0].Rows[0]["JV_Number"].ToString();
            // LOAD Depreciation Detail Records from Details table assumed as [1]
            if (intSJVNo == DepreciationDetails.Tables[2].Rows[0]["LastSJVNumber"].ToString() && PageMode == PageModes.Modify)
                btnDelete.Visible = true;
            else
                btnDelete.Visible = false;
            if (DepreciationDetails.Tables[0].Rows[0]["Service_Status"].ToString() == "C")
            {
                if (DepreciationDetails.Tables[1].Rows.Count > 0)
                {

                    object CurrentDepreciationObject = null;
                    CurrentDepreciationObject = DepreciationDetails.Tables[1].Compute("sum(Depreciation_Current)", "");
                    //CurrentDepreciationObject = DepreciationDetails.Tables[1].Compute("sum(Depreciation_Current)", "Depreciation_Current >= 0.0");
                    object CurrentValueObject = null;
                    CurrentValueObject = DepreciationDetails.Tables[1].Compute("sum(Current_Value)", "");
                    DataRow dRow = DepreciationDetails.Tables[1].NewRow();
                    dRow["Lease_Asset_No"] = "Total: ";
                    //dRow["Book_Depreciation_Rate"] = "Depreciation calculated for " + DepreciationDetails.Tables[1].Rows.Count;
                    dRow["Depreciation_Current"] = Convert.ToString(CurrentDepreciationObject);
                    dRow["Current_Value"] = Convert.ToString(CurrentValueObject);
                    dRow["Acquisition_Date"] = Convert.ToString(DepreciationDetails.Tables[1].Rows[DepreciationDetails.Tables[1].Rows.Count - 1]["Acquisition_Date"].ToString());


                    DepreciationDetails.Tables[1].Rows.Add(dRow);
                    grvDepreciation.DataSource = DepreciationDetails.Tables[1];
                    grvDepreciation.DataBind();
                    grvDepreciation.Rows[grvDepreciation.Rows.Count - 1].Cells[2].Text = "Depreciation calculated for " + (DepreciationDetails.Tables[1].Rows.Count - 1).ToString() + "asset/s";
                    grvDepreciation.Rows[grvDepreciation.Rows.Count - 1].Font.Bold = true;
                    btnXL.Enabled = true;
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "No Records for Depreciation");
                    btnXL.Enabled = false;
                    btnDelete.Enabled = false;

                }
            }
            else if (DepreciationDetails.Tables[0].Rows[0]["Service_Status"].ToString() == "WIP")
            {
                Utility.FunShowAlertMsg(this, "Depreciation Service Under Process");
                btnXL.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Depreciation Service is not Started");
                btnXL.Enabled = false;
                btnDelete.Enabled = false;
            }






            // Enable the Revoke Option Based on the Validation * Only if the Max Depreciation # = Current SJV Number

        }
        finally
        {
            objDepreciation_Client.Close();
        }
    }

    #endregion

    #region "Depreciation Details stored in XML format"
    /// <summary>
    /// Storing the Values of the grid as XML documnet for bulk insert in the Data base
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    private bool LoadXMLValues(int branchId)
    {
        bool RecordsAvailableForDepreciation = false;
        if (grvDepreciation.Rows.Count > 0)
        {
            // Grid sent as XMl
            strDepreciationBuilder.Append("<Root>");
            foreach (GridViewRow grvRow in grvDepreciation.Rows)
            {
                if (branchId == int.Parse(grvDepreciation.DataKeys[grvRow.RowIndex].Value.ToString()))
                {
                    RecordsAvailableForDepreciation = true;
                    Label lblAssetNo = (Label)grvRow.FindControl("lblAssetNo");
                    Label lblAssetDesc = (Label)grvRow.FindControl("lblAssetDesc");
                    Label lblAcqDate = (Label)grvRow.FindControl("lblAcqDate");
                    string lblDepPercent = ((Label)grvRow.FindControl("lblDepPercent")).Text;
                    Label lblValue = (Label)grvRow.FindControl("lblValue");
                    string lblExistPercent = ((Label)grvRow.FindControl("lblExistPercent")).Text;
                    string lblCurrentPercent = ((Label)grvRow.FindControl("lblCurrentPercent")).Text;
                    string lblCurrentValue = ((Label)grvRow.FindControl("lblCurrentValue")).Text;

                    if (lblDepPercent == string.Empty)
                        lblDepPercent = "0";
                    if (lblExistPercent == string.Empty)
                        lblExistPercent = "0";
                    if (lblCurrentPercent == string.Empty)
                        lblCurrentPercent = "0";
                    if (lblCurrentValue == string.Empty)
                        lblCurrentValue = "0";
                    if (txtLastDate.Text == string.Empty && lblCurrentValue != string.Empty && SelectedLOB != -1)
                    {
                        strDepreciationBuilder.Append("<Details Asset_No ='" + lblAssetNo.Text + "' Depereciation_Percent='" + Convert.ToDecimal(lblDepPercent) + "' Existing_Percent='" + Convert.ToDecimal(lblCurrentPercent) + "' Current_Percent='"
                            + Convert.ToDecimal(lblCurrentPercent) + "' Current_Value='" + Convert.ToDecimal(lblCurrentValue) + "' Asset_Value='" + Convert.ToDecimal(lblValue.Text) + "' /> ");
                    }
                    else
                    {
                        strDepreciationBuilder.Append("<Details Asset_No ='" + lblAssetNo.Text + "' Depereciation_Percent='" + Convert.ToDecimal(lblDepPercent) + "' Existing_Percent='" + (Convert.ToDecimal(lblExistPercent) + Convert.ToDecimal(lblCurrentPercent)) + "' Current_Percent='"
                            + Convert.ToDecimal(lblCurrentPercent) + "' Current_Value='" + Convert.ToDecimal(lblCurrentValue) + "' Asset_Value='" + Convert.ToDecimal(lblValue.Text) + "' /> ");
                    }
                }
            }

            strDepreciationBuilder.Append("</Root>");
            return RecordsAvailableForDepreciation;
        }
        return RecordsAvailableForDepreciation;
    }

    #endregion

    #region "User Authorization"

    ////This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                    btnSave.Enabled = false;
                btnDelete.Visible = false;
                btnErrorLog.Visible = false;
                pnlAssetDetails.Visible = false;
                btnXL.Visible = false;
                break;
            case 1: // Modify Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                    btnSave.Enabled = false;
                pnlAssetDetails.Style.Add("Display", "Block");
                btnXL.Visible = true;
                btnSave.Visible = false;
                //CalendarExtender1.Enabled = false;
                btnCalculate.Visible = false;
                txtCurrentDate.ReadOnly = true;
                txtCurrentDate.Attributes.Remove("onblur");
                calMonthYear.Enabled = false;
                // pnlAssetDetails.Visible = false;
                break;
            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                    Response.Redirect(strRedirectPage,false);
                if (bClearList)
                    //ddlBranch.ClearDropDownList();
                pnlAssetDetails.Style.Add("Display", "Block");
                //CalendarExtender1.Enabled = false;
                btnSave.Enabled = btnCalculate.Visible = false;
                btnXL.Visible =  true;
                //pnlAssetDetails.Visible = false;
                txtCurrentDate.ReadOnly = true;
                txtCurrentDate.Attributes.Remove("onblur");
                calMonthYear.Enabled = false;
                txtSysJVNumber.Visible = false;
                txtSysJVDate.Visible = false;
                break;
        }

    }

    ////Code end

    #endregion


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        objDepreciation_Client = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
        try
        {
            objS3G_LOANAD_DepreciationTable = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationDataTable();
            objS3G_LOANAD_DepreciationDataRow = objS3G_LOANAD_DepreciationTable.NewS3G_LOANAD_LeaseAssetDepreciationRow();
            objS3G_LOANAD_DepreciationDataRow.Company_ID = int.Parse(CompanyId);
            objS3G_LOANAD_DepreciationDataRow.Depreciation_ID = PageIdValue;
            objS3G_LOANAD_DepreciationDataRow.LOB_ID = Convert.ToInt32(txtLOB.Attributes["LOB_ID"]);
            objS3G_LOANAD_DepreciationDataRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objS3G_LOANAD_DepreciationDataRow.Last_Calculated_Date = Utility.StringToDate((txtLastDate.Text != string.Empty) ? txtLastDate.Text : DateTime.Today.ToString());
            objS3G_LOANAD_DepreciationDataRow.Current_Calculated_Date = Utility.StringToDate(txtCurrentDate.Text);
            objS3G_LOANAD_DepreciationDataRow.Depreciation_Method = txtDepMethod.Text;
            objS3G_LOANAD_DepreciationDataRow.XMLValues = int.MaxValue.ToString();
            objS3G_LOANAD_DepreciationDataRow.Created_By = int.Parse(UserId);
            objS3G_LOANAD_DepreciationDataRow.Created_Date = DateTime.Now.Date;
            objS3G_LOANAD_DepreciationTable.AddS3G_LOANAD_LeaseAssetDepreciationRow(objS3G_LOANAD_DepreciationDataRow);
            string strErrMsg = string.Empty;
            intErrCode = objDepreciation_Client.FunPubDeleteAssetDepreciation(out strErrMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_DepreciationTable, ObjSerMode));
            if (intErrCode == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.LADP_Revoke);
                //btnXL.Visible = true;
                //btnXL.Enabled = true;
                //btnSave.Enabled = false;
                //CalendarExtender1.Enabled = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else if (intErrCode == -1)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                strRedirectPageView = "";
            }
            else if (intErrCode == -2)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                strRedirectPageView = "";
            }
            else if (intErrCode == 106)
            {
                Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                return;
            }
            else if (intErrCode == 106)
            {
                Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                return;
            }
            else if (intErrCode > 100)
            {
                Utility.FunShowValidationMsg(this.Page, "OLDP", intErrCode, strErrMsg, false);
                return;
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Error while revoking Depreciation");
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, "Error while revoking  depreciation details ", strRedirectPageAdd);
        }
        finally
        {
            objDepreciation_Client.Close();
        }
    }

    protected void txtMonthYear_TextChanged(object sender, EventArgs e)
    {

        FunCurrentMonthDate();

    }

    private void FunCurrentMonthDate()
    {
        int strMonth = Utility.StringToDate(txtMonthYear.Text).Month;
        int strYear = Utility.StringToDate(txtMonthYear.Text).Year;
        int intDays = DateTime.DaysInMonth(strYear, strMonth);
        string strDate = (strMonth + "/" + intDays + "/" + strYear);
        txtCurrentDate.Text = DateTime.Parse(strDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    }
    protected void btnErrorLog_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTable = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
           //Procparam.Add("@Company_ID", CompanyId.ToString());
            Procparam.Add("@Depreciation_ID", PageIdValue);
            dtTable = Utility.GetDefaultData("S3G_LOANAD_GETDEP_ERRORLOG", Procparam);
            if (dtTable.Rows.Count <= 0)
            {
                Utility.FunShowAlertMsg(this, "No Error Log Records");
                btnErrorLog.Enabled = false;
            }
            else
            {

            string strServerPath = Server.MapPath(".").ToString() + "\\PDF Files\\OLDep" + PageIdValue.Replace("/","_") + ".txt";

            if (System.IO.File.Exists(strServerPath))
            {
                System.IO.File.Delete(strServerPath);
            }

            StreamWriter sWriter = new StreamWriter(strServerPath, false);

            if (dtTable.Rows.Count > 0)
            {
                sWriter.WriteLine("Error Log for Operating Lease Depreciation - " + PageIdValue);
                sWriter.WriteLine("_____________________________________________________________");
                sWriter.WriteLine();

                for (int i = 0; i <= dtTable.Rows.Count - 1; i++)
                {
                    sWriter.WriteLine((i + 1).ToString() + "   " + dtTable.Rows[i]["LOCATION_NAME"].ToString().ToUpper() + " - " + dtTable.Rows[i]["ERRORDESCRIPTION"].ToString());
                }

                sWriter.Flush();
                sWriter.Close();

                Process.Start("notepad.exe", strServerPath);
            }

                //if (!string.IsNullOrEmpty(strServerPath))
                //{
                //    string strFileName = strServerPath.Replace("\\", "/").Trim();

                //    Response.Clear();
                //    Response.AppendHeader("content-disposition", "attachment; filename=" + ".." + strFileName);
                //    Response.ContentType = "application/octet-stream";
                //    Response.WriteFile(strFileName);
                //    Response.End();
                //}
            }           
        }
        catch (Exception ex)
        {

        }
    }


    // Added By Shibu 24-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "066");
        Procparam.Add("@Lob_Id", obj_Page.hdnLOBID.Value);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }
}
