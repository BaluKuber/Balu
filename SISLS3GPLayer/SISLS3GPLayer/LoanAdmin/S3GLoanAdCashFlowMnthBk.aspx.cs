#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin 
/// Screen Name			: Cash Flow Monthly Booking
/// Created By			: Nataraj Y
/// Created Date		: 10-Sep-2010
/// Purpose	            : 
/// 
/// Modified By         : Thangam M
/// Modified on         : 13/Oct/2011
/// Purpose             : To fix UAT cases
/// <Program Summary>
#endregion

#region Namespaces
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
using System.Globalization;
using S3GBusEntity.LoanAdmin;
using S3GBusEntity;
#endregion

public partial class LoanAdmin_S3GLoanAdCashFlowMnthBk : ApplyThemeForProject
{
    #region Variable declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    Dictionary<string, string> Procparam;
    int intCompany_Id;
    int intUserId;
    int intResult;
    string strCFMNo;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bClearList = false;
    public string strDateFormat;
    static string strMode;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=CMB';";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdCashFlowMnthBk.aspx';";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=CMB";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    LoanAdminAccMgtServices.S3G_LOANAD_CashFlowMonthlyBookingDataTable ObjCFMBkTable;
    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjLoanAdminAccMgtServices;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();

        intCompany_Id = ObjUserInfo.ProCompanyIdRW;

