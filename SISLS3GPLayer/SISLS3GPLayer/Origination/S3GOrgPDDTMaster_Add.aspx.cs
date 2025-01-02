#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: PRDDT Creation
/// Created By			: Kannan RC
/// Created Date		: 15-July-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 10-Aug-2010   
/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Collections;
using System.Web.UI.HtmlControls;
using S3GBusEntity.Origination;
using PRDDCMgtServicesReference;
using System.IO;


public partial class Origination_S3GOrgPDDTMaster_Add : ApplyThemeForProject
{
    #region Intialization
    public string Program_Id = "41";
    int intCompanyId;
    public int intUserId;
    string strUserLogin = string.Empty;
    int PRDDTransID = 0;
    int intErrCode = 0;
    public string strDateFormat = string.Empty;
    string strMode = string.Empty;
    int maxversion = 0;
    bool chkbox = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    bool bMod = false;
    DataTable dt = new DataTable();
    Dictionary<string, string> Procparam;
    string strRedirectPage = "../Origination/S3GOrgPDDTMaster_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgPDDTMaster_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgPDDTMaster_View.aspx';";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    Dictionary<string, string> dictParam = null;
    SerializationMode SerMode = SerializationMode.Binary;
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    PRDDCMgtServicesClient objPRDDT_MasterClient;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();

    //PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
    //PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable ObjS3G_PRDDTLookUpDataTable = new PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable();

    PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable ObjS3G_PRDDDocCategoryDataTable = null;
    PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable ObjS3G_PRDDTransMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable ObjS3G_PRDDTransDocMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable ObjS3G_ORG_PRDDCTransMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable ObjS3G_ORG_PRDDCTransDocMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable ObjS3G_ORG_ExitsPRDDCTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable();
    PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterRow ObjS3G_ORG_ExitsPRDDCTransRows;
    //added by  saranya 
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;
    public static Origination_S3GOrgPDDTMaster_Add obj_PageValue;
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {

        obj_PageValue = this;
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        strUserLogin = ObjUserInfo.ProUserLoginRW;
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end        
        txtDate.Attributes.Add("readonly", "readonly");
        //S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        txtDate.Text = DateTime.Today.ToString(strDateFormat);
        ProgramCode = "041";
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            PRDDTransID = Convert.ToInt32(fromTicket.Name);
            ViewState["PRDDTransID"] = PRDDTransID;
            strMode = Request.QueryString["qsMode"];
        }
        if (!IsPostBack)
        {
            if (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow)
            {
                FunPriLoadRefPoint();
                FunPriGetLookUpList();
            }
            if ((PRDDTransID > 0) && (strMode == "M"))
            {
                FunPriDisableControls(1);
            }
            else if ((PRDDTransID > 0) && (strMode == "Q"))
            {
                FunPriDisableControls(-1);
            }
            else
            {
                //  FunPriLoadRefPoint();
                //FunPriGetLookUpList();
                FunPriDisableControls(0);
            }
            // ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;

            // WF 
            if (PageMode == PageModes.WorkFlow)
            {
                ViewState["PageMode"] = PageModes.WorkFlow;
            }
            if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())
            {
                try
                {
                    PreparePageForWorkFlowLoad();
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                    Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
                }
            }
        }
        if (PageMode == PageModes.WorkFlow)
        {
            if (ViewState["PRDDTransID"] != null)
                PRDDTransID = Convert.ToInt32(ViewState["PRDDTransID"]);
        }

        //if (!IsPostBack)
        //{
        foreach (GridViewRow grvData in gvPRDDT.Rows)
        {
            Label myThrobber = (Label)grvData.FindControl("myThrobber");
            HiddenField hidThrobber = (HiddenField)grvData.FindControl("hidThrobber");

            if (hidThrobber.Value != "")
            {
                myThrobber.Text = hidThrobber.Value;
            }
        }
        //}

        //Values assgnment for Csutomer selection control

        if (ddlBranch.SelectedValue.ToString() != "0")
        {
            ucCustomerCodeLov.strBranchID = ddlBranch.SelectedValue.ToString();
        }
        else
        {
            ucCustomerCodeLov.strBranchID = "-1";
        }
        if (ddlLOB.SelectedValue.ToString() != "0")
        {
            ucCustomerCodeLov.strLOBID = ddlLOB.SelectedValue.ToString();
        }
        else
        {
            ucCustomerCodeLov.strLOBID = "-1";
        }
        if (ddlConstitition.Items.Count > 0 && ddlConstitition.SelectedValue.ToString() != "0")
        {
            ucCustomerCodeLov.strRegionID = ddlConstitition.SelectedValue.ToString();
        }
        else
        {
            ucCustomerCodeLov.strRegionID = "-1";
        }
        ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;

        TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        if (PageMode != PageModes.WorkFlow)
        {
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");
        }
        txt.ToolTip = "Customer Code";
    }
    #endregion
    #region "WF Code"
    void PreparePageForWorkFlowLoad()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowServiceClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

        try
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            byte[] byte_PreDisDoc = objWorkflowServiceClient.FunPubLoadPreDisDocTransaction(WFSessionValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValues.Document_Type);
            DataSet dsWorkflow = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_PreDisDoc, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));

            if (dsWorkflow.Tables.Count > 1)
            {
                if (dsWorkflow.Tables[1].Rows.Count > 0)
                {
                    PRDDTransID = Convert.ToInt32(dsWorkflow.Tables[1].Rows[0]["Doc_Id"].ToString());
                    ViewState["PRDDTransID"] = PRDDTransID;
                    FunPriDisableControls(1);
                }
            }
            else
            {
                if (dsWorkflow.Tables[0].Rows.Count > 0)
                {
                    ddlLOB.Items.Add(new ListItem(dsWorkflow.Tables[0].Rows[0]["LOB"].ToString(), dsWorkflow.Tables[0].Rows[0]["LOB_ID"].ToString()));
                    ddlLOB.ToolTip = dsWorkflow.Tables[0].Rows[0]["LOB"].ToString();
                    //ddlLOB.ClearDropDownList();

                    ddlBranch.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Location_ID"].ToString();
                    ddlBranch.SelectedText = dsWorkflow.Tables[0].Rows[0]["Location_Name"].ToString();
                    // ddlBranch.Clear();
                    //FunPriLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
                    ddlConstitition.Items.Add(new ListItem(dsWorkflow.Tables[0].Rows[0]["Constitution_Name"].ToString(), dsWorkflow.Tables[0].Rows[0]["Constitution_ID"].ToString()));
                    ddlConstitition.ToolTip = dsWorkflow.Tables[0].Rows[0]["Constitution_Name"].ToString();
                    // ddlConstitition.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Constitution_ID"].ToString();
                    //  ddlConstitition.ClearDropDownList();

                    //ddlRefPoint.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString();
                    //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), PRDDTransID);
                    ddlRefPoint.Items.Add(new ListItem(dsWorkflow.Tables[0].Rows[0]["Ref_Name"].ToString(), dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString()));
                    ddlRefPoint.ToolTip = dsWorkflow.Tables[0].Rows[0]["Ref_Name"].ToString();

                    lblEnquiry.CssClass = "styleReqFieldLabel";
                    if (dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString() == "1")
                    {
                        lblEnquiry.Text = "Enquiry No.";
                    }
                    else if (dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString() == "2")
                    {
                        lblEnquiry.Text = "Pricing No.";
                    }
                    else if (dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString() == "3")
                    {
                        lblEnquiry.Text = "Application No.";
                    }
                    else
                    {
                        lblEnquiry.Text = "Ref. Code";
                    }
                    // FunPriBindRefNo();
                    ddlEnquiry.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Document_Type_ID"].ToString();
                    ddlEnquiry.SelectedText = dsWorkflow.Tables[0].Rows[0]["Document_No"].ToString();
                    ddlEnquiry.ToolTip = dsWorkflow.Tables[0].Rows[0]["Document_No"].ToString();



                    //PopulateCustomerCode();
                    FunPriGetCustomerDetails();



                    // ddlRefPoint.ClearDropDownList();
                    //ddlEnquiry.Clear();
                    FunPriGetPRDDTransType();
                    tpPDDT.Enabled = true;
                    gvPRDDT.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objWorkflowServiceClient.Close();
        }
    }
    #endregion
    public void read()
    {
        txtCustomerID.ReadOnly = true;
        txtCustomerCode.ReadOnly = true;
        txtCustomerName.ReadOnly = true;
        txtStatus.ReadOnly = true;
        ddlLOB.Focus();
    }

    #region DDL
    private void FunPriGetLookUpList()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (PRDDTransID == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_Id", intUserId.ToString());
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Program_Id", Program_Id);

            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
            //if (strMode != "C" )
            //{
            //    ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code ", "Location_Name" });
            //}
            //ddlEnquiry.BindDataTable("S3G_ORG_GetPDDTEnquiryNumber", Procparam, new string[] { "Enquiry_Response_ID", "Enquiry_No" });


            //FunPriLoadCustomer();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriLoadCustomer()
    {
        Procparam = new Dictionary<string, string>();
        if (PRDDTransID == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }
        Procparam.Add("@User_Id", intUserId.ToString());
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Program_Id", Program_Id);
        Procparam.Add("@Type", "CUSTOMER");
        if (ddlRefPoint.SelectedIndex > 0)
            Procparam.Add("@Option", ddlRefPoint.SelectedValue);
        if (ddlEnquiry.SelectedValue != "0")
            Procparam.Add("@Ref_ID", ddlEnquiry.SelectedValue);

        ddlCustomerCode.BindDataTable("S3G_ORG_GetPDDTCustomer", Procparam, true, "-- Select --", new string[] { "Customer_ID", "Customer_Code" });


    }

    private void FunPriLoadRefPoint()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OptionValue", "1");
            DataTable dt = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", Procparam);
            DataRow[] dr = dt.Select("Value IN(1,2,3)", "Name", DataViewRowState.CurrentRows);
            dt = dr.CopyToDataTable();
            ddlRefPoint.BindDataTable(dt, new string[] { "Value", "Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlRefPoint_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // FunPriGetLookUpList();
            //tpPDDT.Enabled = false;
            gvPRDDT.Visible = false;
            //if (ddlEnquiry.Items.Count > 0)
            //    ddlEnquiry.Items.Clear();
            ddlEnquiry.Clear();
            ddlConstitition.Items.Clear();
            Clear();
            ddlBranch.Clear();
            FunProClearCustomerDetails(false);

            // FunPriLoadCustomer();
            //if (ddlConstitition.Items.Count > 0)
            //    ddlConstitition.Items.Clear();

            if (ddlRefPoint.SelectedIndex > 0)
            {
                ddlLOB.SelectedIndex = 0;
                lblEnquiry.CssClass = "styleReqFieldLabel";
                //rfvEnqNo.Enabled = true;
                if (ddlRefPoint.SelectedValue == "1")
                {
                    lblEnquiry.Text = "Enquiry No.";
                    ddlEnquiry.ErrorMessage = "Select Enquiry Number";
                }
                else if (ddlRefPoint.SelectedValue == "2")
                {
                    lblEnquiry.Text = "Pricing No.";
                    ddlEnquiry.ErrorMessage = "Select Pricing Number";
                }
                else if (ddlRefPoint.SelectedValue == "3")
                {
                    lblEnquiry.Text = "Application No.";
                    ddlEnquiry.ErrorMessage = "Select Application Number";
                }
                else
                {
                    lblEnquiry.Text = "Ref. Code";
                    // rfvEnqNo.Enabled = false;
                }

                //Shibu
                //if (ddlCustomerCode.SelectedIndex > 0)
                //{
                //    PopulateEnquiry();
                //}
                //else
                //{
                //    FunPriBindRefNo();
                //}
                //if (ddlEnquiry.Items.Count == 2)
                //{
                //    ddlEnquiry.SelectedValue = ddlEnquiry.Items[1].Value;
                //    FunPriGetCustomerDetails();
                //    FunPriGetPRDDTransType();
                //    tpPDDT.Enabled = true;
                //    gvPRDDT.Visible = true;
                //}
                //End



                //if (ddlCustomerCode.SelectedIndex != 0 && ddlEnquiry.Items.Count > 2)
                //{
                //    Dictionary<string, string> dictParam = new Dictionary<string, string>();
                //    dictParam.Add("@Customer_Id", ddlCustomerCode.SelectedValue.ToString());
                //    dictParam.Add("@Company_ID", intCompanyId.ToString());
                //    DataTable dtCustDetails = Utility.GetDefaultData("S3G_GetCustomerDetails", dictParam);

                //    if (dtCustDetails.Rows.Count > 0)
                //        fnBindCustomerDetails(dtCustDetails.Rows[0], "Customer");
                //}
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }
    //No use Ref No DDL this Control changed to Auto Suggestion By Shibu 
    private void FunPriBindRefNo()
    {
        // ddlEnquiry.Items.Clear();
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();



        //Procparam.Add("@Company_ID", intCompanyId.ToString());
        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
        //Procparam.Add("@Constitution_ID", ddlConstitition.SelectedValue);
        //Procparam.Add("@OptionValue", ddlRefPoint.SelectedValue);
        //ddlEnquiry.BindDataTable("S3G_ORG_GetPDDTRefDocNo", Procparam, new string[] { "RefDoc_ID", "RefDoc_No" });
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
            //if (ddlConstitition.SelectedValue != "")
            //    ddlConstitition.ClearDropDownList();



            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            {
                ddlBranch.Clear();
            }
            else
            {
                if (ddlBranch.SelectedValue != "0") ddlBranch.SelectedValue = "0";
            }

            //if (ddlEnquiry.SelectedValue != null)
            //    ddlEnquiry.Items.Clear();


            //Clear();
            // FunPriLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
            if (Convert.ToInt32(ddlLOB.SelectedValue) < 0)
            {
                //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
                //Dictionary<string, string> Procparam = new Dictionary<string, string>();
                //Procparam.Add("@Is_Active", "1");
                //Procparam.Add("@User_Id", intUserId.ToString());
                //Procparam.Add("@LOB_Id", ddlLOB.SelectedValue.ToString());
                //Procparam.Add("@Company_ID", intCompanyId.ToString());
                //Procparam.Add("@Program_Id", Program_Id);
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code ", "Location_Name" });
            }
            else
            {
                //if (ddlBranch.SelectedValue !="0") ddlBranch.SelectedValue= "0";
                //if (ddlConstitition.Items.Count > 0) ddlConstitition.SelectedIndex = 0;
                ddlBranch.Clear();
                ddlEnquiry.Clear();
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
                FunProClearCustomerDetails(false);
                ddlConstitition.Items.Clear();
                ucCustomerCodeLov.ButtonEnabled = false;
                //ddlEnquiry.SelectedIndex = 0;
            }
            ddlLOB.Focus();
            //if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            //{
            //    FunPriBindRefNo();
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }
    // No Need Constitution it's build Based On Customer( Each Customer have Only One Constitution)  By Shibu 
    private void FunPriLoadConstitution(int intLineofBusinessID)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            if (PageMode == PageModes.Create)
            {
                dictParam.Add("@Is_Active", "1");
            }
            dictParam.Add("@LOB_ID", intLineofBusinessID.ToString());
            ddlConstitition.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, dictParam, new string[] { "Constitution_ID", "ConstitutionName" });
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp, lblHeading.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }
    //Not Use
    protected void ddlConstitition_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlConstitition.SelectedValue) > 0)
            {
                //saranya
                //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), PRDDTransID);
                // FunPriBindRefNo();
            }
            else
            {
                //  if (ddlEnquiry.Items.Count > 0) ddlEnquiry.SelectedIndex = 0;
                //  Clear();
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriLoadEnquiryNumber(int intLineofBusinessID, int intBranchID, int intConstititionID, int PRDDTransID)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@LOB_ID", intLineofBusinessID.ToString());
            dictParam.Add("@Location_Id", intBranchID.ToString());
            dictParam.Add("@Constitution_ID", intConstititionID.ToString());
            dictParam.Add("@Document_Type", ddlRefPoint.SelectedValue);
            dictParam.Add("@PreDisbursement_Doc_Tran_ID", PRDDTransID.ToString());
            dictParam.Add("@Program_Id", Program_Id);
            //Shibu
            // ddlEnquiry.BindDataTable("S3G_ORG_GetEnquiryNumber", dictParam, new string[] { "ID", "Number" });
            ddlCustomerCode.BindDataTable("S3G_ORG_GetPRDDCustomerDropdown", dictParam, true, "-- Select --", new string[] { "Customer_ID", "Customer_Code" });

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriGetPRDDTransID()
    {
        try
        {
            Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            DataTable dtConstitution = new DataTable();
            if (ddlRefPoint.SelectedValue == "1")
            {

                Dictionary<string, string> dictparam2 = new Dictionary<string, string>();
                dictparam2.Add("@Enquiry_ID", ddlEnquiry.SelectedValue);
                dictparam2.Add("@Company_ID", intCompanyId.ToString());
                dtConstitution = Utility.GetDefaultData("S3G_ORG_GET_CONSTITUTION", dictparam2);

                ListItem lst = new ListItem(dtConstitution.Rows[0]["Constitution_Name"].ToString(), dtConstitution.Rows[0]["Constitution_ID"].ToString());
                ddlConstitition.Items.Add(lst);
            }

            dictParam1.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (ddlConstitition.Items.Count > 0 && ddlConstitition.SelectedValue != "0")
            {
                dictParam1.Add("@Constitution_ID", ddlConstitition.SelectedValue);
            }
            dictParam1.Add("@Company_ID", intCompanyId.ToString());
            dictParam1.Add("@Option", ddlRefPoint.SelectedValue);
            dictParam1.Add("@Ref_ID", ddlEnquiry.SelectedValue);

            DataTable dtDetails = Utility.GetDefaultData("S3G_ORG_GetPRDDTID", dictParam1);
            if (dtDetails.Rows.Count > 0)
            {
                DataRow dtRow = dtDetails.Rows[0];
                hidPRTID.Value = dtRow["PRDDC_ID"].ToString();
            }

            if (ddlRefPoint.SelectedValue == "1")
            {
                if (dtConstitution.Rows[0]["Customer_ID"] != null || dtConstitution.Rows[0]["Customer_ID"].ToString() != "")
                {
                    FunPriGetCustomerDetails();
                }
            }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriGetPRDDTransType()
    {
        if (hidPRTID.Value != "")
        {
            objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
            try
            {
                PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryRow ObjPRDDTypeTransRow;
                ObjS3G_PRDDDocCategoryDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable();
                ObjPRDDTypeTransRow = ObjS3G_PRDDDocCategoryDataTable.NewS3G_ORG_PRDDCDocumentCategoryRow();
                ObjPRDDTypeTransRow.Company_ID = intCompanyId;
                ObjPRDDTypeTransRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                ObjPRDDTypeTransRow.Constitution_ID = Convert.ToInt32(ddlConstitition.SelectedValue);
                ObjPRDDTypeTransRow.Option = Convert.ToInt32(ddlRefPoint.SelectedValue);
                ObjPRDDTypeTransRow.Ref_ID = Convert.ToInt32(ddlEnquiry.SelectedValue);
                if (hidPRTID.Value != "") ObjPRDDTypeTransRow.PRDDC_ID = Convert.ToInt32(hidPRTID.Value);
                ObjS3G_PRDDDocCategoryDataTable.AddS3G_ORG_PRDDCDocumentCategoryRow(ObjPRDDTypeTransRow);

                byte[] bytePRDDTypeTrans = objPRDDT_MasterClient.FunPubGetTypeTrans(SerMode, ClsPubSerialize.Serialize(ObjS3G_PRDDDocCategoryDataTable, SerMode));
                PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable dtPRDDTypeTrans = (PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable)ClsPubSerialize.DeSerialize(bytePRDDTypeTrans, SerMode, typeof(PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable));

                if (dtPRDDTypeTrans.Rows.Count > 0)
                {
                    gvPRDDT.DataSource = dtPRDDTypeTrans;
                    gvPRDDT.DataBind();
                    ViewState["dtPRDDTypeTrans"] = dtPRDDTypeTrans;
                    gvPRDDT.Visible = true;
                    tpPDDT.Visible = true;
                    ViewState["Docpath"] = dtPRDDTypeTrans.Rows[0]["Document_Path"].ToString();

                    for (int i = 0; i < gvPRDDT.Rows.Count; i++)
                    {
                        if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            //Commented and added by saranya for fixing observation on 01-Mar-2012
                            TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                            //Label txtScanBy = (Label)gvPRDDT.Rows[i].FindControl("txtScannedBy");
                            UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[i].FindControl("ddlScannedBy");
                            Label lblScannedBy = (Label)gvPRDDT.Rows[i].FindControl("lblScannedBy");
                            //End Here
                            ImageButton hyplnkView = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                            AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");
                            CheckBox CbxCheck = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");

                            if ((dtPRDDTypeTrans.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "false")
                                 || (dtPRDDTypeTrans.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "0"))
                            // && dtPRDDTypeTrans.Rows[i]["Is_Mandatory"].ToString() == "False")
                            {
                                //Commented and added by saranya for fixing observation on 01-Mar-2012
                                //txtScanDate.Enabled = ddlScannedBy.Enabled = false;
                                //txtScanDate.Text = "";
                                //ddlScannedBy.ClearDropDownList();
                                //txtScanDate.Visible = ddlScannedBy.Visible = false;
                                //End Here
                                asyFileUpload.Enabled = false;
                                hyplnkView.Enabled = false;
                                hyplnkView.CssClass = "styleGridEditDisabled";
                            }
                        }
                    }
                }
                else
                {
                    gvPRDDT.DataSource = null;
                    gvPRDDT.DataBind();
                    Utility.FunShowAlertMsg(this, "Document details not defined in Pre Disbursements Master");
                    strRedirectPageView = strRedirectPageAdd;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                    return;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Document details not defined in predisbursment master')", true);
                }
            }
            catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
            {
                lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
                lblErrorMessage.Text = ex.Message;
            }
            finally
            {
                objPRDDT_MasterClient.Close();
            }
        }
    }

    protected void ddlEnquiry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlEnquiry.SelectedValue != "0")
            {
                // PopulateCustomerCode();
                //FunPriGetCustomerDetails();
                FunPriGetPRDDTransID();
                FunPriGetPRDDTransType();
                tpPDDT.Visible = true;
                gvPRDDT.Visible = true;
                tpPDDT.Enabled = true;
            }
            else
            {
                //PopulateCustomerCode();
                // if (ddlConstitition.Items.Count > 0) ddlConstitition.SelectedIndex = 0;
                if (ddlLOB.Items.Count > 0)
                {
                    ddlLOB.SelectedIndex = 0;
                    ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
                }
                // if (ddlCustomerCode.Items.Count > 0) ddlCustomerCode.SelectedIndex = 0;
                if (ddlBranch.SelectedValue != "0") ddlBranch.SelectedValue = "0";
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
                ddlBranch.Clear();
                ddlEnquiry.Clear();
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
                FunProClearCustomerDetails(false);
                ddlConstitition.Items.Clear();
                Clear();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriGetCustomerDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Option", ddlRefPoint.SelectedValue);
            dictParam.Add("@Ref_ID", ddlEnquiry.SelectedValue);
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtCustDetails = Utility.GetDefaultData("S3G_ORG_GetCustomerDetails", dictParam);

            DataRow dtRow = dtCustDetails.Rows[0];
            // If Customer Dropdown is Empty to Load Customer values Added By Shibu
            //if (ddlCustomerCode.Items.Count <= 1)
            //{
            //    ddlCustomerCode.Items.Clear();
            //    ddlCustomerCode.Items.Add(new ListItem("--Select--", "0"));
            //    ddlCustomerCode.Items.Add(new ListItem(dtRow["Customer_Code"].ToString(), dtRow["Customer_ID"].ToString()));
            //}
            // End
            //Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            //dictParam1.Add("@Option", ddlRefPoint.SelectedValue);
            //dictParam1.Add("@Ref_ID", ddlEnquiry.SelectedValue);
            //dictParam1.Add("@Company_ID", intCompanyId.ToString());
            //if (PRDDTransID == 0)
            //{
            //    dictParam1.Add("@Is_Active", "1");
            //}
            //dictParam1.Add("@Program_Id", Program_Id);
            //dictParam1.Add("@User_ID", intUserId.ToString());

            //DataTable dtLBPDetails = Utility.GetDefaultData("S3G_ORG_GetLOBEnquiryNumber", dictParam1);
            //DataRow dtRow1 = dtLBPDetails.Rows[0];

            //ddlLOB.Items.Clear();
            //ddlLOB.Items.Add(new ListItem(dtRow1["LOB_Name"].ToString(), dtRow1["LOB_ID"].ToString()));
            //ddlLOB.SelectedValue = dtRow1["LOB_ID"].ToString();
            //ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;

            //FunPriLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
            //ddlBranch.Clear();
            ////ddlBranch.Items.Add(new ListItem(dtRow1["Location"].ToString(), dtRow1["Location_ID"].ToString()));
            //ddlBranch.SelectedValue = dtRow1["Location_ID"].ToString();
            //ddlBranch.SelectedText = dtRow1["Location"].ToString();





            //ddlConstitition.Items.Clear();
            //ddlConstitition.Items.Add(new ListItem(dtRow1["Constitution_Name"].ToString(), dtRow1["Constitution_ID"].ToString()));
            //ddlConstitition.SelectedValue = dtRow1["Constitution_ID"].ToString();
            if (ddlRefPoint.SelectedValue != "1")
            {
                if (Convert.ToUInt16(ddlConstitition.SelectedValue) > 0)
                    FunPriGetPRDDTransID();
            }
            fnBindCustomerDetails(dtRow, "Enquiry");



            if ((blnIsWorkflowApplicable && Session["DocumentNo"] != null) || ViewState["Mode"] == null)
            {
                txtStatus.Text = "Null";
            }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
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

    /// <summary>
    /// This methode used in bind the customer details
    /// </summary>
    #region Bind CustomerDetails
    private void fnBindCustomerDetails(DataRow dtRow, string Type)
    {
        try
        {
            txtCustomerName.Text = dtRow["Title"].ToString() + " " + dtRow["Customer_Name"].ToString();
            txtCustomerID.Text = dtRow["Customer_ID"].ToString();
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Text = dtRow["Customer_Code"].ToString();
            S3GCustomerCommAddress.SetCustomerDetails(dtRow, true);
            S3GCustomerPermAddress.SetCustomerDetails(dtRow, false);

            if (Type != "Customer")
            {
                txtProduct.Text = dtRow["Product_ID"].ToString();
                txtProductName.Text = dtRow["Product"].ToString();
                ddlCustomerCode.SelectedValue = dtRow["Customer_ID"].ToString();
                ((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value = dtRow["Customer_ID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }
    #endregion

    /// <summary>
    /// This methode used in SetCustomer Permanent Address in Textarea 
    /// </summary>
    #region SetCustomerPerAddress
    public string SetCustomerPerAddress(DataRow drCust)
    {
        string strAddress = "";
        try
        {
            if (drCust["Perm_Address1"].ToString() != "") strAddress += drCust["Perm_Address1"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_Address2"].ToString() != "") strAddress += drCust["Perm_Address2"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_City"].ToString() != "") strAddress += drCust["Perm_City"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_State"].ToString() != "") strAddress += drCust["Perm_State"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_Country"].ToString() != "") strAddress += drCust["Perm_Country"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_PINCode"].ToString() != "") strAddress += drCust["Perm_PINCode"].ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        return strAddress;
    }
    #endregion

    /// <summary>
    /// This methode used in Get PRDDTransaction details
    /// </summary>
    #region Get PRDDTrans
    private void FunGetPRDDTrans()
    {
        objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

        try
        {
            PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsRow ObjS3G_PRDDTransRowDetails;
            ObjS3G_ORG_PRDDCTransMasterDataTable = new PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable();
            ObjS3G_PRDDTransRowDetails = ObjS3G_ORG_PRDDCTransMasterDataTable.NewS3G_ORG_GetPRDDCTransDetailsRow();
            ObjS3G_PRDDTransRowDetails.PreDisbursement_Doc_Tran_ID = PRDDTransID;
            ObjS3G_PRDDTransRowDetails.Company_ID = intCompanyId;
            ObjS3G_ORG_PRDDCTransMasterDataTable.AddS3G_ORG_GetPRDDCTransDetailsRow(ObjS3G_PRDDTransRowDetails);

            byte[] bytePRDDTransDetails = objPRDDT_MasterClient.FunPubGetPRDDTrans(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCTransMasterDataTable, SerMode));
            ObjS3G_ORG_PRDDCTransMasterDataTable = (PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable)ClsPubSerialize.DeSerialize(bytePRDDTransDetails, SerMode, typeof(PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable));
            if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows.Count > 0)
            {
                txtPRDDC.Text = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["PRDDC_Number"].ToString();
                ddlRefPoint.Items.Add(new ListItem(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Ref_Name"].ToString(), ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString()));
                ddlRefPoint.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Ref_Name"].ToString();
                lblEnquiry.CssClass = "styleReqFieldLabel";
                if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString() == "1")
                {
                    lblEnquiry.Text = "Enquiry No.";
                }
                else if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString() == "2")
                {
                    lblEnquiry.Text = "Pricing No.";
                }
                else if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString() == "3")
                {
                    lblEnquiry.Text = "Application No.";
                }
                else
                {
                    lblEnquiry.Text = "Ref. Code";
                }
                ddlLOB.Items.Add(new ListItem(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["LOB"].ToString(), ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["LOB"].ToString();

                hidPRTID.Value = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["PRDDC_ID"].ToString();


                ddlBranch.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Location_Name"].ToString();
                ddlBranch.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Location_Name"].ToString();
                //ddlConstitition.Items.Clear();
                //FunPriLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
                ddlConstitition.Items.Add(new ListItem(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Constitution_Name"].ToString(), ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Constitution_ID"].ToString()));
                ddlConstitition.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Constitution_ID"].ToString();
                ddlConstitition.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Constitution_Name"].ToString();

                //saranya
                //ddlRefPoint.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type"].ToString();
                //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), PRDDTransID);
                //saranya

                //FunPriBindRefNo();
                //FunPriLoadRefPoint();

                txtProduct.Text = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Product_ID"].ToString();
                txtProductName.Text = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Product_Name"].ToString();
                ddlEnquiry.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type_ID"].ToString();
                ddlEnquiry.SelectedText = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Doc_Number"].ToString();
                ddlEnquiry.ToolTip = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Doc_Number"].ToString();
                fnBindCustomerDetails(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0], "Customer");

                //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), PRDDTransID);
                //ddlEnquiry.SelectedValue = ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Document_Type_ID"].ToString();

                //FunPriGetCustomerDetails(Convert.ToInt32(ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Enquiry_Response_ID"].ToString()));
                // FunPriGetCustomerDetails();

                if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Status"].ToString() == "1")
                {
                    txtStatus.Text = "Null";
                }
                else if (ObjS3G_ORG_PRDDCTransMasterDataTable.Rows[0]["Status"].ToString() == "2")
                {
                    txtStatus.Text = "Hold";
                }
                else
                {
                    txtStatus.Text = "Process";
                }
            }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objPRDDT_MasterClient.Close();
        }
    }

    /// <summary>
    /// This methode used in Get PRDDocument Transaction  details
    /// </summary>
    private void FunGetPRDDTransDocDetatils()
    {
        objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            //<<Performance>>
            //PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsRow ObjPRDDTransDocMasterRow;
            //ObjS3G_ORG_PRDDCTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable();
            //ObjPRDDTransDocMasterRow = ObjS3G_ORG_PRDDCTransDocMasterDataTable.NewS3G_ORG_GetPRDDCTransDocDetailsRow();
            //ObjPRDDTransDocMasterRow.PreDisbursement_Doc_Tran_ID = PRDDTransID;
            //ObjPRDDTransDocMasterRow.Company_ID = intCompanyId;
            //ObjPRDDTransDocMasterRow.Enquiry_Response_ID = Convert.ToInt32(ddlEnquiry.SelectedValue);
            //ObjPRDDTransDocMasterRow.Company_ID = intCompanyId;
            //ObjS3G_ORG_PRDDCTransDocMasterDataTable.AddS3G_ORG_GetPRDDCTransDocDetailsRow(ObjPRDDTransDocMasterRow);
            //byte[] bytePRDDTransDocDetails = objPRDDT_MasterClient.FunPubGetPRDDTransDoc(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCTransDocMasterDataTable, SerMode));
            //ObjS3G_ORG_PRDDCTransDocMasterDataTable = (PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable)ClsPubSerialize.DeSerialize(bytePRDDTransDocDetails, SerMode, typeof(PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable));

            //Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            ////dictParam1.Add("@Enquiry_Response_ID", Convert.ToString(ddlEnquiry.SelectedValue));
            //dictParam1.Add("@Company_ID", intCompanyId.ToString());
            //dictParam1.Add("@PreDisbursement_Doc_Tran_ID", PRDDTransID.ToString());
            //DataTable dtIsDetails = Utility.GetDefaultData("S3G_ORG_GetPRDDCTransDocIsDetails", dictParam1);
            //DataRow dtRow1 = dtIsDetails.Rows[0];

            //<<Performance>>
            Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
            dictParam1.Add("@Company_ID", intCompanyId.ToString());
            dictParam1.Add("@PreDisbursement_Doc_Tran_ID", PRDDTransID.ToString());
            dictParam1.Add("@Document_Type_ID", ddlEnquiry.SelectedValue);
            DataSet Dset = Utility.GetDataset("S3G_ORG_GetPRDDCTransDocDetails", dictParam1);

            DataTable dtGrid = Dset.Tables[0];
            DataTable dtIsDetails = Dset.Tables[1];

            ViewState["dtIsDetails"] = dtIsDetails;

            if (dtGrid.Rows.Count > 0)
            {
                ViewState["Docpath"] = dtGrid.Rows[0]["ViewDoc"].ToString();
            }
            gvPRDDT.DataSource = dtGrid;// Modify 
            gvPRDDT.DataBind();

            for (int i = 0; i < gvPRDDT.Rows.Count; i++)
            {
                if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    int PRDDC_Doc_Cat_ID = Convert.ToInt32(gvPRDDT.DataKeys[i]["PRDDC_Doc_Cat_ID"].ToString());
                    CheckBox Cbx1 = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");
                    //modified  and added by saranya for fixing observation on 01-Mar-2012
                    // DropDownList ddlScannedBy = (DropDownList)gvPRDDT.Rows[i].FindControl("ddlScannedBy");
                    UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[i].FindControl("ddlScannedBy");
                    TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                    Label lblScannedBy = (Label)gvPRDDT.Rows[i].FindControl("lblScannedBy");

                    UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[i].FindControl("ddlCollectedBy");
                    TextBox txtcolldate = (TextBox)gvPRDDT.Rows[i].FindControl("txtCollectedDate");
                    Label lblCollectedBy = (Label)gvPRDDT.Rows[i].FindControl("lblCollectedBy");
                    //end 

                    TextBox txtScanRef = (TextBox)gvPRDDT.Rows[i].FindControl("txtScan");
                    ImageButton Viewdoct = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                    TextBox txtRemarks = (TextBox)gvPRDDT.Rows[i].FindControl("txtRemarks");
                    Label lblDesc = (Label)gvPRDDT.Rows[i].FindControl("lblDesc");
                    TextBox txOD = (TextBox)gvPRDDT.Rows[i].FindControl("txOD");
                    TextBox txtScan = (TextBox)gvPRDDT.Rows[i].FindControl("txtScan");
                    Label lblColUser = (Label)gvPRDDT.Rows[i].FindControl("lblColUser");
                    Label lblPath = (Label)gvPRDDT.Rows[i].FindControl("lblPath");
                    Label myThrobber = (Label)gvPRDDT.Rows[i].FindControl("myThrobber");
                    HiddenField hidThrobber = (HiddenField)gvPRDDT.Rows[i].FindControl("hidThrobber");

                    if (!string.IsNullOrEmpty(lblPath.Text.Trim()))
                    {
                        if (lblPath.Text.Trim() == ViewState["Docpath"].ToString().Trim())
                        {
                            Viewdoct.Enabled = false;
                            Viewdoct.CssClass = "styleGridEditDisabled";
                        }
                        else
                        {
                            Viewdoct.CssClass = "styleGridEdit";
                        }
                    }
                    else
                    {
                        Viewdoct.Enabled = false;
                        Viewdoct.CssClass = "styleGridEditDisabled";
                    }

                    AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");

                    Label lblType = (Label)gvPRDDT.Rows[i].FindControl("lblType");

                    Cbx1.Checked = false;
                    if (PRDDC_Doc_Cat_ID == Convert.ToInt32(dtGrid.Rows[i]["PRDDC_Doc_Cat_ID"].ToString()))
                    {
                        //Commented and added by saranya
                        //txtScanBy.Text = Convert.ToString(ObjS3G_ORG_PRDDCTransDocMasterDataTable.Rows[i]["Scandedby"]);
                        if ((dtGrid.Rows[i]["Scanned_Date"].ToString()) != string.Empty)
                        {
                            txtScanDate.Text = Convert.ToDateTime(dtGrid.Rows[i]["Scanned_Date"].ToString()).ToString(strDateFormat);
                        }
                        else
                        {
                            txtScanDate.Text = "";
                        }

                        if ((Convert.ToString(dtGrid.Rows[i]["Scandedby"])) != string.Empty)
                        {
                            ddlScannedBy.SelectedValue = Convert.ToString(dtGrid.Rows[i]["Scanned_By"]);
                            //ddlScannedBy.ClearDropDownList();
                            lblScannedBy.Text = Convert.ToString(dtGrid.Rows[i]["Scanned_By"]);
                        }
                        else
                        {
                            ddlScannedBy.SelectedValue = "0";
                            lblScannedBy.Text = "";
                        }


                        if ((dtGrid.Rows[i]["Collected_Date"].ToString()) != string.Empty)
                        {
                            txtcolldate.Text = Convert.ToDateTime(dtGrid.Rows[i]["Collected_Date"].ToString()).ToString(strDateFormat);
                        }
                        else
                        {
                            txtcolldate.Text = "";
                        }
                        if ((Convert.ToString(dtGrid.Rows[i]["CollectedBy"])) != string.Empty)
                        {
                            ddlCollectedBy.SelectedValue = Convert.ToString(dtGrid.Rows[i]["Collected_By"]);
                            //ddlCollectedBy.ClearDropDownList();
                            lblCollectedBy.Text = Convert.ToString(dtGrid.Rows[i]["Collected_By"]);
                        }
                        else
                        {
                            ddlCollectedBy.SelectedValue = "0";
                            lblCollectedBy.Text = "";
                        }
                        //end
                        maxversion = Convert.ToInt32(dtGrid.Rows[i]["Version_No"]);


                        if (Convert.ToBoolean(dtGrid.Rows[i]["PRDDTrans"]) == true)
                        {
                            Cbx1.Checked = true;
                            Cbx1.Enabled = false;
                            //txtcollBy.Text = Convert.ToString(ObjS3G_ORG_PRDDCTransDocMasterDataTable.Rows[i]["CollectedBy"]);

                        }
                        //else
                        //{
                        //    txtcollBy.Text = ObjUserInfo.ProUserNameRW;
                        //    lblColUser.Text = "";
                        //}

                        MaxVerChk.Value += maxversion + "@@" + chkbox;

                        if (txtcolldate.Text == "")
                        {
                            txtcolldate.Text = "";
                            Cbx1.Checked = false;
                        }
                        MaxVerChk.Value += "@@" + txtScanDate.Text;
                        MaxVerChk.Value += "@@" + txtcolldate.Text;
                        txtScanRef.Text = Convert.ToString(dtGrid.Rows[i]["Scanned_Ref_No"]);
                        txtRemarks.Text = Convert.ToString(dtGrid.Rows[i]["Remarks"]);
                        MaxVerChk.Value += "@@" + txtRemarks.Text;

                        if (lblPath.Text.Trim() != ViewState["Docpath"].ToString().Trim())
                        {
                            string[] Path = Convert.ToString(dtGrid.Rows[i]["Scanned_Ref_No"]).Split('/');
                            //hidThrobber.Value = Path[Path.Length - 1].ToString();
                            myThrobber.Text = Path[Path.Length - 1].ToString();
                            txOD.Text = Convert.ToString(dtGrid.Rows[i]["Scanned_Ref_No"]);
                        }
                        //else
                        //{
                        //    txtScanBy.Text = ObjUserInfo.ProUserNameRW;                            
                        //}
                    }

                    if (dtGrid.Rows.Count == dtIsDetails.Rows.Count)
                    {
                        if ((dtIsDetails.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "false")
                            || (dtIsDetails.Rows[i]["Is_NeedScanCopy"].ToString() == "0"))
                        // && dtIsDetails.Rows[i]["Is_Mandatory"].ToString() == "False")
                        {
                            //txtScanDate.Enabled = txtScanBy.Enabled = false;
                            //txtScanDate.Text = "";
                            //txtScanBy.Text = "";
                            ddlScannedBy.Visible = txtScanDate.Visible = false;
                            Viewdoct.Enabled = false;
                            Viewdoct.CssClass = "styleGridEditDisabled";
                            asyFileUpload.Enabled = false;
                            myThrobber.Text = "";
                            //Cbx1.Enabled = false;
                        }
                    }
                    MaxVerChk.Value += "~~~";
                }
            }
        }
        //catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        //{
        //    lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        //}
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //if (objPRDDT_MasterClient != null)
            //objPRDDT_MasterClient.Close();
        }
    }
    #endregion

    protected void hyplnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvPRDDT", strFieldAtt);
            Label lblPath = (Label)gvPRDDT.Rows[gRowIndex].FindControl("lblPath");
            if (lblPath.Text.Trim() != ViewState["Docpath"].ToString().Trim())
            {
                string strFileName = lblPath.Text.Replace("\\", "/").Trim();
                string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "File not to be scanned yet");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }


    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }

    /// <summary>
    /// This methode used in Get XML form for PRDDT
    /// </summary>
    #region Save/ Clear/ Cancel
    protected string FunProFormMLAXML()
    {
        int enq = 1;

        string[] temprow = null;
        int Counts = 0;
        int rowcount = 0;
        int versionchk = 0;
        string strVersionNo = "1";
        StringBuilder strbuXML = new StringBuilder();
        if (!string.IsNullOrEmpty(MaxVerChk.Value))
        {
            temprow = (MaxVerChk.Value).Split('~', '~', '~');
            strVersionNo = Convert.ToString(temprow[0]).Substring(0, 1);
        }
        strbuXML.Append("<Root>");
        foreach (GridViewRow grvData in gvPRDDT.Rows)
        {
            int fileIndex = 1;

            string strlblPRTID = ((Label)grvData.FindControl("lblPRTID")).Text;
            string strScanBy = "";//Convert.ToString(intUserId);
            string strCollectdBy = "";
            //Modified by saranya
            //if(((Label)grvData.FindControl("lblColUser")).Text!="")
            //    strCollectdBy = ((Label)grvData.FindControl("lblColUser")).Text;
            //else
            //    strCollectdBy = Convert.ToString(intUserId);


            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion
            //if (((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedIndex > 0)

            //    strCollectdBy = ((Label)grvData.FindControl("lblCollectedBy")).Text;

            //else
            //    strCollectdBy = ((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedValue;

            //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
            if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue != "0")
            {
                if (((Label)grvData.FindControl("lblCollectedBy")).Text != "")
                {
                    strCollectdBy = ((Label)grvData.FindControl("lblCollectedBy")).Text;
                }
                else
                {
                    strCollectdBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
                }
            }
            else
            {
                strCollectdBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
            }
            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion
            //if (((Label)grvData.FindControl("lblScanUser")).Text != "")
            //    strScanBy = ((Label)grvData.FindControl("lblScanUser")).Text;
            //else
            //    strScanBy = Convert.ToString(intUserId);    
            //if (((DropDownList)grvData.FindControl("ddlScannedBy")).Visible)
            //{
            //    if (((DropDownList)grvData.FindControl("ddlScannedBy")).SelectedIndex > 0)
            //        strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
            //    else
            //        strScanBy = ((DropDownList)grvData.FindControl("ddlScannedBy")).SelectedValue;
            //}
            //else
            //{
            //    strScanBy = "-1";
            //}
            //Changed By Shibu 19-Sep-2013 to Add Auto Suggestion
            if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).Visible)
            {
                if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).SelectedValue != "0")
                    strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
                else
                    strScanBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).SelectedValue;
            }
            else
            {
                strScanBy = "0";
            }


            string strCollectdDate = ((TextBox)grvData.FindControl("txtCollectedDate")).Text;
            string strScanDate = "";
            if (((TextBox)grvData.FindControl("txtScannedDate")).Visible)
            {
                strScanDate = ((TextBox)grvData.FindControl("txtScannedDate")).Text;
            }
            //End here

            CheckBox CbxCheck = (CheckBox)grvData.FindControl("CbxCheck");
            AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)grvData.FindControl("asyFileUpload");

            if (PRDDTransID != 0)
            {
                if (asyFileUpload.FileName.ToString() != "")
                {
                    //strScanBy = Convert.ToString(intUserId);
                    strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
                    //strScanDate = DateTime.Now.ToString(strDateFormat).ToString();
                    strScanDate = ((TextBox)grvData.FindControl("txtScannedDate")).Text;
                }
            }
            string sCollectedOrWaived = ((DropDownList)grvData.FindControl("ddlCollAndWaiver")).SelectedValue;
            string strScanRefNo = ((TextBox)grvData.FindControl("txOD")).Text;
            string strRemarks = ((TextBox)grvData.FindControl("txtRemarks")).Text.Replace("'", "\"").Replace(">", "").Replace("<", "").Replace("&", "");
            string strPRDDTrans = Convert.ToString(((CheckBox)grvData.FindControl("CbxCheck")).Checked);
            string[] temp;
            if (!string.IsNullOrEmpty(MaxVerChk.Value))
            {
                temp = Convert.ToString(temprow[rowcount]).Split('@', '@');
                maxversion = Convert.ToInt32(temp[0]);
                chkbox = Convert.ToBoolean(temp[2]);
                if (strPRDDTrans.ToLower() == Convert.ToString(chkbox).ToLower())
                {
                    //{ strVersionNo = Convert.ToString(maxversion);
                    Counts = 0;
                }
                else
                {

                    versionchk = versionchk + 1;
                    Counts = 1;
                    bMod = true;
                }
                if (temp[4].ToString() == strScanDate.ToString() && temp[6].ToString() == strCollectdDate.ToString() && temp[8].ToString() == strRemarks.ToString())
                {
                    //Counts = 0;
                }
                else
                {
                    //Counts = 1;
                    bMod = true;
                }
                if (versionchk > 0)
                {
                    maxversion = maxversion + 1;
                    strVersionNo = Convert.ToString(maxversion);
                }
            }
            strScanDate = strScanDate == string.Empty ? strScanDate : Utility.StringToDate(strScanDate).ToString();
            strCollectdDate = strCollectdDate == string.Empty ? strCollectdDate : Utility.StringToDate(strCollectdDate).ToString();//Utility.StringToDate(DateTime.Now.ToString(strDateFormat)).ToString()
            strbuXML.Append(" <Details  PRDDC_Doc_Cat_ID='" + strlblPRTID + "' Collected_By='" + strCollectdBy.ToString() + "' Collected_Date='" + strCollectdDate +
                "' Scanned_By='" + strScanBy.ToString() + "' Scanned_Date='" + strScanDate + "' Scanned_Ref_No='" + strScanRefNo.ToString() + "' Remarks='" + strRemarks.ToString() + "' PRDDTrans='" + strPRDDTrans.ToString() + "' Version_No='" + "' Counts='" + Counts.ToString() + "'  Doc_CollectOrWaived='" + sCollectedOrWaived + "'/>"); // Added By Shibu to Update Collected / Waived Values 6-Jun-2013
            rowcount = rowcount + 3;

        }
        string tem = "Version_No='" + strVersionNo + "'";
        strbuXML.Replace("Version_No=''", tem);
        strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }
    #endregion

    /// <summary>
    /// This methode used in Insert and Update for PRDDT details
    /// </summary>
    #region Save and Update PRDDT
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string strPRDDT_No = string.Empty;
        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=PRT';";
        string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgPDDTMaster_Add.aspx';";

        objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

        try
        {
            DataTable dt1 = new DataTable();
            PRDDTransID = Convert.ToInt32(ViewState["PRDDTransID"]);
            if (PRDDTransID > 0)
            {
                dt1 = (DataTable)ViewState["dtIsDetails"];
            }
            else
            {
                dt1 = (DataTable)ViewState["dtPRDDTypeTrans"];
                if ((DataTable)ViewState["dtPRDDTypeTrans"] == null)
                {
                    lblErrorMessage.Text = "";
                    Utility.FunShowAlertMsg(this, "Document details not defined in Pre Disbursements Master");
                    strRedirectPageView = strRedirectPageAdd;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                    return;
                }
            }

            int counts = 0;
            int Length = gvPRDDT.Rows.Count;

            for (int i = 0; i < gvPRDDT.Rows.Count; i++)
            {
                if (gvPRDDT.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox txtScanDate = (TextBox)gvPRDDT.Rows[i].FindControl("txtScannedDate");
                    ImageButton hyplnkView = (ImageButton)gvPRDDT.Rows[i].FindControl("hyplnkView");
                    AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPRDDT.Rows[i].FindControl("asyFileUpload");
                    CheckBox CbxCheck = (CheckBox)gvPRDDT.Rows[i].FindControl("CbxCheck");
                    Label lblType = (Label)gvPRDDT.Rows[i].FindControl("lblType");
                    Label lblProgramName = (Label)gvPRDDT.Rows[i].FindControl("lblProgramName");
                    HiddenField hidThrobber = (HiddenField)gvPRDDT.Rows[i].FindControl("hidThrobber");
                    Label myThrobber = (Label)gvPRDDT.Rows[i].FindControl("myThrobber");
                    //asyFilepath.Text = asyFileUpload.FileName;
                    //Added by Senthil on Apr 16th 2012 Begin

                    DropDownList ddlCollAndWaiver = (DropDownList)gvPRDDT.Rows[i].FindControl("ddlCollAndWaiver");

                    if (CbxCheck.Checked)
                    {
                        //DropDownList ddlCollectedBy = gvPRDDT.Rows[i].FindControl("ddlCollectedBy") as DropDownList;
                        //DropDownList ddlScannedBy = gvPRDDT.Rows[i].FindControl("ddlScannedBy") as DropDownList;
                        UserControls_S3GAutoSuggest ddlCollectedBy = gvPRDDT.Rows[i].FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;
                        UserControls_S3GAutoSuggest ddlScannedBy = gvPRDDT.Rows[i].FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;
                        if (ddlCollectedBy.SelectedValue == "0")
                        {
                            Utility.FunShowAlertMsg(this, " Select Collected / Waived By  ");
                            return;
                        }

                        if (asyFileUpload.Enabled && ddlScannedBy.SelectedValue == "0")
                        {
                            if (ddlCollAndWaiver.SelectedValue == "C") // Added By Shibu 6-Jun-2013 Condition only for Collected
                            {
                                Utility.FunShowAlertMsg(this, " Select Scanned By  ");
                                return;
                            }
                        }


                    }
                    //End Here

                    if (lblProgramName.Text.Trim() != "")
                    {
                        if ((dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "true" ||
                                dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "1")
                            && CbxCheck.Checked == false)
                        {
                            if (ddlCollAndWaiver.SelectedValue == "C")
                            {
                                Utility.FunShowAlertMsg(this, "Pre Disbursements Document has to be collected for document type - " + lblType.Text.Trim().ToUpper());
                                return;
                            }
                        }
                        if ((dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "true"
                            && dt1.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "true") ||

                            (dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "1"
                            && dt1.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "1"))
                        {
                            if (myThrobber.Text.Trim() == "")
                            {
                                if (ddlCollAndWaiver.SelectedValue == "C")
                                {
                                    Utility.FunShowAlertMsg(this, "Pre Disbursements Document has to be scanned for document type - " + lblType.Text.Trim().ToUpper());
                                    return;
                                }
                            }
                        }
                    }

                    if (hidThrobber.Value.Trim() != "")
                    {
                        string fileExtension = asyFileUpload.FileName.Substring(asyFileUpload.FileName.LastIndexOf('.') + 1);
                        if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                        {
                            cvPRDTT.ErrorMessage = "File extension not supported, only image & pdf files should be uploaded.";
                            cvPRDTT.IsValid = false;
                            return;
                        }
                    }

                    // Modified By Senthilkumar P , 24/Mar/2012
                    // Reg. Document status not selected in Pre. Dis. Documents.

                    // Validation Begin

                    if (((dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "false"
                            && dt1.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "false") ||

                            (dt1.Rows[i]["Is_Mandatory"].ToString().ToLower() == "0"
                            && dt1.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "0"))
                        && (!(CbxCheck.Checked)))
                        counts++;

                    // Validation End...

                    if (CbxCheck.Checked) counts++;
                }
            }

            if (Length == counts)
                txtStatus.Text = "Process";
            else if (counts < Length)
                txtStatus.Text = "Hold";
            else
                txtStatus.Text = "Null";

            int intRowindex = 0;
            foreach (GridViewRow grv in gvPRDDT.Rows)
            {
                TextBox txOD = grv.FindControl("txOD") as TextBox;
                //txOD.Text = ViewState["Docpath"].ToString().Trim();
                //Commented and added by saranya
                //Label txtCollected = grv.FindControl("txtColletedDate") as Label;
                //Label txtCollectedBy = grv.FindControl("txtColletedBy") as Label;
                //Label txtScaned = grv.FindControl("txtScannedDate") as Label;
                //Label txtScanedBy = grv.FindControl("txtScannedBy") as Label;
                //TextBox txtScan = grv.FindControl("txtScan") as TextBox;
                TextBox txtCollected = grv.FindControl("txtColletedDate") as TextBox;
                // DropDownList ddlCollectedBy = grv.FindControl("ddlCollectedBy") as DropDownList;
                UserControls_S3GAutoSuggest ddlCollectedBy = grv.FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;

                Label lblCollectedBy = grv.FindControl("lblCollectedBy") as Label;
                TextBox txtScaned = grv.FindControl("txtScannedDate") as TextBox;

                // DropDownList ddlScannedBy = grv.FindControl("ddlScannedBy") as DropDownList;
                UserControls_S3GAutoSuggest ddlScannedBy = grv.FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;

                Label lblScannedBy = grv.FindControl("lblScannedBy") as Label;
                TextBox txtScan = grv.FindControl("txtScan") as TextBox;
                //End Here
                TextBox txtRemark = grv.FindControl("txtRemarks") as TextBox;
                ImageButton HypLnk = (ImageButton)grv.FindControl("hyplnkView");
                HiddenField hidThrobber = (HiddenField)grv.FindControl("hidThrobber");
                DropDownList ddlCollAndWaiver = (DropDownList)grv.FindControl("ddlCollAndWaiver"); // Added by Shibu 4-Jun-2013
                AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");

                string strPath = "";
                string strNewFileName = AsyncFileUpload1.FileName;
                string strEnqNumber = ddlEnquiry.SelectedText.Replace("/", "-");

                if (AsyncFileUpload1.FileName != "" && hidThrobber.Value.Trim() != "")
                {
                    if (AsyncFileUpload1.HasFile)
                    {
                        if (ViewState["Docpath"].ToString() != "")
                        {
                            strPath = Path.Combine(ViewState["Docpath"].ToString(), "COMPANY" + intCompanyId.ToString() + "/" + strEnqNumber + "/" + "PDDTC-" + intRowindex.ToString());
                            if (Directory.Exists(strPath))
                            {
                                Directory.Delete(strPath, true);
                            }
                            Directory.CreateDirectory(strPath);
                            strPath = strPath + "/" + strNewFileName;
                        }
                        txOD.Text = strPath;// txOD.Text + strEnqNumber + "\\" + "PDDTC-" + intRowindex.ToString() + "\\" + strNewFileName;

                        FileInfo f1 = new FileInfo(strPath);

                        if (f1.Exists == true)
                            f1.Delete();

                        AsyncFileUpload1.SaveAs(strPath);
                    }
                }

                //if (((CheckBox)grv.FindControl("CbxCheck")).Checked)
                //{                    
                //    if (txtRemark.Text.Length == 0)
                //    {
                //        cvPRDTT.ErrorMessage = "Enter the Remarks";                        
                //        cvPRDTT.IsValid = false;
                //        return;
                //    }
                //}
                intRowindex++;
            }

            ObjS3G_PRDDTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterRow objPRDDTransDocRow;
            ObjS3G_PRDDTransMasterDataTable = new PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PPDDTransMasterRow objPRDDTransRow;
            objPRDDTransRow = ObjS3G_PRDDTransMasterDataTable.NewS3G_ORG_PPDDTransMasterRow();
            objPRDDTransRow.Company_ID = intCompanyId;
            PRDDTransID = Convert.ToInt32(ViewState["PRDDTransID"]);
            objPRDDTransRow.PreDisbursement_Doc_Tran_ID = PRDDTransID;
            objPRDDTransRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objPRDDTransRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objPRDDTransRow.Constitution_ID = Convert.ToInt32(ddlConstitition.SelectedValue);
            objPRDDTransRow.Document_Type = Convert.ToInt32(ddlRefPoint.SelectedValue);
            objPRDDTransRow.Enquiry_Response_ID = Convert.ToInt32(ddlEnquiry.SelectedValue);
            objPRDDTransRow.PRDDC_ID = Convert.ToInt32(hidPRTID.Value);
            //objPRDDTransRow.Customer_ID = Convert.ToInt64(txtCustomerID.Text);
            if (((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value != null && ((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value != "" && ((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value != "0")
            {
                objPRDDTransRow.Customer_ID = Convert.ToInt64(txtCustomerID.Text);
            }
            if (PRDDTransID == 0)
            {
                objPRDDTransRow.PRDDC_Date = Utility.StringToDate(txtDate.Text);
            }
            else
            {
                objPRDDTransRow.PRDDC_Date = DateTime.Now;
            }

            if (txtStatus.Text == "Null")
            {
                objPRDDTransRow.Status = "1";
            }
            else if (txtStatus.Text == "Hold")
            {
                objPRDDTransRow.Status = "2";
            }
            else
            {
                objPRDDTransRow.Status = "3";
            }

            objPRDDTransRow.Created_By = intUserId;
            objPRDDTransRow.Created_On = DateTime.Now;
            objPRDDTransRow.Modified_By = intUserId;
            objPRDDTransRow.Modified_On = DateTime.Now;
            if (PRDDTransID == 0)
            {
                objPRDDTransRow.PRDDC_Number = "0";
            }
            else
            {
                objPRDDTransRow.PRDDC_Number = txtPRDDC.Text;
            }
            objPRDDTransRow.XML_PRDDTDeltails = FunProFormMLAXML();
            ObjS3G_PRDDTransMasterDataTable.AddS3G_ORG_PPDDTransMasterRow(objPRDDTransRow);
            byte[] ObjPRDDTDataTable = ClsPubSerialize.Serialize(ObjS3G_PRDDTransMasterDataTable, SerMode);

            if (PRDDTransID > 0)
            {
                intErrCode = objPRDDT_MasterClient.FunPubModifyPRDDTransMaster(SerMode, ObjPRDDTDataTable);
            }
            else
            {
                intErrCode = objPRDDT_MasterClient.FunPubCreatePRDDTransMaster(out strPRDDT_No, SerMode, ObjPRDDTDataTable);

            }

            if (intErrCode == 0)
            {
                if (PRDDTransID > 0)
                {
                    strPRDDT_No = txtPRDDC.Text;
                    //strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Pre Disbursements Document details updated successfully");
                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler)                       
                    {
                        try
                        {
                            WorkFlowSession WFValues = new WorkFlowSession();
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, int.Parse(ddlRefPoint.SelectedValue));
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow not Assigned";
                            }
                            ShowWFAlertMessage(strPRDDT_No, ProgramCode, strAlert);
                            return;
                        }
                        catch
                        {
                            strAlert = "Pre Disbursements Document Transaction details updated successfull";

                        }
                    }
                }
                else
                {
                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler)                       
                    {
                        try
                        {
                            WorkFlowSession WFValues = new WorkFlowSession();
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, int.Parse(ddlRefPoint.SelectedValue));

                                strAlert = "";

                                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                btnSave.Enabled = false;
                                //End here
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow not Assigned";
                            }
                            ShowWFAlertMessage(strPRDDT_No, ProgramCode, strAlert);
                            return;
                        }
                        catch
                        {
                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here

                            strAlert = "Pre Disbursements Document Transaction " + strPRDDT_No + " details added successfully";
                            strAlert += @"\n\n And Job not assigned to the next user.";
                            strAlert += @"\n\nWould you like to add one more record?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            ddlLOB.Focus();
                        }
                    }
                    else
                    {
                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(null, ddlEnquiry.SelectedText.Trim(), ProgramCode, null, null, "O", CompanyId, int.Parse(ddlLOB.SelectedValue), null, null, txtProductName.Text.Trim(), out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), ddlEnquiry.SelectedText.Trim(), int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), int.Parse(ddlRefPoint.SelectedValue));
                        }

                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strAlert = "Pre Disbursements Document Transaction " + strPRDDT_No + " details added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        ddlLOB.Focus();
                    }
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Pre Disbursements Document Transaction details already exists");
            }
            else if (intErrCode == -1)
            {
                if (PRDDTransID == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    strRedirectPageView = "";
                    ddlLOB.Focus();
                }
            }
            else if (intErrCode == -2)
            {
                if (PRDDTransID == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    strRedirectPageView = "";
                    ddlLOB.Focus();
                }
            }
            ddlLOB.Focus();

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Access to the path") && ex.Message.Contains("denied"))
            {
                cvPRDTT.ErrorMessage = "File Path not formed well or Access to the path is denied";
                cvPRDTT.IsValid = false;
                return;
            }
            else if (ex.Message.Contains("Could not find a part of the path"))
            {
                cvPRDTT.ErrorMessage = "File Path defined in Pre Disbursements Document Master is not avilable";
                cvPRDTT.IsValid = false;
                return;
            }
            else
            {
                cvPRDTT.ErrorMessage = "File Path defined in Pre Disbursements Document Master is not avilable ";//ex.Message;
                cvPRDTT.IsValid = false;
                return;
            }
        }
        finally
        {
            //if (objPRDDT_MasterClient != null)
            objPRDDT_MasterClient.Close();
        }
    }
    #endregion

    /* WorkFlow Properties */
    private int WFLOBId { get { return int.Parse(ddlLOB.SelectedValue); } }
    private int WFProduct { get { return 3; } }    /// <summary>
    /// This methode used in Clear the all enterable data
    /// </summary>
    #region Clear
    public void Clear()
    {
        try
        {
            txtCustomerID.Text = "";
            txtCustomerCode.Text = "";
            txtCustomerName.Text = "";
            txtProduct.Text = "";
            txtProductName.Text = "";
            txtStatus.Text = "";
            S3GCustomerCommAddress.ClearCustomerDetails();
            S3GCustomerPermAddress.ClearCustomerDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }

    }
    #endregion

    /// <summary>
    /// This methode used in Cancel button event
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //wf Cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect("../Origination/S3GORGTransLander.aspx?Code=PRT");
    }

    /// <summary>
    /// This methode used in Tab Index Change event
    /// </summary>
    protected void tcPDDT_ActiveTabChanged(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
    }

    /// <summary>
    /// This methode used in Clear the value
    /// </summary>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlConstitition.Items.Clear();// SelectedIndex = -1;
            //ddlConstitition.ClearDropDownList();
            Clear();
            gvPRDDT.Visible = false;
            FunPriGetLookUpList();
            ddlBranch.Clear();
            ddlRefPoint.ClearSelection();
            //if (ddlEnquiry.I 0) ddlEnquiry.Items.Clear();
            ddlEnquiry.Clear();
            //if (ddlCustomerCode.SelectedIndex > 0)
            //ddlCustomerCode.SelectedIndex = -1;
            //ddlCustomerCode.ClearDropDownList();
            //txtCustomerCode.Text = string.Empty;
            //txtCustomerID.Text = string.Empty;
            FunProClearCustomerDetails(false);
            lblErrorMessage.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// This methode used  Create ,Modify and Query [User Role Access]
    /// </summary>
    #region Role Access Setup
    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                read();
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                txtPRDDC.Enabled = false;
                ddlLOB.Focus();
                tpPDDT.Enabled = true;
                gvPRDDT.Visible = true;
                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                read();
                //txtPRDDC.Enabled = false;
                //ddlLOB.Enabled = ddlBranch.Enabled = ddlRefPoint.Enabled = false;
                //ddlConstitition.Enabled = ddlCustomerCode.Enabled = ddlEnquiry.Enabled = false;
                //txtDate.ReadOnly = true;
                //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                //btnClear.Enabled = false;
                //ucCustomerCodeLov.ButtonEnabled = false;
                //TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                //txt.Enabled = false;
                //txtCustomerName.Enabled = false;
                //tpPDDT.Enabled = true;

                // ddlRefPoint.ClearDropDownList();
                ucCustomerCodeLov.ButtonEnabled = false;
                // TextBox txt1 = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                // txt1.Enabled = false;
                txtPRDDC.ReadOnly = true;
                ddlEnquiry.ReadOnly = true;
                ddlBranch.ReadOnly = true;
                txtPRDDC.Enabled = true;
                btnClear.Enabled = false;
                //   btnSave.Enabled = false;
                FunGetPRDDTrans();
                //FunPriGetPRDDTransType();
                FunGetPRDDTransDocDetatils();

                if (ViewState["PageMode"] == null)
                {
                    FunPriDeletePRDDT();
                }

                break;

            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                // ddlRefPoint.ClearDropDownList();
                ucCustomerCodeLov.ButtonEnabled = false;
                //  TextBox txt1 = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                // txt1.Enabled = false;
                txtPRDDC.ReadOnly = true;
                ddlEnquiry.ReadOnly = true;
                ddlBranch.ReadOnly = true;
                txtPRDDC.Enabled = true;
                btnClear.Enabled = false;
                btnSave.Enabled = false;

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                read();
                FunGetPRDDTrans();
                //FunPriGetPRDDTransType();
                FunGetPRDDTransDocDetatils();

                gvPRDDT.Columns[11].Visible = false;
                gvPRDDT.FooterRow.Visible = false;
                gvPRDDT.Columns[gvPRDDT.Columns.Count - 1].Visible = false;
                foreach (GridViewRow grv in gvPRDDT.Rows)
                {
                    //  DropDownList ddlCollectedBy = (DropDownList)grv.FindControl("ddlCollectedBy");
                    UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)grv.FindControl("ddlCollectedBy");
                    DropDownList ddlCollAndWaiver = (DropDownList)grv.FindControl("ddlCollAndWaiver");
                    TextBox TxtCollectedDt = (TextBox)grv.FindControl("txtCollectedDate");

                    // DropDownList ddlScannedBy = (DropDownList)grv.FindControl("ddlScannedBy");
                    UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)grv.FindControl("ddlScannedBy");

                    TextBox TxtScannedDt = (TextBox)grv.FindControl("txtScannedDate");
                    TextBox TxtScanRef = (TextBox)grv.FindControl("txtScan");
                    TextBox TxtRemarks = (TextBox)grv.FindControl("txtRemarks");
                    CheckBox CkBox = (CheckBox)grv.FindControl("CbxCheck");
                    ImageButton hyplnkView = (ImageButton)grv.FindControl("hyplnkView");
                    Label myThrobber = (Label)grv.FindControl("myThrobber");
                    AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");
                    AjaxControlToolkit.CalendarExtender DtCollect = (AjaxControlToolkit.CalendarExtender)grv.FindControl("calCollectedDate");
                    AjaxControlToolkit.CalendarExtender DtScan = (AjaxControlToolkit.CalendarExtender)grv.FindControl("calScannedDate");
                    TxtScanRef.ReadOnly = true;
                    TxtRemarks.ReadOnly = true;
                    CkBox.Enabled = false;   //myThrobber.Visible 
                    TxtCollectedDt.ReadOnly = true;
                    asyFileUpload.Enabled = false;
                    TxtScannedDt.ReadOnly = true;
                    DtCollect.Enabled = false;
                    DtScan.Enabled = false;
                    ddlCollectedBy.ReadOnly = true;
                    ddlScannedBy.ReadOnly = true;
                    ddlCollAndWaiver.Enabled = false;
                    //if (myThrobber.Text.Trim() == "")
                    //{
                    ////    TxtScannedDt.Text  = TxtScannedBy.Text = "";    
                    //    TxtScannedDt.Visible = ddlScannedBy.Visible = false;
                    //}
                    //if (!CkBox.Checked)
                    //{
                    //    //TxtCollectedBy.Text = TxtCollectedDt.Text = "";
                    //    ddlCollectedBy.Visible = TxtCollectedDt.Visible = false;
                    //}
                }

                if (bClearList)
                {
                    //  ddlLOB.ClearDropDownList();
                    // ddlBranch.Clear();
                    // ddlConstitition.ClearDropDownList();
                    //ddlEnquiry.ClearDropDownList();
                    //ddlCustomerCode.ClearDropDownList();
                }
                break;
        }

    }
    #endregion

    /// <summary>
    /// This methode used  Covert date format
    /// </summary>
    #region Date Format
    private string ConvertToCurrentFormat(string strDate)
    {
        //  string dT = strDate;

        try
        {
            if (strDate.Contains("1900"))
                strDate = string.Empty;


            strDate = strDate.Replace("12:00:00 AM", "");

            CultureInfo myDTFI = new CultureInfo("en-GB", true);
            DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
            DTF.ShortDatePattern = "dd/MM/yyyy";
            DateTime _Date = new DateTime();
            if (strDate != "")
            {
                _Date = System.Convert.ToDateTime(strDate, DTF);
                return _Date.ToString("dd/MM/yyyy");
            }
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            return strDate;
            // throw ex;
        }
    }

    private void FunPriLoadEnquiryDetailsFromWorkflow()
    {
        try
        {

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                //int strConstitution = ddlConstitition.SelectedValue == "" ? 0 : Convert.ToInt32(ddlConstitition.SelectedValue);
                //saranya
                //FunPriLoadEnquiryNumber(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), strConstitution , PRDDTransID);

                //if (ddlConstitition.Items.Count > 0) ddlConstitition.SelectedIndex = 0;
                // FunPriBindRefNo();

                //if (ddlEnquiry.Items.Count > 0) ddlEnquiry.SelectedIndex = 0;
                ucCustomerCodeLov.ButtonEnabled = true;
            }
            else
            {
                ddlConstitition.Items.Clear();
                if (ddlEnquiry.SelectedValue != "0") ddlEnquiry.SelectedValue = "0";
                Clear();
                FunProClearCustomerDetails(false);
                tpPDDT.Enabled = false;
                gvPRDDT.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }


    #region Exits PRDDTransaction
    private void FunPriDeletePRDDT()
    {
        objPRDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

        try
        {
            ObjS3G_ORG_ExitsPRDDCTransRows = ObjS3G_ORG_ExitsPRDDCTransDocMasterDataTable.NewS3G_ORG_ExitsPRDDTMasterRow();
            ObjS3G_ORG_ExitsPRDDCTransRows.Company_ID = intCompanyId;
            ObjS3G_ORG_ExitsPRDDCTransRows.Document_Type = Convert.ToInt32(ddlRefPoint.SelectedValue);
            ObjS3G_ORG_ExitsPRDDCTransRows.PreDisbursement_Doc_Tran_ID = PRDDTransID;
            ObjS3G_ORG_ExitsPRDDCTransDocMasterDataTable.AddS3G_ORG_ExitsPRDDTMasterRow(ObjS3G_ORG_ExitsPRDDCTransRows);
            SerializationMode SerMode = SerializationMode.Binary;

            intErrCode = objPRDDT_MasterClient.FunPubExitsPRDDTrans(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_ExitsPRDDCTransDocMasterDataTable, SerMode));
            if (intErrCode > 0)
            {
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                Utility.FunShowAlertMsg(this, "Pre Disbursements Document Transaction details used in transaction");
                return;
            }

            //objPRDDT_MasterClient.Close();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objPRDDT_MasterClient.Close();
        }
    }
    #endregion

    private void FunPriMoveFile(string strFilePath, string strNewPath)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(strFilePath);
            FileInfo fileNewInfo = new FileInfo(strNewPath);
            if (!Directory.Exists(fileNewInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileNewInfo.DirectoryName);
            }
            if (File.Exists(strNewPath))
            {
                File.Delete(strNewPath);
            }
            fileInfo.MoveTo(strNewPath);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvPRDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        try
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    ObjStatus.Option = 35;
            //    ObjStatus.Param1 = intCompanyId.ToString();
            //    ViewState["UserDetails"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblPath = (Label)e.Row.FindControl("lblPath");
                Label lblmyThrobber = (Label)e.Row.FindControl("myThrobber");

                //Added by saranya
                //DropDownList ddlCollectedby = (DropDownList)e.Row.FindControl("ddlCollectedby");

                //Changed By Shibu 19-Sep-2013 to Add Auto Suggestion
                UserControls_S3GAutoSuggest ddlCollectedby = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlCollectedby");

                AjaxControlToolkit.CalendarExtender calCollectedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calCollectedDate");
                AjaxControlToolkit.CalendarExtender calScannedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calScannedDate");
                calScannedDate.Format = calCollectedDate.Format = strDateFormat;
                TextBox txtColletedDate = (TextBox)e.Row.FindControl("txtCollectedDate");
                //end
                ImageButton Viewdoct = (ImageButton)e.Row.FindControl("hyplnkView");
                CheckBox Cbx1 = (CheckBox)e.Row.FindControl("CbxCheck");
                //DropDownList ddlScannedby = (DropDownList)e.Row.FindControl("ddlScannedby");
                TextBox txtScannedDate = (TextBox)e.Row.FindControl("txtScannedDate");
                TextBox txtUpload = (TextBox)e.Row.FindControl("txOD");
                //commented by saranya
                //Label lblCollectedby = (Label)e.Row.FindControl("txtColletedBy");
                //Label txtColletedDate = (Label)e.Row.FindControl("txtColletedDate");
                //Label txtScannedDate = (Label)e.Row.FindControl("txtScannedDate");
                //Label lblScanneddby = (Label)e.Row.FindControl("txtScannedBy");
                //txtColletedDate.Attributes.Add("readonly", "readonly");
                //txtScannedDate.Attributes.Add("readonly", "readonly");
                //end
                //Added by Shibu 6-Jun-2013
                CheckBox chkOptMan = (CheckBox)e.Row.FindControl("chkOptMan");
                DropDownList ddlCollAndWaiver = (DropDownList)e.Row.FindControl("ddlCollAndWaiver");
                //DropDownList ddlScannedBy = (DropDownList)e.Row.FindControl("ddlScannedBy");
                UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlScannedBy");
                Label lblOptMan = (Label)e.Row.FindControl("lblOptMan");
                string Doc_CollectOrWaived = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["Doc_CollectOrWaived"].ToString());
                if (Doc_CollectOrWaived != "")
                {
                    ddlCollAndWaiver.SelectedValue = Doc_CollectOrWaived;

                }
                if (ddlCollAndWaiver.SelectedValue == "W")
                {
                    ddlScannedBy.Enabled = false;
                    txtScannedDate.Enabled = false;
                }
                else
                {
                    ddlScannedBy.Enabled = true;
                    txtScannedDate.Enabled = true;
                }
                if (lblOptMan.Text.Trim() == "True" || lblOptMan.Text.Trim() == "1")
                {
                    chkOptMan.Checked = true;
                }
                else
                {
                    chkOptMan.Checked = false;
                }

                //End 

                //Utility.FillDLL(ddlCollectedby, (DataTable)ViewState["UserDetails"]);
                //Utility.FillDLL(ddlScannedby, (DataTable)ViewState["UserDetails"]);

                //Added by saranya
                /*
                ObjStatus.Option = 35;
                ObjStatus.Param1 = intCompanyId.ToString();
                Utility.FillDLL(ddlCollectedby, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));

                ObjStatus.Option = 35;
                ObjStatus.Param1 = intCompanyId.ToString();
                Utility.FillDLL(ddlScannedby, ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus));
                */
                Label lblCollectedBy = e.Row.FindControl("lblCollectedBy") as Label;
                Label lblScannedBy = e.Row.FindControl("lblScannedBy") as Label;

                if (lblCollectedBy.Text != "")
                {
                    ddlCollectedby.SelectedValue = lblCollectedBy.Text;
                    ddlCollectedby.SelectedText = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["CollectedBy"].ToString());
                }
                if (lblScannedBy.Text != "")
                {
                    ddlScannedBy.SelectedValue = lblScannedBy.Text;
                    ddlScannedBy.SelectedText = Convert.ToString(gvPRDDT.DataKeys[e.Row.RowIndex]["ScandedBy"].ToString());
                }


                if (txtColletedDate.Text.Contains("1900"))
                {
                    txtColletedDate.Text = "";

                    //Viewdoct.Enabled = false;
                }
                if (PRDDTransID > 0)
                {
                    Viewdoct.Enabled = true;
                    Viewdoct.CssClass = "styleGridEdit";
                }
                else
                {
                    Viewdoct.Enabled = false;
                    Viewdoct.CssClass = "styleGridEditDisabled";
                }

                if (txtScannedDate.Text.Contains("1900"))
                {
                    Cbx1.Checked = false;
                    txtScannedDate.Text = "";
                    txtScannedDate.Visible = false; //added
                    ddlScannedBy.Clear();
                    ddlScannedBy.Visible = false; //added   
                }
                if (ViewState["Docpath"] != null)
                    txtUpload.Text = ViewState["Docpath"].ToString();

                if (strMode == "M")
                {
                    txtUpload.Text = lblPath.Text;
                    if (string.IsNullOrEmpty(txtUpload.Text))
                    {
                        Viewdoct.Enabled = false;
                        Viewdoct.CssClass = "styleGridEditDisabled";
                    }
                }

                //if (lblCollectedby.Text == "")
                //    lblCollectedby.Text = ObjUserInfo.ProUserNameRW;

                //if (lblScanneddby.Text == "")
                //    lblScanneddby.Text = ObjUserInfo.ProUserNameRW;



            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {

                ddlConstitition.Items.Clear();
                FunProClearCustomerDetails(true);
                ddlEnquiry.Clear();
                FunProGetCustomerDetails(hdnCustomerId.Value.ToString());
                // FunProLoadPANum(Convert.ToInt32(hdnCustomerId.Value.ToString()), Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), Convert.ToInt32(ViewState["intPDDTransID"]));

                ViewState["CustomerID"] = hdnCustomerId.Value;


            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to display Customer Details");
        }
    }
    protected void FunProGetCustomerDetails(string intCustomerID)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Customer_Id", intCustomerID.ToString());
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtCustDetails = Utility.GetDefaultData("S3G_GetCustomerDetails", dictParam);

            if (dtCustDetails.Rows.Count > 0)
            {
                fnBindCustomerDetails(dtCustDetails.Rows[0], "Customer");
                ddlConstitition.Items.Add(new ListItem(dtCustDetails.Rows[0]["Constitution_Name"].ToString(), dtCustDetails.Rows[0]["Constitution_ID"].ToString()));
            }
            else
            {
                throw new Exception("Unable to load Customer information");
            }

        }
        catch (Exception ex)
        {
            cvPRDTT.ErrorMessage = ex.ToString();
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }
    protected void FunProClearCustomerDetails(bool FromCustomer)
    {
        if (!FromCustomer)
        {
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Text = "";
            txtCustomerName.Text = "";
            txtCustomerID.Text = "";

            S3GCustomerCommAddress.ClearCustomerDetails();
            S3GCustomerPermAddress.ClearCustomerDetails();
            ucCustomerCodeLov.strBranchID = "-1";
            ucCustomerCodeLov.strLOBID = "-1";
            ucCustomerCodeLov.strRegionID = "-1";
        }
        txtProduct.Text = "";
        txtProductName.Text = "";
        txtStatus.Text = "";
        ViewState["DocDetails"] = gvPRDDT.DataSource = null;
        gvPRDDT.DataBind();
        ViewState["DocPath"] = "";
    }

    //No Need Customer DropdownControl it's Changed to Popup Control By Shibu
    //protected void ddlCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        txtCustomerID.Text = ddlCustomerCode.SelectedValue;
    //        if (ddlConstitition.Items.Count > 0)
    //        {
    //            //ddlConstitition.SelectedIndex = -1;
    //            ddlConstitition.Items.Clear();
    //        }
    //        //if (ddlLOB.Items.Count > 0)
    //        //{
    //        //    ddlLOB.SelectedIndex = 0;
    //        //    ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
    //        //}
    //        //if (ddlBranch.Items.Count > 0) ddlBranch.SelectedIndex = 0;

    //        gvPRDDT.Visible = false;
    //        if (ddlCustomerCode.SelectedIndex == 0) Clear();
    //        //if (ddlRefPoint.SelectedIndex > 0) ddlRefPoint.ClearSelection();
    //        //if (ddlEnquiry.Items.Count > 0)
    //        //{
    //        //    ddlEnquiry.SelectedIndex = 0;
    //        //    ddlEnquiry.Items.Clear();
    //        //}
    //        FunPriGetLookUpList();
    //        Clear();


    //        PopulateEnquiry();
    //        if (ddlEnquiry.Items.Count == 2)
    //        {
    //            ddlEnquiry.SelectedValue = ddlEnquiry.Items[1].Value;
    //            FunPriGetCustomerDetails();
    //            FunPriGetPRDDTransType();
    //            tpPDDT.Enabled = true;
    //            gvPRDDT.Visible = true;
    //        }

    //        if (ddlCustomerCode.SelectedIndex != 0 && ddlEnquiry.Items.Count > 2)
    //        {
    //            Dictionary<string, string> dictParam = new Dictionary<string, string>();
    //            dictParam.Add("@Customer_Id", ddlCustomerCode.SelectedValue.ToString());
    //            dictParam.Add("@Company_ID", intCompanyId.ToString());
    //            DataTable dtCustDetails = Utility.GetDefaultData("S3G_GetCustomerDetails", dictParam);

    //            if (dtCustDetails.Rows.Count > 0)
    //                fnBindCustomerDetails(dtCustDetails.Rows[0], "Customer");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}
    //Add by Shibu 6-Jun-2013 
    protected void ddlCollAndWaiver_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlCollAndWaiver = sender as DropDownList;
        int gvRowIndex = ((GridViewRow)ddlCollAndWaiver.Parent.Parent).RowIndex;
        UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPRDDT.Rows[gvRowIndex].FindControl("ddlScannedBy");
        TextBox txtScannedDate = (TextBox)gvPRDDT.Rows[gvRowIndex].FindControl("txtScannedDate");
        if (ddlCollAndWaiver.SelectedValue == "W")
        {
            //gvPRDDT.HeaderRow.Cells[6].Text = "Waived By";
            //gvPRDDT.HeaderRow.Cells[7].Text = "Waived Date";
            //gvPRDDT.HeaderRow.Cells[9].Enabled = false;
            //gvPRDDT.HeaderRow.Cells[10].Enabled = false;
            try
            {
                txtScannedDate.Text = "";
                if (ddlScannedBy.Visible == true)
                {
                    ddlScannedBy.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            }
            ddlScannedBy.Enabled = false;
            txtScannedDate.Enabled = false;
        }
        else
        {
            //gvPRDDT.HeaderRow.Cells[6].Text = "Collected By";
            //gvPRDDT.HeaderRow.Cells[7].Text = "Collected Date";
            //gvPRDDT.HeaderRow.Cells[9].Enabled = true;
            //gvPRDDT.HeaderRow.Cells[10].Enabled = true;
            ddlScannedBy.Enabled = true;
            txtScannedDate.Enabled = true;
        }
    }

    protected void ddlCollectedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        int intCurrentRow = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex;
        //((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex
        UserControls_S3GAutoSuggest ddlCollectedBy = gvPRDDT.Rows[intCurrentRow].FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;
        if (ddlCollectedBy.SelectedValue != "0")
        {

            Label lblCollectedBy = (Label)gvPRDDT.Rows[intCurrentRow].FindControl("lblCollectedBy");
            lblCollectedBy.Text = ddlCollectedBy.SelectedValue;
        }

    }
    protected void ddlScannedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        int intCurrentRow = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex;
        UserControls_S3GAutoSuggest ddlScannedBy = gvPRDDT.Rows[intCurrentRow].FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;


        if (ddlScannedBy.SelectedValue != "0")
        {

            Label lblScannedBy = (Label)gvPRDDT.Rows[intCurrentRow].FindControl("lblScannedBy");
            lblScannedBy.Text = ddlScannedBy.SelectedValue;
        }
    }

    //private void PopulateEnquiry()
    //{
    //    try
    //    {
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", intCompanyId.ToString());
    //        Procparam.Add("@User_Id", intUserId.ToString());
    //        Procparam.Add("@Program_Id", Program_Id.ToString());
    //        if (ddlRefPoint.SelectedIndex > 0)
    //            Procparam.Add("@Option", ddlRefPoint.SelectedValue);
    //        if (ddlCustomerCode.SelectedIndex != 0)
    //            Procparam.Add("@Customer_ID", ddlCustomerCode.SelectedValue);
    //        Procparam.Add("@Type", "Enquiry");
    //        ddlEnquiry.BindDataTable("S3G_ORG_GetPDDTCustomer", Procparam, new string[] { "ID", "Number" });
    //    }
    //    catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.Text = objFaultExp.Message;
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    //private void PopulateCustomerCode()
    //{
    //    try
    //    {
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", intCompanyId.ToString());
    //        Procparam.Add("@User_Id", intUserId.ToString());
    //        Procparam.Add("@Program_Id", Program_Id.ToString());
    //        if (ddlRefPoint.SelectedIndex > 0)
    //            Procparam.Add("@Option", ddlRefPoint.SelectedValue);
    //        if (ddlEnquiry.SelectedIndex > 0)
    //            Procparam.Add("@Ref_ID", ddlEnquiry.SelectedValue);
    //        Procparam.Add("@Type", "CUSTOMER");
    //        ddlCustomerCode.BindDataTable("S3G_ORG_GetPDDTCustomer", Procparam, true, "-- Select --", new string[] { "Customer_ID", "Customer_Code" });


    //    }
    //    catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.Text = objFaultExp.Message;
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}


    // Added By Shibu 17-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_PageValue.intUserId.ToString());
        Procparam.Add("@Program_Id", obj_PageValue.Program_Id.ToString());
        Procparam.Add("@Lob_Id", obj_PageValue.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    // Added By Shibu 17-Sep-2013 User List (Auto Suggestion)
    [System.Web.Services.WebMethod]
    public static string[] GetUserList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_Get_User_List, Procparam));

        return suggestions.ToArray();
    }

    // Added By Shibu 17-Sep-2013 Reference Number (Auto Suggestion)
    [System.Web.Services.WebMethod]
    public static string[] GetRefDocList(String prefixText, int count)
    {
        //Procparam.Add("@Company_ID", intCompanyId.ToString());
        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
        //Procparam.Add("@Constitution_ID", ddlConstitition.SelectedValue);
        //Procparam.Add("@OptionValue", ddlRefPoint.SelectedValue);
        //ddlEnquiry.BindDataTable("S3G_ORG_GetPDDTRefDocNo", Procparam, new string[] { "RefDoc_ID", "RefDoc_No" });
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@LOB_ID", obj_PageValue.ddlLOB.SelectedValue);
        Procparam.Add("@Location_ID", obj_PageValue.ddlBranch.SelectedValue);
        Procparam.Add("@Constitution_ID", obj_PageValue.ddlConstitition.SelectedValue);
        Procparam.Add("@OptionValue", obj_PageValue.ddlRefPoint.SelectedValue);
        Procparam.Add("@Company_ID", obj_PageValue.intCompanyId.ToString());
        Procparam.Add("@Customer_ID", obj_PageValue.txtCustomerID.Text);
        Procparam.Add("@SearchText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetPDDTRefDocNo_AGT", Procparam));

        return suggestions.ToArray();
    }


}

