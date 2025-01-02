using System;
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

public partial class Reports_PDCAcknowledgementReport : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //if (Session["PDCAcknow"] != null)
        //{
            ReportDocument Rptd = new ReportDocument();
            Rptd.Load(Server.MapPath("Report/PDCAcknowledgementRPT.rpt"));

            TextObject Companyname = (TextObject)Rptd.ReportDefinition.Sections["Section3"].ReportObjects["txtCompanyname"];
            Companyname.Text = Session["Companyname"].ToString();

            TextObject Currency = (TextObject)Rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcurrency"];
            Currency.Text = "[All Amounts are in" + " " + Session["Currency"].ToString() + "]";

            //TextObject Date = (TextObject)Rptd.Subreports["HeaderDetails.rpt"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtdate"];
            //Date.Text = Session["Date"].ToString();

            //TextObject Currency = (TextObject)Rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcurrency"];
            //Currency.Text = "[All Amounts are in" + " " + Session["Currency"].ToString() + "]";

            Rptd.SetDataSource(GetPDCDetails());
            Rptd.Subreports[0].SetDataSource(GetHeaderDetails());

            TextObject Date = (TextObject)Rptd.Subreports["HeaderDetails"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtdate"];
            Date.Text = Session["Date"].ToString();

            CRVPDCAcknowledgement.ReportSource = Rptd;
            CRVPDCAcknowledgement.DataBind();
        //}
    }

    private List<ClsPubPDCDetails> GetPDCDetails()
    {
        List<ClsPubPDCDetails> PDCDetails;

        if (Session["PDCAcknow"] == null)
        {
            PDCDetails = new List<ClsPubPDCDetails>();
        }
        else
        {
            PDCDetails = (List<ClsPubPDCDetails>)Session["PDCAcknow"];
        }
        return PDCDetails;
    }

    private List<ClsPubHeaderDetails> GetHeaderDetails()
    {
        List<ClsPubHeaderDetails> PDCHeader = new List<ClsPubHeaderDetails>();
        if (Session["PDCHeader"] != null)
        {
            ClsPubHeaderDetails PDCHead = (ClsPubHeaderDetails)Session["PDCHeader"];
            PDCHeader.Add(PDCHead);
        }
        return PDCHeader;
    }
}