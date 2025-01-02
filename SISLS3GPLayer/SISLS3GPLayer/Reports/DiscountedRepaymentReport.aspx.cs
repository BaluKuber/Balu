using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
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
using System.Data;

public partial class Reports_DiscountedRepaymentReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {

        if (Session["Repay"] != null)
        {


        rptd.Load(Server.MapPath("Report/DiscountedRepayment.rpt"));
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();
        //TextObject Heading = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtHeading"];
        //Heading.Text = Session["Heading"].ToString();
        TextObject Date = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtDate"];
        Date.Text = Session["Date"].ToString();
        TextObject Money = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtMoney"];
        Money.Text = Session["AccountingCurrency"].ToString();
        TextObject Denomination = (TextObject)rptd.ReportDefinition.Sections["PageHeaderSection1"].ReportObjects["txtdenomination"];
        Denomination.Text = Session["AccountingCurrency"].ToString();

        if (Session["Type"] != null)
        {
            if (Session["Type"].ToString() == "R")
            {
                //rptd.SetDataSource(GetRepaymentDetails());
                rptd.ReportDefinition.Sections[2].SectionFormat.EnableSuppress = true;
                rptd.ReportDefinition.Sections[4].SectionFormat.EnableSuppress = true;
                rptd.ReportDefinition.Sections[6].SectionFormat.EnableSuppress = true;
            }
            
        }
        else
        {
            //rptd.SetDataSource(GetRepayDetails());
            rptd.ReportDefinition.Sections[3].SectionFormat.EnableSuppress = true;
            rptd.ReportDefinition.Sections[5].SectionFormat.EnableSuppress = true;
            rptd.ReportDefinition.Sections[7].SectionFormat.EnableSuppress = true;
        }

        rptd.SetDataSource(GetRepDetails());
        rptd.Subreports["HeaderDetailsSubReport.rpt"].SetDataSource(GetHeaderDetails());
        //TextObject FinAmount = (TextObject)rptd.Subreports["HeaderDetailsSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtFinAmount"];
        //FinAmount.Text = Session["FinAmt"].ToString();
        //TextObject Terms = (TextObject)rptd.Subreports["HeaderDetailsSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtTerms"];
        //Terms.Text = Session["Terms"].ToString();

        //TextObject IRR = (TextObject)rptd.Subreports["HeaderDetailsSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtIRR"];
        //IRR.Text = Session["IRR"].ToString();

        rptd.Subreports["CustomerNameSubReport.rpt"].SetDataSource(GetCustomerDetails());

        //if (Session["IsAssetPrintOff"].ToString() == "0")
        //{
        //    rptd.Subreports["AssetDetailsSubReport.rpt"].SetDataSource(GetAssetDetails(false));
        //    //TextObject Currency = (TextObject)rptd.Subreports["AssetDetailsSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtCurrency"];
        //    //Currency.Text = Session["AccountingCurrency"].ToString();
        //}
        //else
        //{
        //    rptd.Subreports["AssetDetailsSubReport.rpt"].SetDataSource(GetAssetDetails(true));
        //    //TextObject Currency = (TextObject)rptd.Subreports["AssetDetailsSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtCurrency"];
        //    //Currency.Text = Session["AccountingCurrency"].ToString();

        //}

        CRVRepayment.ReportSource = rptd;
        CRVRepayment.DataBind();
        //rptd.Close();
        //rptd.Dispose();
        }

    }

    //private List<ClsPubAssestDetails> GetAssetDetails(bool IsEmptyData)
    //{
    //    List<ClsPubAssestDetails> Assetdetails;
    //    if (Session["Asset"] == null || IsEmptyData)
    //    {
    //        Assetdetails = new List<ClsPubAssestDetails>();
    //        rptd.ReportDefinition.Sections[1].SectionFormat.EnableSuppress = true;
    //    }
    //    else
    //    {
    //        Assetdetails = (List<ClsPubAssestDetails>)Session["Asset"];
    //    }
    //    return Assetdetails;
    //}
    //private List<ClsPubRepayDetails> GetRepayDetails()
    //{
    //    List<ClsPubRepayDetails> repayDetails;

    //    if (Session["Repay"] == null)
    //    {
    //        repayDetails = new List<ClsPubRepayDetails>();
    //    }
    //    else
    //    {
    //        repayDetails = (List<ClsPubRepayDetails>)Session["Repay"];
    //    }
    //    return repayDetails;
    //}

    private DataTable GetRepDetails()
    {
        DataTable dtRep = new DataTable();
        if (Session["Repay"] != null)
        {
            dtRep = (DataTable)Session["Repay"];

        }

        else
        {
            dtRep = null;
        }
        return dtRep;
    }
    //private List<ClsPubRepayDetails> GetRepaymentDetails()
    //{
    //    List<ClsPubRepayDetails> repaymentDetails;

    //    if (Session["Repay"] == null)
    //    {
    //        repaymentDetails = new List<ClsPubRepayDetails>();
    //    }
    //    else
    //    {
    //        repaymentDetails = (List<ClsPubRepayDetails>)Session["Repay"];
    //    }
    //    return repaymentDetails;
    //}

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
    private List<ClsPubCustomer> GetCustomerDetails()
    {
        List<ClsPubCustomer> CustomerDetails = new List<ClsPubCustomer>();

        if (Session["CustomerInfo"] != null)
        {
            ClsPubCustomer CustomerDetail = (ClsPubCustomer)Session["CustomerInfo"];
            CustomerDetails.Add(CustomerDetail);
        }
        return CustomerDetails;
    }

}
