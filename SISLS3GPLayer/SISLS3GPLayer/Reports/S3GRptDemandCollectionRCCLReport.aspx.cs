using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using S3GBusEntity;
using S3GBusEntity.Reports;
using CrystalDecisions.CrystalReports;
//using CrystalDecisions.Enterprise;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class Reports_S3GRptDemandCollectionRCCLReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {

            List<ClsPubDemandParameterCCL> ObjDemandRegionHeaderCCL = (List<ClsPubDemandParameterCCL>)Session["DemandParameter"];
                List<ClsPubDCRegionCustomerCodeGridDetails> ObjDemandRegionCCL = (List<ClsPubDCRegionCustomerCodeGridDetails>)Session["DemandCollection"];
                ReportDocument ObjReportDocument = new ReportDocument();
                 switch (Session["Selection"].ToString())
                {
                    case "CBD":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCLCustomerLevel.rpt"));
                        break;

                    case "CB":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCLCustomerLevel.rpt"));
                        break;

                    case "CD":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCLCustomerLevel.rpt"));
                        break;

                    case "C":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCLCustomerLevel.rpt"));
                        break;

                    case "GBD":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCLGroupLevel.rpt"));
                        break;

                    case "GB":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCLGroupLevel.rpt"));
                        break;

                    case "GD":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCLGroupLevel.rpt"));
                        break;

                    case "G":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCLGroupLevel.rpt"));
                        break;

                    case "AB":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCL.rpt"));
                        break;

                    case "AD":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCL.rpt"));
                        break;

                    case "A":
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCL.rpt"));
                        break;

                    default:
                        ObjReportDocument.Load(Server.MapPath("Report/DemandColectionRCCL.rpt"));
                        break;

                }

                 ObjReportDocument.Subreports["CRVDemandCollectionRegionCustomerCodeAccountLevel.rpt"].SetDataSource(ObjDemandRegionCCL);
                ObjReportDocument.SetDataSource(ObjDemandRegionHeaderCCL);

                //ObjDemandRegionHeaderCCL[0].BranchId = "";
                //ObjDemandRegionHeaderCCL[0].BranchName = "";
                //ObjDemandRegionHeaderCCL[0].ClassId = "";
                //ObjDemandRegionHeaderCCL[0].CompareFinYearStartMonthStartDate = DateTime.Today;
                //ObjDemandRegionHeaderCCL[0].CompareFromMonthPreMonthEndDate = DateTime.Today;
                //ObjDemandRegionHeaderCCL[0].CompareFromMonthStartDate = DateTime.Today;
                //ObjDemandRegionHeaderCCL[0].CompareToMonthEndDate = DateTime.Today;
                //ObjDemandRegionHeaderCCL[0].FinYearStartMonthStartDate = DateTime.Today;
                //ObjDemandRegionHeaderCCL[0].FrequencyId = "";
                //ObjDemandRegionHeaderCCL[0].FromMonthPreMonthEndDate = DateTime.Today;
                //ObjDemandRegionHeaderCCL[0].FromMonthStartDate = DateTime.Today;
                //ObjDemandRegionHeaderCCL[0].GroupId = "";
                //ObjDemandRegionHeaderCCL[0].GroupingCriteriaId = "";
                //ObjDemandRegionHeaderCCL[0].GroupingCriteriaName = "";
                //ObjDemandRegionHeaderCCL[0].PANum = "";
                //ObjDemandRegionHeaderCCL[0].PreFromMonth = "";
                //ObjDemandRegionHeaderCCL[0].PreToMonth = "";
                //ObjDemandRegionHeaderCCL[0].RegionId = "";
                //ObjDemandRegionHeaderCCL[0].RegionName = "";
                //ObjDemandRegionHeaderCCL[0].SANum = "";
                //ObjDemandRegionHeaderCCL[0].ToMonth = "";
                //ObjDemandRegionHeaderCCL[0].ToMonthEndDate = DateTime.Today;
                //ObjDemandRegionHeaderCCL[0].CustomerId = "";


                
                //List<test> ts = new List<test>();
                //test t = new test();
                //t.LobName="LOb";
                //t.RegionName = "Region";
                //t.FromMonth = "From";
                //t.ToMonth = "To";

                //ts.Add(t);


                //ObjReportDocument.Subreports[0].SetDataSource(ts);               
                TextObject T0 = (TextObject)ObjReportDocument.Subreports["CRVDemandCollectionRegionCustomerCodeAccountLevel.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtCurrency"];
                T0.Text = Session["Currency"].ToString(); 
                TextObject T2 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section2"].ReportObjects["txtName"];
                T2.Text = Session["ReportLevel"].ToString();
                TextObject T1 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section2"].ReportObjects["txtCompanyName"];
                T1.Text=Session["CompanyName"].ToString();
                TextObject T12 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section2"].ReportObjects["txtReportDate"];
                T12.Text = Session["Date"].ToString();
                //TextObject T3 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotOpeningDemand"];
                //T3.Text = Session["TotOpnDemand"].ToString();
                //TextObject T4 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotOpeningCollection"];
                //T4.Text = Session["TotOpnCollection"].ToString();
                //TextObject T5 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotOpeningPercentage"];
                //T5.Text = Session["OpeningPecentage"].ToString();
                //TextObject T6 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotMonthlyDemand"];
                //T6.Text = Session["TotMonDemand"].ToString();
                //TextObject T7 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotMonthlyCollection"];
                //T7.Text = Session["TotMonCollection"].ToString();
                //TextObject T8 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotMonthlyPercentage"];
                //T8.Text = Session["TotMonPercentage"].ToString();
                //TextObject T9 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotClosingDemand"];
                //T9.Text = Session["TotClsDemand"].ToString();
                //TextObject T10 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotClsCollection"];
                //T10.Text = Session["TotClsCollection"].ToString();
                //TextObject T11 = (TextObject)ObjReportDocument.ReportDefinition.Sections["Section4"].ReportObjects["txtTotClsPercentage"];
                //T11.Text = Session["TotClsPercentage"].ToString();
                CRVDemandCollectionRegionCustomerCode.ReportSource = ObjReportDocument;
                //ObjReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, "D:\\s.pdf");
                CRVDemandCollectionRegionCustomerCode.DataBind();
            
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }

    public class test
    {
        public string LobName { get; set; }
        public string RegionName { get; set; }
        public string FromMonth { get; set; }
        public string ToMonth { get; set; }
    }
    
}
