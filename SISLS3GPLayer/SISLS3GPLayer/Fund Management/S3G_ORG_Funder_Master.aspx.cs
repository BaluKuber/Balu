
#region Namespace

using System;
using System.Web;
using System.Data;
using System.Text;
using S3GBusEntity.FundManagement;
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

#endregion

public partial class Origination_S3G_ORG_Funder_Master : ApplyThemeForProject
{
    #region Common Variable declaration

    FunderMgtServiceReference.FundMgtServiceClient objFundMgtServiceClient;
    FundMgtServices.S3G_Funder_MasterDataTable objFunderDatatable;
    FundMgtServices.S3G_Funder_MasterRow objFunderRow;
    S3GBusEntity.FundManagement.FundMgtServices.S3G_FND_SanctionDtlDataTable ObjSanc_DT;
    S3GBusEntity.FundManagement.FundMgtServices.S3G_FND_SanctionDtlRow ObjRow;

    int intCompanyID, intUserID = 0;
    string strMode = string.Empty;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    Int64 intFunderId;
    Int64 intNoteID = 0;
    Int64 intCustomerID = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string strDateFormat = string.Empty;
    static string strModifyAccountNo = string.Empty;
    //string strMode = string.Empty;
    static string strPageName = "Funder Master Creation";
    static string strSuffix = "";
    FormsAuthenticationTicket Ticket;
    public static Origination_S3G_ORG_Funder_Master obj_Page;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Fund Management/S3G_FUNDMGT_FunderMaster_View.aspx";
    string strRedirectPageAdd = "window.location.href='../Fund Management/S3G_ORG_Funder_Master.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../Fund Management/S3G_FUNDMGT_FunderMaster_View.aspx';";
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

    #region "Page Variables"

