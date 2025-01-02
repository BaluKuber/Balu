#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Loan Admin
/// Screen Name         :   S3GLoanAdAssetAcquisition
/// Created By          :   Suresh P
/// Created Date        :   31-Aug-2010
/// Purpose             :   Asset Acquisition
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Web.UI.WebControls;
using LoanAdminAccMgtServicesReference;
using S3GBusEntity;
using AssetMgtServicesReference;
using S3GBusEntity.LoanAdmin;
using System.Web.Security;
#endregion

public partial class S3GLoanAdAssetAcquisition : ApplyThemeForProject
{ 
    #region Declaration

    Dictionary<string, string> ObjDictParams = null;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode SerMode = SerializationMode.Binary;
    public static S3GLoanAdAssetAcquisition obj_Page;
    string intROIRuleMasterID;
    int intErrCode = 0;
    int intCompanyID, intUserID;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    string strRedirectPageLandView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ASA';";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdAssetAcquisition_Add.aspx';";

    string strRedirectPage = "~/LoanAdmin/S3GLoanAdAssetAcquisition_Add.aspx";
    string strRedirectPageView = "S3gLoanAdTransLander.aspx?Code=ASA";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    string strXMLAssetAcquisitionDet = "<Root><Details Desc='0' /></Root>";
    string strDateFormat;
    StringBuilder strbAssetAcquisitionDet = new StringBuilder();

    AssetMgtServicesClient ObjLoanAdminAccMgtServicesClient = null;
    AssetMgtServices.S3G_LOANAD_AssetAcquisitionDataTable ObjS3G_LOANAD_AssetAcquisitionDataTable = null;
    AssetMgtServices.S3G_LOANAD_AssetAcquisitionRow ObjS3G_LOANAD_AssetAcquisitionRow = null;
    AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable ObjS3G_LOANAD_LeaseAssetRegisterDataTable = null;
    AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterRow ObjS3G_LOANAD_LeaseAssetRegisterRow = null;

    DataTable dtAssetAcquisition = null;
    DataTable dtAssetAcquisition_Availability = null;

    #endregion