        intUserId = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];

        if (Request.QueryString.Get("qsViewId") != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            strCFMNo = Convert.ToString(fromTicket.Name);
        }
        txtCFMDate.Attributes.Add("readonly", "readonly");
        txtCFMNumber.Attributes.Add("readonly", "readonly");
        if (!IsPostBack)
        {
            FunPriLoadPage();
        }
    }

    private void FunPriLoadLObandBranch(int intUser_id, int intCompany_id)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@User_Id", intUser_id.ToString());
        Procparam.Add("@Company_ID", intCompany_id.ToString());
        Procparam.Add("@Program_Id", "84");
        ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        ddlLineofBusiness.SelectedValue = "3";
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Branch_ID", "Branch_Code", "Branch_Name" });
        FunPriLoadLocation();
        ddlFinancialYear.FillFinancialYears();
    }

    private void FunPriLoadLocation()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@User_Id", intUserId.ToString());
        Procparam.Add("@Company_ID", intCompany_Id.ToString());
        Procparam.Add("@Program_Id", "84");
        if(ddlLineofBusiness.SelectedIndex > 0)
            Procparam.Add("@Lob_Id", ddlLineofBusiness.SelectedValue);

        // Added by Thangam to fix UAT case ID CFM_002 on 13/Oct/2011
        if (ddlFinancialMonth.SelectedIndex > 0)
            Procparam.Add("@DemandMonth", ddlFinancialMonth.SelectedValue);
        // End here

        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Branch_ID", "Branch_Code", "Branch_Name" });
        ddlBranch.BindDataTable("S3G_Get_DemandedLocation_List", Procparam, new string[] { "Location_ID", "Location" });

       
    }

    public DateTime FunPubEndDt(string strFinancialMonth)
    {

        int intYear = Convert.ToInt32(strFinancialMonth.Substring(0, 4));

        int intMonth = Convert.ToInt32(strFinancialMonth.Substring(4, 2));

        int intEnddt = DateTime.DaysInMonth(intYear, intMonth);

        DateTime EndDate = DateTime.Parse(intMonth.ToString() + "/" + intEnddt.ToString() + "/" + intYear.ToString());

        return EndDate;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjLoanAdminAccMgtServices = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();

        try
        {
            if (grvCashflowDetails.Rows.Count > 0)
            {
                if (FunPubEndDt(ddlFinancialMonth.SelectedValue) > Utility.StringToDate(txtCFMDate.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Cashflow Monthly Booking is a month end process')", true);
                    return;
                }
                string strCFMNumber;
                ObjCFMBkTable = new LoanAdminAccMgtServices.S3G_LOANAD_CashFlowMonthlyBookingDataTable();
                LoanAdminAccMgtServices.S3G_LOANAD_CashFlowMonthlyBookingRow ObjCFMBKRow;
                ObjCFMBKRow = ObjCFMBkTable.NewS3G_LOANAD_CashFlowMonthlyBookingRow();
                ObjCFMBKRow.Company_ID = intCompany_Id;
                ObjCFMBKRow.LOB_ID = Convert.ToInt32(ddlLineofBusiness.SelectedValue);
                ObjCFMBKRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
                ObjCFMBKRow.CashFlowMonthly_Date = Utility.StringToDate(txtCFMDate.Text);
                ObjCFMBKRow.XMlCFMDetails = grvCashflowDetails.FunPubFormXml();
                ObjCFMBKRow.Created_By = intUserId;
                ObjCFMBKRow.CashFlow_Type = ddlCashFlowType.SelectedValue;
                ObjCFMBKRow.CashFlow_Month = ddlFinancialMonth.SelectedValue;
                ObjCFMBKRow.CashFlow_Year = ddlFinancialYear.SelectedItem.Text;
                ObjCFMBkTable.AddS3G_LOANAD_CashFlowMonthlyBookingRow(ObjCFMBKRow);
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] objbyteCFMBkTable = ClsPubSerialize.Serialize(ObjCFMBkTable, SerMode);

                intResult = ObjLoanAdminAccMgtServices.FunPubCreateCFMBkInt(out strCFMNumber, SerMode, objbyteCFMBkTable);

                switch (intResult)
                {
                    case 0:
                        txtCFMNumber.Text = strCFMNumber;
                        strAlert = "Cashflow Monthly Booking Completed Successfully " + strCFMNumber;
                        strAlert += @"\n\nWould you like to Continue?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                        break;

                    case -1:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        strRedirectPageView = "";
                        break;
                    case -2:
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                        strRedirectPageView = "";
                        break;
                    case 3:
                        strAlert = strAlert.Replace("__ALERT__", "Document Sequence not defined for Sys Jv Ref No");
                        strRedirectPageView = "";
                        break;
                    case 5:
                        strAlert = strAlert.Replace("__ALERT__", "Cashflow Monthly booking date cannot be in a closed month");
                        strRedirectPageView = "";
                        break;
                    case 6:
                        strAlert = strAlert.Replace("__ALERT__", "Selected Cashflow month / year is closed");
                        strRedirectPageView = "";
                        break;
                    case 50:
                        strAlert = strAlert.Replace("__ALERT__", "Error in saving Cashflow Monthly Booking");
                        strRedirectPageView = "";
                        break;
                    default:
                        Utility.FunShowValidationMsg(this.Page, "LOANAD_CFMB", intResult, strCFMNumber, false);
                        //strAlert = strAlert.Replace("__ALERT__", "Error in saving Cashflow Monthly Booking");
                        strRedirectPageView = "";
                        return;
                        break;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Pending Cashflow booking for selected financial month");
            }
        }
        catch (Exception ex)
        {
            cvCashFlowMnthBk.ErrorMessage = ex.Message;
            cvCashFlowMnthBk.IsValid = false;
        }
        finally
        {
            ObjLoanAdminAccMgtServices.Close();
        }
    }

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        ObjLoanAdminAccMgtServices = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
        try
        {
            int intResult = 0;
            intResult = ObjLoanAdminAccMgtServices.FunPubRevokeCFMBkInt(strCFMNo, intCompany_Id);

            if (intResult == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "Cashflow Monthly Booking Revoked Sucessfully.");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Unable to Revoke Cashflow Monthly Booking.");
                strRedirectPageView = "";
            }
              
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            cvCashFlowMnthBk.ErrorMessage = ex.Message;
            cvCashFlowMnthBk.IsValid = false;
        }
        finally
        {
            ObjLoanAdminAccMgtServices.Close();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlCashFlowType.SelectedIndex = 0;
        if(ddlFinancialMonth.Items.Count > 0)
        ddlFinancialMonth.SelectedIndex = 0;
        ddlFinancialYear.SelectedIndex = 0;
        ddlLineofBusiness.SelectedIndex = 0;
        ddlBranch.SelectedIndex = 0;
        grvCashflowDetails.DataSource = null;
        grvCashflowDetails.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlFinancialYear.SelectedIndex = 0;
            ddlFinancialMonth.Items.Clear();
            ddlCashFlowType.SelectedIndex = 0;

            FunPriLoadCashflowDetails();
            ddlBranch.Focus();
            //        ddlCashFlowType.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            cvCashFlowMnthBk.ErrorMessage = ex.Message;
            cvCashFlowMnthBk.IsValid = false;
        }
    }

    protected void ddlCashFlowType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCashFlowType.SelectedIndex > 0)
        {
            try
            {
                FunPriLoadCashflowDetails();
                ddlCashFlowType.Focus();
            }
            catch (Exception ex)
            {
                cvCashFlowMnthBk.ErrorMessage = ex.Message;
                cvCashFlowMnthBk.IsValid = false;
            }
         
        } 
        else
        {
            grvCashflowDetails.DataSource = null;
            grvCashflowDetails.DataBind();
        }

    }

    protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCashFlowType.SelectedIndex = 0;

        if (ddlFinancialYear.SelectedIndex > 0)
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompany_Id.ToString());
            Procparam.Add("@LOB_ID", ddlLineofBusiness.SelectedValue.ToString());
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@FinanceYear", ddlFinancialYear.SelectedValue.ToString());
            Procparam.Add("@StartMonth", ClsPubConfigReader.FunPubReadConfig("StartMonth"));

            ddlFinancialMonth.BindDataTable("S3G_LOANAD_GetDemandedMonths", Procparam, new string[] { "FinMonth", "MonthName" });

            //ddlFinancialMonth.FillFinancialMonth(ddlFinancialYear.SelectedItem.Text);
            ddlFinancialYear.Focus();
        }
        else
        {
            txtCFMDate.Text = string.Empty;
        }
        try
        {
            FunPriLoadCashflowDetails();
        }
        catch (Exception ex)
        {
            cvCashFlowMnthBk.ErrorMessage = ex.Message;
            cvCashFlowMnthBk.IsValid = false;
        }
    }

    protected void ddlFinancialMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlCashFlowType.SelectedIndex = 0;
        if (ddlFinancialMonth.SelectedIndex > 0)
        {
            try
            {
                ddlCashFlowType.SelectedIndex = 0;

                txtCFMDate.Text = DateTime.Parse(FunPubEndDt(ddlFinancialMonth.SelectedValue).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                //FunPriLoadLocation();
                FunPriLoadCashflowDetails();
                ddlFinancialMonth.Focus();
            }
            catch (Exception ex)
            {
                cvCashFlowMnthBk.ErrorMessage = ex.Message;
                cvCashFlowMnthBk.IsValid = false;
            }
        }
        else
        {
            txtCFMDate.Text = string.Empty;
        }
    }

    private void FunPriLoadCashflowDetails()
    {
        try
        {
            if (ddlFinancialYear.SelectedIndex > 0 && ddlFinancialMonth.SelectedIndex > 0 && ddlLineofBusiness.SelectedIndex > 0 && ddlBranch.SelectedIndex > 0 && ddlCashFlowType.SelectedIndex > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Clear();
                Procparam.Add("@Company_ID", intCompany_Id.ToString());
                Procparam.Add("@LOB_ID", ddlLineofBusiness.SelectedValue);
                //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
                Procparam.Add("@Option", "1");
                Procparam.Add("@Param1", ddlFinancialYear.SelectedItem.Text);
                Procparam.Add("@Param2", ddlFinancialMonth.SelectedValue);

                switch (ddlCashFlowType.SelectedItem.Value)
                {
                    case "0":
                        Procparam.Add("@Param3", "Inflow");
                        break;
                    case "1":
                        Procparam.Add("@Param3", "Outflow");
                        break;
                    case "2":
                        Procparam.Add("@Param3", "Both");
                        break;

                }
                DataTable dt_Cashflowdtls = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetCashMntlyBk_List, Procparam);
                grvCashflowDetails.Columns[5].Visible = true;
                grvCashflowDetails.Columns[6].Visible = true;
                if (dt_Cashflowdtls != null)
                {
                    if (dt_Cashflowdtls.Rows.Count > 0)
                    {
                        grvCashflowDetails.DataSource = dt_Cashflowdtls;
                        grvCashflowDetails.DataBind();
                        grvCashflowDetails.Columns[5].Visible = false;
                        grvCashflowDetails.Columns[6].Visible = false;
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "No pending Cashflow booking for selected financial month");
                        grvCashflowDetails.DataSource = null;
                        grvCashflowDetails.DataBind();
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "No pending Cashflow booking for selected financial month");
                    grvCashflowDetails.DataSource = null;
                    grvCashflowDetails.DataBind();
                }
            }
            else
            {
                //Utility.FunShowAlertMsg(this, "No pending Cashflow booking for selected month/Year");
                grvCashflowDetails.DataSource = null;
                grvCashflowDetails.DataBind();
            }
           
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error in loading Cashflow booking details");
        }
    }
    private void FunPriCashflowControlStatus(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                ddlCashFlowType.Visible = true;
                lblCahflowType.Visible = true;
                break;
            case 1://Modify
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                //ddlCashFlowType.Visible = false;
                //lblCahflowType.Visible = false;
                break;
            case -1://Query  
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                //ddlCashFlowType.Visible = false;
                //lblCahflowType.Visible = false;
                break;
        }
    }

    private void FunPriLoadPage()
    {
        FunPriLoadLObandBranch(intUserId, intCompany_Id);
        if (strCFMNo != null)
        {
            FunPubGetCFMDetails(strCFMNo);
            if (strMode == "M")
            {
                FunPriCashflowControlStatus(-1);
            }
            if (strMode == "Q")
            {
                FunPriCashflowControlStatus(-1);
            }
        }
        else
        {
            FunPriCashflowControlStatus(0);

            // Commanded By Thangam.M on 13/Oct/2011 to fox UAT case CFM_002 
            //txtCFMDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            // End here

        }
        ddlLineofBusiness.Focus();
    }

    private void FunPubGetCFMDetails(string strCFMNo)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", intCompany_Id.ToString());
        Procparam.Add("@Option", "3");
        Procparam.Add("@Param1", strCFMNo);
        DataSet dsCFMDetails = Utility.GetDataset(SPNames.S3G_LOANAD_GetCashMntlyBk_List, Procparam);
        if (dsCFMDetails.Tables.Count > 0)
        {
            txtCFMNumber.Text = dsCFMDetails.Tables[0].Rows[0]["CashFlowMonthly_No"].ToString();
            txtCFMDate.Text = dsCFMDetails.Tables[0].Rows[0]["CFM_Date"].ToString();
            ddlLineofBusiness.SelectedValue = dsCFMDetails.Tables[0].Rows[0]["LOB_ID"].ToString();
            ddlLineofBusiness.ClearDropDownList();
            //ddlBranch.SelectedValue = dsCFMDetails.Tables[0].Rows[0]["Branch_ID"].ToString();
            ddlBranch.SelectedValue = dsCFMDetails.Tables[0].Rows[0]["Location_Id"].ToString();
            ddlBranch.ClearDropDownList();
            ddlFinancialYear.SelectedValue = dsCFMDetails.Tables[0].Rows[0]["CashFlow_Year"].ToString();
            ddlFinancialYear.ClearDropDownList();
            //ddlFinancialMonth.FillFinancialMonth(dsCFMDetails.Tables[0].Rows[0]["CashFlow_Year"].ToString());
            ListItem ddlItem = new ListItem(dsCFMDetails.Tables[0].Rows[0]["MonthName"].ToString(), dsCFMDetails.Tables[0].Rows[0]["CashFlow_Month"].ToString());
            ddlFinancialMonth.Items.Add(ddlItem);
            ddlFinancialMonth.SelectedValue = dsCFMDetails.Tables[0].Rows[0]["CashFlow_Month"].ToString();
            //ddlFinancialMonth.ClearDropDownList();
            ddlCashFlowType.SelectedValue = dsCFMDetails.Tables[0].Rows[0]["CashFlow_Type"].ToString();
            ddlCashFlowType.ClearDropDownList();
            grvCashflowDetails.Columns[5].Visible = true;
            grvCashflowDetails.Columns[6].Visible = true;
            grvCashflowDetails.DataSource = dsCFMDetails.Tables[1];
            grvCashflowDetails.DataBind();
            grvCashflowDetails.Columns[5].Visible = false;
            grvCashflowDetails.Columns[6].Visible = false;
            grvCashflowDetails.Columns[2].Visible = false;

            if (PageMode == PageModes.Modify)
            {
                btnRevoke.Visible = true;

                if (dsCFMDetails.Tables[0].Rows[0]["Can_Revoke"].ToString() == "1")
                {
                    btnRevoke.Enabled = true;
                }
                else
                {
                    btnRevoke.Enabled = false;
                }
            }
        }
    }
    protected void ddlLineofBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadLocation();
            ddlFinancialYear.SelectedIndex = 0;
            ddlFinancialMonth.Items.Clear();
            ddlCashFlowType.SelectedIndex = 0;

            FunPriLoadCashflowDetails();
            ddlLineofBusiness.Focus();
        }
        catch (Exception ex)
        {
            cvCashFlowMnthBk.ErrorMessage = ex.Message;
            cvCashFlowMnthBk.IsValid = false;
        }
       // ddlCashFlowType.SelectedIndex = 0;
    }

   
}
