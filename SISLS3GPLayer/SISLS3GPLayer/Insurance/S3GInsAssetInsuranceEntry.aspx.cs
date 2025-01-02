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
using System.Text;
using System.Globalization;
using S3GBusEntity;

public partial class Insurance_S3GInsAssetInsuranceEntry : ApplyThemeForProject
{
    #region [Common Variable declaration]
    DataTable DtPolicyDetails;
    int intCompanyId, intUserID = 0;
    int intAINSEId = 0;
    int Allow = 0;
    static int intCustomer = 0;
    decimal dcmTotalPremium = 0;
    Dictionary<string, string> Procparam = null;
    string strAINSEID = "0";
    int intUserId = 0;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    StringBuilder strAINSEBuilder = new StringBuilder();
    DataTable dtAssetInsEntry = null;
    S3GSession ObjS3GSession = new S3GSession();
    StringBuilder strPolicyBuilder = new StringBuilder();
    InsuranceMgtServicesReference.InsuranceMgtServicesClient objInsuranceClient;

    public static Insurance_S3GInsAssetInsuranceEntry obj_Page;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Insurance/S3GInsAssetInsuranceEntry.aspx?qsMode=C";
    string strRedirectPageAdd = "window.location.href='../Insurance/S3GInsAssetInsuranceEntry.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../Insurance/S3GInsTranslander.aspx?Code=AINE';";
    string strPageName = "Asset Insurance Entry";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    #endregion


    #region [Page Load Event]
    protected void Page_Load(object sender, EventArgs e)
    {
        S3GSession ObjS3GSession = null;
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            //Date Format
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            //End
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            if (intCompanyId == 0)
                intCompanyId = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                intAINSEId = Convert.ToInt32(fromTicket.Name);
            }

            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            obj_Page = this;


