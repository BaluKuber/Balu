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


public partial class Reports_S3GRptJournalQueryReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        //ClsPubHeaderDetails headerDetail = (ClsPubHeaderDetails)Session["Header"];
        if (Session["LOB"].ToString() == "OL")
        {
            rptd.Load(Server.MapPath("Report/JournalQueryReportOL.rpt"));
           rptd.Subreports["JournalQueryHeaderReport.rpt"].SetDataSource(GetHeaderDetails());
           TextObject Company = (TextObject)rptd.Subreports["JournalQueryHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtCompany"];
           Company.Text = Session["Company"].ToString();
           //TextObject Title = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtTitle"];
           //Title.Text = Session["Title"].ToString();
           TextObject Date = (TextObject)rptd.Subreports["JournalQueryHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtDate"];
           Date.Text = Session["Date"].ToString();

            rptd.Subreports["JournalQuerySubReport.rpt"].SetDataSource(GetJournalDetails());
            
            TextObject Denomination = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtDenomination"];
            Denomination.Text = Session["Denomination"].ToString();
            TextObject LOB = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtLOB"];
            LOB.Text = Session["LineofBusiness"].ToString();
            TextObject AccountNo = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtAccNo"];
            AccountNo.Text = Session["AccountNo"].ToString();
            //TextObject SubAccountNo = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtSubAccNo"];
            //SubAccountNo.Text = Session["SubAccountNo"].ToString();
            TextObject GLAccount = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtGLAcc"];
            GLAccount.Text = Session["GLAccount"].ToString();
            //TextObject Lan = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtLan"];
            //Lan.Text = Session["Lan"].ToString();
            CRVJournal.ReportSource = rptd;
            CRVJournal.DataBind();
        }
        else if (Session["LOB"].ToString() == "ALL")
        {
            rptd.Load(Server.MapPath("Report/JournalQueryReport.rpt"));
           rptd.Subreports["JournalQueryHeaderReport.rpt"].SetDataSource(GetHeaderDetails());
           TextObject Company = (TextObject)rptd.Subreports["JournalQueryHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtCompany"];
           Company.Text = Session["Company"].ToString();
           
           TextObject Date = (TextObject)rptd.Subreports["JournalQueryHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtDate"];
           Date.Text = Session["Date"].ToString();

            rptd.Subreports["JournalQuerySubReport.rpt"].SetDataSource(GetJournalDetails());
            //TextObject Company = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtCompany"];
            //Company.Text = Session["Company"].ToString();
            //TextObject Title = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtTitle"];
            //Title.Text = Session["Title"].ToString();
            //TextObject Date = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtDate"];
            //Date.Text = Session["Date"].ToString();
            TextObject Denomination = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtDenomination"];
            Denomination.Text = Session["Denomination"].ToString();
            TextObject LOB = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtLOB"];
            LOB.Text = Session["LineofBusiness"].ToString();
            TextObject AccountNo = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtAccNo"];
            AccountNo.Text = Session["AccountNo"].ToString();
            TextObject SubAccountNo = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtSubAccNo"];
            SubAccountNo.Text = Session["SubAccountNo"].ToString();
            TextObject GLAccount = (TextObject)rptd.Subreports["JournalQuerySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtGLAcc"];
            GLAccount.Text = Session["GLAccount"].ToString();
            CRVJournal.ReportSource = rptd;
            CRVJournal.DataBind();
        }
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
    private List<ClsPubTransaction> GetJournalDetails()
    {
        List<ClsPubTransaction> JournalDetails;

        if (Session["Journal"] == null)
        {
            JournalDetails = new List<ClsPubTransaction>();
        }
        else
        {
            JournalDetails = (List<ClsPubTransaction>)Session["Journal"];
        }
        return JournalDetails;
    }
}
