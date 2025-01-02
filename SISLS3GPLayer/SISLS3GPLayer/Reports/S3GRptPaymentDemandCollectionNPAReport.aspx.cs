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


public partial class Reports_S3GRptPaymentDemandCollectionNPAReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {

        ClsPubHeaderDetails Headerdetail = (ClsPubHeaderDetails)Session["Header"];
        ReportDocument rptd = new ReportDocument();
        rptd.Load(Server.MapPath("Report/PaymentDemandCollectionNPAReport.rpt"));
        rptd.SetDataSource(GetDemandCollection());

        TextObject denomination = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtDenomination"];
        denomination.Text = Session["Denomination"].ToString();
        TextObject Stock = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtStock"];
        Stock.Text = Session["LOB"].ToString();
        //TextObject Product = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtProduct"];
        //Product.Text = Session["Product"].ToString();
        rptd.Subreports["PaymentDemandCollectionNPAHeaderReport"].SetDataSource(GetHeaderdetails());
        TextObject Company = (TextObject)rptd.Subreports["PaymentDemandCollectionNPAHeaderReport"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtCompany"];
        Company.Text = Session["Company"].ToString();
        TextObject Date = (TextObject)rptd.Subreports["PaymentDemandCollectionNPAHeaderReport"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtDate"];
        Date.Text = Session["DateTime"].ToString();
        if (Session["Note"] != null)
        {
            rptd.ReportDefinition.Sections[11].SectionFormat.EnableSuppress = false;
            TextObject Note = (TextObject)rptd.ReportDefinition.Sections["Section4"].ReportObjects["txtErrorMsg"];
            Note.Text = Session["Note"].ToString();

        }
        else
        {
            rptd.ReportDefinition.Sections[11].SectionFormat.EnableSuppress = true;
        }
        
        CRVPaymentDemandCollectionNPA.ReportSource = rptd;
        CRVPaymentDemandCollectionNPA.DataBind();

    }
    private List<ClsPubPaymentDCNPADetails> GetDemandCollection()
    {
        List<ClsPubPaymentDCNPADetails> DemandCollection;

        if (Session["PaymentDCNPA"] == null)
        {
            DemandCollection = new List<ClsPubPaymentDCNPADetails>();
        }
        else
        {
            DemandCollection = (List<ClsPubPaymentDCNPADetails>)Session["PaymentDCNPA"];

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
