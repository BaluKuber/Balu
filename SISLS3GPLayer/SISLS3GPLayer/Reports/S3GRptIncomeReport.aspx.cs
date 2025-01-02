
#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Income Report
/// Created By          :   Muni kavitha
/// Created Date        :   27-Oct-2011
/// Purpose             :   To Get the Income Report
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
using System.Collections.Generic;
using S3GBusEntity;
using ReportAccountsMgtServicesReference;
using ReportOrgColMgtServicesReference;
using System.ServiceModel;
using S3GBusEntity.Reports;
using System.Text;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class Reports_S3GRptIncomeReport : ApplyThemeForProject
{

    #region Variable Declaration
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    string intCustomerId;
    int intUserId;

    bool Is_Active;
    int Active;
    int ProgramId = 203;
    string strFilePath = string.Empty;
    Dictionary<string, string> Procparam;
    string strPageName = "Income Report";
    DataTable dtTable = new DataTable();
    List<decimal> lisTotaldec = new List<decimal>();
    ArrayList arlist = new ArrayList();
    ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient objReportOrgMgtServices;
    ReportAccountsMgtServicesClient objReportAccountMgtServices;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
    String strForLOB = string.Empty;
    List<int> lstLOBID = new List<int>();
    //decimal BCD, BCDAmt, BDD, BDDAmt, BCM, BCMAmt, BDM, BDMAmt, BCY, BCYAmt, BDY, BDYAmt;
    //decimal BDYAmt;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPriLoadPage();
          
            //btnPDF.Attributes.Add("OnClientClick", "return fn_IsLetterExists();");

        }
        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = "Due to Data Problem, Unable to Load Page";
            CVIncomeReport.IsValid = false;
        }

    }


    # region Page Methods
    /// <summary>
    /// This Method is called when page is Loding
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            //ObjS3GSession = new S3GSession();
            //Session["AccountingCurrency"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            txtCutoff.Attributes.Add("readonly","true");
            if (!IsPostBack)
            {
                FunPriLoadLob();
                FunPriGetBranch();
                //FunPriLoadLocation();
                //FunPriLoadLocation(0);
                FunPubLoadDenomination();
                if (gvIncome.Rows.Count <= 0)
                    lblAmounts.Visible = pnlIncome.Visible = pnlAccounts.Visible = btnPrintDetails.Visible = false;
                //ddlReportType.SelectedValue = "2";
                //Utility.ClearDropDownList(ddlReportType);
                // FunPriLoadBranch(intCompanyId, intUserId, Is_Active);
            }
            ddlLocation1.AddItemToolTip();
            ddlLocation2.AddItemToolTip();
            ddlReportType.AddItemToolTip();
            ddlDenomination.AddItemToolTip();
            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Repayment Schedule page");
        }
    }
    /// <summary>
    /// To Load Denomination
    /// </summary>
    public void FunPubLoadDenomination()
    {
        try
        {
            objReportAccountMgtServices = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objReportAccountMgtServices.GetDenominations();
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
            if (objReportAccountMgtServices != null)
                objReportAccountMgtServices.Close();
        }
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

    /// <summary>
    /// This Method is called when page is Loading.
    /// To Load the Line of Business in Dropdown List.
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id"></param>
    private void FunPriLoadLob()
    {
        objReportOrgMgtServices = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
        try
        {
            //objReportOrgMgtServices=new ReportOrgColMgtServicesClient();
            //ddlLOB.Items.Clear();
            //byte[] byteLobs = objReportOrgMgtServices.FunPubLOBIncome(ObjUserInfo.ProCompanyIdRW, ObjUserInfo.ProUserIdRW, 1, ProgramId);
            //List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);


            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            Procparam.Add("@Option", "1");
            Procparam.Add("@Program_ID", ProgramId.ToString());
            DataTable dtLOB = Utility.GetDefaultData(SPNames.GetLOB, Procparam);
            if (dtLOB.Rows.Count == 0)
            {
                gvLoadLOB.EmptyDataText = "User has not mapped to any Line of Business</br>Report Cannot be generated";
                btnGo.Enabled = false;
            }
            else
                btnGo.Enabled = true;

            gvLoadLOB.DataSource = dtLOB;
            gvLoadLOB.DataBind();
            //ddlLOB.DataSource = lobs;
            //ddlLOB.DataTextField = "Description";
            //ddlLOB.DataValueField = "ID";
            //ddlLOB.DataBind();
            //ddlLOB.Items[0].Text = "--ALL--";
            //if (ddlLOB.Items.Count == 2)
            //{
            //    ddlLOB.SelectedIndex = 1;

            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            if (objReportOrgMgtServices != null)
                objReportOrgMgtServices.Close();
        }
    }





    /// <summary>
    ///  This Method is called when page is Loading.
    ///  To Load the Branch in Dropdown List.
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id"></param>
    /// <param name="Is_active"></param>
    private void FunPriGetBranch()
    {
        //objReportAccountMgtServices = new ReportAccountsMgtServicesClient();
        objReportOrgMgtServices = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
        try
        {
            // ddlLocation1.Items.Clear();
            int intlob_Id = 0;
            //if (ddlLOB.SelectedIndex > 0)
            intlob_Id = 0;//Convert.ToInt32(ddlLOB.SelectedValue);
           // string strLOB="";
            if (!FunPriValidateLOB())
                strForLOB = null;
            else
                GenerateXMLLOB();
           // GenerateXMLLOB();
            //byte[] byteLobs = objReportAccountMgtServices.FunPubBranch(intCompanyId, intUserId, ProgramId, intlob_Id);
            //List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            byte[] byteLobs = objReportOrgMgtServices.FunPubLoc1_MultiLOB(intCompanyId, intUserId, ProgramId, strForLOB);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlLocation1.DataSource = Branch;
            ddlLocation1.DataTextField = "Description";
            ddlLocation1.DataValueField = "ID";
            ddlLocation1.DataBind();
            ddlLocation1.Items[0].Text = "--ALL--";
            //if (ddlLocation1.Items.Count == 2)
            //{
            //    ddlLocation1.SelectedIndex = 1;
            //}
            //else
            //    ddlLocation1.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            //objReportAccountMgtServices.Close();
            objReportOrgMgtServices.Close();
        }
    }

    private void FunPriLoadLocation()
    {
        //objReportAccountMgtServices = new ReportAccountsMgtServicesClient();
        objReportOrgMgtServices = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();

        try
        {
            //ddlLocation1.Items.Clear();
            int intlob_Id = 0;
            //if (ddlLOB.SelectedIndex > 0)
               // intlob_Id =object// Convert.ToInt32(ddlLOB.SelectedValue);
            if (!FunPriValidateLOB())
                strForLOB = null;
            else
                GenerateXMLLOB();
            //byte[] byteLobs = objReportAccountMgtServices.FunPubBranch(intCompanyId, intUserId, ProgramId, intlob_Id);
            //List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            byte[] byteLobs = objReportOrgMgtServices.FunPubLoc1_MultiLOB(intCompanyId, intUserId, ProgramId, strForLOB );
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);


            ddlLocation2.DataSource = Branch;
            ddlLocation2.DataTextField = "Description";
            ddlLocation2.DataValueField = "ID";
            ddlLocation2.DataBind();
            ddlLocation2.Items[0].Text = "--ALL--";
            //if (ddlLocation1.Items.Count == 2)
            //{
            //    ddlLocation1.SelectedIndex = 1;
            //}
            //else
            //    ddlLocation1.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            //objReportAccountMgtServices.Close();
            objReportOrgMgtServices.Close();
        }
    }

    private void FunPriLoadLocation2()
    {
        //objReportAccountMgtServices = new ReportAccountsMgtServicesClient();
        objReportOrgMgtServices = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
        try
        {
            int lobId = 0;
            //if (ddlLOB.SelectedIndex > 0)
            //    lobId = Convert.ToInt32(ddlLOB.SelectedValue);
            int Location1 = 0;
            if (!FunPriValidateLOB())
                strForLOB = null;
            else
                GenerateXMLLOB();

            if (ddlLocation1.SelectedIndex > 0)
                Location1 = Convert.ToInt32(ddlLocation1.SelectedValue);
            //byte[] byteLobs = objReportAccountMgtServices.FunPubGetLocation2(ProgramId, intUserId, intCompanyId, lobId, Location1);
            //List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            byte[] byteLobs = objReportOrgMgtServices.FunPubLoc2_MultiLOB(ProgramId, intUserId, intCompanyId, strForLOB, Location1);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);


            ddlLocation2.DataSource = Branch;
            ddlLocation2.DataTextField = "Description";
            ddlLocation2.DataValueField = "ID";
            ddlLocation2.DataBind();
            //if (ddlLocation2.Items.Count == 2)
            //{
            //    if (ddlLocation1.SelectedIndex != 0)
            //    {
            //        ddlLocation2.SelectedIndex = 1;
            //        Utility.ClearDropDownList(ddlLocation2);
            //    }
            //    else
            //        ddlLocation2.SelectedIndex = 0;
            //}
            //else
            //{
            ddlLocation2.Items[0].Text = "--ALL--";
            //ddlLocation2.SelectedIndex = 0;
            //}
            if (ddlLocation2.Items.Count == 2)
            {
                ddlLocation2.SelectedIndex = 1;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            //objReportAccountMgtServices.Close();
            objReportOrgMgtServices.Close();
        }
    }


    //protected void txtCutoff_OnTextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        FunPriValidateFutureDate();
    //        FunPriLoadDebtCode(CompanyId, LOB_ID, Branch_ID, cutoff_month);
    //        PnlDetails.Visible = false;
    //        // btnPrint.Visible = false;
    //        //txtdate .Text =txtdate .Text .ToString("y",CultureInfo.CreateSpecificCulture("af-ZA"));
    //        // txtdate.Text = Convert .ToDateTime ( txtdate.Text).ToString("MMM yyyy");
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}





    private void FunPriLoadLocation(int OptionLoc)
    {
        objReportAccountMgtServices = new ReportAccountsMgtServicesClient();
        try
        {
            //ddlLocation1.Items.Clear();
            int intlob_Id = 0;
            //if (ddlLOB.SelectedIndex > 0)
            //    intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            int Location1 = 0;
            if (ddlLocation1.SelectedIndex > 0)
                Location1 = Convert.ToInt32(ddlLocation1.SelectedValue);
            //byte[] byteLobs = objReportAccountMgtServices.FunPubBranch(intCompanyId, intUserId, ProgramId, intlob_Id);
            //List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            byte[] byteLobs = objReportAccountMgtServices.FunPubGetLocation2(ProgramId, intUserId, intCompanyId, intlob_Id, Location1);
            List<ClsPubDropDownList> Location = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            if (OptionLoc == 0)
            {

                ddlLocation1.DataSource = Location;
                ddlLocation1.DataTextField = "Description";
                ddlLocation1.DataValueField = "ID";
                ddlLocation1.DataBind();
                ddlLocation1.Items[0].Text = "--ALL--";

                ddlLocation2.DataSource = Location;
                ddlLocation2.DataTextField = "Description";
                ddlLocation2.DataValueField = "ID";
                ddlLocation2.DataBind();
                ddlLocation2.Items[0].Text = "--ALL--";
            }
            else if (OptionLoc == 1)
            {
                ddlLocation2.DataSource = Location;
                ddlLocation2.DataTextField = "Description";
                ddlLocation2.DataValueField = "ID";
                ddlLocation2.DataBind();
                ddlLocation2.Items[0].Text = "--ALL--";
            }
            //if (ddlLocation1.Items.Count == 2)
            //{
            //    ddlLocation1.SelectedIndex = 1;
            //}
            //else
            //    ddlLocation1.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objReportAccountMgtServices.Close();
        }
    }


    protected void ddlLocation1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           // FunPriLoadLocation(1);
            if (ddlLocation1.SelectedIndex > 0)
            {
                ddlLocation2.Enabled = true;
                FunPriLoadLocation2();

            }
            else
            {
                //FunPriGetBranch();
                ddlLocation2.Enabled = false;
                FunPriLoadLocation();
                ddlLocation2.Enabled = false;

            }
            ddlReportType.ClearSelection();
            ddlDenomination.ClearSelection();
            txtCutoff.Text = "";
            FunClearGrid();
            ddlLocation2.AddItemToolTip();

        }

        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = "Due to Data Problem, Unable to Load Prime Account Number.";
            CVIncomeReport.IsValid = false;
        }
    }

    protected void ddlLocation2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlReportType.ClearSelection();
            ddlDenomination.ClearSelection();
            txtCutoff.Text = "";
            FunClearGrid();
        }

        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = "Due to Data Problem, Unable to Load Prime Account Number.";
            CVIncomeReport.IsValid = false;
        }
    }

    private bool FunPriValidateLOB()
    {
        if (gvLoadLOB.Rows.Count > 0)
        {
            int i = 0;
            for (i = 0; i < gvLoadLOB.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)gvLoadLOB.Rows[i].FindControl("chkSelect");
                if (chk != null)
                {
                    if (chk.Checked == true)
                        return true;
                }
            }
        }
        return false;
    }

    private void  GenerateXMLLOB()
    {
        if (gvLoadLOB.Rows.Count > 0)
        {
            
            strForLOB = "<Root>";
            int i;
            for (i = 0; i < 4; i++)
            {
                if (i < gvLoadLOB.Rows.Count)
                {
                    Label lbl = (Label)gvLoadLOB.Rows[i].FindControl("lblgvLOBID");
                    CheckBox chk = (CheckBox)gvLoadLOB.Rows[i].FindControl("chkSelect");
                    if (lbl != null && chk != null)
                    {
                        if (chk.Checked == true)
                        {
                            strForLOB = strForLOB + "<Details LOB_ID='" + lbl.Text + "'/>";
                            lstLOBID.Add(Convert.ToInt32(lbl.Text));
                        }
                        else
                        {
                            strForLOB = strForLOB + "<Details LOB_ID='0'/>";
                            lstLOBID.Add(0);
                        }
                    }
                }
                else
                {
                    lstLOBID.Add(0);
                    strForLOB = strForLOB + "<Details LOB_ID='0'/>";
                }
            }
            strForLOB = strForLOB + "</Root>";
            //if(!string .IsNullOrEmpty (strForLOB))
            
        }
        //return strForLOB;
    }

    //private string GenerateXMLLOB()
    //{ 
    //       String s = "<Root> <Details LOB_ID='2'/> <Details LOB_ID='3'/><Details LOB_ID='1'/><Details LOB_ID='4'/></Root>";
    //       return s;
    //}

    private string GetIncRecDate(string startDate, string EndDate, string LOBXml)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Program_ID", ProgramId.ToString());

        if (ddlLocation1.SelectedIndex > 0)
            Procparam.Add("@LOCATION_ID1", ddlLocation1.SelectedValue);
        if (ddlLocation2.SelectedIndex > 0)
            Procparam.Add("@LOCATION_ID2", ddlLocation2.SelectedValue);

        Procparam.Add("@CutoffMonth", startDate);
        Procparam.Add("@CutoffMonth_Date", EndDate);
        Procparam.Add("@XML_LOBID", LOBXml);

        DataTable dt = Utility.GetDefaultData("S3G_RPT_ValidateIncRec", Procparam);
        if (dt.Rows.Count > 0)
            return dt.Rows[0][0].ToString();
        else
            return null;
    }

    protected void FunCutOffDates(out string CutoffMonthStart, out string CutoffMonthEnd, out string CutoffFYStartDate, out string CMEndDate, out string CFYEndDate, out string PFYStartDate)
    {
        int FinYearStartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
        int FinYearEndMonth;

        if (FinYearStartMonth == 0)
            FinYearEndMonth = 12;
        else
            FinYearEndMonth = FinYearStartMonth - 1;

        //********current Financial year date details*********//
        // string strCurrentDate = txtCutoff.Text; //DateTime.Now.ToString("yyyyMMdd");
        string strCurrentDate = DateTime.Now.ToString("yyyyMMdd");
        int Currentyear = int.Parse(strCurrentDate.Substring(0, 4));
        int CurrentMonth = int.Parse(strCurrentDate.Substring(4, 2));
        //int Currentyear = int.Parse(strCurrentDate.Substring(3, 4));
        //int CurrentMonth = int.Parse(strCurrentDate.Substring(0, 2));

        string CurrentDate = DateTime.Now.ToString("MM/dd/yyyy");

        string FinancialYear = "";
        string PreviousFinYear = "";
        if (FinYearStartMonth <= CurrentMonth)
        {
            FinancialYear = Convert.ToString(Currentyear + 1);
            PreviousFinYear = Convert.ToString(Currentyear);
            //FinancialYear = Convert.ToString(Currentyear);
            //PreviousFinYear = Convert.ToString(Currentyear - 1);
        }
        else
        {
            FinancialYear = Convert.ToString(Currentyear);
            PreviousFinYear = Convert.ToString(Currentyear - 1);
            //FinancialYear = Convert.ToString(Currentyear - 1);
            //PreviousFinYear = Convert.ToString(Currentyear - 2);
        }

        string count = System.DateTime.DaysInMonth(Convert.ToInt32(PreviousFinYear), FinYearEndMonth).ToString();

        string FinYearStartDate = FinYearStartMonth.ToString() + "/" + "1" + "/" + FinancialYear;
        string PreviousYearStartDate = FinYearStartMonth.ToString() + "/" + "1" + "/" + PreviousFinYear;
        string PreviousYearEndDate = FinYearEndMonth.ToString() + "/" + count + "/" + PreviousFinYear;


        //*****************//

        //***** Getting Current Financial Year values based on current Date************//

        int FYStartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
        int FYEndMonth;
        if (FYStartMonth == 0)
            FYEndMonth = 12;
        else
            FYEndMonth = FYStartMonth - 1;

        string CDate = DateTime.Now.ToString("yyyyMMdd");
        int Cyear = int.Parse(CDate.Substring(0, 4));
        int CMonth = int.Parse(CDate.Substring(4, 2));

        int CFinYear, PFinYear;
        if (FYStartMonth <= CMonth)
        {
            CFinYear = Cyear;
            PFinYear = Cyear - 1;
        }
        else
        {
            CFinYear = Cyear - 1;
            PFinYear = Cyear - 2;
        }

        string CFYStartDate = FYStartMonth.ToString() + "/" + "1" + "/" + CFinYear.ToString();
         CFYEndDate = FYEndMonth.ToString() + "/" + System.DateTime.DaysInMonth(CFinYear, FYEndMonth).ToString() + "/" + (CFinYear + 1).ToString();

         PFYStartDate = FYStartMonth.ToString() + "/" + "1" + "/" + PFinYear.ToString();
        string PFYEndDate = FYEndMonth.ToString() + "/" + System.DateTime.DaysInMonth(PFinYear, FYEndMonth).ToString() + "/" + CFinYear.ToString();

        string CFYPeriod = (CFinYear).ToString() + "-" + (CFinYear + 1).ToString();
        string PFYPeriod = (PFinYear).ToString() + "-" + (PFinYear + 1).ToString();

        string CMStartDate = CMonth.ToString() + "/1/" + Cyear.ToString();
         CMEndDate = CMonth.ToString() + "/" + System.DateTime.DaysInMonth(Cyear, CMonth).ToString() + "/" + Cyear.ToString();

        //*****************//

        //string CutoffMonthDate = Month + "/" + DaysinMonth + "/" + year;


        //********cutoff month date details*********//
        string strCutoffDate = txtCutoff.Text;//.ToString("mmyyyy");
        int year = int.Parse(strCutoffDate.Substring(3, 4));
        int Month = int.Parse(strCutoffDate.Substring(0, 2));
        string DaysinMonth = System.DateTime.DaysInMonth(year, Month).ToString();

         CutoffMonthStart = Month + "/" + "1" + "/" + year;
         CutoffMonthEnd = Month + "/" + DaysinMonth + "/" + year;
        int Cutoffyear;
        if (FYStartMonth <= Month)
        {
            Cutoffyear = year;
        }
        else
        {
            Cutoffyear = year - 1;
        }
         CutoffFYStartDate = FYStartMonth + "/" + "1" + "/" + Cutoffyear;


    }

    private bool FunPriLoadIncomeGrid()
    {
        objReportOrgMgtServices = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();

        try
        {
            GenerateXMLLOB();
            //string CutoffMonthStart="";
            //string CutoffMonthEnd="";
            //string CutoffFYStartDate="";
            //string CMEndDate ="";
            ////string CutoffMonthEnd ="";
            //string CFYEndDate ="";
            //string PFYStartDate = "";
            //FunCutOffDates(out CutoffMonthStart, out CutoffMonthEnd, out CutoffFYStartDate, out CMEndDate,  out CFYEndDate, out PFYStartDate);

            //****Code For Date Validation***//
            int FinYearStartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
            int FinYearEndMonth;

            if (FinYearStartMonth == 0)
                FinYearEndMonth = 12;
            else
                FinYearEndMonth = FinYearStartMonth - 1;

            //********current Financial year date details*********//
            // string strCurrentDate = txtCutoff.Text; //DateTime.Now.ToString("yyyyMMdd");
            string strCurrentDate = DateTime.Now.ToString("yyyyMMdd");
            int Currentyear = int.Parse(strCurrentDate.Substring(0, 4));
            int CurrentMonth = int.Parse(strCurrentDate.Substring(4, 2));
            //int Currentyear = int.Parse(strCurrentDate.Substring(3, 4));
            //int CurrentMonth = int.Parse(strCurrentDate.Substring(0, 2));

            string CurrentDate = DateTime.Now.ToString("MM/dd/yyyy");

            string FinancialYear = "";
            string PreviousFinYear = "";
            if (FinYearStartMonth <= CurrentMonth)
            {
                FinancialYear = Convert.ToString(Currentyear + 1);
                PreviousFinYear = Convert.ToString(Currentyear);
                //FinancialYear = Convert.ToString(Currentyear);
                //PreviousFinYear = Convert.ToString(Currentyear - 1);
            }
            else
            {
                FinancialYear = Convert.ToString(Currentyear);
                PreviousFinYear = Convert.ToString(Currentyear - 1);
                //FinancialYear = Convert.ToString(Currentyear - 1);
                //PreviousFinYear = Convert.ToString(Currentyear - 2);
            }

            string count = System.DateTime.DaysInMonth(Convert.ToInt32(PreviousFinYear), FinYearEndMonth).ToString();

            string FinYearStartDate = FinYearStartMonth.ToString() + "/" + "1" + "/" + FinancialYear;
            string PreviousYearStartDate = FinYearStartMonth.ToString() + "/" + "1" + "/" + PreviousFinYear;
            string PreviousYearEndDate = FinYearEndMonth.ToString() + "/" + count + "/" + PreviousFinYear;


            //*****************//

            //***** Getting Current Financial Year values based on current Date************//

            int FYStartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
            int FYEndMonth;
            if (FYStartMonth == 0)
                FYEndMonth = 12;
            else
                FYEndMonth = FYStartMonth - 1;

            string CDate = DateTime.Now.ToString("yyyyMMdd");
            int Cyear = int.Parse(CDate.Substring(0, 4));
            int CMonth = int.Parse(CDate.Substring(4, 2));

            int CFinYear, PFinYear;
            if (FYStartMonth <= CMonth)
            {
                CFinYear = Cyear;
                PFinYear = Cyear - 1;
            }
            else
            {
                CFinYear = Cyear - 1;
                PFinYear = Cyear - 2;
            }

            string CFYStartDate = FYStartMonth.ToString() + "/" + "1" + "/" + CFinYear.ToString();
            string CFYEndDate = FYEndMonth.ToString() + "/" + System.DateTime.DaysInMonth(CFinYear, FYEndMonth).ToString() + "/" + (CFinYear + 1).ToString();

            string PFYStartDate = FYStartMonth.ToString() + "/" + "1" + "/" + PFinYear.ToString();
            string PFYEndDate = FYEndMonth.ToString() + "/" + System.DateTime.DaysInMonth(PFinYear, FYEndMonth).ToString() + "/" + CFinYear.ToString();

            string CFYPeriod = (CFinYear).ToString() + "-" + (CFinYear + 1).ToString();
            string PFYPeriod = (PFinYear).ToString() + "-" + (PFinYear + 1).ToString();

            string CMStartDate = CMonth.ToString() + "/1/" + Cyear.ToString();
            string CMEndDate = CMonth.ToString() + "/" + System.DateTime.DaysInMonth(Cyear, CMonth).ToString() + "/" + Cyear.ToString();

            //*****************//

            //string CutoffMonthDate = Month + "/" + DaysinMonth + "/" + year;


            //********cutoff month date details*********//
            string strCutoffDate = txtCutoff.Text;//.ToString("mmyyyy");
            int year = int.Parse(strCutoffDate.Substring(3, 4));
            int Month = int.Parse(strCutoffDate.Substring(0, 2));
            string DaysinMonth = System.DateTime.DaysInMonth(year, Month).ToString();

            string CutoffMonthStart = Month + "/" + "1" + "/" + year;
            string CutoffMonthEnd = Month + "/" + DaysinMonth + "/" + year;
            int Cutoffyear;
            if (FYStartMonth <= Month)
            {
                Cutoffyear = year;
            }
            else
            {
                Cutoffyear = year - 1;
            }
            string CutoffFYStartDate = FYStartMonth + "/" + "1" + "/" + Cutoffyear;

            //*****************//
            string IncRecDate = "";
            //****End of Code For Date Validation***//
            //int intResult;
            if (ddlReportType.SelectedValue == "1")
            {
                if (Convert.ToDateTime(PFYStartDate) > Convert.ToDateTime(CutoffMonthStart) || Convert.ToDateTime(CutoffMonthEnd) > Convert.ToDateTime(CMEndDate))
                {
                    FunClearGrid();
                    //txtCutoff.Text = "";
                    Utility.FunShowAlertMsg(this.Page, "For Report Type Actual:\\nCutoff Month Should be between Previous Financial Year Start Month and Current Month");
                    return false;//"Cutoff Month Should be between Previous Financial Year and Current Month"
                }
                else if (Month == CMonth)
                {
                    IncRecDate = GetIncRecDate(CutoffMonthStart, CutoffMonthEnd, strForLOB);
                    if (string .IsNullOrEmpty (IncRecDate))
                    {
                        FunClearGrid();
                        //txtCutoff.Text = "";
                        Utility.FunShowAlertMsg(this.Page, "Income Recognition not Processed for the Selected Cutoff Month");
                        return false;

                    }
                    else if (Convert.ToDateTime(CutoffMonthEnd) > Convert.ToDateTime(IncRecDate))
                    {
                        FunClearGrid();
                        //txtCutoff.Text = "";
                        Utility.FunShowAlertMsg(this.Page, "Income Recognition not Processed for the Selected Cutoff Month");
                        return false;
                    }
                }
            }
            else if (ddlReportType.SelectedValue == "2")
            {
                //if (Convert.ToDateTime(CutoffMonthEnd) > Convert.ToDateTime(CFYEndDate) || Convert.ToDateTime(CutoffMonthEnd) < Convert.ToDateTime(CMEndDate))
                if (Month == CMonth)
                {
                    IncRecDate = GetIncRecDate(CutoffMonthStart, CutoffMonthEnd, strForLOB);
                    if (IncRecDate!=null)
                    {
                        FunClearGrid();
                       // txtCutoff.Text = "";
                        Utility.FunShowAlertMsg(this.Page, "For Report Type Forecast:\\nIncome Recognition has been Processed. Cannot view for Selected Cutoff Month\\n");
                        return false;
                    }
                }
                else if (Convert.ToDateTime(CutoffMonthEnd) < Convert.ToDateTime(CMEndDate) || Convert.ToDateTime(CutoffMonthEnd) > Convert.ToDateTime(CFYEndDate))
                {
                    FunClearGrid();
                    //txtCutoff.Text = "";
                    Utility.FunShowAlertMsg(this.Page, "Cutoff Month Should be Greater than Current Month and with in the Current Financial Year");
                    //Utility.FunShowAlertMsg(this.Page, "Cutoff Month Should be between Previous Financial Year and Current Month");
                    return false;
                }
            }
            //else if (Month == CMonth)
            //{
            //    Procparam = new Dictionary<string, string>();
            //    Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            //    Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            //    Procparam.Add("@Program_ID", ProgramId.ToString());
                
            //    if (ddlLocation1.SelectedIndex > 0)
            //        Procparam.Add("@LOCATION_ID1", ddlLocation1 .SelectedValue);
            //    if (ddlLocation2.SelectedIndex > 0)
            //        Procparam.Add("@LOCATION_ID2", ddlLocation2 .SelectedValue);

            //    Procparam.Add("@CutoffMonth", CutoffMonthStart);
            //    Procparam.Add("@CutoffMonth_Date", CutoffMonthEnd);
            //    Procparam.Add("@XML_LOBID", strForLOB );

            //    DataTable dt = Utility.GetDefaultData("S3G_RPT_ValidateIncRec", Procparam);
            //    string IncRecDate = dt.Rows[0][0].ToString();
            //    if (Convert.ToDateTime(CutoffMonthEnd) > IncRecDate)
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "Income Recognition not done for selected cutoff Month");
            //        return false;
            //    }


            //}
             
//***************************************************///
//            GenerateXMLLOB();
            ClsPubIncomeReportParams IncomeParams = new ClsPubIncomeReportParams();

            IncomeParams.CompanyId = ObjUserInfo.ProCompanyIdRW;
            IncomeParams.User_ID = ObjUserInfo.ProUserIdRW;
            IncomeParams.Program_ID =  ProgramId;

            // if(ddlLOB .SelectedIndex >0)
            //IncomeParams.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            //IncomeParams = Convert.ToInt32(ddlLOB.SelectedValue);
            IncomeParams.LOB_ID1 = lstLOBID[0];
            IncomeParams.LOB_ID2 = lstLOBID[1];
            IncomeParams.LOB_ID3 = lstLOBID[2];
            IncomeParams.LOB_ID4 = lstLOBID[3];
            //if (ddlLocation1.SelectedIndex > 0)
            IncomeParams.Location_ID1 = Convert.ToInt32(ddlLocation1.SelectedValue);
            //if (ddlLocation2.SelectedIndex > 0)
            IncomeParams.Location_ID2 = Convert.ToInt32(ddlLocation2.SelectedValue);

            IncomeParams.Report_Type = Convert.ToInt32(ddlReportType.SelectedValue);


            IncomeParams.CutoffMonth = CutoffMonthStart;
            IncomeParams.CutoffMonth_Date = CutoffMonthEnd;
            IncomeParams.FinYearstart = CutoffFYStartDate;// FinYearStartDate;
            // IncomeParams.CurrentFinYear  = CurrentMonthStart;
            // IncomeParams.PreviousFinYear  = FinYearStartDate;
            IncomeParams.Denomintion = Convert.ToDecimal(ddlDenomination.SelectedValue);
            IncomeParams.XMLLOB_ID = strForLOB;// GenerateXMLLOB();
            //IncomeParams.XMLLOB_ID = GenerateXMLLOB();
            byte[] byteIncome = objReportOrgMgtServices.FunPubGetIncomeReport(IncomeParams);
            //List<ClsPubIncomeReport> ListIncomeReport = (List<ClsPubIncomeReport>)DeSeriliaze(byteIncome);
            ClsPubIncomeReport clsIncomeReport = (ClsPubIncomeReport)DeSeriliaze(byteIncome);
            lblAmounts.Visible = pnlIncome.Visible =  true;


            ViewState ["CutoffMonthStart"]=CutoffMonthStart;
            ViewState ["CutoffMonthEnd"]=CutoffMonthEnd;
            ViewState["CutoffFYStartDate"] = CutoffFYStartDate;

            if (clsIncomeReport.IncomeDetails.Count == 0)
            {

                gvIncome.DataSource = null;
                gvIncome.DataBind();
                // gvIncome.Visible = false;
                // gvIncome.EmptyDataText = "No records Found";
                lblNoRecords.Visible = true;
                 btnPrint.Visible = false;
            }
            else
            {
                gvIncome.Visible = true;
                gvIncome.DataSource = clsIncomeReport.IncomeDetails;
                gvIncome.DataBind();
                lblNoRecords.Visible = false;
                decimal decMonth;
                decimal decYear,decUMFC;
                decMonth = clsIncomeReport.IncomeDetails.Sum(ClsPubIncomeReport => ClsPubIncomeReport.Month);
                decYear = clsIncomeReport.IncomeDetails.Sum(ClsPubIncomeReport => ClsPubIncomeReport.Year);
                decUMFC = clsIncomeReport.IncomeDetails.Sum(ClsPubIncomeReport => ClsPubIncomeReport.DataRow2);
                Label lblTotMonth = (Label)gvIncome.FooterRow.FindControl("lblTotMonth");
                Label lblTotYear = (Label)gvIncome.FooterRow.FindControl("lblTotYear");
                Label lblTotUMFC = (Label)gvIncome.FooterRow.FindControl("lblTotUMFC");

                if (lblTotMonth != null)
                    lblTotMonth.Text = decMonth.ToString();
                if (lblTotYear != null)
                    lblTotYear.Text = decYear.ToString();
                if (lblTotUMFC != null)
                    lblTotUMFC.Text = decUMFC.ToString();

                btnPrint.Visible = true;
            }
            ////DataTable dt = new DataTable();
            //  //  dt=(DataTable)clsIncomeReport.IncomeDetails;
            Session["LOBName1"] = Session["LOBName2"] = Session["LOBName3"] = Session["LOBName4"] = null;
            Session["LOB1"] = Session["LOB2"] = Session["LOB3"] = Session["LOB4"] = null;

            //COMMENTED CODE FOR LOADIN LOB BASED REPORT//

            //Session["IncomeDetails"] = clsIncomeReport.IncomeDetails;
            Session["IncomeDetails"] = clsIncomeReport.MainReportLoc;

            if (clsIncomeReport.LOB1.Count > 0)
            {
                Session["LOBName1"] = clsIncomeReport.LOB1[0].LOBName.ToString();
                Session["LOB1"] = clsIncomeReport.LOB1;
                if (clsIncomeReport.LOB2.Count > 0)
                {
                    Session["LOBName2"] =clsIncomeReport.LOB2[0].LOBName.ToString();
                    Session["LOB2"] = clsIncomeReport.LOB2;
                    if (clsIncomeReport.LOB3.Count > 0)
                    {
                        Session["LOBName3"] = clsIncomeReport.LOB3[0].LOBName.ToString();
                        Session["LOB3"] = clsIncomeReport.LOB3;
                        if (clsIncomeReport.LOB4.Count > 0)
                        {
                            Session["LOBName4"] = clsIncomeReport.LOB4[0].LOBName.ToString();
                            Session["LOB4"] = clsIncomeReport.LOB4;
                        }
                    }
                    else if (clsIncomeReport.LOB4.Count > 0)
                    {
                        Session["LOBName3"] = clsIncomeReport.LOB4[0].LOBName.ToString();
                        Session["LOB3"] = clsIncomeReport.LOB4;
                    }
                }
                else if (clsIncomeReport.LOB3.Count > 0)
                {
                    Session["LOBName2"] = clsIncomeReport.LOB3[0].LOBName.ToString();
                    Session["LOB2"] = clsIncomeReport.LOB3;
                    if (clsIncomeReport.LOB4.Count > 0)
                    {
                        Session["LOBName3"] = clsIncomeReport.LOB4[0].LOBName.ToString();
                        Session["LOB3"] = clsIncomeReport.LOB4;
                    }
                }
                else if (clsIncomeReport.LOB4.Count > 0)
                {
                    Session["LOBName2"] = clsIncomeReport.LOB4[0].LOBName.ToString();
                    Session["LOB2"] = clsIncomeReport.LOB4;
                }
            }
            else if (clsIncomeReport.LOB2.Count > 0)
            {
                Session["LOBName1"] = clsIncomeReport.LOB2[0].LOBName.ToString();
                Session["LOB1"] = clsIncomeReport.LOB2;
                if (clsIncomeReport.LOB3.Count > 0)
                {
                    Session["LOBName2"] = clsIncomeReport.LOB3[0].LOBName.ToString();
                    Session["LOB2"] = clsIncomeReport.LOB3;
                    if (clsIncomeReport.LOB4.Count > 0)
                    {
                        Session["LOBName3"] = clsIncomeReport.LOB4[0].LOBName.ToString();
                        Session["LOB3"] = clsIncomeReport.LOB4;
                    }
                }
                else if (clsIncomeReport.LOB4.Count > 0)
                {
                    Session["LOBName2"] = clsIncomeReport.LOB4[0].LOBName.ToString();
                    Session["LOB2"] = clsIncomeReport.LOB4;
                }

            }
            else if (clsIncomeReport.LOB3.Count > 0)
            {
                Session["LOBName1"] = clsIncomeReport.LOB3[0].LOBName.ToString();
                Session["LOB1"] = clsIncomeReport.LOB3;
                if (clsIncomeReport.LOB4.Count > 0)
                {
                    Session["LOBName2"] = clsIncomeReport.LOB4[0].LOBName.ToString();
                    Session["LOB2"] = clsIncomeReport.LOB4;
                }
            }
            else if (clsIncomeReport.LOB4.Count > 0)
            {
                Session["LOBName1"] = clsIncomeReport.LOB4[0].LOBName.ToString();
                Session["LOB1"] = clsIncomeReport.LOB4;
            }


            if (Session["LOB1"] == null)
                Session["LOB1"] = clsIncomeReport.SampleLOB;
            if (Session["LOB2"] == null)
                Session["LOB2"] = clsIncomeReport.SampleLOB;
            if (Session["LOB3"] == null)
                Session["LOB3"] = clsIncomeReport.SampleLOB;
            if (Session["LOB4"] == null)
                Session["LOB4"] = clsIncomeReport.SampleLOB;

            Session["INC_Account_DTL"] = FunPriHDR_DT();

            //Session["LOBName2"] = "hi";
            if (ddlDenomination.SelectedValue == "1")
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                lblAmounts.Text = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            if (objReportOrgMgtServices != null)
                objReportOrgMgtServices.Close();
        }
        return true ;

    }


    protected void FunProShow_AccountLevel( object sender, EventArgs e)
    {
        try
        {

            GenerateXMLLOB();

           ImageButton imgbtnQuery = (ImageButton)sender;

            //GridView gv = (GridView)imgbtnQuery.Parent.Parent.Parent.Parent;

            //Label lblTotal = (Label)gv.FooterRow.FindControl("lblTotal");
            //HiddenField hid_GT = (HiddenField)gv.FooterRow.FindControl("hid_GT");

            GridViewRow grvData = (GridViewRow)imgbtnQuery.Parent.Parent;
            HiddenField hdn_LOB_ID = grvData.FindControl("hdn_LOB_ID") as HiddenField;
            HiddenField hdn_Method_Value = grvData.FindControl("hdn_Method_Value") as HiddenField;
            HiddenField hdn_Location_ID1 = grvData.FindControl("hdn_Location_ID1") as HiddenField;
            

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@USER_ID", Convert.ToString(intUserId));
            dictParam.Add("@PROGRAM_ID", ProgramId.ToString ());
            dictParam.Add("@LOB_ID", hdn_LOB_ID.Value);
            dictParam.Add("@LOCATION_ID1", hdn_Location_ID1.Value);
            dictParam.Add("@LOCATION_ID2", hdn_Location_ID1.Value);
            dictParam.Add("@Method_Value", hdn_Method_Value.Value);

            dictParam.Add("@ReportType", ddlReportType .SelectedValue);

            dictParam.Add("@CutoffMonth", ViewState["CutoffMonthStart"].ToString());
            dictParam.Add("@CutoffMonth_Date", ViewState["CutoffMonthEnd"].ToString());

            dictParam.Add("@FinYearstart", ViewState["CutoffFYStartDate"].ToString());
            //dictParam.Add("@CurrentFinYear", Utility.StringToDate("1/11/2011").ToString());

            //dictParam.Add("@PreviousFinYear", Utility.StringToDate("1/11/2011").ToString());
            dictParam.Add("@Denomination", ddlDenomination .SelectedValue);

            DataTable dt = Utility.GetDefaultData("S3G_RPT_IncReport_Account", dictParam);
            pnlAccounts.Visible = btnPrintDetails.Visible = true;
            if (dt.Rows.Count > 0)
            {
                ViewState["DT_Grid"] = dt;
                Session["INC_Account_DTL"] = gvAccounts.DataSource = dt;
                gvAccounts.DataBind();

                btnPrintDetails.Visible = true;
               // btnPrint.Visible = btnEmail.Visible = true;

                Label lblTotMonth = (gvAccounts).FooterRow.FindControl("lblTotMonth") as Label;
                Label lblTotYear = (gvAccounts).FooterRow.FindControl("lblTotYear") as Label;
                Label lblTotUMFC = (gvAccounts).FooterRow.FindControl("lblTotUMFC") as Label;

                //if (ddlDenomination.SelectedValue == "1")
                //{
                //    lblTotMonth.Text = Convert.ToDecimal(dt.Compute("Sum(Month)", "")).ToString("0");
                //    lblTotYear.Text = Convert.ToDecimal(dt.Compute("Sum(Year)", "")).ToString("0");
                //    lblTotUMFC.Text = Convert.ToDecimal(dt.Compute("Sum(UMFC)", "")).ToString("0");
                //}
                //else
                //{
                    lblTotMonth.Text = Convert.ToDecimal(dt.Compute("Sum(Month)", "")).ToString(Funsetsuffix());
                    lblTotYear.Text = Convert.ToDecimal(dt.Compute("Sum(Year)", "")).ToString(Funsetsuffix());
                    lblTotUMFC.Text = Convert.ToDecimal(dt.Compute("Sum(UMFC)", "")).ToString(Funsetsuffix());
                //}
                //Label lblTotQuantity = (gvMaturityReturns).FooterRow.FindControl("lblTotQuantity") as Label;
                //lblTotQuantity.Text = dt.Compute("Sum(Available_Quantity)", "").ToString();

                //Label lblTotAmount = (gvMaturityReturns).FooterRow.FindControl("lblTotAmount") as Label;
                //lblTotAmount.Text = dt.Compute("Sum(Amount1)", "").ToString();

            }
            else
            {
                
                             
                gvAccounts.DataSource = null;
                gvAccounts.EmptyDataText = "No Account Level Income Details";
                gvAccounts.DataBind();
               // btnPrint.Visible = btnEmail.Visible = false;
            }           
        }
        catch(Exception ex)
        {
            CVIncomeReport.ErrorMessage = "Unable to Show Account Level Details";
            CVIncomeReport.IsValid = false;
        }

    }

    private DataTable FunPriHDR_DT()
    {
        DataTable dt = new DataTable();
        try
        {
            
            DataRow drEmptyRow;
            //dt.Columns.Add("Location_ID");
            //dt.Columns.Add("Location");
            //dt.Columns.Add("Report_Type");
            //dt.Columns.Add("Investment_Type");
            dt.Columns.Add("Company_Id");
            dt.Columns.Add("Company_Name");
            dt.Columns.Add("Company_Address");
            dt.Columns.Add("col1");
            dt.Columns.Add("col2");
            dt.Columns.Add("Currency_IN");
            dt.Columns.Add("Report_DT");
            dt.Columns.Add("User_Name");
            dt.Columns.Add("GPS_Suffix");

            drEmptyRow = dt.NewRow();
            //drEmptyRow["Location_ID"] = ddlLocation1.SelectedValue;
            //drEmptyRow["Location"] = ddlLocation.SelectedItem.Text;
            //drEmptyRow["Report_Type"] = ddlReportType;
            //drEmptyRow["Investment_Type"] = ddlINVType.SelectedItem.Text;
            drEmptyRow["Company_Id"] = ObjUserInfo.ProCompanyIdRW.ToString();
            drEmptyRow["Company_Name"] = ObjUserInfo.ProCompanyNameRW.ToString();
            drEmptyRow["Company_Address"] = "Income Report for the Month Ending " + DateTime.Now.ToString("MMM-yyyy");
            drEmptyRow["col1"] = ddlReportType.SelectedItem.Text;
            drEmptyRow["col2"] = "";
            drEmptyRow["Report_DT"] = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            drEmptyRow["User_Name"] = ObjUserInfo.ProUserNameRW;
            drEmptyRow["Currency_IN"] = lblAmounts.Text;
            drEmptyRow["GPS_Suffix"] = ObjS3GSession.ProGpsSuffixRW.ToString();
            dt.Rows.Add(drEmptyRow);
                  
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;

    }



    //private DataTable FunPriHDR_DT()
    //{
    //    DataTable dt = new DataTable();
    //    try
    //    {
    //        DataRow drEmptyRow;
    //        dt.Columns.Add("CompanyId");
    //        dt.Columns.Add("LOB_ID");
    //        dt.Columns.Add("LOBName");
    //        dt.Columns.Add("Location_ID");
    //        dt.Columns.Add("LocationName");
    //        dt.Columns.Add("Month");
    //        dt.Columns.Add("Year");
    //        dt.Columns.Add("UMFC");
    //        dt.Columns.Add("Method");
    //        dt.Columns.Add("PRIMEACCOUNTNO");
    //        dt.Columns.Add("SUBACCOUNTNO");
    //        dt.Columns.Add("GPSSuffix");
    //        dt.Columns.Add("Denomination");

    //        dt.Columns.Add("DataRow1");
    //        dt.Columns.Add("DataRow2");
    //       // dt.Columns.Add("Denomination");

    //        //drEmptyRow = dt.NewRow();
    //        //drEmptyRow["CompanyId"] =0;
    //        //drEmptyRow["LOB_ID"] = 0;
    //        //drEmptyRow["LOBName"] = "";
    //        //drEmptyRow["Location_ID"] = "";
    //        //drEmptyRow["LocationName"] = "";
    //        //drEmptyRow["Month"] = 0;
    //        //drEmptyRow["Year"] = 0;
    //        //drEmptyRow["UMFC"] =0;
    //        //drEmptyRow["Method"] = "";
    //        //drEmptyRow["PRIMEACCOUNTNO"] ="";
    //        //drEmptyRow["SUBACCOUNTNO"] = "";
    //        //drEmptyRow["GPSSuffix"] = "";
    //        //drEmptyRow["Denomination"] = "";;
    //        //drEmptyRow["DataRow1"] = "";
    //        //drEmptyRow["DataRow2"] = "";;
           
    //        //dt.Rows.Add(drEmptyRow);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return dt;

    //}


    private void FunPriSetGPSSuffix(decimal value, Label lbl)
    {
        if (value != 0)
            lbl.Text = value.ToString(Funsetsuffix());
        else
            lbl.Text = 0.ToString(Funsetsuffix());
    }

    private void FunPriNoGPSSuffix(decimal value, Label lbl)
    {
        if (value != 0)
            lbl.Text = value.ToString();
        else
            lbl.Text = 0.ToString();
    }


    protected void gvLoadLOB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
            //CheckBox chkSelectAll = gvmail.HeaderRow.FindControl("chkSelectAll") as CheckBox;
            if (chkSelect != null)
            {
                chkSelect.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + gvLoadLOB.ClientID + "','chkSelectAll','chkSelect');");
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("chkSelectAll");
            chkSelectAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvLoadLOB.ClientID + "',this,'chkSelect');");
        }

    }

    protected void LOB_ChkChanged(object sender, EventArgs e)
    {
        try
        {
            FunClearGrid();
            FunPriGetBranch();
            FunPriLoadLocation2();
            ddlLocation2.Enabled = false;
            ddlReportType.SelectedIndex = 0;
            txtCutoff.Text = "";
            ddlLocation1.AddItemToolTip();

            //FunPriGetBranch();
            //FunPriLoadLocation2();
            //gvIncome.DataSource = null;
            //gvIncome.DataBind();
            //pnlIncome.Visible = false;

        }
        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = "Due to Data Problem, Unable to Load Report";
            CVIncomeReport.IsValid = false;
        }
    }

    protected void txtCutoff_OnTextChanged(object sender, EventArgs e)
    {
        ddlDenomination.ClearSelection();
        gvIncome.DataSource = null;
        lblAmounts.Visible = pnlIncome.Visible = pnlAccounts .Visible = false;
        btnPrint.Visible =  btnPrintDetails .Visible = false;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        pnlAccounts.Visible = btnPrintDetails.Visible = false;

        if (!FunPriValidateLOB())
        {
            Utility.FunShowAlertMsg(this.Page, "Select Atleast one Line of Business");
            return;
        }
        FunPriLoadIncomeGrid();
       // FunPriLoadDT_Income();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
       // ddlLOB.ClearSelection();
        FunPriLoadLob();
        FunPriGetBranch();
        //ddlLocation2.Enabled = false;
        FunPriLoadLocation();
        ddlReportType.ClearSelection();
        txtCutoff.Text = "";
        ddlDenomination.ClearSelection();
        gvIncome.DataSource = null;
        lblAmounts.Visible = pnlIncome.Visible = pnlAccounts.Visible = false;
        btnPrint.Visible = btnPrintDetails.Visible = false;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {

            Session["RType"] = ddlReportType.SelectedItem.Text;
            Session["Option"] = "1";
            //Session["LOB"] = ddlLOB.SelectedItem.Text;
            //Session["IRRType"] = ddlIRRType.SelectedItem.Text;
            Session["CompName"] = ObjUserInfo.ProCompanyNameRW.ToString();
            Session["HeaderDateTime"] = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            Session["Rhead"] = "Income Report for the Month Ending " + DateTime.Now.ToString("MMM-yyyy");
            Session["AmountIn"] = lblAmounts.Text; //"[All Amounts are in " +ObjS3GSession.ProCurrencyNameRW.ToString ()+ "]";
            //// Session["GPSdecimal"] = ObjS3GSession.ProGpsSuffixRW.ToString ();
            string strScipt = "window.open('../Reports/S3GRptIncomeReportReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Income Report", strScipt, true);
        }
        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = "Due to Data Problem, Unable to Print the Report";
            CVIncomeReport.IsValid = false;
        }

    }


    protected void btnPrintDetails_Click(object sender, EventArgs e)
    {
        try
        {

            DataSet ds = new DataSet();
            ds.Tables.Add((DataTable)ViewState["DT_Grid"]);
            Session["Option"] = "2";
            ds.Tables[0].TableName = "DT_Grid1";
            ds.Tables.Add(FunPriHDR_DT());
            ds.Tables[1].TableName = "DT_Header1";
            Session["Report_Data"] = ds;
            string strScipt = "window.open('../Reports/S3GRptIncomeReportReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Income Report", strScipt, true);
        

            //Session["RType"] = ddlReportType.SelectedItem.Text;
            //Session["Option"] = "2";
            ////Session["IRRType"] = ddlIRRType.SelectedItem.Text;
            //Session["CompName"] = ObjUserInfo.ProCompanyNameRW.ToString();
            //Session["HeaderDateTime"] = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            //Session["Rhead"] = "Income Report for the Month Ending " + DateTime.Now.ToString("MMM-yyyy");
            //Session["AmountIn"] = lblAmounts.Text; //"[All Amounts are in " +ObjS3GSession.ProCurrencyNameRW.ToString ()+ "]";
            ////// Session["GPSdecimal"] = ObjS3GSession.ProGpsSuffixRW.ToString ();
            
        }
        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = "Due to Data Problem, Unable to Print the Report";
            CVIncomeReport.IsValid = false;
        }

    }

    protected void btnChart_Click(object sender, EventArgs e)
    {
        try
        {
            ////FunToViewChartORReport();
            ////Session["CRoption"] = "C";
            //Session["RType"] = ddlReportType.SelectedItem.Text;
            //Session["LOB"] = ddlLOB.SelectedItem.Text;
            //Session["IRRType"] = ddlIRRType.SelectedItem.Text;
            //Session["CompName"] = ObjUserInfo.ProCompanyNameRW.ToString();
            //Session["HeaderDateTime"] = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            //// Session["Rhead"] = "Weighted Average IRR as on date " + DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW);
            //Session["AmountIn"] = lblAmounts.Text; //"[All Amounts are in " +ObjS3GSession.ProCurrencyNameRW.ToString ()+ "]";
            //// Session["GPSdecimal"] = ObjS3GSession.ProGpsSuffixRW.ToString ();
            //string strScipt = "window.open('../Reports/S3GPerformanceofAssetPieChart.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Weighted Average IRR", strScipt, true);

        }
        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = "Due to Data Problem, Unable to View the Chart";
            CVIncomeReport.IsValid = false;
        }

    }


    /// <summary>
    /// To set the Suffix to total
    /// </summary>
    /// <returns></returns>
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
    /// This Method is called after Clicking the Clear Button
    /// </summary>
    private void FunPriClear()
    {
        try
        {
          //  ddlLOB.ClearSelection();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtCutoff.Text = "";
            ddlDenomination.ClearSelection();
            FunClearGrid();
        }
        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = ex.Message;
            CVIncomeReport.IsValid = false;
        }
    }


   
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunClearGrid();
            FunPriGetBranch();
            FunPriLoadLocation2();
            ddlLocation2.Enabled = false;
            ddlReportType.SelectedIndex = 0;
            txtCutoff.Text = "";
            //FunPriLoadLocation(0);
        }
        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = ex.Message;
            CVIncomeReport.IsValid = false;
        }
    }

    protected void ddlDenomination_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunClearGrid();
        }
        catch (Exception ex)
        {
            CVIncomeReport.ErrorMessage = ex.Message;
            CVIncomeReport.IsValid = false;
        }
    }

    protected void FunClearGrid()
    {
        try
        {

            lblAmounts.Visible = btnPrint.Visible = btnPrintDetails.Visible = pnlIncome.Visible = pnlAccounts.Visible = false;
            gvIncome.DataSource = null;
            gvIncome.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion




}
