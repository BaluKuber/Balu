using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Fund_Management_S3G_RPt_TrancheReport : System.Web.UI.Page
{
    Dictionary<string, string> ProcParam = null;
    System.Data.DataSet dsprint;
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
        dsprint = Utility.GetDataset("S3G_LOANAD_GET_RPT_TrancheDtls", ProcParam);
        
        switch (HttpContext.Current.Session["Format_Type1"].ToString())
        {
            case "4"://RS_Ack_Owner_4
                rptd.Load(Server.MapPath("Tranche.rpt"));
                rptd.SetDataSource(dsprint.Tables[1]);
                rptd.Subreports["RS_Ack_Owner_4.rpt"].SetDataSource(dsprint.Tables[1]);
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
           

        }

     
      
                
             
       

        if (rptd != null)
        {
            CRVNote.ReportSource = rptd;
            CRVNote.DataBind();
        }
        //if (rptd != null)
        //{
        //    rptd.Close();
        //    rptd.Dispose();
        //}
    }

    protected string ExportExcel(System.Data.DataTable dt)
    {
        Workbooks workbooks = null;
        _Workbook workbook = null;
        Worksheet worksheet = null;
        string sFilePath = "";
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

}