/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// /// <Program Summary>
/// Last Updated By		      :   Thalaiselvam N
/// Last Updated Date         :   03-Sep-2011
/// Reason                    :   Encrypted Password Validation
/// <Program Summary>
/// 
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
using System.Text;
using System.Collections.Generic;
using S3GBusEntity;
using LEGALSERVICES=LegalAndRepossessionMgtServicesReference ;
using System.ServiceModel;

public partial class Collateral_S3GCLTCollateralSale : ApplyThemeForProject
{
    #region [Common Variable declaration]
    int intCompanyID,intSaleNo, intUserID = 0;
    UserInfo ObjUserInfo = new UserInfo();
    Dictionary<string, string> Procparam = null;
    string strRedirectPage = "~/Collateral/S3GCLTTransLander.aspx?Code=KSA";
    string strRedirectPageView = "window.location.href='../Collateral/S3GCLTTransLander.aspx?Code=KSA';";
    string strRedirectPageAdd = "window.location.href='../Collateral/S3GCLTCollateralSale.aspx';";
    string strRedirectPageViewTrans = "../Collateral/S3GCLTTransLander.aspx?Code=KSA";
    string strAlert = "alert('__ALERT__');";
    string strKey = "Insert";
    S3GSession ObjS3GSession = new S3GSession();
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    string strCLT_CaptureID = "0";
    HiddenField hdnCID;
    StringBuilder strColSaleDetails = new StringBuilder();
    StringBuilder strColSaleInvoiceDetails = new StringBuilder();
    StringBuilder strColSaleReceiptDetails = new StringBuilder();
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable objCollateralSaleDataTable;
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralSaleRow objCollateralSaleDataRow;
    CollateralMgtServicesReference.CollateralMgtServicesClient objCollateralMgtClient;
    LEGALSERVICES.LegalAndRepossessionMgtServicesClient ObjLegalMgtServicesClnt;
    int intNoofSearch = 2;
    string[] arrSortCol = new string[] { "Customer_Name", "Collateral_Tran_No" };
    string strProcName = "";
   
    ArrayList arrSearchVal = new ArrayList(1);
    PagingValues ObjPaging = new PagingValues();
    
    //Sale Approval
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ApprovalDataTable objDataTable = null;
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
    #endregion

    #region [PageLoad Event]
    protected void Page_Load(object sender, EventArgs e)
    {
        FunPageLoad();
    }
    #endregion

    #region [ User Defined Method]
    private void FunPriBindGrid()
    {
        try
        {
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            FunPriGetSearchValue();


            Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Workflow_Sequence_ID", "0");
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            // Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@Option", "1");
            if (rbtCustomer.Checked == true)
                Procparam.Add("@Param", "1");
            if (rbtAgent.Checked == true)
                Procparam.Add("@Param", "2");
            //Procparam.Add("@CurrentPage", ProPageNumRW.ToString ());
            //Procparam.Add("@PageSize", ProPageNumRW.ToString());
            //Procparam.Add("@SearchValue", hdnSearch.Value );
            //Procparam.Add("@OrderBy", hdnOrderBy.Value );


            grvPaging.BindGridView("S3G_CLT_GetCollSaleCustomerAccDetails", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvPaging.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }

    }

    #region Paging and Searching Methods For Grid

    protected void Move(object sender, EventArgs e)
    {
        /// TextBox txt = (TextBox)sender;
        CheckBox chk = (CheckBox)sender;
        GridViewRow grvData = (GridViewRow)chk.Parent.Parent;

        HiddenField hidCLT_ID = (HiddenField)grvData.FindControl("hidCLT_ID");
        HiddenField hidCA_ID = (HiddenField)grvData.FindControl("hidCA_ID");

        ChkCOM.Checked = false;
        ChkFS.Checked = false;
        ChkHS.Checked = false;
        ChkLS.Checked = false;
        ChkMS.Checked = false;

        //  HiddenField hidCLT_ID=grvPaging .Rows [e].FindControl ("hidCLT_ID") as HiddenField ;
        ViewState["COLLATERAL_CAPTURE_ID"] = hidCLT_ID.Value;
        Label lblCType_No = (Label)grvData.FindControl("lblCType_No");
        ViewState["COLTRAN_NO"] = lblCType_No.Text;
        TxtColTranNo.Text = lblCType_No.Text;
        tcCollateralSale.ActiveTabIndex = 1;
        SetTabIndexChanged();
        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];
        if (strMode == "C")
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COLLATERAL_CAPTURE_ID", hidCA_ID.Value.ToString());
            Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
            DataSet DS = Utility.GetDataset("S3G_CLT_ChkReceiptExists", Procparam);
            if (DS.Tables[0].Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, " A Collateral Sale is already under process for this Collateral Transaction Number. Modify the Collateral data in Modify mode to proceed!");
                return;
            }
            else if ((DS.Tables[0].Rows[0]["RECEIPT_NO"].ToString() == "FIRST RECORD") || (DS.Tables[0].Rows.Count > 0))
            {
                ChkCOM.Visible = true;
                ChkFS.Visible = true;
                ChkHS.Visible = true;
                ChkLS.Visible = true;
                ChkMS.Visible = true;
                ChkCOM_CheckedChanged(this, new EventArgs());
                ChkFS_CheckedChanged(this, new EventArgs());
                ChkHS_CheckedChanged(this, new EventArgs());
                ChkLS_CheckedChanged(this, new EventArgs());
                ChkMS_CheckedChanged(this, new EventArgs());
                //FunCollateralDetails(intCompanyID, Convert.ToInt32(ViewState["CUST_ID"].ToString()), Convert.ToInt32(ddlColTranNo.SelectedValue));
            }
        }
        else
        {
            ChkCOM.Visible = true;
            ChkFS.Visible = true;
            ChkHS.Visible = true;
            ChkLS.Visible = true;
            ChkMS.Visible = true;
            ChkCOM_CheckedChanged(this, new EventArgs());
            ChkFS_CheckedChanged(this, new EventArgs());
            ChkHS_CheckedChanged(this, new EventArgs());
            ChkLS_CheckedChanged(this, new EventArgs());
            ChkMS_CheckedChanged(this, new EventArgs());
            //FunCollateralDetails(intCompanyID, Convert.ToInt32(ViewState["CUST_ID"].ToString()), Convert.ToInt32(ddlColTranNo.SelectedValue));
        }
        if (rbtCustomer.Checked == true)
            PopulateCustomerDetails(hidCA_ID.Value.ToString(), 1);
        if (rbtAgent.Checked == true)
            PopulateCustomerDetails(hidCA_ID.Value.ToString(), 2);
        tpColGeneral.Visible = true;
        tpColDtl.Visible = true;
        if (Request.QueryString["qsMode"] != null)
           strMode = Request.QueryString["qsMode"];
        if (strMode == "C")
        {
            if (tcCollateralSale.ActiveTab == tpColCustAgnt)
            {
                btnClear.Enabled = false;
            }
            else
            {
                btnClear.Enabled = true;
            }
        }
