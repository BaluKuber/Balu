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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using S3GBusEntity;
using S3GBusEntity.Reports;

public partial class Reports_S3GCollectionAnalysisReport : System.Web.UI.Page
{
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;
    ReportDocument rptd = new ReportDocument();
    Dictionary<string, string> ProcParam;
    ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient ObjOrgColClient = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
    int ColumnWidth = 0;
    int ColumnHeight = 0;
    int ColumnLeft = 0;
    int SetColumnLeft = 0;
    TextObject TxtObj;
    FormulaFieldDefinition FrmObj;
    FieldObject FldObj;
    protected void Page_Init(object sender, EventArgs e)
    {
         ClspubCollectionReturnAmount CollectionPerformance = new ClspubCollectionReturnAmount();
        ObjUserInfo = new UserInfo();
        ObjS3GSession = new S3GSession();
        rptd.Load(Server.MapPath("Report/CollectionPerformance.rpt"));
        //Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;

        TextObject txtDateTime = (TextObject)rptd.ReportDefinition.ReportObjects["txtDateTime"];
        txtDateTime.Text = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString(); 
        TextObject TxtCompany = (TextObject)rptd.ReportDefinition.ReportObjects["txtcompany"];
        TxtCompany.Text = ObjUserInfo.ProCompanyNameRW.ToString();
        TextObject TxtDenomination = (TextObject)rptd.ReportDefinition.ReportObjects["txtdenomination"];
        TxtDenomination.Text = "[All Amounts are in " + ObjS3GSession.ProCurrencyNameRW +"]";
        TextObject TxtRptTitle = (TextObject)rptd.ReportDefinition.ReportObjects["TxtRptTitle"];
        TxtRptTitle.Text = "COMPARATIVE ANALYSIS REPORT FOR THE PERIOD FROM " + Session["FromDate"] + " TO " + Session["ToDate"] + " and " + Session["FromComDate"] + " TO " + Session["ToComDate"];
      //  List<ClsPubCollectionPerformance > DSDPDReport = (List<ClsPubCollectionPerformance>)Session["COLANALDTLS"];
        CollectionPerformance = (ClspubCollectionReturnAmount)Session["CollPerformance"];
        FrmObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["P1DueAmount"];
        string Period1 = "Due Amount " + System.Environment.NewLine + Session["FromDate"] + " to " + Session["ToDate"];
        FrmObj.Text = "'" + Period1.Replace("\r\n", "' + chr(10) +'") + "'";
        string Period2 = "Due Amount " + System.Environment.NewLine + Session["FromComDate"] + " to " + Session["ToComDate"]; 
        FrmObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["P2DueAmount"];
        FrmObj.Text = "'" + Period2.Replace("\r\n", "' + chr(10) +'") + "'";
        TextObject txtPeriod1 = (TextObject)rptd.ReportDefinition.ReportObjects["txtPeriod1"];
        TextObject txtPeriod2 = (TextObject)rptd.ReportDefinition.ReportObjects["txtPeriod2"];
        txtPeriod1.Text = "** Period 1: From " + Session["FromDate"] + " to " + Session["ToDate"];
        txtPeriod2.Text = "** Period 2: From " + Session["FromComDate"] + " to " + Session["ToComDate"];
        FrmObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["GPSSuffix"];
        FrmObj.Text = "'" + ObjS3GSession.ProGpsSuffixRW + "'";
        rptd.SetDataSource(CollectionPerformance.GetCollectionAnalysis);
        CRVDPDRPT.ReportSource = rptd;
        CRVDPDRPT.DataBind();
    }
}
