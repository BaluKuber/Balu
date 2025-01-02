#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Loan Admin
/// Screen Name         :   S3GLoanAdMonthClosure
/// Created By          :   Suresh P
/// Created Date        :   19-Aug-2010
/// Purpose             :   Month Closure
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using RuleCardMgtServicesReference;
using S3GBusEntity;
using S3GBusEntity.LoanAdmin;
using System.Text;
using LoanAdminAccMgtServicesReference;
using System.Globalization;
using System.Web.UI.WebControls;
#endregion

public partial class S3GLoanAdMonthClosure : ApplyThemeForProject
{

    Dictionary<string, string> ObjDictParams = null;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode SerMode = SerializationMode.Binary;

    int intMaxMonth;
    string intROIRuleMasterID;
    int intErrCode = 0;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end


    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3GLoanAdMonthClosure.aspx";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdMonthClosure.aspx';";
    string strRedirectPageLandView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=LADMoCL';";
    string strRedirectPageView = "S3gLoanAdTransLander.aspx?Code=LADMoCL";

    string strXMLCreditScoreDet = "<Root><Details Desc='0' /></Root>";
    string strDateFormat;
    StringBuilder strbCreditScoreDet = new StringBuilder();


    LoanAdminAccMgtServicesClient ObjLoanAdminAccMgtServicesClient = null;
    LoanAdminAccMgtServices.S3G_LOANAD_MonthClosureDataTable ObjS3G_LOANAD_MonthClosureDataTable = null;
    LoanAdminAccMgtServices.S3G_LOANAD_MonthClosureRow ObjS3G_LOANAD_MonthClosureRow = null;
    DataTable dtMonthClosure = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket formTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            strMode = Request.QueryString.Get("qsMode");
            if (formTicket != null)
            {
                intROIRuleMasterID = formTicket.Name;
            }
        }

        txtMonthClosureDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
        //cexDocumentDate.Enabled = false;

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end

        if (!IsPostBack)
        {
            FunProLoadLOB();
            ddlFinacialYear.FillFinancialYears();

            if ((intROIRuleMasterID != "") && (strMode == "M"))
            {
                FunPriDisableControls(1);
            }
            else if ((intROIRuleMasterID != "") && (strMode == "Q")) // Query // Modify
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriDisableControls(0);
            }

            btnSave.Enabled = false;
        }
    }
    private void FunPriValidationDefault(bool blnFlag, int intModeID)
    {
        if ((intModeID == 1) || (intModeID == -1))
        {
            FunGetAssetAcquisitionDetails();        // Get Asset Acquisition Details
            //FunProLoadClosureMonth();
        }
        bool blnIsReadOnly = (intModeID == -1) ? true : false;
        FunPriControlEnable(intModeID, blnIsReadOnly);
    }
    private void FunGetAssetAcquisitionDetails()
    {

        try
        {
            ObjDictParams = null;
            ObjDictParams = new Dictionary<string, string>();
            ObjDictParams.Add("@Closure_id", intROIRuleMasterID.ToString());
            DataTable dtBranchList = Utility.GetDefaultData("S3G_LOANAD_GetMonthClosureDetails", ObjDictParams);

            grvMothEndParam.DataSource = dtBranchList;
            grvMothEndParam.DataBind();

            ddlLineofBusiness.SelectedValue = dtBranchList.Rows[0]["LOB_ID"].ToString();
            FunProLoadClosureMonth();
            ddlClosureMonth.SelectedValue = dtBranchList.Rows[0]["Closure_Month"].ToString();
            ddlFinacialYear.SelectedValue = dtBranchList.Rows[0]["Financial_Year"].ToString();
            txtMonthClosureDate.Text = DateTime.Parse(dtBranchList.Rows[0]["Month_Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            CheckBox chkHdrMonthLock = ((CheckBox)grvMothEndParam.HeaderRow.FindControl("chkHdrMonthLock"));
            chkHdrMonthLock.Visible = false;
            if (Convert.ToInt32(dtBranchList.Rows[0]["StatusCode"].ToString()) == 1)
            {
                grvMothEndParam.Columns[5].Visible = true;
                btnSave.Enabled = true;
            }
            else
            {
                grvMothEndParam.Columns[5].Visible = false;
                btnSave.Enabled = false;

            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjDictParams = null;
        }

        //ObjLoanAdminAccMgtServicesClient = new AssetMgtServicesClient();
        //try
        //{
        //    ObjS3G_LOANAD_LeaseAssetRegisterDataTable = new AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable();
        //    ObjS3G_LOANAD_LeaseAssetRegisterRow = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.NewS3G_LOANAD_LeaseAssetRegisterRow();
        //    //ObjS3G_LOANAD_LeaseAssetRegisterRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
        //    //ObjS3G_LOANAD_LeaseAssetRegisterRow.Branch_ID = 1;
        //    //ObjS3G_LOANAD_LeaseAssetRegisterRow.LOB_ID = 3;
        //    ObjS3G_LOANAD_LeaseAssetRegisterRow.Acquisition_No = intROIRuleMasterID; // "2010-2011/AAQ/3";
        //    ObjS3G_LOANAD_LeaseAssetRegisterDataTable.AddS3G_LOANAD_LeaseAssetRegisterRow(ObjS3G_LOANAD_LeaseAssetRegisterRow);

        //    byte[] byteAssetAcquisition = ObjLoanAdminAccMgtServicesClient.FunPubQueryAssetAcquisition(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_LeaseAssetRegisterDataTable, SerMode));

        //    ObjS3G_LOANAD_LeaseAssetRegisterDataTable = new AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable();
        //    ObjS3G_LOANAD_LeaseAssetRegisterDataTable = (AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable)ClsPubSerialize.DeSerialize(byteAssetAcquisition, SerMode, typeof(AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable));

        //    ViewState["DT_AssetAcquisition_Availability"] = ObjS3G_LOANAD_LeaseAssetRegisterDataTable;

        //    //dtAssetAcquisition_Availability = FunPriGetAssetAcquisitionAvailabilityDataTable();
        //    //FunPubBindAssetAcquisitionAvailability(dtAssetAcquisition_Availability);


        //    gvAssetAcquisition.DataSource = ObjS3G_LOANAD_LeaseAssetRegisterDataTable;
        //    gvAssetAcquisition.DataBind();
        //    gvAssetAcquisition.ShowFooter = false;

        //    gvAssetAcquisitionAvailability.DataSource = ObjS3G_LOANAD_LeaseAssetRegisterDataTable;
        //    gvAssetAcquisitionAvailability.DataBind();

        //    ddlLineofBusiness.SelectedValue = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["LOB_ID"].ToString();
        //    ddlBranch.SelectedValue = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["Branch_ID"].ToString();
        //    txtLAAEDate.Text = DateTime.Parse(ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["Acquisition_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        //}
        //catch (FaultException<AssetMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{
        //    lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        //}
        //catch (Exception ex)
        //{
        //    lblErrorMessage.Text = ex.Message;
        //}
        //finally
        //{
        //    ObjLoanAdminAccMgtServicesClient.Close();
        //}
    }
    private void FunPriControlEnable(int intModeID, bool blnIsReadOnly)
    {

        string strProperty = "readonly";
        ///Set readonly property to the controls
        //if (blnIsReadOnly)
        //{
        if (intModeID != 0)
        {
            ddlLineofBusiness.ClearDropDownList();
            ddlClosureMonth.ClearDropDownList();
            ddlFinacialYear.ClearDropDownList();
        }
        //}
        //else
        //{
        //}


        //bool blnFlag = (intModeID > 0) ? false : true;
        /////Set Enable or disable to the controls
        //ddlLineofBusiness.Enabled = blnFlag;
        //ddlClosureMonth.Enabled = blnFlag;
    }
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                {
                    FunPriValidationDefault(true, intModeID);

                    //txtRecoveryPatternYear1.Text = "0.00";
                    //txtRecoveryPatternYear2.Text = "0.00";
                    //txtRecoveryPatternYear3.Text = "0.00";
                    //txtRecoveryPatternYearRest.Text = "0.00";

                    //txtRecoveryPatternYear1.Enabled = true;
                    //txtRecoveryPatternYear2.Enabled = false;
                    //txtRecoveryPatternYear3.Enabled = false;
                    //txtRecoveryPatternYearRest.Enabled = false;

                    ////txtRate.Text = "0.0000";
                    //FunPriSetControlAttributes();
                    //FunPriRateTypeDropdown(false);
                    ////FunPriRateTypeMandotary(false);
                    //FunPriFillRateTypeDropdown(false, false);
                    //FunPriRatePatternDropdown(false, false, false, false, false);
                    //FunPriTimeDropdown(false, false, false, false);
                    //FunPriFrequencyDropdown(false, false, false, false, false, false, false);
                    //FunPriRepaymentMode(false, false, false, false, false);
                    //FunPriEnableIRRRest(false);
                    //FunPriIntrestCalculationDropdown(false, false, false, false, false);
                    //FunPriIntrestLevyDropdown(false, false, false, false, false);
                    //FunPriInterestCalculationMandotary(false);
                    //FunPriRecoveryPatternMandotary(false);
                    //FunPriInsuranceDropdown(false);
                    //FunPriResidualValueDropdown(false);
                    //FunPriMarginDropdown(false, true);

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    //chkActive.Checked = true;
                    btnClear.Enabled = true;
                    //chkActive.Enabled = false;
                    ddlLineofBusiness.Focus();
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    break;
                }
            case 1: // Modify Mode
                {
                    FunPriValidationDefault(false, intModeID);
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    btnClear.Enabled = false;
                    //chkActive.Enabled = true;
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    break;
                }
            case -1:// Query Mode
                {
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }
                    FunPriValidationDefault(false, intModeID);
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    //chkActive.Enabled = false;
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    break;
                }
        }
    }
    protected void FunProLoadLOB()
    {
        try
        {
            ObjDictParams = null;
            ObjDictParams = new Dictionary<string, string>();
            ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            ObjDictParams.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            ObjDictParams.Add("@Is_Active", "1");
            ObjDictParams.Add("@Program_ID", "65");
            ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, ObjDictParams, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjDictParams = null;
        }
    }
    protected void ddlLineOfBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
       // FunProLoadClosureMonth();
        ddlClosureMonth.SelectedIndex = -1;
        ddlFinacialYear.SelectedIndex = -1;
        grvMothEndParam.DataSource = null;
        grvMothEndParam.DataBind();
    }
    protected void FunProLoadClosureMonth()
    {
        int intCurrentMonth = 0;
        intCurrentMonth = DateTime.Now.Month;
        if (intCurrentMonth > 3)
        {
            lblYearLock.Text = DateTime.Now.AddYears(intMaxMonth).Year.ToString() + "-" + DateTime.Now.AddYears(intMaxMonth + 1).Year.ToString();
        }
        else
        {
            lblYearLock.Text = (DateTime.Now.AddYears(intMaxMonth).Year - 1).ToString() + "-" + (DateTime.Now.AddYears(intMaxMonth + 1).Year - 1).ToString();   
        }
        
        ddlClosureMonth.FillFinancialMonth(lblYearLock.Text);
    }
    protected void ddlClosureMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlClosureMonth.SelectedIndex > 0)
        {
            FunProLoadBranch(ObjUserInfo.ProCompanyIdRW);
        }
        else
        {
            grvMothEndParam.DataSource = null;
            grvMothEndParam.DataBind();
        }
    }


    protected void ddlFinacialYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFinacialYear.SelectedIndex > 0)
        {
            lblYearLock.Text = ddlFinacialYear.SelectedItem.Text;
            lblMonthLock.Text = "";
            ddlClosureMonth.FillFinancialMonth(ddlFinacialYear.SelectedItem.Text);
            return;
        }
        else
        {
            ddlClosureMonth.SelectedIndex = -1;
            ddlClosureMonth.ClearDropDownList();
            grvMothEndParam.DataSource = null;
            grvMothEndParam.DataBind();
        }
    }


    protected void grvMothEndParam_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        bool blnStatus = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblStatusCode = e.Row.FindControl("lblStatusCode") as Label;

            CheckBox chkMonth = e.Row.FindControl("chkMonth") as CheckBox;
            chkMonth.Attributes.Add("StatusCode", lblStatusCode.Text.ToString());

            if (lblStatus.Text.ToLower().Equals("open"))
            {
                chkMonth.Visible = true;
                blnStatus = true;
            }
            else
            {
                chkMonth.Visible = false;
            }

            if (strMode == "M")
            {
                /*if (Convert.ToInt32(lblStatusCode.Text) == 1)
                {
                    chkMonth.Visible = true;
                }
                else
                {
                    chkMonth.Visible = false;
                }*/
                chkMonth.Visible = false;
            }

            /*if (strMode == "Q" || strMode == "M")
            {
                //lnkEdit.Enabled = false;
                lnkRemove.Enabled = false;
            }
            */
        }
        if (blnStatus)
        {
            btnSave.Enabled = true;
        }
    }
    /// <summary>
    /// Load the Branches for the company
    /// </summary>
    /// <param name="intCompanyid"></param>
    protected void FunProLoadBranch(int intCompanyid)
    {
        bool blnAnyOpenCheckBox = false;
        try
        {
            ObjDictParams = null;
            ObjDictParams = new Dictionary<string, string>();
            ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            ObjDictParams.Add("@LOB_ID", ddlLineofBusiness.SelectedValue.ToString());
            ObjDictParams.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            if (ddlClosureMonth.SelectedIndex == 1)
            {
                ObjDictParams.Add("@Is_First", "1");
            }
            ObjDictParams.Add("@Closure_Month", ddlClosureMonth.SelectedValue.ToString());
            DataTable dtBranchList = Utility.GetDefaultData("S3G_LOANAD_GetMonthClosure", ObjDictParams);

            //ViewState["DT_MonthClosure"] = dtBranchList;

            grvMothEndParam.DataSource = dtBranchList;
            grvMothEndParam.DataBind();

            foreach (GridViewRow grvMonthEndParamRow in grvMothEndParam.Rows)
            {
                Label lblStatus = grvMonthEndParamRow.FindControl("lblStatus") as Label;
                if (lblStatus.Text.ToLower().Equals("open"))
                {
                    blnAnyOpenCheckBox = true;
                }
            }


            if (blnAnyOpenCheckBox)
            {
                grvMothEndParam.Columns[5].Visible = true;
                btnSave.Enabled = true;
            }
            else
            {
                grvMothEndParam.Columns[5].Visible = false;
                btnSave.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjDictParams = null;
        }
    }
    /// <summary>
    /// CheckBox Event inside the grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void chkSelLOB_OnCheckedChanged(object sender, EventArgs e)
    {
        bool blnAllCheks = true;
        CheckBox chkLOB = null;
        foreach (GridViewRow grvMonthEndParamRow in grvMothEndParam.Rows)
        {
            chkLOB = ((CheckBox)grvMonthEndParamRow.FindControl("chkMonth"));
            if (!chkLOB.Checked && chkLOB.Visible)
            {
                blnAllCheks = false;
                break;
            }
        }
        CheckBox chkHdrMonthLock = ((CheckBox)grvMothEndParam.HeaderRow.FindControl("chkHdrMonthLock"));
        if (!blnAllCheks)
        {
            chkHdrMonthLock.Checked = false;
        }
        else
        {
            chkHdrMonthLock.Checked = true;
        }
        //DataTable dtRoleCode = null;
        /*
        int intLOB_ID = 0;
        CheckBox chkLOB = null;
        foreach (GridViewRow grvMonthEndParamRow in grvMothEndParam.Rows)
        {
            chkLOB = ((CheckBox)grvMonthEndParamRow.FindControl("chkMonth"));
            if (chkLOB.Checked)
            {
                int intStatusCode = Convert.ToInt32(chkLOB.Attributes["StatusCode"].ToString());
                if (intStatusCode == 3)
                {
                    Utility.FunShowAlertMsg(this.Page, "Previous month is not closed for this branch");
                    return;
                }
                else if (intStatusCode == 2)
                {
                    Utility.FunShowAlertMsg(this.Page, "Unable to process. Please select greater of this closure month");
                    return;
                }


                intLOB_ID = Convert.ToInt32(((Label)grvMonthEndParamRow.FindControl("lblBranchId")).Text);
            }
            //else
              //  chkLOB.Enabled = false;
        }
        */
        /* if (intLOB_ID == 0)
         {
             foreach (GridViewRow grvMonthEndParamRow in grvMothEndParam.Rows)
             {
                 chkLOB = ((CheckBox)grvMonthEndParamRow.FindControl("chkMonth"));
                 chkLOB.Enabled = true;
             }
         }*/

    }
    /// <summary>
    /// Get Xml for Month Closure Details
    /// </summary>
    /// <returns></returns>
    private bool FunPriGenerateMonthClosureXMLDet()
    {
        int intLOB_ID = 0;
        CheckBox chkLOB = null;
        strbCreditScoreDet.Append("<Root>");
        foreach (GridViewRow grvMonthEndParamRow in grvMothEndParam.Rows)
        {
            chkLOB = ((CheckBox)grvMonthEndParamRow.FindControl("chkMonth"));
            if (chkLOB.Checked)
            {
                intLOB_ID = Convert.ToInt32(((Label)grvMonthEndParamRow.FindControl("lblBranchId")).Text);
                //int intStatusCode = Convert.ToInt32(chkLOB.Attributes["StatusCode"].ToString());
                //if (intStatusCode == 0)
                //{
                    strbCreditScoreDet.Append(" <Details ");
                    strbCreditScoreDet.Append(" Company_ID = '" + ObjUserInfo.ProCompanyIdRW + "'");
                    strbCreditScoreDet.Append(" LOB_ID = '" + ddlLineofBusiness.SelectedValue.ToString() + "'");
                    strbCreditScoreDet.Append(" Branch_ID = '" + intLOB_ID.ToString() + "'");
                    strbCreditScoreDet.Append(" Closure_Month = '" + ddlClosureMonth.SelectedValue.ToString() + "'");
                    strbCreditScoreDet.Append(" />");
                //}
            }
        }
        strbCreditScoreDet.Append("</Root>");
        strXMLCreditScoreDet = strbCreditScoreDet.ToString();
        return true;

        /*

        dtMonthClosure = (DataTable)ViewState["DT_MonthClosure"];
        strbCreditScoreDet.Append("<Root>");
        foreach (DataRow drow in dtMonthClosure.Rows)
        {
            strbCreditScoreDet.Append(" <Details ");
            strbCreditScoreDet.Append(" Branch_ID = '" + drow["Branch_ID"].ToString() + "'");
            strbCreditScoreDet.Append(" />");
        }
        strbCreditScoreDet.Append("</Root>");
        strXMLCreditScoreDet = strbCreditScoreDet.ToString();
        return true;
        */
    }
    #region Button (Save / Clear / Cancel)
    /// <summary>
    /// Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";

        /// Check the Current Month 
        string strClosureMonth = ddlClosureMonth.SelectedValue;
        DateTime dtCurrenctMonth = Utility.StringToDate(txtMonthClosureDate.Text);
        string strCurrenctMonth = dtCurrenctMonth.Year.ToString() + ((dtCurrenctMonth.Month.ToString().Length == 1) ? ("0" + dtCurrenctMonth.Month).ToString() : dtCurrenctMonth.Month.ToString());
        int intClosureCheck = string.Compare(strClosureMonth, strCurrenctMonth);
        if (intClosureCheck > 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Closure Month should be in less than or equal to the current month");
            return;
        }
        
        int intLOB_ID = 0;
        CheckBox chkLOB = null;
        foreach (GridViewRow grvMonthEndParamRow in grvMothEndParam.Rows)
        {
            chkLOB = ((CheckBox)grvMonthEndParamRow.FindControl("chkMonth"));
            if (chkLOB.Checked)
            {
                intLOB_ID = Convert.ToInt32(((Label)grvMonthEndParamRow.FindControl("lblBranchId")).Text);
            }
        }
        if (intLOB_ID == 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Select the branch");
            return;
        }
        /// Check atleast one branch Selected
      /*  int intLOB_ID = 0;
        CheckBox chkLOB = null;
        string strPreviousMonth = "Previous month is not closed for these branche(s)";
        string strNextMonth = "Unable to process these branches. Please select greater of this closure month";
        bool blnFound = false, blnFoundNext = false;
        int intCountPre = 0, intCountNext = 0;
        foreach (GridViewRow grvMonthEndParamRow in grvMothEndParam.Rows)
        {
            chkLOB = ((CheckBox)grvMonthEndParamRow.FindControl("chkMonth"));
            if (chkLOB.Checked)
            {
                intLOB_ID = Convert.ToInt32(((Label)grvMonthEndParamRow.FindControl("lblBranchId")).Text);

                int intStatusCode = Convert.ToInt32(chkLOB.Attributes["StatusCode"].ToString());
                if (intStatusCode == 3)
                {
                    blnFound = true;
                    strPreviousMonth += @"\r\n" + ((Label)grvMonthEndParamRow.FindControl("lblBranch")).Text.ToString();
                    intCountPre++;
                }
                if (intStatusCode == 2)
                {
                    blnFoundNext = true;
                    strNextMonth += @"\r\n" + ((Label)grvMonthEndParamRow.FindControl("lblBranch")).Text.ToString();
                    intCountNext++;
                }
            }
        }
        if (intLOB_ID == 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Please select the branch");
            return;
        }
        if (blnFound)
        {
            strPreviousMonth = (intCountPre == 1) ? strPreviousMonth.Replace("these branches", "this branch") : strPreviousMonth;
            Utility.FunShowAlertMsg(this.Page, strPreviousMonth);
            return;
        }
        if (blnFoundNext)
        {
            strNextMonth = (intCountNext == 1) ? strNextMonth.Replace("these branches", "this branch") : strNextMonth;
            Utility.FunShowAlertMsg(this.Page, strNextMonth);
            return;
        }
        */
        try
        {
            if (!FunPriGenerateMonthClosureXMLDet())
            {
                return;
            }
            ObjS3G_LOANAD_MonthClosureDataTable = new LoanAdminAccMgtServices.S3G_LOANAD_MonthClosureDataTable();
            ObjS3G_LOANAD_MonthClosureRow = null;
            ObjS3G_LOANAD_MonthClosureRow = ObjS3G_LOANAD_MonthClosureDataTable.NewS3G_LOANAD_MonthClosureRow();
            ObjS3G_LOANAD_MonthClosureRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
            ObjS3G_LOANAD_MonthClosureRow.LOB_ID = Convert.ToInt32(ddlLineofBusiness.SelectedValue);
            ObjS3G_LOANAD_MonthClosureRow.Month_Closure_Date = Utility.StringToDate(txtMonthClosureDate.Text);
            ObjS3G_LOANAD_MonthClosureRow.Closure_Month = ddlClosureMonth.SelectedValue;
            ObjS3G_LOANAD_MonthClosureRow.Financial_Year = ddlFinacialYear.SelectedValue;
            ObjS3G_LOANAD_MonthClosureRow.Month_Closure_Type_code = 1;
            ObjS3G_LOANAD_MonthClosureRow.Month_Closure_Type = 1;
            ObjS3G_LOANAD_MonthClosureRow.Created_By = ObjUserInfo.ProUserIdRW;
            ObjS3G_LOANAD_MonthClosureRow.TXN_ID = 1;
            ObjS3G_LOANAD_MonthClosureRow.XMLParamMonthClosureDet = strXMLCreditScoreDet;
            ObjS3G_LOANAD_MonthClosureDataTable.AddS3G_LOANAD_MonthClosureRow(ObjS3G_LOANAD_MonthClosureRow);

            ObjLoanAdminAccMgtServicesClient = new LoanAdminAccMgtServicesClient();
            intErrCode = ObjLoanAdminAccMgtServicesClient.FunPubCreateMonthClosure(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_MonthClosureDataTable, SerMode));
            if (intErrCode == 0)
            {
                //Utility.FunShowAlertMsg(this.Page, "Month Closure added successfully", strRedirectPageView);
                strAlert = "Month Closure added successfully";
                strAlert += @"\n\nWould you like to add one more Month Closure?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageLandView + "}";
                strRedirectPageLandView = "";
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageLandView, true);
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Previous Closure month is not yet closed ");
                strRedirectPageView = "";
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Previous Closure month is not defined in GPS");
                strRedirectPageView = "";
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "Current Closure month is already Closed ");
                strRedirectPageView = "";
            }
            else if (intErrCode == 4)
            {
                Utility.FunShowAlertMsg(this.Page, "Closure month is not defined in GPS ");
                strRedirectPageView = "";
            }
            else if (intErrCode == 5)
            {
                Utility.FunShowAlertMsg(this.Page, "Billing not processed for the selected branch");
                strRedirectPageView = "";
            }
            else if (intErrCode == 6)
            {
               Utility.FunShowAlertMsg(this.Page, "ODI not processed for the selected branch");
                strRedirectPageView = "";
            }
            else if (intErrCode == 7)
            {
                Utility.FunShowAlertMsg(this.Page, "Deliquency not processed for the selected branch ");
                strRedirectPageView = "";
            }
                else if (intErrCode == 8)
            {
                Utility.FunShowAlertMsg(this.Page, "Interest calculation not processed for the selected branch");
                strRedirectPageView = "";
            }
                else if (intErrCode == 9)
            {
                Utility.FunShowAlertMsg(this.Page, "Demand not processed for the selected branch ");
                strRedirectPageView = "";
            }
            //else if (intErrCode == 21)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Branch is already Closed for this closure month");
            //    //strAlert = strAlert.Replace("__ALERT__", "Month Closure already added for this branch");
            //    strRedirectPageView = "";
            //}
            strAlert = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (FaultException<LoanAdminAccMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjLoanAdminAccMgtServicesClient.Close();
        }
    }
    /// <summary>
    /// Clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlLineofBusiness.SelectedValue = "0";
        ddlClosureMonth.SelectedValue = "0";
        ddlFinacialYear.SelectedValue = "0";
        CheckBox chkLOB = null;
        foreach (GridViewRow grvMonthEndParamRow in grvMothEndParam.Rows)
        {
            chkLOB = ((CheckBox)grvMonthEndParamRow.FindControl("chkMonth"));
            chkLOB.Checked = false;
            chkLOB.Enabled = true;
            chkLOB = null;
        }
        grvMothEndParam.DataSource = null;
        grvMothEndParam.DataBind();
        btnSave.Enabled = false;
    }
    /// <summary>
    /// Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPageView);
    }
    #endregion
}