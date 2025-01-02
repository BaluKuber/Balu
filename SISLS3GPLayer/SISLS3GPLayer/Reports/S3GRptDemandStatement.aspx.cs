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
using ReportOrgColMgtServicesReference;
using System.Globalization;
using Resources;

public partial class Reports_S3GRptDemandStatement : ApplyThemeForProject
{
    
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public int CompanyId;
    public int UserId;
    public int ProgramId;
    public int LobId;
    string LOB_ID;
    bool Is_Active;
    string Branch_ID;
    public int LocationId;
    string strDateFormat;
    public string cutoff_month;
    decimal TotaldueAmount;
    decimal Totalageing30days;
    decimal Totalageing60days;
    decimal Totalageing90days;
    decimal Totalageingabove90days;

    Dictionary<string, string> Procparam = null;
    string strPageName = "DemandStatement";
    ReportAccountsMgtServicesClient objSerClient;
    ReportOrgColMgtServicesClient objser;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriLoadPage();
    }
    private void FunPriLoadPage() 
    {
        try
        {
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
             ObjS3GSession = new S3GSession();
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            UserId = ObjUserInfo.ProUserIdRW;
            ProgramId = 188;
            strDateFormat = ObjS3GSession.ProDateFormatRW;  
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            //string LobId;
            string BranchId;
            string cutoffmonth;
            cutoffmonth = txtdate.Text;
            txtdate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                btnPrint.Visible = false;
                ddllocation2.Enabled = false;
                //PnlDetails.Visible = false;
                FunPriLoadLob(CompanyId, UserId, ProgramId);
                FunPriLoadCategory(CompanyId);
                FunPriLoadBranch(CompanyId, UserId, ProgramId,LobId);
                FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
                             
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Page");
        }
    }

    #region Load LOB

    private void FunPriLoadLob(int CompanyId, int UserId,int ProgramId)
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
            ddlLOB.Items[0].Text = "All";
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

    private void FunPriLoadCategory(int CompanyId)
    {
        try
        {
            objser = new ReportOrgColMgtServicesClient();
            byte[] bytecategory = objser.FunPubGetCategory(CompanyId);
            List<ClsPubDropDownList> category = (List<ClsPubDropDownList>)DeSerialize(bytecategory);
           
            ddlCategory.DataTextField = "Description";
            ddlCategory.DataValueField = "ID";
            ddlCategory.DataSource = category;
            ddlCategory.DataBind();
                  }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Category");
        }
        finally
        {
            objser.Close();
        }
    }

    private void FunPriLoadDebtCode(int CompanyId, string LobId, string BranchId, string cutoffmonth)
    {
        objser = new ReportOrgColMgtServicesClient();
        try
        {
            string YearMonth = txtdate.Text;
            int Month = int.Parse(YearMonth.Substring(0, 2));
            int year = int.Parse(YearMonth.Substring(3, 4));
            string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
            DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
            if (Month < 10)
            {
                cutoff_month = Convert.ToString(year) + "0" + Convert.ToString(Month);
            }
            else
            {
                cutoff_month = Convert.ToString(year) + Convert.ToString(Month);
            }
            objser = new ReportOrgColMgtServicesClient();
            string LOB = string.Empty;
            string Branch = string.Empty;
            string Debtcode = string.Empty;
            string categorycode = string.Empty;
            string option = string.Empty;
            ClsPubDemandSelectionCriteria selectioncriteria = new ClsPubDemandSelectionCriteria();
            selectioncriteria.CompanyId = CompanyId;
            selectioncriteria.LobId = string.Empty;
            selectioncriteria.LocationID1 = string.Empty;
            selectioncriteria.LocationID2 = string.Empty;
            selectioncriteria.DEMANDMONTH = cutoff_month;
            selectioncriteria.UserId = UserId;
            if (ddlLOB.SelectedIndex != 0)
            {
                selectioncriteria.LobId = ddlLOB.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                selectioncriteria.LocationID1 = ddlbranch.SelectedValue;
            }
            if (ddllocation2.SelectedIndex != 0)
            {
                selectioncriteria.LocationID2 = ddllocation2.SelectedValue;
            }

            byte[] byteCode = objser.FunPubGetDebtCollectorCode(selectioncriteria);
            List<ClsPubDropDownList> code = (List<ClsPubDropDownList>)DeSerialize(byteCode);
            ddlDebtCollectorType.DataSource = code;
            ddlDebtCollectorType.DataTextField = "Description";
            ddlDebtCollectorType.DataValueField = "ID";
            ddlDebtCollectorType.DataBind();
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load LOB");
        }
        finally
        {
            if (objser!=null)
            objser.Close();
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

            if (ddlbranch.SelectedIndex > 0)
            {
                LocationId = Convert.ToInt32(ddlbranch.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubGetLocation2(ProgramId, UserId, CompanyId, LobId, LocationId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddllocation2.DataSource = Branch;
            ddllocation2.DataTextField = "Description";
            ddllocation2.DataValueField = "ID";
            ddllocation2.DataBind();
            if (ddllocation2.Items.Count == 2)
            {
                ddllocation2.SelectedIndex = 1;
            }
            else
            {
                ddllocation2.SelectedIndex = 0;
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

    #endregion
    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    private void FunPriLoadBranch(int CompanyId, int UserId, int ProgramId,int LobId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubBranch(CompanyId, UserId, ProgramId,LobId);
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
            ddllocation2.DataSource = Branch;
            ddllocation2.DataTextField = "Description";
            ddllocation2.DataValueField = "ID";
            ddllocation2.DataBind();
            if (ddllocation2.Items.Count == 2)
            {
                ddllocation2.SelectedIndex = 1;
            }
            else
            {
                ddllocation2.SelectedIndex = 0;
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
    private void FunPriValidateFutureDate()
    {
        try
        {
            #region To find Current Year and Month
            //string Today = Convert.ToString(DateTime.Now);
            string YearMonth = txtdate.Text;
            int Currentmonth = DateTime.Now.Month;
            int Currentyear = DateTime.Now.Year;
            #endregion

            int Month = int.Parse(YearMonth.Substring(0, 2));
            int year = int.Parse(YearMonth.Substring(3, 4));
            if (year > Currentyear && Month > Currentmonth)
            {
                txtdate.Text = "";
                Utility.FunShowAlertMsg(this, "Month/Year cannot be Greater than System month/Year.");
                return;
            }
           
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
          
         string YearMonth = txtdate.Text;
         int Month = int.Parse(YearMonth.Substring(0, 2));
         int year = int.Parse(YearMonth.Substring(3, 4));
         string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
         DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
        if (Month < 10)
        {
            cutoff_month = Convert.ToString(year) + "0" + Convert.ToString(Month);
        }
        else
        {
            cutoff_month = Convert.ToString(year) + Convert.ToString(Month);
        }

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_Id", CompanyId.ToString());
        Procparam.Add("@DemandMonth", cutoff_month);

          int ErrorCode=0;
        objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
        ErrorCode = Convert .ToInt32 (objS3GAdminServicesClient.FunGetScalarValue("S3G_RPT_GetDemandMonth", Procparam));  //txtAmtFin.Text.Trim();
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
    private bool FunFindLOBDemandMonth()
    {
        try
        {

            string YearMonth = txtdate.Text;
            int Month = int.Parse(YearMonth.Substring(0, 2));
            int year = int.Parse(YearMonth.Substring(3, 4));
            string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
            DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
            if (Month < 10)
            {
                cutoff_month = Convert.ToString(year) + "0" + Convert.ToString(Month);
            }
            else
            {
                cutoff_month = Convert.ToString(year) + Convert.ToString(Month);
            }

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", CompanyId.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@DemandMonth", cutoff_month);
           int ErrorCode=0;
         objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
         ErrorCode = Convert .ToInt32 (objS3GAdminServicesClient.FunGetScalarValue("S3G_RPT_GetLOBDemandMonth", Procparam));  //txtAmtFin.Text.Trim();
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
    protected void txtdate_OnTextChanged(object sender, EventArgs e)
    {
    try
     {


        FunPriValidateFutureDate();
           FunPriLoadDebtCode(CompanyId, LOB_ID, Branch_ID, cutoff_month);
        PnlDetails.Visible = false;
     btnPrint.Visible = false;
        //txtdate .Text =txtdate .Text .ToString("y",CultureInfo.CreateSpecificCulture("af-ZA"));
       // txtdate.Text = Convert .ToDateTime ( txtdate.Text).ToString("MMM yyyy");
       }
    catch (Exception ex)
    {

    }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlLOB.SelectedValue = "-1";
        ddlbranch.SelectedValue = "-1";
        ddlCategory.SelectedValue = "-1";
        ddlDebtCollectorType.Items.Clear();
       //ddlDebtCollectorType.SelectedValue= "-1";
        ddlAgeing.SelectedValue = "-1";
        //btnPrint.Visible = false;
       PnlDetails.Visible = false;
       ddllocation2.Enabled = false;
       ddllocation2.SelectedValue = "-1";
        txtdate.Text = "";
        btnPrint.Visible=false;
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {

        if (ddlLOB.SelectedIndex > 0)
        {
            if (!FunFindLOBDemandMonth())
            {
                Utility.FunShowAlertMsg(this, "Demand not run for the selected month and LOB ");
                ddlLOB.SelectedValue = "-1";
                txtdate.Text = "";
                return;
            }
            
        }
        else
        {
            if (!FunFindDemandMonth())
            {
                Utility.FunShowAlertMsg(this, "Demand not run for the selected month ");
                txtdate.Text = "";
                return;
            }
            
        }
        if (ddlCategory.SelectedIndex > 0)
        {
            Session["category"] = ddlCategory.SelectedItem.Text;
        }
        else
        {
            Session["category"] = "ALL";
        }

        if (Convert.ToInt32(ddlAgeing.SelectedValue) > 0)
        {
            Session["aging"] = ddlAgeing.SelectedItem.Text;
        }
        else
        {
            Session["aging"] = "ALL";
        }
        //if (Convert.ToInt32(ddlDebtCollectorType.SelectedValue) > 0)
        if (ddlDebtCollectorType.SelectedIndex > 0)
        {
            Session["DCcode"] = ddlDebtCollectorType.SelectedItem.Text;
        }
        else
        {
            Session["DCcode"] = "ALL";
        }
        Session["Cutoff"] = Convert.ToDateTime(txtdate.Text).ToString("MMM yyyy");
        FunPriLoadDemandStatement();
    }
     private void FunPriLoadDemandStatement()
    {
        try
        {
            divDemand.Style.Add("display", "block");
             PnlDetails.Visible = true;
            //grvDemand.Visible = true;
            string YearMonth = txtdate.Text;
            int Month = int.Parse(YearMonth.Substring(0, 2));
            int year = int.Parse(YearMonth.Substring(3, 4));
             string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();
            DateTime CutOffEndDate = Convert.ToDateTime(Month + "/" + cmenddate + "/" + year);
            if (Month < 10)
            {
                cutoff_month = Convert.ToString(year) + "0" + Convert.ToString(Month);
            }
            else
            {
                cutoff_month = Convert.ToString(year) + Convert.ToString(Month);
            }
            objser = new ReportOrgColMgtServicesClient();
            string LOB = string.Empty;
            string Branch = string.Empty;
            string Debtcode = string.Empty;
            string categorycode = string.Empty;
            string option = string.Empty;
            ClsPubDemandSelectionCriteria selectioncriteria = new ClsPubDemandSelectionCriteria();
            selectioncriteria.CompanyId = CompanyId;
            selectioncriteria.LobId = string.Empty;
            selectioncriteria.LocationID1= string.Empty;
            selectioncriteria.LocationID2= string.Empty;
            selectioncriteria.CATEGORY_CODE = string.Empty;
            selectioncriteria.DEBTCOLLECTOR_CODE = string.Empty;
            selectioncriteria.option = string.Empty;
            selectioncriteria.DEMANDMONTH = cutoff_month;
            selectioncriteria.enddate = Convert.ToString(CutOffEndDate);
            selectioncriteria.UserId = UserId;
            if (ddlLOB.SelectedIndex != 0)
            {
                selectioncriteria.LobId = ddlLOB.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                selectioncriteria.LocationID1 = ddlbranch.SelectedValue;
            }
            if (ddlCategory.SelectedIndex != 0)
            {
                selectioncriteria.CATEGORY_CODE = ddlCategory.SelectedValue;
            }
            if (ddlDebtCollectorType.SelectedIndex != 0)
            {
                selectioncriteria.DEBTCOLLECTOR_CODE = ddlDebtCollectorType.SelectedValue;
            }
            if (ddlAgeing.SelectedIndex != 0)
            {
                selectioncriteria.option = ddlAgeing.SelectedValue;
            }
            if (ddllocation2.SelectedIndex != 0)
            {
                selectioncriteria.LocationID2 = ddllocation2.SelectedValue;
            }
            byte[] bytedemand = objser.FunPubGetDemandStatement(selectioncriteria);
            List<ClsPubDemandStatement> demand = (List<ClsPubDemandStatement>)DeSerialize(bytedemand);
            TotaldueAmount = demand.Sum(ClsPubDemandStatement => ClsPubDemandStatement.DueAmount);
            Totalageing30days = demand.Sum(ClsPubDemandStatement => ClsPubDemandStatement.ageing0days);
            Totalageing60days= demand.Sum(ClsPubDemandStatement => ClsPubDemandStatement.ageing30days);
            Totalageing90days = demand.Sum(ClsPubDemandStatement => ClsPubDemandStatement.ageing60days);
            Totalageingabove90days = demand.Sum(ClsPubDemandStatement => ClsPubDemandStatement.ageingabove90days);
            Session["Demand"] = demand;
            grvDemand.DataSource = demand;
            grvDemand.EmptyDataText = "No records found";
            grvDemand.DataBind();
            FunPriDisplayTotalDetails();
            btnPrint.Visible = true;
            //if (grvDemand.Rows.Count != 0)
            //{
            //    grvDemand.HeaderRow.Style.Add("position", "relative");
            //    grvDemand.HeaderRow.Style.Add("z-index", "auto");
            //    grvDemand.HeaderRow.Style.Add("top", "auto");

            //}
            //if (grvDemand.Rows.Count!= 0)
            //{
            //    grvDemand.DataSource = demand;
            //    //grvDemand.EmptyDataText = "No records found";
            //    grvDemand.DataBind();
            //    FunPriDisplayTotalDetails();
            //    btnPrint.Visible = true;
            //    //btnPrint.Visible = false;
            //}
            //else
            //{
            //    grvDemand.EmptyDataText = "No records found";
            //    grvDemand.DataSource = null;
            //    grvDemand.DataBind();
            //    btnPrint.Visible = false;
              
            //}
            
        }
        
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objser.Close();
        }
    }
     private void FunPriDisplayTotalDetails()
     {
         if (grvDemand.Rows.Count > 0)
         {
             ((Label)grvDemand.FooterRow.FindControl("lblDueamountf")).Text = TotaldueAmount.ToString(Funsetsuffix());
             ((Label)grvDemand.FooterRow.FindControl("lblageing0daysf")).Text = Totalageing30days.ToString(Funsetsuffix());
             ((Label)grvDemand.FooterRow.FindControl("lblageing30daysf")).Text = Totalageing60days.ToString(Funsetsuffix());
             ((Label)grvDemand.FooterRow.FindControl("lblageing60daysf")).Text = Totalageing90days.ToString(Funsetsuffix());
             ((Label)grvDemand.FooterRow.FindControl("lblageingabove90daysf")).Text = Totalageingabove90days.ToString(Funsetsuffix());
         }
        
            
             
     }

     protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
     {
         FunPriLoadBranch(CompanyId, UserId, ProgramId, LobId);
         PnlDetails.Visible = false;
         ddlAgeing.SelectedValue = "-1";
         ddlCategory.SelectedValue = "-1";
         ddlDebtCollectorType.Items.Clear();
         btnPrint.Visible = false;
         ddllocation2.Enabled = false;
         FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
         txtdate.Text = "";

         

        // btnPrint.Visible = false;
     }
     protected void ddlDebtCollectorType_SelectedIndexChanged(object sender, EventArgs e)
     {
         
         PnlDetails.Visible = false;
         btnPrint.Visible = false;
     }
     protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
     {
         if (ddlbranch.SelectedIndex > 0)
         {
             ddllocation2.Enabled = true;
             FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId);
             PnlDetails.Visible = false;
             ddlAgeing.SelectedValue = "-1";
             ddlCategory.SelectedValue = "-1";
             ddlDebtCollectorType.Items.Clear();
             btnPrint.Visible = false;
             txtdate.Text = "";
         }
         else
         {
             FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
             ddllocation2.Enabled = false;
             ddlAgeing.SelectedValue = "-1";
             ddlCategory.SelectedValue = "-1";
             ddlDebtCollectorType.Items.Clear();
             btnPrint.Visible = false;
             txtdate.Text = "";
         }

         //btnPrint.Visible = false;
     }
     protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
     {
        
         PnlDetails.Visible = false;
         btnPrint.Visible = false;
     }
     protected void ddlAgeing_SelectedIndexChanged(object sender, EventArgs e)
     {
        
         PnlDetails.Visible = false;
        btnPrint.Visible = false;
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

        string strScipt = "window.open('../Reports/S3GRptDemandStatementReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "DemandStatement", strScipt, true);
    }
 
    
   
}
