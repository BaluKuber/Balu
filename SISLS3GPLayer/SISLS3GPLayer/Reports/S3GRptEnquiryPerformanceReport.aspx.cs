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


public partial class Reports_S3GRptEnquiryPerformanceReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {

        ReportDocument rptd = new ReportDocument();
        rptd.Load(Server.MapPath("Report/EnquirySummaryReport.rpt"));
        rptd.SetDataSource(GetLobLoc());
        //TextObject Company = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtcompany"];
        //Company.Text = Session["Company"].ToString();
        //TextObject denomination = (TextObject)rptd.Subreports["EnquiryMainReport.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtdenomination"];
        //denomination.Text = Session["Denomination"].ToString();
        //TextObject Date = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtDate"];
        //Date.Text = Session["Date"].ToString();

        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();
        TextObject Date = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtDate"];
        Date.Text = Session["Date"].ToString();
        TextObject ReportName = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtReportName"];
        ReportName.Text = Session["Title"].ToString();
        TextObject denomination = (TextObject)rptd.Subreports["EnquiryPerformanceReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtdenomination"];
        denomination.Text = Session["Denomination"].ToString();
        TextObject Lob = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtLOB"];
        Lob.Text = Session["lob"].ToString();
        rptd.Subreports["EnquiryPerformanceReport.rpt"].SetDataSource(GetEnqPerformanceDetails());
        rptd.Subreports["EnquiryReceivedReport.rpt"].SetDataSource(GetEnqReceivedDetails());
        rptd.Subreports["EnquirySuccessfulReport.rpt"].SetDataSource(GetEnqSuccessfulDetails());
        rptd.Subreports["EnquiryUnderFollowupReport.rpt"].SetDataSource(GetEnqFollowupDetails());
        rptd.Subreports["EnquiryRejectedReport.rpt"].SetDataSource(GetEnqRejectDetails());
        CRVEnquiry.ReportSource = rptd;
        CRVEnquiry.DataBind();
    }

    private List<ClsPubLobLocation> GetLobLoc()
    {
        ClsPubEnquiryLocation disbursement = new ClsPubEnquiryLocation();
        List<ClsPubLobLocation> lobloc = new List<ClsPubLobLocation>();

        if (Session["lobloc"] != null)
        {
            disbursement = (ClsPubEnquiryLocation)Session["lobloc"];
            lobloc = disbursement.loblocation;
        }
        else
        {
            lobloc = new List<ClsPubLobLocation>();

        }

        return lobloc;

    }

    //private List<ClsPubEnquiryPerformanceDetails> GetEnqPerformanceDetails()
    //{
    //    List<ClsPubEnquiryPerformanceDetails> EnqPerformanceDetails;
    //    if (Session["EnquiryPerformance"] == null)
    //    {
    //        EnqPerformanceDetails = new List<ClsPubEnquiryPerformanceDetails>();
    //    }
    //    else
    //    {
    //        EnqPerformanceDetails = (List<ClsPubEnquiryPerformanceDetails>)Session["EnquiryPerformance"];
    //    }
    //    return EnqPerformanceDetails;
    //}
    private List<ClsPubEnquiryPerformanceDetails> GetEnqPerformanceDetails()
    {
        ClsPubEnquiryLocation EnqLocation = new ClsPubEnquiryLocation();
        List<ClsPubEnquiryPerformanceDetails> EnqPerformanceDetails = new List<ClsPubEnquiryPerformanceDetails>();

        if (Session["EnquiryPerformance"] != null)
        {
            //EnqPerformanceDetails = new List<ClsPubEnquiryPerformanceDetails>();
            EnqLocation = (ClsPubEnquiryLocation)Session["EnquiryPerformance"];
            EnqPerformanceDetails = EnqLocation.Disbursement;
        }
        else
        {
            EnqPerformanceDetails = new List<ClsPubEnquiryPerformanceDetails>();
            // EnqPerformanceDetails = (List<ClsPubEnquiryPerformanceDetails>)Session["EnquiryPerformance"];
        }
        return EnqPerformanceDetails;
    }


    private List<ClsPubEnquiryCount> GetEnqReceivedDetails()
    {
        List<ClsPubEnquiryCount> CountDetails;
        if (Session["Received"] == null)
        {
            CountDetails = new List<ClsPubEnquiryCount>();
        }
        else
        {
            CountDetails = (List<ClsPubEnquiryCount>)Session["Received"];

        }
        return CountDetails;
    }
    private List<ClsPubEnquiryCount> GetEnqSuccessfulDetails()
    {
        List<ClsPubEnquiryCount> SuccessDetails;
        if (Session["Successful"] == null)
        {
            SuccessDetails = new List<ClsPubEnquiryCount>();
        }
        else
        {
            SuccessDetails = (List<ClsPubEnquiryCount>)Session["Successful"];
        }
        return SuccessDetails;
    }

    private List<ClsPubEnquiryCount> GetEnqFollowupDetails()
    {
        List<ClsPubEnquiryCount> FollowupDetails;
        if (Session["Followup"] == null)
        {
            FollowupDetails = new List<ClsPubEnquiryCount>();
        }
        else
        {
            FollowupDetails = (List<ClsPubEnquiryCount>)Session["Followup"];
        }
        return FollowupDetails;
    }

    private List<ClsPubEnquiryCount> GetEnqRejectDetails()
    {
        List<ClsPubEnquiryCount> RejectedDetails;
        if (Session["Rejected"] == null)
        {
            RejectedDetails = new List<ClsPubEnquiryCount>();
        }
        else
        {
            RejectedDetails = (List<ClsPubEnquiryCount>)Session["Rejected"];
        }
        return RejectedDetails;
    }



    //private List<ClsPubEnquiryHeaderDetails> GetEnqHeaderdetails()
    //{
    //    List<ClsPubEnquiryHeaderDetails> Headerdetails = new List<ClsPubEnquiryHeaderDetails>();
    //    if (Session["EnquiryHeader"] != null)
    //    {
    //        ClsPubEnquiryHeaderDetails Headerdetail = (ClsPubEnquiryHeaderDetails)Session["EnquiryHeader"];
    //        Headerdetails.Add(Headerdetail);
    //    }
    //    return Headerdetails;
    //}


}