    //string[] arrSortCol = new string[] { "PODTL.PO_Number" };
    int intNoofSearch = 1;
    ArrayList arrSearchVal = new ArrayList(1);
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

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGrid();
    }

    #region "Summary paging"

    PagingValues ObjPagingSummary = new PagingValues();

    public delegate void PageAssignValueSummary(int ProPageNumRWSummary, int intPageSizeSummary);
    /// <summary>
    ///Assign Page Number
    /// </summary
    public int ProPageNumRWSummary
    {
        get;
        set;
    }
    /// <summary>
    ///Assign Page Size
    /// </summary
    public int ProPageSizeRWSummary
    {
        get;
        set;

    }

    protected void AssignValueSummary(int intPageNum, int intPageSize)
    {
        ProPageNumRWSummary = intPageNum;
        ProPageSizeRWSummary = intPageSize;
        FunPriLoadFundRSSancDtl();
    }

    #endregion

    #endregion

    #endregion

    #region "EVENTS"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            S3GSession ObjS3GSession = null;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            lblHeading.Text = "Funder Limit Management Master";

            //Date Format
            ObjS3GSession = new S3GSession();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            strPrefixLength = ObjS3GSession.ProGpsPrefixRW;
            strDecMaxLength = ObjS3GSession.ProGpsSuffixRW;

            CalendarExtender2.Format = strDateFormat;
            CalendarExtender3.Format = strDateFormat;
            
            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyID.ToString();

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

            //Summary paging
            ProPageNumRWSummary = 1;
            TextBox txtPageSizeSummary = (TextBox)ucCustomPagingSummary.FindControl("txtPageSize");
            if (txtPageSizeSummary.Text != "")
                ProPageSizeRWSummary = Convert.ToInt32(txtPageSizeSummary.Text);
            else
                ProPageSizeRWSummary = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValueSummary objSummary = new PageAssignValueSummary(this.AssignValueSummary);
            ucCustomPagingSummary.callback = objSummary;
            ucCustomPagingSummary.ProPageNumRW = ProPageNumRWSummary;
            ucCustomPagingSummary.ProPageSizeRW = ProPageSizeRWSummary;

            #endregion

            if (Request.QueryString["qsFunderId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsFunderId"));
                if (fromTicket != null)
                    intFunderId = Convert.ToInt64(fromTicket.Name);
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];

                if (Request.QueryString["qsCustomerId"] != null)
                {
                    FormsAuthenticationTicket Lessee = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCustomerId"));
                    if (Lessee != null)
                        intCustomerID = Convert.ToInt64(Lessee.Name);
                }
            }
            else if (Request.QueryString["qsNoteID"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsNoteID"));
                if (fromTicket != null)
                    intNoteID = Convert.ToInt64(fromTicket.Name);
                strMode = Request.QueryString["qsMode"];
            }
            else
            {
                strMode = Request.QueryString["qsMode"];
            }
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;

            System.Web.HttpContext.Current.Session["AutoSuggestFunderID"] = intFunderId.ToString();

            TxtSGSTRegDate.Attributes.Add("onblur", "fnDoDate(this,'" + TxtSGSTRegDate.ClientID + "','" + strDateFormat + "',true,  false);");
            TxtCGSTRegDate.Attributes.Add("onblur", "fnDoDate(this,'" + TxtCGSTRegDate.ClientID + "','" + strDateFormat + "',true,  false);");
           

            if (!IsPostBack)
            {
                FunPriSetPrefixSuffixLength();
                FunPriLoadLov();
                txtNetDiscountRate.Attributes.Add("readonly", "readonly");

                if (strMode == "M")                     //Modify Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunPriEnableDisableControls();
                    FunPriLoadFunderDtls();
                    FunPriLoadFundRSSancDtl();
                }
                else if (strMode == "Q")                //Query Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FunPriEnableDisableControls();
                    if (intNoteID > 0)
                    {
                        FunPriLoadNoteSancDtl();
                        lblLesseeName.Visible = ddlLesseeName.Visible = btnLesseeSearch.Visible = ucCustomPaging.Visible =
/*added by vinodha.m for the below controls to hide when view sanction button is clicked from note creation form*/
                        chkZeroRcrd.Visible = lblviewall.Visible = false;
/*added by vinodha.m for the below controls to hide when view sanction button is clicked from note creation form*/
                    }
                    else if (intCustomerID > 0)
                    {
                        FunPriLoadApvdSanc();
                        lblLesseeName.Visible = ddlLesseeName.Visible = btnLesseeSearch.Visible = ucCustomPaging.Visible = false;                        
                    }
                    else
                    {
                        FunPriLoadFunderDtls();
                        ddlGLAccount.ClearDropDownList();
                        FunPriDisableFooter();
                        FunPriLoadFundRSSancDtl();
                    }
                }
                else                                   //Create Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    //FunPriBindGridDtls(2);
                    FunPriEnableDisableControls();
                    FunPriBindGrid();
                }
            }
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #region "Button Events"

    protected void btnBankAdd_Click(object sender, EventArgs e)
    {
        try
        {
            pnlBankDtls.Visible = true;
            DataTable dtBankDtl = new DataTable();
            DataRow drbank;
            if (ViewState["BankDetails"] == null)
            {
                dtBankDtl.Columns.Add("Bank_ID");
                dtBankDtl.Columns.Add("Bank_Name");
                dtBankDtl.Columns.Add("Account_Type_ID");
                dtBankDtl.Columns.Add("Account_Type_Desc");
                dtBankDtl.Columns.Add("Benificiary_name");
                dtBankDtl.Columns.Add("Account_No");
                dtBankDtl.Columns.Add("City");
                dtBankDtl.Columns.Add("Branch");
                dtBankDtl.Columns.Add("MICR_Code");
                dtBankDtl.Columns.Add("IFSC");
                dtBankDtl.Columns.Add("Is_Transaction");
                dtBankDtl.Columns.Add("Is_DefaultAccount");
                dtBankDtl.Columns.Add("IS_Modify");
                dtBankDtl.Columns.Add("Is_Active");

                drbank = dtBankDtl.NewRow();
                drbank["Bank_ID"] = "0";
                drbank["Bank_Name"] = Convert.ToString(txtBankName.Text).Trim();
                drbank["Account_Type_ID"] = Convert.ToString(ddlAccountType.SelectedValue).Trim();
                drbank["Account_Type_Desc"] = Convert.ToString(ddlAccountType.SelectedItem.Text).Trim();
                drbank["Benificiary_name"] = Convert.ToString(txtBenificiaryName.Text).Trim();
                drbank["Account_No"] = Convert.ToString(txtAccountNo.Text).Trim();
                drbank["City"] = Convert.ToString(txtBankCity.Text).Trim();
                drbank["Branch"] = Convert.ToString(txtBankBranch.Text).Trim();
                drbank["MICR_Code"] = Convert.ToString(txtMICRCode.Text).Trim();
                drbank["IFSC"] = Convert.ToString(txtIFSCCode.Text).Trim();
                drbank["Is_Transaction"] = "0";
                drbank["Is_DefaultAccount"] = (chkDefaultAccount.Checked == true) ? 1 : 0;
                drbank["Is_Active"] = (chkActiveAccount.Checked == true) ? 1 : 0;
                drbank["IS_Modify"] = "0";

                dtBankDtl.Rows.Add(drbank);
            }
            else
            {
                dtBankDtl = ((DataTable)ViewState["BankDetails"]).Copy();
                if (dtBankDtl.Rows.Count > 0)
                {
                    int qryExists = (from BankDetails in dtBankDtl.AsEnumerable()
                                     where (BankDetails.Field<string>("Account_No") == (Convert.ToString(txtAccountNo.Text).Trim()))
                                     select BankDetails).Count();
                    if (qryExists > 0)
                    {
                        Utility.FunShowAlertMsg(this, "Beneficiary Account No already exists");
                        return;
                    }
                }

                drbank = dtBankDtl.NewRow();
                drbank["Bank_ID"] = "0";
                drbank["Bank_Name"] = Convert.ToString(txtBankName.Text).Trim();
                drbank["Account_Type_ID"] = Convert.ToString(ddlAccountType.SelectedValue).Trim();
                drbank["Account_Type_Desc"] = Convert.ToString(ddlAccountType.SelectedItem.Text).Trim();
                drbank["Benificiary_name"] = Convert.ToString(txtBenificiaryName.Text).Trim();
                drbank["Account_No"] = Convert.ToString(txtAccountNo.Text).Trim();
                drbank["City"] = Convert.ToString(txtBankCity.Text).Trim();
                drbank["Branch"] = Convert.ToString(txtBankBranch.Text).Trim();
                drbank["MICR_Code"] = Convert.ToString(txtMICRCode.Text).Trim();
                drbank["IFSC"] = Convert.ToString(txtIFSCCode.Text).Trim();
                drbank["Is_Transaction"] = "0";
                drbank["Is_DefaultAccount"] = (chkDefaultAccount.Checked == true) ? 1 : 0;
                drbank["Is_Active"] = (chkActiveAccount.Checked == true) ? 1 : 0;
                drbank["IS_Modify"] = "0";

                dtBankDtl.Rows.Add(drbank);
            }

            DataRow[] dr = dtBankDtl.Select("Is_DefaultAccount=1");
            if (dr.Length > 1)
            {
                Utility.FunShowAlertMsg(this, "Default Account should not be greater than one");
                return;
            }

            ViewState["BankDetails"] = dtBankDtl;
            FunPriBindGridDtls(1);
            FunPriClearBankDtls();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnBankModify_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtBankDtl = (DataTable)ViewState["BankDetails"];

            if (dtBankDtl.Rows.Count > 0)
            {
                int qryExists = (from BankDetails in dtBankDtl.AsEnumerable()
                                 where (BankDetails.Field<string>("Account_No") == (Convert.ToString(txtAccountNo.Text).Trim()))
                                 select BankDetails).Count();
                if (qryExists > 0 && strModifyAccountNo != Convert.ToString(txtAccountNo.Text).Trim())
                {
                    Utility.FunShowAlertMsg(this, "Beneficiary Account No already exists");
                    return;
                }
            }

            string strFilter = "Account_No = '" + Convert.ToString(strModifyAccountNo).Trim() + "'";

            DataRow[] drbank = dtBankDtl.Select(strFilter);
            drbank[0]["Bank_Name"] = Convert.ToString(txtBankName.Text).Trim();
            drbank[0]["Account_Type_ID"] = Convert.ToString(ddlAccountType.SelectedValue).Trim();
            drbank[0]["Account_Type_Desc"] = Convert.ToString(ddlAccountType.SelectedItem.Text).Trim();
            drbank[0]["Benificiary_name"] = Convert.ToString(txtBenificiaryName.Text).Trim();
            drbank[0]["Account_No"] = Convert.ToString(txtAccountNo.Text).Trim();
            drbank[0]["City"] = Convert.ToString(txtBankCity.Text).Trim();
            drbank[0]["Branch"] = Convert.ToString(txtBankBranch.Text).Trim();
            drbank[0]["MICR_Code"] = Convert.ToString(txtMICRCode.Text).Trim();
            drbank[0]["IFSC"] = Convert.ToString(txtIFSCCode.Text).Trim();
            drbank[0]["Is_DefaultAccount"] = (chkDefaultAccount.Checked == true) ? 1 : 0;
            drbank[0]["Is_Active"] = (chkActiveAccount.Checked == true) ? 1 : 0;
            drbank[0]["IS_Modify"] = "1";

            dtBankDtl.AcceptChanges();

            DataRow[] dr = dtBankDtl.Select("Is_DefaultAccount=1");
            if (dr.Length > 1)
            {
                Utility.FunShowAlertMsg(this, "Default Account should not be greater than one");
                return;
            }

            ViewState["BankDetails"] = dtBankDtl;
            FunPriBindGridDtls(1);
            FunPriClearBankDtls();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearDtls();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@OPTION", "10");
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            DataTable dtSancDtl = Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam);
        }
        catch (Exception ObjException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ObjException, strPageName);
        }
    }

    protected void btnLesseeAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDouble(txtDiscountRate.Text) > 100)
            {
                cvLessee.ErrorMessage = "Discount Rate should be less than or equal to 100";
                cvLessee.IsValid = false;
                return;
            }

            ViewState["IsNewAddtionalInfo"] = 1;
            moeAdditionalInfo.Hide();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnLesseeClose_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["IsNewAddtionalInfo"] = (Convert.ToString(txtTenor.Text) == "") ? 0 : 1;
            if (Convert.ToInt32(ViewState["IsNewAddtionalInfo"]) == 0)
            {
                FunPriClearLesseeDtls();
            }
            moeAdditionalInfo.Hide();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnLesseeUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            #region "Hide"
            /*
            if (ViewState["LesseeSettings"] != null)
            {
                DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
                string strFilter = "Sanction_Ref_No = '" + Convert.ToString(lblSearchSanctionNo.Text).Trim() + "' and Customer_ID = " + Convert.ToInt64(lblSearchCustomerID.Text) + " and Asset_Category_ID = " + Convert.ToInt64(lblEditAssetID.Text);

                DataRow[] drLessee = dtLessee.Select(strFilter);

                drLessee[0]["End_Customer_Name"] = Convert.ToString(ddlEndCustomer.SelectedText).Trim();
                drLessee[0]["End_Customer_ID"] = Convert.ToInt64(ddlEndCustomer.SelectedValue);
                drLessee[0]["PV_Calculation_Method_ID"] = Convert.ToInt32(ddlPVCalcMethod.SelectedValue);
                drLessee[0]["PV_Calculation_Desc"] = Convert.ToString(ddlPVCalcMethod.SelectedItem.Text);
                drLessee[0]["Discount_Rate"] = (Convert.ToString(txtDiscountRate.Text).Trim() != "") ? Convert.ToDouble(txtDiscountRate.Text) : 0;
                drLessee[0]["Processing_Fee_Perc"] = (Convert.ToString(txtProcessingFeePerc.Text).Trim() != "") ? Convert.ToDouble(txtProcessingFeePerc.Text) : 0;
                drLessee[0]["Processing_Fee"] = (Convert.ToString(txtProcessingFee.Text).Trim() != "") ? Convert.ToDouble(txtProcessingFee.Text) : 0;
                drLessee[0]["ForeClosure_Rate"] = (Convert.ToString(txtForeClosureRate.Text).Trim() != "") ? Convert.ToDouble(txtForeClosureRate.Text) : 0;
                drLessee[0]["Misc_Charges"] = (Convert.ToString(txtMiscCharges.Text).Trim() != "") ? Convert.ToDouble(txtMiscCharges.Text) : 0;
                drLessee[0]["Tenor"] = (Convert.ToString(txtTenor.Text).Trim() != "") ? Convert.ToInt32(txtTenor.Text) : 0;
                drLessee[0]["Over_Due_Rate"] = (Convert.ToString(txtOverDueRate.Text).Trim() != "") ? Convert.ToDouble(txtOverDueRate.Text) : 0;
                drLessee[0]["Collateral_Dtls"] = Convert.ToString(txtCollateralDetails.Text).Trim();
                drLessee[0]["Remarks"] = Convert.ToString(txtRemarks.Text).Trim();
                drLessee[0]["Cheque_Return_Charges"] = (Convert.ToString(txtChqRtnChrgs.Text).Trim() != "") ? Convert.ToDouble(txtChqRtnChrgs.Text) : 0;
                drLessee[0]["PF_Service_Tax"] = (Convert.ToString(txtPFServiceTax.Text).Trim() != "") ? Convert.ToDouble(txtPFServiceTax.Text) : 0;
                drLessee[0]["Total_PF"] = (Convert.ToString(txtTotalPF.Text).Trim() != "") ? Convert.ToDouble(txtTotalPF.Text) : 0;
                drLessee[0]["Is_Modified"] = 1;
                dtLessee.AcceptChanges();
                ViewState["LesseeSettings"] = dtLessee;
                FunPriClearLesseeDtls();
                moeAdditionalInfo.Hide();
            }
            */
            #endregion

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "3");
            Procparam.Add("@PV_Method_ID", Convert.ToString(ddlPVCalcMethod.SelectedValue));
            Procparam.Add("@Discount_Rate", (Convert.ToString(txtDiscountRate.Text).Trim() != "") ? Convert.ToString(txtDiscountRate.Text) : "0");
            Procparam.Add("@Processing_Fee_Perc", (Convert.ToString(txtProcessingFeePerc.Text).Trim() != "") ? Convert.ToString(txtProcessingFeePerc.Text) : "0");
            Procparam.Add("@Processing_Fee_Amt", (Convert.ToString(txtProcessingFee.Text).Trim() != "") ? Convert.ToString(txtProcessingFee.Text) : "0");
            Procparam.Add("@Processing_Fee_ST", (Convert.ToString(txtPFServiceTax.Text).Trim() != "") ? Convert.ToString(txtPFServiceTax.Text) : "0");
            Procparam.Add("@Foreclosure_Rate", (Convert.ToString(txtForeClosureRate.Text).Trim() != "") ? Convert.ToString(txtForeClosureRate.Text) : "0");
            Procparam.Add("@Misc_Charges", (Convert.ToString(txtMiscCharges.Text).Trim() != "") ? Convert.ToString(txtMiscCharges.Text) : "0");
            Procparam.Add("@Tenure", (Convert.ToString(txtTenor.Text).Trim() != "") ? Convert.ToString(txtTenor.Text) : "0");
            Procparam.Add("@ODI_Rate", (Convert.ToString(txtOverDueRate.Text).Trim() != "") ? Convert.ToString(txtOverDueRate.Text) : "0");
            Procparam.Add("@Enduser_ID", Convert.ToString(ddlEndCustomer.SelectedValue));
            Procparam.Add("@Cheque_Rtn_Charge", (Convert.ToString(txtChqRtnChrgs.Text).Trim() != "") ? Convert.ToString(txtChqRtnChrgs.Text) : "0");
            Procparam.Add("@Collateral_Details", Convert.ToString(txtCollateralDetails.Text).Trim());
            Procparam.Add("@Remarks", Convert.ToString(txtRemarks.Text).Trim());
            Procparam.Add("@Funder_Detail_ID", Convert.ToString(lblSrchFndrDtlID.Text));
            Procparam.Add("@Sanction_No", Convert.ToString(lblSearchSanctionNo.Text).Trim());
            Procparam.Add("@Customer_ID", Convert.ToString(lblSearchCustomerID.Text));
            Procparam.Add("@Asset_Category_ID", Convert.ToString(lblEditAssetID.Text));
            Procparam.Add("@Funder_ID", Convert.ToString(intFunderId));
            Procparam.Add("@Created_By", Convert.ToString(intUserID));
            Procparam.Add("@Location_ID", Convert.ToString(txtCorpState.SelectedValue));

            if (txtUpfrontInt.Text != "")
                Procparam.Add("@Upfront_Int", Convert.ToString(txtUpfrontInt.Text));
            Procparam.Add("@Net_Discount_Rate", Convert.ToString(txtNetDiscountRate.Text));
            Procparam.Add("@Discount_Processing", Convert.ToString(chkDiscountProcessing.Checked));
            Procparam.Add("@Discount_Upfront", Convert.ToString(chkUpfront.Checked));

            DataTable dtSancDtl = Utility.GetDefaultData("S3G_FMGT_INSERT_TMPSANCDTL", Procparam);
            FunPriClearLesseeDtls();
            moeAdditionalInfo.Hide();
            FunPriBindGrid();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strValidationMsg = FunPriValidateAddress();
            if (strValidationMsg != "")
            {
                strValidationMsg = "Please Correct the below validations: <br>" + strValidationMsg;
                cvFunder.ErrorMessage = strValidationMsg;
                cvFunder.IsValid = false;
                return;
            }

            if (ViewState["LesseeSettings"] != null && Convert.ToString(((DataTable)ViewState["LesseeSettings"]).Rows[0]["Sanction_Ref_No"]) != "")
            {
                DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
                DataView dvLessee = new DataView(dtLessee);
                dvLessee.RowFilter = "Lessee_Funder_ID = 0 and Document_Upload <> ''";
                dtLessee = dvLessee.ToTable();
                if (dtLessee.Rows.Count > 0)
                {
                    string strpath = string.Empty;
                    if (ViewState["DocumentationPath"] != null || Convert.ToString(ViewState["DocumentationPath"]) != "")
                    {
                        strpath = Convert.ToString(ViewState["DocumentationPath"]);
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Document path not available to Upload");
                        return;
                    }
                    FunProUploadFiles(dtLessee, strpath);
                }
            }

            TextBox txtCountry = txtCommCountry.FindControl("TextBox") as TextBox;
            objFunderDatatable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_Funder_MasterDataTable();
            objFunderRow = objFunderDatatable.NewS3G_Funder_MasterRow();

            objFunderRow.Company_ID = intCompanyID;
            objFunderRow.Created_By = intUserID;
            objFunderRow.Created_On = System.DateTime.Now;
            objFunderRow.Funder_Code = Convert.ToString(txtFunderCode.Text).Trim();
            objFunderRow.Funder_ID = intFunderId;

            objFunderRow.Comm_Address1 = Convert.ToString(txtCommAdress.Text).Trim();
            objFunderRow.Comm_City = Convert.ToString(txtCommCity.Text).Trim();
            objFunderRow.Comm_Country = Convert.ToString(txtCountry.Text).Trim();
            objFunderRow.Comm_City = Convert.ToString(txtCommCity.Text).Trim();
            objFunderRow.Comm_EMail = Convert.ToString(txtCommEmailID.Text).Trim();
            objFunderRow.Comm_Mobile = Convert.ToString(txtCommMobileNo.Text).Trim();
            objFunderRow.Comm_Pincode = Convert.ToString(txtCommPincode.Text).Trim();
            objFunderRow.Comm_State = Convert.ToString(txtCommState.SelectedValue);
            objFunderRow.Comm_TAN = Convert.ToString(txtCommTAN.Text).Trim();
            objFunderRow.Comm_Telephone = Convert.ToString(txtCommTelephoneNo.Text).Trim();
            objFunderRow.Comm_TIN = Convert.ToString(txtCommTIN.Text).Trim();
            objFunderRow.GL_Code = Convert.ToInt32(ddlGLAccount.SelectedValue);
            objFunderRow.Funder_Name = Convert.ToString(txtFunderName.Text).Trim();
            objFunderRow.Perm_Address1 = Convert.ToString(txtCorpAddress.Text).Trim();
            objFunderRow.Perm_City = Convert.ToString(txtCorpCity.Text).Trim();
            objFunderRow.Perm_Country = Convert.ToString(txtCorpCountry.SelectedItem.Text).Trim();
            objFunderRow.Perm_EMail = Convert.ToString(txtCorpEmailID.Text).Trim();
            objFunderRow.Perm_Mobile = Convert.ToString(txtCorpMobileNo.Text).Trim();
            objFunderRow.Perm_Pincode = Convert.ToString(txtCorpPincode.Text).Trim();
            objFunderRow.Perm_State = Convert.ToString(txtCorpState.SelectedValue);
            objFunderRow.Perm_TAN = Convert.ToString(txtCorpTAN.Text).Trim();
            objFunderRow.Perm_Telephone = Convert.ToString(txtCorpTelephoneNo.Text).Trim();
            objFunderRow.Perm_TIN = Convert.ToString(txtCorpTIN.Text).Trim();
            objFunderRow.Remarks = Convert.ToString(txtNote.Text).Trim();
            objFunderRow.Status_ID = 0;

            objFunderRow.CGSTIN = Convert.ToString(txtCGSTin.Text).Trim();
            if (TxtCGSTRegDate.Text != "")
                objFunderRow.CGST_Reg_Date = Utility.StringToDate(TxtCGSTRegDate.Text.ToString());
            else
                objFunderRow.CGST_Reg_Date = Utility.StringToDate("1/1/0001");

            objFunderRow.SGSTIN = Convert.ToString(txtSGSTin.Text).Trim();

            if (TxtCGSTRegDate.Text != "")
                objFunderRow.SGST_Reg_Date = Utility.StringToDate(TxtSGSTRegDate.Text.ToString());
            else
                objFunderRow.SGST_Reg_Date = Utility.StringToDate("1/1/0001");

            if (ViewState["BankDetails"] != null && ((DataTable)ViewState["BankDetails"]).Rows.Count > 0)
            {
                //objFunderRow.XML_BankDtl = Utility.FunPubFormXml((DataTable)ViewState["BankDetails"], true);
                objFunderRow.XML_BankDtl = FunPriFormXMLBankDtl();
            }
            else
            {
                objFunderRow.XML_BankDtl = null;
            }
            objFunderRow.XML_CustomerDtl = null;
            //if (ViewState["LesseeSettings"] != null && Convert.ToString(((DataTable)ViewState["LesseeSettings"]).Rows[0]["Sanction_Ref_No"]) != "")
            //{
            //    objFunderRow.XML_CustomerDtl = Utility.FunPubFormXml((DataTable)ViewState["LesseeSettings"], true);
            //}
            //else
            //{
            //    objFunderRow.XML_CustomerDtl = null;
            //}

            if (ViewState["LimitConsolidationDetails"] != null && Convert.ToInt32(((DataTable)ViewState["LimitConsolidationDetails"]).Rows[0]["Existing_sanction_ID"]) > 0)
            {
                objFunderRow.XML_LimitConslidation = Utility.FunPubFormXml((DataTable)ViewState["LimitConsolidationDetails"], true);
            }
            else
            {
                objFunderRow.XML_LimitConslidation = null;
            }

            objFunderRow.Corp_PAN = Convert.ToString(txtCorpPAN.Text);
            objFunderRow.Short_Name = Convert.ToString(txtShortName.Text);

            objFunderDatatable.AddS3G_Funder_MasterRow(objFunderRow);

            objFundMgtServiceClient = new FunderMgtServiceReference.FundMgtServiceClient();
            string strFunderCode, strErrorMsg = string.Empty;
            Int64 intFunderNo = 0;

            int iErrorCode = objFundMgtServiceClient.FunPubCreateOrModifyFunderMaster(out strFunderCode, out intFunderNo, out strErrorMsg, ObjSerMode, ClsPubSerialize.Serialize(objFunderDatatable, ObjSerMode));
            switch (iErrorCode)
            {
                case 0:
                    btnSave.Enabled = false;
                    if (intFunderId == 0)
                    {
                        strAlert = "Funder Code is " + strFunderCode + " Created successfully";
                        strAlert += @"\n\nWould you like to Create one more Funder Limit Management Master?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPage = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        lblErrorMessage.Text = string.Empty;
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Funder Limit Management Master Details updated successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                        btnSave.Enabled = false;
                    }
                    break;
                case -1:
                    Utility.FunShowAlertMsg(this, "Document sequence number not defined for Funder Limit Management Master Creation");
                    break;
                case -2:
                    Utility.FunShowAlertMsg(this, "Document sequence number exceeded for Funder Limit Management Master Creation");
                    break;
                case -3:
                    Utility.FunShowAlertMsg(this, "Communication GSTIN exists for other Funders");
                    break;
                case -4:
                    Utility.FunShowAlertMsg(this, "Corporate GSTIN exists for other Funders");
                    break;
                case 10:
                    Utility.FunShowAlertMsg(this, "Short Name already Exists");
                    break;
                case 20:
                    Utility.FunShowAlertMsg(this, "Short Name Cant be Modify after Note Creation");
                    break;
                case 50:
                    Utility.FunShowAlertMsg(this, "Error in Saving Details");
                    break;
                case 51:
                    Utility.FunShowAlertMsg(this, "Bank Account Number Already Mapped in Live Note, Contact Admin.");
                    break;
                case 52:
                    Utility.FunShowAlertMsg(this, "Bank Account Number Already Mapped in Live Note, Contact Admin.");
                    break;
            }
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            FunProClearCachedFiles();
            Response.Redirect(strRedirectPage);
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnIncludeConsolidation_Click(object sender, EventArgs e)
    {
        try
        {
            UserControls_S3GAutoSuggest ddlSanctionNo = (UserControls_S3GAutoSuggest)grvLimitConsolidation.FooterRow.FindControl("ddlSanctionNo");
            Label lblExistBalanceLimit = (Label)grvLimitConsolidation.FooterRow.FindControl("lblExistBalanceLimit");
            TextBox txtTransferAmount = (TextBox)grvLimitConsolidation.FooterRow.FindControl("txtTransferAmount");
            UserControls_S3GAutoSuggest ddlNewSanctionNo = (UserControls_S3GAutoSuggest)grvLimitConsolidation.FooterRow.FindControl("ddlNewSanctionNo");

            if (Convert.ToInt64(ddlSanctionNo.SelectedValue) == Convert.ToInt64(ddlNewSanctionNo.SelectedValue))
            {
                Utility.FunShowAlertMsg(this, "Existing Sanction No and New Sanction No should not be same");
                return;
            }
            else if (Convert.ToDouble(lblExistBalanceLimit.Text) < Convert.ToDouble(txtTransferAmount.Text))
            {
                Utility.FunShowAlertMsg(this, "Transfer Amount should be Less than or equal to Balance Unit");
                return;
            }

            FunPriAssignVbl();
            Procparam.Add("@Option", "2");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Sanction_ID", Convert.ToString(ddlSanctionNo.SelectedValue));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@NewSanction_ID", Convert.ToString(ddlNewSanctionNo.SelectedValue));
            Procparam.Add("@Balance_Limit", Convert.ToString(lblExistBalanceLimit.Text));
            Procparam.Add("@Transfer_Limit", Convert.ToString(txtTransferAmount.Text));

            DataTable dtCons = Utility.GetDefaultData("S3G_FMGT_GETSancConsDtl", Procparam);

            if (Convert.ToInt32(dtCons.Rows[0]["ErrorCode"]) == 1)
            {
                Utility.FunShowAlertMsg(this, "New Sanction No was deleted already");
                return;
            }

            string strExistSanctionNo, strNewSanctionNo = string.Empty;
            Int64 iExistingSanctionID, iNewExistingSanctionID = 0;
            double dblBalanceLimit, dblTransferAmt = 0.0;

            strExistSanctionNo = Convert.ToString(ddlSanctionNo.SelectedText);
            strNewSanctionNo = Convert.ToString(ddlNewSanctionNo.SelectedText);
            iExistingSanctionID = Convert.ToInt64(ddlSanctionNo.SelectedValue);
            iNewExistingSanctionID = Convert.ToInt64(ddlNewSanctionNo.SelectedValue);
            dblBalanceLimit = Convert.ToDouble(lblExistBalanceLimit.Text);
            dblTransferAmt = Convert.ToDouble(txtTransferAmount.Text);
            FunPriAddConsolidation(iExistingSanctionID, strExistSanctionNo, dblBalanceLimit, dblTransferAmt, iNewExistingSanctionID, strNewSanctionNo);
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnCopyAddress_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriCopyAddress();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnLesseeSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGrid();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadFundRSSancDtl();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ddlSearchType.SelectedValue = "0";
            txtSearchText.Text = "";
            FunPriLoadFundRSSancDtl();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "Grid Events"

    protected void grvBankDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Label lblgdBankName = grvBankDetails.SelectedRow.FindControl("lblgdBankName") as Label;
            Label lblgdBankID = grvBankDetails.SelectedRow.FindControl("lblgdBankID") as Label;
            Label lblgdAccountType = grvBankDetails.SelectedRow.FindControl("lblgdAccountType") as Label;
            Label lblAccountTypeID = grvBankDetails.SelectedRow.FindControl("lblAccountTypeID") as Label;
            Label lblgdBenificiaryName = grvBankDetails.SelectedRow.FindControl("lblgdBenificiaryName") as Label;
            Label lblgdBenificiaryNo = grvBankDetails.SelectedRow.FindControl("lblgdBenificiaryNo") as Label;
            Label lblgdBankCity = grvBankDetails.SelectedRow.FindControl("lblgdBankCity") as Label;
            Label lblgdBranch = grvBankDetails.SelectedRow.FindControl("lblgdBranch") as Label;
            Label lblgdMicr = grvBankDetails.SelectedRow.FindControl("lblgdMicr") as Label;
            Label lblgdIFSC = grvBankDetails.SelectedRow.FindControl("lblgdIFSC") as Label;
            CheckBox chkIsDefaultAccount = grvBankDetails.SelectedRow.FindControl("chkIsDefaultAccount") as CheckBox;
            CheckBox chkIsActiveAccount = grvBankDetails.SelectedRow.FindControl("chkIsActiveAccount") as CheckBox;
            Label lblTransaction = grvBankDetails.SelectedRow.FindControl("lblTransaction") as Label;

            if (Convert.ToInt16(lblTransaction.Text) == 0)
            {
                txtAccountNo.Text = strModifyAccountNo = Convert.ToString(lblgdBenificiaryNo.Text).Trim();
                txtBankBranch.Text = Convert.ToString(lblgdBranch.Text).Trim();
                txtBankCity.Text = Convert.ToString(lblgdBankCity.Text).Trim();
                txtBankName.Text = Convert.ToString(lblgdBankName.Text).Trim();
                txtMICRCode.Text = Convert.ToString(lblgdMicr.Text).Trim();
                txtBenificiaryName.Text = Convert.ToString(lblgdBenificiaryName.Text).Trim();
                txtIFSCCode.Text = Convert.ToString(lblgdIFSC.Text).Trim();
                ddlAccountType.SelectedValue = Convert.ToString(lblAccountTypeID.Text).Trim();
                chkDefaultAccount.Checked = chkIsDefaultAccount.Checked;
                chkActiveAccount.Checked = chkIsActiveAccount.Checked;

                btnBankModify.Enabled = true; btnBankAdd.Enabled = false;
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Transaction already done for this account");
                return;
            }
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void grvBankDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow && Convert.ToString(strMode) != "Q")
            {
                /* For Deleting purpose, Restrcit to add attribute to the Cell Remove linkbutton*/
                for (int intCellIndex = 0; intCellIndex < e.Row.Cells.Count - 2; intCellIndex++)
                {
                    e.Row.Cells[intCellIndex].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                    (this.grvBankDetails, "Select$" + e.Row.RowIndex);
                }
                e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            }
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void grvLesseeDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblgdUtilizedLimit = (Label)e.Row.FindControl("lblgdUtilizedLimit");
                LinkButton lnkEditLessee = (LinkButton)e.Row.FindControl("lnkLesseeEdit");
                LinkButton lnkEditRemove = (LinkButton)e.Row.FindControl("lnkLesseeRemove");
                FileUpload flUpload1 = (FileUpload)e.Row.FindControl("flUpload1");
                TextBox txtFileupld1 = e.Row.FindControl("txtFileupld1") as TextBox;
                Button btnBrowse1 = (Button)e.Row.FindControl("btnBrowse1");

                if (Convert.ToString(lblgdUtilizedLimit.Text) != "")
                {
                    if (Convert.ToDouble(lblgdUtilizedLimit.Text) > 0 || Convert.ToString(strMode) == "Q")
                    {
                        lnkEditRemove.Enabled = false;
                    }
                    lnkEditLessee.Enabled = (Convert.ToString(strMode) == "Q") ? false : true;
                }
                else
                {
                    lnkEditLessee.Enabled = lnkEditRemove.Enabled = true;
                }

                flUpload1.Attributes.Add("onchange", "fnLoadPath1('" + btnBrowse1.ClientID + "'); ");
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtSanctionDate = (TextBox)e.Row.FindControl("txtgdSanctionDate");
                TextBox txtExpiryDate = (TextBox)e.Row.FindControl("txtgdExpiryDate");
                TextBox txtSanctionLimit = (TextBox)e.Row.FindControl("txtSanctionLimit");
                CalendarExtender ceSanctionDate = (CalendarExtender)e.Row.FindControl("ceSanctionDate");
                CalendarExtender ceExpiryDate = (CalendarExtender)e.Row.FindControl("ceExpiryDate");

                ceExpiryDate.Format = ceSanctionDate.Format = strDateFormat;
                txtSanctionDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtSanctionDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtExpiryDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtExpiryDate.ClientID + "','" + strDateFormat + "',false,  true);");

                FileUpload flUpload = (FileUpload)e.Row.FindControl("flUpload");
                Button btnDlg = (Button)e.Row.FindControl("btnDlg");
                flUpload.Attributes.Add("onchange", "fnAssignPath('" + flUpload.ClientID + "','" + hdnSelectedPath.ClientID + "'); fnLoadPath('" + btnBrowse.ClientID + "');");
                btnDlg.OnClientClick = "fnLoadPath('" + flUpload.ClientID + "');";
            }
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void grvLimitConsolidation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblgdDetailID = (Label)e.Row.FindControl("lblgdDetailID");
                LinkButton lnkRemoveConsolidation = (LinkButton)e.Row.FindControl("lnkRemoveConsolidation");

                lnkRemoveConsolidation.Enabled = (Convert.ToInt64(lblgdDetailID.Text) > 0 || strMode == "Q") ? false : true;
            }
        }
        catch (Exception ObjException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ObjException, strPageName);
        }
    }

    protected void grvLimitConsolidation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grvLimitConsolidation.PageIndex = e.NewPageIndex;
            FunPriBindGridDtls(3);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "Link Button Events"

    protected void lnkBankRemove_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvBankDetails", strSelectID);

            DataTable dtBankDtl = (DataTable)ViewState["BankDetails"];
            dtBankDtl.Rows.RemoveAt(_iRowIdx);
            ViewState["BankDetails"] = dtBankDtl;
            FunPriBindGridDtls(1);
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void lnkLesseeEdit_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvLesseeDetails", strSelectID);

            LinkButton lnklesseeEdit = (LinkButton)grvLesseeDetails.Rows[_iRowIdx].FindControl("lnkLesseeEdit");
            LinkButton lnkLesseeRemove = (LinkButton)grvLesseeDetails.Rows[_iRowIdx].FindControl("lnkLesseeRemove");
            TextBox txtEditCustomerName = (TextBox)grvLesseeDetails.Rows[_iRowIdx].FindControl("txtEditCustomerName");
            TextBox txtEditAssetCategory = (TextBox)grvLesseeDetails.Rows[_iRowIdx].FindControl("txtEditAssetCategory");
            TextBox txtEditSanctionNo = (TextBox)grvLesseeDetails.Rows[_iRowIdx].FindControl("txtEditSanctionNo");
            TextBox txtEditSanctionDate = (TextBox)grvLesseeDetails.Rows[_iRowIdx].FindControl("txtEditSanctionDate");
            TextBox txtEditSanctionLimit = (TextBox)grvLesseeDetails.Rows[_iRowIdx].FindControl("txtEditSanctionLimit");
            TextBox txtEditExpiryDate = (TextBox)grvLesseeDetails.Rows[_iRowIdx].FindControl("txtEditExpiryDate");
            Label lblgdAssetCategoryID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdAssetCategoryID");
            Label lblgdCustomerID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdCustomerID");
            Label lblEditSanctionNo = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdSanctionNo");
            Label lblEditCustomerID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblEditCustomerID");
            Label lblEditAssetCategoryID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblEditAssetCategoryID");
            CalendarExtender ceEditSanctionDate = (CalendarExtender)grvLesseeDetails.Rows[_iRowIdx].FindControl("ceEditSanctionDate");
            CalendarExtender ceEditExpiryDate = (CalendarExtender)grvLesseeDetails.Rows[_iRowIdx].FindControl("ceEditExpiryDate");
            ImageButton imgbtnEditLesseeInfo = (ImageButton)grvLesseeDetails.Rows[_iRowIdx].FindControl("imgbtnEditLesseeInfo");
            Label lblgdLesseeFunderID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdLesseeFunderID");
            Label lblgdUtilizedLimit = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdUtilizedLimit");

            Button btnBrowse1 = (Button)grvLesseeDetails.Rows[_iRowIdx].FindControl("btnBrowse1");
            FileUpload flUpload1 = (FileUpload)grvLesseeDetails.Rows[_iRowIdx].FindControl("flUpload1");
            TextBox txtFileupld1 = (TextBox)grvLesseeDetails.Rows[_iRowIdx].FindControl("txtFileupld1");
            ImageButton hyplnkDocView = (ImageButton)grvLesseeDetails.Rows[_iRowIdx].FindControl("hyplnkDocView");
            Label lblDocumentProof = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblDocumentProof");
            Label lblSancStatus = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblSancStatus");

            if (Convert.ToString(lnklesseeEdit.Text) == "Edit")
            {
                lnklesseeEdit.Text = "Update";
                lnkLesseeRemove.Enabled = imgbtnEditLesseeInfo.Enabled = false;

                txtEditAssetCategory.ReadOnly = txtEditSanctionDate.ReadOnly = 
                txtEditExpiryDate.ReadOnly = (Convert.ToString(lblSancStatus.Text) == "Approved" && Convert.ToDouble(lblgdUtilizedLimit.Text) > 0) ? true : false;

                ceEditExpiryDate.Enabled = ceEditSanctionDate.Enabled = txtEditSanctionDate.Enabled =
                txtEditExpiryDate.Enabled = (Convert.ToString(lblSancStatus.Text) == "Approved" && Convert.ToDouble(lblgdUtilizedLimit.Text) > 0) ? false : true;

                txtEditSanctionLimit.ReadOnly = false;

                //txtEditCustomerName.ReadOnly = txtEditSanctionNo.ReadOnly = 

                ceEditSanctionDate.Format = ceEditExpiryDate.Format = strDateFormat;
                txtEditSanctionDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEditSanctionDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtEditExpiryDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEditExpiryDate.ClientID + "','" + strDateFormat + "',false,  true);");

                hdnCustomerID.Value = Convert.ToString(lblgdCustomerID.Text);
                hdnAssetID.Value = Convert.ToString(lblgdAssetCategoryID.Text);

                lnklesseeEdit.ValidationGroup = "vsEditCustomer";
                btnBrowse1.Visible = flUpload1.Visible = txtFileupld1.Visible = true;
                hyplnkDocView.Visible = false;
            }
            else if (Convert.ToString(lnklesseeEdit.Text) == "Update")
            {
                ViewState["IsNewAddtionalInfo"] = 0;

                #region "Hide"

                /*
                DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];

                string strFilter = string.Empty;
                if ((Convert.ToString(txtEditSanctionNo.Text) != Convert.ToString(lblEditSanctionNo.Text)) || (Convert.ToInt64(hdnCustomerID.Value) != Convert.ToInt64(lblEditCustomerID.Text)) || (Convert.ToInt32(hdnAssetID.Value) != Convert.ToInt32(lblEditAssetCategoryID.Text)))
                {
                    bool blnRslt = FunPriCheckDuplicateSanction(dtLessee, Convert.ToString(txtEditSanctionNo.Text).Trim(), Convert.ToInt64(hdnCustomerID.Value)
                    , Convert.ToInt32(hdnAssetID.Value), "E", Convert.ToInt32(lblEditAssetCategoryID.Text));

                    if (blnRslt == false)
                    {
                        Utility.FunShowAlertMsg(this, "Duplicate entries are not allowed");
                        return;
                    }
                }

                strFilter = "Sanction_Ref_No = '" + Convert.ToString(lblEditSanctionNo.Text).Trim() + "'";
                strFilter += " and Customer_ID = " + Convert.ToInt64(lblEditCustomerID.Text);
                strFilter += " and Asset_Category_ID = " + Convert.ToInt64(lblEditAssetCategoryID.Text);

                DataRow[] drLessee = dtLessee.Select(strFilter);
                drLessee[0]["Customer_ID"] = Convert.ToInt64(hdnCustomerID.Value);
                drLessee[0]["Customer_Name"] = Convert.ToString(txtEditCustomerName.Text).Trim();
                drLessee[0]["Asset_Category_ID"] = Convert.ToInt32(hdnAssetID.Value);
                drLessee[0]["Asset_Category_Desc"] = Convert.ToString(txtEditAssetCategory.Text).Trim();
                drLessee[0]["Limit"] = Convert.ToDouble(txtEditSanctionLimit.Text);
                drLessee[0]["Sanction_Date"] = Convert.ToString(txtEditSanctionDate.Text);
                drLessee[0]["Sanction_Ref_No"] = Convert.ToString(txtEditSanctionNo.Text).Trim();
                drLessee[0]["Expiry_Date"] = Convert.ToString(txtEditExpiryDate.Text);
                drLessee[0]["Balance_Limit"] = Convert.ToDouble(txtEditSanctionLimit.Text);

                double dblPf_Prec = (Convert.ToString(drLessee[0]["Processing_Fee_Perc"]) == "") ? 0 : Convert.ToDouble(drLessee[0]["Processing_Fee_Perc"]);
                double dblLimit = (Convert.ToString(drLessee[0]["Limit"]) == "") ? 0 : Convert.ToDouble(drLessee[0]["Limit"]);
                Int32 iAssetCategory_ID = (Convert.ToString(drLessee[0]["Asset_Category_ID"]) == "") ? 0 : Convert.ToInt32(drLessee[0]["Asset_Category_ID"]);
                string strSanctionDate = Convert.ToString(txtEditSanctionDate.Text);
                FunPriCalculateProcessingFee("1", dblPf_Prec, 0, dblLimit, iAssetCategory_ID, strSanctionDate);

                drLessee[0]["Processing_Fee_Perc"] = (Convert.ToString(txtProcessingFeePerc.Text).Trim() != "") ? Convert.ToDouble(txtProcessingFeePerc.Text) : 0;
                drLessee[0]["Processing_Fee"] = (Convert.ToString(txtProcessingFee.Text).Trim() != "") ? Convert.ToDouble(txtProcessingFee.Text) : 0;
                drLessee[0]["PF_Service_Tax"] = (Convert.ToString(txtPFServiceTax.Text).Trim() != "") ? Convert.ToDouble(txtPFServiceTax.Text) : 0;
                drLessee[0]["Total_PF"] = (Convert.ToString(txtTotalPF.Text).Trim() != "") ? Convert.ToDouble(txtTotalPF.Text) : 0;
                drLessee[0]["Is_Modified"] = 1;

                dtLessee.AcceptChanges();
                ViewState["LesseeSettings"] = dtLessee;
                FunPriBindGridDtls(2);

                txtEditCustomerName.ReadOnly = txtEditAssetCategory.ReadOnly = txtEditSanctionNo.ReadOnly = txtEditSanctionDate.ReadOnly =
                txtEditSanctionLimit.ReadOnly = txtEditExpiryDate.ReadOnly = lnkLesseeRemove.Enabled = imgbtnEditLesseeInfo.Enabled = true;

                ceEditExpiryDate.Enabled = ceEditSanctionDate.Enabled = false;

                hdnCustomerID.Value = hdnAssetID.Value = "";
                lnklesseeEdit.Text = "Edit";

                */

                #endregion

                if (Convert.ToDecimal(txtEditSanctionLimit.Text) < Convert.ToDecimal(lblgdUtilizedLimit.Text))
                {
                    Utility.FunShowAlertMsg(this, "Sanction Limit should be greater than or equal to Utilized Limit");
                    return;
                }

                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();

                Procparam.Add("@Option", (Convert.ToString(lblSancStatus.Text) == "Approved" && Convert.ToDouble(lblgdUtilizedLimit.Text) > 0) ? "5" : "2");
                Procparam.Add("@Funder_ID", Convert.ToString(intFunderId));
                Procparam.Add("@Created_By", Convert.ToString(intUserID));
                Procparam.Add("@Customer_ID", Convert.ToString(hdnCustomerID.Value));
                Procparam.Add("@Funder_Detail_ID", Convert.ToString(lblgdLesseeFunderID.Text));
                Procparam.Add("@Sanction_No", Convert.ToString(txtEditSanctionNo.Text).Trim());
                Procparam.Add("@Sanction_Date", Convert.ToString(Utility.StringToDate(txtEditSanctionDate.Text)));
                Procparam.Add("@Sanction_Limit", Convert.ToString(txtEditSanctionLimit.Text));
                Procparam.Add("@Balance_Limit", Convert.ToString(Convert.ToDecimal(txtEditSanctionLimit.Text) - Convert.ToDecimal(lblgdUtilizedLimit.Text)));
                Procparam.Add("@Expiry_Date", Convert.ToString(Utility.StringToDate(txtEditExpiryDate.Text)));
                Procparam.Add("@Asset_Category_ID", Convert.ToString(hdnAssetID.Value));
                Procparam.Add("@Old_Sanction_No", Convert.ToString(lblEditSanctionNo.Text));
                Procparam.Add("@Old_CustomerID", Convert.ToString(lblEditCustomerID.Text));
                Procparam.Add("@Old_AssetCategoryID", Convert.ToString(lblEditAssetCategoryID.Text));
                Procparam.Add("@Location_ID", Convert.ToString(txtCorpState.SelectedValue));

                Procparam.Add("@Upfront_Int", Convert.ToString(txtUpfrontInt.Text));
                Procparam.Add("@Net_Discount_Rate", Convert.ToString(txtNetDiscountRate.Text));
                Procparam.Add("@Discount_Processing", Convert.ToString(chkDiscountProcessing.Checked));
                Procparam.Add("@Discount_Upfront", Convert.ToString(chkUpfront.Checked));

                //Added on 06Jun2015 Starts Here
                string strUpldPath = string.Empty;
                if (Convert.ToString(txtFileupld1.Text) != "")
                {
                    string strpath = string.Empty;
                    if (ViewState["DocumentationPath"] != null || Convert.ToString(ViewState["DocumentationPath"]) != "")
                    {
                        strpath = Convert.ToString(ViewState["DocumentationPath"]);
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Document path not available to Upload");
                        return;
                    }
                    strUpldPath = FunPriUpldFiles(Convert.ToString(txtEditSanctionNo.Text), Convert.ToInt64(hdnCustomerID.Value), strpath);
                }

                if (Convert.ToString(strUpldPath) != "")
                {
                    Procparam.Add("@Document_Path", Convert.ToString(strUpldPath));
                }

                //Added on 06Jun2015 Ends Here

                DataTable dtSanction = Utility.GetDefaultData("S3G_FMGT_INSERT_TMPSANCDTL", Procparam);
                if (Convert.ToInt32(dtSanction.Rows[0]["Error_Code"]) == 1)
                {
                    Utility.FunShowAlertMsg(this, "Duplicate entries are not allowed");
                    return;
                }
                else if (Convert.ToInt32(dtSanction.Rows[0]["Error_Code"]) == 2)
                {
                    Utility.FunShowAlertMsg(this, "Discount Rate should not be blank");
                    return;
                }

                txtEditCustomerName.ReadOnly = txtEditAssetCategory.ReadOnly = txtEditSanctionNo.ReadOnly = txtEditSanctionDate.ReadOnly =
                txtEditSanctionLimit.ReadOnly = txtEditExpiryDate.ReadOnly = lnkLesseeRemove.Enabled = imgbtnEditLesseeInfo.Enabled = true;

                ceEditExpiryDate.Enabled = ceEditSanctionDate.Enabled = false;

                btnBrowse1.Visible = flUpload1.Visible = txtFileupld1.Visible = false;
                hyplnkDocView.Visible = true;

                hdnCustomerID.Value = hdnAssetID.Value = "";
                lnklesseeEdit.Text = "Edit";
                FunPriBindGrid();
            }
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void lnkLesseeRemove_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvLesseeDetails", strSelectID);

            Label lblEditSanctionNo = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdSanctionNo");
            Label lblgdLesseeFunderID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdLesseeFunderID");
            Label lblEditCustomerID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblEditCustomerID");
            Label lblEditAssetCategoryID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblEditAssetCategoryID");

            #region "Hide"
            /*

            string strFilter = "Sanction_Ref_No = '" + Convert.ToString(lblEditSanctionNo.Text).Trim() + "' and Customer_ID = " + Convert.ToInt64(lblEditCustomerID.Text) + " and Asset_Category_ID = " + Convert.ToInt64(lblEditAssetCategoryID.Text);

            DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
            DataRow[] dr = dtLessee.Select(strFilter);

            if (Convert.ToInt64(lblgdLesseeFunderID.Text) > 0)
            {
                dr[0]["Is_Deleted"] = 1;
                dtLessee.AcceptChanges();
            }
            else
            {
                dr[0].Delete();
                dtLessee.AcceptChanges();
            }
            ViewState["LesseeSettings"] = dtLessee;
            FunPriBindGridDtls(2);

            */
            #endregion

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "4");
            Procparam.Add("@Funder_Detail_ID", Convert.ToString(lblgdLesseeFunderID.Text));
            Procparam.Add("@Sanction_No", Convert.ToString(lblEditSanctionNo.Text));
            Procparam.Add("@Customer_ID", Convert.ToString(lblEditCustomerID.Text));
            Procparam.Add("@Asset_Category_ID", Convert.ToString(lblEditAssetCategoryID.Text));
            Procparam.Add("@Created_By", Convert.ToString(intUserID));

            DataTable dtSancDtl = Utility.GetDefaultData("S3G_FMGT_INSERT_TMPSANCDTL", Procparam);
            FunPriBindGrid();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void lnkLesseeADD_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["IsNewAddtionalInfo"] = 0;
            TextBox txtSanctionRefNo = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionRefNo");
            TextBox txtSanctionDate = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionDate");
            TextBox txtExpiryDate = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdExpiryDate");
            TextBox txtSanctionLimit = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtSanctionLimit");
            UserControls_S3GAutoSuggest txtCustomerName = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtCustomerName");
            UserControls_S3GAutoSuggest txtgdAssetCategory = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtgdAssetCategory");
            Label lblCurrentPath = (Label)grvLesseeDetails.FooterRow.FindControl("lblCurrentPath");

            if (Convert.ToInt32(txtgdAssetCategory.SelectedValue) == 0 && Convert.ToString(txtgdAssetCategory.SelectedText) != "All")
            {
                Utility.FunShowAlertMsg(this, "Select Asset Category");
                return;
            }

            FunPriInsertTmpSancDtl(1, 0, 0);

            /*
            if (ViewState["LesseeSettings"] != null)
            {
                DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
                if (dtLessee.Rows.Count == 0)
                {
                    dtLessee = (DataTable)ViewState["DefaultLesseeSettings"];
                    ViewState["LesseeSettings"] = dtLessee;
                }
                if (Convert.ToString(dtLessee.Rows[0]["Sanction_Ref_No"]) == "")
                {
                    dtLessee.Rows.RemoveAt(0);
                    dtLessee.AcceptChanges();
                }

                bool blnRslt = FunPriCheckDuplicateSanction(dtLessee, Convert.ToString(txtSanctionRefNo.Text).Trim(), Convert.ToInt64(txtCustomerName.SelectedValue)
                    , Convert.ToInt32(txtgdAssetCategory.SelectedValue), "C", 0);

                if (blnRslt == false)
                {
                    Utility.FunShowAlertMsg(this, "Duplicate entries are not allowed");
                    return;
                }

                

                
                DataRow drLessee = dtLessee.NewRow();

                drLessee["Lessee_Funder_ID"] = 0;
                drLessee["Customer_ID"] = Convert.ToInt64(txtCustomerName.SelectedValue);
                drLessee["Customer_Name"] = Convert.ToString(txtCustomerName.SelectedText);
                drLessee["Asset_Category_ID"] = Convert.ToInt32(txtgdAssetCategory.SelectedValue);
                drLessee["Asset_Category_Desc"] = Convert.ToString(txtgdAssetCategory.SelectedText);
                drLessee["Limit"] = Convert.ToDouble(txtSanctionLimit.Text);
                drLessee["Expiry_Date"] = Convert.ToString(txtExpiryDate.Text);
                drLessee["Sanction_Date"] = (Convert.ToString(txtSanctionDate.Text).Trim() != "") ? Convert.ToString(txtSanctionDate.Text) : null;
                drLessee["Sanction_Ref_No"] = Convert.ToString(txtSanctionRefNo.Text).Trim();
                drLessee["Balance_Limit"] = Convert.ToDouble(txtSanctionLimit.Text);
                drLessee["Utilized_Limit"] = 0;
                drLessee["Is_Deleted"] = 0;
                drLessee["Document_Upload"] = Convert.ToString(lblCurrentPath.Text);
                drLessee["End_Customer_Name"] = Convert.ToString(ddlEndCustomer.SelectedText).Trim();
                drLessee["End_Customer_ID"] = (Convert.ToString(ddlEndCustomer.SelectedText) == "") ? 0 : Convert.ToInt64(ddlEndCustomer.SelectedValue);
                drLessee["PV_Calculation_Method_ID"] = Convert.ToInt32(ddlPVCalcMethod.SelectedValue);
                drLessee["PV_Calculation_Desc"] = Convert.ToString(ddlPVCalcMethod.SelectedItem.Text);
                drLessee["Discount_Rate"] = (Convert.ToString(txtDiscountRate.Text).Trim() != "") ? Convert.ToDouble(txtDiscountRate.Text) : 0;
                drLessee["Processing_Fee_Perc"] = (Convert.ToString(txtProcessingFeePerc.Text).Trim() != "") ? Convert.ToDouble(txtProcessingFeePerc.Text) : 0;
                drLessee["Processing_Fee"] = (Convert.ToString(txtProcessingFee.Text).Trim() != "") ? Convert.ToDouble(txtProcessingFee.Text) : 0;
                drLessee["ForeClosure_Rate"] = (Convert.ToString(txtForeClosureRate.Text).Trim() != "") ? Convert.ToDouble(txtForeClosureRate.Text) : 0;
                drLessee["Misc_Charges"] = (Convert.ToString(txtMiscCharges.Text).Trim() != "") ? Convert.ToDouble(txtMiscCharges.Text) : 0;
                drLessee["Tenor"] = (Convert.ToString(txtTenor.Text).Trim() != "") ? Convert.ToInt32(txtTenor.Text) : 0;
                drLessee["Over_Due_Rate"] = (Convert.ToString(txtOverDueRate.Text).Trim() != "") ? Convert.ToDouble(txtOverDueRate.Text) : 0;
                drLessee["Collateral_Dtls"] = Convert.ToString(txtCollateralDetails.Text).Trim();
                drLessee["Remarks"] = Convert.ToString(txtRemarks.Text).Trim();
                drLessee["Cheque_Return_Charges"] = (Convert.ToString(txtChqRtnChrgs.Text).Trim() != "") ? Convert.ToDouble(txtChqRtnChrgs.Text) : 0;
                drLessee["PF_Service_Tax"] = (Convert.ToString(txtPFServiceTax.Text).Trim() != "") ? Convert.ToDouble(txtPFServiceTax.Text) : 0;
                drLessee["Total_PF"] = (Convert.ToString(txtTotalPF.Text).Trim() != "") ? Convert.ToDouble(txtTotalPF.Text) : 0;

                dtLessee.Rows.Add(drLessee);
                ViewState["LesseeSettings"] = dtLessee;
                FunPriBindGridDtls(2);

                
            }
            */
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void lnkRemoveConsolidation_Click(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((LinkButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvLimitConsolidation", strSelectID);

            Label lblgdDetailID = (Label)grvLimitConsolidation.Rows[_iRowIdx].FindControl("lblgdDetailID");
            Label lblgdExsitingSanctionID = (Label)grvLimitConsolidation.Rows[_iRowIdx].FindControl("lblgdExsitingSanctionID");
            Label lblTransferAmount = (Label)grvLimitConsolidation.Rows[_iRowIdx].FindControl("lblTransferAmount");
            Label lblNewSanctionID = (Label)grvLimitConsolidation.Rows[_iRowIdx].FindControl("lblNewSanctionID");

            if (Convert.ToInt64(lblgdDetailID.Text) == 0)
            {
                #region "HIDE"
                /*
                DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
                //Modify Exist Sanction Details
                DataRow[] drLessee = dtLessee.Select("Lessee_Funder_ID = " + Convert.ToInt64(lblgdExsitingSanctionID.Text));
                drLessee[0]["Balance_Limit"] = Convert.ToDouble(drLessee[0]["Balance_Limit"]) + Convert.ToDouble(lblTransferAmount.Text);
                drLessee[0]["Utilized_Limit"] = Convert.ToDouble(drLessee[0]["Utilized_Limit"]) - Convert.ToDouble(lblTransferAmount.Text);
                dtLessee.AcceptChanges();

                //Modify New Sanction Details
                drLessee = dtLessee.Select("Lessee_Funder_ID = " + Convert.ToInt64(lblNewSanctionID.Text));
                drLessee[0]["Balance_Limit"] = Convert.ToDouble(drLessee[0]["Balance_Limit"]) - Convert.ToDouble(lblTransferAmount.Text);
                drLessee[0]["Limit"] = Convert.ToDouble(drLessee[0]["Limit"]) - Convert.ToDouble(lblTransferAmount.Text);
                dtLessee.AcceptChanges();

                ViewState["LesseeSettings"] = dtLessee;
                FunPriBindGridDtls(2);
                */
                #endregion

                DataTable dtCons = (DataTable)ViewState["LimitConsolidationDetails"];
                string strFilter = "Existing_sanction_ID = " + Convert.ToInt64(lblgdExsitingSanctionID.Text);
                strFilter += "and New_Sanction_ID = " + Convert.ToInt64(lblNewSanctionID.Text);
                strFilter += "and Consolidation_ID = 0";
                DataRow[] drCons = dtCons.Select(strFilter);
                drCons[0].Delete();
                dtCons.AcceptChanges();
                ViewState["LimitConsolidationDetails"] = dtCons;
                FunPriBindGridDtls(3);

                FunPriAssignVbl();
                Procparam.Add("@Option", "4");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@Sanction_ID", Convert.ToString(lblgdExsitingSanctionID.Text));
                Procparam.Add("@User_ID", Convert.ToString(intUserID));
                Procparam.Add("@NewSanction_ID", Convert.ToString(lblNewSanctionID.Text));
                Procparam.Add("@Transfer_Limit", Convert.ToString(lblTransferAmount.Text));

                DataTable dtUpd = Utility.GetDefaultData("S3G_FMGT_GETSancConsDtl", Procparam);
                FunPriBindGrid();
            }
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "Image Button Events"

    protected void imgbtnAddLesseeInfo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            btnLesseeAdd.Visible = btnLesseeAdd.Enabled = true;
            btnLesseeUpdate.Visible = btnLesseeUpdate.Enabled = false;
            TextBox txtSanctionLimit = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtSanctionLimit");
            TextBox txtSanctionDate = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionDate");
            TextBox txtSanctionRefNo = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionRefNo");
            UserControls_S3GAutoSuggest ddlAssetCategory = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtgdAssetCategory");
            UserControls_S3GAutoSuggest ddlCustomerName = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtCustomerName");

            txtPopupLimit.Text = txtSanctionLimit.Text;
            lblEditAssetID.Text = lblSearchAssetCategoryID.Text = ddlAssetCategory.SelectedValue;
            lblSerachSanctionDate.Text = Convert.ToString(txtSanctionDate.Text);
            lblSearchCustomerID.Text = Convert.ToString(ddlCustomerName.SelectedValue);
            lblSrchFndrDtlID.Text = "0";

            Int32 _intAdd = (ViewState["IsNewAddtionalInfo"] == null) ? 0 : 1;
            moeAdditionalInfo.Show();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void imgbtnEditLesseeInfo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string strSelectID = ((ImageButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvLesseeDetails", strSelectID);

            Label lblSanctionNo = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdSanctionNo");
            Label lblEditCustomerID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblEditCustomerID");
            Label lblEditAssetCategoryID = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblEditAssetCategoryID");
            Label lblUtilizedAmount = (Label)grvLesseeDetails.Rows[_iRowIdx].FindControl("lblgdUtilizedLimit");

            string strFilter = "Sanction_Ref_No = '" + Convert.ToString(lblSanctionNo.Text).Trim() + "' and Customer_ID = " + Convert.ToInt64(lblEditCustomerID.Text) + " and Asset_Category_ID = " + Convert.ToInt64(lblEditAssetCategoryID.Text);

            DataRow[] drLessee = ((DataTable)ViewState["LesseeSettings"]).Select(strFilter);
            ddlPVCalcMethod.SelectedValue = Convert.ToString(drLessee[0]["PV_Calculation_Method_ID"]);
            ddlEndCustomer.SelectedText = Convert.ToString(drLessee[0]["End_Customer_Name"]);
            ddlEndCustomer.SelectedValue = Convert.ToString(drLessee[0]["End_Customer_ID"]);
            txtDiscountRate.Text = (Convert.ToInt32(drLessee[0]["Discount_Rate"]) > 0) ? Convert.ToString(drLessee[0]["Discount_Rate"]) : "";
            txtProcessingFee.Text = Convert.ToString(drLessee[0]["Processing_Fee"]);
            txtProcessingFeePerc.Text = Convert.ToString(drLessee[0]["Processing_Fee_Perc"]);
            txtForeClosureRate.Text = Convert.ToString(drLessee[0]["ForeClosure_Rate"]);
            txtMiscCharges.Text = Convert.ToString(drLessee[0]["Misc_Charges"]);
            txtTenor.Text = (Convert.ToString(drLessee[0]["Tenor"]) != "0") ? Convert.ToString(drLessee[0]["Tenor"]) : "";
            txtChqRtnChrgs.Text = Convert.ToString(drLessee[0]["Cheque_Return_Charges"]);
            txtCollateralDetails.Text = Convert.ToString(drLessee[0]["Collateral_Dtls"]);
            txtRemarks.Text = Convert.ToString(drLessee[0]["Remarks"]);
            txtOverDueRate.Text = Convert.ToString(drLessee[0]["Over_Due_Rate"]);
            lblSearchSanctionNo.Text = Convert.ToString(lblSanctionNo.Text);
            lblSearchAssetCategoryID.Text = Convert.ToString(drLessee[0]["Asset_Category_ID"]);
            lblSearchCustomerID.Text = lblEditCustomerID.Text;
            lblEditAssetID.Text = lblEditAssetCategoryID.Text;
            lblSerachSanctionDate.Text = Convert.ToString(drLessee[0]["Sanction_Date"]);
            txtPopupLimit.Text = Convert.ToString(drLessee[0]["Limit"]);
            txtPFServiceTax.Text = Convert.ToString(drLessee[0]["PF_Service_Tax"]);
            txtTotalPF.Text = Convert.ToString(drLessee[0]["Total_PF"]);
            lblSrchFndrDtlID.Text = Convert.ToString(drLessee[0]["Lessee_Funder_ID"]);

            txtUpfrontInt.Text = Convert.ToString(drLessee[0]["Upfront_Int"]);
            txtNetDiscountRate.Text = Convert.ToString(drLessee[0]["Net_Discount_Rate"]);
            chkDiscountProcessing.Checked = Convert.ToBoolean(drLessee[0]["Discount_Processing"]);
            chkUpfront.Checked = Convert.ToBoolean(drLessee[0]["Discount_Upfront"]);

            btnLesseeAdd.Visible = btnLesseeAdd.Enabled = false;
            btnLesseeUpdate.Visible = btnLesseeUpdate.Enabled = (Convert.ToDecimal(lblUtilizedAmount.Text) > 0 || (Convert.ToString(strMode) == "Q")) ? false : true;

            chkDiscountProcessing.Enabled = chkUpfront.Enabled = ddlPVCalcMethod.Enabled = (Convert.ToDecimal(lblUtilizedAmount.Text) > 0 || (Convert.ToString(strMode) == "Q")) ? false : true;

            txtUpfrontInt.ReadOnly = txtDiscountRate.ReadOnly = txtProcessingFee.ReadOnly = txtProcessingFeePerc.ReadOnly = txtForeClosureRate.ReadOnly = txtMiscCharges.ReadOnly =
            txtTenor.ReadOnly = txtOverDueRate.ReadOnly = ddlEndCustomer.ReadOnly = txtChqRtnChrgs.ReadOnly = txtCollateralDetails.ReadOnly = txtUpfrontInt.ReadOnly =
            txtRemarks.ReadOnly = (Convert.ToDecimal(lblUtilizedAmount.Text) > 0 || (Convert.ToString(strMode) == "Q")) ? true : false;

            moeAdditionalInfo.Show();
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "Dropdown Events"

    protected void ddlSanctionNo_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            UserControls_S3GAutoSuggest ddlSanctionNo = (UserControls_S3GAutoSuggest)grvLimitConsolidation.FooterRow.FindControl("ddlSanctionNo");
            Label lblExistBalanceLimit = (Label)grvLimitConsolidation.FooterRow.FindControl("lblExistBalanceLimit");
            TextBox txtTransferAmount = (TextBox)grvLimitConsolidation.FooterRow.FindControl("txtTransferAmount");
            UserControls_S3GAutoSuggest ddlNewSanctionNo = (UserControls_S3GAutoSuggest)grvLimitConsolidation.FooterRow.FindControl("ddlNewSanctionNo");

            if (Convert.ToInt64(ddlSanctionNo.SelectedValue) > 0)
            {
                FunPriAssignVbl();
                Procparam.Add("@Option", "1");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@Sanction_ID", Convert.ToString(ddlSanctionNo.SelectedValue));
                Procparam.Add("@User_ID", Convert.ToString(intUserID));

                DataTable dtCons = Utility.GetDefaultData("S3G_FMGT_GETSancConsDtl", Procparam);

                if (Convert.ToInt32(dtCons.Rows[0]["Is_Deleted"]) == 1)
                {
                    Utility.FunShowAlertMsg(this, "Selected Sanction no is deleted");
                    ddlSanctionNo.SelectedValue = lblExistBalanceLimit.Text = txtTransferAmount.Text = "0";
                    ddlSanctionNo.SelectedText = "";
                    return;
                }
                else if (Convert.ToDouble(dtCons.Rows[0]["Balance_Limit"]) == 0)
                {
                    Utility.FunShowAlertMsg(this, "Balance Unit should be greater than 0");
                    ddlSanctionNo.SelectedValue = lblExistBalanceLimit.Text = txtTransferAmount.Text = "0";
                    ddlSanctionNo.SelectedText = "";
                    return;
                }

                lblExistBalanceLimit.Text = Convert.ToString(dtCons.Rows[0]["Balance_Limit"]);
                txtTransferAmount.Text = Convert.ToString(dtCons.Rows[0]["Balance_Limit"]);
                ddlNewSanctionNo.SelectedValue = "0";
                ddlNewSanctionNo.SelectedText = "";
            }
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void txtCustomerName_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            FunPriLoadAdditionalInfo();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "Textbox Events"

    protected void txtProcessingFeePerc_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (((Convert.ToString(txtProcessingFeePerc.Text) == "") ? 0 : Convert.ToDouble(txtProcessingFeePerc.Text)) > 100)
            {
                Utility.FunShowAlertMsg(this, "Processing Fee Percentage should not greater than 100");
                txtProcessingFeePerc.Text = "0";
            }
            double dblPf_Prec = (Convert.ToString(txtProcessingFeePerc.Text) == "") ? 0 : Convert.ToDouble(txtProcessingFeePerc.Text);
            double dblLimit = (Convert.ToString(txtPopupLimit.Text) == "") ? 0 : Convert.ToDouble(txtPopupLimit.Text);
            Int32 iAssetCategory_ID = (Convert.ToString(lblSearchAssetCategoryID.Text) == "") ? 0 : Convert.ToInt32(lblSearchAssetCategoryID.Text);
            string strSanctionDate = Convert.ToString(lblSerachSanctionDate.Text);
            FunPriCalculateProcessingFee("1", dblPf_Prec, 0, dblLimit, iAssetCategory_ID, strSanctionDate);

            chkDiscountProcessing.Checked = chkUpfront.Checked = false;
            txtNetDiscountRate.Text = txtDiscountRate.Text;

        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void txtProcessingFee_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double dblpf_amt = (Convert.ToString(txtProcessingFee.Text) == "") ? 0 : Convert.ToDouble(txtProcessingFee.Text);
            double dblLimit = (Convert.ToString(txtPopupLimit.Text) == "") ? 0 : Convert.ToDouble(txtPopupLimit.Text);
            Int32 iAssetCategory_ID = (Convert.ToString(lblSearchAssetCategoryID.Text) == "") ? 0 : Convert.ToInt32(lblSearchAssetCategoryID.Text);
            string strSanctionDate = Convert.ToString(lblSerachSanctionDate.Text);
            if (dblpf_amt > dblLimit)
            {
                Utility.FunShowAlertMsg(this, "Processing Fee should not be greater than Sanction Limit");
                dblpf_amt = 0;
            }
            FunPriCalculateProcessingFee("2", 0, dblpf_amt, dblLimit, iAssetCategory_ID, strSanctionDate);
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void txtgdSanctionRefNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            #region "Hide"
            /*
            if (ViewState["LesseeSettings"] != null)
            {
                DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
                DataRow[] drLessee = dtLessee.Select("Sanction_Ref_No ='" + Convert.ToString(txtSanctionRefNo.Text) + "'");
                if (drLessee.Length > 0)
                {
                    ViewState["IsNewAddtionalInfo"] = 1;
                    ddlPVCalcMethod.SelectedValue = Convert.ToString(drLessee[0]["PV_Calculation_Method_ID"]);
                    ddlEndCustomer.SelectedText = Convert.ToString(drLessee[0]["End_Customer_Name"]);
                    ddlEndCustomer.SelectedValue = Convert.ToString(drLessee[0]["End_Customer_ID"]);
                    txtDiscountRate.Text = (Convert.ToInt32(drLessee[0]["Discount_Rate"]) > 0) ? Convert.ToString(drLessee[0]["Discount_Rate"]) : "";
                    txtProcessingFee.Text = Convert.ToString(drLessee[0]["Processing_Fee"]);
                    txtProcessingFeePerc.Text = Convert.ToString(drLessee[0]["Processing_Fee_Perc"]);
                    txtForeClosureRate.Text = Convert.ToString(drLessee[0]["ForeClosure_Rate"]);
                    txtMiscCharges.Text = Convert.ToString(drLessee[0]["Misc_Charges"]);
                    txtTenor.Text = (Convert.ToString(drLessee[0]["Tenor"]) != "0") ? Convert.ToString(drLessee[0]["Tenor"]) : "";
                    txtChqRtnChrgs.Text = Convert.ToString(drLessee[0]["Cheque_Return_Charges"]);
                    txtCollateralDetails.Text = Convert.ToString(drLessee[0]["Collateral_Dtls"]);
                    txtRemarks.Text = Convert.ToString(drLessee[0]["Remarks"]);
                    txtOverDueRate.Text = Convert.ToString(drLessee[0]["Over_Due_Rate"]);
                    txtPFServiceTax.Text = Convert.ToString(drLessee[0]["PF_Service_Tax"]);
                    txtTotalPF.Text = Convert.ToString(drLessee[0]["Total_PF"]);
                }
            }
            */
            #endregion

            FunPriLoadAdditionalInfo();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    protected void txtSanctionLimit_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (((Convert.ToString(txtProcessingFeePerc.Text) == "") ? 0 : Convert.ToDouble(txtProcessingFeePerc.Text)) == 0)
            {
                return;
            }

            TextBox txtSanctionLimit = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtSanctionLimit");
            UserControls_S3GAutoSuggest txtgdAssetCategory = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtgdAssetCategory");
            TextBox txtSanctionDate = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionDate");

            double dblPf_Prec = (Convert.ToString(txtProcessingFeePerc.Text) == "") ? 0 : Convert.ToDouble(txtProcessingFeePerc.Text);
            double dblLimit = (Convert.ToString(txtSanctionLimit.Text) == "") ? 0 : Convert.ToDouble(txtSanctionLimit.Text);
            Int32 iAssetCategory_ID = Convert.ToInt32(txtgdAssetCategory.SelectedValue);
            string strSanctionDate = Convert.ToString(txtSanctionDate.Text);
            FunPriCalculateProcessingFee("1", dblPf_Prec, 0, dblLimit, iAssetCategory_ID, strSanctionDate);
        }
        catch (Exception objException)
        {
            cvFunder.ErrorMessage = objException.Message;
            cvFunder.IsValid = false;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region "File Upload Events"

    protected void hyplnkDocView_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvLesseeDetails", strFieldAtt);
            Label lblPath = (Label)grvLesseeDetails.Rows[gRowIndex].FindControl("lblDocumentProof");

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(intFunderId.ToString(), false, 0);

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

    protected void btnBrowse_OnClick(object sender, EventArgs e)
    {
        try
        {
            TextBox txtSanctionNo = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionRefNo");
            UserControls_S3GAutoSuggest txtCustomerName = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtCustomerName");

            if (Convert.ToString(txtSanctionNo.Text) == "")
            {
                Utility.FunShowAlertMsg(this, "Enter the Sanction Ref. No");
                return;
            }

            if (Convert.ToInt32(txtCustomerName.SelectedValue) == 0)
            {
                Utility.FunShowAlertMsg(this, "Enter the Lessee Name");
                return;
            }

            /*
            DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
            DataView dvLessee = new DataView(dtLessee);
            dvLessee.RowFilter = "Lessee_Funder_ID = 0 and Document_Upload <> ''";
            dtLessee = dvLessee.ToTable();
             * */
            Label lblCurrentPath = (Label)grvLesseeDetails.FooterRow.FindControl("lblCurrentPath");
            HttpFileCollection hfc = Request.Files;

            if (ViewState["DocumentationPath"] == null || Convert.ToString(ViewState["DocumentationPath"]) == "")
            {
                Utility.FunShowAlertMsg(this, "Document path not available to Upload");
                return;
            }

            if (hfc.Count > 0)
            {
                HttpPostedFile hpf = hfc[0];
                if (hpf.ContentLength > 0)
                {
                    chkSelect.Enabled = true;
                    chkSelect.Checked = true;
                    chkSelect.ToolTip = flUpload.ToolTip = hdnSelectedPath.Value;
                    lblCurrentPath.Text = hpf.FileName;
                    Cache["Sanction_Documents_" + Convert.ToString(txtSanctionNo.Text) + "_" + (Convert.ToString(txtCustomerName.SelectedValue))] = hpf;
                }
            }
        }
        catch (Exception objException)
        {

        }
    }

    #endregion


    #endregion

    #region "Methods"

    private void FunPriSetPrefixSuffixLength()
    {
        try
        {
            txtForeClosureRate.SetDecimalPrefixSuffix(3, 2, false, false, "Fore Closure Rate(%)");
            txtDiscountRate.SetPercentagePrefixSuffix(2, 2, true, false, "Discount Rate(%)");
            txtProcessingFeePerc.SetDecimalPrefixSuffix(3, 2, false, false, "Processing Fee(%)");
            txtOverDueRate.SetDecimalPrefixSuffix(3, 2, false, false, "Over Due Rate");
            txtChqRtnChrgs.SetDecimalPrefixSuffix(5, 2, false, false, "Cheque Return Charges");

            txtUpfrontInt.SetDecimalPrefixSuffix(3, 2, false, false, "Upfront Interest(%)");
            txtNetDiscountRate.SetDecimalPrefixSuffix(3, 2, false, false, "Net Discount Rate(%)");
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriSetLesseeFooterSettings()
    {
        try
        {
            if (grvLesseeDetails.FooterRow != null)
            {
                TextBox txtSanctionDate = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionDate");
                TextBox txtExpiryDate = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdExpiryDate");
                TextBox txtSanctionLimit = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtSanctionLimit");
                CalendarExtender ceSanctionDate = (CalendarExtender)grvLesseeDetails.FooterRow.FindControl("ceSanctionDate");
                CalendarExtender ceExpiryDate = (CalendarExtender)grvLesseeDetails.FooterRow.FindControl("ceExpiryDate");

                ceExpiryDate.Format = ceSanctionDate.Format = strDateFormat;
                txtSanctionDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtSanctionDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtExpiryDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtExpiryDate.ClientID + "','" + strDateFormat + "',false,  true);");
                //txtSanctionLimit.SetDecimalPrefixSuffix(16, 0, true, false, "Sanction Limit");
            }
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
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            DataSet dsLov = Utility.GetDataset("S3G_ORG_GETFUNDERLOOKUP", Procparam);
            if (dsLov != null && dsLov.Tables.Count > 0)
            {
                ddlGLAccount.FillDataTable(dsLov.Tables[0], "ID", "Name", true);
                ddlAccountType.FillDataTable(dsLov.Tables[3], "ID", "Name", true);
                ddlPVCalcMethod.FillDataTable(dsLov.Tables[4], "ID", "Name", true);

                DataTable dtAddress = dsLov.Tables[1];
                DataTable dtSource = new DataTable();
                dtSource = new DataTable();
                if (dtAddress.Select("Category = 3").Length > 0)
                {
                    dtSource = dtAddress.Select("Category = 3").CopyToDataTable();
                }
                else
                {
                    dtSource = FunProAddAddrColumns(dtSource);
                }
                txtCommCountry.FillDataTable(dtSource, "Name", "Name", false);
                txtCorpCountry.FillDataTable(dtSource, "Name", "Name", false);

                ViewState["LesseeSettings"] = ViewState["DefaultLesseeSettings"] = dsLov.Tables[5];
                ViewState["DefaultLimitConsolidationDetails"] = dsLov.Tables[6];
                if (dsLov.Tables.Count > 7)
                {
                    ViewState["DocumentationPath"] = dsLov.Tables[7].Rows[0]["Document_Path"];
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Define Document Path Setup");
                }

                ddlSearchType.FillDataTable(dsLov.Tables[8], "ID", "Name", true);
            }
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }

    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        try
        {
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Category");

            return dt;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearDtls()
    {
        try
        {
            txtCommAdress.Text = txtCommCity.Text = txtCommCountry.Text = txtCommEmailID.Text = txtCommMobileNo.Text = txtCommPincode.Text =
            txtCommTAN.Text = txtCommTelephoneNo.Text = txtCommTIN.Text = txtCorpAddress.Text = txtCorpCity.Text = txtCorpCountry.Text = txtCorpEmailID.Text =
            txtCorpMobileNo.Text = txtCorpPincode.Text = txtCorpTAN.Text = txtCorpTelephoneNo.Text = txtCorpTIN.Text = txtDiscountRate.Text =
            txtFunderName.Text = txtNote.Text = txtFunderCode.Text = lblLimitFunderName.Text = txtShortName.Text = txtCorpPAN.Text =
            txtUpfrontInt.Text = txtNetDiscountRate.Text = string.Empty;

            chkDiscountProcessing.Checked =  chkUpfront.Checked = false;

            txtCorpCountry.SelectedValue = txtCorpState.SelectedValue = txtCommCountry.SelectedValue = txtCommState.SelectedValue = "0";

            ddlGLAccount.SelectedValue = "0";

            ViewState["BankDetails"] = ViewState["CustomerID"] = ViewState["LimitConsolidationDetails"] = null;
            FunPriBindGridDtls(1);

            ViewState["LesseeSettings"] = ((DataTable)ViewState["LesseeSettings"]).Clone();
            FunPriBindGridDtls(2);

            FunPriClearLesseeDtls();
            FunPriClearBankDtls();
            FunProClearCachedFiles();

            tcFunder.ActiveTabIndex = 0;

            txtCorpState.Clear(); txtCommState.Clear(); ddlLesseeName.Clear();

            txtSGSTin.Text = txtCGSTin.Text = TxtSGSTRegDate.Text = TxtCGSTRegDate.Text = "";
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriCopyAddress()
    {
        try
        {
            txtCommAdress.Text = Convert.ToString(txtCorpAddress.Text);
            txtCommCity.Text = Convert.ToString(txtCorpCity.Text);
            txtCommCountry.Text = Convert.ToString(txtCorpCountry.Text);
            txtCommEmailID.Text = Convert.ToString(txtCorpEmailID.Text);
            txtCommMobileNo.Text = Convert.ToString(txtCorpMobileNo.Text);
            txtCommPincode.Text = Convert.ToString(txtCorpPincode.Text);
            txtCommState.SelectedValue = Convert.ToString(txtCorpState.SelectedValue);
            txtCommState.SelectedText = Convert.ToString(txtCorpState.SelectedText);
            txtCommTAN.Text = Convert.ToString(txtCorpTAN.Text);
            txtCommTelephoneNo.Text = Convert.ToString(txtCorpTelephoneNo.Text);
            txtCommTIN.Text = Convert.ToString(txtCorpTIN.Text);
            txtSGSTin.Text = txtCGSTin.Text;
            TxtSGSTRegDate.Text = TxtCGSTRegDate.Text;
      
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearBankDtls()
    {
        try
        {
            txtBankBranch.Text = txtAccountNo.Text = txtBankCity.Text = txtBenificiaryName.Text = txtBankBranch.Text = txtMICRCode.Text =
            txtBankName.Text = txtIFSCCode.Text = "";
            ddlAccountType.SelectedValue = "0";
            chkDefaultAccount.Checked = chkActiveAccount.Checked = false;
            btnBankAdd.Enabled = true; btnBankModify.Enabled = false;
            strModifyAccountNo = string.Empty;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearLesseeDtls()
    {
        try
        {
            txtChqRtnChrgs.Text = txtCollateralDetails.Text = txtDiscountRate.Text = txtUpfrontInt.Text = txtNetDiscountRate.Text =
            txtForeClosureRate.Text = txtMiscCharges.Text = txtOverDueRate.Text = txtProcessingFee.Text = txtProcessingFeePerc.Text =
            txtRemarks.Text = txtTenor.Text = lblSearchSanctionNo.Text = txtTotalPF.Text = txtPFServiceTax.Text = txtPopupLimit.Text =
            lblSearchAssetCategoryID.Text = lblSerachSanctionDate.Text = lblSearchCustomerID.Text = lblEditAssetID.Text =
            lblSrchFndrDtlID.Text = string.Empty;

            chkDiscountProcessing.Checked = chkUpfront.Checked = false;

            ddlPVCalcMethod.SelectedValue = "0";
            ddlEndCustomer.Clear();
        }
        catch (Exception objException)
        {

            throw objException;
        }
    }

    private void FunPriBindGridDtls(Int32 iOption)
    {
        try
        {
            if (iOption == 1)                                                   //Bind Bank Details
            {
                grvBankDetails.DataSource = (DataTable)ViewState["BankDetails"];
                grvBankDetails.DataBind();
            }
            else if (iOption == 2)                                              //Bind Lessee Details
            {
                DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
                DataRow[] drLessee = dtLessee.Select("IS_Deleted=0");
                if (drLessee.Length > 0)
                {
                    dtLessee = drLessee.CopyToDataTable<DataRow>();
                }
                else
                {
                    dtLessee = (DataTable)ViewState["DefaultLesseeSettings"];
                    ViewState["LesseeSettings"] = dtLessee;
                }
                grvLesseeDetails.DataSource = dtLessee;
                grvLesseeDetails.DataBind();
                grvLesseeDetails.Rows[0].Visible = (Convert.ToString(dtLessee.Rows[0]["Sanction_Ref_No"]) == "") ? false : true;
                grvLesseeDetails.FooterRow.Visible = true;
                FunPriSetLesseeFooterSettings();
            }
            else if (iOption == 3)                                              //Bind Limit Consolidation Details
            {
                if (((DataTable)ViewState["LimitConsolidationDetails"]).Rows.Count == 0)
                {
                    ViewState["LimitConsolidationDetails"] = ViewState["DefaultLimitConsolidationDetails"];
                }
                grvLimitConsolidation.DataSource = (DataTable)ViewState["LimitConsolidationDetails"];
                grvLimitConsolidation.DataBind();
                grvLimitConsolidation.Rows[0].Visible = (Convert.ToString(((DataTable)ViewState["LimitConsolidationDetails"]).Rows[0]["Existing_sanction_ID"]) == "0") ? false : true;
                grvLimitConsolidation.FooterRow.Visible = true;
            }
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }

    private void FunPriLoadFunderDtls()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@FUNDER_ID", Convert.ToString(intFunderId));
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@USER_ID", Convert.ToString(intUserID));

            DataSet dsFunder = Utility.GetDataset("S3G_FUNDMGT_GETFUNDERDETIALS", Procparam);
            if (dsFunder != null)
            {
                txtFunderCode.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Funder_Code"]);
                txtCommAdress.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_Address"]);
                txtCommCity.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_City"]);
                txtCommCountry.SelectedItem.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_Country"]);
                txtCommEmailID.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_EMail"]);
                txtCommMobileNo.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_Mobile"]);
                txtCommPincode.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_Pincode"]);
                txtCommState.SelectedText = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_State"]);
                txtCommState.SelectedValue = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_State_ID"]);
                txtCommTAN.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_TAN"]);
                txtCommTelephoneNo.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_Telephone"]);
                txtCommTIN.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Comm_TIN"]);
                txtCorpAddress.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_Address"]);
                txtCorpCity.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_City"]);
                txtCorpCountry.SelectedItem.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_Country"]);
                txtCorpEmailID.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_EMail"]);
                txtCorpMobileNo.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_Mobile"]);
                txtCorpPincode.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_Pincode"]);
                txtCorpState.SelectedText = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_State"]);
                txtCorpState.SelectedValue = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_State_ID"]);
                txtCorpTAN.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_TAN"]);
                txtCorpTelephoneNo.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_Telephone"]);
                txtCorpTIN.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Perm_TIN"]);
                txtNote.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Note"]);
                txtFunderName.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Funder_Name"]);
                lblLimitFunderName.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Funder_Name"]);
                txtCorpPAN.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Corp_PAN"]);
                txtShortName.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["Short_Name"]);
                txtCorpCountry.SelectedItem.Value = txtCommCountry.SelectedItem.Value = "1";
                txtCGSTin.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["CGSTIN"]);
                TxtCGSTRegDate.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["CGST_reg_date"]);
                txtSGSTin.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["SGSTIN"]);
                TxtSGSTRegDate.Text = Convert.ToString(dsFunder.Tables[0].Rows[0]["SGST_reg_Date"]);
          
                ddlGLAccount.SelectedValue = Convert.ToString(dsFunder.Tables[0].Rows[0]["GL_Code"]);

                ViewState["BankDetails"] = dsFunder.Tables[2];
                //ViewState["LesseeSettings"] = dsFunder.Tables[1];
                ViewState["LimitConsolidationDetails"] = dsFunder.Tables[3];

                FunPriBindGridDtls(1);

                //FunPriBindGridDtls(2);

                FunPriBindGridDtls(3);
                FunPriBindBankHistory(dsFunder.Tables[4]);

                pnlBankDtls.Visible = (dsFunder.Tables[2].Rows.Count > 0) ? true : false;

                if (Convert.ToString(strMode) == "Q")
                {
                    //pnlLesseeInformation.Visible = (dsFunder.Tables[1].Rows.Count > 0) ? true : false;
                    pnlLimit.Visible = (dsFunder.Tables[3].Rows.Count > 0 && Convert.ToInt32(dsFunder.Tables[3].Rows[0]["Existing_Sanction_ID"]) > 0) ? true : false;
                }
                FunPriBindGrid();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadGridDtls(Int32 iOption)
    {
        try
        {
            if (iOption == 1)                           //Lessee Details
            {
                if (ViewState["LesseeSettings"] != null)
                {
                    DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
                    if (dtLessee.Rows.Count > 0)
                    {
                        FunPriBindGridDtls(2);
                        grvLesseeDetails.Rows[0].Visible = (Convert.ToString(dtLessee.Rows[0]["Sanction_Ref_No"]) == "") ? false : true;
                        grvLesseeDetails.FooterRow.Visible = true;
                    }
                }
            }
            else if (iOption == 2)                      //Consolidation Details
            {
                if (ViewState["LimitConsolidationDetails"] != null)
                {
                    DataTable dtcons = (DataTable)ViewState["LimitConsolidationDetails"];
                    if (dtcons.Rows.Count > 0)
                    {
                        FunPriBindGridDtls(3);
                        grvLimitConsolidation.Rows[0].Visible = (Convert.ToString(dtcons.Rows[0]["Existing_sanction_ID"]) == "0") ? false : true;
                        grvLimitConsolidation.FooterRow.Visible = true;
                    }
                }
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriBindBankHistory(DataTable dtBank)
    {
        try
        {
            pnlBankHistory.Visible = (dtBank.Rows.Count > 0) ? true : false;
            grvBankHistory.DataSource = dtBank;
            grvBankHistory.DataBind();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriAddConsolidation(Int64 iExistingSanctionID, string strExistSanctionNo, double dblBalanceLimit, double dblTransferAmt,
        Int64 iNewExistingSanctionID, string strNewSanctionNo)
    {
        try
        {
            DataTable dtCons = (DataTable)ViewState["LimitConsolidationDetails"];
            if (dtCons.Rows.Count > 0)
            {
                if (Convert.ToInt64(dtCons.Rows[0]["Existing_sanction_ID"]) == 0)
                {
                    dtCons.Rows.RemoveAt(0);
                    dtCons.AcceptChanges();
                }

                string strFilter = "Existing_sanction_ID = " + iExistingSanctionID;
                strFilter += "and New_Sanction_ID = " + iNewExistingSanctionID;
                strFilter += "and Consolidation_ID = 0";
                DataRow[] drConsChk = dtCons.Select(strFilter);

                if (drConsChk.Length > 0)
                {
                    Utility.FunShowAlertMsg(this, "Combination already exists");
                    return;
                }

                DataRow drCons = dtCons.NewRow();
                drCons["Consolidation_ID"] = 0;
                drCons["Existing_sanction_ID"] = iExistingSanctionID;
                drCons["Existing_sanction_No"] = strExistSanctionNo;
                drCons["Balance_Unit"] = dblBalanceLimit;
                drCons["Transfer_Amount"] = dblTransferAmt;
                drCons["New_Sanction_ID"] = iNewExistingSanctionID;
                drCons["New_Sanction_No"] = strNewSanctionNo;

                dtCons.Rows.Add(drCons);
                ViewState["LimitConsolidationDetails"] = dtCons;
                FunPriBindGridDtls(3);

                #region "HIDE"

                /*

                //Modify Exist Sanction Details
                DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
                DataRow[] drLessee = dtLessee.Select("Lessee_Funder_ID = " + iExistingSanctionID);
                drLessee[0]["Balance_Limit"] = Convert.ToDouble(drLessee[0]["Balance_Limit"]) - dblTransferAmt;
                drLessee[0]["Utilized_Limit"] = Convert.ToDouble(drLessee[0]["Utilized_Limit"]) + dblTransferAmt;
                dtLessee.AcceptChanges();

                //Modify New Sanction Details
                drLessee = dtLessee.Select("Lessee_Funder_ID = " + iNewExistingSanctionID);
                drLessee[0]["Balance_Limit"] = Convert.ToDouble(drLessee[0]["Balance_Limit"]) + dblTransferAmt;
                drLessee[0]["Limit"] = Convert.ToDouble(drLessee[0]["Limit"]) + dblTransferAmt;
                dtLessee.AcceptChanges();

                ViewState["LesseeSettings"] = dtLessee;
                FunPriBindGridDtls(2);
                 * */

                #endregion

                FunPriAssignVbl();
                Procparam.Add("@Option", "3");
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@Sanction_ID", Convert.ToString(iExistingSanctionID));
                Procparam.Add("@User_ID", Convert.ToString(intUserID));
                Procparam.Add("@NewSanction_ID", Convert.ToString(iNewExistingSanctionID));
                Procparam.Add("@Balance_Limit", Convert.ToString(dblBalanceLimit));
                Procparam.Add("@Transfer_Limit", Convert.ToString(dblTransferAmt));

                DataTable dtUpd = Utility.GetDefaultData("S3G_FMGT_GETSancConsDtl", Procparam);
                FunPriBindGrid();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnableDisableControls()
    {
        try
        {
            if (strMode == "Q")
            {
                btnClear.Enabled = btnSave.Enabled = false;
                txtFunderName.ReadOnly = txtNote.ReadOnly = txtCommAdress.ReadOnly = txtCommCity.ReadOnly = txtCommEmailID.ReadOnly =
                txtCommMobileNo.ReadOnly = txtCommPincode.ReadOnly = txtCommTAN.ReadOnly = txtCommTelephoneNo.ReadOnly = txtCommTIN.ReadOnly =
                txtCorpAddress.ReadOnly = txtCorpCity.ReadOnly = txtCorpEmailID.ReadOnly = txtCorpMobileNo.ReadOnly = txtCorpPincode.ReadOnly =
                txtCorpTAN.ReadOnly = txtCorpTelephoneNo.ReadOnly = txtCorpTIN.ReadOnly = txtCorpPAN.ReadOnly = txtShortName.ReadOnly = true;

                txtCommCountry.Enabled = txtCommState.Enabled = txtCorpState.Enabled = txtCorpCountry.Enabled = btnCopyAddress.Enabled =
                btnBankAdd.Enabled = btnBankClear.Enabled = btnBankModify.Enabled = chkDefaultAccount.Enabled = chkActiveAccount.Enabled = false;

                txtAccountNo.ReadOnly = txtBankBranch.ReadOnly = txtBankCity.ReadOnly = txtBankName.ReadOnly = txtBenificiaryName.ReadOnly = txtIFSCCode.ReadOnly =
                txtMICRCode.ReadOnly = true;

                ddlAccountType.Enabled = false;

                grvBankDetails.Enabled = false;

                if (Request.QueryString["Popup"] != null)
                {
                    btnCancel.Enabled = false;

                    //added by vinodha m to show sanction details in note creation
                    tcFunder.ActiveTabIndex = 1;
                }
            }
            else if (Convert.ToString(strMode) == "M")
            {
                btnClear.Enabled = false;
            }
            else if (Convert.ToString(strMode) == "C")
            {
                pnlLimit.Visible = pnlBankHistory.Visible = tbRSDetails.Enabled = false;
                ViewState["LimitConsolidationDetails"] = null;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriDisableFooter()
    {
        try
        {
            grvLimitConsolidation.FooterRow.Visible = false;
            grvLesseeDetails.FooterRow.Visible = false;
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }

    private void FunPriCalculateProcessingFee(string strOption, double dblPf_Prec, double dblpf_amt, double dblLimit, Int32 iAssetCategory_ID, string strSanctionDate)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", strOption);
            Procparam.Add("@ProcessingFeePerc", Convert.ToString(dblPf_Prec));
            Procparam.Add("@ProcessingFee", Convert.ToString(dblpf_amt));
            Procparam.Add("@Funder_Limit", Convert.ToString(dblLimit));
            Procparam.Add("@Asset_Category_ID", Convert.ToString(iAssetCategory_ID));
            if (Convert.ToString(strSanctionDate) != "")
                Procparam.Add("@Sanction_Date", Utility.StringToDate(strSanctionDate).ToString());
            Procparam.Add("@Location_ID", Convert.ToString(txtCorpState.SelectedValue));

            DataTable dtFunder = Utility.GetDefaultData("S3G_FNDMGT_GETPROCESSINGFEE", Procparam);

            if (dtFunder != null)
            {
                txtProcessingFee.Text = Convert.ToString(dtFunder.Rows[0]["Processing_Fee_Amount"]);
                txtProcessingFeePerc.Text = Convert.ToString(dtFunder.Rows[0]["Processing_Fee_Perc"]);
                txtPFServiceTax.Text = Convert.ToString(dtFunder.Rows[0]["PF_Service_Tax"]);
                txtTotalPF.Text = Convert.ToString(dtFunder.Rows[0]["Total_PF"]);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private string FunPriValidateAddress()
    {
        string strValidationMsg = "";
        try
        {
            TextBox txtCountry = txtCommCountry.FindControl("TextBox") as TextBox;
            if (Convert.ToString(txtCorpCountry.Text).Trim() == "")
            {
                strValidationMsg = strValidationMsg + rfvCorporateCountry.ErrorMessage + "<br>";
            }
            if (Convert.ToString(txtCountry.Text).Trim() == "")
            {
                strValidationMsg = strValidationMsg + rfvCommCountry.ErrorMessage + "<br>";
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return strValidationMsg;
    }

    protected void FunProUploadFiles(DataTable dtLessee, string strPath)
    {
        try
        {
            for (int i = 1; i <= dtLessee.Rows.Count; i++)
            {
                if (Cache["Sanction_Documents" + Convert.ToString(i)] != null)
                {
                    HttpPostedFile hpf = (HttpPostedFile)Cache["Sanction_Documents" + Convert.ToString(i)];

                    string strFolderName = @"\COMPANY" + intCompanyID.ToString();
                    string strFilePath = strPath + strFolderName;

                    if (!Directory.Exists(strFilePath))
                    {
                        Directory.CreateDirectory(strFilePath);
                    }
                    strFilePath += @"\" + System.IO.Path.GetFileName(hpf.FileName).Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + System.IO.Path.GetFileName(hpf.FileName).Split('.')[1].ToString();
                    hpf.SaveAs(strFilePath);

                    dtLessee.Rows[i - 1]["Document_Upload"] = strFilePath;
                    dtLessee.AcceptChanges();
                }
            }

            ViewState["LesseeSettings"] = dtLessee;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private string FunPriUpldFiles(string strSanctionNo, Int64 intCustomerID, string strPath)
    {
        try
        {
            HttpPostedFile hpf = (HttpPostedFile)Cache["Sanction_Documents_" + Convert.ToString(strSanctionNo) + "_" + (Convert.ToString(intCustomerID))];
            string strFolderName = @"\COMPANY" + intCompanyID.ToString();
            string strFilePath = strPath + strFolderName;

            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            strFilePath += @"\" + System.IO.Path.GetFileName(hpf.FileName).Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + System.IO.Path.GetFileName(hpf.FileName).Split('.')[1].ToString();

            if (File.Exists(strFilePath))
            {
                File.Delete(strFilePath);
            }

            hpf.SaveAs(strFilePath);
            Cache.Remove("Sanction_Documents_" + Convert.ToString(strSanctionNo) + "_" + Convert.ToString(intCustomerID));
            return strFilePath;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunProClearCachedFiles()
    {
        try
        {
            DataTable dtLessee = (DataTable)ViewState["LesseeSettings"];
            DataView dvLessee = new DataView(dtLessee);
            dvLessee.RowFilter = "Lessee_Funder_ID = 0 and Document_Upload <> ''";
            dtLessee = dvLessee.ToTable();
            for (int i = 1; i <= dtLessee.Rows.Count; i++)
            {
                if (Cache["Sanction_Documents" + Convert.ToString(i)] != null)
                {
                    Cache.Remove("Sanction_Documents" + Convert.ToString(i));
                }
            }
        }
        catch (Exception ojException)
        {
            throw ojException;
        }
    }

    private bool FunPriCheckDuplicateSanction(DataTable dtLessee, string strSanctionNo, Int64 intCustomerID, Int32 intAssetCategoryID, string strLesseeMode, Int32 intEditAssetID)
    {
        bool blnRslt = true;
        try
        {
            string strFilter = string.Empty;
            strFilter = "Sanction_Ref_No = '" + Convert.ToString(strSanctionNo) + "' and Customer_ID = " + Convert.ToInt64(intCustomerID) + " and Asset_Category_ID = 0";

            DataRow[] drDuplicate = dtLessee.Select(strFilter);
            if (drDuplicate.Length > 0)
            {
                return blnRslt = false;
            }

            strFilter = "Sanction_Ref_No = '" + Convert.ToString(strSanctionNo) + "' and Customer_ID = " + Convert.ToInt64(intCustomerID) + " and Asset_Category_ID = " + Convert.ToInt32(intAssetCategoryID);
            drDuplicate = dtLessee.Select(strFilter);
            if (drDuplicate.Length > 0)
            {
                return blnRslt = false;
            }

            if (Convert.ToInt32(intAssetCategoryID) == 0 && strLesseeMode == "C")
            {
                strFilter = "Sanction_Ref_No = '" + Convert.ToString(strSanctionNo) + "' and Customer_ID = " + Convert.ToInt64(intCustomerID);
                drDuplicate = dtLessee.Select(strFilter);
                if (drDuplicate.Length > 0)
                {
                    return blnRslt = false;
                }
            }
            else if (Convert.ToInt32(intAssetCategoryID) == 0 && strLesseeMode == "E")
            {
                strFilter = "Sanction_Ref_No = '" + Convert.ToString(strSanctionNo).Trim() + "'";
                strFilter += " and Customer_ID = " + Convert.ToInt64(intCustomerID) + " and Asset_Category_ID <> " + Convert.ToInt32(intEditAssetID);
                drDuplicate = dtLessee.Select(strFilter);
                if (drDuplicate.Length > 0)
                {
                    return blnRslt = false;
                }
            }
        }
        catch (Exception objException)
        {
            blnRslt = false;
            throw objException;
        }
        return blnRslt;
    }

    private void FunPriLoadApvdSanc()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OPTION", "9");
            Procparam.Add("@Funder_ID", Convert.ToString(intFunderId));
            Procparam.Add("@CustomerID", Convert.ToString(intCustomerID));

            DataTable dtSanction = Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam);

            if (dtSanction.Rows.Count > 0)
            {
                ViewState["LesseeSettings"] = dtSanction;
                grvLesseeDetails.DataSource = dtSanction;
                grvLesseeDetails.DataBind();
                grvLesseeDetails.FooterRow.Visible = false;
            }
            else
            {
                grvLesseeDetails.DataSource = null;
                grvLesseeDetails.DataBind();
                Utility.FunShowAlertMsg(this, "Sanction Detail not available for this selected Combination");
                pnlLesseeInformation.Visible = false;
            }
            pnlLimit.Visible = false;
            tcFunder.Tabs[0].Enabled = tcFunder.Tabs[2].Enabled = tcFunder.Tabs[3].Enabled = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriInsertTmpSancDtl(Int32 intIs_New, Int32 intIs_Update, Int64 intFunderLesseeID)
    {
        try
        {
            TextBox txtSanctionNo = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionRefNo");
            TextBox txtSanctionDate = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionDate");
            TextBox txtExpiryDate = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdExpiryDate");
            TextBox txtSanctionLimit = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtSanctionLimit");
            UserControls_S3GAutoSuggest txtCustomerName = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtCustomerName");
            UserControls_S3GAutoSuggest txtgdAssetCategory = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtgdAssetCategory");
            Label lblCurrentPath = (Label)grvLesseeDetails.FooterRow.FindControl("lblCurrentPath");
            string strUpldPath = string.Empty;
            if (Convert.ToString(lblCurrentPath.Text) != "")
            {
                string strpath = string.Empty;
                if (ViewState["DocumentationPath"] != null || Convert.ToString(ViewState["DocumentationPath"]) != "")
                {
                    strpath = Convert.ToString(ViewState["DocumentationPath"]);
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Document path not available to Upload");
                    return;
                }
                strUpldPath = FunPriUpldFiles(Convert.ToString(txtSanctionNo.Text), Convert.ToInt64(txtCustomerName.SelectedValue), strpath);
            }


            ObjSanc_DT = new FundMgtServices.S3G_FND_SanctionDtlDataTable();
            ObjRow = ObjSanc_DT.NewS3G_FND_SanctionDtlRow();

            ObjRow.Option = 1;
            ObjRow.Asset_Category_ID = Convert.ToInt32(txtgdAssetCategory.SelectedValue);
            ObjRow.Balance_Limit = Convert.ToDecimal(txtSanctionLimit.Text);
            ObjRow.Cheque_Rtn_Charge = (Convert.ToString(txtChqRtnChrgs.Text) == "") ? 0 : Convert.ToDecimal(txtChqRtnChrgs.Text);
            ObjRow.Collateral_Details = Convert.ToString(txtCollateralDetails.Text);
            ObjRow.Created_By = Convert.ToInt32(intUserID);
            ObjRow.Customer_ID = Convert.ToInt64(txtCustomerName.SelectedValue);
            ObjRow.Discount_Rate = (Convert.ToString(txtDiscountRate.Text) == "") ? 0 : Convert.ToDecimal(txtDiscountRate.Text);
            ObjRow.Document_Path = strUpldPath;
            ObjRow.EndUser_ID = Convert.ToInt32(ddlEndCustomer.SelectedValue);
            ObjRow.Expiry_Date = Utility.StringToDate(txtExpiryDate.Text);
            ObjRow.ForeClosure_Rate = (Convert.ToString(txtForeClosureRate.Text) == "") ? 0 : Convert.ToDecimal(txtForeClosureRate.Text);
            ObjRow.Funder_Detail_ID = intFunderLesseeID;
            ObjRow.Funder_ID = Convert.ToInt64(intFunderId);
            ObjRow.Is_Delete = 0;
            ObjRow.Is_New = intIs_New;
            ObjRow.Is_Update = intIs_Update;
            ObjRow.Misc_Charges = (Convert.ToString(txtMiscCharges.Text) == "") ? 0 : Convert.ToDecimal(txtMiscCharges.Text);
            ObjRow.ODI_Rate = (Convert.ToString(txtOverDueRate.Text) == "") ? 0 : Convert.ToDecimal(txtOverDueRate.Text);
            ObjRow.Processing_Fee_Amt = (Convert.ToString(txtProcessingFee.Text) == "") ? 0 : Convert.ToDecimal(txtProcessingFee.Text);
            ObjRow.Processing_Fee_Perc = (Convert.ToString(txtProcessingFeePerc.Text) == "") ? 0 : Convert.ToDecimal(txtProcessingFeePerc.Text);
            ObjRow.Processing_Fee_ST = (Convert.ToString(txtPFServiceTax.Text) == "") ? 0 : Convert.ToDecimal(txtPFServiceTax.Text);
            ObjRow.PV_Method_ID = Convert.ToInt32(ddlPVCalcMethod.SelectedValue);
            ObjRow.Remarks = Convert.ToString(txtRemarks.Text);
            ObjRow.Sanction_Date = Utility.StringToDate(txtSanctionDate.Text);
            ObjRow.Sanction_Limit = Convert.ToDecimal(txtSanctionLimit.Text);
            ObjRow.Sanction_No = Convert.ToString(txtSanctionNo.Text);
            ObjRow.Status_ID = 1;
            ObjRow.Tenure = (Convert.ToString(txtTenor.Text) == "") ? 0 : Convert.ToInt32(txtTenor.Text);
            ObjRow.Location_ID = Convert.ToInt32(txtCorpState.SelectedValue);

            ObjRow.UpfrontInterest = (Convert.ToString(txtUpfrontInt.Text) == "") ? 0 : Convert.ToDecimal(txtUpfrontInt.Text);
            ObjRow.NetDiscountRate = (Convert.ToString(txtNetDiscountRate.Text) == "") ? 0 : Convert.ToDecimal(txtNetDiscountRate.Text);
            ObjRow.DiscProcessingFee = chkDiscountProcessing.Checked;
            ObjRow.DiscUpfrontInterest = chkUpfront.Checked;

            ObjSanc_DT.AddS3G_FND_SanctionDtlRow(ObjRow);
            objFundMgtServiceClient = new FunderMgtServiceReference.FundMgtServiceClient();

            int iErrorCode = objFundMgtServiceClient.FunPubInsertTmpSancDtl(ObjSerMode, ClsPubSerialize.Serialize(ObjSanc_DT, ObjSerMode));
            switch (iErrorCode)
            {
                case 0:
                    FunPriClearLesseeDtls();
                    FunPriSetLesseeFooterSettings();
                    FunPriBindGrid();
                    break;
                case 1:
                    Utility.FunShowAlertMsg(this, "Duplicate entries are not allowed");
                    break;
                case 2:
                    Utility.FunShowAlertMsg(this, "Discount Rate should not be blank");
                    break;
                default:
                    Utility.FunShowAlertMsg(this, "Error Occured");
                    break;
            }

        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriAssignVbl()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadAdditionalInfo()
    {
        try
        {
            TextBox txtSanctionRefNo = (TextBox)grvLesseeDetails.FooterRow.FindControl("txtgdSanctionRefNo");
            UserControls_S3GAutoSuggest txtCustomerName = (UserControls_S3GAutoSuggest)grvLesseeDetails.FooterRow.FindControl("txtCustomerName");
            FunPriClearLesseeDtls();

            if (Convert.ToString(txtSanctionRefNo.Text) == "")
            {
                return;
            }

            if (Convert.ToString(txtCustomerName.SelectedText) == "" || Convert.ToInt64(txtCustomerName.SelectedValue) == 0)
            {
                return;
            }

            FunPriAssignVbl();
            Procparam.Add("@Option", "2");
            Procparam.Add("@Funder_ID", Convert.ToString(intFunderId));
            Procparam.Add("@Customer_ID", Convert.ToString(txtCustomerName.SelectedValue));
            Procparam.Add("@Sanction_No", Convert.ToString(txtSanctionRefNo.Text));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));

            DataTable dtAddtInfo = Utility.GetDefaultData("S3G_FMGT_GetSanctionDtl", Procparam);

            if (dtAddtInfo.Rows.Count > 0)
            {
                ddlPVCalcMethod.SelectedValue = Convert.ToString(dtAddtInfo.Rows[0]["PV_Calculation_Method_ID"]);
                ddlEndCustomer.SelectedText = Convert.ToString(dtAddtInfo.Rows[0]["End_Customer_Name"]);
                ddlEndCustomer.SelectedValue = Convert.ToString(dtAddtInfo.Rows[0]["End_Customer_ID"]);
                txtDiscountRate.Text = (Convert.ToInt32(dtAddtInfo.Rows[0]["Discount_Rate"]) > 0) ? Convert.ToString(dtAddtInfo.Rows[0]["Discount_Rate"]) : "";
                txtProcessingFee.Text = Convert.ToString(dtAddtInfo.Rows[0]["Processing_Fee"]);
                txtProcessingFeePerc.Text = Convert.ToString(dtAddtInfo.Rows[0]["Processing_Fee_Perc"]);
                txtForeClosureRate.Text = Convert.ToString(dtAddtInfo.Rows[0]["ForeClosure_Rate"]);
                txtMiscCharges.Text = Convert.ToString(dtAddtInfo.Rows[0]["Misc_Charges"]);
                txtTenor.Text = (Convert.ToString(dtAddtInfo.Rows[0]["Tenor"]) != "0") ? Convert.ToString(dtAddtInfo.Rows[0]["Tenor"]) : "";
                txtChqRtnChrgs.Text = Convert.ToString(dtAddtInfo.Rows[0]["Cheque_Return_Charges"]);
                txtCollateralDetails.Text = Convert.ToString(dtAddtInfo.Rows[0]["Collateral_Dtls"]);
                txtRemarks.Text = Convert.ToString(dtAddtInfo.Rows[0]["Remarks"]);
                txtOverDueRate.Text = Convert.ToString(dtAddtInfo.Rows[0]["Over_Due_Rate"]);
                txtPFServiceTax.Text = Convert.ToString(dtAddtInfo.Rows[0]["PF_Service_Tax"]);
                txtTotalPF.Text = Convert.ToString(dtAddtInfo.Rows[0]["Total_PF"]);

                txtUpfrontInt.Text = Convert.ToString(dtAddtInfo.Rows[0]["Upfront_Int"]);
                txtNetDiscountRate.Text = Convert.ToString(dtAddtInfo.Rows[0]["Net_Discount_Rate"]);
                chkDiscountProcessing.Checked = Convert.ToBoolean(dtAddtInfo.Rows[0]["Discount_Processing"]);
                chkUpfront.Checked = Convert.ToBoolean(dtAddtInfo.Rows[0]["Discount_Upfront"]);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadNoteSancDtl()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OPTION", "11");
            Procparam.Add("@Note_ID", Convert.ToString(intNoteID));

            DataTable dtSanction = Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam);

            if (dtSanction.Rows.Count > 0)
            {
                grvLesseeDetails.DataSource = dtSanction;
                grvLesseeDetails.DataBind();
                grvLesseeDetails.FooterRow.Visible = false;
            }
            pnlLimit.Visible = false;
            tcFunder.Tabs[0].Enabled = tcFunder.Tabs[2].Enabled = tcFunder.Tabs[3].Enabled = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriLoadFundRSSancDtl()
    {
        try
        {
            lblSummaryErrMsg.InnerText = "";
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPagingSummary.ProCompany_ID = intCompanyID;
            ObjPagingSummary.ProUser_ID = intUserID;
            ObjPagingSummary.ProTotalRecords = intTotalRecords;
            ObjPagingSummary.ProCurrentPage = ProPageNumRWSummary;
            ObjPagingSummary.ProPageSize = ProPageSizeRWSummary;
            ObjPagingSummary.ProSearchValue = "";
            ObjPagingSummary.ProOrderBy = "";
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Funder_ID", Convert.ToString(intFunderId));
            Procparam.Add("@Option", "1");

            if (Convert.ToInt32(ddlSearchType.SelectedValue) > 0 && Convert.ToString(txtSearchText.Text).Trim() != "")
            {
                Procparam.Add("@Filter_Type", Convert.ToString(ddlSearchType.SelectedValue));
                Procparam.Add("@Filter_Text", Convert.ToString(txtSearchText.Text).Trim());
            }

            grvRSDetail.BindGridView("S3G_FMGT_GetRSSancDtl", Procparam, out intTotalRecords, ObjPagingSummary, out bIsNewRow);
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvRSDetail.Rows[0].Visible = false;
            }

            ucCustomPagingSummary.Navigation(intTotalRecords, ProPageNumRWSummary, ProPageSizeRWSummary);
            ucCustomPagingSummary.setPageSize(ProPageSizeRWSummary);
        }
        catch (Exception ObjException)
        {
            lblSummaryErrMsg.InnerText = ObjException.Message;
        }
    }

    private String FunPriFormXMLBankDtl()
    {
        String strXML = string.Empty;
        StringBuilder strBankDtl = new StringBuilder();
        try
        {
            if (ViewState["BankDetails"] != null)
            {
                DataTable dtBankDtl = (DataTable)ViewState["BankDetails"];
                strBankDtl.Append("<Root>");
                foreach (DataRow drow in dtBankDtl.Rows)
                {
                    strBankDtl.Append("<Details ");
                    strBankDtl.Append(" BANK_ID = '" + Convert.ToString(drow["Bank_ID"]) + "'");
                    strBankDtl.Append(" BANK_NAME = '" + Convert.ToString(drow["Bank_Name"]) + "'");
                    strBankDtl.Append(" ACCOUNT_TYPE_ID = '" + Convert.ToString(drow["Account_Type_ID"]) + "'");
                    strBankDtl.Append(" ACCOUNT_TYPE_DESC = '" + Convert.ToString(drow["Account_Type_Desc"]) + "'");
                    strBankDtl.Append(" BENIFICIARY_NAME = '" + Convert.ToString(drow["Benificiary_name"]) + "'");
                    strBankDtl.Append(" ACCOUNT_NO = '" + Convert.ToString(drow["Account_No"]) + "'");
                    strBankDtl.Append(" CITY = '" + Convert.ToString(drow["City"]) + "'");
                    strBankDtl.Append(" BRANCH = '" + Convert.ToString(drow["Branch"]) + "'");
                    strBankDtl.Append(" MICR_CODE = '" + Convert.ToString(drow["MICR_Code"]) + "'");
                    strBankDtl.Append(" IFSC = '" + Convert.ToString(drow["IFSC"]) + "'");
                    strBankDtl.Append(" IS_TRANSACTION = '" + Convert.ToString(drow["Is_Transaction"]) + "'");
                    strBankDtl.Append(" IS_DEFAULTACCOUNT = '" + Convert.ToString(drow["Is_DefaultAccount"]) + "'");
                    strBankDtl.Append(" IS_MODIFY = '" + Convert.ToString(drow["IS_Modify"]) + "'");
                    strBankDtl.Append(" IS_ACTIVE = '" + Convert.ToString(drow["Is_Active"]) + "'");
                    strBankDtl.Append(" />");
                }
                strBankDtl.Append("</Root>");
                strXML = Convert.ToString(strBankDtl);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return strXML;
    }

    #region Page Methods

    private void FunPriBindGrid()
    {
        try
        {
            lblPagingErrorMessage.InnerText = "";
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = "";
            ObjPaging.ProOrderBy = "";
            Procparam = new Dictionary<string, string>();
            if (Convert.ToString(ddlLesseeName.SelectedValue) != "" && Convert.ToInt32(ddlLesseeName.SelectedValue) > 0)
                Procparam.Add("@Customer_ID", Convert.ToString(ddlLesseeName.SelectedValue));
            Procparam.Add("@Funder_ID", Convert.ToString(intFunderId));
            Procparam.Add("@Option", "1");
            Procparam.Add("@Is_ZeroChk", (chkZeroRcrd.Checked == true) ? "1" : "0");

            grvLesseeDetails.BindGridView("S3G_FMGT_GetSanctionDtl", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            DataTable dtSanctionDtl = ((DataView)grvLesseeDetails.DataSource).ToTable();
            ViewState["LesseeSettings"] = dtSanctionDtl;

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvLesseeDetails.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            if (strMode == "Q")
            {
                grvLesseeDetails.FooterRow.Visible = false;
            }

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblPagingErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblPagingErrorMessage.InnerText = ex.Message;
        }
    }

    #region Paging and Searching Methods For Grid

    private void FunPriGetSearchValue()
    {
        try
        {
            arrSearchVal = grvLesseeDetails.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearSearchValue()
    {
        try
        {
            grvLesseeDetails.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriSetSearchValue()
    {
        try
        {
            grvLesseeDetails.FunPriSetSearchValue(arrSearchVal);
        }
        catch (Exception objException)
        {
            throw objException;
        }
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

            //for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            //{
            //    if (arrSearchVal[iCount].ToString() != "")
            //    {
            //        strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
            //    }
            //}

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvLesseeDetails.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #endregion

    #endregion

    #region "WEB METHODS"

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"]));
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@OPTION", "1");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetCategoryList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"]));
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@OPTION", "2");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSanctionNoList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"]));
        Procparam.Add("@Funder_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestFunderID"]));
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@OPTION", "3");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetNewSanctionNoList(String prefixText, int count)
    {
        UserControls_S3GAutoSuggest ddlSanctionNo = (UserControls_S3GAutoSuggest)obj_Page.grvLimitConsolidation.FooterRow.FindControl("ddlSanctionNo");
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"]));
        Procparam.Add("@Funder_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestFunderID"]));
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@ExistingSanction_ID", Convert.ToString(ddlSanctionNo.SelectedValue));
        Procparam.Add("@OPTION", "5");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetStateCmnList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"]));
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@OPTION", "6");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetEndCustomerLst(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"]));
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@CustomerID", Convert.ToString(obj_Page.lblSearchCustomerID.Text));
        Procparam.Add("@OPTION", "7");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));

        return suggetions.ToArray();
    }

    #endregion

    protected void btnBrowse1_Click(object sender, EventArgs e)
    {
        try
        {
            int intRowIndex = Utility.FunPubGetGridRowID("grvLesseeDetails", ((Button)sender).ClientID);
            TextBox txtEditSanctionNo = (TextBox)grvLesseeDetails.Rows[intRowIndex].FindControl("txtEditSanctionNo");

            HttpFileCollection hfc = Request.Files;
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile hpf = hfc[i];
                if (hpf.ContentLength > 0)
                {
                    FileUpload flUpload = (FileUpload)grvLesseeDetails.Rows[intRowIndex].FindControl("flUpload1");
                    //Label lblActualPath = (Label)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("lblActualPath");
                    TextBox txtFileupld = (TextBox)grvLesseeDetails.Rows[intRowIndex].FindControl("txtFileupld1");
                    txtFileupld.Text = hpf.FileName;
                    Cache["Sanction_Documents_" + Convert.ToString(txtEditSanctionNo.Text) + "_" + (Convert.ToString(hdnCustomerID.Value))] = hpf;
                }
            }
        }
        catch (Exception objException)
        {
        }
    }
}