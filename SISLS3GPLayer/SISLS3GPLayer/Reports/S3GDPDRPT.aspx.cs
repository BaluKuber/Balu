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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using S3GBusEntity;
using S3GBusEntity.Reports;
public partial class Reports_S3GDPDRPT : System.Web.UI.Page
{
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;
    ReportDocument rptd = new ReportDocument();
    Dictionary<string, string> ProcParam;
    string TotFormulaLoc = "";
    string TotFormulaGrnd = "";
    ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient ObjOrgColClient = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
    int ColumnWidth = 0;
    int ColumnHeight = 0;
    int ColumnLeft = 0;
    int SetColumnLeft = 0;
    TextObject TxtObj;
    FormulaFieldDefinition FrmObj;
    FieldObject FldObj;
    decimal DecMinAdjVar;
    decimal DecMaxAdjVar;
    decimal DecCompVar;
    public void GetGPSSuffixVar(int Suffix)
    {
        if (Suffix == 1)
        {
            DecMinAdjVar = -0.5m;
            DecMaxAdjVar = 0.5m;
            DecCompVar = 0.0m;
        }
        else if (Suffix == 2)
        {
            DecMinAdjVar = -0.05m;
            DecMaxAdjVar = 0.05m;
            DecCompVar = 0.00m;
        }
        else if (Suffix == 3)
        {
            DecMinAdjVar = -0.005m;
            DecMaxAdjVar = 0.005m;
            DecCompVar = 0.000m;
        }
        else if (Suffix == 4)
        {
            DecMinAdjVar = -0.0005m;
            DecMaxAdjVar = 0.0005m;
            DecCompVar = 0.000m;
        }
        else if (Suffix == 5)
        {
            DecMinAdjVar = -0.00005m;
            DecMaxAdjVar = 0.00005m;
            DecCompVar = 0.00000m;
        }
        else if (Suffix == 6)
        {
            DecMinAdjVar = -0.000005m;
            DecMaxAdjVar = 0.000005m;
            DecCompVar =   0.000000m;
        }
        else if (Suffix == 7)
        {
            DecMinAdjVar = -0.0000005m;
            DecMaxAdjVar = 0.0000005m;
            DecCompVar = 0.0000000m;
        }
        else if (Suffix == 8)
        {
            DecMinAdjVar = -0.00000005m;
            DecMaxAdjVar =  0.00000005m;
            DecCompVar = 0.00000000m;
        }
        else if (Suffix == 9)
        {
            DecMinAdjVar = -0.000000005m;
            DecMaxAdjVar = 0.000000005m;
            DecCompVar = 0.000000000m;
        }
        else if (Suffix == 10)
        {
            DecMinAdjVar = -0.0000000005m;
            DecMaxAdjVar = 0.0000000005m;
            DecCompVar = 0.0000000000m;
        }
    }


    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();
        ObjS3GSession = new S3GSession();
        rptd.Load(Server.MapPath("Report/S3GDPDReport.rpt"));
        //Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;

