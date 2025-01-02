using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoanAdmin_S3G_ORG_AccountHomeLoanAsset : ApplyThemeForProject
{
    #region Initialization

    /// <summary>
    /// To Initialize Objects and Variables
    /// </summary>
    /// 
    int intCompanyId = 0;
    int intUserId = 0;
    int intBranchId = 2;
    int intSerialNo = 0;
    int intErrorCode = 0;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    UserInfo ObjUserInfo;
    S3GSession objS3GSession;
    //User Authorization
    string strMode = string.Empty;
    string strDateFormat = "";
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    string[] ErrorList = new string[3];
    public static LoanAdmin_S3G_ORG_AccountHomeLoanAsset obj_Page;
    static string strPageName = "Account Asset Details";
    string strNewWin = " window.showModalDialog('../LoanAdmin/S3G_ORG_AccountHomeLoanAsset.aspx?qsMaster=Yes&qsRowID=";
    string NewWinAttributes = "', 'Asset Details', 'dialogwidth:700px;dialogHeight:400px;');";
    //Code end
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        objS3GSession = new S3GSession();
        obj_Page = this;
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        strDateFormat = objS3GSession.ProDateFormatRW;
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
        //Code end
        if (Request.QueryString["qsMode"] != null)
        {
            btnOK.Enabled = false;
        }

        if (Request.QueryString["qsRowID"] != null)                               //When click Add for First Row//
        {
            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);
        }
        if (!IsPostBack)
        {

            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);
            FunPriLoadAssetDetails((DataTable)Session["ApplicationloanAssetDetails"]);
        }
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
    }
    private void FunPriLoadAssetDetails(DataTable dtAssetDetails)
    {
        DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + intSerialNo.ToString());
        if (drAsset.Length == 0)
        {
            txtSlNo.Text = Convert.ToString(dtAssetDetails.Rows.Count + 1);
            return;
        }
        txtSlNo.Text = (intSerialNo).ToString();
        ddlAssettypeList.SelectedItem.Text = drAsset[0]["Asset_Type_value"].ToString();
        txtbuildername.Text = drAsset[0]["Builder_Name"].ToString();
        txtflatarea.Text = drAsset[0]["Flat_Area"].ToString();
        txtflatno.Text = drAsset[0]["Flat_No"].ToString();
        txtaddress.Text = drAsset[0]["Address"].ToString();
        txtvalue.Text = drAsset[0]["Asset_Value"].ToString();
        txtsurveyor.Text = drAsset[0]["Land_Surveyor"].ToString();
        txtbuilduparea.Text = drAsset[0]["Buildup_Area"].ToString();
        
        

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        DataTable dtAssetDetails = new DataTable();
        if (Session["PricingloanAssetDetails"] == null)
        {
            dtAssetDetails.Columns.Add("Asset_Type");
            dtAssetDetails.Columns.Add("Asset_Type_value");
            dtAssetDetails.Columns.Add("Builder_Name");
            dtAssetDetails.Columns.Add("Flat_Area");
            dtAssetDetails.Columns.Add("Buildup_Area");
            dtAssetDetails.Columns.Add("Flat_No");
            dtAssetDetails.Columns.Add("Address");
            dtAssetDetails.Columns.Add("Asset_Value");
            dtAssetDetails.Columns.Add("Land_Surveyor");
            dtAssetDetails.Columns.Add("SlNo");

        }
        else
        {
            dtAssetDetails = (DataTable)Session["PricingloanAssetDetails"];
        }

        if (intSerialNo == 0)
        {

            DataRow Dr = dtAssetDetails.NewRow();
            Dr["SlNo"] = dtAssetDetails.Rows.Count + 1;
            Dr["Asset_Type"] = ddlAssettypeList.SelectedValue.ToString();
            Dr["Asset_Type_value"] = ddlAssettypeList.SelectedItem.Text.ToString();
            Dr["Builder_Name"] = txtbuildername.Text.ToString();
            Dr["Flat_Area"] = txtflatarea.Text.ToString();
            Dr["Buildup_Area"] = txtbuilduparea.Text.ToString();
            Dr["Flat_No"] = txtflatno.Text.Trim();
            Dr["Address"] = txtaddress.Text.Trim();
            Dr["Asset_Value"] = txtaddress.Text.Trim();
            Dr["Land_Surveyor"] = txtsurveyor.Text.Trim();


            dtAssetDetails.Rows.Add(Dr);
        }
        //else
        //{
        //    DataRow[] drAsset = dtAssetDetails.Select("SlNo = " + intSerialNo.ToString());
        //    drAsset["Asset_Type"] = ddlAssettypeList.SelectedValue.ToString();
        //    drAsset["Builder_Name"] = txtbuildername.Text.ToString();
        //    drAsset["Flat_Area"] = txtflatarea.Text.ToString();
        //    drAsset["Buildup_Area"] = txtbuilduparea.Text.ToString();
        //    drAsset["Flat_No"] = txtflatno.Text.Trim();
        //    drAsset["Address"] = txtaddress.Text.Trim();
        //    drAsset["Asset_Value"] = txtaddress.Text.Trim();
        //    drAsset["Land_Surveyor"] = txtsurveyor.Text.Trim();
        //    drAsset[0].AcceptChanges();
        //}
        Session["PricingloanAssetDetails"] = dtAssetDetails;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
    }

}