            if (!IsPostBack)
            {
                //FunsetInsuranceType(DropDownListInsType);
             

                FunPriLoad_LOB_Branch();
                FunProLoadAddressCombos();
                FunPriLoadLOV();

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                strMode = Request.QueryString["qsMode"].ToString();
                if (strMode == "Q")
                {
                    FunPriDisableControls(-1);
                    ucCustomerCodeLov.ButtonEnabled = false;
                }
                if (strMode == "M")
                {
                    FunPriDisableControls(1);
                }
                if (strMode != "Q" && strMode != "M")
                {
                    //FunPriInsertAssetInsEntryDataTable("", "", "", "", "", "", "", "", "", "-1", "-1", "0", "", "");
                    txtAINSEDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);
                    FunPriDisableControls(0);
                    FunPriGenerateNewPolicyRow();
                }
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Attributes.Add("onfocus", "fnLoadCustomer();");
            }


            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvAssetInsuranceEntry.ErrorMessage = "Due to Data Problem,Unable to load Page";
            cvAssetInsuranceEntry.IsValid = false;
        }
        finally
        {
            ObjS3GSession = null;
        }
    }
    #endregion

    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeypress", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
        }
    }

    protected void FunProLoadAddressCombos()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);

            DataTable dtSource = new DataTable();
            if (dtAddr.Select("Category = 1").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            //txtInsCompanyCity.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 2").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 2").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            //txtInsCompanyState.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            //txtInsCompanyCountry.FillDataTable(dtSource, "Name", "Name", false);

        }
        catch (Exception ex)
        {

        }
    }

    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("Category");
        return dt;
    }

    #region Current Policy Grid

    private void FunPriBindPolicyDLL(string Mode)
    {
        try
        {
            if (Mode == "Add")
            {
                DataTable ObjDT = new DataTable();
                ObjDT.Columns.Add("AssetDescription");
                ObjDT.Columns.Add("AssetID");
                ObjDT.Columns.Add("DetailsID");
                ObjDT.Columns.Add("AssetValue");
                ObjDT.Columns.Add("PolicyType");
                ObjDT.Columns.Add("Invoice_ID");
                ObjDT.Columns.Add("PolicyTypeID");
                ObjDT.Columns.Add("RSNo");
                ObjDT.Columns.Add("PolicyNo");
                ObjDT.Columns.Add("PolicyDate", typeof(DateTime));
                ObjDT.Columns.Add("ValidTillDate", typeof(DateTime));
                ObjDT.Columns.Add("PolicyValue");
                ObjDT.Columns.Add("LOSS_PAYEE_DETAILS");
                ObjDT.Columns.Add("Ins_Plan");
                ObjDT.Columns.Add("AssetNumber");
                ObjDT.Columns.Add("Premium");
                ObjDT.Columns.Add("REMARKS");
                ObjDT.Columns.Add("Payment_Request_No");
                ObjDT.Columns.Add("DetailsId");
                ObjDT.Columns.Add("CanEdit");

                DataRow dr_Alert = ObjDT.NewRow();
                dr_Alert["AssetDescription"] = "";
                dr_Alert["AssetID"] = "";
                dr_Alert["DetailsID"] = "";
                dr_Alert["Invoice_ID"] = "";
                dr_Alert["PolicyType"] = "";
                dr_Alert["PolicyTypeID"] = "";
                dr_Alert["RSNo"] = "";
                dr_Alert["PolicyNo"] = "";
                dr_Alert["PolicyDate"] = "01/01/1900";
                dr_Alert["ValidTillDate"] = "01/01/1900";
                dr_Alert["REMARKS"] = "";
                dr_Alert["AssetValue"] = "";
                dr_Alert["LOSS_PAYEE_DETAILS"] = "";
                dr_Alert["PolicyValue"] = "";
                dr_Alert["Premium"] = "";
                dr_Alert["Nominee"] = "";
                dr_Alert["Ins_Plan"] = "";
                dr_Alert["AssetNumber"] = "";
                dr_Alert["Payment_Request_No"] = "";
                dr_Alert["CanEdit"] = "";

                ObjDT.Rows.Add(dr_Alert);

                gvCurrentInsurance.DataSource = ObjDT;
                gvCurrentInsurance.DataBind();
                //FunPubSetFooterRowVisibility();

                //ScriptManager.RegisterStartupScript(this, GetType(), "te", "ReHeight(" + (Convert.ToInt64(((ObjDT.Rows.Count + 1) * 20)) + 80).ToString() + ");Resize();", true);

                ObjDT.Rows.Clear();
                ViewState["DtPolicyDetails"] = ObjDT;

                gvCurrentInsurance.Rows[0].Cells.Clear();
                gvCurrentInsurance.Rows[0].Visible = false;
                FunPriGenerateNewPolicyRow();
                gvCurrentInsurance.Visible = true;

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    protected void FunPubSetFooterRowVisibility()
    {
        if (PageMode == PageModes.Create || PageMode == PageModes.Modify)
        {
            DropDownList ddlAssetDescription = gvCurrentInsurance.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
            if (ddlAssetDescription.Items.Count - 1 == gvCurrentInsurance.Rows.Count && gvCurrentInsurance.Rows[0].Visible == true)
            {
                gvCurrentInsurance.FooterRow.Visible = false;
            }
            else
            {
                gvCurrentInsurance.FooterRow.Visible = true;
            }
        }
    }

    private void FunPriGenerateNewPolicyRow()
    {
        try
        {
            //Dictionary<string, string> objParameters = new Dictionary<string, string>();
            //DropDownList ddlPolicyType = (DropDownList)gvCurrentInsurance.FooterRow.FindControl("ddlFPolicyType");
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@OPTION", "10");
            //ddlPolicyType.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            //DropDownList ddlAssetDescription = gvCurrentInsurance.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
            //Procparam.Clear();
            //Procparam.Add("@OPTION", "1");

            //if (intAINSEId > 0)
            //{
            //    if (ddlMLA.SelectedValue != "0")
            //    {
            //        Procparam.Add("@Panum", ddlMLA.SelectedValue);
            //        Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedItem.Text);
            //        ddlAssetDescription.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "asset_id", "asset_code", "Asset_Description" });
            //    }
            //}
            //else
            //{
            //    // if (ddlMLA.SelectedIndex != -1)
            //    if (ddlMLA.SelectedValue != "0")
            //    {
            //        Procparam.Add("@Panum", ddlMLA.SelectedValue);
            //        if (ddlSLA.Items.Count > 0)
            //        {
            //            Procparam.Add("@Sanum", (ddlSLA.SelectedValue.ToString() == "0") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedItem.Text);
            //        }
            //        else
            //        {
            //            Procparam.Add("@Sanum", ddlMLA.SelectedValue + "DUMMY");
            //        }
            //        ddlAssetDescription.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "asset_id", "asset_code", "Asset_Description" });
            //    }
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void btnAddPolicy_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtPolicyDetails = (DataTable)ViewState["DtPolicyDetails"];
            //DropDownList ddlAssetDescription = gvCurrentInsurance.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
            DropDownList ddlPolicyType = gvCurrentInsurance.FooterRow.FindControl("ddlFPolicyType") as DropDownList;
            TextBox txtPolicyNumber = gvCurrentInsurance.FooterRow.FindControl("txtPolicyNumber") as TextBox;
            TextBox txtPolicyDate = gvCurrentInsurance.FooterRow.FindControl("txtPolicyDate") as TextBox;
            TextBox txtValidDate = gvCurrentInsurance.FooterRow.FindControl("txtValidDate") as TextBox;
            TextBox txtPolicyValue = gvCurrentInsurance.FooterRow.FindControl("txtPolicyValue") as TextBox;
            TextBox txtPremium = gvCurrentInsurance.FooterRow.FindControl("txtPremium") as TextBox;

            if (Utility.CompareDates(txtPolicyDate.Text, txtValidDate.Text) != 1)
            {
                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be greater than Policy Date";
                cvAssetInsuranceEntry.IsValid = false;
                return;
            }
            if (Utility.CompareDates(txtAINSEDate.Text, txtValidDate.Text) != 1)
            {
                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be greater than System Date";
                cvAssetInsuranceEntry.IsValid = false;
                return;
            }


            DataRow[] dr;
            //if (intAINSEId > 0)
            //{
            //    DataTable dtCheck = (DataTable)ViewState["DtCheckPolicyDetails"];
            //    dr = dtCheck.Select("AssetID = " + ddlAssetDescription.SelectedValue + " and MachineNo = '" + txtRegNo.SelectedItem.Text + "'", "PolicyDate ASC");
            //}
            //else
            //{
            //    dr = DtPolicyDetails.Select("AssetID = " + ddlAssetDescription.SelectedValue + " and MachineNo = '" + txtRegNo.SelectedItem.Text + "'", "PolicyDate ASC");
            //}
            //if (!ValidateAsset())
            //{
            //    return;
            //}

            //if (!ValidatePolicyDetails(ddlAssetDescription.SelectedValue, txtRegNo.SelectedItem.Text, txtPolicyDate.Text, txtValidDate.Text))
            //    return;
            //string strErrorMessage = "Policy Date range already exist for selected Asset";
            //if (dr.Length > 0)
            //{
            //    if (Utility.StringToDate(txtPolicyDate.Text) <= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && (Utility.StringToDate(txtValidDate.Text) >= Utility.StringToDate(dr[dr.Length - 1]["ValidTillDate"].ToString())))
            //    {

            //        cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
            //        cvAssetInsuranceEntry.IsValid = false;
            //        return;
            //    }

            //    if (dr.Length == 1)
            //    {

            //        if (Utility.StringToDate(txtPolicyDate.Text) >= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && Utility.StringToDate(txtPolicyDate.Text) <= Utility.StringToDate(dr[0]["ValidTillDate"].ToString()))
            //        {

            //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
            //            cvAssetInsuranceEntry.IsValid = false;
            //            return;
            //        }
            //        if (Utility.StringToDate(txtValidDate.Text) >= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && Utility.StringToDate(txtValidDate.Text) <= Utility.StringToDate(dr[0]["ValidTillDate"].ToString()))
            //        {
            //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
            //            cvAssetInsuranceEntry.IsValid = false;
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        if (Utility.StringToDate(txtPolicyDate.Text) >= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && Utility.StringToDate(txtPolicyDate.Text) <= Utility.StringToDate(dr[dr.Length - 1]["ValidTillDate"].ToString()))
            //        {
            //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
            //            cvAssetInsuranceEntry.IsValid = false;
            //            return;
            //        }
            //        if (Utility.StringToDate(txtValidDate.Text) >= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && Utility.StringToDate(txtValidDate.Text) <= Utility.StringToDate(dr[dr.Length - 1]["ValidTillDate"].ToString()))
            //        {
            //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
            //            cvAssetInsuranceEntry.IsValid = false;
            //            return;
            //        }
            //    }
            //}

            if (Convert.ToInt32(txtPolicyValue.Text) <= Convert.ToInt32(txtPremium.Text))
            {
                txtPremium.Focus();
                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Premium Value should not be greater than or equal to the Policy Value";
                cvAssetInsuranceEntry.IsValid = false;
                return;
            }

            DataRow drPolicy = DtPolicyDetails.NewRow();
            drPolicy["AssetDescription"] = ddlPolicyType.SelectedItem.Text;
            drPolicy["AssetID"] = ddlPolicyType.SelectedValue;
            drPolicy["PolicyType"] = ddlPolicyType.SelectedItem.Text;
            drPolicy["PolicyTypeID"] = ddlPolicyType.SelectedValue;
            drPolicy["RSNO"] = txtPolicyNumber.Text;
            drPolicy["PolicyNo"] = txtPolicyNumber.Text;
            drPolicy["PolicyDate"] = Utility.StringToDate(txtPolicyDate.Text);
            drPolicy["ValidTillDate"] = Utility.StringToDate(txtValidDate.Text);
            drPolicy["PolicyValue"] = txtPolicyValue.Text;
            if (txtPremium.Text == "")
            {
                drPolicy["Premium"] = "0";
            }
            else
            {
                drPolicy["Premium"] = txtPremium.Text;
            }
            DtPolicyDetails.Rows.Add(drPolicy);
            if (intAINSEId > 0)
            {
                DataTable dtCheck = (DataTable)ViewState["DtCheckPolicyDetails"];
                DataRow drCheckPolicy = dtCheck.NewRow();
                drCheckPolicy["AssetDescription"] = txtPolicyNumber.Text;
                drCheckPolicy["AssetID"] = txtPolicyNumber.Text;
                drCheckPolicy["PolicyType"] = ddlPolicyType.SelectedItem.Text;
                drCheckPolicy["PolicyTypeID"] = ddlPolicyType.SelectedValue;
                drCheckPolicy["MachineNo"] = txtPolicyNumber.Text;
                drCheckPolicy["PolicyNo"] = txtPolicyNumber.Text;
                drCheckPolicy["PolicyDate"] = Utility.StringToDate(txtPolicyDate.Text);
                drCheckPolicy["ValidTillDate"] = Utility.StringToDate(txtValidDate.Text);
                drCheckPolicy["PolicyValue"] = txtPolicyValue.Text;
                if (txtPremium.Text == "")
                {
                    drCheckPolicy["Premium"] = "0";
                }
                else
                {
                    drCheckPolicy["Premium"] = txtPremium.Text;
                }
                dtCheck.Rows.Add(drCheckPolicy);
                ViewState["DtCheckPolicyDetails"] = dtCheck;
            }
            gvCurrentInsurance.DataSource = DtPolicyDetails;
            gvCurrentInsurance.DataBind();
            //FunPubSetFooterRowVisibility();
            //ScriptManager.RegisterStartupScript(this, GetType(), "te", "ReHeight(" + (Convert.ToInt64(((DtPolicyDetails.Rows.Count + 1) * 20)) + 80).ToString() + ");Resize();", true);

            ViewState["DtPolicyDetails"] = DtPolicyDetails;
            ViewState["DT_AssetInsEntryDetails"] = DtPolicyDetails;
            if (ViewState["TotalPremium"] != null)
            {
                dcmTotalPremium = Convert.ToDecimal(ViewState["TotalPremium"]);
            }
            if (txtPremium.Text == "")
            {
                dcmTotalPremium += Convert.ToDecimal("0");
            }
            else
            {
                dcmTotalPremium += Convert.ToDecimal(txtPremium.Text);
            }
           
            lblTotalPremium.Text = Convert.ToString(dcmTotalPremium);
            ViewState["TotalPremium"] = lblTotalPremium.Text;
            FunPriGenerateNewPolicyRow();
            gvCurrentInsurance.Visible=true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvAssetInsuranceEntry.ErrorMessage = "Due to Data Problem,Unable to Add Policy";
            cvAssetInsuranceEntry.IsValid = false;
        }

    }
    private void CalculatePremium()
    {
        foreach (GridViewRow grvData in gvCurrentInsurance.Rows)
        {
            if (grvData.RowType == DataControlRowType.DataRow)
            {
                TextBox txtpremium = (TextBox)grvData.FindControl("txtPremium");
                if (txtpremium.Text != "")
                {
                    dcmTotalPremium += Convert.ToDecimal(txtpremium.Text);
                }
                
            }

        }
        lblTotalPremium.Text = Convert.ToString(dcmTotalPremium);
    }

    protected void gvCurrentInsurance_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
        }
    }

    //protected void gvCurrentInsurance_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.Footer)
    //        {
    //            AjaxControlToolkit.CalendarExtender CEPolicyDate = e.Row.FindControl("CEPolicyDate") as AjaxControlToolkit.CalendarExtender;
    //            CEPolicyDate.Format = strDateFormat;
    //            AjaxControlToolkit.CalendarExtender calValidDate = e.Row.FindControl("calValidDate") as AjaxControlToolkit.CalendarExtender;
    //            calValidDate.Format = strDateFormat;
    //            TextBox txtPolicyDate = e.Row.FindControl("txtPolicyDate") as TextBox;
    //            txtPolicyDate.Attributes.Add("readonly", "true");

    //            TextBox txtValidTillDate = e.Row.FindControl("txtValidDate") as TextBox;
    //            txtValidTillDate.Attributes.Add("readonly", "true");

    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            TextBox txtPolicyDateEdit = e.Row.FindControl("lblPolicyDate") as TextBox;
    //            txtPolicyDateEdit.Text = DateTime.Parse(txtPolicyDateEdit.Text, CultureInfo.CurrentCulture).ToString(strDateFormat);
    //            //txtPolicyDate.Attributes.Add("readonly", "true");

    //            TextBox txtValidTillDate = e.Row.FindControl("lblValidTill") as TextBox;
    //            txtValidTillDate.Text = DateTime.Parse(txtValidTillDate.Text, CultureInfo.CurrentCulture).ToString(strDateFormat);
    //            //txtValidTillDate.Attributes.Add("readonly", "true");

    //            AjaxControlToolkit.CalendarExtender calPolicyDateEdit = e.Row.FindControl("calPolicyDateEdit") as AjaxControlToolkit.CalendarExtender;
    //            calPolicyDateEdit.Format = strDateFormat;
    //            AjaxControlToolkit.CalendarExtender calValidDateEdit = e.Row.FindControl("calValidDateEdit") as AjaxControlToolkit.CalendarExtender;
    //            calValidDateEdit.Format = strDateFormat;


    //            DropDownList ddlAssetDescription = e.Row.FindControl("ddlAssetDescription") as DropDownList;
    //            DropDownList txtRegNo = e.Row.FindControl("lblRegNo") as DropDownList;
    //            DataTable dtPolicy = (DataTable)ViewState["DtPolicyDetails"];
    //            DropDownList ddlPolicyType = e.Row.FindControl("ddlPolicyType") as DropDownList;
    //            Procparam = new Dictionary<string, string>();
    //            Procparam.Add("@OPTION", "10");
    //            ddlPolicyType.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
    //            Procparam.Clear();
    //            Procparam.Add("@OPTION", "1");
    //            if (ddlMLA.SelectedValue != "0" && ddlMLA.SelectedValue != "")
    //            {
    //                Procparam.Add("@Panum", ddlMLA.SelectedItem.Text);
    //                Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedItem.Text + "DUMMY" : ddlSLA.SelectedItem.Text);
    //                ddlAssetDescription.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "asset_id", "asset_code", "Asset_Description" });
    //            }


    //            if (dtPolicy != null)
    //            {
    //                if (dtPolicy.Rows.Count > 0)
    //                {
    //                    ddlPolicyType.SelectedValue = dtPolicy.Rows[e.Row.RowIndex]["PolicyTypeID"].ToString();
    //                    ddlAssetDescription.SelectedValue = dtPolicy.Rows[e.Row.RowIndex]["AssetID"].ToString();

    //                    Procparam = new Dictionary<string, string>();

    //                    Procparam.Add("@Option", "5");
    //                    if (ddlMLA.SelectedValue != "0" || ddlMLA.SelectedValue != "")
    //                    {
    //                        Procparam.Add("@Panum", ddlMLA.SelectedItem.Text);
    //                    }

    //                    Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedItem.Text + "DUMMY" : ddlSLA.SelectedItem.Text);

    //                    Procparam.Add("@AssetId", ddlAssetDescription.SelectedValue);

    //                    txtRegNo.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "MachineNo", "MachineNo" });
    //                    txtRegNo.SelectedValue = dtPolicy.Rows[e.Row.RowIndex]["MachineNo"].ToString();
    //                }
    //                if (strMode == "Q")
    //                {

    //                    TextBox txtPolicyNumber = e.Row.FindControl("lblPolicyNumber") as TextBox;
    //                    TextBox txtPolicyDate = e.Row.FindControl("lblPolicyDate") as TextBox;
    //                    TextBox txtValidDate = e.Row.FindControl("lblValidTill") as TextBox;
    //                    TextBox txtPolicyValue = e.Row.FindControl("lblPolicyValue") as TextBox;
    //                    TextBox txtPremium = e.Row.FindControl("lblPremium") as TextBox;
    //                    txtRegNo.ClearDropDownList();
    //                    txtPolicyNumber.ReadOnly = txtPolicyValue.ReadOnly = txtPremium.ReadOnly = true;
    //                    ddlPolicyType.ClearDropDownList();
    //                    ddlAssetDescription.ClearDropDownList();
    //                    calPolicyDateEdit.Enabled = calValidDateEdit.Enabled = false;
    //                }
    //            }


    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + ex.Message;
    //        cvAssetInsuranceEntry.IsValid = false;
    //    }
    //}

    //protected void gvCurrentInsurance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        DtPolicyDetails = (DataTable)ViewState["DtPolicyDetails"];
    //        if (DtPolicyDetails.Rows.Count > 0)
    //        {
    //            dcmTotalPremium = Convert.ToDecimal(ViewState["TotalPremium"]);
    //            dcmTotalPremium -= Convert.ToDecimal(DtPolicyDetails.Rows[e.RowIndex]["Premium"]);
    //            lblTotalPremium.Text = Convert.ToString(dcmTotalPremium);
    //            ViewState["TotalPremium"] = dcmTotalPremium;
    //            DtPolicyDetails.Rows.RemoveAt(e.RowIndex);

    //            if (DtPolicyDetails.Rows.Count == 0)
    //            {
    //                FunPriBindPolicyDLL("Add");
    //            }
    //            else
    //            {
    //                gvCurrentInsurance.DataSource = DtPolicyDetails;
    //                gvCurrentInsurance.DataBind();
    //                FunPriGenerateNewPolicyRow();

    //                //ScriptManager.RegisterStartupScript(this, GetType(), "te", "ReHeight(" + (Convert.ToInt64(((DtPolicyDetails.Rows.Count + 1) * 20)) + 80).ToString() + ");Resize();", true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        cvAssetInsuranceEntry.ErrorMessage = "Due to Data Problem,Unable to Remove Policy";
    //        cvAssetInsuranceEntry.IsValid = false;

    //    }
    //}

    #endregion

    #region [To populate LOV]

    private void FunPriLoadLOV()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            //Procparam.Add("@OPTION", "6");
            //Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
            //Procparam.Add("@USERID", Convert.ToString(intUserID));
            //ddlLOB.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //Procparam.Clear();
            //Procparam.Add("@OPTION", "4");
            //Procparam.Add("@USERID", Convert.ToString(intUserID));
            //ddlBranch.BindDataTable("S3G_CLN_LOADLOV", Procparam, new string[] { "BRANCH_ID", "BRANCH_CODE", "BRANCH_NAME" });
            //Procparam.Clear();
            Procparam.Add("@OPTION", "25");
            ddlPayType.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            ddlPayType.SelectedIndex = 1;
            ddlPayType.ClearDropDownList();
            Procparam.Clear();
            Procparam.Add("@OPTION", "3");
            ddlInsuranceDoneBy.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            Procparam.Clear();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Ins_Type", DropDownListInsType.SelectedValue);

            DropDownListInsType.BindDataTable("S3G_Insurance_GetInsuranceType", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            Procparam.Clear();
            Procparam.Clear();
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriLoadDependentLOV()
    {
        //try
        //{
        //    if (Procparam != null)
        //        Procparam.Clear();
        //    else
        //        Procparam = new Dictionary<string, string>();

        //    Procparam.Add("@TYPE", "1");
        //    Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
        //    if (hdnCustomerID.Value != "")
        //    {
        //        Procparam.Add("@CUSTOMER_ID", hdnCustomerID.Value);
        //    }
        //    if (ddlLOB.SelectedIndex > 0)
        //    {
        //        Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        //    }
        //    if (ddlBranch.SelectedIndex > 0)
        //    {
        //        Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
        //    }
        //    else if (ddlBranch.SelectedIndex == 0 || ddlBranch.SelectedIndex == null)
        //    {
        //        Procparam.Add("@Location_ID", Convert.ToString(0));
        //    }
        //    Procparam.Add("@Program_ID", Convert.ToString(131));
        //    if (PageMode == PageModes.Create)
        //    {
        //        Procparam.Add("@Is_Activated", "1");
        //        //Commented by saranya on 03-07-2012 
        //        //Procparam.Add("@NotIN_LOBs", "'WC','FT','TE','TL'");
        //        Procparam.Add("@NotIN_LOBs", "'WC','FT'");
        //        Procparam.Add("@Check_Access", ObjUserInfo.ProUserIdRW.ToString());
        //        Procparam.Add("@ParamPA_Status", "3,11,13,42,46,4,31,5");
        //        Procparam.Add("@ParamSA_Status", "13,42,46,4,31,5");
        //    }

        //    ddlMLA.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "PANUM", "PANUM" });


        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }

    #endregion

    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        Procparam.Add("@TYPE", "1");
        Procparam.Add("@COMPANY_ID", Convert.ToString(obj_Page.intCompanyId));
        //if (obj_Page.hdnCustomerID.Value != "")
        //{
        //    Procparam.Add("@CUSTOMER_ID", obj_Page.hdnCustomerID.Value);
        //}
        if (obj_Page.ddlLOB.SelectedValue != "0")
        {
            Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));

        }
        if (obj_Page.ddlBranch.SelectedValue != "0")
        {
            Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        }
        else if (obj_Page.ddlBranch.SelectedValue == "0" || obj_Page.ddlBranch.SelectedValue == "")
        {
            Procparam.Add("@Location_ID", Convert.ToString(0));
        }
        Procparam.Add("@Program_ID", Convert.ToString(131));
        if (obj_Page.PageMode == PageModes.Create)
        {
            Procparam.Add("@Is_Activated", "1");
            Procparam.Add("@NotIN_LOBs", "'WC','FT'");
            Procparam.Add("@Check_Access", obj_Page.intUserID.ToString());
            Procparam.Add("@ParamPA_Status", "3,11,13,42,46,4,31,5");
            Procparam.Add("@ParamSA_Status", "13,42,46,4,31,5");
        }
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPLASLA_AIE_AGT", Procparam));

        return suggetions.ToArray();
    }


    #region Load AINSE Details
    private void FunPriLoadAINSEDetails()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@CompanyId", intCompanyId.ToString());
        Procparam.Add("@AccountInsID", intAINSEId.ToString());
        DataSet dsAINSE = Utility.GetDataset("S3G_INS_getAINSEDetails", Procparam);
        if (dsAINSE != null)
        {
            txtAINSENO.Text = dsAINSE.Tables[0].Rows[0]["AINSE_No"].ToString();
            txtAINSEDate.Text = dsAINSE.Tables[0].Rows[0]["AINSEDate"].ToString();
            ddlLOB.SelectedValue = dsAINSE.Tables[0].Rows[0]["Lob_Id"].ToString();
            ddlBranch.SelectedText = dsAINSE.Tables[0].Rows[0]["Location_Name"].ToString();
            ddlBranch.SelectedValue = dsAINSE.Tables[0].Rows[0]["Location_ID"].ToString();
            ddlInsuranceDoneBy.SelectedValue = dsAINSE.Tables[0].Rows[0]["Ins_Done_By"].ToString();
            FunPriLoadDependentLOV();
            ddlMLA.SelectedText = dsAINSE.Tables[0].Rows[0]["Panum"].ToString();
            ddlMLA.SelectedValue = dsAINSE.Tables[0].Rows[0]["PA_SA_REF_ID"].ToString();
            
            DropDownListInsType.SelectedValue = dsAINSE.Tables[0].Rows[0]["Insurance_Type"].ToString();
            //if (ddlSLA.Items.Count > 1)
            //{
            //    lblSLA.CssClass = "styleReqFieldLabel";
            //    rfvSubAccount.Enabled = true;

            //}
            //else
            //{
            //    lblSLA.CssClass = "styleDisplayLabel";
            //    rfvSubAccount.Enabled = false;
            //}
            ddlSLA.SelectedText = dsAINSE.Tables[0].Rows[0]["Sanum"].ToString();
            ddlSLA.SelectedValue = dsAINSE.Tables[0].Rows[0]["Tranche_No"].ToString();
           
            //txtInsuranceCompanyName.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Name"].ToString();
            //txtInsuranceCompanyAddress.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Address"].ToString();
            //txtInsuranceCompanyAddress2.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Address2"].ToString();
            //txtInsCompanyCity.SelectedItem.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_City"].ToString();
            //txtInsCompanyCountry.SelectedItem.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Country"].ToString();
            //txtInsCompanyState.SelectedItem.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_State"].ToString();
            //txtMobile.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Mobile"].ToString();
            //txtPINCode.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Pin"].ToString();
            //txtTelephone.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Telephone"].ToString();
            //txtEmailId.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Email_ID"].ToString();
            //txtWebsite.Text = dsAINSE.Tables[0].Rows[0]["Ins_Company_Web_Site"].ToString();
            FunPriLoadInstallmentDate();
            if (!string.IsNullOrEmpty(dsAINSE.Tables[0].Rows[0]["InstallmentFrom"].ToString()))
            {
                ddlInstallmentFrom.SelectedValue = dsAINSE.Tables[0].Rows[0]["InstallmentFrom"].ToString();
            }
            if (!string.IsNullOrEmpty(dsAINSE.Tables[0].Rows[0]["InstallmentTo"].ToString()))
            {
                ddlInstallmentTo.SelectedValue = dsAINSE.Tables[0].Rows[0]["InstallmentTo"].ToString();
            }
            FunPriLoadPayType();
            ddlPayType.SelectedValue = dsAINSE.Tables[0].Rows[0]["Pay_Type"].ToString();
            if (dsAINSE.Tables[0].Rows[0]["Ins_Company_Id"].ToString() != "")
            {
                ddlPayTo.SelectedValue = dsAINSE.Tables[0].Rows[0]["Ins_Company_Id"].ToString();
            }
            FunPriLoadEntityDetails();
            hdnCustomerID.Value = dsAINSE.Tables[0].Rows[0]["Customer_ID"].ToString();
            txtTestforCustomer.Text = hdnCustomerID.Value;
            FunPubQueryExistCustomerList(Convert.ToInt32(hdnCustomerID.Value));
            ViewState["DT_AssetInsEntryDetails"] = (DataTable)dsAINSE.Tables[1];
            if (dsAINSE.Tables[1].Rows.Count != 0)
            {
                gvCurrentInsurance.DataSource = dsAINSE.Tables[1];
                gvCurrentInsurance.DataBind();
                gvCurrentInsurance.Visible = true;
                //ScriptManager.RegisterStartupScript(this, GetType(), "te", "ReHeight(" + (Convert.ToInt64(((dsAINSE.Tables[1].Rows.Count + 1) * 20)) + 80).ToString() + ");Resize();", true);
            }
            CalculatePremium();
            FunPriGenerateNewPolicyRow();
            //FunPriLoadAssetDescription();
            //FunPubSetFooterRowVisibility();
            //TODO: Need to check
            ViewState["DT_History_Policy"] = dsAINSE.Tables[4];
            if (dsAINSE.Tables[2].Rows.Count > 0)
            {
                pnlInsuranceHistory.Visible = true;
                gvInsuranceHistory.DataSource = dsAINSE.Tables[2];
                gvInsuranceHistory.DataBind();
            }
            else
                pnlInsuranceHistory.Visible = false;

            //gvInsuranceHistory.DataSource = dsAINSE.Tables[2];
            //gvInsuranceHistory.DataBind();
            FunPriLoadInstallment();
            //if (dsAINSE.Tables[3].Rows.Count > 0)
            //{
            //    lblTotalPremium.Text = dsAINSE.Tables[3].Rows[0][0].ToString();
            //}
            //if (lblTotalPremium.Text != "")
            //{
            //    ViewState["TotalPremium"] = lblTotalPremium.Text;
            //}
        }
    }
    #endregion

    protected void btnFAssDesc_Click(object sender, EventArgs e)
    {
        FunProFAssignApprover(sender, e);
    }


    
    protected void btnCancelRange_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderDEVApprover.Hide();
    }

    protected void FunProClearControls(bool ClearCustomer)
    {
        if (ClearCustomer)
        {
            hdnCustomerID.Value = string.Empty;
            S3GCustomerAddress1.ClearCustomerDetails();
            txtTestforCustomer.Text = "";
            ucCustomerCodeLov.FunPubClearControlValue();
        }

        //txtInsuranceCompanyName.Text =
        //txtInsuranceCompanyAddress.Text =
        //txtInsuranceCompanyAddress2.Text = string.Empty;
        //txtInsCompanyCity.SelectedValue =
        //txtInsCompanyCountry.SelectedValue =
        //txtInsCompanyState.SelectedValue = "0";
        //txtMobile.Text =
        //txtPINCode.Text =
        //txtTelephone.Text =
        //txtEmailId.Text =
        //txtWebsite.Text = string.Empty;

        ddlPayTo.SelectedValue = "0";
        S3GCompanyAddress.ClearCustomerDetails();
        lblPayType.Visible = lblHeaderInstallmentTo.Visible = lblHeaderInstallmentFrom.Visible =
        ddlPayType.Visible = ddlInstallmentFrom.Visible = ddlInstallmentTo.Visible = false;
        rfvInstallmentfrom.Enabled = rfvInstallmentto.Enabled = false;

        ViewState["DT_AssetInsEntryDetails"] = null;
        gvInsuranceHistory.DataSource = null;
        gvInsuranceHistory.DataBind();
        lblTotalPremium.Text = "0";
    }


    protected void FunProInitializeGrid()
    {
        DataTable dtAssetInsEntry = new DataTable();

        dtAssetInsEntry = new DataTable();
        dtAssetInsEntry.Columns.Add("Serial_Number");
        dtAssetInsEntry.Columns.Add("AssetDescription");
        dtAssetInsEntry.Columns.Add("AssetID");
        dtAssetInsEntry.Columns.Add("DetailsId");
        dtAssetInsEntry.Columns.Add("PolicyType");
        dtAssetInsEntry.Columns.Add("PolicyTypeID");
        dtAssetInsEntry.Columns.Add("AssetNumber");
        dtAssetInsEntry.Columns.Add("MachineNo");
        dtAssetInsEntry.Columns.Add("PolicyNo");
        dtAssetInsEntry.Columns.Add("PolicyDate", typeof(DateTime));
        dtAssetInsEntry.Columns.Add("ValidTillDate", typeof(DateTime));
        dtAssetInsEntry.Columns.Add("PolicyValue", System.Type.GetType("System.Decimal"));
        dtAssetInsEntry.Columns.Add("Premium", System.Type.GetType("System.Decimal"));

        DataRow drEmptyRow;
        drEmptyRow = dtAssetInsEntry.NewRow();
        drEmptyRow["Serial_Number"] = "0";
        drEmptyRow["AssetDescription"] = "0";
        drEmptyRow["AssetID"] = "0";
        drEmptyRow["DetailsId"] = "0";
        drEmptyRow["PolicyType"] = "0";
        drEmptyRow["PolicyTypeID"] = "0";
        drEmptyRow["AssetNumber"] = "0";
        drEmptyRow["MachineNo"] = "0";
        drEmptyRow["PolicyNo"] = "0";
        drEmptyRow["PolicyDate"] = DateTime.Now;
        drEmptyRow["ValidTillDate"] = DateTime.Now;
        drEmptyRow["PolicyValue"] = 0;
        drEmptyRow["Premium"] = 0;
        dtAssetInsEntry.Rows.Add(drEmptyRow);

        gvCurrentInsurance.DataSource = dtAssetInsEntry;
        gvCurrentInsurance.DataBind();
        gvCurrentInsurance.Visible=true;
        gvCurrentInsurance.Rows[0].Visible = false;
        //FunPubSetFooterRowVisibility();

        dtAssetInsEntry.Rows.Clear();
        ViewState["DT_AssetInsEntryDetails"] = dtAssetInsEntry;
    }

    #region [UserAuthorization]
    ////This is used to implement User Authorization
    private void FunPriDisableControls(int intModeID)
    {
        S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();

        try
        {

            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyId.ToString());
                    Procparam.Add("@AINSEId", intAINSEId.ToString());
                    ViewState["AllowModify"] = objS3GAdminServicesClient.FunGetScalarValue("S3G_INS_AllowModifyAssetInsEntry", Procparam);
                    // Allow = 0;
                    Procparam = null;

                    //FunPriLoadLOV();
                    //FunPriLoadPayType();
                    FunPriLoadAINSEDetails();
                    if (bClearList)
                    {
                        ddlLOB.ClearDropDownList();
                        //ddlBranch.ClearDropDownList();
                        ddlBranch.Enabled = false;
                       
                    }
                    //if (ddlPayTo.Items.Count > 0)
                    //{
                    //    ddlPayTo.ClearDropDownList();
                    //}
                    if (ddlInstallmentFrom.Items.Count > 0)
                    {
                        ddlInstallmentFrom.ClearDropDownList();
                    }
                    if (ddlInstallmentTo.Items.Count > 0)
                    {
                        ddlInstallmentTo.ClearDropDownList();
                    }
                    ddlPayType.ClearDropDownList();
                    ddlInsuranceDoneBy.ClearDropDownList();

                    //txtInsCompanyCity.DropDownStyle = txtInsCompanyState.DropDownStyle = txtInsCompanyCountry.DropDownStyle
                    //  = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                    //txtInsCompanyCity.ClearDropDownList();
                    //txtInsCompanyState.ClearDropDownList();
                    //txtInsCompanyCountry.ClearDropDownList();

                    txtAINSEDate.ReadOnly =
                        //txtInsuranceCompanyName.ReadOnly = txtInsuranceCompanyAddress.ReadOnly =
                        //txtInsuranceCompanyAddress2.ReadOnly =
                        //txtMobile.ReadOnly = txtPINCode.ReadOnly =
                        //txtTelephone.ReadOnly = txtEmailId.ReadOnly =
                        //txtInsCompanyCity.ReadOnly = txtInsCompanyCountry.ReadOnly = txtInsCompanyState.ReadOnly =
                        //txtWebsite.ReadOnly = true;
                    btnClear.Enabled = false;
                    ucCustomerCodeLov.ButtonEnabled = false;

                    //if (ViewState["AllowModify"] != null)
                    //{
                    //    if (ViewState["AllowModify"].ToString() == "0")
                    //    {
                    //        Utility.ClearDropDownList(ddlInstallmentFrom);
                    //        Utility.ClearDropDownList(ddlInstallmentTo);
                    //        //gvCurrentInsurance.FooterRow.Visible = false;

                    //    }
                    //}
                    //int Allow = 0;

                    //Procparam = new Dictionary<string, string>();
                    //Procparam.Add("@Company_ID", intCompanyId.ToString());
                    //Procparam.Add("@AINSEId", intAINSEId.ToString());

                    //Allow =Convert .ToInt32 ( objS3GAdminServicesClient.FunGetScalarValue("S3G_INS_AllowModifyAssetInsEntry", Procparam));

                    //Procparam = null;
                    //DropDownListInsType.Enabled = false;
                    //DropDownListInsType_SelectedIndexChange(DropDownListInsType, EventArgs.Empty);
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                     ddlMLA.ReadOnly = true;
                     ddlSLA.ReadOnly = true;
                    break;

                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    //FunPriLoadLOV();
                    FunPriLoadAINSEDetails();
                    ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    ddlBranch.Enabled = false;
                    ddlMLA.ReadOnly = true;
                    ddlSLA.ReadOnly = true;
                    ddlPayType.ClearDropDownList();
                    ddlInsuranceDoneBy.ClearDropDownList();
                   if (ddlPayTo.Items.Count > 0)
                    {
                        ddlPayTo.ClearDropDownList();
                    }
                    if (ddlInstallmentFrom.Items.Count > 0)
                    {
                        ddlInstallmentFrom.ClearDropDownList();
                    }
                    if (ddlInstallmentTo.Items.Count > 0)
                    {
                        ddlInstallmentTo.ClearDropDownList();
                    }
                    txtAINSEDate.ReadOnly = true;
                    //txtInsuranceCompanyName.ReadOnly = txtInsuranceCompanyAddress.ReadOnly 
                    //  = txtInsuranceCompanyAddress2.ReadOnly = txtEmailId.ReadOnly = txtWebsite.ReadOnly
                    //  = txtMobile.ReadOnly = txtPINCode.ReadOnly = txtTelephone.ReadOnly = true;
                  
                    ucCustomerCodeLov.ButtonEnabled = false;
                    //gvCurrentInsurance.Columns[8].Visible =
                    // gvCurrentInsurance.FooterRow.Visible = false;
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;

                    foreach (GridViewRow grow in gvCurrentInsurance.Rows)
                    {
                       // Button btnFAssDesc = (Button)grow.FindControl("btnFAssDesc");
                        DropDownList ddlFPolicyType = (DropDownList)grow.FindControl("ddlFPolicyType");
                        TextBox txtPolicyNumber = (TextBox)grow.FindControl("txtPolicyNumber");
                        TextBox txtPolicyDate = (TextBox)grow.FindControl("txtPolicyDate");
                        TextBox txtValidTill = (TextBox)grow.FindControl("txtValidTill");
                        TextBox txtLoss_Payee_Details = (TextBox)grow.FindControl("txtLoss_Payee_Details");
                        TextBox txtPolicyValue = (TextBox)grow.FindControl("txtPolicyValue");
                        TextBox txtPremium = (TextBox)grow.FindControl("txtPremium");
                        TextBox txtFRemarks = (TextBox)grow.FindControl("txtFRemarks");
                        AjaxControlToolkit.CalendarExtender CEPolicyDate = (AjaxControlToolkit.CalendarExtender)grow.FindControl("CEPolicyDate");
                        AjaxControlToolkit.CalendarExtender calValidDate = (AjaxControlToolkit.CalendarExtender)grow.FindControl("calValidDate");
                        CEPolicyDate.Enabled = false;
                        calValidDate.Enabled = false;

                        //btnFAssDesc.Enabled = false;
                        ddlFPolicyType.ClearDropDownList();
                        txtPolicyNumber.ReadOnly = true;
                        txtPolicyDate.ReadOnly = true;
                        txtValidTill.ReadOnly = true;
                        txtLoss_Payee_Details.ReadOnly = true;
                        txtPolicyValue.ReadOnly = true;
                        txtPremium.ReadOnly = true;
                        txtFRemarks.ReadOnly = true;
                    }
                  
                    btnSaveRange.Enabled = false;
                    
                    //DropDownListInsType.Enabled = false;
                    //DropDownListInsType_SelectedIndexChange(DropDownListInsType, EventArgs.Empty);
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }
                    break;
             }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objS3GAdminServicesClient.Close();
        }

    }
    private bool FunPriValidateAccountDetails()
    {
        DtPolicyDetails = (DataTable)ViewState["AccountDetails"];

        if (ViewState["AccountDetails"] == null)
        {
            strAlert = strAlert.Replace("__ALERT__", "Enter atleast one Account Details By Select a Customer");
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            return false;
        }
        if (DtPolicyDetails.Rows[0]["PrimeAccountNo"].Equals("0"))
        {
            strAlert = strAlert.Replace("__ALERT__", "Enter a Account Details ");
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            return false;
        }

        return true;
    }

    #endregion

    #region [ButtonEvents]

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {

        try
        {
            hdnCustomerID.Value = "";

            HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCID != null && Convert.ToString(hdnCID.Value) != "")
            {
                lblTotalPremium.Text = "0";
                ddlSLA.Clear();
                ddlInsuranceDoneBy.SelectedValue = "0";
                
                FunProClearControls(false);
                //ddlLOB.SelectedValue = "0";
                //ddlBranch.SelectedValue = "0";
                S3GCustomerAddress1.ClearCustomerDetails();
                txtTestforCustomer.Text = "";
                FunPubQueryExistCustomerList(Convert.ToInt32(hdnCID.Value));
                txtTestforCustomer.Text = hdnCID.Value;
                hdnCustomerID.Value = hdnCID.Value.ToString();
                FunPriLoadDependentLOV();
                FunPriLoadInsurance();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [System.Web.Services.WebMethod]
    public static string[] GetTranche(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
        if (obj_Page.hdnCustomerID.Value != "")
        {
            Procparam.Add("@Cust_ID", obj_Page.hdnCustomerID.Value);
        }
        else
        {
            Procparam.Add("@Cust_ID", "");
        }
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_AIE_GetTranchDtlLocust", Procparam));
        return suggetions.ToArray();

    }


    [System.Web.Services.WebMethod]
    public static string[] GetRSBasedTranche(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
        if (obj_Page.hdnCustomerID.Value != "")
        {
            Procparam.Add("@Cust_ID", obj_Page.hdnCustomerID.Value);
        }
        else
        {
            Procparam.Add("@Cust_ID", "");
        }
        if (obj_Page.ddlSLA.SelectedText != "")     
        {
            Procparam.Add("@Tranche_ID", obj_Page.ddlSLA.SelectedValue);
        }
        else
        {
            Procparam.Add("@Tranche_ID", "");
        }
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_AIE_GetRSBasedTranche", Procparam));
        return suggetions.ToArray();
    }


    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlInsuranceDoneBy.SelectedValue = "0";
       // FunProClearControls(true);
        FunPriLoadInsurance();
        if (ddlMLA.SelectedValue != "0")
        {
            //FunPriLoadSLA();
            //FunPriLoadCustomerDetails();
            FunPriLoadInstallmentDate();
           // FunPriLoadLobBranch();
        }
    }


   

    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlInsuranceDoneBy.SelectedValue = "0";
        //FunProClearControls(false);
        FunPriLoadInsurance();
        if (ddlSLA.SelectedText!="")
        {
            //FunPriLoadAssetDescription();
            FunPriLoadInstallmentDate();
            LoadHistoryDetailsInCreate();
            
        }
    }

    private void FunPriLoadInsurance()
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        DataSet dt = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Userid", intUserId.ToString());
        //if (ddlMLA.SelectedValue != "0" || ddlMLA.SelectedValue != "")
        //{
        //    Procparam.Add("@RS_NO", ddlMLA.SelectedValue);
        //}
        //else{
        //    Procparam.Add("@RS_NO", "0");
        //}

        if (ddlSLA.SelectedValue != "0")
        {
            Procparam.Add("@Tranche_No", ddlSLA.SelectedValue);
        }
        else{
            Procparam.Add("@Tranche_No", "0");
        }
         if (strMode != "Q" && strMode != "M")
         {
             Procparam.Add("@AIENO", "0");
             dt = Utility.GetDataset("S3G_ORG_GetPolicyforCreate", Procparam);
         }
         else
         {
             Procparam.Add("@AIENO", intAINSEId.ToString());
             dt = Utility.GetDataset("S3G_ORG_GETACCINS", Procparam);
         }


        
         //gvCurrentInsurance.Visible = true;
         if (dt.Tables.Count != 0)
         {
             gvCurrentInsurance.DataSource = dt;
             gvCurrentInsurance.DataBind();
             CalculatePremium();
             ViewState["DT_AssetInsEntryDetails"] = dt;
         }
         else
         {
             gvCurrentInsurance.DataSource = null;
             gvCurrentInsurance.DataBind();
             CalculatePremium();
             ViewState["DT_AssetInsEntryDetails"] = dt;
         }
        
         
    }

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../Insurance/S3GInsTranslander.aspx?Code=AINE", false);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadLOV();
            ucCustomerCodeLov.FunPubClearControlValue();
            S3GCustomerAddress1.ClearCustomerDetails();
            txtTestforCustomer.Text = "";
            S3GCompanyAddress.ClearCustomerDetails();
            ddlPayTo.SelectedIndex = 0;
            ddlBranch.Clear();
            ddlMLA.Clear();
            ddlSLA.Clear();
            ddlInsuranceDoneBy.SelectedIndex = 0;
            ViewState["DT_AssetInsEntryDetails"] = null;
            gvInsuranceHistory.DataSource = null;
            gvInsuranceHistory.DataBind();

            gvCurrentInsurance.DataSource = null;
            gvCurrentInsurance.DataBind();


            pnlInsuranceHistory.Visible = false;
            lblTotalPremium.Text = "0";
            ddlPayType.Visible = lblHeaderInstallmentFrom.Visible = lblHeaderInstallmentTo.Visible =
            ddlInstallmentFrom.Visible = ddlInstallmentTo.Visible = lblPayType.Visible = false;
            rfvInstallmentfrom.Enabled = rfvInstallmentto.Enabled = false;

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void FunProFAssignApprover(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        Label lblRSNo = (Label)gvr.FindControl("lblRSNo");
        Label lblInvid = (Label)gvr.FindControl("lblInvid");
        ViewState["TRANCHE_NO"] = ddlSLA.SelectedValue.ToString();
        ViewState["PA_SA_REF_ID"] = lblRSNo.Text.Trim();
        DataTable dtEmptyTable = new DataTable();
        DataRow drEmptyRow;

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@CompanyId", intCompanyId.ToString());
        if (ddlMLA.SelectedText!="")
        {
             Procparam.Add("@PA_SA_REF_ID", ddlMLA.SelectedValue.ToString());
        }
        else
        {
             Procparam.Add("@PA_SA_REF_ID", "0");
        }
       
        Procparam.Add("@TRANCHE_NO", ViewState["TRANCHE_NO"].ToString());
        DataSet dsAINSE = Utility.GetDataset("S3G_INS_GETINVDET", Procparam);
        if (dsAINSE.Tables[0].Rows.Count == 0)
        {
            dtEmptyTable.Columns.Add("PANUM");
            dtEmptyTable.Columns.Add("PA_SA_REF_ID");
            dtEmptyTable.Columns.Add("INVOICE_NO");
            dtEmptyTable.Columns.Add("INVOICE_ID");
            dtEmptyTable.Columns.Add("INVOICE_DATE");
            dtEmptyTable.Columns.Add("ASSET_CATEGORY");
            dtEmptyTable.Columns.Add("ASSET_TYPE");
            dtEmptyTable.Columns.Add("ASSET_DESCRIPTION");
            dtEmptyTable.Columns.Add("ASSET_VALUE");
            dtEmptyTable.Columns.Add("ACTIVE");
            drEmptyRow = dtEmptyTable.NewRow();
            drEmptyRow["PANUM"] = "";
            drEmptyRow["PA_SA_REF_ID"] = "";
            drEmptyRow["INVOICE_NO"] = "";
            drEmptyRow["INVOICE_ID"] = "";
            drEmptyRow["INVOICE_DATE"] = "";
            drEmptyRow["ASSET_CATEGORY"] = "";
            drEmptyRow["ASSET_TYPE"] = "";
            drEmptyRow["ASSET_DESCRIPTION"] = "";
            drEmptyRow["ASSET_VALUE"] = "";
            drEmptyRow["ACTIVE"] = "";
            dtEmptyTable.Rows.Add(drEmptyRow);
            grvRange.DataSource = dtEmptyTable;
            grvRange.DataBind();
            grvRange.Rows[0].Visible = false;
        }
        else
        {
            grvRange.DataSource = dsAINSE;
            grvRange.DataBind();
        }

        if (lblInvid.Text.Trim()!="")
        {
            string GetInvoiceDetails = lblInvid.Text.Trim();
            string[] invID = GetInvoiceDetails.Split(',');
             
            int RowCount = 0;
            int nCount = 0;
            int i = 0;
           foreach (string word1 in invID)
           {
               string GetSplit = invID[i];
               string[] SplitinvID = GetSplit.Split('~');
               foreach (string word in SplitinvID)
               {
                   foreach (GridViewRow grdRow in grvRange.Rows)
                   {
                       Label lblChkSelect = (Label)grdRow.FindControl("lblChkSelect");
                       Label lblInvoiceID = (Label)grdRow.FindControl("lblInvoiceID");
                       CheckBox chkSelect = (CheckBox)grdRow.FindControl("chkSelect");
                       if (lblInvoiceID.Text.Trim() == word.ToString())
                       {
                           chkSelect.Checked = true;
                       }
                   }
                   break;
               }
               i = i + 1;
           }
            RowCount = grvRange.Rows.Count;
            foreach (GridViewRow grdRow in grvRange.Rows)
            {
                CheckBox chkSelect = (CheckBox)grdRow.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    nCount = nCount + 1 ;
                }
            }

            CheckBox chkAll = (CheckBox)grvRange.HeaderRow.FindControl("chkAll");
            if (nCount == RowCount)
            {
                chkAll.Checked = true;
            }
            else
            {
                chkAll.Checked = false;
            }
        }

        if (Request.QueryString["qsMode"].ToString() == "Q")
        {
            foreach (GridViewRow grow in grvRange.Rows)
            {
                // Button btnFAssDesc = (Button)grow.FindControl("btnFAssDesc");
                CheckBox chkSelect = (CheckBox)grow.FindControl("chkSelect");
                chkSelect.Enabled = false;
            }
            CheckBox chkAll = (CheckBox)grvRange.HeaderRow.FindControl("chkAll");
            chkAll.Enabled = false;
        }

        ViewState["InvoiceTable"] = dsAINSE;
        PnlApprover.Visible = true;
        ModalPopupExtenderDEVApprover.Show();
    }

    protected void btnSaveRange_OnClick(object sender, EventArgs e)
    {
        decimal dcResult = 0;
        int nCount = 0;
        string InvID = "";
        string PANUM = "";
        try
        {
            foreach (GridViewRow grdRow in grvRange.Rows)
            {
                CheckBox chkSelect = (CheckBox)grdRow.FindControl("chkSelect");
                Label lblAssValue = (Label)grdRow.FindControl("lblAssValue");
                Label lblPASAREFID = (Label)grdRow.FindControl("lblPASAREFID");
                Label lblInvoiceID = (Label)grdRow.FindControl("lblInvoiceID");
                if (chkSelect.Checked)
                {
                    nCount = nCount + 1;
                    if (lblAssValue.Text != "")
                    {
                        dcResult = dcResult + Convert.ToDecimal(lblAssValue.Text);
                        if (InvID == "")
                        {
                            InvID = lblInvoiceID.Text.Trim() + "~" + lblPASAREFID.Text.Trim();
                        }
                        else
                        {
                            InvID = InvID + "," + lblInvoiceID.Text.Trim() + "~" + lblPASAREFID.Text.Trim();
                        }

                        if (PANUM == "")
                        {
                            PANUM = lblPASAREFID.Text.Trim();
                        }
                        else
                        {
                            PANUM = PANUM + "," + lblPASAREFID.Text.Trim();
                        }
                    }
                }
            }

            if (nCount == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "Select atleast one Invoice");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                ModalPopupExtenderDEVApprover.Show();
                return;
            }

            ViewState["AssetValue"] = dcResult;
            ViewState["InvoiceID"] = InvID;
            ViewState["PANUM"] = PANUM;


            foreach (GridViewRow gRow in gvCurrentInsurance.Rows)
            {
                Label lblRSNo = (Label)gRow.FindControl("lblRSNo");
                Label lblAssetValue = (Label)gRow.FindControl("lblAssetValue");
                Label lblInvid = (Label)gRow.FindControl("lblInvid");
                Label lblPANUM = (Label)gRow.FindControl("lblPANUM");
                //if (lblRSNo.Text == ViewState["PA_SA_REF_ID"].ToString())
                //{
                lblAssetValue.Text = ViewState["AssetValue"].ToString();
                if (ViewState["InvoiceID"] != null)
                {
                    lblInvid.Text = ViewState["InvoiceID"].ToString();
                    lblPANUM.Text = ViewState["PANUM"].ToString();
                }
                //}
            }
            //gvCurrentInsurance.DataBind();
            ModalPopupExtenderDEVApprover.Hide();
            // objInsuranceClient = new InsuranceMgtServicesReference.InsuranceMgtServicesClient();

            //try
            //{
            //    S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable objAssetInsuranceEntryTable = new S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable();
            //    S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryRow objAssetInsuranceRow = objAssetInsuranceEntryTable.NewS3G_INS_AssetInsuranceEntryRow();

            //    objAssetInsuranceRow.XmlInvoiceDetails = gvCurrentInsurance.FunPubFormXml(true);
            //    objAssetInsuranceEntryTable.AddS3G_INS_AssetInsuranceEntryRow(objAssetInsuranceRow);

            //    SerializationMode SerMode = SerializationMode.Binary;
            //    byte[] objByteAssetInsuranceEntryTable = ClsPubSerialize.Serialize(objAssetInsuranceEntryTable, SerMode);
            //    string strAINSENO = "";
            //    intResult = objInsuranceClient.FunPubUpdateInvoiceEntryDetails(SerMode, objByteAssetInsuranceEntryTable);
            //    FunPriLoadInsurance();
            //    ModalPopupExtenderDEVApprover.Hide();
            //}
            //catch (Exception ex)
            //{
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }

    // Code added for Call Id : 4232 Strat 

    protected void Button1_Click(object sender, EventArgs e)
    {
        objInsuranceClient = new InsuranceMgtServicesReference.InsuranceMgtServicesClient();

        try
        {
            int intResult = 0;

            S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable objAssetInsuranceEntryTable = new S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable();
            S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryRow objAssetInsuranceRow = objAssetInsuranceEntryTable.NewS3G_INS_AssetInsuranceEntryRow();
            objAssetInsuranceRow.Company_ID = intCompanyId;
            objAssetInsuranceRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objAssetInsuranceRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objAssetInsuranceRow.Account_Ins_ID = Convert.ToInt64(intAINSEId);
            objAssetInsuranceRow.Ins_Done_By = ddlInsuranceDoneBy.SelectedValue;
            objAssetInsuranceRow.Ins_Company_Name = ""; //txtInsuranceCompanyName.Text;
            objAssetInsuranceRow.Ins_Company_Address = "";// txtInsuranceCompanyAddress.Text;

            objAssetInsuranceRow.Ins_Company_Address2 = "";// txtInsuranceCompanyAddress2.Text;
            objAssetInsuranceRow.Ins_Company_City = "";//txtInsCompanyCity.Text;
            objAssetInsuranceRow.Ins_Company_Country = "";//txtInsCompanyCountry.Text;
            objAssetInsuranceRow.Ins_Company_Email_ID = "";//txtEmailId.Text;
            if (ddlPayTo.Visible)
            {
                objAssetInsuranceRow.Ins_Company_Id = ddlPayTo.SelectedValue;
            }
            //if (txtMobile.Text != "")
            //{
            objAssetInsuranceRow.Ins_Company_Mobile = 0;//Convert.ToInt64(txtMobile.Text);
            //}
            objAssetInsuranceRow.Ins_Company_Pin = "";//txtPINCode.Text;
            objAssetInsuranceRow.Ins_Company_State = "";//txtInsCompanyState.Text;
            objAssetInsuranceRow.Ins_Company_Telephone = "";//txtTelephone.Text;
            objAssetInsuranceRow.Ins_Company_Web_Site = "";//txtWebsite.Text;

            objAssetInsuranceRow.PANum = ddlMLA.SelectedText;
            objAssetInsuranceRow.SANum = ddlSLA.SelectedText;
            objAssetInsuranceRow.RS_No = ddlMLA.SelectedValue;
            objAssetInsuranceRow.Tranche_No = ddlSLA.SelectedValue;
            //objAssetInsuranceRow.SANum = (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedText;
            objAssetInsuranceRow.Account_Link_Key = 1;
            if (intCustomer > 0)
                objAssetInsuranceRow.Customer_ID = intCustomer;

            else
                objAssetInsuranceRow.Customer_ID = Convert.ToInt32(hdnCustomerID.Value);
            objAssetInsuranceRow.UserId = intUserID.ToString();

            if (ddlPayType.Visible == true)
            {
                objAssetInsuranceRow.Pay_Type = Convert.ToInt32(ddlPayType.SelectedValue);
                if (ddlPayType.SelectedValue == "2")
                {
                    objAssetInsuranceRow.Installment_From_Date = Utility.StringToDate(ddlInstallmentFrom.SelectedItem.Text);
                    objAssetInsuranceRow.Installment_To_Date = Utility.StringToDate(ddlInstallmentTo.SelectedItem.Text);
                }
                else
                {
                    objAssetInsuranceRow.Installment_From_Date = Utility.StringToDate("01/01/1900");
                    objAssetInsuranceRow.Installment_To_Date = Utility.StringToDate("01/01/1900");
                }
            }
            else
            {
                objAssetInsuranceRow.Pay_Type = 0;
            }
            objAssetInsuranceRow.Insurance_Type = DropDownListInsType.SelectedValue;
            objAssetInsuranceRow.XmlPolicyDetails = ((DataTable)ViewState["ObjDT"]).FunPubFormXml(true);
            objAssetInsuranceEntryTable.AddS3G_INS_AssetInsuranceEntryRow(objAssetInsuranceRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] objByteAssetInsuranceEntryTable = ClsPubSerialize.Serialize(objAssetInsuranceEntryTable, SerMode);
            string strAINSENO = "";
            if (intAINSEId == 0)
            {
                intResult = objInsuranceClient.FunPubCreateAssetEntryDetails(out strAINSENO, SerMode, objByteAssetInsuranceEntryTable);
            }
            else
            {
                intResult = objInsuranceClient.FunPubUpdateAssetEntryDetails(SerMode, objByteAssetInsuranceEntryTable);
            }
            if (intResult == 0)
            {
                //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                if (intAINSEId == 0)
                {
                    strAlert = "AINSE Number " + strAINSENO + " created successfully";
                    strAlert += @"\n\nWould you like to create one more AINSE?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
                else
                {
                    string strModAlert = "Asset Insurance Entry Details " + strAINSENO + " updated successfully";
                    strAlert = strAlert.Replace("__ALERT__", strModAlert);
                }

            }
            else if (intResult == -1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Sequence for AINSE was not defined");
                strRedirectPageView = "";
            }
            else if (intResult == -2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Sequence exceeded");
                strRedirectPageView = "";
            }
            else if (intResult == 3)
            {
                strAlert = strAlert.Replace("__ALERT__", "Insurance Policy Details Already Exists");
                strRedirectPageView = "";
            }
            else if (intResult == 53)
            {
                strAlert = strAlert.Replace("__ALERT__", "Posting Failed - CashFlow Master Not Defined");
                strRedirectPageView = "";
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Error in creating AINSE");
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objInsuranceClient.Close();
        }
    }

    // Call Id : 4232 Start

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataSet datSet = new DataSet();
            try
            {
                datSet = (DataSet)ViewState["DT_AssetInsEntryDetails"];
                dtAssetInsEntry = datSet.Tables[0];
            }
           
            catch(Exception ex)
            {
                dtAssetInsEntry = (DataTable)ViewState["DT_AssetInsEntryDetails"];
            }

            DataTable ObjDT = new DataTable();
            ObjDT.Columns.Add("AssetDescription");
            ObjDT.Columns.Add("AssetID");
            ObjDT.Columns.Add("DetailsID");
            ObjDT.Columns.Add("AssetValue");
            ObjDT.Columns.Add("PolicyType");
            ObjDT.Columns.Add("Invoice_ID");
            ObjDT.Columns.Add("PolicyTypeID");
            ObjDT.Columns.Add("PA_SA_REF_ID");
            ObjDT.Columns.Add("PANUM");
            ObjDT.Columns.Add("POLICYNUMBER");
            ObjDT.Columns.Add("PolicyDate");
            ObjDT.Columns.Add("ValidTillDate");
            ObjDT.Columns.Add("PolicyValue");
            ObjDT.Columns.Add("LOSS_PAYEE_DETAILS");
            ObjDT.Columns.Add("Ins_Plan");
            ObjDT.Columns.Add("AssetNumber");
            ObjDT.Columns.Add("Premium");
            ObjDT.Columns.Add("REMARKS");
            ObjDT.Columns.Add("Payment_Request_No");
            ObjDT.Columns.Add("DetailsId");
            ObjDT.Columns.Add("CanEdit");


            foreach (GridViewRow drow in gvCurrentInsurance.Rows)
            {
                AjaxControlToolkit.CalendarExtender CEPolicyDate = drow.FindControl("CEPolicyDate") as AjaxControlToolkit.CalendarExtender;
                CEPolicyDate.Format = strDateFormat;
                AjaxControlToolkit.CalendarExtender calValidDate = drow.FindControl("calValidDate") as AjaxControlToolkit.CalendarExtender;
                calValidDate.Format = strDateFormat;
                DropDownList ddlFPolicyType = drow.FindControl("ddlFPolicyType") as DropDownList;
                Label lblAssetValue = drow.FindControl("lblAssetValue") as Label;
                Label lblPolicyTypeId = drow.FindControl("lblPolicyTypeId") as Label;
                TextBox txtPolicyDate = drow.FindControl("txtPolicyDate") as TextBox;
                TextBox txtLoss_Payee_Details = drow.FindControl("txtLoss_Payee_Details") as TextBox;
                TextBox txtFRemarks = drow.FindControl("txtFRemarks") as TextBox;
                TextBox txtPolicyNumber = drow.FindControl("txtPolicyNumber") as TextBox;
                TextBox txtValidTill = drow.FindControl("txtValidTill") as TextBox;
                TextBox txtPremium = drow.FindControl("txtPremium") as TextBox;
                TextBox txtPolicyValue = drow.FindControl("txtPolicyValue") as TextBox;
                TextBox txtNominee = drow.FindControl("txtNominee") as TextBox;
                TextBox txtInsPlan = drow.FindControl("txtInsPlan") as TextBox;
                Label lblPremium = drow.FindControl("lblPremium") as Label;
                Label lblPolicyValue = drow.FindControl("lblPolicyValue") as Label;
                Label lblRSNo = drow.FindControl("lblRSNo") as Label;
                Label lblInvid = drow.FindControl("lblInvid") as Label;
                Label lblRentalSchedNo = drow.FindControl("lblRentalSchedNo") as Label;

                DataRow dr_Alert = ObjDT.NewRow();
                dr_Alert["AssetDescription"] = "";
                dr_Alert["AssetID"] = "0";
                dr_Alert["DetailsID"] = "";
                dr_Alert["Invoice_ID"] = lblInvid.Text.Trim();
                dr_Alert["PolicyType"] = ddlFPolicyType.SelectedItem.ToString();
                dr_Alert["PolicyTypeID"] = ddlFPolicyType.SelectedValue.ToString();
                dr_Alert["PA_SA_REF_ID"] = lblRSNo.Text;
                dr_Alert["PANUM"] = lblRentalSchedNo.Text;
                dr_Alert["POLICYNUMBER"] = txtPolicyNumber.Text;
                if (txtPolicyDate.Text != "")
                {
                    dr_Alert["PolicyDate"] = DateTime.Parse(Utility.StringToDate(txtPolicyDate.Text).ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);
                }
                else
                {
                    dr_Alert["PolicyDate"] = "";
                }
                if (txtValidTill.Text != "")
                {
                    dr_Alert["ValidTillDate"] = DateTime.Parse(Utility.StringToDate(txtValidTill.Text).ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);
                }
                else
                {
                    dr_Alert["ValidTillDate"] = "";
                }
                dr_Alert["REMARKS"] = txtFRemarks.Text;
                dr_Alert["AssetValue"] = lblAssetValue.Text;
                dr_Alert["LOSS_PAYEE_DETAILS"] = txtLoss_Payee_Details.Text;
                dr_Alert["PolicyValue"] = txtPolicyValue.Text;
                if (txtPremium.Text != "")
                {
                    dr_Alert["PREMIUM"] = txtPremium.Text;
                }
                else
                {
                    dr_Alert["PREMIUM"] = "0";
                }
                dr_Alert["DetailsId"] = "";
                dr_Alert["AssetNumber"] = "0";
                dr_Alert["Payment_Request_No"] = "";
                dr_Alert["CanEdit"] = "";
                ObjDT.Rows.Add(dr_Alert);
            }



            //if (dtAssetInsEntry.Rows.Count == 1)
            //{
            //    if (dtAssetInsEntry.Rows[0]["Serial_Number"].ToString().Equals("0"))
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "Enter Current Insurance Policy Details in Grid");
            //        return;
            //    }
            //}
            if (ddlBranch.SelectedText.ToString().Trim() == "")
            {
                strAlert = strAlert.Replace("__ALERT__", "Select Location");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                return;
            }
            if (ddlSLA.SelectedText.ToString().Trim() == "")
            {
                strAlert = strAlert.Replace("__ALERT__", "Select Tranche No");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                return;
            }

            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            if (txtName != null)
            {
                if (txtName.Text == "")
                {
                    strAlert = strAlert.Replace("__ALERT__", "Select a Customer");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    return;
                }
            }

            foreach (GridViewRow grdRow in gvCurrentInsurance.Rows)
            {
                DropDownList ddlFPolicyType = (DropDownList)grdRow.FindControl("ddlFPolicyType");
                TextBox txtPolicyNumber = (TextBox)grdRow.FindControl("txtPolicyNumber");
                TextBox txtPolicyDate = (TextBox)grdRow.FindControl("txtPolicyDate");
                TextBox txtValidTill = (TextBox)grdRow.FindControl("txtValidTill");
                Label lblAssetValue = (Label)grdRow.FindControl("lblAssetValue");
                TextBox txtLoss_Payee_Details = (TextBox)grdRow.FindControl("txtLoss_Payee_Details");
                TextBox txtPolicyValue = (TextBox)grdRow.FindControl("txtPolicyValue");
                TextBox txtPremium = (TextBox)grdRow.FindControl("txtPremium");
                Label lblRentalSchedNo = (Label)grdRow.FindControl("lblRentalSchedNo");
                if (ddlFPolicyType.SelectedIndex == 0 && txtPolicyNumber.Text == "" && lblAssetValue.Text=="0.00" && txtPolicyDate.Text == "" && txtLoss_Payee_Details.Text == "" && txtPolicyValue.Text == "")
                {
                    Utility.FunShowAlertMsg(this, "Asset Value,Policy Type,Policy No,Policy Date,Loss Payee Details,Policy Value is mandatory");
                    return;
                }
                if (lblAssetValue.Text.ToString().StartsWith("0"))
                {
                    Utility.FunShowAlertMsg(this, "Asset value cannot be Zero");
                    return;
                }
                if (txtPolicyValue.Text.ToString().StartsWith("0"))
                {
                    Utility.FunShowAlertMsg(this, "Policy value cannot be Zero");
                    return;
                }
                //if (txtPremium.Text.ToString().StartsWith("0"))
                //{
                //    Utility.FunShowAlertMsg(this, "Premium value cannot be Zero for " + lblRentalSchedNo.Text);
                //    return;
                //}                
                if (txtPolicyValue.Text != "" && txtPremium.Text != "")
                {
                    if (Convert.ToDecimal(txtPolicyValue.Text) <= Convert.ToDecimal(txtPremium.Text))
                    {
                        txtPremium.Focus();
                        Utility.FunShowAlertMsg(this, "Premium Value should not be greater than or equal to the Policy Value");
                        return;
                    }
                }

                DateTime PolicyDate;
                DateTime ValidTill;
                if (txtPolicyDate.Text != "" && txtValidTill.Text != "")
                {
                    PolicyDate = Utility.StringToDate(DateTime.Parse(Utility.StringToDate(txtPolicyDate.Text).ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW));
                    ValidTill = Utility.StringToDate(DateTime.Parse(Utility.StringToDate(txtValidTill.Text).ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW));
                    if (PolicyDate > ValidTill)
                    {
                        txtPremium.Focus();
                        Utility.FunShowAlertMsg(this, "Policy Date cannot be greater than Valid Till Date");
                        return;
                    }
                }  
            }

            // Code Added For Call Id : 4232 Strat

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@XMLPolicyDetail", ObjDT.FunPubFormXml(true));
            Procparam.Add("@Account_Ins_ID", intAINSEId.ToString());
            string strCheck = Utility.GetTableScalarValue("S3G_INS_CheckAssetpolicyDetails", Procparam);
            ViewState["ObjDT"] = ObjDT;
            if (strCheck != "0")
            {
                strAlert = "Same Policy details is entered with " + strCheck + " Tranche";
                strAlert += @"\n\ndo you want to proceed?";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "if(confirm('" + strAlert + "')){ document.getElementById('" + Button1.ClientID + "').click();}else {}", true);
            }
            else
            {
                Button1_Click(sender, e);
            }

            // Call Id : 4232 End
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {

        }
    }
    #endregion

    #region [CustomerAddress]
    public static string SetCustomerAddress(string Address1, string Address2, string City, string State, string Country, string Pincode)
    {
        try
        {
            string strAddress = "";
            if (Address1.ToString() != "") strAddress += Address1.ToString() + System.Environment.NewLine;
            if (Address2.ToString() != "") strAddress += Address2.ToString() + System.Environment.NewLine;
            if (City.ToString() != "") strAddress += City.ToString() + System.Environment.NewLine;
            if (State.ToString() != "") strAddress += State.ToString() + System.Environment.NewLine;
            if (Country.ToString() != "") strAddress += Country.ToString() + System.Environment.NewLine;
            if (Pincode.ToString() != "") strAddress += Pincode.ToString();
            return strAddress;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region [CustomerDetails]
    private void FunPubQueryExistCustomerList(int CustomerID)
    {
        string strCustomerAddress = "";

        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Customer_Id", CustomerID.ToString());
            DataTable dtCustomerDetails = Utility.GetDefaultData("S3G_CLN_GetCustomerDetails", Procparam);
            if (dtCustomerDetails.Rows.Count > 0)
            {
                if (intAINSEId > 0)
                {
                    TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    txtName.Text = dtCustomerDetails.Rows[0]["Title"].ToString() + ' ' + dtCustomerDetails.Rows[0]["Customer_Name"].ToString();
                    txtName.Enabled = false;
                    txtName.ToolTip = txtName.Text;
                    Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
                    btnGetLOV.Visible = false;
                }
                strCustomerAddress = SetCustomerAddress(dtCustomerDetails.Rows[0]["comm_address1"].ToString(), dtCustomerDetails.Rows[0]["comm_address2"].ToString(), dtCustomerDetails.Rows[0]["comm_city"].ToString(), dtCustomerDetails.Rows[0]["comm_state"].ToString(), dtCustomerDetails.Rows[0]["comm_country"].ToString(), dtCustomerDetails.Rows[0]["comm_pincode"].ToString());
                S3GCustomerAddress1.SetCustomerDetails(dtCustomerDetails.Rows[0]["Customer_Code"].ToString(),
                    strCustomerAddress, dtCustomerDetails.Rows[0]["Customer_Name"].ToString(),
                     dtCustomerDetails.Rows[0]["Comm_Telephone"].ToString(),
                    dtCustomerDetails.Rows[0]["comm_mobile"].ToString(),
                    dtCustomerDetails.Rows[0]["comm_email"].ToString(),
                    dtCustomerDetails.Rows[0]["Comm_Website"].ToString());

            }
        }
        catch (Exception ex)
        {
            //    ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }



    }
    #endregion

    #region [ClearCustomerDetails]
    private void FunPriClearCusdetails()
    {
        try
        {
            S3GCustomerAddress1.ClearCustomerDetails();
            txtTestforCustomer.Text = "";

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region [DropdownlistEvents]

    //protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlInsuranceDoneBy.SelectedValue = "0";
    //    ddlMLA.Clear();
    //    ddlSLA.Items.Clear();
    //    FunProClearControls(true);
    //    FunPriLoadDependentLOV();

    //}

    
   
    //private void FunPriLoadLobBranch()
    //{
    //    Procparam = new Dictionary<string, string>();

    //    Procparam.Add("@Option", "17");

    //    Procparam.Add("@companyId", intCompanyId.ToString());

    //    if (ddlMLA.SelectedValue != "0")
    //    {
    //        Procparam.Add("@Panum", ddlMLA.SelectedValue);
    //    }

    //    DataTable dtMachineDetails = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);
    //    if (dtMachineDetails.Rows.Count > 0)
    //    {
    //        ddlLOB.SelectedValue = dtMachineDetails.Rows[0]["Lob_Id"].ToString();
    //        ddlBranch.SelectedValue = dtMachineDetails.Rows[0]["Location_ID"].ToString();
    //    }

    //}

    private void FunPriLoadCustomerDetails()
    {
        //Procparam = new Dictionary<string, string>();

        //Procparam.Add("@Option", "9");
        //if (ddlMLA.SelectedValue != "0")
        //{
        //    Procparam.Add("@Panum", ddlMLA.SelectedValue);
        //}

        //DataTable dtMachineDetails = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);
        //if (dtMachineDetails.Rows.Count > 0)
        //{
        //    hdnCustomerID.Value = dtMachineDetails.Rows[0]["Customer_ID"].ToString();
        //    S3GCustomerAddress1.SetCustomerDetails(dtMachineDetails.Rows[0]["Customer_Code"].ToString(),
        //       dtMachineDetails.Rows[0]["comm_Address1"].ToString() + "\n" +
        //       dtMachineDetails.Rows[0]["comm_Address2"].ToString() + "\n" +
        //       dtMachineDetails.Rows[0]["comm_city"].ToString() + "\n" +
        //       dtMachineDetails.Rows[0]["comm_state"].ToString() + "\n" +
        //       dtMachineDetails.Rows[0]["comm_country"].ToString() + "\n" +
        //       dtMachineDetails.Rows[0]["comm_pincode"].ToString(), dtMachineDetails.Rows[0]["Customer_Name"].ToString(),
        //       dtMachineDetails.Rows[0]["Comm_telephone"].ToString(),
        //       dtMachineDetails.Rows[0]["Comm_mobile"].ToString(),

        //       dtMachineDetails.Rows[0]["comm_email"].ToString(), dtMachineDetails.Rows[0]["comm_website"].ToString());
        //    TextBox txtName = ucCustomerCodeLov.FindControl("txtName") as TextBox;
        //    txtName.Text = dtMachineDetails.Rows[0]["Customer_Name"].ToString();
        //}
        //else
        //{
        //    Utility.FunShowAlertMsg(this, "No Records Found");
        //    ddlMLA.Clear();
        //    return;
        //}

    }

    private void FunPriLoadSLA()
    {
        //if (ddlMLA.SelectedValue != "0")
        //{
        //    Procparam = new Dictionary<string, string>();
        //    Procparam.Add("@OPTION", "7");
        //    Procparam.Add("@COMPANYID", Convert.ToString(intCompanyId));
        //    Procparam.Add("@PANUM", Convert.ToString(ddlMLA.SelectedValue));
        //    ddlSLA.BindDataTable("S3G_CLN_LOADLOV", Procparam, new string[] { "SANUM", "SANUM" });

        //    //if (ddlSLA.Items.Count > 1)
        //    //{
        //    //    rfvSubAccount.Enabled = true;
        //    //    rfvPolicySLA.Enabled = true;
        //    //    ddlSLA.Enabled = true;
        //    //}
        //    //else
        //    //{
        //    //    rfvSubAccount.Enabled = false;
        //    //    rfvPolicySLA.Enabled = false;
        //    //    ddlSLA.Enabled = false;
        //    //}


        //}

    }

   
    protected void ddlInsuranceDoneBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlPayTo.SelectedIndex = -1;
        //S3GCompanyAddress.ClearCustomerDetails();
        FunPriLoadPayType();

        lblHeaderInstallmentFrom.Visible = lblHeaderInstallmentTo.Visible = false;
        ddlInstallmentFrom.Visible = ddlInstallmentTo.Visible = false;
        rfvInstallmentfrom.Enabled = rfvInstallmentto.Enabled = false;

        //ViewState["DT_AssetInsEntryDetails"] = null;

        //dtAssetInsEntry = FunPriAssetInsEntryDataTable();

        //if (dtAssetInsEntry.Rows.Count == 0)
        //{
        //    FunPriInsertAssetInsEntryDataTable("", "", "", "", "", "", "", "", "", "-1", "-1", "0", "", "");
        //}

       // lblTotalPremium.Text = "0";
      
    }

  protected void ddlPayType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPayType.SelectedIndex > 0)
        {
            FunPriLoadInstallment();
        }
        else
        {
            lblHeaderInstallmentFrom.Visible = lblHeaderInstallmentTo.Visible = false;
            ddlInstallmentFrom.Visible = ddlInstallmentTo.Visible = false;
            rfvInstallmentfrom.Enabled = rfvInstallmentto.Enabled = false;
        }
    }
    private void FunPriLoadInstallment()
    {
        if (ddlPayType.SelectedItem.Text.ToUpper() == "BULLET")
        {
            rfvInstallmentfrom.Enabled = rfvInstallmentto.Enabled = false;

            lblHeaderInstallmentFrom.Visible = lblHeaderInstallmentTo.Visible = ddlInstallmentFrom.Visible = ddlInstallmentTo.Visible = false;
        }
        if (ddlPayType.SelectedItem.Text.ToUpper() == "STAGGERED")
        {
            lblHeaderInstallmentFrom.Visible = lblHeaderInstallmentTo.Visible = ddlInstallmentFrom.Visible = ddlInstallmentTo.Visible = true;
            rfvInstallmentfrom.Enabled = rfvInstallmentto.Enabled = true;

        }
    }
    private void FunPriLoadInstallmentDate()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Option", "11");
        Procparam.Add("@CompanyId", intCompanyId.ToString());
        Procparam.Add("@Panum", ddlMLA.SelectedValue);
        Procparam.Add("@Sanum", ddlSLA.SelectedValue);

        DataSet dsInstallment = new DataSet();
        dsInstallment = Utility.GetDataset("S3G_INS_LOADLOV", Procparam);
        if (dsInstallment.Tables[0] != null)
        {
            if (dsInstallment.Tables[0].Rows.Count > 0)
            {
                //ddlInstallmentFrom.BindDataTable(dsInstallment.Tables[0], new string[] { "InstallmentNo", "FromDate" });
                ddlInstallmentFrom.BindDataTable(dsInstallment.Tables[0], new string[] { "FromDate", "FromDate" });
                //ddlInstallmentTo.BindDataTable(dsInstallment.Tables[1], new string[] { "InstallmentNo", "ToDate" });
                ddlInstallmentTo.BindDataTable(dsInstallment.Tables[1], new string[] { "ToDate", "ToDate" });
            }
        }
    }
    private void FunPriLoadPayType()
    {
        //if (ddlPayType.Items.Count > 0)
        //{
        //    ddlPayType.Items.Clear();
        //}
        //Procparam = new Dictionary<string, string>();
        //Procparam.Add("@OPTION", "2");
        //ddlPayType.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

        if (ddlInsuranceDoneBy.SelectedItem.Text.ToUpper() == "COMPANY")
        {
            ddlPayType.Visible = lblPayType.Visible = true;
            rfvPayType.Visible = true;

            //ddlPayTo.Visible = lblPayTo.Visible = true;

            rfvPayType.Enabled = true;

            //    ddlPayType.SelectedValue = "2";
            //    ddlPayType.ClearDropDownList();
            //    lblHeaderInstallmentFrom.Visible = lblHeaderInstallmentTo.Visible = ddlInstallmentFrom.Visible = ddlInstallmentTo.Visible = true;
        }
        if (ddlInsuranceDoneBy.SelectedItem.Text.ToUpper() == "CUSTOMER")
        {
            //ddlPayType.SelectedValue = "1";
            //ddlPayType.ClearDropDownList();
            ddlPayType.Visible = false;
            lblPayType.Visible = false;
            //ddlPayTo.Visible = lblPayTo.Visible = true;
            rfvPayType.Enabled = false;
            lblHeaderInstallmentFrom.Visible = lblHeaderInstallmentTo.Visible = ddlInstallmentFrom.Visible = ddlInstallmentTo.Visible = false;
            rfvInstallmentfrom.Enabled = rfvInstallmentto.Enabled = false;
        }
    }
    protected void ddlAssetDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAssetDescription = (DropDownList)sender;
        if (ddlAssetDescription.SelectedIndex != 0)
        {
            Procparam = new Dictionary<string, string>();

            Procparam.Add("@Option", "5");
            if (ddlMLA.SelectedValue != "0")
            {
                Procparam.Add("@Panum", ddlMLA.SelectedValue);
                Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedText);

                Procparam.Add("@AssetId", ddlAssetDescription.SelectedValue);
                if (ddlLOB.SelectedItem.Text.Contains("OL"))
                {
                    Procparam.Add("@Is_OL", "1");
                }
                else
                {
                    Procparam.Add("@Is_OL", "0");
                }
                if (ViewState["CurrentEdit"] == null)
                {
                    DropDownList ddlMachineNo = gvCurrentInsurance.FooterRow.FindControl("ddlMachineNo") as DropDownList;
                    ddlMachineNo.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Asset_Number", "MachineNo" });
                }
                else
                {
                    int index = int.Parse(ViewState["CurrentEdit"].ToString());
                    DropDownList ddlMachineNo = gvCurrentInsurance.Rows[index].FindControl("ddlMachineNo") as DropDownList;
                    ddlMachineNo.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Asset_Number", "MachineNo" });
                }
            }


            //DataTable dtMachineDetails = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);
            //if (dtMachineDetails != null)
            //{ 
            //    if (dtMachineDetails.Rows.Count > 0)
            //    {
            //        TextBox txtRegNo = gvCurrentInsurance.FooterRow.FindControl("txtRegNo") as TextBox;
            //        txtRegNo.Text = dtMachineDetails.Rows[0][0].ToString();

            //    }
            //}
        }
    }
    //private void FunPriLoadAssetDescription()
    //{
    //    DropDownList ddlAssetDescription = gvCurrentInsurance.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
    //    Procparam = new Dictionary<string, string>();
    //    Procparam.Add("@OPTION", "1");
    //    if (ddlMLA.SelectedValue != "0")
    //    {
    //        Procparam.Add("@Panum", ddlMLA.SelectedValue);
    //    }
    //    if (ddlSLA.SelectedValue != "0" && ddlSLA.SelectedValue.ToString() != "")
    //    {
    //        Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedText);
    //    }
    //    if (ddlLOB.SelectedItem.Text.Contains("OL"))
    //    {
    //        Procparam.Add("@Is_OL", "1");
    //    }
    //    else
    //    {
    //        Procparam.Add("@Is_OL", "0");
    //    }

    //    DataTable dt = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);
    //    if (dt.Rows.Count == 0)
    //    {
    //        Utility.FunShowAlertMsg(this, "Asset Identification is not done for the particular account");
    //    }
    //    ddlAssetDescription.BindDataTable(dt, new string[] { "asset_id", "asset_code", "Asset_Description" });

    //    ViewState["AssetType"] = dt;

    //    //ddlAssetDescription.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "asset_id", "asset_code", "Asset_Description" });
    //}

    #endregion

    #region [BindPrimeAccount]

    #endregion

    #region [GridviewEventsforAccounts]

    //protected void gvCurrentInsurance_RowEditing(object sender, GridViewEditEventArgs e)
    //{


    //}

    private void LoadHistoryDetailsInCreate()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        Procparam.Add("@LOCATION_ID", ddlBranch.SelectedValue);
        Procparam.Add("@PANUM", ddlMLA.SelectedValue);
        if (ddlSLA.SelectedValue != "0")
        {
            Procparam.Add("@SANUM", ddlSLA.SelectedValue);
        }
        DataSet dsAINSE = Utility.GetDataset("S3G_INS_getAINSEHistoryDetails", Procparam);
        ViewState["DT_History_Policy"] = dsAINSE.Tables[1];
        if (dsAINSE.Tables[0].Rows.Count > 0)
        {
            pnlInsuranceHistory.Visible = true;
            gvInsuranceHistory.DataSource = dsAINSE.Tables[0];
            gvInsuranceHistory.DataBind();
        }
        else
            pnlInsuranceHistory.Visible = false;

    }

    private bool ValidatePolicyDetails(string strAssetDescription, string strRegNo, string strPolicyDate, string strValidDate)
    {

        DataTable dt = (DataTable)ViewState["DT_History_Policy"];
        if (dt.Rows.Count > 0)
        {
            DataRow[] dr = dt.Select("AssetID = " + strAssetDescription + " and MachineNo = '" + strRegNo + "'", "PolicyDate ASC");

            string strErrorMessage = "Policy Date range already exist for selected Asset";
            if (dr.Length > 0)
            {
                if (Utility.StringToDate(strPolicyDate) <= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && (Utility.StringToDate(strValidDate) >= Utility.StringToDate(dr[dr.Length - 1]["ValidTillDate"].ToString())))
                {

                    cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
                    cvAssetInsuranceEntry.IsValid = false;
                    return false;
                }

                if (dr.Length == 1)
                {

                    if (Utility.StringToDate(strPolicyDate) >= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && Utility.StringToDate(strPolicyDate) <= Utility.StringToDate(dr[0]["ValidTillDate"].ToString()))
                    {

                        cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
                        cvAssetInsuranceEntry.IsValid = false;
                        return false;
                    }
                    if (Utility.StringToDate(strValidDate) >= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && Utility.StringToDate(strValidDate) <= Utility.StringToDate(dr[0]["ValidTillDate"].ToString()))
                    {
                        cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
                        cvAssetInsuranceEntry.IsValid = false;
                        return false;
                    }
                }
                else
                {
                    if (Utility.StringToDate(strPolicyDate) >= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && Utility.StringToDate(strPolicyDate) <= Utility.StringToDate(dr[dr.Length - 1]["ValidTillDate"].ToString()))
                    {
                        cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
                        cvAssetInsuranceEntry.IsValid = false;
                        return false;
                    }
                    if (Utility.StringToDate(strValidDate) >= Utility.StringToDate(dr[0]["PolicyDate"].ToString()) && Utility.StringToDate(strValidDate) <= Utility.StringToDate(dr[dr.Length - 1]["ValidTillDate"].ToString()))
                    {
                        cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + strErrorMessage;
                        cvAssetInsuranceEntry.IsValid = false;
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private bool ValidateAsset(string strAsssetDesc, string strMachineNo, int intCurrentRow)
    {
        bool isValid = true;
        //string assetId;
        //string regNo;




        // DropDownList ddlAssetDescription = gvCurrentInsurance.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
        // DropDownList ddlMachineNo = gvCurrentInsurance.FooterRow.FindControl("ddlMachineNo") as DropDownList;

        //assetId = ddlAsset.SelectedValue;
        //regNo = ddlRegNo.SelectedItem.Text;

        for (int iCount = 0; iCount < gvCurrentInsurance.Rows.Count; iCount++)
        {
            if (iCount == intCurrentRow)
            {
                continue;
            }
            Label lblAssetDesc = gvCurrentInsurance.Rows[iCount].FindControl("lblAssetId") as Label;
            Label lblMachineNo = gvCurrentInsurance.Rows[iCount].FindControl("lblMachineNo") as Label;

            //DropDownList ddlAssetDescription = gvCurrentInsurance.Rows[iCount].FindControl("ddlAssetDescription") as DropDownList;
            // DropDownList txtRegNo = gvCurrentInsurance.Rows[iCount].FindControl("lblRegNo") as DropDownList;
            //if (lblAssetDesc.Text  == ddlAssetDescription.SelectedItem .Text && lblMachineNo.Text  == ddlMachineNo.SelectedItem .Text)
            if (lblAssetDesc.Text == strAsssetDesc && lblMachineNo.Text == strMachineNo)
            {
                Utility.FunShowAlertMsg(this.Page, "Selected Asset already exists.");
                //cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Selected Asset already exists.";
                // cvAssetInsuranceEntry.IsValid = false;
                isValid = false;
                break;
            }

        }
        return isValid;
    }

    #endregion

    /// <summary>
    /// Code Added for grid edit and update 
    /// Added new method to load LOB and branch
    /// </summary>
    private void FunPriLoad_LOB_Branch()
    {
        try
        {
            strMode = Request.QueryString["qsMode"].ToString();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            //Procparam.Add("@OPTION", "6");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@User_Id", Convert.ToString(intUserID));
            Procparam.Add("@Program_ID", Convert.ToString(131));
            //Modified by saranya  on 07-03-2012  based on sudarsan observation "Asset insurance entry to handle all LOBS where assets are mapped "
            Procparam.Add("@FilterOption", "'OL'");
            if (intAINSEId == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            ddlLOB.BindDataTable("S3G_Get_LOB_LIST", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddlLOB.Items.Count > 0) ddlLOB.SelectedIndex = 1;
            Procparam.Clear();

            Procparam.Add("@OPTION", "15");
            Procparam.Add("@COMPANYID", intCompanyId.ToString());
            ddlPayTo.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Entity_ID", "PayTo" });

            //if (strMode != "C")
            //{
                //Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                //Procparam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
                //Procparam.Add("@Program_ID", Convert.ToString(131));
                //if (intAINSEId == 0)
                //    Procparam.Add("@Is_Active", "1");
                ////ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
                //Procparam = null;
            //}
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private DataTable FunPriAssetInsEntryDataTable()
    {
        try
        {
            DataRow drEmptyRow;
            if (ViewState["DT_AssetInsEntryDetails"] == null)
            {
                dtAssetInsEntry = new DataTable();
                dtAssetInsEntry.Columns.Add("Serial_Number");
                dtAssetInsEntry.Columns.Add("AssetDescription");
                dtAssetInsEntry.Columns.Add("AssetID");
                dtAssetInsEntry.Columns.Add("DetailsID");
                //dtAssetInsEntry.Columns.Add("AssetNumber");
                //dtAssetInsEntry.Columns.Add("AssetCode");

                dtAssetInsEntry.Columns.Add("PolicyType");
                dtAssetInsEntry.Columns.Add("PolicyTypeID");
                dtAssetInsEntry.Columns.Add("AssetNumber");
                dtAssetInsEntry.Columns.Add("MachineNo");
                dtAssetInsEntry.Columns.Add("PolicyNo");
                dtAssetInsEntry.Columns.Add("PolicyDate", typeof(DateTime));
                dtAssetInsEntry.Columns.Add("ValidTillDate", typeof(DateTime));
                dtAssetInsEntry.Columns.Add("PolicyValue", System.Type.GetType("System.Decimal"));
                dtAssetInsEntry.Columns.Add("Premium", System.Type.GetType("System.Decimal"));
                dtAssetInsEntry.Columns.Add("Payment_Request_No");
                dtAssetInsEntry.Columns.Add("CanEdit");
                dtAssetInsEntry.Columns.Add("Nominee");
                dtAssetInsEntry.Columns.Add("Ins_Plan");
                ViewState["DT_AssetInsEntryDetails"] = dtAssetInsEntry;
            }
            dtAssetInsEntry = (DataTable)ViewState["DT_AssetInsEntryDetails"];
        }
        catch (Exception ex)
        {
            cvAssetInsuranceEntry.ErrorMessage = "Due to Data Problem,Unable to get the data";
            cvAssetInsuranceEntry.IsValid = false;
        }
        return dtAssetInsEntry;

    }



    private void FunPriInsertAssetInsEntryDataTable(string strAssetDescription, string strAssetID, string strPolicyType, string strPolicyTypeID, string strAssetNumber, string strMachineNo, string strPolicyNo, string strPolicyDate, string strValidTill, string strPolicyValue, string strPremium, string strDetailsId, string strNominee, string StrInsurancePlan)
    {
        try
        {
            DataRow drEmptyRow;
            dtAssetInsEntry = FunPriAssetInsEntryDataTable();
            if (dtAssetInsEntry.Rows.Count < 1)
            {
                drEmptyRow = dtAssetInsEntry.NewRow();
                //drEmptyRow["Serial_Number"] = "0";
                drEmptyRow["AssetDescription"] = "0";
                drEmptyRow["AssetID"] = "0";
                drEmptyRow["DetailsId"] = "0";
                drEmptyRow["PolicyType"] = "0";
                drEmptyRow["PolicyTypeID"] = "0";
                drEmptyRow["AssetNumber"] = "0";
                drEmptyRow["MachineNo"] = "0";
                drEmptyRow["PolicyNo"] = "0";
                drEmptyRow["PolicyDate"] = DateTime.Now;
                drEmptyRow["ValidTillDate"] = DateTime.Now;
                drEmptyRow["PolicyValue"] = 0;
                drEmptyRow["Premium"] = 0;
                drEmptyRow["Payment_Request_No"] = "0";
                drEmptyRow["CanEdit"] = "0";
                drEmptyRow["Nominee"] = "";
                drEmptyRow["Ins_Plan"] = "";
                dtAssetInsEntry.Rows.Add(drEmptyRow);
            }
            else
            {
                drEmptyRow = dtAssetInsEntry.NewRow();
                drEmptyRow["Serial_Number"] = Convert.ToInt32(dtAssetInsEntry.Rows[dtAssetInsEntry.Rows.Count - 1]["Serial_Number"]) + 1;
                drEmptyRow["AssetDescription"] = strAssetDescription;
                drEmptyRow["AssetID"] = strAssetID;
                drEmptyRow["DetailsId"] = strDetailsId;
                //drEmptyRow["AssetCode"] = "0";
                drEmptyRow["PolicyType"] = strPolicyType;
                drEmptyRow["PolicyTypeID"] = strPolicyTypeID;
                drEmptyRow["AssetNumber"] = strAssetNumber;
                drEmptyRow["MachineNo"] = strMachineNo;
                drEmptyRow["PolicyNo"] = strPolicyNo;
                drEmptyRow["PolicyDate"] = Utility.StringToDate(strPolicyDate);
                drEmptyRow["ValidTillDate"] = Utility.StringToDate(strValidTill);
                drEmptyRow["PolicyValue"] = Convert.ToDecimal(strPolicyValue);
                drEmptyRow["Premium"] = Convert.ToDecimal(strPremium);
                drEmptyRow["Payment_Request_No"] = "0";
                drEmptyRow["CanEdit"] = "0";
                drEmptyRow["Nominee"] = strNominee;
                drEmptyRow["Ins_Plan"] = StrInsurancePlan;
                dtAssetInsEntry.Rows.Add(drEmptyRow);
            }
            if (dtAssetInsEntry.Rows.Count > 1)
            {
                if (dtAssetInsEntry.Rows[0]["Serial_Number"].Equals("0"))
                {
                    dtAssetInsEntry.Rows[0].Delete();
                }
            }
            ViewState["DT_AssetInsEntryDetails"] = dtAssetInsEntry;

            FunPriFillGrid();
            FunPubBindAssetInsEntryDetails(dtAssetInsEntry);
            FunPriGenerateNewPolicyRow();
        }
        catch (Exception ex)
        {
            cvAssetInsuranceEntry.ErrorMessage = "Due to Data Problem,Unable to Processing";
            cvAssetInsuranceEntry.IsValid = false;
        }
    }

    private void FunPriFillGrid()
    {
        try
        {

            DataTable dtAssetInsurance = (DataTable)ViewState["DT_AssetInsEntryDetails"];
            if (dtAssetInsurance.Rows.Count > 0)
            {
                gvCurrentInsurance.DataSource = dtAssetInsurance;
                gvCurrentInsurance.DataBind();
                gvCurrentInsurance.Visible  = true;
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Records Found..");
                return;
            }


        }
        catch (Exception ex)
        {
            cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_BindGrid + this.Page.Header.Title;
            cvAssetInsuranceEntry.IsValid = false;
        }
    }

    private void FunPubBindAssetInsEntryDetails(DataTable dtWorkflow)
    {
        try
        {

            gvCurrentInsurance.DataSource = dtWorkflow;
            gvCurrentInsurance.DataBind();
            //FunPubSetFooterRowVisibility();

            if (dtWorkflow.Rows.Count > 0 && Convert.ToString(dtWorkflow.Rows[0]["Serial_Number"]) == "0")
            {
                gvCurrentInsurance.Rows[0].Visible = false;
            }
            gvCurrentInsurance.Visible = true;


        }
        catch (Exception ex)
        {
            cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_BindGrid + this.Page.Header.Title;
            cvAssetInsuranceEntry.IsValid = false;
        }
    }

    private void FunPriUpdateDataTable(string strSeriel_Num, string strAssetDescription, string strAssetID, string strPolicyType, string strPolicyTypeID, string strAssetNumber, string strMachineNo, string strPolicyNo, string strPolicyDate, string strValidTill, string strPolicyValue, string strPremium, string strDetailsId, string strNominee, string strInsType)
    {
        try
        {
            DataTable dtInsuranceDetails = (DataTable)ViewState["DT_AssetInsEntryDetails"];
            if (dtInsuranceDetails.Rows.Count > 0)
            {
                if (Convert.ToString(dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Serial_Number"]) == strSeriel_Num)
                {
                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["AssetDescription"] = strAssetDescription;
                    if (DropDownListInsType.SelectedValue == "1")
                    {
                        dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["AssetID"] = strAssetID;
                        dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["PolicyType"] = strPolicyType;
                        dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["PolicyTypeID"] = strPolicyTypeID;
                        dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["AssetNumber"] = strAssetNumber;
                        dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["MachineNo"] = strMachineNo;
                    }
                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["DetailsId"] = strDetailsId;
                    //drEmptyRow["AssetNumber"] = "0";
                    //drEmptyRow["AssetCode"] = "0";

                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["PolicyNo"] = strPolicyNo;
                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["PolicyDate"] = Utility.StringToDate(strPolicyDate);
                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["ValidTillDate"] = Utility.StringToDate(strValidTill);
                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["PolicyValue"] = Convert.ToDecimal(strPolicyValue);
                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Premium"] = Convert.ToDecimal(strPremium);
                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Nominee"] = strNominee;
                    dtInsuranceDetails.Rows[Convert.ToInt32(strSeriel_Num) - 1]["Ins_Plan"] = strInsType;

                    dtInsuranceDetails.AcceptChanges();
                    ViewState["DT_AssetInsEntryDetails"] = dtInsuranceDetails;
                }
            }

        }
        catch (Exception ex)
        {
            cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_BindGrid + this.Page.Header.Title;
            cvAssetInsuranceEntry.IsValid = false;
        }
    }


    private bool FunPriIsCheckGridPolicyNo(string strPolicyNo, int intRowIndex)
    {
        foreach (GridViewRow grRow in gvCurrentInsurance.Rows)
        {
            if (grRow.RowType == DataControlRowType.DataRow)
            {
                Label lblPolicyNumber = grRow.FindControl("lblPolicyNumber") as Label;
                if (lblPolicyNumber == null)
                {

                    TextBox txtPolicyNumber = grRow.FindControl("txtPolicyNumber") as TextBox;

                    if (txtPolicyNumber.Text == strPolicyNo && intRowIndex != grRow.RowIndex)
                    {
                        return false;
                    }

                }
                else
                {
                    if (lblPolicyNumber.Text == strPolicyNo)
                    {
                        return false;
                    }
                }
            }


        }
        return true;
    }
    // Grid events

    //protected void gvCurrentInsurance_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {

    //        if (e.CommandName == "Add")
    //        {

    //            DropDownList ddlAssetDescription = gvCurrentInsurance.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
    //            DropDownList ddlPolicyType = gvCurrentInsurance.FooterRow.FindControl("ddlPolicyType") as DropDownList;
    //            DropDownList ddlMachineNo = gvCurrentInsurance.FooterRow.FindControl("ddlMachineNo") as DropDownList;
    //            TextBox txtPolicyNumber = gvCurrentInsurance.FooterRow.FindControl("txtPolicyNumber") as TextBox;
    //            TextBox txtPolicyDate = gvCurrentInsurance.FooterRow.FindControl("txtPolicyDate") as TextBox;
    //            TextBox txtValidTill = gvCurrentInsurance.FooterRow.FindControl("txtValidTill") as TextBox;
    //            TextBox txtPolicyValue = gvCurrentInsurance.FooterRow.FindControl("txtPolicyValue") as TextBox;
    //            TextBox txtPremium = gvCurrentInsurance.FooterRow.FindControl("txtPremium") as TextBox;
    //            TextBox txtNominee = gvCurrentInsurance.FooterRow.FindControl("txtNominee") as TextBox;
    //            TextBox txtInsPlan = gvCurrentInsurance.FooterRow.FindControl("txtInsPlan") as TextBox;
    //            Label lblDetailsId = gvCurrentInsurance.FooterRow.FindControl("lblDetailsId") as Label;
    //            //if (ddlAssetDescription.SelectedIndex > 0)
    //            //{
    //            //    rfvMachineNo.Enabled = true  ;
    //            //}
    //            //else
    //            //{
    //            //    rfvMachineNo.Enabled = false ;
    //            //    //return;
    //            //}

    //            if (DropDownListInsType.SelectedValue == "1")
    //            {

    //                if (ddlAssetDescription.SelectedValue == "0")
    //                {
    //                    cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Select Asset description";
    //                    cvAssetInsuranceEntry.IsValid = false;
    //                    return;
    //                }
    //                if (ddlPolicyType.SelectedValue == "0")
    //                {
    //                    cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Select the policy type";
    //                    cvAssetInsuranceEntry.IsValid = false;
    //                    return;

    //                }

    //                if (ddlMachineNo.SelectedValue == "0")
    //                {
    //                    cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Select the Reg No";
    //                    cvAssetInsuranceEntry.IsValid = false;
    //                    return;

    //                }


    //            }




    //            if (ddlPayTo.SelectedValue == "0")
    //            {
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Select an Insurer Name";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;
    //            }
    //            if (!FunPriIsCheckGridPolicyNo(txtPolicyNumber.Text, 0))
    //            {
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Policy Number already exist for selected Insurer Name";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;
    //            }
    //            if (!FunPriIsExistPolicyNo(txtPolicyNumber.Text, ""))
    //            {
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Policy Number already exist for selected Insurer Name";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;
    //            }
    //            if (Utility.CompareDates(txtAINSEDate.Text.ToString(), txtValidTill.Text) != 1)
    //            {
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be greater than System Date";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;
    //            }
    //            if (DropDownListInsType.SelectedValue == "1")
    //            {
    //                if (ddlMLA.SelectedText != "" && ddlSLA.SelectedText != "")
    //                {
    //                    Procparam = new Dictionary<string, string>();
    //                    Procparam.Add("@Option", "20");
    //                    Procparam.Add("@CompanyId", intCompanyId.ToString());
    //                    Procparam.Add("@Panum", ddlMLA.SelectedValue);
    //                    Procparam.Add("@Sanum", (ddlSLA.SelectedText == "--Select--") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedText);
    //                    Procparam.Add("@AssetId", ddlAssetDescription.SelectedValue);
    //                    DataTable dtAccountDateCheck = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);

    //                    string strAssetType = "0";

    //                    DataTable dtNewAsset = (DataTable)ViewState["AssetType"];
    //                    if (dtNewAsset != null && dtNewAsset.Rows.Count > 0)
    //                    {
    //                        DataRow[] dRows = dtNewAsset.Select("Asset_ID=" + ddlAssetDescription.SelectedValue);
    //                        if (dRows.Length > 0)
    //                        {
    //                            strAssetType = dRows[0][3].ToString();  // 0 - New Asset | 1 - Used Asset
    //                        }
    //                    }

    //                    if (dtAccountDateCheck.Rows.Count >= 0)
    //                    {
    //                        if (Utility.CompareDates(dtAccountDateCheck.Rows[0][0].ToString(), txtPolicyDate.Text) == -1 && strAssetType == "0")
    //                        {
    //                            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Policy Date should be greater than or equal to Account Creation Date";
    //                            cvAssetInsuranceEntry.IsValid = false;
    //                            return;
    //                        }
    //                        if (Utility.CompareDates(dtAccountDateCheck.Rows[0][0].ToString(), txtValidTill.Text) == -1)
    //                        {
    //                            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be greater than Account Creation Date";
    //                            cvAssetInsuranceEntry.IsValid = false;
    //                            return;
    //                        }
    //                        if (Utility.CompareDates(txtPolicyDate.Text, dtAccountDateCheck.Rows[0][1].ToString()) == -1 && strAssetType == "0")
    //                        {
    //                            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Policy Date should be less than Account Tenure ";
    //                            cvAssetInsuranceEntry.IsValid = false;
    //                            return;
    //                        }
    //                        if (Utility.CompareDates(txtValidTill.Text, dtAccountDateCheck.Rows[0][1].ToString()) == -1)
    //                        {
    //                            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be less than Account Tenure ";
    //                            cvAssetInsuranceEntry.IsValid = false;
    //                            return;
    //                        }
    //                        if (Convert.ToDecimal(txtPolicyValue.Text) > Convert.ToDecimal(dtAccountDateCheck.Rows[0][2].ToString()))
    //                        {
    //                            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Policy value should be less than Asset value";
    //                            cvAssetInsuranceEntry.IsValid = false;
    //                            return;
    //                        }

    //                    }

    //                }
    //            }
    //            if (Convert.ToDecimal(txtPolicyValue.Text) <= Convert.ToDecimal(txtPremium.Text))
    //            {
    //                txtPremium.Focus();
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Premium Value should not be greater than or equal to the Policy Value";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;
    //            }

    //            if (Utility.CompareDates(txtPolicyDate.Text, txtValidTill.Text) != 1)
    //            {
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be greater than Policy Date";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;
    //            }

    //            if (DropDownListInsType.SelectedValue == "1")
    //            {
    //                if (!ValidateAsset(ddlAssetDescription.SelectedValue, ddlMachineNo.SelectedItem.Text, -1))
    //                {
    //                    return;
    //                }

    //                if (!ValidatePolicyDetails(ddlAssetDescription.SelectedValue, ddlMachineNo.SelectedItem.Text, txtPolicyDate.Text, txtValidTill.Text))
    //                    return;
    //            }
    //            if (DropDownListInsType.SelectedValue == "1")
    //            {
    //                FunPriInsertAssetInsEntryDataTable(ddlAssetDescription.SelectedItem.Text, ddlAssetDescription.SelectedValue, ddlPolicyType.SelectedItem.Text, ddlPolicyType.SelectedValue, ddlMachineNo.SelectedValue, ddlMachineNo.SelectedItem.Text, txtPolicyNumber.Text, txtPolicyDate.Text, txtValidTill.Text, txtPolicyValue.Text, txtPremium.Text, "0", txtNominee.Text, txtInsPlan.Text);
    //            }
    //            else
    //            {
    //                FunPriInsertAssetInsEntryDataTable("", ddlAssetDescription.SelectedValue, ddlPolicyType.SelectedItem.Text, ddlPolicyType.SelectedValue, ddlMachineNo.SelectedValue, "", txtPolicyNumber.Text, txtPolicyDate.Text, txtValidTill.Text, txtPolicyValue.Text, txtPremium.Text, "0", txtNominee.Text, txtInsPlan.Text);
    //            }


    //            CalculatePremium();
    //            //FunPriInsertAssetInsEntryDataTable("dfgdf", "dfgd", "rt", "dfgh","ij", "dfgdf", "dfgd", "12/4/2011", "4/5/2011", "45.22","544.25");

    //            FunPubSetFooterRowVisibility();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_AddGrid + this.Page.Header.Title;
    //        cvAssetInsuranceEntry.IsValid = false;
    //    }
    //}



    //protected void gvCurrentInsurance_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    try
    //    {
    //        gvCurrentInsurance.EditIndex = e.NewEditIndex;
    //        FunPriFillGrid();
    //        FunPriGenerateEditPolicyRow(e.NewEditIndex);
    //        ViewState["CurrentEdit"] = e.NewEditIndex.ToString();
    //        //((TextBox)gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtPolicyValue")).Style.Add("text-align", "right");
    //        //((TextBox)gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtPremium")).Style.Add("text-align", "right");

    //        AjaxControlToolkit.CalendarExtender calPolicyDateEdit = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("calPolicyDateEdit") as AjaxControlToolkit.CalendarExtender;
    //        calPolicyDateEdit.Format = strDateFormat;
    //        AjaxControlToolkit.CalendarExtender calValidDateEdit = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("calValidDateEdit") as AjaxControlToolkit.CalendarExtender;
    //        calValidDateEdit.Format = strDateFormat;

    //        TextBox txtPolicyDate = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtPolicyDate") as TextBox;
    //        txtPolicyDate.Attributes.Add("readonly", "true");

    //        TextBox txtValidTill = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtValidTill") as TextBox;
    //        txtValidTill.Attributes.Add("readonly", "true");

    //        //DropDownList ddlAssetDescription = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("ddlAssetDescription") as DropDownList;
    //        //HiddenField hdnLookupcode = (HiddenField)gvAppropriationLogic.Rows[e.NewEditIndex].FindControl("hdnLookupcode");
    //        //ddlAssetDescription.BindDataTable(Utility.GetDefaultData(SPNames.S3G_CLN_GetAppLogicReceiptTypes, Procparam));
    //        // ddlAssetDescription.SelectedValue = hdnLookupcode.Value;// lblLookupCode.Text;

    //        //LoadEditItemAssetDescription(e.NewEditIndex);
    //        TextBox txtPolicyValue = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtPolicyValue") as TextBox;
    //        txtPolicyValue.SetDecimalPrefixSuffix(10, 4, true, "Policy Value");
    //        txtPolicyValue.Text = Convert.ToDecimal(txtPolicyValue.Text).ToString(Utility.SetSuffix());

    //        TextBox txtPremium = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtPremium") as TextBox;
    //        TextBox txtNominee = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtNominee") as TextBox;
    //        TextBox txtInsPlan = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtInsPlan") as TextBox;
    //        txtPremium.SetDecimalPrefixSuffix(10, 4, true, "Premium");
    //        txtPremium.Text = Convert.ToDecimal(txtPremium.Text).ToString(Utility.SetSuffix());

    //        gvCurrentInsurance.FooterRow.Visible = false;
    //        if (ViewState["AllowModify"] != null)
    //        {
    //            if (ViewState["AllowModify"].ToString() == "0")
    //            {
    //                DropDownList ddlAssetDescription = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("ddlAssetDescription") as DropDownList;
    //                DropDownList ddlMachineNo = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("ddlMachineNo") as DropDownList;
    //                //DropDownList ddlPolicyType = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("ddlPolicyType") as DropDownList;
    //                //TextBox txtPolicyNumber = gvCurrentInsurance.Rows[e.NewEditIndex].FindControl("txtPolicyNumber") as TextBox;

    //                Utility.ClearDropDownList(ddlAssetDescription);
    //                Utility.ClearDropDownList(ddlMachineNo);
    //                // Utility.ClearDropDownList(ddlPolicyType);
    //                //txtPolicyNumber.Attributes.Add("readonly", "true"); 
    //                txtPolicyValue.Attributes.Add("readonly", "true");

    //                txtPremium.Attributes.Add("readonly", "true");
    //                calPolicyDateEdit.Enabled = false;
    //                calValidDateEdit.Enabled = false;
    //            }

    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_EditGrid + this.Page.Header.Title;
    //        cvAssetInsuranceEntry.IsValid = false;
    //    }
    //}


    //protected void gvCurrentInsurance_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        Label lblSerialNo = (Label)gvCurrentInsurance.Rows[e.RowIndex].FindControl("lblSerialNo");
    //        DropDownList ddlAssetDescription = gvCurrentInsurance.Rows[e.RowIndex].FindControl("ddlAssetDescription") as DropDownList;
    //        DropDownList ddlPolicyType = gvCurrentInsurance.Rows[e.RowIndex].FindControl("ddlPolicyType") as DropDownList;
    //        DropDownList ddlMachineNo = gvCurrentInsurance.Rows[e.RowIndex].FindControl("ddlMachineNo") as DropDownList;
    //        TextBox txtPolicyNumber = gvCurrentInsurance.Rows[e.RowIndex].FindControl("txtPolicyNumber") as TextBox;
    //        TextBox txtPolicyDate = gvCurrentInsurance.Rows[e.RowIndex].FindControl("txtPolicyDate") as TextBox;
    //        TextBox txtValidTill = gvCurrentInsurance.Rows[e.RowIndex].FindControl("txtValidTill") as TextBox;
    //        TextBox txtPolicyValue = gvCurrentInsurance.Rows[e.RowIndex].FindControl("txtPolicyValue") as TextBox;
    //        TextBox txtPremium = gvCurrentInsurance.Rows[e.RowIndex].FindControl("txtPremium") as TextBox;
    //        Label lblDetailsId = (Label)gvCurrentInsurance.Rows[e.RowIndex].FindControl("lblDetailsId");
    //        TextBox txtNominee = gvCurrentInsurance.Rows[e.RowIndex].FindControl("txtNominee") as TextBox;
    //        TextBox txtInsPlan = gvCurrentInsurance.Rows[e.RowIndex].FindControl("txtInsPlan") as TextBox;



    //        if (DropDownListInsType.SelectedValue == "1")
    //        {

    //            if (ddlAssetDescription.SelectedValue == "0")
    //            {
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Select Asset description";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;
    //            }
    //            if (ddlPolicyType.SelectedValue == "0")
    //            {
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Select the policy type";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;

    //            }

    //            if (ddlMachineNo.SelectedValue == "0")
    //            {
    //                cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Select the Reg No";
    //                cvAssetInsuranceEntry.IsValid = false;
    //                return;

    //            }


    //        }


    //        if (!FunPriIsCheckGridPolicyNo(txtPolicyNumber.Text, e.RowIndex))
    //        {
    //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Policy Number already exist for selected Insurer Name";
    //            cvAssetInsuranceEntry.IsValid = false;
    //            return;
    //        }
    //        if (!FunPriIsExistPolicyNo(txtPolicyNumber.Text, lblDetailsId.Text))
    //        {
    //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Policy Number already exist for selected Insurer Name";
    //            cvAssetInsuranceEntry.IsValid = false;
    //            return;
    //        }

    //        if (Convert.ToDecimal(txtPolicyValue.Text) <= Convert.ToDecimal(txtPremium.Text))
    //        {
    //            txtPremium.Focus();
    //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Premium Value should not be greater than or equal to the Policy Value";
    //            cvAssetInsuranceEntry.IsValid = false;
    //            return;
    //        }
    //        if (DropDownListInsType.SelectedValue == "1")
    //        {

    //            if (ddlMLA.SelectedText != "" && ddlSLA.SelectedText != "")
    //            {
    //                Procparam = new Dictionary<string, string>();
    //                Procparam.Add("@Option", "20");
    //                Procparam.Add("@CompanyId", intCompanyId.ToString());
    //                Procparam.Add("@Panum", ddlMLA.SelectedValue);
    //                Procparam.Add("@Sanum", (ddlSLA.SelectedText == "--Select--") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedText);
    //                Procparam.Add("@AssetId", ddlAssetDescription.SelectedValue);
    //                DataTable dtAccountDateCheck = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);

    //                string strAssetType = "0";

    //                DataTable dtNewAsset = (DataTable)ViewState["AssetType"];
    //                if (dtNewAsset != null && dtNewAsset.Rows.Count > 0)
    //                {
    //                    DataRow[] dRows = dtNewAsset.Select("Asset_ID=" + ddlAssetDescription.SelectedValue);
    //                    if (dRows.Length > 0)
    //                    {
    //                        strAssetType = dRows[0][3].ToString();  // 0 - New Asset | 1 - Used Asset
    //                    }
    //                }

    //                if (dtAccountDateCheck.Rows.Count > 0)
    //                {
    //                    if (Utility.CompareDates(dtAccountDateCheck.Rows[0][0].ToString(), txtPolicyDate.Text) == -1 && strAssetType == "0")
    //                    {
    //                        cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Policy Date should be greater than or equal to Account Creation Date";
    //                        cvAssetInsuranceEntry.IsValid = false;
    //                        return;
    //                    }
    //                    if (Utility.CompareDates(dtAccountDateCheck.Rows[0][0].ToString(), txtValidTill.Text) == -1)
    //                    {
    //                        cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be greater than Account Creation Date";
    //                        cvAssetInsuranceEntry.IsValid = false;
    //                        return;
    //                    }

    //                }
    //            }
    //        }


    //        if (Utility.CompareDates(txtPolicyDate.Text, txtValidTill.Text) != 1)
    //        {
    //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be greater than Policy Date";
    //            cvAssetInsuranceEntry.IsValid = false;
    //            return;
    //        }
    //        if (Utility.CompareDates(txtAINSEDate.Text, txtValidTill.Text) != 1)
    //        {
    //            cvAssetInsuranceEntry.ErrorMessage = strErrorMessagePrefix + "Valid Till Date should be greater than System Date";
    //            cvAssetInsuranceEntry.IsValid = false;
    //            return;
    //        }
    //        if (DropDownListInsType.SelectedValue == "1")
    //        {

    //            if (!ValidateAsset(ddlAssetDescription.SelectedValue, ddlMachineNo.SelectedItem.Text, e.RowIndex))
    //            {
    //                return;
    //            }
    //        }





    //        if (DropDownListInsType.SelectedValue == "1")
    //        {
    //            FunPriUpdateDataTable(lblSerialNo.Text, ddlAssetDescription.SelectedItem.Text, ddlAssetDescription.SelectedValue, ddlPolicyType.SelectedItem.Text, ddlPolicyType.SelectedValue, ddlMachineNo.SelectedValue, ddlMachineNo.SelectedItem.Text, txtPolicyNumber.Text, txtPolicyDate.Text, txtValidTill.Text, txtPolicyValue.Text, txtPremium.Text, lblDetailsId.Text, txtNominee.Text, txtInsPlan.Text);
    //        }
    //        else
    //        {

    //            FunPriUpdateDataTable(lblSerialNo.Text, "", ddlAssetDescription.SelectedValue, "", ddlPolicyType.SelectedValue, ddlMachineNo.SelectedValue, "", txtPolicyNumber.Text, txtPolicyDate.Text, txtValidTill.Text, txtPolicyValue.Text, txtPremium.Text, lblDetailsId.Text, txtNominee.Text, txtInsPlan.Text);
    //        }
    //        //FunPriUpdateDataTable(lblSerialNo.Text, "dfgdf", "dfgd", "rt", "k","dfgh", "dfgdf", "dfgd", "12/4/2011", "4/5/2011", "45.22", "544.25");
    //        gvCurrentInsurance.EditIndex = -1;
    //        ViewState["CurrentEdit"] = null;
    //        FunPriFillGrid();

    //        FunPriGenerateNewPolicyRow();
    //        CalculatePremium();
    //        if (ViewState["AllowModify"] != null)
    //        {
    //            if (ViewState["AllowModify"].ToString() == "0")
    //                gvCurrentInsurance.FooterRow.Visible = false;
    //            else
    //                gvCurrentInsurance.FooterRow.Visible = true;
    //        }
    //        else
    //            gvCurrentInsurance.FooterRow.Visible = true;

    //        FunPubSetFooterRowVisibility();
    //    }
    //    catch (Exception ex)
    //    {
    //        cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_UpdateGrid + this.Page.Header.Title;
    //        cvAssetInsuranceEntry.IsValid = false;
    //    }
    //}


    //protected void gvCurrentInsurance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    try
    //    {
    //        ViewState["CurrentEdit"] = null;
    //        gvCurrentInsurance.EditIndex = -1;
    //        FunPriFillGrid();

    //        FunPriGenerateNewPolicyRow();

    //        if (ViewState["AllowModify"] != null)
    //        {
    //            if (ViewState["AllowModify"].ToString() == "0")
    //                gvCurrentInsurance.FooterRow.Visible = false;
    //            else
    //                gvCurrentInsurance.FooterRow.Visible = true;
    //        }
    //        else
    //            gvCurrentInsurance.FooterRow.Visible = true;

    //        FunPubSetFooterRowVisibility();
    //    }
    //    catch (Exception ex)
    //    {
    //        cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_CancelEditGrid + this.Page.Header.Title;
    //        cvAssetInsuranceEntry.IsValid = false;
    //    }
    //}



    protected void gvCurrentInsurance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataSet datSet = new DataSet();
            DataTable dtAssetInsEntry1 = new DataTable();

            DataTable ObjDT = new DataTable();
            ObjDT.Columns.Add("AssetDescription");
            ObjDT.Columns.Add("AssetID");
            ObjDT.Columns.Add("DetailsID");
            ObjDT.Columns.Add("AssetValue");
            ObjDT.Columns.Add("PolicyType");
            ObjDT.Columns.Add("Invoice_ID");
            ObjDT.Columns.Add("PolicyTypeID");
            ObjDT.Columns.Add("PA_SA_REF_ID");
            ObjDT.Columns.Add("PANUM");
            ObjDT.Columns.Add("POLICYNUMBER");
            ObjDT.Columns.Add("PolicyDate");
            ObjDT.Columns.Add("ValidTillDate");
            ObjDT.Columns.Add("PolicyValue");
            ObjDT.Columns.Add("LOSS_PAYEE_DETAILS");
            ObjDT.Columns.Add("Ins_Plan");
            ObjDT.Columns.Add("AssetNumber");
            ObjDT.Columns.Add("Premium");
            ObjDT.Columns.Add("REMARKS");
            ObjDT.Columns.Add("Payment_Request_No");
            ObjDT.Columns.Add("DetailsId");
            ObjDT.Columns.Add("CanEdit");


            foreach (GridViewRow drow in gvCurrentInsurance.Rows)
            {
                AjaxControlToolkit.CalendarExtender CEPolicyDate = drow.FindControl("CEPolicyDate") as AjaxControlToolkit.CalendarExtender;
                CEPolicyDate.Format = strDateFormat;
                AjaxControlToolkit.CalendarExtender calValidDate = drow.FindControl("calValidDate") as AjaxControlToolkit.CalendarExtender;
                calValidDate.Format = strDateFormat;
                DropDownList ddlFPolicyType = drow.FindControl("ddlFPolicyType") as DropDownList;
                Label lblAssetValue = drow.FindControl("lblAssetValue") as Label;
                Label lblPolicyTypeId = drow.FindControl("lblPolicyTypeId") as Label;
                TextBox txtPolicyDate = drow.FindControl("txtPolicyDate") as TextBox;
                TextBox txtLoss_Payee_Details = drow.FindControl("txtLoss_Payee_Details") as TextBox;
                TextBox txtFRemarks = drow.FindControl("txtFRemarks") as TextBox;
                TextBox txtPolicyNumber = drow.FindControl("txtPolicyNumber") as TextBox;
                TextBox txtValidTill = drow.FindControl("txtValidTill") as TextBox;
                TextBox txtPremium = drow.FindControl("txtPremium") as TextBox;
                TextBox txtPolicyValue = drow.FindControl("txtPolicyValue") as TextBox;
                TextBox txtNominee = drow.FindControl("txtNominee") as TextBox;
                TextBox txtInsPlan = drow.FindControl("txtInsPlan") as TextBox;
                Label lblPremium = drow.FindControl("lblPremium") as Label;
                Label lblPolicyValue = drow.FindControl("lblPolicyValue") as Label;
                Label lblRSNo = drow.FindControl("lblRSNo") as Label;
                Label lblRentalSchedNo = drow.FindControl("lblRentalSchedNo") as Label;

                DataRow dr_Alert = ObjDT.NewRow();
                dr_Alert["AssetDescription"] = "";
                dr_Alert["AssetID"] = "0";
                dr_Alert["DetailsID"] = "";
                dr_Alert["Invoice_ID"] = "";
                dr_Alert["PolicyType"] = ddlFPolicyType.SelectedItem.ToString();
                dr_Alert["PolicyTypeID"] = ddlFPolicyType.SelectedValue.ToString();
                dr_Alert["PA_SA_REF_ID"] = lblRSNo.Text;
                dr_Alert["PANUM"] = lblRentalSchedNo.Text;
                dr_Alert["POLICYNUMBER"] = txtPolicyNumber.Text;
                if (txtPolicyDate.Text != "")
                {
                    dr_Alert["PolicyDate"] = DateTime.Parse(Utility.StringToDate(txtPolicyDate.Text).ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);
                }
                else
                {
                    dr_Alert["PolicyDate"] = "";
                }
                if (txtValidTill.Text != "")
                {
                   dr_Alert["ValidTillDate"] = DateTime.Parse(Utility.StringToDate(txtValidTill.Text).ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);
                }
                else
                {
                    dr_Alert["ValidTillDate"] = "";
                }
                dr_Alert["REMARKS"] = txtFRemarks.Text;
                dr_Alert["AssetValue"] = lblAssetValue.Text;
                dr_Alert["LOSS_PAYEE_DETAILS"] = txtLoss_Payee_Details.Text;
                dr_Alert["PolicyValue"] = txtPolicyValue.Text;
                if (txtPremium.Text != "")
                {
                    dr_Alert["Premium"] = txtPremium.Text;
                }
                else
                {
                    dr_Alert["Premium"] = "0";
                }
                dr_Alert["Premium"] = txtPremium.Text;
                dr_Alert["DetailsId"] = "";
                dr_Alert["AssetNumber"] = "0";
                dr_Alert["Payment_Request_No"] = "";
                dr_Alert["CanEdit"] = "";
                ObjDT.Rows.Add(dr_Alert);
            }


            //try
            //{
            //    datSet = (DataSet)ViewState["DT_AssetInsEntryDetails"];
            //    dtAssetInsEntry1 = datSet.Tables[0];
            //}

            //catch (Exception ex)
            //{
            dtAssetInsEntry1 = ObjDT;
            //}
            if (dtAssetInsEntry1.Rows.Count == 1)
            {
                Utility.FunShowAlertMsg(this, "Atleast one Rental Schedule Number have to be included");
                return;
            }
            dtAssetInsEntry1.Rows.RemoveAt(e.RowIndex);


            gvCurrentInsurance.DataSource = dtAssetInsEntry1;
            gvCurrentInsurance.DataBind();

            ViewState["DT_AssetInsEntryDetails"] = dtAssetInsEntry1;
            CalculatePremium();

            //FunPubSetFooterRowVisibility();
        }
        catch (Exception ex)
        {
            cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_DeleteGrid + this.Page.Header.Title;
            cvAssetInsuranceEntry.IsValid = false;
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }


    }

    protected void gvInsuranceHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Label lblPolicyValue = e.Row.FindControl("lblPolicyValue") as Label;
            Label lblPremium = e.Row.FindControl("lblPremium") as Label;
           

            // decimal d1 = 0;
            //if (lblPremium != null)
            //    lblPremium.Text = Convert.ToDecimal(lblPremium.Text).ToString(Funsetsuffix());
            //if (lblPolicyValue != null)
            //    lblPolicyValue.Text = Convert.ToDecimal(lblPolicyValue.Text).ToString(Funsetsuffix());
        }

        catch (Exception ex)
        {
            cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_BindGrid + this.Page.Header.Title;
            cvAssetInsuranceEntry.IsValid = false;
        }
    }


    protected void grvRange_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            chkSelect.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvRange.ClientID + "','chkAll','chkSelect');");
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAllRole");
            CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
            chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvRange.ClientID + "',this,'chkSelect');");
            //chkAll.Checked = true;
        }
    }


    protected void gvCurrentInsurance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender CEPolicyDate = e.Row.FindControl("CEPolicyDate") as AjaxControlToolkit.CalendarExtender;
                CEPolicyDate.Format = strDateFormat;
                AjaxControlToolkit.CalendarExtender calValidDate = e.Row.FindControl("calValidDate") as AjaxControlToolkit.CalendarExtender;
                calValidDate.Format = strDateFormat;
                DropDownList ddlFPolicyType = e.Row.FindControl("ddlFPolicyType") as DropDownList;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@OPTION", "10");
                ddlFPolicyType.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
                Label lblAssetValue = e.Row.FindControl("lblAssetValue") as Label;
                if (lblAssetValue.Text == "")
                {
                    lblAssetValue.Text = "0.00";
                }
                Label lblPolicyTypeId = e.Row.FindControl("lblPolicyTypeId") as Label;
                try
                {
                    ddlFPolicyType.SelectedValue = lblPolicyTypeId.Text;
                }
                catch (Exception ex)
                {
                    ddlFPolicyType.SelectedIndex = 0;
                }

                TextBox txtPolicyDate = e.Row.FindControl("txtPolicyDate") as TextBox;
                if (txtPolicyDate != null)
                {
                    txtPolicyDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtPolicyDate.ClientID + "','" + strDateFormat + "',false,  false);");
                }
                if (PageMode == PageModes.Query)
                {
                    txtPolicyDate.Attributes.Add("readonly", "true");
                    txtPolicyDate.Attributes.Remove("onblur");
                }

                TextBox txtValidTill = e.Row.FindControl("txtValidTill") as TextBox;
                if (txtValidTill != null)
                {
                    txtValidTill.Attributes.Add("onblur", "fnDoDate(this,'" + txtValidTill.ClientID + "','" + strDateFormat + "',false,  false);");
                }
                if (PageMode == PageModes.Query)
                {
                    txtValidTill.Attributes.Add("readonly", "true");
                    txtValidTill.Attributes.Remove("onblur");
                }

                TextBox txtPremium = e.Row.FindControl("txtPremium") as TextBox;
                TextBox txtPolicyValue = e.Row.FindControl("txtPolicyValue") as TextBox;
                TextBox txtNominee = e.Row.FindControl("txtNominee") as TextBox;
                TextBox txtInsPlan = e.Row.FindControl("txtInsPlan") as TextBox;
                //txtPremium.SetDecimalPrefixSuffix(10, 4, true, "Premium");
                //txtPolicyValue.SetDecimalPrefixSuffix(10, 4, true, "Policy Value");

                LinkButton lnkEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                LinkButton lnkRemove = e.Row.FindControl("lnkRemove") as LinkButton;
                Label lblPremium = e.Row.FindControl("lblPremium") as Label;
                Label lblPolicyValue = e.Row.FindControl("lblPolicyValue") as Label;
                Label lblPayment_Request_No = e.Row.FindControl("lblPayment_Request_No") as Label;
                Label lblCanEdit = e.Row.FindControl("lblCanEdit") as Label;
                Label lblNominee = e.Row.FindControl("lblNominee") as Label;
                Label lblInsPlan = e.Row.FindControl("lblInsPlan") as Label;

                if (lblPremium != null && lblPremium.Text.Trim() != "")
                    lblPremium.Text = Convert.ToDecimal(lblPremium.Text).ToString(Funsetsuffix());
                if (lblPolicyValue != null && lblPolicyValue.Text.Trim() != "")
                    lblPolicyValue.Text = Convert.ToDecimal(lblPolicyValue.Text).ToString(Funsetsuffix());
                //if (lblPayment_Request_No.Text != "0")
                //{
                //    lnkEdit.Enabled = false;
                //    lnkRemove.Enabled = false;
                //}

                if (strMode == "Q")
                {
                    //lnkEdit.Enabled = false;
                    lnkRemove.Enabled = false;

                }
                else
                {
                    if (lnkRemove != null)

                        lnkRemove.Attributes.Add("OnClick", "return confirm('Do you want to delete?');");
                }

                if (lblCanEdit.Text != "0")
                {
                    //lnkEdit.Attributes.Add("OnClick", "alert('Claim has been raised. Cannot modify the details.');return false;");
                    lnkRemove.Attributes.Add("OnClick", "alert('Claim has been raised. Cannot delete the details.');return false;");
                }

                //if (ViewState["AllowModify"] != null)
                //{
                //    if (ViewState["AllowModify"].ToString() == "0")
                //    {
                //        //lnkEdit.Enabled = true;
                //        if (lnkRemove != null)
                //        {
                //            lnkRemove.Enabled = false;
                //            lnkRemove.Attributes.Add("Onclick", "");
                //        }

                //    }
                //}
                
            }
        }
        catch (Exception ex)
        {
            cvAssetInsuranceEntry.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_BindGrid + this.Page.Header.Title;
            cvAssetInsuranceEntry.IsValid = false;
        }
    }


    private void LoadEditItemAssetDescription(int row)
    {
        DropDownList ddlAssetDescription = gvCurrentInsurance.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
        ListItem[] lstAsset = new ListItem[ddlAssetDescription.Items.Count];
        ddlAssetDescription.Items.CopyTo(lstAsset, 0);

        DropDownList ddlEditAssetDescription = gvCurrentInsurance.Rows[row].FindControl("ddlAssetDescription") as DropDownList;

        ddlEditAssetDescription.DataSource = lstAsset;
        //ddlEditAssetDescription.DataValueField = "Asset_Number";
        //ddlEditAssetDescription.DataTextField = "MachineNo";
        ddlEditAssetDescription.DataBind();
    }

    private void FunPriGenerateEditPolicyRow(int row)
    {
        try
        {
               DropDownList ddlPolicyType = (DropDownList)gvCurrentInsurance.Rows[row].FindControl("ddlPolicyType");
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@OPTION", "10");
                ddlPolicyType.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
                DropDownList ddlAssetDescription = gvCurrentInsurance.Rows[row].FindControl("ddlAssetDescription") as DropDownList;
                Procparam.Clear();
                Procparam.Add("@OPTION", "1");

                if (intAINSEId > 0)
                {
                    if (ddlMLA.SelectedValue != "0")
                    {
                        Procparam.Add("@Panum", ddlMLA.SelectedValue);
                        Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedText);
                    }
                }
                else
                {
                    // if (ddlMLA.SelectedIndex != -1)
                    if (ddlMLA.SelectedValue != "0")
                    {
                        Procparam.Add("@Panum", ddlMLA.SelectedValue);
                        Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedText);
                    }
                }

                //ddlAssetDescription.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "asset_id", "asset_code", "Asset_Description" });
                //Label lblAssetDesc = gvCurrentInsurance.Rows[row].FindControl("lblAssetDesc") as Label;
                //ddlAssetDescription.SelectedValue = lblAssetDesc.Text;

                Label lblPolicyType = gvCurrentInsurance.Rows[row].FindControl("lblPolicyType") as Label;
                ddlPolicyType.SelectedValue = lblPolicyType.Text;
                Procparam = new Dictionary<string, string>();

                Procparam.Add("@Option", "5");
                if (ddlMLA.SelectedValue != "0")
                {
                    Procparam.Add("@Panum", ddlMLA.SelectedValue);
                    Procparam.Add("@Sanum", (ddlSLA.SelectedValue == "0") ? ddlMLA.SelectedValue + "DUMMY" : ddlSLA.SelectedText);
                    Procparam.Add("@AssetId", ddlAssetDescription.SelectedValue);
                    DropDownList ddlMachineNo = gvCurrentInsurance.Rows[row].FindControl("ddlMachineNo") as DropDownList;
                    ddlMachineNo.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Asset_Number", "MachineNo" });
                    Label lblAsset = gvCurrentInsurance.Rows[row].FindControl("lblMachineNo") as Label;
                    ddlMachineNo.SelectedValue = lblAsset.Text;
                }

            }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected string GVDateFormat(string val)
    {
        return Utility.StringToDate(val).ToString(strDateFormat);
    }
    protected string Funsetsuffix()
    {
        int suffix = 1;
        // S3GSession ObjS3GSession = new S3GSession();
        //suffix = ObjS3GSession.ProGpsSuffixRW;
        suffix = 0;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }
    protected void ddlPayTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        

        if (ddlPayTo.SelectedIndex > 0)
        {
            FunPriLoadEntityDetails();
        }
        else
        {
           S3GCompanyAddress.ClearCustomerDetails();
            //txtInsuranceCompanyName.Text =
            //txtInsuranceCompanyAddress.Text =
            //txtInsuranceCompanyAddress2.Text = string.Empty;
            //txtInsCompanyCity.SelectedValue =
            //txtInsCompanyCountry.SelectedValue =
            //txtInsCompanyState.SelectedValue = "0";
            //txtMobile.Text =
            //txtPINCode.Text =
            //txtTelephone.Text =
            //txtEmailId.Text =
            //txtWebsite.Text = string.Empty;
            //ViewState["DT_AssetInsEntryDetails"] = null;
            //dtAssetInsEntry = FunPriAssetInsEntryDataTable();
            //if (dtAssetInsEntry.Rows.Count == 0)
            //{
            //    FunPriInsertAssetInsEntryDataTable("", "", "", "", "", "", "", "", "", "-1", "-1", "0", "", "");
            //}
            //lblTotalPremium.Text = "0";
        }
    }

    private bool FunPriIsExistPolicyNo(string strPolicyNo, string strDetailsId)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Option", "18");
        Procparam.Add("@PolicyNo", strPolicyNo);
        Procparam.Add("@InsCompanyId", ddlPayTo.SelectedValue);
        if (strDetailsId != "")
        {
            Procparam.Add("@AccountInsDetailsID", strDetailsId);
        }
        DataTable dtPolicyNo = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);
        if (Convert.ToInt32(dtPolicyNo.Rows[0]["PolicyNoCount"]) > 0)
        {
            return false;
        }
        return true;
    }

    private void FunPriLoadEntityDetails()
    {
        Procparam = new Dictionary<string, string>();

        Procparam.Add("@Option", "16");
        if (ddlPayTo.SelectedValue != "0")
        {
            Procparam.Add("@Entity_ID", ddlPayTo.SelectedValue);
        }
        DataTable dtEntitydetails = Utility.GetDefaultData("S3G_INS_LOADLOV", Procparam);
        S3GCompanyAddress.SetCustomerDetails(dtEntitydetails.Rows[0], true);
    //    txtInsuranceCompanyAddress.Text = dtEntitydetails.Rows[0]["Address"].ToString();
    //    txtInsuranceCompanyAddress2.Text = dtEntitydetails.Rows[0]["Address2"].ToString();
    //    txtInsCompanyCity.Text = dtEntitydetails.Rows[0]["City"].ToString();
    //    txtInsCompanyState.Text = dtEntitydetails.Rows[0]["State"].ToString();
    //    txtInsCompanyCountry.Text = dtEntitydetails.Rows[0]["Country"].ToString();
    //    txtTelephone.Text = dtEntitydetails.Rows[0]["Telephone"].ToString();
    //    txtPINCode.Text = dtEntitydetails.Rows[0]["PinCode"].ToString();
    //    txtMobile.Text = dtEntitydetails.Rows[0]["Mobile"].ToString();
    //    txtEmailId.Text = dtEntitydetails.Rows[0]["EMail"].ToString();
    //    txtWebsite.Text = dtEntitydetails.Rows[0]["Website"].ToString();
    //    txtInsCompanyCity.ClearDropDownList();
    //    txtInsCompanyCountry.ClearDropDownList();
    //    txtInsCompanyState.ClearDropDownList();
    //    txtMobile.ReadOnly = txtPINCode.ReadOnly =
    //    txtTelephone.ReadOnly = txtEmailId.ReadOnly =
    //    txtWebsite.ReadOnly = true;
    }
    //private void FunsetInsuranceType(DropDownList DropDownListInsType)
    //{
    //    Dictionary<string, string> procparam = new Dictionary<string, string>();

    //    procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
    //    procparam.Add("@Ins_Type", DropDownListInsType.SelectedValue);

    //    DropDownListInsType.BindDataTable("S3G_Insurance_GetInsuranceType", procparam, new string[] { "Lookup_Code", "Lookup_Description" });
    //    return;
    //}

    //protected void DropDownListInsType_SelectedIndexChange(object sender, EventArgs e)
    //{
    //    if (DropDownListInsType.SelectedValue == "2" || DropDownListInsType.SelectedValue == "3")
    //    {
    //        gvCurrentInsurance.Columns[2].Visible = false;
    //        gvCurrentInsurance.Columns[3].Visible = false;
    //        gvCurrentInsurance.Columns[4].Visible = false;
    //        gvCurrentInsurance.Columns[5].Visible = true;
    //        gvCurrentInsurance.Columns[6].Visible = true;
    //    }
    //    else
    //    {
    //        gvCurrentInsurance.Columns[2].Visible = true;
    //        gvCurrentInsurance.Columns[3].Visible = true;
    //        gvCurrentInsurance.Columns[4].Visible = true;
    //        gvCurrentInsurance.Columns[5].Visible = false;
    //        gvCurrentInsurance.Columns[6].Visible = false;

    //    }
    //}
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
        Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Program_Id", "131");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }
}






