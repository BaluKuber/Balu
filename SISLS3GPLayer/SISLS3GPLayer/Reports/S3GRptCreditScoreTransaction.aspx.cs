#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Credit Score Transaction Report
/// Created By          :   Sangeetha R
/// Created Date        :   
/// Purpose             :   To show the Credit Score
/// Last Updated By     :   
/// Last Updated Date   :   
/// Reason              :  
/// <Program Summary>
#endregion

#region Namespaces
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
using System.Globalization;
using System.Collections.Generic;
using S3GBusEntity;
using ReportOrgColMgtServicesReference;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
#endregion

public partial class Reports_S3GRptCreditScoreTransaction : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int CompanyId;
    int UserId;
    int LobId;
    int LocationId1;
    int ProductId;
    int ProgramId;
    int LocationId2;
    bool Is_Active;
    public string strDateFormat;
    //string strAcc_Rej = "";
    Dictionary<string, string> Procparam;
    string strPageName = "Credit Score Transaction";
    DataTable dtTable = new DataTable();
    ReportOrgColMgtServicesClient objSerClient;
    ReportAccountsMgtServicesClient ObjAccClient;
   
    #endregion

    #region Page Load

    /// <summary>
    /// This Event is called when page is Loding
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVCreditScore.ErrorMessage = "Due to Data Problem, Unable to Load.";
            CVCreditScore.IsValid = false;
        }
    }
    #endregion

    #region Page Methods
    /// <summary>
    /// This Method is called when page is Loding
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            #region Application Standard Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
            /* Changed Date Control start - 30-Nov-2012 */
            //txtStartDate.Attributes.Add("readonly", "readonly");
            //txtEndDate.Attributes.Add("readonly", "readonly");
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */
            #endregion
            ProgramId = 134;

            objSerClient = new ReportOrgColMgtServicesClient();
            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            UserId = ObjUserInfo.ProUserIdRW;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + "  " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

            if (!IsPostBack)
            {
                ClearSession();
                FunPriLoadLob(CompanyId, UserId, ProgramId);
                FunPriLoadBranch();
                FunPriLoadLocation();
                ddllocation2.Enabled = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Credit Score Transaction Page");
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Load Line of Business in DropDown List
    /// </summary>
    /// <param name="Company_id"></param>
    /// <param name="User_id"></param>
    /// <param name="Program_id"></param>
    private void FunPriLoadLob(int Company_id, int User_id, int Program_id)
    {
        try
        {
            ObjAccClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs=ObjAccClient.FunPubSOAGetLOB(Company_id,User_id,Program_id);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            ddlLOB.Focus();
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
            }
            else
                ddlLOB.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load LOB");
        }
        finally
        {
            ObjAccClient.Close();
        }
    }

    /// <summary>
    ///  To Load Branch in Dropdown List
    /// </summary>
    private void FunPriLoadBranch()
    {
        try
        {
            ObjAccClient = new ReportAccountsMgtServicesClient();
            ddlBranch.Items.Clear();
            if (ddlLOB.SelectedIndex > 0)
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = ObjAccClient.FunPubBranch(CompanyId, UserId, ProgramId, LobId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--All--";
            if (ddlBranch.Items.Count == 2)
            {
                ddlBranch.SelectedIndex = 1;
            }
            else
                ddlBranch.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Branch");
        }
        finally
        {
            ObjAccClient.Close();
        }
    }

    /// <summary>
    ///  To Load Branch in Dropdown List
    /// </summary>
    private void FunPriLoadLocation()
    {
        try
        {
            ObjAccClient = new ReportAccountsMgtServicesClient();
            ddllocation2.Items.Clear();
            if (ddlLOB.SelectedIndex > 0)
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = ObjAccClient.FunPubBranch(CompanyId, UserId, ProgramId, LobId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddllocation2.DataSource = Branch;
            ddllocation2.DataTextField = "Description";
            ddllocation2.DataValueField = "ID";
            ddllocation2.DataBind();
            ddllocation2.Items[0].Text = "--All--";
            if (ddllocation2.Items.Count == 2)
            {
                ddllocation2.SelectedIndex = 1;
            }
            else
                ddllocation2.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Branch");
        }
        finally
        {
            ObjAccClient.Close();
        }
    }

    /// <summary>
    /// To Load Location 2
    /// </summary>
    /// <param name="ProgramId"></param>
    /// <param name="UserId"></param>
    /// <param name="CompanyId"></param>
    /// <param name="LobId"></param>
    /// <param name="LocationId"></param>
    private void FunPriLoadLocation2(int ProgramId, int UserId, int CompanyId, int LobId, int LocationId1)
    {
        try
        {
            ObjAccClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }

            if (ddlBranch.SelectedIndex > 0)
            {
                LocationId1 = Convert.ToInt32(ddlBranch.SelectedValue);
            }
            byte[] byteLobs = ObjAccClient.FunPubGetLocation2(ProgramId, UserId, CompanyId, LobId, LocationId1);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddllocation2.DataSource = Branch;
            ddllocation2.DataTextField = "Description";
            ddllocation2.DataValueField = "ID";
            ddllocation2.DataBind();
            ddllocation2.Items[0].Text = "--All--";
            if (ddllocation2.Items.Count == 2)
            {
                ddllocation2.SelectedIndex = 1;
            }
            else
                ddllocation2.SelectedIndex = 0;

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

    /// <summary>
    /// To Load Product in Dropdown List
    /// </summary>
    /// <param name="Company_id"></param>
    /// <param name="LOBId"></param>
    private void FunPriLoadProduct(int Company_id, int LobId)
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            //LobId=Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetProduct(CompanyId, LobId);
            List<ClsPubDropDownList> product = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlProduct.DataSource = product;
            ddlProduct.DataTextField = "Description";
            ddlProduct.DataValueField = "ID";
            ddlProduct.DataBind();
            if (ddlProduct.Items.Count == 2)
            {
                ddlProduct.SelectedIndex = 1;
            }
            else
                ddlProduct.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Product");
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Bind the Credit Score Details Grid
    /// </summary>
    private void FunBindgrid()
    {
        try
        {
            pnlcreditscoredetails.Visible = true;
            ClsPubCredit ObjCredit = new ClsPubCredit();
            ObjCredit.CompanyId = CompanyId;
            ObjCredit.UserId = UserId;
            ObjCredit.LOBId = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjCredit.LocationId1 = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjCredit.LocationId2 = Convert.ToInt32(ddllocation2.SelectedValue);
            ObjCredit.ProductId = Convert.ToInt32(ddlProduct.SelectedValue);
            ObjCredit.Start_Date = Utility .StringToDate (txtStartDate.Text).ToString ();
            ObjCredit.End_Date = Utility.StringToDate(txtEndDate.Text).ToString();
            byte[] byteLobs = objSerClient.FunPubGetCreditScoreDetails(ObjCredit);
            ClsPubCreditScoreTransaction Credit = (ClsPubCreditScoreTransaction)DeSeriliaze(byteLobs);

            Session["Credit"] = Credit;
            Session["LOC"] = Credit;
            grvcreditscoredetails.DataSource = Credit.CreditScoreTrans;
            grvcreditscoredetails.DataBind();
            //if (grvcreditscoredetails.Rows.Count != 0)
            //{
            //    grvcreditscoredetails.HeaderRow.Style.Add("position", "relative");
            //    grvcreditscoredetails.HeaderRow.Style.Add("z-index", "auto");
            //    grvcreditscoredetails.HeaderRow.Style.Add("top", "auto");
            //}

            if (grvcreditscoredetails.Rows.Count == 0)
            {
                grvcreditscoredetails.EmptyDataText = "No Credit Score Details Found";
                grvcreditscoredetails.DataBind();
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

    /// <summary>
    /// To Clear the fields
    /// </summary>
    private void FunRptClear()
    {
        try
        {
            ddlLOB.SelectedValue = "-1";
            ddlBranch.SelectedValue = "-1";
            FunPriLoadBranch();
            if (ddlProduct.Items.Count > 0)
            ddlProduct.Items.Clear();
            FunPriLoadLocation();
            ddllocation2.Enabled = false;
            txtEndDate.Text = "";
            txtStartDate.Text = "";
            pnlcreditscoredetails.Visible = false;
            grvcreditscoredetails.DataSource = null;
            grvcreditscoredetails.DataBind();
            pnlcustomersdetails.Visible = false;
            grvCustDet.DataSource = null;
            grvCustDet.DataBind();
            pnlcustomersdetailsRej.Visible = false;
            grvcustdetrej.DataSource = null;
            grvcustdetrej.DataBind();
            lblAmounts.Visible = false;
            lblCurrency.Visible = false;
            btnPrint.Visible = false;
            ClearSession();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To Validate the Dates
    /// </summary>
    private void FunPriValidateFromEndDate()
    {
        grvcreditscoredetails.DataSource = grvCustDet.DataSource = null;
        grvcreditscoredetails.DataBind();
        grvCustDet.DataBind();
        //ClsPubHeaderDetails ObjHeader = new ClsPubHeaderDetails();
        //ClsPubHeaderDetails objhead = new ClsPubHeaderDetails();
        //ObjHeader.Lob =
        Session["LOB"] = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
        //if ((ddlBranch.Items.Count > 1) && (ddlBranch.SelectedValue != "-1"))
        //{
        //    ObjHeader.Branch = ddlBranch.SelectedItem.ToString();
        //}
        //else
        //{
        //    ObjHeader.Branch = "ALL";
        //}
        if ((ddlProduct.Items.Count > 1)&& (ddlProduct.SelectedValue != "-1"))
        {
            Session["PROD"] = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();
            //Session["PROD"] = ddlProduct.SelectedItem.ToString();
        }
        else
        {
            Session["PROD"] = "ALL";
        }
        Session["StartDate"] = txtStartDate.Text;
        Session["EndDate"] = txtEndDate.Text;
        //Session["Title"] = "Credit Score Transaction for the Period" + " " + txtStartDate.Text + " " + "to" + " " + txtEndDate.Text;
        //Session["Header1"] = ObjHeader;
        //if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()))) &&
        if ((!(string.IsNullOrEmpty(txtStartDate.Text))) &&
           (!(string.IsNullOrEmpty(txtEndDate.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtStartDate.Text) > Utility.StringToDate(txtEndDate.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to the Start Date");
                txtEndDate.Text = "";
                return;
            }
        }
        if ((!(string.IsNullOrEmpty(txtStartDate.Text))) &&
           ((string.IsNullOrEmpty(txtEndDate.Text))))
        {
            txtEndDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        }
        if (((string.IsNullOrEmpty(txtStartDate.Text))) &&
            (!(string.IsNullOrEmpty(txtEndDate.Text))))
        {
            txtStartDate.Text = txtEndDate.Text;

        }
        FunBindgrid();
    }


    /// <summary>
    /// To Clear the Session
    /// </summary>
    private void ClearSession()
    {
        Session["Credit"] = null;
        Session["Header1"] = null;
        Session["SESSION_CRPT_2"] = null;
        Session["SESSION_CRPT_3"] = null;
        Session["Date"] = null;
    } 

    /// <summary>
    /// To Deseriliaze the service Object
    /// </summary>
    /// <param name="byteObj"></param>
    /// <returns></returns>
    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    #endregion

    #region Page Events

    #region DropdownList
    /// <summary>
    /// To Load the Product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        lblCurrency.Visible = false;
        lblAmounts.Visible = false;
        pnlcreditscoredetails.Visible = false;
        pnlcustomersdetails.Visible = false;
        pnlcustomersdetailsRej.Visible = false;
        btnPrint.Visible = false;
        LobId = Convert.ToInt32(ddlLOB.SelectedValue);
        FunPriLoadProduct(CompanyId, LobId);
        FunPriLoadBranch();
        FunPriLoadLocation();
        ddllocation2.Enabled = false;
        grvcreditscoredetails.DataSource = grvCustDet.DataSource = grvcustdetrej.DataSource = null;
        grvcreditscoredetails.DataBind();
        grvCustDet.DataBind();
        grvcustdetrej.DataBind();
        if (ddlLOB.SelectedValue == "-1")
        {
            ddlProduct.Items.Clear();
            ddlBranch.ClearDropDownList();
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            lblCurrency.Visible = false;
            lblAmounts.Visible = false;
            pnlcreditscoredetails.Visible = false;
            pnlcustomersdetails.Visible = false;
            btnPrint.Visible = false;
            grvcreditscoredetails.DataSource = grvCustDet.DataSource = grvcustdetrej.DataSource = null;
            grvcreditscoredetails.DataBind();
            grvCustDet.DataBind();
            grvcustdetrej.DataBind();
        }
    }

    /// <summary>
    /// To Load Location 2 based on Location 1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedValue == "-1")
        {
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId1);
            ddllocation2.Enabled = false;
        }
        else
        {
            ddllocation2.Enabled = true;
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId1);
        }
    }

    ///// <summary>
    ///// To Validate other fields based on the Product
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    lblCurrency.Visible = false;
    //    lblAmounts.Visible = false;
    //    pnlcreditscoredetails.Visible = false;
    //    pnlcustomersdetails.Visible = false;
    //    btnPrint.Visible = false;
    //    grvcreditscoredetails.DataSource = grvCustDet.DataSource = null;
    //    grvcreditscoredetails.DataBind();
    //    grvCustDet.DataBind();
    //    if (ddlProduct.SelectedValue == "-1")
    //    {
    //        lblCurrency.Visible = false;
    //        lblAmounts.Visible = false;
    //        pnlcreditscoredetails.Visible = false;
    //        pnlcustomersdetails.Visible = false;
    //        btnPrint.Visible = false;
    //        grvcreditscoredetails.DataSource = grvCustDet.DataSource = null;
    //        grvcreditscoredetails.DataBind();
    //        grvCustDet.DataBind();
    //    }
    //}
    #endregion

    #region Button(Accepted/Rejected/Go/Clear/Print)
    /// <summary>
    /// To Bind the Activated Accounts
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LnkBtnAccp_Click(object sender, EventArgs e)
    {
        try
        {
            Session["SESSION_CRPT_3"] = null;
            pnlcustomersdetailsRej.Visible = false;
            pnlcustomersdetails.Visible = true;
            lblAmounts.Visible = true;
            lblCurrency.Visible = true;
            lblCurrency.Text = ObjS3GSession.ProCurrencyNameRW;
            ClsPubCredit ObjCustomers = new ClsPubCredit();
            ObjCustomers.CompanyId = CompanyId;
            ObjCustomers.UserId = UserId;
            ObjCustomers.LOBId = Convert.ToInt32(ddlLOB.SelectedValue);
            //int a = grvcreditscoredetails.Rows.Count;
            string strFieldAtt = ((LinkButton)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvcreditscoredetails_")).Replace("grvcreditscoredetails_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;

            Label lblBranchid = (Label)grvcreditscoredetails.Rows[gRowIndex].FindControl("lblBranchid");

            Label lblProductid = (Label)grvcreditscoredetails.Rows[gRowIndex].FindControl("lblProductid");

            Label lblEQC = (Label)grvcreditscoredetails.Rows[gRowIndex].FindControl("lblEQC");

            ObjCustomers.LocationId1 = Convert.ToInt32(lblBranchid.Text);//ddlBranch.SelectedValue);
            ObjCustomers.LocationId2 = Convert.ToInt32(lblBranchid.Text);//ddlBranch.SelectedValue);
            ObjCustomers.ProductId = Convert.ToInt32(lblProductid.Text);//ddlProduct.SelectedValue);
            ObjCustomers.Start_Date = Utility.StringToDate(txtStartDate.Text).ToString();
            ObjCustomers.End_Date = Utility.StringToDate(txtEndDate.Text).ToString();
            if (lblEQC.Text == "ENQUIRY")
            {
                ObjCustomers.Status = 1;
            }
            else if (lblEQC.Text == "CUSTOMER")
            {
                ObjCustomers.Status = 2;
            }

            byte[] byteLobs = objSerClient.FunPubGetCustomersDetails(ObjCustomers);
            List<ClsPubCustomersDetails> Customer = (List<ClsPubCustomersDetails>)DeSeriliaze(byteLobs);
            Session["SESSION_CRPT_2"] = Customer;
            btnPrint.Visible = true;
            grvCustDet.DataSource = Customer;
            grvCustDet.DataBind();
            //if (grvCustDet.Rows.Count != 0)
            //{
            //    grvCustDet.HeaderRow.Style.Add("position", "relative");
            //    grvCustDet.HeaderRow.Style.Add("z-index", "auto");
            //    grvCustDet.HeaderRow.Style.Add("top", "auto");
            //}
            if (grvCustDet.Rows.Count == 0)
            {
                grvCustDet.EmptyDataText = "No Customer Details Found";
                grvCustDet.DataBind();
            }
        }
        catch (Exception ex)
        {
            CVCreditScore.ErrorMessage = "Due to Data Problem, Unable to Load Customer Details.";
            CVCreditScore.IsValid = false;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To bind the Rejected Accounts
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LnkBtnRjtd_Click(object sender, EventArgs e)
    {
        try
        {
            Session["SESSION_CRPT_2"] = null;
            pnlcustomersdetails.Visible = false;
            pnlcustomersdetailsRej.Visible = true;
            lblAmounts.Visible = true;
            lblCurrency.Visible = true;
            lblCurrency.Text = ObjS3GSession.ProCurrencyNameRW;
            ClsPubCredit ObjRejCustomers = new ClsPubCredit();
            ObjRejCustomers.CompanyId = CompanyId;
            ObjRejCustomers.UserId = UserId;
            ObjRejCustomers.LOBId = Convert.ToInt32(ddlLOB.SelectedValue);
            string strFieldAtt = ((LinkButton)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvcreditscoredetails_")).Replace("grvcreditscoredetails_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;

            Label lblBranchid = (Label)grvcreditscoredetails.Rows[gRowIndex].FindControl("lblBranchid");

            Label lblProductid = (Label)grvcreditscoredetails.Rows[gRowIndex].FindControl("lblProductid");

            ObjRejCustomers.LocationId1 = Convert.ToInt32(lblBranchid.Text);//ddlBranch.SelectedValue);
            ObjRejCustomers.LocationId2 = Convert.ToInt32(lblBranchid.Text);//ddlBranch.SelectedValue);
            ObjRejCustomers.ProductId = Convert.ToInt32(lblProductid.Text);
            ObjRejCustomers.Start_Date = Utility.StringToDate(txtStartDate.Text).ToString();
            ObjRejCustomers.End_Date = Utility.StringToDate(txtEndDate.Text).ToString();
            ObjRejCustomers.Status = 8;
            btnPrint.Visible = true;
            byte[] byteLobs = objSerClient.FunPubGetRejCustomersDetails(ObjRejCustomers);
            List<ClsPubCustomersDetails> Customerrej = (List<ClsPubCustomersDetails>)DeSeriliaze(byteLobs);
            Session["SESSION_CRPT_3"] = Customerrej;
            grvcustdetrej.DataSource = Customerrej;
            grvcustdetrej.DataBind();
            //if (grvCustDet.Rows.Count != 0)
            //{
            //    grvCustDet.HeaderRow.Style.Add("position", "relative");
            //    grvCustDet.HeaderRow.Style.Add("z-index", "auto");
            //    grvCustDet.HeaderRow.Style.Add("top", "auto");
    //}
            if (grvcustdetrej.Rows.Count == 0)
            {
                grvcustdetrej.EmptyDataText = "No Customer Details Found";
                grvcustdetrej.DataBind();
            }
        }
        catch (Exception ex)
        {
            CVCreditScore.ErrorMessage = "Due to Data Problem, Unable to Load Customer Details.";
            CVCreditScore.IsValid = false;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Bind the Credit Score Details Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo_Click(object sender, EventArgs e)
    {
        lblAmounts.Visible = false;
        lblCurrency.Visible = false;
        pnlcustomersdetails.Visible = false;
        pnlcustomersdetailsRej.Visible = false;
        btnPrint.Visible = false;
        try
        {
            FunPriValidateFromEndDate();
        }
        catch (Exception ex)
        {
            CVCreditScore.ErrorMessage = "Due to Data Problem, Unable to Load Credit Score/Customer Details Grid";
            CVCreditScore.IsValid = false;
        }
    }

    /// <summary>
    /// To Clear the fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunRptClear();
        }
        catch (Exception ex)
        {
            CVCreditScore.ErrorMessage = "Due to Data Problem, Unable to Clear.";
            CVCreditScore.IsValid = false;
        }
    }

    /// <summary>
    /// To Print Credit Score Transaction Report
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string strScipt = "window.open('../Reports/S3GRptCreditScoreReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CST", strScipt, true);
    }
    #endregion

    #endregion
}