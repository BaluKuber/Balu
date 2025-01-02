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

public partial class Reports_S3GFactoringBusinessReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            rptd.Load(Server.MapPath("Report/FactoringBusinessReport.rpt"));
            DataTable dt = new DataTable();
            TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtdate"];
            date.Text = Session["Date"].ToString();
            if (Session["DocDef"] != null)
                dt = (DataTable)Session["DocDef"];
            rptd.SetDataSource(dt);

            CRVFactoringBusinessReport.ReportSource = rptd;
            CRVFactoringBusinessReport.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
