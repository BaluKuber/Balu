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

public partial class Reports_S3GRptDemandCollectionReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {

        ClsPubHeaderDetails Headerdetail = (ClsPubHeaderDetails)Session["Header"];
        //if (Session["Report"].ToString() == "Report")
        //{
        rptd.Load(Server.MapPath("Report/DemandCollectionRegionBranchWise.rpt"));
        rptd.Subreports["DemandCollectionHeaderReport.rpt"].SetDataSource(GetHeaderdetails());
        TextObject Company = (TextObject)rptd.Subreports["DemandCollectionHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtCompany"];
        Company.Text = Session["Company"].ToString();
        TextObject Date = (TextObject)rptd.Subreports["DemandCollectionHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtDate"];
        Date.Text = Session["Date"].ToString();

        rptd.Subreports["DemandCollectionReport.rpt"].SetDataSource(GetDemandCollection());
        TextObject denomination = (TextObject)rptd.Subreports["DemandCollectionReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtCurrency"];
        denomination.Text = Session["Denomination"].ToString();
        TextObject LOB = (TextObject)rptd.Subreports["DemandCollectionReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtLOB"];
        LOB.Text = Session["LOB"].ToString();

        //rptd.Subreports["DemandCollectionDetailedReport.rpt"].SetDataSource(GetDemandCollectiondetails());
        //TextObject Currency = (TextObject)rptd.Subreports["DemandCollectionDetailedReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtCurrency"];
        //Currency.Text = Session["Denomination"].ToString();


        CRVDemandCollection.ReportSource = rptd;
        CRVDemandCollection.DataBind();
        //}
        //else
        //{
        //    rptd.Load(Server.MapPath("Report/DemandCollectionLineChart.rpt"));
        //    rptd.SetDataSource(GetDemandCollection());
        //    rptd.Subreports["DemandCollectionHeaderReport.rpt"].SetDataSource(GetHeaderdetails());
        //    TextObject Company = (TextObject)rptd.Subreports["DemandCollectionHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtCompany"];
        //    Company.Text = Session["Company"].ToString();
        //    TextObject Date = (TextObject)rptd.Subreports["DemandCollectionHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtDate"];
        //    Date.Text = Session["Date"].ToString();
        //    CRVDemandCollection.ReportSource = rptd;
        //    CRVDemandCollection.DataBind();


        //}

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
    #region Asset Wise D/C
    //private List<ClsPubDemandCollection> GetDemandCollectiondetails()
    //{
    //    List<ClsPubDemandCollection> DemandCollection;

    //    if (Session["Details"] == null)
    //    {
    //        DemandCollection = new List<ClsPubDemandCollection>();
    //    }
    //    else
    //    {
    //        DemandCollection = (List<ClsPubDemandCollection>)Session["Details"];

    //    }

    //    return DemandCollection;

    //}
    #endregion

}
