#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Common
/// Screen Name			: Master Page
/// Created By			: Kaliraj K
/// Created Date		: This master page is used for new window pages
#endregion

#region NameSpaces
using System;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using System.Configuration;
using System.IO.Compression;
using System.IO;

#endregion

namespace SmartLend3G
{
    public partial class S3GMasterPageCollapse : System.Web.UI.MasterPage
    {
        #region Intialization
        string strLocalization;
        UserInfo ObjUserInfo;
        string strMenuText;

        //For Login Session Time
        string sesstime;
        #endregion



        #region Page Load
        private void FunPriCheckIfFileExits()
        {
            String strPath = Server.MapPath(@"~\Data\UserManagement\" + ObjUserInfo.ProUserIdRW.ToString() + "_Disc.xml");
            if (File.Exists(strPath))
            {
                File.Delete(strPath);
                Utility.FunPubKillSession();
                //string strRedirectPageView = "window.location.href='../LoginPage.aspx';";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), new Guid().ToString(), "<script>forceClose();</script>", false);
            }
        }
        private void FunPriCheckIfUserIsActive()
        {
            sesstime = ConfigurationManager.AppSettings["Applcn_Session_Time"].ToString();
            try
            {
                if (Session["Last_Used_Time"] == null)
                {
                    Session["Last_Used_Time"] = DateTime.Now;
                    FunPriUpdateUserAsActive(Convert.ToDateTime(Session["Last_Used_Time"]));
                }
                else
                {
                    DateTime Now = DateTime.Now;
                    DateTime LoggedIn = Convert.ToDateTime(Session["Last_Used_Time"]);
                    TimeSpan Diff = Now - LoggedIn;

                    if (Diff.Minutes > Convert.ToInt32(sesstime) && Diff.Minutes < 45)
                    {
                        Session["Last_Used_Time"] = DateTime.Now;
                        FunPriUpdateUserAsActive(Convert.ToDateTime(Session["Last_Used_Time"]));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        private void FunPriUpdateUserAsActive(DateTime ActiveDateTime)
        {
            S3GAdminServicesReference.S3GAdminServicesClient ObjAdminServiceClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            try
            {

                int ErrorCode = ObjAdminServiceClient.FunPubUpdateUserIsActive(ActiveDateTime, ObjUserInfo.ProUserIdRW, ObjUserInfo.ProCompanyIdRW, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ObjAdminServiceClient.Close();
            }
        }
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ObjUserInfo = new UserInfo();
            try
            {
                System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                int SSOEnabled = (int)AppReader.GetValue("SSOEnabled", typeof(int));
                if (SSOEnabled == 1)
                {
                    //By Chandu to forcefully LogOut User on 22-Aug-2013
                    FunPriCheckIfFileExits();
                    //By Chandu to forcefully LogOut User

                    ///By Chandu For Checking if the user is active on 12-Aug-2013
                    FunPriCheckIfUserIsActive();
                    ///By Chandu For Checking if the user is active
                    ///
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string strPageUrl = Request.Url.ToString();

            string strPageName = strPageUrl.Substring(strPageUrl.LastIndexOf("/") + 1);
            string strPageNames = strPageName;

            String strPath = Server.MapPath(@"~\Data\UserManagement\" + ObjUserInfo.ProUserIdRW.ToString() + ".xml");
            strPageUrl = strPageUrl.Replace(strPageName, "");
            strPageUrl = strPageUrl.TrimEnd('/');
            string strModuleName = strPageUrl.Substring(strPageUrl.LastIndexOf("/") + 1);

            if (!File.Exists(strPath))
                strPath = Server.MapPath(@"~\Data\UserManagement\" + ObjUserInfo.ProUserIdRW.ToString() + "_" + Session.SessionID.ToString() + ".xml");

            if (File.Exists(strPath))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(strPath);

                if (strModuleName != "Common")
                {
                    DataTable dt = ds.Tables[strModuleName].Copy();

                    if (strPageNames.Contains('?'))
                        strPageName = strPageNames.Split('?').ToArray()[0];
                    
		            if (dt == null)
                        Response.Redirect("~/SessionExpired.aspx");

                    if (dt != null && dt.Select("Page_URL like" + "'" + strPageName + "%' OR Detail_Url like "+ "'" + strPageName + "%'").Count() <= 0)
                    {
                        //Response.Redirect("~/SessionExpired.aspx");
                    }
                }
            }
            
            hdndate.Value = DateTime.Today.ToString();
            

            strLocalization = ObjUserInfo.ProLocalizationRW;
            if (ObjUserInfo.ProUserNameRW != null)
            {

                FunPriSetPageTitle();

            }
            else
            {
                Response.Redirect("~/SessionExpired.aspx");
            }

            WebControl ctrl = GetPostBackControl() as WebControl;
            //if (ctrl != null && !SetNextFocus(Controls, ctrl.TabIndex + 1)) { ctrl.Focus(); }

        }

        #endregion



        public Control GetPostBackControl()
        {
            try
            {
                Control control = null;

                Page page = (HttpContext.Current.Handler as Page);
                string ctrlname1 = ScriptManager.GetCurrent(page).AsyncPostBackSourceElementID;

                var tabContainer = page.FindControl(ctrlname1) as AjaxControlToolkit.ComboBox;


                string ctrlname = Request.Params.Get("__EVENTTARGET");
                if (ctrlname1 != null && ctrlname1 != string.Empty && ctrlname1 != "null")
                {
                    control = FindControl(ctrlname);
                    if (control != null)
                        control.Focus();
                    else
                    {
                        foreach (string ctl in Request.Form)
                        {
                            if (ctl != null)
                            {
                                Control c = FindControl(ctl);

                                if (c != null && c.ClientID.Replace("$", "_").Contains(ctrlname1))
                                {
                                    control = c;
                                    c.Focus();
                                    break;
                                }
                            }
                        }
                    }
                }
                return control;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool SetNextFocus(ControlCollection controls, int tabIndex)
        {
            foreach (Control control in controls)
            {
                if (control.HasControls())
                {
                    bool found = SetNextFocus(control.Controls, tabIndex);
                    if (found) { return true; }
                }

                WebControl webControl = control as WebControl;
                if (webControl == null) { continue; }
                if (webControl.TabIndex != tabIndex) { continue; }

                webControl.Focus();
                return true;
            }

            return false;
        }







        /// <summary>
        /// Event triggered onPage Intialization
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {


            //Ramesh M for cultureinfo
            base.OnInit(e);
            //Modified by Nataraj Y to add localization to be obtained from Session variable 
            if (strLocalization != null)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(strLocalization);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(strLocalization);
            }
            else
            {
                strLocalization = "en";//ConfigurationManager.AppSettings["localization"].ToString();
            }

            // FunPriLoadMenu();


        }

        private void FunPriSetPageTitle()
        {
            if (SiteMapPath1.Provider.CurrentNode != null)
            {
                string strPageTitle = SiteMapPath1.Provider.CurrentNode.Description;
                strPageTitle = "S3G - " + strPageTitle;
                this.Page.Title = strPageTitle;
            }

        }


    }



}