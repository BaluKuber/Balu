/// Module Name     :   Loan Admin
/// Screen Name     :   Asset Serial Number Entry
/// Created By      :   Swarna S
/// Created Date    :   19-12-2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.Globalization;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
using ApprovalMgtServicesReference;
using ORGSERVICE = ApprovalMgtServicesReference;

public partial class LoanAdmin_S3GLoanADAssetSerialNoEntry : ApplyThemeForProject
{
    #region [Intialization]
    Dictionary<string, string> dictParam = null;
    int intCAPDetailID;
    int intDEVDetailID;
    int intUserId;
    int intUserLevelID;
    int intCompanyId;
    int intErrCode;
    string strErrorMsg;
    string strMode;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanADAssetSerialNoEntry.aspx'";
    DataTable ObjDTRange = new DataTable();
    DataSet dsCAPDetails = new DataSet();
    UserInfo ObjUserInfo = new UserInfo();
    ORGSERVICE.ApprovalMgtServicesClient objApprovalMgtServicesClient;
    ApprovalMgtServices.S3G_ORG_RV_Matrix_HdrDataTable ObjS3G_CAPMasterListDataTable;
    public static LoanAdmin_S3GLoanADAssetSerialNoEntry obj_Page;
    string strDateFormat = string.Empty;

    LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objAssetSerialNo;

    PagingValues ObjPaging = new PagingValues();
    S3GSession ObjS3GSession = new S3GSession();

    #region  User Authorization
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    #endregion

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public int ProPageNumRW
    {
        get;
        set;
    }

