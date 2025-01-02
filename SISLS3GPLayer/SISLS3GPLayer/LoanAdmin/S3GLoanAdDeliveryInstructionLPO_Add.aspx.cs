#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Delivery Instruction and LPO
/// Created By			: Kannan RC
/// Created Date		: 17-Aug-2010
/// Modified By         : R.Manikandan
/// Modified Date       : 11 - FEB - 2011
/// Reason              : First Round Bug Fixing and Design Change
/// Modified By         : Shibu
/// Modified Date       : 30-Sep-2013
/// Reason              : Performance Tuning Avoid to Load DDL Controls Modify an Query Mode
/// <Program Summary>
#endregion

#region Name Spaces

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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections;

#endregion

#region Delivery Instruction / LPO
public partial class LoanAdmin_S3GLoanAdDeliveryInstructionLPO_Add : ApplyThemeForProject
{
    #region Intialization
    Dictionary<string, string> dictParam = null;
    string strDateFormat;
    int intErrCode;
    int intDeliverID;
    int intUserId;
    int intCompanyId;
    string _DINo;
    string strMode;
    int i;
    string s;
    bool status;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    string strKey = "Insert";
    static string strPageName = "DeliveryInstruction / LPO";
    //LoanAdmin/S3gLoanAdTransLander.aspx?Code=DEI
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=DEI';";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdDeliveryInstructionLPO_Add.aspx';";




