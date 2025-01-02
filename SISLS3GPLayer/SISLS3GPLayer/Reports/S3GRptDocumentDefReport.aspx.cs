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

public partial class Reports_S3GRptDocumentDefReport : System.Web.UI.Page
{
    ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            rptd.Load(Server.MapPath("Report/DocDeficiencyReport.rpt"));
            DataTable dt = new DataTable();
            TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtdate"];
            date.Text = Session["Date"].ToString();
            if (Session["DocDef"] != null)
                dt = (DataTable)Session["DocDef"];
            rptd.SetDataSource(dt);
            
            CRVDocDef.ReportSource = rptd;
            CRVDocDef.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
