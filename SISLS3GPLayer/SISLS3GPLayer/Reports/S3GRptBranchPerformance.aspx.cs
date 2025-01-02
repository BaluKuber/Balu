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
using S3GBusEntity;
using System.Collections.Generic;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.Globalization;

public partial class Reports_S3GRptBranchPerformance : ApplyThemeForProject
{
    Dictionary<string, string> Procparam;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public string strDateFormat;
    public int CompanyId;
    public int UserId;
    string LOB_ID;
    string Branch_ID;
    string Region_Id;
    public int LocationId;
    string CutOffMonth;
    public int LobId;
    public int ProgramId;
    bool Is_Active;
    decimal Totalopeningaccount;
    decimal Totalopeningstock;
    decimal Totalopeningarrears;
    decimal Totaladditionaccount;
    decimal Totaladditionstock;
    decimal Totaladditionarrear;
    decimal Totaldeletionaccount;
    decimal Totaldeletionstock;
    decimal Totaldeletionarrear;
    decimal Totalclosingaccount;
    decimal Totalclosingstock;
    decimal Totalclosingarrear;
    decimal Totalstock;
    decimal Totalarrear;
    decimal TotalLnkOpeningStock;
    decimal TotalLnkOpeningArrear;
    decimal TotalLnkAdditionStock;
    decimal TotalLnkAdditionArrear;
    decimal TotalLnkDeletionStock;
    decimal TotalLnkDeletionArrear;
    decimal TotalLnkClosingStock;
    decimal TotalLnkClosingArrear;
    decimal TotalAssetMonth;
    decimal TotalAssetYear;
    decimal TotalCurrentCollection;
    decimal TotalArrearCollection;
    decimal TotalCollection;
    string strPageName = "Branch Performance";
    ReportAccountsMgtServicesClient objSerClient;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriLoadPage();
        txtCutoffMonthSearch.Attributes.Add("readonly", "readonly");
    }

    private void FunPriLoadPage()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            UserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            ProgramId = 174;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
                 //CalendarExtender1.Format = strDateFormat;
            if (!IsPostBack)
            {
                ddlbranch.Enabled = false;
                pnlAddtionaccountdetails.Visible = false;
                pnlDeletionAccountDetails.Visible = false;
                pnlClosingAccountDetails.Visible = false;
                pnlnoofaccounts.Visible = false;
                pnlOpeningaccountdetails.Visible = false;
                pnlpayments.Visible = false;
                PnlCollection.Visible = false;
                pnlstock.Visible = false;
                btnPrint.Visible = false;
                btnView.Visible = false;
                pnlcumulativecollection.Visible = false;
                pnlAccount.Visible = false;
                FunPriLoadLob(CompanyId, UserId, ProgramId);
                FunPriLoadLocation1(CompanyId, UserId, ProgramId, LobId);
                FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
                //FunPriLoadRegion(CompanyId, Is_Active, UserId);
               // FunPriLoadBranch(CompanyId, UserId, Region_Id, Is_Active);
                FunPubLoadDenomination();
               
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable Load BranchPerformance page");
        }
    }

    private void FunPriLoadLob(int CompanyId, int UserId, int ProgramId)
    {
        try
        {
            ddlLOB.Items.Clear();
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubLOB(CompanyId, UserId, ProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
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

    private void FunPriLoadRegion(int CompanyId, bool Is_Active, int UserId)
    {
        try
        {
            ddlRegion.Items.Clear();
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubGetRegion(CompanyId, true,UserId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlRegion.DataSource = lobs;
            ddlRegion.DataTextField = "Description";
            ddlRegion.DataValueField = "ID";
            ddlRegion.DataBind();
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
  
    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    public void FunPubLoadDenomination()
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.GetDenominations();
            List<ClsPubDropDownList> Denomination = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
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
            objSerClient.Close();
        }
    }

    protected void txtCutoffMonthSearch_OnTextChanged(object sender, EventArgs e)
    {
    try
     {
         lblAmounts.Visible = false;
         FunPriCleargrid();
        FunPriValidateFutureDate();
#region date
    //        //DateTime dt1 = Utility.StringToDate(txtCutoffMonthSearch.Text.Trim());//21/04/2011
    //        //int x = Convert.ToInt32(dt1.Year.ToString() + dt1.Month.ToString());
    //        //int y = Convert.ToInt32(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString());
    //        //DateTime dt1 = Utility.StringToDate(txtCutoffMonthSearch.Text.Trim()).Year;
    //        //DateTime dt = Utility.StringToDate(txtCutoffMonthSearch.Text.Trim()).Month;
    //        //txtCutoffMonthSearch.Text = dt1.Year + dt.Month;
    //        int length = txtCutoffMonthSearch.Text.Length;
    //        if (length > 6)
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "Cutoff month should be in 6 digits");
    //            return;
    //        }
    //        else if (length < 6)
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "Cutoff month should be in 6 digits");
    //            return;
    //        }
    //        //string YearMonth = txtCutoffMonthSearch.Text;
    //        int year = int.Parse(YearMonth.Substring(0, 4));
    //        int Month = int.Parse(YearMonth.Substring(4, 2));
    //        int currentyear = DateTime.Now.Year;
    //        int currentmonth = DateTime.Now.Month;
    //        if (year > currentyear)
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "CutOffyear should not be greater than the Current year");
    //            return;
    //        }
    //        else if (Month > 12)
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "The given month is not valid");
    //            return;
    //        }
    //        else if (year == currentyear && Month > currentmonth)
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "CutOffMonth should not be greater than the Current month");
    //            return;
    //        }
#endregion



     }
    catch (Exception ex)
    {

    }
    }

    private void FunPriLoadCollectiongrid()
    {
        try
        {
            string financialyear;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            int financialyearmonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
            if (financialyearmonth < Month)
            {
                financialyear = Convert.ToString(year);
            }
            else
            {
                financialyear = Convert.ToString(year - 1);
            }
            
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            branchSelectionCriteria.LocationID1= string.Empty;
            branchSelectionCriteria.LocationID2 = string.Empty;
            if (ddlRegion.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
            }
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.CutOffYear = Convert.ToString(year);
            branchSelectionCriteria.CutOffMonth = Month.ToString("00");
            branchSelectionCriteria.Financial_Year_From = financialyear;
            branchSelectionCriteria.UserId = UserId;
            byte[] bytedisburse = objSerClient.FunPubGetCollectionDetails(branchSelectionCriteria);
            List<ClsPubCollection> collections = (List<ClsPubCollection>)DeSerialize(bytedisburse);
            //TotalCurrentCollection = collections.Sum(ClsPubCollection => (ClsPubCollection.CurrentCollection));
            //TotalArrearCollection = collections.Sum(ClsPubCollection => (ClsPubCollection.ArrearCollection));
            //TotalCollection = collections.Sum(ClsPubCollection => (ClsPubCollection.TotalCollection));
            Session["Collection"] = collections;
            grvcollections.DataSource = collections;
            grvcollections.DataBind();
            if (grvcollections.Rows.Count == 0)
            {
                grvcollections.EmptyDataText = "No records found";
                grvcollections.DataBind();
            }
            else
            {
                grvcollections.DataSource = collections;
                grvcollections.DataBind();
                //FunPriDisplayTotalCollection();
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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (ddlLOB.SelectedIndex > 0)
        {
            if (!FunFindLOBDemandMonth())
            {
                Utility.FunShowAlertMsg(this, "Demand not run for the selected month and LOB ");
                txtCutoffMonthSearch.Text = "";
                ddlLOB.SelectedValue= "-1";
                return;
            }

        }
        else
        {
            if (!FunFindDemandMonth())
            {
                Utility.FunShowAlertMsg(this, "Demand not run for the selected month ");
                txtCutoffMonthSearch.Text = "";
                return;
            }

        }
       

        pnlOpeningaccountdetails.Visible = false;
        pnlAddtionaccountdetails.Visible = false;
        pnlDeletionAccountDetails.Visible = false;
        pnlClosingAccountDetails.Visible = false;
        string lob;
        string cutoffmonth;
        lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
        Session["LOB"] = lob;
        cutoffmonth = txtCutoffMonthSearch.Text;
        Session["Cutoffmonth"] = cutoffmonth;
        ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
 
        objHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
        //if (Convert.ToInt32(ddlRegion.SelectedValue) > 0)
        //{
        //    objHeader.Region = (ddlRegion.SelectedItem.Text.Split('-'))[1].ToString();
        //}
        //else
        //{
        //    objHeader.Region = "";
        //    //((Label)grvSummary.FooterRow.FindControl("lblRegion")).Text;
        //}

        //if (Convert.ToInt32(ddlbranch.SelectedValue) > 0)
        //{
        //    objHeader.Branch = (ddlbranch.SelectedItem.Text.Split('-'))[1].ToString();


        //}
        //else
        //{

        //    objHeader.Branch = "";
        //}
        objHeader.Cutoffmonth = txtCutoffMonthSearch.Text;
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

        //Session["LOB"] = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
        //Session["Cutoffmonth"] = Convert.ToDateTime(txtCutoffMonthSearch.Text).ToString("MMM yyyy");
        Session["Header"] = objHeader;   
        pnlAccount.Visible = true;
        pnlpayments.Visible = true;
        PnlCollection.Visible = true;
        pnlstock.Visible = true;
        pnlclass.Visible = true;
        btnPrint.Visible = true;
        btnView.Visible = true;
        FunPriLoadCollectiongrid();
        FunPriGetStockDetails();
        FunPriGetPaymentDetails();
        FunPriGetCumulativeCollectionDetails();
        FunPriGetAccountDetails();
        if (ddlLOB.SelectedIndex > 0)
        {
            if (!FunFindLOBDelinquentmonth())
            {
                grvNPAAccount.EmptyDataText = "Delinquency data not available for the month";
                grvNPAAccount.DataBind();


            }
            else
            {
                FunPriLoadNPAgrid();
            }
        }
        //FunPriLoadNPAgrid();
        FunPriLoadRegBranch();

    }

    private void FunPriGetStockDetails()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        string Region = string.Empty;
        string Branch = string.Empty;
        string YearMonth = txtCutoffMonthSearch.Text;
        int year = int.Parse(YearMonth.Substring(0, 4));
        int Month = int.Parse(YearMonth.Substring(4, 2));
        if (Month < 10)
        {
            CutOffMonth = Convert.ToString(year) + "0" + Convert.ToString(Month);
        }
        else
        {
            CutOffMonth = Convert.ToString(year) + Convert.ToString(Month);
        }
        ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
        branchSelectionCriteria.CompanyId = CompanyId;
        branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
        branchSelectionCriteria.LocationID1 = string.Empty;
        branchSelectionCriteria.LocationID2= string.Empty;
        branchSelectionCriteria.CutOffMonth = CutOffMonth;
        if (ddlRegion.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID1= ddlRegion.SelectedValue;
        }
        if (ddlbranch.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID2= ddlbranch.SelectedValue;
        }
        branchSelectionCriteria.ProgramId = ProgramId;
        branchSelectionCriteria.UserId = UserId;
        branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
        byte[] bytestock = objSerClient.FunPubGetStockDetails(branchSelectionCriteria);
        List<ClsPubBranchStock> branchstock = (List<ClsPubBranchStock>)DeSerialize(bytestock);
        Session["Stock"] = branchstock;
        grvstock.DataSource = branchstock;
        grvstock.DataBind();
        if (grvstock.Rows.Count == 0)
        {
            grvstock.EmptyDataText = "No records found";
            grvstock.DataBind();
        }
        else
        {
            grvstock.DataSource = branchstock;
            grvstock.DataBind();
        }

    }

    private void FunPriLoadRegBranch()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
        branchSelectionCriteria.CompanyId = CompanyId;
        branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
        branchSelectionCriteria.LocationID1 = string.Empty;
        branchSelectionCriteria.LocationID2 = string.Empty;
        branchSelectionCriteria.CutOffMonth = CutOffMonth;
        if (ddlRegion.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
        }
        if (ddlbranch.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
        }
        branchSelectionCriteria.ProgramId = ProgramId;
        branchSelectionCriteria.UserId = UserId;
        byte[] bytestock = objSerClient.FunPubGetRegionBranchDetails(branchSelectionCriteria);
        List<ClsPubRegionBranch> branchstock = (List<ClsPubRegionBranch>)DeSerialize(bytestock);
        Session["regbranch"] = branchstock;
       
    }

    private void FunPriGetPaymentDetails()
    {
        string financialyear;
        string YearMonth = txtCutoffMonthSearch.Text;
        int year = int.Parse(YearMonth.Substring(0, 4));
        int Month = int.Parse(YearMonth.Substring(4, 2));
        string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
        int financialyearmonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
        if (financialyearmonth < Month)
        {
            financialyear = Convert.ToString(year);
        }
        else
        {
            financialyear = Convert.ToString(year - 1);
        }
        string s = financialyearmonth + "/" + "1" + "/" + financialyear;
        DateTime financialstartdate = Convert.ToDateTime(s);
        DateTime CutOffstartdate = Convert.ToDateTime(Month + "/" + "1" + "/" + year);
        DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
        objSerClient = new ReportAccountsMgtServicesClient();
        string Region = string.Empty;
        string Branch = string.Empty;
        ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
        branchSelectionCriteria.CompanyId = CompanyId;
        branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
        branchSelectionCriteria.LocationID1 = string.Empty;
        branchSelectionCriteria.LocationID2 = string.Empty;
        if (ddlRegion.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
        }
        if (ddlbranch.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
        }
        branchSelectionCriteria.FYSTARTDATE = financialstartdate;
        branchSelectionCriteria.CMSTARTDATE = CutOffstartdate;
        branchSelectionCriteria.CMENDDATE = CutOffEndDate;
        branchSelectionCriteria.ProgramId = ProgramId;
        branchSelectionCriteria.UserId = UserId;
        branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
        byte[] bytedisburse = objSerClient.FunPubGetPayment(branchSelectionCriteria);
        ClsPubPayment payment = (ClsPubPayment)DeSerialize(bytedisburse);
        //TotalAssetMonth = payment.PaymentDetails.Sum(ClsPubPayment => ClsPubPayment.AssetClassMonth);
        //TotalAssetYear = payment.PaymentDetails.Sum(ClsPubPayment => ClsPubPayment.AssetClassMonth);

            //details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ApprovedAmount);
        Session["payment"] = payment.assets;
        Session["Units"] = payment.PaymentDetails;
        grvpayment.DataSource = payment.assets;
        grvpayment.DataBind();
        if (grvpayment.Rows.Count == 0)
        {
            grvpayment.EmptyDataText = "No records found";
            grvpayment.DataBind();
        }
        else
        {
            grvpayment.DataSource = payment.assets;
            grvpayment.DataBind();
        }
        grvUnits.DataSource = payment.PaymentDetails;
        grvUnits.DataBind();
        if (grvUnits.Rows.Count == 0)
        {
            grvUnits.EmptyDataText = "No records found";
            grvUnits.DataBind();
        }
        else
        {
            grvUnits.DataSource = payment.PaymentDetails;
            grvUnits.DataBind();
            //FunPriTotalAsset();

        }

             
           
       
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlbranch.Enabled = false;
        ddlLOB.SelectedValue = "-1";
        ddlRegion.SelectedValue = "-1";
        ddlbranch.SelectedValue = "-1";
        txtCutoffMonthSearch.Text = "";
        pnlAccount.Visible = false;
        pnlOpeningaccountdetails.Visible = false;
        pnlAddtionaccountdetails.Visible = false;
        pnlDeletionAccountDetails.Visible = false;
        pnlClosingAccountDetails.Visible = false;
        pnlpayments.Visible = false;
        PnlCollection.Visible = false;
        pnlstock.Visible = false;
        pnlcumulativecollection.Visible = false;
        pnlclass.Visible = false;
        pnlnoofaccounts.Visible = false;
        btnPrint.Visible = false;
        btnView.Visible = false;
        lblAmounts.Visible = false;
        Session["Details"] = null;
        Session["Collection"] = null;
        Session["Stock"] = null;
        Session["Opening"] = null;
        Session["Addition"] = null;
        Session["Deletion"] = null;
        Session["Closing"] = null;
        Session["payment"] = null;
        Session["Units"] = null;
        Session["cumulative"] = null;
        Session["Account"] = null;
        Session["regbranch"] = null;

    }

    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRegion.SelectedIndex > 0)
        {
            ddlbranch.Enabled = true;
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId);
            FunPriCleargrid();
        }
        else
        {
            FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
            ddlbranch.Enabled = false;
            FunPriCleargrid();
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        
         
            
            FunPriCleargrid();
       
    }

    protected void ddlDenomination_SelectedIndexChanged(object sender, EventArgs e)
    {



        FunPriCleargrid();

    }


    private void FunPriCleargrid()
    {

        pnlAccount.Visible = false;
        pnlOpeningaccountdetails.Visible = false;
        pnlpayments.Visible = false;
        PnlCollection.Visible = false;
        pnlstock.Visible = false;
        pnlAddtionaccountdetails.Visible = false;
        pnlDeletionAccountDetails.Visible = false;
        pnlClosingAccountDetails.Visible = false;
        pnlcumulativecollection.Visible = false;
        pnlclass.Visible = false;
        pnlnoofaccounts.Visible = false;
        btnPrint.Visible = false;
        btnView.Visible = false;
          Session["Details"]=null;
          Session["Collection"] =null;
           Session["Stock"]=null; 
           Session["Opening"]=null;
            Session["Addition"] =null;
            Session["Deletion"]=null;
            Session["Closing"]=null; 
            Session["payment"]=null;
            Session["Units"]=null;
            Session["cumulative"]=null;
            Session["Account"]=null;
            Session["regbranch"] = null;
            lblAmounts.Visible = false;


    }

    private void FunPriLoadNPAgrid()
    {
        try
        {
            string cutoffpreviousmonth;
            string cutoffmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            cutoffmonth = year.ToString("0000") + Month.ToString("00");

            string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
            DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            branchSelectionCriteria.LocationID1 = string.Empty;
            branchSelectionCriteria.LocationID2 = string.Empty;
            if (ddlRegion.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
            }
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = cutoffmonth;
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            branchSelectionCriteria.CMENDDATE = CutOffEndDate;
            byte[] bytedisburse = objSerClient.FunPubGetNPADetails(branchSelectionCriteria);
            List<ClsPubNPA> npa = (List<ClsPubNPA>)DeSerialize(bytedisburse);
            Totalopeningaccount = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.OpeningNoOfAccounts));
            Totalopeningstock = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.OpeningStock));
            Totalopeningarrears = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.OpeningArrear));
            Totaladditionaccount = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.AdditionNoOfAccounts));
            Totaladditionstock = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.AdditionStock));
            Totaladditionarrear = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.AdditionArrear));
            Totaldeletionaccount = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.DeletionNoOfAccounts));
            Totaldeletionstock = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.DeletionStock));
            Totaldeletionarrear = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.DeletionArrear));
            Totalclosingaccount = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.ClosingNoOfAccounts));
            Totalclosingstock = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.ClosingStock));
            Totalclosingarrear = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.ClosingArrear));
            Totalarrear = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.Arrears));
            Totalstock = npa.Sum(ClsPubNPA => Convert.ToDecimal(ClsPubNPA.stock));
            Session["Details"] = npa;
            grvNPAAccount.DataSource = npa;
            grvNPAAccount.DataBind();
            if (grvNPAAccount.Rows.Count == 0)
            {
                grvNPAAccount.EmptyDataText= "No records found";
                grvNPAAccount.DataBind();
            }
            else
            {
                grvNPAAccount.DataSource = npa;
                grvNPAAccount.DataBind();
                FunPriDisplayTotalDetails();
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

    protected void LnkBtnopening_Click(object sender, EventArgs e)
    {
        try
        {
            pnlOpeningaccountdetails.Visible = true;
            //LinkButton 
            LinkButton lnk = (LinkButton)sender;
           // HiddenField hid_GT = (HiddenField)g.FooterRow.FindControl("hid_GT");

            GridView g = (GridView)lnk.Parent.Parent.Parent.Parent;
            GridViewRow grvData = (GridViewRow)lnk.Parent.Parent;


            Label lblregion = (Label)grvData.FindControl("lblregion");
            Label lblclassid = (Label)grvData.FindControl("lblclassid");
            
            pnlOpeningaccountdetails.Visible = true;
            string cutoffpreviousmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            branchSelectionCriteria.LocationCode = lblregion.Text;
            branchSelectionCriteria.ClassId = Convert.ToInt32(lblclassid.Text);
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = txtCutoffMonthSearch.Text;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytedisburse = objSerClient.FunPubGetNPAOpeningaccounts(branchSelectionCriteria);
            List<ClsNPAaccount> npaaccount = (List<ClsNPAaccount>)DeSerialize(bytedisburse);
            TotalLnkOpeningStock = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Stock));
            TotalLnkOpeningArrear = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Arrear));
            Session["Opening"] = npaaccount;
            grvOpeningaccountdetails.DataSource = npaaccount;
            grvOpeningaccountdetails.DataBind();
            if (grvOpeningaccountdetails.Rows.Count == 0)
            {
                grvOpeningaccountdetails.EmptyDataText = "No records found";
                grvOpeningaccountdetails.DataBind();
            }
            else
            {
                grvOpeningaccountdetails.DataSource = npaaccount;
                grvOpeningaccountdetails.DataBind();

            }
            FunPriDisplayOpening();
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

    protected void LnkTotalBtnopening_Click(object sender, EventArgs e)
    {
        try
        {
            pnlOpeningaccountdetails.Visible = true;
            string cutoffpreviousmonth;
            string cutoffmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            cutoffmonth = year.ToString("0000") + Month.ToString("00");
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            branchSelectionCriteria.LocationID1 = string.Empty;
            branchSelectionCriteria.LocationID2 = string.Empty;
            if (ddlRegion.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
            }
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = cutoffmonth;
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytedisburse = objSerClient.FunPubGetTotalNPAOpeningaccounts(branchSelectionCriteria);
            List<ClsNPAaccount> npaaccount = (List<ClsNPAaccount>)DeSerialize(bytedisburse);
            TotalLnkOpeningStock = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Stock));
            TotalLnkOpeningArrear = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Arrear));
            Session["Opening"] = npaaccount;
            grvOpeningaccountdetails.DataSource = npaaccount;
            grvOpeningaccountdetails.DataBind();
            if (grvOpeningaccountdetails.Rows.Count == 0)
            {
                grvOpeningaccountdetails.EmptyDataText = "No records found";
                grvOpeningaccountdetails.DataBind();
            }
            else
            {
                grvOpeningaccountdetails.DataSource = npaaccount;
                grvOpeningaccountdetails.DataBind();
            }
            FunPriDisplayOpening();
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

    protected void LnkBtnAddition_Click(object sender, EventArgs e)
    {
        try
        {
            pnlAddtionaccountdetails.Visible = true;
            LinkButton lnk = (LinkButton)sender;
            // HiddenField hid_GT = (HiddenField)g.FooterRow.FindControl("hid_GT");

            GridView g = (GridView)lnk.Parent.Parent.Parent.Parent;
            GridViewRow grvData = (GridViewRow)lnk.Parent.Parent;


            Label lblregion = (Label)grvData.FindControl("lblregion");
            Label lblclassid = (Label)grvData.FindControl("lblclassid");
            pnlAddtionaccountdetails.Visible = true;
            string cutoffpreviousmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            //branchSelectionCriteria.RegionId = lblregionid.Text;
            branchSelectionCriteria.LocationCode = lblregion.Text;
            branchSelectionCriteria.ClassId = Convert.ToInt32(lblclassid.Text);
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = txtCutoffMonthSearch.Text;
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytedisburse = objSerClient.FunPubGetNPAAddtionaccounts(branchSelectionCriteria);
            List<ClsNPAaccount> npaaccount = (List<ClsNPAaccount>)DeSerialize(bytedisburse);
            Session["Addition"] = npaaccount;
             TotalLnkAdditionStock = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Stock));
            TotalLnkAdditionArrear = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Arrear));
            grvAdditionAccountDetails.DataSource = npaaccount;
            grvAdditionAccountDetails.DataBind();
            if (grvAdditionAccountDetails.Rows.Count == 0)
            {
                grvAdditionAccountDetails.EmptyDataText = "No records found";
                grvAdditionAccountDetails.DataBind();
            }
            else
            {
                grvAdditionAccountDetails.DataSource = npaaccount;
                grvAdditionAccountDetails.DataBind();
            }
            FunPriDisplayAdditionTotal();
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

    protected void LnkBtnTotalAddition_Click(object sender, EventArgs e)
    {
        try
        {
            pnlAddtionaccountdetails.Visible = true;
            string cutoffpreviousmonth;
            string cutoffmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            cutoffmonth = year.ToString("0000") + Month.ToString("00");
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            branchSelectionCriteria.LocationID1 = string.Empty;
            branchSelectionCriteria.LocationID2 = string.Empty;
            if (ddlRegion.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
            }
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = cutoffmonth;
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytedisburse = objSerClient.FunPubGetTotalNPAAddtionaccounts(branchSelectionCriteria);
            List<ClsNPAaccount> npaaccount = (List<ClsNPAaccount>)DeSerialize(bytedisburse);
            TotalLnkAdditionStock = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Stock));
            TotalLnkAdditionArrear = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Arrear));
            Session["Addition"] = npaaccount;
            grvAdditionAccountDetails.DataSource = npaaccount;
            grvAdditionAccountDetails.DataBind();
            if (grvAdditionAccountDetails.Rows.Count == 0)
            {
                grvAdditionAccountDetails.EmptyDataText = "No records found";
                grvAdditionAccountDetails.DataBind();
            }
            else
            {
                grvAdditionAccountDetails.DataSource = npaaccount;
                grvAdditionAccountDetails.DataBind();
            }
            FunPriDisplayAdditionTotal();
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

    protected void LnkBtnDeletion_Click(object sender, EventArgs e)
    {
        try
        {
            pnlDeletionAccountDetails.Visible = true;
            LinkButton lnk = (LinkButton)sender;
            // HiddenField hid_GT = (HiddenField)g.FooterRow.FindControl("hid_GT");

            GridView g = (GridView)lnk.Parent.Parent.Parent.Parent;
            GridViewRow grvData = (GridViewRow)lnk.Parent.Parent;

            //Label lblregionid = (Label)grvData.FindControl("lblregionid");
            Label lblregion = (Label)grvData.FindControl("lblregion");
            Label lblclassid = (Label)grvData.FindControl("lblclassid");
            pnlDeletionAccountDetails.Visible = true;
            string cutoffpreviousmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
           // branchSelectionCriteria.RegionId = lblregionid.Text;
            branchSelectionCriteria.LocationCode = lblregion.Text;
            branchSelectionCriteria.ClassId = Convert.ToInt32(lblclassid.Text);
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = txtCutoffMonthSearch.Text;
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytedisburse = objSerClient.FunPubGetNPADeletionaccounts(branchSelectionCriteria);
            List<ClsNPAaccount> npaaccount = (List<ClsNPAaccount>)DeSerialize(bytedisburse);
            TotalLnkDeletionStock = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Stock));
            TotalLnkDeletionArrear = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Arrear));
            Session["Deletion"] = npaaccount;
            grvDeletionAccountDetails.DataSource = npaaccount;
            grvDeletionAccountDetails.DataBind();
            if (grvDeletionAccountDetails.Rows.Count == 0)
            {
                grvDeletionAccountDetails.EmptyDataText = "No records found";
                grvDeletionAccountDetails.DataBind();
            }
            else
            {
                grvDeletionAccountDetails.DataSource = npaaccount;
                grvDeletionAccountDetails.DataBind();
            }
            FunPriDisplayDeletionTotal();
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

    protected void LnkBtnTotalDeletion_Click(object sender, EventArgs e)
    {
        try
        {
            pnlDeletionAccountDetails.Visible = true;
            string cutoffpreviousmonth;
            string cutoffmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            cutoffmonth = year.ToString("0000") + Month.ToString("00");
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            branchSelectionCriteria.LocationID1 = string.Empty;
            branchSelectionCriteria.LocationID2 = string.Empty;
            if (ddlRegion.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
            }
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = cutoffmonth;
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytedisburse = objSerClient.FunPubGetTotalNPADeletionaccounts(branchSelectionCriteria);
            List<ClsNPAaccount> npaaccount = (List<ClsNPAaccount>)DeSerialize(bytedisburse);
            TotalLnkDeletionStock = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Stock));
            TotalLnkDeletionArrear = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Arrear));
            Session["Deletion"] = npaaccount;
            grvDeletionAccountDetails.DataSource = npaaccount;
            grvDeletionAccountDetails.DataBind();
            if (grvDeletionAccountDetails.Rows.Count == 0)
            {
                grvDeletionAccountDetails.EmptyDataText = "No records found";
                grvDeletionAccountDetails.DataBind();
            }
            else
            {
                grvDeletionAccountDetails.DataSource = npaaccount;
                grvDeletionAccountDetails.DataBind();
            }
            FunPriDisplayDeletionTotal();
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

    protected void LnkBtnclosingact_Click(object sender, EventArgs e)
    {
        try
        {
            pnlClosingAccountDetails.Visible = true;
            LinkButton lnk = (LinkButton)sender;
            // HiddenField hid_GT = (HiddenField)g.FooterRow.FindControl("hid_GT");

            GridView g = (GridView)lnk.Parent.Parent.Parent.Parent;
            GridViewRow grvData = (GridViewRow)lnk.Parent.Parent;

            //Label lblregionid = (Label)grvData.FindControl("lblregionid");
            Label lblregion = (Label)grvData.FindControl("lblregion");
            Label lblclassid = (Label)grvData.FindControl("lblclassid");
            pnlClosingAccountDetails.Visible = true;
            string cutoffpreviousmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            //branchSelectionCriteria.RegionId = lblregionid.Text;
            branchSelectionCriteria.LocationCode = lblregion.Text;
            branchSelectionCriteria.ClassId = Convert.ToInt32(lblclassid.Text);
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = txtCutoffMonthSearch.Text;
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytedisburse = objSerClient.FunPubGetNPAClosingaccounts(branchSelectionCriteria);
            List<ClsNPAaccount> npaaccount = (List<ClsNPAaccount>)DeSerialize(bytedisburse);
            TotalLnkClosingStock = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Stock));
            TotalLnkClosingArrear = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Arrear));
            Session["Closing"] = npaaccount;
            grvClosingAccountDetails.DataSource = npaaccount;
            grvClosingAccountDetails.DataBind();
            if (grvClosingAccountDetails.Rows.Count == 0)
            {
                grvClosingAccountDetails.EmptyDataText = "No records found";
                grvClosingAccountDetails.DataBind();
            }
            else
            {

                grvClosingAccountDetails.DataSource = npaaccount;
                grvClosingAccountDetails.DataBind();
            }
            FunPriDisplayTotalClosingDetails();
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

    protected void LnkBtTotalclosingact_Click(object sender, EventArgs e)
    {
        try
        {
            pnlClosingAccountDetails.Visible = true;
            string cutoffpreviousmonth;
            string cutoffmonth;
            string YearMonth = txtCutoffMonthSearch.Text;
            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            cutoffmonth = year.ToString("0000") + Month.ToString("00");
            if (Month == 1)
            {
                year = year - 1 + 12;
            }
            else
            {
                Month = Month - 1;
            }
            cutoffpreviousmonth = year.ToString("0000") + Month.ToString("00");
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string Branch = string.Empty;
            ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
            branchSelectionCriteria.CompanyId = CompanyId;
            branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
            branchSelectionCriteria.LocationID1 = string.Empty;
            branchSelectionCriteria.LocationID2 = string.Empty;
            if (ddlRegion.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
            }
            branchSelectionCriteria.CUTOFFPREVIOUSMONTH = cutoffpreviousmonth;
            branchSelectionCriteria.CutOffMonth = cutoffmonth;
            branchSelectionCriteria.ProgramId = ProgramId;
            branchSelectionCriteria.UserId = UserId;
            branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytedisburse = objSerClient.FunPubGetTotalNPAClosingaccounts(branchSelectionCriteria);
            List<ClsNPAaccount> npaaccount = (List<ClsNPAaccount>)DeSerialize(bytedisburse);
            TotalLnkClosingStock = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Stock));
            TotalLnkClosingArrear = npaaccount.Sum(ClsNPAaccount => Convert.ToDecimal(ClsNPAaccount.Arrear));
            Session["Closing"] = npaaccount;
            grvClosingAccountDetails.DataSource = npaaccount;
            grvClosingAccountDetails.DataBind();
            if (grvClosingAccountDetails.Rows.Count == 0)
            {
                grvClosingAccountDetails.EmptyDataText = "No records found";
                grvClosingAccountDetails.DataBind();
            }
            else
            {
                grvClosingAccountDetails.DataSource = npaaccount;
                grvClosingAccountDetails.DataBind();
            }
            FunPriDisplayTotalClosingDetails();
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

    protected void btnPrint_Click(object sender, EventArgs e)
    {

        string s1 = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();

        if (s1 == "HP")
            Session["l1"] = "Gross Investment on Hire";
        else if (s1 == "LN")
            Session["l1"] = "Gross Investment in Loan";
        else if (s1 == "FL")
            Session["l1"] = "Gross Investment in Lease";
        else
            Session["l1"] = "Receivables";
        string strScipt = "window.open('../Reports/S3GRptBranchPerformanceReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "BranchPerformance", strScipt, true);
    }
    protected void btnView_Click(object sender, EventArgs e)
    {

        string strScipt = "window.open('../Reports/S3GRptBranchPerformanceChart.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "BranchPerformance", strScipt, true);
    }

    private void FunPriGetCumulativeCollectionDetails()
    {
        pnlcumulativecollection.Visible = true;
        string financialyear;
        string cutoffmonth;
        string YearMonth = txtCutoffMonthSearch.Text;
        int year = int.Parse(YearMonth.Substring(0, 4));
        int Month = int.Parse(YearMonth.Substring(4, 2));
        int financialyearmonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
        if (financialyearmonth < Month)
        {
            financialyear = Convert.ToString(year);
        }
        else
        {
            financialyear = Convert.ToString(year - 1);
        }
        cutoffmonth = year.ToString("0000") + Month.ToString("00");
        objSerClient = new ReportAccountsMgtServicesClient();
        string Region = string.Empty;
        string Branch = string.Empty;
        ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
        branchSelectionCriteria.CompanyId = CompanyId;
        branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
        branchSelectionCriteria.LocationID1 = string.Empty;
        branchSelectionCriteria.LocationID2 = string.Empty;
        if (ddlRegion.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
        }
        if (ddlbranch.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
        }
        branchSelectionCriteria.ProgramId = ProgramId;
        branchSelectionCriteria.UserId = UserId;
        branchSelectionCriteria.CutOffMonth = cutoffmonth;
        branchSelectionCriteria.Financial_Year_From = financialyear;
        branchSelectionCriteria.FinancialMonth = financialyearmonth.ToString("00");
        branchSelectionCriteria.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
        byte[] bytestock = objSerClient.FunPubGetCumulativeCollectionDetails(branchSelectionCriteria);
        List<ClsPubCumulativeCollection> collection = (List<ClsPubCumulativeCollection>)DeSerialize(bytestock);
        Session["cumulative"] = collection;
        grvcumulativecollection.DataSource = collection;
        grvcumulativecollection.DataBind();
        if (grvcumulativecollection.Rows.Count == 0)
        {
            grvcumulativecollection.EmptyDataText = "No records found";
            grvcumulativecollection.DataBind();
        }
        else
        {
            grvcumulativecollection.DataSource = collection;
            grvcumulativecollection.DataBind();
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

    private void FunPriGetAccountDetails()
    {
        string YearMonth = txtCutoffMonthSearch.Text;
        int year = int.Parse(YearMonth.Substring(0, 4));
        int Month = int.Parse(YearMonth.Substring(4, 2));
        string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
        DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
        pnlnoofaccounts.Visible = true;
        objSerClient = new ReportAccountsMgtServicesClient();
        string Branch = string.Empty;
        ClsPubBranchInputSelectionCriteria branchSelectionCriteria = new ClsPubBranchInputSelectionCriteria();
        branchSelectionCriteria.CompanyId = CompanyId;
        branchSelectionCriteria.LobId = ddlLOB.SelectedValue;
        branchSelectionCriteria.LocationID1 = string.Empty;
        branchSelectionCriteria.LocationID2 = string.Empty;
        if (ddlRegion.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
        }
        if (ddlbranch.SelectedIndex != 0)
        {
            branchSelectionCriteria.LocationID2= ddlbranch.SelectedValue;
        }
        branchSelectionCriteria.ProgramId = ProgramId;
        branchSelectionCriteria.UserId = UserId;
        branchSelectionCriteria.CMENDDATE = CutOffEndDate;
        byte[] bytestock = objSerClient.FunPubGetAccountDetails(branchSelectionCriteria);
        List<ClsPubBranchAccount> branchaccount = (List<ClsPubBranchAccount>)DeSerialize(bytestock);
        Session["Account"] = branchaccount;
        grvnoofaccounts.DataSource = branchaccount;
        grvnoofaccounts.DataBind();
        if (grvnoofaccounts.Rows.Count == 0)
        {
            grvnoofaccounts.EmptyDataText = "No records found";
            grvnoofaccounts.DataBind();
        }
        else
        {
            grvnoofaccounts.DataSource = branchaccount;
            grvnoofaccounts.DataBind();
        }


    }
    private void FunPriValidateFutureDate()
    {
        try
        {
            #region To find Current Year and Month
            //string Today = Convert.ToString(DateTime.Now);
            string YearMonth = txtCutoffMonthSearch.Text;
            int Currentmonth = DateTime.Now.Month;
            int Currentyear = DateTime.Now.Year;
            #endregion  
            //Commented and Added by saranya 
            //int Month = int.Parse(YearMonth.Substring(4, 2));
            //int year = int.Parse(YearMonth.Substring(0, 4));
            //if (year > Currentyear || Month > Currentmonth)
            //{
            //    txtCutoffMonthSearch.Text = "";
            //    Utility.FunShowAlertMsg(this, "Month/Year cannot be Greater than System month/Year.");
            //    return;
            //}

            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            if (year > Currentyear)
            {
                txtCutoffMonthSearch.Text = "";
                Utility.FunShowAlertMsg(this, "Year should not be Greater than the Current Year");
                return;
            }
            else if (year == Currentyear)
            {
                if (Month > Currentmonth)
                {
                    txtCutoffMonthSearch.Text = "";
                    Utility.FunShowAlertMsg(this, "Month should not be Greater than the Current Month");
                    return;
                }
            }
            //End
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected void grvpayment_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
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
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadLocation1(CompanyId, UserId, ProgramId, LobId);
        FunPriCleargrid();
        txtCutoffMonthSearch.Text = "";
        lblAmounts.Visible = false;
        FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
        ddlbranch.Enabled = false;
        
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
    private void FunPriDisplayTotalDetails()
    {

        if (grvNPAAccount.Rows.Count > 0)
        {
            ((LinkButton)grvNPAAccount.FooterRow.FindControl("LnkTotalBtnopening")).Text = Totalopeningaccount.ToString();
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotalopeningstock")).Text = Totalopeningstock.ToString(Funsetsuffix());
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotalopeningarrear")).Text = Totalopeningarrears.ToString(Funsetsuffix());
            ((LinkButton)grvNPAAccount.FooterRow.FindControl("LnkBtnTotalAddition")).Text = Totaladditionaccount.ToString();
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotaladditionstock")).Text = Totaladditionstock.ToString(Funsetsuffix());
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotaladditionarrear")).Text = Totaladditionarrear.ToString(Funsetsuffix());
            ((LinkButton)grvNPAAccount.FooterRow.FindControl("LnkBtnTotalDeletion")).Text = Totaldeletionaccount.ToString();
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotalDeletionstock")).Text = Totaldeletionstock.ToString(Funsetsuffix());
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotalDeletionarrear")).Text = Totaldeletionarrear.ToString(Funsetsuffix());
            ((LinkButton)grvNPAAccount.FooterRow.FindControl("LnkBtTotalclosingact")).Text = Totalclosingaccount.ToString();
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotalClosingstock")).Text = Totalclosingstock.ToString(Funsetsuffix());
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotalClosingarrear")).Text = Totalclosingarrear.ToString(Funsetsuffix());
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotalstock")).Text = Totalstock.ToString(Funsetsuffix());
            ((Label)grvNPAAccount.FooterRow.FindControl("lblTotalarrears")).Text = Totalarrear.ToString(Funsetsuffix());
        }



    }

    private void FunPriDisplayOpening()
    {

        if (grvOpeningaccountdetails.Rows.Count>0)
        {

            ((Label)grvOpeningaccountdetails.FooterRow.FindControl("lblTotalOpeningstock")).Text = TotalLnkOpeningStock.ToString(Funsetsuffix());
            ((Label)grvOpeningaccountdetails.FooterRow.FindControl("lblTotalOpeningArrear")).Text = TotalLnkOpeningArrear.ToString(Funsetsuffix());
        }



    }

    //private void FunPriTotalAsset()
    //{

    //    if (grvUnits.Rows.Count > 0)
    //    {

    //        ((Label)grvUnits.FooterRow.FindControl("lblTotalassetmonth")).Text = TotalAssetMonth.ToString();
    //        ((Label)grvUnits.FooterRow.FindControl("lblTotalassetytm")).Text = TotalAssetYear.ToString();
    //    }



    //}

    //private void FunPriDisplayTotalCollection()
    //{

    //    if (grvcollections.Rows.Count > 0)
    //    {

    //        ((Label)grvcollections.FooterRow.FindControl("lblTotalCurrentCollection")).Text = TotalCurrentCollection.ToString();
    //        ((Label)grvcollections.FooterRow.FindControl("lblTotalArrearCollection")).Text = TotalArrearCollection.ToString();
    //        ((Label)grvcollections.FooterRow.FindControl("lblTotalCollection")).Text = TotalCollection.ToString();



    //    }



    //}

    private void FunPriDisplayAdditionTotal()
    {

        if (grvAdditionAccountDetails.Rows.Count > 0)
        {

            ((Label)grvAdditionAccountDetails.FooterRow.FindControl("lblTotalAdditionstock")).Text = TotalLnkAdditionStock.ToString(Funsetsuffix());
            ((Label)grvAdditionAccountDetails.FooterRow.FindControl("lblTotalAdditionArrear")).Text = TotalLnkAdditionArrear.ToString(Funsetsuffix());
        }



    }

    private void FunPriDisplayDeletionTotal()
    {

        if (grvDeletionAccountDetails.Rows.Count > 0)
        {

            ((Label)grvDeletionAccountDetails.FooterRow.FindControl("lblTotalDeletionstock")).Text = TotalLnkDeletionStock.ToString(Funsetsuffix());
            ((Label)grvDeletionAccountDetails.FooterRow.FindControl("lblTotalDeletionArrear")).Text = TotalLnkDeletionArrear.ToString(Funsetsuffix());
        }



    }

    private void FunPriDisplayTotalClosingDetails()
    {

        if (grvClosingAccountDetails.Rows.Count > 0)
        {

            ((Label)grvClosingAccountDetails.FooterRow.FindControl("lblTotalClosingstock")).Text = TotalLnkClosingStock.ToString(Funsetsuffix());
            ((Label)grvClosingAccountDetails.FooterRow.FindControl("lblTotalClosingarrear")).Text = TotalLnkClosingArrear.ToString(Funsetsuffix());
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

    private bool FunFindLOBDemandMonth()
    {
        try
        {

            //string YearMonth = txtCutoffMonthSearch.Text;
            //int Month = int.Parse(YearMonth.Substring(0, 2));
            //int year = int.Parse(YearMonth.Substring(3, 4));
            //string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
            //DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
            //if (Month < 10)
            //{
            //    cutoff_month = Convert.ToString(year) + "0" + Convert.ToString(Month);
            //}
            //else
            //{
            //    cutoff_month = Convert.ToString(year) + Convert.ToString(Month);
            //}
            CutOffMonth = txtCutoffMonthSearch.Text;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyId.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@DemandMonth", CutOffMonth);
            int ErrorCode = 0;
            objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            ErrorCode = Convert.ToInt32(objS3GAdminServicesClient.FunGetScalarValue("S3G_RPT_GetLOBDemandMonth", Procparam));  //txtAmtFin.Text.Trim();
            if (ErrorCode == 0)
                return false;
            //Utility.FunShowAlertMsg(this, "Demand not run for the selected month ");
            //txtdate.Text = "";
            else
                return true;

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    private bool FunFindLOBDelinquentmonth()
    {
        try
        {

            //string YearMonth = txtCutoffMonthSearch.Text;
            //int Month = int.Parse(YearMonth.Substring(0, 2));
            //int year = int.Parse(YearMonth.Substring(3, 4));
            //string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
            //DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
            //if (Month < 10)
            //{
            //    cutoff_month = Convert.ToString(year) + "0" + Convert.ToString(Month);
            //}
            //else
            //{
            //    cutoff_month = Convert.ToString(year) + Convert.ToString(Month);
            //}
            CutOffMonth = txtCutoffMonthSearch.Text;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyId.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@DemandMonth", CutOffMonth);
            int ErrorCode = 0;
            objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            ErrorCode = Convert.ToInt32(objS3GAdminServicesClient.FunGetScalarValue("S3G_RPT_GetLOBDelinquentMonth", Procparam));  //txtAmtFin.Text.Trim();
            if (ErrorCode == 0)
                return false;
            //Utility.FunShowAlertMsg(this, "Demand not run for the selected month ");
            //txtdate.Text = "";
            else
                return true;

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private bool FunFindDemandMonth()
    {
        try
        {

            //string YearMonth = txtCutoffMonthSearch.Text;
            //int Month = int.Parse(YearMonth.Substring(0, 2));
            //int year = int.Parse(YearMonth.Substring(3, 4));
            //string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
            //DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
            //if (Month < 10)
            //{
            //    cutoff_month = Convert.ToString(year) + "0" + Convert.ToString(Month);
            //}
            //else
            //{
            //    cutoff_month = Convert.ToString(year) + Convert.ToString(Month);
            //}
            CutOffMonth = txtCutoffMonthSearch.Text;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyId.ToString());
            Procparam.Add("@DemandMonth", CutOffMonth);

            int ErrorCode = 0;
            objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            ErrorCode = Convert.ToInt32(objS3GAdminServicesClient.FunGetScalarValue("S3G_RPT_GetDemandMonth", Procparam));  //txtAmtFin.Text.Trim();
            if (ErrorCode == 0)
                return false;
            //Utility.FunShowAlertMsg(this, "Demand not run for the selected month ");
            //txtdate.Text = "";
            else
                return true;

            //DataSet ds = new DataSet();
            //ds = Utility.GetDataset("S3G_RPT_GetDemandMonth", Procparam);
            //if (ds.Tables[0].Rows.Count > 1)
            //{
            //    string StrErrorMsg = "Demand not run for the selected month ";
            //    Utility.FunShowAlertMsg(this, StrErrorMsg);
            //    txtdate.Text = "";
            //    return;
            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
}