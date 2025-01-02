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

public partial class Reports_S3GPerformanceofAssetReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {


    }


    protected void Page_Init(object sender, EventArgs e)
    {

        rptd.Load(Server.MapPath("Report/PerformanceOfAsset.rpt"));
        TextObject AmountIn = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["AmountIn"];
        AmountIn.Text = Session["AmountIn"].ToString();

        TextObject IRRType = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["IRRType"];
        IRRType.Text = Session["IRRType"].ToString();

        TextObject LOB = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["LOB"];
        LOB.Text = Session["LOB"].ToString();

        //TextObject Rhead = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["Rhead"];
        //Rhead.Text = Session["Rhead"].ToString();

        TextObject RType = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["RType"];
        RType.Text = Session["RType"].ToString();

        // rptd.Load(Server.MapPath("Report/PerformanceOfAsset.rpt"));
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["CompName"];
        Company.Text = Session["CompName"].ToString();

        TextObject HeaderDateTime = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["HeaderDateTime"];
        HeaderDateTime.Text = Session["HeaderDateTime"].ToString();
        //FormulaFieldDefinitions GPSSuffix = (FormulaFieldDefinitions)rptd.ReportDefinition.Sections["Section4"].ReportObjects["GPSSuffix"];
        //GPSSuffixText = Session["GPSdecimal"].ToString();

        //FormulaFieldDefinitions 
        // ReportObject 

        rptd.SetDataSource(GetPerformanceofAsset());
        CRVPOA.ReportSource = rptd;
        CRVPOA.DataBind();
    }



    private List<ClsPubAssetPerformance> GetPerformanceofAsset()
    {
        List<ClsPubAssetPerformance> PerformanceofAsset;

        if (Session["PerformanceofAsset"] == null)
        {
            PerformanceofAsset = new List<ClsPubAssetPerformance>();
        }
        else
        {
            PerformanceofAsset = (List<ClsPubAssetPerformance>)Session["PerformanceofAsset"];
        }
        return PerformanceofAsset;
    }


}
