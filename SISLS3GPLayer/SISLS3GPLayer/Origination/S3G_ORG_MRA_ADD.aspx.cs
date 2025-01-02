using System;
using System.Web;
using System.Data;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.ServiceModel;
using AjaxControlToolkit;

public partial class Origination_S3G_ORG_MRA_ADD : ApplyThemeForProject
{
    #region Common Variable declaration

    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    OrgMasterMgtServices.S3G_ORG_MRADataTable ObjMRADt = new OrgMasterMgtServices.S3G_ORG_MRADataTable();


    int intCompanyID, intUserID = 0;
    string strMode = string.Empty;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    int intMRAId;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string strDateFormat = string.Empty;
    //string strMode = string.Empty;
    static string strPageName = "MRA Creation";
    static string strSuffix = "";
    FormsAuthenticationTicket Ticket;
    public static Origination_S3G_ORG_MRA_ADD obj_Page;


    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Origination/S3GORGTransLander.aspx?Code=MRA";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_MRA_ADD.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=MRA';";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";

    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    S3GSession ObjS3GSession = new S3GSession();
    int strDecMaxLength = 0;
    int strPrefixLength = 0;
    #endregion

    #region EVENTS

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception objException)
        {
            cvMRA.ErrorMessage = objException.Message;
            cvMRA.IsValid = false;
        }
    }

    protected void btnCreateCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = ucCustomerCodeLov.FindControl("hdnID") as HiddenField;
            if (hdnCustomerId != null && Convert.ToString(hdnCustomerId.Value) != "")
            {
                ViewState["CustomerID"] = hdnCustomerId.Value;
                FunPriGetCustomerAddress(Convert.ToInt64(hdnCustomerId.Value));
                lnkViewCustomer.Enabled = true;
            }
        }
        catch (Exception objException)
        {
            cvMRA.ErrorMessage = objException.Message;
            cvMRA.IsValid = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strErrorMsg = cvMRA.ErrorMessage = string.Empty;
            if (Convert.ToDouble(txtForeClosureRate.Text) > 100)
            {
                strErrorMsg = strErrorMsg + "Fore Closure Rate(%) should not be greater than 100%" + "<br/>";
            }

            if (Convert.ToDouble(txtOverdueRate.Text) > 100)
            {
                strErrorMsg = strErrorMsg + "Over Due Rate(%) should not be greater than 100%" + "<br/>";
            }

            if (((Convert.ToString(txtBreakCost.Text) == "") ? 0 : Convert.ToDouble(txtBreakCost.Text)) > 100)
            {
                strErrorMsg = strErrorMsg + "Break Cost(%) should not be greater than 100%" + "<br/>";
            }

            if (Convert.ToString(hdnAcctManager1ID.Value) == "" && Convert.ToString(txtAcctmanager1.Text) != "")
            {
                strErrorMsg = strErrorMsg + "Invalid Account Manager 1" + "<br/>";
            }

            if (Convert.ToString(hdnAcctManager2ID.Value) == "" && Convert.ToString(txtAcctmanager2.Text) != "")
            {
                strErrorMsg = strErrorMsg + "Invalid Account Manager 2" + "<br/>";
            }

            if (Convert.ToString(hdnRegionalMangerID.Value) == "" && Convert.ToString(txtRegionalManager.Text) != "")
            {
                strErrorMsg = strErrorMsg + "Invalid Regional Manager" + "<br/>";
            }

            if (Convert.ToString(strErrorMsg) != "")
            {
                cvMRA.ErrorMessage = strErrorMsg;
                cvMRA.IsValid = false;
                return;
            }

            FunPriSaveMRADtls();
        }
        catch (Exception objException)
        {
            cvMRA.ErrorMessage = objException.Message;
            cvMRA.IsValid = false;
        }
    }
    protected void txtInvoiceGraceDays_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtInvoiceGraceDays.Text != string.Empty)
            {
                if (Convert.ToInt32(txtInvoiceGraceDays.Text) > 99)
                {
                    Utility.FunShowAlertMsg(this, "Invoice Grace Period should not exceed than the 99 Days or month");
                    txtInvoiceGraceDays.Text = string.Empty;
                    txtInvoiceGraceDays.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);

        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
        }
        catch (Exception objException)
        {
            cvMRA.ErrorMessage = objException.Message;
            cvMRA.IsValid = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else
                Response.Redirect(strRedirectPage);
        }
        catch (Exception objException)
        {
            cvMRA.ErrorMessage = objException.Message;
            cvMRA.IsValid = false;
        }
    }

    protected void lnkViewCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(Convert.ToString(ViewState["CustomerID"]), false, 0);
            string strViewPage = "../Origination/S3GOrgCustomerMaster_Add.aspx?qsCustomerId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsFromApplication=yes";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + strViewPage + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
            return;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    #endregion

    #region METHODS

    /// <summary>
    /// Load Page Details
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            obj_Page = this;
            S3GSession ObjS3GSession = null;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            //Date Format
            ObjS3GSession = new S3GSession();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            strPrefixLength = ObjS3GSession.ProGpsPrefixRW;
            strDecMaxLength = ObjS3GSession.ProGpsSuffixRW;

            txtMRACreationDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtMRACreationDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtMRAEffectiveDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtMRAEffectiveDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtAuthorizationDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtAuthorizationDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtResolutionDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtResolutionDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtAmendmentDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtAmendmentDate.ClientID + "','" + strDateFormat + "',false,  false);");

            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txtUserName = ((TextBox)ucCustomerCodeLov.FindControl("txtName"));
            txtUserName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtUserName.ToolTip = txtUserName.Text;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                    intMRAId = Convert.ToInt32(fromTicket.Name);
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

            if (!IsPostBack)
            {
                ceAmendmentDate.Format = ceMRACreationDate.Format = ceMRAEffectiveDate.Format = ceAuthorizationDate.Format = ceResolutionDate.Format = strDateFormat;
                txtMRACreationDate.Text = Convert.ToString(System.DateTime.Now.ToString(strDateFormat));
                FunPriLoadLov();
                if (strMode == "C")
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    intMRAId = 0;
                    FunPriDisableControls();
                    ddlMRAAmended.SelectedValue = ddlInvoiceGraceType.SelectedValue = "2";
                    txtInvoiceGraceDays.Text = "0";
                    //ddlMRAAmended.ClearDropDownList();
                }
                else if (strMode == "M")
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunPriLoadMRADtls(intMRAId);
                    FunPriDisableControls();
                }
                else if (strMode == "Q")
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FunPriLoadMRADtls(intMRAId);
                    FunPriDisableControls();
                }
            }

        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// Load LOV Values
    /// </summary>
    private void FunPriLoadLov()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            DataSet dsLov = Utility.GetDataset("S3G_ORG_GETMRA_LOOKUP", Procparam);

            //MRA Authorization Basis
            FunFillGrid(ddlAuthorizationBasis, dsLov.Tables[0], "ID", "Name");

            //MRA Interim Rent Calculation Basis
            FunFillGrid(ddlInterimCalculation, dsLov.Tables[1], "ID", "Name");

            DataTable dtLov = new DataTable();
            dtLov.Columns.Add("ID");
            dtLov.Columns.Add("Name");
            DataRow dr = dtLov.NewRow();
            dr["ID"] = 0;
            dr["Name"] = "Select";
            dtLov.Rows.Add(dr);
            dr = dtLov.NewRow();
            dr["ID"] = 1;
            dr["Name"] = "Yes";
            dtLov.Rows.Add(dr);
            dr = dtLov.NewRow();
            dr["ID"] = 2;
            dr["Name"] = "No";
            dtLov.Rows.Add(dr);
            dtLov.AcceptChanges();

            FunPriFillDropDown(dtLov, ddlStandardFormat);
            FunPriFillDropDown(dtLov, ddlSignedReceived);
            FunPriFillDropDown(dtLov, ddlBoardAuthorization);
            FunPriFillDropDown(dtLov, ddlOPCNotice);
            FunPriFillDropDown(dtLov, ddlAutoExtension);
            FunPriFillDropDown(dtLov, ddlInterimRent);
            FunPriFillDropDown(dtLov, ddlArbiterationClause);
            FunPriFillDropDown(dtLov, ddlMRAAmended);
            FunPriFillDropDown(dtLov, ddlCustomerNtPrd);
            FunPriFillDropDown(dtLov, ddlJurisdiction);

            txtForeClosureRate.SetDecimalPrefixSuffix(3, 2, false, false, "Fore Closure Rate(%)");
            txtBreakCost.SetDecimalPrefixSuffix(3, 2, false, false, "Break Cost(%)");
            txtOverdueRate.SetDecimalPrefixSuffix(3, 2, false, false, "Over Due Rate(%)");
            txtInterimRentRate.SetDecimalPrefixSuffix(5, 2, true, false, "Interim Rent Rate");
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// Fill Dropdown Value
    /// </summary>
    /// <param name="ddlList"></param>
    /// <param name="dt"></param>
    /// <param name="strValueField"></param>
    /// <param name="strDisplayFeild"></param>
    protected void FunFillGrid(DropDownList ddlList, DataTable dt, string strValueField, string strDisplayFeild)
    {
        try
        {
            if (ddlList != null && ddlList.Items.Count > 0)
            {
                ddlList.Items.Clear();
            }

            ddlList.DataSource = dt;
            ddlList.DataValueField = strValueField;
            ddlList.DataTextField = strDisplayFeild;
            ddlList.DataBind();
            ddlList.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// Fill Default Dropdown LOV values
    /// </summary>
    /// <param name="dtLov"></param>
    /// <param name="objDDL"></param>
    private void FunPriFillDropDown(DataTable dtLov, DropDownList objDDL)
    {
        try
        {
            objDDL.DataSource = dtLov;
            objDDL.DataTextField = "Name";
            objDDL.DataValueField = "ID";
            objDDL.DataBind();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriSetGPSLength()
    {
        try
        {
            txtOPCtoCustomer.CheckGPSLength(true, "OPC to Customer");
            txtCustomertoOPC.CheckGPSLength(true, "Customer to OPC");
            txtAutoExtensionTerm.CheckGPSLength(true, "Auto Extension Term");
            txtCustomerNoticePeriod.CheckGPSLength(true, "Customer Termination Notice period");
            txtOPCNoticePeriod.CheckGPSLength(true, "OPC Termination Notice period");
            txtForeClosureRate.CheckGPSLength(true, "Fore Closure Rate");
            txtBreakCost.CheckGPSLength(true, "Break Cost");
            txtOverdueRate.CheckGPSLength(true, "Overdue Rate");
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// Get and set Customer Details
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPriGetCustomerAddress(Int64 CustomerID)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "1");
            Procparam.Add("@Company_Id", intCompanyID.ToString());
            Procparam.Add("@Customer_ID", Convert.ToString(CustomerID));
            Procparam.Add("@Type_ID", "144");
            DataSet dsCustomer = Utility.GetDataset("S3G_Org_GETMRACMNLST", Procparam);
            DataTable dtCustomer = dsCustomer.Tables[0];
            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Text = txtCustomerCode.Text = Convert.ToString(dtCustomer.Rows[0]["Code"]);
                ucCustomerAddress.SetCustomerDetails(Convert.ToString(dtCustomer.Rows[0]["Code"]),
                        Convert.ToString(dtCustomer.Rows[0]["Address1"]) + "\n" +
                        ((Convert.ToString(dtCustomer.Rows[0]["Address2"]) == "") ? "" : Convert.ToString(dtCustomer.Rows[0]["Address2"]) + "\n") +
                Convert.ToString(dtCustomer.Rows[0]["city"]) + "\n" +
                Convert.ToString(dtCustomer.Rows[0]["state"]) + "\n" +
                Convert.ToString(dtCustomer.Rows[0]["country"]) + "\n" +
                Convert.ToString(dtCustomer.Rows[0]["pincode"]), Convert.ToString(dtCustomer.Rows[0]["Name"]), Convert.ToString(dtCustomer.Rows[0]["Telephone"]),
                Convert.ToString(dtCustomer.Rows[0]["mobile"]),
                Convert.ToString(dtCustomer.Rows[0]["email"]), Convert.ToString(dtCustomer.Rows[0]["website"]));
            }

            if (Convert.ToInt64(intMRAId) == 0 && dsCustomer.Tables.Count > 1)
            {
                if (dsCustomer.Tables[1].Rows.Count > 0)
                {
                    txtAcctmanager1.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Acct_Mngr1_Name"]);
                    txtAcctmanager2.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Acct_Mngr2_Name"]);
                    hdnAcctManager1ID.Value = Convert.ToString(dsCustomer.Tables[1].Rows[0]["ACC_Mngr1"]);
                    hdnAcctManager2ID.Value = Convert.ToString(dsCustomer.Tables[1].Rows[0]["ACC_Mngr2"]);
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// Clear control Values
    /// </summary>
    private void FunPriClearControls()
    {
        try
        {
            txtAcctmanager1.Text = txtAcctmanager2.Text = txtAmendmentDate.Text = txtAuthorizationDate.Text = txtAutoExtensionCond.Text =
            txtAutoExtensionTerm.Text = txtBreakCost.Text = txtClauseNumber.Text = txtCustomerCode.Text = txtCustomerNoticePeriod.Text =
            txtCustomertoOPC.Text = txtForeClosureRate.Text = txtInsuranceCond.Text = txtMRAEffectiveDate.Text = txtInterimRentRate.Text =
            txtOPCNoticePeriod.Text = txtOPCtoCustomer.Text = txtOverdueRate.Text = txtRegionalManager.Text = txtRemarks.Text = txtResolutionDate.Text =
            txtSignatoryDesignation.Text = txtSignatoryEmail.Text = txtSignatoryName.Text = txtSignatoryNumber.Text = txtTerminationCond.Text =
            txtJurisdictionCity.Text = string.Empty;

            ddlArbiterationClause.SelectedValue = ddlAuthorizationBasis.SelectedValue = ddlAutoExtension.SelectedValue = ddlBoardAuthorization.SelectedValue =
            ddlInterimCalculation.SelectedValue = ddlInterimRent.SelectedValue = ddlOPCNotice.SelectedValue = ddlSignedReceived.SelectedValue =
            ddlStandardFormat.SelectedValue = ddlJurisdiction.SelectedValue = ddlCustomerNtPrd.SelectedValue = "0";
            ddlMRAAmended.SelectedValue = "2";

            hdnAcctManager1ID.Value = hdnAcctManager2ID.Value = hdnRegionalMangerID.Value = string.Empty;

            ucCustomerCodeLov.FunPubClearControlValue();
            ucCustomerAddress.ClearCustomerDetails();
            txtInvoiceGraceDays.Text = string.Empty;
            ddlInvoiceGraceType.SelectedIndex = 1;
            ViewState["CustomerID"] = 0;

            lnkViewCustomer.Enabled = false;
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }

    /// <summary>
    /// Insert & Update MRA Details
    /// </summary>
    private void FunPriSaveMRADtls()
    {
        try
        {
            OrgMasterMgtServices.S3G_ORG_MRADataTable objMRADataTable = new OrgMasterMgtServices.S3G_ORG_MRADataTable();
            OrgMasterMgtServices.S3G_ORG_MRARow objMRADataRow = objMRADataTable.NewS3G_ORG_MRARow();

            objMRADataRow.MRA_ID = intMRAId;
            objMRADataRow.Account_Manager_1 = (Convert.ToString(hdnAcctManager1ID.Value) == "" || Convert.ToString(txtAcctmanager1.Text) == "") ? 0 : Convert.ToInt32(hdnAcctManager1ID.Value);
            objMRADataRow.Account_Manager_2 = (Convert.ToString(hdnAcctManager2ID.Value) == "" || Convert.ToString(txtAcctmanager2.Text) == "") ? 0 : Convert.ToInt32(hdnAcctManager2ID.Value);
            objMRADataRow.Arbitration_Clause = Convert.ToInt32(ddlArbiterationClause.SelectedValue);
            objMRADataRow.Authorization_Basis = Convert.ToInt32(ddlAuthorizationBasis.SelectedValue);
            if (Convert.ToString(txtAuthorizationDate.Text) != "")
            {
                objMRADataRow.Authorization_Date = Utility.StringToDate(txtAuthorizationDate.Text);
            }
            objMRADataRow.Auto_Extension_Conditions = Convert.ToString(txtAutoExtensionCond.Text).Trim();
            objMRADataRow.Auto_Extension_rental = Convert.ToInt32(ddlAutoExtension.SelectedValue);
            objMRADataRow.Auto_Extension_term = (Convert.ToString(txtAutoExtensionTerm.Text) == "") ? 0 : Convert.ToInt32(txtAutoExtensionTerm.Text);
            objMRADataRow.Board_Resolution = Convert.ToInt32(ddlBoardAuthorization.SelectedValue);
            if (Convert.ToString(txtResolutionDate.Text) != "")
            {
                objMRADataRow.Board_Resolution_Date = Utility.StringToDate(txtResolutionDate.Text);
            }
            objMRADataRow.Break_Cost = (Convert.ToString(txtBreakCost.Text) != "") ? Convert.ToDouble(txtBreakCost.Text) : 0;
            objMRADataRow.Clause_Number = Convert.ToString(txtClauseNumber.Text).Trim();
            objMRADataRow.Company_ID = Convert.ToInt32(intCompanyID);
            objMRADataRow.Created_By = Convert.ToInt32(intUserID);
            objMRADataRow.Created_On = Utility.StringToDate(DateTime.Now.ToString());
            objMRADataRow.Customer_ID = Convert.ToInt64(ViewState["CustomerID"]);
            objMRADataRow.Customer_Notice_Period = (Convert.ToString(txtCustomerNoticePeriod.Text) == "") ? 0 : Convert.ToInt32(txtCustomerNoticePeriod.Text);
            objMRADataRow.Customer_to_OPC = (Convert.ToString(txtCustomertoOPC.Text) == "") ? 0 : Convert.ToInt32(txtCustomertoOPC.Text);
            objMRADataRow.Foreclosure_Rate = Convert.ToDouble(txtForeClosureRate.Text);
            objMRADataRow.Insurance_Conditions = Convert.ToString(txtInsuranceCond.Text);
            objMRADataRow.Interim_Rent_Applicable = Convert.ToInt32(ddlInterimRent.SelectedValue);
            objMRADataRow.Interim_Rent_Basis = Convert.ToInt32(ddlInterimCalculation.SelectedValue);
            objMRADataRow.InterimRentRate = (Convert.ToString(txtInterimRentRate.Text) != "") ? Convert.ToDouble(txtInterimRentRate.Text) : 0;
            objMRADataRow.MRA_Amended = Convert.ToInt32(ddlMRAAmended.SelectedValue);
            objMRADataRow.MRA_Creation_Date = Utility.StringToDate(txtMRACreationDate.Text);
            if (Convert.ToString(txtMRAEffectiveDate.Text) != "")
            {
                objMRADataRow.MRA_Effective_Date = Utility.StringToDate(txtMRAEffectiveDate.Text);
            }
            objMRADataRow.MRA_Number = "";
            objMRADataRow.MRA_Status = 1;           //Pending
            objMRADataRow.OPC_Notice = Convert.ToInt32(ddlOPCNotice.SelectedValue);
            objMRADataRow.OPC_Notice_Period = (Convert.ToString(txtOPCNoticePeriod.Text) == "") ? 0 : Convert.ToInt32(txtOPCNoticePeriod.Text);
            objMRADataRow.OPC_to_Customer = (Convert.ToString(txtOPCtoCustomer.Text) == "") ? 0 : Convert.ToInt32(txtOPCtoCustomer.Text);
            objMRADataRow.Overdue_Rate = Convert.ToDouble(txtOverdueRate.Text);
            objMRADataRow.Regional_Manager = (Convert.ToString(hdnRegionalMangerID.Value) == "" || Convert.ToString(txtRegionalManager.Text) == "") ? 0 : Convert.ToInt32(hdnRegionalMangerID.Value);
            objMRADataRow.Remarks = Convert.ToString(txtRemarks.Text).Trim();
            objMRADataRow.Signatory_Designation = Convert.ToString(txtSignatoryDesignation.Text).Trim();
            objMRADataRow.Signatory_Email = Convert.ToString(txtSignatoryEmail.Text).Trim();
            objMRADataRow.Signatory_Name = Convert.ToString(txtSignatoryName.Text).Trim();
            objMRADataRow.Signatory_Number = Convert.ToString(txtSignatoryNumber.Text).Trim();
            objMRADataRow.SignedReceived = Convert.ToInt32(ddlSignedReceived.SelectedValue);
            objMRADataRow.Standard_Format = Convert.ToInt32(ddlStandardFormat.SelectedValue);
            objMRADataRow.Termination_Conditions = Convert.ToString(txtTerminationCond.Text).Trim();
            objMRADataRow.Invoice_Grace_Period_Type = Convert.ToInt32(ddlInvoiceGraceType.SelectedValue);//CR75
            objMRADataRow.Invoice_Grace_Period = Convert.ToInt32(txtInvoiceGraceDays.Text);//CR75
            if (Convert.ToString(txtAmendmentDate.Text) != "")
            {
                objMRADataRow.Amendment_Date = Utility.StringToDate(txtAmendmentDate.Text);
            }

            objMRADataRow.Jurisdictaion = Convert.ToInt32(ddlJurisdiction.SelectedValue);
            objMRADataRow.City = Convert.ToString(txtJurisdictionCity.Text);
            objMRADataRow.Lessee_Give_Notice = Convert.ToInt32(ddlCustomerNtPrd.SelectedValue);

            objMRADataTable.AddS3G_ORG_MRARow(objMRADataRow);

            ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            SerializationMode SMode = SerializationMode.Binary;

            string strMRANumber = string.Empty;
            int intMRA_No = 0;
            int intErrCode = ObjOrgMasterMgtServicesClient.FunPubCreateOrModifyMRACreation(out strMRANumber, out intMRA_No, SMode, ClsPubSerialize.Serialize(objMRADataTable, SMode));

            switch (intErrCode)
            {
                case 0:
                    if (intMRAId == 0)
                    {
                        strAlert = "MRA Number is " + strMRANumber + " Created successfully";
                        strAlert += @"\n\nWould you like to Create one more MRA?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPage = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        lblErrorMessage.Text = string.Empty;
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "MRA Details updated successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                        btnSave.Enabled = false;
                    }
                    break;
                case -1:
                    Utility.FunShowAlertMsg(this, "Document sequence number not defined for MRA Creation");
                    break;
                case -2:
                    Utility.FunShowAlertMsg(this, "Document sequence number exceeded for MRA Creation");
                    break;
                case 50:
                    Utility.FunShowAlertMsg(this, "Error in Saving Details");
                    break;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// Load MRA Details
    /// </summary>
    /// <param name="iMRAID"></param>
    private void FunPriLoadMRADtls(Int64 iMRAID)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@MRA_ID", Convert.ToString(intMRAId));
            DataTable dtMRA = Utility.GetDefaultData("S3G_ORG_GETMRADTLS", Procparam);

            if (dtMRA != null && dtMRA.Rows.Count > 0)
            {
                ViewState["CustomerID"] = Convert.ToString(dtMRA.Rows[0]["Customer_ID"]);
                FunPriGetCustomerAddress(Convert.ToInt64(dtMRA.Rows[0]["Customer_ID"]));

                txtAcctmanager1.Text = Convert.ToString(dtMRA.Rows[0]["AcctManager1_Name"]);
                txtAcctmanager2.Text = Convert.ToString(dtMRA.Rows[0]["AcctManager2_Name"]);
                txtAmendmentDate.Text = Convert.ToString(dtMRA.Rows[0]["Amendment_Date"]);
                txtAuthorizationDate.Text = Convert.ToString(dtMRA.Rows[0]["Authorization_Date"]);
                txtAutoExtensionCond.Text = Convert.ToString(dtMRA.Rows[0]["Auto_Extension_Conditions"]);
                txtAutoExtensionTerm.Text = Convert.ToString(dtMRA.Rows[0]["Auto_Extension_term"]);
                txtBreakCost.Text = Convert.ToString(dtMRA.Rows[0]["Break_Cost"]);
                txtClauseNumber.Text = Convert.ToString(dtMRA.Rows[0]["Clause_Number"]);
                txtCustomerNoticePeriod.Text = Convert.ToString(dtMRA.Rows[0]["Customer_Notice_Period"]);
                txtCustomertoOPC.Text = Convert.ToString(dtMRA.Rows[0]["Customer_to_OPC"]);
                txtForeClosureRate.Text = Convert.ToString(dtMRA.Rows[0]["Foreclosure_Rate"]);
                txtInsuranceCond.Text = Convert.ToString(dtMRA.Rows[0]["Insurance_Conditions"]);
                txtMRACreationDate.Text = Convert.ToString(dtMRA.Rows[0]["MRA_Creation_Date"]);
                txtMRAEffectiveDate.Text = Convert.ToString(dtMRA.Rows[0]["MRA_Effective_Date"]);
                txtOPCNoticePeriod.Text = Convert.ToString(dtMRA.Rows[0]["OPC_Notice_Period"]);
                txtOPCtoCustomer.Text = Convert.ToString(dtMRA.Rows[0]["OPC_to_Customer"]);
                txtOverdueRate.Text = Convert.ToString(dtMRA.Rows[0]["Overdue_Rate"]);
                txtRegionalManager.Text = Convert.ToString(dtMRA.Rows[0]["RegionalManager_Name"]);
                txtRemarks.Text = Convert.ToString(dtMRA.Rows[0]["Remarks"]);
                txtResolutionDate.Text = Convert.ToString(dtMRA.Rows[0]["Board_Resolution_Date"]);
                txtSignatoryDesignation.Text = Convert.ToString(dtMRA.Rows[0]["Signatory_Designation"]);
                txtSignatoryEmail.Text = Convert.ToString(dtMRA.Rows[0]["Signatory_Mail"]);
                txtSignatoryName.Text = Convert.ToString(dtMRA.Rows[0]["Signatory_Name"]);
                txtSignatoryNumber.Text = Convert.ToString(dtMRA.Rows[0]["Signatory_Contact_No"]);
                txtTerminationCond.Text = Convert.ToString(dtMRA.Rows[0]["Termination_Conditions"]);
                txtInterimRentRate.Text = (Convert.ToDouble(dtMRA.Rows[0]["Interim_Rent_Rate"]) > 0) ? Convert.ToString(dtMRA.Rows[0]["Interim_Rent_Rate"]) : "";
                txtJurisdictionCity.Text = Convert.ToString(dtMRA.Rows[0]["City"]);

                ddlInvoiceGraceType.SelectedValue = dtMRA.Rows[0]["INVOICE_GRACEPERIOD_TYPE"].ToString();
                txtInvoiceGraceDays.Text = dtMRA.Rows[0]["INVOICE_GRACE_PERIOD"].ToString();

                ddlArbiterationClause.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Arbitration_Clause"]);
                ddlAuthorizationBasis.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Authorization_Basis"]);
                ddlAutoExtension.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Auto_Extension_rental"]);
                ddlBoardAuthorization.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Board_Authorization"]);
                ddlInterimCalculation.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Interim_Rent_Calculation_Basis"]);
                ddlInterimRent.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Interim_Rent_Applicable"]);
                ddlMRAAmended.SelectedValue = Convert.ToString(dtMRA.Rows[0]["MRA_Amended"]);
                ddlOPCNotice.SelectedValue = Convert.ToString(dtMRA.Rows[0]["OPC_Notice"]);
                ddlSignedReceived.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Signed_Received"]);
                ddlStandardFormat.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Standard_Format"]);
                ddlCustomerNtPrd.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Lessee_Notice"]);
                ddlJurisdiction.SelectedValue = Convert.ToString(dtMRA.Rows[0]["Jurisdiction"]);

                hdnAcctManager1ID.Value = Convert.ToString(dtMRA.Rows[0]["AcctManager1ID"]);
                hdnAcctManager2ID.Value = Convert.ToString(dtMRA.Rows[0]["AcctManager2ID"]);
                hdnRegionalMangerID.Value = Convert.ToString(dtMRA.Rows[0]["RegionalManagerID"]);

                FunPriEnableDisableSignDtls(Convert.ToInt32(dtMRA.Rows[0]["Signed_Received"]));
                txtClauseNumber.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["Arbitration_Clause"]) == 1) ? true : false;
                txtAutoExtensionTerm.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["Auto_Extension_rental"]) == 1) ? true : false;
                txtAutoExtensionCond.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["Auto_Extension_rental"]) == 1) ? true : false;
                rfvClauseNumber.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["Arbitration_Clause"]) == 1) ? true : false;
                rfvAutoExtensionTerm.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["Auto_Extension_rental"]) == 1) ? true : false;
                lblAutoExtensionTerm.CssClass = (Convert.ToInt32(dtMRA.Rows[0]["Auto_Extension_rental"]) == 1) ? "styleReqFieldLabel" : "styleDisplayLabel";
                lblClauseNumber.CssClass = (Convert.ToInt32(dtMRA.Rows[0]["Arbitration_Clause"]) == 1) ? "styleReqFieldLabel" : "styleDisplayLabel";
                lblCity.CssClass = (Convert.ToInt32(dtMRA.Rows[0]["Jurisdiction"]) == 1) ? "styleReqFieldLabel" : "styleDisplayLabel";
                txtJurisdictionCity.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["Jurisdiction"]) == 1) ? true : false;
                rfvCity.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["Jurisdiction"]) == 1) ? true : false;
                lblLesseeNoD.CssClass = (Convert.ToInt32(dtMRA.Rows[0]["Lessee_Notice"]) == 1) ? "styleReqFieldLabel" : "styleDisplayLabel";
                txtCustomertoOPC.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["Lessee_Notice"]) == 1) ? true : false;
                lblAmendmentDate.CssClass = (Convert.ToInt32(dtMRA.Rows[0]["MRA_Amended"]) == 1) ? "styleReqFieldLabel" : "styleDisplayLabel";
                txtAmendmentDate.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["MRA_Amended"]) == 1) ? true : false;
                rfvAmendedDate.Enabled = (Convert.ToInt32(dtMRA.Rows[0]["MRA_Amended"]) == 1) ? true : false;
            }

        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    /// <summary>
    /// Disable Controls on Different mode
    /// </summary>
    private void FunPriDisableControls()
    {
        try
        {
            if (strMode == "C")
            {
                FunPriEnableDisableSignDtls(0);
                txtAutoExtensionTerm.Enabled = txtClauseNumber.Enabled = rfvClauseNumber.Enabled = rfvAutoExtensionTerm.Enabled = lnkViewCustomer.Enabled =
                txtOPCtoCustomer.Enabled = txtCustomertoOPC.Enabled = rfvOPCtoCustomer.Enabled = rfvCustomertoOPC.Enabled =
                ddlInterimCalculation.Enabled = txtInsuranceCond.Enabled = rfvInterimCalculation.Enabled = txtAutoExtensionCond.Enabled =
                txtInterimRentRate.Enabled = rfvInterimRentRate.Enabled = rfvAmendedDate.Enabled = rfvCity.Enabled = false;
            }
            else if (strMode == "M")
            {
                btnClear.Enabled = ucCustomerCodeLov.Visible = false;
                lnkViewCustomer.Enabled = true;
                if (Convert.ToInt32(ddlOPCNotice.SelectedValue) == 1)
                {
                    txtOPCtoCustomer.Enabled = txtCustomertoOPC.Enabled = true;
                    rfvOPCtoCustomer.Enabled = rfvCustomertoOPC.Enabled = true;
                    lblOPCtoCustomer.CssClass = lblCustomertoOPC.CssClass = "styleReqFieldLabel";
                }
                else
                {
                    txtOPCtoCustomer.Enabled = txtCustomertoOPC.Enabled = false;
                    rfvOPCtoCustomer.Enabled = rfvCustomertoOPC.Enabled = false;
                    lblOPCtoCustomer.CssClass = lblCustomertoOPC.CssClass = "styleDisplayLabel";
                }
                if (Convert.ToInt32(ddlInterimRent.SelectedValue) == 1)
                {
                    ddlInterimCalculation.Enabled = txtInterimRentRate.Enabled =
                    rfvInterimCalculation.Enabled = rfvInterimRentRate.Enabled = false;
                    txtInsuranceCond.Enabled = true;
                    //lblInterimCalculation.CssClass = lblInterimRentRate.CssClass = "styleReqFieldLabel";
                }
                else
                {
                    ddlInterimCalculation.Enabled = txtInsuranceCond.Enabled = txtInterimRentRate.Enabled =
                    rfvInterimCalculation.Enabled = rfvInterimRentRate.Enabled = false;
                    lblInterimCalculation.CssClass = lblInterimRentRate.CssClass = "styleDisplayLabel";
                }
            }
            else if (strMode == "Q")
            {
                txtAcctmanager1.ReadOnly = txtAcctmanager2.ReadOnly = txtAmendmentDate.ReadOnly = txtAuthorizationDate.ReadOnly = txtAutoExtensionCond.ReadOnly =
                txtAutoExtensionTerm.ReadOnly = txtBreakCost.ReadOnly = txtClauseNumber.ReadOnly = txtCustomerCode.ReadOnly = txtCustomerNoticePeriod.ReadOnly =
                txtCustomertoOPC.ReadOnly = txtForeClosureRate.ReadOnly = txtInsuranceCond.ReadOnly = txtMRAEffectiveDate.ReadOnly = txtInterimRentRate.ReadOnly =
                txtOPCNoticePeriod.ReadOnly = txtOPCtoCustomer.ReadOnly = txtOverdueRate.ReadOnly = txtRegionalManager.ReadOnly = txtRemarks.ReadOnly =
                txtResolutionDate.ReadOnly = txtSignatoryDesignation.ReadOnly = txtSignatoryEmail.ReadOnly = txtSignatoryName.ReadOnly = txtSignatoryNumber.ReadOnly =
                txtTerminationCond.ReadOnly = lnkViewCustomer.Enabled = true;

                ddlArbiterationClause.ClearDropDownList();
                ddlAuthorizationBasis.ClearDropDownList(); ddlAutoExtension.ClearDropDownList(); ddlBoardAuthorization.ClearDropDownList();
                ddlInterimCalculation.ClearDropDownList(); ddlInterimRent.ClearDropDownList(); ddlMRAAmended.ClearDropDownList(); ddlOPCNotice.ClearDropDownList();
                ddlSignedReceived.ClearDropDownList(); ddlStandardFormat.ClearDropDownList(); ddlCustomerNtPrd.ClearDropDownList();
                ddlJurisdiction.ClearDropDownList();
                ddlInvoiceGraceType.ClearDropDownList();
                txtInvoiceGraceDays.ReadOnly = true;
                btnSave.Enabled = btnClear.Enabled = ucCustomerCodeLov.Visible = lblCode.Visible = false;
                ceAmendmentDate.Enabled = ceAuthorizationDate.Enabled = ceMRAEffectiveDate.Enabled = ceResolutionDate.Enabled = false;

                if (Request.QueryString["Popup"] != null)
                {
                    lnkViewCustomer.Visible = btnCancel.Enabled = false;
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnableDisableSignDtls(Int32 _iValue)
    {
        try
        {
            if (_iValue == 1)
            {
                txtSignatoryDesignation.Enabled = txtSignatoryName.Enabled = txtSignatoryNumber.Enabled =
                txtSignatoryEmail.Enabled = true;
                // rfvSignatoryName.Enabled = rfvSignatoryDesignation.Enabled = rfvSignatoryNumber.Enabled = true;

            }
            else
            {
                txtSignatoryDesignation.Enabled = txtSignatoryName.Enabled = txtSignatoryNumber.Enabled =
                txtSignatoryEmail.Enabled = false;
                //rfvSignatoryName.Enabled = rfvSignatoryDesignation.Enabled = rfvSignatoryNumber.Enabled = false;

            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }


    #endregion

    [System.Web.Services.WebMethod]
    public static string[] GetSalesPersonList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Get_Userlist_AGT", Procparam));

        return suggetions.ToArray();
    }

}