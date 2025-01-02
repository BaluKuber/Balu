#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin  
/// Screen Name			: Location Master add
/// Created By			: Tamilselvan.S
/// Created Date		: 04 May 2011
/// Purpose	            : 
#endregion

#region [Namespace]

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
using S3GBusEntity;
using System.Collections.Generic;
using System.Text;

#endregion [Namespace]

public partial class System_Admin_S3GSysAdminLocationMaster_Add : ApplyThemeForProject
{
    #region Initialization

    static string[] strarrControlsID = new string[0];
    int intCompanyId = 0;
    int intUserId = 0;
    static string strPageName = "Location Master";
    int intErrorCode = 0;
    int intLocationCat_ID = 0;
    int intLocationMap_ID = 0;
    SerializationMode SerMode = SerializationMode.Binary;

    ApplyThemeForProject ATFP = new ApplyThemeForProject();
    UserInfo usrInfo = new UserInfo();
    //User Authorization
    string strType = "Cate";
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    static DataTable dtHierarchyWidth = new DataTable();
    //Code end

    UserInfo ObjUserInfo = new UserInfo();
    PagingValues ObjPaging = new PagingValues();
    string strDateFormat = string.Empty;
    public static System_Admin_S3GSysAdminLocationMaster_Add obj_Page;

    S3GSession ObjS3GSession = new S3GSession();
    S3GAdminServicesReference.S3GAdminServicesClient objAdminService;
    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable objLocationCategory_DataTable = new SystemAdmin.S3G_SYSAD_LocationCategoryDataTable();
    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryRow objLocationCategory_DataRow;
    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterDataTable objLocationMaster_DataTable = new SystemAdmin.S3G_SYSAD_LocationMasterDataTable();
    S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterRow objLocationMaster_DataRow;

    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminLocationMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminLocationMaster_Add.aspx';";
    string strRedirectPage = "../System Admin/S3GSysAdminLocationMaster_View.aspx";
    string strErrorMessage = @"Correct the following validation(s):<ul><li> ";
    string strErrorMsgLast = @"</ul></li>";

    #endregion Initialization

    #region [Page Event's]

