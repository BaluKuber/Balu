
#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using AjaxControlToolkit;
#endregion

public partial class Origination_S3G_Org_HSN_Add : ApplyThemeForProject
{
    #region Intialization
    int intErrCode = 0;
    int intHSNId = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode SerMode = SerializationMode.Binary;

    string strKey = "Insert";
    string strMode = string.Empty;

    Dictionary<string, string> Procparm = new Dictionary<string, string>();
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "../Origination/S3G_Org_HSN_View.aspx";
    string strRedirectPageView = "window.location.href='../Origination/S3G_Org_HSN_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_Org_HSN_Add.aspx';";

    static bool IsAcitve;

    public static Origination_S3G_Org_HSN_Add ojb_TransLander = null;

    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_HSNMstDataTable ObjHSNDataTable;
    //S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_HSNMstRow ObjHSNRow;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;




    #endregion

    #region PageLoad
    /// <summary>
    ///Page Load Events
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ojb_TransLander = this;

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            if (Request.QueryString["qsPId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsPId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intHSNId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }


            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;



            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

            if (!IsPostBack)
            {
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                if (((intHSNId > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if (((intHSNId > 0)) && (strMode == "Q"))
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);

                }
            }
        }
        catch
        {

        }
    }
    #endregion
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                chkActive.Enabled = false;


                break;

            case 1: // Modify Mode
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                txtHSNCode.ReadOnly = true;
                txtHSNDesc.ReadOnly = true;
                rdbcode.Enabled = false;
                FunGetHSNDet();
                btnClear.Enabled = false;
                break;

            case -1:// Query Mode


                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView, false);
                }
                chkActive.Enabled = false;

                txtHSNDesc.ReadOnly = true;
                txtHSNCode.Enabled = true;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                rdbcode.Enabled = false;
                FunGetHSNDet();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                break;
        }

    }




    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjOrgMasterMgtServicesClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {


            ObjHSNDataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_HSNMstDataTable();
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Org_HSNMstRow ObjHSNRow = ObjHSNDataTable.NewS3G_Org_HSNMstRow();
            //  ObjHSNRow = ObjHSNDataTable.NewS3G_Org_HSNMstRow();
            ObjHSNRow.HSN_Id = intHSNId;
            ObjHSNRow.Is_Active = chkActive.Checked;
            ObjHSNRow.HSN_Code = txtHSNCode.Text.ToString();
            ObjHSNRow.HSN_Desc = txtHSNDesc.Text.ToString();
            ObjHSNRow.Company_id = intCompanyID;
            ObjHSNRow.Created_by = Convert.ToInt32(intUserID.ToString());
            ObjHSNRow.Modified_by = Convert.ToInt32(intUserID.ToString());
            ObjHSNRow.Code_Type = Convert.ToInt32(rdbcode.SelectedValue);
            if (Convert.ToInt32(rdbcode.SelectedValue) == 1)
                ObjHSNRow.SAC_Id = Convert.ToInt32(TxtSAC.SelectedValue);

            else

                ObjHSNRow.SAC_Id = 0;

            ObjHSNDataTable.AddS3G_Org_HSNMstRow(ObjHSNRow);
            SerializationMode SerMode = SerializationMode.Binary;

            intErrCode = ObjOrgMasterMgtServicesClient.FunHSNMasterInsertInt(SerMode, ClsPubSerialize.Serialize(ObjHSNDataTable, SerMode));




            if (intErrCode == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The entered  code already exist.');", true);
                return;
            }
            if (intErrCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Code mapped to Asset');", true);
                return;
            }


            else if (intErrCode == 0)
            {
                if (intHSNId > 0)
                {


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('HSN details updated successfully');window.location.href='../Origination/S3G_Org_HSN_View.aspx';", true);
                }
                else
                {
                    //Added by Bhuvana M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    if (rdbcode.SelectedValue == "1")
                    {
                        strAlert = "HSN details added successfully";
                        strAlert += @"\n\nWould you like to add one more HSN?";
                    }
                    else if (rdbcode.SelectedValue == "2")
                    {
                        strAlert = "SAC Code details added successfully";
                        strAlert += @"\n\nWould you like to add one more SAC?";
                    }
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
            }

            lblErrorMessage.Text = string.Empty;
        }

        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjOrgMasterMgtServicesClient.Close();
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtHSNCode.Text = "";
        txtHSNDesc.Text = "";
        TxtSAC.Clear();
        rdbcode.SelectedValue = "1";
        lblHSNCode.Text = "HSN Code";
        lblHSNDesc.Text = "HSN Description";
        LblSAC.Visible = true;
        TxtSAC.Visible = true;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3G_Org_HSN_View.aspx", false);
    }
    private void FunGetHSNDet()
    {


        try
        {
            Dictionary<string, string> Procparm = new Dictionary<string, string>();

            Procparm = new Dictionary<string, string>();
            Procparm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparm.Add("@HSN_id", intHSNId.ToString());
            DataSet ds = Utility.GetDataset("S3G_Get_HSN_Details", Procparm);

            if (ds.Tables.Count > 0)
            {
                txtHSNCode.Text = ds.Tables[0].Rows[0]["HSN_Code"].ToString();
                txtHSNDesc.Text = ds.Tables[0].Rows[0]["HSN_Desc"].ToString();
                txtHSNDesc.Text = ds.Tables[0].Rows[0]["HSN_Desc"].ToString();
                rdbcode.SelectedValue = ds.Tables[0].Rows[0]["code_type"].ToString();
                if (ds.Tables[0].Rows[0]["Is_Active"].ToString() == "True")
                {
                    chkActive.Checked = true;
                }
                else
                {
                    chkActive.Checked = false;
                }
                if (rdbcode.SelectedValue == "1")
                {
                    lblHSNCode.Text = "HSN Code";
                    lblHSNDesc.Text = "HSN Description";
                    LblSAC.Visible = true;
                    TxtSAC.Visible = true;
                    TxtSAC.SelectedText = ds.Tables[0].Rows[0]["SAC_Desc"].ToString();
                    TxtSAC.SelectedValue = ds.Tables[0].Rows[0]["SAC_Id"].ToString();
                    //rfvSACDet.Enabled = true;
                }
                else if (rdbcode.SelectedValue == "2")
                {
                    lblHSNCode.Text = "SAC Code";
                    lblHSNDesc.Text = "SAC Description";
                    LblSAC.Visible = false;
                    TxtSAC.Visible = false;
                    //rfvSACDet.Enabled = false;
                }

            }



        }

        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }




    }
    protected void rdbcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbcode.SelectedValue == "1")
        {
            lblHSNCode.Text = "HSN Code";
            lblHSNDesc.Text = "HSN Description";
            LblSAC.Visible = true;
            TxtSAC.Visible = true;
            //rfvSACDet.Enabled = true;
        }
        else if (rdbcode.SelectedValue == "2")
        {
            lblHSNCode.Text = "SAC Code";
            lblHSNDesc.Text = "SAC Description";
            LblSAC.Visible = false;
            TxtSAC.Visible = false;
            //rfvSACDet.Enabled = false;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetHSNCode(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(ojb_TransLander.intCompanyID));
        Procparam.Add("@Option", "4");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GETTAXGUIDE_TRANSVALUES", Procparam));


        return suggetions.ToArray();
    }

}