using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.IO;
using System.Configuration;
using S3GBusEntity.LoanAdmin;
using System.Globalization;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string[] GetCustomerList(String prefixText, int count)
    {
        List<String> suggetions = null;
        //if (System.Web.HttpContext.Current.Session["CustomerDT"] != null)
        //{
        //    DataTable dtCustomer = new DataTable();
        //    dtCustomer = (DataTable)System.Web.HttpContext.Current.Session["CustomerDT"];
        //    suggetions = GetSuggestions(prefixText, count, dtCustomer);

        //    string filterExpression = "Customer_Code like '" + key + "%'";
        //    DataRow[] dtSuggestions = dt1.Select(filterExpression);
        //    foreach (DataRow dr in dtSuggestions)
        //    {
        //        string suggestion = dr["Customer_Code"].ToString();
        //        suggestions.Add(suggestion);
        //    }
        //}
        return suggetions.ToArray();
    }
    
}

