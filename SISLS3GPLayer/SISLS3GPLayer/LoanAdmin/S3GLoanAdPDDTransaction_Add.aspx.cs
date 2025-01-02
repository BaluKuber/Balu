#region Page Header

/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: S3GLoanAdPDDTransaction_Add
/// Created By			: Thangam M
/// Start Date		    : 
/// End Date		    : 
/// Purpose	            : To fetch Post Disb. Doc Transaction Details
/// Modified By         : --
/// Modified Date       : --
///  
///

#endregion

#region Namespaces

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

#endregion

public partial class LoanAdmin_S3GLoanAdPDDTransaction_Add : ApplyThemeForProject
{

    #region Intialization

    int intCompanyId;
    public int intUserId;
    string strUserLogin = string.Empty;
    int intPDDTransID = 0;
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
    string strRedirectPage = "../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PDT";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdPDDTransaction_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=PDT';";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    Dictionary<string, string> dictParam = null;
    SerializationMode SerMode = SerializationMode.Binary;
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    PRDDCMgtServicesClient objPDDT_MasterClient;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();

    PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable ObjS3G_PDDTransMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable ObjS3G_PDDTransDocMasterDataTable = null;
    PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable ObjS3G_ORG_ExitsPDDCTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable();
   
    public static LoanAdmin_S3GLoanAdPDDTransaction_Add obj_PageValue; 
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
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        txtDate.Text = DateTime.Today.ToString(strDateFormat);
        ProgramCode = "041";
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));



        if (!IsPostBack)
        {
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                ViewState["intPDDTransID"] = Convert.ToInt32(fromTicket.Name);
                ViewState["PageMode"] = PageMode.ToString();
            }
            else
            {
                ViewState["intPDDTransID"] = "0";
                ViewState["PageMode"] = "Create";
            }

            FunPrGetBranchLOB();

            if (PageMode == PageModes.Modify)
            {
                FunProDisableControls(1);
            }
            else if (PageMode == PageModes.Query)
            {
                FunProDisableControls(-1);
            }
            else
            {
                FunProDisableControls(0);
            }
            //ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;

            if (PageMode == PageModes.WorkFlow)
            {
                ViewState["PageMode"] = PageModes.WorkFlow;
            }

            if (PageMode == PageModes.WorkFlow)
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

        foreach (GridViewRow grvData in gvPDDT.Rows)
        {
            Label myThrobber = (Label)grvData.FindControl("myThrobber");
            HiddenField hidThrobber = (HiddenField)grvData.FindControl("hidThrobber");

            if (hidThrobber.Value != "")
            {
                myThrobber.Text = hidThrobber.Value;
            }
        }

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
        if (ViewState["PageMode"].ToString() == "Create")
        {
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");
        }
        txt.ToolTip = "Customer Code";

    }

    #endregion

    #region Workflow Methods
    /// <summary>
    /// Workflow Function
    /// </summary>
    private void PreparePageForWorkFlowLoad()
    {
        if (!IsPostBack)
        {
            WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowServiceClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

            WorkFlowSession WFSessionValues = new WorkFlowSession();
            byte[] byte_PreDisDoc = objWorkflowServiceClient.FunPubLoadPostDisb(WFSessionValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValues.Document_Type, WFSessionValues.PANUM, WFSessionValues.SANUM);
            DataSet dsWorkflow = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_PreDisDoc, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));


            //if (dsWorkflow.Tables.Count > 1)
            //{
            //    if (dsWorkflow.Tables[1].Rows.Count > 0)
            //    {


            //    }
            //}
            //else
            //{
            if (dsWorkflow.Tables[0].Rows.Count > 0)
            {
                ddlLOB.SelectedValue = dsWorkflow.Tables[0].Rows[0]["LOB_ID"].ToString();
                ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
                ddlLOB.ClearDropDownList();

                ddlBranch.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Location_ID"].ToString();
                //ddlBranch.ClearDropDownList();

                FunProLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue));
                ddlConstitition.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Constitution_ID"].ToString();

                ViewState["CustomerID"] = dsWorkflow.Tables[0].Rows[0]["Customer_ID"].ToString();

                FunProClearCustomerDetails(true);
                if (ViewState["CustomerID"] != null)
                {
                    FunProGetCustomerDetails(ViewState["CustomerID"].ToString());
                    FunProLoadPANum(Convert.ToInt32(ViewState["CustomerID"].ToString()), Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), Convert.ToInt32(ViewState["intPDDTransID"]));

                }
                ddlPANum.SelectedValue = dsWorkflow.Tables[0].Rows[0]["PA_SA_REF_ID"].ToString();
                FunProLoadSANum(ddlPANum.SelectedItem.Text.ToString());
                if(ddlSANum.Items.Count > 0)
                    ddlSANum.SelectedValue = dsWorkflow.Tables[0].Rows[0]["PA_SA_REF_ID"].ToString();
                
                
            }

            //    }
        }
    }
    #endregion

    #region Page Control Events

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearControl(true);

            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
            //FunPubFillLocations();

            if (ddlLOB.SelectedValue.ToString() != "0")
            {
                //FunProLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue.ToString()));
            }
            ddlLOB.Focus();

        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }

    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlConstitition.Items.Clear();
            FunProClearCustomerDetails(false);
            
            ddlBranch.Focus();
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void ddlConstitition_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
          //  FunProClearCustomerDetails(false);
            ddlConstitition.Focus();
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {
                //if (ViewState["CustomerID"] == null || ViewState["CustomerID"].ToString() != hdnCustomerId.Value)
                //{
                ddlConstitition.Items.Clear();
                FunProClearCustomerDetails(true);
                        FunProGetCustomerDetails(hdnCustomerId.Value.ToString());
                        FunProLoadPANum(Convert.ToInt32(hdnCustomerId.Value.ToString()), Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), Convert.ToInt32(ViewState["intPDDTransID"]));
       
                ViewState["CustomerID"] = hdnCustomerId.Value;
                ddlPANum.Focus();
                //}
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to display Customer Details");
        }
    }

    protected void ddlPANum_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSANum.Items.Clear();
            txtProductName.Text = txtProduct.Text = "";
            gvPDDT.DataSource = null;
            gvPDDT.DataBind();

            if (ddlPANum.SelectedValue.ToString() != "0")
            {
                // Added By Shibu 10-Jun-2013
                FunProLoadSANum(ddlPANum.SelectedItem.Text.ToString());
            }
            ddlPANum.Focus();
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "Unable to load Sub Account Number";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunProClearControl(false);
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "Unable to Clear controls";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        objPDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            DataTable Dtl = new DataTable();

            Dtl = (DataTable)ViewState["DocDetails"];
            if (Dtl == null)
            {
                lblErrorMessage.Text = "";
                Utility.FunShowAlertMsg(this, "Document details not defined in Post Disbursements Master");
                strRedirectPageView = strRedirectPageAdd;
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strRedirectPageView, true);
                return;
            }


            int counts = 0;
            int Length = gvPDDT.Rows.Count;

            #region Validate Grid Details

            for (int i = 0; i < gvPDDT.Rows.Count; i++)
            {
                if (gvPDDT.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    TextBox txtScanDate = (TextBox)gvPDDT.Rows[i].FindControl("txtScannedDate");
                    ImageButton hyplnkView = (ImageButton)gvPDDT.Rows[i].FindControl("hyplnkView");
                    AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPDDT.Rows[i].FindControl("asyFileUpload");
                    CheckBox CbxCheck = (CheckBox)gvPDDT.Rows[i].FindControl("CbxCheck");
                    Label lblType = (Label)gvPDDT.Rows[i].FindControl("lblType");
                    Label lblProgramName = (Label)gvPDDT.Rows[i].FindControl("lblProgramName");
                    HiddenField hidThrobber = (HiddenField)gvPDDT.Rows[i].FindControl("hidThrobber");
                    Label myThrobber = (Label)gvPDDT.Rows[i].FindControl("myThrobber");
                    //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
                    //DropDownList ddlCollectedBy = gvPDDT.Rows[i].FindControl("ddlCollectedBy") as DropDownList;

                    //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
                    UserControls_S3GAutoSuggest ddlCollectedBy = gvPDDT.Rows[i].FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;

                   // DropDownList ddlScannedBy = gvPDDT.Rows[i].FindControl("ddlScannedBy") as DropDownList;
                    UserControls_S3GAutoSuggest ddlScannedBy = gvPDDT.Rows[i].FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;

                    DropDownList ddlCollAndWaiver = gvPDDT.Rows[i].FindControl("ddlCollAndWaiver") as DropDownList;
                    TextBox txtStockDate = gvPDDT.Rows[i].FindControl("txtStockDate") as TextBox;
                    TextBox txtTotalStockAmount = gvPDDT.Rows[i].FindControl("txtTotalStockAmount") as TextBox;
                    if (CbxCheck.Checked)
                    {
                        //Added By Shibu 10-Jun-2013
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
                    // Added By Chandru 12-Aug-2013
                    if (lblType.Text.ToUpper().StartsWith("ST"))
                    {
                        if (txtStockDate.Text == "")
                        {
                            Utility.FunShowAlertMsg(this, " Select Stock Date ");
                            return;
                        }
                        if (txtTotalStockAmount.Text == "")
                        {
                            Utility.FunShowAlertMsg(this, " Enter Total Stock Amount ");
                            return;
                        }
                    }
                    if ((Dtl.Rows[i]["Is_Mandatory"].ToString().ToLower() == "true" ||
                                Dtl.Rows[i]["Is_Mandatory"].ToString().ToLower() == "1")
                            && CbxCheck.Checked == false)
                        {
                            if (ddlCollAndWaiver.SelectedValue == "C")
                            {
                                Utility.FunShowAlertMsg(this, "Pre Disbursements Document has to be collected for document type - " + lblType.Text.Trim().ToUpper());
                                return;
                            }
                        }
              

                   if (Dtl.Rows[i]["Is_Mandatory"].ToString() == "True" && CbxCheck.Checked == false)
                    {
                        Utility.FunShowAlertMsg(this, "Post Disbursements Document has to be collected for document type - " + lblType.Text.Trim().ToUpper());
                        return;
                    }
                    if (ddlCollAndWaiver.SelectedValue == "C")
                    {
                        if (Dtl.Rows[i]["Is_Mandatory"].ToString() == "True" && Dtl.Rows[i]["Is_NeedScanCopy"].ToString() == "True")
                        {
                            if (myThrobber.Text.Trim() == "")
                            {
                                Utility.FunShowAlertMsg(this, "Post Disbursements Document has to be scanned for document type - " + lblType.Text.Trim().ToUpper());
                                return;
                            }
                        }
                    }
                    if (hidThrobber.Value.Trim() != "")
                    {
                        //string fileExtension = hidThrobber.Value.Substring(hidThrobber.Value.LastIndexOf('.') + 1);
                        string fileExtension = asyFileUpload.FileName.Substring(asyFileUpload.FileName.LastIndexOf('.') + 1);
                        if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                        {
                            cvPDTT.ErrorMessage = "File extension not supported, only image & pdf files should be uploaded.";
                            cvPDTT.IsValid = false;
                            return;
                        }
                    }

                    if (CbxCheck.Checked) counts++;
                }
            }

            #endregion

            if (Length == counts)
                txtStatus.Text = "Process";
            else if (counts < Length)
                txtStatus.Text = "Hold";
            else
                txtStatus.Text = "Null";

            #region Upload Files

            int intRowindex = 0;
            foreach (GridViewRow grv in gvPDDT.Rows)
            {
                TextBox txOD = grv.FindControl("txOD") as TextBox;
                TextBox txtCollectedDate = grv.FindControl("txtCollectedDate") as TextBox;
                TextBox txtScannedDate = grv.FindControl("txtScannedDate") as TextBox;
                //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
                //DropDownList ddlCollectedBy = grv.FindControl("ddlCollectedBy") as DropDownList;

                //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
                UserControls_S3GAutoSuggest ddlCollectedBy = grv.FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;

                //DropDownList ddlScannedBy = grv.FindControl("ddlScannedBy") as DropDownList;
                UserControls_S3GAutoSuggest ddlScannedBy = grv.FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;

                DropDownList ddlCollAndWaiver = grv.FindControl("ddlCollAndWaiver") as DropDownList;

                TextBox txtScan = grv.FindControl("txtScan") as TextBox;
                TextBox txtRemark = grv.FindControl("txtRemarks") as TextBox;
                ImageButton HypLnk = (ImageButton)grv.FindControl("hyplnkView");
                HiddenField hidThrobber = (HiddenField)grv.FindControl("hidThrobber");

                AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");

                string strPath = "";
                string strNewFileName = AsyncFileUpload1.FileName;
                string strEnqNumber = ddlPANum.SelectedItem.Text.Replace("/", "-");

               if (AsyncFileUpload1.FileName != "" && hidThrobber.Value.Trim() != "")
               // if ( hidThrobber.Value.Trim() != "")
                {
                    if (AsyncFileUpload1.HasFile)
                    {
                        if (ViewState["DocPath"] != null && ViewState["DocPath"].ToString() != "")
                        {
                            strPath = ViewState["DocPath"].ToString() + "PDDTran" + "/" + strEnqNumber + "/" + "PDDTC-" + (intRowindex + 1).ToString();
                            if (Directory.Exists(strPath))
                            {
                                Directory.Delete(strPath, true);
                            }
                            Directory.CreateDirectory(strPath);
                            strPath = strPath + "/" + strNewFileName;
                        }
                        txOD.Text = strPath;

                        FileInfo f1 = new FileInfo(strPath);

                        if (f1.Exists == true)
                            f1.Delete();

                        AsyncFileUpload1.SaveAs(strPath);
                    }
                }
                intRowindex++;
            }

            #endregion

            ObjS3G_PDDTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterRow objPDDTransDocRow;
            ObjS3G_PDDTransMasterDataTable = new PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PPDDTransMasterRow objPDDTransRow;
            objPDDTransRow = ObjS3G_PDDTransMasterDataTable.NewS3G_ORG_PPDDTransMasterRow();
            objPDDTransRow.Company_ID = intCompanyId;
            objPDDTransRow.PreDisbursement_Doc_Tran_ID = Convert.ToInt32(ViewState["intPDDTransID"]);
            objPDDTransRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objPDDTransRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objPDDTransRow.Constitution_ID = Convert.ToInt32(ddlConstitition.SelectedValue);
            if (ViewState["PageMode"].ToString().ToString() == "Create")
            {
                objPDDTransRow.PRDDC_ID = Convert.ToInt32(ViewState["PDDC_ID"].ToString());
                objPDDTransRow.PRDDC_Date = DateTime.Now;
                objPDDTransRow.PRDDC_Number = "0";
                if (ddlSANum.Items.Count > 1)
                {
                    objPDDTransRow.Enquiry_Response_ID = Convert.ToInt32(ddlSANum.SelectedValue.ToString());
                }
                else
                {
                    objPDDTransRow.Enquiry_Response_ID = Convert.ToInt32(ddlPANum.SelectedValue.ToString());
                }
            }
            else
            {
                objPDDTransRow.Enquiry_Response_ID = 0;
                objPDDTransRow.PRDDC_ID = 0;
                objPDDTransRow.PRDDC_Date = Utility.StringToDate(txtDate.Text);
                objPDDTransRow.PRDDC_Number = txtPDDC.Text;
            }

            objPDDTransRow.Customer_ID = Convert.ToInt64(txtCustomerID.Text);

            if (txtStatus.Text == "Null")
            {
                objPDDTransRow.Status = "1";
            }
            else if (txtStatus.Text == "Hold")
            {
                objPDDTransRow.Status = "2";
            }
            else
            {
                objPDDTransRow.Status = "3";
            }

            objPDDTransRow.Created_By = intUserId;
            objPDDTransRow.Created_On = DateTime.Now;
            objPDDTransRow.Modified_By = intUserId;
            objPDDTransRow.Modified_On = DateTime.Now;
            objPDDTransRow.XML_PRDDTDeltails = FunProFormMLAXML();
            ObjS3G_PDDTransMasterDataTable.AddS3G_ORG_PPDDTransMasterRow(objPDDTransRow);
            byte[] ObjPRDDTDataTable = ClsPubSerialize.Serialize(ObjS3G_PDDTransMasterDataTable, SerMode);
            //objPDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
            string strPDDT_No = "";
            if (ViewState["PageMode"].ToString().ToString() == "Modify")
            {
                intErrCode = objPDDT_MasterClient.FunPubModifyPDDTransMaster(SerMode, ObjPRDDTDataTable);
            }
            else
            {
                intErrCode = objPDDT_MasterClient.FunPubCreatePDDTransMaster(out strPDDT_No, SerMode, ObjPRDDTDataTable);

            }

            if (intErrCode == 0)
            {
                if (Convert.ToInt32(ViewState["intPDDTransID"]) > 0)
                {
                    strKey = "Modify";
                    strPDDT_No = txtPDDC.Text;
                    strAlert = strAlert.Replace("__ALERT__", "Post Disbursements Document Transaction details updated successfully"); //Add by Shibu 6-Jun-2013
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strPDDT_No, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            //strAlert = "Work Flow is not assigned";
                            strAlert = strAlert.Replace("__ALERT__", "Post Disbursements Document Transaction details updated successfully");
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }

                        ShowWFAlertMessage(strPDDT_No, ProgramCode, strAlert);
                        return;
                    }

                    //foreach (GridViewRow grv in gvPDDT.Rows)
                    //{
                    //    Label lblPDTID = grv.FindControl("lblPDTID") as Label;
                    //    DropDownList ddlCollAndWaiver = grv.FindControl("ddlCollAndWaiver") as DropDownList;
                        
                    //    //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
                    //    //DropDownList ddlScannedBy = grv.FindControl("ddlScannedBy") as DropDownList;
                    //    UserControls_S3GAutoSuggest ddlScannedBy = grv.FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;
                       
                    //    Label lblType = grv.FindControl("lblType") as Label;
                    //    if (ddlCollAndWaiver.SelectedValue == "C")
                    //    {
                    //        DataSet dsEmail_ID = new DataSet();
                    //        DataTable dtEmail_ID = new DataTable();
                    //        dictParam = new Dictionary<string, string>();
                    //        dictParam.Add("@PDDC_Doc_Cat_ID", lblPDTID.Text);
                    //        dictParam.Add("@Company_ID", intCompanyId.ToString());
                    //        dictParam.Add("@User_ID", intUserId.ToString());

                    //        dsEmail_ID = Utility.GetDataset("S3G_loanAd_GetEmailId", dictParam);

                    //        dtEmail_ID = dsEmail_ID.Tables[0];

                    //        string strToEmail = "";
                    //        for (int i = 0; i < dtEmail_ID.Rows.Count; i++)
                    //        {
                    //            strToEmail += "," + dtEmail_ID.Rows[i]["Email_ID"].ToString();
                    //        }

                    //        if (strToEmail != "")
                    //        {
                    //            strToEmail = strToEmail.Remove(0, 1);
                    //            FunPubSentMail(dsEmail_ID.Tables[1].Rows[0]["Email_ID"].ToString(), strToEmail, lblType.Text, ddlScannedBy.SelectedText);
                    //        }
                    //    }
                    //}

                }
                else
                {
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strPDDT_No, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }
                        ShowWFAlertMessage(strPDDT_No, ProgramCode, strAlert);
                        return;
                    }
                    else
                    {
                        // FORCE PULL IMPLEMENTATION KR
                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(ProgramCode, null, null, "L", CompanyId, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), null, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), null, "", int.Parse(dtrForce["PRODUCTID"].ToString()));
                                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                                btnSave.Enabled = false;
                                //END
                            }
                            catch
                            {
                                strAlert = "Work Flow is not assigned";
                            }
                        }

                        strAlert = "Post Disbursements Document Transaction " + strPDDT_No + " details added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                        return;
                    }
                   
                   
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Post Disbursements Document Transaction details already exists");
            }
            else if (intErrCode == -1)
            {
                if (Convert.ToInt32(ViewState["intPDDTransID"]) == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    strRedirectPageView = "";
                    ddlLOB.Focus();
                }
            }
            else if (intErrCode == -2)
            {
                if (Convert.ToInt32(ViewState["intPDDTransID"]) == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    strRedirectPageView = "";
                    ddlLOB.Focus();
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
           // strAlert = strAlert.Replace("__ALERT__", "Post Disbursements Document details updated successfully");
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
                cvPDTT.ErrorMessage = "File Path not formed well or Access to the path is denied";
                cvPDTT.IsValid = false;
                return;
            }
            else if (ex.Message.Contains("Could not find a part of the path"))
            {
                cvPDTT.ErrorMessage = "File Path defined in Post Disbursements Document Master is not avilable";
                cvPDTT.IsValid = false;
                return;
            }
            else if (ex.Message.ToUpper().Contains("MAIL"))
            {
                cvPDTT.ErrorMessage = "Mail server not available..";
                cvPDTT.IsValid = false;
                return;
            }
            else
            {
                cvPDTT.ErrorMessage = "File Path defined in Post Disbursements Document Master is not avilable ";//ex.Message;
                cvPDTT.IsValid = false;
                return;
            }
        }
        finally
        {
            // if (objPDDT_MasterClient != null)
            objPDDT_MasterClient.Close();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    protected void gvPDDT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
   
        try
        {
            //No Need DropdownControl Changed to AutoSuggestion 
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    ObjStatus.Option = 35;
            //    ObjStatus.Param1 = intCompanyId.ToString();
            //    ViewState["UserDetails"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblPath = (Label)e.Row.FindControl("lblPath");
                //Label lblCollectedby = (Label)e.Row.FindControl("txtColletedBy");
                //Label txtColletedDate = (Label)e.Row.FindControl("txtColletedDate");
              
               

                ImageButton Viewdoct = (ImageButton)e.Row.FindControl("hyplnkView");
                CheckBox Cbx1 = (CheckBox)e.Row.FindControl("CbxCheck");
               
                Label lblIs_Mandatory = (Label)e.Row.FindControl("lblIs_Mandatory");
                TextBox txtUpload = (TextBox)e.Row.FindControl("txOD");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                  ImageButton hyplnkView = (ImageButton)e.Row.FindControl("hyplnkView");

                Label lblNeedScan = (Label)e.Row.FindControl("lblNeedScan");
                AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)e.Row.FindControl("asyFileUpload");
                
                 Label lblCollectedBy=(Label)e.Row.FindControl("lblCollectedBy");


                //DropDownList ddlCollectedby = (DropDownList)e.Row.FindControl("ddlCollectedby");
                
                //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
                UserControls_S3GAutoSuggest ddlCollectedby =(UserControls_S3GAutoSuggest)e.Row.FindControl("ddlCollectedby");
               
                AjaxControlToolkit.CalendarExtender calCollectedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calCollectedDate");
                calCollectedDate.Format = calCollectedDate.Format = strDateFormat;
                TextBox txtColletedDate = (TextBox)e.Row.FindControl("txtCollectedDate");
               
                //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
                // DropDownList ddlScannedby = (DropDownList)e.Row.FindControl("ddlScannedby");
                UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)e.Row.FindControl("ddlScannedBy");

                AjaxControlToolkit.CalendarExtender calScannedDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calScannedDate");
                AjaxControlToolkit.CalendarExtender calStockDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calStockDate");
                calScannedDate.Format = calCollectedDate.Format = calStockDate.Format = strDateFormat;
                TextBox txtScannedDate = (TextBox)e.Row.FindControl("txtScannedDate");
                CheckBox chkOptMan = (CheckBox)e.Row.FindControl("chkOptMan");
                DropDownList ddlCollAndWaiver = (DropDownList)e.Row.FindControl("ddlCollAndWaiver");
               // DropDownList ddlScannedBy = (DropDownList)e.Row.FindControl("ddlScannedBy");
                Label lblOptMan = (Label)e.Row.FindControl("lblOptMan");
                Label lblColUser = (Label)e.Row.FindControl("lblColUser");
                Label lblScanUser = (Label)e.Row.FindControl("lblScanUser");
                string Doc_CollectOrWaived = Convert.ToString(gvPDDT.DataKeys[e.Row.RowIndex]["Doc_CollectOrWaived"].ToString());
                Label lblCollectedDate = (Label)e.Row.FindControl("lblCollectedDate");
               
                //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
                //Utility.FillDLL(ddlCollectedby, (DataTable)ViewState["UserDetails"]);
                //Utility.FillDLL(ddlScannedby, (DataTable)ViewState["UserDetails"]);
                
                TextBox txtStockDate = (TextBox)e.Row.FindControl("txtStockDate");
                TextBox txtTotalStockAmount = (TextBox)e.Row.FindControl("txtTotalStockAmount");
                Label lblType = (Label)e.Row.FindControl("lblType");
                txtTotalStockAmount.CheckGPSLength(true, "Total Stock Amount");

                if (Doc_CollectOrWaived != "")
                {
                    ddlCollAndWaiver.SelectedValue = Doc_CollectOrWaived;

                }
                if (lblOptMan.Text.Trim() == "True" || lblOptMan.Text.Trim() == "1")
                {
                    chkOptMan.Checked = true;
                }
                else
                {
                    chkOptMan.Checked = false;
                }
                if (lblColUser.Text != "")
                {
                    ddlCollectedby.SelectedValue = lblColUser.Text;
                    ddlCollectedby.SelectedText = Convert.ToString(gvPDDT.DataKeys[e.Row.RowIndex]["CollectedBy"].ToString());
                }
                if (lblScanUser.Text != "")
                {
                    ddlScannedBy.SelectedValue=lblScanUser.Text;
                    ddlScannedBy.SelectedText = Convert.ToString(gvPDDT.DataKeys[e.Row.RowIndex]["ScannedBy"].ToString());
                }
                 txtColletedDate.Attributes.Add("readonly", "readonly");
                txtScannedDate.Attributes.Add("readonly", "readonly");
                if (lblCollectedDate.Text != "")
                {
                    txtColletedDate.Text = lblCollectedDate.Text;
                }
                if (txtColletedDate.Text.Contains("1900"))
                {
                    txtColletedDate.Text = "";
                }
                if (txtColletedDate.Text != "")
                {
                    txtColletedDate.Text = Convert.ToDateTime(txtColletedDate.Text).ToString(strDateFormat);
                }
                if (txtScannedDate.Text != "")
                {
                    txtScannedDate.Text = Convert.ToDateTime(txtScannedDate.Text).ToString(strDateFormat);
                }
                if (txtStockDate.Text != "")
                {
                    txtStockDate.Text = Convert.ToDateTime(txtStockDate.Text).ToString(strDateFormat);
                }
                txtStockDate.Attributes.Add("readonly", "readonly");
                txtTotalStockAmount.Attributes.Add("readonly", "readonly");
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
                

                //End 

                if (lblNeedScan.Text != "True")
                {
                    asyFileUpload.Visible = false;
                   // txtScannedDate.Visible = lblScanneddby.Visible = Viewdoct.Visible = false;
                }
                else
                {
                    asyFileUpload.Visible = true;
                }
                
               

                if (ViewState["PageMode"].ToString() == "Query")
                {
                    txtRemarks.Attributes.Add("readonly", "readonly");
                }

                if (ViewState["PageMode"].ToString() != "Create")
                {
                    if (lblPath.Text.Trim() == ViewState["DocPath"].ToString().Trim())
                    {
                        //hyplnkView.Enabled = false;
                        //hyplnkView.CssClass = "styleGridQueryDisabled";
                        hyplnkView.Visible = false;
                    }

                    Cbx1.Enabled = false;
                    if (lblIs_Mandatory.Text == "True")
                    {
                        Cbx1.Checked = true;
                    }
                }

                
                if (Convert.ToInt32(ViewState["intPDDTransID"]) > 0)
                {
                    Viewdoct.Enabled = true;
                }
                else
                {
                    Viewdoct.Enabled = false;
                }

                if (txtScannedDate.Text.Contains("1900"))
                {
                    Cbx1.Checked = false;
                    txtScannedDate.Text = "";
                }
                if (ViewState["DocPath"] != null)
                    txtUpload.Text = ViewState["DocPath"].ToString();

                if (ViewState["PageMode"].ToString() == "Modify")
                {
                    txtUpload.Text = lblPath.Text;
                }

                //if (lblCollectedby.Text == "")
                //    lblCollectedby.Text = ObjUserInfo.ProUserNameRW;

                //if (lblScanneddby.Text == "")
                //    lblScanneddby.Text = ObjUserInfo.ProUserNameRW;

                if (lblType.Text.ToUpper().StartsWith("ST"))
                {
                    calStockDate.Enabled = true;
                    txtTotalStockAmount.Attributes.Remove("readonly");
                }

            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlCollectedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        int intCurrentRow = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex;
        //((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex
        UserControls_S3GAutoSuggest ddlCollectedBy = gvPDDT.Rows[intCurrentRow].FindControl("ddlCollectedBy") as UserControls_S3GAutoSuggest;
        if (ddlCollectedBy.SelectedValue != "0")
        {
          
            Label lblCollectedBy = (Label)gvPDDT.Rows[intCurrentRow].FindControl("lblCollectedBy");
            lblCollectedBy.Text = ddlCollectedBy.SelectedValue;
        }

    }
    protected void ddlScannedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
       // DropDownList ddlScannedBy = sender as DropDownList;
        int intCurrentRow = ((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex;
        //((GridViewRow)((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent).RowIndex
        UserControls_S3GAutoSuggest ddlScannedBy = gvPDDT.Rows[intCurrentRow].FindControl("ddlScannedBy") as UserControls_S3GAutoSuggest;
      
        if (ddlScannedBy.SelectedValue !="0")
        {
            Label lblScannedBy = (Label)gvPDDT.Rows[intCurrentRow].FindControl("lblScannedBy");
            lblScannedBy.Text = ddlScannedBy.SelectedValue;
        }
    }
    //Added by Shibu 10-Jun-2013
    protected void ddlCollAndWaiver_SelectedIndexChanged(object sender, EventArgs e)
    {
          DropDownList ddlCollAndWaiver = sender as DropDownList;
        int gvRowIndex = ((GridViewRow)ddlCollAndWaiver.Parent.Parent).RowIndex;
        //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
        //  DropDownList ddlScannedBy = (DropDownList)gvPDDT.Rows[gvRowIndex].FindControl("ddlScannedBy");
 UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPDDT.Rows[gvRowIndex].FindControl("ddlScannedBy");
        TextBox txtScannedDate = (TextBox)gvPDDT.Rows[gvRowIndex].FindControl("txtScannedDate");
        if (ddlCollAndWaiver.SelectedValue == "W")
        {
            //gvPDDT.HeaderRow.Cells[6].Text = "Waived By";
            //gvPDDT.HeaderRow.Cells[7].Text = "Waived Date";
            //gvPDDT.HeaderRow.Cells[9].Enabled = false;
            //gvPDDT.HeaderRow.Cells[10].Enabled = false;
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
            //gvPDDT.HeaderRow.Cells[6].Text = "Collected By";
            //gvPDDT.HeaderRow.Cells[7].Text = "Collected Date";
            //gvPDDT.HeaderRow.Cells[9].Enabled = true;
            //gvPDDT.HeaderRow.Cells[10].Enabled = true;
            ddlScannedBy.Enabled = true;
            txtScannedDate.Enabled = true;
        }
    }

    protected void hyplnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvPDDT", strFieldAtt);
            Label lblPath = (Label)gvPDDT.Rows[gRowIndex].FindControl("lblPath");

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ViewState["intPDDTransID"].ToString(), false, 0);

            if (lblPath.Text.Trim() != ViewState["DocPath"].ToString().Trim())
            {
                string strFileName = lblPath.Text.Replace("\\", "/").Trim();
                string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "&qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M" + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
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

    #endregion

    # region Methods

    public void FunPubSentMail(string strFrom, string strTo, string DocType, string ScanedBy)
    {
        try
        {

            Dictionary<string, string> dictMail = new Dictionary<string, string>();
            dictMail.Add("FromMail", strFrom);
            dictMail.Add("ToMail", strTo);
            dictMail.Add("Subject", "Post Disbursements Document Transaction");
            ArrayList arrMailAttachement = new ArrayList();
            StringBuilder strBody = GetHTMLTextEmail(DocType, ScanedBy);
            //strBody.Append("Test");
            //if (strnewFile != "")
            //{
            //    arrMailAttachement.Add(strnewFile);
            //}
            Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private StringBuilder GetHTMLTextEmail(string strDocType, string ScanedBy)
    {
        StringBuilder strMailBodey = new StringBuilder();
        strMailBodey.Append(

           "<font size=\"1\"  color=\"black\" face=\"Times New Roman\">" +
           "<table width=\"100%\">" +

           "<tr > <td  align=\"Left\" > Dear Madam / Sir ,</td> </tr>" +

           "<tr > <td  align=\"Left\" > </td> </tr> " +

           "<tr >" +

           "<td  align=\"Left\" > The document " + strDocType + " has been uploaded into the system by " + ScanedBy + "." +

           "</td>" +

           "</tr>" +

           "<tr > <td  align=\"Left\" > </td> </tr> <BR/>" +

           "<tr > <td  align=\"Left\" > Please do not reply to this mail. </td> </tr> " +

           "<tr > <td  align=\"Left\" > </td> </tr> " +

           "<tr > <td  align=\"Left\" > </td> </tr> " +

           "<tr > <td  align=\"Left\" > " + ObjUserInfo.ProUserNameRW + "</td> </tr>" +

           "<tr > <td  align=\"Left\" > " + ObjUserInfo.ProCompanyNameRW + "</td> </tr>" +

           "</table> </font>");

        return strMailBodey;

    }

    protected void FunProDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                gvPDDT.Columns[10].Visible = false;
                txtPDDC.Enabled = false;
                ddlLOB.Focus();
                tpPDDT.Enabled = true;
                gvPDDT.Visible = true;
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }

              
                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
               // ddlLOB.Enabled = ddlBranch.Enabled = false;
               // ddlConstitition.Enabled = ddlCustomerCode.Enabled = ddlPANum.Enabled = ddlSANum.Enabled = false;
                txtDate.ReadOnly = true;
                 txtPDDC.ReadOnly = true;
                 ucCustomerCodeLov.ButtonEnabled = false;
                //lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                tpPDDT.Enabled = true;
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                txtPDDC.Enabled = false;

                FunGetPDDTrans();
                 //FunGetPRDDTransDocDetatils();

                //ddlLOB.Enabled = ddlBranch.Enabled = false;
                //ddlConstitition.Enabled = ddlCustomerCode.Enabled = ddlPANum.Enabled = ddlSANum.Enabled = false;
                //txtDate.ReadOnly = true;
                ////lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                //btnClear.Enabled = false;
                //tpPDDT.Enabled = true;
                break;

            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                txtPDDC.ReadOnly = true;
                txtPDDC.Enabled = true;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                ucCustomerCodeLov.ButtonEnabled = false;
              gvPDDT.Columns[9].Visible = false;
                gvPDDT.Columns[gvPDDT.Columns.Count - 2].Visible = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                FunGetPDDTrans();
                // FunGetPRDDTransDocDetatils();

                foreach (GridViewRow grv in gvPDDT.Rows)
                {
                 
                    TextBox TxtScanRef = (TextBox)grv.FindControl("txtScan");
                    TextBox TxtRemarks = (TextBox)grv.FindControl("txtRemarks");
                    CheckBox CkBox = (CheckBox)grv.FindControl("CbxCheck");
                    ImageButton hyplnkView = (ImageButton)grv.FindControl("hyplnkView");
                   
                    AjaxControlToolkit.CalendarExtender DtCollect = (AjaxControlToolkit.CalendarExtender)grv.FindControl("cexDate1");
                    AjaxControlToolkit.CalendarExtender DtScan = (AjaxControlToolkit.CalendarExtender)grv.FindControl("cexDate2");
                    UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)grv.FindControl("ddlCollectedBy");
                    DropDownList ddlCollAndWaiver = (DropDownList)grv.FindControl("ddlCollAndWaiver");
                    UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)grv.FindControl("ddlScannedBy");

                    TxtScanRef.ReadOnly = true;
                    TxtRemarks.ReadOnly = true;
                    ddlCollectedBy.ReadOnly = true;
                    ddlScannedBy.ReadOnly = false;
                    CkBox.Enabled = false;   //myThrobber.Visible 
                    ddlCollAndWaiver.Enabled = false;
                   
                }

                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                   // ddlBranch.Clear();
                    //ddlConstitition.ClearDropDownList();
                    //ddlPANum.ClearDropDownList();
                    //ddlSANum.ClearDropDownList();
                    //ddlCustomerCode.ClearDropDownList();
                }
                break;
        }
    }

    protected void FunPrGetBranchLOB()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (Convert.ToInt32(ViewState["intPDDTransID"]) == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_Id", intUserId.ToString());
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Program_ID", "185");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
            //if (ViewState["PageMode"].ToString() != "Create")
            //{
            //    ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
            //}
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

    protected void FunProGetCustomerDetails(string intCustomerID)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Customer_ID", intCustomerID.ToString());
            DataTable dtCustomer = Utility.GetDefaultData("S3G_LOANAD_GetPDTCustomerDetails", dictParam);

            if (dtCustomer.Rows.Count > 0)
            {
                FunProFillCustomer(dtCustomer.Rows[0]);
                ddlConstitition.Items.Add(new ListItem(dtCustomer.Rows[0]["Constitution_Name"].ToString(), dtCustomer.Rows[0]["Constitution_ID"].ToString()));
            }
            else
            {
                throw new Exception("Unable to load Customer information");
            }
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = ex.ToString();
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void FunProFillCustomer(DataRow DRow)
    {
        txtCustomerName.Text = DRow["Title"].ToString() + " " +DRow["Customer_Name"].ToString();
        txtCustomerID.Text = DRow["Customer_ID"].ToString();
        TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        txt.Text = DRow["Customer_Code"].ToString();

        S3GCustomerCommAddress.SetCustomerDetails(DRow, true);
        S3GCustomerPermAddress.SetCustomerDetails(DRow, false);
    }

    protected void FunProLoadConstitution(int intLineofBusinessID)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            if (ViewState["PageMode"].ToString() == "Create")
            {
                dictParam.Add("@Is_Active", "1");
            }
            dictParam.Add("@LOB_ID", intLineofBusinessID.ToString());
            ddlConstitition.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, dictParam, new string[] { "Constitution_ID", "ConstitutionName" });
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }

    }

    protected void FunProLoadPANum(int intCustomer_ID, int intLineofBusinessID, int intBranchID, int intConstititionID, int PDDTransID)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Type", "1");
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@LOB_ID", intLineofBusinessID.ToString());
            dictParam.Add("@Location_ID", intBranchID.ToString());
            dictParam.Add("@Customer_ID", intCustomer_ID.ToString());
            if (ViewState["PageMode"].ToString() == "Create" || PageMode == PageModes.WorkFlow)
            {
                dictParam.Add("@Is_Activated", "2");
                dictParam.Add("@Constitution_ID", intConstititionID.ToString());
            }
            else
            {
                dictParam.Add("@Is_Activated", "2");
            }
            dictParam.Add("@PostDisbursement_Doc_Tran_ID", PDDTransID.ToString());

            if (ViewState["PageMode"].ToString() == "Create" || PageMode == PageModes.WorkFlow)
            {
                ddlPANum.BindDataTable("S3G_LOANAD_GetPLASLA_PDT", dictParam, new string[] { "PA_SA_REF_ID", "PANum" });
            }
            else
            {
                ddlPANum.BindDataTable("S3G_LOANAD_GetPLASLA_PDT", dictParam, new string[] { "PANum", "PANum" });
            }

            try
            {
                if (ddlPANum.Items.Count == 1)
                {
                    throw new Exception("");
                }
            }
            catch (Exception e)
            {
                cvPDTT.ErrorMessage = "No new accounts found for the selected Customer.";
                cvPDTT.IsValid = false;
            }
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "Unable to load Prime Account Number";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void FunProLoadSANum(string PANum)
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Type", "2");
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            dictParam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            dictParam.Add("@Customer_ID", txtCustomerID.Text);
            dictParam.Add("@PANum", PANum);
            if (ViewState["PageMode"].ToString() == "Create" || PageMode == PageModes.WorkFlow)
            {
                dictParam.Add("@Is_Activated", "2");
            }
            dictParam.Add("@Constitution_ID", ddlConstitition.SelectedValue.ToString());
            dictParam.Add("@PostDisbursement_Doc_Tran_ID", ViewState["intPDDTransID"].ToString());

            DataSet DSet = new DataSet();
            DSet = Utility.GetDataset("S3G_LOANAD_GetPLASLA_PDT", dictParam);

            if (ViewState["PageMode"].ToString() == "Create" || PageMode == PageModes.WorkFlow)
            {
                ddlSANum.BindDataTable(DSet.Tables[0], new string[] { "PA_SA_REF_ID", "SANum" });
            }
            else
            {
                ddlSANum.BindDataTable(DSet.Tables[0], new string[] { "SANum", "SANum" });
            }
            try
            {
             txtProduct.Text = DSet.Tables[1].Rows[0]["Product_ID"].ToString();
                txtProductName.Text = DSet.Tables[1].Rows[0]["Product_Name"].ToString();
            }
            catch (Exception ex)
            {
            }
            if (ViewState["PageMode"].ToString() == "Create" || PageMode == PageModes.WorkFlow)
            {
                if (DSet.Tables[2].Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Document details not defined in Post Disbursement Documents Master");
                    return;
                }
                ViewState["DocDetails"] = gvPDDT.DataSource = DSet.Tables[2];
            }
            ViewState["DocPath"] = DSet.Tables[2].Rows[0]["Document_Path"].ToString();
            ViewState["PDDC_ID"] = DSet.Tables[2].Rows[0]["PDDC_ID"].ToString();
            gvPDDT.DataBind();

            if (ddlSANum.Items.Count == 1)
            {
                lblSANum.CssClass = "styleDisplayLabel";
                rfvSANum.Enabled = false;
            }
            else
            {
                lblSANum.CssClass = "styleReqFieldLabel";
                rfvSANum.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "Unable to load Sub Account Number";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 

    //protected void FunPubFillLocations()
    //{
    //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
    //    Procparam.Add("@User_Id", Convert.ToString(intUserId));
    //    Procparam.Add("@Program_Id", "185");
    //    if (PageMode == PageModes.Create)
    //    {
    //        Procparam.Add("@Is_Active", "1");
    //    }
    //    Procparam.Add("@LOB_Id", ddlLOB.SelectedValue.ToString());
    //    ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
    //}

    protected void FunProClearControl(bool FromLOB)
    {
        try
        {
            if (!FromLOB)
            {
                ddlLOB.SelectedValue = "0";
            }
            //ddlBranch.SelectedValue = "0";
            ddlBranch.Clear();
            ddlConstitition.Items.Clear();
            FunProClearCustomerDetails(false);
        }
        catch (Exception ex)
        {
            cvPDTT.ErrorMessage = "";
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
        ddlPANum.Items.Clear();
        ddlSANum.Items.Clear();

        ViewState["DocDetails"] = gvPDDT.DataSource = null;
        gvPDDT.DataBind();
        ViewState["DocPath"] = "";
    }

    protected void FunGetPDDTrans()
    {
        try
        {
            DataSet DSet = new DataSet();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@PostDisbursement_Doc_Tran_ID", ViewState["intPDDTransID"].ToString());

            DSet = Utility.GetDataset("S3G_LOANAD_GetPDDCTransDetails", dictParam);

            if (DSet != null)
            {
                DataTable dtHeader = DSet.Tables[0];

                ddlLOB.SelectedValue = dtHeader.Rows[0]["LOB_ID"].ToString();
                ddlBranch.SelectedValue = dtHeader.Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = dtHeader.Rows[0]["Location_Name"].ToString();
                
                ddlConstitition.Items.Clear();
                //FunProLoadConstitution(Convert.ToInt32(ddlLOB.SelectedValue.ToString()));
                ddlConstitition.Items.Add(new ListItem(dtHeader.Rows[0]["Constitution_Name"].ToString(), dtHeader.Rows[0]["Constitution_ID"].ToString()));
           
                FunProFillCustomer(DSet.Tables[2].Rows[0]);
                ucCustomerCodeLov.ButtonEnabled = false;

                if (btnSave.Enabled == true)
                {
                    FunProLoadPANum(Convert.ToInt32(txtCustomerID.Text), Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlConstitition.SelectedValue), Convert.ToInt32(ViewState["intPDDTransID"]));
                    ddlPANum.SelectedValue = dtHeader.Rows[0]["PANum"].ToString();
                    FunProLoadSANum(ddlPANum.SelectedValue.ToString());
                    ddlSANum.SelectedValue = dtHeader.Rows[0]["SANum"].ToString();
                }
                else
                {
                    ddlPANum.Items.Add(new ListItem(dtHeader.Rows[0]["PANum"].ToString(), dtHeader.Rows[0]["PANum"].ToString()));
                    ddlSANum.Items.Add(new ListItem(dtHeader.Rows[0]["SANum"].ToString(), dtHeader.Rows[0]["SANum"].ToString()));
                }
                
                txtPDDC.Text = dtHeader.Rows[0]["PDDT_Number"].ToString();
                txtProductName.Text = dtHeader.Rows[0]["Product_Name"].ToString();
                txtProduct.Text = dtHeader.Rows[0]["Product_ID"].ToString();
                ViewState["DocPath"] = DSet.Tables[3].Rows[0]["Document_Path"].ToString();
                string strDocPath = DSet.Tables[3].Rows[0]["Document_Path"].ToString();
                if (dtHeader.Rows[0]["Status"].ToString() == "1")
                {
                    txtStatus.Text = "";
                }
                else if (dtHeader.Rows[0]["Status"].ToString() == "2")
                {
                    txtStatus.Text = "Hold";
                }
                else
                {
                    txtStatus.Text = "Process";
                }

                ViewState["DocDetails"] = gvPDDT.DataSource = DSet.Tables[1];
                gvPDDT.DataBind();
                FunGetPRDDTransDocDetatils(DSet.Tables[1], strDocPath);
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

    protected void FunGetPRDDTransDocDetatils(DataTable dtDetails, string strPath)
    {
       // objPDDT_MasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
           /* PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsRow ObjPRDDTransDocMasterRow;
            ObjS3G_ORG_PDDCTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable();
            ObjPRDDTransDocMasterRow = ObjS3G_ORG_PDDCTransDocMasterDataTable.NewS3G_ORG_GetPRDDCTransDocDetailsRow();
            ObjPRDDTransDocMasterRow.PreDisbursement_Doc_Tran_ID = intPDDTransID;
            ObjPRDDTransDocMasterRow.Company_ID = intCompanyId;
            ObjPRDDTransDocMasterRow.Enquiry_Response_ID = ddlPANum.SelectedValue;
            ObjPRDDTransDocMasterRow.Company_ID = intCompanyId; ObjS3G_ORG_PDDCTransDocMasterDataTable.AddS3G_ORG_GetPRDDCTransDocDetailsRow(ObjPRDDTransDocMasterRow);
            byte[] bytePRDDTransDocDetails = objPDDT_MasterClient.FunPubGetPRDDTransDoc(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PDDCTransDocMasterDataTable, SerMode));
            ObjS3G_ORG_PDDCTransDocMasterDataTable = (PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable)ClsPubSerialize.DeSerialize(bytePRDDTransDocDetails, SerMode, typeof(PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable));*/



            if (dtDetails.Rows.Count > 0)
            {
                ViewState["DocPath"] = strPath;
            }
          //  gvPDDT.DataSource = dtDetails;// Modify 
          //  gvPDDT.DataBind();

            for (int i = 0; i < gvPDDT.Rows.Count; i++)
            {
                if (gvPDDT.Rows[i].RowType == DataControlRowType.DataRow)
               {
                    int PDDC_Doc_Cat_ID = Convert.ToInt32(gvPDDT.DataKeys[i]["PDDC_Doc_Cat_ID"].ToString());
                    CheckBox Cbx1 = (CheckBox)gvPDDT.Rows[i].FindControl("CbxCheck");
                    //modified  and added by saranya for fixing observation on 01-Mar-2012
                    // DropDownList ddlScannedBy = (DropDownList)gvPDDT.Rows[i].FindControl("ddlScannedBy");
                    UserControls_S3GAutoSuggest ddlScannedBy = (UserControls_S3GAutoSuggest)gvPDDT.Rows[i].FindControl("ddlScannedBy");
                    TextBox txtScanDate = (TextBox)gvPDDT.Rows[i].FindControl("txtScannedDate");
                    Label lblScannedBy = (Label)gvPDDT.Rows[i].FindControl("lblScannedBy");

                    UserControls_S3GAutoSuggest ddlCollectedBy = (UserControls_S3GAutoSuggest)gvPDDT.Rows[i].FindControl("ddlCollectedBy");
                    TextBox txtcolldate = (TextBox)gvPDDT.Rows[i].FindControl("txtCollectedDate");
                    Label lblCollectedBy = (Label)gvPDDT.Rows[i].FindControl("lblCollectedBy");
                    //end 

                    TextBox txtScanRef = (TextBox)gvPDDT.Rows[i].FindControl("txtScan");
                    ImageButton Viewdoct = (ImageButton)gvPDDT.Rows[i].FindControl("hyplnkView");
                    TextBox txtRemarks = (TextBox)gvPDDT.Rows[i].FindControl("txtRemarks");
                    Label lblDesc = (Label)gvPDDT.Rows[i].FindControl("lblDesc");
                    TextBox txOD = (TextBox)gvPDDT.Rows[i].FindControl("txOD");
                    TextBox txtScan = (TextBox)gvPDDT.Rows[i].FindControl("txtScan");
                    Label lblColUser = (Label)gvPDDT.Rows[i].FindControl("lblColUser");
                    Label lblPath = (Label)gvPDDT.Rows[i].FindControl("lblPath");
                    Label myThrobber = (Label)gvPDDT.Rows[i].FindControl("myThrobber");
                    HiddenField hidThrobber = (HiddenField)gvPDDT.Rows[i].FindControl("hidThrobber");

                    if (!string.IsNullOrEmpty(lblPath.Text.Trim()))
                    {
                        if (lblPath.Text.Trim() == ViewState["DocPath"].ToString().Trim())
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

                    AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)gvPDDT.Rows[i].FindControl("asyFileUpload");

                    Label lblType = (Label)gvPDDT.Rows[i].FindControl("lblType");

                    Cbx1.Checked = false;
                    if (PDDC_Doc_Cat_ID == Convert.ToInt32(dtDetails.Rows[i]["PDDC_Doc_Cat_ID"].ToString()))
                    {
                        if ((dtDetails.Rows[i]["Scanned_Date"].ToString()) != string.Empty)
                        {
                            txtScanDate.Text = Convert.ToDateTime(dtDetails.Rows[i]["Scanned_Date"].ToString()).ToString(strDateFormat);
                        }
                        else
                        {
                            txtScanDate.Text = "";
                        }

                        if ((Convert.ToString(dtDetails.Rows[i]["ScannedBy"])) != string.Empty)
                        {
                            ddlScannedBy.SelectedValue = Convert.ToString(dtDetails.Rows[i]["Scanned_By"]);
                            //ddlScannedBy.ClearDropDownList();
                            lblScannedBy.Text = Convert.ToString(dtDetails.Rows[i]["Scanned_By"]);
                        }
                        else
                        {
                            ddlScannedBy.SelectedValue = "0";
                            lblScannedBy.Text = "";
                        }


                        if ((dtDetails.Rows[i]["Collected_Date"].ToString()) != string.Empty)
                        {
                            txtcolldate.Text = Convert.ToDateTime(dtDetails.Rows[i]["Collected_Date"].ToString()).ToString(strDateFormat);
                        }
                        else
                        {
                            txtcolldate.Text = "";
                        }
                        if ((Convert.ToString(dtDetails.Rows[i]["CollectedBy"])) != string.Empty)
                        {
                            ddlCollectedBy.SelectedValue = Convert.ToString(dtDetails.Rows[i]["Collected_By"]);
                            //ddlCollectedBy.ClearDropDownList();
                            lblCollectedBy.Text = Convert.ToString(dtDetails.Rows[i]["Collected_By"]);
                        }
                        else
                        {
                            ddlCollectedBy.SelectedValue = "0";
                            lblCollectedBy.Text = "";
                        }
                        //end
                       // maxversion = Convert.ToInt32(dtDetails.Rows[i]["Version_No"]);


                        if (Convert.ToBoolean(dtDetails.Rows[i]["PDDTrans"]) == true)
                         {
                            Cbx1.Checked = true;
                            Cbx1.Enabled = false;
                        }
                        //else
                        //{
                        //    txtcollBy.Text = ObjUserInfo.ProUserNameRW;
                        //    lblColUser.Text = "";
                        //}

                       // MaxVerChk.Value += maxversion + "@@" + chkbox;

                        if (txtcolldate.Text == "")
                        {
                            txtcolldate.Text = "";
                            Cbx1.Checked = false;
                        }
                        MaxVerChk.Value += "@@" + txtScanDate.Text;
                        MaxVerChk.Value += "@@" + txtcolldate.Text;
                        txtScanRef.Text = Convert.ToString(dtDetails.Rows[i]["Scanned_Ref_No"]);
                        txtRemarks.Text = Convert.ToString(dtDetails.Rows[i]["Remarks"]);
                        MaxVerChk.Value += "@@" + txtRemarks.Text;

                        if (lblPath.Text.Trim() != ViewState["DocPath"].ToString().Trim())
                        {
                            string[] Path = Convert.ToString(dtDetails.Rows[i]["Scanned_Ref_No"]).Split('/');
                            //hidThrobber.Value = Path[Path.Length - 1].ToString();
                            myThrobber.Text = Path[Path.Length - 1].ToString();
                            txOD.Text = Convert.ToString(dtDetails.Rows[i]["Scanned_Ref_No"]);
                        }
                        //else
                        //{
                        //    txtScanBy.Text = ObjUserInfo.ProUserNameRW;                            
                        //}
                    }

                    if (dtDetails.Rows.Count == dtDetails.Rows.Count)
                    {
                        if ((dtDetails.Rows[i]["Is_NeedScanCopy"].ToString().ToLower() == "false")
                            || (dtDetails.Rows[i]["Is_NeedScanCopy"].ToString() == "0"))
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
      
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            //if (objPDDT_MasterClient != null)
           // objPDDT_MasterClient.Close();
        }
    }

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
        foreach (GridViewRow grvData in gvPDDT.Rows)
        {
            int fileIndex = 1;

            string strlblPDTID = ((Label)grvData.FindControl("lblPDTID")).Text;
            string strScanBy = "";
            string strCollectdBy = "";
            //Removed By Shibu 19-Sep-2013 to Add Auto Suggestion 
            //if (((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedIndex > 0)
            //{    if (((Label)grvData.FindControl("lblCollectedBy")).Text!="")
            //    {
            //        strCollectdBy = ((Label)grvData.FindControl("lblCollectedBy")).Text;
            //    }
            //       else
            //    {
            //        strCollectdBy = ((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedValue;
            //    }
            //}  
            //else
            //{
            //    strCollectdBy = ((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedValue;
            //}

            //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
            if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue !="0")
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
            //if (((DropDownList)grvData.FindControl("ddlScannedBy")).Visible)
            //{
            //    if (((DropDownList)grvData.FindControl("ddlScannedBy")).SelectedIndex > 0)
            //    {
            //        if (((Label)grvData.FindControl("lblScannedBy")).Text != "")
            //        {
            //            strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
            //        }
            //        else
            //        {
            //            strScanBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
            //        }
            //    }
            //    else
            //    {
            //        strScanBy = ((DropDownList)grvData.FindControl("ddlScannedBy")).SelectedValue;
            //    }
            //}
            //else
            //{
            //    strScanBy = "-1";
            //}

            //Added By Shibu 19-Sep-2013 to Add Auto Suggestion 
            if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).Visible)
            {
                if (((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).SelectedValue !="0")
                {
                    if (((Label)grvData.FindControl("lblScannedBy")).Text != "")
                    {
                        strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
                    }
                    else
                    {
                        strScanBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlCollectedBy")).SelectedValue;
                    }
                }
                else
                {
                    strScanBy = ((UserControls_S3GAutoSuggest)grvData.FindControl("ddlScannedBy")).SelectedValue;
                }
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
            //if (((Label)grvData.FindControl("lblColUser")).Text != "")
            //    strCollectdBy = ((Label)grvData.FindControl("lblColUser")).Text;
            //else
            //    strCollectdBy = Convert.ToString(intUserId);

            //if (((Label)grvData.FindControl("lblScanUser")).Text != "")
            //    strScanBy = ((Label)grvData.FindControl("lblScanUser")).Text;
            //else
            //    strScanBy = Convert.ToString(intUserId);

            //if (((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedIndex > 0)
            //    strCollectdBy = ((Label)grvData.FindControl("lblCollectedBy")).Text;
            //else
            //    strCollectdBy = ((DropDownList)grvData.FindControl("ddlCollectedBy")).SelectedValue;

            
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

            //string strCollectdDate = ((TextBox)grvData.FindControl("txtCollectedDate")).Text;
            //string strScanDate = "";
            //if (((TextBox)grvData.FindControl("txtScannedDate")).Visible)
            //{
            //    if (((TextBox)grvData.FindControl("txtScannedDate")).Text.Contains("1900"))
            //    {
            //        strScanDate = "";
            //    }
            //        else
            //    {
            //           strScanDate = ((TextBox)grvData.FindControl("txtScannedDate")).Text;
            //    }
            //}
           

            CheckBox CbxCheck = (CheckBox)grvData.FindControl("CbxCheck");
            AjaxControlToolkit.AsyncFileUpload asyFileUpload = (AjaxControlToolkit.AsyncFileUpload)grvData.FindControl("asyFileUpload");
                       
            if (Convert.ToInt32(ViewState["intPDDTransID"]) != 0)
            {
                if (asyFileUpload.FileName.ToString() != "")
                {
                    //strScanBy = Convert.ToString(intUserId);
                    strScanBy = ((Label)grvData.FindControl("lblScannedBy")).Text;
                    //strScanDate = DateTime.Now.ToString(strDateFormat).ToString();
                    strScanDate = ((TextBox)grvData.FindControl("txtScannedDate")).Text;
                }
            }

            string strScanRefNo = ((TextBox)grvData.FindControl("txOD")).Text;
            string strRemarks = ((TextBox)grvData.FindControl("txtRemarks")).Text.Replace("'", "\"").Replace(">", "").Replace("<", "").Replace("&", "");
            string strPRDDTrans = Convert.ToString(((CheckBox)grvData.FindControl("CbxCheck")).Checked);
            string[] temp;
            if (!string.IsNullOrEmpty(MaxVerChk.Value))
            {
                temp = Convert.ToString(temprow[rowcount]).Split('@', '@');
                //maxversion = Convert.ToInt32(temp[0]);
                //chkbox = Convert.ToBoolean(temp[2]);
                if (strPRDDTrans.ToLower() == Convert.ToString(chkbox).ToLower())
                {
                    //{ strVersionNo = Convert.ToString(maxversion);
                    // Counts = 0;
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
            string sCollectedOrWaived = ((DropDownList)grvData.FindControl("ddlCollAndWaiver")).SelectedValue;
            strScanDate = strScanDate == string.Empty ? strScanDate : Utility.StringToDate(strScanDate).ToString();
            strCollectdDate = strCollectdDate == string.Empty ? strCollectdDate : Utility.StringToDate(strCollectdDate).ToString();//Utility.StringToDate(DateTime.Now.ToString(strDateFormat)).ToString()

            string strStockDate = ((TextBox)grvData.FindControl("txtStockDate")).Text;
            strStockDate = strStockDate == string.Empty ? strStockDate : Utility.StringToDate(strStockDate).ToString();

            string strStockAmount = ((TextBox)grvData.FindControl("txtTotalStockAmount")).Text;
            //strStockDate = strStockAmount == string.Empty ? strStockAmount : strStockAmount.ToString();

            strbuXML.Append(" <Details  PDDC_Doc_Cat_ID='" + strlblPDTID + "' Collected_By='" + strCollectdBy.ToString() + "' Collected_Date='" + strCollectdDate +
                "' Scanned_By='" + strScanBy.ToString() + "' Scanned_Date='" + strScanDate + "' Scanned_Ref_No='" + strScanRefNo.ToString() + "' Remarks='" + strRemarks.ToString() + 
                "' PDDTrans='" + strPRDDTrans.ToString() + "' Version_No='" + "' Counts='" + Counts.ToString() + "' Doc_CollectOrWaived='" + sCollectedOrWaived +
                "' Stock_Date='" + strStockDate + "' Total_StockAmount='" + strStockAmount + "'/>"); // Added By Shibu to Update Collected / Waived Values 6-Jun-2013
            rowcount = rowcount + 3;

        }
        string tem = "Version_No='" + strVersionNo + "'";
        strbuXML.Replace("Version_No=''", tem);
        strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }

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
        Procparam.Add("@Program_Id", "185");
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
    #endregion
}

