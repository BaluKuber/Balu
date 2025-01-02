#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Branch Performance Chart
/// Created By          :   JeyaGomathi M
/// Created Date        :   19-oct-2011
/// Purpose             :   To get the Branch Performance Report
/// Last Updated By		:   
/// Last Updated Date   :   
/// Reason              :  
/// <Program Summary>
#endregion


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


public partial class Reports_S3GRptBranchPerformanceChart : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {

        rptd.Load(Server.MapPath("Report/AssetChart.rpt"));
        rptd.SetDataSource(GetUnit());
        rptd.Subreports["CollectionChart"].SetDataSource(GetCollectionDetails());
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtcompany"];
        Company.Text = Session["Company"].ToString();
        TextObject header = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtheading"];
        header.Text = "Branch Performance Report for" + " " + Session["Cutoffmonth"].ToString();
        TextObject date = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtdate"];
        date.Text = Session["Date"].ToString();
        CRVBranchperformance.ReportSource = rptd;
        CRVBranchperformance.DataBind();


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
        //    //obj.Region = "";
        //    //obj.Branch = "";
        //    //obj.Year = "";
        //    //obj.CurrentCollection = "0";
        //    //obj.ArrearCollection = "0";
        //    //obj.TotalCollection = "0";
        //}

        return collections;

    }
}
