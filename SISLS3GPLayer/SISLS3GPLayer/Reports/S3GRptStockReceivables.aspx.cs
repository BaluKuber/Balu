#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Stocks and Receivables Report
/// Created By          :   Sangeetha R
/// Created Date        :   
/// Purpose             :   To Generate report for the Disbursed Amount Vs Collected Amount for every Account
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
using System.Collections.Generic;
using System.Globalization;
using S3GBusEntity.Reports;
using ReportOrgColMgtServicesReference;
using ReportAccountsMgtServicesReference;
using S3GBusEntity;
#endregion

public partial class Reports_S3GRptStockReceivables : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int CompanyId;
    int UserId;
    bool Is_Active;
    string DDMMYY;
    int Currentmonth;
    int Currentyear;
    int Month;
    int ProgramId;
    int LocationId1;
    int year;
    int LobId;
    public string cutoff_month;
    public string strDateFormat;
    decimal TotalGrossStock;
    decimal TotalUMFC;
    decimal TotalBilledUncollectedPrincipal;
    decimal TotalBilledUncollectedFC;
    decimal TotalNetStock;
    object GrossObject;
    object UMFCObject;
    object BpObject;
    object BFCObject;
    object NetStockObj;
    DataSet ds;
    Dictionary<string, string> Procparam;
    string strPageName = "Stock and Receivables Report";
    DataTable dtTable = new DataTable();
    ReportOrgColMgtServicesClient objSerClient;
    ReportAccountsMgtServicesClient ObjAccClient;
    DataTable dt=new DataTable();
    #endregion

    #region Page Load
    /// <summary>
    /// This event is handled during Page Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //strDateFormat = ObjS3GSession.ProDateFormatRW;
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            ObjS3GSession = new S3GSession();
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVStockRec.ErrorMessage = "Due to Data Problem, Unable to Load Stock Receivables Page.";
            CVStockRec.IsValid = false;
        }
    }
    #endregion

    #region Page Methods
    /// <summary>
    /// This Method is called when page is getting Loading
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            #region Application Standard Date Format
            string today;
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date

            /* Changed Date Control start - 30-Nov-2012 */
            //txtDate.Attributes.Add("readonly", "readonly");
            txtDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDate.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */

            //txtDate.Text = Convert.ToString(DateTime.Now.Date);
            //string today = txtDate.Text;
            //DDMMYY = today.Substring(0, 7);
            //today = DateTime.Now.Date.ToString(strDateFormat);
            //txtDate.Text = today.Substring(0, 11);
            //txtDate.Text = DateTime.Now.Date.ToString(strDateFormat);
            #endregion
            ProgramId = 187;

            objSerClient = new ReportOrgColMgtServicesClient();
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            UserId = ObjUserInfo.ProUserIdRW;

            Session["Date"] = DateTime.Now.ToString(strDateFormat) + "  " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.Date.ToString(strDateFormat);
                ClearSession();
                FunPriLoadLob(CompanyId, UserId, ProgramId);
                ddllocation2.Enabled = false;
                //FunPriLoadBranch(CompanyId, UserId, Is_Active);
                FunPriLoadBranch();
                FunPriLoadLocation();
                FunPubLoadDenomination();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Stock Receivables page");
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Load LOB
    /// </summary>
    /// <param name="Company_id"></param>
    /// <param name="User_id"></param>
    /// <param name="Program_id"></param>
    private void FunPriLoadLob(int Company_id, int User_id, int Program_id)
    {
        try
        {
            ObjAccClient = new ReportAccountsMgtServicesClient();
            ddlLOB.Items.Clear();
            ddlLOB.Focus();
            byte[] byteLobs = ObjAccClient.FunPubLOB(Company_id, User_id, Program_id);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            //ddlLOB.Items[0].Text = "All";
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
            throw ex;
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
            int lob_id = 0;
            if (ddlLOB.SelectedIndex > 0)
                lob_id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = ObjAccClient.FunPubBranch(CompanyId, UserId, ProgramId, lob_id);
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
            int lob_id = 0;
            if (ddlLOB.SelectedIndex > 0)
                lob_id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = ObjAccClient.FunPubBranch(CompanyId, UserId, ProgramId, lob_id);
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
    /// To Load Denomination
    /// </summary>
    public void FunPubLoadDenomination()
    {
        try
        {
            ObjAccClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = ObjAccClient.GetDenominations();
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
            ObjAccClient.Close();
        }
    }

    private void FunPriClearStock()
    {
        try
        {
            ddlLOB.Focus();
            //ddlLOB.ClearSelection();
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
            }
            else
                ddlLOB.SelectedIndex = 0;
            if (ddlBranch.Items.Count == 2)
            {
                ddlBranch.SelectedIndex = 1;
            }
            else
                ddlBranch.SelectedIndex = 0;
            //ddlBranch.ClearSelection();
            FunPriLoadBranch();
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId1);
            ddllocation2.Enabled = false;
            ddlDenomination.ClearSelection();
            txtDate.Text = DateTime.Now.Date.ToString(strDateFormat);
            //ddlTeam.ClearSelection();
            //ddlCSP.ClearSelection();
            //txtDate.Text = "";
            FunPriValidateGrid();
            ddlCustomerReference.SelectedIndex = 0;
            ClearSession();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To Clear Session
    /// </summary>
    private void ClearSession()
    {
        Session["Header"] = null;
        Session["Denomination"] = null;
        Session["StockReceivables"] = null;
        Session["LOB"] = null;
        Session["Date"] = null;
        Session["Report"] = null;
    }
    private void Clear()
    {
        pnlcontractwise.Visible = false;
        pnlStockReceivables.Visible = false;
        pnlgroup.Visible = false;
        pnlIndustry.Visible = false;
        pnlDetails.Visible = false;
        grvStockRec.DataSource = null;
        grvStockRec.DataBind();
        grvcontract.DataSource = null;
        grvcontract.DataBind();
        grvgroupwise.DataSource = null;
        grvgroupwise.DataBind();
        grvindustry.DataSource = null;
        grvindustry.DataBind();
        grvdetails.DataSource = null;
        grvdetails.DataBind();
        btnPrint.Visible = false;
        
    }

    /// <summary>
    /// To Validate Grid
    /// </summary>
    private void FunPriValidateGrid()
    {
        pnlStockReceivables.Visible = false;
        grvStockRec.Visible = false;
        grvStockRec.DataBind();
        btnPrint.Visible = false;
        lblAmounts.Visible = false;
        //lblCurrency.Visible = false;
    }

    /// <summary>
    /// To Validate Future Date
    /// </summary>
    private void FunPriValidateFutureDate()
    {
        try
        {
            string today;
            //#region To find Current Year and Month
            ////string Today = Convert.ToString(DateTime.Now);
            //string YearMonth = txtDate.Text;
            //int Currentmonth = DateTime.Now.Month;
            //int Currentyear = DateTime.Now.Year;
            //#endregion

            //int Month = int.Parse(YearMonth.Substring(0, 2));
            //int year = int.Parse(YearMonth.Substring(3, 4));
            //if (year > Currentyear || Month > Currentmonth)
            //{
            //    txtDate.Text = "";
            //    Utility.FunShowAlertMsg(this, "Month/Year cannot be Greater than System month/Year.");
            //    return;
            //}
            ClsPubHeaderDetails ObjHeader = new ClsPubHeaderDetails();
            ClsPubStockReceivableDetails objstocks = new ClsPubStockReceivableDetails();
            if (ddlLOB.Items.Count > 1)
            {
                objstocks.LOB = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            }
            //if (ddlLOB.Items.Count > 1)
            //{
            //    ObjHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            //}
            //else
            //{
            //    objstocks.LOB = "ALL";
            //}
            //if ((ddlBranch.Items.Count > 1) && (ddlBranch.SelectedValue != "-1"))
            //{
            //    ObjHeader.Branch = (ddlBranch.SelectedItem.Text.Split('-'))[1].ToString();
            //}
            //else
            //{
            //    ObjHeader.Branch = "ALL";
            //}
            ObjHeader.Date = txtDate.Text;
            //today = DateTime.Now.Date.ToString(strDateFormat);
            //txtDate.Text = today.Substring(0, 11);
            //txtDate.Text = DateTime.Now.Date.ToString(strDateFormat);
            //txtDate.Text = today.Substring(0, 11);
            Session["Header"] = ObjHeader;
            if (ddlDenomination.SelectedValue == "1")
            {
                Session["Denomination"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Denomination"] = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            lblAmounts.Visible = true;
            if (ddlDenomination.SelectedValue == "1")
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                lblAmounts.Text = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            string s1;

            s1 = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();
            if (s1 == "LN")
                Session["LOB"] = "Gross Investment in Loan";
            else if (s1 == "TL")
                Session["LOB"] = "Gross Investment in Loan";
            else if (s1 == "FL")
                Session["LOB"] = "Gross Investment in Lease";
            else if (s1 == "OL")
                Session["LOB"] = "Gross Investment in Lease";
            else if (s1 == "HP")
                Session["LOB"] = "Gross Stock in Hire";
            //else if (s1 == "All")
            //    Session["LOB"] = "Gross Stock";
            else
                Session["LOB"] = "Gross Stock";

            //if (year == Currentyear && Month == Currentmonth)
            //{
            //    txtDate.Text = "";
            //    Utility.FunShowAlertMsg(this, "Month cannot be Greater than current month");
            //    return;
            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //private void FunPriLoadStockReceivablesDetails()
    //{
    //    try
    //    {
    //        lblAmounts.Visible = true;
    //        //lblCurrency.Visible = true;
    //        //lblCurrency.Text = ObjS3GSession.ProCurrencyNameRW;

           
   
    //        if (ddlCustomerReference.SelectedIndex == 0)
    //        {
    //            btnPrint.Visible = true;
    //            btnPrint.Enabled = true;
    //            pnlStockRec.Visible = true;
    //            grvCustomer.Visible = true;

    //            objSerClient = new ReportOrgColMgtServicesClient();
    //            ClsPubStockReceivableparameters StockRecParam = new ClsPubStockReceivableparameters();
    //            StockRecParam.CompanyId = CompanyId;
    //            StockRecParam.UserId = UserId;
    //            StockRecParam.LobId = ddlLOB.SelectedValue;
    //            if (ddlBranch.SelectedIndex != 0)
    //            {
    //                StockRecParam.LocationId1 = ddlBranch.SelectedValue;
    //            }
    //            else
    //            {
    //                StockRecParam.LocationId1 = "";
    //            }
    //            if (ddlBranch.SelectedIndex != 0)
    //            {
    //                StockRecParam.LocationId2 = ddllocation2.SelectedValue;
    //            }
    //            else
    //            {
    //                StockRecParam.LocationId2 = "";
    //            }
    //            //StockRecParam.StartDate = FirstDate;
    //            StockRecParam.CUST_ID = 0;
    //            StockRecParam.REFERENCE_ID = 0;
    //            StockRecParam.EndDate = Utility.StringToDate(txtDate.Text).ToString();
    //            StockRecParam.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
    //            //strDateFormat(txtDate.Text);
    //            //StockRecParam.FromMonth = FromMonth.ToString();
    //            //StockRecParam.ToMonth = ToMonth.ToString();

    //            byte[] byteLobs = objSerClient.FunPubGetStockReceivablesDetails(StockRecParam);
    //            List<ClsPubStockReceivableDetails> StockReceivableDetail = (List<ClsPubStockReceivableDetails>)DeSeriliaze(byteLobs);
    //            TotalGrossStock = StockReceivableDetail.Sum(ClsPubStockReceivableDetails => ClsPubStockReceivableDetails.GrossStock);
    //            TotalUMFC = StockReceivableDetail.Sum(ClsPubStockReceivableDetails => ClsPubStockReceivableDetails.UMFC);
    //            TotalBilledUncollectedPrincipal = StockReceivableDetail.Sum(ClsPubStockReceivableDetails => ClsPubStockReceivableDetails.BilledUncollectedPrincipal);
    //            TotalBilledUncollectedFC = StockReceivableDetail.Sum(ClsPubStockReceivableDetails => ClsPubStockReceivableDetails.BilledUncollectedFC);
    //            TotalNetStock = StockReceivableDetail.Sum(ClsPubStockReceivableDetails => ClsPubStockReceivableDetails.NetStock);
    //            Session["StockReceivable"] = StockReceivableDetail;
    //            grvCustomer.DataSource = StockReceivableDetail;
    //            grvCustomer.DataBind();
    //            if (grvCustomer.Rows.Count > 0)
    //            {
    //                ((Label)grvCustomer.FooterRow.FindControl("lbltotGrossStock")).Text = TotalGrossStock.ToString();
    //                ((Label)grvCustomer.FooterRow.FindControl("lbltotUMFC")).Text = TotalUMFC.ToString();
    //                ((Label)grvCustomer.FooterRow.FindControl("lbltotBP")).Text = TotalBilledUncollectedPrincipal.ToString();
    //                ((Label)grvCustomer.FooterRow.FindControl("lbltotBFC")).Text = TotalBilledUncollectedFC.ToString();
    //                ((Label)grvCustomer.FooterRow.FindControl("lbltotNetStock")).Text = TotalNetStock.ToString();
                    
    //            }
    //        }
    //        else
    //        {
    //            pnlStockReceivables.Visible = false;
    //            grvCustomer.DataSource = "";
    //            grvCustomer.DataBind();
    //            //btnPrint.Visible = true;
    //            //btnPrint.Enabled = true;
    //            pnlStockRec.Visible = true;
    //            grvStockRec.Visible = true;
    //            Procparam = new Dictionary<string, string>();
    //            Procparam.Add("@COMPANY_ID", CompanyId.ToString());
    //            Procparam.Add("@USER_ID", UserId.ToString());
    //            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
    //            Procparam.Add("@LOCATION_ID1", ddlBranch.SelectedValue);
    //            if (ddllocation2.SelectedIndex != 0)
    //            {
    //                Procparam.Add("@LOCATION_ID2", ddllocation2.SelectedValue);
    //            }
    //            Procparam.Add("@ENDDATE", (Utility.StringToDate(txtDate.Text)).ToString());
    //            Procparam.Add("@DENOMINATION", ddlDenomination.SelectedValue);
    //            Procparam.Add("@ReferencePoint", ddlCustomerReference.SelectedValue);
              
    //            DataTable dtGroup = Utility.GetDefaultData("S3G_RPT_STOCKRECEIVABLEDETAILSNEW_Group", Procparam);
    //            if (dtGroup.Rows.Count > 0)
    //            {
    //                pnlStockReceivables.Visible = true;
    //                Session["StockReceivable"] = dtGroup;
    //                grvStockRec.DataSource = dtGroup; 
    //                grvStockRec.DataBind();
    //                if (grvStockRec.Rows.Count > 0)
    //                {

    //                    Label lbltotGrossStock = (Label)grvStockRec.FooterRow.FindControl("lbltotGrossStock");
    //                    Label lbltotUMFC = (Label)grvStockRec.FooterRow.FindControl("lbltotUMFC");
    //                    Label lbltotBP = (Label)grvStockRec.FooterRow.FindControl("lbltotBP");
    //                    Label lbltotBFC = (Label)grvStockRec.FooterRow.FindControl("lbltotBFC");
    //                    Label lbltotNetStock = (Label)grvStockRec.FooterRow.FindControl("lbltotNetStock");

    //                    //Label lblGrossStock = (Label)grvCustomer.FindControl("lblGrossStock");
    //                    GrossObject = UMFCObject = BpObject = BFCObject = null;
    //                    GrossObject = dtGroup.Compute("sum(GrossStock)", "GrossStock >= 0");
    //                    lbltotGrossStock.Text = Convert.ToString(GrossObject);

    //                    UMFCObject = dtGroup.Compute("sum(UMFC)", "UMFC >= 0");
    //                    lbltotUMFC.Text = Convert.ToString(UMFCObject);

    //                    BpObject = dtGroup.Compute("sum(BilledUncollectedPrincipal)", "BilledUncollectedPrincipal >= 0");
    //                    lbltotBP.Text = Convert.ToString(BpObject);

    //                    BFCObject = dtGroup.Compute("sum(BilledUncollectedFC)", "BilledUncollectedFC >= 0");
    //                    lbltotBFC.Text = Convert.ToString(BFCObject);

    //                    //NetStockObj = dtGroup.Compute("NetStock", "NetStock > 0");
    //                    //lbltotNetStock.Text = Convert.ToString(NetStockObj);

    //                }
    //                else
    //                {
    //                    btnPrint.Enabled = false;
    //                    Session["StockReceivable"] = null;
    //                    grvStockRec.EmptyDataText = "No Records found";
    //                    grvStockRec.DataBind();
    //                    pnlStockReceivables.Visible = false;
    //                    grvCustomer.DataSource = null;
    //                    grvCustomer.DataBind();
    //                }
                    
    //                //FunPriDisplayTotal();
    //            }
    //        }
    //        //if (grvStockRec.Rows.Count != 0)
    //        //{
    //        //    grvStockRec.HeaderRow.Style.Add("position", "relative");
    //        //    grvStockRec.HeaderRow.Style.Add("z-index", "auto");
    //        //    grvStockRec.HeaderRow.Style.Add("top", "auto");
    //        //}
    //        if (grvStockRec.Rows.Count == 0 && grvCustomer.Rows.Count == 0)
    //        {
    //            btnPrint.Enabled = false;
    //            Session["StockReceivable"] = null;
    //            grvStockRec.EmptyDataText = "No Records found";
    //            grvStockRec.DataBind();
    //            pnlStockReceivables.Visible = false;
    //            grvCustomer.DataSource = null;
    //            grvCustomer.DataBind();
    //        }
    //        else
    //        {
                
    //        }
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


    private void FunPriLoadStockReceivablesDetails()
    {
        try
        {
            ds = new DataSet();
            Session["DT_Grid"] = null;
            btnPrint.Visible = true;
            btnPrint.Enabled = true;
            pnlStockReceivables.Visible = false;
            pnlcontractwise.Visible = false;
            pnlgroup.Visible = false;
            grvStockRec.DataSource = null;
            grvStockRec.DataBind();
            grvcontract.DataSource = null;
            grvcontract.DataBind();
            grvgroupwise.DataSource = null;
            grvgroupwise.DataBind();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID",CompanyId.ToString());
            Procparam.Add("@User_ID", UserId.ToString());
            Procparam.Add("@lob_id", ddlLOB.SelectedValue);
            Procparam.Add("@location_id1", ddlBranch.SelectedValue);
            Procparam.Add("@location_id2", ddllocation2.SelectedValue);
            Procparam.Add("@ENDDATE", Utility.StringToDate(txtDate.Text.Trim()).ToString());
            Procparam.Add("@DENOMINATION", ddlDenomination.SelectedValue);
            Procparam.Add("@REFERENCE_ID", ddlCustomerReference.SelectedValue);
            dt = Utility.GetDefaultData("S3G_RPT_SRReport_CUST", Procparam);
            if (ddlCustomerReference.SelectedValue == "1")
            {
                pnlStockReceivables.Visible = true;
                grvStockRec.Visible = true;
                grvStockRec.DataSource = dt;
                grvStockRec.DataBind();
                Session["DT_Grid"] = dt;
                Label lbltotGrossPrincipal = (Label)(grvStockRec).FooterRow.FindControl("lbltotGrossPrincipal");
                lbltotGrossPrincipal.Text =Convert.ToDecimal(dt.Compute("sum(Gross_principal)","")).ToString(Funsetsuffix());
                Label lbltotUMFC = (Label)(grvStockRec).FooterRow.FindControl("lbltotUMFC");
                lbltotUMFC.Text = Convert.ToDecimal(dt.Compute("sum(UMFC)","")).ToString(Funsetsuffix());
                Label lbltotgrossstock = (Label)(grvStockRec).FooterRow.FindControl("lbltotgrossstock");
                lbltotgrossstock.Text = Convert.ToDecimal(dt.Compute("sum(gross_stock)","")).ToString(Funsetsuffix());
                Label lbltotNetPrincipal = (Label)(grvStockRec).FooterRow.FindControl("lbltotNetPrincipal");
                lbltotNetPrincipal.Text = Convert.ToDecimal(dt.Compute("sum(Net_Principal)","")).ToString(Funsetsuffix());
                Label lbltotInterestOS = (Label)(grvStockRec).FooterRow.FindControl("lbltotInterestOS");
                lbltotInterestOS.Text = Convert.ToDecimal(dt.Compute("sum(Interest_OS)","")).ToString(Funsetsuffix());
                Label lbltotfutureInstallments = (Label)(grvStockRec).FooterRow.FindControl("lbltotfutureInstallments");
                lbltotfutureInstallments.Text = Convert.ToDecimal(dt.Compute("sum(future_instal)","")).ToString(Funsetsuffix());
                Label lbltotnetstock = (Label)(grvStockRec).FooterRow.FindControl("lbltotnetstock");
                lbltotnetstock.Text = Convert.ToDecimal(dt.Compute("sum(net_stock)","")).ToString(Funsetsuffix());

                
            }
            else if (ddlCustomerReference.SelectedValue == "0")
            {
                pnlcontractwise.Visible = true;
                grvcontract.Visible = true;
                grvcontract.DataSource = dt;
                grvcontract.DataBind();
                grvcontract.Columns[16].Visible = false;
                Session["DT_Grid"] = dt;
                Label lblconttotGrossPrincipal = (Label)(grvcontract).FooterRow.FindControl("lblconttotGrossPrincipal");
                lblconttotGrossPrincipal.Text = Convert.ToDecimal(dt.Compute("sum(Gross_principal)", "")).ToString(Funsetsuffix());
                Label lblconttotUMFC = (Label)(grvcontract).FooterRow.FindControl("lblconttotUMFC");
                lblconttotUMFC.Text = Convert.ToDecimal(dt.Compute("sum(UMFC)", "")).ToString(Funsetsuffix());
                Label lblconttotgrossstock = (Label)(grvcontract).FooterRow.FindControl("lblconttotgrossstock");
                lblconttotgrossstock.Text = Convert.ToDecimal(dt.Compute("sum(gross_stock)", "")).ToString(Funsetsuffix());
                Label lblconttotNetPrincipal = (Label)(grvcontract).FooterRow.FindControl("lblconttotNetPrincipal");
                lblconttotNetPrincipal.Text = Convert.ToDecimal(dt.Compute("sum(Net_Principal)", "")).ToString(Funsetsuffix());
                Label lblconttotInterest_OS = (Label)(grvcontract).FooterRow.FindControl("lblconttotInterest_OS");
                lblconttotInterest_OS.Text = Convert.ToDecimal(dt.Compute("sum(Interest_OS)", "")).ToString(Funsetsuffix());
                Label lblconttotfutureInstallments = (Label)(grvcontract).FooterRow.FindControl("lblconttotfutureInstallments");
                lblconttotfutureInstallments.Text = Convert.ToDecimal(dt.Compute("sum(future_instal)", "")).ToString(Funsetsuffix());
                Label lblconttotnetstock = (Label)(grvcontract).FooterRow.FindControl("lblconttotnetstock");
                lblconttotnetstock.Text = Convert.ToDecimal(dt.Compute("sum(net_stock)", "")).ToString(Funsetsuffix());

            }
            else if (ddlCustomerReference.SelectedValue == "2")
            {
                pnlgroup.Visible = true;
                grvgroupwise.Visible = true;
                grvgroupwise.DataSource = dt;
                grvgroupwise.DataBind();
                Session["DT_Grid"] = dt;
                Label lblgptotGrossPrincipal = (Label)(grvgroupwise).FooterRow.FindControl("lblgptotGrossPrincipal");
                lblgptotGrossPrincipal.Text = Convert.ToDecimal(dt.Compute("sum(Gross_principal)", "")).ToString(Funsetsuffix());
                Label lblgptotUMFC = (Label)(grvgroupwise).FooterRow.FindControl("lblgptotUMFC");
                lblgptotUMFC.Text = Convert.ToDecimal(dt.Compute("sum(UMFC)","")).ToString(Funsetsuffix());
                Label lblgptotgrossstock = (Label)(grvgroupwise).FooterRow.FindControl("lblgptotgrossstock");
                lblgptotgrossstock.Text = Convert.ToDecimal(dt.Compute("sum(gross_stock)", "")).ToString(Funsetsuffix());
                Label lblgptotNetPrincipal = (Label)(grvgroupwise).FooterRow.FindControl("lblgptotNetPrincipal");
                lblgptotNetPrincipal.Text = Convert.ToDecimal(dt.Compute("sum(Net_Principal)", "")).ToString(Funsetsuffix());
                Label lblgptotInterestOS = (Label)(grvgroupwise).FooterRow.FindControl("lblgptotInterestOS");
                lblgptotInterestOS.Text = Convert.ToDecimal(dt.Compute("sum(Interest_OS)", "")).ToString(Funsetsuffix());
                Label lblgptotfutureInstallments = (Label)(grvgroupwise).FooterRow.FindControl("lblgptotfutureInstallments");
                lblgptotfutureInstallments.Text = Convert.ToDecimal(dt.Compute("sum(future_instal)", "")).ToString(Funsetsuffix());
                Label lblgptotnetstock = (Label)(grvgroupwise).FooterRow.FindControl("lblgptotnetstock");
                lblgptotnetstock.Text = Convert.ToDecimal(dt.Compute("sum(net_stock)", "")).ToString(Funsetsuffix());
            }
            else if (ddlCustomerReference.SelectedValue == "3")
            {
                pnlIndustry.Visible = true;
                grvindustry.Visible = true;
                grvindustry.DataSource = dt;
                grvindustry.DataBind();
                Session["DT_Grid"] = dt;
                Label lblindustrytotGrossPrincipal = (Label)(grvindustry).FooterRow.FindControl("lblindustrytotGrossPrincipal");
                lblindustrytotGrossPrincipal.Text =Convert.ToDecimal(dt.Compute("sum(Gross_principal)", "")).ToString(Funsetsuffix());
                Label lblindustrytotUMFC = (Label)(grvindustry).FooterRow.FindControl("lblindustrytotUMFC");
                lblindustrytotUMFC.Text = Convert.ToDecimal(dt.Compute("sum(UMFC)", "")).ToString(Funsetsuffix());
                Label lblindustrytotgrossstock = (Label)(grvindustry).FooterRow.FindControl("lblindustrytotgrossstock");
                lblindustrytotgrossstock.Text = Convert.ToDecimal(dt.Compute("sum(gross_stock)", "")).ToString(Funsetsuffix());
                Label lblindustrytotNetPrincipal = (Label)(grvindustry).FooterRow.FindControl("lblindustrytotNetPrincipal");
                lblindustrytotNetPrincipal.Text = Convert.ToDecimal(dt.Compute("sum(Net_Principal)", "")).ToString(Funsetsuffix());
                Label lblindustrytotindustryNIC = (Label)(grvindustry).FooterRow.FindControl("lblindustrytotindustryNIC");
                lblindustrytotindustryNIC.Text = Convert.ToDecimal(dt.Compute("sum(Interest_OS)", "")).ToString(Funsetsuffix());
                Label lblindustrytotfutureInstallments = (Label)(grvindustry).FooterRow.FindControl("lblindustrytotfutureInstallments");
                lblindustrytotfutureInstallments.Text = Convert.ToDecimal(dt.Compute("sum(future_instal)", "")).ToString(Funsetsuffix());
                Label lblindustrytotnetstock = (Label)(grvindustry).FooterRow.FindControl("lblindustrytotnetstock");
                lblindustrytotnetstock.Text = Convert.ToDecimal(dt.Compute("sum(net_stock)", "")).ToString(Funsetsuffix());
            }

            ds.Tables.Add(dt);
           ds.Tables.Add(FunPriHDR_DT());
           
            ds.Tables[0].TableName = "Stock";
            //ds.Tables[1].TableName = "Detail";
            
            Session["Report"] = ds;
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
        if (grvStockRec.Rows.Count > 0)
        {
            ((Label)grvStockRec.FooterRow.FindControl("lbltotGrossStock")).Text = TotalGrossStock.ToString();
            ((Label)grvStockRec.FooterRow.FindControl("lbltotUMFC")).Text = TotalUMFC.ToString();
            ((Label)grvStockRec.FooterRow.FindControl("lbltotBP")).Text = TotalBilledUncollectedPrincipal.ToString();
            ((Label)grvStockRec.FooterRow.FindControl("lbltotBFC")).Text = TotalBilledUncollectedFC.ToString();
            ((Label)grvStockRec.FooterRow.FindControl("lbltotNetStock")).Text = TotalNetStock.ToString();
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

    #region DropDownList and textBox Events

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Clear();
            ClearSession();
            FunPriLoadBranch();
            if (ddlLOB.SelectedValue == "-1")
            {
                //ddlBranch.Items.Clear();
                ddlBranch.ClearSelection();
                //txtDate.Text = "";
            }
            //ddlBranch.SelectedValue == "-1";
            //lblAmounts.Visible = false;
            //ddlBranch.
            ddlBranch.ClearSelection();
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId1);
            ddllocation2.Enabled = false;
            txtDate.Text = DateTime.Now.Date.ToString(strDateFormat);
            //txtDate.Text = "";
            if (pnlStockReceivables.Visible == true)
            {
                FunPriValidateGrid();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlCustomerReference_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Clear();
            ClearSession();
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// To Load Location 2 based on Branch
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBranch.SelectedValue == "-1")
        {
            Clear();
            ClearSession();
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId1);
            ddllocation2.Enabled = false;
        }
        else
        {
            ddllocation2.Enabled = true;
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId1);
        }
    }

    /// <summary>
    /// Validating the Date field
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void txtDate_OnTextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunPriValidateFutureDate();
    //    }
    //    catch (Exception ex)
    //    {
    //        CVStockRec.ErrorMessage = "Unable to Validate Future Date";
    //        CVStockRec.IsValid = false;
    //    }
    //}

    /// <summary>
    /// To Validate the Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDenomination_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Clear();
            ClearSession();
        }
        catch (Exception ex)
        {
            CVStockRec.ErrorMessage = "Unable to Validate Grid.";
            CVStockRec.IsValid = false;
        }
    }

    #endregion

    #region Button(Ok / Clear / Print)

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
            ClearSession();
            FunPriLoadStockReceivablesDetails();
            FunPriValidateFutureDate();
        }
        catch (Exception ex)
        {
            CVStockRec.ErrorMessage = "Due to Data Problem Unable to Load the Stock Receivables Details Grid";
            CVStockRec.IsValid = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
            FunPriClearStock();
        }
        catch (Exception ex)
        {
            CVStockRec.ErrorMessage = "Unable to Clear";
            CVStockRec.IsValid = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Denomination"] = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            Session["id"] = ddlCustomerReference.SelectedValue.ToString();
            Session["Header"] = "Stock and Receivables Report as on" + " " + txtDate.Text.Trim();
            string strScipt = "window.open('../Reports/S3GRptStockReceivablesReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Stock", strScipt, true);
        }
        catch (Exception ex)
        {
            CVStockRec.ErrorMessage = "Unable to Open Report Page.";
            CVStockRec.IsValid = false;
        }
    }
    #endregion

    #region Image Button Events
    protected void imgbtnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            Session["StockReceivable"] = null;
            grvdetails.DataSource = null;
            grvdetails.DataBind();
            if (ddlCustomerReference.SelectedValue == "1")
            {
                int intRowIndex = Utility.FunPubGetGridRowID("grvStockRec", ((ImageButton)sender).ClientID.ToString());
                Label lblLOBId = (Label)grvStockRec.Rows[intRowIndex].FindControl("lblLOBId");
                Label lblBranchId = (Label)grvStockRec.Rows[intRowIndex].FindControl("lblBranchId");
                Label lblcustomerid = (Label)grvStockRec.Rows[intRowIndex].FindControl("lblcustomerid");
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", CompanyId.ToString());
                Procparam.Add("@User_ID", UserId.ToString());
                Procparam.Add("@lob_id", ddlLOB.SelectedValue);
                Procparam.Add("@location_id1", lblBranchId.Text);
                Procparam.Add("@ENDDATE", Utility.StringToDate(txtDate.Text.Trim()).ToString());
                Procparam.Add("@DENOMINATION", ddlDenomination.SelectedValue);
                Procparam.Add("@REFERENCE_ID", ddlCustomerReference.SelectedValue);
                Procparam.Add("@ID", lblcustomerid.Text);
                dt = Utility.GetDefaultData("S3G_RPT_SRReport_CUST_Det", Procparam);
                pnlDetails.Visible = true;
                grvdetails.Visible = true;
                grvdetails.DataSource = dt;
                grvdetails.DataBind();
                Session["StockReceivable"] = dt;

                DataTable dtt = new DataTable();
                DataTable dttran = new DataTable();
                if (Session["DT_GRID"] != null)
                    dtt = ((DataTable)Session["DT_GRID"]);
                if (dtt.Rows.Count > 0)
                {
                    DataRow[] dtrow = dtt.Select("lob_id='" + lblLOBId.Text + "' and LOCATION_ID='" + lblBranchId.Text + "' and CUSTOMER_ID='" + lblcustomerid.Text + "'");
                    if (dtrow.Length > 0)
                    {
                        dttran = dtrow.CopyToDataTable();
                        Session["DT_GRID"] = dttran;
                    }
                }
                ds = new DataSet();
                ds.Tables.Add(dttran);
                ds.Tables.Add(dt);
                ds.Tables[0].TableName = "STOCK";
                ds.Tables[1].TableName = "Detail";
                Session["Report"] = ds;
            }
            else if (ddlCustomerReference.SelectedValue == "2")
            {

                int intRowIndex = Utility.FunPubGetGridRowID("grvgroupwise", ((ImageButton)sender).ClientID.ToString());
                Label lblgpcontLOBId = (Label)grvgroupwise.Rows[intRowIndex].FindControl("lblgpcontLOBId");
                Label lblgpBranchId = (Label)grvgroupwise.Rows[intRowIndex].FindControl("lblgpBranchId");
                Label lblgpcustomerid = (Label)grvgroupwise.Rows[intRowIndex].FindControl("lblgpcustomerid");
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", CompanyId.ToString());
                Procparam.Add("@User_ID", UserId.ToString());
                Procparam.Add("@lob_id", ddlLOB.SelectedValue);
                Procparam.Add("@location_id1", lblgpBranchId.Text);
                Procparam.Add("@ENDDATE", Utility.StringToDate(txtDate.Text.Trim()).ToString());
                Procparam.Add("@DENOMINATION", ddlDenomination.SelectedValue);
                Procparam.Add("@REFERENCE_ID", ddlCustomerReference.SelectedValue);
                Procparam.Add("@ID", lblgpcustomerid.Text);
                dt = Utility.GetDefaultData("S3G_RPT_SRReport_CUST_Det", Procparam);
                pnlDetails.Visible = true;
                grvdetails.Visible = true;
                grvdetails.DataSource = dt;
                grvdetails.DataBind();
                Session["StockReceivable"] = dt;

                DataTable dtt = new DataTable();
                DataTable dttran = new DataTable();
                if (Session["DT_GRID"] != null)
                    dtt = ((DataTable)Session["DT_GRID"]);
                if (dtt.Rows.Count > 0)
                {
                    DataRow[] dtrow = dtt.Select("lob_id='" + lblgpcontLOBId.Text + "' and LOCATION_ID='" + lblgpBranchId.Text + "' and group_id='" + lblgpcustomerid.Text + "'");
                    if (dtrow.Length > 0)
                    {
                        dttran = dtrow.CopyToDataTable();
                        Session["DT_GRID"] = dttran;
                    }
                }
                ds = new DataSet();
                ds.Tables.Add(dttran);
                ds.Tables.Add(dt);
                ds.Tables[0].TableName = "STOCK";
                ds.Tables[1].TableName = "Detail";
                Session["Report"] = ds;
            }
            else if (ddlCustomerReference.SelectedValue == "3")
            {

                int intRowIndex = Utility.FunPubGetGridRowID("grvindustry", ((ImageButton)sender).ClientID.ToString());
                Label lblindustrycontLOBId = (Label)grvindustry.Rows[intRowIndex].FindControl("lblindustrycontLOBId");
                Label lblindustryBranchId = (Label)grvindustry.Rows[intRowIndex].FindControl("lblindustryBranchId");
                Label lblindustrycustomerid = (Label)grvindustry.Rows[intRowIndex].FindControl("lblindustrycustomerid");
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", CompanyId.ToString());
                Procparam.Add("@User_ID", UserId.ToString());
                Procparam.Add("@lob_id", ddlLOB.SelectedValue);
                Procparam.Add("@location_id1", lblindustryBranchId.Text);
                Procparam.Add("@ENDDATE", Utility.StringToDate(txtDate.Text.Trim()).ToString());
                Procparam.Add("@DENOMINATION", ddlDenomination.SelectedValue);
                Procparam.Add("@REFERENCE_ID", ddlCustomerReference.SelectedValue);
                Procparam.Add("@ID", lblindustrycustomerid.Text);
                dt = Utility.GetDefaultData("S3G_RPT_SRReport_CUST_Det", Procparam);
                pnlDetails.Visible = true;
                grvdetails.Visible = true;
                grvdetails.DataSource = dt;
                grvdetails.DataBind();
                Session["StockReceivable"] = dt;

                DataTable dtt = new DataTable();
                DataTable dttran = new DataTable();
                if (Session["DT_GRID"] != null)
                    dtt = ((DataTable)Session["DT_GRID"]);
                if (dtt.Rows.Count > 0)
                {
                    DataRow[] dtrow = dtt.Select("lob_id='" + lblindustrycontLOBId.Text + "' and LOCATION_ID='" + lblindustryBranchId.Text + "' and industry_id='" + lblindustrycustomerid.Text + "'");
                    if (dtrow.Length > 0)
                    {
                        dttran = dtrow.CopyToDataTable();
                        Session["DT_GRID"] = dttran;
                    }
                }
                ds = new DataSet();
                ds.Tables.Add(dttran);
                ds.Tables.Add(dt);
                ds.Tables[0].TableName = "STOCK";
                ds.Tables[1].TableName = "Detail";
                Session["Report"] = ds;
            }
            Label lbldettotGrossPrincipal = (Label)(grvdetails).FooterRow.FindControl("lbldettotGrossPrincipal");
            lbldettotGrossPrincipal.Text = Convert.ToDecimal(dt.Compute("sum(Gross_principal)", "")).ToString(Funsetsuffix());
            Label lbldettotUMFC = (Label)(grvdetails).FooterRow.FindControl("lbldettotUMFC");
            lbldettotUMFC.Text = Convert.ToDecimal(dt.Compute("sum(UMFC)", "")).ToString(Funsetsuffix());
            Label lbldettotgrossstock = (Label)(grvdetails).FooterRow.FindControl("lbldettotgrossstock");
            lbldettotgrossstock.Text = Convert.ToDecimal(dt.Compute("sum(gross_stock)", "")).ToString(Funsetsuffix());
            Label lbldettotNetPrincipal = (Label)(grvdetails).FooterRow.FindControl("lbldettotNetPrincipal");
            lbldettotNetPrincipal.Text = Convert.ToDecimal(dt.Compute("sum(Net_Principal)", "")).ToString(Funsetsuffix());
            Label lbldettotinterestOutstanding = (Label)(grvdetails).FooterRow.FindControl("lbldettotinterestOutstanding");
            lbldettotinterestOutstanding.Text = Convert.ToDecimal(dt.Compute("sum(Interest_OS)", "")).ToString(Funsetsuffix());
            Label lbldettotfutureInstallments = (Label)(grvdetails).FooterRow.FindControl("lbldettotfutureInstallments");
            lbldettotfutureInstallments.Text = Convert.ToDecimal(dt.Compute("sum(future_instal)", "")).ToString(Funsetsuffix());
            Label lbldettotnetstock = (Label)(grvdetails).FooterRow.FindControl("lbldettotnetstock");
            lbldettotnetstock.Text = Convert.ToDecimal(dt.Compute("sum(net_stock)", "")).ToString(Funsetsuffix());

        }
        catch (Exception ex)
        {
            throw ex;
        }
           
    }
    
      
    #endregion

    #endregion

    
    protected void grvStockRec_RowDataBound(object sender, GridViewRowEventArgs e)
    {
         
       if (e.Row.RowType == DataControlRowType.Header)                 // if header - then set the style dynamically.
        {
            for (int i_cellVal = 2; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                e.Row.Cells[i_cellVal].CssClass = "styleGridHeader";
            }
        }
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow) // to hide the "ID" column
        {

            //e.Row.Cells[1].Visible = (intQuery == 0) ? false : true;
            //e.Row.Cells[2].Visible = (intModify == 0) ? false : true;

            e.Row.Cells[1].Visible=false;                          // ID Column - always invisible
            e.Row.Cells[2].Visible = false;                             // User ID Column - always invisible
            e.Row.Cells[3].Visible = false;                             // User Level ID Column - always invisible

        }
       
       


    }

    protected void grvdetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

      



    }

    protected void grvTransLander_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
         private DataTable FunPriHDR_DT()
    {
        DataTable dtbl = new DataTable();
        try
        {
           
            DataRow drEmptyRow;
            dtbl.Columns.Add("lob_id");
            dtbl.Columns.Add("lob");
            dtbl.Columns.Add("LOCATION_ID");
            dtbl.Columns.Add("Level_ID");
            dtbl.Columns.Add("LOCATION_NAME");
            dtbl.Columns.Add("levelname");
            dtbl.Columns.Add("panum");
            dtbl.Columns.Add("sanum");
            dtbl.Columns.Add("Gross_principal");
            dtbl.Columns.Add("UMFC");
            dtbl.Columns.Add("gross_stock");
            dtbl.Columns.Add("NOI");
            dtbl.Columns.Add("Net_Principal");
            dtbl.Columns.Add("Interest_OS");
            dtbl.Columns.Add("future_instal");
            dtbl.Columns.Add("net_stock");
            dtbl.Columns.Add("gpssuffix");


            drEmptyRow = dtbl.NewRow();
            //drEmptyRow["lob_id"] = 0;
            //drEmptyRow["lob"] = "";
            //drEmptyRow["LOCATION_ID"] = "";
            //drEmptyRow["Level_ID"] = "";
            //drEmptyRow["LOCATION_NAME"] = "";
            //drEmptyRow["levelname"] = "";
            //drEmptyRow["panum"] = "";
            //drEmptyRow["sanum"] ="";
            //drEmptyRow["Gross_principal"] = 0;//lblGTAmount .Text ;
            //drEmptyRow["UMFC"] = 0;
            //drEmptyRow["gross_stock"] = 0;
            //drEmptyRow["NOI"] ="";
            //drEmptyRow["Net_Principal"] =0;
            //drEmptyRow["Interest_OS"] = 0;
            //drEmptyRow["future_instal"] = 0;
            //drEmptyRow["net_stock"] = 0;

            dtbl.Rows.Add(drEmptyRow);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dtbl;

    }
    
    
}

 