;

    }
    void SetTabIndexChanged()
    {
        if (Request.QueryString["qsMode"] == "M" || Request.QueryString["qsMode"] == "Q")
        {
            tpColGeneral.Enabled = tpColDtl.Enabled = tpColSaleApp.Enabled = tpRevoke.Enabled = tpSaleInvoice.Enabled = true;
            tpColCustAgnt.Enabled = false;
        }
        else
        {

            if (tcCollateralSale.ActiveTab == tpColCustAgnt)
            {
                tpColGeneral.Enabled = tpColDtl.Enabled = tpColSaleApp.Enabled = tpRevoke.Enabled = tpSaleInvoice.Enabled = false;

            }
            else
            {
                tpColGeneral.Enabled = tpColDtl.Enabled = tpColSaleApp.Enabled = tpRevoke.Enabled = tpSaleInvoice.Enabled = true;
                tpColCustAgnt.Enabled = false;

            }

        }
    }

    private void FunPriGetSearchValue()
    {
        arrSearchVal = grvPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        grvPaging.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        grvPaging.FunPriSetSearchValue(arrSearchVal);
    }

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {

        if (rbtCustomer.Checked == true)
            arrSortCol[0] = "Customer_Name";
        if (rbtAgent.Checked == true)
            arrSortCol[0] = "UTPA_Name";

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '" + arrSearchVal[iCount].ToString() + "%'";
                }
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvPaging.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private string FunPriGetSortDirectionStr(string strColumn)
    {
        string strSortDirection = string.Empty;
        string strSortExpression = string.Empty;
        // By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;
        strSortDirection = "DESC";
        try
        {
            // Retrieve the last strColumn that was sorted.
            // Check if the same strColumn is being sorted.
            // Otherwise, the default value can be returned.
            strSortExpression = hdnSortExpression.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
            // Save new values in hidden control.
            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strColumn;
            strOrderBy = " " + strColumn + " " + strSortDirection;
            hdnOrderBy.Value = strOrderBy;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
        return strSortDirection;
    }

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        if (rbtCustomer.Checked == true)
            arrSortCol[0] = "Customer_Name";
        if (rbtAgent.Checked == true)
            arrSortCol[0] = "UTPA_Name";
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (lnkbtnSearch.ID == "lnkbtnSort" + (iCount + 1).ToString())
                {
                    strSortColName = arrSortCol[iCount].ToString();
                    break;
                }
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();
            arrSortCol[0] = "";
            if (strDirection == "ASC")
            {
                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    #endregion
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
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw new Exception("Unable to Intialize data");
        }
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
    public void FunPubBindInvoiceDetails()
    {
        //FunProIntializeInvoiceGV();
        //FunProIntializeReceiptGV();
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@CUST_ID", ViewState["CUST_ID"].ToString());
        Procparam.Add("@LOB_ID", ViewState["LOB_ID"].ToString());
        if (ViewState["Location_ID"] != null)
        {
            Procparam.Add("@Location_ID", ViewState["Location_ID"].ToString());
        }
        Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
        DataSet dsSalesBidDetails = Utility.GetDataset("S3G_CLT_GetGLCodeOnCustId", Procparam);
        if (dsSalesBidDetails.Tables[0].Rows.Count != 0)
        {
            ViewState["Cust_GL_Code"] = dsSalesBidDetails.Tables[0].Rows[0]["GL_CODE"].ToString();
            Label LblSInvCrdGLCode = (Label)GrvSInvoiceDetails.Rows[0].FindControl("LblSInvCrdGLCode");
            LblSInvCrdGLCode.Text = dsSalesBidDetails.Tables[0].Rows[0]["GL_CODE"].ToString();

            //Procparam= new Dictionary<string,string>();
            //Procparam.Add("@GL_CODE",ViewState["Cust_GL_Code"].ToString());
            //Procparam.Add("@Company_ID",intCompanyID.ToString());
            //DataSet dsSaleDebCredGLCode = Utility.GetDataset("S3G_LR_GetSLCodeOnGLCode", Procparam);
            if (dsSalesBidDetails.Tables[1].Rows.Count != 0)
            {
                if (dsSalesBidDetails.Tables[1].Rows.Count == 1)
                {
                    DropDownList ddlSInvCrdSLCode = new DropDownList();
                    ddlSInvCrdSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvCrdSLCode");
                    ddlSInvCrdSLCode.BindDataTable(dsSalesBidDetails.Tables[1]);
                    ddlSInvCrdSLCode.Items[1].Selected = true;
                    ddlSInvCrdSLCode.Enabled = true;
                    //((RequiredFieldValidator)GrvSInvoiceDetails.Rows[0].FindControl("rfvddlSInvCrdSLCode")).Enabled = true;
                }
                else
                {
                    DropDownList ddlSInvCrdSLCode = new DropDownList();
                    ddlSInvCrdSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvCrdSLCode");
                    ddlSInvCrdSLCode.BindDataTable(dsSalesBidDetails.Tables[1]);
                    //((RequiredFieldValidator)GrvSInvoiceDetails.Rows[0].FindControl("rfvddlSInvCrdSLCode")).Enabled = true;
                    //Label LblSInvPrtySLCode = (Label)GrvReceiptDetails.Rows[0].FindControl("LblSInvPrtySLCode");
                    //LblSInvPrtySLCode.Text = dsSaleDebCredGLCode.Tables[0].Rows[0]["SL_Code"].ToString();
                }
            }
            else
            {
                DropDownList ddlSInvCrdSLCode = new DropDownList();
                ddlSInvCrdSLCode = (DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvCrdSLCode");
                ddlSInvCrdSLCode.Items[0].Selected = true;
                ddlSInvCrdSLCode.Enabled = false;
                //((RequiredFieldValidator)GrvSInvoiceDetails.Rows[0].FindControl("rfvddlSInvCrdSLCode")).Enabled = false;
                //Label LblSInvPrtySLCode = (Label)GrvReceiptDetails.Rows[0].FindControl("LblSInvPrtySLCode");
                //LblSInvPrtySLCode.Text = "";

            }

        }
        else
        {
            Label LblSInvCrdGLCode = (Label)GrvSInvoiceDetails.Rows[0].FindControl("LblSInvCrdGLCode");
            LblSInvCrdGLCode.Text = "";
        }
        TextBox TxtSInvAmount = (TextBox)GrvSInvoiceDetails.Rows[0].FindControl("TxtSInvAmount");
        TxtSInvAmount.Text = ViewState["TOTAL_AMOUNT"].ToString();
        TextBox TxtSInvTotal = (TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal");
        TxtSInvTotal.Text = ViewState["TOTAL_AMOUNT"].ToString();
        FillReceipts();
  
    }
    public void FunPubGetReceipts(int intSaleNo)
    {
        DataSet DS;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Collaterasl_Sale_Id", intSaleNo.ToString());

        DS = Utility.GetDataset("S3G_CLT_GetReceipt_Capture", Procparam);
        if (DS.Tables[0].Rows.Count > 0)
        {
            ViewState["INVRECEIPTDETAILS"] = DS.Tables[0];
        }
    }
    private void FunPubProGetSaleInvDetails(int _CompanyID, int intColSaleId)
    {
        try
        {
            DataSet dsSalesInvDetails;
            Procparam= new Dictionary<string,string>();
            Procparam.Add("@Company_ID",intCompanyID.ToString());
            Procparam.Add("@Collateral_Sale_ID",intColSaleId.ToString());
            dsSalesInvDetails = Utility.GetDataset("S3G_CLT_GetCollateralSaleInvoice", Procparam);
            if (dsSalesInvDetails.Tables.Count > 0 && _CompanyID > 0)
            {

                DataTable dtInvDetails = dsSalesInvDetails.Tables[0];
                if (dtInvDetails.Rows.Count > 0)
                {
                    ViewState["DetailsTable"] = dtInvDetails;
                    GrvSInvoiceDetails.DataSource = dtInvDetails;
                    GrvSInvoiceDetails.DataBind();
                    TextBox TxtSInvAmount = (TextBox)GrvSInvoiceDetails.Rows[0].FindControl("TxtSInvAmount");
                    TxtSInvAmount.Text = ViewState["TOTAL_AMOUNT"].ToString();
                    ((TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal")).Text = ViewState["TOTAL_AMOUNT"].ToString();
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

                    ddlSInvDebitSLCode.ClearSelection();
                    ddlSInvDebitSLCode.Items.FindByText(dtInvDetails.Rows[0]["Debit_SL_Account"].ToString()).Selected = true;
                    //if (dtInvDetails.Rows[0]["Debit_SL_Account"].ToString() == "" || dtInvDetails.Rows[0]["Debit_SL_Account"].ToString() == "0")
                    //{
                    //    ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[0].FindControl("rfvddlSInvDebitSLCode")).Enabled = false;
                    //}
                    //ddlSInvCrdSLCode.ClearSelection();
                    //ddlSInvCrdSLCode.Items.FindByText(dtInvDetails.Rows[0]["Credit_SL_Account"].ToString()).Selected = true;
                }
                DataTable dtRcptdetails = dsSalesInvDetails.Tables[1];
                if (dtRcptdetails.Rows.Count > 0)
                {
                    ViewState["INVRECEIPTDETAILS"] = dtRcptdetails;
                    GrvReceiptDetails.DataSource = dtRcptdetails;
                    GrvReceiptDetails.DataBind();

                    //if (strMode == "M")
                    //{
                    //    Utility.FunShowAlertMsg(Page, "As receipt(s) exists for this invoice, you would not be able to modify the contents. Hence, showing in Query mode!");
                    //    FunPriColSaleControlStatus(-1);

                    //}
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
                    if (strMode == "M")
                    {
                        FillReceipts();
                    }
                    if (strMode == "Q")
                    {
                        GrvReceiptDetails.EmptyDataText = "No Records Found!";
                        GrvReceiptDetails.DataBind();

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Invoice details");
        }
        

    }
    public void FunPubProInsertCollateralSale(StringBuilder strColSaleDetails, StringBuilder strColSaleInvoiceDetails, StringBuilder strColSaleReceiptDetails)
    {
        objCollateralMgtClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {
            string ColSaleNo;
            int ErrorCode = 0;
            objCollateralSaleDataTable = new S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable();
            objCollateralSaleDataRow = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralSaleRow)objCollateralSaleDataTable.NewRow();
            objCollateralSaleDataRow.Collateral_Capture_ID = Convert.ToInt32(ViewState["COLLATERAL_CAPTURE_ID"].ToString());
            objCollateralSaleDataRow.Collateral_Sale_Date = Utility.StringToDate(txtCollateralSaleDate.Text.ToString());
            objCollateralSaleDataRow.Collateral_Tran_No = ViewState["COLTRAN_NO"].ToString();
            objCollateralSaleDataRow.Company_ID = intCompanyID;
            objCollateralSaleDataRow.Created_By = intUserID;
            objCollateralSaleDataRow.Created_On = DateTime.Now;
            objCollateralSaleDataRow.XMLSALEDETAILS = strColSaleDetails.ToString();
            objCollateralSaleDataRow.XMLINVDETAILS = strColSaleInvoiceDetails.ToString();
            objCollateralSaleDataRow.XMLRECEIPTDETAILS = strColSaleReceiptDetails.ToString();
            objCollateralSaleDataTable.Rows.Add(objCollateralSaleDataRow);
            if (objCollateralSaleDataTable.Rows.Count > 0)
            {
              
                ErrorCode = objCollateralMgtClient.FunPubInsertCollateralSale(out ColSaleNo, ClsPubSerialize.Serialize(objCollateralSaleDataTable, SerializationMode.Binary), SerializationMode.Binary);
                switch (ErrorCode)
                {
                    case 0:
                        txtCollateralSaleNo.Text = ColSaleNo;

                        if (intSaleNo > 0)
                        {
                            //Added by Bhuvana  on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = strAlert.Replace("__ALERT__", "Collateral Sales detail have been updated sucessfully");
                        }
                        else
                        {
                            //Added by Bhuvana  on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = "Collateral Sale Number " + ColSaleNo + " added successfully";
                            strAlert += @"\n\nWould you like to add one more Collateral Sale detail?";
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
                        if (intSaleNo > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Collateral Sale details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Collateral Sale details");
                        }
                        strRedirectPageView = "";
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
        finally
        {
            //if (objCollateralMgtClient ==null)
            {
                objCollateralMgtClient.Close();
            }
        }
    }
    public void FunPubProUpdateCollateralSale(StringBuilder strColSaleDetails,StringBuilder strColSaleInvoiceDetails,StringBuilder strColSaleReceiptDetails)
    {
        objCollateralMgtClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {
            
            //if (tpSaleInvoice.Visible == true)
            //{
            //    decimal InvoiceTotal = 0;
            //    decimal ReceiptTotal = 0;
            //    bool rowchecked = false;
            //    InvoiceTotal = Convert.ToDecimal(((TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal")).Text);
            //    ReceiptTotal = 0;
            //    foreach (GridViewRow gvr in GrvReceiptDetails.Rows)
            //    {
            //        CheckBox ChkRcpt = (CheckBox)gvr.FindControl("ChkRcpt");
            //        if ((ChkRcpt.Checked == true) && (gvr.Enabled == true))
            //        {
            //            ReceiptTotal += Convert.ToDecimal(((TextBox)gvr.FindControl("TxtSInvTxnAmnt")).Text);
            //            if (rowchecked != true)
            //            {
            //                rowchecked = true;
            //            }
            //        }
            //    }
            //    if ((rowchecked == true) && (InvoiceTotal > ReceiptTotal))
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "The Receipt amount should be equal to or greater than the Invoice amount!");
            //        return;
            //    }
            //}
            intSaleNo = Convert.ToInt32(ViewState["COLLATERAL_SALE_ID"]);
            string ColSaleNo;
            int ErrorCode = 0;
            objCollateralSaleDataTable = new S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable();
            objCollateralSaleDataRow = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralSaleRow)objCollateralSaleDataTable.NewRow();
            objCollateralSaleDataRow.Collateral_Capture_ID = Convert.ToInt32(ViewState["COLLATERAL_CAPTURE_ID"].ToString());
            objCollateralSaleDataRow.Collateral_Sale_Date = Utility.StringToDate(txtCollateralSaleDate.Text.ToString());
            objCollateralSaleDataRow.Collateral_Tran_No = ViewState["COLTRAN_NO"].ToString();  
            objCollateralSaleDataRow.Company_ID = intCompanyID;
            objCollateralSaleDataRow.Created_By = intUserID;
            objCollateralSaleDataRow.Modified_By = intUserID;
            objCollateralSaleDataRow.Created_On = DateTime.Now;
            objCollateralSaleDataRow.XMLSALEDETAILS = strColSaleDetails.ToString();
            objCollateralSaleDataTable.Rows.Add(objCollateralSaleDataRow);
            if (objCollateralSaleDataTable.Rows.Count > 0)
            {
               
                ErrorCode = objCollateralMgtClient.FunPubUpdateCollateralSale(ClsPubSerialize.Serialize(objCollateralSaleDataTable, SerializationMode.Binary), SerializationMode.Binary, Convert.ToInt32(ViewState["COLLATERAL_SALE_ID"]));
                switch (ErrorCode)
                {
                    case 0:
                        if (intSaleNo > 0)
                        {
                            //Added by Bhuvana  on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = strAlert.Replace("__ALERT__", "Collateral Sales detail have been updated sucessfully");
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
                        if (intSaleNo > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Collateral Sale details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Collateral Sale details");
                        }
                        strRedirectPageView = "";
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
        finally
        {
            //if (objCollateralMgtClient.State != null)
            //{
                objCollateralMgtClient.Close();
            //}
        }
    }

    private void PopulateCustomerDetails(string custID, int optCA)
    {
        bool bCLT_Available = false;
        if (ViewState["COLLATERAL_CAPTURE_ID"] != null)
            strCLT_CaptureID = Convert.ToString(ViewState["COLLATERAL_CAPTURE_ID"]);

        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@CA_ID", custID);
        Procparam.Add("@Collateral_Capture_ID", strCLT_CaptureID);
        Procparam.Add("@Option", optCA.ToString());

        DataSet dsCustAndAccDetails = Utility.GetDataset("S3G_CLT_GetCollSaleCustomerDetails", Procparam);

        if (dsCustAndAccDetails.Tables[0].Rows.Count > 0)
        {
            if (optCA == 1)
            {
                pnlHeader.Visible = true;
                S3GCustomerAddress1.SetCustomerDetails(Convert.ToInt32(custID), true);
                pnlAgent.Visible = false;
            }
            else if (optCA == 2)
            {
                pnlAgent.Visible = true;
                txtACode.Text = dsCustAndAccDetails.Tables[0].Rows[0]["UTPA_Code"].ToString();
                txtAName.Text = dsCustAndAccDetails.Tables[0].Rows[0]["UTPA_Name"].ToString();
                txtAaddress.Text = dsCustAndAccDetails.Tables[0].Rows[0]["Address"].ToString();
                txtAcity.Text = dsCustAndAccDetails.Tables[0].Rows[0]["City"].ToString();
                txtAState.Text = dsCustAndAccDetails.Tables[0].Rows[0]["State"].ToString();
                txtACountry.Text = dsCustAndAccDetails.Tables[0].Rows[0]["Country"].ToString();
                pnlHeader.Visible = false;
            }

        }
        FunCollateralDetails(intCompanyID,Convert.ToInt32(custID),Convert.ToInt32(strCLT_CaptureID));
    }   
        
    public void FunCollateralDetails(int CompanyID,int CustomerID,int Capture_ID)
    {
         if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];
         if (strMode == "M" || strMode == "Q")
         {
             FunPriBindApprovalGrid();
             FunPubGetReceipts(intSaleNo);
             if (ViewState["APPROVAL"] != null)//If there are approval based details for the Collateral sale
             {
                 DataTable DTSet = (DataTable)ViewState["APPROVAL"];
                 DataRow[] DRSet = DTSet.Select("Approval_ID=" + ViewState["APPROVAL_ID"].ToString(), "Approval_ID");
                 switch (DRSet[0]["Status_Code"].ToString())
                 {
                     case "3"://Approved

                         if (Convert.ToInt32(DRSet[0]["Created_by"].ToString()) == intUserID)
                         {
                             if (ViewState["INVRECEIPTDETAILS"] != null)//Receipt details have been filled and finalised.
                             {
                                 Utility.FunShowAlertMsg(this.Page, "The Collateral Sale has a receipt attached to it. Hence, showing in Query mode!!");
                                 //FunProIntializeData();
                                 //FunProIntializeInvoiceGV();
                                 //FunProIntializeReceiptGV();
                                 //FunPubBindInvoiceDetails();
                                 ViewState["STATUS"] = -1;
                                 //FunPriColSaleControlStatus(-1);
                                 //tpRevoke.Visible = false;
                                 //return;
                             }
                             else
                             {
                                 //FunPriColSaleControlStatus(-3);
                                 ViewState["STATUS"] = -3;
                                 //return;
                             }
                         }
                         if (ViewState["INVRECEIPTDETAILS"] != null)//Receipt details have been filled and finalised.
                         {
                             //Utility.FunShowAlertMsg(this.Page, "The Collateral Sale has a receipt attached to it. Hence, showing in Query mode!!");
                             //FunProIntializeData();
                             //FunProIntializeInvoiceGV();
                             //FunProIntializeReceiptGV();
                             //FunPubBindInvoiceDetails();
                             //FunPriColSaleControlStatus(-1);
                             ViewState["STATUS"] = -1;
                             //tpRevoke.Visible = false;
                             //return;
                         }
                         else if (ViewState["INVRECEIPTDETAILS"] == null)//There are no receipt details for the Collateral Sale.
                         {
                             //Utility.FunShowAlertMsg(this.Page, "The Collateral Sale has been approved. Hence, you would not be able to make any changes to the Collateral details.Proceeding to Sale Invoice!");
                             //FunProIntializeData();
                             //FunProIntializeInvoiceGV();
                             //FunProIntializeReceiptGV();
                             //FunPubBindInvoiceDetails();
                             //FunPriColSaleControlStatus(-2);
                             ViewState["STATUS"] = -2;
                             //FunPubProGetSaleInvDetails(intCompanyID, intSaleNo);
                             //return;
                         }
                         break;
                     case "4"://Rejected
                         //Utility.FunShowAlertMsg(this.Page, "The Collateral Sale has been rejected. Hence, showing in Query mode!!");
                         ViewState["STATUS"] = -1;
                         //FunPriColSaleControlStatus(-1);
                         break;
                     case "5"://Revoked
                         ViewState["STATUS"] = 1;
                         //FunPriColSaleControlStatus(1);
                         break;

                 }
             }
             else //If there are no approval based details for the Collateral sale
             {
                 ViewState["STATUS"] = 1;
                 //FunPriColSaleControlStatus(1);
             }
          Procparam = new Dictionary<string, string>();
             Procparam.Add("@COMPANY_ID", CompanyID.ToString());
             Procparam.Add("@CUSTOMER_ID", CustomerID.ToString());
             Procparam.Add("@CAPTURE_ID", Capture_ID.ToString());
             DataSet DsCollateralDetails = new DataSet();
             if ((ViewState["STATUS"] != null)&& Convert.ToInt32(ViewState["STATUS"].ToString()) == -1)
             {
                 DsCollateralDetails = Utility.GetDataset("S3G_CLT_GetCollateralItemsRefNumbersReceipted", Procparam);
             }
             else
             {
                 DsCollateralDetails = Utility.GetDataset("S3G_CLT_GetCollateralItemsRefMod", Procparam);
             }

             DataTable DTHSDetails = DsCollateralDetails.Tables[0];
             DataTable DTMSDetails = DsCollateralDetails.Tables[1];
             DataTable DTLSDetails = DsCollateralDetails.Tables[2];
             DataTable DTFSDetails = DsCollateralDetails.Tables[3];
             DataTable DTCOMDetails = DsCollateralDetails.Tables[4];

             //Binding High Security Collateral Details

             if (DTHSDetails.Rows.Count != 0)
             {
                 ViewState["HS_ROW_COUNT"] = DTHSDetails.Rows.Count;
                 ChkHS.Enabled = true;
                 gvHighLiqDetails.Visible = true;
                 gvHighLiqDetails.DataSource = DTHSDetails;
                 gvHighLiqDetails.DataBind();

             }
             else
             {
                 ChkHS.Visible = false;
                 ChkHS.Checked = false;
                 gvHighLiqDetails.EmptyDataText = "No Records Found!";
                 gvHighLiqDetails.Visible = false;
                 gvHighLiqDetails.DataSource = null;
                 gvHighLiqDetails.DataBind();
             }

             //Binding Medium Security Collateral Details

             if (DTMSDetails.Rows.Count != 0)
             {
                 ViewState["MS_ROW_COUNT"] = DTMSDetails.Rows.Count;
                 ChkMS.Enabled = true;
                 gvMedLiqDetails.Visible = true;
                 gvMedLiqDetails.DataSource = DTMSDetails;
                 gvMedLiqDetails.DataBind();

             }
             else
             {
                 ChkMS.Visible = false;
                 ChkMS.Checked = false;
                 gvMedLiqDetails.Visible = false;
                 gvMedLiqDetails.EmptyDataText = "No Records Found!";
                 gvMedLiqDetails.DataSource = null;
                 gvMedLiqDetails.DataBind();
             }

             // Binding Low Security Collateral Details

             if (DTLSDetails.Rows.Count != 0)
             {
                 ViewState["LS_ROW_COUNT"] = DTLSDetails.Rows.Count;
                 ChkLS.Enabled = true;
                 gvLowLiqDetails.Visible = true;
                 gvLowLiqDetails.DataSource = DTLSDetails;
                 gvLowLiqDetails.DataBind();

             }
             else
             {
                 ChkLS.Visible = false;
                 ChkLS.Checked = false;
                 gvLowLiqDetails.EmptyDataText = "No Records Found!";
                 gvLowLiqDetails.Visible = false;
                 gvLowLiqDetails.DataSource = null;
                 gvLowLiqDetails.DataBind();
             }

             // Binding Financial Securities Collateral Details

             if (DTFSDetails.Rows.Count != 0)
             {
                 ViewState["FS_ROW_COUNT"] = DTFSDetails.Rows.Count;
                 ChkFS.Enabled = true;
                 gvFinDetails.Visible = true;
                 gvFinDetails.DataSource = DTFSDetails;
                 gvFinDetails.DataBind();

             }
             else
             {
                 ChkFS.Visible = false;
                 ChkFS.Checked = false;
                 gvFinDetails.EmptyDataText = "No Records Found!";
                 gvFinDetails.Visible = false;
                 gvFinDetails.DataSource = null;
                 gvFinDetails.DataBind();
             }

             // Binding Commodities Securities Collateral Details

             if (DTCOMDetails.Rows.Count != 0)
             {
                 ViewState["COM_ROW_COUNT"] = DTCOMDetails.Rows.Count;
                 ChkCOM.Enabled = true;
                 gvCommoDetails.Visible = true;
                 gvCommoDetails.DataSource = DTCOMDetails;
                 gvCommoDetails.DataBind();

             }
             else
             {
                 ChkCOM.Visible = false;
                 ChkCOM.Checked = false;
                 gvCommoDetails.EmptyDataText = "No Records Found!";
                 gvCommoDetails.Visible = false;
                 gvCommoDetails.DataSource = null;
                 gvCommoDetails.DataBind();
             }

             if ((DTHSDetails.Rows.Count + DTMSDetails.Rows.Count + DTLSDetails.Rows.Count + DTFSDetails.Rows.Count + DTCOMDetails.Rows.Count) == 0)
             {
                 strAlert = strAlert.Replace("__ALERT__", "All the Collaterals for this Customer have been exhausted. Hence, you would  not be able to proceed with the Sale! ");
                 ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                 ViewState["EXHAUST"] = true;
             }      
         }
         else
         {
             Procparam = new Dictionary<string, string>();
             Procparam.Add("@COMPANY_ID", CompanyID.ToString());
             Procparam.Add("@CUSTOMER_ID", CustomerID.ToString());
             Procparam.Add("@CAPTURE_ID", Capture_ID.ToString());
             DataSet DsCollateralDetails = new DataSet();
             if ((ViewState["STATUS"] != null) && (Convert.ToInt32(ViewState["STATUS"].ToString()) == -1))
             {
                    DsCollateralDetails = Utility.GetDataset("S3G_CLT_GetCollateralItemsRefNumbersReceipted", Procparam);
             }
             else
             {
                 DsCollateralDetails = Utility.GetDataset(SPNames.S3G_CLT_GetCollateralItemsRefNumbers, Procparam);
             }

             DataTable DTHSDetails = DsCollateralDetails.Tables[0];
             DataTable DTMSDetails = DsCollateralDetails.Tables[1];
             DataTable DTLSDetails = DsCollateralDetails.Tables[2];
             DataTable DTFSDetails = DsCollateralDetails.Tables[3];
             DataTable DTCOMDetails = DsCollateralDetails.Tables[4];

             //Binding High Security Collateral Details

             if (DTHSDetails.Rows.Count != 0)
             {
                 ViewState["HS_ROW_COUNT"] = DTHSDetails.Rows.Count;
                 ChkHS.Enabled = true;
                 gvHighLiqDetails.Visible = true;
                 gvHighLiqDetails.DataSource = DTHSDetails;
                 gvHighLiqDetails.DataBind();

             }
             else
             {
                 ChkHS.Visible = false;
                 ChkHS.Checked = false;
                 gvHighLiqDetails.EmptyDataText = "No Records Found!";
                 gvHighLiqDetails.Visible = false;
                 gvHighLiqDetails.DataSource = null;
                 gvHighLiqDetails.DataBind();
             }

             //Binding Medium Security Collateral Details

             if (DTMSDetails.Rows.Count != 0)
             {
                 ViewState["MS_ROW_COUNT"] = DTMSDetails.Rows.Count;
                 ChkMS.Enabled = true;
                 gvMedLiqDetails.Visible = true;
                 gvMedLiqDetails.DataSource = DTMSDetails;
                 gvMedLiqDetails.DataBind();

             }
             else
             {
                 ChkMS.Visible = false;
                 ChkMS.Checked = false;
                 gvMedLiqDetails.Visible = false;
                 gvMedLiqDetails.EmptyDataText = "No Records Found!";
                 gvMedLiqDetails.DataSource = null;
                 gvMedLiqDetails.DataBind();
             }

             // Binding Low Security Collateral Details

             if (DTLSDetails.Rows.Count != 0)
             {
                 ViewState["LS_ROW_COUNT"] = DTLSDetails.Rows.Count;
                 ChkLS.Enabled = true;
                 gvLowLiqDetails.Visible = true;
                 gvLowLiqDetails.DataSource = DTLSDetails;
                 gvLowLiqDetails.DataBind();

             }
             else
             {
                 ChkLS.Visible = false;
                 ChkLS.Checked = false;
                 gvLowLiqDetails.EmptyDataText = "No Records Found!";
                 gvLowLiqDetails.Visible = false;
                 gvLowLiqDetails.DataSource = null;
                 gvLowLiqDetails.DataBind();
             }

             // Binding Financial Securities Collateral Details

             if (DTFSDetails.Rows.Count != 0)
             {
                 ViewState["FS_ROW_COUNT"] = DTFSDetails.Rows.Count;
                 ChkFS.Enabled = true;
                 gvFinDetails.Visible = true;
                 gvFinDetails.DataSource = DTFSDetails;
                 gvFinDetails.DataBind();

             }
             else
             {
                 ChkFS.Visible = false;
                 ChkFS.Checked = false;
                 gvFinDetails.EmptyDataText = "No Records Found!";
                 gvFinDetails.Visible = false;
                 gvFinDetails.DataSource = null;
                 gvFinDetails.DataBind();
             }

             // Binding Commodities Securities Collateral Details

             if (DTCOMDetails.Rows.Count != 0)
             {
                 ViewState["COM_ROW_COUNT"] = DTCOMDetails.Rows.Count;
                 ChkCOM.Enabled = true;
                 gvCommoDetails.Visible = true;
                 gvCommoDetails.DataSource = DTCOMDetails;
                 gvCommoDetails.DataBind();

             }
             else
             {
                 ChkCOM.Visible = false;
                 ChkCOM.Checked = false;
                 gvCommoDetails.EmptyDataText = "No Records Found!";
                 gvCommoDetails.Visible = false;
                 gvCommoDetails.DataSource = null;
                 gvCommoDetails.DataBind();
             }

             if ((DTHSDetails.Rows.Count + DTMSDetails.Rows.Count + DTLSDetails.Rows.Count + DTFSDetails.Rows.Count + DTCOMDetails.Rows.Count) == 0)
             {
                 strAlert = strAlert.Replace("__ALERT__", "All the Collaterals for this Customer have been exhausted or there are no units to sell. Hence, you would  not be able to proceed with the Sale! ");
                 ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                 ViewState["EXHAUST"] = true;
             }
         }
    }

    protected void GrvSInvoiceDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //TextBox TxtSInvAmount = ((TextBox)e.Row.FindControl("TxtSInvAmount"));
            //TxtSInvAmount.Attributes.Add("onblur", "javascript:return GenGrandTotal('" + TxtSInvAmount.ClientID + "')");
        }

    }
    protected void GrvReceiptDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((TextBox)e.Row.FindControl("TxtRcptRefNo")).Text.ToString().Trim() != string.Empty)
            {
                if (ViewState["INVRECEIPTDETAILS"] != null)
                {
                    DataTable dtReceipt = (DataTable)ViewState["INVRECEIPTDETAILS"];
                    if (dtReceipt.Rows.Count > 0)
                    {
                        DataView dvReceipt = new DataView(dtReceipt);
                        dvReceipt.Sort = "Receipt_No";
                        dvReceipt.RowFilter = "Receipt_No='" + ((TextBox)e.Row.FindControl("TxtRcptRefNo")).Text.ToString().Trim() + "'";
                        if (dvReceipt.ToTable().Rows.Count > 0)
                        {
                            e.Row.Enabled = false;
                            ((CheckBox)e.Row.FindControl("ChkRcpt")).Checked = true;
                        }
                        else
                        {
                            e.Row.Enabled = true;
                        }
                    }
                }
            }
        }
    }
  private void FunGridValidation(GridView grvColateralValidation, bool Flag)
    {

        string strColValue;

        foreach (GridViewRow grvRow in grvColateralValidation.Rows)
        {
            int intcolcount = 0;
            for (intcolcount = 0; intcolcount <= grvRow.Cells.Count - 1; intcolcount++)
            {
               strColValue= grvRow.Cells[intcolcount].Text;
               if (strColValue == "")
               {

                   if (grvRow.Cells[intcolcount].Controls.Count > 0)
                   {

                       if (grvRow.Cells[intcolcount].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.Textbox")
                       {
                           strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[0]).Text;
                           if (strColValue == "0")
                           {
                               Utility.FunShowAlertMsg(this, "Enter a Amount and Point");
                               return;
                           }

                       }

                       if (grvRow.Cells[intcolcount].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                       {
                           strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[0]).Checked == true ? "1" : "0";
                           if (strColValue == "0")
                           {
                               Utility.FunShowAlertMsg(this,"Selecet Atleast a Colateral Record");
                               return;
                           }

                       }
                   }

               }
            }
        }
        
       }
  protected void ddlSInvAccountType_SelectedIndexChanged(object sender, EventArgs e)
  {
    

          GridViewRow GrvRow = (GridViewRow)((DropDownList)sender).NamingContainer;
          int RowIndex = GrvRow.RowIndex;

          DropDownList ddlSInvAccountType;
          DropDownList ddlSInvAccountCode;
          Label LblSInvDebitGLCode;
          DropDownList ddlSInvDebitSLCode;
          Label LblSInvGlDesc;
          Label LblSInvCrdGLCode;
          DropDownList ddlSInvCrdSLCode;
          //TextBox TxtSInvAmount;
          //TextBox TxtSInvTotal;
          ddlSInvAccountType = (DropDownList)GrvRow.FindControl("ddlSInvAccountType");
          ddlSInvAccountCode = (DropDownList)GrvRow.FindControl("ddlSInvAccountCode");
          ddlSInvAccountCode.Items.Clear();
          ddlSInvAccountCode.Items.Add("--Select--");
          ddlSInvAccountCode.ClearSelection();

          ddlSInvDebitSLCode = (DropDownList)GrvRow.FindControl("ddlSInvDebitSLCode");
          ddlSInvDebitSLCode.Items.Clear();
          ddlSInvDebitSLCode.Items.Add("--Select--");
          ddlSInvDebitSLCode.Enabled = false;
          ddlSInvDebitSLCode.ClearSelection();

          ddlSInvCrdSLCode = (DropDownList)GrvRow.FindControl("ddlSInvCrdSLCode");
          ddlSInvCrdSLCode.Items.Clear();
          ddlSInvCrdSLCode.Items.Add("--Select--");
          ddlSInvCrdSLCode.Enabled = false;
          ddlSInvCrdSLCode.ClearSelection();

          LblSInvDebitGLCode = (Label)GrvRow.FindControl("LblSInvDebitGLCode");
          LblSInvDebitGLCode.Text = "";
          LblSInvGlDesc = (Label)GrvRow.FindControl("LblSInvGlDesc");
          LblSInvGlDesc.Text = "";
          LblSInvCrdGLCode = (Label)GrvRow.FindControl("LblSInvCrdGLCode");
          LblSInvCrdGLCode.Text = "";

          //TxtSInvAmount = (TextBox)GrvRow.FindControl("TxtSInvAmount");
          //TxtSInvAmount.Clear();
          //TxtSInvTotal = (TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal");
          //TxtSInvTotal.Clear();
      
          if (ddlSInvAccountType.SelectedValue == "--Select--")
          {
              GrvReceiptDetails.ClearGrid();
          }
          else
          {
              switch (((DropDownList)sender).SelectedValue)
              {
                  case "E":
                      {
                          GrvReceiptDetails.ClearGrid();
                          GrvSInvoiceDetails.Columns[1].Visible = true;
                          //GrvSInvoiceDetails.Columns[1].HeaderText = "Entity Code";
                          Procparam = new Dictionary<string, string>();
                          Procparam.Add("@Company_ID", intCompanyID.ToString());
                          //Procparam.Add("@Branch_ID", ViewState["BRANCH_ID"].ToString());
                          //Procparam.Add("@LOB_ID", ViewState["LOB_ID"].ToString());
                          ((DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvAccountCode")).BindDataTable(SPNames.S3G_LR_GetEntity, Procparam, new string[] { "Entity_ID", "Entity" });
                          break;
                      }
                  case "C":
                      {
                          GrvReceiptDetails.ClearGrid();
                          GrvSInvoiceDetails.Columns[1].Visible = true;
                          //GrvSInvoiceDetails.Columns[1].HeaderText = "Customer Code";
                          Procparam = new Dictionary<string, string>();
                          Procparam.Add("@Company_ID", intCompanyID.ToString());
                          //Procparam.Add("@Branch_ID", ViewState["BRANCH_ID"].ToString());
                          //Procparam.Add("@LOB_ID", ViewState["LOB_ID"].ToString());
                          Procparam.Add("@Cust_id", ViewState["CUST_ID"].ToString());
                          ((DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvAccountCode")).BindDataTable(SPNames.S3G_LR_GetCustomer, Procparam, new string[] { "Customer_ID", "Customer_Code" });
                          break;

                      }

              }
          }
     
      if (Request.QueryString["qsMode"] != null)
          strMode = Request.QueryString["qsMode"];
      if (strMode == "M")
      {
          FunPubBindInvoiceDetails();
          if (((DropDownList)sender).SelectedIndex > 0)
          {
              tpRevoke.Enabled = false;
              btnSave.Enabled = true;
          }
          else
          {
              tpRevoke.Enabled = true;
              btnSave.Enabled = false;
          }
      }
  }
  protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
  {
      if (Request.QueryString["qsMode"] != null)
          strMode = Request.QueryString["qsMode"];
      if (strMode == "M")
      {
          if (ddlStatus.SelectedIndex > 0)
          {
             tpColDtl.Enabled = false;
             btnSave.Enabled = true;
          }
          else
          {
              tpColDtl.Enabled = true;
              btnSave.Enabled = false;
          }
      }
  }
  protected void GetCust(object sender, EventArgs e)
  {
      rbtAgent.Checked = false;
      arrSortCol[0] = hdnSearch.Value = "";
      TextBox txtSearch = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch1");
      txtSearch.Text = "";
      TextBox txtSearch2 = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch2");
      txtSearch2.Text = "";
      //FunProHeaderSearch(txtSearch, e);

      LinkButton lnkSort = (LinkButton)grvPaging.HeaderRow.FindControl("lnkbtnSort1");
      FunProSortingColumn(lnkSort, e);


      FunPriBindGrid();

  }

  protected void GetAgent(object sender, EventArgs e)
  {
      rbtCustomer.Checked = false;
      arrSortCol[0] = hdnSearch.Value = "";
      TextBox txtSearch = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch1");
      txtSearch.Text = "";
      TextBox txtSearch2 = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch2");
      txtSearch2.Text = "";
      //FunProHeaderSearch(txtSearch, e);

      LinkButton lnkSort = (LinkButton)grvPaging.HeaderRow.FindControl("lnkbtnSort1");

      FunProSortingColumn(lnkSort, e);

      FunPriBindGrid();
  }
  protected void ddlSInvAccountCode_SelectedIndexChanged(object sender, EventArgs e)
  {
   
 
          ObjLegalMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();

          try
          {
              GridViewRow GrvRow = (GridViewRow)((DropDownList)sender).NamingContainer;
              int RowIndex = GrvRow.RowIndex;

              DropDownList ddlSInvAccountType;
              DropDownList ddlSInvAccountCode;
              Label LblSInvDebitGLCode;
              DropDownList ddlSInvDebitSLCode;
              Label LblSInvGlDesc;
              Label LblSInvCrdGLCode;
              DropDownList ddlSInvCrdSLCode;
              //TextBox TxtSInvAmount;
              //TextBox TxtSInvTotal;

              //ddlSInvAccountType = (DropDownList)GrvRow.FindControl("ddlSInvAccountType");
              //ddlSInvAccountCode = (DropDownList)GrvRow.FindControl("ddlSInvAccountCode");
              //ddlSInvAccountCode.ClearDropDownList();
              //ddlSInvAccountCode.ClearSelection();

              ddlSInvDebitSLCode = (DropDownList)GrvRow.FindControl("ddlSInvDebitSLCode");
              ddlSInvDebitSLCode.Items.Clear();
              ddlSInvDebitSLCode.Items.Add("--Select--");
              ddlSInvDebitSLCode.Enabled = false;
              ddlSInvDebitSLCode.ClearSelection();

              ddlSInvCrdSLCode = (DropDownList)GrvRow.FindControl("ddlSInvCrdSLCode");
              ddlSInvCrdSLCode.Items.Clear();
              ddlSInvCrdSLCode.Items.Add("--Select--");
              ddlSInvCrdSLCode.Enabled = false;
              ddlSInvCrdSLCode.ClearSelection();

              LblSInvDebitGLCode = (Label)GrvRow.FindControl("LblSInvDebitGLCode");
              LblSInvDebitGLCode.Text = "";
              LblSInvGlDesc = (Label)GrvRow.FindControl("LblSInvGlDesc");
              LblSInvGlDesc.Text = "";
              LblSInvCrdGLCode = (Label)GrvRow.FindControl("LblSInvCrdGLCode");
              LblSInvCrdGLCode.Text = "";

              string AccType = ((DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvAccountType")).SelectedValue;
              string AccCode = ((DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvAccountCode")).SelectedValue;

              switch (AccType)
              {
                  case "E":
                      {
                          DataSet dsSaleDebCredGLCode;

                          byte[] byte_SaleDebCredGLCode = ObjLegalMgtServicesClnt.FunPubGetSaleDebCredGLCode(intCompanyID, AccCode, 0);
                          dsSaleDebCredGLCode = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_SaleDebCredGLCode, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));

                          DataTable dtSaleDebCredGLCode = dsSaleDebCredGLCode.Tables[0];
                          if (dtSaleDebCredGLCode.Rows.Count != 0)
                          {
                              //Label LblSInvDebitGLCode = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvDebitGLCode");
                              //Label LblSInvGlDesc = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvGlDesc");
                              LblSInvDebitGLCode.Text = dtSaleDebCredGLCode.Rows[0]["GL_Code"].ToString();
                              LblSInvGlDesc.Text = dtSaleDebCredGLCode.Rows[0]["Account_Code_Desc"].ToString();
                          }
                          Procparam = new Dictionary<string, string>();
                          Procparam.Add("@Company_Id", intCompanyID.ToString());
                          Procparam.Add("@GL_CODE", dtSaleDebCredGLCode.Rows[0]["GL_Code"].ToString());
                          dsSaleDebCredGLCode = Utility.GetDataset(SPNames.S3G_LR_GetSLCodeOnGLCode, Procparam);
                          if (dsSaleDebCredGLCode.Tables[0].Rows.Count != 0)
                          {
                              if (dsSaleDebCredGLCode.Tables[0].Rows.Count == 1)
                              {
                                  //DropDownList ddlSInvDebitSLCode = new DropDownList();
                                  ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                                  ddlSInvDebitSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                                  ddlSInvDebitSLCode.Items[1].Selected = true;
                                  ddlSInvDebitSLCode.Enabled = true;
                                  ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = true;
                              }
                              else
                              {
                                  //DropDownList ddlSInvDebitSLCode = new DropDownList();
                                  ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                                  ddlSInvDebitSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                                  ddlSInvDebitSLCode.Enabled = true;
                                  ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = true;
                              }
                          }
                          else
                          {
                              //DropDownList ddlSInvDebitSLCode = new DropDownList();
                              ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                              ddlSInvDebitSLCode.Items[0].Selected = true;
                              ddlSInvDebitSLCode.Enabled = false;
                              ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = false;
                          }
                          break;
                      }
                  case "C":
                      {
                          DataSet dsSaleDebCredGLCode;
                          //ObjLegalMgtServicesClnt = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();
                          byte[] byte_SaleDebCredGLCode = ObjLegalMgtServicesClnt.FunPubGetSaleDebCredGLCode(intCompanyID, AccCode, 1);
                          dsSaleDebCredGLCode = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_SaleDebCredGLCode, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));

                          DataTable dtSaleDebCredGLCode = dsSaleDebCredGLCode.Tables[0];
                          if (dtSaleDebCredGLCode.Rows.Count != 0)
                          {
                              LblSInvDebitGLCode = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvDebitGLCode");
                              LblSInvGlDesc = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvGlDesc");
                              LblSInvDebitGLCode.Text = dtSaleDebCredGLCode.Rows[0]["GL_Code"].ToString();
                              LblSInvGlDesc.Text = dtSaleDebCredGLCode.Rows[0]["Account_Code_Desc"].ToString();
                          }

                          Procparam = new Dictionary<string, string>();
                          Procparam.Add("@Company_Id", intCompanyID.ToString());
                          Procparam.Add("@GL_CODE", dtSaleDebCredGLCode.Rows[0]["GL_Code"].ToString());
                          dsSaleDebCredGLCode = Utility.GetDataset(SPNames.S3G_LR_GetSLCodeOnGLCode, Procparam);

                          if (dsSaleDebCredGLCode.Tables[0].Rows.Count != 0)
                          {
                              if (dsSaleDebCredGLCode.Tables[0].Rows.Count == 1)
                              {
                                  //Filling Debit SL Code in Invoice Grid
                                  //DropDownList ddlSInvDebitSLCode = new DropDownList();
                                  ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                                  ddlSInvDebitSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                                  ddlSInvDebitSLCode.Items[1].Selected = true;
                                  ddlSInvDebitSLCode.Enabled = false;
                                  ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = true;
                                  //Filling Party SL Code in Receipt
                              }
                              else
                              {
                                  //DropDownList ddlSInvDebitSLCode = new DropDownList();
                                  ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                                  ddlSInvDebitSLCode.BindDataTable(dsSaleDebCredGLCode.Tables[0]);
                                  ddlSInvDebitSLCode.Enabled = true;
                                  ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = true;
                              }
                          }
                          else
                          {
                              //DropDownList ddlSInvDebitSLCode = new DropDownList();
                              ddlSInvDebitSLCode = (DropDownList)GrvSInvoiceDetails.Rows[RowIndex].FindControl("ddlSInvDebitSLCode");
                              ddlSInvDebitSLCode.Items[0].Selected = true;
                              ddlSInvDebitSLCode.Enabled = false;
                              ((RequiredFieldValidator)GrvSInvoiceDetails.Rows[RowIndex].FindControl("rfvddlSInvDebitSLCode")).Enabled = false;
                          }

                          //Label LblSInvCrdGLCode = (Label)GrvSInvoiceDetails.Rows[RowIndex].FindControl("LblSInvCrdGLCode");
                          LblSInvCrdGLCode.Text = ViewState["Cust_GL_Code"].ToString();
                          break;
                      }

              }

          }
          catch (Exception ex)
          {
              ClsPubCommErrorLog.CustomErrorRoutine(ex);
          }
          finally
          {
              //if (ObjLegalMgtServicesClnt != null)
              ObjLegalMgtServicesClnt.Close();
              FillReceipts();
          }

          if (Request.QueryString["qsMode"] != null)
              strMode = Request.QueryString["qsMode"];
          if (strMode == "M")
          {
              FunPubBindInvoiceDetails();
          }

  }

  protected void ChkRcpt_CheckedChanged(object sender, EventArgs e)
  {
      decimal total = 0;
      TextBox TxtSInvTxnTotal = (TextBox)GrvReceiptDetails.FooterRow.FindControl("TxtSInvTxnTotal");
      foreach (GridViewRow gvr in GrvReceiptDetails.Rows)
      {
          CheckBox ChkRcpt = (CheckBox)gvr.FindControl("ChkRcpt");
          TextBox TxtRcptRefNo = (TextBox)gvr.FindControl("TxtRcptRefNo");
          TextBox TxtSInvTxnDate = (TextBox)gvr.FindControl("TxtSInvTxnDate");
          TextBox TxtSInvTxnAmnt = (TextBox)gvr.FindControl("TxtSInvTxnAmnt");

          if ((ChkRcpt.Checked == true) && (gvr.Enabled == true))
          {
              total += Convert.ToDecimal(TxtSInvTxnAmnt.Text.ToString().Trim());
          } 

      }
      TxtSInvTxnTotal.Text = Convert.ToString(total);
      if (Request.QueryString["qsMode"] != null)
          strMode = Request.QueryString["qsMode"];
      if (strMode == "M")
      {
        btnSave.Enabled = true;
      }
  }
  public void FillReceipts()
  {
      try
      {
          DropDownList ddlSInvAccountCode =new DropDownList();
          if (GrvSInvoiceDetails.Rows.Count != 0)
          {
              ddlSInvAccountCode = ((DropDownList)GrvSInvoiceDetails.Rows[0].FindControl("ddlSInvAccountCode"));
          }
          DataSet DS = new DataSet();
          if (Procparam != null)
          {
              Procparam.Clear();
          }
             if (ddlSInvAccountCode.SelectedValue != "--Select--")
              {
                  Procparam = new Dictionary<string, string>();
                  Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                  Procparam.Add("@Due_Flag", "82");
                  Procparam.Add("@Entity_ID", ddlSInvAccountCode.SelectedValue.ToString());
                  DS = Utility.GetDataset("S3G_CLT_GetReceipt", Procparam);
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
          ClsPubCommErrorLog.CustomErrorRoutine(ex);
          throw ex;
      }
  }
    private void FunGridPanelClear()
    {
        
        //*** To clear Gridview Values and make Panels Visiable
        gvHighLiqDetails.DataSource = null;
        gvHighLiqDetails.DataBind();
        gvMedLiqDetails.DataSource = null;
        gvMedLiqDetails.DataBind();
        gvLowLiqDetails.DataSource = null;
        gvLowLiqDetails.DataBind();
        gvCommoDetails.DataSource = null;
        gvCommoDetails.DataBind();
        gvFinDetails.DataSource = null;
        gvFinDetails.DataBind();
        ChkCOM.Checked = false;
        ChkFS.Checked = false;
        ChkHS.Checked = false;
        ChkLS.Checked = false;
        ChkMS.Checked = false;
        
    }

    public static string SetCustomerAddress(string Address1, string Address2, string City, string State, string Country, string Pincode)
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
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    
    private void FunPageLoad()
    {

        try
        {
            ViewState["EXHAUST"] = false;
             txtCollateralSaleDate.Text = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW);
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

            #endregion

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            if (intCompanyID == 0)
            {
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            }
            if (intUserID == 0)
            {
                intUserID = ObjUserInfo.ProUserIdRW;
            }
            if (!IsPostBack)
            {
                if (PageMode == PageModes.Create)
                {
                    FunPriBindGrid();
                }
               if (Request.QueryString["qsMode"] != null)
                 strMode = Request.QueryString["qsMode"];
             if (Request.QueryString["qsViewId"] != string.Empty && Request.QueryString["qsViewId"] != null)
             {
                 FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                 intSaleNo = Convert.ToInt32(fromTicket.Name);
             }
             bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                
             if (intSaleNo > 0)
             {
                 bool blnTranExists;

                 FunPubProGetSaleNoDetails(intCompanyID, intSaleNo);
                 if (strMode == "M")
                 {
                     FunPriBindApprovalGrid();
                     FunPubGetReceipts(intSaleNo);
                     if (ViewState["APPROVAL"] != null)//If there are approval based details for the Collateral sale
                     {
                         DataTable DTSet = (DataTable)ViewState["APPROVAL"];
                         DataRow[] DRSet = DTSet.Select("Approval_ID=" + ViewState["APPROVAL_ID"].ToString(), "Approval_ID");
                         switch (DRSet[0]["Status_Code"].ToString())
                         {
                             case "3"://Approved
                                 
                                 if (Convert.ToInt32(DRSet[0]["Created_by"].ToString()) == intUserID)
                                 {
                                     if (ViewState["INVRECEIPTDETAILS"] != null)//Receipt details have been filled and finalised.
                                     {
                                         Utility.FunShowAlertMsg(this.Page, "The Collateral Sale has a receipt attached to it. Hence, showing in Query mode!!");
                                         FunProIntializeData();
                                         FunProIntializeInvoiceGV();
                                         FunProIntializeReceiptGV();
                                         FunPubBindInvoiceDetails();
                                         ViewState["STATUS"] =-1;
                                         FunPriColSaleControlStatus(-1);
                                         tpRevoke.Visible = false;
                                         return;
                                     }
                                     else
                                     {
                                         FunPriColSaleControlStatus(-3);
                                         ViewState["STATUS"] = -3;
                                         return;
                                     }
                                 }
                                 if (ViewState["INVRECEIPTDETAILS"] != null)//Receipt details have been filled and finalised.
                                 {
                                     Utility.FunShowAlertMsg(this.Page, "The Collateral Sale has a receipt attached to it. Hence, showing in Query mode!!");
                                     FunProIntializeData();
                                     FunProIntializeInvoiceGV();
                                     FunProIntializeReceiptGV();
                                     FunPubBindInvoiceDetails();
                                     FunPriColSaleControlStatus(-1);
                                     ViewState["STATUS"] = -1;
                                     tpRevoke.Visible = false;
                                     return;
                                 }
                                 else if (ViewState["INVRECEIPTDETAILS"] == null)//There are no receipt details for the Collateral Sale.
                                 {
                                     if (Convert.ToBoolean(ViewState["EXHAUST"].ToString()) == false)
                                     {
                                         Utility.FunShowAlertMsg(this.Page, "The Collateral Sale has been approved. Hence, you would not be able to make any changes to the Collateral details.Proceeding to Sale Invoice!");
                                     }
                                     FunProIntializeData();
                                     FunProIntializeInvoiceGV();
                                     FunProIntializeReceiptGV();
                                     FunPubBindInvoiceDetails();
                                     FunPriColSaleControlStatus(-2);
                                     ViewState["STATUS"] = -2;
                                     FunPubProGetSaleInvDetails(intCompanyID, intSaleNo);
                                     return;
                                 }
                                 break;
                             case "4"://Rejected
                                 Utility.FunShowAlertMsg(this.Page, "The Collateral Sale has been rejected. Hence, showing in Query mode!!");
                                 ViewState["STATUS"] = -1;
                                 FunPriColSaleControlStatus(-1);
                                 break;
                             case "5"://Revoked
                                 ViewState["STATUS"] = 1;
                                 FunPriColSaleControlStatus(1);
                                 break;

                         }
                     }
                     else //If there are no approval based details for the Collateral sale
                     {
                         ViewState["STATUS"] = 1;
                         FunPriColSaleControlStatus(1);
                     }
                 }
                 
                 if (strMode == "Q")
                 {
                     FunPriBindApprovalGrid();
                     FillReceipts();
                     ViewState["STATUS"] = -1;
                     FunPriColSaleControlStatus(-1);
                 }
             }
             else
             {
                 ViewState["STATUS"] = 0;
                 FunPriColSaleControlStatus(0);
             }
            }
          
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjUserInfo = null;

        }
    }
    private void FunPriBindStatus()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();

            Procparam = new Dictionary<string, string>();

            Procparam.Add("@Approval", "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LookupType_Code", "5");

            ddlStatus.BindDataTable("S3G_CLT_GetLookupTypeCode", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPubProGetSaleNoDetails(int CompanyID, int intSaleNo)
    {
        objCollateralMgtClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {

            DataSet DsColSaledetails;
            byte[] byteDsColSaledetails;
            byteDsColSaledetails = objCollateralMgtClient.FunPubGetColSaleDetails(intSaleNo, CompanyID);
            DsColSaledetails = (DataSet)ClsPubSerialize.DeSerialize(byteDsColSaledetails, SerializationMode.Binary, typeof(DataSet));
            DataTable DtCustSaleDetails = DsColSaledetails.Tables[0];
            DataTable DtItemDetails = DsColSaledetails.Tables[1];

            //Filling Collateral Sale Header details
            ViewState["LOB_ID"] = DtCustSaleDetails.Rows[0]["LOB_ID"].ToString();
            ViewState["Location_ID"] = DtCustSaleDetails.Rows[0]["Location_ID"].ToString();
            ViewState["CUST_ID"] = DtCustSaleDetails.Rows[0]["CUSTOMER_ID"].ToString();
            ViewState["COLLATERAL_SALE_ID"] = DtCustSaleDetails.Rows[0]["COLLATERAL_SALE_ID"].ToString();
            ViewState["APPROVAL_ID"] = DtCustSaleDetails.Rows[0]["APPROVAL_ID"].ToString();
            ViewState["COLLATERAL_SALE_NO"] = DtCustSaleDetails.Rows[0]["COLLATERAL_SALE_NO"].ToString();
            ViewState["COLLATERAL_SALE_DATE"] = DtCustSaleDetails.Rows[0]["COLLATERAL_SALE_DATE"].ToString();
            ViewState["COLLATERAL_CAPTURE_ID"] = DtCustSaleDetails.Rows[0]["COLLATERAL_CAPTURE_ID"].ToString();
            ViewState["COLTRAN_NO"] = DtCustSaleDetails.Rows[0]["COLLATERAL_TRAN_NO"].ToString();
            TxtColTranNo.Text = ViewState["COLTRAN_NO"].ToString();
            txtCollateralSaleDate.Text = Convert.ToDateTime(ViewState["COLLATERAL_SALE_DATE"].ToString()).ToString(ObjS3GSession.ProDateFormatRW);
            txtCollateralSaleNo.Text = ViewState["COLLATERAL_SALE_NO"].ToString();
            //ListItem  List = new ListItem(DtCustSaleDetails.Rows[0]["COLLATERAL_TRAN_NO"].ToString(),DtCustSaleDetails.Rows[0]["COLLATERAL_CAPTURE_ID"].ToString());
            //ddlColTranNo.Items.Add(List);
            if (DtCustSaleDetails.Rows[0]["COLLATERAL_COLLECTED_BY"].ToString() == "1")
            {
                PopulateCustomerDetails(ViewState["CUST_ID"].ToString(), 1);
            }
            else
            {
                PopulateCustomerDetails(ViewState["CUST_ID"].ToString(), 2);
            }

            //FunCollateralDetails(CompanyID, Convert.ToInt32(ViewState["CUST_ID"].ToString()),Convert.ToInt32(ViewState["COLLATERAL_CAPTURE_ID"].ToString()));
            SetTabIndexChanged();
            //Filling Collateral Sale Details

            DataTable DTHSSaleDetails = DsColSaledetails.Tables[1];
            DataTable DTMSSaleDetails = DsColSaledetails.Tables[2];
            DataTable DTLSSaleDetails = DsColSaledetails.Tables[3];
            DataTable DTFSSaleDetails = DsColSaledetails.Tables[4];
            DataTable DTCSSaleDetails = DsColSaledetails.Tables[5];
            if (DTCSSaleDetails.Rows.Count > 0)
            {
                ChkCOM.Checked = true;
                foreach (DataRow dr in DTCSSaleDetails.Rows)
                {
                    foreach (GridViewRow gvrCom in gvCommoDetails.Rows)
                    {
                        Label LblItemRefNo = (Label)gvrCom.FindControl("LblItemRefNo");
                        CheckBox chkCommSale = (CheckBox)gvrCom.FindControl("chkCommSale");
                        TextBox txtCommSaleUnit = (TextBox)gvrCom.FindControl("txtCommSaleUnit");
                        TextBox txtCommAmount = (TextBox)gvrCom.FindControl("txtCommAmount");
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrCom.Enabled = true;
                            chkCommSale.Checked = true;
                            txtCommSaleUnit.Text = dr["SALE_UNIT"].ToString();
                            txtCommAmount.Text = dr["SALE_AMOUNT"].ToString();

                        }
                    }
                }
                TextBox TxtTotalUnits = (TextBox)gvCommoDetails.FooterRow.FindControl("TxtTotalUnits");
                TextBox TxtTotalAmount = (TextBox)gvCommoDetails.FooterRow.FindControl("TxtTotalAmount");
                TxtTotalUnits.Text = DsColSaledetails.Tables[10].Rows[0]["CS_TOTAL_SALE_UNIT"].ToString();
                TxtTotalAmount.Text = DsColSaledetails.Tables[10].Rows[0]["CS_TOTAL_SALE_AMOUNT"].ToString();
                foreach (GridViewRow gvrCom in gvCommoDetails.Rows)
                {
                    CheckBox chkCommSale = (CheckBox)gvrCom.FindControl("chkCommSale");
                    if (chkCommSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
                        {
                            gvrCom.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrCom.Cells[10].Enabled = true;
                        }
                        else
                        {
                            gvrCom.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                ChkCOM_CheckedChanged(this, new EventArgs());
            }
            if (DTFSSaleDetails.Rows.Count > 0)
            {
                ChkFS.Checked = true;
                foreach (DataRow dr in DTFSSaleDetails.Rows)
                {
                    foreach (GridViewRow gvrFin in gvFinDetails.Rows)
                    {
                        Label LblItemRefNo = (Label)gvrFin.FindControl("LblItemRefNo");
                        CheckBox chkFinSale = (CheckBox)gvrFin.FindControl("chkFinSale");
                        TextBox txtFinSaleUnit = (TextBox)gvrFin.FindControl("txtFinSaleUnit");
                        TextBox txtFinAmount = (TextBox)gvrFin.FindControl("txtFinAmount");
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrFin.Enabled = true;
                            chkFinSale.Checked = true;
                            txtFinSaleUnit.Text = dr["SALE_UNIT"].ToString();
                            txtFinAmount.Text = dr["SALE_AMOUNT"].ToString();
                        }

                    }
                }
                TextBox TxtTotalUnits = (TextBox)gvFinDetails.FooterRow.FindControl("TxtTotalUnits");
                TextBox TxtTotalAmount = (TextBox)gvFinDetails.FooterRow.FindControl("TxtTotalAmount");
                TxtTotalUnits.Text = DsColSaledetails.Tables[9].Rows[0]["FS_TOTAL_SALE_UNIT"].ToString();
                TxtTotalAmount.Text = DsColSaledetails.Tables[9].Rows[0]["FS_TOTAL_SALE_AMOUNT"].ToString();
                foreach (GridViewRow gvrFin in gvFinDetails.Rows)
                {
                    CheckBox chkFinSale = (CheckBox)gvrFin.FindControl("chkFinSale");
                    if (chkFinSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                        {
                            gvrFin.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrFin.Cells[9].Enabled = true;
                        }
                        else
                        {
                            gvrFin.Enabled = false;
                        }
                    }
                    else
                    {
                        if (strMode == "M")
                        {
                            for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                            {
                                gvrFin.Cells[ColumnIndex].Enabled = false;
                            }
                            gvrFin.Cells[9].Enabled = true;
                        }
                        else
                        {
                            gvrFin.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                ChkFS_CheckedChanged(this, new EventArgs());
            }
            if (DTHSSaleDetails.Rows.Count > 0)
            {
                ChkHS.Checked = true;
                foreach (DataRow dr in DTHSSaleDetails.Rows)
                {
                    foreach (GridViewRow gvrHS in gvHighLiqDetails.Rows)
                    {
                        Label LblItemRefNo = (Label)gvrHS.FindControl("LblItemRefNo");
                        CheckBox chkHigSale = (CheckBox)gvrHS.FindControl("chkHigSale");
                        TextBox txtHigSaleUnit = (TextBox)gvrHS.FindControl("txtHigSaleUnit");
                        TextBox txtHigAmount = (TextBox)gvrHS.FindControl("txtHigAmount");
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrHS.Enabled = true;
                            chkHigSale.Checked = true;
                            txtHigSaleUnit.Text = dr["SALE_UNIT"].ToString();
                            txtHigAmount.Text = dr["SALE_AMOUNT"].ToString();

                        }
                    }
                }
                TextBox TxtTotalUnits = (TextBox)gvHighLiqDetails.FooterRow.FindControl("TxtTotalUnits");
                TextBox TxtTotalAmount = (TextBox)gvHighLiqDetails.FooterRow.FindControl("TxtTotalAmount");
                TxtTotalUnits.Text = DsColSaledetails.Tables[6].Rows[0]["HS_TOTAL_SALE_UNIT"].ToString();
                TxtTotalAmount.Text = DsColSaledetails.Tables[6].Rows[0]["HS_TOTAL_SALE_AMOUNT"].ToString();
                foreach (GridViewRow gvrHS in gvHighLiqDetails.Rows)
                {
                    CheckBox chkHigSale = (CheckBox)gvrHS.FindControl("chkHigSale");
                    if (chkHigSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
                        {
                            gvrHS.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrHS.Cells[10].Enabled = true;
                        }
                        else
                        {
                            gvrHS.Enabled = false;
                        }

                    }
                }
            }
            else
            {
                ChkHS_CheckedChanged(this, new EventArgs());
            }
            if (DTLSSaleDetails.Rows.Count > 0)
            {
                ChkLS.Checked = true;
                foreach (DataRow dr in DTLSSaleDetails.Rows)
                {
                    foreach (GridViewRow gvrLow in gvLowLiqDetails.Rows)
                    {
                        Label LblItemRefNo = (Label)gvrLow.FindControl("LblItemRefNo");
                        CheckBox chkLowSale = (CheckBox)gvrLow.FindControl("chkLowSale");
                        TextBox txtLowSaleUnit = (TextBox)gvrLow.FindControl("txtLowSaleUnit");
                        TextBox txtLowAmount = (TextBox)gvrLow.FindControl("txtLowAmount");
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrLow.Enabled = true;
                            chkLowSale.Checked = true;
                            txtLowSaleUnit.Text = dr["SALE_UNIT"].ToString();
                            txtLowAmount.Text = dr["SALE_AMOUNT"].ToString();

                        }
                    }
                }
                TextBox TxtTotalUnits = (TextBox)gvLowLiqDetails.FooterRow.FindControl("TxtTotalUnits");
                TextBox TxtTotalAmount = (TextBox)gvLowLiqDetails.FooterRow.FindControl("TxtTotalAmount");
                TxtTotalUnits.Text = DsColSaledetails.Tables[8].Rows[0]["LS_TOTAL_SALE_UNIT"].ToString();
                TxtTotalAmount.Text = DsColSaledetails.Tables[8].Rows[0]["LS_TOTAL_SALE_AMOUNT"].ToString();
                foreach (GridViewRow gvrLow in gvLowLiqDetails.Rows)
                {
                    CheckBox chkLowSale = (CheckBox)gvrLow.FindControl("chkLowSale");
                    if (chkLowSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                        {
                            gvrLow.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrLow.Cells[9].Enabled = true;
                        }
                        else
                        {
                            gvrLow.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                ChkLS_CheckedChanged(this, new EventArgs());
            }
            if (DTMSSaleDetails.Rows.Count > 0)
            {
                ChkMS.Checked = true;
                foreach (DataRow dr in DTMSSaleDetails.Rows)
                {
                    foreach (GridViewRow gvrMed in gvMedLiqDetails.Rows)
                    {
                        Label LblItemRefNo = (Label)gvrMed.FindControl("LblItemRefNo");
                        CheckBox chkMedSale = (CheckBox)gvrMed.FindControl("chkMedSale");
                        TextBox txtMedSaleUnit = (TextBox)gvrMed.FindControl("txtMedSaleUnit");
                        TextBox txtMedAmount = (TextBox)gvrMed.FindControl("txtMedAmount");
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrMed.Enabled = true;
                            chkMedSale.Checked = true;
                            txtMedSaleUnit.Text = dr["SALE_UNIT"].ToString();
                            txtMedAmount.Text = dr["SALE_AMOUNT"].ToString();

                        }
                    }
                }
                TextBox TxtTotalUnits = (TextBox)gvMedLiqDetails.FooterRow.FindControl("TxtTotalUnits");
                TextBox TxtTotalAmount = (TextBox)gvMedLiqDetails.FooterRow.FindControl("TxtTotalAmount");
                TxtTotalUnits.Text = DsColSaledetails.Tables[7].Rows[0]["MS_TOTAL_SALE_UNIT"].ToString();
                TxtTotalAmount.Text = DsColSaledetails.Tables[7].Rows[0]["MS_TOTAL_SALE_AMOUNT"].ToString();
                foreach (GridViewRow gvrMed in gvMedLiqDetails.Rows)
                {
                    CheckBox chkMedSale = (CheckBox)gvrMed.FindControl("chkMedSale");
                    if (chkMedSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 11; ColumnIndex++)
                        {
                            gvrMed.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrMed.Enabled = false;
                            gvrMed.Cells[11].Enabled = true;
                        }
                        else
                        {
                            gvrMed.Enabled = false;
                        }
                    }
                    else
                    {
                        if (strMode == "M")
                        {
                            for (int ColumnIndex = 0; ColumnIndex < 11; ColumnIndex++)
                            {
                                gvrMed.Cells[ColumnIndex].Enabled = false;
                            }
                            gvrMed.Cells[11].Enabled = true;
                        }
                        else
                        {
                            gvrMed.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                ChkMS_CheckedChanged(this, new EventArgs());
            }

            ViewState["TOTAL_AMOUNT"] = DsColSaledetails.Tables[11].Rows[0]["TOTAL_SALE_AMOUNT"].ToString();
            ViewState["TOTAL_UNITS"] = DsColSaledetails.Tables[11].Rows[0]["TOTAL_SALE_UNITS"].ToString();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objCollateralMgtClient.Close();
        }
    }
    

    private void FunPriAddGrid(GridView Grid, string ValFld, string AmntFld, string FtValFls, string FtAmntFld,string Chk)
    {
        decimal SaleUnit=0,SaleAmount=0;
        foreach (GridViewRow Row in Grid.Rows)
        {
            CheckBox ChkRec = (CheckBox)Row.FindControl(Chk);
            TextBox txtUnits = (TextBox)Row.FindControl(ValFld);
            TextBox txtAmount = (TextBox)Row.FindControl(AmntFld);

            if (ChkRec.Checked == true)
            {
                SaleUnit += Convert.ToDecimal(ChkEmpty(txtUnits.Text.ToString()));
                SaleAmount += Convert.ToDecimal(ChkEmpty(txtAmount.Text.ToString().Replace("NaN","")));
            }
        }

        if (Grid.Rows.Count != 0)
        {
            TextBox TxtTotalUnits = (TextBox)Grid.FooterRow.FindControl(FtValFls);
            TextBox TxtTotalAmount = (TextBox)Grid.FooterRow.FindControl(FtAmntFld);
            TxtTotalUnits.SetDecimalPrefixSuffix(10, 2, false);
            TxtTotalAmount.SetDecimalPrefixSuffix(10, 2, false);
            TxtTotalUnits.Text = SaleUnit.ToString();
            TxtTotalAmount.Text = SaleAmount.ToString();
        }
    }
    public string ChkEmpty(string str)
    {
        if (str == "")
        {
            return "0";
        }
        else
        {
            return str;
        }
        
    }
    public void FunPubProInsertSaleApprovalDetails()
    {
         objCollateralMgtClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
 
        try
        {

            if (tpRevoke.Visible == true)
            {
                if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, TxtRvkPwd.Text.Trim()) > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Invalid Password"); TxtRvkPwd.Focus();
                    return;
                }
            }
            else
            {
                if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtPassword.Text.Trim()) > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Invalid Password"); txtPassword.Focus();
                    return;
                }
            }
            
            int intErrorCode  = 0;
           
            objDataTable = new S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ApprovalDataTable();

            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ApprovalRow objRow;

            objRow = objDataTable.NewS3G_CLT_ApprovalRow();

            //For example reference only not a original value

            objRow.Collateral_Sale_ID = Convert.ToInt32(ViewState["COLLATERAL_SALE_ID"].ToString());
            objRow.Collateral_Sale_No = ViewState["COLLATERAL_SALE_NO"].ToString();

            objRow.Status_Type_Code = 5;
            if (tpRevoke.Visible == true)
            {
                objRow.Status_Code =5;
                objRow.Task_ApprovalUserID = intUserID;
                objRow.Created_By = intUserID;
                objRow.Remarks = TxtRvkRemarks.Text.ToString();
                objRow.Password = TxtRvkPwd.Text.ToString();
                objRow.Company_ID = intCompanyID;
            }
            else
            {

                if (ddlStatus.SelectedValue != "")
                {
                    objRow.Status_Code = Convert.ToInt32(ddlStatus.SelectedValue);
                }
                else
                {
                    objRow.Status_Code = 0;
                }
                objRow.Task_ApprovalUserID = intUserID;
                objRow.Created_By = intUserID;
                objRow.Remarks = txtRemarks.Text.ToString();
                objRow.Password = txtPassword.Text.ToString();
                objRow.Company_ID = intCompanyID;
            }
          
            try
            {
                if (ViewState["APPROVAL_ID"].ToString() != "")
                {
                    objRow.Approval_ID = Convert.ToInt32(ViewState["APPROVAL_ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
               
            }
            objDataTable.AddS3G_CLT_ApprovalRow(objRow);
            if ((tpRevoke.Visible == false) && (ddlStatus.SelectedValue.ToString() != ""))
            {
                intErrorCode = objCollateralMgtClient.FunPubCreateCollateralSaleApproval(SerializationMode.Binary, ClsPubSerialize.Serialize(objDataTable, SerializationMode.Binary));
            }
            else
            {
                intErrorCode = objCollateralMgtClient.FunPubCreateCollateralSaleApproval(SerializationMode.Binary, ClsPubSerialize.Serialize(objDataTable, SerializationMode.Binary));
            }

            if (intErrorCode == 0)
            {
                //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                if ((tcCollateralSale.ActiveTab == tpColSaleApp) && (ddlStatus.SelectedItem.Text == "Approved"))
                {
                    strAlert = strAlert.Replace("__ALERT__", "Collateral Sale has been Approved successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    return;
                }
                else if ((tcCollateralSale.ActiveTab == tpColSaleApp) && (ddlStatus.SelectedItem.Text == "Rejected"))
                {
                    strAlert = strAlert.Replace("__ALERT__", "Collateral Sale has been Rejected successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    return;
                }
                else if (tcCollateralSale.ActiveTab == tpRevoke)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Collateral Sale has been Revoked successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    return;
                }
            }
            else
            {
                if (intErrorCode == 11)
                {
                    Utility.FunShowAlertMsg(this.Page, "The Created User should not able to Approve"); ddlStatus.Focus(); return;
                }
                else if (intErrorCode == 12)
                {
                    Utility.FunShowAlertMsg(this.Page, "The user who approved the Sale should Revoke the Sale Approval"); return;
                }
                else if (intErrorCode == 1)
                {
                    Utility.FunShowAlertMsg(this.Page, "Invalid Password"); txtPassword.Focus(); return;
                }
                else if (intErrorCode == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, "Approval can be done by a user of Level 3 and above only"); return;
                }
                else if ((intErrorCode == -1) || (intErrorCode == -2) || (intErrorCode == 50))
                {
                    Utility.FunShowValidationMsg(this.Page, "", intErrorCode); return;
                }
                else
                {
                    Utility.FunShowValidationMsg(this.Page, "Collateral Details", intErrorCode); return;
                }
              

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //if (ObjS3GAdminServices != null)
            //{
                ObjS3GAdminServices.Close();
            //}
            //if (objCollateralMgtClient != null)
            //{
                objCollateralMgtClient.Close();
            //}
        }

    }
    private void FunPriBindApprovalGrid()
    {
        Procparam= new Dictionary<string,string>();
        Procparam.Add("@Company_ID",intCompanyID.ToString());
        Procparam.Add("@Collateral_Sale_ID",ViewState["COLLATERAL_SALE_ID"].ToString());
        DataSet DS = Utility.GetDataset("S3G_CLT_GetCLTSaleApprovalDetails", Procparam);
        if (DS.Tables[0].Rows.Count > 0)
        {
            ViewState["APPROVAL"] = DS.Tables[0];
            grdApprovalHistory.DataSource = DS.Tables[0];
            grdApprovalHistory.DataBind();
        }
        else
        {
            grdApprovalHistory.EmptyDataText = "No Records Found!";
            grdApprovalHistory.DataBind();
        }
    }
    private void FunPriColSaleControlStatus(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                ChkCOM.Visible = false;
                ChkFS.Visible = false;
                ChkHS.Visible = false;
                ChkLS.Visible = false;
                ChkMS.Visible = false;
                tcCollateralSale.ActiveTab = tpColCustAgnt;
                tpColGeneral.Visible = false;
                tpColDtl.Visible = false;
                tpColSaleApp.Visible = false;
                tpSaleInvoice.Visible = false;
                tpRevoke.Visible = false;
                btnClear.Enabled = false;
                //if (!bCreate)
                //{
                    btnSave.Enabled = false;
                //}
                    txthiddenfield.Text = "C";
                break;


            case 1: //Modify

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                tcCollateralSale.ActiveTab = tpColGeneral;
                tpColGeneral.Visible = true;
                tpColDtl.Visible = true;
                tpColSaleApp.Visible = true;
                tpSaleInvoice.Visible = false;
                tpRevoke.Visible = false;
                tpColCustAgnt.Visible = false;
                ChkCOM.Enabled = true;
                ChkFS.Enabled = true;
                ChkHS.Enabled = true;
                ChkLS.Enabled = true;
                ChkMS.Enabled = true;
                foreach (GridViewRow gvr in gvCommoDetails.Rows)
                {
                    CheckBox ChkCommSale = (CheckBox)gvr.FindControl("chkCommSale");
                    chkCommSale_CheckedChanged(ChkCommSale, new EventArgs());
                }
                foreach (GridViewRow gvr in gvHighLiqDetails.Rows)
                {
                    CheckBox ChkHighSale = (CheckBox)gvr.FindControl("chkHigSale");
                    chkHigSale_CheckedChanged(ChkHighSale, new EventArgs());
                }
                foreach (GridViewRow gvr in gvLowLiqDetails.Rows)
                {
                    CheckBox ChkLowSale = (CheckBox)gvr.FindControl("chkLowSale");
                    chkLowSale_CheckedChanged(ChkLowSale, new EventArgs());
                }
                foreach (GridViewRow gvr in gvMedLiqDetails.Rows)
                {
                    CheckBox chkMedSale = (CheckBox)gvr.FindControl("chkMedSale");
                    chkMedSale_CheckedChanged(chkMedSale, new EventArgs());
                }
                foreach (GridViewRow gvr in gvFinDetails.Rows)
                {
                    CheckBox chkFinSale = (CheckBox)gvr.FindControl("chkFinSale");
                    chkFinSale_CheckedChanged(chkFinSale, new EventArgs());
                }

                btnClear.Enabled = false;
                txtApprovalName.Text = ObjUserInfo.ProUserNameRW;
                FunPriBindStatus();

                //if (!bModify)
                //{
                    btnSave.Enabled = false;
                //}
                txthiddenfield.Text = "M";
                break;
            case -1://Query

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                tpColCustAgnt.Visible = false;
                tcCollateralSale.ActiveTab = tpColGeneral;
                tpColGeneral.Visible = true;
                tpColDtl.Visible = true;
                if (ViewState["APPROVAL"] != null)
                {
                    tpColSaleApp.Visible = true;
                }
                else
                {
                    tpColSaleApp.Visible = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                ChkCOM.Enabled = false;
                ChkFS.Enabled = false;
                ChkHS.Enabled = false;
                ChkLS.Enabled = false;
                ChkMS.Enabled = false;
                //ddlColTranNo.Enabled = false;
                //ucCustomerCodeLov.ControlStatus(false);
                txtCollateralSaleDate.Enabled = false;
                txtCollateralSaleNo.Enabled = false;
                gvCommoDetails.Enabled = false;
                gvFinDetails.Enabled = false;
                gvHighLiqDetails.Enabled = false;
                gvLowLiqDetails.Enabled = false;
                gvMedLiqDetails.Enabled = false;
                GrvSInvoiceDetails.Enabled=false;
                tpRevoke.Visible = false;
                txtApprovalName.Enabled = false;
                txtPassword.Enabled = false;
                txtRemarks.Enabled = false;
                btnClear.Enabled = false;
                //btnGo.Enabled = false;
                FunPubProGetSaleInvDetails(intCompanyID, intSaleNo);
                if (ViewState["DetailsTable"] != null)
                {
                    tpSaleInvoice.Visible = true;
                }
                else
                {
                    tpSaleInvoice.Visible = false;
                }
                btnSave.Enabled = false;  
                ddlStatus.Enabled = false;
                GrvReceiptDetails.Enabled = false;
                txthiddenfield.Text = "Q";
                break;
            case -2://If the Approval details are already present for this Collateral Sale goto invoice.
                tcCollateralSale.ActiveTab = tpSaleInvoice;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                ChkCOM.Enabled = false;
                ChkFS.Enabled = false;
                ChkHS.Enabled = false;
                ChkLS.Enabled = false;
                ChkMS.Enabled = false;
                //ddlColTranNo.Enabled = false;
                txtCollateralSaleDate.Enabled = false;
                txtCollateralSaleNo.Enabled = false;
                gvCommoDetails.Enabled = false;
                gvFinDetails.Enabled = false;
                gvHighLiqDetails.Enabled = false;
                gvLowLiqDetails.Enabled = false;
                gvMedLiqDetails.Enabled = false;
                txtApprovalName.Enabled = false;
                txtPassword.Enabled = false;
                txtRemarks.Enabled = false;
                btnClear.Enabled = false;
                //if (!bModify)
                //{
                    btnSave.Enabled = false;
                //}
                //btnGo.Enabled = false;
                tpColCustAgnt.Visible = false;
                tcCollateralSale.ActiveTab = tpSaleInvoice;
                tpSaleInvoice.Visible = true;
                ddlStatus.Enabled = false;
                txthiddenfield.Text = "M";
                GrvSInvoiceDetails.Enabled = true;
                if (ViewState["RECEIPTS"] != null)
                {
                    GrvReceiptDetails.Enabled = true;
                }
                else
                {
                    GrvReceiptDetails.Enabled = false;
                }
                tpRevoke.Visible = false;
                break;
            case -3:
                tcCollateralSale.ActiveTab = tpColGeneral;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                ChkCOM.Enabled = false;
                ChkFS.Enabled = false;
                ChkHS.Enabled = false;
                ChkLS.Enabled = false;
                ChkMS.Enabled = false;
                tpColCustAgnt.Visible = false;
                //ddlColTranNo.Enabled = false;
                //ucCustomerCodeLov.ControlStatus(false);
                txtCollateralSaleDate.Enabled = false;
                txtCollateralSaleNo.Enabled = false;
                TxtColTranNo.Enabled = false;
                gvCommoDetails.Enabled = false;
                gvFinDetails.Enabled = false;
                gvHighLiqDetails.Enabled = false;
                gvLowLiqDetails.Enabled = false;
                gvMedLiqDetails.Enabled = false;
                txtApprovalName.Enabled = false;
                txtPassword.Enabled = false;
                txtRemarks.Enabled = false;
                tpRevoke.Visible = true;
                TxtRvkAppName.Enabled = true;
                TxtRvkPwd.Enabled = true;
                TxtRvkRemarks.Enabled = true;
                LblRevoke.Enabled = true;
                btnClear.Enabled = false;
                //if (!bModify)
                //{
                //}
                //btnGo.Enabled = false;
                tpSaleInvoice.Visible = true;
                ddlStatus.Enabled = false;
                txthiddenfield.Text = "M";
                TxtRvkAppName.Text = ObjUserInfo.ProUserNameRW;
                FunProIntializeData();
                FunProIntializeInvoiceGV();
                FunProIntializeReceiptGV();
                FunPubBindInvoiceDetails();
                FunPubProGetSaleInvDetails(intCompanyID, intSaleNo);
                GrvSInvoiceDetails.Enabled = true;
                if (ViewState["RECEIPTS"] != null)
                {
                    GrvReceiptDetails.Enabled = true;
                }
                else
                {
                    GrvReceiptDetails.Enabled = false;
                }
                btnSave.Enabled = false; 
                break;
        }
    }

    #endregion

    #region [Button Events]
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        //hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
        ViewState["CUST_ID"] = hdnCID.Value;
        if (hdnCID != null && hdnCID.Value != "")
        {
            //FunCustomerList(Convert.ToInt32(hdnCID.Value));
            //PopulateTranNo(Convert.ToInt32(hdnCID.Value), intCompanyID);
        }
    }
    protected void txtHigSaleUnit_TextChanged(object sender, EventArgs e)
    {
        FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit", "txtHigAmount", "TxtTotalUnits", "TxtTotalAmount", "chkHigSale");
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //if (Request.QueryString["qsMode"] != null)
        //    strMode = Request.QueryString["qsMode"];
        //if (strMode == "C")
        //{
        //    tpColCustAgnt.Enabled = true;
        //    btnSave.Enabled = false;
        //    if (tcCollateralSale.ActiveTabIndex == 0)

        //        Response.Redirect(strRedirectPageViewTrans);
        //    else

        //    tcCollateralSale.ActiveTabIndex = 0;
        //    SetTabIndexChanged();
        //    FunPriBindGrid();
        //    //Response.Redirect(strRedirectPage);
        //}

        //else
        //{
            Response.Redirect(strRedirectPageViewTrans,false);

        //}
    }
    //protected void btnGo_Click(object sender, EventArgs e)
    //{
    //    if (Request.QueryString["qsMode"] != null)
    //        strMode = Request.QueryString["qsMode"];
    //    if (ddlColTranNo.SelectedItem.Text == "--Select--")
    //    {
    //        Utility.FunShowAlertMsg(this.Page, "Select a Collatral Transaction Number to proceed!");
    //    }
    //    if (strMode == "C")
    //    {
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@COLLATERAL_CAPTURE_ID", ddlColTranNo.SelectedValue.ToString());
    //        Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
    //        DataSet DS = Utility.GetDataset("S3G_CLT_ChkReceiptExists", Procparam);
    //        if (DS.Tables[0].Rows.Count == 0)
    //        {
    //            Utility.FunShowAlertMsg(this.Page, " A Collateral Sale is already under process for this Collateral Transaction Number. Modify the Collateral data in Modify mode to proceed!");
    //            return;
    //        }
    //        else if ((DS.Tables[0].Rows[0]["RECEIPT_NO"].ToString() == "FIRST RECORD") || (DS.Tables[0].Rows.Count > 0))
    //        {
    //            ChkCOM.Visible = true;
    //            ChkFS.Visible = true;
    //            ChkHS.Visible = true;
    //            ChkLS.Visible = true;
    //            ChkMS.Visible = true;
    //            ChkCOM_CheckedChanged(this, new EventArgs());
    //            ChkFS_CheckedChanged(this, new EventArgs());
    //            ChkHS_CheckedChanged(this, new EventArgs());
    //            ChkLS_CheckedChanged(this, new EventArgs());
    //            ChkMS_CheckedChanged(this, new EventArgs());
    //            FunCollateralDetails(intCompanyID, Convert.ToInt32(ViewState["CUST_ID"].ToString()), Convert.ToInt32(ddlColTranNo.SelectedValue));
    //        }
    //    }
    //    else
    //    {
    //        ChkCOM.Visible = true;
    //        ChkFS.Visible = true;
    //        ChkHS.Visible = true;
    //        ChkLS.Visible = true;
    //        ChkMS.Visible = true;
    //        ChkCOM_CheckedChanged(this, new EventArgs());
    //        ChkFS_CheckedChanged(this, new EventArgs());
    //        ChkHS_CheckedChanged(this, new EventArgs());
    //        ChkLS_CheckedChanged(this, new EventArgs());
    //        ChkMS_CheckedChanged(this, new EventArgs());
    //        FunCollateralDetails(intCompanyID, Convert.ToInt32(ViewState["CUST_ID"].ToString()), Convert.ToInt32(ddlColTranNo.SelectedValue));
    //    }
        
    //}
    #endregion

     
    protected void btnSave_Click(object sender, EventArgs e)
    {
       decimal InvoiceTotal=0; decimal ReceiptTotal = 0; bool rowchecked = false;

        if ((ChkCOM.Visible == true) || (ChkFS.Visible == true) || (ChkHS.Visible == true) || (ChkLS.Visible == true) || (ChkMS.Visible == true))
        {
            if ((!ChkCOM.Checked) && (!ChkFS.Checked) && (!ChkHS.Checked) && (!ChkLS.Checked) && (!ChkMS.Checked))
            {
                Utility.FunShowAlertMsg(this.Page, "Select a Commodity type to make a sale!");
                return;
            }
        }

        bool ItemChecked=false;
        strColSaleDetails.Append("<Root>");
        if (ChkHS.Checked)
        {
            foreach(GridViewRow gvr in gvHighLiqDetails.Rows)
            {
                CheckBox chkHigSale = (CheckBox)gvr.FindControl("chkHigSale");
                if (chkHigSale.Checked == true)
                {
                    ItemChecked = true;
                    break;
                }

            }
            if (ItemChecked == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Select an High Liquidity Asset to make a sale!");
                return;
            }
            else
            {
               
                foreach (GridViewRow gvr in gvHighLiqDetails.Rows)
                    {
                        Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                        TextBox txtHigSaleUnit = (TextBox)gvr.FindControl("txtHigSaleUnit");
                        TextBox txtHigAmount = (TextBox)gvr.FindControl("txtHigAmount");
                        CheckBox chkHigSale = (CheckBox)gvr.FindControl("chkHigSale");
                        if ((chkHigSale.Checked == true) && (chkHigSale.Enabled == true))
                        {
                            if(Convert.ToDecimal(txtHigAmount.Text.ToString()) != 0 )
                            {
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" SALE_UNIT = '" + Convert.ToString(txtHigSaleUnit.Text) + "'");
                            strColSaleDetails.Append(" SALE_AMOUNT = '" + Convert.ToString(txtHigAmount.Text) + "'/>");
                            }
                            else 
                            {
                                Utility.FunShowAlertMsg(this.Page, "The Sale Amount should not be Zero or Negative");
                                tcCollateralSale.ActiveTab = tpColDtl;
                                txtHigAmount.Focus();
                                return;
                                //"The value in the row " + gvr.RowIndex.ToString() + " is set a zero.
                            }
                        }
                     }
               
            }
        }
        if (ChkMS.Checked)
        {
            foreach (GridViewRow gvr in gvMedLiqDetails.Rows)
            {
                CheckBox chkMedSale = (CheckBox)gvr.FindControl("chkMedSale");
                if (chkMedSale.Checked == true)
                {
                    ItemChecked = true;
                    break;
                }

            }
            if (ItemChecked == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Select an Medium Liquidity Asset to make a sale!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvMedLiqDetails.Rows)
                {
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtMedSaleUnit = (TextBox)gvr.FindControl("txtMedSaleUnit");
                    TextBox txtMedAmount = (TextBox)gvr.FindControl("txtMedAmount");
                    CheckBox chkMedSale = (CheckBox)gvr.FindControl("chkMedSale");
                    if((chkMedSale.Checked ==true) &&(chkMedSale.Enabled==true))
                    {
                        if (Convert.ToDecimal(txtMedAmount.Text.ToString()) != 0)
                        {
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" SALE_UNIT = '" + Convert.ToString(txtMedSaleUnit.Text) + "'");
                            strColSaleDetails.Append(" SALE_AMOUNT = '" + Convert.ToString(txtMedAmount.Text) + "'/>");
                        }
                        else
                        {
                            Utility.FunShowAlertMsg(this.Page, "The Sale Amount should not be Zero or Negative.");
                            tcCollateralSale.ActiveTab = tpColDtl;
                            txtMedAmount.Focus();
                            return;
                           // "The value in the row " + gvr.RowIndex.ToString() + " is set a zero.
                        }
                    }
                }

            }
        }
        if (ChkLS.Checked)
        {
            foreach (GridViewRow gvr in gvLowLiqDetails.Rows)
            {
                CheckBox chkLowSale = (CheckBox)gvr.FindControl("chkLowSale");
                if (chkLowSale.Checked == true)
                {
                    ItemChecked = true;
                    break;
                }

            }
            if (ItemChecked == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Select an Low Liquidity Asset to make a sale!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvLowLiqDetails.Rows)
                {
                    CheckBox chkLowSale = (CheckBox)gvr.FindControl("chkLowSale");
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtLowSaleUnit = (TextBox)gvr.FindControl("txtLowSaleUnit");
                    TextBox txtLowAmount = (TextBox)gvr.FindControl("txtLowAmount");
                    if((chkLowSale.Checked==true)&&(chkLowSale.Enabled==true))
                    {
                        if (Convert.ToDecimal(txtLowAmount.Text.ToString()) != 0)
                        {
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" SALE_UNIT = '" + Convert.ToString(txtLowSaleUnit.Text) + "'");
                            strColSaleDetails.Append(" SALE_AMOUNT = '" + Convert.ToString(txtLowAmount.Text) + "'/>");
                        }
                        else
                        {
                            Utility.FunShowAlertMsg(this.Page, "The Sale Amount should not be Zero or Negative");
                            tcCollateralSale.ActiveTab = tpColDtl;
                            txtLowAmount.Focus();
                            return;
                            //"The value in the row " + gvr.RowIndex.ToString() + " is set a zero.
                        }
                    }
                }

            }
        }
        if (ChkCOM.Checked)
        {
            foreach (GridViewRow gvr in gvCommoDetails.Rows)
            {
                CheckBox chkCommSale = (CheckBox)gvr.FindControl("chkCommSale");
                if (chkCommSale.Checked == true)
                {
                    ItemChecked = true;
                    break;
                }

            }
            if (ItemChecked == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Select an Commodities Asset to make a sale!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvCommoDetails.Rows)
                {
                    CheckBox chkCommSale = (CheckBox)gvr.FindControl("chkCommSale");
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtCommSaleUnit = (TextBox)gvr.FindControl("txtCommSaleUnit");
                    TextBox txtCommAmount = (TextBox)gvr.FindControl("txtCommAmount");
                    Label LblAvlUnits = (Label)gvr.FindControl("LblAvlUnits");
                    LblAvlUnits.Text = txtCommSaleUnit.Text; 
                    if ((chkCommSale.Checked == true) && (chkCommSale.Enabled == true))
                    {
                        if (Convert.ToDecimal(txtCommAmount.Text.ToString()) != 0)
                        {
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" SALE_UNIT = '" + Convert.ToString(txtCommSaleUnit.Text) + "'");
                            strColSaleDetails.Append(" SALE_AMOUNT = '" + Convert.ToString(txtCommAmount.Text) + "'/>");
                        }
                        else
                        {
                            Utility.FunShowAlertMsg(this.Page, "The Sale Amount should not be Zero or Negative.");
                            tcCollateralSale.ActiveTab = tpColDtl;
                            txtCommAmount.Focus();
                            return;
                            //"The value in the row " + gvr.RowIndex.ToString() + " is set a zero.
                        }
                    }
                }

            }
        }
        if (ChkFS.Checked)
        {
            foreach (GridViewRow gvr in gvFinDetails.Rows)
            {
                CheckBox chkFinSale = (CheckBox)gvr.FindControl("chkFinSale");
                if (chkFinSale.Checked == true)
                {
                    ItemChecked = true;
                    break;
                }

            }
            if (ItemChecked == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Select an Financial Securities Asset to make a sale!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvFinDetails.Rows)
                {
                    CheckBox chkFinSale = (CheckBox)gvr.FindControl("chkFinSale");
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtFinSaleUnit = (TextBox)gvr.FindControl("txtFinSaleUnit");
                    TextBox txtFinAmount = (TextBox)gvr.FindControl("txtFinAmount");
                    if((chkFinSale.Checked==true) &&(chkFinSale.Enabled==true))
                    {
                        if (Convert.ToDecimal(txtFinAmount.Text.ToString()) != 0)
                        {
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" SALE_UNIT = '" + Convert.ToString(txtFinSaleUnit.Text) + "'");
                            strColSaleDetails.Append(" SALE_AMOUNT = '" + Convert.ToString(txtFinAmount.Text) + "'/>");
                        }
                        else
                        {
                            Utility.FunShowAlertMsg(this.Page, "The Sale Amount should not be Zero or Negative");
                            tcCollateralSale.ActiveTab = tpColDtl;
                            txtFinAmount.Focus();
                            return;
                            //"The value in the row " + gvr.RowIndex.ToString() + " is set a zero. 
                        }
                    }
                }

            }
        }
        strColSaleDetails.Append("</Root>");

        strColSaleInvoiceDetails.Append("<Root>");
        if (GrvSInvoiceDetails.Rows.Count > 0)
        {
            InvoiceTotal = Convert.ToDecimal(ViewState["TOTAL_AMOUNT"].ToString());
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
                strColSaleInvoiceDetails.Append(" <INVDETAILS Sale_Invoice_Sno = '1'");
                strColSaleInvoiceDetails.Append(" Sale_Type='" + Convert.ToString(ddlSInvAccountType.SelectedValue.ToString()) + "'");
                strColSaleInvoiceDetails.Append(" Sale_Account_Code = '" + Convert.ToString(ddlSInvAccountCode.SelectedValue.ToString()) + "'");
                strColSaleInvoiceDetails.Append(" Debit_GL_Account = '" + Convert.ToString(LblSInvDebitGLCode.Text.ToString()) + "'");
                if (ddlSInvDebitSLCode.SelectedItem.Text.ToString() == "")
                {
                    strColSaleInvoiceDetails.Append(" Debit_SL_Account = '" + DBNull.Value + "'");
                }
                else
                {
                    strColSaleInvoiceDetails.Append(" Debit_SL_Account = '" + Convert.ToString(ddlSInvDebitSLCode.SelectedItem.Text.ToString()) + "'");
                }
                strColSaleInvoiceDetails.Append(" Description = '" + Convert.ToString(LblSInvGlDesc.Text.ToString()) + "'");
                strColSaleInvoiceDetails.Append(" Credit_GL_Account ='" + Convert.ToString(LblSInvCrdGLCode.Text.ToString()) + "'");
                if (ddlSInvCrdSLCode.SelectedItem.Text.ToString() == "")
                {
                    strColSaleInvoiceDetails.Append(" Credit_SL_Account = '" + DBNull.Value + "'");
                }
                else
                {
                    strColSaleInvoiceDetails.Append(" Credit_SL_Account = '" + Convert.ToString(ddlSInvCrdSLCode.SelectedItem.Text.ToString()) + "'");
                }
                strColSaleInvoiceDetails.Append(" Amount ='" + decimal.Parse(TxtSInvAmount.Text.ToString()) + "'/>");
            }
        }
        strColSaleInvoiceDetails.Append("</Root>");

        

        foreach (GridViewRow gvr in GrvReceiptDetails.Rows)
        {
            CheckBox ChkRcpt = (CheckBox)gvr.FindControl("ChkRcpt");
            if ((ChkRcpt.Checked == true) && (gvr.Enabled == true))
            {
                ReceiptTotal += Convert.ToDecimal(((TextBox)gvr.FindControl("TxtSInvTxnAmnt")).Text);
                if (rowchecked != true)
                {
                    rowchecked = true;
                }
            }
        }
        if ((rowchecked == true) && (InvoiceTotal > ReceiptTotal))
        {
            Utility.FunShowAlertMsg(this.Page, "The Receipt amount should be equal to or greater than the Invoice amount!");
            return;
        }

        strColSaleReceiptDetails.Append("<Root>");

        if (GrvReceiptDetails.Rows.Count > 0)
        {
            foreach (GridViewRow GvrRcptDtls in GrvReceiptDetails.Rows)
            {
                CheckBox ChkRcpt = (CheckBox)GvrRcptDtls.FindControl("ChkRcpt");
                TextBox TxtRcptRefNo = (TextBox)GvrRcptDtls.FindControl("TxtRcptRefNo");
                TextBox TxtSInvTxnDate = (TextBox)GvrRcptDtls.FindControl("TxtSInvTxnDate");
                TextBox TxtSInvTxnAmnt = (TextBox)GvrRcptDtls.FindControl("TxtSInvTxnAmnt");
                Label LblRcptID = (Label)GvrRcptDtls.FindControl("LblRcptID");
                if ((ChkRcpt.Checked == true) && (GvrRcptDtls.Enabled == true))
                {
                    strColSaleReceiptDetails.Append(" <RCPTDETAILS Sale_Invoice_Rcpt_SrNo = '" + Convert.ToString(GvrRcptDtls.RowIndex + 1) + "'");
                    strColSaleReceiptDetails.Append(" Receipt_ID='" + Convert.ToString(LblRcptID.Text.ToString()) + "'");
                    strColSaleReceiptDetails.Append(" Receipt_No = '" + Convert.ToString(TxtRcptRefNo.Text.ToString()) + "'");
                    strColSaleReceiptDetails.Append(" Trxn_Date = '" + Utility.StringToDate(TxtSInvTxnDate.Text.ToString()) + "'");
                    strColSaleReceiptDetails.Append(" Trxn_Amount = '" + Convert.ToString(TxtSInvTxnAmnt.Text.ToString()) + "'/>");

                }

            }
        }

        strColSaleReceiptDetails.Append("</Root>");

        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];
       
        if (strMode == "C")
        {
            FunPubProInsertCollateralSale(strColSaleDetails,strColSaleInvoiceDetails,strColSaleReceiptDetails);
        }
        else if (strMode == "M")
        {
            bool ValueExists=false;
            foreach (GridViewRow GvrInvDtls in GrvSInvoiceDetails.Rows)
            {
                DropDownList ddlSInvAccountType = (DropDownList)GvrInvDtls.FindControl("ddlSInvAccountType");
                if (ddlSInvAccountType.SelectedIndex > 0)
                {
                    ValueExists = true;
                    break;
                }

            }
            if((TxtRvkPwd.Text !=string.Empty) &&(ValueExists==true))
            {
                Utility.FunShowAlertMsg(this.Page, "You may only process either Revoke or Sale Invoice, one at a time.Make changes and proceed!");
                return;
            }
            if (ValueExists == false)
            {
                strColSaleInvoiceDetails.Replace(strColSaleInvoiceDetails.ToString(), "<Root></Root>");
                strColSaleReceiptDetails.Replace(strColSaleReceiptDetails.ToString(),"<Root></Root>");
            }
            if (tcCollateralSale.ActiveTab == tpColDtl)
            {
                FunPubProUpdateCollateralSale(strColSaleDetails, strColSaleInvoiceDetails, strColSaleReceiptDetails);
            }
            else if ((tcCollateralSale.ActiveTab == tpColSaleApp) || (tcCollateralSale.ActiveTab == tpRevoke))
            {
                FunPubProInsertSaleApprovalDetails();
            }
            else if ((tcCollateralSale.ActiveTab == tpSaleInvoice))
            {
                FunPubProUpdateCollateralSaleInvoiceDetails(strColSaleDetails, strColSaleInvoiceDetails, strColSaleReceiptDetails);
            }
        }

        
    }

    private void FunPubProUpdateCollateralSaleInvoiceDetails(StringBuilder strColSaleDetails, StringBuilder strColSaleInvoiceDetails, StringBuilder strColSaleReceiptDetails)
    {
        objCollateralMgtClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {

            if (tpSaleInvoice.Visible == true)
            {
                decimal InvoiceTotal = 0;
                decimal ReceiptTotal = 0;
                bool rowchecked = false;
                InvoiceTotal = Convert.ToDecimal(((TextBox)GrvSInvoiceDetails.FooterRow.FindControl("TxtSInvTotal")).Text);
                ReceiptTotal = 0;
                foreach (GridViewRow gvr in GrvReceiptDetails.Rows)
                {
                    CheckBox ChkRcpt = (CheckBox)gvr.FindControl("ChkRcpt");
                    if ((ChkRcpt.Checked == true) && (gvr.Enabled == true))
                    {
                        ReceiptTotal += Convert.ToDecimal(((TextBox)gvr.FindControl("TxtSInvTxnAmnt")).Text);
                        if (rowchecked != true)
                        {
                            rowchecked = true;
                        }
                    }
                }
                if ((rowchecked == true) && (InvoiceTotal > ReceiptTotal))
                {
                    Utility.FunShowAlertMsg(this.Page, "The Receipt amount should be equal to or greater than the Invoice amount!");
                    return;
                }
            }
            intSaleNo = Convert.ToInt32(ViewState["COLLATERAL_SALE_ID"]);
            string ColSaleNo;
            int ErrorCode = 0;
            objCollateralSaleDataTable = new S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralSaleDataTable();
            objCollateralSaleDataRow = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralSaleRow)objCollateralSaleDataTable.NewRow();
            objCollateralSaleDataRow.Collateral_Capture_ID = Convert.ToInt32(ViewState["COLLATERAL_CAPTURE_ID"].ToString());
            objCollateralSaleDataRow.Collateral_Sale_Date = Utility.StringToDate(txtCollateralSaleDate.Text.ToString());
            objCollateralSaleDataRow.Collateral_Tran_No = ViewState["COLTRAN_NO"].ToString();
            objCollateralSaleDataRow.Company_ID = intCompanyID;
            objCollateralSaleDataRow.Created_By = intUserID;
            objCollateralSaleDataRow.Modified_By = intUserID;
            objCollateralSaleDataRow.Created_On = DateTime.Now;
            objCollateralSaleDataRow.XMLSALEDETAILS = strColSaleDetails.ToString();
            objCollateralSaleDataRow.XMLINVDETAILS = strColSaleInvoiceDetails.ToString();
            objCollateralSaleDataRow.XMLRECEIPTDETAILS = strColSaleReceiptDetails.ToString();
            objCollateralSaleDataTable.Rows.Add(objCollateralSaleDataRow);

            if (objCollateralSaleDataTable.Rows.Count > 0)
            {

                ErrorCode = objCollateralMgtClient.FunPubUpdateCollateralSaleInvoicedetails(ClsPubSerialize.Serialize(objCollateralSaleDataTable, SerializationMode.Binary), SerializationMode.Binary, Convert.ToInt32(ViewState["COLLATERAL_SALE_ID"]));
                switch (ErrorCode)
                {
                    case 0:
                        if (intSaleNo > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Collateral Sales Invoice detail have been updated sucessfully");
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
                        if (intSaleNo > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Collateral Sale Invoice details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Collateral Sale Invoice details");
                        }
                        strRedirectPageView = "";
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objCollateralMgtClient.Close();
        }

    }

#region Checkbox Events

    protected void  ChkHS_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkHS.Checked)
        {
            gvHighLiqDetails.Enabled = false;

            FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit", "txtHigAmount", "TxtTotalUnits", "TxtTotalAmount", "chkHigSale");
        }
        else
        {
            gvHighLiqDetails.Enabled = true;
            foreach (GridViewRow gvr in gvHighLiqDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[10].Enabled = true;
                    if (((CheckBox)gvr.FindControl("chkHigSale")).Checked == true)
                    {
                        gvr.Cells[8].Enabled = true;
                        gvr.Cells[9].Enabled = true;
                    }
                }
                else
                {
                    gvr.Enabled = false;
                }
            }

            FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit", "txtHigAmount", "TxtTotalUnits", "TxtTotalAmount", "chkHigSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }
    protected void ChkMS_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkMS.Checked)
        {
            gvMedLiqDetails.Enabled = false;
            FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "txtMedAmount", "TxtTotalUnits", "TxtTotalAmount","chkMedSale");
        }
        else
        {
            gvMedLiqDetails.Enabled = true;
            foreach (GridViewRow gvr in gvMedLiqDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 11; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[11].Enabled = true;
                    
                }
                else
                {
                    gvr.Enabled = false;
                }
            }
        }
        FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "txtMedAmount", "TxtTotalUnits", "TxtTotalAmount", "chkMedSale");
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }

    protected void ChkLS_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkLS.Checked)
        {
            gvLowLiqDetails.Enabled = false;
            FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit", "txtLowAmount", "TxtTotalUnits", "TxtTotalAmount", "chkLowSale");
        }
        else
        {
            gvLowLiqDetails.Enabled = true;
            foreach (GridViewRow gvr in gvLowLiqDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[9].Enabled = true;
                    if (((CheckBox)gvr.FindControl("chkLowSale")).Checked == true)
                    {
                        gvr.Cells[7].Enabled = true;
                        gvr.Cells[8].Enabled = true;
                    }
                }
                else
                {
                    gvr.Enabled = false;
                }
            
            }
            FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit", "txtLowAmount", "TxtTotalUnits", "TxtTotalAmount", "chkLowSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }
    protected void ChkCOM_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkCOM.Checked)
        {
            gvCommoDetails.Enabled = false;
            FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit", "txtCommAmount", "TxtTotalUnits", "TxtTotalAmount", "chkCommSale");
        }
        else
        {
            gvCommoDetails.Enabled = true;
            foreach (GridViewRow gvr in gvCommoDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[10].Enabled = true;
                    if (((CheckBox)gvr.FindControl("chkCommSale")).Checked == true)
                    {
                        gvr.Cells[8].Enabled = true;
                        gvr.Cells[9].Enabled = true;
                    }
                }
                else
                {
                    gvr.Enabled = false;
                }
            }
            FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit", "txtCommAmount", "TxtTotalUnits", "TxtTotalAmount", "chkCommSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }
    protected void ChkFS_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkFS.Checked)
        {
            gvFinDetails.Enabled = false;
            FunPriAddGrid(gvFinDetails, "txtFinSaleUnit", "txtFinAmount", "TxtTotalUnits", "TxtTotalAmount", "chkFinSale");
        }
        else
        {
            gvFinDetails.Enabled = true;
            foreach (GridViewRow gvr in gvFinDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[9].Enabled = true;
                }
                else
                {
                    gvr.Enabled = false;
                }
            }
            FunPriAddGrid(gvFinDetails, "txtFinSaleUnit", "txtFinAmount", "TxtTotalUnits", "TxtTotalAmount", "chkFinSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }
#endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlStatus.SelectedValue = "0";
        txtPassword.Text = "";
        txtRemarks.Text = "";
        tpColCustAgnt.Enabled = true;
        tcCollateralSale.ActiveTabIndex = 0;
        SetTabIndexChanged();
        FunPriBindGrid();
        btnSave.Enabled = false;

        if (Request.QueryString["qsMode"] != null)
           strMode = Request.QueryString["qsMode"];
        if (strMode == "C")
        {
            if (tcCollateralSale.ActiveTab == tpColCustAgnt)
            {
                btnClear.Enabled = false;
            }
            else
            {
                btnClear.Enabled = true;
            }
        }
    }
    protected void chkHigSale_CheckedChanged(object sender, EventArgs e)
    
    {
        CheckBox chkHigSale = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow) chkHigSale.NamingContainer;
        int RowIndex=gvr.RowIndex;
        TextBox txtHigAmount = (TextBox)gvHighLiqDetails.Rows[RowIndex].FindControl("txtHigAmount");
        TextBox txtHigSaleUnit = (TextBox)gvHighLiqDetails.Rows[RowIndex].FindControl("txtHigSaleUnit");
        if (chkHigSale.Checked == true)
        {
            for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            {
                gvHighLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            }
            txtHigSaleUnit.Focus();
            FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit", "txtHigAmount", "TxtTotalUnits", "TxtTotalAmount", "chkHigSale");
        }
        else
        {
            for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            {
                gvHighLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            }

            Label LblAvlUnits = (Label)gvHighLiqDetails.Rows[RowIndex].FindControl("LblAvlUnits");
            Label LblValAmnt = (Label)gvHighLiqDetails.Rows[RowIndex].FindControl("LblValAmnt");
            txtHigSaleUnit.Text = LblAvlUnits.Text;
            txtHigAmount.Text = Convert.ToString(LblValAmnt.Text);
            FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit", "txtHigAmount", "TxtTotalUnits", "TxtTotalAmount", "chkHigSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }

    protected void chkMedSale_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkMedSale=(CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkMedSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
        TextBox txtMedAmount = (TextBox)gvMedLiqDetails.Rows[RowIndex].FindControl("txtMedAmount");
        TextBox txtMedSaleUnit = (TextBox)gvMedLiqDetails.Rows[RowIndex].FindControl("txtMedSaleUnit");
        if (chkMedSale.Checked == true)
        {
            //for (int ColumnIndex = 0; ColumnIndex < 11; ColumnIndex++)
            //{
            //    gvMedLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            //}
            //txtMedSaleUnit.Focus();
            FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "txtMedAmount", "TxtTotalUnits", "TxtTotalAmount", "chkMedSale");
            gvMedLiqDetails.Rows[RowIndex].Cells[10].Enabled = true;
            txtMedAmount.Enabled = true;
        }
        else
        {
            for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            {
                gvMedLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            }
          
            //txtMedAmount.Text = "";
            //txtMedSaleUnit.Text = "";
            FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "txtMedAmount", "TxtTotalUnits", "TxtTotalAmount", "chkMedSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }

    protected void chkLowSale_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkLowSale =(CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkLowSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
        TextBox txtLowAmount = (TextBox)gvLowLiqDetails.Rows[RowIndex].FindControl("txtLowAmount");
        TextBox txtLowSaleUnit = (TextBox)gvLowLiqDetails.Rows[RowIndex].FindControl("txtLowSaleUnit");
        if (chkLowSale.Checked == true)
        {
            for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            {
                gvLowLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            }
            txtLowSaleUnit.Focus();
            FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit", "txtLowAmount", "TxtTotalUnits", "TxtTotalAmount", "chkLowSale");
        }
        else
        {
            for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
            {
                gvLowLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            }

            Label LblAvlUnits = (Label)gvLowLiqDetails.Rows[RowIndex].FindControl("LblAvlUnits");
            Label LblValAmnt = (Label)gvLowLiqDetails.Rows[RowIndex].FindControl("LblValAmnt");
            txtLowSaleUnit.Text = LblAvlUnits.Text;
            txtLowAmount.Text = Convert.ToString(LblValAmnt.Text); 
         
            FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit", "txtLowAmount", "TxtTotalUnits", "TxtTotalAmount","chkLowSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }

    protected void chkFinSale_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkFinSale=(CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkFinSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
        TextBox txtFinAmount = (TextBox)gvFinDetails.Rows[RowIndex].FindControl("txtFinAmount");
        TextBox txtFinSaleUnit = (TextBox)gvFinDetails.Rows[RowIndex].FindControl("txtFinSaleUnit");
        if (chkFinSale.Checked == true)
        {
            //for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            //{
            //    gvFinDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            //}
            //txtFinSaleUnit.Focus();

            FunPriAddGrid(gvFinDetails, "txtFinSaleUnit", "txtFinAmount", "TxtTotalUnits", "TxtTotalAmount", "chkFinSale");
            gvFinDetails.Rows[RowIndex].Cells[8].Enabled = true;
        }
        else
        {
            //for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            //{
            //    gvFinDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            //}
          
            //txtFinAmount.Text = "";
            //txtFinSaleUnit.Text = "";
            FunPriAddGrid(gvFinDetails, "txtFinSaleUnit", "txtFinAmount", "TxtTotalUnits", "TxtTotalAmount", "chkFinSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
    }

   protected void chkCommSale_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkCommSale = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkCommSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
        TextBox txtCommAmount = (TextBox)gvCommoDetails.Rows[RowIndex].FindControl("txtCommAmount");
        TextBox txtCommSaleUnit = (TextBox)gvCommoDetails.Rows[RowIndex].FindControl("txtCommSaleUnit");
        if (chkCommSale.Checked == true)
        {
            for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            {
                gvCommoDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            }
            txtCommSaleUnit.Focus();
            FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit", "txtCommAmount", "TxtTotalUnits", "TxtTotalAmount", "chkCommSale");
        }
        else
        {
            for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            {
                gvCommoDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            }

            Label LblAvlUnits = (Label)gvCommoDetails.Rows[RowIndex].FindControl("LblAvlUnits");
            Label LblValAmnt = (Label)gvCommoDetails.Rows[RowIndex].FindControl("LblValAmnt");
            txtCommSaleUnit.Text = LblAvlUnits.Text;
            txtCommAmount.Text = Convert.ToString(LblValAmnt.Text); 
            FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit", "txtCommAmount", "TxtTotalUnits", "TxtTotalAmount", "chkCommSale");
        }
        if (ViewState["STATUS"] != null)
        {
            FunPubSetTab(ViewState["STATUS"].ToString());
        }
      
    }
   public void FunPubSetTab(string Status)
   {
       switch (Status)
       {
           case "1": //Not Yet Approved or Revoked State
               if (tcCollateralSale.ActiveTab == tpColCustAgnt)
               {
                   btnSave.Enabled = false;
               }
               else if (tcCollateralSale.ActiveTab == tpColDtl)
               {
                   btnSave.Enabled = true;
                   //tpColSaleApp.Enabled = false;
               }
               break;
           case "-1": //Query Mode or the Collateral Sale has been finalized with a receipt or rejected state
               btnSave.Enabled = false;
               break;
           case "-2"://Approved and No receipt
               if (tcCollateralSale.ActiveTab == tpSaleInvoice)
               {
                   btnSave.Enabled = true;
               }
               break;
           case "-3"://Approved State
               if (tcCollateralSale.ActiveTab == tpSaleInvoice)
               {
                   btnSave.Enabled = true;
               }
               break;

       }
   }

   protected void gvHighLiqDetails_DataBound(object sender, EventArgs e)
   {
       if (gvHighLiqDetails.DataSource != null)
       {
           TextBox txtHigSaleUnit;
           TextBox txtHigAmount;
           Label LblValAmnt;
           Label LblAvlUnits;
           TextBox TxtTotalUnits = (TextBox)gvHighLiqDetails.FooterRow.FindControl("TxtTotalUnits");
           TextBox TxtTotalAmount = (TextBox)gvHighLiqDetails.FooterRow.FindControl("TxtTotalAmount");

           foreach (GridViewRow gvrow in gvHighLiqDetails.Rows)
           {
               txtHigSaleUnit = (TextBox)gvrow.FindControl("txtHigSaleUnit");
               txtHigAmount = (TextBox)gvrow.FindControl("txtHigAmount");
               LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
               LblAvlUnits = (Label)gvrow.FindControl("LblAvlUnits");
               txtHigAmount.Text = LblValAmnt.Text;
               txtHigSaleUnit.Text = LblAvlUnits.Text.ToString();
               txtHigSaleUnit.SetDecimalPrefixSuffix(8, 4, false,"Hight Sale Unit");
               txtHigAmount.SetDecimalPrefixSuffix(8, 4,false,"Hight Sale Amount");
               txtHigSaleUnit.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["HS_ROW_COUNT"].ToString() + ",'" + gvHighLiqDetails.ClientID + "','txtHigSaleUnit','" + TxtTotalUnits.ClientID + "','chkHigSale');");
               txtHigSaleUnit.Attributes.Add("onblur", "javascript:return CalcSaleAmt('" + txtHigSaleUnit.ClientID + "','" + LblAvlUnits.Text + "','" + gvrow.Cells[6].Text.ToString() + "','" + txtHigAmount.ClientID + "');");
               txtHigAmount.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["HS_ROW_COUNT"].ToString() + ",'" + gvHighLiqDetails.ClientID + "','txtHigAmount','" + TxtTotalAmount.ClientID + "','chkHigSale');");

           }
       }

   }

   protected void gvMedLiqDetails_DataBound(object sender, EventArgs e)
   {
       if (gvMedLiqDetails.DataSource != null)
       {
           TextBox txtMedSaleUnit;
           TextBox txtMedAmount;
           Label LblValAmnt;
           TextBox TxtTotalUnits = (TextBox)gvMedLiqDetails.FooterRow.FindControl("TxtTotalUnits");
           TextBox TxtTotalAmount = (TextBox)gvMedLiqDetails.FooterRow.FindControl("TxtTotalAmount");


           foreach (GridViewRow gvrow in gvMedLiqDetails.Rows)
           {
               txtMedSaleUnit = (TextBox)gvrow.FindControl("txtMedSaleUnit");
               txtMedAmount = (TextBox)gvrow.FindControl("txtMedAmount");
               LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
               txtMedSaleUnit.SetDecimalPrefixSuffix(8, 4, false, "Medium Sale Unit");
               txtMedAmount.SetDecimalPrefixSuffix(8, 4, false, "Medium Sale Amount");
               //txtMedSaleUnit.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["MS_ROW_COUNT"].ToString() + ",'" + gvMedLiqDetails.ClientID + "','txtMedSaleUnit','" + TxtTotalUnits.ClientID + "')");
               //txtMedSaleUnit.Attributes.Add("onblur", "javascript:return CalcSaleAmt(" + txtMedSaleUnit.Text + "," + LblValAmnt.Text + ",'" + txtMedAmount.ClientID + "')");
               txtMedAmount.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["MS_ROW_COUNT"].ToString() + ",'" + gvMedLiqDetails.ClientID + "','txtMedAmount','" + TxtTotalAmount.ClientID + "','chkMedSale');");
               txtMedSaleUnit.Text = "1";
               txtMedAmount.Text = LblValAmnt.Text.ToString();
           }
       }
   }
   protected void gvLowLiqDetails_DataBound(object sender, EventArgs e)
   {
       if (gvLowLiqDetails.DataSource != null)
       {
           TextBox txtLowSaleUnit;
           TextBox txtLowAmount;
           Label LblValAmnt;
           Label LblAvlUnits;
           TextBox TxtTotalUnits = (TextBox)gvLowLiqDetails.FooterRow.FindControl("TxtTotalUnits");
           TextBox TxtTotalAmount = (TextBox)gvLowLiqDetails.FooterRow.FindControl("TxtTotalAmount");


           foreach (GridViewRow gvrow in gvLowLiqDetails.Rows)
           {
               txtLowSaleUnit = (TextBox)gvrow.FindControl("txtLowSaleUnit");
               txtLowAmount = (TextBox)gvrow.FindControl("txtLowAmount");
               LblAvlUnits = (Label)gvrow.FindControl("LblAvlUnits");
               LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
               txtLowAmount.Text = LblValAmnt.Text;
               txtLowSaleUnit.Text = LblAvlUnits.Text.ToString();
               txtLowSaleUnit.SetDecimalPrefixSuffix(8, 4, false,"Low Sale Unit");
               txtLowAmount.SetDecimalPrefixSuffix(8, 4, false, "Low Sale Amount");
               txtLowSaleUnit.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["LS_ROW_COUNT"].ToString() + ",'" + gvLowLiqDetails.ClientID + "','txtLowSaleUnit','" + TxtTotalUnits.ClientID + "','chkLowSale')");
               txtLowSaleUnit.Attributes.Add("onblur", "javascript:return CalcSaleAmt('" + txtLowSaleUnit.ClientID + "','" + LblAvlUnits.Text + "','" + gvrow.Cells[5].Text.ToString() + "','" + txtLowAmount.ClientID + "')");
               txtLowAmount.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["LS_ROW_COUNT"].ToString() + ",'" + gvLowLiqDetails.ClientID + "','txtLowAmount','" + TxtTotalAmount.ClientID + "','chkLowSale')");
           }
       }
   }
   protected void gvCommoDetails_DataBound(object sender, EventArgs e)
   {
       if (gvCommoDetails.DataSource !=null)
       {
           TextBox txtCommSaleUnit;
           TextBox txtCommAmount;
           Label LblValAmnt;
           Label LblAvlUnits;
           TextBox TxtTotalUnits = (TextBox)gvCommoDetails.FooterRow.FindControl("TxtTotalUnits");
           TextBox TxtTotalAmount = (TextBox)gvCommoDetails.FooterRow.FindControl("TxtTotalAmount");

           foreach (GridViewRow gvrow in gvCommoDetails.Rows)
           {
               txtCommSaleUnit = (TextBox)gvrow.FindControl("txtCommSaleUnit");
               txtCommAmount = (TextBox)gvrow.FindControl("txtCommAmount");
               LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
               LblAvlUnits = (Label)gvrow.FindControl("LblAvlUnits");
               txtCommAmount.Text = LblValAmnt.Text;
               txtCommSaleUnit.Text = LblAvlUnits.Text.ToString();
               txtCommSaleUnit.SetDecimalPrefixSuffix(8, 4, false, "Commodities Sale Unit");
               txtCommAmount.SetDecimalPrefixSuffix(8, 4, false, "Commodities Sale Amount");
               txtCommSaleUnit.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["COM_ROW_COUNT"].ToString() + ",'" + gvCommoDetails.ClientID + "','txtCommSaleUnit','" + TxtTotalUnits.ClientID + "','chkCommSale')");
               txtCommSaleUnit.Attributes.Add("onblur", "javascript:return CalcSaleAmt('" + txtCommSaleUnit.ClientID + "','" + LblAvlUnits.Text + "','" + gvrow.Cells[6].Text.ToString() + "','" + txtCommAmount.ClientID + "')");
               txtCommAmount.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["COM_ROW_COUNT"].ToString() + ",'" + gvCommoDetails.ClientID + "','txtCommAmount','" + TxtTotalAmount.ClientID + "','chkCommSale')");
           }
       }

   }
   protected void gvFinDetails_DataBound(object sender, EventArgs e)
   {
       if (gvFinDetails.DataSource != null)
       {
           TextBox txtFinSaleUnit;
           TextBox txtFinAmount;
           Label LblValAmnt;
           TextBox TxtTotalUnits = (TextBox)gvFinDetails.FooterRow.FindControl("TxtTotalUnits");
           TextBox TxtTotalAmount = (TextBox)gvFinDetails.FooterRow.FindControl("TxtTotalAmount");
           foreach (GridViewRow gvrow in gvFinDetails.Rows)
           {
               txtFinSaleUnit = (TextBox)gvrow.FindControl("txtFinSaleUnit");
               txtFinAmount = (TextBox)gvrow.FindControl("txtFinAmount");
               LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
               txtFinSaleUnit.SetDecimalPrefixSuffix(8, 4, false, "Finacial Services Sale Unit");
               txtFinAmount.SetDecimalPrefixSuffix(8, 4, false, "Financial Services Sale Amount");
               //txtFinSaleUnit.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["FS_ROW_COUNT"].ToString() + ",'" + gvFinDetails.ClientID + "','txtFinSaleUnit','" + TxtTotalUnits.ClientID + "')");
               //txtFinSaleUnit.Attributes.Add("onblur", "javascript:return CalcSaleAmt(" + txtFinSaleUnit.Text + "," + LblValAmnt.Text + ",'"+ txtFinAmount.ClientID +"')");
               txtFinAmount.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["FS_ROW_COUNT"].ToString() + ",'" + gvFinDetails.ClientID + "','txtFinAmount','" + TxtTotalAmount.ClientID + "','chkFinSale');");
               txtFinSaleUnit.Text = "1";
               txtFinAmount.Text = LblValAmnt.Text;

           }
       }
   }
   protected void btnTab_Click(object sender, EventArgs e)
   {
       if (Request.QueryString["qsMode"] != null)
           strMode = Request.QueryString["qsMode"];
       if (strMode == "C")
       {
           if (tcCollateralSale.ActiveTab == tpColCustAgnt)
           {
               btnClear.Enabled = false;
           }
           else
           {
               btnClear.Enabled = true;
           }
           if (tcCollateralSale.ActiveTabIndex == 2)
           {
               btnSave.Enabled = true;
           }
           else
           {
               btnSave.Enabled = false;
           }
       }
       else if (strMode == "M")
       {
           btnSave.Enabled = false;
           if (tcCollateralSale.ActiveTab == tpRevoke)
           {
               btnSave.Enabled = true;
               tpSaleInvoice.Enabled = false;
               TxtRvkPwd.Focus();
           }
           else if (tcCollateralSale.ActiveTab == tpSaleInvoice)
           {
               FunPubSetTab(ViewState["STATUS"].ToString());
               
           }
       }

   }

   protected void txtHigAmount_TextChanged(object sender, EventArgs e)
   {
       FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit", "txtHigAmount", "TxtTotalUnits", "TxtTotalAmount", "chkHigSale");
       if (ViewState["STATUS"] != null)
       {
           FunPubSetTab(ViewState["STATUS"].ToString());
       }
   }
   protected void txtMedAmount_TextChanged(object sender, EventArgs e)
   {
       FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "txtMedAmount", "TxtTotalUnits", "TxtTotalAmount", "chkMedSale");
       if (ViewState["STATUS"] != null)
       {
           FunPubSetTab(ViewState["STATUS"].ToString());
       }
   }
   protected void txtLowSaleUnit_TextChanged(object sender, EventArgs e)
   {
       FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit", "txtLowAmount", "TxtTotalUnits", "TxtTotalAmount", "chkLowSale");
       if (ViewState["STATUS"] != null)
       {
           FunPubSetTab(ViewState["STATUS"].ToString());
       }
   }
   protected void txtCommSaleUnit_TextChanged(object sender, EventArgs e)
   {
       FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit", "txtCommAmount", "TxtTotalUnits", "TxtTotalAmount", "chkCommSale");
       if (ViewState["STATUS"] != null)
       {
           FunPubSetTab(ViewState["STATUS"].ToString());
       }
   }
   protected void txtCommAmount_TextChanged(object sender, EventArgs e)
   {
       FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit", "txtCommAmount", "TxtTotalUnits", "TxtTotalAmount", "chkCommSale");
       if (ViewState["STATUS"] != null)
       {
           FunPubSetTab(ViewState["STATUS"].ToString());
       }
   }
   protected void txtFinSaleUnit_TextChanged(object sender, EventArgs e)
   {
       FunPriAddGrid(gvFinDetails, "txtFinSaleUnit", "txtFinAmount", "TxtTotalUnits", "TxtTotalAmount", "chkFinSale");
       if (ViewState["STATUS"] != null)
       {
           FunPubSetTab(ViewState["STATUS"].ToString());
       }
   }
   protected void txtMedSaleUnit_TextChanged(object sender, EventArgs e)
   {
       FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "txtMedAmount", "TxtTotalUnits", "TxtTotalAmount", "chkMedSale");
       if (ViewState["STATUS"] != null)
       {
           FunPubSetTab(ViewState["STATUS"].ToString());
       }
   }
   protected void txtLowAmount_TextChanged(object sender, EventArgs e)
   {
       FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit", "txtLowAmount", "TxtTotalUnits", "TxtTotalAmount", "chkLowSale");
       if (ViewState["STATUS"] != null)
       {
           FunPubSetTab(ViewState["STATUS"].ToString());
       }
   }
}
