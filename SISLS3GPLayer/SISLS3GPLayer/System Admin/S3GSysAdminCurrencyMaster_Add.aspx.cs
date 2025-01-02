/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminCurrencyMaster_Add
/// Created By      :   Kaliraj K
/// Created Date    :   10-May-2010
/// Purpose         :   To insert and update Currency master details

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



public partial class System_Admin_S3GSysAdminCurrencyMaster_Add : ApplyThemeForProject
{
    #region Intialization
    AccountMgtServicesReference.AccountMgtServicesClient objCurrencyMasterClient;
    int intErrCode = 0;
    int intCurrencyId = 0;
    AccountMgtServices.S3G_SYSAD_CurrencyMaster_CUDataTable ObjS3G_SYSAD_CurrencyMaster_CUDataTable = new AccountMgtServices.S3G_SYSAD_CurrencyMaster_CUDataTable();
    AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable ObjS3G_SYSAD_CurrencyMasterDataTable = new AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable();
    AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable ObjS3G_SYSAD_CurrencyMaster_ViewDataTable = new AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable();

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "../System Admin/S3GSysAdminCurrencyMaster_View.aspx";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminCurrencyMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminCurrencyMaster_Add.aspx';";
    int intCompanyID = 0;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    //Code end
    UserInfo ObjUserInfo = null;
    int intUserID = 0;
    Dictionary<string, string> Procparam = null;
    #endregion

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {

        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        if (Request.QueryString["qsCurrencyId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCurrencyId"));

            if (fromTicket != null)
            {
                intCurrencyId = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }

            strMode = Request.QueryString["qsMode"];
        }

