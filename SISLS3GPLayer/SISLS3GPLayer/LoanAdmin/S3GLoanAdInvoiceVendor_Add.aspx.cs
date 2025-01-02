
#region Page Header
//Module Name      :   LoanAdmin
//Screen Name      :   S3GLoanAdInvoiceVendor_Add.aspx
//Created By       :   Kaliraj K
//Created Date     :   18-JUL-2010
//Purpose          :   To insert and update invoice vendor details

//Modified By      :   Gunasekar.K
//Modified On      :   30th- Nov-2010
//Purpose          :   Unit value should not be editable but No of units should be always one for Operating Lease LOB        

#endregion

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
using System.Web.UI.HtmlControls;

public partial class S3GLoanAdInvoiceVendor_Add : ApplyThemeForProject
{
    #region initialization

    LoanAdminMgtServicesReference.LoanAdminMgtServicesClient ObjInvoiceClient;
    LoanAdminMgtServices.S3G_LOANAD_InvoiceVendorDetailsDataTable ObjS3G_LOANAD_InvoiceVendorDetailsDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;
    ClsSystemJournal ObjSysJournal = new ClsSystemJournal();
    int intErrCode = 0;
    string strInvoiceNo = string.Empty;
    int intInvoiceID = 0;
    int intUserID = 0;
    decimal dScore = 0;
    int intCompanyID = 0;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    DataSet dsAsset = new DataSet();
    Dictionary<string, string> Procparam = null;
    Dictionary<string, string> dictLOB = null;

    StringBuilder strbLOBDet = new StringBuilder();
    StringBuilder strbInvoiceDet = new StringBuilder();
    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=IVE";
    string strKey = "Insert";

    public static S3GLoanAdInvoiceVendor_Add obj_Page;

    string strXMLAssetDet = string.Empty;
    string strXMLWarrantyDet = string.Empty;
    string strFileName = string.Empty;
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdInvoiceVendor_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=IVE';";
    DataTable dtDefault = new DataTable();
    DataTable dtFrequency = new DataTable();
    int iCount = 0;
    DataTable dtInvoice = new DataTable();

    #endregion

    #region PageLoad

