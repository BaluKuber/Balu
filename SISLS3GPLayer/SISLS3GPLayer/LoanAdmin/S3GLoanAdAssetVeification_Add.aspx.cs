
    #region Header

//Module Name      :   Loan Admin
//Screen Name      :   S3GLoanAdAssetVeification_Add.aspx
//Created By       :   Saisree
//Created Date     :   03-Sep-2010
//Purpose          :   To insert and update Assert Verification

//Modified by      :  Swarnalatha  BM
//Modified on      :  18-Feb-2011
//Purpose          :  Bug fixation Round 1

#endregion


    #region Namespaces

using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.IO;
using System.Configuration;
using S3GBusEntity.LoanAdmin;
using System.Globalization;
using System.Web.UI.WebControls;

#endregion

public partial class LoanAdmin_S3GLOANADAssetVeification_Add : ApplyThemeForProject
{
    #region Declaration

    int intCompanyID, intUserID = 0, intErrCode = 0;
    public Dictionary<string, string> Procparam = null;
    string intAssetVerificationNo = string.Empty;

    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    HiddenField hdnCustomerId;
    AssetMgtServicesReference.AssetMgtServicesClient objAssetVerification_Client;
    AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsDataTable objS3G_LOANAD_AssetDataTable = null;
    AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsRow objS3G_LOANAD_AssetDataRow = null;
    ClsSystemJournal ObjSysJournal = new ClsSystemJournal();

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=ASV";
    public string strmode = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLOANADAssetVeification_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ASV';";
    public string strCustid;
    //User Authorization
    public  string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    #endregion      

    #region Page Loading

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriPageLoad();
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    #region  Save,Cancel,Clear
    /// <summary>
    /// to load Customer 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != string.Empty)
            {
                ViewState["FromCustomer"] = true;
                hdnCustID.Value = hdnCustomerId.Value;
                ddlMLA.SelectedIndex = 0;
                if(ddlMLA.Items.Count>0)
                ddlMLA.ClearDropDownList();
                ddlSLA.Items.Clear();
                ddlLOB.SelectedIndex = 0;
                ddlBranch.SelectedIndex = 0;
                ddlAssetCode.Items.Clear();
                PopulatePANCustomer(hdnCustomerId.Value, false);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            // wf cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else
                Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunClear(1);
            TextBox tx = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            tx.Text = string.Empty;
            txtHour.Text = "12";
            txtMins.Text = "00";
            ddlAMPM.SelectedValue = "AM";
            ViewState["FromCustomer"] = null;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion    
       
    #region DropDownList Events

    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriddlMLAchanged();
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriSLAChanged();
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["FromCustomer"] == null)
            {
                ucCustDetails.ClearCustomerDetails();
                ucCustomerCodeLov.FunPubClearControlValue();
            }
            PopulatePANum();
            if (ddlBranch.SelectedValue.Equals("0"))
            {
                ddlMLA.SelectedIndex = 0;
                ddlMLA.ClearDropDownList();
                ddlSLA.Items.Clear();
                ddlAssetCode.Items.Clear();
            }
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
            if (ViewState["FromCustomer"] == null)
            {
                ucCustDetails.ClearCustomerDetails();
                ucCustomerCodeLov.FunPubClearControlValue();
            }

            PopulatePANum();
            if (ddlLOB.SelectedValue.Equals("0"))
            {
                ddlMLA.SelectedIndex = 0;
                ddlMLA.ClearDropDownList();
                ddlAssetCode.Items.Clear();
            }

        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void ddlInspBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriInspection();
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //protected void ddlCustCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //PopulatePANum();
    //    ddlMLA.Items.Clear();
    //    ddlLOB.SelectedIndex = ddlBranch.SelectedIndex = 0;
    //    ucCustDetails.ClearCustomerDetails();
    //    //txtName.Text = txtCity.Text = txtAddress.Text = txtAddress2.Text = txtCountry.Text = txtCustCode.Text =
    //    //    txtName.Text = txtState.Text = txtCountry.Text = string.Empty;
    //}

    #endregion

    #region Custom Validation

