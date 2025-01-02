#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Document Path Setup
/// Created By			: Nataraj Y
/// Created Date		: 28-May-2010
/// Purpose	            : 
/// Last Updated Date   : 28-May-2010
/// Reason              : Document Path Setup
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 29-Jun-2010
/// Reason              : Bug Fixing
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Configuration;
#endregion

public partial class System_Admin_S3GSysAdminDocPath_Add_ : ApplyThemeForProject
{

    #region Initialization
    int intErrCode = 0;
    DocMgtServicesReference.DocMgtServicesClient ObjDocServicesClient;
    int intUserId = 0;
    int intCompanyID = 0;
    int intDocPathId = 0;
    int inthdUserid;
    string strRedirectPageMC;
    string strMode = string.Empty;
    SerializationMode SerMode = SerializationMode.Binary;
    DocMgtServices.S3G_SYSAD_DocumentationPathDataTable ObjS3G_SYSAD_DocumentationPathDataTable;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bClearList = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminDocPath_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminDocPath_Add.aspx';";
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            if (Request.QueryString["qsDocPathId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsDocPathId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intDocPathId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            if (!IsPostBack)
            {
                FunLoadCombos(intCompanyID);
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (((intDocPathId > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }

                else if (((intDocPathId > 0)) && (strMode == "Q"))
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);

                }

            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }

        txtDocPath.ToolTip = txtDocPath.Text.Trim();
    }
    #endregion

    #region Page Events
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjDocServicesClient = new DocMgtServicesReference.DocMgtServicesClient();
        try
        {
            //condition added to validate Doc Location path - Kuppusamy.B - Feb-29-2012
            if (txtDocPath.Text.Trim() != "")
            {
                string[] strScan = txtDocPath.Text.Trim().Split('\\');
                var matchQuery = from word in strScan
                                 //where word==""
                                 where string.IsNullOrEmpty(word.Trim())
                                 select word;

                // Count the matches.
                int wordCount = matchQuery.Count();

                if (wordCount >= 2 || txtDocPath.Text.Trim().Contains('/') || txtDocPath.Text.Trim().Contains(": ") || txtDocPath.Text.Trim().Contains(" \\"))
                {
                    Utility.FunShowAlertMsg(this.Page, "Invalid Document path");
                    return;
                }

            }

            if (txtDocPath.Text.Trim() != "" && !Directory.Exists(txtDocPath.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this.Page, "Invalid Document path");
                return;
            }

            if (Directory.Exists(txtDocPath.Text))
            {
                if (!(txtDocPath.Text.EndsWith(":")))
                {
                    lblErrorMessage.InnerText = "";
                    ObjS3G_SYSAD_DocumentationPathDataTable = new DocMgtServices.S3G_SYSAD_DocumentationPathDataTable();
                    DocMgtServices.S3G_SYSAD_DocumentationPathRow ObjDocPathRow;
                    ObjDocPathRow = ObjS3G_SYSAD_DocumentationPathDataTable.NewS3G_SYSAD_DocumentationPathRow();
                    ObjDocPathRow.Document_Path_ID = intDocPathId;
                    ObjDocPathRow.Company_ID = intCompanyID;
                    ObjDocPathRow.Document_Flag_ID = Convert.ToInt32(ddlDocumentflag.SelectedItem.Value);
                    ObjDocPathRow.Document_Path = txtDocPath.Text.Trim();
                    ObjDocPathRow.Created_By = intUserId;
                    ObjDocPathRow.Modified_By = intUserId;
                    ObjDocPathRow.Is_Active = chkActive.Checked;
                    ObjDocPathRow.LOB_ID = Convert.ToInt32(ddlLOBcode.SelectedItem.Value);
                    ObjDocPathRow.Role_Code_ID = Convert.ToInt32(ddlRolecode.SelectedItem.Value);
                    ObjS3G_SYSAD_DocumentationPathDataTable.AddS3G_SYSAD_DocumentationPathRow(ObjDocPathRow);
                    byte[] byteDocumentpathTable = ClsPubSerialize.Serialize(ObjS3G_SYSAD_DocumentationPathDataTable, SerMode);
                    if (intDocPathId > 0)
                    {
                        //if (FunCheckLobisvalid(ddlLOBcode.SelectedValue))
                        //{
                        intErrCode = ObjDocServicesClient.FunPubModifyDocPathDetails(SerMode, byteDocumentpathTable);
                        //}
                        //else
                        //{
                        //strAlert = strAlert.Replace("__ALERT__", "Selected Line of Business rights not assigned");
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        //return;
                        //}
                    }
                    else
                    {
                        intErrCode = ObjDocServicesClient.FunPubCreateDocPathDetails(SerMode, byteDocumentpathTable);
                    }
                    if (intErrCode == 0)
                    {
                        if (intDocPathId > 0)
                        {
                            //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = strAlert.Replace("__ALERT__", "Document Path details updated successfully");
                            //strRedirectPageView = "";
                        }
                        else
                        {
                            //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here
                            strAlert = "Document Path details added successfully";
                            strAlert += @"\n\nWould you like to add one more Record?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                        }
                    }
                    else if (intErrCode == 1)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Document Path details already exists for the given combination");
                        strRedirectPageView = "";
                    }
                    else if (intErrCode == 5)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Document Path details already exists");
                        strRedirectPageView = "";
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Error in Document path Creation");
                        strRedirectPageView = "";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Invalid document path');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Invalid document path');", true);
            }
        }
        catch (FaultException<DocMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
            throw ex;
        }
        finally
        {
            ObjDocServicesClient.Close();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/System Admin/S3GSysAdminDocPath_View.aspx");
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlDocumentflag.SelectedIndex = 0;
            ddlLOBcode.SelectedIndex = 0;
            //Modification Start--Ganapathy
            ddlRolecode.SelectedIndex = 0;
            //ListItem ListItem = new ListItem("--Select--", "0");
            //ddlRolecode.Items.Add(ListItem);
            //Modification End
            txtDocPath.Text = "";
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }

    protected void ddlLOBcode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            /* if (ddlLOBcode.SelectedIndex > 0)
             {
                 FunPriGetRoleCode(Convert.ToInt32(ddlLOBcode.SelectedItem.Value));
             }
             else
             {
                 ddlRolecode.Items.Clear();
                 //Modification Start--Ganapathy
                 //ListItem ListItem = new ListItem("--Select--", "0");
                 //ddlRolecode.Items.Add(ListItem);
                 //Modification End
             }*/
            ddlLOBcode.AddItemToolTip();
            ddlRolecode.AddItemToolTip();
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }
    #endregion

