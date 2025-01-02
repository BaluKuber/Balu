using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SessionExpired : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            S3GAdminServicesReference.S3GAdminServicesClient ObjWebServiceClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            HttpCookie CookUser_ID = new HttpCookie("CookUser_ID");
            HttpCookie CookCOMPANY_ID = new HttpCookie("CookCOMPANY_ID");
            HttpCookie CookSession_ID = new HttpCookie("CookSession_ID");

            try
            {
                CookUser_ID = Request.Cookies["CookUser_ID"];
                CookCOMPANY_ID = Request.Cookies["CookCOMPANY_ID"];
                CookSession_ID = Request.Cookies["CookSession_ID"];
                if (CookUser_ID == null)
                    return;
                int intErrorCode = ObjWebServiceClient.FunPubUpdateLogOutFlags(Convert.ToInt32(CookCOMPANY_ID.Value), Convert.ToInt32(CookUser_ID.Value), Convert.ToString(CookSession_ID.Value), "", 3);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ObjWebServiceClient.Close();
                if (CookUser_ID != null)
                {
                    CookUser_ID.Expires.AddSeconds(1);
                    CookCOMPANY_ID.Expires.AddSeconds(1);
                    CookSession_ID.Expires.AddSeconds(1);
                }
            }
        }
        else
        {
            Response.Redirect("~/LoginPage.aspx");
        }
    }
}
