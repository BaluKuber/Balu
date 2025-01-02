
#region Page Header
//Module Name      :   LoanAdmin
//Screen Name      :   S3GLoanAdSysJournal_Add.aspx
//Created By       :   Kaliraj K
//Created Date     :   29-Apr-2011
//Purpose          :   To Query Sys Journal Details

#endregion

using System;
using System.Web.UI;
using System.Data;
using System.Text;
using S3GBusEntity.LoanAdmin;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Resources;

public partial class S3GLoanAdSysJournal : ApplyThemeForProject
{
    #region initialization

    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjSJVClient;
    LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalDataTable ObjSJVClientDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;

    int intErrCode = 0;
    int intSJVID = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int intRowId = 0;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    //Code end

    DataSet dsSJV = new DataSet();
    Dictionary<string, string> Procparam = null;

    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=MAJ";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdSysJournal_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=MAJ';";
    public static S3GLoanAdSysJournal obj_Page;
    #endregion

    #region PageLoad
    protected new void Page_PreInit(object sender, EventArgs e) //transaction screen page init
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            #region Common Session Values
            //Add by chandru on 23/04/2012
            if (ddlLOB.SelectedValue != "")
            {
                System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = ddlLOB.SelectedValue;
                System.Web.HttpContext.Current.Session["LOBAutoSuggestText"] = ddlLOB.SelectedItem.Text;
            }
            else
            {
                System.Web.HttpContext.Current.Session["LOBAutoSuggestValue"] = null;
            }
            //Add by chandru on 23/04/2012
            #endregion
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyID.ToString();
        //txtSJVValueDate.Attributes.Add("readonly", "readonly");
        //CalendarExtender1.Format = strDateFormat;

        txtSJVDate.Attributes.Add("readonly", "readonly");
        CalendarExtender2.Format = strDateFormat;

        if (Request.QueryString["Popup"] != null)
            btnCancel.Enabled = false;
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));

            if (fromTicket != null)
            {
                intSJVID = Convert.ToInt32(fromTicket.Name);
                //hdnMJVID.Value = intMJVID.ToString();
                strMode = Request.QueryString.Get("qsMode");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }


        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end


        if (!IsPostBack)
        {
            //Validation Msgs from Resource File
            txtSJVDate.Text = DateTime.Now.ToString(strDateFormat);
            //txtSJVValueDate.Text = DateTime.Now.ToString(strDateFormat);
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

            if ((intSJVID > 0) && (strMode == "Q"))
            {
                FunGetSJVDetails();
                FunPriDisableControls(-1);

            }

        }

    }



    #endregion


    #region Page Methods

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetSJVDetails()
    {
        try
        {
            if (intSJVID > 0)
            {
                //FunPriBindLOBBranch();

                strProcName = "S3G_LoanAd_GetSJVDetails";
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@SJV_No", intSJVID.ToString());
                dsSJV = Utility.GetTableValues(strProcName, Procparam);

                ListItem lst = new ListItem(dsSJV.Tables[0].Rows[0]["LOB"].ToString(), dsSJV.Tables[0].Rows[0]["LOB_ID"].ToString());
                ddlLOB.Items.Add(lst);

                lst = new ListItem(dsSJV.Tables[0].Rows[0]["Location"].ToString(), dsSJV.Tables[0].Rows[0]["Location_ID"].ToString());
                ddlBranch.Items.Add(lst);

                ddlLOB.SelectedValue = dsSJV.Tables[0].Rows[0]["LOB_ID"].ToString();
                ddlBranch.SelectedValue = dsSJV.Tables[0].Rows[0]["Location_ID"].ToString();

                txtSJVDate.Text = DateTime.Parse(dsSJV.Tables[0].Rows[0]["SJVDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                txtSJVNo.Text = dsSJV.Tables[0].Rows[0]["SJVNo"].ToString();
                txtNarration.Text = dsSJV.Tables[0].Rows[0]["Narration"].ToString();
                //txtSJVValueDate.Text = DateTime.Parse(dsSJV.Tables[0].Rows[0]["SJVValueDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                grvSysJournal.DataSource = dsSJV.Tables[1];
                grvSysJournal.DataBind();

            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            dsSJV.Dispose();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

    private void FunPriBindLOBBranch()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Program_ID", "77");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            //Branch
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Program_ID", "77");
            ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_Id", "Location_Code", "Location_Name" });


        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    ////This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                if (!bCreate)
                {
                    btnSave.Enabled = false;

                }

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                ddlLOB.Enabled = false;
                ddlBranch.Enabled = false;
                btnClear.Enabled = false;                
                break;

            case -1:// Query Mode


                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }

                if (bClearList)
                {
                    //ddlLOB.ClearDropDownList();
                   // ddlBranch.ClearDropDownList();

                }

                ddlLOB.Enabled = true;
                ddlBranch.Enabled = true;
                //txtSJVValueDate.Enabled = true;
                txtSJVDate.Enabled = true;
                txtSJVNo.Enabled = true;
                txtNarration.ReadOnly = true;
                //txtSJVValueDate.ReadOnly = true;
                txtSJVDate.ReadOnly = true;
                txtSJVNo.ReadOnly = true;
                CalendarExtender2.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                break;
        }

    }

    ////Code end
    #endregion

    // Added By Shibu 24-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "077");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

}