        TextObject txtPrintdate = (TextObject)rptd.ReportDefinition.ReportObjects["txtPrintdate"];
        txtPrintdate.Text = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
        TextObject TxtCompany = (TextObject)rptd.ReportDefinition.ReportObjects["txtcompany"];
        TxtCompany.Text = ObjUserInfo.ProCompanyNameRW.ToString();
        TextObject TxtDenomination = (TextObject)rptd.ReportDefinition.ReportObjects["txtdenomination"];
        if (Session["Denomination"].ToString() != "Actual")
        {
            TxtDenomination.Text = "[All Amounts are " + ObjS3GSession.ProCurrencyNameRW + " in " + Session["Denomination"].ToString() + "]";
        }
        else
        {
            TxtDenomination.Text = "[All Amounts are in " + ObjS3GSession.ProCurrencyNameRW + "]";
        }
        TextObject TxtRptTitle = (TextObject)rptd.ReportDefinition.ReportObjects["TxtRptTitle"];
        string Month = Session["Month"].ToString();
        Month = "01" + "/" + Month.Substring(4, 2) + "/" + Month.Substring(0, 4);
        TxtRptTitle.Text = "DPD REPORT FOR THE MONTH " + Utility.StringToDate(Month).ToString("MMMM").ToUpper() + " " + Utility.StringToDate(Month).ToString("yyyy");
        //TxtRptTitle.Text = "DPD REPORT FOR THE MONTH " + Convert.ToDateTime(Month.ToString("DDMMYYYY")).ToString("MMMM").ToUpper() + " " + Convert.ToDateTime(Month..ToString("DDMMYYYY")).ToString("yyyy");
        //ReportMgtServices DSDPDReport = (ReportMgtServices)Session["DPDRPT"];
        //Session["Bucket"] = ;
        //Session["LOB"] = ddlLOB.SelectedValue;
        //Session["Denomination"] = ddlDenomination.SelectedValue;
        //Session["RCPT"] = TxtRcptDate.Text;
        //Session["BRANCH"] = ddlBranch.SelectedValue;
        //Session["REGION"] = ddlRegion.SelectedValue;
        byte[] byteDSDPDReport = ObjOrgColClient.FunPubGetDPDReportDetailsforRPT(Convert.ToInt32(Session["LOB"].ToString()), Session["Month"].ToString(), Convert.ToInt32(Session["Denom"].ToString()), Session["Bucket"].ToString(), Session["RCPT"].ToString(), ObjUserInfo.ProCompanyIdRW, Convert.ToInt32(Session["Location1"].ToString()), Convert.ToInt32(Session["Location2"].ToString()), 182, ObjUserInfo.ProUserIdRW, Convert.ToInt32(Session["AccountStatus"].ToString()));
        ReportMgtServices DSDPDReport = new ReportMgtServices();
        DSDPDReport = (ReportMgtServices)ClsPubSerialize.DeSerialize(byteDSDPDReport, SerializationMode.Binary, typeof(ReportMgtServices));


