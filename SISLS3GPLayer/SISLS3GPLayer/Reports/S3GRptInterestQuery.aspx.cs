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

#region [Page Name Space]
using System;
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
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using S3GBusEntity;
using System.Globalization;
using System.ServiceModel;
using System.Web.Services;
using System.Text;
using System.IO;
#endregion

public partial class Reports_S3GRptInterestQuery : ApplyThemeForProject
{
   #region Initialization
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
    string strPageName = "Interest Query Report";
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
    StringBuilder strBuilder = null;
#endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
        CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
        txtStartDateSearch.Attributes.Add("readonly", "readonly");
        FunPriLoadPage();
    }
    #region Load Page
    public void FunPriLoadPage()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            ObjS3GSession = new S3GSession();
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
          
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            txtStartDateSearch.Text = DateTime.Now.Date.ToString(strDateFormat);
            UserId = ObjUserInfo.ProUserIdRW;
            ProgramId = 228;
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");
            txt.Attributes.Add("ReadOnly", "ReadOnly");
            if (!IsPostBack)
            {
                FunPriLoadLob();
            }


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Page");
        }
    }
    #endregion
    #region [Load LOB]
    protected void FunPriLoadLob()
    {
         if (Procparam != null)
                Procparam.Clear();
            else
             Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(CompanyId));
            Procparam.Add("@User_ID", Convert.ToString(UserId));
            Procparam.Add("@FilterOption", "'WC','TL','TE'");
            Procparam.Add("@Program_ID", ProgramId.ToString());
            Procparam.Add("@Is_UserLobActive", "1");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
    }
    
    #endregion
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            grvprimeaccount.DataSource = null;
            grvprimeaccount.DataBind();
            grvprimeaccount.EmptyDataText = "";
            grvprimeaccount.DataBind();
            grvIncomeRecognition.DataSource = null;
            grvIncomeRecognition.DataBind();

            //txtStartDateSearch.Text = "";
            if (ddlLOB.Items.Count > 1)
            {
                ddlLOB.SelectedIndex = -1;

            }
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {
                hdnCustID.Value = hdnCustomerId.Value;
                FunPubGetCustomerDetails(hdnCustID.Value);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Customer page");
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
    }
    private void FunPriValidateFromEndDate()
    {
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
            Utility.FunShowAlertMsg(this.Page, "Select Atleast One Account Number");
            return;

        }
        ClsPubCustomer objcust = new ClsPubCustomer();
        objcust.Customer = ucCustDetails.CustomerName + " (" + ucCustDetails.CustomerCode + ")";
        Session["CustomerCode"] = objcust.Customer;
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
        }
        else
        {
            objcust.Address = ucCustDetails.CustomerAddress;
        }
        objcust.EMail = ucCustDetails.EmailID;
        objcust.Mobile = ucCustDetails.Mobile;
        objcust.WebSite = ucCustDetails.Website;
        Session["CustomerInfo"] = objcust;

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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Repayment Schedule page");
        }
        finally
        {
            objSerClient.Close();
        }
    }
    protected void chkSelectAccount_CheckedChanged(object sender, EventArgs e)
    {
        
    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grvprimeaccount.DataSource = null;
            grvprimeaccount.DataBind();
            grvprimeaccount.EmptyDataText = "";
            grvprimeaccount.DataBind();
            grvIncomeRecognition.DataSource = null;
            grvIncomeRecognition.DataBind();
            if (hdnCustID.Value == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Customer");
                return;
                //ddlLOB.SelectedValue = "-1";
            }
            divacc.Style.Add("display", "block");
            if (ddlLOB.SelectedIndex > 0)
            {
                Funpriloadpasa();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load page");
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

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Account Grid");
        }
    }
   
    protected void btnclear_Click(object sender, EventArgs e)
    {
        divacc.Style.Add("display", "none");
        txtCustomerCode.Text = ucCustDetails.CustomerName;
        ucCustDetails.ClearCustomerDetails();
        ucCustomerCodeLov.FunPubClearControlValue();
        txtCustomerCode.Text = "";
        ucCustDetails.ClearCustomerDetails();
        grvprimeaccount.DataSource = null;
        grvprimeaccount.DataBind();
        grvprimeaccount.EmptyDataText = "";
        grvprimeaccount.DataBind();
        grvIncomeRecognition.DataSource = null;
        grvIncomeRecognition.DataBind();
        txtStartDateSearch.Text = "";
        hdnCustID.Value = "";
        btnPrint.Enabled = false;
        pnlIRDetails.Visible = false;
        txtStartDateSearch.Text = DateTime.Now.Date.ToString(strDateFormat);
        if (ddlLOB.Items.Count > 0)
        {
            ddlLOB.SelectedIndex = -1;
        }
    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
       
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        try
        {
            LoadXML();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", CompanyId.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Customer_ID", hdnCustID.Value);
            Procparam.Add("@CutOffStartDate", "01/04/1901 12:00:00 AM");
            Procparam.Add("@CutOffEndDate", Convert.ToString(Utility.StringToDate(txtStartDateSearch.Text)));
            Procparam.Add("@XMLAccountDetails", strBuilder.ToString());
            Session["Procparam"] = Procparam;
            DataTable dtIncome = Utility.GetDefaultData("S3G_RPT_GetIncomeReportsDtls", Procparam);
            if (dtIncome.Rows.Count > 0)
            {
                pnlIRDetails.Visible = true;
                grvIncomeRecognition.Visible = true;
                grvIncomeRecognition.DataSource = dtIncome;
                grvIncomeRecognition.DataBind();
                btnPrint.Enabled = true;

            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Records found");
                btnPrint.Enabled = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Calculate Interest query");
        }
    }

    private void LoadXML()
    {
      
            strBuilder = new StringBuilder();
            strBuilder.Append("<Root>");
            foreach (GridViewRow grow in grvprimeaccount.Rows)
            {
                Label lblPANumber = (Label)grow.FindControl("lblMLA");
                Label lblSANumber = (Label)grow.FindControl("lblSLA");
                CheckBox chkSelectAccount = (CheckBox)grow.FindControl("chkSelectAccount");
                if (chkSelectAccount.Checked)
                {
                    strBuilder.Append("<Details PANUM ='" + lblPANumber.Text + "'");
                    if (string.IsNullOrEmpty(lblSANumber.Text))
                    {
                        strBuilder.Append(" SANUM='" + lblPANumber.Text + "DUMMY" + "'/> ");
                    }
                    else
                    {
                        strBuilder.Append(" SANUM='" + lblSANumber.Text + "'  /> ");
                    }
                }
            }
            strBuilder.Append("</Root>");
        
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string strScipt = "window.open('../Reports/S3GRptInterestQueryReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Interest Memorandum Report", strScipt, true);
    }
}



