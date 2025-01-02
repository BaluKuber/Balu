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
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;

public partial class Common_S3GShowPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var outputStream = new MemoryStream();
        try
        {
            string virtualPath = string.Empty;
            string strFileName = string.Empty;
            string strFilePath = string.Empty;
            strFileName = Request.QueryString["qsFileName"];
            System.Threading.Thread.Sleep(1500);
            if (strFileName.Contains("Bill").ToString() == "0")
            {
                virtualPath = "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + ConfigurationManager.AppSettings["receiptpdfpath"].ToString() + strFileName;
                strFilePath = Server.MapPath(".").Replace("Common", "Collection") + "\\PDF Files\\" + strFileName;
            }
            else
            {
                virtualPath = "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + strFileName;
                strFilePath = strFileName;
            }

            if (Request.QueryString["rptType"] != null)
            {
                if (Convert.ToString(Request.QueryString["rptType"]) == "Rcpt")
                {
                    virtualPath = "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + ConfigurationManager.AppSettings["receiptpdfpath"].ToString() + strFileName;
                    strFilePath = Server.MapPath(".").Replace("Common", "Collection") + "\\PDF Files\\" + strFileName;
                    strFileName = strFilePath;
                }
            }

            //Response.Redirect(virtualPath);

            //Newly Added

            var pdfReader = new PdfReader(strFileName);
            var pdfStamper = new PdfStamper(pdfReader, outputStream);
            //Add the auto-print javascript
            var writer = pdfStamper.Writer;
            //writer.AddJavaScript(GetAutoPrintJs());
            pdfStamper.Close();
            var content = outputStream.ToArray();
            outputStream.Close();
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(content);
            Response.End();
            outputStream.Close();
            outputStream.Dispose(); //Upto This

        }
        catch (Exception ex)
        {
            //lblErrorMsg.Text = ex.Message;
        }
        finally
        {
            outputStream.Close();
            outputStream.Dispose(); //Upto This
        }
    }

    //And This
    protected string GetAutoPrintJs()
    {
        var script = new StringBuilder();
        script.Append("var pp = getPrintParams();");
        script.Append("pp.interactive= pp.constants.interactionLevel.full;");
        script.Append("print(pp);"); return script.ToString();
    }
}
