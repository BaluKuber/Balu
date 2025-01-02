#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin 
/// Screen Name			: Account Split
/// Created By			: Nataraj Y
/// Created Date		: 06-Sep-2010
/// Purpose	            : 
/// Modified By			: Nataraj Y
/// Modified Date		: 22-Mar-2011
/// Purpose	            : Bug Fixing Round 2 
/// Modified By			: Nataraj Y
/// Modified Date		: 21-Apr-2011
/// Purpose	            : Bug Fixing Round 3
/// Modified By			: Shibu
/// Modified Date		: 30-Sep-2013
/// Purpose	            : Performance Tuning To DDL changed to Auto Suggestion Control 
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
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
using S3GBusEntity.LoanAdmin;
#endregion

public partial class LoanAdmin_S3GLoanAdAccountConsolidation : ApplyThemeForProject
{
    #region Variable declaration
    Dictionary<string, string> Procparam;
    int _SlNo = 0;
    int intResult;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bClearList = false;
    string strConsNumber;
    public string strDateFormat;
    static string strMode;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACON';";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdAccountConsolidation.aspx';";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACON";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    ContractMgtServicesReference.ContractMgtServicesClient ObjContarctMgtServices;
    ContractMgtServices.S3G_LOANAD_AccountConsolidationDataTable ObjAccountConsolidationTable;
    public static LoanAdmin_S3GLoanAdAccountConsolidation obj_Page;
    UserInfo ObjUserInfo;
    int intCompany_Id, intUserId;
    string strPageName = "Account Consolidation";
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (ObjUserInfo.ProUserLevelIdRW < 3)
        //{

        //    strAlert = "alert('You do not have access rights to this page');" + strRedirectPageView + "";
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
        //    return;
        //}
        obj_Page = this;

        ObjUserInfo = new UserInfo();
        obj_Page = this;
        intCompany_Id = ObjUserInfo.ProCompanyIdRW;

