//Program summary
/// Module Name     :   Loan Admin
/// Screen Name     :   Vendor C Form
/// Created By      :   Swarna S
/// Created Date    :   27-Dec-2014
/// Purpose         :   To do data entry on Vendor C Form.
//Program summary
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
using AjaxControlToolkit;
using S3GBusEntity.LoanAdmin;

public partial class LoanAdmin_S3G_LAD_VendorCForm : ApplyThemeForProject
{
    #region [Intialization]
    Dictionary<string, string> dictParam = null;
    int intUserId;
    int intUserLevelID;
    int intCompanyId;
    int intErrCode;
    string strErrorMsg;
    string strMode;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3G_LAD_VendorCForm.aspx'";
    string _DateFormat = "dd/MM/yyyy";

    #region  User Authorization
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    #endregion

    DataTable ObjDTRange = new DataTable();
    UserInfo ObjUserInfo = new UserInfo();

    public static LoanAdmin_S3G_LAD_VendorCForm obj_Page;
    string strDateFormat = string.Empty;

    LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objVendorCForm;

    PagingValues ObjPaging = new PagingValues();
    S3GSession ObjS3GSession = new S3GSession();
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

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        if (rdbtnType.SelectedValue == "1")
        {
            FunLoadPruchaseDetails();
        }
        else
        {
            FunLoadRSDetails();
        }
    }

    #endregion [Intialization]

    #region [Page Load]

    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriPageLoad();
    }

    public void FunPriPageLoad()
    {
        S3GSession ObjS3GSession = null;
        ObjS3GSession = new S3GSession();
        try
        {
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }

        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        intUserLevelID = ObjUserInfo.ProUserLevelIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        obj_Page = this;

        ProPageNumRW = 1;
        if (rdbtnType.SelectedValue == "1")
        {
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;
        }
        else
        {
            TextBox txtPageSize = (TextBox)ucCustomPagingRS.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPagingRS.callback = obj;
            ucCustomPagingRS.ProPageNumRW = ProPageNumRW;
            ucCustomPagingRS.ProPageSizeRW = ProPageSizeRW;
        }
        if (!IsPostBack)
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetStateLookup", Procparam);
            Utility.FillDataTable(ddlComState, dtAddr, "Location_Category_ID", "LocationCat_Description");
            if (!bCreate)
            {
                btnSave.Enabled = false;
                btnMove.Enabled = false;
            }
        }
    }
    #endregion [Page Load]

    #region [Generic]
    [System.Web.Services.WebMethod]
    public static string[] GetRentalScheduleNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETRENTALSCHEDULE", Procparam), false);
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetRentalScheduleNoForRS(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETRENTALSCHEDULERS", Procparam), false);
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerName(String prefixText, int count)
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
    public static string[] GetVendorName(String prefixText, int count)
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

    protected void rdbtnType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnType.SelectedValue == "1")
        {
            pnlPurchase.Visible = true;
            pnlRentalSchedule.Visible = false;
            ddlRRSNo.Clear();
            ddlCustomerName.Clear();
            TextBox1.Text = "";
            TextBox2.Text = "";
            gvVendorCFormPurchase.DataSource = null;
            gvVendorCFormPurchase.DataBind();
            //FunPriLoadPurchase();
        }
        else if (rdbtnType.SelectedValue == "2")
        {
            pnlPurchase.Visible = false;
            pnlRentalSchedule.Visible = true;
            ddlVendName.Clear();
            ddlStatus.SelectedIndex = 0;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            gvVendorCFRS.DataSource = null;
            gvVendorCFRS.DataBind();
            //FunPriLoadRentalSchedule();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        objVendorCForm = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            if (rdbtnType.SelectedValue == "1")
            {
                if (gvVendorCFormPurchase.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "No records to save");
                    return;
                }
            }
            else if (rdbtnType.SelectedValue == "2")
            {
                if (gvVendorCFRS.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "No records to save");
                    return;
                }
            }




            LoanAdminMgtServices.S3G_LAD_VENDORCDataTable ObjS3G_LAD_VENDORCDataTable = new LoanAdminMgtServices.S3G_LAD_VENDORCDataTable();
            LoanAdminMgtServices.S3G_LAD_VENDORCRow ObjS3G_LAD_VENDORCRow;
            ObjS3G_LAD_VENDORCRow = ObjS3G_LAD_VENDORCDataTable.NewS3G_LAD_VENDORCRow();

            if (gvVendorCFormPurchase.Rows.Count != 0)
            {
                ObjS3G_LAD_VENDORCRow.XMLPurchaseDetails = gvVendorCFormPurchase.FunPubFormXml(true);
            }
            else
            {
                ObjS3G_LAD_VENDORCRow.XMLPurchaseDetails = "";
            }

            if (gvVendorCFRS.Rows.Count != 0)
            {
                ObjS3G_LAD_VENDORCRow.XMLRSDetails = gvVendorCFRS.FunPubFormXml(true);
            }
            else
            {
                ObjS3G_LAD_VENDORCRow.XMLRSDetails = "";
            }
                        
            ObjS3G_LAD_VENDORCRow.Company_ID = intCompanyId.ToString();
            ObjS3G_LAD_VENDORCRow.User_ID = intUserId.ToString();
            ObjS3G_LAD_VENDORCRow.CType = rdbtnType.SelectedValue.ToString();
            
            ObjS3G_LAD_VENDORCDataTable.AddS3G_LAD_VENDORCRow(ObjS3G_LAD_VENDORCRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_LAD_VENDORCDataTable, SerMode);

            intErrCode = objVendorCForm.FunPubUpdateVendorC(SerMode, ObjDeliveryInsDataTable);

            if (intErrCode == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "Vendor C Form details added successfully");
                //strAlert += @"\n\nWould you like to add one more record?";
                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageAdd + "}";
             }
            if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Error in adding Purchase Details");
                //strAlert += @"\n\nWould you like to add one more record?";
                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageAdd + "}";
                strRedirectPageAdd = "";
            }
            if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Error in adding RS Details");
                //strAlert += @"\n\nWould you like to add one more record?";
                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageAdd + "}";
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
            lblErrorMessage.Text = ex.ToString();
        }
        finally
        {
            if (objVendorCForm != null)
            {
                objVendorCForm.Close();
            }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        //ddlRRSNo.Clear();
        //ddlVendName.Clear();
        //ddlPRSNo.Clear();
        pnlPurchase.Visible = false;
        pnlRentalSchedule.Visible = false;
        lblErrorMessage.Text = "";
        foreach (ListItem rdbtnlist in rdbtnType.Items)
        {
            rdbtnlist.Selected = false;
        }
    }

    #endregion [Generic]

    #region [Purchase]

    protected void btnPurchaseSearch_Click(object sender, EventArgs e)
    {
        FunLoadPruchaseDetails();
    }

    protected void FunLoadPruchaseDetails()
    {
        try
        {
            int intTotalRecords = 0;
            bool bIsNewRow = false;

            Dictionary<string, string> Procparam = null;
            Procparam = new Dictionary<string, string>();
           
            if (ddlVendName.SelectedValue == "0" && ddlVendName.SelectedText != "")
                Procparam.Add("@VENDOR_ID", "-1");
            else if (ddlVendName.SelectedValue != "0")
                Procparam.Add("@VENDOR_ID", ddlVendName.SelectedValue.ToString());

            if (ddlComState.SelectedValue == "0" && ddlComState.Text != "" && ddlComState.Text != "0")
                Procparam.Add("@Delivery_State_ID", "-1");
            else if (ddlComState.SelectedValue != "0")
                Procparam.Add("@Delivery_State_ID", ddlComState.SelectedValue.ToString());
            if (txtFromDate.Text != "")
            {
                Procparam.Add("@From_Date", Convert.ToDateTime(Utility.StringToDate(txtFromDate.Text)).ToString());
            }
            else
            {
                Procparam.Add("@From_Date", "");
            }

            if (txtToDate.Text != "")
            {
                Procparam.Add("@To_Date", Convert.ToDateTime(Utility.StringToDate(txtToDate.Text)).ToString());
            }
            else
            {
                Procparam.Add("@To_Date", "");
            }

            Procparam.Add("@Status", ddlStatus.SelectedValue);

            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            gvVendorCFormPurchase.BindGridView("S3G_GETPURCHASEDETAILS", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            Cache["PurchaseDetails"] = gvVendorCFormPurchase.DataSource;

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            if (bIsNewRow)
            {
                gvVendorCFormPurchase.Rows[0].Visible = false;
                btnSave.Enabled = false;
                btnMove.Enabled = false;
            }
            else
            {
                if (bCreate)
                {
                    btnSave.Enabled = true;
                    btnMove.Enabled = true;
                }
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

    protected void chkFrom_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAsset = new DataTable();
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //Label lblRSNo = (Label)gvr.FindControl("lblRSNo");
            Label lblPSlno = (Label)gvr.FindControl("lblPSlno");

            //string PANUM = lblRSNo.Text.Trim();
            //string InvNo = lblInvoiceNo.Text.Trim();
            //string ACatg = lblAssetCategory.Text.Trim();
            //string AType = lblAssetType.Text.Trim();
            //string ASType = lblAssetSubType.Text.Trim();
            string SerialNo = lblPSlno.Text;
            //dtAsset = (DataTable)ViewState["DT_AssetSerialNo"];

            if (rdbtnType.SelectedValue == "1")
            {
                if (btn.Checked)
                {
                    foreach (GridViewRow gvrow in gvVendorCFormPurchase.Rows)
                    {
                        if (gvrow.RowType == DataControlRowType.DataRow)
                        {
                            if (((Label)gvrow.Cells[0].FindControl("lblPSlno")).Text.ToString() == SerialNo.ToString())
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
                    foreach (GridViewRow gvrow in gvVendorCFormPurchase.Rows)
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
            else
            {
                if (btn.Checked)
                {
                    foreach (GridViewRow gvrow in gvVendorCFRS.Rows)
                    {
                        if (gvrow.RowType == DataControlRowType.DataRow)
                        {
                            if (((Label)gvrow.Cells[0].FindControl("lblPSlno")).Text.ToString() == SerialNo.ToString())
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
                    foreach (GridViewRow gvrow in gvVendorCFRS.Rows)
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
        }
        catch (Exception objException)
        {
            cvVendor.ErrorMessage = objException.Message;
            cvVendor.IsValid = false;
        }
    }

    protected void btnMove_Click(object sender, EventArgs e)
    {
        string CFormNo = "";
        string IssueDate = "";
        string ValidTillDate = "";
        string Remarks = "";
        int nCntFrom = 0;
        if (rdbtnType.SelectedValue == "1")
        {
            foreach (GridViewRow gvrow in gvVendorCFormPurchase.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkFrom = (CheckBox)gvrow.FindControl("chkFrom");
                    CheckBox chkTo = (CheckBox)gvrow.FindControl("chkTo");
                    TextBox txtECForm = (TextBox)gvrow.FindControl("txtECForm");
                    TextBox txtEPIssueDate = (TextBox)gvrow.FindControl("txtEPIssueDate");
                    TextBox txtEPValidTillDate = (TextBox)gvrow.FindControl("txtEPValidTillDate");
                    TextBox txtERemarks = (TextBox)gvrow.FindControl("txtERemarks");
                    if (chkFrom.Checked == true)
                    {
                        nCntFrom = nCntFrom + 1;
                        CFormNo = txtECForm.Text.Trim();
                        IssueDate = txtEPIssueDate.Text.Trim();
                        ValidTillDate = txtEPValidTillDate.Text.Trim();
                        Remarks = txtERemarks.Text.Trim();
                    }
                }
            }
            if (nCntFrom == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record to copy from");
                return;
            }

            int nCnt = 0;
            foreach (GridViewRow gvrow in gvVendorCFormPurchase.Rows)
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

            foreach (GridViewRow gvrow in gvVendorCFormPurchase.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkTo = (CheckBox)gvrow.FindControl("chkTo");
                    TextBox txtECForm = (TextBox)gvrow.FindControl("txtECForm");
                    TextBox txtEPIssueDate = (TextBox)gvrow.FindControl("txtEPIssueDate");
                    TextBox txtEPValidTillDate = (TextBox)gvrow.FindControl("txtEPValidTillDate");
                    TextBox txtERemarks = (TextBox)gvrow.FindControl("txtERemarks");

                    if (chkTo.Checked == true)
                    {
                        txtECForm.Text = CFormNo;
                        txtEPIssueDate.Text = IssueDate;
                        txtEPValidTillDate.Text = ValidTillDate;
                        txtERemarks.Text = Remarks;
                    }
                }
            }
        }
        else
        {
            foreach (GridViewRow gvrow in gvVendorCFRS.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkFrom = (CheckBox)gvrow.FindControl("chkFrom");
                    CheckBox chkTo = (CheckBox)gvrow.FindControl("chkTo");
                    TextBox txtECForm = (TextBox)gvrow.FindControl("txtECForm");
                    TextBox txtEPIssueDate = (TextBox)gvrow.FindControl("txtEPIssueDate");
                    TextBox txtEPValidTillDate = (TextBox)gvrow.FindControl("txtEPValidTillDate");
                    TextBox txtERemarks = (TextBox)gvrow.FindControl("txtERemarks");
                    if (chkFrom.Checked == true)
                    {
                        nCntFrom = nCntFrom + 1;
                        CFormNo = txtECForm.Text.Trim();
                        IssueDate = txtEPIssueDate.Text.Trim();
                        ValidTillDate = txtEPValidTillDate.Text.Trim();
                        Remarks = txtERemarks.Text.Trim();
                    }
                }
            }
            if (nCntFrom == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record to copy from");
                return;
            }

            int nCnt = 0;
            foreach (GridViewRow gvrow in gvVendorCFRS.Rows)
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

            foreach (GridViewRow gvrow in gvVendorCFRS.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkTo = (CheckBox)gvrow.FindControl("chkTo");
                    TextBox txtECForm = (TextBox)gvrow.FindControl("txtECForm");
                    TextBox txtEPIssueDate = (TextBox)gvrow.FindControl("txtEPIssueDate");
                    TextBox txtEPValidTillDate = (TextBox)gvrow.FindControl("txtEPValidTillDate");
                    TextBox txtERemarks = (TextBox)gvrow.FindControl("txtERemarks");

                    if (chkTo.Checked == true)
                    {
                        txtECForm.Text = CFormNo;
                        txtEPIssueDate.Text = IssueDate;
                        txtEPValidTillDate.Text = ValidTillDate;
                        txtERemarks.Text = Remarks;
                    }
                }
            }
        }

        //ViewState["DT_AssetSerialNo"] = gvAssetSerialNo.DataSource;
    }

    protected void FunPriLoadPurchase()
    {
        DataTable dtPurchase = new DataTable();
        DataRow drPurchase;
        dtPurchase.Columns.Add("PANUM");
        dtPurchase.Columns.Add("PA_SA_REF_ID");
        dtPurchase.Columns.Add("ENTITY_NAME");
        dtPurchase.Columns.Add("ENTITY_ID");
        dtPurchase.Columns.Add("INVOICE_DATE");
        dtPurchase.Columns.Add("INVOICE_ID");
        dtPurchase.Columns.Add("INVOICE_NO");
        dtPurchase.Columns.Add("INVOICE_AMT");
        dtPurchase.Columns.Add("C_FORM_NO");
        dtPurchase.Columns.Add("ISSUE_DATE");
        dtPurchase.Columns.Add("VALID_TLL_DATE");
        dtPurchase.Columns.Add("REMARKS");
        dtPurchase.Columns.Add("VENDORC_ID");
        dtPurchase.Columns.Add("RowNumber");
        drPurchase = dtPurchase.NewRow();
        dtPurchase.Rows.Add(drPurchase);

        gvVendorCFormPurchase.DataSource = dtPurchase;
        gvVendorCFormPurchase.DataBind();
        gvVendorCFormPurchase.Rows[0].Visible = false;
        ViewState["PurchaseDetails"] = dtPurchase.Clone();
    }

    protected void ddlFVendorNo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Label lblFInvoiceDate = (Label)gvVendorCFormPurchase.FooterRow.FindControl("lblFInvoiceDate");
        Label lblFInvoiceAmt = (Label)gvVendorCFormPurchase.FooterRow.FindControl("lblFInvoiceAmt");
        DropDownList ddlFVendorNo = (DropDownList)gvVendorCFormPurchase.FooterRow.FindControl("ddlFVendorNo");
        UserControls_S3GAutoSuggest ddlFRSNo = (UserControls_S3GAutoSuggest)gvVendorCFormPurchase.FooterRow.FindControl("ddlFRSNo");
        
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@InvNo", ddlFVendorNo.SelectedValue);
        Procparam.Add("@PA_SA_REF_ID", ddlFRSNo.SelectedValue);
        DataSet ds = Utility.GetDataset("S3G_LAD_GetInvDateAmount", Procparam);
        if (ds.Tables[0].Rows.Count>0)
        {
             lblFInvoiceAmt.Text = ds.Tables[0].Rows[0]["INVOICE_AMOUNT"].ToString();
             lblFInvoiceDate.Text = ds.Tables[0].Rows[0]["INVOICE_DATE"].ToString();
        }
        else
        {
            lblFInvoiceDate.Text = "";
            lblFInvoiceAmt.Text = "";
        }
    }

    protected void ddlFVendor_OnItemSelected(object sender, EventArgs e)
    {
        UserControls_S3GAutoSuggest ddlFVendor = (UserControls_S3GAutoSuggest)gvVendorCFormPurchase.FooterRow.FindControl("ddlFVendor");
        UserControls_S3GAutoSuggest ddlFRSNo = (UserControls_S3GAutoSuggest)gvVendorCFormPurchase.FooterRow.FindControl("ddlFRSNo");
        DropDownList ddlFVendorNo = (DropDownList)gvVendorCFormPurchase.FooterRow.FindControl("ddlFVendorNo");
        
        Dictionary<string, string> Procparam = null;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@PA_SA_REF_ID", ddlFRSNo.SelectedValue);
        Procparam.Add("@ENTITY_ID", ddlFVendor.SelectedValue);
        ddlFVendorNo.BindDataTable("S3G_GETINVOICEFORENTITY", Procparam, new string[] { "INVOICE_NO", "INVOICE_NO" });
    }

    protected void ddlFRSNo_OnItemSelected(object sender, EventArgs e)
    {
        UserControls_S3GAutoSuggest ddlFVendor = (UserControls_S3GAutoSuggest)gvVendorCFormPurchase.FooterRow.FindControl("ddlFVendor");
        UserControls_S3GAutoSuggest ddlFRSNo = (UserControls_S3GAutoSuggest)gvVendorCFormPurchase.FooterRow.FindControl("ddlFRSNo");
        DropDownList ddlFVendorNo = (DropDownList)gvVendorCFormPurchase.FooterRow.FindControl("ddlFVendorNo");

        Dictionary<string, string> Procparam = null;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@PA_SA_REF_ID", ddlFRSNo.SelectedValue);
        Procparam.Add("@ENTITY_ID", ddlFVendor.SelectedValue);
        ddlFVendorNo.BindDataTable("S3G_GETINVOICEFORENTITY", Procparam, new string[] { "INVOICE_NO","INVOICE_NO" });
    }

    protected void txtFPIssueDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int intErrCount = 0;
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtFPIssueDate = (TextBox)grv.FindControl("txtFPIssueDate") as TextBox;
            TextBox txtFPValidTillDate = (TextBox)grv.FindControl("txtFPValidTillDate") as TextBox;
            if (txtFPIssueDate.Text != String.Empty && txtFPValidTillDate.Text != String.Empty)
            {
                intErrCount = Utility.CompareDates(txtFPIssueDate.Text, txtFPValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtFPValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Issue date");
                    txtFPValidTillDate.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void txtEPIssueDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int intErrCount = 0;
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtEPIssueDate = (TextBox)grv.FindControl("txtEPIssueDate") as TextBox;
            TextBox txtEPValidTillDate = (TextBox)grv.FindControl("txtEPValidTillDate") as TextBox;
            if (txtEPIssueDate.Text != String.Empty && txtEPValidTillDate.Text != String.Empty)
            {
                intErrCount = Utility.CompareDates(txtEPIssueDate.Text, txtEPValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtEPValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Issue date");
                    txtEPValidTillDate.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void txtFPValidTillDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int intErrCount = 0;
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtFPValidTillDate = (TextBox)grv.FindControl("txtFPValidTillDate") as TextBox;
            TextBox txtFPIssueDate = (TextBox)grv.FindControl("txtFPIssueDate") as TextBox;
            if (txtFPIssueDate.Text != String.Empty && txtFPValidTillDate.Text != String.Empty)
            {
                string CurrentDate = DateTime.Now.ToShortDateString();
                CurrentDate = DateTime.Parse(CurrentDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
                intErrCount = Utility.CompareDates(CurrentDate, txtFPValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtFPValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Current date");
                    txtFPValidTillDate.Focus();
                    return;
                }
                intErrCount = 0;

                intErrCount = Utility.CompareDates(txtFPIssueDate.Text, txtFPValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtFPValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Issue date");
                    txtFPValidTillDate.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void txtEPValidTillDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int intErrCount = 0;
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtEPValidTillDate = (TextBox)grv.FindControl("txtEPValidTillDate") as TextBox;
            TextBox txtEPIssueDate = (TextBox)grv.FindControl("txtEPIssueDate") as TextBox;
            if (txtEPIssueDate.Text != String.Empty && txtEPValidTillDate.Text != String.Empty)
            {
                string CurrentDate = DateTime.Now.ToShortDateString();
                CurrentDate = DateTime.Parse(CurrentDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
                intErrCount = Utility.CompareDates(CurrentDate, txtEPValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtEPValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Current date");
                    txtEPValidTillDate.Focus();
                    return;
                }
                intErrCount = 0;

                intErrCount = Utility.CompareDates(txtEPIssueDate.Text, txtEPValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtEPValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Issue date");
                    txtEPValidTillDate.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void gvVendorCFormPurchase_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        DropDownList ddlFVendorNo = (DropDownList)gvVendorCFormPurchase.FooterRow.FindControl("ddlFVendorNo");
        Label lblFInvoiceDate = (Label)gvVendorCFormPurchase.FooterRow.FindControl("lblFInvoiceDate");
        Label lblSlNo = (Label)gvVendorCFormPurchase.FooterRow.FindControl("lblSlNo");
        Label lblFInvoiceAmt = (Label)gvVendorCFormPurchase.FooterRow.FindControl("lblFInvoiceAmt");
        UserControls_S3GAutoSuggest ddlFRSNo = (UserControls_S3GAutoSuggest)gvVendorCFormPurchase.FooterRow.FindControl("ddlFRSNo");
        UserControls_S3GAutoSuggest ddlFVendor = (UserControls_S3GAutoSuggest)gvVendorCFormPurchase.FooterRow.FindControl("ddlFVendor");
        TextBox txtFCForm = (TextBox)gvVendorCFormPurchase.FooterRow.FindControl("txtFCForm");
        TextBox txtFPIssueDate = (TextBox)gvVendorCFormPurchase.FooterRow.FindControl("txtFPIssueDate");
        TextBox txtFPValidTillDate = (TextBox)gvVendorCFormPurchase.FooterRow.FindControl("txtFPValidTillDate");
        TextBox txtFRemarks = (TextBox)gvVendorCFormPurchase.FooterRow.FindControl("txtFRemarks");
        Button btnAdd = (Button)gvVendorCFormPurchase.FooterRow.FindControl("btnAdd");
        DateTime IssueDate;
        DateTime ValidTillDate;
        if (e.CommandName == "AddNew")
        {
            DataTable dtCAPApproval = (DataTable)ViewState["PurchaseDetails"];
            DataTable dtapprove = new DataTable();
            DataRow dRow;

            dRow = dtCAPApproval.NewRow();
            dRow["PANUM"] = ddlFRSNo.SelectedText;
            dRow["VENDORC_ID"] = "0";
            dRow["PA_SA_REF_ID"] = ddlFRSNo.SelectedValue;
            dRow["ENTITY_ID"] = ddlFVendor.SelectedValue;
            dRow["ENTITY_NAME"] = ddlFVendor.SelectedText;
            dRow["INVOICE_NO"] = ddlFVendorNo.SelectedItem.ToString();
            dRow["INVOICE_DATE"] = DateTime.Parse(Utility.StringToDate(lblFInvoiceDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
            dRow["INVOICE_AMT"] = lblFInvoiceAmt.Text.Trim();
            dRow["C_FORM_NO"] = txtFCForm.Text.Trim();

            IssueDate = Utility.StringToDate(txtFPIssueDate.Text.Trim());
            IssueDate = IssueDate.Date;
            dRow["ISSUE_DATE"] = DateTime.Parse(Utility.StringToDate(txtFPIssueDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
            ValidTillDate = Utility.StringToDate(txtFPValidTillDate.Text.Trim());
            ValidTillDate = ValidTillDate.Date;
            dRow["VALID_TLL_DATE"] = DateTime.Parse(Utility.StringToDate(txtFPValidTillDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
            dRow["REMARKS"] = txtFRemarks.Text.Trim();

            dtCAPApproval.Rows.Add(dRow);
            gvVendorCFormPurchase.DataSource = dtCAPApproval;
            ViewState["PurchaseDetails"] = dtCAPApproval;
            gvVendorCFormPurchase.DataBind();
        }
    }

    protected void gvVendorCFormPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
    }

    protected void gvVendorCFormPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            gvVendorCFormPurchase.EditIndex = e.NewEditIndex;
            int intRowId = e.NewEditIndex;

            DataTable dtWBT = (DataTable)ViewState["PurchaseDetails"];
            gvVendorCFormPurchase.DataSource = dtWBT;
            gvVendorCFormPurchase.DataBind();

            GridViewRow grvRow = gvVendorCFormPurchase.Rows[intRowId];

            CalendarExtender CEEPIssueDate = (CalendarExtender)grvRow.FindControl("CEEPIssueDate");
            CEEPIssueDate.Format = strDateFormat;

            CalendarExtender CEEPValidTill = (CalendarExtender)grvRow.FindControl("CEEPValidTill");
            CEEPValidTill.Format = strDateFormat;

            TextBox txtEPIssueDate = (TextBox)grvRow.FindControl("txtEPIssueDate");
            txtEPIssueDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEPIssueDate.ClientID + "','" + strDateFormat + "',null,null);");

            TextBox txtEPValidTillDate = (TextBox)grvRow.FindControl("txtEPValidTillDate");
            txtEPValidTillDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEPValidTillDate.ClientID + "','" + strDateFormat + "',null,null);");
            gvVendorCFormPurchase.FooterRow.Visible = false;
         
          }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvVendorCFormPurchase_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            DataTable dtPurchaseUpdate = new DataTable();
            int intRowId = 0;
            intRowId = e.RowIndex;
            DateTime IssueDate;
            DateTime ValidTillDate;
            GridViewRow grvRow = gvVendorCFormPurchase.Rows[intRowId];
            TextBox txtECForm = (TextBox)grvRow.FindControl("txtECForm");
            TextBox txtEPIssueDate = (TextBox)grvRow.FindControl("txtEPIssueDate");
            TextBox txtEPValidTillDate = (TextBox)grvRow.FindControl("txtEPValidTillDate");
            TextBox txtERemarks = (TextBox)grvRow.FindControl("txtERemarks");
            LinkButton btnGridSave = (LinkButton)grvRow.FindControl("lnkUpdate");

            dtPurchaseUpdate = (DataTable)ViewState["PurchaseDetails"];
            
            lblErrorMessage.Text = string.Empty;
            dtPurchaseUpdate.Rows[intRowId]["C_FORM_NO"] = txtECForm.Text.Trim();
            IssueDate = Utility.StringToDate(txtEPIssueDate.Text.Trim());
            IssueDate = IssueDate.Date;
            dtPurchaseUpdate.Rows[intRowId]["ISSUE_DATE"] = DateTime.Parse(Utility.StringToDate(txtEPIssueDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
            ValidTillDate = Utility.StringToDate(txtEPValidTillDate.Text.Trim());
            ValidTillDate = ValidTillDate.Date;
            dtPurchaseUpdate.Rows[intRowId]["VALID_TLL_DATE"] = DateTime.Parse(Utility.StringToDate(txtEPValidTillDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
            dtPurchaseUpdate.Rows[intRowId]["REMARKS"] = txtERemarks.Text.Trim();
            gvVendorCFormPurchase.EditIndex = -1;

            dtPurchaseUpdate = (DataTable)ViewState["PurchaseDetails"];
            gvVendorCFormPurchase.DataSource = dtPurchaseUpdate;
            gvVendorCFormPurchase.DataBind();

            gvVendorCFormPurchase.FooterRow.Visible = true;
            ViewState["PurchaseDetails"] = dtPurchaseUpdate;
           
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvVendorCFormPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtPurchaseDelete = new DataTable();
        gvVendorCFormPurchase.EditIndex = -1;
        dtPurchaseDelete = (DataTable)ViewState["PurchaseDetails"];
        gvVendorCFormPurchase.DataSource = dtPurchaseDelete;
        gvVendorCFormPurchase.DataBind();
        if (dtPurchaseDelete.Rows.Count == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "There should be atleast one row in the grid");
                return;
            }
        dtPurchaseDelete.Rows.RemoveAt(e.RowIndex);
        if (dtPurchaseDelete.Rows.Count == 0)
        {
            gvVendorCFormPurchase.DataSource = dtPurchaseDelete;
            gvVendorCFormPurchase.DataBind();
        }
        else
        {
            //Reset Row_ID in DataTable
            for (int intCount = 0; intCount < dtPurchaseDelete.Rows.Count; intCount++)
            {
                dtPurchaseDelete.Rows[intCount]["ROWNUMBER"] = (intCount + 1).ToString();
            }
            gvVendorCFormPurchase.DataSource = dtPurchaseDelete;
            gvVendorCFormPurchase.DataBind();
        }
        ViewState["PurchaseDetails"] = dtPurchaseDelete;
        gvVendorCFormPurchase.FooterRow.Visible = true;
    }

    protected void gvVendorCFormPurchase_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            DataTable dtPurchaseCancel = new DataTable();
            gvVendorCFormPurchase.EditIndex = -1;
            dtPurchaseCancel = (DataTable)ViewState["PurchaseDetails"];
            gvVendorCFormPurchase.DataSource = dtPurchaseCancel;
            gvVendorCFormPurchase.DataBind();
            gvVendorCFormPurchase.FooterRow.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion [Purchase]

    #region [RS]

    protected void btnRSSearch_Click(object sender, EventArgs e)
    {
        FunLoadRSDetails();
    }

    protected void FunLoadRSDetails()
    {
        try
        {
            int intTotalRecords = 0;
            bool bIsNewRow = false;

            Dictionary<string, string> Procparam = null;
            Procparam = new Dictionary<string, string>();
           
            if (ddlCustomerName.SelectedValue == "0" && ddlCustomerName.SelectedText != "")
                Procparam.Add("@Cust_ID", "-1");
            else if (ddlCustomerName.SelectedValue != "0" && ddlCustomerName.SelectedText != "")//By Siva.K on 22JUn2015 existing data cleared and search again then data will not refresh
                Procparam.Add("@Cust_ID", ddlCustomerName.SelectedValue.ToString());

            if (ddlRRSNo.SelectedValue == "0" && ddlRRSNo.SelectedText != "")
                Procparam.Add("@PA_SA_REF_ID", "-1");
            else if (ddlComState.SelectedValue != "0")
                Procparam.Add("@PA_SA_REF_ID", ddlRRSNo.SelectedValue.ToString());

            if (TextBox1.Text != "")
            {
                Procparam.Add("@From_Date", Convert.ToDateTime(Utility.StringToDate(TextBox1.Text)).ToString());
            }
            else
            {
                Procparam.Add("@From_Date", "");
            }
            if (TextBox2.Text != "")
            {
                Procparam.Add("@To_Date", Convert.ToDateTime(Utility.StringToDate(TextBox2.Text)).ToString());
            }
            else
            {
                Procparam.Add("@To_Date", "");
            }
            //Procparam.Add("@Status", ddlStatus.SelectedValue);

            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            gvVendorCFRS.BindGridView("S3G_GETRSDETAILS", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            Cache["RSDetails"] = gvVendorCFRS.DataSource;

            ucCustomPagingRS.Visible = true;
            ucCustomPagingRS.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPagingRS.setPageSize(ProPageSizeRW);

            if (bIsNewRow)
            {
                gvVendorCFRS.Rows[0].Visible = false;
                btnSave.Enabled = false;
                btnMove.Enabled = false;
            }
            else
            {
                if (bCreate)
                {
                    btnSave.Enabled = true;
                    btnMove.Enabled = true;
                }
            }
            ucCustomPagingRS.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPagingRS.setPageSize(ProPageSizeRW);

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

    protected 
        
    void FunPriLoadRentalSchedule()
    {
        DataTable dtRS = new DataTable();
        DataRow drRS;
        dtRS.Columns.Add("CUSTOMER_ID");
        dtRS.Columns.Add("PANUM");
        dtRS.Columns.Add("PA_SA_REF_ID");
        dtRS.Columns.Add("CUSTOMER_NAME");
        dtRS.Columns.Add("C_FORM_NO");
        dtRS.Columns.Add("ISSUE_DATE");
        dtRS.Columns.Add("VALID_TLL_DATE");
        dtRS.Columns.Add("REMARKS");
        dtRS.Columns.Add("VENDORC_ID");
        dtRS.Columns.Add("RowNumber");
        drRS = dtRS.NewRow();
        dtRS.Rows.Add(drRS);

        gvVendorCFRS.DataSource = dtRS;
        gvVendorCFRS.DataBind();
        gvVendorCFRS.Rows[0].Visible = false;
        ViewState["RSDetails"] = dtRS.Clone();
    }
    
    protected void ddlFRSNoRS_OnItemSelected(object sender, EventArgs e)
    {
        UserControls_S3GAutoSuggest ddlFRSNoRS = (UserControls_S3GAutoSuggest)gvVendorCFRS.FooterRow.FindControl("ddlFRSNoRS");
        Label lblICustomerName = (Label)gvVendorCFRS.FooterRow.FindControl("lblICustomerName");
        HiddenField hdnCustomerValue = (HiddenField)gvVendorCFRS.FooterRow.FindControl("hdnCustomerValue");

        Dictionary<string, string> Procparam = null;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@PA_SA_REF_ID", ddlFRSNoRS.SelectedValue);

        DataSet ds = Utility.GetDataset("S3G_LAD_GETCUSTOMERFROMRS", Procparam);
        if (ds != null)
        {
            hdnCustomerValue.Value = ds.Tables[0].Rows[0]["CUSTOMER_ID"].ToString();
            lblICustomerName.Text = ds.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString();
        }
        else
        {
            hdnCustomerValue.Value  = "";
            lblICustomerName.Text = "";
        }
    }

    protected void txtFRSIssueDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int intErrCount = 0;
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtFRSIssueDate = (TextBox)grv.FindControl("txtFRSIssueDate") as TextBox;
            TextBox txtFRSValidTillDate = (TextBox)grv.FindControl("txtFRSValidTillDate") as TextBox;
            if (txtFRSIssueDate.Text != String.Empty && txtFRSValidTillDate.Text != String.Empty)
            {
                intErrCount = Utility.CompareDates(txtFRSIssueDate.Text, txtFRSValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtFRSValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Issue date");
                    txtFRSValidTillDate.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void txtERSIssueDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int intErrCount = 0;
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtERSIssueDate = (TextBox)grv.FindControl("txtERSIssueDate") as TextBox;
            TextBox txtERSValidTillDate = (TextBox)grv.FindControl("txtERSValidTillDate") as TextBox;
            if (txtERSIssueDate.Text != String.Empty && txtERSValidTillDate.Text != String.Empty)
            {
                intErrCount = Utility.CompareDates(txtERSIssueDate.Text, txtERSValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtERSValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Issue date");
                    txtERSValidTillDate.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void txtFRSValidTillDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int intErrCount = 0;
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtFRSIssueDate = (TextBox)grv.FindControl("txtFRSIssueDate") as TextBox;
            TextBox txtFRSValidTillDate = (TextBox)grv.FindControl("txtFRSValidTillDate") as TextBox;

            if (txtFRSIssueDate.Text != String.Empty && txtFRSValidTillDate.Text != String.Empty)
            {
                string CurrentDate = DateTime.Now.ToShortDateString();
                CurrentDate = DateTime.Parse(CurrentDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
                intErrCount = Utility.CompareDates(CurrentDate, txtFRSValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtFRSValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Current date");
                    txtFRSValidTillDate.Focus();
                    return;
                }
                intErrCount = 0;
                intErrCount = Utility.CompareDates(txtFRSIssueDate.Text, txtFRSValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtFRSValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Issue date");
                    txtFRSValidTillDate.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void txtERSValidTillDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int intErrCount = 0;
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtERSIssueDate = (TextBox)grv.FindControl("txtERSIssueDate") as TextBox;
            TextBox txtERSValidTillDate = (TextBox)grv.FindControl("txtERSValidTillDate") as TextBox;
            if (txtERSIssueDate.Text != String.Empty && txtERSValidTillDate.Text != String.Empty)
            {
                string CurrentDate = DateTime.Now.ToShortDateString();
                CurrentDate = DateTime.Parse(CurrentDate.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
                intErrCount = Utility.CompareDates(CurrentDate, txtERSValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtERSValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Current date");
                    txtERSValidTillDate.Focus();
                    return;
                }
                intErrCount = 0;

                intErrCount = Utility.CompareDates(txtERSIssueDate.Text, txtERSValidTillDate.Text);
                if (intErrCount == -1)
                {
                    txtERSValidTillDate.Text = String.Empty;
                    Utility.FunShowAlertMsg(this, "Valid Date Should Be Greater Than Issue date");
                    txtERSValidTillDate.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    //#region [Old Grid Info]
    //protected void gvVendorCFRS_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    //{
    //    Label lblICustomerName = (Label)gvVendorCFRS.FooterRow.FindControl("lblICustomerName");
    //    UserControls_S3GAutoSuggest ddlFRSNoRS = (UserControls_S3GAutoSuggest)gvVendorCFRS.FooterRow.FindControl("ddlFRSNoRS");
    //    TextBox txtFRSCForm = (TextBox)gvVendorCFRS.FooterRow.FindControl("txtFRSCForm");
    //    HiddenField hdnCustomerValue = (HiddenField)gvVendorCFRS.FooterRow.FindControl("hdnCustomerValue");
    //    TextBox txtFRSIssueDate = (TextBox)gvVendorCFRS.FooterRow.FindControl("txtFRSIssueDate");
    //    TextBox txtFRSValidTillDate = (TextBox)gvVendorCFRS.FooterRow.FindControl("txtFRSValidTillDate");
    //    TextBox txtFRSRemarks = (TextBox)gvVendorCFRS.FooterRow.FindControl("txtFRSRemarks");
    //    Button btnAdd = (Button)gvVendorCFRS.FooterRow.FindControl("btnAdd");
    //    DateTime IssueDate;
    //    DateTime ValidTillDate;
    //    if (e.CommandName == "AddNew")
    //    {
    //        DataTable dtCAPApproval = (DataTable)ViewState["RSDetails"];
    //        DataTable dtapprove = new DataTable();
    //        DataRow dRow;
    //        DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
    //        dtformat.ShortDatePattern = ObjS3GSession.ProDateFormatRW;

    //        dRow = dtCAPApproval.NewRow();
    //        dRow["VENDORC_ID"] = "0";
    //        dRow["PA_SA_REF_ID"] = ddlFRSNoRS.SelectedValue;
    //        dRow["CUSTOMER_ID"] = hdnCustomerValue.Value;
    //        dRow["PANUM"] = ddlFRSNoRS.SelectedText;
    //        dRow["CUSTOMER_NAME"] = lblICustomerName.Text.Trim();
    //        IssueDate = Utility.StringToDate(txtFRSIssueDate.Text.Trim());
    //        IssueDate = IssueDate.Date;
    //        string A = IssueDate.ToString();
    //        A = A.Substring(1, 10);
    //        dRow["ISSUE_DATE"] = DateTime.Parse(Utility.StringToDate(txtFRSIssueDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
    //        dRow["C_FORM_NO"] = txtFRSCForm.Text.Trim();
    //        ValidTillDate = Utility.StringToDate(txtFRSValidTillDate.Text.Trim());
    //        ValidTillDate = ValidTillDate.Date;
    //        dRow["VALID_TLL_DATE"] = DateTime.Parse(Utility.StringToDate(txtFRSValidTillDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
    //        dRow["REMARKS"] = txtFRSRemarks.Text.Trim();

    //        dtCAPApproval.Rows.Add(dRow);
    //        gvVendorCFRS.DataSource = dtCAPApproval;
    //        ViewState["RSDetails"] = dtCAPApproval;
    //        gvVendorCFRS.DataBind();
    //    }
    //}

    //protected void gvVendorCFRS_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //}

    //protected void gvVendorCFRS_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    try
    //    {
    //        gvVendorCFRS.EditIndex = e.NewEditIndex;
    //        int intRowId = e.NewEditIndex;

    //        DataTable dtWBT = (DataTable)ViewState["RSDetails"];
    //        gvVendorCFRS.DataSource = dtWBT;
    //        gvVendorCFRS.DataBind();

    //        GridViewRow grvRow = gvVendorCFRS.Rows[intRowId];

    //        CalendarExtender CEERSIssueDate = (CalendarExtender)grvRow.FindControl("CEERSIssueDate");
    //        CEERSIssueDate.Format = strDateFormat;

    //        CalendarExtender CEERSValidTill = (CalendarExtender)grvRow.FindControl("CEERSValidTill");
    //        CEERSValidTill.Format = strDateFormat;

    //        TextBox txtERSIssueDate = (TextBox)grvRow.FindControl("txtERSIssueDate");
    //        txtERSIssueDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtERSIssueDate.ClientID + "','" + strDateFormat + "',null,null);");

    //        TextBox txtERSValidTillDate = (TextBox)grvRow.FindControl("txtERSValidTillDate");
    //        txtERSValidTillDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtERSValidTillDate.ClientID + "','" + strDateFormat + "',null,null);");

    //        gvVendorCFRS.FooterRow.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    //protected void gvVendorCFRS_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        DataTable dtRSUpdate = new DataTable();
    //        int intRowId = 0;
    //        DateTime IssueDate;
    //        DateTime ValidTillDate;
    //        intRowId = e.RowIndex;
    //        GridViewRow grvRow = gvVendorCFRS.Rows[intRowId];
    //        TextBox txtERSCForm = (TextBox)grvRow.FindControl("txtERSCForm");
    //        TextBox txtERSIssueDate = (TextBox)grvRow.FindControl("txtERSIssueDate");
    //        TextBox txtERSValidTillDate = (TextBox)grvRow.FindControl("txtERSValidTillDate");
    //        TextBox txtERSRemarks = (TextBox)grvRow.FindControl("txtERSRemarks");
    //        LinkButton btnGridSave = (LinkButton)grvRow.FindControl("lnkUpdate");

    //        dtRSUpdate = (DataTable)ViewState["RSDetails"];

    //        lblErrorMessage.Text = string.Empty;
    //        dtRSUpdate.Rows[intRowId]["C_FORM_NO"] = txtERSCForm.Text.Trim();
    //        IssueDate = Utility.StringToDate(txtERSIssueDate.Text.Trim());
    //        IssueDate = IssueDate.Date;
    //        dtRSUpdate.Rows[intRowId]["ISSUE_DATE"] = DateTime.Parse(Utility.StringToDate(txtERSIssueDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
    //        ValidTillDate = Utility.StringToDate(txtERSValidTillDate.Text.Trim());
    //        ValidTillDate = ValidTillDate.Date;
    //        dtRSUpdate.Rows[intRowId]["VALID_TLL_DATE"] = DateTime.Parse(Utility.StringToDate(txtERSValidTillDate.Text.Trim()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 
    //        dtRSUpdate.Rows[intRowId]["REMARKS"] = txtERSRemarks.Text.Trim();
    //        gvVendorCFRS.EditIndex = -1;

    //        dtRSUpdate = (DataTable)ViewState["RSDetails"];
    //        gvVendorCFRS.DataSource = dtRSUpdate;
    //        gvVendorCFRS.DataBind();

    //        gvVendorCFRS.FooterRow.Visible = true;
    //        ViewState["RSDetails"] = dtRSUpdate;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    //protected void gvVendorCFRS_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    DataTable dtRSDelete = new DataTable();
    //    gvVendorCFRS.EditIndex = -1;
    //    dtRSDelete = (DataTable)ViewState["RSDetails"];
    //    gvVendorCFRS.DataSource = dtRSDelete;
    //    gvVendorCFRS.DataBind();
    //    if (dtRSDelete.Rows.Count == 1)
    //    {
    //        Utility.FunShowAlertMsg(this.Page, "There should be atleast one row in the grid");
    //        return;
    //    }
    //    dtRSDelete.Rows.RemoveAt(e.RowIndex);
    //    if (dtRSDelete.Rows.Count == 0)
    //    {
    //        gvVendorCFRS.DataSource = dtRSDelete;
    //        gvVendorCFRS.DataBind();
    //    }
    //    else
    //    {
    //        //Reset Row_ID in DataTable
    //        for (int intCount = 0; intCount < dtRSDelete.Rows.Count; intCount++)
    //        {
    //            dtRSDelete.Rows[intCount]["ROWNUMBER"] = (intCount + 1).ToString();
    //        }
    //        gvVendorCFRS.DataSource = dtRSDelete;
    //        gvVendorCFRS.DataBind();
    //    }
    //    ViewState["RSDetails"] = dtRSDelete;
    //    gvVendorCFRS.FooterRow.Visible = true;
    //}

    //protected void gvVendorCFRS_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    try
    //    {
    //        DataTable dtRSCancel = new DataTable();
    //        gvVendorCFRS.EditIndex = -1;
    //        dtRSCancel = (DataTable)ViewState["RSDetails"];
    //        gvVendorCFRS.DataSource = dtRSCancel;
    //        gvVendorCFRS.DataBind();
    //        gvVendorCFRS.FooterRow.Visible = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}
    //#endregion

    #endregion

}