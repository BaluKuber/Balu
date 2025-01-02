#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS  LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Orgination
/// Screen Name         :   Credit Parameter Approval - Customer/Enquiry Mode
/// Created By          :   S.Kannan
/// Created Date        :   22-June-2010
/// Purpose             :   To approve the credit parameters
/// Last Updated By		:   Rajendran
/// Last Updated Date   :   1/3/2011
/// Reason              :   
/// Last Updated By		:   Thalaiselvam N
/// Last Updated Date   :   03-Sep-2011
/// Reason              :   Encrypted Password Validation 
/// <Program Summary>
#endregion


#region NameSpaces
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Collections.Generic;
using S3GBusEntity.Origination;
using System.Web.Security;
using System.Drawing;
using System.Text;
using System.Web.UI.HtmlControls;
#endregion

public partial class Origination_S3GOrgCreditParameterApproval_EN_Add : ApplyThemeForProject
{

    CreditMgtServicesReference.CreditMgtServicesClient ObjMgtCreditMgtClient;
    CreditMgtServices.S3G_ORG_GetCreditParameterApproval_EnquiryDataTable _ObjCreditParameterApprovalDataTable = new CreditMgtServices.S3G_ORG_GetCreditParameterApproval_EnquiryDataTable(); // datatable
    CreditMgtServices.S3G_ORG_CustomerMasterDataTable _ObjCustomerDataTable = new CreditMgtServices.S3G_ORG_CustomerMasterDataTable(); // datatable

    #region Local Identifiers

    // Values from previous page   
    private int intCount = 0;
    string _ModeTransLander = string.Empty;
    private bool isAlreadySaved;

    // to get the user details
    UserInfo userInfo = new UserInfo();

    // for Required Amount calculation   
    decimal _RevisedLimitForTheCustomer;
    string _Remark;
    string _Mode = string.Empty;
    double _ActualCPTHygenie;

    // Validation 
    bool IsUserAlreadyEdited;
    bool IsApproved;

    // password verification
    bool IsCorrectPassword;

    // score board
    DataTable dtScoreBoard;

    int Credit_Parameter_Approval_ID;

    Dictionary<string, string> Procparam = null;
    StringBuilder sbConditionsXML = new StringBuilder();
    // Redirect
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgCreditParameterApproval.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgCreditParameterApproval_EN_Add.aspx';";
    string strRedirectPage = "S3GOrgCreditParameterApproval.aspx?qsMode=C";

    string SANCTIONED_AMOUNT;
    DataTable dt = new DataTable();

    public static DataTable dtLOB = new DataTable();
    public static DataTable dtGroup = new DataTable();

    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        //S3GSession ObjS3GSession = new S3GSession();
        //DateFormate = ObjS3GSession.ProDateFormatRW;
        ProgramCode = "051";
        GetValuesFromQueryString();

        string creditParameterNumber = string.Empty;
        if (PageMode == PageModes.WorkFlow)
        {
            PreparePageForWFLoad();
        }


