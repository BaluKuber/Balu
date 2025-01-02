using System;
using System.Collections;
using System.Collections.Generic;
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
using S3GBusEntity;
using S3GBusEntity.Reports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class Reports_S3GCollectionReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (Session["AccountLevel"].ToString() == "FALSE")
            {
                List<ClsPubCollectionDetails> objListCollection = (List<ClsPubCollectionDetails>)Session["CollectionPrecise"];
                List<ClsPubCollectionHeader> objListCollectionHeader = (List<ClsPubCollectionHeader>)Session["CollectionHeader"];
                ReportDocument objReportDocument = new ReportDocument();
                objReportDocument.Load(Server.MapPath(@"Report\CollectionReport.rpt"));
                TextObject T4 = (TextObject)objReportDocument.ReportDefinition.Sections["Section2"].ReportObjects["txtDate"];
                T4.Text = Session["Date"].ToString();
                TextObject T0 = (TextObject)objReportDocument.ReportDefinition.Sections["Section2"].ReportObjects["txtCompanyName"];
                T0.Text = Session["CompanyName"].ToString();
                objReportDocument.Subreports["S3GCollectionHeaderSubReport.rpt"].SetDataSource(objListCollectionHeader);
                objReportDocument.Subreports["CRVCollectionReportForSubReport.rpt"].SetDataSource(objListCollection);
                TextObject T1 = (TextObject)objReportDocument.Subreports["S3GCollectionHeaderSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtCurrency"];
                T1.Text = Session["Currency"].ToString();
                TextObject T5 = (TextObject)objReportDocument.Subreports["S3GCollectionHeaderSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["CompanyName"];
                T5.Text = Session["ReportName"].ToString();
                //TextObject T2 = (TextObject)objReportDocument.Subreports["S3GCollectionHeaderSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtStartDate"];
                //T2.Text = Session["StartDate"].ToString();
                //TextObject T3 = (TextObject)objReportDocument.Subreports["S3GCollectionHeaderSubReport.rpt"].ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtEndDate"];
                //T3.Text = Session["EndDate"].ToString();
                objReportDocument.SetDataSource(objListCollection);                
                CRVCollectionReport.ReportSource = objReportDocument;
                CRVCollectionReport.DataBind();
            }

            else
            {
                List<ClsPubCollectionDetails> objListCollection = (List<ClsPubCollectionDetails>)Session["CollectionDetails"];
                List<ClsPubCollectionHeader> objListCollectionHeader = (List<ClsPubCollectionHeader>)Session["CollectionHeader"];
                ReportDocument objReportDocument = new ReportDocument();
                objReportDocument.Load(Server.MapPath(@"Report\CollectionReportDetailed.rpt"));
                TextObject T4 = (TextObject)objReportDocument.ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtDate"];
                T4.Text = Session["Date"].ToString();
                TextObject T0 = (TextObject)objReportDocument.ReportDefinition.Sections["Section2"].ReportObjects["CompanyName"];
                T0.Text = Session["ReportName"].ToString();
                //objReportDocument.Subreports["S3GCollectionHeaderSubReport.rpt"].SetDataSource(objListCollectionHeader);
                TextObject T1 = (TextObject)objReportDocument.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtCurrency"];
                T1.Text = Session["Currency"].ToString();
                TextObject T5 = (TextObject)objReportDocument.ReportDefinition.Sections["Section1"].ReportObjects["txtCompanyName"];
                T5.Text = Session["CompanyName"].ToString();
                //TextObject T2 = (TextObject)objReportDocument.ReportDefinition.Sections["Section1"].ReportObjects["txtStartDate"];
                //T2.Text = Session["StartDate"].ToString();
                //TextObject T3 = (TextObject)objReportDocument.ReportDefinition.Sections["Section1"].ReportObjects["txtEndDate"];
                //T3.Text = Session["EndDate"].ToString();                
                objReportDocument.SetDataSource(objListCollection);                
                CRVCollectionReport.ReportSource = objReportDocument;
                CRVCollectionReport.DataBind();
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw new ApplicationException(ae.Message);
        }
    }
}
