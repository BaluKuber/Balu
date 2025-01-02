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
using System.Collections.Generic;
using S3GBusEntity.Reports;
//using System.Web.UI.MobileControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class Reports_S3GRptCreditScoreReport : System.Web.UI.Page
{
    private ReportDocument Rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        
        //ClsPubHeaderDetails HeaderDetail = (ClsPubHeaderDetails)Session["Header1"];

        //ReportDocument Rptd = new ReportDocument();
        
        Rptd.Load(Server.MapPath("Report/CreditScoreReport.rpt"));
        Rptd.SetDataSource(GetLocCodCat());
        TextObject Company = (TextObject)Rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();

        TextObject Currency = (TextObject)Rptd.Subreports["CreditCustomers.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtcurrency"];
        Currency.Text = "[All Amounts are in" + " " + Session["Currency"].ToString()+"]";

        TextObject Creditdate = (TextObject)Rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtDate"];
        Creditdate.Text = Session["Date"].ToString();

        TextObject header = (TextObject)Rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtheader"];
        header.Text = "Credit Score Transaction Report from" + " " + Session["StartDate"] + " " + "to" +" "+ Session["EndDate"];

        TextObject LOB = (TextObject)Rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtlob"];
        LOB.Text = Session["LOB"].ToString();

        TextObject PROD = (TextObject)Rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtproduct"];
        PROD.Text = Session["PROD"].ToString();

        //TextObject txtCustDetails = (TextObject)Rptd.Subreports["CreditCustomers.rpt"].ReportDefinition.Sections["Section1"].ReportObjects["txtar"];
        //txtCustDetails.Text = "";
        //txtCustDetails.Text ="Customer Details : "+ Session["strAcc_Rej"].ToString();

        //TextObject Title = (TextObject)Rptd.Subreports["CreditScore.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txttitle"];
        //Title.Text = Session["Title"].ToString();

        Rptd.Subreports["CreditCustomers.rpt"].SetDataSource(GetCustomerDetails());
        Rptd.Subreports["CustomersDetailsRej.rpt"].SetDataSource(GetCustomerDetailsRej());
        //Rptd.Subreports["Header.rpt"].SetDataSource(GetHeaderDetails());
        Rptd.Subreports["CreditScore.rpt"].SetDataSource(GetCreditScoreDetails());

        CrystalReportViewer1.ReportSource = Rptd;
        CrystalReportViewer1.DataBind();
    }

    private List<ClsPubCustomersDetails> GetCustomerDetails()
    {
        List<ClsPubCustomersDetails> CustomersDetails;
        if (Session["SESSION_CRPT_2"] == null)
        {
            CustomersDetails = new List<ClsPubCustomersDetails>();
        }
        else
        {
            CustomersDetails = (List<ClsPubCustomersDetails>)Session["SESSION_CRPT_2"];
        }
        return CustomersDetails;
    }
    private List<ClsPubCustomersDetails> GetCustomerDetailsRej()
    {
        List<ClsPubCustomersDetails> CustomersDetailsRejt;
        if (Session["SESSION_CRPT_3"] == null)
        {
            CustomersDetailsRejt = new List<ClsPubCustomersDetails>();
        }
        else
        {
            CustomersDetailsRejt = (List<ClsPubCustomersDetails>)Session["SESSION_CRPT_3"];
        }
        return CustomersDetailsRejt;
    }
    //private List<ClsPubHeaderDetails> GetHeaderDetails()
    //{
    //    List<ClsPubHeaderDetails> HeaderDetails = new List<ClsPubHeaderDetails>();
    //    if (Session["Header1"] != null)
    //    {
    //        ClsPubHeaderDetails HeaderDetail = (ClsPubHeaderDetails)Session["Header1"];
    //        HeaderDetails.Add(HeaderDetail);
    //    }
    //    return HeaderDetails;
    //}

    private List<ClsPubLocationCodeCategory> GetLocCodCat()
    {
        ClsPubCreditScoreTransaction CreditScoreDetails = new ClsPubCreditScoreTransaction();
        List<ClsPubLocationCodeCategory> LocCodCat = new List<ClsPubLocationCodeCategory>();
        if (Session["LOC"] != null)
        {
            CreditScoreDetails = (ClsPubCreditScoreTransaction)Session["LOC"];
            LocCodCat = CreditScoreDetails.Creditlocation;
        }
        else
        {
            LocCodCat = new List<ClsPubLocationCodeCategory>();
        }
        return LocCodCat;
    }
    private List<ClsPubCreditScoreDetails> GetCreditScoreDetails()
    {
        ClsPubCreditScoreTransaction CreditScoreDetails = new ClsPubCreditScoreTransaction();
        List<ClsPubCreditScoreDetails> CreditScoreDetailsgrid = new List<ClsPubCreditScoreDetails>();
        if (Session["Credit"] != null)
        {
            CreditScoreDetails = (ClsPubCreditScoreTransaction)Session["Credit"];
            CreditScoreDetailsgrid = CreditScoreDetails.CreditScoreTrans;
            //CreditScoreDetails = new List<ClsPubCreditScoreDetails>();
            //Rptd.ReportDefinition.Sections[0].SectionFormat.EnableSuppress = true; 
        }
        else
        {
            CreditScoreDetailsgrid = new List<ClsPubCreditScoreDetails>();
        }
        return CreditScoreDetailsgrid;
    }    

}
