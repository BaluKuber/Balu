#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Legal & Repossession
/// Screen Name			: Sale Bid Entry
/// Created By			: Srivatsan S
/// Created Date		: 28-April-2011
/// Purpose	            : This module is used to store and retrieve Sale Bid Entry details
///<Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using LEGAL = S3GBusEntity.LegalRepossession;
using LEGALSERVICES = LegalAndRepossessionMgtServicesReference;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.ServiceModel;
#endregion


public partial class LegalRepossession_S3GLRSaleBidEntry : ApplyThemeForProject
{
    #region Initialization
    int _CompanyID, _UserID, _CustomerID, _SlNo = 0;
    string _DateFormat = "dd/MM/yyyy";
    Dictionary<string, string> Procparam = null;
    string strDateFormat = string.Empty;
    string strDSNNo = string.Empty;
    string strBidNo = string.Empty;
    Decimal decRoundOff = 0;
    int intSaleBidId=0;
    int intErrorCode=0;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    StringBuilder strbBidDetails = new StringBuilder();
    string strRedirectPage = "../LegalRepossession/S3GLRTransLander.aspx?Code=GSBE";
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRSaleBidEntry.aspx'";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GSBE';";
    LEGAL.LegalRepossessionMgtServices.S3G_LR_SaleBidDataTable objSaleBidDataTable;
    LEGALSERVICES.LegalAndRepossessionMgtServicesClient ObjLegatMgtServicesClnt;
       //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    S3GSession ObjS3GSession;
    UserInfo ObjUserInfo;
    public static LegalRepossession_S3GLRSaleBidEntry obj_Page;

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        try
        {
            FunPubSetIndex(1);
            ObjUserInfo = new UserInfo();
            obj_Page = this;
            ObjS3GSession = new S3GSession();
            
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _CompanyID = ObjUserInfo.ProCompanyIdRW;
            _UserID = ObjUserInfo.ProUserIdRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end
            //FunSetComboBoxAttributes(TxtSBidCity, "City", "30");
            //FunSetComboBoxAttributes(TxtSBidState, "State", "60");
            //FunSetComboBoxAttributes(TxtSBidCountry, "Country", "60");
            
            if (!IsPostBack)
            {
                Session["CUSTOMERID"] = Session["SALEBIDID"]=Session["SALENOTIFASSTID"] = "";
                if (Request.QueryString["qsMode"] != null)
                    strMode = Request.QueryString["qsMode"];
                if (Request.QueryString["qsViewId"] != string.Empty && Request.QueryString["qsViewId"] != null)
                {
                    FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                    intSaleBidId =Convert.ToInt32(fromTicket.Name);
                }
                FunProIntializeData();
                FunToolTip();
                FunPriInitialRow();
               
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (intSaleBidId > 0)
                {
                    bool blnTranExists;
                   
                    FunPubProGetSaleBidDetails(_CompanyID, intSaleBidId);
                    if (strMode == "M")
                    {
                        DataSet DS;
                        Procparam = new Dictionary<string, string>();
                        Procparam.Add("@SALEBIDID", Convert.ToString(intSaleBidId));
                        //Procparam.Add("@Consitution_Id", txtConstitution.Attributes["Const_ID"]);
                        DS = Utility.GetDataset(SPNames.S3G_LR_GetInvoiceonSaleBid, Procparam);
                        if (DS.Tables[0].Rows.Count != 0)
                        {
                            Utility.FunShowAlertMsg(this.Page, "An Invoice Entry exists for the Sale Bid Number. Hence, you would not be able to modify the contents.Showing in Query mode.");
                            FunPriSaleBidControlStatus(-1);
                            //lblHeading.Text = lblHeading.Text + ": An Invoice Entry exists for the Sale Bid Number. Hence, you won't be able to modify the contents.Showing in Query mode!";
                        }
                        else
                        {
                            FunPriSaleBidControlStatus(1);
                        }
                    }
                    if (strMode == "Q")
                    {
                        FunPriSaleBidControlStatus(-1);
                    }
                }
                else
                {
                    PopulateBranchList();
                    PopulateLOBList();
                    FunPriSaleBidControlStatus(0);
                }
            }
        }
        catch (Exception ex)
        {
            cvSaleBidentry.ErrorMessage = ex.Message;
            cvSaleBidentry.IsValid = false;
        }
        
    }
    #endregion

    #region Dropdown Events
    protected void ddlSBidBranch_SelectedIndexChanged(object sender, EventArgs e)
    { 
        //if (ddlSBidBranch.Items.Count > 0)
        //{ ddlSBidBranch.ToolTip = ddlSBidBranch.SelectedItem.Text; }
        if (Convert.ToInt32(ddlBranch.SelectedValue) != 0)
        {
            PopulateSaleNotificationNumber();
            ddlSBidSSNo.Enabled = true;
        }
        else
        {
            ddlSBidSSNo.ClearDropDownList();
            ddlSBidSSNo.ClearSelection();
            ddlSBidSSNo.Enabled = false;
            TxtSBidMLANo.Clear();
            TxtSBidRefNo.Clear();
            TxtSBidRepDocNo.Clear();
            TxtSBidSLANo.Clear();
            UcSBiddCustomerDetails.ClearCustomerDetails();
            GRVAssetDetails.ClearGrid();
            grvBidDetails.ClearGrid();
            FunProIntializeData();
            DataTable dtBidDetails = (DataTable)ViewState["DetailsTable"];
            dtBidDetails.Rows.Clear();
            ViewState["DetailsTable"] = dtBidDetails;
            FunPriInitialRow();
            grvBidDetails.Enabled = false;
        }
    }

    protected void ddSBidlLOB_SelectIndexChanged(object sender, EventArgs e)
    {
        if (ddSBidLOB.SelectedIndex != 0)
        {
            ddlBranch.Enabled = true;
            PopulateBranchList();
            ddlSBidSSNo.Enabled = false;
            ddlSBidSSNo.ClearDropDownList();
            ddlSBidSSNo.ClearSelection();
            ddlSBidSSNo.Enabled = false;
        }
        else
        {
            ddlSBidSSNo.ClearDropDownList();
            ddlSBidSSNo.ClearSelection();
            ddlSBidSSNo.Enabled = false;
            ddlBranch.Clear();
            ddlBranch.Enabled = false;
            PopulateBranchList();
            TxtSBidMLANo.Clear();
            TxtSBidRefNo.Clear();
            TxtSBidRepDocNo.Clear();
            TxtSBidSLANo.Clear();
            UcSBiddCustomerDetails.ClearCustomerDetails();
            GRVAssetDetails.ClearGrid();
            grvBidDetails.ClearGrid();
            //FunProIntializeData();
            DataTable dtBidDetails = (DataTable)ViewState["DetailsTable"];
            dtBidDetails.Rows.Clear();
            ViewState["DetailsTable"] = dtBidDetails;
            FunPriInitialRow();
            grvBidDetails.Enabled = false;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.ObjUserInfo.ProCompanyIdRW.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Program_Id", "141");
        Procparam.Add("@Lob_Id", obj_Page.ddSBidLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }


    protected void ddlSBidSSNo_SelectIndexChanged(object sender, EventArgs e)
    {
        if (ddlSBidSSNo.SelectedIndex != 0)
        {
            FunPriGetSaleNotificationDetails(_CompanyID, Convert.ToInt32(ddlSBidSSNo.SelectedValue));
            grvBidDetails.Enabled = true;
            TextBox TxtSBidBidderName = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidderName");
            TxtSBidBidderName.Focus();
        }
        else
        {
            TxtSBidMLANo.Clear();
            TxtSBidRefNo.Clear();
            TxtSBidRepDocNo.Clear();
            TxtSBidSLANo.Clear();
            UcSBiddCustomerDetails.ClearCustomerDetails();
            GRVAssetDetails.ClearGrid();
            grvBidDetails.ClearGrid();
            //FunProIntializeData();
            DataTable dtBidDetails = (DataTable)ViewState["DetailsTable"];
            dtBidDetails.Rows.Clear();
            ViewState["DetailsTable"] = dtBidDetails;
            FunPriInitialRow();
            grvBidDetails.Enabled = false;
        }
    }
    #endregion

    #region Button Events
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ex)
        {
            cvSaleBidentry.ErrorMessage = ex.Message;
            cvSaleBidentry.IsValid = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
             Insert_Router();
        
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvSaleBidentry.ErrorMessage = ex.Message;
            cvSaleBidentry.IsValid = false;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    #endregion

    #region User Defined functions

    protected void FunProIntializeData()
    {
        try
        {
            DataTable dtBidViewDetails;
            dtBidViewDetails = new DataTable("BIDDETAILS");
            dtBidViewDetails.Columns.Add("BID_NO");
            dtBidViewDetails.Columns.Add("BIDDER_NAME");
            dtBidViewDetails.Columns.Add("BIDDER_ADDRESS");
            dtBidViewDetails.Columns.Add("VALIDITY");
            dtBidViewDetails.Columns.Add("BID_VALUE");
            dtBidViewDetails.Columns.Add("BID_AMOUNT");
            dtBidViewDetails.Columns.Add("REMARKS");
            ViewState["DetailsTable"] = dtBidViewDetails;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to Intialize data");
        }
    }
   
    public void Insert_Router()
    {
        ObjLegatMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {
            DataTable dtSubmit = (DataTable)ViewState["DetailsTable"];
            
            TextBox TxtSBidBidderName = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidderName");
            TextBox TxtSBidCntctDtls = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidCntctDtls");
            TextBox TxtSBidValidity = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidValidity");
            TextBox TxtSBidBidValue = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidValue");
            TextBox TxtSBidBidAmount = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidAmount");
            TextBox TxtSBidRemarks = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidRemarks");

            if (dtSubmit.Rows.Count == 1)
            {
                if (dtSubmit.Rows[0]["BID_NO"].ToString() == "0" && dtSubmit.Rows[0]["BIDDER_NAME"].ToString() == "" && dtSubmit.Rows[0]["BIDDER_ADDRESS"].ToString() == "" && dtSubmit.Rows[0]["VALIDITY"].ToString() == "" && dtSubmit.Rows[0]["BID_VALUE"].ToString() == "" && dtSubmit.Rows[0]["BID_AMOUNT"].ToString() == "" && dtSubmit.Rows[0]["REMARKS"].ToString() == "")
                {
                    Utility.FunShowAlertMsg(this.Page, "Add one row in Sale Bid details");
                    return;
                }

            }
           //if (TxtSBidBidderName.Text != "" || TxtSBidCntctDtls.Text != "" || TxtSBidValidity.Text != "" || TxtSBidBidValue.Text != "" || TxtSBidBidAmount.Text != "" || TxtSBidRemarks.Text != "")
           //{
           
           //    DataView dvBidDetails = new DataView(dtSubmit);
           //    dvBidDetails.Sort = "BID_NO";
           //    dvBidDetails.RowFilter = "BIDDER_NAME='" + TxtSBidBidderName.Text.ToString().Trim() + "' AND BIDDER_ADDRESS='" + TxtSBidCntctDtls.Text.ToString().Trim() + "' AND VALIDITY='" + Utility.StringToDate(TxtSBidValidity.Text.Trim()).ToString() + "' AND BID_VALUE='" + TxtSBidBidValue.Text.ToString().Trim() + "' AND BID_AMOUNT='" + TxtSBidBidAmount.Text.ToString().Trim() + "'";
           //    if (dvBidDetails.ToTable().Rows.Count != 0)
           //    {
           //        Utility.FunShowAlertMsg(this.Page, "The Bid Details already exists!");
           //        return;
           //    }
           //    else
           //    {
           //        if (grvBidDetails.Rows.Count < 5)
           //        {
           //            DataRow drBidDetails;
           //            drBidDetails = dtSubmit.NewRow();

           //            drBidDetails["BID_NO"] = dtSubmit.Rows.Count + 1;
           //            drBidDetails["BIDDER_NAME"] = TxtSBidBidderName.Text.Trim();
           //            drBidDetails["BIDDER_ADDRESS"] = TxtSBidCntctDtls.Text.Trim();
           //            drBidDetails["VALIDITY"] = Utility.StringToDate(TxtSBidValidity.Text.Trim()).ToString();
           //            drBidDetails["BID_VALUE"] = TxtSBidBidValue.Text.Trim();
           //            drBidDetails["BID_AMOUNT"] = TxtSBidBidAmount.Text.Trim();
           //            drBidDetails["REMARKS"] = TxtSBidRemarks.Text.Trim();
           //            dtSubmit.Rows.Add(drBidDetails);
           //            grvBidDetails.DataSource = dtSubmit;
           //            grvBidDetails.DataBind();
           //            ViewState["DetailsTable"] = dtSubmit;
           //        }
           //    }
                
           // }
            intSaleBidId = Session["SALEBIDID"].ToString() == "" ? 0 : Convert.ToInt32(Session["SALEBIDID"].ToString());
            objSaleBidDataTable = new LEGAL.LegalRepossessionMgtServices.S3G_LR_SaleBidDataTable();
            LEGAL.LegalRepossessionMgtServices.S3G_LR_SaleBidRow ObjSaleBidRow;
            ObjSaleBidRow = objSaleBidDataTable.NewS3G_LR_SaleBidRow();
            ObjSaleBidRow.LOB_ID = Convert.ToInt32(ddSBidLOB.SelectedValue);
            ObjSaleBidRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjSaleBidRow.Company_ID = _CompanyID;
            ObjSaleBidRow.Created_By = _UserID;
            ObjSaleBidRow.Customer_ID = Convert.ToInt32(Session["CUSTOMERID"].ToString());
            ObjSaleBidRow.Is_Active = ChkSBidIsActive.Checked ? true : false;
            ObjSaleBidRow.Modified_By = _UserID;
            ObjSaleBidRow.PANum = TxtSBidMLANo.Text.ToString().Trim();
            ObjSaleBidRow.Sale_Bid_ID = intSaleBidId;
            ObjSaleBidRow.Sale_Notification_ID  = Convert.ToInt32(ddlSBidSSNo.SelectedValue.ToString());
            ObjSaleBidRow.SANum = TxtSBidSLANo.Text.ToString().Trim();
            ObjSaleBidRow.Sale_Bid_No ="";
           
            strbBidDetails.Append("<Root>");
            if (dtSubmit.Rows.Count > 0)
            {
                foreach (DataRow dtBidDetailsRow in dtSubmit.Rows)
                {
                    strbBidDetails.Append(" <BIDDETAILS SALE_NOTIFICATION_ASSET_ID='" + Convert.ToInt32(Session["SALENOTIFASSTID"].ToString()) + "'");
                    strbBidDetails.Append(" BID_NO = '" + Convert.ToInt32(dtBidDetailsRow["BID_NO"].ToString()) + "'");
                    strbBidDetails.Append(" BIDDER_NAME = '" + Convert.ToString(dtBidDetailsRow["BIDDER_NAME"].ToString()) + "'");
                    strbBidDetails.Append(" BIDDER_ADDRESS = '" + Convert.ToString(dtBidDetailsRow["BIDDER_ADDRESS"].ToString()) + "'");
                    //Modified by chandru on 26/03/2012
                    //strbBidDetails.Append(" BID_VALIDUPTO = '" + Convert.ToDateTime(dtBidDetailsRow["VALIDITY"].ToString()).ToString(strDateFormat) + "'");
                    strbBidDetails.Append(" BID_VALIDUPTO = '" + Utility.StringToDate(dtBidDetailsRow["VALIDITY"].ToString()) + "'");
                    //Modified by chandru on 26/03/2012
                    strbBidDetails.Append(" BID_VALUE = '" + dtBidDetailsRow["BID_VALUE"].ToString() + "'");
                    strbBidDetails.Append(" BID_AMOUNT = '" + dtBidDetailsRow["BID_AMOUNT"].ToString() + "'");
                    strbBidDetails.Append(" REMARKS = '" + Convert.ToString(dtBidDetailsRow["REMARKS"].ToString()) + "'/>");
                }
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Bid details cannot be empty");
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                return;
            }
            strbBidDetails.Append("</Root>");

            ObjSaleBidRow.XMLBIDDETAILS = strbBidDetails.ToString();
            objSaleBidDataTable.AddS3G_LR_SaleBidRow(ObjSaleBidRow);

            if (objSaleBidDataTable.Rows.Count > 0)
            {
                
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_ORG_SaleBid_DataTable = ClsPubSerialize.Serialize(objSaleBidDataTable, SerMode);

                if (intSaleBidId > 0)
                {
                    intErrorCode = ObjLegatMgtServicesClnt.FunPubUpdateSalesBidEntry(SerMode, byteobjS3G_ORG_SaleBid_DataTable);
                }
                else
                {
                    intErrorCode = ObjLegatMgtServicesClnt.FunPubInsertSalesBidEntry(out strDSNNo, SerMode, byteobjS3G_ORG_SaleBid_DataTable);
                }

                switch (intErrorCode)
                {
                    case 0:
                        TxtSBidBidNo.Text = strDSNNo;
                        if (intSaleBidId > 0)
                        {
                            //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = strAlert.Replace("__ALERT__", "Sales Bid details updated successfully");
                        }
                        else
                        {
                            //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = "Sales Bid Number " + strDSNNo + " has been added successfully";
                            strAlert += @"\n\nWould you like to add one more sale Bid Entry?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                        }
                        break;
                   
                    case -1:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        strRedirectPageView = "";
                        break;
                    case -2:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                        strRedirectPageView = "";
                        break;
                    default:
                        if (intSaleBidId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Sale Bid details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Sale Bid details");
                        }
                        strRedirectPageView = "";
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            //if (ObjLegatMgtServicesClnt != null)
                ObjLegatMgtServicesClnt.Close();

        }
    }

    public string ChkNull(string val)
    {
        return val == "" ? "0" : val;
    }
    private void FunPriGridviewRowDeleting(GridViewDeleteEventArgs e)
    {
        try
        { 
            DataTable dtBidDetails;
            dtBidDetails = (DataTable)ViewState["DetailsTable"];
            dtBidDetails.Rows.RemoveAt(e.RowIndex);
            grvBidDetails.DataSource = dtBidDetails;
            grvBidDetails.DataBind();
            if (dtBidDetails.Rows.Count == 0)
            { FunPriInitialRow(); grvBidDetails.Columns[0].Visible = false; }
            ViewState["currenttable"] = dtBidDetails;
            if (grvBidDetails.Rows.Count > 1)
            {
                TextBox TxtSBidBidValue = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidValue");
                TxtSBidBidValue.Text = ((Label)grvBidDetails.Rows[grvBidDetails.Rows.Count - 1].FindControl("lblSBidBidAmount")).Text;
            }
            else if (grvBidDetails.Rows.Count == 1)
            {
                if (((Label)grvBidDetails.Rows[0].FindControl("lblSBidBidAmount")).Text != string.Empty)
                {
                    TextBox TxtSBidBidValue = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidValue");
                    TxtSBidBidValue.Text = ((Label)grvBidDetails.Rows[0].FindControl("lblSBidBidAmount")).Text;
                }
                else
                {
                    TextBox TxtSBidBidValue = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidValue");
                    TxtSBidBidValue.Text = ((Label)GRVAssetDetails.Rows[0].FindControl("txtMarketvalue")).Text.ToString();
                } 
            }
            TextBox TxtSBidBidderName = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidderName");
            TxtSBidBidderName.Focus();
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (strMode == "M")
            {
                grvBidDetails.Rows[grvBidDetails.Rows.Count - 1].Enabled = false;
                grvBidDetails.FooterRow.Visible = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriGridviewRowcommand(GridViewCommandEventArgs e)
    
    {
        DataRow drBidDetails;
        DataTable dtBidDetails = (DataTable)ViewState["DetailsTable"];
        if (e.CommandName == "AddNew")
        {
            TextBox TxtSBidBidderName = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidderName");
            TextBox TxtSBidCntctDtls = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidCntctDtls");
            TextBox TxtSBidValidity = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidValidity");
            TextBox TxtSBidBidValue = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidValue");
            TextBox TxtSBidBidAmount = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidAmount");
            TextBox TxtSBidRemarks = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidRemarks");

            if (dtBidDetails.Rows.Count > 0)
            {
                if (dtBidDetails.Rows[0]["BID_NO"].ToString() == "0" && dtBidDetails.Rows[0]["BIDDER_NAME"].ToString() == "" && dtBidDetails.Rows[0]["BIDDER_ADDRESS"].ToString() == "" && dtBidDetails.Rows[0]["VALIDITY"].ToString() == "" && dtBidDetails.Rows[0]["BID_VALUE"].ToString() == "" && dtBidDetails.Rows[0]["BID_AMOUNT"].ToString() == "" && dtBidDetails.Rows[0]["REMARKS"].ToString() == "")
                { dtBidDetails.Rows[0].Delete(); }
            }
            if (TxtSBidBidderName.Text == "" && TxtSBidCntctDtls.Text == "" && TxtSBidValidity.Text == "" && TxtSBidBidValue.Text == "" && TxtSBidBidAmount.Text == "" && TxtSBidRemarks.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Add one row in Sale Bid details");
                TxtSBidBidderName.Focus();
                return;
            }
            if (TxtSBidBidderName.Text.Trim() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the Bidder Name");
                TxtSBidBidderName.Focus();
                return;
            }
            else if (TxtSBidCntctDtls.Text.Trim() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the Bidder Address");
                TxtSBidCntctDtls.Focus();
                return;
            }
            else if (TxtSBidValidity.Text.Trim() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the Bid Validity");
                TxtSBidValidity.Focus();
                return;
            }
            else if (TxtSBidBidValue.Text.Trim() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the Bid value");
                TxtSBidBidValue.Focus();
                return;
            }
            else if (TxtSBidBidAmount.Text.Trim() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the Bid Amount");
                TxtSBidBidAmount.Focus();
                return;
            }

            //else if (TxtSBidRemarks.Text.Trim() == "")
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Enter the Remarks");
            //    TxtSBidRemarks.Focus();
            //    return;

            //}
            if(Convert.ToDecimal(TxtSBidBidValue.Text)> Convert.ToDecimal(TxtSBidBidAmount.Text))
            {
                 Utility.FunShowAlertMsg(this.Page, "The Bid Amount should be greater the Bid Value");
                return;
            }
            DataView dvBidDetails = new DataView(dtBidDetails);
            dvBidDetails.Sort = "BID_NO";
            dvBidDetails.RowFilter = "(BIDDER_NAME='" + TxtSBidBidderName.Text.ToString().Trim()+ "' AND BID_AMOUNT="+ TxtSBidBidAmount.Text.ToString().Trim() + ") OR BID_AMOUNT=" + TxtSBidBidAmount.Text.ToString().Trim();
            if (dvBidDetails.ToTable().Rows.Count != 0)
            {
                Utility.FunShowAlertMsg(this.Page, "The Bid Amount already exists");
                return;
            }
            else
            {
                if (grvBidDetails.Rows.Count == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, "You cannot enter more than 5 bid entries");
                    return;
                }
                string ReposDate = ((Label)GRVAssetDetails.Rows[0].FindControl("lblRepDate")).Text.ToString();
                string BidValidity = ((TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidValidity")).Text.ToString();
                if (Utility.CompareDates(ReposDate,BidValidity) == -1)
                {
                    Utility.FunShowAlertMsg(this.Page, "The Bid Validity date should be greater that the Repossession date of the Asset");
                    return;
                }
                drBidDetails = dtBidDetails.NewRow();
                if (dtBidDetails.Rows.Count == 0)
                {
                    drBidDetails["BID_NO"] = "1";
                }
                else
                {
                    drBidDetails["BID_NO"] = Convert.ToInt32(dtBidDetails.Rows[dtBidDetails.Rows.Count-1]["BID_NO"].ToString()) + 1;
                }
                drBidDetails["BIDDER_NAME"] = TxtSBidBidderName.Text.Trim();
                drBidDetails["BIDDER_ADDRESS"] = TxtSBidCntctDtls.Text.Trim();
                drBidDetails["VALIDITY"] = Utility.StringToDate(TxtSBidValidity.Text.Trim()).ToString();
                drBidDetails["BID_VALUE"] = TxtSBidBidValue.Text.Trim();
                drBidDetails["BID_AMOUNT"] = TxtSBidBidAmount.Text.Trim();
                drBidDetails["REMARKS"] = TxtSBidRemarks.Text.Trim();
                dtBidDetails.Rows.Add(drBidDetails);
                grvBidDetails.DataSource = dtBidDetails;
                grvBidDetails.DataBind();
                ViewState["DetailsTable"] = dtBidDetails;
                grvBidDetails.Columns[0].Visible = true;
                TxtSBidBidderName.Focus();
                ((TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidValue")).Text  = TxtSBidBidAmount.Text.Trim();
                if (Request.QueryString["qsMode"] != null)
                    strMode = Request.QueryString["qsMode"];
                if (strMode == "M")
                {
                    grvBidDetails.Rows[grvBidDetails.Rows.Count - 1].Enabled = false;
                }
            }
            
        }
        else if (e.CommandName == "Edit")
        {
                //LinkButton LnkEdit= (LinkButton)e.CommandSource;
                //GridViewRow gvr = (GridViewRow)LnkEdit.NamingContainer;
                //int RowIndex=gvr.RowIndex;
                //grvBidDetails.EditIndex = RowIndex;
                //grvBidDetails.DataBind();
                ////((LinkButton)grvBidDetails.Rows[RowIndex].FindControl("LnkEdit")).Text = "Update";
                ////((LinkButton)grvBidDetails.Rows[RowIndex].FindControl("LnkEdit")).CommandName = "Update";
        }

    }

    public void ClearFields()
    {
        ddlBranch.Clear();
        //ddlSBidBranch.Items[0].Selected = true;
        ddlSBidSSNo.ClearSelection();
        ddlSBidSSNo.Items[0].Selected = true;
        ddSBidLOB.ClearSelection();
        ddSBidLOB.Items[0].Selected = true;
        GRVAssetDetails.ClearGrid();
        TxtSBidRefNo.Text = string.Empty;
        TxtSBidRepDocNo.Text = string.Empty;
        TxtSBidMLANo.Text = string.Empty;
        TxtSBidSLANo.Text = string.Empty;
        UcSBiddCustomerDetails.ClearCustomerDetails();
        DataTable dtBidDetails = (DataTable)ViewState["DetailsTable"];
        dtBidDetails.Rows.Clear();
        ViewState["DetailsTable"] = dtBidDetails;
        grvBidDetails.DataSource = null;
        grvBidDetails.DataBind();
        FunPriInitialRow();
        grvBidDetails.Enabled = false;
    }
    private void FunToolTip()
    {
        try
        {
            //throw new NotImplementedException();
            //ddlSBidBranch.ToolTip = LblSBidBranch.Text;
            ddlSBidSSNo.ToolTip = LblSBidSSNo.Text;
            ddSBidLOB.ToolTip = LblSBidLOB.Text;
            GRVAssetDetails.ToolTip = "Asset Information";
            TxtSBidMLANo.ToolTip = LblSBidMLA.Text;
            TxtSBidRefNo.ToolTip = LblSBidRefNo.Text;
            TxtSBidRepDocNo.ToolTip = LblSBidRepDocNo.Text;
            TxtSBidSLANo.ToolTip = LblSBidSLANo.Text;
                       
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;

        }
    }
    private void PopulateLOBList()
    {
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(_CompanyID));
            ////Procparam.Add("@Consitution_Id", txtConstitution.Attributes["Const_ID"]);
            //Procparam.Add("@User_ID", Convert.ToString(_UserID));
            //Procparam.Add("@Is_Active", "1");
            //ddSBidLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            /*Commented by Sriavtsan to use the latest code as below*/
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Add("OPTION", "7");
            //Procparam.Add("@COMPANYID", Convert.ToString(_CompanyID));
            //Procparam.Add("@USERID", Convert.ToString(_UserID));

            //if (intSaleBidId > 0)
            //{
            //    Procparam.Add("@Mode", "1");
            //}
            //ddSBidLOB.BindDataTable("S3G_LR_LOADLOV", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            /*Commented by Sriavtsan to use the latest code as below*/
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(_CompanyID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@FilterOption", "'HP','TL','TE','FT','WC'");
            Procparam.Add("@User_ID", _UserID.ToString());
            Procparam.Add("@Program_ID", "155");
            ddSBidLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddSBidLOB.Items.Count == 2)
            {
                ddSBidLOB.SelectedIndex = 1;
                ddSBidlLOB_SelectIndexChanged(this, new EventArgs());
            }
            else
            {
                ddSBidLOB.SelectedIndex = 0;

            }
        }
        catch (FaultException<LegalAndRepossessionMgtServicesReference.ClsPubFaultException> objFaultExp)
        { throw objFaultExp; }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void PopulateBranchList()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(_CompanyID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", _UserID.ToString());
            Procparam.Add("@Program_ID", "155");
            Procparam.Add("@Lob_Id", Convert.ToString(ddSBidLOB.SelectedValue));
            //ddlSBidBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //if (ddlSBidBranch.Items.Count == 2)
            //{
            //    ddlSBidBranch.SelectedIndex = 1;
                PopulateSaleNotificationNumber();
                ddlSBidSSNo.Enabled = true;
            
           
                //ddlSBidBranch.SelectedIndex = 0;
                ddlSBidSSNo.ClearDropDownList();
                ddlSBidSSNo.ClearSelection();
                ddlSBidSSNo.Enabled = false;
                TxtSBidMLANo.Clear();
                TxtSBidRefNo.Clear();
                TxtSBidRepDocNo.Clear();
                TxtSBidSLANo.Clear();
                UcSBiddCustomerDetails.ClearCustomerDetails();
                GRVAssetDetails.ClearGrid();
                grvBidDetails.ClearGrid();
                //FunProIntializeData();
                DataTable dtBidDetails = (DataTable)ViewState["DetailsTable"];
                dtBidDetails.Rows.Clear();
                ViewState["DetailsTable"] = dtBidDetails;
                FunPriInitialRow();
                grvBidDetails.Enabled = false;
            }
          
        catch (FaultException<LegalAndRepossessionMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void PopulateSaleNotificationNumber()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@LOB_ID", ddSBidLOB.SelectedValue);
            //Procparam.Add("@Location_ID", ddlSBidBranch.SelectedValue);
            Procparam.Add("@Company_ID", _CompanyID.ToString());
            Procparam.Add("@User_ID", _UserID.ToString());
            ddlSBidSSNo.BindDataTable(SPNames.S3G_LR_GetSaleNotificationIDforBid, Procparam, new string[] { "ID", "SNNO" });
           
        }
        catch (FaultException<LegalAndRepossessionMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriGetSaleNotificationDetails(int CompanyID,int SnID)
        {
        try
        {
            DataSet DS = new DataSet();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(CompanyID));
            Procparam.Add("@SN_ID", SnID.ToString());
            Procparam.Add("@Program_ID", "155");
            DS = Utility.GetDataset(SPNames.S3G_LR_GetSaleNotificationDetails, Procparam);

            // Table 0  Lob,Branch,Prime Account Number,Sub Account Number,and more
            if (DS.Tables[0].Rows.Count >= 1)
            {
               
                Session["CUSTOMERID"] = DS.Tables[0].Rows[0]["Customer_ID"].ToString();
                TxtSBidRepDocNo.Text = Convert.ToString(DS.Tables[0].Rows[0]["RepossNo_Text"]);
                TxtSBidMLANo.Text = Convert.ToString(DS.Tables[0].Rows[0]["PANUM"]); ;
                if (Convert.ToString(DS.Tables[0].Rows[0]["SANUM"]).Contains("DUMMY"))
                    TxtSBidSLANo.Text = string.Empty;
                else
                    TxtSBidSLANo.Text = Convert.ToString(DS.Tables[0].Rows[0]["SANUM"]);

                string strCustomerAddress = SetAddress(DS.Tables[0].Rows[0]["Comm_Address1"].ToString(), DS.Tables[0].Rows[0]["Comm_Address2"].ToString(), DS.Tables[0].Rows[0]["Comm_City"].ToString(), DS.Tables[0].Rows[0]["Comm_State"].ToString(), DS.Tables[0].Rows[0]["Comm_Country"].ToString(), DS.Tables[0].Rows[0]["Comm_Pincode"].ToString());

                //UcSBiddCustomerDetails.SetCustomerDetails(DS.Tables[0].Rows[0]["Customer_Code"].ToString(), strCustomerAddress, DS.Tables[0].Rows[0]["Customer_Name"].ToString(), DS.Tables[0].Rows[0]["Comm_Mobile"].ToString(), DS.Tables[0].Rows[0]["Comm_EMail"].ToString(), DS.Tables[0].Rows[0]["Comm_Website"].ToString());
                UcSBiddCustomerDetails.SetCustomerDetails(Convert.ToInt32(Session["CUSTOMERID"].ToString()),true);
            }
                       
           if (DS.Tables[1].Rows.Count >= 1)
            {
                Session["SALENOTIFASSTID"] = DS.Tables[1].Rows[0]["Sale_Notification_Asset_ID"].ToString();
                TxtSBidRefNo.Text = DS.Tables[1].Rows[0]["Regno"].ToString();
                PnlAssetDetails.Visible = true;
                GRVAssetDetails.DataSource = DS.Tables[1];
                GRVAssetDetails.DataBind();
            }
            else if (DS.Tables[1].Rows.Count == 0)
            {
                PnlAssetDetails.Visible = false;
                GRVAssetDetails.DataSource = null;
                GRVAssetDetails.DataBind();

            }

           TextBox TxtSBidBidValue = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidValue");
           TxtSBidBidValue.Text = ((Label)GRVAssetDetails.Rows[0].FindControl("txtMarketvalue")).Text.ToString();  
          
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    private void FunPriInitialRow()
    {
        try
        {
            DataTable dtBidDetails = (DataTable)ViewState["DetailsTable"];
            DataRow drBidDetails;
            
            drBidDetails = dtBidDetails.NewRow();
            drBidDetails["BID_NO"] = "0";
            drBidDetails["BIDDER_NAME"] = string.Empty;
            drBidDetails["BIDDER_ADDRESS"] = string.Empty;
            drBidDetails["VALIDITY"] = string.Empty;
            drBidDetails["BID_VALUE"] = string.Empty;
            drBidDetails["BID_AMOUNT"] = string.Empty;
            drBidDetails["REMARKS"] = string.Empty;
            dtBidDetails.Rows.Add(drBidDetails);
            grvBidDetails.DataSource = dtBidDetails;
            grvBidDetails.DataBind();
            grvBidDetails.Rows[0].Visible = false;
            ViewState["DetailsTable"] = dtBidDetails;

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    public static string SetAddress(string Address1, string Address2, string City, string State, string Country, string Pincode)
    {
        try
        {
            string strAddress = "";
            if (Address1.ToString() != "") strAddress += Address1.ToString() + System.Environment.NewLine;
            if (Address2.ToString() != "") strAddress += Address2.ToString() + System.Environment.NewLine;
            if (City.ToString() != "") strAddress += City.ToString() + System.Environment.NewLine;
            if (State.ToString() != "") strAddress += State.ToString() + System.Environment.NewLine;
            if (Country.ToString() != "") strAddress += Country.ToString() + System.Environment.NewLine;
            if (Pincode.ToString() != "") strAddress += Pincode.ToString();
            return strAddress;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriSaleBidControlStatus(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                ChkSBidIsActive.Checked = true;
                ChkSBidIsActive.Enabled = false;
                //txtEntityCode.Enabled = true;
                //txtEntityName.Enabled = true;
                //ddlSBidBranch.Enabled = false;
                ddlSBidSSNo.Enabled = false;
                btnClear.Enabled = true;
                grvBidDetails.Enabled = false;
                grvBidDetails.Columns[0].Visible = false;
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                break;

            case 1: //Modify

                 lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                //tcEntityMaster.Tabs[1].Enabled = true;
                //txtEntityCode.Enabled = false;
                ////txtCountry.AutoPostBack = true;
                //txtEntityName.Enabled = false;
                // btnModify.Visible = true;
                 ddSBidLOB.Enabled = false;
                ddlBranch.Enabled = false;
                ddlSBidSSNo.Enabled = false;
                btnClear.Enabled = false;
                grvBidDetails.Columns[0].Visible = true;
                grvBidDetails.FooterRow.Visible = false;
                //btnSave.Enabled = true;
                // btnAdd.Enabled = false;
                //btnModify.Enabled = false;
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }

                break;
            case -1://Query

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                ChkSBidIsActive.Enabled = false;
                ddlBranch.Enabled = false;
                ddlSBidSSNo.Enabled = false;
                ddSBidLOB.Enabled = false;
                //GRVAssetDetails.Enabled = false;

                //TxtSBidMLANo.Enabled = false;
                //TxtSBidRefNo.Enabled = false;
                //TxtSBidSLANo.Enabled = false;
                //TxtSBidRepDocNo.Enabled = false;
                              
                //grvBidDetails.Enabled = false;
                grvBidDetails.Columns[0].Visible = true;
                grvBidDetails.Columns[7].Visible = false;
                grvBidDetails.FooterRow.Visible = false;
                break;
        }
    }

    private string ConvertToCurrentFormat(string strDate)
    {
        try
        {
            if (strDate.Contains("1900"))
                strDate = string.Empty;
            strDate = strDate.Replace("12:00:00 AM", "");
            CultureInfo myDTFI = new CultureInfo("en-GB", true);
            DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
            DTF.ShortDatePattern = _DateFormat;
            DateTime _Date = new DateTime();
            if (strDate != "")
            {
                _Date = System.Convert.ToDateTime(strDate, DTF);
                return _Date.ToString(_DateFormat);
            }
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private DateTime LastDate(DateTime dt)
    {
        try
        {
            DateTime dtTo = dt;
            dtTo = dt.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));
            return dtTo;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #region  DateFormat
    public string FormatDate(string strDate)
    {
        return DateTime.Parse(strDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
    }
    #endregion
    #endregion

    #region GridView Events
   
    protected void grvBidDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSBidValidity = (Label)e.Row.FindControl("lblSBidValidity");
                if (lblSBidValidity.Text.Trim() != string.Empty)
                {
                    DateTime Date = Convert.ToDateTime(lblSBidValidity.Text);
                    lblSBidValidity.Text = Date.ToString(strDateFormat);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                S3GSession ObjS3GSession = new S3GSession();
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_TxtSBidValidity = e.Row.FindControl("CalendarExtenderSD_TxtSBidValidity") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_TxtSBidValidity.Format = ObjS3GSession.ProDateFormatRW;
                ((TextBox)e.Row.FindControl("TxtSBidBidValue")).SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, true, "Bid Value");
                ((TextBox)e.Row.FindControl("TxtSBidBidAmount")).SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, true, "Bid Amount");
            }
            TextBox TxtSBidValidity = (TextBox)e.Row.FindControl("TxtSBidValidity");

            TxtSBidValidity.Attributes.Add("onblur", "fnDoDate(this,'" + TxtSBidValidity.ClientID + "','" + strDateFormat + "',false,  false);");

            if (PageMode == PageModes.Query)
            {
                TxtSBidValidity.Attributes.Add("readonly", "true");
                TxtSBidValidity.Attributes.Remove("onblur");
            }
        }
        catch (Exception objException)
        {
            cvSaleBidentry.ErrorMessage = objException.Message;
            cvSaleBidentry.IsValid = false;
        }
    }

    protected void grvBidDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FunPriGridviewRowcommand(e);
    }
    protected void grvBidDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunPriGridviewRowDeleting(e);
    }
    protected void grvBidDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {

            S3GSession ObjS3GSession = new S3GSession();
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_TxtSBidValidity = e.Row.FindControl("CalendarExtenderSD_TxtSBidValidity") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_TxtSBidValidity.Format = ObjS3GSession.ProDateFormatRW;
        }

    }
    #endregion

    #region Retrieve Bid Details

    public void FunPubProGetSaleBidDetails(int intCompanyId, int SalesBidId)
    {
        ObjLegatMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {
            DataSet dsSalesBidDetails;
           
            byte[] byte_BidDetails = ObjLegatMgtServicesClnt.FunPubGetSalesBidDetails(intCompanyId,SalesBidId);
            dsSalesBidDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_BidDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsSalesBidDetails.Tables.Count > 0 && intCompanyId > 0)
            {
                DataTable dtSaleBid = dsSalesBidDetails.Tables[0];
                intSaleBidId =Convert.ToInt32(dtSaleBid.Rows[0]["Sale_Bid_ID"].ToString());
                Session["SALEBIDID"] = intSaleBidId;
                TxtSBidBidNo.Text = dtSaleBid.Rows[0]["Sale_Bid_No"].ToString();
                Session["CUSTOMERID"] = dtSaleBid.Rows[0]["Customer_ID"].ToString();

                ddSBidLOB.Items.Add("--Select--");
                ListItem item = new ListItem(dtSaleBid.Rows[0]["LOB_NAME"].ToString(), dtSaleBid.Rows[0]["LOB_ID"].ToString());
                ddSBidLOB.Items.Add(item);
                ddSBidLOB.Items[1].Selected = true;


                //ddlSBidBranch.Items.Add("--Select--");
                item = new ListItem(dtSaleBid.Rows[0]["Location_Name"].ToString(), dtSaleBid.Rows[0]["Location_ID"].ToString());
                ddlBranch.SelectedValue = dtSaleBid.Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = dtSaleBid.Rows[0]["Location_Name"].ToString();
                //ddlSBidBranch.Items[1].Selected = true;
                               
                
                ddlSBidSSNo.Items.Add("--Select--");
                item = new ListItem(dtSaleBid.Rows[0]["Sale_Notification_No"].ToString(), dtSaleBid.Rows[0]["Sale_Notification_ID"].ToString());
                ddlSBidSSNo.Items.Add(item);
                ddlSBidSSNo.Items[1].Selected = true;
                ddlSBidSSNo_SelectIndexChanged(this, new EventArgs());
                
                ChkSBidIsActive.Checked =Convert.ToBoolean(dtSaleBid.Rows[0]["Is_Active"].ToString()) ;
                
                string strCustomerAddress = SetAddress(dtSaleBid.Rows[0]["Comm_Address1"].ToString(), dtSaleBid.Rows[0]["Comm_Address2"].ToString(), dtSaleBid.Rows[0]["Comm_City"].ToString(), dtSaleBid.Rows[0]["Comm_State"].ToString(), dtSaleBid.Rows[0]["Comm_Country"].ToString(), dtSaleBid.Rows[0]["Comm_Pincode"].ToString());

                UcSBiddCustomerDetails.SetCustomerDetails(Convert.ToInt32(dtSaleBid.Rows[0]["Customer_ID"].ToString()),true);

                TxtSBidMLANo.Text = dtSaleBid.Rows[0]["PANum"].ToString();
                TxtSBidSLANo.Text = dtSaleBid.Rows[0]["SANum"].ToString();
                hdnID.Value = dtSaleBid.Rows[0]["Created_By"].ToString();
                
                DataTable dtBidDetails = dsSalesBidDetails.Tables[1];
                ViewState["DetailsTable"] = dtBidDetails;
                Session["SALENOTIFASSTID"] = dtBidDetails.Rows[0]["Sale_Notification_Asset_ID"].ToString();
                grvBidDetails.DataSource = dtBidDetails;
                grvBidDetails.DataBind();
                TextBox TxtSBidBidValue = (TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidBidValue");
                TxtSBidBidValue.Text = dtBidDetails.Rows[dtBidDetails.Rows.Count - 1]["Bid_Amount"].ToString();
                grvBidDetails.Rows[grvBidDetails.Rows.Count - 1].Enabled = false;
                            
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Entity details");
        }
    }
    #endregion

    protected void TxtSBidValidity_TextChanged(object sender, EventArgs e)
    {

        string ReposDate = ((Label)GRVAssetDetails.Rows[0].FindControl("lblRepDate")).Text.ToString();
        string BidValidity = ((TextBox)grvBidDetails.FooterRow.FindControl("TxtSBidValidity")).Text.ToString();
        //if (!string.IsNullOrEmpty(TxtSBidValidity.Text))
        //{
            if (Utility.CompareDates(ReposDate, BidValidity) == -1)
            {
                Utility.FunShowAlertMsg(this.Page, "The Bid Validity date should be greater that the Repossession date of the Asset");
                return;
            }
        //}
    }
}
