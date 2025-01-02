#region Page Header
//Module Name      :   Legal And Repossession
//Screen Name      :   S3GLRLegalHearingTrack.aspx
//Created By       :   Anitha V
//Created Date     :   06-May-2011
//Purpose          :   To Insert and Update Legal Hearing Track Details

#endregion
#region Namespace
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
using S3GBusEntity;
using System.ServiceModel;
using System.Text;
using Resources;
using System.Globalization;

#endregion
public partial class LegalRepossession_S3GLRLegalHearingTrack : ApplyThemeForProject
{

    #region VariableDeclaration

    int intCompanyID = 0;
    int intuserID = 0;
    int intLHTID = 0;
    int intErrorCode = 0;
    int intRowId = 0;
    int intcount = 0;
    int intLHRDID = 0;

    //string strFlag = string.Empty;
    string strLHRNO = string.Empty;
    string strDateFormat = string.Empty;
    string strMode = string.Empty;
    string strKey = "Insert";
    string strAlert = "alert('_ALERT_');";
    string strRedirectPage = "../LegalRepossession/S3GLRTransLander.aspx?Code=GLHT";
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRLegalHearingTrack.aspx';";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GLHT';";

    string strLookupType;
    string strLookupCode;
    string strNextHearingDate = string.Empty;
    StringBuilder strXML = new StringBuilder();

    public static LegalRepossession_S3GLRLegalHearingTrack obj_Page;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bClearList = false;

    UserInfo objUserInfo = new UserInfo();
    Dictionary<string, string> proceparam = null;
    S3GSession objS3GSession = new S3GSession();

    DataTable dtLegalHearing = null;
    DataTable dtHistory = null;

    LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient objClient;
    S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackDataTable objDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;


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
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            obj_Page = this;
            FunProLoadPage();
        }

        catch (Exception ex)
        {
            tabcontainer1.ActiveTabIndex = 0;
            lblErrorMessage.Text = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad + this.Page.Header.Title;
        }

    }
    protected void FunProLoadPage()
    {
        try
        {
            intCompanyID = objUserInfo.ProCompanyIdRW;
            intuserID = objUserInfo.ProUserIdRW;
            strDateFormat = objS3GSession.ProDateFormatRW;
            //CalendarExtender1.Format = strDateFormat;
            CalendarExtender3.Format = strDateFormat;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            //txtLHRDate.Attributes.Add("readonly", "readonly");
            //txtLHRNo.Attributes.Add("readonly", "readonly");
            bCreate = objUserInfo.ProCreateRW;
            bModify = objUserInfo.ProModifyRW;
            bQuery = objUserInfo.ProViewRW;

            strMode = Request.QueryString.Get("qsMode");
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intLHTID = Convert.ToInt32(fromTicket.Name);
                    //strMode = Request.QueryString.Get("qsMode");
                }

                else
                {

                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }

            if (grdLegalHearing.FooterRow != null)
            {
                DropDownList ddlNotifyEmployee = (DropDownList)grdLegalHearing.FooterRow.FindControl("ddlNotifyEmployee");
                if (ddlNotifyEmployee != null)
                    ddlNotifyEmployee.AddItemToolTip();
            }
            if (grdLegalHearing.EditIndex == 0)
            {
                //GridViewRow grvRow = grdLegalHearing.Rows[intRowId];
                //TextBox txtActivityDate = (TextBox)grvRow.FindControl("txtActivityDate");

                DropDownList ddlNotifyEmployee = (DropDownList)grdLegalHearing.Rows[0].FindControl("ddlNotifyEmployee");
                if (ddlNotifyEmployee != null)
                    ddlNotifyEmployee.AddItemToolTip();
            }

            ddlLNN.AddItemToolTip();
            if (!IsPostBack)
            {

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                //rfvddlBranch.ErrorMessage = ValidationMsgs.S3G_ValMsg_Branch;
                rfvddlLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
                // txtLHRDate.Attributes.Add("readonly", "readonly");
                if (intLHTID > 0 && strMode == "M")
                {
                    FunPriBindGridLegalHearing();

                    FunPubGetLHTDetails();
                    FunPriDisableControls(1);

                }
                else if (intLHTID > 0 && strMode == "Q")
                {
                    FunPriBindGridLegalHearing();
                    FunPubGetLHTDetails();
                    FunPriDisableControls(-1);

                }
                else
                {
                    FunPriLoadLOBBranch();
                    FunPriBindGridLegalHearing();
                    //FunPriBindGridHistory ();
                    FunBindAdvocateName();
                    FunBindNotifyEmployee();
                    FunPriDisableControls(0);
                    txtLHRDate.Text = DateTime.Now.ToString(strDateFormat);
                    if (grdLegalHearing.FooterRow != null)
                    {
                        TextBox txtActivityDate = (TextBox)grdLegalHearing.FooterRow.FindControl("txtActivityDate");

                        txtActivityDate.Text = DateTime.Now.ToString(strDateFormat);
                        //txtActivityDate.Attributes.Add("readonly", "readonly");
                        TextBox txtNextHearingDate = (TextBox)grdLegalHearing.FooterRow.FindControl("txtNextHearingDate");
                        //txtNextHearingDate.Attributes.Add("readonly", "readonly");
                    }

                }
                if (grdLegalHearing.FooterRow != null)
                {
                    //AjaxControlToolkit.CalendarExtender CalendarExtender4 = grdLegalHearing.FooterRow.FindControl("CalendarExtender4") as AjaxControlToolkit.CalendarExtender;
                    //CalendarExtender4.Format = strDateFormat;
                    AjaxControlToolkit.CalendarExtender CalendarExtender2 = grdLegalHearing.FooterRow.FindControl("CalendarExtender2") as AjaxControlToolkit.CalendarExtender;
                    CalendarExtender2.Format = strDateFormat;
                }

                tabcontainer1.ActiveTabIndex = 0;
            }
        }

        catch (Exception ex)
        {
            throw ex;//lblErrorMessage.Text = ex.ToString();
        }
    }

    #endregion
    #region DisableControls
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0:
                    //Create Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    btnAddNextHearingDetail.Enabled = false;
                    //btnDelete .Enabled =false ;
                    Closure.Enabled = false;
                    History.Enabled = false;
                    break;
                case 1:
                    //Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    //ddlLOB.Enabled = false;
                    //ddlBranch.Enabled = false;
                    //ddlLNN.Enabled = false;
                    //Utility.ClearDropDownList(ddlLOB);
                    //Utility.ClearDropDownList(ddlBranch);
                    //Utility.ClearDropDownList(ddlLNN);

                    btnClear.Enabled = false;
                    txtCaseNo.ReadOnly = true;
                    //CalendarExtender1.Enabled = false;
                    int intcomp = Utility.CompareDates(DateTime.Now.ToString(), hdnNHD.Value.ToString());
                    if (intcomp == 1)
                    {
                        //strFlag = "F";
                        btnAddNextHearingDetail.Enabled = false;
                    }

                    break;
                case -1:
                    //Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }
                    //ddlLOB.Enabled = false;
                    //ddlBranch.Enabled = false;
                    //ddlLNN.Enabled = false;
                    Utility.ClearDropDownList(ddlLOB);
                    //Utility.ClearDropDownList(ddlBranch);
                    Utility.ClearDropDownList(ddlLNN);

                    rbtYes.Enabled = rbtNo.Enabled = false;
                    btnSave.Enabled = false;
                    btnAddNextHearingDetail.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnClear.Enabled = false;
                    //CalendarExtender1.Enabled = false;
                    HideActivityDetailsAction();
                    break;

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    #endregion
    #region Get Legal Hearing Track Details
    private void FunPubGetLHTDetails()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();
            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@LHR_ID", intLHTID.ToString());
            DataSet ds = new DataSet();
            ds = Utility.GetDataset("S3G_LR_GetLHTDetails", proceparam);

            //Header Panel
            txtLHRNo.Text = ds.Tables[0].Rows[0]["LHR_No"].ToString();
            txtLHRDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["LHR_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            FunPriLoadLOBBranch();


            ddlLOB.SelectedValue = ds.Tables[0].Rows[0]["LOB_ID"].ToString();
            ddlBranch.SelectedValue = ds.Tables[0].Rows[0]["Location_ID"].ToString();
            ddlBranch.SelectedText = ds.Tables[0].Rows[0]["Location_Name"].ToString();

            FunBindLRN();

            ddlLNN.SelectedValue = ds.Tables[0].Rows[0]["LRN_ID"].ToString();
            txtLRNDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["LRN_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            txtMLA.Text = ds.Tables[0].Rows[0]["PANum"].ToString();
            txtSLA.Text = ds.Tables[0].Rows[0]["SANum"].ToString();

            //Customer Details
            hdnCustomerId.Value = ds.Tables[0].Rows[0]["Customer_ID"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(ds.Tables[1].Rows[0], true);

            //Case Details
            txtCaseNo.Text = ds.Tables[0].Rows[0]["Case_No"].ToString();
            txtCaseDetails.Text = ds.Tables[0].Rows[0]["Case_Details"].ToString();
            txtCourtDetails.Text = ds.Tables[0].Rows[0]["Court_Details"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

            //Advocate Details
            FunBindAdvocateName();
            ddlAdvocateName.SelectedValue = ds.Tables[0].Rows[0]["Entity_ID"].ToString();
            txtMobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
            txtPhone.Text = ds.Tables[0].Rows[0]["Telephone"].ToString();
            txtEMail.Text = ds.Tables[0].Rows[0]["EMail"].ToString();
            chkActive.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Is_Active"].ToString());

            //Bind ActivityDetails Grid
            hdnNHD.Value = ds.Tables[2].Rows[0]["NextHearingDate"].ToString();
            grdLegalHearing.DataSource = ds.Tables[2];
            grdLegalHearing.DataBind();



            ViewState["LegalHearingDetails"] = ds.Tables[2];
            grdLegalHearing.FooterRow.Visible = false;

            //***Commented at bug fixing *****//
            //if (chkActive.Checked == false)
            //{
            //    btnSave.Enabled = false;
            //    btnAddNextHearingDetail.Enabled = false;
            //    //btnDelete.Enabled = false;
            //    HideActivityDetailsAction();
            //    //strFlag = "F";
            //}
            //*******Commented****** //

            //Bind History Details Grid

            grvHistory.DataSource = ds.Tables[3];
            grvHistory.DataBind();
            ViewState["HistoryDetails"] = ds.Tables[3];
            grvHistory.EmptyDataText = "No History Details available";

            //Modified by Kavitha//  

            ////closure Details
            //if (ds.Tables[0].Rows[0]["Closure_Date"].ToString() != "")
            //{
            //    txtClosureDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //    btnSave.Enabled = false;
            //    btnAddNextHearingDetail.Enabled = false;
            //    HideActivityDetailsAction();
            //    //strFlag = "F";

            //}
            //txtDegree.Text = ds.Tables[0].Rows[0]["Decree"].ToString();
            //txtRemarksC.Text = ds.Tables[0].Rows[0]["Closure_Remarks"].ToString();

            //closure Details

            strLookupType = ds.Tables[0].Rows[0]["Legal_Status_Type_Code"].ToString();
            strLookupCode = ds.Tables[0].Rows[0]["Legal_Status_Code"].ToString();

            if (strLookupType == "6")
            {
                if (strLookupCode == "3")
                {
                    txtClosureDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    //ds.Tables[0].Rows[0]["Closure_Date"].ToString();
                    txtDegree.Text = ds.Tables[0].Rows[0]["Decree"].ToString();
                    txtRemarksC.Text = ds.Tables[0].Rows[0]["Closure_Remarks"].ToString();
                    btnSave.Enabled = false;
                    btnAddNextHearingDetail.Enabled = false;
                    HideActivityDetailsAction();
                    rbtYes.Checked = true;
                    rbtNo.Checked = rbtYes.Enabled = rbtNo.Enabled = false;

                    //TextBox txtActivity = (TextBox)grdLegalHearing.Rows[0].FindControl("txtActivity");
                    //TextBox txtActualPoints = (TextBox)grdLegalHearing.Rows[0].FindControl("txtActualPoints");
                    //txtActualPoints.ReadOnly 
                }
                else
                {
                    // btnSave.Enabled = false;
                    // btnAddNextHearingDetail.Enabled = false;
                    // HideActivityDetailsAction();
                    rbtYes.Checked = false;
                    rbtNo.Checked = rbtYes.Enabled = rbtNo.Enabled = true;
                }

            }
            //Modified by Kavitha//  

            if (strMode == "M" || strMode == "Q")
            {
                Utility.ClearDropDownList(ddlLOB);
                //Utility.ClearDropDownList(ddlBranch);
                ddlBranch.Enabled = false;
                Utility.ClearDropDownList(ddlLNN);
            }
        }
        catch (Exception ex)
        {
            throw ex; //lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region LOadLOBBranch

    private void FunPriLoadLOBBranch()
    {
        try
        {
            //Bind LOB

            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            if (strMode == "C")
                proceparam.Add("@Is_Active", "1");
            proceparam.Add("@User_Id", intuserID.ToString());
            proceparam.Add("@Program_ID", "153");
            // proceparam.Add("@FilterOption", "'FL','LN','HP','OL'");
            ddlLOB.BindDataTable(SPNames.LOBMaster, proceparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            proceparam.Clear();

            if (Request.QueryString.Get("qsMode") != "C")
            {
                //Bind Branch

                proceparam = new Dictionary<string, string>();
                proceparam.Add("@Company_id", intCompanyID.ToString());
                proceparam.Add("@User_Id", intuserID.ToString());
                if (strMode == "C")
                    proceparam.Add("@Is_Active", "1");
                proceparam.Add("@Program_ID", "153");
                proceparam.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, proceparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            }
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
                FunResetMLASLACustomerDetails();
                FunBindLRN();

            }
            else
            {
                ddlLOB.SelectedIndex = 0;
                FunBindLRN();
            }

            if (ddlBranch.SelectedValue !="")
            {
                //ddlBranch.SelectedIndex = 1;
                FunResetMLASLACustomerDetails();
                FunBindLRN();
            }
            else
            {
                //ddlBranch.SelectedIndex = 0;
                FunBindLRN();
            }
        }
        catch (Exception ex)
        {
            throw ex; //lblErrorMessage.Text = ex.ToString();
        }

    }
    #endregion

    #region BindGridLegalHearing

    private void FunPriBindGridLegalHearing()
    {
        try
        {
            dtLegalHearing = new DataTable();

            DataRow dr;

            dtLegalHearing.Columns.Add("ActivityDate");
            dtLegalHearing.Columns.Add("ActivityDone");
            dtLegalHearing.Columns.Add("ActualPoints");
            dtLegalHearing.Columns.Add("NotifyEmployee");
            dtLegalHearing.Columns.Add("NextHearingDate");
            dtLegalHearing.Columns.Add("NotifyEmployeeValue");
            dtLegalHearing.Columns.Add("Mode");
            dtLegalHearing.Columns.Add("ID");
            dr = dtLegalHearing.NewRow();

            dtLegalHearing.Rows.Add(dr);

            grdLegalHearing.DataSource = dtLegalHearing;
            grdLegalHearing.DataBind();

            ViewState["LegalHearingDetails"] = dtLegalHearing;

            grdLegalHearing.Rows[0].Visible = false;
            if (grdLegalHearing.FooterRow != null)
            {
                TextBox txtActivityDate = (TextBox)grdLegalHearing.FooterRow.FindControl("txtActivityDate");

                txtActivityDate.Text = DateTime.Now.ToString(strDateFormat);
                //txtActivityDate.Attributes.Add("readonly", "readonly");
                TextBox txtNextHearingDate = (TextBox)grdLegalHearing.FooterRow.FindControl("txtNextHearingDate");
                //txtNextHearingDate.Attributes.Add("readonly", "readonly");
            }


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intuserID.ToString());
        Procparam.Add("@Program_Id", "141");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    #region BindGridHistory

    private void FunPriBindGridHistory()
    {
        try
        {
            dtLegalHearing = new DataTable();

            DataRow dr;

            dtLegalHearing.Columns.Add("LHRNo");
            dtLegalHearing.Columns.Add("Date");
            dtLegalHearing.Columns.Add("ActivityDone");
            dtLegalHearing.Columns.Add("ActualPoints");
            dtLegalHearing.Columns.Add("NotifyEmployee");
            dtLegalHearing.Columns.Add("NextHearingDate");

            dr = dtLegalHearing.NewRow();

            dtLegalHearing.Rows.Add(dr);

            grvHistory.DataSource = dtLegalHearing;
            grvHistory.DataBind();
            grvHistory.EmptyDataText = "No History Details available";

            ViewState["LegalHearingDetails"] = dtLegalHearing;

            grvHistory.Rows[0].Visible = false;

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion



    #region BindLRNNumber
    private void FunBindLRN()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();

            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());

            //proceparam.Add("@Branch_ID", ddlBranch.SelectedValue.ToString());

            proceparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            proceparam.Add("@Action", "1");
            proceparam.Add("@ProgramCode", "GLHT");
            if (intLHTID == 0)
                proceparam.Add("@Mode", "C");

            ddlLNN.BindDataTable("S3G_LR_GetLRNNumber", proceparam, new string[] { "LRN_ID", "LRN_No", "Customer_Name" });

            if (ddlLNN.Items.Count == 2)
            {
                ddlLNN.SelectedIndex = 1;
                FunBindMLASLACustomer();
            }
            else
                ddlLNN.SelectedIndex = 0;
            ddlLNN.AddItemToolTip();
        }
        catch (Exception ex)
        {

        }
    }
    #endregion
    #region ResetMLASLACustomerDetails
    private void FunResetMLASLACustomerDetails()
    {
        try
        {
            txtMLA.Text = "";
            txtSLA.Text = "";
            txtLRNDate.Text = "";
            S3GCustomerAddress1.ClearCustomerDetails();
            //PanelCustomerDetails.Visible = false;    
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion

    ////#region BindMLA

    //private void FunBindMLA()
    //{
    //    try
    //    {
    //        if (proceparam != null)
    //            proceparam.Clear();

    //        proceparam = new Dictionary<string, string>();
    //        proceparam.Add("@Type", "1");
    //        proceparam.Add("@LRN_No", ddlLNN.SelectedValue);
    //        ddlMLA.BindDataTable("S3G_LR_GetLegalHearingMLASLACustomer", proceparam, new string[] { "PANum", "PANum" });
    //       // ddlSLA.BindDataTable("S3G_LR_GetLegalHearingMLASLACustomer", proceparam, new string[] { "SANum", "SANum" });
    //        //int count = ddlSLA.Items.Count;

    //        //if (ddlMLA.Items.Count == 2)
    //        //{
    //        //    ddlMLA.SelectedValue = ddlMLA.Items[1].Value;
    //        //}
    //        //if (ddlSLA.Items.Count == 2)
    //        //{
    //        //    ddlSLA.SelectedValue = ddlSLA.Items[1].Value;
    //        //}

    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage .Text =ex.ToString ();
    //    }

    //}
    //#endregion
    //#region BindSLA
    //private void FunBindSLA()
    //{
    //    try
    //    {
    //        if (proceparam != null)
    //            proceparam.Clear();

    //        proceparam = new Dictionary<string, string>();
    //        proceparam.Add("@Type", "2");
    //        proceparam.Add("@LRN_No", ddlLNN.SelectedValue);
    //        proceparam.Add("@PANum", ddlMLA.SelectedValue);

    //        ddlSLA.BindDataTable("S3G_LR_GetLegalHearingMLASLACustomer", proceparam, new string[] { "SANum", "SANum" });
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.ToString();
    //    }
    //}
    //#endregion


    #region BindAdvocateName
    private void FunBindAdvocateName()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();

            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            ddlAdvocateName.BindDataTable("S3G_LR_GetEntityName", proceparam, new string[] { "Entity_ID", "Entity_Name" });

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion

    #region BindAdvocateDetails
    private void FunBindAdvocateDetails()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();

            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@Entity_ID", ddlAdvocateName.SelectedValue);

            DataSet ds = new DataSet();
            if (Convert.ToInt32(ddlAdvocateName.SelectedValue) > 0)
            {
                ds = Utility.GetDataset("S3G_LR_GetEntityName", proceparam);

                if (ds != null)
                {
                    txtMobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    txtPhone.Text = ds.Tables[0].Rows[0]["Telephone"].ToString();
                    txtEMail.Text = ds.Tables[0].Rows[0]["EMail"].ToString();

                }
            }
            else
            {
                txtMobile.Text = "";
                txtPhone.Text = "";
                txtEMail.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion

    #region BindNotifyEmployee
    private void FunBindNotifyEmployee()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();

            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@IsActive", "1");

            DropDownList ddlNotifyEmployee = (DropDownList)grdLegalHearing.FooterRow.FindControl("ddlNotifyEmployee");

            ddlNotifyEmployee.BindDataTable("S3G_LR_GetNotifyEmployee", proceparam, new string[] { "User_ID", "User_Code", "User_Name" });
            ddlNotifyEmployee.AddItemToolTip();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region BindMLASLACustomerDetails
    private void FunBindMLASLACustomer()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();

            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@LRN_ID", ddlLNN.SelectedValue.ToString());

            DataSet ds = new DataSet();
            if (Convert.ToInt32(ddlLNN.SelectedValue) > 0)
            {
                ds = Utility.GetDataset("S3G_LR_GetLegalHearingMLASLACustomer", proceparam);

                if (ds != null)
                {

                    txtMLA.Text = ds.Tables[0].Rows[0]["PANum"].ToString();   //Display Prime Account Number
                    if (ds.Tables[0].Rows[0]["SANum"] != null)
                    {
                        txtSLA.Text = ds.Tables[0].Rows[0]["SANum"].ToString();  //Display Sub Account Number

                    }
                    else
                    {
                        txtSLA.Enabled = false;
                    }
                    txtLRNDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["LRN_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                    hdnCustomerId.Value = ds.Tables[0].Rows[0]["Customer_ID"].ToString();
                    //hdncutoff.Value = ds.Tables[0].Rows[0]["LR_Cutoff_Date"].ToString();
                    // PanelCustomerDetails.Visible = true;
                    S3GCustomerAddress1.SetCustomerDetails(ds.Tables[1].Rows[0], true);
                    hdncheckdate.Value = DateTime.Parse(ds.Tables[2].Rows[0]["CheckDate"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    //ds.Tables[2].Rows[0]["CheckDate"].ToString();

                    hdnEntry.Value = ds.Tables[2].Rows[0]["Record"].ToString();
                }
            }
            else
            {
                txtLRNDate.Text = "";
                txtMLA.Text = "";
                txtSLA.Text = "";
                S3GCustomerAddress1.ClearCustomerDetails();
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region GenerateXMLDetails
    private string FunPubFormXMLDetails()
    {
        try
        {


            strXML.Append("<Root>");
            foreach (GridViewRow grvRow in grdLegalHearing.Rows)
            {
                string strActivityDate = ((Label)grvRow.FindControl("lblDate")).Text.Trim();
                string strActivityDateValue = Utility.StringToDate(strActivityDate).ToString();
                //string strActivity = ((Label)grvRow.FindControl("lblActivity")).Text;
                string strActivity = ((TextBox)grvRow.FindControl("txtActivity")).Text;
                //string strActualPoints = ((Label)grvRow.FindControl("lblActualPoints")).Text;
                string strActualPoints = ((TextBox)grvRow.FindControl("txtActualPoints")).Text;
                string strNotifyEmployee = ((Label)grvRow.FindControl("lblNotifyEmployeeValue")).Text;

                string strNextHearingDate = ((Label)grvRow.FindControl("lblNextHearingDate")).Text.Trim();
                string strNextHearingDateValue = Utility.StringToDate(strNextHearingDate).ToString();
                string strMode = ((Label)grvRow.FindControl("lblMode")).Text;
                string strID = ((Label)grvRow.FindControl("lblID")).Text;
                strXML.Append("<Details Activity_Date='" + strActivityDateValue.ToString() + "' Activity_Done='" + strActivity.ToString() + "' Actual_Point='" + strActualPoints.ToString() + "' Notify_Employee='" + strNotifyEmployee.ToString() + "' Next_Hearing_Date='" + strNextHearingDateValue.ToString() + "' Mode='" + strMode.ToString() + "' ID='" + strID + "'/>");


            }
            strXML.Append("</Root>");


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
        return strXML.ToString();
    }
    #endregion

    //#region BindCustomerDetails

    //private void FunBindCustomerDetails()
    //{
    //    try
    //    {
    //        if (proceparam != null)
    //            proceparam.Clear();

    //        proceparam = new Dictionary<string, string>();
    //        proceparam.Add("@Type", "3");
    //        proceparam .Add ("@LRN_No",ddlLNN .SelectedValue .ToString ());
    //        proceparam .Add ("@PANum",ddlMLA .SelectedValue .ToString ());
    //        if (ddlSLA.Items .Count >1)
    //        {
    //            proceparam.Add("@SANum", ddlSLA.SelectedValue.ToString());
    //        }
    //        DataSet ds = new DataSet();
    //        ds = Utility.GetDataset("S3G_LR_GetLegalHearingMLASLACustomer", proceparam);

    //        S3GCustomerAddress1.SetCustomerDetails(ds.Tables[0].Rows[0], true);
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.ToString();
    //    }
    //}

    //#endregion
    #region SelectedIndexChangedEvents
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunResetMLASLACustomerDetails();
            FunBindLRN();

            //Bind Branch

            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_id", intCompanyID.ToString());
            proceparam.Add("@User_Id", intuserID.ToString());
            if (strMode == "C")
                proceparam.Add("@Is_Active", "1");
            proceparam.Add("@Program_ID", "153");
            proceparam.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, proceparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunResetMLASLACustomerDetails();
            FunBindLRN();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }

    }

    protected void ddlLNN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //FunBindMLA();
            //string strex = ddlLNN.SelectedItem.Text.Substring(0, ddlLNN.SelectedItem.Text.LastIndexOf("-")).Trim ().ToString ();
            FunBindMLASLACustomer();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void ddlAdvocateName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunBindAdvocateDetails();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region GridViewEvents
    #region GridRowCommand
    protected void grdLegalHearing_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            TextBox txtActivityDate = (TextBox)grdLegalHearing.FooterRow.FindControl("txtActivityDate");
            TextBox txtActivity = (TextBox)grdLegalHearing.FooterRow.FindControl("txtActivity");
            TextBox txtActualPoints = (TextBox)grdLegalHearing.FooterRow.FindControl("txtActualPoints");
            DropDownList ddlNotifyEmployee = (DropDownList)grdLegalHearing.FooterRow.FindControl("ddlNotifyEmployee");
            TextBox txtNextHearingDate = (TextBox)grdLegalHearing.FooterRow.FindControl("txtNextHearingDate");

            if (e.CommandName == "Add")
            {
                dtLegalHearing = (DataTable)ViewState["LegalHearingDetails"];

                DataRow dr;
                if (dtLegalHearing.Rows[0]["ActivityDone"].ToString() == "")
                {
                    dtLegalHearing.Rows.RemoveAt(0);
                }
                dr = dtLegalHearing.NewRow();
                dr["ActivityDate"] = txtActivityDate.Text.Trim();
                dr["ActivityDone"] = txtActivity.Text.Trim();
                dr["ActualPoints"] = txtActualPoints.Text.Trim();
                dr["NotifyEmployee"] = ddlNotifyEmployee.SelectedItem.Text.ToString();
                dr["NotifyEmployeeValue"] = ddlNotifyEmployee.SelectedValue;
                dr["NextHearingDate"] = txtNextHearingDate.Text.Trim();

                if (intLHTID == 0)
                {
                    dr["Mode"] = "C";
                    dr["ID"] = "0";
                }
                else
                {
                    int intcomp = Utility.CompareDates(txtNextHearingDate.Text.ToString(), hdnaddNHD.Value.ToString());
                    if (intcomp == 1)
                    {

                        Utility.FunShowAlertMsg(this, "Next hearing Date should be greater than Last Hearing Date");
                        FunPriBindGridLegalHearing();
                        //grdLegalHearing.FooterRow.Visible = false;
                        return;
                    }
                    else
                    {
                        dr["Mode"] = "I";
                        dr["ID"] = "0";

                    }
                }

                dtLegalHearing.Rows.Add(dr);

                grdLegalHearing.DataSource = dtLegalHearing;
                grdLegalHearing.DataBind();

                ViewState["LegalHearingDetails"] = dtLegalHearing;
                grdLegalHearing.FooterRow.Visible = false;
                if (intLHTID > 0)
                {
                    //strFlag = "F";

                    btnAddNextHearingDetail.Enabled = false;
                }

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region GridviewRowEditing
    protected void grdLegalHearing_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            grdLegalHearing.EditIndex = e.NewEditIndex;
            //intRowId = Convert.ToInt32(grdLegalHearing.DataKeys[e.NewEditIndex].Value.ToString()) ;
            intRowId = e.NewEditIndex;
            dtLegalHearing = (DataTable)ViewState["LegalHearingDetails"];
            grdLegalHearing.DataSource = dtLegalHearing;
            grdLegalHearing.DataBind();
            GridViewRow grvRow = grdLegalHearing.Rows[intRowId];
            TextBox txtActivityDate = (TextBox)grvRow.FindControl("txtActivityDate");
            //txtActivityDate.Attributes.Add("readonly", "readonly");
            TextBox txtActivity = (TextBox)grvRow.FindControl("txtActivity");
            TextBox txtActualPoints = (TextBox)grvRow.FindControl("txtActualPoints");
            DropDownList ddlNotifyEmployee = (DropDownList)grvRow.FindControl("ddlNotifyEmployee");
            TextBox txtNextHearingDate = (TextBox)grvRow.FindControl("txtNextHearingDate");
            //txtNextHearingDate.Attributes.Add("readonly", "readonly");

            AjaxControlToolkit.CalendarExtender CalendarExtender2 = grvRow.FindControl("CalendarExtender2") as AjaxControlToolkit.CalendarExtender;
            CalendarExtender2.Format = strDateFormat;

            txtActivityDate.Text = dtLegalHearing.Rows[intRowId]["ActivityDate"].ToString();
            txtActivity.Text = dtLegalHearing.Rows[intRowId]["ActivityDone"].ToString();
            txtActualPoints.Text = dtLegalHearing.Rows[intRowId]["ActualPoints"].ToString();
            hdnLHRDID.Value = dtLegalHearing.Rows[intRowId]["ID"].ToString();

            if (proceparam != null)
                proceparam.Clear();

            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@IsActive", "1");
            proceparam.Add("@option", "1");
            proceparam.Add("@USER_ID", dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString());

            ddlNotifyEmployee.BindDataTable("S3G_LR_GetNotifyEmployee", proceparam, new string[] { "User_ID", "User_Code", "User_Name" });
            ddlNotifyEmployee.AddItemToolTip();
            //ddlNotifyEmployee.SelectedItem.Text = dtLegalHearing.Rows[intRowId]["NotifyEmployee"].ToString();
            //***************************//
            //DataTable dt = Utility.GetDefaultData("S3G_LR_GetNotifyEmployee", proceparam);
            //bool flag = false;
            //System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem(dtLegalHearing.Rows[intRowId]["NotifyEmployee"].ToString(), dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString());
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["User_ID"].ToString() == dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString())
            //    {
            //        flag = true;
            //        liSelect = new System.Web.UI.WebControls.ListItem(dt.Rows[intRowId]["NotifyEmployee"].ToString(), dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString());
            //    }
            //}

            //if (flag == true)
            //    ddlNotifyEmployee.SelectedValue = dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString();
            //else
            //{
            //    //System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem(dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString(), dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString());

            //    ddlNotifyEmployee.Items.Insert(1, liSelect);
            //    ddlNotifyEmployee.SelectedValue = dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString();
            //}    

            //***************************//

            ddlNotifyEmployee.SelectedValue = dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"].ToString();

            txtNextHearingDate.Text = dtLegalHearing.Rows[intRowId]["NextHearingDate"].ToString();
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            grdLegalHearing.FooterRow.Visible = false;

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region GridviewRowUpdating
    protected void grdLegalHearing_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            intRowId = e.RowIndex;
            dtLegalHearing = (DataTable)ViewState["LegalHearingDetails"];

            GridViewRow grvRow = grdLegalHearing.Rows[intRowId];
            TextBox txtActivityDate = (TextBox)grvRow.FindControl("txtActivityDate");
            TextBox txtActivity = (TextBox)grvRow.FindControl("txtActivity");
            TextBox txtActualPoints = (TextBox)grvRow.FindControl("txtActualPoints");
            DropDownList ddlNotifyEmployee = (DropDownList)grvRow.FindControl("ddlNotifyEmployee");
            TextBox txtNextHearingDate = (TextBox)grvRow.FindControl("txtNextHearingDate");


            dtLegalHearing.Rows[intRowId]["ActivityDate"] = txtActivityDate.Text.Trim();
            dtLegalHearing.Rows[intRowId]["ActivityDone"] = txtActivity.Text.Trim();
            dtLegalHearing.Rows[intRowId]["ActualPoints"] = txtActualPoints.Text.Trim();
            dtLegalHearing.Rows[intRowId]["NotifyEmployee"] = ddlNotifyEmployee.SelectedItem.Text.ToString();
            dtLegalHearing.Rows[intRowId]["NextHearingDate"] = txtNextHearingDate.Text;
            dtLegalHearing.Rows[intRowId]["NotifyEmployeeValue"] = ddlNotifyEmployee.SelectedValue;
            intLHRDID = Convert.ToInt32(hdnLHRDID.Value.ToString());
            if (intLHRDID == 0)
            {
                dtLegalHearing.Rows[intRowId]["Mode"] = "I";
            }
            else
            {
                dtLegalHearing.Rows[intRowId]["Mode"] = "U";
            }

            grdLegalHearing.EditIndex = -1;
            //btnAddNextHearingDetail.Enabled = false;
            //strGridMode = "U";
            grdLegalHearing.DataSource = dtLegalHearing;
            grdLegalHearing.DataBind();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            if (intLHTID > 0)
            {
                btnClear.Enabled = false;
            }
            grdLegalHearing.FooterRow.Visible = false;
            ViewState["LegalHearingDetails"] = dtLegalHearing;
            int count = dtLegalHearing.Rows.Count;
            Label lblNextHearingDate = (Label)grdLegalHearing.Rows[count - 1].FindControl("lblNextHearingDate");
            hdnNHD.Value = lblNextHearingDate.Text.ToString();
            int intcomp = Utility.CompareDates(DateTime.Now.ToString(), hdnNHD.Value.ToString());
            if (intcomp == 1)
            {
                btnAddNextHearingDetail.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region Gridview Caceling Edit
    protected void grdLegalHearing_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grdLegalHearing.EditIndex = -1;
            dtLegalHearing = (DataTable)ViewState["LegalHearingDetails"];
            grdLegalHearing.DataSource = dtLegalHearing;
            grdLegalHearing.DataBind();
            grdLegalHearing.FooterRow.Visible = false;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            if (intLHTID > 0)
            {
                btnClear.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region GridRowDataBound
    protected void grdLegalHearing_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //ddlNotifyEmployee.AddItemToolTip();  

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                Label lblNextHearingDate = e.Row.FindControl("lblNextHearingDate") as Label;

                if (lblNextHearingDate != null)
                {
                    if (!string.IsNullOrEmpty(lblNextHearingDate.Text))
                    {
                        if (Utility.StringToDate(lblNextHearingDate.Text) <= Utility.StringToDate(DateTime.Now.ToString(strDateFormat)))
                        {
                            if (lnkEdit != null)
                                lnkEdit.Enabled = false;
                        }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //if (grdLegalHearing.FooterRow!=null )
                //{
                //    DropDownList ddlNotifyEmployee = (DropDownList)grdLegalHearing.FooterRow.FindControl("ddlNotifyEmployee");
                //    if (ddlNotifyEmployee != null)
                //        ddlNotifyEmployee.AddItemToolTip();
                //}
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();

        }
    }
    #endregion
    #endregion
    //    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        try
    //        {

    //            FunBindSLA();
    //            if (ddlSLA.Items.Count == 1)
    //            {
    //                FunBindCustomerDetails();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            lblErrorMessage.Text = ex.ToString();
    //        }
    //    }
    //    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            FunBindCustomerDetails();
    //        }
    //        catch (Exception ex)
    //        {
    //            lblErrorMessage.Text = ex.ToString();
    //        }

    //    }
    #region ClickEvents
    #region Save
    protected void btnSave_Click(object sender, EventArgs e)
    {
        objClient = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();

        try
        {
            //            vsFooter.Visible = false;
            dtLegalHearing = (DataTable)ViewState["LegalHearingDetails"];
            if (dtLegalHearing.Rows[0]["ActivityDone"].ToString() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Add the Activity Details");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('" + ValidationMsgs.S3G_ValMsg_AddAtleastOneRec + "');", true);
                return;
            }

            //if (intLHTID == 0)
            //{

            //string strEntry = hdnEntry.Value.ToString();
            //if (strEntry == "F")
            //{
            //    int intcheckdatevalue = Utility.CompareDates(txtLHRDate.Text.ToString(), hdncheckdate.Value);
            //    if (intcheckdatevalue == 1)
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "LHR Date should be equal to or greater than LRN Date");
            //        return;
            //    }
            //    else
            //    {

            //    }
            //}
            //else if (strEntry == "A")
            //{
            //    int intcheckdatevalue = Utility.CompareDates(txtLHRDate.Text.ToString(), hdncheckdate.Value);
            //    if (intcheckdatevalue != -1)
            //    {
            //    Utility.FunShowAlertMsg(this.Page, "LHR Date should be greater than Last LHR Date");
            //    return;
            //    }
            //}

            //int intcheckLRNDate = Utility.CompareDates(txtLHRDate.Text.ToString(), hdncutoff.Value);
            //if (intcheckLRNDate == -1)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "LHR Date should not greater thn LRN Cut-Off Date");
            //    return;
            //}
            //}

            objDataTable = new S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackDataTable();

            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackRow objRow;

            objRow = objDataTable.NewS3G_LR_LegalHearingTrackRow();

            objRow.LHR_ID = intLHTID;

            objRow.Company_ID = intCompanyID;
            objRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objRow.Customer_ID = Convert.ToInt32(hdnCustomerId.Value);
            objRow.Advocate_ID = Convert.ToInt32(ddlAdvocateName.SelectedValue);
            objRow.LRN_ID = Convert.ToInt32(ddlLNN.SelectedValue);
            objRow.LRN_No = ddlLNN.SelectedItem.Text.Substring(0, ddlLNN.SelectedItem.Text.LastIndexOf("-")).Trim().ToString();
            objRow.LHR_Date = Utility.StringToDate(txtLHRDate.Text.Trim());
            objRow.PANum = txtMLA.Text.ToString();
            objRow.SANum = txtSLA.Text.ToString();
            objRow.Case_No = txtCaseNo.Text;
            //objRow.Case_No = txtCaseNo.Text;//Convert.ToInt32(txtCaseNo.Text.Trim());
            objRow.Case_Details = txtCaseDetails.Text.Trim();
            objRow.Court_Details = txtCourtDetails.Text.Trim();
            objRow.Remarks = txtRemarks.Text.Trim();
            // Modified by kavitha//
            //if (txtClosureDate.Text.ToString() != "")
            //{
            //    //int intcount = dtLegalHearing.Rows.Count;
            //    //hdnLHD.Value = dtLegalHearing.Rows[intcount]["NextHearingDate"].ToString();
            //    int count = dtLegalHearing.Rows.Count;
            //    Label lblNextHearingDate = (Label)grdLegalHearing.Rows[count - 1].FindControl("lblNextHearingDate");
            //    hdnLHD.Value = lblNextHearingDate.Text.ToString();
            //    int intResult = Utility.CompareDates(txtClosureDate.Text.ToString(), hdnLHD.Value);
            //    if (intResult == -1)
            //    {
            //        objRow.Closure_Date = Utility.StringToDate(txtClosureDate.Text.Trim());
            //        objRow.Decree = txtDegree.Text.ToString();
            //        objRow.Closure_Remarks = txtRemarksC.Text.ToString();
            //    }
            //    else
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "Closure Date should be greater than the Last Hearing Date");
            //        return;
            //    }


            //}


            //if (txtDegree.Text.ToString() != "")
            if (rbtYes.Checked == true)
            {
                int count = dtLegalHearing.Rows.Count;
                Label lblNextHearingDate = (Label)grdLegalHearing.Rows[count - 1].FindControl("lblNextHearingDate");
                hdnLHD.Value = lblNextHearingDate.Text.ToString();
                int intResult = Utility.CompareDates(txtClosureDate.Text.ToString(), hdnLHD.Value);
                if (intResult <= 0)
                {
                    objRow.ClosureFlag = 1;
                    objRow.Closure_Date = Utility.StringToDate(txtClosureDate.Text.Trim());
                    objRow.Decree = txtDegree.Text.Trim();
                    objRow.Closure_Remarks = txtRemarksC.Text.Trim();
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Closure Date should be greater than the Last Hearing Date");
                    return;
                }


            }
            else
            {
                objRow.ClosureFlag = 0;
                objRow.Closure_Date = Utility.StringToDate(DateTime.Now.ToString(strDateFormat));
                objRow.Decree = null;// txtDegree.Text.ToString();
                objRow.Closure_Remarks = null;// txtRemarksC.Text.ToString();

            }


            // Modified by kavitha//


            // objRow.Decree = "";
            //objRow.Closure_Remarks = "";


            objRow.XmlActivityDetails = FunPubFormXMLDetails();
            objRow.Created_By = intuserID;
            objRow.Is_Active = chkActive.Checked;
            if (intLHTID > 0)
            {
                objRow.LHR_No = txtLHRNo.Text.ToString();
                objRow.DeleteKey = 0;
            }
            objDataTable.AddS3G_LR_LegalHearingTrackRow(objRow);
            if (intLHTID == 0)
            {

                intErrorCode = objClient.FunPubCreateLegalHearingTrack(out strLHRNO, SerMode, ClsPubSerialize.Serialize(objDataTable, SerMode));
            }
            else
            {
                intErrorCode = objClient.FunPubModifyLegalHearingTrack(SerMode, ClsPubSerialize.Serialize(objDataTable, SerMode));

            }
            if (intErrorCode == 0)
            {
                if (intLHTID > 0)
                {
                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    Utility.FunShowAlertMsg(this.Page, "Legal Hearing Track " + ValidationMsgs.S3G_ValMsg_Update, strRedirectPage);
                    return;
                }
                else
                {

                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    //strAlert = "Legal Hearing Track " + strLHRNO + " " + ValidationMsgs.S3G_ValMsg_Save;
                    //strAlert += @"\n" + ValidationMsgs.S3G_ValMsg_Next + " Legal Hearing Track ";

                    strAlert = "Legal Hearing Track details added successfully - " + strLHRNO;
                    strAlert += @"\n" + ValidationMsgs.S3G_ValMsg_Next;
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    return;
                }

            }
            else
            {
                if ((intErrorCode == -1) || (intErrorCode == -2) || (intErrorCode == 50))
                    Utility.FunShowValidationMsg(this.Page, "", intErrorCode);
                else
                    Utility.FunShowValidationMsg(this.Page, "LegalhearingTrack", intErrorCode);
                return;

            }



        }

        catch (Exception ex)
        {
            // lblErrorMessage.Text = ex.ToString();
            lblErrorMessage.Text = Resources.ValidationMsgs.S3G_ErrMsg_InsertUpdate + this.Page.Header.Title;
            //"Unable to save the details";
        }
        finally
        {
            objClient.Close();
        }

    }
    #endregion

    #region Cancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = "Unable to cancel";
        }

    }
    #endregion
    #region Clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClear();


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = Resources.ValidationMsgs.S3G_ErrMsg_Clear + this.Page.Header.Title;
        }
    }

    private void FunPriClear()
    {
        try
        {
            //FunProLoadPage();
            S3GCustomerAddress1.ClearCustomerDetails();
            FunPriLoadLOBBranch();
            ddlBranch.Clear();
            // txtLHRDate.Text=
            txtLRNDate.Text = txtCaseDetails.Text = txtCaseNo.Text = txtCourtDetails.Text = "";
            txtClosureDate.Text = txtDegree.Text = txtRemarks.Text = txtRemarksC.Text = txtMLA.Text = txtSLA.Text = "";
            //S3GCustomerAddress1.ClearCustomerDetails();

            //ddlLOB.SelectedIndex = 0;
            // ddlBranch.SelectedIndex = 0;
            ddlAdvocateName.SelectedIndex = 0;
            //if (ddlLNN.Items.Count > 0)
            //{
            //    ddlLNN.ClearDropDownList();
            //}
            txtMobile.Text = txtPhone.Text = txtEMail.Text = "";

            grdLegalHearing.DataSource = null;
            grdLegalHearing.DataBind();
            FunPriBindGridLegalHearing();
            FunBindNotifyEmployee();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion

    #region AddNextHearingDetails
    protected void btnAddNextHearingDetail_Click(object sender, EventArgs e)
    {
        try
        {


            //objClient = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();
            //objDataTable = new S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackDataTable();

            //S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackRow objRow;

            //objRow = objDataTable.NewS3G_LR_LegalHearingTrackRow();

            //objRow.LHR_ID = intLHTID;

            //objRow.Company_ID = intCompanyID;
            //objRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            //objRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            //objRow.Customer_ID = Convert.ToInt32(hdnCustomerId.Value);
            //objRow.Advocate_ID = Convert.ToInt32(ddlAdvocateName.SelectedValue);
            //objRow.LRN_ID = Convert.ToInt32(ddlLNN.SelectedValue);
            //objRow.LRN_No = ddlLNN.SelectedItem.Text.Substring(0, ddlLNN.SelectedItem.Text.LastIndexOf("-")).Trim().ToString();
            //objRow.LHR_Date = Utility.StringToDate(txtLHRDate.Text.Trim());
            //objRow.PANum = txtMLA.Text.ToString();
            //objRow.SANum = txtSLA.Text.ToString();
            //objRow.Case_No = txtCaseNo.Text.Trim();
            //objRow.Case_Details = txtCaseDetails.Text.Trim();
            //objRow.Court_Details = txtCourtDetails.Text.Trim();
            //objRow.Remarks = txtRemarks.Text.Trim();

            //if (rbtYes.Checked == true)
            //{
            //    objRow.ClosureFlag = 1;
            //    objRow.Closure_Date = Utility.StringToDate(txtClosureDate.Text.Trim());
            //}
            //else
            //{
            //    objRow.ClosureFlag = 0;
            //    objRow.Closure_Date = Utility.StringToDate(DateTime.Now.ToString(strDateFormat));
            //}
            ////if (!string .IsNullOrEmpty (txtClosureDate.Text))
            ////{
            ////    objRow.Closure_Date = Utility.StringToDate(txtClosureDate.Text.Trim());
            ////}
            ////else
            ////    objRow.Closure_Date =null;
            //objRow.Decree = txtDegree.Text.Trim();
            //objRow.Closure_Remarks = txtRemarksC.Text.Trim();

            //objRow.XmlActivityDetails = FunPubFormXMLDetails();
            //objRow.Created_By = intuserID;
            ////chkActive.Checked = false;
            //objRow.Is_Active = chkActive.Checked;
            //objRow.LHR_No = txtLHRNo.Text.ToString();
            //objRow.DeleteKey = 0;

            //objDataTable.AddS3G_LR_LegalHearingTrackRow(objRow);
            //intErrorCode = objClient.FunPubModifyLegalHearingTrack(SerMode, ClsPubSerialize.Serialize(objDataTable, SerMode));

            //if (intErrorCode == 0)
            //{
            if (proceparam != null)
                proceparam.Clear();
            //proceparam = new Dictionary<string, string>();
            //proceparam.Add("@Company_ID", intCompanyID.ToString());
            //proceparam.Add("@LHR_ID", intLHTID.ToString());
            ////proceparam.Add("@option", "2");
            //DataSet ds = new DataSet();
            //ds = Utility.GetDataset("S3G_LR_GetLHTDetails", proceparam);
            //hdnaddNHD.Value = ds.Tables[4].Rows[0]["NextHearingDate"].ToString();
            //grvHistory.DataSource = ds.Tables[4];
            //grvHistory.DataBind();
            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@LHR_ID", intLHTID.ToString());
            proceparam.Add("@option", "1");
            DataSet ds = new DataSet();
            ds = Utility.GetDataset("S3G_LR_GetLHTDetails", proceparam);
            hdnaddNHD.Value = ds.Tables[0].Rows[0]["NextHearingDate"].ToString();
            grvHistory.DataSource = ds.Tables[0];
            grvHistory.DataBind();
            grvHistory.EmptyDataText = "No History Details available";
            //}
            //else
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Not able to insert Next Hearing Details");
            //    return;
            //}
            FunPriBindGridLegalHearing();
            grdLegalHearing.FooterRow.Visible = true;
            FunBindNotifyEmployee();

            //dtLegalHearing = (DataTable)ViewState["LegalHearingDetails"];
            //grdLegalHearing.DataSource = dtLegalHearing;
            //grdLegalHearing.DataBind();
            //grdLegalHearing.FooterRow.Visible = true;
            //FunBindNotifyEmployee();

            if (grdLegalHearing.FooterRow != null)
            {
                TextBox txtActivityDate = (TextBox)grdLegalHearing.FooterRow.FindControl("txtActivityDate");

                txtActivityDate.Text = DateTime.Now.ToString(strDateFormat);
                txtActivityDate.Attributes.Add("readonly", "readonly");
                TextBox txtNextHearingDate = (TextBox)grdLegalHearing.FooterRow.FindControl("txtNextHearingDate");
                //txtNextHearingDate.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarExtender2 = grdLegalHearing.FooterRow.FindControl("CalendarExtender2") as AjaxControlToolkit.CalendarExtender;
                CalendarExtender2.Format = strDateFormat;
                
                if (txtNextHearingDate != null)
                {
                    txtNextHearingDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtNextHearingDate.ClientID + "','" + strDateFormat + "',false,  'F');");
                }
                if (PageMode == PageModes.Query)
                {
                    txtNextHearingDate.Attributes.Add("readonly", "true");
                    txtNextHearingDate.Attributes.Remove("onblur");
                }

            }

            btnAddNextHearingDetail.Enabled = false;

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = "Unable to add Next Hearing Details";
        }


    }
    #endregion
    //#region Delete
    //protected void btnDelete_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        objClient = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();
    //        objDataTable = new S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackDataTable();

    //        S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_LegalHearingTrackRow objRow;

    //        objRow = objDataTable.NewS3G_LR_LegalHearingTrackRow();

    //        objRow.LHR_ID = intLHTID;

    //        objRow.Company_ID = intCompanyID;
    //        objRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
    //        objRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
    //        objRow.Customer_ID = Convert.ToInt32(hdnCustomerId.Value);
    //        objRow.Advocate_ID = Convert.ToInt32(ddlAdvocateName.SelectedValue);
    //        objRow.LRN_ID = Convert.ToInt32(ddlLNN.SelectedValue);
    //        objRow.LRN_No = ddlLNN.SelectedItem.Text.ToString();
    //        objRow.LHR_Date = Utility.StringToDate(txtLHRDate.Text.Trim());
    //        objRow.PANum = txtMLA.Text.ToString();
    //        objRow.SANum = txtSLA.Text.ToString();
    //        objRow.Case_No = Convert.ToInt32(txtCaseNo.Text.Trim());
    //        objRow.Case_Details = txtCaseDetails.Text.ToString();
    //        objRow.Court_Details = txtCourtDetails.Text.ToString();
    //        objRow.Remarks = txtRemarks.Text.ToString();
    //        if (txtClosureDate.Text != "")
    //        {
    //            objRow.Closure_Date = Utility.StringToDate(txtClosureDate.Text.Trim());
    //        }

    //        objRow.Decree = txtDegree.Text.ToString();
    //        objRow.Closure_Remarks = txtRemarksC.Text.ToString();

    //        objRow.XmlActivityDetails = FunPubFormXMLDetails();
    //        objRow.Created_By = intuserID;
    //        chkActive.Checked = false;
    //        objRow.Is_Active = chkActive.Checked;
    //        objRow.LHR_No = txtLHRNo.Text.ToString();
    //        objRow.DeleteKey = 1;

    //        objDataTable.AddS3G_LR_LegalHearingTrackRow(objRow);
    //        intErrorCode = objClient.FunPubModifyLegalHearingTrack(SerMode, ClsPubSerialize.Serialize(objDataTable, SerMode));


    //        if (intErrorCode == 0)
    //        {

    //            Utility.FunShowAlertMsg(this.Page, "Legal Hearing Track Details Deleted successfully");
    //            btnSave.Enabled = false;
    //            btnDelete.Enabled = false;
    //            btnClear.Enabled = false;
    //            btnAddNextHearingDetail.Enabled = false;
    //            HideActivityDetailsAction();


    //        }
    //        else
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "Legal Hearing Details are not able to deleted");
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.ToString();
    //    }

    //}
    //#endregion
    #endregion
    #region TabChangedEvent
    protected void User_ActiveTabChanged(object sender, EventArgs e)
    {
        try
        {
            //if (intLHTID > 0)
            //{
            //    if (strMode == "M")
            //    {
            //        if ( strFlag !="F")
            //        {
            //            if (tabcontainer1.ActiveTabIndex == 1)
            //            {
            //                btnAddNextHearingDetail.Enabled = false;
            //            }
            //            else if(tabcontainer1.ActiveTabIndex ==2)
            //            {
            //                btnAddNextHearingDetail.Enabled = false;
            //            }
            //            else if (tabcontainer1.ActiveTabIndex == 0)
            //            {
            //                btnAddNextHearingDetail.Enabled = true;
            //            }

            //        }
            //    }
            //    if (strMode == "Q")
            //    {
            //        btnAddNextHearingDetail.Enabled = false;
            //    }

            //}
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region FunToDisableEditLinkButton
    private void HideActivityDetailsAction()
    {
        if (grdLegalHearing.Rows.Count > 0)
        {
            ((LinkButton)grdLegalHearing.Rows[0].FindControl("lnkEdit")).Enabled = false;
        }
        txtDegree.ReadOnly = true;
        txtRemarksC.ReadOnly = true;
        CalendarExtender3.Enabled = false;
        txtCaseNo.ReadOnly = true;
        txtCaseDetails.ReadOnly = true;
        txtCourtDetails.ReadOnly = true;
        txtRemarks.ReadOnly = true;
        //ddlAdvocateName.Enabled = false;
        Utility.ClearDropDownList(ddlAdvocateName);

    }
    #endregion


    protected void FunProEnableClosureYes(object sender, EventArgs e)
    {
        if (rbtYes.Checked == true)
        {
            //grdLegalHearing.FooterRow.Visible = false;

            if (grdLegalHearing.FooterRow.Visible == true)
            {
                if (proceparam != null)
                    proceparam.Clear();
                proceparam = new Dictionary<string, string>();
                proceparam.Add("@Company_ID", intCompanyID.ToString());
                proceparam.Add("@LHR_ID", intLHTID.ToString());
                proceparam.Add("@option", "2");
                DataSet ds = new DataSet();
                ds = Utility.GetDataset("S3G_LR_GetLHTDetails", proceparam);
                ViewState["LegalHearingDetails"] = ds.Tables[0];
                //hdnaddNHD.Value = ds.Tables[0].Rows[0]["NextHearingDate"].ToString();
                hdnNHD.Value = ds.Tables[0].Rows[0]["NextHearingDate"].ToString();
                grdLegalHearing.DataSource = ds.Tables[0];
                grdLegalHearing.DataBind();
                grdLegalHearing.FooterRow.Visible = false;

                grvHistory.DataSource = ds.Tables[1];
                grvHistory.DataBind();
                grvHistory.EmptyDataText = "No History Details available";

                //int intcomp = Utility.CompareDates(DateTime.Now.ToString(), hdnNHD.Value.ToString());
                //if (intcomp == 1)
                //{
                //    btnAddNextHearingDetail.Enabled = false;
                //}
                //else
                //{
                //    btnAddNextHearingDetail.Enabled = true;
                //}
            }


            rbtNo.Checked = false;
            CalendarExtender3.Enabled = true;
            txtClosureDate.Text = DateTime.Now.ToString(strDateFormat);
            txtDegree.ReadOnly = txtRemarksC.ReadOnly = false;
            rfvClosureDate.Enabled = true;
            rfvDegree.Enabled = true;
            btnAddNextHearingDetail.Enabled = false;

            //lblClosureDate.CssClass = "styleReqFieldLabel";
            // lblDegree.CssClass = "styleReqFieldLabel";
            //lblRemarks .CssClass ="styleFieldLabel";
        }

    }

    protected void FunProEnableClosureNo(object sender, EventArgs e)
    {
        if (rbtNo.Checked == true)
        {
            rbtYes.Checked = false;
            CalendarExtender3.Enabled = false;
            txtDegree.ReadOnly = txtRemarksC.ReadOnly = true;
            txtClosureDate.Text = txtDegree.Text = txtRemarksC.Text = "";
            rfvClosureDate.Enabled = false;
            rfvDegree.Enabled = false;
            int intcomp = Utility.CompareDates(DateTime.Now.ToString(), hdnNHD.Value.ToString());
            if (intcomp == 1)
            {
                btnAddNextHearingDetail.Enabled = false;
            }
            else
            {
                btnAddNextHearingDetail.Enabled = true;
            }
            // lblClosureDate.CssClass = "styleFieldLabel";
            // lblDegree.CssClass = "styleFieldLabel";
            // lblRemarks .CssClass ="styleFieldLabel";
        }
    }

    //protected void txtDegree_TextChanged1(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //txtDegree.ReadOnly = false;
    //        txtRemarksC.ReadOnly = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.ToString();
    //    }

    //}
}

