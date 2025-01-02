

#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Income Report
/// Created By          :   Muni Kavitha    
/// Created Date        :   22-Oct-2011
/// Purpose             :   To show Income Report
/// <Program Summary>
/// 
#endregion

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

public partial class Reports_S3GRptIncomeReportReport : System.Web.UI.Page
{
    private ReportDocument rptd = new ReportDocument();


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {


        if (Session["Option"].ToString() == "1")
        {
            rptd.Load(Server.MapPath("Report/IncomeReport.rpt"));
            TextObject LOBName1 = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text1"];
            TextObject L1M = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text8"];
            TextObject L1Y = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text9"];
            TextObject L2M = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text13"];
            TextObject L2Y = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text12"];
            TextObject L3M = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text15"];
            TextObject L3Y = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text14"];
            TextObject L4M = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text17"];
            TextObject L4Y = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text16"];

            TextObject L1U = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text11"];
            TextObject L2U = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text42"];
            TextObject L3U = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text43"];
            TextObject L4U = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text44"];

            TextObject L1F = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text26"];
            TextObject L2F = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text39"];
            TextObject L3F = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text40"];
            TextObject L4F = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text41"];
                


            //LOBName1.Text = "";
            if (Session["LOBName1"] != null)
                LOBName1.Text = Convert.ToString(Session["LOBName1"]);
            else
                LOBName1.Text = L1M.Text = L1Y.Text =L1U.Text = L1F.Text ="";

            TextObject LOBName2 = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text5"];
            //LOBName2.Text = "";
            if (Session["LOBName2"] != null)
                LOBName2.Text = Convert.ToString(Session["LOBName2"]);
            else
                LOBName2.Text = L2M.Text = L2Y.Text = L2U.Text = L2F.Text = "";

            TextObject LOBName3 = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text3"];
            // LOBName3.Text = "";
            if (Session["LOBName3"] != null)
                LOBName3.Text = Convert.ToString(Session["LOBName3"]);
            else
                LOBName3.Text = L3M.Text = L3Y.Text = L3U.Text = L3F.Text = "";
            TextObject LOBName4 = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Text4"];
            // LOBName4.Text = "";
            if (Session["LOBName4"] != null)
                LOBName4.Text = Convert.ToString(Session["LOBName4"]);
            else
                LOBName4.Text = L4M.Text = L4Y.Text = L4U.Text = L4F.Text = "";

            TextObject RType = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["RType"];
            RType.Text = Session["RType"].ToString();

            TextObject AmountIn = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["AmountIn"];
            AmountIn.Text = Session["AmountIn"].ToString();

            TextObject Header2 = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Header2"];
            Header2.Text = Session["Rhead"].ToString();

            TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["CompName"];
            Company.Text = Session["CompName"].ToString();

            TextObject HeaderDateTime = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["HeaderDateTime"];
            HeaderDateTime.Text = Session["HeaderDateTime"].ToString();

            LineObject GP_Line = (LineObject)rptd.ReportDefinition.Sections["Section3"].ReportObjects["Line1"];
            LineObject GPF_Line=(LineObject)rptd.ReportDefinition.Sections["Section3"].ReportObjects["Line2"];
            LineObject GT_Line = (LineObject)rptd.ReportDefinition.Sections["Section4"].ReportObjects["Line3"];
            LineObject HT_Line = (LineObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Line4"];
            LineObject HB_Line = (LineObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Line5"];
            LineObject HM_Line = (LineObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["Line6"];


            if (Session["LOBName4"] != null)
            {
                GP_Line.Right = GPF_Line.Right = GT_Line.Right = HT_Line.Right = HB_Line.Right = HM_Line.Right = 14200;// 15120;
                AmountIn.Left = 14200-4200;// 15120 - 4200;
            }
            else if (Session["LOBName3"] != null)
            {
                GP_Line.Right = GPF_Line.Right = GT_Line.Right = HT_Line.Right = HB_Line.Right = HM_Line.Right = 10960;// 12120;
                AmountIn.Left = 10960-4200;// 12120 - 4200;

            }
            else if (Session["LOBName2"] != null)
            {
                GP_Line.Right = GPF_Line.Right = GT_Line.Right = HT_Line.Right = HB_Line.Right = HM_Line.Right = 7840;// 9160;
                AmountIn.Left = 7840 - 4200;// 9160 - 4200;
                //HeaderDateTime.Left = 9000;
                HeaderDateTime.Left = 6600;
                Company.Width = Header2.Width = 6360;
                Company.Left = Header2.Left = 1700;
            }
            else if (Session["LOBName1"] != null)
            {
                GP_Line.Right = GPF_Line.Right = GT_Line.Right = HT_Line.Right = HB_Line.Right = HM_Line.Right = 4850;// 6190;
                AmountIn.Left = 4850 - 4200;// 6190 - 4200;
                //HeaderDateTime.Left = 9000;
                HeaderDateTime.Left = 6600;
                Company.Width = Header2.Width = 6360;
                Company.Left = Header2.Left = 1700;

            }
            //LineObject line = (LineObject)rptd.Section3.ReportObjects["Line1"];

            rptd.SetDataSource(GetIncomeDetails());

            //rptd.Database.Tables[0].SetDataSource(ds.Tables [0]);
            //rptd.Database.Tables[1].SetDataSource(ds.Tables[1]);
            //rptd.Database.Tables[2].SetDataSource(ds.Tables[2]);
            //rptd.Database.Tables[3].SetDataSource(ds.Tables[3]);
            //rptd.Database.Tables[4].SetDataSource(ds.Tables[4]);
            if (Session["LOB1"] != null)
                rptd.Subreports["LOB1.rpt"].SetDataSource(GetLOB1());
            if (Session["LOB2"] != null)
                rptd.Subreports["LOB2.rpt"].SetDataSource(GetLOB2());
            if (Session["LOB3"] != null)
                rptd.Subreports["LOB3.rpt"].SetDataSource(GetLOB3());
            if (Session["LOB4"] != null)
                rptd.Subreports["LOB4.rpt"].SetDataSource(GetLOB4());
            //if(Session["INC_Account_DTL"]!=null )
            //    rptd.Subreports["INC_Account.rpt"].SetDataSource((DataTable)Session["INC_Account_DTL"]);

        }
        else if (Session["Option"].ToString() == "2")
        {
            rptd.Load(Server.MapPath("Report/Income_Account.rpt"));
            DataSet ds = new DataSet();
            if (Session["Report_Data"] != null)
                ds = (DataSet)Session["Report_Data"];
            rptd.SetDataSource(ds);

        }
        //rptd.Load(Server.MapPath("Report/CT1.rpt"));


      
       
//********Commented for Multiple Load*********//
        //if (Session["LOB1"] != null)
        //{
        //    rptd.Subreports["LOB1.rpt"].SetDataSource(GetLOB1());
        //    if (Session["LOB2"] != null)
        //    {
        //        rptd.Subreports["LOB2.rpt"].SetDataSource(GetLOB2());
        //        if (Session["LOB3"] != null)
        //        {
        //            rptd.Subreports["LOB3.rpt"].SetDataSource(GetLOB3());
        //            if (Session["LOB4"] != null)
        //                rptd.Subreports["LOB4.rpt"].SetDataSource(GetLOB4());

        //        }
        //        else if (Session["LOB4"] != null)
        //        {
        //            rptd.Subreports["LOB3.rpt"].SetDataSource(GetLOB3());
        //        }
        //    }
        //    else if (Session["LOB3"] != null)
        //    {
        //        rptd.Subreports["LOB2.rpt"].SetDataSource(GetLOB2());
        //        if (Session["LOB4"] != null)
        //        {
        //            rptd.Subreports["LOB3.rpt"].SetDataSource(GetLOB3());
        //        }
        //    }
        //    else if (Session["LOB4"] != null)
        //    {
        //        rptd.Subreports["LOB2.rpt"].SetDataSource(GetLOB2());
        //    }
        //}
        //else if (Session["LOB2"] != null)
        //{
        //    rptd.Subreports["LOB1.rpt"].SetDataSource(GetLOB1());
        //    if (Session["LOB3"] != null)
        //    {
        //        rptd.Subreports["LOB2.rpt"].SetDataSource(GetLOB2());
        //        if (Session["LOB4"] != null)
        //            rptd.Subreports["LOB3.rpt"].SetDataSource(GetLOB3());

        //    }
        //    else if (Session["LOB4"] != null)
        //    {
        //        rptd.Subreports["LOB2.rpt"].SetDataSource(GetLOB2());
        //    }

        //}
        //else if (Session["LOB3"] != null)
        //{
        //    rptd.Subreports["LOB1.rpt"].SetDataSource(GetLOB1());
        //    if (Session["LOB4"] != null)
        //    {
        //        rptd.Subreports["LOB2.rpt"].SetDataSource(GetLOB2());    
        //    }
        //}
        //else if (Session["LOB4"] != null)
        //{
        //    rptd.Subreports["LOB1.rpt"].SetDataSource(GetLOB1());
        //}
//********Commented for Multiple Load*********//


        CRVIncome.ReportSource = rptd;
        CRVIncome.DataBind();
    }


    //private List<ClsPubAssetPerformance> GetPerformanceofAsset()
    //{
    //    List<ClsPubAssetPerformance> PerformanceofAsset;

    //    if (Session["PerformanceofAsset"] == null)
    //    {
    //        PerformanceofAsset = new List<ClsPubAssetPerformance>();
    //    }
    //    else
    //    {
    //        PerformanceofAsset = (List<ClsPubAssetPerformance>)Session["PerformanceofAsset"];
    //    }
    //    return PerformanceofAsset;
    //}


    private List<ClsPubIncomeReport> GetIncomeDetails()
    {
        List<ClsPubIncomeReport> IncomeDetails;

        if (Session["IncomeDetails"] == null)
        {
            IncomeDetails = new List<ClsPubIncomeReport>();
        }
        else
        {
            IncomeDetails = (List<ClsPubIncomeReport>)Session["IncomeDetails"];
        }
        return IncomeDetails;
    }

    private List<ClsPubIncomeReport> GetIncome()
    {
        List<ClsPubIncomeReport> Income;

        if (Session["IncomeDetails"] == null)
        {
            Income = new List<ClsPubIncomeReport>();
        }
        else
        {
            Income = (List<ClsPubIncomeReport>)Session["IncomeDetails"];
        }
        return Income;
    }

    private List<ClsPubIncomeReport> GetLOB1()
    {
        List<ClsPubIncomeReport> LOB1;

        if (Session["LOB1"] == null)
        {
            LOB1 = new List<ClsPubIncomeReport>();
        }
        else
        {
            LOB1 = (List<ClsPubIncomeReport>)Session["LOB1"];
        }
        return LOB1;
    }

    private List<ClsPubIncomeReport> GetLOB2()
    {
        List<ClsPubIncomeReport> LOB2;

        if (Session["LOB2"] == null)
        {
            LOB2 = new List<ClsPubIncomeReport>();
        }
        else
        {
            LOB2 = (List<ClsPubIncomeReport>)Session["LOB2"];
        }
        return LOB2;
    }

    private List<ClsPubIncomeReport> GetLOB3()
    {
        List<ClsPubIncomeReport> LOB3;

        if (Session["LOB3"] == null)
        {
            LOB3 = new List<ClsPubIncomeReport>();
        }
        else
        {
            LOB3 = (List<ClsPubIncomeReport>)Session["LOB3"];
        }
        return LOB3;
    }

    private List<ClsPubIncomeReport> GetLOB4()
    {
        List<ClsPubIncomeReport> LOB4;

        if (Session["LOB4"] == null)
        {
            LOB4 = new List<ClsPubIncomeReport>();
        }
        else
        {
            LOB4 = (List<ClsPubIncomeReport>)Session["LOB4"];
        }
        return LOB4;
    }
}
