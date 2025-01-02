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

public partial class Reports_S3gOrgPricingReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        //if (Convert .ToString ( Session["IsAbstract"]) =="true")
        //{        
        rptd.Load(Server.MapPath(@"Report\S3GOrgPricingReport.rpt"));
        rptd.SetDataSource(GetRptDet());
        rptd.Subreports["CheckListForHPProposal"].SetDataSource(GetRptCheckListDet()); // 2 b edited
        rptd.Subreports["RepaymentStructureForPricing"].SetDataSource(GetRptRepaymentDet()); // 2 b edited
        //TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcompany"];
        //Company.Text = Session["Company"].ToString();
        TextObject header = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtheader"];
        header.Text = "PRICING OFFER";
        TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtdate"];
        date.Text = Session["Date"].ToString();
        //rptd.Subreports["HeaderDetails.rpt"].SetDataSource(GetHeaderDetails());


        //Currency.Text = "[All amounts are in" + " " + Session["Currency"].ToString() + "]";
        
        //TextObject Currencys = (TextObject)rptd.Subreports["Details.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
        //Currencys.Text = "[All amounts are in" + " " + Session["Currency"].ToString() + "]";
        CRVPricingReport.ReportSource = rptd;
        CRVPricingReport.DataBind();
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


    private DataTable GetRptDet()
    {
        DataTable dtReport = new DataTable();        

        if (Session["Report_Pricing"] != null)
        {
            dtReport = (DataTable)Session["Report_Pricing"];            
        }
        else
        {
            dtReport = null;

        }

        return dtReport;

    }


    private DataTable GetRptCheckListDet()
    {        
        DataTable dtCheckList = new DataTable();

        if (Session["CheckList"] != null)
        {            
            dtCheckList = (DataTable)Session["CheckList"];
        }
        else
        {
            dtCheckList = null;

        }

        return dtCheckList;

    }


    private DataTable GetRptRepaymentDet()
    {
        DataTable dtRepaymentDet = new DataTable();

        if (Session["Repayment_Structure"] != null)
        {
            dtRepaymentDet = (DataTable)Session["Repayment_Structure"];
        }
        else
        {
            dtRepaymentDet = null;

        }
        dtRepaymentDet.TableName = "command";
        return dtRepaymentDet;

    }
    

}
