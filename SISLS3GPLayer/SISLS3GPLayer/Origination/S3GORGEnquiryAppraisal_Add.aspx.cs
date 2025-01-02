using System;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using S3GBusEntity;
using System.Web.Security;
using System.Configuration;
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class Origination_S3GORGEnquiryAppraisal_Add : ApplyThemeForProject
{

    #region Intialization

    int intErrCode = 0;
    int intCompanyID = 0;
    string strDateFormat = string.Empty;
    int intUserID = 0;
    int intEnquiryNo = 0;
    int intEnqCust = 0;
    Dictionary<string, string> Procparam = null;
    UserInfo uinfo = null;
    S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable ObjS3G_EnquiryCustomerDatatable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
    S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable objS3G_EnquiryPendingDocDataTable;
    S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryPendingDocRow objS3G_EnquiryPendingDocRow;
    S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow objS3G_EnquiryCustomerRow;
    EnquiryMgtServicesReference.EnquiryMgtServicesClient objEnquiryMgtServicesClient;
    StringBuilder strScoreDtls = new StringBuilder();
    SerializationMode SerMode = SerializationMode.Binary;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=ENCA&MultipleDNC=1'";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGEnquiryAppraisal_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGEnquiryAppraisal_Add.aspx;";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    string strTansactionType = string.Empty;
    string strLOB_ID = string.Empty;
    string strLocation_ID = string.Empty;
    string strPredisbursement_ID = string.Empty;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    bool boolEnquiryORCustomer = false;
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    Dictionary<string, string> dictDropdownListParam;
    int index = 0;
    int CustomerID = 0;
    ArrayList arrSearchVal = new ArrayList(1);
    int intNoofSearch = 6;
    string[] arrSortCol = new string[] { "Constitution_Name", "Enquiry_No", "(LOB_Code+' - '+LOB_Name)", "(LocM.Location_Code+' - '+Locc.LocationCat_Description)", "(Product_Code+'-'+Product_Name)", "(CM.Customer_Code+'-'+CM.Customer_Name) ", "" };
    PagingValues ObjPaging = new PagingValues();
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public static DataTable dtLOB = new DataTable();
    public static DataTable dtGroup = new DataTable();

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
            ProgramCode = "047";
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            UserInfo ObjUserInfo = new UserInfo();
            FunPubSetIndex(1);
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end
            S3GSession ObjS3GSession = new S3GSession();
            CalendarExtender1.Format = strDateFormat = ObjS3GSession.ProDateFormatRW;
            if (Request.QueryString["qsEnquiryUpdationID"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsEnquiryUpdationID"]);
                intEnquiryNo = Convert.ToInt32(fromTicket.Name);
            }

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                intEnquiryNo = Convert.ToInt32(fromTicket.Name);
            }
            if (blnIsWorkflowApplicable)
            {
                //strTansactionType = "Enquiry";
            }

            SetTabIndexChanged();
            //tcEnquiryAppraisal_TabIndexChanged(null, null);
            txtDate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                //FunPriBindGrid("0");
                if (PageMode == PageModes.Create)
                {
                    FunPriLoadDocumentType();
                    GlobalParameters();
                }
                //rdbCustomEnquiry.SelectedValue = "0";
                //rdbCustomEnquiry_SelectedIndexChanged(null, null);
                FunToolTip();
                //txtFacilityamt.CheckGPSLength(Convert.ToInt32(txtFacilityamt.Attributes["MaxC"]), txtFacilityamt.MaxLength);
                txtFacilityamt.CheckGPSLength(true);
                if (!blnIsWorkflowApplicable || Session["DocumentNo"] == null)
                {
                    if (Request.QueryString["qsTransactionType"] == "4")
                        strTansactionType = "Customer";
                    else if (Request.QueryString["qsTransactionType"] == "1")
                        strTansactionType = "Enquiry";
                    else if (Request.QueryString["qsTransactionType"] == "2")
                        strTansactionType = "Pricing";
                    else if (Request.QueryString["qsTransactionType"] == "3")
                        strTansactionType = "Application";
                    else
                    {
                        FunLoadEnquiryResponseID(intEnquiryNo);
                    }

                    FunEnquiryCustomer(strTansactionType);
                    if (intEnquiryNo > 0 && Request.QueryString["qsMode"] == "C")
                    {
                        FunPriDisableControls(0);
                        FunPubdetailsForEnquiryNo(intCompanyID, intEnquiryNo, Convert.ToInt32(ddlDocumentType.SelectedValue)); // Enquiry Create Mode
                    }
                    else if (strTansactionType == "Customer" && Request.QueryString["qsMode"] == "C") // Create Mode for Customer
                    {
                        FunPubPendingCustomerDetails();
                        FunLoadLOBMaster();
                        FunPriDisableControls(0);
                    }
                    else if (intEnquiryNo > 0 && Request.QueryString["qsMode"] == "M" && (strTansactionType == "Enquiry" || strTansactionType == "Pricing" || strTansactionType == "Application")) //Edit Mode for Enquiry
                    {
                        FunPubEnquiryUpdate(intCompanyID, intEnquiryNo);
                        FunPriDisableControls(1);
                        btnClear.Enabled = false;
                    }
                    else if (intEnquiryNo > 0 && strTansactionType == "Customer" && Request.QueryString["qsMode"] == "M") // Edit Mode for Customer
                    {
                        FunLoadLOBMaster();
                        FunPubCustomerForUpdate(intEnquiryNo);
                        FunPriDisableControls(1);
                        btnClear.Enabled = false;
                    }
                    else if (intEnquiryNo > 0 && Request.QueryString["qsMode"] == "Q" && strTansactionType == "Customer") // Query Mode for Customer
                    {
                        FunLoadLOBMaster();
                        FunPubCustomerForUpdate(intEnquiryNo);
                        btnFacSave.Enabled = Rdbfurtherprocno.Enabled = Rdbfurtherprocyes.Enabled = btnClear.Enabled = false;
                        FunPriDisableControls(-1);
                    }
                    if (intEnquiryNo > 0 && Request.QueryString["qsMode"] == "Q" && (strTansactionType == "Enquiry" || strTansactionType == "Pricing" || strTansactionType == "Application")) // Query Mode for Enquiry
                    {
                        FunPubEnquiryUpdate(intCompanyID, intEnquiryNo);
                        FunPriDisableControls(-1);
                        btnFacSave.Enabled = Rdbfurtherprocno.Enabled = Rdbfurtherprocyes.Enabled = btnClear.Enabled = false;
                    }
                }
                else
                {

                }

                if (PageMode == PageModes.WorkFlow)
                {
                    PreparePageForWFLoad();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void PreparePageForWFLoad()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();
        try
        {
            tbgeneral.Enabled = true;
            tbfacility.Enabled = true;
            tbAppraisalType.Enabled = false;
            tcEnquiryAppraisal.ActiveTab = tbgeneral;
            //tcEnquiryAppraisal_TabIndexChanged(null, null);
            SetTabIndexChanged();
            tcEnquiryAppraisal.TabIndex = 0;
            ddlLOB.Visible = false;
            tblFacility.Visible = false;
            pnlFacilityDDL.Visible = true;
            if (!IsPostBack)
            {
                WorkFlowSession WFSessionValues = new WorkFlowSession();

                FunEnquiryCustomer(strTansactionType);

                FunPriDisableControls(0);
                FunLoadLOBMaster();
                //WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();
                byte[] byte_EnquiryCustomerAppraisal = objWorkflowToDoClient.FunPubLoadEnquiryCustomerAppraisal(WFSessionValues.WorkFlowDocumentNo, int.Parse(CompanyId), WFSessionValues.Document_Type);
                DataSet dsEnquiryAppraisal = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_EnquiryCustomerAppraisal, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));
                if (dsEnquiryAppraisal.Tables.Count > 0)
                {
                    if (dsEnquiryAppraisal.Tables[0].Rows.Count > 0)
                    {
                        ddlDocumentType.SelectedValue = dsEnquiryAppraisal.Tables[0].Rows[0]["Document_Type"].ToString();

                        intEnquiryNo = Convert.ToInt32(dsEnquiryAppraisal.Tables[0].Rows[0]["Document_Type_ID"]);
                        ddlLOB.SelectedValue = (dsEnquiryAppraisal.Tables[0].Rows[0]["LOB_Id"] != null) ? Convert.ToString(dsEnquiryAppraisal.Tables[0].Rows[0]["LOB_Id"]) : "0";
                    }
                    strTansactionType =
                    txtTransactionType.Text = ddlDocumentType.SelectedItem.Text;
                }
                if (intEnquiryNo > 0)
                {
                    FunPubdetailsForEnquiryNo(intCompanyID, intEnquiryNo, Convert.ToInt32(ddlDocumentType.SelectedValue)); // Enquiry Create Mode
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objWorkflowToDoClient.Close();
        }
    }

    private void FunToolTip()
    {
        try
        {
            //throw new NotImplementedException();
            txtEnquiryNo.ToolTip = lblEnquiryno.Text;
            txtproductcode.ToolTip = lblproductcode.Text;
            txtDate.ToolTip = lblDate.Text;
            txtTransactionType.ToolTip = lblTransaction.Text;
            //txtComAddress.ToolTip = lblAddress.Text;                
            //txtEmailID.ToolTip = lblEmailID.Text;
            //txtMobileNo.ToolTip = lblMobileNo.Text;
            txtFacilityamt.ToolTip = lblfacilityamt.Text;
            txtLOB.ToolTip = ddlLOB.ToolTip = lblLOB.Text;
            txtConstitution.ToolTip = lblConstitution.Text;
            btnFacSave.ToolTip = btnFacSave.Text;
            btnCancel.ToolTip = btnCancel.Text;
            btnClear.ToolTip = btnClear.Text;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriLoadDocumentType()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OptionValue", "4");
            DataTable dt = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", Procparam);
            DataRow[] dr = dt.Select("Value NOT IN(4,5)", "Name", DataViewRowState.CurrentRows);
            dt = dr.CopyToDataTable();
            ddlDocumentType.BindDataTable(dt, new string[] { "Value", "Name" });
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GlobalParameters()
    {
        DataTable dtTable = new DataTable();
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            dtTable = Utility.GetDefaultData(SPNames.S3G_Get_GobalCompanyDetails, Procparam);

            txtFacilityamt.Attributes.Add("MaxC", dtTable.Rows[0]["Currency_Max_Digit"].ToString());
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

    #region "To get the Enquiry Response ID"

    private void FunLoadEnquiryResponseID(int intEnquiryNo)
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();

        try
        {
            //throw new NotImplementedException();
            ObjS3G_EnquiryCustomerDatatable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
            objS3G_EnquiryCustomerRow = ObjS3G_EnquiryCustomerDatatable.NewS3G_ORG_EnquiryCustomerDetailsRow();
            objS3G_EnquiryCustomerRow.Enq_Cus_App_ID = intEnquiryNo;
            ObjS3G_EnquiryCustomerDatatable.AddS3G_ORG_EnquiryCustomerDetailsRow(objS3G_EnquiryCustomerRow);
            byte[] bytesEnquiryDetails = objEnquiryMgtServicesClient.FunPubQueryEnquiryResponseID(intEnquiryNo);

            ObjS3G_EnquiryCustomerDatatable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
            ObjS3G_EnquiryCustomerDatatable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryDetails, SerializationMode.Binary, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));
            if (ObjS3G_EnquiryCustomerDatatable.Rows.Count > 0)
            {
                objS3G_EnquiryCustomerRow = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow)ObjS3G_EnquiryCustomerDatatable.Rows[0];
                int intResponseID = objS3G_EnquiryCustomerRow.IsEnquiry_Response_IDNull() ? 0 : objS3G_EnquiryCustomerRow.Enquiry_Response_ID;
                int intType = objS3G_EnquiryCustomerRow.IsTypeNull() ? 0 : objS3G_EnquiryCustomerRow.Type;
                if (intType == 1)//(objS3G_EnquiryCustomerRow.IsEnquiry_Response_IDNull() || Convert.ToString(objS3G_EnquiryCustomerRow.Enquiry_Response_ID) =="")
                    strTansactionType = "Enquiry";
                else if (intType == 2)
                    strTansactionType = "Pricing";
                else if (intType == 3)
                    strTansactionType = "Application";
                else
                    strTansactionType = "Customer";
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }

    #endregion

    private void FunLoadLOBMaster()
    {
        try
        {
            uinfo = new UserInfo();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_Id", Convert.ToString(uinfo.ProUserIdRW));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Program_ID", "47");
            DataTable dtLOB = Utility.GetDefaultData(SPNames.LOBMaster, Procparam);

            ViewState["dtLOB"] = dtLOB;

            ddlLOB.BindDataTable(dtLOB.Copy(), new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #region "Appraised Customer for Edit"

    private void FunPubCustomerForUpdate(int intEnqCust)
    {
        // throw new NotImplementedException();
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            lblEnquiryno.Visible = txtEnquiryNo.Visible = lblDate.Visible = txtDate.Visible = ddlLOB.Visible = pnlFacilityDDL.Visible =
            lblproductcode.Visible = txtproductcode.Visible = imgDate.Visible = false;
            txtLOB.Visible = txtConstitution.Visible = lblConstitution.Visible = txtConstitution.Visible = tblFacility.Visible = true;

            ObjS3G_EnquiryCustomerDatatable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow objS3G_EnquiryCustomerRow;
            objS3G_EnquiryCustomerRow = ObjS3G_EnquiryCustomerDatatable.NewS3G_ORG_EnquiryCustomerDetailsRow();

            objS3G_EnquiryCustomerRow.Enq_Cus_App_ID = intEnqCust;

            ObjS3G_EnquiryCustomerDatatable.AddS3G_ORG_EnquiryCustomerDetailsRow(objS3G_EnquiryCustomerRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] bytesEnquiryDetails = objEnquiryMgtServicesClient.FunPubCustomerForUpdate(intEnqCust);
            ObjS3G_EnquiryCustomerDatatable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryDetails, SerializationMode.Binary, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));

            if (ObjS3G_EnquiryCustomerDatatable.Rows.Count > 0)
            {
                DataRow dtrCustomer = ObjS3G_EnquiryCustomerDatatable.Rows[0];

                if (Convert.ToInt32(ObjS3G_EnquiryCustomerDatatable.Rows[0]["Is_Further_Process"]) == 0)
                    Rdbfurtherprocno.Checked = true;
                else
                    Rdbfurtherprocyes.Checked = true;

                ViewState["Enq_App_ID"] = ObjS3G_EnquiryCustomerDatatable.Rows[0]["Enq_Cus_App_ID"].ToString();
                S3GCustomerPermAddress.SetCustomerDetails(ObjS3G_EnquiryCustomerDatatable.Rows[0], true);

                txtLOB.Text = ObjS3G_EnquiryCustomerDatatable.Rows[0]["LOB_Name"].ToString();
                txtConstitution.Text = ObjS3G_EnquiryCustomerDatatable.Rows[0]["Constitution_Name"].ToString();
                txtFacilityamt.Text = ObjS3G_EnquiryCustomerDatatable.Rows[0]["Facility_Amount"].ToString();

                FunPubCustomerDocumentForUpdate(intEnqCust);

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Enq_Cus_App_ID", intEnqCust.ToString());
                DataSet dSet = Utility.GetDataset("S3G_ORG_GetEnquiry_Customer_Facility", Procparam);
                if (dSet != null && dSet.Tables.Count > 0)
                {
                    ViewState["dtFacilityGroup"] = dSet.Tables[0];
                    ViewState["dtFacilityGrid"] = dSet.Tables[1];

                    grvFacilityGroup.DataSource = dSet.Tables[0];
                    grvFacilityGroup.DataBind();
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }


    protected void FunPubCustomerDocumentForUpdate(int intEnquiryNo)
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            ObjS3G_EnquiryCustomerDatatable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow objS3G_EnquiryCustomerRow;
            objS3G_EnquiryCustomerRow = ObjS3G_EnquiryCustomerDatatable.NewS3G_ORG_EnquiryCustomerDetailsRow();
            objS3G_EnquiryCustomerRow.Enq_Cus_App_ID = Convert.ToInt32(intEnquiryNo);


            ObjS3G_EnquiryCustomerDatatable.AddS3G_ORG_EnquiryCustomerDetailsRow(objS3G_EnquiryCustomerRow);


            SerializationMode SerMode = SerializationMode.Binary;
            byte[] bytesEnquiryDocDetails = objEnquiryMgtServicesClient.FunPubCustomerDocumentForUpdate(intEnquiryNo);
            ObjS3G_EnquiryCustomerDatatable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryDocDetails, SerializationMode.Binary, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));

            GrvEnquiryDocDetails.DataSource = ObjS3G_EnquiryCustomerDatatable;
            GrvEnquiryDocDetails.DataBind();

            foreach (GridViewRow grRow in GrvEnquiryDocDetails.Rows)
            {
                for (int i = 0; i < ObjS3G_EnquiryCustomerDatatable.Rows.Count; i++)
                {
                    DropDownList ddlEnqShiftProgTo = (DropDownList)grRow.FindControl("ddlEnqShiftProgTo");
                    if (ObjS3G_EnquiryCustomerDatatable.Rows[i]["Shift_To_Progress_ID"] != null)
                        ddlEnqShiftProgTo.SelectedValue = Convert.ToInt32(ObjS3G_EnquiryCustomerDatatable.Rows[i]["Shift_To_Progress_ID"]).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }


    #endregion

    private void FunPubdetailsForEnquiryNo(int intCompany_ID, int intEnquiryNo, int intType)
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            ObjS3G_EnquiryCustomerDatatable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow objS3G_EnquiryCustomerRow;
            objS3G_EnquiryCustomerRow = ObjS3G_EnquiryCustomerDatatable.NewS3G_ORG_EnquiryCustomerDetailsRow();
            objS3G_EnquiryCustomerRow.Company_ID = Convert.ToInt32(intCompanyID);
            objS3G_EnquiryCustomerRow.Type = intType;
            objS3G_EnquiryCustomerRow.ID = intEnquiryNo;

            ObjS3G_EnquiryCustomerDatatable.AddS3G_ORG_EnquiryCustomerDetailsRow(objS3G_EnquiryCustomerRow);


            SerializationMode SerMode = SerializationMode.Binary;
            byte[] bytesEnquiryDetails = objEnquiryMgtServicesClient.FunPubGetEnquiryDetailsByEnquiryNo(SerMode, ClsPubSerialize.Serialize(ObjS3G_EnquiryCustomerDatatable, SerMode));
            ObjS3G_EnquiryCustomerDatatable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryDetails, SerializationMode.Binary, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));

            if (ObjS3G_EnquiryCustomerDatatable.Rows.Count > 0)
            {
                DataRow dtrCustomerRow = ObjS3G_EnquiryCustomerDatatable.Rows[0];

                // txtDate.Text =Utility.StringToDate(dtrCustomerRow["Date"].ToString()).ToString(strDateFormat); //DateTime.Now.ToString("dd/MM/yyyy");
                txtDate.Text = DateTime.Now.ToString(strDateFormat);

                ViewState["Date"] = Utility.StringToDate(dtrCustomerRow["Date"].ToString());

                txtEnquiryNo.Text = dtrCustomerRow["Number"].ToString();

                txtproductcode.Text = dtrCustomerRow["Product_Code"].ToString() + "-" + dtrCustomerRow["Product_Name"].ToString();

                S3GCustomerPermAddress.SetCustomerDetails(ObjS3G_EnquiryCustomerDatatable.Rows[0], true);

                txtLOB.Text = dtrCustomerRow["LOB_Name"].ToString();
                txtFacilityamt.Text = dtrCustomerRow["Amount"].ToString();
                ViewState["LOB_ID"] = dtrCustomerRow["LOB_ID"].ToString();
                ViewState["Location_ID"] = dtrCustomerRow["Location_ID"].ToString();
                ViewState["Predisbursement_ID"] = dtrCustomerRow["PreDisbursement_Doc_Tran_ID"].ToString();
                ViewState["Customer_ID"] = dtrCustomerRow["Customer_ID"].ToString();
                ViewState["Doc_Ref_ID"] = dtrCustomerRow["ID"].ToString();

                byte[] bytesEnquiryPendingDocDetails = objEnquiryMgtServicesClient.FunPubGetEnquiryPendingDoc(intCompanyID, Convert.ToInt32(ddlDocumentType.SelectedValue), intEnquiryNo);
                objS3G_EnquiryPendingDocDataTable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryPendingDocDetails, SerializationMode.Binary, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable));

                GrvEnquiryDocDetails.DataSource = objS3G_EnquiryPendingDocDataTable;
                GrvEnquiryDocDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }

    }

    #region "Visibility Toggle Enquiry /Customer"

    private void FunEnquiryCustomer(string strTansactionType)
    {
        try
        {
            if (strTansactionType == "Customer")
            {
                AjaxControlToolkit.CalendarExtender CalendarExtender1 = (AjaxControlToolkit.CalendarExtender)UpdatePanel1.FindControl("CalendarExtender1");
                lblEnquiryno.Visible = txtEnquiryNo.Visible = lblDate.Visible = txtDate.Visible = txtLOB.Visible =
                lblproductcode.Visible = txtproductcode.Visible = imgDate.Visible = false;
                txtFacilityamt.ReadOnly = false;
                txtConstitution.Visible = lblConstitution.Visible = tblFacility.Visible = true;
                rfvDate.Enabled = pnlFacilityDDL.Visible = false;
                rfvDate.Visible = false;
                txtTransactionType.Text = "Customer";
                txtFacilityamt.Text = string.Empty;
            }
            else if (strTansactionType == "Enquiry")
            {
                txtConstitution.Visible = lblConstitution.Visible = tblFacility.Visible = false;
                txtFacilityamt.ReadOnly = pnlFacilityDDL.Visible = true;
                txtLOB.Visible = txtEnquiryNo.Visible = txtDate.Visible = txtproductcode.Visible = lblEnquiryno.Visible =
                   lblDate.Visible = lblproductcode.Visible = imgDate.Visible = true;
                rfvDate.Enabled = true;
                rfvDate.Visible = true;
                txtTransactionType.Text = "Enquiry";
            }
            else if (strTansactionType == "Pricing")
            {
                txtConstitution.Visible = lblConstitution.Visible = tblFacility.Visible = false;
                txtFacilityamt.ReadOnly = pnlFacilityDDL.Visible = true;
                txtLOB.Visible = txtEnquiryNo.Visible = txtDate.Visible = txtproductcode.Visible = lblEnquiryno.Visible =
                   lblDate.Visible = lblproductcode.Visible = imgDate.Visible = true;
                rfvDate.Enabled = true;
                rfvDate.Visible = true;
                txtTransactionType.Text = "Pricing";
            }
            else if (strTansactionType == "Application")
            {
                txtConstitution.Visible = lblConstitution.Visible = tblFacility.Visible = false;
                txtFacilityamt.ReadOnly = pnlFacilityDDL.Visible = true;
                txtLOB.Visible = txtEnquiryNo.Visible = txtDate.Visible = txtproductcode.Visible = lblEnquiryno.Visible =
                   lblDate.Visible = lblproductcode.Visible = imgDate.Visible = true;
                rfvDate.Enabled = true;
                rfvDate.Visible = true;
                txtTransactionType.Text = "Application";
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //wf Cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else if (tcEnquiryAppraisal.ActiveTabIndex == 0)
                Response.Redirect("S3GORGTransLander.aspx?Code=ENCA&MultipleDNC=1");
            else
                if (Request.QueryString["qsMode"] != null && Request.QueryString["qsMode"] != "C")
                    Response.Redirect("S3GORGTransLander.aspx?Code=ENCA&MultipleDNC=1");
                else
                    Response.Redirect("S3GORGEnquiryAppraisal_Add.aspx?qsMode=C&qsTransactionType=0");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTransactionType.Text == "Customer")
            {
                ddlLOB.SelectedIndex = -1;
                txtFacilityamt.Text = string.Empty;
            }

            foreach (GridViewRow grRow in GrvEnquiryDocDetails.Rows)
            {
                DropDownList ddlEnqShiftProgTo = (DropDownList)grRow.FindControl("ddlEnqShiftProgTo");
                ddlEnqShiftProgTo.SelectedIndex = -1;
            }
            Rdbfurtherprocno.Checked = false;
            Rdbfurtherprocyes.Checked = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #region "Customers for whom the mandatory documnts are pending"

    private void FunPubPendingCustomerDetails()
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            DataTable dtCustomerDocTable = new DataTable();
            dtCustomerDocTable = objEnquiryMgtServicesClient.FunPubPendingCustomer(intCompanyID);
            //ddlCustomerCode.FillDataTable(dtCustomerDocTable, "Customer_ID", "Customer_Code");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }

    #endregion

    #region "Enquiry Number in Edit Mode"

    private void FunPubEnquiryUpdate(int intCompanyID, int intEnquiryNo)
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {

            ObjS3G_EnquiryCustomerDatatable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow objS3G_EnquiryCustomerRow;
            objS3G_EnquiryCustomerRow = ObjS3G_EnquiryCustomerDatatable.NewS3G_ORG_EnquiryCustomerDetailsRow();
            objS3G_EnquiryCustomerRow.Company_ID = Convert.ToInt32(intCompanyID);
            objS3G_EnquiryCustomerRow.Enq_Cus_App_ID = Convert.ToInt32(intEnquiryNo);

            ObjS3G_EnquiryCustomerDatatable.AddS3G_ORG_EnquiryCustomerDetailsRow(objS3G_EnquiryCustomerRow);


            SerializationMode SerMode = SerializationMode.Binary;
            byte[] bytesEnquiryDetails = objEnquiryMgtServicesClient.FunPubEnquiryForUpdate(SerMode, ClsPubSerialize.Serialize(ObjS3G_EnquiryCustomerDatatable, SerMode));
            ObjS3G_EnquiryCustomerDatatable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryDetails, SerializationMode.Binary, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));

            //ObjS3G_EnquiryCustomerDatatable.Rows[1];
            DataRow dtrEnqCustomer = ObjS3G_EnquiryCustomerDatatable.Rows[0];

            ViewState["Enq_App_Doc_ID"] = dtrEnqCustomer["Enq_Cus_App_Doc_ID"].ToString();
            ViewState["Enq_App_ID"] = dtrEnqCustomer["Enq_Cus_App_ID"].ToString();

            txtDate.Text = Utility.StringToDate(dtrEnqCustomer["Date"].ToString()).ToString(strDateFormat);
            txtproductcode.Text = dtrEnqCustomer["Product_Code"].ToString() + "-" + dtrEnqCustomer["Product_Name"].ToString();

            txtFacilityamt.Text = dtrEnqCustomer["Facility_Amount"].ToString();
            txtLOB.Text = dtrEnqCustomer["LOB_Name"].ToString();
            txtEnquiryNo.Text = dtrEnqCustomer["Doc_Number"].ToString();
            S3GCustomerPermAddress.SetCustomerDetails(dtrEnqCustomer, true);


            if (Convert.ToInt32(dtrEnqCustomer["Is_Further_Process"]) == 1)
                Rdbfurtherprocyes.Checked = true;
            else
                Rdbfurtherprocno.Checked = true;

            // txtLOB.Enabled = txtcusaddress.Enabled = txtDate.Enabled = txtcuscode.Enabled = txtproductcode.Enabled = txtEnquiryNo.Enabled =
            // txtcustname.Enabled = txtFacilityamt.Enabled = false;

            FunPubEnquiryDocumentForUpdate(intEnquiryNo);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }


    private void FunPubEnquiryDocumentForUpdate(int intEnquiryNo)
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            SerializationMode SerMode = SerializationMode.Binary;
            objS3G_EnquiryCustomerRow = ObjS3G_EnquiryCustomerDatatable.NewS3G_ORG_EnquiryCustomerDetailsRow();
            ObjS3G_EnquiryCustomerDatatable.AddS3G_ORG_EnquiryCustomerDetailsRow(objS3G_EnquiryCustomerRow);
            objS3G_EnquiryCustomerRow.Enq_Cus_App_ID = intEnquiryNo;


            objS3G_EnquiryCustomerRow.EnquiryUpdation_ID = intEnquiryNo;
            byte[] bytesEnquiryDocDetails = objEnquiryMgtServicesClient.FunPubEnquiryDocumentForUpdate(intEnquiryNo);
            ObjS3G_EnquiryCustomerDatatable = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryDocDetails, SerializationMode.Binary, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));

            GrvEnquiryDocDetails.DataSource = ObjS3G_EnquiryCustomerDatatable;
            GrvEnquiryDocDetails.DataBind();


            int iShiftLoop = 0;

            foreach (GridViewRow grRow in GrvEnquiryDocDetails.Rows)
            {

                //for (int i = 0; i < ObjS3G_EnquiryCustomerDatatable.Rows.Count; i++)
                //{
                DropDownList ddlEnqShiftProgTo = (DropDownList)grRow.FindControl("ddlEnqShiftProgTo");
                if (ObjS3G_EnquiryCustomerDatatable.Rows[iShiftLoop]["Shift_To_Progress_ID"] != null)
                    ddlEnqShiftProgTo.SelectedValue = Convert.ToInt32(ObjS3G_EnquiryCustomerDatatable.Rows[iShiftLoop]["Shift_To_Progress_ID"]).ToString();
                //}
                iShiftLoop++;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }

    #endregion

    #region "Customer related pending documnets"

    protected void FunLoadCustomerData()
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            DataSet dtCustomerDocTable = new DataSet();
            dtCustomerDocTable = objEnquiryMgtServicesClient.FunPubPendingCustomerDetails(CustomerID, intCompanyID);

            if (dtCustomerDocTable.Tables[0].Rows.Count > 0)
            {
                DataRow dtrCustomer = dtCustomerDocTable.Tables[0].Rows[0];
                S3GCustomerPermAddress.SetCustomerDetails(dtrCustomer, true);
                txtConstitution.Text = dtrCustomer["Constitution_Name"].ToString();
                ViewState["Customer_ID"] = dtrCustomer["Customer_ID"].ToString();
            }
            else
                S3GCustomerPermAddress.ClearCustomerDetails();

            if (dtCustomerDocTable.Tables[1].Rows.Count > 0)
            {
                GrvEnquiryDocDetails.DataSource = dtCustomerDocTable.Tables[1];
                GrvEnquiryDocDetails.DataBind();
            }
            else
            {
                GrvEnquiryDocDetails.DataSource = null;
                GrvEnquiryDocDetails.DataBind();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }

    protected void ddlCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
    {

        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        try
        {
            DataTable dtCustomerDocTable = new DataTable();
            //dtCustomerDocTable = objEnquiryMgtServicesClient.FunPubPendingCustomerDetails(Convert.ToInt32(ddlCustomerCode.SelectedValue), intCompanyID);


            if (dtCustomerDocTable.Rows.Count > 0)
            {
                //txtCustomerName.Text = dtCustomerDocTable.Rows[0]["Customer_Name"].ToString();
                //txtAddress.Text = dtCustomerDocTable.Rows[0]["Customer_Address"].ToString();
                S3GCustomerPermAddress.SetCustomerDetails(dtCustomerDocTable.Rows[0], true);

                txtConstitution.Text = dtCustomerDocTable.Rows[0]["Constitution_Name"].ToString();
                ViewState["Customer_ID"] = dtCustomerDocTable.Rows[0]["Customer_ID"].ToString();
                ViewState["LOB_ID"] = dtCustomerDocTable.Rows[0]["LOB_ID"].ToString();

                GrvEnquiryDocDetails.DataSource = dtCustomerDocTable;
                GrvEnquiryDocDetails.DataBind();
            }
            else
            {
                txtConstitution.Text = string.Empty;
                S3GCustomerPermAddress.ClearCustomerDetails();
                GrvEnquiryDocDetails.DataSource = null;
                GrvEnquiryDocDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }

    #endregion

    protected void csvOption_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        try
        {
            if (Rdbfurtherprocyes.Checked == false && Rdbfurtherprocno.Checked == false)
            {
                args.IsValid = false;
                csvOption.ErrorMessage = "Select an option";
            }
            else
            {
                args.IsValid = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void csvShift_ServerValidate(object sender, ServerValidateEventArgs args)
    {
        try
        {
            if (Rdbfurtherprocyes.Checked)
            {
                foreach (GridViewRow grRow in GrvEnquiryDocDetails.Rows)
                {
                    DropDownList ddlEnqShiftProgTo = (DropDownList)grRow.FindControl("ddlEnqShiftProgTo");
                    if (ddlEnqShiftProgTo.SelectedIndex == 0)
                    {
                        args.IsValid = false;
                        csvShift.ErrorMessage = "Select Shift Progress To";
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #region "Save Appraised Enquiry/Customer"

    protected void btnFacSave_Click(object sender, EventArgs e)
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        if (Page.IsValid)
        {
            try
            {
                string strKey = "Insert";
                string strAlert = "alert('__ALERT__');";
                string strRedirectPageAdd = "window.location.href='../Origination/S3GORGEnquiryAppraisal_Add.aspx?qsMode=C';";
                string strRedirectPageView = "window.location.href='../Origination/S3GORGEnquiryAppraisal_View.aspx';";
                string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
                ObjS3G_EnquiryCustomerDatatable = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();

                #region "Transaction Type is Enquiry"


                if (Request.QueryString["qsMode"] == "M")// Check Need to be changed later
                {
                    strScoreDtls.Append("<Root>");

                    foreach (GridViewRow grvRow in GrvEnquiryDocDetails.Rows)
                    {
                        Label lblDocPen = grvRow.FindControl("lblEnqDocPending") as Label;
                        Label lblEnqDocCatID = grvRow.FindControl("lblEnqDocCatID") as Label;
                        Label lblEnqDocAppID = grvRow.FindControl("lblAppDocID") as Label;
                        DropDownList ddldesc = grvRow.FindControl("ddlEnqShiftProgTo") as DropDownList;

                        strScoreDtls.Append("<Details Doc_ID= '" + Convert.ToInt32(lblEnqDocCatID.Text) + "' Shift_To_Progress_ID= '" + Convert.ToInt32(ddldesc.SelectedValue) + "' Enq_Cus_App_Doc_ID= '" + lblEnqDocAppID.Text + "' /> ");
                        //strScoreDtls.Append("Docs='" + ddldesc.SelectedItem.Text.Trim() + "'/>");
                    }
                    strScoreDtls.Append("</Root>");

                    objS3G_EnquiryCustomerRow = ObjS3G_EnquiryCustomerDatatable.NewS3G_ORG_EnquiryCustomerDetailsRow();
                    objS3G_EnquiryCustomerRow.Is_Further_Process = Rdbfurtherprocno.Checked ? Convert.ToByte(0) : Convert.ToByte(1);
                    objS3G_EnquiryCustomerRow.XmlEnquiryDetails = strScoreDtls.ToString();
                    objS3G_EnquiryCustomerRow.Enq_Cus_App_ID = Convert.ToInt32(ViewState["Enq_App_ID"]);
                    objS3G_EnquiryCustomerRow.Modified_On = DateTime.Now;
                    objS3G_EnquiryCustomerRow.Modified_By = intUserID;
                    objS3G_EnquiryCustomerRow.Enq_Cus_App_Doc_ID = 1;
                    ObjS3G_EnquiryCustomerDatatable.AddS3G_ORG_EnquiryCustomerDetailsRow(objS3G_EnquiryCustomerRow);

                    if (grvFacilityGroup.Rows.Count > 0)
                    {
                        DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];
                        DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];

                        for (int i = 0; i <= dtFacilityGroup.Rows.Count - 1; i++)
                        {
                            if (dtFacilityGrid.Check("Group_ID='" + dtFacilityGroup.Rows[i]["Group_ID"].ToString() + "'", 0))
                            {
                                Utility.FunShowAlertMsg(this, "Group : " + dtFacilityGroup.Rows[i]["Group_Text"].ToString() + " having no records");
                                return;
                            }
                            else
                            {
                                string strTotal = dtFacilityGroup.Rows[i]["Total_Amount"].ToString();
                                dtFacilityGrid.Update("Group_ID='" + dtFacilityGroup.Rows[i]["Group_ID"].ToString() + "'", "Total_Amount", strTotal.ToString());
                            }
                        }

                        decimal decTotal = (decimal)dtFacilityGroup.Compute("SUM(Total_Amount)", "1=1");
                        objS3G_EnquiryCustomerRow.Facility_Amount = Convert.ToDouble(decTotal);
                        objS3G_EnquiryCustomerRow.XMLFacility = dtFacilityGrid.FunPubFormXml();
                    }

                    intErrCode = objEnquiryMgtServicesClient.FunPubModifyEnquiryAppraisal(SerMode, ClsPubSerialize.Serialize(ObjS3G_EnquiryCustomerDatatable, SerMode));

                    if (intErrCode == 0)
                    {
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnFacSave.Enabled = false;
                        //End here

                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Details updated successfully');" + strRedirectPage, true);
                    }


                }
                else
                {

                    strScoreDtls.Append("<Root>");

                    foreach (GridViewRow grvRow in GrvEnquiryDocDetails.Rows)
                    {
                        Label lblDocPen = grvRow.FindControl("lblEnqDocPending") as Label;
                        Label lblEnqDocCatID = grvRow.FindControl("lblEnqDocCatID") as Label;
                        Label lblEnqDocAppID = grvRow.FindControl("lblAppDocID") as Label;
                        DropDownList ddldesc = grvRow.FindControl("ddlEnqShiftProgTo") as DropDownList;

                        strScoreDtls.Append("<Details Doc_ID= '" + Convert.ToInt32(lblEnqDocCatID.Text) + "' Shift_To_Progress_ID='" + Convert.ToInt32(ddldesc.SelectedValue) + "' Enq_Cus_App_Doc_ID= '" + "0" + "' /> ");
                    }
                    strScoreDtls.Append("</Root>");

                    objS3G_EnquiryCustomerRow = ObjS3G_EnquiryCustomerDatatable.NewS3G_ORG_EnquiryCustomerDetailsRow();
                    objS3G_EnquiryCustomerRow.Company_ID = Convert.ToInt32(intCompanyID);
                    if (txtDate.Text != "")
                    {
                        objS3G_EnquiryCustomerRow.Date = Utility.StringToDate(txtDate.Text);
                    }
                    else
                    {
                        objS3G_EnquiryCustomerRow.Date = DateTime.Now;
                    }
                    if (ViewState["Date"] != null)
                    {
                        if (Utility.CompareDates(txtDate.Text, Utility.StringToDate(ViewState["Date"].ToString()).ToString()) == 1)
                        {
                            Utility.FunShowAlertMsg(this.Page, "Selected date is less than the Document Date");
                            return;
                        }
                    }
                    objS3G_EnquiryCustomerRow.Type = Convert.ToInt32(ddlDocumentType.SelectedValue);
                    if (txtTransactionType.Text == "Customer")
                    {
                        objS3G_EnquiryCustomerRow.Appraisal_Transaction_Type = txtTransactionType.Text;
                        boolEnquiryORCustomer = true;
                    }
                    else
                    {
                        objS3G_EnquiryCustomerRow.Appraisal_Transaction_Type = txtTransactionType.Text;
                        objS3G_EnquiryCustomerRow.ID = Convert.ToInt32(ViewState["Doc_Ref_ID"]);
                        objS3G_EnquiryCustomerRow.PreDisbursement_Doc_Tran_ID = Convert.ToInt32(ViewState["Predisbursement_ID"]);
                        objS3G_EnquiryCustomerRow.Branch_ID = Convert.ToInt32(ViewState["Location_ID"]);
                        boolEnquiryORCustomer = false;
                    }
                    objS3G_EnquiryCustomerRow.Is_Further_Process = Convert.ToByte(Rdbfurtherprocyes.Checked ? 1 : 0);
                    objS3G_EnquiryCustomerRow.Created_By = intUserID;
                    if (boolEnquiryORCustomer)
                    {
                        objS3G_EnquiryCustomerRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                    }
                    else
                    {
                        objS3G_EnquiryCustomerRow.LOB_ID = Convert.ToInt32(ViewState["LOB_ID"]);
                    }

                    double FaclityAmount;
                    if (double.TryParse(txtFacilityamt.Text, out FaclityAmount))
                        objS3G_EnquiryCustomerRow.Facility_Amount = FaclityAmount;

                    objS3G_EnquiryCustomerRow.LOB_Name = txtLOB.Text;
                    if (ViewState["Customer_ID"] != null && !string.IsNullOrEmpty(ViewState["Customer_ID"].ToString()))
                    {
                        objS3G_EnquiryCustomerRow.Customer_ID = Convert.ToInt32(ViewState["Customer_ID"]);
                    }
                    objS3G_EnquiryCustomerRow.XmlEnquiryDetails = strScoreDtls.ToString();
                    ObjS3G_EnquiryCustomerDatatable.AddS3G_ORG_EnquiryCustomerDetailsRow(objS3G_EnquiryCustomerRow);

                    if (grvFacilityGroup.Rows.Count > 0)
                    {
                        DataTable dtFacilityGroup =  (DataTable)ViewState["dtFacilityGroup"];
                        DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];

                        for (int i = 0; i <= dtFacilityGroup.Rows.Count - 1; i++)
                        {
                            if (dtFacilityGrid.Check("Group_ID='" + dtFacilityGroup.Rows[i]["Group_ID"].ToString() + "'", 0))
                            {
                                Utility.FunShowAlertMsg(this, "Group : " + dtFacilityGroup.Rows[i]["Group_Text"].ToString() + " having no records");
                                return;
                            }
                            else
                            {
                                //decimal decTotal = (decimal)dtFacilityGrid.Compute("Groupt_ID='" + dtFacilityGroup.Rows[i]["Groupt_ID"].ToString() + "'", "1=1");
                                string strTotal = dtFacilityGroup.Rows[i]["Total_Amount"].ToString();
                                dtFacilityGrid.Update("Group_ID='" + dtFacilityGroup.Rows[i]["Group_ID"].ToString() + "'", "Total_Amount", strTotal.ToString());  
                            }
                        }

                        decimal decTotal = (decimal)dtFacilityGroup.Compute("SUM(Total_Amount)", "1=1");
                        objS3G_EnquiryCustomerRow.Facility_Amount = Convert.ToDouble(decTotal);
                        objS3G_EnquiryCustomerRow.XMLFacility = dtFacilityGrid.FunPubFormXml();
                    }

                    //objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
                    intErrCode = objEnquiryMgtServicesClient.FunPubCreateEnquiryAppraisal(SerMode, ClsPubSerialize.Serialize(ObjS3G_EnquiryCustomerDatatable, SerMode), boolEnquiryORCustomer);

                    if (intErrCode == 0)
                    {
                        if (isWorkFlowTraveler)
                        {
                            WorkFlowSession WFValues = new WorkFlowSession();
                            try
                            {
                                int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), WFValues.LOBId, WFValues.BranchID, WFValues.WorkFlowDocumentNo, WFValues.WorkFlowProgramId, WFValues.WorkFlowStatusID.ToString(), WFValues.ProductId, int.Parse(ddlDocumentType.SelectedValue));
                                strAlert = "";
                            }
                            catch (Exception ex)
                            {
                                strAlert = "Work Flow is not Assigned";
                            }
                            ShowWFAlertMessage(WFValues.WorkFlowDocumentNo, ProgramCode, strAlert);
                            return;
                        }
                        else
                        {
                            if (txtTransactionType.Text == "Customer")
                            {
                                strAlert = "Appraisal done successfully for the Customer ";
                                //strAlert = "Appraisal done successfully for the Customer " + txtCustomerName.Text;

                            }
                            else
                            {
                                DataTable WFFP = new DataTable();
                                if (CheckForForcePullOperation(null, txtEnquiryNo.Text.Trim(), ProgramCode, null, null, "O", CompanyId, null, null, txtLOB.Text.Trim(), txtproductcode.Text.Trim(), out WFFP))
                                {
                                    DataRow dtrForce = WFFP.Rows[0];
                                    try
                                    {
                                        int intWorkflowStatus = UpdateWorkFlowTasks(CompanyId.ToString(), UserId.ToString(), int.Parse(dtrForce["LOBId"].ToString()), int.Parse(dtrForce["LocationID"].ToString()), txtEnquiryNo.Text.Trim(), int.Parse(dtrForce["WFPROGRAMID"].ToString()), dtrForce["WFSTATUSID"].ToString(), int.Parse(dtrForce["PRODUCTID"].ToString()), int.Parse(ddlDocumentType.SelectedValue));

                                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                        btnFacSave.Enabled = false;
                                        //End here
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                }

                                strAlert = "Appraisal done successfully for the Document Number " + txtEnquiryNo.Text;
                            }

                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnFacSave.Enabled = false;
                            //End here

                            strAlert += @"\n\nWould you like to appraise another record?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPage + "}";

                            Utility.FunShowAlertMsg(this, strAlert);
                            strRedirectPageView = strRedirectPage;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                            return;
                        }
                    }
                    else if (intErrCode == 3)
                        Utility.FunShowAlertMsg(this, "Already appraisal has been done this Customer");
                }

                #endregion


            }
            catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
            {
                // lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                throw ex;
            }
            finally
            {
                objEnquiryMgtServicesClient.Close();
            }
        }
    }
    #endregion

    /* WorkFlow Properties */
    private int WFLOBId { get { return 8; } }
    private int WFProduct { get { return 3; } }    /// <summary>

    #region "User Authorization"


    ////This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    Rdbfurtherprocyes.Checked = true;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    tbgeneral.Enabled = true;
                    FunPridisableFacilityTab();


                    //tcEnquiryAppraisal_TabIndexChanged(null, null);
                    SetTabIndexChanged();

                    if (!bCreate)
                    {
                        btnFacSave.Enabled = false;

                    }

                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    tbgeneral.Enabled = true;
                    tbfacility.Enabled = true;
                    tbAppraisalType.Enabled = false;
                    tcEnquiryAppraisal.ActiveTab = tbgeneral;

                    //tcEnquiryAppraisal_TabIndexChanged(null, null);
                    SetTabIndexChanged();

                    if (Procparam == null)
                        Procparam = new Dictionary<string, string>();
                    else
                        Procparam.Clear();

                    Procparam.Add("@Param1", intCompanyID.ToString());
                    Procparam.Add("@Param2", intEnquiryNo.ToString());
                    Procparam.Add("@Option", "29");

                    DataTable Obj_Dt = new DataTable();

                    Obj_Dt = Utility.GetDataset("S3G_ORG_GetStatusLookUp", Procparam).Tables[0];
                    if (Obj_Dt.Rows.Count > 0)
                    {
                        if (strTansactionType == "Enquiry")
                            Utility.FunShowAlertMsg(this.Page, "Modification not possible. The Enquiry Number is assigned for credit appraisal");
                        else if (strTansactionType == "Customer")
                            Utility.FunShowAlertMsg(this.Page, "Modification not possible. The Customer is assigned for credit appraisal");
                        else if (strTansactionType == "Pricing")
                            Utility.FunShowAlertMsg(this.Page, "Modification not possible. The Pricing Number is assigned for credit appraisal");
                        else if (strTansactionType == "Application")
                            Utility.FunShowAlertMsg(this.Page, "Modification not possible. The Application Number is assigned for credit appraisal");
                        QueryView();
                    }

                    if (!bModify)
                    {
                        btnFacSave.Enabled = false;

                    }
                    if (strTansactionType == "Enquiry")
                    {
                        txtLOB.ReadOnly = txtDate.Enabled = txtproductcode.ReadOnly = txtEnquiryNo.ReadOnly =
                         txtFacilityamt.ReadOnly = true;
                        imgDate.Visible = false;
                        CalendarExtender1.Enabled = false;
                        txtTransactionType.Text = "Enquiry";
                    }
                    else if (strTansactionType == "Pricing")
                    {
                        txtLOB.ReadOnly = txtDate.Enabled = txtproductcode.ReadOnly = txtEnquiryNo.ReadOnly =
                         txtFacilityamt.ReadOnly = true;
                        imgDate.Visible = false;
                        CalendarExtender1.Enabled = false;
                        txtTransactionType.Text = "Pricing";
                    }
                    else if (strTansactionType == "Application")
                    {
                        txtLOB.ReadOnly = txtDate.Enabled = txtproductcode.ReadOnly = txtEnquiryNo.ReadOnly =
                         txtFacilityamt.ReadOnly = true;
                        imgDate.Visible = false;
                        CalendarExtender1.Enabled = false;
                        txtTransactionType.Text = "Application";
                    }
                    else if (strTansactionType == "Customer")
                    {
                        ddlLOB.Enabled = lblproductcode.Visible = lblEnquiryno.Visible = txtEnquiryNo.Visible =
                        btnClear.Enabled = false;
                        txtLOB.ReadOnly = txtFacilityamt.ReadOnly = txtConstitution.ReadOnly = true;
                        txtTransactionType.Text = "Customer";

                        pnlFacilityDDL.Visible = false;
                        tbfacility.Visible = true;
                    }
                    btnClear.Enabled = false;
                    FunPridisableFacilityTab();
                    break;


                case -1:// Query Mode
                    QueryView();
                    //tcEnquiryAppraisal_TabIndexChanged(null, null);
                    SetTabIndexChanged();
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPridisableFacilityTab()
    {
        try
        {
            if (!string.IsNullOrEmpty(txtLOB.Text))
            {
                if (txtLOB.Text.Split('-')[0].ToString().Trim().ToLower() == "ol")
                {
                    tbfacility.Enabled = false;
                }
                else
                {
                    tbfacility.Enabled = true;
                }
            }
            else
            {
                tbfacility.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void QueryView()
    {
        try
        {
            //tbgeneral.Visible = true;
            tbgeneral.Enabled = true;
            tbfacility.Enabled = true;
            tbAppraisalType.Enabled = false;
            tcEnquiryAppraisal.ActiveTab = tbgeneral;

            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            btnFacSave.Enabled = false;
            txtDate.ReadOnly = txtFacilityamt.ReadOnly = true;
            CalendarExtender1.Enabled = false;
            imgDate.Visible = false;

            if (!bQuery)
            {
                Response.Redirect(strRedirectPage, false);
            }

            if (bClearList)
            {
                if (strTansactionType == "Customer")
                {
                    //ddlCustomerCode.ClearDropDownList();
                    txtTransactionType.Text = "Customer";
                }
                else
                {
                    ddlLOB.ClearDropDownList();
                    txtTransactionType.Text = "Enquiry";
                }
            }
            //ddlCustomerCode.Enabled = true;
            ddlLOB.Enabled = true;

            foreach (GridViewRow GVR in GrvEnquiryDocDetails.Rows)
            {
                DropDownList DDL = (DropDownList)GVR.FindControl("ddlEnqShiftProgTo");
                DDL.ClearDropDownList();
            }
            Rdbfurtherprocyes.Enabled = Rdbfurtherprocno.Enabled = false;
            FunPridisableFacilityTab();
            grvFacilityGroup.ShowFooter = false;
            foreach (GridViewRow gRow in grvFacilityGroup.Rows)
            {
                GridView grvFacility = (GridView)gRow.FindControl("grvFacility");
                HtmlTableCell tdGroupDelete = (HtmlTableCell)gRow.FindControl("tdGroupDelete");
                grvFacility.ShowFooter = false;
                grvFacility.Columns[grvFacility.Columns.Count - 1].Visible = tdGroupDelete.Visible = false;

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    //protected void tbfacility_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    index = 1;
    //    if (tbfacility.OnClientClick == true)
    //        btnFacSave.ValidationGroup = "vgfac";

    //}

    //protected void tbfacility_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    index = 0;
    //    if (tbfacility.OnClientClick == true)
    //        btnFacSave.ValidationGroup = "vgman";

    //}

    ////Code end


    #endregion
    //protected void tc_facility(object sender, EventArgs e)
    //{
    //    if (tcEnquiryAppraisal.ActiveTabIndex == 0)
    //        btnFacSave.ValidationGroup = "vsMan";
    //    else
    //        btnFacSave.ValidationGroup = "vsfac";

    //}

    #region Paging Config

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (lnkbtnSearch.ID == "lnkbtnSort" + (iCount + 1).ToString())
                {
                    strSortColName = arrSortCol[iCount].ToString();
                    break;
                }
            }

            string strDirection = string.Empty;
            string strSortDirection = string.Empty;

            if (((ImageButton)gvEnquiryAppraisal.HeaderRow.FindControl(imgbtnSearch)).CssClass == "styleImageSortingAsc")
            {
                strSortDirection = "DESC";
            }
            else
            {
                strSortDirection = "ASC";
            }

            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strSortColName;
            hdnOrderBy.Value = " " + strSortColName + " " + strSortDirection;

            FunProHeaderSearch(null, null);
            //if (rdbCustomEnquiry.SelectedValue == "0")
            //{
            //    FunPriBindGrid("0");

            //}
            //else
            //{
            //    FunPriBindGrid("1");
            //}

            if (strSortDirection == "ASC")
            {
                ((ImageButton)gvEnquiryAppraisal.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)gvEnquiryAppraisal.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.InnerText = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriBindGrid(string Option)
    {
        try
        {
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            FunPriGetSearchValue();

            //if (Request.QueryString["qsEnquiryUpdationID"] != null)
            //{
            //    FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsEnquiryUpdationID"]);
            //    intEnquiryNo = Convert.ToInt32(fromTicket.Name);
            //}

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@EnquiryUpdation_ID", "0");
            Procparam.Add("@Option", Option.ToString());

            gvEnquiryAppraisal.BindGridView("S3G_ORG_GetEnquiryDetails", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                gvEnquiryAppraisal.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            if (Option == "6")
            {
                ToggleColumns(false);

            }
            else
            {
                ToggleColumns(true);
            }

            btnFacSave.Visible = btnClear.Visible = false;
            //lblErrorMessage.InnerText = "";

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

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

    private void FunPriGetSearchValue()
    {
        try
        {
            arrSearchVal = gvEnquiryAppraisal.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriSetSearchValue()
    {
        try
        {
            gvEnquiryAppraisal.FunPriSetSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private string FunPriGetSortDirectionStr(string strColumn)
    {
        string strSortDirection = string.Empty;
        string strSortExpression = string.Empty;
        // By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;
        strSortDirection = "DESC";
        try
        {
            // Retrieve the last strColumn that was sorted.
            // Check if the same strColumn is being sorted.
            // Otherwise, the default value can be returned.

            strSortExpression = hdnSortExpression.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
            // Save new values in hidden control.
            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strColumn;
            strOrderBy = " " + strColumn + " " + strSortDirection;
            hdnOrderBy.Value = strOrderBy;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        return strSortDirection;
    }

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    if (arrSortCol[iCount].ToString() == "LOB_Code")
                    {
                        strSearchVal += " and LOB_Code like '%" + arrSearchVal[iCount].ToString() + "%'";
                    }
                    if (arrSortCol[iCount].ToString().Trim().Equals("(CM.Customer_Code+'-'+CM.Customer_Name)") && ddlDocumentType.SelectedValue == "6")
                    {
                        strSearchVal += " and (Customer_Code+'-'+Customer_Name)  like '%" + arrSearchVal[iCount].ToString() + "%'";
                    }
                    else
                    {
                        strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                    }
                }
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            if (ddlDocumentType.SelectedValue == "1")
            {
                FunPriBindGrid("1");

            }
            else if (ddlDocumentType.SelectedValue == "2")
            {
                FunPriBindGrid("2");

            }
            else if (ddlDocumentType.SelectedValue == "3")
            {
                FunPriBindGrid("3");

            }
            else if (ddlDocumentType.SelectedValue == "6")
            {
                FunPriBindGrid("6");

            }

            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)gvEnquiryAppraisal.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void gvEnquiryAppraisal_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gvEnquiryAppraisal = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("chkSelectRecord");
                chkBoxSelect.Attributes["onclick"] = string.Format
                                          (
                                             "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                             gvEnquiryAppraisal.ClientID
                                         );
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;
            ProPageSizeRW = intPageSize;
            if (ddlDocumentType.SelectedValue == "1")
            {
                FunPriBindGrid("1");

            }
            else if (ddlDocumentType.SelectedValue == "2")
            {
                FunPriBindGrid("2");
            }
            else if (ddlDocumentType.SelectedValue == "3")
            {
                FunPriBindGrid("3");
            }
            else if (ddlDocumentType.SelectedValue == "6")
            {
                FunPriBindGrid("6");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    protected void chkEnqNO_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkEnqNO.Checked)
        //{
        //   
        //}
    }

    protected void rdbCustomEnquiry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // btnShowAll.Visible = true;

            if (ddlDocumentType.SelectedValue == "1")
            {
                FunPriBindGrid("1");

            }
            else if (ddlDocumentType.SelectedValue == "2")
            {
                FunPriBindGrid("2");
            }
            else if (ddlDocumentType.SelectedValue == "3")
            {
                FunPriBindGrid("3");
            }
            else if (ddlDocumentType.SelectedValue == "6")
            {
                FunPriBindGrid("6");
            }
            gvEnquiryAppraisal.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            btnShowAll.Visible = true;
            gvEnquiryAppraisal.Visible = true;
            //LinkButton lnkbtnNumber = (LinkButton)gvEnquiryAppraisal.HeaderRow.FindControl("lnkbtnSort2");
            ucCustomPaging.Visible = true;
            if (Convert.ToInt32(ddlDocumentType.SelectedValue) > 0)
            {

                if (ddlDocumentType.SelectedValue == "1")
                {

                    // lnkbtnNumber.Text = "Enquiry Number";
                    FunPriBindGrid("1"); //Enquiry

                }
                else if (ddlDocumentType.SelectedValue == "2")
                {
                    //lnkbtnNumber.Text = "Pricing Number";
                    FunPriBindGrid("2"); //Pricing
                }
                else if (ddlDocumentType.SelectedValue == "3")
                {
                    //lnkbtnNumber.Text = "Application Number";
                    FunPriBindGrid("3"); //Application
                }
                else if (ddlDocumentType.SelectedValue == "6")
                {

                    FunPriBindGrid("6"); //Customer
                }
            }
            gvEnquiryAppraisal.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void ToggleColumns(bool Canshow)
    {
        try
        {
            gvEnquiryAppraisal.Columns[0].Visible = !Canshow;

            gvEnquiryAppraisal.Columns[3].Visible = Canshow;
            gvEnquiryAppraisal.Columns[4].Visible = Canshow;
            gvEnquiryAppraisal.Columns[5].Visible = Canshow;
            gvEnquiryAppraisal.Columns[6].Visible = Canshow;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void chkSelectRecord_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((CheckBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvEnquiryAppraisal_")).Replace("gvEnquiryAppraisal_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;

            foreach (GridViewRow grRow in gvEnquiryAppraisal.Rows)
            {
                if (grRow.RowIndex != gRowIndex)
                {
                    CheckBox CkBox = (CheckBox)grRow.FindControl("chkSelectRecord");
                    CkBox.Checked = false;
                }
            }

            CheckBox CkSelect = (CheckBox)gvEnquiryAppraisal.Rows[gRowIndex].FindControl("chkSelectRecord");

            if (ddlDocumentType.SelectedValue == "1")
            {
                string strRefno = ((Label)gvEnquiryAppraisal.Rows[gRowIndex].FindControl("lblEnquiryUpdationID")).Text;
                intEnquiryNo = Convert.ToInt32(strRefno);

                if (CkSelect.Checked)
                {
                    FunLoadData();
                }
                lblEnquiryno.Text = "Enquiry Number";
                rfvLOB.Enabled = false;
                rqvAmount.Enabled = false;
            }
            else if (ddlDocumentType.SelectedValue == "2")
            {
                string strRefno = ((Label)gvEnquiryAppraisal.Rows[gRowIndex].FindControl("lblEnquiryUpdationID")).Text;
                intEnquiryNo = Convert.ToInt32(strRefno);

                if (CkSelect.Checked)
                {
                    FunLoadData();
                }
                lblEnquiryno.Text = "Pricing Number";
                rfvLOB.Enabled = false;
                rqvAmount.Enabled = false;
            }
            else if (ddlDocumentType.SelectedValue == "3")
            {
                string strRefno = ((Label)gvEnquiryAppraisal.Rows[gRowIndex].FindControl("lblEnquiryUpdationID")).Text;
                intEnquiryNo = Convert.ToInt32(strRefno);

                if (CkSelect.Checked)
                {
                    FunLoadData();
                }
                lblEnquiryno.Text = "Application Number";
                rfvLOB.Enabled = false;
                rqvAmount.Enabled = false;
            }
            else
            {
                string strRefno = ((Label)gvEnquiryAppraisal.Rows[gRowIndex].FindControl("lblCustomerID")).Text;
                CustomerID = Convert.ToInt32(strRefno);

                if (CkSelect.Checked)
                {
                    FunLoadData();
                }
                rfvLOB.Enabled = true;
                rqvAmount.Enabled = true;

                FunProInitializeFacilityGroup();
            }
            CkSelect.Checked = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void FunLoadData()
    {
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowToDoClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();
        try
        {
            tbgeneral.Enabled = true;
            tbfacility.Enabled = true;
            tcEnquiryAppraisal.ActiveTab = tbgeneral;
            //tcEnquiryAppraisal_TabIndexChanged(null, null);
            SetTabIndexChanged();
            tcEnquiryAppraisal.TabIndex = 0;
            if (!blnIsWorkflowApplicable || Session["DocumentNo"] == null)
            {
                if (ddlDocumentType.SelectedValue == "1")
                {
                    strTansactionType = "Enquiry";
                    FunEnquiryCustomer(strTansactionType);
                }
                else if (ddlDocumentType.SelectedValue == "2")
                {
                    strTansactionType = "Pricing";
                    FunEnquiryCustomer(strTansactionType);
                }
                else if (ddlDocumentType.SelectedValue == "3")
                {
                    strTansactionType = "Application";
                    FunEnquiryCustomer(strTansactionType);
                }
                else
                {
                    strTansactionType = "Customer";
                    FunEnquiryCustomer(strTansactionType);
                }

                if (intEnquiryNo > 0 && Request.QueryString["qsMode"] == "C")
                {
                    FunPubdetailsForEnquiryNo(intCompanyID, intEnquiryNo, Convert.ToInt32(ddlDocumentType.SelectedValue)); // Enquiry Create Mode
                    FunPriDisableControls(0);

                }
                else if (strTansactionType == "Customer" && Request.QueryString["qsMode"] == "C") // Create Mode for Customer
                {
                    FunLoadCustomerData();
                    //FunPubPendingCustomerDetails();
                    FunLoadLOBMaster();
                    FunPriDisableControls(0);
                }
                else if (intEnquiryNo > 0 && Request.QueryString["qsMode"] == "M" && strTansactionType == "Enquiry") //Edit Mode for Enquiry
                {
                    FunPubEnquiryUpdate(intCompanyID, intEnquiryNo);
                    FunPriDisableControls(1);
                    btnClear.Enabled = false;
                }
                else if (intEnquiryNo > 0 && strTansactionType == "Customer" && Request.QueryString["qsMode"] == "M") // Edit Mode for Customer
                {
                    FunPubCustomerForUpdate(intEnquiryNo);
                    FunPriDisableControls(1);
                    btnClear.Enabled = false;
                }
                else if (intEnquiryNo > 0 && Request.QueryString["qsMode"] == "Q" && strTansactionType == "Customer") // Query Mode for Customer
                {
                    FunPubCustomerForUpdate(intEnquiryNo);
                    btnFacSave.Enabled = Rdbfurtherprocno.Enabled = Rdbfurtherprocyes.Enabled = btnClear.Enabled = false;
                    FunPriDisableControls(-1);
                }
                if (intEnquiryNo > 0 && Request.QueryString["qsMode"] == "Q" && strTansactionType == "Enquiry") // Query Mode for Enquiry
                {
                    FunPubEnquiryUpdate(intCompanyID, intEnquiryNo);
                    FunPriDisableControls(-1);
                    btnFacSave.Enabled = Rdbfurtherprocno.Enabled = Rdbfurtherprocyes.Enabled = btnClear.Enabled = false;
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    PreparePageForWFLoad();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objWorkflowToDoClient.Close();
        }
    }
    void ClearGridViewSelection()
    {
        try
        {
            foreach (GridViewRow grRow in gvEnquiryAppraisal.Rows)
            {
                CheckBox CkBox = (CheckBox)grRow.FindControl("chkSelectRecord");
                CkBox.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    void SetTabIndexChanged()
    {
        try
        {
            if (tcEnquiryAppraisal.ActiveTab == tbAppraisalType)
            {
                btnFacSave.Visible = false;
                btnClear.Visible = false;
                tbgeneral.Enabled = tbfacility.Enabled = false;
                //btnShowAll.Visible = true;
                //ClearGridViewSelection();
            }
            else if (tcEnquiryAppraisal.ActiveTab == tbgeneral)
            {
                btnFacSave.Visible = true;
                btnClear.Visible = true;
                btnShowAll.Visible = false;
            }
            else if (tcEnquiryAppraisal.ActiveTab == tbfacility)
            {
                btnFacSave.Visible = true;
                btnClear.Visible = true;
                btnShowAll.Visible = false;


            }
            if (tcEnquiryAppraisal.ActiveTabIndex == 0)
            {
                InitiateValidators();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void InitiateValidators()
    {
        try
        {
            if (!Rdbfurtherprocyes.Checked)
            {
                rfvLOB.Enabled = false;
                rqvAmount.Enabled = false;
            }
            else
            {
                rfvLOB.Enabled = true;
                rqvAmount.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void tcEnquiryAppraisal_TabIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetTabIndexChanged();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void Rdbfurtherprocno_CheckedChanged(object sender, EventArgs e)
    {
        //InitiateValidators();
    }
    protected void Rdbfurtherprocyes_CheckedChanged(object sender, EventArgs e)
    {
        //InitiateValidators();
    }

    //Added By Thangam M on 13/Mar/2012 to fix bug id- 5604
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriBindGrid(ddlDocumentType.SelectedValue);
            gvEnquiryAppraisal.FunPriClearSearchValue(arrSearchVal);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// Modified by :   Thangam M
    /// Modified on :   08/May/2014
    /// Purpose     :   Credit score changes
    /// </summary>
    
    #region Customer Facility : Credit score changes
    

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        Procparam = new Dictionary<string, string>();
        uinfo = new UserInfo();

        DropDownList ddlLOB = (DropDownList)sender;

        DropDownList ddlProduct;

        GridView grvFacility = (GridView)ddlLOB.Parent.Parent.Parent.Parent;

        if (((GridViewRow)ddlLOB.Parent.Parent).RowType == DataControlRowType.Footer)
        {
            ddlProduct = (DropDownList)grvFacility.FooterRow.FindControl("ddlFProduct");
        }
        else
        {
            ddlProduct = (DropDownList)((GridViewRow)ddlLOB.Parent.Parent).FindControl("ddlProduct");
        }

        if (strMode.ToUpper().Trim() != "Q")
            Procparam.Add("@Is_Active", "1");

        Procparam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        ddlProduct.BindDataTable(SPNames.SYS_ProductMaster, Procparam, new string[] { "Product_ID", "Product_Code", "Product_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvFacilityGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["dtGroup"] == null)
            {
                uinfo = new UserInfo();

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
                Procparam.Add("@LookupType_Code", "114");
                ViewState["dtGroup"] = dtGroup = Utility.GetDefaultData("S3G_LOANAD_GetLookUpValues", Procparam);
            }
            else
            {
                dtGroup = (DataTable)ViewState["dtGroup"];
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblHGroupId = (Label)e.Row.FindControl("lblHGroupId");
            GridView grvFacility = (GridView)e.Row.FindControl("grvFacility");
            FunProSetDataSource(grvFacility, lblHGroupId.Text);

            e.Row.Cells[0].Style.Add("padding", "0px");
            grvFacilityGroup.GridLines = GridLines.None;

            if (Request.QueryString["qsMode"] == "Q")
            {
                HtmlTableCell tdGroupDelete = (HtmlTableCell)e.Row.FindControl("tdGroupDelete");
                tdGroupDelete.Visible = false;
                tdAction.Visible = false;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlHFGroup = (DropDownList)e.Row.FindControl("ddlHFGroup");

            ddlHFGroup.BindDataTable(dtGroup.Copy(), new string[] { "Lookup_Code", "Lookup_Description" });
            ddlHFGroup.Items.RemoveAt(0);

            DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];

            for (int i = 0; i <= dtFacilityGroup.Rows.Count - 1; i++)
            {
                ddlHFGroup.Items.Remove(ddlHFGroup.Items.FindByValue(dtFacilityGroup.Rows[i]["Group_ID"].ToString()));
            }

            TextBox txtFFacilityAmount = (TextBox)e.Row.FindControl("txtFFacilityAmount");

            if (ddlHFGroup.SelectedValue == "0")
            {
                txtFFacilityAmount.Visible = false;
            }
            else
            {
                txtFFacilityAmount.Visible = true;
            }

            if (ddlHFGroup.Items.Count == 0 || Request.QueryString["qsMode"] == "Q")
            {
                grvFacilityGroup.ShowFooter = false;
            }
            else
            {
                grvFacilityGroup.ShowFooter = true;
            }
        }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void grvFacility_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            uinfo = new UserInfo();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_Id", Convert.ToString(uinfo.ProUserIdRW));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Program_ID", "47");
            dtLOB = (DataTable)ViewState["dtLOB"];
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlFLOB = (DropDownList)e.Row.FindControl("ddlFLOB");
            ddlFLOB.BindDataTable(dtLOB.Copy(), new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            GridViewRow grvGroup = (GridViewRow)e.Row.Parent.Parent.Parent.Parent;
            Label lblHGroupId = (Label)grvGroup.FindControl("lblHGroupId");
            TextBox txtFFacilityAmount = (TextBox)e.Row.FindControl("txtFFacilityAmount");

            if (lblHGroupId.Text == "0")
            {
                txtFFacilityAmount.Enabled = true;
                txtFFacilityAmount.BackColor = System.Drawing.Color.White;
            }
            else
            {
                txtFFacilityAmount.Enabled = false;
                txtFFacilityAmount.BackColor = System.Drawing.Color.Gray;
            }

            if (Request.QueryString["qsMode"] == "Q")
            {
                e.Row.Visible = false;
                GridView grvFacility = (GridView)e.Row.Parent.Parent;
                grvFacility.Columns[grvFacility.Columns.Count - 1].Visible = false;
            }
        }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
        GridView grvFacility = (GridView)((Button)sender).Parent.Parent.Parent.Parent;
        DropDownList ddlFLOB = (DropDownList)grvFacility.FooterRow.FindControl("ddlFLOB");
        DropDownList ddlFProduct = (DropDownList)grvFacility.FooterRow.FindControl("ddlFProduct");
        Label lblHGroupId = (Label)grvFacility.Parent.FindControl("lblHGroupId");
        TextBox txtFFacilityAmount = (TextBox)grvFacility.FooterRow.FindControl("txtFFacilityAmount");

        string strMsg = "";
        Control ctrl = null;
        if (ddlFLOB.SelectedValue == "0")
        {
            strMsg = "    . Select Line of Bussiness\\n";
            ctrl = ddlFLOB;
        }
        //if (ddlFProduct.Items.Count == 0 || (ddlFProduct.Items.Count > 0 && ddlFProduct.SelectedValue == "0"))
        //{
        //    strAlert = strAlert + "    . Select Product\\n";
        //    if (ctrl == null)
        //    {
        //        ctrl = ddlFProduct;
        //    }
        //}
        if (lblHGroupId.Text == "0")
        {
            if (string.IsNullOrEmpty(txtFFacilityAmount.Text))
            {
                strMsg = strMsg + "    . Enter Facility Amount\\n";
                if (ctrl == null)
                {
                    ctrl = txtFFacilityAmount;
                }
            }

            if (!string.IsNullOrEmpty(txtFFacilityAmount.Text) && Convert.ToDecimal(txtFFacilityAmount.Text) == 0)
            {
                strMsg = strMsg + "    . Facility Amount cannot be Zero";
                if (ctrl == null)
                {
                    ctrl = txtFFacilityAmount;
                }
            }
        }

        if (!string.IsNullOrEmpty(strMsg))
        {
            strMsg = "Correct the following validation(s):\\n\\n" + strMsg;
            Utility.FunShowAlertMsg(this, strMsg);
            ctrl.Focus();
            return;
        }

        DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];

        if (!dtFacilityGrid.Copy().Check("LOB_ID='" + ddlFLOB.SelectedValue + "' AND Product_ID='" + ddlFProduct.SelectedValue + "'", 0))
        {
            Utility.FunShowAlertMsg(this, "Given combination already added");
            return;
        }
        if (!dtFacilityGrid.Copy().Check("LOB_ID='" + ddlFLOB.SelectedValue + "' AND Product_ID='0'", 0))
        {
            Utility.FunShowAlertMsg(this, "Given combination already added");
            return;
        }
        if (!dtFacilityGrid.Copy().Check("LOB_ID='" + ddlFLOB.SelectedValue + "'", 0) && ddlFProduct.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this, "Given combination already added");
            return;
        }

        DataRow dRow = dtFacilityGrid.NewRow();

        dRow["SlNo"] = "1";
        dRow["LOB_ID"] = ddlFLOB.SelectedValue;
        dRow["LOB"] = ddlFLOB.SelectedItem.Text;
        dRow["Product_ID"] = ddlFProduct.SelectedValue;
        if (ddlFProduct.SelectedValue != "0")
        {
            dRow["Product"] = ddlFProduct.SelectedItem.Text;
        }
        dRow["Group_ID"] = lblHGroupId.Text;
        //dRow["Group_Text");
        dRow["Facility_Amount"] = (txtFFacilityAmount.Text == "") ? "0" : txtFFacilityAmount.Text;
        //dRow["Total_Amount");

        dtFacilityGrid.Rows.Add(dRow);

        ViewState["dtFacilityGrid"] = dtFacilityGrid;

        FunProSetDataSource(grvFacility, lblHGroupId.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void ddlHFGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        DropDownList ddlHFGroup = (DropDownList)sender;
        TextBox txtFFacilityAmount = (TextBox)((GridViewRow)ddlHFGroup.Parent.Parent).FindControl("txtFFacilityAmount");
        if (ddlHFGroup.SelectedValue == "0")
        {
            txtFFacilityAmount.Visible = false;
        }
        else
        {
            txtFFacilityAmount.Visible = true;
        }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnHAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];

            DropDownList ddlHFGroup = (DropDownList)grvFacilityGroup.FooterRow.FindControl("ddlHFGroup");
            TextBox txtFFacilityAmount = (TextBox)grvFacilityGroup.FooterRow.FindControl("txtFFacilityAmount");

            DataRow dRow = dtFacilityGroup.NewRow();
            dRow["Group_ID"] = ddlHFGroup.SelectedValue;
            dRow["Group_Text"] = ddlHFGroup.SelectedItem.Text;

            if (ddlHFGroup.SelectedValue == "0")
            {
                dRow["Total_Amount"] = "0";
            }
            else
            {
                string strMsg = "";
                if (string.IsNullOrEmpty(txtFFacilityAmount.Text))
                {
                    strMsg = "    . Enter Facility Amount\\n";
                }
                if (!string.IsNullOrEmpty(txtFFacilityAmount.Text) && Convert.ToDecimal(txtFFacilityAmount.Text) == 0)
                {
                    strMsg = "    . Facility Amount cannot be Zero";
                }

                if (!string.IsNullOrEmpty(strMsg))
                {
                    strMsg = "Correct the following validation(s):\\n\\n" + strMsg;
                    Utility.FunShowAlertMsg(this, strMsg);
                    txtFFacilityAmount.Focus();
                    return;
                }

                dRow["Total_Amount"] = txtFFacilityAmount.Text;
            }

            dtFacilityGroup.Rows.Add(dRow);

            grvFacilityGroup.DataSource = dtFacilityGroup;
            grvFacilityGroup.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProInitializeFacilityGroup()
    {
        try
        {
        DataTable dtGridData = new DataTable();

        dtGridData.Columns.Add("Group_ID");
        dtGridData.Columns.Add("Group_Text");
        dtGridData.Columns.Add("Total_Amount", typeof(decimal));

        ViewState["dtFacilityGroup"] = dtGridData.Copy();
        ViewState["dtFacilityGrid"] = null;

        DataRow dRow = dtGridData.NewRow();
        dtGridData.Rows.Add(dRow);

        grvFacilityGroup.DataSource = dtGridData;
        grvFacilityGroup.DataBind();
        grvFacilityGroup.Rows[0].Visible = false;
        dtGridData.Rows.Clear();

        lblTotalAmount.Text = "0";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProInitializeFacilityGrid(GridView grv, string strGroup)
    {
        try
        {
        DataTable dtGridData = new DataTable();

        dtGridData.Columns.Add("SlNo");
        dtGridData.Columns.Add("LOB_ID");
        dtGridData.Columns.Add("LOB");
        dtGridData.Columns.Add("Product_ID");
        dtGridData.Columns.Add("Product");
        dtGridData.Columns.Add("Group_ID");
        dtGridData.Columns.Add("Group_Text");
        dtGridData.Columns.Add("Facility_Amount", typeof(decimal));
        dtGridData.Columns.Add("Total_Amount");

        DataRow dRow = dtGridData.NewRow();
        dtGridData.Rows.Add(dRow);

        grv.DataSource = dtGridData;
        grv.DataBind();
        grv.Rows[0].Visible = false;
        dtGridData.Rows.Clear();

        if (ViewState["dtFacilityGrid"] == null)
            ViewState["dtFacilityGrid"] = dtGridData;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProSetDataSource(GridView grv, string strGroup)
    {
        try
        {
        if (ViewState["dtFacilityGrid"] == null)
        {
            FunProInitializeFacilityGrid(grv, strGroup);
        }

        if (strGroup == "")
        {
            strGroup = "0";
        }

        DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];
        DataRow[] dRows = dtFacilityGrid.Select("Group_ID = '" + strGroup + "'");
        decimal dcTotal = 0;
        if (dRows.Length > 0)
        {
            DataTable dtFilered = dRows.CopyToDataTable();

            grv.DataSource = dtFilered;
            grv.DataBind();

            //To update sub-total
            dcTotal = (decimal)dtFacilityGrid.Compute("SUM(Facility_Amount)", "Group_ID = '" + strGroup + "'");
        }
        else
        {
            FunProInitializeFacilityGrid(grv, strGroup);
        }

        DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];

        foreach (DataRow drow in dtFacilityGroup.Rows)
        {
            if (drow["Group_ID"].ToString() == strGroup && strGroup == "0")
            {
                drow["Total_Amount"] = dcTotal.ToString();
            }
        }

        if (strGroup == "0")
        {
            Label lblHFacilityAmount = (Label)grv.Parent.FindControl("lblHFacilityAmount");
            lblHFacilityAmount.Text = dcTotal.ToString();
        }

        if (dtFacilityGroup.Rows.Count > 0)
        {
            dcTotal = (decimal)dtFacilityGroup.Compute("SUM(Total_Amount)", "1 = 1");
            lblTotalAmount.Text = dcTotal.ToString();
        }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
        GridView grvFacility = (GridView)((ImageButton)sender).Parent.Parent.Parent.Parent.Parent;
        DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];
        GridViewRow grvRow = (GridViewRow)((ImageButton)sender).Parent.Parent.Parent;

        Label lblLOBId = (Label)grvRow.FindControl("lblLOBId");
        Label lblProductId = (Label)grvRow.FindControl("lblProductId");
        Label lblHGroupId = (Label)grvFacility.Parent.FindControl("lblHGroupId");

        dtFacilityGrid.Delete("LOB_ID='" + lblLOBId.Text + "' AND Product_ID='" + lblProductId.Text + "' AND Group_ID='" + lblHGroupId.Text + "'");

        ViewState["dtFacilityGrid"] = dtFacilityGrid;

        FunProSetDataSource(grvFacility, lblHGroupId.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnGroupDelete_OnClick(object sender, EventArgs e)
    {
        try
        {
        GridViewRow grvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];
        DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];
        Label lblHGroupId = (Label)grvRow.FindControl("lblHGroupId");

        dtFacilityGroup.Rows[grvRow.RowIndex].Delete();
        ViewState["dtFacilityGroup"] = dtFacilityGroup;

        dtFacilityGrid.Delete("Group_ID='" + lblHGroupId.Text + "'");
        ViewState["dtFacilityGrid"] = dtFacilityGrid;

        if (dtFacilityGroup.Rows.Count == 0)
        {
            FunProInitializeFacilityGroup();
        }
        else
        {
            grvFacilityGroup.DataSource = dtFacilityGroup;
            grvFacilityGroup.DataBind();
        }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnEdit_OnClick(object sender, EventArgs e)
    {
        try
        {
        FunProActionVisibility(sender, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnUpdate_OnClick(object sender, EventArgs e)
    {
        try
        {
        DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];

        GridView grvFacility = (GridView)((ImageButton)sender).Parent.Parent.Parent.Parent.Parent;
        GridViewRow grvRow = (GridViewRow)((ImageButton)sender).Parent.Parent.Parent;
        Panel pnlItemActions = (Panel)grvRow.FindControl("pnlItemActions");
        Panel pnlEditActions = (Panel)grvRow.FindControl("pnlEditActions");

        Label lblHGroupId = (Label)grvFacility.Parent.FindControl("lblHGroupId");

        Label lblLOBId = (Label)grvRow.FindControl("lblLOBId");
        Label lblProductId = (Label)grvRow.FindControl("lblProductId");
        Label lblLOB = (Label)grvRow.FindControl("lblLOB");
        Label lblProduct = (Label)grvRow.FindControl("lblProduct");
        Label lblFacilityAmount = (Label)grvRow.FindControl("lblFacilityAmount");
        DropDownList ddlLOB = (DropDownList)grvRow.FindControl("ddlLOB");
        DropDownList ddlProduct = (DropDownList)grvRow.FindControl("ddlProduct");
        TextBox txtFacilityAmount = (TextBox)grvRow.FindControl("txtFacilityAmount");

        string strMsg = "";
        Control ctrl = null;
        if (ddlLOB.SelectedValue == "0")
        {
            strMsg = "    . Select Line of Bussiness\\n";
            ctrl = ddlLOB;
        }
        //if (ddlProduct.Items.Count == 0 || (ddlProduct.Items.Count > 0 && ddlProduct.SelectedValue == "0"))
        //{
        //    strAlert = strAlert + "    . Select Product\\n";
        //    if (ctrl == null)
        //    {
        //        ctrl = ddlProduct;
        //    }
        //}
        if (lblHGroupId.Text == "0")
        {
            if (string.IsNullOrEmpty(txtFacilityAmount.Text))
            {
                strMsg = strMsg + "    . Enter Facility Amount\\n";
                if (ctrl == null)
                {
                    ctrl = txtFacilityAmount;
                }
            }
            if (!string.IsNullOrEmpty(txtFacilityAmount.Text) && Convert.ToDecimal(txtFacilityAmount.Text) == 0)
            {
                strMsg = strMsg + "    . Facility Amount cannot be Zero";
                if (ctrl == null)
                {
                    ctrl = txtFacilityAmount;
                }
            }
        }

        if (!string.IsNullOrEmpty(strMsg))
        {
            strMsg = "Correct the following validation(s):\\n\\n" + strMsg;
            Utility.FunShowAlertMsg(this, strMsg);
            ctrl.Focus();
            return;
        }

        DataTable dtTemp = dtFacilityGrid.Copy();

        dtTemp.Update("LOB_ID='" + lblLOBId.Text + "' AND Product_ID='" + lblProductId.Text + "' AND Group_ID='" + lblHGroupId.Text + "'", ddlLOB.SelectedValue, ddlLOB.SelectedItem.Text, ddlProduct.SelectedValue, ddlProduct.SelectedItem.Text, txtFacilityAmount.Text);

        if (!dtTemp.Check("LOB_ID='" + ddlLOB.SelectedValue + "' AND Product_ID='" + ddlProduct.SelectedValue + "'", 1))
        {
            Utility.FunShowAlertMsg(this, "Given combination already added");
            return;
        }

        if (!dtTemp.Check("LOB_ID='" + ddlLOB.SelectedValue + "' AND Product_ID='0'", 0) && ddlProduct.SelectedValue != "0") 
        {
            Utility.FunShowAlertMsg(this, "Given combination already added");
            return;
        }
        if (!dtFacilityGrid.Copy().Check("LOB_ID='" + ddlLOB.SelectedValue + "'", 0) && ddlProduct.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this, "Given combination already added");
            return;
        }

        dtFacilityGrid.Update("LOB_ID='" + lblLOBId.Text + "' AND Product_ID='" + lblProductId.Text + "' AND Group_ID='" + lblHGroupId.Text + "'", ddlLOB.SelectedValue, ddlLOB.SelectedItem.Text, ddlProduct.SelectedValue, ddlProduct.SelectedItem.Text, (txtFacilityAmount.Text == "") ? "0" : txtFacilityAmount.Text);

        ViewState["dtFacilityGrid"] = dtFacilityGrid;

        FunProActionVisibility(sender, false);

        FunProSetDataSource(grvFacility, lblHGroupId.Text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void imgbtnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
        FunProActionVisibility(sender, false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProActionVisibility(object sender, bool blVisible)
    {
        try
        {

            GridView grvFacility = (GridView)((ImageButton)sender).Parent.Parent.Parent.Parent.Parent;
            GridViewRow grvRow = (GridViewRow)((ImageButton)sender).Parent.Parent.Parent;
            Panel pnlItemActions = (Panel)grvRow.FindControl("pnlItemActions");
            Panel pnlEditActions = (Panel)grvRow.FindControl("pnlEditActions");

            Label lblLOBId = (Label)grvRow.FindControl("lblLOBId");
            Label lblProductId = (Label)grvRow.FindControl("lblProductId");
            Label lblLOB = (Label)grvRow.FindControl("lblLOB");
            Label lblProduct = (Label)grvRow.FindControl("lblProduct");
            Label lblFacilityAmount = (Label)grvRow.FindControl("lblFacilityAmount");
            DropDownList ddlLOB = (DropDownList)grvRow.FindControl("ddlLOB");
            DropDownList ddlProduct = (DropDownList)grvRow.FindControl("ddlProduct");
            TextBox txtFacilityAmount = (TextBox)grvRow.FindControl("txtFacilityAmount");

            pnlItemActions.Visible = lblLOB.Visible = lblProduct.Visible = lblFacilityAmount.Visible = !blVisible;
            pnlEditActions.Visible = ddlLOB.Visible = ddlProduct.Visible = txtFacilityAmount.Visible = blVisible;

            GridViewRow grvGroup = (GridViewRow)grvRow.Parent.Parent.Parent.Parent;
            Label lblHGroupId = (Label)grvGroup.FindControl("lblHGroupId");

            if (blVisible)
            {
                dtLOB = (DataTable)ViewState["dtLOB"];
                ddlLOB.BindDataTable(dtLOB.Copy(), new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
                ddlLOB.SelectedValue = lblLOBId.Text;
                ddlLOB_SelectedIndexChanged(ddlLOB, null);
                ddlProduct.SelectedValue = lblProductId.Text;
                grvRow.BackColor = grvFacility.FooterStyle.BackColor;

                if (lblHGroupId.Text == "0")
                {
                    txtFacilityAmount.Enabled = true;
                    txtFacilityAmount.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    txtFacilityAmount.Enabled = false;
                    txtFacilityAmount.BackColor = System.Drawing.Color.Gray;
                }
            }
            else
            {
                grvRow.BackColor = System.Drawing.Color.White;
            }

            grvFacility.FooterRow.Visible = !blVisible;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion
}

#region Extension Methods for DataTable
static class clsExtensions
{
    public static DataTable Delete(this DataTable table, string filter)
    {
        table.Select(filter).Delete();
        return table;
    }
    public static void Delete(this IEnumerable<DataRow> rows)
    {
        foreach (var row in rows)
            row.Delete();
    }

    public static DataTable Update(this DataTable table, string filter, string strLOBID, string strLOB, string strProductID, string strProduct, string strAmount)
    {
        table.Select(filter).Update(strLOBID, strLOB, strProductID, strProduct, strAmount);
        return table;
    }

    public static void Update(this IEnumerable<DataRow> rows, string strLOBID, string strLOB, string strProductID, string strProduct, string strAmount)
    {
        foreach (var row in rows)
        {
            row["LOB_ID"] = strLOBID;
            row["LOB"] = strLOB;
            row["Product_ID"] = strProductID;
            if (strProductID != "0")
                row["Product"] = strProduct;
            else
                row["Product"] = "";
            row["Facility_Amount"] = strAmount;
        }
    }

    public static DataTable Update(this DataTable table, string filter, string strColumnName, string strValue)
    {
        table.Select(filter).Update(filter, strColumnName, strValue);
        return table;
    }

    public static void Update(this IEnumerable<DataRow> rows, string filter, string strColumnName, string strValue)
    {
        foreach (var row in rows)
        {
            row[strColumnName] = strValue;
        }
    }

    public static bool Check(this DataTable table, string filter, int intOccurance)
    {
        if (table.Select(filter).Length > intOccurance)
            return false;
        else
            return true;
    }
}
#endregion
