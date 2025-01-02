#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Statement Of Account
/// Created By          :   JeyaGomathi M
/// Created Date        :   8-mar-2011
/// Purpose             :   To get the Statement of Account
/// Last Updated By		:   
/// Last Updated Date   :   
/// Reason              :  
/// <Program Summary>
#endregion


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
using ReportAccountsMgtServicesReference;
using System.Collections.Generic;
using S3GBusEntity.Reports;
using S3GBusEntity;
using System.Globalization;

public partial class Reports_S3GRptStatementOfAccounts : ApplyThemeForProject
{
    Dictionary<string, string> Procparam;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public string strDateFormat;
    int CompanyId;
    int UserId;
    bool Is_Active;
    public int ProgramId;
    string Customer_ID;
    string LOB_ID;
    string Branch_ID;
    public int LobId;
    public int LocationId;
    string strPageName = "Statement Of Accounts";
    ReportAccountsMgtServicesClient objSerClient;
    DateTime StartDate;
    DateTime EndDate;
    public bool chk=false;
    decimal Totalyettobebilled;
    decimal Totalbilled;
    decimal totalbilledbalance;
    string Region_Id;
    string today;
    decimal TotalAmountFinanced;
    decimal InvoiceAmount;
    decimal CapitalisedAmount;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Application Standard Date Format
        
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
        CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
        CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
        /* Changed Date Control start - 30-Nov-2012 */
        //txtStartDateSearch.Attributes.Add("readonly", "readonly");
        //txtEndDateSearch.Attributes.Add("readonly", "readonly");
        txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
        txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
        /* Changed Date Control end - 30-Nov-2012 */

        //today = DateTime.Now.Date.ToString(strDateFormat);