        intUserId = ObjUserInfo.ProUserIdRW;
        if (Request.QueryString["Popup"] != null)
            btnCancel.Enabled = false;

        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];

        if (Request.QueryString.Get("qsViewId") != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            strConsNumber = Convert.ToString(fromTicket.Name);
        }

        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
        txtConsolidateAccountDate.Attributes.Add("readonly", "readonly");
        //txtConsolidationDate.Attributes.Add("readonly", "readonly");
        txtConsolidateAccountDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtConsolidateAccountDate.ClientID + "','" + strDateFormat + "',true,  false);");
        //txtCustomerAddress.Attributes.Add("readonly", "readonly");
        txtNewFinancedAmount.Attributes.Add("readonly", "readonly");
        txtConsNumber.Attributes.Add("readonly", "readonly");
        // strDateFormat = ObjS3GSession.ProDateFormatRW;
        calExeConsAccDate.Format = DateFormate;
        calExeConsDate.Format = DateFormate;
        if (!IsPostBack)
        {
            FunPriPageLoad();
            cmbCustomerCode.SelectedIndex = 0;
            cmbCustomerCode.Focus();
            if (strConsNumber == "")
                PanAccount.Visible = false;
            else
                PanAccount.Visible = true;

        }

    }

    protected void cmbCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadValuesFromCustomer();
        cmbCustomerCode.Focus();
    }

    protected void ddlLineofBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLineofBusiness.SelectedIndex > 0)
        {
            if (!FunPriLOBBasedvalidations(ddlLineofBusiness.SelectedItem.Text))
            {
                Utility.FunShowAlertMsg(this, "Consolidation not applicable for selected Line of Business");
                return;
            }
            ddlBranch.Clear();
            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //Procparam.Clear();
            //Procparam.Add("@Is_Active", "1");
            //Procparam.Add("@User_Id", UserId);
            //Procparam.Add("@Company_ID", CompanyId);
            //Procparam.Add("@Lob_Id", ddlLineofBusiness.SelectedValue);
            //Procparam.Add("@Program_Id", "70");
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
        }

        txtNewFinancedAmount.Text = "0";
        grvConsolidation.DataSource = null;
        grvConsolidation.DataBind();
        PanAccount.Visible = false;
        ddlLineofBusiness.Focus();

    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtNewFinancedAmount.Text = "0";
        grvConsolidation.DataSource = null;
        grvConsolidation.DataBind();
        PanAccount.Visible = false;
        ddlBranch.Focus();
    }

    protected void ChkSelectAccount_CheckedChanged(object sender, EventArgs e)
    {

        bool blnPrevSubNum = false;
        bool blnCurrentSunNum = false;
        bool blnIsFirstRow = true;
        string strSANum = "";
        string strCurrentPANum = "";
        string strPrevPANum = "";

        if (hdnMLASLA.Value == "True")
        {
            foreach (GridViewRow grvRow in grvConsolidation.Rows)
            {
                CheckBox chkbxSelectAccount = (CheckBox)grvRow.FindControl("ChkSelectAccount");
                if (chkbxSelectAccount.Checked)
                {
                    strSANum = ((Label)grvRow.FindControl("lblSubNo")).Text;
                    strCurrentPANum = ((Label)grvRow.FindControl("lblPanNo")).Text;
                    if (strSANum.Length > 0)
                        blnCurrentSunNum = true;
                    else
                        blnCurrentSunNum = false;
                    if (!blnIsFirstRow)
                    {
                        //if (FunPriChkMLASLA(grvConsolidation, strCurrentPANum, strPrevPANum))
                        //{
                        if (blnCurrentSunNum != blnPrevSubNum)
                        {
                            Utility.FunShowAlertMsg(this, "Cannot Consolidate");
                            ((CheckBox)sender).Checked = false;
                            return;
                        }
                        else
                        {
                            if ((blnCurrentSunNum & blnPrevSubNum) && (strCurrentPANum != strPrevPANum))
                            {
                                Utility.FunShowAlertMsg(this, "Cannot Consolidate");
                                ((CheckBox)sender).Checked = false;
                                return;
                            }
                        }
                        //}
                        //else
                        //{
                        //    Utility.FunShowAlertMsg(this, "Cannot Consolidate");
                        //    ((CheckBox)sender).Checked = false;
                        //    return;
                        //}
                    }
                    blnIsFirstRow = false;
                    blnPrevSubNum = blnCurrentSunNum;
                    strPrevPANum = strCurrentPANum;
                }

            }
        }

        string strFieldAtt = ((CheckBox)sender).ClientID;
        if (((CheckBox)sender).Checked)
        {

            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvConsolidation_")).Replace("grvConsolidation_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;

            decimal decAmount = Convert.ToDecimal(((Label)grvConsolidation.Rows[gRowIndex].FindControl("lblPrincipalOutStanding")).Text);

            decimal decTotalamount = Convert.ToDecimal(txtNewFinancedAmount.Text) + decAmount;
            txtNewFinancedAmount.Text = decTotalamount.ToString();

        }
        if (!((CheckBox)sender).Checked)
        {
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvConsolidation_")).Replace("grvConsolidation_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            decimal decAmount = Convert.ToDecimal(((Label)grvConsolidation.Rows[gRowIndex].FindControl("lblPrincipalOutStanding")).Text);
            if (txtNewFinancedAmount.Text != "0")
            {
                decimal decTotalamount = Convert.ToDecimal(txtNewFinancedAmount.Text) - decAmount;
                txtNewFinancedAmount.Text = decTotalamount.ToString();
            }

        }
    }

    #region Button
    protected void btnGo_Click(object sender, EventArgs e)
    {
        txtNewFinancedAmount.Text = "0";
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Option", "1");
        Procparam.Add("@Company_ID", CompanyId.ToString());
        Procparam.Add("@Customer_ID", cmbCustomerCode.SelectedValue);
        Procparam.Add("@LOB_ID", ddlLineofBusiness.SelectedValue);
        Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
        //Procparam.Add("@Param1", hdnMLASLA.Value);
        //if (hdnMLASLA.Value == "True")
        //    Procparam.Add("@Param2", ddlMLA.SelectedItem.Text);
        DataTable dtConsolidation = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAccCons_Split_List, Procparam);
        if (dtConsolidation.Rows.Count > 1)
        {
            grvConsolidation.DataSource = dtConsolidation;
            grvConsolidation.DataBind();
            grvConsolidation.Columns[8].Visible = false;
            grvConsolidation.Columns[9].Visible = false;
            PanAccount.Visible = true;
        }
        else
        {
            PanAccount.Visible = false;
            Utility.FunShowAlertMsg(this, "Consolidation is applicable only if you have multiple accounts for selected combination");
            btnGo.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveConsolidation();
            btnSave.Focus();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cv_Consolidation.ErrorMessage = ex.Message;
            cv_Consolidation.IsValid = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtConsNumber.Text = "";
        txtConsolidateAccountDate.Text = "";
        //   txtCustomerAddress.Text = "";
        S3GCustomerCommAddress.ClearCustomerDetails();
        txtNewFinancedAmount.Text = "0";
        txtConsolidationDate.Text = "";
        grvConsolidation.ClearGrid();
        PanAccount.Visible = false;
        //ddlBranch.SelectedIndex = 0;
        ddlBranch.Clear();
        ddlLineofBusiness.SelectedIndex = 0;
        cmbCustomerCode.SelectedIndex = 0;

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else            
        Response.Redirect(strRedirectPage,false);
    }

    protected void btnCreateAcc_Click(object sender, EventArgs e)
    {
        Session.Remove("ApplicationAssetDetails");
        Session.Remove("AccountAssetCustomer");
        FunPriPageLoad();

    }

    protected void btnConsCancel_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriCancelConsolidation();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    protected void btnViewAccount_Click(object sender, EventArgs e)
    {
        if (hdnAccID.Value != "")
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hdnAccID.Value, false, 0);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Open Account", "window.showModalDialog('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&IsFromAccount=Acccon&qsMode=Q', 'Account Creation', 'dialogwidth:900px;dialogHeight:900px;')", true);
        }
    }
    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// Method to Save Consolidation
    /// </summary>
    private void FunPriSaveConsolidation()
    {
        ObjContarctMgtServices = new ContractMgtServicesReference.ContractMgtServicesClient();

        try
        {
            if (txtNewFinancedAmount.Text != "0")
            {
                if (FunPriCheckConsolAccounts())
                {
                    string strAcconNo;
                    ObjAccountConsolidationTable = new ContractMgtServices.S3G_LOANAD_AccountConsolidationDataTable();
                    ContractMgtServices.S3G_LOANAD_AccountConsolidationRow ObjAccountConsolidatioRow;
                    ObjAccountConsolidatioRow = ObjAccountConsolidationTable.NewS3G_LOANAD_AccountConsolidationRow();
                    ObjAccountConsolidatioRow.Company_ID = int.Parse(CompanyId);
                    ObjAccountConsolidatioRow.LOB_ID = Convert.ToInt32(ddlLineofBusiness.SelectedValue);
                    ObjAccountConsolidatioRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
                    ObjAccountConsolidatioRow.Consolidated_Customer_ID = Convert.ToInt32(cmbCustomerCode.SelectedValue);
                    ObjAccountConsolidatioRow.Account_Consolidation_Date = Utility.StringToDate(txtConsolidateAccountDate.Text);
                    ObjAccountConsolidatioRow.New_Amount_Finanaced = Convert.ToDecimal(txtNewFinancedAmount.Text);
                    ObjAccountConsolidatioRow.Consolidated_PANum = "12";
                    ObjAccountConsolidatioRow.Created_By = int.Parse(UserId);
                    ObjAccountConsolidatioRow.XMLConsolDetails = grvConsolidation.FunPubFormXml(true);

                    ObjAccountConsolidationTable.AddS3G_LOANAD_AccountConsolidationRow(ObjAccountConsolidatioRow);
                    SerializationMode SerMode = SerializationMode.Binary;
                    byte[] objbyteAccountConsolidate = ClsPubSerialize.Serialize(ObjAccountConsolidationTable, SerMode);
                    intResult = ObjContarctMgtServices.FunPubCreateAccountConsolidation(out strAcconNo, SerMode, objbyteAccountConsolidate);
                    switch (intResult)
                    {
                        case 0:
                            strAlert = "Consolidation no " + strAcconNo + " Generated sucessfully";
                            strAlert += @"\n\nWould you like to Consolidate more accounts?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            txtConsNumber.Text = strAcconNo;
                            break;
                        case -1:
                            strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                            strRedirectPageView = "";
                            break;
                        case -2:
                            strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                            strRedirectPageView = "";
                            break;
                        case 2:
                            strAlert = strAlert.Replace("__ALERT__", "Consolidation details cannot be empty");
                            strRedirectPageView = "";
                            break;
                        case 4://Changed for Bug ID 3234
                            strAlert = strAlert.Replace("__ALERT__", "Consolidation date cannot be less than selected agreement date");
                            strRedirectPageView = "";
                            break;
                        case 5:
                            strAlert = strAlert.Replace("__ALERT__", "Consolidation Date cannot be in a closed month");
                            strRedirectPageView = "";
                            break;
                        default:
                            strAlert = strAlert.Replace("__ALERT__", "Error in creating Consolidation");
                            strRedirectPageView = "";
                            break;
                    }

                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "More than one account should be Chosen for Consolidation");
                    strRedirectPageView = "";

                }
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Select atleast one account for Consolidation");
                strRedirectPageView = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new Exception("Unable to save Acoount consolidation");
        }
        finally
        {
            ObjContarctMgtServices.Close();
        }
    }

    /// <summary>
    /// TO Load Customer Details
    /// </summary>
    private void FunPriLoadCustomerCode()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Option", "6");
        Procparam.Add("@Company_ID", CompanyId.ToString());
        cmbCustomerCode.BindDataTable("S3G_LOANAD_GetAccCons_Split_List", Procparam, true, "--Select--", new string[] { "Customer_ID", "Customer_Code", "Customer_Name" });
    }

    /// <summary>
    /// To Show Customer Details and Load LOB and Branch
    /// </summary>
    private void FunPriLoadValuesFromCustomer()
    {
        if (cmbCustomerCode.SelectedIndex > 0)
        {
            FunPriLoadCustomerDetails(cmbCustomerCode.SelectedItem.Value);
            FunPriLoadLObandBranch();
        }
        //Added for bug id 3238
        grvConsolidation.DataSource = null;
        grvConsolidation.DataBind();
        //ddlLineofBusiness.Focus();
    }


    private void FunPriLoadLObandBranch()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@User_Id", UserId);
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@FilterOption", "'HP','LN','FL'");
        Procparam.Add("@Program_Id", "70");
        ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        //ddlLineofBusiness.Items.
        Procparam.Remove("@FilterOption");
        if (strMode != "C")
        {
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
        }
    }

    private void FunPriLoadCustomerDetails(string strCustomer_Id)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Option", "4");
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@Customer_ID", strCustomer_Id);
        //txtCustomerAddress.Text = Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, Procparam).Rows[0]["Customer_Address"].ToString();
        S3GCustomerCommAddress.SetCustomerDetails(Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, Procparam).Rows[0], true);

    }

    private void FunPriLoadMLASLADetails(string strCustomer_Id, string strLOBid)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Option", "4");
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@Customer_ID", strCustomer_Id);
        // txtCustomerAddress.Text = Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, Procparam).Rows[0]["Customer_Address"].ToString();
        S3GCustomerCommAddress.SetCustomerDetails(Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, Procparam).Rows[0], true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strType"></param>
    private bool FunPriLOBBasedvalidations(string strType)
    {
        strType = strType.Split('-')[0].Trim();
        switch (strType.ToLower())
        {

            case "hp": //Term loan Extensible
            case "fl": //Term loan
            case "ln": //Factoring 

                btnGo.Enabled = true;
                Procparam = new Dictionary<string, string>();
                Procparam.Clear();
                Procparam.Add("@CompanyId", CompanyId);
                Procparam.Add("@LobId", ddlLineofBusiness.SelectedValue);
                hdnMLASLA.Value = Utility.GetDefaultData("S3g_LoanAd_GetMLASLAApplicable", Procparam).Rows[0]["MLAandSLA"].ToString();

                return true;
                break;

            default://for default case
                btnGo.Enabled = false;
                return false;

                break;
        }
    }

    private bool FunPriChkMLASLA(GridView grvGridview, string strCurrentMLA, string strprevMLA)
    {
        string strCurrentPANum;
        int intCount = 0;
        foreach (GridViewRow grvRow in grvGridview.Rows)
        {
            strCurrentPANum = ((Label)grvRow.FindControl("lblPanNo")).Text;
            if (strCurrentMLA == strCurrentPANum)
            {
                intCount = intCount + 1;
            }
            else if (strprevMLA == strCurrentPANum)
            {
                intCount = intCount + 1;
            }
        }
        if (intCount > 1)
        {
            if (strCurrentMLA != strprevMLA)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    private bool FunPriCheckConsolAccounts()
    {
        int intReccount = 0;

        foreach (GridViewRow grvRow in grvConsolidation.Rows)
        {
            if (((CheckBox)grvRow.FindControl("ChkSelectAccount")).Checked)
            {
                intReccount++;
            }
        }
        if (intReccount > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FunPriPageLoad()
    {
        if (strMode == "C")
        {
            FunPriLoadCustomerCode();
            FunPriLoadLObandBranch();
        }
        if (strConsNumber != null)
        {
            FunPriLoadAccountConsolidationDtls(strConsNumber);
            if (strMode == "M")
            {
                FunPriConsolidationControlStatus(1);
            }
            if (strMode == "Q")
            {
                FunPriConsolidationControlStatus(-1);
            }
        }
        else
        {
            FunPriConsolidationControlStatus(0);
        }
    }

    private void FunPriConsolidationControlStatus(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                btnCreateAcc.Visible = false;
                btnViewAccount.Visible = false;
                btnConsCancel.Visible = false;
                btnViewAccount.Visible = false;
                break;
            case 1://Modify

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                imgConsAccDate.Visible = false;
                imgConsDate.Visible = false;
                txtConsolidateAccountDate.Attributes.Remove("onblur");
                txtConsolidateAccountDate.Attributes.Add("readonly", "readonly");
                if (hdnStatus.Value == "3")
                {
                    btnCreateAcc.Visible = false;
                    btnConsCancel.Visible = false;

                }
                else
                {

                    if (hdnStatus.Value == "1")
                    {
                        btnConsCancel.Visible = false;
                        btnCreateAcc.Visible = true;

                    }
                    else
                    {
                        btnConsCancel.Visible = true;
                        btnCreateAcc.Visible = false;

                    }
                    btnViewAccount.Visible = true;
                }
                if (hdnAccID.Value == "")
                {
                    btnViewAccount.Visible = false;
                    btnConsCancel.Visible = false;
                }
                else
                {
                    btnViewAccount.Visible = true;
                    if (hdnStatus.Value != "5" && hdnStatus.Value != "3")
                        btnConsCancel.Visible = true;
                    else
                        btnConsCancel.Visible = false;
                }
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(txtConsNumber.Text, false, 0);
                btnCreateAcc.Attributes.Add("OnClick", string.Format("javascript:ViewModal('{0}')", FormsAuthentication.Encrypt(Ticket)));
                break;
            case -1://Query  
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                imgConsAccDate.Visible = false;
                btnCreateAcc.Visible = false;
                btnConsCancel.Visible = false;
                imgConsDate.Visible = false;
                btnViewAccount.Visible = true;
                txtConsolidateAccountDate.Attributes.Remove("onblur");
                txtConsolidateAccountDate.Attributes.Add("readonly", "readonly");
                break;
        }
    }

    private void FunPriLoadAccountConsolidationDtls(string strAcconNo)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@ConsNo", strConsNumber);
        DataSet dsAccConsDetails = Utility.GetDataset(SPNames.S3G_LOANAD_GetAccConsDetails, Procparam);
        if (dsAccConsDetails.Tables[0].Rows.Count > 0)
        {
            DataRow drAccConsDetailsRow = dsAccConsDetails.Tables[0].Rows[0];
            cmbCustomerCode.Items.Add(new ListItem(drAccConsDetailsRow["Customer"].ToString(),drAccConsDetailsRow["Consolidated_Customer_ID"].ToString()));
            cmbCustomerCode.ToolTip = drAccConsDetailsRow["Customer"].ToString();
         
            //cmbCustomerCode.ClearDropDownList();
            ddlBranch.SelectedText = drAccConsDetailsRow["Location_Name"].ToString();
            ddlBranch.SelectedValue = drAccConsDetailsRow["Location_ID"].ToString();
            ddlBranch.ToolTip = drAccConsDetailsRow["Location_Name"].ToString();
            //ddlBranch.ClearDropDownList();
            ddlLineofBusiness.Items.Add(new ListItem(drAccConsDetailsRow["LOB_Name"].ToString(), drAccConsDetailsRow["LOB_ID"].ToString()));
            ddlLineofBusiness.ToolTip = drAccConsDetailsRow["LOB_Name"].ToString();
            //ddlLineofBusiness.ClearDropDownList();

            txtConsNumber.Text = drAccConsDetailsRow["Consolidation_No"].ToString();
            //txtCustomerAddress.Text = dsConsDetails.Tables[0].Rows[0]["Customer_Address"].ToString();
            S3GCustomerCommAddress.SetCustomerDetails(drAccConsDetailsRow, true);
            txtConsolidateAccountDate.Text = drAccConsDetailsRow["Account_Consolidation_Date"].ToString();
            txtConsolidationDate.Text = drAccConsDetailsRow["Account_Consolidation_Date"].ToString();
            txtNewFinancedAmount.Text = drAccConsDetailsRow["New_Amount_Finanaced"].ToString();
            hdnStatus.Value = drAccConsDetailsRow["Consolidation_Status_Code"].ToString();
            hdnConsPAN.Value = drAccConsDetailsRow["Consolidated_PANum"].ToString();
            hdnConsSAN.Value = drAccConsDetailsRow["Consolidated_SANum"].ToString();
            hdnAccID.Value = drAccConsDetailsRow["Account_Creation_ID"].ToString();
            if (hdnConsPAN.Value != "" || hdnConsSAN.Value != "")
            {
                btnCreateAcc.Enabled = false;
                btnViewAccount.Visible = true;
            }
            else
            {
                btnCreateAcc.Enabled = true;
                btnViewAccount.Visible = false;
            }
            calExeConsAccDate.Enabled = false;
            calExeConsDate.Enabled = false;
            grvConsolidation.DataSource = dsAccConsDetails.Tables[1];
            grvConsolidation.DataBind();

            grvConsolidation.Columns[5].Visible = false;
            grvConsolidation.Columns[7].Visible = false;
            grvConsolidation.Columns[9].Visible = false;
            grvConsolidation.Columns[8].Visible = false;
            grvConsolidation.Columns[1].Visible = false;
        }
    }

    private void FunPriCancelConsolidation()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@DocNumber", txtConsNumber.Text);
            Procparam.Add("@Company_Id", "1");
            Procparam.Add("@Option", "4");
            DataTable dtTable = Utility.GetDefaultData("S3G_LoanAd_UpdateAccountStatus_SplitandConslidation", Procparam);
            if (dtTable.Rows[0]["Status"].ToString() == "Sucess")
            {
                strAlert = strAlert.Replace("__ALERT__", "Consolidation " + txtConsNumber.Text + " Cancelled Sucessfully");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Error in Cancelling Consolidation " + txtConsNumber.Text);
                strRedirectPageView = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
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

        Procparam.Add("@Company_ID", obj_Page.intCompany_Id.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "070");
        Procparam.Add("@Lob_Id", obj_Page.ddlLineofBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    #endregion

   
}