    #region Page Methods

    /// <summary>
    /// This method is used to display Company details
    /// </summary>
    private void FunPriGetDocPathDetails()
    {
        ObjDocServicesClient = new DocMgtServicesReference.DocMgtServicesClient();
        try
        {
            DataTable dtDocPath;
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteDocPathDetails = ObjDocServicesClient.FunPubGetDocPathDetails(intCompanyID, intDocPathId);
            dtDocPath = (DataTable)ClsPubSerialize.DeSerialize(byteDocPathDetails, SerMode, typeof(DataTable));
            ddlDocumentflag.SelectedValue = dtDocPath.Rows[0]["Document_Flag_ID"].ToString();
            if (Convert.ToInt32(dtDocPath.Rows[0]["LOB_ID"].ToString()) > 0)
                ddlLOBcode.SelectedValue = dtDocPath.Rows[0]["LOB_ID"].ToString();
            FunPriGetRoleCode();
            ddlRolecode.SelectedValue = dtDocPath.Rows[0]["Role_Code_ID"].ToString();
            txtDocPath.Text = dtDocPath.Rows[0]["Document_Path"].ToString();
            hdnID.Value = dtDocPath.Rows[0]["Created_By"].ToString();
            chkActive.Checked = Convert.ToBoolean(dtDocPath.Rows[0]["Is_Active"].ToString());
            lblErrorMessage.InnerText = string.Empty;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
            throw ex;
        }
        finally
        {
            ObjDocServicesClient.Close();
        }
    }

    /// <summary>
    /// Method to load combos
    /// </summary>
    /// <param name="intCompanyID"></param>
    private void FunLoadCombos(int intCompanyID)
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            if (intDocPathId == 0)
            {
                Procparam.Add("@Is_Active", "1");
            }
            if (intDocPathId == 0)
            {
                Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            }
            Procparam.Add("@Program_ID", "15");
            //ddlLOBcode.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOBcode.BindDataTable(SPNames.LOBMaster, Procparam, true, "All", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            FunPriGetRoleCode();
           
            Procparam.Clear();
            Procparam.Add("@Document_Flag_ID", "0");
            ddlDocumentflag.BindDataTable(SPNames.DocumentFlagList, Procparam, new string[] { "Document_Flag_ID", "Document_Flag" });
            ddlDocumentflag.AddItemToolTip();
            ddlLOBcode.AddItemToolTip();
            ddlRolecode.AddItemToolTip();
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }

    /// <summary>
    /// Loads Role code based on LOB Selected
    /// </summary>
    /// <param name="intLOB_ID">lob id for which role to be loaded</param>
    private void FunPriGetRoleCode()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());

            if (intDocPathId == 0)
            {
                //Procparam.Add("@LOB_ID", intLOB_ID.ToString());
                // Procparam.Add("@Program_ID", "15");
                // Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
                Procparam.Add("@Is_Active", "1");
            }
            ddlRolecode.BindDataTable("S3G_Get_UserRole_DetailsDocPath", Procparam, new string[] { "Role_Code_ID", "Program_Name" });
            ddlRolecode.AddItemToolTip();
            ddlLOBcode.AddItemToolTip();
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }


    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode
                    chkActive.Enabled = false;
                    chkActive.Checked = true;
                    break;

                case 1: // Modify Mode
                    FunPriGetDocPathDetails();
                    ddlDocumentflag.Enabled = false;
                    ddlLOBcode.ClearDropDownList();
                    ddlRolecode.ClearDropDownList();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    btnClear.Enabled = false;
                    break;

                case -1:// Query Mode                
                    FunPriGetDocPathDetails();
                    txtDocPath.ReadOnly = true;

                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPageView);
                    }
                    if (bClearList)
                    {
                        ddlDocumentflag.ClearDropDownList();
                        ddlLOBcode.ClearDropDownList();
                        ddlRolecode.ClearDropDownList();
                    }

                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    chkActive.Enabled = false;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    break;
            }
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }

    #endregion
    private bool FunCheckLobisvalid(string strLobId)
    {
        try
        {
            Dictionary<string, string> Procparm = new Dictionary<string, string>();
            Procparm.Add("@Company_ID", intCompanyID.ToString());
            Procparm.Add("@User_Id", intUserId.ToString());
            Procparm.Add("@LOB_ID", strLobId);
            if (intDocPathId == 0)
            {
                Procparm.Add("@Is_Active", "1");
            }
            Procparm.Add("@Is_UserLobActive", "1");
            Procparm.Add("@Program_ID", "15");
            DataTable dtBool = Utility.GetDefaultData("S3G_GetUserLobMapping", Procparm);
            if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
                return true;
            else
                return false;
        }
        catch (Exception ae)
        {
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
    }
}
