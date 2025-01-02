using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data;

public partial class WebService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] P = Request.Params["Parameters"].Split(new char[] { '#' });
        string[] PEx = Request.Params["ExtraParams"].Split(new char[] { '#' });
        String ProcName = Request.Params["MethodName"];
        String SearchFieldName = Request.Params["SearchFieldName"];
        String Search = Request.Params["Search"];
        String TextField = Request.Params["TextField"];
        String ValueField = Request.Params["ValueField"];



        Dictionary<String, String> Param = new Dictionary<String, String>();
        Param.Clear();
        for (int i = 0; i < P.Length; i++)
        {
            if (P[i].Trim() != "")
            {
                string[] L = P[i].Split(new char[] { ',' });
                Param.Add(L[0], L[1]);
            }
        }
        for (int i = 0; i < PEx.Length; i++)
        {
            if (PEx[i].Trim() != "")
            {
                string[] L = PEx[i].Split(new char[] { ',' });
                Param.Add(L[0], L[1]);
            }
        }
        Param.Add(SearchFieldName, Search);

        DataTable dt = new DataTable();
        dt = Utility.GetDefaultData(ProcName, Param);

        string q = "[";
        for (int r = 0; r < dt.Rows.Count; r++)
        {
            if (q == "[")
            {
                q += "{\"Value\":" + dt.Rows[r][ValueField] + ",\"Name\":\"" + dt.Rows[r][TextField] + "\"}";
            }
            else
            {
                q += "," + "{\"Value\":" + dt.Rows[r][ValueField] + ",\"Name\":\"" + dt.Rows[r][TextField] + "\"}";
            }

        }
        if (dt.Rows.Count == 0)
            q += "{\"Value\":1,\"Name\":\"No records found\"}";
        q += "]";
        Response.Write(q);
    }
}
