
//Module Name      :   Origination
//Screen Name      :   S3GORGProforma_Add.aspx
//Created By       :   Kaliraj K
//Created Date     :   12-JUL-2010
//Purpose          :   To insert and update proforma details

using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;

public partial class S3GORGProforma_Add : ApplyThemeForProject
{
    #region intColCountalization

    ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjProformaClient;
    ApplicationMgtServices.S3G_ORG_ProformaDataTable ObjS3G_ORG_ProformaDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;

    int intProformaID = 0;
    int intUserID = 0;
    int intCompanyID = 0;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    public static S3GORGProforma_Add obj_Page;
    //Code end

    DataSet dsAsset = new DataSet();
    Dictionary<string, string> Procparam = null;
    Dictionary<string, string> dictLOB = null;

    StringBuilder strbLOBDet = new StringBuilder();
    StringBuilder strbProformaDet = new StringBuilder();
    string strRedirectPage = "../Origination/S3GORGTransLander.aspx?Code=PROF";
    string strKey = "Insert";

    string strXMLAssetDet = string.Empty;
    string strXMLWarrantyDet = string.Empty;
    string strFileName = string.Empty;
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGProforma_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=PROF';";
    DataTable dtDefault = new DataTable();
    DataTable dtFrequency = new DataTable();
    int iCount = 0;
    DataTable dtProforma = new DataTable();
    #endregion

    #region PageLoad