    #region Page Loading

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            cexDate.Format = strDateFormat;
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket formTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                strMode = Request.QueryString.Get("qsMode");
                if (formTicket != null)
                {
                    intROIRuleMasterID = formTicket.Name;
                }
            }
            AssetAcquisition_Availability.Enabled = true;
            if (Request.QueryString["qsViewId"] == null)
            {
                AssetAcquisition_Availability.Enabled = false;
            }

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            if (!IsPostBack)
            {
                AssetAcquisition.ActiveTab = AssetAcquisition_Create;
                txtLAAEDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtLAAEDate.ClientID + "','" + strDateFormat + "',true,  false);");
                //txtLAAEDate.Attributes.Add("readonly", "readonly");
                txtLAAEDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                //ObjDictParams = new Dictionary<string, string>();
                //ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                //ObjDictParams.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
                //if (PageMode == PageModes.Create)
                //{
                //    ObjDictParams.Add("@Is_Active", "1");
                //}
                //ObjDictParams.Add("@FilterOption", "'OL'");
                //ObjDictParams.Add("@Program_ID", "66");
                //ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, ObjDictParams, false, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });

                //if (ddlLineofBusiness.Items.Count == 0)
                //{
                //    Utility.FunShowAlertMsg(this, "The Line of Bussiness Operating Lease is not Activated.", strRedirectPageView);
                //}

                //ListItem lit, litOL;
                //litOL = ddlLineofBusiness.Items.FindByValue("3");
                //lit = new ListItem("--Select--", "0");
                //ddlLineofBusiness.Items.Clear();
                //ddlLineofBusiness.Items.Add(lit);
                //ddlLineofBusiness.Items.Add(litOL);



                /*        Procparam.Add("@LOB_ID", Convert.ToString(ComboBoxLOBSearch.SelectedValue));
                        Procparam.Add("@Branch_ID", Convert.ToString(ComboBoxBranchSearch.SelectedValue));
                        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                        Procparam.Add("@User_ID", Convert.ToString(intUserID));
                        cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetAssetAcquisitionID, Procparam, true, "--Select--", new string[] { "ID", "Acquisition_No" });
                        */
                /*
                string[] strLOBCodeToFilter = new string[] { "OL" };
                FunPriFilterAndLoadLOB();
                ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                ObjDictParams.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
                ObjDictParams.Add("@Is_Active", "1");
                ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, Procparam, true, "--Select--", new string[] { "ID", "Acquisition_No" });
                */
                //cmbDocumentNumberSearch.BindDataTable(SPNames.S3G_LOANAD_GetDepreciatedSJV, Procparam, true, "--Select--", new string[] { "ID", "System_JV_Number" });


              //  FunPriLookupForLOB();
               // FunProLoadBranch(ObjUserInfo.ProCompanyIdRW);
                FunPriInsertAssetAcquisitionDataTable("-1", "-1","-1", "", "");

                if ((intROIRuleMasterID != "") && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if ((intROIRuleMasterID != "") && (strMode == "Q")) // Query // Modify
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriLoadLOB();
                    FunPriDisableControls(0);
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLookupForLOB()
    {
        try
        {
            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            //Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            //Procparam.Add("@LOB_ID", ddlLineofBusiness .SelectedValue.ToString());
            //if (PageMode == PageModes.Create)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
            //ddlBranch.AddItemToolTip();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPriLoadLOB()
    {  try
        {
        ObjDictParams = new Dictionary<string, string>();
        ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        ObjDictParams.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
        if (PageMode == PageModes.Create)
        {
            ObjDictParams.Add("@Is_Active", "1");
        }
        ObjDictParams.Add("@FilterOption", "'OL'");
        ObjDictParams.Add("@Program_ID", "66");
        ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, ObjDictParams, false, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });

        if (ddlLineofBusiness.Items.Count == 0)
        {
            Utility.FunShowAlertMsg(this, "The Line of Bussiness Operating Lease is not Activated.", strRedirectPageView);
        }
        }
    catch (Exception ex)
    {
        lblErrorMessage.Text = ex.Message;
    }
    }

    #endregion

    #region Page Events

    protected void gvAssetAcquisitionAvailability_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblLANBookingFromDate = (Label)e.Row.FindControl("lblLANBookingFromDate");
                if (lblLANBookingFromDate.Text.Equals(""))
                {
                    lblLANBookingFromDate.Text = "";
                }
                else
                {
                    lblLANBookingFromDate.Text = DateTime.Parse(lblLANBookingFromDate.Text.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }

                Label lblLANBookingToDate = (Label)e.Row.FindControl("lblLANBookingToDate");
                if (lblLANBookingToDate.Text.Equals(""))
                {
                    lblLANBookingToDate.Text = "";
                }
                else
                {
                    lblLANBookingToDate.Text = DateTime.Parse(lblLANBookingToDate.Text.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }

                if (PageMode == PageModes.Modify)
                {
                    Label lblCanEdit = (Label)e.Row.FindControl("lblCanEdit");
                    DropDownList ddlPerformance = (DropDownList)e.Row.FindControl("ddlPerformance");
                    Label lblPerforming = (Label)e.Row.FindControl("lblPerforming");
                    Label lblStatusCode = (Label)e.Row.FindControl("lblStatusCode");
                    ObjDictParams = new Dictionary<string, string>();
                    ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                    ddlPerformance.BindDataTable("S3G_LOANAD_GetAssetAcquisitionStatus", ObjDictParams, false, new string[] { "Lookup_Code", "Lookup_Description" });
                    ddlPerformance.SelectedValue = lblStatusCode.Text;
                    if (lblCanEdit.Text == "1")
                    {
                        lblPerforming.Visible = false;
                        ddlPerformance.Visible = true;
                    }
                    else
                    {
                        lblPerforming.Visible = true;
                        ddlPerformance.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

     protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlAssetCode = gvAssetAcquisition.FooterRow.FindControl("ddlAssetCode") as DropDownList;
            DropDownList ddlAssetDescription = gvAssetAcquisition.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
            DropDownList ddlAssetType = gvAssetAcquisition.FooterRow.FindControl("ddlAssetType") as DropDownList;
            TextBox txtAssetDescription = gvAssetAcquisition.FooterRow.FindControl("txtAssetDescription") as TextBox;

            ddlAssetCode.Items.Clear();
            ddlAssetDescription.Items.Clear();
            txtAssetDescription.Text = "";

            if (ddlAssetType.SelectedValue != "0")
            {
                ObjDictParams = new Dictionary<string, string>();
                ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                ObjDictParams.Add("@Asset_Type", ddlAssetType.SelectedValue.ToString());
                if (chkDOGenerated.Checked)
                {
                    ObjDictParams.Add("@FromDO", "1");
                    DataTable dtAsset = (DataTable)ViewState["DT_AssetAcquisition"];
                    ObjDictParams.Add("@Inv_IDs", dtAsset.FunPubFormXml());
                }
                ddlAssetCode.BindDataTable("S3G_LOANAD_GetAssetLists", ObjDictParams, new string[] { "Asset_ID", "Asset_Code" });
                ddlAssetDescription.BindDataTable("S3G_LOANAD_GetAssetLists", ObjDictParams, new string[] { "Asset_ID", "Asset_Description" });
                ObjDictParams = null;
            }

            ddlAssetType.Focus();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ddlAssetCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlAssetCode = gvAssetAcquisition.FooterRow.FindControl("ddlAssetCode") as DropDownList;
            DropDownList ddlAssetDescription = gvAssetAcquisition.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
            TextBox txtAssetDescription = gvAssetAcquisition.FooterRow.FindControl("txtAssetDescription") as TextBox;

            if (Convert.ToInt32(ddlAssetCode.SelectedValue) > 0)
            {
                txtAssetDescription.Text = ddlAssetDescription.Items.FindByValue(ddlAssetCode.SelectedValue).Text;
            }
            else
                txtAssetDescription.Text = "";
            //gvAssetAcquisition.ShowFooter = true;
            //gvAssetAcquisition.FooterRow.Visible = true;

            ddlAssetCode.Focus();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void gvAssetAcquisition_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[0].Style.Add("position", "relative");
                //e.Row.Cells[0].BorderWidth = 0;

                Label lblAssetType = (Label)e.Row.FindControl("lblAssetType");
                if (chkDOGenerated.Checked)
                {
                    lblAssetType.Text = "Invoice Number";
                }
                else
                {
                    lblAssetType.Text = "Asset Type";
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlAssetCode = e.Row.FindControl("ddlAssetCode") as DropDownList;
                DropDownList ddlAssetDescription = e.Row.FindControl("ddlAssetDescription") as DropDownList;
                DropDownList ddlAssetType = e.Row.FindControl("ddlAssetType") as DropDownList;

                ObjDictParams = new Dictionary<string, string>();
                if (!chkDOGenerated.Checked)
                {
                    ObjDictParams.Add("@Type", "1");
                    ddlAssetType.BindDataTable("S3G_ORG_GetAssetCategory", ObjDictParams, new string[] { "ID", "Name" });
                }
                else
                {
                    ObjDictParams.Add("@Company_ID", CompanyId.ToString());
                    ObjDictParams.Add("@Location_ID", ddlBranch.SelectedValue.ToString());

                    DataTable dtAsset = (DataTable)ViewState["DT_AssetAcquisition"];
                    //string strDOs = "0";
                    //if (dtAsset != null)
                    //{
                    //    foreach (DataRow dRow in dtAsset.Rows)
                    //    {
                    //        if (!string.IsNullOrEmpty(dRow["Asset_Type_ID"].ToString()))
                    //        {
                    //            strDOs = strDOs + "," + dRow["Asset_Type_ID"].ToString();
                    //        }
                    //    }
                    //}

                    //ObjDictParams.Add("@DO_IDs", strDOs);

                    ObjDictParams.Add("@DO_IDs", dtAsset.FunPubFormXml());

                    //Changed By Thangam M on 12/Nov/2011 to get Asset from Invoice instead of DO 
                    //ddlAssetType.BindDataTable("S3G_ORG_GetAssetFromDO", ObjDictParams, new string[] { "DeliveryInstruction_ID", "DeliveryInstruction_No" });
                    //End

                    ddlAssetType.BindDataTable("S3G_ORG_GetAssetFromDO", ObjDictParams, new string[] { "Invoice_ID", "Invoice_No" });
                }

                //ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                //ddlAssetCode.BindDataTable("S3G_LOANAD_GetAssetLists", ObjDictParams, new string[] { "Asset_ID", "Asset_Code" });
                //ddlAssetDescription.BindDataTable("S3G_LOANAD_GetAssetLists", ObjDictParams, new string[] { "Asset_ID", "Asset_Description" });
                //ObjDictParams = null;
                ddlAssetType.Focus();
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton lnkEdit = e.Row.FindControl("lnkEdit") as LinkButton;
                LinkButton lnkRemove = e.Row.FindControl("lnkRemove") as LinkButton;

                if (strMode == "Q" || strMode == "M")
                {
                    //lnkEdit.Enabled = false;
                    lnkRemove.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void gvAssetAcquisition_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string strAssetID, strAssetCode, strAssetDescription;

            if (e.CommandName == "Add")
            {
                DropDownList ddlAssetCode = gvAssetAcquisition.FooterRow.FindControl("ddlAssetCode") as DropDownList;
                TextBox txtAssetDescription = gvAssetAcquisition.FooterRow.FindControl("txtAssetDescription") as TextBox;
                DropDownList ddlAssetType = gvAssetAcquisition.FooterRow.FindControl("ddlAssetType") as DropDownList;

                if (ddlAssetType.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select the Invoice Number");
                    ddlAssetType.Focus();
                    return;
                }
                strAssetID = ddlAssetCode.SelectedValue.ToString();
                strAssetCode = ddlAssetCode.SelectedItem.Text.ToString();
                strAssetDescription = txtAssetDescription.Text;
                if (strAssetID.Equals("0"))
                {
                    Utility.FunShowAlertMsg(this.Page, "Select the Asset Code");
                    ddlAssetCode.Focus();
                    return;

                }
                if (Convert.ToInt32(ddlAssetCode.SelectedValue) == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Select Asset information");
                    ddlAssetCode.Focus();
                    return;
                }

                FunPriInsertAssetAcquisitionDataTable(strAssetID, ddlAssetType.SelectedItem.Text, ddlAssetType.SelectedValue.ToString(), strAssetCode, strAssetDescription);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void gvAssetAcquisition_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtAssetAcquisition = FunPriGetAssetAcquisitionDataTable();
            dtAssetAcquisition.Rows.RemoveAt(e.RowIndex);

            ViewState["DT_AssetAcquisition"] = dtAssetAcquisition;

            dtAssetAcquisition = FunPriGetAssetAcquisitionDataTable();
            if (dtAssetAcquisition.Rows.Count == 0)
            {
                FunPriInsertAssetAcquisitionDataTable("-1", "-1","-1", "", "");
            }
            else
            {
                FunPubBindAssetAcquisition(dtAssetAcquisition);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region Page Methos

    private void FunPriValidationDefault(bool blnFlag, int intModeID)
    {
        try
        {
            if ((intModeID == 1) || (intModeID == -1))
            {
                FunGetAssetAcquisitionDetails();        // Get Asset Acquisition Details
            }
            bool blnIsReadOnly = (intModeID == -1) ? true : false;
            FunPriControlEnable(intModeID, blnIsReadOnly);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunGetAssetAcquisitionDetails()
    {
        gvAssetAcquisition.DataSource = null;
        gvAssetAcquisition.DataBind();
        gvAssetAcquisition.Visible = false;

        ObjLoanAdminAccMgtServicesClient = new AssetMgtServicesClient();
        try
        {
            ObjS3G_LOANAD_LeaseAssetRegisterDataTable = new AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable();
            ObjS3G_LOANAD_LeaseAssetRegisterRow = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.NewS3G_LOANAD_LeaseAssetRegisterRow();
            //ObjS3G_LOANAD_LeaseAssetRegisterRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
            //ObjS3G_LOANAD_LeaseAssetRegisterRow.Branch_ID = 1;
            //ObjS3G_LOANAD_LeaseAssetRegisterRow.LOB_ID = 3;
            ObjS3G_LOANAD_LeaseAssetRegisterRow.Acquisition_No = intROIRuleMasterID; // "2010-2011/AAQ/3";
            ObjS3G_LOANAD_LeaseAssetRegisterDataTable.AddS3G_LOANAD_LeaseAssetRegisterRow(ObjS3G_LOANAD_LeaseAssetRegisterRow);

            byte[] byteAssetAcquisition = ObjLoanAdminAccMgtServicesClient.FunPubQueryAssetAcquisition(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_LeaseAssetRegisterDataTable, SerMode));

            ObjS3G_LOANAD_LeaseAssetRegisterDataTable = new AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable();
            ObjS3G_LOANAD_LeaseAssetRegisterDataTable = (AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable)ClsPubSerialize.DeSerialize(byteAssetAcquisition, SerMode, typeof(AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable));

            ViewState["DT_AssetAcquisition_Availability"] = ObjS3G_LOANAD_LeaseAssetRegisterDataTable;

            //dtAssetAcquisition_Availability = FunPriGetAssetAcquisitionAvailabilityDataTable();
            //FunPubBindAssetAcquisitionAvailability(dtAssetAcquisition_Availability);


            gvAssetAcquisition_View.DataSource = ObjS3G_LOANAD_LeaseAssetRegisterDataTable;
            gvAssetAcquisition_View.DataBind();


            gvAssetAcquisitionAvailability.DataSource = ObjS3G_LOANAD_LeaseAssetRegisterDataTable;
            gvAssetAcquisitionAvailability.DataBind();

            if (ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows.Count > 0)
            {
                ddlLineofBusiness.Items.Add(new ListItem(ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["LOB_Name"].ToString(), ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["LOB_ID"].ToString()));
                ddlLineofBusiness.ToolTip = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["LOB_Name"].ToString();
              //  ddlBranch.SelectedValue = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["Location_ID"].ToString();
                
                ddlBranch.SelectedText= ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["Location_Name"].ToString();
                ddlBranch.SelectedValue = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["Location_ID"].ToString();
                ddlBranch.ToolTip = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["Location_Name"].ToString();

                txtLAAEDate.Text = DateTime.Parse(ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["Acquisition_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                txtAssetAcquisitionNO.Text = ObjS3G_LOANAD_LeaseAssetRegisterDataTable.Rows[0]["Acquisition_No"].ToString();
            }

        }
        catch (FaultException<AssetMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjLoanAdminAccMgtServicesClient.Close();
        }
    }

    private void FunPriControlEnable(int intModeID, bool blnIsReadOnly)
    {
        try
        {
            string strProperty = "readonly";
            ///Set readonly property to the controls
            if (blnIsReadOnly)
            {
                ddlBranch.ReadOnly = true;
                //ddlLineofBusiness.ClearDropDownList();
                //ddlBranch.ClearDropDownList();
            }
            else
            {
            }


            bool blnFlag = (intModeID > 0) ? false : true;
            if (!blnFlag)
            {
                ddlBranch.ReadOnly = true;
                //ddlLineofBusiness.ClearDropDownList();
                //ddlBranch.ClearDropDownList();
            }
            ///Set Enable or disable to the controls
            //ddlLineofBusiness.Enabled = blnFlag;
            //ddlBranch.Enabled = blnFlag;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode
                    {
                        FunPriValidationDefault(true, intModeID);

                        //txtRecoveryPatternYear1.Text = "0.00";
                        //txtRecoveryPatternYear2.Text = "0.00";
                        //txtRecoveryPatternYear3.Text = "0.00";
                        //txtRecoveryPatternYearRest.Text = "0.00";

                        //txtRecoveryPatternYear1.Enabled = true;
                        //txtRecoveryPatternYear2.Enabled = false;
                        //txtRecoveryPatternYear3.Enabled = false;
                        //txtRecoveryPatternYearRest.Enabled = false;

                        ////txtRate.Text = "0.0000";
                        //FunPriSetControlAttributes();
                        //FunPriRateTypeDropdown(false);
                        ////FunPriRateTypeMandotary(false);
                        //FunPriFillRateTypeDropdown(false, false);
                        //FunPriRatePatternDropdown(false, false, false, false, false);
                        //FunPriTimeDropdown(false, false, false, false);
                        //FunPriFrequencyDropdown(false, false, false, false, false, false, false);
                        //FunPriRepaymentMode(false, false, false, false, false);
                        //FunPriEnableIRRRest(false);
                        //FunPriIntrestCalculationDropdown(false, false, false, false, false);
                        //FunPriIntrestLevyDropdown(false, false, false, false, false);
                        //FunPriInterestCalculationMandotary(false);
                        //FunPriRecoveryPatternMandotary(false);
                        //FunPriInsuranceDropdown(false);
                        //FunPriResidualValueDropdown(false);
                        //FunPriMarginDropdown(false, true);

                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                        //chkActive.Checked = true;
                        btnClear.Enabled = true;
                        //chkActive.Enabled = false;
                        ddlLineofBusiness.Focus();
                        lblAssetAcquisitionNO.Visible = false;
                        txtAssetAcquisitionNO.Visible = false;
                        if (!bCreate)
                        {
                            btnSave.Enabled = false;
                        }

                        //Command By Thangam on 17/Nov/2011 to show assets based on Invoice
                        //lblDOGenerated.Visible = chkDOGenerated.Visible = true;
                        //End here

                        break;
                    }
                case 1: // Modify Mode
                    {
                        FunPriValidationDefault(false, intModeID);
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                        btnClear.Enabled = false;
                        //chkActive.Enabled = true;
                        gvAssetAcquisition.ShowFooter = false;
                        gvAssetAcquisition.FooterRow.Visible = false;
                        txtLAAEDate.ReadOnly = true;
                        txtLAAEDate.Attributes.Remove("onblur");
                        if (!bModify)
                        {
                            btnSave.Enabled = false;
                        }
                        cexDate.Enabled = false;
                        break;
                    }
                case -1:// Query Mode
                    {
                        if (!bQuery)
                        {
                            Response.Redirect(strRedirectPage,false);
                        }

                        FunPriValidationDefault(false, intModeID);
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                        gvAssetAcquisition.ShowFooter = false;
                        gvAssetAcquisition.FooterRow.Visible = false;
                        //chkActive.Enabled = false;
                        btnClear.Enabled = false;
                        btnSave.Enabled = false;
                        cexDate.Enabled = false;
                        txtLAAEDate.ReadOnly = true;
                        txtLAAEDate.Attributes.Remove("onblur");
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region AcqAvailability

    private DataTable FunPriGetAssetAcquisitionAvailabilityDataTable()
    {
        try
        {
            dtAssetAcquisition_Availability = (DataTable)ViewState["DT_AssetAcquisition_Availability"];
            return dtAssetAcquisition_Availability;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPubBindAssetAcquisitionAvailability(DataTable dtWorkflow)
    {
        try
        {
            gvAssetAcquisitionAvailability.DataSource = dtWorkflow;
            gvAssetAcquisitionAvailability.DataBind();
            if (dtWorkflow.Rows[0]["Asset_Serial_Number"].Equals("0"))
            {
                gvAssetAcquisition.Rows[0].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region AcqDataTable

    private DataTable FunPriGetAssetAcquisitionDataTable()
    {
        try
        {
            DataRow drEmptyRow;
            if (ViewState["DT_AssetAcquisition"] == null)
            {
                dtAssetAcquisition = new DataTable();
                /*
                DataColumn dcNew = null;
                dcNew = new DataColumn("Asset_Serial_Number", typeof(Int32));
                dcNew.AutoIncrement = true;
                dcNew.AutoIncrementSeed = 1;
                dcNew.AutoIncrementStep = 1;
                dtAssetAcquisition.Columns.Add(dcNew);
                */
                dtAssetAcquisition.Columns.Add("Asset_Serial_Number");
                dtAssetAcquisition.Columns.Add("Asset_ID");
                dtAssetAcquisition.Columns.Add("Asset_Type");
                dtAssetAcquisition.Columns.Add("Asset_Type_ID");
                dtAssetAcquisition.Columns.Add("Asset_Code");
                dtAssetAcquisition.Columns.Add("Asset_Description");
                /*
                dtAssetAcquisition.Columns.Add("Lease_Asset_No");
                dtAssetAcquisition.Columns.Add("Asset_Value");
                dtAssetAcquisition.Columns.Add("Vendor_Code");
                dtAssetAcquisition.Columns.Add("Invoice_No");
                dtAssetAcquisition.Columns.Add("Invoice_Date");
                dtAssetAcquisition.Columns.Add("AINS_No");
                dtAssetAcquisition.Columns.Add("Policy_No");
                */
                ViewState["DT_AssetAcquisition"] = dtAssetAcquisition;
            }

            dtAssetAcquisition = (DataTable)ViewState["DT_AssetAcquisition"];
            /*
            if (dtAssetAcquisition.Rows.Count == 0)
            {
                drEmptyRow = dtAssetAcquisition.NewRow();
                drEmptyRow["Asset_Serial_Number"] = "0";
                dtAssetAcquisition.Rows.Add(drEmptyRow);
            }
            */
            return dtAssetAcquisition;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriInsertAssetAcquisitionDataTable(string strAsset_ID, string strAssetType, string strAssetTypeID, string strAssetCode, string strAssetDescription)
    {
        try
        {
            //if (strAsset_ID.Equals("-1"))
            //{
            //    ViewState["DT_AssetAcquisition"] = null;
            //}

            DataRow drEmptyRow;
            dtAssetAcquisition = FunPriGetAssetAcquisitionDataTable();

            if (strAsset_ID.Equals("-1"))
            {
                if (dtAssetAcquisition.Rows.Count == 0)
                {
                    drEmptyRow = dtAssetAcquisition.NewRow();
                    drEmptyRow["Asset_Serial_Number"] = "0";
                    dtAssetAcquisition.Rows.Add(drEmptyRow);
                }
            }
            else
            {
                drEmptyRow = dtAssetAcquisition.NewRow();
                //drEmptyRow["Asset_Serial_Number"] = Convert.ToInt32(dtAssetAcquisition.Rows[dtAssetAcquisition.Rows.Count - 1]["Asset_Serial_Number"]) + 1;
                drEmptyRow["Asset_Serial_Number"] = gvAssetAcquisition.Rows.Count + 1;
                drEmptyRow["Asset_ID"] = strAsset_ID;
                drEmptyRow["Asset_Type"] = strAssetType;
                drEmptyRow["Asset_Type_ID"] = strAssetTypeID;
                drEmptyRow["Asset_Code"] = strAssetCode;
                drEmptyRow["Asset_Description"] = strAssetDescription;
                dtAssetAcquisition.Rows.Add(drEmptyRow);
            }

            if (dtAssetAcquisition.Rows.Count > 1)
            {
                if (dtAssetAcquisition.Rows[0]["Asset_Serial_Number"].Equals("0"))
                {
                    dtAssetAcquisition.Rows[0].Delete();
                    //gvWorkFlowSequence.Rows[0].Visible = false;
                }
            }

            ViewState["DT_AssetAcquisition"] = dtAssetAcquisition;

            dtAssetAcquisition = FunPriGetAssetAcquisitionDataTable();
            FunPubBindAssetAcquisition(dtAssetAcquisition);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPubBindAssetAcquisition(DataTable dtWorkflow)
    {
        try
        {
            gvAssetAcquisition.DataSource = dtWorkflow;
            gvAssetAcquisition.DataBind();
            if (dtWorkflow.Rows[0]["Asset_Serial_Number"].Equals("0"))
            {
                gvAssetAcquisition.Rows[0].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    private bool FunPriGenerateAssetAcquisitionXMLDet()
    {
        try
        {
            dtAssetAcquisition = (DataTable)ViewState["DT_AssetAcquisition"];

            if (dtAssetAcquisition.Rows.Count == 1)
            {
                if (dtAssetAcquisition.Rows[0]["Asset_Serial_Number"].ToString().Equals("0") && PageMode == PageModes.Create) 
                {
                    return false;
                }
            }
            strbAssetAcquisitionDet.Append("<Root>");
            foreach (DataRow drow in dtAssetAcquisition.Rows)
            {
                strbAssetAcquisitionDet.Append("<Details ");
                strbAssetAcquisitionDet.Append(" Asset_ID = '" + drow["Asset_ID"].ToString() + "'");
                strbAssetAcquisitionDet.Append(" Asset_Serial_Number = '" + drow["Asset_Serial_Number"].ToString() + "'");

                if (chkDOGenerated.Checked)
                {
                    strbAssetAcquisitionDet.Append(" DO_ID = '" + drow["Asset_Type_ID"].ToString() + "'");
                }

                /*strbAssetAcquisitionDet.Append(" Asset_Value = '" + drow["Asset_Value"].ToString() + "'");
                strbAssetAcquisitionDet.Append(" Vendor_ID = '" + drow["Vendor_ID"].ToString() + "'");
                strbAssetAcquisitionDet.Append(" Invoice_No = '" + drow["Invoice_No"].ToString() + "'");
                strbAssetAcquisitionDet.Append(" Invoice_Date = '" + drow["Invoice_Date"].ToString() + "'");*/
                strbAssetAcquisitionDet.Append(" />");
            }
            strbAssetAcquisitionDet.Append("</Root>");
            strXMLAssetAcquisitionDet = strbAssetAcquisitionDet.ToString();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private string FunPriGenerateStatusXMLDet()
    {
        try
        {
            string strReturn = "";
            StringBuilder XML = new StringBuilder();
            DataTable dtAssetAvailability = (DataTable)ViewState["DT_AssetAcquisition_Availability"];

            XML.Append("<Root>");
            for (int i = 0; i <= dtAssetAvailability.Rows.Count - 1; i++)
            {
                if (dtAssetAvailability.Rows[i]["CanEdit"].ToString() == "1")
                {
                    XML.Append("<Details ");
                    XML.Append(" Lease_Asset_No = '" + ((Label)gvAssetAcquisitionAvailability.Rows[i].FindControl("lblLeaseAssetNo")).Text + "'");
                    XML.Append(" Performing_Status = '" + ((DropDownList)gvAssetAcquisitionAvailability.Rows[i].FindControl("ddlPerformance")).SelectedValue + "'");
                    XML.Append(" />");
                }
            }
            XML.Append("</Root>");
            strReturn = XML.ToString();
            return strReturn;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void FunProLoadBranch(int intCompanyid)
    {
        try
        {
            //CompanyMgtServicesReference.CompanyMgtServicesClient objCompanyMasterClient = null;
            //CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable objS3G_SYSAD_Branch_ListDataTable = null;
            //CompanyMgtServices.S3G_SYSAD_Branch_ListRow objBranchRow = null;
            //try
            //{

            //    objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

            //    objS3G_SYSAD_Branch_ListDataTable = new CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable();
            //    objBranchRow = objS3G_SYSAD_Branch_ListDataTable.NewS3G_SYSAD_Branch_ListRow();
            //    objBranchRow.Company_ID = intCompanyid;
            //    objS3G_SYSAD_Branch_ListDataTable.AddS3G_SYSAD_Branch_ListRow(objBranchRow);

            //    byte[] bytesobjBranchListDataTable = objCompanyMasterClient.FunPubBranch_List(SerMode, ClsPubSerialize.Serialize(objS3G_SYSAD_Branch_ListDataTable, SerMode));
            //    objS3G_SYSAD_Branch_ListDataTable = (CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable)ClsPubSerialize.DeSerialize(bytesobjBranchListDataTable, SerMode, typeof(CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable));

            //    DataTable dtBranchList = objS3G_SYSAD_Branch_ListDataTable;
            //    DataColumn dc = null;

            //    dc = new DataColumn("Status", typeof(System.Boolean));
            //    dc.DefaultValue = false;
            //    dtBranchList.Columns.Add(dc);


            //    dc = new DataColumn("Closed", typeof(System.Boolean));
            //    dc.DefaultValue = false;
            //    dtBranchList.Columns.Add(dc);


            //    ViewState["DT_AssetAcquisition"] = dtBranchList;

            //    //grvMothEndParam.DataSource = dtBranchList;
            //    //grvMothEndParam.DataBind();
            //}
            //catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
            //{
            //    lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            //}
            //catch (Exception ex)
            //{
            //    lblErrorMessage.Text = ex.Message;
            //}
            //finally
            //{
            //    objCompanyMasterClient.Close();
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region Button (Save / Clear / Cancel)

    /// <summary>
    /// Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblErrorMessage.Text = "";
        try
        {
            if (PageMode == PageModes.Create)
            {
                if (!FunPriGenerateAssetAcquisitionXMLDet())
                {
                    Utility.FunShowAlertMsg(this.Page, "Please fill the Asset general information.");
                    return;
                }

                if (PageMode== PageModes.Create && ViewState["DepDate"] != null
                    && Convert.ToDateTime(Utility.StringToDate(ViewState["DepDate"].ToString()).ToString("dd/MMM/yyyy")) >=
                    Convert.ToDateTime(Utility.StringToDate(txtLAAEDate.Text).ToString("dd/MMM/yyyy")))
                {
                    Utility.FunShowAlertMsg(this.Page, "LAAE Date should be greater than the Last Depreciation Date [" + ViewState["DepDate"].ToString() + "].");
                    txtLAAEDate.Focus();
                    return;
                }

                ObjS3G_LOANAD_AssetAcquisitionDataTable = new AssetMgtServices.S3G_LOANAD_AssetAcquisitionDataTable();
                ObjS3G_LOANAD_AssetAcquisitionRow = null;
                ObjS3G_LOANAD_AssetAcquisitionRow = ObjS3G_LOANAD_AssetAcquisitionDataTable.NewS3G_LOANAD_AssetAcquisitionRow();
                ObjS3G_LOANAD_AssetAcquisitionRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
                ObjS3G_LOANAD_AssetAcquisitionRow.LOB_ID = Convert.ToInt32(ddlLineofBusiness.SelectedValue);
                ObjS3G_LOANAD_AssetAcquisitionRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
                ObjS3G_LOANAD_AssetAcquisitionRow.Acquisition_Date = Utility.StringToDate(txtLAAEDate.Text);
                ObjS3G_LOANAD_AssetAcquisitionRow.Created_By = ObjUserInfo.ProUserIdRW;
                //ObjS3G_LOANAD_AssetAcquisitionRow.TXN_ID = 1;
                ObjS3G_LOANAD_AssetAcquisitionRow.XMLParamtAssetAcquisitionDet = strXMLAssetAcquisitionDet;
                ObjS3G_LOANAD_AssetAcquisitionDataTable.AddS3G_LOANAD_AssetAcquisitionRow(ObjS3G_LOANAD_AssetAcquisitionRow);

                ObjLoanAdminAccMgtServicesClient = new AssetMgtServicesClient();

                string strDSN = "";
                if (intROIRuleMasterID == null || intROIRuleMasterID == "")
                {
                    intErrCode = ObjLoanAdminAccMgtServicesClient.FunPubCreateAssetAcquisition(out strDSN, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_AssetAcquisitionDataTable, SerMode));
                    switch (intErrCode)
                    {
                        case 0: Utility.FunShowAlertMsg(this.Page, "Error in adding Asset Acquisition", strRedirectPageView);
                            break;
                        case 1:
                            strAlert = "Asset Acquisition added successfully";
                            strAlert += @"\n\nAsset Acquisition Number - " + strDSN;
                            strAlert += @"\n\nWould you like to add one more Record?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageLandView + "}";
                            strRedirectPageLandView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageLandView, true);
                            break;
                        case 2: //Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                            Utility.FunShowAlertMsg(this.Page, "Document Number not defined for Asset Acquisition.");
                            break;
                        case 3: //Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                            Utility.FunShowAlertMsg(this.Page, "Document Number exceeded for Asset Acquisition.");
                            return;
                            break;
                        case 4: //Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                            Utility.FunShowAlertMsg(this.Page, "Document Number not defined for Lease Asset Register Number.");
                            break;
                        case 5:
                            //Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                            Utility.FunShowAlertMsg(this.Page, "Document Number exceeded for Lease Asset Register Number.");
                            break;
                        case 10:
                            Utility.FunShowAlertMsg(this.Page, "LAAE Date should be greater than the Last Depreciation calculated date");
                            break;
                        default: break;
                    }
                }
                else if (intROIRuleMasterID != null || intROIRuleMasterID != "")
                {
                    string strXMLAssetAcquisitionAvailabilityDet = gvAssetAcquisitionAvailability.FunPubFormXml().Replace("%", "");
                }
            }
            else
            {
                ObjDictParams = new Dictionary<string, string>();
                ObjDictParams.Add("@Acquisition_No", txtAssetAcquisitionNO.Text);
                ObjDictParams.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                ObjDictParams.Add("@StatusXML", FunPriGenerateStatusXMLDet());
                ObjDictParams.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());

                DataTable dt = Utility.GetDefaultData("S3G_LOANAD_UpdateAssetAcquisition", ObjDictParams);

                Utility.FunShowAlertMsg(this, "Asset Acquisition details updated successfully", strRedirectPageView);
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Asset Acquisition details updated successfully');" + strRedirectPageView, true);
                return;
            }
        }
        catch (FaultException<AssetMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            if (ObjLoanAdminAccMgtServicesClient != null)
            {
                ObjLoanAdminAccMgtServicesClient.Close();
            }
        }
    }

    /// <summary>
    /// Clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //ddlLineofBusiness.SelectedValue = "0";
            ddlBranch.SelectedValue = "0";
            txtLAAEDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //FunPriInsertAssetAcquisitionDataTable("-1", "", "");

            ViewState["DT_AssetAcquisition"] = null;
            dtAssetAcquisition = FunPriGetAssetAcquisitionDataTable();
            //FunPubBindAssetAcquisition(dtAssetAcquisition);


            if (dtAssetAcquisition.Rows.Count == 0)
            {
                FunPriInsertAssetAcquisitionDataTable("-1", "-1", "-1", "", "");
            }
            /*else
            {
                FunPubBindAssetAcquisition(dtAssetAcquisition);
            }*/
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            // wf cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else            
                Response.Redirect(strRedirectPageView);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion


    protected void gvAssetAcquisition_View_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //for (int i = 0; i <= gvAssetAcquisition_View.Columns.Count - 1; i++)
                //{
                //    e.Row.Cells[i].Style.Add("position", "relative");
                //    e.Row.Cells[i].BorderStyle = BorderStyle.None;
                //}
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblInvoiceDate_view = (Label)e.Row.FindControl("lblInvoiceDate_view");
                if (!string.IsNullOrEmpty(lblInvoiceDate_view.Text))
                {
                    lblInvoiceDate_view.Text = Utility.StringToDate(lblInvoiceDate_view.Text).ToString(strDateFormat);
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["DT_AssetAcquisition"] = null;
            DataTable dtAssetAcquisition = FunPriGetAssetAcquisitionDataTable();

            ObjDictParams = new Dictionary<string, string>();
            ObjDictParams.Add("@Company_ID", CompanyId);
            ObjDictParams.Add("@LOB_ID", ddlLineofBusiness.SelectedValue.ToString());
            ObjDictParams.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            DataTable dtDepDate = Utility.GetDefaultData("S3G_LOANAD_GetLastDepreciationDate", ObjDictParams);

            if (dtDepDate.Rows.Count > 0)
            {
                ViewState["DepDate"] = dtDepDate.Rows[0][0].ToString();
            }
            else
            {
                ViewState["DepDate"] = DateTime.Today.AddDays(1).ToString(strDateFormat);
            }

            if (dtAssetAcquisition.Rows.Count == 0)
            {
                FunPriInsertAssetAcquisitionDataTable("-1", "-1","-1", "", "");
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void chkDOGenerated_CheckedChanged(object sender, EventArgs e)
    {
        ViewState["DT_AssetAcquisition"] = null;
        DataTable dtAssetAcquisition = FunPriGetAssetAcquisitionDataTable();

        if (dtAssetAcquisition.Rows.Count == 0)
        {
            FunPriInsertAssetAcquisitionDataTable("-1", "-1", "-1", "", "");
        }

        Label lblAssetType = (Label)gvAssetAcquisition.HeaderRow.FindControl("lblAssetType");
        if (chkDOGenerated.Checked)
        {
            lblAssetType.Text = "Invoice Number";
        }
        else
        {
            lblAssetType.Text = "Asset Type";
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

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "066");
        Procparam.Add("@Lob_Id", obj_Page.ddlLineofBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

}