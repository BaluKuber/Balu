
#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Orgination
/// Screen Name         :   Business Target Master
/// Created By          :   Ponnurajesh.C
/// Created Date        :   10-April-2012
/// Purpose             :   This screen is used to create/set/update Business Targets.
/// <Program Summary>
#endregion

using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Xml.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services;
using System.Globalization;
using System.Text;
using OrgMasterMgtServicesReference;

#region Class Origination_S3GTargetMaster

public partial class Origination_S3GORGTargetMaster_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyId, intUserId = 0;
    int intMaxMonth;
    int intLRNId = 0;
    int ptype;
    string strRedirectPage = "S3GORGTargetMaster_Add.aspx";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGTargetMaster_Add.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=TRG';";
    string strRedirectModify = "S3GORGTransLander.aspx?Code=TRG";
    string updateStatus = "U";
    static int intCustomer = 0;
    int intPricing_ID = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int intRow = 0;
    int inttargmstr = 0;
    int intErrCode = 0;
    string strCreate = string.Empty;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    StringBuilder strbTargetMasterDetails = new StringBuilder();
    StringBuilder strcpyTargetMasterDetails = new StringBuilder();
    UserInfo uinfo = null;
    Dictionary<string, string> dictDropdownListParam;
    Dictionary<string, string> Procparam = null;
    SerializationMode SerMode = SerializationMode.Binary;
    S3GSession ObjS3GSession = new S3GSession();
    UserInfo ObjUserInfo = new UserInfo();
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //OrgMasterMgtServicesClient ObjOrgMgtServicesClient = null;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMgtServicesClient;
    OrgMasterMgtServices.S3G_ORG_TargetMasterDataTable ObjS3G_ORG_TargetMasterDataTable = null;
    OrgMasterMgtServices.S3G_ORG_TargetMasterRow ObjS3G_ORG_TargetMasterRow = null;
    OrgMasterMgtServices.S3G_ORG_TargetMasterDetailsDataTable ObjS3G_ORG_TargetMasterDetailsDataTable = null;
    OrgMasterMgtServices.S3G_ORG_TargetMasterDetailsRow ObjS3G_ORG_TargetMasterDetailsRow = null;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
    DataTable targetasset = new DataTable();
    string strDebtCollectorID = "";
    string strXMLTargetMasterDetails = "<Root><Details Desc='0' /></Root>";
    string strXMLcpyTargetMasterDetails = "<Root><Details Desc='0' /></Root>";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strPeriod = "1";
    string strFinYear = "";
    string mon = "";
    string strFinMonth = "";
    int intFinMonth = 0;
    int intFinMonForCalc = 0;
    ArrayList arrList = new ArrayList(12);
    //string strDateFormat;
    static string strPageName = "Target Master";
    string strAssetclass;
    string strAssetmake;
    string strAssetclassvalue;
    string strAssetmakevalue;
    string strTargetAmount;
    string strCommissionPercentage;
    string strUnits;
    string strSplCommisionPercentage;
    int intBusinessTargetId = 0;
    public static Origination_S3GORGTargetMaster_Add obj_Page;
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        //txtLegalCutOffDate.Attributes.Add("readonly", "readonly");
        //Date Format
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;

        if (intCompanyID == 0)
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
        if (intUserID == 0)
            intUserID = ObjUserInfo.ProUserIdRW;
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];
        if (strMode == "M")
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
        else if (strMode == "Q")
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
        //Code end


        //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        //FunPriBindGrid();
        obj_Page = this;
        if (!IsPostBack)
        {
            //FunToolTip();
            FunPriClearControls();
            FunPriCleardropdown();

            FunPriLoadLOV();
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
                ddlLOB_SelectedIndexChanged(sender, e);
            }

            if (targetasset.Rows.Count == 0)
            {
                FunPriInsertTargetAssetDataTable("-1", "-1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            else
            {
                FunPubBindTargetAsset(targetasset);
            }


            if (strMode == "M" || strMode == "Q")
                Funpriload();

        }

    }
    private void Funpriload()
    {
        try
        {
            chkcpytrg.Enabled = false;
            DataSet ds = new DataSet();
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (strMode == "Q")
            {
                chkActive.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                txtportfolirr.Enabled = false;
                txtMKTExeNm.Enabled = false;
                txtMKTExecd.Enabled = false;
                txtAddress.Enabled = false;
                txtdbtcllctr.Enabled = false;
                txtemailaddr.Enabled = false;
                txtMobile.Enabled = false;
                ddlLOB.Enabled = false;
                ddlMxecnm.Enabled = false;
                ddlMxectyp.Enabled = false;
                ddlprod.Enabled = false;
                ddlfreqtyp.Enabled = false;
                ddlassetcateg.Enabled = false;
                ddlBranch.Enabled = false;

            }


            Funtrgadd();


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Button Events
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveTargetMaster();

        }
        catch (Exception ex)
        {

        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        FunPriClearControls();
        FunPriCleardropdown();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=TRG");

    }
    #endregion

    #region Pass XML values
    private bool FunPriGenerateTargmastrXMLDet()
    {
        try
        {
            targetasset = (DataTable)ViewState["Target_AssetDetails"];
            if (targetasset.Rows.Count == 1)
            {
                if (targetasset.Rows[0]["Asset_Serial_Number"].ToString().Equals("0"))
                {
                    return false;
                }
            }

            strbTargetMasterDetails.Append("<Root>");
            int i = 0;
            foreach (DataRow drow in targetasset.Rows)
            {
                strbTargetMasterDetails.Append("<Details  Company_ID ='" + Convert.ToString(ObjUserInfo.ProCompanyIdRW) + "' DebtCollector_ID='" + strDebtCollectorID + "'   ");
                strbTargetMasterDetails.Append(" Asset_Serial_Number = '" + drow["Asset_Serial_Number"].ToString() + "'");
                strbTargetMasterDetails.Append(" Asset_Class = '" + drow["Asset_Classvalue"].ToString() + "'");
                strbTargetMasterDetails.Append(" Asset_Make = '" + drow["Asset_Makevalue"].ToString() + "'");
                strbTargetMasterDetails.Append(" Period = '" + drow["Period"].ToString() + "'");
                strbTargetMasterDetails.Append(" Target_Amount = '" + drow["Target_Amount"].ToString() + "'");
                strbTargetMasterDetails.Append(" Units = '" + drow["Units"].ToString() + "'");
                strbTargetMasterDetails.Append(" Commission = '" + drow["Commission"].ToString() + "'");
                strbTargetMasterDetails.Append(" Special_Commission = '" + drow["Special_Commission"].ToString() + "'");
                strbTargetMasterDetails.Append(" />");
            }
            strbTargetMasterDetails.Append("</Root>");
            strXMLTargetMasterDetails = strbTargetMasterDetails.ToString();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        return true;
    }

    private bool FunPriGeneratecpyTrgMstrXMLDet()
    {
        try
        {
            string strMktExeccode = null;
            string strMktExecname = null;


            strcpyTargetMasterDetails.Append("<Root>");
            CheckBox chkSelRoleCode = null;
            RequiredFieldValidator rfvRemarks = null;
            bool isAtleastOneExecutiveSelected = false;
            foreach (GridViewRow grvData in grvcpytrgt.Rows)
            {
                chkSelRoleCode = ((CheckBox)grvData.FindControl("chkSelRoleCode"));
                if (chkSelRoleCode.Checked)
                {
                    isAtleastOneExecutiveSelected = true;
                    strMktExeccode = ((Label)grvData.FindControl("lblRoleCenterID")).Text;
                    strMktExecname = ((Label)grvData.FindControl("lblAssigned")).Text;

                    strcpyTargetMasterDetails.Append(" <Details MARKETING_CODE='" + Convert.ToString(strMktExeccode) + "'");
                    strcpyTargetMasterDetails.Append(" MARKETING_NAME='" + Convert.ToString(strMktExecname) + "' /> ");
                }
            }
            if (isAtleastOneExecutiveSelected == false)
                return false;
            strcpyTargetMasterDetails.Append("</Root>");
            strXMLcpyTargetMasterDetails = strcpyTargetMasterDetails.ToString();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        return true;
    }
    #endregion

    #region General functional Methods

    private void Funtrgadd()
    {
        try
        {

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));


                if (fromTicket != null)
                {
                    intBusinessTargetId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Business Target Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            DataSet ds = new DataSet();
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            if (strMode == "C")
            {
                Procparam.Add("@Companyid", Convert.ToString(intCompanyID));
                Procparam.Add("@LOBID", Convert.ToString(ddlLOB.SelectedValue));
                Procparam.Add("@Product_ID", Convert.ToString(ddlprod.SelectedValue));
                Procparam.Add("@Mktgcode", Convert.ToString(txtMKTExecd.Text.ToString()));
                Procparam.Add("@Assetctg", Convert.ToString(ddlassetcateg.SelectedValue));
                Procparam.Add("@Location_id", Convert.ToString(ddlBranch.SelectedValue));
                ds = Utility.GetDataset(SPNames.S3G_ORG_GetBustrgdetcreate, Procparam);
            }
            else
            {
                Procparam.Add("@businessid", Convert.ToString(intBusinessTargetId));
                ds = Utility.GetDataset(SPNames.S3G_ORG_GetBustargetdet, Procparam);
            }
            if (ds.Tables[0].Rows.Count >= 1)
            {

                ddlLOB.SelectedValue = ds.Tables[0].Rows[0]["LOB_ID"].ToString();
                ddlBranch.SelectedValue = ds.Tables[0].Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = ds.Tables[0].Rows[0]["Location"].ToString();
                FunPriLoadProd();
                ddlprod.SelectedValue = ds.Tables[0].Rows[0]["Product_ID"].ToString();
                ddlMxectyp.SelectedValue = ds.Tables[0].Rows[0]["Marketing_Type"].ToString();
                //int typemex;
                //typemex = Convert.ToInt32(ddlMxectyp.SelectedValue);
                //if (typemex == 1)
                //    typemex = typemex + 1;
                //else if (typemex == 2)
                //    typemex = typemex - 1;

                //FunPriLoadbusexec(typemex);

                ddlMxecnm.SelectedValue = ds.Tables[0].Rows[0]["Marketing_Code"].ToString();
                ddlMxecnm.SelectedText = ds.Tables[0].Rows[0]["Marketing_Name"].ToString();

                Funpriloadbusdet();
                ddlassetcateg.SelectedValue = ds.Tables[0].Rows[0]["Asset_Category"].ToString();
                ddlfreqtyp.SelectedValue = ds.Tables[0].Rows[0]["Frequency_type"].ToString();
                txtportfolirr.Text = ds.Tables[0].Rows[0]["Portfolio_IRR"].ToString();
                if (strMode != "C")
                {
                    txtMKTExecd.Text = ds.Tables[0].Rows[0]["Marketing_Code"].ToString();
                    txtdbtcllctr.Text = ds.Tables[0].Rows[0]["Business_Target_No"].ToString();
                }
                Funloadassetdet();
                int dscount;
                dscount = (ds.Tables[0].Rows.Count);
                for (int i = 0; i <= dscount - 1; i++)
                {
                    DropDownList ddlassetclass = gvTargetMasterDetails.FooterRow.FindControl("ddlassetclass") as DropDownList;
                    ddlassetclass.SelectedValue = ds.Tables[0].Rows[i]["Asset_Class"].ToString();
                    GridViewRow gvRow = (GridViewRow)ddlassetclass.Parent.Parent;
                    DropDownList ddlassetmake = gvTargetMasterDetails.FooterRow.FindControl("ddlassetmake") as DropDownList;
                    Procparam.Clear();
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                    Procparam.Add("@AssetType_ID", Convert.ToString(ddlassetcateg.SelectedValue));
                    Procparam.Add("@Type", "Make");
                    Procparam.Add("@classid", Convert.ToString(ddlassetclass.SelectedItem.Text));
                    DataTable ObjmakeTable = Utility.GetDefaultData("S3G_Org_GetAssetClassMake_List", Procparam);
                    Procparam.Clear();
                    ViewState["ddlassetmake"] = ObjmakeTable;
                    ddlassetmake.BindDataTable(ObjmakeTable, new string[] { "make_id", "make_desc" });
                    ddlassetmake.SelectedValue = ds.Tables[0].Rows[i]["Asset_Make"].ToString();
                    FunPriSetFooterPeriod();
                    TextBox txtTargetAmount = gvTargetMasterDetails.FooterRow.FindControl("txtTargetAmount") as TextBox;
                    TextBox txtUnits = gvTargetMasterDetails.FooterRow.FindControl("txtUnits") as TextBox;
                    TextBox txtCommissionPercentage = gvTargetMasterDetails.FooterRow.FindControl("txtCommissionPercentage") as TextBox;
                    TextBox txtSplCommissionPercentage = gvTargetMasterDetails.FooterRow.FindControl("txtSplCommissionPercentage") as TextBox;
                    txtTargetAmount.Text = ds.Tables[0].Rows[i]["Target_Amount"].ToString();
                    txtUnits.Text = ds.Tables[0].Rows[i]["Units"].ToString();
                    txtCommissionPercentage.Text = ds.Tables[0].Rows[i]["Commission_Percen"].ToString();
                    txtSplCommissionPercentage.Text = ds.Tables[0].Rows[i]["SplCommision_Percen"].ToString();
                    Funmodbind(ds, i);
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void Funmodbind(DataSet ds, int i)
    {
        try
        {
            string strTargetAmount, strCommissionPercentage, strSplCommisionPercentage, strAssetclass, strAssetmake, strUnits;
            TextBox txtTargetAmount = gvTargetMasterDetails.FooterRow.FindControl("txtTargetAmount") as TextBox;
            TextBox txtUnits = gvTargetMasterDetails.FooterRow.FindControl("txtUnits") as TextBox;
            TextBox txtCommissionPercentage = gvTargetMasterDetails.FooterRow.FindControl("txtCommissionPercentage") as TextBox;
            TextBox txtSplCommissionPercentage = gvTargetMasterDetails.FooterRow.FindControl("txtSplCommissionPercentage") as TextBox;
            Label lblPeriodFoot = (Label)gvTargetMasterDetails.FooterRow.FindControl("lblPeriodFoot");
            DropDownList ddlassetclass = gvTargetMasterDetails.FooterRow.FindControl("ddlassetclass") as DropDownList;
            DropDownList ddlassetmake = gvTargetMasterDetails.FooterRow.FindControl("ddlassetmake") as DropDownList;

            //strPeriod = ds.Tables[0].Rows[i]["Period"].ToString();
            //strUnits = ds.Tables[0].Rows[i]["Units"].ToString();
            //strAssetclass = ds.Tables[0].Rows[i]["Asset_Class"].ToString();
            //strAssetmake = ds.Tables[0].Rows[i]["Asset_Make"].ToString();
            //strAssetclassvalue = ddlassetclass.SelectedValue;
            //strAssetmakevalue = ddlassetmake.SelectedValue;
            //strTargetAmount = ds.Tables[0].Rows[i]["Target_Amount"].ToString();
            //strCommissionPercentage = ds.Tables[0].Rows[i]["Commission_Percen"].ToString();
            //strSplCommisionPercentage = ds.Tables[0].Rows[i]["SplCommision_Percen"].ToString();
            strPeriod = lblPeriodFoot.Text.Trim();
            strUnits = txtUnits.Text.Trim();
            strAssetclass = ddlassetclass.SelectedItem.Text.Trim();
            strAssetmake = ddlassetmake.SelectedItem.Text.Trim();
            strAssetclassvalue = ddlassetclass.SelectedValue;
            strAssetmakevalue = ddlassetmake.SelectedValue;
            strTargetAmount = txtTargetAmount.Text.Trim();
            strCommissionPercentage = txtCommissionPercentage.Text.Trim();
            strSplCommisionPercentage = txtSplCommissionPercentage.Text.Trim();
            FunPriInsertTargetAssetDataTable(strAssetclass, strAssetmake, strPeriod, strTargetAmount, strUnits, strCommissionPercentage, strSplCommisionPercentage, strAssetclassvalue, strAssetmakevalue);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void FunPriSaveTargetMaster()
    {
        lblErrorMessage.Text = String.Empty;
        ObjOrgMgtServicesClient = new OrgMasterMgtServicesClient();
        try
        {

            if (!FunPriGenerateTargmastrXMLDet())
            {
                Utility.FunShowAlertMsg(this.Page, "Add atleast one row in Target Details");
                return;
            }
            if ((hidTRG.Value) == "1")
            {

                if (!FunPriGeneratecpyTrgMstrXMLDet())
                {
                    Utility.FunShowAlertMsg(this.Page, "Add atleast one row in Target Details");
                    return;
                }

            }
            if (Page.IsValid)
            {
                ObjS3G_ORG_TargetMasterDataTable = new OrgMasterMgtServices.S3G_ORG_TargetMasterDataTable();
                //ObjS3G_ORG_TargetMasterRow = null;
                ObjS3G_ORG_TargetMasterRow = ObjS3G_ORG_TargetMasterDataTable.NewS3G_ORG_TargetMasterRow();
                ObjS3G_ORG_TargetMasterRow.Company_ID = Convert.ToString(intCompanyID);
                ObjS3G_ORG_TargetMasterRow.LOB_ID = ddlLOB.SelectedValue;
                ObjS3G_ORG_TargetMasterRow.Product_ID = ddlprod.SelectedValue;
                ObjS3G_ORG_TargetMasterRow.Marketing_Type = ddlMxectyp.SelectedValue;
                ObjS3G_ORG_TargetMasterRow.Marketing_Code = txtMKTExecd.Text.ToString();
                ObjS3G_ORG_TargetMasterRow.Location_ID = ddlBranch.SelectedValue;
                ObjS3G_ORG_TargetMasterRow.Frequency_Type = ddlfreqtyp.SelectedValue;
                if (txtportfolirr.Text.Trim() != string.Empty)
                {
                    ObjS3G_ORG_TargetMasterRow.Portfolio_IRR = txtportfolirr.Text.ToString();
                }
                else
                {
                    ObjS3G_ORG_TargetMasterRow.Portfolio_IRR = "0";
                }
                ObjS3G_ORG_TargetMasterRow.Asset_Category = ddlassetcateg.SelectedValue;


                if (txtdbtcllctr.Text != string.Empty)
                    ObjS3G_ORG_TargetMasterRow.Business_Target_No = txtdbtcllctr.Text;
                else
                    ObjS3G_ORG_TargetMasterRow.Business_Target_No = "0";

                ObjS3G_ORG_TargetMasterRow.Is_Active = Convert.ToString(chkActive.Checked);


                ObjS3G_ORG_TargetMasterRow.Created_By = Convert.ToString(intUserId);
                ObjS3G_ORG_TargetMasterRow.Created_On = Convert.ToString(DateTime.Now);
                if (strMode == "M")
                {
                    ObjS3G_ORG_TargetMasterRow.Modified_By = Convert.ToString(intUserId);
                }
                ObjS3G_ORG_TargetMasterRow.XMLParamTargetmasterDet = strXMLTargetMasterDetails;
                ObjS3G_ORG_TargetMasterRow.XMLParamCpyTargetmasterDet = strXMLcpyTargetMasterDetails;
                if (hidTRG.Value == "1")
                    if ((ddlcpytrgMxectyp.SelectedValue) == "2")
                        ObjS3G_ORG_TargetMasterRow.Marketing_Typecpy = "2";
                    else
                        ObjS3G_ORG_TargetMasterRow.Marketing_Typecpy = "1";
                else
                    ObjS3G_ORG_TargetMasterRow.Marketing_Typecpy = "0";
                ObjS3G_ORG_TargetMasterDataTable.AddS3G_ORG_TargetMasterRow(ObjS3G_ORG_TargetMasterRow);

                if (txtdbtcllctr.Text == null || txtdbtcllctr.Text == String.Empty)
                {
                    //ObjOrgMgtServicesClient
                    intErrCode = ObjOrgMgtServicesClient.FunPubCreateTargetMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_TargetMasterDataTable, SerMode));
                }
                else if (txtdbtcllctr.Text != null || txtdbtcllctr.Text != String.Empty)
                {
                    intErrCode = ObjOrgMgtServicesClient.FunPubModifyTargetMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_TargetMasterDataTable, SerMode));
                }
                switch (intErrCode)
                {
                    case 0:
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        //Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._1, strRedirectPage);
                        if (txtdbtcllctr.Text == null || txtdbtcllctr.Text == string.Empty)
                        {
                            // Utility.FunShowAlertMsg(this.Page, "Debt Collector Master " + Resources.ValidationMsgs.S3G_ValMsg_Save, strRedirectPageView);

                            strAlert = "Target Master " + Resources.ValidationMsgs.S3G_ValMsg_Save;
                            strAlert += @"\n" + Resources.ValidationMsgs.S3G_ValMsg_Next;
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                            // return;
                        }
                        else if (txtdbtcllctr.Text != null || txtdbtcllctr.Text != string.Empty)
                        {
                            Utility.FunShowAlertMsg(this.Page, "Target Master " + Resources.ValidationMsgs.S3G_ValMsg_Update, strRedirectModify);
                        }
                        break;
                    case 38:
                        Utility.FunShowAlertMsg(this.Page, " Target already exists for specified LOB,PERIOD and Marketing Executive type");
                        return;
                        break;
                    case 36:
                        if (txtdbtcllctr.Text == null || txtdbtcllctr.Text == string.Empty)
                        {
                            //Utility.FunShowAlertMsg(this.Page, "Target Master " + Resources.ValidationMsgs.S3G_ValMsg_Save, strRedirectPageView);
                            Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._1, strRedirectModify);
                            return;
                            break;
                        }
                        else if (txtdbtcllctr.Text != null || txtdbtcllctr.Text != string.Empty)
                        {
                            //Utility.FunShowAlertMsg(this.Page, "Target Master " + Resources.ValidationMsgs.S3G_ValMsg_Save, strRedirectModify);
                            Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._1, strRedirectModify);
                            return;
                            break;
                        }
                        break;
                    case 37:
                        if (txtdbtcllctr.Text == null || txtdbtcllctr.Text == string.Empty)
                        {
                            // Utility.FunShowAlertMsg(this.Page, "Target Master " + Resources.ValidationMsgs.S3G_ValMsg_Save, strRedirectPageView);
                            Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._2, strRedirectPageView);
                            return;
                            break;
                        }
                        else if (txtdbtcllctr.Text != null || txtdbtcllctr.Text != string.Empty)
                        {
                            //Utility.FunShowAlertMsg(this.Page, "Target Master " + Resources.ValidationMsgs.S3G_ValMsg_Save, strRedirectModify);
                            Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._2, strRedirectModify);
                            return;
                            break;
                        }
                        break;
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void FunPriLoadLOV()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            /********** LOAD LOB *************/
            //Procparam.Add("@OPTION", "1");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            if (strMode == "C")
                Procparam.Add("@Is_Active", "1");
            //Procparam.Add("@FilterOption", "'HP','LN','OL','FL'");
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            Procparam.Add("@Program_ID", "208");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            /********* LOAD ASSET CATEGORY ***********/
            Procparam.Clear();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            string asstyp = "Asset_Category";
            Procparam.Add("@Type", asstyp);
            ddlassetcateg.BindDataTable(SPNames.S3G_ORG_GetAssetCategory_List, Procparam, new string[] { "ID", "Name" });
            Procparam.Clear();


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FunPriClearControls()
    {
        try
        {

            txtMKTExeNm.Text = txtMobile.Text = txtemailaddr.Text = txtAddress.Text = txtMKTExecd.Text = txtPhone.Text = txtMobile.Text = string.Empty;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FunPriCleardropdown()
    {
        try
        {

            ddlassetcateg.ClearSelection();
            //ddlBranch.ClearSelection();
            ddlBranch.Clear();
            ddlfreqtyp.ClearSelection();
            ddlLOB.ClearSelection();
            //ddlMxecnm.ClearSelection();
            ddlMxecnm.Clear();
            ddlMxectyp.ClearSelection();
            ddlprod.ClearSelection();
            ViewState["Target_AssetDetails"] = null;
            targetasset = FunPriGetTargetAssetDataTable();
            if (targetasset.Rows.Count == 0)
            {
                FunPriInsertTargetAssetDataTable("-1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            gvTargetMasterDetails.EditIndex = -1;

            if (hidTRG.Value != string.Empty)
            {
                grvcpytrgt.DataSource = string.Empty;
                grvcpytrgt.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "208");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    private void FunPriLoadProd()
    {
        try
        {

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));

            Procparam.Add("@Is_Active", "1");

            ddlprod.BindDataTable(SPNames.S3G_ORG_GetLOBProductList, Procparam, new string[] { "Product_ID", "Product_code" });
            Procparam.Clear();


            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            //Procparam.Add("@User_Id", Convert.ToString(intUserId));
            //Procparam.Add("@Is_Active", "1");
            //Procparam.Add("@Program_ID", "208");
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //Procparam.Clear();


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetUserList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        
        if (obj_Page.ddlMxectyp.SelectedValue == "1")
        {
            Procparam.Add("@Prefix", prefixText);
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_Get_User_List, Procparam));
        }
        else
        {
            Procparam.Add("@PrefixText", prefixText);
            Procparam.Add("@Entity_Type", "2,4");
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));
        }
        return suggestions.ToArray();
    }

    private void FunPriLoadbusexec(int ptype)
    {
        try
        {

            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //Procparam.Add("@ptype", Convert.ToString(ptype));

            //ddlMxecnm.BindDataTable(SPNames.S3G_ORG_Getbusexecname, Procparam, new string[] { "User_ID", "User_name" });
            //Procparam.Clear();
            ddlMxecnm.Clear();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void Funpriloadbusdet()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            DataSet ds = new DataSet();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));

            if (ddlMxectyp.SelectedValue == "1")
            {
                Procparam.Add("@User_ID", Convert.ToString(intUserId));
                ds = Utility.GetDataset(SPNames.S3G_SYSAD_Get_User_Details, Procparam);
                if (ds.Tables[0].Rows.Count >= 1)
                {
                    txtMKTExeNm.ReadOnly = false;
                    txtMobile.ReadOnly = false;
                    txtemailaddr.ReadOnly = false;
                    txtMKTExeNm.Text = ds.Tables[0].Rows[0]["User_Name"].ToString();
                    txtMobile.Text = ds.Tables[0].Rows[0]["Mobile_No"].ToString();
                    txtemailaddr.Text = ds.Tables[0].Rows[0]["Email_ID"].ToString();
                    txtMKTExecd.Text = ddlMxecnm.SelectedValue.ToString();
                }
            }
            else if (ddlMxectyp.SelectedValue == "2")
            {
                Procparam.Add("@Entity_Id", Convert.ToString(ddlMxecnm.SelectedValue));
                Procparam.Add("@ErrorCode", Convert.ToString(intUserId));
                ds = Utility.GetDataset(SPNames.S3G_ORG_GetEntityDetails, Procparam);
                if (ds.Tables[0].Rows.Count >= 1)
                {
                    txtMKTExeNm.Text = ds.Tables[0].Rows[0]["Entity_Name"].ToString();
                    txtMobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    txtemailaddr.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                    txtPhone.Text = ds.Tables[0].Rows[0]["Telephone"].ToString();
                    txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + ds.Tables[0].Rows[0]["Address2"].ToString();
                    txtMKTExecd.Text = ds.Tables[0].Rows[0]["Entity_ID"].ToString();


                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void Funloadassetdet()
    {

        try
        {


            DropDownList ddlassetclass = gvTargetMasterDetails.FooterRow.FindControl("ddlassetclass") as DropDownList;
            ddlassetclass.Focus();
            ddlassetclass.ClearDropDownList();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@AssetType_ID", ddlassetcateg.SelectedValue);
            Procparam.Add("@Type", "class");
            Procparam.Add("@classid", string.Empty);
            DataTable ObjDataTable = Utility.GetDefaultData("S3G_Org_GetAssetClassMake_List", Procparam);
            Procparam.Clear();

            ViewState["ddlassetclass"] = ObjDataTable;

            ddlassetclass.BindDataTable(ObjDataTable, new string[] { "class_id", "class_desc" });


        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private void FunPubBindTargetAsset(DataTable targetasset)
    {
        try
        {
            gvTargetMasterDetails.DataSource = targetasset;
            gvTargetMasterDetails.DataBind();
            if (targetasset.Rows[0]["Asset_Serial_Number"].ToString().Equals("0"))
            {
                gvTargetMasterDetails.Rows[0].Visible = false;
            }
            gvTargetMasterDetails.Visible = true;
            FunPriSetFooterControl();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void ddlassetmk(string intclassid)
    {
        DropDownList ddlassetmake = (DropDownList)gvTargetMasterDetails.FooterRow.FindControl("ddlassetmake");
        DropDownList ddlassetclass = (DropDownList)gvTargetMasterDetails.FooterRow.FindControl("ddlassetclass");
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        Procparam.Add("@AssetType_ID", Convert.ToString(ddlassetcateg.SelectedValue));
        Procparam.Add("@Type", "Make");
        Procparam.Add("@classid", intclassid);
        DataTable ObjmakeTable = Utility.GetDefaultData("S3G_Org_GetAssetClassMake_List", Procparam);
        Procparam.Clear();
        ViewState["ddlassetmake"] = ObjmakeTable;
        ddlassetmake.BindDataTable(ObjmakeTable, new string[] { "make_id", "make_desc" });
    }
    protected void btnFreqChange_Click(object sender, EventArgs e)
    {
        ViewState["Target_AssetDetails"] = null;
        targetasset = FunPriGetTargetAssetDataTable();
        if (targetasset.Rows.Count == 0)
        {
            FunPriInsertTargetAssetDataTable("-1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
        gvTargetMasterDetails.EditIndex = -1;
        FunPriSetFooterPeriod();
        if (hidTRG.Value == "2")
            Funtrgadd();
    }
    private void FunPriSetFooterControl()
    {
        try
        {
            if (gvTargetMasterDetails.FooterRow != null)
            {
                Label lblPeriodFoot = (Label)gvTargetMasterDetails.FooterRow.FindControl("lblPeriodFoot");
                ((DropDownList)gvTargetMasterDetails.FooterRow.FindControl("ddlassetclass")).Style.Add("text-align", "right");
                ((TextBox)gvTargetMasterDetails.FooterRow.FindControl("txtTargetAmount")).Style.Add("text-align", "right");
                ((TextBox)gvTargetMasterDetails.FooterRow.FindControl("txtUnits")).Style.Add("text-align", "right");
                ((TextBox)gvTargetMasterDetails.FooterRow.FindControl("txtCommissionPercentage")).Style.Add("text-align", "right");
                ((TextBox)gvTargetMasterDetails.FooterRow.FindControl("txtSplCommissionPercentage")).Style.Add("text-align", "right");
            }
        }
        catch (Exception ex)
        {
            //   ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriBindGrid(DataTable dt)
    {
        try
        {
            //DataTable dt = (DataTable)ViewState["Target_AssetDetails"];
            gvTargetMasterDetails.DataSource = dt;
            gvTargetMasterDetails.DataBind();
            FunPriSetFooterControl();
        }
        catch (Exception ex)
        {
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    #endregion

    #region Calculate Frequency
    protected string FunPriCalcFrequency(int i, string strPeriod, string strFinMonth)
    {
        int count = 0;
        while (count < i)
        {
            if (Convert.ToInt32(strFinMonth) < 12)
            {
                strPeriod = (Convert.ToInt32(strPeriod) + 1).ToString();
                strFinMonth = (Convert.ToInt32(strFinMonth) + 1).ToString();
                //strPeriod = strFinYear + strFinMonth;
            }
            else
            {
                strFinMonth = "01";
                strFinYear = (Convert.ToInt32(strFinYear) + 1).ToString();
                strPeriod = strFinYear + strFinMonth;
            }
            count++;
        }
        return strPeriod;
    }
    private string Funsetsuffix()
    {
        int suffix = 1;
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }
    protected void FunPriSetFooterPeriod()
    {
        try
        {
            hdnFT.Value = ddlfreqtyp.SelectedValue;
            intFinMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
            if (intFinMonth == 1)
                intFinMonForCalc = 12;
            else
                intFinMonForCalc = intFinMonth - 1;
            if (DateTime.Now.Month > intFinMonForCalc)
            {
                intMaxMonth = 0;
            }
            else
            {
                intMaxMonth = -1;
            }
            strFinYear = DateTime.Now.AddYears(intMaxMonth).Year.ToString() + "-" + DateTime.Now.AddYears(intMaxMonth + 1).Year.ToString();
            mon = DateTime.Now.Month.ToString("00");
            int yr;
            yr = DateTime.Now.Year;


            if (yr == Convert.ToInt32(strFinYear.Substring(0, 4)))
                strPeriod = strFinYear.Substring(0, 4) + mon;
            else if (yr == Convert.ToInt32(strFinYear.Substring(5, 4)))
                strPeriod = strFinYear.Substring(5, 4) + mon;
            //********//

            //Commented for UAT Observations//
            //strPeriod = strFinYear.Substring(0, 4) + mon;
            //**********//

            Label lblPeriodFoot = (Label)gvTargetMasterDetails.FooterRow.FindControl("lblPeriodFoot");

            //For Monthly
            if (Convert.ToInt32(ddlfreqtyp.SelectedValue) == 1)
            {
                lblPeriodFoot.Text = FunPriCalc_InitPeriod(12, 1);
            }
            //For Quarterly
            if (Convert.ToInt32(ddlfreqtyp.SelectedValue) == 2)
            {
                lblPeriodFoot.Text = FunPriCalc_InitPeriod(4, 3);
            }
            //For HalfYearly
            if (Convert.ToInt32(ddlfreqtyp.SelectedValue) == 3)
            {
                lblPeriodFoot.Text = FunPriCalc_InitPeriod(2, 6);
            }
            //For Annually
            if (Convert.ToInt32(ddlfreqtyp.SelectedValue) == 4)
            {
                strPeriod = (Convert.ToInt32(strFinYear.Substring(0, 4)) + 1).ToString();
                lblPeriodFoot.Text = strPeriod;
            }


        }

        catch (Exception ex)
        {

        }
    }
    protected string FunPriSetFooterPeriodWithRows(string strPeriod)
    {
        try
        {
            if (strPeriod != "")
            {
                strFinYear = strPeriod.Substring(0, 4);
                if (strPeriod.Length > 4)
                    strFinMonth = strPeriod.Substring(4, 2);

                //For Monthly
                if (Convert.ToInt32(ddlfreqtyp.SelectedValue) == 1)
                {
                    strPeriod = FunPriCalcFrequency(1, strPeriod, strFinMonth);
                }

                //For Quarterly
                if (Convert.ToInt32(ddlfreqtyp.SelectedValue) == 2)
                {
                    strPeriod = FunPriCalcFrequency(3, strPeriod, strFinMonth);
                }

                //For HalfYearly
                if (Convert.ToInt32(ddlfreqtyp.SelectedValue) == 3)
                {
                    strPeriod = FunPriCalcFrequency(6, strPeriod, strFinMonth);
                }

                //For Annually
                if (Convert.ToInt32(ddlfreqtyp.SelectedValue) == 4)
                {
                    strPeriod = (Convert.ToInt32(strPeriod.Substring(0, 4)) + 1).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            // cvDebtCollectorDetails.ErrorMessage = "Error: Unable to Set Period";
            // cvDebtCollectorDetails.IsValid = false;
        }
        return strPeriod;
    }
    private string FunPriCalc_InitPeriod(int intLoop, int intFreq)
    {
        try
        {
            string strQuarterly = strFinYear.Substring(0, 4) + intFinMonForCalc.ToString("00");
            for (int i = 0; i < intLoop; i++)
            {
                int count = 0;
                while (count < intFreq)
                {
                    if (intFinMonForCalc < 12)
                    {
                        strQuarterly = (Convert.ToInt32(strQuarterly) + 1).ToString();
                        intFinMonth = Convert.ToInt32(strQuarterly.Substring(4, 2));
                        intFinMonForCalc = intFinMonth;
                    }
                    else
                    {
                        strQuarterly = (Convert.ToInt32(strQuarterly.Substring(0, 4)) + 1).ToString() + "01";
                        intFinMonth = Convert.ToInt32(strQuarterly.Substring(4, 2));
                        intFinMonForCalc = intFinMonth;
                    }
                    count++;
                }
                arrList.Add(strQuarterly);
            }
            int j = 0;
            while (j < arrList.Count)
            {
                if (Convert.ToInt32(arrList[j]) < Convert.ToInt32(strPeriod))
                {
                    arrList.Remove(arrList[j]);
                    j = 0;
                }
                else
                {
                    j++;
                }
            }
            strPeriod = arrList[0].ToString();
        }
        catch (Exception ex)
        {
            //cvDebtCollectorDetails.ErrorMessage = ex.Message;
            // cvDebtCollectorDetails.IsValid = false;
        }
        return strPeriod;
    }
    #endregion

    #region Dropdownlist Events
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadProd();
            FunPriClearControls();
            ddlfreqtyp.ClearSelection();
            ddlMxectyp.ClearSelection();
            //ddlMxecnm.ClearSelection();
            ddlMxecnm.Clear();
        }
        catch (Exception objException)
        {

        }

    }
    protected void ddlMxectyp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            if (ddlMxectyp.SelectedValue == "1")
            {
                ptype = 2;
            }
            else if (ddlMxectyp.SelectedValue == "2")
            {
                ptype = 1;
            }

            FunPriLoadbusexec(ptype);
        }
        catch (Exception objException)
        {
        }
    }
    protected void ddlMxecnm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            Funpriloadbusdet();
        }
        catch (Exception objException)
        {
        }

    }

    protected void ddlassetcateg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Funloadassetdet();
            ddlassetcateg.Attributes.Add("onChange", "return fnClearGrid();");
            if ((hidTRG.Value) == "2")
            {
                ViewState["Target_AssetDetails"] = null;
                targetasset = FunPriGetTargetAssetDataTable();
                if (targetasset.Rows.Count == 0)
                {
                    FunPriInsertTargetAssetDataTable("-1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                }
                Funtrgadd();
            }
        }
        catch (Exception objException)
        {
        }

    }

    protected void ddlfreqtyp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlfreqtyp.Attributes.Add("onChange", "return fnClearGrid();");
            FunPriSetFooterPeriod();
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlcpytrgMxectyp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            DataSet ds = new DataSet();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));

            if (ddlcpytrgMxectyp.SelectedValue == "1")
            {
                if (FunPubGetDatabaseType() == S3GDALDBType.SQL)
                    Procparam.Add("@User_ID", string.Empty);
                else
                    Procparam.Add("@User_ID", "0");

                ds = Utility.GetDataset(SPNames.S3G_SYSAD_Get_User_Details, Procparam);
                //if (ds.Tables[0].Rows.Count >= 1)
                //{

                //    txtMKTExeNm.Text = ds.Tables[0].Rows[0]["User_Name"].ToString();
                //    txtMKTExecd.Text = ds.Tables[0].Rows[0]["User_Id"].ToString();

                //}
                hidTRG.Value = "1";

            }
            else if (ddlcpytrgMxectyp.SelectedValue == "2")
            {
                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                int ptype = 1;
                Procparam.Add("@ptype", Convert.ToString(ptype));
                ds = Utility.GetDataset(SPNames.S3G_ORG_Getbusexecname, Procparam);
                //if (ds.Tables[0].Rows.Count >= 1)
                //{
                //    txtMKTExeNm.Text = ds.Tables[0].Rows[0]["User_Name"].ToString();
                //    txtMKTExecd.Text = ds.Tables[0].Rows[0]["User_Id"].ToString();

                //}
                hidTRG.Value = "1";

            }

            // gvTargetMasterDetails.DataSource = dt;
            //gvTargetMasterDetails.DataBind();
            // FunPriSetFooterControl();
            grvcpytrgt.DataSource = ds;
            grvcpytrgt.DataBind();


        }
        catch (Exception ex)
        {
        }

    }

    protected void ddlassetclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlassetclass;

        if (gvTargetMasterDetails.FooterRow.Visible)
        {
            ddlassetclass = (DropDownList)gvTargetMasterDetails.FooterRow.FindControl("ddlassetclass");
            ddlassetmk(Convert.ToString(ddlassetclass.SelectedItem.Text));
        }
        else
        {
            DropDownList ddlassetmake;
            ddlassetclass = (DropDownList)sender;
            GridViewRow gvRow = (GridViewRow)ddlassetclass.Parent.Parent;
            ddlassetmake = (DropDownList)gvTargetMasterDetails.Rows[gvRow.RowIndex].FindControl("ddlassetmake");
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Is_Active", "1");
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //Procparam.Add("@Module_Code", Convert.ToString(ddlModuleCode_Grd.SelectedValue));
            //ddlassetmake.BindDataTable(SPNames.SYS_ProgramMaster, Procparam, new string[] { "Program_ID", "Program_Name" });
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@AssetType_ID", Convert.ToString(ddlassetcateg.SelectedValue));
            Procparam.Add("@Type", "Make");
            Procparam.Add("@classid", Convert.ToString(ddlassetclass.SelectedItem.Text));
            DataTable ObjmakeTable = Utility.GetDefaultData("S3G_Org_GetAssetClassMake_List", Procparam);
            Procparam.Clear();
            ViewState["ddlassetmake"] = ObjmakeTable;
            ddlassetmake.BindDataTable(ObjmakeTable, new string[] { "make_id", "make_desc" });
        }
    }

    #endregion

    #region Checkbox Events
    protected void chkcpytrg_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcpytrg.Checked)
        {
            Pnlcpytrgt.Visible = true;
            FunPriClearControls();
            ddlfreqtyp.ClearSelection();
            ddlMxectyp.ClearSelection();
            //ddlMxecnm.ClearSelection();
            ddlMxecnm.Clear();
            ddlLOB.ClearSelection();
            //ddlBranch.ClearSelection();
            ddlBranch.Clear();
            hidTRG.Value = "2";
        }
        else
        {
            Pnlcpytrgt.Visible = false;
            hidTRG.Value = string.Empty;
        }
        ddlcpytrgMxectyp.ClearSelection();


    }
    #endregion


    #region Gridview Events

    protected void gvTargetMasterDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            gvTargetMasterDetails.Columns[8].Visible = true;
            gvTargetMasterDetails.ShowFooter = true;
            if (strMode == "Q")
            {
                gvTargetMasterDetails.Columns[8].Visible = false;
                gvTargetMasterDetails.ShowFooter = false;

            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[0].Style.Add("position", "relative");
                //e.Row.Cells[0].BorderWidth = 0;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblPeriodFoot = e.Row.FindControl("lblPeriodFoot") as Label;
                lblPeriodFoot.Text = strPeriod;
                TextBox txtTargetAmount = (TextBox)e.Row.FindControl("txtTargetAmount");
                txtTargetAmount.SetDecimalPrefixSuffix(10, 4, true, "Target Amount");
                txtTargetAmount.Focus();
                DropDownList ddlassetclass = e.Row.FindControl("ddlassetclass") as DropDownList;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                Procparam.Add("@AssetType_ID", ddlassetcateg.SelectedValue.ToString());
                Procparam.Add("@Type", "class");
                Procparam.Add("@classid", string.Empty);
                DataTable ObjDataTable = Utility.GetDefaultData("S3G_Org_GetAssetClassMake_List", Procparam);
                Procparam.Clear();

                ViewState["ddlassetclass"] = ObjDataTable;

                ddlassetclass.BindDataTable(ObjDataTable, new string[] { "class_id", "class_desc" });

                DropDownList ddlassetmake = e.Row.FindControl("ddlassetmake") as DropDownList;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                Procparam.Add("@AssetType_ID", Convert.ToString(ddlassetcateg.SelectedValue));
                Procparam.Add("@Type", "Make");
                Procparam.Add("@classid", Convert.ToString(ddlassetclass.SelectedItem.Text));
                DataTable ObjmakeTable = Utility.GetDefaultData("S3G_Org_GetAssetClassMake_List", Procparam);
                Procparam.Clear();
                ViewState["ddlassetmake"] = ObjmakeTable;
                ddlassetmake.BindDataTable(ObjmakeTable, new string[] { "make_id", "make_desc" });


            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkRemove = e.Row.FindControl("lnkRemove") as LinkButton;
                LinkButton lnkEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                Label lblPeriod = e.Row.FindControl("lblPeriod") as Label;
                DropDownList ddlassetclass = e.Row.FindControl("ddlassetclass") as DropDownList;
                DropDownList ddlassetmake = e.Row.FindControl("ddlassetmake") as DropDownList;

                Label lblTargetAmount = e.Row.FindControl("lblTargetAmount") as Label;
                Label lblCommissionPercentage = e.Row.FindControl("lblCommissionPercentage") as Label;
                Label lblSplCommissionPercentage = e.Row.FindControl("lblSplCommissionPercentage") as Label;

                //decimal d1=0;
                if (lblTargetAmount != null)
                {
                    if (!string.IsNullOrEmpty(lblTargetAmount.Text))
                        lblTargetAmount.Text = Convert.ToDecimal(lblTargetAmount.Text).ToString(Funsetsuffix());
                }
                if (lblSplCommissionPercentage != null)
                {
                    if (lblSplCommissionPercentage.Text == "0")
                        lblSplCommissionPercentage.Text = string.Empty;
                }
                strPeriod = lblPeriod.Text;

                if (ViewState["Target_AssetDetails"] != null)
                {
                    if (e.Row.RowIndex != ((DataTable)ViewState["Target_AssetDetails"]).Rows.Count - 1)
                    {
                        //LinkButton btnRemove = (LinkButton)e.Row.FindControl("btnRemove");
                        lnkRemove.Enabled = false;
                        lnkRemove.OnClientClick = string.Empty;
                        // lnkRemove.Attributes.Remove("OnClientClick");
                    }
                }
                if (strPeriod != string.Empty)
                {
                    mon = DateTime.Now.Month.ToString("00");
                    string s = DateTime.Now.Year.ToString("0000");
                    if (strPeriod.Length > 4)
                    {
                        s = s + DateTime.Now.Month.ToString("00");
                        if (Convert.ToInt32(strPeriod) < Convert.ToInt32(s))
                        {

                            if (lnkRemove != null)
                            {
                                lnkRemove.Enabled = false;
                                lnkRemove.OnClientClick = string.Empty;
                                //lnkRemove.Attributes.Remove("OnClientClick");
                            }
                            if (lnkEdit != null)
                                lnkEdit.Enabled = false;
                        }
                    }
                    else
                    {

                        if (Convert.ToInt32(strPeriod) < Convert.ToInt32(s))
                        {
                            if (lnkRemove != null)
                            {
                                lnkRemove.Enabled = false;
                                lnkRemove.OnClientClick = string.Empty;
                                //lnkRemove.Attributes.Remove("OnClientClick");
                            }
                            if (lnkEdit != null)
                                lnkEdit.Enabled = false;
                        }

                    }
                }


            }


            if (strMode == "C" && hidTRG.Value == "2")
            {
                gvTargetMasterDetails.Columns[8].Visible = false;
                //gvTargetMasterDetails.ShowFooter = false;
            }


        }

        catch (Exception ex)
        {
        }
    }
    protected void gvTargetMasterDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string strTargetAmount, strCommissionPercentage, strSplCommisionPercentage, strAssetclass, strAssetmake, strUnits;

            if (e.CommandName == "Add")
            {
                if (ddlfreqtyp.SelectedValue == "--Select--")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select Frequency Type");
                    ddlfreqtyp.Focus();
                    return;
                }

                TextBox txtTargetAmount = gvTargetMasterDetails.FooterRow.FindControl("txtTargetAmount") as TextBox;
                TextBox txtUnits = gvTargetMasterDetails.FooterRow.FindControl("txtUnits") as TextBox;
                TextBox txtCommissionPercentage = gvTargetMasterDetails.FooterRow.FindControl("txtCommissionPercentage") as TextBox;
                TextBox txtSplCommissionPercentage = gvTargetMasterDetails.FooterRow.FindControl("txtSplCommissionPercentage") as TextBox;
                Label lblPeriodFoot = (Label)gvTargetMasterDetails.FooterRow.FindControl("lblPeriodFoot");
                DropDownList ddlassetclass = gvTargetMasterDetails.FooterRow.FindControl("ddlassetclass") as DropDownList;
                DropDownList ddlassetmake = gvTargetMasterDetails.FooterRow.FindControl("ddlassetmake") as DropDownList;

                strPeriod = lblPeriodFoot.Text.Trim();
                strUnits = txtUnits.Text.Trim();
                strAssetclass = ddlassetclass.SelectedItem.Text.Trim();
                strAssetmake = ddlassetmake.SelectedItem.Text.Trim();
                strAssetclassvalue = ddlassetclass.SelectedValue;
                strAssetmakevalue = ddlassetmake.SelectedValue;


                if (lblPeriodFoot.Text.Trim().Equals(string.Empty))
                {
                    Utility.FunShowAlertMsg(this.Page, "Select Frequency Type to set period");
                    return;
                }
                if (txtTargetAmount.Text == string.Empty)
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter Target Amount");
                    txtTargetAmount.Focus();
                    return;
                }
                // Developer Name : Anbuvel.T
                // Modified Date : 08-Nov-2012
                // Description : Code Commented based on Kali Suggestion/SRS

                //if (Convert.ToInt32(ddlassetclass.SelectedValue) <= 0)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Select Asset Class");
                //    ddlassetclass.Focus();
                //    return;
                //}
                //if (Convert.ToInt32(ddlassetmake.SelectedValue) <= 0)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Select Asset Make");
                //    ddlassetmake.Focus();
                //    return;
                //}
                if (txtCommissionPercentage.Text == string.Empty)
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter Commission %");
                    txtCommissionPercentage.Focus();
                    return;
                }
                strTargetAmount = txtTargetAmount.Text.Trim();
                strCommissionPercentage = txtCommissionPercentage.Text.Trim();

                if (txtSplCommissionPercentage.Text == string.Empty)
                    strSplCommisionPercentage = "0";
                else
                    strSplCommisionPercentage = txtSplCommissionPercentage.Text.Trim();
                FunPriInsertTargetAssetDataTable(strAssetclass, strAssetmake, strPeriod, strTargetAmount, strUnits, strCommissionPercentage, strSplCommisionPercentage, strAssetclassvalue, strAssetmakevalue);

            }

        }
        catch (Exception ex)
        {
            if (e.CommandName == "Add")
                cvTargetMaster.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_AddGrid + this.Page.Header.Title;
            cvTargetMaster.IsValid = false;
        }
    }
    protected void gvTargetMasterDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["Target_AssetDetails"];
            gvTargetMasterDetails.EditIndex = -1;
            //  gvDebtCollectorDetails.EditIndex = e.NewEditIndex;
            FunPriBindGrid(dt);
            gvTargetMasterDetails.FooterRow.Visible = true;

            targetasset = FunPriGetTargetAssetDataTable();
            targetasset.Rows.RemoveAt(e.RowIndex);
            if (targetasset.Rows.Count == 0)


                ViewState["Target_AssetDetails"] = targetasset;
            targetasset = FunPriGetTargetAssetDataTable();
            if (targetasset.Rows.Count == 0)
            {
                FunPriInsertTargetAssetDataTable("-1", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                FunPriSetFooterPeriod();
            }
            else
            {
                FunPubBindTargetAsset(targetasset);
                strPeriod = FunPriSetFooterPeriodWithRows(strPeriod);
            }
        }
        catch (Exception ex)
        {
            cvTargetMaster.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_DeleteGrid + this.Page.Header.Title;
            cvTargetMaster.IsValid = false;
        }
    }

    protected void gvTargetMasterDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvTargetMasterDetails.EditIndex = e.NewEditIndex;
            DataTable dt = (DataTable)ViewState["Target_AssetDetails"];
            DataRow drEditRow = dt.Rows[e.NewEditIndex];
            FunPriBindGrid(dt);

            ((TextBox)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("txtTargetAmount")).Style.Add("text-align", "right");
            ((TextBox)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("txtUnits")).Style.Add("text-align", "right");
            ((TextBox)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("txtCommissionPercentage")).Style.Add("text-align", "right");
            ((TextBox)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("txtSplCommissionPercentage")).Style.Add("text-align", "right");

            //DropDownList ddlassetclass = gvTargetMasterDetails.FooterRow.FindControl("ddlassetclass") as DropDownList;
            //DropDownList ddlassetclass = (DropDownList)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("ddlassetclass");
            //DropDownList ddlassetmake = gvTargetMasterDetails.FooterRow.FindControl("ddlassetmake") as DropDownList;

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@AssetType_ID", ddlassetcateg.SelectedValue.ToString());
            Procparam.Add("@Type", "class");
            Procparam.Add("@classid", string.Empty);
            DataTable ObjDataTable = Utility.GetDefaultData("S3G_Org_GetAssetClassMake_List", Procparam);
            Procparam.Clear();


            ViewState["ddlassetclass"] = ObjDataTable;


            DropDownList ddlassetclass = (DropDownList)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("ddlassetclass");
            //ddlassetclass.Focus();

            ddlassetclass.BindDataTable(ObjDataTable, new string[] { "class_id", "class_desc" });

            //if (ViewState["ddlAssetClass"] == null)
            //{
            //Funloadassetdet();

            //}
            //else
            //{
            //    ddlassetclass.BindDataTable((DataTable)ViewState["ddlAssetClass"], new string[] { "Asset_Category_ID", "Category_Code", "Category_Description" });
            //}
            //ddlassetclass.ClearSelection();
            //ponnurajesh////ddlassetclass.SelectedItem.Text = drEditRow["Asset_Class"].ToString();
            ddlassetclass.SelectedValue = drEditRow["Asset_Classvalue"].ToString();


            //((DropDownList )gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("ddlAssetmake")).Style.Add("text-align", "right");
            //((DropDownList)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("ddlAssetclass")).Style.Add("text-align", "right");


            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@AssetType_ID", Convert.ToString(ddlassetcateg.SelectedValue));
            Procparam.Add("@Type", "Make");
            Procparam.Add("@classid", Convert.ToString(ddlassetclass.SelectedItem.Text));
            DataTable ObjmakeTable = Utility.GetDefaultData("S3G_Org_GetAssetClassMake_List", Procparam);
            Procparam.Clear();
            ViewState["ddlassetmake"] = ObjmakeTable;
            DropDownList ddlassetmake = (DropDownList)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("ddlassetmake");
            ddlassetmake.BindDataTable(ObjmakeTable, new string[] { "make_id", "make_desc" });
            //ddlassetmake.SelectedItem.Text = drEditRow["Asset_Make"].ToString();
            ddlassetmake.SelectedValue = drEditRow["Asset_Makevalue"].ToString();


            TextBox txtTargetAmount = (TextBox)gvTargetMasterDetails.Rows[e.NewEditIndex].FindControl("txtTargetAmount");
            txtTargetAmount.SetDecimalPrefixSuffix(10, 4, true, "Target Amount");
            txtTargetAmount.Text = (Convert.ToDecimal(txtTargetAmount.Text).ToString(Funsetsuffix()));

            txtTargetAmount.Focus();
            gvTargetMasterDetails.FooterRow.Visible = false;
        }
        catch (Exception ex)
        {
            cvTargetMaster.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_EditGrid + this.Page.Header.Title;
            cvTargetMaster.IsValid = false;
        }
    }

    protected void gvTargetMasterDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {

            DataTable dt = (DataTable)ViewState["Target_AssetDetails"];
            Label lblSerialNo = (Label)gvTargetMasterDetails.Rows[e.RowIndex].FindControl("lblSerialNo");
            DropDownList ddlassetclass = (DropDownList)gvTargetMasterDetails.Rows[e.RowIndex].FindControl("ddlAssetclass");
            DropDownList ddlassetmake = (DropDownList)gvTargetMasterDetails.Rows[e.RowIndex].FindControl("ddlAssetmake");
            Label lblPeriod = (Label)gvTargetMasterDetails.Rows[e.RowIndex].FindControl("lblPeriod");
            TextBox txtTargetAmount = (TextBox)gvTargetMasterDetails.Rows[e.RowIndex].FindControl("txtTargetAmount");
            TextBox txtUnits = (TextBox)gvTargetMasterDetails.Rows[e.RowIndex].FindControl("txtUnits");
            TextBox txtCommissionPercentage = (TextBox)gvTargetMasterDetails.Rows[e.RowIndex].FindControl("txtCommissionPercentage");
            TextBox txtSplCommissionPercentage = (TextBox)gvTargetMasterDetails.Rows[e.RowIndex].FindControl("txtSplCommissionPercentage");

            string strTargetAmount, strCommissionPercentage, strSplCommisionPercentage, strUnits, strAssetclass, strAssetmake, strAssetclassvalue, strAssetmakevalue;
            if (txtTargetAmount.Text == string.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, "Enter Target Amount");
                return;
            }
            if (txtCommissionPercentage.Text == string.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, "Enter Commission %");
                return;
            }
            strTargetAmount = txtTargetAmount.Text.Trim();
            strCommissionPercentage = txtCommissionPercentage.Text.Trim();
            strUnits = txtUnits.Text.Trim();
            strAssetclass = ddlassetclass.SelectedItem.Text.Trim();
            strAssetmake = ddlassetmake.SelectedItem.Text.Trim();
            strAssetclassvalue = ddlassetclass.SelectedValue;
            strAssetmakevalue = ddlassetmake.SelectedValue;
            if (txtSplCommissionPercentage.Text == string.Empty)
                strSplCommisionPercentage = "0";
            else
                strSplCommisionPercentage = txtSplCommissionPercentage.Text.Trim();

            FunPriUpdateDataTable(lblSerialNo.Text, strAssetclass, strAssetmake, lblPeriod.Text, strTargetAmount, strUnits, strCommissionPercentage, strSplCommisionPercentage, strAssetclassvalue, strAssetmakevalue);
            gvTargetMasterDetails.EditIndex = -1;
            FunPriBindGrid(dt);
            gvTargetMasterDetails.FooterRow.Visible = true;

        }
        catch (Exception ex)
        {
            cvTargetMaster.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_UpdateGrid + this.Page.Header.Title;
            cvTargetMaster.IsValid = false;
        }
    }

    protected void gvTargetMasterDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["Target_AssetDetails"];
            gvTargetMasterDetails.EditIndex = -1;
            FunPriBindGrid(dt);
            gvTargetMasterDetails.FooterRow.Visible = true;

        }
        catch (Exception ex)
        {
            cvTargetMaster.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_CancelEditGrid + this.Page.Header.Title;
            cvTargetMaster.IsValid = false;
        }
    }

    protected void grvcpytrgt_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            CheckBox chkSelRoleCode = (CheckBox)e.Row.FindControl("chkSelRoleCode");
            chkSelRoleCode.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvcpytrgt.ClientID + "','chkAll','chkSelRoleCode');");
            //if (lblAssigned.Text == "1")
            //{
            //    if (IsLOBCalled == false)
            //    {
            //        if ((intConstitutionID > 0) && (strMode == "M"))
            //        {
            //            IsLOBCalled = true;
            //            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //            Procparam.Add("@Constitution_ID", intConstitutionID.ToString());
            //            DataTable dt_IsExists = Utility.GetDefaultData("S3G_ORG_InsertConstitutionTransactionIsExits", Procparam);
            //            if (dt_IsExists != null && dt_IsExists.Rows.Count > 0)
            //                IsEnableLOB = false;
            //        }
            //    }

            ////if (!(IsEnableLOB))
            //    ////    chkSelectLOB.Enabled = false;
            //    //chkSelRoleCode.Checked = true;
            //}
            //if (strMode == "Q")
            //{
            //   chkSelRoleCode.Enabled = false;
            //}
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {

            CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
            chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvcpytrgt.ClientID + "',this,'chkSelRoleCode');");
            //chkAll.Checked = true;
            if (ViewState["SelectAll"] != null)
            {
                bool SelectAll = bool.Parse(ViewState["SelectAll"].ToString());
                chkAll.Checked = SelectAll;
            }
            if (strMode == "Q")
            {
                chkAll.Enabled = false;
            }
        }


    }

    #endregion


    #region Edit/Insert Datatable

    private DataTable FunPriGetTargetAssetDataTable()
    {
        try
        {

            if (ViewState["Target_AssetDetails"] == null)
            {
                targetasset = new DataTable();

                targetasset.Columns.Add("Asset_Serial_Number");
                targetasset.Columns.Add("Asset_Class");
                targetasset.Columns.Add("Asset_Make");
                targetasset.Columns.Add("Period");
                targetasset.Columns.Add("Target_Amount");
                targetasset.Columns.Add("Units");
                targetasset.Columns.Add("Commission");
                targetasset.Columns.Add("Special_Commission");
                targetasset.Columns.Add("Asset_Classvalue");
                targetasset.Columns.Add("Asset_Makevalue");

                ViewState["Target_AssetDetails"] = targetasset;
            }

            targetasset = (DataTable)ViewState["Target_AssetDetails"];

            return targetasset;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriInsertTargetAssetDataTable(string strAssetClass, string strAssetMake, string strPeriod, string strTarget_Amount, string strUnits, string strCommission, string strSplCommission, string strAssetClassvalue, string strAssetMakevalue)
    {
        try
        {

            DataRow drEmptyRow;
            targetasset = FunPriGetTargetAssetDataTable();

            if (strAssetClass.Equals("-1"))
            {
                if (targetasset.Rows.Count == 0)
                {
                    drEmptyRow = targetasset.NewRow();
                    drEmptyRow["Asset_Serial_Number"] = "0";
                    targetasset.Rows.Add(drEmptyRow);
                }
            }
            else
            {
                drEmptyRow = targetasset.NewRow();
                drEmptyRow["Asset_Serial_Number"] = Convert.ToInt32(targetasset.Rows[targetasset.Rows.Count - 1]["Asset_Serial_Number"]) + 1;
                drEmptyRow["Asset_Class"] = strAssetClass;
                drEmptyRow["Asset_Make"] = strAssetMake;
                drEmptyRow["Period"] = strPeriod;
                drEmptyRow["Target_Amount"] = strTarget_Amount;
                drEmptyRow["Units"] = strUnits;
                drEmptyRow["Commission"] = strCommission;
                drEmptyRow["Special_Commission"] = strSplCommission;
                drEmptyRow["Asset_Classvalue"] = strAssetClassvalue;
                drEmptyRow["Asset_Makevalue"] = strAssetMakevalue;
                targetasset.Rows.Add(drEmptyRow);
            }

            if (targetasset.Rows.Count > 1)
            {
                if (targetasset.Rows[0]["Asset_Serial_Number"].Equals("0"))
                {
                    targetasset.Rows[0].Delete();
                    //gvWorkFlowSequence.Rows[0].Visible = false;
                }
            }

            ViewState["Target_AssetDetails"] = targetasset;

            targetasset = FunPriGetTargetAssetDataTable();
            FunPubBindTargetAsset(targetasset);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriUpdateDataTable(string strSeriel_Num, string strAssetClass, string strAssetMake, string strPeriod, string strTotalAmt, string strUnits, string strCommision, string strsplCommision, string strAssetClassvalue, string strAssetMakevalue)   // update view state 
    {
        try
        {
            DataTable targetasset = (DataTable)ViewState["Target_AssetDetails"];
            if (Convert.ToString(targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Asset_Serial_Number"]) == strSeriel_Num)
            {
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Asset_Class"] = strAssetClass;
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Asset_Make"] = strAssetMake;
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Period"] = strPeriod;
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Target_Amount"] = strTotalAmt;
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Units"] = strUnits;
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Commission"] = strCommision;
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Special_Commission"] = strsplCommision;
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Asset_Classvalue"] = strAssetClassvalue;
                targetasset.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Asset_Makevalue"] = strAssetMakevalue;
                targetasset.AcceptChanges();
                ViewState["Target_AssetDetails"] = targetasset;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion


}
#endregion