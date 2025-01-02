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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class Reports_S3GStatementOfAccountsReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {

     
        
            ClsPubCustomer cust = (ClsPubCustomer)Session["Customer"];
            
            //List<ClsPubTransaction> trans=(List<ClsPubTransaction>)Session["Transaction"];
            
            rptd.Load(Server.MapPath("Report/TransactionDetailsReport.rpt"));
            rptd.SetDataSource(GetOpeningBalance());
            TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtcompany"];
            Company.Text = Session["Company"].ToString();
            TextObject name = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtname"];
            name.Text = "Statement of Account for the period" + " " +  Session["Startdate"] + " " + "to"+" "+ Session["Enddate"];
            TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtdate"];
            date.Text = Session["Date"].ToString();
            
            TextObject Customer = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcustomer"];
            Customer.Text = Session["CustomerCode"].ToString();

            //TextObject Currency1 = (TextObject)rptd.ReportDefinition.Sections["Section3"].ReportObjects["txtCurrency"];
            //Currency1.Text = Session["Currency"].ToString();
            rptd.Subreports["TransactionDetails"].SetDataSource(GetTransactionDetails());
            rptd.Subreports["AccountDts"].SetDataSource(GetAccountDetails());
            rptd.Subreports["SummaryAccount.rpt"].SetDataSource(GetSummaryDetails());
            rptd.Subreports["MemoAccount.rpt"].SetDataSource(GetMemoDetails());
            TextObject openingbalance = (TextObject)rptd.Subreports["TransactionDetails"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtopeningbalance"];
            openingbalance.Text = "Opening Balance";// as on  " + " " + Session["Startdate"] + " " + "is";
            TextObject Currency1 = (TextObject)rptd.Subreports["TransactionDetails"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtCurrency"];
            Currency1.Text = "[All amounts are in"+" "+ Session["Currency"].ToString()+"]";
            TextObject due = (TextObject)rptd.Subreports["SummaryAccount.rpt"].ReportDefinition.Sections["Section3"].ReportObjects["txtInstallment"];
            due.Text = Session["due"].ToString();
           

            
            //TextObject to = (TextObject)rptd.Subreports["TransactionDetails"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtOpeningBalance"];
            //to.Text = Session["OpeningBalance"].ToString();

            rptd.Subreports["CustomerNameSubReport.rpt"].SetDataSource(GetCustomerDetails());
            if (Session["IsAssetPrintOff"].ToString() == "0")
            {
                rptd.Subreports["Assets.rpt"].SetDataSource(GetAssetDetails(false));
                rptd.Subreports["Terms.rpt"].SetDataSource(GetAssetDetails(false));

               
            }
            else
            {
                rptd.Subreports["Assets.rpt"].SetDataSource(GetAssetDetails(true));
                rptd.Subreports["Terms.rpt"].SetDataSource(GetAssetDetails(false));
              
            }

            //List<ClsPubAsset> Asset = (List<ClsPubAsset>)Session["Assets"];
            //rptd.Subreports["AssetReport.rpt"].SetDataSource(Asset);

            //List<ClsPubPASA> PASA = (List<ClsPubPASA>)Session["PASA"];
            //rptd.Subreports["PrimeAccountSubAccountReport.rpt"].SetDataSource(PASA);
            //ClsPubSummary summary = (ClsPubSummary)Session["Summary"];
            // //summary.Total = summary.InstallmentDues + summary.InsuranceDues + summary.InterestDues + summary.ODIDues + summary.OtherDues;
            //List<ClsPubSummary> summarys = new List<ClsPubSummary>();
            //summarys.Add(summary);
            //rptd.Subreports["SummaryReport.rpt"].SetDataSource(summarys);
            //TextObject sum = (TextObject)rptd.Subreports["SummaryReport.rpt"].ReportDefinition.Sections["Section4"].ReportObjects["txtsum"];
            //sum.Text = Session["Sum"].ToString();

            //ClsPubMemorandum memo=(ClsPubMemorandum)Session["Memorandum"];
            //List<ClsPubMemorandum> memos=new List<ClsPubMemorandum>();
            //memos.Add(memo);
            //rptd.Subreports["MemorandumReport.rpt"].SetDataSource(memos);
            
            //Opening Balance

            //ParameterValues myvals = new ParameterValues();
            //ParameterDiscreteValue myDiscrete = new ParameterDiscreteValue();
            //myDiscrete.Value = Session["OpenBal"].ToString();
            //myvals.Add(myDiscrete);
            //rptd.DataDefinition.ParameterFields["OpenBal"].ApplyCurrentValues(myvals);



            CRVStatementOfAccounts.ReportSource=rptd;     
            CRVStatementOfAccounts.DataBind();          
                      
       

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
     private List<ClsPubAsset> GetAccountDetails()
     {
         ClsPubSOAAsset assetaccountdetails;
         List<ClsPubAsset> Assetdetails;
         if (Session["Assets"] == null)
         {
             Assetdetails = new List<ClsPubAsset>();
         }
         else
         {
             assetaccountdetails = (ClsPubSOAAsset)Session["Assets"];
             Assetdetails = assetaccountdetails.AccountDetails;

             //TextObject Currency = (TextObject)rptd.Subreports["Accounts.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtCurrency"];
             //Currency.Text = "All amounts are in" + " " + Session["Currency"].ToString();
         }
         if (Assetdetails.Count == 0)
         {
             ClsPubAsset obj = new ClsPubAsset();
             obj.PrimeAccountNo = "0";
             obj.SubAccountNo = "0";
             Assetdetails.Add(obj);
         }
         return Assetdetails;
     }
     private List<ClsPubAsset> GetAssetDetails(bool IsEmptyData)
     {
         ClsPubSOAAsset AssetAccountdetails;
         List<ClsPubAsset> obj;

         if (Session["Assets"] == null || IsEmptyData)
         {
             obj = new List<ClsPubAsset>();
         }
         else
         {
             AssetAccountdetails = (ClsPubSOAAsset)Session["Assets"];
             obj = AssetAccountdetails.AssetDetails;
          }
         if (obj.Count == 0)
         {
             ClsPubAsset obj1 = new ClsPubAsset();
             obj1.PrimeAccountNo = "0";
             obj1.SubAccountNo = "0";
             obj.Add(obj1);
         }
         return obj;
     }



     private List<ClsPubTransaction> GetTransactionDetails()
     {
         ClsPubSOAAsset transactiondetails;
         List<ClsPubTransaction> obj;

         if (Session["Assets"] == null)
         {
             obj = new List<ClsPubTransaction>();
         }
         else
         {
             transactiondetails = (ClsPubSOAAsset)Session["Assets"];
             obj = transactiondetails.Transaction;
         }
         if (obj.Count == 0)
         {
             ClsPubTransaction obj1 = new ClsPubTransaction();
             obj1.PrimeAccountNo = "0";
             obj1.SubAccountNo = "0";
             obj.Add(obj1);
         }

         return obj;
     }

     private List<ClsPubSOAOpeningBalance> GetOpeningBalance()
     {
         ClsPubSOAAsset openingdetails;
         List<ClsPubSOAOpeningBalance> obj;

         if (Session["Assets"] == null)
         {
             obj = new List<ClsPubSOAOpeningBalance>();
         }
         else
         {
             openingdetails = (ClsPubSOAAsset)Session["Assets"];
             obj = openingdetails.Openingbalance;
             

         }
         return obj;
     }
     private List<ClsPubSummaryAccount> GetSummaryDetails()
     {
         List<ClsPubSummaryAccount> details = new List<ClsPubSummaryAccount>();
         if (Session["SummaryAccount"] != null)
         {
             details = (List<ClsPubSummaryAccount>)Session["SummaryAccount"];
         }
         else
         {
             details = new List<ClsPubSummaryAccount>();
             //rptd.ReportDefinition.Sections[3].SectionFormat.EnableSuppress = true;

         }

         return details;

     }

     private List<ClsPubMemorandumAccount> GetMemoDetails()
     {
         List<ClsPubMemorandumAccount> details = new List<ClsPubMemorandumAccount>();
         if (Session["Memoaccount"] != null)
         {
             details = (List<ClsPubMemorandumAccount>)Session["Memoaccount"];
         }
         else
         {
             details = new List<ClsPubMemorandumAccount>();
             //rptd.ReportDefinition.Sections[3].SectionFormat.EnableSuppress = true;

         }

         return details;

     }


}
