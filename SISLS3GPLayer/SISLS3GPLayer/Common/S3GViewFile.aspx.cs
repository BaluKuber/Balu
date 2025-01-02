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

public partial class Common_S3GViewFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string strFileName = string.Empty;
            strFileName = Request.QueryString["qsFileName"];
            //Changed by Thangam on 16-Jul-2012 for UAT bug fixing IVE_034
            //Response.WriteFile(strFileName);
            FileInfo flInfo = new FileInfo(strFileName);

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + flInfo.Name);
            Response.AddHeader("Content-Length", flInfo.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(flInfo.FullName);
            Response.End();                 
            //End here
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
    }
}
