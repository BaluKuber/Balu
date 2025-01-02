#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Legal & Repossession
/// Screen Name			: Sale Invoice Entry
/// Created By			: Srivatsan S
/// Created Date		: 07-May-2011
/// Purpose	            : This module is used to store and retrieve Sale Invoice Entry details
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

public partial class LegalRepossession_S3GLRSaleInvoiceEntry : ApplyThemeForProject
{
    #region Initialization
    int _CompanyID, _UserID, _CustomerID, _SlNo = 0;
    string _DateFormat = "dd/MM/yyyy";
    Dictionary<string, string> Procparam = null;
    string strDateFormat = string.Empty;
    string strDSNNo = string.Empty;
    string strBidNo = string.Empty;
    Decimal decRoundOff = 0;
    int intSaleInvId = 0;
    int intErrorCode = 0;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    StringBuilder strbBidDetails = new StringBuilder();
    StringBuilder strbRcptDetails = new StringBuilder();
    string strRedirectPage = "../LegalRepossession/S3GLRTransLander.aspx?Code=GSIE";
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRSaleInvoiceEntry.aspx'";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GSIE';";
    LEGAL.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceDataTable objSaleInvDataTable;
    LEGALSERVICES.LegalAndRepossessionMgtServicesClient ObjLegalMgtServicesClnt;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    S3GSession ObjS3GSession;
    decimal intReceiptTotal = 0;
    public static LegalRepossession_S3GLRSaleInvoiceEntry obj_Page;

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        try
        {
            obj_Page = this;
            FunPubSetIndex(1);
            UserInfo ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _CompanyID = ObjUserInfo.ProCompanyIdRW;
            _UserID = ObjUserInfo.ProUserIdRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            TxtSInvInvDate.Text = DateTime.Now.ToString(strDateFormat);
            Session["RcptTotal"] = intReceiptTotal;
            if (!IsPostBack)
            {

                ViewState["CUSTOMERID"] = ViewState["SALEINVID"] = ViewState["SALENOTIFASSTID"] = ""; ViewState["Status"] = 0;
                if (Request.QueryString["qsMode"] != null)
                    strMode = Request.QueryString["qsMode"];
                if (Request.QueryString["qsViewId"] != string.Empty && Request.QueryString["qsViewId"] != null)
                {
                    FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                    intSaleInvId = Convert.ToInt32(fromTicket.Name);
                }
                FunProIntializeData();
                FunProIntializeInvoiceGV();
                FunProIntializeReceiptGV();
             
                
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (intSaleInvId > 0)
                {
                    bool blnTranExists;

                    FunPubProGetSaleInvDetails(_CompanyID, intSaleInvId);
                 
                    if (strMode == "Q")
                    {
                        ViewState["Status"] = -1;
                        FunPriSaleInvControlStatus(-1);
                    }
                }
                else
                {
                    //PopulateBranchList();
                    PopulateLOBList();
                    FunPriSaleInvControlStatus(0);
                }
             }
           
        }
        catch (Exception ex)
        {
            cvSaleInventry.ErrorMessage = ex.Message;
            cvSaleInventry.IsValid = false;
        }

    }
   protected void FunProIntializeData()
    {
        try
        {
            // Invoice Details Datatable

            DataTable dtInvoiceDetails;
            dtInvoiceDetails = new DataTable("INVDETAILS");
            dtInvoiceDetails.Columns.Add("ACCNT_TYPE");
            dtInvoiceDetails.Columns.Add("ACCNT_CODE");
            dtInvoiceDetails.Columns.Add("DBT_GL_CODE");
            dtInvoiceDetails.Columns.Add("DBT_SL_CODE");
            dtInvoiceDetails.Columns.Add("DBT_GL_DESC");
            dtInvoiceDetails.Columns.Add("CRDT_GL_CODE");
            dtInvoiceDetails.Columns.Add("CRDT_SL_CODE");
            dtInvoiceDetails.Columns.Add("CRDT_GL_DESC");
            dtInvoiceDetails.Columns.Add("AMNT");
            ViewState["InvoiceDetailsTable"] = dtInvoiceDetails;

            //Receipt Details Datatable
            DataTable dtReceipteDetails;
            dtReceipteDetails = new DataTable("RCPTDETAILS");
            dtReceipteDetails.Columns.Add("RECEIPT_ID");
            dtReceipteDetails.Columns.Add("RECEIPT_NO");
            dtReceipteDetails.Columns.Add("TRXNDT");
            dtReceipteDetails.Columns.Add("TRXNAMNT");
            ViewState["ReceiptDetailsTable"] = dtReceipteDetails;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to Intialize data");
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
       Procparam.Add("@Company_ID", obj_Page.CompanyId.ToString());
       Procparam.Add("@Type", "GEN");
       Procparam.Add("@User_ID", obj_Page.UserId.ToString());
       Procparam.Add("@Program_Id", "141");
       Procparam.Add("@Lob_Id", obj_Page.ddlSInvLOB.SelectedValue);
       Procparam.Add("@Is_Active", "1");
       Procparam.Add("@PrefixText", prefixText);
       suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

       return suggetions.ToArray();
   }

    protected void FunProIntializeInvoiceGV()
    {
        DataTable dtInvoiceDetails = (DataTable)ViewState["InvoiceDetailsTable"];
        DataRow drInvoiceDetails = dtInvoiceDetails.NewRow();
        drInvoiceDetails["ACCNT_TYPE"] = "";
        drInvoiceDetails["ACCNT_CODE"] = "";
        drInvoiceDetails["DBT_GL_CODE"] = "";
        drInvoiceDetails["DBT_SL_CODE"] = "";
        drInvoiceDetails["DBT_GL_DESC"] = "";
        drInvoiceDetails["CRDT_GL_CODE"] = "";
        drInvoiceDetails["CRDT_SL_CODE"] = "";
        drInvoiceDetails["CRDT_GL_DESC"] = "";
        drInvoiceDetails["AMNT"] = "";
        dtInvoiceDetails.Rows.Add(drInvoiceDetails);
        GrvSInvoiceDetails.DataSource = dtInvoiceDetails;
        GrvSInvoiceDetails.DataBind();
        ViewState["InvoiceDetailsTable"] = dtInvoiceDetails;
   }

    protected void FunProIntializeReceiptGV()
    {
        DataTable dtReceipteDetails = (DataTable)ViewState["ReceiptDetailsTable"];
        DataRow drReceipteDetails = dtReceipteDetails.NewRow();
        drReceipteDetails["RECEIPT_ID"] = "";
        drReceipteDetails["RECEIPT_NO"] = "";
        drReceipteDetails["TRXNDT"] = "";
        drReceipteDetails["TRXNAMNT"] = "";
        dtReceipteDetails.Rows.Add(drReceipteDetails);
        GrvReceiptDetails.DataSource = dtReceipteDetails;
        GrvReceiptDetails.DataBind();
        ViewState["ReceiptDetailsTable"] = dtReceipteDetails;
    }
    private void FunPubProGetSaleInvDetails(int _CompanyID, int intSaleInvId)
    {
        ObjLegalMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {
            DataSet dsSalesInvDetails;
            
            byte[] byte_InvDetails = ObjLegalMgtServicesClnt.FunPubGetSalesInvoiceDetails(_CompanyID, intSaleInvId);
            dsSalesInvDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_InvDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsSalesInvDetails.Tables.Count > 0 && _CompanyID > 0)
            {
                DataTable dtSaleInv = dsSalesInvDetails.Tables[0];
                intSaleInvId = Convert.ToInt32(dtSaleInv.Rows[0]["Sale_Invoice_ID"].ToString());
                ViewState["SALEINVID"] = intSaleInvId;
                ViewState["SALENOTIFASSTID"] = dtSaleInv.Rows[0]["Sale_Notification_Asset_ID"].ToString();
                TxtSInvInvNo.Text = dtSaleInv.Rows[0]["SIE_NO"].ToString();
                TxtSInvInvDate.Text = ConvertToCurrentFormat(dtSaleInv.Rows[0]["SIE_Date"].ToString());
                ViewState["CUSTOMERID"] = dtSaleInv.Rows[0]["Customer_ID"].ToString();
                
                ddlSInvLOB.ClearSelection();
                ddlSInvLOB.Items.Add("--Select--");
                ListItem item = new ListItem(dtSaleInv.Rows[0]["LOB_NAME"].ToString(), dtSaleInv.Rows[0]["LOB_ID"].ToString());
                ddlSInvLOB.Items.Add(item);
                ddlSInvLOB.Items[1].Selected = true;

                ddlSInvBranch.Clear();
                //ddlSInvBranch.Items.Add("--Select--");
                item = new ListItem(dtSaleInv.Rows[0]["LocationName"].ToString(), dtSaleInv.Rows[0]["LOCATION_ID"].ToString());
                ddlSInvBranch.SelectedText = item.ToString();
                //ddlSInvBranch.Items[1].Selected = true;
                ddlSInvBranch_SelectedIndexChanged(this, new EventArgs());
                
                ddlSInvSSNo.ClearSelection();
                ddlSInvSSNo.Items.Add("--Select--");
                item = new ListItem(dtSaleInv.Rows[0]["Sale_Notification_No"].ToString(), dtSaleInv.Rows[0]["Sale_Notification_ID"].ToString());
                ddlSInvSSNo.Items.Add(item);
                ddlSInvSSNo.Items[1].Selected = true;
                
                ChkSInvIsActive.Checked = Convert.ToBoolean(dtSaleInv.Rows[0]["Is_Active"].ToString());

                DataTable dtInvDetails = dsSalesInvDetails.Tables[1];
                ViewState["DetailsTable"] = dtInvDetails;
                GrvSInvoiceDetails.DataSource = dtInvDetails;
                GrvSInvoiceDetails.DataBind();
                ((TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal")).Text = dtInvDetails.Rows[0]["AMNT"].ToString();
                DropDownList ddlSInvAccountType = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvAccountType");
                DropDownList ddlSInvAccountCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvAccountCode");
                DropDownList ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvDebitSLCode");
                DropDownList ddlSInvCrdSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvCrdSLCode");
                
                ddlSInvAccountType.ClearSelection();
                ddlSInvAccountType.Items.FindByValue(dtInvDetails.Rows[0]["Sale_Type"].ToString()).Selected = true;
                ddlSInvAccountType_SelectedIndexChanged(ddlSInvAccountType, new EventArgs());
                ddlSInvAccountCode.ClearSelection();
                ddlSInvAccountCode.Items.FindByValue(dtInvDetails.Rows[0]["Sale_Account_Code"].ToString()).Selected = true;
                ddlSInvAccountCode_SelectedIndexChanged(ddlSInvAccountCode, new EventArgs());
                ViewState["INVRECEIPTDETAILS"] = dsSalesInvDetails.Tables[2];
                FunPriGetSaleNotificationDetails(_CompanyID, Convert.ToInt32(dtSaleInv.Rows[0]["Sale_Notification_ID"].ToString()));
                ddlSInvDebitSLCode.ClearSelection();
                ddlSInvDebitSLCode.Items.FindByText(dtInvDetails.Rows[0]["Debit_SL_Account"].ToString()).Selected = true;
                if (dtInvDetails.Rows[0]["Debit_SL_Account"].ToString() == "" || dtInvDetails.Rows[0]["Debit_SL_Account"].ToString() == "0")
                {
                    ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[0].FindControl("rfvddlSInvDebitSLCode")).Enabled = false;
                }
                //ddlSInvCrdSLCode.ClearSelection();
                //ddlSInvCrdSLCode.Items.FindByText(dtInvDetails.Rows[0]["Credit_SL_Account"].ToString()).Selected = true;
               
                DataTable dtRcptdetails= dsSalesInvDetails.Tables[2];
                if(dtRcptdetails.Rows.Count > 0)
                {
                    ViewState["INVRECEIPTDETAILS"] = dtRcptdetails;
                    GrvReceiptDetails.DataSource=dtRcptdetails;
                    GrvReceiptDetails.DataBind();

                    if (strMode == "M")
                    {
                            Utility.FunShowAlertMsg(Page, "As receipt(s) exists for this invoice, you would not be able to modify the contents. Hence, showing in Query mode!");
                            ViewState["Status"] = -1;
                            FunPriSaleInvControlStatus(-1);
                                            
                    }
                        decimal total = 0;
                        TextBox TxtSInvTxnTotal = (TextBox)GrvReceiptDetails.FooterRow.FindControl("TxtSInvTxnTotal");
                        foreach (GridViewRow gvr in GrvReceiptDetails.Rows)
                        {
                            CheckBox ChkRcpt = (CheckBox)gvr.FindControl("ChkRcpt");
                            TextBox TxtSInvTxnAmnt = (TextBox)gvr.FindControl("TxtSInvTxnAmnt");
                            ChkRcpt.Checked = true;
                            if ((ChkRcpt.Checked == true))
                            {
                                total += Convert.ToDecimal(TxtSInvTxnAmnt.Text.ToString().Trim());
                            }

                        }
                        TxtSInvTxnTotal.Text = Convert.ToString(total);
                    
                 }
                else 
                { 
                    //if (Request.QueryString["qsMode"] != null)
                    //strMode = Request.QueryString["qsMode"];
                    if(strMode=="M")
                    {
                    FillReceipts();
                    FunPriSaleInvControlStatus(1);
                    }
                    if(strMode=="Q") 
                    {
                        GrvReceiptDetails.EmptyDataText="No Records Found!";
                        GrvReceiptDetails.DataBind();

                    }
                }
             }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Entity details");
        }
        finally
        {
            //if (ObjLegalMgtServicesClnt != null)
                ObjLegalMgtServicesClnt.Close();
        }
    
    }
    #endregion
    protected void ddlSInvLOB_SelectIndexChanged(object sender, EventArgs e)
    {  
       if ((ddlSInvLOB.SelectedIndex > 0))
        {
            ddlSInvBranch.Enabled = true;
            //PopulateBranchList();
            //ddlSInvBranch.Item_Selected= true;
            ddlSInvSSNo.Enabled = false;
          
        }
        else
        {
            ddlSInvBranch.Clear();
            ddlSInvBranch.Enabled = false;
            ddlSInvSSNo.ClearSelection();
            ddlSInvSSNo.Enabled = false;
            TxtSInvMLANo.Clear();
            TxtSInvRefNo.Clear();
            TxtSInvRepDocNo.Clear();
            TxtSInvSLANo.Clear();
            UcSInvCustomerDetails.ClearCustomerDetails();
            GRVAssetDetails.ClearGrid();
            GrvBidDetails.ClearGrid();
            GrvSInvoiceDetails.Enabled = false;
            GrvReceiptDetails.ClearGrid();
            GrvReceiptDetails.Enabled = false;
        }
     }
    
    protected void ddlSInvBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];
        if (strMode != "M")
        {
            //if ((ddlSInvBranch.SelectedIndex > 0))
            //{
            if ((ddlSInvBranch.SelectedValue != "0"))
            {
                PopulateSaleNotificationNumber();
                ddlSInvSSNo.Enabled = true;
                ddlSInvSSNo.Items[0].Selected = true;
            }
            else
            {
                ddlSInvSSNo.ClearSelection();
                ddlSInvSSNo.Enabled = false;
                TxtSInvMLANo.Clear();
                TxtSInvRefNo.Clear();
                TxtSInvRepDocNo.Clear();
                TxtSInvSLANo.Clear();
                UcSInvCustomerDetails.ClearCustomerDetails();
                GRVAssetDetails.ClearGrid();
                GrvBidDetails.ClearGrid();
                GrvSInvoiceDetails.Enabled = false;
                GrvReceiptDetails.ClearGrid();
                GrvReceiptDetails.Enabled = false;
            }
        }
    }
    protected void ddlSInvSSNo_SelectIndexChanged(object sender, EventArgs e)
    {
        if (ddlSInvSSNo.SelectedIndex != 0)
        {
            FunPriGetSaleNotificationDetails(_CompanyID, Convert.ToInt32(ddlSInvSSNo.SelectedValue));
            GrvReceiptDetails.Enabled = true;
            GrvSInvoiceDetails.Enabled = true;
        }
        else
        {
            TxtSInvMLANo.Clear();
            TxtSInvRefNo.Clear();
            TxtSInvRepDocNo.Clear();
            TxtSInvSLANo.Clear();
            UcSInvCustomerDetails.ClearCustomerDetails();
            GRVAssetDetails.ClearGrid();
            GrvBidDetails.ClearGrid();
            GrvSInvoiceDetails.Enabled = false;
            GrvReceiptDetails.ClearGrid();
            GrvReceiptDetails.Enabled = false;
        }
    }
   
    protected void GRVAssetDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Label lblRepDate = (Label)e.Row.FindControl("lblRepDate");
            //    if (lblRepDate.Text.Trim() != string.Empty)
            //    {
            //        DateTime Date = Convert.ToDateTime(lblRepDate.Text);
            //        lblRepDate.Text = Date.ToString(strDateFormat);
            //    }
            //}
        }
        catch (Exception objException)
        {
            cvSaleInventry.ErrorMessage = objException.Message;
            cvSaleInventry.IsValid = false;
        }

    }
    protected void GrvBidDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRepDate = (Label)e.Row.FindControl("lblRepDate");
                if (lblRepDate.Text.Trim() != string.Empty)
                {
                    DateTime Date = Convert.ToDateTime(lblRepDate.Text);
                    lblRepDate.Text = Date.ToString(strDateFormat);
                }
            }
        }
        catch (Exception objException)
        {
            cvSaleInventry.ErrorMessage = objException.Message;
            cvSaleInventry.IsValid = false;
        }
    }
    protected void GrvReceiptDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (((TextBox)e.Row.FindControl("TxtRcptRefNo")).Text.ToString().Trim() != string.Empty)
        //    {
        //        if (ViewState["INVRECEIPTDETAILS"] != null)
        //        {
        //            DataTable dtReceipt = (DataTable)ViewState["INVRECEIPTDETAILS"];
        //            if (dtReceipt.Rows.Count > 0)
        //            {
        //                DataView dvReceipt = new DataView(dtReceipt);
        //                dvReceipt.Sort = "Receipt_No";
        //                dvReceipt.RowFilter = "Receipt_No='" + ((TextBox)e.Row.FindControl("TxtRcptRefNo")).Text.ToString().Trim() +"'";
        //                if (dvReceipt.ToTable().Rows.Count > 0)
        //                {
        //                    e.Row.Enabled = false;
        //                    ((CheckBox)e.Row.FindControl("ChkRcpt")).Checked = true;
        //                }
        //                else
        //                {
        //                    e.Row.Enabled = true;
        //                }
        //            }
        //        }
        //    }
        //}
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
            cvSaleInventry.ErrorMessage = ex.Message;
            cvSaleInventry.IsValid = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        GRVAssetDetails.ClearGrid();
        TxtSInvInvDate.Text =DateTime.Now.ToString(strDateFormat);
        TxtSInvInvNo.Text = "";
        TxtSInvMLANo.Text = "";
        TxtSInvRefNo.Text = "";
        TxtSInvRepDocNo.Text = "";
        TxtSInvSLANo.Text = "";
        ddlSInvBranch.Clear();
        ddlSInvLOB.ClearSelection();
        ddlSInvSSNo.ClearSelection();
        UcSInvCustomerDetails.ClearCustomerDetails();
        GrvBidDetails.DataSource = null;
        GrvBidDetails.DataBind();
        GrvSInvoiceDetails.ClearGrid();
        GrvReceiptDetails.ClearGrid();
        GrvSInvoiceDetails.Enabled = false;
        GrvReceiptDetails.Enabled = false;
        ViewState["InvoiceDetailsTable"] = null;
        ViewState["ReceiptDetailsTable"] = null;
        FunProIntializeData();
        FunProIntializeInvoiceGV();
        FunProIntializeReceiptGV();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ex)
        {
            cvSaleInventry.ErrorMessage = ex.Message;
            cvSaleInventry.IsValid = false;
        }
    }
    protected void ddlSInvAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow GrvRow = (GridViewRow)((DropDownList)sender).NamingContainer;
        int RowIndex = GrvRow.RowIndex;

        DropDownList ddlSInvAccountCode;
        Label LblSInvDebitGLCode;
        DropDownList ddlSInvDebitSLCode;
        Label LblSInvGlDesc;
        Label LblSInvCrdGLCode;
        DropDownList ddlSInvCrdSLCode;
        //TextBox TxtSInvAmount;
        //TextBox TxtSInvTotal;

        ddlSInvAccountCode = (DropDownList)GrvRow.FindControl("ddlSInvAccountCode");
        ddlSInvAccountCode.ClearSelection();
        ddlSInvDebitSLCode = (DropDownList)GrvRow.FindControl("ddlSInvDebitSLCode");
        ddlSInvDebitSLCode.ClearSelection();
        ddlSInvCrdSLCode = (DropDownList)GrvRow.FindControl("ddlSInvCrdSLCode");
        ddlSInvCrdSLCode.ClearSelection();

        LblSInvDebitGLCode = (Label)GrvRow.FindControl("LblSInvDebitGLCode");
        LblSInvDebitGLCode.Text = "";
        LblSInvGlDesc = (Label)GrvRow.FindControl("LblSInvGlDesc");
        LblSInvGlDesc.Text = "";
        LblSInvCrdGLCode = (Label)GrvRow.FindControl("LblSInvCrdGLCode");
        LblSInvCrdGLCode.Text = "";


        switch (((DropDownList)sender).SelectedValue)
        {
            case "E":
                {
                    GrvSInvoiceDetails.Columns[1].Visible = true;
                    //GrvSInvoiceDetails.Columns[1].HeaderText = "Entity Code";
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", _CompanyID.ToString());
                    ((DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvAccountCode")).BindDataTable(SPNames.S3G_LR_GetEntity, Procparam, new string[] { "Entity_ID", "Entity" });
                    break;
                }
            case "C":
                {
                    GrvSInvoiceDetails.Columns[1].Visible = true;
                    //GrvSInvoiceDetails.Columns[1].HeaderText = "Customer Code";
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", _CompanyID.ToString());
                    Procparam.Add("@Cust_id", ViewState["CUSTOMERID"].ToString());
                    ((DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvAccountCode")).BindDataTable(SPNames.S3G_LR_GetCustomer, Procparam, new string[] { "Customer_ID", "Customer_Code" });
                    break;
                }

        }

    }
    protected void ddlSInvAccountCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObjLegalMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {
            GridViewRow GrvRow = (GridViewRow)((DropDownList)sender).NamingContainer;
            int RowIndex = GrvRow.RowIndex;
            string AccType = ((DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvAccountType")).SelectedValue;
            string AccCode = ((DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvAccountCode")).SelectedValue;
           
            switch (AccType)
            {
                case "E":
                    {
                        DataSet dsSaleDebCredGLCode;
                        
                        byte[] byte_SaleDebCredGLCode = ObjLegalMgtServicesClnt.FunPubGetSaleDebCredGLCode(_CompanyID, AccCode, 0);
                        dsSaleDebCredGLCode = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_SaleDebCredGLCode, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));

                        DataTable dtSaleDebCredGLCode = dsSaleDebCredGLCode.Tables[0];
                        if (dtSaleDebCredGLCode.Rows.Count != 0)
                        {
                            Label LblSInvDebitGLCode = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvDebitGLCode");
                            Label LblSInvGlDesc = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvGlDesc");
                            LblSInvDebitGLCode.Text = dtSaleDebCredGLCode.Rows[0]["GL_Code"].ToString();
                            LblSInvGlDesc.Text = dtSaleDebCredGLCode.Rows[0]["Account_Code_Desc"].ToString();
                        }

                        //byte_SaleDebCredGLCode = ObjLegalMgtServicesClnt.FunPubGetSaleDebCredSLCode(_CompanyID, dtSaleDebCredGLCode.Rows[0]["GL_Code"].ToString(), Convert.ToInt32(ddlSInvLOB.SelectedValue), Convert.ToInt32(ddlSInvBranch.SelectedValue));
                        //dsSaleDebCredGLCode = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_SaleDebCredGLCode, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
                        //if (dsSaleDebCredGLCode.Tables[0].Rows.Count != 0)
                        //{
                        //    if (dsSaleDebCredGLCode.Tables[0].Rows.Count == 1)
                        //    {
                        //        DropDownList ddlSInvDebitSLCode = new DropDownList();
                        //        ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                        //        ddlSInvDebitSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                        //        ddlSInvDebitSLCode.Items[1].Selected = true;
                        //        ddlSInvDebitSLCode.Enabled = true;
                        //        ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = true;
                        //     }
                        //    else
                        //    {
                        //        DropDownList ddlSInvDebitSLCode = new DropDownList();
                        //        ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                        //        ddlSInvDebitSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                        //        ddlSInvDebitSLCode.Enabled = true;
                        //        ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = true;
                        //    }
                        //}
                        //else
                        //{
                        //    DropDownList ddlSInvDebitSLCode = new DropDownList();
                        //    ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                        //    ddlSInvDebitSLCode.Items[0].Selected = true;
                        //    ddlSInvDebitSLCode.Enabled = false;
                        //    ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = false;
                        //}
                        DropDownList ddlSInvDebitSLCode = new DropDownList();
                        ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                        ddlSInvDebitSLCode.Items.Clear();
                        ddlSInvDebitSLCode.Items.Add(dtSaleDebCredGLCode.Rows[0]["ENTITY_CODE"].ToString());
                        ddlSInvDebitSLCode.Items[0].Selected = true;
                        ddlSInvDebitSLCode.Enabled = false;
                        ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = false;
                        if (strMode == "Q")
                        {
                            Label LblSInvCrdGLCode = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvCrdGLCode");
                            LblSInvCrdGLCode.Text = ViewState["Cust_GL_Code"].ToString();
                        }
                        break;
                    }
                case "C":
                    {
                        DataSet dsSaleDebCredGLCode;
                        //ObjLegalMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();
                        byte[] byte_SaleDebCredGLCode = ObjLegalMgtServicesClnt.FunPubGetSaleDebCredGLCode(_CompanyID, AccCode, 1);
                        dsSaleDebCredGLCode = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_SaleDebCredGLCode, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));

                        DataTable dtSaleDebCredGLCode = dsSaleDebCredGLCode.Tables[0];
                        if (dtSaleDebCredGLCode.Rows.Count != 0)
                        {
                            Label LblSInvDebitGLCode = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvDebitGLCode");
                            Label LblSInvGlDesc = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvGlDesc");
                            LblSInvDebitGLCode.Text = dtSaleDebCredGLCode.Rows[0]["GL_Code"].ToString();
                            LblSInvGlDesc.Text = dtSaleDebCredGLCode.Rows[0]["Account_Code_Desc"].ToString();
                        }

                        //byte_SaleDebCredGLCode = ObjLegalMgtServicesClnt.FunPubGetSaleDebCredSLCode(_CompanyID, dtSaleDebCredGLCode.Rows[0]["GL_Code"].ToString(), Convert.ToInt32(ddlSInvLOB.SelectedValue), Convert.ToInt32(ddlSInvBranch.SelectedValue));
                        //dsSaleDebCredGLCode = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_SaleDebCredGLCode, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
                        //if (dsSaleDebCredGLCode.Tables[0].Rows.Count != 0)
                        //{
                        //    if (dsSaleDebCredGLCode.Tables[0].Rows.Count == 1)
                        //    {
                        //        //Filling Debit SL Code in Invoice Grid
                        //        DropDownList ddlSInvDebitSLCode = new DropDownList();
                        //        ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                        //        ddlSInvDebitSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                        //        ddlSInvDebitSLCode.Items[1].Selected = true;
                        //        ddlSInvDebitSLCode.Enabled = false;
                        //        ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = true;
                        //        //Filling Party SL Code in Receipt
                        //     }
                        //    else
                        //    {
                        //        DropDownList ddlSInvDebitSLCode = new DropDownList();
                        //        ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                        //        ddlSInvDebitSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                        //        ddlSInvDebitSLCode.Enabled = true;
                        //        ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = true;
                        //    }
                        //}
                        //else
                        //{
                        //    DropDownList ddlSInvDebitSLCode = new DropDownList();
                        //    ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                        //    ddlSInvDebitSLCode.Items[0].Selected = true;
                        //    ddlSInvDebitSLCode.Enabled = false;
                        //    ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = false;
                        //}
                        DropDownList ddlSInvDebitSLCode = new DropDownList();
                        ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                        ddlSInvDebitSLCode.Items.Clear();
                        ddlSInvDebitSLCode.Items.Add(dtSaleDebCredGLCode.Rows[0]["CUSTOMER_CODE"].ToString());
                        ddlSInvDebitSLCode.Items[0].Selected = true;
                        ddlSInvDebitSLCode.Enabled = false;
                        ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = false;
                        if (strMode == "Q")
                        {
                            Label LblSInvCrdGLCode = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvCrdGLCode");
                            LblSInvCrdGLCode.Text = ViewState["Cust_GL_Code"].ToString();
                        }
                        break;
                    }

            }
        }
        catch(Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            //if (ObjLegalMgtServicesClnt != null)
                ObjLegalMgtServicesClnt.Close();
            FillReceipts();
        }

     
    }

    public void Insert_Router()
    {
        ObjLegalMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {
            decimal InvoiceTotal = 0;
            decimal ReceiptTotal = 0;
            bool rowchecked = false;
            InvoiceTotal = Convert.ToDecimal(((TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal")).Text);
            ReceiptTotal = 0;
            if (ViewState["MAXBID"] != null)
            {
                decimal MaxBidAmnt = Convert.ToDecimal(ViewState["MAXBID"].ToString());
                if (InvoiceTotal < MaxBidAmnt)
                {
                    Utility.FunShowAlertMsg(this.Page,"The Invoice Amount should be greater than or equal to the Maximum Bid Amount!");
                    return;
                }

            }
          
            foreach (GridViewRow gvr in GrvReceiptDetails.Rows)
            {
                CheckBox ChkRcpt = (CheckBox)gvr.FindControl("ChkRcpt");
                if((ChkRcpt.Checked== true) && (gvr.Enabled==true))
                {
                    ReceiptTotal += Convert.ToDecimal(((TextBox)gvr.FindControl("TxtSInvTxnAmnt")).Text);
                    if (rowchecked != true)
                    {
                        rowchecked = true;
                    }
                }
            }
            if((rowchecked==true) &&( InvoiceTotal > ReceiptTotal))
            {
                Utility.FunShowAlertMsg(this.Page, "The Receipt amount should be equal to or greater than the Invoice amount!");
                return;
            }

            intSaleInvId = ViewState["SALEINVID"].ToString() == "" ? 0 : Convert.ToInt32(ViewState["SALEINVID"].ToString());
            objSaleInvDataTable = new LEGAL.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceDataTable();
            LEGAL.LegalRepossessionMgtServices.S3G_LR_SaleInvoiceRow ObjSaleInvRow;
            ObjSaleInvRow = objSaleInvDataTable.NewS3G_LR_SaleInvoiceRow();
            ObjSaleInvRow.Branch_ID = Convert.ToInt32(ddlSInvBranch.SelectedValue.ToString());
            ObjSaleInvRow.Company_ID=_CompanyID;
            ObjSaleInvRow.Created_By=_UserID;
            ObjSaleInvRow.Customer_ID=Convert.ToInt32(ViewState["CUSTOMERID"].ToString());
            ObjSaleInvRow.LOB_ID=Convert.ToInt32(ddlSInvLOB.SelectedValue.ToString());
            ObjSaleInvRow.LRN_No = ViewState["LRNNO"].ToString();
            ObjSaleInvRow.Modified_By=_UserID;
            ObjSaleInvRow.Reposssion_Docket_No=TxtSInvRepDocNo.Text.ToString().Split('-')[0].Trim();
            ObjSaleInvRow.Sale_Invoice_ID=intSaleInvId;
            ObjSaleInvRow.Sale_Notification_Asset_ID = Convert.ToInt32(ViewState["SALENOTIFASSTID"].ToString());
            ObjSaleInvRow.Sale_Notification_ID=Convert.ToInt32(ddlSInvSSNo.SelectedValue.ToString());
            ObjSaleInvRow.Sale_Notification_No = ddlSInvSSNo.SelectedItem.Text.ToString().Split('-')[0].Trim(); ;
            ObjSaleInvRow.SIE_Date=Utility.StringToDate(TxtSInvInvDate.Text.ToString());
            ObjSaleInvRow.SIE_NO = TxtSInvInvNo.Text.ToString().Trim();
            ObjSaleInvRow.Reposssion_Docket_id = Convert.ToInt32(ViewState["REPOSSDKTID"].ToString());
            strbBidDetails.Append("<Root>");
            if (GrvSInvoiceDetails.Rows.Count > 0)
            {
                foreach (GridViewRow GvrInvDtls in GrvSInvoiceDetails.Rows)
                {
                    DropDownList ddlSInvAccountType = (DropDownList)GvrInvDtls.FindControl("ddlSInvAccountType");
                    DropDownList ddlSInvAccountCode = (DropDownList)GvrInvDtls.FindControl("ddlSInvAccountCode");
                    Label LblSInvDebitGLCode = (Label)GvrInvDtls.FindControl("LblSInvDebitGLCode");
                    DropDownList ddlSInvDebitSLCode = (DropDownList)GvrInvDtls.FindControl("ddlSInvDebitSLCode");
                    Label LblSInvGlDesc = (Label)GvrInvDtls.FindControl("LblSInvGlDesc");
                    Label LblSInvCrdGLCode = (Label)GvrInvDtls.FindControl("LblSInvCrdGLCode");
                    DropDownList ddlSInvCrdSLCode = (DropDownList)GvrInvDtls.FindControl("ddlSInvCrdSLCode");
                    TextBox TxtSInvAmount = (TextBox)GvrInvDtls.FindControl("TxtSInvAmount");
                    strbBidDetails.Append(" <INVDETAILS Sale_Invoice_Sno = '1'");
                    strbBidDetails.Append(" Sale_Type='" + Convert.ToString(ddlSInvAccountType.SelectedValue.ToString()) + "'");
                    strbBidDetails.Append(" Sale_Account_Code = '" + Convert.ToString(ddlSInvAccountCode.SelectedValue.ToString()) + "'");
                    strbBidDetails.Append(" Debit_GL_Account = '" + Convert.ToString(LblSInvDebitGLCode.Text.ToString()) + "'");
                    if (ddlSInvDebitSLCode.SelectedItem.Text.ToString() == "")
                    {
                        strbBidDetails.Append(" Debit_SL_Account = '" + DBNull.Value + "'");
                    }
                    else
                    {
                        strbBidDetails.Append(" Debit_SL_Account = '" + Convert.ToString(ddlSInvDebitSLCode.SelectedItem.Text.ToString()) + "'");
                    }
                    strbBidDetails.Append(" Description = '" + Convert.ToString(LblSInvGlDesc.Text.ToString()) + "'");
                    if (rowchecked == true)
                    {
                        strbBidDetails.Append(" Credit_GL_Account ='" + Convert.ToString(ViewState["Cust_GL_Code"].ToString()) + "'");
                    }
                    else
                    {
                        strbBidDetails.Append(" Credit_GL_Account ='" + Convert.ToString(LblSInvCrdGLCode.Text.ToString()) + "'");
                    }
                    if (ddlSInvCrdSLCode.SelectedItem.Text.ToString() == "")
                    {
                        strbBidDetails.Append(" Credit_SL_Account = '" + DBNull.Value + "'");
                    }
                    else
                    {
                        strbBidDetails.Append(" Credit_SL_Account = '" + Convert.ToString(ddlSInvCrdSLCode.SelectedItem.Text.ToString()) + "'");
                    }
                    strbBidDetails.Append(" Amount ='" + decimal.Parse(TxtSInvAmount.Text.ToString())+ "'/>");
                 }
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invoice details cannot be empty");
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                return;
            }
            strbBidDetails.Append("</Root>");
           
            
            strbRcptDetails.Append("<Root>");
            if (GrvReceiptDetails.Rows.Count > 0)
            {
                foreach (GridViewRow GvrRcptDtls in GrvReceiptDetails.Rows)
                {
                    CheckBox ChkRcpt = (CheckBox)GvrRcptDtls.FindControl("ChkRcpt");
                    TextBox TxtRcptRefNo = (TextBox)GvrRcptDtls.FindControl("TxtRcptRefNo");
                    TextBox TxtSInvTxnDate = (TextBox)GvrRcptDtls.FindControl("TxtSInvTxnDate");
                    TextBox TxtSInvTxnAmnt = (TextBox)GvrRcptDtls.FindControl("TxtSInvTxnAmnt");
                    Label LblRcptID = (Label)GvrRcptDtls.FindControl("LblRcptID");
                    if ((ChkRcpt.Checked == true) && (GvrRcptDtls.Enabled==true))
                    {
                        strbRcptDetails.Append(" <RCPTDETAILS Sale_Invoice_Rcpt_SrNo = '" + Convert.ToString(GvrRcptDtls.RowIndex + 1) + "'");
                        strbRcptDetails.Append(" Receipt_ID='" + Convert.ToString(LblRcptID.Text.ToString()) + "'");
                        strbRcptDetails.Append(" Receipt_No = '" + Convert.ToString(TxtRcptRefNo.Text.ToString()) + "'");
                        strbRcptDetails.Append(" Trxn_Date = '" + Utility.StringToDate(TxtSInvTxnDate.Text.ToString()) + "'");
                        strbRcptDetails.Append(" Trxn_Amount = '" + Convert.ToString(TxtSInvTxnAmnt.Text.ToString()) + "'/>");

                    }

                }
            }
           
            strbRcptDetails.Append("</Root>");
            ObjSaleInvRow.XMLInvoiceDetails = strbBidDetails.ToString();
            ObjSaleInvRow.XMLReceiptDetails = strbRcptDetails.ToString();
            ObjSaleInvRow.Is_Active = ChkSInvIsActive.Checked ? true : false;
            objSaleInvDataTable.AddS3G_LR_SaleInvoiceRow(ObjSaleInvRow);

            if (objSaleInvDataTable.Rows.Count > 0)
            {
               
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_ORG_SaleInv_DataTable = ClsPubSerialize.Serialize(objSaleInvDataTable, SerMode);

                if (intSaleInvId > 0)
                {
                    intErrorCode = ObjLegalMgtServicesClnt.FunPubUpdateSalesInvoiceEntry(SerMode, byteobjS3G_ORG_SaleInv_DataTable);
                }
                else
                {
                    intErrorCode = ObjLegalMgtServicesClnt.FunPubInsertSalesInvoiceEntry(out strDSNNo, SerMode, byteobjS3G_ORG_SaleInv_DataTable);
                }

                switch (intErrorCode)
                {
                    case 0:
                        TxtSInvInvNo.Text = strDSNNo;
                        if (intSaleInvId > 0)
                        {
                            //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = strAlert.Replace("__ALERT__", "Sales Invoice details updated sucessfully");
                        }
                        else
                        {
                            //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = "Sales Invoice Number " + strDSNNo + " has been added successfully";
                            strAlert += @"\n\nWould you like to add one more Sale Invoice Entry?";
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
                        if (intSaleInvId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Sale Invoice details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Sale Invoice details");
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
            //if (ObjLegalMgtServicesClnt != null)
                ObjLegalMgtServicesClnt.Close();

        }
    }
    public string ChkNull(string val)
    {
        return val == "" ? "0" : val;
    }
   #region Dropdown Filling
    //private void PopulateBranchList()
    //{
    //    try
    //    {
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", Convert.ToString(_CompanyID));
    //        Procparam.Add("@Is_Active", "1");
    //        Procparam.Add("@User_ID", _UserID.ToString());
    //        Procparam.Add("@Program_ID", "156");
    //        Procparam.Add("@Lob_Id", Convert.ToString(ddlSInvLOB.SelectedValue));
    //        //ddlSInvBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
    //        //if (ddlSInvBranch.Items.Count == 2)
    //        //{
    //        //    ddlSInvBranch.SelectedIndex = 1;
    //            PopulateSaleNotificationNumber();
    //            ddlSInvSSNo.Enabled = true;
    //            ddlSInvSSNo.Items[0].Selected = true;
    //        //}
    //        //else
    //        //{
    //            ddlSInvSSNo.ClearSelection();
    //            ddlSInvBranch.Enabled = true;
    //            ddlSInvSSNo.Enabled = false;
    //            TxtSInvMLANo.Clear();
    //            TxtSInvRefNo.Clear();
    //            TxtSInvRepDocNo.Clear();
    //            TxtSInvSLANo.Clear();
    //            UcSInvCustomerDetails.ClearCustomerDetails();
    //            GRVAssetDetails.ClearGrid();
    //            GrvBidDetails.ClearGrid();
    //            GrvSInvoiceDetails.Enabled = false;
    //            GrvReceiptDetails.ClearGrid();
    //            GrvReceiptDetails.Enabled = false;
    //        //}

    //    }
    //    catch (FaultException<LegalAndRepossessionMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        throw objFaultExp;
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        throw ex;
    //    }
    //}
    #endregion

    #region user Defined Functions
    private void PopulateSaleNotificationNumber()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@LOB_ID", ddlSInvLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlSInvBranch.SelectedValue);
            Procparam.Add("@Company_ID", _CompanyID.ToString());
            Procparam.Add("@User_ID", _UserID.ToString());
            ddlSInvSSNo.BindDataTable(SPNames.S3G_LR_GetSaleBidNotificationID, Procparam, new string[] { "ID", "SNNO" });

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
    private void PopulateLOBList()
    {
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("OPTION", "7", Convert.ToString(_CompanyID));
            ////Procparam.Add("@Consitution_Id", txtConstitution.Attributes["Const_ID"]);
            //Procparam.Add("@User_ID", Convert.ToString(_UserID));
            //Procparam.Add("@Is_Active", "1");
            //ddlSInvLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(_CompanyID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@FilterOption", "'HP','TL','TE','FT','WC'");
            Procparam.Add("@User_ID", _UserID.ToString());
            Procparam.Add("@Program_ID", "156");
            ddlSInvLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddlSInvLOB.Items.Count == 2)
            {
                ddlSInvBranch.Enabled = true;
                ddlSInvLOB.SelectedIndex = 1;
                ddlSInvLOB_SelectIndexChanged(this, new EventArgs());
            }
            else
            {
                ddlSInvLOB.SelectedIndex = 0;

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
    private void FunPriGetSaleNotificationDetails(int CompanyID, int SnID)
    {
        ObjLegalMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

        try
        {
            int SaleBidID = 0;
            DataSet dsSalesBidDetails;
           
            byte[] byte_BidDetails = ObjLegalMgtServicesClnt.FunPubGetSaleBidNotificationDetails(out SaleBidID, _CompanyID, Convert.ToInt32(ddlSInvBranch.SelectedValue), Convert.ToInt32(ddlSInvLOB.SelectedValue), SnID);
            dsSalesBidDetails = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_BidDetails, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
            if (dsSalesBidDetails.Tables.Count > 0 && _CompanyID > 0)
            {

                ViewState["CUSTOMERID"] = dsSalesBidDetails.Tables[0].Rows[0]["Customer_ID"].ToString();
                TxtSInvRepDocNo.Text = Convert.ToString(dsSalesBidDetails.Tables[0].Rows[0]["RepossNo"]);
                TxtSInvMLANo.Text = Convert.ToString(dsSalesBidDetails.Tables[0].Rows[0]["PANUM"]); ;
                if (Convert.ToString(dsSalesBidDetails.Tables[0].Rows[0]["SANUM"]).Contains("DUMMY"))
                    TxtSInvSLANo.Text = string.Empty;
                else
                    TxtSInvSLANo.Text = Convert.ToString(dsSalesBidDetails.Tables[0].Rows[0]["SANUM"]);

                string strCustomerAddress = SetAddress(dsSalesBidDetails.Tables[0].Rows[0]["Comm_Address1"].ToString(), dsSalesBidDetails.Tables[0].Rows[0]["Comm_Address2"].ToString(), dsSalesBidDetails.Tables[0].Rows[0]["Comm_City"].ToString(), dsSalesBidDetails.Tables[0].Rows[0]["Comm_State"].ToString(), dsSalesBidDetails.Tables[0].Rows[0]["Comm_Country"].ToString(), dsSalesBidDetails.Tables[0].Rows[0]["Comm_Pincode"].ToString());

                //UcSInvCustomerDetails.SetCustomerDetails(dsSalesBidDetails.Tables[0].Rows[0]["Customer_Code"].ToString(), strCustomerAddress, dsSalesBidDetails.Tables[0].Rows[0]["Customer_Name"].ToString(), dsSalesBidDetails.Tables[0].Rows[0]["Comm_Mobile"].ToString(), dsSalesBidDetails.Tables[0].Rows[0]["Comm_EMail"].ToString(), dsSalesBidDetails.Tables[0].Rows[0]["Comm_Website"].ToString());
                UcSInvCustomerDetails.SetCustomerDetails(Convert.ToInt32(ViewState["CUSTOMERID"].ToString()),true);
            }

            if (dsSalesBidDetails.Tables[1].Rows.Count >= 1)
            {
                ViewState["SALENOTIFASSTID"] = dsSalesBidDetails.Tables[1].Rows[0]["Sale_Notification_Asset_ID"].ToString();
                ViewState["LRNNO"] = dsSalesBidDetails.Tables[1].Rows[0]["LRN_No"].ToString();
                TxtSInvRefNo.Text = dsSalesBidDetails.Tables[1].Rows[0]["Regno"].ToString();
                PNLAssetDetails.Visible = true;
                GRVAssetDetails.DataSource = dsSalesBidDetails.Tables[1];
                GRVAssetDetails.DataBind();
            }
            else if (dsSalesBidDetails.Tables[1].Rows.Count == 0)
            {
                PNLAssetDetails.Visible = false;
                GRVAssetDetails.DataSource = null;
                GRVAssetDetails.DataBind();
              
            }
            ViewState["REPOSSDKTID"] = dsSalesBidDetails.Tables[1].Rows[0]["Repossession_ID"].ToString();

            if (dsSalesBidDetails.Tables[2].Rows.Count != 0)
            {
                
                GrvBidDetails.DataSource = dsSalesBidDetails.Tables[2];
                GrvBidDetails.DataBind();
            }
            else
            {
                GrvBidDetails.EmptyDataText = "No Records Found!";
                GrvBidDetails.DataBind();
            }
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (((DataTable)ViewState["INVRECEIPTDETAILS"]!=null)&&(((DataTable)ViewState["INVRECEIPTDETAILS"]).Rows.Count != 0))
            {
                ViewState["Status"] = -1;
            }

            if (((strMode != "Q") && (strMode != "M")) && Convert.ToInt32(ViewState["Status"].ToString()) != -1)
            {
                Utility.FunShowAlertMsg(this.Page, "The highest bidder " + dsSalesBidDetails.Tables[6].Rows[0]["BIDDER_NAME"].ToString() + " needs to be an Entity or a Customer to proceed with the Sale Invoice Entry");
            }
            if (Convert.ToInt32(ViewState["Status"].ToString()) == -1)
            {
            if (dsSalesBidDetails.Tables[3].Rows.Count != 0)
            {
                ViewState["Cust_GL_Code"] = dsSalesBidDetails.Tables[3].Rows[0]["GL_CODE"].ToString();
                Label LblSInvCrdGLCode = (Label)GrvSInvoiceDetails.Rows[0].FindControl("LblSInvCrdGLCode");
                LblSInvCrdGLCode.Text = dsSalesBidDetails.Tables[3].Rows[0]["GL_CODE"].ToString();
                Label LblCrdDescription = (Label)GrvSInvoiceDetails.Rows[0].FindControl("LblCrdDescription");
                LblCrdDescription.Text = dsSalesBidDetails.Tables[4].Rows[0]["CRDT_Account_Tab_Desc"].ToString();

                byte[] byte_SaleDebCredGLCode = ObjLegalMgtServicesClnt.FunPubGetSaleDebCredSLCode(_CompanyID, dsSalesBidDetails.Tables[3].Rows[0]["GL_CODE"].ToString(), Convert.ToInt32(ddlSInvLOB.SelectedValue), Convert.ToInt32(ddlSInvBranch.SelectedValue));
                DataSet dsSaleDebCredGLCode = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_SaleDebCredGLCode, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
               

                if (dsSaleDebCredGLCode.Tables[0].Rows.Count != 0)
                {
                    LblCrdDescription = (Label)GrvSInvoiceDetails.Rows[0].FindControl("LblCrdDescription");
                    LblCrdDescription.Text = dsSaleDebCredGLCode.Tables[0].Rows[0]["Account_Tab_Desc"].ToString();
                    if (dsSaleDebCredGLCode.Tables[0].Rows.Count == 1)
                    {
                        DropDownList ddlSInvCrdSLCode = new DropDownList();
                        ddlSInvCrdSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvCrdSLCode");
                        ddlSInvCrdSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                        ddlSInvCrdSLCode.Items[1].Selected = true;
                        //ddlSInvCrdSLCode.Enabled = true;
                        ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[0].FindControl("rfvddlSInvCrdSLCode")).Enabled = true;
                    }
                    else
                    {
                        DropDownList ddlSInvCrdSLCode = new DropDownList();
                        ddlSInvCrdSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvCrdSLCode");
                        ddlSInvCrdSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                        ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[0].FindControl("rfvddlSInvCrdSLCode")).Enabled = true;
                        //Label LblSInvPrtySLCode = (Label)GrvReceiptDetails.Rows[0].FindControl("LblSInvPrtySLCode");
                        //LblSInvPrtySLCode.Text = dsSaleDebCredGLCode.Tables[0].Rows[0]["SL_Code"].ToString();
                    }
                }
                else
                {
                    DropDownList ddlSInvCrdSLCode = new DropDownList();
                    ddlSInvCrdSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvCrdSLCode");
                    ddlSInvCrdSLCode.Items.Clear();
                    ddlSInvCrdSLCode.Items.Add(dsSalesBidDetails.Tables[0].Rows[0]["Customer_Code"].ToString());
                    ddlSInvCrdSLCode.Items[0].Selected = true;
                    ddlSInvCrdSLCode.Enabled = false;
                    ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[0].FindControl("rfvddlSInvCrdSLCode")).Enabled = false;
                    //Label LblSInvPrtySLCode = (Label)GrvReceiptDetails.Rows[0].FindControl("LblSInvPrtySLCode");
                    //LblSInvPrtySLCode.Text = "";

                }

            }
            else
            {
                Label LblSInvCrdGLCode = (Label)GrvSInvoiceDetails.Rows[0].FindControl("LblSInvCrdGLCode");
                LblSInvCrdGLCode.Text = "";
            }
            ViewState["MAXBID"] = dsSalesBidDetails.Tables[6].Rows[0]["MAX_BID_AMOUNT"].ToString();
            TextBox TxtSInvAmount = (TextBox)GrvSInvoiceDetails.Rows[0].FindControl("TxtSInvAmount");
            TxtSInvAmount.Text = dsSalesBidDetails.Tables[6].Rows[0]["MAX_BID_AMOUNT"].ToString();
            TxtSInvAmount.SetDecimalPrefixSuffix(8, 4, true, true, "Total Amount");
            TextBox TxtSInvTotal = (TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal");
            TxtSInvTotal.Text = dsSalesBidDetails.Tables[6].Rows[0]["MAX_BID_AMOUNT"].ToString();
            }
            else
            {
                ViewState["Cust_GL_Code"] = dsSalesBidDetails.Tables[3].Rows[0]["GL_CODE"].ToString();
                Label LblSInvCrdGLCode = (Label)GrvSInvoiceDetails.Rows[0].FindControl("LblSInvCrdGLCode");
                LblSInvCrdGLCode.Text = "";
                DropDownList ddlSInvCrdSLCode = new DropDownList();
                ddlSInvCrdSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvCrdSLCode");
                ddlSInvCrdSLCode.Items.Clear();
                ddlSInvCrdSLCode.Items.Add("--Select--");
                ddlSInvCrdSLCode.Items[0].Selected = true;
                ddlSInvCrdSLCode.Enabled = false;
                Label LblCrdDescription = (Label)GrvSInvoiceDetails.Rows[0].FindControl("LblCrdDescription");
                LblCrdDescription.Text = "";

                ViewState["MAXBID"] = dsSalesBidDetails.Tables[6].Rows[0]["MAX_BID_AMOUNT"].ToString();
                TextBox TxtSInvAmount = (TextBox)GrvSInvoiceDetails.Rows[0].FindControl("TxtSInvAmount");
                TxtSInvAmount.Text = dsSalesBidDetails.Tables[6].Rows[0]["MAX_BID_AMOUNT"].ToString();
                TxtSInvAmount.SetDecimalPrefixSuffix(8, 4, true, true, "Total Amount");
                TextBox TxtSInvTotal = (TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal");
                TxtSInvTotal.Text = dsSalesBidDetails.Tables[6].Rows[0]["MAX_BID_AMOUNT"].ToString();
            }
          
            if (strMode == "C")
            {
                FillReceipts();
            }

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
           // if (ObjLegalMgtServicesClnt != null)
                ObjLegalMgtServicesClnt.Close();
        }
    }
    //public void FillReceipts()
    //{
    //      try
    //        {
    //            DataSet DS= new DataSet();
    //            if (Procparam != null)
    //                Procparam.Clear();
    //            else
    //            Procparam = new Dictionary<string, string>();
    //            Procparam.Add("@Company_ID", Convert.ToString(_CompanyID));
    //            if (TxtSInvMLANo.Text.ToString().Trim() != string.Empty)
    //            {
    //                Procparam.Add("@PANUM", TxtSInvMLANo.Text.ToString().Trim());
    //            }
    //            if (TxtSInvSLANo.Text.ToString().Trim() != string.Empty)
    //            {
    //                Procparam.Add("@SANUM", TxtSInvSLANo.Text.ToString().Trim());
    //            }
    //           Procparam.Add("@Due_Flag", "81");
               
    //           DS= Utility.GetDataset(SPNames.S3G_LR_GetReceiptDetailsonPASANum, Procparam);
    //           ViewState["INVRECEIPTDETAILS"] = DS.Tables[1];
    //           if (DS.Tables[0].Rows.Count != 0)
    //           {
    //               GrvReceiptDetails.Enabled = true;
    //               GrvReceiptDetails.DataSource = DS.Tables[0];
    //               GrvReceiptDetails.DataBind();
                   
    //           }
    //           else
    //           {
    //               GrvReceiptDetails.EmptyDataText = "No Records Found!";
    //               GrvReceiptDetails.Enabled = false;
    //               GrvReceiptDetails.DataBind();
    //           }
    //        }
    //        catch (FaultException<LegalAndRepossessionMgtServicesReference.ClsPubFaultException> objFaultExp)
    //        {
    //            throw objFaultExp;
    //        }
    //        catch (Exception ex)
    //        {
    //              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //            throw ex;
    //        }
    //}

    public void FillReceipts()
    {
        try
        {
            DropDownList ddlSInvAccountCode = new DropDownList();
            if (GrvSInvoiceDetails.Rows.Count != 0)
            {
                ddlSInvAccountCode = ((DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvAccountCode"));
            }
            DataSet DS = new DataSet();
            if (Procparam != null)
            {
                Procparam.Clear();
            }
            if (ddlSInvAccountCode.SelectedValue != "0")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(_CompanyID));
                Procparam.Add("@Due_Flag", "81");
                Procparam.Add("@Entity_ID", ddlSInvAccountCode.SelectedValue.ToString());
                DS = Utility.GetDataset(SPNames.S3G_LR_GetReceiptDetailsonPASANum, Procparam);
                if (DS.Tables[1].Rows.Count > 0)
                {
                    ViewState["INVRECEIPTDETAILS"] = DS.Tables[1];
                }
                if (DS.Tables[0].Rows.Count != 0)
                {
                    ViewState["RECEIPTS"] = DS.Tables[0].Rows.Count;
                    GrvReceiptDetails.Enabled = true;
                    GrvReceiptDetails.DataSource = DS.Tables[0];
                    GrvReceiptDetails.DataBind();

                }
                else
                {
                    GrvReceiptDetails.EmptyDataText = "No Records Found!";
                    GrvReceiptDetails.Enabled = false;
                    GrvReceiptDetails.DataBind();
                }
            }

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

     private void FunPriSaleInvControlStatus(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                ddlSInvBranch.Enabled = true;
                ddlSInvSSNo.Enabled = false;
                btnClear.Enabled = true;
                ChkSInvIsActive.Enabled = false;
                ChkSInvIsActive.Checked = true;
                GrvReceiptDetails.Enabled = false;
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                break;

            case 1: //Modify

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                ChkSInvIsActive.Enabled = true;
                ddlSInvBranch.Enabled = false;
                ddlSInvLOB.Enabled = false;
                ddlSInvSSNo.Enabled = false;
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
                ChkSInvIsActive.Enabled = false;
                ddlSInvBranch.Enabled = false;
                ddlSInvSSNo.Enabled = false;
                ddlSInvLOB.Enabled = false;
                GRVAssetDetails.Enabled = false;
                GrvBidDetails.Enabled = false;
                GrvSInvoiceDetails.Enabled =false;
                GrvReceiptDetails.Enabled =false;
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

    protected void ChkRcpt_CheckedChanged(object sender, EventArgs e)
    {
        decimal total = 0;
        TextBox TxtSInvTxnTotal = (TextBox)GrvReceiptDetails.FooterRow.FindControl("TxtSInvTxnTotal");
       foreach(GridViewRow gvr in GrvReceiptDetails.Rows)
       {  
           CheckBox ChkRcpt=(CheckBox)gvr.FindControl("ChkRcpt");
           TextBox TxtRcptRefNo = (TextBox)gvr.FindControl("TxtRcptRefNo");
           TextBox TxtSInvTxnDate = (TextBox)gvr.FindControl("TxtSInvTxnDate");
           TextBox TxtSInvTxnAmnt = (TextBox)gvr.FindControl("TxtSInvTxnAmnt");
      
           if ((ChkRcpt.Checked == true) &&(gvr.Enabled==true))
           {
               total += Convert.ToDecimal(TxtSInvTxnAmnt.Text.ToString().Trim());
           }
         
       }
       TxtSInvTxnTotal.Text = Convert.ToString(total);
    }
}
