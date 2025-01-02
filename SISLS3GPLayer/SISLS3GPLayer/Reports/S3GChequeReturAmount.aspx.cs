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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using S3GBusEntity.Reports;
using System.Collections.Generic;
public partial class Reports_S3GChequeReturAmount : System.Web.UI.Page
{
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            ClspubCollectionReturnAmount CollectionPerformance = new ClspubCollectionReturnAmount();
            if (Session["CollPerformance"] != "")
            {

                CollectionPerformance = (ClspubCollectionReturnAmount)Session["CollPerformance"];

                rptd.Load(Server.MapPath("Report/ChequeReturnAmount.rpt"));
                TextObject txtCompany = (TextObject)rptd.ReportDefinition.ReportObjects["txtCompany"];
                TextObject txtClnPerformanceTitle = (TextObject)rptd.ReportDefinition.ReportObjects["txtClnPerformance"];
                TextObject txtDateTime = (TextObject)rptd.ReportDefinition.ReportObjects["txtDateTime"];
                txtDateTime.Text = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString(); 
                txtCompany.Text = ObjUserInfo.ProCompanyNameRW;
                TextObject TxtDenomination = (TextObject)rptd.ReportDefinition.ReportObjects["txtDenomination"];
                TxtDenomination.Text = "[All Amounts are in " + ObjS3GSession.ProCurrencyNameRW + "]";
                txtClnPerformanceTitle.Text = "CHEQUE RETURN REPORT FOR THE PERIOD FROM " + Session["FromDate"].ToString() + " TO " + Session["ToDate"].ToString();
                FormulaFieldDefinition FrmObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["GPSSuffix"];
                FrmObj.Text ="'" + ObjS3GSession.ProGpsSuffixRW +"'";
                rptd.SetDataSource(CollectionPerformance.GetChequeReturnAmount);
                CRVCollectionPerformanceReport.ReportSource = rptd;
                CRVCollectionPerformanceReport.DataBind();
               // rptd.Close();
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
       
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        rptd.Close();
        rptd.Dispose();
    }
}