    SerializationMode SerMode = SerializationMode.Binary;
    LoanAdminMgtServicesClient objLoanAdmin_MasterClient;
    //LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable ObjS3G_DeliveryInsDataTable = null;
    //LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow;
    LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable ObjS3G_GetAssetDataTable = new LoanAdminMgtServices.S3G_LOANAD_GetAssetDetailsDataTable();
    LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable ObjS3G_GetCustDataTable = new LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable();
    public static LoanAdmin_S3GLoanAdDeliveryInstructionLPO_Add obj_Page;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        // WF Initializtion 
        ProgramCode = "055";
        obj_Page = this;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            intDeliverID = Convert.ToInt32(fromTicket.Name);
            strMode = Request.QueryString["qsMode"];
        }
        UserInfo ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        lblCompanyName.Text = ObjUserInfo.ProCompanyNameRW.ToUpper();
        //DATE Format
        txtDate.Attributes.Add("readonly", "readonly");
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        txtDate.Text = DateTime.Today.ToString(strDateFormat);
        txtDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
        //cexDate.Format = strDateFormat;
        //cexDate.SelectedDate = DateTime.Today;
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

        if (!IsPostBack)
        {
           
            //FunPriGetLookUpList();


            if (Request.QueryString["qsMode"] == "Q")
            {
                //FunPriGetDeliveryDetails();
                //FunPriGetDeliveryGridDetails();
                FunPriDisableControls(-1);
            }
            else if (Request.QueryString["qsMode"] == "M")
            {
                FunPriDisableControls(1);
            }
            else
            {
                //FunPriGetLOBBranchDropDownList();
                //FunPriGetVendorDropDownList();
                //FunPriGetLookUpList();
                FunPriDisableControls(0);
                FunPriGetLOBBranchDropDownList();
                //FunPriGetVendorDropDownList();
                FunPriGetCompanyInfo();

            }
            //if (PageMode == PageModes.Create)
            //{
            //    ddlMLA.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
            //    ddlVendorCode.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
            //}
        }

        if (PageMode == PageModes.WorkFlow && !IsPostBack)
        {
            try
            {
                PreparePageForWFLoad();
            }
            catch (Exception ex)
            {

                Utility.FunShowAlertMsg(this, "Invalid data to load, access side menu");
            }
        }
    }

    private void PreparePageForWFLoad()
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            // Get The IDVALUE from Document Sequence #
            DataRow HeaderValues = GetHeaderDetailsFromDOCNo(WFSessionValues.PANUM, ProgramCode);


            FunPriGetLOBBranchDropDownList(); // To Fill LOB
            FunPriGetLookupLocationList();     // To Fill Location
            FunPriGetCompanyInfo();   // To fill Company Info.
            
            ddlLOB.SelectedValue = HeaderValues["Lob_Id"].ToString();
            ddlBranch.SelectedValue = HeaderValues["Location_Id"].ToString();

            FunPriGetMLADetails(int.Parse(ddlLOB.SelectedValue), int.Parse(ddlBranch.SelectedValue), intCompanyId);
            ddlMLA.SelectedValue = HeaderValues["UniqueId"].ToString();
            if (ddlMLA.SelectedIndex > 0)
            {
                FunPriLoadMLADetails();
            }
            if (ddlLOB.Items.Count > 0) ddlLOB.ClearDropDownList();
           // if (ddlBranch.Items.Count > 0) ddlBranch.ClearDropDownList();
            if (ddlMLA.Items.Count > 0) ddlMLA.ClearDropDownList();
        }
    }
    #endregion

    #region Bind DropDownlist
    private void FunPriGetLOBBranchDropDownList()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intDeliverID == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_Id", intUserId.ToString());
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            //Procparam.Add("@Program_ID", "55");
            ddlLOB.BindDataTable(SPNames.S3G_LOANAD_GetDILOBList, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Branch_ID", "Branch_Code", "Branch_Name" });
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //if (Request.QueryString["qsMode"] != "C")
            //{
            //    ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            //}
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
                 
    }

    //Method added to get Location based on LOB - Kuppusamy B
    private void FunPriGetLookupLocationList()
    {
        try
        {
            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //if (intDeliverID == 0)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            //Procparam.Add("@User_Id", intUserId.ToString());
            //Procparam.Add("@Company_ID", intCompanyId.ToString());
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //ddlBranch.AddItemToolTip();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            return;
        }
    }
    private void FunPriGetMLADetails(int intLOBID, int intBranchID, int intCompanyID)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            dictParam.Add("@Type", "1");
            dictParam.Add("@Is_Activated", "0");
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
            {
                dictParam.Add("@IS_OL", "1");
            }
            else
            {
                dictParam.Add("@IS_OL", "0");
            }
            ddlMLA.BindDataTable(SPNames.S3G_LOANAD_GetMLANO, dictParam, new string[] { "PANum", "PANum" });
            //if (ddlMLA.Items.Count == 2)
            //{
            //    ddlMLA.SelectedIndex = 1;
            //    FunPriBindMLA();
            //}
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }



    private void FunPriGetSLADetails(string intMLA, int intCompanyID)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            dictParam.Add("@Type", "2");
            dictParam.Add("@IS_Activated", "0");
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
            {
                dictParam.Add("@IS_OL", "1");
            }
            else
            {
                dictParam.Add("@IS_OL", "0");
            }
            dictParam.Add("@PANum", ddlMLA.SelectedItem.Text);
            ddlSLA.BindDataTable("S3G_LOANAD_GetMLANO", dictParam, new string[] { "SANum", "SANum" });
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }
    protected void getSAN()
    {
        for (i = 0; i < ddlSLA.Items.Count; i++)
        {
            s = ddlSLA.Items[i].ToString();
            status = s.Contains("DUMMY");
            if (status == true)
            {
                if (ddlSLA.Items.Count == 2)
                {
                    ViewState["dum"] = ddlSLA.Items[i].Text;
                    ddlSLA.Items.Clear();
                    break;
                }
                else
                {
                    ViewState["dum"] = ddlSLA.Items[i].Text;
                    ddlSLA.Items.RemoveAt(i);

                }
            }
        }
    }

    private void FunPriBindMLA()
    {
        try
        {
            //ddlVendorCode.SelectedIndex = -1;
            if (bCreate)
            {
                btnSave.Enabled = true;
                //ddlVendorCode.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
            }
           
            S3GVendorAddress.ClearCustomerDetails();
            txtVendorName.Text = "";
            gvDlivery.DataSource = null;
            gvDlivery.DataBind();
            
            
            if (ddlMLA.SelectedItem.Text != "--Select--")
            {
                FunPriGetCustomerDetails(Convert.ToString(ddlMLA.SelectedItem.Text));
                FunPriGetSLADetails((Convert.ToString(ddlMLA.SelectedItem.Text)), intCompanyId);
                getSAN();
                if (ddlSLA.Items.Count <= 1)
                {
                    // FunPriGetExistingDetails(ddlMLA.SelectedItem.Text, Convert.ToString(ViewState["dum"]));
                    FunPriGetVendor(Convert.ToString(ddlMLA.SelectedItem.Text), Convert.ToString(ViewState["dum"]));
                    rfvSLA.Visible = false;
                    rfvSLA.Enabled = false;

                }
                else
                {
                    //FunPriGetVendor(Convert.ToString(ddlMLA.SelectedItem.Text), Convert.ToString(ddlSLA.SelectedItem.Text));
                    rfvSLA.Visible = true;
                    rfvSLA.Enabled = true;
                }
                if (ddlMLA.SelectedItem.Text == "--Select--")
                {
                    //Clear();
                    S3GCustomerAddress1.ClearCustomerDetails();
                    S3GVendorAddress.ClearCustomerDetails();
                    txtVendorName.Text = "";
                    ddlVendorCode.Items.Clear();

                    ddlSLA.SelectedIndex = -1;
//                    Panel3.Visible = false;
                }
            }
            else
            {
                //Clear();
                S3GCustomerAddress1.ClearCustomerDetails();
                S3GVendorAddress.ClearCustomerDetails();
                txtVendorName.Text = "";
                ddlVendorCode.Items.Clear();
                ddlSLA.Items.Clear();
//                Panel3.Visible = false;

            }
            //if (ddlSLA.Items.Count == 2) ddlSLA.SelectedValue = ddlSLA.Items[1].Value;

        }

        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriGetVendor(string intMLA, string intSLA)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            if ((ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")) && ((ddlMLA.SelectedValue == "0") || (ddlMLA.SelectedValue == "") ))
            {
                if(PageMode == PageModes.Create)
                ddlAssetCode.Visible = true;
                //rfvAssetCode.Enabled = true;
                //lblAssetCodeOl.Visible = true;
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                ddlVendorCode.BindDataTable("S3G_LOANAD_GetEntityDO", dictParam, new string[] { "Entity_ID", "Vendor" });

            }
            else
            {
                ddlAssetCode.Visible = false;
                //rfvAssetCode.Enabled = false;
                lblAssetCodeOl.Visible = false;
                if (ddlMLA.SelectedItem.Text != "--Select--")
                {
                    dictParam.Add("@PANum", Convert.ToString(ddlMLA.SelectedItem.Text));
                    dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                    dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                    //dictParam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedItem.Text));
                    dictParam.Add("@SANum", "");
                    dictParam.Add("@PASA", "1");

                    ddlVendorCode.BindDataTable("S3G_LOANAD_GetVendorCode", dictParam, new string[] { "Entity_ID", "Vendor" });
                }
                else if (ddlSLA.SelectedItem.Text != "--Select--")
                {
                    dictParam.Add("@PANum", Convert.ToString(ddlMLA.SelectedItem.Text));
                    dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                    dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                    //dictParam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedItem.Text));
                    dictParam.Add("@SANum", Convert.ToString(ddlSLA.SelectedItem.Text));
                    dictParam.Add("@PASA", "0");
                    ddlVendorCode.BindDataTable("S3G_LOANAD_GetVendorCode", dictParam, new string[] { "Entity_ID", "Vendor" });
                }
            }
        }

        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }


        //dictParam = new Dictionary<string, string>();
        //dictParam.Add("@PANum", Convert.ToString(ddlMLA.SelectedValue));
        //dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
        //dictParam.Add("@SANum", Convert.ToString(ddlMLA.SelectedValue));4
        //dictParam.Add("@PASA", 1);
        //ddlVendorCode.BindDataTable("S3G_LOANAD_GetVendorCode", dictParam, new string[] { "Entity_ID", "Vendor" });
    }


    #endregion

    #region DDL Events

    #region LOB
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
      ContractMgtServicesReference.ContractMgtServicesClient objContractMgtserviceClient = new ContractMgtServicesReference.ContractMgtServicesClient();

        try
        {
            if (ddlLOB.SelectedIndex > 0)
            {
                ddlBranch.Clear();
                //FunPriGetLookupLocationList();
            }
            else
            {
                ddlBranch.Clear();
                //ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            }
            btnAdd.Visible = false;            
            lblAssetCodeOl.Visible = false;
            ddlAssetCode.Visible = false;

            byte[] byteMLASLAApplicable = objContractMgtserviceClient.FunPubGetMLASLAApplicable(Convert.ToInt32(ddlLOB.SelectedValue), intCompanyId);
            DataSet dsMLASLAApplicable = (DataSet)ClsPubSerialize.DeSerialize(byteMLASLAApplicable, SerializationMode.Binary, typeof(DataSet));
            //if (ddlBranch.Items.Count > 0)
            //{ ddlBranch.SelectedIndex = 0; }
            if (ddlMLA.Items.Count > 0)
            { ddlMLA.SelectedIndex = 0; }
            if (ddlVendorCode.Items.Count > 0)
            { ddlVendorCode.SelectedIndex = 0; }
            //if (ddlDILOP.Items.Count > 0)
            //{// ddlDILOP.SelectedIndex = 0; }
            Clear();
//            Panel3.Visible = false;
            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            {

                if (dsMLASLAApplicable.Tables.Count > 0)
                {
                    if (dsMLASLAApplicable.Tables[0].Rows.Count > 0)
                    {
                        string strMLASLAApplicable = dsMLASLAApplicable.Tables[0].Rows[0][0].ToString();
                        if (strMLASLAApplicable == "False")
                        {
                            ddlSLA.Enabled = false;
                            // rfvSLA.Visible = false;                        
                        }
                        else
                        {
                            ddlSLA.Enabled = true;
                            //rfvSLA.Visible = true;

                        }
                    }
                }

            }
            else
            {
                //ddlBranch.SelectedIndex = 0;

                //if (Convert.ToInt32(ddlBranch.SelectedValue) == 0)
                //{
                //    ddlMLA.SelectedIndex = 0;
                //    //ddlMLA.ClearDropDownList();
                //}
                //if (Convert.ToInt32(ddlMLA.SelectedValue) == 0)
                //{
                //    ddlVendorCode.SelectedIndex = 0;
                //    //ddlVendorCode.ClearDropDownList();
                //    //ddlDILOP.SelectedIndex = 0;
                //}
                //if (Convert.ToInt32(ddlVendorCode.SelectedValue) == 0)
                //{
                //    Panel3.Visible = false;
                //}
                if (dsMLASLAApplicable.Tables[0].Rows.Count > 0)
                {
                    string strMLASLAApplicable = dsMLASLAApplicable.Tables[0].Rows[0][0].ToString();
                    if (strMLASLAApplicable == "True")
                    {
                        ddlSLA.SelectedIndex = 0;
                    }
                }
                //ddlDILOP.SelectedIndex = 0;
                //ddlVendorCode.SelectedIndex = 0;
                //Clear();
            }
        }


        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objContractMgtserviceClient.Close();
        }
              
    }

    #endregion

    #region Branch
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlAssetCode.Visible = false;
            //rfvAssetCode.Enabled = false;
            lblAssetCodeOl.Visible = false;
            btnAdd.Visible = false;
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                Clear();
               // S3GCustomerAddress1.Caption = "Company";
                FunPriGetMLADetails((Convert.ToInt32(ddlLOB.SelectedValue)), (Convert.ToInt32(ddlBranch.SelectedValue)), intCompanyId);
            }
            else
            {
                ddlMLA.SelectedIndex = 0;
                ddlVendorCode.SelectedIndex = 0;
                //ddlDILOP.SelectedIndex = 0;
                Clear();
//                Panel3.Visible = false;


            }
          
           
               
           
            //if ((ddlMLA.SelectedValue == "0") && (ddlSLA.SelectedValue == "0") && (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")))
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT"))
            {

                ddlVendorCode.Items.Clear();
                FunPriGetVendor("0", "0");
                FunGetCompanyDetails(intCompanyId);
                S3GCustomerAddress1.Caption = "Company";
                Panel2.GroupingText = "Company Details";
            }
            else
            {
                S3GCustomerAddress1.Caption = "Customer";
                Panel2.GroupingText = "Customer Details";
            }
        }
        catch (Exception ex)
        {
            cvDelivery.ErrorMessage = ex.Message;
            cvDelivery.IsValid = false;
            return;
        }
    }
    #endregion

    #region PANum/SANum
    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadMLADetails(); // Function FunPriLoadMLADetails is written for common usage purpose for WORKFLOW. On 30 Jan 2012. By Narasimha Rao.
    }
    private void FunPriLoadMLADetails()
    {
        ddlVendorCode.Items.Clear();
        //ddlVendorCode.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
        FunPriBindMLA();
        if (ddlLOB.SelectedIndex > 0)
        {
            S3GCustomerAddress1.Caption = "Customer";
            Panel2.GroupingText = "Customer Details";
        }

        if ((ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")) && (ddlMLA.SelectedValue == "0"))
        {
            FunPriGetVendor("0", "0");
            FunGetCompanyDetails(intCompanyId);
        }
        else
        {
            ddlAssetCode.Visible = false;
            //rfvAssetCode.Enabled = false;
            lblAssetCodeOl.Visible = false;
            btnAdd.Visible = false;
        }
    }

    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT") && (ddlMLA.SelectedValue == "0")) 
            {
                FunPriGetVendor("0", "0");
                FunGetCompanyDetails(intCompanyId);

            }
            else
            {
                ddlAssetCode.Visible = false;
               // rfvAssetCode.Enabled = false;
                lblAssetCodeOl.Visible = false;
            }

            if (bCreate)
            {
                btnSave.Enabled = true;
            }
            ddlVendorCode.SelectedIndex = -1;
            S3GVendorAddress.ClearCustomerDetails();
            txtVendorName.Text = "";
            if (ddlSLA.Items.Count  >= 2)
            {
                gvDlivery.DataSource = null;
                gvDlivery.DataBind();
            }
           

//            Panel3.Visible = false;
            
            //guna
            //FunPriGetVendor(Convert.ToString(ddlMLA.SelectedItem.Text), Convert.ToString(ddlSLA.SelectedItem.Text));
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@PANum", Convert.ToString(ddlMLA.SelectedItem.Text));
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            //dictParam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedItem.Text));
            dictParam.Add("@SANum", Convert.ToString(ddlSLA.SelectedItem.Text));
            dictParam.Add("@PASA", "0");
            ddlVendorCode.BindDataTable("S3G_LOANAD_GetVendorCode", dictParam, new string[] { "Entity_ID", "Vendor" });
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }
    #endregion
    #endregion

    #region GetValues for DI

    #region Company Info


    private void FunPriGetCompanyInfo()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtVendorDetails = Utility.GetDefaultData("S3G_LOANAD_GetCompanyInfo", dictParam);
            DataRow dtRow = dtVendorDetails.Rows[0];
            lblCCity.Text = dtRow["City"].ToString();
            lblCZipcode.Text = dtRow["Zip_code"].ToString();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    #endregion

    #region GetAssetValue
    private void FunPriGetAssetValue(int intCompanyID, int intVendorID, string strPANum, string strSANum)
    {

        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();      
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictParam.Add("@Entity_ID", intVendorID.ToString());
            dictParam.Add("@PANum", Convert.ToString(ddlMLA.SelectedValue));
            if(ddlSLA.Items.Count >= 2)
            dictParam.Add("@SANum", Convert.ToString(ddlSLA.SelectedValue));
            else 
            {
                string StrSAN;
                StrSAN = (ddlMLA.SelectedItem.Text + "DUMMY");
                dictParam.Add("@SANum", StrSAN);
            }
            DataTable dtAssetMaster = Utility.GetDefaultData("S3G_LOANAD_GetAssetDetails", dictParam);
            ViewState["dtAssetMaster"] = dtAssetMaster;

            if ((ddlMLA.SelectedValue != "0") && (ddlSLA.SelectedValue != "0"))
            {
                if (dtAssetMaster.Rows.Count == 0)
                {
                    gvDlivery.DataSource = null;
                    gvDlivery.DataBind();
                    btnSave.Enabled = false;
                    //Panel3.Visible = false;
                    Utility.FunShowAlertMsg(this, "Delivery Instruction/LPO already updated");
                    return;
                }
                else
                {
                    btnSave.Enabled = true;
                    gvDlivery.DataSource = dtAssetMaster;
                    gvDlivery.DataBind();

                }


            }
            else
            {
                gvDlivery.DataSource = dtAssetMaster;
                gvDlivery.DataBind();
            }
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
    #endregion

    #region GetVendor
    protected void ddlVendorCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
            if ((ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")) && (ddlMLA.SelectedValue == "0"))
            {
                if (ViewState["AssetDetails"] != null)
                {
                    ViewState["AssetDetails"] = null;
                }
                ddlAssetCode.Items.Clear();
                if ((PageMode == PageModes.Create) && (ddlVendorCode.SelectedIndex > 0))
                {
                    ddlAssetCode.Visible = true;
                    lblAssetCodeOl.Visible = true;
                    btnAdd.Visible = true;
                    dictParam = new Dictionary<string, string>();
                    dictParam.Add("@Company_ID", intCompanyId.ToString());
                    DataTable dtAssetCode = Utility.GetDefaultData("S3G_LOANAD_GetCompany_DO", dictParam);
                    // DataRow dtRow = dtCompany.Rows[0];
                    ddlAssetCode.BindDataTable("S3G_LOANAD_AssetCodeOL_DO", dictParam, new string[] { "Asset_ID", "Asset_Code" });
                }
                else
                {
                    ddlAssetCode.Visible = false;
                    lblAssetCodeOl.Visible = false;
                    btnAdd.Visible = false;
                }

            }
            else
            {
                ddlAssetCode.Visible = false;
               // rfvAssetCode.Enabled = false;
                lblAssetCodeOl.Visible = false;
                btnAdd.Visible = false;
            }
            
            if (bCreate)
            {
                btnSave.Enabled = true;
            }
            
            if (Convert.ToInt32(ddlVendorCode.SelectedValue) > 0)
            {
                FunPriGetAssetValue(Convert.ToInt32(intCompanyId), Convert.ToInt32(ddlVendorCode.SelectedValue), ddlMLA.SelectedValue, ddlSLA.SelectedValue);
                FunPriGetVendotDetails(Convert.ToInt32(ddlVendorCode.SelectedValue));
                
            }
            else if (Convert.ToInt32(ddlVendorCode.SelectedValue) == 0)
            {
                gvDlivery.DataSource = null;
                gvDlivery.DataBind();
                ddlVendorCode.SelectedIndex = 0;
                txtVendorName.Text = "";
                S3GVendorAddress.ClearCustomerDetails();
                //Panel3.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }
    private void FunPriGetVendotDetails(int intVendorID)
    {
        try
        {
            string strVendorAddress = "";
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Entity_ID", intVendorID.ToString());
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtVendorDet = Utility.GetDefaultData("S3G_LOANAD_GetVendorDetails", dictParam);
            txtVendorName.Text = dtVendorDet.Rows[0]["Entity_Name"].ToString();
            strVendorAddress = Utility.SetVendorAddress(dtVendorDet.Rows[0]);
            S3GVendorAddress.SetCustomerDetails("", strVendorAddress, "",
            dtVendorDet.Rows[0]["TelePhone"].ToString(),
            dtVendorDet.Rows[0]["Mobile"].ToString(),
            dtVendorDet.Rows[0]["EMail"].ToString(),
            dtVendorDet.Rows[0]["Website"].ToString());
           // DataTable dtVendorDet1 = new DataTable();
            if (PageMode != PageModes.Create)
            {
                lblVAddress1.Text = dtVendorDet.Rows[0]["Address"].ToString();
                lblVAddress2.Text = dtVendorDet.Rows[0]["Address2"].ToString();
                lblVCity.Text = dtVendorDet.Rows[0]["City"].ToString();
                lblVPincode.Text = dtVendorDet.Rows[0]["PINCode"].ToString();
                lblVState.Text = dtVendorDet.Rows[0]["State"].ToString();
                lblVCountry.Text = dtVendorDet.Rows[0]["Country"].ToString();
                
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    #endregion

    #region GetCustomerDetails
    private void FunPriGetCustomerDetails(string strMLAID)
    {
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsRow ObjCustMasterRow;
            ObjCustMasterRow = ObjS3G_GetCustDataTable.NewS3G_LOANAD_GetDICustomerDetailsRow();
            ObjCustMasterRow.Company_ID = intCompanyId;
            ObjCustMasterRow.PANum = ddlMLA.SelectedItem.Text;
            //ObjCustMasterRow.SANum = ddlSLA.SelectedValue;
            ObjS3G_GetCustDataTable.AddS3G_LOANAD_GetDICustomerDetailsRow(ObjCustMasterRow);
            
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteCustMasterDetails = objLoanAdmin_MasterClient.FunPubGetCustDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_GetCustDataTable, SerMode));
            LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable dtCustMaster = (LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable)ClsPubSerialize.DeSerialize(byteCustMasterDetails, SerializationMode.Binary, typeof(LoanAdminMgtServices.S3G_LOANAD_GetDICustomerDetailsDataTable));
            DataRow dtRow = dtCustMaster.Rows[0];
            lblCustID.Text = dtRow["Customer_ID"].ToString();

            
            //ViewState["dtCustMaster"] = dtCustMasterDetails;
            if (PageMode != PageModes.Create)
            {
                lblCuAddress1.Text = dtRow["Comm_Address1"].ToString();
                lblCuAddress2.Text = dtRow["Comm_Address2"].ToString();
                lblCuCity.Text = dtRow["Comm_City"].ToString();
                lblCuPincode.Text = dtRow["Comm_PINCode"].ToString();
                lblCuState.Text = dtRow["Comm_State"].ToString();
                lblCuCountry.Text = dtRow["Comm_Country"].ToString();
            }
            S3GCustomerAddress1.SetCustomerDetails(dtCustMaster.Rows[0], true);
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
            if (objLoanAdmin_MasterClient != null)
            {
                objLoanAdmin_MasterClient.Close();
            }
        }

    }
    #endregion

    #region GetXML
    protected string FunProFormXML()
    {
        StringBuilder strbuXML = new StringBuilder();
        strbuXML.Append("<Root>");
        foreach (GridViewRow grvData in gvDlivery.Rows)
        {

            if (((CheckBox)grvData.FindControl("CbAssets")).Checked)
            {
                //string strlblPRTID = ((Label)grvData.FindControl("lblDeliveryIns")).Text;
                string strAssetID = ((Label)grvData.FindControl("lblAssetID")).Text;
                string strModelDesc = ((TextBox)grvData.FindControl("txtModelDesc")).Text;
                string strQuly = ((TextBox)grvData.FindControl("txtQuantity")).Text;
                string strAssetValue = ((TextBox)grvData.FindControl("txtAssetValue")).Text;
                if(strAssetValue == "")
                strAssetValue = ((TextBox)grvData.FindControl("txtAssetValue1")).Text;
                string strRemarks = ((TextBox)grvData.FindControl("txtRemarks")).Text;
                strbuXML.Append(" <Details  Asset_ID='" + strAssetID.ToString() + "' Model_description='" + strModelDesc.ToString() +
                 "' Asset_quantity='" + strQuly.ToString() + "' Asset_Value='" + strAssetValue.ToString() + "' Remarks ='" + strRemarks.ToString() + "'/>");
            }
        }
        strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }
    #endregion

    #endregion

    #region Bind Grid
    protected void gvDlivery_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal total = 0;

        try
        {
           if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox Quly = (TextBox)e.Row.FindControl("txtQuantity");
                TextBox calc = (TextBox)e.Row.FindControl("txtDyAsset");
                TextBox Dcal = (TextBox)e.Row.FindControl("txtAssetValue");
                if ((!ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")) && (ddlMLA.SelectedValue != "0") && (PageMode == PageModes.Create))
                {
                    Quly.Attributes.Add("onblur", "fnBlur()");
                }
                else
                {
                    Quly.Attributes.Remove("onblur");
                }
                if ((!ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")) && (ddlMLA.SelectedValue != "0") && (PageMode == PageModes.Create))
                {
                    Dcal.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    Dcal.Attributes.Remove("readonly");
                    Quly.Attributes.Remove("onblur");
                }

                if ((Request.QueryString["qsMode"] == "C") && (!ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")) && (ddlMLA.SelectedValue != "0") && (PageMode == PageModes.Create))
                {
                    if (Quly.Text != "")
                        total = Convert.ToDecimal(Quly.Text) * Convert.ToDecimal(calc.Text);
                    else
                        total = 0;
                    Dcal.Text = total.ToString();
                }
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt1 = (TextBox)e.Row.FindControl("txtModelDesc");
                TextBox txt2 = (TextBox)e.Row.FindControl("txtAssetValue");
                TextBox txt3 = (TextBox)e.Row.FindControl("txtRemarks");
                TextBox txt4 = (TextBox)e.Row.FindControl("txtQuantity");
                CheckBox chk = (CheckBox)e.Row.FindControl("CbAssets");
                //CheckBox chkall = (CheckBox)e.Row.FindControl("ChAll");
                 //CheckBox ChkAll = (CheckBox)e.Row.FindControl("chAll");
                
                
                // Manikandan  XXX
                if ((ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")) && (ddlMLA.SelectedValue == "0"))
                {
                    TextBox txtAssetValue = (TextBox)e.Row.FindControl("txtAssetValue");
                    txtAssetValue.SetDecimalPrefixSuffix(10, 3, true, false, "Asset Value");
                    txt4.SetDecimalPrefixSuffix(4, 0, true, false, "Unit Quantity");
                }

                if ((Request.QueryString["qsMode"] == "Q") || (Request.QueryString["qsMode"] == "M"))
                {
                    chk.Checked = true;
                    chk.Enabled = false;
                    //ChkAll.Checked = true;
                    txt1.ReadOnly = true;
                    txt2.ReadOnly = true;
                    txt3.ReadOnly = true;
                    txt4.ReadOnly = true;

                }
                if (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT") && (PageMode == PageModes.Create))
                {
                    txt2.ReadOnly = false;
                    txt4.ReadOnly = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CbAssets = (CheckBox)e.Row.FindControl("CbAssets");
                CbAssets.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + gvDlivery.ClientID + "','chAll','CbAssets');");
                //TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chAll = (CheckBox)e.Row.FindControl("chAll");
                chAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvDlivery.ClientID + "',this,'CbAssets');");

                if ((Request.QueryString["qsMode"] == "Q") || (Request.QueryString["qsMode"] =="M"))
                {
                    chAll.Enabled = false;
                    chAll.Checked = true;
                }
            }
        }

        catch (Exception exp)
        {
            cvDelivery.ErrorMessage = exp.Message;
            cvDelivery.IsValid = false;
        }

    }
    #endregion  

    #region To Get Delivery Instruction in Query Mode
    private void FunPriGetDeliveryDetails()
    {
        //Changed By Shibu Unwanted DDL Load Events An Address Load Events are Removed
        try
        {
            DataSet dset = new DataSet();
            DataSet ds = new DataSet();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@DeliveryInstruction_ID", intDeliverID.ToString());
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            ds = Utility.GetDataset("S3G_LOANAD_GetDeliveryInstruction", dictParam);
            DataTable dtDeliveryDetails = ds.Tables[0].Copy();
            dset.Tables.Add(dtDeliveryDetails);

            ViewState["DSet"] = dset; 
            DataRow dtRow = dtDeliveryDetails.Rows[0];

            ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dtDeliveryDetails.Rows[0]["LOB"].ToString(), dtDeliveryDetails.Rows[0]["LOB_ID"].ToString()));
            ddlLOB.ToolTip = dtDeliveryDetails.Rows[0]["LOB"].ToString();
            
            //ddlBranch.SelectedValue = dtDeliveryDetails.Rows[0]["Branch_ID"].ToString();
            //FunPriGetLookupLocationList();
            
            ddlBranch.SelectedValue = dtDeliveryDetails.Rows[0]["Location_ID"].ToString();
            ddlBranch.SelectedText = dtDeliveryDetails.Rows[0]["Location_Name"].ToString();
            ddlBranch.ToolTip = dtDeliveryDetails.Rows[0]["Location_Name"].ToString();

            txtLPONo.Text = dtDeliveryDetails.Rows[0]["DeliveryInstruction_No"].ToString();
            _DINo = txtLPONo.Text = dtDeliveryDetails.Rows[0]["DeliveryInstruction_No"].ToString();
            lblIsActivated.Text = dtDeliveryDetails.Rows[0]["Active"].ToString();

            /*Changed for oracle conversion - kuppusamy b - 22-Feb-2012*/
            //lblStatus.Text = dtDeliveryDetails.Rows[0]["DeliveryInstruction_Status_Code"].ToString();
            lblStatus.Text = dtDeliveryDetails.Rows[0]["DI_Status_Code"].ToString();

            lblIsPrint.Text = dtDeliveryDetails.Rows[0]["Is_DIGeneration"].ToString();
            //txtDate.Text = ConvertToCurrentFormat(Convert.ToString(dtDeliveryDetails.Rows[0]["DeliveryInstruction_Date"].ToString()));
            txtDate.Text = DateTime.Parse(dtDeliveryDetails.Rows[0]["DeliveryInstruction_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            
           // FunPriGetMLADetails((Convert.ToInt32(ddlLOB.SelectedValue)), (Convert.ToInt32(ddlBranch.SelectedValue)), intCompanyId);
            //ddlMLA.SelectedItem.Text = dtDeliveryDetails.Rows[0]["PANum"].ToString();            
            if (dtDeliveryDetails.Rows[0]["PANum"] != "0")
                ddlMLA.Items.Add(new System.Web.UI.WebControls.ListItem(dtDeliveryDetails.Rows[0]["PANum"].ToString(), dtDeliveryDetails.Rows[0]["PANum"].ToString()));
            else
                ddlMLA.Items.Add(new System.Web.UI.WebControls.ListItem("--Select--", "0")); //ddlMLA.SelectedValue = null;
         
            if (dtDeliveryDetails.Rows[0]["PANum"] !=  "0")
            //FunPriGetSLADetails((Convert.ToString(ddlMLA.SelectedItem.Text)),intCompanyId);
           
            txtSan.Text = dtDeliveryDetails.Rows[0]["SANum"].ToString();
            if (txtSan.Text == ddlMLA.SelectedItem.Text + "DUMMY")
            {
                ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem("--Select--", "0"));              
            }
            else
            {
                //if (string.IsNullOrEmpty (dtDeliveryDetails.Rows[0]["SANum"].ToString()))
                //{
                //    ddlSLA.SelectedItem.Text = string.Empty;                    
                //}
                //else
                //{
                ddlSLA.Items.Add(new System.Web.UI.WebControls.ListItem(dtDeliveryDetails.Rows[0]["SANum"].ToString(), dtDeliveryDetails.Rows[0]["SANum"].ToString())); 
                //ddlSLA.SelectedItem.Text = dtDeliveryDetails.Rows[0]["SANum"].ToString();
                //}
            }
            //if (ddlSLA.Items.Count != 0)
            //{
            //    ddlSLA.SelectedValue = dtDeliveryDetails.Rows[0]["SANum"].ToString();
            //}
            FunPriGetVendor(Convert.ToString(ddlMLA.SelectedItem.Text), Convert.ToString(ddlSLA.SelectedItem.Text));
            ddlVendorCode.SelectedValue = dtDeliveryDetails.Rows[0]["Entity_ID"].ToString();
          
           // FunPriGetVendotDetails(Convert.ToInt32(ddlVendorCode.SelectedValue));
         
            txtVendorName.Text = dtDeliveryDetails.Rows[0]["Entity"].ToString();
            string strVendorAddress =SetVendorAddressdetails(dtDeliveryDetails.Rows[0]);
            S3GVendorAddress.SetCustomerDetails("", strVendorAddress, "",
            dtDeliveryDetails.Rows[0]["Entity_TelePhone"].ToString(),
            dtDeliveryDetails.Rows[0]["Entity_Mobile"].ToString(),
            dtDeliveryDetails.Rows[0]["Entity_EMail"].ToString(),
            dtDeliveryDetails.Rows[0]["Entity_Website"].ToString());
            // DataTable dtVendorDet1 = new DataTable();
            if (PageMode != PageModes.Create)
            {
                lblVAddress1.Text = dtDeliveryDetails.Rows[0]["Address"].ToString();
                lblVAddress2.Text = dtDeliveryDetails.Rows[0]["Address2"].ToString();
                lblVCity.Text = dtDeliveryDetails.Rows[0]["EMCity"].ToString();
                lblVPincode.Text = dtDeliveryDetails.Rows[0]["PINCode"].ToString();
                lblVState.Text = dtDeliveryDetails.Rows[0]["State"].ToString();
                lblVCountry.Text = dtDeliveryDetails.Rows[0]["Country"].ToString();
            }

            if ((ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")) && (ddlMLA.SelectedValue == "0"))
            {
                Panel2.GroupingText = "Company Details";
                S3GCustomerAddress1.Caption = "Company";
            }

            S3GCustomerAddress1.SetCustomerDetails(dtDeliveryDetails.Rows[0],true);

            //FunPriGetCustomerDetails(ddlMLA.SelectedItem.Text);
            if (lblIsActivated.Text == "1")
            {
              btnDICancel.Enabled = false;
              btnPrint.Enabled = true;
              btnEmail.Enabled = true;
              btnDLGeneration.Enabled = true;
             
            }
            if (lblIsActivated.Text == "3")
            {
                btnDICancel.Enabled = false;
            }
            //if (lblIsPrint.Text == "True")
            //{
            //    btnDICancel.Enabled = false;
            //}
            if (lblStatus.Text == "2")
            {
                btnDLGeneration.Enabled = false;
                btnDICancel.Enabled = false;
                btnEmail.Enabled = false;
                btnPrint.Enabled = false;
            }
                

            if (lblIsActivated.Text == "0")
            {
                btnDICancel.Enabled = true;
                btnDLGeneration.Enabled = true;
                btnEmail.Enabled = true;
                btnPrint.Enabled = true;
            }


            ///Commented by R. Manikandan to enable the Cancel DI Button
            //OR condition added for Oracle data flow - Kuppusamy.B - 18-April-2012

            if (lblIsPrint.Text == "1" && lblStatus.Text != "2")
            {
                btnEmail.Enabled = true;
                btnPrint.Enabled = true;
            }
            else
            {
                btnEmail.Enabled = false;
                btnPrint.Enabled = false;
            }

            //if (lblIsPrint.Text == "True" && lblIsActivated.Text == "0")
            //{
            //    btnDICancel.Enabled = false;
            //}


        }
        catch (FaultException<LoanAdminMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            cvDelivery.ErrorMessage = objFaultExp.Detail.ProReasonRW;
            cvDelivery.IsValid = false;
        }
        catch (Exception ex)
        {
            cvDelivery.ErrorMessage = ex.Message;
            cvDelivery.IsValid = false;
        }

    }

    private string SetVendorAddressdetails(DataRow drCust)
    {
        string strAddress = "";
        if (drCust["Address"].ToString() != "") strAddress += drCust["Address"].ToString() + System.Environment.NewLine;
        if (drCust["Address2"].ToString() != "") strAddress += drCust["Address2"].ToString() + System.Environment.NewLine;
        if (drCust["EMCity"].ToString() != "") strAddress += drCust["EMCity"].ToString() + System.Environment.NewLine;
        if (drCust["State"].ToString() != "") strAddress += drCust["State"].ToString() + System.Environment.NewLine;
        if (drCust["Country"].ToString() != "") strAddress += drCust["Country"].ToString() + System.Environment.NewLine;
        if (drCust["Pincode"].ToString() != "") strAddress += drCust["Pincode"].ToString();
        return strAddress;
    }

    private void FunPriGetDeliveryGridDetails()
    {
        try
        {

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@DeliveryInstruction_ID", intDeliverID.ToString());
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtDeliveryGridViewDetails = Utility.GetDefaultData("S3G_LOANAD_DeliveryInstructionDocDetailsView", dictParam);
            ViewState["dtDeliveryGridViewDetails"] = dtDeliveryGridViewDetails;

            DataSet dset = new DataSet();
            dset = (DataSet)ViewState["DSet"];
            gvDlivery.DataSource = dtDeliveryGridViewDetails;
            gvDlivery.DataBind();
            dtDeliveryGridViewDetails.TableName = "ListTable1";
            dset.Tables.Add(dtDeliveryGridViewDetails);
            ViewState["DSet"] = dset;
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
    #endregion

    #region Role Access Setup/Page Load
    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                //FunPriGetDeliveryGridDetails();
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                txtLPONo.Enabled = false;
                btnDLGeneration.Enabled = false;
                btnPrint.Enabled = false;
                btnEmail.Enabled = false;
                btnDICancel.Enabled = false;
                break;

            case 1:
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    Response.Redirect(strRedirectPageView);
                }
                btnAdd.Visible = false;
                lblAssetCodeOl.Visible = false;
                ddlAssetCode.Visible = false;
              
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = true;
                txtLPONo.ReadOnly = true;
                txtDate.ReadOnly = true;
                ddlBranch.ReadOnly = true;
                rfvMLA.Visible = false;
                rfvVendorCode.Visible = false;
                FunPriGetDeliveryDetails();
                FunPriGetDeliveryGridDetails();
               if (bClearList)
                {
                    //ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    /*Condition added if ddlVendorCode.items.count = 0 - Kuppusamy.B - 30/01/2012*/
                    //if (ddlVendorCode.Items.Count > 0)
                    ddlVendorCode.ClearDropDownList();
                    //if(ddlMLA.Items.Count > 0)
                    //ddlMLA.ClearDropDownList();
                    //if(ddlSLA.Items.Count > 0)
                    //ddlSLA.ClearDropDownList();

                }
                break;


            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                btnAdd.Visible = false;
                lblAssetCodeOl.Visible = false;
                ddlAssetCode.Visible = false;
              
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                ddlBranch.ReadOnly = true;
                btnDICancel.Visible = false;
                btnCancel.Enabled = true;
                txtLPONo.ReadOnly = true;
                txtDate.ReadOnly = true;
                rfvMLA.Visible = false;
                rfvVendorCode.Visible = false;
                FunPriGetDeliveryDetails();
                FunPriGetDeliveryGridDetails();
                if (bClearList)
                {
                    //ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    ddlVendorCode.ClearDropDownList();
                    //if (ddlMLA.Items.Count > 0)
                    //ddlMLA.ClearDropDownList();
                    //if (ddlSLA.Items.Count > 0)
                    //ddlSLA.ClearDropDownList();
                }
                break;
        }

    }
    #endregion

    #region Button Events
    #region Save
    protected void btnSave_Click(object sender, EventArgs e)
    {
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            //if  && (ddlMLA.SelectedValue == "0"))
            //{
                
            //}
           
            int counts = 0;
            string strDelivery_No = string.Empty;

            
            
            foreach (GridViewRow grv in gvDlivery.Rows)
            {
                if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                {
                    counts++;
                }

            }
            if (counts == 0)
            {
                cvDelivery.ErrorMessage = "Select the Asset Details";
                cvDelivery.IsValid = false;
                return;
            }
            foreach (GridViewRow grv in gvDlivery.Rows)
            {
                int quantity;
                TextBox txtMoDesc = grv.FindControl("txtModelDesc") as TextBox;
                TextBox txtQuanlity = grv.FindControl("txtQuantity") as TextBox;
                TextBox txtRemarks = grv.FindControl("txtRemarks") as TextBox;
                TextBox txtAssetValue = grv.FindControl("txtAssetValue") as TextBox;

                        //ObjDeliveryInsRow.Asset_quantity = txtQuanlity.Text;
                if (txtMoDesc.Text.Length == 0 && txtQuanlity.Text.Length == 0)
                {
                    if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                    {
                        cvDelivery.ErrorMessage = "Enter the Asset Details";
                        cvDelivery.IsValid = false;
                        return;
                    }
                }
                
                if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                {
                    if (txtQuanlity.Text == "0")
                     {
                        cvDelivery.ErrorMessage = "Unit Quantity cannot be zero";
                        cvDelivery.IsValid = false;
                         return;
                     }
                }
                
                if (txtMoDesc.Text.Length == 0)
                {
                    if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                    {
                        cvDelivery.ErrorMessage = "Enter the Model Description";
                        cvDelivery.IsValid = false;
                        return;
                    }
                }
                
                if (txtQuanlity.Text.Length == 0)
                {
                    if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                    {
                        cvDelivery.ErrorMessage = "Enter the Unit Quantity";
                        //txtQuanlity.Clear();
                        txtQuanlity.Focus();
                        cvDelivery.IsValid = false;
                        return;
                    }
                }
                if ((ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT") && (ddlMLA.SelectedValue == "0") && ((txtAssetValue.Text.Length == 0 ) || (txtAssetValue.Text.Trim() == "0"))))
                    
                {
                    if (ViewState["AssetDetails"] == null)
                    {
                        cvDelivery.ErrorMessage = "Select the Asset Code";
                        ddlAssetCode.Focus(); 
                        cvDelivery.IsValid = false;
                        return;
                    }
                    
                    if(txtAssetValue.Text.Trim() == "")
                    {
                        cvDelivery.ErrorMessage = "Enter the Asset Value";
                        //txtQuanlity.Clear();
                        txtAssetValue.Focus();
                        cvDelivery.IsValid = false;
                        return;
                    }
                    else if (txtAssetValue.Text.Trim() == "0")
                    {
                        cvDelivery.ErrorMessage = "Asset Value Cannot be zero";
                        //txtQuanlity.Clear();
                        txtAssetValue.Focus();
                        cvDelivery.IsValid = false;
                        return;
                    }
                }


            }
            //ObjS3G_PRDDTransDocMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable();
            //PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterRow objPRDDTransDocRow;
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable ObjS3G_DeliveryInsDataTable = new LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionDataTable();
            LoanAdminMgtServices.S3G_LOANAD_InsertDeliveryInstructionRow ObjDeliveryInsRow;
            ObjDeliveryInsRow = ObjS3G_DeliveryInsDataTable.NewS3G_LOANAD_InsertDeliveryInstructionRow();
            ObjDeliveryInsRow.Company_ID = intCompanyId;
            ObjDeliveryInsRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjDeliveryInsRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);

            ObjDeliveryInsRow.PANum = Convert.ToString(ddlMLA.SelectedValue);
            if ((ddlMLA.SelectedValue == "0") && (ddlLOB.SelectedItem.Text.ToUpper().Contains("OPERAT")))
            {
                ObjDeliveryInsRow.SANum = Convert.ToString("0");
            }
            //ObjDeliveryInsRow.SANum = ddlSLA.SelectedItem.Text;
            else
            {
                getSAN();
                if (Convert.ToInt32(ddlSLA.SelectedIndex) == 0)
                {
                    ObjDeliveryInsRow.SANum = (ddlMLA.SelectedItem.Text + "DUMMY");
                }
                else
                {
                    ObjDeliveryInsRow.SANum = Convert.ToString(ddlSLA.SelectedItem.Text);
                }
            }
        

            if (intDeliverID == 0)
            {
                txtLPONo.Text = "";
            }
            else
            {
                ObjDeliveryInsRow.DeliveryInstruction_No = txtLPONo.Text;
            }
            if (lblCustID.Text != "")
            {
                ObjDeliveryInsRow.Customer_ID = Convert.ToInt32(lblCustID.Text);
            }
            else
            {
                ObjDeliveryInsRow.Customer_ID = Convert.ToInt32("0");
            }
            //ObjDeliveryInsRow.Customer_ID = Convert.ToInt32(S3GCustomerAddress1.CustomerCode);
            ObjDeliveryInsRow.Vendor_ID = Convert.ToInt32(ddlVendorCode.SelectedValue);
            ObjDeliveryInsRow.DeliveryInstruction_Date = Utility.StringToDate(txtDate.Text);
            //nvert.ToDateTime(txtDate.Text);
            ObjDeliveryInsRow.DeliveryInstruction_Status_Code = 1;
            ObjDeliveryInsRow.DeliveryInstruction_Statustype_Code = 3;
            ObjDeliveryInsRow.IS_LPO = true;
            ObjDeliveryInsRow.Created_By = intUserId;
            ObjDeliveryInsRow.Created_On = DateTime.Now;
            ObjDeliveryInsRow.TXN_Id = 11;
            ObjDeliveryInsRow.XML_DeliveryDeltails = FunProFormXML();

            ObjS3G_DeliveryInsDataTable.AddS3G_LOANAD_InsertDeliveryInstructionRow(ObjDeliveryInsRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_DeliveryInsDataTable, SerMode);
           


            if (intDeliverID > 0)
            {
                //intErrCode = objLoanAdmin_MasterClient.FunPubModifyCashflowMaster(SerMode, ObjCashDataTable);
            }
            else
            {
                intErrCode = objLoanAdmin_MasterClient.FunPubCreateDeliveryIns(out strDelivery_No, SerMode, ObjDeliveryInsDataTable);
            }
            if (intErrCode == 0)
            {
                if (intDeliverID > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Delivery Instruction updated successfully");
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                }
                else
                {
                    if (isWorkFlowTraveler) 
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strDelivery_No, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }
                        ShowWFAlertMessage(strDelivery_No, ProgramCode, strAlert);
                        return;
                    }
                    else 
                    {
                        // FORCE PULL IMPLEMENTATION KR
                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(ProgramCode, ddlMLA.SelectedItem.Text, null, "L", CompanyId, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), ddlMLA.SelectedItem.Text, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), ddlMLA.SelectedItem.Text, "", int.Parse(dtrForce["PRODUCTID"].ToString()));
                                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                                btnSave.Enabled = false;
                                //END
                            }
                            catch
                            {
                                strAlert = "Work Flow is not assigned";
                            }
                        }
                        strAlert = "Delivery Instruction details " + strDelivery_No + " added successfully";
                        strAlert += @"\n\nWould you like to add one more record?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                    } 
                
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Delivery Instruction details already exists");
                strRedirectPageView = "";
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined for delivery Instruction");
                strRedirectPageView = "";
            }

            else if (intErrCode == 6)
            {
                //Utility.FunShowAlertMsg(this.Page, "The Unit Quantity Entered Greater than the No of Asset Units");
                //return;
                cvDelivery.ErrorMessage = "The Unit Quantity Entered is Greater than the Number of Asset Units";
                cvDelivery.IsValid = false;
                return;

            }
            else if (intErrCode == 3)
            {
                
                //Utility.FunShowAlertMsg(this.Page, "The Unit Quantity Should not be Zero");
                //return;
                cvDelivery.ErrorMessage = "The Unit Quantity Should not be Zero";
                cvDelivery.IsValid = false;
                return;
            }


            else if (intErrCode == -1)
            {
                if (intDeliverID == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    strRedirectPageView = "";
                }

            }
            else if (intErrCode == -2)
            {
                if (intDeliverID == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    strRedirectPageView = "";
                }

            }
            else if (intErrCode == 50)
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to save the record");
                return;
            }
            //else if (intErrCode == -1)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined for delivery Instruction");
            //    strRedirectPageView = "";
            //}

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
            //objLoanAdmin_MasterClient.Close();

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            cvDelivery.ErrorMessage = objFaultExp.Detail.ProReasonRW; ;
            cvDelivery.IsValid = false;
            return;
            //lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            cvDelivery.ErrorMessage = "Unable to Save" ;
            cvDelivery.IsValid = false;
            return;
            //lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            if (objLoanAdmin_MasterClient != null)
            {
                objLoanAdmin_MasterClient.Close();
            }
        }
    }       
    #endregion

    #region Other Button 
    #region Cancel Button
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            
        Response.Redirect("../LoanAdmin/S3gLoanAdTransLander.aspx?Code=DEI");
    }
#endregion
    
    #region Cancel Delivery Instruction
    protected void btnDICancel_Click(object sender, EventArgs e)
    {
        int ErrorCode = 0;
        string Flag;
        Flag = "C";
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesClient();
        try
        {   
            int intResult = objLoanAdmin_MasterClient.FunCancelDeliveryIns(out ErrorCode, intDeliverID, Flag);
            if (intResult == 0)
            {
                //foreach (GridViewRow grv in gvDlivery.Rows)
                //{
                //    TextBox txtRemarks = grv.FindControl("txtRemarks") as TextBox;
                //    txtRemarks.Text = "Delivery Instruction Cancelled";
                //    //btnDICancel.Enabled = false;
                //}
                //strKey = "Cancel";
                //strAlert = "Do you want to delete Delivery Instruction/LPO Details";
                //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageView + "}";
                //strRedirectPageView = "";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                string strRedi = "../LoanAdmin/S3gLoanAdTransLander.aspx?Code=DEI";
                Utility.FunShowAlertMsg(this.Page, "Delivery Instruction / LPO Cancelled successfully", strRedi);
                return;
              
                //btnDICancel.Enabled = false;
                //btnDLGeneration.Enabled = false;
                //btnEmail.Enabled = false;
                //btnPrint.Enabled = false;
            }
            if (intResult == 5)
            {
                Utility.FunShowAlertMsg(this.Page, "Delivery Instruction / LPO Cancelation Failed");
                btnDICancel.Enabled = false;
            }
            // modified by anitha 8/4/2011
            // Again Modified by R.Manikandan to allow DI for cancellation after it has been generated
            // modified on Nov 25 2011
            //if (intResult == 7)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Delivery Instruction / LPO already generated, So unable to cancel DI/LPO");
            //    btnDICancel.Enabled = false;
            //}

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvDelivery.ErrorMessage = "Unable to cancel Delivery Instruction / LPO ";
            cvDelivery.IsValid = false;
        }
        finally
        {
            objLoanAdmin_MasterClient.Close();
        }
    }
    #endregion

    #region DL Generation Button Events
    protected void btnDLGeneration_Click(object sender, EventArgs e)
    {
        try
        {
            FunCrstalReportGeneration("D");
        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "LPO Generation PDF already open");
            //throw ex;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvDelivery.ErrorMessage = "Unable to Generate Delivery Instruction / LPO ";
            cvDelivery.IsValid = false;
        }

        //finally
        //{
        //   objLoanAdmin_MasterClient.Close();
        //}

    }


    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    //base.VerifyRenderingInServerForm(control);
    //}

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        FunCrstalReportGeneration("P");
    }
    #endregion

    #region TO Print Delivery Instruction
    private void FunPrint(String strFlag)
    {
        int ErrorCode = 0;
        //string Flag;
        //Flag = "P";
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesClient();
        try
        {
         
            int counts = 0;
            foreach (GridViewRow grv in gvDlivery.Rows)
            {
                if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                {
                    counts++;
                }

            }
            if (counts == 0)
            {
                cvDelivery.ErrorMessage = "Select atleast one Asset details";
                cvDelivery.IsValid = false;
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Asset_Description");
            dt.Columns.Add("Model_description");
            dt.Columns.Add("Asset_quantity");
            dt.Columns.Add("Asset_Value");
            dt.Columns.Add("Remarks");
            int intRowindex = 0;
            foreach (GridViewRow grv in gvDlivery.Rows)
            {
                if (((CheckBox)grv.FindControl("CbAssets")).Checked)
                {
                    //      Label lalAssetCode = grv.FindControl("lblAssetCode") as Label;
                    Label lalAssetDesc = grv.FindControl("lblAssetDesc") as Label;
                    TextBox txtModel = grv.FindControl("txtModelDesc") as TextBox;
                    TextBox txtquly = grv.FindControl("txtQuantity") as TextBox;
                    TextBox txtvalue = grv.FindControl("txtAssetValue") as TextBox;
                    TextBox txtRemarks = grv.FindControl("txtRemarks") as TextBox;
                    dt.NewRow();
                    DataRow dr = dt.NewRow();
                    //    dr["Asset_Code"] = lalAssetCode.Text;
                    dr["Asset_Description"] = lalAssetDesc.Text;
                    dr["Model_description"] = txtModel.Text;
                    dr["Asset_quantity"] = txtquly.Text;
                    dr["Asset_Value"] = txtvalue.Text;
                    dr["Remarks"] = txtRemarks.Text;
                    dt.Rows.Add(dr);

                }

            }
            ViewState["dt"] = dt;
            intRowindex++;

            //Guid objGuid = new Guid();
            //string strnewFile;
            //string strFileName;
            if (txtLPONo.Text != string.Empty)
            {

            //    strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + txtLPONo.Text.Replace("/", "").Replace(" ", "").Replace(":", "").Replace("-", "") + ".pdf");
            //    strFileName = "/LoanAdmin/PDF Files/" + txtLPONo.Text.Replace("/", "").Replace(" ", "-").Replace(":", "").Replace("-", "") + ".pdf";

            //    string[] str_newfile = strnewFile.Split('\\');
            //    string str1 = str_newfile[str_newfile.Length - 1];
            //    string[] str_filename = strFileName.Split('/');
            //    string str2 = str_filename[str_filename.Length - 1];

            //    if (File.Exists(strnewFile))
            //    {
            //        File.Delete(strnewFile);
            //    }

                String htmlText;
               
                int intResult = 0;

                DataSet dset = new DataSet();
                dset = (DataSet)ViewState["DSet"];
               
                if (strFlag == "P")
                {
                    dset.Tables[1].Rows[0]["Is_DIPrint"] = "1";
                    intResult = objLoanAdmin_MasterClient.FunCancelDeliveryIns(out ErrorCode, intDeliverID, strFlag);
                }
                if (strFlag == "D")
                {

                    //Code modified for Modified Report - For both Oracle and SQL DB - Kuppusamy.B - 18-April-2012
                    if (dset.Tables[1].Rows[0]["Is_DIGeneration"] == "1")
                    {
                        dset.Tables[1].Rows[0]["Is_DIPrint"] = "1";
                    }

                    //if (Convert.ToBoolean(dset.Tables[1].Rows[0]["Is_DIGeneration"]) == true)
                    //{
                    //    dset.Tables[1].Rows[0]["Is_DIPrint"] = "True";
                    //}

                    // Modified for report generation - for Is_DIGeneration value is 1 or 0 - Kuppusamy.B - Feb-13-2012
                    
                    //For SQL_Database 
                    //dset.Tables[1].Rows[0]["Is_DIGeneration"] = "True"; 
                    //For Oracle Database 
                    //dset.Tables[1].Rows[0]["Is_DIGeneration"] = "1";

                    // Modified for report generation - for Is_DIGeneration value is 1 or 0 - Kuppusamy.B - April-19-2012
                    //Common code for SQL & Oracle DB
                    dset.Tables[1].Rows[0]["Is_DIGeneration"] = "1";

                    intResult = objLoanAdmin_MasterClient.FunCancelDeliveryIns(out ErrorCode, intDeliverID, strFlag);
                }
                ViewState["DSet"] = dset;
                #region Comemented Parts
                //    if (intResult != 0)
                //    // if (str1 != str2)
                //    {

                //        String htmlTexts = GetHTMLDupText();
                //        htmlText = htmlTexts;
                //        Document doc = new Document();
                //        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));


                //        doc.AddCreator("Sundaram Infotech Solutions");
                //        doc.AddTitle("New PDF Document");
                //        doc.Open();
                //        List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
                //        for (int k = 0; k < htmlarraylist.Count; k++)
                //        { doc.Add((IElement)htmlarraylist[k]); }
                //        doc.AddAuthor("S3G Team");
                //        doc.Close();
                //        //System.Diagnostics.Process.Start(strnewFile);
                //        string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

                //        //PdfAction.JavaScript("this.print(true);", writer);
                //        //doc.Close();
                //    }
                //    else
                //    {
                //        String htmlTexts = GetHTMLText();
                //        htmlText = htmlTexts;
                //        Document doc = new Document();
                //        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));


                //        doc.AddCreator("Sundaram Infotech Solutions");
                //        doc.AddTitle("New PDF Document");
                //        doc.Open();
                //        List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
                //        for (int k = 0; k < htmlarraylist.Count; k++)
                //        { doc.Add((IElement)htmlarraylist[k]); }
                //        doc.AddAuthor("S3G Team");
                //        doc.Close();
                //        //System.Diagnostics.Process.Start(strnewFile);
                //        string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

                //    }

                //}
                //else
                //{
                //    strnewFile = (Server.MapPath(".") + "\\PDF Files\\LPO Generation.pdf");
                //    strFileName = "/LoanAdmin/PDF Files/" + "LPO Generation.pdf";
                //}
                #endregion

                btnEmail.Enabled = true;
                btnPrint.Enabled = true;
                
            }
        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "DI/LPO Generation PDF already open");
            //throw ex;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvDelivery.ErrorMessage = "Unable to Generate Delivery Instruction / LPO ";
            cvDelivery.IsValid = false;
        }
        finally
        {
            if (objLoanAdmin_MasterClient != null)
            {
                objLoanAdmin_MasterClient.Close();
                
            }
        }
    }
    #region HTML Formation 
    private string FunPriGetHtmlTable()
    {
        DataTable dt_Asset = new DataTable();
        dt_Asset = (DataTable)ViewState["dt"];
        string strHtml = string.Empty;
        // strHtml = "<table border=\"1\" >";// border=\"1px\" width=\"100%\">";
        for (int i_row = 0; i_row < dt_Asset.Rows.Count; i_row++)
        {
            strHtml += " <tr>";

            for (int i_column = 0; i_column < dt_Asset.Columns.Count; i_column++)
            {

                //for (int i = 0; i < dt_Asset.Rows.Count; i++)
                //{
                if ((dt_Asset.Columns[i_column].ColumnName == "Asset_quantity") || (dt_Asset.Columns[i_column].ColumnName == "Asset_Value"))
                {
                    strHtml += " <td align =\"right\" > ";
                }
                else
                {
                    strHtml += " <td> ";
                }
                strHtml += dt_Asset.Rows[i_row][i_column].ToString();

                strHtml += " </td> ";

            }
            strHtml += " </tr> ";
            //}

        }
        // strHtml += "</table>";
        dt_Asset.Clear();
        dt_Asset.Dispose();
        return strHtml;
    }
    #endregion

    #region Email

    protected void FunEmailGeneration(string strFilePath)
    {
        try
        {
            Dictionary<string, string> dictMail = new Dictionary<string, string>();
            if (S3GVendorAddress.EmailID == string.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, " Unable to send the Email Due to Email Id is not Given");
                return;
            }
            dictMail.Add("FromMail", "manikandan.r@sundaraminfotech.in");
            dictMail.Add("ToMail",  S3GVendorAddress.EmailID);
            dictMail.Add("Subject", "Delivery Instruction/LPO");

            ArrayList arrMailAttachement = new ArrayList();
            arrMailAttachement.Add(strFilePath);

            StringBuilder strBody = new StringBuilder();
            strBody.Append("DI created sucessfully");

            try
            {
                Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
                //Utility.FunShowAlertMsg(this, "Mail sent successfully");
            }
            catch (Exception objException)
            {
                //if (objException.Message == "Mailbox unavailable. The server response was: 5.7.1 Unable to relay")
                if (objException.Message == "Error in :Mailbox unavailable. The server response was: 5.7.1 Client does not have permissions to send as this sender")
                {
                    Utility.FunShowAlertMsg(this, "Mail not sent.");
                }
                return;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            //Utility.FunShowAlertMsg(this, "Unable to Send Mail");
            if (objException.Message == "Error in :Mailbox unavailable. The server response was: 5.7.1 Client does not have permissions to send as this sender")
            {
                Utility.FunShowAlertMsg(this, "Mail not sent.");
            }
            return;
        }

    }


    protected void btnEmail_Click(object sender, EventArgs e)
   {
       try
       {
           FunCrstalReportGeneration("E");
           #region Commented

           //    int counts = 0;
           //    foreach (GridViewRow grv in gvDlivery.Rows)
           //    {
           //        if (((CheckBox)grv.FindControl("CbAssets")).Checked)
           //        {
           //            counts++;
           //        }

           //    }
           //    if (counts == 0)
           //    {
           //        cvDelivery.ErrorMessage = "Select atleast one Asset details.";
           //        cvDelivery.IsValid = false;
           //        return;
           //    }
           //    DataTable dt = new DataTable();
           //    dt.Columns.Add("Asset_Code");
           //    dt.Columns.Add("Asset_Description");
           //    dt.Columns.Add("Model_description");
           //    dt.Columns.Add("Asset_quantity");
           //    dt.Columns.Add("Asset_Value");
           //    dt.Columns.Add("Remarks");

           //    foreach (GridViewRow grv in gvDlivery.Rows)
           //    {
           //        if (((CheckBox)grv.FindControl("CbAssets")).Checked)
           //        {
           //            Label lalAssetCode = grv.FindControl("lblAssetCode") as Label;
           //            Label lalAssetDesc = grv.FindControl("lblAssetDesc") as Label;
           //            TextBox txtModel = grv.FindControl("txtModelDesc") as TextBox;
           //            TextBox txtquly = grv.FindControl("txtQuantity") as TextBox;
           //            TextBox txtvalue = grv.FindControl("txtAssetValue") as TextBox;
           //            TextBox txtRemarks = grv.FindControl("txtRemarks") as TextBox;
           //            dt.NewRow();
           //            DataRow dr = dt.NewRow();
           //            dr["Asset_Code"] = lalAssetCode.Text;
           //            dr["Asset_Description"] = lalAssetDesc.Text;
           //            dr["Model_description"] = txtModel.Text;
           //            dr["Asset_quantity"] = txtquly.Text;
           //            dr["Asset_Value"] = txtvalue.Text;
           //            dr["Remarks"] = txtRemarks.Text;
           //            dt.Rows.Add(dr);

           //        }

           //    }
           //    ViewState["dt"] = dt;

           //    string body;
           //    body =  GetHTMLText();
           //    CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
           //    ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
           //    ObjCom_Mail.ProFromRW = "s3g@sundaraminfotech.in";
           //    if (S3GVendorAddress.EmailID == string.Empty)
           //    {
           //        Utility.FunShowAlertMsg(this.Page, " Unable to send the Email Due to Email Id is not Given");
           //        return;
           //    }
           //    ObjCom_Mail.ProTORW = S3GVendorAddress.EmailID;          
           //    ObjCom_Mail.ProSubjectRW = "Delivery Instruction";
           //    ObjCom_Mail.ProMessageRW = body;
           //    ObjCommonMail.FunSendMail(ObjCom_Mail);
           //    Utility.FunShowAlertMsg(this, "Mail sent successfully");
           //}
           //catch (Exception objException)
           //{
           //      ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
           //    //Utility.FunShowAlertMsg(this, "Unable to Send Mail");
           //    if (objException.Message == "Mailbox unavailable. The server response was: 5.7.1 Unable to relay")
           //    {
           //        Utility.FunShowAlertMsg(this, " Invalid EMail ID. Mail not sent. ");
           //    }
           //    return;
           //}

           //try
           //{
           //    string body;
           //    body = "Respected Sir/Madam, <br/>  <br/> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp " +
           //              (S3GCustomerAddress1.FindControl("txtCustomerName") as TextBox).Text;
           //    body += "<br/><br/>With thanks and regards,<br/>";


           //    CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
           //    ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
           //    ObjCom_Mail.ProFromRW =  "s3g@sundaraminfotech.in";
           //    ObjCom_Mail.ProTORW = S3GCustomerAddress1.EmailID;// txtMEmailid.Text;

           //    ObjCom_Mail.ProSubjectRW = "Delivery Instruction";
           //    ObjCom_Mail.ProMessageRW = body;
           //    ObjCommonMail.FunSendMail(ObjCom_Mail);
           //    Utility.FunShowAlertMsg(this, "Mail sent successfully");
           //}
           #endregion
       }
       catch (Exception objException)
       {
             ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
           //Utility.FunShowAlertMsg(this, "Unable to Send Mail");
           if (objException.Message == "Mailbox unavailable. The server response was: 5.7.1 Unable to relay")
           {
               Utility.FunShowAlertMsg(this, "Mail not sent.");
           }
           return;
       }
    
   }
    #endregion

    #endregion

    #region Clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlLOB.SelectedIndex = 0;
        //ddlBranch.SelectedIndex = 0;
        ddlBranch.Clear();
        //if (Convert.ToInt32(ddlBranch.SelectedValue) == 0)
        //{
        //    ddlVendorCode.SelectedIndex = -1;
        //    ddlMLA.SelectedIndex = -1;
        //    ddlSLA.SelectedIndex = -1;
        //}
        //ddlVendorCode.SelectedIndex = -1;

        ddlVendorCode.Items.Clear();
        ddlMLA.Items.Clear();
        ddlSLA.Items.Clear();
        txtVendorName.Text = "";

        S3GVendorAddress.ClearCustomerDetails();
        S3GCustomerAddress1.ClearCustomerDetails();
        lblAssetCodeOl.Visible = btnAdd.Visible = ddlAssetCode.Visible = false;
        //ddlDILOP.SelectedIndex = 0;
        
        //gvDlivery.Visible = false;
        if(bCreate) 
        btnSave.Enabled = true;
        

        Clear();
//        Panel3.Visible = false;
        gvDlivery.DataSource = null;
        gvDlivery.DataBind();
    }

    private void Clear()
    {
        S3GCustomerAddress1.ClearCustomerDetails();
        S3GVendorAddress.ClearCustomerDetails();
        txtVendorName.Text = "";
        ddlVendorCode.Items.Clear();
        ddlAssetCode.Items.Clear();
        ddlAssetCode.Visible = false;
        if (ViewState["AssetDetails"] != null)
        {
            ViewState["AssetDetails"] = null;
        }
        

        ddlMLA.Items.Clear();
        if (PageMode == PageModes.Create)
        {
            ddlMLA.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
            ddlVendorCode.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
        }
        ddlSLA.Items.Clear();
        gvDlivery.DataSource = null;
        gvDlivery.DataBind();
    }
    #endregion

    
    #endregion
    #endregion

    #region Other Functions
    // modified by Anitha 7-Apr-2011

    //#region Date Format
    //private string ConvertToCurrentFormat(string strDate)
    //{
    //    //  string dT = strDate;

    //    try
    //    {
    //        if (strDate.Contains("1900"))
    //            strDate = string.Empty;


    //        strDate = strDate.Replace("12:00:00 AM", "");

    //        CultureInfo myDTFI = new CultureInfo("en-GB", true);
    //        DateTimeFormatInfo DTF = myDTFI.DateTimeFormat;
    //        DTF.ShortDatePattern = "dd/MM/yyyy";
    //        DateTime _Date = new DateTime();
    //        if (strDate != "")
    //        {
    //            _Date = System.Convert.ToDateTime(strDate, DTF);
    //            return _Date.ToString("dd/MM/yyyy");
    //        }
    //        else
    //            return string.Empty;
    //    }
    //    catch (Exception ex)
    //    {
    //        return strDate;
    //        // throw ex;
    //    }
    //}
    //#endregion

    #region Calculation of Asset
    //protected void FunCalculation(object sender, EventArgs e)
    //{
    //    decimal total = 0;
    //    string strFieldAtt = ((TextBox)sender).ClientID;
    //    string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvDlivery")).Replace("gvDlivery_ctl", "");
    //    int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
    //    gRowIndex = gRowIndex - 2;
    //    TextBox Quly = (TextBox)gvDlivery.Rows[gRowIndex].FindControl("txtQuantity");
    //    TextBox calc = (TextBox)gvDlivery.Rows[gRowIndex].FindControl("txtDyAsset");
    //    TextBox Dcal = (TextBox)gvDlivery.Rows[gRowIndex].FindControl("txtAssetValue");
       
    //    if (Quly.Text != "")
    //        total = Convert.ToDecimal(Quly.Text) * Convert.ToDecimal(calc.Text);
    //    else
    //        total = 0;
    //    Dcal.Text = total.ToString();

    //}


    //protected void txtQuantity_TextChange(object sender, EventArgs e)
    //{
    //    decimal total = 0;
    //    TextBox Quly = (TextBox)sender;
    //    Quly.AutoPostBack = true;
    //    GridViewRow gvRow = (GridViewRow)Quly.Parent.Parent;
    //    //ddlSubAccount = (DropDownList)gvMLASLA.Rows[gvRow.RowIndex].FindControl("ddlSANum");
    //    if (Request.QueryString["qsMode"] == "C")
    //    {
    //        TextBox calc = (TextBox)gvDlivery.Rows[gvRow.RowIndex].FindControl("txtDyAsset");
    //        TextBox Dcal = (TextBox)gvDlivery.Rows[gvRow.RowIndex].FindControl("txtAssetValue");
    //        if (Quly.Text != "")
    //            total = Convert.ToDecimal(Quly.Text) * Convert.ToDecimal(calc.Text);
    //        else
    //            total = 0;
    //        Dcal.Text = total.ToString();
    //    }

    //}









    #endregion

    #endregion
    #region CrstalReport Open
    private void FunCrstalReportGeneration(string strFlag)
    {
         try
            {
                Guid objGuid;
                objGuid = Guid.NewGuid();
                DataSet dset = new DataSet();
                dset = (DataSet)ViewState["DSet"];
              if(strFlag == "D")
                dset.Tables[1].Rows[0]["Temp1"] = "1";
              else
                dset.Tables[1].Rows[0]["Temp1"] = "0";

                ReportDocument rpd = new ReportDocument();
                rpd.Load(Server.MapPath("DeliveryInstruction.rpt"));

                rpd.SetDataSource(dset.Tables[1]);
                rpd.Subreports[0].SetDataSource(dset.Tables[0]);

               string strFileName = Server.MapPath(".") + "\\PDF Files\\DI\\" + txtLPONo.Text.Replace("/", "") + objGuid.ToString() + ".pdf";

               string strFolder = Server.MapPath(".") + "\\PDF Files\\DI";

                //if (strFlag == "P")
                //{
                //    var outputStream = new MemoryStream();
                //    var pdfReader = new PdfReader(strFileName);
                //    var pdfStamper = new PdfStamper(pdfReader, outputStream);
                //    //Add the auto-print javascript
                //    var writer = pdfStamper.Writer;
                //    writer.AddJavaScript(GetAutoPrintJs());
                //    pdfStamper.Close();
                //    var content = outputStream.ToArray();
                //    outputStream.Close();
                //    Response.ContentType = "application/pdf";
                //    Response.BinaryWrite(content);
                //    Response.End();
                //    outputStream.Close();
                //    outputStream.Dispose();
                //}


               if (!(System.IO.Directory.Exists(strFolder)))
               {
                 DirectoryInfo di = Directory.CreateDirectory(strFolder);
                 
               }
             //if (System.IO.Directory.Exists(strFolder))
             //   {
             //       string[] files = System.IO.Directory.GetFiles(strFolder);
             //       foreach (string s in files)
             //       {
             //           File.Delete(s);
             //       }

             //   }
      
      
                string strScipt = "";
                rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName); //ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "DeliveryInstruction");
                if (strFlag == "E")
                {
                    FunEmailGeneration(strFileName);
                    return;
                }
                else if (strFlag == "D")
                {
                    strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=/LoanAdmin/PDF Files/DI/" + txtLPONo.Text.Replace("/", "") + objGuid.ToString() + ".pdf', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                }

                // string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=/LoanAdmin/PDF Files/DI/" + txtLPONo.Text.Replace("/", "") + objGuid.ToString() + ".pdf&qsNeedPrint=no', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                else
                {
                    strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

              if(strFlag == "D")  
                FunPrint("D");
              else
                FunPrint("P");

            }
            catch (System.IO.IOException ex)
            {
                Utility.FunShowAlertMsg(this.Page, "Error: Unable to Generate DI/LPO");
                //throw ex;
            }
            catch (Exception objException)
            {
                  ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
                cvDelivery.ErrorMessage = "Unable to Generate Delivery Instruction / LPO ";
                cvDelivery.IsValid = false;
            }

        
    }
    protected string GetAutoPrintJs()
    {
        var script = new StringBuilder();
        script.Append("var pp = getPrintParams();");
        script.Append("pp.interactive= pp.constants.interactionLevel.full;");
        script.Append("print(pp);"); return script.ToString();
    }
    #endregion
    protected void FunGetCompanyDetails(int Company_Id)
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", intCompanyId.ToString());
       
        DataTable dtCompany = Utility.GetDefaultData("S3G_LOANAD_GetCompany_DO", dictParam);
       // DataRow dtRow = dtCompany.Rows[0];
        S3GCustomerAddress1.SetCustomerDetails(dtCompany.Rows[0], true);


    }
    //  removed ddlAsset Code Commmented on 17 - Nov - 2011 by R. Manikadan r
   

    // Created on 17 - Nov - 2011 to create more account for OL 
    // Modified by R. Manikandan
  
    protected void btnAdd_Click(object sender, EventArgs e)
    {
       
        rfvMLA.Enabled = false;
        DataTable dtAssetCode;
        DataRow Drow;
        if (ddlAssetCode.SelectedIndex <= 0)
        {
            cvDelivery.ErrorMessage = "Select the Asset Code";
            cvDelivery.IsValid = false;
            return;
        }
       
        dtAssetCode = new DataTable("AssetDetails");
         if (ViewState["AssetDetails"] == null)
        {
            dtAssetCode.Columns.Add("Asset_ID");
            dtAssetCode.Columns.Add("Asset_Code");
            dtAssetCode.Columns.Add("Asset_Description");
            dtAssetCode.Columns.Add("Model_Description");
            dtAssetCode.Columns.Add("ASSET_COST");
            dtAssetCode.Columns.Add("Asset_quantity");
            dtAssetCode.Columns.Add("Asset_Value");
            dtAssetCode.Columns.Add("Remarks");
        }
        else
        {
            dtAssetCode = (DataTable)ViewState["AssetDetails"];
        }

          
         if(dtAssetCode.Rows.Count >= 0)
         {
            Drow = dtAssetCode.NewRow();
            if (dtAssetCode.Rows.Count > 0)
            {
                dtAssetCode = (DataTable)ViewState["AssetDetails"];
            }
            if (ddlAssetCode.SelectedIndex > 0)
            {
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Company_ID", intCompanyId.ToString());
                dictParam.Add("@Asset_ID", Convert.ToString(ddlAssetCode.SelectedValue));
                DataSet dsAssetCode = Utility.GetDataset("S3G_LOANAD_AssetCodeOL_DO", dictParam);
                DataTable dtAsset = dsAssetCode.Tables[1];
               

                Drow["Asset_ID"] = ddlAssetCode.SelectedValue;
                Drow["Asset_Code"] = dtAsset.Rows[0]["Asset_Code"].ToString();
                Drow["Asset_Description"] = dtAsset.Rows[0]["Asset_Description"].ToString();
                Drow["Model_Description"] = dtAsset.Rows[0]["Model_Description"].ToString();
                Drow["ASSET_COST"] = dtAsset.Rows[0]["ASSET_COST"].ToString();
                Drow["Asset_quantity"] = dtAsset.Rows[0]["Asset_quantity"].ToString();
                Drow["Asset_Value"] = dtAsset.Rows[0]["Asset_Value"].ToString();
                Drow["Remarks"] = dtAsset.Rows[0]["Remarks"].ToString();
            }

            dtAssetCode.Rows.Add(Drow);
            ViewState["AssetDetails"] = dtAssetCode;
            gvDlivery.DataSource = dtAssetCode;
            gvDlivery.DataBind();
            if (ddlAssetCode.SelectedIndex > 0)
            {
                ddlAssetCode.Items.RemoveAt(ddlAssetCode.SelectedIndex);
                ddlAssetCode.SelectedValue = "0";
            }
         }
         foreach (GridViewRow grv in gvDlivery.Rows)
         {
             //TextBox txtAssetValue1 = grv.FindControl("txtAssetValue1") as TextBox;
             TextBox txtAssetValue = grv.FindControl("txtAssetValue") as TextBox;
             TextBox txtDyAsset = grv.FindControl("txtDyAsset") as TextBox;
             TextBox txtQuantity = grv.FindControl("txtQuantity") as TextBox;
            // txtAssetValue1.Visible = true;
             txtAssetValue.ReadOnly = false;
             txtDyAsset.ReadOnly = false;
             txtDyAsset.Visible = false;
             //txtAssetValue.Visible = false;
             txtQuantity.ReadOnly = false;
            
             txtAssetValue.Text = txtDyAsset.Text; // = txtAssetValue1.Text;

         }
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

        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "055");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

}

  


#endregion