    /// <summary>
    /// Event for Pre-Initialize the Components  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /*  protected new void Page_PreInit(object sender, EventArgs e)
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
      }*/

    protected void Page_Load(object sender, EventArgs e)
    {

        // WF Implementation
        ProgramCode = "044";


        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        //Code Commented and added by Ganapathy on 22/10/2012 for date control Implementation BEGINS
        //txtProformaDate.Attributes.Add("readonly", "readonly");
        txtProformaDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtProformaDate.ClientID + "','" + strDateFormat + "',true,  false);");
        //ENDS
        CalendarExtender1.Format = strDateFormat;
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));

            if (fromTicket != null)
            {
                intProformaID = Convert.ToInt32(fromTicket.Name);
                hdnProformaID.Value = intProformaID.ToString();
                strMode = Request.QueryString.Get("qsMode");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        intProformaID = Convert.ToInt32(hdnProformaID.Value);
        //intProformaID = 5;
        //strMode = "M";

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end

        obj_Page = this;

        if (!IsPostBack)
        {
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if (PageMode == PageModes.Create)
            {
                FunPriBindLOBBranchVendor();
            }
            if ((intProformaID > 0) && (strMode == "M"))
            {
                FunPriBindRefDocNo();
                FunPriDisableControls(1);
                FunGetProformaDetails();
            }
            else if ((intProformaID > 0) && (strMode == "Q"))
            {
                FunGetProformaDetails();
                FunPriDisableControls(-1);

            }
            else
            {
                FunPriDisableControls(0);
            }

            // WF 
            if (PageMode == PageModes.WorkFlow)
            {
                ViewState["PageMode"] = PageModes.WorkFlow;
            }
            if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())
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


        if (PageMode == PageModes.WorkFlow)
        {
            if (ViewState["intProformaID"] != null)
                intProformaID = Convert.ToInt32(ViewState["intProformaID"]);
        }



        //lnkDownload.Visible = false;

        if (rdoYes.Checked)
        {
            fileScanImage.Enabled = true;
            rdoYes.Checked = true;

        }
        else if (rdoNo.Checked)
        {
            fileScanImage.Enabled = false;
            rdoNo.Checked = true;
        }
        else
        {
            rdoYes.Checked = true;
        }
        if (strMode == "Q")
        {
            fileScanImage.Enabled = false;
        }
        grvBookInAsset.Visible = false;

        if ((IsPostBack) && (hdnFileName.Value != ""))
        {
            //FileInfo ajaxFileInfo=new FileInfo(hdnFileName.Value);
            lblDisplayFile.Text = hdnFileName.Value;

        }

    }


    #endregion


    #region "WF Code"
    void PreparePageForWorkFlowLoad()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowServiceClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();

        try
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();
            byte[] byte_PreDisDoc = objWorkflowServiceClient.FunPubLoadProforma(WFSessionValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValues.Document_Type, WFSessionValues.PANUM, WFSessionValues.SANUM);
            DataSet dsWorkflow = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_PreDisDoc, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));


            if (dsWorkflow.Tables.Count > 1)
            {
                if (dsWorkflow.Tables[1].Rows.Count > 0)
                {
                    intProformaID = Convert.ToInt32(dsWorkflow.Tables[1].Rows[0]["Doc_Id"].ToString());
                    ViewState["intProformaID"] = intProformaID;

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
                    ddlBranch.ReadOnly = true;

                    ddlRefDocType.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Document_Type"].ToString();
                    FunPriBindRefDocNo();
                    ddlRefDocNo.SelectedValue = dsWorkflow.Tables[0].Rows[0]["Document_Type_ID"].ToString();
                    FunPriSetRefDocNo();

                    ddlRefDocType.ClearDropDownList();
                    ddlRefDocNo.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objWorkflowServiceClient.Close();
        }
    }
    #endregion



    #region Page Events

    /// <summary>
    /// This is used to save Proforma details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void File_CheckedChanged(object sender, EventArgs e)
    {
        if (rdoYes.Checked)
            lblDisplayFile.Visible = true;
        else
            lblDisplayFile.Visible = false;
    }

    protected void ddlRefDocNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriClear(false);

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
                FunGetProformaDetails();
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
            FunGetProformaDetails();
        }
    }

    protected void ddlVendorCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlVendorCode.SelectedIndex > 0)
            if (ddlVendorCode.SelectedValue != "0")
            {
                FunPriBindVendorDet();
                lblDisplayFile.Text = "";
            }
            else
            {
                FunPriClearVendor();
            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriClear(false);
        FunPriBindRefDocNo();
        FunPubLoadBranch();
        if (ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.LastIndexOf("-")).Trim() == "OL")
        {
            chkBookInAsset.Checked = true;
        }
        else
        {
            chkBookInAsset.Checked = false;
        }

        txtTaxRegNo.Text = "";
        txtVATNo.Text = "";
        txtProformaNo.Text = "";
        txtProformaDate.Text = "";
        chkBookInAsset.Checked = false;
        lblDisplayFile.Text = string.Empty;
        File_CheckedChanged(this, new EventArgs());
    }

    private void FunPriBindRefDocNo()
    {
        try
        {
            //ddlRefDocNo.Items.Clear();
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            //Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            //Procparam.Add("@OptionValue", ddlRefDocType.SelectedValue);
            //ddlRefDocNo.BindDataTable(SPNames.S3G_ORG_GetProformaRefDocNo, Procparam, new string[] { "RefDoc_ID", "RefDoc_No" });
            //ddlSLA.Items.Clear();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
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

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@OptionValue", obj_Page.ddlRefDocType.SelectedValue);
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@Prefix", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetProformaRefDocNo_AGT", Procparam));

        return suggetions.ToArray();
        
    }
    //end here//

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriClear(false);
        FunPriBindRefDocNo();
        if (ddlRefDocType.SelectedValue == "4")
        {
            ddlSLA.Enabled = true;
        }
        else
        {
            ddlSLA.Enabled = false;
        }

        txtTaxRegNo.Text = "";
        txtVATNo.Text = "";
        txtProformaNo.Text = "";
        txtProformaDate.Text = "";
        chkBookInAsset.Checked = false;
        lblDisplayFile.Text = string.Empty;
        File_CheckedChanged(this, new EventArgs());
    }

    protected void ddlRefDocType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClear(false);
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
        ObjProformaClient = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
        try
        {
            bool bVATRequired = false;
            lnkDownload.Enabled = false;
            //AjaxControlToolkit.AsyncFileUpload fileScanImage = (AjaxControlToolkit.AsyncFileUpload)tbProforma.FindControl("fileScanImage");
            //string strFileName = FileUpload1.FileName;
            //FunPriCalculateTax();
            if ((rdoYes.Checked) && (!FunFileLoad(false)))
            {
                return;
            }
            if (grvAssetDetails.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Asset details does not exist for the selected Reference Document Number");
                return;
            }
            bool bReqVal = true;
            foreach (GridViewRow grvData in grvAssetDetails.Rows)
            {
                TextBox txtTAX = (TextBox)grvData.FindControl("txtTax");
                TextBox txtVAT = (TextBox)grvData.FindControl("txtVAT");
                TextBox txtUnitValue = (TextBox)grvData.FindControl("txtUnitValue");
                TextBox txtNoofUnit = (TextBox)grvData.FindControl("txtNoofUnit");
                TextBox txtOthers = (TextBox)grvData.FindControl("txtOthers");

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
                if ((txtTAX.Text == "") && (txtVAT.Text == ""))
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

            }

            foreach (GridViewRow grvData in grvWarranty.Rows)
            {
                DropDownList ddlWarrantyType = (DropDownList)grvData.FindControl("ddlWarrantyType");
                DropDownList ddlServiceFrequency = (DropDownList)grvData.FindControl("ddlServiceFrequency");
                TextBox txtFromDate = (TextBox)grvData.FindControl("txtFromDate");
                TextBox txtToDate = (TextBox)grvData.FindControl("txtToDate");
                TextBox txtRemarks = (TextBox)grvData.FindControl("txtRemarks");

                if ((ddlWarrantyType.SelectedValue != "0") || (txtFromDate.Text != "") || (txtToDate.Text != "") || (ddlServiceFrequency.SelectedValue != "0") || (txtRemarks.Text != ""))
                {
                    if (ddlWarrantyType.SelectedValue == "0")
                    {
                        rfvWarrantyType.IsValid = false;
                        bReqVal = false;
                    }
                    if (txtFromDate.Text == "")
                    {
                        rfvFromDate.IsValid = false;
                        bReqVal = false;
                    }
                    if (txtToDate.Text == "")
                    {
                        rfvToDate.IsValid = false;
                        bReqVal = false;
                    }
                    if (ddlServiceFrequency.SelectedValue == "0")
                    {
                        rfvServiceFrequency.IsValid = false;
                        bReqVal = false;
                    }
                    //if (txtRemarks.Text == "")
                    //{
                    //    rfvRemarks.IsValid = false;
                    //    bReqVal = false;
                    //}
                    if ((txtFromDate.Text != "") && (txtToDate.Text != ""))
                    {
                        if (!FunPriValidateFromEndDate(txtFromDate.Text, txtToDate.Text))
                        {
                            rfvCompareDate.ErrorMessage = "To Date should be greater than or equal to the From Date in Warranty Details";
                            rfvCompareDate.IsValid = false;
                            bReqVal = false;
                            //Utility.FunShowAlertMsg(this, ValidationMsgs.S3G_ValMsg_DateCompare+" in Warranty Details");

                        }
                    }
                }
                if (bReqVal == false)
                {
                    return;
                }
            }

            if ((bVATRequired) && (txtVATNo.Text == ""))
            {
                rfvVAT.ErrorMessage = "Enter VAT Number if VAT % exists in Asset Details";
                rfvVAT.IsValid = false;
                return;
            }
            if ((!bVATRequired) && (txtVATNo.Text != ""))
            {
                rfvVAT.ErrorMessage = "Enter VAT % in Asset Details if VAT Number Exists";
                rfvVAT.IsValid = false;
                return;
            }

            ObjS3G_ORG_ProformaDataTable = new ApplicationMgtServices.S3G_ORG_ProformaDataTable();
            ApplicationMgtServices.S3G_ORG_ProformaRow ObjProformaRow;
            ObjProformaRow = ObjS3G_ORG_ProformaDataTable.NewS3G_ORG_ProformaRow();
            ObjProformaRow.Proforma_ID = intProformaID;
            ObjProformaRow.Company_ID = intCompanyID;
            ObjProformaRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjProformaRow.Customer_ID = Convert.ToInt32(hdnCustomerID.Value);
            ObjProformaRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjProformaRow.Vendor_ID = Convert.ToInt32(ddlVendorCode.SelectedValue);
            ObjProformaRow.Ref_Doc_Type = Convert.ToInt32(ddlRefDocType.SelectedValue);


            if (ddlRefDocType.SelectedValue == "4")
            {
                //ObjInvoiceRow.Ref_Docu_No = Convert.ToInt32(ddlSLA.SelectedValue);
                ObjProformaRow.Ref_Doc_No = Convert.ToInt32(hdnSLA.Value);
            }
            else
            {
                ObjProformaRow.Ref_Doc_No = Convert.ToInt32(ddlRefDocNo.SelectedValue);
            }

            //ObjProformaRow.Ref_Doc_No = Convert.ToInt32(ddlRefDocNo.SelectedValue);

            ObjProformaRow.Book_in_Asset = chkBookInAsset.Checked;
            ObjProformaRow.TaxRegNo = txtTaxRegNo.Text;
            ObjProformaRow.VATNo = txtVATNo.Text.Trim();
            ObjProformaRow.Proforma_No = txtProformaNo.Text.Trim();
            ObjProformaRow.Proforma_Date = Utility.StringToDate(txtProformaDate.Text);
            ObjProformaRow.Proforma_Image = rdoYes.Checked;

            if (rdoYes.Checked)
            {
                ObjProformaRow.ScanPath = fileScanImage.FileName;
                hdnFile.Value = fileScanImage.FileName;
            }
            else
            {
                ObjProformaRow.ScanPath = "";
            }
            ObjProformaRow.Created_By = intUserID;
            //fileScanImage.PostedFile.SaveAs(strFileName);
            string strAssetDetails = grvAssetDetails.FunPubFormXml().Replace("%", "").Replace("VAT=''", "VAT='0'").Replace("Tax=''", "Tax='0'");
            strAssetDetails = strAssetDetails.Replace("VATValue=''", "VATValue='0'").Replace("TaxValue=''", "TaxValue='0'");
            ObjProformaRow.XMLAssetDetails = strAssetDetails.Replace("Others=''", "Others='0'");
            ObjProformaRow.XMLWarrantyDetails = grvWarranty.FunPubFormXml().Replace("%", "");
            //ObjProformaRow.XMLWarrantyDetails = grvWarranty.FunPubFormXml().Replace("%", "");


            ObjS3G_ORG_ProformaDataTable.AddS3G_ORG_ProformaRow(ObjProformaRow);

            intErrCode = ObjProformaClient.FunPubCreateProformaDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_ProformaDataTable, SerMode));

            if (intErrCode == 0)
            {
                if (rdoYes.Checked)
                {
                    FunFileLoad(true);
                    lnkDownload.Enabled = true;
                }
                if (intProformaID > 0)
                {
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    //Utility.FunShowAlertMsg(this.Page, "Proforma details updated successfully", strRedirectPage);
                    //return;
                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler)                       
                    {
                        try
                        {

                            WorkFlowSession WFValues = new WorkFlowSession();
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, WFValues.Document_Type);
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow not Assigned";
                            }
                            ShowWFAlertMessage("", ProgramCode, strAlert);
                            return;
                        }
                        catch
                        {

                            Utility.FunShowAlertMsg(this.Page, "Proforma details updated successfully", strRedirectPage);
                            return;

                        }
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this.Page, "Proforma details updated successfully", strRedirectPage);
                        return;
                    }
                }
                else
                {
                    //FunFileLoad(true);
                    //fileScanImage.FileName
                    /* FunGetProformaDetails();
                     strAlert = "Proforma details added successfully";
                     strAlert += @"\n\nWould you like to add one more Proforma?";
                     strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                     strRedirectPageView = "";
                     ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                     lblErrorMessage.Text = string.Empty;
                     return;*/

                    if (ViewState["PageMode"] != null && ViewState["PageMode"].ToString() == PageModes.WorkFlow.ToString())  //if (isWorkFlowTraveler)                       
                    {
                        try
                        {
                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here

                            WorkFlowSession WFValues = new WorkFlowSession();
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, WFValues.Document_Type);
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow not Assigned";
                            }
                            ShowWFAlertMessage("", ProgramCode, strAlert);
                            return;
                        }
                        catch
                        {

                            strAlert = "Proforma details added successfully";
                            strAlert += @"\n\n And Job not assigned to the next user.";
                            strAlert += @"\n\nWould you like to add one more Proforma?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                            lblErrorMessage.Text = string.Empty;
                            return;

                        }
                    }
                    else
                    {
                        DataTable WFFP = new DataTable();
                        string strRefDocNo = string.Empty;
                        //if (!string.IsNullOrEmpty(ddlRefDocNo.SelectedText.ToString()))
                            //strRefDocNo = ddlRefDocNo.SelectedText.Substring(0, ddlRefDocNo.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();
                        if (CheckForForcePullOperation(null, ddlRefDocNo.SelectedValue.Trim(), ProgramCode, null, null, "O", CompanyId, int.Parse(ddlLOB.SelectedValue), null, null, "", out WFFP))
                        {
                            DataRow dtrForce = WFFP.Rows[0];
                            int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), ddlRefDocNo.SelectedValue.Trim(), int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), int.Parse(ddlRefDocType.SelectedValue));
                        }

                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strAlert = "Proforma details added successfully";
                        strAlert += @"\n\nWould you like to add one more Proforma?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                        return;
                    }
                }


            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Proforma details already exist");
                return;
            }
            //else if (intErrCode == 2)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create Proforma Code");
            //}
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "Proforma cannot be updated.Account has been activated");
                return;
            }
            else if (intErrCode == 4)
            {
                Utility.FunShowAlertMsg(this.Page, "Proforma Number already exists");
                return;
            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }

        finally
        {
            ObjProformaClient.Close();
        }
    }


    /// <summary>
    /// This is used to redirect page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["IsFromAccount"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
            }
            else
            {
                Response.Redirect(strRedirectPage,false);
            }
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {
            //string strFileName = Server.MapPath(".").ToString().Replace("\\Origination","") + @"\Data\Proforma\" + hdnFile.Value;
            string strFileName = "/Data/Proforma/" + hdnFile.Value;
            string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            //string strScipt = "window.open('.." + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

            ////// set the http content type to "APPLICATION/OCTET-STREAM
            ////Response.ContentType = "APPLICATION/OCTET-STREAM";

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
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
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
            FunPriClear(true);
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
        if ((e.Row.RowType == DataControlRowType.DataRow) && (intProformaID > 0))
        {
            //DataBinder.Eval(e.Row.DataItem, "NetAmount");
            Label lblNetAmt = (Label)e.Row.FindControl("lblNetAmt");
            lblNetAmt.Text = Convert.ToDecimal(lblNetAmt.Text).ToString("0.00").ToString();
        }
    }

    ////protected void grvWarranty_RowCreated(object sender, GridViewRowEventArgs e)
    ////{

    ////    if (e.Row.RowType == DataControlRowType.DataRow)
    ////    {



    ////    }
    ////}

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
            S3GSession ObSession = new S3GSession();
            //txtUnitValue.SetDecimalPrefixSuffix(ObSession.ProGpsPrefixRW, ObSession.ProGpsSuffixRW, true, "Unit value");
            txtUnitValue.SetPercentagePrefixSuffix(10, 2, true, true, "Unit value");
            txtTax.SetPercentagePrefixSuffix(2, 2, true, "Tax %");
            txtVAT.SetPercentagePrefixSuffix(2, 2, true, "VAT %");
            txtNoofUnit.CheckGPSLength(true, "Number of units");

            //txtOthers.SetDecimalPrefixSuffix(ObSession.ProGpsPrefixRW, ObSession.ProGpsSuffixRW, true, "Others");
            txtOthers.SetDecimalPrefixSuffix(10, 2, true, "Others");

            if (intProformaID > 0)
            {
                if (txtTaxVal.Text != "")
                    txtTaxVal.Text = Convert.ToDecimal(txtTaxVal.Text).ToString("0.00").ToString();
                if (txtVATVal.Text != "")
                    txtVATVal.Text = Convert.ToDecimal(txtVATVal.Text).ToString("0.00").ToString();
                if (txtOthers.Text != "")
                    txtOthers.Text = Convert.ToDecimal(txtOthers.Text).ToString("0.00").ToString();
                txtNetValue.Text = Convert.ToDecimal(txtNetValue.Text).ToString("0.00").ToString();
            }
            string strValue = "fnCalculateTax("
                + "  '" + txtUnitValue.ClientID + "'"
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
            //TextBox txtReqVal = (TextBox)e.Row.FindControl("txtReqValue1");
            //strReqValue += ",'" + ddlYes.ClientID + "','" + txtReqVal.ClientID + "'";
            //ddlField.Attributes.Add("onchange", "fnChangeAttribute('" + ddlField.ClientID + "','" + ddlNumeric.ClientID + "'" + strReqValue + ")");

            //After adding new row the following conditions should work



            if (strMode == "Q")
            {
                txtUnitValue.ReadOnly = true;
                txtNoofUnit.ReadOnly = true;
                txtTax.ReadOnly = true;
                txtVAT.ReadOnly = true;
                txtOthers.ReadOnly = true;
                txtVATVal.ReadOnly = true;
                txtTaxVal.ReadOnly = true;
                txtTotValue.ReadOnly = true;
                txtNetValue.ReadOnly = true;

            }

        }
    }

    protected void grvWarranty_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlFreq = (DropDownList)e.Row.FindControl("ddlServiceFrequency");
            TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
            ddlFreq.DataTextField = dtFrequency.Columns["Name"].ToString();
            ddlFreq.DataValueField = dtFrequency.Columns["Value"].ToString();
            ddlFreq.DataSource = dtFrequency.DefaultView;
            ddlFreq.DataBind();
            ddlFreq.Items.Insert(0, (new ListItem("--Select--", "0")));

            ////if (ddlFreq.Items.Count > 0)
            ////{
            ////    if (ddlFreq.Items.Contains(ddlFreq.Items.FindByValue("1")))
            ////    {
            ////        ddlFreq.Items.RemoveAt(ddlFreq.Items.IndexOf(ddlFreq.Items.FindByValue("1")));
            ////    }
            ////}

            TextBox txtFromDate = (TextBox)e.Row.FindControl("txtFromDate");
            TextBox txtToDate = (TextBox)e.Row.FindControl("txtToDate");
            DropDownList ddlWarrantyType = null;
            if (intProformaID > 0)
            {
                ddlWarrantyType = (DropDownList)e.Row.FindControl("ddlWarrantyType");

                ddlFreq.SelectedValue = dsAsset.Tables[1].Rows[iCount]["Service_Frequency"].ToString();
                ddlWarrantyType.SelectedValue = dsAsset.Tables[1].Rows[iCount]["Warranty_Type"].ToString();
                if (dsAsset.Tables[1].Rows[iCount]["Warranty_From"].ToString() != "")
                {
                    txtFromDate.Text = DateTime.Parse(dsAsset.Tables[1].Rows[iCount]["Warranty_From"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    txtToDate.Text = DateTime.Parse(dsAsset.Tables[1].Rows[iCount]["Warranty_To"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                //txtFromDate.Text = dsAsset.Tables[1].Rows[iCount]["Warranty_From"].ToString();
                //txtToDate.Text = dsAsset.Tables[1].Rows[iCount]["Warranty_To"].ToString();
            }

            txtFromDate.Attributes.Add("readonly", "readonly");
            AjaxControlToolkit.CalendarExtender ceFromDate = e.Row.FindControl("ceFromDate") as AjaxControlToolkit.CalendarExtender;
            ceFromDate.Format = strDateFormat;


            txtToDate.Attributes.Add("readonly", "readonly");
            AjaxControlToolkit.CalendarExtender ceToDate = e.Row.FindControl("ceToDate") as AjaxControlToolkit.CalendarExtender;
            ceToDate.Format = strDateFormat;

            iCount++;

            if (strMode == "Q")
            {
                txtFromDate.ReadOnly = true;
                txtToDate.ReadOnly = true;
                ddlWarrantyType.ClearDropDownList();
                ddlFreq.ClearDropDownList();
                txtRemarks.ReadOnly = true;
                ceFromDate.Enabled = false;
                ceToDate.Enabled = false;
            }
        }

    }

    #endregion

    #region Page Methods

    /// <summary>
    /// to bind LOB and Product details
    /// </summary>

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Entity_Type", "6,8");
        Procparam.Add("@PrefixText", prefixText);

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));

        return suggetions.ToArray();

    }

    private void FunPriBindLOBBranchVendor()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (intProformaID == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }

            //Procparam.Add("@FilterType", "0");Commented by Srivatsan for reusability.
            //Procparam.Add("@FilterOption", "'HP','TL','TE','FT','WC'");
            Procparam.Add("@FilterOption", "'HP','FL','LN','OL','TL','TE'");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Program_ID", "44");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            //Code commented  - "OL" Lob should be loaded - Kuppusamy.B - 23-March-2012 
            //if (ddlLOB.Items.Count > 0)
            //{
            //    if (ddlLOB.Items.Contains(ddlLOB.Items.FindByValue("3")))
            //    {
            //        ddlLOB.Items.RemoveAt(ddlLOB.Items.IndexOf(ddlLOB.Items.FindByValue("3")));
            //    }
            //}
            if (strMode == "M" && strMode == "Q")
            {
                FunPubLoadBranch();
            }
            //Vendor
            //Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //Procparam.Add("@Entity_Type", "'6','8'");
            //ddlVendorCode.BindDataTable(SPNames.S3G_ORG_GetEntityMasterVendor, Procparam, new string[] { "Entity_Master_ID", "Entity_Code", "Entity_Name" });

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@OptionValue", "1");
            ddlRefDocType.BindDataTable("S3G_ORG_GetProformaLookup", Procparam, new string[] { "Value", "Name" });
            if (ddlRefDocType.Items.Count > 0)
            {
                if (ddlRefDocType.Items.Contains(ddlRefDocType.Items.FindByValue("5")))
                {
                    ddlRefDocType.Items.RemoveAt(ddlRefDocType.Items.IndexOf(ddlRefDocType.Items.FindByValue("5")));
                }
            }
        }
        catch (Exception objException)
        {
            lblErrorMessage.Text = objException.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    ////public void checkTaxVatValidation(object sender, ServerValidateEventArgs args)
    ////{
    ////    foreach (GridViewRow grvData in grvAssetDetails.Rows)
    ////    {
    ////        TextBox txtTAX = (TextBox)grvData.FindControl("txtTax");
    ////        TextBox txtVAT = (TextBox)grvData.FindControl("txtVAT");
    ////       if(txtTAX.Text=="")
    ////       {
    ////           custTaxVat.ErrorMessage = "Tax % or Vat % should be mandatory ";
    ////           args.IsValid = false;
    ////           return;
    ////       }

    ////    }
    ////    args.IsValid = true;
    ////}

    /// <summary>
    /// This method is used to display User details
    /// </summary>

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "44");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    public void FunPubLoadBranch()
    {
        //Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //if (intProformaID == 0)
        //{
        //    Procparam.Add("@Is_Active", "1");
        //}
        ////Procparam.Add("@FilterType", "0");Commented by Srivatsan for reusability.
        //Procparam.Add("@User_ID", intUserID.ToString());
        //Procparam.Add("@Program_ID", "44");
        //Procparam.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
        ddlBranch.Clear();
    }
    private void FunGetProformaDetails()
    {

        try
        {
            strProcName = "S3G_ORG_GetProformaAssetDetails";
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Proforma_ID", intProformaID.ToString());
            Procparam.Add("@RefDoc_Type", ddlRefDocType.SelectedValue);
            Procparam.Add("@RefDoc_No", ddlRefDocNo.SelectedValue);
            Procparam.Add("@SLA_No", ddlSLA.SelectedValue);

            dsAsset = Utility.GetTableValues(strProcName, Procparam);
            if ((intProformaID > 0) || (ddlRefDocNo.SelectedValue != "0"))
            {
                grvBookInAsset.DataSource = dsAsset.Tables[0];
                grvBookInAsset.DataBind();

                grvAssetDetails.DataSource = dsAsset.Tables[0];
                grvAssetDetails.DataBind();

                if (PageMode == PageModes.Create)
                {
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@OptionValue", "2");
                    dtFrequency = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", Procparam);
                }
                else
                {
                    dtFrequency = dsAsset.Tables[4];
                }

                grvWarranty.DataSource = dsAsset.Tables[1];
                grvWarranty.DataBind();

                if (dsAsset.Tables[2].Rows.Count > 0 && dsAsset.Tables[2]!=null)
                {
                    //txtAddress1.Text = dsAsset.Tables[2].Rows[0]["Comm_Address1"].ToString();
                    //txtAddress2.Text = dsAsset.Tables[2].Rows[0]["Comm_Address2"].ToString();
                    //txtCity.Text = dsAsset.Tables[2].Rows[0]["Comm_City"].ToString();
                    //txtState.Text = dsAsset.Tables[2].Rows[0]["Comm_State"].ToString();
                    //txtCountry.Text = dsAsset.Tables[2].Rows[0]["Comm_Country"].ToString();
                    //txtPincode.Text = dsAsset.Tables[2].Rows[0]["Comm_PinCode"].ToString();
                    //txtCustCode.Text = dsAsset.Tables[2].Rows[0]["Customer_Code"].ToString();

                    hdnCustomerID.Value = dsAsset.Tables[2].Rows[0]["Customer_ID"].ToString();
                    S3GCustomerAddress1.SetCustomerDetails(dsAsset.Tables[2].Rows[0], true);

                    //string strCustomerCode = "";
                    //strCustomerCode = Utility.SetCustomerAddress(dsAsset.Tables[2].Rows[0]);


                    //txtMobile.Text = dsAsset.Tables[2].Rows[0]["Comm_Mobile"].ToString();
                    //txtTelephone.Text = dsAsset.Tables[2].Rows[0]["Comm_Telephone"].ToString();
                    //txtEmailid.Text = dsAsset.Tables[2].Rows[0]["Comm_EMail"].ToString();                   
                    //txtCustName.Text = dsAsset.Tables[2].Rows[0]["Customer_Name"].ToString();
                    //txtWebSite.Text = dsAsset.Tables[2].Rows[0]["Comm_Website"].ToString();
                    if (dsAsset.Tables[2].Rows[0]["PASARefID"].ToString() != "0")
                    {
                        hdnSLA.Value = dsAsset.Tables[2].Rows[0]["PASARefID"].ToString();
                    }
                }
            }
            if (intProformaID > 0)
            {
                ListItem lst;

                lst = new ListItem(dsAsset.Tables[3].Rows[0]["LOB_Name"].ToString(), dsAsset.Tables[3].Rows[0]["LOB_ID"].ToString());
                ddlLOB.Items.Add(lst);
                ddlLOB.SelectedValue = dsAsset.Tables[3].Rows[0]["LOB_ID"].ToString();
                // Added by Srivatsan
                //FunPubLoadBranch();

                ddlBranch.SelectedValue = dsAsset.Tables[3].Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = dsAsset.Tables[3].Rows[0]["Location"].ToString();

                ddlVendorCode.SelectedValue = dsAsset.Tables[3].Rows[0]["Vendor_ID"].ToString();
                ddlVendorCode.SelectedText = dsAsset.Tables[3].Rows[0]["Entity_Name"].ToString();

                lst = new ListItem(dsAsset.Tables[3].Rows[0]["Ref_Type_Text"].ToString(), dsAsset.Tables[3].Rows[0]["Ref_Doc_Type"].ToString());
                ddlRefDocType.Items.Add(lst);

                ddlRefDocType.SelectedValue = dsAsset.Tables[3].Rows[0]["Ref_Doc_Type"].ToString();
                //ddlRefDocNo.SelectedValue = dsAsset.Tables[3].Rows[0]["Ref_Doc_No"].ToString();
                //FunPriBindRefDocNo();
                //ddlRefDocNo.Items.Insert(0, new ListItem(dsAsset.Tables[3].Rows[0]["RefDocNoText"].ToString(), dsAsset.Tables[3].Rows[0]["Ref_Doc_No"].ToString()));
                ddlRefDocNo.SelectedValue = dsAsset.Tables[3].Rows[0]["Ref_Doc_No"].ToString();
                ddlRefDocNo.SelectedText = dsAsset.Tables[3].Rows[0]["RefDocNoText"].ToString();
                ddlRefDocNo.ToolTip = dsAsset.Tables[3].Rows[0]["RefDocNoText"].ToString();

                if (ddlRefDocNo.SelectedValue == "4")
                {
                    FunBindSLA();
                }
                else
                {
                    lst = new ListItem("--Select--", "0");
                    ddlSLA.Items.Add(lst);
                }

                ddlSLA.SelectedValue = hdnSLA.Value;

                txtProformaNo.Text = dsAsset.Tables[3].Rows[0]["Proforma_No"].ToString();
                txtProformaDate.Text = DateTime.Parse(dsAsset.Tables[3].Rows[0]["Proforma_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                //txtProformaDate.Text = dsAsset.Tables[3].Rows[0]["Proforma_Date"].ToString();
                if (dsAsset.Tables[3].Rows[0]["Book_in_Asset"].ToString() == "0")
                {
                    chkBookInAsset.Checked = false;//(dsAsset.Tables[3].Rows[0]["Book_in_Asset"].ToString());
                }
                else
                {
                    chkBookInAsset.Checked = true;
                }
                txtTaxRegNo.Text = dsAsset.Tables[3].Rows[0]["TaxReg_No"].ToString();
                txtVATNo.Text = dsAsset.Tables[3].Rows[0]["VAT_No"].ToString();
                if (dsAsset.Tables[3].Rows[0]["Proforma_Image"].ToString() == "True" || dsAsset.Tables[3].Rows[0]["Proforma_Image"].ToString() == "1")
                {
                    rdoYes.Checked = true;
                    lnkDownload.Enabled = true;
                    hdnFile.Value = dsAsset.Tables[3].Rows[0]["Scan_Image"].ToString();
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
                //txtVATNo.Text = dsAsset.Tables[3].Rows[0]["VAT_No"].ToString();
                //Scan_Image

                //FunPriBindVendorDet();

                txtVendorName.Text = dsAsset.Tables[3].Rows[0]["Entity_Name"].ToString();
                string strVendorAddress = Utility.SetVendorAddress(dsAsset.Tables[3].Rows[0]);

                S3GVendorAddress.SetCustomerDetails("", strVendorAddress, "",
                dsAsset.Tables[3].Rows[0]["Telephone"].ToString(),
                dsAsset.Tables[3].Rows[0]["Mobile"].ToString(),
                dsAsset.Tables[3].Rows[0]["EMail"].ToString(),
                dsAsset.Tables[3].Rows[0]["Website"].ToString());
                txtTaxRegNo.Text = dsAsset.Tables[3].Rows[0]["Tax_Account_Number"].ToString();
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            dsAsset.Dispose();
        }

    }

    //This is to Update calculation value after postback trigger happened.
    //protected void FunPriCalculateTax(object sender, EventArgs e)
    private void FunPriCalculateTax()
    {
        //string strTextBox = ((TextBox)sender).ClientID;
        //int gRowIndex = Utility.FunPubGetGridRowID("grvAssetDetails", strTextBox);
        //int gRowIndex = 0;
        foreach (GridViewRow grvRow in grvAssetDetails.Rows)
        {
            TextBox txt = (TextBox)grvRow.FindControl("txtUnitValue");
            TextBox txtUnitValue = (TextBox)grvRow.FindControl("txtUnitValue");
            TextBox txtNoofUnit = (TextBox)grvRow.FindControl("txtNoofUnit");
            TextBox txtTax = (TextBox)grvRow.FindControl("txtTax");
            TextBox txtVAT = (TextBox)grvRow.FindControl("txtVAT");
            TextBox txtOthers = (TextBox)grvRow.FindControl("txtOthers");
            TextBox txtVATVal = (TextBox)grvRow.FindControl("txtVATVal");
            TextBox txtTaxVal = (TextBox)grvRow.FindControl("txtTaxVal");
            TextBox txtTotValue = (TextBox)grvRow.FindControl("txtTotValue");
            TextBox txtNetValue = (TextBox)grvRow.FindControl("txtNetValue");

            decimal decTotValue = 0.0m;
            // decimal decOthersValue = 0.0m;
            decimal decTaxValue = 0.0m;
            decimal decVATValue = 0.0m;
            decimal decNETValue = 0.0m;
            if ((txtUnitValue.Text != "") && (txtNoofUnit.Text != ""))
            {
                decTotValue = (Convert.ToDecimal(txtUnitValue.Text) * Convert.ToDecimal(txtNoofUnit.Text));
                txtTotValue.Text = decTotValue.ToString();
            }
            //else
            //{
            //    return;
            //}
            if (txtTax.Text != "")
            {
                decTaxValue = (decTotValue / 100) * Convert.ToDecimal(txtTax.Text);
                txtTaxVal.Text = decTaxValue.ToString();
            }
            if (txtVAT.Text != "")
            {
                decVATValue = (decTotValue / 100) * Convert.ToDecimal(txtVAT.Text);
                txtVATVal.Text = decVATValue.ToString();
            }
            if ((txtVAT.Text != "") && (txtOthers.Text != "") && (txtVAT.Text != "0"))
            {
                decNETValue = decTotValue + Convert.ToDecimal(txtOthers.Text) + decVATValue;
                txtNetValue.Text = decNETValue.ToString();
            }
            if ((txtVAT.Text == "") && (txtOthers.Text != "") && (txtTax.Text != ""))
            {
                decNETValue = decTotValue + Convert.ToDecimal(txtOthers.Text) + decTaxValue;
                txtNetValue.Text = decNETValue.ToString();
            }
        }
    }

    #region "File Upload"

    public bool FunFileLoad(bool bUpload)
    {

        AjaxControlToolkit.AsyncFileUpload fileScanImage = (AjaxControlToolkit.AsyncFileUpload)tbProforma.FindControl("fileScanImage");

        ////if ((hdnFileUploaded.Value == "1") && (strMode == "M") && (fileScanImage.HasFile == false))
        ////{
        ////    return true;
        ////}

        if ((hdnFileUploaded.Value == "1") && (strMode == "M") && (hdnFileName.Value == ""))
        {
            return true;
        }

        if (hdnFileName.Value != "")
        {
            try
            {
                //System.Text.RegularExpressions.Regex strFileValidationExpression = new System.Text.RegularExpressions.Regex(@"\.txt$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                strFileName = fileScanImage.FileName;
                string strApplicationPath = Server.MapPath(".");
                string strFilePath = strApplicationPath.Replace("\\Origination", "") + "\\Data\\Proforma";

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }
                strFilePath = strFilePath + "\\" + strFileName;
                if (File.Exists(strFilePath))
                {
                    //lblErrorMessage.Text = "Already the same file name(" + strFileName + ") exists in the target path";
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
            Utility.FunShowAlertMsg(this.Page, "Browse a file to upload");
            return false;
        }
        return true;
    }

    #endregion

    private void FunPriClear(bool bClearAll)
    {
        try
        {
            if (bClearAll)
            {
                ddlLOB.SelectedIndex = 0;
                //ddlBranch.SelectedIndex = 0;
                //ddlBranch.Items.Clear();
                ddlBranch.Clear();
                //ddlVendorCode.SelectedIndex = 0;
                ddlVendorCode.Clear();
                ddlRefDocType.SelectedIndex = 0;
                //if (ddlRefDocNo.Items.Count > 0)
                //    ddlRefDocNo.SelectedIndex = 0;
                ddlRefDocNo.Clear();

                txtTaxRegNo.Text = "";
                txtVATNo.Text = "";
                txtProformaNo.Text = "";
                txtProformaDate.Text = "";
                chkBookInAsset.Checked = false;
                FunPriClearVendor();
                lblDisplayFile.Visible = false;

            }
            //AjaxControlToolkit.AsyncFileUpload fileScanImage = (AjaxControlToolkit.AsyncFileUpload)tbProforma.FindControl("fileScanImage");
            //ddlRefDocType.SelectedIndex = 0;
            FunPriClearVendor();
            rdoNo.Checked = false;
            rdoYes.Checked = true;
            fileScanImage.Enabled = true;

            lblDisplayFile.Text = "";
            lblDisplayFile.Text = string.Empty;

            //txtCustName.Text = "";
            //txtAddress.Text = "";
            //txtCustCode.Text = "";
            //txtMobile.Text = "";
            //txtEmailid.Text = "";
            //txtWebSite.Text = "";
            S3GCustomerAddress1.ClearCustomerDetails();

            grvAssetDetails.DataSource = null;
            grvAssetDetails.DataBind();

            grvBookInAsset.DataSource = null;
            grvBookInAsset.DataBind();

            grvWarranty.DataSource = null;
            grvWarranty.DataBind();
            //ddlRefDocType.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriClearVendor()
    {

        //ddlVendorCode.SelectedIndex = 0;
        ddlVendorCode.Clear();
        txtVendorName.Text = "";
        S3GVendorAddress.ClearCustomerDetails();

        //txtVenAddress.Text = "";
        //txtVenMobile.Text = "";
        //txtVenEmailid.Text = "";
        //txtVendorName.Text = "";
        //txtVenWebSite.Text = "";
    }

    private void FunPriBindVendorDet()
    {
        try
        {
            string strVendorAddress = "";
            strProcName = SPNames.S3G_ORG_GetEntityMasterVendor;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Entity_ID", ddlVendorCode.SelectedValue);
            DataTable dtVendorDet = Utility.GetDefaultData(strProcName, Procparam);
            txtVendorName.Text = dtVendorDet.Rows[0]["Entity_Name"].ToString();
            strVendorAddress = Utility.SetVendorAddress(dtVendorDet.Rows[0]);

            S3GVendorAddress.SetCustomerDetails("", strVendorAddress, "",
            dtVendorDet.Rows[0]["Telephone"].ToString(),
            dtVendorDet.Rows[0]["Mobile"].ToString(),
            dtVendorDet.Rows[0]["EMail"].ToString(),
            dtVendorDet.Rows[0]["Website"].ToString());

            //txtVenMobile.Text = dtVendorDet.Rows[0]["Mobile"].ToString();
            //txtVenEmailid.Text = dtVendorDet.Rows[0]["EMail"].ToString();
            //txtVenWebSite.Text = dtVendorDet.Rows[0]["Website"].ToString();

            txtTaxRegNo.Text = dtVendorDet.Rows[0]["Tax_Account_Number"].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FunPriGenerateProformaXMLDet()
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

    private void FunBindSLA()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Type", "Type3");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            //Changed by Palani Kumar.A on 17/01/2014 for Dispaly with Customer Name.
            //Procparam.Add("@Param2", ddlRefDocNo.SelectedText.Trim());
            string strDocNo = string.Empty;
            if (!string.IsNullOrEmpty(ddlRefDocNo.SelectedText.ToString()))
            {
                //strDocNo = ddlRefDocNo.SelectedText.Substring(0, ddlRefDocNo.SelectedText.Trim().ToString().LastIndexOf("-") - 1).ToString();
                Procparam.Add("@Param2", ddlRefDocNo.SelectedText.Trim());
            }
            else
            {
                Procparam.Add("@Param2", ddlRefDocNo.SelectedText.Trim());
            }
            //----------End 
            ddlSLA.BindDataTable(SPNames.S3G_LOANAD_GetPLASLA_List, Procparam, new string[] { "PA_SA_REF_ID", "SANum" });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
    {

        FunGetProformaDetails();
        hdnSLA.Value = ddlSLA.SelectedValue;
    }

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
                //txtProformaNo.ReadOnly = true;
                btnClear.Enabled = false;
                break;

            case -1:// Query Mode


                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }

                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    ddlBranch.ReadOnly = true;
                    ddlRefDocType.ClearDropDownList();
                    ddlRefDocNo.Enabled = false;
                    //ddlVendorCode.ClearDropDownList();
                    
                }
                ddlVendorCode.ReadOnly = true;
                ddlLOB.Enabled = true;
                ddlBranch.Enabled = true;
                ddlRefDocType.Enabled = true;
                ddlRefDocNo.Enabled = false;
                rdoYes.Enabled = false;
                rdoNo.Enabled = false;
                CalendarExtender1.Enabled = false;
                txtProformaNo.ReadOnly = true;
                txtTaxRegNo.ReadOnly = true;
                txtVATNo.ReadOnly = true;
                fileScanImage.Enabled = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;

                break;
        }

    }

    ////Code end


    #endregion

}



