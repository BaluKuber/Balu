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
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        #region Intialization
        string strLocalization;
        UserInfo ObjUserInfo;
        string strMenuText;
        #endregion


       
        #region Page Load
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPageUrl = Request.Url.ToString();

            string strPageName = strPageUrl.Substring(strPageUrl.LastIndexOf("/") + 1);
            strPageUrl = strPageUrl.Replace(strPageName, "");
            strPageUrl = strPageUrl.TrimEnd('/');
            string strModuleName = strPageUrl.Substring(strPageUrl.LastIndexOf("/") + 1);


            ObjUserInfo = new UserInfo();
            strLocalization = ObjUserInfo.ProLocalizationRW;
            if (ObjUserInfo.ProUserNameRW != null)
            {

                FunPriSetPageTitle();

            }
            else
            {
                Response.Redirect("~/SessionExpired.aspx");
            }

        }

        #endregion


    

     


      




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