    /// <summary>
    /// Event for Pre-Initialize the Components  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /* protected new void Page_PreInit(object sender, EventArgs e)
     {
         try
         {

             if (Request.QueryString["IsFromAccount"] != null)
             {
                 this.Page.MasterPageFile = "~/Common/MasterPage.master";
                 UserInfo ObjUserInfo = new UserInfo();
                 this.Page.Theme = ObjUserInfo.ProUserThemeRW;
             }
             else
             {
                 this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                 UserInfo ObjUserInfo = new UserInfo();
                 this.Page.Theme = ObjUserInfo.ProUserThemeRW;
             }
         }
         catch (Exception objException)
         {
               ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
             throw new ApplicationException("Unable to Initialise the Controls in Vendor Invoice");
         }
     }
 */
    protected void Page_Load(object sender, EventArgs e)
    {
        //WF Implementation
        ProgramCode = "058";
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        //HtmlForm fMP = Master.FindControl("form1") as HtmlForm;
        //fMP.Attributes.Add("onKeyDown","return fnCheckKeyPress();");
        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        //txtInvoiceDate.Attributes.Add("readonly", "readonly");

        CalendarExtender1.Format = strDateFormat;
        txtInvoiceDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoiceDate.ClientID + "','" + strDateFormat + "',true,  false);");
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));

            if (fromTicket != null)
            {
                intInvoiceID = Convert.ToInt32(fromTicket.Name);
                hdnInvoiceID.Value = intInvoiceID.ToString();
                strMode = Request.QueryString.Get("qsMode");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        intInvoiceID = Convert.ToInt32(hdnInvoiceID.Value);
        //intInvoiceID = 5;
        //strMode = "M";

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end
        obj_Page = this;


        if (!IsPostBack)
        {
            txtInvoiceDate.Text = DateTime.Now.ToString(strDateFormat);
            //Validation Msgs from Resource File
            rfvLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
          //  rfvBranch.ErrorMessage = ValidationMsgs.S3G_ValMsg_Branch;
            rfvVendorCode.ErrorMessage = ValidationMsgs.S3G_ValMsg_Vendor;

            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

            if (strMode == "")
            {
                FunPriBindLOBBranchVendor();
                FunGetInvoiceType();
            }

            if ((intInvoiceID > 0) && (strMode == "M"))
            {
                //FunGetInvoiceType();
                FunPriBindRefDocNo();
                FunPriDisableControls(1);
                FunGetInvoiceDetails();
                trTotal.Visible = false;
                if (ddlInvoiceType.SelectedValue == "2")
                {
                    //grvAssetDetails.Columns[7].HeaderText = "Input Tax Credit";
                    if (grvAssetDetails.HeaderRow != null)
                    {
                        grvAssetDetails.HeaderRow.Cells[7].Text = "Input Tax Credit";
                    }
                }
            }
            else if ((intInvoiceID > 0) && (strMode == "Q"))
            {
                //FunGetInvoiceType();
                FunGetInvoiceDetails();
                FunPriDisableControls(-1);
                if (ddlInvoiceType.SelectedValue == "2")
                {
                    //grvAssetDetails.Columns[7].HeaderText = "Input Tax Credit";
                    if (grvAssetDetails.HeaderRow != null)
                    {
                        grvAssetDetails.HeaderRow.Cells[7].Text = "Input Tax Credit";
                    }
                }

            }
            else
            {
                FunPriDisableControls(0);
                trTotal.Visible = false;
            }
            if (PageMode == PageModes.WorkFlow)
            {
                ViewState["PageMode"] = PageModes.WorkFlow;
            }
            if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())
            {
                try
                {
                    PreparePageForWFLoad();
                }
                catch (Exception ex)
                {
                      ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                    Utility.FunShowAlertMsg(this.Page, "Invalid data to load, access from menu");
                }
            }

        }
        //lnkDownload.Visible = false;

        if (rdoYes.Checked)
        {
            //fileScanImage.Enabled = true;
            rdoYes.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "javascript:fnDisableControls(1);", true);
        }
        else if (rdoNo.Checked)
        {
            //fileScanImage.Enabled = false;
            rdoNo.Checked = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "javascript:fnDisableControls(0);", true);
        }
        else
        {
            rdoYes.Checked = true;
        }

        if (hdnRadioFile.Value == "1")
            fileScanImage.Enabled = true;
        else
            fileScanImage.Enabled = false;

        if ((intInvoiceID > 0) && (strMode == "Q"))
        {
            fileScanImage.Enabled = false;

        }
        if ((IsPostBack) && (hdnFileName.Value != ""))
        {
            //FileInfo ajaxFileInfo = new FileInfo(hdnFileName.Value);
            lblDisplayFile.Text = hdnFileName.Value;
        }
        ddlInvoiceType.Enabled = false;
        //if (ddlInvoiceType.Items.Count>0)
        //{
        //ddlInvoiceType.SelectedIndex = 0;
        //}
        


        if (PageMode == PageModes.WorkFlow)
        {
            if (ViewState["intInvoiceID"] != null)
                intInvoiceID = Convert.ToInt32(ViewState["intInvoiceID"]);
        }

    }


    #endregion
    private void PreparePageForWFLoad1()
    {
        if (!IsPostBack)
        {
            /* WorkFlowSession WFSessionValues = new WorkFlowSession();
             // Get The IDVALUE from Document Sequence #
             //DataRow HeaderValues = GetHeaderDetailsFromDOCNo(WFSessionValues.PANUM, ProgramCode);

             FunPriBindLOBBranchVendor();

             //FunPriGetLOBBranchDropDownList(); // To Fill LOB
             //FunPriGetLookupLocationList();     // To Fill Location
             //FunPriGetCompanyInfo();   // To fill Company Info.

             ddlLOB.SelectedValue = HeaderValues["Lob_Id"].ToString();
             ddlBranch.SelectedValue = HeaderValues["Location_Id"].ToString();

             //FunPriGetMLADetails(int.Parse(ddlLOB.SelectedValue), int.Parse(ddlBranch.SelectedValue), intCompanyId);
             //ddlMLA.SelectedValue = HeaderValues["UniqueId"].ToString();
             //if (ddlMLA.SelectedIndex > 0)
             //{
             //    FunPriLoadMLADetails();
             //}


             if (ddlLOB.Items.Count > 0) ddlLOB.ClearDropDownList();
             if (ddlBranch.Items.Count > 0) ddlBranch.ClearDropDownList();
             //if (ddlMLA.Items.Count > 0) ddlMLA.ClearDropDownList();
             */
        }
    }


    private void PreparePageForWFLoad()
    {
        if (!IsPostBack)
        {
            WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowServiceClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

            WorkFlowSession WFSessionValues = new WorkFlowSession();
            byte[] byte_PreDisDoc = objWorkflowServiceClient.FunPubLoadInvoice(WFSessionValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValues.Document_Type, WFSessionValues.PANUM, WFSessionValues.SANUM);
            DataSet dsWorkflow = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_PreDisDoc, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));


            if (dsWorkflow.Tables.Count > 1)
            {
                if (dsWorkflow.Tables[1].Rows.Count > 0)
                {
                    intInvoiceID = Convert.ToInt32(dsWorkflow.Tables[1].Rows[0]["Doc_Id"].ToString());
                    ViewState["intInvoiceID"] = intInvoiceID;
                    //intProformaID = Convert.ToInt32(dsWorkflow.Tables[1].Rows[0]["Doc_Id"].ToString());
                    //ViewState["intProformaID"] = intProformaID;
                    hdnInvoiceID.Value = intInvoiceID.ToString();
                    //Similar like modify mode
                    FunPriBindRefDocNo();
                    FunPriDisableControls(1);
                    FunGetInvoiceDetails();
                    trTotal.Visible = false;
                    if (ddlInvoiceType.SelectedValue == "2")
                    {
                        //grvAssetDetails.Columns[7].HeaderText = "Input Tax Credit";
                        if (grvAssetDetails.HeaderRow != null)
                        {
                            grvAssetDetails.HeaderRow.Cells[7].Text = "Input Tax Credit";
                        }
                    }

                }
            }
            else
            {
                if (dsWorkflow.Tables[0].Rows.Count > 0)
                {
                    ddlLOB.SelectedValue = dsWorkflow.Tables[0].Rows[0]["LOB_ID"].ToString();
                    ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
                    ddlLOB.ClearDropDownList();

                    ddlBranch.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Location_ID"].ToString();
                    //ddlBranch.ClearDropDownList();

                    ddlRefDocType.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString();
                    FunPriBindRefDocNo();
                    ddlRefDocNo.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Document_Type_ID"].ToString();
                    FunPriSetRefDocNo();

                    ddlRefDocType.ClearDropDownList();
                    ddlRefDocNo.Enabled = false;
                    if (ddlSLA.Enabled)//if SLA Loaded/Applicable
                    {
                        if (ddlSLA.Items.Count > 0)
                            ddlSLA.ClearDropDownList();
                    }
                }

            }


        }
    }

    #region Page Events

    /// <summary>
    /// This is used to save Invoice details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void File_CheckedChanged(object sender, EventArgs e)
    {
        //if (rdoYes.Checked)
        //    //lblDisplayFile.Visible = true;
        //    lblDisplayFile.Attributes.Add("display", "block");
        //else
        //    //lblDisplayFile.Visible = false;
        //    lblDisplayFile.Attributes.Add("display", "none");
    }

    protected void ddlInvoiceType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriAssignVat();
        ddlInvoiceType.Enabled = true;
    }

    protected void ddlRefDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriClear(false, "DOCNO");
        FunPriSetRefDocNo();

    }

    private void FunPriSetRefDocNo()
    {
        if (ddlRefDocType.SelectedValue == "4")
        {
            FunBindSLA();
            //if (ddlSLA.Items[1].Text.Contains("DUMMY"))
            if (ddlSLA.Items.Count == 1)
            {
                rfvSLA.Enabled = false;
                PriBindVendor();
                //FunGetInvoiceDetails();
                //ddlSLA.Items.RemoveAt(1);
            }
            else
            {
                //ddlSLA.Items.RemoveAt(ddlSLA.Items 
                //ddlSLA.Items.RemoveAt(1);
                rfvSLA.Enabled = true;
            }
        }
        else
        {
            PriBindVendor();
            //FunGetInvoiceDetails();
        }
    }

    private void FunPriAssignVat()
    {

        if (ddlInvoiceType.SelectedValue == "2")
        {

            //grvAssetDetails.Columns[7].HeaderText = "Input Tax Credit";

            if (grvAssetDetails.HeaderRow != null)
            {
                grvAssetDetails.HeaderRow.Cells[7].Text = "Input Tax Credit";
            }
            foreach (GridViewRow grvRows in grvAssetDetails.Rows)
            {
                TextBox txtVAT = (TextBox)grvRows.FindControl("txtVAT");
                TextBox txtVATVal = (TextBox)grvRows.FindControl("txtVATVal");
                TextBox txtOthe = (TextBox)grvRows.FindControl("txtOthers");
                Label lblVATPer = (Label)grvRows.FindControl("lblVATPer");


                TextBox txtUnitValue = (TextBox)grvRows.FindControl("txtUnitValue");
                TextBox txtNoofUnit = (TextBox)grvRows.FindControl("txtNoofUnit");
                TextBox txtTax = (TextBox)grvRows.FindControl("txtTax");
                TextBox txtTaxVal = (TextBox)grvRows.FindControl("txtTaxVal");
                TextBox txtTotValue = (TextBox)grvRows.FindControl("txtTotValue");
                TextBox txtNetValue = (TextBox)grvRows.FindControl("txtNetValue");

                //txtUnitValue.Text = "";
                ////if (Utility.FunGetValueBySplit(ddlLOB.SelectedItem.Text) == "OL")
                ////{
                ////    txtNoofUnit.Text = "1";
                ////}
                //else
                //{
                //    txtNoofUnit.Text = "";
                //}

                if (Utility.FunGetValueBySplit(ddlLOB.SelectedItem.Text) == "OL")
                {
                    if ((txtUnitValue.Text != "") && (txtNoofUnit.Text != ""))
                    {
                        txtTotValue.Text = (Convert.ToDecimal(txtUnitValue.Text) * Convert.ToInt32(txtNoofUnit.Text)).ToString("0.00").ToString();
                    }
                    if ((lblVATPer.Text != "") && (txtTotValue.Text != ""))
                    {
                        txtVAT.Text = lblVATPer.Text;
                        txtOthe.Text = ((Convert.ToDecimal(txtTotValue.Text) * Convert.ToDecimal(txtVAT.Text)) / 100).ToString("0.00").ToString();
                        txtVATVal.Text = txtOthe.Text;
                        txtNetValue.Text = (Convert.ToDecimal(txtTotValue.Text) + Convert.ToDecimal(txtVATVal.Text)).ToString("0.00").ToString();
                    }

                    txtVAT.ReadOnly = true;
                    txtTax.ReadOnly = true;
                    txtOthe.SetDecimalPrefixSuffix(10, 2, true, "Input Tax Credit");
                }
                else
                {
                    txtTax.Text = "";
                    txtTaxVal.Text = "";
                    txtTotValue.Text = "";
                    txtNetValue.Text = "";
                    txtVAT.Text = lblVATPer.Text;
                    txtOthe.Text = txtVATVal.Text;
                    txtVAT.ReadOnly = true;
                    txtTax.ReadOnly = true;
                    txtOthe.SetDecimalPrefixSuffix(10, 2, true, "Input Tax Credit");
                }
            }
            FunChkVATInv();
        }
        else if (grvAssetDetails.HeaderRow != null)
        {
            grvAssetDetails.HeaderRow.Cells[7].Text = "Others";

            foreach (GridViewRow grvRows in grvAssetDetails.Rows)
            {
                TextBox txtVAT = (TextBox)grvRows.FindControl("txtVAT");
                TextBox txtVATVal = (TextBox)grvRows.FindControl("txtVATVal");
                TextBox txtOthe = (TextBox)grvRows.FindControl("txtOthers");
                TextBox txtUnitValue = (TextBox)grvRows.FindControl("txtUnitValue");
                TextBox txtNoofUnit = (TextBox)grvRows.FindControl("txtNoofUnit");
                TextBox txtTax = (TextBox)grvRows.FindControl("txtTax");
                TextBox txtTaxVal = (TextBox)grvRows.FindControl("txtTaxVal");
                TextBox txtTotValue = (TextBox)grvRows.FindControl("txtTotValue");
                TextBox txtNetValue = (TextBox)grvRows.FindControl("txtNetValue");


                ////txtUnitValue.Text = "";
                ////if (Utility.FunGetValueBySplit(ddlLOB.SelectedItem.Text) == "OL")
                ////{
                ////    txtNoofUnit.Text = "1";
                ////}
                //else
                //{
                //    txtNoofUnit.Text = "";
                //}
                txtTax.Text = "";
                txtTaxVal.Text = "";
                txtVAT.Text = "";
                txtVATVal.Text = "";
                txtOthe.Text = "";
                txtTotValue.Text = "";
                txtNetValue.Text = "";
                if (ddlInvoiceType.SelectedValue == "3")
                {
                    txtVAT.ReadOnly = true;
                }
                else
                {
                    txtVAT.ReadOnly = false;
                }
                if (Utility.FunGetValueBySplit(ddlLOB.SelectedItem.Text) == "OL")
                {
                    if ((txtUnitValue.Text != "") && (txtNoofUnit.Text != ""))
                    {
                        txtTotValue.Text = (Convert.ToDecimal(txtUnitValue.Text) * Convert.ToInt32(txtNoofUnit.Text)).ToString("0.00").ToString();
                    }
                }
                txtTax.ReadOnly = false;
                txtOthe.SetDecimalPrefixSuffix(10, 2, true, "Others");
            }
        }
    }

    private void PriBindVendor()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        Procparam.Add("@RefDoc_Type", ddlRefDocType.SelectedValue);
        Procparam.Add("@RefDoc_No", ddlRefDocNo.SelectedValue);
        //ddlRefDocNo will return account creation ID
        Procparam.Add("@SLA_No", ddlSLA.SelectedValue);//IF SLA No is 0 then there is no sub a/c
        DataTable dtVendor = new DataTable();
        dtVendor = Utility.GetDefaultData("S3G_LoanAd_GetInvoiceEntityDetails", Procparam);
        //ddlVendorCode.BindDataTable("S3G_LoanAd_GetInvoiceEntityDetails", Procparam, new string[] { "Entity_ID", "Entity_Code", "Entity_Name" });
        ddlVendorCode.FillDataTable(dtVendor, "Entity_ID", "Entity_Name");
        DataTable dtCustomer = Utility.GetDefaultData("S3G_LoanAd_GetInvoiceCustomerDetails", Procparam);
        if (dtCustomer.Rows.Count > 0)
        {
            hdnCustomerID.Value = dtCustomer.Rows[0]["Customer_ID"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(dtCustomer.Rows[0], true);
            //txtCustName.Text = dsAsset.Tables[1].Rows[0]["Customer_Name"].ToString();
        }

    }

    private void PriBindCustomer()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        Procparam.Add("@RefDoc_Type", ddlRefDocType.SelectedValue);
        Procparam.Add("@RefDoc_No", ddlRefDocNo.SelectedValue);
        //ddlRefDocNo will return account creation ID
        Procparam.Add("@SLA_No", hdnSLA.Value);//IF SLA No is 0 then there is no sub a/c
        DataTable dtCustomer = Utility.GetDefaultData("S3G_LoanAd_GetInvoiceCustomerDetails", Procparam);
        if (dtCustomer.Rows.Count > 0)
        {
            hdnCustomerID.Value = dtCustomer.Rows[0]["Customer_ID"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(dtCustomer.Rows[0], true);
            //txtCustName.Text = dsAsset.Tables[1].Rows[0]["Customer_Name"].ToString();
        }

    }

    private void FunBindSLA()
    {
        Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Type", "Type3");
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //Procparam.Add("@Branch_ID", ddlBranch.SelectedValue);
        //Procparam.Add("@Param2", ddlRefDocNo.SelectedItem.Text);

        Procparam.Add("@Type", "2");
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        Procparam.Add("@Location_Id", ddlBranch.SelectedValue);
        Procparam.Add("@PANum", ddlRefDocNo.SelectedText);
        Procparam.Add("@Is_Closed", "1");
        Procparam.Add("@ParamPA_Status", "6,7,26,45,55");
        Procparam.Add("@ParamSA_Status", "6,7,26,45,55");
        Procparam.Add("@Program_ID", "58");
        ddlSLA.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_AIE, Procparam, new string[] { "PA_SA_REF_ID", "SANum" });
    }
    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSLA.SelectedValue == "0")
        {
            FunPriClear(false, "DOCNO");
            return;
        }
        FunPriClear(false, "DOCNO");
        PriBindVendor();
        //FunGetInvoiceDetails();
        //hdnSLA.Value = ddlSLA.SelectedValue;

    }


    protected void ddlVendorCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlInvoiceType.Items.Clear();
            FunGetInvoiceType();
            ddlInvoiceType.Enabled = true;
            if (ddlVendorCode.Items.Count > 0)
            {
                FunPriBindVendorDet();
                FunGetInvoiceDetails();

                //if (ddlInvoiceType.SelectedValue == "2")
                //{

                //}
                FunPriAssignVat();


            }

            else
            {
                FunPriClearVendor();
            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        /// Added for LOB details 
        /// User Managements
        ///
        //ddlInvoiceType.Items.Clear();
        //Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //if (intInvoiceID == 0)
        //{
        //    Procparam.Add("@Is_Active", "1");
        //}
        //Procparam.Add("@User_ID", intUserID.ToString());
        //Procparam.Add("@Program_Code", "IVE");
        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });

        FunPriClear(true, "LOB");
        FunPriBindRefDocNo();
        if (ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.LastIndexOf("-")).Trim() == "OL")
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@OptionValue", "3");
            ddlRefDocType.BindDataTable("S3G_ORG_GetProformaLookup", Procparam, new string[] { "Value", "Name" });

            chkBookInAsset.Checked = true;

            //if (ddlRefDocType.Items.Count != 6)
            //{
            //    ddlRefDocType.Items.Insert(5, new ListItem("Purchase Order", "5"));
            //}

        }
        else
        {

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@OptionValue", "1");
            ddlRefDocType.BindDataTable("S3G_ORG_GetProformaLookup", Procparam, new string[] { "Value", "Name" });
            ddlRefDocType.SelectedIndex = 0;
            chkBookInAsset.Checked = false;

            if (ddlRefDocType.Items.FindByValue("5") != null)
            {
                ddlRefDocType.Items.Remove(ddlRefDocType.Items.FindByValue("5"));
                //ddlRefDocType.Items.RemoveAt(5);
            }
        }

        if ((ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.LastIndexOf("-")).Trim() == "OL") && (grvBookInAsset.Rows.Count > 0))
        {
            grvBookInAsset.Visible = true;
            //tdBookIn.Visible = true;
            //chkBookInAsset.Visible = true;
        }
        else
        {
            grvBookInAsset.Visible = false;
            //tdBookIn.Visible = false;
            //chkBookInAsset.Visible = false;
        }

    }

    private void FunPriBindRefDocNo()
    {
        //ddlRefDocNo.Items.Clear();
        //Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        //Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
        //Procparam.Add("@OptionValue", ddlRefDocType.SelectedValue);
        //if (ddlRefDocType.SelectedValue == "4")
        //{
        //    Procparam = new Dictionary<string, string>();
        //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //    Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        //    Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
        //    Procparam.Add("@Type", "1");
        //    Procparam.Add("@Is_Closed", "1");
        //    Procparam.Add("@ParamPA_Status", "6,7,26,45,55");
        //    Procparam.Add("@ParamSA_Status", "6,7,26,45,55");
        //    Procparam.Add("@Program_ID", "58");
        //    ddlRefDocNo.BindDataTable("S3G_GetPAN_INV", Procparam, new string[] { "Account_Creation_ID", "PANum" });
        //}
        //else
        //{
        //    ddlRefDocNo.BindDataTable(SPNames.S3G_ORG_GetProformaRefDocNo, Procparam, new string[] { "RefDoc_ID", "RefDoc_No" });
        //}
        //ddlSLA.Items.Clear();

    }

    // changed by bhuvana for performance on September 16th 2013//
    [System.Web.Services.WebMethod]
    public static string[] GetDocumentNumber(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@OptionValue", obj_Page.ddlRefDocType.SelectedValue);
        Procparam.Add("@Prefix", prefixText);
        if (obj_Page.ddlRefDocType.SelectedValue == "4")
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
            Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
            Procparam.Add("@Type", "1");
            Procparam.Add("@Is_Closed", "1");
            Procparam.Add("@ParamPA_Status", "6,7,26,45,55");
            Procparam.Add("@ParamSA_Status", "6,7,26,45,55");
            Procparam.Add("@Program_ID", "58");
            Procparam.Add("@Prefix", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GetPAN_INV_AGT", Procparam));
        }
        else
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetProformaRefDocNo_AGT", Procparam));
        }

        

        

        return suggetions.ToArray();

    }
    //end here//

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriClear(false, "Branch");
        //FunPriBindRefDocNo();
    }

    protected void ddlRefDocType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClear(false, "DOCTYPE");
            ddlRefDocNo.Clear();
            FunPriBindRefDocNo();
            if (ddlRefDocType.SelectedValue == "4")
            {
                ddlSLA.Enabled = true;
            }
            else
            {
                ddlSLA.Enabled = false;
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // To Check VAT Invoice available in Tax Guide Master
            //FunChkVATInv();
            ddlInvoiceType.Enabled = true;
            bool bVATRequired = false;
            bool blnWarrantyReqVal = true;
            bool blnCurrentRows = false;
            StringBuilder strbRows = new StringBuilder();
            lnkDownload.Enabled = false;
            AjaxControlToolkit.AsyncFileUpload fileScanImage = (AjaxControlToolkit.AsyncFileUpload)tbInvoice.FindControl("fileScanImage");
            // To Check VAT Invoice available in Tax Guide Master
            if (ddlInvoiceType.SelectedValue == "2")
            {
                FunChkVATInv();
                //return;
            }
            if ((rdoYes.Checked) && (!FunFileLoad(false)))
            {
                return;
            }
            if (grvAssetDetails.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Asset " + ValidationMsgs.S3G_ValMsg_NotExist + " for the selected Reference Document Number");
                return;
            }
            bool bReqVal = true;
            bool bReqRows = true;


            TextBox txtTAX = null;
            TextBox txtVAT = null;
            TextBox txtUnitValue = null;
            TextBox txtNoofUnit = null;
            TextBox txtOthers = null;


            foreach (GridViewRow grvData in grvAssetDetails.Rows)
            {
                blnCurrentRows = false;
                txtTAX = (TextBox)grvData.FindControl("txtTax");
                txtVAT = (TextBox)grvData.FindControl("txtVAT");
                txtUnitValue = (TextBox)grvData.FindControl("txtUnitValue");
                txtNoofUnit = (TextBox)grvData.FindControl("txtNoofUnit");
                txtOthers = (TextBox)grvData.FindControl("txtOthers");

                if (Utility.FunGetValueBySplit(ddlLOB.SelectedItem.Text) != "OL")
                {
                    if (txtTAX.Text != "" || txtVAT.Text != "" || txtUnitValue.Text != "" || txtNoofUnit.Text != "" || txtOthers.Text != "")
                    {
                        bReqRows = false;
                        blnCurrentRows = true;
                    }
                }
                else if (ddlInvoiceType.SelectedValue != "2" && ((txtTAX.Text != "" || txtVAT.Text != "" || txtUnitValue.Text != "" || txtOthers.Text != "")))
                {
                    bReqRows = false;
                    blnCurrentRows = true;
                }
                else if (ddlInvoiceType.SelectedValue == "2" && ((txtTAX.Text != "" || txtUnitValue.Text != "" || txtOthers.Text != "")))
                {
                    bReqRows = false;
                    blnCurrentRows = true;
                }

                if (blnCurrentRows) //If there is any row then bReqRows will be false
                {
                    if (txtUnitValue.Text == "")
                    {
                        rfvUnitValue.IsValid = false;
                        bReqVal = false;
                    }
                    if (txtNoofUnit.Text == "")
                    {
                        rfvNoofUnit.IsValid = false;
                        bReqVal = false;
                    }
                    ////if (txtOthers.Text == "")
                    ////{
                    ////    rfvOthers.IsValid = false;
                    ////    bReqVal = false;
                    ////}

                    if ((txtTAX.Text == "") && (txtVAT.Text == "") && (ddlInvoiceType.SelectedValue != "2"))
                    {
                        rfvTax.IsValid = false;
                        bReqVal = false;
                        //Utility.FunShowAlertMsg(this, "Either Tax % or VAT % should be entered");
                        // args.IsValid = false;
                    }

                    if (txtVAT.Text != "")
                    {
                        bVATRequired = true;
                    }

                    if ((ddlInvoiceType.SelectedValue == "2") && (txtVAT.Text == ""))
                    {
                        rfvVAT.ErrorMessage = "Enter VAT % in Asset Details";
                        rfvVAT.IsValid = false;
                        return;
                    }
                }
                if (bReqVal == false)
                {
                    break;
                }
                if ((txtTAX.Text != "" || txtVAT.Text != "") && (txtUnitValue.Text != "" && txtNoofUnit.Text != ""))
                {
                    strbRows = strbRows.Append("'" + grvData.RowIndex.ToString() + "'");
                }
            }


            ////foreach (GridViewRow grvData in grvAssetDetails.Rows)
            ////{
            ////    txtTAX = (TextBox)grvData.FindControl("txtTax");
            ////    txtVAT = (TextBox)grvData.FindControl("txtVAT");
            ////    txtUnitValue = (TextBox)grvData.FindControl("txtUnitValue");
            ////    txtNoofUnit = (TextBox)grvData.FindControl("txtNoofUnit");
            ////    txtOthers = (TextBox)grvData.FindControl("txtOthers");
            ////}

            foreach (GridViewRow grvData in grvWarranty.Rows)
            {
                DropDownList ddlWarrantyType = (DropDownList)grvData.FindControl("ddlWarrantyType");
                DropDownList ddlServiceFrequency = (DropDownList)grvData.FindControl("ddlServiceFrequency");
                TextBox txtFromDate = (TextBox)grvData.FindControl("txtFromDate");
                TextBox txtToDate = (TextBox)grvData.FindControl("txtToDate");
                TextBox txtRemarks = (TextBox)grvData.FindControl("txtRemarks");

                if ((ddlWarrantyType.SelectedValue != "0") || (txtFromDate.Text != "") || (txtToDate.Text != "") || (ddlServiceFrequency.SelectedValue != "0") || (txtRemarks.Text.Trim() != ""))
                {
                    if (ddlWarrantyType.SelectedValue == "0")
                    {
                        rfvWarrantyType.IsValid = false;
                        blnWarrantyReqVal = false;
                    }
                    if (txtFromDate.Text == "")
                    {
                        rfvFromDate.IsValid = false;
                        blnWarrantyReqVal = false;
                    }
                    if (txtToDate.Text == "")
                    {
                        rfvToDate.IsValid = false;
                        blnWarrantyReqVal = false;
                    }
                    if (ddlServiceFrequency.SelectedValue == "0")
                    {
                        rfvServiceFrequency.IsValid = false;
                        blnWarrantyReqVal = false;
                    }
                    //if (txtRemarks.Text == "")
                    //{
                    //    rfvRemarks.IsValid = false;
                    //    bReqVal=false;
                    //}
                    if ((txtFromDate.Text != "") && (txtToDate.Text != ""))
                    {

                        if (!FunPriValidateFromEndDate(txtInvoiceDate.Text, txtFromDate.Text))
                        {
                            rfvCompareWarrantyDate.ErrorMessage = "Warranty From Date should be greater than or equal to the Invoice Date";
                            rfvCompareWarrantyDate.IsValid = false;
                            blnWarrantyReqVal = false;
                            //Utility.FunShowAlertMsg(this, ValidationMsgs.S3G_ValMsg_DateCompare+" in Warranty Details");

                        }

                        if (!FunPriValidateFromEndDate(txtFromDate.Text, txtToDate.Text))
                        {
                            rfvCompareDate.ErrorMessage = "From Date should be lesser than or equal to the To Date in Warranty Details";
                            rfvCompareDate.IsValid = false;
                            blnWarrantyReqVal = false;
                            //Utility.FunShowAlertMsg(this, ValidationMsgs.S3G_ValMsg_DateCompare+" in Warranty Details");

                        }
                    }
                }


                if ((ddlWarrantyType.SelectedValue != "0") && (txtFromDate.Text != "") && (txtToDate.Text != "") && (ddlServiceFrequency.SelectedValue != "0"))
                {
                    if (!(strbRows.ToString().Contains("'" + grvData.RowIndex.ToString() + "'")))
                    {
                        rfvAssetWarrantyCompare.IsValid = false;
                        blnWarrantyReqVal = false;
                    }
                }
            }

            if (bReqRows)
            {
                rfvRow.IsValid = false;
                return; //if there is no row then no need to go for next line.
            }



            if ((bVATRequired) && (txtVATNo.Text == ""))
            {
                rfvVAT.ErrorMessage = "Enter VAT Number if VAT % exists in Asset Details";
                rfvVAT.IsValid = false;
                return;
            }
            //if ((!bVATRequired) && (txtVATNo.Text != ""))
            //{
            //    rfvVAT.ErrorMessage = "Enter VAT % in Asset Details if VAT Number Exists";
            //    rfvVAT.IsValid = false;
            //    return;
            //}

            if ((bReqVal == false) || (blnWarrantyReqVal == false))
            {
                return;
            }

            ObjS3G_LOANAD_InvoiceVendorDetailsDataTable = new LoanAdminMgtServices.S3G_LOANAD_InvoiceVendorDetailsDataTable();
            LoanAdminMgtServices.S3G_LOANAD_InvoiceVendorDetailsRow ObjInvoiceRow;
            ObjInvoiceRow = ObjS3G_LOANAD_InvoiceVendorDetailsDataTable.NewS3G_LOANAD_InvoiceVendorDetailsRow();
            ObjInvoiceRow.Invoice_ID = intInvoiceID;
            ObjInvoiceRow.Company_ID = intCompanyID;
            ObjInvoiceRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjInvoiceRow.Customer_Code = hdnCustomerID.Value == "" ? 0 : Convert.ToInt32(hdnCustomerID.Value);//hdnCustomerID.Value;
            ObjInvoiceRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjInvoiceRow.Vendor_ID = Convert.ToInt32(ddlVendorCode.SelectedValue);
            ObjInvoiceRow.Ref_Docu_Type = Convert.ToInt32(ddlRefDocType.SelectedValue);
            ObjInvoiceRow.Invoice_Type = Convert.ToInt32(ddlInvoiceType.SelectedValue);

            if ((ddlRefDocType.SelectedValue == "4"))
            {
                ObjInvoiceRow.Ref_Docu_No = Convert.ToInt32(ddlRefDocNo.SelectedValue);
                ObjInvoiceRow.Ref_Doc_SLA = ddlSLA.SelectedValue;
            }
            else
            {
                ObjInvoiceRow.Ref_Docu_No = Convert.ToInt32(ddlRefDocNo.SelectedValue);
                ObjInvoiceRow.Ref_Doc_SLA = "0";
            }

            ObjInvoiceRow.IsBookin_Asset = chkBookInAsset.Checked;
            ObjInvoiceRow.TaxReg_No = txtTaxRegNo.Text;
            ObjInvoiceRow.Vat_No = txtVATNo.Text.Trim();
            ObjInvoiceRow.Vendor_Invoice_No = txtInvoiceNo.Text.Trim();
            ObjInvoiceRow.Vendor_Invoice_Date = Utility.StringToDate(txtInvoiceDate.Text);

            //ObjInvoiceRow.Invoice_Image =

            if (rdoYes.Checked)
            {
                ObjInvoiceRow.Invoice_Image = fileScanImage.FileName;
                hdnFile.Value = fileScanImage.FileName;
            }
            else
            {
                ObjInvoiceRow.Invoice_Image = "";
            }
            ObjInvoiceRow.Created_By = intUserID;
            //fileScanImage.PostedFile.SaveAs(strFileName);

            string strAssetDetails = grvAssetDetails.FunPubFormXml().Replace("%", "").Replace("No.ofUnits", "NoofUnits");
            strAssetDetails = strAssetDetails.Replace("VAT=''", "VAT='0'").Replace("Tax=''", "Tax='0'");
            strAssetDetails = strAssetDetails.Replace("VATValue=''", "VATValue='0'").Replace("TaxValue=''", "TaxValue='0'");
            strAssetDetails = strAssetDetails.Replace("UnitValue=''", "UnitValue='0'").Replace("NoofUnits=''", "NoofUnits='0'");
            strAssetDetails = strAssetDetails.Replace("TotalValue=''", "TotalValue='0'").Replace("GrossAmount=''", "GrossAmount='0'");
            ObjInvoiceRow.XMLAssetDetails = strAssetDetails.Replace("Others=''", "Others='0'");
            ObjInvoiceRow.XMLWarrantyDetails = grvWarranty.FunPubFormXml().Replace("%", "");

            ObjS3G_LOANAD_InvoiceVendorDetailsDataTable.AddS3G_LOANAD_InvoiceVendorDetailsRow(ObjInvoiceRow);

            //This is used to manage system journal entry
            //if (ddlRefDocType.SelectedValue == "3" || ddlRefDocType.SelectedValue == "4")
            //{
            FunPriSysJournalEntry();
            //}
            //Code end
            string strErrMsg = string.Empty;
            ObjInvoiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
            intErrCode = ObjInvoiceClient.FunPubCreateInvoiceDetails(out strErrMsg, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_InvoiceVendorDetailsDataTable, SerMode), ObjSysJournal);

            if (intErrCode == 0)
            {
                if (rdoYes.Checked)
                {
                    FunFileLoad(true);
                    lnkDownload.Enabled = true;
                }
                if (intInvoiceID > 0)
                {
                    //Utility.FunShowAlertMsg(this.Page, "Invoice " + ValidationMsgs.S3G_ValMsg_Update, strRedirectPage);
                    //return;
                    strInvoiceNo = txtInvoiceNo.Text;
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strInvoiceNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }
                        ShowWFAlertMessage(strInvoiceNo, ProgramCode, strAlert);
                        return;
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this.Page, "Invoice " + ValidationMsgs.S3G_ValMsg_Update, strRedirectPage);
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                        return;
                    }
                }
                else
                {
                    //FunFileLoad(true);
                    //fileScanImage.FileName

                    //FunGetInvoiceDetails();
                    if (isWorkFlowTraveler)
                    {
                        WorkFlowSession WFValues = new WorkFlowSession();
                        try
                        {
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, strInvoiceNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.PANUM, WFValues.SANUM, WFValues.ProductId);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            strAlert = "";
                        }
                        catch (Exception ex)
                        {
                            strAlert = "Work Flow is not assigned";
                        }
                        ShowWFAlertMessage(strInvoiceNo, ProgramCode, strAlert);
                        return;
                    }
                    else
                    {
                        // FORCE PULL IMPLEMENTATION KR
                        DataTable WFFP = new DataTable();
                        if (CheckForForcePullOperation(ProgramCode, null, null, "L", CompanyId, out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), null, int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), null, "", int.Parse(dtrForce["PRODUCTID"].ToString()));
                                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                                btnSave.Enabled = false;
                                //END
                            }
                            catch
                            {
                                strAlert = "Work Flow is not assigned";
                            }
                        }
                        strAlert = "Invoice details " + strErrMsg + " added successfully";
                        strAlert += @"\n" + ValidationMsgs.S3G_ValMsg_Next + " Invoice?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                        btnSave.Enabled = false;
                        //END
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                        return;
                    }
                    //strAlert = "Invoice details " + strErrMsg + " added successfully";
                    //strAlert += @"\n" + ValidationMsgs.S3G_ValMsg_Next + " Invoice?";
                    //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    //strRedirectPageView = "";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    //lblErrorMessage.Text = string.Empty;
                    //return;

                }
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Invoice " + ValidationMsgs.S3G_ValMsg_Exist);
                return;
            }
            else if (intErrCode == 6)
            {
                Utility.FunShowAlertMsg(this.Page, "Number of units cannot be greater than actual units - " + strErrMsg);
                return;
            }
            else if (intErrCode == 11)
            {
                if (ddlRefDocType.SelectedValue == "4")
                {
                    Utility.FunShowAlertMsg(this.Page, ValidationMsgs.LOANAD_INV_11 + "Account");
                }
                else if (ddlRefDocType.SelectedValue == "3")
                {
                    Utility.FunShowAlertMsg(this.Page, ValidationMsgs.LOANAD_INV_11 + "Application");
                }
                else if (ddlRefDocType.SelectedValue == "2")
                {
                    Utility.FunShowAlertMsg(this.Page, ValidationMsgs.LOANAD_INV_11 + "Pricing");
                }
                else if (ddlRefDocType.SelectedValue == "1")
                {
                    Utility.FunShowAlertMsg(this.Page, ValidationMsgs.LOANAD_INV_11 + "Enquiry");
                }


                return;
            }
            else
            {
                if ((intErrCode == -1) || (intErrCode == -2) || (intErrCode == 50))
                    Utility.FunShowValidationMsg(this.Page, "", intErrCode);
                else
                    Utility.FunShowValidationMsg(this.Page, "LOANAD_INV", intErrCode, strErrMsg.Trim(), false);
                return;
            }
            //else if (intErrCode == 2)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create Invoice Code");
            //}


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
            if (ObjInvoiceClient != null)
                ObjInvoiceClient.Close();
        }
    }




    /// <summary>
    /// This is used to redirect page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else if (Request.QueryString["IsFromAccount"] != null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
        }
        else
        {
            Response.Redirect(strRedirectPage,false);
        }
    }

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {//string strFileName = Server.MapPath(".").ToString().Replace("\\Origination","") + @"\Data\Invoice\" + hdnFile.Value;

            string strFilePath = "";
            //string strFileName = fileScanImage.FileName;
            //string strLoanAdminPath = Server.MapPath(".");
            //string strFilePath = strLoanAdminPath.Replace("\\LoanAdmin", "") + "\\Data\\Invoice";
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Program_ID", "58");
            DataTable dtPath = Utility.GetDefaultData("S3G_LOANAD_GetDocPath", Procparam);
            if (dtPath != null && dtPath.Rows.Count > 0)
            {
                strFilePath = dtPath.Rows[0]["DOCUMENT_PATH"].ToString();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "The File not Exist");
                return;
            }
            string strFileName = strFilePath + "/" + hdnFile.Value;

            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            //string strScipt = "window.open('.." + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

            //Response.Clear();
            ////Response.AppendHeader("content-disposition", "attachment; filename=" + strFileName);
            //Response.AppendHeader("content-disposition", "attachment; filename=" + strFilePath + "/" + strFileName );
            //Response.ContentType = "application/octet-stream";
            //Response.WriteFile(strFilePath + "/" + strFileName);
            //Response.End();
        }
        catch (Exception ex)
        {
            throw ex;
        }












        ////// set the http content type to "LoanAdmin/OCTET-STREAM
        ////Response.ContentType = "LoanAdmin/OCTET-STREAM";

        ////// initialize the http content-disposition header to
        ////// indicate a file attachment with the default filename
        ////// "myFile.txt"
        ////System.String disHeader = "Attachment; Filename=" + strFileName;

        ////Response.AppendHeader("Content-Disposition", disHeader);

        ////// transfer the file byte-by-byte to the response object
        ////System.IO.FileInfo fileToDownload = new
        ////   System.IO.FileInfo(strFileName);
        ////Response.Flush();
        ////Response.WriteFile(fileToDownload.FullName);

        //Utility.FunShowAlertMsg(this.Page, "Success");
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
            ddlBranch.Clear();
            FunPriClear(true, "CLEAR");
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvBookInAsset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow) && (intInvoiceID > 0))
        {
            //DataBinder.Eval(e.Row.DataItem, "NetAmount");
            Label lblNetAmt = (Label)e.Row.FindControl("lblNetAmt");
            if (lblNetAmt.Text != "")
                lblNetAmt.Text = Convert.ToDecimal(lblNetAmt.Text).ToString("0.00").ToString();
        }
    }

    protected void grvAssetDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtUnitValue = (TextBox)e.Row.FindControl("txtUnitValue");
            TextBox txtNoofUnit = (TextBox)e.Row.FindControl("txtNoofUnit");
            TextBox txtTax = (TextBox)e.Row.FindControl("txtTax");
            TextBox txtVAT = (TextBox)e.Row.FindControl("txtVAT");
            TextBox txtOthers = (TextBox)e.Row.FindControl("txtOthers");
            TextBox txtVATVal = (TextBox)e.Row.FindControl("txtVATVal");
            TextBox txtTaxVal = (TextBox)e.Row.FindControl("txtTaxVal");
            TextBox txtTotValue = (TextBox)e.Row.FindControl("txtTotValue");
            TextBox txtNetValue = (TextBox)e.Row.FindControl("txtNetValue");
            Label lblVATPer = (Label)e.Row.FindControl("lblVATPer");

            txtUnitValue.Attributes.Add("onKeyDown", "return fnCheckKeyPress();");
            txtTax.Attributes.Add("onKeyDown", "return fnCheckKeyPress();");
            txtVAT.Attributes.Add("onKeyDown", "return fnCheckKeyPress();");
            txtNoofUnit.Attributes.Add("onKeyDown", "return fnCheckKeyPress();");
            txtOthers.Attributes.Add("onKeyDown", "return fnCheckKeyPress();");

            //txtUnitValue.SetDecimalPrefixSuffix(10, 2, true, "Unit value");
            //txtTax.SetDecimalPrefixSuffix(2, 2, true, "Tax %");
            //txtVAT.SetDecimalPrefixSuffix(2, 2, true, "VAT %");

            txtUnitValue.SetPercentagePrefixSuffix(10, 2, true, true, "Unit value");
            txtTax.SetPercentagePrefixSuffix(2, 2, true, "Tax %");
            txtVAT.SetPercentagePrefixSuffix(2, 2, true, "VAT %");
            txtNoofUnit.CheckGPSLength(true, "Number of units");

            if (ddlInvoiceType.SelectedValue == "2")
            {
                txtOthers.SetDecimalPrefixSuffix(10, 2, true, "Input Tax Credit");
            }
            else
            {
                txtOthers.SetDecimalPrefixSuffix(10, 2, true, "Others");
            }

            if (intInvoiceID > 0)
            {
                //txtTaxVal.Text = Convert.ToDecimal(txtTaxVal.Text).ToString("0.00").ToString();
                //txtVATVal.Text = Convert.ToDecimal(txtVATVal.Text).ToString("0.00").ToString();
                //txtNetValue.Text = Convert.ToDecimal(txtNetValue.Text).ToString("0.00").ToString();

                if (txtTaxVal.Text != "")
                    txtTaxVal.Text = Convert.ToDecimal(txtTaxVal.Text).ToString("0.00").ToString();
                if (txtVATVal.Text != "")
                    txtVATVal.Text = Convert.ToDecimal(txtVATVal.Text).ToString("0.00").ToString();
                if (txtOthers.Text != "")
                    txtOthers.Text = Convert.ToDecimal(txtOthers.Text).ToString("0.00").ToString();

                //if (txtUnitValue.Text != "")
                //    txtUnitValue.Text = Convert.ToDecimal(txtUnitValue.Text).ToString("0.00").ToString();
                //if (txtNoofUnit.Text != "")
                //    txtNoofUnit.Text = Convert.ToDecimal(txtOthers.Text).ToString("0.00").ToString();
                //if (txtOthers.Text != "")
                //    txtOthers.Text = Convert.ToDecimal(txtOthers.Text).ToString("0.00").ToString();
                if (txtNetValue.Text != "")
                {
                    txtNetValue.Text = Convert.ToDecimal(txtNetValue.Text).ToString("0.00").ToString();
                }

                if ((ddlInvoiceType.SelectedValue == "2"))
                {
                    txtVAT.ReadOnly = true;

                    if (txtUnitValue.Text != "")
                    {
                        txtOthers.ReadOnly = true;
                        txtTax.ReadOnly = true;
                        txtNoofUnit.ReadOnly = true;
                        txtUnitValue.ReadOnly = true;

                    }
                    else
                    {
                        txtVAT.Text = lblVATPer.Text;
                    }
                }
            }
            string strValue = "fnCalculateTax("
                + "this"
                + ", '" + txtUnitValue.ClientID + "'"
                + ", '" + txtNoofUnit.ClientID + "'"
                + ", '" + txtTax.ClientID + "'"
                + ", '" + txtVAT.ClientID + "'"
                + ", '" + txtOthers.ClientID + "'"
                + ", '" + txtVATVal.ClientID + "'"
                + ", '" + txtTaxVal.ClientID + "'"
                + ", '" + txtTotValue.ClientID + "'"
                + ", '" + txtNetValue.ClientID + "')";

            txtUnitValue.Attributes.Add("onchange", strValue);
            txtNoofUnit.Attributes.Add("onchange", strValue);
            txtTax.Attributes.Add("onchange", strValue);
            txtVAT.Attributes.Add("onchange", strValue);
            txtOthers.Attributes.Add("onchange", strValue);

            //ddlYes.Style.Add("display", "none");
            //TextBox txtbReqVal = (TextBox)e.Row.FindControl("txtbReqValue1");
            //strbReqValue += ",'" + ddlYes.ClientID + "','" + txtbReqVal.ClientID + "'";
            //ddlField.Attributes.Add("onchange", "fnChangeAttribute('" + ddlField.ClientID + "','" + ddlNumeric.ClientID + "'" + strbReqValue + ")");

            //After adding new row the following conditions should work

            dScore += txtNetValue.Text != "" ? Convert.ToDecimal(txtNetValue.Text) : 0;

            //if (ddlLOB.SelectedValue == "3")
            //////if (ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.LastIndexOf("-")).Trim() == "OL")
            //////{
            //////    //----------------Changed by Gunasekar on 30-11-2010
            //////    //----------------Unit value should be editable but No of units should be always one for Operating Lease LOB
            //////    ////txtUnitValue.ReadOnly = true;
            //////    ////txtUnitValue.Text = "1";
            //////    txtNoofUnit.ReadOnly = true;
            //////    txtNoofUnit.Text = "1";
            //////    //----------------Ends Here
            //////}

        }

        txtTotalAmt.Text = dScore.ToString();


    }

    protected void grvWarranty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlFreq = (DropDownList)e.Row.FindControl("ddlServiceFrequency");
            ddlFreq.DataTextField = dtFrequency.Columns["Name"].ToString();
            ddlFreq.DataValueField = dtFrequency.Columns["Value"].ToString();
            ddlFreq.DataSource = dtFrequency.DefaultView;
            ddlFreq.DataBind();
            ddlFreq.Items.Insert(0, (new ListItem("--Select--", "0")));

            TextBox txtFromDate = (TextBox)e.Row.FindControl("txtFromDate");
            TextBox txtToDate = (TextBox)e.Row.FindControl("txtToDate");
            
            
            if (intInvoiceID > 0)
            {
                DropDownList ddlWarrantyType = (DropDownList)e.Row.FindControl("ddlWarrantyType");

                ddlFreq.SelectedValue = dsAsset.Tables[0].Rows[iCount]["Service_Frequency"].ToString();
                ddlWarrantyType.SelectedValue = dsAsset.Tables[0].Rows[iCount]["Warranty_Type"].ToString();
                if (dsAsset.Tables[0].Rows[iCount]["Warranty_From_Date"].ToString() != "")
                {
                    txtFromDate.Text = DateTime.Parse(dsAsset.Tables[0].Rows[iCount]["Warranty_From_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    txtToDate.Text = DateTime.Parse(dsAsset.Tables[0].Rows[iCount]["Warranty_To_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                //txtFromDate.Text = dsAsset.Tables[1].Rowsi[Count]["Warranty_From_Date"].ToString();
                //txtToDate.Text = dsAsset.Tables[1].Rows[iCount]["Warranty_To_Date"].ToString();
            }

            //txtFromDate.Attributes.Add("readonly", "readonly");
            AjaxControlToolkit.CalendarExtender ceFromDate = e.Row.FindControl("ceFromDate") as AjaxControlToolkit.CalendarExtender;
            ceFromDate.Format = strDateFormat;

            //txtToDate.Attributes.Add("readonly", "readonly");
            AjaxControlToolkit.CalendarExtender ceToDate = e.Row.FindControl("ceToDate") as AjaxControlToolkit.CalendarExtender;
            ceToDate.Format = strDateFormat;
            if (PageMode != PageModes.Query)
            {
                txtFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
                txtToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtToDate.ClientID + "','" + strDateFormat + "',false,  false);");
            }
            else
            {
                txtFromDate.ReadOnly = true;
                txtFromDate.Attributes.Remove("onblur");
                txtToDate.ReadOnly = true;
                txtToDate.Attributes.Remove("onblur");
            }
            iCount++;
        }
    }

    #endregion

    #region Page Methods

    /// <summary>
    /// to bind LOB and Product details
    /// </summary>

    private void FunPriBindLOBBranchVendor()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        if (intInvoiceID == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }
        Procparam.Add("@FilterType", "0");
        Procparam.Add("@FilterCode", "'TL','TE','FT','WC'");
        Procparam.Add("@User_ID", intUserID.ToString());
        Procparam.Add("@Program_Code", "IVE");
        ddlLOB.BindDataTable(SPNames.S3G_ORG_GetSpecificLOBList, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        if (strMode != "C")
        {
            //Branch
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //if (intInvoiceID == 0)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            //Procparam.Add("@User_ID", intUserID.ToString());
            //Procparam.Add("@Program_Code", "IVE");
            //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
        }

        //Vendor
        //Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //Procparam.Add("@Entity_Type", "'6','8'");
        //ddlVendorCode.BindDataTable(SPNames.S3G_ORG_GetEntityMasterVendor, Procparam, new string[] { "Entity_Master_ID", "Entity_Code", "Entity_Name" });

        //RefDocType
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@OptionValue", "1");
        ddlRefDocType.BindDataTable("S3G_ORG_GetProformaLookup", Procparam, new string[] { "Value", "Name" });
    }

    // Created By R. Manikandan to be loaded after Vendor Loading 
    // To fix UAT Bug: IVE_027
    private void FunGetInvoiceType()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@LookupType_Code", "88");
        ddlInvoiceType.BindDataTable("S3G_LOANAD_GetLookupTypeDescription", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
    }
    private void FunChkVATInv()
    {
        try
        {
            StringBuilder strXMLAsset = null;
            strXMLAsset = new StringBuilder();
            strXMLAsset.Append("<Root>");
            foreach (GridViewRow grvRow in grvAssetDetails.Rows)
            {
                Label lblAssetID = (Label)grvRow.FindControl("lblAssetID");
                strXMLAsset.Append("<Details Asset_ID ='" + lblAssetID.Text + "'/>");
            }
            strXMLAsset.Append("</Root>");

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            Procparam.Add("@xmlAsset", strXMLAsset.ToString());
            DataTable dtVatChk = Utility.GetDefaultData("S3G_LOANAD_ChkVATInv", Procparam);
            if (dtVatChk == null || (dtVatChk.Rows.Count <= 0))
            {
                Utility.FunShowAlertMsg(this, "VAT % not defined in Tax Guide Master");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //private void FunLoadAssets()
    //{
    //    strProcName = "S3G_LOANAD_GetInvoiceAsset_List";
    //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //    Procparam.Add("@RefDoc_Type", ddlRefDocType.SelectedValue);
    //    Procparam.Add("@RefDoc_No", ddlRefDocNo.SelectedValue);
    //    Procparam.Add("@SLA_No", ddlSLA.SelectedValue);
    //    Procparam.Add("@Entity_ID", ddlVendorCode.SelectedValue);

    //}

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetInvoiceDetails()
    {

        try
        {
            strProcName = "S3G_LoanAd_GetInvoiceAssetDetails";
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Invoice_ID", intInvoiceID.ToString());
            Procparam.Add("@RefDoc_Type", ddlRefDocType.SelectedValue);
            Procparam.Add("@SLA_No", ddlSLA.SelectedValue);
            Procparam.Add("@RefDoc_No", ddlRefDocNo.SelectedValue);
            Procparam.Add("@Entity_ID", ddlVendorCode.SelectedValue);
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);

            dsAsset = Utility.GetTableValues(strProcName, Procparam);

            //Changed by Thangam as Tables[1] --> Tables[0] on 15/Nov/2013
            if (dsAsset.Tables[0].Rows.Count > 0)
            {
                if ((intInvoiceID > 0) || (ddlRefDocNo.SelectedValue != "0") || ViewState["PageMode"] != null)//For Workflow
                {
                    if (intInvoiceID > 0)
                    {
                        ddlLOB.Items.Add(new ListItem(dsAsset.Tables[1].Rows[0]["LOB_Name"].ToString(), dsAsset.Tables[1].Rows[0]["LOB_ID"].ToString()));
                        ddlLOB.ToolTip = dsAsset.Tables[1].Rows[0]["LOB_Name"].ToString();
                        ddlInvoiceType.Items.Add(new ListItem(dsAsset.Tables[1].Rows[0]["Invoice_Type_Desc"].ToString(), dsAsset.Tables[1].Rows[0]["Invoice_Type"].ToString()));
                        ddlInvoiceType.ToolTip = dsAsset.Tables[1].Rows[0]["Invoice_Type_Desc"].ToString();
                        //   ddlInvoiceType.SelectedValue = dsAsset.Tables[1].Rows[0]["Invoice_Type"].ToString();
                    }
                    grvBookInAsset.DataSource = dsAsset.Tables[0];
                    grvBookInAsset.DataBind();

                    grvAssetDetails.DataSource = dsAsset.Tables[0];
                    grvAssetDetails.DataBind();

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@OptionValue", "2");
                    dtFrequency = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", Procparam);

                    grvWarranty.DataSource = dsAsset.Tables[0];
                    grvWarranty.DataBind();

                    if (dsAsset.Tables.Count > 1 && dsAsset.Tables[1] != null && dsAsset.Tables[1].Rows.Count > 0)
                    {

                        //hdnCustomerID.Value = dsAsset.Tables[1].Rows[0]["Customer_ID"].ToString();
                        //S3GCustomerAddress1.SetCustomerDetails(dsAsset.Tables[1].Rows[0], true);
                        //txtCustName.Text = dsAsset.Tables[1].Rows[0]["Customer_Name"].ToString();


                        if (dsAsset.Tables[1].Rows[0]["PASARefID"].ToString() != "0")
                        {
                            hdnSLA.Value = dsAsset.Tables[1].Rows[0]["PASARefID"].ToString();
                        }
                    }
                }
                if (intInvoiceID > 0)
                {
                    ddlBranch.SelectedText = dsAsset.Tables[1].Rows[0]["Location_Name"].ToString();
                    ddlBranch.ToolTip = dsAsset.Tables[1].Rows[0]["Location_Name"].ToString();
                    ddlBranch.SelectedValue = dsAsset.Tables[1].Rows[0]["Location_ID"].ToString();
                    //ddlVendorCode.SelectedValue = dsAsset.Tables[1].Rows[0]["Vendor_ID"].ToString();
                    ddlVendorCode.Items.Insert(0, new ListItem(dsAsset.Tables[1].Rows[0]["Vendor_Name"].ToString(), dsAsset.Tables[1].Rows[0]["Vendor_ID"].ToString()));
                    //FunPriBindVendorDet();
                    ddlRefDocType.Items.Add(new ListItem(dsAsset.Tables[1].Rows[0]["Ref_Type"].ToString(), dsAsset.Tables[1].Rows[0]["Ref_Docu_Type"].ToString()));
                    ddlRefDocType.ToolTip = dsAsset.Tables[1].Rows[0]["Ref_Type"].ToString();

                    //ddlRefDocType.SelectedValue = dsAsset.Tables[1].Rows[0]["Ref_Docu_Type"].ToString();
                    //ddlRefDocNo.SelectedValue = dsAsset.Tables[1].Rows[0]["Ref_Doc_No"].ToString();
                    // FunPriBindRefDocNo();

                    //ddlRefDocNo.Items.Insert(0, new ListItem(dsAsset.Tables[1].Rows[0]["RefDocNoText"].ToString(), dsAsset.Tables[1].Rows[0]["Ref_Docu_No"].ToString()));
                    ddlRefDocNo.SelectedText = dsAsset.Tables[1].Rows[0]["RefDocNoText"].ToString();
                    ddlRefDocNo.SelectedValue = dsAsset.Tables[1].Rows[0]["Ref_Docu_No"].ToString();
                    ddlRefDocNo.ToolTip = dsAsset.Tables[1].Rows[0]["RefDocNoText"].ToString();


                    FunBindSLA();
                    ddlSLA.SelectedValue = hdnSLA.Value;
                    // PriBindCustomer();

                    hdnCustomerID.Value = dsAsset.Tables[1].Rows[0]["Customer_ID"].ToString();
                    S3GCustomerAddress1.SetCustomerDetails(dsAsset.Tables[1].Rows[0], true);

                    txtInvoiceNo.Text = dsAsset.Tables[1].Rows[0]["Vendor_Invoice_No"].ToString();
                    txtInvoiceDate.Text = DateTime.Parse(dsAsset.Tables[1].Rows[0]["Vendor_Invoice_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    //txtInvoiceDate.Text = dsAsset.Tables[1].Rows[0]["Vendor_Invoice_Date"].ToString();
                    chkBookInAsset.Checked = Convert.ToBoolean(dsAsset.Tables[1].Rows[0]["IsBookin_Asset"].ToString());
                    txtTaxRegNo.Text = dsAsset.Tables[1].Rows[0]["TaxReg_No"].ToString();
                    txtVATNo.Text = dsAsset.Tables[1].Rows[0]["VAT_No"].ToString();

                    if (txtVATNo.Text.Trim() != "")
                    {
                        txtVATNo.ReadOnly = true;
                    }
                    else
                    {
                        txtVATNo.ReadOnly = false;
                    }

                    if (dsAsset.Tables[1].Rows[0]["Invoice_Image"].ToString().Trim() != "")
                    {
                        rdoYes.Checked = true;
                        lnkDownload.Enabled = true;
                        hdnFile.Value = dsAsset.Tables[1].Rows[0]["Invoice_Image"].ToString();
                        lblDisplayFile.Text = hdnFile.Value;
                        fileScanImage.Enabled = true;
                        hdnFileUploaded.Value = "1";
                        //rfvScanImage.Enabled = true;
                    }
                    else
                    {
                        //rfvScanImage.Enabled = false;
                        rdoNo.Checked = true;
                        fileScanImage.Enabled = false;
                        hdnFileUploaded.Value = "0";
                    }
                    //txtVATNo.Text = dsAsset.Tables[1].Rows[0]["VAT_No"].ToString();
                    //Scan_Image

                    //  FunPriBindVendorDet();
                    FunLoadVendorDetails(dsAsset.Tables[1].Rows[0]);
                }

                if ((ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.LastIndexOf("-")).Trim() == "OL") && (grvBookInAsset.Rows.Count > 0))
                {
                    grvBookInAsset.Visible = true;
                    //tdBookIn.Visible = true;
                    //chkBookInAsset.Visible = true;
                }
                else
                {
                    grvBookInAsset.Visible = false;
                    //tdBookIn.Visible = false;
                    //chkBookInAsset.Visible = false;
                }
            }
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
            dsAsset.Dispose();
        }
    }

    #region "File Upload"

    public bool FunFileLoad(bool bUpload)
    {
        AjaxControlToolkit.AsyncFileUpload fileScanImage = (AjaxControlToolkit.AsyncFileUpload)tbInvoice.FindControl("fileScanImage");

        if ((hdnFileUploaded.Value == "1") && (strMode == "M") && (hdnFileName.Value == ""))
        {
            return true;
        }
        if (hdnFileName.Value != "")
        {
            try
            {
                //System.Text.RegularExpressions.Regex strFileValidationExpression = new System.Text.RegularExpressions.Regex(@"\.txt$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                string strFilePath = "";
                string strFileName = fileScanImage.FileName;
                //string strLoanAdminPath = Server.MapPath(".");
                //string strFilePath = strLoanAdminPath.Replace("\\LoanAdmin", "") + "\\Data\\Invoice";
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Program_ID", "58");
                DataTable dtPath = Utility.GetDefaultData("S3G_LOANAD_GetDocPath", Procparam);
                if (dtPath != null && dtPath.Rows.Count > 0)
                {
                    strFilePath = dtPath.Rows[0]["DOCUMENT_PATH"].ToString();
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Define Scan Image Path in Document path setup");
                    return false;
                }

                if (!Directory.Exists(strFilePath))
                {
                    Utility.FunShowAlertMsg(this, "Define Scan Image Path in Document path setup");
                    return false;
                }
                strFilePath = strFilePath + "\\" + strFileName;
                if (File.Exists(strFilePath))
                {
                    Utility.FunShowAlertMsg(this.Page, "Already the same file name(" + strFileName + ") exists in the target path");
                    return false;
                }
                else
                {
                    if (bUpload)
                    {
                        fileScanImage.PostedFile.SaveAs(strFilePath);
                    }
                    //ViewState["FilePath"] = strFilePath;
                    //ViewState["Download"] = 1;
                    //lblErrorMessage.Text = "Updated successfully";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        else
        {
            rfvFile.IsValid = false;
            //Utility.FunShowAlertMsg(this.Page, "Browse a file to upload");
            return false;
        }
        return true;
    }

    #endregion

    private void FunPriClear(bool bClearAll, string strControlName)
    {
        if (bClearAll)
        {
            if (strControlName != "LOB")
            {
                ddlLOB.SelectedIndex = 0;
            }
            //ddlBranch.SelectedIndex = 0;
            //ddlBranch.Items.Clear();
            if (ddlVendorCode.Items.Count > 0)
                ddlVendorCode.SelectedIndex = 0;
            ddlRefDocType.SelectedIndex = 0;
            ddlSLA.Items.Clear();
            if (ddlSLA.Items.Count > 0)
                ddlSLA.SelectedIndex = 0;
            //if (ddlRefDocNo.Items.Count > 0)
            //    ddlRefDocNo.SelectedIndex = 0;
            ddlRefDocNo.Clear();
            txtInvoiceDate.Text = DateTime.Now.ToString(strDateFormat);
            txtTaxRegNo.Text = "";
            txtVATNo.Text = "";
            txtInvoiceNo.Text = "";
            txtInvoiceDate.Text = "";
            chkBookInAsset.Checked = false;
            ddlInvoiceType.Items.Clear();
            //lblDisplayFile.Visible = false;
            lblDisplayFile.Text = "";

        }
        AjaxControlToolkit.AsyncFileUpload fileScanImage = (AjaxControlToolkit.AsyncFileUpload)tbInvoice.FindControl("fileScanImage");

        rdoNo.Checked = false;
        rdoYes.Checked = true;
        fileScanImage.Enabled = true;

        //txtCustName.Text = "";
        //txtAddress1.Text = "";
        //txtAddress2.Text = "";
        //txtCity.Text = "";
        //txtState.Text = "";
        //txtCountry.Text = "";
        //txtPincode.Text = "";
        //txtMobile.Text = "";
        //txtTelephone.Text = "";
        //txtEmailid.Text = "";

        FunPriClearVendor();

        S3GCustomerAddress1.ClearCustomerDetails();

        grvAssetDetails.DataSource = null;
        grvAssetDetails.DataBind();

        grvBookInAsset.DataSource = null;
        grvBookInAsset.DataBind();

        grvWarranty.DataSource = null;
        grvWarranty.DataBind();
    }

    private void FunPriClearVendor()
    {
        if (ddlVendorCode.Items.Count > 0)
        {
            ddlVendorCode.Items.Clear();
            // ddlVendorCode.SelectedIndex = 0;

        }

        txtVendorName.Text = "";


        S3GVendorAddress.ClearCustomerDetails();


        //txtVenAddress.Text = "";
        //txtVenAddress2.Text = "";
        //txtVenCity.Text = "";
        //txtVenState.Text = "";
        //txtVenCountry.Text = "";
        //txtVenPincode.Text = "";
        //txtVenMobile.Text = "";
        //txtVenTelephone.Text = "";
        //txtVenEmailid.Text = "";
    }

    private void FunLoadVendorDetails(DataRow dtVendorDet)
    {
        txtVendorName.Text = dtVendorDet["V_Name"].ToString();
       string strVendorAddress = SetVendorAddressdetails(dtVendorDet);
       S3GVendorAddress.SetCustomerDetails("", strVendorAddress, txtVendorName.Text, dtVendorDet["Vendor_Telephone"].ToString(), dtVendorDet["Vendor_Mobile"].ToString(), dtVendorDet["Vendor_EMail"].ToString(), dtVendorDet["Vendor_Website"].ToString());
        txtTaxRegNo.Text = dtVendorDet["Tax_Account_Number"].ToString();
        txtVATNo.Text = dtVendorDet["VAT_Number"].ToString();

        if (txtVATNo.Text.Trim() != "")
        {
            txtVATNo.ReadOnly = true;
        }
        else
        {
            txtVATNo.ReadOnly = false;
        }
    }
    private string SetVendorAddressdetails(DataRow drCust)
    {
        string strAddress = "";
        if (drCust["Vendor_Address"].ToString() != "") strAddress += drCust["Vendor_Address"].ToString() + System.Environment.NewLine;
        if (drCust["Vendor_Address2"].ToString() != "") strAddress += drCust["Vendor_Address2"].ToString() + System.Environment.NewLine;
        if (drCust["Vendor_City"].ToString() != "") strAddress += drCust["Vendor_City"].ToString() + System.Environment.NewLine;
        if (drCust["Vendor_State"].ToString() != "") strAddress += drCust["Vendor_State"].ToString() + System.Environment.NewLine;
        if (drCust["Vendor_Country"].ToString() != "") strAddress += drCust["Vendor_Country"].ToString() + System.Environment.NewLine;
        if (drCust["Vendor_Pincode"].ToString() != "") strAddress += drCust["Vendor_Pincode"].ToString();
        return strAddress;
    }
    private void FunPriBindVendorDet()
    {

        string strVendorAddress = "";
        strProcName = SPNames.S3G_ORG_GetEntityMasterVendor;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Entity_ID", ddlVendorCode.SelectedValue);
        DataTable dtVendorDet = Utility.GetDefaultData(strProcName, Procparam);

        if (dtVendorDet.Rows.Count > 0)
        {
            txtVendorName.Text = dtVendorDet.Rows[0]["Entity_Name"].ToString();
            strVendorAddress = Utility.SetVendorAddress(dtVendorDet.Rows[0]);
            S3GVendorAddress.SetCustomerDetails("", strVendorAddress, txtVendorName.Text, dtVendorDet.Rows[0]["Telephone"].ToString(), dtVendorDet.Rows[0]["Mobile"].ToString(), dtVendorDet.Rows[0]["EMail"].ToString(), dtVendorDet.Rows[0]["Website"].ToString());
            txtTaxRegNo.Text = dtVendorDet.Rows[0]["Tax_Account_Number"].ToString();
            txtVATNo.Text = dtVendorDet.Rows[0]["VAT_Number"].ToString();

            if (txtVATNo.Text.Trim() != "")
            {
                txtVATNo.ReadOnly = true;
            }
            else
            {
                txtVATNo.ReadOnly = false;
            }
        }
        else
        {
            txtVendorName.Text = "";
            S3GVendorAddress.ClearCustomerDetails();
            txtVATNo.Text = "";
            txtTaxRegNo.Text = "";
        }


        //txtVenAddress.Text = dtVendorDet.Rows[0]["Address"].ToString();
        //txtVenAddress2.Text = dtVendorDet.Rows[0]["Address2"].ToString();
        //txtVenCity.Text = dtVendorDet.Rows[0]["City"].ToString();
        //txtVenState.Text = dtVendorDet.Rows[0]["State"].ToString();
        //txtVenCountry.Text = dtVendorDet.Rows[0]["Country"].ToString();
        //txtVenPincode.Text = dtVendorDet.Rows[0]["PinCode"].ToString();
        //txtVenMobile.Text = dtVendorDet.Rows[0]["Mobile"].ToString();
        //txtVenTelephone.Text = dtVendorDet.Rows[0]["Telephone"].ToString();
        //txtVenEmailid.Text = dtVendorDet.Rows[0]["EMail"].ToString();





    }
    private void FunPriGenerateInvoiceXMLDet()
    {
        try
        {
            strXMLAssetDet = grvAssetDetails.FunPubFormXml(enumGridType.TemplateGrid);
            strXMLWarrantyDet = grvWarranty.FunPubFormXml(enumGridType.TemplateGrid);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// This method will validate the from and to date - entered by the user.
    /// </summary>
    private bool FunPriValidateFromEndDate(string strFromDate, string strToDate)
    {
        DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
        dtformat.ShortDatePattern = "MM/dd/yy";

        //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
        if (Utility.StringToDate(strFromDate) > Utility.StringToDate(strToDate)) // start date should be less than or equal to the enddate
        {
            return false;
        }
        return true;
    }


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

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                ddlLOB.Enabled = false;
                ddlBranch.Enabled = false;
                ddlRefDocType.Enabled = false;
                ddlRefDocNo.Enabled = false;
                ddlSLA.Enabled = false;
                ddlVendorCode.Enabled = false;
                ddlInvoiceType.Enabled = false;
                btnClear.Enabled = false;
                txtInvoiceDate.ReadOnly = true;
                txtInvoiceDate.Attributes.Remove("onblur");
                break;

            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                { Response.Redirect(strRedirectPage,false); }

                if (bClearList)
                {
                    ddlBranch.ReadOnly = true;
                    //ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    //ddlRefDocType.ClearDropDownList();
                    ddlRefDocNo.Enabled = false;
                    ddlVendorCode.ClearDropDownList();
                    ddlSLA.ClearDropDownList();
                    ddlInvoiceType.ClearDropDownList();
                }
                ddlLOB.Enabled = true;
                ddlBranch.Enabled = true;
                ddlRefDocType.Enabled = true;
                ddlRefDocNo.Enabled = false; //BUG FIXING BY Palanikumar.A on 21/01/2014
                rdoYes.Enabled = false;
                rdoNo.Enabled = false;
                CalendarExtender1.Enabled = false;
                txtInvoiceNo.ReadOnly = true;
                txtTaxRegNo.ReadOnly = true;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                ddlSLA.Enabled = true;
                txtInvoiceDate.ReadOnly = true;
                txtInvoiceDate.Attributes.Remove("onblur");

                //lnkDownload.Enabled = false;
                foreach (GridViewRow GvRow in grvAssetDetails.Rows)
                {
                    TextBox txtUnitValue = (TextBox)GvRow.FindControl("txtUnitValue");
                    TextBox txtNoofUnit = (TextBox)GvRow.FindControl("txtNoofUnit");
                    TextBox txtTax = (TextBox)GvRow.FindControl("txtTax");
                    TextBox txtVAT = (TextBox)GvRow.FindControl("txtVAT");
                    TextBox txtOthers = (TextBox)GvRow.FindControl("txtOthers");
                    if (txtUnitValue.Text != null)
                    { txtUnitValue.ReadOnly = true; }
                    if (txtNoofUnit.Text != null)
                    { txtNoofUnit.ReadOnly = true; }
                    if (txtTax.Text != null)
                    { txtTax.ReadOnly = true; }
                    if (txtVAT.Text != null)
                    { txtVAT.ReadOnly = true; }
                    if (txtOthers.Text != null)
                    { txtOthers.ReadOnly = true; }
                }
                foreach (GridViewRow GvRow in grvWarranty.Rows)
                {
                    DropDownList ddlWarrantyType = (DropDownList)GvRow.FindControl("ddlWarrantyType");
                    AjaxControlToolkit.CalendarExtender ceFromDate = (AjaxControlToolkit.CalendarExtender)GvRow.FindControl("ceFromDate");
                    AjaxControlToolkit.CalendarExtender ceToDate = (AjaxControlToolkit.CalendarExtender)GvRow.FindControl("ceToDate");
                    DropDownList ddlServiceFrequency = (DropDownList)GvRow.FindControl("ddlServiceFrequency");
                    TextBox txtRemarks = (TextBox)GvRow.FindControl("txtRemarks");
                    if (bClearList)
                    {
                        ddlWarrantyType.ClearDropDownList();
                        ddlServiceFrequency.ClearDropDownList();
                    }
                    if (ceFromDate != null)
                    { ceFromDate.Enabled = false; }
                    if (ceToDate != null)
                    { ceToDate.Enabled = false; }
                    if (txtRemarks.Text != null)
                    { txtRemarks.ReadOnly = true; }
                }
                break;
        }

    }

    ////Code end

    ////////methods added by Kali on 03-Sep-2010 for Journal entry creation

    //////private void FunPriSysJournalEntry()
    //////{
    //////    string strXMLAccDetails = string.Empty;
    //////    StringBuilder strbSysJournal = new StringBuilder();
    //////    strbSysJournal.Append("<Root> ");

    //////    foreach (GridViewRow grvData in grvAssetDetails.Rows)
    //////    {
    //////        TextBox txtTxnAmt = ((TextBox)grvData.FindControl("txtNetValue"));
    //////        Label lblDim2 = ((Label)grvData.FindControl("lblAssetID"));

    //////        strbSysJournal.Append("<Details ");
    //////        strbSysJournal.Append("Occur_No='" + grvData.RowIndex + "' "); // Record Order by
    //////        strbSysJournal.Append("Txn_Amt='" + ObjSysJournal.Txn_Amount + "' "); // Debit or Credit Amount
    //////        strbSysJournal.Append("Acc_Flag='0'"); // Debit or Credit
    //////        strbSysJournal.Append("Dim2_Code='1'"); // Global_Dimension2_Code (Asset or Processing Fees)
    //////        strbSysJournal.Append("Dim2_No='" + lblDim2.Text + "' "); // Global_Dimension2_No
    //////        strbSysJournal.Append(" /> ");

    //////        //ObjSysJournal.Occurrence_No = grvData.RowIndex;     // Record Order by
    //////        //ObjSysJournal.Accounting_Flag = '0';                // Debit or Credit
    //////        //ObjSysJournal.Txn_Amount = txtTxnAmt.Text;          // Debit or Credit Amount
    //////        //ObjSysJournal.Global_Dimension2_Code = "1";         //Asset or Processing Fees
    //////        //ObjSysJournal.Global_Dimension2_No = lblDim2.Text;  //LAR Number or Asset ID or Processing ID
    //////        //strXMLAccDetails += Utility.FunPubSysJournalXMLGenerate(ObjSysJournal);
    //////        //strXMLAccDetails += strXMLAccDetails;
    //////    }
    //////    strbSysJournal.Append(" </Root>");
    //////    //ObjSysJournal.Company_ID = intCompanyID;
    //////    //ObjSysJournal.Branch_ID =Convert.ToInt32(ddlBranch.SelectedValue);
    //////    //ObjSysJournal.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
    //////    //ObjSysJournal.Customer_ID = Convert.ToInt32(hdnCustomerID.Value);
    //////    //ObjSysJournal.Account_Link_Key = strInvoiceNo;  //Page generated document number control

    //////    ObjSysJournal.Narration = "";                   // Now it is optional
    //////    ObjSysJournal.Value_Date = DateTime.Now;        //Posting Date
    //////    ObjSysJournal.Txn_Currency_Code = string.Empty;           //Assign if there is any txn currency.

    //////    ObjSysJournal.Program_ID = 58;                  //Page ProgramID
    //////    //ObjSysJournal.Global_Dimension1_Code = 6;       //Vendor or Dealer...

    //////    ObjSysJournal.Global_Dimension1_No = ddlVendorCode.SelectedValue;   // Vendor or Dealer Code
    //////    ObjSysJournal.JV_Status_Code = 1;               //Active or Cancelled or Reverse Journal
    //////    ObjSysJournal.Reference_Number = ddlRefDocNo.SelectedValue; // MLA Number or PANum 
    //////    //ObjSysJournal.Created_By = intUserID;           // Record created by

    //////    ObjSysJournal.XMLSysJournal = strbSysJournal.ToString();

    //////    Utility.FunPubSysJournalEntry(ObjSysJournal);
    //////}

    ////////Method code end.

    #region SysJournal


    //Method added by Kaliz K on 03-Sep-2010 for Sys Journal entry creation

    private void FunPriSysJournalEntry()
    {
        string strXMLAccDetails = string.Empty;
        StringBuilder strbSysJournal = new StringBuilder();
        strbSysJournal.Append("<Root> ");

        foreach (GridViewRow grvData in grvAssetDetails.Rows)
        {
            strbSysJournal.Append("<Details ");
            strbSysJournal.Append("Occur_No='" + grvData.RowIndex + "' "); // Record Order by

            //Below code to be changed on every page
            TextBox txtTxnAmt = ((TextBox)grvData.FindControl("txtNetValue"));
            Label lblDim2 = ((Label)grvData.FindControl("lblAssetID"));
            strbSysJournal.Append("Txn_Amt='" + txtTxnAmt.Text + "' "); // Debit or Credit Amount
            strbSysJournal.Append("Dim2_No='" + lblDim2.Text + "' "); // Global_Dimension2_No
            //Code end

            strbSysJournal.Append("Acc_Flag='" + ((int)enumTxnType.Debit) + "' "); // Debit or Credit
            strbSysJournal.Append("Dim2_Code='" + ((int)enumDim2.AssetCode) + "' "); // Global_Dimension2_Code (Asset or Processing Fees)
            strbSysJournal.Append(" /> ");

        }
        strbSysJournal.Append(" </Root>");

        ObjSysJournal.XMLSysJournal = strbSysJournal.ToString();
        ObjSysJournal.Narration = "";                   // Now it is optional
        ObjSysJournal.JV_Status_Code = Convert.ToInt32(enumJVStatus.Active);               //Active or Cancelled or Reverse Journal

        //Below code to be changed on every page
        ObjSysJournal.Txn_Currency_Code = string.Empty;           //Assign if there is any txn currency.
        ObjSysJournal.Value_Date = DateTime.Now;        // Posting Date
        ObjSysJournal.Global_Dimension1_No = ddlVendorCode.SelectedValue;   // Vendor or Dealer Code (Entity ID)
        ObjSysJournal.Reference_Number = ddlRefDocNo.SelectedValue; // MLA Number or PANum 
        //Code end

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
        Procparam.Add("@Program_Id", "058");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    //Method code end.

    #endregion

    #endregion

}



