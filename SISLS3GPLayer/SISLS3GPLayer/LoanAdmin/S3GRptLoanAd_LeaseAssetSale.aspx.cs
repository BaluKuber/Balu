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

public partial class LoanAdmin_S3GRptLoanAd_LeaseAssetSale : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        if (rptd != null)
        {
            rptd.Close();
            rptd.Dispose();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        //if (Convert .ToString ( Session["IsAbstract"]) =="true")
        //{

        rptd.Load(Server.MapPath("S3GLoanAdLeaseAssetSale.rpt"));
        rptd.SetDataSource((DataTable)Session["AssetInfo"]);
        rptd.Subreports["CustomerDetailsReport"].SetDataSource((DataTable)Session["AssetInfo"]);
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();
        TextObject Company_Footer = (TextObject)rptd.ReportDefinition.Sections["Section3"].ReportObjects["txtCompanyFooter"];
        Company_Footer.Text = Session["Company"].ToString();
        //TextObject header = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtheader"];
        //header.Text = "Lease Asset Sale Report";
        TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtdate"];
        date.Text = Session["Date"].ToString();       



        //rptd.Subreports["HeaderDetails.rpt"].SetDataSource(GetHeaderDetails());


        //Currency.Text = "[All amounts are in" + " " + Session["Currency"].ToString() + "]";

        //TextObject Currencys = (TextObject)rptd.Subreports["Details.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
        //Currencys.Text = "[All amounts are in" + " " + Session["Currency"].ToString() + "]";
        CRVLeaseAssetSaleReport.ReportSource = rptd;
        CRVLeaseAssetSaleReport.DataBind();




        //}
        //else
        //{
        //    rptd.Load(Server.MapPath("Report/Details.rpt"));
        //    TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtcompany"];
        //    Company.Text = Session["Company"].ToString();
        //    TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtdate"];
        //    date.Text = Session["Date"].ToString();
        //    rptd.Subreports["HeaderDetails.rpt"].SetDataSource(GetHeaderDetails());
        //    rptd.Subreports["Details.rpt"].SetDataSource(GetDetails());
        //    TextObject Currencys = (TextObject)rptd.Subreports["Details.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
        //    Currencys.Text = "[All amounts are in" + " " + Session["Currency"]+"]";
        //    CRVDisbursementReport.ReportSource = rptd;
        //    CRVDisbursementReport.DataBind();
        //}

       
    }

}
