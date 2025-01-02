    //Module Name      :   LegalRepossession
    //Screen Name      :   S3GLRSaleNotification.aspx
    //Created By       :   Irsathameen K
    //Created Date     :   27-Apr-2011
    //Purpose          :   To insert and update LegalRepossession


    #region Namespaces

using System;
using System.Web.UI;
using System.Data;
using System.Text;
using S3GBusEntity.LegalRepossession;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Resources;
using System.Xml.Linq;

#endregion

public partial class LegalRepossession_S3GLRSaleNotification : ApplyThemeForProject
{
    #region Declaration

    LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient ObjSaleNotificationClient;
    LegalRepossessionMgtServices.S3G_LR_SaleNotificationDetailsDataTable ObjS3G_LR_SaleNotificationDataTable = null;          
    SerializationMode SerMode = SerializationMode.Binary;

    int intErrCode = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int intSaleNoID;
    DateTime MDate;
    static string strPageName = "Sale Notification";
    bool status;
    string strXMLSaleBidDet = null;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    DataTable dt = null;
    Dictionary<string, string> dictBranch = null;
    Dictionary<string, string> dictLOB = null;
    Dictionary<string, string> dictParam = null;

    public static LegalRepossession_S3GLRSaleNotification obj_Page;


    string strRedirectPage = "../LegalRepossession/S3GLRTransLander.aspx?Code=GSN";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRSaleNotification.aspx';";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GSN';";

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            FunPriLoadPage(); }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load Sale Notification";
            cvCustomerMaster.IsValid = false;
        }
    }
    #endregion
    
    #region DropDown Events

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            ddlBranch.Clear();
            FunPriBindBranch();
            FunPriLoadRepossessionNo();

        }
         catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the Repossession Document Number";
            cvCustomerMaster.IsValid = false;
        }      
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            FunPriLoadRepossessionNo();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the Repossession Document Number";
            cvCustomerMaster.IsValid = false;
        }
    }

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
        Procparam.Add("@Program_Id", "141");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }


    protected void ddlrepdocno_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            //Repossession Document Number
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));            
            dictParam.Add("@ReposssNo ", Convert.ToString(ddlrepdocno.SelectedValue));
            dictParam.Add("@User_ID", Convert.ToString(intUserID));
            DS = Utility.GetDataset(SPNames.S3G_LR_GetRepDocDetails, dictParam);

            // Prime & Sub A/c Number,Customer Details
            if (DS.Tables[0].Rows.Count >= 1)
            {
                txtPAN.Text = DS.Tables[0].Rows[0]["PANUM"].ToString();
              if(DS.Tables[0].Rows[0]["SANUM"].ToString().Contains("DUMMY"))
                txtSAN.Text = string.Empty;
                else
                txtSAN.Text = DS.Tables[0].Rows[0]["SANUM"].ToString();
                hidcuscode.Value = DS.Tables[0].Rows[0]["Customer_ID"].ToString();
                S3GCustomerAddress1.SetCustomerDetails(DS.Tables[0].Rows[0], true);
            }
            else if (DS.Tables[0].Rows.Count == 0)
            {
                txtPAN.Text = txtSAN.Text = hidcuscode.Value = string.Empty;
                S3GCustomerAddress1.ClearCustomerDetails();
            }
            //Repossession Asset Details
            if (DS.Tables[1].Rows.Count >= 1)
            {
                PNLAssetDetails.Visible = true;
                GRVAssetDetails.DataSource = DS.Tables[1];
                GRVAssetDetails.DataBind();
            }
            else if (DS.Tables[1].Rows.Count ==0)
            {
                PNLAssetDetails.Visible = false;
                GRVAssetDetails.DataSource = null;
                GRVAssetDetails.DataBind();

            }

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }

    }

    #endregion

    #region Button Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        { FunPriSaveSaleNotificationDetails(); }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }
    protected void btnClear_Click(object sender, System.EventArgs e)
    {
        try
        {
            //if (ddlBranch.Items.Count > 0)
            //{ ddlBranch.SelectedIndex = 0; }
            if (ddlLOB.Items.Count > 0)
            { ddlLOB.SelectedIndex = 0; }
            if (ddlrepdocno.Items.Count > 0)
                ddlrepdocno.SelectedIndex = 0;
            FunPriClearControls();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Clear the Controls";
            cvCustomerMaster.IsValid = false;
        }

    }
    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        try
        {
            Response.Redirect("../LegalRepossession/S3GLRTransLander.aspx?Code=GSN");
        }
        catch (Exception ex)
        {
            throw ex;
        }

       // try
       // {
       //     Response.Redirect("../LegalRepossession/S3GLRTransLander.aspx?Code=GSN");
       //     //Response.Redirect(strRedirectPage);
       // }
       // catch (Exception objException)
       //{
       //       ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
       //     cvCustomerMaster.ErrorMessage = "Unable to Redirect the Page";
       //     cvCustomerMaster.IsValid = false;
       // }
    }

    #endregion

    #region Gridview Events

    protected void GRVSaleBid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FunPriGridviewRowcommand(e);   

    }    
    protected void GRVSaleBid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunPriGridviewRowDeleting(e);
    }
    protected void GRVSaleBid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbldataofpub = (Label)e.Row.FindControl("lbldataofpub");
                Label lblFloorPrice = (Label)e.Row.FindControl("lblFloorPrice");
              
                //if (lbldataofpub.Text.Trim() != string.Empty)
                //{
                //    DateTime Date = Convert.ToDateTime(lbldataofpub.Text);
                //    lbldataofpub.Text = Date.ToString(strDateFormat);
                // }
               
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                S3GSession ObjS3GSession = new S3GSession();
                AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupDate = e.Row.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
                CalendarExtenderSD_FollowupDate.Format = ObjS3GSession.ProDateFormatRW;

                TextBox txtFloorPrice = (TextBox)e.Row.FindControl("txtFloorPrice");
                txtFloorPrice.SetDecimalPrefixSuffix(14, 4, true, "Floor Price");

                TextBox txtdataofpub = (TextBox)e.Row.FindControl("txtdataofpub");
                
                txtdataofpub.Attributes.Add("onblur", "fnDoDate(this,'" + txtdataofpub.ClientID + "','" + strDateFormat + "',false,  true);");
                
                if (PageMode == PageModes.Query)
                {
                    txtdataofpub.Attributes.Add("readonly", "true");
                    txtdataofpub.Attributes.Remove("onblur");
                }
            }
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
        
    }
    protected void GRVSaleBid_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {

            S3GSession ObjS3GSession = new S3GSession();
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupDate = e.Row.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_FollowupDate.Format = ObjS3GSession.ProDateFormatRW;

            TextBox txtFloorPrice = e.Row.FindControl("txtFloorPrice") as TextBox;
            txtFloorPrice.Style.Add("text-align", "right");
        }
    }
    protected void GRVAssetDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRepDate = (Label)e.Row.FindControl("lblRepDate");
                //if (lblRepDate.Text.Trim() != string.Empty)
                //{
                //    DateTime Date = Convert.ToDateTime(lblRepDate.Text);
                //    lblRepDate.Text = Date.ToString(strDateFormat);
                //}
            }
            
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }

    }

    #endregion


    #region User Defined Functions

    private void FunPriLoadPage()
    {
        try
        {
            // this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            //Begin
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
          
            //End
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                { intSaleNoID = Convert.ToInt32(fromTicket.Name); }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

           
            
            if (!IsPostBack)
            {

                txtSNDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                FunPriBindBranchLOB();
                FunPriInitialRow();
                //User Authorization            
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                if ((intSaleNoID > 0) && (strMode == "Q")) // Query MOde
                {
                    FunPriGetSaleNotificationDetails();
                    FunPriDisableControls(1);
                }
                else if ((intSaleNoID > 0) && (strMode == "M")) // Modify MOde
                {
                    FunPriGetSaleNotificationDetails();
                    FunPriDisableControls(2);
                }
                else  //Create Mode
                { FunPriDisableControls(0); }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriInitialRow()
    {
        try
        {
            dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SaleBidpub");
            dt.Columns.Add("dataofpub");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("FloorPrice");

            dr = dt.NewRow();
            dr["SaleBidpub"] = string.Empty;
            dr["dataofpub"] = string.Empty;
            dr["Remarks"] = string.Empty;
            dr["FloorPrice"] = string.Empty;
            dt.Rows.Add(dr);
            GRVSaleBid.DataSource = dt;
            GRVSaleBid.DataBind();
            GRVSaleBid.Rows[0].Visible = false;
            ViewState["currenttable"] = dt;

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriBindBranchLOB()
    {
        try
        {

            dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictLOB.Add("@User_ID", Convert.ToString(intUserID));
            dictLOB.Add("@Is_Active", "1");
            dictLOB.Add("@Program_ID", "149");
            ddlLOB.BindDataTable(SPNames.LOBMaster, dictLOB, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        
       
        
    }
    public void FunPriBindBranch()
    {
        //Branch
        dictBranch = new Dictionary<string, string>();
        dictBranch.Add("@Company_ID", Convert.ToString(intCompanyID));
        dictBranch.Add("@Is_Active", "1");
        dictBranch.Add("@User_ID", Convert.ToString(intUserID));
        dictBranch.Add("@Program_ID", "149");
        dictBranch.Add("@LOB_ID", ddlLOB.SelectedValue);
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictBranch, true, "-- Select --", new string[] { "Location_ID", "Location" });
    }
    private void FunPriLoadRepossessionNo()
    {
        try
        {
            //Repossession Document Number
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            dictParam.Add("@User_ID", Convert.ToString(intUserID));
            ddlrepdocno.BindDataTable(SPNames.S3G_LR_GetRepossessionDocNo, dictParam, new string[] { "Repossession_Docket_No_Value", "Repossession_Docket_No_Text" });
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }

    }       
    private void FunPriSaveSaleNotificationDetails()
    {
        ObjSaleNotificationClient = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();

        try
        {
            string strSNNo = "";

            dt = (DataTable)ViewState["currenttable"];
            if (dt.Rows[0]["SaleBidpub"].ToString() == "" && dt.Rows[0]["dataofpub"].ToString() == "" && dt.Rows[0]["Remarks"].ToString() == "" && dt.Rows[0]["FloorPrice"].ToString() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Add one row in Sale Notification details");
                return;
            }


            ObjS3G_LR_SaleNotificationDataTable = new LegalRepossessionMgtServices.S3G_LR_SaleNotificationDetailsDataTable();
            LegalRepossessionMgtServices.S3G_LR_SaleNotificationDetailsRow ObjSaleNotificationRow;

            ObjSaleNotificationRow = ObjS3G_LR_SaleNotificationDataTable.NewS3G_LR_SaleNotificationDetailsRow();
            ObjSaleNotificationRow.Company_ID = intCompanyID;
            ObjSaleNotificationRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjSaleNotificationRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjSaleNotificationRow.SNDate = Utility.StringToDate(txtSNDate.Text);
            ObjSaleNotificationRow.PANum = Convert.ToString(txtPAN.Text);
           if(txtSAN.Text==string.Empty)
               ObjSaleNotificationRow.SANum = Convert.ToString(txtPAN.Text) + "DUMMY";
           else
               ObjSaleNotificationRow.SANum = Convert.ToString(txtSAN.Text);
            ObjSaleNotificationRow.Customer_ID = Convert.ToInt32(hidcuscode.Value);//hidden Customer_ID;    
            ObjSaleNotificationRow.Is_Active = chkActive.Checked;
            ObjSaleNotificationRow.RepossNo = Convert.ToString(ddlrepdocno.SelectedValue);
            FunPriGenerateSaleBidXMLDetails();
            ObjSaleNotificationRow.XMLSaleBidDetails = strXMLSaleBidDet;
            if (strMode == "M")
                ObjSaleNotificationRow.SN_ID = intSaleNoID;
            else
                ObjSaleNotificationRow.SN_ID = 0;
            ObjSaleNotificationRow.Created_By = intUserID;
            ObjS3G_LR_SaleNotificationDataTable.AddS3G_LR_SaleNotificationDetailsRow(ObjSaleNotificationRow);
           
            intErrCode = ObjSaleNotificationClient.FunPubCreateSaleNotificationDetails(out strSNNo, SerMode, ClsPubSerialize.Serialize(ObjS3G_LR_SaleNotificationDataTable, SerMode));
            
            if (intErrCode == 0)
            {
                //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intSaleNoID > 0)                                                            
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('"+ "Sale Notification details updated successfully - " + txtSNNo.Text + "');" + strRedirectPageView, true);                
                else
                {
                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                txtSNNo.Text = strSNNo;
                strAlert = "Sale Notification details added successfully - " + strSNNo;
                strAlert += @"\n\nWould you like to add one more Sale Notification?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";                
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
                return;
                }
            }
           
            else if (intErrCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                return;
            }
            else if (intErrCode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                return;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            lblErrorMessage.Text = "Unable Save the  Sale Notification Details";
            cvCustomerMaster.IsValid = false;
        }
        finally
        {
            //if (ObjSaleNotificationClient != null)
                ObjSaleNotificationClient.Close();
        }
    }
    private void FunPriGenerateSaleBidXMLDetails()
    {
        
        try
        { strXMLSaleBidDet = GRVSaleBid.FunPubFormXml(); }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    chkActive.Enabled = false;   
                    break;
                case 1:// Query Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    if (ddlLOB.Items.Count != 0)
                    { ddlLOB.ClearDropDownList(); }
                    ddlBranch.Enabled = false;
                    //if (ddlBranch.Items.Count != 0)
                    //{ ddlBranch.ClearDropDownList(); }
                    if (ddlrepdocno.Items.Count != 0)
                    { ddlrepdocno.ClearDropDownList(); }
                    chkActive.Enabled = btnSave.Enabled = btnClear.Enabled = GRVSaleBid.FooterRow.Visible = GRVSaleBid.Columns[5].Visible = false;                     
                    break;
                case 2:// Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (ddlLOB.Items.Count != 0)
                    { ddlLOB.ClearDropDownList(); }
                    ddlBranch.Enabled = false;

                    //if (ddlBranch.Items.Count != 0)
                    //{ ddlBranch.ClearDropDownList(); }
                    if (ddlrepdocno.Items.Count != 0)
                    { ddlrepdocno.ClearDropDownList(); }
                    btnClear.Enabled = false;
                    break;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriGetSaleNotificationDetails()
    {
        try
        {
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@SN_ID",intSaleNoID.ToString());
            dictParam.Add("@Program_ID", "149");
            dictParam.Add("@User_ID", intUserID.ToString());
            DS = Utility.GetDataset(SPNames.S3G_LR_GetSaleNotificationDetails, dictParam);
            
            // Table 0  Lob,Branch,Prime Account Number,Sub Account Number,and more
            if (DS.Tables[0].Rows.Count >= 1)
            {
                ddlLOB.Items[0].Text = Convert.ToString(DS.Tables[0].Rows[0]["LOBName"]);
                ddlLOB.Items[0].Value = Convert.ToString(DS.Tables[0].Rows[0]["LOB_ID"]);
                ddlLOB.Items[0].Selected =true;
                FunPriBindBranch();
                ddlBranch.SelectedText = Convert.ToString(DS.Tables[0].Rows[0]["LocationName"]);
                ddlBranch.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["Location_ID"]);
                FunPriLoadRepossessionNo();
                ddlrepdocno.Items[0].Text = Convert.ToString(DS.Tables[0].Rows[0]["RepossNo_Text"]);
                ddlrepdocno.Items[0].Value = Convert.ToString(DS.Tables[0].Rows[0]["RepossNo_Value"]);
                txtPAN.Text=Convert.ToString(DS.Tables[0].Rows[0]["PANUM"]);;
                if (Convert.ToString(DS.Tables[0].Rows[0]["SANUM"]).Contains("DUMMY"))
                    txtSAN.Text = string.Empty;
                else
                txtSAN.Text = Convert.ToString(DS.Tables[0].Rows[0]["SANUM"]);
                txtSNNo.Text = Convert.ToString(DS.Tables[0].Rows[0]["SN_Number"]);
                //DateTime Date = Convert.ToDateTime(DS.Tables[0].Rows[0]["SN_Date"]);
                //txtSNDate.Text = Date.ToString(strDateFormat);
                
                //ponnurajesh//
                txtSNDate.Text = DS.Tables[0].Rows[0]["SN_Date"].ToString();
                chkActive.Checked = Convert.ToBoolean(DS.Tables[0].Rows[0]["Is_Active"]);
                hidcuscode.Value = Convert.ToString(DS.Tables[0].Rows[0]["Customer_ID"]);
                S3GCustomerAddress1.SetCustomerDetails(DS.Tables[0].Rows[0], true);
            }

            // Table 1  Asset Details
            if (DS.Tables[1].Rows.Count >= 1)
            {
                PNLAssetDetails.Visible = true;
                GRVAssetDetails.DataSource = DS.Tables[1];
                GRVAssetDetails.DataBind();
            }
            // Table 2  Sale Bid Details
            if (DS.Tables[2].Rows.Count >= 1)
            {                
                GRVSaleBid.DataSource = DS.Tables[2];
                GRVSaleBid.DataBind();
                ViewState["currenttable"] = DS.Tables[2];
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriGridviewRowDeleting(GridViewDeleteEventArgs e)
    {
        try
        { 
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["currenttable"];
            if (PageMode == PageModes.Modify && dtDelete.Rows.Count == 1)
            {
                Utility.FunShowAlertMsg(this, "Atleast one sale notification details required");
                return;
            }
            dtDelete.Rows.RemoveAt(e.RowIndex);
            GRVSaleBid.DataSource = dtDelete;
            GRVSaleBid.DataBind();
            ViewState["currenttable"] = dtDelete;

            if (dtDelete.Rows.Count == 0)
            { FunPriInitialRow(); }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriGridviewRowcommand(GridViewCommandEventArgs e)
    {
      

        try
        {
            DataRow dr;
                Label txtMarketvalue = null ;

            foreach (GridViewRow grvData in GRVAssetDetails.Rows)
            {
                txtMarketvalue = ((Label)grvData.FindControl("txtMarketvalue"));
            }

             if (e.CommandName == "AddNew")
            {
                dt = (DataTable)ViewState["currenttable"];

                TextBox txtSaleBidpub = (TextBox)GRVSaleBid.FooterRow.FindControl("txtSaleBidpub");
                TextBox txtdataofpub = (TextBox)GRVSaleBid.FooterRow.FindControl("txtdataofpub");
                TextBox txtRemarks = (TextBox)GRVSaleBid.FooterRow.FindControl("txtRemarks");
                TextBox txtFloorPrice = (TextBox)GRVSaleBid.FooterRow.FindControl("txtFloorPrice");

              

                if (Convert.ToDecimal(txtFloorPrice.Text) > Convert.ToDecimal(txtMarketvalue.Text))
                {
                    Utility.FunShowAlertMsg(this.Page, "Floor Price Should not be greater than Latest Market Value");
                    return;
                }


                //Double dcmAmount = 0.0;

                //foreach (GridViewRow grvData in GRVSaleBid.Rows)
                //{
                //    if (grvData.RowType == DataControlRowType.DataRow)
                //    {
                //        Label lblFloorPrice = (Label)grvData.FindControl("lblFloorPrice");
                //        if (lblFloorPrice != null)
                //        {
                //            if (lblFloorPrice.Text != "")
                //            {
                //                dcmAmount = dcmAmount + Convert.ToDouble(lblFloorPrice.Text);
                //            }
                //        }
                //    }
                //}
                //dcmAmount = dcmAmount + Convert.ToDouble(txtFloorPrice.Text);
                //if (dcmAmount > Convert.ToDouble(txtMarketvalue.Text))
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Floor Price Should not be greater than Latest Market Value");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SaleBidpub"].ToString() == "" && dt.Rows[0]["dataofpub"].ToString() == "" && dt.Rows[0]["Remarks"].ToString() == "" && dt.Rows[0]["FloorPrice"].ToString() == "")
                    { dt.Rows[0].Delete(); }
                }

                if (txtSaleBidpub.Text == "" && txtdataofpub.Text == "" && txtRemarks.Text == "" && txtFloorPrice.Text == "")
                {
                    Utility.FunShowAlertMsg(this.Page, "Add one row in Sale Notification details");
                    return;
                }
                dr = dt.NewRow();

                dr["SaleBidpub"] = txtSaleBidpub.Text.Trim();
                //if(txtdataofpub.Text.Trim()!=string.Empty)
                //dr["dataofpub"] = Utility.StringToDate(txtdataofpub.Text.Trim());
                dr["dataofpub"] = txtdataofpub.Text.Trim();
                dr["Remarks"] = txtRemarks.Text.Trim();
                dr["FloorPrice"] = txtFloorPrice.Text.Trim();

                dt.Rows.Add(dr);
                GRVSaleBid.DataSource = dt;
                GRVSaleBid.DataBind();
                ViewState["currenttable"] = dt;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriClearControls()
    {
        try
        {
            
            txtSAN.Text = txtPAN.Text = txtSNNo.Text = string.Empty;
            PNLAssetDetails.Visible = false;
            GRVAssetDetails.DataSource = null;
            GRVAssetDetails.DataBind();
            GRVSaleBid.DataSource = null;
            GRVSaleBid.DataBind();
            FunPriInitialRow();
            S3GCustomerAddress1.ClearCustomerDetails();            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
        
    #endregion
     
}
