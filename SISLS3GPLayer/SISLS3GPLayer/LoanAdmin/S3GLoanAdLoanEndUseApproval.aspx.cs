

#region Namespaces

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
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Resources;
using S3GBusEntity;
using S3GBusEntity.Collection;
using System.Text;
using System.IO;
using S3GBusEntity.LoanAdmin;

#endregion

public partial class LoanAdmin_S3GLoanAdLoanEndUseApproval : ApplyThemeForProject
{
    #region Variable Declaration

    ApprovalMgtServicesReference.ApprovalMgtServicesClient ObjLoanAdminApprovalMgtServicesClient;
    ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalDataTable ObjLoanEndUseApprovalDt = new ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalDataTable ();

    SerializationMode SerMode = SerializationMode.Binary;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    int intCompanyID, intUserID, i, intErrCode = 0, intGPSPrefix = 0, intGPSSuffix = 0;
    Int64 intLoanApprovalID = 0;
    long endno;

    DataTable dt = new DataTable();
    Dictionary<string, string> dictParam = null;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();

    string strLoanEndUseID = "";
    static string strPageName = "Loan End Use Approval";
    string StrXMLPDC, strPDCNo, strchequeNo, s, strexistingdate;
    string strRedirectPage = "../LoanAdmin/S3GLoanAdLoanEndUseApproval.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdLoanEndUseApproval.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=LEUA';";
    public static LoanAdmin_S3GLoanAdLoanEndUseApproval obj_Page;
    #endregion

    #region "WebMethods"

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetUser(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Userlist_AGT", Procparam));

        return suggetions.ToArray();
    }

    #endregion

    #region "METHODS"

