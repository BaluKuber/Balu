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

public partial class Reports_S3GRptSanctionDetailsReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        ClsPubHeaderDetails Headerdetail = (ClsPubHeaderDetails)Session["Header"];
        if (Session["IsSummary"].ToString() == "TRUE")
        {
            rptd.Load(Server.MapPath("Report/SanctionDetailsSummaryReport.rpt"));
            rptd.Subreports["SanctionDetailsSummarySubReport.rpt"].SetDataSource(GetSummaryDetails());
            TextObject denomination = (TextObject)rptd.Subreports["SanctionDetailsSummarySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtdenomination"];
            denomination.Text = Session["Denomination"].ToString();
            TextObject Lob = (TextObject)rptd.Subreports["SanctionDetailsSummarySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtLOB"];
            Lob.Text = Session["lob"].ToString();
            rptd.Subreports["SanctionDetailsHeaderReport.rpt"].SetDataSource(GetHeaderdetails());
            TextObject Company = (TextObject)rptd.Subreports["SanctionDetailsHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtCompany"];
            Company.Text = Session["Company"].ToString();
            TextObject Date = (TextObject)rptd.Subreports["SanctionDetailsHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtDate"];
            Date.Text = Session["Date"].ToString();
            //TextObject Title = (TextObject)rptd.Subreports["SanctionDetailsSummarySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtTitle"];
            //Title.Text = Session["Title"].ToString();
            //TextObject Location2 = (TextObject)rptd.Subreports["SanctionDetailsSummarySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtLocation2"];
            //Location2.Text = Session["Location2"].ToString();
            TextObject Product = (TextObject)rptd.Subreports["SanctionDetailsSummarySubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtProduct"];
            Product.Text = Session["Product"].ToString();
            CRVSanctoin.ReportSource = rptd;
            CRVSanctoin.DataBind();
        }
        else
        {
            rptd.Load(Server.MapPath("Report/SanctionDetailReport.rpt"));
            rptd.SetDataSource(GetMainDetails());
            TextObject Currency = (TextObject)rptd.ReportDefinition.Sections["PageHeaderSection1"].ReportObjects["txtMoney"];
            Currency.Text = Session["Denomination"].ToString();
            TextObject Disbursable = (TextObject)rptd.ReportDefinition.Sections["Section4"].ReportObjects["txtDisbursableAmt"];
            Disbursable.Text = Session["DisbursableAmt"].ToString();
            TextObject Disbursed = (TextObject)rptd.ReportDefinition.Sections["Section4"].ReportObjects["txtDisbursedAmt"];
            Disbursed.Text = Session["DisbursedAmt"].ToString();
            rptd.Subreports["SanctionDetailsHeaderReport.rpt"].SetDataSource(GetHeaderdetails());
            TextObject Company = (TextObject)rptd.Subreports["SanctionDetailsHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtCompany"];
            Company.Text = Session["Company"].ToString();
            TextObject Date = (TextObject)rptd.Subreports["SanctionDetailsHeaderReport.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtDate"];
            Date.Text = Session["Date"].ToString();
            TextObject Location2 = (TextObject)rptd.Subreports["SanctionDetailsHeaderReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtLocation2"];
            Location2.Text = Session["Location2"].ToString();

            rptd.Subreports["Disbursable"].SetDataSource(GetDisbursable());
            rptd.Subreports["Disbursed"].SetDataSource(GetDisbursed());
            CRVSanctoin.ReportSource = rptd;
            CRVSanctoin.DataBind();

        }
    }
    private List<ClsPubSanctionDetails> GetSummaryDetails()
    {
        List<ClsPubSanctionDetails> Sanctiondetails;

        if (Session["Summary"] == null)
        {
            Sanctiondetails = new List<ClsPubSanctionDetails>();

        }
        else
        {
            Sanctiondetails = (List<ClsPubSanctionDetails>)Session["Summary"];

        }
        return Sanctiondetails;
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
    private List<ClsPubSanctionDetails> GetMainDetails()
    {
        List<ClsPubSanctionDetails> Sanction;

        if (Session["Sanction"] == null)
        {
            Sanction = new List<ClsPubSanctionDetails>();
        }
        else
        {
            Sanction = (List<ClsPubSanctionDetails>)Session["Sanction"];

        }
        return Sanction;
    }
    private List<ClsPubSanctionDisbursableDetails> GetDisbursable()
    {
        List<ClsPubSanctionDisbursableDetails> Disbursable;

        if (Session["Disbursable"] == null)
        {
            Disbursable = new List<ClsPubSanctionDisbursableDetails>();
            rptd.ReportDefinition.Sections[4].SectionFormat.EnableSuppress = true;
        }
        else
        {
            Disbursable = (List<ClsPubSanctionDisbursableDetails>)Session["Disbursable"];
            if (Disbursable.Count == 0)
            {
                ClsPubSanctionDisbursableDetails cls = new ClsPubSanctionDisbursableDetails();
                cls.APPLICATION_PROCESS_ID = 0;
                cls.DISBURSABLE_DATE = "0";
                cls.DISBURSABLE_AMOUNT = 0;

            }

        }
        return Disbursable;
    }
    private List<ClsPubSanctionDisbursedDetails> GetDisbursed()
    {
        List<ClsPubSanctionDisbursedDetails> Disbursed;

        if (Session["Disbursed"] == null)
        {
            Disbursed = new List<ClsPubSanctionDisbursedDetails>();
            rptd.ReportDefinition.Sections[5].SectionFormat.EnableSuppress = true;
        }
        else
        {
            Disbursed = (List<ClsPubSanctionDisbursedDetails>)Session["Disbursed"];

            if (Disbursed.Count == 0)
            {
                ClsPubSanctionDisbursedDetails clstest = new ClsPubSanctionDisbursedDetails();
                clstest.APPLICATION_PROCESS_ID = 0;
                clstest.DISBURSED_AMOUNT = 0;
                clstest.DISBURSED_NO = "";
                clstest.DISBURSED_DATE = "";
                Disbursed.Add(clstest);
            }

        }
        return Disbursed;
    }
    

}
