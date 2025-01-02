
#region NameSpaces
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using S3GBusEntity;
using System.Web.Security;
using System.Web.UI;
using System.Text;

#endregion

/// ---------><summary>
/// Created By      : Kuppusamy B
/// Purpose         : Password change based on GPS Values
/// Created Date    : 01/06/2012
/// Last Modified   :    
/// Software Pattern: 
/// </summary>

public partial class ChangePassword : ApplyThemeForProject
{
    int intCompanyID = 0;
    int intUserID = 0;
    string strUserName;
    string strNewPassword;
    int intErrorCode;
    string strUpperCaseNeed;
    string strNumberNeed;
    string strSpecCharNeed;
    int intPWDLength;
    string strRedirectPage = "../LoginPage.aspx";
    Boolean boolLoginPage;
    int intGPSPwdItrCount;
    string strPWDDefaultString;

    DataSet dsChagePWD = new DataSet();
    Dictionary<string, string> Procparam = null;

    S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();

    protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            this.Page.MasterPageFile = "~/Common/MasterPage.master";
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Request.QueryString.Get("qsMode") == "login"
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strUserName = ObjUserInfo.ProUserNameRW;

            //if (Request.QueryString.Get("qsMode") == "login")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "javascript:fnhideMenuButton();fnhideHeaderControls();fnValidatePWd();", true);
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "javascript:fnhideMenuButton();fnValidatePWd();", true);
            //}

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            DataSet ds = Utility.GetDataset("S3G_SYSAD_Get_GPS_PWD_Values", Procparam);

            if (ds.Tables[0].Rows.Count > 0)
            {
                strUpperCaseNeed = ds.Tables[0].Rows[0]["UpperCase_Char"].ToString();
                strNumberNeed = ds.Tables[0].Rows[0]["Numeral_Char"].ToString();
                strSpecCharNeed = ds.Tables[0].Rows[0]["Spec_Char"].ToString();
                intPWDLength = Convert.ToInt32(ds.Tables[0].Rows[0]["Min_pwd_length"].ToString());
            }

            strPWDDefaultString = "New Password should contains atleast ";

            if (strUpperCaseNeed == "True" || strUpperCaseNeed == "1")
            {
                strPWDDefaultString = strPWDDefaultString + "one uppercase alphabet[A-Z];";
            }

            if (strNumberNeed == "True" || strNumberNeed == "1")
            {
                strPWDDefaultString = strPWDDefaultString + " one numeral[0-9];";
            }

            if (strSpecCharNeed == "True" || strSpecCharNeed == "1")
            {
                strPWDDefaultString = strPWDDefaultString + " one special character[!,@,#,$,..];";
            }

            if (strUpperCaseNeed == "False" || strUpperCaseNeed == "0" && strNumberNeed == "False" || strNumberNeed == "0" && strSpecCharNeed == "False" || strSpecCharNeed == "0")
            {
                strPWDDefaultString = "Enter a password contains atleast one uppercase alphabet[A-Z]; a numeral[0-9] ; a special character[!,@,#,$,..];";
            }

            if (intPWDLength > 0)
            {
                strPWDDefaultString = strPWDDefaultString + " Length should be minimum " + intPWDLength + " digits.";
            }
            else
            {
                strPWDDefaultString = "Enter a password contains atleast one uppercase alphabet[A-Z]; a numeral[0-9] ; a special character[!,@,#,$,..];";
            }

            //hdnUpperCase.Value = strUpperCaseNeed;
            //hdnNumeral.Value = strNumberNeed;
            //hdnSpecChar.Value = strSpecCharNeed;
            //hdnPwdLength.Value = intPWDLength.ToString();

            lblPwdMessage.Text = strPWDDefaultString;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ObjS3GAdminServices.FunPubPasswordValidation(intUserID, txtOldPwd.Text.Trim()) > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid Old Password.');", true);
            return;
        }

        if (txtNewPwd.Text.Length < intPWDLength)
        {
            Utility.FunShowAlertMsg(this.Page, "New Password length should be minimum " + intPWDLength + " digits");
            return;
        }

        if (strUpperCaseNeed == "True" && !pwdHasCapitals(txtNewPwd.Text.Trim()))
        {
            Utility.FunShowAlertMsg(this.Page, "New Password should contain atleast one uppercase alphabet[A-Z]");
            return;
        }

        if (strNumberNeed == "True" && !pwdHasNumbers(txtNewPwd.Text.Trim()))
        {
            Utility.FunShowAlertMsg(this.Page, "New Password should contain atleast one numeral[0-9]");
            return;
        }

        if (strSpecCharNeed == "True" && !pwdHasSpecChar(txtNewPwd.Text.Trim()))
        {
            Utility.FunShowAlertMsg(this.Page, "New Password should contain atleast one special character[!,@,#,$,..]");
            return;
        }

        strNewPassword = ClsPubCommCrypto.EncryptText(txtNewPwd.Text.Trim());

        Procparam = new Dictionary<string, string>();

        Procparam.Add("@User_ID", intUserID.ToString());
        Procparam.Add("@Password", strNewPassword);
        Procparam.Add("@Company_ID", intCompanyID.ToString());

        dsChagePWD = Utility.GetDataset("S3G_SYSAD_Update_ChangePassword", Procparam);

        if (dsChagePWD.Tables[1].Rows.Count > 0)
        {
            intGPSPwdItrCount = Convert.ToInt32(dsChagePWD.Tables[1].Rows[0]["PWD_Itr_Count"].ToString());
        }

        if (dsChagePWD.Tables[0].Rows.Count > 0)
        {
            intErrorCode = Convert.ToInt32(dsChagePWD.Tables[0].Rows[0]["ErrorCode"].ToString());
            if (intErrorCode == 0)
            {
                //Utility.FunShowAlertMsg(this.Page, "Updated successfully",strRedirectPage);
                //By Siva.K on 05JUN2015 Redirect into Login Page.
                string strAlertMsg = "Updated successfully";
                string strRedirectPage = "../LoginPage.aspx";
                StringBuilder strMsg = new StringBuilder();
                strMsg.Append("alert('" + strAlertMsg + "');");
                strMsg.Append("window.top.location.href='" + strRedirectPage + "';");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", strMsg.ToString(), true);
            }
            else if (intErrorCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "You cannot enter previous "+ intGPSPwdItrCount + " passwords.");
                return;
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to update the password.");
                return;
            }
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString.Get("qsMode") == "login")
        {
            Response.Redirect("../LoginPage.aspx", false);
        }
        else
        {
            Response.Redirect("./HomePage.aspx", false);  
        }
    }

    #region Methods to check GPS values

    private bool pwdHasCapitals(string text)
    {
        foreach (char c in text)
        {
            if (char.IsUpper(c))
            return true;
        }
        return false;
    }

    private bool pwdHasNumbers(string text)
    {
        foreach (char c in text)
        {
            if (char.IsNumber(c))
                return true;
        }
        return false;
    }

    private bool pwdHasSpecChar(string text)
    {
        foreach (char c in text)
        {
            if (!char.IsLetterOrDigit(c))
                return true;
        }
        return false;
    }

    # endregion Methods to check GPS values

}
