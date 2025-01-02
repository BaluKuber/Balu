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
using System.IO;
using System.Text;
using S3GAdminServicesReference;
using S3GBusEntity;
using System.Net;
public partial class LoginPage : System.Web.UI.Page
{
    S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminClient;
    string strAlert;
    string strPWDRemainingDays;
    string strRedirectPWDPage = "window.location.href='./Common/Changepassword.aspx?qsMode=login';";
    string strRedirectHomePage = "window.location.href='./Common/S3GMaster.aspx';"; //By Siva.K on 05JUN2015
    string strRedirectLoginPage = "window.location.href='./LoginPage.aspx';";
    string strPWDGPSRecyItrCount;
    int strPWDSystemCount;
    Dictionary<string, string> Procparam = null;
    DataTable dtItrCount = new DataTable();
    string var;
    private Boolean FunPriEncryptedCount()
    {

        String StrEncrypted = File.ReadAllLines(var).GetValue(0).ToString();//ClsPubCommCrypto.EncryptText(StrEncrypted);
        String StrDecrypt = LicenceEnc.Decrpyt(StrEncrypted);
        if (StrDecrypt == "Invalid Key!")
            return false;
        int LimitUsers = Convert.ToInt32(StrDecrypt);



        DataTable dt = new DataTable();
        int ActiveUserCount;
        Dictionary<String, String> ParamsDict = new Dictionary<String, String>();
        ParamsDict.Clear();
        ParamsDict.Add("@LocationCode", "0");
        ParamsDict.Add("@Company_Id", "1");
        dt = Utility.GetDefaultData("S3G_SYSAD_GET_ACTIVEUSERCOUNT", ParamsDict);

        if (dt.Rows.Count == 0)
            ActiveUserCount = 0;
        else
            ActiveUserCount = Convert.ToInt32(dt.Rows[0][0].ToString());


        if (ActiveUserCount + 1 > LimitUsers)
        {
            return false;
        }
        return true;
    }

