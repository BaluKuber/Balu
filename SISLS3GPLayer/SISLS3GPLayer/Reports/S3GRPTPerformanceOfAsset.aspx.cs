
#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Performance of Asset 
/// Created By          :   Muni kavitha
/// Created Date        :   1-Aug-2011
/// Purpose             :   To Get the Performance of Assets 
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

public partial class Reports_S3GRPTPerformanceOfAsset : ApplyThemeForProject
{

    #region Variable Declaration
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    string intCustomerId;
    int intUserId;

    bool Is_Active;
    int Active;
    int ProgramId = 198;
    string strFilePath = string.Empty;
    Dictionary<string, string> Procparam;
    string strPageName = "Performance of Asset";
    DataTable dtTable = new DataTable();
    List<decimal> lisTotaldec = new List<decimal>();
    ArrayList arlist = new ArrayList();
    ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient objReportOrgMgtServices;
    ReportAccountsMgtServicesClient objReportAccountMgtServices;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
    decimal BCD, BCDAmt, BDD, BDDAmt, BCM, BCMAmt, BDM, BDMAmt, BCY, BCYAmt, BDY, BDYAmt;
    //decimal BDYAmt;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVPerformanceofAsset.ErrorMessage = "Due to Data Problem, Unable to Load Page";
            CVPerformanceofAsset.IsValid = false;
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
            if (!IsPostBack)
            {
                FunPriLoadLob();
                FunPubLoadDenomination();
                if (gvPerformanceAsset.Rows.Count <= 0)
                    lblAmounts.Visible = pnlWIRR.Visible = false;
                //ddlReportType.SelectedValue = "2";
                //Utility.ClearDropDownList(ddlReportType);
                // FunPriLoadBranch(intCompanyId, intUserId, Is_Active);
            }
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
            ddlLOB.Items.Clear();
            byte[] byteLobs = objReportOrgMgtServices.FunPubLOBAssetPerf(ObjUserInfo.ProCompanyIdRW, ObjUserInfo.ProUserIdRW, 1, ProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;

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
    }


