using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.ServiceModel;
using System.Collections.Generic;
using System.Globalization;

public partial class LoanAdmin_S3G_RPT_RS_ReportPage : System.Web.UI.Page
{
    Dictionary<string, string> ProcParam = null;
    System.Data.DataSet dsprint;
    UserInfo ObjUserInfo = null;
    string[] sConsolidate_Name = null;
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
        dsprint = Utility.GetDataset("S3G_LOANAD_GET_RPT_RSDtls", ProcParam);        

        switch (HttpContext.Current.Session["Format_Type"].ToString())
        {
            case "1"://RS_Format_1
                rptd.Load(Server.MapPath("RS_Format_1.rpt"));
                rptd.SetDataSource(dsprint.Tables[0]);
                rptd.Subreports["RS_Format_1_sub"].SetDataSource(dsprint.Tables[1]);
                break;
            case "2"://RS_Format_2
                rptd.Load(Server.MapPath("RS_Format_2.rpt"));
                rptd.SetDataSource(dsprint.Tables[0]);
                rptd.Subreports["RS_Format_2_sub"].SetDataSource(dsprint.Tables[1]);
                break;
            case "3"://RS_Format_3
                rptd.Load(Server.MapPath("RS_Format_3.rpt"));
                rptd.SetDataSource(dsprint.Tables[0]);
                rptd.Subreports["RS_Format_3_sub"].SetDataSource(dsprint.Tables[1]);
                break;
            case "4"://RS_Ack_Owner_4
                rptd.Load(Server.MapPath("RS_Ack_Owner_4.rpt"));
                rptd.SetDataSource(dsprint.Tables[0]);
                //rptd.Subreports["RS_Format_3_sub"].SetDataSource(dsprint.Tables[1]);
                break;
            case "5"://ADO Annexures

                //CreateWorkBook(dsprint, "ExportToExcel", 80);
                sConsolidate_Name = new string[] { "Annex-1_AOO_WA", "Annex-1_AOO WOA" };
                Print_click(dsprint, sConsolidate_Name);
               // //System.Data.DataTable dt=new System.Data.DataTable();
               ////string sfilePath= ExportExcel(dsprint.Tables[0]);
               // try
               // {
               //     string sFilePath = Server.MapPath(".") + "\\PDF Files\\test.xls";
               //     Context.Response.Clear();
               //     Context.Response.AddHeader("Content-Disposition", "attachment;filename=testSaran.xls");
               //     Context.Response.Charset = "";
               //     Context.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
               //     Context.Response.ContentType = "application/xls";
               //     Context.Response.WriteFile(sFilePath);
               //     Context.Response.End();
               // }
               // catch (Exception ex)
               // {
               //     //ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
               //     throw ex;
               // }
                break;
            case "6"://ADO Annexures with Amount
                
                System.Data.DataTable dt = dsprint.Tables[0];    
            
                dt.Columns.Add("Amt", typeof(string)).SetOrdinal(6);
                dt.AcceptChanges();

                foreach (DataRow dr in dt.Rows)
                {
                    String value = dr["Amount"].ToString();
                    value = Convert.ToDecimal(value).ToString("N", new CultureInfo("hi-IN"));
                    dr["Amt"] = value;                    
                    dt.AcceptChanges();                    
                }

                GridView Grv = new GridView();                
                dt.Columns.Remove("Amount");
                if (dt.Columns[6].ColumnName == "Amt")
                dt.Columns[6].ColumnName = "Amount";
                dt.AcceptChanges();
                Grv.DataSource = dt;
                Grv.DataBind();

                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;

                GridView grvh = new GridView();

                if (Grv.Rows.Count > 0)
                {
                    System.Data.DataTable dtHeader = new System.Data.DataTable();
                    dtHeader.Columns.Add("Col");

                    DataRow row = dtHeader.NewRow();
                    row["Col"] = "Annexure 2";
                    dtHeader.Rows.Add(row);

                    grvh.DataSource = dtHeader;
                    grvh.DataBind();

                    grvh.GridLines = GridLines.Both;

                    grvh.HeaderRow.Visible = false;
                                        
                    grvh.Rows[0].Cells[0].ColumnSpan = 10;

                    grvh.Rows[0].HorizontalAlign = HorizontalAlign.Left;
                    grvh.Rows[0].Font.Bold = true;
                    grvh.Rows[0].Font.Underline = true;
                                        
                    grvh.ForeColor = System.Drawing.Color.DarkBlue;
                    grvh.Font.Name = "calibri";                    
                    grvh.Font.Size = 10;
                                                            
                    string attachment = "attachment; filename=RS_Report.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.xlsx";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    GridView grv1 = new GridView();                   
                    grv1.ForeColor = System.Drawing.Color.DarkBlue;
                    grv1.Font.Name = "calibri";
                    grv1.Font.Size = 10;
                    grv1.RenderControl(htw);
                    grvh.RenderControl(htw);
                    Grv.RenderControl(htw);
                    Response.Write(sw.ToString());
                    Response.End();
                }
                break;

            case "7"://ADO Annexures Without Amount

                //CreateWorkBook(dsprint, "ExportToExcel", 80);
                GridView Grv2 = new GridView();
                Grv2.DataSource = dsprint.Tables[0];
                Grv2.DataBind();
                Grv2.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv2.ForeColor = System.Drawing.Color.DarkBlue;

                GridView grvhdr = new GridView();

                if (Grv2.Rows.Count > 0)
                {

                    System.Data.DataTable dtHeader = new System.Data.DataTable();
                    dtHeader.Columns.Add("Col");

                    DataRow row = dtHeader.NewRow();
                    row["Col"] = "Annexure 2";
                    dtHeader.Rows.Add(row);

                    grvhdr.DataSource = dtHeader;
                    grvhdr.DataBind();

                    grvhdr.GridLines = GridLines.Both;

                    grvhdr.HeaderRow.Visible = false;

                    grvhdr.Rows[0].Cells[0].ColumnSpan = 9;

                    grvhdr.Rows[0].HorizontalAlign = HorizontalAlign.Left;
                    grvhdr.Rows[0].Font.Bold = true;
                    grvhdr.Rows[0].Font.Underline = true;

                    grvhdr.ForeColor = System.Drawing.Color.DarkBlue;
                    grvhdr.Font.Name = "calibri";
                    grvhdr.Font.Size = 10;

                    string attachment = "attachment; filename=RS_Report.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.xlsx";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);

                    GridView grv1 = new GridView();
                    grv1.ForeColor = System.Drawing.Color.DarkBlue;
                    grv1.Font.Name = "calibri";
                    grv1.Font.Size = 10;
                    grv1.RenderControl(htw);
                    grvhdr.RenderControl(htw);
                    Grv2.RenderControl(htw);
                    Response.Write(sw.ToString());
                    Response.End();
                }                
                break;
        }

        if (rptd != null)
        {
            CRVNote.ReportSource = rptd;
            CRVNote.DataBind();
        }
    }

    protected string ExportExcel(System.Data.DataTable dt)
    {
        Workbooks workbooks = null;
        _Workbook workbook = null;
        Worksheet worksheet = null;
         string sFilePath ="";
        try
        {
            if (dt == null || dt.Rows.Count == 0) return "";
            Application xlApp = new Application();

            if (xlApp == null)
            {
                return "";
            }
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            workbooks = xlApp.Workbooks;
            workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);

             sFilePath = Server.MapPath(".") + "\\PDF Files\\test.xls";
            workbook.SaveAs(sFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12,
    System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false,
    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, System.Reflection.Missing.Value,
    System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            worksheet = (Worksheet)workbook.Worksheets[1];
            worksheet.Name = "Operational Info";
            //worksheet.Cells.AutoFit();
            Range range;
            long totalCount = dt.Rows.Count;
            long rowRead = 0;
            float percent = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 41;
                range.Font.Bold = true;
                range.Font.ColorIndex = 2;

            }

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                }
                rowRead++;
                percent = ((float)(100 * rowRead)) / totalCount;
            }

            xlApp.Visible = false;
            // workbook.Application.Sheets.Add(worksheet);
            workbook.Save();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (workbooks != null)
                workbook.Close(true, Missing.Value, Missing.Value);
            if (workbook != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

            worksheet = null;
            if (workbook != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            workbook = null;
        }
        return sFilePath;
    }


    private string CreateExcel()
    {
        Application app = null;
        Workbooks books = null;
        Workbook book = null;
        Sheets sheets = null;
        Worksheet sheet = null;
        Range range = null;
        string sFilePath;
        try
        {
            app = new Application();
            books = app.Workbooks;
       
            book = app.Workbooks.Add(System.Reflection.Missing.Value);
           
            //book.Worksheets.Add(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            //book.Worksheets.Add(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            ////book = books.Add(System.Reflection.Missing.Value);
            //app.Workbooks.Add(3);
            sFilePath = Server.MapPath(".") + "\\PDF Files\\RS" + System.DateTime.Today.ToString("yyyyMMdd") + DateTime.Now.Millisecond + ".xls";
            book.SaveAs(sFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12,
    System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false,
    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, false, false, System.Reflection.Missing.Value,
    System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            book.Close(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            app.Quit();
        }
        finally
        {
            if (range != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
            if (sheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
            if (sheets != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
            if (book != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            if (books != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
            if (app != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        }
        return sFilePath;
    }

    protected void Print_click(System.Data.DataSet dsprint, string[] sConsolidate_Name)
    {
        ApplicationClass excelApplicationClass = new ApplicationClass();
        _Workbook finalWorkbook = null;
        Workbook workBook = null;
        Workbooks workbooks = null;
        Worksheet workSheet = null;
        Worksheet newWorksheet = null;
        
        System.Data.DataTable dt = new System.Data.DataTable();
     string FilePath = CreateExcel().ToString();
        try
        {
            //if (OS_Consolidate_File.ToString() == "")
            //{
                System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                workbooks = excelApplicationClass.Workbooks;

            finalWorkbook = excelApplicationClass.Workbooks.Open(FilePath, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

               for (int i = 0; i < sConsolidate_Name.Length; i++)
               {

                   int countWorkSheet = finalWorkbook.Worksheets.Count;

                   //  finalWorkbook = books.Add();     
                   newWorksheet = (Worksheet)finalWorkbook.Sheets[i + 1];

                   newWorksheet.Name = sConsolidate_Name[i];

                   //Open the source WorkBook

                   newWorksheet = GenerateWorkSheet(newWorksheet, dsprint.Tables[i]);

               } 

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (finalWorkbook != null)
            {
                finalWorkbook.Close(true, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
            }

            if (workBook != null)
                workBook.Close(true, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

            if (workSheet != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

            workSheet = null;

            if (workBook != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);

            if (finalWorkbook != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(finalWorkbook);

            workBook = null;

            if (excelApplicationClass != null)
            {
                excelApplicationClass.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApplicationClass);
                excelApplicationClass = null;
            }
        }
        try
        {
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + FilePath + "");
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(FilePath);
            Response.End();
           
        }
        catch (Exception ex)
        {
        //    ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            throw ex;
        }
    }

    protected Worksheet GenerateWorkSheet(Worksheet worksheet, System.Data.DataTable dt)
    {
        try
        {
            Range range;
            long totalCount = dt.Rows.Count;
            long rowRead = 0;
            float percent = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 41;
                range.Font.Bold = true;
                range.Font.ColorIndex = 2;

            }

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                }
                rowRead++;
                percent = ((float)(100 * rowRead)) / totalCount;
            }

            return worksheet;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //finally
        //{
        //    if (workbook != null)
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //    worksheet = null;
        //    if (workbook != null)
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //    workbook = null;
        //}
    }
    
/*
    /// <summary>
    /// Method to create workbook
    /// </summary>
    /// <param name="cList"></param>
    /// <param name="wbName"></param>
    /// <param name="CellWidth"></param>
    private void CreateWorkBook(DataSet dsprint, string wbName, int CellWidth)
    {
        string attachment = "attachment; filename=\"" + wbName + ".xml\"";
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", attachment);
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        //HttpContext.Current.Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter sw = new System.IO.StringWriter();
        sw.WriteLine("<?xml version=\"1.0\"?>");
        sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
        sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
        sw.WriteLine("xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
        sw.WriteLine("xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
        sw.WriteLine("xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
        sw.WriteLine("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">");
        sw.WriteLine("<LastAuthor>Try Not Catch</LastAuthor>");
        sw.WriteLine("<Created>2013-01-09T19:14:19Z</Created>");
        sw.WriteLine("<Version>11.9999</Version>");
        sw.WriteLine("</DocumentProperties>");
        sw.WriteLine("<ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">");
        sw.WriteLine("<WindowHeight>9210</WindowHeight>");
        sw.WriteLine("<WindowWidth>19035</WindowWidth>");
        sw.WriteLine("<WindowTopX>0</WindowTopX>");
        sw.WriteLine("<WindowTopY>90</WindowTopY>");
        sw.WriteLine("<ProtectStructure>False</ProtectStructure>");
        sw.WriteLine("<ProtectWindows>False</ProtectWindows>");
        sw.WriteLine("</ExcelWorkbook>");
        sw.WriteLine("<Styles>");
        sw.WriteLine("<Style ss:ID=\"Default\" ss:Name=\"Normal\">");
        sw.WriteLine("<Alignment ss:Vertical=\"Bottom\"/>");
        sw.WriteLine("<Borders/>");
        sw.WriteLine("<Font/>");
        sw.WriteLine("<Interior/>");
        sw.WriteLine("<NumberFormat/>");
        sw.WriteLine("<Protection/>");
        sw.WriteLine("</Style>");
        sw.WriteLine("<Style ss:ID=\"s22\">");
        sw.WriteLine("<Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
        sw.WriteLine("<Borders>");
        sw.WriteLine("<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("</Borders>");
        sw.WriteLine("<Font ss:Bold=\"1\"/>");
        sw.WriteLine("</Style>");
        sw.WriteLine("<Style ss:ID=\"s23\">");
        sw.WriteLine("<Alignment ss:Vertical=\"Bottom\" ss:WrapText=\"1\"/>");
        sw.WriteLine("<Borders>");
        sw.WriteLine("<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("</Borders>");
        sw.WriteLine("</Style>");
        sw.WriteLine("<Style ss:ID=\"s24\">");
        sw.WriteLine("<Alignment ss:Vertical=\"Bottom\" ss:WrapText=\"1\"/>");
        sw.WriteLine("<Borders>");
        sw.WriteLine("<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"");
        sw.WriteLine("ss:Color=\"#000000\"/>");
        sw.WriteLine("</Borders>");
        sw.WriteLine("<Font ss:Color=\"#FFFFFF\"/>");
        sw.WriteLine("<Interior ss:Color=\"#FF6A6A\" ss:Pattern=\"Solid\"/>");
        //set header colour here
        sw.WriteLine("</Style>");
        sw.WriteLine("</Styles>");
        foreach (DataTable myTable in dsprint.Tables)
        {
            CreateWorkSheet(myTable.TableName, sw, myTable, CellWidth);
        }
        sw.WriteLine("</Workbook>");
        HttpContext.Current.Response.Write(sw.ToString());
        HttpContext.Current.Response.End();
    }

    /// <summary>
    /// Method to create worksheet
    /// </summary>
    /// <param name="wsName"></param>
    /// <param name="sw"></param>
    /// <param name="dt"></param>
    /// <param name="cellwidth"></param>
    private void CreateWorkSheet(string wsName, System.IO.StringWriter sw, DataTable dt, int cellwidth)
    {
        if (dt.Columns.Count > 0)
        {
            sw.WriteLine("<Worksheet ss:Name=\"" + wsName + "\">");
            int cCount = dt.Columns.Count;
            long rCount = dt.Rows.Count + 1;
            sw.WriteLine("<Table ss:ExpandedColumnCount=\"" + cCount +
              "\" ss:ExpandedRowCount=\"" + rCount + "\"x:FullColumns=\"1\"");
            sw.WriteLine("x:FullRows=\"1\">");
            for (int i = (cCount - cCount); i <= (cCount - 1); i++)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"1\" ss:Width=\"" + cellwidth + "\"/>");
            }
            DataTableRowIteration(dt, sw);
            sw.WriteLine("</Table>");
            sw.WriteLine("<WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
            sw.WriteLine("<Selected/>");
            sw.WriteLine("<DoNotDisplayGridlines/>");
            sw.WriteLine("<ProtectObjects>False</ProtectObjects>");
            sw.WriteLine("<ProtectScenarios>False</ProtectScenarios>");
            sw.WriteLine("</WorksheetOptions>");
            sw.WriteLine("</Worksheet>");
        }
    }

    /// <summary>
    /// Method to create rows by iterating thru datatable rows
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="sw"></param>
    private void DataTableRowIteration(DataTable dt, System.IO.StringWriter sw)
    {
        sw.WriteLine("");
        foreach (DataColumn dc in dt.Columns)
        {
            string tcText = dc.ColumnName;
            sw.WriteLine("<data>" + tcText + "</data>");
        }
        sw.WriteLine("");
        foreach (DataRow dr in dt.Rows)
        {
            sw.WriteLine("");
            foreach (DataColumn tc in dt.Columns)
            {
                string gcText = dr[tc].ToString();
                sw.WriteLine("<data>" + gcText + "</data>");
            }
            sw.WriteLine("");
        }
    }
    */

}