/// Module Name     :   Origination
/// Screen Name     :   Enquiry Response
/// Created By      :   Narayanan
/// Created Date    :   25-May-2010
/// Purpose         :   To Insert and Update 
/// Modified By     :   M.Saran 
/// Modified Date   :   
/// Purpose         :   To Bug Fix and Update the Functionality

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
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
public partial class Orgination_S3GORGEnquiryResponse : ApplyThemeForProject
{

    #region Variable declaration

    EnquiryMgtServicesReference.EnquiryMgtServicesClient ObjEnquiryResponseClient;

    public string strDateFormat;
    int intCompanyId = 0, intUserId = 0, intSerialNo = 0;
    string strRedirectPage = "~/Origination/S3GORGTransLander.aspx?Code=ENRES&Create=1&Modify=1";
    int intEnquiryResponseId = 0;
    int intEnquiryUpdationId = 0;
    int intErrorCode = 0;
    int Tenure = 0;

    static string strPageName = "Enquiry Response";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgTranslander.aspx?Code=ENRES&Create=1&Modify=1';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGEnquiryResponse.aspx?qsMode=C';";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";

    string strROIDetails = "";
    string strAlertDetails = "";
    string strFollowupDetails = "";
    string strPaymentDetails = "";
    string strInflowDetails = "";
    string strOutflowDetails = "";
    string strRepaymentDetails = "";
    string TenureType = "";
    string StrQSMode = "";
    string strLineNumber = "";
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;

    //User Authorization
    string strMode = string.Empty;
    bool bClearList = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    Dictionary<string, string> Procparam = null;

    bool PaintBG = true;

    DataTable DtAlertDetails = new DataTable();
    DataTable DtFollowUp = new DataTable();
    DataTable DtCashFlow = new DataTable();
    DataTable DtCashFlowOut = new DataTable();
    DataTable DtRepayGrid = new DataTable();
    DataTable DtRepaySummary = new DataTable();
    bool blnIsworkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"].ToString());