    private void FunPriBindBranchLOB()
    {
        try
        {
            // LOB

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@Is_Active", "1");
            dictParam.Add("@User_ID", intUserID.ToString());
            dictParam.Add("@Program_ID", "106");
            dictParam.Add("@FilterOption", "'FL','HP','LN','OL','TE','TL','PL'");
            ddlLOB.BindDataTable(SPNames.LOBMaster, dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            RFVddlLOB.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LOB;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    public void FunPubFillBranch()
    {
        try
        {
            //Branch
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@Is_Active", "1");
            dictParam.Add("@User_ID", intUserID.ToString());
            dictParam.Add("@Program_ID", "106");
            dictParam.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
            ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            ddlBranch.SelectedIndex = 0;
            RFVddlBranch.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Branch;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadPage()
    {
        try
        {
            obj_Page = this;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            // Get Prefix and suffix values from Session
            intGPSPrefix = ObjS3GSession.ProGpsPrefixRW;
            intGPSSuffix = ObjS3GSession.ProGpsSuffixRW;
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
            txtUserName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtUserName.ToolTip = txtUserName.Text;

            CalendarExtenderEndUseDate.Format = strDateFormat;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                    strLoanEndUseID = Convert.ToString(fromTicket.Name);
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }
            else
            {
                strMode = Request.QueryString["qsMode"];                
            }
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end  
            if (!IsPostBack)
            {
                txtEndUseDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndUseDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtEndUseDate.Text = Convert.ToString(System.DateTime.Now.ToString(strDateFormat));
                if (strMode == "C")
                {
                    lblLoanEndUseApproval.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    FunPriBindBranchLOB();
                    FunPriLoadLov();
                }
                else if (strMode == "Q")
                {
                    lblLoanEndUseApproval.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FunPriLoadApprovalDtls(strLoanEndUseID);
                    FunPriToggleQueryMode();
                }
                
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriLoadCustomerEntityDtls(string HdnId)
    {
        try
        {
            ViewState["hdnCustorEntityID"] = HdnId;
            DataTable dtCustEntityDtls = new DataTable();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", intCompanyID.ToString());
            dictParam.Add("@ID", HdnId);
            dictParam.Add("@TypeID", "144");
            dtCustEntityDtls = Utility.GetDefaultData("S3G_LOANAD_GETCustomerorEntityDetails", dictParam);
            if (dtCustEntityDtls.Rows.Count > 0)
            {
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Text = txtCustomerCode.Text = dtCustEntityDtls.Rows[0]["Code"].ToString();

                ucCustomerAddress.SetCustomerDetails(dtCustEntityDtls.Rows[0]["Code"].ToString(),
                        dtCustEntityDtls.Rows[0]["Address1"].ToString() + "\n" +
                dtCustEntityDtls.Rows[0]["Address2"].ToString() + "\n" +
                dtCustEntityDtls.Rows[0]["city"].ToString() + "\n" +
                dtCustEntityDtls.Rows[0]["state"].ToString() + "\n" +
                dtCustEntityDtls.Rows[0]["country"].ToString() + "\n" +
                dtCustEntityDtls.Rows[0]["pincode"].ToString(), dtCustEntityDtls.Rows[0]["Name"].ToString(),
                dtCustEntityDtls.Rows[0]["Telephone"].ToString(),
                dtCustEntityDtls.Rows[0]["Mobile"].ToString(),
                dtCustEntityDtls.Rows[0]["email"].ToString(), dtCustEntityDtls.Rows[0]["website"].ToString());
                ViewState["Address"] = dtCustEntityDtls;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void LoadPrimeAccNo(string strCustomer_ID)
    {
        try
        {
            // Load Prime Account Number
            dictParam = new Dictionary<string, string>();
            dictParam.Clear();
            dictParam.Add("@Option", "1");
            dictParam.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
            //dictParam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            //dictParam.Add("@Is_Activated", "1");
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@Customer_Id", strCustomer_ID);
            ddlPAN.Items.Clear();
            ddlPAN.BindDataTable("S3G_LOANAD_GETLOANENDUSEPANUM", dictParam, new string[] { "PANum", "PANum" });
            if (ddlPAN.Items.Count == 2)
            {
                ddlPAN.SelectedIndex = 1;
                ddlPAN_SelectedIndexChanged(this, new EventArgs());
            }
            else
            {
                FunPriClearGrid();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private DataTable FunPriApprovedDtls(string strApprovalID, string strStageID, string strStageName, string strApprovedAmount, string strApproveredID, string strApprovedBy,
        string strApprovedDate, string strRemarks, string strPhotoUpload, string strDocumentUpload, string strEndUse,
        string strEndUseID, string strAction, string strActionID)
    {
        try
        {
            DataTable dtableApprovedDtls = new DataTable();
            DataRow dr;
            if (ViewState["LoanApprovedDtls"] == null)
            {
                DataColumn Approval_ID = new DataColumn("Approval_ID", System.Type.GetType("System.String"));
                DataColumn Stage_ID = new DataColumn("Stage_ID", System.Type.GetType("System.String"));
                DataColumn Stage_Name = new DataColumn("Stage_Name", System.Type.GetType("System.String"));
                DataColumn Approved_Amount = new DataColumn("Approved_Amount", System.Type.GetType("System.Double"));
                DataColumn Approvered_ID = new DataColumn("Approvered_ID", System.Type.GetType("System.String"));
                DataColumn Approved_By = new DataColumn("Approved_By", System.Type.GetType("System.String"));
                DataColumn Approved_Date = new DataColumn("Approved_Date", System.Type.GetType("System.String"));
                DataColumn Remarks = new DataColumn("Remarks", System.Type.GetType("System.String"));
                DataColumn Photo_Upload = new DataColumn("Photo_Upload", System.Type.GetType("System.String"));
                DataColumn Document_Upload = new DataColumn("Document_Upload", System.Type.GetType("System.String"));
                DataColumn End_Use = new DataColumn("End_Use", System.Type.GetType("System.String"));
                DataColumn End_Use_ID = new DataColumn("End_Use_ID", System.Type.GetType("System.String"));
                DataColumn Action = new DataColumn("Action", System.Type.GetType("System.String"));
                DataColumn Action_ID = new DataColumn("Action_ID", System.Type.GetType("System.String"));

                dtableApprovedDtls.Columns.Add(Approval_ID); 
                dtableApprovedDtls.Columns.Add(Stage_ID);
                dtableApprovedDtls.Columns.Add(Stage_Name);
                dtableApprovedDtls.Columns.Add(Approved_Amount);
                dtableApprovedDtls.Columns.Add(Approvered_ID);
                dtableApprovedDtls.Columns.Add(Approved_By);
                dtableApprovedDtls.Columns.Add(Approved_Date);
                dtableApprovedDtls.Columns.Add(Remarks);
                dtableApprovedDtls.Columns.Add(Photo_Upload);
                dtableApprovedDtls.Columns.Add(Document_Upload);
                dtableApprovedDtls.Columns.Add(End_Use);
                dtableApprovedDtls.Columns.Add(End_Use_ID);
                dtableApprovedDtls.Columns.Add(Action);
                dtableApprovedDtls.Columns.Add(Action_ID);

                dr = dtableApprovedDtls.NewRow();
                dtableApprovedDtls.Rows.Add(dr);

                ViewState["LoanApprovedDtls"] = dtableApprovedDtls;
            }
            else
            {
                dtableApprovedDtls = (DataTable)ViewState["LoanApprovedDtls"];
                if (dtableApprovedDtls.Rows.Count == 0)
                {
                    dr = dtableApprovedDtls.NewRow();
                    dr["Approval_ID"] = strApprovalID;
                    dr["Stage_ID"] = strStageID;
                    dr["Stage_Name"] = strStageName;
                    dr["Approved_Amount"] = Convert.ToDouble(strApprovedAmount);
                    dr["Approvered_ID"] = strApproveredID;
                    dr["Approved_By"] = strApprovedBy;
                    dr["Approved_Date"] = strApprovedDate;
                    dr["Remarks"] = strRemarks;
                    dr["Photo_Upload"] = strPhotoUpload;
                    dr["Document_Upload"] = strDocumentUpload;
                    dr["End_Use"] = strEndUse;
                    dr["End_Use_ID"] = strEndUseID;
                    dr["Action"] = strAction;
                    dr["Action_ID"] = strActionID;


                    dtableApprovedDtls.Rows.Add(dr); 
                    ViewState["LoanApprovedDtls"] = dtableApprovedDtls;
                }
                else
                {

                    dr = dtableApprovedDtls.NewRow();
                    dr["Approval_ID"] = strApprovalID;
                    dr["Stage_ID"] = strStageID;
                    dr["Stage_Name"] = strStageName;
                    dr["Approved_Amount"] = Convert.ToDouble(strApprovedAmount);
                    dr["Approvered_ID"] = strApproveredID;
                    dr["Approved_By"] = strApprovedBy;
                    dr["Approved_Date"] = strApprovedDate;
                    dr["Remarks"] = strRemarks;
                    dr["Photo_Upload"] = strPhotoUpload;
                    dr["Document_Upload"] = strDocumentUpload;
                    dr["End_Use"] = strEndUse;
                    dr["End_Use_ID"] = strEndUseID;
                    dr["Action"] = strAction;
                    dr["Action_ID"] = strActionID;

                    dtableApprovedDtls.Rows.Add(dr);
                    if (Convert.ToString(dtableApprovedDtls.Rows[0]["Stage_ID"]) == "" || dtableApprovedDtls.Rows[0]["Stage_ID"].Equals("0"))
                    {
                        dtableApprovedDtls.Rows[0].Delete();
                    }
                    ViewState["LoanApprovedDtls"] = dtableApprovedDtls;
                }
            }
            return dtableApprovedDtls;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
            return null;
        }
    }

    private void FunPriBindLoanApprovedDtls(DataTable dtableApprovedDtls)
    {
        try
        {
            pnlLoanApprovedDtls.Visible = true;

            grvPaymentApprovedDetails.DataSource = dtableApprovedDtls;
            grvPaymentApprovedDetails.DataBind();
            if (Convert.ToString(dtableApprovedDtls.Rows[0]["Stage_ID"]) == "" || dtableApprovedDtls.Rows[0]["Stage_ID"].Equals("0"))
            {
                grvPaymentApprovedDetails.Rows[0].Visible = false;                
            }
            grvPaymentApprovedDetails.FooterRow.Visible = true;

            FunPriLoadGridLov();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    private void FunPriClearGrid()
    {
        try
        {
            pnlPaymentDetails.Visible = false;
            grvPaymentDetails.DataSource = null;
            grvPaymentDetails.DataBind();
            ViewState["LoanApprovedDtls"] = null;
            DataTable dtableApprovedDtls = FunPriApprovedDtls("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
            FunPriBindLoanApprovedDtls(dtableApprovedDtls);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearControls()
    {
        try
        {
            if (ddlLOB.Items.Count > 0) ddlLOB.SelectedIndex = -1;
            ddlBranch.Items.Clear();
            ddlPAN.Items.Clear();
            ucCustomerCodeLov.FunPubClearControlValue();
            ucCustomerAddress.ClearCustomerDetails();
            txtEndUseDate.Text = txtEndUseNumber.Text = string.Empty;
            FunPriClearGrid();
            txtEndUseDate.Text = Convert.ToString(System.DateTime.Now.ToString(strDateFormat));
            ddlAmountUtilized.SelectedValue = "0";
            txtComponents.Text = string.Empty;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadLov()
    {
        try
        {
            if (dictParam != null)
                dictParam.Clear();
            else
                dictParam = new Dictionary<string, string>();

            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LookupType_Code", "122");//Amount Utilized
            ddlAmountUtilized.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, dictParam, new string[] { "Lookup_Code", "Lookup_Description" });

            DataTable dtableApprovedDtls = FunPriApprovedDtls("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
            FunPriBindLoanApprovedDtls(dtableApprovedDtls);
            FunPriSetMaxLength();

            //Get Documentation Path 
            if (dictParam != null)
                dictParam.Clear();
            else
                dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@Program_ID", "260");//Loan End Use Approval Program ID
            DataSet dsDoc = Utility.GetDataset("S3G_Get_DocumentationPath", dictParam);
            DataTable dtableDOc = dsDoc.Tables[0];
            if (dtableDOc.Rows.Count > 0)
            {
                ViewState["DocumentationPath"] = Convert.ToString(dtableDOc.Rows[0]["Document_Path"]);
            }
            else
            {
                ViewState["DocumentationPath"] = "";
                Utility.FunShowAlertMsg(this, "Document Path not yet defined");
            }

        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadGridLov()
    {
        try
        {

            if (grvPaymentApprovedDetails != null && grvPaymentApprovedDetails.FooterRow != null)
            {
                //Bind Stage Name 
                if (dictParam != null)
                    dictParam.Clear();
                else
                    dictParam = new Dictionary<string, string>();
                dictParam.Add("@OPTION", "1");
                dictParam.Add("@Company_ID", intCompanyID.ToString());
                dictParam.Add("@Customer_ID", Convert.ToString(ViewState["CustomerID"]));
                dictParam.Add("@PANUM", ddlPAN.SelectedValue.ToString());
                dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                DataSet ds = Utility.GetDataset("S3G_LOANAD_GET_PANUMOUTFLOWPAID_DTLS", dictParam);

                DataTable dtablePymtDtls = ds.Tables[0];
                DropDownList ddlFooterStageName = (DropDownList)grvPaymentApprovedDetails.FooterRow.FindControl("ddlFooterStageName");
                ddlFooterStageName.Items.Clear();
                if (dtablePymtDtls != null && dtablePymtDtls.Rows.Count > 0)
                {
                    ddlFooterStageName.BindDataTable(dtablePymtDtls, new string[] { "Stage_No", "Stage_Description" });
                }
                //Bind End Use Details
                DropDownList ddlFooterEndUse = (DropDownList)grvPaymentApprovedDetails.FooterRow.FindControl("ddlFooterEndUse");
                ddlFooterEndUse.Items.Clear();
                if (dictParam != null)
                    dictParam.Clear();
                else
                    dictParam = new Dictionary<string, string>();

                dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictParam.Add("@LookupType_Code", "121");//End Use Type
                ddlFooterEndUse.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, dictParam, new string[] { "Lookup_Code", "Lookup_Description" });

                //Bind Action Details
                DropDownList ddlFooterAction = (DropDownList)grvPaymentApprovedDetails.FooterRow.FindControl("ddlFooterAction");
                ddlFooterAction.Items.Clear();
                if (dictParam != null)
                    dictParam.Clear();
                else
                    dictParam = new Dictionary<string, string>();

                dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictParam.Add("@LookupType_Code", "123");//Loen End Use Action
                ddlFooterAction.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, dictParam, new string[] { "Lookup_Code", "Lookup_Description" });

                TextBox txtApprovedDate = (TextBox)grvPaymentApprovedDetails.FooterRow.FindControl("txtApprovedDate");
                AjaxControlToolkit.CalendarExtender CEApprovedDate = (AjaxControlToolkit.CalendarExtender)grvPaymentApprovedDetails.FooterRow.FindControl("CECApprovedDate");
                CEApprovedDate.Format = strDateFormat;
                txtApprovedDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtApprovedDate.ClientID + "','" + strDateFormat + "',false,  true);");
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriSetMaxLength()
    {
        try
        {
            if (grvPaymentApprovedDetails != null)
            {
                TextBox txtApprovedAmount = (TextBox)grvPaymentApprovedDetails.FooterRow.FindControl("txtApprovedAmount") as TextBox;
                txtApprovedAmount.CheckGPSLength(true, "Amount");
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriSaveApprovalDtls()
    {
        Int32 iMode = 0;
        try
        {
            ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalDataTable LoanEndUseApprovalDataTable = new ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalDataTable();
            ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalRow LoanEndUseApprovalDataRow = LoanEndUseApprovalDataTable.NewS3g_LOANAD_LoanEndUseApprovalRow();

            if (Convert.ToString(strLoanEndUseID) != "")
            {
            }

            DataTable dtableLoanApprovedDtls = (DataTable)ViewState["LoanApprovedDtls"];
            DataRow[] drow = dtableLoanApprovedDtls.Select("Approval_ID=0");
            if (drow.Length > 0)
            {
                LoanEndUseApprovalDataRow.Option = 1;
                iMode = 1;
            }
            else
            {
                LoanEndUseApprovalDataRow.Option = 0;
                iMode = 0;
            }

            LoanEndUseApprovalDataRow.Loan_Approval_ID = 0;
            LoanEndUseApprovalDataRow.End_Use_Date = Utility.StringToDate(txtEndUseDate.Text);
            LoanEndUseApprovalDataRow.End_Use_Number = Convert.ToString(txtEndUseNumber.Text);
            LoanEndUseApprovalDataRow.Company_ID = intCompanyID;
            LoanEndUseApprovalDataRow.Components = Convert.ToString(txtComponents.Text);
            LoanEndUseApprovalDataRow.Created_By = intUserID;
            if (ViewState["hdnCustorEntityID"] != "" && Convert.ToString(ViewState["hdnCustorEntityID"]) != "")
                LoanEndUseApprovalDataRow.Customer_Code = Convert.ToInt32(ViewState["hdnCustorEntityID"].ToString());
            LoanEndUseApprovalDataRow.Location_Code = Convert.ToString(ddlBranch.SelectedValue);
            LoanEndUseApprovalDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            LoanEndUseApprovalDataRow.PANUM = Convert.ToString(ddlPAN.SelectedValue);
            LoanEndUseApprovalDataRow.Amount_Utilized_Code = Convert.ToInt32(ddlAmountUtilized.SelectedValue);

            if (ViewState["PaymentDetails"] != null)
            {
                if (((DataTable)ViewState["PaymentDetails"]).Rows.Count > 0)
                {
                    DataTable dtablePmtDtls = (DataTable)ViewState["PaymentDetails"];
                    LoanEndUseApprovalDataRow.XML_Payment_Dtls = dtablePmtDtls.FunPubFormXml(true);
                    //LoanEndUseApprovalDataRow.XML_Payment_Dtls = grvPaymentDetails.FunPubFormXml(true);
                }
                else
                {
                    LoanEndUseApprovalDataRow.XML_Payment_Dtls = "<Root></Root>";
                }
            }
            else
            {
                LoanEndUseApprovalDataRow.XML_Payment_Dtls = "<Root></Root>";
            }

            if (ViewState["LoanApprovedDtls"] != null)
            {
                if (((DataTable)ViewState["LoanApprovedDtls"]).Rows.Count > 0 && ((DataTable)ViewState["LoanApprovedDtls"]).Rows[0]["Stage_ID"].ToString() != ""&&((DataTable)ViewState["LoanApprovedDtls"]).Rows[0]["Stage_ID"].ToString() != "0")
                {
                    DataTable dtableAprvlDtls = (DataTable)ViewState["LoanApprovedDtls"];
                    //LoanEndUseApprovalDataRow.XML_Approval_Dtls = dtableAprvlDtls.FunPubFormXml(true);
                    LoanEndUseApprovalDataRow.XML_Approval_Dtls = grvPaymentApprovedDetails.FunPubFormXml(true);
                }
                else
                {
                    LoanEndUseApprovalDataRow.XML_Approval_Dtls = "<Root></Root>";
                }
            }
            else
            {
                LoanEndUseApprovalDataRow.XML_Approval_Dtls = "<Root></Root>";
            }

            LoanEndUseApprovalDataTable.AddS3g_LOANAD_LoanEndUseApprovalRow(LoanEndUseApprovalDataRow);

            ObjLoanAdminApprovalMgtServicesClient = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();
            SerializationMode SMode = SerializationMode.Binary;

            string strEndUseNumber = string.Empty;
            int intApproval_No = 0;
            int intErrCode = ObjLoanAdminApprovalMgtServicesClient.FunPubCreateOrModifyLoanEndUseApproval(out strEndUseNumber, out intApproval_No, SMode, ClsPubSerialize.Serialize(LoanEndUseApprovalDataTable, SMode));

            //intErrCode = 90;

            if (intErrCode == 0)
            {
                //if (strMode == "C")
                //{
                    //strAlert = "End Use Number is " + strEndUseNumber;
                if (iMode == 1)
                {
                    strAlert = "Loan End Use Approval Created successfully";
                    strAlert += @"\n\nWould you like to add one more Loan End Use Approval?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPage = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
                else
                {
                    strAlert = "Loan End Use Approval Updated successfully";
                    strAlert += @"\n\nWould you like to add one more Loan End Use Approval?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPage = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
                //}
            }
            else if (intErrCode == 90)
            {
                Utility.FunShowAlertMsg(this, "Document sequence number not defined for Loan End Use Approval");
            }
            else if (intErrCode == 91)
            {
                Utility.FunShowAlertMsg(this, "Document sequence number exceeded for Loan End Use Approval");
            }
            else if (intErrCode == 91)
            {
                Utility.FunShowAlertMsg(this, "Return Sucess");
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private Double FunPriGetSatgeApprovedAmt(string strStageId)
    {
        double _dblExistApprovedAmt = 0;
        try
        {
            DataTable dtableApprovedDtls = (DataTable)ViewState["LoanApprovedDtls"];
            if (dtableApprovedDtls != null && dtableApprovedDtls.Rows.Count > 0)
            {
                DataView dvApprovedView = new DataView(dtableApprovedDtls);
                dvApprovedView.RowFilter = "Approval_ID = 0 and Stage_Id =" + strStageId;
                dtableApprovedDtls = dvApprovedView.ToTable();
                if (dtableApprovedDtls.Rows.Count > 0)
                {
                    _dblExistApprovedAmt = Math.Round(Convert.ToDouble(dtableApprovedDtls.Compute("sum(Approved_Amount)", "Approved_Amount>=0")), 2);
                }
            }
        }
        catch (Exception objException)
        {
            //throw objException;
        }
        return _dblExistApprovedAmt;
    }

    private Double FunPriCheckSatgeApprovedAmt(string strStageId)
    {
        double _dblApprovedAmt = 0;
        try
        {
            DataTable dtableApprovedDtls = (DataTable)ViewState["LoanApprovedDtls"];
            if (dtableApprovedDtls != null && dtableApprovedDtls.Rows.Count > 0)
            {
                DataView dvApprovedView = new DataView(dtableApprovedDtls);
                dvApprovedView.RowFilter = "Action_ID = 1 and Stage_Id =" + (Convert.ToInt32(strStageId) - 1);
                dtableApprovedDtls = dvApprovedView.ToTable();
                if (dtableApprovedDtls.Rows.Count > 0)
                {
                    _dblApprovedAmt = Math.Round(Convert.ToDouble(dtableApprovedDtls.Compute("sum(Approved_Amount)", "Approved_Amount>=0")), 2);
                }
            }
        }
        catch (Exception objException)
        {
            //throw objException;
        }
        return _dblApprovedAmt;
    }

    private void FunPriLoadApprovalDtls(String strApprovalID)
    {
        try
        {
            if (dictParam != null)
                dictParam.Clear();
            else
                dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@Approval_ID", Convert.ToString(strApprovalID));
            DataSet ds = Utility.GetDataset("S3G_LOANAD_GET_LOANENDUSEAPPROVAL", dictParam);
            
            if (ds == null)
            {
                return;
            }
            DataTable dtableGeneral = ds.Tables[0];//Header Details
            if (dtableGeneral != null && dtableGeneral.Rows.Count > 0)
            {
                txtComponents.Text = Convert.ToString(dtableGeneral.Rows[0]["Components"]);
                //txtCustomerCode.Text = Convert.ToString(dtableGeneral.Rows[0]["Customer_ID"]);
                txtEndUseDate.Text = Convert.ToString(dtableGeneral.Rows[0]["End_Use_Date"]);
                txtEndUseNumber.Text = Convert.ToString(dtableGeneral.Rows[0]["End_Use_Number"]);

                ddlLOB.Items.Insert(0, Convert.ToString(dtableGeneral.Rows[0]["LOB_Name"]));
                ddlBranch.Items.Insert(0, Convert.ToString(dtableGeneral.Rows[0]["Location"]));
                ddlPAN.Items.Insert(0, Convert.ToString(dtableGeneral.Rows[0]["PANUM"]));
                ddlAmountUtilized.Items.Insert(0, Convert.ToString(dtableGeneral.Rows[0]["Amount_Utilized"]));

                FunPriLoadCustomerEntityDtls(Convert.ToString(dtableGeneral.Rows[0]["Customer_ID"]));

            }

            DataTable dtablePymtdtls = ds.Tables[1];//Payment Details
            if (dtablePymtdtls != null && dtablePymtdtls.Rows.Count > 0)
            {
                pnlPaymentDetails.Visible = true;
                grvPaymentDetails.DataSource = dtablePymtdtls;
                grvPaymentDetails.DataBind();
            }

            DataTable dtableApproveddtls = ds.Tables[2];//Approval Details
            if (dtableApproveddtls != null && dtableApproveddtls.Rows.Count > 0)
            {
                ViewState["LoanApprovedDtls"] = dtableApproveddtls;
                pnlLoanApprovedDtls.Visible = true;
                grvPaymentApprovedDetails.DataSource = dtableApproveddtls;
                grvPaymentApprovedDetails.DataBind();
                grvPaymentApprovedDetails.FooterRow.Visible = false;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriToggleQueryMode()
    {
        try
        {
            txtComponents.ReadOnly = txtCustomerCode.ReadOnly = txtEndUseDate.ReadOnly = txtEndUseNumber.ReadOnly = true;
            btnSave.Enabled = btnClear.Enabled = btnCreateCustomer.Visible = false;
            grvPaymentApprovedDetails.Columns[14].Visible = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    #region "DropDownList Events"

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlBranch.Items.Count > 0)
                ddlBranch.SelectedIndex = 0;
            ddlBranch.Items.Clear();
            if (ddlLOB.SelectedIndex > 0)
            {
                FunPubFillBranch();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ucCustomerCodeLov.FunPubClearControlValue();
            ucCustomerAddress.ClearCustomerDetails();
            ddlPAN.Items.Clear();
            FunPriClearGrid();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void ddlPAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearGrid();
            if (Convert.ToString(ddlPAN.SelectedValue) != "0")
            {
                dictParam = new Dictionary<string, string>();
                dictParam.Clear();
                dictParam.Add("@OPTION", "1");
                dictParam.Add("@Company_ID", intCompanyID.ToString());
                dictParam.Add("@Customer_ID", Convert.ToString(ViewState["CustomerID"]));
                dictParam.Add("@PANUM", ddlPAN.SelectedValue.ToString());
                dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                DataSet ds = Utility.GetDataset("S3G_LOANAD_GET_PANUMOUTFLOWPAID_DTLS", dictParam);

                DataTable dtablePymtDtls = ds.Tables[0];
                if (dtablePymtDtls.Rows.Count > 0)
                {
                    ViewState["PaymentDetails"] = dtablePymtDtls;
                    pnlPaymentDetails.Visible = true;
                    grvPaymentDetails.DataSource = dtablePymtDtls;
                    grvPaymentDetails.DataBind();

                    if (grvPaymentApprovedDetails != null && grvPaymentApprovedDetails.FooterRow != null)
                    {
                        //Bind Stage Name
                        DropDownList ddlFooterStageName = (DropDownList)grvPaymentApprovedDetails.FooterRow.FindControl("ddlFooterStageName");
                        ddlFooterStageName.Items.Clear();                       
                        ddlFooterStageName.BindDataTable(dtablePymtDtls, new string[] { "Stage_No", "Stage_Description" });
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "No Payment Details found against selected Prime Account Number");
                    ddlPAN.SelectedIndex = -1; 
                    ViewState["PaymentDetails"] = null;
                }

                DataTable dtableApprovedDtls = ds.Tables[1];
                if (dtableApprovedDtls != null && dtableApprovedDtls.Rows.Count > 0)
                {
                    ViewState["LoanApprovedDtls"] = dtableApprovedDtls;
                    FunPriBindLoanApprovedDtls(dtableApprovedDtls);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region "Button Events"

    protected void btnCreateCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = ucCustomerCodeLov.FindControl("hdnID") as HiddenField;
            if (hdnCustomerId != null)
            {
                ViewState["CustomerID"] = hdnCustomerId.Value;
                FunPriLoadCustomerEntityDtls(hdnCustomerId.Value);
                LoadPrimeAccNo(hdnCustomerId.Value);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(ViewState["DocumentationPath"]) == "")
            {
                Utility.FunShowAlertMsg(this, "Document Path not yet Defined");
                return;
            }                

            DataTable dtableLoanApprovedDtls = (DataTable)ViewState["LoanApprovedDtls"];
            if (dtableLoanApprovedDtls == null)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one Approval Details");
                return;
            }
            else if (dtableLoanApprovedDtls.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one Approval Details");
                return;
            }
            else if (Convert.ToString(dtableLoanApprovedDtls.Rows[0]["Stage_ID"]) == "" || dtableLoanApprovedDtls.Rows[0]["Stage_ID"].Equals("0"))
            {
                Utility.FunShowAlertMsg(this, "Add atleast one Approval Details");
                return;
            }

            if (ViewState["PaymentDetails"] != null)
            {
                DataTable dtablePmtDtls=(DataTable)ViewState["PaymentDetails"];
                for (int i = 0; i < dtablePmtDtls.Rows.Count; i++)
                {
                    double dblCurrentAprvdAmt = 0;
                    string strStageId = Convert.ToString(dtablePmtDtls.Rows[i]["Stage_No"]);
                    double dblExistApprovedAmt = Convert.ToDouble(dtablePmtDtls.Rows[i]["Approved_Amt"]);
                    if (Convert.ToString(dtableLoanApprovedDtls.Compute("sum(Approved_Amount)", "Approval_ID = 0 and Approved_Amount>=0 and Stage_ID=" + strStageId)) != "")
                        dblCurrentAprvdAmt = Math.Round(Convert.ToDouble(dtableLoanApprovedDtls.Compute("sum(Approved_Amount)", "Approval_ID = 0 and Approved_Amount>=0 and Stage_ID=" + strStageId)), 2);
                    dblCurrentAprvdAmt = dblCurrentAprvdAmt + dblExistApprovedAmt;
                    dtablePmtDtls.Rows[i]["Approved_Amt"] = Convert.ToString(dblCurrentAprvdAmt);
                }
            }

            FunPriSaveApprovalDtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else
                Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=LEUA");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region "LinkButton Events"

    protected void lnkbtnStageId_Click(object sender, EventArgs e)
    {
        try
        {
            int intRowID = Utility.FunPubGetGridRowID("grvPaymentDetails", ((LinkButton)sender).ClientID.ToString());
            LinkButton lnkbtnStageId = (LinkButton)grvPaymentDetails.Rows[intRowID].FindControl("lnkbtnStageId");
            Label lblStageAmount = (Label)grvPaymentDetails.Rows[intRowID].FindControl("lblStageAmount");
            Label lblPaidAmount = (Label)grvPaymentDetails.Rows[intRowID].FindControl("lblPaidTillNow");
            string _strStageId = Convert.ToString(lnkbtnStageId.Text);

            if (Convert.ToString(lblStageAmount.Text) != "" && Convert.ToString(lblPaidAmount.Text) != "")
            {
                double dblBalanceStageAmt = Convert.ToDouble(lblStageAmount.Text) - Convert.ToDouble(lblPaidAmount.Text);
                ViewState["StageBalanceAmt"] = dblBalanceStageAmt.ToString();
            }
            else
            {
                ViewState["StageBalanceAmt"] = "0";
            }
            DataTable dtableApprovedDtls = (DataTable)ViewState["LoanApprovedDtls"];
            int cnt = (dtableApprovedDtls != null) ? dtableApprovedDtls.Rows.Count : 0;
            if (cnt == 0)
                dtableApprovedDtls = FunPriApprovedDtls("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
            else
            {
                int qryExists = (from LoanApprovedDetails in dtableApprovedDtls.AsEnumerable()
                                 where (LoanApprovedDetails.Field<string>("Stage_ID") == _strStageId)
                                 select LoanApprovedDetails).Count();
                if (qryExists > 0)
                {
                    Utility.FunShowAlertMsg(this, "Stage already exists");
                    return;
                }
            }
            FunPriBindLoanApprovedDtls(dtableApprovedDtls);
            Label lblFooterStageId = (Label)grvPaymentApprovedDetails.FooterRow.FindControl("lblFooterStageId");
            Label lblFooterStageName = (Label)grvPaymentApprovedDetails.FooterRow.FindControl("lblFooterStageName");
            lblFooterStageId.Text = lnkbtnStageId.Text;
            lblFooterStageName.Text = lnkbtnStageId.Text;
        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }
        
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Label lblFooterStageId = (Label)grvPaymentApprovedDetails.FooterRow.FindControl("lblFooterStageId");
            //Label lblFooterStageName = (Label)grvPaymentApprovedDetails.FooterRow.FindControl("lblFooterStageName");ddlFooterStageName
            DropDownList ddlFooterStageName = (DropDownList)grvPaymentApprovedDetails.FooterRow.FindControl("ddlFooterStageName");
            TextBox txtApprovedAmount = (TextBox)grvPaymentApprovedDetails.FooterRow.FindControl("txtApprovedAmount");
            TextBox txtApprovedBy = (TextBox)grvPaymentApprovedDetails.FooterRow.FindControl("txtApprovedBy");
            TextBox txtApprovedDate = (TextBox)grvPaymentApprovedDetails.FooterRow.FindControl("txtApprovedDate");
            TextBox txtRemarks = (TextBox)grvPaymentApprovedDetails.FooterRow.FindControl("txtRemarks");
            DropDownList ddlFooterEndUse = (DropDownList)grvPaymentApprovedDetails.FooterRow.FindControl("ddlFooterEndUse");
            DropDownList ddlFooterAction = (DropDownList)grvPaymentApprovedDetails.FooterRow.FindControl("ddlFooterAction");
            CheckBox chkPhoto = (CheckBox)grvPaymentApprovedDetails.FooterRow.FindControl("chkPhoto");
            CheckBox chkDocument = (CheckBox)grvPaymentApprovedDetails.FooterRow.FindControl("chkDocument");
            AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grvPaymentApprovedDetails.FooterRow.FindControl("asyFilePhotoUpload");
            AjaxControlToolkit.AsyncFileUpload AsyncFileUpload2 = (AjaxControlToolkit.AsyncFileUpload)grvPaymentApprovedDetails.FooterRow.FindControl("asyFileDocumentUpload");

            DataTable dtablePaymentDtls = (DataTable)ViewState["PaymentDetails"];
            double _dblBalanceStageAmount = 0;

            if (dtablePaymentDtls != null && dtablePaymentDtls.Rows.Count > 0)
            {
                //Get Selected Stage Row Index
                Int32 rowidx = new System.Data.DataView(dtablePaymentDtls).ToTable(false, new[] { "Stage_No" })
                .AsEnumerable()
                .Select(row => row.Field<Int32>("Stage_No")) 
                .ToList()
                .FindIndex(col => col == Convert.ToInt32(ddlFooterStageName.SelectedValue));
                if (rowidx >= 0)
                {
                    double _dblStageAmt = (rowidx == 0) ? 0 : Convert.ToDouble(dtablePaymentDtls.Rows[rowidx - 1]["Stage_amount"]);
                    double _dblStageApvdAmt = (rowidx == 0) ? 0 : Convert.ToDouble(dtablePaymentDtls.Rows[rowidx - 1]["Approved_Amt"]);
                    if (_dblStageApvdAmt < _dblStageAmt)
                    {
                        double _dblAppvalAmt = FunPriCheckSatgeApprovedAmt(Convert.ToString(ddlFooterStageName.SelectedValue));
                        if (_dblAppvalAmt < _dblStageAmt)
                        {
                            Utility.FunShowAlertMsg(this, "Previous Stage not approved completely");
                            return;
                        }
                    }

                    double _dblExistApprovedAmt = FunPriGetSatgeApprovedAmt(Convert.ToString(ddlFooterStageName.SelectedValue));
                    _dblStageAmt = Convert.ToDouble(dtablePaymentDtls.Rows[rowidx]["Stage_amount"]);
                    double _dblPaidAmt = Convert.ToDouble(dtablePaymentDtls.Rows[rowidx]["Paid_Amount"]);
                    double _dblApprovedAmt = Convert.ToDouble(dtablePaymentDtls.Rows[rowidx]["Approved_Amt"]);
                    _dblBalanceStageAmount = _dblStageAmt - (_dblExistApprovedAmt + _dblApprovedAmt);
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Selected stage doesnot exists in Payment Details");
                    return;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Stage Cant be approved without payment Details");
                return;
            }
            //double dblBalanceStageAmount = Convert.ToDouble(ViewState["StageBalanceAmt"].ToString());
            double dblApprovedAmt = Convert.ToDouble(txtApprovedAmount.Text);

            if (Convert.ToString(ddlFooterStageName.Text) == "")
            {
                Utility.FunShowAlertMsg(this, "Stage Name should not be empty");
                return;
            }

            if (Convert.ToInt32(ddlAmountUtilized.SelectedValue) == 0)
            {
                Utility.FunShowAlertMsg(this, "Select Amount Utilized");
                return;
            }
            else if (Convert.ToInt32(ddlAmountUtilized.SelectedValue) == 1)
            {
                if (dblApprovedAmt != _dblBalanceStageAmount)
                {
                    Utility.FunShowAlertMsg(this, "Partial Approval is not allowed");
                    return;
                }
            }
            else if (dblApprovedAmt > _dblBalanceStageAmount)
            {
                Utility.FunShowAlertMsg(this, "Approval amount should not be greater than Stage Balance Amount");
                return;
            }
            string _strStageID = Convert.ToString(ddlFooterStageName.SelectedValue);
            string _strStageName = Convert.ToString(ddlFooterStageName.SelectedItem.Text);
            string _strApprovedAmount = Convert.ToString(txtApprovedAmount.Text);
            string _strApproveredID = Convert.ToString(hdnUserId.Value);
            string _strApprovedBy = Convert.ToString(txtApprovedBy.Text);
            string _strApprovedDate = Convert.ToString(txtApprovedDate.Text);
            string _strRemarks = Convert.ToString(txtRemarks.Text);
            string _strPhotoUpload = string.Empty;
            string _strDocumentUpload = string.Empty;
            string _strAction = Convert.ToString(ddlFooterAction.SelectedItem.Text);
            string _strEndUse = Convert.ToString(ddlFooterEndUse.SelectedItem.Text);
            string _strActionID = Convert.ToString(ddlFooterAction.SelectedValue);
            string _strEndUseID = Convert.ToString(ddlFooterEndUse.SelectedValue);
            
            if (Convert.ToString(ViewState["DocumentationPath"]) == "")
            {
                Utility.FunShowAlertMsg(this, "Document Path not yet Defined");
                return;
            }    

            #region "Upload Photo"
            if (chkPhoto.Checked == true && AsyncFileUpload1 != null)
            {
                if (AsyncFileUpload1.FileName != "")
                {
                    FileInfo fileInfo = new FileInfo(AsyncFileUpload1.FileName);
                    if (AsyncFileUpload1.HasFile)
                    {
                        string fileExtension = AsyncFileUpload1.FileName.Substring(AsyncFileUpload1.FileName.LastIndexOf('.') + 1);
                        if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                        {
                            Utility.FunShowAlertMsg(this, "File extension not supported, only image & pdf files should be uploaded.");
                            return;
                        }
                        else
                        {
                            string strNewFileName = @"\COMPANY" + intCompanyID.ToString();
                            string strPath = "";
                            strPath = Convert.ToString(ViewState["DocumentationPath"]) + strNewFileName;
                            if (!Directory.Exists(strPath))
                            {
                                Directory.CreateDirectory(strPath);
                            }

                            strPath += @"\" + AsyncFileUpload1.FileName.Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + AsyncFileUpload1.FileName.Split('.')[1].ToString();

                            AsyncFileUpload1.SaveAs(strPath);
                            _strPhotoUpload = strPath;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Photo to Upload');", true);
                    return;
                }
            }
            AsyncFileUpload1.Attributes.Clear();
            #endregion

            #region "Upload Document"
            if (chkDocument.Checked == true && AsyncFileUpload2 != null)
            {
                if (AsyncFileUpload2.FileName != "")
                {
                    FileInfo fileInfo = new FileInfo(AsyncFileUpload2.FileName);
                    if (AsyncFileUpload2.HasFile)
                    {
                        string fileExtension = AsyncFileUpload2.FileName.Substring(AsyncFileUpload2.FileName.LastIndexOf('.') + 1);
                        if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                        {
                            Utility.FunShowAlertMsg(this, "File extension not supported, only image & pdf files should be uploaded.");
                            return;
                        }
                        else
                        {
                            string strNewFileName = @"\COMPANY" + intCompanyID.ToString();
                            string strPath = "";
                            strPath = Convert.ToString(ViewState["DocumentationPath"]) + strNewFileName;
                            if (!Directory.Exists(strPath))
                            {
                                Directory.CreateDirectory(strPath);
                            }

                            strPath += @"\" + AsyncFileUpload2.FileName.Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + AsyncFileUpload2.FileName.Split('.')[1].ToString();

                            AsyncFileUpload2.SaveAs(strPath);
                            _strDocumentUpload = strPath;
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Document to Upload');", true);
                    return;
                }
            }
            AsyncFileUpload2.Attributes.Clear();

            ClearContents(AsyncFileUpload1);
            ClearContents(AsyncFileUpload2);
            #endregion

            DataTable dtableApprovedDtls = FunPriApprovedDtls("0", _strStageID, _strStageName, _strApprovedAmount, _strApproveredID, _strApprovedBy,
                _strApprovedDate, _strRemarks, _strPhotoUpload, _strDocumentUpload, _strEndUse, _strEndUseID, _strAction, _strActionID);
            FunPriBindLoanApprovedDtls(dtableApprovedDtls);

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }
    
    protected void lnkRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int intRowID = Utility.FunPubGetGridRowID("grvPaymentApprovedDetails", ((LinkButton)sender).ClientID.ToString());
            LinkButton lnkRemove = (LinkButton)grvPaymentApprovedDetails.Rows[intRowID].FindControl("lnkRemove");
            DataTable dtableLoanApprvedDtls = (DataTable)ViewState["LoanApprovedDtls"];
            if (dtableLoanApprvedDtls != null && dtableLoanApprvedDtls.Rows.Count > 0)
            {
                dtableLoanApprvedDtls.Rows.RemoveAt(intRowID);
                ViewState["LoanApprovedDtls"] = dtableLoanApprvedDtls;
                if (dtableLoanApprvedDtls.Rows.Count > 0)
                {
                    //grvPaymentApprovedDetails.DataSource = dtableLoanApprvedDtls;
                    //grvPaymentApprovedDetails.DataBind();
                    FunPriBindLoanApprovedDtls(dtableLoanApprvedDtls);
                }
                else
                {
                    dtableLoanApprvedDtls = FunPriApprovedDtls("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
                    FunPriBindLoanApprovedDtls(dtableLoanApprvedDtls);
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
        }
    }

    #endregion

    #region "AsyncFileUpload Events"

    protected void asyFilePhotoUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    protected void asyFileDocumentUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    //Clear AsyncFileUpload Control
    private void ClearContents(Control control)
    {
        for (var i = 0; i < Session.Keys.Count; i++)
        {
            if (Session.Keys[i].Contains(control.ClientID))
            {
                Session.Remove(Session.Keys[i]);
                break;
            }
        }
    }

    #endregion



    protected void grvPaymentApprovedDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindApprovedDetails(e);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    private void FunPriBindApprovedDetails(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Bind Action Details
                DropDownList ddlItemAction = (DropDownList)e.Row.FindControl("ddlItemAction");
                LinkButton lnkRemove = (LinkButton)e.Row.FindControl("lnkRemove");
                ddlItemAction.Items.Clear();
                if (dictParam != null)
                    dictParam.Clear();
                else
                    dictParam = new Dictionary<string, string>();

                dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictParam.Add("@LookupType_Code", "123");//Loen End Use Action
                ddlItemAction.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, dictParam, new string[] { "Lookup_Code", "Lookup_Description" });

                DataTable dtableAprvd = (DataTable)ViewState["LoanApprovedDtls"];
                if (dtableAprvd != null && dtableAprvd.Rows.Count > 0)
                {
                    ddlItemAction.SelectedValue = Convert.ToString(dtableAprvd.Rows[e.Row.RowIndex]["Action_ID"]);
                }

                if (Convert.ToString(strMode) == "Q")
                {
                    ddlItemAction.ClearDropDownList();
                }
                else if (Convert.ToString(strMode).ToUpper() == "C")
                {
                    if (Convert.ToString(dtableAprvd.Rows[e.Row.RowIndex]["Approval_ID"]) != "0" && Convert.ToString(dtableAprvd.Rows[e.Row.RowIndex]["Action_ID"]) == "1")
                    {
                        ddlItemAction.ClearDropDownList();
                        lnkRemove.Enabled = false;
                    }
                    else if (Convert.ToString(dtableAprvd.Rows[e.Row.RowIndex]["Approval_ID"]) != "0" && Convert.ToString(dtableAprvd.Rows[e.Row.RowIndex]["Action_ID"]) == "2")
                    {
                        ddlItemAction.Items.RemoveAt(0);
                        lnkRemove.Enabled = false;
                    }
                    else if (Convert.ToString(dtableAprvd.Rows[e.Row.RowIndex]["Approval_ID"]) == "0")
                    {
                        ddlItemAction.Items.RemoveAt(0);
                        lnkRemove.Enabled = true;
                    }
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void ddlItemAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int intRowID = Utility.FunPubGetGridRowID("grvPaymentApprovedDetails", ((DropDownList)sender).ClientID.ToString());
            Label lblAction = (Label)grvPaymentApprovedDetails.Rows[intRowID].FindControl("lblAction");
            Label lblActionID = (Label)grvPaymentApprovedDetails.Rows[intRowID].FindControl("lblActionID");
            DropDownList ddlItemAction = (DropDownList)grvPaymentApprovedDetails.Rows[intRowID].FindControl("ddlItemAction");
            DataTable dtabelApprovedDtls = (DataTable)ViewState["LoanApprovedDtls"];
            if (dtabelApprovedDtls != null && dtabelApprovedDtls.Rows.Count > 0)
            {
                dtabelApprovedDtls.Rows[intRowID]["Action_ID"] = ddlItemAction.SelectedValue;
                dtabelApprovedDtls.Rows[intRowID]["Action"] = ddlItemAction.SelectedItem.Text;
                lblAction.Text = ddlItemAction.SelectedItem.Text;
                lblActionID.Text = ddlItemAction.SelectedValue;
                ViewState["LoanApprovedDtls"] = dtabelApprovedDtls;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }   

    protected void hyplnkView_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvPaymentApprovedDetails", strFieldAtt);
            Label lblPath = (Label)grvPaymentApprovedDetails.Rows[gRowIndex].FindControl("lblPhotoProof");

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(strLoanEndUseID.ToString(), false, 0);

            if (lblPath.Text.Trim() != "")
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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Loan End Use Approval");
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void hyplnkDocView_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvPaymentApprovedDetails", strFieldAtt);
            Label lblPath = (Label)grvPaymentApprovedDetails.Rows[gRowIndex].FindControl("lblDocumentProof");

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(strLoanEndUseID.ToString(), false, 0);

            if (lblPath.Text.Trim() != "")
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
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Loan End Use Approval");
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlAmountUtilized_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtableAprvedDtls = (DataTable)ViewState["LoanApprovedDtls"];
            DataView dvApprovedView = new DataView(dtableAprvedDtls);
            dvApprovedView.RowFilter = "Approval_ID > 0";
            dtableAprvedDtls = dvApprovedView.ToTable();
            if (dtableAprvedDtls.Rows.Count > 0)
            {
                ViewState["LoanApprovedDtls"] = dtableAprvedDtls;
                FunPriBindLoanApprovedDtls(dtableAprvedDtls);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }
}