         #endregion
        //txtStartDateSearch.Text = today.Substring(0, 11);
        //txtEndDateSearch.Text = today.Substring(0, 11);
        FunPriLoadPage();
        

    }
    #region Load Page
    public void FunPriLoadPage()
    {
        try
        {
            //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            //strDateFormat = ObjS3GSession.ProDateFormatRW;
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            ObjS3GSession = new S3GSession();
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
            lblAmounts.Text = "[All Amounts are in" +" "+ ObjS3GSession.ProCurrencyNameRW + " ] ";
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            UserId = ObjUserInfo.ProUserIdRW;
            ProgramId = 133;
            ddlbranch.Enabled = false;
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");
            txt.Attributes.Add("ReadOnly", "ReadOnly");
            if (!IsPostBack)
            {
                //today = DateTime.Now.Date.ToString(strDateFormat);
                //txtStartDateSearch.Text = today.Substring(0, 11);
                //txtEndDateSearch.Text = today.Substring(0, 11);
                FunPriLoadLob(CompanyId, UserId, ProgramId);
                //FunPriLoadRegion(CompanyId, Is_Active, UserId);
                FunPriLoadLocation1(CompanyId, UserId, ProgramId, LobId);
                FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
                //FunPriLoadBranch(CompanyId, UserId, Region_Id, Is_Active);
                //FunPriLoadBranch(CompanyId, UserId, Is_Active);
                EnableDisableDetailsGrid(false);
          

                //ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
                //TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                //txt.Attributes.Add("onfocus", "fnLoadCustomer()");
                //pnlsummary.Visible = false;
                //PnlMemorandum.Visible = false;
          
                //pnlAssetDetails.Visible = false;
                //pnlTransactionDetails.Visible = false;

            }


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Page");
        }
    }
    #endregion
    #region Load LOB

    private void FunPriLoadLob(int CompanyId, int UserId, int ProgramId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubSOAGetLOB(CompanyId, UserId, ProgramId);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlLOB.DataSource = LOB;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            //ddlLOB.Items[0].Text = "All";
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
            }
            else
            {
                ddlLOB.SelectedIndex = 0;
            }
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load LOB");
        }
        finally
        {
            objSerClient.Close();
        }
    }

    #endregion
    #region Load Region
    private void FunPriLoadLocation1(int CompanyId, int UserId, int ProgramId, int LobId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubBranch(CompanyId, UserId, ProgramId, LobId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddlRegion.DataSource = Branch;
            ddlRegion.DataTextField = "Description";
            ddlRegion.DataValueField = "ID";
            ddlRegion.DataBind();
            ddlRegion.Items[0].Text = "All";
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    private void FunPriLoadLocation(int CompanyId, int UserId, int ProgramId, int LobId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubBranch(CompanyId, UserId, ProgramId, LobId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddlbranch.DataSource = Branch;
            ddlbranch.DataTextField = "Description";
            ddlbranch.DataValueField = "ID";
            ddlbranch.DataBind();
            ddlbranch.Items[0].Text = "All";
            if (ddlbranch.Items.Count == 2)
            {
                ddlbranch.SelectedIndex = 1;
            }
            else
            {
                ddlbranch.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }
    private void FunPriLoadLocation2(int ProgramId, int UserId, int CompanyId, int LobId, int LocationId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            if (ddlRegion.SelectedIndex > 0)
            {
                LocationId = Convert.ToInt32(ddlRegion.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubGetLocation2(ProgramId, UserId, CompanyId, LobId, LocationId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddlbranch.DataSource = Branch;
            ddlbranch.DataTextField = "Description";
            ddlbranch.DataValueField = "ID";
            ddlbranch.DataBind();
            if (ddlbranch.Items.Count == 2)
            {
                ddlbranch.SelectedIndex = 1;
            }
            else
            {
                ddlbranch.SelectedIndex = 0;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }
    //private void FunPriLoadRegion(int CompanyId, bool Is_Active, int UserId)
    //{
    //    try
    //    {
    //        ddlRegion.Items.Clear();
    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        byte[] byteLobs = objSerClient.FunPubGetRegion(CompanyId, true,UserId);
    //        List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
    //        ddlRegion.DataSource = lobs;
    //        ddlRegion.DataTextField = "Description";
    //        ddlRegion.DataValueField = "ID";
    //        ddlRegion.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //    finally
    //    {
    //        objSerClient.Close();
    //    }
    //}
    #endregion
    #region Load branch
    private void FunPriLoadBranch(int CompanyId, int UserId, string Region_Id, bool Is_active)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            ddlbranch.Items.Clear();
            if (ddlRegion.SelectedIndex != 0)
            {
                Region = ddlRegion.SelectedValue;
            }
            byte[] byteLobs = objSerClient.FunPubGetRegBranch(CompanyId, UserId, Region, true);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlbranch.DataSource = Branch;
            ddlbranch.DataTextField = "Description";
            ddlbranch.DataValueField = "ID";
            ddlbranch.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }
    #endregion
    #region load product
    
    #endregion
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            
             ddlRegion.SelectedValue = "-1";
            ddlLOB.SelectedValue = "-1";
            ddlbranch.SelectedValue = "-1";
            EnableDisableDetailsGrid(false);
            //lblCurrency.Visible = false;
            lblAmounts.Visible = false;
            txtStartDateSearch.Text = DateTime.Now.Date.ToString(strDateFormat);
            txtEndDateSearch.Text = DateTime.Now.Date.ToString(strDateFormat);
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {
                hdnCustID.Value = hdnCustomerId.Value;
                FunPubGetCustomerDetails(hdnCustID.Value);
            }
        }
        catch(Exception)
        {

        }
        finally
        {
            objSerClient.Close();
        }

    }

    private void FunPubGetCustomerDetails(string CustomerID)
    {
        try
        {
            DataTable dtCustomer = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "56");
            Procparam.Add("@Param1", CustomerID);
            dtCustomer = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);

            if (dtCustomer.Rows.Count > 0)
            {
                ucCustDetails.SetCustomerDetails(dtCustomer.Rows[0], true);
                txtCustomerCode.Text = ucCustDetails.CustomerName;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to display Customer Details");
        }
    }

    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (hdnCustID.Value == "")
        {
            Utility.FunShowAlertMsg(this.Page, "Select the Customer");
       
        }

            
        
        FunPriValidateFromEndDate();
    }
       
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        pnlAccountDetails.Visible = pnlasset.Visible = pnlsummary.Visible = false;
        pnlTransactionDetails.Visible = PnlMemorandum.Visible = false;
        BtnPrint.Visible = lblAmounts.Visible =  false;
        divAssetview.Visible = false;
        divAccountsummary.Visible = false;
       
        //bool isCheck = false;
        //CheckBox chkAll = (CheckBox)sender;

        //if (chkAll.Checked)
        //{
        //    isCheck = true;
        //}

        //foreach (GridViewRow item in grvprimeaccount.Rows)
        //{
        //    ((CheckBox)item.FindControl("chkSelectAccount")).Checked = isCheck;
        //}
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        divacc.Style.Add("display", "none");
        ddlbranch.Enabled = false;
        ddlRegion.SelectedValue = "-1";
        ddlLOB.SelectedValue = "-1";
        ddlbranch.SelectedValue = "-1";
        divAsset.Style.Add("display", "none");
        divAccount.Style.Add("display", "none");
        divTransaction.Style.Add("display", "none");
        EnableDisableDetailsGrid(false);
        txtCustomerCode.Text = ucCustDetails.CustomerName;
        ucCustDetails.ClearCustomerDetails();
        ucCustomerCodeLov.FunPubClearControlValue();
        txtCustomerCode.Text = "";
        ucCustDetails.ClearCustomerDetails();
        grvprimeaccount.DataSource = null;
        grvprimeaccount.DataBind();
        grvprimeaccount.EmptyDataText = "";
        grvprimeaccount.DataBind();
        txtStartDateSearch.Text = "";
        txtEndDateSearch.Text = "";
        lblAmounts.Visible = false;
        //lblCurrency.Visible = false;
        chkPoff.Checked = false;
        hdnCustID.Value = "";
        divAssetview.Visible = false;
        divAccountsummary.Visible = false;
       
     }
    private void EnableDisableDetailsGrid(bool isVisible)
    {
        pnlAccountDetails.Visible = isVisible;
        pnlasset.Visible = isVisible;
        pnlTransactionDetails.Visible = isVisible;
        pnlsummary.Visible = isVisible;
        PnlMemorandum.Visible = isVisible;
        BtnPrint.Visible = isVisible;
        //today = DateTime.Now.Date.ToString(strDateFormat);
        //txtStartDateSearch.Text = today.Substring(0, 11);
        //txtEndDateSearch.Text = today.Substring(0, 11);
        if (!isVisible)
        {
            gvasset.DataSource = null;
            gvasset.DataBind();
            grvAccount.DataSource = null;
            grvAccount.DataBind();
            grvtransaction.DataSource = null;
            grvtransaction.DataBind();
            DisableSession();
        }
    }
    private void DisableSession()
    {
        Session["PASA"] = null;
        Session["Transaction"] = null;
        Session["Assets"] = null;
        Session["Summary"] = null;
        Session["Memorandum"] = null;
    }
    private void FunPriValidateFromEndDate()
    {


        //if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()))) &&
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtStartDateSearch.Text) > Utility.StringToDate(txtEndDateSearch.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to the Start Date");
                txtEndDateSearch.Text = "";
                return;
            }

        }
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           ((string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtEndDateSearch.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        }
        if (((string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
            (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtStartDateSearch.Text = txtEndDateSearch.Text;

        }
        //lblCurrency.Visible = true;
        List<ClsPubPASA> PASAs = new List<ClsPubPASA>();
        PASAs.Clear();
        foreach (GridViewRow item in grvprimeaccount.Rows)
        {
            if (((CheckBox)item.FindControl("chkSelectAccount")).Checked == true)
            {
                ClsPubPASA pasa = new ClsPubPASA();
                pasa.PrimeAccountNo = ((Label)item.FindControl("lblMLA")).Text;
                pasa.SubAccountNo = ((Label)item.FindControl("lblSLA")).Text;
                PASAs.Add(pasa);
                Session["PASA"] = PASAs;
            }
        }

        if (PASAs.Count == 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Select Atleast One Rental Schedule Number");
            return;

        }

        EnableDisableDetailsGrid(true);
        lblAmounts.Visible = true;
        divAccountsummary.Visible = true;

         ClsPubCustomer objcust = new ClsPubCustomer();
        objcust.Customer = ucCustDetails.CustomerName + " (" + ucCustDetails.CustomerCode + ")";
        Session["CustomerCode"] = objcust.Customer;
        //objcust.CustomerCode=ucCustDetails.CustomerCode;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Customer_Id", hdnCustID.Value);
        Procparam.Add("@COMPANY_ID", CompanyId.ToString());
        DataTable dtaddress = Utility.GetDefaultData("S3G_RPT_GetCustomerAddress", Procparam);
        if (dtaddress.Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["ADDRESS"].ToString()))
                objcust.Address += dtaddress.Rows[0]["ADDRESS"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["ADDRESS2"].ToString()))
                objcust.Address += " \n" + dtaddress.Rows[0]["ADDRESS2"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["CITY"].ToString()))
                objcust.Address += " \n" + dtaddress.Rows[0]["CITY"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["STATE"].ToString()))
                objcust.Address += " , " + dtaddress.Rows[0]["STATE"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["COUNTRY"].ToString()))
                objcust.Address += " \n" + dtaddress.Rows[0]["COUNTRY"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["PINCODE"].ToString()))
                objcust.Address += " - " + dtaddress.Rows[0]["PINCODE"].ToString();
            //intcustomerTypeID = Convert.ToInt32(dtaddress.Rows[0]["TYPEID"].ToString());
        }
        else
        {
            objcust.Address = ucCustDetails.CustomerAddress;
        }
        //objcust.Address = ucCustDetails.CustomerAddress;
        objcust.EMail = ucCustDetails.EmailID;
        objcust.Mobile = ucCustDetails.Mobile;
        objcust.WebSite = ucCustDetails.Website;
        Session["CustomerInfo"] = objcust;
        //ClsPubHeaderDetails objheader = new ClsPubHeaderDetails();
        //objheader.StartDate = txtStartDateSearch.Text;
        Session["Startdate"] = txtStartDateSearch.Text;
        //objheader.EndDate = txtEndDateSearch.Text;
        Session["Enddate"] = txtEndDateSearch.Text;
      
        
        
        //List<ClsPubPASA> PASAs = new List<ClsPubPASA>();
        //PASAs.Clear();
        //foreach (GridViewRow item in grvprimeaccount.Rows)
        //{
        //    if (((CheckBox)item.FindControl("chkSelectAccount")).Checked==true)
        //    {
        //        chk = true;
        //        //divAsset.Visible = true;
        //        //divAsset.Style.Add("display", "block");
        //        //divAccount.Style.Add("display", "block");
        //        //divTransaction.Style.Add("display", "block");
        //        ////pnlsummary.Visible = true;
        //        //PnlMemorandum.Visible = true;
        //        ClsPubPASA pasa = new ClsPubPASA();
        //        pasa.PrimeAccountNo = ((Label)item.FindControl("lblMLA")).Text;
        //        pasa.SubAccountNo = ((Label)item.FindControl("lblSLA")).Text;
        //        PASAs.Add(pasa);
        //        Session["PASA"] = PASAs;
        //      }
        //}
        //if (!chk)
        //{
        //    Utility.FunShowAlertMsg(this.Page, "Please select Atleast one  primeAccount Number");
        //    pnlasset.Visible = false;
        //    pnlAccountDetails.Visible = false;
        //    pnlTransactionDetails.Visible = false;
        //    PnlMemorandum.Visible = false;
        //    pnlsummary.Visible = false;
        //}
        //else
        //{
        //    lblAmounts.Visible = true;
        //    lblCurrency.Visible = true;
        //    lblCurrency.Text = ObjS3GSession.ProCurrencyNameRW;
        //    pnlasset.Visible = true;
        //    pnlAccountDetails.Visible = true;
        //    pnlTransactionDetails.Visible = true;
        //    PnlMemorandum.Visible = true;
        //    pnlsummary.Visible = true;
        //}
      
        byte[] bytePASAs = ClsPubSerialize.Serialize(PASAs, SerializationMode.Binary);
        decimal OpeningBalance;
        objSerClient = new ReportAccountsMgtServicesClient();
        //byte[] byteLobs = objSerClient.FunPubGetTransactionDetails(out OpeningBalance, CompanyId,Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString(),Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString(), bytePASAs);
        //List<ClsPubTransaction> trans = (List<ClsPubTransaction>)DeSerialize(byteLobs);
        //Session["Transaction"] = trans;
        //Session["OpenBal"] = OpeningBalance.ToString();
        //lblOpeningBalance.Text = GetOpeningBalance(OpeningBalance);
        //grvtransaction.DataSource = trans;
        //grvtransaction.DataBind();
        //if (grvtransaction.Rows.Count != 0)
        //{
        //    grvtransaction.HeaderRow.Style.Add("position", "relative");
        //    grvtransaction.HeaderRow.Style.Add("z-index", "auto");
        //    grvtransaction.HeaderRow.Style.Add("top", "auto");

        //}
        //if (grvtransaction.Rows.Count <=0)
        //{
        //    grvtransaction.EmptyDataText = "No Records Found";
        //    grvtransaction.DataBind();
        //}

        byte[] byteAsset = objSerClient.FunPubGetSOAAssetDetails(out OpeningBalance,CompanyId, Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString(), Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString(), bytePASAs);
        ClsPubSOAAsset assets = (ClsPubSOAAsset)DeSerialize(byteAsset);
        byte[] bytesummaryaccount = objSerClient.FunPubGetSummaryAccountDetails(CompanyId, Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString(), Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString(), bytePASAs);
        List<ClsPubSummaryAccount> details = (List<ClsPubSummaryAccount>)DeSerialize(bytesummaryaccount);
        Session["SummaryAccount"] = details;
        byte[] bytememoaccount = objSerClient.FunPubGetMemorandumAccountDetails(CompanyId, Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString(), Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString(), bytePASAs);
        List<ClsPubMemorandumAccount> memoaccount = (List<ClsPubMemorandumAccount>)DeSerialize(bytememoaccount);
        Session["Memoaccount"] = memoaccount;
        TotalAmountFinanced = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.AmountFinanced);
        Totalyettobebilled = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.YetToBeBilled);
        Totalbilled = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.Billed);
        totalbilledbalance = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.Balance);
        InvoiceAmount = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.InvoiceAmount);
        CapitalisedAmount = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.CapitalisedAmount);
         Session["Assets"] = assets;
         gvasset.DataSource = assets.AssetDetails;
         gvasset.DataBind();
         decimal totalAmt = assets.AssetDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.Total_SCH_Amt);
         if (gvasset.FooterRow != null)
             ((Label)gvasset.FooterRow.FindControl("lblTotalAmt")).Text = totalAmt.ToString();
         grvAccount.DataSource = assets.AccountDetails;
         grvAccount.DataBind();
         grvtransaction.DataSource = assets.Transaction;
         grvtransaction.DataBind();
         Utility.GetAlternativeColorToGrid(grvtransaction, "lblPANum");
         lblOpeningBalance.Text = GetOpeningBalance(OpeningBalance);
         //TotalAmountFinanced = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.AmountFinanced);
         //Totalyettobebilled = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.YetToBeBilled);
         //Totalbilled = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.Billed);
         //totalbilledbalance = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.Balance);
         //totalDues = assets.Transaction.Sum(clspubTransaction => clspubTransaction.Dues);
         //totalReceipts = assets.Transaction.Sum(clspubTransaction => clspubTransaction.Receipts);
         decimal totalDues = assets.Transaction.Sum(clspubTransaction => clspubTransaction.Dues);
         decimal totalReceipts = assets.Transaction.Sum(clspubTransaction => clspubTransaction.Receipts);
         decimal totalBalance = OpeningBalance + totalDues - totalReceipts;

         if (grvtransaction.FooterRow != null)
         {
             ((Label)grvtransaction.FooterRow.FindControl("lblTotalDues")).Text = totalDues.ToString();
             ((Label)grvtransaction.FooterRow.FindControl("lblTotalReceipts")).Text = totalReceipts.ToString();
             ((Label)grvtransaction.FooterRow.FindControl("lblTotalbalance")).Text = totalBalance.ToString();
         }

         //if (gvasset.Rows.Count != 0)
         //{
         //    gvasset.HeaderRow.Style.Add("position", "relative");
         //    gvasset.HeaderRow.Style.Add("z-index", "auto");
         //    gvasset.HeaderRow.Style.Add("top", "auto");

         //}
         divAssetview.Visible = true;
         if (gvasset.Rows.Count <= 0)
         {
             gvasset.EmptyDataText = "No Invoice Found";
             gvasset.DataBind();
        }
         if (grvAccount.Rows.Count <= 0)
         {
             grvAccount.EmptyDataText = "No Rental Schedule Found";
             grvAccount.DataBind();
         }


         if (grvAccount.FooterRow != null)
         {
             ((Label)grvAccount.FooterRow.FindControl("lblTotalAmountFinanced")).Text = TotalAmountFinanced.ToString();
             ((Label)grvAccount.FooterRow.FindControl("lblTotalYetbilled")).Text = Totalyettobebilled.ToString();
             ((Label)grvAccount.FooterRow.FindControl("lblTotalbilled")).Text = Totalbilled.ToString();
             ((Label)grvAccount.FooterRow.FindControl("lblTotalbilledbalance")).Text = totalbilledbalance.ToString();
             ((Label)grvAccount.FooterRow.FindControl("lblTotal_Invoice")).Text = InvoiceAmount.ToString();
             ((Label)grvAccount.FooterRow.FindControl("lblTotalCapitalised")).Text = CapitalisedAmount.ToString();
         }
        //decimal totalyettobebilled = assets.Sum(ClsTrans => ClsTrans.Dues);
        //decimal totalReceipts = trans.Sum(ClsTrans => ClsTrans.Receipts);

         string lob;

         lob = (ddlLOB.SelectedItem.Text.Split('-'))[0].ToString().Trim();
         
        if(lob=="TE")
        {
            lblInstallmentDues.Text="Principal Due";
            Session["due"] = lblInstallmentDues.Text;
        }
        else if (lob == "FT")
        {
            lblInstallmentDues.Text = "Principal Due";
            Session["due"] = lblInstallmentDues.Text;
        }
        else if (lob == "WC")
        {
            lblInstallmentDues.Text = "Principal Due";
            Session["due"] = lblInstallmentDues.Text;
        }
        else
        {
            lblInstallmentDues.Text = "Rental Due";
            Session["due"] = lblInstallmentDues.Text;
        }
        byte[] byteSummary = objSerClient.FunPubGetSummaryDetails(CompanyId, Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString(), Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString(), bytePASAs);

        ClsPubSummary summary = (ClsPubSummary)DeSerialize(byteSummary);
        if (summary != null)
        {
            txtInstallmentDues.Text = summary.InstallmentDues.ToString(Funsetsuffix());
            //txtInstallmentDues.Style("TEXT-ALIGN") = TextAlign.Right.ToString();
            txtInterestDues.Text = summary.InterestDues.ToString(Funsetsuffix());
            txtInsuranceDues.Text = summary.InsuranceDues.ToString(Funsetsuffix());
            txtODIDues.Text = summary.ODIDues.ToString(Funsetsuffix());
            txtOtherDues.Text = summary.OtherDues.ToString(Funsetsuffix());
            Session["Summary"] = summary;
            Session["Sum"] = txtInstallmentDues.Text + txtInterestDues.Text + txtInsuranceDues.Text + txtODIDues.Text + txtOtherDues.Text;
        }

        byte[] byteMemo = objSerClient.FunPubGetMemorandumDetails(CompanyId, Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString(), Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString(), bytePASAs);
        ClsPubMemorandum memo = (ClsPubMemorandum)DeSerialize(byteMemo);
        if (memo != null)
        {

            txtChequeReturnDue.Text = memo.ChequeReturnDues.ToString(Funsetsuffix());
            txtDocumentChargesDue.Text = memo.DocumentChargesDues.ToString(Funsetsuffix());
            txtODIDue.Text = memo.ODIDues.ToString(Funsetsuffix());
            txtverifiDue.Text = memo.VerificationChargesDues.ToString(Funsetsuffix());
            txtMemoOtherDues.Text = memo.OtherDues.ToString(Funsetsuffix());
            Session["Memorandum"] = memo;
        }
            
    }


    
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (hdnCustID.Value =="")
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Customer");
                ddlLOB.SelectedValue = "-1";
            }
            pnlsummary.Visible = false;
            PnlMemorandum.Visible = false;
            pnlasset.Visible = false;
            pnlAccountDetails.Visible = false;
            pnlTransactionDetails.Visible = false;
            BtnPrint.Visible = false;
            divAssetview.Visible = false;
            divAccountsummary.Visible = false;
            lblAmounts.Visible = false;
            FunPriLoadLocation1(CompanyId, UserId, ProgramId, LobId);
            FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
            divacc.Style.Add("display", "block");
            Funpriloadpasa();
        }
        catch (Exception ex)
        {
        }
        finally
        {
            objSerClient.Close();
        }
    }

    protected void txtStartDateSearch_OnTextChanged(object sender, EventArgs e)
    {
        try
        {

            pnlsummary.Visible = false;
            PnlMemorandum.Visible = false;
            pnlasset.Visible = false;
            pnlAccountDetails.Visible = false;
            pnlTransactionDetails.Visible = false;
            BtnPrint.Visible = false;
            divAssetview.Visible = false;
            divAccountsummary.Visible = false;
            
        }
        catch (Exception ex)
        {
        }
        
    }

    protected void txtEndDateSearch_OnTextChanged(object sender, EventArgs e)
    {
        try
        {

            pnlsummary.Visible = false;
            PnlMemorandum.Visible = false;
            pnlasset.Visible = false;
            pnlAccountDetails.Visible = false;
            pnlTransactionDetails.Visible = false;
            BtnPrint.Visible = false;
            divAssetview.Visible = false;
            divAccountsummary.Visible = false;
        }
        catch (Exception ex)
        {
        }
        
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (hdnCustID.Value == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Customer");
                ddlbranch.SelectedValue = "-1";
            }
            pnlsummary.Visible = false;
            PnlMemorandum.Visible = false;
            pnlasset.Visible = false;
            pnlAccountDetails.Visible = false;
            pnlTransactionDetails.Visible = false;
            BtnPrint.Visible = false;
            divAssetview.Visible = false;
            divAccountsummary.Visible = false;
            Funpriloadpasa();
         
        }
        catch (Exception ex)
        {
        }
        finally
        {
            objSerClient.Close();
        }
    }
    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (chkPoff.Checked)
        {
            Session["IsAssetPrintOff"] = "1";
        }
        else
        {
            Session["IsAssetPrintOff"] = "0";
        }
        //Session["OpeningBalance"] = lblOpeningBalance.Text;
        string strScipt = "window.open('../Reports/S3GStatementOfAccountsReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "SOA", strScipt, true);

    }

    protected void btnAsset_Click(object sender, EventArgs e)
    {
        System.IO.StringWriter tw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        gvasset.RenderControl(hw);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment; filename=Invoices.xls");
        this.EnableViewState = false;
        Response.Write(tw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }
/// <summary>
/// To Get Opening Balance
/// </summary>
/// <param name="val"></param>
/// <returns></returns>
    private string GetOpeningBalance(decimal val)
    {
        string OpeningBalance = string.Empty;
        if (val < 0)
        {

            OpeningBalance = "Opening Balance as on " + txtStartDateSearch.Text + " is " + val.ToString(FunSetSuffix()).Replace("-", " ").TrimStart() + " " + "Cr.";
        }
        else
        {
            OpeningBalance = "Opening Balance as on " + txtStartDateSearch.Text + " is " + val.ToString(FunSetSuffix()) + " " + "Dr.";

        }
        return OpeningBalance;
 

        //return "Opening Balance as on " + txtStartDateSearch.Text + " is " + val.ToString(FunSetSuffix());
    }

    
    /// <summary>
    /// To set the Suffix to total
    /// </summary>
    /// <returns></returns>
    private string FunSetSuffix()
    {

        int suffix = 1;
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }
    protected void grvtransaction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Label lblDues = e.Row.FindControl("lblDues") as Label;
            decimal d1 = 0;
            if (lblDues.Text != "")
            d1 = Convert.ToDecimal(lblDues.Text);
            lblDues.Text = d1.ToString(Funsetsuffix());

            Label lblReceipts = e.Row.FindControl("lblReceipts") as Label;
            decimal d2 = 0;
            if (lblReceipts.Text != "")
                d2 = Convert.ToDecimal(lblReceipts.Text);
            lblReceipts.Text = d2.ToString(Funsetsuffix());

            Label lblbalance = e.Row.FindControl("lblbalance") as Label;
            decimal d3 = 0;
            if (lblbalance.Text != "")
            d3 = Convert.ToDecimal(lblbalance.Text);
            lblbalance.Text = d3.ToString(Funsetsuffix());

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPANum = (Label)e.Row.FindControl("lblPANum");
                Label lblSANum = (Label)e.Row.FindControl("lblSANum");
                string pan;
                pan = lblPANum.Text.Trim() + "DUMMY";
                if (string.Compare(lblSANum.Text.Trim(), pan, true) == 0)
                {
                    lblSANum.Text = "";
                }
                //if (lblSANum.Text.Trim() == lblPANum.Text.Trim() + "DUMMY")
                //{
                //    lblSANum.Text = "";
                //}


            }



        }
        catch (Exception ex)
        {
            

        }
    }

    protected void grvprimeaccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMLA = (Label)e.Row.FindControl("lblMLA");
                Label lblSLA = (Label)e.Row.FindControl("lblSLA");

                if (lblSLA.Text.Trim() == lblMLA.Text.Trim() + "DUMMY")
                {
                    lblSLA.Text = "";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = e.Row.FindControl("chkSelectAccount") as CheckBox;
                //CheckBox chkSelectAll = gvmail.HeaderRow.FindControl("chkSelectAll") as CheckBox;
                if (chkSelect != null)
                {
                    chkSelect.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvprimeaccount.ClientID + "','chkSelectAll','chkSelectAccount');");
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("chkSelectAll");
                chkSelectAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvprimeaccount.ClientID + "',this,'chkSelectAccount');");
            }

        }

        catch (Exception ex)
        {


        }
    }

    protected void grvAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPANum = (Label)e.Row.FindControl("lblPANum");
                Label lblSANum = (Label)e.Row.FindControl("lblSANum");

                if (lblSANum.Text.Trim() == lblPANum.Text.Trim() + "DUMMY")
                {
                    lblSANum.Text = "";
                }
            }
        }

        catch (Exception ex)
        {


        }
    }

    protected void gvasset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPANum = (Label)e.Row.FindControl("lblPANum");
                Label lblSANum = (Label)e.Row.FindControl("lblSANum");

                if (lblSANum.Text.Trim() == lblPANum.Text.Trim() + "DUMMY")
                {
                    lblSANum.Text = "";
                }
            }
        }

        catch (Exception ex)
        {


        }
    }
    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRegion.SelectedIndex > 0)
        {
            ddlbranch.Enabled = true;
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId);
            Funpriloadpasa();
            pnlsummary.Visible = false;
            PnlMemorandum.Visible = false;
            pnlasset.Visible = false;
            pnlAccountDetails.Visible = false;
            pnlTransactionDetails.Visible = false;
            BtnPrint.Visible = false;
            divAssetview.Visible = false;
            divAccountsummary.Visible = false;
        }
        else
        {
            FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
            ddlbranch.Enabled = false;
            Funpriloadpasa();
            pnlsummary.Visible = false;
            PnlMemorandum.Visible = false;
            pnlasset.Visible = false;
            pnlAccountDetails.Visible = false;
            pnlTransactionDetails.Visible = false;
            BtnPrint.Visible = false;
            divAssetview.Visible = false;
            divAccountsummary.Visible = false;
        }

    }
    private void Funpriloadpasa()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            ClsPubSOASelectionCriteria selection = new ClsPubSOASelectionCriteria();
            selection.CompanyId = CompanyId;
            selection.CustomerId = Convert.ToInt32(hdnCustID.Value); 
            selection.LobId = string.Empty;
            selection.LocationID1 = string.Empty;
            selection.LocationID2 = string.Empty;
            selection.ProductId = string.Empty;
            selection.ProgramId = ProgramId;
            selection.UserId = UserId;
            if (ddlLOB.SelectedIndex != 0)
            {
                selection.LobId = ddlLOB.SelectedValue;
            }
            if (ddlRegion.SelectedIndex != 0)
            {
                selection.LocationID1 = ddlRegion.SelectedValue;
            }
            if(ddlbranch.SelectedIndex!=0)
            {
                selection.LocationID2=ddlbranch.SelectedValue;
            }
            
            byte[] bytePASA = objSerClient.FunPubGetPASA(selection);
            List<ClsPubPASA> PASADetails = (List<ClsPubPASA>)DeSerialize(bytePASA);
            grvprimeaccount.DataSource = PASADetails;
            grvprimeaccount.DataBind();
            if (grvprimeaccount.Rows.Count == 0)
            {
                grvprimeaccount.EmptyDataText = "No Accounts Found";
                grvprimeaccount.DataBind();
            }
           
        }

        catch (Exception ex)
        {

        }
        finally
       {
                objSerClient.Close();
            }
    }
    protected void chkSelectAccount_CheckedChanged(object sender, EventArgs e)
    {
        pnlAccountDetails.Visible = false;
        pnlasset.Visible = false;
        pnlTransactionDetails.Visible = false;
        pnlsummary.Visible = false;
        PnlMemorandum.Visible = false;
        BtnPrint.Visible = false;
        lblAmounts.Visible = false;
        divAccountsummary.Visible = false;
        divAssetview.Visible = false;
        //lblCurrency.Visible = false;
    }
 
}
