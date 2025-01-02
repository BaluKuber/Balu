
//Module Name      :   Origination
//Screen Name      :   S3GORGCreditGuideTransaction_Add.aspx
//Created By       :   Ramesh M
//Created Date     :   19-JULY-2010
//Purpose          :   To insert and update Credit score guide transaction details

using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Collections;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;


public partial class Origination_S3GORGCreditGuideTransaction_Add : ApplyThemeForProject
{
    CreditMgtServicesReference.CreditMgtServicesClient ObjCreditScoreClient;
    CreditMgtServices.S3G_ORG_CreditScoreDataTable ObjS3G_ORG_CreditScoreDataTable = new CreditMgtServices.S3G_ORG_CreditScoreDataTable();
    CreditMgtServices.S3G_ORG_CreditGuideTransactionDataTable ObjS3G_ORG_CreditGuideTransactionDataTable = new CreditMgtServices.S3G_ORG_CreditGuideTransactionDataTable();
    SerializationMode SerMode = SerializationMode.Binary;
    string strXML = "";
    string strAlert = "";
    int intResult = 0;
    int intCreditScoreID = 0;
    int intCreditScoreTran_ID = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    int iCount = 0;
    int intRow = 0;
    int intNoofYears = 0;
    bool bYearBind = true;
    int intYear = 0;
    string strDateFormat = string.Empty;
    int intNoofSearch = 6;
    string[] arrSortCol = new string[] { "DocNo", "CM.Customer_Code+'-'+CM.Customer_Name", "CM.Customer_Code+'-'+CM.Customer_Name", "LM.LOB_Code+'-'+LM.LOB_Name", "PM.Product_Code+'-'+PM.Product_Name", "Convert(Varchar(5),COM.Constitution_Code)+'-'+COM.Constitution_Name" };
    string strProcName = SPNames.S3G_ORG_GetEnquiryDetailsCGT_Paging;
    Dictionary<string, string> Procparam = null;
    ArrayList arrSearchVal = new ArrayList(1);
    PagingValues ObjPaging = new PagingValues();
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    Dictionary<string, string> dictLOB = null;
    DataTable dtCreditScore = new DataTable();
    DataTable dtDefault = new DataTable();
    DataTable Dt = new DataTable();
    DataSet DS = new DataSet();

    string strKey = "Insert";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code endS3GORGTransLander.aspx?Code=CPA&create=1&MultipleDNC=1&DNCOption=1&Modify=0
    string strRedirectPage = "../Origination/S3GORGTransLander.aspx?Code=OCGT&MultipleDNC=1";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGCreditGuideTransaction_Add.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=OCGT&MultipleDNC=1';";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
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
        FunPriBindGrid(ddlDocumentType.SelectedValue);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ProgramCode = "043";

        FunPubVisibleGridColumn(false);  //hide credit guide score master column

        uinfo = new UserInfo();
        intCompanyID = uinfo.ProCompanyIdRW;
        UserInfo ObjUserInfo = new UserInfo();
        intUserID = uinfo.ProUserIdRW;
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            if (fromTicket != null)
            {
                string[] arrayIds = fromTicket.Name.Split('-');
                intCreditScoreTran_ID = Convert.ToInt32(arrayIds[0].ToString());
                intCreditScoreID = Convert.ToInt32(arrayIds[1].ToString());
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }
        if (Request.QueryString.Get("qsMode") != null)
            strMode = Request.QueryString.Get("qsMode").ToString();

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end 

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
            FunPriGetNegativeApplicable();
            btnSave.Enabled = false;
            btnAdd.Enabled = true;
            btnGo.Visible = true;
            ddlYear.Visible = false;
            //lbltotalscore.Visible = false;
            // txtTotalScore.Visible = false;
            btnAdd.Visible = false;
            lblYear.Visible = false;
            ddlYear.Enabled = false;
            lblActualtotalscore.Visible = false;
            txtActualTotal.Visible = false;
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
            Session["TotalScore"] = "0";
            Session["Product_ID"] = null;
            Session["Enquiry_Response_ID"] = null;
            Session["Customer_ID"] = null;
            Session["ScoreYearValueModeC"] = "0";

            if (intCreditScoreTran_ID > 0 && intCreditScoreID > 0)
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                hdnCreditScoreID.Value = intCreditScoreID.ToString();  //need to be change
                hdnCreditScoreTran_ID.Value = intCreditScoreTran_ID.ToString();  //need to be change
                FunPriBindLookup();
                FunGetCreditScoreParameterDetails();

