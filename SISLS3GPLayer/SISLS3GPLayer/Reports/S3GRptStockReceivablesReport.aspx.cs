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
//using System.Web.UI.MobileControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class Reports_S3GRptStockReceivablesReport : System.Web.UI.Page
{
    DataTable dt;
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        
           
    }

      protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if(Session["id"].ToString()=="1")
            rptd.Load(Server.MapPath("Report/Stock.rpt"));
            if(Session["id"].ToString()=="2")
                rptd.Load(Server.MapPath("Report/Stock _Group.rpt"));
            if (Session["id"].ToString() == "3")
                rptd.Load(Server.MapPath("Report/Stock_Industrty.rpt"));
            if (Session["id"].ToString() == "0")
                rptd.Load(Server.MapPath("Report/Stock_Contract.rpt"));
            DataSet ds = new DataSet();
           if (Session["Report"] != null)
           ds = (DataSet)Session["Report"];
           dt = new DataTable();
           rptd.SetDataSource(ds.Tables[1]);
           rptd.Subreports["StockDetail"].SetDataSource(ds.Tables[0]);
           if (ds.Tables.Count > 1)
               rptd.Subreports["Details"].SetDataSource(ds.Tables[1]);
           TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtcompany"];
           Company.Text = Session["Company"].ToString();
           TextObject Header = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtheader"];
           Header.Text = Session["Header"].ToString();
           CRVSR.ReportSource = rptd;
            CRVSR.DataBind();
        }
        catch (Exception ex)
        {

        }
      }
    // protected void Page_Init(object sender, EventArgs e)
    //{
    //    ClsPubHeaderDetails Headerdetail = (ClsPubHeaderDetails)Session["Header"];
    //    ReportDocument rptd = new ReportDocument();

    //    rptd.Load(Server.MapPath("Report/StockReceivables.rpt"));


    //    TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtCompany"];
    //    Company.Text = Session["Company"].ToString();

    //    TextObject StockDate = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtDate"];
    //    StockDate.Text = Session["Date"].ToString();

        
        
    //    rptd.Subreports["StockHeader.rpt"].SetDataSource(GetHeaderdetails());

    //    rptd.Subreports["StockRecGrid.rpt"].SetDataSource(GetStockReceivableDetails());
    //    TextObject LOB = (TextObject)rptd.Subreports["StockRecGrid.rpt"].ReportDefinition.Sections["GroupHeaderSection4"].ReportObjects["txtStock"];
    //    LOB.Text = Session["LOB"].ToString();

    //    //TextObject Denomination = (TextObject)rptd.Subreports["StockRecGrid.rpt"].ReportDefinition.Sections["GroupHeaderSection5"].ReportObjects["txtCurrency"];
    //    //Denomination.Text = Session["Denomination"].ToString();

    //    CRVSR.ReportSource = rptd;
    //    CRVSR.DataBind();
    //}
    //private List<ClsPubStockReceivableDetails> GetStockReceivableDetails()
    //{
    //    List<ClsPubStockReceivableDetails> StockReceivables;

    //    if (Session["StockReceivable"] == null)
    //    {
    //        StockReceivables = new List<ClsPubStockReceivableDetails>();
    //    }
    //    else
    //    {
    //        StockReceivables = (List<ClsPubStockReceivableDetails>)Session["StockReceivable"];

    //    }
    //    return StockReceivables;
    //}

    //private List<ClsPubHeaderDetails> GetHeaderdetails()
    //{
    //    List<ClsPubHeaderDetails> Headerdetails = new List<ClsPubHeaderDetails>();
    //    if (Session["Header"] != null)
    //    {
    //        ClsPubHeaderDetails Headerdetail = (ClsPubHeaderDetails)Session["Header"];
    //        Headerdetails.Add(Headerdetail);
    //    }
    //    return Headerdetails;
    //}
}
