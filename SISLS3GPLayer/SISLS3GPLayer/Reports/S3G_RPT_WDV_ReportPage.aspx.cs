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

public partial class Reports_S3G_RPT_WDV_ReportPage : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        ReportDocument rptd = new ReportDocument();
        DataSet ds = new DataSet();
        rptd.Load(Server.MapPath("Report/WDVReport.rpt"));
        //TextObject openingbalance = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtopeningbalance"];
        //openingbalance.Text = Session["OpeningBalance"].ToString();
        //TextObject openingbalance1 = (TextObject)rptd.ReportDefinition.Sections["PageHeaderSection1"].ReportObjects["txtopeningbalance1"];
        //openingbalance1.Text = Session["OpeningBalance"].ToString();
        ds = (DataSet)Session["DT_Grid"];
        rptd.SetDataSource(ds);
        CRVWDVReport.ReportSource = rptd;
        CRVWDVReport.DataBind();
    }
}
