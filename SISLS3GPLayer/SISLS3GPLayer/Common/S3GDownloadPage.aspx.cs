using System;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
/// <summary>
/// Author : Rajendran
/// Purpose: 1. Download files from WebApplications
///          2. Open Generated files thru browser
/// </summary>
public partial class Common_S3GDownloadPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = "File Download";
            string strFileName = string.Empty;            
            strFileName = Request.QueryString["qsFileName"];
            PushDownload(strFileName);
        }
        catch (Exception ex)
        {
            string str = ex.Message;
        }
    }
    void PushDownload(string strFileName)
    {
        if (string.IsNullOrEmpty(Request.QueryString["qsNeedPrint"]) || Request.QueryString["qsNeedPrint"].ToString().ToLower() == "no")
        {
            if (!string.IsNullOrEmpty(Request.QueryString["qsHelp"]))
            {
                Response.Clear();
                Response.AppendHeader("content-disposition", "attachment; filename=" + strFileName);
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(strFileName);
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.AppendHeader("content-disposition", "attachment; filename=" + ".." + strFileName);
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(".." + strFileName);
                Response.End();
            }
           
        }
        else
        {
            var outputStream = new MemoryStream();
            var pdfReader = new PdfReader(strFileName);
            var pdfStamper = new PdfStamper(pdfReader, outputStream);
            //Add the auto-print javascript
            var writer = pdfStamper.Writer;
            writer.AddJavaScript(GetAutoPrintJs());
            pdfStamper.Close();
            var content = outputStream.ToArray();
            outputStream.Close();
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(content);
            Response.End();
            outputStream.Close();
            outputStream.Dispose(); 
        }
    }
    void PushOpen(string strFileName)
    {
        Response.Redirect(".." + strFileName);
        Response.End();
    }

    protected string GetAutoPrintJs()
    {
        var script = new StringBuilder();
        script.Append("var pp = getPrintParams();");
        script.Append("pp.interactive= pp.constants.interactionLevel.full;");
        script.Append("print(pp);"); return script.ToString();
    }
}
