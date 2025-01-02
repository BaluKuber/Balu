using S3GBusEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Origination_S3GOrgAssetMaster_Add_HomeLoan : ApplyThemeForProject
{
    
    int intCompanyId = 0;
    int intUserId = 0;
    int intAssetId = 0;
    int intAsset_CatId = 0;
    string strCategoryType = string.Empty;
    int intErrorCode = 0;
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_AssetMaster_HLDataTable ObjS3G_ORG_AssetMasterDataTable;
   
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    string strRedirectPageView = "window.location.href='../Common/S3GMaster.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgAssetMaster_Add_HomeLoan.aspx';";
   
    static string strPageName = "Asset master HL";
    UserInfo ObjUserInfo;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    Dictionary<string, string> Procparam;
    DataTable dtasset;
    int strAsset_id = 0;
    DataSet dsgrid;
    //Code end
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

            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString["qsAssetId"] != null)
            {
            }
            if (!IsPostBack)
            {
                TabContainerAP.ActiveTab = TabMainPage;
                FunPriLoadAssetType();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunPriLoadAssetType()
    {
         Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@user_id", intUserId.ToString());
        Procparam.Add("@Lookup_Desc", "Asset Type");
        ddlAssetType.BindDataTable("S3G_ORG_GetLookup_PL", Procparam, new string[] { "Id", "Value" });
       
    }

    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtassetcode.Text = "";
        txtassetdesc.Text = "";
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@company_id", intCompanyId.ToString());
        Procparam.Add("@user_id", intCompanyId.ToString());
        Procparam.Add("@Lookup_Desc", ddlAssetType.SelectedItem.Text.Trim());
        dsgrid = Utility.GetDataset("Get_AssetMaster_HL", Procparam);
        if (dsgrid.Tables[0].Rows.Count > 0)
        {
            txtassetcode.Text = dsgrid.Tables[0].Rows[0]["Asset_code"].ToString();
            txtassetdesc.Text = dsgrid.Tables[0].Rows[0]["Asset_Desc"].ToString();
            strAsset_id = Convert.ToInt32(dsgrid.Tables[0].Rows[0]["Asset_id"].ToString());
            chkactive.Checked = Convert.ToBoolean(dsgrid.Tables[0].Rows[0]["is_active"].ToString());
        }
        if (dsgrid.Tables[1].Rows.Count > 0)
        {
            ViewState["dtasset"] = dsgrid.Tables[1];
            gvAccountLedger.DataSource = dsgrid.Tables[1];
            gvAccountLedger.DataBind();
        }
        else
        {
            FunPriInitializeAssetGridData();
        }
    }

    private void FunPriInitializeAssetGridData()
    {
        DataRow dRow;
          dtasset = new DataTable();

            dtasset.Columns.Add("flag");
            dtasset.Columns.Add("description");
            dtasset.Columns.Add("Characteristics");
            dtasset.Columns.Add("Mandatory_value");
            dtasset.Columns.Add("mandatory_scan");
            dtasset.Columns.Add("Mandatory_Document");
            dtasset.Columns.Add("link");
            //  dtasset.Columns.Add("Gp_no");
            dtasset.Columns.Add("Gp_Desc");



            dRow = dtasset.NewRow();

            dRow["flag"] = "";
            dRow["description"] = "";
            dRow["Characteristics"] = "";
            dRow["Mandatory_value"] = "true";
            dRow["mandatory_scan"] = "true";
            dRow["Mandatory_Document"] = "true";
            dRow["link"] = "true";
            // dRow["Gp_no"] = "";
            dRow["Gp_Desc"] = "";

            dtasset.Rows.Add(dRow);

            ViewState["dtasset"] = dtasset;
            FunFillgrid(gvAccountLedger, dtasset);
            ((DataTable)ViewState["dtasset"]).Rows.RemoveAt(0);
            gvAccountLedger.Rows[0].Visible = false;
      

    }

    private void FunFillgrid(GridView grv, DataTable dtbl)
    {
        try
        {
            grv.DataSource = dtbl;
            grv.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void gvAccountLedger_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Footer)
        {

            DropDownList ddlflagid = (DropDownList)e.Row.FindControl("ddlflagfooter");
            DropDownList ddlcharfooter = (DropDownList)e.Row.FindControl("ddlcharfooter");
            

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@company_id", intCompanyId.ToString());
            Procparam.Add("@user_id", intCompanyId.ToString());
            Procparam.Add("@Lookup_Desc", ddlAssetType.SelectedItem.Text.Trim());
            ddlflagid.BindDataTable("S3G_ORG_GetLookup_PL", Procparam, new string[] { "Id", "Value" });

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@company_id", intCompanyId.ToString());
            Procparam.Add("@user_id", intCompanyId.ToString());
            Procparam.Add("@Lookup_Desc", "Characteristics");
            ddlcharfooter.BindDataTable("S3G_ORG_GetLookup_PL", Procparam, new string[] { "Id", "Value" });

          

        }
    }

    protected void gvAccountLedger_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {

            DropDownList ddlflagfooter = (DropDownList)gvAccountLedger.FooterRow.FindControl("ddlflagfooter");
            DropDownList ddlcharfooter = (DropDownList)gvAccountLedger.FooterRow.FindControl("ddlcharfooter");
            TextBox txtdescripitionfooter = (TextBox)gvAccountLedger.FooterRow.FindControl("txtdescripitionfooter");
            CheckBox chkmandatoryvalue = (CheckBox)gvAccountLedger.FooterRow.FindControl("chkmandatoryvalue");
             CheckBox chkmandatoryscanfooter = (CheckBox)gvAccountLedger.FooterRow.FindControl("chkmandatoryscanfooter");
             CheckBox chkmandatorydocumentsfooter = (CheckBox)gvAccountLedger.FooterRow.FindControl("chkmandatorydocumentsfooter");
             CheckBox chkLinkfooter = (CheckBox)gvAccountLedger.FooterRow.FindControl("chkLinkfooter");
              TextBox txtgpdescfooter = (TextBox)gvAccountLedger.FooterRow.FindControl("txtgpdescfooter");

              DataTable dtasset = (DataTable)ViewState["dtasset"];

              DataRow dr = dtasset.NewRow();
              dr["flag"] = ddlflagfooter.SelectedItem.Text.ToString();
            dr["description"] = txtdescripitionfooter.Text.ToString();
            dr["Characteristics"] = ddlcharfooter.SelectedItem.Text.Trim();
            if(chkmandatoryvalue.Checked)
            dr["Mandatory_value"] ="true";
            else
                dr["Mandatory_value"] = "false";
            if (chkmandatoryscanfooter.Checked)
                dr["mandatory_scan"] = "true";
            else
                dr["mandatory_scan"] = "false";
            if (chkmandatorydocumentsfooter.Checked)
                dr["Mandatory_Document"] = "true";
            else
                dr["Mandatory_Document"] = "false";
            if (chkLinkfooter.Checked)
                dr["link"] = "true";
            else
                dr["link"] = "false";
            // dRow["Gp_no"] = "";
            dr["Gp_Desc"] = txtgpdescfooter.Text.ToString();
           

            //ViewState["dtlegalclearance"] = dt;
            dtasset.Rows.Add(dr);

            ViewState["dtasset"] = dtasset;
            gvAccountLedger.DataSource = dtasset;
            gvAccountLedger.DataBind();
          

        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_ORG_AssetMasterDataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_AssetMaster_HLDataTable();
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_AssetMaster_HLRow ObjAssetMasterRow;
            dtasset = (DataTable)ViewState["dtasset"]; 
            ObjAssetMasterRow = ObjS3G_ORG_AssetMasterDataTable.NewS3G_Org_AssetMaster_HLRow();
            ObjAssetMasterRow.Company_id = intCompanyId;
            ObjAssetMasterRow.User_id = intUserId;
             ObjAssetMasterRow.Asset_id = strAsset_id;
             ObjAssetMasterRow.Asset_Type = ddlAssetType.SelectedItem.Text.ToString();
            ObjAssetMasterRow.Asset_Code = txtassetcode.Text.ToString();
            ObjAssetMasterRow.Asset_Desc = txtassetdesc.Text.Trim();
            ObjAssetMasterRow.Is_Active = Convert.ToInt32(Convert.ToBoolean(chkactive.Checked.ToString()));
            ObjAssetMasterRow.XmlDetail = dtasset.FunPubFormXml();
            /******Code here added by Nataraj Y on 17 -05-2011 for new Asset code****/
          
            ObjS3G_ORG_AssetMasterDataTable.AddS3G_Org_AssetMaster_HLRow(ObjAssetMasterRow);

            if (ObjS3G_ORG_AssetMasterDataTable.Rows.Count > 0)
            {
                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteobjS3G_ORG_AssetMasterDataTable = ClsPubSerialize.Serialize(ObjS3G_ORG_AssetMasterDataTable, SerMode);
                //ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                if (intAssetId == 0)
                {
                    intErrorCode = ObjOrgMasterMgtServicesClient.FunPubCreateAssetCodeHL(SerMode, byteobjS3G_ORG_AssetMasterDataTable);
                }
               
                 switch (intErrorCode)
                    {
                        case 0:

                           
                            strAlert = "Asset code added successfully";
                            strAlert += @"\n\nWould you like to add one more Asset?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            break;
                       default:
                            strAlert = strAlert.Replace("__ALERT__", "Error in adding asset code");
                            strRedirectPageView = "";
                            break;
                    }
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            // if (ObjOrgMasterMgtServicesClient != null)
            ObjOrgMasterMgtServicesClient.Close();

        }

    }

   
}
