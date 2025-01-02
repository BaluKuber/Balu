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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class Reports_S3GRptPricingReport : System.Web.UI.Page
{
    ReportDocument rptd = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            rptd.Load(Server.MapPath("Report/PricingReport.rpt"));
            DataSet ds = new DataSet();
            if (Session["Report_Data"] != null)
                ds = (DataSet)Session["Report_Data"];
            rptd.SetDataSource(ds);

            TextObject Date = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtdate"];
            Date.Text = Session["Date"].ToString();

            TextObject Denomination = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtCurrency"];
            Denomination.Text = Session["Denomination"].ToString();

            CRVPR.ReportSource = rptd;
            CRVPR.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
