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

public partial class Reports_S3GRptEntityLedgerReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        rptd.Load(Server.MapPath("Report/VendorDetailsReport.rpt"));
        rptd.Subreports["VendorDetailsHeaderReport.rpt"].SetDataSource(GetHeaderDetails());
        TextObject Company = (TextObject)rptd.Subreports["VendorDetailsHeaderReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtCompany"];
        Company.Text = Session["Company"].ToString();
        TextObject Date = (TextObject)rptd.Subreports["VendorDetailsHeaderReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtDate"];
        Date.Text = Session["Date"].ToString();

        rptd.Subreports["VendorInfoSubReport.rpt"].SetDataSource(GetVendorInfo());
        TextObject EntityName = (TextObject)rptd.Subreports["VendorInfoSubReport.rpt"].ReportDefinition.Sections["Section3"].ReportObjects["txtEntityName"];
        EntityName.Text = Session["EntityName"].ToString();

        rptd.Subreports["VendorDetailsSubReport.rpt"].SetDataSource(GetVendorDetails());
        TextObject Denomination = (TextObject)rptd.Subreports["VendorDetailsSubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtDenomination"];
        Denomination.Text = Session["Denomination"].ToString();

        TextObject OpeningBalance = (TextObject)rptd.Subreports["VendorDetailsSubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtOpeningBalance"];
        OpeningBalance.Text = Session["OpeningBalance"].ToString();
        //TextObject Balance = (TextObject)rptd.Subreports["VendorDetailsSubReport.rpt"].ReportDefinition.Sections["Section4"].ReportObjects["txtBalance"];
        //Balance.Text = Session["Balance"].ToString();
        
        CRVVendor.ReportSource = rptd;
        CRVVendor.DataBind();
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
    private List<ClsPubTransaction> GetVendorDetails()
    {
        List<ClsPubTransaction> VendorDetails;

        if (Session["Vendor"] == null)
        {
            VendorDetails = new List<ClsPubTransaction>();
        }
        else
        {
            VendorDetails = (List<ClsPubTransaction>)Session["Vendor"];
        }
        return VendorDetails;
    }
    private List<ClsPubCustomer> GetVendorInfo()
    {
        List<ClsPubCustomer> VendorInfos = new List<ClsPubCustomer>();

        if (Session["VendorInfo"] != null)
        {
            ClsPubCustomer VendorInfo = (ClsPubCustomer)Session["VendorInfo"];
            VendorInfos.Add(VendorInfo);
        }
        return VendorInfos;
    }

}