        for (int rowindex = 0; rowindex <= DSDPDReport.Tables[2].Rows.Count - 1; rowindex++)
        {
            decimal total = 0;
            for (int colindex = 5; colindex <= DSDPDReport.Tables[2].Columns.Count - 1; colindex++)
            {
                DSDPDReport.Tables[2].Rows[rowindex][colindex] = Convert.ToDecimal( Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString()).ToString(Funsetsuffix()));
            }
            DSDPDReport.Tables[2].AcceptChanges();
        }

        for (int rowindex = 0; rowindex <= DSDPDReport.Tables[2].Rows.Count - 1; rowindex++)
        {
            decimal total = 0;
            for (int colindex = 7; colindex <= DSDPDReport.Tables[2].Columns.Count - 2; colindex = colindex + 2)
            {
                total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString());
            }

            if ((total < Convert.ToDecimal(99.95))||(total > Convert.ToDecimal(100.05)))
            {
                total = 0;
                for (int colindex4 = 7; colindex4 <= DSDPDReport.Tables[2].Columns.Count - 2; colindex4 = colindex4 + 2)
                {
                    total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex4].ToString());
                }
                DSDPDReport.Tables[2].Rows[rowindex][DSDPDReport.Tables[2].Columns.Count - 1] = total;
                DSDPDReport.Tables[2].AcceptChanges();
                continue;
            }
            //decimal diff = 100-total;

            if ((total >= Convert.ToDecimal(99.95)) && (total <= Convert.ToDecimal(100.05)))
            {
                decimal diff = 100 - total;

                //if (total > 100)
                //{
                //    total = total + diff;
                //}
                //else if(total < 100)
                //{
                //    total = total + diff;
                //}
                total = 0;
                for (int colindex = 7; colindex <= DSDPDReport.Tables[2].Columns.Count - 2; colindex = colindex + 2)
                {
                    if (Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString()) != DecCompVar)
                    {
                        DSDPDReport.Tables[2].Rows[rowindex][colindex] = Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex]) + diff;
                        DSDPDReport.Tables[2].AcceptChanges();
                        for (int colindex2 = 7; colindex2 <= DSDPDReport.Tables[2].Columns.Count - 2; colindex2 = colindex2 + 2)
                        {
                            total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex2].ToString());
                        }
                        DSDPDReport.Tables[2].Rows[rowindex][DSDPDReport.Tables[2].Columns.Count - 1] = total;
                        DSDPDReport.Tables[2].AcceptChanges();
                        break;
                    }
                }
            }
           
        }
       // DataRow dr;
       //for (int i = 0; i <= 50; i++)
       // {
       //     dr = DSDPDReport.Tables[2].NewRow();
       //     dr[0] = DSDPDReport.Tables[2].Rows[0][0].ToString();
       //     dr[1] = DSDPDReport.Tables[2].Rows[0][1].ToString();
       //     dr[2] = DSDPDReport.Tables[2].Rows[0][2].ToString();
       //     dr[3] = DSDPDReport.Tables[2].Rows[0][3].ToString();
       //     dr[4] = DSDPDReport.Tables[2].Rows[0][4].ToString();
       //     dr[5] = DSDPDReport.Tables[2].Rows[0][5].ToString();
       //     dr[6] = DSDPDReport.Tables[2].Rows[0][6].ToString();
       //     dr[7] = DSDPDReport.Tables[2].Rows[0][7].ToString();
       //     dr[8] = DSDPDReport.Tables[2].Rows[0][8].ToString();
       //     dr[9] = DSDPDReport.Tables[2].Rows[0][9].ToString();
       //     dr[10] = DSDPDReport.Tables[2].Rows[0][10].ToString();
       //     dr[11] = DSDPDReport.Tables[2].Rows[0][11].ToString();
       //     dr[12] = DSDPDReport.Tables[2].Rows[0][12].ToString();
       //     dr[13] = DSDPDReport.Tables[2].Rows[0][13].ToString();
       //     dr[14] = DSDPDReport.Tables[2].Rows[0][14].ToString();
       //     dr[15] = DSDPDReport.Tables[2].Rows[0][15].ToString();
       //     dr[16] = DSDPDReport.Tables[2].Rows[0][16].ToString();
       //     dr[17] = DSDPDReport.Tables[2].Rows[0][17].ToString();
       //     dr[18] = DSDPDReport.Tables[2].Rows[0][18].ToString();
       //     dr[19] = DSDPDReport.Tables[2].Rows[0][19].ToString();
       //     dr[20] = DSDPDReport.Tables[2].Rows[0][20].ToString();
       //     dr[21] = DSDPDReport.Tables[2].Rows[0][21].ToString();
       //     dr[22] = DSDPDReport.Tables[2].Rows[0][22].ToString();
       //     dr[23] = DSDPDReport.Tables[2].Rows[0][23].ToString();
       //     dr[24] = DSDPDReport.Tables[2].Rows[0][24].ToString();
       //     dr[25] = DSDPDReport.Tables[2].Rows[0][25].ToString();
       //     DSDPDReport.Tables[2].Rows.Add(dr);

       // }
        rptd.SetDataSource(DSDPDReport.Tables[2]);

        switch (DSDPDReport.Tables[2].Columns.Count)
        {
            case 8:
                ColumnWidth = 1500;
                ColumnLeft = 100;
                ColumnHeight = 500;
                break;
            case 10:
                ColumnWidth = 1500;
                ColumnLeft = 100;
                ColumnHeight = 500;
                break;
            case 12:
                ColumnWidth = 1500;
                ColumnLeft = 100;
                ColumnHeight = 500;
                break;
            case 14:
                ColumnWidth = 1500;
                ColumnLeft = 100;
                ColumnHeight = 500;
                break;
            case 16:
                ColumnWidth = 1500;
                ColumnLeft = 100;
                ColumnHeight = 500;
                break;
            case 18:
                ColumnWidth = 1400;
                ColumnLeft = 10;
                ColumnHeight = 500;
                break;
            case 20:
                ColumnWidth = 1500;
                ColumnLeft = 70;
                ColumnHeight = 500;
                break;
            case 22:
                ColumnWidth = 1500;
                ColumnLeft = 70;
                ColumnHeight = 500;
                break;
            case 24:
                ColumnWidth = 1500;
                ColumnLeft = 50;
                ColumnHeight = 500;
                break;
            case 26:
                ColumnWidth = 1500;
                ColumnLeft = 10;
                ColumnHeight = 500;
                break;

        }
        CRVDPDRPT.ReportSource = rptd;
        CRVDPDRPT.DataBind();
        int RowIndex;
        SetColumnLeft = ColumnLeft;
        DataSet DSBucket = (DataSet)Session["Buckets"];
        FrmObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["t5"];
        FrmObj.Text = "'" + ObjS3GSession.ProGpsSuffixRW + "'";
        for (RowIndex = 1; RowIndex < DSDPDReport.Tables[1].Rows.Count; RowIndex++)
        {
            TxtObj = (TextObject)rptd.ReportDefinition.ReportObjects["Text" + Convert.ToString(RowIndex + 1)];
            FrmObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["Col" + Convert.ToString(RowIndex + 1)];
            FldObj = (FieldObject)rptd.ReportDefinition.ReportObjects["Col" + Convert.ToString(RowIndex + 1)];
            //if (RowIndex < 5)
            //{
            //    ColumnWidth = 900;
            //}
            //else
            //{
            //    ColumnWidth = 1100;
            //    //ColumnWidth = 700;
            //}

            //TxtObj.Width = ColumnWidth;
            //TxtObj.Height = ColumnHeight;
            //TxtObj.Left = SetColumnLeft;

            //FldObj.Width = ColumnWidth;
            //FldObj.Height = ColumnHeight;
            //FldObj.Left = SetColumnLeft;

            if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "Asset_Class")
            {
                FrmObj.Text = "'Asset Class'";
            }
            else if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "branch_name")
            {
                FrmObj.Text = "'Branch Name'";
            }
            else if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "Panum")
            {
                FrmObj.Text = "'Account No'";
            }
            else if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "Sanum")
            {
                FrmObj.Text = "'Sub Account No'";
            }
            else if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "Customer_Code")
            {
                FrmObj.Text = "'Customer Code'";

            }
            else if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "Customer_Name")
            {
                FrmObj.Text = "'Customer Name'";
            }
            else if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "Demand_Arrears")
            {
                FrmObj.Text = "'Demand'";
            }

            if (RowIndex == 5)
            {
                //ColumnWidth = 1450;
                FrmObj.Text = "'Demand Arrears'";

                FldObj = (FieldObject)rptd.ReportDefinition.ReportObjects["RTotCol" + Convert.ToString(RowIndex + 1)];
                //FldObj.Width = ColumnWidth;
                //FldObj.Height = ColumnHeight;
                //FldObj.Left = SetColumnLeft;

                //Grand Summary
                FldObj = (FieldObject)rptd.ReportDefinition.ReportObjects["RTotColGrnd" + Convert.ToString(RowIndex + 1)];
                //FldObj.Width = ColumnWidth;
                //FldObj.Height = ColumnHeight;
                //FldObj.Left = SetColumnLeft;
            }
            if (RowIndex > 5)
            {
                //ColumnWidth = 1450;
                if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString().Contains("%") == false)
                {
                    if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "total")
                    {
                        FrmObj.Text = "'" + "Total" + "'";
                    }
                    else
                    {
                        FrmObj.Text = "'" + (DSBucket.Tables[0].Select("Bucket = " + DSDPDReport.Tables[1].Rows[RowIndex][0].ToString().Replace("Bucket", ""))[0]["Bucket_From"].ToString() + " - " + DSBucket.Tables[0].Select("Bucket = " + DSDPDReport.Tables[1].Rows[RowIndex][0].ToString().Replace("Bucket", ""))[0]["Bucket_To"].ToString()) + " days'";
                    }

                    FldObj = (FieldObject)rptd.ReportDefinition.ReportObjects["RTotCol" + Convert.ToString(RowIndex + 1)];
                    //FldObj.Width = ColumnWidth;
                    //FldObj.Height = ColumnHeight;
                    //FldObj.Left = SetColumnLeft;

                    FldObj = (FieldObject)rptd.ReportDefinition.ReportObjects["RTotColGrnd" + Convert.ToString(RowIndex + 1)];
                    //FldObj.Width = ColumnWidth;
                    //FldObj.Height = ColumnHeight;
                    //FldObj.Left = SetColumnLeft;

                }
                else
                {
                    if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "total%")
                    {
                        FrmObj.Text = "'" + "Total%" + "'";
                        FormulaFieldDefinition FrmSumObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["RTotCol" + Convert.ToString(RowIndex + 1)];
                        FrmSumObj.Text = "Round(" + TotFormulaLoc.Substring(0, TotFormulaLoc.Length - 2) + ",1)";
                        
                        //FrmSumObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["total"];
                        //FrmSumObj.Text = TotFormulaLoc.Substring(0, TotFormulaLoc.Length - 2);

                        //TxtObj = (TextObject)rptd.ReportDefinition.ReportObjects["TxtRTotCol8"];
                        //TxtObj.Width = ColumnWidth;
                        //TxtObj.Height = ColumnHeight;
                        //TxtObj.Left = 800;

                        FrmSumObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["RTotColGrnd" + Convert.ToString(RowIndex + 1)];
                        FrmSumObj.Text = "Round(" + TotFormulaGrnd.Substring(0, TotFormulaGrnd.Length - 2) + ",1)";


                        //TxtObj = (TextObject)rptd.ReportDefinition.ReportObjects["TxtRTotColGrnd" + Convert.ToString(RowIndex + 1)];
                        //TxtObj.Width = ColumnWidth;
                        //TxtObj.Height = ColumnHeight;
                        //TxtObj.Left = 800;
                    }
                    else
                    {
                        FrmObj.Text = "'" + "Branch%" + "'";
                        

                       // TxtObj = (TextObject)rptd.ReportDefinition.ReportObjects["TxtRTotCol8"];
                        //TxtObj.Width = ColumnWidth;
                        //TxtObj.Height = ColumnHeight;
                        //TxtObj.Left = 800;

                        FormulaFieldDefinition FrmSumObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["RTotCol" + Convert.ToString(RowIndex + 1)];
                        if (RowIndex == 7)
                        {
                            //FrmSumObj.Text = "Truncate((((Sum ({Demand.Column" + ((RowIndex + 1) - 1) + "},{Demand.Column1})/Sum ({Demand.Column6},{Demand.Column1}))*100)+0.0001),4)";
                            FrmSumObj.Text = "((Sum ({Demand.Column" + ((RowIndex + 1) - 1) + "},{Demand.Column1})/Sum ({Demand.Column6},{Demand.Column1}))*100)+0.0001";


                        }
                        else
                        {
                            //FrmSumObj.Text = "Truncate(((Sum ({Demand.Column" + ((RowIndex + 1) - 1) + "},{Demand.Column1})/Sum ({Demand.Column6},{Demand.Column1}))*100),4)";
                            FrmSumObj.Text = "(Sum ({Demand.Column" + ((RowIndex + 1) - 1) + "},{Demand.Column1})/Sum ({Demand.Column6},{Demand.Column1}))*100";

                        }

                        //FrmSumObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["t" + Convert.ToString(RowIndex + 1)];
                        //FrmSumObj.Text = "(Sum ({Demand.Column" + ((RowIndex + 1) - 1) + "},{Demand.Column1})/Sum ({Demand.Column6},{Demand.Column1}))*100";

                        TotFormulaLoc += "{@RTotCol" + Convert.ToString(RowIndex + 1) + "} +";

                        FrmSumObj = (FormulaFieldDefinition)rptd.DataDefinition.FormulaFields["RTotColGrnd" + Convert.ToString(RowIndex + 1)];
                      

                        if (RowIndex == 7)
                        {
                            FrmSumObj.Text = "((Sum ({Demand.Column" + ((RowIndex + 1) - 1) + "})/Sum ({Demand.Column6}))*100) + 0.0001";
                        }
                        else
                        {
                            FrmSumObj.Text = "(Sum ({Demand.Column" + ((RowIndex + 1) - 1) + "})/Sum ({Demand.Column6}))*100";
                        }

                        TotFormulaGrnd += "{@RTotColGrnd" + Convert.ToString(RowIndex + 1) + "} +";

                        //TxtObj = (TextObject)rptd.ReportDefinition.ReportObjects["TxtRTotColGrnd" + Convert.ToString(RowIndex + 1)];
                        //TxtObj.Width = ColumnWidth;
                        //TxtObj.Height = ColumnHeight;
                        //TxtObj.Left = SetColumnLeft;
                    }



                }
                //Grand Summary

            }
            //if (RowIndex < 5)
            //{
            //    SetColumnLeft = SetColumnLeft + TxtObj.Width + 1;
            //}
            //else
            //{
            //    SetColumnLeft = SetColumnLeft + TxtObj.Width + 1;
            //}
            
        }

    }

}
