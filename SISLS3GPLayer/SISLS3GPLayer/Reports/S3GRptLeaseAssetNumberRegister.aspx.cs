#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Lease Asset Register Report
/// Created By          :   Saranya I
/// Purpose             :   To Get the Complete Details of assets.
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
using System.Globalization;
using System.Collections.Generic;
using S3GBusEntity.Reports;
using ReportAdminMgtServicesReference;
using ReportAccountsMgtServicesReference;
using System.Xml.Linq;
using System.Runtime.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using S3GBusEntity;
using System.Net.Mail;
using System.Net;
using System.ServiceModel;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class Reports_S3GRptLeaseAssetNumberRegister : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int Customer_ID;
    int intProgramId = 173;
    int intUserId;
    int LobId;
    string PANum;
    string SANum;
    bool Is_Active;
    int Active;
    string Type;
    string strLAN = "x";
    decimal BalanceAmt = 0;
    decimal decbal = 0;
    decimal TotalCredit;
    decimal TotalDebit;
    decimal TotalBalance;
    int LocationId;
    int LocationId1;
    int LocationId2;
    public string strDateFormat;
    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    string strPageName = "Lease Asset Register";
    string strFilePath = string.Empty;
    DataTable dtTable = new DataTable();
    ReportAdminMgtServicesReference.ReportAdminMgtServicesClient objSerClient;
    ReportAccountsMgtServicesClient objAccClient;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
    //ReportAdminMgtServicesReference.ReportAdminMgtServicesClient objSerClient;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            #region Application Standard Date Format
            S3GSession ObjS3GSession = new S3GSession();

            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
            /* Changed Date Control start - 30-Nov-2012 */
            //txtDateFrom.Attributes.Add("readonly", "readonly");
            //txtDateTo.Attributes.Add("readonly", "readonly");
            txtDateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtDateFrom.ClientID + "','" + strDateFormat + "',true,  false);");
            txtDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtDateTo.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */
            #endregion

            #region Report Date
            txtReportDate.Text = DateTime.Now.ToString();
            txtReportDate.Text = DateTime.Now.ToString(strDateFormat);
            txtReportDate.ReadOnly = true;
            #endregion

            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;

            intUserId = ObjUserInfo.ProUserIdRW;

            intUserId = ObjUserInfo.ProUserIdRW;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();


            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            if (!IsPostBack)
            {
                ddlLOB.Focus();

                FunPriLoadLob();
                FunPubLoadDenomination();
                FunPriLoadLocation1();
                FunPriLoadLocation2();
                FunPriLoadLan();
                //FunPriloadCustomercodes();
            }
            /*
            ucCustomerCodeLov.strRegionID = null;
            FunPriloadCustomercodes();
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtName.Attributes.Add("ReadOnly", "ReadOnly");
             */
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Lease Asset Number Register Page");
        }

    }
    
    private void FunPriLoadLob()
    {

        objSerClient = new ReportAdminMgtServicesClient();
        try
        {
            //ddlLOB.Items.Clear();
            byte[] byteLobs = objSerClient.FunPubGetLOB(intCompanyId, intUserId, intProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
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

    private void FunPriLoadLocation1()
    {
        objAccClient = new ReportAccountsMgtServicesClient();
        try
        {
            int intLobId = 0;
            intLobId = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objAccClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intLobId);
            List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlRegion.DataSource = Region;
            ddlRegion.DataTextField = "Description";
            ddlRegion.DataValueField = "ID";
            ddlRegion.DataBind();
            ddlRegion.Items[0].Text = "--ALL--";
            if (ddlRegion.Items.Count == 2)
                ddlRegion.SelectedIndex = 1;
            else
                ddlRegion.SelectedIndex = 0;

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objAccClient.Close();
        }

    }

    private void FunPriLoadLocation2()
    {
        objAccClient = new ReportAccountsMgtServicesClient();
        try
        {
            int intLobId = 0;

            intLobId = Convert.ToInt32(ddlLOB.SelectedValue);

            byte[] byteLobs = objAccClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intLobId);
            //byte[] byteLobs = objAccClient.FunPubGetLocation2(intProgramId, intUserId, intCompanyId, LobId, LocationId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--ALL--";
            if (ddlBranch.Items.Count == 2)
                ddlBranch.SelectedIndex = 1;
            else
                ddlBranch.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objAccClient.Close();
        }
    }

    private void FunPriGetLocation2()
    {
        objAccClient = new ReportAccountsMgtServicesClient();
        try
        {
            int lobId = 0;

            lobId = Convert.ToInt32(ddlLOB.SelectedValue);
            int Location1 = 0;
            if (ddlRegion.SelectedIndex > 0)
                Location1 = Convert.ToInt32(ddlRegion.SelectedValue);
            byte[] byteLobs = objAccClient.FunPubGetLocation2(intProgramId, intUserId, intCompanyId, lobId, Location1);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            if (ddlBranch.Items.Count == 2)
            {
                if (ddlRegion.SelectedIndex != 0)
                {
                    ddlBranch.SelectedIndex = 1;
                    //Utility.ClearDropDownList(ddlLocation2);
                }
                else
                    ddlBranch.SelectedIndex = 0;
            }
            //else
            //{
            ddlBranch.Items[0].Text = "--ALL--";
            //ddlLocation2.SelectedIndex = 0;
            //}

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objAccClient.Close();
        }
    }
    #region 
    /*
    private void FunPriLoadPrimeAccount()
    {
        objSerClient = new ReportAdminMgtServicesClient();
        try
        {

            ClsPubPrimeAccountDetails ObjPrimeAccounts = new ClsPubPrimeAccountDetails();
            ObjPrimeAccounts.Type = "1";
            ObjPrimeAccounts.CompanyId = intCompanyId;
            //if (!string.IsNullOrEmpty(ddlLOB.SelectedValue))
            ObjPrimeAccounts.LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            if (ddlRegion.SelectedIndex > 0)
            {
                if (ddlBranch.SelectedIndex > 0)
                {
                    ObjPrimeAccounts.locationId = Convert.ToInt32(ddlBranch.SelectedValue);
                }
                else
                {
                    ObjPrimeAccounts.locationId = Convert.ToInt32(ddlRegion.SelectedValue);
                }
            }
            ObjPrimeAccounts.IsActivated = 1;
            //if (hdnCustomerId != null && hdnCustomerId.Value != "")
            ObjPrimeAccounts.CustomerId = hdnCustID.Value;
            ObjPrimeAccounts.CheckAccess = Convert.ToString(intUserId);
            ObjPrimeAccounts.ProgramId = intProgramId;

            byte[] bytePrimeAccounts = ClsPubSerialize.Serialize(ObjPrimeAccounts, SerializationMode.Binary);
            byte[] byteLobs = objSerClient.FunPubGetMLA(bytePrimeAccounts);
            List<ClsPubDropDownList> PANum = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlPanum.DataSource = PANum;
            ddlPanum.DataTextField = "Description";
            ddlPanum.DataValueField = "ID";
            ddlPanum.DataBind();

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

    protected void ddlPanum_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPubLoadSANumber();
        FunPriValidateGrid();
        ddlDenomination.ClearSelection();
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        chkWithYield.Checked = false;
        ChkExpense.Checked = false;

        //lblLaNumber.Enabled = false;
        //lblLanFrom.Enabled = false;
        //cmbLanFrom.Enabled = false;
        //lblLanTo.Enabled = false;
        //cmbLanTo.Enabled = false;

    }

    protected void ddlSanum_SelectedIndexChanged(object sender, EventArgs e)
    {

        FunPriValidateGrid();
        FunPriValidateControls();
        ddlDenomination.ClearSelection();
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        chkWithYield.Checked = false;
        ChkExpense.Checked = false;
        //lblLaNumber.Enabled = false;
        //lblLanFrom.Enabled = false;
        //cmbLanFrom.Enabled = false;
        //lblLanTo.Enabled = false;
        //cmbLanTo.Enabled = false;

    }

    private void FunPriloadCustomercodes()
    {

        ucCustomerCodeLov.strLOBID = ddlLOB.SelectedValue.ToString();
        ucCustomerCodeLov.strRegionID = null;
        ucCustomerCodeLov.strBranchID = null;
        if (ddlRegion.SelectedIndex > 0)
        {
            ucCustomerCodeLov.strBranchID = ddlRegion.SelectedValue.ToString();
        }
        if (ddlBranch.SelectedIndex > 0)
        {
            ucCustomerCodeLov.strRegionID = ddlBranch.SelectedValue.ToString();
        }
    }
    */
    #endregion
    private void FunPriLoadLan()
    {
        try
        {
            // LAN From
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@USER_ID", Convert.ToString(intUserId));
            Procparam.Add("@PROGRAM_ID", Convert.ToString(intProgramId));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            if (ddlRegion.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID1", Convert.ToString(ddlRegion.SelectedValue));
            if (ddlBranch.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID2", Convert.ToString(ddlBranch.SelectedValue));

            //cmbLanFrom.BindDataTable("S3G_RPT_GetLAN", Procparam, new string[] { "LAN", "LAN" });
            cmbLanFrom.BindDataTable("S3G_RPT_GetLAN", Procparam, new string[] { "LAN_ID", "LAN" });
            cmbLanFrom.Items[0].Text = "--ALL--";

            // LAN TO

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@USER_ID", Convert.ToString(intUserId));
            Procparam.Add("@PROGRAM_ID", Convert.ToString(intProgramId));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            if (ddlRegion.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID1", Convert.ToString(ddlRegion.SelectedValue));
            if (ddlBranch.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID2", Convert.ToString(ddlBranch.SelectedValue));
            //cmbLanTo.BindDataTable("S3G_RPT_GetLAN", Procparam, new string[] { "LAN", "LAN" });
            cmbLanTo.BindDataTable("S3G_RPT_GetLAN", Procparam, new string[] { "LAN_ID", "LAN" });
            cmbLanTo.Items[0].Text = "--ALL--";
            //cmbLanFrom.AddItemToolTip();
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriValidateFromEndDate()
    {
        #region Validate From and To Date
        //if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()))) &&
        if ((!(string.IsNullOrEmpty(txtDateFrom.Text))) && (!(string.IsNullOrEmpty(txtDateTo.Text))))    // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtDateFrom.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtDateTo.Text, dtformat))) // start date should be less than or equal to the enddate
            {
                if (Utility.StringToDate(txtDateFrom.Text) > Utility.StringToDate(txtDateTo.Text)) // start date should be less than or equal to the enddate
                {
                    Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to From Date");
                    txtDateTo.Text = " ";
                    FunPriValidateGrid();
                    return;
                }
            }
            if ((!(string.IsNullOrEmpty(txtDateFrom.Text))) &&
             ((string.IsNullOrEmpty(txtDateTo.Text))))
            {
                txtDateTo.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            }
            if (((string.IsNullOrEmpty(txtDateFrom.Text))) &&
            (!(string.IsNullOrEmpty(txtDateTo.Text))))
            {
                txtDateFrom.Text = txtDateTo.Text;
            }
        }
        #endregion


        if (grvAssetdetails.Rows.Count > 0)
        {
            pnlAssetdetails.Visible = false;
            grvAssetdetails.DataSource = null;
            grvAssetdetails.DataBind();
        }

        if (cmbLanFrom.Enabled == true)
        {

            if (cmbLanFrom.SelectedIndex > 0 && cmbLanTo.SelectedIndex == 0)
            {
                Utility.FunShowAlertMsg(this, "Select LAN To.");
                return;

            }
            else
            {
                btnPrint.Enabled = true;
                FunPriBindLanDetails();
            }
        }
        else
        {
            btnPrint.Enabled = true;
            FunPriBindLanDetails();
        }
        if (grvLeaseDetails.Rows.Count > 0)
        {
            btnPrint.Visible = true;
            lblAmounts.Visible = true;

            if (ddlDenomination.SelectedValue == "1")
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
                Session["Denomination"] = lblAmounts.Text;
            }
            else
            {
                lblAmounts.Text = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
                Session["Denomination"] = lblAmounts.Text;
            }
        }
        else
        {
            btnPrint.Visible = true;
            lblAmounts.Visible = false;
        }
        //Session["Title"] = "Journal Query From " + txtStartDate.Text + " " + "To" + " " + txtEndDate.Text;
        Session["LOB"] = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
        //if (txtCustomerCode.Text != string.Empty)
        //{
        //    Session["Customer"] = txtCustomerCode.Text;
        //}
        //else
        //{
        //    Session["Customer"] = "--";
        //}
        Session["Title"] = "Lease Asset Register Details From " + txtDateFrom.Text + " " + "To" + " " + txtDateTo.Text;
        if (ddlBranch.SelectedIndex > 0)
        {
            Session["Location"] = ddlBranch.SelectedItem.Text;
        }
        else
        {
            Session["Location"] = "ALL";
        }
        //if (ddlPanum.Items.Count > 1)
        //{
        //    if (ddlPanum.SelectedIndex > 0)
        //    {

        //        Session["AccountNo"] = ddlPanum.SelectedItem.Text;
        //    }
        //    else
        //    {

        //        Session["AccountNo"] = "All";
               
        //    }
        //}
        //else
        //{
        //    Session["AccountNo"] = "--";
        //}
        //if (ddlSanum.Items.Count > 1)
        //{
        //    if (ddlSanum.SelectedIndex > 0)
        //    {

        //        Session["SubAccountNo"] = ddlSanum.SelectedItem.Text;
        //    }
        //    else
        //    {

        //        Session["SubAccountNo"] = "All";
        //        //((Label)grvSummary.FooterRow.FindControl("lblProduct")).Text;
        //    }
        //}
        //else
        //{

        //    Session["SubAccountNo"] = "--";
        //    //((Label)grvSummary.FooterRow.FindControl("lblProduct")).Text;
        //}

    }

    public void FunPubLoadDenomination()
    {
        objAccClient = new ReportAccountsMgtServicesClient();
        try
        {

            byte[] byteLobs = objAccClient.GetDenominations();
            List<ClsPubDropDownList> Denomination = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlDenomination.DataSource = Denomination;
            ddlDenomination.DataTextField = "Description";
            ddlDenomination.DataValueField = "ID";
            ddlDenomination.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objAccClient.Close();
        }
    }

    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    private void FunPriValidateGrid()
    {
        pnlLease.Visible = false;
        grvLeaseDetails.DataSource = null;
        grvLeaseDetails.DataBind();
        pnlAssetdetails.Visible = false;
        grvAssetdetails.DataSource = null;
        grvAssetdetails.DataBind();
        lblAmounts.Visible = false;
        btnPrint.Visible = false;

    }

    private void FunPriValidateControls()
    {
        //ddlPanum.Items.Clear();
        //ddlSanum.Items.Clear();
        ddlDenomination.ClearSelection();
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        chkWithYield.Checked = false;
        ChkExpense.Checked = false;

    }

    #region Load sanum
    /*
    public void FunPubLoadSANumber()
    {
        objSerClient = new ReportAdminMgtServicesClient();
        try
        {
            ddlSanum.Items.Clear();
            byte[] byteLobs = objSerClient.FunPubGetSLA("2", intCompanyId, ddlLOB.SelectedValue, hdnCustID.Value, ddlPanum.SelectedValue, 1, intProgramId);
            List<ClsPubDropDownList> SANum = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlSanum.DataSource = SANum;
            ddlSanum.DataTextField = "Description";
            ddlSanum.DataValueField = "ID";
            ddlSanum.DataBind();
            if (ddlSanum.Items.Count == 2)
            {
                ddlSanum.SelectedIndex = 1;
            }
            else
            {
                ddlSanum.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Sub Account Number.";
            CVLAN.IsValid = false;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    */
    #endregion

    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           // ucCustomerCodeLov.strRegionID = null;
            ddlBranch.Enabled = true;
            ddlBranch.Items.Clear();
            if (ddlRegion.SelectedIndex > 0)
            {
                FunPriGetLocation2();
            }
            else
            {
                FunPriLoadLocation2();
                ddlBranch.Enabled = false;
            }
            FunPriLoadLan();
            FunPriValidateGrid();
            FunPriValidateControls();
            //ddlPanum.Items.Clear();
            //ddlSanum.Items.Clear();
            //ucCustomerCodeLov.FunPubClearControlValue();
            //ucCustomerCodeLov.ReRegisterSearchControl("LAN");

            //FunPriloadCustomercodes();
            //txtCustomerCode.Text = "";


        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Location1.";
            CVLAN.IsValid = false;
        }

    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadLan();
            FunPriValidateGrid();
            FunPriValidateControls();
            //ddlPanum.Items.Clear();

           
            //ddlSanum.Items.Clear();
            //ucCustomerCodeLov.FunPubClearControlValue();
            //ucCustomerCodeLov.ReRegisterSearchControl("LAN");

            //FunPriloadCustomercodes();
            //txtCustomerCode.Text = "";

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Location2.";
            CVLAN.IsValid = false;
        }
    }

    protected void cmbLanFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();
            //if (cmbLanTo.SelectedIndex > 0)
            //    cmbLanTo.ClearSelection();
             
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@USER_ID", Convert.ToString(intUserId));
            Procparam.Add("@PROGRAM_ID", Convert.ToString(intProgramId));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            if (ddlRegion.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID1", Convert.ToString(ddlRegion.SelectedValue));
            if (ddlBranch.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID2", Convert.ToString(ddlBranch.SelectedValue));
            if (cmbLanFrom.SelectedIndex > 0)
                Procparam.Add("@LAN_FROM", Convert.ToString(cmbLanFrom.SelectedValue));
            //cmbLanTo.BindDataTable("S3G_RPT_GetLAN", Procparam, new string[] { "LAN", "LAN" });
            cmbLanTo.BindDataTable("S3G_RPT_GetLAN", Procparam, new string[] { "LAN_ID", "LAN" });
            cmbLanTo.Items[0].Text = "--ALL--";

            chkWithYield.Checked = false;
            ChkExpense.Checked = false;

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load LANNumber.";
            CVLAN.IsValid = false;
        }
    }

    protected void cmbLanTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbLanFrom.SelectedIndex == 0)
            {
                Utility.FunShowAlertMsg(this, "Select LAN From.");
                cmbLanTo.ClearSelection();
                return;
            }
            //FunPriValidateLAN();
            FunPriValidateGrid();
            chkWithYield.Checked = false;
            ChkExpense.Checked = false;

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load LANNumber.";
            CVLAN.IsValid = false;
        }
    }


    protected void chkWithYield_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Clear Grid Details.";
            CVLAN.IsValid = false;
        }
    }

    protected void ChkExpense_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Clear Grid Details.";
            CVLAN.IsValid = false;
        }
    }

    #region Load Cutomer Details
    /*
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {

                hdnCustID.Value = hdnCustomerId.Value;
                //txtCustomerCode.Text = hdnCustID.Value.ToString();
                FunPubGetCustomerDetails();
            }
            ddlPanum.Items.Clear();
            ddlSanum.Items.Clear();
            FunPriLoadPrimeAccount();
            FunPriValidateGrid();
            FunPriValidateControls();
            cmbLanFrom.ClearSelection();
            cmbLanTo.ClearSelection();
            lblLaNumber.Enabled = false;
            lblLanFrom.Enabled = false;
            cmbLanFrom.Enabled = false;
            lblLanTo.Enabled = false;
            cmbLanTo.Enabled = false;

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Unable to Load customer Details";
            CVLAN.IsValid = false;
        }

    }

    private void FunPubGetCustomerDetails()
    {
        try
        {
            DataTable dtCustomer = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "56");
            Procparam.Add("@Param1", hdnCustID.Value.ToString());
            dtCustomer = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);

            if (dtCustomer.Rows.Count > 0)
            {
                hdnCustName.Value = dtCustomer.Rows[0]["Customer_Name"].ToString();
                //ucCustDetails.SetCustomerDetails(dtCustomer.Rows[0], true);
                txtCustomerCode.Text = hdnCustName.Value;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to display Customer Details");
        }
    }
    */
    #endregion

    private void FunPriBindLanDetails()
    {
        objSerClient = new ReportAdminMgtServicesClient();
        try
        {
            lblAmounts.Visible = true;
            btnPrint.Visible = true;
            lblError.Text = "";
            pnlLease.Visible = true;
            divLeaseDetails.Style.Add("display", "block");

            ClsPubLANRegisterinput ObjInput = new ClsPubLANRegisterinput();
            ObjInput.CompanyId = intCompanyId;
            ObjInput.UserId = intUserId;
            ObjInput.ProgramId = intProgramId;
            ObjInput.LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            if (ddlRegion.SelectedIndex > 0)
            {
                ObjInput.LocationId1 = Convert.ToInt32(ddlRegion.SelectedValue);
            }
            else
            {
                ObjInput.LocationId1 = 0;
            }
            if (ddlBranch.SelectedIndex != 0)
            {
                ObjInput.LocationId2 = Convert.ToInt32(ddlBranch.SelectedValue);
            }
            else
            {
                ObjInput.LocationId2 = 0;
            }
            if (cmbLanFrom.SelectedIndex > 0)
            {
                ObjInput.LanFrom = cmbLanFrom.SelectedValue;
            }
            if (cmbLanTo.SelectedIndex > 0)
            {
                ObjInput.LanTo = cmbLanTo.SelectedValue;
            }
            //if (ddlPanum.SelectedIndex > 0)
            //{
            //    ObjInput.Panum = ddlPanum.SelectedValue;

            //}
            //if (ddlSanum.SelectedIndex > 0)
            //{
            //    ObjInput.Sanum = ddlSanum.SelectedValue;
            //}


            ObjInput.FromDate = Utility.StringToDate(txtDateFrom.Text);
            ObjInput.ToDate = Utility.StringToDate(txtDateTo.Text);
            ObjInput.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            if (chkWithYield.Checked == true && ChkExpense.Checked == true)
            {
                ObjInput.Option = 3;        //Both
            }
            else if (chkWithYield.Checked == true && ChkExpense.Checked == false)
            {
                ObjInput.Option = 1;    //Only With Yield
            }
            else if (chkWithYield.Checked == false && ChkExpense.Checked == true)
            {
                ObjInput.Option = 2;
            }
            else
            {
                ObjInput.Option = 0;
            }
            byte[] byteLanDetail = ClsPubSerialize.Serialize(ObjInput, SerializationMode.Binary);
            byte[] byteLobs = objSerClient.FunPubGetLanDetails(byteLanDetail);
            ClsPubLeaseAssetRegisterDetails LanDetails = (ClsPubLeaseAssetRegisterDetails)DeSeriliaze(byteLobs);
            TotalCredit = LanDetails.LANDetails.Sum(ClsPubLANRegisterDetails => ClsPubLANRegisterDetails.Credit);
            TotalDebit = LanDetails.LANDetails.Sum(ClsPubLANRegisterDetails => ClsPubLANRegisterDetails.Debit);
            TotalBalance = LanDetails.LANDetails.Sum(ClsPubLANRegisterDetails => ClsPubLANRegisterDetails.Balance);

            Session["LanDetails"] = LanDetails;
            Session["LanLocation"] = LanDetails;
            grvLeaseDetails.DataSource = LanDetails.LANDetails; //LanDetails;
            grvLeaseDetails.DataBind();
            if (chkWithYield.Checked == true)
            {
                if (ChkExpense.Checked == false)
                {
                    //grvLeaseDetails.Columns[7].Visible = false;
                    Session["Option"] = "Yield";
                }
                else
                {
                   // grvLeaseDetails.Columns[7].Visible = true;
                    Session["Option"] = "NotYield";
                }
            }
            else
            {
                //grvLeaseDetails.Columns[7].Visible = true;
                Session["Option"] = "NotYield";
            }

            if (grvLeaseDetails.Rows.Count != 0)
            {
                grvLeaseDetails.HeaderRow.Style.Add("position", "relative");
                grvLeaseDetails.HeaderRow.Style.Add("z-index", "auto");
                grvLeaseDetails.HeaderRow.Style.Add("top", "auto");

            }
            if (grvLeaseDetails.Rows.Count == 0)
            {
                Session["LanDetails"] = null;
                Session["LanLocation"] = null;
                btnPrint.Enabled = false;
                lblError.Text = "No Lan Details Found";
                grvLeaseDetails.DataBind();
            }
            else
            {
                FunPriDisplayTotal();
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

    private void FunPriDisplayTotal()
    {
        ((Label)grvLeaseDetails.FooterRow.FindControl("lblTotalCredit")).Text = TotalCredit.ToString(Funsetsuffix());
        ((Label)grvLeaseDetails.FooterRow.FindControl("lblTotalDebit")).Text = TotalDebit.ToString(Funsetsuffix());

        ((Label)grvLeaseDetails.FooterRow.FindControl("lblTotalBalance")).Text = ((Label)grvLeaseDetails.Rows[grvLeaseDetails.Rows.Count - 1].FindControl("lblBalance")).Text;

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




    //<summary>
    //To clear the fields
    //</summary>
    private void FunRptClear()
    {
        try
        {
            FunPriLoadLob();
            ddlLOB.Focus();
            FunPubLoadDenomination();
            FunPriLoadLocation1();
            ddlBranch.Enabled = false;
            FunPriLoadLocation2();




            //if (ddlPanum.Items.Count > 0)
            //{
            //    ddlPanum.Items.Clear();
            //}
            //if (ddlSanum.Items.Count > 0)
            //{
            //    ddlSanum.Items.Clear();
            //}

            btnPrint.Visible = false;
            lblAmounts.Visible = false;
            //ucCustomerCodeLov.FunPubClearControlValue();
            //ucCustomerCodeLov.ReRegisterSearchControl("LAN");
            //txtCustomerCode.Text = "";

            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            chkWithYield.Checked = false;
            ChkExpense.Checked = false;
            lblLaNumber.Enabled = true;
            lblLanFrom.Enabled = true;
            cmbLanFrom.Enabled = true;
            lblLanTo.Enabled = true;
            cmbLanTo.Enabled = true;
            FunPriLoadLan();
            FunPriValidateGrid();
            ClearSession();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void ClearSession()
    {
        Session["LanDetails"] = null;
        Session["LanLocation"] = null;
        Session["Option"] = null;
        Session["Asset"] = null;

    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            ClearSession();

            FunPriValidateFromEndDate();

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Lease Asset Number Register Grid";
            CVLAN.IsValid = false;
        }

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            FunRptClear();

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Unable to Perform Specified operation";
            CVLAN.IsValid = false;
        }
    }


    protected void btnprint_Click(object sender, EventArgs e)
    {
        //if (ddlPanum.SelectedIndex > 0)
        //{
        //    Session["Wise"] = "Account";
        //}
        //else
        //{
        //    Session["Wise"] = "Lan";
        //}
        string strScipt = "window.open('../Reports/S3gRptLANRegisterReport.aspx', 'LAN','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "LAN", strScipt, true);

    }


    private void FunPriValidateLAN() //latest
    {
        string strLanFrm, FromNo, strLanTo, ToNo;
        if (cmbLanFrom.SelectedIndex > 0)
        {
            strLanFrm = cmbLanFrom.SelectedItem.Text.Split('/')[0].ToString();
        }
        else
        {
            strLanFrm = null;
        }
        if (cmbLanFrom.SelectedIndex > 0)
        {
            FromNo = cmbLanFrom.SelectedItem.Text.Split('/')[1].ToString();
        }
        else
        {
            FromNo = null;
        }

        if (cmbLanTo.SelectedIndex > 0)
        {
            strLanTo = cmbLanTo.SelectedItem.Text.Split('/')[0].ToString();
        }
        else
        {
            strLanTo = null;
        }
        if (cmbLanTo.SelectedIndex > 0)
        {
            ToNo = cmbLanTo.SelectedItem.Text.Split('/')[1].ToString();
        }
        else
        {
            ToNo = null;
        }

        if (CompareStrings(strLanFrm, strLanTo))
        {
            if (strLanFrm == strLanTo && Convert.ToDouble(FromNo) > Convert.ToDouble(ToNo))
            {
                Utility.FunShowAlertMsg(this, "LAN To should be greater than LAN From.");
                cmbLanFrom.ClearSelection();
                cmbLanTo.ClearSelection();
                FunPriValidateGrid();
                return;
            }
        }
    }

    private bool CompareStrings(string strLanFrm, string strLanTo)
    {
        // compare the values, using the CompareTo method on the first string
        int cmpVal = strLanFrm.CompareTo(strLanTo);
        if (cmpVal == 0) // the values are the same
        {
            return true;
        }
        else if (cmpVal > 0) // the first value is greater than the second value
        {
            Utility.FunShowAlertMsg(this, strLanFrm + " is greater than " + strLanTo + ",LAN To should be greater than LAN From.");
            cmbLanFrom.ClearSelection();
            cmbLanTo.ClearSelection();
            FunPriValidateGrid();
            return false;
        }
        else if (cmpVal < 0)// the second string is greater than the first string
        {
            return true;
        }
        return true;

    }

    protected void GRDValue_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //    try
        //    {
        //       if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            Label lbcredit = e.Row.FindControl("lbcredit") as Label;
        //            Label lblBal = e.Row.FindControl("lblBal") as Label;
        //            LinkButton LnkBtnAccp = e.Row.FindControl("LnkBtnAccp") as LinkButton;

        //            if (ChkExpense.Checked == false && chkWithYield.Checked == true)
        //            {
        //                GRDValue.Columns[7].Visible = false;
        //                GRDValue.Columns[8].Visible = true;

        //                //lblBal.Text = (Convert.ToDecimal(lblBal.Text) + Convert.ToDecimal(lbcredit.Text)).ToString();
        //                lblBal.Text = (Convert.ToDecimal(lblBal.Text) - Convert.ToDecimal(lbcredit.Text)).ToString();
        //                LinkButton LnkBtnPrevAccp;
        //                if (e.Row.RowIndex != 0)
        //                {
        //                    LnkBtnPrevAccp = GRDValue.Rows[e.Row.RowIndex - 1].FindControl("LnkBtnAccp") as LinkButton;
        //                }
        //                else
        //                {
        //                    LnkBtnPrevAccp = LnkBtnAccp;
        //                }


        //                if (e.Row.RowIndex == 0 || LnkBtnPrevAccp.Text != LnkBtnAccp.Text)
        //                {
        //                    if (Convert.ToDecimal(lblBal.Text) != 0)
        //                    {
        //                        lblBal.Text = ((-1) * Convert.ToDecimal(lbcredit.Text)).ToString();
        //                    }
        //                    else
        //                    {
        //                        lblBal.Text = (0).ToString(Funsetsuffix());
        //                    }
        //                }
        //                else
        //                {
        //                    decimal PrevBalance = 0;
        //                    decimal CurCredit = 0;

        //                    Label lblPrevBal = GRDValue.Rows[e.Row.RowIndex - 1].FindControl("lblBal") as Label;

        //                    PrevBalance = Convert.ToDecimal(lblPrevBal.Text);
        //                    CurCredit = Convert.ToDecimal(lbcredit.Text);

        //                    lblBal.Text = (PrevBalance - CurCredit).ToString();
        //                    //PrevBalance 
        //                }

        //                //lblBal.Text = Convert.ToDecimal(lblBal.Text) + Convert.ToDecimal(lbcredit.Text);

        //            }

        //            else //if (ChkExpense.Checked == true && chkWithYield.Checked == false)
        //            {

        //            //IMPORTANT
        //            if (LnkBtnAccp.Text == strLAN)
        //            {
        //                BalanceAmt = BalanceAmt + Convert.ToDecimal(lblBal.Text);
        //                lblBal.Text = BalanceAmt.ToString();
        //            }
        //            else
        //            {
        //                BalanceAmt = 0;
        //                BalanceAmt = Convert.ToDecimal(lblBal.Text);
        //                lblBal.Text = BalanceAmt.ToString();
        //                strLAN = LnkBtnAccp.Text;
        //            }
        //            //IMP
        //            }

        //         }

        //    }
        //    catch (Exception ex)
        //    {
        //        CVLAN.ErrorMessage = "Unable to Bind the Grid";
        //        CVLAN.IsValid = false;
        //    }
    }


    protected void lnkLan_Click(object sender, EventArgs e)
    {
        objSerClient = new ReportAdminMgtServicesClient();
        try
        {

            pnlAssetdetails.Visible = true;
            //divAssetDetails.Style.Add("display", "block");
            GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
            int rowindex = grdrow.RowIndex;
            LinkButton lnk = (LinkButton)grvLeaseDetails.Rows[rowindex].FindControl("lnkLan");
            //Label lbl1 = (Label)GRDValue.Rows[rowindex].FindControl("lblPanum");
            //Label lbl2 = (Label)GRDValue.Rows[rowindex].FindControl("lblsub");
            //Label lbl3 = (Label)GRDValue.Rows[rowindex].FindControl("lblBranch");
            byte[] byteLobs = objSerClient.FunPubGetLanAssetDetails(intCompanyId, lnk.Text.ToString());
            List<ClsPubLANdetails> assetdetails = (List<ClsPubLANdetails>)DeSeriliaze(byteLobs);
            Session["Asset"] = assetdetails;
            grvAssetdetails.DataSource = assetdetails;
            grvAssetdetails.DataBind();

            if (grvAssetdetails.Rows.Count == 0)
            {
                Session["Asset"] = null;
                lblError.Text = "No Asset Details Found";
                grvAssetdetails.DataBind();
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

}
