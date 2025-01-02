#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Legal & Repossession
/// Screen Name			: Garage Master
/// Created By			: Srivatsan S
/// Created Date		: 21-April-2011
/// Purpose	            : This module is used to store and retrieve Garage details
///<Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using LEGAL = S3GBusEntity.LegalRepossession;
using LEGALSERVICES = LegalAndRepossessionMgtServicesReference;
using System.Collections;
using System.Configuration;
using System.Globalization;
#endregion

public partial class LegalRepossession_S3GLRGarage_Maint_Add : ApplyThemeForProject
{

    #region Initialization
    enum AccountMode { ALL, CURRENT };

    int intCompanyId = 0;
    static int intCustomer = 0;
    int intUserId = 0;
    int intGarageId = 0;
    int intGARAGEMAINTNO = 0;
    int intErrorCode = 0;
    int intGarageOwnerID = 0;
    int intGarage_Maint_ID = 0;
    string strGarageCode = string.Empty;
    string strGarageOwnerCode = string.Empty;
    string strErrorMessage = string.Empty;
    string strDateFormat = string.Empty;

    string strMaint = string.Empty;
    bool blnTranExists = false;
    string strPageName = "Garage Maintenance";
    Dictionary<string, string> Procparam = null;
    LEGAL.LegalRepossessionMgtServices.S3G_LEGAL_Garage_MasterDataTable ObjGarageMasterDataTable;
    LEGALSERVICES.LegalAndRepossessionMgtServicesClient ObjLegalMgtServicesClient;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjOrgMasterMgtServicesClient;
    UserInfo ObjUserInfo;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRGarage_Maint_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GDM';";
    string strRedirectPage = "~/LegalRepossession/S3GLoanAdTransLander.aspx?Code=GDM";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    S3GSession ObjS3GSession;
    //Code end
    #endregion
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ObjS3GSession = new S3GSession();
            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            
            intUserId = ObjUserInfo.ProUserIdRW;
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            //Code end

            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intGarage_Maint_ID = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Garage Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }

            }
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            this.TxtRntSqFeet.Attributes.Add("onkeyup", "CalcTotrent()");
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            if (!IsPostBack)
            {
                txtGarageMaintDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);

                Session["Garage_Code"] = "";
                FunPriBindGarageDetail("Add");


                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                ddlGarageOwnerCode.BindDataTable("S3G_LR_GetGarageOwner", Procparam, new string[] { "Garage_ID", "Garage_Code" });
                Procparam.Clear();

                Procparam.Add("@Company_ID", intCompanyId.ToString());

                //S3GSession ObjS3GSession = new S3GSession();

                PopulateLOBList();


                if (intGarage_Maint_ID > 0)
                {
                    bool blnTranExists;
                    
                    FunPubProGetGarageDetails();
                   


                    if (strMode == "M")
                    {
                        FunPriEntityControlStatus(1);

                    }
                    if (strMode == "Q")
                    {
                        FunPriEntityControlStatus(-1);
                    }
                }
                else
                {
                    FunPriEntityControlStatus(0);
                    //TxtGrgName.Focus();
                }
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Attributes.Add("onfocus", "fnLoadCustomer();");

            }
        }
        catch (Exception ex)
        {
            cvGarageMaster.IsValid = false;
            cvGarageMaster.ErrorMessage = "Unable to load garage details due to data problem";
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }
    #endregion



    protected void FunProLoadAddressCombos()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);

            DataTable dtSource = new DataTable();
            if (dtAddr.Select("Category = 1").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            //txtInsCompanyCity.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 2").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 2").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            //txtInsCompanyState.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            //txtInsCompanyCountry.FillDataTable(dtSource, "Name", "Name", false);

        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }
    }

    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("Category");

        return dt;
    }
    private void PopulateLOBList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@User_Id", Convert.ToString(intUserId));
            Procparam.Add("@FilterOption", "'HP','LN','OL','FL'");
            Procparam.Add("@Program_ID", "245");
            if (PageMode == PageModes.Create)
                Procparam.Add("@Is_UserLobActive", "1");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.Items.RemoveAt(0);
                ddlLOB.SelectedIndex = 0;

                //PopulatePANum();


            }
            PopulateBranchList();


        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }
    }

    private void PopulateBranchList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Program_ID", "245");
            if (ddlLOB.SelectedIndex > 0)
            {
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            }
            if (PageMode == PageModes.Create)
                Procparam.Add("@Is_Active", "1");

            ddlBranch.BindDataTable("S3G_Get_Branch_List", Procparam, new string[] { "Location_ID", "Location" });
        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }



    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ddlGarageOwnerCode.Items.Clear();
            //  FunPriClear();
            ddlLOB.Focus();

            PopulateBranchList();
            ddlBranch.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            ddlGarageOwnerCode.Items.Clear();
            //FunPriClear();

            ddlBranch.Focus();
        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }
    }

    protected void ddlGarageOwnerCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //FunPriClear();

            if (ddlGarageOwnerCode.SelectedValue == "0")
            {
                return;
            }

            DataSet dsAccountDetails;

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));

            Procparam.Add("@Garage_ID", Convert.ToString(ddlGarageOwnerCode.SelectedValue));
            dsAccountDetails = Utility.GetDataset("S3G_LR_GetGarage_Maint_Details", Procparam);
            if (dsAccountDetails.Tables[0].Rows.Count > 0)
            {
                DataTable dtAccountDetails = dsAccountDetails.Tables[0];
                TxtGrgAddress1.Text = dtAccountDetails.Rows[0]["Garage_Address1"].ToString();
                TxtSqFeet.Text = dtAccountDetails.Rows[0]["Square_Feet"].ToString();
                TxtPlcyAmnt.Text = dtAccountDetails.Rows[0]["Policy_Amount"].ToString();
                txtPolicyNo.Text = dtAccountDetails.Rows[0]["Policy_No"].ToString();
                TxtRntSqFeet.Text = dtAccountDetails.Rows[0]["Rent_Per_SqFeet"].ToString();
                TxtSqFeet.Text = dtAccountDetails.Rows[0]["Square_Feet"].ToString();
                TxtGrgCpcty.Text = dtAccountDetails.Rows[0]["No_Of_Assets_Garage"].ToString();
                txtInscompany.Text = dtAccountDetails.Rows[0]["Insurance_Company"].ToString();
                txtPymntFreq.Text = dtAccountDetails.Rows[0]["PaymentFrequency"].ToString();
                TxtTotRent.Text = dtAccountDetails.Rows[0]["total_rent"].ToString();
                RdbCvrPrk.SelectedValue = dtAccountDetails.Rows[0]["Is_Covered_Parking"].ToString();
            }


        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }

    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (PageMode != PageModes.Query)
            {
                hdnCustomerID.Value = "";
                S3GCustomerAddress1.ClearCustomerDetails();
                HiddenField hdnCID = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                if (hdnCID != null && hdnCID.Value != "")
                {
                    FunPubQueryExistCustomerList(Convert.ToInt32(hdnCID.Value));
                    hdnCustomerID.Value = hdnCID.Value.ToString();

                    DropDownList ddlPANum = gvGarageMaint.FooterRow.FindControl("ddlPANum") as DropDownList;
                    FunProLoadPrimAccounts(ddlPANum);
                }
            }
        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }
    }

    #region [CustomerAddress]
    public static string SetCustomerAddress(string Address1, string Address2, string City, string State, string Country, string Pincode)
    {
        try
        {
            string strAddress = "";
            if (Address1.ToString() != "") strAddress += Address1.ToString() + System.Environment.NewLine;
            if (Address2.ToString() != "") strAddress += Address2.ToString() + System.Environment.NewLine;
            if (City.ToString() != "") strAddress += City.ToString() + System.Environment.NewLine;
            if (State.ToString() != "") strAddress += State.ToString() + System.Environment.NewLine;
            if (Country.ToString() != "") strAddress += Country.ToString() + System.Environment.NewLine;
            if (Pincode.ToString() != "") strAddress += Pincode.ToString();
            return strAddress;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region [CustomerDetails]
    private void FunPubQueryExistCustomerList(int CustomerID)
    {
        string strCustomerAddress = "";

        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Customer_Id", CustomerID.ToString());
            DataTable dtCustomerDetails = Utility.GetDefaultData("S3G_CLN_GetCustomerDetails", Procparam);
            if (dtCustomerDetails.Rows.Count > 0)
            {
                if (intGarageId > 0)
                {
                    TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    txtName.Text = dtCustomerDetails.Rows[0]["Title"].ToString() + ' ' + dtCustomerDetails.Rows[0]["Customer_Name"].ToString();
                    txtName.Enabled = false;
                    txtName.ToolTip = txtName.Text;
                    Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
                    btnGetLOV.Visible = false;
                }
                strCustomerAddress = SetCustomerAddress(dtCustomerDetails.Rows[0]["comm_address1"].ToString(), dtCustomerDetails.Rows[0]["comm_address2"].ToString(), dtCustomerDetails.Rows[0]["comm_city"].ToString(), dtCustomerDetails.Rows[0]["comm_state"].ToString(), dtCustomerDetails.Rows[0]["comm_country"].ToString(), dtCustomerDetails.Rows[0]["comm_pincode"].ToString());
                S3GCustomerAddress1.SetCustomerDetails(dtCustomerDetails.Rows[0]["Customer_Code"].ToString(),
                    strCustomerAddress, dtCustomerDetails.Rows[0]["Customer_Name"].ToString(),
                     dtCustomerDetails.Rows[0]["Comm_Telephone"].ToString(),
                    dtCustomerDetails.Rows[0]["comm_mobile"].ToString(),
                    dtCustomerDetails.Rows[0]["comm_email"].ToString(),
                    dtCustomerDetails.Rows[0]["Comm_Website"].ToString());

            }
        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }



    }
    #endregion

    #region [ClearCustomerDetails]
    private void FunPriClearCusdetails()
    {
        try
        {
            S3GCustomerAddress1.ClearCustomerDetails();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    public void FunPubProGetGarageDetails()
    {
        try
        {

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Garage_Maint_ID", intGarage_Maint_ID.ToString());
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            DataSet dset = Utility.GetDataset("S3G_LR_GetGarageMaintanance", Procparam);

            if (dset != null)
            {

                ddlGarageOwnerCode.SelectedValue = dset.Tables[0].Rows[0]["Garage_ID"].ToString();
                ddlGarageOwnerCode_SelectedIndexChanged(null, null);
                ddlLOB.SelectedValue = dset.Tables[0].Rows[0]["LOB_ID"].ToString();
                ddlBranch.SelectedValue = dset.Tables[0].Rows[0]["Location_ID"].ToString();

                hdnCustomerID.Value = dset.Tables[0].Rows[0]["Customer_ID"].ToString();
                FunPubQueryExistCustomerList(Convert.ToInt32(hdnCustomerID.Value));

                TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txt.Text = dset.Tables[2].Rows[0]["Customer_Name"].ToString();
                S3GCustomerAddress1.SetCustomerDetails(dset.Tables[2].Rows[0], true);
                txtMaintDocNo.Text = dset.Tables[0].Rows[0]["GARAGEMAINTNO"].ToString();
                txtGarageMaintDate.Text = dset.Tables[0].Rows[0]["Created_On"].ToString();
                for (int i = 0; i <= dset.Tables[1].Rows.Count - 1; i++)
                {
                    dset.Tables[1].Rows[i]["Rental"] = (Convert.ToDecimal(dset.Tables[1].Rows[i]["Area"].ToString()) * Convert.ToDecimal(TxtRntSqFeet.Text)).ToString();
                }
                dset.Tables[1].AcceptChanges();


                ViewState["DtGarageDetails"] = gvGarageMaint.DataSource = dset.Tables[1];
                gvGarageMaint.DataBind();



            }


        }
        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }
    }

    public string ReturnFormattedDecimalValue(string DecimalVal)
    {
        bool IsGreater = false;
        string[] digits = new string[2];
        digits = DecimalVal.Split('.');
        if (Convert.ToInt32(digits[1].ToString()) > 0)
        {
            IsGreater = true;
        }

        if (IsGreater == true)
        {
            return DecimalVal;
        }
        else
        {
            return digits[0];
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        if (txtName != null)
        {
            if (txtName.Text == "")
            {
                strAlert = strAlert.Replace("__ALERT__", "Select a Customer");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                return;
            }
        }

        ObjLegalMgtServicesClient = new LEGALSERVICES.LegalAndRepossessionMgtServicesClient();
        int intResult = 0;
        try
        {
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_Garage_MaintDataTable objGarageMainTable = new S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_Garage_MaintDataTable();
            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_Garage_MaintRow objGarageMainRow = objGarageMainTable.NewS3G_LR_Garage_MaintRow();


            //Garage ID and garage Owner Code...............
            objGarageMainRow.Garage_Maint_ID = intGarage_Maint_ID;
            objGarageMainRow.Garage_ID = Convert.ToInt32(ddlGarageOwnerCode.SelectedValue);
            objGarageMainRow.Company_ID = intCompanyId;
            objGarageMainRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objGarageMainRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            if (intCustomer > 0)
                objGarageMainRow.Customer_ID = intCustomer;

            else
                objGarageMainRow.Customer_ID = Convert.ToInt32(hdnCustomerID.Value);
            objGarageMainRow.UserId = intUserId.ToString();

            DataTable DtGarageDetails = (DataTable)ViewState["DtGarageDetails"];
            objGarageMainRow.XmlGarageDetails = DtGarageDetails.FunPubFormXml();
            objGarageMainTable.AddS3G_LR_Garage_MaintRow(objGarageMainRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] objByteGarageMainTable = ClsPubSerialize.Serialize(objGarageMainTable, SerMode);

            intErrorCode = ObjLegalMgtServicesClient.FunCreateMaintenance(out strMaint, SerMode, ClsPubSerialize.Serialize(objGarageMainTable, SerMode));
            if (intErrorCode == -1)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                return;
            }
            else if (intErrorCode == -2)
            {
                strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                return;
            }
            else if (intErrorCode == 3)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Selected asset has already Repossessed');" + strRedirectPageView, true);
            }

            if (intErrorCode == 50)
            {
                //cvRepossession.ErrorMessage = "Error in Saving";
                //cvRepossession.IsValid = false;
                return;
            }



            if (strMode == "M")
            {
                //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Garage Details updated successfully');" + strRedirectPageView, true);

            }

            else
            {
                //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                strAlert = "Garage Maintenance " + strMaint + " added successfully";
                strAlert += @"\n\nWould you like to add one more record?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }


            //return;
        }


        catch (Exception ex)
        {
            cvGarageMaster.ErrorMessage = ex.Message;
            cvGarageMaster.IsValid = false;
            return;

        }
        finally
        {
            if (ObjLegalMgtServicesClient != null)
                ObjLegalMgtServicesClient.Close();
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {

            ddlGarageOwnerCode.SelectedIndex = 0;
            ddlLOB.SelectedValue = ddlBranch.SelectedValue = "0";
            TxtGrgAddress1.Text = "";
            ucCustomerCodeLov.FunPubClearControlValue();
            S3GCustomerAddress1.ClearCustomerDetails();
            RdbCvrPrk.ClearSelection();
            TxtRntSqFeet.Text = "";
            TxtSqFeet.Text = "";
            TxtTotRent.Text = TxtPlcyAmnt.Text = txtPolicyNo.Text = txtPymntFreq.Text = TxtGrgCpcty.Text = txtInscompany.Text = "";
            FunPriBindGarageDetail("Add");
        }

            //gvGarageMaint.DataSource = null;
        //gvGarageMaint.DataBind();




        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvGarageMaster.IsValid = false;
            cvGarageMaster.ErrorMessage = "Unable to clear data";
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/LegalRepossession/S3GLRTransLander.aspx?Code=GDM");
    }



    private void FunPriEntityControlStatus(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);



                ddlGarageOwnerCode.Enabled = true;

                btnClear.Enabled = true;
                btnSave.Enabled = true;


                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                break;

            case 1: //Modify


                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                ddlGarageOwnerCode.Enabled = false;
                btnClear.Enabled = false;
                if (!bModify)
                {
                    btnSave.Enabled = false;

                }

                break;
            case -1://Query

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                ddlGarageOwnerCode.Enabled = false;
                ddlLOB.Enabled = false;
                ddlBranch.Enabled = false;
                TxtGrgAddress1.ReadOnly = true;
                FunPubProGetGarageDetails();
                gvGarageMaint.FooterRow.Visible = false;

                btnClear.Enabled = false;
                btnSave.Enabled = false;
                RdbCvrPrk.Enabled = false;

                ucCustomerCodeLov.ButtonEnabled = false;


                gvGarageMaint.Columns[1].Visible = true;

                gvGarageMaint.Columns[11].Visible = false;






                break;
        }
    }
    public string FunWrapText(string strWarptext, int intWraplength)
    {
        string strWarppedtext = string.Empty;
        if (strWarptext.Length > 0)
        {
            int intcharlength = 1;
            foreach (char chr in strWarptext)
            {

                if ((intcharlength % intWraplength) == 0)
                {
                    if (intcharlength > 0)
                        strWarppedtext += chr.ToString() + System.Environment.NewLine;
                    else
                        strWarppedtext += chr.ToString();
                }
                else
                {
                    strWarppedtext += chr.ToString();
                }


                intcharlength++;
            }
        }
        return strWarppedtext;
    }

    protected string GVDateFormat(string val)
    {
        if (!string.IsNullOrEmpty(val))
        {
            return Utility.StringToDate(val).ToString(strDateFormat);
        }
        else
        {
            return "";
        }
    }

    private void FunPriBindGarageDetail(string Mode)
    {
        try
        {
            if (Mode == "Add")
            {
                DataTable ObjDT = new DataTable();
                ObjDT.Columns.Add("AssetDescription");
                ObjDT.Columns.Add("AssetID");
                ObjDT.Columns.Add("AssetNumber");
                ObjDT.Columns.Add("DetailsID");
                ObjDT.Columns.Add("MachineNo");
                ObjDT.Columns.Add("InDate");
                ObjDT.Columns.Add("OutDate");
                ObjDT.Columns.Add("Area");
                ObjDT.Columns.Add("Rental");
                ObjDT.Columns.Add("Cust");
                ObjDT.Columns.Add("PANUM");
                ObjDT.Columns.Add("SANUM");

                DataRow dr_Alert = ObjDT.NewRow();
                dr_Alert["AssetDescription"] = "";
                dr_Alert["AssetID"] = "";
                dr_Alert["AssetNumber"] = "";
                dr_Alert["DetailsID"] = "";
                dr_Alert["MachineNo"] = "";
                dr_Alert["InDate"] = "";
                dr_Alert["OutDate"] = "";
                dr_Alert["Area"] = "";
                dr_Alert["Rental"] = "";
                dr_Alert["Cust"] = "";
                dr_Alert["PANUM"] = "";
                dr_Alert["SANUM"] = "";

                ObjDT.Rows.Add(dr_Alert);

                gvGarageMaint.DataSource = ObjDT;
                gvGarageMaint.DataBind();

                ObjDT.Rows.Clear();
                ViewState["DtGarageDetails"] = ObjDT;

                //gvGarageMaint.Rows[0].Cells.Clear();
                gvGarageMaint.Rows[0].Visible = false;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    protected void gvGarageMaint_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
            // FunPriGenerateNewPolicyRow();
            //AjaxControlToolkit.CalendarExtender CEInDate = e.Row.FindControl("CEInDate") as AjaxControlToolkit.CalendarExtender;
            //CEInDate.Format = strDateFormat;
            //AjaxControlToolkit.CalendarExtender calOutDate = e.Row.FindControl("calOutDate") as AjaxControlToolkit.CalendarExtender;
            //calOutDate.Format = strDateFormat;
           

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtAreaReq = (TextBox)e.Row.FindControl("txtAreaReq");
                TextBox txtCust = (TextBox)e.Row.FindControl("txtCust");
                TextBox txtRental = (TextBox)e.Row.FindControl("txtRental");
                Label lblAssetId = (Label)e.Row.FindControl("lblAssetId");
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDelete");
                AjaxControlToolkit.CalendarExtender calOutDateEdit = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calOutDateEdit");


                if (lblAssetId.Text != "0")
                {
                    imgbtnDelete.Visible = false;
                }
                txtAreaReq.Attributes.Add("onchange", "javascript:FnCalcRentallbl('" + txtAreaReq.ClientID + "', '" + TxtRntSqFeet.ClientID + "', '" + txtRental.ClientID + "', '" + txtCust.ClientID + "');");

                calOutDateEdit.Format = strDateFormat;
            }


            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlPANum = e.Row.FindControl("ddlPANum") as DropDownList;
                FunProLoadPrimAccounts(ddlPANum);

                TextBox txtAreaReq = (TextBox)e.Row.FindControl("txtAreaReq");
                TextBox txtCust = (TextBox)e.Row.FindControl("txtCust");
                TextBox txtRental = (TextBox)e.Row.FindControl("txtRental");

                txtRental.Attributes.Add("readonly", "readonly");

                txtAreaReq.Attributes.Add("onchange", "javascript:FnCalcRentaltxt('" + txtAreaReq.ClientID + "', '" + TxtRntSqFeet.ClientID + "', '" + txtRental.ClientID + "', '" + txtCust.ClientID + "');");

                AjaxControlToolkit.CalendarExtender calOutDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("calOutDate");
                calOutDate.Format = strDateFormat;
            }
        }

 

    protected void FunProLoadPrimAccounts(DropDownList ddlPANum)
    {
        if (ddlLOB.SelectedValue != "0" && ddlBranch.SelectedValue != "0" && (ucCustomerCodeLov.FindControl("hdnID") as HiddenField).Value != "")
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Garage_ID", Convert.ToString(ddlGarageOwnerCode.SelectedValue));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            Procparam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@Type", "1");
            Procparam.Add("@Customer_ID", (ucCustomerCodeLov.FindControl("hdnID") as HiddenField).Value);

            ddlPANum.BindDataTable("S3G_LR_Get_ACC_For_Garage", Procparam, new string[] { "Repossession_ID", "PANum" });
        }
    }

    private void FunProLoadSubAccounts(string strPAN, DropDownList ddlPANum, DropDownList ddlSANum)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Garage_ID", Convert.ToString(ddlGarageOwnerCode.SelectedValue));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            Procparam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            Procparam.Add("@Type", "2");
            Procparam.Add("@PANum", ddlPANum.SelectedItem.Text);
            Procparam.Add("@Customer_ID", (ucCustomerCodeLov.FindControl("hdnID") as HiddenField).Value);

            ddlSANum.BindDataTable("S3G_LR_Get_ACC_For_Garage", Procparam, new string[] { "Repossession_ID", "SANum" });

            if (ddlSANum.Items.Count == 1)
            {
                ddlSANum.Enabled = false;
                FunProLoadAsset();
            }
            else
            {

                (gvGarageMaint.FooterRow.FindControl("ddlAssetDescription") as DropDownList).Items.Clear();
                ddlSANum.Enabled = true;
            }
        }
        catch (Exception objFaultExp)
        {
            throw objFaultExp;
        }
    }

    protected void rbnEdit_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvGarageMaint", ((RadioButton)sender).ClientID);
        ImageButton imgbtnEdit = (ImageButton)gvGarageMaint.Rows[intRowIndex].FindControl("imgbtnEdit");

        Label lblCust = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblCust");
        Label lblAreaReq = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblAreaReq");
        Label lblOutDate = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblOutDate");
        Label lblRental = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblRental");
        TextBox txtAreaReq = (TextBox)gvGarageMaint.Rows[intRowIndex].FindControl("txtAreaReq");
        TextBox txtCust = (TextBox)gvGarageMaint.Rows[intRowIndex].FindControl("txtCust");
        TextBox txtOutDate = (TextBox)gvGarageMaint.Rows[intRowIndex].FindControl("txtOutDate");
        TextBox txtRental = (TextBox)gvGarageMaint.Rows[intRowIndex].FindControl("txtRental");

        //Image imgHODate = (Image)gvGarageMaint.Rows[intRowIndex].FindControl("imgHODate");
        //Image imgBankDate = (Image)gvGarageMaint.Rows[intRowIndex].FindControl("imgBankDate");

        imgbtnEdit.Visible = txtAreaReq.Visible = txtCust.Visible = txtOutDate.Visible = txtRental.Visible = true;
        ((RadioButton)sender).Visible = lblCust.Visible = lblAreaReq.Visible = lblOutDate.Visible = lblRental.Visible = false;
        ((RadioButton)sender).Checked = false;

        gvGarageMaint.Rows[intRowIndex].BackColor = gvGarageMaint.FooterStyle.BackColor;
    }

    protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvGarageMaint", ((ImageButton)sender).ClientID);
        RadioButton rbnEdit = (RadioButton)gvGarageMaint.Rows[intRowIndex].FindControl("rbnEdit");
        Label lblCust = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblCust");
        Label lblAreaReq = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblAreaReq");
        Label lblRental = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblRental");
        Label lblOutDate = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblOutDate");
        Label lblSLNo = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblSLNo");
        TextBox txtAreaReq = (TextBox)gvGarageMaint.Rows[intRowIndex].FindControl("txtAreaReq");
        TextBox txtCust = (TextBox)gvGarageMaint.Rows[intRowIndex].FindControl("txtCust");
        TextBox txtOutDate = (TextBox)gvGarageMaint.Rows[intRowIndex].FindControl("txtOutDate");
        TextBox txtRental = (TextBox)gvGarageMaint.Rows[intRowIndex].FindControl("txtRental");
        Label lblInDate = (Label)gvGarageMaint.Rows[intRowIndex].FindControl("lblInDate");


        if (Utility.CompareDates(Utility.StringToDate(txtGarageMaintDate.Text).ToString(), Utility.StringToDate(txtOutDate.Text).ToString()) != 1)
        {
            cvGarageMaster.ErrorMessage = "Out Date should be greater than System Date";
            cvGarageMaster.IsValid = false;
            return;
        }
        if (Utility.CompareDates(Utility.StringToDate(lblInDate.Text).ToString(), Utility.StringToDate(txtOutDate.Text).ToString()) != 1)
        {
            cvGarageMaster.ErrorMessage = "Out Date should be greater than In Date";
            cvGarageMaster.IsValid = false;
            return;
        }

        rbnEdit.Visible = lblCust.Visible = lblAreaReq.Visible = lblOutDate.Visible = lblRental.Visible = true;
        ((ImageButton)sender).Visible = txtAreaReq.Visible = txtCust.Visible = txtOutDate.Visible = txtRental.Visible = false;

        DataTable DtGarageDetails = (DataTable)ViewState["DtGarageDetails"];

        if (DtGarageDetails != null)
        {
            DtGarageDetails.Rows[intRowIndex]["OutDate"] = txtOutDate.Text;
            DtGarageDetails.Rows[intRowIndex]["Area"] = txtAreaReq.Text;
            DtGarageDetails.Rows[intRowIndex]["Cust"] = txtCust.Text;
            DtGarageDetails.Rows[intRowIndex]["Rental"] = lblRental.Text;

            DtGarageDetails.AcceptChanges();
            ViewState["DtGarageDetails"] = DtGarageDetails;
        }

        lblOutDate.Text = txtOutDate.Text;
        lblAreaReq.Text = txtAreaReq.Text;
        lblCust.Text = txtCust.Text;
        lblRental.Text = txtRental.Text;

        gvGarageMaint.Rows[intRowIndex].BackColor = System.Drawing.Color.White;
    }

    protected void ddlPANUM_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlPANum = gvGarageMaint.FooterRow.FindControl("ddlPANum") as DropDownList;
        DropDownList ddlSANum = gvGarageMaint.FooterRow.FindControl("ddlSANum") as DropDownList;
        if (ddlPANum.SelectedValue == "0")
        {
            ddlSANum.Items.Clear();
        }
        else
        {
            FunProLoadSubAccounts(ddlPANum.SelectedValue, ddlPANum, ddlSANum);
        }
    }

    protected void ddlSANUM_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlSANum = gvGarageMaint.FooterRow.FindControl("ddlSANum") as DropDownList;

        if (ddlSANum.SelectedValue == "0")
        {
            (gvGarageMaint.FooterRow.FindControl("ddlAssetDescription") as DropDownList).Items.Clear();
        }
        else
        {
            FunProLoadAsset();
        }
    }

    protected void ddlAssetDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAssetDescription = gvGarageMaint.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
        TextBox txtRegNum = gvGarageMaint.FooterRow.FindControl("txtRegNum") as TextBox;

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        Procparam.Add("@AssetNumber", ddlAssetDescription.SelectedValue.ToString());
        DataTable dtAsset = Utility.GetDefaultData("S3G_LR_GetAssetDtl", Procparam);

        if (dtAsset != null && dtAsset.Rows.Count > 0)
        {
            txtRegNum.Text = dtAsset.Rows[0]["REGN_NUMBER"].ToString();
        }
        else
        {
            txtRegNum.Text = "";
        }
    }

    protected void FunProLoadAsset()
    {
        DropDownList ddlPANum = gvGarageMaint.FooterRow.FindControl("ddlPANum") as DropDownList;
        DropDownList ddlSANum = gvGarageMaint.FooterRow.FindControl("ddlSANum") as DropDownList;
        DropDownList ddlAssetDescription = gvGarageMaint.FooterRow.FindControl("ddlAssetDescription") as DropDownList;

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        Procparam.Add("@PANUM", ddlPANum.SelectedItem.Text);
        if (ddlSANum.Items.Count > 1 && ddlSANum.SelectedValue != "0")
        {
            Procparam.Add("@SANUM", ddlSANum.SelectedItem.Text);
        }
        else
        {
            Procparam.Add("@SANUM", ddlPANum.SelectedItem.Text + "DUMMY");
        }
        Procparam.Add("@XMLGarageDetail", ((DataTable)ViewState["DtGarageDetails"]).FunPubFormXml());
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@Repossession_ID", ddlPANum.SelectedValue.ToString());
        ddlAssetDescription.BindDataTable("S3G_LR_GetSANAssetCode", Procparam, new string[] { "Asset_Number", "Asset_Description" });
    }



    protected void btnAddPolicy_OnClick(object sender, EventArgs e)
    {
        try
        {

            //AjaxControlToolkit.CalendarExtender CEInDate = e.Row.FindControl("CEInDate") as AjaxControlToolkit.CalendarExtender;
            //CEInDate.Format = strDateFormat;
            //AjaxControlToolkit.CalendarExtender calOutDate = e.Row.FindControl("calOutDate") as AjaxControlToolkit.CalendarExtender;
            //calOutDate.Format = strDateFormat;
            DataTable DtGarageDetails = (DataTable)ViewState["DtGarageDetails"];

            DropDownList ddlAssetDescription = gvGarageMaint.FooterRow.FindControl("ddlAssetDescription") as DropDownList;
            DropDownList ddlPANUM = gvGarageMaint.FooterRow.FindControl("ddlPANUM") as DropDownList;
            DropDownList ddlSANUM = gvGarageMaint.FooterRow.FindControl("ddlSANUM") as DropDownList;
            TextBox txtRegNum = gvGarageMaint.FooterRow.FindControl("txtRegNum") as TextBox;
            TextBox txtInDate = gvGarageMaint.FooterRow.FindControl("txtInDate") as TextBox;
            TextBox txtOutDate = gvGarageMaint.FooterRow.FindControl("txtOutDate") as TextBox;
            TextBox txtAreaReq = gvGarageMaint.FooterRow.FindControl("txtAreaReq") as TextBox;
            TextBox txtRental = gvGarageMaint.FooterRow.FindControl("txtRental") as TextBox;
            TextBox txtCust = gvGarageMaint.FooterRow.FindControl("txtCust") as TextBox;
            Label lblInDate = gvGarageMaint.FooterRow.FindControl("lblInDate") as Label;

            if (Utility.CompareDates(Utility.StringToDate(txtGarageMaintDate.Text).ToString(), Utility.StringToDate(txtOutDate.Text).ToString()) != 1)
            {
                cvGarageMaster.ErrorMessage = "Out Date should be greater than System Date";
                cvGarageMaster.IsValid = false;
                return;
            }
            if (Utility.CompareDates(Utility.StringToDate(txtInDate.Text).ToString(), Utility.StringToDate(txtOutDate.Text).ToString()) != 1)
            {
                cvGarageMaster.ErrorMessage = "Out Date should be greater than In Date";
                cvGarageMaster.IsValid = false;
                return;
            }

            DataRow dr_Alert = DtGarageDetails.NewRow();
            dr_Alert["AssetDescription"] = ddlAssetDescription.SelectedItem.Text;
            dr_Alert["AssetID"] = "0";
            dr_Alert["AssetNumber"] = ddlAssetDescription.SelectedValue;
            dr_Alert["DetailsID"] = "0";
            dr_Alert["MachineNo"] = txtRegNum.Text;
            dr_Alert["InDate"] = txtInDate.Text;
            dr_Alert["OutDate"] = txtOutDate.Text;
            dr_Alert["Area"] = txtAreaReq.Text;
            dr_Alert["Rental"] = txtRental.Text;
            dr_Alert["Cust"] = txtCust.Text;
            dr_Alert["PANUM"] = ddlPANUM.SelectedItem.Text;
            if (ddlSANUM.SelectedValue != "0")
                dr_Alert["SANUM"] = ddlSANUM.SelectedItem.Text;
            DtGarageDetails.Rows.Add(dr_Alert);

            gvGarageMaint.DataSource = DtGarageDetails;
            gvGarageMaint.DataBind();

            ViewState["DtGarageDetails"] = DtGarageDetails;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }

    }
    protected void gvGarageMaint_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable DtGarageDetails = (DataTable)ViewState["DtGarageDetails"];
        DtGarageDetails.Rows.RemoveAt(e.RowIndex);

        if (DtGarageDetails.Rows.Count == 0)
        {
            FunPriBindGarageDetail("Add");
        }
        else
        {
            gvGarageMaint.DataSource = DtGarageDetails;
            gvGarageMaint.DataBind();
            ViewState["DtGarageDetails"] = DtGarageDetails;
        }
    }


}