    #region "paging"
    int intNoofSearch = 6;
    string[] arrSortCol = new string[] { "LOB", "Location", "Product", "EnquiryNo", "EnquiryDate", "CustomerCode+' - '+Customer_Name" };
    string strProcName = "S3G_ORG_GetEnquiryResponse_Paging";
    ArrayList arrSearchVal = new ArrayList(1);
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

    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        arrSearchVal = grvEnquiryUpdation.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        grvEnquiryUpdation.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        grvEnquiryUpdation.FunPriSetSearchValue(arrSearchVal);
    }

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                }
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvEnquiryUpdation.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private string FunPriGetSortDirectionStr(string strColumn)
    {
        string strSortDirection = string.Empty;
        string strSortExpression = string.Empty;
        // By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;
        strSortDirection = "DESC";
        try
        {

            strSortExpression = hdnSortExpression.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
            // Save new values in hidden control.
            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strColumn;
            strOrderBy = " " + strColumn + " " + strSortDirection;
            hdnOrderBy.Value = strOrderBy;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
        return strSortDirection;
    }



    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (lnkbtnSearch.ID == "lnkbtnSort" + (iCount + 1).ToString())
                {
                    strSortColName = arrSortCol[iCount].ToString();
                    break;
                }
            }

            string strDirection = string.Empty;
            string strSortDirection = string.Empty;

            if (((ImageButton)grvEnquiryUpdation.HeaderRow.FindControl(imgbtnSearch)).CssClass == "styleImageSortingAsc")
            {
                strSortDirection = "DESC";
            }
            else
            {
                strDirection = "ASC";
            }

            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strSortColName;
            hdnOrderBy.Value = " " + strSortColName + " " + strSortDirection;

            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvEnquiryUpdation.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvEnquiryUpdation.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }



    private void FunPriBindGrid()
    {
        try
        {
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            FunPriGetSearchValue();


            Procparam = new Dictionary<string, string>();

            grvEnquiryUpdation.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvEnquiryUpdation.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }

    }


    #endregion
    #endregion


    #endregion

    #region Page Load

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();


            #region " WF INITIATION"
            ProgramCode = "046";
            #endregion

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intEnquiryResponseId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Enquiry Response Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
                StrQSMode = strMode;
            }


            //txtResidualValue_Cashflow.Attributes.Add("onblur", "ToggleResidual(this," + txtResidualAmt_Cashflow.ClientID + ")");
            //txtResidualAmt_Cashflow.Attributes.Add("onblur", "ToggleResidual(this," + txtResidualValue_Cashflow.ClientID + ")");
            //txtMarginMoneyPer_Cashflow.Attributes.Add("onblur", "ToggleResidual(this," + txtMarginMoneyAmount_Cashflow.ClientID + ")");
            //txtMarginMoneyAmount_Cashflow.Attributes.Add("onblur", "ToggleResidual(this," + txtMarginMoneyPer_Cashflow.ClientID + ")");
            /*txtResidualValue_Cashflow.Attributes.Add("onfocusOut", "ToggleResidual(this," + txtResidualAmt_Cashflow.ClientID + ")");
            txtResidualAmt_Cashflow.Attributes.Add("onfocusOut", "ToggleResidual(this," + txtResidualValue_Cashflow.ClientID + ")");
            txtMarginMoneyPer_Cashflow.Attributes.Add("onfocusOut", "ToggleResidual(this," + txtMarginMoneyAmount_Cashflow.ClientID + ")");
            txtMarginMoneyAmount_Cashflow.Attributes.Add("onfocusOut", "ToggleResidual(this," + txtMarginMoneyPer_Cashflow.ClientID + ")");*/


            #region Paging Config
            arrSearchVal = new ArrayList(intNoofSearch);
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



            if (!IsPostBack)
            {
                try
                {
                    if (ddlROIRuleList.SelectedIndex == -1)
                    {
                        FunPriToggleROIControls(false);
                    }
                    else if (ddlPaymentRuleList.SelectedIndex == -1)
                    {
                        FunPriToggleROIControls(false);
                    }
                    else
                    {
                        FunPriToggleROIControls(true);
                    }


                    FunProGetIRRDetails();
                    FunPriLoadDropDownList();
                    //User Authorization
                    bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                    if (intEnquiryResponseId > 0)
                    {
                        if (strMode == "M")
                        {
                            // btnNext.Enabled = true;
                            btnSave.ValidationGroup = "EnquiryResponse2";
                            // btnNext.ValidationGroup = "EnquiryResponse2";
                            btnSave.Attributes.Add("onclick", "return fnCheckPageValidators('EnquiryResponse2','f');");
                            // btnNext.Attributes.Add("onclick", "return fnCheckPageValidators('EnquiryResponse2','f');");
                            vs_Main.ValidationGroup = "EnquiryResponse2";
                            FunPriLoadEnquiryResponseDetails(intEnquiryResponseId);
                            FunPriDisableControls(1);

                        }
                        else
                        {
                            //btnNext.Enabled = true;
                            FunPriLoadEnquiryResponseDetails(intEnquiryResponseId);
                            FunPriDisableControls(-1);
                        }
                        FunPriLoadROIDropDown();

                    }
                    else
                    {
                        btnSave.ValidationGroup = "EnquiryResponse2";
                        //  btnNext.ValidationGroup = "EnquiryResponse2";
                        btnSave.Attributes.Add("onclick", "return fnCheckPageValidators('EnquiryResponse2','f');");
                        // btnNext.Attributes.Add("onclick", "return fnCheckPageValidators('EnquiryResponse2','f');");
                        vs_Main.ValidationGroup = "EnquiryResponse2";
                        FunPriDisableControls(0);
                    }
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                }
            }
            FunPriSetMaxLength();
            //FunPriValidateApplicationStart();

            if (PageMode == PageModes.WorkFlow)
            {
                try
                {
                    PreparePageForWorkFlowLoad();
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                    Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
                }
            }

        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void PreparePageForWorkFlowLoad()
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            // Get The IDVALUE from Document Sequence #
            intEnquiryUpdationId = GetUniqueIdFromDOCNo(WFSessionValues.WorkFlowDocumentNo, ProgramCode);
            if (intEnquiryUpdationId == 0)
            {
                return;
            }

            FunPriLoadEnquiryDetails(Convert.ToString(intEnquiryUpdationId));
            FunPriBindCashFlowDetails();
            FunPriLoadPaymentDropDown();
            FunPriBindRepaymentDetails("0");

            FunPriBindAlertDetails();

            FunPriBindFollowupDetails();
            // TabContainerER.ActiveTab = TabContainerER.Tabs[1];
            TabContainerER.ActiveTabIndex = 1;
            gvPaymentRuleDetails.DataSource = null;
            gvPaymentRuleDetails.DataBind();
            divROIRuleInfo.Visible = false;
            div8.Visible = false;
            //FunPriClearROIValues();
            RFVresidualvalue.Enabled = false;
            hdnROIRule.Value = "";
            hdnPayment.Value = "";
            ddlResponse.Focus();
        }
    }




    private void FunPriSetMaxLength()
    {

        if (!IsPostBack)
        {
            txtMarginMoneyAmount_Cashflow.CheckGPSLength(true, "Margin Amount");
            txtFinanceAmount.CheckGPSLength(true, "Finance Amount");
            txtResidualvalue.CheckGPSLength(false, "Residual Value");
            txtResidualValue_Cashflow.SetDecimalPrefixSuffix(2, 4, false, "Residual %");
            txtResidualAmt_Cashflow.CheckGPSLength(false, "Residual Amount");
            funPrisetratemaxlength();


        }
        if (gvOutFlow != null)
        {
            if (gvOutFlow.FooterRow != null)
            {
                TextBox txtAmount = gvOutFlow.FooterRow.FindControl("txtAmount_Outflow") as TextBox;
                txtAmount.CheckGPSLength(true, "Amount Outflow");
            }
        }
        if (gvInflow != null)
        {
            if (gvInflow.FooterRow != null)
            {
                TextBox txtAmountInflow = gvInflow.FooterRow.FindControl("txtAmount_Inflow") as TextBox;
                txtAmountInflow.CheckGPSLength(false, "Amount Inflow");
            }
        }
        if (gvRepaymentDetails != null)
        {
            if (gvRepaymentDetails.FooterRow != null)
            {
                TextBox txtBreakPer = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;
                txtBreakPer.SetDecimalPrefixSuffix(2, 2, false, "Breakup Percentage");
            }
        }
    }

    private void funPrisetratemaxlength()
    {

        if (ddl_Return_Pattern.SelectedIndex > 0)
        {
            if (ddl_Return_Pattern.SelectedValue == "1" || ddl_Return_Pattern.SelectedValue == "2")
            {
                txt_Rate.SetDecimalPrefixSuffix(2, 4, false, false, "Rate");
            }
            else
            {
                txt_Rate.SetDecimalPrefixSuffix(5, 4, false, false, "Rate");
            }
        }
        else
        {
            txt_Rate.SetDecimalPrefixSuffix(2, 4, false, false, "Rate");
        }
    }

    #endregion

    #region DropDownList Events

    protected void ddlResponse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlResponse.SelectedIndex > 0)
            {
                if (ddlResponse.SelectedValue == "190" || ddlResponse.SelectedValue == "191" || ddlResponse.SelectedValue == "192")
                {
                    txtStatus.Text = "Rejected";
                    ViewState["statusID"] = "198";
                }
                else if (ddlResponse.SelectedValue == "189" || ddlResponse.SelectedValue == "188" || ddlResponse.SelectedValue == "233")
                {

                    txtStatus.Text = "Responded";
                    ViewState["statusID"] = "197";
                    if (ddlLOBAssign.Items.Count <= 1)
                    {
                        string t = ddlResponse.SelectedValue;
                        FunPriLoadDropDownList();
                        ddlResponse.ClearSelection();
                        ddlResponse.SelectedValue = t;
                    }
                }
                else
                {
                    txtStatus.Text = "Under Process";
                    ViewState["statusID"] = "193";
                }
            }
            else
            {
                txtStatus.Text = "Under Process";
                ViewState["statusID"] = "193";
            }

            //Added by saran on 22-Nov-2011 for observation raised by malolan.


            if (StrQSMode == "M")
            {
                if (ViewState["cashlobid"] != null)
                    FunPriLoadProductROI(ViewState["cashlobid"].ToString());
                FunPriLoadPaymentDropDown();
            }




        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }


    protected void ddlLOBAssign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hdnROIRule.Value = "";
            hdnPayment.Value = "";
            FunPriToggleROIControls(false);
            if (Convert.ToInt64(ddlLOBAssign.SelectedValue) > 0)
                FunPriLoadProductROI(ddlLOBAssign.SelectedValue);
            else
                FunPriLoadProductROI(ViewState["cashlobid"].ToString());
            gvPaymentRuleDetails.DataSource = null;
            gvPaymentRuleDetails.DataBind();
            if (ddlProductAssign.SelectedIndex > 0)
                FunPriLoadPaymentDropDown();
            FunPriBindCashFlowDetails();
            FunPriBindRepaymentCashflowDetails();
            if (ddlLOBAssign.SelectedIndex > 0)
            {
                FunPriAssignRate(ddlLOBAssign.SelectedValue);
            }
            else
            {
                if (ViewState["cashlobid"] != null)
                    FunPriAssignRate(ViewState["cashlobid"].ToString());

            }
            FunPriLoadWorkflow(ddlLOBAssign.SelectedValue, ddlProductAssign.SelectedValue, true);
            FungetLocationdetails("S3g_Org_GetCustomerLookup");
            FunOLRelatedChanges();

            FunChangeLOB();
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }

    }

    protected void ddlProductAssign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadWorkflow(ddlLOBAssign.SelectedValue, ddlProductAssign.SelectedValue, true);
            FunPriLoadPaymentDropDown();

        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void ddlBranchAssign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunChangeLOB();
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void ddlROIRuleList_SelectedIndexChanged(object sender, EventArgs e)
    {
        /* txtMarginMoneyPer_Cashflow.Text =
         txtMarginMoneyAmount_Cashflow.Text =
         txtResidualAmt_Cashflow.Text =
         txtResidualValue_Cashflow.Text = "";
        // RFVresidualvalue.Enabled = false;
         if (ddlROIRuleList.SelectedIndex == 0)
         {
             divROIRuleInfo.Visible = false;
             return;
         }
         btnFetchROI.Focus();*/
    }

    protected void btnFetchROI_Click(object sender, EventArgs e)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            txtMarginMoneyPer_Cashflow.Text =
            txtMarginMoneyAmount_Cashflow.Text =
            txtResidualAmt_Cashflow.Text =
            txtResidualValue_Cashflow.Text = "";
            if (ddlROIRuleList.SelectedIndex == 0)
            {
                divROIRuleInfo.Visible = false;
                //Utility.FunShowAlertMsg(this, "Select the ROI Rule");
                return;
            }
            FunPriToggleROIControls(true);
            divROIRuleInfo.Visible = true;
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            DataTable ObjDTROI = new DataTable();
            ObjStatus.Option = 40;
            ObjStatus.Param1 = ddlROIRuleList.SelectedValue;
            ObjDTROI = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["ROIDetails"] = ObjDTROI;
            strROIDetails = ObjDTROI.FunPubFormXml(true);
            string strRoirValue;
            if (hdnROIRule.Value != "")
            {
                strRoirValue = hdnROIRule.Value;
            }
            else
            {
                strRoirValue = "0";
            }

            switch (ObjDTROI.Rows[0]["Repayment_Mode"].ToString())
            {
                case "3":
                    Dictionary<int, decimal> dictRecovery = new Dictionary<int, decimal>();
                    if (!string.IsNullOrEmpty(ObjDTROI.Rows[0]["Recovery_Pattern_Year1"].ToString()) && Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year1"].ToString()) != 0)
                        dictRecovery.Add(1, Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year1"].ToString()));

                    if (!string.IsNullOrEmpty(ObjDTROI.Rows[0]["Recovery_Pattern_Year2"].ToString()) && Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year2"].ToString()) != 0)
                        dictRecovery.Add(2, Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year2"].ToString()));

                    if (!string.IsNullOrEmpty(ObjDTROI.Rows[0]["Recovery_Pattern_Year3"].ToString()) && Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year3"].ToString()) != 0)
                        dictRecovery.Add(3, Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Year3"].ToString()));

                    if (!string.IsNullOrEmpty(ObjDTROI.Rows[0]["Recovery_Pattern_Rest"].ToString()) && Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Rest"].ToString()) != 0)
                        dictRecovery.Add(4, Convert.ToDecimal(ObjDTROI.Rows[0]["Recovery_Pattern_Rest"].ToString()));

                    int inMax = dictRecovery.Keys.Max();
                    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                    int intNoofYears = objRepaymentStructure.FunPubGetNoofYearsFromTenure(ViewState["TenureType"].ToString(), ViewState["Tenure"].ToString());

                    if (inMax != 4)
                    {
                        if (inMax != intNoofYears)
                        {
                            Utility.FunShowAlertMsg(this, "Tenure and Recovery Pattern are not matching");
                            ddlROIRuleList.SelectedValue = strRoirValue;
                            if (strRoirValue != "0" && strRoirValue != string.Empty)
                                divROIRuleInfo.Visible = true;
                            else
                                divROIRuleInfo.Visible = false;
                            return;
                        }

                    }
                    else
                    {
                        if (intNoofYears < 4)
                        {
                            Utility.FunShowAlertMsg(this, "Tenure and Recovery Pattern are not matching");
                            ddlROIRuleList.SelectedValue = strRoirValue;
                            if (strRoirValue != "0" && strRoirValue != string.Empty)
                                divROIRuleInfo.Visible = true;
                            else
                                divROIRuleInfo.Visible = false;
                            return;
                        }
                    }
                    break;

            }

            //Ol realted changes on 29-7-2011.
            if (txtResidualvalue.Text.Length > 0 || txtResidualAmt_Cashflow.Text.Length > 0 || txtResidualValue_Cashflow.Text.Length > 0)
            {
                if (ObjDTROI.Rows[0]["Residual_Value"].ToString() != "1")
                {
                    ddlROIRuleList.SelectedValue = strRoirValue;
                    Utility.FunShowAlertMsg(this, "Residual value is given.choose the ROI rule with residual value");
                    if (strRoirValue != "0" && strRoirValue != string.Empty)
                        divROIRuleInfo.Visible = true;
                    else
                        divROIRuleInfo.Visible = false;
                    return;
                }
            }

            FunPriLoadROIDetails(ObjDTROI);
            TabContainerER.ActiveTabIndex = 2;
            hdnROIRule.Value = ddlROIRuleList.SelectedValue;
            if (hdnROIRule.Value != "")
            {
                FunctionClearCashFlowRel();
                //Making null to handle new ROI Rule Card and make Flate rate For IRR given as null
                ViewState["decRate"] = null;
            }
            funPrisetratemaxlength();
            ddlPaymentRuleList.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvEnquiryResponse.ErrorMessage = "Unable to load ROI Rule Details";
            cvEnquiryResponse.IsValid = false;
            divROIRuleInfo.Visible = false;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }



    protected void ddlPaymentRuleList_SelectedIndexChanged(object sender, EventArgs e)
    {
        /* if (ddlPaymentRuleList.SelectedIndex == 0)
         {
             // FunPriToggleROIControls(false);
             gvPaymentRuleDetails.DataSource = null;
             gvPaymentRuleDetails.DataBind();
             div8.Visible = false;
         }
         btnFetchPayment.Focus();*/
    }

    protected void btnFetchPayment_Click(object sender, EventArgs e)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            if (ddlPaymentRuleList.SelectedIndex == 0)
            {
                gvPaymentRuleDetails.DataSource = null;
                gvPaymentRuleDetails.DataBind();
                div8.Visible = false;
                if (ddlROIRuleList.SelectedIndex > 0)
                    divROIRuleInfo.Visible = true;
                else
                    divROIRuleInfo.Visible = false;
                //Utility.FunShowAlertMsg(this, "Select the Payment Rule");
                return;
            }
            div8.Visible = true;
            //divROIRuleInfo.Visible = true;
            DataTable ObjDTPayment = new DataTable();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 45;
            ObjStatus.Param1 = ddlPaymentRuleList.SelectedValue;
            ObjDTPayment = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            FunPriShowPaymentDetails(ObjDTPayment);
            TabContainerER.ActiveTabIndex = 2;
            hdnPayment.Value = ddlPaymentRuleList.SelectedValue;
            if (hdnPayment.Value != "")
            {
                FunctionClearCashFlowRel();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvEnquiryResponse.ErrorMessage = "Unable to load payment rule card Details";
            cvEnquiryResponse.IsValid = false;
            div8.Visible = true;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }



    protected void ddlEntityName_InFlowFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;
            Label lblHeading = gvInflow.HeaderRow.FindControl("lblHeading") as Label;
            //Procparam = new Dictionary<string, string>();
            if (ddlEntityName_InFlowFrom.SelectedItem.Text.ToUpper() == "CUSTOMER")
            {
                if (txtCustomerCode.Text != string.Empty) //SelectedIndex > 0)
                {
                    ddlEntityName_InFlow.Items.Clear();
                    ListItem lstItem = new ListItem(txtCustomerCode.Text + "-" + txtCustomerName.Text, ViewState["Customer_ID"].ToString());
                    ddlEntityName_InFlow.Items.Add(lstItem);

                }
                else
                {
                    ddlEntityName_InFlowFrom.SelectedIndex = 1;
                }

            }
            else
            {
                ddlEntityName_InFlow.Items.Clear();
                lblHeading.Text = "Entity Name";
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Option", "11");
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                ddlEntityName_InFlow.BindDataTable(SPNames.S3G_ORG_GetPricing_List, Procparam, true, new string[] { "Entity_ID", "Entity_Code", "Entity_Name" });
            }
            ddlEntityName_InFlow.Focus();
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }

    }
    protected void ddlOutflowDesc_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlPaymentto_OutFlow = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
            DropDownList ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            ddlPaymentto_OutFlow.SelectedIndex = -1;
            ddlEntityName_InFlow.SelectedIndex = -1;
            ddlPaymentto_OutFlow.Focus();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_Main.ErrorMessage = "Unable to load Enity/Customer";
            cv_Main.IsValid = false;
        }

    }
    protected void ddlPaymentto_OutFlow_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlEntityName_InFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            DropDownList ddlPaymentto_OutFlow = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
            Label lblHeading = gvOutFlow.HeaderRow.FindControl("lblHeading") as Label;
            Procparam = new Dictionary<string, string>();
            DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            string[] strArrayIds = ddlOutflowDesc.SelectedValue.Split(',');
            //    Procparam = new Dictionary<string, string>();
            if (ddlPaymentto_OutFlow.SelectedItem.Text.ToUpper() == "CUSTOMER")
            {
                //Procparam.Add("@Option", "1");
                //Procparam.Add("@Company_ID", intCompanyId.ToString());
                //ddlEntityName_InFlow.BindDataTable(SPNames.S3G_ORG_GetPricing_List, Procparam, false, new string[] { "Customer_ID", "Customer_Code", "Customer_Name" });
                //lblHeading.Text = "Customer Name";
                if (txtCustomerCode.Text != string.Empty) //SelectedIndex > 0)
                {
                    ddlEntityName_InFlow.Items.Clear();
                    ListItem lstItem = new ListItem(txtCustomerCode.Text + "-" + txtCustomerName.Text, ViewState["Customer_ID"].ToString());
                    ddlEntityName_InFlow.Items.Add(lstItem);
                }
                else
                {
                    ddlPaymentto_OutFlow.SelectedIndex = 1;
                }

            }
            else
            {
                // FunPriBindCashFlowDetails();
                lblHeading.Text = "Entity Name";
                Procparam.Add("@Option", "11");
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                if (strArrayIds.Length >= 4)
                {
                    if (strArrayIds[4].ToString() == "41")
                    {
                        Procparam.Add("@ID", "1");
                    }
                }
                ddlEntityName_InFlow.BindDataTable(SPNames.S3G_ORG_GetPricing_List, Procparam, true, new string[] { "Entity_ID", "Entity_Code", "Entity_Name" });
                ddlEntityName_InFlow.Focus();

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_Main.ErrorMessage = "Unable to load Enity/Customer Code";
            cv_Main.IsValid = false;
        }
    }

    #endregion


    #region Grid Events


    protected void gvInflow_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridInflow1 = e.Row.FindControl("txtDate_GridInflow") as TextBox;
                txtDate_GridInflow1.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_InflowDate1 = e.Row.FindControl("CalendarExtenderSD_InflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_InflowDate1.Format = strDateFormat;
            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }


    protected void gvOutFlow_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridOutflow = e.Row.FindControl("txtDate_GridOutflow") as TextBox;
                txtDate_GridOutflow.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_OutflowDate = e.Row.FindControl("CalendarExtenderSD_OutflowDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_OutflowDate.Format = strDateFormat;
            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }

    }
    protected void grvEnquiryUpdation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblEnquiryDate = (Label)e.Row.FindControl("lblEnquiryDate");
                if (lblEnquiryDate != null)
                {
                    if (!string.IsNullOrEmpty(lblEnquiryDate.Text))
                        lblEnquiryDate.Text = DateTime.Parse(lblEnquiryDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                }
            }

        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }
    protected void grvEnquiryUpdation_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridViewRow objGridRow = grvEnquiryUpdation.Rows[e.NewEditIndex];
            Label lblEnquiryUpdationId = (Label)objGridRow.FindControl("lblID");
            hdnTenure.Value = ((Label)objGridRow.FindControl("lblEnquiryTenure")) != null ? ((Label)objGridRow.FindControl("lblEnquiryTenure")).Text : "0";
            intEnquiryUpdationId = Convert.ToInt32(lblEnquiryUpdationId.Text);
            FunPriLoadEnquiryDetails(Convert.ToString(intEnquiryUpdationId));


            FunPriBindCashFlowDetails();
            FunPriLoadPaymentDropDown();
            FunPriBindRepaymentDetails("0");

            FunPriBindAlertDetails();

            FunPriBindFollowupDetails();
            // TabContainerER.ActiveTab = TabContainerER.Tabs[1];
            TabContainerER.ActiveTabIndex = 1;
            gvPaymentRuleDetails.DataSource = null;
            gvPaymentRuleDetails.DataBind();
            divROIRuleInfo.Visible = false;
            div8.Visible = false;
            //FunPriClearROIValues();
            RFVresidualvalue.Enabled = false;
            hdnROIRule.Value = "";
            hdnPayment.Value = "";
            ddlResponse.Focus();

        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void gvInflow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtCashFlow = (DataTable)ViewState["DtCashFlow"];
            if (DtCashFlow.Rows.Count > 0)
            {
                DtCashFlow.Rows.RemoveAt(e.RowIndex);

                if (DtCashFlow.Rows.Count == 0)
                {
                    // FunPriBindCashFlowDetails();
                    ViewState["DtCashFlow"] = DtCashFlow;
                    gvInflow.Rows[0].Cells.Clear();
                    gvInflow.Rows[0].Visible = false;
                }
                else
                {
                    gvInflow.DataSource = DtCashFlow;
                    gvInflow.DataBind();
                    FillDataFrom_ViewState_CashInflow();
                }
            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void gvOutFlow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
            if (DtCashFlowOut.Rows.Count > 0)
            {
                DtCashFlowOut.Rows.RemoveAt(e.RowIndex);

                if (DtCashFlowOut.Rows.Count == 0)
                {
                    ViewState["DtCashFlowOut"] = DtCashFlowOut;
                    gvOutFlow.Rows[0].Cells.Clear();
                    gvOutFlow.Rows[0].Visible = false;

                    //FunPriBindCashFlowDetails();
                }
                else
                {
                    gvOutFlow.DataSource = DtCashFlowOut;
                    gvOutFlow.DataBind();
                    FillDataFrom_ViewState_CashOutflow();
                }
            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void gvRepaymentDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            if (DtRepayGrid.Rows.Count > 0)
            {
                LessingSummaryAmount(e.RowIndex);
                DtRepayGrid.Rows.RemoveAt(e.RowIndex);

                if (DtRepayGrid.Rows.Count == 0)
                {
                    ViewState["DtRepayGrid"] = DtRepayGrid;
                    gvRepaymentDetails.Rows[0].Cells.Clear();
                    gvRepaymentDetails.Rows[0].Visible = false;
                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    txtFromInstallment_RepayTab1_upd.Text = "1";
                    //FunPriBindRepaymentDetails("");
                }
                else
                {
                    intSerialNo = 0;
                    gvRepaymentDetails.DataSource = DtRepayGrid;
                    gvRepaymentDetails.DataBind();
                    FunPriBindRepaymentCashflowDetails();

                    TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
                    Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
                    /*TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
                    Label lblTODate_ReapyTab_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblTODate_ReapyTab");
                    DateTime dtTodate = Utility.StringToDate(lblTODate_ReapyTab_Upd.Text);
                    DateTime dtNextFromdate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedItem.Text, dtTodate);
                    txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);*/
                    FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
                    if (ddl_Repayment_Mode.SelectedValue != "2")
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
                    }

                }
                FunPriIRRReset();
                grvRepayStructure.DataSource = null;
                grvRepayStructure.DataBind();
                ViewState["RepayStructure"] = null;
            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void gvRepaymentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                intSerialNo = 0;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                intSerialNo += 1;
                e.Row.Cells[0].Text = intSerialNo.ToString();
            }

        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void gvRepaymentDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                TextBox txtfromdate_RepayTab = e.Row.FindControl("txtfromdate_RepayTab") as TextBox;
                txtfromdate_RepayTab.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab = e.Row.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_fromdate_RepayTab.Format = DateFormate; //ObjS3GSession.ProDateFormatRW;



            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender calext_FromDate = e.Row.FindControl("calext_FromDate") as AjaxControlToolkit.CalendarExtender;
                calext_FromDate.Format = DateFormate;// ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }


    protected void gvAlert_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtAlertDetails = (DataTable)ViewState["DtAlertDetails"];
            if (DtAlertDetails.Rows.Count > 0)
            {
                DtAlertDetails.Rows.RemoveAt(e.RowIndex);

                if (DtAlertDetails.Rows.Count == 0)
                {
                    ViewState["DtAlertDetails"] = DtAlertDetails;
                    gvAlert.Rows[0].Cells.Clear();
                    gvAlert.Rows[0].Visible = false;

                    //  FunPriBindAlertDetails();
                }
                else
                {
                    gvAlert.DataSource = DtAlertDetails;
                    gvAlert.DataBind();
                    FillDataFrom_ViewState();
                }
            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void gvFollowUp_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtDate_GridFollowup = e.Row.FindControl("txtDate_GridFollowup") as TextBox;
                txtDate_GridFollowup.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupDate = e.Row.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_FollowupDate.Format = strDateFormat;

            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void gvFollowUp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtFollowUp = (DataTable)ViewState["DtFollowUp"];
            if (DtFollowUp.Rows.Count > 0)
            {
                DtFollowUp.Rows.RemoveAt(e.RowIndex);

                if (DtFollowUp.Rows.Count == 0)
                {
                    ViewState["DtFollowUp"] = DtFollowUp;
                    gvFollowUp.Rows[0].Cells.Clear();
                    gvFollowUp.Rows[0].Visible = false;

                    //FunPriBindFollowupDetails();
                }
                else
                {
                    gvFollowUp.DataSource = DtFollowUp;
                    gvFollowUp.DataBind();
                    FillDataFrom_ViewState_FollowUp();
                }
            }
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    #endregion

    #region ToggleMethods

    private void FunPriToggleROIControls(bool blnIsVisible)
    {
        try
        {
            tr1.Visible = tr11.Visible =
            tr2.Visible = tr12.Visible =
            tr3.Visible = tr13.Visible =
            tr4.Visible = tr14.Visible =
            tr5.Visible = tr15.Visible =
            tr6.Visible = tr16.Visible =
            tr7.Visible = tr17.Visible =
            tr8.Visible = tr18.Visible =
            tr9.Visible = tr19.Visible =
            tr10.Visible = tr20.Visible =
            divROIRuleInfo.Visible =
            blnIsVisible;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriToggleROIValues(bool blnIsDisabled)
    {
        try
        {
            tr1.Disabled = tr11.Disabled =
             tr2.Disabled = tr12.Disabled =
             tr3.Disabled = tr13.Disabled =
             tr4.Disabled = tr14.Disabled =
             tr5.Disabled = tr15.Disabled =
             tr6.Disabled = tr16.Disabled =
             tr7.Disabled = tr17.Disabled =
             tr8.Disabled = tr18.Disabled =
             tr9.Disabled = tr19.Disabled =
             tr10.Disabled = tr20.Disabled = blnIsDisabled;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriUpdateROIRuleDecRate()//Added on 3/11/2011 by saran for UAT raised mail modify mode not allowing to save forr IRR to flat rate
    {
        DataTable ObjDTROI = new DataTable(); ;
        ObjDTROI = (DataTable)ViewState["ROIDetails"];
        decimal decRate = 0;
        switch (ddl_Return_Pattern.SelectedValue)
        {

            case "1":
                decRate = Convert.ToDecimal(txt_Rate.Text);
                break;
            case "2":
                //ObjCommonBusLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, Convert.ToDecimal(txtFacilityAmt.Text), Convert.ToDouble(9.6365), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decRate, Convert.ToDecimal(10.05), decPLR);
                if (ViewState["decRate"] != null)
                {
                    decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
                }//Hard Coded for testing IRR
                break;
        }
        ObjDTROI.Rows[0]["IRR_Rate"] = decRate;
        ObjDTROI.Rows[0].AcceptChanges();
        ViewState["ROIRules"] = ObjDTROI;
    }


    private void FunPriUpdateROIRule()
    {
        DataTable ObjDTROI = new DataTable(); ;
        ObjDTROI = (DataTable)ViewState["ROIDetails"];
        ObjDTROI.Rows[0]["Serial_Number"] = txt_Serial_Number.Text == "" ? 0 : Convert.ToInt64(txt_Serial_Number.Text);
        ObjDTROI.Rows[0]["Model_Description"] = txt_Model_Description.Text;
        ObjDTROI.Rows[0]["Rate_Type"] = ddl_Rate_Type.SelectedValue;
        ObjDTROI.Rows[0]["ROI_Rule_Number"] = txt_ROI_Rule_Number.Text;
        ObjDTROI.Rows[0]["Return_Pattern"] = ddl_Return_Pattern.SelectedValue;
        ObjDTROI.Rows[0]["Time_Value"] = ddl_Time_Value.SelectedValue;
        ObjDTROI.Rows[0]["Frequency"] = ddl_Frequency.SelectedValue;
        ObjDTROI.Rows[0]["Repayment_Mode"] = ddl_Repayment_Mode.SelectedValue;
        ObjDTROI.Rows[0]["Rate"] = txt_Rate.Text;
        ObjDTROI.Rows[0]["IRR_Rest"] = ddl_IRR_Rest.SelectedValue;
        ObjDTROI.Rows[0]["Interest_Calculation"] = ddl_Interest_Calculation.SelectedValue;
        ObjDTROI.Rows[0]["Interest_Levy"] = ddl_Interest_Levy.SelectedValue;
        if (txt_Recovery_Pattern_Year1.Text != string.Empty)
            ObjDTROI.Rows[0]["Recovery_Pattern_Year1"] = txt_Recovery_Pattern_Year1.Text;
        else
            ObjDTROI.Rows[0]["Recovery_Pattern_Year1"] = "0.00";
        if (txt_Recovery_Pattern_Year2.Text != string.Empty)
            ObjDTROI.Rows[0]["Recovery_Pattern_Year2"] = txt_Recovery_Pattern_Year2.Text;
        else
            ObjDTROI.Rows[0]["Recovery_Pattern_Year2"] = "0.00";
        if (txt_Recovery_Pattern_Year3.Text != string.Empty)
            ObjDTROI.Rows[0]["Recovery_Pattern_Year3"] = txt_Recovery_Pattern_Year3.Text;
        else
            ObjDTROI.Rows[0]["Recovery_Pattern_Year3"] = "0.00";
        if (txt_Recovery_Pattern_Rest.Text != string.Empty)
            ObjDTROI.Rows[0]["Recovery_Pattern_Rest"] = txt_Recovery_Pattern_Rest.Text;
        else
            ObjDTROI.Rows[0]["Recovery_Pattern_Rest"] = "0.00";

        ObjDTROI.Rows[0]["Insurance"] = ddl_Insurance.SelectedValue;
        ObjDTROI.Rows[0]["Residual_Value"] = ddl_Residual_Value.SelectedValue;
        ObjDTROI.Rows[0]["Margin"] = ddl_Margin.SelectedValue;
        ObjDTROI.Rows[0]["Margin_Percentage"] = txt_Margin_Percentage.Text == "" ? 0 : Convert.ToDecimal(txt_Margin_Percentage.Text);
        ObjDTROI.Rows[0].AcceptChanges();
        ViewState["ROIDetails"] = ObjDTROI;
    }

    #endregion

    #region Button Events

    protected void btnGenerateRepay_Click(object sender, EventArgs e)
    {
        try
        {
            txtAccountIRR_Repay.Text = "";
            txtBusinessIRR_Repay.Text = "";
            txtCompanyIRR_Repay.Text = "";
            if (ViewState["DtCashFlow"] != null)
            {
                DataTable DtCashFlow = (DataTable)ViewState["DtCashFlow"];
                if (DtCashFlow.Rows.Count > 0)
                {
                    DataRow[] drUMFC = null;
                    if (DtCashFlow.Columns.Contains("CashFlow_ID"))
                    {
                        drUMFC = DtCashFlow.Select("CashFlow_ID = 34");
                    }
                    else
                    {
                        drUMFC = DtCashFlow.Select("CashFlow_Flag_ID = 34");
                    }
                    if (drUMFC.Length > 0)
                    {
                        drUMFC[0].Delete();
                        DtCashFlow.AcceptChanges();
                        ViewState["DtCashFlow"] = DtCashFlow;
                        if (DtCashFlow.Rows.Count > 0)
                        {
                            FillDataFrom_ViewState_CashInflow();
                        }
                    }

                }
            }
            FunPriGenerateRepayment(DateTime.Now);
            /*UMFC has been calculated automatically for other than Product & TermLoan Return Pattern 
            (Also applicable to HP,FL,LN,TE,TL) Updated on 28th Oct 2010*/
            string Lob = "";
            if (ddlLOBAssign.SelectedIndex > 0)
            {
                Lob = ddlLOBAssign.SelectedItem.Text.Split('-')[0].ToString().Trim();
            }
            else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
            {
                Lob = txtLOB.Text.Split('-')[0].ToString().Trim();
            }
            if (!Lob.StartsWith("OL") && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
            {
                FunPriInsertUMFC();
                /* DtCashFlow = (DataTable)ViewState["DtCashFlow"];

                 DataRow dr = DtCashFlow.NewRow();
                 DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["Date"] };
                 dr["Date"] = DateTime.Now.Date.ToString(strDateFormat);
                 string[] strArrayIds = ddlInflowDesc1.SelectedValue.Split(',');
                 dr["CashInFlowID"] = strArrayIds[0];
                 dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
                 dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
                 dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
                 dr["CashFlow_Flag_ID"] = strArrayIds[4];
                 dr["CashInFlow"] = ddlInflowDesc1.SelectedItem;
                 dr["EntityID"] = S3GCustomerAddress1.CustomerId.ToString();
                 dr["Entity"] = S3GCustomerAddress1.CustomerName;
                 dr["InflowFromId"] = "144";
                 dr["InflowFrom"] = "Customer";
                 dr["Amount"] = FunPriGetInterestAmount().ToString();
                 DtCashFlow.Rows.Add(dr);

                 gvInflow.DataSource = DtCashFlow;
                 gvInflow.DataBind();

                 ViewState["DtCashFlow"] = DtCashFlow;
                 FillDataFrom_ViewState_CashInflow();*/

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);

            cv_TabRepayment.ErrorMessage = ex.Message.ToString();
            cv_TabRepayment.IsValid = false;
        }
    }

    private void FunPriInsertUMFC()
    {
        try
        {
            string LOBID = "";
            if (ddlLOBAssign.SelectedIndex > 0)
            {
                LOBID = ddlLOBAssign.SelectedValue;
            }
            else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
            {
                LOBID = ViewState["cashlobid"].ToString();
            }
            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyId.ToString());
            if (LOBID != string.Empty)
                objParameters.Add("@LobId", LOBID);

            DataSet dsUMFC = Utility.GetDataset("s3g_org_loadInflowLov", objParameters);
            DtCashFlow = (DataTable)ViewState["DtCashFlow"];
            //DataSet dsUMFC = (DataSet)ViewState["InflowDDL"];
            DataRow dr = DtCashFlow.NewRow();
            DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["Date"] };
            dr["Date"] = DateTime.Today.ToString();
            string[] strArrayIds = null;
            string cashflowdesc = "";
            foreach (DataRow drOut in dsUMFC.Tables[2].Rows)
            {
                string[] strCashflow = drOut["CashFlow_ID"].ToString().Split(',');
                if (strCashflow[4].ToString() == "34")
                {
                    strArrayIds = strCashflow;
                    cashflowdesc = drOut["CashFlow_Description"].ToString();
                }
            }
            if (strArrayIds == null)
            {
                Utility.FunShowAlertMsg(this, "Define the Cashflow for UMFC in Cashflow Master");
                return;
            }
            dr["CashInFlowID"] = strArrayIds[0];
            dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
            dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
            dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
            dr["CashFlow_Flag_ID"] = strArrayIds[4];
            dr["CashInFlow"] = cashflowdesc;
            dr["EntityID"] = ViewState["Customer_ID"].ToString();
            dr["Entity"] = S3GCustomerAddress1.CustomerName;
            dr["InflowFromId"] = "144";
            dr["InflowFrom"] = "Customer";
            if (ddl_Repayment_Mode.SelectedValue == "2")
            {

                if (ddl_Return_Pattern.SelectedIndex > 0 && Convert.ToInt32(ddl_Return_Pattern.SelectedValue) > 2)
                {
                    dr["Amount"] = FunPriGetInterestAmount().ToString();
                }
                else
                {
                    dr["Amount"] = FunPriGetStructureAdhocInterestAmount().ToString();
                }

            }
            else
            {
                dr["Amount"] = FunPriGetInterestAmount().ToString();
            }

            DtCashFlow.Rows.Add(dr);

            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();

            ViewState["DtCashFlow"] = DtCashFlow;
            //FunPriGenerateNewInflow();
            FillDataFrom_ViewState_CashInflow();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriGenerateRepayment(DateTime dtStartDate)
    {

        ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
        DataSet dsRepayGrid = new DataSet();
        DataTable dtMoratorium = null;
        string LOBID = "";
        string Lob = "";
        int tenure = Convert.ToInt32(ViewState["Tenure"]);

        if (ddlLOBAssign.SelectedIndex > 0)
        {
            LOBID = ddlLOBAssign.SelectedValue;
            Lob = ddlLOBAssign.SelectedItem.Text.Split('-')[0].ToString().Trim();
        }
        else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
        {
            LOBID = ViewState["cashlobid"].ToString();
            Lob = txtLOB.Text.Split('-')[0].ToString().Trim();
        }
        if (objRepaymentStructure.FunPubGetCashFlowDetails(intCompanyId, Convert.ToInt32(LOBID)).Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this, "Define cashflow for Installment Payment");
            return;
        }
        Dictionary<string, string> objMethodParameters = new Dictionary<string, string>();
        if (ddlLOBAssign.SelectedIndex > 0)
            objMethodParameters.Add("LOB", ddlLOBAssign.SelectedItem.Text);
        else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
            objMethodParameters.Add("LOB", txtLOB.Text);
        objMethodParameters.Add("Tenure", tenure.ToString());
        objMethodParameters.Add("TenureType", ViewState["TenureType"].ToString());
        //For OL related changed on 25/7/2011.
        string strFinAmount = GetLOBBasedFinAmt();
        objMethodParameters.Add("FinanceAmount", strFinAmount);
        objMethodParameters.Add("ReturnPattern", ddl_Return_Pattern.SelectedValue);
        objMethodParameters.Add("MarginPercentage", txtMarginMoneyPer_Cashflow.Text);
        objMethodParameters.Add("Rate", txt_Rate.Text);
        objMethodParameters.Add("TimeValue", ddl_Time_Value.SelectedValue);
        objMethodParameters.Add("RepaymentMode", ddl_Repayment_Mode.SelectedValue);
        objMethodParameters.Add("CompanyId", intCompanyId.ToString());
        objMethodParameters.Add("LobId", LOBID);
        //objMethodParameters.Add("DocumentDate", DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat));
        objMethodParameters.Add("DocumentDate", DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat));
        objMethodParameters.Add("Frequency", ddl_Frequency.SelectedValue);
        objMethodParameters.Add("RecoveryYear1", txt_Recovery_Pattern_Year1.Text);
        objMethodParameters.Add("RecoveryYear2", txt_Recovery_Pattern_Year2.Text);
        objMethodParameters.Add("RecoveryYear3", txt_Recovery_Pattern_Year3.Text);

        objMethodParameters.Add("RecoveryYear4", txt_Recovery_Pattern_Rest.Text);
        if (ViewState["hdnRoundOff"] != null)
        {
            if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                objMethodParameters.Add("Roundoff", ViewState["hdnRoundOff"].ToString());
            else
                objMethodParameters.Add("Roundoff", "2");
        }
        else
        {
            objMethodParameters.Add("Roundoff", "2");
        }

        //Code added for Ol related issue on 25-7-2011.
        if (Lob.ToUpper() == "OL")
        {
            //if (ViewState["OLExistingAsset"] != null && Convert.ToInt32(ViewState["OLExistingAsset"]) > 0)
            //{
            DataTable dtoutflw = (DataTable)ViewState["DtCashFlowOut"];
            //DataTable dtoutflwdesc = (DataTable)ViewState["CashOutflowList"];
            if (dtoutflw.Rows.Count == 0)
            {
                DataRow drOutflow = dtoutflw.NewRow();
                drOutflow["Date"] = Utility.StringToDate(DateTime.Now.ToString(strDateFormat));
                drOutflow["CashOutFlow"] = "Fin amount";
                drOutflow["EntityID"] = ViewState["Customer_ID"].ToString();
                drOutflow["Entity"] = S3GCustomerAddress1.CustomerName;
                drOutflow["OutflowFromId"] = "144";
                drOutflow["OutflowFrom"] = "Customer";
                drOutflow["Amount"] = strFinAmount;
                drOutflow["CashOutFlowID"] = "-1";
                drOutflow["Accounting_IRR"] = true;
                drOutflow["Business_IRR"] = true;
                drOutflow["Company_IRR"] = true;
                drOutflow["CashFlow_Flag_ID"] = "41";
                dtoutflw.Rows.Add(drOutflow);
            }
            ViewState["DtCashFlowOut"] = dtoutflw;
            //}
        }

        // DtRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(Utility.StringToDate(DateTime.Parse(dtStartDate.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat)), objMethodParameters);
        if (ddl_Return_Pattern.SelectedValue == "2")
        {
            if (txtResidualAmt_Cashflow.Text.Trim() != "" && txtResidualAmt_Cashflow.Text.Trim() != "0")
            {
                objMethodParameters.Add("decResidualAmount", txtResidualAmt_Cashflow.Text);
            }
            if (txtResidualValue_Cashflow.Text.Trim() != "" && txtResidualValue_Cashflow.Text.Trim() != "0")
            {
                objMethodParameters.Add("decResidualValue", txtResidualValue_Cashflow.Text);
            }
            switch (ddl_IRR_Rest.SelectedValue)
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
            dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(Utility.StringToDate(DateTime.Parse(dtStartDate.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat)), (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"], objMethodParameters, dtMoratorium, out decRateOut);
            ViewState["decRate"] = Math.Round(Convert.ToDouble(decRateOut), 4);
        }
        else
        {
            dsRepayGrid = objRepaymentStructure.FunPubGenerateRepaymentSchedule(Utility.StringToDate(DateTime.Parse(dtStartDate.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat)), objMethodParameters, dtMoratorium);
        }

        decimal decFinAmount = objRepaymentStructure.FunPubGetAmountFinanced(strFinAmount, txtMarginMoneyPer_Cashflow.Text);

        if (dsRepayGrid == null)
        {
            /* It Calculates and displays the Repayment Details for ST-ADHOC */

            FunPriShowRepaymetDetails(decFinAmount + FunPriGetStructureAdhocInterestAmount());
            return;
        }

        if (dsRepayGrid.Tables[0].Rows.Count > 0)
        {
            gvRepaymentDetails.DataSource = dsRepayGrid.Tables[0];
            gvRepaymentDetails.DataBind();
            ViewState["DtRepayGrid"] = dsRepayGrid.Tables[0];
            if (ddl_Rate_Type.SelectedItem.Text == "Floating")
            {
                ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
                ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
            }
            else
            {
                ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
                ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
            }
            // FunPriShowRepaymetDetails((decimal)DtRepayGrid.Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
            FunPriCalculateIRR();
            btnReset.Enabled = false;
            FunPriGenerateNewRepayment();
            FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
        }
        else
        {

            btnReset.Enabled = true;

        }
        strFinAmount = GetLOBBasedFinAmt();

        if (dsRepayGrid.Tables[0].Rows.Count > 0)
        {
            FunPriShowRepaymetDetails((decimal)dsRepayGrid.Tables[0].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));
        }
        else
        {
            //FunPriShowRepaymetDetails(decFinAmount + FunPriGetInterestAmount());
            /* It Calculates and displays the Repayment Details for ST-ADHOC */
            FunPriShowRepaymetDetails(decFinAmount + FunPriGetStructureAdhocInterestAmount());
        }
    }

    private string GetLOBBasedFinAmt()
    {
        /* string LOB = "";
         string strFinAmount = "";
         if (ddlLOBAssign.SelectedIndex > 0)
             LOB = ddlLOBAssign.SelectedItem.Text.Split('-')[0].ToString();
         else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
             LOB = txtLOB.Text.Split('-')[0].ToString();

         if (!string.IsNullOrEmpty(LOB))
         {
             if (LOB.Trim().ToLower() == "ol")
             {
                 strFinAmount = txtAssetValue.Text;
             }
             else
             {
                 strFinAmount = txtFinanceAmount.Text;
             }
         }
         else
         {
             strFinAmount = txtFinanceAmount.Text;
         }*/
        string strFinAmount = "";
        strFinAmount = txtFinanceAmount.Text;
        return strFinAmount;
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {  //wf Cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else
                Response.Redirect(strRedirectPage);
        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        try
        {

            ClearForm();

        }
        catch (Exception ex)
        {
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }



    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        ObjEnquiryResponseClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {

            if (Convert.ToInt64(ddlResponse.SelectedValue) > 0)
            {
                if (ddlResponse.SelectedValue == "190" || ddlResponse.SelectedValue == "191" || ddlResponse.SelectedValue == "192" || ddlResponse.SelectedValue == "233")
                {
                    S3GBusEntity.EnquiryResponse.EnquiryResponseEntity ObjEnquiryResponseEntity = new S3GBusEntity.EnquiryResponse.EnquiryResponseEntity();
                    if (intEnquiryResponseId > 0)
                    {
                        ObjEnquiryResponseEntity.EnquiryResponse_ID = intEnquiryResponseId;
                        if (ViewState["statusID"] != null)
                        {
                            ObjEnquiryResponseEntity.Status = ViewState["statusID"].ToString();
                        }
                        ObjEnquiryResponseEntity.EnquiryResponseDetailId = Convert.ToInt32(ddlResponse.SelectedValue);
                        ObjEnquiryResponseEntity.Responded_By = intUserId;
                        // Modified By R. Manikandan
                        // To System not saving ROI Rules and all XML
                        FunPriInsertEnquiryResponse();
                    }
                    else
                    {
                        ObjEnquiryResponseEntity.EnquiryResponse_ID = Convert.ToInt32(ViewState["ResID"]);
                        ObjEnquiryResponseEntity.Enquiry_No = txtEnquiryNo.Text;
                        ObjEnquiryResponseEntity.Company_ID = intCompanyId;
                        ObjEnquiryResponseEntity.Responded_By = intUserId;
                        ObjEnquiryResponseEntity.EnquiryResponseDetailId = Convert.ToInt32(ddlResponse.SelectedValue);
                        ObjEnquiryResponseEntity.Status = ViewState["statusID"].ToString();
                        if (!string.IsNullOrEmpty(txtFinanceAmount.Text))
                            ObjEnquiryResponseEntity.Finance_Amount_Sought = Convert.ToDecimal(txtFinanceAmount.Text);
                        else
                            ObjEnquiryResponseEntity.Finance_Amount_Sought = Convert.ToDecimal(0);

                        if (!string.IsNullOrEmpty(txtResidualvalue.Text))
                            ObjEnquiryResponseEntity.Residual_Margin_Amount = Convert.ToDecimal(txtResidualvalue.Text);
                        else
                            ObjEnquiryResponseEntity.Residual_Margin_Amount = Convert.ToDecimal(0);
                        if (ddlLOBAssign.SelectedIndex > 0)
                            ObjEnquiryResponseEntity.LOB_ID = Convert.ToInt32(ddlLOBAssign.SelectedValue);
                        else
                            ObjEnquiryResponseEntity.LOB_ID = Convert.ToInt32(ViewState["cashlobid"].ToString());
                        if (ddlProductAssign.SelectedIndex > 0)
                            ObjEnquiryResponseEntity.Product_ID = Convert.ToInt32(ddlProductAssign.SelectedValue);
                        else
                            ObjEnquiryResponseEntity.Product_ID = Convert.ToInt32(ViewState["cashproductid"].ToString());
                        if (ddlBranchAssign.SelectedIndex > 0)
                            ObjEnquiryResponseEntity.Branch_ID = Convert.ToInt32(ddlBranchAssign.SelectedValue);
                        else
                            ObjEnquiryResponseEntity.Branch_ID = Convert.ToInt32(ViewState["cashbranchid"].ToString());
                        if (!string.IsNullOrEmpty(ViewState["WorkflowId"].ToString()))
                            ObjEnquiryResponseEntity.WorkFlow_Sequence = Convert.ToInt32(ViewState["WorkflowId"].ToString());
                        // Modified By R. Manikandan
                        // To System not saving ROI Rules and all XML
                        FunPriInsertEnquiryResponse();

                    }
                    if (intEnquiryResponseId > 0)
                    {
                        //
                        try
                        {
                            intErrorCode = ObjEnquiryResponseClient.FunPubModifyEnquiryResponse(ObjEnquiryResponseEntity);
                            // Utility.FunShowAlertMsg(this, "update");
                            if (intErrorCode == 0)
                            {
                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here

                                Utility.FunShowAlertMsg(this, "Enquiry Response updated sucessfully");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            StackTrace trace = new StackTrace(ex, true);
                            //Added Line Number for the Exception Error
                            // 1. Solution 1
                            // strLineNumber = "Error @ " + ex.StackTrace.ToString().Split(':').Last().ToString() + e.message;
                            // 2. Soltution 2
                            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                            cv_Main.ErrorMessage = "Error @ " + trace.GetFrame(0).GetMethod().ReflectedType.FullName + "at Line:" + trace.GetFrame(0).GetFileLineNumber();
                            cv_Main.IsValid = false;
                        }
                    }
                    else
                    {
                        try
                        {

                            string strEnquiryResponseId = "";
                            intErrorCode = ObjEnquiryResponseClient.FunPubInsertEnquiryResponse(out strEnquiryResponseId, ObjEnquiryResponseEntity);
                            //Utility.FunShowAlertMsg(this, "save");
                            if (intErrorCode == 0)
                            {
                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here

                                //Utility.FunShowAlertMsg(this, "Enquiry Response created sucessfully");
                                strAlert = "Enquiry " + txtEnquiryNo.Text + "  Responded successfully";
                                strAlert += @"\n\nWould you like to Response one more Enquiry?";
                                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                                strRedirectPageView = "";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);

                                if (isWorkFlowTraveler) // Update the WF as CANCELED
                                {
                                    WorkFlowSession WFValues = new WorkFlowSession();
                                    //int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                                }
                                else
                                {
                                    TabContainerER.ActiveTabIndex = 0;
                                    FunPriLoadEnquiryAssignedDetails();
                                }

                            }
                            else if (intErrorCode == 25)
                            {
                                Utility.FunShowAlertMsg(this, "Selected enquiry is already responded");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                            StackTrace trace = new StackTrace(ex, true);
                            //Added Line Number for the Exception Error
                            // 1. Solution 1
                            // strLineNumber = "Error @ " + ex.StackTrace.ToString().Split(':').Last().ToString() + e.message;
                            // 2. Soltution 2
                            cv_Main.ErrorMessage = "Error @ " + trace.GetFrame(0).GetMethod().ReflectedType.FullName + "at Line:" + trace.GetFrame(0).GetFileLineNumber();
                            cv_Main.IsValid = false;
                        }
                    }
                }

                else
                {
                    try
                    {

                        if (!(Convert.ToInt32(ddlROIRuleList.SelectedValue) > 0))
                        {
                            ScriptManagerAlert("ROI Rule List", " Select the ROI Rule List");
                            TabContainerER.ActiveTabIndex = 2;
                            return;
                        }
                        //if (ViewState["OLExistingAsset"] != null && Convert.ToInt32(ViewState["OLExistingAsset"]) <= 0)
                        string LOB = "";
                        LOB = FunPriGetLOBstring();
                        if (!string.IsNullOrEmpty(LOB))
                        {
                            if (LOB.Split('-')[0].ToString().ToLower().Trim() != "ol")
                            {
                                if (!(Convert.ToInt32(ddlPaymentRuleList.SelectedValue) > 0))
                                {
                                    ScriptManagerAlert("Payment Rule List", " Select the Payment Rule List");
                                    TabContainerER.ActiveTabIndex = 2;
                                    return;
                                }
                                if (((DataTable)ViewState["DtCashFlowOut"]).Rows.Count == 0)
                                {
                                    ScriptManagerAlert("Cash Outflow details", " Add atleast one Outflow details");
                                    TabContainerER.ActiveTabIndex = 2;
                                    return;
                                }

                                //validation for finance amount based on assetvalue(finance amount <= (assetvalue-margin amount(if margin is applicable)))

                                ClsRepaymentStructure objclsRepaymentStructure = new ClsRepaymentStructure();
                                decimal totalfinamount = Convert.ToDecimal(txtAssetValue.Text) - (objclsRepaymentStructure.FunPubGetMarginAmout(txtAssetValue.Text, txtMarginMoneyPer_Cashflow.Text));
                                if (Convert.ToDecimal(txtFinanceAmount.Text) > totalfinamount)
                                {
                                    Utility.FunShowAlertMsg(this, "Finance amount should not be greater than asset value with margin money(" + totalfinamount + ")");
                                    TabContainerER.ActiveTabIndex = 1;
                                    txtFinanceAmount.Focus();
                                    return;
                                }

                                string strLOB = "";
                                if (ddlLOBAssign.SelectedIndex > 0)
                                    strLOB = ddlLOBAssign.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower();
                                else
                                    strLOB = txtLOB.Text.Split('-')[0].ToString().Trim().ToLower();
                                if (strLOB != "ol")
                                {
                                    if (ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "PRODUCT" && ddl_Repayment_Mode.SelectedItem.Text.ToUpper().Trim() != "TERM LOAN")
                                    {
                                        if (ddl_Return_Pattern.SelectedValue == "1" || ddl_Return_Pattern.SelectedValue == "2")
                                        {

                                            decimal decUMFC = (decimal)((DataTable)ViewState["DtCashFlow"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 34");
                                            if (decUMFC != FunPriGetInterestAmount())
                                            {
                                                cvEnquiryResponse.ErrorMessage = "<br/> Correct the following validation(s): <br/><br/> Unmatured Finance Amount in inflow not matching with Interest";
                                                cvEnquiryResponse.IsValid = false;
                                                Utility.FunShowAlertMsg(this, "Unmatured Finance Amount(UMFC) in inflow not matching with Interest");
                                                TabContainerER.ActiveTabIndex = 2;
                                                return;
                                            }
                                        }
                                    }

                                }

                                decimal decToatlFinanceAmt = 0;
                                if (!string.IsNullOrEmpty(Convert.ToString(((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 41"))))
                                    decToatlFinanceAmt = (decimal)((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");
                                else
                                {
                                    cvEnquiryResponse.ErrorMessage = "<br/> Correct the following validation(s): <br/><br/> Payment Cashflow Description not available in Outflow Details";
                                    cvEnquiryResponse.IsValid = false;
                                    Utility.FunShowAlertMsg(this, "Payment Cashflow Description not available in Outflow Details");
                                    return;
                                }
                                if (FunPriGetAmountFinanced() != decToatlFinanceAmt)
                                {
                                    ScriptManagerAlert("Repayment details", "Total amount financed in Cashoutflow should be equal to amount financed with margin money");
                                    TabContainerER.ActiveTabIndex = 2;
                                    return;
                                }
                            }
                        }


                        if (txtAccountIRR_Repay.Text == "" || txtBusinessIRR_Repay.Text == "" || txtCompanyIRR_Repay.Text == "")
                        {
                            FunPriIRRReset();
                            ScriptManagerAlert("Repayment details", "Calculate IRR");
                            cvEnquiryResponse.IsValid = false;
                            cvEnquiryResponse.ErrorMessage = "Recalculate IRR ";
                            return;

                        }
                        //if (((DataTable)ViewState["DtAlertDetails"]).Rows.Count == 0)
                        //{
                        //    ScriptManagerAlert("Alert details", " Add atleast one Alert details");
                        //    TabContainerER.ActiveTabIndex = 4;
                        //    return;
                        //}

                        //if (((DataTable)ViewState["DtFollowUp"]).Rows.Count == 0)
                        //{
                        //    ScriptManagerAlert("FollowUp details", " Add atleast one Followup details");
                        //    TabContainerER.ActiveTabIndex = 5;
                        //    return;
                        //}

                        if (ddlLOBAssign.SelectedIndex > 0 || ddlProductAssign.SelectedIndex > 0 || ddlBranchAssign.SelectedIndex > 0)
                        {
                            int count = 0;
                            int equalcount = 0;
                            if (ddlLOBAssign.SelectedIndex > 0)
                            {
                                count = count + 1;
                            }
                            if (ddlProductAssign.SelectedIndex > 0)
                            {
                                count = count + 1;
                            }
                            if (ddlBranchAssign.SelectedIndex > 0)
                            {
                                count = count + 1;
                            }
                            if (count < 3)
                            {
                                ScriptManagerAlert("Alert details", " Select all the Change To fields");
                                TabContainerER.ActiveTabIndex = 1;
                                return;
                            }
                            else
                            {
                                string[] tempLOB = (ddlLOBAssign.SelectedItem.Text).Split('-');
                                if (txtLOBAssign.Text.StartsWith(tempLOB[0].Trim().ToString()))
                                {
                                    equalcount = equalcount + 1;
                                }
                                string[] tempProduct = (ddlProductAssign.SelectedItem.Text).Split('-');
                                if (txtProductAssign.Text.StartsWith(tempProduct[0].Trim().ToString()))
                                {
                                    equalcount = equalcount + 1;
                                }
                                string[] tempBranch = (ddlBranchAssign.SelectedItem.Text).Split('-');
                                if (txtBranchAssign.Text.StartsWith(tempBranch[0].Trim().ToString()))
                                {
                                    equalcount = equalcount + 1;
                                }
                                if (equalcount == 3)
                                {
                                    ScriptManagerAlert("Alert details", "Both Existing Fields and Change To fields are Same");
                                    TabContainerER.ActiveTabIndex = 1;
                                    return;
                                }
                            }
                        }
                        // Commented By R. Manikandan
                        // To System not saving ROI Rules and all XML
                        //FunPriInsertEnquiryResponse();
                    }
                    catch (Exception ex)
                    {
                        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                        StackTrace trace = new StackTrace(ex, true);
                        //Added Line Number for the Exception Error
                        // 1. Solution 1
                        // strLineNumber = "Error @ " + ex.StackTrace.ToString().Split(':').Last().ToString() + e.message;
                        // 2. Soltution 2
                        cv_Main.ErrorMessage = "Error @ " + trace.GetFrame(0).GetMethod().ReflectedType.FullName + "at Line:" + trace.GetFrame(0).GetFileLineNumber();
                        cv_Main.IsValid = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            StackTrace trace = new StackTrace(ex, true);
            //Console.WriteLine(trace.GetFrame(0).GetMethod().ReflectedType.FullName);
            //Console.WriteLine("Line: " + trace.GetFrame(0).GetFileLineNumber());
            //Console.WriteLine("Column: " + trace.GetFrame(0).GetFileColumnNumber());

            //if (ObjEnquiryResponseClient != null)
            //    if (ObjEnquiryResponseClient.State == CommunicationState.Opened)
            //        ObjEnquiryResponseClient.Close();
            //Added Line Number for the Exception Error
            // 1. Solution 1
            // strLineNumber = "Error @ " + ex.StackTrace.ToString().Split(':').Last().ToString() + e.message;
            // 2. Soltution 2
            cv_Main.ErrorMessage = "Error @ " + ex.Message + "at Line:" + trace.GetFrame(0).GetFileLineNumber();
            cv_Main.IsValid = false;
        }
        finally
        {
            ObjEnquiryResponseClient.Close();
        }
    }

    protected void CashInflow_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtCashFlow = (DataTable)ViewState["DtCashFlow"];

            TextBox txtDate_GridInflow1 = gvInflow.FooterRow.FindControl("txtDate_GridInflow") as TextBox;
            DropDownList ddlInflowDesc1 = gvInflow.FooterRow.FindControl("ddlInflowDesc") as DropDownList;
            TextBox txtAmount_Inflow1 = gvInflow.FooterRow.FindControl("txtAmount_Inflow") as TextBox;
            DropDownList ddlEntityName_InFlow1 = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;

            if (!ISDuplicateData_Inflow(ddlInflowDesc1.SelectedValue, txtDate_GridInflow1.Text))
            {
                DataRow dr = DtCashFlow.NewRow();
                DtCashFlow.PrimaryKey = new DataColumn[] { DtCashFlow.Columns["Date"], DtCashFlow.Columns["CashInFlowID"], DtCashFlow.Columns["InflowFromId"], DtCashFlow.Columns["EntityID"] };
                dr["Date"] = Utility.StringToDate(txtDate_GridInflow1.Text);
                string[] strArrayIds = ddlInflowDesc1.SelectedValue.Split(',');
                dr["CashInFlowID"] = strArrayIds[0];
                dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
                dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
                dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
                dr["CashFlow_Flag_ID"] = strArrayIds[4];
                dr["CashInFlow"] = ddlInflowDesc1.SelectedItem;
                dr["EntityID"] = ddlEntityName_InFlow1.SelectedValue;
                dr["Entity"] = ddlEntityName_InFlow1.SelectedItem;
                dr["InflowFromId"] = ddlEntityName_InFlowFrom.SelectedValue;
                dr["InflowFrom"] = ddlEntityName_InFlowFrom.SelectedItem;
                dr["Amount"] = txtAmount_Inflow1.Text;
                DtCashFlow.Rows.Add(dr);

                gvInflow.DataSource = DtCashFlow;
                gvInflow.DataBind();

                ViewState["DtCashFlow"] = DtCashFlow;
                FillDataFrom_ViewState_CashInflow();
                // TabContainerER.ActiveTabIndex = 2;
            }
            else
            {


                ScriptManagerAlert("Cash inflow", "Cash flow description should not be duplicated for the same date in Inflow details");
                /*  txtDate_GridInflow1.Text = txtAmount_Inflow1.Text = "";
                  ddlInflowDesc1.SelectedIndex = ddlEntityName_InFlow1.SelectedIndex = -1;*/


            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void CashOutflow_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {

            DataTable dtAcctype = ((DataTable)ViewState["PaymentRules"]);
            if (dtAcctype == null)
            {
                cvEnquiryResponse.ErrorMessage = "<br/> Correct the following validation(s): <br/><br/>  Select a Payment Rule";
                cvEnquiryResponse.IsValid = false;
                return;

                if (dtAcctype.Rows.Count == 0)
                {
                    cvEnquiryResponse.ErrorMessage = "<br/> Correct the following validation(s): <br/><br/>  Select a Payment Rule";
                    cvEnquiryResponse.IsValid = false;
                    return;
                }
            }
            DtCashFlowOut.PrimaryKey = new DataColumn[] { DtCashFlowOut.Columns["Date"], DtCashFlowOut.Columns["CashOutFlowID"], DtCashFlowOut.Columns["OutflowFromId"], DtCashFlowOut.Columns["EntityID"] };

            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];

            dtAcctype.DefaultView.RowFilter = " FieldName = 'AccountType'";

            //if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Normal Payment")
            //{
            //    DtCashFlowOut.PrimaryKey = new DataColumn[] { DtCashFlowOut.Columns["CashOutFlowID"], DtCashFlowOut.Columns["Date"], DtCashFlowOut.Columns["EntityID"] };
            //}
            TextBox txtDate_GridOutflow = gvOutFlow.FooterRow.FindControl("txtDate_GridOutflow") as TextBox;
            DropDownList ddlOutflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            DropDownList ddlEntityName_OutFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            TextBox txtAmount_Outflow = gvOutFlow.FooterRow.FindControl("txtAmount_Outflow") as TextBox;
            DropDownList ddlPaymentto_OutFlow = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;





            //  if (!ISDuplicateData_Outflow(ddlOutflowDesc.SelectedValue, txtDate_GridOutflow.Text))
            //  {
            DataRow dr = DtCashFlowOut.NewRow();

            //DtCashFlowOut.PrimaryKey = new DataColumn[] { DtCashFlowOut.Columns["CashOutFlowID"], DtCashFlowOut.Columns["Date"] };
            dr["Date"] = Utility.StringToDate(txtDate_GridOutflow.Text);
            string[] strArrayIds = ddlOutflowDesc.SelectedValue.Split(',');


            if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Deferred Payment")
            {
                if (Utility.CompareDates(DateTime.Now.ToString(strDateFormat), txtDate_GridOutflow.Text) == 0)
                {
                    Utility.FunShowAlertMsg(this, "Outflow date should be greater than Enquiry Response date for Deferred Payment");
                    return;
                }
            }
            if (dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Trade Advance" || dtAcctype.DefaultView.ToTable().Rows[0]["FieldValue"].ToString() == "Normal Payment")
            {
                if (Utility.StringToDate(DateTime.Now.ToString(strDateFormat)) != Utility.StringToDate(txtDate_GridOutflow.Text))
                {
                    Utility.FunShowAlertMsg(this, "Outflow date should be equal to Enquiry Response date for Normal Payment/Trade Advance");
                    return;
                }
            }


            dr["CashOutFlowID"] = strArrayIds[0];
            dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[1]));
            dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[2]));
            dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strArrayIds[3]));
            dr["CashFlow_Flag_ID"] = strArrayIds[4];
            dr["CashOutFlow"] = ddlOutflowDesc.SelectedItem;
            dr["OutflowFrom"] = ddlPaymentto_OutFlow.SelectedItem;
            dr["OutflowFromId"] = ddlPaymentto_OutFlow.SelectedValue;
            dr["EntityID"] = ddlEntityName_OutFlow.SelectedValue;
            dr["Entity"] = ddlEntityName_OutFlow.SelectedItem;
            dr["Amount"] = txtAmount_Outflow.Text;
            DtCashFlowOut.Rows.Add(dr);
            ViewState["DtCashFlowOut"] = DtCashFlowOut;
            if (!string.IsNullOrEmpty(Convert.ToString(((DataTable)ViewState["DtCashFlowOut"]).Compute("Sum(Amount)", "CashFlow_Flag_ID = 41"))))
            {
                decimal decToatlFinanceAmt = (decimal)DtCashFlowOut.Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");

                if (FunPriGetAmountFinanced() < decToatlFinanceAmt)
                {
                    cvEnquiryResponse.ErrorMessage = "<br/> Correct the following validation(s): <br/><br/>  Total finance amount in cashoutflow should be equal to amount financed.";
                    cvEnquiryResponse.IsValid = false;
                    DtCashFlowOut.Rows.RemoveAt(DtCashFlowOut.Rows.Count - 1);
                    ViewState["DtCashFlowOut"] = DtCashFlowOut;
                    //decToatlFinanceAmt = (decimal)DtCashFlowOut.Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");
                    return;
                }
            }
            if (DtCashFlowOut.Rows.Count > 0)
            {
                decimal decToatlOutflowAmt = (decimal)DtCashFlowOut.Compute("Sum(Amount)", "CashFlow_Flag_ID > 0");
                //lblTotalOutFlowAmount.Text = decToatlOutflowAmt.ToString();
                gvOutFlow.DataSource = DtCashFlowOut;
                gvOutFlow.DataBind();
                FillDataFrom_ViewState_CashOutflow();
            }
            else
            {
                FunPriBindCashFlowDetails();
            }



        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            if (ex.Message.Contains("Column 'CashOutFlowID, Date, EntityID' is constrained to be unique."))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cash Outflow", "alert('Cash flow cannot be repeated for the same date');", true);
            }
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void Repayment_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            DateTime dtTodate;
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            DropDownList ddlRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            // TextBox txtAmountRepaymentCashFlow_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtAmountRepaymentCashFlow_RepayTab") as TextBox;
            TextBox txtPerInstallmentAmount_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            TextBox txtBreakup_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;
            TextBox txtFromInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            TextBox txtToInstallment_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToInstallment_RepayTab") as TextBox;
            TextBox txtfromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            TextBox txtToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToDate_RepayTab") as TextBox;
            string[] strIds = ddlRepaymentCashFlow_RepayTab1.SelectedValue.ToString().Split(',');
            if (strIds[4] == "23")
            {

                if (DtRepayGrid.Rows.Count > 0)
                {
                    // objRepaymentStructure.FunPubGetNextRepaydate(DtRepayGrid, ddl_Frequency.SelectedItem.Value);
                    FunPriGetNextRepaydate();
                    if (Utility.StringToDate(txtfromdate_RepayTab1.Text) < dtNextDate)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlapped');", true);
                        return;
                    }
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
            objMethodParameters.Add("Frequency", ddl_Frequency.SelectedItem.Value);
            objMethodParameters.Add("TenureType", ViewState["TenureType"].ToString());
            objMethodParameters.Add("Tenure", ViewState["Tenure"].ToString());
            objMethodParameters.Add("DocumentDate", DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat));
            string strErrorMessage = "";
            DateTime dtNextFromdate;
            objRepaymentStructure.FunPubAddRepayment(out dtNextFromdate, out strErrorMessage, out DtRepayGrid, DtRepayGrid, objMethodParameters);
            if (strErrorMessage != "")
            {
                Utility.FunShowAlertMsg(this, strErrorMessage);
                return;
            }

            if (strIds[4] == "23")
            {
                decimal decIRRActualAmount = 0;
                decimal decTotalAmount = 0;
                string strFinAmount = GetLOBBasedFinAmt();
                decimal DecRoundOff;
                if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                    DecRoundOff = Convert.ToDecimal(ViewState["hdnRoundOff"]);
                else
                    DecRoundOff = 2;

                if (!objRepaymentStructure.FunPubValidateTotalAmount(DtRepayGrid, strFinAmount, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue, txt_Rate.Text, ViewState["TenureType"].ToString(), ViewState["Tenure"].ToString(), out decIRRActualAmount, out decTotalAmount, "1", DecRoundOff))
                {
                    Utility.FunShowAlertMsg(this, "Total Amount Should be equal to finance amount + interest (" + decTotalAmount + ")");
                    if (DtRepayGrid.Rows.Count > 0)
                        DtRepayGrid.Rows.RemoveAt(DtRepayGrid.Rows.Count - 1);
                    return;
                }
                if (((decimal)DtRepayGrid.Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23")) > 100)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Breakup Percentage cannot be greater that 100%');", true);
                    if (DtRepayGrid.Rows.Count > 0)
                        DtRepayGrid.Rows.RemoveAt(DtRepayGrid.Rows.Count - 1);
                    return;
                }
            }

            if (DtRepayGrid.Rows.Count > 0)
            {
                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();
            }

            TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            Label lblToInstallment_Upd = (Label)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lblToInstallment_RepayTab");
            txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(Convert.ToDecimal(lblToInstallment_Upd.Text.Trim()) + Convert.ToInt32("1"));
            TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            //txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(dtNextFromdate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            ViewState["DtRepayGrid"] = DtRepayGrid;
            FunPriGenerateNewRepayment();
            FunPriIRRReset();
            FunPriCalculateSummary(DtRepayGrid, "CashFlow", "TotalPeriodInstall");
            ((LinkButton)gvRepaymentDetails.Rows[gvRepaymentDetails.Rows.Count - 1].FindControl("lnRemoveRepayment")).Visible = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabRepayment.ErrorMessage = ex.Message;
            cv_TabRepayment.IsValid = false;
        }
    }

    private void FunPriGenerateNewRepayment()
    {
        try
        {
            DropDownList ddlRepaymentCashFlow_RepayTab = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            Utility.FillDLL(ddlRepaymentCashFlow_RepayTab, ((DataTable)ViewState["RepayCashInflowList"]), true);


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Cashflow Description in Repayment");
        }
    }


    protected void ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlCashFlowDesc = sender as DropDownList;
            FunPriDoCashflowBasedValidation(ddlCashFlowDesc);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabRepayment.ErrorMessage = "Error in fetching values based on cash flow details";
            cv_TabRepayment.IsValid = false;
        }

    }


    private void FunPriDoCashflowBasedValidation(DropDownList ddlCashFlowDesc)
    {
        try
        {

            string[] strvalues = ddlCashFlowDesc.SelectedValue.Split(',');
            TextBox txtFromInstallment_RepayTab1_upd = gvRepaymentDetails.FooterRow.FindControl("txtFromInstallment_RepayTab") as TextBox;
            TextBox txtfromdate_RepayTab1_Upd = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
            TextBox txtPerInstallmentAmount_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtPerInstallmentAmount_RepayTab") as TextBox;
            TextBox txtBreakup_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtBreakup_RepayTab") as TextBox;

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
            if (strvalues[4].ToString() != "23")
            {

                txtFromInstallment_RepayTab1_upd.Attributes.Remove("readonly");
                txtFromInstallment_RepayTab1_upd.ReadOnly = false;
                CalendarExtenderSD_ToDate_RepayTab.Enabled = false;
                CalendarExtenderSD_fromdate_RepayTab.Enabled = false;
                txtfromdate_RepayTab1_Upd.Text = "";
                txtBreakup_RepayTab1.Attributes.Add("readonly", "readonly");

            }
            else
            {
                if (ddl_Time_Value.SelectedValue == "2" || ddl_Time_Value.SelectedValue == "4")
                {
                    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                    objRepaymentStructure.dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedValue, Utility.StringToDate(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat)));
                    txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                else
                {


                    ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
                    objRepaymentStructure.FunPubGetNextRepaydate((DataTable)ViewState["DtRepayGrid"], ddl_Frequency.SelectedItem.Value);
                    txtFromInstallment_RepayTab1_upd.Text = Convert.ToString(objRepaymentStructure.intNextInstall + 1);
                    txtfromdate_RepayTab1_Upd.Text = DateTime.Parse(objRepaymentStructure.dtNextDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                txtFromInstallment_RepayTab1_upd.Attributes.Add("readonly", "readonly");
                txtBreakup_RepayTab1.Attributes.Remove("readonly");
                txtFromInstallment_RepayTab1_upd.ReadOnly = true;

                CalendarExtenderSD_ToDate_RepayTab.Enabled = true;
                CalendarExtenderSD_ToDate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;

                CalendarExtenderSD_fromdate_RepayTab.Enabled = true;
                CalendarExtenderSD_fromdate_RepayTab.Format = ObjS3GSession.ProDateFormatRW;

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw new ApplicationException(ex.Message);
        }
    }

    protected void Alert_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtAlertDetails = (DataTable)ViewState["DtAlertDetails"];

            DropDownList ddlAlert_Type = gvAlert.FooterRow.FindControl("ddlType_AlertTab") as DropDownList;
            DropDownList ddlAlert_ContactList = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as DropDownList;
            CheckBox ChkAlertEmail = gvAlert.FooterRow.FindControl("ChkEmail") as CheckBox;
            CheckBox ChkAlertSMS = gvAlert.FooterRow.FindControl("ChkSMS") as CheckBox;

            if (ChkAlertEmail.Checked || ChkAlertSMS.Checked)
            {

                if (DtAlertDetails.Rows.Count > 0)
                {
                    if (
                        (DtAlertDetails.Rows[gvAlert.Rows.Count - 1]["Type"].ToString() == ddlAlert_Type.SelectedValue) &&
                        (DtAlertDetails.Rows[gvAlert.Rows.Count - 1]["UserContact"].ToString() == ddlAlert_ContactList.SelectedValue)
                        )
                    {
                        Utility.FunShowAlertMsg(this, " Selected combination already exists");
                        //ddlAlert_Type.SelectedIndex =
                        //    ddlAlert_ContactList.SelectedIndex = -1;
                        return;
                    }
                }



                DataRow dr = DtAlertDetails.NewRow();

                dr["Type"] = ddlAlert_Type.SelectedValue;
                dr["UserContact"] = ddlAlert_ContactList.SelectedValue;
                dr["EMail"] = ChkAlertEmail.Checked;
                dr["SMS"] = ChkAlertSMS.Checked;

                DtAlertDetails.Rows.Add(dr);

                gvAlert.DataSource = DtAlertDetails;
                gvAlert.DataBind();

                ViewState["DtAlertDetails"] = DtAlertDetails;
                FillDataFrom_ViewState();
            }
            else
            {
                cv_Main.ErrorMessage = " select Email or SMS";
                cv_Main.IsValid = false;
                Utility.FunShowAlertMsg(this, " Select Email or SMS");
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void FollowUp_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtFollowUp = (DataTable)ViewState["DtFollowUp"];

            TextBox txttxtDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtDate_GridFollowup") as TextBox;
            DropDownList ddlfrom_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as DropDownList;
            DropDownList ddlTo_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as DropDownList;
            TextBox txtAction_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtAction_GridFollowup") as TextBox;
            TextBox txtActionDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtActionDate_GridFollowup") as TextBox;
            TextBox txtCustomerResponse_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtCustomerResponse_GridFollowup") as TextBox;
            TextBox txtRemarks_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtRemarks_GridFollowup") as TextBox;

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupDate1 = gvFollowUp.FooterRow.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_FollowupDate1.Format = strDateFormat;

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupActionDate1 = gvFollowUp.FooterRow.FindControl("CalendarExtenderSD_FollowupActionDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_FollowupActionDate1.Format = strDateFormat;

            if (CompareDates(txttxtDate_GridFollowup1.Text, txtActionDate_GridFollowup1.Text))
            {
                if (ddlfrom_GridFollowup1.SelectedValue != ddlTo_GridFollowup1.SelectedValue)
                {
                    DataRow dr = DtFollowUp.NewRow();

                    dr["Date"] = txttxtDate_GridFollowup1.Text;
                    dr["From"] = ddlfrom_GridFollowup1.SelectedValue;
                    dr["To"] = ddlTo_GridFollowup1.SelectedValue;
                    dr["Action"] = txtAction_GridFollowup1.Text;
                    dr["ActionDate"] = txtActionDate_GridFollowup1.Text;
                    dr["CustomerResponse"] = txtCustomerResponse_GridFollowup1.Text;
                    dr["Remarks"] = txtRemarks_GridFollowup1.Text;

                    DtFollowUp.Rows.Add(dr);

                    gvFollowUp.DataSource = DtFollowUp;
                    gvFollowUp.DataBind();

                    ViewState["DtFollowUp"] = DtFollowUp;
                    FillDataFrom_ViewState_FollowUp();
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "From user and To user cannot be the same");
                    return;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Action Date cannot be less than the Followup Date");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    #endregion

    #region Methods

    private void FunPriLoadProductROI(string LOBID)
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            string strProcedureName = "S3g_Org_GetCustomerLookup";

            ddlProductAssign.DataSource = null;
            ddlProductAssign.DataBind();

            Procparam.Add("@Option", "31");
            Procparam.Add("@Param1", LOBID);
            ddlProductAssign.BindDataTable(strProcedureName, Procparam, new string[] { "Product_ID", "Product_Code" });

            ddlROIRuleList.DataSource = null;
            ddlROIRuleList.DataBind();

            Procparam.Clear();
            Procparam.Add("@Option", "41");
            Procparam.Add("@Param1", intCompanyId.ToString());
            Procparam.Add("@Param2", LOBID);
            ddlROIRuleList.BindDataTable(strProcedureName, Procparam, new string[] { "ROI_Rules_ID", "ROI_Rule_Number", "Model_Description" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadWorkflow(string LOBID, string ProductID, bool SelecT)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            if (LOBID == "0" || ProductID == "0")
            {

            }
            else
            {
                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

                if (SelecT)
                {
                    ObjStatus.Option = 150;
                    ObjStatus.Param1 = ProductID;
                    ObjStatus.Param2 = LOBID;
                    ObjStatus.Param3 = intCompanyId.ToString();
                    ObjStatus.Param4 = "0";
                    DtAlertDetails = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                    if (DtAlertDetails.Rows.Count > 0)
                    {
                        txtworkflowSequence_Change.Text = Convert.ToString(DtAlertDetails.Rows[0]["Workflow_Sequence"]);
                        ViewState["WorkflowId"] = Convert.ToString(DtAlertDetails.Rows[0]["Workflow_ID"]);
                    }
                    else
                    {
                        ObjStatus.Option = 150;
                        ObjStatus.Param1 = ProductID;
                        ObjStatus.Param2 = LOBID;
                        ObjStatus.Param3 = intCompanyId.ToString();
                        ObjStatus.Param4 = "1";
                        DtAlertDetails = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                        if (DtAlertDetails.Rows.Count > 0)
                        {
                            txtworkflowSequence_Change.Text = "";
                            ScriptManagerAlert("workflow", "Workflow Sequence not defined for the Enquiry Response");
                            return;
                        }
                        txtworkflowSequence_Change.Text = "";
                        ScriptManagerAlert("workflow", "Workflow Sequence not defined for the selected Line of Business and Product");
                    }
                }
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriLoadEnquiryDetails(string EnquiryNo)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 153;
            ObjStatus.Param1 = EnquiryNo;
            ObjStatus.Param2 = Convert.ToString(intCompanyId);
            DtAlertDetails = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (DtAlertDetails.Rows.Count > 0)
            {
                string strCustomerAddress = "";
                ViewState["OLExistingAsset"] = Convert.ToInt32(DtAlertDetails.Rows[0]["AssetTypeExisting"]);
                txtLOB.Text = Convert.ToString(DtAlertDetails.Rows[0]["LOB"]);
                //txtBranch.Text = Convert.ToString(DtAlertDetails.Rows[0]["Branch"]);
                txtBranch.Text = Convert.ToString(DtAlertDetails.Rows[0]["Location"]);
                txtEnquiryNo.Text = Convert.ToString(DtAlertDetails.Rows[0]["EnquiryNo"]);
                txtEnquiryDate.Text = DateTime.Parse(Convert.ToString(DtAlertDetails.Rows[0]["EnquiryDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);

                txtCustomerCode.Text = Convert.ToString(DtAlertDetails.Rows[0]["CustomerCode"]);
                txtCustomerName.Text = Convert.ToString(DtAlertDetails.Rows[0]["Customer_Name"]);

                strCustomerAddress = Convert.ToString(DtAlertDetails.Rows[0]["Address"]) + '\n' +
                                  Convert.ToString(DtAlertDetails.Rows[0]["City"]) + '\n' +
                                  Convert.ToString(DtAlertDetails.Rows[0]["State"]) + '\n' +
                                  Convert.ToString(DtAlertDetails.Rows[0]["Country"]) + '\n' +
                                  Convert.ToString(DtAlertDetails.Rows[0]["PINCodeZipCode"]);

                S3GCustomerAddress1.SetCustomerDetails(string.Empty, strCustomerAddress, txtCustomerName.Text, Convert.ToString(DtAlertDetails.Rows[0]["Telephone"]), Convert.ToString(DtAlertDetails.Rows[0]["Mobile"]), Convert.ToString(DtAlertDetails.Rows[0]["EMail"]), Convert.ToString(DtAlertDetails.Rows[0]["Website"]));
                //S3GCustomerAddress1.SetCustomerDetails(DtAlertDetails.Rows[0], true);
                //S3GCustomerAddress1.SetCustomerDetails(Convert.ToInt32(DtAlertDetails.Rows[0]["Customer_ID"].ToString()), false);
                //S3GCustomerAddress1.SetCustomerDetails(Convert.ToString(DtAlertDetails.Rows[0]["CustomerCode"]),
                //    strCustomerAddress, Convert.ToString(DtAlertDetails.Rows[0]["Customer_Name"]),
                //    Convert.ToString(DtAlertDetails.Rows[0]["Mobile"]),
                //    Convert.ToString(DtAlertDetails.Rows[0]["EMail"]),
                //    Convert.ToString(DtAlertDetails.Rows[0]["Website"]));



                txtStatus.Text = Convert.ToString(DtAlertDetails.Rows[0]["Status"]);
                if (Convert.ToString(DtAlertDetails.Rows[0]["AssetCode"]) != null && Convert.ToString(DtAlertDetails.Rows[0]["AssetCode"]) != string.Empty)
                {
                    string[] assetdesc = Convert.ToString(DtAlertDetails.Rows[0]["AssetCode"]).Split('-');
                    //if (assetdesc[1] != null)
                    /*    Begin -  Modified  By Senthillumar P [PSK]  */
                    if (assetdesc.Length > 1)
                    {
                        txtAssetDescription.Text = assetdesc[1].ToString();
                    }
                    else
                    {
                        txtAssetDescription.Text = assetdesc[0].ToString();
                    }

                    /*    End -  Modified  By Senthillumar P [PSK]  */
                }
                if (!string.IsNullOrEmpty(Convert.ToString(DtAlertDetails.Rows[0]["MarginAmount"])))
                    txtAssetValue.Text = Convert.ToString(Convert.ToInt64(DtAlertDetails.Rows[0]["FacilityAmount"].ToString()) + Convert.ToInt64(DtAlertDetails.Rows[0]["MarginAmount"].ToString()));
                else
                    txtAssetValue.Text = Convert.ToString(DtAlertDetails.Rows[0]["FacilityAmount"]);
                txtFinanceAmount.Text = Convert.ToString(DtAlertDetails.Rows[0]["FacilityAmount"]);
                txtResidualvalue.Text = Convert.ToString(DtAlertDetails.Rows[0]["ResidualValue"]);
                //OL related changes.
                //if (DtAlertDetails.Rows[0]["ResidualValue"].ToString() != string.Empty || DtAlertDetails.Rows[0]["ResidualValue"].ToString() != "0")
                //{
                //    txtResidualAmt_Cashflow.Text = Convert.ToString(DtAlertDetails.Rows[0]["ResidualValue"].ToString());
                //}
                ViewState["Customer_ID"] = Convert.ToString(DtAlertDetails.Rows[0]["Customer_ID"]);
                txtBlockdepreciation.Text = Convert.ToString(DtAlertDetails.Rows[0]["Block_Depreciation_Rate"]);
                txtBookdepreciation.Text = Convert.ToString(DtAlertDetails.Rows[0]["Book_Depreciation_Rate"]);
                if (!string.IsNullOrEmpty(Convert.ToString(DtAlertDetails.Rows[0]["Constitution_ID"])))
                {
                    ViewState["constitutionid"] = Convert.ToString(DtAlertDetails.Rows[0]["Constitution_ID"]);
                }
                //assignment
                txtLOBAssign.Text = Convert.ToString(DtAlertDetails.Rows[0]["LOB"]);
                //txtBranchAssign.Text = Convert.ToString(DtAlertDetails.Rows[0]["Branch"]);
                txtBranchAssign.Text = Convert.ToString(DtAlertDetails.Rows[0]["Location"]);
                txtProductAssign.Text = Convert.ToString(DtAlertDetails.Rows[0]["Product"]);
                txtworkflowSequence.Text = Convert.ToString(DtAlertDetails.Rows[0]["WorkflowCode"]);
                ViewState["WorkflowId"] = Convert.ToString(DtAlertDetails.Rows[0]["WorkflowID"]);
                FunPriLoadDropDownList();

                FunPriLoadProductROI(Convert.ToString(DtAlertDetails.Rows[0]["LOB_ID"]));
                FunPriLoadWorkflow(Convert.ToString(DtAlertDetails.Rows[0]["LOB_ID"]), Convert.ToString(DtAlertDetails.Rows[0]["Product_ID"]), false);
                ViewState["cashlobid"] = Convert.ToString(DtAlertDetails.Rows[0]["LOB_ID"]);
                ViewState["cashproductid"] = Convert.ToString(DtAlertDetails.Rows[0]["Product_ID"]);
                //ViewState["cashbranchid"] = Convert.ToString(DtAlertDetails.Rows[0]["Branch_ID"]);
                ViewState["cashbranchid"] = Convert.ToString(DtAlertDetails.Rows[0]["Location_ID"]);
                if (ddlLOBAssign.SelectedIndex > 0)
                {
                    FunPriAssignRate(ddlLOBAssign.SelectedValue);
                }
                else
                {
                    if (ViewState["cashlobid"] != null)
                        FunPriAssignRate(ViewState["cashlobid"].ToString());

                }
                FunPriLoadPaymentDropDown();


                //follow up
                txtLOB_Followup.Text = Convert.ToString(DtAlertDetails.Rows[0]["LOB"]);
                txtBranch_Followup.Text = Convert.ToString(DtAlertDetails.Rows[0]["Location"]);
                txtEnquiry_Followup.Text = Convert.ToString(DtAlertDetails.Rows[0]["EnquiryNo"]);
                txtEnquiryDate_Followup.Text = DateTime.Parse(Convert.ToString(DtAlertDetails.Rows[0]["EnquiryDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                txtProspectName_Followup.Text = Convert.ToString(DtAlertDetails.Rows[0]["Customer_Name"]);
                //+"," + Environment.NewLine +
                //Convert.ToString(DtAlertDetails.Rows[0]["Address"]) + "," + Environment.NewLine +
                //Convert.ToString(DtAlertDetails.Rows[0]["Address2"]) + "," + Environment.NewLine +
                //Convert.ToString(DtAlertDetails.Rows[0]["Mobile"]) + "," + Environment.NewLine +
                //Convert.ToString(DtAlertDetails.Rows[0]["Email"]);
                txtOfferNo_Followup.Text = "";
                txtApplication_Followup.Text = "";
                ViewState["Tenure"] = Convert.ToInt32(DtAlertDetails.Rows[0]["Tenure"]);
                ViewState["TenureType"] = Convert.ToString(DtAlertDetails.Rows[0]["TenureType"]);
                if (!string.IsNullOrEmpty(Convert.ToString(DtAlertDetails.Rows[0]["Tenure"])))
                {
                    txtTenure.Text = DtAlertDetails.Rows[0]["Tenure"].ToString();
                }
                if (!string.IsNullOrEmpty(Convert.ToString(DtAlertDetails.Rows[0]["TenureType"])))
                {
                    txtTenureType.Text = DtAlertDetails.Rows[0]["TenureType"].ToString();
                }
                AssetValueRename();
                FunOLRelatedChanges();

            }
            else
            {

                throw new ApplicationException(" check the data");
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }
    private void FunOLRelatedChanges()
    {

        //for  OL related changes on 25/07/2011.
        //if (ViewState["OLExistingAsset"] != null && Convert.ToInt32(ViewState["OLExistingAsset"]) > 0)
        string LOB = "";
        LOB = FunPriGetLOBstring();
        if (!string.IsNullOrEmpty(LOB))
        {
            if (LOB.Split('-')[0].ToString().ToLower().Trim() == "ol")
            {
                FundisableOLRelatedCtrls(false);
            }
            else
            {
                FundisableOLRelatedCtrls(true);
            }
        }

    }

    private void FundisableOLRelatedCtrls(bool blnflag)
    {

        //ddlLOBAssign.Enabled =
        //ddlBranchAssign.Enabled = 
        //ddlProductAssign.Enabled = 
        ddlPaymentRuleList.Enabled =
        rfvddlPaymentRuleList.Enabled =
            //gvInflow.Enabled = 
        gvOutFlow.Enabled =
        btnFetchPayment.Enabled = blnflag;
        if (ddlLOBAssign.SelectedIndex <= 0)
            ddlLOBAssign.Enabled = blnflag;
        else
            ddlLOBAssign.Enabled = !blnflag;
    }


    private void AssetValueRename()
    {
        if (txtLOB.Text.Trim().Length > 0)
        {
            string[] lobcode = (txtLOB.Text.Trim()).Split('-');
            if (lobcode[0].ToString() != null)
            {
                if (lobcode[0].ToString().Trim().ToLower() == "ft")
                    lblAssetValue.Text = "Invoice Value";
                else
                    lblAssetValue.Text = "Asset Value";
            }

        }

    }

    private void FunPriLoadEnquiryResponseDetails(int intEnquiryResponseId)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        ObjEnquiryResponseClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            byte[] byteEnquiryResponseDetails = ObjEnquiryResponseClient.FunPubGetEnquiryResponse(intEnquiryResponseId);
            DataSet objEnquiryResponseDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byteEnquiryResponseDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (objEnquiryResponseDetails.Tables.Count == 0)
            {
                return;
            }
            if (objEnquiryResponseDetails.Tables[0].Rows.Count > 0)
            {
                txtLOB.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["LOB_Name"]);
                txtBranch.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Location"]);
                txtEnquiryNo.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Enquiry_No"]);
                txtEnquiryDate.Text = DateTime.Parse(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Enquiry_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW); ;
                txtCustomerCode.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Customer_Code"]);
                txtCustomerName.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Customer_Name"]);


                S3GCustomerAddress1.SetCustomerDetails(objEnquiryResponseDetails.Tables[0].Rows[0], true);

                txtStatus.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["StatusName"]);
                lblEnqStatus.Value = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Status"]);
                txtRespondedDate.Text = DateTime.Parse(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Response_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                txtAssetDescription.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["AssetCode"]);
                //txtAssetValue.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Margin_Amount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Margin_Amount"])))
                    txtAssetValue.Text = Convert.ToString(Convert.ToInt64(objEnquiryResponseDetails.Tables[0].Rows[0]["Finance_Amount_Sought"].ToString()) + Convert.ToInt64(objEnquiryResponseDetails.Tables[0].Rows[0]["Margin_Amount"].ToString()));
                else
                    txtAssetValue.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Finance_Amount_Sought"]);
                txtFinanceAmount.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Finance_Amount_Sought"]);
                if (Convert.ToInt64(objEnquiryResponseDetails.Tables[0].Rows[0]["Residual_Margin_Amount"]) != 0)
                    txtResidualvalue.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Residual_Margin_Amount"]);
                ViewState["Tenure"] = Convert.ToInt64(objEnquiryResponseDetails.Tables[0].Rows[0]["Tenure"]);
                ViewState["TenureType"] = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Tenure_Type"]);
                ViewState["Customer_ID"] = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Customer_ID"]);

                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Tenure"])))
                {
                    txtTenure.Text = objEnquiryResponseDetails.Tables[0].Rows[0]["Tenure"].ToString();
                }

                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Tenure_Type"])))
                {
                    txtTenureType.Text = objEnquiryResponseDetails.Tables[0].Rows[0]["Tenure_Type"].ToString();
                }


                if (ddlLOBAssign.SelectedIndex > 0)
                {
                    FunPriLoadProductROI(ddlLOBAssign.SelectedValue);
                }
                else
                {
                    FunPriLoadProductROI(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["LOB_ID"]));
                }
                if (ddlLOBAssign.SelectedIndex > 0)
                {
                    ViewState["cashlobid"] = ddlLOBAssign.SelectedValue;
                }
                else
                {
                    ViewState["cashlobid"] = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["LOB_ID"]);
                }
                if (ddlProductAssign.SelectedIndex > 0)
                {
                    ViewState["cashproductid"] = ddlProductAssign.SelectedValue;
                }
                else
                {
                    ViewState["cashproductid"] = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Product_ID"]);
                }
                if (ddlBranchAssign.SelectedIndex > 0)
                {
                    ViewState["cashbranchid"] = ddlBranchAssign.SelectedValue;
                }
                else
                {
                    //ViewState["cashbranchid"] = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Branch_ID"]);
                    ViewState["cashbranchid"] = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Location_ID"]);
                }
                ViewState["OLExistingAsset"] = Convert.ToInt32(objEnquiryResponseDetails.Tables[0].Rows[0]["AssetTypeExisting"]);
                //FunPriLoadProductROI(ddlLOBAssign.SelectedValue);
                //ddlProductAssign.SelectedValue = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Product_Id"]);
                ddlROIRuleList.SelectedValue = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_ROI_Rules_ID"]);
                divROIRuleInfo.Visible = true;
                FunPriLoadROIDropDown();
                if (objEnquiryResponseDetails.Tables[8].Rows.Count > 0)
                {
                    FunPriLoadROIDetails(objEnquiryResponseDetails.Tables[8]);
                    ViewState["ROIDetails"] = objEnquiryResponseDetails.Tables[8];
                }
                FunPriLoadPaymentDropDown();
                ddlPaymentRuleList.SelectedValue = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_Payment_RuleCard_ID"]);
                FunPriShowPaymentDetails(objEnquiryResponseDetails.Tables[7]);

                txtBlockdepreciation.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Repay_Block_Depriciation"]);
                txtBookdepreciation.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Repay_Book_Depriciation"]);
                ddlResponse.SelectedValue = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Response_Details_ID"]);

                if (ddlResponse.SelectedValue == "190" || ddlResponse.SelectedValue == "191" || ddlResponse.SelectedValue == "192")
                {
                    /*Status_ID assigned on Modify mode*/
                    ViewState["statusID"] = "198";

                    //btnNext.Enabled = false;
                    for (int intTabIndex = 0; intTabIndex < TabContainerER.Tabs.Count; intTabIndex++)
                    {
                        if (intTabIndex == 1)
                        {
                            TabContainerER.Tabs[intTabIndex].Enabled = true;
                        }
                        else
                        {
                            TabContainerER.Tabs[intTabIndex].Enabled = false;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Repay_Accounting_IRR"])))
                {
                    txtAccountIRR_Repay.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Repay_Accounting_IRR"]);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Repay_Company_IRR"])))
                {
                    txtBusinessIRR_Repay.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Repay_Company_IRR"]);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Repay_Business_IRR"])))
                {
                    txtCompanyIRR_Repay.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Repay_Business_IRR"]);
                }
                //assignment
                txtLOBAssign.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["LOB_Name"]);
                //txtBranchAssign.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Branch_Name"]);
                txtBranchAssign.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Location"]);
                txtProductAssign.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Product_Name"]);
                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Workflow_Sequnce_ID"])))
                {
                    txtworkflowSequence.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Workflow_Sequnce_ID"]);
                    //txtworkflowSequence_Change.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Workflow_Sequnce_ID"]);
                }
                //follow up
                txtLOB_Followup.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["LOB_Name"]);
                //txtBranch_Followup.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Branch_Name"]);
                txtBranch_Followup.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Location"]);
                txtEnquiry_Followup.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Enquiry_No"]);
                txtEnquiryDate_Followup.Text = DateTime.Parse(Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Enquiry_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                txtProspectName_Followup.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Customer_Name"]) + "," + Environment.NewLine +
                    Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Comm_Address1"]) + "," + Environment.NewLine +
                    Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Comm_Address2"]) + "," + Environment.NewLine +
                    Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Mobile_No"]) + "," + Environment.NewLine +
                    Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Email_Id"]);
                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[2].Rows[0]["Offer_Number"])))
                {
                    txtOfferNo_Followup.Text = Convert.ToString(objEnquiryResponseDetails.Tables[2].Rows[0]["Offer_Number"]);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(objEnquiryResponseDetails.Tables[2].Rows[0]["Application_Number"])))
                {
                    if (Convert.ToString(objEnquiryResponseDetails.Tables[2].Rows[0]["Application_Number"]) != "0")
                        txtApplication_Followup.Text = Convert.ToString(objEnquiryResponseDetails.Tables[2].Rows[0]["Application_Number"]);
                }

                if (Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_ResidualValue"]) != "0.0000")
                    txtResidualValue_Cashflow.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_ResidualValue"]);
                if (Convert.ToDecimal(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_ResidualValueAmount"]) > 0)
                    txtResidualAmt_Cashflow.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_ResidualValueAmount"]);

                if (Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_Margin"]) != "0.0000")
                    txtMarginMoneyPer_Cashflow.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_Margin"]);
                if (Convert.ToDecimal(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_Margin_Amount"]) > 0)
                    txtMarginMoneyAmount_Cashflow.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Offer_Margin_Amount"]);

                txtMobileNo.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Mobile_No"]);
                txtEmailID.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["Email_ID"]);
                txtRespondedBy.Text = Convert.ToString(objEnquiryResponseDetails.Tables[0].Rows[0]["User_Name"]);
                FunPriLoadGridDropDownList();
                FunPriBindAlertDetails();
                FunPriLoadFollowup(intEnquiryResponseId.ToString());

                if (gvRepaymentDetails.Rows.Count > 0)
                {
                    FunPriBindRepaymentCashflowDetails();

                    AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                    AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;

                    CalendarExtenderSD_fromdate_RepayTab1.Format = strDateFormat;
                    CalendarExtenderSD_ToDate_RepayTab1.Format = strDateFormat;

                    TextBox txtfromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
                    TextBox txtToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToDate_RepayTab") as TextBox;

                    txtfromdate_RepayTab1.Attributes.Add("readonly", "readonly");
                    txtToDate_RepayTab1.Attributes.Add("readonly", "readonly");
                    FunPriShowRepaymentFooter();
                }
                else
                {
                    FunPriBindRepaymentDetails("0");
                }
                ViewState["RepaymentSummary"] = objEnquiryResponseDetails.Tables[5];
                if (objEnquiryResponseDetails.Tables[5].Rows.Count > 0)
                {

                    gvRepaymentSummary.DataSource = objEnquiryResponseDetails.Tables[5];
                    gvRepaymentSummary.DataBind();
                }

                if (objEnquiryResponseDetails.Tables[1].Rows.Count > 0)
                {
                    ViewState["DtAlertDetails"] = objEnquiryResponseDetails.Tables[1];
                    gvAlert.DataSource = objEnquiryResponseDetails.Tables[1];
                    gvAlert.DataBind();

                    FillDataFrom_ViewState();
                }
                if (objEnquiryResponseDetails.Tables[9].Rows.Count > 0)
                {
                    ViewState["DtCashFlow"] = objEnquiryResponseDetails.Tables[9];
                    gvInflow.DataSource = objEnquiryResponseDetails.Tables[9];
                    gvInflow.DataBind();
                    FillDataFrom_ViewState_CashInflow();
                }
                else
                {
                    S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

                    ObjStatus.Option = 310;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    if (ddlLOBAssign.SelectedIndex > 0)
                        ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
                    else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                        ObjStatus.Param2 = ViewState["cashlobid"].ToString();

                    DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                    ViewState["CashInflowList"] = DtCashFlow;

                    ObjStatus.Option = 170;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    if (ddlLOBAssign.SelectedIndex > 0)
                        ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
                    else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                        ObjStatus.Param2 = ViewState["cashlobid"].ToString();
                    DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                    ViewState["RepayCashInflowList"] = DtCashFlow;
                    DtCashFlow.Dispose();

                    ObjStatus.Option = 311;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    if (ddlLOBAssign.SelectedIndex > 0)
                        ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
                    else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                        ObjStatus.Param2 = ViewState["cashlobid"].ToString();

                    //DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                    //ViewState["CashOutflowList"] = DtCashFlow;

                    ObjStatus.Option = 38;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    DtFollowUp = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                    ViewState["EntityMasterList"] = DtFollowUp;

                    ObjStatus.Option = 6;
                    ObjStatus.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();
                    ViewState["CashFlowFrom"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


                    ObjStatus.Option = 151;
                    ObjStatus.Param1 = null;
                    DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


                    gvInflow.DataSource = DtCashFlow;
                    gvInflow.DataBind();

                    DtCashFlow.Rows.Clear();
                    ViewState["DtCashFlow"] = DtCashFlow;
                    DtCashFlow.Dispose();

                    gvInflow.Rows[0].Cells.Clear();
                    gvInflow.Rows[0].Visible = false;
                    FillDataFrom_ViewState_CashInflow();
                }
                if (objEnquiryResponseDetails.Tables[10].Rows.Count > 0)
                {
                    ViewState["DtCashFlowOut"] = objEnquiryResponseDetails.Tables[10];
                    gvOutFlow.DataSource = objEnquiryResponseDetails.Tables[10];
                    gvOutFlow.DataBind();
                    FillDataFrom_ViewState_CashOutflow();
                }
                else
                {
                    S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();



                    ObjStatus.Option = 170;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    if (ddlLOBAssign.SelectedIndex > 0)
                        ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
                    else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                        ObjStatus.Param2 = ViewState["cashlobid"].ToString();
                    DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                    ViewState["RepayCashInflowList"] = DtCashFlow;
                    DtCashFlow.Dispose();

                    ObjStatus.Option = 311;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    if (ddlLOBAssign.SelectedIndex > 0)
                        ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
                    else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                        ObjStatus.Param2 = ViewState["cashlobid"].ToString();

                    DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                    ViewState["CashOutflowList"] = DtCashFlow;

                    ObjStatus.Option = 38;
                    ObjStatus.Param1 = intCompanyId.ToString();
                    DtFollowUp = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                    ViewState["EntityMasterList"] = DtFollowUp;

                    ObjStatus.Option = 6;
                    ObjStatus.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();
                    ViewState["CashFlowFrom"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


                    ObjStatus.Option = 152;
                    DtCashFlowOut = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                    gvOutFlow.DataSource = DtCashFlowOut;
                    gvOutFlow.DataBind();

                    DtCashFlowOut.Rows.Clear();
                    ViewState["DtCashFlowOut"] = DtCashFlowOut;
                    DtCashFlowOut.Dispose();

                    gvOutFlow.Rows[0].Cells.Clear();
                    gvOutFlow.Rows[0].Visible = false;
                    FillDataFrom_ViewState_CashOutflow();
                }

                if (objEnquiryResponseDetails.Tables[11].Rows.Count > 0)
                {
                    grvRepayStructure.DataSource = objEnquiryResponseDetails.Tables[11];
                    grvRepayStructure.DataBind();
                    ViewState["RepayStructure"] = objEnquiryResponseDetails.Tables[11];
                }

                if (objEnquiryResponseDetails.Tables[4].Rows.Count > 0)
                {
                    gvRepaymentDetails.DataSource = objEnquiryResponseDetails.Tables[4];
                    gvRepaymentDetails.DataBind();
                    ViewState["DtRepayGrid"] = objEnquiryResponseDetails.Tables[4];
                    FunPriShowRepaymetDetails((decimal)objEnquiryResponseDetails.Tables[4].Compute("SUM(TotalPeriodInstall)", "CashFlow_Flag_ID =23"));

                }
            }
            else
            {
                throw new ApplicationException(" check the data");
            }
            AssetValueRename();
            FunOLRelatedChanges();

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjEnquiryResponseClient.Close();
            ObjCustomerService.Close();
        }
    }


    private void FunPriShowRepaymentFooter()
    {
        if (ddl_Rate_Type.SelectedItem.Value == "2" && Request.QueryString["qsMode"].ToString() != "Q")
        {
            ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = true;
            ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = false;
        }
        else
        {
            ((TextBox)gvRepaymentDetails.Rows[0].FindControl("txRepaymentFromDate")).Visible = false;
            ((Label)gvRepaymentDetails.Rows[0].FindControl("lblfromdate_RepayTab")).Visible = true;
        }
    }



    private void FunPriLoadGridDropDownList()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus;

            ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 35;
            ObjStatus.Param1 = intCompanyId.ToString();
            ViewState["UserListFolloup"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();
            ViewState["CashFlowFrom"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            ObjStatus.Option = 310;
            ObjStatus.Param1 = intCompanyId.ToString();
            if (ddlLOBAssign.SelectedIndex > 0)
                ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
            else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                ObjStatus.Param2 = ViewState["cashlobid"].ToString();
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["CashInflowList"] = DtCashFlow;
            ObjStatus.Option = 311;
            ObjStatus.Param1 = intCompanyId.ToString();
            if (ddlLOBAssign.SelectedIndex > 0)
                ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
            else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                ObjStatus.Param2 = ViewState["cashlobid"].ToString();
            ViewState["CashOutflowList"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ObjStatus.Option = 170;
            ObjStatus.Param1 = intCompanyId.ToString();
            if (ddlLOBAssign.SelectedIndex > 0)
                ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
            else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                ObjStatus.Param2 = ViewState["cashlobid"].ToString();
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["RepayCashInflowList"] = DtCashFlow;


            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.ALERT_TYPE.ToString();
            //ViewState["Alert_Type"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            //Commented and added by saranya on 08-Mar-2012 based on sudarsan observation
            DataTable dtAlertType = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            DataRow[] dr = dtAlertType.Select("ID in(141,216,142,217,143,218)");
            dtAlertType = dr.CopyToDataTable();
            ViewState["Alert_Type"] = dtAlertType;
            //End Here


            ObjStatus.Option = 35;
            ObjStatus.Param1 = intCompanyId.ToString();
            ViewState["UserList"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            ObjStatus.Option = 38;
            ObjStatus.Param1 = intCompanyId.ToString();
            DtFollowUp = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["EntityMasterList"] = DtFollowUp;




        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void ClearForm()
    {
        try
        {

            txtAccountIRR_Repay.Text = txtCompanyIRR_Repay.Text = txtBusinessIRR_Repay.Text = string.Empty;
            hdnROIRule.Value = "";
            hdnPayment.Value = "";
            ddlLOBAssign.SelectedIndex = -1;
            //ddlBranchAssign.SelectedIndex = -1;
            ddlBranchAssign.Items.Clear();

            //follow up
            txtLOB_Followup.Text = txtBranch_Followup.Text = txtEnquiry_Followup.Text = txtEnquiryDate_Followup.Text = txtProspectName_Followup.Text =
            txtOfferNo_Followup.Text = txtApplication_Followup.Text = txtResidualValue_Cashflow.Text = txtResidualAmt_Cashflow.Text =
            txtMarginMoneyPer_Cashflow.Text = txtMarginMoneyAmount_Cashflow.Text = txtworkflowSequence_Change.Text = string.Empty;
            FunPriBindCashFlowDetails();
            FunPriBindRepaymentDetails("0");
            FunPriBindAlertDetails();
            FunPriBindFollowupDetails();
            gvPaymentRuleDetails.DataSource = null;
            gvPaymentRuleDetails.DataBind();
            if (intEnquiryResponseId == 0)
            {
                //FunPriLoadDropDownList();
                //FunPriLoadROIDropDown();
                ddlROIRuleList.SelectedValue = "0";
                ddlPaymentRuleList.SelectedValue = "0";
                ddlResponse.SelectedValue = "0";
                FunPriToggleROIControls(false);
                TabContainerER.ActiveTabIndex = 1;
            }
            else
            {
                FunPriToggleTabIndex(false);
            }

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriToggleTabIndex(Boolean ED)
    {
        try
        {
            for (int i = 1; TabContainerER.Tabs.Count - 1 >= i; i++)
            {
                TabContainerER.Tabs[i].Enabled = ED;
            }


            gvAlert.Enabled = false;
            gvFollowUp.Enabled = false;
            gvOutFlow.Enabled = false;
            gvInflow.Enabled = false;
            gvRepaymentDetails.Enabled = false;
            gvFollowUp.Enabled = false;

            ddlLOBAssign.Enabled = false;
            ddlBranchAssign.Enabled = false;
            ddlProductAssign.Enabled = false;
            ddlResponse.Enabled = false;
            ddlROIRuleList.Enabled = false;
            ddlPaymentRuleList.Enabled = false;

            txtFinanceAmount.ReadOnly = true;
            txtResidualvalue.ReadOnly = true;
            txtResidualValue_Cashflow.ReadOnly = true;
            txtResidualAmt_Cashflow.ReadOnly = true;
            txtMarginMoneyPer_Cashflow.ReadOnly = true;
            txtMarginMoneyAmount_Cashflow.ReadOnly = true;


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private bool CompareDates(string startDate, string endDate)
    {
        try
        {

            CultureInfo myDTFI = new CultureInfo("en-GB", true);
            DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
            DTF.ShortDatePattern = strDateFormat;
            DateTime strDT = new DateTime();
            DateTime endDT = new DateTime();
            if (startDate != "")
            {
                strDT = System.Convert.ToDateTime(startDate, DTF);
            }
            if (endDate != "")
            {
                endDT = System.Convert.ToDateTime(endDate, DTF);
            }
            if (strDT > endDT)
                return false;
            else
                return true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void GetCurrentUserInfo()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 163;
            ObjStatus.Param1 = intUserId.ToString();
            DtAlertDetails = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            if (DtAlertDetails.Rows.Count > 0)
            {
                txtMobileNo.Text = Convert.ToString(DtAlertDetails.Rows[0]["Mobile_No"]);
                txtEmailID.Text = Convert.ToString(DtAlertDetails.Rows[0]["Email_ID"]);
                txtRespondedBy.Text = Convert.ToString(DtAlertDetails.Rows[0]["User_Name"]);
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }





    private void SetDDLFirstitem(DropDownList DDl, string str)
    {
        try
        {
            for (int i = 0; i < DDl.Items.Count; i++)
            {
                if (DDl.Items[i].Value == str)
                {
                    DDl.SelectedValue = str;
                    return;
                }
                else if (str == "-1")
                {
                    ListItem liSelect = new ListItem("--Select--", "-1");
                    DDl.Items.Insert(0, liSelect);
                    DDl.SelectedIndex = 0;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }



    private void FunPriLoadEnquiryAssignedDetails()
    {
        try
        {
            /*OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3GBusEntity.S3G_Status_Parameters();
            ObjStatus.Option = 161;
            ObjStatus.Param1 = intCompanyId.ToString();
            ObjStatus.Param2 = intUserId.ToString();
            grvEnquiryUpdation.DataSource = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            grvEnquiryUpdation.DataBind();
            ObjCustomerService.Close();*/
            FunPriBindGrid();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private bool ISDuplicateData_Inflow(string Val1, string Val2)
    {
        try
        {
            Boolean Rrn = false;
            DtCashFlow = (DataTable)ViewState["DtCashFlow"];
            if (!string.IsNullOrEmpty(Val1))
            {
                string[] temp = Val1.Split(',');
                //  Utility.FunShowAlertMsg(this,Val2 );
                Val1 = temp[0].ToString();
            }

            for (int Idx = 0; Idx <= DtCashFlow.Rows.Count - 1; Idx++)
            {
                //Utility.FunShowAlertMsg(this, "val1=" + Val1 + "VS1=" + Convert.ToString(DtCashFlow.Rows[Idx]["CashInFlowID"]) + "------" + "val2=" + Val2 + "VS2=" + DateTime.Parse(DtCashFlow.Rows[Idx]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW));
                if ((Val1 == Convert.ToString(DtCashFlow.Rows[Idx]["CashInFlowID"])) && (Val2 == DateTime.Parse(DtCashFlow.Rows[Idx]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW)))
                    Rrn = true;
            }
            return Rrn;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private bool ISDuplicateData_Outflow(string Val1, string Val2)
    {
        try
        {
            Boolean Rrn = false;
            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];
            if (!string.IsNullOrEmpty(Val1))
            {
                string[] temp = Val1.Split(',');
                //  Utility.FunShowAlertMsg(this,Val2 );
                Val1 = temp[0].ToString();
            }

            for (int Idx = 0; Idx <= DtCashFlowOut.Rows.Count - 1; Idx++)
            {
                //Utility.FunShowAlertMsg(this, "val1=" + Val1 + "VS1=" + Convert.ToString(DtCashFlow.Rows[Idx]["CashInFlowID"]) + "------" + "val2=" + Val2 + "VS2=" + DateTime.Parse(DtCashFlow.Rows[Idx]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW));
                if ((Val1 == Convert.ToString(DtCashFlowOut.Rows[Idx]["CashOutFlowID"])) && (Val2 == DateTime.Parse(DtCashFlowOut.Rows[Idx]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW)))
                    Rrn = true;
            }
            return Rrn;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadPaymentDropDown()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            string strProcedureName = "S3g_Org_GetCustomerLookup";

            if (ddlPaymentRuleList.DataSource != null)
            {
                ddlPaymentRuleList.DataSource = null;
                ddlPaymentRuleList.DataBind();
            }

            Procparam.Add("@Option", "42");
            Procparam.Add("@Param1", intCompanyId.ToString());
            if (Convert.ToInt64(ddlProductAssign.SelectedValue) > 0 && Convert.ToInt64(ddlLOBAssign.SelectedValue) > 0)
            {
                Procparam.Add("@Param2", ddlProductAssign.SelectedValue);
                Procparam.Add("@Param3", ddlLOBAssign.SelectedValue);
                ddlPaymentRuleList.BindDataTable(strProcedureName, Procparam, new string[] { "Payment_RuleCard_ID", "Payment_Rule_Number" });
            }
            else if ((!string.IsNullOrEmpty(ViewState["cashlobid"].ToString())) && (!string.IsNullOrEmpty(ViewState["cashproductid"].ToString())))
            {
                Procparam.Add("@Param2", ViewState["cashproductid"].ToString());
                Procparam.Add("@Param3", ViewState["cashlobid"].ToString());
                ddlPaymentRuleList.BindDataTable(strProcedureName, Procparam, new string[] { "Payment_RuleCard_ID", "Payment_Rule_Number" });
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void LoadDetails(int intCustomerId)
    {
        try
        {


            TabContainerER.ActiveTabIndex = 1;
            FunPriToggleTabIndex(true);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunProDisableTabs(int intActiveTab)
    {
        try
        {

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriLoadROIDropDown()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            string strProcedureName = "S3g_Org_GetCustomerLookup";

            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_RATE_TYPE.ToString());
            ddl_Rate_Type.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });
            Procparam.Clear();
            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_RETURN_PATTERN.ToString());
            ddl_Return_Pattern.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });

            Procparam.Clear();
            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_TIME_VALUE.ToString());
            ddl_Time_Value.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });

            Procparam.Clear();
            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_FREQUENCY.ToString());
            ddl_Frequency.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });
            ddl_Interest_Calculation.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });
            ddl_Interest_Levy.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });

            Procparam.Clear();
            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_REPAYMENT_MODE.ToString());
            ddl_Repayment_Mode.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });

            Procparam.Clear();
            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_RESIDUAL_VALUE.ToString());
            ddl_Residual_Value.BindDataTable(strProcedureName, Procparam, false, new string[] { "Value", "Name" });



            Procparam.Clear();
            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_MARGIN.ToString());
            ddl_Margin.BindDataTable(strProcedureName, Procparam, false, new string[] { "Value", "Name" });

            Procparam.Clear();
            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_IRR_REST.ToString());
            ddl_IRR_Rest.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });

            Procparam.Clear();
            Procparam.Add("@Option", "44");
            Procparam.Add("@Param1", S3G_Statu_Lookup.ORG_ROI_RULES_INSURANCE.ToString());
            ddl_Insurance.BindDataTable(strProcedureName, Procparam, new string[] { "Value", "Name" });


            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 34;
            ObjStatus.Param1 = null;
            DtAlertDetails = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["CustomerList"] = DtAlertDetails;

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriClearROIValues()
    {
        try
        {
            tr1.InnerText = tr11.InnerText =
              tr2.InnerText = tr12.InnerText =
              tr3.InnerText = tr13.InnerText =
              tr4.InnerText = tr14.InnerText =
              tr5.InnerText = tr15.InnerText =
              tr6.InnerText = tr16.InnerText =
              tr7.InnerText = tr17.InnerText =
              tr8.InnerText = tr18.InnerText =
              tr9.InnerText = tr19.InnerText =
              tr10.InnerText = tr20.InnerText = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }



    private void FunPriLoadROIDetails(DataTable objROIDetails)
    {
        try
        {
            FunPriLoadROIDropDown();
            if (objROIDetails.Rows.Count > 0)
            {


                lblReturn_Pattern.Visible =
                ddl_Return_Pattern.Visible =
                lblTime_Value.Visible =
                ddl_Time_Value.Visible =
                lblFrequency.Visible =
                ddl_Frequency.Visible =
                lblRepayment_Mode.Visible =
                ddl_Repayment_Mode.Visible =
                lblIRR_Rest.Visible =
                ddl_IRR_Rest.Visible =
                lblInterest_Calculation.Visible =
                ddl_Interest_Calculation.Visible =
                lblInterest_Levy.Visible =
                ddl_Interest_Levy.Visible =
                lblInsurance.Visible =
                ddl_Insurance.Visible =
                lblResidual_Value.Visible =
                ddl_Residual_Value.Visible =
                lblMargin.Visible =
                ddl_Margin.Visible =
                lblMargin_Percentage.Visible =
                txt_Margin_Percentage.Visible = true;
                FunPriShowROIControls(objROIDetails.Rows[0]["Serial_Number"], tr1, txt_Serial_Number);
                FunPriShowROIControls(objROIDetails.Rows[0]["Model_Description"], tr2, txt_Model_Description);
                FunPriShowROIControls(objROIDetails.Rows[0]["Rate_Type"], tr3, ddl_Rate_Type);
                FunPriShowROIControls(objROIDetails.Rows[0]["ROI_Rule_Number"], tr4, txt_ROI_Rule_Number);
                // FunPriShowROIControls(objROIDetails.Rows[0]["Return_Pattern"], tr5, ddl_Return_Pattern);
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Return_Pattern"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Return_Pattern"], tr5, ddl_Return_Pattern);
                else
                {
                    lblReturn_Pattern.Visible = false;
                    ddl_Return_Pattern.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Rate"].ToString()))
                {
                    FunPriShowROIControls(objROIDetails.Rows[0]["Rate"], tr3, txt_Rate);
                    rfvRate.Enabled = true;
                }
                else
                {
                    lblRate.Visible = false;
                    txt_Rate.Visible = false;
                    rfvRate.Enabled = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Time_Value"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Time_Value"], tr6, ddl_Time_Value);
                else
                {
                    lblTime_Value.Visible = false;
                    ddl_Time_Value.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Frequency"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Frequency"], tr7, ddl_Frequency);
                else
                {
                    lblFrequency.Visible = false;
                    ddl_Frequency.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Repayment_Mode"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Repayment_Mode"], tr8, ddl_Repayment_Mode);
                else
                {
                    lblRepayment_Mode.Visible = false;
                    ddl_Repayment_Mode.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["IRR_Rest"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["IRR_Rest"], tr9, ddl_IRR_Rest);
                else
                {
                    lblIRR_Rest.Visible = false;
                    ddl_IRR_Rest.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Interest_Calculation"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Interest_Calculation"], tr10, ddl_Interest_Calculation);
                else
                {
                    lblInterest_Calculation.Visible = false;
                    ddl_Interest_Calculation.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Interest_Levy"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Interest_Levy"], tr11, ddl_Interest_Levy);
                else
                {
                    lblInterest_Levy.Visible = false;
                    ddl_Interest_Levy.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Recovery_Pattern_Year1"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Recovery_Pattern_Year1"], tr12, txt_Recovery_Pattern_Year1);
                else
                {
                    lblRecovery_Pattern_Year1.Visible = false;
                    txt_Recovery_Pattern_Year1.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Recovery_Pattern_Year2"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Recovery_Pattern_Year2"], tr13, txt_Recovery_Pattern_Year2);
                else
                {
                    lblRecovery_Pattern_Year2.Visible = false;
                    txt_Recovery_Pattern_Year2.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Recovery_Pattern_Year3"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Recovery_Pattern_Year3"], tr14, txt_Recovery_Pattern_Year3);
                else
                {
                    lblRecovery_Pattern_Year3.Visible = false;
                    txt_Recovery_Pattern_Year3.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Recovery_Pattern_Rest"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Recovery_Pattern_Rest"], tr15, txt_Recovery_Pattern_Rest);
                else
                {
                    lblRecovery_Pattern_Rest.Visible = false;
                    txt_Recovery_Pattern_Rest.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Insurance"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Insurance"], tr16, ddl_Insurance);
                else
                {
                    lblInsurance.Visible = false;
                    ddl_Insurance.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Residual_Value"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Residual_Value"], tr17, ddl_Residual_Value);
                else
                {
                    lblResidual_Value.Visible = false;
                    ddl_Residual_Value.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Margin"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Margin"], tr18, ddl_Margin);
                else
                {
                    lblMargin.Visible = false;
                    ddl_Margin.Visible = false;
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Margin_Percentage"].ToString()))
                    FunPriShowROIControls(objROIDetails.Rows[0]["Margin_Percentage"], tr19, txt_Margin_Percentage);
                else
                {
                    lblMargin_Percentage.Visible = false;
                    txt_Margin_Percentage.Visible = false;
                }
                txtMarginMoneyPer_Cashflow.ReadOnly = true;
                txtMarginMoneyAmount_Cashflow.ReadOnly = true;
                rfvMarginPercent.Enabled = false;
                txtResidualAmt_Cashflow.ReadOnly = true;
                txtResidualValue_Cashflow.ReadOnly = true;
                txtMarginMoneyPer_Cashflow.Text = "";
                txtMarginMoneyAmount_Cashflow.Text = "";
                txtResidualAmt_Cashflow.Text = "";
                txtResidualValue_Cashflow.Text = "";
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Margin"].ToString()))
                {
                    if (objROIDetails.Rows[0]["Margin"].ToString() == "1")
                    {
                        txtMarginMoneyPer_Cashflow.Text = objROIDetails.Rows[0]["Margin_Percentage"].ToString();
                        txtMarginMoneyAmount_Cashflow.Text = Convert.ToString(FunPriGetMarginAmout());

                        txtMarginMoneyPer_Cashflow.ReadOnly = true;
                        txtMarginMoneyAmount_Cashflow.ReadOnly = true;
                        rfvMarginPercent.Enabled = true;

                    }
                    else
                    {
                        txtMarginMoneyPer_Cashflow.Text = "";
                        // txtMarginAmountAsset.ReadOnly = true;
                        txtMarginMoneyPer_Cashflow.ReadOnly = true;
                        txtMarginMoneyAmount_Cashflow.ReadOnly = true;
                        rfvMarginPercent.Enabled = false;

                    }
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["Residual_Value"].ToString()))
                {
                    if (objROIDetails.Rows[0]["Residual_Value"].ToString() == "1")
                    {

                        txtResidualAmt_Cashflow.ReadOnly = false;
                        txtResidualValue_Cashflow.ReadOnly = false;
                        if (txtResidualvalue.Text == "" || txtResidualvalue.Text == "0" || txtResidualvalue.Text == string.Empty)
                        {
                            RFVresidualvalue.Enabled = true;
                        }
                        else
                        {
                            txtResidualAmt_Cashflow.Text = txtResidualvalue.Text;
                            RFVresidualvalue.Enabled = false;
                            txtResidualValue_Cashflow.ReadOnly = true;
                        }
                    }
                    else
                    {
                        RFVresidualvalue.Enabled = false;
                        txtResidualAmt_Cashflow.ReadOnly = true;
                        txtResidualValue_Cashflow.ReadOnly = true;
                        if (txtResidualvalue.Text != "" || txtResidualvalue.Text != "0")
                        {
                            txtResidualAmt_Cashflow.Text = txtResidualvalue.Text;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(objROIDetails.Rows[0]["IRR_Rate"].ToString()))
                {
                    if (Convert.ToDecimal(objROIDetails.Rows[0]["IRR_Rate"].ToString()) > 0)
                        ViewState["decRate"] = objROIDetails.Rows[0]["IRR_Rate"].ToString();
                }
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }





    private void FunPriShowPaymentDetails(DataTable ObjDTPayment)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            string vendor = "";
            if (ObjDTPayment.Rows.Count > 0)
                vendor = ObjDTPayment.Rows[0]["Entity_Type"].ToString().ToLower();
            S3GBusEntity.S3G_Status_Parameters ObjStatus1 = new S3G_Status_Parameters();

            ObjStatus1.Option = 1;
            ObjStatus1.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();
            if (gvOutFlow != null)
            {
                if (gvOutFlow.FooterRow != null)
                {
                    DropDownList ddlEntityName_InFlowFrom = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
                    DataTable dtCashFlowFrom = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus1);
                    switch (vendor)
                    {
                        case "vendor":
                            dtCashFlowFrom.Rows.RemoveAt(0);
                            break;
                        case "customer":
                            dtCashFlowFrom.Rows.RemoveAt(1);
                            break;
                    }
                    ViewState["CashFlowTo"] = dtCashFlowFrom;
                    Utility.FillDLL(ddlEntityName_InFlowFrom, dtCashFlowFrom, true);
                }
            }
            DataTable ObjDTPaymentGen = new DataTable();
            DataColumn dc1 = new DataColumn("FieldName");
            DataColumn dc2 = new DataColumn("FieldValue");
            ObjDTPaymentGen.Columns.Add(dc1);
            ObjDTPaymentGen.Columns.Add(dc2);
            ViewState["PaymentRules"] = ObjDTPaymentGen;
            gvPaymentRuleDetails.DataSource = null;
            gvPaymentRuleDetails.DataBind();


            if (ObjDTPayment.Rows.Count > 0)
            {
                for (int i = 0; i < ObjDTPayment.Columns.Count; i++)
                {
                    if (Convert.ToString(ObjDTPayment.Rows[0][i]) != string.Empty)
                    {
                        if (!ObjDTPayment.Columns[i].ColumnName.Contains("_ID"))
                        {
                            DataRow dr = ObjDTPaymentGen.NewRow();
                            dr[0] = ObjDTPayment.Columns[i].ColumnName.Replace("_", " ");
                            if (ObjDTPayment.Rows.Count > 0) dr[1] = ObjDTPayment.Rows[0][i].ToString();
                            else dr[1] = string.Empty;
                            ObjDTPaymentGen.Rows.Add(dr);
                        }
                    }
                }
                gvPaymentRuleDetails.DataSource = ObjDTPaymentGen;
                gvPaymentRuleDetails.DataBind();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }
    private void FunPriShowROIControls(Object Data, HtmlTableRow tr, Object ObjCtl)
    {
        try
        {
            bool blnIsRowEnable = false;
            if (!string.IsNullOrEmpty(Convert.ToString(Data)))
            {
                //if (PaintBG)
                //{
                //    if (this.Theme == "S3GTheme_Silver")
                //    {
                //        tr.BgColor = "#d6d7e0";
                //    }
                //    else
                //    {
                //        tr.BgColor = "aliceblue";
                //    }
                //    PaintBG = false;

                //}
                //else
                //{
                //    PaintBG = true;
                //}
                tr.Visible = true;

                if (ObjCtl.GetType().Name == "TextBox")
                {
                    //((TextBox)ObjCtl).Text = Convert.ToString(Data);
                    TextBox txtBox = ((TextBox)ObjCtl);
                    txtBox.Text = Convert.ToString(Data);
                    if (txtBox.ID == "txt_Rate" || txtBox.ID == "txt_Margin_Percentage")
                        blnIsRowEnable = true;
                }
                else if (ObjCtl.GetType().Name == "DropDownList")
                {
                    /* DropDownList DDL = new DropDownList();
                     DDL = ((DropDownList)ObjCtl);
                     DDL.SelectedValue = Convert.ToString(Data);*/
                    DropDownList DDL = new DropDownList();

                    DDL = ((DropDownList)ObjCtl);

                    if (DDL.ID == "ddl_Time_Value" || DDL.ID == "ddl_Frequency" || DDL.ID == "ddl_Insurance")
                        blnIsRowEnable = true;

                    if (DDL.Items.Count > 0)
                    {
                        if (Convert.ToString(Data) != "0")
                            DDL.SelectedValue = Convert.ToString(Data);
                        if (DDL.ID == "ddl_Residual_Value")
                        {
                            DDL.SelectedValue = Convert.ToString(Data);
                        }
                    }

                }
                else if (ObjCtl.GetType().Name == "CheckBox")
                {
                    ((CheckBox)ObjCtl).Checked = Convert.ToBoolean(Data);
                }
                if (ddlROIRuleList.SelectedItem.Text.ToUpper().Contains("RRA"))//ROI Rule number selected in drop down
                {
                    // tr.Disabled = true;
                    WebControl ctrl = (WebControl)ObjCtl;
                    ctrl.Enabled = false;
                }
                else
                {
                    // tr.Disabled = false;
                    if (blnIsRowEnable)
                    {
                        WebControl ctrl = (WebControl)ObjCtl;
                        ctrl.Enabled = true;
                    }
                    else
                    {
                        WebControl ctrl = (WebControl)ObjCtl;
                        ctrl.Enabled = false;
                    }
                }
            }
            else
            {
                tr.Visible = false;
            }


        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void ShowRIODetails(DataTable ObjDt)
    {
        try
        {
            DataTable ObjDTPaymentGen = new DataTable();
            DataColumn dc1 = new DataColumn("FieldName");
            DataColumn dc2 = new DataColumn("FieldValue");
            ObjDTPaymentGen.Columns.Add(dc1);
            ObjDTPaymentGen.Columns.Add(dc2);

            for (int i = 0; i < ObjDt.Columns.Count; i++)
            {
                DataRow dr = ObjDTPaymentGen.NewRow();
                dr[0] = ObjDt.Columns[i].ColumnName.Replace("_", " ");

                if (ObjDt.Rows.Count > 0)
                    dr[1] = ObjDt.Rows[0][i].ToString();
                else
                    dr[1] = string.Empty;
                ObjDTPaymentGen.Rows.Add(dr);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void SetWhiteSpaceDLL(DropDownList ObjDLL)
    {
        try
        {
            if (ObjDLL.Items.Count == 0)
            {
                ListItem liSelect = new ListItem("", "-1");
                ObjDLL.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SetDDLFirstitem(DropDownList DDl)
    {
        try
        {
            DDl.SelectedValue = "-1";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void SetSelectItem_DLL(DropDownList ObjDrop, string str)
    {
        try
        {
            if (!string.IsNullOrEmpty(str))
            {
                //for (int i = 0; i < ObjDrop.Items.Count; i++)
                //{
                //    if (ObjDrop.Items[i].ToString() == str)
                //    {
                //        ObjDrop.SelectedIndex = i;
                //    }                    
                //}
                ObjDrop.SelectedValue = str;
                ObjDrop.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }



    private void FunPriBindCashFlowDetails()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 310;
            ObjStatus.Param1 = intCompanyId.ToString();
            if (ddlLOBAssign.SelectedIndex > 0)
                ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
            else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                ObjStatus.Param2 = ViewState["cashlobid"].ToString();

            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["CashInflowList"] = DtCashFlow;

            ObjStatus.Option = 170;
            ObjStatus.Param1 = intCompanyId.ToString();
            if (ddlLOBAssign.SelectedIndex > 0)
                ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
            else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                ObjStatus.Param2 = ViewState["cashlobid"].ToString();
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["RepayCashInflowList"] = DtCashFlow;
            DtCashFlow.Dispose();

            ObjStatus.Option = 311;
            ObjStatus.Param1 = intCompanyId.ToString();
            if (ddlLOBAssign.SelectedIndex > 0)
                ObjStatus.Param2 = ddlLOBAssign.SelectedValue.ToString();
            else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
                ObjStatus.Param2 = ViewState["cashlobid"].ToString();

            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["CashOutflowList"] = DtCashFlow;

            ObjStatus.Option = 38;
            ObjStatus.Param1 = intCompanyId.ToString();
            DtFollowUp = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["EntityMasterList"] = DtFollowUp;

            ObjStatus.Option = 6;
            ObjStatus.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();
            ViewState["CashFlowFrom"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


            ObjStatus.Option = 151;
            ObjStatus.Param1 = null;
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();

            DtCashFlow.Rows.Clear();
            ViewState["DtCashFlow"] = DtCashFlow;
            DtCashFlow.Dispose();

            gvInflow.Rows[0].Cells.Clear();
            gvInflow.Rows[0].Visible = false;

            //Out flow grid
            ObjStatus.Option = 152;
            DtCashFlowOut = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            gvOutFlow.DataSource = DtCashFlowOut;
            gvOutFlow.DataBind();

            DtCashFlowOut.Rows.Clear();
            ViewState["DtCashFlowOut"] = DtCashFlowOut;
            DtCashFlowOut.Dispose();

            gvOutFlow.Rows[0].Cells.Clear();
            gvOutFlow.Rows[0].Visible = false;

            TextBox txtDate_GridInflow1 = gvInflow.FooterRow.FindControl("txtDate_GridInflow") as TextBox;
            txtDate_GridInflow1.Attributes.Add("readonly", "readonly");

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_InflowDate1 = gvInflow.FooterRow.FindControl("CalendarExtenderSD_InflowDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_InflowDate1.Format = strDateFormat;

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_OutflowDate1 = gvOutFlow.FooterRow.FindControl("CalendarExtenderSD_OutflowDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_OutflowDate1.Format = strDateFormat;

            FillDataFrom_ViewState_CashInflow();


            FillDataFrom_ViewState_CashOutflow();




        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FillDataFrom_ViewState_CashInflow()
    {
        try
        {
            TextBox txtDate_GridInflow2 = gvInflow.FooterRow.FindControl("txtDate_GridInflow") as TextBox;
            txtDate_GridInflow2.Attributes.Add("readonly", "readonly");

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_InflowDate1 = gvInflow.FooterRow.FindControl("CalendarExtenderSD_InflowDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_InflowDate1.Format = strDateFormat;

            DropDownList ddlInflowDesc = gvInflow.FooterRow.FindControl("ddlInflowDesc") as DropDownList;
            DropDownList ddlEntityName_InFlow = gvInflow.FooterRow.FindControl("ddlEntityName_InFlow") as DropDownList;
            DropDownList ddlEntityName_InFlowFrom = gvInflow.FooterRow.FindControl("ddlEntityName_InFlowFrom") as DropDownList;

            DataTable dtInflowlist = (DataTable)ViewState["CashInflowList"];
            int flag_id = 0;

            var varquery = from Cashflow in dtInflowlist.AsEnumerable()
                           where Cashflow.Field<string>("CashFlow_Id").Substring(Cashflow.Field<string>("CashFlow_Id").Length - 2, 2) != "34"
                           select Cashflow;
            if (varquery.Count() > 0)
            {
                dtInflowlist = varquery.CopyToDataTable();

                Utility.FillDLL(ddlInflowDesc, dtInflowlist, true);
                SetWhiteSpaceDLL(ddlInflowDesc);
            }



            Utility.FillDLL(ddlEntityName_InFlowFrom, ((DataTable)ViewState["CashFlowFrom"]), true);
            SetWhiteSpaceDLL(ddlEntityName_InFlowFrom);

            Utility.FillDLL(ddlEntityName_InFlow, ((DataTable)ViewState["EntityMasterList"]), true);
            SetWhiteSpaceDLL(ddlEntityName_InFlow);

            int count = 0;

            DtCashFlow = (DataTable)ViewState["DtCashFlow"];

            foreach (GridViewRow gvr in gvInflow.Rows)
            {
                TextBox txtDate_GridInflow1 = gvr.FindControl("txtDate_GridInflow") as TextBox;
                DropDownList ddlInflowDesc1 = gvr.FindControl("ddlInflowDesc") as DropDownList;
                DropDownList ddlEntityName_InFlow1 = gvr.FindControl("ddlEntityName_InFlow") as DropDownList;
                TextBox txtAmount_Inflow1 = gvr.FindControl("txtAmount_Inflow") as TextBox;

                if (ddlInflowDesc1 != null)
                {
                    Utility.FillDLL(ddlInflowDesc1, ((DataTable)ViewState["CashInflowList"]), false);
                    Utility.FillDLL(ddlEntityName_InFlowFrom, ((DataTable)ViewState["CashFlowFrom"]), true);
                    Utility.FillDLL(ddlEntityName_InFlow1, ((DataTable)ViewState["EntityMasterList"]), false);



                    txtDate_GridInflow1.Text = DateTime.Parse(DtCashFlow.Rows[count]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    SetSelectItem_DLL(ddlInflowDesc1, DtCashFlow.Rows[count]["CashInFlowID"].ToString());
                    SetSelectItem_DLL(ddlEntityName_InFlow1, DtCashFlow.Rows[count]["EntityID"].ToString());
                    txtAmount_Inflow1.Text = DtCashFlow.Rows[count]["Amount"].ToString();

                    txtDate_GridInflow1.ReadOnly = true;
                    txtAmount_Inflow1.ReadOnly = true;
                }
                count = count + 1;
            }
            // ddlEntityName_InFlowFrom.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FillDataFrom_ViewState_CashOutflow()
    {
        try
        {
            TextBox txtDate_GridOutflow2 = gvOutFlow.FooterRow.FindControl("txtDate_GridOutflow") as TextBox;
            txtDate_GridOutflow2.Attributes.Add("readonly", "readonly");

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_OutflowDate1 = gvOutFlow.FooterRow.FindControl("CalendarExtenderSD_OutflowDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_OutflowDate1.Format = strDateFormat;

            DropDownList ddlInflowDesc = gvOutFlow.FooterRow.FindControl("ddlOutflowDesc") as DropDownList;
            DropDownList ddlEntityName_OutFlow = gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow") as DropDownList;
            DropDownList ddlPaymentto_OutFlow = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
            Utility.FillDLL(ddlInflowDesc, ((DataTable)ViewState["CashOutflowList"]), true);
            SetWhiteSpaceDLL(ddlInflowDesc);

            Utility.FillDLL(ddlPaymentto_OutFlow, ((DataTable)ViewState["CashFlowFrom"]), true);
            SetWhiteSpaceDLL(ddlPaymentto_OutFlow);

            Utility.FillDLL(ddlEntityName_OutFlow, ((DataTable)ViewState["EntityMasterList"]), true);
            SetWhiteSpaceDLL(ddlEntityName_OutFlow);


            int count = 0;

            DtCashFlowOut = (DataTable)ViewState["DtCashFlowOut"];

            foreach (GridViewRow gvr in gvOutFlow.Rows)
            {
                TextBox txtDate_GridOutflow = gvr.FindControl("txtDate_GridOutflow") as TextBox;
                DropDownList ddlOutflowDesc = gvr.FindControl("ddlOutflowDesc") as DropDownList;
                DropDownList ddlEntityNameOutFlow = gvr.FindControl("ddlPaymentto_OutFlow") as DropDownList;
                TextBox txtAmount_Outflow = gvr.FindControl("txtAmount_Outflow") as TextBox;

                if (ddlOutflowDesc != null)
                {
                    Utility.FillDLL(ddlOutflowDesc, ((DataTable)ViewState["CashOutflowList"]), false);
                    Utility.FillDLL(ddlPaymentto_OutFlow, ((DataTable)ViewState["CashFlowFrom"]), true);
                    Utility.FillDLL(ddlEntityNameOutFlow, ((DataTable)ViewState["EntityMasterList"]), false);

                    //txtDate_GridOutflow.Text = DtCashFlowOut.Rows[count]["Date"].ToString();
                    txtDate_GridOutflow.Text = DateTime.Parse(DtCashFlowOut.Rows[count]["Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    SetSelectItem_DLL(ddlOutflowDesc, DtCashFlowOut.Rows[count]["CashOutFlowID"].ToString());
                    SetSelectItem_DLL(ddlEntityNameOutFlow, DtCashFlowOut.Rows[count]["EntityID"].ToString());
                    txtAmount_Outflow.Text = DtCashFlowOut.Rows[count]["Amount"].ToString();

                    txtDate_GridOutflow.ReadOnly = true;
                    txtAmount_Outflow.ReadOnly = true;
                }
                count = count + 1;
            }
            //ddlPaymentto_OutFlow.SelectedIndex = 1;

            DropDownList ddlEntityName_InFlowFrom = gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow") as DropDownList;
            if (ViewState["CashFlowTo"] != null)
            {
                DataTable dtCashFlowFrom = (DataTable)ViewState["CashFlowTo"];
                string vendor = (string)ViewState["vendor"];
                if (dtCashFlowFrom.Rows.Count > 1)
                {
                    switch (vendor)
                    {
                        case "vendor":
                            dtCashFlowFrom.Rows.RemoveAt(0);
                            break;
                        case "customer":
                            dtCashFlowFrom.Rows.RemoveAt(1);
                            break;
                    }
                    ViewState["CashFlowTo"] = dtCashFlowFrom;
                }
                Utility.FillDLL(ddlEntityName_InFlowFrom, dtCashFlowFrom, true);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }



    private void FunPriBindRepaymentDetails(string ResponseID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            if (ResponseID == "0")
            {
                ObjStatus.Option = 52;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                intSerialNo = 0;
                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();

                DtRepayGrid.Rows.Clear();
                ViewState["DtRepayGrid"] = DtRepayGrid;

                gvRepaymentDetails.Rows[0].Cells.Clear();
                gvRepaymentDetails.Rows[0].Visible = false;

                RepaymentSummaryGrid(ResponseID);
            }
            else
            {
                ObjStatus.Option = 159;
                ObjStatus.Param1 = ResponseID;
                DtRepayGrid = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                intSerialNo = 0;
                gvRepaymentDetails.DataSource = DtRepayGrid;
                gvRepaymentDetails.DataBind();

                ViewState["DtRepayGrid"] = DtRepayGrid;

                RepaymentSummaryGrid(ResponseID);
            }

            if (gvRepaymentDetails.Rows.Count > 0)
            {
                FunPriBindRepaymentCashflowDetails();

                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;

                CalendarExtenderSD_fromdate_RepayTab1.Format = strDateFormat;
                CalendarExtenderSD_ToDate_RepayTab1.Format = strDateFormat;

                TextBox txtfromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtfromdate_RepayTab") as TextBox;
                TextBox txtToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("txtToDate_RepayTab") as TextBox;

                txtfromdate_RepayTab1.Attributes.Add("readonly", "readonly");
                txtToDate_RepayTab1.Attributes.Add("readonly", "readonly");
            }



        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriBindRepaymentCashflowDetails()
    {
        try
        {
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_fromdate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_fromdate_RepayTab") as AjaxControlToolkit.CalendarExtender;
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_ToDate_RepayTab1 = gvRepaymentDetails.FooterRow.FindControl("CalendarExtenderSD_ToDate_RepayTab") as AjaxControlToolkit.CalendarExtender;

            CalendarExtenderSD_fromdate_RepayTab1.Format = strDateFormat;
            CalendarExtenderSD_ToDate_RepayTab1.Format = strDateFormat;

            DropDownList ddlRepaymentCashFlow_RepayTab = gvRepaymentDetails.FooterRow.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
            Utility.FillDLL(ddlRepaymentCashFlow_RepayTab, ((DataTable)ViewState["RepayCashInflowList"]), true);
            SetWhiteSpaceDLL(ddlRepaymentCashFlow_RepayTab);

            int count = 0;

            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];

            foreach (GridViewRow gvr in gvRepaymentDetails.Rows)
            {
                DropDownList ddlRepaymentCashFlow_RepayTab1 = gvr.FindControl("ddlRepaymentCashFlow_RepayTab") as DropDownList;
                TextBox txtfromdate_RepayTab1 = gvr.FindControl("txtfromdate_RepayTab") as TextBox;
                TextBox txtToDate_RepayTab1 = gvr.FindControl("txtToDate_RepayTab") as TextBox;


                if (ddlRepaymentCashFlow_RepayTab1 != null)
                {
                    txtfromdate_RepayTab1.Text = DateTime.Parse(DtRepayGrid.Rows[count]["FromDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    txtToDate_RepayTab1.Text = DateTime.Parse(DtRepayGrid.Rows[count]["ToDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);

                    Utility.FillDLL(ddlRepaymentCashFlow_RepayTab1, ((DataTable)ViewState["RepayCashInflowList"]), false);
                    SetSelectItem_DLL(ddlRepaymentCashFlow_RepayTab1, DtRepayGrid.Rows[count]["CashFlow"].ToString());
                    ddlRepaymentCashFlow_RepayTab1.Enabled = false;
                }
                count = count + 1;
            }

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }





    private void RepaymentSummaryGrid(string ResponseID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 168;
            ObjStatus.Param1 = ResponseID;
            DtRepaySummary = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            gvRepaymentSummary.DataSource = DtRepaySummary;
            gvRepaymentSummary.DataBind();

            ViewState["RepaymentSummary"] = DtRepaySummary;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void DoSummaryAmount(string FlowDesc, Int64 Amt)
    {
        try
        {
            int ISData = 0;

            DtRepaySummary = (DataTable)ViewState["RepaymentSummary"];

            if (DtRepaySummary.Rows.Count == 0)
                ISData++;

            for (int Idx = 0; Idx <= DtRepaySummary.Rows.Count - 1; Idx++)
            {
                if (Convert.ToString(DtRepaySummary.Rows[Idx]["CashFlow_Description"]).ToUpper() == FlowDesc.ToUpper())
                {
                    DtRepaySummary.Rows[Idx]["Amount"] = Convert.ToInt64(DtRepaySummary.Rows[Idx]["Amount"]) + Amt;
                    DtRepaySummary.AcceptChanges();

                    gvRepaymentSummary.DataSource = DtRepaySummary;
                    gvRepaymentSummary.DataBind();
                    ViewState["RepaymentSummary"] = DtRepaySummary;

                    return;
                }
                else
                {
                    ISData++;
                }
            }

            if (ISData > 0)
            {
                DataRow dr = DtRepaySummary.NewRow();
                dr["CashFlow_Description"] = FlowDesc;
                dr["Amount"] = Amt;
                DtRepaySummary.Rows.Add(dr);

                gvRepaymentSummary.DataSource = DtRepaySummary;
                gvRepaymentSummary.DataBind();
                ViewState["RepaymentSummary"] = DtRepaySummary;
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void LessingSummaryAmount(int RowIdx)
    {
        try
        {
            string FlowDesc = string.Empty;
            DataTable DtRepaydetails = new DataTable();
            DtRepaydetails = (DataTable)ViewState["DtRepayGrid"];
            DtRepaySummary = (DataTable)ViewState["RepaymentSummary"];
            FlowDesc = Convert.ToString(DtRepaydetails.Rows[RowIdx]["CashFlow"]);

            for (int Idx = 0; Idx <= DtRepaySummary.Rows.Count - 1; Idx++)
            {
                if (FlowDesc == Convert.ToString(DtRepaySummary.Rows[Idx]["CashFlow_Description"]))
                {
                    if (Convert.ToInt64(DtRepaySummary.Rows[Idx]["Amount"]) == Convert.ToInt64(DtRepaydetails.Rows[RowIdx]["Amount"]))
                    {
                        DtRepaySummary.Rows.RemoveAt(Idx);
                        gvRepaymentSummary.DataSource = DtRepaySummary;
                        gvRepaymentSummary.DataBind();
                        ViewState["RepaymentSummary"] = DtRepaySummary;
                        //  return;
                    }
                    else
                    {
                        DtRepaySummary.Rows[Idx]["Amount"] = Convert.ToInt64(DtRepaySummary.Rows[Idx]["Amount"]) - Convert.ToInt64(DtRepaydetails.Rows[RowIdx]["Amount"]);
                        DtRepaySummary.AcceptChanges();
                        gvRepaymentSummary.DataSource = DtRepaySummary;
                        gvRepaymentSummary.DataBind();
                        ViewState["RepaymentSummary"] = DtRepaySummary;
                        //return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriBindAlertDetails()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.ALERT_TYPE.ToString();
            //Commented and added by saranya on 08-Mar-2012 based on sudarsan observation
            DataTable dtAlertType = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            DataRow[] dr = dtAlertType.Select("ID in(141,216,142,217,143,218)");
            dtAlertType = dr.CopyToDataTable();
            ViewState["Alert_Type"] = dtAlertType; //ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            //End Here

            ObjStatus.Option = 35;
            ObjStatus.Param1 = intCompanyId.ToString();
            ViewState["UserList"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


            DataTable ObjDT = new DataTable();
            ObjStatus.Option = 46;
            ObjStatus.Param1 = null;
            ObjDT = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            gvAlert.DataSource = ObjDT;
            gvAlert.DataBind();

            ObjDT.Rows.Clear();
            ViewState["DtAlertDetails"] = ObjDT;

            gvAlert.Rows[0].Cells.Clear();
            gvAlert.Rows[0].Visible = false;

            FillDataFrom_ViewState();
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FillDataFrom_ViewState()
    {
        try
        {
            //Button ObjddlContact_AlertTab2 = gvAlert.FooterRow.FindControl("LbtnAddAlert") as Button;
            //CheckBox ChkEmail2 = gvAlert.FooterRow.FindControl("ChkEmail") as CheckBox;
            //CheckBox ChkSMS2 = gvAlert.FooterRow.FindControl("ChkSMS") as CheckBox;
            //ObjddlContact_AlertTab2.Attributes.Add("onclick", "return fnValidateEMail('" + ChkEmail2.ClientID + "','" + ChkSMS2.ClientID + "')");

            DropDownList ObjddlType_AlertTab = gvAlert.FooterRow.FindControl("ddlType_AlertTab") as DropDownList;
            DropDownList ObjddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as DropDownList;

            Utility.FillDLL(ObjddlType_AlertTab, ((DataTable)ViewState["Alert_Type"]), true);
            Utility.FillDLL(ObjddlContact_AlertTab, ((DataTable)ViewState["UserList"]), true);

            int count = 0;

            DtAlertDetails = (DataTable)ViewState["DtAlertDetails"];

            foreach (GridViewRow gvr in gvAlert.Rows)
            {
                DropDownList ObjddlType_AlertTab1 = gvr.FindControl("ddlType_AlertTab") as DropDownList;
                DropDownList ObjddlContact_AlertTab1 = gvr.FindControl("ddlContact_AlertTab") as DropDownList;
                CheckBox ChkEmail = gvr.FindControl("ChkEmail") as CheckBox;
                CheckBox ChkSMS = gvr.FindControl("ChkSMS") as CheckBox;

                if (ObjddlContact_AlertTab1 != null)
                {
                    Utility.FillDLL(ObjddlType_AlertTab1, ((DataTable)ViewState["Alert_Type"]), false);
                    Utility.FillDLL(ObjddlContact_AlertTab1, ((DataTable)ViewState["UserList"]), false);

                    SetSelectItem_DLL(ObjddlType_AlertTab1, DtAlertDetails.Rows[count]["Type"].ToString());
                    SetSelectItem_DLL(ObjddlContact_AlertTab1, DtAlertDetails.Rows[count]["UserContact"].ToString());

                    ObjddlType_AlertTab1.Enabled = false;
                    ObjddlContact_AlertTab1.Enabled = false;
                    ChkEmail.Enabled = false;
                    ChkSMS.Enabled = false;
                }

                count = count + 1;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadAlerts(string ResponseID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            if (ResponseID != "0")
            {
                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

                DataTable ObjDT = new DataTable();
                ObjStatus.Option = 160;
                ObjStatus.Param1 = ResponseID;
                ObjDT = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                strAlertDetails = ObjDT.FunPubFormXml();
                gvAlert.DataSource = ObjDT;
                gvAlert.DataBind();

                ViewState["DtAlertDetails"] = ObjDT;

                FillDataFrom_ViewState();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriBindFollowupDetails()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();


            ObjStatus.Option = 35;
            ObjStatus.Param1 = intCompanyId.ToString();
            ViewState["UserListFolloup"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


            DataTable ObjDT = new DataTable();
            ObjStatus.Option = 47;
            ObjStatus.Param1 = null;
            ObjDT = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            gvFollowUp.DataSource = ObjDT;
            gvFollowUp.DataBind();

            ObjDT.Rows.Clear();
            ViewState["DtFollowUp"] = ObjDT;

            gvFollowUp.Rows[0].Cells.Clear();
            gvFollowUp.Rows[0].Visible = false;

            FillDataFrom_ViewState_FollowUp();

            TextBox txttxtDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtDate_GridFollowup") as TextBox;
            TextBox txtActionDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtActionDate_GridFollowup") as TextBox;

            TextBox txtAction_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtAction_GridFollowup") as TextBox;
            TextBox txtCustomerResponse_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtCustomerResponse_GridFollowup") as TextBox;
            TextBox txtRemarks_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtRemarks_GridFollowup") as TextBox;

            txtAction_GridFollowup1.Attributes.Add("onkeypress", "wraptext(" + txtAction_GridFollowup1.ClientID + ",20);");
            txtCustomerResponse_GridFollowup1.Attributes.Add("onkeypress", "wraptext(" + txtCustomerResponse_GridFollowup1.ClientID + ",20);");
            txtRemarks_GridFollowup1.Attributes.Add("onkeypress", "wraptext(" + txtRemarks_GridFollowup1.ClientID + ",20);");

            AjaxControlToolkit.CalendarExtender txtDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
            txtDate_GridFollowup1.Format = strDateFormat;

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupActionDate1 = gvFollowUp.FooterRow.FindControl("CalendarExtenderSD_FollowupActionDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_FollowupActionDate1.Format = strDateFormat;

            txttxtDate_GridFollowup1.Attributes.Add("readonly", "readonly");
            txtActionDate_GridFollowup1.Attributes.Add("readonly", "readonly");
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FillDataFrom_ViewState_FollowUp()
    {
        try
        {
            if (gvFollowUp.Rows.Count == 0)
            {
                FunPriBindFollowupDetails();
                return;
            }

            TextBox txtAction_GridFollowup2 = gvFollowUp.FooterRow.FindControl("txtAction_GridFollowup") as TextBox;
            TextBox txtCustomerResponse_GridFollowup2 = gvFollowUp.FooterRow.FindControl("txtCustomerResponse_GridFollowup") as TextBox;
            TextBox txtRemarks_GridFollowup2 = gvFollowUp.FooterRow.FindControl("txtRemarks_GridFollowup") as TextBox;

            txtAction_GridFollowup2.Attributes.Add("onkeypress", "wraptext(" + txtAction_GridFollowup2.ClientID + ",20);");
            txtCustomerResponse_GridFollowup2.Attributes.Add("onkeypress", "wraptext(" + txtCustomerResponse_GridFollowup2.ClientID + ",20);");
            txtRemarks_GridFollowup2.Attributes.Add("onkeypress", "wraptext(" + txtRemarks_GridFollowup2.ClientID + ",20);");

            AjaxControlToolkit.CalendarExtender txtDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
            txtDate_GridFollowup1.Format = strDateFormat;

            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupActionDate1 = gvFollowUp.FooterRow.FindControl("CalendarExtenderSD_FollowupActionDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_FollowupActionDate1.Format = strDateFormat;

            DropDownList ddlfrom_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as DropDownList;
            DropDownList ddlTo_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as DropDownList;

            Utility.FillDLL(ddlfrom_GridFollowup, ((DataTable)ViewState["UserListFolloup"]), true);
            Utility.FillDLL(ddlTo_GridFollowup, ((DataTable)ViewState["UserListFolloup"]), true);

            int count = 0;

            DtFollowUp = (DataTable)ViewState["DtFollowUp"];

            foreach (GridViewRow gvr in gvFollowUp.Rows)
            {
                TextBox txttxtDate_GridFollowup1 = gvr.FindControl("txtDate_GridFollowup") as TextBox;
                DropDownList ObjddlFrom_FollowTab1 = gvr.FindControl("ddlfrom_GridFollowup") as DropDownList;
                DropDownList ObjddlTo_FollowTab1 = gvr.FindControl("ddlTo_GridFollowup") as DropDownList;
                TextBox txtAction_GridFollowup1 = gvr.FindControl("txtAction_GridFollowup") as TextBox;
                TextBox txtActionDate_GridFollowup1 = gvr.FindControl("txtActionDate_GridFollowup") as TextBox;
                TextBox txtCustomerResponse_GridFollowup1 = gvr.FindControl("txtCustomerResponse_GridFollowup") as TextBox;
                TextBox txtRemarks_GridFollowup1 = gvr.FindControl("txtRemarks_GridFollowup") as TextBox;

                if (ObjddlFrom_FollowTab1 != null)
                {
                    Utility.FillDLL(ObjddlFrom_FollowTab1, ((DataTable)ViewState["UserListFolloup"]), false);
                    Utility.FillDLL(ObjddlTo_FollowTab1, ((DataTable)ViewState["UserListFolloup"]), false);

                    //txttxtDate_GridFollowup1.Text = DtFollowUp.Rows[count]["Date"].ToString();
                    txttxtDate_GridFollowup1.Text = DtFollowUp.Rows[count]["Date"].ToString();

                    SetSelectItem_DLL(ObjddlFrom_FollowTab1, DtFollowUp.Rows[count]["From"].ToString());
                    SetSelectItem_DLL(ObjddlTo_FollowTab1, DtFollowUp.Rows[count]["To"].ToString());
                    txtAction_GridFollowup1.Text = DtFollowUp.Rows[count]["Action"].ToString();
                    //txtActionDate_GridFollowup1.Text = DtFollowUp.Rows[count]["ActionDate"].ToString();

                    txtActionDate_GridFollowup1.Text = DateTime.Parse(DtFollowUp.Rows[count]["ActionDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);

                    txtCustomerResponse_GridFollowup1.Text = DtFollowUp.Rows[count]["CustomerResponse"].ToString();
                    txtRemarks_GridFollowup1.Text = DtFollowUp.Rows[count]["Remarks"].ToString();

                    txttxtDate_GridFollowup1.ReadOnly = true;
                    ObjddlFrom_FollowTab1.Enabled = false;
                    ObjddlTo_FollowTab1.Enabled = false;
                    txtAction_GridFollowup1.ReadOnly = true;
                    txtActionDate_GridFollowup1.ReadOnly = true;
                    txtCustomerResponse_GridFollowup1.ReadOnly = true;
                    txtRemarks_GridFollowup1.ReadOnly = true;

                }
                count = count + 1;
            }
            TextBox txtAction_GridFollowupfoot = gvFollowUp.FooterRow.FindControl("txtActionDate_GridFollowup") as TextBox;
            txtAction_GridFollowupfoot.Attributes.Add("readonly", "readonly");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void ScriptManagerAlert(string Title, string AlertMsg)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Title, "alert('" + AlertMsg + "');", true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriInsertEnquiryResponse()
    {
        ObjEnquiryResponseClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            S3GBusEntity.EnquiryResponse.EnquiryResponseEntity ObjEnquiryResponseEntity = new S3GBusEntity.EnquiryResponse.EnquiryResponseEntity();
            if (intEnquiryResponseId > 0)
            {
                ObjEnquiryResponseEntity.EnquiryResponse_ID = intEnquiryResponseId;
                ObjEnquiryResponseEntity.Responded_By = intUserId;
                ObjEnquiryResponseEntity.EnquiryResponseDetailId = Convert.ToInt32(ddlResponse.SelectedValue);
                if (ViewState["statusID"] != null)
                    ObjEnquiryResponseEntity.Status = ViewState["statusID"].ToString();
                ObjEnquiryResponseEntity.Offer_ROI_Rules_ID = Convert.ToInt32(ddlROIRuleList.SelectedValue);
                ObjEnquiryResponseEntity.Offer_Payment_RuleCard_ID = Convert.ToInt32(ddlPaymentRuleList.SelectedValue);

                if (!string.IsNullOrEmpty(txtResidualValue_Cashflow.Text) && txtResidualValue_Cashflow.Text != "__.____")
                    ObjEnquiryResponseEntity.Offer_ResidualValue = Convert.ToDecimal(txtResidualValue_Cashflow.Text);
                else
                    ObjEnquiryResponseEntity.Offer_ResidualValue = Convert.ToDecimal(0);

                if (!string.IsNullOrEmpty(txtResidualAmt_Cashflow.Text))
                    ObjEnquiryResponseEntity.Offer_ResidualValueAmount = Convert.ToDecimal(txtResidualAmt_Cashflow.Text);
                else
                    ObjEnquiryResponseEntity.Offer_ResidualValueAmount = Convert.ToDecimal(0);

                if (!string.IsNullOrEmpty(txtMarginMoneyPer_Cashflow.Text) && txtMarginMoneyPer_Cashflow.Text != "__.____")
                    ObjEnquiryResponseEntity.Offer_Margin = Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text);
                else
                    ObjEnquiryResponseEntity.Offer_Margin = Convert.ToDecimal(0);

                if (!string.IsNullOrEmpty(txtMarginMoneyAmount_Cashflow.Text))
                    ObjEnquiryResponseEntity.Offer_Margin_Amount = Convert.ToDecimal(txtMarginMoneyAmount_Cashflow.Text);
                else
                    ObjEnquiryResponseEntity.Offer_Margin_Amount = Convert.ToDecimal(0);
                if (!ddlROIRuleList.SelectedItem.Text.ToUpper().Contains("RRA"))
                {
                    FunPriUpdateROIRule();
                }
                FunPriUpdateROIRuleDecRate();
                ObjEnquiryResponseEntity.XML_ROIRULE = ((DataTable)ViewState["ROIDetails"]).FunPubFormXml(true);

            }
            else
            {
                ObjEnquiryResponseEntity.EnquiryResponse_ID = Convert.ToInt32(ViewState["ResID"]);
                ObjEnquiryResponseEntity.Enquiry_No = txtEnquiryNo.Text;
                ObjEnquiryResponseEntity.Company_ID = intCompanyId;
                ObjEnquiryResponseEntity.Responded_By = intUserId;
                ObjEnquiryResponseEntity.EnquiryResponseDetailId = Convert.ToInt32(ddlResponse.SelectedValue);
                ObjEnquiryResponseEntity.Status = ViewState["statusID"].ToString();
                if (!string.IsNullOrEmpty(txtFinanceAmount.Text))
                    ObjEnquiryResponseEntity.Finance_Amount_Sought = Convert.ToDecimal(txtFinanceAmount.Text);
                else
                    ObjEnquiryResponseEntity.Finance_Amount_Sought = Convert.ToDecimal(0);

                if (!string.IsNullOrEmpty(txtResidualvalue.Text))
                    ObjEnquiryResponseEntity.Residual_Margin_Amount = Convert.ToDecimal(txtResidualvalue.Text);
                else
                    ObjEnquiryResponseEntity.Residual_Margin_Amount = Convert.ToDecimal(0);
                if (ddlLOBAssign.SelectedIndex > 0)
                    ObjEnquiryResponseEntity.LOB_ID = Convert.ToInt32(ddlLOBAssign.SelectedValue);
                else
                    ObjEnquiryResponseEntity.LOB_ID = Convert.ToInt32(ViewState["cashlobid"].ToString());
                if (ddlProductAssign.SelectedIndex > 0)
                    ObjEnquiryResponseEntity.Product_ID = Convert.ToInt32(ddlProductAssign.SelectedValue);
                else
                    ObjEnquiryResponseEntity.Product_ID = Convert.ToInt32(ViewState["cashproductid"].ToString());
                if (ddlBranchAssign.SelectedIndex > 0)
                    ObjEnquiryResponseEntity.Branch_ID = Convert.ToInt32(ddlBranchAssign.SelectedValue);
                else
                    ObjEnquiryResponseEntity.Branch_ID = Convert.ToInt32(ViewState["cashbranchid"].ToString());
                if (!string.IsNullOrEmpty(ViewState["WorkflowId"].ToString()))
                    ObjEnquiryResponseEntity.WorkFlow_Sequence = Convert.ToInt32(ViewState["WorkflowId"].ToString());
                //else
                //    ObjEnquiryResponseEntity.WorkFlow_Sequence = 9;
                ObjEnquiryResponseEntity.Offer_ROI_Rules_ID = Convert.ToInt32(ddlROIRuleList.SelectedValue);
                ObjEnquiryResponseEntity.Offer_Payment_RuleCard_ID = Convert.ToInt32(ddlPaymentRuleList.SelectedValue);

                if (!string.IsNullOrEmpty(txtResidualValue_Cashflow.Text) && txtResidualValue_Cashflow.Text != "__.____")
                    ObjEnquiryResponseEntity.Offer_ResidualValue = Convert.ToDecimal(txtResidualValue_Cashflow.Text);
                else
                    ObjEnquiryResponseEntity.Offer_ResidualValue = Convert.ToDecimal(0);

                if (!string.IsNullOrEmpty(txtResidualAmt_Cashflow.Text))
                    ObjEnquiryResponseEntity.Offer_ResidualValueAmount = Convert.ToDecimal(txtResidualAmt_Cashflow.Text);
                else
                    ObjEnquiryResponseEntity.Offer_ResidualValueAmount = Convert.ToDecimal(0);

                if (!string.IsNullOrEmpty(txtMarginMoneyPer_Cashflow.Text) && txtMarginMoneyPer_Cashflow.Text != "__.____")
                    ObjEnquiryResponseEntity.Offer_Margin = Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text);
                else
                    ObjEnquiryResponseEntity.Offer_Margin = Convert.ToDecimal(0);

                if (!string.IsNullOrEmpty(txtMarginMoneyAmount_Cashflow.Text))
                    ObjEnquiryResponseEntity.Offer_Margin_Amount = Convert.ToDecimal(txtMarginMoneyAmount_Cashflow.Text);
                else
                    ObjEnquiryResponseEntity.Offer_Margin_Amount = Convert.ToDecimal(0);

                if (!string.IsNullOrEmpty(txtBlockdepreciation.Text))
                    ObjEnquiryResponseEntity.Repay_Block_Depriciation = Convert.ToDecimal(txtBlockdepreciation.Text);

                if (!string.IsNullOrEmpty(txtBookdepreciation.Text))
                    ObjEnquiryResponseEntity.Repay_Book_Depriciation = Convert.ToDecimal(txtBookdepreciation.Text);
                if (!string.IsNullOrEmpty(txtApplication_Followup.Text))
                {
                    ObjEnquiryResponseEntity.ApplicationNumber = Convert.ToInt32(txtApplication_Followup.Text);
                }
                if (!ddlROIRuleList.SelectedItem.Text.ToUpper().Contains("RRA"))
                {
                    FunPriUpdateROIRule();
                }
                FunPriUpdateROIRuleDecRate();
                ObjEnquiryResponseEntity.XML_ROIRULE = ((DataTable)ViewState["ROIDetails"]).FunPubFormXml(true);

            }
            if (!string.IsNullOrEmpty(txtAccountIRR_Repay.Text))
            {
                ObjEnquiryResponseEntity.Repay_Accounting_IRR = Convert.ToDecimal(txtAccountIRR_Repay.Text);
            }
            if (!string.IsNullOrEmpty(txtCompanyIRR_Repay.Text))
            {
                ObjEnquiryResponseEntity.Repay_Company_IRR = Convert.ToDecimal(txtCompanyIRR_Repay.Text);
            }
            if (!string.IsNullOrEmpty(txtBusinessIRR_Repay.Text))
            {
                ObjEnquiryResponseEntity.Repay_Business_IRR = Convert.ToDecimal(txtBusinessIRR_Repay.Text);
            }


            //if (gvInflow.Rows.Count > 1)

            if (((DataTable)ViewState["DtCashFlow"]).Rows.Count == 0)
            {
                ObjEnquiryResponseEntity.XmlCashInflowDetails = "<Root></Root>";

            }
            else
            {
                ObjEnquiryResponseEntity.XmlCashInflowDetails = gvInflow.FunPubFormXml(true);
            }

            if (((DataTable)ViewState["DtFollowUp"]).Rows.Count == 0)
            {
                ObjEnquiryResponseEntity.XmlFollowDetails = "<Root></Root>";

            }
            else
            {
                ObjEnquiryResponseEntity.XmlFollowDetails = gvFollowUp.FunPubFormXml();
            }
            if (((DataTable)ViewState["DtAlertDetails"]).Rows.Count == 0)
            {
                ObjEnquiryResponseEntity.XmlAlertDetails = "<Root></Root>";

            }
            else
            {
                ObjEnquiryResponseEntity.XmlAlertDetails = gvAlert.FunPubFormXml();
            }



            string LOB = "";
            LOB = FunPriGetLOBstring();
            if (!string.IsNullOrEmpty(LOB))
            {
                if (LOB.Split('-')[0].ToString().ToLower().Trim() == "ol")
                {
                    //if (ViewState["OLExistingAsset"] != null && Convert.ToInt32(ViewState["OLExistingAsset"]) > 0)
                    {
                        ObjEnquiryResponseEntity.XmlOutFlowDetails = "<Root></Root>";

                    }
                }
                else
                {
                    ObjEnquiryResponseEntity.XmlOutFlowDetails = gvOutFlow.FunPubFormXml(true);
                }
            }

            if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count == 0)
            {
                ObjEnquiryResponseEntity.XmlRepaymentDetails = "<Root></Root>";
            }
            else
            {
                ObjEnquiryResponseEntity.XmlRepaymentDetails = gvRepaymentDetails.FunPubFormXml(true);
            }

            if (ViewState["RepayStructure"] != null)
            {
                DataTable dtrepaystructure = new DataTable();
                dtrepaystructure = (DataTable)ViewState["RepayStructure"];
                if (dtrepaystructure.Rows.Count > 0)
                {
                    ObjEnquiryResponseEntity.XML_REPAYSTRUCTURE = dtrepaystructure.FunPubFormXml(true);
                }
                else
                {
                    ObjEnquiryResponseEntity.XML_REPAYSTRUCTURE = "<Root></Root>";
                }
            }


            if (intEnquiryResponseId > 0)
            {
                try
                {
                    intErrorCode = ObjEnquiryResponseClient.FunPubModifyEnquiryResponse(ObjEnquiryResponseEntity);
                    // Utility.FunShowAlertMsg(this, "update");
                    if (intErrorCode == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Enquiry Response updated sucessfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                    }
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                }
            }
            else
            {
                try
                {
                    string strEnquiryResponseId = "";
                    intErrorCode = ObjEnquiryResponseClient.FunPubInsertEnquiryResponse(out strEnquiryResponseId, ObjEnquiryResponseEntity);
                    //Utility.FunShowAlertMsg(this, "save");
                    //return;
                    if (intErrorCode == 0)
                    {
                        //Utility.FunShowAlertMsg(this, "Enquiry Response created sucessfully");                  
                        if (PageMode == PageModes.WorkFlow)  // WORKFLOW IMPLEMENTATION
                        {
                            strEnquiryResponseId = txtEnquiryNo.Text;
                            WorkFlowSave(strEnquiryResponseId);
                        }
                        else
                        {
                            //Location implementaion
                            DataTable WFFP = new DataTable();
                            if (CheckForForcePullOperation(null, txtEnquiryNo.Text.Trim(), ProgramCode, null, null, "O", CompanyId, null, null, txtLOB.Text.Trim(), txtProductAssign.Text.Trim(), out WFFP))
                            {
                                DataRow dtrForce = WFFP.Rows[0];
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), txtEnquiryNo.Text.Trim(), int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), 1);
                            }

                            strAlert = "Enquiry " + txtEnquiryNo.Text + "  Responded successfully";
                            strAlert += @"\n\nWould you like to Response one more Enquiry?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);

                            TabContainerER.ActiveTabIndex = 0;
                            FunPriLoadEnquiryAssignedDetails();
                        }

                    }
                    else if (intErrorCode == 25)
                    {
                        Utility.FunShowAlertMsg(this, "Selected enquiry is already responded");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                }
            }

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjEnquiryResponseClient.Close();
        }
    }

    #region " WORK FLOW SAVE"
    private void WorkFlowSave(string strEnquiryResponseId)
    {
        WorkFlowSession WFValues = new WorkFlowSession();
        DataTable dtWorkFlow;
        int WFProgramId = 0;
        string strMsg = "";
        if (isWorkFlowTraveler) // WORK FLOW TRAVELLER 
        {
            try
            {
                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, 1);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                strMsg = "Work Flow is not assigned";
            }
        }
        else if (CheckForWorkFlowConfiguration(ProgramCode, WFLOBId, WFProductId, out WFProgramId, out dtWorkFlow) > 0) // WORK FLOW STARTER
        {
            try
            {
                int intWorkflowStatus = InsertWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFLOBId, WFBranchId, txtEnquiryNo.Text, WFProgramId, WFProductId, 1);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
                strMsg = "Work Flow is not assigned";
            }
        }

        // Navigate to Next WF Page
        ShowWFAlertMessage(strEnquiryResponseId, WFValues.WorkFlowProgramId.ToString(), strMsg);

    }

    /* WorkFlow Properties */
    private int WFLOBId { get { return int.Parse(ddlLOBAssign.SelectedValue); } }
    private int WFBranchId { get { return int.Parse(ddlBranchAssign.SelectedValue); } }
    private int WFProductId { get { return int.Parse(ddlProductAssign.SelectedValue); } }

    #endregion


    private Int32 GetId(Int16 iOptions)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            Int32 Id = 0;

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = iOptions;

            if (iOptions == 164)
            {
                if (ddlBranchAssign.SelectedValue == "-1" || ddlBranchAssign.SelectedValue == "")
                {
                    ObjStatus.Param1 = txtBranchAssign.Text;
                    Id = GetTbl(ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                }
                else
                {
                    Id = Convert.ToInt32(ddlBranchAssign.SelectedValue);
                }
            }
            else if (iOptions == 165)
            {
                if (ddlLOBAssign.SelectedValue == "-1" || ddlLOBAssign.SelectedValue == "")
                {
                    ObjStatus.Param1 = txtLOBAssign.Text;
                    Id = GetTbl(ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                }
                else
                {
                    Id = Convert.ToInt32(ddlLOBAssign.SelectedValue);
                }
            }
            else if (iOptions == 166)
            {
                if (ddlProductAssign.SelectedValue == "-1" || ddlProductAssign.SelectedValue == "")
                {
                    ObjStatus.Param1 = txtProductAssign.Text;
                    Id = GetTbl(ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                }
                else
                {
                    Id = Convert.ToInt32(ddlProductAssign.SelectedValue);
                }
            }
            else if (iOptions == 167)
            {
                //if (ddlworkflowSequence.SelectedValue == "-1" || ddlworkflowSequence.SelectedValue == "")
                if (txtworkflowSequence_Change.Text == "")
                {
                    ObjStatus.Param1 = txtworkflowSequence.Text;
                    Id = GetTbl(ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                }
                else
                {
                    if (txtworkflowSequence_Change.Text != "")
                    {
                        ObjStatus.Param1 = txtworkflowSequence_Change.Text;
                        Id = GetTbl(ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                    }
                }
            }



            return Id;


        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private Int32 GetTbl(DataTable ObjDt)
    {
        try
        {
            Int32 Id = 0;

            if (ObjDt.Rows.Count > 0)
            {
                Id = Convert.ToInt32(ObjDt.Rows[0][0]);
            }
            return Id;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            if (!bQuery)
            {
                Response.Redirect("~/Origination/S3G_Org_CreditParameterTransaction_View.aspx");
            }
            switch (intModeID)
            {
                case 0: // Create Mode
                    TabContainerER.ActiveTabIndex = 0;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    FunPriLoadEnquiryAssignedDetails();
                    //btnSave.Enabled = bCreate;
                    //btnClear.Enabled = true;
                    break;

                case 1: // Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    btnClear.Enabled = false;
                    TabContainerER.Tabs[0].Visible = false;
                    TabContainerER.ActiveTabIndex = 1;
                    // FunProDisableTabs(1);
                    btnSave.Enabled = bModify;
                    if (bClearList)
                    {
                        if (!string.IsNullOrEmpty(ddlLOBAssign.SelectedValue))
                            ddlLOBAssign.ClearDropDownList();
                        if (!string.IsNullOrEmpty(ddlBranchAssign.SelectedValue))
                            ddlBranchAssign.ClearDropDownList();
                        if (!string.IsNullOrEmpty(ddlProductAssign.SelectedValue))
                            ddlProductAssign.ClearDropDownList();
                        //ddlResponse.ClearDropDownList();
                        if (!string.IsNullOrEmpty(ddlROIRuleList.SelectedValue))
                            ddlROIRuleList.ClearDropDownList();
                        if (!string.IsNullOrEmpty(ddlPaymentRuleList.SelectedValue))
                            ddlPaymentRuleList.ClearDropDownList();
                    }
                    txtFinanceAmount.ReadOnly =
                    txtResidualvalue.ReadOnly =
                    txtResidualValue_Cashflow.ReadOnly =
                    txtResidualAmt_Cashflow.ReadOnly =
                    txtMarginMoneyPer_Cashflow.ReadOnly =
                    txtMarginMoneyAmount_Cashflow.ReadOnly = true;
                    if (Procparam == null)
                        Procparam = new Dictionary<string, string>();
                    else
                        Procparam.Clear();
                    Procparam.Add("@Param1", intCompanyId.ToString());
                    Procparam.Add("@Param2", intEnquiryResponseId.ToString());
                    Procparam.Add("@Option", "30");
                    DataTable Obj_Dt = new DataTable();
                    Obj_Dt = Utility.GetDataset("S3G_ORG_GetStatusLookUp", Procparam).Tables[0];
                    if (Obj_Dt.Rows.Count > 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Modification is Restricted. The Enquiry Number is already processed and forwarded to further Transaction");
                        lblEnqStatus.Value = "198";
                        hdnEdit_Status.Value = "1";
                        QueryView();
                    }
                    //  FunPriDisableControls();
                    break;
                case -1:// Query Mode
                    QueryView();
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void QueryView()
    {
        try
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            TabContainerER.Tabs[0].Visible = false;
            TabContainerER.ActiveTabIndex = 1;
            // FunProDisableTabs(1);
            txtFinanceAmount.ReadOnly =
            txtResidualvalue.ReadOnly =
            txtResidualValue_Cashflow.ReadOnly =
            txtResidualAmt_Cashflow.ReadOnly =
            txtMarginMoneyPer_Cashflow.ReadOnly =
            txtMarginMoneyAmount_Cashflow.ReadOnly = true;           
            if (bClearList)
            {
                if (!string.IsNullOrEmpty(ddlLOBAssign.SelectedValue))
                ddlLOBAssign.ClearDropDownList();
                if (!string.IsNullOrEmpty(ddlBranchAssign.SelectedValue))
                ddlBranchAssign.ClearDropDownList();
                if (!string.IsNullOrEmpty(ddlProductAssign.SelectedValue))
                ddlProductAssign.ClearDropDownList();
                if (!string.IsNullOrEmpty(ddlResponse.SelectedValue))
                ddlResponse.ClearDropDownList();
                if (!string.IsNullOrEmpty(ddlROIRuleList.SelectedValue))
                ddlROIRuleList.ClearDropDownList();
                if (!string.IsNullOrEmpty(ddlPaymentRuleList.SelectedValue))
                ddlPaymentRuleList.ClearDropDownList();
            }
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnCalIRR.Visible = false;
            btnReset.Visible = false;
            btnFetchPayment.Visible = false;
            btnFetchROI.Visible = false;
            // btnShowRepayment.Visible = false;
            if (gvInflow.Rows.Count > 0)
            {
                gvInflow.Columns[12].Visible = false;
                gvInflow.FooterRow.Visible = false;
            }
            if (gvOutFlow.Rows.Count > 0)
            {
                gvOutFlow.Columns[12].Visible = false;
                gvOutFlow.FooterRow.Visible = false;
            }

            if (gvAlert.Rows.Count > 0)
            {
                gvAlert.FooterRow.Visible = false;
                gvAlert.Columns[4].Visible = false;
            }
            if (gvFollowUp.Rows.Count > 0)
            {
                gvFollowUp.FooterRow.Visible = false;
                gvFollowUp.Columns[7].Visible = false;
            }
            if (gvRepaymentDetails.Rows.Count > 0)
            {
                gvRepaymentDetails.FooterRow.Visible = false;
                int len = gvRepaymentDetails.Columns.Count - 1;
                gvRepaymentDetails.Columns[len].Visible = false;
            }
            divROIRuleInfo.Disabled = true;
            txt_Rate.ReadOnly = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriLoadCashflowDetails(string ResponseID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

            ObjStatus.Option = 157;
            ObjStatus.Param1 = ResponseID;
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            gvInflow.DataSource = DtCashFlow;
            gvInflow.DataBind();
            ViewState["DtCashFlow"] = DtCashFlow;
            FillDataFrom_ViewState_CashInflow();
            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.CASH_FLOW_FROM.ToString();
            ViewState["CashFlowFrom"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            ObjStatus.Option = 158;
            ObjStatus.Param1 = ResponseID;
            DtCashFlow = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

            gvOutFlow.DataSource = DtCashFlow;
            gvOutFlow.DataBind();
            ViewState["DtCashFlowOut"] = DtCashFlow;
            FillDataFrom_ViewState_CashOutflow();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriLoadFollowup(string ResponseID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            if (intEnquiryResponseId > 0)
            {

                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();

                DataTable ObjFollowup = new DataTable();
                ObjStatus.Option = 72;
                ObjStatus.Param1 = ResponseID;
                ObjFollowup = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);


                gvFollowUp.DataSource = ObjFollowup;
                gvFollowUp.DataBind();

                ViewState["DtFollowUp"] = ObjFollowup;

                FillDataFrom_ViewState_FollowUp();
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }
    }

    private void FunPriLoadDropDownList()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            string strProcedureName = "S3g_Org_GetCustomerLookup";
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_Id", intUserId.ToString());
            Procparam.Add("@Program_ID", "46");
            if (ViewState["constitutionid"] != null)
            {
                Procparam.Add("@Consitution_Id", ViewState["constitutionid"].ToString());
            }
            ddlLOBAssign.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //FungetLocationdetails(strProcedureName);
            Procparam.Clear();
            Procparam.Add("@Option", "1");
            Procparam.Add("@Param1", "ENQUIRY_RESPONSE");
            ddlResponse.BindDataTable(strProcedureName, Procparam, new string[] { "ID", "NAME" });
            Procparam.Clear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FungetLocationdetails(string strProcedureName)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "4");
            Procparam.Add("@Param1", intCompanyId.ToString());
            Procparam.Add("@Param2", intUserId.ToString());
            Procparam.Add("@Param3", "46");
            if (ddlLOBAssign.SelectedIndex > 0)
                Procparam.Add("@Param4", ddlLOBAssign.SelectedValue);
            if (intEnquiryResponseId > 0)
                Procparam.Add("@Param5", "0");
            else
                Procparam.Add("@Param5", "1");
            ddlBranchAssign.BindDataTable(strProcedureName, Procparam, new string[] { "Location_ID", "Location" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriDisableControls()
    {
        try
        {
            txtFinanceAmount.Enabled = txtResidualvalue.Enabled =
            ddlLOBAssign.Enabled = ddlBranchAssign.Enabled =
            ddlProductAssign.Enabled = ddlResponse.Enabled =
            ddlROIRuleList.Enabled = ddlPaymentRuleList.Enabled =
            txtResidualValue_Cashflow.Enabled = txtResidualAmt_Cashflow.Enabled =
            txtMarginMoneyPer_Cashflow.Enabled = txtMarginMoneyAmount_Cashflow.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion

    #region IRR Methods

    protected void FunCalculateIRR(object sender, EventArgs e)
    {
        try
        {

            FunPriCalculateIRR();

        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Incorrect cashflow details," + ex.Message.Replace("'", " ").Replace(";", " ") + "');", true);
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_TabRepayment.ErrorMessage = "Incorrect Cash Flow details,cannot calculate IRR";
            cv_TabRepayment.IsValid = false;
        }

    }


    private void FunPriCalculateIRR()
    {
        try
        {
            double douAccountingIRR = 0;
            double douBusinessIRR = 0;
            double douCompanyIRR = 0;

            string LOB = "";
            int tenure = Convert.ToInt32(ViewState["Tenure"]);
            ClsRepaymentStructure objRepaymentStructure = new ClsRepaymentStructure();
            decimal decIRRActualAmount = 0;
            decimal decTotalAmount = 0;
            DataTable dtMoratorium = null;
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];

            if (DtRepayGrid.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, " Add atleast one repayment");
                return;
            }
            string strRate = txt_Rate.Text;
            switch (ddl_Return_Pattern.SelectedValue)
            {
                case "1":
                    strRate = txt_Rate.Text;
                    break;
                case "2":
                    if (ViewState["decRate"] != null)
                    {
                        strRate = ViewState["decRate"].ToString();
                    }
                    break;
            }
            if (strRate == "")
                strRate = "0";
            //if (ViewState["decRate"] != null)
            //{
            //    strRate = ViewState["decRate"].ToString();
            //}
            //else
            //{
            //    strRate = txt_Rate.Text;
            //}
            string strFinAmount = GetLOBBasedFinAmt();
            decimal DecRoundOff;
            if (Convert.ToString(ViewState["hdnRoundOff"]) != "")
                DecRoundOff = Convert.ToDecimal(ViewState["hdnRoundOff"]);
            else
                DecRoundOff = 2;

            if (!objRepaymentStructure.FunPubValidateTotalAmount(DtRepayGrid, strFinAmount, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue, strRate, ViewState["TenureType"].ToString(), ViewState["Tenure"].ToString(), out decIRRActualAmount, out decTotalAmount, "", DecRoundOff))
            {
                Utility.FunShowAlertMsg(this, "Total amount should be equal to finance amount + interest (" + decTotalAmount + ")");
                return;
            }

            decimal decBreakPercent = ((decimal)((DataTable)ViewState["DtRepayGrid"]).Compute("Sum(Breakup)", "CashFlow_Flag_ID = 23"));
            if (decBreakPercent != 0)
            {
                if (decBreakPercent != 100)
                {
                    Utility.FunShowAlertMsg(this, "Total break up percentage should be equal to 100%");
                    return;
                }
            }
            LOB = FunPriGetLOBstring();

            DataTable dtRepaymentStructure = new DataTable();

            objRepaymentStructure.FunPubCalculateIRR(DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture).ToString(strDateFormat), hdnPLR.Value, ddl_Frequency.SelectedValue, strDateFormat, txtResidualAmt_Cashflow.Text, txtResidualValue_Cashflow.Text, out douAccountingIRR, out douBusinessIRR, out douCompanyIRR
                , out dtRepaymentStructure, (DataTable)ViewState["DtRepayGrid"], (DataTable)ViewState["DtCashFlow"], (DataTable)ViewState["DtCashFlowOut"]
                , strFinAmount, txtMarginMoneyPer_Cashflow.Text, ddl_Return_Pattern.SelectedValue
                , strRate, ViewState["TenureType"].ToString(), ViewState["Tenure"].ToString(), ddl_IRR_Rest.SelectedValue,
                ddl_Time_Value.SelectedValue, LOB, ddl_Repayment_Mode.SelectedValue, "", dtMoratorium, false, "NIL", 0);



            if (dtRepaymentStructure.Rows.Count > 0)
            {
                grvRepayStructure.DataSource = dtRepaymentStructure;
                grvRepayStructure.DataBind();
                ViewState["RepayStructure"] = dtRepaymentStructure;
                PanRepay.Visible = true;
            }
            else
            {
                PanRepay.Visible = false;
            }
            txtAccountIRR_Repay.Text = douAccountingIRR.ToString("0.0000");
            txtBusinessIRR_Repay.Text = douBusinessIRR.ToString("0.0000");
            txtCompanyIRR_Repay.Text = douCompanyIRR.ToString("0.0000");

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Incorrect Cash Flow details,cannot calculate IRR");
        }
    }
    private string FunPriGetLOBstring()
    {
        string LOB = "";
        if (ddlLOBAssign.SelectedIndex > 0)
            LOB = ddlLOBAssign.SelectedItem.Text;
        else if (!string.IsNullOrEmpty(txtLOB.Text.Trim()))
            LOB = txtLOB.Text.Trim();
        return LOB;
    }

    private bool FunPriValidateTotalAmount(out decimal decActualAmount, out decimal decTotalAmount, string strOption)
    {
        try
        {
            decimal decFinamt = FunPriGetAmountFinanced();
            decimal decRate;
            switch (ddl_Return_Pattern.SelectedItem.Text)
            {

                case "IRR (Internal Rate of Return)":
                    //ObjCommonBusLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, Convert.ToDecimal(txtFacilityAmt.Text), Convert.ToDouble(9.6365), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decRate, Convert.ToDecimal(10.05), decPLR);
                    decRate = Convert.ToDecimal(txt_Rate.Text) / 2;//Hard Coded for testing IRR
                    break;
                default:
                    decRate = Convert.ToDecimal(txt_Rate.Text);
                    break;
            }
            if (strOption != "3")
            {
                decTotalAmount = decFinamt + Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(ViewState["TenureType"].ToString(), decFinamt, decRate, Convert.ToInt32(ViewState["Tenure"].ToString())), 0);
            }
            else
            {
                decTotalAmount = decFinamt;
            }
            decActualAmount = 0;
            if (((DataTable)ViewState["DtRepayGrid"]).Rows.Count <= 0)
            {
                cv_TabRepayment.ErrorMessage = " Correct the following validation(s): <br/><br/>  Add atleast one row Repayment details";
                cv_TabRepayment.IsValid = false;
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
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }



    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            txtBusinessIRR_Repay.Text = "";
            txtCompanyIRR_Repay.Text = "";
            txtAccountIRR_Repay.Text = "";
            FunPriBindRepaymentDetails("0");
            //PanRepay.Visible = false;
            grvRepayStructure.DataSource = null;
            grvRepayStructure.DataBind();
            ViewState["RepayStructure"] = null;



        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cv_Main.ErrorMessage = ex.Message;
            cv_Main.IsValid = false;
        }
    }

    protected void FunProGetIRRDetails()
    {
        try
        {
            DataTable dtIRRDetails = Utility.FunPubGetGlobalIRRDetails(intCompanyId, null);
            ViewState["IRRDetails"] = dtIRRDetails;

            bool blnIsVisible = true;
            if (dtIRRDetails.Rows.Count > 0)
            {
                if (dtIRRDetails.Rows[0]["IsIRRApplicable"].ToString() == "True")
                {
                    blnIsVisible = true;

                }
                else
                {
                    blnIsVisible = false;
                }
                txtAccountIRR_Repay.Visible = lblAccountIRR_Repay.Visible =
                txtCompanyIRR_Repay.Visible = lblCompanyIRR_Repay.Visible =

                txtCompanyIRR_Repay.Visible = lblCompanyIRR_Repay.Visible =
                txtAccountIRR_Repay.Visible = lblAccountIRR_Repay.Visible = blnIsVisible;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error in fetching Global IRR Details");
        }
    }

    private void FunPriAssignRate(string strLobId)
    {
        try
        {
            DataTable dtPLR = (DataTable)ViewState["IRRDetails"];
            dtPLR.DefaultView.RowFilter = "LOB_ID = " + strLobId;
            dtPLR = dtPLR.DefaultView.ToTable();
            if (dtPLR.Rows.Count > 0)
            {
                hdnCTR.Value = dtPLR.Rows[0]["Corporate_Tax_Rate"].ToString();
                hdnPLR.Value = dtPLR.Rows[0]["Prime_Lending_Rate"].ToString();
                ViewState["hdnRoundOff"] = dtPLR.Rows[0]["Roundoff"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    #endregion


    private void FunPriValidateApplicationStart()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANYID", intCompanyId.ToString());
            Procparam.Add("@PROGRAMID", "46");
            DataSet dsGlobalStartPoint = Utility.GetDataset("S3G_ORG_GETSTARTINGPOINTENQ", Procparam);

            if (dsGlobalStartPoint.Tables[1].Rows.Count > 0)
            {
                bool blnIsAllowROIModify = Convert.ToBoolean(dsGlobalStartPoint.Tables[1].Rows[0]["IS_PROGRAM"].ToString());
                if (blnIsAllowROIModify)
                {
                    ddlROIRuleList.Enabled = true;
                }
                else
                {
                    ddlROIRuleList.Enabled = false;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Define ROI Rule Modification in GlobalParameter setup");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }


    }

    private decimal FunPriGetAmountFinanced()
    {
        try
        {
            decimal decFinanaceAmt;
            string strFinAmount = GetLOBBasedFinAmt();
            decFinanaceAmt = Convert.ToDecimal(strFinAmount);//- FunPriGetMarginAmout() ;
            return Math.Round(decFinanaceAmt, 0);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void txRepaymentFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtBoxFromdate = (TextBox)sender;
            FunPriGenerateRepayment(Utility.StringToDate(txtBoxFromdate.Text));
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void txt_Margin_Percentage_OnTextChanged(object sender, EventArgs e)
    {

        if (Convert.ToDecimal(txt_Margin_Percentage.Text) > 100)
        {
            txtMarginMoneyPer_Cashflow.Text = txt_Margin_Percentage.Text = "";
            Utility.FunShowAlertMsg(this, "Margin Percentage should not be greater than 100%");
            txt_Margin_Percentage.Focus();
            return;
        }
        else
        {
            txt_Margin_Percentage.SetDecimalPrefixSuffix(2, 4, false);
            txtMarginMoneyPer_Cashflow.Text = txt_Margin_Percentage.Text;
            txtMarginMoneyAmount_Cashflow.Text = Convert.ToString(FunPriGetMarginAmout());
            /* if (gvRepaymentDetails != null)
             {
                 if (gvRepaymentDetails.FooterRow != null)
                 {
                     TextBox txtAmountRepaymentCashFlow_RepayTab = (TextBox)gvRepaymentDetails.FooterRow.FindControl("txtAmountRepaymentCashFlow_RepayTab");
                     txtAmountRepaymentCashFlow_RepayTab.Text = Convert.ToString(FunPriGetAmountFinanced());
                 }
             }*/
            // txtAmountRepaymentCashFlow_RepayTab=
        }


    }
    private void FunPriLobbasedRoi(string strType)
    {
        switch (strType.ToLower())
        {
            case "hp":  //Hire Purchase
            case "ln": //Loan
            case "fl":  //Financial Leasing
            case "ol":  //Operating Lease
                rfvTimeValue.Enabled = true;
                rfvFrequency.Enabled = true;
                break;

            default:
                rfvTimeValue.Enabled = false;
                rfvFrequency.Enabled = false;
                break;
        }
    }
    private bool FunPriValidateTenurePeriod(DateTime dtStartDate, DateTime dtEndDate)
    {
        DateTime dateInterval = new DateTime();
        int tenure = 0;
        string tenureType = "";
        if (ViewState["Tenure"] != null)
        {
            tenure = Convert.ToInt32(ViewState["Tenure"]);
        }
        else
        {
            tenure = Convert.ToInt32(hdnTenure.Value);
        }
        if (ViewState["TenureType"] != null)
        {
            tenureType = Convert.ToString(ViewState["TenureType"]);
        }
        switch (tenureType.ToLower())
        {
            case "months":
                dateInterval = dtStartDate.AddMonths(tenure);
                break;
            case "weeks":

                int intAddweeks = Convert.ToInt32(tenure) * 7;
                dateInterval = dtStartDate.AddDays(intAddweeks);
                break;
            case "days":
                dateInterval = dtStartDate.AddDays(tenure);
                break;
        }

        if (dtEndDate > dateInterval)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    private bool FunPriValidateTenurePeriod(int intActualTenurePeriod)
    {
        int tenure = 0;
        if (ViewState["Tenure"] != null)
        {
            tenure = Convert.ToInt32(ViewState["Tenure"]);
        }
        else
        {
            tenure = Convert.ToInt32(hdnTenure.Value);
        }
        if (intActualTenurePeriod == tenure)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private void FunPriCalculateSummary(DataTable objDataTable, string strGroupByField, string strSumField)
    {
        try
        {
            DataTable dtSummaryDetails = Utility.FunPriCalculateSumAmount(objDataTable, strGroupByField, strSumField);


            DataTable dtSummaryDtls = new DataTable();
            DataColumn dc1 = new DataColumn("CashFlow_Description");
            DataColumn dc2 = new DataColumn("Amount");
            dtSummaryDtls.Columns.Add(dc1);
            dtSummaryDtls.Columns.Add(dc2);
            if (dtSummaryDetails.Rows.Count > 0)
            {

                for (int i = 0; i < dtSummaryDetails.Rows.Count; i++)
                {
                    DataRow dr = dtSummaryDtls.NewRow();
                    dr["CashFlow_Description"] = dtSummaryDetails.Rows[i]["CashFlow"];
                    dr["Amount"] = dtSummaryDetails.Rows[i]["TotalPeriodInstall"];
                    dtSummaryDtls.Rows.Add(dr);
                }
            }
            gvRepaymentSummary.DataSource = dtSummaryDtls;
            gvRepaymentSummary.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error in calculating Summary");
        }

    }

    private void FunPriIRRReset()
    {
        txtAccountIRR_Repay.Text = txtBusinessIRR_Repay.Text =
        txtCompanyIRR_Repay.Text = "";

    }

    private decimal FunPriGetMarginAmout()
    {
        decimal decMarginAmount;


        if (txtMarginMoneyPer_Cashflow.Text != "")
        {
            //decMarginAmount = (Convert.ToDecimal(txtFinanceAmount.Text) * (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) / 100));
            decMarginAmount = (Convert.ToDecimal(txtAssetValue.Text) * (Convert.ToDecimal(txtMarginMoneyPer_Cashflow.Text) / 100));
        }
        else
        {
            decMarginAmount = 0;
        }


        return decMarginAmount;
    }


    protected void txtResidualValue_Cashflow_TextChanged(object sender, EventArgs e)
    {
        if (txtResidualValue_Cashflow.Text != "")
        {
            RFVresidualvalue.Enabled = false;
            txtResidualAmt_Cashflow.ReadOnly = true;

        }
        else
        {
            RFVresidualvalue.Enabled = true;
            txtResidualAmt_Cashflow.ReadOnly = false;
        }
        // txtResidualValue_Cashflow.Focus();
    }

    protected void txtResidualAmt_Cashflow_TextChanged(object sender, EventArgs e)
    {
        if (txtResidualAmt_Cashflow.Text != "")
        {
            RFVresidualvalue.Enabled = false;
            txtResidualValue_Cashflow.ReadOnly = true;

        }
        else
        {
            RFVresidualvalue.Enabled = true;
            txtResidualValue_Cashflow.ReadOnly = false;
        }
        // txtResidualAmt_Cashflow.Focus();
    }
    protected void txtAssetvalue_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            FunctionClearCashFlowRel();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvEnquiryResponse.ErrorMessage = "Due to Data Problem,Unable to process";
            cvEnquiryResponse.IsValid = false;
        }
    }
    protected void txtFinanceAmount_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            ClsRepaymentStructure objclsRepaymentStructure = new ClsRepaymentStructure();
            decimal marginamount = objclsRepaymentStructure.FunPubGetMarginAmout(txtAssetValue.Text, txtMarginMoneyPer_Cashflow.Text);
            decimal totalfinamount = Convert.ToDecimal(txtAssetValue.Text) - marginamount;
            if (txtMarginMoneyPer_Cashflow.Text != "")
                txtMarginMoneyAmount_Cashflow.Text = marginamount.ToString();
            else
                txtMarginMoneyAmount_Cashflow.Text = "";
            string strFinAmount = GetLOBBasedFinAmt();
            if (ViewState["AssetTypeExisting"] != null && Convert.ToInt32(ViewState["AssetTypeExisting"]) > 0)
            {
                if (Convert.ToDecimal(txtFinanceAmount.Text) > totalfinamount)
                {
                    Utility.FunShowAlertMsg(this, "Finance amount should not be greater than asset value with margin money(" + totalfinamount + ")");
                    txtFinanceAmount.Text = "";
                    txtFinanceAmount.Focus();
                    return;
                }
            }
            FunctionClearCashFlowRel();

        }
        catch (Exception ex)
        {

            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvEnquiryResponse.ErrorMessage = "Due to Data Problem,Unable to Calculate Margin";
            cvEnquiryResponse.IsValid = false;
        }



    }

    private void FunctionClearCashFlowRel()
    {
        try
        {
            FunPriIRRReset();
            FunPriBindCashFlowDetails();
            FunPriBindRepaymentDetails("0");
            grvRepayStructure.DataSource = null;
            grvRepayStructure.DataBind();
            ViewState["RepayStructure"] = null;
            FunPriShowRepaymetDetails(0);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }



    private int FunPriGetNoofYearsFromTenure()
    {
        int intNoofYears = 0;
        int tenure = 0;
        if (ViewState["Tenure"] != null)
            tenure = Convert.ToInt32(ViewState["Tenure"]);
        else
            tenure = Convert.ToInt32(hdnTenure.Value);
        if (ViewState["TenureType"] != null)
        {
            switch (Convert.ToString(ViewState["TenureType"]).ToLower())
            {
                case "months":
                    intNoofYears = (int)Math.Ceiling(Convert.ToDecimal(tenure / 12.00));
                    break;
                case "weeks":
                    intNoofYears = (int)Math.Ceiling(Convert.ToDecimal(tenure / 52.00));
                    break;
                case "days":
                    intNoofYears = (int)Math.Ceiling(Convert.ToDecimal(tenure / 365.00));
                    break;
            }
        }

        return intNoofYears;
    }



    #region Properties
    protected DateTime dtNextDate { get; set; }
    protected int intNextInstall { get; set; }
    #endregion


    private void FunPriGetNextRepaydate()
    {
        try
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
            int intToInstall = 0;
            DateTime dtNextFromdate = DateTime.Now;
            DataRow[] drRow = DtRepayGrid.Select("CashFlow_Flag_ID = 23", "ToInstall desc");
            if (drRow.Length > 0)
            {
                dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(ddl_Frequency.SelectedValue, Convert.ToDateTime(drRow[0].ItemArray[8].ToString()));
                intNextInstall = Convert.ToInt32(drRow[0].ItemArray[6].ToString());
            }
            else
            {
                dtNextDate = dtNextFromdate;
                intNextInstall = 0;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }


    private decimal FunPriGetInterestAmount()
    {
        /*decimal decFinAmount = FunPriGetAmountFinanced();
        decimal decRate = 0;
        int tenure = 0;
        string tenuretype = "";
        if (ViewState["Tenure"] != null)
            tenure = Convert.ToInt32(ViewState["Tenure"]);
        else
            tenure = Convert.ToInt32(hdnTenure.Value);
        if (ViewState["TenureType"] != null)
            tenuretype = ViewState["TenureType"].ToString();

        switch (ddl_Return_Pattern.SelectedValue)
        {

            case "1":
                decRate = Convert.ToDecimal(txt_Rate.Text);
                break;
            case "2":
                //ObjCommonBusLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, Convert.ToDecimal(txtFacilityAmt.Text), Convert.ToDouble(9.6365), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decRate, Convert.ToDecimal(10.05), decPLR);
                if (ViewState["decRate"] != null)
                {
                    decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
                }//Hard Coded for testing IRR
                break;
        }
        string strLOB = "";
        if (ddlLOBAssign.SelectedIndex > 0)
            strLOB = ddlLOBAssign.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower();
        else
            strLOB = txtLOB.Text.Split('-')[0].ToString().Trim().ToLower();

        switch (strLOB)
        {
            case "tl":
            case "te":
                if (ddl_Repayment_Mode.SelectedValue == "5")
                {
                    decRate = 0;
                }
                break;
            case "ft":
            case "wc":
                decRate = 0;
                break;
        }

        return Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(tenuretype.ToLower(), FunPriGetAmountFinanced(), decRate, tenure), 0);
          */

        decimal decFinAmount = FunPriGetAmountFinanced();
        decimal decUMFC = 0;
        if (!string.IsNullOrEmpty(lblTotalAmount.Text))
        {
            string strTotalAmount = (lblTotalAmount.Text.Split(':').Length > 1) ? lblTotalAmount.Text.Split(':')[1].Trim() : "";
            if (strTotalAmount != "")
            {
                decimal decTotalRepayable = Convert.ToDecimal(strTotalAmount);
                decUMFC = decTotalRepayable - decFinAmount;
            }
        }
        return decUMFC;
    }
    private decimal FunPriGetStructureAdhocInterestAmount()
    {
        decimal decFinAmount = FunPriGetAmountFinanced();
        decimal decRate = 0;
        int tenure = 0;
        string tenuretype = "";
        if (ViewState["Tenure"] != null)
            tenure = Convert.ToInt32(ViewState["Tenure"]);
        else
            tenure = Convert.ToInt32(hdnTenure.Value);
        if (ViewState["TenureType"] != null)
            tenuretype = ViewState["TenureType"].ToString();

        switch (ddl_Return_Pattern.SelectedValue)
        {

            case "1":
                decRate = Convert.ToDecimal(txt_Rate.Text);
                break;
            case "2":
                //ObjCommonBusLogic.FunPubCalculateFlatRate(dtRepaymentTab, dtCashInflow, dtCashOutflow, ddl_Frequency.SelectedItem.Text, Convert.ToInt32(txtTenure.Text), ddlTenureType.SelectedItem.Text, strDateFormat, Convert.ToDecimal(txtFacilityAmt.Text), Convert.ToDouble(9.6365), strIrrRest, "Empty", strTimeval, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out decRate, Convert.ToDecimal(10.05), decPLR);
                if (ViewState["decRate"] != null)
                {
                    decRate = Convert.ToDecimal(ViewState["decRate"].ToString());
                }//Hard Coded for testing IRR
                break;
        }
        string strLOB = "";
        if (ddlLOBAssign.SelectedIndex > 0)
            strLOB = ddlLOBAssign.SelectedItem.Text.Split('-')[0].ToString().Trim().ToLower();
        else
            strLOB = txtLOB.Text.Split('-')[0].ToString().Trim().ToLower();

        switch (strLOB)
        {
            case "tl":
            case "te":
                if (ddl_Repayment_Mode.SelectedValue == "5")
                {
                    decRate = 0;
                }
                break;
            case "ft":
            case "wc":
                decRate = 0;
                break;
        }

        return Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(tenuretype.ToLower(), FunPriGetAmountFinanced(), decRate, tenure), 0);
    }


    private void FunPriShowRepaymetDetails(decimal decAmountRepayble)
    {
        int tenure = 0;
        if (ViewState["Tenure"] != null)
        {
            tenure = Convert.ToInt32(ViewState["Tenure"]);
        }
        if (ViewState["DtRepayGrid"] != null)
        {
            DtRepayGrid = (DataTable)ViewState["DtRepayGrid"];
        }

        if (tenure.ToString() != "" || tenure.ToString() != string.Empty || tenure.ToString() != "0")
        {

            lblTotalAmount.Text = "Total Amount Repayable : " + decAmountRepayble.ToString();

            lblFrequency_Display.Text = "Tenure &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + ViewState["Tenure"].ToString() + " " + ViewState["TenureType"].ToString();

            if (txt_Rate.Text.Trim() != "")
            {
                if (ViewState["decRate"] != null)
                {
                    lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + ViewState["decRate"].ToString();
                }
                else
                {
                    lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + txt_Rate.Text;
                }
            }

            //lblMarginResidual.Text = "Rate &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; : " + txt_Rate.Text;


        }

    }

    protected void gvAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((CheckBox)e.Row.FindControl("ChkEmail")).Text == "0" || ((CheckBox)e.Row.FindControl("ChkEmail")).Text.ToLower() == "false")
                ((CheckBox)e.Row.FindControl("ChkEmail")).Checked = false;
            else
                ((CheckBox)e.Row.FindControl("ChkEmail")).Checked = true;
            ((CheckBox)e.Row.FindControl("ChkEmail")).Text = String.Empty;

            if (((CheckBox)e.Row.FindControl("ChkSMS")).Text == "0" || ((CheckBox)e.Row.FindControl("ChkSMS")).Text.ToLower() == "false")

                ((CheckBox)e.Row.FindControl("ChkSMS")).Checked = false;
            else
                ((CheckBox)e.Row.FindControl("ChkSMS")).Checked = true;
            ((CheckBox)e.Row.FindControl("ChkSMS")).Text = String.Empty;

        }
    }

    protected void FunChangeLOB()
    {
        try
        {
            txtLOB_Followup.Text = ddlLOBAssign.SelectedItem.Text;
            txtBranch_Followup.Text = ddlBranchAssign.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }
}

