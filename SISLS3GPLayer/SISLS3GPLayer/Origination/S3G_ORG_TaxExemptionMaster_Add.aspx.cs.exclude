///Module Name      :   Origination
///Screen Name      :   S3G_ORG_TaxExemptionMaster_Add.aspx
///Created By       :   Vinodha.M
///Created Date     :   9-Oct-2014
///Purpose          :   To insert and update Tax Exemption details

using S3GBusEntity;
using S3GBusEntity.Origination;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Origination_S3G_ORG_TaxExemptionMaster_Add : ApplyThemeForProject
{

    #region Intialization

    DataTable dtTaxExemptionHistory = new DataTable();
    DataSet dsFilterDtlsByLessee;

    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjTaxExemptionMasterClient;
    OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable ObjS3G_ORG_TaxExemptionMasterDataTable = new OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable();
    string strDateFormat = string.Empty;
    Dictionary<string, string> Procparam = null;
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;
    int intTaxID = 0;
    int intUserId = 0;
    int intCompanyID = 0;
    bool bClearList = false;
    string strMode = string.Empty;
    string Cust_ID = string.Empty;
    string Loc_ID = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;
    string strRedirectPage = "../Origination/S3G_ORG_TaxExemptionMaster_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_TaxExemptionMaster_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3G_ORG_TaxExemptionMaster_View.aspx';";
    public static Origination_S3G_ORG_TaxExemptionMaster_Add obj_Page;
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
        FunPriBindGrid();
    }

    #endregion

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender1.Format = strDateFormat;

            #region Paging Config

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



            #endregion

            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            if (Request.QueryString["qsTaxId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsTaxId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intTaxID = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            txtFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtToDate.ClientID + "','" + strDateFormat + "',false,  false);");

            if (!IsPostBack)
            {
                if (dtTaxExemptionHistory.Rows.Count == 0)
                {
                    FunPriInsertTaxExemptionHistoryDataTable();
                }
                else
                {
                    FunPubBindTaxExemptionHistory(dtTaxExemptionHistory);
                }
                FunpriLoadCashFlowType();
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (((intTaxID > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if (((intTaxID > 0)) && (strMode == "Q"))
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion

    /// <summary>
    /// This is to disable controls based on user level role id
    /// </summary>
    /// <param name="intModeID"></param>

    #region Page Events


    protected void ddlLessee_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLessee.SelectedValue != "0")
            {
                Cust_ID = ddlLessee.SelectedValue;
                FunBindTANByCustID(Cust_ID);
                //ddlTAN.ClearSelection();
                //ddlTAN.Enabled = false;
                FunPriBindGrid();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void txtSection_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtSection.Text != String.Empty)
            {
                FunPriBindGrid();
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    //protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlLocation.SelectedValue != "0")
    //        {
    //            Cust_ID = ddlLessee.SelectedValue;
    //            ddlTAN.ClearSelection();
    //            FunBindTANByCustID(Cust_ID,ddlLocation.SelectedValue);                
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    /// <summary>
    /// This is used to save TaxGuide details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSave_Click(object sender, EventArgs e)
    {
        vsUserMgmt.Visible = true;

        if (Utility.StringToDate(txtToDate.Text) < Utility.StringToDate(txtFromDate.Text))
        {
            Utility.FunShowAlertMsg(this.Page, "Effective To Cannot be Less Than Effective From");
            return;
        }

        if (Convert.ToInt32(txtExemptionLimit.Text) <= 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Exemption Limit Should Be Greater Than Zero");
            return;
        }

        string Tax_Code = "";
        decimal Balance_Exe_Amount = 0;
        ObjTaxExemptionMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterRow ObjTaxExemptionMasterRow;
            ObjTaxExemptionMasterRow = ObjS3G_ORG_TaxExemptionMasterDataTable.NewS3G_ORG_TaxExemptionMasterRow();

            ObjTaxExemptionMasterRow.Company_ID = intCompanyID;
            if (ddlLessee.SelectedValue != "0")
                ObjTaxExemptionMasterRow.Customer_ID = Convert.ToInt32(ddlLessee.SelectedValue);
            //if (ddlLocation.SelectedValue != "0")
            //    ObjTaxExemptionMasterRow.State_ID = Convert.ToInt32(ddlLocation.SelectedValue);
            if (ddlTAN.SelectedValue != "0")
                ObjTaxExemptionMasterRow.TAN = ddlTAN.SelectedValue;
            if (ddlCashFlowType.SelectedValue != "0")
                ObjTaxExemptionMasterRow.Cashflow_ID = Convert.ToInt32(ddlCashFlowType.SelectedValue);

            ObjTaxExemptionMasterRow.Effective_From = Utility.StringToDate(txtFromDate.Text);
            ObjTaxExemptionMasterRow.Effective_To = Utility.StringToDate(txtToDate.Text);
            ObjTaxExemptionMasterRow.Tax_Law_Section = txtSection.Text;
            ObjTaxExemptionMasterRow.Certificate_No = txtCertificateNo.Text;
            if (!string.IsNullOrEmpty(txtExemptionLimit.Text))
                ObjTaxExemptionMasterRow.Exe_Limit_Amount = (txtExemptionLimit.Text.Replace(".", "") == "") ? 0 : Convert.ToDecimal(txtExemptionLimit.Text);
            ObjTaxExemptionMasterRow.Created_By = intUserId;
            ObjTaxExemptionMasterRow.Tax_ID = intTaxID;
            ObjTaxExemptionMasterRow.Is_Active = chkActive.Checked;
            ObjS3G_ORG_TaxExemptionMasterDataTable.AddS3G_ORG_TaxExemptionMasterRow(ObjTaxExemptionMasterRow);

            intErrCode = ObjTaxExemptionMasterClient.FunPubCreateTaxExemption(out Tax_Code, out Balance_Exe_Amount, SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_TaxExemptionMasterDataTable, SerMode));

            if (intErrCode == 0)
            {
                btnSave.Enabled = false;
                txtTaxCode.Text = Tax_Code;
                txtBalanceLimit.Text = Balance_Exe_Amount.ToString();
                strAlert = "Tax Exemption " + Tax_Code + " added successfully";
                strAlert += @"\n\nWould you like to add one more Tax Exemption?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 4)
            {
                btnSave.Enabled = false;
                txtTaxCode.Text = Tax_Code;
                strAlert = "Tax Exemption " + Tax_Code + " Updated successfully";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 5)
            {
                btnSave.Enabled = false;
                txtTaxCode.Text = Tax_Code;
                strAlert = "Tax Exemption " + Tax_Code + " Updated successfully";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Tax Exemption already exist");
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create TaxCode");
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "Effective from date cannot be less than date of incorporation of the company");
            }
            lblErrorMessage.Text = string.Empty;

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjTaxExemptionMasterClient.Close();
        }
    }

    /// <summary>
    /// This is used to redirect page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

    /// <summary>
    /// This is used to clear data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLessee.Clear();
            //ddlLocation.Clear();
            FunPriInsertTaxExemptionHistoryDataTable();
            ddlTAN.SelectedIndex = ddlCashFlowType.SelectedIndex = 0;
            txtFromDate.Text = txtToDate.Text = txtSection.Text = txtExemptionLimit.Text = txtCertificateNo.Text = txtTaxCode.Text = String.Empty;
            chkActive.Checked = true;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            // ObjTaxGuideMasterClient.Close();
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetTaxExemptionDetails()
    {
        ObjTaxExemptionMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_ORG_TaxExemptionMasterDataTable = new OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable();
            OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterRow ObjTaxExemptionMasterRow;
            SerializationMode SerMode = SerializationMode.Binary;
            ObjTaxExemptionMasterRow = ObjS3G_ORG_TaxExemptionMasterDataTable.NewS3G_ORG_TaxExemptionMasterRow();
            ObjTaxExemptionMasterRow.Company_ID = intCompanyID;
            ObjTaxExemptionMasterRow.Tax_ID = intTaxID;

            ObjS3G_ORG_TaxExemptionMasterDataTable.AddS3G_ORG_TaxExemptionMasterRow(ObjTaxExemptionMasterRow);

            byte[] byteTaxExemptionMasterDetails = ObjTaxExemptionMasterClient.FunPubQueryTaxExemption(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_TaxExemptionMasterDataTable, SerMode));

            ObjS3G_ORG_TaxExemptionMasterDataTable = new OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable();
            ObjS3G_ORG_TaxExemptionMasterDataTable = (OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable)ClsPubSerialize.DeSerialize(byteTaxExemptionMasterDetails, SerializationMode.Binary, typeof(OrgMasterMgtServices.S3G_ORG_TaxExemptionMasterDataTable));

            txtTaxCode.Text = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Tax_Code"].ToString();
            ddlLessee.SelectedValue = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Customer_ID"].ToString();
            ddlLessee.SelectedText = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Customer_Name"].ToString();
            Cust_ID = ddlLessee.SelectedValue;
            //ddlLocation.SelectedValue = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["State_ID"].ToString();
            //ddlLocation.SelectedText = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["State_DESC"].ToString();
            //Loc_ID = ddlLocation.SelectedValue;
            FunBindTANByCustID(Cust_ID);
            ddlTAN.SelectedValue = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["TAN"].ToString();
            txtFromDate.Text = DateTime.Parse(ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Effective_From"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            txtToDate.Text = DateTime.Parse(ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Effective_To"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            txtSection.Text = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Tax_Law_Section"].ToString();
            ddlCashFlowType.SelectedValue = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Cashflow_ID"].ToString();
            txtExemptionLimit.Text = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Exe_Limit_Amount"].ToString();
            txtCertificateNo.Text = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Certificate_No"].ToString();
            txtBalanceLimit.Text = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Balance_Exe_Amount"].ToString();

            hdnID.Value = ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Created_By"].ToString();

            if (ObjS3G_ORG_TaxExemptionMasterDataTable.Rows[0]["Is_Active"].ToString() == "True")
                chkActive.Checked = true;
            else
                chkActive.Checked = false;

            FunPriBindGrid();
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjTaxExemptionMasterClient.Close();
        }
    }

    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                chkActive.Enabled = false;
                chkActive.Checked = true;
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlTAN.Items.Insert(0, liSelect);
                ddlTAN.Enabled = false;
                txtBalanceLimit.Enabled = false;
                CalendarExtender1.Format = strDateFormat;
                CalendarExtender2.Format = strDateFormat;
                break;

            case 1: // Modify Mode

                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                btnClear.Enabled = false;
                FunGetTaxExemptionDetails();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                ddlLessee.Enabled = false;
                ddlTAN.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtSection.Enabled = true;
                ddlCashFlowType.Enabled = true;
                txtCertificateNo.Enabled = true;
                txtExemptionLimit.Enabled = true;
                txtBalanceLimit.Enabled = false;
                txtTaxCode.Enabled = false;
                chkActive.Enabled = true;
                CalendarExtender1.Format = strDateFormat;
                CalendarExtender2.Format = strDateFormat;
                FunDisableControls();
                break;

            case -1:// Query Mode                
                FunGetTaxExemptionDetails();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                if (bClearList)
                {
                    ddlLessee.ReadOnly = true;
                    ddlTAN.ClearDropDownList();
                    txtFromDate.ReadOnly = true;
                    txtToDate.ReadOnly = true;
                    txtSection.ReadOnly = true;
                    ddlCashFlowType.ClearDropDownList();
                    txtExemptionLimit.ReadOnly = true;
                    txtCertificateNo.ReadOnly = true;
                    txtTaxCode.ReadOnly = true;
                    chkActive.Enabled = false;
                    gvTaxExemptionHistory.Enabled = false;
                }

                CalendarExtender1.Enabled = false;
                CalendarExtender2.Enabled = false;

                txtFromDate.Attributes.Remove("onblur");
                txtToDate.Attributes.Remove("onblur");
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                chkActive.Enabled = false;
                txtTaxCode.Enabled = true;
                break;
        }
    }


    private void FunDisableControls()
    {
        try
        {
            if (txtExemptionLimit.Text != String.Empty && txtBalanceLimit.Text != String.Empty)
            {
                if (txtExemptionLimit.Text != txtBalanceLimit.Text)
                {

                    ddlTAN.Enabled = txtFromDate.Enabled = CalendarExtender1.Enabled = CalendarExtender2.Enabled =
                    txtToDate.Enabled = txtSection.Enabled = ddlCashFlowType.Enabled = txtCertificateNo.Enabled = txtExemptionLimit.Enabled =
                    chkActive.Enabled = btnSave.Enabled = btnClear.Enabled = false;
                    if (chkActive.Checked == true)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Since This Tax Exemption have been used in Transaction,You Cannot Update");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunBindTANByCustID(string Cust_ID)
    {
        try
        {
            if (ddlLessee.SelectedValue != "0")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@User_ID", Convert.ToString(intUserId));
                Procparam.Add("@CustomerID", Convert.ToString(Cust_ID));                
                dsFilterDtlsByLessee = Utility.GetDataset("S3G_ORG_GetTaxExmpDetailsByCustID", Procparam);
                ddlTAN.BindDataTable(dsFilterDtlsByLessee.Tables[0], new string[] { "TAN", "TAN" });
                ddlTAN.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private void FunPriBindGrid()
    {
        Procparam = new Dictionary<string, string>();
        try
        {
            Procparam.Add("@Tax_ID", Convert.ToString(intTaxID));
            if (ddlLessee.SelectedValue != "0")
                Procparam.Add("@Customer_ID", Convert.ToString(ddlLessee.SelectedValue));
            if(txtSection.Text!=String.Empty)
                Procparam.Add("@Section", txtSection.Text);
            //Paging Properties set

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            //Paging Properties end

            //Paging Config            
            bool bIsNewRow = false;

            gvTaxExemptionHistory.BindGridView("S3G_ORG_Get_TaxExmHis_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);


            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                gvTaxExemptionHistory.Rows[0].Visible = false;
            }

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

        }
        catch (FaultException<OrgMasterMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            // ObjTaxExemptionMasterClient.Close();
        }
    }

    private void FunpriLoadCashFlowType()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            ddlCashFlowType.BindDataTable("S3G_ORG_Get_TxE_CashFlowFlag", Procparam, new string[] { "CashFlow_Flag_ID", "CashFlowFlag_Desc" });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriInsertTaxExemptionHistoryDataTable()
    {
        try
        {
            DataRow drEmptyRow;
            dtTaxExemptionHistory = FunPubGetTaxExemptionHistoryDataTable();

            if (dtTaxExemptionHistory.Rows.Count == 0)
            {
                drEmptyRow = dtTaxExemptionHistory.NewRow();
                drEmptyRow["SNO"] = "0";
                dtTaxExemptionHistory.Rows.Add(drEmptyRow);
            }

            if (dtTaxExemptionHistory.Rows.Count > 1)
            {
                if (dtTaxExemptionHistory.Rows[0]["SNO"].Equals("0"))
                {
                    dtTaxExemptionHistory.Rows[0].Delete();
                }
            }

            ViewState["Tax_Exemption_HistoryDetails"] = dtTaxExemptionHistory;

            dtTaxExemptionHistory = FunPubGetTaxExemptionHistoryDataTable();
            FunPubBindTaxExemptionHistory(dtTaxExemptionHistory);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable FunPubGetTaxExemptionHistoryDataTable()
    {
        try
        {
            if (ViewState["Tax_Exemption_HistoryDetails"] == null)
            {
                dtTaxExemptionHistory = new DataTable();
                dtTaxExemptionHistory.Columns.Add("TAN");
                dtTaxExemptionHistory.Columns.Add("Tax_Law_Section");
                dtTaxExemptionHistory.Columns.Add("Effective_From");
                dtTaxExemptionHistory.Columns.Add("Effective_To");
                dtTaxExemptionHistory.Columns.Add("CashFlowFlag_Desc");
                dtTaxExemptionHistory.Columns.Add("Exe_Limit_Amount");
                dtTaxExemptionHistory.Columns.Add("Certificate_No");
                dtTaxExemptionHistory.Columns.Add("SNO");
                ViewState["Tax_Exemption_HistoryDetails"] = dtTaxExemptionHistory;
            }
            dtTaxExemptionHistory = (DataTable)ViewState["Tax_Exemption_HistoryDetails"];
            return dtTaxExemptionHistory;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPubBindTaxExemptionHistory(DataTable dtTaxExemptionHistory)
    {
        try
        {
            gvTaxExemptionHistory.DataSource = dtTaxExemptionHistory;
            gvTaxExemptionHistory.DataBind();
            if (dtTaxExemptionHistory.Rows[0]["SNO"].ToString().Equals("0"))
            {
                gvTaxExemptionHistory.Rows[0].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetLesseeDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_TxE_Get_LesseeDtls", Procparam), false);
        return suggetions.ToArray();
    }

    //Delivery State Details
    [System.Web.Services.WebMethod]
    public static string[] GetDeliveryStateDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetStateName", Procparam));
        return suggetions.ToArray();
    }

    #endregion

}