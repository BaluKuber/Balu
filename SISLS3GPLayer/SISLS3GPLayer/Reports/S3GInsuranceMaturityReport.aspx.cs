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

public partial class Reports_S3GInsuranceMaturityReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        //if (Convert .ToString ( Session["IsAbstract"]) =="true")
        //{

        rptd.Load(Server.MapPath("Report/InsuranceMaturityReport.rpt"));
        rptd.SetDataSource(Session["InsuranceMaturity"]);
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();
        TextObject header = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtheader"];
        header.Text = "Insurance Maturity Report";
        TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtdate"];
        date.Text = Session["Date"].ToString();
        //rptd.Subreports["HeaderDetails.rpt"].SetDataSource(GetHeaderDetails());
        

        //Currency.Text = "[All amounts are in" + " " + Session["Currency"].ToString() + "]";

        //TextObject Currencys = (TextObject)rptd.Subreports["Details.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
        //Currencys.Text = "[All amounts are in" + " " + Session["Currency"].ToString() + "]";
        CRVInsuranceMaturityReport.ReportSource = rptd;
        CRVInsuranceMaturityReport.DataBind();

    }

}
