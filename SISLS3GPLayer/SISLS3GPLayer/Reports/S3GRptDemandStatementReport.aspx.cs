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


public partial class Reports_S3GRptDemandStatementReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        ReportDocument rptd = new ReportDocument();
        rptd.Load(Server.MapPath("Report/DemandStatement.rpt"));
        //rptd.SetDataSource(GetLob());
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();
        TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtdate"];
        date.Text = Session["Date"].ToString();
        TextObject cutoffmonth = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcutoffmonth"];
        cutoffmonth.Text = "Demand Statement Report For" + " " + Session["Cutoff"].ToString();
        //rptd.Subreports["Header"].SetDataSource(GetHeaderDetails());
        rptd.Subreports["Demand"].SetDataSource(GetDetails());
        TextObject Currency = (TextObject)rptd.Subreports["Demand"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtCurrency"];
        Currency.Text = "[All amounts are in" + " " + Session["Currency"].ToString()+"]";
        //TextObject category = (TextObject)rptd.Subreports["Demand"].ReportDefinition.Sections["GroupHeaderSection3"].ReportObjects["txtcategory"];
        //category.Text = Session["category"].ToString();
        //TextObject Debtcode = (TextObject)rptd.Subreports["Demand"].ReportDefinition.Sections["GroupHeaderSection3"].ReportObjects["txtdebtcode"];
        //Debtcode.Text = Session["DCcode"].ToString();
        //TextObject ageing = (TextObject)rptd.Subreports["Demand"].ReportDefinition.Sections["GroupHeaderSection3"].ReportObjects["txtageing"];
        //ageing.Text = Session["aging"].ToString();

        CRVDemandStatement.ReportSource = rptd;
        CRVDemandStatement.DataBind();
    }
    private List<ClsPubHeaderDetails> GetHeaderDetails()
    {
        List<ClsPubHeaderDetails> headerDetails = new List<ClsPubHeaderDetails>();
        if (Session["Header"] != null)
        {
            ClsPubHeaderDetails headerDetail = (ClsPubHeaderDetails)Session["Header"];
            headerDetails.Add(headerDetail);
        }
        return headerDetails;
    }

    private List<ClsPubDemandStatement> GetDetails()
    {

        List<ClsPubDemandStatement> demand = new List<ClsPubDemandStatement>();
        if (Session["Demand"] == null)
        {
            demand = new List<ClsPubDemandStatement>();
            
            //TextObject Currency = (TextObject)rptd.Subreports["AbstractSubreport.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
            //Currency.Text = Session["Currency"].ToString();
        }
        else
        {

            demand = (List<ClsPubDemandStatement>)Session["Demand"];
         
        }

        return demand;
    }


    //private List<ClsPubDemandStatement> GetLob()
    //{

    //    List<ClsPubDemandStatement> lob = new List<ClsPubDemandStatement>();
    //    if (Session["Demand"] == null)
    //    {
    //        demand = new List<ClsPubDemandStatement>();

    //        //TextObject Currency = (TextObject)rptd.Subreports["AbstractSubreport.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
    //        //Currency.Text = Session["Currency"].ToString();
    //    }
    //    else
    //    {

    //        demand = (List<ClsPubDemandStatement>)Session["Demand"];

    //    }

    //    return demand;
    //}
}