        if (!IsPostBack)
        {

            FunBindCurrency();

            //User Authorization
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if ((intCurrencyId > 0) && (strMode == "M"))
            {
                //FunGetCurrencyDetails();
                FunPriDisableControls(1);

            }
            else if ((intCurrencyId > 0) && (strMode == "Q")) // Query // Modify
            {
                //FunGetCurrencyDetails();
                FunPriDisableControls(-1);

            }
            else
            {
                FunPriDisableControls(0);
            }

            //Code end

        }
    }

    #endregion

    #region Page Events

    /// <summary>
    /// This is used to save currecny details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSave_Click(object sender, EventArgs e)
    {
        objCurrencyMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            AccountMgtServices.S3G_SYSAD_CurrencyMaster_CURow ObjCurrencyMasterRow;
            ObjCurrencyMasterRow = ObjS3G_SYSAD_CurrencyMaster_CUDataTable.NewS3G_SYSAD_CurrencyMaster_CURow();
            ObjCurrencyMasterRow.Company_ID = intCompanyID;
            ObjCurrencyMasterRow.Currency_ID = Convert.ToInt32(ddlCurrencyName.SelectedValue);
            //ObjCurrencyMasterRow.Currency_Code = txtCurrencyCode.Text;
            //ObjCurrencyMasterRow.Currency_Name = txtCurrencyName.Text;
            ObjCurrencyMasterRow.Precision = Convert.ToInt32(txtPrecision.Text);
            ObjCurrencyMasterRow.Is_Active = chkActive.Checked;
            ObjCurrencyMasterRow.Created_By = intUserID;
            ObjCurrencyMasterRow.Modified_By = intUserID;

            ObjS3G_SYSAD_CurrencyMaster_CUDataTable.AddS3G_SYSAD_CurrencyMaster_CURow(ObjCurrencyMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;

            if (intCurrencyId > 0)
            {
                intErrCode = objCurrencyMasterClient.FunPubModifyCurrency(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_CurrencyMaster_CUDataTable, SerMode));
            }
            else
            {
                intErrCode = objCurrencyMasterClient.FunPubCreateCurrency(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_CurrencyMaster_CUDataTable, SerMode));
            }

            if (intErrCode == 0)
            {
                if (intCurrencyId > 0)
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    Utility.FunShowAlertMsg(this.Page, "Currency details updated successfully", strRedirectPage);
                }
                else
                {
                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    //Utility.FunShowAlertMsg(this.Page,"Currency details added successfully",strRedirectPage);
                    strAlert = "Currency details added successfully";
                    strAlert += @"\n\nWould you like to add one more currency?";

                    //strAlert = strAlert.Replace("__ALERT__", "Line of Business details added successfully");
                    //strAlert = "if(confirm('Line of Business details added successfully\nWould you like to add one more record?')){window.location.href='../System Admin/S3GSysAdminLOBMaster_Add.aspx';}else {" + strRedirectPage + "}";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
            }
            else if (intErrCode == 1)
            {
                if (intCurrencyId > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Currency cannot be deactivated,once it has been defined as default currency for a company");
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Currency Name already exist");
                }
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Currency Name already exist");
            }
            lblErrorMessage.Text = string.Empty;

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objCurrencyMasterClient.Close();
        }
    }

    /// <summary>
    /// This is used to redirect page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    /// <summary>
    /// This is used to clear data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnClear_Click(object sender, EventArgs e)
    {
        //txtCurrencyName.Text = "";
        ddlCurrencyName.SelectedIndex = 0;
        txtPrecision.Text = "";
        chkActive.Checked = true;
    }

    //protected void btnDelete_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        AccountMgtServices.S3G_SYSAD_CurrencyMasterRow ObjCurrencyMasterRow;
    //        ObjCurrencyMasterRow = ObjS3G_SYSAD_CurrencyMasterDataTable.NewS3G_SYSAD_CurrencyMasterRow();
    //        ObjCurrencyMasterRow.Company_ID = 3;
    //        ObjCurrencyMasterRow.Currency_ID = intCurrencyId;
    //        ObjS3G_SYSAD_CurrencyMasterDataTable.AddS3G_SYSAD_CurrencyMasterRow(ObjCurrencyMasterRow);
    //        SerializationMode SerMode = SerializationMode.Binary;
    //        objCurrencyMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
    //        intErrCode = objCurrencyMasterClient.FunPubDeleteCurrency(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_CurrencyMasterDataTable, SerMode));
    //        Utility.FunShowAlertMsg(this.Page, "Record deleted successfully", strRedirectPage);
    //    }
    //    catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    #endregion

    #region Page Methods

    /// <summary>
    /// To bind Currency Name
    /// </summary>

    private void FunBindCurrency()
    {
        //LOB List
        try
        {

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Is_Active", "1");
            ddlCurrencyName.BindDataTable(SPNames.SYS_CurrencyMaster, Procparam, new string[] { "Currency_ID", "Currency_Code", "Currency_Name" });
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = "Unable to load Currency Name";
        }

    }

    /// <summary>
    /// This method is used to display Currency details
    /// </summary>
    private void FunGetCurrencyDetails()
    {
        objCurrencyMasterClient = new AccountMgtServicesReference.AccountMgtServicesClient();
        try
        {
            ObjS3G_SYSAD_CurrencyMasterDataTable = new AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable();
            AccountMgtServices.S3G_SYSAD_CurrencyMasterRow ObjCurrencyMasterRow;
            ObjCurrencyMasterRow = ObjS3G_SYSAD_CurrencyMasterDataTable.NewS3G_SYSAD_CurrencyMasterRow();
            ObjCurrencyMasterRow.Company_ID = intCompanyID;
            ObjCurrencyMasterRow.Currency_ID = intCurrencyId;
            ObjS3G_SYSAD_CurrencyMasterDataTable.AddS3G_SYSAD_CurrencyMasterRow(ObjCurrencyMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteCurrencyDetails = objCurrencyMasterClient.FunPubQueryCurrency(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_CurrencyMasterDataTable, SerMode));
            ObjS3G_SYSAD_CurrencyMaster_ViewDataTable = (AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteCurrencyDetails, SerializationMode.Binary, typeof(AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable));

            //txtCurrencyCode.Text = ObjS3G_SYSAD_CurrencyMaster_ViewDataTable.Rows[0]["Currency_Code"].ToString();
            //txtCurrencyName.Text = ObjS3G_SYSAD_CurrencyMaster_ViewDataTable.Rows[0]["Currency_Name"].ToString();
            ddlCurrencyName.SelectedValue = intCurrencyId.ToString();
            txtPrecision.Text = ObjS3G_SYSAD_CurrencyMaster_ViewDataTable.Rows[0]["Precision"].ToString();
            hdnID.Value = ObjS3G_SYSAD_CurrencyMaster_ViewDataTable.Rows[0]["Created_By"].ToString();
            if (ObjS3G_SYSAD_CurrencyMaster_ViewDataTable.Rows[0]["Is_Active"].ToString() == "True")
                chkActive.Checked = true;
            else
                chkActive.Checked = false;

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objCurrencyMasterClient.Close();
        }
    }

    //Added by Kali K on 26-JUL-2010 
    //This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                chkActive.Enabled = false;
                chkActive.Checked = true;

                break;

            case 1: // Modify Mode
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }

                FunGetCurrencyDetails();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                ddlCurrencyName.Enabled = false;
                chkActive.Enabled = true;

                btnClear.Enabled = false;

                break;

            case -1:// Query Mode
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                FunGetCurrencyDetails();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                ddlCurrencyName.Enabled = true;
                txtPrecision.ReadOnly = true;
                chkActive.Enabled = false;
                if (bClearList)
                {
                    ddlCurrencyName.ClearDropDownList();
                }
                btnClear.Enabled = false;
                btnSave.Enabled = false;

                break;
        }

    }

    //Code end

    #endregion

}
