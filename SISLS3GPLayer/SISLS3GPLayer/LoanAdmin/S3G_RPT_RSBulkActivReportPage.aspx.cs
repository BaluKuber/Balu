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
using S3GBusEntity.Reports;
using CrystalDecisions.CrystalReports.Engine;

public partial class LoanAdmin_S3G_RPT_RSBulkActivReportPage : System.Web.UI.Page
{
    Dictionary<string, string> ProcParam = null;
    DataSet dsprint;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            ProcParam = (Dictionary<string, string>)HttpContext.Current.Session["ProcParam"];
            dsprint = Utility.GetDataset("S3G_Fund_RSBulkAct_Print", ProcParam);
            ReportDocument rptd = new ReportDocument();
            rptd.Load(Server.MapPath("BulkActCashFlwPrint.rpt"));
            rptd.SetDataSource(dsprint.Tables[0]);
            rptd.Subreports["CashFlSub.rpt"].SetDataSource(dsprint.Tables[1]);
            CRVCashFlow.ReportSource = rptd;
            CRVCashFlow.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}