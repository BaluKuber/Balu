/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Reports
/// Screen Name			: Interest Query Report Additional Report file
/// Created By			: Manikandan. R
/// Start Date		    : 01-Dec-2012
/// End Date		    : 
/// Purpose	            : To Fetch Interest Report
/// Modified By         : --
/// Modified Date       : --
///  
///
#region [NAMESPACE]
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
using S3GBusEntity;
using System.Collections.Generic;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
#endregion [NAMESPACE]

public partial class Reports_S3GRptInterestQueryReport : System.Web.UI.Page
{
    static string strPageName = "Interest Query Report";
    Dictionary<string, string> Procparam;
    private ReportDocument rptd = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            Procparam = (Dictionary<string, string>)HttpContext.Current.Session["Procparam"];
            DataTable dtInterst = Utility.GetDefaultData("S3G_RPT_GetIncomeReportsDtls", Procparam);
            if (dtInterst.Columns.Count > 0)
            {
                if (dtInterst.Rows.Count > 0)
                {
                    //rptd.Load(Server.MapPath(@"Report\crBusinessReport.rpt"));
                    rptd.Load(Server.MapPath(@"Report\Interest_Report.rpt"));
                    rptd.SetDataSource(dtInterst);
                    CrvInterest.ReportSource = rptd;
                    CrvInterest.DataBind();
                }

            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
}
