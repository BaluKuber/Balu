using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using S3GBusEntity;
using S3GBusEntity.LoanAdmin;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Globalization;
using System.IO;

public partial class LoanAdmin_S3GLOANADAssetIdentificationEntry_Add : ApplyThemeForProject
{
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    StringBuilder strAssetBuilder = new StringBuilder();
    string strAssetIDNo = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLOANADAssetIdentificationEntry_Add.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=ASI';";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    int IsAdd = 0;
    string strRedirectPage = "~/LoanAdmin/S3GLoanAdTransLander.aspx?Code=ASI";

    public static LoanAdmin_S3GLOANADAssetIdentificationEntry_Add obj_Page;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end


    AssetMgtServicesReference.AssetMgtServicesClient objAssetIdentification_Client;
    AssetMgtServices.S3G_LOANAD_AssetIdentificationDataTable objS3G_LOANAD_AssetDataTable = null;
    AssetMgtServices.S3G_LOANAD_AssetIdentificationRow objS3G_LOANAD_AssetDataRow = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        // WF Initializtion 
        ProgramCode = "060";

        //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        FunPubSetIndex(1);

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end
        //Date Format
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        //End
        if (intCompanyID == 0)
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
        if (intUserID == 0)
            intUserID = ObjUserInfo.ProUserIdRW;


        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
            strAssetIDNo = fromTicket.Name;
            IsAdd = 1;
        }

        //if (grvAsset.Rows.Count > 0)
        //{
        //    FileUpload flUpload = (FileUpload)grvAsset.Rows[0].FindControl("flUpload");
        //}

        obj_Page = this;

        if (!IsPostBack)
        {
            FunToolTip();
            PopulateLOBList();
            if (Request.QueryString["qsMode"] != "C")
            {
                PopulateBranchList();
            }
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            txtAIEDate.Text = DateTime.Now.ToString(strDateFormat);
            if (Request.QueryString["qsMode"] == "Q")
            {
                FunIdentifiedAssetForModification(strAssetIDNo);
                FunPriDisableControls(-1);
            }

            else if (Request.QueryString["qsMode"] == "M")
            {
                FunIdentifiedAssetForModification(strAssetIDNo);
                FunPriDisableControls(1);
            }

            else
            {
                FunPriDisableControls(0);
            }

            //divAstGrid.Style.Add("width", hdnScreenWidth.Value);
            //divAstGrid.Style.Add("width", "1300px");
            //divAstGrid.Style.Add("width", Request.Browser.ScreenPixelsWidth.ToString());
        }

        // WORK FLOW IMPLEMENTATION
        if (PageMode == PageModes.WorkFlow & !IsPostBack)
        {
            try
            {
                PreparePageForWorkFlowLoad();
            }
            catch (Exception ex)
            {
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
            }
        }


    }
    #region Workflow Methods
    /// <summary>
    /// Workflow Function
    /// </summary>
    private void PreparePageForWorkFlowLoad()
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            // Get The IDVALUE from Document Sequence #

            //DataTable HeaderValues = GetHeaderDetailsFromPANUMandSANUM(WFSessionValues.PANUM, ProgramCode, WFSessionValues.SANUM);
            PopulateLOBList();
            PopulateBranchList();
            ddlLOB.SelectedValue = WFSessionValues.LOBId.ToString();
            ddlBranch.SelectedValue = WFSessionValues.BranchID.ToString();
            PopulatePANum();
            ddlMLA.SelectedValue = WFSessionValues.PANUM;
            PopulateSANum(WFSessionValues.PANUM);
            if (WFSessionValues.SANUM != null)
                ddlSLA.SelectedValue = WFSessionValues.SANUM;
            LoadMLARelatedDetails();
        }
    }
    #endregion
    private void FunToolTip()
    {
        //throw new NotImplementedException();
        ddlBranch.ToolTip = lblBranch.Text;
        ddlLOB.ToolTip = lblLOB.Text;
        ddlMLA.ToolTip = lblPAN.Text;
        ddlSLA.ToolTip = lblSLA.Text;
        ddlInvoice.ToolTip = lblInvoice.Text;
        txtAIE.ToolTip = lblAIENo.Text;
        txtAIEDate.ToolTip = lblAIEDate.Text;
        btnSave.ToolTip = btnSave.Text;
        btnCancel.ToolTip = btnCancel.Text;
    }

    #region "LOV/Branch, LOB, PAN, SAN"

    private void PopulateBranchList()
    {
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //if (PageMode == PageModes.Create)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            //Procparam.Add("@User_ID", intUserID.ToString());
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //Procparam.Add("@Program_ID", "60");
          //  ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //ddlBranch.BindDataTable("S3G_ORG_GetBranchList", Procparam, new string[] { "Branch_ID", "Branch_Code" });

            //if (ddlBranch.Items.Count == 2)
            //{
            //    ddlBranch.Items.RemoveAt(0);
            //    ddlBranch.SelectedIndex = 0;

            //    FunClearControls(true);
            //    PopulatePANum();
            //}

            //if (ddlLOB.Items.Count > 1)
            //{
            //    ddlLOB.Focus();
            //}
            //else if (ddlBranch.Items.Count > 1)
            //{
            //    ddlBranch.Focus();
            //}
            //else
            //{
            //    ddlMLA.Focus();
            //}
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PopulateLOBList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_Id", Convert.ToString(intUserID));
            if (PageMode == PageModes.Create)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@Program_ID", "60");
            ddlLOB.BindDataTable(SPNames.S3G_LOANAD_LOBASSETIDEN, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //ddlLOB.BindDataTable("S3G_ORG_GetLOBDetails", Procparam, new string[] { "LOB_ID", "LOB" });

            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.Items.RemoveAt(0);
                ddlLOB.SelectedIndex = 0;

                //PopulatePANum();
            }

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClearControls(true);
        PopulatePANum();
        ddlBranch.Focus();
    }

    private void PopulatePANum()
    {
        try
        {
            //if (ddlLOB.SelectedValue != "0" && ddlBranch.SelectedValue != "0")
            //{
            //    Procparam = new Dictionary<string, string>();
            //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //    Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            //    Procparam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            //    Procparam.Add("@Type", "1");
            //    Procparam.Add("@Program_ID", "60");
            //    if (PageMode != PageModes.Create)
            //    {
            //        Procparam.Add("@PageMode", "1");
            //    }
            //    //ddlMLA.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "PANum", "PANum" });
            //    ddlMLA.BindDataTable("S3G_LOANAD_GetAccounts_AIE", Procparam, new string[] { "PANum", "PANum" });
            //}
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@Type", "1");
        Procparam.Add("@Program_ID", "60");
        if (obj_Page.PageMode != PageModes.Create)
        {
            Procparam.Add("@PageMode", "1");
        }

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAccounts_AIE_AGT", Procparam));

        return suggetions.ToArray();
    }


    // Added By Shibu 24-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", obj_Page.ProgramCode);
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadMLARelatedDetails();
    }

    void LoadMLARelatedDetails()
    {
        if (ddlMLA.SelectedValue !="0")
        {
            InitializeDropDownList(ddlInvoice);
            FunClearVendorDetails();
            PopulateSANum(ddlMLA.SelectedValue);
            PopulatePANCustomer(ddlMLA.SelectedValue);
        }
        else
        {
            ddlSLA.Items.Clear();
            ddlInvoice.Items.Clear();
            S3GCustomerAddress.ClearCustomerDetails();
            FunClearVendorDetails();
            grvAsset.DataSource = null;
            grvAsset.DataBind();
            divAstGrid.Style.Add("display", "none");
        }

        ddlMLA.Focus();
    }

    protected void FunProCheckInvoiceMandatory()
    {
        try
        {
            GetPASARefID();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            if (ddlSLA.SelectedValue.ToString() != "0")
            {
                Procparam.Add("@SANum", ddlSLA.SelectedValue.ToString());
            }
            else
            {
                Procparam.Add("@SANum", ddlMLA.SelectedValue.ToString() + "DUMMY");
            }
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OL"))
            {
                Procparam.Add("@IS_OL", "1");
            }
            else
            {
                Procparam.Add("@Is_OL", "0");
            }
            DataTable dtNewAssets = Utility.GetDefaultData("S3G_LOANAD_CheckForNewAssets", Procparam);
            if (dtNewAssets != null && dtNewAssets.Rows.Count > 0)     // Laoding Invoice based on the New assets availabilty
            {
                PopulateInvoiceNumber();
                lblInvoice.CssClass = "styleReqFieldLabel";
                rfvInvoice.Enabled = true;
            }
            else
            {
                LoadAssetGrid();  // added based on the CR
                lblInvoice.CssClass = "styleDisplayLabel";
                rfvInvoice.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PopulateSANum(string strPAN)
    {
        //throw new NotImplementedException();
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            Procparam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@Type", "2");
            Procparam.Add("@PANum", ddlMLA.SelectedValue);
            Procparam.Add("@Program_ID", "60");
            if (PageMode != PageModes.Create)
            {
                Procparam.Add("@PageMode", "1");
            }
            //ddlSLA.BindDataTable("S3G_LOANAD_GetPLASLA_AIE", Procparam, new string[] { "SANum", "SANum" });
            ddlSLA.BindDataTable("S3G_LOANAD_GetAccounts_AIE", Procparam, new string[] { "SANum", "SANum" });
            
            if (ddlSLA.Items.Count == 1)
            {
                FunClearControls(false);
                FunProCheckInvoiceMandatory();
                lblSLA.CssClass = "styleDisplayLabel";
                rfvddlSLA.Enabled = false;
                ddlSLA.Enabled = false;
            }
            else if (ddlSLA.Items.Count == 2)
            {
                ddlSLA.Items.RemoveAt(0);
                ddlSLA.SelectedIndex = 0;
                FunProSLAEventSelection();
                ddlSLA.Enabled = true;
            }
            else
            {
                lblSLA.CssClass = "styleReqFieldLabel";
                rfvddlSLA.Enabled = true;
                ddlSLA.Enabled = true;

                trAssetMessage.Visible = false;
                grvAsset.DataSource = null;
                grvAsset.DataBind();
                divAstGrid.Style.Add("display", "none");
            }


            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
            //Procparam.Add("@Param2", ddlMLA.SelectedItem.Text);
            //Procparam.Add("@Type", "Type6");
            //Procparam.Add("@Param1", "2");

            //DataSet Dset = new DataSet();

            //Dset = Utility.GetDataset("S3G_LOANAD_GetPLASLA_List", Procparam);
            //ddlSLA.BindDataTable(Dset.Tables[0], new string[] { "SANum", "SANum" });

            //string IsBaseMLA = "";
            //if (Dset != null)
            //{
            //    IsBaseMLA = Convert.ToString(Dset.Tables[1].Rows[0]["MLA_Applicable"]);

            //    if (IsBaseMLA != "" && IsBaseMLA == "1")
            //    {
            //        lblSLA.CssClass = "styleReqFieldLabel";
            //        rfvddlSLA.Enabled = true;

            //        if (ddlSLA.Items.Count == 1)
            //        {
            //            FunClearControls(false);
            //            ddlMLA.SelectedIndex = -1;
            //            Utility.FunShowAlertMsg(this.Page, "Sub account not exists. Cannot proceed further.");
            //            return;
            //        }
            //        else if (ddlSLA.Items.Count == 2)
            //        {
            //            ddlSLA.Items.RemoveAt(0);
            //            ddlSLA.SelectedIndex = 0;
            //            FunProSLAEventSelection();
            //        }
            //    }
            //    else
            //    {
            //        PopulateInvoiceNumber();
            //        GetPASARefID();
            //        lblSLA.CssClass = "styleDisplayLabel";
            //        rfvddlSLA.Enabled = false;
            //    }
            //}
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    #endregion

    #region "PAN realted Customer Details"

    private void PopulatePANCustomer(string strPAN)
    {
        objAssetIdentification_Client = new AssetMgtServicesReference.AssetMgtServicesClient();
        try
        {
            DataTable dtTable = new DataTable();
           

            byte[] bytesAsserDetails = objAssetIdentification_Client.FunGetPANumCustomer(strPAN);

            dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesAsserDetails, ObjSerMode, typeof(DataTable));

            S3GCustomerAddress.SetCustomerDetails(dtTable.Rows[0], true);
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.ToString();
        }
        finally
        {
            objAssetIdentification_Client.Close();
        }
    }

    #endregion

    #region "PAN SAN referenced Vendor Invoice Number"

    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProSLAEventSelection();
        ddlSLA.Focus();
    }

    protected void FunProSLAEventSelection()
    {
        FunClearVendorDetails();

        grvAsset.DataSource = null;
        grvAsset.DataBind();
        divAstGrid.Style.Add("display", "none");

        if (ddlSLA.SelectedValue.ToString() != "0")
        {
            FunProCheckInvoiceMandatory();
        }
    }

    private void GetPASARefID()
    {
        //throw new NotImplementedException();

        DataTable dtTable = new DataTable();
        //objAssetIdentification_Client = new AssetMgtServicesReference.AssetMgtServicesClient();

        //byte[] bytesAsserDetails = objAssetIdentification_Client.FunGetPASArefID(ddlMLA.SelectedValue, ddlSLA.SelectedValue);
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@PANum", ddlMLA.SelectedValue);
        if (ddlSLA.Items.Count == 1 && ddlSLA.SelectedValue == "0")
            Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
        else
            Procparam.Add("@SANum", ddlSLA.SelectedValue);

        dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetPASARefID, Procparam);
        if (dtTable != null && dtTable.Rows.Count > 0)
            ViewState["PASARefID"] = dtTable.Rows[0]["PA_SA_REF_ID"];


    }

    private void PopulateInvoiceNumber()
    {
        //throw new NotImplementedException();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@PANum", ddlMLA.SelectedValue);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            if (ddlSLA.Items.Count == 1 && ddlSLA.SelectedValue == "0")
                Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
            else
                Procparam.Add("@SANum", ddlSLA.SelectedValue);

            DataSet DsInvoice = new DataSet();
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
            {
                DsInvoice = Utility.GetDataset("S3G_LOANAD_GetInvoiceDetails_For_OL", Procparam);
                ddlInvoice.FillDataTable(DsInvoice.Tables[0], "Invoice_ID", "Invoice_No");
            }
            else
            {
                DsInvoice = Utility.GetDataset("S3G_LOANAD_GetPrePostDisbursementDocs", Procparam);
                ddlInvoice.DataSource = DsInvoice.Tables[2];
                ddlInvoice.DataValueField = "Invoice_ID";
                ddlInvoice.DataTextField = "Disp";
                ddlInvoice.DataBind();

                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlInvoice.Items.Insert(0, liSelect);
            }

            if (ddlInvoice.Items.Count == 0)
            {
                InitializeDropDownList(ddlInvoice);
            }
            FunClearVendorDetails();
            grvAsset.DataSource = null;
            grvAsset.DataBind();
            divAstGrid.Style.Add("display", "none");

            if (ddlInvoice.Items.Count == 2)
            {
                ddlInvoice.Items.RemoveAt(0);
                ddlInvoice.SelectedIndex = 0;

                PopulateVendorCode(Convert.ToInt32(ddlInvoice.SelectedValue));
                GetPASARefID();
                if (PageMode == PageModes.Create || PageMode == PageModes.WorkFlow)
                {
                    LoadAssetGrid();
                }
            }

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region "Invoice based Vendor Details"

    protected void ddlInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInvoice.SelectedIndex > 0)
        {
            PopulateVendorCode(Convert.ToInt32(ddlInvoice.SelectedValue));
            LoadAssetGrid();
        }
        else
        {
            FunClearVendorDetails();
            grvAsset.DataSource = null;
            grvAsset.DataBind();
            divAstGrid.Style.Add("display", "none");
        }

        ddlInvoice.Focus();
    }

    private void PopulateVendorCode(int intVendorID)
    {

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Invoice_ID", ddlInvoice.SelectedValue.ToString());
        Procparam.Add("@Company_Id", intCompanyID.ToString());
        DataTable dtTable = Utility.GetDefaultData("S3G_LOANAD_GetAssetVendorDetails", Procparam);
        if (dtTable.Rows.Count > 0)
        {
            S3GVendorAddress.SetCustomerDetails(dtTable.Rows[0]["Entity_Code"].ToString(),
             dtTable.Rows[0]["Address"].ToString(), dtTable.Rows[0]["Entity_Name"].ToString(),
             dtTable.Rows[0]["Telephone"].ToString(),
             dtTable.Rows[0]["Mobile"].ToString(),
             dtTable.Rows[0]["EMail"].ToString(), dtTable.Rows[0]["Website"].ToString());
            txtInvoiceDate.Text = DateTime.Parse(dtTable.Rows[0]["Vendor_Invoice_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            ViewState["Vendor_ID"] = dtTable.Rows[0]["Vendor_ID"];
        }
        else
        {
            S3GVendorAddress.ClearCustomerDetails();
            txtInvoiceDate.Text = string.Empty;
        }
    }

    #endregion



    #region "Asset Details stored in XML format"
    /// <summary>
    /// Storing the Values of the grid as XML documnet for bulk insert in the Data base
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    private void LoadXMLValues()
    {
        // Grid sent as XMl
        strAssetBuilder.Append("<Root>");
        DataTable dtTable = new DataTable();
        dtTable.Columns.Add("REGN_NUMBER");
        dtTable.Columns.Add("ENGINE_NUMBER");
        dtTable.Columns.Add("CHASIS_NUMBER");
        dtTable.Columns.Add("SERIAL_NUMBER");
        dtTable.Columns.Add("INSTALLATION_REF_NO");
        dtTable.Columns.Add("INSTALLATION_DATE");
        dtTable.Columns.Add("INSTALLATION_DOC_PATH");
        DataRow dtRow;

        int intSlNo = 1;

        foreach (GridViewRow grvRow in grvAsset.Rows)
        {
            TextBox txtRegNo = (TextBox)grvRow.FindControl("txtRegNo");
            TextBox txtEngineNo = (TextBox)grvRow.FindControl("txtEngineNo");
            TextBox txtChassisNo = (TextBox)grvRow.FindControl("txtChassisNo");
            TextBox txtInstallationNo = (TextBox)grvRow.FindControl("txtInstallationNo");
            TextBox txtInstallationDate = (TextBox)grvRow.FindControl("txtInstallationDate");
            TextBox txtSerialNo = (TextBox)grvRow.FindControl("txtSerialNo");
            Label lblAssetID = (Label)grvRow.FindControl("lblAssetID");
            Label lblSINO = (Label)grvRow.FindControl("lblSINO");
            Label lblActualPath = (Label)grvRow.FindControl("lblActualPath");
            Label lblCurrentPath = (Label)grvRow.FindControl("lblCurrentPath");
            CheckBox chkSelect = (CheckBox)grvRow.FindControl("chkSelect");

            string strFilePath = "";
            if (chkSelect.Checked)
            {
                strFilePath = lblCurrentPath.Text;
            }
            else
            {
                strFilePath = lblActualPath.Text;
            }

            //Assed By Thangam M on 14/Feb/2012 to fix date issue in oracle
            string strInstDate = "";
            strInstDate = txtInstallationDate.Text == "" ? "" : Utility.StringToDate(txtInstallationDate.Text).ToString();

            if (PageMode == PageModes.Create)
            {
                if (!string.IsNullOrEmpty(txtRegNo.Text.Trim()) || !string.IsNullOrEmpty(txtEngineNo.Text.Trim()) ||
                    !string.IsNullOrEmpty(txtChassisNo.Text.Trim()) || !string.IsNullOrEmpty(txtInstallationNo.Text.Trim()) ||
                    !string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
                {
                    strAssetBuilder.Append("<Details ID ='" + intSlNo.ToString() + "' ASSST_CODE = '" + grvRow.Cells[2].Text.ToString() + "' REGN_NUMBER ='" + txtRegNo.Text + "' ENGINE_NUMBER='" + txtEngineNo.Text + "' PA_SA_REF_ID='" + ViewState["PASARefID"].ToString() + "' CHASIS_NUMBER='"
                    + txtChassisNo.Text + "' INSTALLATION_REF_NO='" + txtInstallationNo.Text + "' INSTALLATION_DATE ='" + strInstDate + "' INSTALLATION_DOC_PATH ='" + strFilePath +
                    "' SERIAL_NUMBER='" + txtSerialNo.Text + "' ASSET_ID='" + lblAssetID.Text + "' /> ");

                    intSlNo = intSlNo + 1;
                }
            }
            else
            {
                strAssetBuilder.Append("<Details ID ='" + lblSINO.Text + "' ASSST_CODE = '" + grvRow.Cells[2].Text.ToString() + "' REGN_NUMBER ='" + txtRegNo.Text + "' ENGINE_NUMBER='" + txtEngineNo.Text + "' PA_SA_REF_ID='" + ViewState["PASARefID"].ToString() + "' CHASIS_NUMBER='"
                   + txtChassisNo.Text + "' INSTALLATION_REF_NO='" + txtInstallationNo.Text + "' INSTALLATION_DATE ='" + strInstDate + "' INSTALLATION_DOC_PATH ='" + strFilePath +
                   "' SERIAL_NUMBER='" + txtSerialNo.Text + "' ASSET_ID='" + lblAssetID.Text + "' /> ");
            }

            dtRow = dtTable.NewRow();
            dtRow["REGN_NUMBER"] = txtRegNo.Text.Trim();
            dtRow["ENGINE_NUMBER"] = txtEngineNo.Text.Trim();
            dtRow["CHASIS_NUMBER"] = txtChassisNo.Text.Trim();
            dtRow["INSTALLATION_REF_NO"] = txtInstallationNo.Text.Trim();
            dtRow["INSTALLATION_DATE"] = txtInstallationDate.Text.Trim();
            if (chkSelect.Checked)
            {
                dtRow["INSTALLATION_DOC_PATH"] = lblActualPath.Text.Trim();
            }
            dtRow["SERIAL_NUMBER"] = txtSerialNo.Text.Trim();
            dtTable.Rows.Add(dtRow);
        }

        ViewState["Table"] = dtTable;
        strAssetBuilder.Append("</Root>");
    }

    #endregion

    private void LoadAssetGrid()
    {
        DataSet dtSet = new DataSet();
        int intCount;
        DataTable dtTableAsset = new DataTable();
        DataTable dtTable = new DataTable();

        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
        {
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            if (ddlInvoice.SelectedValue.ToString() != "0")
            {
                Procparam.Add("@Invoice_No", ddlInvoice.SelectedValue.ToString());
            }
            Procparam.Add("@PANum", ddlMLA.SelectedValue.ToString());
            if (ddlSLA.Items.Count > 0 && ddlSLA.SelectedValue.ToString() != "0")
            {
                Procparam.Add("@SANum", ddlSLA.SelectedValue.ToString());
            }

            dtTableAsset = Utility.GetDefaultData("S3G_LOANAD_GetAssetDetailsForOL", Procparam);
        }
        else
        {
            if (ddlInvoice.SelectedValue.ToString() != "0")
            {
                Procparam.Add("@Invoice_ID", ddlInvoice.SelectedValue);
            }
            Procparam.Add("@PA_SA_Ref_ID", ViewState["PASARefID"].ToString());
            if (PageMode == PageModes.Create)
            {
                Procparam.Add("@Mode", "0");
            }
            else
            {
                Procparam.Add("@Mode", "1");
            }

            dtTableAsset = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAssetDetailsForInvoice, Procparam);
        }

        if (dtTableAsset.Rows.Count > 0)
        {
            dtTableAsset.Columns.Add("INSTALLATION_REF_NO");
            dtTableAsset.Columns.Add("INSTALLATION_DATE");
            dtTableAsset.Columns.Add("INSTALLATION_DOC_PATH");
            dtTableAsset.Columns.Add("REGN_NUMBER");
            dtTableAsset.Columns.Add("ENGINE_NUMBER");
            dtTableAsset.Columns.Add("CHASIS_NUMBER");
            dtTableAsset.Columns.Add("SERIAL_NUMBER");
            grvAsset.DataSource = dtTableAsset;
            grvAsset.DataBind();
            trAssetMessage.Visible = false;
            FunProToggleSaveButton(true);
            divAstGrid.Style.Add("display", "block");
        }
        else
        {
            grvAsset.DataSource = null;
            grvAsset.DataBind();
            trAssetMessage.Visible = true;
            FunProToggleSaveButton(false);
            divAstGrid.Style.Add("display", "none");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('The number of Asset units do not match');" + strRedirectPageAdd, true);
        }


    }

    public string GetColumnValue(DropDownList MyDLL, string strColumnName, DataTable Dt)
    {
        try
        {
            if (Dt != null)
            {
                DataRow[] DRows = Dt.Select(MyDLL.DataValueField.ToString() + " like '" + MyDLL.SelectedValue.ToString() + "%'");

                foreach (DataRow dr in DRows)
                {
                    return dr[strColumnName].ToString();
                }
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    #region "Saving Details"

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        if (grvAsset != null && grvAsset.Rows.Count == 0)
        {
            strAlert = strAlert.Replace("__ALERT__", "Cannot save record without asset details");
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
        }



        if (IsNotBlank())
        {
            FunProUploadFiles();
            LoadXMLValues();

            //Changed By Thangam M on 18/Nov/2011
            if (CheckRowUniquenes() && FunProCheckInsDate())
            {
                objAssetIdentification_Client = new AssetMgtServicesReference.AssetMgtServicesClient();
                Label lblAssetID = ((Label)grvAsset.FindControl("lblAssetID"));
                try
                {
                    //if (CheckRowUniquenes())
                    //{
                    //if (Convert.ToInt32(ViewState["Count"]) >= Convert.ToInt32(ViewState["Units"]))
                    //{
                    //    if (Convert.ToInt32(ViewState["Count"]) != 0)
                    //    {
                    //        //return;
                    //    }
                    //}

                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('The number of Asset units do not match ');", true);
                    //    return;
                    //}
                    objS3G_LOANAD_AssetDataTable = new AssetMgtServices.S3G_LOANAD_AssetIdentificationDataTable();
                    objS3G_LOANAD_AssetDataRow = objS3G_LOANAD_AssetDataTable.NewS3G_LOANAD_AssetIdentificationRow();

                    objS3G_LOANAD_AssetDataRow.Asset_Identification_No = strAssetIDNo;
                    objS3G_LOANAD_AssetDataRow.Company_ID = intCompanyID;
                    objS3G_LOANAD_AssetDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                    objS3G_LOANAD_AssetDataRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
                    objS3G_LOANAD_AssetDataRow.Asset_Identification_Date = DateTime.Now;

                    objS3G_LOANAD_AssetDataRow.Vendor_ID = Convert.ToInt32(ViewState["Vendor_ID"]);
                    objS3G_LOANAD_AssetDataRow.Vendor_Invoice_ID = Convert.ToInt32(ddlInvoice.SelectedValue);

                    objS3G_LOANAD_AssetDataRow.Created_By = intUserID;
                    objS3G_LOANAD_AssetDataRow.Created_Date = DateTime.Now;

                    objS3G_LOANAD_AssetDataRow.IsAdd = IsAdd;

                    objS3G_LOANAD_AssetDataRow.PA_SA_ID = Convert.ToInt32(ViewState["PASARefID"]);
                    objS3G_LOANAD_AssetDataRow.XMLAccount = strAssetBuilder.ToString();

                    if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
                    {
                        objS3G_LOANAD_AssetDataRow.IsOL = 1;
                    }
                    else
                    {
                        objS3G_LOANAD_AssetDataRow.IsOL = 0;
                    }

                    objS3G_LOANAD_AssetDataTable.AddS3G_LOANAD_AssetIdentificationRow(objS3G_LOANAD_AssetDataRow);

                    string strDuplication = "";
                    string strDSNo = "";
                    intErrCode = objAssetIdentification_Client.FunPubCreateAssetIdentification(out strDuplication, out strDSNo, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_AssetDataTable, ObjSerMode));

                    if (intErrCode == 0)
                    {
                        if (isWorkFlowTraveler)
                        {
                            WorkFlowSession WFValues = new WorkFlowSession();
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, "", WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                                btnSave.Enabled = false;
                                //END
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow not Assigned";
                            }
                            ShowWFAlertMessage(strDSNo, ProgramCode, strAlert);
                            return;
                        }
                        else if (strAssetIDNo == string.Empty)
                        {
                            // FORCE PULL IMPLEMENTATION KR
                            DataTable WFFP = new DataTable();

                            if (CheckForForcePullOperation(ProgramCode, ddlMLA.SelectedValue, null, "L", CompanyId, out WFFP))
                            {
                                DataRow dtrForce = WFFP.Rows[0];
                                try
                                {
                                    int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), ddlMLA.SelectedValue, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), ddlMLA.SelectedValue, "", int.Parse(dtrForce["PRODUCTID"].ToString()));
                                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                                    btnSave.Enabled = false;
                                    //END
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            strAlert = "Asset Identification details added successfully - " + strDSNo;
                            strAlert += @"\n\nWould you like to add one more record?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END

                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        }
                        else
                        {
                            //strAlert = strAlert.Replace("__ALERT__", "Asset Verification details updated successfully");
                            //strRedirectPageView = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Details updated successfully');" + strRedirectPageView, true);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }

                        FunProClearCaches();
                    }
                    else if (intErrCode == -1)
                    {
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    }
                    else if (intErrCode == -2)
                    {
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    }
                    else if (intErrCode == 4)
                    {
                        custAsset.ErrorMessage = "Registration number should be unique. '" + strDuplication + "' already exists";
                        custAsset.IsValid = false;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Registration number should be unique." + @"\n\n" + strDuplication + " already exist');", true);
                        return;
                    }
                    else if (intErrCode == 5)
                    {
                        custAsset.ErrorMessage = "Engine number should be unique. '" + strDuplication + "' already exists";
                        custAsset.IsValid = false;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Engine number should be unique." + @"\n\n" + strDuplication + " already exist');", true);
                        return;
                    }
                    else if (intErrCode == 6)
                    {
                        custAsset.ErrorMessage = "Chassis number should be unique. '" + strDuplication + "' already exists";
                        custAsset.IsValid = false;
                        // ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Chassis number should be unique." + @"\n\n" + strDuplication + " already exist');", true);
                        return;
                    }
                    else if (intErrCode == 7)
                    {
                        custAsset.ErrorMessage = "Installation Ref number should be unique. '" + strDuplication + "' already exists";
                        custAsset.IsValid = false;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Installation Ref No should be unique." + @"\n\n" + strDuplication + " already exist');", true);
                        return;
                    }
                    else if (intErrCode == 8)
                    {
                        custAsset.ErrorMessage = "Serial number should be unique. '" + strDuplication + "' already exists";
                        custAsset.IsValid = false;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Serail Number should be unique." + @"\n\n" + strDuplication + " already exist');", true);
                        return;
                    }
                    else if (intErrCode == 50)
                    {
                        custAsset.ErrorMessage = "Error in saving details.";
                        custAsset.IsValid = false;
                        return;
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    objAssetIdentification_Client.Close();
                }
            }
        }
    }

    private bool CheckRowUniquenes()
    {
        //throw new NotImplementedException();
        bool blnIsUnique = true;
        DataTable dtTable = new DataTable();
        dtTable = (DataTable)ViewState["Table"];

        int intCount = grvAsset.Rows.Count - 1;

        if (dtTable != null)
        {
            for (int intRowsi = 0; intRowsi <= intCount; intRowsi++)
            {
                for (int intRowsj = 0; intRowsj <= intCount; intRowsj++)
                {
                    if (intRowsi != intRowsj)
                    {
                        if (!string.IsNullOrEmpty(dtTable.Rows[intRowsi]["REGN_NUMBER"].ToString()) && dtTable.Rows[intRowsi]["REGN_NUMBER"].ToString().Trim().ToUpper() == dtTable.Rows[intRowsj]["REGN_NUMBER"].ToString().Trim().ToUpper())
                        {
                            custAsset.ErrorMessage = "Registration Number should be unique";
                            custAsset.IsValid = false;
                            blnIsUnique = false;
                            return blnIsUnique;
                        }
                        else if (!string.IsNullOrEmpty(dtTable.Rows[intRowsi]["ENGINE_NUMBER"].ToString()) && dtTable.Rows[intRowsi]["ENGINE_NUMBER"].ToString().Trim().ToUpper() == dtTable.Rows[intRowsj]["ENGINE_NUMBER"].ToString().Trim().ToUpper())
                        {
                            custAsset.ErrorMessage = "Engine Number should be unique";
                            custAsset.IsValid = false;
                            blnIsUnique = false;
                            return blnIsUnique;
                        }
                        else if (!string.IsNullOrEmpty(dtTable.Rows[intRowsi]["CHASIS_NUMBER"].ToString()) && dtTable.Rows[intRowsi]["CHASIS_NUMBER"].ToString().Trim().ToUpper() == dtTable.Rows[intRowsj]["CHASIS_NUMBER"].ToString().Trim().ToUpper())
                        {
                            custAsset.ErrorMessage = "Chassis Number should be unique";
                            custAsset.IsValid = false;
                            blnIsUnique = false;
                            return blnIsUnique;
                        }
                        else if (!string.IsNullOrEmpty(dtTable.Rows[intRowsi]["INSTALLATION_REF_NO"].ToString()) && dtTable.Rows[intRowsi]["INSTALLATION_REF_NO"].ToString().Trim().ToUpper() == dtTable.Rows[intRowsj]["INSTALLATION_REF_NO"].ToString().Trim().ToUpper())
                        {
                            custAsset.ErrorMessage = "Installation Number should be unique";
                            custAsset.IsValid = false;
                            blnIsUnique = false;
                            return blnIsUnique;
                        }
                    }
                }
            }
        }
        return blnIsUnique;
    }

    #endregion

    #region "Identified Assets for modification"

    private void FunIdentifiedAssetForModification(string strAssetIDNo)
    {
        //throw new NotImplementedException();
        objAssetIdentification_Client = new AssetMgtServicesReference.AssetMgtServicesClient();
        try
        {
            DataTable dtTable = new DataTable();
           

            byte[] bytesAsserDetails = objAssetIdentification_Client.FunGetAssetIdentificationforModify(strAssetIDNo, intCompanyID, 1);

            dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesAsserDetails, ObjSerMode, typeof(DataTable));

            txtAIE.Text = dtTable.Rows[0]["Asset_Identification_No"].ToString();
            txtAIEDate.Text = DateTime.Parse(dtTable.Rows[0]["Asset_Identification_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            ddlLOB.SelectedValue = dtTable.Rows[0]["LOB_ID"].ToString();
            ddlBranch.SelectedValue = dtTable.Rows[0]["Location_ID"].ToString();
            ddlBranch.SelectedText = dtTable.Rows[0]["Location_Name"].ToString();
            ddlBranch.ToolTip = dtTable.Rows[0]["Location_Name"].ToString();

            ViewState["PASARefID"] = dtTable.Rows[0]["PA_SA_REF_ID"].ToString();

           // PopulatePANum();
            ddlMLA.SelectedValue = dtTable.Rows[0]["PANum"].ToString();
            ddlMLA.SelectedText = dtTable.Rows[0]["PANum"].ToString();
          //  PopulateSANum(ddlMLA.SelectedValue);
            if (!dtTable.Rows[0]["SANum"].ToString().Contains("DUMMY"))
            {
                ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem(dtTable.Rows[0]["SANum"].ToString(), dtTable.Rows[0]["SANum"].ToString()));
                ddlSLA.ToolTip = dtTable.Rows[0]["SANum"].ToString();
            }
            else
            {
                ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem("--Select--", "0"));
                rfvddlSLA.Enabled = false;
            }
           // ddlSLA.SelectedValue = dtTable.Rows[0]["SANum"].ToString();
            PopulateInvoiceNumber();
            ddlInvoice.SelectedValue = dtTable.Rows[0]["Invoice_ID"].ToString();
            PopulateVendorCode(Convert.ToInt32(ddlInvoice.SelectedValue));
            S3GCustomerAddress.SetCustomerDetails(dtTable.Rows[0], true);

            //S3GVendorAddress.SetCustomerDetails(dtTable.Rows[0]["Entity_Code"].ToString(),
            //     dtTable.Rows[0]["Address"].ToString(), dtTable.Rows[0]["Entity_Name"].ToString(),
            //     dtTable.Rows[0]["Mobile"].ToString(),
            //     dtTable.Rows[0]["EMail"].ToString(), dtTable.Rows[0]["Website"].ToString());

            //if (!string.IsNullOrEmpty(dtTable.Rows[0]["Vendor_Invoice_Date"].ToString()))
            //{
            //    txtInvoiceDate.Text = DateTime.Parse(dtTable.Rows[0]["Vendor_Invoice_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //}

            grvAsset.DataSource = dtTable;
            grvAsset.DataBind();
            if (dtTable.Rows.Count > 0)
            {
                divAstGrid.Style.Add("display", "block");
            }
            else
            {
                divAstGrid.Style.Add("display", "none");
            }
            trAssetMessage.Visible = false;
        }
        catch (Exception ex)
        {

        }
        finally
        {
            objAssetIdentification_Client.Close();
        }
    }

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FunProClearCaches();

        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect(strRedirectPage,false);
    }

    #region "User Authorization"


    ////This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                if (!bCreate)
                {
                    btnSave.Enabled = false;

                }

                grvAsset.Columns[11].Visible = false;

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                ddlLOB.Enabled = ddlBranch.Enabled = ddlMLA.Enabled = ddlSLA.Enabled = ddlInvoice.Enabled = false;
                break;


            case -1:// Query Mode


                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
               // grvAsset.Columns[grvAsset.Columns.Count-2].Visible = false;
                foreach (GridViewRow grvRow in grvAsset.Rows)
                {
                    TextBox txtRegNo = (TextBox)grvRow.FindControl("txtRegNo");
                    TextBox txtEngineNo = (TextBox)grvRow.FindControl("txtEngineNo");
                    TextBox txtChassisNo = (TextBox)grvRow.FindControl("txtChassisNo");
                    TextBox txtInstallationNo = (TextBox)grvRow.FindControl("txtInstallationNo");
                    TextBox txtInstallationDate = (TextBox)grvRow.FindControl("txtInstallationDate");
                    TextBox txtSerialNo = (TextBox)grvRow.FindControl("txtSerialNo");
                    FileUpload flUpload = (FileUpload)grvRow.FindControl("flUpload");
                    txtRegNo.ReadOnly = txtEngineNo.ReadOnly = txtChassisNo.ReadOnly = txtInstallationNo.ReadOnly = txtInstallationDate.ReadOnly = txtSerialNo.ReadOnly = true;
                    flUpload.Enabled = false;
                }

                if (bClearList)
                {
                    //ddlBranch.Clear();
                    ddlLOB.ClearDropDownList();
                    ddlMLA.ReadOnly = true;
                    ddlSLA.ClearDropDownList();
                    if (ddlInvoice.Items.Count > 0)
                    {
                        ddlInvoice.ClearDropDownList();
                    }
                }

                //grvAsset.Columns[10].Visible = false;
                break;
        }

    }

    ////Code end


    #endregion

    #region "Asset Validation"

    protected void custAsset_ServerValidate(object sender, ServerValidateEventArgs args)
    {

    }


    protected bool IsNotBlank()
    {
        int iCount = 0;
        foreach (GridViewRow grvRow in grvAsset.Rows)
        {
            TextBox txtRegNo = (TextBox)grvRow.FindControl("txtRegNo");
            TextBox txtEngineNo = (TextBox)grvRow.FindControl("txtEngineNo");
            TextBox txtChassisNo = (TextBox)grvRow.FindControl("txtChassisNo");
            TextBox txtInstallationNo = (TextBox)grvRow.FindControl("txtInstallationNo");
            TextBox txtSerialNo = (TextBox)grvRow.FindControl("txtSerialNo");
            Label lblAssetType = (Label)grvRow.FindControl("lblAssetType");


            if (!string.IsNullOrEmpty(txtRegNo.Text.Trim()) || !string.IsNullOrEmpty(txtEngineNo.Text.Trim()) ||
                !string.IsNullOrEmpty(txtChassisNo.Text.Trim()) || !string.IsNullOrEmpty(txtInstallationNo.Text.Trim()) ||
               !string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (lblAssetType.Text == "94" && string.IsNullOrEmpty(txtRegNo.Text.Trim()))
                {
                    txtRegNo.Focus();
                    custAsset.IsValid = false;
                    custAsset.ErrorMessage = "Registration Number required for " + grvRow.Cells[3].Text + " in Sl.No " + Convert.ToString(grvRow.RowIndex + 1);
                    return false;
                }
                else if ((lblAssetType.Text == "94" || lblAssetType.Text == "93") && string.IsNullOrEmpty(txtChassisNo.Text.Trim()))
                {
                    txtChassisNo.Focus();
                    custAsset.IsValid = false;
                    custAsset.ErrorMessage = "Chassis Number required for " + grvRow.Cells[3].Text + " in Sl.No " + Convert.ToString(grvRow.RowIndex + 1);
                    return false;
                }

                //else if (lblAssetType.Text == "93" && string.IsNullOrEmpty(txtChassisNo.Text.Trim()))
                //{
                //    txtSerialNo.Focus();
                //    custAsset.IsValid = false;
                //    custAsset.ErrorMessage = "Serial Number required for " + grvRow.Cells[3].Text + " in Sl.No " + Convert.ToString(grvRow.RowIndex + 1);
                //    return false;
                //}
                if (grvRow.RowIndex == grvAsset.Rows.Count - 1)
                {
                    return true;
                }
            }
            else if (PageMode == PageModes.Modify)
            {
                if (lblAssetType.Text == "94" && string.IsNullOrEmpty(txtRegNo.Text.Trim()))
                {
                    txtRegNo.Focus();
                    custAsset.IsValid = false;
                    custAsset.ErrorMessage = "Registration Number required for " + grvRow.Cells[3].Text + " in Sl.No " + Convert.ToString(grvRow.RowIndex + 1);
                    return false;
                }
                else if ((lblAssetType.Text == "94" || lblAssetType.Text == "93") && string.IsNullOrEmpty(txtChassisNo.Text.Trim()))
                {
                    txtChassisNo.Focus();
                    custAsset.IsValid = false;
                    custAsset.ErrorMessage = "Chassis Number required for " + grvRow.Cells[3].Text + " in Sl.No " + Convert.ToString(grvRow.RowIndex + 1);
                    return false;
                }
                else if (string.IsNullOrEmpty(txtRegNo.Text.Trim()) && string.IsNullOrEmpty(txtEngineNo.Text.Trim()) &&
                   string.IsNullOrEmpty(txtChassisNo.Text.Trim()) && string.IsNullOrEmpty(txtInstallationNo.Text.Trim()) &&
                   string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
                {
                    custAsset.IsValid = false;
                    custAsset.ErrorMessage = "Enter any one of the valid Identifications in Sl.No " + Convert.ToString(grvRow.RowIndex + 1);
                    return false;
                }
            }
            else
            {
                iCount++;
            }




            //if (PageMode == PageModes.Create)
            //{
            //    if (!string.IsNullOrEmpty(txtRegNo.Text.Trim()) || !string.IsNullOrEmpty(txtEngineNo.Text.Trim()) ||
            //    !string.IsNullOrEmpty(txtChassisNo.Text.Trim()) || !string.IsNullOrEmpty(txtInstallationNo.Text.Trim()) ||
            //    !string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        iCount++;
            //    }
            //}
            //else
            //{
            //    if (string.IsNullOrEmpty(txtRegNo.Text.Trim()) && string.IsNullOrEmpty(txtEngineNo.Text.Trim()) &&
            //        string.IsNullOrEmpty(txtChassisNo.Text.Trim()) && string.IsNullOrEmpty(txtInstallationNo.Text.Trim()) &&
            //        string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            //    {
            //        custAsset.IsValid = false;
            //        custAsset.ErrorMessage = "Enter any one of the valid identifications in Sl.No " + Convert.ToString(grvRow.RowIndex + 1);
            //        return false;
            //    }
            //}
        }

        //if (PageMode == PageModes.Create || ddlMLA.SelectedItem.Text.Contains("OL"))
        //{
        if (iCount == grvAsset.Rows.Count)
        {
            custAsset.IsValid = false;
            custAsset.ErrorMessage = "Atleast one asset needs to be identified to save.";
            return false;
        }      

        return true;
        //}
        //else
        //{
        //    return true;
        //}
    }

    //Method Added By Thangam M on 18/Nov/2011 to fix UAT bug id L.AID_008
    protected bool FunProCheckInsDate()
    {
        foreach (GridViewRow grvRow in grvAsset.Rows)
        {
            TextBox txtInstallationDate = (TextBox)grvRow.FindControl("txtInstallationDate");

            if (ddlInvoice.SelectedValue != "0" && !string.IsNullOrEmpty(txtInstallationDate.Text))
            {
                if (Utility.StringToDate(txtInstallationDate.Text) < Utility.StringToDate(txtInvoiceDate.Text))
                {
                    custAsset.IsValid = false;
                    custAsset.ErrorMessage = "Installation Date should be greater than or equal to Invoice Date [" + txtInvoiceDate.Text + "] in Sl.No " + Convert.ToString(grvRow.RowIndex + 1);
                    return false;
                }
            }
        }

        return true;
    }

    #endregion

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunClearControls(true);

            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            //if (PageMode == PageModes.Create)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            //Procparam.Add("@User_ID", intUserID.ToString());
            //Procparam.Add("@Company_ID", intCompanyID.ToString());
            //Procparam.Add("@Program_ID", "60");
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });

            //ddlBranch.SelectedIndex = 0;
            PopulatePANum();

            ddlLOB.Focus();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void FunClearControls(bool ClearPAN)
    {
        try
        {
            if (ClearPAN)
            {
                //InitializeDropDownList(ddlMLA);
                ddlMLA.Clear();
            }
            InitializeDropDownList(ddlSLA);
            InitializeDropDownList(ddlInvoice);

            FunClearVendorDetails();

            S3GCustomerAddress.ClearCustomerDetails();

            trAssetMessage.Visible = false;
            grvAsset.DataSource = null;
            grvAsset.DataBind();
            divAstGrid.Style.Add("display", "none");

            FunProToggleSaveButton(true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void FunProToggleSaveButton(bool CanEnable)
    {
        if (PageMode == PageModes.Create)
        {
            if (bCreate && CanEnable)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }
    }

    protected void FunClearVendorDetails()
    {
        try
        {
            S3GVendorAddress.ClearCustomerDetails();
            txtInvoiceDate.Text = "";
            trAssetMessage.Visible = false;
            FunProToggleSaveButton(true);

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void InitializeDropDownList(DropDownList ddlSource)
    {
        ddlSource.Items.Clear();
        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
        ddlSource.Items.Insert(0, liSelect);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = 0;
            //ddlBranch.SelectedIndex = 0;
            ddlBranch.Clear();
            FunClearControls(true);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void grvAsset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtRegNo = (TextBox)e.Row.FindControl("txtRegNo");
            TextBox txtEngineNo = (TextBox)e.Row.FindControl("txtEngineNo");
            TextBox txtChassisNo = (TextBox)e.Row.FindControl("txtChassisNo");
            TextBox txtInstallationNo = (TextBox)e.Row.FindControl("txtInstallationNo");
            TextBox txtSerialNo = (TextBox)e.Row.FindControl("txtSerialNo");
            TextBox txtInstallationDate = (TextBox)e.Row.FindControl("txtInstallationDate");
            AjaxControlToolkit.CalendarExtender CEInsDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CEInsDate");
            FileUpload flUpload = (FileUpload)e.Row.FindControl("flUpload");
            Button btnBrowse = (Button)e.Row.FindControl("btnBrowse");
            Button btnDlg = (Button)e.Row.FindControl("btnDlg");
            HiddenField hdnSelectedPath = (HiddenField)e.Row.FindControl("hdnSelectedPath");
            Label lblActualPath = (Label)e.Row.FindControl("lblActualPath");
            ImageButton BtnView = (ImageButton)e.Row.FindControl("BtnView");

            txtRegNo.Attributes.Add("onblur", "javascript:FunCheckForZero('" + txtRegNo.ClientID + "', 'Registration Number ');");
            txtEngineNo.Attributes.Add("onblur", "javascript:FunCheckForZero('" + txtEngineNo.ClientID + "', 'Engine Number ');");
            txtChassisNo.Attributes.Add("onblur", "javascript:FunCheckForZero('" + txtChassisNo.ClientID + "', 'Chassis Number ');");
            txtInstallationNo.Attributes.Add("onblur", "javascript:FunCheckForZero('" + txtInstallationNo.ClientID + "', 'Installation Number ');");
            txtSerialNo.Attributes.Add("onblur", "javascript:FunCheckForZero('" + txtSerialNo.ClientID + "', 'Serial Number ');");
            //txtInstallationDate.Attributes.Add("readonly", "readonly");
            CEInsDate.Format = strDateFormat;
            if (PageMode != PageModes.Query)
            {
                txtInstallationDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInstallationDate.ClientID + "','" + strDateFormat + "',true,  false);");
            }
            else
            {
                txtInstallationDate.Attributes.Remove("onblur");
            }
            flUpload.Attributes.Add("onchange", "fnAssignPath('" + flUpload.ClientID + "','" + hdnSelectedPath.ClientID + "'); fnLoadPath('" + btnBrowse.ClientID + "');");
            btnDlg.OnClientClick = "fnLoadPath('" + flUpload.ClientID + "');";

            if (!string.IsNullOrEmpty(lblActualPath.Text.Trim()))
            {
                BtnView.ToolTip = System.IO.Path.GetFileName(lblActualPath.Text);
                BtnView.Visible = true;
            }

            if (PageMode == PageModes.Query)
            {
                CEInsDate.Enabled = false;
            }
        }
    }

    protected void btnBrowse_OnClick(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("grvAsset", ((Button)sender).ClientID);

        HttpFileCollection hfc = Request.Files;
        for (int i = 0; i < hfc.Count; i++)
        {
            HttpPostedFile hpf = hfc[i];
            if (hpf.ContentLength > 0)
            {
                CheckBox chkSelect = (CheckBox)grvAsset.Rows[intRowIndex].FindControl("chkSelect");
                HiddenField hdnSelectedPath = (HiddenField)grvAsset.Rows[intRowIndex].FindControl("hdnSelectedPath");
                FileUpload flUpload = (FileUpload)grvAsset.Rows[intRowIndex].FindControl("flUpload");
                Label lblCurrentPath = (Label)grvAsset.Rows[intRowIndex].FindControl("lblCurrentPath");

                chkSelect.Enabled = true;
                chkSelect.Checked = true;
                chkSelect.ToolTip = flUpload.ToolTip = hdnSelectedPath.Value;
                lblCurrentPath.Text = hpf.FileName;

                string strViewst = "File" + intRowIndex.ToString();
                Cache[strViewst] = hpf;
            }
        }      
    }

    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }

    protected void hyplnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvAsset", strFieldAtt);

            Label lblActualPath = (Label)grvAsset.Rows[gRowIndex].FindControl("lblActualPath");

            if (!string.IsNullOrEmpty(lblActualPath.Text.Trim()))
            {
                string strFileName = lblActualPath.Text.Replace("\\", "/").Trim();
                string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "File not to be scanned yet");
            }          
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            //lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunProUploadFiles()
    {
        try
        {
            for (int i = 0; i <= grvAsset.Rows.Count - 1; i++)
            {
                string strViewst = "File" + i.ToString();
                CheckBox chkSelect = (CheckBox)grvAsset.Rows[i].FindControl("chkSelect");
                Label lblCurrentPath = (Label)grvAsset.Rows[i].FindControl("lblCurrentPath");

                if (chkSelect.Checked)
                {
                    HttpPostedFile hpf = (HttpPostedFile)Cache[strViewst];
                    string strServerPath = Server.MapPath(".").Replace("\\LoanAdmin", "");
                    string strFilePath = "\\Data\\Invoice\\" + intCompanyID.ToString() + "_" + ddlMLA.SelectedValue.Replace("/", "");

                    if (!System.IO.Directory.Exists(strServerPath + strFilePath))
                    {
                        System.IO.Directory.CreateDirectory(strServerPath + strFilePath);
                    }

                    if (ddlSLA.SelectedValue.ToString() != "0")
                    {
                        strFilePath = strFilePath + @"\" + ddlSLA.SelectedItem.ToString().Replace("/", "") + "_" + (i + 1).ToString() + "_" + System.IO.Path.GetFileName(hpf.FileName).Replace("%20", "_").Replace(" ", "_");
                    }
                    else
                    {
                        strFilePath = strFilePath + @"\" + (i + 1).ToString() + "_" + System.IO.Path.GetFileName(hpf.FileName).Replace("%20", "_").Replace(" ", "_");
                    }

                    lblCurrentPath.Text = strFilePath;
                    hpf.SaveAs(strServerPath + strFilePath);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void FunProClearCaches()
    {
        for (int i = 0; i <= grvAsset.Rows.Count - 1; i++) 
        {
            string strViewst = "File" + i.ToString();
            if (Cache[strViewst] != null)
            {
                Cache.Remove(strViewst);
            }
        }
    }

}