    protected void custCheck_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        try
        {
            int intLength = 0;
            intLength = (txtLocation.Text.Length + txtRemarks.Text.Length + txtAVDate.Text.Length);
            if (intLength > 350)
            {
                args.IsValid = false;
                custCheck.ErrorMessage = Resources.LocalizationResources.SizeCheckforfollowup;
            }
            else if (Utility.StringToDate(txtInspDate.Text).ToString("dd-MMM-yyyy") == DateTime.Today.ToString("dd-MMM-yyyy") &&
             (Convert.ToDateTime(DateTime.Now.ToString("hh:mm tt").ToString()) <
                    Convert.ToDateTime(Utility.StringToDate(txtInspDate.Text).ToString("dd-MMM-yyyy") + " " + txtHour.Text + ":" + txtMins.Text + " " + ddlAMPM.SelectedValue.ToString())))
            {
                args.IsValid = false;
                custCheck.ErrorMessage = "Inspection time should be lesser than the System time [" + DateTime.Now.ToString("hh:mm tt") + "].";
                txtHour.Focus();
            }
            else if (ViewState["CreationDate"] != null &&
                Convert.ToDateTime(Utility.StringToDate(ViewState["CreationDate"].ToString()).ToString("dd-MMM-yyyy")) > Convert.ToDateTime(Utility.StringToDate(txtInspDate.Text).ToString("dd-MMM-yyyy")))
            {
                args.IsValid = false;
                custCheck.ErrorMessage = "Inspection date should be greater than the Account creation date [" + Utility.StringToDate(ViewState["CreationDate"].ToString()).ToString(strDateFormat) + "].";
                txtHour.Focus();
            }
            else
            {
                args.IsValid = true;
            }
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    #region User Defined Functions
    /// <summary>
    /// To populate asset status and inspected by values
    /// </summary>
    private void PopulateLookupDescriptions()
    {
        //throw new NotImplementedException();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();//Asset status
            Procparam.Add("@LookupType_Code", "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            ddlAssetStatus.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

            Procparam = new Dictionary<string, string>();//Inspected By
            //Procparam.Add("@LookupType_Code", "68");
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //ddlInspBy.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            ddlInspBy.BindDataTable("S3G_LOANAD_GetInspectorsType", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }      

    /// <summary>
    /// To Populate Prime account number
    /// </summary>
    private void PopulatePANum()
    {
        //throw new NotImplementedException();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Type", "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Is_Activated", "2");
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
            //if (!hdnCustID.Value.Equals(string.Empty))
            if (ViewState["FromCustomer"] != null) 
                Procparam.Add("@Customer_ID", hdnCustID.Value);
            if (!ddlLOB.SelectedValue.Equals("0") && !ddlBranch.SelectedValue.Equals("0"))
                ddlMLA.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "PANum", "PANum" });//S3G_LOANAD_GetPANum
            if (ddlSLA.Items.Count > 0) ddlSLA.ClearDropDownList();

            ddlSLA.Items.Clear();
            ddlAssetCode.Items.Clear();
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriddlMLAchanged()
    {
        try
        {
            ddlAssetCode.Items.Clear();
            ddlSLA.Items.Clear();

            if (ViewState["FromCustomer"] == null)
            {
                ucCustDetails.ClearCustomerDetails();
                ucCustomerCodeLov.FunPubClearControlValue();
            }

            if (!ddlMLA.SelectedValue.Equals("0"))
            {
                PopulatePANCustomer(ddlMLA.SelectedValue, false);
                PopulateSANum(ddlMLA.SelectedValue);
                //PopulatePANCustomer(ddlMLA.SelectedValue);
                if (ddlSLA.Items.Count == 1 || ddlSLA.SelectedValue == "0")
                    PopulateAssetCodebyLOB();
                //{
                //    if (ddlLOB.SelectedValue == "4")
                //    {  PopulateAssetCodeForOL();      }
                //    else
                //    {  PopulateAssetCode();           }
                //if (ddlLOB.SelectedItem.Text.ToLower().Trim() != "ol  -  operatinglease")
                //    PopulateAssetCode();
                //else
                //    PopulateAssetCodeForOL();
                //  }
            }
            //else
            //{
            //    ddlSLA.SelectedIndex  = 0;
            //    if (ddlSLA.Items.Count > 0) ddlSLA.ClearDropDownList();
            //    ddlAssetCode.SelectedIndex  = 0;
            //    if (ddlAssetCode.Items.Count > 0) ddlAssetCode.ClearDropDownList();
            //ucCustDetails.ClearCustomerDetails();
            // txtCustCode.Text = txtCountry.Text = txtCity.Text = txtAddress.Text = txtAddress2.Text = txtPinCode.Text =
            // txtState.Text = txtName.Text = string.Empty;
            // }
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }  
    private void PopulateSANum(string strPAN)
    {
        //throw new NotImplementedException();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Type", "2");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Is_Activated", "1");
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Customer_ID", hdnCustID.Value);
            //Procparam.Add("@User_ID", intUserID.ToString()); 
            Procparam.Add("@PANum", strPAN);
            //getsanum
            if (!ddlMLA.SelectedValue.Equals("0") && (!hdnCustID.Value.Equals(string.Empty)))
            {
                ddlSLA.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "SANum", "SANum" });//S3G_LOANAD_GetSANum
            }
            else
            {
                ddlAssetCode.Items.Clear();
            }



        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }     
    private void PopulateAssetCodebyLOB()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            if (ddlSLA.SelectedValue.ToString() == "0")
            {
                Procparam.Add("@SANum", ddlMLA.SelectedValue.ToString() + "DUMMY");
            }
            else
            {
                Procparam.Add("@SANum", ddlSLA.SelectedValue.ToString());
            }

            DataTable dt = Utility.GetDefaultData("S3G_CLN_GetAccountCreationDate", Procparam);
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewState["CreationDate"] = dt.Rows[0][0].ToString();
            }

