#region DETAILS

//CREATED BY    : VINODHA M
//CREATED DATE  : DEC 22,2014
//DESCRIPTION   : RS Charge Maintenance Against Invoices

#endregion

#region NAMESPACES

using AjaxControlToolkit;
using S3GBusEntity;
using S3GBusEntity.LoanAdmin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

public partial class LoanAdmin_S3GLoanAd_RSChargeMain_Add : ApplyThemeForProject
{

    #region Intialization

    SerializationMode SerMode = SerializationMode.Binary;
    UserInfo ObjUserInfo = null;

    int intErrCode = 0;
    int intErrCount = 0;
    int intRSCMID = 0;
    int intUserId = 0;
    int intCompanyID = 0;
    Int64 F17_Count=0;

    bool bClearList = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;

    string strMode = string.Empty;
    string strFormType = string.Empty;
    string strDateFormat = string.Empty;
    string Loc_ID = string.Empty;
    string Tranche_ID = string.Empty;
    string Cust_ID = string.Empty;
    string Fund_ID = string.Empty;
    string strXMLRSCMDet = null;
    string strProcName = null;
    StringBuilder strbRSCMDet = new StringBuilder();

    string strRedirectPage = "../LoanAdmin/S3GLoanAd_RSChargeMain_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAd_RSChargeMain_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAd_RSChargeMain_View.aspx';";

    static string strPageName = "RS Charge Maintenance";

    DataTable dtRSCM = new DataTable();
    DataTable dtSaveRSCM = new DataTable();
    DataSet dsRSCM = new DataSet();

    Dictionary<string, string> dictparam;
    public static String PA_SA_Ref_ID;

