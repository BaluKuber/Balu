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
using CrystalDecisions.CrystalReports.Engine;
public partial class Reports_S3gRptLANRegisterReport : System.Web.UI.Page
{
    ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {

        //if (Session["Wise"].ToString() == "Account")
        //{
        //    rptd.Load(Server.MapPath("Report/LeaseAssetRegisterReportAccountWise.rpt"));
        //    rptd.SetDataSource(GetLobLoc());
        //    TextObject Customer = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtCustomer"];
        //    Customer.Text = Session["Customer"].ToString();

        //    TextObject LineofBus = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtLOB"];
        //    LineofBus.Text = Session["LOB"].ToString();

        //    TextObject Account = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtAccNo"];
        //    Account.Text = Session["AccountNo"].ToString();

        //    TextObject SubAccount = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtSubAccNo"];
        //    SubAccount.Text = Session["SubAccountNo"].ToString();

        //}
        //else
        //{
            rptd.Load(Server.MapPath("Report/LeaseAssetRegisterReport.rpt"));
            rptd.SetDataSource(GetLobLoc());
            TextObject LOB = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtLOB"];
            LOB.Text = Session["LOB"].ToString();

        //}
        //rptd.SetDataSource(GetLobLoc());
        
        TextObject Company = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtCompany"];
        Company.Text = Session["Company"].ToString();

        TextObject Date = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtDateTime"];
        Date.Text = Session["Date"].ToString();

        TextObject Title = (TextObject)rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtTitle"];
        Title.Text = Session["Title"].ToString();

        //TextObject LOB = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtLOB"];
        //LOB.Text = Session["LOB"].ToString();

        //TextObject Customer = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtCustomer"];
        //Customer.Text = Session["Customer"].ToString();

        //TextObject LineofBus = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtLineofBusiness"];
        //LineofBus.Text = Session["LOB"].ToString();

        //TextObject Account = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtAccNo"];
        //Account.Text = Session["AccountNo"].ToString();

        //TextObject SubAccount = (TextObject)rptd.ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtSubAccNo"];
        //SubAccount.Text = Session["SubAccountNo"].ToString();


        rptd.Subreports["LeaseAssetNumberSubReport.rpt"].SetDataSource(GetLanDetails());
        //if (Session["Option"].ToString() == "Yield")
        //{

        //    rptd.Subreports["LeaseAssetNumberSubReport.rpt"].ReportDefinition.Sections[2].SectionFormat.EnableSuppress = true;
        //    rptd.Subreports["LeaseAssetNumberSubReport.rpt"].ReportDefinition.Sections[4].SectionFormat.EnableSuppress = true;
        //    rptd.Subreports["LeaseAssetNumberSubReport.rpt"].ReportDefinition.Sections[6].SectionFormat.EnableSuppress = true;
        //}
        //else
        //{
        //    rptd.Subreports["LeaseAssetNumberSubReport.rpt"].ReportDefinition.Sections[3].SectionFormat.EnableSuppress = true;
        //    rptd.Subreports["LeaseAssetNumberSubReport.rpt"].ReportDefinition.Sections[5].SectionFormat.EnableSuppress = true;
        //    rptd.Subreports["LeaseAssetNumberSubReport.rpt"].ReportDefinition.Sections[7].SectionFormat.EnableSuppress = true;

        //}

        TextObject Denomination = (TextObject)rptd.Subreports["LeaseAssetNumberSubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection2"].ReportObjects["txtDenomination"];
        Denomination.Text = Session["Denomination"].ToString();

        TextObject Currency = (TextObject)rptd.Subreports["LeaseAssetNumberSubReport.rpt"].ReportDefinition.Sections["GroupHeaderSection1"].ReportObjects["txtCurrency"];
        Currency.Text = Session["Denomination"].ToString();

        

        rptd.Subreports["LeaseAssetDetailsSubReport.rpt"].SetDataSource(GetAssetDetails());


        CRVLan.ReportSource = rptd;
        CRVLan.DataBind();
    }

    private List<ClsPubLobLocation> GetLobLoc()
    {
        ClsPubLeaseAssetRegisterDetails LAN = new ClsPubLeaseAssetRegisterDetails();
        List<ClsPubLobLocation> lobloc = new List<ClsPubLobLocation>();

        if (Session["LanLocation"] != null)
        {
            LAN = (ClsPubLeaseAssetRegisterDetails)Session["LanLocation"];
            lobloc = LAN.LocationLan;   
        }
        else
        {
            lobloc = new List<ClsPubLobLocation>();

        }

        return lobloc;

    }
    private List<ClsPubLANRegisterDetails> GetLanDetails()
    {
        ClsPubLeaseAssetRegisterDetails LanDetails = new ClsPubLeaseAssetRegisterDetails();
        List<ClsPubLANRegisterDetails> LAN = new List<ClsPubLANRegisterDetails>();

        if (Session["LanDetails"] != null)
        {
            LanDetails = (ClsPubLeaseAssetRegisterDetails)Session["LanDetails"];
            LAN = LanDetails.LANDetails;
        }
        else
        {
            LAN = new List<ClsPubLANRegisterDetails>();
        }
        return LAN;
    }
      private List<ClsPubLANdetails> GetAssetDetails()
      {
          List<ClsPubLANdetails> AssetDetails;
          if(Session["Asset"]== null)
          {
              AssetDetails=new List<ClsPubLANdetails>();
          }
          else
          {
              AssetDetails=(List<ClsPubLANdetails>)Session["Asset"];
          }
          return AssetDetails;
      }


    //private List<ClsPubLANHeaderDetails>GetHeaderdetails()
    //{
    //     List<ClsPubLANHeaderDetails> Headerdetails = new List<ClsPubLANHeaderDetails>();
    //      if(Session["Header"]!=null)
    //      {
    //          ClsPubLANHeaderDetails Headerdetail = (ClsPubLANHeaderDetails)Session["Header"];
    //          Headerdetails.Add(Headerdetail);
    //      }
    //    return Headerdetails;
    //}
    
}