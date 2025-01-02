using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Text;
using System.Globalization;
using System.ServiceModel;
using System.Collections;
using System.Web.Security;
using System.Configuration;

public partial class Origination_S3G_ORG_CreditParameterTransaction : ApplyThemeForProject
{
    #region Local Variables
    S3GSession ObjS3GSession;
    int intAppraisalId = 0;
    int intCustomerId = 0;
    int intEnquiryResponseId = 0;
    int intNoofSearch = 8;
    int intCompanyId = 0;
    int intUserId = 0;
    int intCreditParamTransId = 0;
    int intErrorCode = 0;
    int intCurrentPageId;
    int intPageSize;
    int intCurrentPage;
    string strSearchValue = "";
    string strOrderBy = "";
    int intTotalRecords;
    int intSerialNumber;
    public decimal TotActual = 0;
    public decimal TotReq = 0;
    public decimal TotPercentage = 0;
    private DataSet dtAppraisalDetails;

    //PagingValues ObjPaging = new PagingValues();
    string[] arrSortCol = new string[] { "LOB_Code+'-'+LOB_Name", "UserLocM.Location_Code+'-'+UserLocM.LocationCat_Description", "Product_Code+'-'+Product_Name", "DocNo", "Enquiry_Date", "CM.Customer_Code+'-'+CM.Customer_Name", "CM.Customer_Name", "Convert(Varchar(5),COM.Constitution_Code)+'-'+COM.Constitution_Name" };
    ArrayList arrSearchVal = new ArrayList(1);
    string _DateFormat = "dd/MM/yyyy";
    int _SerialNo = 0;
    Dictionary<string, string> Procparam = null;
    static string strMessage = @" Correct the following validation(s): </br></br>   ";

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgTranslander.aspx?Code=CRPT&MultipleDNC=1';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_Org_CreditParameterTransaction.aspx';";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    //User Authorization
    string strMode = string.Empty;
    bool bClearList = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    string strProcName = "S3G_Org_GetCreditParamTrans_AppDetails1";
    //Code end
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);

    #endregion
    #region Mani

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

    public int intPrefix
    {
        get;
        set;

    }
    public int intSuffix
    {
        get;
        set;

    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGrid(ddlDocumentType.SelectedValue);
        FunSetDateFormat();
    }
    #endregion

    private void FunPriBindGrid(string Option)
    {
        try
        {
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            ObjPaging.ProProgram_ID = 35;

            //FunPriGetSearchValue();
            //ObjPaging.ProCurrentPage = intCurrentPageId;
            //ObjPaging.ProUser_ID = intUserId;
            //ObjPaging.ProCompany_ID = intCompanyId;
            //ObjPaging.ProPageSize = intPageSize;
            //ObjPaging.ProSearchValue = rdnlCreditType.SelectedIndex;
            //ObjPaging.ProSearchValue = hdnSearch.Value;
            //ObjPaging.ProOrderBy = hdnOrderBy.Value;
            //ObjPaging.ProTotalRecords = intTotalRecords;

            FunPriGetSearchValue();

            Procparam = new Dictionary<string, string>();
            //if (rdnlCreditType.SelectedIndex == 0)
            //    Procparam.Add("@AppraisalType", "0");
            //else
            Procparam.Add("@AppraisalType", Option.ToString());
            grvEnquiryPaging.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvEnquiryPaging.Rows[0].Visible = false;
            }


            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            lblErrorMessage.Text = "";

            //Paging Config End

            FunSetDateFormat();
            if (ddlDocumentType.SelectedValue == "6")
            {
                grvEnquiryPaging.Columns[4].Visible = false;
                grvEnquiryPaging.Columns[5].Visible = false;
                grvEnquiryPaging.Columns[6].Visible = false;
                grvEnquiryPaging.Columns[7].Visible = false;
                grvEnquiryPaging.Columns[8].Visible = true;


            }
            else
            {
                grvEnquiryPaging.Columns[4].Visible = true;
                grvEnquiryPaging.Columns[5].Visible = true;
                grvEnquiryPaging.Columns[6].Visible = true;
                grvEnquiryPaging.Columns[7].Visible = true;
                grvEnquiryPaging.Columns[8].Visible = true;
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

    }

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        ProgramCode = "035";

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        arrSearchVal = new ArrayList(intNoofSearch);

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
        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
        }

        #endregion

        UserInfo ObjUserInfo = new UserInfo();
        ObjS3GSession = new S3GSession();

        int intGPSPrefix = 0;
        int intGPSSuffix = 0;
        intPrefix = 4;
        intSuffix = 3;

        intGPSPrefix = ObjS3GSession.ProGpsPrefixRW;
        intGPSSuffix = ObjS3GSession.ProGpsSuffixRW;

        if (intPrefix > intGPSPrefix)
        {
            intPrefix = intGPSPrefix;
        }
        if (intSuffix > intGPSSuffix)
        {
            intSuffix = intGPSSuffix;
        }

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        _DateFormat = ObjS3GSession.ProDateFormatRW;
        arrSearchVal = new ArrayList(intNoofSearch);
        lblErrorMessage.Text = "";
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            if (fromTicket != null)
            {
                intCreditParamTransId = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid Credit Parameter Transaction Details");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            strMode = Request.QueryString["qsMode"];
        }

        if (TabContainerCPT.ActiveTabIndex == 1)
        {
            TabContainerCPT.Tabs[1].Enabled = false;
            TabContainerCPT.Tabs[2].Enabled = false;
        }
        try
        {
            if (!IsPostBack)
            {
                FunPriLoadDocumentType();
                //FunPriBindGrid();
                FunSetDateFormat();
                FunPriSetResourceSettings();
                //User Authorization
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                if (!blnIsWorkflowApplicable || Session["DocumentNo"] == null)
                {
                    if (intCreditParamTransId > 0)
                    {
                        if (strMode == "M")
                        {
                            FunPriDisableControls(1);
                            divSave.Visible = true;
                        }
                        else
                        {
                            FunPriDisableControls(-1);

                            //Changed By Thangam M 0n 16/Feb/2012 to show the buttons
                            divSave.Visible = true;
                            btnSave.Enabled = btnCancel.Enabled = false;
                        }

                    }
                    else
                    {
                        FunPriDisableControls(0);
                        ddlStatus.Enabled = false;
                        ddlStatus.SelectedValue = "1";
                        txtStatus.Text = ddlStatus.SelectedItem.Text;
                    }
                }
                else
                {
                    //PreparePageForWFLoad();
                }


                if (PageMode == PageModes.WorkFlow)
                {
                    PreparePageForWFLoad();
                    TabContainerCPT.ActiveTabIndex = 1;
                    TabContainerCPT.Tabs[1].Enabled = true;
                    divSave.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        //FunPriSetMaxLength();
    }

    private void PreparePageForWFLoad()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

        try
        {
            WorkFlowSession WFValues = new WorkFlowSession();
            byte[] byte_ToDoList = objWorkflowToDoClient.FunPubLoadCreditGuideTransaction(WFValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFValues.Document_Type);
            DataSet dsEnquiryAppraisal = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_ToDoList, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsEnquiryAppraisal.Tables.Count > 0)
            {
                if (dsEnquiryAppraisal.Tables[0].Rows.Count > 0)
                {
                    //ViewState["AppraisalId"] = intAppraisalId = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Enq_cus_App_ID"]);
                    //ViewState["CustomerId"] = intCustomerId = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Customer_Id"]);
                    //ViewState["EnquiryResponseId"] = intEnquiryResponseId = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0][""Document_Type_ID"]);
                    ddlDocumentType.SelectedValue = Convert.ToString(dsEnquiryAppraisal.Tables[0].Rows[0]["Document_Type"]);
                    ViewState["AppraisalId"] = intAppraisalId = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Enq_cus_App_ID"]); // = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Enq_Cus_App_Id"]);
                    ViewState["CustomerId"] = intCustomerId = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Customer_Id"]);// = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Customer_Id"]);
                    ViewState["EnquiryResponseId"] = intEnquiryResponseId = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Document_Type_ID"]);
                }
                FunPriLoadDetails(intAppraisalId);
            }
            //FunPriLoadAppraisalDetails();
            //FunPriBindGrid(ddlDocumentType.SelectedValue);
            //FunSetDateFormat();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objWorkflowToDoClient.Close();
        }
    }

    private void FunPriLoadDocumentType()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OptionValue", "4");
            DataTable dt = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", Procparam);
            DataRow[] dr = dt.Select("Value NOT IN(4,5)", "Name", DataViewRowState.CurrentRows);
            dt = dr.CopyToDataTable();
            ddlDocumentType.BindDataTable(dt, new string[] { "Value", "Name" });
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Button Events

    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            if (strMode == "M")
            {
                if (ddlStatus.SelectedItem.ToString().ToLower() == "approved")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert1", "alert('The Selected Enquiry Number is Approved Already');", true);
                    return;
                }
                //if (ddlStatus.SelectedItem.ToString().ToLower() == "rejected")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert1", "alert('The Selected Enquiry Number is rejected Already');", true);
                //    return;
                //}
            }

            if (!FunPriValidateActualScore())
            {
                return;
            }
            string strXmlvalue = LoadXMLValues();

            //By Thangam on 11/Sep/2013 based on Sudarsan sir requirement
            //if (TotActual < TotReq)
            //{
            //    string strSaveAlert = "alert('__ALERT__');";
            //    strSaveAlert = @"\n Status was rejected. Would you like to proceed?";

            //    //Changed By Thangam M 0n 16/Feb/2012 to show the buttons
            //    //strSaveAlert = "if(confirm('" + strSaveAlert + "')){  document.getElementById('" + Button1.ClientID + "').click();}else{document.getElementById('ctl00_ContentPlaceHolder1_divSave').style.visibility='hidden';}";
            //    strSaveAlert = "if(confirm('" + strSaveAlert + "')){  document.getElementById('" + Button1.ClientID + "').click();}else{}";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strSaveAlert, true);
            //    return;
            //}
            //else
            //{
            string strSaveAlert = "alert('__ALERT__');";
            strSaveAlert = "document.getElementById('" + Button1.ClientID + "').click();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strSaveAlert, true);
            return;
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    public string LoadXMLValues()
    {
        StringBuilder strBuilder = new StringBuilder();
        try
        {
            TotActual = 0;
            TotReq = 0;
            decimal intActualScore = 0;


            strBuilder.Append("<Root>");
            //Description='Bank Statement' PercentageofImportance='10.000' RequiredScore='7' 
            //ActualScore='1' ID='2' CreditScoreItemID='2'

            foreach (GridViewRow grvRow in gvCreditScoreDetails.Rows)
            {
                string strDescription = ((Label)grvRow.FindControl("lblDescription")).Text;//grvRow.Cells[0].Text;
                string strPercentageofImportance = ((Label)grvRow.FindControl("lblPerImp")).Text;//grvRow.Cells[1].Text;
                string strRequiredScore = ((Label)grvRow.FindControl("lblReqScore")).Text;//grvRow.Cells[2].Text;

                Label lblGlobalParameterID = (Label)grvRow.FindControl("lblGlobalParameterID");
                Label lblCreditScoreItemID = (Label)grvRow.FindControl("lblCreditScoreItemID");
                TextBox txtActualScore = (TextBox)grvRow.FindControl("txtActualValues");

                if (txtActualScore.Text != "")
                {
                    TotActual = TotActual + Convert.ToDecimal(txtActualScore.Text);
                    intActualScore = Convert.ToDecimal(txtActualScore.Text);
                }
                if (strRequiredScore != "" && strDescription.ToUpper() != "HYGIENE FACTOR") TotReq = TotReq + Convert.ToDecimal(strRequiredScore);

                if (txtActualScore.Text != "")
                {
                    if (Convert.ToDouble(txtActualScore.Text) > Convert.ToDouble(strRequiredScore))
                    {
                        intActualScore = Convert.ToDecimal(strRequiredScore);
                    }
                }

                strBuilder.Append("<Details Description ='" + strDescription + "' PercentageofImportance='" + strPercentageofImportance + "'  CreditScoreItemID='" + lblCreditScoreItemID.Text + "' ");
                strBuilder.Append(" RequiredScore='" + strRequiredScore + "' ActualScore='" + intActualScore.ToString() + "' ID='" + lblGlobalParameterID.Text + "' /> ");
            }
            strBuilder.Append("</Root>");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        return strBuilder.ToString();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //wf Cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();

            if (Request.QueryString["Popup"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "window.close();", true);
            }
            else
            {
                if (strMode == "")
                {
                    if (TabContainerCPT.ActiveTabIndex != 0)
                    {
                        TabContainerCPT.ActiveTabIndex = 0;
                        TabContainerCPT.Tabs[1].Enabled = false;
                        TabContainerCPT.Tabs[2].Enabled = false;
                        TabContainerCPT.Tabs[0].Enabled = true;
                        divSave.Visible = false;

                        foreach (GridViewRow grvEnquiryPagingRow in grvEnquiryPaging.Rows)
                        {
                            CheckBox chkselect = ((CheckBox)grvEnquiryPagingRow.FindControl("chkselect"));
                            chkselect.Enabled = true;
                            chkselect.Checked = false;
                        }
                    }
                    else
                        Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=CRPT&MultipleDNC=1");
                }
                else
                {
                    Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=CRPT&MultipleDNC=1");
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //ddlStatus.SelectedIndex = 5;
            //txtStatus
            foreach (GridViewRow objGridRow in gvCreditScoreDetails.Rows)
            {
                TextBox txtActualScore = new TextBox();
                txtActualScore = (TextBox)objGridRow.FindControl("txtActualValues");
                if (txtActualScore.Enabled)
                {
                    txtActualScore.Text = "";
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region ScoreGrid method

    private bool FunPriValidateActualScore()
    {
        bool blnIsValid = true;
        try
        {
            TabContainerCPT.ActiveTabIndex = 1;
            TabContainerCPT.Tabs[1].Enabled = true;
            TabContainerCPT.Tabs[2].Enabled = true;
            TabContainerCPT.Tabs[0].Enabled = false;

            if (gvCreditScoreDetails.Rows.Count == 0)
            {
                //lblErrorMessage.Text = strMessage + "   "+Resources.LocalizationResources.GCPT_GlobalCreditParam;
                Utility.FunShowAlertMsg(this, Resources.LocalizationResources.GCPT_GlobalCreditParam);
                blnIsValid = false;
                TabContainerCPT.ActiveTabIndex = 1;
                TabContainerCPT.Tabs[1].Enabled = true;
                TabContainerCPT.Tabs[2].Enabled = true;
                TabContainerCPT.Tabs[0].Enabled = false;
                return blnIsValid;
            }
            foreach (GridViewRow gvRow in gvCreditScoreDetails.Rows)
            {
                TextBox txtActualScore = (TextBox)gvRow.FindControl("txtActualValues");
                string strRequiredScore = ((Label)gvRow.FindControl("lblReqScore")).Text;  //gvRow.Cells[2].Text;
                if (txtActualScore.Enabled)
                {
                    if (string.IsNullOrEmpty(txtActualScore.Text.Trim()))
                    {
                        //lblErrorMessage.Text = strMessage + "   " + Resources.LocalizationResources.GCPT_ActualScores;
                        Utility.FunShowAlertMsg(this, Resources.LocalizationResources.GCPT_ActualScores);
                        TabContainerCPT.ActiveTabIndex = 1;
                        TabContainerCPT.Tabs[1].Enabled = true;
                        TabContainerCPT.Tabs[2].Enabled = true;
                        TabContainerCPT.Tabs[0].Enabled = false;
                        txtActualScore.Focus();
                        blnIsValid = false;
                        return blnIsValid;
                    }
                    if (!string.IsNullOrEmpty(strRequiredScore))
                    {
                        decimal dclRequiredScore = Convert.ToDecimal(strRequiredScore);
                        if (Convert.ToDecimal(txtActualScore.Text.Trim()) > dclRequiredScore)
                        {
                            //lblErrorMessage.Text = strMessage + "   " + Resources.LocalizationResources.GCPT_ActualScoresGreater;
                            Utility.FunShowAlertMsg(this, Resources.LocalizationResources.GCPT_ActualScoresGreater);
                            TabContainerCPT.ActiveTabIndex = 1;
                            TabContainerCPT.Tabs[1].Enabled = true;
                            TabContainerCPT.Tabs[2].Enabled = true;
                            TabContainerCPT.Tabs[0].Enabled = false;
                            txtActualScore.Focus();
                            blnIsValid = false;
                            return blnIsValid;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        return blnIsValid;
    }

    private bool FunPriSearchDefaultScoreItems(string strSearchScoreItem)
    {
        bool blnIsExist = true;
        try
        {
            string[] strDefaultScoreItems = new string[2];
            strDefaultScoreItems[0] = "CREDIT SCORE";
            strDefaultScoreItems[1] = "COLLATERAL";

            foreach (string strScoreItem in strDefaultScoreItems)
            {
                if (strScoreItem.ToUpper() == strSearchScoreItem.ToUpper())
                {
                    blnIsExist = false;
                    return blnIsExist;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        return blnIsExist;
    }

    private void FunPriToggleCreditScoreItems()
    {
        try
        {
            if (gvCreditScoreDetails.Rows.Count > 0)
            {
                foreach (GridViewRow objGridRow in gvCreditScoreDetails.Rows)
                {
                    TextBox txtActualScore = new TextBox();
                    txtActualScore = (TextBox)objGridRow.FindControl("txtActualValues");
                    //if (!FunPriSearchDefaultScoreItems(objGridRow.Cells[0].Text))
                    if (!FunPriSearchDefaultScoreItems(((Label)objGridRow.FindControl("lblDescription")).Text))
                    {
                        txtActualScore.Enabled = false;
                        txtActualScore.ReadOnly = true;
                        if (txtActualScore.Text == "" || txtActualScore.Text == string.Empty)
                            txtActualScore.Text = "0";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    //  #region Local Methods

    private DataSet FunPriGetAppraisalDetails()
    {
        DataSet dtAppraisal = new DataSet();
        CreditParameterMgtServicesReference.CreditParameterMgtServicesClient ObjCreditParameterMgtServices = new CreditParameterMgtServicesReference.CreditParameterMgtServicesClient();

        try
        {

            if (intCreditParamTransId > 0)
            {
                byte[] byte_Menu = ObjCreditParameterMgtServices.FunPubGetCreditParameterTransaction(intCreditParamTransId);
                dtAppraisal = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_Menu, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            }
            else
            {
                //if (rdnlCreditType.SelectedIndex == 0)
                //{
                //dtAppraisal = ObjCreditParameterMgtServices.FunPubGetEnquiryCustomerAppraisalCPT(0, intCompanyId, intAppraisalId, intPageSize, intCurrentPage, strSearchValue, strOrderBy, intTotalRecords);
                byte[] byte_Menu = ObjCreditParameterMgtServices.FunPubGetEnquiryCustomerAppraisalCPT(Convert.ToInt32(ddlDocumentType.SelectedValue), intCompanyId, intAppraisalId, intPageSize, intCurrentPage, strSearchValue, strOrderBy, intTotalRecords);
                dtAppraisal = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_Menu, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
                //}
                //else
                //{
                //dtAppraisal = ObjCreditParameterMgtServices.FunPubGetEnquiryCustomerAppraisalCPT(1, intCompanyId, intAppraisalId, intPageSize, intCurrentPage, strSearchValue, strOrderBy, intTotalRecords);
                //byte[] byte_Menu = ObjCreditParameterMgtServices.FunPubGetEnquiryCustomerAppraisalCPT(1, intCompanyId, intAppraisalId, intPageSize, intCurrentPage, strSearchValue, strOrderBy, intTotalRecords);
                //dtAppraisal = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_Menu, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
                //}
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjCreditParameterMgtServices.Close();
        }

        return dtAppraisal;

    }

    private void FunPriShowAppraisalDetails()
    {
        try
        {
            FunPriBindGrid(ddlDocumentType.SelectedValue);
            FunSetDateFormat();

            if (ddlDocumentType.SelectedValue == "6")
            {
                grvEnquiryPaging.Columns[4].Visible = false;
                grvEnquiryPaging.Columns[5].Visible = false;
                grvEnquiryPaging.Columns[6].Visible = false;
                grvEnquiryPaging.Columns[7].Visible = false;
                grvEnquiryPaging.Columns[8].Visible = true;
            }
            else
            {
                grvEnquiryPaging.Columns[4].Visible = true;
                grvEnquiryPaging.Columns[5].Visible = true;
                grvEnquiryPaging.Columns[6].Visible = true;
                grvEnquiryPaging.Columns[7].Visible = true;
                grvEnquiryPaging.Columns[8].Visible = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriLoadAppraisalDetails()
    {
        try
        {
            dtAppraisalDetails = FunPriGetAppraisalDetails();
            //  DataSet dtAppraisalDetails = FunPriBindGrid();
            if (dtAppraisalDetails.Tables[0].Rows.Count > 0)
            {

                if (intCreditParamTransId > 0)
                {
                    txtLOB.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["LOB_Name"]);
                    if (Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Product_Name"]) != "")
                    {
                        txtBranch.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Location"]);
                        txtEnquiryNo.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Doc_Number"]);
                        if (dtAppraisalDetails.Tables[0].Rows[0]["Doc_Date"].ToString() != "") txtEnquiryDate.Text = DateTime.Parse(Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Doc_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                        txtProductCode.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Product_Name"]);

                        lblBranch.Text = "Location";
                        lblProductCode.Text = "Product";
                        trEnquiryDate.Visible = true;
                        trEnqNo.Visible = true;
                    }
                    else
                    {
                        trEnquiryDate.Visible = false;
                        trEnqNo.Visible = false;
                        lblBranch.Text = "Constitution";
                        lblProductCode.Text = "Group Name";

                        txtBranch.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Constitution_Name"]);
                        txtProductCode.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["GroupName"]);
                    }
                    txtCreditParamNumber.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["CreditParamNumber"]);
                    txtCreditParamDate.Text = DateTime.Parse(Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["CreditParamDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW); ;
                    ddlStatus.SelectedValue = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["CreditParamStatus_Id"]);
                    txtStatus.Text = ddlStatus.SelectedItem.Text;
                }
                else
                {
                    hdnLOBId.Value = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["LOB_Id"]);
                    txtLOB.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["LOB_Name"]);


                    if (ddlDocumentType.SelectedValue == "6") //From EnquiryNumber
                    {
                        trEnquiryDate.Visible = false;
                        trEnqNo.Visible = false;
                        txtBranch.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Constitution_Name"]);
                        txtProductCode.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["GroupName"]);
                        lblBranch.Text = "Constitution";
                        lblProductCode.Text = "Group Name";


                    }
                    else // From Customer
                    {
                        txtEnquiryNo.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Doc_Number"]);
                        txtEnquiryDate.Text = DateTime.Parse(Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Doc_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                        txtProductCode.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Product_Name"]);
                        lblBranch.Text = "Location";
                        lblProductCode.Text = "Product";
                        trEnquiryDate.Visible = true;
                        trEnqNo.Visible = true;
                        txtBranch.Text = Convert.ToString(dtAppraisalDetails.Tables[0].Rows[0]["Location"]);
                    }
                    txtCreditParamNumber.Text = "";
                    txtCreditParamDate.Text = DateTime.Now.Date.ToString(ObjS3GSession.ProDateFormatRW);
                    ddlStatus.SelectedIndex = 1;
                    txtStatus.Text = ddlStatus.SelectedItem.Text;
                }
                S3GCustomerPermAddress.SetCustomerDetails(dtAppraisalDetails.Tables[0].Rows[0], true);
                TabContainerCPT.ActiveTabIndex = 1;
                TabContainerCPT.Tabs[1].Enabled = true;
                TabContainerCPT.Tabs[2].Enabled = true;
                gvCreditScoreDetails.DataSource = dtAppraisalDetails.Tables[1];
                gvCreditScoreDetails.DataBind();
                FunPriToggleCreditScoreItems();

                if (dtAppraisalDetails.Tables[2].Rows.Count > 0)
                {
                    ViewState["dtOthers"] = dtAppraisalDetails.Tables[2];
                    grvOtherInfo.DataSource = dtAppraisalDetails.Tables[2];
                    grvOtherInfo.DataBind();

                    ViewState["Global_Credit_Parameter_Id"] = dtAppraisalDetails.Tables[3].Rows[0]["Global_Credit_Parameter_Id"].ToString();
                }
                else
                {
                    FunProInitializOtherGrid();
                }
                ViewState["GroupRef"] = dtAppraisalDetails.Tables[3];
                FunProLoadNumFooterDDL(dtAppraisalDetails.Tables[3].Copy());

                double Total = 0;
                foreach (GridViewRow objGridRow in gvCreditScoreDetails.Rows)
                {
                    TextBox txtActualScore = new TextBox();
                    txtActualScore = (TextBox)objGridRow.FindControl("txtActualValues");
                    if ((txtActualScore.Text != "") && (txtActualScore.Text != null))
                    {
                        Total = Convert.ToDouble(txtActualScore.Text) + Convert.ToDouble(Total);
                        txtActualScore.Focus();
                    }
                }

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriSetResourceSettings()
    {
        try
        {
            lblHeading.Text = Resources.LocalizationResources.GCPT_lblHeading;
            lblLOB.Text = Resources.LocalizationResources.GCPT_lblLOB;
            lblBranch.Text = Resources.LocalizationResources.GCPT_lblBranch;
            lblEnquiryNo.Text = Resources.LocalizationResources.GCPT_lblEnquiryNo;
            lblEnquiryDate.Text = Resources.LocalizationResources.GCPT_lblEnquiryDate;
            lblProductCode.Text = Resources.LocalizationResources.GCPT_lblProductCode;
            lblCreditParamNumber.Text = Resources.LocalizationResources.GCPT_lblCreditParamNumber;
            lblCreditParamDate.Text = Resources.LocalizationResources.GCPT_lblCreditParamDate;
            lblStatus.Text = Resources.LocalizationResources.GCPT_lblStatus;
            btnSave.Text = Resources.LocalizationResources.GCPT_btnSave;
            btnClear.Text = Resources.LocalizationResources.GCPT_btnClear;
            rfvStatus.ErrorMessage = Resources.LocalizationResources.GCPT_rfvStatus;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    //#endregion

    #region RadioButtonList Events
    protected void rdnlCreditType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hdnSortDirection.Value = "DESC";
            hdnSortExpression.Value = "";
            hdnOrderBy.Value = "";
            hdnSearch.Value = string.Empty;

            FunPriShowAppraisalDetails();
            grvEnquiryPaging.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = "Due to mismatched data, unable to Show";
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }
    #endregion

    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        arrSearchVal = grvEnquiryPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        grvEnquiryPaging.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        grvEnquiryPaging.FunPriSetSearchValue(arrSearchVal);
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
                    if (arrSortCol[iCount].ToString() == "DocNo")
                    {
                        switch (ddlDocumentType.SelectedValue)
                        {
                            case "1"://Enquiry
                                strSearchVal += " and EU.Enquiry_No like '%" + arrSearchVal[iCount].ToString() + "%'";
                                break;
                            case "2"://Pricing
                                strSearchVal += " and Pricing.Business_Offer_Number like '%" + arrSearchVal[iCount].ToString() + "%'";
                                break;
                            case "3"://App
                                strSearchVal += " and App.Application_Number like '%" + arrSearchVal[iCount].ToString() + "%'";
                                break;
                            default:
                                strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                                break;
                        }
                    }
                    else
                        strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                }
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid(ddlDocumentType.SelectedValue);
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvEnquiryPaging.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

            foreach (GridViewRow objGridRow in grvEnquiryPaging.Rows)
            {
                Label EnqDt = (Label)objGridRow.FindControl("lblEnquiryDate");
                if (EnqDt.Text.Trim() != "") EnqDt.Text = Utility.StringToDate(EnqDt.Text).ToShortDateString();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunSetDateFormat()
    {
        try
        {
            if (Convert.ToInt32(ddlDocumentType.SelectedValue) != 6)
            {
                foreach (GridViewRow objGridRow in grvEnquiryPaging.Rows)
                {
                    Label EnqDt = (Label)objGridRow.FindControl("lblEnquiryDate");
                    if (EnqDt.Text.Trim() != "") EnqDt.Text = Utility.StringToDate(EnqDt.Text).ToShortDateString();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
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
            // Retrieve the last strColumn that was sorted.
            // Check if the same strColumn is being sorted.
            // Otherwise, the default value can be returned.
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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
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

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid(ddlDocumentType.SelectedValue);
            FunSetDateFormat();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvEnquiryPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvEnquiryPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region Grid Events
    protected void grvEnquiryPaging_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridViewRow objGridRow = grvEnquiryPaging.Rows[e.NewEditIndex];
            ViewState["AppraisalId"] = intAppraisalId = Convert.ToInt32(((Label)objGridRow.FindControl("lblEnqCusAppId")).Text);
            ViewState["CustomerId"] = intCustomerId = Convert.ToInt32(((Label)objGridRow.FindControl("lblCustomerId")).Text);
            if (Convert.ToInt32(ddlDocumentType.SelectedValue) != 6)
            {
                ViewState["EnquiryResponseId"] = intEnquiryResponseId = Convert.ToInt32(((Label)objGridRow.FindControl("lblEnquiryResponseId")).Text);
            }
            FunPriLoadAppraisalDetails();
            FunPriBindGrid(ddlDocumentType.SelectedValue);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = "Due to mismatched data,unable to view the Enquiry/Customer Details";
        }
    }


    #endregion

    protected void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hdnSortDirection.Value = "DESC";
            hdnSortExpression.Value = "";
            hdnOrderBy.Value = "";
            hdnSearch.Value = string.Empty;
            pnlCreateCreditParam.Visible = true;
            grvEnquiryPaging.Visible = true;
            ucCustomPaging.Visible = true;




            //FunPriShowAppraisalDetails();



            if (Convert.ToInt32(ddlDocumentType.SelectedValue) > 0)
            {

                if (ddlDocumentType.SelectedValue == "1")
                {
                    FunPriBindGrid("1"); //Enquiry
                    //chkEnqNO_CheckedChanged(null, null);

                }
                else if (ddlDocumentType.SelectedValue == "2")
                {

                    FunPriBindGrid("2"); //Pricing
                    //chkEnqNO_CheckedChanged(null, null);
                }
                else if (ddlDocumentType.SelectedValue == "3")
                {

                    FunPriBindGrid("3"); //Application
                    //chkEnqNO_CheckedChanged(null, null);
                }
                else if (ddlDocumentType.SelectedValue == "6")
                {

                    FunPriBindGrid("6"); //Customer
                    //chkCustomer_CheckedChanged(null, null);
                }
            }
            grvEnquiryPaging.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
                    TabContainerCPT.ActiveTabIndex = 0;
                    //FunPriShowAppraisalDetails();
                    //FunPriBindGrid(ddlDocumentType.SelectedValue);
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    btnSave.Enabled = bCreate;
                    btnClear.Enabled = true;
                    break;

                case 1: // Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    TabContainerCPT.Tabs[0].Visible = false;
                    TabContainerCPT.ActiveTabIndex = 1;
                    //FunPriBindGrid(ddlDocumentType.SelectedValue);
                    //FunPriBindGrid();
                    //FunPriLoadDetails(intCreditParamTransId);
                    FunPriLoadAppraisalDetails();
                    btnSave.Enabled = bModify;
                    btnCancel.Enabled = false;
                    ddlStatus.Enabled = false;
                    if (ddlStatus.SelectedItem.ToString().ToLower() == "approved" || ddlStatus.SelectedItem.ToString().ToLower() == "rejected")
                    {
                        if (dtAppraisalDetails.Tables[0].Rows[0]["Reject_Page"].ToString() == "Approval")
                        {

                            btnSave.Enabled = false;
                            foreach (GridViewRow objGridRow in gvCreditScoreDetails.Rows)
                            {
                                TextBox txtActualScore = new TextBox();
                                txtActualScore = (TextBox)objGridRow.FindControl("txtActualValues");
                                txtActualScore.ReadOnly = true;
                            }

                            FunProDisableOthersGrid();
                        }

                        if (ddlStatus.SelectedItem.ToString().ToLower() == "approved")
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert1", "alert('The Selected Document Number is Approved Already');", true);
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert1", "alert('The Selected Document Number is rejected Already');", true);
                    }
                    if (dtAppraisalDetails.Tables.Count > 4)
                    {
                        if (dtAppraisalDetails.Tables[4] != null && dtAppraisalDetails.Tables[4].Rows.Count > 0)
                        {
                            btnSave.Enabled = false;
                            foreach (GridViewRow objGridRow in gvCreditScoreDetails.Rows)
                            {
                                TextBox txtActualScore = new TextBox();
                                txtActualScore = (TextBox)objGridRow.FindControl("txtActualValues");
                                txtActualScore.ReadOnly = true;
                            }
                            FunProDisableOthersGrid();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert1", "alert('The Selected Document Number is partially approved ');", true);
                        }
                    }

                    break;
                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FunPriLoadAppraisalDetails();
                    TabContainerCPT.Tabs[0].Visible = false;
                    TabContainerCPT.ActiveTabIndex = 1;
                    if (bClearList)
                    {
                        ddlStatus.ClearDropDownList();
                    }
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    gvCreditScoreDetails.Enabled = false;
                    ddlStatus.Enabled = false;

                    grvOtherInfo.FooterRow.Visible = false;
                    grvOtherInfo.Columns[grvOtherInfo.Columns.Count - 1].Visible = false;

                    for (int i = 0; i <= grvOtherInfo.Rows.Count - 1; i++)
                    {
                        TextBox txtValue = (TextBox)grvOtherInfo.Rows[i].FindControl("txtValue");
                        TextBox txtRemarks = (TextBox)grvOtherInfo.Rows[i].FindControl("txtRemarks");

                        txtValue.ReadOnly = txtRemarks.ReadOnly = true;
                    }

                    FunProDisableOthersGrid();

                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunProDisableOthersGrid()
    {
        foreach (GridViewRow objGridRow in grvOtherInfo.Rows)
        {
            TextBox txtValue = (TextBox)objGridRow.FindControl("txtValue");
            TextBox txtRemarks = (TextBox)objGridRow.FindControl("txtRemarks");
            txtValue.ReadOnly = txtRemarks.ReadOnly = true;
        }

        grvOtherInfo.Columns[grvOtherInfo.Columns.Count - 1].Visible = false;
        grvOtherInfo.FooterRow.Visible = false;
    }

    //  Modified By Mani
    public void SumScore(object sender, EventArgs e)
    {
        try
        {
            double Total = 0;

            foreach (GridViewRow objGridRow in gvCreditScoreDetails.Rows)
            {
                TextBox txtActualScore = new TextBox();
                txtActualScore = (TextBox)objGridRow.FindControl("txtActualValues");
                if ((txtActualScore.Text != "") && (txtActualScore.Text != null))
                {
                    Total = Convert.ToDouble(txtActualScore.Text) + Convert.ToDouble(Total);
                    txtActualScore.Focus();
                }
                //txtTotal.Text = ("Actual Score Total:" + Total).ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }
    protected void chkselect_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            intSerialNumber = 0;
            CheckBox chkselect = null;
            foreach (GridViewRow grvEnquiryPagingRow in grvEnquiryPaging.Rows)
            {
                chkselect = ((CheckBox)grvEnquiryPagingRow.FindControl("chkselect"));
                if (chkselect.Checked)
                {
                    intSerialNumber = Convert.ToInt32(chkselect.Attributes["SerialNumber"].ToString());
                }
                else
                    chkselect.Enabled = false;
            }

            if (intSerialNumber == 0)
            {
                foreach (GridViewRow grvEnquiryPagingRow in grvEnquiryPaging.Rows)
                {
                    chkselect = ((CheckBox)grvEnquiryPagingRow.FindControl("chkselect"));
                    chkselect.Enabled = true;
                }
            }
            if (intSerialNumber > 0)
            {
                FunPriLoadDetails(intSerialNumber);
                divSave.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }

    private void FunPriLoadDetails(int enqcusid)
    {
        //S3G_Org_Getcustomerenquirydetails

        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //if (rdnlCreditType.SelectedIndex == 0)
        //    dictParam.Add("@Appraisal", "0");
        //else
        dictParam.Add("@Appraisal", ddlDocumentType.SelectedValue);
        dictParam.Add("@Company_ID", intCompanyId.ToString());
        dictParam.Add("@Enq_cus_ID", enqcusid.ToString());
        DataTable dtAppraisalDetails1 = Utility.GetDefaultData("S3G_Org_Getcustomerenquirydetails", dictParam);
        //DataSet dtAppraisalDetails1 = new DataSet();

        //  DataSet dtAppraisalDetails = FunPriBindGrid();
        DataRow dtRow = dtAppraisalDetails1.Rows[0];
        try
        {
            if (intCreditParamTransId > 0)
            {
                if (Convert.ToString(dtRow["LOB_Name"]) != "")
                {
                    txtLOB.Text = Convert.ToString(dtRow["LOB_Name"]);
                    txtBranch.Text = Convert.ToString(dtRow["Location"]);
                    txtEnquiryNo.Text = Convert.ToString(dtRow["Doc_No"]);
                    txtEnquiryDate.Text = DateTime.Parse(Convert.ToString(dtRow["Doc_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    txtProductCode.Text = Convert.ToString(dtRow["Product_Name"]);
                    ViewState["AppraisalId"] = Convert.ToString(dtRow["Enq_cus_App_ID"]); // = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Enq_Cus_App_Id"]);
                    ViewState["CustomerId"] = Convert.ToString(dtRow["Customer_Id"]);// = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Customer_Id"]);
                    ViewState["EnquiryResponseId"] = Convert.ToString(dtRow["Document_Type_ID"]);

                    lblBranch.Text = "Location";
                    lblProductCode.Text = "Product";
                    trEnquiryDate.Visible = true;
                    trEnqNo.Visible = true;
                }
                else
                {
                    trEnquiryDate.Visible = false;
                    trEnqNo.Visible = false;
                    lblBranch.Text = "Constitution";
                    lblProductCode.Text = "Group Name";

                    txtBranch.Text = Convert.ToString(dtRow["Constitution_Name"]);
                    txtProductCode.Text = Convert.ToString(dtRow["GroupName"]);
                }
                txtCreditParamNumber.Text = Convert.ToString(dtRow["CreditParamNumber"]);
                txtCreditParamDate.Text = DateTime.Parse(Convert.ToString(dtRow["CreditParamDate"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW); ;
                ddlStatus.SelectedValue = Convert.ToString(dtRow["CreditParamStatus_Id"]);
                txtStatus.Text = ddlStatus.SelectedItem.Text;
                S3GCustomerPermAddress.SetCustomerDetails(dtRow, true);
            }
            else
            {
                hdnLOBId.Value = Convert.ToString(dtRow["LOB_Id"]);
                txtLOB.Text = Convert.ToString(dtRow["LOB_Name"]);
                txtBranch.Text = Convert.ToString(dtRow["Location"]);

                if (ddlDocumentType.SelectedValue == "6") //From Customer
                {
                    trEnquiryDate.Visible = false;
                    trEnqNo.Visible = false;
                    lblBranch.Text = "Constitution";
                    lblProductCode.Text = "Group Name";

                    txtBranch.Text = Convert.ToString(dtRow["Constitution_Name"]);
                    txtProductCode.Text = Convert.ToString(dtRow["GroupName"]);


                }
                else
                {
                    txtEnquiryNo.Text = Convert.ToString(dtRow["Doc_No"]);
                    txtEnquiryDate.Text = DateTime.Parse(Convert.ToString(dtRow["Doc_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    txtProductCode.Text = Convert.ToString(dtRow["Product_Name"]);
                    ViewState["AppraisalId"] = Convert.ToString(dtRow["Enq_cus_App_ID"]); // = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Enq_Cus_App_Id"]);
                    ViewState["CustomerId"] = Convert.ToString(dtRow["Customer_Id"]);// = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Customer_Id"]);
                    ViewState["EnquiryResponseId"] = Convert.ToString(dtRow["Document_Type_ID"]);
                    lblBranch.Text = "Location";
                    lblProductCode.Text = "Product";
                    trEnquiryDate.Visible = true;
                    trEnqNo.Visible = true;
                }

                txtCreditParamNumber.Text = "";
                txtCreditParamDate.Text = DateTime.Parse(DateTime.Now.Date.ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);
                ddlStatus.SelectedIndex = 1;
                txtStatus.Text = ddlStatus.SelectedItem.Text;
            }
            ViewState["AppraisalId"] = Convert.ToString(dtRow["Enq_cus_App_ID"]); // = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Enq_Cus_App_Id"]);
            ViewState["CustomerId"] = Convert.ToString(dtRow["Customer_Id"]);
            S3GCustomerPermAddress.SetCustomerDetails(dtRow, true);

            TabContainerCPT.ActiveTabIndex = 1;
            TabContainerCPT.Tabs[1].Enabled = true;
            TabContainerCPT.Tabs[2].Enabled = true;
            TabContainerCPT.Tabs[0].Enabled = false;

            intAppraisalId = enqcusid;

            FunProInitializOtherGrid();

            DataSet dtAppraisalDetails = FunPriGetAppraisalDetails();
            if (dtAppraisalDetails != null && dtAppraisalDetails.Tables.Count > 0)
            {
                if (dtAppraisalDetails.Tables[0] != null && dtAppraisalDetails.Tables[0].Rows.Count > 0)
                {
                    if (dtAppraisalDetails.Tables[1] != null)
                    {
                        gvCreditScoreDetails.DataSource = dtAppraisalDetails.Tables[1];
                        gvCreditScoreDetails.DataBind();
                    }
                    FunPriToggleCreditScoreItems();
                }

                if (dtAppraisalDetails.Tables[2] != null && dtAppraisalDetails.Tables[2].Rows.Count > 0)
                {
                    if (dtAppraisalDetails.Tables[2] != null)
                    {
                        ViewState["Global_Credit_Parameter_Id"] = dtAppraisalDetails.Tables[2].Rows[0]["Global_Credit_Parameter_Id"].ToString();
                        FunProLoadNumFooterDDL(dtAppraisalDetails.Tables[2]);
                    }
                }
            }

            /* Added by Vijay */
            if (Request.QueryString["qsViewId"] == null)
            {
                if (gvCreditScoreDetails.Rows.Count == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "LOB and Constitutions combination values are not defined in Global Credit Parameter Master");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvEnquiryPaging_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label SerialNumber = (Label)e.Row.FindControl("SerialNumber");
                CheckBox chkbox = (CheckBox)e.Row.FindControl("chkselect");
                chkbox.Attributes.Add("SerialNumber", SerialNumber.Text.ToString());
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvCreditScoreDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblReqScore = (Label)e.Row.FindControl("lblReqScore");
                Label lblDescription = (Label)e.Row.FindControl("lblDescription");
                TextBox txtActScore = (TextBox)e.Row.FindControl("txtActualValues");
                Label lblGlobalParameterID = (Label)e.Row.FindControl("lblGlobalParameterID");
                //txtActScore.SetDecimalPrefixSuffix(4, 3, true, "Actual Score");

                if (lblReqScore.Text != "")
                {
                    double txtReqScore = 0;
                    if (lblReqScore.Text != "") txtReqScore = Convert.ToDouble(lblReqScore.Text);

                    Label lblPerImp = (Label)e.Row.FindControl("lblPerImp");

                    if (txtReqScore == 0 && txtActScore != null)
                    {
                        txtActScore.Enabled = false;
                        txtActScore.ReadOnly = true;
                        txtActScore.Text = "0";
                        e.Row.Style.Add("display", "none");
                        //e.Row.Visible = false;
                    }
                    if (txtActScore.Text != "") TotActual = TotActual + Convert.ToDecimal(txtActScore.Text);
                    if (lblPerImp.Text != "") TotPercentage = TotPercentage + Convert.ToDecimal(lblPerImp.Text);
                    if (lblReqScore.Text != "" && lblDescription.Text.ToUpper() != "HYGIENE FACTOR") TotReq = TotReq + Convert.ToDecimal(lblReqScore.Text);
                    if ((lblDescription.Text.ToUpper() == "EXISTING PERFORMANCE") && (Convert.ToInt32(lblGlobalParameterID.Text) > 0) && (PageMode != PageModes.Query) && (txtStatus.Text.ToUpper() != "APPROVED"))
                    {
                        Dictionary<string, string> dictParam = new Dictionary<string, string>();
                        dictParam.Add("@Appraisal", ddlDocumentType.SelectedValue);
                        dictParam.Add("@Company_ID", intCompanyId.ToString());
                        dictParam.Add("@Enq_cus_ID", intSerialNumber.ToString());
                        dictParam.Add("@GlobalCreditParamId", lblGlobalParameterID.Text);
                        DataTable dtActualScore = Utility.GetDefaultData("S3G_ORG_CALC_ExistingPerf", dictParam);
                        if (dtActualScore.Rows.Count > 0)
                        {
                            txtActScore.Text = dtActualScore.Rows[0]["ACTUAL_SCORE"].ToString();
                            txtActScore.ReadOnly = true;
                            txtActScore.Enabled = false;
                        }
                        else
                        {

                        }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalPerImp = (Label)e.Row.FindControl("lblTotalPerImp");
                Label lblTotalReqScore = (Label)e.Row.FindControl("lblTotalReqScore");
                Label lblTotalActScore = (Label)e.Row.FindControl("lblTotalActScore");

                lblTotalPerImp.Text = TotPercentage.ToString();
                lblTotalReqScore.Text = TotReq.ToString();
                lblTotalActScore.Text = TotActual.ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string strDocNo = "";
        S3GBusEntity.CreditParameterTransactionEntity objCreditParameterTransactionEntity = new CreditParameterTransactionEntity();
        CreditParameterMgtServicesReference.CreditParameterMgtServicesClient objCreditParameterMgtServicesClient = new CreditParameterMgtServicesReference.CreditParameterMgtServicesClient();
        try
        {
            string strXmlvalue = LoadXMLValues();
            FunProApplyValue();
            if (intCreditParamTransId == 0)
            {
                objCreditParameterTransactionEntity.CompanyId = intCompanyId;
                objCreditParameterTransactionEntity.CreatedBy = intUserId;
                objCreditParameterTransactionEntity.CreatedOn = DateTime.Now;
                objCreditParameterTransactionEntity.CreditParamDate = Utility.StringToDate(txtCreditParamDate.Text);

                //Thangam M on 12/sep/2013
                //As per sudarsan sir discussion status will set to "Under process" for all cases.
                //if (TotActual >= TotReq)
                //    objCreditParameterTransactionEntity.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
                //else
                //    objCreditParameterTransactionEntity.StatusId = 8;
                objCreditParameterTransactionEntity.StatusId = Convert.ToInt32(ddlStatus.SelectedValue);

                objCreditParameterTransactionEntity.AppraisalId = Convert.ToInt32(ViewState["AppraisalId"]);//Convert.ToInt32(Utility.Load("AppraisalId",""));
                if (ViewState["CustomerId"] != null && ViewState["CustomerId"].ToString() != "")
                {
                    objCreditParameterTransactionEntity.CustomerId = Convert.ToInt32(ViewState["CustomerId"]);//Convert.ToInt32(Utility.Load("CustomerId", ""));
                }
                objCreditParameterTransactionEntity.Document_Type = Convert.ToInt32(ddlDocumentType.SelectedValue);
                //if (rdnlCreditType.SelectedIndex == 0)
                //{
                objCreditParameterTransactionEntity.EnquiryResponseId = Convert.ToInt32(ViewState["EnquiryResponseId"]);
                //}
                objCreditParameterTransactionEntity.ModifiedBy = intUserId;
                objCreditParameterTransactionEntity.ModifiedOn = DateTime.Now;
                objCreditParameterTransactionEntity.XmlScoreDetails = strXmlvalue;// strBuilder.ToString();//gvCreditScoreDetails.FunPubFormXml();
                objCreditParameterTransactionEntity.XmlOthers = ((DataTable)ViewState["dtOthers"]).FunPubFormXml();
                intErrorCode = objCreditParameterMgtServicesClient.FunPubInsertCreditParamTransaction(out strDocNo, objCreditParameterTransactionEntity);
            }
            else
            {
                objCreditParameterTransactionEntity.AppraisalId = intCreditParamTransId;
                objCreditParameterTransactionEntity.XmlScoreDetails = strXmlvalue;// strBuilder.ToString();// gvCreditScoreDetails.FunPubFormXml();
                objCreditParameterTransactionEntity.ModifiedBy = intUserId;
                objCreditParameterTransactionEntity.ModifiedOn = DateTime.Now;
                objCreditParameterTransactionEntity.XmlOthers = ((DataTable)ViewState["dtOthers"]).FunPubFormXml();
                intErrorCode = objCreditParameterMgtServicesClient.FunPubModifyCreditParamTransaction(objCreditParameterTransactionEntity);
                strDocNo = txtCreditParamNumber.Text.Trim();
            }
            if (intErrorCode == 0)
            {
                if (intCreditParamTransId > 0)
                {
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    strAlert = strAlert.Replace("__ALERT__", "Credit Parameter updated sucessfully");
                    //if (TotActual >= TotReq)
                    //    strAlert = strAlert.Replace("__ALERT__", "Credit Parameter updated sucessfully");
                    //else
                    //    strAlert = strAlert.Replace("__ALERT__", "Credit Parameter updated sucessfully.But the status was rejected");
                    //Added by Thangam on 03/May/2012
                    btnSave.Enabled = false;
                    //End here
                }
                else
                {
                    strAlert = "Credit Parameter added successfully - " + strDocNo;
                    //if (TotActual >= TotReq)
                    //    strAlert = "Credit Parameter added successfully - " + strDocNo;
                    //else
                    //    strAlert = "Credit Parameter added successfully.But the status was rejected - " + strDocNo;

                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        if (TotActual >= TotReq)
                        {
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strDocNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, int.Parse(ddlDocumentType.SelectedValue));
                                strAlert = "";
                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is Not Assigned"; // To Close the Status.
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                            }
                            //ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode);
                            ShowWFAlertMessage(strDocNo, ProgramCode, strAlert);
                            return;
                        }
                        else     // In case of Rejection ....
                        {
                            try
                            {
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is Not Assigned";
                            }
                        }
                    }
                    else
                    {
                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(null, txtEnquiryNo.Text.Trim(), ProgramCode, null, null, "O", CompanyId, null, null, txtLOB.Text.Trim(), txtProductCode.Text.Trim(), out WFFP))
                        {

                            DataRow dtrForce = WFFP.Rows[0];
                            if (TotActual >= TotReq)
                            {

                                try
                                {
                                    int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), strDocNo, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), int.Parse(ddlDocumentType.SelectedValue));

                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here
                                }
                                catch (Exception ex)
                                {
                                    strAlert = "Work Flow is Not Assigned"; // To Close the Status.
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());

                                }
                            }
                            else   // Incase of Rejection....
                            {
                                try
                                {
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());

                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here
                                }
                                catch (Exception ex)
                                {
                                    strAlert = "Work Flow is Not Assigned";
                                }
                            }

                        }

                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strAlert += @"\n\nWould you like to add one more Credit Parameter?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Added by Thangam on 03/May/2012
                        btnSave.Enabled = false;
                        //End here
                    }
                }
            }
            else if (intErrorCode == 1)
            {
                //strAlert = strAlert.Replace("__ALERT__", "Credit Parameter Number cannot be empty");
                strAlert = strAlert.Replace("__ALERT__", "Document details not defined for Credit Parameter Transaction ");
                strRedirectPageView = "";

                //Changed By Thangam M 0n 16/Feb/2012 to show the buttons
                //divSave.Visible = false;
            }
            else if (intErrorCode == 2)
            {
                //strAlert = strAlert.Replace("__ALERT__", "Credit Parameter Number cannot be empty");
                strAlert = strAlert.Replace("__ALERT__", "Document control has reach the maximium size defined. ");
                strRedirectPageView = "";

                //Changed By Thangam M 0n 16/Feb/2012 to show the buttons
                //divSave.Visible = false;
            }
            else
            {
                if (intCreditParamTransId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Error in updating Credit Parameter details");
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Error in adding Credit Parameter details");
                }
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;

        }
        finally
        {
            objCreditParameterMgtServicesClient.Close();
            // objCreditParameterTransactionEntity = null;
        }
    }

    /* WorkFlow Properties */
    private int WFLOBId { get { return 8; } }
    private int WFProduct { get { return 3; } }

    protected void FunProInitializOtherGrid()
    {
        DataTable dtOthers = new DataTable();
        dtOthers.Columns.Add("Desc");
        dtOthers.Columns.Add("Flag_ID");
        dtOthers.Columns.Add("Value");
        dtOthers.Columns.Add("Flag");
        dtOthers.Columns.Add("Group_Ref");
        dtOthers.Columns.Add("Remarks");
        dtOthers.Columns.Add("RecordId");
        dtOthers.Columns.Add("Key");
        dtOthers.Columns.Add("Link");

        DataRow dRow = dtOthers.NewRow();
        dtOthers.Rows.Add(dRow);

        grvOtherInfo.DataSource = dtOthers;
        grvOtherInfo.DataBind();

        grvOtherInfo.Rows[0].Visible = false;
        dtOthers.Rows.Clear();

        ViewState["dtOthers"] = dtOthers;
    }

    protected void FunProLoadNumFooterDDL(DataTable dt)
    {
        if (grvOtherInfo.FooterRow != null)
        {
            DropDownList ddlFGroupRef = (DropDownList)grvOtherInfo.FooterRow.FindControl("ddlFGroupRef");
            ddlFGroupRef.BindDataTable(dt, new string[] { "Group_Ref", "Group_Ref" });
        }
    }

    protected void grvOtherInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblKey = (Label)e.Row.FindControl("lblKey");
            LinkButton lnkbtnDelete = (LinkButton)e.Row.FindControl("lnkbtnDelete");
            //if (lblKey.Text == "0")
            //{
            //    lnkbtnDelete.Visible = false;
            //}
            //else
            //{
            //    lnkbtnDelete.Visible = true;
            //}
        }
    }

    protected void grvOtherInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunProApplyValue();

        DataTable dtOthers = (DataTable)ViewState["dtOthers"];
        Label lblLink = (Label)grvOtherInfo.Rows[e.RowIndex].FindControl("lblLink");

        //DataRow[] drDelete = dtOthers.Select("Link='" + lblLink.Text + "'");
        //foreach (DataRow dRow in drDelete)
        //{
        //    dRow.Delete();
        //}

        dtOthers.Rows[e.RowIndex].Delete();

        dtOthers.AcceptChanges();

        grvOtherInfo.DataSource = dtOthers;
        grvOtherInfo.DataBind();

        for (int i = 0; i <= dtOthers.Rows.Count - 1; i++)
        {
            dtOthers.Rows[i]["RecordId"] = i.ToString();
        }

        ViewState["dtOthers"] = dtOthers;

        if (dtOthers.Rows.Count == 0)
        {
            FunProInitializOtherGrid();
        }

        FunProLoadNumFooterDDL(((DataTable)ViewState["GroupRef"]).Copy());

    }

    protected void grvOtherInfo_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddNew")
        {
            try
            {
                FunProApplyValue();

                DataTable dtOthers = (DataTable)ViewState["dtOthers"];
                DropDownList ddlFGroupRef = (DropDownList)grvOtherInfo.FooterRow.FindControl("ddlFGroupRef");


                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                Procparam.Add("@Group_Ref_No", ddlFGroupRef.SelectedValue);
                Procparam.Add("@Global_Credit_Parameter_ID", ViewState["Global_Credit_Parameter_Id"].ToString());
                Procparam.Add("@Rows", dtOthers.Rows.Count.ToString());

                DataSet dsNew = Utility.GetDataset("S3G_Org_GetGroupRef_CPT", Procparam);

                if (ddlFGroupRef.SelectedItem.Text == "OTH")
                {
                    DataRow dRow = dtOthers.NewRow();

                    Label lblFFlag = (Label)grvOtherInfo.FooterRow.FindControl("lblFFlag");
                    TextBox txtFDescription = (TextBox)grvOtherInfo.FooterRow.FindControl("txtFDescription");
                    TextBox txtFValue = (TextBox)grvOtherInfo.FooterRow.FindControl("txtFValue");
                    TextBox txtFRemarks = (TextBox)grvOtherInfo.FooterRow.FindControl("txtFRemarks");

                    dRow["Desc"] = txtFDescription.Text;
                    dRow["Flag_ID"] = "-1";
                    dRow["value"] = txtFValue.Text;
                    dRow["Flag"] = lblFFlag.Text;
                    dRow["Group_Ref"] = ddlFGroupRef.SelectedItem.Text;
                    dRow["Remarks"] = txtFRemarks.Text;
                    dRow["RecordId"] = "-1";
                    dRow["Key"] = "-1";
                    dRow["Link"] = "-1";

                    dtOthers.Rows.Add(dRow);
                }
                else
                {
                    dsNew.Tables[0].Rows[dsNew.Tables[0].Rows.Count - 1]["Key"] = "1";
                    dtOthers.Merge(dsNew.Tables[0]);
                }

                ViewState["dtOthers"] = dtOthers;
                grvOtherInfo.DataSource = dtOthers;
                grvOtherInfo.DataBind();

                ViewState["GroupRef"] = dsNew.Tables[1];

                FunProLoadNumFooterDDL(dsNew.Tables[1].Copy());
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    protected void ddlFGroupRef_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlFGroupRef = (DropDownList)sender;

        Label lblFFlag = (Label)grvOtherInfo.FooterRow.FindControl("lblFFlag");
        TextBox txtFDescription = (TextBox)grvOtherInfo.FooterRow.FindControl("txtFDescription");
        TextBox txtFValue = (TextBox)grvOtherInfo.FooterRow.FindControl("txtFValue");
        TextBox txtFRemarks = (TextBox)grvOtherInfo.FooterRow.FindControl("txtFRemarks");

        if (ddlFGroupRef.SelectedItem.Text == "OTH")
        {
            lblFFlag.Visible = txtFDescription.Visible = txtFValue.Visible = txtFRemarks.Visible = true;
            lblFFlag.Text = "OTH";
        }
        else
        {
            lblFFlag.Visible = txtFDescription.Visible = txtFValue.Visible = txtFRemarks.Visible = false;
        }
    }

    protected void FunProApplyValue()
    {
        DataTable dtOthers = (DataTable)ViewState["dtOthers"];
        for (int i = 0; i <= dtOthers.Rows.Count - 1; i++)
        {
            TextBox txtValue = (TextBox)grvOtherInfo.Rows[i].FindControl("txtValue");
            TextBox txtRemarks = (TextBox)grvOtherInfo.Rows[i].FindControl("txtRemarks");

            dtOthers.Rows[i]["Value"] = txtValue.Text;
            dtOthers.Rows[i]["Remarks"] = txtRemarks.Text;
        }
        dtOthers.AcceptChanges();

        ViewState["dtOthers"] = dtOthers;
    }
}