    private void FunPriLoadAssetGrid()
    {
        objReportOrgMgtServices = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
        try
        {
            int FinYearStartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
            string strCurrentDate = DateTime.Now.ToString("yyyyMMdd");
            int year = int.Parse(strCurrentDate.Substring(0, 4));
            int Month = int.Parse(strCurrentDate.Substring(4, 2));
            string DaysinMonth = System.DateTime.DaysInMonth(year, Month).ToString();
            //string CurrentMonthStart = "";
            //string FinYearStart = dt.Substring(0, 4) + FinYearStartMonth.ToString ("00");

            string CurrentDate = DateTime.Now.ToString("MM/dd/yyyy");

            string FinancialYear = "";
            if (FinYearStartMonth < Month)
            {
                FinancialYear = Convert.ToString(year);
            }
            else
            {
                FinancialYear = Convert.ToString(year - 1);
            }

            //DateTime CurrentMonthStart = Convert.ToDateTime(Month + "/" + "1" + "/" + year);
            //DateTime CurrentMonthEnd = Convert.ToDateTime(Month + "/" + DaysinMonth + "/" + year);

            //DateTime FinYearStartDate = Convert.ToDateTime(FinYearStartMonth + "/" + "1" + "/" + FinancialYear);

            string CurrentMonthStart = Month + "/" + "1" + "/" + year;
            string CurrentMonthEnd = Month + "/" + DaysinMonth + "/" + year;
            string FinYearStartDate = FinYearStartMonth + "/" + "1" + "/" + FinancialYear;
            //string financialyear;
            //string YearMonth = txtCutoffMonthSearch.Text;
            //int year = int.Parse(YearMonth.Substring(3, 4));
            //int Month = int.Parse(YearMonth.Substring(0, 2));
            //string cmenddate = System.DateTime.DaysInMonth(year, Month).ToString();

            ClsPubAssetPerfParam AssetParams = new ClsPubAssetPerfParam();
            AssetParams.CompanyId = ObjUserInfo.ProCompanyIdRW;
            AssetParams.IRR_Type = Convert.ToInt32(ddlIRRType.SelectedValue);
            AssetParams.Report_Type = Convert.ToInt32(ddlReportType.SelectedValue);
            AssetParams.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            AssetParams.CurrentDate = CurrentDate;
            AssetParams.CurrentMonthStartDate = CurrentMonthStart;
            AssetParams.FinYearStartDate = FinYearStartDate;
            AssetParams.Denomintion = Convert.ToDecimal(ddlDenomination.SelectedValue);
            AssetParams.User_ID = ObjUserInfo.ProUserIdRW;

            //AssetParams.FinMonthYear = ConfigurationManager.AppSettings["StartMonth"];
            //AssetParams.FinMonthYear = FinYearStart;
            //AssetParams.RptGenTime = DateTime.Now.ToString ();

            byte[] byteAssets = objReportOrgMgtServices.FunPubGetAssestPerformance(AssetParams);
            List<ClsPubAssetPerformance> ListAssetPerformance = (List<ClsPubAssetPerformance>)DeSeriliaze(byteAssets);
            lblAmounts.Visible = pnlWIRR.Visible = true;
            if (ListAssetPerformance.Count != 0)
            {
                ////                lblAmounts.Visible = pnlWIRR.Visible = true;
                //                if (gvPerformanceAsset.Rows.Count == 0)
                //                {
                //                    gvPerformanceAsset.EmptyDataText = "No records Found";
                //                }
                //                //else
                //                //{
                //                //    //gvPerformanceAsset.HeaderRow.Style.Add("position", "relative");
                //                //    //gvPerformanceAsset.HeaderRow.Style.Add("z-index", "auto");
                //                //    //gvPerformanceAsset.HeaderRow.Style.Add("top", "auto");
                //                //    btnPrint.Visible = true;
                //                //}
                //            }
                //            else
                //            {ListAssetPerformance
                lblNoRecords.Visible = false;
                gvPerformanceAsset.Visible = true;

                btnPrint.Visible = btnChart.Visible = true;
                gvPerformanceAsset.DataSource = ListAssetPerformance;
                gvPerformanceAsset.DataBind();

                BCD = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BCD);
                BCDAmt = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BCDAmt);
                BDD = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BDD);
                BDDAmt = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BDDAmt);

                BCM = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BCM);
                BCMAmt = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BCMAmt);
                BDM = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BDM);
                BDMAmt = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BDMAmt);

                BCY = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BCY);
                BCYAmt = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BCYAmt);
                BDY = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BDY);
                BDYAmt = ListAssetPerformance.Sum(ClsPubAssetPerformance => ClsPubAssetPerformance.BDYAmt);

                Label lblBCD = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBCD");
                Label lblBCDAmt = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBCDAmt");
                Label lblBDD = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBDD");
                Label lblBDDAmt = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBDDAmt");

                Label lblBCM = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBCM");
                Label lblBCMAmt = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBCMAmt");
                Label lblBDM = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBDM");
                Label lblBDMAmt = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBDMAmt");

                Label lblBCY = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBCY");
                Label lblBCYAmt = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBCYAmt");
                Label lblBDY = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBDY");
                Label lblBDYAmt = (Label)gvPerformanceAsset.FooterRow.FindControl("lblBDYAmt");

                if (BCD > 0) BCD = 100;
                if (BDD > 0) BDD = 100;
                if (BCM > 0) BCM = 100;
                if (BDM > 0) BDM = 100;
                if (BCY > 0) BCY = 100;
                if (BDY > 0) BDY = 100;
                //  if (BDY > 100) BDY = 100;

                FunPriSetGPSSuffix(BCD, lblBCD);
                FunPriSetGPSSuffix(BDD, lblBDD);
                FunPriSetGPSSuffix(BCM, lblBCM);
                FunPriSetGPSSuffix(BDM, lblBDM);
                FunPriSetGPSSuffix(BCY, lblBCY);
                FunPriSetGPSSuffix(BDY, lblBDY);

                if (ddlDenomination.SelectedValue == "1")
                {
                    FunPriNoGPSSuffix(BCDAmt, lblBCDAmt);
                    FunPriNoGPSSuffix(BDDAmt, lblBDDAmt);
                    FunPriNoGPSSuffix(BCMAmt, lblBCMAmt);
                    FunPriNoGPSSuffix(BDMAmt, lblBDMAmt);
                    FunPriNoGPSSuffix(BCYAmt, lblBCYAmt);
                    FunPriNoGPSSuffix(BDYAmt, lblBDYAmt);
                }
                else
                {
                    FunPriSetGPSSuffix(BCDAmt, lblBCDAmt);
                    FunPriSetGPSSuffix(BDDAmt, lblBDDAmt);
                    FunPriSetGPSSuffix(BCMAmt, lblBCMAmt);
                    FunPriSetGPSSuffix(BDMAmt, lblBDMAmt);
                    FunPriSetGPSSuffix(BCYAmt, lblBCYAmt);
                    FunPriSetGPSSuffix(BDYAmt, lblBDYAmt);
                }
                Session["PerformanceofAsset"] = ListAssetPerformance;

                if (ddlDenomination.SelectedValue == "1")
                {
                    lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
                }
                else
                {
                    lblAmounts.Text = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
                }
                // Session["AccountingCurrency"] = lblAmounts.Text;
            }
            if (ListAssetPerformance.Count == 0)
            {
                lblNoRecords.Visible = true;
                gvPerformanceAsset.DataSource = null;
                gvPerformanceAsset.DataBind();
                gvPerformanceAsset.Visible = false;
                //gvPerformanceAsset.EmptyDataText = "No records Found";
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
    }

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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        FunPriLoadAssetGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlLOB.ClearSelection();
        ddlReportType.ClearSelection();
        ddlIRRType.ClearSelection();
        ddlDenomination.ClearSelection();
        gvPerformanceAsset.DataSource = null;
        lblAmounts.Visible = pnlWIRR.Visible = false;
        btnPrint.Visible = btnChart.Visible = false;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {

            Session["RType"] = ddlReportType.SelectedItem.Text;
            Session["LOB"] = ddlLOB.SelectedItem.Text;
            Session["IRRType"] = ddlIRRType.SelectedItem.Text;
            Session["CompName"] = ObjUserInfo.ProCompanyNameRW.ToString();
            Session["HeaderDateTime"] = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            // Session["Rhead"] = "Weighted Average IRR as on date " + DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW);
            Session["AmountIn"] = lblAmounts.Text; //"[All Amounts are in " +ObjS3GSession.ProCurrencyNameRW.ToString ()+ "]";
            // Session["GPSdecimal"] = ObjS3GSession.ProGpsSuffixRW.ToString ();
            string strScipt = "window.open('../Reports/S3GPerformanceofAssetReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Weighted Average IRR", strScipt, true);

            //FunToViewChartORReport();
            //Session["CRoption"] = "R";
        }
        catch (Exception ex)
        {
            CVPerformanceofAsset.ErrorMessage = "Due to Data Problem, Unable to Print the Report";
            CVPerformanceofAsset.IsValid = false;
        }

    }


    protected void btnChart_Click(object sender, EventArgs e)
    {
        try
        {
            //FunToViewChartORReport();
            //Session["CRoption"] = "C";
            Session["RType"] = ddlReportType.SelectedItem.Text;
            Session["LOB"] = ddlLOB.SelectedItem.Text;
            Session["IRRType"] = ddlIRRType.SelectedItem.Text;
            Session["CompName"] = ObjUserInfo.ProCompanyNameRW.ToString();
            Session["HeaderDateTime"] = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            // Session["Rhead"] = "Weighted Average IRR as on date " + DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW);
            Session["AmountIn"] = lblAmounts.Text; //"[All Amounts are in " +ObjS3GSession.ProCurrencyNameRW.ToString ()+ "]";
            // Session["GPSdecimal"] = ObjS3GSession.ProGpsSuffixRW.ToString ();
            string strScipt = "window.open('../Reports/S3GPerformanceofAssetPieChart.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Weighted Average IRR", strScipt, true);

        }
        catch (Exception ex)
        {
            CVPerformanceofAsset.ErrorMessage = "Due to Data Problem, Unable to View the Chart";
            CVPerformanceofAsset.IsValid = false;
        }

    }


    protected void FunToViewChartORReport()
    {
        try
        {
            Session["RType"] = ddlReportType.SelectedItem.Text;
            Session["LOB"] = ddlLOB.SelectedItem.Text;
            Session["IRRType"] = ddlIRRType.SelectedItem.Text;
            Session["CompName"] = ObjUserInfo.ProCompanyNameRW.ToString();
            Session["HeaderDateTime"] = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            // Session["Rhead"] = "Weighted Average IRR as on date " + DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW);
            Session["AmountIn"] = lblAmounts.Text; //"[All Amounts are in " +ObjS3GSession.ProCurrencyNameRW.ToString ()+ "]";
            // Session["GPSdecimal"] = ObjS3GSession.ProGpsSuffixRW.ToString ();
            string strScipt = "window.open('../Reports/S3GPerformanceofAssetReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Weighted Average IRR", strScipt, true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnEmail_Click(object sender, EventArgs e)
    {
        PopupSelectUsersToMail.Enabled = true;
        PopupSelectUsersToMail.Show();
        //PnlSendmail.Visible = true;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        DataTable dt = Utility.GetDefaultData("S3G_RPT_GetUserToMail", Procparam);
        gvmail.DataSource = dt;
        gvmail.DataBind();
    }
    private List<ClsPubAssetPerformance> GetPerformanceofAsset()
    {
        List<ClsPubAssetPerformance> PerformanceofAsset;

        if (Session["PerformanceofAsset"] == null)
        {
            PerformanceofAsset = new List<ClsPubAssetPerformance>();
        }
        else
        {
            PerformanceofAsset = (List<ClsPubAssetPerformance>)Session["PerformanceofAsset"];
        }
        return PerformanceofAsset;
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        try
        {
            string strTouserMailId = string.Empty;
            CheckBox chkSelectAll = (CheckBox)gvmail.HeaderRow.FindControl("chkSelectAll");
            if (chkSelectAll.Checked == true)
            {
                for (int i = 0; i < gvmail.Rows.Count; i++)
                {
                    Label lblmailid = (Label)gvmail.Rows[i].FindControl("lblmailid");
                    strTouserMailId = strTouserMailId + lblmailid.Text.Trim() + ",";
                }
            }
            else
            {
                for (int i = 0; i < gvmail.Rows.Count; i++)
                {
                    Label lblmailid = (Label)gvmail.Rows[i].FindControl("lblmailid");
                    CheckBox chkSelect = (CheckBox)gvmail.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                        strTouserMailId = strTouserMailId + lblmailid.Text.Trim() + ",";
                }
            }
            if (string.IsNullOrEmpty(strTouserMailId))
            {
                Utility.FunShowAlertMsg(this, "Select atleast one user to send mail");
                return;
            }

            FunProGeneratePDFToAttach();
            ArrayList arrMailAttachement = new ArrayList();
            arrMailAttachement.Add(strFilePath);

            FunPubSentMail(strTouserMailId, arrMailAttachement);
        }
        catch (Exception ex)
        {
            CVmails.ErrorMessage = "Invalid EMail ID. Mail not sent.";
            CVmails.IsValid = false;
        }
    }

    protected void FunProGeneratePDFToAttach()
    {
        ReportDocument rptd = new ReportDocument();

        rptd.Load(Server.MapPath("Report/PerformanceOfAsset.rpt"));
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["CompName"];
        Company.Text = ObjUserInfo.ProCompanyNameRW.ToString();

        TextObject AmountIn = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["AmountIn"];
        AmountIn.Text = lblAmounts.Text;

        TextObject IRRType = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["IRRType"];
        IRRType.Text = ddlIRRType.SelectedItem.Text;

        TextObject LOB = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["LOB"];
        LOB.Text = ddlLOB.SelectedItem.Text;

        //TextObject Rhead = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["Rhead"];
        //Rhead.Text = "Weighted Average IRR as on date " + DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW);

        TextObject RType = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["RType"];
        RType.Text = ddlReportType.SelectedItem.Text;

        TextObject HeaderDateTime = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["HeaderDateTime"];
        HeaderDateTime.Text = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

        Guid objGuid;
        objGuid = Guid.NewGuid();

        // strFilePath =@"\\chnwsfsr01\Smartlend NXG\Team\kavitha\"+ objGuid +"WIRR.PDF"; //Server.MapPath(".") + "\\PDF Files\\" + LMPath;
        strFilePath = Server.MapPath(".") + "\\PDF Files\\" + objGuid + "WIRR.PDF";
        rptd.SetDataSource(GetPerformanceofAsset());
        rptd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, strFilePath);

    }

    public void FunPubSentMail(string strTOuser, ArrayList FileToAttach)
    {

        CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
        objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {
            string body;
            //body = GetHTMLText();
            body = "Dear sir / madam<br/><br/> ";
            body = body + "Please find attached the Weighted Average IRR report as of yesterday. This is an auto generated mail. Please do not respond to this mail.<br/>";
            body = body + "Please do not print this email unless it is absolutely necessary. Spread environmental awareness.";

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@option", "1");
            string strFromUser = objS3GAdminServicesClient.FunGetScalarValue("S3G_RPT_GetUserToMail", Procparam).ToString();

            ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
            ObjCom_Mail.ProFromRW = strFromUser;// "s3g@sundaraminfotech.in";
            ObjCom_Mail.ProTORW = strTOuser; //strTouserMailId;
            ObjCom_Mail.ProSubjectRW = "Weighted IRR Report";
            ////Need to Inplement Body of mail"
            ObjCom_Mail.ProMessageRW = body;
            ObjCom_Mail.ProFileAttachementRW = FileToAttach;
            ObjCommonMail.FunSendMail(ObjCom_Mail);
            Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_SucMsg_MailSent);
        }
        catch (FaultException<CommonMailServiceReference.ClsPubFaultException> ex)
        {
            if (ObjCommonMail != null)
                ObjCommonMail.Close();
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
        catch (Exception ex)
        {
            if (ObjCommonMail != null)
                ObjCommonMail.Close();
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
        finally
        {
            if (ObjCommonMail != null)
                ObjCommonMail.Close();
            objS3GAdminServicesClient.Close();
        }
    }

    protected void chkSelectAll(object sender, EventArgs e)
    {
        CheckBox chkAll = (CheckBox)sender;
        //GridView g = (GridView)chkAll.Parent.Parent.Parent.Parent;
        if (chkAll.Checked == true)
        {
            for (int i = 0; i < gvmail.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)gvmail.Rows[i].FindControl("chkSelect");
                chkSelect.Checked = true;
            }
        }
        else if (chkAll.Checked == false)
        {
            for (int i = 0; i < gvmail.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)gvmail.Rows[i].FindControl("chkSelect");
                chkSelect.Checked = false;
            }
        }
    }

    protected void gvmail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
            //CheckBox chkSelectAll = gvmail.HeaderRow.FindControl("chkSelectAll") as CheckBox;
            if (chkSelect != null)
            {
                chkSelect.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + gvmail.ClientID + "','chkSelectAll','chkSelect');");
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("chkSelectAll");
            chkSelectAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvmail.ClientID + "',this,'chkSelect');");
        }

    }
    protected void btnClosePopup_Click(object sender, EventArgs e)
    {
        PopupSelectUsersToMail.Hide();
    }
    /// <summary>
    /// To assign the Repayment Details Total in footer row
    /// </summary>
    //private void FunPriDisplayTotal()
    //{
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblInstallmentAmount")).Text = TotalInstallmentAmount.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblPrincipalAmount")).Text = TotalPrincipalAmount.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblFinanceCharges")).Text = TotalFinanceCharges.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblUMFC")).Text = TotalUMFC.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblInsuranceAmount")).Text = TotalInsuranceAmount.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblOthers")).Text = TotalOthers.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblVatRecovery")).Text = TotalVatrecovery.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblTaxSetOff")).Text = TotalVatSetoff.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblTax")).Text = TotalTax.ToString(Funsetsuffix());
    //}

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
            ddlLOB.ClearSelection();
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
            FunClearGrid();
        }
        catch (Exception ex)
        {
            CVPerformanceofAsset.ErrorMessage = ex.Message;
            CVPerformanceofAsset.IsValid = false;
        }
    }


    protected void ddlIRRType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunClearGrid();
        }
        catch (Exception ex)
        {
            CVPerformanceofAsset.ErrorMessage = ex.Message;
            CVPerformanceofAsset.IsValid = false;
        }
    }


    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunClearGrid();
        }
        catch (Exception ex)
        {
            CVPerformanceofAsset.ErrorMessage = ex.Message;
            CVPerformanceofAsset.IsValid = false;
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
            CVPerformanceofAsset.ErrorMessage = ex.Message;
            CVPerformanceofAsset.IsValid = false;
        }
    }

    protected void FunClearGrid()
    {
        try
        {
            lblAmounts.Visible = btnPrint.Visible = btnChart.Visible = pnlWIRR.Visible = false;
            gvPerformanceAsset.DataSource = null;
            gvPerformanceAsset.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion




}