    private String readlicpath()
    {
        try
        {
            string strpath1 = ConfigurationManager.AppSettings["Applcn"].ToString();
            return strpath1;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private Boolean ResourceExists()
    {
        if (File.Exists(var))
        {
            return true;
        }
        return false;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        txtUserCode.Focus();
        if (!IsPostBack)
        {
            modalPopSession.Hide();
            Procparam = new Dictionary<string, string>();
            DataTable dt = Utility.GetDefaultData("S3G_SA_GET_COMPVISION", Procparam);

            dvText.InnerHtml = dt.Rows[0][0].ToString();            
        }
    }
    /// <summary>
    /// PreInit Event of Page to set theme
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.Theme = "S3GTheme_Blue";
    }
    protected void imgbtnLogin_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            modalPopSession.Hide();
            PnlPopup.Visible = false;
            string strUserLogin = txtUserCode.Text.Trim();
            string strPassword = txtPassword.Text;
            int intCompanyID = 0;
            int intUserID = 0;
            int intUser_Level_Id = 0;
            string strCompanyName = string.Empty;
            string strCompanyCode = string.Empty;
            string strUsername = string.Empty;
            string strLocalization = string.Empty;
            string strUserTheme = string.Empty;
            string strAccess = string.Empty;
            string strCountryName = string.Empty;
            string strUserType = string.Empty;
            string strUserMailID = string.Empty;
            string strMarqueeText = string.Empty;
            DateTime Last_LoginDate;
            int interrorCode;
            int loginDiff;

            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            int SSOEnabled = (int)AppReader.GetValue("SSOEnabled", typeof(int));


            ObjS3GAdminClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            interrorCode = ObjS3GAdminClient.FunPubValidateLogin(out intCompanyID, out intUserID, out intUser_Level_Id,
                out strUsername, out strCompanyName, out strCompanyCode, out Last_LoginDate, out strUserTheme,
                out strAccess, out strCountryName, out strUserType, out strMarqueeText, out loginDiff, strUserLogin, strPassword, SSOEnabled);

            DataSet ds = new DataSet();
            Procparam = new Dictionary<string, string>();

            switch (interrorCode)
            {
                case 1:
                    modalPopSession.Hide();
                    cvLogin.ErrorMessage = "Invalid User ID or Password";
                    cvLogin.IsValid = false;
                    cvLogin.CssClass = "styleMandatoryLabelLogin";
                    txtUserCode.Focus();

                    if (Procparam != null)
                    {
                        Procparam.Clear();
                    }
                    Procparam.Add("@User_Code", strUserLogin);
                    dtItrCount = Utility.GetDefaultData("S3G_UPD_UserStatus_DisableAccess", Procparam);
                    if (dtItrCount.Rows.Count > 0)
                    {
                        if (dtItrCount.Rows[0]["UserStatus"].ToString() == "0")
                        {
                            Utility.FunShowAlertMsg(this, "User account locked, contact system administrator.", "LoginPage.aspx");
                        }
                        else if (dtItrCount.Rows[0]["UserStatus"].ToString() == "1")
                        {
                            Utility.FunShowAlertMsg(this, "Invalid UserID Or Password", "LoginPage.aspx");
                        }
                        else if (dtItrCount.Rows[0]["UserStatus"].ToString() == "2")
                        {
                            Utility.FunShowAlertMsg(this, "User account locked, contact system administrator.", "LoginPage.aspx");
                        }
                        else if (dtItrCount.Rows[0]["UserStatus"].ToString() == "3")
                        {

                        }
                        else
                        {

                        }
                    }
                    break;

                case 2:
                    modalPopSession.Hide();
                    cvLogin.ErrorMessage = "Invalid Password";
                    cvLogin.IsValid = false;
                    cvLogin.CssClass = "styleMandatoryLabelLogin";
                    txtPassword.Focus();

                    if (Procparam != null)
                    {
                        Procparam.Clear();
                    }
                    Procparam.Add("@User_Code", strUserLogin);
                    dtItrCount = Utility.GetDefaultData("S3G_UPD_UserStatus_DisableAccess", Procparam);
                    if (dtItrCount.Rows.Count > 0)
                    {
                        if (dtItrCount.Rows[0]["UserStatus"].ToString() == "0")
                        {
                            Utility.FunShowAlertMsg(this, "User account locked, contact system administrator.", "LoginPage.aspx");
                        }
                        else if (dtItrCount.Rows[0]["UserStatus"].ToString() == "1")
                        {
                            Utility.FunShowAlertMsg(this, "Invalid UserID Or Password", "LoginPage.aspx");
                        }
                        else if (dtItrCount.Rows[0]["UserStatus"].ToString() == "2")
                        {
                            Utility.FunShowAlertMsg(this, "User account locked, contact system administrator.", "LoginPage.aspx");
                        }
                        else if (dtItrCount.Rows[0]["UserStatus"].ToString() == "3")
                        {

                        }
                        else
                        {

                        }
                    }
                    break;
                case 3:
                    modalPopSession.Hide();
                    /* Added on 14-Sep-2015 - Thalai - Message display in dynamic - Start */
                    Utility.FunShowAlertMsg(this, "The User ID that you are using is currently active or may not have logged out properly." +
                        "In case if it is an improper logout kindly wait for " + ConfigurationManager.AppSettings["Applcn_Session_Time"].ToString()
                        + " minutes or contact your administrator");
                    /* Added on 14-Sep-2015 - Thalai - Message display in dynamic - End */
                    break;
                case 0:
                    ////By Chandu 23-Oct-2013
                    //Temporarily commented for build
                  /*  var = readlicpath();
                    if (SSOEnabled == 1)
                    {
                        if (ResourceExists())
                        {
                            Boolean AllowAccess = FunPriEncryptedCount();
                            if (!AllowAccess)
                            {
                                Utility.FunShowAlertMsg(this, "In sufficient licence,please try later");
                                return;
                            }
                        }
                        else
                        {
                            Utility.FunShowAlertMsg(this, "Error in Licence Key Resource file.Kindly Contact System Administrator!");
                            return;
                        }
                    }
                   */
                    //Temporarily commented for build
                    ////By Chandu 23-Oct-2013
                    UserInfo UserInfo = new UserInfo(intCompanyID, intUserID, intUser_Level_Id, strUserLogin, strCompanyName, strUsername, ddlLanguage.SelectedItem.Value, strUserTheme, Last_LoginDate, strAccess, strCountryName, strUserType, strMarqueeText);
                    /*Method added to handle Password policy - BCA Enhancement - Kuppu - Sep-13*/
                    // FunPriGetPWDChangeDays(intCompanyID, strUserLogin, strRedirectPWDPage);
                    if (loginDiff == 1)
                    {
                        lblSession.Text = "The same user's Login exist in another session,</BR> Do you want to continue?.";
                        PnlPopup.Visible = true;
                        modalPopSession.Show();
                        //lblSession.Text="Login within session time";
                    }
                    else
                    {
                        UserInfo ObjUserInfo = new UserInfo();
                        String strPath = Server.MapPath(@"~\Data\UserManagement\");

                        DirectoryInfo d = new DirectoryInfo(strPath);//Assuming Test is your Folder
                        FileInfo[] Files = d.GetFiles("*_" + Session.SessionID.ToString() + ".xml"); //Getting Text files

                        foreach (FileInfo file in Files)
                        {
                            if (file.Name != ObjUserInfo.ProUserIdRW.ToString() + "_" + Session.SessionID.ToString() + ".xml")
                            {
                                lblSession.Text = "Another session exist in this PC, </BR> Do you want to continue?.";
                                PnlPopup.Visible = true;
                                modalPopSession.Show();
                                return;
                            }
                        }

                        int Executed = FunPriUserLoginTrans(UserInfo.ProCompanyIdRW, UserInfo.ProUserIdRW, UserInfo.ProLastLoginRW);
                    }
                    break;


                case 5:
                    UserInfo UserInfo_PWD = new UserInfo(intCompanyID, intUserID, intUser_Level_Id, strUserLogin, strCompanyName, strUsername, ddlLanguage.SelectedItem.Value, strUserTheme, Last_LoginDate, strAccess, strCountryName, strUserType, strMarqueeText);
                    strAlert += @"Please reset the Initial password to continue.";
                    Utility.FunShowAlertMsg(this, strAlert, "Common/Changepassword.aspx?qsMode=login");
                    break;
            }
        }
        catch (Exception ex)
        {
            S3GBusEntity.ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            Utility.FunShowAlertMsg(this, ex.Message.Replace("'", ""));
        }
        finally
        {
            if (ObjS3GAdminClient != null)
            {
                ObjS3GAdminClient.Close();
            }
        }
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        UserInfo ObjUserInfo = new UserInfo();
        String strPath = Server.MapPath(@"~\Data\UserManagement\");

        DirectoryInfo d = new DirectoryInfo(strPath);//Assuming Test is your Folder
        FileInfo[] Files = d.GetFiles(ObjUserInfo.ProUserIdRW.ToString() + "_*.xml"); //Getting Text files

        foreach (FileInfo file in Files)
        {
            File.Delete(strPath + file.Name);
        }

        Files = d.GetFiles("*_" + Session.SessionID.ToString() + ".xml"); //Getting Text files

        foreach (FileInfo file in Files)
        {
            if (file.Name != ObjUserInfo.ProUserIdRW.ToString() + "_" + Session.SessionID.ToString() + ".xml")
            {
                File.Delete(strPath + file.Name);
            }
        }
        int Executed = FunPriUserLoginTrans(ObjUserInfo.ProCompanyIdRW, ObjUserInfo.ProUserIdRW, ObjUserInfo.ProLastLoginRW);
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/LoginPage.aspx");
    }
    private int FunPriUserLoginTrans(int Company_ID, int User_ID, DateTime Last_Login)
    {
        int intErrorCode;
        S3GAdminServicesClient ObjWebServiceClient = new S3GAdminServicesClient();
        try
        {
            String strPath = Server.MapPath(@"~\Data\UserManagement\" + User_ID + "_Disc.xml");
            if (File.Exists(strPath))
            {
                File.Delete(strPath);
            }

            String Host_Name = string.Empty;
            int maxVal = 0;
            String IP_Address = string.Empty;

            try
            {
                Host_Name = System.Net.Dns.GetHostEntry(Request.UserHostName).HostName;
                maxVal = System.Net.Dns.GetHostAddresses(Host_Name).Length - 1;
                IP_Address = System.Net.Dns.GetHostAddresses(Host_Name).GetValue(maxVal).ToString();
            }
            catch (Exception ex1)
            {
                Host_Name = "Host Unknown";
                IP_Address = "0.0.0.0";
                goto Err_;
            }


        Err_:
            String Session_ID = System.Guid.NewGuid().ToString();

            StringBuilder StrInsUserDet = new StringBuilder();

            StrInsUserDet.Append("<Root>");
            StrInsUserDet.Append("<Details Company_ID = '" + Convert.ToString(Company_ID) + "'");
            StrInsUserDet.Append(" USER_ID = '" + Convert.ToString(User_ID.ToString()) + "'");
            StrInsUserDet.Append(" IP_Address = '" + Convert.ToString(IP_Address.ToString()) + "'");
            StrInsUserDet.Append(" Host_Name = '" + Convert.ToString(Host_Name.ToString()) + "'");
            StrInsUserDet.Append(" Session_ID = '" + Convert.ToString(Session_ID.ToString()) + "'");
            StrInsUserDet.Append(" />");
            StrInsUserDet.Append("</Root>");

            SystemAdmin.S3G_SYSAD_UserLoginDetailsDataTable dt = new SystemAdmin.S3G_SYSAD_UserLoginDetailsDataTable();
            SystemAdmin.S3G_SYSAD_UserLoginDetailsRow dr = dt.NewS3G_SYSAD_UserLoginDetailsRow();
            dr.COMPANY_ID = Company_ID.ToString();
            dr.USER_ID = User_ID.ToString();
            dr.HOST_NAME = Host_Name.ToString();
            dr.IP_ADDRESS = IP_Address.ToString();
            dr.Session_ID = Session_ID.ToString();
            dr.XMLUserLoginDetails = StrInsUserDet.ToString();
            dt.AddS3G_SYSAD_UserLoginDetailsRow(dr);


            SerializationMode SerMode = SerializationMode.Binary;
            intErrorCode = ObjWebServiceClient.FunPubInsertUserLoginDetails(SerMode, ClsPubSerialize.Serialize(dt, SerMode));

            HttpCookie CookUser_ID = new HttpCookie("CookUser_ID");
            Response.Cookies["CookUser_ID"].Value = User_ID.ToString();

            HttpCookie CookCOMPANY_ID = new HttpCookie("CookCOMPANY_ID");
            Response.Cookies["CookCOMPANY_ID"].Value = Company_ID.ToString();

            HttpCookie CookSession_ID = new HttpCookie("CookSession_ID");
            Response.Cookies["CookSession_ID"].Value = Session_ID.ToString();
            Response.Redirect("Common/S3GMaster.aspx", false);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            ObjWebServiceClient.Close();
        }
        return 0;
    }
    //public void FunPriGetPWDChangeDays(int intCompanyID, string strUserLogin, string strRedirectPWDPage)
    //{
    //    DataSet dsChagePWD = new DataSet();

    //    Procparam = new Dictionary<string, string>();
    //    Procparam.Add("@Company_ID", intCompanyID.ToString());
    //    Procparam.Add("@User_Code", strUserLogin.ToString());
    //    dsChagePWD = Utility.GetDataset("S3G_Get_PWDChange_Days", Procparam);
    //    int iPWDRemainingDays = 10;
    //    if (dsChagePWD.Tables[0].Rows.Count > 0)
    //    {
    //        strPWDRemainingDays = dsChagePWD.Tables[0].Rows[0]["Remaining_Days"].ToString();

    //        try
    //        {
    //            if (strPWDRemainingDays != "")
    //                iPWDRemainingDays = Convert.ToInt32(strPWDRemainingDays);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //        if (strPWDRemainingDays != "")
    //        {
    //            if (iPWDRemainingDays <= 0)
    //            {
    //                strAlert += @"Your password has been expired.Kindly change it now.";
    //                Utility.FunShowAlertMsg(this, strAlert, "Common/Changepassword.aspx?qsMode=login");
    //            }
    //            else if (iPWDRemainingDays == 1 || iPWDRemainingDays == 2)
    //            {
    //                strAlert += @"Your password will expire in " + strPWDRemainingDays + " Day(s)";
    //                strAlert += @"\n Do you want to change it now ?";
    //                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPWDPage + "}else {" + strRedirectHomePage + "}";
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "", strAlert, true);
    //            }
    //            else
    //            {
    //                //Response.Redirect("Common/HomePage.aspx", false);
    //                Response.Redirect("Common/S3GMaster.aspx", false);
    //            }
    //        }
    //        else
    //        {
    //            //Response.Redirect("Common/HomePage.aspx", false);
    //            Response.Redirect("Common/S3GMaster.aspx", false);
    //        }
    //    }
    //}

    private static Random random = new Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    protected void lnkForgetPass_Click(object sender, EventArgs e)
    {

        string strUserLogin = txtUserCode.Text.Trim();
        string strPassword = txtPassword.Text;
        int intCompanyID = 1;
        string strUserMailID = string.Empty;
        int interrorCode;
        string strNewPassword = string.Empty;
        string strEncrtPassword = string.Empty;
        string strUsername = string.Empty;

        strNewPassword = RandomString(6);

        strEncrtPassword = ClsPubCommCrypto.EncryptText(strNewPassword);

        ObjS3GAdminClient = new S3GAdminServicesReference.S3GAdminServicesClient();
        interrorCode = ObjS3GAdminClient.FunPubForgetPassword(out strUsername, out strUserMailID,
            strUserLogin, strEncrtPassword, intCompanyID);

        if (interrorCode == 1)
        {
            modalPopSession.Hide();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invalid User ID');", true);
            return;
        }

        if (strUserMailID == string.Empty)
        {
            modalPopSession.Hide();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Customer does not have a Email Id');", true);
            return;
        }

        System.Data.DataTable dt = new System.Data.DataTable();
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_Id", intCompanyID.ToString());
        Procparam.Add("@Lob_Id", "0");
        Procparam.Add("@Location_ID", "0");
        Procparam.Add("@Template_Type_Code", "11");
        dt = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);
        StringBuilder strBody = new StringBuilder();
        String strHTML = string.Empty;

        if (dt.Rows.Count == 0)
        {
            strHTML = "<table > <TR> <TD> Your OTP password : ~NewPassword~</TD> </TR> </Table>";
        }
        else
        {
            strHTML = dt.Rows[0]["Template_Content"].ToString();
        }

        DataTable dtHeader = new DataTable();
        dtHeader.Columns.Add("NewPassword");
        dtHeader.Columns.Add("UserName");

        DataRow Objdr = dtHeader.NewRow();
        Objdr["NewPassword"] = strNewPassword;
        Objdr["UserName"] = strUsername;
        dtHeader.Rows.Add(Objdr);

        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dtHeader);

        string strImagePath = String.Empty;
        if (strHTML.Contains("~CompanyLogo~"))
        {
            strImagePath = Server.MapPath("../Images/TemplateImages/CompanyLogo.png");
            strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strImagePath, strHTML);
        }

        Dictionary<string, string> dictMail = new Dictionary<string, string>();
        //opc056 start
        dictMail.Add("FromMail", "s3gnoreply@opc.co.in");
        //opc056 end
        dictMail.Add("ToMail", strUserMailID);
        dictMail.Add("Subject", "S3G Password Reset");
        dictMail.Add("DisplayName", "S3G-noreply");
        ArrayList arrMailAttachement = new ArrayList();

        strBody.Append(strHTML);

        try
        {
            string strErrMsg = Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
            if (strErrMsg != "")
                Utility.FunShowAlertMsg(this, strErrMsg);
            else
                Utility.FunShowAlertMsg(this, "Mail sent successfully");
        }
        catch (Exception objException)
        {
            Utility.FunShowAlertMsg(this, "Mail not sent, Please contact system admin");
            return;
        }
    }

}
