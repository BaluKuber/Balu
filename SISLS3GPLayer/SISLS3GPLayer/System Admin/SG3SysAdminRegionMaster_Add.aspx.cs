#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Region and Branch Details creation
/// Created By			: Kannan RC
/// Created Date		: 13-May-2010
/// 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 12-July-2010   
/// Purpose             : Add Role Access setup 
/// 
/// Last Updated By		: Gunasekar.K
/// Last Updated Date   : 15-Oct-2010   
/// Purpose             : To Address the bug -1746
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
using System.Text;
#endregion

// <summary>
// Changes Done by S.Kannan
// added lblActive,lblActiveBranch with CssClass=styleDisplayLabel for error Code 679
// </summary>

public partial class System_Admin_SG3SysAdminRegionMaster_Add : ApplyThemeForProject
{

    #region Paging Config - For Region

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    int inthdUserid;
    string strRedirectPageMC;
    PagingValues ObjPaging = new PagingValues();

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
        FunGetRegionDetatils();
    }
    #endregion

    #region Intialization
    CompanyMgtServicesReference.CompanyMgtServicesClient objRegionMasterClient;
    CompanyMgtServicesReference.CompanyMgtServicesClient objBranchMasterClient;

    static bool _IsEdited = false; // added by S.Kannan - to check if the end-user edited the record or not.
    int intErrCode = 0;
    int intRegionId = 0;
    int intBranchId = 0;
    string strMode;
    string _ViewMode = "";
    string qsMode;
    string qsModes;
    string qsTab;
    static string strPageName = "Region and Branch Master";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../System Admin/SG3SysAdminRegionMaster_View.aspx'";
    CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable ObjS3G_BranchList = new CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable();
    CompanyMgtServices.S3G_SYSAD_RegionMaster_CUDataTable ObjS3G_RegionMaster_CUDataTable = new CompanyMgtServices.S3G_SYSAD_RegionMaster_CUDataTable();
    CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable ObjS3G_RegionMaster_View = new CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable();
    CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable ObjS3G_RegionList = new CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable();
    CompanyMgtServices.S3G_SYSAD_BranchMaster_CUDataTable ObjS3G_BranchMaster_CUDataTable = new CompanyMgtServices.S3G_SYSAD_BranchMaster_CUDataTable();
    CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable ObjS3G_BranchMaster_View = new CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable();
    CompanyMgtServices.S3G_SYSAD_Region_NameDataTable ObjS3G_RegionName = new CompanyMgtServices.S3G_SYSAD_Region_NameDataTable();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["qsRegion_ID"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsRegion_ID"));
            strMode = Request.QueryString["qsMode"];
            if (fromTicket != null)
            {
                intRegionId = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

        }

        if (Request.QueryString["qsBranch_ID"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsBranch_ID"));
            strMode = Request.QueryString["qsMode"];
            if (fromTicket != null)
            {
                intBranchId = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }


        txtRegionName.Attributes.Add("onblur", "Trim('" + txtRegionName.ClientID + "');");
        //txtRegionName.Attributes.Add("onblur", "RemoveStartingNumeric('" + txtRegionName.ClientID + "');");
        txtBranchCode.Attributes.Add("onblur", "Trim('" + txtBranchCode.ClientID + "');");
        txtBranchName.Attributes.Add("onblur", "Trim('" + txtBranchName.ClientID + "');");
        //txtBranchName.Attributes.Add("onblur", "RemoveStartingNumeric('" + txtBranchName.ClientID + "');");

        TextBox textBox = txtStateName.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeypress", "maxlengthfortxt(60);fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this);");
            textBox.MaxLength = 60;
        }

        //txtStateName.Attributes.Add("onblur", "funCheckFirstLetterisNumeric('" + txtStateName.ClientID + "');");
        //txtStateName.Attributes.Add("onblur", "RemoveStartingNumeric('" + txtStateName.ClientID + "');");
        ddlRegionCode.Attributes.Add("style", "width:205px");

        if (Request.QueryString["qsMode"] != null)
            qsModes = Convert.ToString(Request.QueryString["qsModes"]);

        if (Request.QueryString["qsTab"] != null)
            qsTab = Convert.ToString(Request.QueryString["qsTab"]);
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        if (!IsPostBack)
        {
            int intMode;
            FunGetRegionCodeList();

            /// <summary>
            /// This method used for Role Access 
            /// </summary>
            if (qsTab == "region")
            {
                if (((intRegionId > 0)) && (strMode == "M"))
                {
                    //FunGetRegionDetatils();
                    FunPriRegionTabControlStatus(1);
                }
                else if (((intRegionId > 0)) && (strMode == "Q"))
                {
                    FunPriRegionTabControlStatus(-1);
                }
                else
                {
                    FunPriRegionTabControlStatus(0);

                }

            }
            else
            {
                /// <summary>
                /// This method used for Role Access
                /// </summary>
                if (((intBranchId > 0)) && (strMode == "M"))
                {
                    //FunGetBranchDetatils();
                    FunPriBranchTabControlStatus(1);

                }
                else if (((intBranchId > 0)) && (strMode == "Q"))
                {
                    FunPriBranchTabControlStatus(-1);
                }
                else
                {
                    FunPriBranchTabControlStatus(0);

                }


            }


        }

    }

    #region Region
    /// <summary>
    /// Create and modify region details 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRegionSave_Click(object sender, EventArgs e)
    {
        objRegionMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {


            // if (_IsEdited) // added by S.Kannan - to check if the end-user edited the record or not.
            //  { // added by S.Kannan - to check if the end-user edited the record or not.
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../System Admin/SG3SysAdminRegionMaster_View.aspx?qsTab=reg';";
            string strRedirectPageAdd = "window.location.href='../System Admin/SG3SysAdminRegionMaster_Add.aspx?qsTab=region';";
            //Modified By Nataraj
            if (txtRegionCode.Text.CheckZeroValidate())
            {
                if (txtRegionName.Text.CheckZeroValidate())
                {
                    CompanyMgtServices.S3G_SYSAD_RegionMaster_CURow ObjRegionMasterRow;
                    ObjRegionMasterRow = ObjS3G_RegionMaster_CUDataTable.NewS3G_SYSAD_RegionMaster_CURow();
                    ObjRegionMasterRow.Region_ID = intRegionId;
                    ObjRegionMasterRow.Company_ID = intCompanyID;
                    ObjRegionMasterRow.Region_Code = txtRegionCode.Text;
                    ObjRegionMasterRow.Region_Name = txtRegionName.Text;
                    ObjRegionMasterRow.Is_Active = CbxRegion.Checked;
                    ObjRegionMasterRow.Created_By = intUserID;
                    ObjRegionMasterRow.Created_On = DateTime.Now;
                    ObjRegionMasterRow.Modified_By = intUserID;
                    ObjRegionMasterRow.Modified_On = DateTime.Now;
                    ObjS3G_RegionMaster_CUDataTable.AddS3G_SYSAD_RegionMaster_CURow(ObjRegionMasterRow);
                    SerializationMode SerMode = SerializationMode.Binary;

                    if (intRegionId > 0)
                    {
                        intErrCode = objRegionMasterClient.FunPubModifyRegion(SerMode, ClsPubSerialize.Serialize(ObjS3G_RegionMaster_CUDataTable, SerMode));
                    }
                    else
                    {
                        intErrCode = objRegionMasterClient.FunPubCreateRegion(SerMode, ClsPubSerialize.Serialize(ObjS3G_RegionMaster_CUDataTable, SerMode));
                    }
                    if (intErrCode == 0)
                    {
                        if (intRegionId > 0)
                        {
                            strKey = "Modify";
                            strAlert = strAlert.Replace("__ALERT__", "Region details updated successfully");
                        }
                        else
                        {
                            strAlert = "Region details added successfully";
                            strAlert += @"\n\nWould you like to add one more record?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            CbxRegion.Checked = true;
                        }
                    }
                    else if (intErrCode == 1)
                    {
                        if (intRegionId > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Branch mapped with this region,Cannot Inactivate");
                            CbxRegion.Checked = true;
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Region code already exists in the system");
                        }
                        strRedirectPageView = "";
                    }
                    else if (intErrCode == 2)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Region Name already exists in the system");
                        strRedirectPageView = "";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                    // }
                    // else // added by S.Kannan - to check if the end-user edited the record or not.
                    // {
                    //Utility.FunShowAlertMsg(this, "No records edited"); // added by S.Kannan - to check if the end-user edited the record or not.
                    //  }
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Region Name cannot be Zero");
                    return;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Region Code cannot be Zero");
                return;
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
        finally
        {
            objRegionMasterClient.Close();
        }

    }
    /// <summary>
    /// Get region details using modify
    /// </summary>
    private void FunGetRegionDetatils()
    {
        objRegionMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewRow ObjRegionMasterRow;
            ObjRegionMasterRow = ObjS3G_RegionMaster_View.NewS3G_SYSAD_RegionMaster_ViewRow();
            ObjRegionMasterRow.Region_ID = intRegionId;
            ObjRegionMasterRow.Company_ID = intCompanyID;

            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;


            ObjS3G_RegionMaster_View.AddS3G_SYSAD_RegionMaster_ViewRow(ObjRegionMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteRegionDetails = objRegionMasterClient.FunPubQueryRegion_View(SerMode, ClsPubSerialize.Serialize(ObjS3G_RegionMaster_View, SerMode));
            //byte[] byteRegionDetails = objRegionMasterClient.FunPubQueryRegion(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_RegionMaster_View, SerMode), ObjPaging);
            //FunPubQueryRegion( SerMode, ClsPubSerialize.Serialize(ObjS3G_RegionMaster_View, SerMode));
            ObjS3G_RegionMaster_View = (CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteRegionDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable));
            txtRegionCode.Text = ObjS3G_RegionMaster_View.Rows[0]["Region_Code"].ToString();
            txtRegionName.Text = ObjS3G_RegionMaster_View.Rows[0]["Region_Name"].ToString();
            hdnID.Value = ObjS3G_RegionMaster_View.Rows[0]["Created_By"].ToString();
            if (ObjS3G_RegionMaster_View.Rows[0]["Is_Active"].ToString() == "True")
                CbxRegion.Checked = true;
            else
                CbxRegion.Checked = false;


        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {

            throw new ApplicationException("Unable to Load Region Details");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Region Details");
        }
        finally
        {
            objRegionMasterClient.Close();
        }


    }
    /// <summary>
    /// Get Branch details
    /// </summary>
    private void FunGetBranchDetatils()
    {
        objBranchMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewRow ObjBranchMasterRow;
            ObjBranchMasterRow = ObjS3G_BranchMaster_View.NewS3G_SYSAD_BranchDetails_ViewRow();
            ObjBranchMasterRow.Branch_ID = intBranchId;
            ObjBranchMasterRow.Company_ID = intCompanyID;


            ObjS3G_BranchMaster_View.AddS3G_SYSAD_BranchDetails_ViewRow(ObjBranchMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteBranchDetails = objBranchMasterClient.FunPubQueryBranch_List(SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchMaster_View, SerMode));
            //byte[] byteBranchDetails = objBranchMasterClient.FunPubQueryBranch(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchMaster_View, SerMode), ObjPaging);

            ObjS3G_BranchMaster_View = (CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable)ClsPubSerialize.DeSerialize(byteBranchDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable));
            ddlBranch.SelectedValue = ObjS3G_BranchMaster_View.Rows[0]["Branch_ID"].ToString();
            //txtBranchCode.Text = ObjS3G_BranchMaster_View.Rows[0]["Branch_Code"].ToString();
            //txtBranchName.Text = ObjS3G_BranchMaster_View.Rows[0]["Branch_Name"].ToString();
            //txtStateName.Text = ObjS3G_BranchMaster_View.Rows[0]["State_Name"].ToString();
            txtStateName.SelectedItem.Text = ObjS3G_BranchMaster_View.Rows[0]["State_Name"].ToString();
            ddlBranchCode.SelectedValue = ObjS3G_BranchMaster_View.Rows[0]["Region_ID"].ToString();
            hdnID.Value = ObjS3G_BranchMaster_View.Rows[0]["Created_By"].ToString();
            if (ObjS3G_BranchMaster_View.Rows[0]["Is_Active"].ToString() == "True")
                CbxBranch.Checked = true;
            else
                CbxBranch.Checked = false;

            if (!CbxBranch.Checked)
            {
                ddlToRegion.SelectedIndex = 0;
                ddlToRegion.Enabled = false;
            }


        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {

            throw new ApplicationException("Unable to Load Region Details");
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Region Details");
        }
        finally
        {
            objBranchMasterClient.Close();

        }


    }


    #endregion

    #region Branch
    /// <summary>
    /// CREATE AND MODIFY Branch details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBranchSave_Click(object sender, EventArgs e)
    {
        objBranchMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

        try
        {
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            string strRedirectPageView = "window.location.href='../System Admin/SG3SysAdminRegionMaster_View.aspx?qsTab=ban';";
            string strRedirectPageAdd = "window.location.href='../System Admin/SG3SysAdminRegionMaster_Add.aspx';";
            CompanyMgtServices.S3G_SYSAD_BranchMaster_CURow ObjBranchMasterRow;
            ObjBranchMasterRow = ObjS3G_BranchMaster_CUDataTable.NewS3G_SYSAD_BranchMaster_CURow();
            ObjBranchMasterRow.Branch_ID = intBranchId;
            if (intBranchId == 0)
            {
                if (!txtBranchCode.Text.CheckZeroValidate())
                {
                    Utility.FunShowAlertMsg(this, "Branch Code should not be Zero.");
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
                if (!txtBranchName.Text.CheckZeroValidate())
                {
                    Utility.FunShowAlertMsg(this, "Branch Name should not be Zero.");
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
                if (!txtStateName.Text.CheckZeroValidate())
                {
                    Utility.FunShowAlertMsg(this, "State Name should not be Zero.");
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
                //Commented by Nataraj Y
                //if (txtBranchCode.Text.Length > 0)
                //{
                //    char charFirst = Convert.ToChar(txtBranchCode.Text.Substring(0, 1).ToLower());
                //    if (char.IsDigit(charFirst) && Convert.ToInt32(charFirst) == 48)
                //    {
                //        //--Added by guna on 15th-oct-2010 to address the bug -1746
                //        //strAlert = strAlert.Replace("__ALERT__", "Branch Code should not be starts with 0.");
                //        strAlert = strAlert.Replace("__ALERT__", "Branch Code should not start with 0.");
                //        //--Ends Here
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                //        lblErrorMessage.Text = string.Empty;
                //        return;
                //    }
                //}

                //if (txtBranchName.Text.Length > 0)
                //{
                //    char charFirst = Convert.ToChar(txtBranchName.Text.Substring(0, 1).ToLower());
                //    if (char.IsDigit(charFirst))
                //    {
                //        //--Added by guna on 15th-oct-2010 to address the bug -1746
                //        //strAlert = strAlert.Replace("__ALERT__", "Branch Name should not be starts with numeric.");
                //        strAlert = strAlert.Replace("__ALERT__", "Branch Name should not start with Number.");
                //        //--Ends Here
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                //        lblErrorMessage.Text = string.Empty;
                //        return;
                //    }
                //}
                //if (txtStateName.Text.Length > 0)
                //{
                //    char charFirst = Convert.ToChar(txtStateName.Text.Substring(0, 1).ToLower());
                //    if (char.IsDigit(charFirst))
                //    {
                //        //--Added by guna on 15th-oct-2010 to address the bug -1746
                //        //strAlert = strAlert.Replace("__ALERT__", "State Name should not be starts with numeric.");
                //        strAlert = strAlert.Replace("__ALERT__", "State Name should not start with Number.");
                //        //--Ends Here
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                //        lblErrorMessage.Text = string.Empty;
                //        return;
                //    }
                //}
            }
            if (intBranchId == 0)
            {
                //ObjBranchMasterRow.Region_ID = Convert.ToInt32(ddlBranchCode.SelectedValue);
                //ObjBranchMasterRow.Prev_Region_ID = Convert.ToInt32(ddlBranchCode.SelectedValue);
                ObjBranchMasterRow.Region_ID = Convert.ToInt32(ddlRegionCode.SelectedValue);
                ObjBranchMasterRow.Prev_Region_ID = Convert.ToInt32(ddlRegionCode.SelectedValue);

            }
            else if (intBranchId > 0)
            {
                if (Convert.ToInt32(ddlToRegion.SelectedValue) == 0)
                {
                    ObjBranchMasterRow.Region_ID = Convert.ToInt32(ddlBranchCode.SelectedValue);
                    ObjBranchMasterRow.Prev_Region_ID = Convert.ToInt32(ddlBranchCode.SelectedValue);
                }
                else if (Convert.ToInt32(ddlToRegion.SelectedValue) > 0)
                {
                    ObjBranchMasterRow.Prev_Region_ID = Convert.ToInt32(ddlBranchCode.SelectedValue);
                    ObjBranchMasterRow.Region_ID = Convert.ToInt32(ddlToRegion.SelectedValue);
                }
                //else
                //{
                //    ObjBranchMasterRow.Region_ID = Convert.ToInt32(ddlRegionCode.SelectedValue);
                //    ObjBranchMasterRow.Prev_Region_ID = Convert.ToInt32(ddlRegionCode.SelectedValue);

                // ObjBranchMasterRow.Prev_Region_ID = Convert.ToInt32(ddlBranchCode.SelectedValue);
                //}
            }

            ObjBranchMasterRow.Branch_Code = txtBranchCode.Text;
            ObjBranchMasterRow.Branch_Name = txtBranchName.Text;
            ObjBranchMasterRow.State_Name = txtStateName.SelectedItem.Text.Trim();
            ObjBranchMasterRow.Is_Active = CbxBranch.Checked;
            ObjBranchMasterRow.Created_By = intUserID;
            ObjBranchMasterRow.Created_On = DateTime.Now;
            ObjBranchMasterRow.Modified_By = intUserID;
            ObjBranchMasterRow.Modified_On = DateTime.Now;
            //if (ObjBranchMasterRow.Branch_ID == 0)
            //{
            //    //ObjBranchMasterRow.Prev_Region_ID = 0;
            //}
            //else
            //{

            //    //ObjBranchMasterRow.Prev_Region_ID = Convert.ToInt32(ddlBranchCode.SelectedValue);
            //}

            ObjS3G_BranchMaster_CUDataTable.AddS3G_SYSAD_BranchMaster_CURow(ObjBranchMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;

            if (intBranchId > 0)
            {
                intErrCode = objBranchMasterClient.FunPubModifyBranch(SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchMaster_CUDataTable, SerMode));
            }
            else
            {
                intErrCode = objBranchMasterClient.FunPubCreateBranch(SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchMaster_CUDataTable, SerMode));
            }
            if (intErrCode == 0)
            {
                if (intBranchId > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Branch details updated successfully");
                }
                else
                {
                    strAlert = "Branch details added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    CbxBranch.Checked = true;
                }
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "Branch code already exists in the system");
                strRedirectPageView = "";
            }
            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Branch Name already exists in the system");
                strRedirectPageView = "";
            }
            else if (intErrCode == 3)
            {
                strAlert = strAlert.Replace("__ALERT__", "From Region and To Region are same.Kindly Select other region");
                strRedirectPageView = "";
                btnBranchSaveSummary.Visible = false;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);

            lblErrorMessage.Text = string.Empty;


        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objBranchMasterClient.Close();
        }

    }
    /// <summary>
    /// Get branch dropdownlist
    /// </summary>
    private void FunPriGetBranchList()
    {
        objBranchMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            CompanyMgtServices.S3G_SYSAD_Branch_ListRow ObjBranchListRow;
            ObjBranchListRow = ObjS3G_BranchList.NewS3G_SYSAD_Branch_ListRow();
            //ObjBranchListRow.Region_Id = 1;
            ObjBranchListRow.Company_ID = intCompanyID;
            ObjS3G_BranchList.AddS3G_SYSAD_Branch_ListRow(ObjBranchListRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] bytesBranchListDetails = objBranchMasterClient.FunPubBranch_List(SerMode, ClsPubSerialize.Serialize(ObjS3G_BranchList, SerMode));
            ObjS3G_BranchList = (CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable)ClsPubSerialize.DeSerialize(bytesBranchListDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable));
            ddlBranch.FillDataTable(ObjS3G_BranchList, ObjS3G_BranchList.Branch_IDColumn.ColumnName.Trim(), ObjS3G_BranchList.BranchColumn.ColumnName.Trim());
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load branch");
        }
        finally
        {
            objBranchMasterClient.Close();
        }


    }

    /// <summary>
    /// Bind region code  dropdownlist
    /// </summary>
    private void FunGetRegionCodeList()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@User_ID", intUserID.ToString());
            if (intBranchId == 0)
            {
                dictParam.Add("@Is_Active", "1");
            }
            ddlRegionCode.BindDataTable("S3G_Get_Region_Code", dictParam, new string[] { "Region_Id", "Region" });
            ddlBranchCode.BindDataTable("S3G_Get_Region_Code", dictParam, new string[] { "Region_Id", "Region" });

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Category", "2");
            txtStateName.BindDataTable("S3G_SYSAD_GetAddressLoodup", Procparam, new string[] { "ID", "Name" });
        }
        catch (Exception ex)
        {

            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);

        }



    }
    /// <summary>
    /// bind to region dropdownlist
    /// </summary>
    private void FunGetToRegionList()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@Is_Active", "1");
            dictParam.Add("@User_ID", intUserID.ToString());
            DataTable dtToRegion = new DataTable();
            dtToRegion = Utility.GetDefaultData("S3G_Get_Region_Code", dictParam);

            //if (ddlBranchCode.SelectedIndex > 0)
            //{
            dtToRegion.DefaultView.RowFilter = "Region <> '" + ddlBranchCode.SelectedItem.ToString() + "'"; // Added By S.Kannan
            //}


            ddlToRegion.FillDataTable(dtToRegion.DefaultView.ToTable(), "Region_Id", "Region"); // Added By S.Kannan
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Region");

        }

    }

    #endregion

    protected void btnRegionCancel_Click(object sender, EventArgs e)
    {
        txtRegionCode.Text = "";
        txtRegionName.Text = "";
        //--Added by Guna on 15th-Oct-2010 to address the bug -1744
        //Response.Redirect("../System Admin/SG3SysAdminRegionMaster_View.aspx?qsTab=reg");
        Response.Redirect("~/System Admin/SG3SysAdminRegionMaster_View.aspx?qsTab=reg");
        //--Ends here


    }
    protected void btnBranchCancel_Click(object sender, EventArgs e)
    {
        try
        {
            txtBranchCode.Text = "";
            txtBranchName.Text = "";
            //txtStateName.Text = "";
            CbxBranch.Checked = false;
            //--Added by Guna on 15th-Oct-2010 to address the bug -1744
            //Response.Redirect("../System Admin/SG3SysAdminRegionMaster_View.aspx?qsTab=ban");
            Response.Redirect("~/System Admin/SG3SysAdminRegionMaster_View.aspx?qsTab=ban");
            //--Ends Here
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        objBranchMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            CompanyMgtServices.S3G_SYSAD_Region_NameRow ObjRegionNameRow;
            ObjRegionNameRow = ObjS3G_RegionName.NewS3G_SYSAD_Region_NameRow();
            ObjRegionNameRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjS3G_RegionName.AddS3G_SYSAD_Region_NameRow(ObjRegionNameRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] bytesRegionNameListDetails = objBranchMasterClient.FunPubRegionName(SerMode, ClsPubSerialize.Serialize(ObjS3G_RegionName, SerMode));
            ObjS3G_RegionName = (CompanyMgtServices.S3G_SYSAD_Region_NameDataTable)ClsPubSerialize.DeSerialize(bytesRegionNameListDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_Region_NameDataTable));
            //ddlBranchCode.FillDataTable(ObjS3G_RegionName, ObjS3G_RegionName.Region_IdColumn.ColumnName.Trim(), ObjS3G_RegionList.RegionColumn.ColumnName.Trim());
            ddlBranchCode.DataValueField = ObjS3G_RegionName.Region_IDColumn.ColumnName.Trim();
            ddlBranchCode.DataTextField = ObjS3G_RegionList.RegionColumn.ColumnName.Trim();
            ddlBranchCode.DataSource = ObjS3G_RegionName;
            ddlBranchCode.DataBind();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objBranchMasterClient.Close();
        }

    }
    protected void btnRegionClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtRegionCode.Text = "";
            txtRegionName.Text = "";
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);

        }
    }
    protected void btnBranchClear_Click(object sender, EventArgs e)
    {
        try
        {
            //   ddlBranch.SelectedIndex = 0;
            txtBranchCode.Text = "";
            txtBranchName.Text = "";
            if (txtStateName.Items.Count > 1)
            {
                txtStateName.SelectedIndex = 0;
            }
            //txtStateName.Text = "";
            ddlRegionCode.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// This method used Role Accesss
    /// </summary>
    /// <param name="intModeID"></param>
    /// <param name="qsTab"></param>
    private void FunPriDisableControls(int intModeID, string qsTab)
    {
        try
        {
            if (qsTab == "region")
            {
                FunPriRegionTabControlStatus(intModeID);
            }
            else
            {
                FunPriBranchTabControlStatus(intModeID);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);

        }

    }

    private void FunPriRegionTabControlStatus(int intModeID)
    {
        try
        {
            if (intModeID != 0)
            {
                FunGetRegionDetatils();
            }

            if (intModeID == 0)
            {
                if (!bCreate)
                {
                    btnRegionSave.Enabled = false;
                }
                CbxRegion.Checked = true;
                CbxRegion.Enabled = false;
                tcRegBranch.ActiveTabIndex = 0;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                // CbxRegion.Checked = true;
                txtRegionCode.Focus();
                TabPanel2.Enabled = false;
                txtRegionCode.Focus();

            }
            else if (intModeID == 1)
            {
                if (!bModify)
                {
                    btnRegionSave.Enabled = false;
                }
                tcRegBranch.ActiveTabIndex = 0;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnRegionClear.Enabled = false;
                rfvBranchCode.Visible = false;
                CbxRegion.Enabled = true;
                TabPanel2.Enabled = false;
                txtRegionCode.ReadOnly = true;
                txtRegionCode.Focus();
            }
            else if (intModeID == -1)
            {
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                btnRegionSave.Enabled = false;
                btnRegionClear.Enabled = false;
                txtRegionCode.ReadOnly = true;
                txtRegionCode.Enabled = false;
                txtRegionName.ReadOnly = true;
                CbxRegion.Enabled = false;
                TabPanel2.Enabled = false;
                tcRegBranch.ActiveTabIndex = 0;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }


    private void FunPriBranchTabControlStatus(int intModeID)
    {
        try
        {
            if (intModeID != 0)
            {
                FunGetBranchDetatils();
            }
            if (intModeID == 0)
            {
                if (!bCreate)
                {
                    btnBranchSave.Enabled = false;
                }
                txtStateName.Enabled = true;
                CbxBranch.Checked = true;   // ADDED BY S.KANNAN - by default it should be active
                CbxBranch.Enabled = false;  // ADDED BY S.KANNAN - by default it cannot be changed to inactive in create mode                   
                tcRegBranch.ActiveTabIndex = 1;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                FunGetRegionCodeList();
                lblBranchC.Text = "Branch Name";
                ddlBranchCode.Visible = false;
                txtBranchName.Visible = true;
                ddlBranch.Visible = false;
                rfvBranch.Visible = false;
                ddlRegionCode.Enabled = true;
                txtStateName.Visible = true;
                //rfvToRegion.Visible = false;
                rfvRegionCode.Visible = false;
                rfvRegionName.Visible = false;
                lblRegionCode.Text = "Region";
                //lblRegionName.Text = "State Name";                    
                Tr7.Visible = true;
                ddlToRegion.Visible = false;
                txtRegionName.Visible = true;
                CbxBranch.Checked = true;
                txtBranchCode.Focus();
                TabPanel1.Enabled = false;
                txtBranchCode.Focus();
                rfvBranchRegionCode.Visible = true;

            }
            else if (intModeID == 1)
            {
                if (!bModify)
                {
                    btnBranchSave.Enabled = false;
                }

                tcRegBranch.ActiveTabIndex = 1;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                FunGetToRegionList();
                FunPriGetBranchList();
                FunGetBranchDetatils();
                CbxBranch.Enabled = true;       // ADDED BY S.KANNAN - It should be enabled in modify mode to make it active/inactive
                ddlBranchCode.Visible = true;
                ddlRegionCode.Visible = false;
                ddlBranch.Visible = true;
                trBranchCode.Visible = false;
                rfvBranch.Visible = true;
                lblBranchC.Text = "Branch";
                txtBranchName.Visible = false;
                //rfvToRegion.Visible = true;
                lblStateName.Visible = false;
                //rfvStateName.Visible = false;
                rfvStateName.Enabled = false;
                rfvRegionCode.Visible = false;
                rfvRegionName.Visible = false;
                txtBranchCode.Enabled = false;
                txtBranchName.Enabled = false;
                txtStateName.Visible = false;
                //txtStateName.Enabled = true;
                //txtStateName.Visible = true;
                ddlToRegion.Visible = true;
                lblRegionName.Text = "To Region";
                lblRegionCode.Text = "From Region";
                ddlRegionCode.Enabled = false;
                ddlBranchCode.Enabled = false;
                btnBranchClear.Enabled = false;
                TabPanel1.Enabled = false;
                ddlBranch.Enabled = false;
                rfvBranchName.Visible = false;
                rfvBranchRegionCode.Visible = false;
            }
            else if (intModeID == -1)
            {
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                FunPriGetBranchList();
                FunGetToRegionList();
                ddlBranchCode.Visible = true;
                ddlRegionCode.Visible = false;
                ddlBranch.Visible = true;
                ddlBranch.Enabled = true;
                ddlToRegion.Visible = false;
                lblRegionName.Visible = false;

                trBranchCode.Visible = false;
                lblBranchC.Text = "Branch";
                txtBranchName.Visible = false;
                lblStateName.Visible = false;
                txtBranchCode.Enabled = false;
                txtBranchName.Enabled = false;
                txtStateName.Visible = false;
                //lblRegionName.Text = "To Region";
                lblRegionCode.Text = "From Region";
                ddlBranchCode.Enabled = true;
                btnBranchClear.Enabled = false;
                btnBranchSave.Enabled = false;
                rfvBranchName.Visible = false;
                rfvBranchRegionCode.Visible = false;
                TabPanel1.Enabled = false;
                ListItem lit;
                //lit = new ListItem(ddlToRegion.SelectedItem.Text, ddlToRegion.SelectedItem.Value);
                //ddlToRegion.Items.Clear();
                //ddlToRegion.Items.Add(lit);

                lit = new ListItem(ddlBranchCode.SelectedItem.Text, ddlBranchCode.SelectedItem.Value);
                ddlBranchCode.Items.Clear();
                ddlBranchCode.Items.Add(lit);

                lit = new ListItem(ddlBranch.SelectedItem.Text, ddlBranch.SelectedItem.Value);
                ddlBranch.Items.Clear();
                ddlBranch.Items.Add(lit);

                CbxBranch.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }









    // this method was added by S.Kannan - to check if the end-user edited the record or not.
    // Changed its Autopostback property to true
    protected void txtRegionCode_TextChanged(object sender, EventArgs e)
    {
        //string qsmode =  Request.QueryString["qsMode"].ToString(); 
        //if (!((!(string.IsNullOrEmpty (qsmode))) &&
        //    ((string.Compare (qsmode ,"region")) == 0) &&
        //    (Request.QueryString["rid"] == null)))
        //       // Modify mode

        //if (txtRegionCode.Text.Trim().Length < txtRegionCode.Text.Length) // Added By S.Kannan
        //{
        //    Utility.FunShowAlertMsg(this, "Space trimmed in Region Code");
        //    txtRegionCode.Text = txtRegionCode.Text.Trim();
        //}

        //_IsEdited = true;
    }

    // this method was added by S.Kannan - to check if the end-user edited the record or not.
    // Changed its Autopostback property to true
    protected void txtRegionName_TextChanged(object sender, EventArgs e)
    {
        //string qsmode = Request.QueryString["qsMode"].ToString();
        //if (!((!(string.IsNullOrEmpty(qsmode))) &&
        //   ((string.Compare(qsmode, "region")) == 0) &&
        //   (Request.QueryString["rid"] == null)))
        //if (txtRegionName.Text.Trim().Length < txtRegionName.Text.Length) // Added By S.Kannan
        //{
        //    Utility.FunShowAlertMsg(this, "Space trimmed in Region Name");
        //    txtRegionName.Text = txtRegionName.Text.Trim();
        //}

        //_IsEdited = true;
    }

    // this method was added by S.Kannan - to check if the end-user edited the record or not.    
    protected void CbxRegion_CheckedChanged(object sender, EventArgs e)
    {
        //   _IsEdited = true;
    }

    // this method was added by S.Kannan - to check if the end-user edited the record or not.
    // Changed its Autopostback property to true
    protected void txtStateName_TextChanged(object sender, EventArgs e)
    {
        //if (txtStateName.Text.Trim().Length < txtStateName.Text.Length) // Added By S.Kannan
        //{
        //    Utility.FunShowAlertMsg(this, "Space trimmed in State Name");
        //    txtStateName.Text = txtStateName.Text.Trim();
        //}
        //_IsEdited = true;
    }
    // this method was added by S.Kannan - to check if the end-user edited the record or not.
    // Changed its Autopostback property to true
    protected void txtBranchCode_TextChanged(object sender, EventArgs e)
    {
        //if (txtBranchCode.Text.Trim().Length < txtBranchCode.Text.Length) // Added By S.Kannan
        //{
        //    Utility.FunShowAlertMsg(this, "Space trimmed in Branch Code");
        //    txtBranchCode.Text = txtBranchCode.Text.Trim();
        //}
        //_IsEdited = true;
    }
    // this method was added by S.Kannan - to check if the end-user edited the record or not.
    // Changed its Autopostback property to true
    protected void txtBranchName_TextChanged(object sender, EventArgs e)
    {
        //    if (txtBranchName.Text.Trim().Length < txtBranchName.Text.Length) // Added By S.Kannan
        //    {
        //        Utility.FunShowAlertMsg(this, "Space trimmed in Branch Name");
        //        txtBranchName.Text = txtBranchName.Text.Trim();
        //    }
        //    _IsEdited = true;
    }
    protected void CbxBranch_CheckedChanged(object sender, EventArgs e)
    {
        if (!CbxBranch.Checked)
        {
            ddlToRegion.SelectedIndex = 0;
            ddlToRegion.Enabled = false;
        }
        else
        {
            ddlToRegion.Enabled = true;
        }
    }
}
