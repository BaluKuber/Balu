#region NameSpaces

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;
using System.Text;
using S3GBusEntity;
using System.Web.UI.WebControls;
using S3GBusEntity.LoanAdmin;
using Resources;
using System.Globalization;
using System.IO;

#endregion

//Error messages Updated to resource file Swarna 24-Jan-2011

public partial class LoanAdmin_S3GLoanAdAccountSpecificRevision : ApplyThemeForProject
{
    //const string strRedirectOnCancel = "S3gLoanAdTransLander.aspx?Code=ASR";
    #region Declaration
    ClsSystemJournal ObjSysJournal = new ClsSystemJournal();
    UserInfo ObjUserInfo = new UserInfo();
    Dictionary<string, string> Procparam = null;
    ContractMgtServicesReference.ContractMgtServicesClient ObjSpecificRevisionClient;
    protected S3GBusEntity.CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3GLoanAdAccountSpecificRevision.aspx";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdAccountSpecificRevision.aspx';";
    string strRedirectPageLandView = "../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ASR";
    public int LastSelectedLOB { get; set; }
    string strRtPattern = string.Empty;                 //Added on 14-May-2014 for IRR
    public static LoanAdmin_S3GLoanAdAccountSpecificRevision obj_Page;
    #endregion

    #region Events

