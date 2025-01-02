
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.UI.HtmlControls;
using Resources;

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
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLOANADAssetVerification_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ASV';";
    public string strCustid;
    //User Authorization
    public string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    static string strPageName = "Asset Verification";
    string strUser_Type;
    string emailCustomerName;
    public static LoanAdmin_S3GLOANADAssetVeification_Add obj_Page;
    #endregion

    #region Page Loading
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
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
                //ddlMLA.SelectedIndex = 0;
                //if (ddlMLA.Items.Count > 0)
                //    ddlMLA.ClearDropDownList();
                PopulatePANum();
                ddlSLA.Items.Clear();
                //ddlLOB.SelectedIndex = 0;
                //ddlBranch.SelectedIndex = 0;
                //ddlBranch.Items.Clear();
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
            tcASV.ActiveTabIndex = 0;
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
            //PopulateBranchList();
            ddlBranch.Clear();
          //  PopulatePANum();
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

    private void FunPriGetEntityEmailID()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();//Entity - Email ID
            Procparam.Add("@InspectedType", ddlInspBy.SelectedValue);
            Procparam.Add("@InspectedCode", ddlInspCode.SelectedValue);

            DataTable dt = Utility.GetDefaultData("S3G_CLN_Get_Entity_EmailID", Procparam);
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewState["PopupEmailID"] = dt.Rows[0][0].ToString();
            }
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
            intLength = (txtInstructions.Text.Length + txtRemarks.Text.Length + txtAVDate.Text.Length);
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
            else if (Utility.StringToDate(txtInspDateTo.Text).ToString("dd-MMM-yyyy") == DateTime.Today.ToString("dd-MMM-yyyy") &&
             (Convert.ToDateTime(DateTime.Now.ToString("hh:mm tt").ToString()) <
                    Convert.ToDateTime(Utility.StringToDate(txtInspDateTo.Text).ToString("dd-MMM-yyyy") + " " + txtHourTo.Text + ":" + txtMinsTo.Text + " " + ddlAMPMTo.SelectedValue.ToString())))
            {
                args.IsValid = false;
                custCheck.ErrorMessage = "Inspection time should be lesser than the System time [" + DateTime.Now.ToString("hh:mm tt") + "].";
                txtHourTo.Focus();
            }
            else if (Convert.ToDateTime(Utility.StringToDate(txtInspDateTo.Text).ToString("dd-MMM-yyyy")) <= Convert.ToDateTime(Utility.StringToDate(txtInspDate.Text).ToString("dd-MMM-yyyy")))
            {
                args.IsValid = false;
                custCheck.ErrorMessage = "Inspection To Date should not be lesser than the Inspection From Date";
                txtInspDateTo.Focus();
            }
            //else if (Convert.ToDateTime(Utility.StringToDate(txtInspTime.Text).ToString("hh:mm tt")) < Convert.ToDateTime(Utility.StringToDate(txtInspTimeTo.Text).ToString("hh:mm tt")))
            //{
            //    args.IsValid = false;
            //    custCheck.ErrorMessage = "time issue";
            //    txtInspDateTo.Focus();
            //}

            // Commanded By Thangam M on 14/Nov/2011 based on the UAT II (Bug ID - AVER_018)

            //else if (ViewState["CreationDate"] != null &&
            //    Convert.ToDateTime(Utility.StringToDate(ViewState["CreationDate"].ToString()).ToString("dd-MMM-yyyy")) > Convert.ToDateTime(Utility.StringToDate(txtInspDate.Text).ToString("dd-MMM-yyyy")))
            //{
            //    args.IsValid = false;
            //    custCheck.ErrorMessage = "Inspection date should be greater than the Account creation date [" + Utility.StringToDate(ViewState["CreationDate"].ToString()).ToString(strDateFormat) + "].";
            //    txtHour.Focus();
            //}

            //Commanded End

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
            if (PageMode == PageModes.Create)
            {
                Procparam.Add("@Is_Activated", "2");
            }
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@Program_ID", ProgramCode);
            //if (!hdnCustID.Value.Equals(string.Empty))
            if (ViewState["FromCustomer"] != null)
                Procparam.Add("@Customer_ID", hdnCustID.Value);
            if (!ddlLOB.SelectedValue.Equals("0") && !ddlBranch.SelectedValue.Equals("0"))
                ddlMLA.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "PANum", "PANum" });//S3G_LOANAD_GetPANum
            //Code Modifed By Ganapathy on 3rd July begins
            if (ddlSLA.Items.Count > 0)
            {
                ddlSLA.ClearDropDownList();
                lblSLA.CssClass = "styleDisplayLabel";
            }
            //Code Modifed By Ganapathy on 3rd July end

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
                if (ddlSLA.Items.Count == 1)
                {
                    PopulateAssetCodebyLOB();
                }
                else
                {
                    lblSLA.CssClass = "styleReqFieldLabel";
                    rfvddlSLA.Enabled = true;
                }
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
            else
            {
                lblSLA.CssClass = "styleDisplayLabel";
                rfvddlSLA.Enabled = true;
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
            Procparam.Add("@Is_Activated", "2");
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Customer_ID", hdnCustID.Value);
            //Procparam.Add("@User_ID", intUserID.ToString()); 
            Procparam.Add("@PANum", strPAN);
            Procparam.Add("@Program_ID", ProgramCode);
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
        S3GSession ObjS3GSession = new S3GSession();
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPubSetIndex(1);

            //Date Format
            //ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender1.Format = strDateFormat;
            CalendarExtender1To.Format = strDateFormat;
            CalendarExtender2.Format = strDateFormat;
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
                ViewState["AssetVerificationNo"] = intAssetVerificationNo;
            }

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

                }
            }
            #endregion

            CheckForUTPA();

            if (!IsPostBack)
            {
               
                if (Request.QueryString["qsMode"] != null)
                {
                    strmode = Request.QueryString["qsMode"];
                    ViewState["strmode"] = strmode;
                }

                FunLoadErrorMessage();
                if (Request.QueryString["qsMode"] == "C")
                {
                    PopulateLOBList();
                    PopulateBranchList();
                    ddlBranch.SelectedValue = "0";
                }
                
                ViewState["FromCustomer"] = null;
                // PopulateCustomer();
                PopulateLookupDescriptions();
                txtVerification.CheckGPSLength(false);
                txtMemo.CheckGPSLength(false);
                txtAVDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                string strCanEdit = "";

                if (strmode.Equals("Q"))
                {
                    strCanEdit = FunVerifedAssetForModification(intAssetVerificationNo);
                    FunPriDisableControls(-1);
                }
                else if (strmode.Equals("M"))
                {
                    //Changed By Thangam M on 11/Jan/2012 to fix UAT bug AVER_028
                    strCanEdit = FunVerifedAssetForModification(intAssetVerificationNo);
                    if (strCanEdit == "0")
                    {
                        Utility.FunShowAlertMsg(this, "Cannot modify the record. Payment has been made.");
                        FunPriDisableControls(-1);
                    }
                    else if (strCanEdit == "5")
                    {
                        Utility.FunShowAlertMsg(this, "Cannot modify the locked month record.");
                        FunPriDisableControls(-1);
                    }
                    else
                    {
                        FunPriDisableControls(1);
                    }
                }
                else if (strmode.Equals("C"))
                {
                    FunPriDisableControls(0);
                }


                if (Request.QueryString.Get("qsViewId") != null)
                {
                    if (Request.QueryString.Get("qsMode") != null)
                    {
                        if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)
                        {
                            FunPriControlStatus(0);
                            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                        }
                        else if (string.Compare("M", Request.QueryString.Get("qsMode")) == 0)
                        {
                            FunPriControlStatus(1);
                            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                        }
                    }
                }
                else
                {
                    FunPriControlStatus(0);
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                }

                FunPriSetControlStatus();
            }

            if ((IsPostBack) && (hdnFileName.Value != ""))
            {
                lblDisplayFile.Text = hdnFileName.Value;
            }

            //if ((ddlInspBy.SelectedValue == "9") && (ddlInspCode.SelectedValue == ObjUserInfo.ProUserIdRW.ToString()))
            //{
            //    FunPriControlStatusForUTPA();
            //}
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);

        }
    }


    private void FunPriControlStatus(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode
                    if (CheckForUTPA())
                    {
                        //TPASV1.Enabled = false;
                        TPASV2.Enabled = true;
                    }
                    else
                    {
                        TPASV1.Enabled = true;
                        TPASV2.Enabled = false;
                    }
                    break;

                case 1: //Modify

                    if (!CheckForUTPA())
                    {
                        //Condition added to enable Respond tab on Logged_in user and Inspection_By are same.
                        if (ddlInspBy.SelectedValue == "9" && ddlInspCode.SelectedValue == ObjUserInfo.ProUserIdRW.ToString())
                        {
                            TPASV2.Enabled = true;
                        }
                        else
                        {
                            TPASV2.Enabled = false;
                        }
                    }

                    if (!CheckForUTPA())
                    {
                        btnCancel.Enabled = true;
                    }
                    else
                    {
                        //txtResspondedDate.Text = DateTime.Now.ToString(strDateFormat);
                    }
                    break;

                case -1://Query

                    if (!bQuery)
                    {
                        Response.Redirect("~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=ASV");
                    }

                    //txtTerms.ReadOnly = txtFieldRequest.ReadOnly = true;
                    //txtSendEmail.ReadOnly = true;

                    btnClear.Enabled = btnSave.Enabled = false;
                    break;
            }

        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    public bool CheckForUTPA()
    {
        try
        {
            if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                strUser_Type = "UTPA"; //UTPA User
                return true;
            }
            else
            {
                strUser_Type = ObjUserInfo.ProUserNameRW; //Normal User
                return false;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }


    public void FunDisableRespondControls(bool CanEnable)
    {
        try
        {
            //txtRespondedBy.ReadOnly = txtValue.ReadOnly =
            //txtResponseDesgn.ReadOnly = txtEmailRes.ReadOnly = txtMobile.ReadOnly =
            //txtFieldRespond.ReadOnly = !CanEnable; //imgRespondedDate.Visible = !CanEnable;
            //txtResspondedDate_CalendarExtender.Enabled = CanEnable;

            //ddlClientCreditability.Enabled =
            //ddlClientNetWorth.Enabled =
            //chkCancelled.Enabled = CanEnable;
        }
        catch (Exception ex)
        {
            throw;
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

            PopulatePANCustomer(HeaderValues["Customer_Id"].ToString(), true);

            PopulateLOBList();
            PopulateBranchList();
            //ddlAssetStatus.SelectedValue = Convert.ToString(dtTable.Rows[0]["Asset_Status_code"]);

            hdnCustID.Value = HeaderValues["Customer_Id"].ToString();
            ddlLOB.SelectedValue = HeaderValues["Lob_Id"].ToString();
            ddlBranch.SelectedValue = HeaderValues["Location_ID"].ToString();

            PopulatePANum();
            ddlMLA.SelectedValue = HeaderValues["UniqueId"].ToString();

            PopulateSANum(HeaderValues["UniqueId"].ToString());
            PopulateAssetCodebyLOB();
            //PopulatePANCustomer(HeaderValues["Customer_Id"].ToString());
            if (ddlLOB.Items.Count > 0) ddlLOB.ClearDropDownList();
           // if (ddlBranch.Items.Count > 0) ddlBranch.ClearDropDownList();
            if (ddlMLA.Items.Count > 0) ddlMLA.ClearDropDownList();
        }
    }
    #endregion

    private void FunLoadErrorMessage()
    {
        try
        {
            rfvddlLOB.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LOB;
            rfvddlCust.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_CustomerCode;
         //   rfvddlBranch.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Branch;
            rfvddlMLA.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Select_PriAc;
            rfvAssetCode.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_AssetCode;
            rfvddlAssetStatus.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_AssetStatus;
            rfvddlInspBy.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_InspectionBy;
            rfvInspCode.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_InspectionCode;
            rfvInspecTime.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_InspectionTime;
            rfvInsTime.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Inspectiontimeformat;
            rfvDate.ErrorMessage = Resources.ValidationMsgs.S3g_ValMsg_Date;
            rfvLocation.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_Location;

            rfvddlInspBy.ErrorMessage = "Select Requested To";
            rfvInspCode.ErrorMessage = "Select Name";
            rfvDate.ErrorMessage = "Select Inspection From Date";
            rfvDateTo.ErrorMessage = "Select Inspection To Date";
            rfvLocation.ErrorMessage = "Enter Instructions";

            rfvDate1.ErrorMessage = "Select Inspection Date - Respond Information Tab";
            //rfvInsTime1.ErrorMessage = "Enter Inspection Time - Respond Information Tab";
            rfvLocation1.ErrorMessage = "Enter Location - Respond Information Tab";
            rfvddlAssetStatus.ErrorMessage = "Enter Asset Status - Respond Information Tab";
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
    private void PopulatePANCustomer(string strPAN, bool isWorkFlowCall)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(CompanyId));
            //Procparam.Add("@PANum", strPAN);
            if (!ddlMLA.SelectedValue.Equals("0") && (PageMode.Equals(PageModes.Create) || PageMode.Equals(PageModes.WorkFlow)))
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

            if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                //Procparam.Clear();
                //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                //Procparam.Add("@UTPA_ID", intUserID.ToString());
                //Procparam.Add("@Is_Active", "1");
                //ddlBranch.BindDataTable("S3G_Get_UTPA_Branch_List", Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            }
            else
            {
                //Procparam.Clear();

                //Procparam.Add("@Is_Active", "1");
                //Procparam.Add("@User_ID", intUserID.ToString());
                //Procparam.Add("@Company_ID", intCompanyID.ToString());
                //if (ddlLOB.SelectedIndex != 0)
                //{
                //    Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                //}
                //Procparam.Add("@Program_ID", ProgramCode);
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
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

            if (ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
            {
                Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@UTPA_ID", intUserID.ToString());
                Procparam.Add("@Is_Active", "1");
                ddlLOB.BindDataTable("S3G_Get_UTPA_LOB_LIST", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            }
            else
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@User_ID", Convert.ToString(intUserID));

                //if (intAssetVerificationNo.Equals(string.Empty))
                Procparam.Add("@Is_Active", "1");
                Procparam.Add("@Program_ID", ProgramCode);
                ddlLOB.BindDataTable(SPNames.S3G_LOANAD_LOBASSETIDEN, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            }


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

    //Changed By Thangam M on 11/Jan/2012 to fix UAT bug AVER_028
    private string FunVerifedAssetForModification(string strAssetNo)
    {
        objAssetVerification_Client = new AssetMgtServicesReference.AssetMgtServicesClient();
        try
        {
            string strCanEdit = "1";

            txtVerification.ReadOnly = txtMemo.ReadOnly = false;
            DataTable dtTable = new DataTable();


            byte[] bytesEnquiryDetails = objAssetVerification_Client.FunVerifedAssetForModification(strAssetNo);
            dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesEnquiryDetails, SerializationMode.Binary, typeof(DataTable));
            ddlAssetStatus.SelectedValue = Convert.ToString(dtTable.Rows[0]["Asset_Status_code"]);
            
            //Performance Issue Avoid to Load full DropdownList value  Changed By Shibu  
            ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dtTable.Rows[0]["LOB_Name"].ToString(), dtTable.Rows[0]["LOB_ID"].ToString()));
            ddlLOB.ToolTip = dtTable.Rows[0]["LOB_Name"].ToString();

            ddlBranch.SelectedValue = dtTable.Rows[0]["Location_ID"].ToString();
            ddlBranch.SelectedText = dtTable.Rows[0]["Location_Name"].ToString();
            ddlBranch.ToolTip = dtTable.Rows[0]["Location_Name"].ToString();
            
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

            //PopulatePANum();
            ddlMLA.Items.Clear();
            ddlMLA.Items.Add(new System.Web.UI.WebControls.ListItem(dtTable.Rows[0]["PANum"].ToString(),dtTable.Rows[0]["PANum"].ToString()));
            ddlMLA.ToolTip = dtTable.Rows[0]["PANum"].ToString();
           
            //PopulateSANum(dtTable.Rows[0]["PANum"].ToString());
            ddlSLA.Items.Clear();
            if (!dtTable.Rows[0]["SANum"].ToString().Contains("DUMMY"))
            {
                ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem(dtTable.Rows[0]["SANum"].ToString(), dtTable.Rows[0]["SANum"].ToString()));
                ddlSLA.ToolTip = dtTable.Rows[0]["SANum"].ToString();
            }
            else
            {
                ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem("--Select--", "0"));             
            }
          //  PopulateAssetCodebyLOB();

            if (dtTable.Rows[0]["Created_Date"].ToString() != "")
            {
                ViewState["CreationDate"] = dtTable.Rows[0]["Created_Date"].ToString();
            }
           
            if (Utility.FunGetValueBySplit(dtTable.Rows[0]["LOB_Name"].ToString()) != "OL")
                PopulateAssetCode();
            else
                PopulateAssetCodeForOL();

            ddlAssetCode.SelectedValue = dtTable.Rows[0]["Asset_ID"].ToString();

            txtAVNo.Text = dtTable.Rows[0]["Asset_Verification_No"].ToString();
        
            if (dtTable.Rows[0]["Inspection_Date"].ToString() != "")
            {
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
            }
            if (dtTable.Rows[0]["Inspection_Date_To"].ToString() != "")
            {
                txtInspDateTo.Text = DateTime.Parse(dtTable.Rows[0]["Inspection_Date_To"].ToString().Split(' ')[0], CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtInspTimeTo.Text = (dtTable.Rows[0]["Inspection_Date_To"].ToString().Split(' ')[1]).Split(':')[0] + ":" + (dtTable.Rows[0]["Inspection_Date_To"].ToString().Split(' ')[1]).Split(':')[1] + " " + dtTable.Rows[0]["Inspection_Date_To"].ToString().Split(' ')[2];
                string[] strTo = Convert.ToDateTime(dtTable.Rows[0]["Inspection_Date_To"].ToString()).ToString("HH:mm:ss").Split(':');
                 if (Convert.ToInt32(strTo[0]) >= 12)                     // To check whether AM or PM
                 {
                     if (Convert.ToInt32(strTo[0]) > 12)                  // To keep it as 12
                     {
                         txtHourTo.Text = (Convert.ToInt32(strTo[0]) - 12).ToString();
                     }

                     ddlAMPMTo.SelectedValue = "PM";

                     if (txtHourTo.Text.Length == 1)
                     {
                         txtHourTo.Text = "0" + txtHourTo.Text;
                     }
                 }
                 else
                 {
                     if (strTo[0] == "00")
                     {
                         txtHourTo.Text = "12";
                     }
                     else
                     {
                         txtHourTo.Text = strTo[0];
                     }
                     ddlAMPMTo.SelectedValue = "AM";
                 }
                 txtMinsTo.Text = strTo[1];
            }
           
            txtAVDate.Text = DateTime.Parse(dtTable.Rows[0]["Asset_Verification_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            txtInstructions.Text = dtTable.Rows[0]["Location"].ToString();
            //PopulatePANCustomer(dtTable.Rows[0]["Customer_ID"].ToString(), false);
            ucCustDetails.SetCustomerDetails(dtTable.Rows[0], true);
            hdnCustID.Value = dtTable.Rows[0]["Customer_ID"].ToString();
            //if(isWorkFlowCall)
            ucCustomerCodeLov.FunPubSetControlValue(dtTable.Rows[0]["Customer_ID"].ToString(), dtTable.Rows[0]["Customer_Code"].ToString());

            if (Convert.ToString(dtTable.Rows[0]["Verification_Charges"]) != string.Empty || dtTable.Rows[0]["Verification_Charges"] != null)
                txtVerification.Text = dtTable.Rows[0]["Verification_Charges"].ToString();
            if (Convert.ToString(dtTable.Rows[0]["Memo_Charges"]) != string.Empty || dtTable.Rows[0]["Memo_Charges"] != null)
                txtMemo.Text = dtTable.Rows[0]["Memo_Charges"].ToString();

            //Changed By Thangam M on 11/Jan/2012 to fix UAT bug AVER_028
            strCanEdit = dtTable.Rows[0]["Can_Edit"].ToString();

            if (Convert.ToString(dtTable.Rows[0]["Inspection_Date_Resp"]) != string.Empty)
            {
                txtInspDate1.Text = DateTime.Parse(dtTable.Rows[0]["Inspection_Date_Resp"].ToString().Split(' ')[0], CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtInspTime1.Text = (dtTable.Rows[0]["Inspection_Date_Resp"].ToString().Split(' ')[1]).Split(':')[0] + ":" + (dtTable.Rows[0]["Inspection_Date_Resp"].ToString().Split(' ')[1]).Split(':')[1] + " " + dtTable.Rows[0]["Inspection_Date_Resp"].ToString().Split(' ')[2];
                string[] str1 = Convert.ToDateTime(dtTable.Rows[0]["Inspection_Date_Resp"].ToString()).ToString("HH:mm:ss").Split(':');

                if (Convert.ToInt32(str1[0]) >= 12)                     // To check whether AM or PM
                {
                    if (Convert.ToInt32(str1[0]) > 12)                  // To keep it as 12
                    {
                        txtHour1.Text = (Convert.ToInt32(str1[0]) - 12).ToString();
                    }

                    ddlAMPM1.SelectedValue = "PM";

                    if (txtHour1.Text.Length == 1)
                    {
                        txtHour1.Text = "0" + txtHour1.Text;
                    }
                }
                else
                {
                    if (str1[0] == "00")
                    {
                        txtHour1.Text = "12";
                    }
                    else
                    {
                        txtHour1.Text = str1[0];
                    }
                    ddlAMPM1.SelectedValue = "AM";
                }
                txtMins1.Text = str1[1];
            }

            if (Convert.ToString(dtTable.Rows[0]["Physically_Verified"]) != string.Empty)
            {
                chkVerified.Checked = Convert.ToBoolean(dtTable.Rows[0]["Physically_Verified"]);
            }

            if (Convert.ToString(dtTable.Rows[0]["Remarks"]) != string.Empty)
            {
                txtRemarks1.Text = dtTable.Rows[0]["Remarks"].ToString();
            }

            if (Convert.ToString(dtTable.Rows[0]["Location_Resp"]) != string.Empty)
            {
                txtLocation1.Text = dtTable.Rows[0]["Location_Resp"].ToString();
            }

            //fileScanImage.Text = dtTable.Rows[0]["Location_Resp"].ToString();
            if (dtTable.Rows[0]["Verification_Image"].ToString().Trim() != "")
            {
                lnkDownload.Enabled = true;
                hdnFile.Value = dtTable.Rows[0]["Verification_Image"].ToString();
                lblDisplayFile.Text = hdnFile.Value;
                //fileScanImage.Enabled = true;
                hdnFileUploaded.Value = "1";
            }
            else
            {
                //fileScanImage.Enabled = false;
                hdnFileUploaded.Value = "0";
            }


            dtTable.Clear();
            dtTable.Dispose();

            return strCanEdit;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {

            objAssetVerification_Client.Close();

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        FunPriSetControlStatus();

        AjaxControlToolkit.AsyncFileUpload fileScanImage = (AjaxControlToolkit.AsyncFileUpload)TPASV2.FindControl("fileScanImage");

        if (!FunFileLoad(false))
        {
            return;
        }

        objAssetVerification_Client = new AssetMgtServicesReference.AssetMgtServicesClient();
        if (Page.IsValid)
        {
            objS3G_LOANAD_AssetDataTable = new AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsDataTable();
            objS3G_LOANAD_AssetDataRow = objS3G_LOANAD_AssetDataTable.NewS3G_LOANAD_AssetVerficiationDetailsRow();

            if (ViewState["AssetVerificationNo"] != null)
            {
                intAssetVerificationNo = ViewState["AssetVerificationNo"].ToString();
            }

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
                objS3G_LOANAD_AssetDataRow.Inspection_Date = Utility.StringToDate(txtInspDate.Text.Trim());
                objS3G_LOANAD_AssetDataRow.Inspection_Time = txtHour.Text + ":" + txtMins.Text + " " + ddlAMPM.SelectedItem.Text;

                objS3G_LOANAD_AssetDataRow.Inspection_Date_To = Utility.StringToDate(txtInspDateTo.Text.Trim());
                objS3G_LOANAD_AssetDataRow.Inspection_Time_To = txtHourTo.Text + ":" + txtMinsTo.Text + " " + ddlAMPMTo.SelectedItem.Text;

                objS3G_LOANAD_AssetDataRow.Location = txtInstructions.Text.Trim();
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

                objS3G_LOANAD_AssetDataRow.User_Type = strUser_Type;

                //Add parameters for UTPA user @UserType = 'UTPA'
                if (CheckForUTPA())
                {
                    objS3G_LOANAD_AssetDataRow.Inspection_Date_Resp = Utility.StringToDate(txtInspDate1.Text.Trim());
                    objS3G_LOANAD_AssetDataRow.InspectionTime_Resp = txtHour1.Text + ":" + txtMins1.Text + " " + ddlAMPM1.SelectedItem.Text;
                    objS3G_LOANAD_AssetDataRow.Physically_Verified = Convert.ToBoolean(chkVerified.Checked ? 1 : 0);
                    objS3G_LOANAD_AssetDataRow.Asset_Status_Type_Code = 1;
                    objS3G_LOANAD_AssetDataRow.Asset_Status_code = Convert.ToInt32(ddlAssetStatus.SelectedValue);
                    objS3G_LOANAD_AssetDataRow.Location_Resp = txtLocation1.Text.Trim();
                    objS3G_LOANAD_AssetDataRow.Remarks = txtRemarks1.Text.Trim();

                    if (fileScanImage.FileName.Length > 0)
                    {
                        objS3G_LOANAD_AssetDataRow.Verification_Image = fileScanImage.FileName;
                    }
                    else
                    {
                        objS3G_LOANAD_AssetDataRow.Verification_Image = "";
                    }

                }
                else
                {
                    if (txtInspDate1.Text != string.Empty)
                    {
                        objS3G_LOANAD_AssetDataRow.Inspection_Date_Resp = Utility.StringToDate(txtInspDate1.Text.Trim());
                    }
                    else
                    {
                        //Value hardcoded - due to "Datetime" Datatype in Bus Entity Dataset - Have to insert null for this condition
                        objS3G_LOANAD_AssetDataRow.Inspection_Date_Resp = Convert.ToDateTime("01/01/1900");
                    }
                    objS3G_LOANAD_AssetDataRow.InspectionTime_Resp = txtHour1.Text + ":" + txtMins1.Text + " " + ddlAMPM1.SelectedItem.Text;
                    objS3G_LOANAD_AssetDataRow.Physically_Verified = Convert.ToBoolean(chkVerified.Checked ? 1 : 0);
                    objS3G_LOANAD_AssetDataRow.Asset_Status_Type_Code = 1;
                    objS3G_LOANAD_AssetDataRow.Asset_Status_code = Convert.ToInt32(ddlAssetStatus.SelectedValue);
                    objS3G_LOANAD_AssetDataRow.Location_Resp = txtLocation1.Text.Trim();
                    objS3G_LOANAD_AssetDataRow.Remarks = txtRemarks1.Text.Trim();

                    if (fileScanImage.FileName.Length > 0)
                    {
                        objS3G_LOANAD_AssetDataRow.Verification_Image = fileScanImage.FileName;
                    }
                    else
                    {
                        objS3G_LOANAD_AssetDataRow.Verification_Image = "";
                    }
                }

                objS3G_LOANAD_AssetDataTable.AddS3G_LOANAD_AssetVerficiationDetailsRow(objS3G_LOANAD_AssetDataRow);



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
                    FunFileLoad(true);

                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strErrMsg, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        catch (Exception ex)
                        {
                            strErrMsg = strErrMsg + "  Work Flow Not assigned";
                            int intWorkFlowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString());
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        ShowWFAlertMessage(strErrMsg, ProgramCode, strAlert);
                        return;
                    }
                    else if (intAssetVerificationNo == string.Empty)
                    {
                        // FORCE PULL IMPLEMENTATION KR
                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(ProgramCode, ddlMLA.SelectedItem.Text, null, "L", CompanyId, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), ddlMLA.SelectedItem.Text, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), ddlMLA.SelectedItem.Text, "", int.Parse(dtrForce["PRODUCTID"].ToString()));
                                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                                btnSave.Enabled = false;
                                //END
                            }
                            catch (Exception ex)
                            {
                                strAlert = strAlert + "  Work Flow Not assigned";
                                int intWorkFlowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), "", int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString());
                            }
                        }

                        strAlert = "Asset Verification Number " + strErrMsg + " added successfully";
                        strAlert += @"\n\nWould you  like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                        strRedirectPageView = "";
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
                    Utility.FunShowAlertMsg(this.Page, "Memo master is not defined for Verification charges.");
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
                else if (intErrCode == 15)
                {
                    Utility.FunShowAlertMsg(this.Page, "Receipt has been created more than the given Memo Charges.");
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
                objAssetVerification_Client.Close();
            }
        }
    }
    private void FunCleardropdown(int x)
    {
        if (x == 0)
        {
          //  ddlBranch.Clear();
            //ddlBranch.Items.Clear();
           // ddlLOB.ClearDropDownList();
            ddlAssetStatus.ClearDropDownList();
            ddlInspBy.ClearDropDownList();
        }
        else
        {
            //ddlBranch.SelectedIndex = 0;
          ddlBranch.Clear();
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
        if (ddlSLA.Items.Count > 0)
            ddlSLA.ClearDropDownList();
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

                ucCustDetails.ClearCustomerDetails();
                txtAVNo.Text = string.Empty;
                txtInspDate.Text = string.Empty;
                txtInspTime.Text = string.Empty;
                txtInspDateTo.Text = string.Empty;
                txtInspTimeTo.Text = string.Empty;
                txtInstructions.Text = string.Empty;
                txtMemo.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtVerification.Text = string.Empty;
                FunCleardropdown(1);

                txtInspDate1.Text = string.Empty;
                txtInspTime1.Text = string.Empty;
                chkVerified.Checked = false;
                txtLocation1.Text = string.Empty;
                txtRemarks1.Text = string.Empty;
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
                    ddlMLA.Enabled = ddlLOB.Enabled = ddlBranch.Enabled =
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
                    btnMailPreview.Enabled = false;
                    txtInspDate.ReadOnly = txtInspTime.ReadOnly = txtVerification.ReadOnly = txtMemo.ReadOnly = txtInstructions.ReadOnly = txtRemarks.ReadOnly = true;
                    txtInspDateTo.ReadOnly = txtInspTimeTo.ReadOnly = true;
                    CalendarExtender1.Enabled = CalendarExtender1To.Enabled = chkVerified.Enabled = false;
                    imgDate.Visible = false;
                    btnGetLOV.Visible = false;
                    txtHour.ReadOnly = txtMins.ReadOnly = true;
                    txtHourTo.ReadOnly = txtMinsTo.ReadOnly = true;
                    ddlAMPM.ClearDropDownList();
                    ddlAMPMTo.ClearDropDownList();

                    txtInspDate1.ReadOnly = txtInspTime1.ReadOnly = txtLocation1.ReadOnly = txtRemarks1.ReadOnly = true;
                    CalendarExtender2.Enabled = false;
                    txtHour1.ReadOnly = txtMins1.ReadOnly = true;
                    ddlAMPM1.ClearDropDownList();
                    //fileScanImage.Enabled = false;
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

    private void FunPriSetControlStatus()
    {
        strmode = ViewState["strmode"].ToString();

        if (ObjUserInfo.ProUserTypeRW.ToString() != "UTPA" && (strmode == "M" || strmode == "C"))
        {
            if (ddlInspBy.SelectedValue == "9" && ddlInspCode.SelectedValue == ObjUserInfo.ProUserIdRW.ToString())
            {
                FunPriSetControlsForUTPA();
            }
            else
            {
                FunPriSetControlsForNormalUser();
            }
        }
        else if (ObjUserInfo.ProUserTypeRW.ToString() == "UTPA")
        {
            FunPriSetControlsForUTPA();
        }
    }

    private void FunPriSetControlsForNormalUser()
    {
        txtAVDate.ReadOnly = false;
        txtInspDate.ReadOnly = false;
        CalendarExtender1.Enabled = true;
        txtInspTime.ReadOnly = false;
        txtHour.ReadOnly = false;
        txtMins.ReadOnly = false;

        txtInspDateTo.ReadOnly = false;
        CalendarExtender1To.Enabled = true;
        txtInspTimeTo.ReadOnly = false;
        txtHourTo.ReadOnly = false;
        txtMinsTo.ReadOnly = false;

        txtVerification.ReadOnly = false;
        txtMemo.ReadOnly = false;
        txtInstructions.ReadOnly = false;
        btnMailPreview.Enabled = true;
        lnkDownload.Enabled = false;

        rfvddlLOB.Enabled = true;
        rfvddlCust.Enabled = true;
      //  rfvddlBranch.Enabled = true;
        rfvddlMLA.Enabled = true;
        rfvAssetCode.Enabled = true;
        rfvddlInspBy.Enabled = true;
        rfvInspCode.Enabled = true;
        rfvInsTime.Enabled = true;
        rfvDate.Enabled = true;
        rfvLocation.Enabled = true;
        rfvDateTo.Enabled = true;

        rfvDate1.Enabled = false;
        rfvInsTime1.Enabled = false;
        rfvddlAssetStatus.Enabled = false;
        rfvLocation1.Enabled = false;
    }

    private void FunPriSetControlsForUTPA()
    {
        ddlAssetCode.ClearDropDownList();
        txtAVDate.ReadOnly = true;
        ddlInspBy.ClearDropDownList();
        ddlInspCode.ClearDropDownList();
        txtInspDate.ReadOnly = true;
        CalendarExtender1.Enabled = false;
        txtInspTime.ReadOnly = true;
        txtHour.ReadOnly = true;
        txtMins.ReadOnly = true;
        ddlAMPM.ClearDropDownList();

        txtInspDateTo.ReadOnly = true;
        CalendarExtender1To.Enabled = false;
        txtInspTimeTo.ReadOnly = true;
        txtHourTo.ReadOnly = true;
        txtMinsTo.ReadOnly = true;
        ddlAMPMTo.ClearDropDownList();

        txtVerification.ReadOnly = true;
        txtMemo.ReadOnly = true;
        txtInstructions.ReadOnly = true;
        btnMailPreview.Enabled = false;

        rfvddlLOB.Enabled = false;
        rfvddlCust.Enabled = false;
        //rfvddlBranch.Enabled = false;
        rfvddlMLA.Enabled = false;
        rfvAssetCode.Enabled = false;
        rfvddlInspBy.Enabled = false;
        rfvInspCode.Enabled = false;
        rfvInsTime.Enabled = false;
        rfvDate.Enabled = false;
        rfvLocation.Enabled = false;
        rfvDateTo.Enabled = false;

        rfvDate1.Enabled = true;
        rfvInsTime1.Enabled = true;
        rfvddlAssetStatus.Enabled = true;
        rfvLocation1.Enabled = true;

        if (strmode == "M")
        {
            //fileScanImage.Enabled = true;
            if (lblDisplayFile.Text != "")
                lnkDownload.Enabled = true;
            else
                lnkDownload.Enabled = false;
        }
        else
        {
            //fileScanImage.Enabled = false;
            lnkDownload.Enabled = false;
        }
    }

    public bool FunFileLoad(bool bUpload)
    {
        AjaxControlToolkit.AsyncFileUpload fileScanImage = (AjaxControlToolkit.AsyncFileUpload)TPASV2.FindControl("fileScanImage");

        strMode = ViewState["strmode"].ToString();

        if (hdnFileName.Value == "")//code added temporarily for build - by kali - Oct-09-2012
        {
            return true;
        }
        if ((hdnFileUploaded.Value == "1") && (strMode == "M") && (hdnFileName.Value == ""))
        {
            return true;
        }
        if (hdnFileName.Value != "")
        {
            try
            {
                //System.Text.RegularExpressions.Regex strFileValidationExpression = new System.Text.RegularExpressions.Regex(@"\.txt$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                string strFilePath = "";
                string strFileName = fileScanImage.FileName;
                //string strLoanAdminPath = Server.MapPath(".");
                //string strFilePath = strLoanAdminPath.Replace("\\LoanAdmin", "") + "\\Data\\Invoice";
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Program_ID", "52");
                DataTable dtPath = Utility.GetDefaultData("S3G_LOANAD_GetDocPath", Procparam);
                if (dtPath != null && dtPath.Rows.Count > 0)
                {
                    strFilePath = dtPath.Rows[0]["DOCUMENT_PATH"].ToString();
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Define Scan Image Path in Document path setup");
                    return false;
                }

                if (!Directory.Exists(strFilePath))
                {
                    Utility.FunShowAlertMsg(this, "Define Scan Image Path in Document path setup");
                    return false;
                }
                //strFilePath = strFilePath + "\\" + strFileName;
                //if (File.Exists(strFilePath))
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Already the same file name(" + strFileName + ") exists in the target path");
                //    return false;
                //}
                //else
                //{
                //    if (bUpload)
                //    {
                //        fileScanImage.PostedFile.SaveAs(strFilePath);
                //    }
                //    ViewState["FilePath"] = strFilePath;
                //    ViewState["Download"] = 1;
                //    lblErrorMessage.Text = "Updated successfully";
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        else
        {
            //rfvFile.IsValid = false;
            //Utility.FunShowAlertMsg(this.Page, "Browse a file to upload");
            return false;
        }
        return true;
    }

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string strFilePath = "";
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Program_ID", "52");
            DataTable dtPath = Utility.GetDefaultData("S3G_LOANAD_GetDocPath", Procparam);
            if (dtPath != null && dtPath.Rows.Count > 0)
            {
                strFilePath = dtPath.Rows[0]["DOCUMENT_PATH"].ToString().Replace("\\", "/").Trim();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "The File not Exist");
                return;
            }
            string strFileName = strFilePath + "/" + hdnFile.Value;

            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            //string strScipt = "window.open('.." + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    # region Mail Preview

    private string GetHTMLText()
    {
        string strhtmltext = "";
        try
        {
            FunPriGetEntityEmailID();

            string name = ucCustDetails.CustomerName;
            string address = ucCustDetails.CustomerAddress;

            //txtTo.Text = ViewState["PopupEmailID"].ToString();
            txtTo.Text = "kuppusamy.b@sundaraminfotech.in";
            txtSubject.Text = "Asset Verification Request - " + name;

            string strbranch = "";
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0) strbranch = ddlBranch.SelectedText;
            string strLOB = "";
            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0) strLOB = ddlLOB.SelectedItem.ToString();
            //string strStatus = ddlStatus.SelectedItem.ToString();
            string strEnquiryNumber = "";

            strhtmltext = //"<font size=\"2\"  color=\"black\" face=\"verdana\">" +
               " <table align=\"center\" width=\"80%\" style=\"font-family:calibri,Arial, Sans-Serif; color:" + txtInstructions.ForeColor.Name.ToString() + "\">" +
            "<tr >" +
                "<td align=\"center\" >" +
                    "<font size=\"2\"  color=\"Black\" face=\"verdana\">" +
                   "<b>ASSET VERIFICATION REQUEST - " + name + "</b> " +
                      "</font> " +
                "</td>" +
           " </tr>" +
           "<tr>" +
                  "<td colspan=\"2\" height=\"15px\">" +
                        "</td>" +
                    "</tr>" +
           " <tr>" +
              "  <td  align=\"left\"  valign=\"top\" > " +
                  "  Respected Sir/Madam," +
               " <br />" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
               " Please verify the asset of the below account and send report at the earliest. </td>" +
            "</tr>" +

            "<tr>" +
               " <td align=\"left\"  valign=\"top\">" + "<br />" +
                  " Account Number - " + ddlMLA.SelectedItem.Text +
               " </td>" +
           " </tr>" +

            " <tr>" +
                "<td align=\"left\"  valign=\"top\">" +
                  " Customer Name - " + name +
               " </td>" +
           " </tr>" +

            " <tr>" +
                "<td align=\"left\"  valign=\"top\">" +
                  " Customer Address - " + "<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + address.Replace("\n", "<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") +
               " </td>" +
           " </tr>" +

            " <tr>" +
               " <td align=\"left\" valign=\"top\">" +
                  " Inspect between dates " + txtInspDate.Text + " and " + txtInspDateTo.Text +
               " </td>" +
            "</tr>" +

            " <tr>" +
               " <td align=\"left\" valign=\"top\">" +
                  " Inspect between time " + txtInspTime.Text + " and " + txtInspTimeTo.Text +
               " </td>" +
            "</tr>" +

            " <tr>" +
               " <td align=\"left\" valign=\"top\">" +
                  " Instructions -" + " <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                    txtInstructions.Text +
               " </td>" +
          "</tr>" +
              " <tr>" +
               " <td align=\"left\" valign=\"top\">" +
                  " <br />With Thanks & Regards <br />" +
                ObjUserInfo.ProUserNameRW +
               " </td>" +
            "</tr>" +
        "</table>";// +"</font>";
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
        return strhtmltext;
    }

    protected void btnMailPreview_Click(object sender, EventArgs e)
    {
        try
        {
            // FunPriAssignValuesToEmailPopUP();

            if (ModalPopupExtenderMailPreview.Enabled == false)
            {
                ModalPopupExtenderMailPreview.Enabled = true;

            }

            ModalPopupExtenderMailPreview.Show();

            FunPriAssignValuesToEmailPopUP();
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }

        //txtTo.Text = txtRemarks.Text;
        //txtBody.Text = "Respected Sir/Madam, \n\n\t" +
        //                txtFieldRequest.Text;
        //txtBody.Text += "\n\nWith thanks and regards,\n";
        //txtBody.Text += txtRequestBy.Text;
    }

    private void FunPriAssignValuesToEmailPopUP()
    {

        try
        {
            FunPriGetEntityEmailID();

            string name = ucCustDetails.CustomerName;
            string address = ucCustDetails.CustomerAddress;

            if (ViewState["PopupEmailID"] != null)
            {
                txtTo.Text = ViewState["PopupEmailID"].ToString();
            }
            txtSubject.Text = "Asset Verification Request - " + name;

            string Body = "Respected Sir/Madam, \n\n";
            Body += "Please verify the asset of the below account and send report at the earliest.\n\n";
            Body += "Account Number   - " + ddlMLA.SelectedItem.Text + "\n";
            Body += "Customer Name    - " + name + "\n";
            Body += "Customer Address - \n\t" + address.Replace("\n", "\n\t") + "\n\n";
            Body += "Inspect between dates " + txtInspDate.Text + " and " + txtInspDateTo.Text + "\n\n";
            Body += "Inspect between time " + txtHour.Text + ":" + txtMins.Text + " " + ddlAMPM.SelectedItem.Text + " and " + txtHourTo.Text + ":" + txtMinsTo.Text + " " + ddlAMPMTo.SelectedItem.Text + "\n\n";
            Body += "Instructions - \n\t" + txtInstructions.Text + "\n\n";
            Body += "\n\n With thanks and regards,\n\n";
            Body += ObjUserInfo.ProUserNameRW + "\n\n";
            txtBody.Text = Body;
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }

    }

    protected void PreviewPDF_Click(object sender, EventArgs e)
    {
        //this feature may require in future..so commented
        //try
        //{
        //    String htmlText = GetHTMLText();
        //    string FIRNum = "";
        //    FIRNum = ViewState["FIRNum"].ToString();
        //    if ((string.IsNullOrEmpty(FIRNum)) || FIRNum == "")
        //    {
        //        FIRNum = DateTime.Now.ToString() + "/";
        //    }
        //    string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + FIRNum.Replace("/", "").Replace(" ", "").Replace(":", "") + ".pdf");
        //    string strFileName = "/Origination/PDF Files/" + FIRNum.Replace("/", "").Replace(" ", "").Replace(":", "") + ".pdf";
        //    Document doc = new Document();
        //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));
        //    doc.AddCreator("Sundaram Infotech Solutions");
        //    doc.AddTitle("New PDF Document");
        //    doc.Open();
        //    List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
        //    for (int k = 0; k < htmlarraylist.Count; k++)
        //    {
        //        doc.Add((IElement)htmlarraylist[k]);
        //    }
        //    doc.AddAuthor("S3G Team");
        //    doc.Close();

        //    Session["strPath"] = strnewFile;
        //    string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

        //    //System.Diagnostics.Process.Start(strnewFile);
        //    ModalPopupExtenderMailPreview.Hide();
        //}
        //catch (Exception ex)
        //{
        //    Utility.FunShowAlertMsg(this, ex.Message);
        //}
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {

        ModalPopupExtenderMailPreview.Hide();
        CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();

        try
        {
            string Body = GetHTMLText();

            FunPriGetEntityEmailID();

            string name = ucCustDetails.CustomerName;
            string address = ucCustDetails.CustomerAddress;

            //txtTo.Text = ViewState["PopupEmailID"].ToString();
            txtTo.Text = "kuppusamy.b@sundaraminfotech.in";
            txtSubject.Text = "Asset Verification Request - " + name;

            //string Body = "Respected Sir/Madam, \n\n";
            //Body += "Please verify the account of the below account and send report at the earliest.\n\n";
            //Body += "Account Number   - " + ddlMLA.SelectedItem.Text + "\n";
            //Body += "Customer Name    - " + name + "\n";
            //Body += "Customer Address - \n\t" + address.Replace("\n", "\n\t") + "\n\n";
            //Body += "Inspect between dates " + txtInspDate.Text + " and " + txtInspDateTo.Text + "\n\n";
            //Body += "Inspect between time " + txtHour.Text + ":" + txtMins.Text + " " + ddlAMPM.SelectedItem.Text + " and " + txtHourTo.Text + ":" + txtMinsTo.Text + " " + ddlAMPMTo.SelectedItem.Text + "\n\n";
            //Body += "Instructions - \n\t" + txtInstructions.Text + "\n\n";
            //Body += "\n\n With thanks and regards,\n\n";
            //Body += ObjUserInfo.ProUserNameRW + "\n\n";
            //txtBody.Text = Body;

            //CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
            ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
            ObjCom_Mail.ProFromRW = "kuppusamy.b@sundaraminfotech.in";
            ObjCom_Mail.ProTORW = txtTo.Text;

            ObjCom_Mail.ProSubjectRW = txtSubject.Text;
            ObjCom_Mail.ProMessageRW = Body;
            ObjCommonMail.FunSendMail(ObjCom_Mail);
            Utility.FunShowAlertMsg(this, "Mail sent successfully");
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, "Invalid EMail ID. Mail not sent.");
        }
        finally
        {
            ObjCommonMail.Close();
        }
    }

    protected void btnClosePreview_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopupExtenderMailPreview.Hide();
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            lblErrorMessage.Text = ex.Message;
        }
    }


    # endregion

    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

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
        if (obj_Page.ObjUserInfo.ProUserTypeRW.ToUpper() == "UTPA")
        {

            Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
            Procparam.Add("@UTPA_ID", obj_Page.intUserID.ToString());
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@PrefixText", prefixText);
           // ddlBranch.BindDataTable("S3G_Get_UTPA_Branch_List", Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_UTPA_Branch_List_AGT", Procparam));
        }
        else
        {
            Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
            Procparam.Add("@Type", "GEN");
            Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
            Procparam.Add("@Program_Id", "052");
            Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@PrefixText", prefixText);
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));
        }
        return suggestions.ToArray();
    }

}
