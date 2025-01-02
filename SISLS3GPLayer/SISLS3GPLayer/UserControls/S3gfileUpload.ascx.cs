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


public partial class S3GFileUpload : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtFilePath.Text = hidFilePath.Value;
        btnBrowse.Attributes.Add("onclick", "return getfile(" + fileupload.ClientID + "," + txtFilePath.ClientID + "," + hidFilePath.ClientID + ")");       
    }  
}
