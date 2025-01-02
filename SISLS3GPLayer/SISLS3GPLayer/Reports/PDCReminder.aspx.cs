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

public partial class Reports_PDC_Reminder_ : System.Web.UI.Page
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void Page_Init(object sender, EventArgs e)
    {       
        try
        {
            if (Session["sessionGridDetails"] != null)
            {
                //List<ClsPubRptPDCReminderHeaderDetails> headerDetails = (List<ClsPubRptPDCReminderHeaderDetails>)Session["PDCReminderHeaderDetails"];
                //List<ClsPubRptPDCReminderGridDetails> PDCReminderGridDetails = (List<ClsPubRptPDCReminderGridDetails>)Session["PDCReminderGridDetails"];
                //ReportDocument rptd = new ReportDocument();
                //rptd.Load(Server.MapPath("Report/PDCReminderDetailsReport.rpt"));
                //rptd.SetDataSource(PDCReminderGridDetails);
                //rptd.Subreports["HeaderDetailsSubReport.rpt"].SetDataSource(headerDetails);
                //CRVPDCReminder.ReportSource = rptd;
                //CRVPDCReminder.DataBind();
                ReportDocument rptd = new ReportDocument();
                List<ClsPubRptPDCReminderGridDetails> griddetails = (List<ClsPubRptPDCReminderGridDetails>)Session["sessionGridDetails"];
                List<ClsPubPDCReminderAssetDetails> AssetDetails = (List<ClsPubPDCReminderAssetDetails>)Session["PDCReminderAssets"];
                rptd.Load(Server.MapPath(@"Report\PDCReminderFormat.rpt"));
                rptd.SetDataSource(griddetails);
                rptd.Subreports["PDCReminderAssetDetails.rpt"].SetDataSource(AssetDetails);                
                CRVPDCReminder.ReportSource = rptd;
                CRVPDCReminder.DataBind();
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }      
    }
}
