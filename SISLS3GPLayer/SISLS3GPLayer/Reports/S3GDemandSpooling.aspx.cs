using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using S3GBusEntity;

public partial class Reports_S3GDemandSpooling : System.Web.UI.Page
{
    string strCompanyId, strCategory, strDC, strDemandMonth, strDebtCollectorCode, strLobId, strIsAddressReq, strBranchId, strLobName, strBranch;
    Dictionary<string, string> Procparam;
    protected void Page_Load(object sender, EventArgs e)
    {
        strCompanyId = Request.QueryString.Get("qsCompanyId");
        strDemandMonth = Request.QueryString.Get("qsDemandMonth");
        strLobId = Request.QueryString.Get("qsLobId");
        strBranchId = Request.QueryString.Get("qsBranchId");
        strIsAddressReq = Request.QueryString.Get("qsIsAddressReq");
        strLobName = Request.QueryString.Get("qsLobName");
        strBranch = Request.QueryString.Get("qsBranch");
        strDebtCollectorCode = Request.QueryString.Get("qsDebtCollectorCode");
        strCategory = Request.QueryString.Get("qsCategory");
        strDC = Request.QueryString.Get("qsDC");

        if (!IsPostBack)
            FunPriLoadReport(strCompanyId, strDemandMonth, "1");
        else
            FunPriLoadReport(strCompanyId, strDemandMonth, "0");
    }

    private void FunPriLoadReport(string strCompanyId, string strDemandMonth, string strOption)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_Id", strCompanyId);
        Procparam.Add("@Demand_Month", strDemandMonth);
        if (strLobId != "0")
            Procparam.Add("@LOB_ID", strLobId);
        if (strBranchId != "0")
            Procparam.Add("@Location_Id", strBranchId);
        Procparam.Add("@IsAddressReq", strIsAddressReq);
        if (strDebtCollectorCode != "0" && strDebtCollectorCode != "")
            Procparam.Add("@DebtCollector_Code", strDebtCollectorCode);
        if (strCategory != "0" && strCategory != "")
            Procparam.Add("@Category", strCategory);
        if (strDC != "0" && strCategory != "")
            Procparam.Add("@IsDC", strDC);

        DataSet dsSet;
        if (strOption == "1")
            ViewState["dsSet"] = null;

        if (ViewState["dsSet"] == null)
        {
            dsSet = Utility.GetDataset(SPNames.S3G_Cln_GetdemandSpoolingData, Procparam);
            ViewState["dsSet"] = dsSet;
        }
        dsSet = (DataSet)ViewState["dsSet"];
        if (dsSet != null && dsSet.Tables[0] != null && dsSet.Tables[0].Rows.Count > 0)
        {
            ReportDocument rptd = new ReportDocument();
            rptd.Load(Server.MapPath("Report/DemandSpoolingReport.rpt"));

            TextObject txtDemandMonth = (TextObject)rptd.ReportDefinition.Sections["Section1"].ReportObjects["txtDemandMonth"];
            txtDemandMonth.Text = "Demand Spooling for - " + strDemandMonth + " (Line of Business - " + strLobName + " Location - " + strBranch + ")";
            rptd.SetDataSource(dsSet.Tables[0]);
            rptd.Subreports["MemoDetails"].SetDataSource(dsSet.Tables[1]);
            CRVDemand.ReportSource = rptd;
            CRVDemand.DataBind();
        }
        else
        {
            Utility.FunShowAlertMsg(this, "No demand data exists for the month");
            string strScipt = "window.close();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Repay", strScipt, true);
            return;
        }
    }
}
