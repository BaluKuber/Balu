using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Fund_Management_S3G_RPT_Note_ReportPage : System.Web.UI.Page
{
    Dictionary<string, string> ProcParam = null;
    DataSet dsprint;
    ReportDocument rptd = new ReportDocument();

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        if (rptd != null)
        {
            rptd.Close();
            rptd.Dispose();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
         ProcParam = (Dictionary<string, string>)HttpContext.Current.Session["ProcParam"];
         dsprint = Utility.GetDataset("S3G_Fund_Get_NotePrint", ProcParam);         
        
        switch (HttpContext.Current.Session["Format_Type"].ToString())
        {
            //case "1"://Note_Format_1
            //    rptd.Load(Server.MapPath("NoteCreation.rpt"));
            //    rptd.SetDataSource(dsprint.Tables[0]);         
            //    rptd.Subreports["amort.rpt"].SetDataSource(dsprint.Tables[1]);
            //    break;
            case "1"://Note_Format_1
                rptd.Load(Server.MapPath("Note_LT.rpt"));
                rptd.SetDataSource(dsprint.Tables[0]);
                rptd.Subreports["Note_LT_SubText"].SetDataSource(dsprint.Tables[2]);
                rptd.Subreports["Note_LT_amort"].SetDataSource(dsprint.Tables[1]);
                rptd.Subreports["Note_LT_Annex"].SetDataSource(dsprint.Tables[3]);
                break;
            case "2"://Note_Format_2
                rptd.Load(Server.MapPath("Note_BNP.rpt"));
                rptd.SetDataSource(dsprint.Tables[0]);
                rptd.Subreports["Note_LT_SubText"].SetDataSource(dsprint.Tables[2]);
                rptd.Subreports["Note_LT_amort"].SetDataSource(dsprint.Tables[1]);                
                break;
            case "3"://Note_Format_3
                rptd.Load(Server.MapPath("Note_IndusInd.rpt"));
                rptd.SetDataSource(dsprint.Tables[0]);
                rptd.Subreports["Note_LT_SubText"].SetDataSource(dsprint.Tables[2]);
                rptd.Subreports["Note_LT_amort"].SetDataSource(dsprint.Tables[1]);  
                break;
            case "4"://Note_Format_4
                rptd.Load(Server.MapPath("Note_RelianceCap.rpt"));
                rptd.SetDataSource(dsprint.Tables[0]);
                rptd.Subreports["Note_LT_SubText"].SetDataSource(dsprint.Tables[2]);
                rptd.Subreports["Note_LT_amort"].SetDataSource(dsprint.Tables[1]);  
                break;
        }
        if (rptd != null)
        {
            CRVNote.ReportSource = rptd;
            CRVNote.DataBind();
        }
    }
}