    protected void Page_PreInit(object sender, EventArgs e)
    {

        if (usrInfo.ProCompanyIdRW == 0)
            HttpContext.Current.Response.Redirect("../SessionExpired.aspx");

        if (Request.QueryString["qsMode"] != null)
        {
            //_pageMode = Request.QueryString["qsMode"];             
            SetPageMode(Request.QueryString["qsMode"].ToString());
        }

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            if (fromTicket.Name != null)
                PageIdValue = fromTicket.Name;
        }
        if (Request.QueryString["qsWorkFlow"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsWorkFlow"));
            if (fromTicket.Name != null)
                isWorkFlowTraveler = true;
            WorkFlowId = fromTicket.Name;

            SetPageMode("w");
        }

        // CPT Setup Master Page 
        //if (Request.QueryString["Popup"] != null)
        //    Page.MasterPageFile = "~/Common/MasterPage.master";
        //else if (Request.QueryString["IsFromAccount"] != null)
        //    Page.MasterPageFile = "~/Common/MasterPage.master";
        //else
        //    Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";


        // Setup Page Theme
        Page.Theme = usrInfo.ProUserThemeRW;

        // Initialize from User Information
        CompanyId = usrInfo.ProCompanyIdRW.ToString();
        UserId = usrInfo.ProUserIdRW.ToString();
        // Initialize Date Formate
        DateFormate = ObjS3GSession.ProDateFormatRW; //"dd/MM/yyyy";

        DropDownList d = (DropDownList)Page.Master.FindControl("ddlTheme");
        if (d != null)
            d.SelectedValue = usrInfo.ProUserThemeRW;

        if (!String.IsNullOrEmpty(Request.Form["__EVENTTARGET"]))
        {
            PostBackControlId = Request.Form["__EVENTTARGET"];
        }
        //KeepSessionAlive();


        FunPubLoadActiveHierarchy();
        FunPubLoadHierarchyForMapping();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        try
        {
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtenderMRACreationFrom.Format = strDateFormat;
            CalendarExtender1.Format = strDateFormat;
            CalendarExtender2.Format = strDateFormat;
            CalendarExtender3.Format = strDateFormat;
            CalendarExtenderAddressEffFrom.Format = strDateFormat;
            
            usrInfo = new UserInfo();
            intCompanyId = usrInfo.ProCompanyIdRW;
            intUserId = usrInfo.ProUserIdRW;
            //User Authorization
            bCreate = usrInfo.ProCreateRW;
            bModify = usrInfo.ProModifyRW;
            bQuery = usrInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            //Code end
            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = intCompanyId.ToString();
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString["Type"] != null)
                strType = Request.QueryString["Type"];
            if (Request.QueryString["qsLocCatId"] != null && strType == "Cate")
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsLocCatId"));
                intLocationCat_ID = Convert.ToInt32(fromTicket.Name.ToString());
            }
            else if (Request.QueryString["qsLocMapId"] != null && strType == "Map")
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsLocMapId"));
                intLocationMap_ID = Convert.ToInt32(fromTicket.Name.ToString());
            }
            if (strMode.ToUpper() == "C")
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
            else if (strMode.ToUpper() == "Q")
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            else if (strMode.ToUpper() == "M")
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnSaveLocCategory.ValidationGroup = "vgLocationGroup";
                btnSaveLocCategory.OnClientClick = "return fnCheckPageValidators('vgLocationGroup','f')";
            }
            if (!IsPostBack)
            {
                txtCSTDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCSTDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtVATDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtVATDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtWCTDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtWCTDate.ClientID + "','" + strDateFormat + "',true,  false);");
                TxtSGSTRegDate.Attributes.Add("onblur", "fnDoDate(this,'" + TxtSGSTRegDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtAddressEffectiveFrom.Text = DateTime.Now.Date.ToString(strDateFormat);
                FunPriLoadMasterData();
                if (strType == "Cate")
                {
                    Dictionary<string, string> dictParam = new Dictionary<string, string>();
                    dictParam.Add("@Company_ID", usrInfo.ProCompanyIdRW.ToString());
                    if (strMode != "Q" && strMode != "M")
                        dictParam.Add("@IsActive", "1");
                    DataTable dtHierarchy = Utility.GetDefaultData("S3G_SYSAD_GetHierachyMasterDetails", dictParam);
                    rblHierarchy.DataTextField = "Location_Description";
                    rblHierarchy.DataValueField = "Hierachy";
                    rblHierarchy.DataSource = dtHierarchy;
                    rblHierarchy.DataBind();
                    if (dtHierarchy.Rows.Count == 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Define Hierarchy Master.");
                        Response.Redirect(strRedirectPage, false);
                    }

                    if (intLocationCat_ID > 0)
                    {
                        if (Request.QueryString["Hir"] != null)
                        {
                            tcLocCategory.ActiveTabIndex = Convert.ToInt32(Request.QueryString["Hir"].ToString());
                            FunProCateTabChanged();
                        }

                        FunPubLoadLocationCategoryDetails();
                    }
                    pnlLocationMapping.Visible = false;
                    if (tcLocCategory.ActiveTab != null && strMode != "M" && strMode != "Q")
                    {
                        DataRow[] drow = dtHierarchyWidth.Select("Location_Description='" + tcLocCategory.ActiveTab.HeaderText + "'");
                        if (drow != null && drow.Count() > 0)
                            txtLocationCode.MaxLength = Convert.ToInt32(drow[0]["Width"]);
                        lblLocationCode.Text = tcLocCategory.ActiveTab.HeaderText + " Code"; //"Location " + 
                        lblLocationDescription.Text = tcLocCategory.ActiveTab.HeaderText + " Description";//"Location " +
                        if (tcLocCategory.Tabs.Count > 0 && tcLocCategory.ActiveTabIndex != 0)
                            lblParent.Text = tcLocCategory.Tabs[tcLocCategory.ActiveTabIndex - 1].HeaderText;
                    }
                    if (intLocationCat_ID == 0)
                    {
                        lblParent.Visible = ddlParent.Visible = false;

                        if (Request.QueryString["Hir"] != null)
                        {
                            tcLocCategory.ActiveTabIndex = Convert.ToInt32(Request.QueryString["Hir"].ToString());
                            FunProCateTabChanged();
                        }
                    }
                }
                else if (strType == "Map")
                {
                    pnlHierarchyType.Visible = pnlLocationCatDetails.Visible = false;
                    if (intLocationMap_ID > 0)
                    {
                        FunPubLoadLocationMappingDetails();
                    }
                }
            }
            if (strType == "Cate" && strMode != "M" && strMode != "Q")
            {
                //FunPubLoadlocationDetailsBasedOnHierarchy();
                //ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
                //AjaxControlToolkit.TabContainer tcDefCat = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocCategory");
                //string strHText = tcDefCat.ActiveTab.HeaderText;
                //AjaxControlToolkit.TabPanel tp = (AjaxControlToolkit.TabPanel)tcDefCat.FindControl("tpLocDef" + strHText);
                //GridView gv = ((GridView)tp.FindControl("gv" + strHText));
            }
            if (strType == "Map" && strMode != "M" && strMode != "Q")
            {
                txtLocationMappingCode.Text = "";
                txtMappingDescription.Text = "";
                string strCodes = string.Empty;
                string strSaveCodes = string.Empty;
                string strDescriptions = string.Empty;
                int intCount = 0;
                foreach (string str in strarrControlsID)
                {
                    DropDownList ddlList = new DropDownList();
                    ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
                    AjaxControlToolkit.TabContainer tcDefMap = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocationMapping");
                    AjaxControlToolkit.TabPanel tpLocDef = (AjaxControlToolkit.TabPanel)tcDefMap.FindControl("tpLocDef" + str);
                    if (tpLocDef != null)
                    {
                        ddlList = ((DropDownList)tpLocDef.FindControl("ddlHierarchy" + str));
                        if (ddlList != null && ddlList.SelectedIndex > 0)
                        {
                            if (intCount == 0)
                            {
                                if (strCodes == string.Empty)
                                {
                                    strCodes += ddlList.SelectedValue;
                                    strSaveCodes += ddlList.SelectedValue;
                                    strDescriptions = ddlList.SelectedItem.Text;
                                }
                                else
                                {
                                    strCodes += ddlList.SelectedValue;  // "|" +
                                    strSaveCodes += "|" + ddlList.SelectedValue;
                                    strDescriptions = ddlList.SelectedItem.Text;
                                }
                            }
                            else
                                ddlList.SelectedIndex = 0;
                        }
                        else
                            intCount++;
                    }
                }
                hdnMappingCode.Value = strSaveCodes;
                txtLocationMappingCode.Text = strCodes;
                txtMappingDescription.Text = strDescriptions;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvLocationMaster.IsValid = false;
            cvLocationMaster.ErrorMessage = strErrorMessage + "Unable to load the page." + strErrorMsgLast;
        }
    }

    private void FunPriLoadMasterData()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
        ObjStatus.Option = 1020;
        ObjStatus.Param1 = "STATE";
        ObjStatus.Param2 = intCompanyId.ToString();
        Utility.FillDLL(txtComState, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));
    }

    #endregion [Page Event's]

    #region [Button Event's]

    protected void btnCategoryGo_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtLocation = ((DataTable)ViewState["dt_Location"]);
            //Code Added and Commented by Saran on 12-Jan-2012 to achieve Operational Location start
            //if (dtLocation == null || dtLocation.Columns.Count != 11)
            if (dtLocation == null || dtLocation.Columns.Count != 15)
            //Code Added and Commented by Saran on 12-Jan-2012 to achieve Operational Location end
            {
                dtLocation = FunPubCreateLocationTableStructure();
            }
            if (dtLocation.Rows.Count == 0)
            {
                DataRow drNew = dtLocation.NewRow();
                drNew["SNo"] = dtLocation.Rows.Count + 1;
                drNew["Location_Code"] = txtLocationCode.Text;
                drNew["Location_Description"] = txtLocationName.Text;
                drNew["Hierarchy_Description"] = tcLocCategory.ActiveTab.HeaderText;
                drNew["Hierarchy_Type"] = tcLocCategory.ActiveTabIndex + 1;// Convert.ToInt32(rblHierarchy.SelectedValue);
                string strMappingCode = tcLocCategory.ActiveTabIndex == 0 ? "" : ddlParent.SelectedItem.Text;
                drNew["CurrenctMapping_Code"] = tcLocCategory.ActiveTabIndex == 0 ? txtLocationCode.Text : strMappingCode + "|" + txtLocationCode.Text;
                drNew["Mapping_Description"] = tcLocCategory.ActiveTabIndex == 0 ? txtLocationName.Text : lblMappingCodeDescription.Text + "|" + txtLocationName.Text;
                string[] strcodes = strMappingCode.Split('|');
                drNew["Loc_Parent_Code"] = strMappingCode;
                drNew["Parent_Code"] = tcLocCategory.ActiveTabIndex == 0 ? "" : (strcodes[strcodes.Length - 1]).ToString();
                //Code Added by Saran on 12-Jan-2012 to achieve Operational Location start
                drNew["Is_Operational"] = cbxOperationalLoc.Checked;
                //Code Added by Saran on 12-Jan-2012 to achieve Operational Location end

                //Code Added by vino on 17-Oct-2014

                if (rblisdealertype.SelectedIndex == 0)
                    drNew["Is_DealerType"] = true;
                else
                    drNew["Is_DealerType"] = false;


                drNew["VATTIN"] = txtVATTIN.Text.Trim();
                drNew["CSTTIN"] = txtCSTTIN.Text.Trim();
                drNew["WCT"] = txtWCT.Text.Trim();

                drNew["VAT_Date"] = txtVATDate.Text.Trim();
                drNew["CST_Date"] = txtCSTDate.Text.Trim();
                drNew["WCT_Date"] = txtWCTDate.Text.Trim();

                if (txtSGSTin.Text != "")
                    drNew["GSTIN"] = txtSGSTin.Text.Trim();
                if (TxtSGSTRegDate.Text != "")
                    drNew["GST_Reg_Date"] = TxtSGSTRegDate.Text.Trim();

                drNew["GST_State_Code"] = txtGSTStateCode.Text.Trim();

                drNew["Effective_From"] = txtAddressEffectiveFrom.Text.Trim();


                dtLocation.Rows.Add(drNew);
                ViewState["dt_Location"] = dtLocation;
            }
            else
            {

                //************************//
                //Added new For Code Duplication//
                string strToCompareCode = "";
                string strMappingCode1 = tcLocCategory.ActiveTabIndex == 0 ? "" : ddlParent.SelectedItem.Text;
                strToCompareCode = tcLocCategory.ActiveTabIndex == 0 ? txtLocationCode.Text : strMappingCode1 + "|" + txtLocationCode.Text;
                DataRow[] drCode = dtLocation.Select("CurrenctMapping_Code='" + strToCompareCode + "'");
                //DataRow[] drCode = dtLocation.Select("Location_Code='" + txtLocationCode.Text.Trim() + "'");
                //Added new For Code Duplication//

                DataRow[] drName = dtLocation.Select("Location_Description='" + txtLocationName.Text.Trim() + "'");

                if (drCode.Count() == 1)
                    Utility.FunShowAlertMsg(this.Page, "Location Code Already Existing in this list ");
                else if (drName.Count() == 1)
                    Utility.FunShowAlertMsg(this.Page, "Location Description Already Existing in this list ");
                else
                {
                    DataRow drNew = dtLocation.NewRow();
                    drNew["SNo"] = dtLocation.Rows.Count + 1;
                    drNew["Location_Code"] = txtLocationCode.Text;
                    drNew["Location_Description"] = txtLocationName.Text;
                    drNew["Hierarchy_Description"] = tcLocCategory.ActiveTab.HeaderText;
                    drNew["Hierarchy_Type"] = tcLocCategory.ActiveTabIndex + 1; ;// Convert.ToInt32(rblHierarchy.SelectedValue);
                    string strMappingCode = tcLocCategory.ActiveTabIndex == 0 ? "" : ddlParent.SelectedItem.Text;
                    drNew["CurrenctMapping_Code"] = tcLocCategory.ActiveTabIndex == 0 ? txtLocationCode.Text : strMappingCode + "|" + txtLocationCode.Text;
                    drNew["Mapping_Description"] = tcLocCategory.ActiveTabIndex == 0 ? txtLocationName.Text : lblMappingCodeDescription.Text + "|" + txtLocationName.Text;
                    drNew["Loc_Parent_Code"] = strMappingCode;
                    drNew["Parent_Code"] = tcLocCategory.ActiveTabIndex == 0 ? "" : (strMappingCode.Split('|')[(strMappingCode.Split('|').Length - 1)]);

                    //Code Added by Saran on 12-Jan-2012 to achieve Operational Location start
                    drNew["Is_Operational"] = cbxOperationalLoc.Checked;
                    //Code Added by Saran on 12-Jan-2012 to achieve Operational Location end

                    //Code Added by vino on 17-Oct-2014
                    if (rblisdealertype.SelectedIndex == 0)
                        drNew["Is_DealerType"] = true;
                    else
                        drNew["Is_DealerType"] = false;
                    drNew["VATTIN"] = txtVATTIN.Text.Trim();
                    drNew["CSTTIN"] = txtCSTTIN.Text.Trim();
                    drNew["WCT"] = txtVATTIN.Text.Trim();

                    drNew["VAT_Date"] = txtVATDate.Text.Trim();
                    drNew["CST_Date"] = txtCSTDate.Text.Trim();
                    drNew["WCT_Date"] = txtWCTDate.Text.Trim();

                    if (txtSGSTin.Text != "")
                        drNew["GSTIN"] = txtSGSTin.Text.Trim();
                    if (TxtSGSTRegDate.Text != "")
                        drNew["GST_Reg_Date"] = TxtSGSTRegDate.Text.Trim();

                    drNew["GST_State_Code"] = txtGSTStateCode.Text.Trim();
                    drNew["Effective_From"] = txtAddressEffectiveFrom.Text.Trim();
                    dtLocation.Rows.Add(drNew);
                    ViewState["dt_Location"] = dtLocation;
                    //CurrenctDisplay
                }
            }
            FunPubLoadlocationDetailsBasedOnHierarchy();
            FunPubClearCategoryuEntry();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvLocationMaster.IsValid = false;
            cvLocationMaster.ErrorMessage = strErrorMessage + "Unable to add the Category details." + strErrorMsgLast;
        }
    }

    public DataTable FunPubCreateLocationAddressTableStructure()
    {
        DataTable dtAddress = new DataTable();
        dtAddress.Columns.Add("Address1", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("Address2", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("CityName", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("StateId", typeof(Int32));
        dtAddress.Columns.Add("StateName", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("Country", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("Pinno", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("Telephone", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("MobileNo", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("Email", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("Website", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("LandMark", typeof(string)).DefaultValue = string.Empty;
        dtAddress.Columns.Add("Is_DealerType", typeof(bool)).DefaultValue = true;
        return dtAddress;
    }
    private void FunGetAddressDetails()
    {
        DataTable dtAddress = ((DataTable)ViewState["dtAddress"]);
        if (dtAddress == null || dtAddress.Columns.Count != 12)
        {
            dtAddress = FunPubCreateLocationAddressTableStructure();
        }
        if (dtAddress.Rows.Count == 0)
        {
            DataRow drNew = dtAddress.NewRow();
            drNew["Address1"] = txtComAddress1.Text.Trim();
            drNew["Address2"] = txtCOmAddress2.Text.Trim();
            drNew["CityName"] = txtComCity.Text.Trim();
            if (txtComState.SelectedItem.Text == "--Select--")
            {
                drNew["StateId"] = "0";
                drNew["StateName"] = string.Empty;
            }
            else
            {
                drNew["StateId"] = txtComState.SelectedValue.ToString();
                drNew["StateName"] = txtComState.SelectedItem.Text.Trim();
            }

            drNew["Country"] = txtComCountry.Text.Trim();
            drNew["Pinno"] = txtComPincode.Text.Trim();
            drNew["Telephone"] = txtComTelephone.Text.Trim();
            drNew["MobileNo"] = txtComMobile.Text.Trim();
            drNew["Email"] = txtComEmail.Text.Trim();
            drNew["Website"] = txtComWebsite.Text.Trim();
            drNew["LandMark"] = txtCommLandmark.Text.Trim();
            if (rblisdealertype.SelectedIndex == 0)
                drNew["Is_DealerType"] = true;
            else
                drNew["Is_DealerType"] = false;
            dtAddress.Rows.Add(drNew);
            ViewState["dtAddress"] = dtAddress;
        }
    }

    protected void btnSaveLocationCategory_Click(object sender, EventArgs e)
    {
        try
        {
            //if (Page.IsValid)
            if (strMode != "Q")
            {
                FunGetAddressDetails();
            }
            FunPubInsertLocationCategory();
            ddlParent.SelectedIndex = -1;
            lblMappingCodeDescription.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvLocationMaster.IsValid = false;
            cvLocationMaster.ErrorMessage = strErrorMessage + "Unable to save the location category." + strErrorMsgLast;
        }
    }

    protected void btnCancelLocCategory_Click(object sender, EventArgs e)
    {
        String StrQryStr = "";
        string strHir = "";
        if (Request.QueryString["qsType"] != null)
            StrQryStr = Request.QueryString["qsType"].ToString().Trim();
        if (Request.QueryString["Hir"] != null)
        {
            strHir = "&Hir=" + Request.QueryString["Hir"].ToString();
        }

        Response.Redirect(strRedirectPage + "?Type=" + strType + "&Level=" + HttpUtility.UrlEncode(StrQryStr) + strHir);
    }

    protected void btnSaveLocationMapping_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
                FunPubInsertLocationMaster();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvLocationMaster.IsValid = false;
            cvLocationMaster.ErrorMessage = strErrorMessage + "Unable to save the location mapping details." + strErrorMsgLast;
        }
    }

    protected void btnLocationMapping_Go_Click(object sender, EventArgs e)
    {
        try
        {
            string[] strCodeCheck = txtLocationMappingCode.Text.Split('|');
            if (strCodeCheck != null && strCodeCheck.Count() == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Select at least minimum two mapping...");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvLocationMaster.IsValid = false;
            cvLocationMaster.ErrorMessage = strErrorMessage + "" + strErrorMsgLast;
        }
    }

    #endregion [Button Event's]

    #region [Tab Event's]

    protected void tcLocCategory_ActiveTabChanged(object sender, EventArgs e)
    {
        FunProCateTabChanged();
    }

    protected void FunProCateTabChanged()
    {
        try
        {
            lblMappingCodeDescription.Text = "";
            FunPubLoadlocationDetailsBasedOnHierarchy();
            if (tcLocCategory.ActiveTabIndex == 0)
            {
                lblParent.Visible = ddlParent.Visible = lblMappingCodeDescription.Visible = false;
            }
            else
            {
                lblParent.Visible = ddlParent.Visible = lblMappingCodeDescription.Visible = true;
            }
            FunPubLoadParentMappingDropDown(tcLocCategory.ActiveTabIndex);
        }
        catch (Exception ex)
        {
            cvLocationMaster.IsValid = false;
            cvLocationMaster.ErrorMessage = strErrorMessage + "" + strErrorMsgLast;
        }
    }

    #endregion [Tab Event's]

    #region [Function's]

    #region "Page Values Initialization "

    private void SetPageMode(string queryMode)
    {
        switch (queryMode.ToLower())
        {
            case "c":
                PageMode = PageModes.Create;
                break;
            case "q":
                PageMode = PageModes.Query;
                break;
            case "m":
                PageMode = PageModes.Modify;
                break;
            case "d":
                PageMode = PageModes.Delete;
                break;
            case "w":
                PageMode = PageModes.WorkFlow;
                break;
            default:
                break;
        }
    }
    #endregion

    public void FunPubLoadLocationCategoryDetails()
    {
        try
        {
            DataTable dtLocCategoryDetails;
            //opc048 start
            //objAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            //byte[] byteLocCatDetails = objAdminService.FunPubQueryLocationCategoryDetails(intCompanyId, intUserId, intLocationCat_ID, SerMode);
            //dtLocCategoryDetails = (DataTable)ClsPubSerialize.DeSerialize(byteLocCatDetails, SerMode, typeof(DataTable));
            // rblHierarchy.SelectedValue = Convert.ToString(dtLocCategoryDetails.Rows[0]["Hierarchy_Type"]);  
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", usrInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@User_ID", usrInfo.ProUserIdRW.ToString());
            dictParam.Add("@Location_Category_ID", intLocationCat_ID.ToString());
            DataSet dsLocMappingDetails = Utility.GetDataset("S3G_SYSAD_GetLocationCategoryDetails", dictParam);
            dtLocCategoryDetails = dsLocMappingDetails.Tables[0];
            //opc048 End

            lblLocationCode.Text = Convert.ToString(dtLocCategoryDetails.Rows[0]["Location_Description"]) + " Code";//"Location " +
            lblLocationDescription.Text = Convert.ToString(dtLocCategoryDetails.Rows[0]["Location_Description"]) + " Description";//"Location " +
            txtLocationCode.Text = Convert.ToString(dtLocCategoryDetails.Rows[0]["Location_Code"]);
            txtLocationName.Text = Convert.ToString(dtLocCategoryDetails.Rows[0]["LocationCat_Description"]);
            cbxActive.Checked = Convert.ToBoolean(dtLocCategoryDetails.Rows[0]["Is_Active"]);
            FunProBindLocationDetails(intCompanyId, intUserId, intLocationCat_ID);
            lblParent.Text = Convert.ToString(dtLocCategoryDetails.Rows[0]["ParentLocation"]);
            rblHierarchy.Enabled = false;

            if ((Convert.ToInt32(dtLocCategoryDetails.Rows[0]["Hierarchy_Type"]) - 1) != 0)
            {
                FunPubLoadParentMappingDropDown(Convert.ToInt32(dtLocCategoryDetails.Rows[0]["Hierarchy_Type"]) - 1);
                ddlParent.SelectedValue = Convert.ToString(dtLocCategoryDetails.Rows[0]["Previous_LocMap"]);
                lblMappingCodeDescription.Text = Convert.ToString(dtLocCategoryDetails.Rows[0]["Previous_LocMap"]);
                ddlParent.ClearDropDownList();
            }
            else
            {
                ddlParent.Visible = lblMappingCodeDescription.Visible = lblParent.Visible = false;
            }
            if (strMode == "Q" || strMode == "M")
            {
                //opc048 start
                gvLocationHistory.DataSource = dsLocMappingDetails.Tables[1];
                gvLocationHistory.DataBind();
                //opc048 end

                txtLocationCode.ReadOnly = txtLocationName.ReadOnly = true;
                btnCategoryGo.Enabled = btnSaveLocCategory.Enabled = cbxActive.Enabled = false;
                //Code Added by Saran on 12-Jan-2012 to achieve Operational Location start
                lblOperationalLoc.Visible = cbxOperationalLoc.Visible = false;
                //Code Added by Saran on 12-Jan-2012 to achieve Operational Location end

                //changed by bhuvana
                txtLocationName.Enabled = true;
                txtLocationName.ReadOnly = false;

                tcLocCategory.Visible = false;
                if (strMode == "Q")
                {
                    txtComAddress1.Enabled = txtCOmAddress2.Enabled = txtComCity.Enabled = txtComCountry.Enabled = txtComEmail.Enabled = txtCommLandmark.Enabled =
                        txtComMobile.Enabled = txtComPincode.Enabled = txtComState.Enabled = txtComTelephone.Enabled = txtComWebsite.Enabled = true;
                    txtComCity.ReadOnly = true;
                    txtComAddress1.ReadOnly = true;
                    txtCOmAddress2.ReadOnly = true;
                    txtComCountry.ReadOnly = true;
                    txtComEmail.ReadOnly = true;
                    txtCommLandmark.ReadOnly = true;
                    txtComMobile.ReadOnly = true;
                    txtComPincode.ReadOnly = true;
                    txtComTelephone.ReadOnly = true;
                    txtComWebsite.ReadOnly = true;
                    txtComState.Enabled = false;
                    txtLocationName.ReadOnly = txtVATTIN.ReadOnly = txtVATDate.ReadOnly =
                        txtCSTTIN.ReadOnly = txtCSTDate.ReadOnly = txtWCT.ReadOnly =
                        txtWCTDate.ReadOnly = txtSGSTin.ReadOnly = TxtSGSTRegDate.ReadOnly = txtGSTStateCode.ReadOnly = true;
                    rblisdealertype.Enabled = CalendarExtender1.Enabled = CalendarExtender2.Enabled =
                        CalendarExtender3.Enabled = CalendarExtenderMRACreationFrom.Enabled = false;
                }

                //txtComAddress1.Text = dtLocCategoryDetails.Rows[1]["Loc_Address1"].ToString();
                //txtCOmAddress2.Text = dtLocCategoryDetails.Rows[1]["Loc_Address2"].ToString();
                //txtComCity.Text = dtLocCategoryDetails.Rows[1]["Loc_City"].ToString();
                //txtComCountry.Text = dtLocCategoryDetails.Rows[1]["Loc_Country"].ToString();
                //txtComState.SelectedValue = dtLocCategoryDetails.Rows[1]["Loc_State_ID"].ToString();
                //txtComEmail.Text = dtLocCategoryDetails.Rows[1]["Loc_Email"].ToString();
                //txtCommLandmark.Text = dtLocCategoryDetails.Rows[1]["Loc_Landmark"].ToString();
                //txtComWebsite.Text = dtLocCategoryDetails.Rows[1]["Loc_Website"].ToString();
                //txtComTelephone.Text = dtLocCategoryDetails.Rows[1]["Loc_Telephone"].ToString();
                //txtComMobile.Text = dtLocCategoryDetails.Rows[1]["Loc_Mobile"].ToString();
                //txtComPincode.Text = dtLocCategoryDetails.Rows[1]["Loc_Pincode"].ToString();
                //rblHierarchy.Enabled = false;
            }
            if (strMode == "M")
                btnSaveLocCategory.Enabled = cbxActive.Enabled = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Location Category details");
        }
        //finally
        //{
        //    if (objAdminService != null)
        //        objAdminService.Close();
        //}
    }

    public void FunPubLoadLocationMappingDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", usrInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@LocationMap_ID", intLocationMap_ID.ToString());
            DataTable dtLocMappingDetails = Utility.GetDefaultData("S3G_SYSAD_GetLocationMappingDetails", dictParam);

            txtLocationMappingCode.Text = Convert.ToString(dtLocMappingDetails.Rows[0]["LocationMap_Code"]).Replace("|", "");
            cbxActiveMapping.Checked = Convert.ToBoolean(dtLocMappingDetails.Rows[0]["Is_Active"]);
            cbxOperationalLocM.Checked = Convert.ToBoolean(dtLocMappingDetails.Rows[0]["Is_Operational"]);
            if (strMode == "Q" || strMode == "M")
            {
                txtLocationMappingCode.ReadOnly = true;
                btnSaveLocationMapping.Enabled = false;
                tcLocCategory.Visible = false;
                cbxActiveMapping.Enabled = false;

                if (strMode == "M")
                {
                    cbxActiveMapping.Enabled = btnSaveLocationMapping.Enabled = true;
                    cbxOperationalLocM.Enabled = true;
                }
            }
            string strDescription = string.Empty;
            foreach (string str in strarrControlsID)
            {
                DropDownList ddlList = new DropDownList();
                ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
                AjaxControlToolkit.TabContainer tcDefMap = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocationMapping");
                AjaxControlToolkit.TabPanel tpLocDef = (AjaxControlToolkit.TabPanel)tcDefMap.FindControl("tpLocDef" + str);
                if (tpLocDef != null)
                {
                    ddlList = ((DropDownList)tpLocDef.FindControl("ddlHierarchy" + str));
                    DataRow[] drow = dtLocMappingDetails.Select("Hierarchy_Description='" + str + "'");
                    if (drow != null && drow.Count() > 0)
                    {
                        ddlList.SelectedValue = Convert.ToString(drow[0]["Location_Category_ID"]);
                        //ddlList.SelectedValue = Convert.ToString(drow[0]["LocationCat_Code"]);
                        strDescription = ddlList.SelectedItem.Text;
                        ddlList.ClearDropDownList();
                    }
                    if (ddlList != null && ddlList.Items.Count > 0)
                        ddlList.ClearDropDownList();
                }
            }
            txtMappingDescription.Text = strDescription;
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Location Mapping details");
        }
    }
    private void FunProBindLocationDetails(int intCompanyId, int intUserId, int intLocationCat_ID)
    {
        Dictionary<string, string> objProcedureParameter;
        DataTable dt = new DataTable();
        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@Company_ID", Convert.ToString(intCompanyId));
        objProcedureParameter.Add("@User_ID", Convert.ToString(intCompanyId));
        objProcedureParameter.Add("@LocationMap_ID", Convert.ToString(intCompanyId));
        objProcedureParameter.Add("@Location_Category_ID", Convert.ToString(intLocationCat_ID));
        dt = Utility.GetDefaultData("S3G_SA_GET_LOCATTADDRESS", objProcedureParameter);
        if (dt.Rows.Count > 0)
        {
            txtComAddress1.Text = dt.Rows[0]["Loc_Address1"].ToString();
            txtCOmAddress2.Text = dt.Rows[0]["Loc_Address2"].ToString();
            txtComCity.Text = dt.Rows[0]["Loc_City"].ToString();
            txtComCountry.Text = dt.Rows[0]["Loc_Country"].ToString();
            txtComState.SelectedValue = dt.Rows[0]["Loc_State_ID"].ToString();
            txtComEmail.Text = dt.Rows[0]["Loc_Email"].ToString();
            txtCommLandmark.Text = dt.Rows[0]["Loc_Landmark"].ToString();
            txtComWebsite.Text = dt.Rows[0]["Loc_Website"].ToString();
            txtComTelephone.Text = dt.Rows[0]["Loc_Telephone"].ToString();
            txtComMobile.Text = dt.Rows[0]["Loc_Mobile"].ToString();
            txtComPincode.Text = dt.Rows[0]["Loc_Pincode"].ToString();
            if (dt.Rows[0]["Is_DealerType"].ToString() == "True")
            {
                rblisdealertype.SelectedValue = "1";
            }
            else
                rblisdealertype.SelectedValue = "0";
            txtVATTIN.Text = dt.Rows[0]["VATTIN"].ToString();
            txtCSTTIN.Text = dt.Rows[0]["CSTTIN"].ToString();
            txtWCT.Text = dt.Rows[0]["WCT"].ToString();

            txtVATDate.Text = dt.Rows[0]["VATDATE"].ToString();
            txtCSTDate.Text = dt.Rows[0]["CSTDATE"].ToString();
            txtWCTDate.Text = dt.Rows[0]["WCTDATE"].ToString();

            txtSGSTin.Text = dt.Rows[0]["SGSTIN"].ToString();
            TxtSGSTRegDate.Text = dt.Rows[0]["SGST_Reg_DATE"].ToString();
            txtGSTStateCode.Text = dt.Rows[0]["GST_State_Code"].ToString();
            
        }
        else
        {
            txtComAddress1.Text = string.Empty;
            txtCOmAddress2.Text = string.Empty;
            txtComCity.Text = string.Empty;
            txtComCountry.Text = string.Empty;
            txtComState.SelectedValue = "0";
            txtComEmail.Text = string.Empty;
            txtCommLandmark.Text = string.Empty;
            txtComWebsite.Text = string.Empty;
            txtComTelephone.Text = string.Empty;
            txtComMobile.Text = string.Empty;
            txtComPincode.Text = string.Empty;
        }
    }

    public void FunPubLoadActiveHierarchy()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", usrInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@IsActive", "1");
            DataTable dtHierarchy = Utility.GetDefaultData("S3G_SYSAD_GetHierachyMasterDetails", dictParam);
            dtHierarchyWidth = dtHierarchy.Copy();
            ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            AjaxControlToolkit.TabContainer tcDefMap = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocCategory");
            AjaxControlToolkit.TabPanel tpLocDef = new AjaxControlToolkit.TabPanel();
            int intCount = 0;
            foreach (DataRow drow in dtHierarchy.Rows)
            {
                tpLocDef = new AjaxControlToolkit.TabPanel();
                tpLocDef.ID = "tpLocDef" + Convert.ToString(drow["Location_Description"]);
                tpLocDef.HeaderText = Convert.ToString(drow["Location_Description"]);
                tcDefMap.Tabs.Add(tpLocDef);
                intCount++;
            }
            if (tcLocCategory.ActiveTab != null)
            {
                DataRow[] drow = dtHierarchyWidth.Select("Location_Description='" + tcLocCategory.ActiveTab.HeaderText + "'");
                if (drow != null && drow.Count() > 0)
                    txtLocationCode.MaxLength = Convert.ToInt32(drow[0]["Width"]);
                lblLocationCode.Text = tcLocCategory.ActiveTab.HeaderText + " Code";// "Location " + 
                lblLocationDescription.Text = tcLocCategory.ActiveTab.HeaderText + " Description";//"Location " +
                if (tcLocCategory.Tabs.Count > 0 && tcLocCategory.ActiveTabIndex != 0)
                    lblParent.Text = tcLocCategory.Tabs[tcLocCategory.ActiveTabIndex - 1].HeaderText;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    public void FunPubLoadHierarchyForMapping()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", usrInfo.ProCompanyIdRW.ToString());
            DataTable dtLocCategory = Utility.GetDefaultData("S3G_SYSAD_GetAllLocationCategory", dictParam);
            dictParam.Add("@IsActive", "1");
            DataTable dtHierarchy = Utility.GetDefaultData("S3G_SYSAD_GetHierachyMasterDetails", dictParam);
            ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            AjaxControlToolkit.TabContainer tcDefMap = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocationMapping");
            AjaxControlToolkit.TabPanel tpLocDef = new AjaxControlToolkit.TabPanel();
            int intCount = 0;
            strarrControlsID = new string[dtHierarchy.Rows.Count];

            DropDownList ddlHierarchy = new DropDownList();
            foreach (DataRow drow in dtHierarchy.Rows)
            {
                ddlHierarchy = new DropDownList();
                tpLocDef = new AjaxControlToolkit.TabPanel();
                tpLocDef.ID = "tpLocDef" + Convert.ToString(drow["Location_Description"]);
                tpLocDef.HeaderText = Convert.ToString(drow["Location_Description"]);
                ddlHierarchy.ID = "ddlHierarchy" + Convert.ToString(drow["Location_Description"]);
                ddlHierarchy.Width = Unit.Pixel(200);
                DataRow[] drLoc = dtLocCategory.Select("Hierarchy_Type='" + drow["Hierachy"] + "'");
                if (drLoc != null && drLoc.Count() > 0)
                {
                    ddlHierarchy.DataTextField = "LocationCat_Description";
                    ddlHierarchy.DataValueField = "Location_Category_ID";// "Location_Code";
                    ddlHierarchy.AutoPostBack = true;
                    ddlHierarchy.DataSource = drLoc.CopyToDataTable();
                    ddlHierarchy.DataBind();
                    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                    ddlHierarchy.Items.Insert(0, liSelect);
                    ddlHierarchy.SelectedIndexChanged += new EventHandler(ddlHierarchy_SelectedIndexChanged);
                }
                strarrControlsID[intCount] = Convert.ToString(drow["Location_Description"]);
                tpLocDef.Controls.Add(ddlHierarchy);

                tcDefMap.Tabs.Add(tpLocDef);
                intCount++;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    void ddlHierarchy_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnSaveLocationMapping.Enabled = false;
        if (((DropDownList)sender).SelectedIndex > 0 && FunPubCheckExistingMapping(Convert.ToString(((DropDownList)sender).SelectedValue)) == 0)
        {
            btnSaveLocationMapping.Enabled = true;
            //  ((DropDownList)sender).SelectedIndex = 0;

        }
        //throw new NotImplementedException();
    }

    public DataTable FunPubCreateLocationTableStructure()
    {
        DataTable dtLocation = new DataTable();
        dtLocation.Columns.Add("SNo", typeof(Int32));
        dtLocation.Columns.Add("Location_Code", typeof(String));
        dtLocation.Columns.Add("Location_Description", typeof(String));
        dtLocation.Columns.Add("Hierarchy_Description", typeof(String));
        dtLocation.Columns.Add("Hierarchy_Type", typeof(Int32));
        dtLocation.Columns.Add("CurrenctMapping_Code", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("CurrenctDisplay", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("Mapping_Description", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("Is_Active", typeof(bool)).DefaultValue = true;
        dtLocation.Columns.Add("Remove", typeof(string)).DefaultValue = "Remove";
        dtLocation.Columns.Add("Loc_Parent_Code", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("Parent_Code", typeof(string)).DefaultValue = "";
        //Code Added by Saran on 12-Jan-2012 to achieve Operational Location start
        dtLocation.Columns.Add("Is_Operational", typeof(bool)).DefaultValue = true;
        //Code Added by Saran on 12-Jan-2012 to achieve Operational Location end

        //Code Added by vino on 17-Oct-2014 
        dtLocation.Columns.Add("Is_DealerType", typeof(bool)).DefaultValue = true;
        dtLocation.Columns.Add("VATTIN", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("CSTTIN", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("WCT", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("VAT_Date", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("CST_Date", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("WCT_Date", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("GSTIN", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("GST_Reg_Date", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("GST_State_Code", typeof(string)).DefaultValue = "";
        dtLocation.Columns.Add("Effective_From", typeof(string)).DefaultValue = "";
        return dtLocation;
    }

    public void FunPubLoadlocationDetailsBasedOnHierarchy()
    {
        try
        {
            ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            AjaxControlToolkit.TabContainer tcDefMap = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocCategory");
            if (((DataTable)ViewState["dt_Location"]) != null)
            {
                DataTable dtList = new DataTable();
                AjaxControlToolkit.TabPanel tpLocDef = new AjaxControlToolkit.TabPanel();
                foreach (AjaxControlToolkit.TabPanel tp in tcDefMap.Tabs)
                {
                    tpLocDef = (AjaxControlToolkit.TabPanel)tcDefMap.FindControl("tpLocDef" + tp.HeaderText);
                    DataRow[] drlist = ((DataTable)ViewState["dt_Location"]).Select("Hierarchy_Description='" + Convert.ToString(tp.HeaderText) + "'");
                    GridView gv = new GridView();
                    gv.ID = "gv" + tp.HeaderText;
                    tpLocDef.Controls.Remove(gv);
                    if (drlist != null && drlist.Count() > 0)
                    {
                        dtList = drlist.CopyToDataTable();
                        gv.RowDataBound += new GridViewRowEventHandler(gv_RowDataBound);
                        gv.EmptyDataText = "No Records found";
                        gv.Width = Unit.Percentage(98);
                        gv.AutoGenerateColumns = true;
                        gv.DataSource = dtList;
                        gv.DataBind();
                        tpLocDef.Controls.Add(gv);
                    }
                    else
                    {
                        gv.DataSource = ((DataTable)ViewState["dt_Location"]).Clone();
                        gv.DataBind();
                        tpLocDef.Controls.Add(gv);
                    }
                }
            }
            if (tcLocCategory.ActiveTab != null)
            {
                DataRow[] drow = dtHierarchyWidth.Select("Location_Description='" + tcLocCategory.ActiveTab.HeaderText + "'");
                if (drow != null && drow.Count() > 0)
                    txtLocationCode.MaxLength = Convert.ToInt32(drow[0]["Width"]);
                lblLocationCode.Text = tcLocCategory.ActiveTab.HeaderText + " Code"; // "Location " +
                lblLocationDescription.Text = tcLocCategory.ActiveTab.HeaderText + " Description"; //"Location " +
                if (tcLocCategory.Tabs.Count > 0 && tcLocCategory.ActiveTabIndex != 0)
                    lblParent.Text = tcLocCategory.Tabs[tcLocCategory.ActiveTabIndex - 1].HeaderText;
                txtLocationName.Text = "";
                txtLocationCode.Text = "";


                if (tcLocCategory.ActiveTabIndex == 1)
                {
                    lblCSTTIN.Visible = true;
                    lblVATTIN.Visible = true;
                    lblWCT.Visible = true;

                    txtVATTIN.Visible = true;
                    txtCSTTIN.Visible = true;
                    txtWCT.Visible = true;

                    lblVATDate.Visible = true;
                    lblCSTDate.Visible = true;
                    lblWCTDate.Visible = true;

                    txtVATDate.Visible = true;
                    txtCSTDate.Visible = true;
                    txtWCTDate.Visible = true;

                    imgVATDate.Visible = true;
                    Image1.Visible = true;
                    Image2.Visible = true;


                    lbldealertype.Visible = true;
                    rblisdealertype.Visible = true;
                    txtSGSTin.Visible = true;
                    TxtSGSTRegDate.Visible = txtGSTStateCode.Visible = true;
                    lblSGSTIN.Visible = true;
                    lblSGSTREGDate.Visible = lblGSTStateCode.Visible = true;
                    imgSGSTRegDate.Visible = true;
                }
                else
                {
                    lblCSTTIN.Visible = false;
                    lblVATTIN.Visible = false;
                    lblWCT.Visible = false;

                    txtVATTIN.Visible = false;
                    txtCSTTIN.Visible = false;
                    txtWCT.Visible = false;

                    lblVATDate.Visible = false;
                    lblCSTDate.Visible = false;
                    lblWCTDate.Visible = false;

                    txtVATDate.Visible = false;
                    txtCSTDate.Visible = false;
                    txtWCTDate.Visible = false;

                    imgVATDate.Visible = false;
                    Image1.Visible = false;
                    Image2.Visible = false;

                    lbldealertype.Visible = false;
                    rblisdealertype.Visible = false;
                    txtSGSTin.Visible = false;
                    TxtSGSTRegDate.Visible = txtGSTStateCode.Visible = false;
                    lblSGSTIN.Visible = false;
                    lblSGSTREGDate.Visible = lblGSTStateCode.Visible = false;
                    imgSGSTRegDate.Visible = false;
                }
            }
            if (ViewState["dt_Location"] != null && ((DataTable)ViewState["dt_Location"]).Rows.Count > 0)
            {
                int intTabCount = tcDefMap.Tabs.Count;
                for (int i = 0; i < tcDefMap.Tabs.Count; i++)
                {
                    if (i != tcDefMap.ActiveTabIndex)
                    {
                        tcDefMap.Tabs[i].Enabled = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    public void FunPubLoadParentMappingDropDown(int intCurrentTabIndex)
    {
        try
        {
            DataTable dtLocations = new DataTable();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", usrInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@Hierarchy_Type", (intCurrentTabIndex).ToString());
            DataTable dtExisting = Utility.GetDefaultData("S3G_SYSAD_GetLocationCategory", dictParam);
            dtExisting.Columns["Location_Code"].ColumnName = "CurrenctMapping_Code";
            dtExisting.Columns["LocationCat_Description"].ColumnName = "Mapping_Description";
            if (((DataTable)ViewState["dt_Location"]) != null)
            {
                dtLocations = ((DataTable)ViewState["dt_Location"]);
                if (dtLocations.Rows.Count > 0)
                {
                    DataRow[] drLocations = dtLocations.Select("Hierarchy_Type='" + Convert.ToString(intCurrentTabIndex) + "'");
                    if (drLocations != null && drLocations.Length > 0)
                    {
                        ddlParent.DataSource = drLocations.CopyToDataTable();
                        ddlParent.DataBind();
                        dtExisting.Merge(drLocations.CopyToDataTable(), false, MissingSchemaAction.Add);
                    }
                    else
                        FunPubLoadDropDownList(dtExisting);
                }
                else
                    FunPubLoadDropDownList(dtExisting);
            }
            else
                FunPubLoadDropDownList(dtExisting);
            ddlParent.Items.Insert(0, new ListItem("--Select--", "--Select--"));
            ddlParent.AddItemToolTipValue();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    private void FunPubLoadDropDownList(DataTable dtExisting)
    {
        ddlParent.DataSource = dtExisting;
        ddlParent.DataBind();
    }

    void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Text = "Location Code";
            e.Row.Cells[2].Text = "Location Description";
            e.Row.Cells[3].Text = "Hierarchy Description";
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = true;
            e.Row.Cells[5].Text = "Current Mapping";
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[7].Text = "Mapping Description";
            e.Row.Cells[8].Visible = true;
            e.Row.Cells[8].Text = "Active";
            e.Row.Cells[9].Visible = e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Text = "Operational Location";
            e.Row.Cells[12].Text = "Operational";
            e.Row.Cells[13].Text = "Is Dealer Type";
            e.Row.Cells[17].Text = "VAT Regn Date";
            e.Row.Cells[18].Text = "CST Regn Date";
            e.Row.Cells[19].Text = "WCT Regn Date";
            e.Row.Cells[21].Text = "GST Regn Date";
            e.Row.Cells[22].Text = "GST State Code";

            if (tcLocCategory.ActiveTabIndex != 1)
            {
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false;
                e.Row.Cells[18].Visible = false;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = true;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = true;
            e.Row.Cells[9].Visible = e.Row.Cells[10].Visible = false;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;
            LinkButton lnkRemove = new LinkButton();
            lnkRemove.Text = "Remove";
            lnkRemove.CommandArgument = e.Row.Cells[0].Text;
            lnkRemove.Click += new EventHandler(lnkRemove_Click);

            e.Row.Cells[6].Controls.Add(lnkRemove);
            e.Row.Cells[6].Visible = false;

            if (tcLocCategory.ActiveTabIndex != 1)
            {
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false;
                e.Row.Cells[18].Visible = false;
            }
        }
    }

    void lnkRemove_Click(object sender, EventArgs e)
    {
        string strSNo = ((LinkButton)sender).CommandArgument;
        if (((DataTable)ViewState["dt_Location"]) != null)
        {
            DataTable dtList = ((DataTable)ViewState["dt_Location"]);
            DataRow[] drow = dtList.Select("SNo='" + strSNo + "'");
            if (drow != null && drow.Count() > 0)
            {
                dtList.Rows.Remove(drow[0]);
                dtList.AcceptChanges();
                ViewState["dt_Location"] = dtList;
            }
        }
        FunPubLoadlocationDetailsBasedOnHierarchy();
    }

    public void FunPubInsertLocationCategory()
    {
        objAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
        objLocationCategory_DataTable = new S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable();
        objLocationCategory_DataRow = objLocationCategory_DataTable.NewS3G_SYSAD_LocationCategoryRow();
        try
        {
            String StrQryStr = "";
            if (Request.QueryString["qsType"] != null)
                StrQryStr = Request.QueryString["qsType"].ToString().Trim();

            if (strMode != "Q" && strMode != "M")
            {
                if (((DataTable)ViewState["dtAddress"]) == null && ((DataTable)ViewState["dtAddress"]).Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Address Details should be added.");
                    return;
                }
                if (((DataTable)ViewState["dt_Location"]) != null && ((DataTable)ViewState["dt_Location"]).Rows.Count > 0)
                {
                    string strLocationDetails = ((DataTable)ViewState["dt_Location"]).FunPubFormXml();
                    objLocationCategory_DataRow.Company_ID = intCompanyId;
                    objLocationCategory_DataRow.Created_By = intUserId;
                    objLocationCategory_DataRow.XMLLocationDetails = strLocationDetails;
                    objLocationCategory_DataRow.XMLAddressDetails = ((DataTable)ViewState["dtAddress"]).FunPubFormXml();
                    objLocationCategory_DataTable.AddS3G_SYSAD_LocationCategoryRow(objLocationCategory_DataRow);
                    string strExistingCode = string.Empty;
                    string strExistingDescrption = string.Empty;
                    intErrorCode = objAdminService.FunPubInsertLocationCategory(out strExistingCode, out strExistingDescrption, SerMode, ClsPubSerialize.Serialize(objLocationCategory_DataTable, SerMode));
                    if (intErrorCode == 30)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Already Existing Details : \\n Location Code :- " + strExistingCode + " \\n Location Description :- " + strExistingDescrption);
                        FunPubLoadlocationDetailsBasedOnHierarchy();
                        return;
                    }
                    else if (intErrorCode == 40 || intErrorCode == 50)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Error Occured in Location Defined");
                        FunPubLoadlocationDetailsBasedOnHierarchy();
                        return;
                    }
                    else if (intErrorCode == 0)
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnSaveLocCategory.Enabled = false;
                        //End here
                        Utility.FunShowAlertMsg(this.Page, "Location Definition Added successfully", strRedirectPage + "?Type=" + strType + "&Level=" + HttpUtility.UrlEncode(StrQryStr) + "&Hir=" + tcLocCategory.ActiveTabIndex.ToString());
                        FunPubClearCategoryuEntry();
                        ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
                        AjaxControlToolkit.TabContainer tcDefMap = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocCategory");
                        for (int i = 0; i < tcDefMap.Tabs.Count; i++)
                        {
                            tcDefMap.Tabs[i].Enabled = true;
                        }
                        ((DataTable)ViewState["dt_Location"]).Rows.Clear();
                    }
                    else if (intErrorCode == 106)
                    {
                        Utility.FunShowAlertMsg(this.Page, "GSTIN already exists.");
                        FunPubLoadlocationDetailsBasedOnHierarchy();
                        return;
                    }
                    else if (intErrorCode == 107)
                    {
                        Utility.FunShowAlertMsg(this.Page, "GST State Code already exists.");
                        FunPubLoadlocationDetailsBasedOnHierarchy();
                        return;
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this.Page, "Error Occured in Location Defined");
                        FunPubLoadlocationDetailsBasedOnHierarchy();
                        return;
                    }
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Atleast one Category should be added.");
                    return;
                }
            }
            else if (strMode == "M")
            {
                objLocationCategory_DataRow.Company_ID = intCompanyId;
                objLocationCategory_DataRow.Modified_By = intUserId;
                objLocationCategory_DataRow.Location_Category_ID = intLocationCat_ID;
                // changed by bhuvan for sakthi fin
                objLocationCategory_DataRow.VATTIN = txtVATTIN.Text.Trim();
                objLocationCategory_DataRow.CSTTIN = txtCSTTIN.Text.Trim();
                objLocationCategory_DataRow.WCT = txtWCT.Text.Trim();

                if (txtVATDate.Text != "")
                    objLocationCategory_DataRow.VATDATE = Utility.StringToDate(txtVATDate.Text.Trim());
                if (txtCSTDate.Text != "")
                    objLocationCategory_DataRow.CSTDATE = Utility.StringToDate(txtCSTDate.Text.Trim());
                if (txtWCTDate.Text != "")
                    objLocationCategory_DataRow.WCTDATE = Utility.StringToDate(txtWCTDate.Text.Trim());

                objLocationCategory_DataRow.Location_Description = txtLocationName.Text;
                objLocationCategory_DataRow.XMLAddressDetails = ((DataTable)ViewState["dtAddress"]).FunPubFormXml();
                //end here
                objLocationCategory_DataRow.Is_Active = Convert.ToBoolean(cbxActive.Checked);

                objLocationCategory_DataRow.SGSTIN = txtSGSTin.Text.Trim();
                if (TxtSGSTRegDate.Text != "")
                    objLocationCategory_DataRow.SGST_Reg_Date = Utility.StringToDate(TxtSGSTRegDate.Text.Trim());

                objLocationCategory_DataRow.GST_State_Code = txtGSTStateCode.Text;
                objLocationCategory_DataRow.Effective_From = Utility.StringToDate(txtAddressEffectiveFrom.Text.Trim());

                objLocationCategory_DataTable.AddS3G_SYSAD_LocationCategoryRow(objLocationCategory_DataRow);
                intErrorCode = objAdminService.FunPubUpdateLocationCategory(SerMode, ClsPubSerialize.Serialize(objLocationCategory_DataTable, SerMode));
                if (intErrorCode == 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSaveLocCategory.Enabled = false;
                    //End here
                    Utility.FunShowAlertMsg(this.Page, "Location Definition Updated successfully", strRedirectPage + "?Type=" + strType + "&Level=" + HttpUtility.UrlEncode(StrQryStr) + "&Hir=" + Request.QueryString["Hir"].ToString());
                }
                else if (intErrorCode == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, "Location Description already exists.");
                    return;
                }
                else if (intErrorCode == 31)
                {
                    Utility.FunShowAlertMsg(this.Page, "The Location has mapped, cannot be modified.");
                    return;
                }
                else if (intErrorCode == 106)
                {
                    Utility.FunShowAlertMsg(this.Page, "GSTIN already exists.");
                    return;
                }
                else if (intErrorCode == 107)
                {
                    Utility.FunShowAlertMsg(this.Page, "GST State Code already exists.");
                    return;
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Error Occured in Location Defined");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
        finally
        {
            if (objAdminService != null)
                objAdminService.Close();
        }
    }

    public void FunPubInsertLocationMaster()
    {
        objAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
        objLocationMaster_DataTable = new S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationMasterDataTable();
        objLocationMaster_DataRow = objLocationMaster_DataTable.NewS3G_SYSAD_LocationMasterRow();
        string strExistingDetails = string.Empty;
        try
        {
            if (strMode != "Q" && strMode != "M")
            {
                //FunPubCheckExistingMapping();


                StringBuilder strb = new StringBuilder();
                strb.Append("<Root><Details ");
                strb.Append("Location_Code='" + hdnMappingCode.Value.ToString() + "  Is_Operational='" + cbxOperationalLocM.Checked + " '/></Root>");
                // string strLocationDetails = ((DataTable)ViewState["dt_LocationMapping"]).FunPubFormXml();
                objLocationMaster_DataRow.Company_ID = intCompanyId;
                objLocationMaster_DataRow.Created_By = intUserId;
                objLocationMaster_DataRow.XMLLocationMasterDetails = strb.ToString();// strLocationDetails;
                objLocationMaster_DataTable.AddS3G_SYSAD_LocationMasterRow(objLocationMaster_DataRow);
                intErrorCode = objAdminService.FunPubInsertLocationMaster(out strExistingDetails, SerMode, ClsPubSerialize.Serialize(objLocationMaster_DataTable, SerMode));
                if (intErrorCode == 0 && strExistingDetails != string.Empty)
                {
                    Utility.FunShowAlertMsg(this.Page, "The location mapping code : " + strExistingDetails + "-already exist.");
                    return;
                }
                else if (intErrorCode == 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSaveLocationMapping.Enabled = false;
                    //End here
                    Utility.FunShowAlertMsg(this.Page, "Location Mapping Added successfully.");
                    FunPubClearMappingEntry();
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Error Occured in Location Master.");
                    return;
                }
            }
            else if (strMode == "M")
            {
                objLocationMaster_DataRow.Company_ID = intCompanyId;
                objLocationMaster_DataRow.Modified_By = intUserId;
                objLocationMaster_DataRow.Location_ID = intLocationMap_ID;
                objLocationMaster_DataRow.Is_Active = Convert.ToBoolean(cbxActiveMapping.Checked);
                //Code Added by Saran on 12-Jan-2012 to achieve Operational Location start
                objLocationMaster_DataRow.Is_Operational = Convert.ToBoolean(cbxOperationalLocM.Checked);
                //Code Added by Saran on 12-Jan-2012 to achieve Operational Location end
                objLocationMaster_DataTable.AddS3G_SYSAD_LocationMasterRow(objLocationMaster_DataRow);
                intErrorCode = objAdminService.FunPubUpdateLocationMapping(SerMode, ClsPubSerialize.Serialize(objLocationMaster_DataTable, SerMode));
                if (intErrorCode == 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSaveLocationMapping.Enabled = false;
                    //End here
                    Utility.FunShowAlertMsg(this.Page, "Location Mapping details Updated successfully", strRedirectPage + "?Type=" + strType);
                }
                else if (intErrorCode == 59)
                {
                    Utility.FunShowAlertMsg(this.Page, "Lower level Location has referring with selected location, deactive low level to top level");
                    return;
                }
                else if (intErrorCode == 58)
                {
                    Utility.FunShowAlertMsg(this.Page, "Active top level Location");
                    return;
                }
                else if (intErrorCode == 60)
                {
                    Utility.FunShowAlertMsg(this.Page, "Selected location should be active in Location Definition");
                    return;
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Error Occured in Location Mapping updation");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
        finally
        {
            if (objAdminService != null)
                objAdminService.Close();
        }

    }

    #region [Clear Function]

    public void FunPubClearCategoryuEntry()
    {
        rblHierarchy.ClearSelection();
        txtLocationCode.Text = txtLocationName.Text = lblMappingCodeDescription.Text = "";

        //Code Added by Saran on 12-Jan-2012 to achieve Operational Location start
        cbxOperationalLoc.Checked = false;
        //Code Added by Saran on 12-Jan-2012 to achieve Operational Location end
        txtVATTIN.Text = "";
        txtCSTTIN.Text = "";
        txtWCT.Text = "";


        txtVATDate.Text = "";
        txtCSTDate.Text = "";
        txtWCTDate.Text = "";
        txtSGSTin.Text = TxtSGSTRegDate.Text = txtGSTStateCode.Text = "";

        ddlParent.ClearSelection();
    }

    public void FunPubClearMappingEntry()
    {
        txtLocationMappingCode.Text = "";
        foreach (string str in strarrControlsID)
        {
            DropDownList ddlList = new DropDownList();
            ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            AjaxControlToolkit.TabContainer tcDefMap = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocationMapping");
            AjaxControlToolkit.TabPanel tpLocDef = (AjaxControlToolkit.TabPanel)tcDefMap.FindControl("tpLocDef" + str);
            if (tpLocDef != null)
            {
                ddlList = ((DropDownList)tpLocDef.FindControl("ddlHierarchy" + str));
                ddlList.SelectedIndex = 0;
            }
            tcDefMap.ActiveTabIndex = 0;
        }
    }

    #endregion [Clear Function]

    #region [Validation for Current Existing mapping check]
    public int FunPubCheckExistingMapping(string strCurrentMappingCode)
    {
        int intErrorCode = 0;
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminClient = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {
            ContentPlaceHolder CPH = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            AjaxControlToolkit.TabContainer tcDefMap = (AjaxControlToolkit.TabContainer)CPH.FindControl("tcLocationMapping");
            AjaxControlToolkit.TabPanel tcParrent = tcDefMap.ActiveTabIndex == 0 ? (AjaxControlToolkit.TabPanel)tcDefMap.ActiveTab : (AjaxControlToolkit.TabPanel)tcDefMap.Tabs[tcDefMap.ActiveTabIndex - 1];
            DropDownList ddlParent = (DropDownList)tcParrent.FindControl("ddlHierarchy" + tcParrent.HeaderText);

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", usrInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@Current_MapCode", strCurrentMappingCode);
            dictParam.Add("@Current_Level", (tcDefMap.ActiveTabIndex + 1).ToString());
            dictParam.Add("@Parrent_Code", (tcDefMap.ActiveTabIndex > 0 ? ddlParent.SelectedValue : "").ToString());
            dictParam.Add("@CurrentMapping", hdnMappingCode.Value);
            intErrorCode = Convert.ToInt32(ObjS3GAdminClient.FunGetScalarValue("S3G_SYSAD_ExistLocationMapping", dictParam));
            dictParam.Clear();
            if (intErrorCode == 60 || intErrorCode == 63)
            {
                for (int i = tcDefMap.ActiveTabIndex + 1; i < tcDefMap.Tabs.Count; i++)
                {
                    tcDefMap.Tabs[i].Enabled = false;
                }
            }
            else
            {
                for (int i = tcDefMap.ActiveTabIndex + 1; i < tcDefMap.Tabs.Count; i++)
                {
                    tcDefMap.Tabs[i].Enabled = true;
                }
            }
            if (intErrorCode == 61)
            {
                //Utility.FunShowAlertMsg(this.Page, "Select Location already mapped");
            }
            else if (intErrorCode == 62)
            {
                Utility.FunShowAlertMsg(this.Page, "Upper Level Locations not mapped");
            }
            else if (intErrorCode == 63)
            {
                Utility.FunShowAlertMsg(this.Page, "Select Location has mapped with other upper level location");
            }
        }
        catch (Exception ex)
        { }
        finally
        {
            if (ObjS3GAdminClient != null)
                ObjS3GAdminClient.Close();
        }
        return intErrorCode == 60 ? 0 : intErrorCode;
    }
    #endregion [Validation for Current Existing mapping check]

    #endregion [Function's]


    protected void ddlParent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMappingCodeDescription.Text = "";
        FunPubLoadlocationDetailsBasedOnHierarchy();
        if (ddlParent.SelectedIndex > 0)
            lblMappingCodeDescription.Text = ddlParent.SelectedItem.Value;
    }

    #region Common City Bind Method
    [System.Web.Services.WebMethod]
    public static string[] GetCityList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@PrefixText", prefixText.Trim());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SA_GET_CITY_LIST", Procparam));
        return suggetions.ToArray();
    }
    #endregion
}
