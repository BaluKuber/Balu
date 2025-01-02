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
using LEGALSERVICES = LegalAndRepossessionMgtServicesReference;
using System.ServiceModel;


public partial class Collateral_S3GCLTCollateralRelease : ApplyThemeForProject
{
    #region [Common Variable declaration]
    int intCompanyID, intSaleNo, intUserID = 0;
    UserInfo ObjUserInfo = new UserInfo();
    Dictionary<string, string> Procparam = null;
    string strRedirectPage = "~/Collateral/S3GCLTTransLander.aspx?Code=KRE";
    string strRedirectPageView = "window.location.href='../Collateral/S3GCLTTransLander.aspx?Code=KRE';";
    string strRedirectPageAdd = "window.location.href='../Collateral/S3GCLTCollateralRelease.aspx';";
    string strRedirectPageViewTrans = "../Collateral/S3GCLTTransLander.aspx?Code=KRE";
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
    
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralReleaseDataTable objCollateralReleaseDataTable;
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralReleaseRow objCollateralReleaseDataRow;
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
                Utility.FunShowAlertMsg(this.Page, " A Collateral Release is already under process for this Collateral Transaction Number. Modify the Collateral data in Modify mode to proceed!");
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
        

    }
    void SetTabIndexChanged()
    {
        if (Request.QueryString["qsMode"] == "M" || Request.QueryString["qsMode"] == "Q")
        {
            tpColGeneral.Enabled = tpColDtl.Enabled =  true;
            tpColCustAgnt.Enabled = false;
        }
        else
        {

            if (tcCollateralSale.ActiveTab == tpColCustAgnt)
            {
                tpColGeneral.Enabled = tpColDtl.Enabled =  false;

            }
            else
            {
                tpColGeneral.Enabled = tpColDtl.Enabled = true;
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion
   
  

  
    
  
    
    public void FunPubProInsertCollateralSale(StringBuilder strColSaleDetails)
    {
        objCollateralMgtClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {
            string ColSaleNo;
            int ErrorCode = 0;
            objCollateralReleaseDataTable = new S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralReleaseDataTable();
            objCollateralReleaseDataRow = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralReleaseRow)objCollateralReleaseDataTable.NewRow();
            objCollateralReleaseDataRow.Collateral_Capture_ID = Convert.ToInt32(ViewState["COLLATERAL_CAPTURE_ID"].ToString());
            objCollateralReleaseDataRow.Collateral_Release_Date = Utility.StringToDate(txtCollateralSaleDate.Text.ToString());
            objCollateralReleaseDataRow.Collateral_Tran_No = ViewState["COLTRAN_NO"].ToString();
            objCollateralReleaseDataRow.Company_ID = intCompanyID;
            objCollateralReleaseDataRow.Created_By = intUserID;
            objCollateralReleaseDataRow.Created_On = DateTime.Now;
            objCollateralReleaseDataRow.XMLRELEASEDETAILS = strColSaleDetails.ToString();
            objCollateralReleaseDataTable.Rows.Add(objCollateralReleaseDataRow);
            if (objCollateralReleaseDataTable.Rows.Count > 0)
            {

                ErrorCode = objCollateralMgtClient.FunPubInsertCollateralRelease(out ColSaleNo, ClsPubSerialize.Serialize(objCollateralReleaseDataTable, SerializationMode.Binary), SerializationMode.Binary);
                switch (ErrorCode)
                {
                    case 0:
                        txtCollateralSaleNo.Text = ColSaleNo;

                        

                        if (intSaleNo > 0)
                        {
                            //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = strAlert.Replace("__ALERT__", "Collateral Release detail have been updated sucessfully");
                        }
                        else
                        {
                            //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = "Collateral Release Number " + ColSaleNo + " added successfully";
                            strAlert += @"\n\nWould you like to add one more Collateral Release detail?";
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
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Collateral Release details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Collateral Release details");
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
            //if (objCollateralMgtClient ==null)
            {
                objCollateralMgtClient.Close();
            }
        }
    }
    public void FunPubProUpdateCollateralSale(StringBuilder strColSaleDetails)
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
            intSaleNo = Convert.ToInt32(ViewState["COLLATERAL_RELEASE_ID"]);
            string ColSaleNo;
            int ErrorCode = 0;
            objCollateralReleaseDataTable = new S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralReleaseDataTable();
            objCollateralReleaseDataRow = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralReleaseRow)objCollateralReleaseDataTable.NewRow();
            objCollateralReleaseDataRow.Collateral_Capture_ID = Convert.ToInt32(ViewState["COLLATERAL_CAPTURE_ID"].ToString());
            objCollateralReleaseDataRow.Collateral_Release_Date = Utility.StringToDate(txtCollateralSaleDate.Text.ToString());
            objCollateralReleaseDataRow.Collateral_Tran_No = ViewState["COLTRAN_NO"].ToString();
            objCollateralReleaseDataRow.Company_ID = intCompanyID;
            objCollateralReleaseDataRow.Created_By = intUserID;
            objCollateralReleaseDataRow.Modified_By = intUserID;
            objCollateralReleaseDataRow.Created_On = DateTime.Now;
            objCollateralReleaseDataRow.XMLRELEASEDETAILS = strColSaleDetails.ToString();
            objCollateralReleaseDataTable.Rows.Add(objCollateralReleaseDataRow);
            if (objCollateralReleaseDataTable.Rows.Count > 0)
            {

                ErrorCode = objCollateralMgtClient.FunPubUpdateCollateralRelease(ClsPubSerialize.Serialize(objCollateralReleaseDataTable, SerializationMode.Binary), SerializationMode.Binary, Convert.ToInt32(ViewState["COLLATERAL_RELEASE_ID"]));
                switch (ErrorCode)
                {
                    case 0:
                        //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        if (intSaleNo > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Collateral Release detail have been updated sucessfully");
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
                            strAlert = strAlert.Replace("__ALERT__", "Error in updating Collateral Release details");
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding Collateral Release details");
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
        FunCollateralDetails(intCompanyID, Convert.ToInt32(custID), Convert.ToInt32(strCLT_CaptureID));
    }

    public void FunCollateralDetails(int CompanyID, int CustomerID, int Capture_ID)
    {
        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];
        if (strMode == "M" || strMode == "Q")
        {
           
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", CompanyID.ToString());
            Procparam.Add("@CUSTOMER_ID", CustomerID.ToString());
            Procparam.Add("@CAPTURE_ID", Capture_ID.ToString());
            DataSet DsCollateralDetails = new DataSet();
            DsCollateralDetails = Utility.GetDataset("S3G_CLT_GetColrELEASEItemsRefMod", Procparam);

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
                strAlert = strAlert.Replace("__ALERT__", "All the Collaterals for this Customer have been exhausted. Hence, you would  not be able to proceed with the Release! ");
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

            DsCollateralDetails = Utility.GetDataset("S3G_CLT_GetColrELEASEItemsRefNumbers", Procparam);
            
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
                strAlert = strAlert.Replace("__ALERT__", "All the Collaterals for this Customer have been exhausted or there are no units to sell. Hence, you would  not be able to proceed with the Release! ");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                ViewState["EXHAUST"] = true;
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
                strColValue = grvRow.Cells[intcolcount].Text;
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
                                Utility.FunShowAlertMsg(this, "Selecet Atleast a Colateral Record");
                                return;
                            }

                        }
                    }

                }
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
                            FunPriColSaleControlStatus(1);
                       
                    }

                    if (strMode == "Q")
                    {
                       
                        FunPriColSaleControlStatus(-1);
                    }
                }
                else
                {
                   
                    FunPriColSaleControlStatus(0);
                }
                  
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjUserInfo = null;

        }
    }
    

    private void FunPubProGetSaleNoDetails(int CompanyID, int intSaleNo)
    {
        objCollateralMgtClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {

            DataSet DsColSaledetails;
            byte[] byteDsColSaledetails;
            byteDsColSaledetails = objCollateralMgtClient.FunPubGetColReleaseDetails(intSaleNo, CompanyID);
            DsColSaledetails = (DataSet)ClsPubSerialize.DeSerialize(byteDsColSaledetails, SerializationMode.Binary, typeof(DataSet));
            DataTable DtCustSaleDetails = DsColSaledetails.Tables[0];
            DataTable DtItemDetails = DsColSaledetails.Tables[1];

            //Filling Collateral Sale Header details
            ViewState["LOB_ID"] = DtCustSaleDetails.Rows[0]["LOB_ID"].ToString();
            ViewState["Location_ID"] = DtCustSaleDetails.Rows[0]["Location_ID"].ToString();
            ViewState["CUST_ID"] = DtCustSaleDetails.Rows[0]["CUSTOMER_ID"].ToString();
            ViewState["COLLATERAL_RELEASE_ID"] = DtCustSaleDetails.Rows[0]["COLLATERAL_RELEASE_ID"].ToString();
            //ViewState["APPROVAL_ID"] = DtCustSaleDetails.Rows[0]["APPROVAL_ID"].ToString();
            ViewState["COLLATERAL_RELEASE_NO"] = DtCustSaleDetails.Rows[0]["COLLATERAL_RELEASE_NO"].ToString();
            ViewState["COLLATERAL_RELEASE_DATE"] = DtCustSaleDetails.Rows[0]["COLLATERAL_RELEASE_DATE"].ToString();
            ViewState["COLLATERAL_CAPTURE_ID"] = DtCustSaleDetails.Rows[0]["COLLATERAL_CAPTURE_ID"].ToString();
            ViewState["COLTRAN_NO"] = DtCustSaleDetails.Rows[0]["COLLATERAL_TRAN_NO"].ToString();
            TxtColTranNo.Text = ViewState["COLTRAN_NO"].ToString();
            txtCollateralSaleDate.Text = Convert.ToDateTime(ViewState["COLLATERAL_RELEASE_DATE"].ToString()).ToString(ObjS3GSession.ProDateFormatRW);
            txtCollateralSaleNo.Text = ViewState["COLLATERAL_RELEASE_NO"].ToString();
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
                        
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrCom.Enabled = true;
                            chkCommSale.Checked = true;
                            txtCommSaleUnit.Text = dr["RELEASE_UNIT"].ToString();
                            

                        }
                    }
                }
                if (gvCommoDetails.FooterRow != null)
                {
                    TextBox TxtTotalUnits = (TextBox)gvCommoDetails.FooterRow.FindControl("TxtTotalUnits");
                    TxtTotalUnits.Text = DsColSaledetails.Tables[10].Rows[0]["CS_TOTAL_RELEASE_UNIT"].ToString();
                }
                foreach (GridViewRow gvrCom in gvCommoDetails.Rows)
                {
                    CheckBox chkCommSale = (CheckBox)gvrCom.FindControl("chkCommSale");
                    if (chkCommSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                        {
                            gvrCom.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrCom.Cells[9].Enabled = true;
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
                        
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrFin.Enabled = true;
                            chkFinSale.Checked = true;
                            txtFinSaleUnit.Text = dr["RELEASE_UNIT"].ToString();
                            
                        }

                    }
                }
                if (gvFinDetails.FooterRow != null)
                {
                    TextBox TxtTotalUnits = (TextBox)gvFinDetails.FooterRow.FindControl("TxtTotalUnits");
                    TxtTotalUnits.Text = DsColSaledetails.Tables[9].Rows[0]["FS_TOTAL_RELEASE_UNIT"].ToString();
                }
                
                foreach (GridViewRow gvrFin in gvFinDetails.Rows)
                {
                    CheckBox chkFinSale = (CheckBox)gvrFin.FindControl("chkFinSale");
                    if (chkFinSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 8; ColumnIndex++)
                        {
                            gvrFin.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrFin.Cells[8].Enabled = true;
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
                            for (int ColumnIndex = 0; ColumnIndex < 8; ColumnIndex++)
                            {
                                gvrFin.Cells[ColumnIndex].Enabled = false;
                            }
                            gvrFin.Cells[8].Enabled = true;
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
                        
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrHS.Enabled = true;
                            chkHigSale.Checked = true;
                            txtHigSaleUnit.Text = dr["RELEASE_UNIT"].ToString();
                            

                        }
                    }
                }
                if (gvHighLiqDetails.FooterRow != null)
                {
                    TextBox TxtTotalUnits = (TextBox)gvHighLiqDetails.FooterRow.FindControl("TxtTotalUnits");
                    TxtTotalUnits.Text = DsColSaledetails.Tables[6].Rows[0]["HS_TOTAL_RELEASE_UNIT"].ToString();
                }
                
                foreach (GridViewRow gvrHS in gvHighLiqDetails.Rows)
                {
                    CheckBox chkHigSale = (CheckBox)gvrHS.FindControl("chkHigSale");
                    if (chkHigSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                        {
                            gvrHS.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrHS.Cells[9].Enabled = true;
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
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrLow.Enabled = true;
                            chkLowSale.Checked = true;
                            txtLowSaleUnit.Text = dr["RELEASE_UNIT"].ToString();
                        }
                    }
                }
                if (gvLowLiqDetails.FooterRow != null)
                {
                    TextBox TxtTotalUnits = (TextBox)gvLowLiqDetails.FooterRow.FindControl("TxtTotalUnits");
                    TxtTotalUnits.Text = DsColSaledetails.Tables[8].Rows[0]["LS_TOTAL_RELEASE_UNIT"].ToString();
                }
                foreach (GridViewRow gvrLow in gvLowLiqDetails.Rows)
                {
                    CheckBox chkLowSale = (CheckBox)gvrLow.FindControl("chkLowSale");
                    if (chkLowSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 8; ColumnIndex++)
                        {
                            gvrLow.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrLow.Cells[8].Enabled = true;
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
                        
                        if (LblItemRefNo.Text == dr["COLLATERAL_ITEM_REF_NO"].ToString())
                        {
                            gvrMed.Enabled = true;
                            chkMedSale.Checked = true;
                            txtMedSaleUnit.Text = dr["RELEASE_UNIT"].ToString();
                            

                        }
                    }
                }
                if (gvMedLiqDetails.FooterRow != null)
                {
                    TextBox TxtTotalUnits = (TextBox)gvMedLiqDetails.FooterRow.FindControl("TxtTotalUnits");
                    TxtTotalUnits.Text = DsColSaledetails.Tables[7].Rows[0]["MS_TOTAL_RELEASE_UNIT"].ToString();
                }
                foreach (GridViewRow gvrMed in gvMedLiqDetails.Rows)
                {
                    CheckBox chkMedSale = (CheckBox)gvrMed.FindControl("chkMedSale");
                    if (chkMedSale.Checked == false)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
                        {
                            gvrMed.Cells[ColumnIndex].Enabled = false;
                        }
                        if (strMode == "M")
                        {
                            gvrMed.Enabled = false;
                            gvrMed.Cells[10].Enabled = true;
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
                            for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
                            {
                                gvrMed.Cells[ColumnIndex].Enabled = false;
                            }
                            gvrMed.Cells[10].Enabled = true;
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

            
            ViewState["TOTAL_UNITS"] = DsColSaledetails.Tables[11].Rows[0]["TOTAL_RELEASE_UNITS"].ToString();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objCollateralMgtClient.Close();
        }
    }


    private void FunPriAddGrid(GridView Grid, string ValFld,string FtValFls,string Chk)
    {
        decimal SaleUnit = 0, SaleAmount = 0;
        foreach (GridViewRow Row in Grid.Rows)
        {
            CheckBox ChkRec = (CheckBox)Row.FindControl(Chk);
            TextBox txtUnits = (TextBox)Row.FindControl(ValFld);
            
            if (ChkRec.Checked == true)
            {
                SaleUnit += Convert.ToDecimal(ChkEmpty(txtUnits.Text.ToString()));
                
            }
        }
        if (Grid.Rows.Count != 0)
        {
            TextBox TxtTotalUnits = (TextBox)Grid.FooterRow.FindControl(FtValFls);
            
            TxtTotalUnits.SetDecimalPrefixSuffix(10, 2, false);
            
            TxtTotalUnits.Text = SaleUnit.ToString();
           
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
                tpColCustAgnt.Visible = false;
                ChkCOM.Enabled = false;
                ChkFS.Enabled = false;
                ChkHS.Enabled = false;
                ChkLS.Enabled = false;
                ChkMS.Enabled = false;
                //foreach (GridViewRow gvr in gvCommoDetails.Rows)
                //{
                //    CheckBox ChkCommSale = (CheckBox)gvr.FindControl("chkCommSale");
                //    chkCommSale_CheckedChanged(ChkCommSale, new EventArgs());
                //}
                //foreach (GridViewRow gvr in gvHighLiqDetails.Rows)
                //{
                //    CheckBox ChkHighSale = (CheckBox)gvr.FindControl("chkHigSale");
                //    chkHigSale_CheckedChanged(ChkHighSale, new EventArgs());
                //}
                //foreach (GridViewRow gvr in gvLowLiqDetails.Rows)
                //{
                //    CheckBox ChkLowSale = (CheckBox)gvr.FindControl("chkLowSale");
                //    chkLowSale_CheckedChanged(ChkLowSale, new EventArgs());
                //}
                //foreach (GridViewRow gvr in gvMedLiqDetails.Rows)
                //{
                //    CheckBox chkMedSale = (CheckBox)gvr.FindControl("chkMedSale");
                //    chkMedSale_CheckedChanged(chkMedSale, new EventArgs());
                //}
                //foreach (GridViewRow gvr in gvFinDetails.Rows)
                //{
                //    CheckBox chkFinSale = (CheckBox)gvr.FindControl("chkFinSale");
                //    chkFinSale_CheckedChanged(chkFinSale, new EventArgs());
                //}

                btnClear.Enabled = false;
                txtCollateralSaleDate.Enabled = false;
                txtCollateralSaleNo.Enabled = false;
                gvCommoDetails.Enabled = false;
                gvFinDetails.Enabled = false;
                gvHighLiqDetails.Enabled = false;
                gvLowLiqDetails.Enabled = false;
                gvMedLiqDetails.Enabled = false;
                

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
                
                
                btnClear.Enabled = false;
                //btnGo.Enabled = false;
               
                btnSave.Enabled = false;
               
                txthiddenfield.Text = "Q";
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
        FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit", "TxtTotalUnits", "chkHigSale");
        
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
        

        if ((ChkCOM.Visible == true) || (ChkFS.Visible == true) || (ChkHS.Visible == true) || (ChkLS.Visible == true) || (ChkMS.Visible == true))
        {
            if ((!ChkCOM.Checked) && (!ChkFS.Checked) && (!ChkHS.Checked) && (!ChkLS.Checked) && (!ChkMS.Checked))
            {
                Utility.FunShowAlertMsg(this.Page, "Select a Commodity type to make a Release!");
                return;
            }
        }

        bool ItemChecked = false;
        strColSaleDetails.Append("<Root>");
        if (ChkHS.Checked)
        {
            foreach (GridViewRow gvr in gvHighLiqDetails.Rows)
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
                Utility.FunShowAlertMsg(this.Page, "Select an High Liquidity Asset to make a Release!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvHighLiqDetails.Rows)
                {
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtHigSaleUnit = (TextBox)gvr.FindControl("txtHigSaleUnit");
                    
                    CheckBox chkHigSale = (CheckBox)gvr.FindControl("chkHigSale");
                    if ((chkHigSale.Checked == true) && (chkHigSale.Enabled == true))
                    {
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" Release_Unit = '" + Convert.ToString(txtHigSaleUnit.Text) + "'/>");
                           
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
                Utility.FunShowAlertMsg(this.Page, "Select an Medium Liquidity Asset to make a Release!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvMedLiqDetails.Rows)
                {
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtMedSaleUnit = (TextBox)gvr.FindControl("txtMedSaleUnit");
                    
                    CheckBox chkMedSale = (CheckBox)gvr.FindControl("chkMedSale");
                    if ((chkMedSale.Checked == true) && (chkMedSale.Enabled == true))
                    {
                       
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" Release_Unit = '" + Convert.ToString(txtMedSaleUnit.Text) + "'/>");
                            
                       
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
                Utility.FunShowAlertMsg(this.Page, "Select an Low Liquidity Asset to make a Release!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvLowLiqDetails.Rows)
                {
                    CheckBox chkLowSale = (CheckBox)gvr.FindControl("chkLowSale");
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtLowSaleUnit = (TextBox)gvr.FindControl("txtLowSaleUnit");
                    
                    if ((chkLowSale.Checked == true) && (chkLowSale.Enabled == true))
                    {
                        
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" Release_Unit = '" + Convert.ToString(txtLowSaleUnit.Text) + "'/>");
                            
                       
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
                Utility.FunShowAlertMsg(this.Page, "Select an Commodities Asset to make a Release!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvCommoDetails.Rows)
                {
                    CheckBox chkCommSale = (CheckBox)gvr.FindControl("chkCommSale");
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtCommSaleUnit = (TextBox)gvr.FindControl("txtCommSaleUnit");
                   
                    if ((chkCommSale.Checked == true) && (chkCommSale.Enabled == true))
                    {
                        
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" Release_Unit = '" + Convert.ToString(txtCommSaleUnit.Text) + "'/>");
                            
                       
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
                Utility.FunShowAlertMsg(this.Page, "Select an Financial Securities Asset to make a Release!");
                return;
            }
            else
            {

                foreach (GridViewRow gvr in gvFinDetails.Rows)
                {
                    CheckBox chkFinSale = (CheckBox)gvr.FindControl("chkFinSale");
                    Label LblItemRefNo = (Label)gvr.FindControl("LblItemRefNo");
                    TextBox txtFinSaleUnit = (TextBox)gvr.FindControl("txtFinSaleUnit");
                    
                    if ((chkFinSale.Checked == true) && (chkFinSale.Enabled == true))
                    {
                       
                            strColSaleDetails.Append(" <Details ITEM_REF_ID='" + Convert.ToString(LblItemRefNo.Text) + "'");
                            strColSaleDetails.Append(" Release_Unit = '" + Convert.ToString(txtFinSaleUnit.Text) + "'/>");
                            
                       
                    }
                }

            }
        }
        strColSaleDetails.Append("</Root>");

        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];

        if (strMode == "C")
        {
            FunPubProInsertCollateralSale(strColSaleDetails);
        }
        else if (strMode == "M")
        {
          
                FunPubProUpdateCollateralSale(strColSaleDetails);
         
            
        }
    }

    

    #region Checkbox Events

    protected void ChkHS_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkHS.Checked)
        {
            gvHighLiqDetails.Enabled = false;

            FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit","TxtTotalUnits","chkHigSale");
        }
        else
        {
            gvHighLiqDetails.Enabled = true;
            foreach (GridViewRow gvr in gvHighLiqDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[9].Enabled = true;
                    if (((CheckBox)gvr.FindControl("chkHigSale")).Checked == true)
                    {
                        gvr.Cells[8].Enabled = true;
                       // gvr.Cells[9].Enabled = true;
                    }
                }
                else
                {
                    gvr.Enabled = false;
                }
            }

            FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit","TxtTotalUnits","chkHigSale");
        }
        
    }
    protected void ChkMS_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkMS.Checked)
        {
            gvMedLiqDetails.Enabled = false;
            FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit","TxtTotalUnits","chkMedSale");
        }
        else
        {
            gvMedLiqDetails.Enabled = true;
            foreach (GridViewRow gvr in gvMedLiqDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[10].Enabled = true;

                }
                else
                {
                    gvr.Enabled = false;
                }
            }
        }
        FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit","TxtTotalUnits","chkMedSale");
        
    }

    protected void ChkLS_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkLS.Checked)
        {
            gvLowLiqDetails.Enabled = false;
            FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit","TxtTotalUnits","chkLowSale");
        }
        else
        {
            gvLowLiqDetails.Enabled = true;
            foreach (GridViewRow gvr in gvLowLiqDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 8; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[8].Enabled = true;
                    if (((CheckBox)gvr.FindControl("chkLowSale")).Checked == true)
                    {
                        gvr.Cells[7].Enabled = true;
                        //gvr.Cells[8].Enabled = true;
                    }
                }
                else
                {
                    gvr.Enabled = false;
                }

            }
            FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit","TxtTotalUnits","chkLowSale");
        }
       
    }
    protected void ChkCOM_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkCOM.Checked)
        {
            gvCommoDetails.Enabled = false;
            FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit","TxtTotalUnits","chkCommSale");
        }
        else
        {
            gvCommoDetails.Enabled = true;
            foreach (GridViewRow gvr in gvCommoDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[9].Enabled = true;
                    if (((CheckBox)gvr.FindControl("chkCommSale")).Checked == true)
                    {
                        gvr.Cells[8].Enabled = true;
                        //gvr.Cells[9].Enabled = true;
                    }
                }
                else
                {
                    gvr.Enabled = false;
                }
            }
            FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit","TxtTotalUnits","chkCommSale");
        }
      
    }
    protected void ChkFS_CheckedChanged(object sender, EventArgs e)
    {
        if (!ChkFS.Checked)
        {
            gvFinDetails.Enabled = false;
            FunPriAddGrid(gvFinDetails, "txtFinSaleUnit","TxtTotalUnits","chkFinSale");
        }
        else
        {
            gvFinDetails.Enabled = true;
            foreach (GridViewRow gvr in gvFinDetails.Rows)
            {
                Label LblIsSold = (Label)gvr.FindControl("LblIsSold");
                if (LblIsSold.Text == "False")
                {
                    for (int ColumnIndex = 0; ColumnIndex < 8; ColumnIndex++)
                    {
                        gvr.Cells[ColumnIndex].Enabled = false;
                    }
                    gvr.Cells[8].Enabled = true;
                }
                else
                {
                    gvr.Enabled = false;
                }
            }
            FunPriAddGrid(gvFinDetails, "txtFinSaleUnit","TxtTotalUnits","chkFinSale");
        }
       
    }
    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
       
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
        GridViewRow gvr = (GridViewRow)chkHigSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
        
        TextBox txtHigSaleUnit = (TextBox)gvHighLiqDetails.Rows[RowIndex].FindControl("txtHigSaleUnit");
        if (chkHigSale.Checked == true)
        {
            for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
            {
                gvHighLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            }
            txtHigSaleUnit.Focus();
            FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit","TxtTotalUnits","chkHigSale");
        }
        else
        {
            for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
            {
                gvHighLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            }

            Label LblAvlUnits = (Label)gvHighLiqDetails.Rows[RowIndex].FindControl("LblAvlUnits");
            Label LblValAmnt = (Label)gvHighLiqDetails.Rows[RowIndex].FindControl("LblValAmnt");
            txtHigSaleUnit.Text = LblAvlUnits.Text;
            
            FunPriAddGrid(gvHighLiqDetails, "txtHigSaleUnit","TxtTotalUnits","chkHigSale");
        }
        
    }

    protected void chkMedSale_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkMedSale = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkMedSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
        
        TextBox txtMedSaleUnit = (TextBox)gvMedLiqDetails.Rows[RowIndex].FindControl("txtMedSaleUnit");
        if (chkMedSale.Checked == true)
        {
            //for (int ColumnIndex = 0; ColumnIndex < 11; ColumnIndex++)
            //{
            //    gvMedLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            //}
            //txtMedSaleUnit.Focus();
            FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "TxtTotalUnits", "chkMedSale");
        }
        else
        {
            for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            {
                gvMedLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            }

            //txtMedAmount.Text = "";
            //txtMedSaleUnit.Text = "";
            FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "TxtTotalUnits", "chkMedSale");
        }
       
    }

    protected void chkLowSale_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkLowSale = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkLowSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
        
        TextBox txtLowSaleUnit = (TextBox)gvLowLiqDetails.Rows[RowIndex].FindControl("txtLowSaleUnit");
        if (chkLowSale.Checked == true)
        {
            for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
            {
                gvLowLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            }
            txtLowSaleUnit.Focus();
            FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit","TxtTotalUnits","chkLowSale");
        }
        else
        {
            for (int ColumnIndex = 0; ColumnIndex < 8; ColumnIndex++)
            {
                gvLowLiqDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            }

            Label LblAvlUnits = (Label)gvLowLiqDetails.Rows[RowIndex].FindControl("LblAvlUnits");
            Label LblValAmnt = (Label)gvLowLiqDetails.Rows[RowIndex].FindControl("LblValAmnt");
            txtLowSaleUnit.Text = LblAvlUnits.Text;
            

            FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit","TxtTotalUnits","chkLowSale");
        }
        
    }

    protected void chkFinSale_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkFinSale = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkFinSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
       
        TextBox txtFinSaleUnit = (TextBox)gvFinDetails.Rows[RowIndex].FindControl("txtFinSaleUnit");
        if (chkFinSale.Checked == true)
        {
            //for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            //{
            //    gvFinDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            //}
            //txtFinSaleUnit.Focus();

            FunPriAddGrid(gvFinDetails, "txtFinSaleUnit", "TxtTotalUnits", "chkFinSale");
        }
        else
        {
            //for (int ColumnIndex = 0; ColumnIndex < 10; ColumnIndex++)
            //{
            //    gvFinDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            //}

            //txtFinAmount.Text = "";
            //txtFinSaleUnit.Text = "";
            FunPriAddGrid(gvFinDetails, "txtFinSaleUnit", "TxtTotalUnits","chkFinSale");
        }
       
    }

    protected void chkCommSale_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkCommSale = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkCommSale.NamingContainer;
        int RowIndex = gvr.RowIndex;
        
        TextBox txtCommSaleUnit = (TextBox)gvCommoDetails.Rows[RowIndex].FindControl("txtCommSaleUnit");
        if (chkCommSale.Checked == true)
        {
            for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
            {
                gvCommoDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = true;
            }
            txtCommSaleUnit.Focus();
            FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit","TxtTotalUnits","chkCommSale");
        }
        else
        {
            for (int ColumnIndex = 0; ColumnIndex < 9; ColumnIndex++)
            {
                gvCommoDetails.Rows[RowIndex].Cells[ColumnIndex].Enabled = false;
            }

            Label LblAvlUnits = (Label)gvCommoDetails.Rows[RowIndex].FindControl("LblAvlUnits");
            Label LblValAmnt = (Label)gvCommoDetails.Rows[RowIndex].FindControl("LblValAmnt");
            txtCommSaleUnit.Text = LblAvlUnits.Text;
           
            FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit","TxtTotalUnits","chkCommSale");
        }
        

    }    
    

    protected void gvHighLiqDetails_DataBound(object sender, EventArgs e)
    {
        if (gvHighLiqDetails.DataSource != null)
        {
            TextBox txtHigSaleUnit;
           
            Label LblValAmnt;
            Label LblAvlUnits;
            TextBox TxtTotalUnits = (TextBox)gvHighLiqDetails.FooterRow.FindControl("TxtTotalUnits");
           

            foreach (GridViewRow gvrow in gvHighLiqDetails.Rows)
            {
                txtHigSaleUnit = (TextBox)gvrow.FindControl("txtHigSaleUnit");
                
                LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
                LblAvlUnits = (Label)gvrow.FindControl("LblAvlUnits");
               
                txtHigSaleUnit.Text = LblAvlUnits.Text.ToString();
                txtHigSaleUnit.SetDecimalPrefixSuffix(8, 4, false, "Hight Release Unit");
                
                txtHigSaleUnit.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["HS_ROW_COUNT"].ToString() + ",'" + gvHighLiqDetails.ClientID + "','txtHigSaleUnit','" + TxtTotalUnits.ClientID + "','chkHigSale');");
                txtHigSaleUnit.Attributes.Add("onblur", "javascript:return CalcSaleAmt('" + txtHigSaleUnit.ClientID + "','" + LblAvlUnits.Text + "','" + gvrow.Cells[6].Text.ToString() + "');");

            }
        }

    }

    protected void gvMedLiqDetails_DataBound(object sender, EventArgs e)
    {
        if (gvMedLiqDetails.DataSource != null)
        {
            TextBox txtMedSaleUnit;
            
            Label LblValAmnt;
            TextBox TxtTotalUnits = (TextBox)gvMedLiqDetails.FooterRow.FindControl("TxtTotalUnits");
           


            foreach (GridViewRow gvrow in gvMedLiqDetails.Rows)
            {
                txtMedSaleUnit = (TextBox)gvrow.FindControl("txtMedSaleUnit");
                
                LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
                txtMedSaleUnit.SetDecimalPrefixSuffix(8, 4, false, "Medium Release Unit");
                
                txtMedSaleUnit.Text = "1";
                
            }
        }
    }
    protected void gvLowLiqDetails_DataBound(object sender, EventArgs e)
    {
        if (gvLowLiqDetails.DataSource != null)
        {
            TextBox txtLowSaleUnit;
           
            Label LblValAmnt;
            Label LblAvlUnits;
            TextBox TxtTotalUnits = (TextBox)gvLowLiqDetails.FooterRow.FindControl("TxtTotalUnits");
           


            foreach (GridViewRow gvrow in gvLowLiqDetails.Rows)
            {
                txtLowSaleUnit = (TextBox)gvrow.FindControl("txtLowSaleUnit");
               
                LblAvlUnits = (Label)gvrow.FindControl("LblAvlUnits");
                LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
               
                txtLowSaleUnit.Text = LblAvlUnits.Text.ToString();
                txtLowSaleUnit.SetDecimalPrefixSuffix(8, 4, false, "Low Release Unit");
                
                txtLowSaleUnit.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["LS_ROW_COUNT"].ToString() + ",'" + gvLowLiqDetails.ClientID + "','txtLowSaleUnit','" + TxtTotalUnits.ClientID + "','chkLowSale')");
                txtLowSaleUnit.Attributes.Add("onblur", "javascript:return CalcSaleAmt('" + txtLowSaleUnit.ClientID + "','" + LblAvlUnits.Text + "','" + gvrow.Cells[5].Text.ToString() + "')");
            }
        }
    }
    protected void gvCommoDetails_DataBound(object sender, EventArgs e)
    {
        if (gvCommoDetails.DataSource != null)
        {
            TextBox txtCommSaleUnit;
           
            Label LblValAmnt;
            Label LblAvlUnits;
            TextBox TxtTotalUnits = (TextBox)gvCommoDetails.FooterRow.FindControl("TxtTotalUnits");
           
            foreach (GridViewRow gvrow in gvCommoDetails.Rows)
            {
                txtCommSaleUnit = (TextBox)gvrow.FindControl("txtCommSaleUnit");
                
                LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
                LblAvlUnits = (Label)gvrow.FindControl("LblAvlUnits");
               
                txtCommSaleUnit.Text = LblAvlUnits.Text.ToString();
                txtCommSaleUnit.SetDecimalPrefixSuffix(8, 4, false, "Commodities Release Unit");
                
                txtCommSaleUnit.Attributes.Add("onkeyup", "javascript:return AddGrid(" + ViewState["COM_ROW_COUNT"].ToString() + ",'" + gvCommoDetails.ClientID + "','txtCommSaleUnit','" + TxtTotalUnits.ClientID + "','chkCommSale')");
                txtCommSaleUnit.Attributes.Add("onblur", "javascript:return CalcSaleAmt('" + txtCommSaleUnit.ClientID + "','" + LblAvlUnits.Text + "','" + gvrow.Cells[6].Text.ToString() + "')");
            }
        }

    }
    protected void gvFinDetails_DataBound(object sender, EventArgs e)
    {
        if (gvFinDetails.DataSource != null)
        {
            TextBox txtFinSaleUnit;
            
            Label LblValAmnt;
            TextBox TxtTotalUnits = (TextBox)gvFinDetails.FooterRow.FindControl("TxtTotalUnits");
           
            foreach (GridViewRow gvrow in gvFinDetails.Rows)
            {
                txtFinSaleUnit = (TextBox)gvrow.FindControl("txtFinSaleUnit");
               
                LblValAmnt = (Label)gvrow.FindControl("LblValAmnt");
                txtFinSaleUnit.SetDecimalPrefixSuffix(8, 4, false, "Finacial Services Release Unit");
                txtFinSaleUnit.Text = "1";
              

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
            
        }

    }

    
    protected void txtLowSaleUnit_TextChanged(object sender, EventArgs e)
    {
        FunPriAddGrid(gvLowLiqDetails, "txtLowSaleUnit","TxtTotalUnits", "chkLowSale");
        
    }
    protected void txtCommSaleUnit_TextChanged(object sender, EventArgs e)
    {
        FunPriAddGrid(gvCommoDetails, "txtCommSaleUnit", "TxtTotalUnits", "chkCommSale");
     
    }
   
    protected void txtFinSaleUnit_TextChanged(object sender, EventArgs e)
    {
        FunPriAddGrid(gvFinDetails, "txtFinSaleUnit",  "TxtTotalUnits", "chkFinSale");
       
    }
    protected void txtMedSaleUnit_TextChanged(object sender, EventArgs e)
    {
        FunPriAddGrid(gvMedLiqDetails, "txtMedSaleUnit", "TxtTotalUnits",  "chkMedSale");
       
    }
   
}
