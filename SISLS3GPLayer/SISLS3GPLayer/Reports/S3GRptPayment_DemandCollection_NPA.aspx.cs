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
using S3GBusEntity.Reports;
using System.Globalization;


public partial class Reports_S3GRptPayment_DemandCollection_NPA : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    //string PANum;
    //string SANum;
    string RegionId;
    int intProgramId = 178;
    bool Is_Active;
    public string strDateFormat;
    decimal TotalLastMnthGrowth;
    decimal TotalCumulative;
    decimal TotalLastYearGrowth;
    decimal TotalArrearDemand;
    decimal TotalArrearCol;
    decimal TotalArrearPercent;
    decimal TotalCurrentDemand;
    decimal TotalCurrentCol;
    decimal TotalCurrentPer;
    decimal TotalTotDemand;
    decimal TotalTotCol;
    decimal TotalTotPer;
    decimal TotalBadDebts;
    decimal TotalStock;
    decimal TotalNpa;
    decimal TotalNpaPercent;
    decimal TotalClosingArrear;
    decimal TotalArrearPercentage;
    Dictionary<string, string> Procparam;
    Dictionary<int, string> dictDemandmonth = new Dictionary<int, string>();

    string strPageName = "Payment DemandCollection NPA Details";
    DataTable dtTable = new DataTable();
    ReportAccountsMgtServicesClient objSerClient;
    ReportOrgColMgtServicesClient ObjOrgColClient;
    #endregion

    #region Page Load

    /// <summary>
    /// This event is handled for load the page
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Due to Data Problem, Unable to Load Payment DemandCollection NPA Details Page.";
            CVPayment.IsValid = false;
        }

    }
    #endregion

    # region Page Methods

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
            //CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            //CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
            txtStartMonth.Attributes.Add("readonly", "readonly");
            txtEndMonth.Attributes.Add("readonly", "readonly");
            #endregion

            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            Session["DateTime"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            if (!IsPostBack)
            {

                FunPriLoadLob();
                FunPriLoadBranch();
                FunPriLoadLocation();
                FunPubLoadProduct();
                FunPubLoadDenomination();
                //FunPriFillArrayDemandMonth();
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);

            ddlProduct.AddItemToolTip();
            if (ddlProduct.SelectedIndex > 0)
                ddlProduct.ToolTip = ddlProduct.SelectedItem.Text;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Payment DemandCollection NPA Details page");
        }
    }

    /// <summary>
    /// To Load LOB
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id"></param>
    private void FunPriLoadLob()
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubLOB(intCompanyId, intUserId, intProgramId);
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

    /// <summary>
    /// To Load Location 1
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="Is_active"></param>
    private void FunPriLoadBranch()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlRegion.DataSource = Branch;
            ddlRegion.DataTextField = "Description";
            ddlRegion.DataValueField = "ID";
            ddlRegion.DataBind();
            ddlRegion.Items[0].Text = "--ALL--";
            //if (ddlBranch.Items.Count == 2)
            //    ddlBranch.SelectedIndex = 1;
            //else
            //    ddlBranch.SelectedIndex = 0;

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Branch");
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Load Location 2
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id"></param>
    /// <param name="RegionId"></param>
    /// <param name="Is_active"></param>
    private void FunPriLoadLocation()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--ALL--";

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Branch");
        }
        finally
        {
            objSerClient.Close();
        }
    }

    private void FunPriLoadLocation2()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int LobId = 0;
            if (ddlLOB.SelectedIndex > 0)
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            int Location1 = 0;
            if (ddlRegion.SelectedIndex > 0)
                Location1 = Convert.ToInt32(ddlRegion.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetLocation2(intProgramId, intUserId, intCompanyId, LobId, Location1);
            List<ClsPubDropDownList> Location = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlBranch.DataSource = Location;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            if (ddlBranch.Items.Count == 2)
            {
                if (ddlRegion.SelectedIndex != 0)
                {
                    ddlBranch.SelectedIndex = 1;
                    //Utility.ClearDropDownList(ddlBranch);
                }
                else
                    ddlBranch.SelectedIndex = 0;
            }
            else
            {
                ddlBranch.Items[0].Text = "--ALL--";
                ddlBranch.SelectedIndex = 0;
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
    /// To Load Product
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id"></param>
    public void FunPubLoadProduct()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int LobId = 0;
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubGetProductDetails(intCompanyId, LobId);
            List<ClsPubDropDownList> Product = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlProduct.DataSource = Product;
            ddlProduct.DataTextField = "Description";
            ddlProduct.DataValueField = "ID";
            ddlProduct.DataBind();

            ddlProduct.AddItemToolTip();
            //if (ddlProduct.SelectedIndex > 0)
            //    ddlProduct.ToolTip = ddlProduct.SelectedItem.Text;
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
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.GetDenominations();
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
            objSerClient.Close();
        }
    }

    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    /// <summary>
    /// To Clear the Values
    /// </summary>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    private void FunPriClearPayment()
    {
        try
        {
            ddlLOB.ClearSelection();
            ddlRegion.ClearSelection();
            FunPriLoadLocation();
            ddlBranch.Enabled = false;
            FunPubLoadProduct();
            ddlProduct.ToolTip= "Product";
            txtStartMonth.Text = "";
            txtEndMonth.Text = "";
            lblError.Text = "";
            ddlDenomination.ClearSelection();
            FunPriValidateGrid();
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
        Session["PaymentDCNPA"] = null;
    }

    /// <summary>
    /// To Validate Grid
    /// </summary>
    private void FunPriValidateGrid()
    {
        ddlDenomination.ClearSelection();
        pnlDetails.Visible = false;
        grvDetails.DataSource = null;
        grvDetails.DataBind();
        btnPrint.Visible = false;
        lblAmounts.Visible = false;
        lblErrorMsg.Visible = false;
    }

    /// <summary>
    /// To Validate Future Date
    /// </summary>
    /// <param name="text"></param>
    private void FunPriValidateFutureDate(TextBox text)
    {
        try
        {
            #region To find Current Year and Month
            //string Today = Convert.ToString(DateTime.Now);
            string YearMonth = text.Text;
            int Currentmonth = DateTime.Now.Month;
            int Currentyear = DateTime.Now.Year;
            #endregion

            int Month = int.Parse(YearMonth.Substring(0, 2));
            int year = int.Parse(YearMonth.Substring(3, 4));
            //if (year > Currentyear || Month > Currentmonth)
            if (year > Currentyear)
            {
                text.Text = "";
                Utility.FunShowAlertMsg(this, "Year cannot be Greater than System Year.");
                return;
            }
            else if (year == Currentyear)
            {
                if (Month > Currentmonth)
                {
                    text.Text = "";
                    Utility.FunShowAlertMsg(this, "Month cannot be Greater than System month.");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To Validate End Date
    /// </summary>
    private void FunPriValidateEndDate()
    {
        string StartYearMonth = txtStartMonth.Text;
        string EndYearMonth = txtEndMonth.Text;
        int StartMonth = int.Parse(StartYearMonth.Substring(0, 2));
        int Startyear = int.Parse(StartYearMonth.Substring(3, 4));
        int EndMonth = int.Parse(EndYearMonth.Substring(0, 2));
        int Endyear = int.Parse(EndYearMonth.Substring(3, 4));
        //if (Startyear > Endyear || StartMonth > EndMonth)
        if (Startyear > Endyear)
        {
            Utility.FunShowAlertMsg(this, "End Year Should be Greater than or equal to Start Year.");
            txtEndMonth.Text = "";
            pnlDetails.Visible = false;
            grvDetails.DataSource = null;
            grvDetails.DataBind();
            btnPrint.Visible = false;
            lblAmounts.Visible = false;
            return;
        }
        else if (Startyear == Endyear)
        {
            if (StartMonth > EndMonth)
            {
                Utility.FunShowAlertMsg(this, "End month Should be Greater than or equal to Start Month.");
                txtEndMonth.Text = "";
                pnlDetails.Visible = false;
                grvDetails.DataSource = null;
                grvDetails.DataBind();
                btnPrint.Visible = false;
                lblAmounts.Visible = false;
                return;
            }

        }

    }

    /// <summary>
    /// To Bind the Payment Demand/Collection and NPA Details
    /// </summary>
    private void FunPriLoadPaymentDCNPADetails()
    {
        try
        {
            lblAmounts.Visible = true;
            btnPrint.Visible = true;
            btnPrint.Enabled = true;
            pnlDetails.Visible = true;
            lblError.Text = "";
            divDetails.Style.Add("display", "block");
            if (ddlDenomination.SelectedValue == "1")
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                lblAmounts.Text = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            //To month start Date & End Date
            if (txtEndMonth.Text == string.Empty)
            {
                int Currentmonth = DateTime.Now.Month;
                int Currentyear = DateTime.Now.Year;
                if (Currentmonth < 10)
                {
                    txtEndMonth.Text = "0" + Convert.ToString(Currentmonth) + "/" + Convert.ToString(Currentyear);
                }
                else
                {
                    txtEndMonth.Text = Convert.ToString(Currentmonth) + "/" + Convert.ToString(Currentyear);
                }
            }

            string EndYearMonth = txtEndMonth.Text;
            string StarYearMonth = txtStartMonth.Text;
            int StartMonth = int.Parse(StarYearMonth.Substring(0, 2));
            int Startyear = int.Parse(StarYearMonth.Substring(3, 4));

            int EndMonth = int.Parse(EndYearMonth.Substring(0, 2));
            int Endyear = int.Parse(EndYearMonth.Substring(3, 4));
            string ToMonthEndDate = System.DateTime.DaysInMonth(Endyear, EndMonth).ToString(); //To Find End Date
            DateTime ToMonth_startDate = Convert.ToDateTime(EndMonth + "/" + "1" + "/" + Endyear); //Start Date
            DateTime ToMonth_EndDate = Convert.ToDateTime(EndMonth + "/" + ToMonthEndDate + "/" + Endyear);//End Date

            //To month Previous Month Start Date & End Date
            int PreviousMonth;
            int PreEndYear;
            int PreStartYear;
            if (EndMonth == 1)
            {
                PreviousMonth = 12;
                PreEndYear = Endyear - 1;
            }
            else
            {
                PreviousMonth = EndMonth - 1;
                PreEndYear = Endyear;
            }
            int StartPreMonth;
            if (StartMonth == 1)
            {
                StartPreMonth = 12;
                PreStartYear = Startyear - 1;
            }
            else
            {
                StartPreMonth = StartMonth - 1;
                PreStartYear = Startyear;
            }
            string PreToMonthEndDate = System.DateTime.DaysInMonth(PreEndYear, PreviousMonth).ToString();
            DateTime FromMonth_Pre_startDate = Convert.ToDateTime(StartPreMonth + "/" + "1" + "/" + PreStartYear);
            DateTime ToMonth_Pre_EndDate = Convert.ToDateTime(PreviousMonth + "/" + PreToMonthEndDate + "/" + PreEndYear);

            //From Month Start Date
            //string StarYearMonth = txtStartMonth.Text;
            //int StartMonth = int.Parse(StarYearMonth.Substring(0, 2));
            //int Startyear = int.Parse(StarYearMonth.Substring(3, 4));
            DateTime FromMonth_StartDate = Convert.ToDateTime(StartMonth + "/" + "1" + "/" + Startyear);

            //To Find Previous Year Start Date and End Date
            int FromPreviousYear = Startyear - 1;
            DateTime FromMonth_PreYear_StartDate = Convert.ToDateTime(StartMonth + "/" + "1" + "/" + FromPreviousYear);
            int ToPreviousYear = Endyear - 1;
            string PreYearToMonthEndDate = System.DateTime.DaysInMonth(ToPreviousYear, EndMonth).ToString();
            DateTime ToMonth_PreYear_EndDate = Convert.ToDateTime(EndMonth + "/" + PreYearToMonthEndDate + "/" + ToPreviousYear);
           
            //To find Financial Year Start Date
            string SYearMonth = txtStartMonth.Text;
            int S_Month = int.Parse(SYearMonth.Substring(0, 2));
            int Start_year = int.Parse(SYearMonth.Substring(3, 4));
            int previous_year = Start_year - 1;
            int CurMonth = DateTime.Now.Month;
            int FinYearStarMonth=0;
            DateTime FinYear_StartMonth_StartDate;
            FinYearStarMonth =Convert.ToInt32( ClsPubConfigReader.FunPubReadConfig("StartMonth"));
            if (S_Month < FinYearStarMonth)
            {
                //string FinYearStarYearMonthStartDate = ddlFromYearMonthBase.Items[1].Text;
                //int FinYearStartyear = int.Parse(FinYearStarYearMonthStartDate.Substring(0, 4));
                //int FinYearStartMonth = int.Parse(FinYearStarYearMonthStartDate.Substring(4, 2));
                FinYear_StartMonth_StartDate = Convert.ToDateTime(FinYearStarMonth + "/" + "1" + "/" + previous_year);
            }
            else
            {
                 FinYear_StartMonth_StartDate = Convert.ToDateTime(FinYearStarMonth + "/" + "1" + "/" + Start_year);
            }
            

            //Previous From Month
            int PreviousFromMonth;
            int PreFromYear;
            if (StartMonth == 1)
            {
                PreviousFromMonth = 12;
                PreFromYear = Startyear - 1;
            }
            else
            {
                PreviousFromMonth = StartMonth - 1;
                PreFromYear = Startyear;
            }
            string Pre_FromMonth;
            if (PreviousFromMonth < 10)
            {
                Pre_FromMonth = Convert.ToString(PreFromYear) + "0" + Convert.ToString(PreviousFromMonth);
            }
            else
            {
                Pre_FromMonth = Convert.ToString(PreFromYear) + Convert.ToString(PreviousFromMonth);
            }
            string FromMonth;
            if (StartMonth < 10)
            {
                FromMonth = Convert.ToString(Startyear) + "0" + Convert.ToString(StartMonth);
            }
            else
            {
                FromMonth = Convert.ToString(Startyear) + Convert.ToString(StartMonth);
            }
            string ToMonth;
            if (EndMonth < 10)
            {
                ToMonth = Convert.ToString(Endyear) + "0" + Convert.ToString(EndMonth);
            }
            else
            {
                ToMonth = Convert.ToString(Endyear) + Convert.ToString(EndMonth);
            }

            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            ClsPubPaymentDCNPAParameters PaymentParameter = new ClsPubPaymentDCNPAParameters();
            PaymentParameter.CompanyId = intCompanyId;
            PaymentParameter.UserId = intUserId;
            PaymentParameter.ProgramId = intProgramId;
            PaymentParameter.LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            if (ddlRegion.SelectedIndex > 0)
            {
                PaymentParameter.LocationId1 = Convert.ToInt32(ddlRegion.SelectedValue);
            }
            else
            {
                PaymentParameter.LocationId1 = 0;
            }
            if (ddlBranch.SelectedIndex > 0)
            {
                PaymentParameter.LocationId2 = Convert.ToInt32(ddlBranch.SelectedValue);
            }
            else
            {
                PaymentParameter.LocationId2 = 0;

            }

            if (ddlProduct.SelectedIndex > 0)
            {
                PaymentParameter.ProductId = ddlProduct.SelectedValue;
            }
            else
            {
                PaymentParameter.ProductId = "0";

            }
            PaymentParameter.ToMonthStartDate = Convert.ToString(FromMonth_StartDate);
            PaymentParameter.ToMonthEndDate = Convert.ToString(ToMonth_EndDate);
            PaymentParameter.PreToMonthStartDate = Convert.ToString(FromMonth_Pre_startDate);
            PaymentParameter.PreToMonthEndDate = Convert.ToString(ToMonth_Pre_EndDate);
            PaymentParameter.FromMonthStartDate = Convert.ToString(FromMonth_StartDate);
            PaymentParameter.PreYearFromMonthStartDate = Convert.ToString(FromMonth_PreYear_StartDate);
            PaymentParameter.PreYearToMonthEndDate = Convert.ToString(ToMonth_PreYear_EndDate);
            PaymentParameter.FinYearStartMonthStartDate = Convert.ToString(FinYear_StartMonth_StartDate);
            PaymentParameter.PreFromMonth = Pre_FromMonth;
            PaymentParameter.FromMonth = FromMonth;
            PaymentParameter.ToMonth = ToMonth;
            PaymentParameter.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] bytePaymentDetail = ClsPubSerialize.Serialize(PaymentParameter, SerializationMode.Binary);

            byte[] byteLobs = ObjOrgColClient.FunPubGetPaymentDemandCollectionNPA(bytePaymentDetail);
            //List<ClsPubPaymentDCNPADetails> PaymentDCNPADetails = (List<ClsPubPaymentDCNPADetails>)DeSeriliaze(byteLobs);

            ClsPubCumulative Cumulative = (ClsPubCumulative)DeSeriliaze(byteLobs);
            TotalLastMnthGrowth = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.GrowthPercentageLastMonth);
            TotalCumulative = Cumulative.Cumm;
            TotalLastYearGrowth = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.GrowthPercentageLastYear);
            TotalArrearDemand = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.ArrearDemand);
            TotalArrearCol = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.ArrearCollection);
            //TotalArrearPercent = PaymentDCNPADetails.Sum(ClsPubPaymentDCNPADetails => ClsPubPaymentDCNPADetails.ArrearCollectionPercentage);
            TotalCurrentDemand = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.CurrentDemand);
            TotalCurrentCol = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.CurrentCollection);
            //TotalCurrentPer = PaymentDCNPADetails.Sum(ClsPubPaymentDCNPADetails => ClsPubPaymentDCNPADetails.CurrentPercentage);
            TotalTotDemand = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.TotalDemand);
            TotalTotCol = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.TotalCollection);
            //TotalTotPer = PaymentDCNPADetails.Sum(ClsPubPaymentDCNPADetails => ClsPubPaymentDCNPADetails.TotalPercentage);
            TotalBadDebts = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.BadDebts);
            TotalStock = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.Stock);
            TotalNpa = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.Npa);
            //TotalNpaPercent = PaymentDCNPADetails.Sum(ClsPubPaymentDCNPADetails => ClsPubPaymentDCNPADetails.NpaPercentage);
            TotalClosingArrear = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.ClosingArrear);
            TotalArrearPercentage = Cumulative.PaymentDCNPADetails.Sum(ClsPubCumulative => ClsPubCumulative.ArrearPercentage);

            if (Cumulative.CA_Exists > 0)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Note : In order to set off advance collections in Closing Arrears, current demand is displayed even when though there is no actual due. Due to this column wise Closing Arrears Total might not tally.";
                Session["Note"] = lblErrorMsg.Text;
                
            }
            else
            {
                lblErrorMsg.Visible = false;
                Session["Note"] = null;
                
                
            }
            //Session["Note"] = "saranya";
            Session["PaymentDCNPA"] = Cumulative.PaymentDCNPADetails;
            grvDetails.DataSource =Cumulative.PaymentDCNPADetails;
            grvDetails.DataBind();
            if (ddlLOB.SelectedIndex > 0)
            {
                grvDetails.Columns[2].Visible = false;
            }
            else
            {
                grvDetails.Columns[2].Visible = true;
            }
            if (ddlRegion.SelectedIndex > 0)
            {
                grvDetails.Columns[4].Visible = false;
            }
            else
            {
                grvDetails.Columns[4].Visible = true;
            }
            //if (ddlBranch.SelectedIndex != 0)
            //{
            //    grvDetails.Columns[6].Visible = false;

            //}
            //else
            //{
            //    grvDetails.Columns[6].Visible = true;
            //}
            if (ddlProduct.SelectedIndex > 0)
            {
                grvDetails.Columns[6].Visible = false;

            }
            else
            {
                grvDetails.Columns[6].Visible = true;
            }
            if (grvDetails.Rows.Count == 0)
            {
                btnPrint.Enabled = false;
                lblErrorMsg.Visible = false;
                Session["PaymentDCNPA"] = null;
                lblError.Text = "No Records Found";
                grvDetails.DataBind();
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
            ObjOrgColClient.Close();
        }

    }

    private void FunPriDisplayTotal()
    {
        //Modified "0" as Total on 11/14/2011 as per Vasanth 
        ((Label)grvDetails.FooterRow.FindControl("lblTotGrowthLastMonth")).Text = (0).ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotCumulative")).Text = TotalCumulative.ToString(Funsetsuffix());
        //((Label)grvDetails.FooterRow.FindControl("lblTotCumulative")).Text = ((Label)grvDetails.Rows[grvDetails.Rows.Count - 1].FindControl("lblCumulative")).Text;

        //Modified "100" as Total on 11/14/2011 as per Vasanth 
        ((Label)grvDetails.FooterRow.FindControl("lblTotGrowthLastYear")).Text = (100).ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotOpeningDemand")).Text = TotalArrearDemand.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotOpeningCollection")).Text = TotalArrearCol.ToString(Funsetsuffix());
        //((Label)grvDetails.FooterRow.FindControl("lblTotOpeningpercentage")).Text = TotalArrearPercent.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotMonthlyDemand")).Text = TotalCurrentDemand.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotMonthlyCollection")).Text = TotalCurrentCol.ToString(Funsetsuffix());
        //((Label)grvDetails.FooterRow.FindControl("lblTotMonthlypercentage")).Text = TotalCurrentPer.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotClosingDemand")).Text = TotalTotDemand.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotClosingCollection")).Text = TotalTotCol.ToString(Funsetsuffix());
        //((Label)grvDetails.FooterRow.FindControl("lblTotClosingpercentage")).Text = TotalTotPer.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotBadDebts")).Text = TotalBadDebts.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotStock")).Text = TotalStock.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotNpa")).Text = TotalNpa.ToString(Funsetsuffix());
        //((Label)grvDetails.FooterRow.FindControl("lblTotNpaPercentage")).Text = TotalNpaPercent.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotClosingArrear")).Text = TotalClosingArrear.ToString(Funsetsuffix());
        ((Label)grvDetails.FooterRow.FindControl("lblTotArrearPercentage")).Text = TotalArrearPercentage.ToString(Funsetsuffix());

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
    #endregion

    #region Page Events
    #region DropdownList Events

    /// <summary>
    /// To Load the Branch
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            FunPriValidateGrid();
            FunPubLoadProduct();
            ddlProduct.ToolTip = "Product";
            ddlBranch.Enabled = true;
            if (ddlRegion.SelectedIndex > 0)
            {
                FunPriLoadLocation2();
            }
            else
            {
                ddlBranch.Enabled = false;
                FunPriLoadLocation();
            }
            txtStartMonth.Text = "";
            txtEndMonth.Text = "";
            ddlDenomination.ClearSelection();

        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVPayment.IsValid = false;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Validate the Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
            FunPriValidateGrid();
            //txtStartMonth.Text = "";
            //txtEndMonth.Text = "";

        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVPayment.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate The Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadBranch();
            FunPriLoadLocation();
            ddlBranch.Enabled = false;
            FunPriValidateGrid();
            FunPubLoadProduct();
            ddlProduct.ToolTip = "Product";
            txtStartMonth.Text = "";
            txtEndMonth.Text = "";
            // ddlDenomination.ClearSelection();

        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVPayment.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate The Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlProduct.ToolTip = ddlProduct.SelectedItem.Text;
            FunPriValidateGrid();

        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVPayment.IsValid = false;
        }
    }

    protected void txtStartMonth_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateFutureDate(txtStartMonth);
        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Unable to Validate Start Month/Year .";
            CVPayment.IsValid = false;
        }
    }

    protected void txtEndMonth_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateFutureDate(txtEndMonth);
            FunPriValidateEndDate();
        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Unable to Validate End Month/Year .";
            CVPayment.IsValid = false;
        }
    }

    #endregion

    #region Button (Customer / Ok / Clear / Print)

    /// <summary>
    /// To validate the From and To Date
    /// To Bind the Sanction Details Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //ClearSession();
            FunPriFillArrayDemandMonth();

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@XMLParamtDemandMonthLists", FunPriGetDemandMonths((Dictionary<int, string>)ViewState["Months"]));
            DataSet ds = new DataSet();
            ds = Utility.GetDataset("S3G_RPT_GetDemandMonthDetails", Procparam);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string StrErrorMsg = "Demand not run for the selected month ";
                if (ds.Tables[1].Rows.Count > 0)
                {
                    StrErrorMsg += "(";
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        StrErrorMsg += dr["Demand_Month"].ToString() + ",";
                    }
                    StrErrorMsg = StrErrorMsg.Substring(0, StrErrorMsg.Length - 1);
                    StrErrorMsg += ")";
                    Utility.FunShowAlertMsg(this, StrErrorMsg);
                    //ddlToYearMonthBase.ClearSelection();
                    txtStartMonth.Text = "";
                    txtEndMonth.Text = "";
                    FunPriValidateGrid();
                    return;
                }
            }
            FunPriLoadPaymentDCNPADetails();
            ClsPubHeaderDetails ObjHeader = new ClsPubHeaderDetails();
            ObjHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            if (Convert.ToInt32(ddlRegion.SelectedValue) > 0)
            {
                ObjHeader.Region = ddlRegion.SelectedItem.Text;
            }
            else
            {
                ObjHeader.Region = "All";
            }
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                ObjHeader.Branch = ddlBranch.SelectedItem.Text;
            }
            else
            {
                ObjHeader.Branch = "All";
            }
            if (Convert.ToInt32(ddlProduct.SelectedValue) > 0)
            {
                ObjHeader.Product = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();
                Session["Product"] = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();
            }
            else
            {
                ObjHeader.Product = "All";
                Session["Product"] = "All";
            }

            ObjHeader.FromYearMonth = Convert.ToDateTime(txtStartMonth.Text).ToString("MMMM yyyy");
            ObjHeader.ToYearMonth = Convert.ToDateTime(txtEndMonth.Text).ToString("MMMM yyyy");
            Session["Header"] = ObjHeader;
            if (ddlDenomination.SelectedValue == "1")
            {
                Session["Denomination"] = "[All Amounts are In" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Denomination"] = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            string s1 = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();

            if (s1 == "HP")
                Session["LOB"] = "Gross Stock on Hire";
            else if (s1 == "LN")
                Session["LOB"] = "Gross Investment on Loan";
            else if (s1 == "FL")
                Session["LOB"] = "Gross Investment on Lease";
            else
                Session["LOB"] = "Gross Stock";
        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Due to Data Problem, Unable to Load Payment DemandCollection NPA Details Grid.";
            CVPayment.IsValid = false;
        }


    }

    /// <summary>
    /// To Clear The Values
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearPayment();

        }
        catch (Exception ex)
        {
            CVPayment.ErrorMessage = "Unable to Clear.";
            CVPayment.IsValid = false;
        }

    }

    /// <summary>
    /// export to crystal format
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        string strScipt = "window.open('../Reports/S3GRptPaymentDemandCollectionNPAReport.aspx', 'Payment','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Payment", strScipt, true);

    }
    protected void grvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //FunPriDisplayTotal();
            Label lblTotOpeningDemand = e.Row.FindControl("lblTotOpeningDemand") as Label;
            Label lblTotOpeningCollection = e.Row.FindControl("lblTotOpeningCollection") as Label;
            Label lblTotOpeningpercentage = e.Row.FindControl("lblTotOpeningpercentage") as Label;

            Label lblTotMonthlyDemand = e.Row.FindControl("lblTotMonthlyDemand") as Label;
            Label lblTotMonthlyCollection = e.Row.FindControl("lblTotMonthlyCollection") as Label;
            Label lblTotMonthlypercentage = e.Row.FindControl("lblTotMonthlypercentage") as Label;

            Label lblTotClosingDemand = e.Row.FindControl("lblTotClosingDemand") as Label;
            Label lblTotClosingCollection = e.Row.FindControl("lblTotClosingCollection") as Label;
            Label lblTotClosingpercentage = e.Row.FindControl("lblTotClosingpercentage") as Label;


            lblTotOpeningDemand.Text = TotalArrearDemand.ToString(Funsetsuffix());
            lblTotOpeningCollection.Text = TotalArrearCol.ToString(Funsetsuffix());

            if (TotalArrearDemand == 0)
                lblTotOpeningpercentage.Text = 0.ToString(Funsetsuffix());
            else
                lblTotOpeningpercentage.Text = ((TotalArrearCol / TotalArrearDemand) * 100).ToString(Funsetsuffix());

            lblTotMonthlyDemand.Text = TotalCurrentDemand.ToString(Funsetsuffix());
            lblTotMonthlyCollection.Text = TotalCurrentCol.ToString(Funsetsuffix());

            if (TotalCurrentDemand == 0)
                lblTotMonthlypercentage.Text = 0.ToString(Funsetsuffix());
            else
                lblTotMonthlypercentage.Text = ((TotalCurrentCol / TotalCurrentDemand) * 100).ToString(Funsetsuffix());

            lblTotClosingDemand.Text = TotalTotDemand.ToString(Funsetsuffix());
            lblTotClosingCollection.Text = TotalTotCol.ToString(Funsetsuffix());

            if (TotalTotDemand == 0)
                lblTotClosingpercentage.Text = 0.ToString(Funsetsuffix());
            else
                lblTotClosingpercentage.Text = ((TotalTotCol / TotalTotDemand) * 100).ToString(Funsetsuffix());

            Label lblTotStock = e.Row.FindControl("lblTotStock") as Label;
            Label lblTotNpa = e.Row.FindControl("lblTotNpa") as Label;
            Label lblTotNpaPercentage = e.Row.FindControl("lblTotNpaPercentage") as Label;

            lblTotNpa.Text = TotalNpa.ToString(Funsetsuffix());
            lblTotStock.Text = TotalStock.ToString(Funsetsuffix());

            if (TotalStock == 0)
                lblTotNpaPercentage.Text = 0.ToString(Funsetsuffix());
            else
                lblTotNpaPercentage.Text = ((TotalNpa / TotalStock) * 100).ToString(Funsetsuffix());

            Label lblTotClosingArrear = e.Row.FindControl("lblTotClosingArrear") as Label;
            Label lblTotArrearPercentage = e.Row.FindControl("lblTotArrearPercentage") as Label;

            lblTotClosingArrear.Text = TotalClosingArrear.ToString(Funsetsuffix());

            if (TotalStock == 0)
                lblTotArrearPercentage.Text = 0.ToString(Funsetsuffix());
            else
                lblTotArrearPercentage.Text = ((TotalClosingArrear / TotalStock) * 100).ToString(Funsetsuffix());



        }
    }

    //protected void txtEndMonth_OnTextChanged(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (ddlLOB.SelectedIndex <= 0)
    //        {
    //            Utility.FunShowAlertMsg(this, "Select a Line of Business");
    //            txtEndMonth.Text = "";
    //            return;
    //        }


    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_Id", intCompanyId.ToString());
    //        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
    //        Procparam.Add("@XMLParamtDemandMonthLists", FunPriGetDemandMonths((Dictionary<int, string>)ViewState["Months"]));
    //        DataSet ds = new DataSet();
    //        ds = Utility.GetDataset("S3G_RPT_GetDemandMonthDetails", Procparam);
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            string StrErrorMsg = "Demand not run for the selected months ";
    //            if (ds.Tables[1].Rows.Count > 0)
    //            {
    //                StrErrorMsg += "(";
    //                foreach (DataRow dr in ds.Tables[1].Rows)
    //                {
    //                    StrErrorMsg += dr["Demand_Month"].ToString() + ",";
    //                }
    //                StrErrorMsg = StrErrorMsg.Substring(0, StrErrorMsg.Length - 1);
    //                StrErrorMsg += ")";
    //                Utility.FunShowAlertMsg(this, StrErrorMsg);
    //                //ddlToYearMonthBase.ClearSelection();
    //                txtEndMonth.Text = "";
    //                return;
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        CVPayment.ErrorMessage = "Unable to Validate Demand.";
    //        CVPayment.IsValid = false;
    //    }
    //}

    private Dictionary<int, string> FunPriFillArrayDemandMonth()
    {
        dictDemandmonth = new Dictionary<int, string>();
        int intActualMonth = Convert.ToInt32(ClsPubConfigReader.FunPubReadConfig("StartMonth"));
        int intvalue=0;
        string strActualYear = "";
        string endmonth = "";
        if (txtStartMonth.Text != string.Empty)
        {
            string[] StartYear = txtStartMonth.Text.Split('/');
            string strActualStartYear = StartYear[1].ToString();
            intActualMonth = Convert.ToInt32(StartYear[0].ToString());
            if (txtEndMonth.Text != string.Empty)
                endmonth = txtEndMonth.Text;
            else
            {
                endmonth = DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();
            }
            string[] EndYears = endmonth.Split('/');
            string strActuaEndlYear = EndYears[1].ToString();
            strActualYear = strActualStartYear;
            while (Convert.ToInt32(strActualStartYear) <= Convert.ToInt32(strActuaEndlYear))
            {
                for (int intMonthCnt = 1; intMonthCnt <= 12; intMonthCnt++)
                {
                    if (intActualMonth >= 13)
                    {
                        intActualMonth = 1;
                        strActualStartYear = (Convert.ToInt32(strActualStartYear) + 1).ToString();
                        strActualYear = strActualStartYear.ToString();
                    }
                    System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(strActualYear + intActualMonth.ToString("00"), strActualYear + intActualMonth.ToString("00"));
                    if (Convert.ToInt32((EndYears[1].ToString() + EndYears[0].ToString())) >= Convert.ToInt32(strActualYear + intActualMonth.ToString("00")))
                    {
                        dictDemandmonth.Add(intvalue, liPSelect.Text);
                       
                    }
                    intActualMonth = intActualMonth + 1;
                    intvalue = intvalue + 1;
                }
            }
        }
        int indexvalue = 0;
        Dictionary<int, string> tempdictDemandmonth = new Dictionary<int, string>();
        tempdictDemandmonth = dictDemandmonth;
        foreach (KeyValuePair<int, string> kvp in dictDemandmonth)
        {
            if (kvp.Value.ToString() == endmonth.Split('/')[1].ToString() + endmonth.Split('/')[0].ToString())
                indexvalue = kvp.Key;
            //if (indexvalue > 0)
            //    tempdictDemandmonth.Remove(indexvalue);
            
        }
        ViewState["Months"] = dictDemandmonth;
        return dictDemandmonth;
    }

    private string FunPriGetDemandMonths(Dictionary<int, string> dictDemandmonths)
    {
        string strDemandmnth = "";
        strDemandmnth = "<Root>";
        foreach (KeyValuePair<int, string> kvp in dictDemandmonths)
        {

            strDemandmnth += "<Details  Demand_Month= '" + kvp.Value + "' />";


        }
        strDemandmnth += "</Root>";

        return strDemandmnth;
    }
    #endregion
    #endregion
}
