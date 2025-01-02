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


public partial class Reports_S3GRptBranchPerformanceReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        
        rptd.Load(Server.MapPath("Report/Branchperformance.rpt"));
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();
        TextObject lob = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtlob"];
        lob.Text = Session["LOB"].ToString();
        TextObject header = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtheading"];
        header.Text = "Branch Performance Report for" + " " + Session["Cutoffmonth"].ToString();
        TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtdate"];
        date.Text = Session["Date"].ToString();
        TextObject currency = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtcurrency"];
        currency.Text = Session["Denomination"].ToString();
        rptd.SetDataSource(getregbranch());
        rptd.Subreports["branch.rpt"].SetDataSource(GetNPADetails());
        rptd.Subreports["Opening"].SetDataSource(GetNPAOpeningDetails());
        rptd.Subreports["Addition"].SetDataSource(GetNPAAdditionDetails());
        rptd.Subreports["Deletion"].SetDataSource(GetNPADeletionDetails());
        rptd.Subreports["Closing"].SetDataSource(GetNPAClosingDetails());
        //rptd.Subreports["HeaderDetails"].SetDataSource(GetHeaderDetails());
        rptd.Subreports["payment.rpt"].SetDataSource(GetPayment());
        rptd.Subreports["unit.rpt"].SetDataSource(GetUnit());
        rptd.Subreports["collection.rpt"].SetDataSource(GetCollectionDetails());
        rptd.Subreports["cumulative.rpt"].SetDataSource(GetCumulativeCollectionDetails());
        rptd.Subreports["stock.rpt"].SetDataSource(GetStockDetails());
        TextObject txtlob = (TextObject)rptd.Subreports["stock.rpt"].ReportDefinition.Sections["Section3"].ReportObjects["txtstock"];
        txtlob.Text = Session["l1"].ToString();
        rptd.Subreports["Account.rpt"].SetDataSource(GetnoofAccounts());
        CRVBranchperformance.ReportSource = rptd;
        CRVBranchperformance.DataBind();

   
    }

    private List<ClsPubNPA> GetNPADetails()
    {
        List<ClsPubNPA> NPAdetails = new List<ClsPubNPA>();

        if (Session["Details"] != null)
        {
            NPAdetails = (List<ClsPubNPA>)Session["Details"];
            //rptd.ReportDefinition.Sections[3].SectionFormat.EnableSuppress = true;
        }
        else
        {
            NPAdetails = new List<ClsPubNPA>();
            //rptd.ReportDefinition.Sections[2].SectionFormat.EnableSuppress = true;
        }
        //if (NPAdetails.Count == 0)
        //{
        //    ClsPubNPA obj = new ClsPubNPA();
        //    obj.Region = "";
        //    obj.Branch = "";
        //    obj.ClassId = 0; ;
        //    obj.RegionId = 0;
        //    obj.BranchId = 0;
        //    obj.AssetClass = "";
        //    obj.OpeningNoOfAccounts = "";
        //    obj.OpeningStock = "";
        //    obj.OpeningArrear = "";
        //    obj.AdditionNoOfAccounts = "";
        //    obj.AdditionStock = "";
        //    obj.AdditionArrear = "";
        //    obj.DeletionNoOfAccounts = "";
        //    obj.DeletionStock = "";
        //    obj.DeletionArrear = "";
        //    obj.ClosingNoOfAccounts = "";
        //    obj.ClosingStock = "";
        //    obj.ClosingArrear= "";
        //    obj.stock = "";
        //    obj.Arrears = "";
        //            }
            return NPAdetails;

    }

    
  
    private List<ClsPubCollection> GetCollectionDetails()
    {
        List<ClsPubCollection> collections = new List<ClsPubCollection>();

        if (Session["Collection"] != null)
        {
            collections = (List<ClsPubCollection>)Session["Collection"];
        }
        else
        {
            collections = new List<ClsPubCollection>();
        }
        //if (collections.Count == 0)
        //{
        //    ClsPubCollection obj = new ClsPubCollection();
        //    obj.Region = "";
        //    obj.Branch = "";
        //    obj.Year = "";
        //    obj.CurrentCollection = "0";
        //    obj.ArrearCollection = "0";
        //    obj.TotalCollection = "0";
        //}

        return collections;

    }
  
    private List<ClsPubBranchStock> GetStockDetails()
    {
        List<ClsPubBranchStock> listbranchstock = new List<ClsPubBranchStock>();
        //ClsPubSummary summary = (ClsPubSummary)Session["Summary"];
        //List<ClsPubSummary> summarys = new List<ClsPubSummary>();
        
        //List<ClsPubBranchStock> branchstock = new List<ClsPubBranchStock>();
       // ClsPubBranchStock branchstock = (ClsPubBranchStock)Session["Stock"];
       // List<ClsPubBranchStock> Listbranchstock = new List<ClsPubBranchStock>();

        if (Session["Stock"] != null)
        {
            listbranchstock = (List<ClsPubBranchStock>)Session["Stock"];
            
        }
        else
        {
           // branchstock = new List<ClsPubBranchStock>();
            listbranchstock = new List<ClsPubBranchStock>();
        }
        //if (listbranchstock.Count == 0)
        //{
        //    ClsPubBranchStock obj = new ClsPubBranchStock();
        //    obj.Region = "";
        //    obj.Branch = "";
        //    obj.StockOnHire = "";
        //    obj.Arrears= "0";
        //    obj.ArrearsStock = "0";
            
        //}


        return listbranchstock;

    }


    private List<ClsNPAaccount> GetNPAOpeningDetails()
    {
        List<ClsNPAaccount> NPAdetails = new List<ClsNPAaccount>();

        if (Session["Opening"] != null)
        {
            NPAdetails = (List<ClsNPAaccount>)Session["Opening"];
            //rptd.ReportDefinition.Sections[7].SectionFormat.EnableSuppress = true;
        }
        else
        {
            NPAdetails = new List<ClsNPAaccount>();
            rptd.ReportDefinition.Sections[6].SectionFormat.EnableSuppress = true;
        }
        return NPAdetails;

    }

    private List<ClsNPAaccount> GetNPAAdditionDetails()
    {
        List<ClsNPAaccount> NPAdetails = new List<ClsNPAaccount>();

        if (Session["Addition"] != null)
        {
            NPAdetails = (List<ClsNPAaccount>)Session["Addition"];
            //rptd.ReportDefinition.Sections[8].SectionFormat.EnableSuppress = true;
        }
        else
        {
            NPAdetails = new List<ClsNPAaccount>();
            rptd.ReportDefinition.Sections[7].SectionFormat.EnableSuppress = true;
        }
        return NPAdetails;

    }

    private List<ClsNPAaccount> GetNPADeletionDetails()
    {
        List<ClsNPAaccount> NPAdetails = new List<ClsNPAaccount>();

        if (Session["Deletion"] != null)
        {
            NPAdetails = (List<ClsNPAaccount>)Session["Deletion"];
            //rptd.ReportDefinition.Sections[9].SectionFormat.EnableSuppress = true;
        }
        else
        {
            NPAdetails = new List<ClsNPAaccount>();
            rptd.ReportDefinition.Sections[8].SectionFormat.EnableSuppress =true;
        }
        return NPAdetails;

    }

    private List<ClsNPAaccount> GetNPAClosingDetails()
    {
        List<ClsNPAaccount> NPAdetails = new List<ClsNPAaccount>();

        if (Session["Closing"] != null)
        {
            NPAdetails = (List<ClsNPAaccount>)Session["Closing"];
            //rptd.ReportDefinition.Sections[10].SectionFormat.EnableSuppress = true;
        }
        else
        {
            NPAdetails = new List<ClsNPAaccount>();
            rptd.ReportDefinition.Sections[9].SectionFormat.EnableSuppress = true;
        }
        return NPAdetails;

    }

    private List<ClsPubBranchAsset> GetPayment()
    {
        List<ClsPubBranchAsset> asssets = new List<ClsPubBranchAsset>();

        if (Session["payment"] != null)
        {
            asssets = (List<ClsPubBranchAsset>)Session["payment"];
           
        }
        else
        {
            asssets = new List<ClsPubBranchAsset>();

        }
        //if (asssets.Count == 0)
        //{
        //    ClsPubBranchAsset obj = new ClsPubBranchAsset();
        //    obj.Region = "";
        //    obj.Branch = "";
        //    obj.AllAssetsMonth = "";
        //    obj.AllAssetsYTM= "";
            
        //}
        return asssets;

    }

    private List<ClsPubPaymentDetails> GetUnit()
    {
        List<ClsPubPaymentDetails> units = new List<ClsPubPaymentDetails>();

        if (Session["Units"] != null)
        {
            units = (List<ClsPubPaymentDetails>)Session["Units"];

        }
        else
        {
            units = new List<ClsPubPaymentDetails>();

        }
        //if (units.Count == 0)
        //{
        //    ClsPubPaymentDetails obj = new ClsPubPaymentDetails();
        //    obj.Region = "";
        //    obj.Branch = "";
        //    obj.AssetClass = "";
        //    obj.AssetClassMonth = "";
        //    obj.AssetClassYTM = "";
        //}
        return units;

    }

    private List<ClsPubCumulativeCollection> GetCumulativeCollectionDetails()
    {
        List<ClsPubCumulativeCollection> cumulative = new List<ClsPubCumulativeCollection>();

        if (Session["cumulative"] != null)
        {
            cumulative = (List<ClsPubCumulativeCollection>)Session["cumulative"];

        }
        else
        {
            cumulative = new List<ClsPubCumulativeCollection>();

        }
        //if (cumulative.Count == 0)
        //{
        //    ClsPubCumulativeCollection obj = new ClsPubCumulativeCollection();
        //    obj.Region = "";
        //    obj.Branch = "";
        //    obj.CumulativeCollection= "";
          
        //}
        return cumulative;

    }

    private List<ClsPubBranchAccount> GetnoofAccounts()
    {
        List<ClsPubBranchAccount> account = new List<ClsPubBranchAccount>();

        if (Session["Account"] != null)
        {
            account = (List<ClsPubBranchAccount>)Session["Account"];

        }
        else
        {
            account = new List<ClsPubBranchAccount>();

        }
        //if (account.Count == 0)
        //{
        //    ClsPubBranchAccount obj = new ClsPubBranchAccount();
        //    obj.Region = "";
        //    obj.Branch = "";
        //    obj.Accounts = "";

        //}
        return account;

    }

    private List<ClsPubRegionBranch> getregbranch()
    {
        List<ClsPubRegionBranch> regbranch = new List<ClsPubRegionBranch>();

        if (0 != null)
        {
            regbranch = (List<ClsPubRegionBranch>)Session["regbranch"];

        }
        else
        {
            regbranch = new List<ClsPubRegionBranch>();

        }
        if (regbranch.Count == 0)
        {
            ClsPubRegionBranch obj = new ClsPubRegionBranch();
            obj.Region = "";
            obj.Branch = "";
         

        }
        return regbranch;

    }


    //public List<test> GetTest()
    //{
    //    List<test> tests = new List<test>();

    //    test t = new test();
    //    t.AssetClass = "CCP";
    //    t.AssetClassMonth = 17;
    //    t.AssetClassYTM = 23;
    //    tests.Add(t);
        
    //    t = new test();
    //    t.AssetClass = "CTR";
    //    t.AssetClassMonth = 12;
    //    t.AssetClassYTM = 2;
    //    tests.Add(t);

    //    return tests;
    //}

}

//public class test
//{
//    public string AssetClass { get; set; }
//    public decimal AssetClassMonth { get; set; }
//    public decimal AssetClassYTM { get; set; }
//}