            if (Utility.FunGetValueBySplit(ddlLOB.SelectedItem.Text) != "OL")
                PopulateAssetCode();
            else
                PopulateAssetCodeForOL();
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }     
    private void FunPriSLAChanged()
    {
        try
        {
            ddlAssetCode.Items.Clear();
            if (!ddlSLA.SelectedValue.Equals("0"))
                PopulateAssetCodebyLOB();
           

        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
       
    }    
    private void FunPriPageLoad()
    {
        S3GSession ObjS3GSession = null;
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPubSetIndex(1);
            //Date Format
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender1.Format = strDateFormat;
            //End
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                intAssetVerificationNo = fromTicket.Name;
            }

            //txtVerification.Attributes.Add("onblur", "ChkIsZero(this)");
            //txtMemo.Attributes.Add("onblur", "ChkIsZero(this)");

            #region " WF INITIATION"
            ProgramCode = "052";

            if (PageMode == PageModes.WorkFlow && !IsPostBack)
            {
                try
                {
                    PreparePageForWorkFlowLoad();
                    ViewState["PageMode"] = PageModes.WorkFlow;
                }
                catch (Exception ex)
                {
                      ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                    Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
                }
            }
            #endregion

            if (!IsPostBack)
            {
                
                FunLoadErrorMessage();
                PopulateBranchList();
                PopulateLOBList();

                ViewState["FromCustomer"] = null;
                // PopulateCustomer();
                PopulateLookupDescriptions();
                txtVerification.CheckGPSLength(false);
                txtMemo.CheckGPSLength(false);
                //txtMemo.SetDecimalPrefixSuffix(10, 2, true,"Memo Charges");
                txtAVDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                
                if (Request.QueryString["qsMode"] != null)
                strmode = Request.QueryString["qsMode"];
                if (strmode.Equals("Q"))
                {

                    FunVerifedAssetForModification(intAssetVerificationNo);
                    FunPriDisableControls(-1);
                }
                else if (strmode.Equals("M"))
                {
                    FunVerifedAssetForModification(intAssetVerificationNo);
                    FunPriDisableControls(1);
                }
                else if (strmode.Equals("C"))
                {
                    FunPriDisableControls(0);
                }
            }
        }
        catch (Exception ex)
        {
             
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #region Workflow Methods
    /// <summary>
    /// Workflow Function
    /// </summary>
    private void PreparePageForWorkFlowLoad()
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            // Get The IDVALUE from Document Sequence #
            DataRow HeaderValues = GetHeaderDetailsFromDOCNo(WFSessionValues.PANUM, ProgramCode);

            PopulatePANCustomer(HeaderValues["Customer_Id"].ToString(),true);

            PopulateLOBList();
            PopulateBranchList();             
            //ddlAssetStatus.SelectedValue = Convert.ToString(dtTable.Rows[0]["Asset_Status_code"]);

            hdnCustID.Value = HeaderValues["Customer_Id"].ToString();
            ddlLOB.SelectedValue = HeaderValues["Lob_Id"].ToString();
            ddlBranch.SelectedValue = HeaderValues["Branch_Id"].ToString();
                        
            PopulatePANum();          
            ddlMLA.SelectedValue = HeaderValues["UniqueId"].ToString();
            
            PopulateSANum(HeaderValues["UniqueId"].ToString());
            PopulateAssetCodebyLOB();
            //PopulatePANCustomer(HeaderValues["Customer_Id"].ToString());
        }
    }
    #endregion
    private void FunLoadErrorMessage()
    {
        try
        {
            rfvddlLOB.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LOB;
            rfvddlCust.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_CustomerCode;
            rfvddlBranch.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Branch;
            rfvddlMLA.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Select_PriAc;
            rfvAssetCode.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_AssetCode;
            rfvddlAssetStatus.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_AssetStatus;
            rfvddlInspBy.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_InspectionBy;
            rfvInspCode.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_InspectionCode;
            rfvInspecTime.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_InspectionTime;
            rfvInsTime.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Inspectiontimeformat;
            rfvDate.ErrorMessage = Resources.ValidationMsgs.S3g_ValMsg_Date;
            rfvLocation.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Location;
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }  
    private void PopulateAssetCode()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@PANum", ddlMLA.SelectedValue);
            if (ddlSLA.Items.Count == 1 && ddlSLA.SelectedValue.Equals("0"))
                Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
            else
                Procparam.Add("@SANum", ddlSLA.SelectedValue);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            if (!ddlMLA.SelectedValue.Equals("0"))
            {
                ddlAssetCode.Items.Clear();
                ddlAssetCode.BindDataTable(SPNames.S3G_LOANAD_GetSANAssetCode, Procparam, new string[] { "Asset_Number", "Asset_ID", "Asset_Description" });
                ddlAssetCode.SelectedIndex = 0;
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }  
    private void PopulateAssetCodeForOL()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            if (!ddlMLA.SelectedValue.Equals("0"))
            Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            if (ddlSLA.Items.Count == 1 && ddlSLA.SelectedValue.Equals("0"))
                Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
            else
                Procparam.Add("@SANum", ddlSLA.SelectedValue);
            if (!ddlMLA.SelectedValue.Equals("0"))
            {
                ddlAssetCode.Items.Clear();
                ddlAssetCode.BindDataTable(SPNames.S3G_LOANAD_GetSANAssetCodeForOL, Procparam, new string[] { "Lease_Asset_Details_ID", "Lease_Asset_No" });
            }

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }     
    private void PopulatePANCustomer(string strPAN,bool isWorkFlowCall)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(CompanyId));
            //Procparam.Add("@PANum", strPAN);
            if(!ddlMLA.SelectedValue.Equals("0") && PageMode.Equals(PageModes.Create))
            Procparam.Add("@Option", "56");
            Procparam.Add("@Param1", strPAN);
            DataTable dtTable = Utility.GetDefaultData("S3G_LOANAD_GetCommonCustomer", Procparam);

            if (dtTable.Rows.Count > 0)
            {
                ucCustDetails.SetCustomerDetails(dtTable.Rows[0], true);
                hdnCustID.Value = dtTable.Rows[0]["Customer_ID"].ToString();
                //if(isWorkFlowCall)
                    ucCustomerCodeLov.FunPubSetControlValue(strPAN, dtTable.Rows[0]["Customer_Code"].ToString());
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw (ex);
        }
    }     
    private void PopulateBranchList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Branch_ID", "Branch_Code", "Branch_Name" });
            

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #region PopulateLOBList
    /// <summary>
    /// to populate Line of business
    /// </summary>
    private void PopulateLOBList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));

            //if (intAssetVerificationNo.Equals(string.Empty))
                Procparam.Add("@Is_Active", "1");
                ddlLOB.BindDataTable(SPNames.S3G_LOANAD_LOBASSETIDEN, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                

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
    private void FunPriInspection()
    {
        try
        {
            if (intAssetVerificationNo != string.Empty)
            {
               
                if (!(ddlInspBy.SelectedValue.Equals("9")))
                {
                    txtVerification.ReadOnly = false;

                }
                else
                {
                    txtVerification.Text = string.Empty;
                    txtVerification.ReadOnly = true;
                }

            }
            
            PopulateInspCode(ddlInspBy.SelectedValue);
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }   
    private void PopulateInspCode(string strEntityType)
    {
        //throw new NotImplementedException();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));


            if (strEntityType.Equals("7"))
            {
                //if (ddlInspBy.SelectedItem.ToString().Equals("FIA"))
                //    Procparam.Add("@TypeDesc", "Field Surveyor");
                //else
                Procparam.Add("@TypeDesc", "3");
                ddlInspCode.BindDataTable(SPNames.S3G_LOANAD_GetEntity, Procparam, new string[] { "Entity_ID", "Entity_Code" });
            }
            else if (strEntityType.Equals("8"))
            {
                Procparam.Add("@TypeDesc", "7");
                ddlInspCode.BindDataTable(SPNames.S3G_LOANAD_GetEntity, Procparam, new string[] { "Entity_ID", "Entity_Code" });
            }
            else if (strEntityType.Equals("9"))
                ddlInspCode.BindDataTable(SPNames.S3G_LOANAD_GetEmployeeInspectorCode, Procparam, new string[] { "User_ID", "User_Code" });
            else
            {
                ddlInspCode.SelectedIndex = 0;
                if (ddlInspCode.Items.Count > 0)
                    ddlInspCode.ClearDropDownList();
            }

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }


    }   


    private void FunVerifedAssetForModification(string strAssetNo)
    {
        try
        {
            txtVerification.ReadOnly = txtMemo.ReadOnly = false;
            DataTable dtTable = new DataTable();
            objAssetVerification_Client = new AssetMgtServicesReference.AssetMgtServicesClient();

            byte[] bytesEnquiryDetails = objAssetVerification_Client.FunVerifedAssetForModification(strAssetNo);
            dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesEnquiryDetails, SerializationMode.Binary, typeof(DataTable));
            ddlAssetStatus.SelectedValue = Convert.ToString(dtTable.Rows[0]["Asset_Status_code"]);
            ddlLOB.SelectedValue = dtTable.Rows[0]["LOB_ID"].ToString();
            ddlBranch.SelectedValue = dtTable.Rows[0]["Branch_ID"].ToString();
            ddlInspBy.SelectedValue = dtTable.Rows[0]["Insepection_By_Code"].ToString();
            if (ddlInspBy.SelectedValue.ToString() == "9")
            {
                txtVerification.ReadOnly = true;
            }
            PopulateInspCode(ddlInspBy.SelectedValue);
            ddlInspCode.SelectedValue = dtTable.Rows[0]["Inspection_Code"].ToString();

            //PopulateCustomer();
            hdnCustID.Value = ddlCustCode.Text = dtTable.Rows[0]["Customer_ID"].ToString();
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Text = Convert.ToString(dtTable.Rows[0]["Customer_Code"]);
            txtName.Enabled = false;
            txtName.ToolTip = txtName.Text;
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            hdnCustomerId.Value = Convert.ToString(dtTable.Rows[0]["Customer_Id"]);

            PopulatePANum();
            ddlMLA.SelectedValue = dtTable.Rows[0]["PANum"].ToString();
            PopulateSANum(dtTable.Rows[0]["PANum"].ToString());
            ddlSLA.SelectedValue = dtTable.Rows[0]["SANum"].ToString();
            PopulateAssetCodebyLOB();

            //if (ddlLOB.SelectedValue == "4")
            // PopulateAssetCodeForOL();  
            //else
            // PopulateAssetCode();       
            ddlAssetCode.SelectedValue = dtTable.Rows[0]["Asset_ID"].ToString();

            txtAVNo.Text = dtTable.Rows[0]["Asset_Verification_No"].ToString();
            txtInspDate.Text = DateTime.Parse(dtTable.Rows[0]["Inspection_Date"].ToString().Split(' ')[0], CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            txtInspTime.Text = (dtTable.Rows[0]["Inspection_Date"].ToString().Split(' ')[1]).Split(':')[0] + ":" + (dtTable.Rows[0]["Inspection_Date"].ToString().Split(' ')[1]).Split(':')[1] + " " + dtTable.Rows[0]["Inspection_Date"].ToString().Split(' ')[2];
            string[] str = Convert.ToDateTime(dtTable.Rows[0]["Inspection_Date"].ToString()).ToString("HH:mm:ss").Split(':');

            if (Convert.ToInt32(str[0]) >= 12)                     // To check whether AM or PM
            {
                if (Convert.ToInt32(str[0]) > 12)                  // To keep it as 12
                {
                    txtHour.Text = (Convert.ToInt32(str[0]) - 12).ToString();
                }

                ddlAMPM.SelectedValue = "PM";

                if (txtHour.Text.Length == 1)
                {
                    txtHour.Text = "0" + txtHour.Text;
                }
            }
            else
            {
                if (str[0] == "00")
                {
                    txtHour.Text = "12";
                }
                else
                {
                    txtHour.Text = str[0];
                }
                ddlAMPM.SelectedValue = "AM";
            }
            txtMins.Text = str[1];

            txtAVDate.Text = DateTime.Parse(dtTable.Rows[0]["Asset_Verification_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            txtRemarks.Text = dtTable.Rows[0]["Remarks"].ToString();
            txtLocation.Text = dtTable.Rows[0]["Location"].ToString();
            PopulatePANCustomer(dtTable.Rows[0]["Customer_ID"].ToString(),false);
            //txtCustCode.Text = dtTable.Rows[0]["Customer_Code"].ToString();
            //txtAddress.Text = dtTable.Rows[0]["Customer_Address"].ToString();
            //txtAddress.Text = dtTable.Rows[0]["comm_address1"].ToString();
            //txtAddress2.Text = dtTable.Rows[0]["comm_address2"].ToString();
            //txtCity.Text = dtTable.Rows[0]["comm_city"].ToString();
            //txtState.Text = dtTable.Rows[0]["comm_state"].ToString();
            //txtCountry.Text = dtTable.Rows[0]["comm_country"].ToString();
            //txtPinCode.Text = dtTable.Rows[0]["comm_pincode"].ToString();
            //txtName.Text = dtTable.Rows[0]["Customer_Name"].ToString();
            //txtCustCode.Attributes.Add("Cust_ID", dtTable.Rows[0]["Customer_ID"].ToString());
            //txtAddress.Text = dtTable.Rows[0]["Customer_Address"].ToString();
            //if (Convert.ToString(dtTable.Rows[0]["Physically_Verified"]) == "False")
            chkVerified.Checked = Convert.ToBoolean(dtTable.Rows[0]["Physically_Verified"]);
            //else
            //    chkVerified.Checked = true;

            if (Convert.ToString(dtTable.Rows[0]["Verification_Charges"]) != string.Empty || dtTable.Rows[0]["Verification_Charges"] != null)
                txtVerification.Text = dtTable.Rows[0]["Verification_Charges"].ToString();
            if (Convert.ToString(dtTable.Rows[0]["Memo_Charges"]) != string.Empty || dtTable.Rows[0]["Memo_Charges"] != null)
                txtMemo.Text = dtTable.Rows[0]["Memo_Charges"].ToString();
            dtTable.Clear();
            dtTable.Dispose();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            if (objAssetVerification_Client != null)
                objAssetVerification_Client.Close();
            
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            objS3G_LOANAD_AssetDataTable = new AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsDataTable();
            objS3G_LOANAD_AssetDataRow = objS3G_LOANAD_AssetDataTable.NewS3G_LOANAD_AssetVerficiationDetailsRow();

            try
            {

                objS3G_LOANAD_AssetDataRow.Asset_Verification_No = intAssetVerificationNo;
                objS3G_LOANAD_AssetDataRow.Company_ID = intCompanyID;
                objS3G_LOANAD_AssetDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                objS3G_LOANAD_AssetDataRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
                objS3G_LOANAD_AssetDataRow.PANum = ddlMLA.SelectedValue.ToString();
                if (ddlSLA.SelectedValue.ToString() == "0")
                {
                    objS3G_LOANAD_AssetDataRow.SANum = ddlMLA.SelectedValue.ToString() + "DUMMY";
                }
                else
                {
                    objS3G_LOANAD_AssetDataRow.SANum = ddlSLA.SelectedValue.ToString();
                }
                objS3G_LOANAD_AssetDataRow.Customer_ID = Convert.ToInt32(hdnCustID.Value);
                objS3G_LOANAD_AssetDataRow.Asset_Verification_Date = Utility.StringToDate(txtAVDate.Text);
                objS3G_LOANAD_AssetDataRow.Asse_ID = Convert.ToInt32(ddlAssetCode.SelectedValue);
                objS3G_LOANAD_AssetDataRow.Inspection_By_Type_Code = 5;
                objS3G_LOANAD_AssetDataRow.Insepection_By_Code = Convert.ToInt32(ddlInspBy.SelectedValue);
                objS3G_LOANAD_AssetDataRow.Inspection_Code = Convert.ToInt32(ddlInspCode.SelectedValue);
                //string str = DateTime.Parse().ToString()+ " " + txtInspTime.Text.Trim().ToString();

                objS3G_LOANAD_AssetDataRow.Inspection_Date = Utility.StringToDate(txtInspDate.Text.Trim());
                //objS3G_LOANAD_AssetDataRow.Inspection_Time = txtInspTime.Text;
                objS3G_LOANAD_AssetDataRow.Inspection_Time = txtHour.Text + ":" + txtMins.Text + " " + ddlAMPM.SelectedItem.Text;
                objS3G_LOANAD_AssetDataRow.Location = txtLocation.Text.Trim();
                objS3G_LOANAD_AssetDataRow.Remarks = txtRemarks.Text.Trim();
                objS3G_LOANAD_AssetDataRow.Physically_Verified = Convert.ToBoolean(chkVerified.Checked ? 1 : 0);
                objS3G_LOANAD_AssetDataRow.Asset_Status_Type_Code = 1;
                objS3G_LOANAD_AssetDataRow.Asset_Status_code = Convert.ToInt32(ddlAssetStatus.SelectedValue);
                objS3G_LOANAD_AssetDataRow.Created_By = intUserID;
                objS3G_LOANAD_AssetDataRow.Created_On = DateTime.Now;
                string strErrMsg = string.Empty;
                if (intAssetVerificationNo != string.Empty)
                {

                    if (txtVerification.Text.Trim().Length > 0)
                        objS3G_LOANAD_AssetDataRow.Verification_Charges = Convert.ToInt64(txtVerification.Text);
                    if (txtMemo.Text.Trim().Length > 0)
                        objS3G_LOANAD_AssetDataRow.Memo_Charges = Convert.ToInt64(txtMemo.Text);
                    objS3G_LOANAD_AssetDataRow.Modified_By = intUserID;
                    objS3G_LOANAD_AssetDataRow.Modified_On = DateTime.Now;
                }

                objS3G_LOANAD_AssetDataTable.AddS3G_LOANAD_AssetVerficiationDetailsRow(objS3G_LOANAD_AssetDataRow);
                objAssetVerification_Client = new AssetMgtServicesReference.AssetMgtServicesClient();

                if (intAssetVerificationNo == string.Empty)
                {
                    intErrCode = objAssetVerification_Client.FunPubCreateAssetVerification(out strErrMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_AssetDataTable, ObjSerMode));
                }
                else
                {
                    intErrCode = objAssetVerification_Client.FunPubModifyAssetVerification(out strErrMsg, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_AssetDataTable, ObjSerMode));
                    // FunPriSysJournalEntry();
                }

                if (intErrCode == 0)
                {
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strErrMsg, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not Assigned";
                        }
                        ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode,strAlert);
                        return;
                    }
                    else if (intAssetVerificationNo == string.Empty)
                    {
                        // FORCE PULL IMPLEMENTATION KR
                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(ProgramCode, ddlMLA.SelectedItem.Text, null, "L", CompanyId, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), ddlMLA.SelectedItem.Text, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), ddlMLA.SelectedItem.Text, "", int.Parse(dtrForce["PRODUCTID"].ToString()));
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }

                        strAlert = "Asset Verification Number " + strErrMsg + " added successfully";
                        strAlert += @"\n\nWould you  like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    }
                    else
                    {
                        //strAlert = strAlert.Replace("__ALERT__", "Asset Verification details updated successfully");
                        //strRedirectPageView = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + Resources.LocalizationResources.AssetverificationUpdate + "');" + strRedirectPageView, true);
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    }
                }
                else if (intErrCode == -1)
                {
                    Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                    return;
                }
                else if (intErrCode == -2)
                {
                    Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                    return;
                }
                else if (intErrCode == 10)
                {
                    Utility.FunShowAlertMsg(this.Page, "Memo master is not defined for Verfication charges.");
                    return;
                }
                else if (intErrCode == 11)
                {
                    Utility.FunShowAlertMsg(this.Page, "Document number is not defined for Memorandum Booking.");
                    return;
                }
                else if (intErrCode == 12)
                {
                    Utility.FunShowAlertMsg(this.Page, "Document number is exceeded for Memorandum Booking.");
                    return;
                }
                //else if (intErrCode == -3)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Document Number is not defined for Followup");
                //    return;
                //}
                //else if (intErrCode == -4)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Document Number is exceeded for Followup");
                //    return;
                //}
                else
                {
                    if (PageMode == PageModes.Create)
                    {
                        Utility.FunShowValidationMsg(this.Page, "AVHB", intErrCode, "Error in saving Asset verification", false);
                    }
                    else
                    {
                        Utility.FunShowValidationMsg(this.Page, "AVHB", intErrCode, strErrMsg, false);
                    }
                    return;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                lblErrorMessage.Text = string.Empty;
            }
            //                Exception(FaultException<AssetMgtServicesReference.ClsPubFaultException> 
            catch (Exception ex)
            {
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                lblErrorMessage.Text = ex.Message.ToString();

            }
            finally
            {
                if (objAssetVerification_Client != null)
                    objAssetVerification_Client.Close();
            }
        }
    }
    private void FunCleardropdown(int x)
    {
        if (x == 0)
        {
            ddlBranch.ClearDropDownList();
            ddlLOB.ClearDropDownList();
            ddlAssetStatus.ClearDropDownList();
            ddlInspBy.ClearDropDownList();
        }
        else
        {
            ddlBranch.SelectedIndex = 0;
            ddlLOB.SelectedIndex = 0;
            ddlAssetStatus.SelectedIndex = 0;
            ddlInspBy.SelectedIndex = 0;
            //ddlAssetCode.SelectedIndex = 0;
            //ddlSLA.SelectedIndex = 0;
            ddlMLA.SelectedIndex = 0;
            ddlInspCode.SelectedIndex = 0;
        }
        if (ddlAssetCode.Items.Count > 0)
            ddlAssetCode.ClearDropDownList();
        if (ddlSLA.Items.Count > 0)
            ddlAssetCode.ClearDropDownList();
        if (ddlMLA.Items.Count > 0)
            ddlMLA.ClearDropDownList();
        if (ddlInspCode.Items.Count > 0)
            ddlInspCode.ClearDropDownList();
    }

    private void FunClear(int val)
    {
        try
        {
            hdnCustID.Value = string.Empty;
            if (val != 0)
            {
                chkVerified.Checked = false;
                ucCustDetails.ClearCustomerDetails();
                txtAVNo.Text = string.Empty;
                txtInspDate.Text = string.Empty;
                txtInspTime.Text = string.Empty;
                txtLocation.Text = string.Empty;
                txtMemo.Text = string.Empty;
                txtRemarks.Text = string.Empty; 
                txtVerification.Text = string.Empty;
                FunCleardropdown(1);
            }
            else
                FunCleardropdown(0);
                     
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
            Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    txtVerification.Enabled = txtMemo.Enabled = false;
                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    ddlAssetStatus.Enabled = ddlAssetCode.Enabled = ddlMLA.Enabled = ddlLOB.Enabled = ddlBranch.Enabled =
                    ddlSLA.Enabled = ddlCustCode.Enabled = btnClear.Enabled = false;
                    txtInspTime.ReadOnly = true;
                    txtMemo.ReadOnly = false;
                    if (ddlInspBy.SelectedValue == "10")
                        txtVerification.ReadOnly = true;
                    btnGetLOV.Visible = false;
                    break;
                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = false;
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }
                    if (bClearList)
                    {
                        FunClear(0);
                    }
                    btnClear.Enabled = false;
                    txtInspDate.ReadOnly = txtInspTime.ReadOnly = txtVerification.ReadOnly = txtMemo.ReadOnly = txtLocation.ReadOnly = txtRemarks.ReadOnly = true;
                    CalendarExtender1.Enabled = chkVerified.Enabled = false;
                    imgDate.Visible = false;
                    btnGetLOV.Visible = false;
                    txtHour.ReadOnly = txtMins.ReadOnly = true;
                    ddlAMPM.ClearDropDownList();
                    break;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }       
    //#region "System Journal Entry"

    //private void FunPriSysJournalEntry()
    //{
    //    string strXMLAccDetails = string.Empty;
    //    strXMLAccDetails = "<Root> ";

    //    ObjSysJournal.Accounting_Flag = '0';                // Debit or Credit
    //    ObjSysJournal.Txn_Amount = txtVerification.Text;          // Debit or Credit Amount
    //    ObjSysJournal.Global_Dimension2_Code = "1";         //Asset or Processing Fees
    //    ObjSysJournal.Global_Dimension2_No = ddlAssetCode.SelectedValue;  //LAR Number or Asset ID or Processing ID
    //    strXMLAccDetails += Utility.FunPubSysJournalXMLGenerate(ObjSysJournal);

    //    strXMLAccDetails += "</Root>";
    //    ObjSysJournal.Company_ID = intCompanyID;
    //    ObjSysJournal.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
    //    ObjSysJournal.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
    //    ObjSysJournal.Customer_ID = Convert.ToInt32(txtCustCode.Attributes["Cust_ID"]);
    //    ObjSysJournal.Account_Link_Key = intAssetVerificationNo;  //Page generated document number control

    //    ObjSysJournal.Narration = "";                   // Now it is optional
    //    ObjSysJournal.Value_Date = DateTime.Now;        //Posting Date
    //    ObjSysJournal.Txn_Currency_Code = string.Empty;           //Assign if there is any txn currency.

    //    ObjSysJournal.Program_ID = 52;                  //Page ProgramID
    //    ObjSysJournal.Global_Dimension1_Code = 7;       //Vendor or Dealer...

    //    ObjSysJournal.Global_Dimension1_No = ddlInspCode.SelectedValue;   // Vendor or Dealer Code
    //    ObjSysJournal.JV_Status_Code = 1;               //Active or Cancelled or Reverse Journal
    //    ObjSysJournal.Reference_Number = ddlMLA.SelectedValue; // MLA Number or PANum 
    //    ObjSysJournal.Created_By = intUserID;           // Record created by

    //    ObjSysJournal.XMLSysJournal = strXMLAccDetails;

    //    Utility.FunPubSysJournalEntry(ObjSysJournal);
    //}

    //#endregion

    #endregion

}