    public static LoanAdmin_S3GLoanAd_RSChargeMain_Add obj_Page;
    PagingValues ObjPaging = new PagingValues();
    S3GSession ObjS3GSession = new S3GSession();
    LoanAdminMgtServicesReference.LoanAdminMgtServicesClient ObjRSChargeMainClient;
    LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable ObjS3G_LOANAD_RS_Charge_MgmtDataTable = new LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable();


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
        FunPriBindAssetPopUpGrid(PA_SA_Ref_ID);
    }

    #endregion

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            #region Paging Config

            ProPageNumRW = 1;
            TextBox txtPageSize = (TextBox)ucPopUpPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucPopUpPaging.callback = obj;
            ucPopUpPaging.ProPageNumRW = ProPageNumRW;
            ucPopUpPaging.ProPageSizeRW = ProPageSizeRW;

            #endregion

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

            strDateFormat = ObjS3GSession.ProDateFormatRW;

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            if (Request.QueryString["qsRSCMID"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsRSCMID"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intRSCMID = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }

            txthF8FilingDate.Attributes.Add("onblur", "fnDoDate(this,'" + txthF8FilingDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txthF17ReleasingDate.Attributes.Add("onblur", "fnDoDate(this,'" + txthF17ReleasingDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txthF17nocdate.Attributes.Add("onblur", "fnDoDate(this,'" + txthF17nocdate.ClientID + "','" + strDateFormat + "',false,  false);");

            if (!IsPostBack)
            {
                loblist();
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (((intRSCMID > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if (((intRSCMID > 0)) && (strMode == "Q"))
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
                Label lblName = (Label)S3GFunderAddress.FindControl("lblCustomerName");
                lblName.Text = "Funder Name";
                PA_SA_Ref_ID = string.Empty;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region PAGE METHODS

    private void FunPubBindGridRSCM(DataTable dtRSCM)
    {
        try
        {
            gvRSChargeMaintenance.DataSource = dtRSCM;
            ViewState["RSCM_Details"] = dtRSCM;
            gvRSChargeMaintenance.DataBind();
            if (dtRSCM.Rows.Count > 0)
            {
                if (dtRSCM.Rows[0]["SNO"].ToString().Equals("0"))
                {
                    gvRSChargeMaintenance.Rows[0].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriInsertRSCMDataTable()
    {
        try
        {
            DataRow drEmptyRow;
            dtRSCM = FunPubGetRSCMDataTable();

            if (dtRSCM.Rows.Count == 0)
            {
                drEmptyRow = dtRSCM.NewRow();
                drEmptyRow["SNO"] = "0";
                dtRSCM.Rows.Add(drEmptyRow);
            }

            if (dtRSCM.Rows.Count > 1)
            {
                if (dtRSCM.Rows[0]["SNO"].Equals("0"))
                {
                    dtRSCM.Rows[0].Delete();
                }
            }

            ViewState["RSCM_Details"] = dtRSCM;

            dtRSCM = FunPubGetRSCMDataTable();
            FunPubBindGridRSCM(dtRSCM);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable FunPubGetRSCMDataTable()
    {
        try
        {
            if (ViewState["RSCM_Details"] == null)
            {
                dtRSCM = new DataTable();
                dtRSCM.Columns.Add("SNO");
                dtRSCM.Columns.Add("Charge_Mgmt_Dtl_ID");
                dtRSCM.Columns.Add("RSNO");
                dtRSCM.Columns.Add("PA_SA_Ref_ID");
                dtRSCM.Columns.Add("ORG_FILINGDATE");
                dtRSCM.Columns.Add("F8FilingDueDate");
                dtRSCM.Columns.Add("F8FilingDate");
                dtRSCM.Columns.Add("F8ChargeID");
                dtRSCM.Columns.Add("F8SRNNO");
                dtRSCM.Columns.Add("PVAmount");
                dtRSCM.Columns.Add("F8Remarks");
                dtRSCM.Columns.Add("F8CLBApplicable");
                dtRSCM.Columns.Add("ORG_RELEASINGDATE");
                dtRSCM.Columns.Add("F17ReleaseDueDate");
                dtRSCM.Columns.Add("F17ReleaseDate");
                dtRSCM.Columns.Add("F17ChargeID");
                dtRSCM.Columns.Add("F17SRNNO");
                dtRSCM.Columns.Add("F17Remarks");
                dtRSCM.Columns.Add("F17CLBApplicable");
                ViewState["RSCM_Details"] = dtRSCM;
            }
            dtRSCM = (DataTable)ViewState["RSCM_Details"];
            return dtRSCM;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode                
                btnSave.Enabled = false;
                txthF8FilingDate.Enabled = false;
                hCalendarExtender1.Enabled = false;
                txthF8ChargeID.Enabled = false;
                txthF8SRNNO.Enabled = false;
                txthPVAmount.Enabled = false;
                hchbF8CLBApplicable.Enabled = false;
                txthF8Remarks.Enabled = false;
                btnF8Go.Enabled = false;
                btnF17Go.Enabled = false;
                txthF17nocdate.Enabled = false;
                hcalext3.Enabled = false;
                txthF17ReleasingDate.Enabled = false;
                hCalendarExtender2.Enabled = false;
                txthF17ChargeID.Enabled = false;
                txthF17SRNNO.Enabled = false;
                hchbF17CLBApplicable.Enabled = false;
                txthF17Remarks.Enabled = false;
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                break;

            case 1: // Modify Mode

                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                ddlLineOfBusiness.Enabled = ddlCustomerName.Enabled = ddlTranche.Enabled = false;
                btnClear.Enabled = false;
                btnF17Go.Enabled = false;
                txthF17nocdate.Enabled = false;
                txthF17ReleasingDate.Enabled = false;
                hCalendarExtender2.Enabled = false;
                txthF17ChargeID.Enabled = false;
                txthF17SRNNO.Enabled = false;
                hchbF17CLBApplicable.Enabled = false;
                txthF17Remarks.Enabled = false;
                FunGetRSChargeMainDetails();
                break;

            case -1:// Query Mode                                
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                ddlLineOfBusiness.Enabled = false;
                ddlCustomerName.Enabled = false;
                ddlTranche.Enabled = false;
                //gvRSChargeMaintenance.Enabled = false;
                FunGetRSChargeMainDetails();
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                if (bClearList)
                {
                    //ddlLocation.ReadOnly = true;
                    ddlTranche.ReadOnly = true;
                }
                txthF8FilingDate.Enabled = false;
                hCalendarExtender1.Enabled = false;
                txthF8ChargeID.Enabled = false;
                txthF8SRNNO.Enabled = false;
                txthPVAmount.Enabled = false;
                hchbF8CLBApplicable.Enabled = false;
                txthF8Remarks.Enabled = false;
                btnF8Go.Enabled = false;
                btnF17Go.Enabled = false;
                txthF17nocdate.Enabled = false;
                txthF17ReleasingDate.Enabled = false;
                hCalendarExtender2.Enabled = false;
                txthF17ChargeID.Enabled = false;
                txthF17SRNNO.Enabled = false;
                hchbF17CLBApplicable.Enabled = false;
                txthF17Remarks.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                break;
        }
    }

    private void loblist()
    {
        try
        {
            dictparam = new Dictionary<string, string>();
            dictparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@Is_Active", "1");
            if (intRSCMID == 0)
            {
                dictparam.Add("@User_ID", intUserId.ToString());
            }
            dictparam.Add("@Program_ID", "294");
            ddlLineOfBusiness.BindDataTable(SPNames.LOBMaster, dictparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLineOfBusiness.Items.RemoveAt(0);
            ddlLineOfBusiness.SelectedValue = "3";
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    private DataTable FunPubGetCustFundIDS(string Tranche_ID)
    {
        try
        {
            dictparam = new Dictionary<string, string>();
            dictparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@User_ID", Convert.ToString(intUserId));
            dictparam.Add("@Tranche_ID", Convert.ToString(ddlTranche.SelectedValue));
            dsRSCM = Utility.GetDataset("S3G_LAD_RSCM_GetCustFundID", dictparam);
            dtRSCM = dsRSCM.Tables[0];
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        return dtRSCM;
    }

    /// <summary>
    /// This method is used to load the customer address based on the customer control customer id
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPriGetCustomerDetails(string Cust_ID)
    {
        try
        {
            if (!String.IsNullOrEmpty(Cust_ID))
            {
                dictparam = new Dictionary<string, string>();
                dictparam.Add("@CustomerID", Cust_ID.ToString());
                dictparam.Add("@CompanyID", intCompanyID.ToString());
                System.Data.DataTable dtCustomer = Utility.GetDefaultData("S3G_Get_Exist_Customer_Details_Enquiry_Updation", dictparam);
                TextBox txtName = (TextBox)S3GCustomerAddress.FindControl("txtCustomerName");
                txtName.Text = dtCustomer.Rows[0]["Customer_Code"].ToString();
                S3GCustomerAddress.SetCustomerDetails(dtCustomer.Rows[0]["Customer_Code"].ToString(),
                        dtCustomer.Rows[0]["comm_Address1"].ToString() + "\n" +
                 dtCustomer.Rows[0]["comm_Address2"].ToString() + "\n" +
                dtCustomer.Rows[0]["comm_city"].ToString() + "\n" +
                dtCustomer.Rows[0]["comm_state"].ToString() + "\n" +
                dtCustomer.Rows[0]["comm_country"].ToString() + "\n" +
                dtCustomer.Rows[0]["comm_pincode"].ToString(), dtCustomer.Rows[0]["Customer_Name"].ToString(), dtCustomer.Rows[0]["Comm_Telephone"].ToString(),
                dtCustomer.Rows[0]["Comm_mobile"].ToString(),
                dtCustomer.Rows[0]["comm_email"].ToString(), dtCustomer.Rows[0]["comm_website"].ToString());
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriGetFunderDetails(string Fund_ID)
    {
        try
        {
            if (!String.IsNullOrEmpty(Fund_ID))
            {
                dictparam = new Dictionary<string, string>();
                dictparam.Add("@ID", Fund_ID.ToString());
                dictparam.Add("@Company_ID", intCompanyID.ToString());
                dictparam.Add("@TypeID", "150");
                DataTable dtVendorDet = Utility.GetDefaultData("S3G_LOANAD_GETCustomerorEntityDetails", dictparam);
                TextBox txtName = (TextBox)S3GFunderAddress.FindControl("txtCustomerName");
                txtName.Text = dtVendorDet.Rows[0]["Code"].ToString();
                S3GFunderAddress.SetCustomerDetails(dtVendorDet.Rows[0]["Code"].ToString(),
                        dtVendorDet.Rows[0]["Address1"].ToString() + "\n" +
                 dtVendorDet.Rows[0]["Address2"].ToString() + "\n" +
                dtVendorDet.Rows[0]["city"].ToString() + "\n" +
                dtVendorDet.Rows[0]["state"].ToString() + "\n" +
                dtVendorDet.Rows[0]["country"].ToString() + "\n" +
                dtVendorDet.Rows[0]["pincode"].ToString(), dtVendorDet.Rows[0]["Name"].ToString(), dtVendorDet.Rows[0]["Telephone"].ToString(),
                dtVendorDet.Rows[0]["mobile"].ToString(),
                dtVendorDet.Rows[0]["email"].ToString(), dtVendorDet.Rows[0]["website"].ToString());
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    private void FunPubGetGridDetails()
    {
        try
        {
            dictparam = new Dictionary<string, string>();
            dictparam.Add("@Company_ID", intCompanyID.ToString());
            dictparam.Add("@User_ID", intUserId.ToString());            
            dictparam.Add("@Tranche_ID", Convert.ToString(ddlTranche.SelectedValue));

            dtRSCM = Utility.GetDefaultData("S3G_LAD_RSCM_GetGridDtls", dictparam);
            if (dtRSCM.Rows.Count > 0)
            {
                lbrfunderdisbursementdate.Text = dtRSCM.Rows[0]["ORG_FILINGDATE"].ToString();
                lbrhF8filingduedate.Text = dtRSCM.Rows[0]["F8FilingDueDate"].ToString();
                txthPVAmount.Text = dtRSCM.Rows[0]["PVAmount"].ToString();
                if (lbrfunderdisbursementdate.Text != String.Empty && lbrhF8filingduedate.Text != String.Empty)
                {
                    btnSave.Enabled = true;
                    txthF8FilingDate.Enabled = true;
                    hCalendarExtender1.Enabled = true;
                    txthF8ChargeID.Enabled = true;
                    txthF8SRNNO.Enabled = true;
                    txthPVAmount.Enabled = true;
                    hchbF8CLBApplicable.Enabled = true;
                    txthF8Remarks.Enabled = true;
                    btnF8Go.Enabled = true;
                    FunPubBindGridRSCM(dtRSCM);
                }                        
            }
            else
            {
                btnSave.Enabled = false;
                FunPubBindGridRSCM(dtRSCM.Clone());
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected bool subProgram(string str)
    {
        if (str.ToUpper() == "TRUE")
            return true;
        else
            return false;
    }

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetRSChargeMainDetails()
    {
        ObjRSChargeMainClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            ObjS3G_LOANAD_RS_Charge_MgmtDataTable = new LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable();
            LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtRow ObjRS_Charge_MgmtRow;
            SerializationMode SerMode = SerializationMode.Binary;
            ObjRS_Charge_MgmtRow = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.NewS3G_LOANAD_RS_Charge_MgmtRow();
            ObjRS_Charge_MgmtRow.Company_ID = intCompanyID;
            ObjRS_Charge_MgmtRow.RSCM_ID = intRSCMID;

            ObjS3G_LOANAD_RS_Charge_MgmtDataTable.AddS3G_LOANAD_RS_Charge_MgmtRow(ObjRS_Charge_MgmtRow);

            byte[] byteRSChargeMainDetails = ObjRSChargeMainClient.FunPubQueryRSChargeMaintenance(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_RS_Charge_MgmtDataTable, SerMode));

            ObjS3G_LOANAD_RS_Charge_MgmtDataTable = new LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable();
            ObjS3G_LOANAD_RS_Charge_MgmtDataTable = (LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable)ClsPubSerialize.DeSerialize(byteRSChargeMainDetails, SerializationMode.Binary, typeof(LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtDataTable));

            if (ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows.Count > 0)
            {
                loblist();
                ddlLineOfBusiness.SelectedValue = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows[0]["Lob_ID"].ToString();
                //ddlLocation.SelectedText = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows[0]["LocationCat_Description"].ToString();
                //ddlLocation.SelectedValue = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows[0]["Location_ID"].ToString();
                ddlCustomerName.SelectedText = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows[0]["Customer_Name"].ToString();
                ddlCustomerName.SelectedValue = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows[0]["Customer_ID"].ToString();
                ddlTranche.SelectedText = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows[0]["Tranche_Name"].ToString();
                ddlTranche.SelectedValue = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows[0]["Tranche_ID"].ToString();
                ddlTranche_SelectedIndexChanged(null, null);
                txtRSCMCode.Text = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.Rows[0]["RSCM_CODE"].ToString();
            }

            dsRSCM = new DataSet();
            strProcName = "S3G_LAD_Get_RSCM_Dtls";
            dictparam = new Dictionary<string, string>();
            dictparam.Add("@Company_ID", intCompanyID.ToString());
            dictparam.Add("@RSCM_ID", intRSCMID.ToString());
            dsRSCM = Utility.GetTableValues(strProcName, dictparam);
            if (dsRSCM.Tables[1].Rows.Count > 0)
            {
                //F8 DETAILS                
                lbrfunderdisbursementdate.Text = dsRSCM.Tables[1].Rows[0]["ORG_FILINGDATE"].ToString();
                lbrhF8filingduedate.Text = dsRSCM.Tables[1].Rows[0]["F8FilingDueDate"].ToString();
                txthF8FilingDate.Text = dsRSCM.Tables[1].Rows[0]["F8FilingDate"].ToString();
                txthF8ChargeID.Text = dsRSCM.Tables[1].Rows[0]["F8ChargeID"].ToString();
                txthF8SRNNO.Text = dsRSCM.Tables[1].Rows[0]["F8SRNNO"].ToString();
                txthPVAmount.Text = dsRSCM.Tables[1].Rows[0]["PVAmount"].ToString();
                hchbF8CLBApplicable.Checked = Convert.ToBoolean(dsRSCM.Tables[1].Rows[0]["F8CLBApplicable"].ToString());
                txthF8Remarks.Text = dsRSCM.Tables[1].Rows[0]["F8Remarks"].ToString();

                //F17 DETAILS                
                lbrforeclosuredate.Text = dsRSCM.Tables[1].Rows[0]["Closure_Date"].ToString();
                txthF17nocdate.Text = dsRSCM.Tables[1].Rows[0]["NOC_DATE"].ToString();                
                txthF17ReleasingDate.Text = dsRSCM.Tables[1].Rows[0]["F17ReleaseDate"].ToString();
                txthF17ChargeID.Text = dsRSCM.Tables[1].Rows[0]["F17ChargeID"].ToString();
                txthF17SRNNO.Text = dsRSCM.Tables[1].Rows[0]["F17SRNNO"].ToString();
                hchbF17CLBApplicable.Checked = Convert.ToBoolean(dsRSCM.Tables[1].Rows[0]["F17CLBApplicable"].ToString());
                txthF17Remarks.Text = dsRSCM.Tables[1].Rows[0]["F17Remarks"].ToString();

                if(!String.IsNullOrEmpty(dsRSCM.Tables[1].Rows[0]["F17_cnt"].ToString()))
                F17_Count = Convert.ToInt64(dsRSCM.Tables[1].Rows[0]["F17_cnt"].ToString());
                                
                dtRSCM = dsRSCM.Tables[1].Copy();
                ViewState["RSCM_Details"] = dtRSCM;
                btnSave.Enabled = true;
                FunPubBindGridRSCM(dtRSCM);
                if(!String.IsNullOrEmpty(txthF17nocdate.Text))
                txthF17nocdate_TextChanged(null, null);

                if (!String.IsNullOrEmpty(lbrforeclosuredate.Text))
                {
                    btnF17Go.Enabled = true;
                    txthF17nocdate.Enabled = true;
                    txthF17ReleasingDate.Enabled = true;
                    hCalendarExtender2.Enabled = true;
                    txthF17ChargeID.Enabled = true;
                    txthF17SRNNO.Enabled = true;
                    hchbF17CLBApplicable.Enabled = true;
                    txthF17Remarks.Enabled = true;
                }                
            }
            dsRSCM.Dispose();
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjRSChargeMainClient.Close();
        }
    }

    private void FunPriBindAssetPopUpGrid(string PA_SA_Ref_ID)
    {
        try
        {
            dictparam = new Dictionary<string, string>();
            dictparam.Add("@PA_SA_Ref_ID", PA_SA_Ref_ID);

            int intTotalRecords = 0;
            bool bIsNewRow = false;

            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserId;

            grvAssetDtls.BindGridView("S3G_LAD_RSCM_GetAssetGridDtls", dictparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            ucPopUpPaging.Visible = true;
            ucPopUpPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucPopUpPaging.setPageSize(ProPageSizeRW);

            if (bIsNewRow)
                grvAssetDtls.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    private bool FunPriGenerateRSCMDtlsXML()
    {
        try
        {
            dtRSCM = (DataTable)ViewState["RSCM_Details"];

            if (dtRSCM.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Atleast One Grid Details Should Be Entered");
                return false;
            }

            if (dtRSCM.Rows.Count == 1)
            {
                if (dtRSCM.Rows[0]["SNO"].ToString().Equals("0") && PageMode == PageModes.Create)
                {
                    return false;
                }
            }
            strbRSCMDet.Append("<Root>");
            ViewState["RSCM_Details"] = dtRSCM;

            foreach (GridViewRow row in gvRSChargeMaintenance.Rows)
            {
                if (((Label)row.FindControl("lblORG_FILINGDATE")).Text != "" && ((TextBox)row.FindControl("txtF8FilingDate")).Text != "" && ((TextBox)row.FindControl("txtF8ChargeID")).Text != "" && ((TextBox)row.FindControl("txtF8SRNNO")).Text != "")
                {
                    strbRSCMDet.Append("<Details ");
                    strbRSCMDet.Append(" Charge_Mgmt_Dtl_ID = '" + (((Label)row.FindControl("lblCharge_Mgmt_Dtl_ID")).Text).ToString() + "'");
                    strbRSCMDet.Append(" PA_SA_Ref_ID = '" + (((Label)row.FindControl("lblPA_SA_Ref_ID")).Text).ToString() + "'");
                    if (!String.IsNullOrEmpty((((Label)row.FindControl("lblF8FilingDueDate")).Text)))
                        strbRSCMDet.Append(" F8_Filing_Due_Date = '" + Utility.StringToDate((((Label)row.FindControl("lblF8FilingDueDate")).Text).ToString()) + "'");
                    if (!String.IsNullOrEmpty((((TextBox)row.FindControl("txtF8FilingDate")).Text).ToString()))
                        strbRSCMDet.Append(" F8_Filing_Date = '" + Utility.StringToDate((((TextBox)row.FindControl("txtF8FilingDate")).Text).ToString()) + "'");
                    strbRSCMDet.Append(" F8_Charge_ID = '" + (((TextBox)row.FindControl("txtF8ChargeID")).Text).ToString() + "'");
                    strbRSCMDet.Append(" F8_SRN_No = '" + (((TextBox)row.FindControl("txtF8SRNNO")).Text).ToString() + "'");
                    strbRSCMDet.Append(" F8_PV_Amount = '" + txthPVAmount.Text.ToString() + "'");
                    strbRSCMDet.Append(" F8_Remarks = '" + (((TextBox)row.FindControl("txtF8Remarks")).Text).ToString() + "'");
                    strbRSCMDet.Append(" F8_CLB_Applicable = '" + Convert.ToBoolean(((CheckBox)row.FindControl("chbF8CLBApplicable")).Checked).ToString() + "'");
                    if(!String.IsNullOrEmpty(txthF17nocdate.Text))
                        strbRSCMDet.Append(" F17_NOC_date = '" + Utility.StringToDate((txthF17nocdate.Text).ToString()) + "'");
                    if (!String.IsNullOrEmpty((((Label)row.FindControl("lblF17ReleaseDueDate")).Text).ToString()))
                        strbRSCMDet.Append(" F17_Release_due_date = '" + Utility.StringToDate((((Label)row.FindControl("lblF17ReleaseDueDate")).Text).ToString()) + "'");
                    if (!String.IsNullOrEmpty((((TextBox)row.FindControl("txtF17ReleaseDate")).Text).ToString()))
                        strbRSCMDet.Append(" F17_Release_date = '" + Utility.StringToDate((((TextBox)row.FindControl("txtF17ReleaseDate")).Text).ToString()) + "'");
                    strbRSCMDet.Append(" F17_Charge_ID = '" + (((TextBox)row.FindControl("txtF17ChargeID")).Text).ToString() + "'");
                    strbRSCMDet.Append(" F17_SRN_No = '" + (((TextBox)row.FindControl("txtF17SRNNO")).Text).ToString() + "'");
                    strbRSCMDet.Append(" F17_Remarks = '" + (((TextBox)row.FindControl("txtF17Remarks")).Text).ToString() + "'");
                    strbRSCMDet.Append(" F17_CLB_Applicable = '" + Convert.ToBoolean(((CheckBox)row.FindControl("chbF17CLBApplicable")).Checked).ToString() + "'");
                    strbRSCMDet.Append(" />");
                }
            }
            strbRSCMDet.Append("</Root>");
            strXMLRSCMDet = strbRSCMDet.ToString();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void BindHeaderDtlsToGrid(String Type)
    {
        int CheckedCount = 0;
        try
        {
            foreach (GridViewRow row in gvRSChargeMaintenance.Rows)
            {
                CheckBox gchkselect = ((CheckBox)row.FindControl("gchkselect"));
                if (gchkselect.Checked)
                {
                    CheckedCount = CheckedCount + 1;
                    if (Type == "F8")//Form 8
                    {
                        if (!String.IsNullOrEmpty(((Label)row.FindControl("lblF8FilingDueDate")).Text))
                        {
                            (((Label)row.FindControl("lblF8FilingDueDate")).Text) = lbrhF8filingduedate.Text;
                            (((TextBox)row.FindControl("txtF8FilingDate")).Text) = txthF8FilingDate.Text;
                            ((TextBox)row.FindControl("txtF8ChargeID")).Text = txthF8ChargeID.Text;
                            ((TextBox)row.FindControl("txtF8SRNNO")).Text = txthF8SRNNO.Text;
                            //((TextBox)row.FindControl("txtPVAmount")).Text = txthPVAmount.Text;
                            ((TextBox)row.FindControl("txtF8Remarks")).Text = txthF8Remarks.Text;
                            ((CheckBox)row.FindControl("chbF8CLBApplicable")).Checked = hchbF8CLBApplicable.Checked;
                            ((CheckBox)row.FindControl("chbF8CLBApplicable")).Enabled = hchbF8CLBApplicable.Enabled;
                        }
                    }
                    if (Type == "F17")//Form 17
                    {
                        if (!String.IsNullOrEmpty(((Label)row.FindControl("lblF17ReleaseDueDate")).Text))
                        {
                            //((Label)row.FindControl("lblF17ReleaseDueDate")).Text = lbrhF17ReleasingDueDate.Text;
                            ((TextBox)row.FindControl("txtF17ReleaseDate")).Text = txthF17ReleasingDate.Text;
                            ((TextBox)row.FindControl("txtF17ChargeID")).Text = txthF17ChargeID.Text;
                            ((TextBox)row.FindControl("txtF17SRNNO")).Text = txthF17SRNNO.Text;
                            ((TextBox)row.FindControl("txtF17Remarks")).Text = txthF17Remarks.Text;
                            ((CheckBox)row.FindControl("chbF17CLBApplicable")).Checked = hchbF17CLBApplicable.Checked;
                            ((CheckBox)row.FindControl("chbF17CLBApplicable")).Enabled = hchbF17CLBApplicable.Enabled;                            
                        }
                    }
                }
            }
            if (gvRSChargeMaintenance.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Grid Details does not Exist");
                return;
            }
            if (gvRSChargeMaintenance.Rows.Count > 0 && CheckedCount == 0)
            {
                Utility.FunShowAlertMsg(this, "Atleast One Grid Details Should Be Checked");
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriClearDependentData()
    {
        try
        {
            S3GCustomerAddress.ClearCustomerDetails();
            S3GFunderAddress.ClearCustomerDetails();
            lbrfunderdisbursementdate.Text = lbrhF8filingduedate.Text = txthF8FilingDate.Text = txthF8ChargeID.Text = txthF8SRNNO.Text = txthPVAmount.Text = txthF8Remarks.Text = String.Empty;            
            hchbF8CLBApplicable.Checked = false;
            FunPubBindGridRSCM(dtRSCM.Clone());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region TEXT CHANGED EVENTS

    private void CheckDate(String strFormType, String StartDate, TextBox EndDate)
    {
        if (!String.IsNullOrEmpty(StartDate) && !String.IsNullOrEmpty(EndDate.Text))
        {
            intErrCount = Utility.CompareDates(StartDate, EndDate.Text);
            if (intErrCount == -1)
            {
                EndDate.Text = String.Empty;
                if (strFormType == "F8")
                    Utility.FunShowAlertMsg(this.Page, "Filing Date Should Not Be Less Than Funder Disbursement Date");
                else if (strFormType == "F17")
                    Utility.FunShowAlertMsg(this.Page, "Release Date Should Not Be Less Than No Dues Certifiate Date");
                else
                    Utility.FunShowAlertMsg(this.Page, "No Dues Certifiate Date Should Not Be Less Than Fore Closure Date");
                EndDate.Focus();
                return;
            }
        }
    }

    private void CheckDateSelectCheckBox(String strFormType, String StartDate, TextBox EndDate, CheckBox chbx)
    {
        if (!String.IsNullOrEmpty(StartDate) && !String.IsNullOrEmpty(EndDate.Text))
        {
            intErrCount = Utility.CompareDates(StartDate, EndDate.Text);
            if (strFormType == "F8")
            {
                if (intErrCount == 1)
                {
                    chbx.Enabled = true;
                    chbx.Checked = true;
                }
                else
                {
                    chbx.Enabled = false;
                    chbx.Checked = false;
                }
            }
            else if (strFormType == "F17")
            {
                if (intErrCount == 1)
                {
                    hchbF17CLBApplicable.Enabled = true;
                    hchbF17CLBApplicable.Checked = true;
                }
                else
                {
                    hchbF17CLBApplicable.Enabled = false;
                    hchbF17CLBApplicable.Checked = false;
                }
            }
        }
    }

    //Header F8 Filing Date Validation
    protected void txthF8FilingDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            strFormType = "F8";            
            CheckDate(strFormType, lbrfunderdisbursementdate.Text, txthF8FilingDate);            
            CheckDateSelectCheckBox(strFormType, lbrhF8filingduedate.Text, txthF8FilingDate, hchbF8CLBApplicable);
            strFormType = String.Empty;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    //Header F17 Releasing Date Validation
    protected void txthF17nocdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (strMode == "C")
            {
                CheckDate(strFormType, lbrforeclosuredate.Text, txthF17nocdate);
            }
            dictparam = new Dictionary<string, string>();
            dictparam.Add("@Company_ID", intCompanyID.ToString());
            dictparam.Add("@NOC_Date", Utility.StringToDate(txthF17nocdate.Text).ToString());
            System.Data.DataTable dt = Utility.GetDefaultData("S3G_LOANAD_RSCM_GETdate", dictparam);
            lbrhF17ReleasingDueDate.Text = dt.Rows[0]["RELEASING_DUE_DATE"].ToString();
            foreach (GridViewRow grv in gvRSChargeMaintenance.Rows)
            {
                ((Label)grv.FindControl("lblORG_RELEASINGDATE") as Label).Text = txthF17nocdate.Text;
                ((Label)grv.FindControl("lblF17ReleaseDueDate") as Label).Text = lbrhF17ReleasingDueDate.Text;
            }            
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    //Header F17 Releasing Date Validation
    protected void txthF17ReleasingDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            strFormType = "F17";
            CheckDate(strFormType, txthF17nocdate.Text, txthF17ReleasingDate);            
            CheckDateSelectCheckBox(strFormType, lbrhF17ReleasingDueDate.Text, txthF17ReleasingDate, hchbF17CLBApplicable);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    #region GRID Events

    protected void gvRSChargeMaintenance_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView gvRSChargeMaintenance1 = (GridView)sender;
                GridViewRow gvrow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell cell0 = new TableCell();
                cell0.ColumnSpan = 4;
                gvrow.Cells.Add(cell0);

                TableCell cell4 = new TableCell();
                cell4.Text = "Form 8";
                cell4.HorizontalAlign = HorizontalAlign.Center;
                cell4.ColumnSpan = 6;
                gvrow.Cells.Add(cell4);

                TableCell cell5 = new TableCell();
                cell5.Text = "Form 17";
                cell5.HorizontalAlign = HorizontalAlign.Center;
                cell5.ColumnSpan = 6;
                gvrow.Cells.Add(cell5);

                gvRSChargeMaintenance1.Controls[0].Controls.AddAt(0, gvrow);                
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvRSChargeMaintenance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    //e.Row.Cells[0].Visible = false;
            //    //e.Row.Cells[1].Visible = false;
            //    //e.Row.Cells[2].Visible = false;
            //    //e.Row.Cells[3].Visible = false;
            //    CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
            //    chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvRSChargeMaintenance.ClientID + "',this,'gchkselect');");
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CalendarExtender cal1 = (CalendarExtender)e.Row.FindControl("CalendarExtender1");
                cal1.Format = strDateFormat;
                CalendarExtender cal2 = (CalendarExtender)e.Row.FindControl("CalendarExtender2");
                cal2.Format = strDateFormat;
                TextBox txtF8FilingDate = (TextBox)e.Row.FindControl("txtF8FilingDate");
                txtF8FilingDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtF8FilingDate.ClientID + "','" + strDateFormat + "',null,null);");


                //if filing due date is empty,disable f8 grid details
                Label lblF8FilingDueDate = (Label)e.Row.FindControl("lblF8FilingDueDate");
                if (String.IsNullOrEmpty(lblF8FilingDueDate.Text))
                {
                    (e.Row.FindControl("txtF8FilingDate") as TextBox).Enabled = false;
                    (e.Row.FindControl("CalendarExtender1") as CalendarExtender).Enabled = false;
                    (e.Row.FindControl("txtF8ChargeID") as TextBox).Enabled = false;
                    (e.Row.FindControl("txtF8SRNNO") as TextBox).Enabled = false;
                    //(e.Row.FindControl("txtPVAmount") as TextBox).Enabled = false;                    
                    (e.Row.FindControl("txtF8Remarks") as TextBox).Enabled = false;
                    (e.Row.FindControl("chbF8CLBApplicable") as CheckBox).Enabled = false;
                }
                else
                {
                    (e.Row.FindControl("txtF8FilingDate") as TextBox).Enabled = true;
                    (e.Row.FindControl("CalendarExtender1") as CalendarExtender).Enabled = true;
                    (e.Row.FindControl("txtF8ChargeID") as TextBox).Enabled = true;
                    (e.Row.FindControl("txtF8SRNNO") as TextBox).Enabled = true;
                    //(e.Row.FindControl("txtPVAmount") as TextBox).Enabled = true;
                    (e.Row.FindControl("txtF8Remarks") as TextBox).Enabled = true;
                    (e.Row.FindControl("chbF8CLBApplicable") as CheckBox).Enabled = true;
                }

                //if releasing due date is empty,disable f17 grid details
                Label lblF17ReleaseDueDate = (Label)e.Row.FindControl("lblF17ReleaseDueDate");
                if (String.IsNullOrEmpty(lbrforeclosuredate.Text))
                {
                    (e.Row.FindControl("txtF17ReleaseDate") as TextBox).Enabled = false;
                    (e.Row.FindControl("CalendarExtender2") as CalendarExtender).Enabled = false;
                    (e.Row.FindControl("txtF17ChargeID") as TextBox).Enabled = false;
                    (e.Row.FindControl("txtF17SRNNO") as TextBox).Enabled = false;
                    (e.Row.FindControl("txtF17Remarks") as TextBox).Enabled = false;
                    (e.Row.FindControl("chbF17CLBApplicable") as CheckBox).Enabled = false;
                }
                else
                {
                    (e.Row.FindControl("txtF17ReleaseDate") as TextBox).Enabled = true;
                    (e.Row.FindControl("CalendarExtender2") as CalendarExtender).Enabled = true;
                    (e.Row.FindControl("txtF17ChargeID") as TextBox).Enabled = true;
                    (e.Row.FindControl("txtF17SRNNO") as TextBox).Enabled = true;
                    (e.Row.FindControl("txtF17Remarks") as TextBox).Enabled = true;
                    (e.Row.FindControl("chbF17CLBApplicable") as CheckBox).Enabled = true;
                }

                if (strMode == "Q")
                {

                    (e.Row.FindControl("gchkselect") as CheckBox).Enabled = false;
                    (e.Row.FindControl("lnkbtnviewinvoice") as LinkButton).Enabled = true;

                    (e.Row.FindControl("txtF8FilingDate") as TextBox).Enabled = false;
                    (e.Row.FindControl("CalendarExtender1") as CalendarExtender).Enabled = false;
                    (e.Row.FindControl("txtF8ChargeID") as TextBox).Enabled = false;
                    (e.Row.FindControl("txtF8SRNNO") as TextBox).Enabled = false;
                    //(e.Row.FindControl("txtPVAmount") as TextBox).Enabled = false;                    
                    (e.Row.FindControl("txtF8Remarks") as TextBox).Enabled = false;
                    (e.Row.FindControl("chbF8CLBApplicable") as CheckBox).Enabled = false;

                    (e.Row.FindControl("txtF17ReleaseDate") as TextBox).Enabled = false;
                    (e.Row.FindControl("CalendarExtender2") as CalendarExtender).Enabled = false;
                    (e.Row.FindControl("txtF17ChargeID") as TextBox).Enabled = false;
                    (e.Row.FindControl("txtF17SRNNO") as TextBox).Enabled = false;
                    (e.Row.FindControl("txtF17Remarks") as TextBox).Enabled = false;
                    (e.Row.FindControl("chbF17CLBApplicable") as CheckBox).Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void lnkbtnviewinvoice_Click(object sender, EventArgs e)
    {
        try
        {
            PA_SA_Ref_ID = String.Empty;
            GridViewRow gvrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            PA_SA_Ref_ID = ((Label)gvrow.FindControl("lblPA_SA_Ref_ID") as Label).Text;
            FunPriBindAssetPopUpGrid(PA_SA_Ref_ID);
            ModalPopupExtenderApprover.Show();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnDEVModalCancel_Click(object sender, EventArgs e)
    {
        PA_SA_Ref_ID = string.Empty;
        ModalPopupExtenderApprover.Hide();
    }

    protected void txtF8FilingDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtORG_FILINGDATE, dtfilduedate, dtfildate;
        CheckBox chbF8CLBApplicable;
        try
        {
            GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
            chbF8CLBApplicable = ((CheckBox)gvrow.FindControl("chbF8CLBApplicable") as CheckBox);
            Label lblORG_FILINGDATE = ((Label)gvrow.FindControl("lblORG_FILINGDATE") as Label);
            Label lblF8FilingDueDate = ((Label)gvrow.FindControl("lblF8FilingDueDate") as Label);
            TextBox txtF8FilingDate = (TextBox)gvrow.FindControl("txtF8FilingDate") as TextBox;

            if (!String.IsNullOrEmpty(lblORG_FILINGDATE.Text))
            {
                dtORG_FILINGDATE = Utility.StringToDate(lblORG_FILINGDATE.Text);
                if (!String.IsNullOrEmpty(txtF8FilingDate.Text))
                {
                    dtfildate = Utility.StringToDate(txtF8FilingDate.Text);
                    if (dtORG_FILINGDATE != null && dtfildate != null)
                    {
                        if (dtfildate < dtORG_FILINGDATE)
                        {
                            Utility.FunShowAlertMsg(this.Page, "Filing Date Should Not Be Less Than Funder Disbursement Date");
                            txtF8FilingDate.Text = String.Empty;
                            return;
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(lblF8FilingDueDate.Text))
            {
                dtfilduedate = Utility.StringToDate(lblF8FilingDueDate.Text);
                if (!String.IsNullOrEmpty(txtF8FilingDate.Text))
                {
                    dtfildate = Utility.StringToDate(txtF8FilingDate.Text);
                    if (dtfilduedate != null && dtfildate != null)
                    {
                        if (dtfildate > dtfilduedate)
                        {
                            chbF8CLBApplicable.Enabled = true;
                            chbF8CLBApplicable.Checked = true;
                        }
                        else
                        {
                            chbF8CLBApplicable.Enabled = false;
                            chbF8CLBApplicable.Checked = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void txtF17ReleaseDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtORG_RELEASINGDATE, dtRelduedate, dtReldate;
        CheckBox chbF17CLBApplicable;
        try
        {
            GridViewRow gvrow = (GridViewRow)((TextBox)sender).NamingContainer;
            chbF17CLBApplicable = ((CheckBox)gvrow.FindControl("chbF17CLBApplicable") as CheckBox);
            Label lblORG_RELEASINGDATE = ((Label)gvrow.FindControl("lblORG_RELEASINGDATE") as Label);
            Label lblF17ReleaseDueDate = ((Label)gvrow.FindControl("lblF17ReleaseDueDate") as Label);
            TextBox txtF17ReleaseDate = (TextBox)gvrow.FindControl("txtF17ReleaseDate") as TextBox;

            if (!String.IsNullOrEmpty(lblORG_RELEASINGDATE.Text))
            {
                dtORG_RELEASINGDATE = Utility.StringToDate(lblORG_RELEASINGDATE.Text);
                if (!String.IsNullOrEmpty(txtF17ReleaseDate.Text))
                {
                    dtReldate = Utility.StringToDate(txtF17ReleaseDate.Text);
                    if (dtORG_RELEASINGDATE != null && dtReldate != null)
                    {
                        if (dtReldate < dtORG_RELEASINGDATE)
                        {
                            Utility.FunShowAlertMsg(this.Page, "Release Date Should Not Be Less Than No Dues Certificate Date");
                            txtF17ReleaseDate.Text = String.Empty;
                            return;
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(lblF17ReleaseDueDate.Text))
            {
                dtRelduedate = Utility.StringToDate(lblF17ReleaseDueDate.Text);
                if (!String.IsNullOrEmpty(txtF17ReleaseDate.Text))
                {
                    dtReldate = Utility.StringToDate(txtF17ReleaseDate.Text);
                    if (dtRelduedate != null && dtReldate != null)
                    {
                        if (dtReldate > dtRelduedate)
                        {
                            chbF17CLBApplicable.Enabled = true;
                            chbF17CLBApplicable.Checked = true;
                        }
                        else
                        {
                            chbF17CLBApplicable.Enabled = false;
                            chbF17CLBApplicable.Checked = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region BUTTON EVENTS

    protected void btnF8Go_Click(object sender, EventArgs e)
    {
        String Type = "F8";
        BindHeaderDtlsToGrid(Type);
        Type = String.Empty;
    }

    protected void btnF17Go_Click(object sender, EventArgs e)
    {
        String Type = "F17";
        BindHeaderDtlsToGrid(Type);
        Type = String.Empty;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjRSChargeMainClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            //if (ddlLocation.SelectedText == "")
            //{
            //    Utility.FunShowAlertMsg(this, "Location Should Not Be Empty");
            //    ddlLocation.Focus();
            //    return;
            //}

            if (ddlCustomerName.SelectedText == "")
            {
                Utility.FunShowAlertMsg(this, "Customer Name Should Not Be Empty");
                ddlCustomerName.Focus();
                return;
            }

            if (ddlTranche.SelectedText == "")
            {
                Utility.FunShowAlertMsg(this, "Tranche Should Not Be Empty");
                ddlTranche.Focus();
                return;
            }

            foreach (GridViewRow grv in gvRSChargeMaintenance.Rows)
            {
                Label lblRSNO = (Label)grv.FindControl("lblRSNO") as Label;
                Label lblORG_FILINGDATE = (Label)grv.FindControl("lblORG_FILINGDATE") as Label;
                TextBox txtF8FilingDate = (TextBox)grv.FindControl("txtF8FilingDate") as TextBox;
                TextBox txtF8ChargeID = (TextBox)grv.FindControl("txtF8ChargeID") as TextBox;
                TextBox txtF8SRNNO = (TextBox)grv.FindControl("txtF8SRNNO") as TextBox;
                //TextBox txtPVAmount = (TextBox)grv.FindControl("txtPVAmount") as TextBox;

                Label lblORG_RELEASINGDATE = (Label)grv.FindControl("lblORG_RELEASINGDATE") as Label;
                TextBox txtF17ReleaseDate = (TextBox)grv.FindControl("txtF17ReleaseDate") as TextBox;
                TextBox txtF17ChargeID = (TextBox)grv.FindControl("txtF17ChargeID") as TextBox;
                TextBox txtF17SRNNO = (TextBox)grv.FindControl("txtF17SRNNO") as TextBox;

                #region F8 Validation

                if (gvRSChargeMaintenance.Rows.Count < 2)
                {
                    if (!String.IsNullOrEmpty(lblORG_FILINGDATE.Text))
                    {
                        if (String.IsNullOrEmpty(txtF8FilingDate.Text) && (String.IsNullOrEmpty(txtF8ChargeID.Text)) && (String.IsNullOrEmpty(txtF8SRNNO.Text)))
                        {
                            Utility.FunShowAlertMsg(this, "Atleast One Grid Details Should Be Entered");
                            txtF8FilingDate.Focus();
                            return;
                        }
                        if (String.IsNullOrEmpty(txtF8FilingDate.Text) || String.IsNullOrEmpty(txtF8ChargeID.Text) || String.IsNullOrEmpty(txtF8SRNNO.Text))
                        {
                            Utility.FunShowAlertMsg(this, "All The Grid Details Should Be Entered");
                            txtF8FilingDate.Focus();
                            return;
                        }
                    }
                }

                //F8 Charge ID
                if (!String.IsNullOrEmpty(txtF8FilingDate.Text))
                {
                    if (String.IsNullOrEmpty(txtF8ChargeID.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Charge ID Should Not be Empty For The RS " + lblRSNO.Text + " In Form 8");
                        txtF8ChargeID.Focus();
                        return;
                    }
                }

                //F8 SRN No
                if (!String.IsNullOrEmpty(txtF8FilingDate.Text))
                {
                    if (String.IsNullOrEmpty(txtF8SRNNO.Text))
                    {
                        Utility.FunShowAlertMsg(this, "SRN No Should Not be Empty For The RS " + lblRSNO.Text + " In Form 8");
                        txtF8SRNNO.Focus();
                        return;
                    }
                }

                ////F8 Total Charge Amount
                //if (!String.IsNullOrEmpty(txtF8FilingDate.Text))
                //{
                //    if (String.IsNullOrEmpty(txtPVAmount.Text))
                //    {
                //        Utility.FunShowAlertMsg(this, "Total Charge Amount Should Not be Empty For The RS " + lblRSNO.Text + " In Form 8");
                //        txtPVAmount.Focus();
                //        return;
                //    }
                //}

                #endregion

                #region F17 Validation

                if (!String.IsNullOrEmpty(lblORG_RELEASINGDATE.Text))
                {
                    if ((strMode == "M" && String.IsNullOrEmpty(txtF17ReleaseDate.Text)) && (!String.IsNullOrEmpty(txtF17ChargeID.Text) || !String.IsNullOrEmpty(txtF17SRNNO.Text)))
                    {
                        Utility.FunShowAlertMsg(this, "Release Date Should Not be Empty For The RS " + lblRSNO.Text + " In Form 17");
                        txtF17ReleaseDate.Focus();
                        return;
                    }
                }

                //F17 Charge ID
                if (!String.IsNullOrEmpty(txtF17ReleaseDate.Text))
                {
                    if (String.IsNullOrEmpty(txtF17ChargeID.Text))
                    {
                        Utility.FunShowAlertMsg(this, "Charge ID Should Not be Empty For The RS " + lblRSNO.Text + " In Form 17");
                        txtF17ChargeID.Focus();
                        return;
                    }
                }

                //F17 SRN No
                if (!String.IsNullOrEmpty(txtF17ReleaseDate.Text))
                {
                    if (String.IsNullOrEmpty(txtF17SRNNO.Text))
                    {
                        Utility.FunShowAlertMsg(this, "SRN No Should Not be Empty For The RS " + lblRSNO.Text + " In Form 17");
                        txtF17SRNNO.Focus();
                        return;
                    }
                }

                #endregion
            }
            string RSCM_Code = "";
            LoanAdminMgtServices.S3G_LOANAD_RS_Charge_MgmtRow ObjRSChargeMainRow;
            ObjRSChargeMainRow = ObjS3G_LOANAD_RS_Charge_MgmtDataTable.NewS3G_LOANAD_RS_Charge_MgmtRow();
            ObjRSChargeMainRow.Company_ID = intCompanyID;
            if (ddlLineOfBusiness.SelectedValue != "0")
                ObjRSChargeMainRow.Lob_ID = Convert.ToInt32(ddlLineOfBusiness.SelectedValue);
            ObjRSChargeMainRow.Location_ID = 0;
            if (ddlCustomerName.SelectedValue != "0")
                ObjRSChargeMainRow.Customer_ID = Convert.ToInt32(ddlCustomerName.SelectedValue);
            if (ddlTranche.SelectedValue != "0")
                ObjRSChargeMainRow.Tranche_ID = Convert.ToInt32(ddlTranche.SelectedValue);
            ObjRSChargeMainRow.User_ID = intUserId;
            ObjRSChargeMainRow.RSCM_ID = intRSCMID;

            if (ViewState["RSCM_Details"] != null)
            {
                FunPriGenerateRSCMDtlsXML();
            }
            if (strXMLRSCMDet.Contains("Details"))
            {
                ObjRSChargeMainRow.XMLRSCMDtls = strXMLRSCMDet;
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Atleast One Grid Details Should Be Entered");
                return;
            }

            //ObjRSChargeMainRow.XMLRSCMDtls = gvRSChargeMaintenance.FunPubFormXml();
            ObjS3G_LOANAD_RS_Charge_MgmtDataTable.AddS3G_LOANAD_RS_Charge_MgmtRow(ObjRSChargeMainRow);
            intErrCode = ObjRSChargeMainClient.FunPubCreateRSChargeMaint(out RSCM_Code, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_RS_Charge_MgmtDataTable, SerMode));

            if (intErrCode == 0)
            {
                btnSave.Enabled = false;
                txtRSCMCode.Text = RSCM_Code;
                strAlert = "RS Charge Maintenance " + RSCM_Code + " added successfully";
                strAlert += @"\n\nWould you like to add one more RS Charge Maintenance?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 4)
            {
                btnSave.Enabled = false;
                txtRSCMCode.Text = RSCM_Code;
                strAlert = "RS Charge Maintenance " + RSCM_Code + " Updated successfully";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 5)
            {
                btnSave.Enabled = false;
                txtRSCMCode.Text = RSCM_Code;
                strAlert = "RS Charge Maintenance " + RSCM_Code + " Deleted successfully";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "RS Charge Maintenance already exist");
            }
            else if (intErrCode == 7)
            {
                Utility.FunShowAlertMsg(this.Page, "Define RS Charge Maintenance at Location Level");
            }
            else if (intErrCode == 8)
            {
                Utility.FunShowAlertMsg(this.Page, "Define RS Charge Maintenance at LOB Level");
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create RS Charge Maintenance");
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "Effective from date cannot be less than date of incorporation of the company");
            }
            lblErrorMessage.Text = string.Empty;
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjRSChargeMainClient.Close();
        }
    }

    /// <summary>
    /// This is used to clear data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txthF8FilingDate.Enabled = hCalendarExtender1.Enabled = txthF8ChargeID.Enabled = txthF8SRNNO.Enabled = txthPVAmount.Enabled = txthF8Remarks.Enabled = btnF8Go.Enabled = btnSave.Enabled = false;
            //ddlLocation.Clear();
            ddlTranche.Clear();
            S3GCustomerAddress.ClearCustomerDetails();
            S3GFunderAddress.ClearCustomerDetails();
            lbrfunderdisbursementdate.Text = lbrhF8filingduedate.Text = txthF8FilingDate.Text = txthF8ChargeID.Text = txthF8SRNNO.Text = txthPVAmount.Text = txthF8Remarks.Text =
            lbrforeclosuredate.Text = txthF17nocdate.Text = lbrhF17ReleasingDueDate.Text = txthF17ReleasingDate.Text = txthF17ChargeID.Text = txthF17SRNNO.Text = txthF17Remarks.Text = String.Empty;
            hchbF8CLBApplicable.Checked = hchbF17CLBApplicable.Checked = false;
            FunPubBindGridRSCM(dtRSCM.Clone());
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// This is used to redirect page(create to translander)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

    #endregion

    #region SELECTED INDEX CHANGED EVENTS

    //protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlLocation.SelectedValue != "0")
    //        {
    //            Loc_ID = ddlLocation.SelectedValue;
    //            ddlTranche.Clear();
    //            FunPriClearDependentData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomerName.SelectedValue != "0")
            {
                 ddlTranche.Clear();
                 FunPriClearDependentData();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    
    protected void ddlTranche_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTranche.SelectedValue != "0")
            {
                FunPriClearDependentData();
                Tranche_ID = ddlTranche.SelectedValue;
                FunPubGetCustFundIDS(Tranche_ID);
                if (dtRSCM.Rows.Count > 0)
                {
                    Cust_ID = dtRSCM.Rows[0][0].ToString();
                    Fund_ID = dtRSCM.Rows[0][1].ToString();
                    FunPriGetCustomerDetails(Cust_ID);
                    FunPriGetFunderDetails(Fund_ID);
                    FunPubGetGridDetails();
                    Cust_ID = String.Empty;
                    Fund_ID = String.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    //code added by vinodha to handle the impacts during the check all and uncheck all
    protected void chkAll_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {            
            CheckBox btn = (CheckBox)sender;
            if (btn.Checked)
            {
                foreach (GridViewRow row in gvRSChargeMaintenance.Rows)
                {
                    CheckBox gchkselect = ((CheckBox)row.FindControl("gchkselect"));
                    gchkselect.Checked = true;
                }                
            }
            else
            {
                foreach (GridViewRow row in gvRSChargeMaintenance.Rows)
                {
                    CheckBox gchkselect = ((CheckBox)row.FindControl("gchkselect"));
                    gchkselect.Checked = false;
                }
            }
        }
        catch (Exception objException)
        {
           
        }
    }

    protected void gchkselect_OnCheckedChanged(object sender, EventArgs e)
    {
        Int32 RowCount,CheckedCount=0;
        try
        {
            RowCount = gvRSChargeMaintenance.Rows.Count;
            foreach (GridViewRow grdRow in gvRSChargeMaintenance.Rows)
            {
                CheckBox gchkselect = (CheckBox)grdRow.FindControl("gchkselect");
                if (gchkselect.Checked == true)
                {
                    CheckedCount = CheckedCount + 1 ;
                }
            }

            CheckBox chkAll = (CheckBox)gvRSChargeMaintenance.HeaderRow.FindControl("chkAll");
            if (CheckedCount == RowCount)
            {
                chkAll.Checked = true;
            }
            else
            {
                chkAll.Checked = false;
            }
        }
        catch (Exception objException)
        {

        }
    }

    #region AUTO SUGGEST CONTROL EVENTS

    //<<Performance>>
    //Location Details
    //[System.Web.Services.WebMethod]
    //public static string[] GetLocationDetails(String prefixText, int count)
    //{
    //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //    List<String> suggetions = new List<String>();

    //    Procparam.Clear();
    //    Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
    //    Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
    //    Procparam.Add("@PrefixText", prefixText);
    //    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_RSCM_Get_LocDtls", Procparam));
    //    return suggetions.ToArray();
    //}

    //<<Performance>>
    //customer code Details
    [System.Web.Services.WebMethod]
    public static string[] GetCustomerDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_RSCM_GetCustCode", Procparam));
        return suggetions.ToArray();
    }

    //<<Performance>>
    //Tranche Details
    [System.Web.Services.WebMethod]
    public static string[] GetTrancheDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        //Procparam.Add("@Loc_ID", obj_Page.ddlLocation.SelectedValue.ToString());
        Procparam.Add("@Cust_ID", obj_Page.ddlCustomerName.SelectedValue.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_RSCM_GetTranchDtl", Procparam));
        return suggetions.ToArray();
    }

    #endregion

}