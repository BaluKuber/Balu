
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
using S3GBusEntity;
using System.Collections.Generic;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class Collection_CustomerAtaGlanceReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
     {
        try
        {
            S3GSession ObjS3GSession = new S3GSession();
            List<ClsPubCustomer> customer = (List<ClsPubCustomer>)Session["CustomerName"];
            List<ClsPubCustomerGlanceDetails> customerglancedetails = (List<ClsPubCustomerGlanceDetails>)Session["CustomerGlanceDetails"];            
            ClsPubSOAAsset assets=(ClsPubSOAAsset)Session["Assets"];            
            ReportDocument rptd = new ReportDocument();
            rptd.Load(Server.MapPath(@"Report\CustomerAtAGlanceReport.rpt"));
            rptd.SetDataSource(assets.Openingbalance);
            TextObject T9 = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtReportDate"];
            T9.Text = Session["Date"].ToString();
            TextObject T2 = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtReportTitle"];
            T2.Text = Session["Title"].ToString();
            TextObject T4 = (TextObject)rptd.Subreports["CustomerAtAGlanceDetailsSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["TxtCurrency"];
            T4.Text = Session["Currency"].ToString();
            TextObject T5 = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["TxtCompanyName"];
            T5.Text = Session["CompanyName"].ToString();
            FieldObject F2 = (FieldObject)rptd.Subreports["CustomerAtAGlanceDetailsSubReport.rpt"].ReportDefinition.Sections["Section3"].ReportObjects["AppliedAmt1"];
            F2.ObjectFormat.EnableCanGrow = true;
            TextObject openingbalance = (TextObject)rptd.ReportDefinition.Sections["DetailSection4"].ReportObjects["txtOpeningBalance"];
            openingbalance.Text = "Opening Balance as on  " + Session["Startdate"] + " " + "is";
            rptd.Subreports["CustomerAtAGlanceDetailsSubReport.rpt"].SetDataSource(customerglancedetails);
            rptd.Subreports["CAGAsset.rpt"].SetDataSource(assets.AssetDetails);
            rptd.Subreports["AssetAccounts.rpt"].SetDataSource(assets.AccountDetails);
            if (assets.Transaction.Count != 0)
            {
                rptd.Subreports["TransactionDetails.rpt"].SetDataSource(assets.Transaction);
            }
            else
            {
                //System.Data.DataTable dt = new System.Data.DataTable();
                //dt = null;
                //rptd.Subreports["TransactionDetails.rpt"].SetDataSource(dt);
                List<ClsPubTransaction> obj = new List<ClsPubTransaction>();
                ClsPubTransaction obj1 = new ClsPubTransaction();
                obj1.PrimeAccountNo = "0";
                obj1.SubAccountNo = "0";
                obj.Add(obj1);
                rptd.Subreports["TransactionDetails.rpt"].SetDataSource(obj); 
            }
            rptd.Subreports["CustomerNameSubReport.rpt"].SetDataSource(customer);
            CRVCustomerAtaGlance.ReportSource = rptd;
            CRVCustomerAtaGlance.DataBind();      
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
        }
    }
}

