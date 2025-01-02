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

public partial class Reports_S3GFactoringMaturityReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_Init(object sender, EventArgs e)
    {        
        rptd.Load(Server.MapPath("Report/FactoringMaturityReport.rpt"));
        rptd.SetDataSource(GetRptDet());


        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();

        TextObject header = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtheader"];
        header.Text = "Factoring Maturity Report as on ";

        TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtdate"];
        date.Text = Session["Date"].ToString();
        

        CRVFactoringMaturityReport.ReportSource = rptd;
        CRVFactoringMaturityReport.DataBind();        

    }


    private List<ClsPubFactoringMaturityDetails> GetRptDet()
    {
        ClsPubFactoringMaturity factoring = new ClsPubFactoringMaturity();
        List<ClsPubFactoringMaturityDetails> rptdet = new List<ClsPubFactoringMaturityDetails>();

        if (Session["Report"] != null)
        {
            factoring = (ClsPubFactoringMaturity)Session["Report"];
            rptdet = factoring.FactoringMaturity;
        }
        else
        {
            rptdet = new List<ClsPubFactoringMaturityDetails>();

        }

        return rptdet;

    }


}
