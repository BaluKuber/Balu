/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name               : Legal Repossession 
/// Screen Name               : Repossession Track
/// Created By                : Swarnalatha
/// Created Date              : 23-April-2011
/// Purpose                   : 
/// Last Updated By           :
/// Last Updated Date         : 
/// Reason                    : 
/// <Program Summary>
#region NameSpace
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.IO;
using System.Configuration;
using S3GBusEntity.Collection;
using System.Globalization;
using System.Web.Services;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
#endregion

public partial class LegalRepossession_S3GLRlRepossessionTrack : ApplyThemeForProject
{
    # region Variable Declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> Procparam = null;
    int strTrack = 0;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    StringBuilder strTrackBuilder = new StringBuilder();
    DataTable dtList = new DataTable();
    string strKey = "Insert";
    string strPageName = "Repossession Track";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LegalRepossession/S3GLRTransLander.aspx?Code=GRT";
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRlRepossessionTrack.aspx';";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GRT';";
    string strValid = "LR_GRT";
    //User Authorization
    public string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    //bool bDelete = false;
    //bool bMakerChecker = false;
    bool bMakerChecker = false;
    //Code end
    string strProcName = string.Empty;//S3G_ORG_GetEntityDetails_Paging 
    LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient obj_TrackClient;
    S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionTrackDataTable objS3G_LR_TrackTable = null;
    S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionTrackRow objS3G_LR_TrackRow = null;



    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriPageLoad();
        }
        catch (Exception)
        {

            cvTracks.ErrorMessage = "Due to data problem, Unable to load the Repossession track";
            cvTracks.IsValid = false;
        }
    }
    #endregion

    #region Drop Down
    protected void ddlLRNno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (!ddlLRNno.SelectedValue.Equals("0"))
                PopulateDocketno((ddlLRNno.SelectedItem.Text.Substring(0, ddlLRNno.SelectedItem.Text.LastIndexOf("-")).Trim().ToString()));
            else
            {
                ddlDocketno.SelectedIndex = 0;
                if (ddlDocketno.Items.Count > 0)
                    ddlDocketno.ClearDropDownList();
                FunPriClearTabValues(true, true);
            }
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    protected void ddlDocketno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!ddlDocketno.SelectedValue.Equals("0"))
                FunPriGetTabValues(ddlLRNno.SelectedValue);
            else
            {
                FunPriClearTabValues(false, false);
            }

        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    protected void RDInsBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PopulateInspectorCode();
            FunProClearTrackDetails(false, true);
        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void ddlInspecBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearTrackDetails(false, true);
            PopulatNewGarageCode();
            PopulateGaragedetails();


        }
        catch (Exception ex)
        {

              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void ddlNewGarage_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunProClearTrackDetails(false, false);
        PopulateGaragedetails();

    }
    #endregion

    #region Button events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriInsertTrack();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    protected void BtnNewInspection_Click(object sender, EventArgs e)
    {
        try
        {

            FunpriNewEntry();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearTabValues(true, true);
            txtDocketdate.Text = "";
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    #endregion

    #region Textbox events
    protected void txtInspecDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtInspecDate.Text))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtDocketdate.Text).ToString("dd/MMM/yyyy")) >
                Convert.ToDateTime(Utility.StringToDate(txtInspecDate.Text).ToString("dd/MMM/yyyy")))
            {
                Utility.FunShowAlertMsg(this, "Inspection date should be greater than or equal to the Repossession docket date (" + txtDocketdate.Text + ")");
                txtInspecDate.Text = string.Empty;
            }

            string str = string.Empty;
            if (ViewState["InsDate"] != null)
                str = Convert.ToString(ViewState["InsDate"].ToString());
            if (GrvHistory.Rows.Count != 0)
                if (FunPriValidateFromEndDate(str, txtInspecDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "Inspection date should be greater than the Last inspected date (" + str + ")");
                    txtInspecDate.Text = string.Empty;
                }
        }

    }

    protected void txtNewGarageIn_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtNewGarageIn.Text))
        {
            if (FunPriValidateFromEndDate(txtOldGarageOut.Text, txtNewGarageIn.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this, "Garage in date should be greater than the last Garage out date");
                txtNewGarageIn.Text = string.Empty;
            }

            string str = string.Empty;
            if (ViewState["LastGIN"] != null)
                str = Convert.ToString(ViewState["LastGIN"].ToString());
            if (FunPriValidateFromEndDate(str, txtNewGarageIn.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this, "Garage in date should be greater than the last Garage in date(" + str + ")");
                txtNewGarageIn.Text = string.Empty;
            }
        }
    }

    protected void txtOldGarageOut_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtOldGarageOut.Text))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtDocketdate.Text).ToString("dd/MMM/yyyy")) >
                Convert.ToDateTime(Utility.StringToDate(txtOldGarageOut.Text).ToString("dd/MMM/yyyy")))
            {
                Utility.FunShowAlertMsg(this, "Old Garage out date should be greater than or equal to the Repossession docket date (" + txtDocketdate.Text + ")");
                txtOldGarageOut.Text = string.Empty;
            }

            if (FunPriValidateFromEndDate(txtOldGarageOut.Text, txtNewGarageIn.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this, "Old Garage Out date should be lesser than the last Garage In date");
                txtNewGarageIn.Text = string.Empty;
            }


            string str = string.Empty;
            if (ViewState["LastGOUT"] != null)
                str = Convert.ToString(ViewState["LastGOUT"].ToString());
            if (FunPriValidateFromEndDate(str, txtOldGarageOut.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this, "Old Garage out date should be greater than the last Garage out date(" + str + ")");
                txtOldGarageOut.Text = string.Empty;
            }
        }
    }
    #endregion

    #region UserDefined Function

    /// <summary>
    /// perform page load functions
    /// </summary>
    private void FunPriPageLoad()
    {
        S3GSession ObjS3GSession = null;
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPubSetIndex(1);
            //Date Format
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            //End
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strTrack = Convert.ToInt32(fromTicket.Name);
            }

            txtNewGarageIn.Attributes.Add("onblur", "fnDoDate(this,'" + txtNewGarageIn.ClientID + "','" + strDateFormat + "',true, false );");
            txtOldGarageOut.Attributes.Add("onblur", "fnDoDate(this,'" + txtOldGarageOut.ClientID + "','" + strDateFormat + "',true, false );");
            txtInspecDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInspecDate.ClientID + "','" + strDateFormat + "',true, false );");

            //PopulateErrorMessage();
            if (!IsPostBack)
            {
                CalendarExtender1.Format = CalendarExtender2.Format = CalendarExtender3.Format = strDateFormat;
                tcRepossTrack.ActiveTabIndex = 0;
                if (Request.QueryString["qsMode"] != null)
                    strMode = Request.QueryString["qsMode"].ToString();

                //CalendarExtender2.Format = strDateFormat;
                PopulateLRNno();
                PopulateInspectorCode();
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (strMode == "Q")
                {
                    FunGetTrackForModification(strTrack);
                    FunPriDisableControls(-1);
                }
                else if (strMode == "M")
                {
                    FunGetTrackForModification(strTrack);
                    FunPriDisableControls(1);
                    tcRepossTrack.Tabs[3].Enabled = false;
                }
                else
                {
                    txtTrackDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    FunPriDisableControls(0);
                    tcRepossTrack.Tabs[3].Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ObjS3GSession = null;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjS3GSession = null;
        }
    }

    /// <summary>
    /// to populate error message
    /// </summary>
    //private void PopulateErrorMessage()
    //{
    //    rfvddlLRN.ErrorMessage = "Select Legal Repossession Number in General Tab";
    //    rfvddlDocketno.ErrorMessage = "Select Repossession Docket Number in General Tab";
    //    rfvddlInspecBy.ErrorMessage = "Select Inspector Name in Track Details Tab";
    //    rfvtxtInspecDate.ErrorMessage = "Select Inspection Date in Track Details Tab";
    //    rfvNewin.ErrorMessage = "Select New Garage InDate";
    //    rfvOldout.ErrorMessage = "Select Old Garage OutDate";
    //}

    /// <summary>
    /// To pululate All details on Modify/Query mode
    /// </summary>
    /// <param name="strTrack"></param>
    private void FunGetTrackForModification(int strTrack)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Track_ID", Convert.ToString(strTrack));
            DataSet Dst = new DataSet();
            Dst = Utility.GetDataset("S3g_LR_GetTrackDetails", Procparam);
            if (Dst.Tables[0].Rows.Count > 0)
            {
                ddlLRNno.SelectedValue = Dst.Tables[0].Rows[0]["LRN_ID"].ToString();
                PopulateDocketno(ddlLRNno.SelectedItem.Text.Substring(0, ddlLRNno.SelectedItem.Text.LastIndexOf("-")).Trim().ToString());
                txtTrackNo.Text = Dst.Tables[0].Rows[0]["LR_Track_No"].ToString();
                txtTrackDate.Text = Dst.Tables[0].Rows[0]["Repossesion_Track_Date"].ToString();
                ddlDocketno.SelectedValue = Dst.Tables[0].Rows[0]["Repossession_ID"].ToString();
                FunPriGetTabValues(ddlDocketno.SelectedValue);
                ViewState["Track_Details_ID"] = Dst.Tables[0].Rows[0]["LR_Track_Details_ID"].ToString();
                RDInsBy.SelectedValue = Dst.Tables[0].Rows[0]["Inspection_Done_By_Type_Code"].ToString();
                PopulateInspectorCode();
                ddlInspecBy.SelectedValue = Dst.Tables[0].Rows[0]["Inspector_ID"].ToString();
                txtInspecDate.Text = Dst.Tables[0].Rows[0]["Inspected_Date"].ToString();
                PopulatNewGarageCode();
                ddlNewGarage.SelectedValue = Dst.Tables[0].Rows[0]["New_Garage_ID"].ToString();
                PopulateGaragedetails();
                txtNewGarageIn.Text = Dst.Tables[0].Rows[0]["Garage_In"].ToString();
                txtOldGarageOut.Text = Dst.Tables[0].Rows[0]["Garage_Out"].ToString();
                txtRemarks.Text = Dst.Tables[0].Rows[0]["Remarks"].ToString();
            }
            if (Dst.Tables[1].Rows.Count > 0)
            {
                ViewState["Inspector"] = ddlInspecBy.SelectedItem.Text;
                ViewState["Garagecode"] = ddlNewGarage.SelectedValue;
                ViewState["InsDate"] = txtInspecDate.Text;
                ViewState["LastGIN"] = Dst.Tables[1].Rows[0]["Garage_In"].ToString();
                ViewState["LastGOUT"] = Dst.Tables[1].Rows[0]["Garage_Out"].ToString();
                ViewState["Remarks"] = Dst.Tables[0].Rows[0]["Remarks"].ToString();
            }
            if (Dst.Tables[2].Rows.Count > 0)
            {
                ViewState["history"] = Dst.Tables[2];
                GrvHistory.DataSource = Dst.Tables[2];
                GrvHistory.DataBind();
            }

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To populate the Legal Repossession number in page load
    /// </summary>
    private void PopulateLRNno()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", null);
            Procparam.Add("@Location_ID", null);
            if (strTrack == 0)
                Procparam.Add("@ProgramCode", "GRT");
            //Procparam.Add("@Action", "2");
            ddlLRNno.BindDataTable("S3G_LR_GetLRNNumber", Procparam, new string[] { "LRN_ID", "LRN_No", "Customer_Name" });
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    /// to populate Docket Number Based on the Legal Repossession number value
    /// </summary>
    /// <param name="strLRN"></param>
    private void PopulateDocketno(string strLRN)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LRN_No", strLRN);
            if (strMode == string.Empty)
                Procparam.Add("@Mode", "C");
            else
                Procparam.Add("@Mode", strMode);
            Procparam.Add("@ProgramCode", "GRT");
            ddlDocketno.BindDataTable("S3G_LR_GetDocketNumber", Procparam, new string[] { "Repossession_ID", "Repossession_Docket_No" });

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    /// To clear Tab Values
    /// </summary>
    private void FunPriClearTabValues(bool ClearLRNum, bool ClearRDNum)
    {
        //------------------Start General Tab------------------
        if (ClearLRNum)
        {
            ddlLRNno.SelectedIndex = 0;
        }
        txtAction.Text = string.Empty;
        if (ClearRDNum)
        {
            ddlDocketno.SelectedIndex = 0;
            ddlDocketno.ClearDropDownList();
        }
        txtTrackNo.Text = string.Empty;
        txtLOB.Text = string.Empty;
        txtBranch.Text = string.Empty;
        txtPANum.Text = string.Empty;
        txtSANum.Text = string.Empty;
        txtAcconutDate.Text = string.Empty;
        txtFinAmount.Text = string.Empty;
        txtTenure.Text = string.Empty;
        ucCustDetails.ClearCustomerDetails();
        grvGuarantor.DataSource = null;
        grvGuarantor.DataBind();
        //------------------End General Tab------------------
        //------------------Start Repossession Asset Tab------------------
        grvAsset.DataSource = null;
        grvAsset.DataBind();
        txtGcode.Text = string.Empty;
        txtGID.Text = string.Empty;
        txtGIn.Text = string.Empty;
        txtCondAsset.Text = string.Empty;
        txtAssetInventory.Text = string.Empty;
        txtInsValid.Text = string.Empty;
        txtGaddress.Text = string.Empty;
        //------------------End Repossession Asset Tab------------------
        //------------------Start Repossession Track  History Tab----------------
        RDInsBy.SelectedValue = "1";
        FunProClearTrackDetails(true, true);

        //------------------End Repossession Track History Tab------------------
        ViewState["InsDate"] = null;
        ViewState["LastGIN"] = null;
        ViewState["LastGOUT"] = null;
        ViewState["history"] = null;
        ViewState["Track_Details_ID"] = null;
        GrvHistory.DataSource = null;
        GrvHistory.DataBind();

    }

    protected void FunProClearTrackDetails(bool ClearInsp, bool ClearGrgCode)
    {
        if (ClearInsp)
        {
            ddlInspecBy.SelectedIndex = 0;
        }
        if (ClearGrgCode)
        {
           
          ddlNewGarage.SelectedIndex = 0;
        }
        txtInspecDate.Text = string.Empty;
        txtNewGarageIn.Text = string.Empty;
        txtOldGarageOut.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        txtGarageName.Text = string.Empty;
        txtGarageAddress.Text = string.Empty;

    }

    /// <summary>
    /// to get tab values 
    /// Modified by anitha for concatenate customer name with the LRN number
    /// </summary>
    /// <param name="p"></param>
    private void FunPriGetTabValues(string p)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            DataSet DSN = new DataSet();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LRN_No", Convert.ToString(ddlLRNno.SelectedItem.Text.Substring(0, ddlLRNno.SelectedItem.Text.LastIndexOf("-")).Trim()));
            Procparam.Add("@Docket_ID", ddlDocketno.SelectedItem.Text);
            if (!ddlLRNno.SelectedValue.Equals("0") && (!ddlDocketno.SelectedValue.Equals("0")))
            {

                DSN = Utility.GetDataset("S3G_LR_GetLRNRepossessionDetails", Procparam);

                PopulateTabValues(DSN);
            }
            else
            {
                FunPriClearTabValues(true, true);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    /// to populate Tab values
    /// </summary>
    /// <param name="DS"></param>
    private void PopulateTabValues(DataSet DS)
    {
        try
        {
            // --------------Start General Tab Value-------------------
            if (DS.Tables[0].Rows.Count >= 1)
            {
                txtAction.Text = DS.Tables[0].Rows[0]["Lookup_Description"].ToString();
                txtDocketdate.Text = DS.Tables[0].Rows[0]["Repossession_Docket_Date"].ToString();
                txtBranch.Text = DS.Tables[0].Rows[0]["Branch_Name"].ToString();
                txtLOB.Text = DS.Tables[0].Rows[0]["LOB_Name"].ToString();
                txtPANum.Text = DS.Tables[0].Rows[0]["PANum"].ToString();
                txtSANum.Text = DS.Tables[0].Rows[0]["SANum"].ToString();
                txtAcconutDate.Text = DS.Tables[0].Rows[0]["Created_Date"].ToString();
                txtFinAmount.Text = DS.Tables[0].Rows[0]["Finance_Amount"].ToString();
                txtTenure.Text = DS.Tables[0].Rows[0]["Tenure"].ToString();
                DataRow dtRow = DS.Tables[0].Rows[0];
                ucCustDetails.SetCustomerDetails(dtRow, true);
            }
            if (DS.Tables[1].Rows.Count >= 1)
            {
                grvGuarantor.DataSource = DS.Tables[1];
                grvGuarantor.DataBind();
                pnlGuarantor.Visible = true;
            }
            // --------------End General Tab Value-------------------
            // --------------Start Asset Tab Value-------------------
            if (DS.Tables[2].Rows.Count >= 1)
            {
                grvAsset.DataSource = DS.Tables[2];
                grvAsset.DataBind();
                pnlAsset.Visible = true;
            }
            if (DS.Tables[3].Rows.Count >= 1)
            {
                txtGID.Text = DS.Tables[3].Rows[0]["Garage_ID"].ToString();
                txtGcode.Text = DS.Tables[3].Rows[0]["Garage_Code"].ToString();
                txtGIn.Text = DS.Tables[3].Rows[0]["Garage_In"].ToString();
                txtGaddress.Text = DS.Tables[3].Rows[0]["Address"].ToString();
                txtAssetInventory.Text = DS.Tables[3].Rows[0]["Asset_Inventory_Details"].ToString();
                txtCondAsset.Text = DS.Tables[3].Rows[0]["Asset_Condition"].ToString();
                txtInsValid.Text = DS.Tables[4].Rows[0]["Insurance_validity"].ToString();
            }
            // --------------End Asset Tab Value-------------------
            // --------------Start History Tab Value-------------------
            // --------------Start End Tab Value-------------------
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    /// <summary>
    /// User Access
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    //btnListing.Enabled = false;
                }

                BtnNewInspection.Visible = false;
                break;
            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {

                }
                btnClear.Enabled = false;
                ddlLRNno.ClearDropDownList();
                ddlDocketno.ClearDropDownList();
                txtInspecDate.Text = string.Empty;
                txtNewGarageIn.Text = string.Empty;
                txtOldGarageOut.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtGarageName.Text = string.Empty;
                txtGarageAddress.Text = string.Empty;

                txtCondAsset.Attributes.Add("readonly", "true");
                break;



            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                //btnListing.Enabled = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                txtCondAsset.ReadOnly = txtRemarks.ReadOnly = true;
                BtnNewInspection.Visible = false;
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                ddlLRNno.ClearDropDownList();
                CalendarExtender1.Enabled = CalendarExtender2.Enabled = CalendarExtender3.Enabled = false;
                ddlDocketno.ClearDropDownList();
                ddlInspecBy.ClearDropDownList();
                ddlNewGarage.ClearDropDownList();
                RDInsBy.Enabled = false;
                Image1.Visible = Image2.Visible = Image3.Visible = false;
                txtInspecDate.ReadOnly = true;
                txtInspecDate.Attributes.Remove("onblur");
                txtOldGarageOut.ReadOnly = true;
                txtOldGarageOut.Attributes.Remove("onblur");
                txtNewGarageIn.ReadOnly = true;
                txtNewGarageIn.Attributes.Remove("onblur");

                break;
        }
    }

    /// <summary>
    /// to populate Garage code
    /// </summary>
    private void PopulatNewGarageCode()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //if (PageMode == PageModes.Create)
            //{
            //    Procparam.Add("@Is_Active", "1");
            //}
            Procparam.Add("@RPID", ddlDocketno.SelectedValue);


            if (ddlInspecBy.SelectedIndex > 0)
            {
                ddlNewGarage.BindDataTable("S3G_LR_GetRTrackGarageMaster", Procparam, new string[] { "Garage_ID", "Garage_Code" });
               ddlNewGarage.SelectedIndex = 0;
            }
            else
            {
                ddlNewGarage.SelectedIndex = 0;
                ddlNewGarage.ClearDropDownList();
            }
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// to populate Garadetails based on garage id
    /// </summary>
    private void PopulateGaragedetails()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Garage_ID", ddlNewGarage.SelectedValue);
            if (!ddlNewGarage.SelectedValue.Equals("0"))
                dtList = Utility.GetDefaultData("S3G_LR_GetGarageMaster", Procparam);
            else
            {
                txtGarageName.Text = string.Empty;
                txtGarageAddress.Text = string.Empty;
                rfvNewin.Enabled = false;
                rfvOldout.Enabled = false;
            }
            if (dtList.Rows.Count > 0)
            {
                txtGarageName.Text = dtList.Rows[0]["Garage_Name"].ToString();
                txtGarageAddress.Text = dtList.Rows[0]["Address"].ToString();
                rfvNewin.Enabled = true;
                rfvOldout.Enabled = true;
            }

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// TO populate ddlInspecBy Dropdown in Track details tab
    /// </summary>
    private void PopulateInspectorCode()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (RDInsBy.SelectedValue == "1")
                Procparam.Add("@Option", "T");
            else
            {
                Procparam.Add("@Option", "E");
                if (PageMode == PageModes.Create)
                {
                    Procparam.Add("@Is_Active", "1");
                }
            }
            ddlInspecBy.BindDataTable("S3G_LR_GetInspectorCode", Procparam, new string[] { "INS_ID", "User_Code" });
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// to make new inspection entry
    /// </summary>
    private void FunpriNewEntry()
    {
        try
        {
            DataTable dtHistry = new DataTable();
            if (ViewState["history"] != null)
                dtHistry = (DataTable)ViewState["history"];
            else
            {
                dtHistry.NewRow();
                dtHistry.Columns.Add("LR_Track_No");
                dtHistry.Columns.Add("Inspector_Name");
                dtHistry.Columns.Add("Inspected_Date");
                dtHistry.Columns.Add("Repossesion_Track_Date");
                dtHistry.Columns.Add("Garage_Code");
                dtHistry.Columns.Add("Address");
                dtHistry.Columns.Add("Remarks");
            }
            DataRow dr;
            dr = dtHistry.NewRow();
            dr["LR_Track_No"] = txtTrackNo.Text;
            dr["Inspector_Name"] = Convert.ToString(ViewState["Inspector"]);
            dr["Inspected_Date"] = Convert.ToString(ViewState["InsDate"]);
            dr["Repossesion_Track_Date"] = txtTrackDate.Text;
            ddlNewGarage.SelectedValue = Convert.ToString(ViewState["Garagecode"]);
            PopulateGaragedetails();
            dr["Garage_Code"] = txtGarageName.Text;
            dr["Address"] = txtGarageAddress.Text;
            dr["Remarks"] = Convert.ToString(ViewState["Remarks"]);
            dtHistry.Rows.InsertAt(dr, 0);

            // dtHistry.Select("","Inspected_Date desc");
            if (dtHistry.Rows.Count > 0)
            {
                //GrvHistory.DataSource = dtHistry;
                //GrvHistory.DataBind();
                ViewState["history"] = dtHistry;
            }
            ddlInspecBy.SelectedIndex = 0;
            ddlInspecBy.ClearDropDownList();
            RDInsBy.SelectedValue = "1";
            txtInspecDate.Text = string.Empty;
            ddlNewGarage.SelectedIndex = 0;
            ddlNewGarage.ClearDropDownList();
            txtNewGarageIn.Text = txtOldGarageOut.Text = txtRemarks.Text = txtGarageAddress.Text = txtGarageName.Text = txtCondAsset.Text = string.Empty;
            ViewState["Track_Details_ID"] = null;
            txtCondAsset.Attributes.Remove("readonly");
            rfvNewin.Enabled = false;
            rfvOldout.Enabled = false;
            PopulateInspectorCode();
            ddlInspecBy.SelectedIndex = 0;
            BtnNewInspection.Enabled = false;


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To Save Update Track details
    /// </summary>
    private void FunPriInsertTrack()
    {
        //if (ddlInspecBy.SelectedValue == "0")
        //{
        //    Utility.FunShowAlertMsg(this, "Select a Inspector Name");
        //    return;
        //}
        //if (ddlNewGarage.SelectedValue == "0")
        //{
        //    Utility.FunShowAlertMsg(this, "Select a New Garage Code");
        //    return;
        //}
        //if (string.IsNullOrEmpty(txtOldGarageOut.Text))
        //{
        //    Utility.FunShowAlertMsg(this, "Select a Old Garage Out Date");
        //    return;
        //}
        //if (string.IsNullOrEmpty(txtNewGarageIn.Text))
        //{
        //    Utility.FunShowAlertMsg(this, "Select a New Garage In Date");
        //    return;
        //}
        string strLRNDocno = string.Empty;
        if (Page.IsValid)
        {

            obj_TrackClient = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();
            try
            {
                objS3G_LR_TrackTable = new S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionTrackDataTable();
                objS3G_LR_TrackRow = objS3G_LR_TrackTable.NewS3G_LR_RepossessionTrackRow();
                objS3G_LR_TrackRow.Company_ID = intCompanyID;
                objS3G_LR_TrackRow.LR_Track_ID = strTrack;
                objS3G_LR_TrackRow.Created_By = intUserID;
                objS3G_LR_TrackRow.LRN_ID = Convert.ToInt32(ddlLRNno.SelectedValue);
                objS3G_LR_TrackRow.LRN_No = Convert.ToString(ddlLRNno.SelectedItem.Text.Substring(0, ddlLRNno.SelectedItem.Text.LastIndexOf("-")).Trim());
                objS3G_LR_TrackRow.Repossession_ID = Convert.ToInt32(ddlDocketno.SelectedValue);
                objS3G_LR_TrackRow.Repossession_Docket_No = ddlDocketno.SelectedItem.Text;
                objS3G_LR_TrackRow.Current_Garage_ID = Convert.ToInt32(txtGID.Text.Trim());
                objS3G_LR_TrackRow.Old_Garage_In = Utility.StringToDate(txtGIn.Text.Trim());
                Label lblAssetid = (Label)grvAsset.Rows[0].FindControl("lblAssetID");
                objS3G_LR_TrackRow.Asset_ID = Convert.ToInt32(lblAssetid.Text.Trim());
                Label lblAssetcode = (Label)grvAsset.Rows[0].FindControl("lblAssetCode");
                objS3G_LR_TrackRow.Asset_Code = lblAssetcode.Text.Trim();
                objS3G_LR_TrackRow.Inspection_Done_By_Type_Code = Convert.ToInt32(RDInsBy.SelectedValue);
                objS3G_LR_TrackRow.Inspector_ID = Convert.ToInt32(ddlInspecBy.SelectedValue);
                if (txtInspecDate.Text.Trim() != string.Empty)
                    objS3G_LR_TrackRow.Inspected_Date = Utility.StringToDate(txtInspecDate.Text.Trim());
                if (ViewState["Track_Details_ID"] != null)
                    objS3G_LR_TrackRow.Track_Details_ID = Convert.ToInt32(ViewState["Track_Details_ID"].ToString());
                if (!ddlNewGarage.SelectedValue.Equals("0"))
                {
                    objS3G_LR_TrackRow.New_Garage_ID = Convert.ToInt32(ddlNewGarage.SelectedValue);
                    objS3G_LR_TrackRow.Old_Garage_Out = Utility.StringToDate(txtOldGarageOut.Text.Trim());
                    objS3G_LR_TrackRow.New_Garage_In = Utility.StringToDate(txtNewGarageIn.Text.Trim());
                }
                if (txtRemarks.Text != string.Empty)
                    objS3G_LR_TrackRow.Remarks = txtRemarks.Text.Trim();
                objS3G_LR_TrackRow.Asset_Condition = txtCondAsset.Text;

                objS3G_LR_TrackTable.AddS3G_LR_RepossessionTrackRow(objS3G_LR_TrackRow);
                intErrCode = obj_TrackClient.FunPubCreateLegalRepossessionTrack(out strLRNDocno, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LR_TrackTable, ObjSerMode));
                if (intErrCode == -1)
                {
                    Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoNotDefined);
                    return;
                }
                else if (intErrCode == -2)
                {
                    Utility.FunShowAlertMsg(this, Resources.LocalizationResources.DocNoExceeds);
                    return;
                }
                if (intErrCode == 1)
                {
                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert(' Repossession track updated successfully ');" + strRedirectPageView, true);
                    //  Utility.FunShowAlertMsg(this, "Repossession track updated successfully");
                    return;
                }
                if (intErrCode == 0)
                {
                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strAlert = "Repossession track  number " + strLRNDocno.ToString() + " generated successfully";
                    strAlert += @"\n\nWould you like to add one more record? ";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Unable to save Data");
                    return;
                }

            }

            
            catch (Exception ex)
            {
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
                throw ex;
            }
            finally
            {
                //if (obj_TrackClient != null)
                    obj_TrackClient.Close();

                //(objInsuranceClient != null)
                //objInsuranceClient.Close();
            }
        }

    }

    /// <summary>
    /// This method will validate the from and to date - entered by the user.
    /// </summary>
    private bool FunPriValidateFromEndDate(String Start, String End)
    {
        bool s = false;
        DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
        dtformat.ShortDatePattern = "MM/dd/yy";

        //if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()))) &&
        if ((!(string.IsNullOrEmpty(Start))) &&
           (!(string.IsNullOrEmpty(End))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(Start) > Utility.StringToDate(End)) // start date should be less than or equal to the enddate
            {
                s = true;
            }
            else
            {
                s = false;
            }
        }
        return s;
    }
    #endregion

}
