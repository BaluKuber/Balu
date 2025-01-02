#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Document Number Control Creation
/// Created By			: Kannan RC
/// Created Date		: 13-May-2010
/// Purpose	            : 
/// 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 12-July-2010   
/// Reason              : Add Role Access setup 
/// 
/// Last Updated By		: Gunasekar.K
/// Last Updated Date   : 12-Oct-2010   
/// Reason              : To Address the Company Level scenario
/// 
/// Last Updated By		: Gunasekar.K
/// Last Updated Date   : 15-Oct-2010   
/// Reason              : To Address the Bug Ids 1737 and 1738
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;
using S3GBusEntity;
#endregion

public partial class System_Admin_S3G_SysAdminDocumentNumberControl_Add : ApplyThemeForProject
{
    #region Intialization
    DocMgtServicesReference.DocMgtServicesClient ObjDocumentNumberControlClient;
    DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable ObjS3G_DocTypeListDataTable = new DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable();
    DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable ObjS3G_GetDNCDataTable = new DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable();
    DocMgtServices.S3G_SYSAD_DNCMasterDataTable ObjS3G_DNCMasterDataTable = new DocMgtServices.S3G_SYSAD_DNCMasterDataTable();
    DocMgtServices.S3G_SYSAD_GetBatch_ListDataTable ObjS3G_BatchMasterDataTable = new DocMgtServices.S3G_SYSAD_GetBatch_ListDataTable();
    int intDocSeqId = 0;
    int intErrCode = 0;
    string _ViewMode = "";
    int intUserID = 0;
    int intCompanyID = 0;
    int inthdUserid;
    string strRedirectPageMC;
    string strMode = string.Empty;
    UserInfo ObjUserInfo = null;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bClearList = false;
    bool bMakerChecker = false;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminDocumentNumberControl_View.aspx'";
    public static System_Admin_S3G_SysAdminDocumentNumberControl_Add obj_Page;
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Request.QueryString["qsDNCId"] != null)
            {

                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsDNCId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intDocSeqId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            txtBatch.Attributes.Add("onblur", "Trim('" + txtBatch.ClientID + "');");
            ObjUserInfo = new UserInfo();
            btnDelete.Visible = false;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            ddlBatch.Visible = false;
            rfvddlBatch.Visible = false;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            obj_Page = this;
            if (!IsPostBack)
            {
                FunPriGetLOBList();
                //FunPriGetBranchList();
                FunPriGetDocTypeList();
                FunPriGetBatchList();
                ddlFinYear.FillFinancialYears();

                /// <summary> 
                ///  This method used for set role access
                /// </summary>

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (((intDocSeqId > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if (((intDocSeqId > 0)) && (strMode == "Q"))
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;

        }

    }
    #endregion

    #region Validation
    /// <summary>
    /// Validation in create and modify
    /// </summary>

    private void validtion()
    {
        ddlLOB.Enabled = false;
        ddlBranch.Enabled = false;
        ddlDocType.Enabled = false;
        ddlFinYear.Enabled = false;
        txtBatch.Enabled = false;
        txtFromNo.Enabled = false;
        //txtToNo.Enabled = false;
        txtToNo.Enabled = true;
        txtLastNo.Enabled = false;
        rfvBatch.Visible = false;
        rfvDocType.Visible = false;
        //rfvFinYear.Visible = false;
        //rfvFromNos.Visible = false;
        //rfvToNo.Visible = false;
        //btnDelete.Enabled = false;
        CbxActive.Enabled = true;
    }

    private void Modifyvalidtion()
    {
        ddlLOB.Enabled = false;
        ddlBranch.Enabled = false;
        ddlDocType.Enabled = false;
        ddlFinYear.Enabled = false;
        txtBatch.Enabled = false;
        txtFromNo.Enabled = true;
        txtLastNo.Enabled = false;
        txtToNo.Enabled = true;
        rfvBatch.Visible = false;
        rfvDocType.Visible = false;
        //rfvFinYear.Visible = false;
        //rfvFromNos.Visible = false;
        //rfvToNo.Visible = false;
        txtLastNo.Text = "";


    }
    #endregion

    #region Page Methods

    private void FunPriGetLOBList()
    {
        try
        {
            /*
            CompanyMgtServices.S3G_SYSAD_LOBMasterRow ObjLOBMasterRow;
            ObjLOBMasterRow = ObjS3G_LOBMasterListDataTable.NewS3G_SYSAD_LOBMasterRow();
            ObjLOBMasterRow.Company_ID = intCompanyID;        
            if (intDocSeqId == 0)
            {
                ObjLOBMasterRow.Is_Active = true;
            }
            ObjS3G_LOBMasterListDataTable.AddS3G_SYSAD_LOBMasterRow(ObjLOBMasterRow);

            objLOB_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;

            byte[] bytesLOBListDetails = objLOB_MasterClient.FunPubQueryLOB_LIST(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMasterListDataTable, SerMode));
            ObjS3G_LOBMasterListDataTable = (CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable)ClsPubSerialize.DeSerialize(bytesLOBListDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable));

            ddlLOB.FillDataTable(ObjS3G_LOBMasterListDataTable, ObjS3G_LOBMasterListDataTable.LOB_IDColumn.ColumnName, ObjS3G_LOBMasterListDataTable.LOBCode_LOBNameColumn.ColumnName);
            objLOB_MasterClient.Close();
            */

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (intDocSeqId == 0)
            {
                dictParam.Add("@Is_Active", "1");
            }
            //--Added by Guna on 15th-Oct-2010 to address the Bud -1738
            //if (strMode != "M" && strMode != "Q")
            //{
            //    dictParam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            //}
            dictParam.Add("@Program_ID", "10");
            ////dictParam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            //--Ends Here

            ddlLOB.BindDataTable("S3G_Get_LOB_LIST", dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.AddItemToolTip();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable To Load Line of Business");
        }


    }

    //private void FunPriGetBranchList()
    //{
    //    try
    //    {
    //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
    //        dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
    //        if (intDocSeqId == 0)
    //        {
    //            dictParam.Add("@Is_Active", "1");
    //        }

    //        //--Added by Guna on 15th-Oct-2010 to address the Bud -1738
    //        //if (strMode != "M" && strMode != "Q")
    //        //{
    //        //    dictParam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
    //        //}
    //        dictParam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
    //        //--Ends Here
    //        dictParam.Add("@Program_ID", "10");
    //        ddlBranch.BindDataTable("S3G_Get_Branch_List", dictParam, new string[] { "Location_ID", "Location" });
    //        ddlBranch.AddItemToolTip();
    //        //.BindDataTable("S3G_Get_LOB_LIST", dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
    //        //CompanyMgtServices.S3G_SYSAD_Branch_ListRow ObjBranchListRow;
    //        //ObjBranchListRow = ObjS3G_BranchList.NewS3G_SYSAD_Branch_ListRow();
    //        //ObjBranchListRow.Company_ID = intCompanyID;
    //        //ObjS3G_BranchList.AddS3G_SYSAD_Branch_ListRow(ObjBranchListRow);
    //        //objBranchList_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
    //        //SerializationMode SerMode = SerializationMode.Binary;
    //        //byte[] bytesBranchListDetails = objBranchList_MasterClient.FunPubBranch_List(SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchList, SerMode));
    //        //ObjS3G_BranchList = (CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable)ClsPubSerialize.DeSerialize(bytesBranchListDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable));
    //        //ddlBranch.FillDataTable(ObjS3G_BranchList, ObjS3G_BranchList.Branch_IDColumn.ColumnName.Trim(), ObjS3G_BranchList.BranchColumn.ColumnName.Trim());

    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        throw new ApplicationException("Unable To Load Branch");
    //    }
    //}

    private void FunPriGetDocTypeList()
    {
        ObjDocumentNumberControlClient = new DocMgtServicesReference.DocMgtServicesClient();
        try
        {
            DocMgtServices.S3G_SYSAD_DocumentationType_ListRow ObjDocTypeListRow;
            ObjDocTypeListRow = ObjS3G_DocTypeListDataTable.NewS3G_SYSAD_DocumentationType_ListRow();
            ObjS3G_DocTypeListDataTable.AddS3G_SYSAD_DocumentationType_ListRow(ObjDocTypeListRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] bytesDocTypeListDetails = ObjDocumentNumberControlClient.FunPubDocTypeList(SerMode, ClsPubSerialize.Serialize(ObjS3G_DocTypeListDataTable, SerMode));
            ObjS3G_DocTypeListDataTable = (DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable)ClsPubSerialize.DeSerialize(bytesDocTypeListDetails, SerializationMode.Binary, typeof(DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable));
            ddlDocType.FillDataTable(ObjS3G_DocTypeListDataTable, ObjS3G_DocTypeListDataTable.Document_Type_IDColumn.ColumnName.Trim(), ObjS3G_DocTypeListDataTable.DocumenTypeColumn.ColumnName.Trim());
            ddlDocType.AddItemToolTip();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable To Load Document Type");
        }
        finally
        {
            // if (ObjDocumentNumberControlClient != null)
            ObjDocumentNumberControlClient.Close();
        }
    }

    private void GetDateYear()
    {
        try
        {
            ListItem liPSelect = new ListItem(((DateTime.Now.Year - 1) + "-" + (DateTime.Now.Year)), "1");
            ddlFinYear.Items.Insert(1, liPSelect);

            ListItem liCSelect = new ListItem(((DateTime.Now.Year) + "-" + (DateTime.Now.Year + 1)), "1");
            ddlFinYear.Items.Insert(2, liCSelect);

            ListItem liNSelect = new ListItem(((DateTime.Now.Year + 1) + "-" + (DateTime.Now.Year + 2)), "1");
            ddlFinYear.Items.Insert(3, liNSelect);


        }
        catch (Exception exp)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(exp);
            lblErrorMessage.Text = exp.Message;
        }

    }

    private void FunPriGetBatchList()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            ddlBatch.BindDataTable("S3G_GetBatch_List", dictParam, new string[] { "Doc_Number_Seq_ID", "Batch" });
            ListItem lit = new ListItem("--Add Batch--", "-1");
            ddlBatch.Items.Insert(ddlBatch.Items.Count, lit);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable To Load Batch");
        }

    }

    #region Get DNC
    /// <summary>
    /// Get DNC Details 
    /// </summary>
    private void FunGetEscalationDetatils()
    {
        ObjDocumentNumberControlClient = new DocMgtServicesReference.DocMgtServicesClient();
        try
        {
            DocMgtServices.S3G_SYSAD_Get_DNCDetailsRow ObjDNCDetailsRow;
            ObjDNCDetailsRow = ObjS3G_GetDNCDataTable.NewS3G_SYSAD_Get_DNCDetailsRow();
            ObjDNCDetailsRow.Doc_Number_Seq_ID = intDocSeqId;
            ObjS3G_GetDNCDataTable.AddS3G_SYSAD_Get_DNCDetailsRow(ObjDNCDetailsRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteDNCDetails = ObjDocumentNumberControlClient.FunPubGetDNCDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_GetDNCDataTable, SerMode));
            ObjS3G_GetDNCDataTable = (DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable)ClsPubSerialize.DeSerialize(byteDNCDetails, SerializationMode.Binary, typeof(DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable));
            if (ObjS3G_GetDNCDataTable.Rows[0]["LOB"].ToString() == "0 - 0")
            {
                ddlLOB.SelectedItem.Text = "--Select--";
            }
            else
            {
                ddlLOB.SelectedValue = ObjS3G_GetDNCDataTable.Rows[0]["LOB_ID"].ToString();
            }

            if (ObjS3G_GetDNCDataTable.Rows[0]["Location"].ToString() == "0 - 0")
            {
                ddlBranch.SelectedValue = "0";
            }
            else
            {
                if (Convert.ToString(ObjS3G_GetDNCDataTable.Rows[0]["Location"]).Contains("0 - 0"))               
                     ddlBranch.SelectedValue = "0";                
                else              
                    if (!string.IsNullOrEmpty(ObjS3G_GetDNCDataTable.Rows[0]["Location_ID"].ToString()))
                    {
                        ddlBranch.SelectedValue = ObjS3G_GetDNCDataTable.Rows[0]["Location_ID"].ToString();
                        ddlBranch.SelectedText = ObjS3G_GetDNCDataTable.Rows[0]["Location"].ToString();
                    }
                    else
                        ddlBranch.SelectedValue = "0";  
                                         
            }
            ddlDocType.SelectedValue = ObjS3G_GetDNCDataTable.Rows[0]["Document_Type_ID"].ToString();

            if (string.IsNullOrEmpty(ObjS3G_GetDNCDataTable.Rows[0]["Fin_Year"].ToString()))
            {
                ddlFinYear.SelectedItem.Text = "--Select--";
            }
            else
            {
                ddlFinYear.SelectedItem.Text = ObjS3G_GetDNCDataTable.Rows[0]["Fin_Year"].ToString();
            }

            txtBatch.Text = ObjS3G_GetDNCDataTable.Rows[0]["Batch"].ToString();
            txtFromNo.Text = ObjS3G_GetDNCDataTable.Rows[0]["From_Number"].ToString();
            txtToNo.Text = ObjS3G_GetDNCDataTable.Rows[0]["To_Number"].ToString();
            txtLastNo.Text = ObjS3G_GetDNCDataTable.Rows[0]["Last_Used_Number"].ToString();
            hdnID.Value = ObjS3G_GetDNCDataTable.Rows[0]["Created_By"].ToString();
            if (ObjS3G_GetDNCDataTable.Rows[0]["Is_Active"].ToString() == "True")
                CbxActive.Checked = true;
            else
                CbxActive.Checked = false;

            if (txtLastNo.Text == "0")
            {
                Modifyvalidtion();
            }
            else
            {
                validtion();
            }


        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            // if(ObjDocumentNumberControlClient!=null)
            ObjDocumentNumberControlClient.Close();
        }
    }

    #endregion

    private bool FunCheckLobisvalid(string strLobId)
    {
        bool blnResult = false;
        try
        {
            Dictionary<string, string> Procparm = new Dictionary<string, string>();
            Procparm.Add("@Company_ID", intCompanyID.ToString());
            Procparm.Add("@User_Id", intUserID.ToString());
            Procparm.Add("@LOB_ID", strLobId);
            if (intDocSeqId == 0)
            {
                Procparm.Add("@Is_Active", "1");
            }
            Procparm.Add("@Is_UserLobActive", "1");
            Procparm.Add("@Program_ID", "10");
            DataTable dtBool = Utility.GetDefaultData("S3G_GetUserLobMapping", Procparm);
            if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
                blnResult = true;
            else
                blnResult = false;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        return blnResult;

    }

    private bool FunCheckBranchisvalid(string strBranchId)
    {
        bool blnResult = false;
        try
        {
            Dictionary<string, string> Procparm = new Dictionary<string, string>();
            Procparm.Add("@Company_ID", intCompanyID.ToString());
            Procparm.Add("@User_Id", intUserID.ToString());
            Procparm.Add("@Location_ID", strBranchId);
            if (intDocSeqId == 0)
            {
                Procparm.Add("@Is_Active", "1");
            }
            Procparm.Add("@Program_ID", "10");
            Procparm.Add("@Is_UserLobActive", "1");
            DataTable dtBool = Utility.GetDefaultData("S3G_GetUserBranchMapping", Procparm);
            if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
                blnResult = true;
            else
                blnResult = false;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        return blnResult;

    }

    #region ValueDisable
    /// <summary>
    /// this method using role access
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    CbxActive.Checked = true;
                    CbxActive.Enabled = false;
                    txtLastNo.Enabled = false;
                    rfvDocType.Visible = true;
                    //rfvFinYear.Visible = true;
                    btnDelete.Visible = false;
                    ddlBatch.Visible = false;
                    CbxActive.Enabled = false;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    break;

                case 1: // Modify Mode
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    CbxActive.Enabled = true;
                    FunGetEscalationDetatils();
                    btnClear.Enabled = false;
                    if (bDelete)
                        btnDelete.Visible = true;
                    ddlBatch.Visible = false;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    break;

                case -1:// Query Mode
                    FunGetEscalationDetatils();
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPageView);
                    }


                    CbxActive.Enabled = false;
                    ddlFinYear.Enabled = true;
                    ddlDocType.Enabled = true;
                    ddlBranch.Enabled = false;
                    ddlLOB.Enabled = true;
                    txtFromNo.ReadOnly = true;
                    txtLastNo.ReadOnly = true;
                    txtToNo.ReadOnly = true;
                    txtBatch.ReadOnly = true;
                    txtBatch.Enabled = true;
                    txtFromNo.Enabled = true;
                    txtLastNo.Enabled = true;
                    txtToNo.Enabled = true;
                    btnDelete.Visible = false;
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    if (bClearList)
                    {
                        ddlLOB.ClearDropDownList();
                        //ddlBranch.Clear();
                        ddlDocType.ClearDropDownList();
                        ddlFinYear.ClearDropDownList();
                    }
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    break;
            }
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }

    }
    #endregion

    #endregion

    #region Page Events

    #region Save/Delete/Clear
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../System Admin/S3G_SysAdminDocumentNumberControl_View.aspx");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #region Create DNC
    /// <summary>
    /// Create New DNC Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjDocumentNumberControlClient = new DocMgtServicesReference.DocMgtServicesClient();
        bool bNumberValidation = false;
        try
        {
            //--Added by Guna on 15th-Oct-2010 to address the Bug ID 1737
            if ((Convert.ToInt64(txtFromNo.Text) == 0) && (Convert.ToInt64(txtToNo.Text) == 0))
            {
                cvDNC.ErrorMessage = "From Number and To Number should be greater than 0";
                cvDNC.IsValid = false;
                return;
            }
            //--Ends Here
            if (Convert.ToInt64(txtFromNo.Text) == 0)
            {
                cvDNC.ErrorMessage = "From Number should be greater than 0";
                cvDNC.IsValid = false;
                return;
            }
            if (Convert.ToInt64(txtToNo.Text) == 0)
            {
                cvDNC.ErrorMessage = "To Number should be greater than 0";
                cvDNC.IsValid = false;
                return;
            }
            if (bNumberValidation == true)
            {

                return;
            }

            if (Convert.ToInt64(txtFromNo.Text) == Convert.ToInt64(txtToNo.Text))
            {
                cvDNC.ErrorMessage = "To Number should be greater than From Number";
                cvDNC.IsValid = false;
                return;
            }


            if (Convert.ToInt64(txtFromNo.Text) > Convert.ToInt64(txtToNo.Text))
            {
                cvDNC.ErrorMessage = "To Number should be greater than From Number";
                cvDNC.IsValid = false;
                return;
            }

            if (intDocSeqId > 0)
            {
                cvDNC.ErrorMessage = "";
            }
            else
            {
                //-----Commend By Guna on 12-Oct-2010 To address the Company Level Scenario
                //-----Commend the Below lines
                ////if (ddlBranch.SelectedValue == "0" && ddlLOB.SelectedValue == "0")
                ////{
                ////    cvDNC.ErrorMessage = "Select the Line of Business or Branch";
                ////    cvDNC.IsValid = false;
                ////    return;
                ////}
                //----Ends Here
            }
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminDocumentNumberControl_View.aspx';";
            string strRedirectPageAdd = "window.location.href='../System Admin/S3G_SysAdminDocumentNumberControl_Add.aspx';";


            DocMgtServices.S3G_SYSAD_DNCMasterRow ObjDNCMasterRow;
            ObjDNCMasterRow = ObjS3G_DNCMasterDataTable.NewS3G_SYSAD_DNCMasterRow();
            ObjDNCMasterRow.Company_ID = intCompanyID;
            ObjDNCMasterRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjDNCMasterRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjDNCMasterRow.Document_Type_ID = Convert.ToInt32(ddlDocType.SelectedValue);
            if (ddlFinYear.SelectedIndex > 0)
                ObjDNCMasterRow.Fin_Year = ddlFinYear.SelectedItem.Text;
            else
                ObjDNCMasterRow.Fin_Year = "";
            ObjDNCMasterRow.Batch = txtBatch.Text;
            ObjDNCMasterRow.From_Number = Convert.ToDecimal(txtFromNo.Text);
            ObjDNCMasterRow.To_Number = Convert.ToDecimal(txtToNo.Text);

            //Modified by Nataraj Y on 29/10/2010 To address the Company Level Scenario
            if (Convert.ToInt32(ddlLOB.SelectedValue) == 0 && Convert.ToInt32(ddlBranch.SelectedValue) == 0)
            {
                ObjDNCMasterRow.Level = "C";
            }
            //ends here
            else if (Convert.ToInt32(ddlBranch.SelectedValue) == 0)
            {
                ObjDNCMasterRow.Level = "L";
            }
            else if (Convert.ToInt32(ddlLOB.SelectedValue) == 0)
            {
                ObjDNCMasterRow.Level = "B";
            }
            else if (Convert.ToInt32(ddlLOB.SelectedValue) != 0 && Convert.ToInt32(ddlBranch.SelectedValue) != 0)
            {
                ObjDNCMasterRow.Level = "S";
            }
            //-----Added By Guna on 12-Oct-2010 To address the Company Level Scenario
            //if (Convert.ToInt32(ddlLOB.SelectedValue) == 0 && Convert.ToInt32(ddlBranch.SelectedValue) == 0)
            //{
            //    ObjDNCMasterRow.Level = "C";
            //}
            //-- Ends Here 

            //-----Added By kannan RC on 14-dec-2010 To Last used number case
            if (intDocSeqId > 0)
            {
                if (txtLastNo.Text.Length > 0)
                {
                    if (Convert.ToInt64(txtLastNo.Text) > Convert.ToInt64(txtToNo.Text))
                    {
                        cvDNC.ErrorMessage = "To Number should be greater than Last used Number";
                        cvDNC.IsValid = false;
                        return;
                    }
                    if (Convert.ToInt64(txtLastNo.Text) == Convert.ToInt64(txtToNo.Text))
                    {
                        cvDNC.ErrorMessage = "To Number should be greater than Last used Number";
                        cvDNC.IsValid = false;
                        return;
                    }
                }

            }
            //---End here

            if (txtLastNo.Text == "")
            {
                ObjDNCMasterRow.Last_Used_Number = "0";
            }
            else
            {
                ObjDNCMasterRow.Last_Used_Number = txtLastNo.Text;                //validtion();
                //cvDNC.ErrorMessage = "Cannot modify this Document Number, its already used in some transaction.";
                //cvDNC.IsValid = false;
                //return;

            }
            ObjDNCMasterRow.Created_By = intUserID;
            ObjDNCMasterRow.Created_On = DateTime.Now;
            ObjDNCMasterRow.Doc_Number_Seq_ID = intDocSeqId;
            ObjDNCMasterRow.Modified_By = intUserID;
            ObjDNCMasterRow.Modified_On = DateTime.Now;
            ObjDNCMasterRow.Is_Active = CbxActive.Checked;


            ObjS3G_DNCMasterDataTable.AddS3G_SYSAD_DNCMasterRow(ObjDNCMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;

            if (intDocSeqId > 0)
            {
                if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
                {
                    if (FunCheckLobisvalid(ddlLOB.SelectedValue))
                    {

                        intErrCode = ObjDocumentNumberControlClient.FunPubModifyDNCDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_DNCMasterDataTable, SerMode));
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Selected Line of Business rights not assigned");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        return;
                    }
                }
                else
                {
                    intErrCode = ObjDocumentNumberControlClient.FunPubModifyDNCDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_DNCMasterDataTable, SerMode));
                }


                if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
                {
                    if (FunCheckBranchisvalid(ddlBranch.SelectedValue))
                    {

                        intErrCode = ObjDocumentNumberControlClient.FunPubModifyDNCDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_DNCMasterDataTable, SerMode));
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Selected Branch rights not assigned");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        return;
                    }
                }
                else
                {
                    intErrCode = ObjDocumentNumberControlClient.FunPubModifyDNCDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_DNCMasterDataTable, SerMode));
                }


            }
            else
            {
                intErrCode = ObjDocumentNumberControlClient.FunPubCreateDNCDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_DNCMasterDataTable, SerMode));
            }
            if (intErrCode == 0)
            {
                if (intDocSeqId > 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Document Number Control details updated successfully");
                }
                else
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strAlert = "Document Number Control details added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    CbxActive.Checked = true;
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number for selected combination already exists");
                strRedirectPageView = "";
            }

            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number for selected combination is already exists");
                strRedirectPageView = "";
            }
            else if (intErrCode == 3)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number for selected combination is already exists");
                strRedirectPageView = "";
            }

            else if (intErrCode == 4)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number for selected combination is already exists.");
                strRedirectPageView = "";
            }

            else if (intErrCode == 5)
            {
                strAlert = strAlert.Replace("__ALERT__", "Enter the To Number greater than From Number");
                strRedirectPageView = "";
            }
            else if (intErrCode == 6)
            {
                strAlert = strAlert.Replace("__ALERT__", "Enter the From Number and To Number already exists this selected year");
                strRedirectPageView = "";
            }

            else if (intErrCode == 7)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number for selected combination already exists");
                strRedirectPageView = "";
                //Deactivate the active indicator and insert for that combination of Company and Line of Business and Branch wise
            }

            else if (intErrCode == 8)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number for selected combination already exists");
                strRedirectPageView = "";
                //Deactivate the active indicator and insert for that combination of Company and Line of business wise
            }
            else if (intErrCode == 9)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number for selected combination already exists");
                strRedirectPageView = "";
                //Deactivate the active indicator and insert for that combination of company and branch wise         
            }
            else if (intErrCode == 19)
            {
                strAlert = strAlert.Replace("__ALERT__", "Financial year to be mapped");
                strRedirectPageView = "";
                //Deactivate the active indicator and insert for that combination of company and branch wise         
            }
            else if (intErrCode == 20)
            {
                strAlert = strAlert.Replace("__ALERT__", "Batch Number already exists");
                strRedirectPageView = "";
                //Deactivate the active indicator and insert for that combination of company and branch wise         
            }
            else if (intErrCode == 21)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number cannot be defined for closed Financial Year");
                strRedirectPageView = "";
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Error in saving document sequence number");
                strRedirectPageView = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjDocumentNumberControlClient.Close();
        }
    }
    #endregion

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ObjDocumentNumberControlClient = new DocMgtServicesReference.DocMgtServicesClient();
        try
        {
            string strAlert = "alert('__ALERT__');";
            string strKey = "Insert";
            string strRedirectPageView = "window.location.href='../System Admin/S3G_SysAdminDocumentNumberControl_View.aspx';";
            string strRedirectPageAdd = "window.location.href='../System Admin/S3G_SysAdminDocumentNumberControl_Add.aspx';";


            DocMgtServices.S3G_SYSAD_Get_DNCDetailsRow ObjDNCDetailsRow;
            ObjDNCDetailsRow = ObjS3G_GetDNCDataTable.NewS3G_SYSAD_Get_DNCDetailsRow();
            ObjDNCDetailsRow.Doc_Number_Seq_ID = intDocSeqId;

            intErrCode = ObjDocumentNumberControlClient.FunPubDeleteDNCDetails(intDocSeqId);
            // strAlert = strAlert.Replace("__ALERT__", "Document Number details deleted Successfully");
            if (intErrCode == 0)
            {
                if (intDocSeqId > 0)
                {
                    //strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Document Number details deleted Successfully");
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Document Number cannot be deleted,since the sequence has been used");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
        }
        catch (FaultException<DocMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            // if(ObjDocumentNumberControlClient!=null)
            ObjDocumentNumberControlClient.Close();
        }

    }
    /// <summary>
    /// Clear the all user enterable details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = 0;
            ddlBranch.SelectedValue = "0";
            ddlFinYear.SelectedIndex = 0;
            ddlDocType.SelectedIndex = 0;
            txtBatch.Text = "";
            txtFromNo.Text = "";
            txtToNo.Text = "";
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region Dropdown List
    /// <summary>
    /// selected change value using branch
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlBatch.SelectedValue) == -1)
            {
                txtBatch.Enabled = true;
            }
            else
            {
                txtBatch.Enabled = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #endregion
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        //DataTable dtCommon = new DataTable();
        //DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "DOC");
        Procparam.Add("@User_ID", obj_Page.ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Program_Id", "10");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GET_BRANCHLIST_AGT", Procparam));

        return suggetions.ToArray();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
        }
    }





}