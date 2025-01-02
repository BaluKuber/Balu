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

public partial class Reports_S3GDisbursementReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        //if (Convert .ToString ( Session["IsAbstract"]) =="true")
        //{

            rptd.Load(Server.MapPath("Report/Disbursement.rpt"));
            rptd.SetDataSource(GetLobLoc());
            TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcompany"];
            Company.Text = Session["Company"].ToString();
            TextObject header = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtheader"];
            header.Text = "Disbursement Report from " + " " + Session["StartDate"] + " " + "to" +" "+ Session["EndDate"];
            TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtdate"];
            date.Text = Session["Date"].ToString();
            //rptd.Subreports["HeaderDetails.rpt"].SetDataSource(GetHeaderDetails());
            rptd.Subreports["Abstract.rpt"].SetDataSource(GetAbstractDetails());
            TextObject Currency = (TextObject)rptd.Subreports["Abstract.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
            Currency.Text = "[All amounts are in" + " " + Session["Currency"].ToString() + "]";
            rptd.Subreports["Details.rpt"].SetDataSource(GetDetails());
            TextObject Currencys = (TextObject)rptd.Subreports["Details.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
            Currencys.Text = "[All amounts are in" + " " + Session["Currency"].ToString() + "]";
            CRVDisbursementReport.ReportSource = rptd;
            CRVDisbursementReport.DataBind();
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
    private List<ClsPubDisbursementDetails> GetDetails()
    {
        ClsPubDisbursement disbursement = new ClsPubDisbursement();
        List<ClsPubDisbursementDetails> details = new List<ClsPubDisbursementDetails>();
        if (Session["Details"] != null)
        {
            disbursement = (ClsPubDisbursement)Session["Details"];
            details = disbursement.Disbursement;
        }
        else
        {
            details = new List<ClsPubDisbursementDetails>();
            rptd.ReportDefinition.Sections[5].SectionFormat.EnableSuppress = true;

        }

        return details;

    }
    private List<ClsPubHeaderDetails> GetHeaderDetails()
    {
        List<ClsPubHeaderDetails> headerDetails = new List<ClsPubHeaderDetails>();
        if (Session["Header"]!= null)
        {
            ClsPubHeaderDetails headerDetail = (ClsPubHeaderDetails)Session["Header"];
            headerDetails.Add(headerDetail);
        }
        return headerDetails;
    }
    private List<ClsPubLobLocation> GetLobLoc()
    {
        ClsPubDisbursement disbursement = new ClsPubDisbursement();
        List<ClsPubLobLocation> lobloc = new List<ClsPubLobLocation>();

        if (Session["lobloc"] != null)
        {
            disbursement = (ClsPubDisbursement)Session["lobloc"];
            lobloc = disbursement.loblocation; 

        }
        else
        {
            lobloc = new List<ClsPubLobLocation>();

        }

        return lobloc;

    }
    private List<ClsPubDisbursementDetails> GetAbstractDetails()
    {
        ClsPubDisbursement disbursement = new ClsPubDisbursement();
         List<ClsPubDisbursementDetails> abstractdetails = new List<ClsPubDisbursementDetails>();
        if (Session["Abstract"] != null)
        {
            disbursement = (ClsPubDisbursement)Session["Abstract"];
            abstractdetails = disbursement.Disbursement;
            
            //TextObject Currency = (TextObject)rptd.Subreports["AbstractSubreport.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtCurrency"];
            //Currency.Text = Session["Currency"].ToString();
        }
        else
        {
            abstractdetails = new List<ClsPubDisbursementDetails>();
            rptd.ReportDefinition.Sections[4].SectionFormat.EnableSuppress = true;
        }

        return abstractdetails;
    }
   }

