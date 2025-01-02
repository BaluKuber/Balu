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

public partial class LandingPage :ApplyThemeForProject
{
    S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminClient;
    string strAlert;
    //string strRedirectHomePage = "window.location.href='./Common/HomePage.aspx';";
    string strRedirectHomePage = "window.location.href='./Common/S3GMaster.aspx';";
    string strRedirectLoginPage = "window.location.href='./LandingPage.aspx';";
    string strPWDGPSRecyItrCount;
   
    Dictionary<string, string> Procparam = null;
    

        /// <summary>
    /// PreInit Event of Page to set theme
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.Theme = "S3GTheme_Blue";
    }

    protected void btnGo_Click(object sender, ImageClickEventArgs e)
    {
        if (rbtAcceptCondtions.SelectedValue == "0")
        {
            string strUserLogin = "";
            string strPassword = "";
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
            string strMarqueeText = string.Empty;
            DateTime Last_LoginDate;
            int interrorCode;
            int intUserStatus;
            string strPWDIterationCount;

            ObjS3GAdminClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            interrorCode = ObjS3GAdminClient.FunPubValidateDemoLogin(out intCompanyID, out intUserID, out intUser_Level_Id, out strUsername, out strCompanyName, out strCompanyCode, out Last_LoginDate, out strUserTheme, out strAccess, out strCountryName, out strUserType, out strMarqueeText, Convert.ToInt32(rdbtnList.SelectedValue), "Demo", txtFullName.Text, txtMobilenumber.Text, txtemail.Text, txtDesg.Text, txtcomp.Text, txtopera.Text, txtcurrent.Text, "Demo@123");

            switch (interrorCode)
            {
                case 10:
                    Utility.FunShowAlertMsg(this, "Not a existing User");
                    break;
                case 11:
                    Utility.FunShowAlertMsg(this, "Usage limit expired");
                    break;
                case 0:
                    UserInfo UserInfo = new UserInfo(intCompanyID, intUserID, intUser_Level_Id, strUserLogin, strCompanyName, strUsername, ddlLanguage.SelectedItem.Value, strUserTheme, Last_LoginDate, strAccess, strCountryName, strUserType, strMarqueeText);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", strRedirectHomePage, true);
                    break;
            }
        }
    }

    protected void rdbtnList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnList.SelectedValue == "1")
        {
            FunProToggleControls(true);
        }
        else
        {
            FunProToggleControls(false);
        }
    }

    protected void FunProToggleControls(bool blShowControls)
    {
        tr1.Visible = tr2.Visible = tr3.Visible = tr4.Visible = blShowControls;
        
        RequiredFieldValidator4.Enabled = blShowControls;
        RequiredFieldValidator5.Enabled = blShowControls;
        RequiredFieldValidator6.Enabled = blShowControls;
        RequiredFieldValidator7.Enabled = blShowControls;
        
    }
}