    public int ProPageSizeRW
    {
        get;
        set;
    }
    #endregion [Intialization]

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriGetAssetGridDetails();
    }

    private void FunPriGetAssetGridDetails()
    {
        try
        {
            int intTotalRecords = 0;
            bool bIsNewRow = false;

            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            DataSet dtable = new DataSet();
            Procparam.Clear();
            Procparam.Add("@Cust_ID", ddlLesseeName.SelectedValue);

            if (ddlVendorName.SelectedValue == "0" && ddlVendorName.SelectedText != "")
                Procparam.Add("@Entity_ID", "-1");
            else if (ddlVendorName.SelectedValue != "0" && ddlVendorName.SelectedText == "")
                Procparam.Add("@Entity_ID", "0");
            else
                Procparam.Add("@Entity_ID", ddlVendorName.SelectedValue.ToString());

            if (ddlInvoiceNo.SelectedValue == "0" && ddlInvoiceNo.SelectedText != "")
                Procparam.Add("@Inv_ID", "-1");
            else
                Procparam.Add("@Inv_ID", ddlInvoiceNo.SelectedValue.ToString());

            if (txtRSNo.Text.Trim() != "")
            {
                Procparam.Add("@PANUM", txtRSNo.Text.Trim());
            }
            else
            {
                Procparam.Add("@PANUM", "");
            }

            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
          
            gvAssetSerialNo.BindGridView("S3G_LAD_ASSETSERIALNO", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            Cache["AssetSerialNo"] = gvAssetSerialNo.DataSource;
       
            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            if (bIsNewRow)
            {
                gvAssetSerialNo.Rows[0].Visible = false;
                btnSave.Enabled = false;
            }
            else
            {
                if (bCreate)
                    btnSave.Enabled = true;
            }

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        FunProPageLoad();
    }

    protected void FunProPageLoad()
    {
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        intUserLevelID = ObjUserInfo.ProUserLevelIdRW;

        #region  User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        #endregion

        obj_Page = this;

        ProPageNumRW = 1;
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        if (txtPageSize.Text != "")
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        else
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

        PageAssignValue obj = new PageAssignValue(this.AssignValue);
        ucCustomPaging.callback = obj;
        ucCustomPaging.ProPageNumRW = ProPageNumRW;
        ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

        if (!IsPostBack)
        {
            ucCustomPaging.Visible = false;

            if (!bCreate)
            {
                btnSave.Enabled = false;
            }
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLesseeNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCUSTOMERS", Procparam), false);
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendorNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETVENDORS", Procparam), false);
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetInvDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Cust_Id", obj_Page.ddlLesseeName.SelectedValue.ToString());
        Procparam.Add("@Entity_ID", obj_Page.ddlVendorName.SelectedValue.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETINVOICE", Procparam), false);
        return suggetions.ToArray();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlLesseeName.Clear();
        ddlVendorName.Clear();
        ddlInvoiceNo.Clear();
        txtRSNo.Text = "";
        gvAssetSerialNo.DataSource = null;
        gvAssetSerialNo.DataBind();
        ucCustomPaging.Visible = false;
    }

    protected void btnGo_Click(object sender, EventArgs e)
   {
       FunPriGetAssetGridDetails();
       //FunPriLoadAssetSerialNo();
   }
 
    private void FunPriLoadAssetSerialNo()
   {
       Dictionary<string, string> Procparam;
       Procparam = new Dictionary<string, string>();
       DataSet dt = new DataSet();
       Procparam.Clear();
       Procparam.Add("@Company_ID", intCompanyId.ToString());
       Procparam.Add("@Cust_ID", ddlLesseeName.SelectedValue);
       Procparam.Add("@Entity_ID", ddlVendorName.SelectedValue);
       Procparam.Add("@Inv_ID", ddlInvoiceNo.SelectedValue);
       if (txtRSNo.Text.Trim() != "")
       {
           Procparam.Add("@PANUM", txtRSNo.Text.Trim());
       }
       else
       {
           Procparam.Add("@PANUM", "");
       }

       dt = Utility.GetDataset("S3G_LAD_ASSETSERIALNO", Procparam);

       //gvCurrentInsurance.Visible = true;
       if (dt.Tables[0].Rows.Count != 0)
       {
           gvAssetSerialNo.DataSource = dt;
           ViewState["DT_AssetSerialNo"] = dt;
       }
       else
       {
           gvAssetSerialNo.DataSource = null;
           ViewState["DT_AssetSerialNo"] = null;
           //FunPriInitializeGrid();
       }
       gvAssetSerialNo.DataBind();
   }

    protected void gvAssetSerialNo_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkTo = (CheckBox)e.Row.FindControl("chkTo");
            Label lblSerialId = (Label)e.Row.FindControl("lblSerialId");
            TextBox txtSerialNo = (TextBox)e.Row.FindControl("txtSerialNo");
            TextBox txtDeliveryAddress = (TextBox)e.Row.FindControl("txtDeliveryAddress");

            DropDownList drpLocation = (DropDownList)e.Row.FindControl("drpLocation");
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtLocation = Utility.GetDefaultData("S3G_SYSAD_GetStateLookup", Procparam);
            Utility.FillDataTable(drpLocation, dtLocation, "Location_Category_ID", "LocationCat_Description");
            Label lblLocation = (Label)e.Row.FindControl("lblLocation");
            try
            {
                drpLocation.SelectedValue = lblLocation.Text.Trim();
            }
            catch (Exception ex)
            {
                drpLocation.SelectedIndex = 0;
            }
            chkTo.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + gvAssetSerialNo.ClientID + "','chkAll','chkTo');");            
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
            chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvAssetSerialNo.ClientID + "',this,'chkTo');");
        }
    }

    protected void chkFrom_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAsset = new DataTable();
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            Label lblRSNo = (Label)gvr.FindControl("lblRSNo");
            Label lblSNo = (Label)gvr.FindControl("lblSNo");
            Label lblInvoiceNo = (Label)gvr.FindControl("lblInvoiceNo");
            Label lblAssetCategory = (Label)gvr.FindControl("lblAssetCategory");
            Label lblAssetType = (Label)gvr.FindControl("lblAssetType");
            Label lblAssetSubType = (Label)gvr.FindControl("lblAssetSubType");

            string PANUM = lblRSNo.Text.Trim();
            string InvNo = lblInvoiceNo.Text.Trim();
            string ACatg = lblAssetCategory.Text.Trim();
            string AType = lblAssetType.Text.Trim();
            string ASType = lblAssetSubType.Text.Trim();
            string SerialNo = lblSNo.Text;
            //dtAsset = (DataTable)ViewState["DT_AssetSerialNo"];
         
            if (btn.Checked)
            {
                foreach (GridViewRow gvrow in gvAssetSerialNo.Rows)
                {
                    if (gvrow.RowType == DataControlRowType.DataRow)
                    {
                        if (((Label)gvrow.Cells[0].FindControl("lblSNo")).Text.ToString() == SerialNo.ToString())
                        {
                            ((CheckBox)gvrow.Cells[1].FindControl("chkFrom")).Checked = true;
                            ((CheckBox)gvrow.Cells[2].FindControl("chkTo")).Enabled = false;
                        }
                        else
                        {
                            ((CheckBox)gvrow.Cells[1].FindControl("chkFrom")).Enabled = false;
                        }
                    }
                }
            }
            else
            {
                foreach (GridViewRow gvrow in gvAssetSerialNo.Rows)
                {
                    if (gvrow.RowType == DataControlRowType.DataRow)
                    {
                        ((CheckBox)gvrow.Cells[1].FindControl("chkFrom")).Enabled = true;
                        ((CheckBox)gvrow.Cells[1].FindControl("chkFrom")).Checked = false;
                        ((CheckBox)gvrow.Cells[2].FindControl("chkTo")).Checked = false;
                        ((CheckBox)gvrow.Cells[2].FindControl("chkTo")).Enabled = true;
                    }
                }
            }
        }
        catch (Exception objException)
        {
            cvAsset.ErrorMessage = objException.Message;
            cvAsset.IsValid = false;
        }
    }

    protected void btnMove_Click(object sender, EventArgs e)
    {
        string DeliveryAddress = "";
        string Location = "";
        int nCntFrom = 0;
        foreach (GridViewRow gvrow in gvAssetSerialNo.Rows)
        {
            if (gvrow.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkFrom = (CheckBox)gvrow.FindControl("chkFrom");
                CheckBox chkTo = (CheckBox)gvrow.FindControl("chkTo");
                TextBox txtSerialNo = (TextBox)gvrow.FindControl("txtSerialNo");
                TextBox txtDeliveryAddress = (TextBox)gvrow.FindControl("txtDeliveryAddress");
                DropDownList drpLocation = (DropDownList)gvrow.FindControl("drpLocation");

                if (chkFrom.Checked == true)
                {
                    nCntFrom = nCntFrom + 1;
                    DeliveryAddress = txtDeliveryAddress.Text.Trim();
                    Location = drpLocation.SelectedValue;
                    break;
                }
            }
        }
        if (nCntFrom == 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Select atleast one record to copy from");
            return;
        }

        int nCnt = 0;
        foreach (GridViewRow gvrow in gvAssetSerialNo.Rows)
        {
            if (gvrow.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTo = (CheckBox)gvrow.FindControl("chkTo");
                if (chkTo.Checked == true)
                {
                    nCnt = nCnt + 1;
                }
            }
        }
        if (nCnt == 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Select atleast one destination to copy");
            return;
        }
        foreach (GridViewRow gvrow in gvAssetSerialNo.Rows)
        {
            if (gvrow.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTo = (CheckBox)gvrow.FindControl("chkTo");
                TextBox txtSerialNo = (TextBox)gvrow.FindControl("txtSerialNo");
                TextBox txtDeliveryAddress = (TextBox)gvrow.FindControl("txtDeliveryAddress");
                DropDownList drpLocation = (DropDownList)gvrow.FindControl("drpLocation");

                if (chkTo.Checked == true)
                {
                    txtDeliveryAddress.Text = DeliveryAddress;
                    drpLocation.SelectedValue = Location;
                }
            }
        }
        //ViewState["DT_AssetSerialNo"] = gvAssetSerialNo.DataSource;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Common/HomePage.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (gvAssetSerialNo.Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this.Page, "No records to save");
            return;
        }
    
        objAssetSerialNo = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            LoanAdminMgtServices.S3G_LAD_AssetSerialNoDataTable ObjS3G_LAD_AssetSerialNoDataTable = new LoanAdminMgtServices.S3G_LAD_AssetSerialNoDataTable();
            LoanAdminMgtServices.S3G_LAD_AssetSerialNoRow ObjS3G_LAD_AssetSerialNoRow;
            ObjS3G_LAD_AssetSerialNoRow = ObjS3G_LAD_AssetSerialNoDataTable.NewS3G_LAD_AssetSerialNoRow();
            ObjS3G_LAD_AssetSerialNoRow.CompanyID = intCompanyId.ToString();
            ObjS3G_LAD_AssetSerialNoRow.UserID = intUserId.ToString();
            ObjS3G_LAD_AssetSerialNoRow.CustID = ddlLesseeName.SelectedValue;
            ObjS3G_LAD_AssetSerialNoRow.EntityID = ddlVendorName.SelectedValue;
            ObjS3G_LAD_AssetSerialNoRow.InvID = ddlInvoiceNo.SelectedValue;
            ObjS3G_LAD_AssetSerialNoRow.XmlAssetSerialNo = gvAssetSerialNo.FunPubFormXml(true);
            ObjS3G_LAD_AssetSerialNoDataTable.AddS3G_LAD_AssetSerialNoRow(ObjS3G_LAD_AssetSerialNoRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_LAD_AssetSerialNoDataTable, SerMode);

            intErrCode = objAssetSerialNo.FunPubUpdateAssetSerialNo(SerMode, ObjDeliveryInsDataTable);

            if (intErrCode == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "Asset Serial Number Details added successfully");
                //Utility.FunShowAlertMsg(this.Page, strAlert);
                //Response.Redirect("~/LoanAdmin/S3GLoanADAssetSerialNoEntry.aspx");
               // return;
                //strAlert += @"\n\nWould you like to add one more record?";
                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageAdd + "}";
                strRedirectPageAdd = "";
            }
            if (intErrCode == 5)
            {
                strAlert = strAlert.Replace("__ALERT__","Asset Serial Number already exists. Please enter a different value");
                strRedirectPageAdd = "";
            }
            else if (intErrCode == 50)
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to save the record");
                return;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);
            lblErrorMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            if (objAssetSerialNo != null)
            {
                objAssetSerialNo.Close();
            }
        }
    }
}
