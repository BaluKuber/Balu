#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   System Admin
/// Screen Name         :   S3GSysAdminLOBMaster_Add
/// Created By          :   Suresh P
/// Created Date        :   11-MAY-2010
/// Purpose             :   To Create or Modify LOB Master details
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;


#endregion

public partial class System_Admin_S3GSysAdminLOBMaster_Add : ApplyThemeForProject
{
    #region Initialization
    int intLOBID = 0;
    int intErrCode = 0;
    int intCompanyID;
    int intUserID;
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;    
    int inthdUserid;
    string strRedirectPageMC;
    UserInfo ObjUserInfo = null;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminLOBMaster_View.aspx'";
    CompanyMgtServicesReference.CompanyMgtServicesClient ObjLOBMasterClient;
    CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable ObjS3G_LOBMaster_CUDataTable = new CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable();
    #endregion
    
    /// <summary>
    /// Page Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["qsLobID"] != null)
        {

            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsLobID"));
            strMode = Request.QueryString.Get("qsMode");
            if (fromTicket != null)
            {
                intLOBID = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
         ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        txtLOBName.Attributes.Add("onblur", "Trim('" + txtLOBName.ClientID + "');");

        if (!IsPostBack)
        {
            chkActive.Checked = false;
            #region MakerChecker            
            if (((intLOBID > 0)) && (strMode == "M"))
            {
                              
                    FunPriDisableControls(1);               

            }
            else if (((intLOBID > 0)) && (strMode == "Q"))
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriDisableControls(0);
            }
            #endregion MakerChecker
        }
    }
    
    /// <summary>
    /// Get LineofBusiness Details
    /// </summary>
    private void FunPriGetLOBDetails()
    {
        try
        {
            //ObjUserInfo = new UserInfo();
            CompanyMgtServices.S3G_SYSAD_LOBMaster_CURow ObjLOBMasterRow;

            ObjLOBMasterRow = ObjS3G_LOBMaster_CUDataTable.NewS3G_SYSAD_LOBMaster_CURow();
            ObjLOBMasterRow.Company_ID = intCompanyID;
            //ObjUserInfo.ProCompanyIdRW;
            ObjLOBMasterRow.LOB_ID = intLOBID;
            ObjS3G_LOBMaster_CUDataTable.AddS3G_SYSAD_LOBMaster_CURow(ObjLOBMasterRow);

            ObjLOBMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteLOBDetails = ObjLOBMasterClient.FunPubQueryLOB(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMaster_CUDataTable, SerMode));

            CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable dtLOB = (CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable)ClsPubSerialize.DeSerialize(byteLOBDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable));
            CompanyMgtServices.S3G_SYSAD_LOBMaster_CURow drLOB = dtLOB.Rows[0] as CompanyMgtServices.S3G_SYSAD_LOBMaster_CURow;

            txtLOBCode.Text = drLOB.LOB_Code.ToString();
            txtLOBName.Text = drLOB.LOB_Name.ToString();
            hdnID.Value = drLOB.Created_By.ToString();
            chkActive.Checked = drLOB.Is_Active;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        ObjLOBMasterClient.Close();
    }

    /// <summary>
    /// Save the Record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //string s = "create";
        //s += @"\nWould you like to add one more record?";
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "if(confirm('" + s + "')){window.location.href='../System Admin/S3GSysAdminLOBMaster_Add.aspx';}else {window.location.href='../System Admin/S3GSysAdminLOBMaster_View.aspx';}", true);
        //return;

        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminLOBMaster_View.aspx';";
        string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminLOBMaster_Add.aspx';";


        if (txtLOBName.Text.Substring(0, 1).Equals(" "))
        {
            txtLOBName.Text = txtLOBName.Text.Trim();
        }

        try
        {
            // ObjUserInfo = new UserInfo();

            CompanyMgtServices.S3G_SYSAD_LOBMaster_CURow ObjLOBMasterRow;
            ObjLOBMasterRow = ObjS3G_LOBMaster_CUDataTable.NewS3G_SYSAD_LOBMaster_CURow();
            //intLOBID = Convert.ToInt32(Request.QueryString["qsLobID"]);
            ObjLOBMasterRow.LOB_ID = intLOBID;
            ObjLOBMasterRow.Company_ID = intCompanyID;
            //ObjUserInfo.ProCompanyIdRW;
            ObjLOBMasterRow.LOB_Code = txtLOBCode.Text;
            ObjLOBMasterRow.LOB_Name = txtLOBName.Text;
            ObjLOBMasterRow.Is_Active = chkActive.Checked;
            ObjLOBMasterRow.Created_By = intUserID;
            //ObjUserInfo.ProUserIdRW;
            ObjLOBMasterRow.Created_On = DateTime.Now;
            ObjLOBMasterRow.Modified_By = intUserID;
            //ObjUserInfo.ProUserIdRW;
            ObjLOBMasterRow.Modified_On = DateTime.Now;
            ObjS3G_LOBMaster_CUDataTable.AddS3G_SYSAD_LOBMaster_CURow(ObjLOBMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            ObjLOBMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            if (intLOBID > 0)
            {
                intErrCode = ObjLOBMasterClient.FunPubModifyLOB(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMaster_CUDataTable, SerMode));
            }
            else
            {
                intErrCode = ObjLOBMasterClient.FunPubCreateLOB(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMaster_CUDataTable, SerMode));
            }


            if (intErrCode == 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert(' The Entered Line of Business is already exist ');", true);
                return;

            }
            if (intErrCode == 0)
            {
                if (intLOBID > 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Line of Business details updated successfully");
                }
                else
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strAlert = "Line of Business details added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";

                    //strAlert = strAlert.Replace("__ALERT__", "Line of Business details added successfully");
                    //strAlert = "if(confirm('Line of Business details added successfully\nWould you like to add one more record?')){window.location.href='../System Admin/S3GSysAdminLOBMaster_Add.aspx';}else {" + strRedirectPage + "}";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
            }
            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "Line of Business Code already exists in the system. Kindly enter a new Line of Business Code");
                strRedirectPageView = "";
            }
            else if (intErrCode == 3)
            {
                strAlert = strAlert.Replace("__ALERT__", "Line of Business Name already exists in the system. Kindly enter a new Line of Business Name");
                strRedirectPageView = "";
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
    }
    
    /// <summary>
    /// Clears all the record on confirmation 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (intLOBID == 0)
        {
            txtLOBCode.Text = "";
            txtLOBName.Text = "";
            //chkActive.Checked = false;
        }
    }
    
    /// <summary>
    /// Redirect the LOB Master View Page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminLOBMaster_View.aspx");
    }

    #region ValueDisable
    /// <summary>
    /// Disable Controls based on the User Roles
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                if (!bCreate)
                {
                    //  btnSave.Enabled = false;
                }
                txtLOBCode.Enabled = true;
                txtLOBName.Enabled = true;
                chkActive.Checked = true;
                btnReset.Enabled = true;
                //  btnClear.Enabled = true;
                chkActive.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                break;

            case 1: // Modify Mode
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                FunPriGetLOBDetails();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                txtLOBCode.Enabled = false;
                //changed by bhuvan 
                txtLOBName.Enabled = true;
                //end here
                btnReset.Enabled = false;
                chkActive.Enabled = true;
                break;

            case -1:// Query Mode
                FunPriGetLOBDetails();
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                txtLOBCode.ReadOnly = true;
                txtLOBName.ReadOnly = true;
                btnSave.Enabled = false;
                btnReset.Enabled = false;
                chkActive.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                break;
        }

    }
    #endregion

}