                TabContainerCGT.ActiveTabIndex = 1;
                TabCreditGuideQuery.Enabled = false;

            }
            else
            {
                FunPriLoadDocumentType();
                TabContainerCGT.ActiveTabIndex = 0;
                TabContainerCGT.Tabs[1].Enabled = TabContainerCGT.Tabs[2].Enabled = false;
                //FunPriShowAppraisalDetails();
            }

            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if ((intCreditScoreTran_ID > 0) && (strMode == "M"))
            {
                FunPriDisableControls(1);
            }
            else if ((intCreditScoreTran_ID > 0) && (strMode == "Q")) // Query // Modify
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriDisableControls(0);
            }
            //Code end
            if (strMode == "M")
            {
                btnGo.Visible = false;
                btnSave.Enabled = false;
            }
            ProgramCode = "043";

            // Workf Flow Implementation
            if (PageMode == PageModes.WorkFlow)
            {
                PreparePageForWFLoad();
                TabContainerCGT.Tabs[1].Enabled = true;

            }

        }

    }

    protected void btnBranchShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunPriBindGrid(ddlDocumentType.SelectedValue);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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

    private void FunPriGetNegativeApplicable()
    {
        try
        {
            ViewState["Negative"] = "0";

            Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", intCompanyID.ToString());
            DataTable dt = Utility.GetDefaultData("S3G_ORG_Get_GPS_Negative", Procparam);
            if (dt.Rows.Count > 0)
            {
                ViewState["Negative"] = dt.Rows[0][0].ToString();
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PreparePageForWFLoad()
    {
        if (!IsPostBack)
        {
            TabCreditGuideQuery.Enabled = false;
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

            byte[] byte_CreditGuideTransaction = objWorkflowToDoClient.FunPubLoadCreditGuideTransaction(WFSessionValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValues.Document_Type);

            DataSet dsEnquiryAppraisal = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_CreditGuideTransaction, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            int intEnqCusAppId = 0;

            if (dsEnquiryAppraisal.Tables.Count > 0)
            {
                if (dsEnquiryAppraisal != null && dsEnquiryAppraisal.Tables[0].Rows.Count > 0)
                {

                    intEnqCusAppId = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Enq_Cus_App_Id"]);
                    ddlDocumentType.SelectedValue = dsEnquiryAppraisal.Tables[0].Rows[0]["Document_Type"].ToString();
                    TabContainerCGT.Tabs[1].Enabled = true;
                    if (ddlDocumentType.SelectedValue == "1")
                    {
                        lblEnqNo.Text = "Enquiry No.";
                    }
                    else if (ddlDocumentType.SelectedValue == "2")
                    {
                        lblEnqNo.Text = "Pricing No.";
                    }
                    else if (ddlDocumentType.SelectedValue == "3")
                    {
                        lblEnqNo.Text = "Application No.";
                    }
                    FunPriShowEnquiryCustomerDetails(intEnqCusAppId.ToString());
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Customer Appraisal need to enter");
                    Response.Redirect("window.location.href='../Origination/S3GORGEnquiryAppraisal_Add.aspx?qsMode=C';");
                }
            }
        }
    }
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                if (!bCreate)
                {

                    btnSave.Enabled = false;
                    //grvCreditScore.Columns[7].Visible = false;
                }
                else
                {
                    btnGo.Enabled = true;
                }

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                if (!bModify)
                {

                    btnSave.Enabled = true;
                    //btnGridSave.Enabled = false;
                }

                break;

            case -1:// Query Mode


                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }

                if (bClearList)
                {

                }

                btnClear.Enabled = false;
                btnSave.Enabled = false;

                foreach (GridViewRow e in grvNumbers.Rows)
                {
                    TextBox txtValue = (TextBox)e.FindControl("txtValue");
                    Label lblValue = (Label)e.FindControl("lblValue");
                    lblValue.Visible = true;
                    txtValue.Visible = false;
                }


                break;
        }

    }


    private void FunPriBindGrid(string Option)
    {
        try
        {
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            ObjPaging.ProProgram_ID = Convert.ToInt32(ProgramCode);

            FunPriGetSearchValue();


            Procparam = new Dictionary<string, string>();
            //if (rdnlCreditType.SelectedIndex == 0)
            //    Procparam.Add("@AppraisalType", "0");
            //else
            //    Procparam.Add("@AppraisalType", "1");
            Procparam.Add("@AppraisalType", Option.ToString());
            grvPaging.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvPaging.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            if (ddlDocumentType.SelectedValue == "6") //Customer
            {
                grvPaging.Columns[0].Visible = true;
                grvPaging.Columns[1].Visible = true;
                grvPaging.Columns[2].Visible = false;
                grvPaging.Columns[3].Visible = false;
                grvPaging.Columns[4].Visible = true;
                grvPaging.Columns[5].Visible = false;
            }
            else //Enquiry,Pricing,Application
            {
                grvPaging.Columns[0].Visible = true;
                grvPaging.Columns[1].Visible = true;
                grvPaging.Columns[2].Visible = true;
                grvPaging.Columns[3].Visible = true;
                grvPaging.Columns[4].Visible = false;
                grvPaging.Columns[5].Visible = false;
            }

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }

    }
    private void FunPubVisibleGridColumn(bool blnvalue)
    {
        grvCreditScore.Columns[3].Visible = blnvalue;
        grvCreditScore.Columns[4].Visible = blnvalue;
        grvCreditScore.Columns[5].Visible = blnvalue;
        grvCreditScore.Columns[6].Visible = blnvalue;
        grvCreditScore.Columns[7].Visible = blnvalue;
        grvCreditScore.Columns[8].Visible = blnvalue;
    }
    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        arrSearchVal = grvPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        grvPaging.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        grvPaging.FunPriSetSearchValue(arrSearchVal);
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
                ((TextBox)grvPaging.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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

            if (((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass == "styleImageSortingAsc")
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

            FunPriBindGrid(ddlDocumentType.SelectedValue);

            if (strDirection == "ASC")
            {
                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion
    private void FunPriShowAppraisalDetails()
    {
        try
        {
            FunPriBindGrid(ddlDocumentType.SelectedValue);
            if (ddlDocumentType.SelectedValue == "6") //Customer
            {
                grvPaging.Columns[0].Visible = true;
                grvPaging.Columns[1].Visible = true;
                grvPaging.Columns[2].Visible = false;
                grvPaging.Columns[3].Visible = false;
                grvPaging.Columns[4].Visible = true;
                grvPaging.Columns[5].Visible = false;
            }
            else //Enquiry,Pricing,Application
            {
                grvPaging.Columns[0].Visible = true;
                grvPaging.Columns[1].Visible = true;
                grvPaging.Columns[2].Visible = true;
                grvPaging.Columns[3].Visible = true;
                grvPaging.Columns[4].Visible = false;
                grvPaging.Columns[5].Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #region RadioButtonList Events
    //protected void rdnlCreditType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        hdnSearch.Value = string.Empty;
    //        hdnOrderBy.Value = string.Empty;

    //        FunPriShowAppraisalDetails();
    //        if (rdnlCreditType.SelectedIndex == 0)
    //        {
    //            chkEnqNO_CheckedChanged(null, null);
    //        }
    //        else
    //        {
    //            chkCustomer_CheckedChanged(null, null);
    //        }
    //        grvPaging.FunPriClearSearchValue(arrSearchVal);
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //    }
    //}
    #endregion

    protected void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hdnSearch.Value = string.Empty;
            hdnOrderBy.Value = string.Empty;
            btnBranchShowAll.Visible = true;
            grvPaging.Visible = true;
            ucCustomPaging.Visible = true;
            //FunPriShowAppraisalDetails();
            if (Convert.ToInt32(ddlDocumentType.SelectedValue) > 0)
            {

                if (ddlDocumentType.SelectedValue == "1")
                {
                    FunPriBindGrid("1"); //Enquiry
                    chkEnqNO_CheckedChanged(null, null);

                }
                else if (ddlDocumentType.SelectedValue == "2")
                {

                    FunPriBindGrid("2"); //Pricing
                    chkEnqNO_CheckedChanged(null, null);
                }
                else if (ddlDocumentType.SelectedValue == "3")
                {

                    FunPriBindGrid("3"); //Application
                    chkEnqNO_CheckedChanged(null, null);
                }
                else if (ddlDocumentType.SelectedValue == "6")
                {

                    FunPriBindGrid("6"); //Customer
                    chkCustomer_CheckedChanged(null, null);
                }
            }
            grvPaging.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunGetCreditScoreParameterDetails()
    {
        DataSet dsCreditScore = new DataSet();

        ObjCreditScoreClient = new CreditMgtServicesReference.CreditMgtServicesClient();

        try
        {
            lbltotalscore.Visible = true;
            txtTotalScore.Visible = true;
            //            btnAdd.Visible = true;
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@CreditScoreTran_ID", intCreditScoreTran_ID.ToString());
            DS = Utility.GetDataset("S3G_ORG_GetCreditScoreTranParameterDetails", dictDropdownListParam);
            dictDropdownListParam = null;
            if (intCreditScoreID > 0 && intCreditScoreTran_ID > 0)
            {
                btnGo.Visible = false;
                btnAdd.Text = "Next >>";
                //btnAddtAdd.Text = "Next >>";
                //btnAddtAdd.Enabled = true;
                btnAdd.Enabled = true;
                btnSave.Enabled = false;

                txtPastYears.ReadOnly = true;
                txtFutureYears.ReadOnly = true;
                txtPastYearStartFrom.ReadOnly = true;
                if (bYearBind)
                {
                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DS.Tables[0].Rows[0]["product_ID"].ToString()))
                        {
                            Session["Product_ID"] = DS.Tables[0].Rows[0]["Product_ID"].ToString();
                            txtproduct.Text = DS.Tables[0].Rows[0]["Product_Name"].ToString();
                            ViewState["hdnproductid"] = DS.Tables[0].Rows[0]["Product_ID"].ToString();
                            Session["Enquiry_Response_ID"] = DS.Tables[0].Rows[0]["Document_Type_ID"].ToString();
                        }
                        else
                        {
                            txtproduct.Visible = false;
                            Lblproduct.Visible = false;
                        }
                        hdnEdit_Status.Value = DS.Tables[0].Rows[0]["Edit_Status"].ToString();
                        btnSave.Enabled = DS.Tables[0].Rows[0]["Edit_Status"].ToString() == "0" ? false : true;
                        txtlob.Text = DS.Tables[0].Rows[0]["Lob_Name"].ToString();
                        ViewState["hdnLobid"] = DS.Tables[0].Rows[0]["LOB_ID"].ToString();
                        txtConstitution.Text = DS.Tables[0].Rows[0]["Constitution_Name"].ToString();
                        ViewState["hdnConstitutionid"] = DS.Tables[0].Rows[0]["Constitution_ID"].ToString();
                        txtPastYears.Text = DS.Tables[0].Rows[0]["Past_Years"].ToString();
                        txtFutureYears.Text = DS.Tables[0].Rows[0]["Future_Years"].ToString();
                        txtPastYearStartFrom.Text = DS.Tables[0].Rows[0]["PastYear_StartingFrom"].ToString();

                        int intPastYear = Convert.ToInt32(DS.Tables[0].Rows[0]["Past_Years"].ToString());
                        int intFutureYear = Convert.ToInt32(DS.Tables[0].Rows[0]["Future_Years"].ToString());
                        int intPastyearStartFrom = Convert.ToInt32(DS.Tables[0].Rows[0]["PastYear_StartingFrom"].ToString());
                        int intSumYear = 0;

                        intSumYear = intPastYear + intFutureYear;
                        if (intSumYear == 0)
                            intSumYear = 3;
                        for (int i = 1; i < intSumYear + 1; i++)
                        {
                            ddlYear.Items.Add(new ListItem(intPastyearStartFrom.ToString(), i.ToString()));
                            intPastyearStartFrom++;
                        }
                        //lblmsg.Text = ddlYear.SelectedItem.Text;
                        lblAddtMsg.Text = ddlYear.SelectedItem.Text;
                        Session["Customer_ID"] = DS.Tables[0].Rows[0]["Customer_ID"].ToString();

                        S3GCustomerAddress1.SetCustomerDetails(DS.Tables[0].Rows[0], true);
                        //txtCustomerCode.Text = DS.Tables[0].Rows[0]["Customer_Code"].ToString();
                        //txtCustomerName.Text = DS.Tables[0].Rows[0]["Customer_Name"].ToString();
                        //txtCustomerAddress1.Text = Utility.SetCustomerAddress(DS.Tables[0].Rows[0]);
                        //txtMobileNo.Text = DS.Tables[0].Rows[0]["Comm_Mobile"].ToString();
                        //txtEmailID.Text = DS.Tables[0].Rows[0]["Comm_Email"].ToString();
                        //txtWebSite.Text = DS.Tables[0].Rows[0]["Comm_Website"].ToString();
                        if (DS.Tables[0].Rows[0]["Document_Type_ID"].ToString() == "0")
                        {
                            lblEnqNo.Visible = false;
                            txtEnquiryNo.Visible = false;
                        }
                        else
                        {
                            txtEnquiryNo.Text = DS.Tables[0].Rows[0]["Doc_Number"].ToString();
                        }
                        if (DS.Tables[0].Rows[0]["Document_Type"].ToString() == "1")
                        {
                            lblEnqNo.Text = "Enquiry No";
                        }
                        else if (DS.Tables[0].Rows[0]["Document_Type"].ToString() == "2")
                        {
                            lblEnqNo.Text = "Pricing No";
                        }
                        else if (DS.Tables[0].Rows[0]["Document_Type"].ToString() == "3")
                        {
                            lblEnqNo.Text = "Application No";
                        }
                        Session["LOB_ID"] = DS.Tables[0].Rows[0]["LOB_ID"].ToString();
                        Session["Constitution_ID"] = DS.Tables[0].Rows[0]["Constitution_ID"].ToString();


                        //dictDropdownListParam = new Dictionary<string, string>();
                        //dictDropdownListParam.Add("@COMPANY_ID", intCompanyID.ToString());
                        //dictDropdownListParam.Add("@LOB_ID", DS.Tables[0].Rows[0]["LOB_ID"].ToString());
                        //dictDropdownListParam.Add("@CONSTITUTION_ID", DS.Tables[0].Rows[0]["Constitution_ID"].ToString());
                        //if (!string.IsNullOrEmpty(DS.Tables[0].Rows[0]["product_ID"].ToString()))
                        //    dictDropdownListParam.Add("@PRODUCT_ID", DS.Tables[0].Rows[0]["product_ID"].ToString());
                        //DataSet ds = Utility.GetDataset("S3G_ORG_GetCreditGuideTransactionCutoffScore", dictDropdownListParam);
                        //dictDropdownListParam = null;
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{

                        //    //if (Convert.ToDecimal(DS.Tables[0].Rows[0]["TotalScore"]) < Convert.ToDecimal(ds.Tables[0].Rows[0]["Score"]))
                        //    //{
                        //    //    txtTotalScore.Text = DS.Tables[0].Rows[0]["TotalScore"].ToString();
                        //    //    Session["TotalScore"] = DS.Tables[0].Rows[0]["TotalScore"].ToString();
                        //    //    Session["Score"] = ds.Tables[0].Rows[0]["Score"];
                        //    //}
                        //    //else
                        //    //{
                        //    //    txtTotalScore.Text = ds.Tables[0].Rows[0]["Score"].ToString();
                        //    //    Session["Score"] = ds.Tables[0].Rows[0]["Score"].ToString();
                        //    //    Session["TotalScore"] = DS.Tables[0].Rows[0]["TotalScore"];
                        //    //}

                        //    //txtActualTotal.Text = Session["TotalScore"].ToString();
                        //}
                        //else
                        //{
                        //    TabContainerCGT.ActiveTabIndex = 0;
                        //    TabContainerCGT.Tabs[1].Enabled = false;
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert26", "alert('Score item not defined in Global Credit Parameter');", true);
                        //    return;
                        //}
                        //if (ds.Tables[1].Rows.Count == 0)
                        //{
                        //    TabContainerCGT.ActiveTabIndex = 0;
                        //    TabContainerCGT.Tabs[1].Enabled = false;
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert27", "alert('Credit Score Guide not defined in this combination');", true);
                        //    return;
                        //}
                        //ds = null;
                    }

                }
            }
            //if (ddlYear.Items.Count > 0)
            //{
            //    dictDropdownListParam = new Dictionary<string, string>();
            //    dictDropdownListParam.Add("@CreditScoreTran_ID", intCreditScoreTran_ID.ToString());
            //    dictDropdownListParam.Add("@YearValueMaster", (Convert.ToInt32(ddlYear.SelectedItem.Text).ToString() + "-" + (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1).ToString()).ToString()); //ddlYear.SelectedValue.ToString().Trim());
            //    dictDropdownListParam.Add("@YearValueTrnas", ddlYear.SelectedItem.Text.ToString().Trim());
            //    DS = Utility.GetDataset("S3G_ORG_GetCreditScoreTranParameterDetails", dictDropdownListParam);
            //    dictDropdownListParam = null;
            //    if (intCreditScoreTran_ID > 0)
            //    {
            //        dictDropdownListParam = new Dictionary<string, string>();
            //        dictDropdownListParam.Add("@CreditGuideTrns_ID", intCreditScoreTran_ID.ToString());
            //        DataSet dtTemp = Utility.GetDataset("S3G_ORG_GetCreditScoreGuideTransYearDetails", dictDropdownListParam);
            //        if (dtTemp.Tables[0].Columns.Count > 0)
            //        {
            //            grvCreditScoreYearValues.DataSource = dtTemp.Tables[0];
            //            grvCreditScoreYearValues.DataBind();
            //            ViewState["modifydtTemp"] = dtTemp.Tables[0];
            //            lblActualtotalscore.Visible = true;
            //            txtActualTotal.Visible = true;
            //        }

            //        //if (dtTemp.Tables[1].Columns.Count > 0)
            //        //{
            //        //    ////gvrAddtionalYearValue.DataSource = dtTemp.Tables[1];
            //        //    ////gvrAddtionalYearValue.DataBind();
            //        //    //ViewState["modifydtAddtTemp"] = dtTemp.Tables[1];
            //        //}

            //    }

            //}
            //if (intCreditScoreID > 0 && intCreditScoreTran_ID > 0)  //In update mode there is no record in table then execute this loop
            //{
            //    if (DS.Tables[0].Rows.Count == 0 || DS.Tables[1].Rows.Count == 0)
            //    {
            //        if (DS.Tables[0].Rows.Count == 0)
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert12", "alert('No found transaction details');", true);
            //            return;
            //        }
            //        else if (DS.Tables[1].Rows.Count == 0)
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert17", "alert('No found year values details');", true);
            //            return;
            //        }
            //    }
            //}
            dtCreditScore = new DataTable();
            DataRow dr;
            dtCreditScore.Columns.Add("CrScoreParam_ID");
            //dtCreditScore.Columns.Add("Actual_Value");
            //dtCreditScore.Columns.Add("Actual_Score");
            //dtCreditScore.Columns.Add("ReqValue1");
            //dtCreditScore.Columns.Add("Score1");
            dtCreditScore.Columns.Add("Desc");
            dtCreditScore.Columns.Add("FieldAtt");
            dtCreditScore.Columns.Add("NumericAtt");
            dtCreditScore.Columns.Add("DiffPer");
            dtCreditScore.Columns.Add("DiffMark");
            dtCreditScore.Columns.Add("CrdtGuideTrans_Year_Values_ID");
            for (int intColCount = 1; intColCount <= intNoofYears; intColCount++)
            {
                dtCreditScore.Columns.Add("ReqValue" + intColCount.ToString());
                dtCreditScore.Columns.Add("Score" + intColCount.ToString());
            }
            dr = dtCreditScore.NewRow();
            dtCreditScore.Rows.Add(dr);

            if ((intCreditScoreID == 0) && (hdnCreditScoreID.Value == "0"))
            {
                ViewState["CreditScore"] = dtCreditScore;
                grvCreditScore.DataSource = dtCreditScore;
                grvCreditScore.DataBind();
                grvCreditScore.Rows[0].Visible = false;


            }
            else
            {

                FunProLoadCreditScoreDetails();
                return;

                ObjS3G_ORG_CreditScoreDataTable = new CreditMgtServices.S3G_ORG_CreditScoreDataTable();
                CreditMgtServices.S3G_ORG_CreditScoreRow ObjCreditScoreRow = null;

                ObjCreditScoreRow = ObjS3G_ORG_CreditScoreDataTable.NewS3G_ORG_CreditScoreRow();

                ObjCreditScoreRow.Company_ID = intCompanyID;
                ObjCreditScoreRow.Created_By = intUserID;
                ObjCreditScoreRow.CreditScore_Guide_ID = Convert.ToInt32(hdnCreditScoreID.Value);
                ObjCreditScoreRow.YearValue = (Convert.ToInt32(ddlYear.SelectedItem.Text).ToString() + "-" + (Convert.ToInt32(ddlYear.SelectedItem.Text) + 1)).ToString();//Convert.ToInt32(ddlYear.SelectedValue).ToString();

                ObjS3G_ORG_CreditScoreDataTable.AddS3G_ORG_CreditScoreRow(ObjCreditScoreRow);

                byte[] byteCreditScoreDetails = ObjCreditScoreClient.FunPubQueryCreditScoreParameterDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CreditScoreDataTable, SerMode));
                dsCreditScore = (DataSet)ClsPubSerialize.DeSerialize(byteCreditScoreDetails, SerializationMode.Binary, typeof(DataSet));

                if (dsCreditScore.Tables[1].Rows.Count > 0)
                {
                    dtCreditScore = dsCreditScore.Tables[1].Copy();
                    //code added by saran To remove None and repeat - NumericAtt from dtcreditscore table
                    DataRow[] drCreditScre = dtCreditScore.Select("NumericAtt in (6,7)");
                    foreach (DataRow drCredit in drCreditScre)
                        drCredit.Delete();
                    dtCreditScore.AcceptChanges();
                    ViewState["CreditScore"] = dtCreditScore;
                    FunPriBindLookup();

                    grvCreditScore.DataSource = dtCreditScore;
                    grvCreditScore.DataBind();
                    if (ViewState["modifydtTemp"] != null)
                    {
                        grvCreditScoreYearValues.DataSource = (DataTable)ViewState["modifydtTemp"];
                        grvCreditScoreYearValues.DataBind();
                    }
                    if (!btnAdd.Enabled)
                        btnAdd.Enabled = true;

                    if (DS.Tables[3].Rows.Count > 0)//For modify mode
                    {
                        iCount = 0;
                        intRow = 0;
                        ViewState["AdditionalInfo"] = DS.Tables[3];
                        grvAdditional.DataSource = DS.Tables[3];
                        grvAdditional.DataBind();
                    }
                    else if (dsCreditScore.Tables[3].Rows.Count > 0)
                    {
                        iCount = 0;
                        intRow = 0;
                        ViewState["AdditionalInfo"] = dsCreditScore.Tables[3];
                        grvAdditional.DataSource = dsCreditScore.Tables[3];
                        grvAdditional.DataBind();
                        //btnAddtAdd.Enabled = true;
                    }

                    //if (ddlYear.Items.Count == 1 && strMode == "M")
                    //{
                    //    btnAdd.Enabled = false;
                    //}

                    if (intCreditScoreID > 0 && intCreditScoreTran_ID > 0 && Request.QueryString.Get("qsMode").ToString().Trim() == "M")
                    {
                        if (hdnEdit_Status.Value.Trim() == "0")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert17", "alert('This record cannot be modified since it is referred in transaction');", true);
                            return;
                        }
                    }
                }
                else
                {
                    btnAdd.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert30", "alert('Credit Score Guide not defined for this combination');", true);
                    return;

                    FunPriBindLookup();
                    ViewState["CreditScore"] = dtCreditScore;
                    grvCreditScore.DataSource = dtCreditScore;
                    grvCreditScore.DataBind();
                    grvCreditScore.Rows[0].Visible = false;
                    if (!btnAdd.Enabled)
                        btnAdd.Enabled = true;
                    if (ddlYear.Items.Count == 1 && strMode == "M")
                    {
                        btnAdd.Enabled = false;
                    }
                }


            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
        }
        catch (Exception ex)
        {
        }
        finally
        {
            dsCreditScore.Dispose();
            dsCreditScore = null;
            ObjCreditScoreClient.Close();
        }
    }

    //protected void FunPro

    private void FunPriBindYear(int iYearsPresent, int intPastStartFrom)
    {

        ddlYear.Items.Clear();
        for (int intColCount = 1; intColCount <= iYearsPresent; intColCount++)
        {

            ddlYear.Items.Add(new ListItem(intPastStartFrom.ToString(), intColCount.ToString()));
            intPastStartFrom++;
        }

    }

    protected void chkEnqNO_CheckedChanged(object sender, EventArgs e)
    {
        //lbltotalscore.Visible = txtTotalScore.Visible = false;
        hdnSearch.Value = string.Empty;
        btnGo.Enabled = false;
        btnSave.Enabled = false;
        if (ddlDocumentType.SelectedValue == "1" || ddlDocumentType.SelectedValue == "2" || ddlDocumentType.SelectedValue == "3")
        {
            FunPubClear();
            txtproduct.Enabled = true;
        }
        else
        {
            txtproduct.Enabled = false;

        }

    }
    protected void chkCustomer_CheckedChanged(object sender, EventArgs e)
    {
        //lblatotalscore.Visible = txtTotalScore.Visible = false;
        btnGo.Enabled = false;
        btnSave.Enabled = false;
        if (ddlDocumentType.SelectedValue == "6")
        {
            FunPubClear();
            txtproduct.Enabled = false;
        }
        else
        {
            txtproduct.Enabled = true;
        }

    }

    protected void txtPastYears_OnTextChanged(object sender, EventArgs e)
    {

        int currentyear = System.DateTime.Today.Year;
        int currentmonth = System.DateTime.Today.Month;
        if (currentmonth < 4)
        {
            currentyear = currentyear - 1;
        }
        if (txtPastYears.Text == "")
        {
            txtPastYearStartFrom.Text = currentyear.ToString();
        }
        else
        {
            currentyear = currentyear - Convert.ToInt32(txtPastYears.Text);
            txtPastYearStartFrom.Text = currentyear.ToString();
        }


    }

    private void FunPriShowEnquiryCustomerDetails(string Enq_Cus_App_ID)
    {
        try
        {
            //txtCustomerCode.Text = string.Empty;
            //txtCustomerName.Text = string.Empty;
            //txtCustomerAddress1.Text = string.Empty;
            //txtMobileNo.Text = string.Empty;
            //txtEmailID.Text = string.Empty;
            //txtWebSite.Text = string.Empty;

            S3GCustomerAddress1.ClearCustomerDetails();
            txtPastYears.Text = string.Empty;
            txtFutureYears.Text = string.Empty;
            txtPastYearStartFrom.Text = string.Empty;
            //            lblmsg.Text = string.Empty;

            if (grvCreditScore.Rows.Count > 0)
            {
                DataTable Dtnull = new DataTable();
                Dtnull = null;
                grvCreditScore.DataSource = Dtnull;
                grvCreditScore.DataBind();

                grvCreditScoreYearValues.DataSource = Dtnull;
                grvCreditScoreYearValues.DataBind();
            }
            txtEnquiryNo.Text = string.Empty;
            int currentyear = System.DateTime.Today.Year;
            int currentmonth = System.DateTime.Today.Month;
            uinfo = new UserInfo();
            ViewState["Enq_Cus_App_ID"] = Enq_Cus_App_ID;
            dictDropdownListParam = new Dictionary<string, string>();
            dictDropdownListParam.Add("@Enq_Cus_App_ID", Enq_Cus_App_ID);
            if (ddlDocumentType.SelectedValue == "6")
                dictDropdownListParam.Add("@Customer_ID", "1");
            dictDropdownListParam.Add("@Company_ID", intCompanyID.ToString());
            DS = Utility.GetDataset("S3G_ORG_GetEnquiryCustomAppraisalForCreditGuideTrans", dictDropdownListParam);
            uinfo = null;
            dictDropdownListParam = null;



            if (DS.Tables[2].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(DS.Tables[2].Rows[0]["product_code"].ToString()))
                {
                    Lblproduct.Visible = true;
                    txtproduct.Visible = true;
                    Session["Product_ID"] = DS.Tables[2].Rows[0]["Product_ID"].ToString();
                    Session["Enquiry_Response_ID"] = DS.Tables[2].Rows[0]["Document_Type_ID"].ToString();
                    txtproduct.Text = DS.Tables[2].Rows[0]["Product_Code"].ToString() + "-" + DS.Tables[2].Rows[0]["Product_Name"].ToString();
                    ViewState["hdnproductid"] = DS.Tables[2].Rows[0]["Product_ID"].ToString();
                }
                else
                {
                    txtproduct.Visible = false;
                    Lblproduct.Visible = false;
                    ViewState["hdnproductid"] = "";
                }
                if (!string.IsNullOrEmpty(DS.Tables[2].Rows[0]["Doc_Number"].ToString()))
                {
                    lblEnqNo.Visible = true;
                    txtEnquiryNo.Visible = true;
                    txtEnquiryNo.Text = DS.Tables[2].Rows[0]["Doc_Number"].ToString();
                }
                else
                {
                    txtEnquiryNo.Text = "";
                    txtEnquiryNo.Visible = false;
                    lblEnqNo.Visible = false;
                }
                txtConstitution.Text = DS.Tables[2].Rows[0]["Constitution_ID"].ToString() + "-" + DS.Tables[2].Rows[0]["Constitution_Name"].ToString();
                txtlob.Text = DS.Tables[2].Rows[0]["LOB_Code"].ToString() + "-" + DS.Tables[2].Rows[0]["LOB_Name"].ToString();
                ViewState["hdnLobid"] = DS.Tables[2].Rows[0]["LOB_ID"].ToString();
                ViewState["hdnConstitutionid"] = DS.Tables[2].Rows[0]["Constitution_ID"].ToString();

                Session["Customer_ID"] = DS.Tables[2].Rows[0]["Customer_ID"].ToString();
                S3GCustomerAddress1.SetCustomerDetails(DS.Tables[2].Rows[0], true);

                //txtCustomerCode.Text = DS.Tables[2].Rows[0]["Customer_Code"].ToString();
                //txtCustomerName.Text =DS.Tables[2].Rows[0]["Customer_Name"].ToString();
                //txtCustomerAddress1.Text =Utility.SetCustomerAddress(DS.Tables[2].Rows[0]);
                //txtMobileNo.Text = DS.Tables[2].Rows[0]["Comm_Mobile"].ToString();
                //txtEmailID.Text = DS.Tables[2].Rows[0]["Comm_Email"].ToString();
                //txtWebSite.Text = DS.Tables[2].Rows[0]["Comm_Website"].ToString();

                if (currentmonth < 4)
                {
                    currentyear = currentyear - 1;
                }
                if (string.IsNullOrEmpty(txtPastYearStartFrom.Text))
                    txtPastYearStartFrom.Text = currentyear.ToString();

                Session["LOB_ID"] = DS.Tables[2].Rows[0]["LOB_ID"].ToString();
                Session["Constitution_ID"] = DS.Tables[2].Rows[0]["Constitution_ID"].ToString();
                TabContainerCGT.ActiveTabIndex = 1;

                Session["Score"] = null;
                dictDropdownListParam = new Dictionary<string, string>();  ///modify by ramesh get totalscore cutoff  
                dictDropdownListParam.Add("@COMPANY_ID", intCompanyID.ToString());
                dictDropdownListParam.Add("@LOB_ID", DS.Tables[2].Rows[0]["LOB_ID"].ToString());
                dictDropdownListParam.Add("@CONSTITUTION_ID", DS.Tables[2].Rows[0]["Constitution_ID"].ToString());
                if (!string.IsNullOrEmpty(DS.Tables[2].Rows[0]["product_code"].ToString()))
                    dictDropdownListParam.Add("@PRODUCT_ID", DS.Tables[2].Rows[0]["Product_ID"].ToString());
                DataSet ds = Utility.GetDataset("S3G_ORG_GetCreditGuideTransactionCutoffScore", dictDropdownListParam);
                dictDropdownListParam = null;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["Score"] = ds.Tables[0].Rows[0]["Score"].ToString();
                    if (Convert.ToDecimal(Session["Score"].ToString().Trim()) == 0)
                    {
                        TabContainerCGT.ActiveTabIndex = 0;
                        TabContainerCGT.Tabs[1].Enabled = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert28", "alert('Score item not defined in Global Credit Parameter');", true);
                        return;
                    }
                }
                else
                {
                    TabContainerCGT.ActiveTabIndex = 0;
                    TabContainerCGT.Tabs[1].Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert29", "alert('Score item not defined in Global Credit Parameter');", true);
                    return;
                }
                if (ds.Tables[1].Rows.Count == 0)
                {
                    TabContainerCGT.ActiveTabIndex = 0;
                    TabContainerCGT.Tabs[1].Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert30", "alert('Credit Score Guide not defined for this combination');", true);
                    return;
                }
                ds = null;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert31", "alert('No Record Found');", true);
                return;
            }

            txtFutureYears.Enabled = true;
            txtPastYears.Enabled = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }



    public void FunPubClear()
    {
        //txtCustomerCode.Text = string.Empty;
        //txtCustomerName.Text = string.Empty;
        //txtCustomerAddress1.Text = string.Empty;
        //txtMobileNo.Text = string.Empty;
        //txtEmailID.Text = string.Empty;
        //txtWebSite.Text = string.Empty;

        S3GCustomerAddress1.ClearCustomerDetails();
        txtTotalScore.Text = string.Empty;
        txtActualTotal.Text = string.Empty;
        txtPastYears.Text = string.Empty;
        txtPastYears.Enabled = true;
        txtFutureYears.Text = string.Empty;
        txtFutureYears.Enabled = true;
        txtPastYearStartFrom.Text = string.Empty;
        txtEnquiryNo.Text = string.Empty;

        //        lblmsg.Text = "";
        txtproduct.Text = "";
        txtConstitution.Text = "";
        txtlob.Text = "";
        ddlYear.Items.Clear();
        DataTable dt = new DataTable();
        dt = null;
        grvCreditScore.DataSource = dt;
        grvCreditScore.DataBind();

        grvCreditScoreYearValues.DataSource = dt;
        grvCreditScoreYearValues.DataBind();
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            tcCreditScore.ActiveTabIndex = 0;
            Session["TotalScore"] = "0.00";
            Session["PreYearValue"] = "0.00";


            //if (string.IsNullOrEmpty(txtPastYears.Text.Trim()) && string.IsNullOrEmpty(txtFutureYears.Text.Trim()))
            //{
            //    txtFutureYears.Text = "";
            //    txtPastYears.Text = "";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Past/Future year should not be zero or empty');", true);
            //    return;
            //}
            if (txtPastYears.Text.Trim() == "")
                txtPastYears.Text = "0";
            if (txtFutureYears.Text.Trim() == "")
                txtFutureYears.Text = "0";
            //if (Convert.ToInt32(txtPastYears.Text) == 0 && Convert.ToInt32(txtFutureYears.Text) == 0)
            //{
            //    txtFutureYears.Text = "";
            //    txtPastYears.Text = "";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert6", "alert('Past/Future year should not be zero or empty');", true);
            //    return;
            //}



            if (grvCreditScore.Rows.Count > 0)
            {
                DataTable dtDelete = new DataTable();
                dtDelete = null;
                grvCreditScore.DataSource = dtDelete;
                grvCreditScore.DataBind();

                grvCreditScoreYearValues.DataSource = dtDelete;
                grvCreditScoreYearValues.DataBind();

            }
            //if (string.IsNullOrEmpty(txtPastYears.Text.Trim()) && string.IsNullOrEmpty(txtFutureYears.Text.Trim()))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Past/Future year should not be empty');", true);
            //    txtPastYears.Focus();
            //    return;
            //}

            if (blnIsWorkflowApplicable || Session["DocumentNo"] != null)
            {

                uinfo = new UserInfo();
                dictDropdownListParam = new Dictionary<string, string>();
                dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                dictDropdownListParam.Add("@LOB_ID", ViewState["hdnLobid"].ToString());
                dictDropdownListParam.Add("@Product_ID", ViewState["hdnproductid"].ToString());
                dictDropdownListParam.Add("@Constitution_ID", ViewState["hdnConstitutionid"].ToString());
                DS = Utility.GetDataset("S3G_ORG_GetEnquiryCustomAppraisalForCreditGuideTrans", dictDropdownListParam);
                if (DS.Tables[4].Rows.Count > 0)
                {
                    //if (DS.Tables[2].Rows.Count == 0)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Credit score guide not set for this combination');", true);
                    //    return;
                    //}
                    txtTotalScore.Text = "";
                    hdnCreditScoreID.Value = DS.Tables[4].Rows[0]["Crdtscoreguide_Id"].ToString();
                    intCreditScoreID = Convert.ToInt32(hdnCreditScoreID.Value);
                    intYear = Convert.ToInt32(DS.Tables[4].Rows[0]["Years"].ToString());
                    int intPast = 0;
                    int intFuture = 0;
                    int intPastStartFrom = 0;
                    int intAssignYear = 0;
                    int intStartYear = 0;

                    if (!string.IsNullOrEmpty(txtPastYears.Text.Trim()))
                        intPast = Convert.ToInt32(txtPastYears.Text);

                    if (!string.IsNullOrEmpty(txtFutureYears.Text.Trim()))
                        intFuture = Convert.ToInt32(txtFutureYears.Text);

                    if (!string.IsNullOrEmpty(txtPastYearStartFrom.Text.Trim()))
                        intPastStartFrom = Convert.ToInt32(txtPastYearStartFrom.Text.Trim());
                    else
                        intPastStartFrom = DateTime.Now.Year - (intPast + intFuture);

                    intAssignYear = intPast + intFuture;
                    //int intAssignYear = Convert.ToInt32(txtPastYears.Text) + Convert.ToInt32(txtFutureYears.Text);
                    if (intAssignYear <= intYear)
                    {

                        FunPriBindYear(intAssignYear, intPastStartFrom);
                        //                        lblmsg.Text = ddlYear.SelectedItem.Text;
                        //lblAddtMsg.Text = ddlYear.SelectedItem.Text;
                    }
                    else
                    {
                        if (intYear <= 1)
                        {
                            grvCreditScore.Columns.Clear();
                            grvCreditScore.Dispose();
                            //                            lblmsg.Text = string.Empty;
                            lblAddtMsg.Text = string.Empty;
                            txtTotalScore.Text = string.Empty;
                            Utility.FunShowAlertMsg(this.Page, "Maximum " + intYear.ToString() + " Past/Future year(s) can be allowed for this Combination");
                            txtPastYears.Focus();
                            return;

                        }
                        else
                        {
                            grvCreditScore.Columns.Clear();
                            grvCreditScore.Dispose();
                            txtTotalScore.Text = string.Empty;
                            //                            lblmsg.Text = string.Empty;
                            lblAddtMsg.Text = string.Empty;
                            Utility.FunShowAlertMsg(this.Page, "Maximum " + intYear.ToString() + " Past/Future year(s) can be allowed for this Combination");
                            txtPastYears.Focus();
                            return;
                        }

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Not yet set credit score guide for this combination');", true);
                    return;
                }
                uinfo = null;
                dictDropdownListParam = null;
                DS.Dispose();
                FunPriBindLookup();
                FunGetCreditScoreParameterDetails();
            }
            else if (txtEnquiryNo.Text != "")
            {
                btnAdd.Enabled = true;
                //btnAddtAdd.Enabled = true;
                btnSave.Enabled = false;
                if (ddlDocumentType.SelectedValue == "6")
                {
                    // if (ddlLob.SelectedIndex > 0 && ddlConstitution.SelectedIndex > 0)
                    if (Convert.ToInt32(ViewState["hdnLobid"].ToString()) > 0 && Convert.ToInt32(ViewState["hdnConstitutionid"].ToString()) > 0)
                    {
                        uinfo = new UserInfo();
                        dictDropdownListParam = new Dictionary<string, string>();
                        dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                        dictDropdownListParam.Add("@LOB_ID", ViewState["hdnLobid"].ToString());
                        dictDropdownListParam.Add("@Constitution_ID", ViewState["hdnConstitutionid"].ToString());

                        DS = Utility.GetDataset("S3G_ORG_GetEnquiryCustomAppraisalForCreditGuideTrans", dictDropdownListParam);
                        if (DS.Tables[4].Rows.Count > 0)
                        {
                            hdnCreditScoreID.Value = DS.Tables[4].Rows[0]["Crdtscoreguide_Id"].ToString();
                            intCreditScoreID = Convert.ToInt32(hdnCreditScoreID.Value);
                            intYear = Convert.ToInt32(DS.Tables[4].Rows[0]["Years"].ToString());
                            int intPast = 0;
                            int intFuture = 0;
                            int intPastStartFrom = 0;
                            int intAssignYear = 0;
                            int intStartYear = 0;

                            if (!string.IsNullOrEmpty(txtPastYears.Text.Trim()))
                                intPast = Convert.ToInt32(txtPastYears.Text);

                            if (!string.IsNullOrEmpty(txtFutureYears.Text.Trim()))
                                intFuture = Convert.ToInt32(txtFutureYears.Text);


                            if (!string.IsNullOrEmpty(txtPastYearStartFrom.Text.Trim()))
                                intPastStartFrom = Convert.ToInt32(txtPastYearStartFrom.Text.Trim());

                            else
                            {

                                intPastStartFrom = DateTime.Now.Year - intPast;

                            }
                            intAssignYear = intPast + intFuture;
                            if (intAssignYear <= intYear)
                            {

                                FunPriBindYear(intAssignYear, intPastStartFrom);
                                //                                lblmsg.Text = ddlYear.SelectedItem.Text;
                                lblAddtMsg.Text = ddlYear.SelectedItem.Text;
                            }
                            else
                            {
                                grvCreditScore.Columns.Clear();
                                grvCreditScore.Dispose();
                                if (intYear > 1)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert10", "alert('Maximum " + intYear.ToString() + " Past/Future year(s) can be allowed for this Combination');", true);
                                    return;
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert11", "alert('Maximum " + intYear.ToString() + " Past/Future year(s) can be allowed for this Combination');", true);
                                    return;
                                }

                            }
                        }
                        else
                        {
                            grvCreditScore.Columns.Clear();
                            grvCreditScore.Dispose();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Not yet set credit score guide for this combination');", true);
                            return;
                        }
                        uinfo = null;
                        dictDropdownListParam = null;
                        DS.Dispose();
                    }
                }
                else
                {
                    if (Convert.ToInt32(ViewState["hdnLobid"].ToString()) > 0 && Convert.ToInt32(ViewState["hdnConstitutionid"].ToString()) > 0 && Convert.ToInt32(ViewState["hdnproductid"].ToString()) > 0)
                    {
                        uinfo = new UserInfo();
                        dictDropdownListParam = new Dictionary<string, string>();
                        dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                        dictDropdownListParam.Add("@LOB_ID", ViewState["hdnLobid"].ToString());
                        dictDropdownListParam.Add("@Constitution_ID", ViewState["hdnConstitutionid"].ToString());
                        dictDropdownListParam.Add("@Product_ID", ViewState["hdnproductid"].ToString());
                        DS = Utility.GetDataset("S3G_ORG_GetEnquiryCustomAppraisalForCreditGuideTrans", dictDropdownListParam);
                        if (DS.Tables[4].Rows.Count > 0)
                        {
                            hdnCreditScoreID.Value = DS.Tables[4].Rows[0]["Crdtscoreguide_Id"].ToString();
                            intCreditScoreID = Convert.ToInt32(hdnCreditScoreID.Value);
                            intYear = Convert.ToInt32(DS.Tables[4].Rows[0]["Years"].ToString());
                            int intPast = 0;
                            int intFuture = 0;
                            int intPastStartFrom = 0;
                            int intAssignYear = 0;
                            int intStartYear = 0;

                            if (!string.IsNullOrEmpty(txtPastYears.Text.Trim()))
                                intPast = Convert.ToInt32(txtPastYears.Text);

                            if (!string.IsNullOrEmpty(txtFutureYears.Text.Trim()))
                                intFuture = Convert.ToInt32(txtFutureYears.Text);

                            if (!string.IsNullOrEmpty(txtPastYearStartFrom.Text.Trim()))
                                intPastStartFrom = Convert.ToInt32(txtPastYearStartFrom.Text.Trim());

                            else
                                intPastStartFrom = DateTime.Now.Year - (intPast + intFuture);

                            intAssignYear = intPast + intFuture;
                            if (intAssignYear <= intYear)
                            {

                                FunPriBindYear(intAssignYear, intPastStartFrom);
                                //                                lblmsg.Text = ddlYear.SelectedItem.Text;
                                lblAddtMsg.Text = ddlYear.SelectedItem.Text;
                            }
                            else
                            {
                                if (grvCreditScore.Rows.Count > 0)
                                {
                                    DataTable dtDelete = new DataTable();
                                    dtDelete = null;
                                    grvCreditScore.DataSource = dtDelete;
                                    grvCreditScore.DataBind();
                                    //                                    lblmsg.Text = "";
                                    lblAddtMsg.Text = "";

                                }
                                if (intYear > 1)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert10", "alert('Maximum " + intYear.ToString() + " Past/Future year(s) can be allowed for this Combination');", true);
                                    return;
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert10", "alert('Maximum " + intYear.ToString() + " Past/Future year(s) can be allowed for this Combination');", true);
                                    return;
                                }

                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Not yet set credit score guide for this combination');", true);
                            return;
                        }
                        uinfo = null;
                        dictDropdownListParam = null;
                        DS.Dispose();

                    }

                }

                if (intCreditScoreID > 0)
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    btnClear.Enabled = false;
                    hdnCreditScoreID.Value = intCreditScoreID.ToString();  //need to be change
                }
                else
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                }
                FunPriBindLookup();

                FunGetCreditScoreParameterDetails();

            }
            else
            {
                Utility.FunShowAlertMsg(this, "Please select Document No/Customer Code");
                return;
            }
            if (grvCreditScore.Rows.Count > 0)
            {
                lblActualtotalscore.Visible = true;
                txtActualTotal.Visible = true;
            }
            else
            {
                lblActualtotalscore.Visible = false;
                txtActualTotal.Visible = false;
            }
            txtPastYears.Enabled = false;
            txtFutureYears.Enabled = false;

        }
        catch (Exception exx)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(exx);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DataTable dttemp = new DataTable();
        if (ViewState["YearvaluesDetails"] == null)
        {

            dtCreditScore.Columns.Add("Description");
            dtCreditScore.Columns.Add("Actual_Value");
            dtCreditScore.Columns.Add("Actual_Score");
            dtCreditScore.Columns.Add("Year");
            dtCreditScore.Columns.Add("YearCount");
            dtCreditScore.Columns.Add("CrdtGuideTrans_Year_Values_ID");
            dtCreditScore.Columns.Add("Crdtscoreguideparam_Id");

        }
        else
        {
            dtCreditScore = (DataTable)ViewState["YearvaluesDetails"];
        }

        //added 3/12/2010
        DataTable dttemp2 = new DataTable();
        DataRow DataRows = null;
        DataColumn desc = new DataColumn("Description", typeof(System.String));
        dttemp.Columns.Add(desc);
        dttemp.PrimaryKey = new DataColumn[] { desc };
        dttemp.Columns.Add("Actual Value -" + ddlYear.SelectedItem.Text);
        dttemp.Columns.Add("Actual Score -" + ddlYear.SelectedItem.Text);
        //added 3/12/2010 end
        foreach (GridViewRow gr in grvCreditScore.Rows)
        {
            DataRow Dr = dtCreditScore.NewRow();
            Label lblDesc = (Label)gr.FindControl("lblDesc");
            DropDownList ddlYes = (DropDownList)gr.FindControl("ddlYes");
            TextBox txtActualValue1 = (TextBox)gr.FindControl("txtActualValue1");
            TextBox txtActualScore1 = (TextBox)gr.FindControl("txtActualScore1");
            Label lblCrScoreParam_ID = (Label)gr.FindControl("lblCrScoreParam_ID");
            Label lblYearvaluesID = (Label)gr.FindControl("lblYearvaluesID");
            if (txtActualValue1.Visible == true)
            {
                //if (txtActualValue1.Text.Trim() == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Actual Value should not be empty');", true);
                //    txtActualValue1.Focus();
                //    return;
                //}
                Dr["Description"] = lblDesc.Text;
                Dr["Actual_Value"] = txtActualValue1.Text.Trim();
                Dr["CrdtGuideTrans_Year_Values_ID"] = lblYearvaluesID.Text;
                Dr["Actual_Score"] = txtActualScore1.Text.Trim();
                Dr["Crdtscoreguideparam_Id"] = lblCrScoreParam_ID.Text;
                Dr["Year"] = ddlYear.SelectedItem.Text;
                Dr["YearCount"] = ddlYear.SelectedValue;

                dtCreditScore.Rows.Add(Dr);



                // added 3/12/2010  
                DataRows = dttemp.NewRow();
                DataRows["Description"] = lblDesc.Text;
                DataRows["Actual Value -" + ddlYear.SelectedItem.Text] = txtActualValue1.Text;
                DataRows["Actual Score -" + ddlYear.SelectedItem.Text] = txtActualScore1.Text;
                dttemp.Rows.Add(DataRows);

                dttemp2 = (DataTable)ViewState["dttemp"];
                if (dttemp2 != null && dttemp2.Rows.Count > 0)
                {
                    dttemp2.Merge(dttemp, false, MissingSchemaAction.Add);

                }
                // 3/12/2010 end

            }
            else if (ddlYes.Visible == true)
            {
                //if (ddlYes.SelectedIndex == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert2('Select Actual Value');", true);
                //    ddlYes.Focus();
                //    return;
                //}
                Dr["Description"] = lblDesc.Text;
                if (ddlYes.SelectedIndex == 0)
                    Dr["Actual_Value"] = "";
                else
                    Dr["Actual_Value"] = ddlYes.SelectedValue;

                Dr["CrdtGuideTrans_Year_Values_ID"] = lblYearvaluesID.Text;
                Dr["Actual_Score"] = txtActualScore1.Text;
                Dr["Crdtscoreguideparam_Id"] = lblCrScoreParam_ID.Text;
                Dr["Year"] = ddlYear.SelectedItem.Text;
                Dr["YearCount"] = ddlYear.SelectedValue;
                dtCreditScore.Rows.Add(Dr);

                if (ddlYes.SelectedItem.Text.Trim() == "Select")
                    ddlYes.SelectedItem.Text = "";
                // added 3/12/2010  
                DataRows = dttemp.NewRow();
                DataRows["Description"] = lblDesc.Text;
                DataRows["Actual Value -" + ddlYear.SelectedItem.Text] = ddlYes.SelectedItem.Text;
                DataRows["Actual Score -" + ddlYear.SelectedItem.Text] = txtActualScore1.Text;
                dttemp.Rows.Add(DataRows);

                dttemp2 = (DataTable)ViewState["dttemp"];
                if (dttemp2 != null && dttemp2.Rows.Count > 0)
                {
                    dttemp2.Merge(dttemp, false, MissingSchemaAction.Add);

                }
            }

            ViewState["YearvaluesDetails"] = dtCreditScore;
        }
        //-----------------3/12/2010  ramesh
        if (dtCreditScore.Rows.Count > 0)
        {
            if (ViewState["modifydtTemp"] != null)
            {
                DataTable dtTempmodify = (DataTable)ViewState["modifydtTemp"];
                grvCreditScoreYearValues.DataSource = dtTempmodify;
                grvCreditScoreYearValues.DataBind();
            }
            else
            {
                if (dttemp2 != null && dttemp2.Rows.Count > 0)
                {
                    grvCreditScoreYearValues.DataSource = dttemp2;
                    grvCreditScoreYearValues.DataBind();
                    ViewState["dttemp"] = dttemp2;
                }
                else
                {
                    grvCreditScoreYearValues.DataSource = dttemp;
                    grvCreditScoreYearValues.DataBind();
                    ViewState["dttemp"] = dttemp;
                }
            }
        }


        //-------------end
        if (intCreditScoreTran_ID > 0 && hdnEdit_Status.Value == "0")
            btnSave.Enabled = false;
        if (ddlYear.Items.Count == ddlYear.SelectedIndex + 1)
        {
            if (intCreditScoreID > 0 && intCreditScoreTran_ID > 0 && Request.QueryString.Get("qsMode").ToString() != "Q" && hdnEdit_Status.Value == "1")
            {

                //                lblmsg.Text = "All year(s) modified successfully. Please click Save";
                btnSave.Enabled = true;
                grvCreditScore.Enabled = false;
                grvCreditScore.Visible = true;

            }
            else
            {
                if (Request.QueryString.Get("qsMode").ToString() != "Q" && hdnEdit_Status.Value == "0" && intCreditScoreTran_ID == 0)
                {
                    //                    lblmsg.Text = "All year(s) added successfully. Please click Save";
                    btnSave.Enabled = true;
                    grvCreditScore.Enabled = false;
                    btnGo.Enabled = false;
                }
            }
            btnAdd.Enabled = false;

        }
        else
        {
            ddlYear.SelectedIndex = ddlYear.SelectedIndex + 1;
            bYearBind = false;
            //            lblmsg.Text = ddlYear.SelectedItem.Text;
            FunGetCreditScoreParameterDetails();
        }
    }
    /*
        protected void btnAdditionalAdd_Click(object sender, EventArgs e)
        {

            DataTable dttemp = new DataTable();
            if (ViewState["AddtYearvaluesDetails"] == null)
            {

                dtCreditScore.Columns.Add("Description");
                dtCreditScore.Columns.Add("Actual_Value");
                dtCreditScore.Columns.Add("Actual_Score");
                dtCreditScore.Columns.Add("Year");
                dtCreditScore.Columns.Add("YearCount");
                dtCreditScore.Columns.Add("CrdtGuideTrans_Year_Values_ID");
                dtCreditScore.Columns.Add("Crdtscoreguideparam_Id");

            }
            else
            {
                dtCreditScore = (DataTable)ViewState["AddtYearvaluesDetails"];
            }

            //added 3/12/2010
            DataTable dttemp2 = new DataTable();
            DataRow DataRows = null;
            DataColumn desc = new DataColumn("Description", typeof(System.String));
            dttemp.Columns.Add(desc);
            dttemp.PrimaryKey = new DataColumn[] { desc };
            dttemp.Columns.Add("Actual Value -" + ddlYear.SelectedItem.Text);
            //added 3/12/2010 end
            foreach (GridViewRow gr in grvAdditional.Rows)
            {
                DataRow Dr = dtCreditScore.NewRow();
                Label lblDesc = (Label)gr.FindControl("lblAddtDesc");
                TextBox txtActualValue1 = (TextBox)gr.FindControl("txtAddtActualValue");
                Label lblCrScoreParam_ID = (Label)gr.FindControl("lblAddtCrScoreParam_ID");
                Label lblYearvaluesID = (Label)gr.FindControl("lblAddtYearvaluesID");
                if (txtActualValue1.Visible == true)
                {

                    if (string.IsNullOrEmpty(txtActualValue1.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Enter " + lblDesc.Text);
                        return;
                    }
                    Dr["Description"] = lblDesc.Text;
                    Dr["Actual_Value"] = txtActualValue1.Text.Trim();
                    Dr["CrdtGuideTrans_Year_Values_ID"] = lblYearvaluesID.Text;
                    Dr["Crdtscoreguideparam_Id"] = lblCrScoreParam_ID.Text;
                    Dr["Year"] = ddlYear.SelectedItem.Text;
                    Dr["YearCount"] = ddlYear.SelectedValue;

                    dtCreditScore.Rows.Add(Dr);

                    // added 3/12/2010  
                    DataRows = dttemp.NewRow();
                    DataRows["Description"] = lblDesc.Text;
                    DataRows["Actual Value -" + ddlYear.SelectedItem.Text] = txtActualValue1.Text;
                    dttemp.Rows.Add(DataRows);

                    dttemp2 = (DataTable)ViewState["dttemp"];
                    if (dttemp2 != null && dttemp2.Rows.Count > 0)
                    {
                        dttemp2.Merge(dttemp, false, MissingSchemaAction.Add);

                    }
                    // 3/12/2010 end

                }

                ViewState["AddtYearvaluesDetails"] = dtCreditScore;
            }

            if (ViewState["YearvaluesDetails"] != null)
            {
                DataTable dtTempYearVal = new DataTable();
                dtTempYearVal = (DataTable)ViewState["YearvaluesDetails"];

                dtTempYearVal.Merge(dtCreditScore, false, MissingSchemaAction.Add);

            }

            //-----------------3/12/2010  ramesh
            if (dtCreditScore.Rows.Count > 0)
            {
                if (ViewState["modifydtAddtTemp"] != null)
                {
                    DataTable dtTempmodify = (DataTable)ViewState["modifydtAddtTemp"];
                    gvrAddtionalYearValue.DataSource = dtTempmodify;
                    gvrAddtionalYearValue.DataBind();
                }
                else
                {
                    if (dttemp2 != null && dttemp2.Rows.Count > 0)
                    {
                        gvrAddtionalYearValue.DataSource = dttemp;
                        gvrAddtionalYearValue.DataBind();
                        ViewState["dttemp"] = dttemp;
                    }
                    else
                    {
                        gvrAddtionalYearValue.DataSource = dttemp;
                        gvrAddtionalYearValue.DataBind();
                        ViewState["dttemp"] = dttemp;
                    }
                }

            }


            //-------------end
            if (intCreditScoreTran_ID > 0 && hdnEdit_Status.Value == "0")
                btnSave.Enabled = false;
            if (ddlYear.Items.Count == ddlYear.SelectedIndex + 1)
            {
                if (intCreditScoreID > 0 && intCreditScoreTran_ID > 0 && Request.QueryString.Get("qsMode").ToString() != "Q" && hdnEdit_Status.Value == "1")
                {

                    lblAddtMsg.Text = "All year(s) modified successfully. Please click Save";
                    btnSave.Enabled = true;
                    grvAdditional.Enabled = false;
                    grvAdditional.Visible = true;

                }
                else
                {
                    if (Request.QueryString.Get("qsMode").ToString() != "Q" && hdnEdit_Status.Value == "0" && intCreditScoreTran_ID == 0)
                    {
                        lblAddtMsg.Text = "All year(s) added successfully. Please click Save";
                        grvAdditional.Enabled = false;
                    }
                }
                btnAddtAdd.Enabled = false;

            }
            else
            {
                ddlYear.SelectedIndex = ddlYear.SelectedIndex + 1;
                bYearBind = false;
                lblAddtMsg.Text = ddlYear.SelectedItem.Text;
                //FunGetCreditScoreParameterDetails();
            }
        }
        */
    protected void ddlFieldAtt_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((DropDownList)sender).ClientID;
        string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvCreditScore_")).Replace("grvCreditScore_ctl", "");
        int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
        gRowIndex = gRowIndex - 2;

        DropDownList ddlNumericF = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlNumeric");
        TextBox txtDiffPerF = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtDiffPer");
        TextBox txtDiffMarkF = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtDiffMark");

        if (((DropDownList)sender).SelectedValue == "5")
        {
            txtDiffPerF.Text = "";
            txtDiffMarkF.Text = "";
            txtDiffPerF.Enabled = false;
            txtDiffMarkF.Enabled = false;
            ddlNumericF.SelectedIndex = 0;
            ddlNumericF.Enabled = false;
            grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1").Visible = true;
            grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1").Visible = false;
        }
        else if (((DropDownList)sender).SelectedValue == "4")
        {
            txtDiffPerF.Text = "";
            txtDiffMarkF.Text = "";
            txtDiffPerF.Enabled = false;
            txtDiffMarkF.Enabled = false;
            ddlNumericF.SelectedIndex = 0;
            ddlNumericF.Enabled = false;
            grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1").Visible = false;
            grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1").Visible = true;
        }
        else
        {
            txtDiffPerF.Enabled = true;
            txtDiffMarkF.Enabled = true;
            ddlNumericF.Enabled = true;
            grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1").Visible = true;
            grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1").Visible = false;
        }

    }
    protected void ddlFieldAttF_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlNumericF = (DropDownList)grvCreditScore.FooterRow.FindControl("ddlNumericF");
        TextBox txtDiffPerF = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffPerF");
        TextBox txtDiffMarkF = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffMarkF");

        if (((DropDownList)sender).SelectedValue == "5")
        {
            txtDiffPerF.Text = "";
            txtDiffMarkF.Text = "";
            txtDiffPerF.Enabled = false;
            txtDiffMarkF.Enabled = false;
            ddlNumericF.SelectedIndex = 0;
            ddlNumericF.Enabled = false;
            grvCreditScore.FooterRow.FindControl("txtReqValue1F").Visible = true;
            grvCreditScore.FooterRow.FindControl("ddlYes1F").Visible = false;
        }
        else if (((DropDownList)sender).SelectedValue == "4")
        {
            txtDiffPerF.Text = "";
            txtDiffMarkF.Text = "";
            txtDiffPerF.Enabled = false;
            txtDiffMarkF.Enabled = false;
            ddlNumericF.SelectedIndex = 0;
            ddlNumericF.Enabled = false;
            grvCreditScore.FooterRow.FindControl("txtReqValue1F").Visible = false;
            grvCreditScore.FooterRow.FindControl("ddlYes1F").Visible = true;
        }
        else
        {
            txtDiffPerF.Enabled = true;
            txtDiffMarkF.Enabled = true;
            ddlNumericF.Enabled = true;
            grvCreditScore.FooterRow.FindControl("txtReqValue1F").Visible = true;
            grvCreditScore.FooterRow.FindControl("ddlYes1F").Visible = false;
        }

    }
    protected void Year_SelectedIndexChanged(object sender, EventArgs e)
    {
        bYearBind = false;
        FunGetCreditScoreParameterDetails();
    }
    private void FunPriBindLookup()
    {
        dictLOB = new Dictionary<string, string>();
        dtDefault = Utility.GetDefaultData("S3G_ORG_GetCreditScoreLookup", dictLOB);


    }
    private int FunPriTypeCast(string val)
    {

        int outInt = 0;
        decimal outDec = 0.0M;
        DateTime outDatetime;
        if (val.Contains(":"))
            return 2;
        if (Int32.TryParse(val, out outInt))
            return 2;
        if (Decimal.TryParse(val, out outDec))
            return 3;
        if (val is string)
            return 4;
        if (DateTime.TryParse(val, out outDatetime))
            return 1;

        return 0;
    }
    protected void grvCreditScoreYearValues_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal priceTotal = 0;
        int quantityTotal = 0;
        int intCells = 2;
        string strParameterValue = "";
        string strParameterName = "";
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
            {
                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("bActual", "Actual");
                e.Row.Cells[i].CssClass = "style = text-align:right;";
            }

        }

        if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
        {
            for (int i_cellVal = 0; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                try
                {
                    Int32 type = 0;       // 1 = int, 2 = datetime, 3 = string

                    type = FunPriTypeCast(e.Row.Cells[i_cellVal].Text);

                    // cell alignment
                    switch (type)
                    {
                        case 1:  // datetime - trim to code standard
                            e.Row.Cells[i_cellVal].Text = DateTime.Parse(e.Row.Cells[i_cellVal].Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                            e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Left;
                            break;

                        case 2:  // int - right to left
                            e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Right;
                            break;
                        case 3:  // decimal - do nothing - left align(default)
                            e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Right;
                            break;
                        case 4:  // string - do nothing - left align(default)
                            e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Left;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //continue;
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            if (grvCreditScoreYearValues.HeaderRow.Cells.Count >= 2)
            {
                for (int i = 0; i <= grvCreditScoreYearValues.Rows.Count - 1; i++)
                {
                    if (grvCreditScoreYearValues.Rows[i].Cells[2].Text.Trim() != "&nbsp;")
                    {
                        priceTotal += Convert.ToDecimal(grvCreditScoreYearValues.Rows[i].Cells[2].Text);
                        e.Row.Cells[2].Text = priceTotal.ToString();
                        e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[0].CssClass = "style = text-align:Left;";
                        strParameterValue = grvCreditScoreYearValues.Rows[i].Cells[1].Text;
                        strParameterName = grvCreditScoreYearValues.Rows[i].Cells[0].Text;
                        if (strParameterValue == "1" || strParameterValue == "0")
                        {
                            foreach (GridViewRow gr in grvCreditScore.Rows)
                            {
                                Label lblDesc = (Label)gr.FindControl("lblDesc");
                                if (lblDesc.Text.Trim() == strParameterName.Trim())
                                {
                                    DropDownList dropdown = (DropDownList)gr.FindControl("ddlYes");
                                    if (dropdown.Visible == true)
                                    {
                                        if (strParameterValue.Trim() == "1")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[1].Text = "Yes";
                                            grvCreditScoreYearValues.Rows[i].Cells[1].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        if (strParameterValue.Trim() == "0")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[1].Text = "No";
                                            grvCreditScoreYearValues.Rows[i].Cells[1].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (grvCreditScoreYearValues.HeaderRow.Cells.Count >= 4)
            {
                priceTotal = 0;
                for (int i = 0; i <= grvCreditScoreYearValues.Rows.Count - 1; i++)
                {
                    if (grvCreditScoreYearValues.Rows[i].Cells[4].Text.Trim() != "&nbsp;")
                    {
                        priceTotal += Convert.ToDecimal(grvCreditScoreYearValues.Rows[i].Cells[4].Text);
                        e.Row.Cells[4].Text = priceTotal.ToString();
                        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                        strParameterValue = grvCreditScoreYearValues.Rows[i].Cells[3].Text;
                        strParameterName = grvCreditScoreYearValues.Rows[i].Cells[0].Text;
                        if (strParameterValue == "1" || strParameterValue == "0")
                        {
                            foreach (GridViewRow gr in grvCreditScore.Rows)
                            {
                                Label lblDesc = (Label)gr.FindControl("lblDesc");
                                if (lblDesc.Text.Trim() == strParameterName.Trim())
                                {
                                    DropDownList dropdown = (DropDownList)gr.FindControl("ddlYes");
                                    if (dropdown.Visible == true)
                                    {
                                        if (strParameterValue.Trim() == "1")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[3].Text = "Yes";
                                            grvCreditScoreYearValues.Rows[i].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        if (strParameterValue.Trim() == "0")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[3].Text = "No";
                                            grvCreditScoreYearValues.Rows[i].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (grvCreditScoreYearValues.HeaderRow.Cells.Count >= 6)
            {
                priceTotal = 0;
                for (int i = 0; i <= grvCreditScoreYearValues.Rows.Count - 1; i++)
                {
                    if (grvCreditScoreYearValues.Rows[i].Cells[6].Text.Trim() != "&nbsp;")
                    {
                        priceTotal += Convert.ToDecimal(grvCreditScoreYearValues.Rows[i].Cells[6].Text);
                        e.Row.Cells[6].Text = priceTotal.ToString();
                        e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                        strParameterValue = grvCreditScoreYearValues.Rows[i].Cells[5].Text;
                        strParameterName = grvCreditScoreYearValues.Rows[i].Cells[0].Text;
                        if (strParameterValue == "1" || strParameterValue == "0")
                        {
                            foreach (GridViewRow gr in grvCreditScore.Rows)
                            {
                                Label lblDesc = (Label)gr.FindControl("lblDesc");
                                if (lblDesc.Text.Trim() == strParameterName.Trim())
                                {
                                    DropDownList dropdown = (DropDownList)gr.FindControl("ddlYes");
                                    if (dropdown.Visible == true)
                                    {
                                        if (strParameterValue.Trim() == "1")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[5].Text = "Yes";
                                            grvCreditScoreYearValues.Rows[i].Cells[5].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        if (strParameterValue.Trim() == "0")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[5].Text = "No";
                                            grvCreditScoreYearValues.Rows[i].Cells[5].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (grvCreditScoreYearValues.HeaderRow.Cells.Count >= 8)
            {
                priceTotal = 0;
                for (int i = 0; i <= grvCreditScoreYearValues.Rows.Count - 1; i++)
                {
                    if (grvCreditScoreYearValues.Rows[i].Cells[8].Text.Trim() != "&nbsp;")
                    {
                        priceTotal += Convert.ToDecimal(grvCreditScoreYearValues.Rows[i].Cells[8].Text);
                        e.Row.Cells[8].Text = priceTotal.ToString();
                        e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                        strParameterValue = grvCreditScoreYearValues.Rows[i].Cells[7].Text;
                        strParameterName = grvCreditScoreYearValues.Rows[i].Cells[0].Text;
                        if (strParameterValue == "1" || strParameterValue == "0")
                        {
                            foreach (GridViewRow gr in grvCreditScore.Rows)
                            {
                                Label lblDesc = (Label)gr.FindControl("lblDesc");
                                if (lblDesc.Text.Trim() == strParameterName.Trim())
                                {
                                    DropDownList dropdown = (DropDownList)gr.FindControl("ddlYes");
                                    if (dropdown.Visible == true)
                                    {
                                        if (strParameterValue.Trim() == "1")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[7].Text = "Yes";
                                            grvCreditScoreYearValues.Rows[i].Cells[7].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        if (strParameterValue.Trim() == "0")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[7].Text = "No";
                                            grvCreditScoreYearValues.Rows[i].Cells[7].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (grvCreditScoreYearValues.HeaderRow.Cells.Count >= 10)
            {
                priceTotal = 0;
                for (int i = 0; i <= grvCreditScoreYearValues.Rows.Count - 1; i++)
                {
                    if (grvCreditScoreYearValues.Rows[i].Cells[10].Text.Trim() != "&nbsp;")
                    {
                        priceTotal += Convert.ToDecimal(grvCreditScoreYearValues.Rows[i].Cells[10].Text);
                        e.Row.Cells[10].Text = priceTotal.ToString();
                        e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                        strParameterValue = grvCreditScoreYearValues.Rows[i].Cells[9].Text;
                        strParameterName = grvCreditScoreYearValues.Rows[i].Cells[0].Text;
                        if (strParameterValue == "1" || strParameterValue == "0")
                        {
                            foreach (GridViewRow gr in grvCreditScore.Rows)
                            {
                                Label lblDesc = (Label)gr.FindControl("lblDesc");
                                if (lblDesc.Text.Trim() == strParameterName.Trim())
                                {
                                    DropDownList dropdown = (DropDownList)gr.FindControl("ddlYes");
                                    if (dropdown.Visible == true)
                                    {
                                        if (strParameterValue.Trim() == "1")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[9].Text = "Yes";
                                            grvCreditScoreYearValues.Rows[i].Cells[9].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        if (strParameterValue.Trim() == "0")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[9].Text = "No";
                                            grvCreditScoreYearValues.Rows[i].Cells[9].HorizontalAlign = HorizontalAlign.Left;

                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (grvCreditScoreYearValues.HeaderRow.Cells.Count >= 12)
            {
                priceTotal = 0;
                for (int i = 0; i <= grvCreditScoreYearValues.Rows.Count - 1; i++)
                {
                    if (grvCreditScoreYearValues.Rows[i].Cells[12].Text.Trim() != "&nbsp;")
                    {
                        priceTotal += Convert.ToDecimal(grvCreditScoreYearValues.Rows[i].Cells[12].Text);
                        e.Row.Cells[12].Text = priceTotal.ToString();
                        e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                        strParameterValue = grvCreditScoreYearValues.Rows[i].Cells[11].Text;
                        strParameterName = grvCreditScoreYearValues.Rows[i].Cells[0].Text;
                        if (strParameterValue == "1" || strParameterValue == "0")
                        {
                            foreach (GridViewRow gr in grvCreditScore.Rows)
                            {
                                Label lblDesc = (Label)gr.FindControl("lblDesc");
                                if (lblDesc.Text.Trim() == strParameterName.Trim())
                                {
                                    DropDownList dropdown = (DropDownList)gr.FindControl("ddlYes");
                                    if (dropdown.Visible == true)
                                    {
                                        if (strParameterValue.Trim() == "1")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[11].Text = "Yes";
                                            grvCreditScoreYearValues.Rows[i].Cells[11].HorizontalAlign = HorizontalAlign.Left;

                                        }
                                        if (strParameterValue.Trim() == "0")
                                        {
                                            grvCreditScoreYearValues.Rows[i].Cells[11].Text = "No";
                                            grvCreditScoreYearValues.Rows[i].Cells[11].HorizontalAlign = HorizontalAlign.Left;

                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }



            e.Row.Cells[0].Text = "Totals:";

            //e.Row.Cells[intCells].Text = quantityTotal.ToString("d");
            //e.Row.Cells[1].HorizontalAlign = _e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            // e.Row.Font.Bold = true;
        }


    }
    protected void grvCreditScore_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            DataTable dtDefaultNew;
            string strReqValue = string.Empty;
            DataView dvSearchView;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Request.QueryString.Get("qsMode").ToString() == "Q")
                {
                    TextBox txtReqVal3 = ((TextBox)e.Row.FindControl("txtReqValue1"));
                    TextBox txtActualValue3 = ((TextBox)e.Row.FindControl("txtActualValue1"));
                    DropDownList ddlYes = (DropDownList)e.Row.FindControl("ddlYes");

                    txtReqVal3.ReadOnly = true;
                    txtActualValue3.ReadOnly = true;
                    if (ddlYes.Visible == true)
                        ddlYes.Enabled = false;
                }

                dtCreditScore = (DataTable)ViewState["CreditScoreYr"];
                if ((dtCreditScore != null) && (dtCreditScore.Rows[iCount]["FieldAtt"].ToString() != ""))
                {

                    DropDownList ddlField = (DropDownList)e.Row.FindControl("ddlFieldAtt");
                    DropDownList ddlNumeric = (DropDownList)e.Row.FindControl("ddlNumeric");
                    DropDownList ddlYes1 = (DropDownList)e.Row.FindControl("ddlYes1");
                    DropDownList ddlYes = (DropDownList)e.Row.FindControl("ddlYes");
                    ddlYes1.Visible = false;
                    ddlYes.Visible = false;

                    TextBox txtReqVal = ((TextBox)e.Row.FindControl("txtReqValue1"));
                    TextBox txtActualValue1 = ((TextBox)e.Row.FindControl("txtActualValue1"));
                    TextBox txtActualScore = (TextBox)e.Row.FindControl("txtActualScore1");
                    TextBox txtDiffPer = ((TextBox)e.Row.FindControl("txtDiffPer"));
                    TextBox txtDiffMark = ((TextBox)e.Row.FindControl("txtDiffMark"));

                    RangeValidator RangePercentage = ((RangeValidator)e.Row.FindControl("RangePercentage"));//range validator
                    RequiredFieldValidator RequiredvalidatorTextbox = ((RequiredFieldValidator)e.Row.FindControl("RequiredvalidatorTextbox"));//range validator
                    RequiredFieldValidator RequiredvalidatorCombo = ((RequiredFieldValidator)e.Row.FindControl("RequiredvalidatorCombo"));//range validator
                    RangePercentage.Enabled = false;
                    RequiredvalidatorTextbox.Enabled = false;
                    RequiredvalidatorCombo.Enabled = false;

                    txtActualValue1.Visible = true;
                    // txtReqVal.Enabled = false;
                    txtActualValue1.Attributes.Add("maxlength", "29");

                    dvSearchView = new DataView(dtDefault);
                    dvSearchView.RowFilter = "[Type] LIKE 'ORG_FieldAttribute'";
                    dtDefaultNew = dvSearchView.ToTable();
                    ddlField.FillDataTable(dtDefaultNew, "Value", "Name");
                    dvSearchView.Dispose();
                    ddlField.SelectedValue = dtCreditScore.Rows[iCount]["FieldAtt"].ToString();


                    dvSearchView = new DataView(dtDefault); ;
                    dvSearchView.RowFilter = "[Type] LIKE 'ORG_NumericOperator'";
                    dtDefaultNew = dvSearchView.ToTable();
                    ddlNumeric.FillDataTable(dtDefaultNew, "Value", "Name");
                    dvSearchView.Dispose();
                    ddlNumeric.SelectedValue = dtCreditScore.Rows[iCount]["NumericAtt"].ToString();
                    AjaxControlToolkit.CalendarExtender ceReqVal1 = e.Row.FindControl("CalendarExtender1") as AjaxControlToolkit.CalendarExtender;
                    AjaxControlToolkit.FilteredTextBoxExtender fteAmount = e.Row.FindControl("fteAmount1") as AjaxControlToolkit.FilteredTextBoxExtender;
                    AjaxControlToolkit.FilteredTextBoxExtender fteAmount2 = e.Row.FindControl("fteAmount2") as AjaxControlToolkit.FilteredTextBoxExtender;

                    if (Convert.ToInt32(hdnCreditScoreID.Value) > 0 && Convert.ToInt32(hdnCreditScoreTran_ID.Value) > 0)
                    {

                        //if (DS.Tables[0].Rows.Count > 0 && DS.Tables[1].Rows.Count > 0)
                        //{
                        //    Label lblYearvaluesID = (Label)e.Row.FindControl("lblYearvaluesID");
                        //    txtActualValue1.Text = DS.Tables[1].Rows[intRow]["Actual_Value"].ToString();
                        //    txtActualScore.Text = DS.Tables[1].Rows[intRow]["Actual_Score"].ToString();
                        //    lblYearvaluesID.Text = DS.Tables[1].Rows[intRow]["CRDTGUIDETRANSYEARVAL_ID"].ToString();
                        //    ddlYes.SelectedValue = DS.Tables[1].Rows[intRow]["Actual_Value"].ToString();

                        //    intRow++;
                        //}
                    }
                    if (ddlField.SelectedValue == "5")//date attributes
                    {
                        Label lbldesc = (Label)e.Row.FindControl("lblDesc");
                        txtDiffPer.Enabled = false;
                        txtDiffMark.Enabled = false;
                        txtReqVal.Visible = true;
                        if (string.IsNullOrEmpty(txtReqVal.Text.Trim()))
                        {
                            txtActualValue1.Enabled = false;
                            txtActualValue1.BackColor = System.Drawing.Color.LightGray;
                            txtActualScore.Enabled = false;
                            RequiredvalidatorTextbox.Enabled = false;
                        }
                        else
                        {
                            txtActualValue1.Attributes.Add("readonly", "readonly");
                            ddlYes.Visible = false;
                            RequiredvalidatorTextbox.Enabled = true;
                            RequiredvalidatorTextbox.ErrorMessage = "Enter Actual value of " + lbldesc.Text;
                            ceReqVal1.Enabled = true;
                            fteAmount.Enabled = false;
                            fteAmount2.Enabled = false;
                            ceReqVal1.Format = strDateFormat;
                            //txtReqVal.Attributes.Add("readonly", "readonly");
                            txtReqVal.Text = dtCreditScore.Rows[iCount]["ReqValue"].ToString() != "" ? DateTime.Parse(dtCreditScore.Rows[iCount]["ReqValue"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat) : "";
                            // MEE_txtPer.Enabled = false;
                        }
                    }
                    else if (ddlField.SelectedValue == "4") //Text Yes or No
                    {
                        Label lbldesc = (Label)e.Row.FindControl("lblDesc");
                        RequiredvalidatorCombo.Enabled = true;
                        RequiredvalidatorCombo.ErrorMessage = "Select Actual value of " + lbldesc.Text;
                        txtDiffPer.Text = "";
                        txtDiffMark.Text = "";
                        txtDiffPer.Enabled = false;
                        txtDiffMark.Enabled = false;
                        ddlNumeric.SelectedIndex = 0;
                        ddlNumeric.Enabled = false;
                        txtReqVal.Visible = false;
                        ddlYes.Visible = true;
                        ddlYes.SelectedValue = dtCreditScore.Rows[e.Row.RowIndex]["Actual_Value"].ToString();
                        if (hdnEdit_Status.Value.Trim() == "0" && intCreditScoreTran_ID > 0)
                        {
                            ddlYes.Enabled = false;
                        }
                        ddlYes1.Visible = true;
                        ddlYes1.SelectedValue = dtCreditScore.Rows[iCount]["ReqValue"].ToString();

                        if (ddlYes1.SelectedValue == "-1")
                        {
                            ddlYes.Visible = true;
                            txtActualValue1.Enabled = false;
                            txtActualValue1.Visible = false;
                            ddlYes.Enabled = false;
                            ddlYes.BackColor = System.Drawing.Color.LightGray;
                            ddlYes1.Visible = true;
                            txtReqVal.Visible = false;

                            txtActualScore.Enabled = false;

                            RequiredvalidatorCombo.Enabled = false;
                        }

                        txtActualValue1.Visible = false;
                        //Calendar
                        fteAmount.Enabled = false;
                        fteAmount2.Enabled = false;
                        ceReqVal1.Enabled = false;
                        //MEE_txtPer.Enabled = false;
                    }
                    else if (ddlField.SelectedValue == "2")//percentage
                    {
                        Label lbldesc = (Label)e.Row.FindControl("lblDesc");
                        RequiredvalidatorTextbox.Enabled = true;
                        RequiredvalidatorTextbox.ErrorMessage = "Enter Actual value of " + lbldesc.Text;

                        //txtActualValue1.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this)");
                        //txtActualValue1.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this);");
                        //txtActualValue1.SetDecimalPrefixSuffix(2, 2, true, "Percentage");
                        if (string.IsNullOrEmpty(txtReqVal.Text.Trim()))
                        {
                            txtActualValue1.Enabled = false;
                            txtActualValue1.BackColor = System.Drawing.Color.LightGray;
                            txtActualScore.Enabled = false;
                            RequiredvalidatorTextbox.Enabled = false;
                            RangePercentage.Enabled = false;
                        }
                        else
                        {
                            txtActualValue1.Attributes.Add("maxlength", "5");//onkeypress="fnAllowNumbersOnly(false,true,this)"

                            // MEE_txtPer.Enabled = true;
                            fteAmount2.Enabled = true;
                            fteAmount.Enabled = false;
                            RangePercentage.Enabled = true;
                            RequiredvalidatorTextbox.Enabled = true;
                            RangePercentage.Enabled = true;
                        }
                    }
                    else if (ddlField.SelectedValue == "3")//ratio
                    {
                        Label lbldesc = (Label)e.Row.FindControl("lblDesc");
                        RequiredvalidatorTextbox.Enabled = true;
                        RequiredvalidatorTextbox.ErrorMessage = "Enter Actual value of " + lbldesc.Text;
                        if (string.IsNullOrEmpty(txtReqVal.Text.Trim()))
                        {
                            txtActualValue1.Enabled = false;
                            txtActualValue1.BackColor = System.Drawing.Color.LightGray;
                            txtActualScore.Enabled = false;
                            RequiredvalidatorTextbox.Enabled = false;
                        }
                        else
                        {
                            txtActualValue1.Enabled = true;
                            txtActualValue1.Visible = true;
                            fteAmount2.Enabled = true;
                            fteAmount.Enabled = false;
                            //MEE_txtPer.Enabled = false;
                            txtActualValue1.Attributes.Add("maxlength", "7");

                            //txtActualValue1.Attributes.Add("onkeypress", "fnAllowRatioOnly(true,false,this)"); //allow ratio format only

                            RequiredvalidatorTextbox.Enabled = true;
                        }
                    }
                    else
                    {

                        if (string.IsNullOrEmpty(txtReqVal.Text.Trim()))
                        {
                            txtActualValue1.Enabled = false;
                            txtActualScore.Enabled = false;
                            txtActualValue1.BackColor = System.Drawing.Color.LightGray;
                            RequiredvalidatorTextbox.Enabled = false;
                        }
                        else
                        {
                            Label lbldesc = (Label)e.Row.FindControl("lblDesc");
                            RequiredvalidatorTextbox.Enabled = true;
                            RequiredvalidatorTextbox.ErrorMessage = "Enter Actual value of " + lbldesc.Text;
                            txtReqVal.Visible = true;
                            ddlYes.Visible = false;
                            ddlYes1.Visible = false;
                            txtActualValue1.Visible = true;
                            //Calendar
                            //txtActualValue1.Attributes.Add("maxlength", "28");
                            //txtActualScore.Attributes.Add("maxlength", "28");
                            txtActualValue1.SetDecimalPrefixSuffix(10, 4, false, false, "Actual Value");
                            txtActualScore.SetDecimalPrefixSuffix(10, 4, false, false, "Actual Score");
                            txtActualValue1.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this)");
                            fteAmount.Enabled = true;
                            fteAmount2.Enabled = false;
                            ceReqVal1.Enabled = false;
                        }
                        //MEE_txtPer.Enabled = false;
                    }
                    //This is to disable desc,diffper,diffmark,fieldatt,numericope for all other years
                    if (ddlYear.SelectedIndex > 0)
                    {
                        txtDiffPer.Enabled = false;
                        txtDiffMark.Enabled = false;
                        ddlField.Enabled = false;
                        ddlNumeric.Enabled = false;
                    }
                    if (hdnEdit_Status.Value.Trim() == "0" && intCreditScoreTran_ID > 0)  //14/12/2010
                    {
                        txtReqVal.ReadOnly = true;
                        txtActualValue1.ReadOnly = true;

                    }
                    if (Request.QueryString.Get("qsMode").ToString() == "Q")
                        ceReqVal1.Enabled = false;
                    iCount++;
                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    protected void grvAdditional_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //DataTable dtDefaultNew=new DataTable();
        //string strReqValue = string.Empty;
        //dtDefaultNew = (DataTable)ViewState["AdditionalInfo"];
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (Request.QueryString.Get("qsMode").ToString() == "Q")
        //    {

        //        TextBox txtActualValue3 = ((TextBox)e.Row.FindControl("txtAddtActualValue"));
        //        txtActualValue3.ReadOnly = true;

        //    }
        //    if ((dtDefaultNew != null) && (dtDefaultNew.Rows[iCount]["FieldAtt"].ToString() != ""))
        //    {

        //        TextBox txtActualValue1 = ((TextBox)e.Row.FindControl("txtAddtActualValue"));

        //        RangeValidator RangePercentage = ((RangeValidator)e.Row.FindControl("RangeAddtPercentage"));//range validator
        //        RequiredFieldValidator RequiredvalidatorTextbox = ((RequiredFieldValidator)e.Row.FindControl("RequiredvalidatorAddt"));//range validator
        //        RangePercentage.Enabled = false;
        //        RequiredvalidatorTextbox.Enabled = false;

        //        txtActualValue1.Visible = true;
        //        txtActualValue1.Attributes.Add("maxlength", "29");


        //        AjaxControlToolkit.CalendarExtender ceReqVal1 = e.Row.FindControl("CalendarExtender2") as AjaxControlToolkit.CalendarExtender;
        //        AjaxControlToolkit.FilteredTextBoxExtender fteAmount = e.Row.FindControl("fteAddtAmount1") as AjaxControlToolkit.FilteredTextBoxExtender;
        //        AjaxControlToolkit.FilteredTextBoxExtender fteAmount2 = e.Row.FindControl("fteAddtAmount2") as AjaxControlToolkit.FilteredTextBoxExtender;

        //        if (Convert.ToInt32(hdnCreditScoreID.Value) > 0 && Convert.ToInt32(hdnCreditScoreTran_ID.Value) > 0)
        //        {
        //            if (DS.Tables[0].Rows.Count > 0 && DS.Tables[3].Rows.Count > 0)
        //            {
        //                Label lblYearvaluesID = (Label)e.Row.FindControl("lblAddtYearvaluesID");
        //                txtActualValue1.Text = DS.Tables[3].Rows[intRow]["Actual_Value"].ToString();
        //                lblYearvaluesID.Text = DS.Tables[3].Rows[intRow]["CRDTGUIDETRANSYEARVAL_ID"].ToString();
        //                intRow++;
        //            }
        //        }
        //        if (dtDefaultNew.Rows.Count > 0 && dtDefaultNew.Rows[iCount]["FieldAtt"].ToString() == "5")//date attributes
        //        {
        //            Label lbldesc = (Label)e.Row.FindControl("lblAddtDesc");

        //            txtActualValue1.Attributes.Add("readonly", "readonly");

        //            //RequiredvalidatorTextbox.Enabled = true;
        //            RequiredvalidatorTextbox.ErrorMessage = "Enter Actual value of " + lbldesc.Text;
        //            ceReqVal1.Enabled = true;
        //            fteAmount.Enabled = false;
        //            fteAmount2.Enabled = false;
        //            ceReqVal1.Format = strDateFormat;

        //        }
        //        else if (dtDefaultNew.Rows.Count > 0 && dtDefaultNew.Rows[iCount]["FieldAtt"].ToString() == "2")//percentage
        //        {
        //            Label lbldesc = (Label)e.Row.FindControl("lblAddtDesc");
        //            RequiredvalidatorTextbox.Enabled = true;
        //            RequiredvalidatorTextbox.ErrorMessage = "Enter Actual value of " + lbldesc.Text;


        //            txtActualValue1.Attributes.Add("maxlength", "5");
        //            fteAmount2.Enabled = true;
        //            fteAmount.Enabled = false;
        //            RangePercentage.Enabled = true;
        //            //RequiredvalidatorTextbox.Enabled = true;
        //            RangePercentage.Enabled = true;

        //        }
        //        else if (dtDefaultNew.Rows.Count > 0 && dtDefaultNew.Rows[iCount]["FieldAtt"].ToString() == "3")//ratio
        //        {
        //            Label lbldesc = (Label)e.Row.FindControl("lblAddtDesc");
        //            RequiredvalidatorTextbox.Enabled = true;
        //            RequiredvalidatorTextbox.ErrorMessage = "Enter Actual value of " + lbldesc.Text;

        //            txtActualValue1.Enabled = true;
        //            txtActualValue1.Visible = true;
        //            fteAmount2.Enabled = true;
        //            fteAmount.Enabled = false;
        //            txtActualValue1.Attributes.Add("maxlength", "7");

        //            //RequiredvalidatorTextbox.Enabled = true;

        //        }
        //        else
        //        {


        //            Label lbldesc = (Label)e.Row.FindControl("lblAddtDesc");
        //            //RequiredvalidatorTextbox.Enabled = true;
        //            RequiredvalidatorTextbox.ErrorMessage = "Enter Actual value of " + lbldesc.Text;

        //            txtActualValue1.Visible = true;
        //            //Calendar
        //            //txtActualValue1.Attributes.Add("maxlength", "28");
        //            //txtActualScore.Attributes.Add("maxlength", "28");
        //            txtActualValue1.SetDecimalPrefixSuffix(10, 0, false, false, "Actual Value");

        //            txtActualValue1.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this)");
        //            fteAmount.Enabled = true;
        //            fteAmount2.Enabled = false;
        //            ceReqVal1.Enabled = false;

        //        }
        //        //This is to disable desc,diffper,diffmark,fieldatt,numericope for all other years

        //        if (Request.QueryString.Get("qsMode").ToString() == "Q")
        //            ceReqVal1.Enabled = false;
        //        iCount++;
        //    }

        //}

    }

    protected void FunCalculation(object sender, EventArgs e)
    {
        try
        {
            decimal dcAmount_Set = 0;
            decimal dcAmount_Set_Point = 0;
            decimal dcAdditionalPercentage = 0;
            decimal dcAdditionalPercentageMark = 0;
            decimal dcAdditionalPercentageAmount = 0;
            decimal dcEntered_Amount = 0;
            decimal dcBalance_Amount = 0;
            decimal dcCreditScore = 0;

            if (Request.QueryString.Get("qsMode").ToString().Trim() == "M" || Request.QueryString.Get("qsMode").ToString().Trim() == "C")
            {
                decimal dcTotalScores = 0;
                foreach (GridViewRow gr in grvCreditScore.Rows)
                {
                    TextBox txtActualScore2 = (TextBox)gr.FindControl("txtActualScore1");
                    if (!string.IsNullOrEmpty(txtActualScore2.Text))
                        dcTotalScores += Convert.ToDecimal(txtActualScore2.Text);
                    Session["PreYearValue"] = dcTotalScores.ToString("0.00");
                }
            }

            string strFieldAtt = ((TextBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvCreditScore_")).Replace("grvCreditScore_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;

            //270000
            DropDownList ddlFieldAtt = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlFieldAtt");
            TextBox txtEntered_Amount = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtActualValue1");
            TextBox txtActualScore1 = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtActualScore1");
            if (ddlFieldAtt.SelectedValue == "1")
            {
                try
                {
                    if (string.IsNullOrEmpty(txtEntered_Amount.Text.Trim()))
                    {
                        txtActualScore1.Text = string.Empty;
                        txtEntered_Amount.Focus();
                        FunPubGetTotalActualScore();
                        return;

                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (ddlFieldAtt.SelectedValue == "5")
            {
                try
                {
                    //DateTime tempdt = Convert.ToDateTime(txtEntered_Amount.Text); // to validate; 
                    DateTime tempdt = Utility.StringToDate(txtEntered_Amount.Text); // to validate; 
                    txtEntered_Amount.Text = tempdt.ToString(strDateFormat);// DateTime.Parse(dsAsset.Tables[3].Rows[0]["Proforma_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                catch (FormatException fex)
                {
                    Utility.FunShowAlertMsg(this, "Enter proper date format");
                    txtEntered_Amount.Text = "";
                    txtEntered_Amount.Focus();
                    return;
                }
            }
            if (ddlFieldAtt.SelectedValue == "2") //percentage
            {
                if (txtEntered_Amount.Text.Contains("."))
                {
                    if (txtEntered_Amount.Text.Length - 1 != txtEntered_Amount.Text.Replace(".", "").Length)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Percentage');", true);
                        txtEntered_Amount.Text = string.Empty;
                        txtEntered_Amount.Focus();
                        return;
                    }
                    int RatioSep = txtEntered_Amount.Text.IndexOf('.');
                    if (txtEntered_Amount.Text.Length - 1 == txtEntered_Amount.Text.IndexOf('.'))
                    {
                        txtEntered_Amount.Text = txtEntered_Amount.Text + "00";
                    }
                    if (RatioSep == 0)
                    {
                        txtEntered_Amount.Text = "0" + txtEntered_Amount.Text;
                    }
                }


            }

            if (ddlFieldAtt.SelectedValue == "2") //percentage
            {
                if (txtEntered_Amount.Text.Contains("."))
                {
                    int DecSep = txtEntered_Amount.Text.IndexOf('.');
                    string Frac = txtEntered_Amount.Text.Substring(DecSep + 1);
                    if (Frac.Length >= 5)
                    {
                        Frac = Frac.Substring(0, 5);
                        string ActValue = txtEntered_Amount.Text.Substring(0, DecSep) + "." + Frac;
                        if (Convert.ToDecimal(ActValue) >= 100)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                            txtEntered_Amount.Focus();
                            return;
                        }
                    }
                }
                if (txtEntered_Amount.Text.Contains(":"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Percentage');", true);
                    txtEntered_Amount.Text = string.Empty;
                    txtEntered_Amount.Focus();
                    return;
                }
                if (!string.IsNullOrEmpty(txtEntered_Amount.Text.Trim()))
                {
                    if (Convert.ToDecimal(txtEntered_Amount.Text) >= 100)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                        txtEntered_Amount.Focus();
                        return;
                    }
                }
                else
                {
                    txtActualScore1.Text = string.Empty;
                    txtEntered_Amount.Focus();
                    FunPubGetTotalActualScore();
                    return;
                }
            }
            //200000
            TextBox txtAmount_Set = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1");
            //10
            txtAmount_Set.Enabled = false;
            TextBox txtAmount_Set_Point = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtScore1");
            // 10%
            TextBox txtAdditionalPercentage = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtDiffPer");
            //2
            TextBox txtAdditional_Mark = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtDiffMark");

            if (string.IsNullOrEmpty(txtAdditionalPercentage.Text.Trim()))
            {
                txtAdditionalPercentage.Text = "0";
            }
            if (string.IsNullOrEmpty(txtAdditional_Mark.Text.Trim()))
            {
                txtAdditional_Mark.Text = "0";
            }

            DropDownList ddlNumeric = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlNumeric");
            if (ddlFieldAtt.SelectedValue == "3") //
            {
                if (!txtEntered_Amount.Text.Contains(":"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert5", "alert('Enter the valid ratio');", true);
                    txtEntered_Amount.Text = string.Empty;
                    txtEntered_Amount.Focus();
                    return;
                }
                try
                {
                    string[] strRatioReqvalue = txtAmount_Set.Text.Split(':');
                    string[] strActualvalue = txtEntered_Amount.Text.Split(':');

                    float intReqFirst = float.Parse(strRatioReqvalue[0]);//this is Required value(master value)
                    float intReqSecond = float.Parse(strRatioReqvalue[1]);//this is Required value( value)

                    float intActualFirst = float.Parse(strActualvalue[0]); //this is Actual value(user enterd value)
                    float intActualSecond = float.Parse(strActualvalue[1]);//this is Actual value(user enterd value)

                    if (intActualFirst == 0 && intActualSecond == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert6", "alert('Enter the valid ratio');", true);
                        txtEntered_Amount.Text = string.Empty;
                        txtEntered_Amount.Focus();
                        return;
                    }
                    if (strActualvalue.Length > 2)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert6", "alert('Enter the valid ratio');", true);
                        txtEntered_Amount.Text = string.Empty;
                        txtEntered_Amount.Focus();
                        return;
                    }
                    float flReqResult = intReqFirst / (intReqFirst + intReqSecond); //master result
                    // float flActualResult = intActualFirst / (intActualFirst + intActualSecond);//transaction result
                    float flActualResult = (intReqFirst / intReqSecond) * (intActualFirst / (intActualFirst + intActualSecond));//transaction result
                    float flActualResult2 = (intReqFirst / intReqSecond) * (intActualSecond / (intActualSecond + intActualFirst));//transaction result
                    float flActualFinalResult = flActualResult / (flActualResult + flActualResult2);//transaction result


                    //--------------02/12/2010 added for ratio additional score
                    float flAdditionalPecenAmount = 0;
                    float flAdditionalPecenMark = 0;
                    float flbalanceAmount = 0;
                    float flAmount_Set_Point = 0;
                    float flActualScore = 0;

                    if (txtAmount_Set_Point.Text.Trim() != "")
                        flAmount_Set_Point = float.Parse(txtAmount_Set_Point.Text);

                    if (txtAdditionalPercentage.Text.Trim() != "") //calculation of addtional percentage in ratio
                    {
                        if (Convert.ToDecimal(txtAdditionalPercentage.Text.Trim()) > 0)
                        {
                            flAdditionalPecenAmount = flReqResult * (float.Parse(txtAdditionalPercentage.Text) / 100);

                            if (txtAdditional_Mark.Text.Trim() != "")
                                flAdditionalPecenMark = float.Parse(txtAdditional_Mark.Text);
                            //flbalanceAmount = (flActualFinalResult - flReqResult) / flActualFinalResult;

                            flbalanceAmount = flActualFinalResult - flReqResult; //27/11/2010

                            if (flbalanceAmount < flAdditionalPecenAmount)
                                flbalanceAmount = 0;
                            if (flbalanceAmount > flAdditionalPecenAmount)
                            {
                                decimal flfold;
                                flfold = Math.Floor(Convert.ToDecimal(flbalanceAmount) / Convert.ToDecimal(flAdditionalPecenAmount));
                                flbalanceAmount = float.Parse(flfold.ToString()) * flAdditionalPecenAmount;
                            }
                        }
                    }
                    //--------------02/12/2010 added for ratio additional score end=============
                    DataTable Dt = new DataTable();
                    Dt.Columns.Add("Actual_Value");
                    Dt.Columns.Add("Entered_Value");
                    DataRow drow1 = Dt.NewRow();
                    drow1["Actual_Value"] = flReqResult;
                    drow1["Entered_Value"] = flActualFinalResult;
                    Dt.Rows.Add(drow1);
                    int intCount = (int)Dt.Compute("count(Entered_Value)", Convert.ToDecimal(flActualFinalResult).ToString() + ddlNumeric.SelectedItem.Text + Convert.ToDecimal(flReqResult).ToString());
                    Dt = null;
                    if (ddlNumeric.SelectedItem.Text.Equals("<") || ddlNumeric.SelectedItem.Text.Equals("<="))
                    {
                        if (intCount > 0)
                        {
                            //txtActualScore1.Text = txtAmount_Set_Point.Text.ToString();
                            if (flActualFinalResult == 0)
                            {
                                txtActualScore1.Text = Convert.ToDecimal(flActualScore.ToString()).ToString("0.00");
                                FunPubGetTotalActualScore();
                                return;
                            }
                            flbalanceAmount = flReqResult - flActualFinalResult;
                            if (Convert.ToDecimal(txtAdditionalPercentage.Text.Trim()) > 0)
                            {
                                if (flbalanceAmount < flAdditionalPecenAmount)
                                    flbalanceAmount = 0;
                                if (flbalanceAmount > flAdditionalPecenAmount)
                                {

                                    decimal flfold;
                                    flfold = Math.Floor(Convert.ToDecimal(flbalanceAmount) / Convert.ToDecimal(flAdditionalPecenAmount));
                                    flbalanceAmount = float.Parse(flfold.ToString()) * flAdditionalPecenAmount;

                                }

                                if (intCount > 0)
                                    flActualScore = +flAmount_Set_Point + ((flbalanceAmount / flAdditionalPecenAmount) * flAdditionalPecenMark);

                                txtActualScore1.Text = Convert.ToDecimal(flActualScore.ToString()).ToString("0.00");
                                FunPubGetTotalActualScore();
                                return;
                            }
                            else
                            {
                                txtActualScore1.Text = Convert.ToDecimal(flAmount_Set_Point.ToString()).ToString("0.00");
                                FunPubGetTotalActualScore();///this point no diff percentage so added only score
                                return;
                            }

                        }
                        else
                        {
                            txtActualScore1.Text = "0";
                            FunPubGetTotalActualScore();
                            return;
                        }
                    }
                    else
                    {
                        if (intCount > 0 && (flbalanceAmount >= 0))
                        {
                            if (flAdditionalPecenMark != 0)
                                flActualScore = +flAmount_Set_Point + ((flbalanceAmount / flAdditionalPecenAmount) * flAdditionalPecenMark);
                            else
                                flActualScore = flAmount_Set_Point;

                        }
                        if (intCount == 0 && (flbalanceAmount > 0))  //it's  used for '=' operator greater than target
                        {
                            if (ddlNumeric.SelectedItem.Text.Trim() != "=")
                            {
                                if (flAdditionalPecenMark != 0)
                                    flActualScore = flAmount_Set_Point + ((flbalanceAmount / flAdditionalPecenAmount) * flAdditionalPecenMark);
                                else
                                    flActualScore = flAmount_Set_Point;
                            }
                        }
                        txtActualScore1.Text = flActualScore.ToString("0.00");
                        FunPubGetTotalActualScore();
                        return;
                    }
                }
                catch (Exception exx)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert67", "alert('Enter the valid ratio');", true);
                    txtEntered_Amount.Text = string.Empty;
                    txtEntered_Amount.Focus();
                    return;
                }

            }

            //if (string.IsNullOrEmpty(txtAmount_Set.Text.Trim()))//This calculation for not numric operator only 'yes' or 'No' 
            if (ddlFieldAtt.SelectedValue == "4")
            {
                DropDownList ddlYes1 = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1");
                DropDownList ddlYes = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlYes");

                if (ddlYes1.SelectedValue == "1")
                {
                    if (ddlYes.SelectedValue == "1")
                        txtActualScore1.Text = txtAmount_Set_Point.Text.ToString();
                    else
                        txtActualScore1.Text = "0";

                    FunPubGetTotalActualScore();
                }
            }

            else //This calculation for numric operator only 
            {
                if (ddlFieldAtt.SelectedValue == "5") //in case field attribute is date then
                {
                    ///--------------------
                    ///
                    dcAmount_Set_Point = Convert.ToDecimal(txtAmount_Set_Point.Text);
                    int intCount = Utility.CompareDates(txtAmount_Set.Text, txtEntered_Amount.Text);//amount set is start date and enteramount is end date

                    if (intCount == -1) //SD lesser
                    {
                        if (ddlNumeric.SelectedItem.Text.Equals("<") || ddlNumeric.SelectedItem.Text.Equals("<="))
                        {
                            dcCreditScore = dcAmount_Set_Point;
                        }
                    }
                    else if (intCount == 1)//SD greater
                    {
                        if (ddlNumeric.SelectedItem.Text.Equals(">"))
                        {
                            dcCreditScore = dcAmount_Set_Point;
                        }
                        if (ddlNumeric.SelectedItem.Text.Equals(">="))
                        {
                            dcCreditScore = dcAmount_Set_Point;
                        }

                    }
                    else if (intCount == 0) //equal
                    {
                        if (ddlNumeric.SelectedItem.Text.Equals("<="))
                        {
                            dcCreditScore = dcAmount_Set_Point;
                        }
                        if (ddlNumeric.SelectedItem.Text.Equals("="))
                        {
                            dcCreditScore = dcAmount_Set_Point;
                        }
                        if (ddlNumeric.SelectedItem.Text.Equals(">="))//added 27/11/2010
                        {
                            dcCreditScore = dcAmount_Set_Point;
                        }

                    }

                }
                else
                {
                    dcAmount_Set = Convert.ToDecimal(txtAmount_Set.Text);
                    dcAmount_Set_Point = Convert.ToDecimal(txtAmount_Set_Point.Text);

                    dcAdditionalPercentage = Convert.ToDecimal(txtAdditionalPercentage.Text);
                    dcAdditionalPercentageMark = Convert.ToDecimal(txtAdditional_Mark.Text);
                    dcAdditionalPercentageAmount = dcAmount_Set * (dcAdditionalPercentage / 100);

                    dcEntered_Amount = Convert.ToDecimal(txtEntered_Amount.Text);
                    dcBalance_Amount = dcEntered_Amount - dcAmount_Set;

                    string strNegativeApply = ViewState["Negative"].ToString();

                    //Thangam--
                    //Need to hadle GPS value
                    if (dcBalance_Amount < dcAdditionalPercentageAmount && strNegativeApply == "0")
                        dcBalance_Amount = 0;
                    if (dcBalance_Amount > dcAdditionalPercentageAmount || strNegativeApply == "1")
                    {

                        if (Convert.ToDecimal(txtAdditionalPercentage.Text.Trim()) > 0)
                        {
                            decimal dcfold;
                            dcfold = Math.Floor(dcBalance_Amount / dcAdditionalPercentageAmount);
                            dcBalance_Amount = dcfold * dcAdditionalPercentageAmount;
                        }
                    }

                    string strOperator = ddlNumeric.SelectedItem.Text;
                    dcCreditScore = Convert.ToDecimal(0);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Actual_Value");
                    dt.Columns.Add("Entered_Value");
                    DataRow drow = dt.NewRow();
                    drow["Actual_Value"] = dcAmount_Set;
                    drow["Entered_Value"] = dcEntered_Amount;
                    dt.Rows.Add(drow);
                    int intCount = (int)dt.Compute("count(Entered_Value)", Convert.ToDecimal(dcEntered_Amount).ToString() + ddlNumeric.SelectedItem.Text + Convert.ToDecimal(dcAmount_Set).ToString());
                    dt = null;
                    if (ddlNumeric.SelectedItem.Text.Equals("<") || ddlNumeric.SelectedItem.Text.Equals("<="))
                    {
                        //--------added 2/12/2010 by ramesh less than value calculation
                        dcBalance_Amount = dcAmount_Set - dcEntered_Amount;

                        //Thangam--
                        //Need to hadle GPS value
                        if (dcBalance_Amount < dcAdditionalPercentageAmount && strNegativeApply == "0")
                            dcBalance_Amount = 0;
                        if (Convert.ToDecimal(txtAdditionalPercentage.Text.Trim()) > 0)
                        {
                            //Thangam--
                            if (dcBalance_Amount > dcAdditionalPercentageAmount || strNegativeApply == "1")
                            {

                                decimal dcfold;
                                dcfold = Math.Floor(dcBalance_Amount / dcAdditionalPercentageAmount);
                                dcBalance_Amount = dcfold * dcAdditionalPercentageAmount;

                            }
                            if (intCount > 0)
                                dcCreditScore = +dcAmount_Set_Point + ((dcBalance_Amount / dcAdditionalPercentageAmount) * dcAdditionalPercentageMark);

                            if (intCount == 0)
                                dcCreditScore = dcAmount_Set_Point + ((dcBalance_Amount / dcAdditionalPercentageAmount) * dcAdditionalPercentageMark);

                        }
                        else
                        {
                            if (intCount > 0)
                                dcCreditScore = dcAmount_Set_Point;
                        }
                        //--------added 2/12/2010 by ramesh less than value calculation
                    }
                    else
                    {//Thangam--
                        if (dcEntered_Amount > 0)
                        {
                            if (intCount > 0 && (dcBalance_Amount >= 0 || strNegativeApply == "1"))
                            {
                                if (dcAdditionalPercentageMark != 0)
                                    dcCreditScore = +dcAmount_Set_Point + ((dcBalance_Amount / dcAdditionalPercentageAmount) * dcAdditionalPercentageMark);
                                else
                                    dcCreditScore = dcAmount_Set_Point;

                                if (ddlNumeric.SelectedItem.Text.Equals("="))
                                {
                                    if (dcBalance_Amount < 0)
                                        dcCreditScore = 0;
                                    else
                                        dcCreditScore = dcAmount_Set_Point;
                                }
                            }//Thangam--
                            if (intCount == 0 && (dcBalance_Amount > 0 || (dcBalance_Amount <= 0 && strNegativeApply == "1"))) //it's  used for '=' operator greater than target
                            {
                                //Thangam--
                                //if (ddlNumeric.SelectedItem.Text.Equals("="))
                                //{
                                if (dcAdditionalPercentageMark != 0)
                                    dcCreditScore = dcAmount_Set_Point + ((dcBalance_Amount / dcAdditionalPercentageAmount) * dcAdditionalPercentageMark);
                                else
                                    dcCreditScore = dcAmount_Set_Point;
                                //}

                                if (ddlNumeric.SelectedItem.Text.Equals("="))
                                {
                                    if (dcBalance_Amount < 0)
                                        dcCreditScore = 0;
                                    else
                                        dcCreditScore = dcAmount_Set_Point;
                                }
                            }

                        }
                        else
                        {
                            dcCreditScore = 0;
                        }
                    }


                }
                txtActualScore1.Text = dcCreditScore.ToString("0.00");
            }
            FunPubGetTotalActualScore();  //total Score
            System.Web.HttpCookie aCookie = new System.Web.HttpCookie(txtEntered_Amount.Text);
            aCookie.Expires = DateTime.Now;
            txtEntered_Amount.Focus();

        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public void FunPubGetTotalActualScore()
    {
        //decimal dcTotalScore = 0;
        //foreach (GridViewRow gr in grvCreditScore.Rows)
        //{
        //    TextBox txtActualScore2 = (TextBox)gr.FindControl("txtActualScore1");
        //    if (!string.IsNullOrEmpty(txtActualScore2.Text))
        //        dcTotalScore += Convert.ToDecimal(txtActualScore2.Text);
        //}
        //if (intCreditScoreTran_ID > 0)
        //{
        //    if (Request.QueryString.Get("qsMode").ToString().Trim() != "M")//create mode
        //    {
        //        if (Convert.ToDecimal(dcTotalScore.ToString("0.00")) < Convert.ToDecimal(Session["Score"])) //compare original score and master score
        //        {

        //            if ((Convert.ToDecimal(Session["TotalScore"]) + Convert.ToDecimal(dcTotalScore.ToString("0.00"))) < Convert.ToDecimal(Session["Score"]))
        //                txtTotalScore.Text = (Convert.ToDecimal(Session["TotalScore"]) + Convert.ToDecimal(dcTotalScore.ToString("0.00"))).ToString();
        //            else
        //                txtTotalScore.Text = Session["Score"].ToString();

        //        }
        //        else
        //        {
        //            txtTotalScore.Text = Session["Score"].ToString();
        //        }

        //    }
        //    else          //modify mode
        //    {
        //        Session["TotalScore"] = ((Convert.ToDecimal(Session["TotalScore"]) - Convert.ToDecimal(Session["PreYearValue"])) + dcTotalScore).ToString();
        //        if (Convert.ToDecimal(Session["TotalScore"]) > Convert.ToDecimal(Session["Score"]))
        //            txtTotalScore.Text = Session["Score"].ToString();
        //        else
        //            txtTotalScore.Text = Session["TotalScore"].ToString();

        //    }
        //}
        //else//create mode
        //{
        //    decimal dctemp = 0;
        //    if (dcTotalScore < Convert.ToDecimal(Session["Score"]))
        //    {
        //        txtTotalScore.Text = ((Convert.ToDecimal(Session["TotalScore"]) - Convert.ToDecimal(Session["PreYearValue"])) + dcTotalScore).ToString();
        //        Session["TotalScore"] = ((Convert.ToDecimal(Session["TotalScore"]) - Convert.ToDecimal(Session["PreYearValue"])) + dcTotalScore).ToString();
        //        if (Convert.ToDecimal(Session["TotalScore"]) > Convert.ToDecimal(Session["Score"]))
        //            txtTotalScore.Text = Session["Score"].ToString();
        //        if (Convert.ToDecimal(Session["TotalScore"]) < 0)
        //        {
        //            txtTotalScore.Text = "0.00";
        //        }
        //    }
        //    else
        //    {
        //        Session["TotalScore"] = ((Convert.ToDecimal(Session["TotalScore"]) - Convert.ToDecimal(Session["PreYearValue"])) + dcTotalScore).ToString();
        //        txtTotalScore.Text = Session["Score"].ToString();
        //    }

        //}
        //txtActualTotal.Text = Session["TotalScore"].ToString();


        DataTable CreditScore = (DataTable)ViewState["CreditScore"];

        decimal dcYearTotal = 0, dcTotal = 0;

        foreach (GridViewRow gr in grvCreditScore.Rows)
        {
            TextBox txtActualScore1 = (TextBox)gr.FindControl("txtActualScore1");
            if (!string.IsNullOrEmpty(txtActualScore1.Text))
            {
                dcYearTotal = dcYearTotal + Convert.ToDecimal(txtActualScore1.Text);
            }
        }

        DataTable dtExistsCS = CreditScore.Clone();
        DataRow[] drExistsCS = CreditScore.Select("Year<>'" + rbtYears.SelectedValue + "'");
        drExistsCS.CopyToDataTable<DataRow>(dtExistsCS, LoadOption.OverwriteChanges);

        for (int i = 0; i <= dtExistsCS.Rows.Count - 1; i++)
        {
            if (!string.IsNullOrEmpty(dtExistsCS.Rows[i]["Actual_Score"].ToString()))
            {
                dcTotal = dcTotal + Convert.ToDecimal(dtExistsCS.Rows[i]["Actual_Score"].ToString());
            }
        }

        txtActualTotal.Text = dcYearTotal.ToString("0.00");
        txtTotalScore.Text = (dcTotal + dcYearTotal).ToString("0.00");
    }

    protected void ddlYes_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((DropDownList)sender).ClientID;
        string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvCreditScore_")).Replace("grvCreditScore_ctl", "");
        int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
        gRowIndex = gRowIndex - 2;

        DropDownList ddlYes1 = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1");
        DropDownList ddlYes = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlYes");
        TextBox txtScore = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtScore1");
        TextBox txtActualScore1 = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtActualScore1");

        if (Request.QueryString.Get("qsMode").ToString().Trim() == "M" || Request.QueryString.Get("qsMode").ToString().Trim() == "C")
        {
            decimal dcTotalScores = 0;
            foreach (GridViewRow gr in grvCreditScore.Rows)
            {
                TextBox txtActualScore3 = (TextBox)gr.FindControl("txtActualScore1");
                if (!string.IsNullOrEmpty(txtActualScore3.Text))
                    dcTotalScores += Convert.ToDecimal(txtActualScore3.Text);
                Session["PreYearValue"] = dcTotalScores.ToString("0.00");
            }
        }

        if (ddlYes.Visible == true)
        {
            decimal dcTotalScore = Convert.ToDecimal(txtScore.Text);
            if (ddlYes.SelectedValue == ddlYes1.SelectedValue)
                txtActualScore1.Text = dcTotalScore.ToString("0.00");
            else
                txtActualScore1.Text = "0.00";

        }
        FunPubGetTotalActualScore();

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjCreditScoreClient = new CreditMgtServicesReference.CreditMgtServicesClient();

        try
        {

            CreditMgtServices.S3G_ORG_CreditGuideTransactionRow ObjCreditGuideTransRow;
            ObjCreditGuideTransRow = ObjS3G_ORG_CreditGuideTransactionDataTable.NewS3G_ORG_CreditGuideTransactionRow();
            ObjCreditGuideTransRow.CreditScore_Guide_ID = Convert.ToInt32(hdnCreditScoreID.Value);
            ObjCreditGuideTransRow.CreditGuideTransaction_ID = Convert.ToInt32(hdnCreditScoreTran_ID.Value);
            if (ViewState["hdnLobid"] != null && !string.IsNullOrEmpty(ViewState["hdnLobid"].ToString()))
            {
                ObjCreditGuideTransRow.LOB_ID = Convert.ToInt32(ViewState["hdnLobid"]);
            }
            else
            {
                ObjCreditGuideTransRow.LOB_ID = 0;
            }
            ObjCreditGuideTransRow.Constitution = ViewState["hdnConstitutionid"] == null ? 0 : Convert.ToInt32(ViewState["hdnConstitutionid"]);
            ObjCreditGuideTransRow.Product_ID = Session["Product_ID"] == null ? 0 : Convert.ToInt32(Session["Product_ID"]);
            ObjCreditGuideTransRow.Past_Years = txtPastYears.Text == "" ? 0 : Convert.ToInt32(txtPastYears.Text);
            ObjCreditGuideTransRow.Future_Years = txtFutureYears.Text == "" ? 0 : Convert.ToInt32(txtFutureYears.Text);
            ObjCreditGuideTransRow.PastYear_StartingFrom = txtPastYearStartFrom.Text == "" ? "0" : txtPastYearStartFrom.Text;
            ObjCreditGuideTransRow.Modified_By = uinfo.ProUserIdRW;
            ObjCreditGuideTransRow.Enq_Cus_App_ID = intCreditScoreTran_ID > 0 ? 0 : Convert.ToInt32(ViewState["Enq_Cus_App_ID"].ToString());
            if (PageMode == PageModes.Create)
            {
                ObjCreditGuideTransRow.Is_CustomerID_EnquiryNumber = Convert.ToInt32(ddlDocumentType.SelectedValue);
            }
            else
            {
                ObjCreditGuideTransRow.Is_CustomerID_EnquiryNumber = 0;
            }
            if (Session["Customer_ID"] != null && !string.IsNullOrEmpty(Session["Customer_ID"].ToString()))
            {
                ObjCreditGuideTransRow.Customer_ID = Convert.ToInt32(Session["Customer_ID"]);
            }
            if (PageMode == PageModes.Create)
            {
                ObjCreditGuideTransRow.Document_Type = Convert.ToInt32(ddlDocumentType.SelectedValue);
            }
            else
            {
                ObjCreditGuideTransRow.Document_Type = 0;
            }
            ObjCreditGuideTransRow.Document_Type_ID = Session["Enquiry_Response_ID"] == null ? 0 : Convert.ToInt32(Session["Enquiry_Response_ID"]);
            ObjCreditGuideTransRow.Company_ID = uinfo.ProCompanyIdRW;
            ObjCreditGuideTransRow.Created_By = uinfo.ProUserIdRW;
            ObjCreditGuideTransRow.Modified_By = uinfo.ProUserIdRW;
            //strXML = Utility.FunPubFormXml((DataTable)ViewState["YearvaluesDetails"]);
            strXML = Utility.FunPubFormXml((DataTable)ViewState["CreditScore"]);
            ObjCreditGuideTransRow.XMLCreditGuideTransaction_Year_Values = strXML.ToString();
            ObjCreditGuideTransRow.XMLCredidNumberValue = ((DataTable)ViewState["dtNumber"]).FunPubFormXml();
            ObjCreditGuideTransRow.XMLProcYears = ((DataTable)ViewState["dtProcessed"]).FunPubFormXml();

            if (grvAdditional.Rows.Count > 0)
                ObjCreditGuideTransRow.XMLAddtional = FunPriFormXML();

            ObjS3G_ORG_CreditGuideTransactionDataTable.AddS3G_ORG_CreditGuideTransactionRow(ObjCreditGuideTransRow);
            if (intCreditScoreID > 0 && intCreditScoreTran_ID > 0)
            {
                intResult = ObjCreditScoreClient.FunPubModifyCreditGuideTransaction(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CreditGuideTransactionDataTable, SerMode));
            }
            else
            {
                intResult = ObjCreditScoreClient.FunPubCreateCreditGuideTransaction(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CreditGuideTransactionDataTable, SerMode));
            }
            if (intResult == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert23", "alert('This Combination already exists.');", true);
                return;
            }
            if (intResult == 50)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert24", "alert('Added failed');", true);
                return;
            }
            if (intResult == 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert25", "alert('This record cannot be modified since it is referred in transaction');", true);
                return;
            }
            else
            {
                if (Convert.ToInt32(hdnCreditScoreTran_ID.Value) > 0 && intResult == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", "alert('Credit Guide Transaction details updated successfully');window.location.href='../Origination/S3GORGTransLander.aspx?Code=OCGT&MultipleDNC=1';", true);
                    return;
                }
                else
                {
                    strAlert = "Credit Guide Transaction details added successfully";
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, int.Parse(ddlDocumentType.SelectedValue));
                            strAlert = "";

                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is Not assigned";
                        }
                        ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode, strAlert);
                        return;
                    }
                    else
                    {
                        // This is commented by Rao on 4th Jan - This is forcepull logic need to implement in all screens.

                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(null, txtEnquiryNo.Text.Trim(), ProgramCode, null, null, "O", CompanyId, null, null, txtlob.Text.Trim(), txtproduct.Text.Trim(), out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), txtEnquiryNo.Text.Trim(), int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), int.Parse(ddlDocumentType.SelectedValue));
                        }

                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strAlert += @"\n\nWould you like to add one more credit guide transaction?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }

                }

            }

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {

        }
        catch (Exception)
        {

        }
        finally
        {
            ObjCreditScoreClient.Close();
        }
    }

    private string FunPriFormXML()
    {
        StringBuilder strXMLAddition = new StringBuilder();
        strXMLAddition.Append("<Root>");
        foreach (GridViewRow grvAdd in grvAdditional.Rows)
        {
            Label lblAddtDesc = (Label)grvAdd.FindControl("lblAddtDesc");
            Label lblAddtCrScoreParam_ID = (Label)grvAdd.FindControl("lblAddtCrScoreParam_ID");
            Label lblAddtYearvaluesID = (Label)grvAdd.FindControl("lblAddtYearvaluesID");
            TextBox txtAddtActualValue1 = (TextBox)grvAdd.FindControl("txtAddtActualValue1");
            TextBox txtAddtActualValue2 = (TextBox)grvAdd.FindControl("txtAddtActualValue2");
            TextBox txtAddtActualValue3 = (TextBox)grvAdd.FindControl("txtAddtActualValue3");

            strXMLAddition.Append(" <Details CRDTSCOREGUIDEPARAM_ID='" + lblAddtCrScoreParam_ID.Text +
                                                                    "' ACTUAL_VALUE1='" + txtAddtActualValue1.Text +
                                                                     "' ACTUAL_VALUE2='" + txtAddtActualValue2.Text +
                                                                      "' ACTUAL_VALUE3='" + txtAddtActualValue3.Text +
                                                                     "' /> ");

        }
        strXMLAddition.Append("</Root>");


        return strXMLAddition.ToString();
    }


    /* WorkFlow Properties */
    private int WFLOBId { get { return 8; } }
    private int WFProduct { get { return 3; } }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //wf Cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else if (strMode.Trim() == "C")
        {
            if (TabContainerCGT.ActiveTabIndex != 0)
            {
                TabContainerCGT.ActiveTabIndex = 0;
                FunPriShowAppraisalDetails();
                btnAdd.Visible = false;
            }
            else
                Response.Redirect(strRedirectPage,false);
        }
        else
            Response.Redirect(strRedirectPage,false);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtFutureYears.Text = string.Empty;
        txtPastYears.Text = string.Empty;
        DataTable Dt = new DataTable();
        Dt = null;
        dtCreditScore = null;
        grvCreditScore.DataSource = Dt;
        grvCreditScore.DataBind();
        grvCreditScore.Enabled = true;
        grvAdditional.DataSource = Dt;
        grvAdditional.DataBind();
        grvAdditional.Enabled = true;
        txtActualTotal.Text = string.Empty;
        btnAdd.Enabled = false;
        btnGo.Enabled = true;
        btnSave.Enabled = false;
        //        lblmsg.Text = string.Empty;
        lblAddtMsg.Text = string.Empty;
        txtTotalScore.Text = string.Empty;
        txtFutureYears.Enabled = true;
        txtPastYears.Enabled = true;
        grvCreditScoreYearValues.DataSource = Dt;
        grvCreditScoreYearValues.DataBind();
        //gvrAddtionalYearValue.DataSource = Dt;
        //gvrAddtionalYearValue.DataBind();
        lblActualtotalscore.Visible = false;
        txtActualTotal.Visible = false;
        txtActualTotal.Text = string.Empty;
        txtTotalScore.Text = string.Empty;
        //txtTotalScore.Visible = false;
        txtPastYearStartFrom.Text = DateTime.Now.Year.ToString();
        ViewState["CreditScore"] = null;
        ViewState["YearvaluesDetails"] = null;
        ViewState["AddtYearvaluesDetails"] = null;
        ViewState["dttemp"] = null;
        ViewState["modifydtTemp"] = null;


    }
    private void FunPriBindEnqCustNo_withProduct()
    {
        uinfo = new UserInfo();
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
        DS = Utility.GetDataset("S3G_ORG_GetEnquiryCustomAppraisalForCreditGuideTrans", dictDropdownListParam);
        uinfo = null;
        dictDropdownListParam = null;
        DS.Dispose();
    }


    protected void chkselect_OnCheckedChanged(object sender, EventArgs e)
    {
        int intEnqCusAppid = 0;
        CheckBox chkselect = null;
        btnAdd.Visible = false;
        foreach (GridViewRow grvPagingRow in grvPaging.Rows)
        {
            chkselect = ((CheckBox)grvPagingRow.FindControl("chkselect"));
            if (chkselect.Checked)
            {
                intEnqCusAppid = Convert.ToInt32(chkselect.Attributes["EnqCusAppID"].ToString());
            }
            else
                chkselect.Enabled = false;
        }

        if (intEnqCusAppid == 0)
        {
            foreach (GridViewRow grvPagingRow in grvPaging.Rows)
            {
                chkselect = ((CheckBox)grvPagingRow.FindControl("chkselect"));
                chkselect.Enabled = true;
            }
        }
        if (intEnqCusAppid > 0)
        {
            TabContainerCGT.Tabs[1].Enabled = true;
            FunPriShowEnquiryCustomerDetails(intEnqCusAppid.ToString());
            if (ddlDocumentType.SelectedValue == "1")
            {
                lblEnqNo.Text = "Enquiry No.";
            }
            else if (ddlDocumentType.SelectedValue == "2")
            {
                lblEnqNo.Text = "Pricing No.";
            }
            else if (ddlDocumentType.SelectedValue == "3")
            {
                lblEnqNo.Text = "Application No.";
            }

            txtTotalScore.Text = txtActualTotal.Text = txtPastYears.Text = txtFutureYears.Text = string.Empty;
            txtActualTotal.Visible = false;
            lblActualtotalscore.Visible = false;
            grvCreditScore.Enabled = true;
            ViewState["CreditScore"] = null;
            ViewState["YearvaluesDetails"] = null;
            ViewState["dttemp"] = null;
            btnSave.Enabled = false;
            btnGo.Enabled = true;
            DataTable Dt = new DataTable();
            Dt = null;

            grvCreditScoreYearValues.DataSource = Dt;
            grvCreditScoreYearValues.DataBind();

            grvNumbers.DataSource = Dt;
            grvNumbers.DataBind();

            rbtYears.Items.Clear();
        }
        else
        {
            TabContainerCGT.Tabs[1].Enabled = false;
        }

    }

    protected void grvPaging_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEnqCusAppID = (Label)e.Row.FindControl("lblEnqCusAppID");
            CheckBox chkbox = (CheckBox)e.Row.FindControl("chkselect");
            chkbox.Attributes.Add("EnqCusAppID", lblEnqCusAppID.Text.ToString());

        }
    }

    protected void rbtYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];

        for (int i = rbtYears.Items.Count - 1; i >= dtProcessed.Rows.Count; i--)
        {
            rbtYears.Items[i].Enabled = false;
        }

        btnAddYear.Visible = false;
        if (PageMode != PageModes.Query)
        {
            btnUpdateYear.Visible = true;
        }

        //For Number

        DataTable dtNumber = (DataTable)ViewState["dtNumber"];
        DataRow[] drNumRows = dtNumber.Select("Year='" + rbtYears.SelectedValue + "'");

        DataTable dtNumberYr = dtNumber.Clone();
        drNumRows.CopyToDataTable<DataRow>(dtNumberYr, LoadOption.OverwriteChanges);

        if (dtNumberYr.Rows.Count == 0)
        {
            FunProInitializNumberGrid();
        }
        else
        {

            DataRow[] drProcessed = dtProcessed.Select("Year='" + rbtYears.SelectedValue + "' AND Processed='1'");
            if (drProcessed.Length == 0)
            {
                for (int i = 0; i <= dtNumberYr.Rows.Count - 1; i++)
                {
                    dtNumberYr.Rows[i]["Value"] = "";
                }
                dtNumberYr.AcceptChanges();
            }
            ViewState["dtNumberYr"] = dtNumberYr;
            grvNumbers.DataSource = dtNumberYr;
            grvNumbers.DataBind();
            FunProLoadNumFooterDDL();
        }

        FunProToggleFmlControls();
        grvNumbers.FooterRow.Visible = false;
        grvNumbers.Columns[grvNumbers.Columns.Count - 1].Visible = false;


        //For Main tab

        DataTable CreditScore = (DataTable)ViewState["CreditScore"];
        DataRow[] drMainRows = CreditScore.Select("Year='" + rbtYears.SelectedValue + "'");

        DataTable dtCreditScoreYr = CreditScore.Clone();
        drMainRows.CopyToDataTable<DataRow>(dtCreditScoreYr, LoadOption.OverwriteChanges);

        if (dtCreditScoreYr.Rows.Count == 0)
        {
            FunProInitializeMainGrid();
        }
        else
        {
            ViewState["CreditScoreYr"] = dtCreditScoreYr;

            FunPriBindLookup();
            grvCreditScore.DataSource = dtCreditScoreYr;
            grvCreditScore.DataBind();
        }

        grvCreditScore.FooterRow.Visible = false;
        grvCreditScore.Columns[grvCreditScore.Columns.Count - 1].Visible = false;

        FunPubGetTotalActualScore();
    }

    protected void btnAddYear_Click(object sender, EventArgs e)
    {

    }

    protected void btnUpdateYear_Click(object sender, EventArgs e)
    {
        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];

        for (int i = 0; i <= dtProcessed.Rows.Count - 1; i++)
        {
            if (dtProcessed.Rows[i]["Year"].ToString() == rbtYears.SelectedValue)
            {
                dtProcessed.Rows[i]["Processed"] = "1";
            }
        }

        ViewState["dtProcessed"] = dtProcessed;

        FunProUpdateYearValues();

        if (rbtYears.Items.Count > 1)
        {
            btnUpdateYear.Visible = false;
        }

        btnSave.Enabled = true;
    }

    protected void FunProInitializeMainGrid()
    {
        try
        {
            DataTable CreditScoreYr = new DataTable();
            DataRow dr;
            CreditScoreYr.Columns.Add("CrScoreParam_ID");
            CreditScoreYr.Columns.Add("Desc");
            CreditScoreYr.Columns.Add("FieldAtt");
            CreditScoreYr.Columns.Add("NumericAtt");
            CreditScoreYr.Columns.Add("DiffPer");
            CreditScoreYr.Columns.Add("DiffMark");

            CreditScoreYr.Columns["DiffPer"].DataType = typeof(string);
            CreditScoreYr.Columns["DiffMark"].DataType = typeof(string);

            CreditScoreYr.Columns.Add("ReqValue");
            CreditScoreYr.Columns.Add("Score");
            CreditScoreYr.Columns.Add("Year");

            dr = CreditScoreYr.NewRow();
            CreditScoreYr.Rows.Add(dr);

            FunPriBindLookup();
            grvCreditScore.DataSource = CreditScoreYr;
            grvCreditScore.DataBind();

            CreditScoreYr.Rows[0].Delete();

            ViewState["CreditScore"] = CreditScoreYr;
            ViewState["CreditScoreYr"] = CreditScoreYr;

            grvCreditScore.Rows[0].Visible = false;
            grvCreditScore.Columns[grvCreditScore.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void FunProInitializNumberGrid()
    {
        DataTable dtNumber = new DataTable();
        dtNumber.Columns.Add("LineType");
        dtNumber.Columns.Add("LineTypeID");
        dtNumber.Columns.Add("Desc");
        dtNumber.Columns.Add("Flag_ID");
        dtNumber.Columns.Add("Value");
        dtNumber.Columns.Add("Formula");
        dtNumber.Columns.Add("Flag");
        dtNumber.Columns.Add("IsLink");
        dtNumber.Columns.Add("ParamNum");
        dtNumber.Columns.Add("ParamText");
        dtNumber.Columns.Add("Year");
        dtNumber.Columns.Add("RecordId");

        DataRow dRow = dtNumber.NewRow();
        dtNumber.Rows.Add(dRow);

        grvNumbers.DataSource = dtNumber;
        grvNumbers.DataBind();

        grvNumbers.Rows[0].Visible = false;
        dtNumber.Rows.Clear();

        ViewState["dtNumber"] = dtNumber;
        ViewState["dtNumberYr"] = dtNumber;

        FunProLoadNumFooterDDL();

        if (ViewState["dtProcessed"] == null)
        {
            DataTable dtProcessed = new DataTable();
            dtProcessed.Columns.Add("ID");
            dtProcessed.Columns.Add("Year");

            ViewState["dtProcessed"] = dtProcessed;
        }

        btnAddYear.Enabled = false;
        btnAddYear.Visible = true;
        btnUpdateYear.Visible = false;

        grvNumbers.FooterRow.Visible = true;
        grvNumbers.Columns[grvNumbers.Columns.Count - 1].Visible = true;
    }

    protected void FunProLoadNumFooterDDL()
    {
        DropDownList ddlFLineType = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFLineType");
        DropDownList ddlFFlag = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFlag");
        DropDownList ddlFFmlLines = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFmlLines");



        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Is_Number", "1");

        DataSet Dset = Utility.GetDataset("S3G_ORG_GetCreidtScoreFlags", Procparam);

        ddlFFlag.BindDataTable(Dset.Tables[0], new string[] { "Flag_ID", "Flag_Code" });
        ddlFLineType.BindDataTable(Dset.Tables[1], new string[] { "Lookup_Code", "Lookup_Description" });

        DataTable dtLinks = new DataTable();
        dtLinks.Columns.Add("Value");
        dtLinks.Columns.Add("Text");
        DataRow dRow;
        DataTable dtNumber = (DataTable)ViewState["dtNumberYr"];
        for (int i = 0; i <= dtNumber.Rows.Count - 1; i++)
        {
            dRow = dtLinks.NewRow();

            dRow["Value"] = (i + 1).ToString();
            dRow["Text"] = (i + 1).ToString() + "." + dtNumber.Rows[i]["Flag"].ToString();

            dtLinks.Rows.Add(dRow);
        }

        ddlFFmlLines.BindDataTable(dtLinks, new string[] { "Value", "Text" });

        FunGetFormulaString();
    }

    protected void FunProToggleFmlControls()
    {
        for (int i = 0; i <= grvNumbers.Rows.Count - 1; i++)
        {
            Label lblLineTypeID = (Label)grvNumbers.Rows[i].FindControl("lblLineTypeID");
            Label lblValue = (Label)grvNumbers.Rows[i].FindControl("lblValue");
            TextBox txtValue = (TextBox)grvNumbers.Rows[i].FindControl("txtValue");

            if (lblLineTypeID.Text == "3" || PageMode == PageModes.Query)
            {
                lblValue.Visible = true;
                txtValue.Visible = false;
            }
            else
            {
                lblValue.Visible = false;
                txtValue.Visible = true;
            }
        }
    }

    protected void FunProLoadCreditScoreDetails()
    {
        DataSet dsScore = new DataSet();
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@CreditScore_ID", intCreditScoreID.ToString());
        if (PageMode != PageModes.Create)
        {
            Procparam.Add("@CreditGuideTransaction_ID", intCreditScoreTran_ID.ToString());
            dsScore = Utility.GetDataset("S3G_ORG_GetCreditScorePara_Tans", Procparam);
        }
        else
        {
            dsScore = Utility.GetDataset("S3G_ORG_GetCreditScoreParameterDetails", Procparam);

        }

        ViewState["dtNumber"] = dsScore.Tables[2];
        ViewState["dtProcessed"] = dsScore.Tables[3];

        DataRow[] drCreditScre = dsScore.Tables[1].Select("NumericAtt in (6,7)");
        foreach (DataRow drCredit in drCreditScre)
            drCredit.Delete();
        dsScore.Tables[1].AcceptChanges();

        ViewState["CreditScore"] = dsScore.Tables[1];

        ViewState["AdditionalInfo"] = dsScore.Tables[4];
        grvAdditional.DataSource = dsScore.Tables[4];
        grvAdditional.DataBind();

        if (grvAdditional.Rows.Count > 0)
        {
            TabContainerCGT.Tabs[2].Enabled = true;
        }

        ddlFinanceYear.BindDataTable(dsScore.Tables[3], new string[] { "Year", "Year" });

        rbtYears.Items.Clear();
        for (int i = 1; i <= ddlFinanceYear.Items.Count - 1; i++)
        {
            rbtYears.Items.Add(ddlFinanceYear.Items[i]);
        }

        if (PageMode == PageModes.Query)
        {
            DataTable dtProcessed = dsScore.Tables[3];
            DataRow[] drProcessed = dtProcessed.Select("Processed=''");
            foreach (DataRow drow in drProcessed)
            {
                drow.Delete();
            }
            dtProcessed.AcceptChanges();
            ViewState["dtProcessed"] = dtProcessed;
        }

        rbtYears.SelectedValue = rbtYears.Items[0].Value;
        rbtYears_SelectedIndexChanged(null, null);

        FunGetFormulaString();

        if (rbtYears.Items.Count == 1)
        {
            btnUpdateYear.Visible = true;
        }
    }

    protected void FunGetFormulaString()
    {
        DropDownList ddlFFmlLines = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFmlLines");

        for (int gv = 0; gv <= grvNumbers.Rows.Count - 1; gv++)
        {
            Label lblFormula = (Label)grvNumbers.Rows[gv].FindControl("lblFormula");

            //lblFormula.Text = lblFormula.Text.Replace("<<", "<b><font color=\"green\" >&lt;&lt;</font></b>").Replace(">>", "<b><font color=\"green\" >&gt;&gt;</font></b>");

            for (int i = 1; i <= ddlFFmlLines.Items.Count - 1; i++)
            {
                lblFormula.Text = lblFormula.Text.Replace("(" + ddlFFmlLines.Items[i].Text + ")", "<font color=\"green\" >(" + ddlFFmlLines.Items[i].Text + ")</font>");

                if (lblFormula.Text.Contains("?") && lblFormula.Text.Contains(ddlFFmlLines.Items[i].Text))
                {
                    int intRowIndex = ddlFFmlLines.Items.IndexOf(ddlFFmlLines.Items.FindByText(ddlFFmlLines.Items[i].Text));
                    FunProSetOptionalCheckBox(intRowIndex, gv);
                }
            }
        }
    }

    protected void grvNumbers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblLink = (Label)e.Row.FindControl("lblLink");
            CheckBox chkLink = (CheckBox)e.Row.FindControl("chkLink");
            //DropDownList ddlFmlOption = (DropDownList)e.Row.FindControl("ddlFmlOption");
            //Label lblFormula = (Label)e.Row.FindControl("lblFormula");

            chkLink.Attributes.Add("onclick", "javascript:fnChkFreeze('" + chkLink.ClientID + "')");

            if (lblLink.Text == "1")
            {
                chkLink.Checked = true;
            }
        }
    }

    protected void txtValue_TextChanged(object sender, EventArgs e)
    {
        DataTable dtNumberYr = (DataTable)ViewState["dtNumberYr"];

        int intRowIndex = Utility.FunPubGetGridRowID("grvNumbers", ((TextBox)sender).ClientID);
        DropDownList ddlFFmlLines = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFmlLines");
        CheckBox chkApplyOptional = (CheckBox)grvNumbers.Rows[intRowIndex].FindControl("chkApplyOptional");
        dtNumberYr.Rows[intRowIndex]["Value"] = ((TextBox)sender).Text;
        dtNumberYr.AcceptChanges();

        ViewState["dtNumberYr"] = dtNumberYr;

        for (int i = 0; i <= grvNumbers.Rows.Count - 1; i++)
        {
            Label lblLineTypeID = (Label)grvNumbers.Rows[i].FindControl("lblLineTypeID");
            if (lblLineTypeID.Text == "3")
            {
                TextBox txtValue = (TextBox)grvNumbers.Rows[i].FindControl("txtValue");
                DropDownList ddlFmlOption = (DropDownList)grvNumbers.Rows[i].FindControl("ddlFmlOption");
                Label lblValue = (Label)grvNumbers.Rows[i].FindControl("lblValue");
                Label lblFormula = (Label)grvNumbers.Rows[i].FindControl("lblFormula");
                Regex rx = new Regex(@"^((?<o1>[^-+*/()]+|\([^-+*/()]+\)|(?<p>\()+[^-+*/()]+|[^-+*/()]+(?<-p>\)))[-+*/])+(?<o2>[^-+*/()]+|\([^-+*/()]+\)|(?<p>\()+[^-+*/()]+|[^-+*/()]+(?<-p>\)))(?(p)(?!))$");
                decimal decScore = 0;

                string strFormatedfml = FunProApplyFormula(dtNumberYr.Rows[i]["Formula"].ToString(), ddlFFmlLines);
                if ((!rx.IsMatch(strFormatedfml.Replace("?", "+")) && !decimal.TryParse(strFormatedfml.Replace("?", "+"), out decScore)))
                {
                    lblValue.Text = txtValue.Text = "";
                }
                else
                {

                    if (!strFormatedfml.Contains("?"))
                    {
                        DataTable tempTable = new DataTable();
                        var result = Convert.ToDecimal(tempTable.Compute(strFormatedfml, ""));
                        lblValue.Text = txtValue.Text = result.ToString(Utility.SetSuffix());
                    }
                    else
                    {
                        if (chkApplyOptional.GetFormulaRow() == i)
                        {
                            if (chkApplyOptional.Checked)
                                lblValue.Text = txtValue.Text = ((TextBox)sender).Text;
                            else
                                lblValue.Text = txtValue.Text = "";
                        }
                    }
                }

                dtNumberYr.Rows[i]["Value"] = lblValue.Text;
                dtNumberYr.AcceptChanges();

                ViewState["dtNumberYr"] = dtNumberYr;
            }
        }

        FunProUpdateScores();
        ((TextBox)sender).Focus();
    }

    protected string FunProApplyFormula(string strForumula, DropDownList ddlLines)
    {
        DataTable dtNumber = (DataTable)ViewState["dtNumberYr"];

        if (dtNumber.Rows.Count > 0)
        {
            for (int i = 1; i <= ddlLines.Items.Count - 1; i++)
            {
                string strRplcVal = dtNumber.Rows[i - 1]["Value"].ToString();
                if (strRplcVal == "")
                {
                    strRplcVal = "0";
                }
                strForumula = strForumula.Replace("(" + ddlLines.Items[i].Text + ")", strRplcVal);
            }
        }

        return strForumula;
    }

    protected void FunProSetOptionalCheckBox(int intIndex, int intFormulaRow)
    {
        if (PageMode != PageModes.Query)
        {
            DataTable dtNumber = (DataTable)ViewState["dtNumber"];
            DataRow[] drNumRows = dtNumber.Select("Year='" + rbtYears.SelectedValue + "'");

            DataTable dtNumberYr = dtNumber.Clone();
            drNumRows.CopyToDataTable<DataRow>(dtNumberYr, LoadOption.OverwriteChanges);

            CheckBox chkApplyOptional = (CheckBox)grvNumbers.Rows[intIndex - 1].FindControl("chkApplyOptional");
            chkApplyOptional.Visible = true;
            chkApplyOptional.SetFormulaRow(intFormulaRow);

            TextBox txtValue = (TextBox)grvNumbers.Rows[intIndex - 1].FindControl("txtValue");
            Label lblValue = (Label)grvNumbers.Rows[intIndex - 1].FindControl("lblValue");

            txtValue.Text = lblValue.Text = dtNumberYr.Rows[intIndex - 1]["Value"].ToString();
        }
    }

    protected void FunProUpdateScores()
    {
        //For Number

        DataTable dtNumberYr = (DataTable)ViewState["dtNumberYr"];
        for (int i = 0; i <= dtNumberYr.Rows.Count - 1; i++)
        {
            TextBox txtValue = (TextBox)grvNumbers.Rows[i].FindControl("txtValue");
            dtNumberYr.Rows[i]["Value"] = txtValue.Text;
        }

        DataTable dtNumber = (DataTable)ViewState["dtNumber"];
        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];

        //For Main tab

        DataTable CreditScoreYr = (DataTable)ViewState["CreditScoreYr"];

        for (int i = 0; i <= CreditScoreYr.Rows.Count - 1; i++)
        {
            TextBox txtActualValue1 = (TextBox)grvCreditScore.Rows[i].FindControl("txtActualValue1");
            TextBox txtScore1 = (TextBox)grvCreditScore.Rows[i].FindControl("txtScore1");
            DropDownList ddlYes = (DropDownList)grvCreditScore.Rows[i].FindControl("ddlYes");

            DataRow[] drApply = dtNumberYr.Select("ISLink='1' AND ParamNum='" + (i + 1).ToString() + "'");
            if (drApply.Length > 0)
            {
                foreach (DataRow dRow in drApply)
                {
                    if (CreditScoreYr.Rows[i]["FieldAtt"].ToString() == "4")
                    {
                        if (!string.IsNullOrEmpty(dRow["Value"].ToString()))
                        {
                            ddlYes.SelectedValue = "1";
                            txtScore1.Text = dRow["Value"].ToString();
                        }
                        else
                        {
                            ddlYes.SelectedValue = "0";
                            txtScore1.Text = "0";
                        }
                    }
                    else
                    {
                        txtActualValue1.Text = dRow["Value"].ToString();
                    }
                }
                FunCalculation((object)txtActualValue1, null);
            }

            CreditScoreYr.Rows[i]["Actual_Value"] = txtActualValue1.Text;
        }
    }

    protected void FunProUpdateYearValues()
    {
        //For Number

        DataTable dtNumberYr = (DataTable)ViewState["dtNumberYr"];
        for (int i = 0; i <= dtNumberYr.Rows.Count - 1; i++)
        {
            TextBox txtValue = (TextBox)grvNumbers.Rows[i].FindControl("txtValue");
            dtNumberYr.Rows[i]["Value"] = txtValue.Text;
        }

        DataTable dtNumber = (DataTable)ViewState["dtNumber"];
        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];

        DataTable dtExists = dtNumberYr.Clone();
        DataRow[] drExist = dtNumber.Select("Year<>'" + rbtYears.SelectedValue + "'");
        drExist.CopyToDataTable<DataRow>(dtExists, LoadOption.OverwriteChanges);

        dtExists.Merge(dtNumberYr);
        ViewState["dtNumber"] = dtExists;


        //For Main tab

        DataTable CreditScoreYr = (DataTable)ViewState["CreditScoreYr"];

        for (int i = 0; i <= CreditScoreYr.Rows.Count - 1; i++)
        {
            TextBox txtActualValue1 = (TextBox)grvCreditScore.Rows[i].FindControl("txtActualValue1");
            TextBox txtActualScore1 = (TextBox)grvCreditScore.Rows[i].FindControl("txtActualScore1");
            DropDownList ddlYes = (DropDownList)grvCreditScore.Rows[i].FindControl("ddlYes");

            DataRow[] drApply = dtNumberYr.Select("ISLink='1' AND ParamNum='" + (i + 1).ToString() + "'");
            if (drApply.Length > 0)
            {
                foreach (DataRow dRow in drApply)
                    if (dRow["Value"].ToString().Trim() != "")
                    {
                        txtActualValue1.Text = dRow["Value"].ToString();
                    }
            }

            if (ddlYes.Visible)
            {
                CreditScoreYr.Rows[i]["Actual_Value"] = ddlYes.SelectedValue;
            }
            else
            {
                CreditScoreYr.Rows[i]["Actual_Value"] = txtActualValue1.Text;
            }
            CreditScoreYr.Rows[i]["Actual_Score"] = txtActualScore1.Text;
        }

        DataTable CreditScore = (DataTable)ViewState["CreditScore"];

        DataTable dtExistsCS = CreditScoreYr.Clone();
        DataRow[] drExistsCS = CreditScore.Select("Year<>'" + rbtYears.SelectedValue + "'");
        drExistsCS.CopyToDataTable<DataRow>(dtExistsCS, LoadOption.OverwriteChanges);

        dtExistsCS.Merge(CreditScoreYr);
        ViewState["CreditScore"] = dtExistsCS;
    }

    protected void chkApplyOptional_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dtNumberYr = (DataTable)ViewState["dtNumberYr"];

        int intRowIndex = Utility.FunPubGetGridRowID("grvNumbers", ((CheckBox)sender).ClientID);

        CheckBox Cur_chkApplyOptional = (CheckBox)sender;

        if ((Cur_chkApplyOptional).Checked)
        {
            foreach (GridViewRow gRow in grvNumbers.Rows)
            {
                CheckBox chkApplyOptional = (CheckBox)gRow.FindControl("chkApplyOptional");
                if (chkApplyOptional.GetFormulaRow() == Cur_chkApplyOptional.GetFormulaRow() && chkApplyOptional != Cur_chkApplyOptional)
                {
                    chkApplyOptional.Checked = false;
                }
            }
        }

        TextBox txtValue = (TextBox)grvNumbers.Rows[intRowIndex].FindControl("txtValue");
        txtValue_TextChanged(txtValue, null);
    }

}

public static partial class Extentions
{
    public static void SetFormulaRow(this CheckBox chkBox, int intRow)
    {
        chkBox.Attributes.Add("FmlRow", intRow.ToString());
    }

    public static int GetFormulaRow(this CheckBox chkBox)
    {
        return Convert.ToInt32(chkBox.Attributes["FmlRow"]);
    }
}