        if (!IsPostBack)
        {
            FunLoadLOBMaster();
            IsApproved = IsUserAlreadyEdited = IsCorrectPassword = false;
            FunInitPage();
            DisplayScoreBoardGrid();
            FunGetCustomerGridSource();
            CreateDisplayGrid();
            TPABEN.Visible = true;
            TPScoreBoard.Visible = true;
            TBCusMode.Visible = true;
            SetCPAType();
        }
        if (CreditParamTransID > 0)
        {
            FunAssignCrditParamTranID_PopUp();//added ramesh 6/1/2011
        }
        ReqID.Attributes.Add("OnClick", "window.open('" + hdnID.Value + "','newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')");
        ReqID2.Attributes.Add("OnClick", "window.open('" + hdnID.Value + "','newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')");
        if ((!(string.IsNullOrEmpty(_ModeTransLander))) && (string.Compare(_ModeTransLander, "Q") == 0))
        {
            btnSave.Enabled = false;
            btnCusSave.Enabled = false;
            gvEnquiryByCustomer.Columns[7].Visible = false;
            if (gvEnquiryByCustomer.FooterRow != null)
            {
                gvEnquiryByCustomer.FooterRow.Visible = false;
            }
        }
        FunPubSetIndex(1); // to select the orgination accordion
    }

    private void PreparePageForWFLoad()
    {
        DataTable WFCPALoad;
        WorkFlowSession WFSessionValues = new WorkFlowSession();
        if (ViewState["WFCPALoad"] == null)
        {
            //WFSessionValues = new WorkFlowSession();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@EnquiryNumber", WFSessionValues.LastDocumentNo);
            Procparam.Add("@Company_ID", CompanyId);
            Procparam.Add("@Document_Type", WFSessionValues.Document_Type.ToString());
            WFCPALoad = Utility.GetDefaultData("S3G_GEN_GetCPAForWF", Procparam);
            ViewState["WFCPALoad"] = WFCPALoad;
        }
        else
            WFCPALoad = (DataTable)ViewState["WFCPALoad"];

        if (WFCPALoad != null && WFCPALoad.Rows.Count > 0)
        {
            DataRow WFRecord = WFCPALoad.Rows[0];
            _EnqNumber = WFRecord["Doc_No"].ToString();
            WFSessionValues.WorkFlowDocumentNo = _EnqNumber;
            _CustId = int.Parse(WFRecord["Customer_ID"].ToString());
            _CompanyId = int.Parse(WFRecord["Company_Id"].ToString());
            _LOBId = int.Parse(WFRecord["Lob_Id"].ToString());
            _ProductId = int.Parse(WFRecord["Product_Id"].ToString());
            _LocationId = int.Parse(WFRecord["Location_Id"].ToString());
            CreditParamTransID = int.Parse(WFRecord["CreditParamTrans_ID"].ToString());
            _Mode = "W";
            _NumOfApproved = int.Parse(WFRecord["NumOfApproved"].ToString());
            _ModeTransLander = "W";
        }
    }

    private void SetCPAType()
    {

        if ((string.Compare(_Mode, "Cus")) == 0)
        {
            lblHeading.Text = "Approval Based on Customer Code";
            tcCPA.Tabs[0].Visible = false;
            tcCPA.ActiveTabIndex = 1;
            txtCusApprovalFor.Text = "Customer";

            //FunProInitializeFacilityGroup();

            if ((string.Compare(_Mode, "Cus")) == 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@CreditParamTransID", CreditParamTransID.ToString());
                DataSet dSet = Utility.GetDataset("S3G_ORG_GetCustomer_FacilityForApproval", Procparam);

                if (dSet != null && dSet.Tables.Count > 0)
                {
                    if (_ModeTransLander != "Q" && string.IsNullOrEmpty(dSet.Tables[0].Rows[0]["Final_Groupt_Sanctioned"].ToString()))
                    {
                        for (int i = 0; i <= dSet.Tables[0].Rows.Count - 1; i++)
                        {
                            dSet.Tables[0].Rows[i]["Final_Groupt_Sanctioned"] = dSet.Tables[0].Rows[i]["Sanctioned_Group_Limit"] = Math.Round(FunPriGetSanctionAmount(dSet.Tables[0].Rows[i]["Total_Amount"].ToString(), null), 0);
                        }

                        for (int i = 0; i <= dSet.Tables[1].Rows.Count - 1; i++)
                        {
                            dSet.Tables[1].Rows[i]["Final_Sanctioned_Limit"] = dSet.Tables[1].Rows[i]["Sanctioned_limit"] = Math.Round(FunPriGetSanctionAmount(dSet.Tables[1].Rows[i]["Facility_Amount"].ToString(), null), 0);
                        }
                    }
                    ViewState["dtFacilityGroup"] = dSet.Tables[0];
                    ViewState["dtFacilityGrid"] = dSet.Tables[1];

                    grvFacilityGroup.DataSource = dSet.Tables[0];
                    grvFacilityGroup.DataBind();

                    if (dSet.Tables[2].Rows.Count > 0)
                    {
                        grvRemarks.DataSource = dSet.Tables[2];
                        grvRemarks.DataBind();
                    }
                    else
                    {
                        imgRemarksHistory.Style.Add("title", "No History Available");
                    }

                    if (PageMode == PageModes.Query)
                    {
                        txtRemark.Visible = false;
                    }
                }
            }
        }
        else
        {
            lblHeading.Text = "Approval Based on Enquiry Number";
            TPABEN.Visible = true;
            TBCusMode.Visible = false;
        }
    }

    private void FunAssignCrditParamTranID_PopUp()
    {
        if (CreditParamTransID > 0)//added ramesh 6/1/2011
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(CreditParamTransID.ToString(), false, 0);
            hdnID.Value = "../Origination/S3G_ORG_CreditParameterTransaction.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

        }
    }

    #endregion

    private void GetValuesFromQueryString()
    {
        if (Request.QueryString["qsEnqNo"] != null)
        {
            FormsAuthenticationTicket TicketEnqno = FormsAuthentication.Decrypt(Request.QueryString.Get("qsEnqNo"));
            if (!(string.IsNullOrEmpty(TicketEnqno.Name)))
                _EnqNumber = Convert.ToString(TicketEnqno.Name);
        }
        if (Request.QueryString["CustId"] != null)
        {
            FormsAuthenticationTicket TicketCustId = FormsAuthentication.Decrypt(Request.QueryString.Get("CustId"));
            if (TicketCustId.Name != "")
            {
                _CustId = Convert.ToInt32(TicketCustId.Name);
            }
        }

        if (Request.QueryString["CompanyId"] != null)
        {
            FormsAuthenticationTicket TicketCompanyId = FormsAuthentication.Decrypt(Request.QueryString.Get("CompanyId"));
            _CompanyId = Convert.ToInt32(TicketCompanyId.Name);
        }
        if (Request.QueryString["LOBId"] != null)
        {
            FormsAuthenticationTicket TicketLOBId = FormsAuthentication.Decrypt(Request.QueryString.Get("LOBId"));
            _LOBId = TicketLOBId.Name == "" ? 0 : Convert.ToInt32(TicketLOBId.Name);
        }
        if (Request.QueryString["ProductId"] != null)
        {
            FormsAuthenticationTicket TicketProductId = FormsAuthentication.Decrypt(Request.QueryString.Get("ProductId"));
            if (!(string.IsNullOrEmpty(TicketProductId.Name)))
                _ProductId = Convert.ToInt32(TicketProductId.Name);
        }
        if (Request.QueryString["ConstitutionId"] != null)
        {
            FormsAuthenticationTicket TicketConstitutionId = FormsAuthentication.Decrypt(Request.QueryString.Get("ConstitutionId"));
            _ConstitutionId = TicketConstitutionId.Name == "" ? 0 : Convert.ToInt32(TicketConstitutionId.Name);
        }
        if (Request.QueryString["LocationID"] != null)
        {
            FormsAuthenticationTicket TicketLocationId = FormsAuthentication.Decrypt(Request.QueryString.Get("LocationID"));
            if (!(string.IsNullOrEmpty(TicketLocationId.Name)))
                _LocationId = Convert.ToInt32(TicketLocationId.Name);
            else
                _LocationId = -1;
        }


        if (Request.QueryString["CreditParamTransId"] != null)
        {
            FormsAuthenticationTicket TicketCreditParamTransId = FormsAuthentication.Decrypt(Request.QueryString.Get("CreditParamTransId"));
            CreditParamTransID = Convert.ToInt32(TicketCreditParamTransId.Name);
        }

        if (Request.QueryString["Mode"] != null)
        {
            FormsAuthenticationTicket TicketMode = FormsAuthentication.Decrypt(Request.QueryString.Get("Mode"));
            _Mode = TicketMode.Name;
        }


        if (Request.QueryString["NumOfApproved"] != null)
        {
            FormsAuthenticationTicket TicketNumOfApproved = FormsAuthentication.Decrypt(Request.QueryString.Get("NumOfApproved"));
            _NumOfApproved = Convert.ToInt32(TicketNumOfApproved.Name);
            txtReqApprovalDone.Text = _NumOfApproved.ToString();
        }

        if (Request.QueryString["qsMode"] != null)
        {
            //FormsAuthenticationTicket TicketNumOfApproved = FormsAuthentication.Decrypt(Request.QueryString.Get("NumOfApproved"));
            _ModeTransLander = Request.QueryString["qsMode"].ToString();
        }

    }
    public int _CompanyId { get; set; }
    public int _LOBId { get; set; }
    public int _ConstitutionId { get; set; }
    public int _ProductId { get; set; }
    public int CreditParamTransID { get; set; }
    public string _EnqNumber { get; set; }
    public int _LocationId { get; set; }
    public int _CustId { get; set; }
    public int _NumOfApproved { get; set; }
    public int _NumOfApprovals { get; set; }

    private void FunInitPage()
    {
        // this is to get the sanction number and the sanction date - if already generated.
        if (Procparam == null)
            Procparam = new Dictionary<string, string>();
        else
            Procparam.Clear();
        Procparam.Add("@CreditParamTransId", CreditParamTransID.ToString());
        Procparam.Add("@Company_Id", CompanyId.ToString());
        DataSet CreditParameterData = Utility.GetDataset(SPNames.S3G_ORG_GetSanctionDetails, Procparam);
        DataTable dt = CreditParameterData.Tables[0];
        if (dt != null && dt.Rows.Count > 0 && (!(string.IsNullOrEmpty(dt.Rows[0]["Santion_Number"].ToString()))))
        {
            txtCusSanctionNumber.Text = txtSanctionNumber.Text = dt.Rows[0]["Santion_Number"].ToString();
            txtCusSanctionNumber.BackColor = Color.LightGray;
            txtCusSanctionDate.Text = txtSanctionDate.Text = Convert.ToDateTime(dt.Rows[0]["Sanction_Date"].ToString()).ToString(DateFormate);
            txtCusSanctionDate.BackColor = Color.LightGray;
        }

        //DataSet CreditParameterData = GetCreditParameterDataOnPageLoad();

        //dtFillUI = GetCreditParameterDataOnPageLoad(); // FunGetDataFromDB();  // to get the enquiry details - and also the number of approvals.

        if (CreditParameterData != null) // Customer Wise
        {
            if (CreditParameterData.Tables[3] != null && CreditParameterData.Tables[3].Rows.Count > 0) // Load Customer Data
            {
                DataRow CustomerRow = CreditParameterData.Tables[3].Rows[0];
                txtCustomerCode.Text = txtCusCustomerCode.Text = CustomerRow["Customer_Code"].ToString();
                txtCustomerName.Text = txtCusCustomerName.Text = CustomerRow["CustomerName"].ToString();
                ViewState["RequiredFacility"] = RequiredFacility();
            }
            if (CreditParameterData.Tables[2] != null && CreditParameterData.Tables[2].Rows.Count > 0)
            {
                DataRow EnquiryRow = CreditParameterData.Tables[2].Rows[0];

                txtEnquiryNumber.Text = EnquiryRow["Doc_No"].ToString();
                if (!(string.IsNullOrEmpty(EnquiryRow["Doc_Date"].ToString())))
                    txtEnquiryDate.Text = Convert.ToDateTime(Utility.StringToDate(EnquiryRow["Doc_Date"].ToString())).ToString(DateFormate);

                if (!(string.IsNullOrEmpty(EnquiryRow["Product_Code"].ToString())))
                    txtProduct.Text = EnquiryRow["Product_Code"].ToString() + " - " + EnquiryRow["Product_Name"].ToString();


                if (!(string.IsNullOrEmpty(EnquiryRow["Finance_Amount_Sought"].ToString())))
                {
                    txtRequFinAmt.Text = EnquiryRow["Finance_Amount_Sought"].ToString();
                    ViewState["RequiredFacility"] = EnquiryRow["Finance_Amount_Sought"].ToString();
                }

                txtAssetAmount.Text = FunPriGetAssetAmount(CreditParameterData.Tables[2]);
                txtAssetDetails.Text = FunPriGetAssetDetails(CreditParameterData.Tables[2]);
                FunPriGetAssetGuideLineValue(CreditParameterData.Tables[2]);
            }

            if (CreditParameterData.Tables[3] != null && CreditParameterData.Tables[3].Rows.Count > 0)
            {
                DataRow MasterRow = CreditParameterData.Tables[3].Rows[0];
                if (!(string.IsNullOrEmpty(MasterRow["LOB_Code"].ToString())))
                    txtLOB.Text = MasterRow["LOB_Code"].ToString() + " - " + MasterRow["LOB_Name"].ToString();
                if (!(string.IsNullOrEmpty(MasterRow["Location_Code"].ToString())))
                    txtBranch.Text = MasterRow["Location_Code"].ToString() + " - " + MasterRow["Location_Name"].ToString();

            }

        }
        else
        {
            strAlert = strAlert.Replace("__ALERT__", "No details found to load.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            return;
        }

        ViewState["dtFillUI"] = CreditParameterData.Tables[1];

    }
    /// <summary>
    /// Enquiry Mode
    /// </summary>
    /// <returns></returns>
    private DataSet FunGetDataFromDB() // Not in Use
    {
        try
        {
            Dictionary<string, string> dictprocparam = new Dictionary<string, string>();
            dictprocparam.Add("@Company_ID", _CompanyId.ToString());
            if (!(string.IsNullOrEmpty(_EnqNumber)))
                dictprocparam.Add("@EnquiryNo", _EnqNumber.ToString());
            else
                dictprocparam.Add("@EnquiryNo", "-1");
            dictprocparam.Add("@LOB_ID", _LOBId.ToString());
            dictprocparam.Add("@Product_ID", _ProductId.ToString());
            dictprocparam.Add("@Location_Id", _LocationId.ToString());
            dictprocparam.Add("@Constitution_ID", _ConstitutionId.ToString());
            DataSet dt_new = Utility.GetDataset("S3G_ORG_GetCreditParameterDetails", dictprocparam);
            return dt_new;
            //return _ObjCreditParameterApprovalDataTable;
        }
        finally
        {
            //ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }
    }
    DataSet GetCreditParameterDataOnPageLoad()
    {
        try
        {
            Dictionary<string, string> dictprocparam = new Dictionary<string, string>();
            dictprocparam.Add("@CreditParamTrans_ID", CreditParamTransID.ToString());
            dictprocparam.Add("@Company_ID", _CompanyId.ToString());
            return Utility.GetDataset("S3G_ORG_GetCreditParameterDetails", dictprocparam);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    /// <summary>
    /// Customer Mode
    /// </summary>
    /// <returns></returns>
    private DataTable FunGetDataFromDB_CustomerMode()
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            CreditMgtServices.S3G_ORG_CustomerMasterRow ObjCustomerRow;
            CreditMgtServices.S3G_ORG_CustomerMasterDataTable ObjCustomerDataTable = new CreditMgtServices.S3G_ORG_CustomerMasterDataTable();
            ObjCustomerRow = ObjCustomerDataTable.NewS3G_ORG_CustomerMasterRow();
            ObjCustomerRow.Customer_ID = _CustId;
            ObjCustomerDataTable.AddS3G_ORG_CustomerMasterRow(ObjCustomerRow);
            SerializationMode SerMode = new SerializationMode();
            if (Procparam == null)
                Procparam = new Dictionary<string, string>();
            else
                Procparam.Clear();
            Procparam.Add("@Customer_ID", _CustId.ToString());
            byte[] bytesCustomer = ObjMgtCreditMgtClient.FunPubQueryCustomerDetailsById(SerMode, ClsPubSerialize.Serialize(ObjCustomerDataTable, SerMode));
            _ObjCustomerDataTable = (CreditMgtServices.S3G_ORG_CustomerMasterDataTable)ClsPubSerialize.DeSerialize(bytesCustomer, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_CustomerMasterDataTable));
            return _ObjCustomerDataTable;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }
    }
    /// <summary>
    /// to return the Asset Details
    /// </summary>
    /// <param name="dtEnquiryDetails"></param>
    /// <returns> Asset Details as String</returns>
    private string FunPriGetAssetDetails(DataTable dtEnquiryDetails)
    {
        string strAssetDetails = string.Empty;
        if ((dtEnquiryDetails != null) && (dtEnquiryDetails.Rows.Count > 0)) // if table and record exists
        {
            if (string.IsNullOrEmpty(dtEnquiryDetails.Rows[0]["EnquiryUpdationAsset_ID"].ToString()))
            {
                strAssetDetails = (dtEnquiryDetails.Rows[0]["Remarks"].ToString());

            }
            else
            {
                strAssetDetails = (dtEnquiryDetails.Rows[0]["Asset_Description"].ToString());
            }
        }
        return strAssetDetails;
    }
    private string FunPriGetAssetGuideLineValue(DataTable dtEnquiryDetails)
    {
        string strAssetGuideLineValue = string.Empty;
        if ((dtEnquiryDetails != null) && (dtEnquiryDetails.Rows.Count > 0)) // if table and record exists
        {
            if (decimal.Parse((dtEnquiryDetails.Rows[0]["GuideLine_Value"].ToString())) > 0)
            {
                txtGuideLineValue.Text = dtEnquiryDetails.Rows[0]["GuideLine_Value"].ToString();
                ViewState["GuideLineValue"] = dtEnquiryDetails.Rows[0]["GuideLine_Value"].ToString();
            }
        }
        return strAssetGuideLineValue;
    }
    /// <summary>
    /// to return the Asset Amount
    /// </summary>
    /// <param name="dtEnquiryDetails"></param>
    /// <returns> Asset Amoount as String</returns>
    private string FunPriGetAssetAmount(DataTable dtEnquiryDetails)
    {
        decimal dblAssetAmt = Convert.ToDecimal("0.000");
        if ((dtEnquiryDetails != null) && (dtEnquiryDetails.Rows.Count > 0)) // if table and record exists
        {
            if (string.IsNullOrEmpty(dtEnquiryDetails.Rows[0]["Margin_Amount"].ToString()))
            {
                if (!(string.IsNullOrEmpty(dtEnquiryDetails.Rows[0]["Facility_Amount"].ToString())))
                {
                    if (!(string.IsNullOrEmpty(dtEnquiryDetails.Rows[0]["Residual_Value"].ToString())))
                        dblAssetAmt = Convert.ToDecimal(dtEnquiryDetails.Rows[0]["Facility_Amount"]) +
                    Convert.ToDecimal(dtEnquiryDetails.Rows[0]["Residual_Value"]);
                    else
                        dblAssetAmt = Convert.ToDecimal(dtEnquiryDetails.Rows[0]["Facility_Amount"]);
                }
            }
            else
            {
                if (!(string.IsNullOrEmpty(dtEnquiryDetails.Rows[0]["Facility_Amount"].ToString())))
                {
                    if (!(string.IsNullOrEmpty(dtEnquiryDetails.Rows[0]["Margin_Amount"].ToString())))
                        dblAssetAmt = Convert.ToDecimal(dtEnquiryDetails.Rows[0]["Facility_Amount"]) +
                            Convert.ToDecimal(dtEnquiryDetails.Rows[0]["Margin_Amount"]);
                    else
                        dblAssetAmt = Convert.ToDecimal(dtEnquiryDetails.Rows[0]["Facility_Amount"]);
                }
            }
        }
        return dblAssetAmt.ToString(Funsetsuffix());
    }

    #region DisplayGrid
    private void CreateDisplayGrid()
    {
        DataTable dtCusApprDetails = DisplayCustomerDetailsGrid();
        if (dtCusApprDetails != null && dtCusApprDetails.Rows.Count > 0)
        {
            // check if this user already have edited the record. 
            for (int i_details = 0; i_details < dtCusApprDetails.Rows.Count; i_details++)
            {
                if (userInfo.ProUserIdRW == Convert.ToInt32(dtCusApprDetails.Rows[i_details]["ApproverID"].ToString()))
                {

                    /* Modified By PSK on 17/Jan/2012 -- Condition addded for Convert Boolean*/

                    IsApproved = Convert.ToBoolean((dtCusApprDetails.Rows[i_details]["ApprovalStatus"].ToString() == "1") ? true : false);

                    IsUserAlreadyEdited = true;     // user already edited the record
                    btnSave.Enabled = false;        // disabling the save button - if the user already Approved the record.
                    break;
                }
            }
        }
        // if this user can approve the credit parameter


        if (string.Compare(_ModeTransLander, "Q") != 0)
        {
            if (!IsUserAlreadyEdited)//((_NumOfApproved < int.Parse(ViewState["NumOfApprovals"].ToString())) && (!IsUserAlreadyEdited))
            {
                if (userInfo.ProUserLevelIdRW < 3)
                {
                    Utility.FunShowAlertMsg(this, "Only Level 3 and above users can approve the record", strRedirectPage);
                    return;
                }
                else
                {
                    DataRow drCusDetails = dtCusApprDetails.NewRow();
                    drCusDetails["ApproverID"] = UserId;
                    drCusDetails["ApprovalStatus"] = false;
                    drCusDetails["EmployeeName"] = userInfo.ProUserNameRW;
                    drCusDetails["ApprovalSno"] = (_NumOfApproved + 1);
                    drCusDetails["ApprovalDate"] = DateTime.Now;
                    dtCusApprDetails.Rows.Add(drCusDetails);
                }
            }
        }
        gvApprovalDetails.DataSource = dtCusApprDetails;
        gvApprovalDetails.DataBind();
        updEnqDetails.Update();
    }
    protected void gvApprovalDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label lblApprovalDate = ((Label)e.Row.FindControl("lblApprovalDate"));
        if ((lblApprovalDate != null) && (!(string.IsNullOrEmpty(lblApprovalDate.Text))))  // if date exists
        {
            lblApprovalDate.Text = DateTime.Parse(lblApprovalDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);
        }

        if (_NumOfApproved < int.Parse(ViewState["NumOfApprovals"].ToString()))   // if this user can approve the credit parameter
        {
            Label lblApproverID = (Label)e.Row.FindControl("lblApproverID");
            TextBox txtPassword = (TextBox)e.Row.FindControl("txtPassword");
            if ((lblApproverID != null && (!(string.IsNullOrEmpty(lblApproverID.Text)))) && (Convert.ToInt32(lblApproverID.Text) == userInfo.ProUserIdRW) && (!IsUserAlreadyEdited))
            {
                CheckBox chkApprove = (CheckBox)e.Row.FindControl("chkApprove");

                TextBox txtRemark = (TextBox)e.Row.FindControl("txtRemark");

                chkApprove.Enabled =
                txtPassword.Enabled = true;

                txtRemark.ReadOnly = false;
            }
            else if (txtPassword != null)
            {
                txtPassword.Attributes.Add("value", "***************");
            }
        }
    }
    private DataTable DisplayCustomerDetailsGrid()
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsRow ObjCreditParameterApprovalDetailsRow;
            CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable ObjCreditParameterApprovalDetailsDataTable = new CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable();

            ObjCreditParameterApprovalDetailsRow = ObjCreditParameterApprovalDetailsDataTable.NewS3G_ORG_CreditParameterApprovalDetailsRow();

            ObjCreditParameterApprovalDetailsDataTable.AddS3G_ORG_CreditParameterApprovalDetailsRow(ObjCreditParameterApprovalDetailsRow);
            SerializationMode SerMode = new SerializationMode();
            int temp_CreditParamTransId = CreditParamTransID;
            byte[] bytesCreditParameterTransaction = ObjMgtCreditMgtClient.FunPubQueryCreditParameterApprovalDetails_Enquiry(SerMode, ClsPubSerialize.Serialize(ObjCreditParameterApprovalDetailsDataTable, SerMode), temp_CreditParamTransId);
            return (CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterTransaction, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable));
        }
        catch (Exception e)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }
    }
    private void FunGetCustomerGridSource()
    {
        try
        {
            //////////////////
            DataTable dt_cust = new DataTable();



            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@CreditParamTrans_ID", Convert.ToString(CreditParamTransID));
            dt_cust = Utility.GetDefaultData("S3G_ORG_GetCreditParameterDetails_CutomerQuery", Procparam);

            if (!dt_cust.Columns.Contains("newRow"))
                dt_cust.Columns.Add(new DataColumn("newRow", typeof(char)));

            if ((dt_cust != null && dt_cust.Rows.Count > 0) && (!(string.IsNullOrEmpty(dt_cust.Rows[0]["Santion_Number"].ToString()))))
            {
                txtCusSanctionNumber.Text = txtSanctionNumber.Text = (dt_cust.Rows[0]["Santion_Number"].ToString());
                if (!(string.IsNullOrEmpty(dt_cust.Rows[0]["Sanction_Date"].ToString())))
                {
                    txtCusSanctionDate.Text = txtSanctionDate.Text = Convert.ToDateTime(dt_cust.Rows[0]["Sanction_Date"].ToString()).ToString(DateFormate);
                }
            }
            if (dt_cust != null && dt_cust.Rows.Count > 0)
            {
                // check if this user already have edited the record. 
                for (int i_details = 0; i_details < dt_cust.Rows.Count; i_details++)
                {
                    if (userInfo.ProUserIdRW == Convert.ToInt32(dt_cust.Rows[i_details]["EmployeeID"].ToString()))
                    {
                        IsApproved = true;
                        IsUserAlreadyEdited = true;     // user already edited the record
                        btnCusSave.Enabled = false;        // disabling the save button - if the user already Approved the record.
                        break;
                    }
                }
            }
            int i_CreateGrid = 0;
            // if this user can approve the credit parameter
            if ((_NumOfApproved < int.Parse(ViewState["NumOfApprovals"].ToString())) && (!IsUserAlreadyEdited))
            {

                if (userInfo.ProUserLevelIdRW < 3)
                {
                    Utility.FunShowAlertMsg(this, "Only Level 3 and above users can approve the record", strRedirectPage);
                    return;
                }
                else
                {
                    DataRow drCusDetails = dt_cust.NewRow();

                    drCusDetails["Required_facility"] = ViewState["RequiredFacility"]; // RequiredFacility();
                    if (SANCTIONED_AMOUNT != null)
                    {
                        drCusDetails["Sanctioned_limit"] = SANCTIONED_AMOUNT; //dtFacility.Rows[0]["RequiredFacility"];
                        drCusDetails["Final_Sanctioned_limit"] = SANCTIONED_AMOUNT; //dtFacility.Rows[0]["RequiredFacility"];
                        ViewState["FinalSanctionedlimit"] = drCusDetails["Final_Sanctioned_limit"];
                    }
                    drCusDetails["EmployeeID"] = UserId;
                    drCusDetails["User_Name"] = userInfo.ProUserNameRW;

                    drCusDetails["newRow"] = "C";  // Existing Customer approval row
                    dt_cust.Rows.Add(drCusDetails);
                }
            }
            ViewState["custApproval"] = dt_cust;
            gvEnquiryByCustomer.DataSource = dt_cust;
            gvEnquiryByCustomer.DataBind();
            //gvEnquiryByCustomerQuery.Visible = false;            
        }
        catch (Exception ex)
        {
        }
    }

    private string RequiredFacility()
    {
        // to get the required facility details
        if (Procparam == null)
            Procparam = new Dictionary<string, string>();
        else
            Procparam.Clear();
        Procparam.Add("@Customer_ID", _CustId.ToString());
        DataTable dtFacility = Utility.GetDefaultData(SPNames.S3G_ORG_GetCreditParameterTranaction_CusMode, Procparam);
        if (dtFacility != null && dtFacility.Rows.Count > 0)
        {
            ViewState["RequiredFacility"] = dtFacility.Rows[0]["Requiredfacility"].ToString();
            return dtFacility.Rows[0]["Requiredfacility"].ToString(); //dtFacility.Rows[0]["RequiredFacility"];
        }
        return null;
    }
    protected void gvApprovalDetailsByCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dtCust_Temp = new DataTable();
        if (ViewState["custApproval"] != null)
            dtCust_Temp = (DataTable)ViewState["custApproval"];

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            DropDownList ddlCusLOBF = ((DropDownList)e.Row.FindControl("ddlCusLOB1"));
            Procparam.Add("@Company_ID", CompanyId);
            Procparam.Add("@User_ID", UserId);
            Procparam.Add("@Program_ID", "51");
            //Procparam.Add("@Is_Active", "1");
            ddlCusLOBF.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            DropDownList ddlCusLOB = ((DropDownList)e.Row.FindControl("ddlCusLOB"));

            // this is to get the LOB ID
            Label lblLOBId = ((Label)e.Row.FindControl("lblLOBId"));
            Label lblLOBText = ((Label)e.Row.FindControl("lblLOBText"));
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            TextBox txtRequiredFacility = ((TextBox)e.Row.FindControl("txtCusRequiredFacility"));
            //Label lblNew = ((Label)e.Row.FindControl("lblNewRecord"));

            //if (lblNew.Text.Trim().Length > 0 && (lblNew.Text.Trim() == "C" || lblNew.Text.Trim() == "L") && (decimal.Parse(txtRequiredFacility.Text.Trim()) == 0))
            //{
            //    Procparam.Add("@Company_ID", CompanyId);
            //    Procparam.Add("@User_ID", UserId);
            //    Procparam.Add("@Is_Active", "1");
            //    Procparam.Add("@LOB_ID", Convert.ToString(_LOBId));
            //    if (CreditParamTransID != 0)
            //        Procparam.Add("@CreditParamApproval_Id", Convert.ToString(CreditParamTransID));
            //    ddlCusLOB.BindDataTable(SPNames.LOBApprovalDistinct, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //}
            //else
            //{

            if (PageMode != PageModes.Query)
            {
                Procparam.Add("@Company_ID", CompanyId);
                Procparam.Add("@User_ID", UserId);
                Procparam.Add("@Program_ID", "51");
                //Procparam.Add("@Is_Active", "1");
                ddlCusLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            }
            else
            {
                ListItem lst = new ListItem(lblLOBText.Text, ((lblLOBId.Text.Length > 0) ? lblLOBId.Text : _LOBId.ToString()));
                ddlCusLOB.Items.Add(lst);
            }

            //}

            //if (lblNew.Text.Length > 0 && (lblNew.Text.Trim() == "C" || lblNew.Text.Trim() == "L") && (decimal.Parse(txtRequiredFacility.Text.Trim()) == 0))
            //    ddlCusLOB.SelectedIndex = -1;
            //else
            ddlCusLOB.SelectedValue = (lblLOBId.Text.Length > 0) ? lblLOBId.Text : _LOBId.ToString(); //_LOBId.ToString(); //lblLOBId.Text; 

            /*
                        DropDownList ddlCusOverRide = ((DropDownList)e.Row.FindControl("ddlCusOverRide"));
                        if (dtCust_Temp.Rows.Count > intCount)
                        {
                            ddlCusOverRide.SelectedValue = dtCust_Temp.Rows[intCount]["Override"].ToString() == "Enable" ? "0" : "1";
                        }
                        intCount++;

                        TextBox txtCusFinalSanctionedAmount = ((TextBox)e.Row.FindControl("txtCusFinalSanctionedAmount"));
                        //if(ddlCusOverRide.SelectedValue=="1")
                        txtCusFinalSanctionedAmount.Enabled = false;
                        if (string.Compare(_ModeTransLander, "Q") == 0)
                        {
                            TextBox lblSanctioned_limit1 = ((TextBox)e.Row.FindControl("txtCusSanctionedLimit"));
                            DropDownList ddlCusLOB1 = ((DropDownList)e.Row.FindControl("ddlCusLOB"));
                            DropDownList ddlCusOverRide1 = ((DropDownList)e.Row.FindControl("ddlCusOverRide"));
                            //TextBox txtCusFinalSanctionedAmount = (TextBox)e.Row.FindControl("txtCusFinalSanctionedAmount");
                            txtCusPassword.Enabled = txtCusFinalSanctionedAmount.Enabled =
                            lblSanctioned_limit1.Enabled =
                                ddlCusLOB1.Enabled =
                                ddlCusOverRide1.Enabled = false;
                        }

                        Label lblCusCreditParameterApprovalID = ((Label)e.Row.FindControl("lblCusCreditParameterApprovalID"));
                        // TextBox txtCusRequiredFacility = ((TextBox)e.Row.FindControl("txtCusRequiredFacility"));
                        TextBox lblSanctioned_limit = ((TextBox)e.Row.FindControl("txtCusSanctionedLimit"));
                        Label lblEmployeeId = ((Label)e.Row.FindControl("lblEmployeeId"));
                        Label lblEmployeeName = ((Label)e.Row.FindControl("lblEmployeeName"));
                        // Get the ADD Button 
                        //TextBox txtCusFinalSanctionedAmount = ((TextBox)e.Row.FindControl("txtCusFinalSanctionedAmount"));
                        if (!(string.IsNullOrEmpty(lblCusCreditParameterApprovalID.Text))) // disable row.
                        {

                            lblSanctioned_limit.Enabled =
                            //ddlCusLOB.Enabled =
                            ddlCusOverRide.Enabled = false;
                            e.Row.Cells[7].Controls[0].Visible = false;
                        }
                        else // Enable the Validations
                        {
                            S3GSession objSesion = new S3GSession();
                            // SET TEXT BOX PRECISIONS
                            lblSanctioned_limit.SetDecimalPrefixSuffix(10, 2, true, "Sanctioned Limit");
                            txtCusFinalSanctionedAmount.SetDecimalPrefixSuffix(10, 2, true, "Sanctioned Amount");
                            txtRequiredFacility.SetDecimalPrefixSuffix(10, 0, true, "Required Facility");

                            e.Row.Cells[7].Controls[0].Visible = true;
                            //ddlCusLOB.Enabled = (decimal.Parse(txtRequiredFacility.Text.Trim()) == 0) ? true : false;
                            lblSanctioned_limit.Enabled = (decimal.Parse(txtRequiredFacility.Text.Trim()) == 0) ? true : false;
                            txtRequiredFacility.Enabled = (decimal.Parse(txtRequiredFacility.Text.Trim()) == 0) ? true : false;
                            txtCusFinalSanctionedAmount.Enabled = false;
                        //}*/
        }

    }

    private void Funpribindgrid()
    {
        DataTable dt = new DataTable();
        ViewState["custApproval"] = dt;
        gvEnquiryByCustomer.DataSource = dt;
        gvEnquiryByCustomer.DataBind();
    }
    protected void gvApprovalDetailsByCust_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {
            DataTable dt = (DataTable)ViewState["custApproval"];
            DataRow dr = dt.NewRow();
            dr["LOB_ID"] = ((DropDownList)gvEnquiryByCustomer.FooterRow.FindControl("ddlCusLOB1")).SelectedValue;
            dr["LOB"] = ((DropDownList)gvEnquiryByCustomer.FooterRow.FindControl("ddlCusLOB1")).SelectedItem.Text;
            dr["EmployeeID"] = UserId;
            dr["User_Name"] = userInfo.ProUserNameRW;
            dr["Required_facility"] = ((TextBox)gvEnquiryByCustomer.FooterRow.FindControl("txtCusRequiredFacility")).Text;
            dr["Sanctioned_limit"] = ((TextBox)gvEnquiryByCustomer.FooterRow.FindControl("txtCusSanctionedLimit")).Text;
            dr["Override"] = ((DropDownList)gvEnquiryByCustomer.FooterRow.FindControl("ddlCusOverRide")).SelectedItem.Text;
            dr["Final_Sanctioned_Limit"] = ((TextBox)gvEnquiryByCustomer.FooterRow.FindControl("txtCusFinalSanctionedAmount")).Text;
            dr["Approved_Date"] = "";
            dt.Rows.Add(dr);
            ViewState["custApproval"] = dt;
            gvEnquiryByCustomer.DataSource = dt;
            gvEnquiryByCustomer.DataBind();
        }
    }

    protected void gvApprovalDetailsByCust_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //gvEnquiryByCustomer.EditIndex = e.NewEditIndex;
        //GridViewRow gvrow = gvEnquiryByCustomer.Rows[e.NewEditIndex];
        //DropDownList ddllob1 = (DropDownList)gvrow.FindControl("ddlCusLOBedit");
        gvEnquiryByCustomer.EditIndex = e.NewEditIndex;
        DropDownList ddltype = gvEnquiryByCustomer.Rows[e.NewEditIndex].FindControl("ddlCusLOB") as DropDownList;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@User_ID", UserId);
        Procparam.Add("@Program_ID", "51");
        //Procparam.Add("@Is_Active", "1");
        ddltype.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        dt = (DataTable)ViewState["custApproval"];
        gvEnquiryByCustomer.ShowFooter = false;
        gvEnquiryByCustomer.DataSource = dt;
        gvEnquiryByCustomer.DataBind();


        DataRow dr = dt.Rows[e.NewEditIndex];

    }

    protected void gvApprovalDetailsByCust_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        dt = (DataTable)ViewState["custApproval"];
        DataRow dr = dt.Rows[e.RowIndex];
        GridViewRow gvrow = gvEnquiryByCustomer.Rows[e.RowIndex];
        dr.BeginEdit();
        dr["LOB_ID"] = ((DropDownList)gvrow.FindControl("ddlCusLOBedit")).SelectedValue;
        dr["LOB"] = ((DropDownList)gvrow.FindControl("ddlCusLOBedit")).SelectedItem.Text;
        dr["EmployeeID"] = UserId;
        dr["User_Name"] = userInfo.ProUserNameRW;
        dr["Required_facility"] = ((TextBox)gvrow.FindControl("txtCusRequiredFacility1")).Text;
        dr["Sanctioned_limit"] = ((TextBox)gvrow.FindControl("txtCusSanctionedLimit1")).Text;
        dr["Override"] = ((DropDownList)gvrow.FindControl("ddlCusOverRide1")).SelectedItem.Text;
        dr["Final_Sanctioned_Limit"] = ((TextBox)gvrow.FindControl("txtCusFinalSanctionedAmount1")).Text;
        dr["Approved_Date"] = "";
        dr.EndEdit();
        ViewState["custApproval"] = dt;
        gvEnquiryByCustomer.EditIndex = -1;
        gvEnquiryByCustomer.ShowFooter = true;
        gvEnquiryByCustomer.DataSource = dt;
        gvEnquiryByCustomer.DataBind();
    }

    protected void gvApprovalDetailsByCust_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        dt = (DataTable)ViewState["custApproval"];
        dt.Rows.RemoveAt(e.RowIndex);
        if (dt.Rows.Count == 0)
        {
            Funpribindgrid();
        }
        else
        {
            gvEnquiryByCustomer.DataSource = dt;
            gvEnquiryByCustomer.DataBind();
        }
    }

    protected void gvApprovalDetailsByCust_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvEnquiryByCustomer.EditIndex = -1;
        gvEnquiryByCustomer.ShowFooter = true;
        dt = (DataTable)ViewState["custApproval"];
        gvEnquiryByCustomer.DataSource = dt;
        gvEnquiryByCustomer.DataBind();
    }

    protected void gvEnquiryByCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtCustApproval = new DataTable();
        if (ViewState["custApproval"] != null)
            dtCustApproval = (DataTable)ViewState["custApproval"];

        if (dtCustApproval.Rows.Count > 0)
        {
            if (((Convert.ToDecimal(((TextBox)gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.SelectedIndex].FindControl("txtCusRequiredFacility")).Text.Trim())) < (Convert.ToDecimal(((TextBox)gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.SelectedIndex].FindControl("txtCusFinalSanctionedAmount")).Text.Trim()))) && (((DropDownList)gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.SelectedIndex].FindControl("ddlCusLOB")).SelectedValue == Convert.ToString(_LOBId)))
            {
                Utility.FunShowAlertMsg(this, "final sanction amount cannot be greater than the required facility");
                return;
            }
            dtCustApproval.Rows[dtCustApproval.Rows.Count - 1]["Final_Sanctioned_limit"] = (gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.SelectedIndex].FindControl("txtCusFinalSanctionedAmount") != null) ? ((TextBox)gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.SelectedIndex].FindControl("txtCusFinalSanctionedAmount")).Text.Trim() : "";
            dtCustApproval.Rows[dtCustApproval.Rows.Count - 1]["Override"] = (((DropDownList)gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.SelectedIndex].FindControl("ddlCusOverRide")).SelectedValue.Trim() == "0") ? "Enable" : "Disable";

            DataRow drCusDetails = dtCustApproval.NewRow();
            drCusDetails.BeginEdit();
            drCusDetails["EmployeeID"] = UserId;
            drCusDetails["User_Name"] = userInfo.ProUserNameRW;
            drCusDetails["Required_facility"] = 0;
            drCusDetails["newRow"] = "L";  // New Loan Row
            drCusDetails.EndEdit();
            dtCustApproval.Rows.Add(drCusDetails);

            // Get Previously Entered Data and Update it to Datatable

            gvEnquiryByCustomer.DataSource = dtCustApproval;
            gvEnquiryByCustomer.DataBind();
            ViewState["custApproval"] = dtCustApproval;
            //Hide the Current Row and New Row's Add Button's
            gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.SelectedIndex].Cells[7].Controls[0].Visible = false;
            gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.SelectedIndex + 1].Cells[7].Controls[0].Visible = false;
        }

    }

    private string FunPriStatusXML()
    {
        try
        {
            sbConditionsXML = new StringBuilder();
            DataTable dtCustApproval = new DataTable();
            dtCustApproval = (DataTable)ViewState["custApproval"];
            sbConditionsXML.Append("<Root>");
            for (int i = 0; i < dtCustApproval.Rows.Count; i++)
            {
                sbConditionsXML.Append("<Details");
                sbConditionsXML.Append(" Final_Sanctioned_Limit = '" + dtCustApproval.Rows[i]["Final_Sanctioned_Limit"] + "'");
                sbConditionsXML.Append(" Override = '" + dtCustApproval.Rows[i]["Override"] + "'");
                sbConditionsXML.Append(" LOB_ID = '" + dtCustApproval.Rows[i]["LOB_ID"] + "'");
                sbConditionsXML.Append(" EmployeeID = '" + dtCustApproval.Rows[i]["EmployeeID"] + "'");
                sbConditionsXML.Append(" User_Name = '" + dtCustApproval.Rows[i]["User_Name"] + "'");
                sbConditionsXML.Append(" Required_facility = '" + dtCustApproval.Rows[i]["Required_facility"] + "'");
                sbConditionsXML.Append(" Sanctioned_limit = '" + dtCustApproval.Rows[i]["Sanctioned_limit"] + "'");
                sbConditionsXML.Append(" Approved_Date = '" + dtCustApproval.Rows[i]["Approved_Date"] + "'");
                sbConditionsXML.Append(" newRow = '" + dtCustApproval.Rows[i]["newRow"] + "'");
                sbConditionsXML.Append(" />");
            }
            sbConditionsXML.Append("</Root>");

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return sbConditionsXML.ToString();
    }

    private int FunCreateCreditParameterCustomerApprovalDetails()
    {
        CreditMgtServicesReference.CreditMgtServicesClient objCPA = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            int errcode = 0;
            decimal decTotal = 0;

            DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];
            DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];

            if (grvFacilityGroup.Rows.Count > 0)
            {
                
                for (int i = 0; i <= dtFacilityGroup.Rows.Count - 1; i++)
                {
                    if (dtFacilityGrid.CheckRows("Group_ID='" + dtFacilityGroup.Rows[i]["Group_ID"].ToString() + "'", 0))
                    {
                        Utility.FunShowAlertMsg(this, "Group : " + dtFacilityGroup.Rows[i]["Group_Text"].ToString() + " having no records");
                        return 25;
                    }
                    else
                    {
                        string strTotal = dtFacilityGroup.Rows[i]["Total_Amount"].ToString();
                        dtFacilityGrid.UpdateRows("Group_ID='" + dtFacilityGroup.Rows[i]["Group_ID"].ToString() + "'", "Total_Amount", strTotal.ToString());

                        strTotal = dtFacilityGroup.Rows[i]["Sanctioned_Group_Limit"].ToString();
                        dtFacilityGrid.UpdateRows("Group_ID='" + dtFacilityGroup.Rows[i]["Group_ID"].ToString() + "'", "Sanctioned_Group_Limit", strTotal.ToString());

                        strTotal = dtFacilityGroup.Rows[i]["Final_Groupt_Sanctioned"].ToString();
                        dtFacilityGrid.UpdateRows("Group_ID='" + dtFacilityGroup.Rows[i]["Group_ID"].ToString() + "'", "Final_Groupt_Sanctioned", strTotal.ToString());
                    }
                }

                decTotal = (decimal)dtFacilityGroup.Compute("SUM(Total_Amount)", "1=1");
                //objS3G_EnquiryCustomerRow.Facility_Amount = Convert.ToDouble(decTotal);
                //ObjS3G_ORG_CPACustomerDetailsRow.XML_Credit = dtFacilityGrid.FunPubFormXml();
            }

            errcode = FunCreateCreditParameterApproval(decTotal.ToString());

            if (errcode == 0)
            {
                CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsDataTable ObjS3G_ORG_CPACustomerDetailsTable = new CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsDataTable();
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsRow ObjS3G_ORG_CPACustomerDetailsRow = ObjS3G_ORG_CPACustomerDetailsTable.NewS3G_ORG_CreditParameterApprovalCusDetailsRow();

                ObjS3G_ORG_CPACustomerDetailsRow.Credit_Parameter_Approval_ID = Credit_Parameter_Approval_ID;
                ObjS3G_ORG_CPACustomerDetailsRow.LOB_ID = 0;
                ObjS3G_ORG_CPACustomerDetailsRow.Required_facility = 0;
                ObjS3G_ORG_CPACustomerDetailsRow.Sanctioned_limit = 0;
                ObjS3G_ORG_CPACustomerDetailsRow.Override = "0";
                ObjS3G_ORG_CPACustomerDetailsRow.Offer_Card = txtRemark.Text;
                ObjS3G_ORG_CPACustomerDetailsRow.Employee_ID = userInfo.ProUserIdRW;

                ObjS3G_ORG_CPACustomerDetailsRow.Final_Sanctioned_Limit = decTotal;
                ObjS3G_ORG_CPACustomerDetailsRow.Approved_Date = DateTime.Now;
                ObjS3G_ORG_CPACustomerDetailsRow.XML_Credit = dtFacilityGrid.FunPubFormXml();
                
                ObjS3G_ORG_CPACustomerDetailsTable.AddS3G_ORG_CreditParameterApprovalCusDetailsRow(ObjS3G_ORG_CPACustomerDetailsRow);
                SerializationMode SMode = SerializationMode.Binary;

                int isFinalApprovalRow = 0;

                if ((_NumOfApproved + 1) == int.Parse(ViewState["NumOfApprovals"].ToString())) // if it is a last approval, then create Sanction Number
                {
                    isFinalApprovalRow = 1;
                }
                errcode = objCPA.FunPubCreateCreditParameterApprovalCustomerDetails(SMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CPACustomerDetailsTable, SMode), isFinalApprovalRow);

            }


            //foreach (GridViewRow gvApprovalDetailsRow in gvEnquiryByCustomer.Rows)
            //{
            //    DropDownList ddlCusLOB = ((DropDownList)gvApprovalDetailsRow.FindControl("ddlCusLOB"));
            //    Label lblNewRecord = ((Label)gvApprovalDetailsRow.FindControl("lblNewRecord"));
            //    if (lblNewRecord.Text.Length > 0 && (lblNewRecord.Text.Trim() == "C" || lblNewRecord.Text.Trim() == "L"))
            //    {
            //        TextBox txtCusRequiredFacility = ((TextBox)gvApprovalDetailsRow.FindControl("txtCusRequiredFacility"));
            //        TextBox lblCusSanctionedLimit = ((TextBox)gvApprovalDetailsRow.FindControl("txtCusSanctionedLimit"));
            //        DropDownList ddlCusOverRide = ((DropDownList)gvApprovalDetailsRow.FindControl("ddlCusOverRide"));
            //        Label lblCusOfferCard = ((Label)gvApprovalDetailsRow.FindControl("lblCusOfferCard"));

            //        if (ddlCusLOB.SelectedValue == "0")
            //        {
            //            Utility.FunShowAlertMsg(this, "Select Line of Business");
            //            ddlCusLOB.Focus();
            //            return 20;
            //        }
            //        else if (txtCusRequiredFacility.Text.Trim() == "0")
            //        {
            //            Utility.FunShowAlertMsg(this, "Enter the Required Facility Amount ");
            //            txtCusRequiredFacility.Focus();
            //            return 20;
            //        }

            //        TextBox txtCusFinalSanctionedAmount = ((TextBox)gvApprovalDetailsRow.FindControl("txtCusFinalSanctionedAmount"));

            //        if (ddlCusLOB.SelectedValue == Convert.ToString(_LOBId))
            //        {
            //            if (txtCusFinalSanctionedAmount.Text.Trim().Length > 0)
            //            {
            //                decimal SanctionAmount;
            //                if (!decimal.TryParse(txtCusFinalSanctionedAmount.Text.Trim(), NumberStyles.AllowDecimalPoint, System.Threading.Thread.CurrentThread.CurrentCulture, out SanctionAmount))
            //                    return 18;
            //            }
            //            else
            //                return 14;
            //        }
            //        else
            //            txtCusFinalSanctionedAmount.Text = lblCusSanctionedLimit.Text;


            //        if (((Convert.ToDecimal(txtCusRequiredFacility.Text)) < (Convert.ToDecimal(txtCusFinalSanctionedAmount.Text.Trim() == "" ? "0" : txtCusFinalSanctionedAmount.Text))) && (ddlCusLOB.SelectedValue == Convert.ToString(_LOBId)))
            //            return 13; // final sanction amt cannot be greater than the required facility

            //        if (lblCusSanctionedLimit.Text.ToString().Trim().Equals(""))
            //        {
            //            return 15;
            //        }

            //        Label lblNew = ((Label)gvApprovalDetailsRow.FindControl("lblNewRecord"));
            //        Label lblLOBId = ((Label)gvApprovalDetailsRow.FindControl("lblLOBId"));
            //        if (lblNew.Text.Length > 0 && (lblNew.Text.Trim() == "C" || lblNew.Text.Trim() == "L") && (_LOBId == int.Parse(ddlCusLOB.SelectedValue)))
            //        {
            //            errcode = FunCreateCreditParameterApproval(txtCusFinalSanctionedAmount.Text);
            //        }

            //        // this is to  insert/update the CreditParameter Approval

            //        if (errcode == 12)
            //        {
            //            Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoNotDefined);
            //            break;
            //        }
            //        else if (errcode == 21)
            //        {
            //            Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoExceeds);
            //            break;
            //        }

            //        else if (errcode == 3)
            //        {
            //            Utility.FunShowAlertMsg(this, "Approval should be in sequence as defined in Rule Card");
            //            break;
            //        }
            //        else if (errcode == 4)
            //        {
            //            Utility.FunShowAlertMsg(this, "Approval Already done for this application number");
            //            break;
            //        }
            //        else if (errcode == 5)
            //        {
            //            Utility.FunShowAlertMsg(this, "Approver not having access to approve");
            //            break;
            //        }
            //        else
            //        {
            //            //try
            //            //{
            //            // this is to insert the CreditParameterApprovalCustomerDetails
            //            CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsDataTable ObjS3G_ORG_CPACustomerDetailsTable = new CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsDataTable();
            //            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsRow ObjS3G_ORG_CPACustomerDetailsRow = ObjS3G_ORG_CPACustomerDetailsTable.NewS3G_ORG_CreditParameterApprovalCusDetailsRow();

            //            ObjS3G_ORG_CPACustomerDetailsRow.Credit_Parameter_Approval_ID = Credit_Parameter_Approval_ID;
            //            ObjS3G_ORG_CPACustomerDetailsRow.LOB_ID = Convert.ToInt32(ddlCusLOB.SelectedValue);
            //            ObjS3G_ORG_CPACustomerDetailsRow.Required_facility = Convert.ToInt64(txtCusRequiredFacility.Text);
            //            ObjS3G_ORG_CPACustomerDetailsRow.Sanctioned_limit = Convert.ToDecimal(lblCusSanctionedLimit.Text.Trim() == "" ? "0" : lblCusSanctionedLimit.Text);
            //            ObjS3G_ORG_CPACustomerDetailsRow.Override = ddlCusOverRide.SelectedItem.ToString();
            //            ObjS3G_ORG_CPACustomerDetailsRow.Offer_Card = lblCusOfferCard.Text;
            //            ObjS3G_ORG_CPACustomerDetailsRow.Employee_ID = userInfo.ProUserIdRW;

            //            ObjS3G_ORG_CPACustomerDetailsRow.Final_Sanctioned_Limit = Convert.ToDecimal(ViewState["FinalSanctionedlimit"].ToString());
            //            ObjS3G_ORG_CPACustomerDetailsRow.Approved_Date = DateTime.Now;
            //            //ObjS3G_ORG_CPACustomerDetailsRow.XML_Credit = FunPriStatusXML();

                        

            //            ObjS3G_ORG_CPACustomerDetailsTable.AddS3G_ORG_CreditParameterApprovalCusDetailsRow(ObjS3G_ORG_CPACustomerDetailsRow);
            //            SerializationMode SMode = SerializationMode.Binary;

            //            int isFinalApprovalRow = 0;
            //            if (lblNewRecord.Text.Trim() == "C")
            //            {
            //                if ((_NumOfApproved + 1) == int.Parse(ViewState["NumOfApprovals"].ToString())) // if it is a last approval, then create Sanction Number
            //                {
            //                    isFinalApprovalRow = 1;
            //                }
            //            }
            //            errcode = objCPA.FunPubCreateCreditParameterApprovalCustomerDetails(SMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CPACustomerDetailsTable, SMode), isFinalApprovalRow);
            //            //break;
            //            //}
            //            //catch (Exception ex)
            //            //{
            //            //    Utility.FunShowAlertMsg(this, "Unable to save data due to data problem!");
            //            //    break;
            //            //}
            //        }
            //    }
            //}
            return errcode;
        }
        catch (Exception e)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        finally
        {
            objCPA.Close();
        }
    }
    protected void SaveCus_Click(object sender, EventArgs e)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {
            if (txtCusPassword.Text.Trim().Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Enter password");
                txtCusPassword.Focus();
                txtCusPassword.BackColor = Color.LightGray;
                return;
            }
            if (ObjS3GAdminServices.FunPubPasswordValidation(userInfo.ProUserIdRW, txtCusPassword.Text.Trim()) > 0)
            {
                IsCorrectPassword = false;
                Utility.FunShowAlertMsg(this, "Password mismatch, Enter correct password");
                txtCusPassword.Focus();
                return;
            }

            //if (gvEnquiryByCustomer.Rows.Count > 1)
            //{
            //    GridViewRow LastRow = gvEnquiryByCustomer.Rows[gvEnquiryByCustomer.Rows.Count - 1];
            //    Label lblNewRecord = ((Label)LastRow.FindControl("lblNewRecord"));
            //    if (lblNewRecord.Text.Length > 0 && (lblNewRecord.Text.Trim() == "C" || lblNewRecord.Text.Trim() == "L"))
            //    {
            //        TextBox txtCusRequiredFacility = ((TextBox)LastRow.FindControl("txtCusRequiredFacility"));
            //        TextBox lblCusSanctionedLimit = ((TextBox)LastRow.FindControl("txtCusSanctionedLimit"));
            //        DropDownList ddlCusOverRide = ((DropDownList)LastRow.FindControl("ddlCusOverRide"));
            //        Label lblCusOfferCard = ((Label)LastRow.FindControl("lblCusOfferCard"));
            //        DropDownList ddlCusLOB = ((DropDownList)LastRow.FindControl("ddlCusLOB"));

            //        if (ddlCusLOB.SelectedValue == "0")
            //        {
            //            Utility.FunShowAlertMsg(this, "Select Line of Business");
            //            ddlCusLOB.Focus();
            //            return;
            //        }
            //        else if (txtCusRequiredFacility.Text.Trim() == "" || Convert.ToInt64(txtCusRequiredFacility.Text) == 0)
            //        {
            //            Utility.FunShowAlertMsg(this, "Enter the Required Facility");
            //            txtCusRequiredFacility.Focus();
            //            return;
            //        }
            //    }
            //}

            int errcode = FunCreateCreditParameterCustomerApprovalDetails();
            if (errcode == 14)
            {
                Utility.FunShowAlertMsg(this, "Cannot create Sanction Number - enter the Final Sanctioned Amount");
                return;
            }
            if (errcode == 15)
            {
                Utility.FunShowAlertMsg(this, "Cannot create Sanction Number - enter the Final Sanctioned Limit");
                return;
            }
            if (errcode == 12)
            {
                Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoNotDefined);
                return;
            }
            else if (errcode == 21)
            {
                Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoExceeds);
                return;
            }
            else if (errcode == 13)
            {
                Utility.FunShowAlertMsg(this, "Final Sanction Amount cannot be greater than the Required Facility Amount");
                return;
            }
            else if (errcode == 20)
            {
                return;
            }

            else if (errcode == 3)
            {
                Utility.FunShowAlertMsg(this, "Approval should be in sequence as defined in Rule Card");
                return;
            }
            else if (errcode == 4)
            {
                Utility.FunShowAlertMsg(this, "Approval Already done for this application number");
                return;
            }
            else if (errcode == 5)
            {
                Utility.FunShowAlertMsg(this, "Approver not having access to approve");
                return;
            }

            else if (errcode == 25)
            {
                Utility.FunShowAlertMsg(this, "Approver not having access to approve");
                return;
            }
            else
            {

                InitializePageValues();

                if ((_NumOfApproved + 1) == int.Parse(ViewState["NumOfApprovals"].ToString())) // if it is a last approval, then create Sanction Number
                    strAlert = strAlert.Replace("__ALERT__", "All the Approvals have been completed successfully");
                else
                    strAlert = strAlert.Replace("__ALERT__", "Credit Parameter Approval completed successfully");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                btnSave.Enabled = false;
            }
        }
        catch (FaultException<CreditMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            Utility.FunShowAlertMsg(this, objFaultExp.Message);
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
            throw ex;
        }
        finally
        {
            ObjS3GAdminServices.Close();
        }
    }
    protected void CancelCus_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage + "&Flag=C");
    }
    protected void chkApprove_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkApprove = ((CheckBox)sender);
        IsApproved = chkApprove.Checked;
    }
    protected void ddlCusLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        // initialization
        string strddlCusLOB = ((DropDownList)sender).ClientID;
        DropDownList ddlCusLOB = null;
        string strSelectedValue = string.Empty;
        int indexValue = 0;

        // when LOB get changed then change the Facility required to Zero.
        foreach (GridViewRow gvApprovalDetailsRow in gvEnquiryByCustomer.Rows)
        {
            ddlCusLOB = ((DropDownList)gvApprovalDetailsRow.FindControl("ddlCusLOB"));
            if ((string.Compare(strddlCusLOB, ddlCusLOB.ClientID)) == 0)
            {
                TextBox txtFacility = (TextBox)gvApprovalDetailsRow.FindControl("txtCusRequiredFacility");
                if (ViewState["DefaultLOB"] == null)
                {
                    ViewState["DefaultLOB"] = _LOBId;
                    ViewState["DefaultRequiredFacility"] = txtFacility.Text;
                    txtFacility.Text = "0";
                    txtFacility.Enabled = true;
                }
                else
                {
                    if (Convert.ToInt32(ViewState["DefaultLOB"]) == Convert.ToInt32(ddlCusLOB.SelectedValue))
                        txtFacility.Text = ViewState["DefaultRequiredFacility"].ToString();
                    else
                    {
                        txtFacility.Text = "0"; txtFacility.Enabled = true;
                    }
                }
            }
        }
    }
    protected void txtCusRequiredFacility_TextChanged(object sender, EventArgs e)
    {
        string strtxtCusRequiredFacility = ((TextBox)sender).ClientID;
        TextBox txtCusRequiredFacility = null;
        TextBox txtFinalSanction = null;
        foreach (GridViewRow gvApprovalDetailsRow in gvEnquiryByCustomer.Rows)
        {
            txtCusRequiredFacility = ((TextBox)gvApprovalDetailsRow.FindControl("txtCusRequiredFacility"));
            txtFinalSanction = ((TextBox)gvApprovalDetailsRow.FindControl("txtCusFinalSanctionedAmount"));
            if ((string.Compare(strtxtCusRequiredFacility, txtCusRequiredFacility.ClientID)) == 0)
            {
                TextBox txtCusSanctionedLimit = ((TextBox)gvApprovalDetailsRow.FindControl("txtCusSanctionedLimit"));
                Label lblCusOfferCard = ((Label)gvApprovalDetailsRow.FindControl("lblCusOfferCard"));
                dtScoreBoard = (DataTable)ViewState["dtScoreBoard"];

                Decimal dc = Convert.ToDecimal(FunPriGetCustomerSanctionLimit(dtScoreBoard, txtCusRequiredFacility.Text, lblCusOfferCard));
                if (dc < Convert.ToDecimal("0.0"))
                {
                    Utility.FunShowAlertMsg(this, "Actual Score is lesser than the Required Score");
                    btnSave.Enabled = false;
                }
                else
                {
                    txtFinalSanction.Text = txtCusSanctionedLimit.Text = dc.ToString("00.00");
                    btnSave.Enabled = true;
                }

                break;
            }
        }
    }
    private string FunPriGetCustomerSanctionLimit(DataTable dt_ScoreBoard, string strRequiredFacility, Label lblCusOfferCard)
    {
        try
        {

            if (dt_ScoreBoard != null && dt_ScoreBoard.Rows.Count > 0)
            {
                //int count = 0;
                DataRow dr_ScoreBoardTotal = dt_ScoreBoard.NewRow();
                dr_ScoreBoardTotal["CreditScore"] = "Total % / Total Score";

                dr_ScoreBoardTotal["PercentageOfImportance"] = Convert.ToDouble(dt_ScoreBoard.Rows[dt_ScoreBoard.Rows.Count - 1]["PercentageOfImportance"].ToString());
                dr_ScoreBoardTotal["RequiredScore"] = Convert.ToDouble(dt_ScoreBoard.Rows[dt_ScoreBoard.Rows.Count - 1]["RequiredScore"].ToString());
                dr_ScoreBoardTotal["ActualScore"] = Convert.ToDouble(dt_ScoreBoard.Rows[dt_ScoreBoard.Rows.Count - 1]["ActualScore"].ToString());
                double requiredHygenie = 0;// = double.Parse(dt_ScoreBoard.Compute("sum(RequiredScore)", "").ToString());
                double actualHygenie = 0;// = double.Parse(dt_ScoreBoard.Compute("sum(ActualScore)", "").ToString());

                //double requiredHygenie, actualHygenie;
                if (dtScoreBoard != null)
                {
                    DataRow[] dtrHygenieRows = dt_ScoreBoard.Select("CreditScore='Hygiene Factor'");
                    if (dtrHygenieRows.Length > 0)
                    {
                        requiredHygenie = Convert.ToDouble(dtrHygenieRows[0]["RequiredScore"]);
                        actualHygenie = Convert.ToDouble(dtrHygenieRows[0]["ActualScore"]);
                    }
                }

                if (strRequiredFacility != "")
                {
                    _RevisedLimitForTheCustomer = CalculateSanctionAmount(decimal.Parse(strRequiredFacility), dt_ScoreBoard, requiredHygenie, actualHygenie, dr_ScoreBoardTotal);
                }
                else
                    _RevisedLimitForTheCustomer = 0;
            }
            return _RevisedLimitForTheCustomer.ToString();
        }
        catch (Exception ex)
        {
            return _RevisedLimitForTheCustomer.ToString();
        }
    }
    protected void DdlCusOverRide_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strddlOverRide = ((DropDownList)sender).ClientID;
        DropDownList ddlOverride = null;
        foreach (GridViewRow gvApprovalDetailsRow in gvEnquiryByCustomer.Rows)
        {
            ddlOverride = ((DropDownList)gvApprovalDetailsRow.FindControl("ddlCusOverRide"));
            if ((string.Compare(strddlOverRide, ddlOverride.ClientID)) == 0)
            {
                TextBox txtCusFinalSanctionedAmount = (TextBox)gvApprovalDetailsRow.FindControl("txtCusFinalSanctionedAmount");
                TextBox txtCusSanctionedAmount = (TextBox)gvApprovalDetailsRow.FindControl("txtCusSanctionedLimit");
                if (ddlOverride.SelectedValue == "0")
                {
                    txtCusFinalSanctionedAmount.Enabled = true;
                    txtCusFinalSanctionedAmount.Focus();
                }
                else
                {
                    txtCusFinalSanctionedAmount.Text = (txtCusSanctionedAmount.Text != string.Empty) ? txtCusSanctionedAmount.Text : "";
                    txtCusFinalSanctionedAmount.Enabled = false;
                }
                break;
            }
        }
    }
    #endregion

    protected void MainPage_Click(object sender, EventArgs e)
    {
        //FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(_CreditParamTransId.ToString(), false, 0);
        //Response.Redirect("~/Origination/S3GOrgCreditParameterApproval.aspx?Code=CPA&qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
        //Response.Redirect("~/Origination/S3GOrgCreditParameterApproval.aspx?qsMode=C");
        //wf Cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect(strRedirectPage);
    }
    protected void TxtPassword_TextChanged(object sender, EventArgs e)
    {
        CheckValidPassword();
    }

    private bool CheckValidPassword()
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {
            string strTextBox = gvApprovalDetails.Rows[gvApprovalDetails.Rows.Count - 1].FindControl("txtPassword").ClientID;
            TextBox txtPassword = null;
            foreach (GridViewRow gvApprovalDetailsRow in gvApprovalDetails.Rows)
            {
                txtPassword = ((TextBox)gvApprovalDetailsRow.FindControl("txtPassword"));
                if ((string.Compare(strTextBox, txtPassword.ClientID)) == 0)
                {
                    string strUserName = ((Label)gvApprovalDetailsRow.FindControl("lblEmpName")).Text;
                    _Remark = ((TextBox)gvApprovalDetailsRow.FindControl("txtRemark")).Text;

                    if (ObjS3GAdminServices.FunPubPasswordValidation(userInfo.ProUserIdRW, txtPassword.Text.Trim()) > 0)
                    {
                        IsCorrectPassword = false;
                        Utility.FunShowAlertMsg(this, "Enter correct password");
                        txtPassword.Focus();
                    }
                    else
                    {
                        IsCorrectPassword = true;
                        TextBox txtRemark = ((TextBox)gvApprovalDetailsRow.FindControl("txtRemark"));
                        txtRemark.Focus();
                    }

                    txtPassword.Attributes.Add("value", txtPassword.Text);

                    break;
                }

            }
            return IsCorrectPassword;
        }
        catch (Exception e)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        finally
        {
            ObjS3GAdminServices.Close();
        }
    }
    private bool IsUserValid(string userName, string pass)
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            CreditMgtServices.S3G_ORG_UserIsValidRow ObjUserIsValidRow;
            CreditMgtServices.S3G_ORG_UserIsValidDataTable ObjUserIsValidDataTable = new CreditMgtServices.S3G_ORG_UserIsValidDataTable();


            ObjUserIsValidRow = ObjUserIsValidDataTable.NewS3G_ORG_UserIsValidRow();

            ObjUserIsValidRow.User_Name = userName;
            ObjUserIsValidRow.Password = pass;

            ObjUserIsValidDataTable.AddS3G_ORG_UserIsValidRow(ObjUserIsValidRow);
            SerializationMode SerMode = new SerializationMode();

            byte[] bytesUserValid = ObjMgtCreditMgtClient.FunPubQueryUserIsValid(SerMode, ClsPubSerialize.Serialize(ObjUserIsValidDataTable, SerMode));
            DataTable dt_User = (CreditMgtServices.S3G_ORG_UserIsValidDataTable)ClsPubSerialize.DeSerialize(bytesUserValid, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_UserIsValidDataTable));

            if (dt_User != null && dt_User.Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        catch (Exception ae)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }
    }
    protected void TxtRemark_TextChanged(object sender, EventArgs e)
    {
        string strTextBox = ((TextBox)sender).ClientID;
        TextBox txtRemark = null;
        foreach (GridViewRow gvApprovalDetailsRow in gvApprovalDetails.Rows)
        {
            txtRemark = ((TextBox)gvApprovalDetailsRow.FindControl("txtRemark"));
            if ((string.Compare(strTextBox, txtRemark.ClientID)) == 0)
            {
                _Remark = txtRemark.Text;

                txtRemark.Attributes.Add("value", _Remark);

                break;
            }

        }
    }


    #region Score Board GridView
    private void DisplayScoreBoardGrid()
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardRow ObjCreditParameterApprovalScoreBoardRow;
            CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable ObjCreditParameterApprovalScoreBoardDataTable = new CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable();


            ObjCreditParameterApprovalScoreBoardRow = ObjCreditParameterApprovalScoreBoardDataTable.NewS3G_ORG_GetCreditParameterApproval_ScoreBoardRow();

            ObjCreditParameterApprovalScoreBoardRow.CreditParamTrans_ID = CreditParamTransID;

            ObjCreditParameterApprovalScoreBoardDataTable.AddS3G_ORG_GetCreditParameterApproval_ScoreBoardRow(ObjCreditParameterApprovalScoreBoardRow);
            SerializationMode SerMode = new SerializationMode();

            byte[] bytesCreditParameterTransaction = ObjMgtCreditMgtClient.FunPubQueryCreditParameterApproval_ScoreBoard(SerMode, ClsPubSerialize.Serialize(ObjCreditParameterApprovalScoreBoardDataTable, SerMode));
            dtScoreBoard = (CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterTransaction, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable));

            dtScoreBoard = FunPriGetSoreBoardSum(dtScoreBoard);
            DataRow[] dtrRows = dtScoreBoard.Select("CreditScore='Hygiene Factor'");
            if (dtrRows.Length > 0)
            {
                _ActualCPTHygenie = Convert.ToDouble(((double.Parse(dtrRows[0]["ActualScore"].ToString()) /
                    (Convert.ToDouble(dtScoreBoard.Compute("sum (ActualScore)", "")) -
                    Convert.ToDouble(dtScoreBoard.Rows[dtScoreBoard.Rows.Count - 1]["ActualScore"]))) * 100).ToString("0000.00"));

                ViewState["_ActualCPTHygenie"] = _ActualCPTHygenie;
            }

            GetNumberOfApprovals();

            

            ViewState["dtScoreBoard"] = dtScoreBoard;
            //txtReqApprovalDone
            gvCScoreBoard.DataSource = dtScoreBoard;
            gvCScoreBoard.DataBind();
        }
        catch (Exception ae)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }
    }

    private void GetNumberOfApprovals()
    {
        DataTable dtFillUI = (DataTable)ViewState["dtFillUI"];
        if (dtFillUI != null && dtFillUI.Rows.Count > 0)
        {
            //  ViewState["RequiredFacility"] = dtFillUI.Rows[0]["Finance_Amount_Sought"].ToString();

            string facility = ViewState["RequiredFacility"].ToString() + ">= Facility_From and " + ViewState["RequiredFacility"].ToString() + " <= Facility_To";
            DataRow[] dtRows = dtFillUI.Select(facility);
            if (dtRows.Length > 0)
            {
                _NumOfApprovals = Convert.ToInt32(dtRows[0]["Number_Of_Approvals"].ToString());
                if (dtRows[0]["Hygiene_Percentage"] != null && dtRows[0]["Hygiene_Percentage"].ToString() != string.Empty)
                {
                    if (decimal.Parse(dtRows[0]["Hygiene_Percentage"].ToString()) != 0)
                    {
                        if (((ViewState["_ActualCPTHygenie"] != null) ? double.Parse(ViewState["_ActualCPTHygenie"].ToString()) : 0) >= double.Parse(dtRows[0]["Hygiene_Percentage"].ToString()))
                        {
                            _NumOfApprovals = Convert.ToInt32(dtRows[0]["Minimal_Approval_Required"].ToString());
                        }
                        else
                        {
                            _NumOfApprovals = Convert.ToInt32(dtRows[0]["Number_Of_Approvals"].ToString());
                        }
                    }
                }
                //else  // IF Hygenie Percentage is null or 0 defined in Global Paramaeter
                //{
                //    _NumOfApprovals = Convert.ToInt32(dtRows[0]["Number_Of_Approvals"].ToString());
                //}
                if (dtRows[0]["GLOBAL_CRDT_PARAM_APPROVAL_ID"] != null && dtRows[0]["GLOBAL_CRDT_PARAM_APPROVAL_ID"].ToString() != string.Empty)
                {
                    ViewState["Approval_Details_ID"] = Convert.ToInt32(dtRows[0]["GLOBAL_CRDT_PARAM_APPROVAL_ID"].ToString());//Global_Credit_Parameter_Approval_ID
                }
                else
                {
                    ViewState["Approval_Details_ID"] = "0";
                }
                txtReqApprovals.Text = _NumOfApprovals.ToString();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Approval limit is not defined in global credit parameter setup");
                _NumOfApprovals = 0;
                btnSave.Enabled = btnCusSave.Enabled = false;
                //return;
            }
        }
        else
        {
            Utility.FunShowAlertMsg(this, "Approval limit or Range Limit is not defined in global credit parameter setup");
            _NumOfApprovals = 0;
            btnSave.Enabled = btnCusSave.Enabled = false;
            //return;
        }

        ViewState["NumOfApprovals"] = _NumOfApprovals;
    }
    private DataTable FunPriGetSoreBoardSum(DataTable dt_ScoreBoard)
    {
        double requiredHygenie = 0, actualHygenie = 0;
        if (dt_ScoreBoard != null && dt_ScoreBoard.Rows.Count > 0)
        {
            int count = 0;
            DataRow dr_ScoreBoardTotal = dt_ScoreBoard.NewRow();
            dr_ScoreBoardTotal["CreditScore"] = "Total % / Total Score";
            dr_ScoreBoardTotal["PercentageOfImportance"] = Convert.ToDouble(Funsetsuffix());
            dr_ScoreBoardTotal["RequiredScore"] = Convert.ToDouble(Funsetsuffix());
            dr_ScoreBoardTotal["ActualScore"] = Convert.ToDouble(Funsetsuffix());
            //for (int i_ScoreBoardRow = 0; i_ScoreBoardRow < dt_ScoreBoard.Rows.Count; i_ScoreBoardRow++)
            //{
            //    ++count;
            //Hygiene Factor  
            dr_ScoreBoardTotal["PercentageOfImportance"] = Convert.ToDouble(dt_ScoreBoard.Compute("sum (PercentageOfImportance)", ""));
            dr_ScoreBoardTotal["RequiredScore"] = Convert.ToDouble(dt_ScoreBoard.Compute("sum (RequiredScore)", ""));
            dr_ScoreBoardTotal["ActualScore"] = Convert.ToDouble(dt_ScoreBoard.Compute("sum (ActualScore)", ""));

            DataRow[] dtrHygenieRows = dt_ScoreBoard.Select("CreditScore='Hygiene Factor'");
            if (dtrHygenieRows.Length > 0)
            {
                requiredHygenie = Convert.ToDouble(dtrHygenieRows[0]["RequiredScore"]);
                actualHygenie = Convert.ToDouble(dtrHygenieRows[0]["ActualScore"]);
            }
            // Exclude the Hygenie factor after total
            dr_ScoreBoardTotal["RequiredScore"] = Convert.ToDouble(dr_ScoreBoardTotal["RequiredScore"]) - requiredHygenie;
            //}            
            dt_ScoreBoard.Rows.Add(dr_ScoreBoardTotal);

            CalculateSanctionAmount(decimal.Parse(ViewState["RequiredFacility"].ToString()), dt_ScoreBoard, requiredHygenie, actualHygenie, dr_ScoreBoardTotal);
        }

        if (dt_ScoreBoard != null && dt_ScoreBoard.Rows.Count == 0) // if no records found
        {
            DataRow dr = dt_ScoreBoard.NewRow();
            dt_ScoreBoard.Rows.Add(dr);
        }
        return dt_ScoreBoard;
    }

    private decimal CalculateSanctionAmount(decimal reqFacilityAmount, DataTable dt_ScoreBoard, double requiredHygenie, double actualHygenie, DataRow dr_ScoreBoardTotal)
    {
        double requiredScore, actualScore, hygenie = 0, negativeVariance = 0, exposureVariance = 0, differenceInScore, variance,
                    _exposureVariance = 0, calExposureVariance = 0;

        decimal actualSactionAmount = 0, requiredAmount = 0, calExposureAmount = 0;

        requiredAmount = reqFacilityAmount; //double.Parse(ViewState["RequiredFacility"].ToString());        

        negativeVariance = double.Parse(dt_ScoreBoard.Rows[0]["Negative_Deviation"].ToString());
        exposureVariance = double.Parse(dt_ScoreBoard.Rows[0]["Exposure_Variance"].ToString());

        requiredScore = double.Parse(dr_ScoreBoardTotal["requiredScore"].ToString());// - requiredHygenie;
        actualScore = double.Parse(dr_ScoreBoardTotal["actualScore"].ToString()) - actualHygenie;
        differenceInScore = requiredScore - actualScore;
        variance = Math.Round((differenceInScore / requiredScore) * 100, 2); // Percentage Value
        calExposureVariance = Math.Round((exposureVariance * variance) / negativeVariance, 2); // Percentage Value
        calExposureAmount = Math.Round((requiredAmount * Convert.ToDecimal(calExposureVariance)) / 100, 2);
        actualSactionAmount = Math.Round(requiredAmount - calExposureAmount, 2);
        double exposurePercentage = Math.Round(exposureVariance * ((differenceInScore / requiredScore) * 100) / negativeVariance, 0);

        if (ViewState["GuideLineValue"] != null)
        {
            if (actualSactionAmount > decimal.Parse(ViewState["GuideLineValue"].ToString()))
            {
                txtSanctionedAmt.Text = SANCTIONED_AMOUNT = actualSactionAmount.ToString(Funsetsuffix());
                Utility.FunShowAlertMsg(this, "Sanctioned Amount is greater than Guide Line Value");
                btnSave.Enabled = false;
            }
            else
                txtSanctionedAmt.Text = SANCTIONED_AMOUNT = actualSactionAmount.ToString(Funsetsuffix());
        }
        else
            txtSanctionedAmt.Text = SANCTIONED_AMOUNT = actualSactionAmount.ToString(Funsetsuffix());
        if ((differenceInScore > negativeVariance) && (exposurePercentage > exposureVariance))
        {
            if ((string.Compare(_Mode, "Cus")) != 0)    // customer mode
                txtOfferCard.Text = "Disable";
            else
                lblOfferCard.Text = "Disable";
        }
        else
        {
            if ((string.Compare(_Mode, "Cus")) != 0)    // customer mode
                txtOfferCard.Text = "Enable";
            else
                lblOfferCard.Text = "Disable";
        }
        return actualSactionAmount;
    }

    private decimal FunPriGetSanctionAmount(string strRequiredFacility, Label lblCusOfferCard)
    {
        try
        {
            if (ViewState["dtScoreBoard"] != null)
            {
                DataTable dt_ScoreBoard = (DataTable)ViewState["dtScoreBoard"];
                if (dt_ScoreBoard != null && dt_ScoreBoard.Rows.Count > 0)
                {
                    DataRow dr_ScoreBoardTotal = dt_ScoreBoard.NewRow();
                    dr_ScoreBoardTotal["CreditScore"] = "Total % / Total Score";

                    dr_ScoreBoardTotal["PercentageOfImportance"] = Convert.ToDouble(dt_ScoreBoard.Rows[dt_ScoreBoard.Rows.Count - 1]["PercentageOfImportance"].ToString());
                    dr_ScoreBoardTotal["RequiredScore"] = Convert.ToDouble(dt_ScoreBoard.Rows[dt_ScoreBoard.Rows.Count - 1]["RequiredScore"].ToString());
                    dr_ScoreBoardTotal["ActualScore"] = Convert.ToDouble(dt_ScoreBoard.Rows[dt_ScoreBoard.Rows.Count - 1]["ActualScore"].ToString());

                    double requiredHygenie = 0;
                    double actualHygenie = 0;

                    if (dt_ScoreBoard != null)
                    {
                        DataRow[] dtrHygenieRows = dt_ScoreBoard.Select("CreditScore='Hygiene Factor'");
                        if (dtrHygenieRows.Length > 0)
                        {
                            requiredHygenie = Convert.ToDouble(dtrHygenieRows[0]["RequiredScore"]);
                            actualHygenie = Convert.ToDouble(dtrHygenieRows[0]["ActualScore"]);
                        }
                    }

                    if (strRequiredFacility != "")
                    {
                        return FunCalculateSanctionAmount(decimal.Parse(strRequiredFacility), dt_ScoreBoard, requiredHygenie, actualHygenie, dr_ScoreBoardTotal);
                    }
                    else
                        return 0;
                }
            }
            return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    private decimal FunCalculateSanctionAmount(decimal reqFacilityAmount, DataTable dt_ScoreBoard, double requiredHygenie, double actualHygenie, DataRow dr_ScoreBoardTotal)
    {
        double requiredScore, actualScore, hygenie = 0, negativeVariance = 0, exposureVariance = 0, differenceInScore, variance,
                    _exposureVariance = 0, calExposureVariance = 0;

        decimal actualSactionAmount = 0, requiredAmount = 0, calExposureAmount = 0;

        requiredAmount = reqFacilityAmount;

        negativeVariance = double.Parse(dt_ScoreBoard.Rows[0]["Negative_Deviation"].ToString());
        exposureVariance = double.Parse(dt_ScoreBoard.Rows[0]["Exposure_Variance"].ToString());

        requiredScore = double.Parse(dr_ScoreBoardTotal["requiredScore"].ToString());
        actualScore = double.Parse(dr_ScoreBoardTotal["actualScore"].ToString()) - actualHygenie;
        differenceInScore = requiredScore - actualScore;
        variance = Math.Round((differenceInScore / requiredScore) * 100, 2); 
        calExposureVariance = Math.Round((exposureVariance * variance) / negativeVariance, 2); 
        calExposureAmount = Math.Round((requiredAmount * Convert.ToDecimal(calExposureVariance)) / 100, 2);
        actualSactionAmount = Math.Round(requiredAmount - calExposureAmount, 2);
        double exposurePercentage = Math.Round(exposureVariance * ((differenceInScore / requiredScore) * 100) / negativeVariance, 0);

        
        return actualSactionAmount;
    }

    protected void gvCScoreBoard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (((Label)e.Row.FindControl("lblCreditScore")) != null)
            {
                string strCreditScore = ((Label)e.Row.FindControl("lblCreditScore")).Text;

                Label lblCreditScore = (Label)e.Row.FindControl("lblCreditScore");
                Label lblPercentOfImport = (Label)e.Row.FindControl("lblPercentOfImport");
                Label lblRequiredScore = (Label)e.Row.FindControl("lblRequiredScore");
                Label lblActualScore = (Label)e.Row.FindControl("lblActualScore");


                lblPercentOfImport.Text = (lblPercentOfImport.Text.Trim().Length > 0) ? Convert.ToDecimal(lblPercentOfImport.Text).ToString(Funsetsuffix(), CultureInfo.InvariantCulture) : "0";
                lblRequiredScore.Text = Convert.ToDecimal(lblRequiredScore.Text).ToString(Funsetsuffix(), CultureInfo.InvariantCulture);
                lblActualScore.Text = Convert.ToDecimal(lblActualScore.Text).ToString(Funsetsuffix(), CultureInfo.InvariantCulture);

                if ((string.Compare(strCreditScore, "Total % / Total Score")) == 0)
                {

                    lblActualScore.Font.Bold =
                    lblRequiredScore.Font.Bold =
                    lblPercentOfImport.Font.Bold =
                    lblCreditScore.Font.Bold = true;

                    lblActualScore.ForeColor =
                    lblRequiredScore.ForeColor =
                    lblPercentOfImport.ForeColor =
                    lblCreditScore.ForeColor = System.Drawing.Color.Black;

                }

            }
        }
        catch (Exception ex) // if Label control was null
        {

        }
    }
    #endregion
    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    #region Save / Cancel
    /// <summary>
    /// Save Button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Save_Click(object sender, EventArgs e)
    {
        if (isAlreadySaved == true)
            return;
        if (CheckValidPassword())  // code to save...
        {
            try
            {

                int errcode = 0;
                // insert the header table data
                errcode = FunCreateCreditParameterApproval("");
                if (errcode == 12)
                {
                    Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoNotDefined);
                    return;
                }
                else if (errcode == 21)
                {
                    Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoExceeds);
                    return;
                }
                if (errcode == 3)
                {
                    Utility.FunShowAlertMsg(this, "Approval should be in sequence as defined in Rule Card");
                    return;
                }
                else if (errcode == 4)
                {
                    Utility.FunShowAlertMsg(this, "Approval Already done for this application number");
                    return;
                }
                else if (errcode == 5)
                {
                    Utility.FunShowAlertMsg(this, "Approver not having access to approve");
                    return;
                }

                // insert the details table row and generate the SCN number
                errcode = FunCreateCreditParameterApprovalDetails();

                if (errcode == 3)
                {
                    Utility.FunShowAlertMsg(this, "Approval should be in sequence as defined in Rule Card");
                    return;
                }
                else if (errcode == 4)
                {
                    Utility.FunShowAlertMsg(this, "Approval Already done for this application number");
                    return;
                }
                else if (errcode == 5)
                {
                    Utility.FunShowAlertMsg(this, "Approver not having access to approve");
                    return;
                }

                if (errcode == 12)
                {
                    Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoNotDefined);
                    return;
                }
                else if (errcode == 21)
                {
                    Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoExceeds);
                    return;
                }

                // Re initialize the Page to show the SANCTION NUMBER
                InitializePageValues();

                if (!(IsUserAlreadyEdited))
                {
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    _NumOfApproved += 1;    // increment the Approved Count
                    Utility.FunShowAlertMsg(this, "Credit Paramater Approval saved successfully");
                }
                else
                {
                    if ((_NumOfApproved + 1) == int.Parse(ViewState["NumOfApprovals"].ToString())) // if it is a last approval, then create Sanction Number
                    {
                        if (isWorkFlowTraveler)
                        {
                            WorkFlowSession WFValues = new WorkFlowSession();
                            if (intApprovalStatus == 9) // If Approved  ....
                            {
                                try
                                {
                                    int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, WFValues.Document_Type);
                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here

                                    strAlert = "";
                                }
                                catch (Exception ex)
                                {
                                    strAlert = "Work Flow is not Assigned";
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                                }
                                ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode, strAlert);
                                return;
                            }
                            else
                            {
                                int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                            }
                        }
                        else
                        {
                            DataTable WFFP = new DataTable();
                            if (CheckForForcePullOperation(null, txtEnquiryNumber.Text.Trim(), ProgramCode, null, null, "O", CompanyId, null, null, txtLOB.Text.Trim(), txtProduct.Text.Trim(), out WFFP))
                            {
                                DataRow dtrForce = WFFP.Rows[0];
                                if (intApprovalStatus == 9) // If Approved ....
                                {
                                    try
                                    {
                                        int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), txtEnquiryNumber.Text.Trim(), int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), 0);

                                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                        btnSave.Enabled = false;
                                        //End here
                                    }
                                    catch (Exception ex)
                                    {
                                        int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());
                                    }
                                }
                                else
                                {
                                    int WorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());
                                }
                            }

                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here

                            Utility.FunShowAlertMsg(this, "All the Approval process have been completed successfully.");
                        }
                    }
                    else
                    {
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        Utility.FunShowAlertMsg(this, "Credit Paramater Approval process has been completed successfully");
                    }
                }
                isAlreadySaved = true;
            }
            catch (FaultException<CreditMgtServicesReference.ClsPubFaultException> objFaultExp)
            {
                Utility.FunShowAlertMsg(this, objFaultExp.Message);
                throw objFaultExp;
            }
            catch (Exception ex)
            {
                Utility.FunShowAlertMsg(this, ex.Message);
                throw ex;
            }

            IsCorrectPassword = false;
        }
        else // user entered wrong password
        {
            Utility.FunShowAlertMsg(this, "Enter/re-enter the password");
        }
    }
    /* WorkFlow Properties */
    private int WFLOBId { get { return 8; } }
    private int WFProduct { get { return 3; } }
    private void InitializePageValues()
    {
        // Reinit the page
        FunInitPage();
        //GetNumberOfApprovals();
        CreateDisplayGrid();
        DisplayScoreBoardGrid();
        FunGetCustomerGridSource();
    }
    /// <summary>
    /// Cancel Button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Cancel_Click(object sender, EventArgs e)
    {
        //wf Cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect(strRedirectPage + "&Flag=E");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strCusSanctionedLimit"></param>
    /// <returns></returns>
    private int FunCreateCreditParameterApproval(string strCusSanctionedLimit)
    {
        CreditMgtServicesReference.CreditMgtServicesClient objCPA = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            // this is to insert the record in Credit Parameter Approval Table
            CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable ObjS3G_ORG_CPATable = new CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalRow ObjS3G_ORG_CPARow = ObjS3G_ORG_CPATable.NewS3G_ORG_CreditParameterApprovalRow();
            ObjS3G_ORG_CPARow.CreditParamTrans_ID = CreditParamTransID;
            ObjS3G_ORG_CPARow.Approval_Details_ID = Convert.ToInt32(ViewState["Approval_Details_ID"].ToString());
            if (_CustId != null && _CustId != 0)
            {
                ObjS3G_ORG_CPARow.Customer_ID = _CustId;
            }

            ObjS3G_ORG_CPARow.isFinalApproval = 0;
            if ((_NumOfApproved + 1) == int.Parse(ViewState["NumOfApprovals"].ToString())) // if it is a last approval, then create Sanction Number
            {
                ObjS3G_ORG_CPARow.Santion_Number = "1"; // create from Document Number Control - handled in SP.
                ObjS3G_ORG_CPARow.Sanction_Date = DateTime.Now; //DateTime.Parse(System.DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);
                ObjS3G_ORG_CPARow.isFinalApproval = 1;
            }
            else
            {
                ObjS3G_ORG_CPARow.Santion_Number = string.Empty;
            }

            if ((string.Compare(_Mode, "Cus")) == 0)    // customer mode
            {
                ObjS3G_ORG_CPARow.Sanction_Amount = Convert.ToDecimal(strCusSanctionedLimit);
            }
            else  // enquiry Mode
            {
                ObjS3G_ORG_CPARow.Sanction_Amount = Convert.ToDecimal(txtSanctionedAmt.Text);
            }
            ObjS3G_ORG_CPARow.LOB_ID = _LOBId;
            ObjS3G_ORG_CPARow.Branch_ID = _LocationId;
            ObjS3G_ORG_CPARow.Product_ID = _ProductId; // productid = -1 was validated in sql.

            // modification was validated in Stored Procedure
            ObjS3G_ORG_CPARow.Created_By = int.Parse(UserId);
            ObjS3G_ORG_CPARow.Created_On = DateTime.Now;// DateTime.Parse(System.DateTime.Now, CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);
            ObjS3G_ORG_CPARow.Modified_By = int.Parse(UserId);
            ObjS3G_ORG_CPARow.Modified_On = DateTime.Now;// DateTime.Parse(System.DateTime.Now, CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);
            ObjS3G_ORG_CPATable.AddS3G_ORG_CreditParameterApprovalRow(ObjS3G_ORG_CPARow);
            SerializationMode SMode = SerializationMode.Binary;
            return objCPA.FunPubCreateCreditParameterApproval(out Credit_Parameter_Approval_ID, SMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CPATable, SMode));
        }
        catch (Exception ae)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
        finally
        {
            objCPA.Close();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    int intApprovalStatus = 0;
    private int FunCreateCreditParameterApprovalDetails()
    {
        CreditMgtServicesReference.CreditMgtServicesClient objCPA = new CreditMgtServicesReference.CreditMgtServicesClient();
        // this is to insert the CreditParameterApprovalDetails
        CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable ObjS3G_ORG_CPADetailsTable = new CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable();
        S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsRow ObjS3G_ORG_CPADetailsRow = ObjS3G_ORG_CPADetailsTable.NewS3G_ORG_CreditParameterApprovalDetailsRow();
        ObjS3G_ORG_CPADetailsRow.Credit_Parameter_Approval_ID = Credit_Parameter_Approval_ID;
        ObjS3G_ORG_CPADetailsRow.Approval_Serial_No = (_NumOfApproved + 1);

        int isFinalApprovalRow = 0;
        if ((_NumOfApproved + 1) == int.Parse(ViewState["NumOfApprovals"].ToString())) // if it is a last approval, then create Sanction Number
        {
            isFinalApprovalRow = 1;
        }

        ObjS3G_ORG_CPADetailsRow.Approver_ID = int.Parse(UserId);
        CheckBox chkApproved = ((CheckBox)gvApprovalDetails.Rows[gvApprovalDetails.Rows.Count - 1].Cells[2].Controls[1]);
        if (chkApproved.Checked == true)
        {
            intApprovalStatus = 9;
            ObjS3G_ORG_CPADetailsRow.Approval_Status_ID = 9;        // Approved            

        }
        else
        {
            intApprovalStatus = 10;
            // Application is Rejected Stop processin the Loan KR 
            ObjS3G_ORG_CPADetailsRow.Approval_Status_ID = 10;        // Rejected

        }

        ObjS3G_ORG_CPADetailsRow.Approval_Date = DateTime.Now;// DateTime.Parse(System.DateTime.Now, CultureInfo.CurrentCulture.DateTimeFormat).ToString(DateFormate);
        ObjS3G_ORG_CPADetailsRow.Remarks = _Remark;
        ObjS3G_ORG_CPADetailsRow.Approval_Details_ID = int.Parse(ViewState["Approval_Details_ID"].ToString());
        ObjS3G_ORG_CPADetailsTable.AddS3G_ORG_CreditParameterApprovalDetailsRow(ObjS3G_ORG_CPADetailsRow);
        SerializationMode SMode = SerializationMode.Binary;
        string sanctionNumber = string.Empty;
        decimal sanctionAmount = decimal.Parse(txtSanctionedAmt.Text.Trim());
        int errorcode = 0;
        try
        {
            errorcode = objCPA.FunPubCreateCreditParameterApprovalDetails(out sanctionNumber, SMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CPADetailsTable, SMode), isFinalApprovalRow, sanctionAmount);
        }
        catch (Exception es)
        {
            throw es;
        }
        finally
        {
            objCPA.Close();
        }
        return errorcode;
    }

    #endregion

    /// <summary>
    /// Modified by :   Thangam M
    /// Modified on :   08/May/2014
    /// Purpose     :   Credit score changes
    /// </summary>

    #region Customer Facility : Credit score changes


    private void FunLoadLOBMaster()
    {
        try
        {
            userInfo = new UserInfo();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", userInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_Id", Convert.ToString(userInfo.ProUserIdRW));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Program_ID", "51");
            DataTable dtLOB = Utility.GetDefaultData(SPNames.LOBMaster, Procparam);

            ViewState["dtLOB"] = dtLOB;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            userInfo = new UserInfo();

            DropDownList ddlLOB = (DropDownList)sender;

            DropDownList ddlProduct;

            GridView grvFacility = (GridView)ddlLOB.Parent.Parent.Parent.Parent;

            if (((GridViewRow)ddlLOB.Parent.Parent).RowType == DataControlRowType.Footer)
            {
                ddlProduct = (DropDownList)grvFacility.FooterRow.FindControl("ddlFProduct");
            }
            else
            {
                ddlProduct = (DropDownList)((GridViewRow)ddlLOB.Parent.Parent).FindControl("ddlProduct");
            }

            //if (strMode.ToUpper().Trim() != "Q")
            //    Procparam.Add("@Is_Active", "1");

            Procparam.Add("@Company_ID", userInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            ddlProduct.BindDataTable(SPNames.SYS_ProductMaster, Procparam, new string[] { "Product_ID", "Product_Code", "Product_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvFacilityGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["dtGroup"] == null)
                {
                    userInfo = new UserInfo();

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", userInfo.ProCompanyIdRW.ToString());
                    Procparam.Add("@LookupType_Code", "114");
                    ViewState["dtGroup"] = dtGroup = Utility.GetDefaultData("S3G_LOANAD_GetLookUpValues", Procparam);
                }
                else
                {
                    dtGroup = (DataTable)ViewState["dtGroup"];
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblHGroupId = (Label)e.Row.FindControl("lblHGroupId");
                GridView grvFacility = (GridView)e.Row.FindControl("grvFacility");
                Label lblHFacilityAmount = (Label)e.Row.FindControl("lblHFacilityAmount");
                Label lblHApprovedAmount = (Label)e.Row.FindControl("lblHApprovedAmount");
                TextBox txtHFacilityAmount = (TextBox)e.Row.FindControl("txtHFacilityAmount");
                TextBox txtHApprovedAmount = (TextBox)e.Row.FindControl("txtHApprovedAmount");
                FunProSetDataSource(grvFacility, lblHGroupId.Text);

                e.Row.Cells[0].Style.Add("padding", "0px");
                grvFacilityGroup.GridLines = GridLines.None;

                if (lblHGroupId.Text == "0")
                {
                    lblHFacilityAmount.Visible = lblHApprovedAmount.Visible = true;
                    txtHFacilityAmount.Visible = txtHApprovedAmount.Visible = false;
                }
                else
                {
                    lblHFacilityAmount.Visible = lblHApprovedAmount.Visible = false;
                    txtHFacilityAmount.Visible = txtHApprovedAmount.Visible = true;
                }

                if (Request.QueryString["qsMode"] == "Q")
                {
                    HtmlTableCell tdGroupDelete = (HtmlTableCell)e.Row.FindControl("tdGroupDelete");
                    tdGroupDelete.Visible = false;
                    tdAction.Visible = false;

                    txtHFacilityAmount.Visible = txtHApprovedAmount.Visible = false;
                    lblHFacilityAmount.Visible = lblHApprovedAmount.Visible = true;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlHFGroup = (DropDownList)e.Row.FindControl("ddlHFGroup");

                ddlHFGroup.BindDataTable(dtGroup.Copy(), new string[] { "Lookup_Code", "Lookup_Description" });
                ddlHFGroup.Items.RemoveAt(0);

                DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];

                for (int i = 0; i <= dtFacilityGroup.Rows.Count - 1; i++)
                {
                    ddlHFGroup.Items.Remove(ddlHFGroup.Items.FindByValue(dtFacilityGroup.Rows[i]["Group_ID"].ToString()));
                }

                TextBox txtFFacilityAmount = (TextBox)e.Row.FindControl("txtFFacilityAmount");

                if (ddlHFGroup.SelectedValue == "0")
                {
                    txtFFacilityAmount.Visible = false;
                }
                else
                {
                    txtFFacilityAmount.Visible = true;
                }

                if (ddlHFGroup.Items.Count == 0 || Request.QueryString["qsMode"] == "Q")
                {
                    grvFacilityGroup.ShowFooter = false;
                }
                else
                {
                    grvFacilityGroup.ShowFooter = true;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvFacility_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                dtLOB = (DataTable)ViewState["dtLOB"];
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlFLOB = (DropDownList)e.Row.FindControl("ddlFLOB");
                ddlFLOB.BindDataTable(dtLOB.Copy(), new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

                GridViewRow grvGroup = (GridViewRow)e.Row.Parent.Parent.Parent.Parent;
                Label lblHGroupId = (Label)grvGroup.FindControl("lblHGroupId");
                TextBox txtFFacilityAmount = (TextBox)e.Row.FindControl("txtFFacilityAmount");
                Label lblFSanctionedAmount = (Label)e.Row.FindControl("lblFSanctionedAmount");
                TextBox txtFApprovedAmount = (TextBox)e.Row.FindControl("txtFApprovedAmount");

                if (lblHGroupId.Text != "0")
                {
                    txtFFacilityAmount.Visible = lblFSanctionedAmount.Visible = txtFApprovedAmount.Visible = false;
                }

                if (Request.QueryString["qsMode"] == "Q")
                {
                    e.Row.Visible = false;
                    GridView grvFacility = (GridView)e.Row.Parent.Parent;
                    grvFacility.Columns[grvFacility.Columns.Count - 1].Visible = false;
                    
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            GridView grvFacility = (GridView)((Button)sender).Parent.Parent.Parent.Parent;
            DropDownList ddlFLOB = (DropDownList)grvFacility.FooterRow.FindControl("ddlFLOB");
            DropDownList ddlFProduct = (DropDownList)grvFacility.FooterRow.FindControl("ddlFProduct");
            Label lblHGroupId = (Label)grvFacility.Parent.FindControl("lblHGroupId");
            TextBox txtFFacilityAmount = (TextBox)grvFacility.FooterRow.FindControl("txtFFacilityAmount");
            Label lblFSanctionedAmount = (Label)grvFacility.FooterRow.FindControl("lblFSanctionedAmount");
            TextBox txtFApprovedAmount = (TextBox)grvFacility.FooterRow.FindControl("txtFApprovedAmount");

            string strMsg = "";
            Control ctrl = null;
            if (ddlFLOB.SelectedValue == "0")
            {
                strMsg = "    . Select Line of Bussiness\\n";
                ctrl = ddlFLOB;
            }
            //if (ddlFProduct.Items.Count == 0 || (ddlFProduct.Items.Count > 0 && ddlFProduct.SelectedValue == "0"))
            //{
            //    strAlert = strAlert + "    . Select Product\\n";
            //    if (ctrl == null)
            //    {
            //        ctrl = ddlFProduct;
            //    }
            //}
            if (lblHGroupId.Text == "0")
            {
                if (string.IsNullOrEmpty(txtFFacilityAmount.Text))
                {
                    strMsg = strMsg + "    . Enter Facility Amount\\n";
                    if (ctrl == null)
                    {
                        ctrl = txtFFacilityAmount;
                    }
                }

                if (!string.IsNullOrEmpty(txtFFacilityAmount.Text) && Convert.ToDecimal(txtFFacilityAmount.Text) == 0)
                {
                    strMsg = strMsg + "    . Facility Amount cannot be Zero";
                    if (ctrl == null)
                    {
                        ctrl = txtFFacilityAmount;
                    }
                }

                if (string.IsNullOrEmpty(txtFApprovedAmount.Text))
                {
                    strMsg = strMsg + "    . Enter Approved Amount\\n";
                    if (ctrl == null)
                    {
                        ctrl = txtFApprovedAmount;
                    }
                }

                if (!string.IsNullOrEmpty(txtFApprovedAmount.Text) && Convert.ToDecimal(txtFApprovedAmount.Text) == 0)
                {
                    strMsg = strMsg + "    . Approved Amount cannot be Zero";
                    if (ctrl == null)
                    {
                        ctrl = txtFApprovedAmount;
                    }
                }
            }

            if (!string.IsNullOrEmpty(strMsg))
            {
                strMsg = "Correct the following validation(s):\\n\\n" + strMsg;
                Utility.FunShowAlertMsg(this, strMsg);
                ctrl.Focus();
                return;
            }

            DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];

            if (!dtFacilityGrid.Copy().CheckRows("LOB_ID='" + ddlFLOB.SelectedValue + "' AND Product_ID='" + ddlFProduct.SelectedValue + "'", 0))
            {
                Utility.FunShowAlertMsg(this, "Given combination already added");
                return;
            }
            if (!dtFacilityGrid.Copy().CheckRows("LOB_ID='" + ddlFLOB.SelectedValue + "' AND Product_ID='0'", 0))
            {
                Utility.FunShowAlertMsg(this, "Given combination already added");
                return;
            }
            if (!dtFacilityGrid.Copy().CheckRows("LOB_ID='" + ddlFLOB.SelectedValue + "'", 0) && ddlFProduct.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Given combination already added");
                return;
            }

            DataRow dRow = dtFacilityGrid.NewRow();

            dRow["SlNo"] = "1";
            dRow["LOB_ID"] = ddlFLOB.SelectedValue;
            dRow["LOB"] = ddlFLOB.SelectedItem.Text;
            dRow["Product_ID"] = ddlFProduct.SelectedValue;
            if (ddlFProduct.SelectedValue != "0")
            {
                dRow["Product"] = ddlFProduct.SelectedItem.Text;
            }
            dRow["Group_ID"] = lblHGroupId.Text;
            //dRow["Group_Text");
            dRow["Facility_Amount"] = (txtFFacilityAmount.Text == "") ? "0" : txtFFacilityAmount.Text;
            dRow["Sanctioned_limit"] = (lblFSanctionedAmount.Text == "") ? "0" : lblFSanctionedAmount.Text;
            dRow["Final_Sanctioned_Limit"] = (txtFApprovedAmount.Text == "") ? "0" : txtFApprovedAmount.Text;
            //dRow["Total_Amount");

            dtFacilityGrid.Rows.Add(dRow);

            ViewState["dtFacilityGrid"] = dtFacilityGrid;

            FunProSetDataSource(grvFacility, lblHGroupId.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ddlHFGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlHFGroup = (DropDownList)sender;
            TextBox txtFFacilityAmount = (TextBox)((GridViewRow)ddlHFGroup.Parent.Parent).FindControl("txtFFacilityAmount");
            if (ddlHFGroup.SelectedValue == "0")
            {
                txtFFacilityAmount.Visible = false;
            }
            else
            {
                txtFFacilityAmount.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnHAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];

            DropDownList ddlHFGroup = (DropDownList)grvFacilityGroup.FooterRow.FindControl("ddlHFGroup");
            TextBox txtFFacilityAmount = (TextBox)grvFacilityGroup.FooterRow.FindControl("txtFFacilityAmount");
            
            DataRow dRow = dtFacilityGroup.NewRow();
            dRow["Group_ID"] = ddlHFGroup.SelectedValue;
            dRow["Group_Text"] = ddlHFGroup.SelectedItem.Text;

            if (ddlHFGroup.SelectedValue == "0")
            {
                dRow["Total_Amount"] = dRow["Sanctioned_Group_Limit"] = dRow["Final_Groupt_Sanctioned"] = "0";
            }
            else
            {
                string strMsg = "";
                if (string.IsNullOrEmpty(txtFFacilityAmount.Text))
                {
                    strMsg = "    . Enter Facility Amount\\n";
                }
                if (!string.IsNullOrEmpty(txtFFacilityAmount.Text) && Convert.ToDecimal(txtFFacilityAmount.Text) == 0)
                {
                    strMsg = "    . Facility Amount cannot be Zero";
                }

                if (!string.IsNullOrEmpty(strMsg))
                {
                    strMsg = "Correct the following validation(s):\\n\\n" + strMsg;
                    Utility.FunShowAlertMsg(this, strMsg);
                    txtFFacilityAmount.Focus();
                    return;
                }

                dRow["Total_Amount"] = txtFFacilityAmount.Text;

                if (!string.IsNullOrEmpty(txtFFacilityAmount.Text))
                {
                    dRow["Sanctioned_Group_Limit"] = dRow["Final_Groupt_Sanctioned"] = Math.Round(FunPriGetSanctionAmount(txtFFacilityAmount.Text, null), 0).ToString();
                }
            }

            dtFacilityGroup.Rows.Add(dRow);

            grvFacilityGroup.DataSource = dtFacilityGroup;
            grvFacilityGroup.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProInitializeFacilityGroup()
    {
        try
        {
            DataTable dtGridData = new DataTable();

            dtGridData.Columns.Add("Group_ID");
            dtGridData.Columns.Add("Group_Text");
            dtGridData.Columns.Add("Total_Amount", typeof(decimal)); //Required_Group_Facility
            dtGridData.Columns.Add("Sanctioned_Group_Limit", typeof(decimal));
            dtGridData.Columns.Add("Final_Groupt_Sanctioned", typeof(decimal));

            ViewState["dtFacilityGroup"] = dtGridData.Copy();
            ViewState["dtFacilityGrid"] = null;

            DataRow dRow = dtGridData.NewRow();
            dtGridData.Rows.Add(dRow);

            grvFacilityGroup.DataSource = dtGridData;
            grvFacilityGroup.DataBind();
            grvFacilityGroup.Rows[0].Visible = false;
            dtGridData.Rows.Clear();

            lblTotalAmount.Text = "0";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProInitializeFacilityGrid(GridView grv, string strGroup)
    {
        try
        {
            DataTable dtGridData = new DataTable();

            dtGridData.Columns.Add("SlNo");
            dtGridData.Columns.Add("LOB_ID");
            dtGridData.Columns.Add("LOB");
            dtGridData.Columns.Add("Product_ID");
            dtGridData.Columns.Add("Product");
            dtGridData.Columns.Add("Group_ID");
            dtGridData.Columns.Add("Group_Text");
            dtGridData.Columns.Add("Facility_Amount", typeof(decimal));
            dtGridData.Columns.Add("Total_Amount"); //Required_Group_Facility
            dtGridData.Columns.Add("Sanctioned_limit", typeof(decimal));
            dtGridData.Columns.Add("Final_Sanctioned_Limit", typeof(decimal));
            dtGridData.Columns.Add("Sanctioned_Group_Limit", typeof(decimal));
            dtGridData.Columns.Add("Final_Groupt_Sanctioned", typeof(decimal));

            DataRow dRow = dtGridData.NewRow();
            dtGridData.Rows.Add(dRow);

            grv.DataSource = dtGridData;
            grv.DataBind();
            grv.Rows[0].Visible = false;
            dtGridData.Rows.Clear();

            if (ViewState["dtFacilityGrid"] == null)
                ViewState["dtFacilityGrid"] = dtGridData;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProSetDataSource(GridView grv, string strGroup)
    {
        try
        {
            if (ViewState["dtFacilityGrid"] == null)
            {
                FunProInitializeFacilityGrid(grv, strGroup);
            }

            if (strGroup == "")
            {
                strGroup = "0";
            }

            DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];
            DataRow[] dRows = dtFacilityGrid.Select("Group_ID = '" + strGroup + "'");
            decimal dcTotal = 0, dcSanctionAmount = 0, dcApprovedAmount = 0;
            if (dRows.Length > 0)
            {
                DataTable dtFilered = dRows.CopyToDataTable();

                grv.DataSource = dtFilered;
                grv.DataBind();

                //To update sub-total
                dcTotal = (decimal)dtFacilityGrid.Compute("SUM(Facility_Amount)", "Group_ID = '" + strGroup + "'");
                dcSanctionAmount = (decimal)dtFacilityGrid.Compute("SUM(Sanctioned_limit)", "Group_ID = '" + strGroup + "'");
                dcApprovedAmount = (decimal)dtFacilityGrid.Compute("SUM(Final_Sanctioned_Limit)", "Group_ID = '" + strGroup + "'");
            }
            else
            {
                FunProInitializeFacilityGrid(grv, strGroup);
            }

            DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];

            foreach (DataRow drow in dtFacilityGroup.Rows)
            {
                if (drow["Group_ID"].ToString() == strGroup && strGroup == "0")
                {
                    drow["Total_Amount"] = dcTotal.ToString();
                    drow["Sanctioned_Group_Limit"] = dcSanctionAmount.ToString();
                    drow["Final_Groupt_Sanctioned"] = dcApprovedAmount.ToString();
                }
            }

            if (strGroup == "0")
            {
                Label lblHFacilityAmount = (Label)grv.Parent.FindControl("lblHFacilityAmount");
                Label lblHSanctionedAmount = (Label)grv.Parent.FindControl("lblHSanctionedAmount");
                Label lblHApprovedAmount = (Label)grv.Parent.FindControl("lblHApprovedAmount");
                lblHFacilityAmount.Text = dcTotal.ToString();
                lblHSanctionedAmount.Text = dcSanctionAmount.ToString();
                lblHApprovedAmount.Text = dcApprovedAmount.ToString();
            }

            if (dtFacilityGroup.Rows.Count > 0)
            {
                dcTotal = (decimal)dtFacilityGroup.Compute("SUM(Total_Amount)", "1 = 1");
                lblTotalAmount.Text = dcTotal.ToString();
            }

            ViewState["dtFacilityGroup"] = dtFacilityGroup;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            GridView grvFacility = (GridView)((ImageButton)sender).Parent.Parent.Parent.Parent.Parent;
            DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];
            GridViewRow grvRow = (GridViewRow)((ImageButton)sender).Parent.Parent.Parent;

            Label lblLOBId = (Label)grvRow.FindControl("lblLOBId");
            Label lblProductId = (Label)grvRow.FindControl("lblProductId");
            Label lblHGroupId = (Label)grvFacility.Parent.FindControl("lblHGroupId");

            dtFacilityGrid.DeleteRows("LOB_ID='" + lblLOBId.Text + "' AND Product_ID='" + lblProductId.Text + "' AND Group_ID='" + lblHGroupId.Text + "'");

            ViewState["dtFacilityGrid"] = dtFacilityGrid;

            FunProSetDataSource(grvFacility, lblHGroupId.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnGroupDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            GridViewRow grvRow = (GridViewRow)((ImageButton)sender).Parent.Parent.Parent;
            DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];
            DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];
            Label lblHGroupId = (Label)grvRow.FindControl("lblHGroupId");

            dtFacilityGroup.Rows[grvRow.RowIndex].Delete();
            dtFacilityGroup.AcceptChanges();
            ViewState["dtFacilityGroup"] = dtFacilityGroup;

            dtFacilityGrid.DeleteRows("Group_ID='" + lblHGroupId.Text + "'");
            dtFacilityGrid.AcceptChanges();
            ViewState["dtFacilityGrid"] = dtFacilityGrid;

            if (dtFacilityGroup.Rows.Count == 0)
            {
                FunProInitializeFacilityGroup();
            }
            else
            {
                grvFacilityGroup.DataSource = dtFacilityGroup;
                grvFacilityGroup.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnEdit_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunProActionVisibility(sender, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnUpdate_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];

            GridView grvFacility = (GridView)((ImageButton)sender).Parent.Parent.Parent.Parent.Parent;
            GridViewRow grvRow = (GridViewRow)((ImageButton)sender).Parent.Parent.Parent;
            Panel pnlItemActions = (Panel)grvRow.FindControl("pnlItemActions");
            Panel pnlEditActions = (Panel)grvRow.FindControl("pnlEditActions");

            Label lblHGroupId = (Label)grvFacility.Parent.FindControl("lblHGroupId");

            Label lblLOBId = (Label)grvRow.FindControl("lblLOBId");
            Label lblProductId = (Label)grvRow.FindControl("lblProductId");
            Label lblLOB = (Label)grvRow.FindControl("lblLOB");
            Label lblProduct = (Label)grvRow.FindControl("lblProduct");
            Label lblFacilityAmount = (Label)grvRow.FindControl("lblFacilityAmount");
            DropDownList ddlLOB = (DropDownList)grvRow.FindControl("ddlLOB");
            DropDownList ddlProduct = (DropDownList)grvRow.FindControl("ddlProduct");
            TextBox txtFacilityAmount = (TextBox)grvRow.FindControl("txtFacilityAmount");
            TextBox txtApprovedAmount = (TextBox)grvRow.FindControl("txtApprovedAmount");
            Label lblSanctionedAmount = (Label)grvRow.FindControl("lblSanctionedAmount");

            string strMsg = "";
            Control ctrl = null;
            if (ddlLOB.SelectedValue == "0")
            {
                strMsg = "    . Select Line of Bussiness\\n";
                ctrl = ddlLOB;
            }
            //if (ddlProduct.Items.Count == 0 || (ddlProduct.Items.Count > 0 && ddlProduct.SelectedValue == "0"))
            //{
            //    strAlert = strAlert + "    . Select Product\\n";
            //    if (ctrl == null)
            //    {
            //        ctrl = ddlProduct;
            //    }
            //}
            if (lblHGroupId.Text == "0")
            {
                if (string.IsNullOrEmpty(txtFacilityAmount.Text))
                {
                    strMsg = strMsg + "    . Enter Facility Amount\\n";
                    if (ctrl == null)
                    {
                        ctrl = txtFacilityAmount;
                    }
                }
                if (!string.IsNullOrEmpty(txtFacilityAmount.Text) && Convert.ToDecimal(txtFacilityAmount.Text) == 0)
                {
                    strMsg = strMsg + "    . Facility Amount cannot be Zero";
                    if (ctrl == null)
                    {
                        ctrl = txtFacilityAmount;
                    }
                }

                if (string.IsNullOrEmpty(txtApprovedAmount.Text))
                {
                    strMsg = strMsg + "    . Enter Approved Amount\\n";
                    if (ctrl == null)
                    {
                        ctrl = txtApprovedAmount;
                    }
                }

                if (!string.IsNullOrEmpty(txtApprovedAmount.Text) && Convert.ToDecimal(txtApprovedAmount.Text) == 0)
                {
                    strMsg = strMsg + "    . Approved Amount cannot be Zero";
                    if (ctrl == null)
                    {
                        ctrl = txtApprovedAmount;
                    }
                }
            }

            if (!string.IsNullOrEmpty(strMsg))
            {
                strMsg = "Correct the following validation(s):\\n\\n" + strMsg;
                Utility.FunShowAlertMsg(this, strMsg);
                ctrl.Focus();
                return;
            }

            DataTable dtTemp = dtFacilityGrid.Copy();

            dtTemp.UpdateRows("LOB_ID='" + lblLOBId.Text + "' AND Product_ID='" + lblProductId.Text + "' AND Group_ID='" + lblHGroupId.Text + "'", ddlLOB.SelectedValue, ddlLOB.SelectedItem.Text, ddlProduct.SelectedValue, ddlProduct.SelectedItem.Text, txtFacilityAmount.Text, lblSanctionedAmount.Text, txtApprovedAmount.Text);

            if (!dtTemp.CheckRows("LOB_ID='" + ddlLOB.SelectedValue + "' AND Product_ID='" + ddlProduct.SelectedValue + "'", 1))
            {
                Utility.FunShowAlertMsg(this, "Given combination already added");
                return;
            }

            if (!dtTemp.CheckRows("LOB_ID='" + ddlLOB.SelectedValue + "' AND Product_ID='0'", 0) && ddlProduct.SelectedValue != "0")
            {
                Utility.FunShowAlertMsg(this, "Given combination already added");
                return;
            }
            if (!dtFacilityGrid.Copy().CheckRows("LOB_ID='" + ddlLOB.SelectedValue + "'", 1) && ddlProduct.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Given combination already added");
                return;
            }

            dtFacilityGrid.UpdateRows("LOB_ID='" + lblLOBId.Text + "' AND Product_ID='" + lblProductId.Text + "' AND Group_ID='" + lblHGroupId.Text + "'", ddlLOB.SelectedValue, ddlLOB.SelectedItem.Text, ddlProduct.SelectedValue, ddlProduct.SelectedItem.Text, (txtFacilityAmount.Text == "") ? "0" : txtFacilityAmount.Text, lblSanctionedAmount.Text, txtApprovedAmount.Text);

            ViewState["dtFacilityGrid"] = dtFacilityGrid;

            FunProActionVisibility(sender, false);

            FunProSetDataSource(grvFacility, lblHGroupId.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunProActionVisibility(sender, false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProActionVisibility(object sender, bool blVisible)
    {
        try
        {

            GridView grvFacility = (GridView)((ImageButton)sender).Parent.Parent.Parent.Parent.Parent;
            GridViewRow grvRow = (GridViewRow)((ImageButton)sender).Parent.Parent.Parent;
            Panel pnlItemActions = (Panel)grvRow.FindControl("pnlItemActions");
            Panel pnlEditActions = (Panel)grvRow.FindControl("pnlEditActions");

            Label lblLOBId = (Label)grvRow.FindControl("lblLOBId");
            Label lblProductId = (Label)grvRow.FindControl("lblProductId");
            Label lblLOB = (Label)grvRow.FindControl("lblLOB");
            Label lblProduct = (Label)grvRow.FindControl("lblProduct");
            Label lblFacilityAmount = (Label)grvRow.FindControl("lblFacilityAmount");
            Label lblApprovedAmount = (Label)grvRow.FindControl("lblApprovedAmount");
            Label lblSanctionedAmount = (Label)grvRow.FindControl("lblSanctionedAmount");
            DropDownList ddlLOB = (DropDownList)grvRow.FindControl("ddlLOB");
            DropDownList ddlProduct = (DropDownList)grvRow.FindControl("ddlProduct");
            TextBox txtFacilityAmount = (TextBox)grvRow.FindControl("txtFacilityAmount");
            TextBox txtSanctionedAmount = (TextBox)grvRow.FindControl("txtSanctionedAmount");
            TextBox txtApprovedAmount = (TextBox)grvRow.FindControl("txtApprovedAmount");

            pnlItemActions.Visible = lblLOB.Visible = lblProduct.Visible = lblFacilityAmount.Visible = lblApprovedAmount.Visible = !blVisible;
            pnlEditActions.Visible = ddlLOB.Visible = ddlProduct.Visible = txtFacilityAmount.Visible = txtApprovedAmount.Visible = blVisible; 

            GridViewRow grvGroup = (GridViewRow)grvRow.Parent.Parent.Parent.Parent;
            Label lblHGroupId = (Label)grvGroup.FindControl("lblHGroupId");

            if (blVisible)
            {
                dtLOB = (DataTable)ViewState["dtLOB"];
                ddlLOB.BindDataTable(dtLOB.Copy(), new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                ddlLOB.SelectedValue = lblLOBId.Text;
                ddlLOB_SelectedIndexChanged(ddlLOB, null);
                ddlProduct.SelectedValue = lblProductId.Text;
                grvRow.BackColor = grvFacility.FooterStyle.BackColor;

                if (lblHGroupId.Text != "0")
                {
                    txtFacilityAmount.Visible = txtApprovedAmount.Visible = lblSanctionedAmount.Visible = false;
                }
            }
            else
            {
                grvRow.BackColor = System.Drawing.Color.White;
            }

            grvFacility.FooterRow.Visible = !blVisible;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void txtFFacilityAmount_TextChanged(object sender, EventArgs e)
    {
        TextBox txtFFacilityAmount = (TextBox)sender;
        GridView grvFacility = (GridView)txtFFacilityAmount.Parent.Parent.Parent.Parent;
        Label lblFSanctionedAmount = (Label)grvFacility.FooterRow.FindControl("lblFSanctionedAmount");
        TextBox txtFApprovedAmount = (TextBox)grvFacility.FooterRow.FindControl("txtFApprovedAmount");

        if(!string.IsNullOrEmpty(txtFFacilityAmount.Text))
        {
            lblFSanctionedAmount.Text = txtFApprovedAmount.Text = Math.Round(FunPriGetSanctionAmount(txtFFacilityAmount.Text, null), 0).ToString();
        }
    }

    protected void txtFacilityAmount_TextChanged(object sender, EventArgs e)
    {
        TextBox txtFacilityAmount = (TextBox)sender;
        GridViewRow grvRow = (GridViewRow)txtFacilityAmount.Parent.Parent;
        Label lblSanctionedAmount = (Label)grvRow.FindControl("lblSanctionedAmount");
        TextBox txtApprovedAmount = (TextBox)grvRow.FindControl("txtApprovedAmount");

        if (!string.IsNullOrEmpty(txtFacilityAmount.Text))
        {
            lblSanctionedAmount.Text = txtApprovedAmount.Text = Math.Round(FunPriGetSanctionAmount(txtFacilityAmount.Text, null), 0).ToString();
        }
    }

    protected void txtHFacilityAmount_TextChanged(object sender, EventArgs e)
    {
        TextBox txtHFacilityAmount = (TextBox)sender;
        GridViewRow grvRow = (GridViewRow)txtHFacilityAmount.Parent.Parent;
        Label lblHSanctionedAmount = (Label)grvRow.FindControl("lblHSanctionedAmount");
        Label lblHGroupId = (Label)grvRow.FindControl("lblHGroupId");
        TextBox txtHApprovedAmount = (TextBox)grvRow.FindControl("txtHApprovedAmount");

        if (!string.IsNullOrEmpty(txtHFacilityAmount.Text))
        {
            lblHSanctionedAmount.Text = txtHApprovedAmount.Text = Math.Round(FunPriGetSanctionAmount(txtHFacilityAmount.Text, null), 0).ToString();
        }

        DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];
        dtFacilityGroup.UpdateRows("Group_ID='" + lblHGroupId.Text + "'", "Final_Groupt_Sanctioned", lblHSanctionedAmount.Text);
        dtFacilityGroup.UpdateRows("Group_ID='" + lblHGroupId.Text + "'", "Sanctioned_Group_Limit", lblHSanctionedAmount.Text);
        dtFacilityGroup.AcceptChanges();
        ViewState["dtFacilityGroup"] = dtFacilityGroup;

    }

    protected void txtHApprovedAmount_TextChanged(object sender, EventArgs e)
    {
        TextBox txtHApprovedAmount = (TextBox)sender;
        Label lblHGroupId = (Label)((GridViewRow)txtHApprovedAmount.Parent.Parent).FindControl("lblHGroupId");

        string strMsg = "";
        Control ctrl = null;
        if (string.IsNullOrEmpty(txtHApprovedAmount.Text))
        {
            strMsg = strMsg + "    . Enter Approved Amount\\n";
            if (ctrl == null)
            {
                ctrl = txtHApprovedAmount;
            }
        }

        if (!string.IsNullOrEmpty(txtHApprovedAmount.Text) && Convert.ToDecimal(txtHApprovedAmount.Text) == 0)
        {
            strMsg = strMsg + "    . Approved Amount cannot be Zero";
            if (ctrl == null)
            {
                ctrl = txtHApprovedAmount;
            }
        }

        if (!string.IsNullOrEmpty(strMsg))
        {
            strMsg = "Correct the following validation(s):\\n\\n" + strMsg;
            Utility.FunShowAlertMsg(this, strMsg);
            ctrl.Focus();
            return;
        }

        DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];
        dtFacilityGroup.UpdateRows("Group_ID='" + lblHGroupId.Text + "'", "Final_Groupt_Sanctioned", txtHApprovedAmount.Text);
        dtFacilityGroup.AcceptChanges();
        ViewState["dtFacilityGroup"] = dtFacilityGroup;
    }

    #endregion
}

#region Extension Methods for DataTable
static class clsdtExtensions
{
    public static DataTable DeleteRows(this DataTable table, string filter)
    {
        table.Select(filter).DeleteRows();
        return table;
    }

    public static void DeleteRows(this IEnumerable<DataRow> rows)
    {
        foreach (var row in rows)
            row.Delete();
    }

    public static DataTable UpdateRows(this DataTable table, string filter, string strLOBID, string strLOB, string strProductID, string strProduct, string strAmount, string strSanctionedAmount, string strApprovedAmount)
    {
        table.Select(filter).UpdateRows(strLOBID, strLOB, strProductID, strProduct, strAmount, strSanctionedAmount, strApprovedAmount);
        return table;
    }

    public static void UpdateRows(this IEnumerable<DataRow> rows, string strLOBID, string strLOB, string strProductID, string strProduct, string strAmount, string strSanctionedAmount, string strApprovedAmount)
    {
        foreach (var row in rows)
        {
            row["LOB_ID"] = strLOBID;
            row["LOB"] = strLOB;
            row["Product_ID"] = strProductID;
            if (strProductID != "0")
                row["Product"] = strProduct;
            else
                row["Product"] = "";
            row["Facility_Amount"] = strAmount;
            row["Sanctioned_limit"] = strSanctionedAmount;
            row["Final_Sanctioned_Limit"] = strApprovedAmount;
        }
    }

    public static DataTable UpdateRows(this DataTable table, string filter, string strColumnName, string strValue)
    {
        table.Select(filter).UpdateRows(filter, strColumnName, strValue);
        return table;
    }

    public static void UpdateRows(this IEnumerable<DataRow> rows, string filter, string strColumnName, string strValue)
    {
        foreach (var row in rows)
        {
            row[strColumnName] = strValue;
        }
    }

    public static bool CheckRows(this DataTable table, string filter, int intOccurance)
    {
        if (table.Select(filter).Length > intOccurance)
            return false;
        else
            return true;
    }
}
#endregion