    # region Page Load
    RepaymentType rePayType = new RepaymentType();
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Application Standard Date Format
        CalendarExtenderToDate.Format = DateFormate;
        #endregion
        RedirectOnCancel = "S3gLoanAdTransLander.aspx?Code=ASR";
        obj_Page = this;
        if (!IsPostBack)
        {
            // to disable the Tab intitially
            if (PageMode != PageModes.Query && PageMode != PageModes.Modify)
                tcSpecificRevision.Tabs[1].Enabled = tcSpecificRevision.Tabs[2].Enabled = false;

           
            if (PageMode == PageModes.Create)
            {
                FunPriLoadLOB();
                //FunPriLoadBranch();
            }
            if (Request.QueryString["Popup"] != null)  //transaction screen page load
                btnCancel.Enabled = false;
            // to change the view - depend on the query string
            FunPriFormActToMode();
            if (PageMode == PageModes.Create)
                txtDate.Text = DateTime.Now.ToString(DateFormate);

            // First Time 
            tcSpecificRevision.ActiveTabIndex = 0;
        }
        FunPriLoadValidationMsg();
    }
    #endregion

    #region Branch selected index changed

    protected void ddlBranchMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClear(1);
        // Load All the Available MLA's
        FunPriLoadMLA();
        // Clear the SLA's
        ddlSLA.Items.Clear();

        //txtEffectiveFrom.Text =
        ddlEffectiveFrom.Items.Clear();
        txtFinAmt.Text = "";
    }
    #endregion


    #region LOB selected index changed
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClear(1);
        // Load All the Available MLA's
        FunPriLoadMLA();
        // Clear the SLA's
        ddlSLA.Items.Clear();

        LastSelectedLOB = ddlLOB.SelectedIndex;

        ddlLOB.SelectedIndex = LastSelectedLOB;
        //load Locations
        FunPriLoadBranch();

        // ASSIGN THE VALUES TO THE STATIC CLASS VARIABLES
        //LOBCode = ddlLOB.SelectedItem.Text.Split(new char[] { '-' }).GetValue(0).ToString();
    }

    private void RRARestriction()
    {
        // Disable the Revision Details for TERM LOAN and TERM LOAN EXTENSION
        if (ddlLOB.SelectedItem.Text.Split(new char[] { '-' }).GetValue(0).ToString().ToLower().Trim() == "tl" || ddlLOB.SelectedItem.Text.Split(new char[] { '-' }).GetValue(0).ToString().ToLower().Trim() == "te")
        {
            if (ViewState["ROIRule"] != null && ViewState["ROIRule"].ToString().StartsWith("RRB"))
                CalendarExtenderToDate.OnClientDateSelectionChanged = "checkDate_OnlyProspective";
            else
                CalendarExtenderToDate.OnClientDateSelectionChanged = "";
        }
        else
        {
            CalendarExtenderToDate.OnClientDateSelectionChanged = "";
        }
    }
    #endregion

    #region MLA selected index changed
    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClear(1);
        if (!ddlMLA.SelectedValue.Equals("0"))
        {
            // Fetch The Sub Accounts List
            FunPriGetSANum();
            LoadMLARelatedDetails();
            RRARestriction();
        }
    }



    #endregion

    #region Go button

    protected void btn_click(object sender, EventArgs e)
    {
        FunPriLoadRevisionGrid(true);
    }
    #endregion

    #region Button save
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveSpecificRevision();
    }
    #endregion

    #region textbox txtRevised TextChanged
    protected void txtRevised_TextChanged(object sender, EventArgs e)
    {
        ////FunPriLoadRevisionGrid();
        //string strFieldAtt = ((TextBox)sender).ClientID;
        //string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvAccountRevisionDetails_")).Replace("grvAccountRevisionDetails_ctl", "");
        //int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
        //gRowIndex = gRowIndex - 2;
        //TextBox txtRevised = ((TextBox)sender);

        //DataTable dt = (DataTable)ViewState["AccountRevisionDetails"];
        //((TextBox)sender).Text = txtRevised.Text;
        ////(TexgrvAccountRevisionDetails.Rows[gRowIndex].FindControl("txtReviseddetails")
        //dt.Rows[gRowIndex]["Revised"] = txtRevised.Text;
        //grvAccountRevisionDetails.DataSource = dt;
        //grvAccountRevisionDetails.DataBind();
        //ViewState["AccountRevisionDetails"] = dt;

    }
    #endregion

    #region Button cancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect(RedirectOnCancel);
    }
    #endregion


    #region Button Generate bill

    private bool CalculateRevisedRepayment()
    {
        bool isRevisionValueEntered = false;
        string ValuesRevised = "sum(Revised)";
        if (LoadRevisedValues() != null)
        {
            DataTable RevisedValues = (DataTable)ViewState["AccountRevisionDetails"];
            if (decimal.Parse(RevisedValues.Rows[1]["Revised"].ToString()) == 0 && decimal.Parse(RevisedValues.Rows[2]["Revised"].ToString()) == 0 && decimal.Parse(RevisedValues.Rows[0]["Revised"].ToString()) == 0
                && decimal.Parse(RevisedValues.Rows[3]["Revised"].ToString()) == 0 && decimal.Parse(RevisedValues.Rows[4]["Revised"].ToString()) == 0)
            //if (decimal.Parse(LoadRevisedValues().Compute(ValuesRevised, "").ToString()) > 0) // IF revised values are valid
            {
                Utility.FunShowAlertMsg(this, "Atleast one revision value must be entered");
                btnSave.Enabled = false;
            }
            if (decimal.Parse(RevisedValues.Rows[0]["Revised"].ToString()) > decimal.Parse(RevisedValues.Rows[0]["Existing"].ToString()))
            {
                Utility.FunShowAlertMsg(this, "Revision Amount Should not greater than Existing Finance Amount");
                btnSave.Enabled = false;
            }
            else
            {
                isRevisionValueEntered = FunGenerateBill();
            }
        }
        return isRevisionValueEntered;
    }
    DataTable LoadRevisedValues()
    {
        DataTable ExistingAndRevisedValues = new DataTable();
        DataTable ExistingAndRevisedSOH = new DataTable();
        if (ViewState["AccountRevisionDetails"] == null)
            ExistingAndRevisedValues = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetExistingRevised, Procparam);
        else
            ExistingAndRevisedValues = (DataTable)ViewState["AccountRevisionDetails"];

        if (ViewState["AccountRevisionDetails"] == null)
            ExistingAndRevisedValues = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetExistingRevised, Procparam);
        else
            ExistingAndRevisedValues = (DataTable)ViewState["AccountRevisionDetails"];

        if (ExistingAndRevisedValues != null && ExistingAndRevisedValues.Rows.Count > 0)
        {
            // if no revised row is available - add it here
            if (ExistingAndRevisedValues.Rows.Count == 1)
            {
                DataRow dr_revised = ExistingAndRevisedValues.NewRow();
                ExistingAndRevisedValues.Rows.Add(dr_revised);
            }
            Dictionary<string, string> Columns = new Dictionary<string, string>();
            Columns.Add("Existing", "System.Decimal");
            Columns.Add("Revised", "System.Decimal");
            if (ViewState["AccountRevisionDetails"] == null)
                ExistingAndRevisedValues = Utility.FunConvertRowToColumn(ExistingAndRevisedValues, "Type", Columns);


            if (grvAccountRevisionDetails.Rows.Count >= 3)
            {
                TextBox NewFinanceAmount = grvAccountRevisionDetails.Rows[0].FindControl("txtRevisedValue") as TextBox;
                TextBox NewRate = grvAccountRevisionDetails.Rows[1].FindControl("txtRevisedValue") as TextBox;
                TextBox NewTenure = grvAccountRevisionDetails.Rows[2].FindControl("txtRevisedValue") as TextBox;
                TextBox NewResidualValue = null; TextBox NewResidualAmount = null;
                if (grvAccountRevisionDetails.Rows.Count > 3)//newly added residual value
                {
                    NewResidualValue = grvAccountRevisionDetails.Rows[3].FindControl("txtRevisedValue") as TextBox;
                    NewResidualAmount = grvAccountRevisionDetails.Rows[4].FindControl("txtRevisedValue") as TextBox;
                }


                // Get the Revised Finance Amount
                if (NewFinanceAmount.Text.Trim().Length > 0)
                {
                    if (decimal.Parse(NewFinanceAmount.Text) > 0)
                    {
                        ExistingAndRevisedValues.Rows[0]["Revised"] = Convert.ToDecimal(NewFinanceAmount.Text.Trim()); // Convert.ToDecimal(ExistingAndRevisedValues.Rows[0]["Existing"].ToString()) +Convert.ToDecimal(ExistingAndRevisedValues.Rows[0]["Existing"].ToString()) + 
                    }
                    else if (decimal.Parse(NewFinanceAmount.Text) == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Finanace amount cannot be zero");
                        return null;
                    }
                    else if ((Convert.ToDecimal(NewFinanceAmount.Text.Trim()) + Convert.ToDecimal(ExistingAndRevisedValues.Rows[0]["Existing"].ToString())) <= 0)
                    {
                        Utility.FunShowAlertMsg(this, "Finanace amount cannot be less than zero");
                        return null;
                    }
                    else
                        ExistingAndRevisedValues.Rows[0]["Revised"] = Convert.ToDecimal(NewFinanceAmount.Text.Trim());
                }
                else
                {
                    ExistingAndRevisedValues.Rows[0]["Revised"] = "0";
                }
                //else if(isNewRevision)
                //    dt.Rows[0]["Revised"] = dt.Rows[0]["Existing"].ToString();

                if (NewRate.Text.Trim().Length > 0)
                {
                    if (decimal.Parse(NewRate.Text) > 0 && decimal.Parse(NewRate.Text) < 40)
                    {
                        ExistingAndRevisedValues.Rows[1]["Revised"] = NewRate.Text.Trim();
                    }
                    else if (decimal.Parse(NewRate.Text) > 40)
                    {
                        //Utility.FunShowAlertMsg(this, "Unrealistic rate of interest");
                        //return null;
                        ExistingAndRevisedValues.Rows[1]["Revised"] = NewRate.Text.Trim(); // Modified By Rao 19 July...
                    }
                    else
                    {

                        Utility.FunShowAlertMsg(this, "Rate cannot be zero");
                        return null;
                    }
                }
                else
                {
                    ExistingAndRevisedValues.Rows[1]["Revised"] = "0";
                }
                //else if (isNewRevision)
                //    dt.Rows[1]["Revised"] = dt.Rows[1]["Existing"].ToString();

                if (NewTenure.Text.Trim().Length > 0)
                {
                    if (decimal.Parse(NewTenure.Text) > 0)
                    {
                        ExistingAndRevisedValues.Rows[2]["Revised"] = NewTenure.Text.Trim();
                    }
                    else
                    {

                        Utility.FunShowAlertMsg(this, "Tenure cannot be zero");
                        return null;
                    }
                }
                else
                {
                    ExistingAndRevisedValues.Rows[2]["Revised"] = "0";
                }

                //For Residual Value/Amount
                if (NewResidualValue != null)
                {
                    if (NewResidualValue.Text.Trim().Length > 0)
                    {
                        if (decimal.Parse(NewResidualValue.Text) > 0)
                        {
                            ExistingAndRevisedValues.Rows[3]["Revised"] = Convert.ToDecimal(NewResidualValue.Text.Trim()); // Convert.ToDecimal(ExistingAndRevisedValues.Rows[0]["Existing"].ToString()) +Convert.ToDecimal(ExistingAndRevisedValues.Rows[0]["Existing"].ToString()) + 
                        }
                        else if (decimal.Parse(NewResidualValue.Text) == 0)
                        {
                            Utility.FunShowAlertMsg(this, "Residual Value cannot be zero");
                            return null;
                        }
                        else if ((Convert.ToDecimal(NewResidualValue.Text.Trim()) + Convert.ToDecimal(ExistingAndRevisedValues.Rows[3]["Existing"].ToString())) <= 0)
                        {
                            Utility.FunShowAlertMsg(this, "Residual Value cannot be less than zero");
                            return null;
                        }
                        else
                            ExistingAndRevisedValues.Rows[3]["Revised"] = Convert.ToDecimal(NewResidualValue.Text.Trim());
                    }
                    else
                    {
                        ExistingAndRevisedValues.Rows[3]["Revised"] = "0";
                    }
                }

                if (NewResidualAmount != null)
                {
                    if (NewResidualAmount.Text.Trim().Length > 0)
                    {
                        if (decimal.Parse(NewResidualAmount.Text) > 0)
                        {
                            ExistingAndRevisedValues.Rows[4]["Revised"] = Convert.ToDecimal(NewResidualAmount.Text.Trim()); // Convert.ToDecimal(ExistingAndRevisedValues.Rows[0]["Existing"].ToString()) +Convert.ToDecimal(ExistingAndRevisedValues.Rows[0]["Existing"].ToString()) + 
                        }
                        else if (decimal.Parse(NewResidualAmount.Text) == 0)
                        {
                            Utility.FunShowAlertMsg(this, "Residual Value cannot be zero");
                            return null;
                        }
                        else if ((Convert.ToDecimal(NewResidualAmount.Text.Trim()) + Convert.ToDecimal(ExistingAndRevisedValues.Rows[4]["Existing"].ToString())) <= 0)
                        {
                            Utility.FunShowAlertMsg(this, "Residual Value cannot be less than zero");
                            return null;
                        }
                        else
                            ExistingAndRevisedValues.Rows[4]["Revised"] = Convert.ToDecimal(NewResidualAmount.Text.Trim());
                    }
                    else
                    {
                        ExistingAndRevisedValues.Rows[4]["Revised"] = "0";
                    }
                }

                decimal rateNew, rateExisting;
                rateExisting = decimal.Parse(ExistingAndRevisedValues.Rows[1]["Existing"].ToString());
                rateNew = decimal.Parse(ExistingAndRevisedValues.Rows[1]["Revised"].ToString());
                // To Avoid Existing and Revised Details could be same
                if ((ExistingAndRevisedValues.Rows[2]["Existing"].ToString().Equals(ExistingAndRevisedValues.Rows[2]["Revised"].ToString())) && (decimal.Parse(ExistingAndRevisedValues.Rows[0]["Revised"].ToString()) == 0))
                {

                    if (rateNew == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Existing and revised values cannot be the same");
                        return null;
                    }
                    else if (rateNew == rateExisting)
                    {
                        Utility.FunShowAlertMsg(this, "Existing and revised values cannot be the same");
                        return null;
                    }
                }
                else if ((rateNew == rateExisting) && (decimal.Parse(ExistingAndRevisedValues.Rows[0]["Revised"].ToString()) == 0))
                {
                    Utility.FunShowAlertMsg(this, "Existing and revised values cannot be the same");
                    return null;
                }

                //if (Utility.StringToDate(txtEffectiveFrom.Text).Month <= Utility.StringToDate(txtDate.Text).Month)
                {
                    short ClosureValue = CheckForOpenMonth(Utility.StringToDate(txtDate.Text), 1);
                    if (ClosureValue != 5)  // Check previous month is closed
                    {
                        ClosureValue = CheckForOpenMonth(Utility.StringToDate(txtDate.Text), 2);
                        if (ClosureValue == 5)
                        {
                            Utility.FunShowAlertMsg(this, "Revision initiation date cannot fall on closed month");
                            return null;
                        }
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Previous month must be closed before initiating specific revision");
                        return null;
                    }

                }
                //else
                //{
                //    Utility.FunShowAlertMsg(this, "Revision initiation can be done only for open month");
                //    return null;
                //}


                ExistingAndRevisedValues.AcceptChanges();
            }
        }
        ViewState["AccountRevisionDetails"] = ExistingAndRevisedValues;
        return ExistingAndRevisedValues;
    }

    private short CheckForOpenMonth(DateTime ClosureDate, short type)
    {
        //To check the Effective date falls on Open Month
        // S3G_LOANAD_ValidateMonthClosure
        Dictionary<string, string> dicParams = new Dictionary<string, string>();
        dicParams.Clear();
        dicParams.Add("@company_Id", CompanyId);
        dicParams.Add("@LOB_ID", ddlLOB.SelectedValue);
        //dicParams.Add("@Branch_ID", ddlBranchMain.SelectedValue);
        dicParams.Add("@Location_Id", ddlBranchMain.SelectedValue);
        dicParams.Add("@Closure_Date", ClosureDate.ToString());
        dicParams.Add("@Type", type.ToString());
        string ClosureValue = "";
        S3GDALDBType DBType = FunPubGetDatabaseType();
        if (DBType == S3GDALDBType.ORACLE)
        {
            ClosureValue = Utility.GetTableScalarValue("S3G_LOANAD_ValidateMonthClosureForPR", dicParams);
        }
        else
        {
            ClosureValue = Utility.GetTableScalarValue("S3G_LOANAD_ValidateMonthClosure", dicParams);
        }
        if (ClosureValue.Length > 0)
            return short.Parse(ClosureValue);
        else
            return 0;
        if (ClosureValue.Length > 0)
            return short.Parse(ClosureValue);
        else
            return 0;


    }

    #endregion

    #region Button clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (PageMode == PageModes.Modify)
            Response.Redirect("S3GLoanAdAccountSpecificRevision.aspx?qsMode=C");
        else
            FunClear(0);
    }
    #endregion

    #region SLA Selecteds index change
    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunClear(1);
            if (!ddlSLA.SelectedValue.Equals("0"))
            {
                DataTable dtAccountSpecificDetails = LoadAccountSpecificDetails();

                if (dtAccountSpecificDetails != null && dtAccountSpecificDetails.Rows.Count > 0)
                {
                    txtFinAmt.Text = dtAccountSpecificDetails.Rows[0]["Finance_Amount"].ToString();
                    ViewState["AccountCreationDate"] = dtAccountSpecificDetails.Rows[0]["Creation_date"].ToString();
                    txtACDate.Text = DateTime.Parse(dtAccountSpecificDetails.Rows[0]["Creation_date"].ToString()).ToString(DateFormate);
                    ViewState["ROIRule"] = dtAccountSpecificDetails.Rows[0]["ROI_Rule_Number"].ToString();
                    RRARestriction();
                    EnableRepaymentPanel(dtAccountSpecificDetails);

                }
                if (!ddlMLA.SelectedValue.Equals("0"))
                {
                    // clearViewStateItems();
                    // Load the Customer Address and other details
                    FunPriLoadCustomerDetails();

                    FunPriLoadRevisionGrid(false);
                    // Load Existing IRR Details
                    FunPriLoadRevisionIRRGrid();

                    // Load SOH Details
                    FunPriLoadSOHGrid();
                }

            }
            else
                // Load the Customer Address and other details
                FunPriLoadCustomerDetails();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    private void EnableRepaymentPanel(DataTable dtAccountSpecificDetails)
    {
        // KR REWORK
        if (short.Parse(dtAccountSpecificDetails.Rows[0]["RepaymentId"].ToString()) == 2)
        {
            pnlRepayAuto.Visible = false;
            pnlRepayManual.Visible = true;
            btnRecalculate.Visible = true;
            ViewState["StructureAdhoc"] = "yes";
        }
        else
        {
            pnlRepayAuto.Visible = true;
            pnlRepayManual.Visible = false;
            btnRecalculate.Visible = false;
        }
    }
    #endregion


    #endregion

    #region Methods

    #region Generate bill
    private bool FunGenerateBill()
    {
        bool isBillGeneratedSuccessfully = true;
        if (!IsAllowedForRevision())
        {
            isBillGeneratedSuccessfully = false;
            return isBillGeneratedSuccessfully;
        }
        try
        {
            //ViewState["ApplyBulkRevisionLogic"]
            #region "Step 1: TO GET the Old Repayment Structure"
            // Step 1: TO GET the Old Repayment Structure
            DataTable dtRepaymentTab;
            if (ddlSLA.SelectedValue.Equals("0") || ddlSLA.SelectedIndex == -1)
                dtRepaymentTab = FunGetRepayDetails(ddlMLA.SelectedValue.ToString(), (ddlMLA.SelectedValue.ToString() + "DUMMY"));
            else
                dtRepaymentTab = FunGetRepayDetails(ddlMLA.SelectedValue.ToString(), ddlSLA.SelectedItem.ToString());

            if (dtRepaymentTab == null || dtRepaymentTab.Rows.Count <= 0)
            {
                Utility.FunShowAlertMsg(this, LocalizationResources.RepayNotExist);
                return false;
            }
            ViewState["OldRePaymentDetails"] = dtRepaymentTab;


            // Check whether Revision Date Falls on Installment Date
            DataRow[] SpecificRows = dtRepaymentTab.Select("InstallmentDate=#" + Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()) + "#");
            if (SpecificRows.Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Specific revision date must fall on installment / due date");
                btnSave.Enabled = false;
                return false;
            }

            #endregion

            #region "Step 2: Generate new Repayment Structure"

            DataTable dtAccountSpecificDetails = (DataTable)ViewState["AccountSpecificDetails"];
            string strRepaymentMode = string.Empty;
            string strReturnPattern = string.Empty;

            strRepaymentMode = dtAccountSpecificDetails.Rows[0]["RepaymentId"].ToString();
            strReturnPattern = dtAccountSpecificDetails.Rows[0]["Return_Pattern"].ToString();

            DataRow dtrSpecificRow;
            dtrSpecificRow = dtAccountSpecificDetails.NewRow();

            DataTable dtRevisedDetails = (DataTable)ViewState["AccountRevisionDetails"];

            if (dtAccountSpecificDetails != null && dtAccountSpecificDetails.Rows.Count > 0)
                dtrSpecificRow = dtAccountSpecificDetails.Rows[0];
            bool isRound;

            decimal PendingPrincipal = 0;
            string filterPrincipalCondition = "InstallmentDate<#" + Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()) + "#";

            DataRow[] RemainingPrincipalRows = dtRepaymentTab.Select(filterPrincipalCondition);

            if (RemainingPrincipalRows.Length > 0)
            {
                foreach (DataRow principal in RemainingPrincipalRows)
                {
                    PendingPrincipal += decimal.Parse(principal["PrincipalAmount"].ToString());
                }
            }

            decimal CalculatedFinanceAmount = 0;
            int CalculatedTenure;
            // Re Calculate the Revised Finance Amount and Update
            RevisedFinanceAmount = FindRevisedValues(0, false);

            // OLD WAY OF ARRIVING NEW FINANCE AMOUNT
            //CalculatedFinanceAmount= decimal.Parse(txtFinAmt.Text.Trim()) + RevisedFinanceAmount - PendingPrincipal;


            CalculatedFinanceAmount = GetRevisedFinanceAmount(dtRevisedDetails, CalculatedFinanceAmount);

            ViewState["CalculatedFinanceAmount"] = CalculatedFinanceAmount;
            RevisedRate = FindRevisedValues(1, true);  // (dtRevisedDetails.Rows[1]["Revised"] != null && dtRevisedDetails.Rows[1]["Revised"].ToString().Length > 0) ? decimal.Parse(dtRevisedDetails.Rows[1]["Revised"].ToString()) : decimal.Parse(dtRevisedDetails.Rows[1]["Existing"].ToString());
            dtRevisedDetails.Rows[1]["Revised"] = RevisedRate;

            RevisedTenure = Convert.ToInt32(FindRevisedValues(2, true)); //(dtRevisedDetails.Rows[2]["Revised"] != null && dtRevisedDetails.Rows[2]["Revised"].ToString().Length > 0) ? int.Parse(dtRevisedDetails.Rows[2]["Revised"].ToString()) : int.Parse(dtRevisedDetails.Rows[2]["Existing"].ToString());
            CalculatedTenure = RevisedTenure - RemainingPrincipalRows.Length;
            //dtRevisedDetails.Rows[2]["Revised"] = RevisedTenure;
            ViewState["CalculatedTenure"] = RevisedTenure;
            ViewState["AccountRevisionDetails"] = dtRevisedDetails;


            //revised Residual value/Amount
            RevisedResidualAmount = FindRevisedValues(4, true);//For residual Amunt

            RevisedFinanceAmount = FindRevisedValues(0, true);//For revised Fin amount
            //Repaymode type start

            switch (strRepaymentMode)
            {
                case "1":
                    if (strReturnPattern != "3" || strReturnPattern != "4" || strReturnPattern != "5")
                    {
                        rePayType = RepaymentType.EMI;
                    }
                    else
                    {
                        rePayType = RepaymentType.Others;
                    }
                    break;
                default:

                    rePayType = RepaymentType.Others;

                    break;

            }

            switch (strReturnPattern)
            {
                case "3":
                    rePayType = RepaymentType.PMPT;
                    break;
                case "4":
                    rePayType = RepaymentType.PMPL;
                    break;
                case "5":
                    rePayType = RepaymentType.PMPM;
                    break;
            }

            bool blnRecovery = false; // For OL & FL with structure fixed 
            if (Convert.ToInt32(strReturnPattern) > 2 && strRepaymentMode == "3")
            {
                blnRecovery = true;
            }


            switch (ddlLOB.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower())
            {
                case "tl":
                case "te":
                    if (strRepaymentMode == "5")
                    {
                        rePayType = RepaymentType.TLE;
                    }
                    //tenure = 1;
                    break;
                case "ft":
                    rePayType = RepaymentType.FC;
                    // tenure = 1;
                    break;
                case "wc":
                    rePayType = RepaymentType.WC;
                    //tenure = 1;
                    break;
            }

            //Repaymode Type End


            int intRoundoff = 2;

            if (ViewState["GlobalRoundOff"] != null)
            {
                if (Convert.ToString(ViewState["GlobalRoundOff"]) != "")
                    intRoundoff = int.Parse(ViewState["GlobalRoundOff"].ToString());
            }
            DateTime InstallmentStartDate = Utility.StringToDate(dtRepaymentTab.Rows[0]["InstallmentDate"].ToString());
            DataTable dtNewOldRepayStructure = new DataTable();
            //dtNewOldRepayStructure = ObjBusinessLogic.FunPubCalculateRepaymentDetails(dtrSpecificRow["Frequency_Value"].ToString(), int.Parse(dtRevisedDetails.Rows[2]["Existing"].ToString()), dtrSpecificRow["TenureDescription"].ToString(), decimal.Parse(dtRevisedDetails.Rows[0]["Existing"].ToString())
            //    , decimal.Parse(dtRevisedDetails.Rows[1]["Existing"].ToString()), rePayType, (ViewState["ResidualValue"] != null) ? Convert.ToDecimal(ViewState["ResidualValue"].ToString()) : 0, InstallmentStartDate, InstallmentStartDate, intRoundoff, out isRound, GetIRRInputValuesFromCase(2, dtrSpecificRow["TimeValue_Name"].ToString()));
            dtNewOldRepayStructure = ObjBusinessLogic.FunPubCalculateRepaymentDetails(dtrSpecificRow["Frequency_Value"].ToString(), int.Parse(dtRevisedDetails.Rows[2]["Existing"].ToString()), dtrSpecificRow["TenureDescription"].ToString(), decimal.Parse(dtRevisedDetails.Rows[0]["Existing"].ToString())
               , decimal.Parse(dtRevisedDetails.Rows[1]["Existing"].ToString()), rePayType, RevisedResidualAmount, InstallmentStartDate, InstallmentStartDate, intRoundoff, out isRound, GetIRRInputValuesFromCase(2, dtrSpecificRow["TimeValue_Name"].ToString()));
            DataTable PaidInstallments = dtNewOldRepayStructure.Clone();
            RemainingPrincipalRows = dtNewOldRepayStructure.Select(filterPrincipalCondition);
            RemainingPrincipalRows.CopyToDataTable<DataRow>(PaidInstallments, LoadOption.OverwriteChanges);
            decimal decPaidFinanceCharge = 0;

            for (int i = 0; i < PaidInstallments.Rows.Count; i++)
            {
                PaidInstallments.Rows[i]["InstallmentAmount"] = dtRepaymentTab.Rows[i]["InstallmentAmount"].ToString();
                decPaidFinanceCharge = decPaidFinanceCharge + Convert.ToDecimal(dtRepaymentTab.Rows[i]["FinanceCharges"].ToString());
            }
            ViewState["PaidFinanceCharges"] = decPaidFinanceCharge.ToString();
            DateTime NewInstallmentStartDate = DateTime.Parse(dtNewOldRepayStructure.Rows[RemainingPrincipalRows.Length]["ToDate"].ToString());

            //Added on 14-May-2014 for Handling IRR Revision Starts Here

            if (Convert.ToString(dtrSpecificRow["Return_Pattern"]) == "2")
            {
                Dictionary<string, string> objParam = new Dictionary<string, string>();
                objParam.Add("@Company_ID", Convert.ToString(CompanyId));
                objParam.Add("@Panum", ddlMLA.SelectedValue);
                DataTable dtActDtls = Utility.GetDefaultData("SP_LOANAD_GETFBDATE", objParam);

                DataTable dtEmpty = new DataTable();
                string strDocDate = Convert.ToString(dtActDtls.Rows[0]["Creation_Date"]);
                Int32 _iFbDate = Convert.ToInt32(dtActDtls.Rows[0]["FB_Date"]);
                DateTime dtDocDate = Utility.StringToDate(strDocDate);
                if (dtDocDate.Date.Day < _iFbDate)
                {
                    CalculatedTenure = CalculatedTenure - 1;
                }

                DataTable RepaymentDetailsForIRR = new DataTable();

                //DataTable RepaymentStructureTable1 = CalculateRepaymentDetails(out RepaymentDetailsForIRR, dtrASRow, RevisedTenure, RevisedFinanceAmount, RevisedRate, NewInstallmentStartDate.ToString(), int.Parse(GetGlobalIRRDetails["roundOff"].ToString()), dtCashInflow, dtCashOutflow);
                DataTable RepaymentStructureTable1 = CalculateRepaymentDetails(out RepaymentDetailsForIRR, dtrSpecificRow, CalculatedTenure, CalculatedFinanceAmount, RevisedRate, NewInstallmentStartDate.ToString(), intRoundoff, dtEmpty, GetCashInflowDetails('O'));
                RevisedRate = Convert.ToDecimal(ViewState["decRate"]);
                //dtRevisedDetails.Rows[1]["Revised"] = RevisedRate;
            }

            //Added on 14-May-2014 for Handling IRR Ends Here

            //DataTable RepaymentStructureTable1 = CalculateRepaymentDetails(out RepaymentDetailsForIRR, dtrASRow, RevisedTenure, RevisedFinanceAmount, RevisedRate, NewInstallmentStartDate.ToString(), int.Parse(GetGlobalIRRDetails["roundOff"].ToString()), dtCashInflow, dtCashOutflow);

            decimal FinAmountDecimal = 0;
            switch (strReturnPattern)
            {
                case "3"://PMPT
                case "4"://PMPL
                case "5"://PMPM
                    FinAmountDecimal = RevisedFinanceAmount;
                    break;
                default:
                    FinAmountDecimal = CalculatedFinanceAmount;
                    break;
            }


            DataTable dtNewRepay = new DataTable();
            dtNewRepay = ObjBusinessLogic.FunPubCalculateRepaymentDetails(dtrSpecificRow["Frequency_Value"].ToString(), CalculatedTenure, dtrSpecificRow["TenureDescription"].ToString(), FinAmountDecimal
            , RevisedRate, rePayType, RevisedResidualAmount, NewInstallmentStartDate, NewInstallmentStartDate, intRoundoff, out isRound, GetIRRInputValuesFromCase(2, dtrSpecificRow["TimeValue_Value"].ToString()));

            string filterNewInstallments = "InstallmentDate>=#" + Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()) + "#";


            ViewState["RevisedUMFC"] = (decimal.Parse(dtNewRepay.Rows[0]["Amount"].ToString()) > CalculatedFinanceAmount) ? decimal.Parse(dtNewRepay.Rows[0]["Amount"].ToString()) - CalculatedFinanceAmount : CalculatedFinanceAmount - decimal.Parse(dtNewRepay.Rows[0]["Amount"].ToString());

            DataRow[] FutureInstallments = dtNewRepay.Select(filterNewInstallments);
            //Code Changed by Rao. On 27 Dec 2011.
            DataTable UnPaidInstallments = dtNewOldRepayStructure.Clone();
            //DataTable UnPaidInstallments = dtRepaymentTab.Clone();

            if (FutureInstallments.Length > 0)
            {
                FutureInstallments.CopyToDataTable<DataRow>(UnPaidInstallments, LoadOption.OverwriteChanges);
            }

            //from date and no of days showing wrongly fixed on 6-Jul-2013 start
            DateTime NewInstallmentFromDate = DateTime.Parse(dtNewOldRepayStructure.Rows[RemainingPrincipalRows.Length]["FromDate"].ToString());
            int intnoofdays = Convert.ToInt32(dtNewOldRepayStructure.Rows[RemainingPrincipalRows.Length]["NoofDays"].ToString());
            UnPaidInstallments.Rows[0]["FromDate"] = NewInstallmentFromDate;
            UnPaidInstallments.Rows[0]["NoofDays"] = intnoofdays;
            UnPaidInstallments.AcceptChanges();
            //from date and no of days showing wrongly fixed on 6-Jul-2013 end

            PaidInstallments.Merge(UnPaidInstallments);
            int iNo = 1;
            foreach (DataRow RepaymentRow in PaidInstallments.Rows)
            {
                RepaymentRow.BeginEdit();
                RepaymentRow["InstallmentNo"] = iNo;
                RepaymentRow.EndEdit();
                iNo++;
            }
            PaidInstallments.AcceptChanges();
            #endregion

            #region " Display Comparison"
            var NextMonthPremium = from RepaymentDetails in dtNewRepay.AsEnumerable() select RepaymentDetails.Field<DateTime>("InstallmentDate");
            if (dtRepaymentTab != null && dtRepaymentTab.Rows.Count > 0)
            {
                if (ViewState["StructureAdhoc"] != null && ViewState["StructureAdhoc"].ToString() == "yes")
                {
                    // Display the Values
                    gvManualExisting.DataSource = dtRepaymentTab;
                    gvManualExisting.DataBind();
                }
                else
                {
                    grvBill.DataSource = dtRepaymentTab;
                    grvBill.DataBind();
                }
            }
            if (dtNewRepay != null && dtNewRepay.Rows.Count > 0)
            {
                grvBill2.DataSource = PaidInstallments; //dtNewRepay;
                grvBill2.DataBind();
                ViewState["NewRepaymentDetails"] = PaidInstallments; //dtNewRepay;                                                
            }

            #endregion
            return isBillGeneratedSuccessfully;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return isBillGeneratedSuccessfully;
            throw;
        }
    }
    private decimal GetRevisedFinanceAmount(DataTable dtRevisedDetails, decimal CalculatedFinanceAmount)
    {
        // NEW PRINCIPAL CALCULATION METHOD FROM DATASET
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("@PANUM", ddlMLA.SelectedValue.ToString());
        if (ddlSLA.SelectedIndex > 0)
            dic.Add("@SANUM", ddlSLA.SelectedItem.Text);
        dic.Add("@COMPANY_ID", CompanyId);
        dic.Add("@EffectiveDate", Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()).ToString());
        string retVal = Utility.GetTableScalarValue("S3G_LOANAD_GetPrincipalDueByAccount", dic);
        // if account type is arrear and the repayment is not started yet
        if (retVal.Trim().Length == 0)
            retVal = txtFinAmt.Text;
        CalculatedFinanceAmount = decimal.Parse(retVal);
        CalculatedFinanceAmount = RevisedFinanceAmount + decimal.Parse(retVal);
        ViewState["CalculatedFinanceAmount"] = CalculatedFinanceAmount;
        dtRevisedDetails.Rows[0]["Revised"] = RevisedFinanceAmount;
        return CalculatedFinanceAmount;
    }

    string GetIRRInputValuesFromCase(short Option, string caseValue)
    {
        string returnValue = "";
        if (Option == 1)
        {
            switch (caseValue)
            {
                case "day wise irr":
                    returnValue = "daily";
                    break;
                case "month wise irr":
                    returnValue = "monthly";
                    break;
                default:
                    returnValue = "daily";
                    break;

            }
        }
        else if (Option == 2)
        {
            switch (caseValue)
            {
                case "adv(advance)":
                case "adf(advance fbd)":
                    returnValue = "advance";
                    break;
                case "arr(arrears)":
                case "arf(arrears fbd)":
                    returnValue = "arrears";
                    break;
                default:
                    returnValue = "advance";
                    break;
            }
        }
        else if (Option == 3)
        {
            switch (caseValue)
            {
                case "1":
                    returnValue = "daily";
                    break;
                case "2":
                    returnValue = "monthly";
                    break;
                default:
                    returnValue = "daily";
                    break;
            }
        }
        else if (Option == 4)
        {
            switch (caseValue)
            {
                case "1":
                case "3":
                    returnValue = "advance";
                    break;
                case "2":
                case "4":
                    returnValue = "arrears";
                    break;
                default:
                    returnValue = "advance";
                    break;
            }
        }
        return returnValue;
    }

    private decimal FindRevisedValues(int optionValue, bool returnExisting)
    {
        DataTable dtRevisedDetails = (DataTable)ViewState["AccountRevisionDetails"];
        if (returnExisting)
            return (dtRevisedDetails.Rows[optionValue]["Revised"] != null && dtRevisedDetails.Rows[optionValue]["Revised"].ToString().Length > 0 && decimal.Parse(dtRevisedDetails.Rows[optionValue]["Revised"].ToString()) != 0) ? decimal.Parse(dtRevisedDetails.Rows[optionValue]["Revised"].ToString()) : decimal.Parse(dtRevisedDetails.Rows[optionValue]["Existing"].ToString());
        else
            return (dtRevisedDetails.Rows[optionValue]["Revised"] != null && dtRevisedDetails.Rows[optionValue]["Revised"].ToString().Length > 0) ? decimal.Parse(dtRevisedDetails.Rows[optionValue]["Revised"].ToString()) : decimal.Parse(dtRevisedDetails.Rows[optionValue]["Existing"].ToString());
    }
    DataTable CalculateRepaymentDetails(out DataTable RepaymentTable, DataRow AccountSpecificRow, int Tenure, decimal FinanceAmount, decimal RateofInterest, string DocDate, int RoundOff, DataTable DtCashFlow, DataTable DtCashFlowOut)
    {
        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
        Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
        DataSet dsRepayGrid = new DataSet();
        DataTable dtMoratorium = null;
        objMethodParameters.Add("LOB", ddlLOB.SelectedItem.Text);
        objMethodParameters.Add("Tenure", Tenure.ToString());
        objMethodParameters.Add("TenureType", AccountSpecificRow["TenureDescription"].ToString());
        objMethodParameters.Add("FinanceAmount", FinanceAmount.ToString());
        //objMethodParameters.Add("ReturnPattern", AccountSpecificRow["RetunPattern"].ToString());
        objMethodParameters.Add("ReturnPattern", AccountSpecificRow["Return_Pattern"].ToString());
        objMethodParameters.Add("MarginPercentage", AccountSpecificRow["Margin_Percentage"].ToString());
        objMethodParameters.Add("Rate", RateofInterest.ToString());
        objMethodParameters.Add("TimeValue", AccountSpecificRow["TimeValue_Value"].ToString());
        //objMethodParameters.Add("RepaymentMode", AccountSpecificRow["Repayment"].ToString());
        objMethodParameters.Add("RepaymentMode", AccountSpecificRow["RepaymentId"].ToString());
        objMethodParameters.Add("CompanyId", CompanyId);
        objMethodParameters.Add("LobId", ddlLOB.SelectedValue);
        //objMethodParameters.Add("DocumentDate", DocDate); // Effective From Date
        objMethodParameters.Add("Frequency", AccountSpecificRow["Frequency_Name"].ToString());
        objMethodParameters.Add("RecoveryYear1", AccountSpecificRow["Recovery_Pattern_Year1"].ToString());
        objMethodParameters.Add("RecoveryYear2", AccountSpecificRow["Recovery_Pattern_Year2"].ToString());
        objMethodParameters.Add("RecoveryYear3", AccountSpecificRow["Recovery_Pattern_Year3"].ToString());
        objMethodParameters.Add("RecoveryYear4", AccountSpecificRow["Recovery_Pattern_Rest"].ToString());
        objMethodParameters.Add("Roundoff", RoundOff.ToString());

        if (AccountSpecificRow["Return_Pattern"].ToString() == "2")
        {
            //objMethodParameters.Add("decResidualAmount", txtResidualAmt_Cashflow.Text);            
            switch (AccountSpecificRow["IRR_Rest"].ToString())
            {
                case "1":
                    objMethodParameters.Add("strIRRrest", "daily");
                    break;
                case "2":
                    objMethodParameters.Add("strIRRrest", "monthly");
                    break;
                default:
                    objMethodParameters.Add("strIRRrest", "daily");
                    break;

            }
            objMethodParameters.Add("decLimit", "0.10");
            decimal decRateOut = 0;

            //Added on 14-May-2014 for IRR rate Change Starts Here
            DataTable dtinflow = new DataTable();

            DataTable dtoutflow = new DataTable();
            dtoutflow.Columns.Add("Date");
            dtoutflow.Columns.Add("CashOutFlowID");
            dtoutflow.Columns.Add("CashOutFlow");
            dtoutflow.Columns.Add("EntityID");
            dtoutflow.Columns.Add("Entity");
            dtoutflow.Columns.Add("OutflowFromId");
            dtoutflow.Columns.Add("OutflowFrom");
            dtoutflow.Columns.Add("Amount");
            dtoutflow.Columns.Add("Accounting_IRR");
            dtoutflow.Columns.Add("Business_IRR");
            dtoutflow.Columns.Add("Company_IRR");
            dtoutflow.Columns.Add("CashFlow_Flag_ID");
            dtoutflow.Columns["Amount"].DataType = typeof(decimal);
            dtoutflow.PrimaryKey = new DataColumn[] { dtoutflow.Columns["CashOutFlowID"], dtoutflow.Columns["Date"], dtoutflow.Columns["EntityID"] };
            for (int i = 0; i < DtCashFlowOut.Rows.Count; i++)
            {
                DataRow dr_out = dtoutflow.NewRow();
                dr_out["Date"] = DtCashFlowOut.Rows[i]["Cashflow_date"];
                dr_out["CashOutFlowID"] = DtCashFlowOut.Rows[i]["Component_Code"];
                dr_out["CashOutFlow"] = DtCashFlowOut.Rows[i]["Cashflow_description"];
                dr_out["EntityID"] = DtCashFlowOut.Rows[i]["Cashflow_Entity_Code"];
                dr_out["Entity"] = "";
                dr_out["OutflowFromId"] = DtCashFlowOut.Rows[i]["CashFlow_Entity_Type"];
                if (Convert.ToString(DtCashFlowOut.Rows[i]["CashFlow_Entity_Type"]) == "144")
                {
                    dr_out["OutflowFrom"] = "Customer";
                }
                else
                {
                    dr_out["OutflowFrom"] = "Entity";
                }
                dr_out["Amount"] = FinanceAmount;
                dr_out["Accounting_IRR"] = DtCashFlowOut.Rows[i]["Accounting_IRR"];
                dr_out["Business_IRR"] = DtCashFlowOut.Rows[i]["Business_IRR"];
                dr_out["Company_IRR"] = DtCashFlowOut.Rows[i]["Company_IRR"];
                dr_out["CashFlow_Flag_ID"] = DtCashFlowOut.Rows[i]["CashFlow_Flag_ID"];
                dtoutflow.Rows.Add(dr_out);
            }

            string strDate = Convert.ToString(DtCashFlowOut.Rows[0]["Cashflow_date"]);
            //strDate = DocDate;
            //strDate = Convert.ToString(txtACDate.Text);
            //objMethodParameters.Add("DocumentDate", strDate);
            objMethodParameters.Add("DocumentDate", DocDate); 

            //Added on 14-May-2014 for IRR rate Change Ends Here

            dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(Utility.StringToDate(DocDate), DtCashFlow, dtoutflow, objMethodParameters, dtMoratorium, out decRateOut);
            ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);

        }
        else
        {
            dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(Utility.StringToDate(DocDate), objMethodParameters, dtMoratorium);

        }
        RepaymentTable = dsRepayGrid.Tables[0];
        DataTable dtRepaymentStructure = new DataTable();
        return RepaymentTable;
    }
    private static void UpdateRepaymentAmount(decimal VariableAmoutTobeAddedNextMonth, DataRow[] dtrUpdRows)
    {
        int updateRow = 0;
        decimal remainingValue = 0;//= (decimal.Parse(dtrUpdRows[updateRow]["InstallmentAmount"].ToString()) + VariableAmoutTobeAddedNextMonth);
        if (VariableAmoutTobeAddedNextMonth <= 0)
        {
            while (VariableAmoutTobeAddedNextMonth <= 0)
            {
                VariableAmoutTobeAddedNextMonth = (decimal.Parse(dtrUpdRows[updateRow]["InstallmentAmount"].ToString()) + VariableAmoutTobeAddedNextMonth);
                dtrUpdRows[updateRow]["InstallmentAmount"] = (VariableAmoutTobeAddedNextMonth > 0) ? VariableAmoutTobeAddedNextMonth : 0;
                remainingValue = (decimal.Parse(dtrUpdRows[updateRow + 1]["InstallmentAmount"].ToString()) + VariableAmoutTobeAddedNextMonth);
                updateRow++;
            }
        }
        else
            dtrUpdRows[0]["InstallmentAmount"] = decimal.Parse(dtrUpdRows[updateRow]["InstallmentAmount"].ToString()) + VariableAmoutTobeAddedNextMonth; //decimal.Parse(dtrUpdRows[0]["InstallmentAmount"].ToString()) + VariableAmoutTobeAddedNextMonth;
    }

    bool IsAllowedForRevision()
    {
        bool isValid = true;
        if (ViewState["AccountRevisionDetails"] == null)
            isValid = false;
        else if (ViewState["AccountRevisionDetails"] != null)
        {
            DataTable RevisionDetails = (DataTable)ViewState["AccountRevisionDetails"];
            if (RevisionDetails.Compute("sum(Revised)", "") != DBNull.Value)
            {
                if ((Convert.ToDecimal(RevisionDetails.Rows[0]["Revised"].ToString()) + Convert.ToDecimal(RevisionDetails.Rows[0]["Existing"].ToString())) <= 0)
                {
                    Utility.FunShowAlertMsg(this, "Revision amount cannot be less than finance amount.");
                    isValid = false;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, LocalizationResources.FillRevision);
                isValid = false;
            }
            //var RevisionValues = from rValue in RevisionDetails.AsEnumerable() where rValue.Field<decimal>("Existing") > 0 select rValue;               
        }
        return isValid;
    }

    #endregion

    #region  Load Validation Messages
    private void FunPriLoadValidationMsg()
    {
        RFVddlLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
       // RequiredFieldValidator1.ErrorMessage = ValidationMsgs.S3G_ValMsg_Branch;//branch
        //RequiredFieldValidator2.ErrorMessage = ValidationMsgs.CLNPDC_13;//paNUM
        RequiredFieldValidator4.ErrorMessage = ValidationMsgs.S3G_ValMsg_EffectiveDate; //DAte
        RequiredFieldValidator10.ErrorMessage = ValidationMsgs.S3G_ValMsg_Status;//status
        RequiredFieldValidator3.ErrorMessage = ValidationMsgs.S3G_ValMsg_FinanceAmt;//finance amt

    }
    #endregion
    #region  Insert Revision Details
    private void SaveSpecificRevision()
    {

        try
        {
            // Archive The Repayment Details 
            //InsertSpecificRevisionArchiveDetails();
            DataTable dt = (DataTable)ViewState["AccountRevisionDetails"];
            ContractMgtServicesReference.ContractMgtServicesClient ObjSpecificRevision = new ContractMgtServicesReference.ContractMgtServicesClient();
            ContractMgtServices.S3G_LOANAD_SpecificRevisionDataTable SpecificationRevisionDataTable = new ContractMgtServices.S3G_LOANAD_SpecificRevisionDataTable();
            ContractMgtServices.S3G_LOANAD_SpecificRevisionRow SpecificationRevisionDataRow = SpecificationRevisionDataTable.NewS3G_LOANAD_SpecificRevisionRow();
            SpecificationRevisionDataRow.Company_ID = int.Parse(CompanyId);
            SpecificationRevisionDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            SpecificationRevisionDataRow.Branch_ID = Convert.ToInt32(ddlBranchMain.SelectedValue);
            SpecificationRevisionDataRow.PANum = ddlMLA.SelectedValue.ToString();
            SpecificationRevisionDataRow.SANum = GetSANum();
            SpecificationRevisionDataRow.Account_Revision_Number = string.IsNullOrEmpty(txtNumber.Text) ? "0" : txtNumber.Text;
            SpecificationRevisionDataRow.Account_Revision_Date = Utility.StringToDate(txtDate.Text);
            SpecificationRevisionDataRow.Account_Revision_Effective_Date = Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text);
            SpecificationRevisionDataRow.Revised_Finance_Amount = Convert.ToDecimal(dt.Rows[0]["Revised"].ToString()); // Convert.ToDecimal(txtFinAmt.Text.Trim()) + Convert.ToDecimal(Convert.ToDecimal(dt.Rows[0]["Revised"].ToString())); 
            if (Convert.ToDecimal(dt.Rows[1]["Revised"].ToString()) != Convert.ToDecimal(dt.Rows[1]["Existing"].ToString()))
                SpecificationRevisionDataRow.Revised_Rate = Convert.ToDecimal(Convert.ToDecimal(dt.Rows[1]["Revised"].ToString())); //  > 0) ? Convert.ToDecimal(Convert.ToDecimal(dt.Rows[1]["Revised"].ToString())) : Convert.ToDecimal(Convert.ToDecimal(dt.Rows[2]["Existing"].ToString()));
            else
                SpecificationRevisionDataRow.Revised_Rate = 0;


            //Residual value
            if (!string.IsNullOrEmpty(dt.Rows[3]["Revised"].ToString()))
            {
                if (Convert.ToDecimal(dt.Rows[3]["Revised"].ToString()) != Convert.ToDecimal(dt.Rows[3]["Existing"].ToString()))
                    SpecificationRevisionDataRow.Residual_Value = Convert.ToDecimal(Convert.ToDecimal(dt.Rows[3]["Revised"].ToString())); //  > 0) ? Convert.ToDecimal(Convert.ToDecimal(dt.Rows[1]["Revised"].ToString())) : Convert.ToDecimal(Convert.ToDecimal(dt.Rows[2]["Existing"].ToString()));
                else
                    SpecificationRevisionDataRow.Residual_Value = Convert.ToDecimal(Convert.ToDecimal(dt.Rows[3]["Existing"].ToString()));
            }

            //Residual Amount
            if (!string.IsNullOrEmpty(txtRevisionFee.Text))
            {
                SpecificationRevisionDataRow.Revision_Fee = Convert.ToDecimal(txtRevisionFee.Text);
            }

            /* IRR AREA */
            DataTable IRRDetails = (DataTable)ViewState["IRR"];
            SpecificationRevisionDataRow.Business_IRR = Convert.ToDecimal(IRRDetails.Rows[0]["Revised"].ToString());
            SpecificationRevisionDataRow.Company_IRR = Convert.ToDecimal(IRRDetails.Rows[1]["Revised"].ToString());
            SpecificationRevisionDataRow.Accounting_IRR = Convert.ToDecimal(IRRDetails.Rows[2]["Revised"].ToString());

            // Insert the REVISED Repayment Details
            SpecificationRevisionDataRow.RepaymentStructure = UpdateAccountRepayDetails("");

            // Insert the Reivsion SOH details
            SpecificationRevisionDataRow.PostingEntries = UpdateAccountPostingEntries();

            // KR FETCH CUSTOMER ID 
            SpecificationRevisionDataRow.Customer_ID = S3GCustomerAddress1.CustomerId;
            if (Convert.ToDecimal(dt.Rows[2]["Revised"].ToString()) != Convert.ToDecimal(dt.Rows[2]["Existing"].ToString()))
                SpecificationRevisionDataRow.Revised_Tenure = Convert.ToInt32(Convert.ToDecimal(dt.Rows[2]["Revised"].ToString())); // > 0) ? Convert.ToInt32(Convert.ToDecimal(dt.Rows[2]["Revised"].ToString())) : Convert.ToInt32(Convert.ToDecimal(dt.Rows[2]["Existing"].ToString()));
            else
                SpecificationRevisionDataRow.Revised_Tenure = 0;
            SpecificationRevisionDataRow.Revision_Status_Type_Code = 9;
            SpecificationRevisionDataRow.Revision_Status_Code = 1;
            SpecificationRevisionDataRow.Created_By = int.Parse(UserId);
            string strRevisionNumber = string.Empty;
            SpecificationRevisionDataTable.AddS3G_LOANAD_SpecificRevisionRow(SpecificationRevisionDataRow);
            //FunPriSysJournalEntry();
            SerializationMode SMode = SerializationMode.Binary;
            // Insert new Account Specific details
            int errcode = ObjSpecificRevision.FunPubCreateRevisionSpecification(out strRevisionNumber, SMode, ClsPubSerialize.Serialize(SpecificationRevisionDataTable, SMode), ObjSysJournal);

            if (errcode == 13)
            {
                strAlert = strRevisionNumber + " Specific revision saved successfully.";
                strAlert += @"\n\nWould you like to add one more specific revision?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else { window.location.href='" + strRedirectPageLandView + "'}";
                strRedirectPageLandView = "";
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageLandView, true);
                //Utility.FunShowAlertMsg(this, strRevisionNumber +" Specific Revision Saved Successfully.");
                FunClear(0);
            }
            else if (errcode == 14)
            {
                strKey = "Modify";
                strAlert = "Specific revision details updated successfully.";
                //strAlert = strAlert.Replace("__ALERT__", "");
                Utility.FunShowAlertMsg(this, strAlert, strRedirectPageLandView);
                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageLandView + "}";
                strRedirectPageLandView = "";
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert , true);
                //Utility.FunShowAlertMsg(this, LocalizationResources.Update);
                //FunClear(0);
                //Response.Redirect(RedirectOnCancel);
            }
            else if (errcode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                return;
            }
            else if (errcode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                return;
            }

            //txtNumber.Text = strRevisionNumber;
            //txtDate.Text = DateTime.Now.ToString(DateFormate);

        }

        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
        }
    }
    #endregion

    #region Load MLA Related Details
    void LoadMLARelatedDetails()
    {
        clearViewStateItems();
        // Load the Customer Address and other details
        FunPriLoadCustomerDetails();

        LoadAccountRelatedDetails();

        // To restrict when MLA has SLA 
        if (ddlSLA.Items.Count > 1 && PageMode == PageModes.Create)
            txtFinAmt.Text = "";

        // KR NEW 
        FunPriLoadRevisionGrid(false);

        // Load Existing IRR Details
        FunPriLoadRevisionIRRGrid();

        // Load SOH Details
        FunPriLoadSOHGrid();
    }
    #endregion

    #region Clear Dropdown
    //SwarnaLatha 25-jan2011
    private void FunClearDDl(DropDownList ddlctrl, int val)
    {
        if (ddlctrl.Items.Count > 0)
        {
            ddlctrl.SelectedIndex = 0;
            if (val == 0)
                ddlctrl.ClearDropDownList();

        }
    }
    #endregion

    #region Clear controls
    //Swarna Latha, 21- Jan -2011, When Cleared Grid controls And all controls are Cleared 
    private void FunClear(int Val)
    {
        clearViewStateItems();
        if (Val == 0)
        {
            ClearControlValues(UpdatePanel1);
            FunClearDDl(ddlLOB, 1);
            //FunClearDDl(ddlBranchMain, 1);
            ddlBranchMain.Clear();
            //FunClearDDl(ddlMLA, 0);
            ddlMLA.Clear();
            FunClearDDl(ddlSLA, 0);
        }
        txtDate.Text = DateTime.Now.ToString(DateFormate);
        txtACDate.Text = txtFinAmt.Text = string.Empty;
        ddlEffectiveFrom.Items.Clear();
        ClearGridValues(UpdatePanel1);
        S3GCustomerAddress1.ClearCustomerDetails();
        btnSave.Enabled = false;
        tcSpecificRevision.Tabs[1].Enabled = tcSpecificRevision.Tabs[2].Enabled = false;

    }
    #endregion

    #region Load Account related Details
    protected void LoadAccountRelatedDetails()
    {

        // Clear All the page controls
        //ClearControlValues(UpdatePanel1);
        // txtDate.Text = DateTime.Now.ToString(DateFormate);
        // Clear the Existing / Revised Grid
        // Clear the Existing / Revised Repayment Structure's
        // Clear the IRR Details Grid
        DataTable dtAccountSpecificDetails = LoadAccountSpecificDetails();
        if (PageMode != PageModes.Query)
        {
            ClearGridValues(UpdatePanel1);
            // Hide the Panel

            // Get the Account Specific Details


            if (dtAccountSpecificDetails != null && dtAccountSpecificDetails.Rows.Count > 0)
            {
                txtFinAmt.Text = dtAccountSpecificDetails.Rows[0]["Finance_Amount"].ToString();
                //SpecificRevision.FinanceAmount = decimal.Parse(dtAccountSpecificDetails.Rows[0]["Finance_Amount"].ToString());
                //SpecificRevision.ROIRule = dtAccountSpecificDetails.Rows[0]["ROI_Rule_Number"].ToString();
                //SpecificRevision.AccountCreationDate= DateTime.Parse(dtAccountSpecificDetails.Rows[0]["Creation_date"].ToString());
                ViewState["ROIRule"] = dtAccountSpecificDetails.Rows[0]["ROI_Rule_Number"].ToString();
                ViewState["AccountCreationDate"] = dtAccountSpecificDetails.Rows[0]["Creation_date"].ToString();
                txtACDate.Text = DateTime.Parse(dtAccountSpecificDetails.Rows[0]["Creation_date"].ToString()).ToString(DateFormate);

                //Added on 14-May-2014 starts here                
                strRtPattern = Convert.ToString(dtAccountSpecificDetails.Rows[0]["Return_Pattern"]);
                //Added on 14-May-2014 Ends here

                EnableRepaymentPanel(dtAccountSpecificDetails);
            }
        }

    }
    #endregion

    #region Mode Based Value
    /// <summary>
    /// To change the form depending on the Mode set by the Query String.
    /// </summary>
    private void FunPriFormActToMode()
    {
        switch (PageMode)
        {
            case PageModes.Query:   // query Mode
                lblAccountSpecificRevision.Text = FunPubGetPageTitles(enumPageTitle.View);
                FunPriLockControls(false);
                txtFinAmt.Enabled =
                imgEffectiveFrom.Visible =
                ddlSLA.Enabled =
                ddlMLA.Enabled =
                ddlBranchMain.Enabled =
                btnGenerateRevision.Enabled =
                ddlLOB.Enabled =
                btnGO.Enabled =
                ddlEffectiveFrom.Enabled =
                btnSave.Enabled = false;
                FunPriLoadFormControls();
                break;
            case PageModes.Modify:   // modify Mode
                lblAccountSpecificRevision.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                FunPriLockControls(true);
                btnSave.Enabled = false;
                FunPriLoadFormControls();
                btnClear.Enabled = false;
                if (ddlRevisionStatus.SelectedItem.Text.ToLower().Contains("pending"))
                    btnGO.Enabled = btnGenerateRevision.Enabled = true;
                else
                    btnGO.Enabled = btnGenerateRevision.Enabled = false;
                //CalendarExtenderToDate.Enabled = false;
                break;
            case PageModes.Create:   // create mode
                lblAccountSpecificRevision.Text = FunPubGetPageTitles(enumPageTitle.Create);
                ddlRevisionStatus.SelectedIndex = 1;
                btnSave.Enabled = false;
                btnBack.Visible = false;
                btnGO.Enabled = true;
                break;
        }

    }
    #endregion

    #region Load form controls No parameter
    private void FunPriLoadFormControls()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", CompanyId);
            Procparam.Add("@Request_No", PageIdValue);
            if (ddlRevisionStatus.SelectedItem.Text.ToString().ToLower() == "approved")
            {
                Procparam.Add("@Approved", "1");
            }
            DataSet SpecificRevisionDetails = Utility.GetDataset(SPNames.S3G_LoanAd_GetSpecificRevision, Procparam);
            FunPriLoadFormControls(SpecificRevisionDetails.Tables[0]);
            if (PageMode == PageModes.Query || PageMode == PageModes.Modify)
            {
                if (ViewState["StructureAdhoc"] != null && ViewState["StructureAdhoc"].ToString() == "yes")
                {
                    gvManualExisting.DataSource = SpecificRevisionDetails.Tables[1];
                    gvManualExisting.DataBind();
                    GrvSARevised.Visible = true;
                    GrvSARevised.DataSource = SpecificRevisionDetails.Tables[2];
                    GrvSARevised.DataBind();
                    btnRecalculate.Enabled = false;
                }
                else
                {
                    grvBill.DataSource = SpecificRevisionDetails.Tables[1];
                    grvBill.DataBind();
                    ViewState["OldRePaymentDetails"] = SpecificRevisionDetails.Tables[1];
                    grvBill2.DataSource = SpecificRevisionDetails.Tables[2];
                    grvBill2.DataBind();
                    ViewState["NewRepaymentDetails"] = SpecificRevisionDetails.Tables[2];
                    pnlRepayManual.Visible = false;
                }
                //if (ddlMLA.Items.Count > 0)
                //    ddlMLA.ClearDropDownList();
                ddlMLA.ReadOnly = true;
                //if (ddlBranchMain.Items.Count > 0)
                //    ddlBranchMain.ClearDropDownList();
                //if (ddlLOB.Items.Count > 0)
                //    ddlLOB.ClearDropDownList();
                //if (ddlSLA.Items.Count > 0)
                //    ddlSLA.ClearDropDownList();


            }
        }
        //SW 17-jan
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            
            throw ex;
        }
        //
    }
    #endregion

    #region Load form controls DataTable as parameter
    private void FunPriLoadFormControls(DataTable dtSpecificRevision)
    {
        try
        {
            if (dtSpecificRevision != null && dtSpecificRevision.Rows.Count > 0)
            {
                //ddlBranchMain.SelectedValue = Convert.ToString(dtSpecificRevision.Rows[0]["Branch_ID"]);
                ddlBranchMain.SelectedValue = Convert.ToString(dtSpecificRevision.Rows[0]["Location_ID"]);
                ddlBranchMain.SelectedText = Convert.ToString(dtSpecificRevision.Rows[0]["Location_Name"]);
                ddlBranchMain.ToolTip = Convert.ToString(dtSpecificRevision.Rows[0]["Location_Name"]);
               
                ddlLOB.Items.Add(new ListItem(dtSpecificRevision.Rows[0]["LOB_Name"].ToString(), dtSpecificRevision.Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = Convert.ToString(dtSpecificRevision.Rows[0]["LOB_Name"]);

                // KR Load MLA's 
                //FunPriLoadMLA();
                ddlRevisionStatus.SelectedValue = dtSpecificRevision.Rows[0]["Revision_Status_Code"].ToString();
              
                // Disable the option once the Revision is Approved
               if (ddlRevisionStatus.SelectedItem.Text.ToString().ToLower() == "approved")
                {
                    btnSave.Enabled = false;
                    btnBack.Enabled = false;
                }
                if (ddlRevisionStatus.SelectedItem.Text.ToString().ToLower() == "canceled")
                {
                    btnSave.Enabled = false;
                    btnBack.Enabled = false;
                    btnClear.Enabled = false;
                }
                if (ddlRevisionStatus.SelectedItem.Text.ToString().ToLower() == "pending" && PageMode == PageModes.Modify)
                    btnBack.Enabled = true;
                else
                    btnBack.Visible = false;

                ddlMLA.SelectedValue = Convert.ToString(dtSpecificRevision.Rows[0]["PANum"]);
                ddlMLA.SelectedText = Convert.ToString(dtSpecificRevision.Rows[0]["PANum"]);
                ddlMLA.ToolTip = Convert.ToString(dtSpecificRevision.Rows[0]["PANum"]);
               

                // Fetch The Sub Accounts List
                //FunPriGetSANum();
                if (!dtSpecificRevision.Rows[0]["SANum"].ToString().Contains("DUMMY") && dtSpecificRevision.Rows[0]["SANum"].ToString()!="")
                {
                    ddlSLA.Items.Add(new ListItem(dtSpecificRevision.Rows[0]["SANum"].ToString(), dtSpecificRevision.Rows[0]["SANum"].ToString()));
                    ddlSLA.ToolTip = Convert.ToString(dtSpecificRevision.Rows[0]["SANum"]);

                }
                else
                {                                   
                    ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem("--Select--", "0"));
                }
                //ddlSLA.Items.Add(new ListItem(dtSpecificRevision.Rows[0]["SANum"].ToString(), dtSpecificRevision.Rows[0]["SANum"].ToString()));
                //ddlSLA.ToolTip = Convert.ToString(dtSpecificRevision.Rows[0]["SANum"]);

                txtNumber.Text = Convert.ToString(dtSpecificRevision.Rows[0]["Account_Revision_Number"]);

                // ReLoad the MLA Related details 
                //LoadMLARelatedDetails();

                clearViewStateItems();
                // Load the Customer Address and other details
               // FunPriLoadCustomerDetails();
                S3GCustomerAddress1.SetCustomerDetails(dtSpecificRevision.Rows[0], true);

                LoadAccountRelatedDetails();

                // To restrict when MLA has SLA 
                if (ddlSLA.Items.Count > 1 && PageMode == PageModes.Create)
                    txtFinAmt.Text = "";

                // KR NEW 
                FunPriLoadRevisionGrid(false);

                // Load Existing IRR Details
                FunPriLoadRevisionIRRGrid();

                // Load SOH Details
                FunPriLoadSOHGrid();


                txtDate.Text = Convert.ToDateTime(dtSpecificRevision.Rows[0]["Account_Revision_Date"]).ToString(DateFormate);

                //CalendarExtenderToDate.Enabled = false;
                //ddlEffectiveFrom.SelectedValue = Convert.ToDateTime(dtSpecificRevision.Rows[0]["Ac_Revision_Effective_Date"]).ToString(DateFormate);
                if (btnSave.Enabled == false)
                {
                  ddlEffectiveFrom.SelectedItem.Text = Convert.ToDateTime(dtSpecificRevision.Rows[0]["Ac_Revision_Effective_Date"]).ToString(DateFormate);
                  ddlEffectiveFrom.ClearDropDownList();
                }
                else
                {
                    ddlEffectiveFrom.SelectedItem.Text = Convert.ToDateTime(dtSpecificRevision.Rows[0]["Ac_Revision_Effective_Date"]).ToString(DateFormate);
                }

                //txtEffectiveFrom.Text = Convert.ToDateTime(dtSpecificRevision.Rows[0]["Ac_Revision_Effective_Date"]).ToString(DateFormate);
                txtDate.Text = Convert.ToDateTime(dtSpecificRevision.Rows[0]["Account_Revision_Date"]).ToString(DateFormate);
                if (PageMode == PageModes.Query && PageMode == PageModes.Modify)
                    txtFinAmt.Text = Convert.ToString(dtSpecificRevision.Rows[0]["Finance_Amount"]);


                if (PageMode == PageModes.Query)
                {
                    txtFinAmt.Text = dtSpecificRevision.Rows[0]["Finance_Amount"].ToString();
                }

                // ASSGIN REVISED DETAILS MODIFY & QUERY MODE 

                //txtRevisiedRate.Text = Convert.ToString(dtSpecificRevision.Rows[0]["Revised_Rate"]);
                //txtRevisiedFinAmount.Text = Convert.ToString(dtSpecificRevision.Rows[0]["Revised_Finance_Amount"]);
                //txtRevisiedTenure.Text = Convert.ToString(dtSpecificRevision.Rows[0]["Revised_Tenure"]);


                //ddlSLA.SelectedValue = Convert.ToString(dtSpecificRevision.Rows[0]["SAnum"]);
               
                //Getting Principal Amount Starts here
                //Added on 29-Jan-2014
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                Procparam.Add("@PANUM", Convert.ToString(ddlMLA.SelectedText));
                Procparam.Add("@COMPANY_ID", Convert.ToString(CompanyId));
                Procparam.Add("@EffectiveDate", Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()).ToString());
                string retVal = Utility.GetTableScalarValue("S3G_LOANAD_GetPrincipalDueByAccount", Procparam);
                // if account type is arrear and the repayment is not started yet
                txtPriAmt.Text = retVal;

                //Getting Principal Amount Starts here

            }

            //SW 17-Jan
            else
                Utility.FunShowAlertMsg(this, LocalizationResources.RequestError);

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        //
    }
    #endregion

    #region Lock Controls
    private void FunPriLockControls(bool blnLock)
    {
        btnClear.Enabled =
         blnLock;
    }
    #endregion

    #region Load MLA

    private void FunPriLoadMLA()
    {
        //try
        //{
        //    if (ddlLOB.SelectedIndex > 0 && ddlBranchMain.SelectedIndex > 0)
        //    {
        //        if (Procparam != null)
        //            Procparam.Clear();
        //        else
        //            Procparam = new Dictionary<string, string>();

        //        Procparam.Add("@Company_ID", Convert.ToString(CompanyId));
        //        //Procparam.Add("@User_ID", UserId);

        //        // Condition already checked               if (ddlLOB != null && ddlLOB.SelectedIndex > 0)
        //        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //        // Condition already checked                if (ddlBranchMain != null && ddlBranchMain.SelectedIndex > 0)
        //        //Procparam.Add("@Branch_ID", ddlBranchMain.SelectedValue);
        //        Procparam.Add("@Location_ID", ddlBranchMain.SelectedValue);
        //        if (PageMode == PageModes.Query || PageMode == PageModes.Modify)
        //            Procparam.Add("@IsAddMode", "0");
        //        ddlMLA.BindDataTable(SPNames.S3G_LOANAD_GetPANum, Procparam, new string[] { "PANum", "PANum" });

        //    }
        //}
        //catch (Exception ex)
        //{
        //    // SW 17-Jan
        //      ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        //    throw ex;
        //    //

        //    //ViewState["MLADetails"] = null;
        //}

        ddlMLA.Clear();
    }
    #endregion

    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.CompanyId));
        Procparam.Add("@Location_ID", ((UserControls_S3GAutoSuggest)(obj_Page.ddlBranchMain)).SelectedValue.ToString());
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
       // Procparam.Add("@User_ID", obj_Page.UserId.ToString());
        Procparam.Add("@Prefix", prefixText);
        if (obj_Page.PageMode == PageModes.Query || obj_Page.PageMode == PageModes.Modify)
               Procparam.Add("@IsAddMode", "0");

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPANum_AGT", Procparam));

        return suggetions.ToArray();
    }

    #region Load LOB
    private void FunPriLoadLOB()
    {
        // LOB ComboBoxLOBSearch
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(CompanyId));
            Procparam.Add("@User_ID", UserId);
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Option", "1");
            Procparam.Add("@Program_Id", "74");
            ddlLOB.BindDataTable(SPNames.S3G_LOANAD_GetLOBCode, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }
        // SW 17-Jan
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        //
    }
    #endregion

    #region Load Branch

    private void FunPriLoadBranch()
    {
        // branch
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(CompanyId));
            //Procparam.Add("@User_ID", UserId);
            //Procparam.Add("@Program_Id", "74");
            //if (ddlLOB.SelectedIndex > 0)
            //    Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
            //if (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow)
            //    Procparam.Add("@Is_Active", "1");
            ////ddlBranchMain.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Branch_ID", "Branch" });
            //ddlBranchMain.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region Get sub account
    private void FunPriGetSANum()
    {
        try
        {           
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            Procparam.Add("@Company_ID ", Convert.ToString(CompanyId));
            Procparam.Add("@User_ID", UserId);
            ddlSLA.BindDataTable(SPNames.S3G_LOANAD_GetSANum, Procparam, new string[] { "SANum", "SANum" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region Get Customer details
    private void FunPriLoadCustomerDetails()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(CompanyId));
            Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            //KR
            DataTable dt_customer = Utility.GetDefaultData("S3G_LOANAD_GetCustomerDetailsByPAN", Procparam);
            if (dt_customer.Rows.Count > 0)
                S3GCustomerAddress1.SetCustomerDetails(dt_customer.Rows[0], true);
        }
        // SW 17-Jan
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        //
    }
    #endregion

    #region load revision grid
    private void FunPriLoadRevisionGrid(bool isNewRevision)
    {
        try
        {
            if ((ddlMLA != null && ddlMLA.SelectedValue !="0") || (ddlSLA != null && ddlSLA.SelectedIndex > 0))
            {
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();

                if (ddlMLA.SelectedValue != "0")
                    Procparam.Add("@PANUM", ddlMLA.SelectedValue.ToString());
                if (ddlSLA.SelectedIndex > 0)
                    Procparam.Add("@SANUM", ddlSLA.SelectedItem.ToString());

                //Sp   S3G_LOANAD_GetExistingRevised Parameter Changed
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                //Procparam.Add("@Branch_Id", ddlBranchMain.SelectedValue);
                Procparam.Add("@Location_Id", ddlBranchMain.SelectedValue);
                Procparam.Add("@Company_ID", CompanyId);
                Procparam.Add("@Revision_Number", PageIdValue);


                tcSpecificRevision.TabIndex = 1;
                grvAccountRevisionDetails.DataSource = null;
                grvAccountRevisionDetails.DataSource = LoadRevisedValues();
                grvAccountRevisionDetails.DataBind();
            }
            else
            {
                Utility.FunShowAlertMsg(this, LocalizationResources.Accountdetailsunavailable);
            }

        }

      // SW 17-Jan
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
        //
    }
    #endregion

    #region load revisionIRR grid
    private void FunPriLoadRevisionIRRGrid()
    {
        try
        {
            if ((ddlMLA != null && ddlMLA.SelectedValue != "0") || (ddlSLA != null && ddlSLA.SelectedIndex > 0))
            {

                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                if (ddlMLA.SelectedValue != "0")
                    Procparam.Add("@PANUM", ddlMLA.SelectedValue.ToString());
                if (ddlSLA.SelectedIndex > 0)
                    Procparam.Add("@SANUM", ddlSLA.SelectedItem.ToString());

                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                //Procparam.Add("@Branch_Id", ddlBranchMain.SelectedValue);
                Procparam.Add("@Location_Id", ddlBranchMain.SelectedValue);
                Procparam.Add("@Company_ID", CompanyId);
                Procparam.Add("@Revision_Number", PageIdValue);
                DataTable dt = new DataTable();
                dt = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetExistingRevisedIRR, Procparam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    // if no revised row is available - add it here
                    if (dt.Rows.Count == 1)
                    {
                        DataRow dr_revised = dt.NewRow();
                        dt.Rows.Add(dr_revised);
                    }

                    dt = Utility.FunConvertRowToColumn(dt, "Type", new string[] { "Existing", "Revised" });

                    dt.AcceptChanges();

                    ViewState["IRR"] = dt;

                    BindIRRGridView(dt);
                }


            }

        }

//17-jan
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
        //
    }
    private void FunPriLoadSOHGrid()
    {
        try
        {
            if ((ddlMLA != null && ddlMLA.SelectedValue != "0") || (ddlSLA != null && ddlSLA.SelectedIndex > 0))
            {
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();

                if (PageMode == PageModes.Create)
                {
                    if (ddlMLA.SelectedValue != "0")
                        Procparam.Add("@PANUM", ddlMLA.SelectedValue.ToString());
                    if (ddlSLA.SelectedIndex > 0)
                        Procparam.Add("@SANUM", ddlSLA.SelectedItem.ToString());
                }
                else
                {
                    Procparam.Add("@SpecificRevisionNo", txtNumber.Text);
                    Procparam.Add("@AddMode", "0");
                }
                Procparam.Add("@Company_ID", CompanyId);
                DataTable dt = new DataTable();
                dt = Utility.GetDefaultData("S3G_LoanAd_GetExistingSOH", Procparam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    // if no revised row is available - add it here
                    if (dt.Rows.Count == 1)
                    {
                        DataRow dr_revised = dt.NewRow();
                        dt.Rows.Add(dr_revised);
                    }

                    dt = Utility.FunConvertRowToColumn(dt, "Type", new string[] { "Existing", "Revised" });

                    dt.AcceptChanges();

                    ViewState["SOH"] = dt;

                    BindSOHGridView(dt);
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }
    #region  DateFormat
    /// <summary>
    /// Created by Tamilselvan.S
    /// Created Date 10/02/2011
    /// </summary>
    /// <param name="strDate"></param>
    /// <returns></returns>
    public string FormatDate(string strDate)
    {
        return DateTime.Parse(strDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);
    }

    #endregion
    private void BindIRRGridView(DataTable dt)
    {
        grvAccountRevisionIRRDetails.DataSource = null;
        grvAccountRevisionIRRDetails.DataSource = dt;
        grvAccountRevisionIRRDetails.DataBind();
    }

    private void BindSOHGridView(DataTable dt)
    {
        gvExistingSOH.DataSource = null;
        gvExistingSOH.DataSource = dt;
        gvExistingSOH.DataBind();
    }

    void LoanRevisedIRRGrid(double accountingIRR, double businesIRR, double companyIRR)
    {
        DataTable IRRDetails = (DataTable)ViewState["IRR"];
        if (IRRDetails != null)
        {
            if (IRRDetails.Columns.Count == 3)
            {
                IRRDetails.Rows[0]["Revised"] = businesIRR;
                IRRDetails.Rows[1]["Revised"] = companyIRR;
                IRRDetails.Rows[2]["Revised"] = accountingIRR;
                IRRDetails.AcceptChanges();
                ViewState["IRR"] = IRRDetails;
                BindIRRGridView(IRRDetails);
            }
        }
    }
    #endregion

    #region Save Revised Details
    bool InsertSpecificRevisionArchiveDetails()
    {
        try
        {
            // this is to archive details - Start
            // step 1: to get the data from the repaydetails table
            DataTable dtIRR = new DataTable("IRR");
            if (ViewState["IRR"] != null)
            {
                dtIRR = (DataTable)ViewState["IRR"];
            }

            else
            {
                Utility.FunShowAlertMsg(this, LocalizationResources.IRRcompulsary);
                return false;
            }
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            Procparam.Add("@Company_ID", CompanyId);
            Procparam.Add("@SANum", GetSANum());
            DataTable dtRepaydetails = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAccountRepayDetails_ByPASA, Procparam);

            if (dtRepaydetails != null && dtRepaydetails.Rows.Count > 0)
            {
                // step:2: this is to save the details to the Archive table
                // code to archeive details...
                string strRevision_Type_Code = string.Empty; // this want to hard code in lookup - to denote the Page type.
                string strRevision_Type = string.Empty; // this want to hard code in lookup - to denote the Page type.               
                int Repay_ID = 0;
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();


                // XML String Start          
                string strXMLAccDetails = string.Empty;
                StringBuilder strbSysJournal = new StringBuilder();
                strbSysJournal.Append("<Root> ");
                int counter = 1;
                for (int i_paydetails = 0; i_paydetails < dtRepaydetails.Rows.Count; i_paydetails++)
                {
                    int installmentamt = Convert.ToInt32(Convert.ToDecimal(dtRepaydetails.Rows[i_paydetails]["Installment_Amount"].ToString()));
                    strbSysJournal.Append(" <Details ");
                    strbSysJournal.Append(" Company_ID ='" + dtRepaydetails.Rows[i_paydetails]["Company_ID"].ToString() + "' ");
                    strbSysJournal.Append(" PANum ='" + ddlMLA.SelectedValue.ToString() + "' ");
                    strbSysJournal.Append(" SANum ='" + GetSANum() + "' ");
                    strbSysJournal.Append(" Installment_Date ='" + Utility.StringToDate(dtRepaydetails.Rows[i_paydetails]["Installment_Date"].ToString()).ToString() + "' ");
                    strbSysJournal.Append(" Installment_Period ='" + dtRepaydetails.Rows[i_paydetails]["Installment_Period"].ToString() + "' ");
                    strbSysJournal.Append(" Installment_Number ='" + dtRepaydetails.Rows[i_paydetails]["Installment_Number"].ToString() + "' ");
                    strbSysJournal.Append(" Installment_Amount ='" + dtRepaydetails.Rows[i_paydetails]["Installment_Amount"].ToString() + "' ");
                    strbSysJournal.Append(" BreakUp_Percentage ='" + dtRepaydetails.Rows[i_paydetails]["BreakUp_Percentage"].ToString() + "' ");
                    strbSysJournal.Append(" /> ");
                }
                strbSysJournal.Append(" </Root>");
                // XML String End

                ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderDataTable ObjdtArch = new ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderDataTable();
                ContractMgtServices.S3G_LOANAD_RepayDetailsIRRArchiveHeaderRow ObjSpecificationRevisionArcheiveRow = ObjdtArch.NewS3G_LOANAD_RepayDetailsIRRArchiveHeaderRow();
                ObjSpecificationRevisionArcheiveRow.Company_ID = int.Parse(CompanyId);
                ObjSpecificationRevisionArcheiveRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                ObjSpecificationRevisionArcheiveRow.Revision_ID = 0;
                ObjSpecificationRevisionArcheiveRow.Revision_Type = 1; // this is to identify the specific page
                ObjSpecificationRevisionArcheiveRow.Revision_Type_Code = 1;// this is to identify the specific page
                ObjSpecificationRevisionArcheiveRow.PANum = ddlMLA.SelectedValue.ToString();
                ObjSpecificationRevisionArcheiveRow.SANum = GetSANum();
                ObjSpecificationRevisionArcheiveRow.Repay_ID = Convert.ToInt32(dtRepaydetails.Rows[0]["RepayID"]);
                ObjSpecificationRevisionArcheiveRow.Business_IRR = Convert.ToDecimal(dtIRR.Rows[0]["Existing"]); // to do - get the existing IRR
                ObjSpecificationRevisionArcheiveRow.Company_IRR = Convert.ToDecimal(dtIRR.Rows[1]["Existing"]); // to do - get the existing IRR
                ObjSpecificationRevisionArcheiveRow.Accounting_IRR = Convert.ToDecimal(dtIRR.Rows[2]["Existing"]); // to do - get the existing IRR
                ObjSpecificationRevisionArcheiveRow.Archive_Date = DateTime.Now;
                ObjSpecificationRevisionArcheiveRow.XMLArchDetails = strbSysJournal.ToString();

                SerializationMode SerMode = SerializationMode.Binary;
                ObjdtArch.AddS3G_LOANAD_RepayDetailsIRRArchiveHeaderRow(ObjSpecificationRevisionArcheiveRow);
                ObjSpecificRevisionClient = new ContractMgtServicesReference.ContractMgtServicesClient();
                int intErrCode = ObjSpecificRevisionClient.FunPubCreateOrModifySpecificationArchive(SerMode, ClsPubSerialize.Serialize(ObjdtArch, SerMode));
                // this is to archive details - End
                ///////////////////////////////////////////////////////////
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
        return true;
    }
    #endregion

    # region method for DUMMY SLA
    private string GetSANum()
    {
        string sla = string.Empty;
        if (ddlSLA.SelectedValue.Equals("0") || ddlSLA.SelectedIndex == -1)
            sla = ddlMLA.SelectedValue.ToString() + "DUMMY";
        else
            sla = ddlSLA.SelectedItem.ToString();
        return sla;
    }
    #endregion

    #region update Repayment details
    // revised Bill - to the SQL table AccountRepayDetails

    private string UpdateAccountRepayDetails(string revisionNumber)
    {
        try
        {
            DataTable dtRepaydetails = (DataTable)ViewState["FinalRepaymentIRR"];// (DataTable)ViewState["RevisedBill"];

            // XML String Start          
            string strXMLAccDetails = string.Empty;
            StringBuilder strbSysJournal = new StringBuilder();
            strbSysJournal.Append("<Root> ");
            string sla = string.Empty;
            if (ddlSLA.SelectedValue.Equals("0") || ddlSLA.SelectedIndex == -1)
                sla = ddlMLA.SelectedValue + "DUMMY";
            else
                sla = ddlSLA.SelectedItem.Text;


            for (int i_paydetails = 0; i_paydetails < dtRepaydetails.Rows.Count; i_paydetails++)
            {
                strbSysJournal.Append(" <Details ");
                strbSysJournal.Append(" Revision_Number ='" + revisionNumber + "' ");
                strbSysJournal.Append(" Company_ID ='" + CompanyId + "' ");
                strbSysJournal.Append(" PANum ='" + ddlMLA.SelectedValue.ToString() + "' ");
                strbSysJournal.Append(" SANum ='" + GetSANum() + "' ");
                strbSysJournal.Append(" Installment_Number ='" + (i_paydetails + 1).ToString() + "' ");
                strbSysJournal.Append(" Installment_Amount ='" + dtRepaydetails.Rows[i_paydetails]["InstallmentAmount"].ToString() + "' ");
                strbSysJournal.Append(" NoofDays  ='" + dtRepaydetails.Rows[i_paydetails]["NoofDays"].ToString() + "' ");
                strbSysJournal.Append(" FromDate ='" + dtRepaydetails.Rows[i_paydetails]["FromDate"].ToString() + "' ");
                strbSysJournal.Append(" ToDate ='" + dtRepaydetails.Rows[i_paydetails]["ToDate"].ToString() + "' ");
                strbSysJournal.Append(" Installment_Date ='" + dtRepaydetails.Rows[i_paydetails]["InstallmentDate"].ToString() + "' ");
                strbSysJournal.Append(" FinanceCharges   ='" + dtRepaydetails.Rows[i_paydetails]["Charge"].ToString() + "' ");
                strbSysJournal.Append(" PrincipalAmount ='" + dtRepaydetails.Rows[i_paydetails]["PrincipalAmount"].ToString() + "' ");
                strbSysJournal.Append(" /> ");
            }
            strbSysJournal.Append(" </Root>");
            // XML String End
            return strbSysJournal.ToString();

            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Add("@XMLArchDetails", strbSysJournal.ToString());

            //Utility.GetDefaultData("S3G_LOANAD_InsertSpecificRevisionStructure", Procparam);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    private string UpdateAccountPostingEntries()
    {
        try
        {
            DataTable dtSOH = (DataTable)ViewState["SOH"];

            decimal decPaidFinanceCharges = 0;
            if (ViewState["PaidFinanceCharges"] != "" || ViewState["PaidFinanceCharges"] != null)
                decPaidFinanceCharges = Convert.ToDecimal(ViewState["PaidFinanceCharges"]);

            ContractMgtServicesReference.ContractMgtServicesClient ObjSpecificRevision = new ContractMgtServicesReference.ContractMgtServicesClient();
            ContractMgtServices.S3G_LoanAd_SpecificPostingDataTable SpecificationRevisionDataTable = new ContractMgtServices.S3G_LoanAd_SpecificPostingDataTable();
            ContractMgtServices.S3G_LoanAd_SpecificPostingRow SpecificationRevisionDataRow = SpecificationRevisionDataTable.NewS3G_LoanAd_SpecificPostingRow();

            SpecificationRevisionDataRow.Revision_Number = "";
            SpecificationRevisionDataRow.Revision_Date = DateTime.Now;
            SpecificationRevisionDataRow.PANum = ddlMLA.SelectedValue;
            if (ddlSLA.SelectedIndex > 0 && ddlSLA.SelectedValue != "-1")
                SpecificationRevisionDataRow.SANum = ddlSLA.SelectedValue;
            else
                SpecificationRevisionDataRow.SANum = ddlMLA.SelectedValue + "DUMMY";
            SpecificationRevisionDataRow.Customer_Code = "";
            SpecificationRevisionDataRow.ExistingSOH = decimal.Parse(dtSOH.Rows[0]["Existing"].ToString());
            SpecificationRevisionDataRow.ExistingUMFC = decimal.Parse(dtSOH.Rows[1]["Existing"].ToString()); //- decPaidFinanceCharges; //Subtraction of PaidFinCharges removing...
            SpecificationRevisionDataRow.NewSOH = decimal.Parse(dtSOH.Rows[0]["Revised"].ToString());
            SpecificationRevisionDataRow.NewFinAmount = decimal.Parse(dtSOH.Rows[2]["Revised"].ToString());
            SpecificationRevisionDataRow.NewUMFC = decimal.Parse(dtSOH.Rows[1]["Revised"].ToString()); //- decimal.Parse(dtSOH.Rows[1]["Existing"].ToString());
            SpecificationRevisionDataRow.Rev_EffectiveDate = Utility.StringToDate(ddlEffectiveFrom.SelectedValue.Trim());
            decimal diffSOH, diffUMFC;
            GetDifferentialPostingEntries(out diffSOH, out diffUMFC);
            SpecificationRevisionDataRow.Ret_BillingAmount = diffSOH; //decimal.Parse(dtSOH.Rows[2]["Revised"].ToString()); ;
            SpecificationRevisionDataRow.Ret_FCAmount = diffUMFC; //decimal.Parse(dtSOH.Rows[3]["Revised"].ToString()); ;

            SpecificationRevisionDataTable.Rows.Add(SpecificationRevisionDataRow);
            return SpecificationRevisionDataTable.FunPubFormXml();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    void GetDifferentialPostingEntries(out decimal differSOH, out decimal differUMFC)
    {
        differSOH = 0;
        differUMFC = 0;

        DataTable dtOldStructure = (DataTable)ViewState["OldRePaymentDetails"];
        DataTable dtNewStructure = ViewState["FinalRepaymentIRR"] as DataTable;

        Dictionary<string, string> dictParams = new Dictionary<string, string>();
        dictParams.Add("@PANUM", ddlMLA.SelectedValue);
        if (ddlSLA.SelectedIndex > 0 && ddlSLA.SelectedValue != "0")
            dictParams.Add("@SANUM", ddlSLA.SelectedValue);
        dictParams.Add("@Company_Id", CompanyId.ToString());
        string strLastIncomeDate;
        strLastIncomeDate = Utility.GetTableScalarValue("S3G_LOANAD_GetLastIncomeDate", dictParams);

        //if (strLastIncomeDate.Length == 0)
        //    strLastIncomeDate = txtDate.Text;

        DataRow[] dtrBilledRows = dtOldStructure.Select("BillStatus=1 and  InstallmentDate>=#" + Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text) + "# and InstallmentDate<=#" + Utility.StringToDate(txtDate.Text) + "#");
        DataRow[] dtrBilledNewRows = dtNewStructure.Select("InstallmentDate>=#" + Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text) + "# and InstallmentDate<=#" + Utility.StringToDate(txtDate.Text) + "#");

        if (strLastIncomeDate.Length > 0)
        {
            DataRow[] dtrIncomeGeneratedRows = dtNewStructure.Select("InstallmentDate>=#" + Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text) + "# and InstallmentDate<=#" + Utility.StringToDate(txtDate.Text) + "#");
            DataRow[] dtrIncomeGeneratedRowsNew = dtNewStructure.Select("InstallmentDate>=#" + Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text) + "# and InstallmentDate<=#" + Utility.StringToDate(strLastIncomeDate) + "#");


            foreach (DataRow dr in dtrIncomeGeneratedRows)
            {
                differUMFC += (dtNewStructure.Columns.Contains("Charges")) ? decimal.Parse(dr["Charges"].ToString()) : decimal.Parse(dr["Charge"].ToString());
                //decimal.Parse(dr["Charges"].ToString());
            }
            foreach (DataRow dr in dtrIncomeGeneratedRowsNew)
            {
                differUMFC -= (dtNewStructure.Columns.Contains("Charges")) ? decimal.Parse(dr["Charges"].ToString()) : decimal.Parse(dr["Charge"].ToString());
            }
        }
        foreach (DataRow dr in dtrBilledRows)
        {
            differSOH += decimal.Parse(dr["InstallmentAmount"].ToString());
        }
        foreach (DataRow dr in dtrBilledNewRows)
        {
            differSOH -= decimal.Parse(dr["InstallmentAmount"].ToString());
        }

    }

    #endregion

    #region Get repayment details
    /// <summary>
    /// To get the Account RepayDetails with From and To Installments
    /// </summary>
    /// <param name="strPANum"></param>
    /// <param name="strSANum"></param>
    /// <returns></returns>
    private DataTable FunGetRepayDetails(string strPANum, string strSANum)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@PANum", strPANum);
            Procparam.Add("@SANum", strSANum);
            Procparam.Add("@Company_ID", CompanyId);

            DataTable dtRepaymentTab = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAccountRepayDetails_ByPASA, Procparam);

            return dtRepaymentTab;
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
        }

        return new DataTable();
    }
    #endregion

    #region To calculate IRR
    private void CalculateIRRForRevisedRepaymentStructure(string RepayType)
    {

        DataTable dtRepaymentDetails = new DataTable();
        DataTable RepaymentDetailsForIRR = new DataTable();
      
        if (RepayType == "structureadhoc")
        {
            //dtRepaymentDetails = (DataTable)ViewState["OldRePaymentDetails"];
            dtRepaymentDetails = (DataTable)ViewState["DtRepayGrid"];

            ViewState["RepaymentDetailsForIRR"] = dtRepaymentDetails;
            RepaymentDetailsForIRR = dtRepaymentDetails;
        }
        else
            dtRepaymentDetails = (DataTable)ViewState["NewRepaymentDetails"];

        DataTable dtAccountSpecificDetails = (DataTable)ViewState["AccountSpecificDetails"];

        DataRow dtrASRow = dtAccountSpecificDetails.Rows[0];

        try
        {
            if (RepayType != "structureadhoc")
            {
                if(dtRepaymentDetails!=null)
                RepaymentDetailsForIRR = ObjBusinessLogic.FunPubGetRepaymentDetails(dtRepaymentDetails, 1, dtrASRow["TenureDescription"].ToString(), true, true, true);
                ViewState["RepaymentDetailsForIRR"] = RepaymentDetailsForIRR;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
        // Get the IRR Details for Company & LOB
        DataTable dtIRR = (DataTable)ViewState["IRR"];
        GetGlobalIRRDetails();
        DataTable dtRepaymentTab = (DataTable)ViewState["PaymentDetails"];
        DataTable dtExistingRevisiedDetails = (DataTable)ViewState["AccountRevisionDetails"];

        DataTable dtCashInflow = GetCashInflowDetails('I');
        DataTable dtCashOutflow = GetCashInflowDetails('O');
        DataTable dtAdditionalCashFlow = dtCashInflow.Clone();

        if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
        {

            //DataSet dsOutlfow = (DataSet)ViewState["OutflowDDL"];
            DataRow drOutflow = dtCashOutflow.NewRow();
            drOutflow["Date"] = Utility.StringToDate(txtDate.Text);
            drOutflow["CashOutFlow"] = "OL Lease Amount";
            drOutflow["EntityID"] = S3GCustomerAddress1.CustomerId;
            drOutflow["Entity"] = S3GCustomerAddress1.CustomerName;
            drOutflow["OutflowFromId"] = "144";
            drOutflow["OutflowFrom"] = "Customer";
            //DataTable dsAssetDetails = (DataTable)Session["PricingAssetDetails"];
            //decimal dcmTotalAssetValue = (decimal)(dsAssetDetails.Compute("Sum(Finance_Amount)", "Noof_Units > 0"));
            drOutflow["Amount"] = ViewState["CalculatedFinanceAmount"].ToString();
            drOutflow["CashOutFlowID"] = "-1";
            drOutflow["Accounting_IRR"] = true;
            drOutflow["Business_IRR"] = true;
            drOutflow["Company_IRR"] = true;
            drOutflow["CashFlow_Flag_ID"] = "41";
            dtCashOutflow.Rows.Add(drOutflow);
            if (dtCashOutflow.Rows.Count > 1)
                dtCashOutflow.Rows[0].Delete();
            dtCashOutflow.AcceptChanges();
        }




        if (dtCashOutflow == null || dtCashOutflow.Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this, LocalizationResources.Cashflownotfound);
            return;
        }
        else // Add the Revised Finance Amount if any thing  KR 20110622
        {
            //AddRevisedFinanceAmountToCashOutFlow(dtAdditionalCashFlow, dtCashOutflow);
            decimal CashFlowAmount = FindRevisedValues(0, false);
            if (CashFlowAmount > 0)
                AddRevisedFinanceAmountToCashOutFlow(dtCashOutflow, dtCashOutflow);
            else
                AddRevisedFinanceAmountToCashOutFlow(dtCashInflow, dtCashInflow);
        }

        // Get THE Revised Finance Amount Values for this particular account from Posting tabl e
        // Only the Negative ( Top Less) Amount's
        //S3G_LOANAD_GETREDUCEDFINAMOUNT
        Dictionary<string, string> dicParams = new Dictionary<string, string>();
        dicParams.Clear();
        dicParams.Add("@PANUM", ddlMLA.SelectedValue.ToString());
        if (ddlSLA.SelectedIndex > 0 && ddlSLA.SelectedValue != "0")
            dicParams.Add("@SANUM", ddlSLA.SelectedItem.Text);
        dicParams.Add("@COMPANY_ID", CompanyId);
        DataTable dtTopLessAmounts = Utility.GetDefaultData("S3G_LOANAD_GETREDUCEDFINAMOUNT", dicParams);
        if (dtTopLessAmounts != null && dtTopLessAmounts.Rows.Count > 0)
        {
            foreach (DataRow dtrOutFlow in dtTopLessAmounts.Rows)
            {
                AddRevisedFinanceAmountToCashOutFlow(dtAdditionalCashFlow, dtCashOutflow, decimal.Parse(dtrOutFlow["NewFinAmount"].ToString()), DateTime.Parse(dtrOutFlow["Revision_Date"].ToString()));
            }
        }

        double accountingIRR = 0, businessIRR = 0, companyIRR = 0;
        try
        {
            // Three Revised Values accordingly Rate,Tenure,FinAmount
            if (dtExistingRevisiedDetails.Rows.Count == 3)
            {
                RevisedFinanceAmount = decimal.Parse(ViewState["CalculatedFinanceAmount"].ToString()); //(dtExistingRevisiedDetails.Rows[0]["Revised"] != null && dtExistingRevisiedDetails.Rows[0]["Revised"].ToString().Length > 0) ? decimal.Parse(dtExistingRevisiedDetails.Rows[0]["Revised"].ToString()) : decimal.Parse(dtExistingRevisiedDetails.Rows[0]["Existing"].ToString());
                RevisedRate = (dtExistingRevisiedDetails.Rows[1]["Revised"] != null && dtExistingRevisiedDetails.Rows[1]["Revised"].ToString().Length > 0) ? decimal.Parse(dtExistingRevisiedDetails.Rows[1]["Revised"].ToString()) : decimal.Parse(dtExistingRevisiedDetails.Rows[1]["Existing"].ToString());
                RevisedTenure = int.Parse(ViewState["CalculatedTenure"].ToString()); //(dtExistingRevisiedDetails.Rows[2]["Revised"] != null && dtExistingRevisiedDetails.Rows[2]["Revised"].ToString().Length > 0) ? int.Parse(dtExistingRevisiedDetails.Rows[2]["Revised"].ToString()) : int.Parse(dtExistingRevisiedDetails.Rows[2]["Existing"].ToString());
            }
            else if (dtExistingRevisiedDetails.Rows.Count == 5)//Added for residual Amount
            {
                //RevisedFinanceAmount = decimal.Parse(ViewState["CalculatedFinanceAmount"].ToString()); //(dtExistingRevisiedDetails.Rows[0]["Revised"] != null && dtExistingRevisiedDetails.Rows[0]["Revised"].ToString().Length > 0) ? decimal.Parse(dtExistingRevisiedDetails.Rows[0]["Revised"].ToString()) : decimal.Parse(dtExistingRevisiedDetails.Rows[0]["Existing"].ToString());
                RevisedFinanceAmount = (dtExistingRevisiedDetails.Rows[0]["Revised"] != null && dtExistingRevisiedDetails.Rows[0]["Revised"].ToString().Length > 0 && decimal.Parse(dtExistingRevisiedDetails.Rows[0]["Revised"].ToString()) > 0) ? decimal.Parse(dtExistingRevisiedDetails.Rows[0]["Revised"].ToString()) : decimal.Parse(dtExistingRevisiedDetails.Rows[0]["Existing"].ToString());
                RevisedRate = (dtExistingRevisiedDetails.Rows[1]["Revised"] != null && dtExistingRevisiedDetails.Rows[1]["Revised"].ToString().Length > 0) ? decimal.Parse(dtExistingRevisiedDetails.Rows[1]["Revised"].ToString()) : decimal.Parse(dtExistingRevisiedDetails.Rows[1]["Existing"].ToString());
                RevisedTenure = int.Parse(ViewState["CalculatedTenure"].ToString()); //(dtExistingRevisiedDetails.Rows[2]["Revised"] != null && dtExistingRevisiedDetails.Rows[2]["Revised"].ToString().Length > 0) ? int.Parse(dtExistingRevisiedDetails.Rows[2]["Revised"].ToString()) : int.Parse(dtExistingRevisiedDetails.Rows[2]["Existing"].ToString());
                RevisedResidualAmount = (dtExistingRevisiedDetails.Rows[4]["Revised"] != null && dtExistingRevisiedDetails.Rows[4]["Revised"].ToString().Length > 0) ? decimal.Parse(dtExistingRevisiedDetails.Rows[4]["Revised"].ToString()) : decimal.Parse(dtExistingRevisiedDetails.Rows[4]["Existing"].ToString());

            }

            // Generate IRR for the new Repayment Details 


            string NewInstallmentStartDate = RepaymentDetailsForIRR.Rows[0]["FromDate"].ToString();
            // Generate IRR for the new Repayment Details 
            //DataTable RepaymentStructureTable1 = CalculateRepaymentDetails(out RepaymentDetailsForIRR, dtrASRow, RevisedTenure, RevisedFinanceAmount, RevisedRate, NewInstallmentStartDate.ToString(), int.Parse(GetGlobalIRRDetails["roundOff"].ToString()), dtCashInflow, dtCashOutflow);
            DataTable RepaymentStructureTable = ObjBusinessLogic.FunPubGenerateRepaymentStructure(RepaymentDetailsForIRR, dtCashInflow, dtCashOutflow, dtrASRow["Frequency_Value"].ToString().ToLower(), RevisedTenure,
              dtrASRow["TenureDescription"].ToString().ToLower(), DateFormate,
              RevisedFinanceAmount, RevisedRate, GetIRRInputValuesFromCase(3, dtrASRow["IRR_Rest"].ToString().ToLower()), "Empty", GetIRRInputValuesFromCase(4, dtrASRow["Frequency_Value"].ToString()), Convert.ToDecimal(0.10),
             Convert.ToDecimal(10.05), 0, Utility.StringToDate(txtACDate.Text), null, RevisedResidualAmount, rePayType, out accountingIRR, out businessIRR, out companyIRR, dtAdditionalCashFlow, ddlLOB.SelectedItem.Text);

            ViewState["FinalRepaymentIRR"] = RepaymentStructureTable;
            if (RepayType == "structureadhoc" && RepaymentStructureTable != null)
            {
                GrvSARevised.Visible = true;
                GrvSARevised.DataSource = RepaymentStructureTable;
                GrvSARevised.DataBind();
            }
            else
                GrvSARevised.Visible = false;

        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("cannot calculate irr"))
            {
                DataTable dtPrevIRR = (DataTable)ViewState["IRR"];
                dtPrevIRR.Rows[0]["Revised"] = dtPrevIRR.Rows[0]["Existing"];
                dtPrevIRR.Rows[1]["Revised"] = dtPrevIRR.Rows[1]["Existing"];
                dtPrevIRR.Rows[2]["Revised"] = dtPrevIRR.Rows[2]["Existing"];
                ViewState["IRR"] = dtPrevIRR;
                Utility.FunShowAlertMsg(this, ex.Message);
                btnSave.Enabled = false;
                return;
            }
            else if (ex.Message.ToLower().Contains("unrealistic"))
            {
                Utility.FunShowAlertMsg(this, ex.Message);
                btnSave.Enabled = false;
                return;
            }
            else if (ex.Message.ToLower().Contains("there is no row at position"))
            {
                Utility.FunShowAlertMsg(this, "No Repayment Structure Records Found");
                btnSave.Enabled = false;
                return;
            }
        }

        LoanRevisedIRRGrid(accountingIRR, businessIRR, companyIRR);

        // Calculated IRR
        DataTable dt = (DataTable)ViewState["IRR"];
        if (dt != null)
        {
            if ((string.IsNullOrEmpty(dt.Rows[2]["Revised"].ToString())))
            {
                dt.Rows[0]["Revised"] = businessIRR;
                dt.Rows[1]["Revised"] = companyIRR;
                dt.Rows[2]["Revised"] = accountingIRR;

            }
            if (dt != null && dt.Rows.Count > 0)
            {
                BindIRRGridView(dt);
                btnSave.Enabled = true;
            }


        }
        else
        {
            Utility.FunShowAlertMsg(this, "IRR Calculation Failed");
        }

    }

    private void ForceUpdatePanels()
    {
        tcSpecificRevision.ActiveTabIndex = 1;
        /*updPnlExisting.Update();
        updRepayment.Update();
        updButtons.Update();*/
    }

    private void AddRevisedFinanceAmountToCashOutFlow(DataTable dtAdditionalCashOutflow, DataTable dtOriginal)
    {
        decimal CashFlowAmount = FindRevisedValues(0, false);
        if (CashFlowAmount != 0)
        {
            DataRow OutFlowRow = dtAdditionalCashOutflow.NewRow();
            DataRow OutFlowRowClone = dtOriginal.Rows[0];
            OutFlowRow["AccountCashFlow_Details_ID"] = OutFlowRowClone["AccountCashFlow_Details_ID"];
            OutFlowRow["Company_ID"] = CompanyId.ToString();
            OutFlowRow["PANum"] = OutFlowRowClone["PANum"].ToString();
            OutFlowRow["SANum"] = OutFlowRowClone["SANum"].ToString();
            OutFlowRow["COMPONENT_CODE"] = OutFlowRowClone["COMPONENT_CODE"].ToString();
            if (CashFlowAmount > 0)
                OutFlowRow["CASHFLOW_TYPE"] = 55; // OutFlowRowClone["CASHFLOW_TYPE"].ToString();
            else
                OutFlowRow["CASHFLOW_TYPE"] = 53; //OutFlowRowClone["CASHFLOW_TYPE"].ToString();
            OutFlowRow["CASHFLOW_ENTITY_TYPE"] = OutFlowRowClone["CASHFLOW_ENTITY_TYPE"].ToString();
            OutFlowRow["CASHFLOW_ENTITY_CODE"] = OutFlowRowClone["CASHFLOW_ENTITY_CODE"].ToString();
            OutFlowRow["CASHFLOW_DATE"] = Utility.StringToDate(txtDate.Text); //OutFlowRowClone["CASHFLOW_DATE"].ToString();
            OutFlowRow["CASHFLOW_AMOUNT"] = (CashFlowAmount < 0) ? 0 - CashFlowAmount : CashFlowAmount; //CashFlowAmount; // FindRevisedValues(0, false);
            OutFlowRow["CashFlow_ID"] = OutFlowRowClone["CashFlow_ID"].ToString();
            OutFlowRow["LOB_ID"] = OutFlowRowClone["LOB_ID"].ToString();
            OutFlowRow["Company_ID"] = OutFlowRowClone["Company_ID"].ToString();
            OutFlowRow["CFSl_No"] = OutFlowRowClone["CFSl_No"].ToString();
            OutFlowRow["CashFlow_Description"] = OutFlowRowClone["CashFlow_Description"].ToString();
            OutFlowRow["Flow_Type"] = OutFlowRowClone["Flow_Type"].ToString();
            OutFlowRow["CashFlow_Flag_ID"] = 41; // Hard coded 41 for Finanace Amount //OutFlowRowClone["CashFlow_Flag_ID"].ToString();
            OutFlowRow["Business_IRR"] = true; ;
            OutFlowRow["Accounting_IRR"] = true;
            OutFlowRow["Is_Active"] = OutFlowRowClone["Is_Active"].ToString();
            OutFlowRow["Created_By"] = OutFlowRowClone["Created_By"].ToString();
            OutFlowRow["Created_On"] = OutFlowRowClone["Created_On"].ToString();

            if (!string.IsNullOrEmpty(OutFlowRowClone["Modified_On"].ToString()))
            {
                OutFlowRow["Modified_On"] = OutFlowRowClone["Modified_On"].ToString();
            }

            OutFlowRow["Company_IRR"] = true;
            OutFlowRow["Amount"] = (CashFlowAmount < 0) ? 0 - CashFlowAmount : CashFlowAmount; // FindRevisedValues(0, false);
            OutFlowRow["Date"] = Utility.StringToDate(txtDate.Text);
            dtAdditionalCashOutflow.Rows.Add(OutFlowRow);
            dtAdditionalCashOutflow.AcceptChanges();
        }
    }

    private void AddRevisedFinanceAmountToCashOutFlow(DataTable dtAdditionalCashOutflow, DataTable dtOriginal, decimal CashFlowAmount, DateTime RevisionDate)
    {
        if (CashFlowAmount != 0)
        {
            DataRow OutFlowRow = dtAdditionalCashOutflow.NewRow();
            DataRow OutFlowRowClone = dtOriginal.Rows[0];
            OutFlowRow["AccountCashFlow_Details_ID"] = OutFlowRowClone["AccountCashFlow_Details_ID"];
            OutFlowRow["Company_ID"] = CompanyId.ToString();
            OutFlowRow["PANum"] = OutFlowRowClone["PANum"].ToString();
            OutFlowRow["SANum"] = OutFlowRowClone["SANum"].ToString();
            OutFlowRow["COMPONENT_CODE"] = OutFlowRowClone["COMPONENT_CODE"].ToString();
            OutFlowRow["CASHFLOW_TYPE"] = OutFlowRowClone["CASHFLOW_TYPE"].ToString();
            OutFlowRow["CASHFLOW_ENTITY_TYPE"] = OutFlowRowClone["CASHFLOW_ENTITY_TYPE"].ToString();
            OutFlowRow["CASHFLOW_ENTITY_CODE"] = OutFlowRowClone["CASHFLOW_ENTITY_CODE"].ToString();
            OutFlowRow["CASHFLOW_DATE"] = OutFlowRowClone["CASHFLOW_DATE"].ToString();
            OutFlowRow["CASHFLOW_AMOUNT"] = CashFlowAmount; // FindRevisedValues(0, false);
            OutFlowRow["CashFlow_ID"] = OutFlowRowClone["CashFlow_ID"].ToString();
            OutFlowRow["LOB_ID"] = OutFlowRowClone["LOB_ID"].ToString();
            OutFlowRow["Company_ID"] = OutFlowRowClone["Company_ID"].ToString();
            OutFlowRow["CFSl_No"] = OutFlowRowClone["CFSl_No"].ToString();
            OutFlowRow["CashFlow_Description"] = OutFlowRowClone["CashFlow_Description"].ToString();
            OutFlowRow["Flow_Type"] = OutFlowRowClone["Flow_Type"].ToString();
            OutFlowRow["CashFlow_Flag_ID"] = 41; // Hard coded 41 for Finanace Amount //OutFlowRowClone["CashFlow_Flag_ID"].ToString();
            OutFlowRow["Business_IRR"] = true; ;
            OutFlowRow["Accounting_IRR"] = true;
            OutFlowRow["Is_Active"] = OutFlowRowClone["Is_Active"].ToString();
            OutFlowRow["Created_By"] = OutFlowRowClone["Created_By"].ToString();
            OutFlowRow["Created_On"] = OutFlowRowClone["Created_On"].ToString();
            OutFlowRow["Modified_On"] = OutFlowRowClone["Modified_On"].ToString();
            OutFlowRow["Company_IRR"] = true;
            OutFlowRow["Amount"] = CashFlowAmount; // FindRevisedValues(0, false);
            OutFlowRow["Date"] = RevisionDate;
            dtAdditionalCashOutflow.Rows.Add(OutFlowRow);
            dtAdditionalCashOutflow.AcceptChanges();
        }
    }


    private DataRow GetGlobalIRRDetails()
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@Lob_ID", ddlLOB.SelectedValue);
        DataTable dtGlobalIRRDtls = Utility.GetDefaultData(SPNames.S3G_ORG_GetGlobalIRRDetails, Procparam);
        return dtGlobalIRRDtls.Rows[0];
    }

    private DataTable GetCashInflowDetails(char FlowType)
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", CompanyId);
        string sla = string.Empty;
        if (ddlSLA.SelectedValue.Equals("0") || ddlSLA.SelectedIndex == -1)
            sla = ddlMLA.SelectedValue.ToString() + "DUMMY";
        else
            sla = ddlSLA.SelectedItem.Text;
        Procparam.Add("@SANum", sla);
        Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
        if (FlowType == 'I')
            Procparam.Add("@Flow_Type", "inflow");
        else if (FlowType == 'O')
            Procparam.Add("@Flow_Type", "outflow");
        if (ddlLOB.SelectedItem.Text.ToUpper().StartsWith("OL"))
            Procparam.Add("@Lob_Code", "OL");
        DataTable dtCashInflow = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetCashFlowDetails, Procparam);
        return dtCashInflow;
    }
    public decimal RevisedFinanceAmount { get; set; }
    public decimal RevisedRate { get; set; }
    public int RevisedTenure { get; set; }

    public decimal RevisedResidualValue { get; set; }
    public decimal RevisedResidualAmount { get; set; }

    #endregion

    #region Load Account specific details
    // Get the Account Specific Details 
    private DataTable LoadAccountSpecificDetails()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            
            if (ddlSLA.SelectedIndex > 0)
                Procparam.Add("@SANum", ddlSLA.SelectedItem.Text);
            else
                Procparam.Add("@SANum", ddlMLA.SelectedValue.ToString() + "DUMMY");

            Procparam.Add("@Company_Id", CompanyId);

            DataSet dtAccountSpecificDetails = Utility.GetDataset(SPNames.S3G_LOANAD_GetAccountSpecificDetails, Procparam);
            if (dtAccountSpecificDetails != null && dtAccountSpecificDetails.Tables.Count == 3)
            {
                ViewState["AccountSpecificDetails"] = dtAccountSpecificDetails.Tables[0];
                if (dtAccountSpecificDetails.Tables[0] != null && dtAccountSpecificDetails.Tables[0].Rows.Count > 0)
                {
                    if (dtAccountSpecificDetails.Tables[0].Rows[0]["Residual_Value"] != DBNull.Value)
                    {
                        ViewState["ResidualValue"] = dtAccountSpecificDetails.Tables[0].Rows[0]["Residual_Value"].ToString();
                    }
                }

                ViewState["GlobalRoundOff"] = dtAccountSpecificDetails.Tables[1].Rows[0]["RoundOff"].ToString();
            }
            Utility.FillDLL(ddlEffectiveFrom, dtAccountSpecificDetails.Tables[2]);
            return dtAccountSpecificDetails.Tables[0];
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    #endregion

    #endregion

    public struct RevisionValues
    {
        public decimal RevisedFinanceAmount { get; set; }
        public decimal RevisedRate { get; set; }
        public int RevisedTenure { get; set; }
        public decimal RevisedResidualValue { get; set; }
        public decimal RevisedResidualAmount { get; set; }
    }

    protected void grvBill2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["NewRepaymentDetails"] != null)
        {
            DataTable myDataTable = (DataTable)ViewState["NewRepaymentDetails"];
            grvBill2.DataSource = myDataTable;

            grvBill2.PageIndex = e.NewPageIndex;
            grvBill2.DataBind();
        }
    }

    protected void grvBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (ViewState["OldRePaymentDetails"] != null)
        {
            DataTable myDataTable = (DataTable)ViewState["OldRePaymentDetails"];
            grvBill.DataSource = myDataTable;

            grvBill.PageIndex = e.NewPageIndex;
            grvBill.DataBind();
        }
    }


    protected void grvAccountRevisionDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtRevisedValue = e.Row.FindControl("txtRevisedValue") as TextBox;
            Label lblExistingdetails = e.Row.FindControl("lblExistingdetails") as Label;

            if (grvAccountRevisionDetails.Rows.Count == 0)  // Finance Amount Text Box 
            {
                txtRevisedValue.Width = 100;
                Literal description = e.Row.FindControl("litDescription") as Literal;
                description.Text = "(+/-)";
                txtRevisedValue.Attributes.Add("onkeypress", "fnAllowNumbersAndPlusMin(true,true,this)");
                //txtRevisedValue.SetDecimalPrefixSuffix(12, 0, true, "Finance Amount");
                txtRevisedValue.MaxLength = 16;
                if (txtRevisedValue.Text.Trim().Equals("0"))
                    txtRevisedValue.Text = "";
            }
            else if (grvAccountRevisionDetails.Rows.Count == 1)  // Rate Of Interest Text Box 
            {
                txtRevisedValue.Width = 75;
                txtRevisedValue.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this)");
                txtRevisedValue.SetDecimalPrefixSuffix(18, 4, true, "Rate "); // Modified By Rao 19 July...
                txtRevisedValue.MaxLength = 7;
            }
            else if (grvAccountRevisionDetails.Rows.Count == 2)  // Rate Of Interest Text Box 
            {
                txtRevisedValue.Width = 40;
                txtRevisedValue.Attributes.Add("onkeypress", "fnAllowNumbersOnly(false,false,this)");
                txtRevisedValue.SetDecimalPrefixSuffix(3, 0, true, "Tenure");
                txtRevisedValue.MaxLength = 3;
            }
            else if (grvAccountRevisionDetails.Rows.Count == 3)  //Residual value
            {
                txtRevisedValue.Width = 75;
                txtRevisedValue.Attributes.Add("onkeypress", "fnAllowNumbersOnly(false,false,this)");
                txtRevisedValue.SetDecimalPrefixSuffix(6, 4, true, "Residual value");
                txtRevisedValue.MaxLength = 10;
                txtRevisedValue.Attributes.Add("onchange", "funCalcResidualAmount(this)");
                if (lblExistingdetails != null)
                {
                    if (!string.IsNullOrEmpty(lblExistingdetails.Text))
                    {
                        if (decimal.Parse(lblExistingdetails.Text) == 0)
                        {
                            txtRevisedValue.Attributes.Add("ContentEditable", "false");
                        }
                    }
                }
            }
            else if (grvAccountRevisionDetails.Rows.Count == 4)  // Residual Amount
            {
                txtRevisedValue.Width = 75;
                txtRevisedValue.Attributes.Add("onkeypress", "fnAllowNumbersOnly(false,false,this)");
                txtRevisedValue.SetDecimalPrefixSuffix(18, 4, true, "Residual Amount");
                txtRevisedValue.MaxLength = 15;

                if (lblExistingdetails != null)
                {
                    if (!string.IsNullOrEmpty(lblExistingdetails.Text))
                    {
                        if (decimal.Parse(lblExistingdetails.Text) == 0)
                        {
                            txtRevisedValue.Attributes.Add("ContentEditable", "false");
                        }
                    }
                }

            }
            if (PageMode == PageModes.Query)
                txtRevisedValue.Enabled = false;

            if ((e.Row.RowType == DataControlRowType.DataRow && PageMode == PageModes.Create) || (e.Row.RowType == DataControlRowType.DataRow && PageMode == PageModes.Modify))
            {
                if (ddlLOB.SelectedItem.Text.Split(new char[] { '-' }).GetValue(0).ToString().ToLower().Trim() == "tl" || ddlLOB.SelectedItem.Text.Split(new char[] { '-' }).GetValue(0).ToString().ToLower().Trim() == "te")
                {
                    if (ViewState["ROIRule"] != null)
                    {
                        if (ViewState["ROIRule"].ToString().StartsWith("RRB"))
                        {
                            if (grvAccountRevisionDetails.Rows.Count == 0 || grvAccountRevisionDetails.Rows.Count == 1)
                            {
                                txtRevisedValue = e.Row.FindControl("txtRevisedValue") as TextBox;
                                txtRevisedValue.Attributes.Add("ContentEditable", "false");
                            }
                        }
                    }
                }
                else if (ddlLOB.SelectedItem.Text.Split(new char[] { '-' }).GetValue(0).ToString().ToLower().Trim() == "ol")
                {
                    if (grvAccountRevisionDetails.Rows.Count == 0)
                    {
                        txtRevisedValue = e.Row.FindControl("txtRevisedValue") as TextBox;
                        txtRevisedValue.Attributes.Add("ContentEditable", "false");
                    }
                }

            }

        }

    }
    protected void txtRevisedValue_TextChanged(object sender, EventArgs e)
    {

    }
    protected void GenerateSpecificRevisionDetails(object sender, EventArgs e)
    {
        GenerateSpecificRevision("Add");
    }

    private void GenerateSpecificRevision(string Mode)
    {
        try
        {
            if (Page.IsValid)
            {
                // check whether Effective from is selected
                //if (txtEffectiveFrom.Text.Trim().Length > 0) // effective date should not be empty.
                if (ddlEffectiveFrom.SelectedIndex > 0) // effective date should not be empty.
                {
                    if (Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()) >= Utility.StringToDate(ViewState["AccountCreationDate"].ToString()))  // To check whether the revision date is greater than account creation date
                    {
                        // Check whether Effective date falls on Open month

                        // STEP 1

                        if (CalculateRevisedRepayment())
                        {
                            if (ViewState["StructureAdhoc"] != null && ViewState["StructureAdhoc"].ToString() == "yes") // not structure adhoc
                            {
                                DataTable dtRepaymentDetails = new DataTable();
                                if (Mode == "Edit")
                                    dtRepaymentDetails = ViewState["DtRepayGrid"] as DataTable;
                                else
                                {
                                    dtRepaymentDetails = null;
                                    ViewState["DtRepayGrid"] = null;
                                }
                                FunPriFillRepaymentDLL(Mode);
                                if (dtRepaymentDetails != null && dtRepaymentDetails.Rows.Count > 0)
                                {
                                    CalculateIRRForRevisedRepaymentStructure("structureadhoc");
                                }
                                // UPDATE STOCK ON HIRE
                                UpdateSOH();
                                tcSpecificRevision.ActiveTabIndex = (tcSpecificRevision.ActiveTabIndex == 2) ? 1 : 2;
                            }
                            else
                            {

                                // STEP 2
                                CalculateIRRForRevisedRepaymentStructure(rePayType.ToString());
                                // UPDATE STOCK ON HIRE
                                UpdateSOH();
                            }
                        }

                        tcSpecificRevision.Tabs[2].Enabled = true;
                    }
                    else
                        Utility.FunShowAlertMsg(this, "Specific revision date should be greater than or equal to account creation date");
                }
                else
                {
                    //txtEffectiveFrom.Focus();
                    ddlEffectiveFrom.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
        }
    }

    private void UpdateSOH()
    {
        // Specific Revision Success for the selected account enable the tab index two to view the revised details            
        ForceUpdatePanels();

        // Update the SOH Grid
        DataTable dtSOH = ViewState["SOH"] as DataTable;

        string sRevisedUMFC;
        if (Convert.ToString(ViewState["RevisedUMFC"]) != "")
            sRevisedUMFC = Convert.ToString(ViewState["RevisedUMFC"]);
        else
            sRevisedUMFC = "0";

        if (dtSOH.Rows.Count == 3)
        {
            dtSOH.Rows[0]["Revised"] = Math.Round(decimal.Parse(ViewState["CalculatedFinanceAmount"].ToString())) + Math.Round(decimal.Parse(sRevisedUMFC));
            dtSOH.Rows[1]["Revised"] = Math.Round(decimal.Parse(sRevisedUMFC));
            //dtSOH.Rows[2]["Revised"] = 0;
            // Re Calculate the Revised Finance Amount and Update                
            dtSOH.Rows[2]["Revised"] = Math.Round(FindRevisedValues(0, false));

            dtSOH.AcceptChanges();
            BindSOHGridView(dtSOH);
        }
    }

    void clearViewStateItems()
    {
        ViewState.Clear();
    }
    protected void CVSLA_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (ddlSLA.Items.Count > 1 && ddlSLA.SelectedValue.Equals("0"))
        {
            ddlSLA.Focus();
            args.IsValid = false;
        }
    }
    protected void OkButton_Click(object sender, EventArgs e)
    {
        MPopUp.Hide();

        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Revision_Number", PageIdValue);
        Procparam.Add("@Remarks", txtCancelReason.Text.Trim());
        Procparam.Add("@Company_Id", CompanyId);

        Utility.GetTableScalarValue("S3G_LOANAD_CancelSpecificRevision", Procparam);
        Utility.FunShowAlertMsg(this, "Specific Revision canceled successfully", RedirectOnCancel);
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        MPopUp.Hide();
    }
    protected void tcSpecificRevision_ActiveTabChanged(object sender, EventArgs e)
    {
        switch (tcSpecificRevision.ActiveTabIndex)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    #region " Structure Adhoc"
    string strPageName = "Specific Revision";
    /* string FrequencyType = "4";
     string Tenure = "12";
     string TenureType = "Months";
     decimal FinanceAmount=150000;
     decimal Marginmoney=2;
     string returnPattern="1";
     decimal Rate=7;*/
    public int FrequencyType { get; set; }
    public int Tenure { get; set; }
    public string TenureType { get; set; }
    public decimal FinanceAmount { get; set; }
    public decimal MarginMoney { get; set; }
    public int ReturnPattern { get; set; }
    public double Rate { get; set; }

    private void FunPriFillInflowDLL(string Mode)
    {
        try
        {

            ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", CompanyId.ToString());
            if (ddlLOB.SelectedValue != "0")
            {
                objParameters.Add("@LobId", ddlLOB.SelectedValue);
            }
            DataSet dsInflow = Utility.GetDataset("s3g_org_loadInflowLov", objParameters);
            ViewState["InflowDDL"] = dsInflow;


        }
        catch (Exception ex)
        {
            ObjCustomerService.Close();
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            if (ObjCustomerService != null)
                ObjCustomerService.Close();
        }
    }
    private void FunPriGenerateNewRepayment()
    {
        try
        {
            DropDownList ddlRepaymentCashFlow_RepayTab = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            Dictionary<string, string> dicParam = new Dictionary<string, string>();
            dicParam.Clear();
            dicParam.Add("@company_Id", CompanyId);
            dicParam.Add("@lob_Id", ddlLOB.SelectedValue);
            ddlRepaymentCashFlow_RepayTab.BindDataTable("S3G_LOANAD_GetOnlyInstallments", dicParam, new string[] { "CashFlow_ID", "CashFlow_Description" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Cashflow Description in Repayment");
        }
    }
    private void FunPriInsertRepayment()
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];

            DataTable dtAccountSpecificDetails = (DataTable)ViewState["AccountSpecificDetails"];

            FrequencyType = int.Parse(dtAccountSpecificDetails.Rows[0]["Frequency_Value"].ToString());
            Tenure = Convert.ToInt32(FindRevisedValues(2, true));// int.Parse(dtAccountSpecificDetails.Rows[0]["Tenure"].ToString());
            TenureType = dtAccountSpecificDetails.Rows[0]["TenureDescription"].ToString();
            FinanceAmount = (ViewState["CalculatedFinanceAmount"] != null) ? decimal.Parse(ViewState["CalculatedFinanceAmount"].ToString()) : decimal.Parse(FindRevisedValues(0, true).ToString()); // decimal.Parse(dtAccountSpecificDetails.Rows[0]["Finance_Amount"].ToString());
            //MarginMoney = decimal.Parse(dtAccountSpecificDetails.Tables[0].Rows[0]["Finance_Amount"].ToString()) * double.Parse(dtAccountSpecificDetails.Tables[0].Rows[0]["Margin_Percentage"].ToString());
            ReturnPattern = int.Parse(dtAccountSpecificDetails.Rows[0]["Return_Pattern"].ToString());
            Rate = Convert.ToDouble(FindRevisedValues(1, true));  // double.Parse(dtAccountSpecificDetails.Rows[0]["Rate"].ToString());                


            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            DateTime dtNextFromdate;
            DropDownList ddlRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            TextBox txtAmountRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtAmountRepaymentCashFlow_RepayTab") as TextBox;
            TextBox txtPerInstallmentAmount_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            TextBox txtBreakup_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;
            TextBox txtFromInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            TextBox txtToInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToInstallment_RepayTab") as TextBox;
            TextBox txtfromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            TextBox txtToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToDate_RepayTab") as TextBox;
            string[] strIds = ddlRepaymentCashFlow_RepayTab1.SelectedValue.ToString().Split(',');
            if (DtRepayGrid.Rows.Count > 0)
            {
                objRepaymentStructure.FunPubGetNextRepaydate(DtRepayGrid, FrequencyType.ToString()); //KR
                if (txtfromdate_RepayTab1.Text != "")
                {
                    if (Utility.StringToDate(txtfromdate_RepayTab1.Text) < objRepaymentStructure.dtNextDate)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlapped');", true);
                        return;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "From date cannot be empty");
                    return;
                }
            }
            else
            {
                if (txtfromdate_RepayTab1.Text == "")
                {
                    Utility.FunShowAlertMsg(this, "From date cannot be empty");
                    return;
                }
            }
            Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
            objMethodParameters.Add("CashFlow", ddlRepaymentCashFlow_RepayTab1.SelectedItem.Text);
            objMethodParameters.Add("CashFlowId", ddlRepaymentCashFlow_RepayTab1.SelectedValue);
            objMethodParameters.Add("PerInstall", txtPerInstallmentAmount_RepayTab1.Text);
            objMethodParameters.Add("Breakup", txtBreakup_RepayTab1.Text);
            objMethodParameters.Add("FromInstall", txtFromInstallment_RepayTab1.Text);
            objMethodParameters.Add("ToInstall", txtToInstallment_RepayTab1.Text);
            objMethodParameters.Add("FromDate", txtfromdate_RepayTab1.Text);
            objMethodParameters.Add("Frequency", FrequencyType.ToString()); //KR
            objMethodParameters.Add("TenureType", TenureType); //KR
            objMethodParameters.Add("Tenure", Tenure.ToString()); //KR
            objMethodParameters.Add("DocumentDate", txtDate.Text);
            string strErrorMessage = "";
            DataTable TemproaryRepayTable = DtRepayGrid.Copy();

            objRepaymentStructure.FunPubAddRepayment(out dtNextFromdate, out strErrorMessage, out TemproaryRepayTable, TemproaryRepayTable, objMethodParameters);
            if (strErrorMessage != "")
            {
                Utility.FunShowAlertMsg(this, strErrorMessage);
                return;
            }
            if (strIds[4] == "23")
            {
                decimal decIRRActualAmount = 0;
                decimal decTotalAmount = 0; // KR BELOW

                decimal DecRoundOff;
                if (ViewState["GlobalRoundOff"] != null)
                    DecRoundOff = Convert.ToDecimal(ViewState["GlobalRoundOff"]);
                else
                    DecRoundOff = 2;

                if (!objRepaymentStructure.FunPubValidateTotalAmount(TemproaryRepayTable, FinanceAmount.ToString(), MarginMoney.ToString(), ReturnPattern.ToString(), Rate.ToString(), TenureType, Tenure.ToString(), out decIRRActualAmount, out decTotalAmount, "1", DecRoundOff))
                {
                    Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                    return;
                }

            }


            gvRepaymentDetails.DataSource = TemproaryRepayTable;
            gvRepaymentDetails.DataBind();

            TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
            txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
            TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);

            // Clear and copy the new rows to original datatable
            DtRepayGrid.Clear();
            DtRepayGrid = TemproaryRepayTable.Copy();

            ViewState["DtRepayGrid"] = DtRepayGrid;
            FunPriGenerateNewRepayment();
            FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private decimal FunPriGetAmountFinanced()
    {
        try
        {
            decimal decFinanaceAmt;
            decFinanaceAmt = Convert.ToDecimal(FinanceAmount.ToString());// -FunPriGetMarginAmout();
            return Math.Round(decFinanaceAmt, 0);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error in getting finance amount");
        }
    }
    private bool FunPriValidateTotalAmount(out decimal decActualAmount, out decimal decTotalAmount, string strOption)
    {
        try
        {
            if (strOption != "3")
            { // KR
                decTotalAmount = FunPriGetAmountFinanced() + Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(TenureType, FunPriGetAmountFinanced(), Convert.ToDecimal(Rate), Convert.ToInt32(Tenure)), 0);
            }
            else
            {
                decTotalAmount = FunPriGetAmountFinanced();
            }
            decActualAmount = 0;
            if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count <= 0)
            {
                //cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + " Add atleast one row Repayment details";
                //cvApplicationProcessing.IsValid = false;
                return false;
            }
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            foreach (DataRow drRepyrow in DtRepayGrid.Rows)
            {
                decActualAmount += (Convert.ToDecimal(drRepyrow["TotalPeriodInstall"].ToString()));
            }
            if (strOption == "1")
            {
                if (decActualAmount > decTotalAmount)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (strOption == "2")
            {
                if (decActualAmount == decTotalAmount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (strOption == "3")
            {
                if (decActualAmount >= decTotalAmount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Total Amount");
        }

    }

    private bool FunPriValidateTenurePeriod(DateTime dtStartDate, DateTime dtEndDate)
    {
        DateTime dateInterval = new DateTime();
        bool blnIsvalid = true;
        try
        {
            switch (TenureType)
            {
                case "months":
                    dateInterval = dtStartDate.AddMonths(Convert.ToInt32(Tenure));
                    break;
                case "weeks":

                    int intAddweeks = Convert.ToInt32(Tenure) * 7;
                    dateInterval = dtStartDate.AddDays(intAddweeks);
                    break;
                case "days":
                    dateInterval = dtStartDate.AddDays(Convert.ToInt32(Tenure));
                    break;
            }
            if (dtEndDate > dateInterval)
            {
                blnIsvalid = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Tenure Period with Tenure Type");
        }
        return blnIsvalid;
    }

    private bool FunPriValidateTenurePeriod(int intActualTenurePeriod)
    {
        bool blnIsValid = false;
        try
        {
            if (intActualTenurePeriod == Convert.ToInt32(Tenure))
            {
                blnIsValid = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Tenure with Tenure Type");
        }
        return blnIsValid;
    }

    private void FunPriCalculateSummary(DataTable objDataTable, string strGroupByField, string strSumField)
    {
        try
        {
            DataTable dtSummaryDetails = Utility.FunPriCalculateSumAmount(objDataTable, strGroupByField, strSumField);
            //gvRepaymentSummary.DataSource = dtSummaryDetails;
            //gvRepaymentSummary.DataBind();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Calculate Repayment Summary");
        }

    }


    DataTable DtRepayGrid = new DataTable();
    private void FunPriRemoveRepayment(GridViewDeleteEventArgs e)
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            if (DtRepayGrid.Rows.Count > 0)
            {
                DtRepayGrid.Rows.RemoveAt(e.RowIndex);

                DataTable dtAccountSpecificDetails = (DataTable)ViewState["AccountSpecificDetails"];

                FrequencyType = int.Parse(dtAccountSpecificDetails.Rows[0]["Frequency_Value"].ToString());
                Tenure = int.Parse(dtAccountSpecificDetails.Rows[0]["Tenure"].ToString());
                TenureType = dtAccountSpecificDetails.Rows[0]["TenureDescription"].ToString();
                FinanceAmount = decimal.Parse(dtAccountSpecificDetails.Rows[0]["Finance_Amount"].ToString());
                //MarginMoney = decimal.Parse(dtAccountSpecificDetails.Tables[0].Rows[0]["Finance_Amount"].ToString()) * double.Parse(dtAccountSpecificDetails.Tables[0].Rows[0]["Margin_Percentage"].ToString());
                ReturnPattern = int.Parse(dtAccountSpecificDetails.Rows[0]["Return_Pattern"].ToString());
                Rate = double.Parse(dtAccountSpecificDetails.Rows[0]["Rate"].ToString());

                if (DtRepayGrid.Rows.Count == 0)
                {
                    //DataTable dtDummy = (DataTable)ViewState["dummyRow"];
                    //gvRepaymentDetails.DataSource = dtDummy;
                    //gvRepaymentDetails.DataBind();
                    FunPriFillRepaymentDLL("Add");
                }
                else
                {

                    gvRepaymentDetails.DataSource = DtRepayGrid;
                    gvRepaymentDetails.DataBind();
                    FunPriGenerateNewRepayment();
                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
                    TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
                    Label lblTODate_ReapyTab_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblTODate_ReapyTab");
                    DateTime dtTodate = Utility.StringToDate(lblTODate_ReapyTab_Upd.Text);
                    DateTime dtNextFromdate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(FrequencyType.ToString(), dtTodate);
                    txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);
                    FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");

                    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                }
                /*if (ddl_Repayment_Mode.SelectedValue != "2")
                {
                    Label lblCashFlowId = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblCashFlow_Flag_ID");
                    if (lblCashFlowId.Text != "23")
                    {
                        ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                    }
                }
                else
                {
                    ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;
                }*/
                //}
            }
        }
        catch (Exception ex)
        {
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Remove Repayment");
        }
    }


    int intSlNo = 0;
    private void FunPriBindRepaymentDetails(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                intSlNo += 1;
                e.Row.Cells[0].Text = intSlNo.ToString();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Repayment Details");
        }
    }
    protected void gvRepaymentDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriAssignRepaymentDateFormat(e);
        }
        catch (Exception ex)
        {
            //cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            //cvApplicationProcessing.IsValid = false;
        }
    }
    protected void gvRepaymentDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveRepayment(e);
        }
        catch (Exception ex)
        {
            //cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            //cvApplicationProcessing.IsValid = false;
        }
    }
    protected void gvRepaymentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindRepaymentDetails(e);
        }
        catch (Exception ex)
        {
            //cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + ex.Message;
            //cvApplicationProcessing.IsValid = false;
        }
    }
    private void FunPriAssignRepaymentDateFormat(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtToDate_RepayTab = e.Row.FindControl("txtToDate_RepayTab") as TextBox;
                txtToDate_RepayTab.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab = e.Row.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_ToDate_RepayTab.Format = DateFormate;

                TextBox txtfromdate_RepayTab = e.Row.FindControl("txtfromdate_RepayTab") as TextBox;
                txtfromdate_RepayTab.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab = e.Row.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_fromdate_RepayTab.Format = DateFormate;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender calext_FromDate = e.Row.FindControl("calext_FromDate") as AjaxControlToolkit.CalendarExtender;
                calext_FromDate.Format = DateFormate;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Assign Date Format in Repayment Details");
        }
    }
    protected void txRepaymentFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtBoxFromdate = (TextBox)sender;
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            if (objRepaymentStructure.FunPubGetCashFlowDetails(int.Parse(CompanyId), Convert.ToInt32(ddlLOB.SelectedValue)).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Define Installment Flag in Cashflow Master for selected Line of Business");
                return;
            }
            //FunPriIRRReset();
            //strDocumentDate = txtBoxFromdate.Text;
            //FunPriGenerateRepaymentSchedule(objRepaymentStructure);
        }
        catch (Exception ex)
        {
            //cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem,Unable to Generate Repayment Schedule";
            //cvApplicationProcessing.IsValid = false;
        }
    }
    protected void ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlCashFlowDesc = sender as DropDownList;
            //FunPriDoCashflowBasedValidation(ddlCashFlowDesc);
        }
        catch (Exception ex)
        {
            //cvApplicationProcessing.ErrorMessage = "Error in fetching values based on cash flow details";
            //cvApplicationProcessing.IsValid = false;
        }

    }
    protected void btnAddRepayment_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriInsertRepayment();
        }
        catch (Exception ex)
        {
            //cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem, Unable to Add Repayment";
            //cvApplicationProcessing.IsValid = false;
        }
    }
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;
    private void FunPriFillRepaymentDLL(string Mode)
    {

        //try
        //{
        /*
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        FunPriFillInflowDLL("s");
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        if ((DataTable)ViewState["DtRepayGrid"] != null)
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            DataTable dummyTable = (DataTable)ViewState["dummyRow"];
            if (DtRepayGrid != null && DtRepayGrid.Rows.Count > 0)
            {
                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();
            }
            else
            {
                gvRepaymentDetails.DataSource = dummyTable;
                gvRepaymentDetails.DataBind();
                gvRepaymentDetails.Rows[0].Cells.Clear();
                gvRepaymentDetails.Rows[0].Visible = false;
            }               
        }
        else
        {
            gvRepaymentDetails.DataSource = null;
            gvRepaymentDetails.DataBind();
            ObjStatus.Option = 52;
            DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            gvRepaymentDetails.DataSource = DtRepayGrid;
            gvRepaymentDetails.DataBind();
            if(DtRepayGrid.Rows.Count==1)
                ViewState["dummyRow"] = DtRepayGrid.Copy();

            DtRepayGrid.Rows.Clear();               
            ViewState["DtRepayGrid"] = DtRepayGrid;
            gvRepaymentDetails.Rows[0].Cells.Clear();
            gvRepaymentDetails.Rows[0].Visible = false;
        }

        FunPriGenerateNewRepayment();

    }
    catch (Exception ex)
    {
        ObjCustomerService.Close();
          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        throw ex;
    }
    finally
    {
        if (ObjCustomerService != null)
            ObjCustomerService.Close();
    }*/
        try
        {

            ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            if (Mode == "Add")
            {
                gvRepaymentDetails.ClearGrid();
                ObjStatus.Option = 52;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                ViewState["dummyRow"] = DtRepayGrid.Copy();
            }
            if (Mode == "Edit")
            {
                if ((DataTable)ViewState["DtRepayGrid"] != null)
                    DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];

            }

            gvRepaymentDetails.DataSource = DtRepayGrid;
            gvRepaymentDetails.DataBind();

            if (Mode == "Add")
            {
                DtRepayGrid.Rows.Clear();
                ViewState["DtRepayGrid"] = DtRepayGrid;
                gvRepaymentDetails.Rows[0].Cells.Clear();
                gvRepaymentDetails.Rows[0].Visible = false;
                //gvRepaymentSummary.ClearGrid();

            }

            // Bind the drop down inside the Grid view
            FunPriGenerateNewRepayment();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error filling Repayment Tab");
        }
        finally
        {
            if (ObjCustomerService != null)
                ObjCustomerService.Close();
        }

    }
    protected void btnGO_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {


            if (Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()) < Utility.StringToDate(ViewState["AccountCreationDate"].ToString()))  // To check whether the revision date is greater than account creation date
            {
                Utility.FunShowAlertMsg(this, "Specific revision date should be greater than or equal to account creation date");
                ddlEffectiveFrom.SelectedIndex = -1;
                return;
            }
            // Step 1: TO GET the Old Repayment Structure
            DataTable dtRepaymentTab;
            if (ddlSLA.SelectedValue.Equals("0") || ddlSLA.SelectedIndex == -1)
                dtRepaymentTab = FunGetRepayDetails(ddlMLA.SelectedValue.ToString(), (ddlMLA.SelectedValue.ToString() + "DUMMY"));
            else
                dtRepaymentTab = FunGetRepayDetails(ddlMLA.SelectedValue.ToString(), ddlSLA.SelectedItem.ToString());

            if (dtRepaymentTab == null || dtRepaymentTab.Rows.Count <= 0)
            {
                Utility.FunShowAlertMsg(this, LocalizationResources.RepayNotExist);
                return;
            }
            //ViewState["OldRePaymentDetails"] = dtRepaymentTab;

            // Check whether Revision Date Falls on Installment Date
            DataRow[] SpecificRows = dtRepaymentTab.Select("InstallmentDate=#" + Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()) + "#");
            if (SpecificRows.Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Specific revision date must fall on installment / due date");
                btnSave.Enabled = false;
                return;
            }


            //if (Utility.StringToDate(txtEffectiveFrom.Text).Month <= Utility.StringToDate(txtDate.Text).Month && Utility.StringToDate(txtEffectiveFrom.Text).Year <= Utility.StringToDate(txtDate.Text).Year)
            {
                short closureValue = CheckForOpenMonth(Utility.StringToDate(txtDate.Text), 1);

                if (closureValue != 5)  // Check previous month is closed
                {

                    closureValue = CheckForOpenMonth(Utility.StringToDate(txtDate.Text), 2);

                    if (closureValue == 6 || closureValue == 7)
                    {
                        tcSpecificRevision.Tabs[1].Enabled = true;
                        tcSpecificRevision.ActiveTabIndex = 1;

                        //Start:- To Dispaly Principal O/S amount By Rao 1st March 2012.

                        // NEW PRINCIPAL CALCULATION METHOD FROM DATASET
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("@PANUM", ddlMLA.SelectedValue.ToString());
                        if (ddlSLA.SelectedIndex > 0)
                            dic.Add("@SANUM", ddlSLA.SelectedItem.Text);
                        dic.Add("@COMPANY_ID", CompanyId);
                        dic.Add("@EffectiveDate", Utility.StringToDate(ddlEffectiveFrom.SelectedItem.Text.Trim()).ToString());
                        string retVal = Utility.GetTableScalarValue("S3G_LOANAD_GetPrincipalDueByAccount", dic);
                        // if account type is arrear and the repayment is not started yet
                        if (retVal.Trim().Length == 0)
                            retVal = txtFinAmt.Text;

                        txtPriAmt.Text = retVal;

                        //End:- To Dispaly Principal O/S amount By Rao 1st March 2012.
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Revision initiation date cannot fall on closed month");
                        return;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Previous month must be closed before initiating specific revision");
                    return;
                }
            }
            //else
            //{
            //    Utility.FunShowAlertMsg(this, "Revision initiation can be done only for open month");
            //    return;
            //}
        }
    }
    #endregion
    protected void txtEffectiveFrom_TextChanged(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
    }
    protected void btnRecalculate_Click(object sender, EventArgs e)
    {

        try
        {
            decimal NewSOH = Math.Round(decimal.Parse(ViewState["CalculatedFinanceAmount"].ToString())) + Math.Round(decimal.Parse(ViewState["RevisedUMFC"].ToString()));
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            decimal RepaymentTotal = decimal.Parse(DtRepayGrid.Compute("sum(TotalPeriodInstall)", "").ToString());
            if (NewSOH == RepaymentTotal)
            {
                GenerateSpecificRevision("Edit");
                btnRecalculate.Enabled = true;
            }
            else
                Utility.FunShowAlertMsg(this, "Sum of repayment details does not match with new stock on hire");
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

            Procparam.Add("@Company_ID", obj_Page.ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@Type", "GEN");
            Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
            Procparam.Add("@Program_Id", "074");
            Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@PrefixText", prefixText);
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));
        
        return suggestions.ToArray();
    }
}
