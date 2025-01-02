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
using S3GBusEntity.Reports;
using System.Collections.Generic;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class Reports_S3GRptDemandCCLReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        ClsPubHeaderDetails Headerdetail = (ClsPubHeaderDetails)Session["Header"];

        ReportDocument rptd = new ReportDocument();
        rptd.Load(Server.MapPath("Report/DemandCCLReportClass.rpt"));


        rptd.Subreports["DemandCCLSummary"].SetDataSource(GetDemandCollection());
        TextObject Currency = (TextObject)rptd.Subreports["DemandCCLSummary"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtCurrency"];
        Currency.Text = Session["Denomination"].ToString();

        rptd.Subreports["DemandCCLHeaderReport.rpt"].SetDataSource(GetHeaderdetails());
        
        TextObject LOB = (TextObject)rptd.Subreports["DemandCCLSummary"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtlob"];
        LOB.Text = Session["LOB"].ToString();

        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtCompany"];
        Company.Text = Session["Company"].ToString();

        TextObject StockDate = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtDate"];
        StockDate.Text = Session["Date"].ToString();

        CRVDemandCCL.ReportSource = rptd;
        CRVDemandCCL.DataBind();
    }
    private List<ClsPubDemandCollection> GetDemandCollection()
    {
        List<ClsPubDemandCollection> DemandCollection;

        if (Session["DemandCollection"] == null)
        {
            DemandCollection = new List<ClsPubDemandCollection>();
        }
        else
        {
            DemandCollection = (List<ClsPubDemandCollection>)Session["DemandCollection"];

        }
        return DemandCollection;
    }

    private List<ClsPubHeaderDetails> GetHeaderdetails()
    {
        List<ClsPubHeaderDetails> Headerdetails = new List<ClsPubHeaderDetails>();
        if (Session["Header"] != null)
        {
            ClsPubHeaderDetails Headerdetail = (ClsPubHeaderDetails)Session["Header"];
            Headerdetails.Add(Headerdetail);
        }
        return Headerdetails;
    }
}
