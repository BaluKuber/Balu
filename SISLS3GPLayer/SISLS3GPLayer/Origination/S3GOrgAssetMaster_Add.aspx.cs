#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Asset Master
/// Created By			: Nataraj Y
/// Created Date		: 29-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 31-May-2010
/// Reason              : Asset Master
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 01-June-2010
/// Reason              : Asset Master
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 21-June-2010
/// Reason              : Asset Master 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 02-Jul-2010
/// Reason              : Table change and grid implementation
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 23-Jul-2010
/// Reason              : Review Comments implementation

/// Last Updated By	    : Thangam M, Manikandan R, Saran M
/// Last Updated Date   : 10-09-2010
/// Reason              : Bug fixing for the follwing Test cases: 
///                         AH-010,AH-005,TC-140,TC-064,TC-075
///                         
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 11-Nov-2010
/// Reason              : Bug Fixing
/// 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 01-Dec-2010
/// Reason              : Bug Fixing Round 3
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using S3GBusEntity;
using System.Globalization;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Configuration;
using System.Text.RegularExpressions;
#endregion

public partial class Origination_S3G_OrgAssetMaster_Add : ApplyThemeForProject
{
    #region Initialization
    int intCompanyId = 0;
    int intUserId = 0;
    int intAssetId = 0;
    int intAsset_CatId = 0;
    int strCategoryType = 0;
    int intErrorCode = 0;
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryDataTable ObjS3G_ORG_AssetCategoryDataTable;
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetMasterDataTable ObjS3G_ORG_AssetMasterDataTable;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgAssetMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgAssetMaster_Add.aspx';";
    string strRedirectPage = "~/Origination/S3GOrgAssetMaster_View.aspx";
    UserInfo ObjUserInfo;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    //Code end
    public static Origination_S3G_OrgAssetMaster_Add obj_Page;
    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            //Code end
            obj_Page = this;
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];

            if (Request.QueryString["qsAssetCatId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsAssetCatId"));
                intAsset_CatId = Convert.ToInt32(fromTicket.Name);
            }
            if (Request.QueryString["qsType"] != null)
                strCategoryType = Convert.ToInt32(Request.QueryString["qsType"]);

            if (!IsPostBack)
            {
                FunProIntializeData();

                if (intAsset_CatId > 0)
                {
                    FunPubLoadAssetCategories(intCompanyId, intAsset_CatId, strCategoryType);
                    if (strMode == "M")
                    {
                        if (strCategoryType == 2)
                            rfvAssetCategory1.ErrorMessage = "Select the Asset Category";
                        else if (strCategoryType == 3)
                            rfvAssetCategory1.ErrorMessage = "Select the Asset Type";
                        FunPriAssetControlStatus(1);
                    }
                    if (strMode == "Q")
                    {
                        FunPriAssetControlStatus(-1);
                    }
                    FunPriGetLabelName(strCategoryType - 1);
                }
                else
                {
                    FunPriAssetControlStatus(0);

                    if (strCategoryType == 1)
                        txtCode.Text = "10";
                    else if (strCategoryType == 2)
                        txtCode.Text = "100";
                    else
                        txtCode.Text = "10000";

                    FunPriGetLastGenCode(strCategoryType - 1);

                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region Page Events

    protected void chkCategoryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!tcCode.Enabled)
            {
                return;
            }

            int intType = tcCode.ActiveTabIndex + 1;

            DataTable dtCommon = new DataTable();

            if (intType == 1)
            {
                dtCommon = (DataTable)ViewState["CategoryTable"];

                if (dtCommon.Rows.Count == 0)
                    txtCode.Text = "10";
                else
                    txtCode.Text = dtCommon.Rows[dtCommon.Rows.Count - 1]["Code"].ToString();
            }
            else if (intType == 2)
            {
                dtCommon = (DataTable)ViewState["TypeTable"];

                if (dtCommon.Rows.Count == 0)
                    txtCode.Text = "100";
                else
                    txtCode.Text = dtCommon.Rows[dtCommon.Rows.Count - 1]["Code"].ToString();
            }
            else
            {
                dtCommon = (DataTable)ViewState["SubTypeTable"];

                if (dtCommon.Rows.Count == 0)
                    txtCode.Text = "10000";
                else
                    txtCode.Text = dtCommon.Rows[dtCommon.Rows.Count - 1]["Code"].ToString();
            }
            FunPriGetLastGenCode(tcCode.ActiveTabIndex);
        }
        catch (Exception)
        {
        }
        //finally
        //{
        //    if (ObjOrgMasterMgtServicesClient != null)
        //        ObjOrgMasterMgtServicesClient.Close();
        //}
    }

    protected void ddlAssetCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAssetCategory.SelectedValue != "0" && (strCategoryType == 3))//Asset Sub Type                
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Code", ddlAssetCategory.SelectedValue.ToString());
            DataTable dtExisting = Utility.GetDefaultData("S3G_SYSAD_GetAssetParentDetails", dictParam);
            if (dtExisting.Rows.Count > 0)
            {
                lblhassetcategory.Visible = lblrassetcategory.Visible = true;
                lblrassetcategory.Text = dtExisting.Rows[0][0].ToString();
            }
        }
    }

    protected void grvAsset_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtDelete;
        switch (tcCode.ActiveTab.HeaderText)
        {
            case "Category":
                dtDelete = (DataTable)ViewState["CategoryTable"];
                dtDelete.Rows.RemoveAt(e.RowIndex);
                if (dtDelete.Rows.Count <= 0)
                {
                    grvAssetClass.DataSource = null;
                }
                else
                {
                    grvAssetClass.DataSource = dtDelete;
                }
                grvAssetClass.DataBind();
                ViewState["CategoryTable"] = dtDelete;
                hsnrow.Style["display"] = "none";
                txtHSNCode.IsMandatory = false;
                break;
            case "Type":

                dtDelete = (DataTable)ViewState["TypeTable"];
                dtDelete.Rows.RemoveAt(e.RowIndex);
                if (dtDelete.Rows.Count <= 0)
                {
                    grvAssetMake.DataSource = null;
                }
                else
                {
                    grvAssetMake.DataSource = dtDelete;
                }
                grvAssetMake.DataBind();
                ViewState["TypeTable"] = dtDelete;
                hsnrow.Style["display"] = "block";
                txtHSNCode.IsMandatory = true;
                break;
            case "Sub Type":

                dtDelete = (DataTable)ViewState["SubTypeTable"];
                dtDelete.Rows.RemoveAt(e.RowIndex);
                if (dtDelete.Rows.Count <= 0)
                {
                    grvAssetType.DataSource = null;
                }
                else
                {
                    grvAssetType.DataSource = dtDelete;
                }
                grvAssetType.DataBind();
                ViewState["SubTypeTable"] = dtDelete;
                hsnrow.Style["display"] = "none";
                txtHSNCode.IsMandatory = false;
                break;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3gOrgAssetMaster_View.aspx");
    }

    protected void btnCategoryGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCodeDescription.Text != String.Empty)
                txtCodeDescription.Text = RemoveSpace(txtCodeDescription.Text);

            DataRow drClass;
            DataRow drMake;
            DataRow drType;

            DataTable dtCommon = new DataTable();

            if (strCategoryType == 2)
            {

                if (txtHSNCode.SelectedText.ToString() == "" && txtHSNCode.Visible == true)
                {
                    Utility.FunShowAlertMsg(this.Page, "Select HSN Code");
                    return;
                }
            }

            switch (tcCode.ActiveTab.HeaderText)
            {
                case "Category":
                    dtCommon = (DataTable)ViewState["CategoryTable"];
                    if (!FunPriValidCategoryDuplicate(txtCode.Text, txtCodeDescription.Text, "Category", ""))
                    {
                        return;
                    }
                    if (dtCommon.Rows.Count < 10)
                    {
                        drClass = dtCommon.NewRow();
                        drClass["Code"] = txtCode.Text.Trim();
                        drClass["Description"] = txtCodeDescription.Text;
                        drClass["CategoryType"] = "1";
                        dtCommon.Rows.Add(drClass);
                        grvAssetClass.DataSource = dtCommon;
                        grvAssetClass.DataBind();
                        ViewState["CategoryTable"] = dtCommon;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Class", "alert('Cannot add more that 10 Rows');", true);
                    }
                    break;
                case "Type":
                    dtCommon = (DataTable)ViewState["TypeTable"];
                    if (!FunPriValidCategoryDuplicate(txtCode.Text, txtCodeDescription.Text, "Type", ddlAssetCategory.SelectedValue))
                    {
                        return;
                    }
                    if (dtCommon.Rows.Count < 10)
                    {
                        drMake = dtCommon.NewRow();
                        drMake["Code"] = txtCode.Text.Trim();
                        drMake["Description"] = txtCodeDescription.Text;
                        drMake["CategoryType"] = "2";
                        drMake["ParentCode"] = ddlAssetCategory.SelectedValue;
                        dtCommon.Rows.Add(drMake);
                        grvAssetMake.DataSource = dtCommon;
                        grvAssetMake.DataBind();
                        ViewState["TypeTable"] = dtCommon;
                    }
                    break;
                case "Sub Type":
                    dtCommon = (DataTable)ViewState["SubTypeTable"];
                    if (!FunPriValidCategoryDuplicate(txtCode.Text, txtCodeDescription.Text, "Sub Type", ddlAssetCategory.SelectedValue))
                    {
                        return;
                    }
                    if (dtCommon.Rows.Count < 10)
                    {
                        drType = dtCommon.NewRow();
                        drType["Code"] = txtCode.Text.Trim();
                        drType["Description"] = txtCodeDescription.Text;
                        drType["CategoryType"] = "3";
                        drType["ParentCode"] = ddlAssetCategory.SelectedValue;
                        dtCommon.Rows.Add(drType);
                        grvAssetType.DataSource = dtCommon;
                        grvAssetType.DataBind();
                        ViewState["SubTypeTable"] = dtCommon;
                    }
                    break;
            }
            txtCode.Text = (Convert.ToInt32(dtCommon.Rows[dtCommon.Rows.Count - 1]["Code"]) + 1).ToString();
            txtCodeDescription.Text = "";
            btnCategorySubmit.Enabled = true;
            hdnHSN.Text = txtHSNCode.SelectedValue;
            txtHSNCode.SelectedValue = "0";
            txtHSNCode.SelectedText = "--Select--";
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Code"))
            {
                //Utility.FunShowAlertMsg(this, "Asset Category Code already exists in the grid");
                Utility.FunShowAlertMsg(this, "Asset " + tcCode.ActiveTab.HeaderText + " already exists in the grid");
            }
            if (ex.Message.Contains("Description"))
            {
                Utility.FunShowAlertMsg(this, "Asset " + tcCode.ActiveTab.HeaderText.Replace("Code", "") + " Name already exists in the grid");
            }
        }
    }

    private bool FunPriValidCategoryDuplicate(string strCategoryCode, string strCategoryDesc, string strCategoryType, string strParentCode)
    {
        Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
        dictProcParam.Add("@Company_ID", intCompanyId.ToString());
        dictProcParam.Add("@Code", strCategoryCode);
        strCategoryDesc = RemoveSpace(strCategoryDesc);

        dictProcParam.Add("@Description", strCategoryDesc);
        if (strCategoryType == "Category")
            dictProcParam.Add("@CategoryType", "1");
        else if (strCategoryType == "Type")
            dictProcParam.Add("@CategoryType", "2");
        else
            dictProcParam.Add("@CategoryType", "3");
        if (!String.IsNullOrEmpty(strParentCode))
            dictProcParam.Add("@ParentCode", strParentCode.TrimStart().TrimEnd());
        DataTable dtDupliacteTable = Utility.GetDefaultData("S3G_ORG_CheckDuplicateAssetCategory", dictProcParam);
        //if (dtDupliacteTable.Rows[0].ItemArray[0].ToString() == "1")
        //{
        //    Utility.FunShowAlertMsg(this, strCategoryType + " Code already exists.Enter a new " + strCategoryType + " Code");
        //    return false;
        //}
        if (dtDupliacteTable.Rows[0].ItemArray[0].ToString() == "2")
        {
            Utility.FunShowAlertMsg(this, strCategoryType + " Name already exists.Enter a new " + strCategoryType + " Name");
            return false;
        }
        else
        {
            return true;
        }
    }

    protected string RemoveSpace(string str)
    {
        //to remove the space at the start and end
        str = Regex.Replace(str, @"^\s+|\s+$", "");
        //to replace single spaces wherever more than one spaces exist
        str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ");
        return str;
    }

    protected void btnCategorySubmit_Click(object sender, EventArgs e)
    {
        if (txtCodeDescription.Text != String.Empty)
            txtCodeDescription.Text = RemoveSpace(txtCodeDescription.Text);

        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_ORG_AssetCategoryDataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryDataTable();
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_AssetCategoryRow ObjAssetCategoryrRow;

            if (strCategoryType == 2)
            {
                if (strMode == "M")
                {
                    if (txtHSNCode.SelectedText.ToString() == "" && txtHSNCode.Visible == true)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Select HSN Code");
                        return;
                    }
                    hdnHSN.Text = txtHSNCode.SelectedValue;
                }

            }


            DataTable dtSubmit = new DataTable();
            dtSubmit.Merge((DataTable)ViewState["CategoryTable"]);
            dtSubmit.Merge((DataTable)ViewState["TypeTable"]);
            dtSubmit.Merge((DataTable)ViewState["SubTypeTable"]);

            if (PageMode == PageModes.Create)
            {
                if (dtSubmit.Rows.Count > 0)
                {

                    ObjAssetCategoryrRow = ObjS3G_ORG_AssetCategoryDataTable.NewS3G_ORG_AssetCategoryRow();
                    ObjAssetCategoryrRow.Company_ID = intCompanyId;
                    ObjAssetCategoryrRow.Created_By = intUserId;
                    ObjAssetCategoryrRow.Category_ID = "0";
                    ObjAssetCategoryrRow.Category_Description = txtCodeDescription.Text.Trim();
                    ObjAssetCategoryrRow.Is_Active = chkActive.Checked;
                    ObjAssetCategoryrRow.XMLDetails = dtSubmit.FunPubFormXml();
                    if (strCategoryType == 2)
                        ObjAssetCategoryrRow.HSN_Id = Convert.ToInt32(hdnHSN.Text);
                    else
                        ObjAssetCategoryrRow.HSN_Id = 0;
                    ObjS3G_ORG_AssetCategoryDataTable.AddS3G_ORG_AssetCategoryRow(ObjAssetCategoryrRow);

                    if (ObjS3G_ORG_AssetCategoryDataTable.Rows.Count > 0)
                    {
                        SerializationMode SerMode = SerializationMode.Binary;
                        byte[] byteobjS3G_ORG_AssetCategoryDataTable = ClsPubSerialize.Serialize(ObjS3G_ORG_AssetCategoryDataTable, SerMode);

                        int intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateAssetCategoryInt(SerMode, byteobjS3G_ORG_AssetCategoryDataTable);
                        if (intErrorCode == 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Asset added successfully");
                            FunProIntializeData();

                            //Added By Thangam M on 15/Feb/2012 to refresh the code after save
                            int intCategory = tcCode.ActiveTabIndex;
                            FunPriGetLastGenCode(intCategory);

                        }
                        if (intErrorCode == 1)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Asset already exists");
                            strRedirectPageAdd = "";
                            FunProIntializeData();

                        }
                        if (intErrorCode == 2)
                        {
                            strAlert = strAlert.Replace("__ALERT__", lblCodeDesc.Text + " already exists");
                            FunProIntializeData();
                            strRedirectPageAdd = "";

                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Error adding asset category code");
                            strRedirectPageAdd = "";
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Atleast one category Name should be given');", true);
                }
            }
            else
            {
                ObjAssetCategoryrRow = ObjS3G_ORG_AssetCategoryDataTable.NewS3G_ORG_AssetCategoryRow();
                ObjAssetCategoryrRow.Company_ID = intCompanyId;
                ObjAssetCategoryrRow.Created_By = intUserId;
                ObjAssetCategoryrRow.Category_Type = "";
                ObjAssetCategoryrRow.Category_Code = txtCode.Text;
                ObjAssetCategoryrRow.Category_Description = txtCodeDescription.Text.TrimStart().TrimEnd();
                ObjAssetCategoryrRow.Is_Active = chkActive.Checked;
                ObjAssetCategoryrRow.Category_ID = intAsset_CatId.ToString();
                if (strCategoryType == 2)
                    ObjAssetCategoryrRow.HSN_Id = Convert.ToInt32(hdnHSN.Text);
                else
                    ObjAssetCategoryrRow.HSN_Id = 0;
                ObjS3G_ORG_AssetCategoryDataTable.AddS3G_ORG_AssetCategoryRow(ObjAssetCategoryrRow);
                if (ObjS3G_ORG_AssetCategoryDataTable.Rows.Count > 0)
                {
                    SerializationMode SerMode = SerializationMode.Binary;
                    byte[] byteobjS3G_ORG_AssetCategoryDataTable = ClsPubSerialize.Serialize(ObjS3G_ORG_AssetCategoryDataTable, SerMode);
                    //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                    int intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateAssetCategoryInt(SerMode, byteobjS3G_ORG_AssetCategoryDataTable);

                    if (intErrorCode == 0)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Asset updated successfully");
                    }

                    if (intErrorCode == 10)
                    {
                        strAlert = strAlert.Replace("__ALERT__", lblCodeDesc.Text + " already exists");
                        strRedirectPageView = "";
                    }

                    if (intErrorCode == 11)
                    {
                        strAlert = strAlert.Replace("__ALERT__", lblCode.Text + " Mapped");
                        strRedirectPageView = "";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            //if (ObjOrgMasterMgtServicesClient != null)
            ObjOrgMasterMgtServicesClient.Close();
        }
    }

    #endregion

    #region Page Methods

    public void FunPubLoadParentMappingDropDown(int intCurrentTabIndex)
    {
        try
        {
            DataTable dtAsset = new DataTable();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", (intCurrentTabIndex).ToString());
            DataTable dtExisting = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);

            if (intCurrentTabIndex == 1)
                dtAsset = ((DataTable)ViewState["CategoryTable"]);
            else if (intCurrentTabIndex == 2)
                dtAsset = ((DataTable)ViewState["TypeTable"]);

            if (dtAsset.Rows.Count > 0)
            {
                dtExisting.Merge(dtAsset);
            }

            ddlAssetCategory.DataSource = null;
            ddlAssetCategory.DataBind();
            ddlAssetCategory.BindDataTable(dtExisting, "Code", "Description");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }

    public void FunPubLoadParentDropDown(int intCurrentTabIndex)
    {
        try
        {
            DataTable dtAsset = new DataTable();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@Category_Type", (intCurrentTabIndex).ToString());
            DataTable dtExisting = Utility.GetDefaultData("S3G_SYSAD_GetAssetType", dictParam);

            if (intCurrentTabIndex == 1)
                dtAsset = ((DataTable)ViewState["CategoryTable"]);
            else if (intCurrentTabIndex == 2)
                dtAsset = ((DataTable)ViewState["TypeTable"]);

            if (dtAsset.Rows.Count > 0)
            {
                dtExisting.Merge(dtAsset);
            }

            ddlAssetCategory.DataSource = null;
            ddlAssetCategory.DataBind();
            ddlAssetCategory.BindDataTable(dtExisting, "Id", "Description");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw;
        }
    }


    /// <summary>
    /// To Initialize Data table for temp adding of data
    /// </summary>
    protected void FunProIntializeData()
    {
        DataTable dtAssetCategory;
        DataTable dtAssetType;
        DataTable dtAssetSubType;

        dtAssetCategory = new DataTable("Asset Category");
        dtAssetCategory.Columns.Add("Code");
        dtAssetCategory.Columns.Add("Description");
        dtAssetCategory.Columns.Add("CategoryType");
        dtAssetCategory.Columns[0].Unique = true;
        dtAssetCategory.Columns[1].Unique = true;

        dtAssetType = new DataTable("Asset Type");
        dtAssetType.Columns.Add("Code");
        dtAssetType.Columns.Add("Description");
        dtAssetType.Columns.Add("CategoryType");
        dtAssetType.Columns.Add("ParentCode");
        dtAssetType.Columns[0].Unique = true;
        dtAssetType.Columns[1].Unique = true;

        dtAssetSubType = new DataTable("Asset Sub Type");
        dtAssetSubType.Columns.Add("Code");
        dtAssetSubType.Columns.Add("Description");
        dtAssetSubType.Columns.Add("CategoryType");
        dtAssetSubType.Columns.Add("ParentCode");
        dtAssetSubType.Columns[0].Unique = true;
        dtAssetSubType.Columns[1].Unique = true;


        ViewState["CategoryTable"] = dtAssetCategory;
        ViewState["TypeTable"] = dtAssetType;
        ViewState["SubTypeTable"] = dtAssetSubType;

        grvAssetClass.DataSource = null;
        grvAssetClass.DataBind();

        grvAssetMake.DataSource = null;
        grvAssetMake.DataBind();

        grvAssetType.DataSource = null;
        grvAssetType.DataBind();

        txtCode.Text = "";
        txtCodeDescription.Text = "";

    }

    /// <summary>
    /// To load asset category codes based on company asset category and type
    /// </summary>
    /// <param name="intCompanyId"></param>
    /// <param name="intAsset_CatId"></param>
    /// <param name="intAssetType_Id"></param>
    protected void FunPubLoadAssetCategories(int intCompanyId, int intAsset_CatId, int strCategoryType)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            DataSet dsAssetCategory = new DataSet();

            byte[] byte_AssetCategory = ObjOrgMasterMgtServicesClient.FunPubQueryAssetCategoryDetails(intCompanyId, intAsset_CatId, null);
            dsAssetCategory = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_AssetCategory, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));

            if (strCategoryType == 1)
            {
                grvAssetClass.DataSource = dsAssetCategory.Tables[0].DefaultView;
                grvAssetClass.DataBind();
                tcCode.ActiveTabIndex = 0;

                hsnrow.Style["display"] = "none";
                txtHSNCode.IsMandatory = false;
            }
            else if (strCategoryType == 2)
            {
                grvAssetMake.DataSource = dsAssetCategory.Tables[0].DefaultView;
                grvAssetMake.DataBind();
                tcCode.ActiveTabIndex = 1;
                txtHSNCode.SelectedValue = dsAssetCategory.Tables[0].Rows[0]["Hsn_code"].ToString();
                txtHSNCode.SelectedText = dsAssetCategory.Tables[0].Rows[0]["hsn_name"].ToString();

                hsnrow.Style["display"] = "block";
                txtHSNCode.IsMandatory = true;
            }
            else
            {
                grvAssetType.DataSource = dsAssetCategory.Tables[0].DefaultView;
                grvAssetType.DataBind();
                lblhassetcategory.Visible = lblrassetcategory.Visible = true;
                lblrassetcategory.Text = dsAssetCategory.Tables[0].Rows[0]["Grand_Parent"].ToString();
                tcCode.ActiveTabIndex = 2;

                hsnrow.Style["display"] = "none";
                txtHSNCode.IsMandatory = false;
            }

            txtCode.Text = dsAssetCategory.Tables[0].Rows[0]["Code"].ToString();
            txtCodeDescription.Text = dsAssetCategory.Tables[0].Rows[0]["Description"].ToString();
            if (!String.IsNullOrEmpty(dsAssetCategory.Tables[0].Rows[0]["Is_Active"].ToString()))
                chkActive.Checked = Convert.ToBoolean(dsAssetCategory.Tables[0].Rows[0]["Is_Active"]);

            if (strCategoryType != 1)
            {
                FunPubLoadParentDropDown(strCategoryType - 1);
                lblAssetCategory.Visible = ddlAssetCategory.Visible = true;
                ddlAssetCategory.SelectedValue = Convert.ToString(dsAssetCategory.Tables[0].Rows[0]["ParentId"]);

                if (strMode != "C")
                    ddlAssetCategory.ClearDropDownList();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new Exception("Unable to load Asset category details");
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();
        }
    }

    /// <summary>
    /// Asset Controls  
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriAssetControlStatus(int intModeID)
    {
        switch (intModeID)
        {

            case 0://Create
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                chkActive.Enabled = false;
                chkActive.Checked = true;
                txtCode.Attributes.Add("readonly", "readonly");
                rfvAssetCategory1.Enabled = rfvDesc.Enabled = false;

                if (!bCreate)
                {
                    btnCategoryGo.Enabled = false;
                    btnCategorySubmit.Enabled = false;
                }
                break;

            case 1://Modify
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                txtCode.Attributes.Add("readonly", "readonly");
                if (!bModify)
                    btnCategorySubmit.Enabled = false;
                btnCategoryGo.Enabled = false;
                tcCode.Enabled = false;
                //if (chkActive.Checked == false)
                //{
                //    btnCategorySubmit.Enabled = false;
                //    txtCode.ReadOnly = true;
                //    txtCodeDescription.ReadOnly = true;
                //}
                break;

            case -1://Query

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }
                chkActive.Enabled = false;
                txtHSNCode.ReadOnly = true;
                btnCategoryGo.Enabled = false;
                btnCategorySubmit.Enabled = false;
                txtCode.ReadOnly = true;
                txtCodeDescription.ReadOnly = true;
                tcCode.Enabled = false;

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                break;
        }
    }

    /// <summary>
    /// Method to get Last generated code
    /// </summary>
    /// <param name="strCategory"></param>
    private void FunPriGetLastGenCode(int intCategory)
    {
        try
        {
            tcCode.ActiveTabIndex = intCategory;
            switch (intCategory)
            {
                case 0:
                    lblCode.Text = "Asset Category Code";
                    lblCodeDesc.Text = "Asset Category Name";
                    lblAssetCategory.Visible = ddlAssetCategory.Visible = false;
                    rfvAssetCategory.Enabled = false;
                    hsnrow.Style["display"] = "none";
                    txtHSNCode.IsMandatory = false;
                    break;
                case 1:
                    rfvAssetCategory.ErrorMessage = "Select the Asset Category";
                    lblAssetCategory.Text = "Asset Category";
                    lblCode.Text = "Asset Type Code";
                    lblCodeDesc.Text = "Asset Type Name";
                    lblAssetCategory.Visible = ddlAssetCategory.Visible = true;
                    rfvAssetCategory.Enabled = true;
                    FunPubLoadParentMappingDropDown(intCategory);
                    hsnrow.Style["display"] = "block";
                    txtHSNCode.IsMandatory = true;
                    break;
                case 2:
                    rfvAssetCategory.ErrorMessage = "Select the Asset Type";
                    lblAssetCategory.Text = "Asset Type";
                    lblCode.Text = "Asset Sub Type Code";
                    lblCodeDesc.Text = "Asset Sub Type Name";
                    lblAssetCategory.Visible = ddlAssetCategory.Visible = true;
                    rfvAssetCategory.Enabled = true;
                    FunPubLoadParentMappingDropDown(intCategory);
                    hsnrow.Style["display"] = "none";
                    txtHSNCode.IsMandatory = false;
                    break;
            }
            //Added by Thangam M on 15/Feb/2012 to change the validation messages based on the tab
            FunSetCodeValidations();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjOrgMasterMgtServicesClient.Close();
        }
    }

    private void FunPriGetLabelName(int intCategory)
    {
        try
        {
            tcCode.ActiveTabIndex = intCategory;
            switch (intCategory)
            {
                case 0:
                    lblCode.Text = "Asset Category Code";
                    lblCodeDesc.Text = "Asset Category Name";
                    lblAssetCategory.Visible = ddlAssetCategory.Visible = false;
                    rfvAssetCategory.Enabled = false;
                    break;
                case 1:
                    rfvAssetCategory.ErrorMessage = "Select the Asset Category";
                    lblAssetCategory.Text = "Asset Category";
                    lblCode.Text = "Asset Type Code";
                    lblCodeDesc.Text = "Asset Type Name";
                    lblAssetCategory.Visible = ddlAssetCategory.Visible = true;
                    rfvAssetCategory.Enabled = true;
                    break;
                case 2:
                    rfvAssetCategory.ErrorMessage = "Select the Asset Type";
                    lblAssetCategory.Text = "Asset Type";
                    lblCode.Text = "Asset Sub Type Code";
                    lblCodeDesc.Text = "Asset Sub Type Name";
                    lblAssetCategory.Visible = ddlAssetCategory.Visible = true;
                    rfvAssetCategory.Enabled = true;
                    break;
            }
            //Added by Thangam M on 15/Feb/2012 to change the validation messages based on the tab
            FunSetCodeValidations();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();
        }

    }

    //Added by Thangam M on 15/Feb/2012 to change the validation messages based on the tab
    private void FunSetCodeValidations()
    {
        rfvCode.ErrorMessage = "Enter the " + lblCode.Text;
        rfvCodeDescription.ErrorMessage = "Enter the " + lblCodeDesc.Text;
        rfvDesc.ErrorMessage = "Enter the " + lblCodeDesc.Text;
    }

    /// <summary>
    /// Method to uncheck all checkbox
    /// </summary>
    /// <param name="grv"></param>
    private void FunGridUncheckAll(GridView grv)
    {
        foreach (GridViewRow grvRow in grv.Rows)
        {
            CheckBox chkBox = (CheckBox)grvRow.FindControl("chkCategoryCode");
            chkBox.Checked = false;
        }
    }

    #endregion


    [System.Web.Services.WebMethod]
    public static string[] GetHSNList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_HSN_AGT", Procparam));

        return suggetions.ToArray();

    }



}
