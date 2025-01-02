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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


public partial class LoanAdmin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Dictionary<string, string> objProcedureParameter = new Dictionary<string, string>();
        //DataSet ds = Utility.GetDataset("S3G_Test", objProcedureParameter);

        //DataTable DtRepaySchedule = ds.Tables[0];
        //DataTable DtRepayStructure = ds.Tables[1];
        //Dictionary<string, string> objMethodParameters =new Dictionary<string, string>();
        //objMethodParameters.Add("VAT", "12");
        //objMethodParameters.Add("Recovery", "Staggered");
        //objMethodParameters.Add("SetOff", "NA");
        //objMethodParameters.Add("ITCAmount", "2500");

        //ClsRepaymentStructure obj = new ClsRepaymentStructure();
        //obj.FunPubGetRepaymentTax(out DtRepaySchedule, out DtRepayStructure, objMethodParameters);

        //GridView1.DataSource = DtRepayStructure;
        //GridView1.DataBind();

        //GridView2.DataSource = DtRepaySchedule;
        //GridView2.DataBind();

        string address = "vijayakumarr@sundaraminfotech.in";
string[] host = (address.Split('@'));
string hostname = host[1];

IPHostEntry IPhst = Dns.Resolve("sischnxng04.sis.ad");
IPEndPoint endPt = new IPEndPoint(IPhst.AddressList[0], 25);
Socket s= new Socket(endPt.AddressFamily, 
        SocketType.Stream,ProtocolType.Tcp);
s.Connect(endPt);


    }
}
