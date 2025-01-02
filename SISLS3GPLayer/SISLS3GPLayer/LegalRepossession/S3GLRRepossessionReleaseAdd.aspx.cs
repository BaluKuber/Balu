/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name               : Legal Repossession 
/// Screen Name               : Repossession Release
/// Created By                : Gangadharrao
/// Created Date              : 23-April-2011
/// Purpose                   : 
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    :

/// <Program Summary>
#region [NameSpaces]
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using S3GBusEntity;
using System.Globalization;
#endregion
public partial class LegalRepossession_S3GLRRepossessionRelease :ApplyThemeForProject
{
    #region [Common Variable declaration]
    DataTable dtInsDetails;
    int intCompanyID, intUserID = 0;
    string strPageTitle = "Repossession Release";
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    StringBuilder strInsDetailsBuilder = new StringBuilder();
    int intResult;
    string strKey = "Update";
    string strpath = "";
    int ReleaseID=0;
    string StrProgramCode = "GRR";
   // string StrMode;
    StringBuilder strPASABuilder = new StringBuilder();
    //  StringBuilder strChallanBuilder = new StringBuilder();
    // DataTable dtAddLessDetails;
    //string strAccountXml = "";
    //StringBuilder strAccountDetailsBuilder = new StringBuilder();
    //string strKey = "Insert";
    
    string strRedirectPage = "~/LegalRepossession/S3GLRTransLander.aspx?Code=GRR";
    string strRedirectPageView = "window.location.href='../LegalRepossession/S3GLRTransLander.aspx?Code=GRR';";
    string strRedirectPageAdd = "window.location.href='../LegalRepossession/S3GLRRepossessionReleaseAdd.aspx';";
    string strRedirectRepossession = "../LegalRepossession/S3GLegalRepossessionRepossession_Add.aspx";
    string strAlert = "alert('__ALERT__');";
    string strlblHeading="RepossessionRelease";
    S3GSession ObjS3GSession = new S3GSession();
    string[] strCompanyLOBBranch = new string[3];
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    string StrMode = "";
    DataSet dtRepossessionDetails;
   
    #endregion

    #region [PageLoad Event]
    protected void Page_Load(object sender, EventArgs e)
    {

        FunPriPageLoad();
    }
    #endregion

    #region [Metods]
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            
            case 1:
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                FunDocketNumber("Q");
                FunRespossessionDetails("Q");
                if (DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW) == txtReleaseDate.Text)
                {
                 //   btnSave.Text = "UnRelease";
                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }
                btnClear.Enabled = false;
                ddlLRN.Enabled = false;
                ddlDocketNo.Enabled = false;
                imgReleaseDate.Visible = false;
                btnLedger.Enabled = false;
             //   btnRepo.Enabled = false;
                txtReleaseDate.Enabled = false;

                break;
       
               
            case -1:// Query Mode
                 lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                 FunDocketNumber("Q");
                 FunRespossessionDetails("Q");
                 btnSave.Enabled = false;
                 btnClear.Enabled = false;
                 ddlLRN.Enabled = false;
                 ddlDocketNo.Enabled = false;
                 imgReleaseDate.Visible = false;
                 btnLedger.Enabled = false;
               //  btnRepo.Enabled = false;
                 txtReleaseDate.Enabled = false;
             
                break;
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

    public static string SetGarageAddress(string Garage_Name, string Garage_Address1, string Garage_Address2, string Garage_City, string Garage_State, string Garage_Country, string Garage_Pin, string Garage_Telephone, string Garage_Mobile, string Garage_Email_ID, string Garage_Web_Site)
    {
        try
        {
            string strAddress = "";
            if (Garage_Name.ToString() != "") strAddress += Garage_Name.ToString() + System.Environment.NewLine;
            if (Garage_Address1.ToString() != "") strAddress += Garage_Address1.ToString() + System.Environment.NewLine;
            if (Garage_Address2.ToString() != "") strAddress += Garage_Address1.ToString() + System.Environment.NewLine;
            if (Garage_City.ToString() != "") strAddress += Garage_City.ToString() + System.Environment.NewLine;
            if (Garage_State.ToString() != "") strAddress += Garage_State.ToString() + System.Environment.NewLine;
            if (Garage_Country.ToString() != "") strAddress += Garage_Country.ToString() + System.Environment.NewLine;
            if (Garage_Pin.ToString() != "") strAddress += Garage_Pin.ToString() + System.Environment.NewLine;
            if (Garage_Telephone.ToString() != "") strAddress += Garage_Telephone.ToString() + System.Environment.NewLine;
            if (Garage_Mobile.ToString() != "") strAddress += Garage_Mobile.ToString() + System.Environment.NewLine;
            if (Garage_Email_ID.ToString() != "") strAddress += Garage_Email_ID.ToString() + System.Environment.NewLine;
            if (Garage_Web_Site.ToString() != "") strAddress += Garage_Web_Site.ToString();
            return strAddress;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion
    private void FunSaveRepoReleaseDetails()
    {
        DataSet dtRepoReleaseDetails = new DataSet();
        int intResult = 0;
        string strRepoReleaseNo = "";
        LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient objRepoReleaseClient = new LegalAndRepossessionMgtServicesReference.LegalAndRepossessionMgtServicesClient();
        dtRepoReleaseDetails = (DataSet)Session["RepoReleaseDetails"];

        S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionReleaseDataTable objRepoReleaseDataTable = new S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionReleaseDataTable();
        S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_RepossessionReleaseRow objRepoProcessessionRow = objRepoReleaseDataTable.NewS3G_LR_RepossessionReleaseRow();

        try
        {
            if (dtRepoReleaseDetails.Tables[0].Rows.Count > 0)
            {
                /*Revision History:
                 *Performed by : Srivatsan.S
                 *Performed on : 05/11/2011
                 *Description  : In the Code below the LOBID is referencing to the Location ID and viceversa. 
                 *In the next line the latest code is placed which is referencing correctly.
                 */
                
                //string strLOBID = dtRepoReleaseDetails.Tables[0].Rows[0]["Location_ID"].ToString();
                //string strBranchID = dtRepoReleaseDetails.Tables[0].Rows[0]["LOB_ID"].ToString();

                string strLOBID = dtRepoReleaseDetails.Tables[0].Rows[0]["LOB_ID"].ToString();
                string strBranchID = dtRepoReleaseDetails.Tables[0].Rows[0]["Location_ID"].ToString();

                strCompanyLOBBranch[0] = Convert.ToString(intCompanyID);
                strCompanyLOBBranch[1] = Convert.ToString(strLOBID);
                strCompanyLOBBranch[2] = Convert.ToString(strBranchID);
            }
            objRepoProcessessionRow.Repossession_Release_ID = ReleaseID;
            objRepoProcessessionRow.Repossession_Release_No = "1";
            objRepoProcessessionRow.LRN_ID = Convert.ToInt32(ddlLRN.SelectedValue);
            objRepoProcessessionRow.LRN_No = Convert.ToString(ddlLRN.SelectedItem.Text);
            objRepoProcessessionRow.Repossession_ID = Convert.ToInt32(ddlDocketNo.SelectedValue);
            objRepoProcessessionRow.Repossession_Docket_No = Convert.ToString(ddlDocketNo.SelectedItem.Text);
            objRepoProcessessionRow.Released_By = "";


            //if (strDateFormat == "dd/MM/yyyy")
            //{
            //    DateTime dtReleaseDate = DateTime.Parse(txtReleaseDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
            //    objRepoProcessessionRow.Repossession_Release_Date = Convert.ToDateTime(dtReleaseDate);
            //    objRepoProcessessionRow.Created_On = Convert.ToDateTime(dtReleaseDate);
            //}
            //if (strDateFormat == "dd-MM-yyyy")
            //{
            //    DateTime dtReleaseDate = DateTime.Parse(txtReleaseDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
            //    objRepoProcessessionRow.Repossession_Release_Date = Convert.ToDateTime(dtReleaseDate);
            //    objRepoProcessessionRow.Created_On = Convert.ToDateTime(dtReleaseDate);
            //}
            //else
            //{
            //    objRepoProcessessionRow.Repossession_Release_Date = Convert.ToDateTime(txtReleaseDate.Text);
            //    objRepoProcessessionRow.Created_On = Convert.ToDateTime(txtReleaseDate.Text);
            //}

            objRepoProcessessionRow.Repossession_Release_Date = Utility.StringToDate(txtReleaseDate.Text);
            objRepoProcessessionRow.Created_On = Utility.StringToDate(txtReleaseDate.Text);

            objRepoProcessessionRow.Created_By = intUserID;
            objRepoReleaseDataTable.AddS3G_LR_RepossessionReleaseRow(objRepoProcessessionRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] objbyteRepoReleaseTable = ClsPubSerialize.Serialize(objRepoReleaseDataTable, SerMode);

            intResult = objRepoReleaseClient.FunPubCreateRepoRelease(out strRepoReleaseNo, SerMode, objbyteRepoReleaseTable, strCompanyLOBBranch);

            if (ReleaseID == 0)
            {
                //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intResult == 0)
                {

                    strAlert = "Repossession Release " + strRepoReleaseNo + " created sucessfully";
                    strAlert += @"\n\nWould you like to create one more Release?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";


                }
                else if (intResult == 1)
                {

                    strAlert = strAlert.Replace("__ALERT__", "Document Sequence for Repossession Release was not defined");

                    strRedirectPageView = "";
                }
                else
                {

                    strAlert = strAlert.Replace("__ALERT__", "Error in creating  Repossession Release");


                    strRedirectPageView = "";
                }
            }
            else
            {
                if (intResult == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Updated Repossession Release Details sucessfully");



                }

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            objRepoReleaseClient.Close();
            objRepoReleaseDataTable = null;
            Session["RepoReleaseDetails"] = null;
        }
    }
    private void FunRespossessionDetails(string strOption)
    {
        try
        {
            
            string[] strAddress;

            string strCustomerAddress = "";
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            btnRepo.Visible = true;
            //**************Repossession Details in Create Mode*********************
            if (strOption == "C")
            {
                
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LRN_No", Convert.ToString(ddlLRN.SelectedItem.Text));
                Procparam.Add("@Docket_ID", Convert.ToString(ddlDocketNo.SelectedItem.Text));
                
                dtRepossessionDetails = Utility.GetDataset("S3G_LR_GetLRNRepossessionDetails", Procparam);
                Procparam.Clear();
            }
            //**************Repossession Release in View Mode************************
            if (strOption == "Q")
            {

                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@Repossession_Release_ID", Convert.ToString(ReleaseID));
                dtRepossessionDetails = Utility.GetDataset("S3G_LR_GetRepossessionReleaseDetails", Procparam);
                Procparam.Clear();
            }


            if (dtRepossessionDetails.Tables.Count > 0)
            {
                Session["RepoReleaseDetails"] = dtRepossessionDetails;
                if (dtRepossessionDetails.Tables[0].Rows.Count > 0)
                {
                    PnlGarageDetails.Visible = true;
                    btnSave.Visible = btnClear.Visible = btnCancel.Visible = true;
                    pnlGeneralDetails.Visible = true;
                    pnlCustomerDetails.Visible = true;
                    if (strOption == "Q")
                    {
                        ddlLRN.SelectedValue = dtRepossessionDetails.Tables[0].Rows[0]["LRN_ID"].ToString();
                        ddlDocketNo.SelectedValue = dtRepossessionDetails.Tables[0].Rows[0]["Repossession_ID"].ToString();
                        txtRRNo.Text = dtRepossessionDetails.Tables[5].Rows[0]["Release_No"].ToString();
                        txtReleaseDate.Text = dtRepossessionDetails.Tables[5].Rows[0]["Release_Date"].ToString();
                    }
                    //****************** To Fetch the Customer Details**************************
                    strCustomerAddress = SetCustomerAddress(dtRepossessionDetails.Tables[0].Rows[0]["comm_address1"].ToString(), dtRepossessionDetails.Tables[0].Rows[0]["comm_address2"].ToString(), dtRepossessionDetails.Tables[0].Rows[0]["comm_city"].ToString(), dtRepossessionDetails.Tables[0].Rows[0]["comm_state"].ToString(), dtRepossessionDetails.Tables[0].Rows[0]["comm_country"].ToString(), dtRepossessionDetails.Tables[0].Rows[0]["comm_pincode"].ToString());
                    S3GCustomerAddress1.SetCustomerDetails(
                        dtRepossessionDetails.Tables[0].Rows[0]["Customer_Code"].ToString(),
                        strCustomerAddress, 
                        dtRepossessionDetails.Tables[0].Rows[0]["Customer_Name"].ToString(),
                        dtRepossessionDetails.Tables[0].Rows[0]["Comm_Telephone"].ToString(),
                        dtRepossessionDetails.Tables[0].Rows[0]["COMM_MOBILE"].ToString(),
                        dtRepossessionDetails.Tables[0].Rows[0]["COMM_EMAIL"].ToString(),
                        dtRepossessionDetails.Tables[0].Rows[0]["Comm_Website"].ToString());
                    
                    //*******************To Fetch Repossession Details and Account Details*************************
                    txtAction.Text = dtRepossessionDetails.Tables[0].Rows[0]["Lookup_Description"].ToString();
                    string strLOBName = dtRepossessionDetails.Tables[0].Rows[0]["LOB_Name"].ToString();
                    string strBranchName = dtRepossessionDetails.Tables[0].Rows[0]["Branch_Name"].ToString();
                    string strPANum = dtRepossessionDetails.Tables[0].Rows[0]["PANum"].ToString();
                    //if (strPANum != "")
                    //{
                    //    btnAccount.Visible = false;
                    //}

                    string StrSANum = dtRepossessionDetails.Tables[0].Rows[0]["SAnum"].ToString();
                    string strFinanceAmount = dtRepossessionDetails.Tables[0].Rows[0]["Finance_Amount"].ToString();
                    string strDocketDate = dtRepossessionDetails.Tables[0].Rows[0]["Repossession_Docket_Date"].ToString();
                    string strTenure = dtRepossessionDetails.Tables[0].Rows[0]["Tenure"].ToString();
                    txtLOB.Text = (!string.IsNullOrEmpty(strLOBName)) ? strLOBName : "";
                    txtBranch.Text = (!string.IsNullOrEmpty(strBranchName)) ? strBranchName : "";
                    txtPANum.Text = (!string.IsNullOrEmpty(strPANum)) ? strPANum : "";
                    txtSANum.Text = (!string.IsNullOrEmpty(StrSANum)) ? StrSANum : "";
                    if (strDocketDate != "")
                    {
                        txtRepoDate.Text = strDocketDate;
                    }
                    txtAmountFinanced.Text = (!string.IsNullOrEmpty(strFinanceAmount)) ? strFinanceAmount : "";
                    txtTenure.Text = (!string.IsNullOrEmpty(strTenure)) ? strTenure : "";


                }
                //************To Bind the Guarantor Details********************
                if (dtRepossessionDetails.Tables[1].Rows.Count > 0)
                {
                    btnSave.Visible = btnClear.Visible = btnCancel.Visible = true;
                    PnlGuarantor.Visible = true;
                    grvGuarantor.DataSource = dtRepossessionDetails.Tables[1];
                    grvGuarantor.DataBind();


                }
                else
                {
                    PnlGuarantor.Visible = true;
                    lblEmpty.Visible = true;
                }
                //*****************To Bind Asset Details**********************
                if (dtRepossessionDetails.Tables[2].Rows.Count > 0)
                {
                    btnSave.Visible = btnClear.Visible = btnCancel.Visible = true;
                    pnlAssetDetails.Visible = true;
                    grvRepossessionAssetDetails.DataSource = dtRepossessionDetails.Tables[2];
                    grvRepossessionAssetDetails.DataBind();
                }
                //*****************To Fetch the Garage Details*****************
                if (dtRepossessionDetails.Tables[3].Rows.Count > 0)
                {
                    btnSave.Visible = btnClear.Visible = btnCancel.Visible = true;
                    string strGarageCode = dtRepossessionDetails.Tables[3].Rows[0]["Garage_Code"].ToString();
                    string strGarageAddress = "";

                    strGarageAddress=SetGarageAddress(dtRepossessionDetails.Tables[3].Rows[0]["Garage_Name"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_Address1"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_Address2"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_City"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_State"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_Country"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_Pin"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_Telephone"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_Mobile"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_Email_ID"].ToString()
                                     , dtRepossessionDetails.Tables[3].Rows[0]["Garage_Web_Site"].ToString());
                    //string strGarageAddress="";
                    //strGarageAddress= dtRepossessionDetails.Tables[3].Rows[0]["Address"].ToString();
                    //if (strGarageAddress != "")
                    //{
                    //    strAddress = strGarageAddress.Trim().Split(',');
                    //    strGarageAddress = "";
                    //    strGarageAddress = SetCustomerAddress(strAddress[0], strAddress[1], strAddress[2], strAddress[3], strAddress[4], "");
                    //}
                    string strAssetCondition = dtRepossessionDetails.Tables[3].Rows[0]["Asset_Condition"].ToString();
                    string StrInventory = dtRepossessionDetails.Tables[3].Rows[0]["Asset_Inventory_Details"].ToString();

                    txtGarageCode.Text = (!string.IsNullOrEmpty(strGarageCode)) ? strGarageCode : "";
                    txtGarageAddress.Text = strGarageAddress;


                    txtAssetInventory.Text = (!string.IsNullOrEmpty(StrInventory)) ? StrInventory : "";
                    txtAssetCondition.Text = (!string.IsNullOrEmpty(strAssetCondition)) ? strAssetCondition : "";




                }
                //*****************To Fetch Insurance Details*****************
                if (dtRepossessionDetails.Tables[4].Rows.Count > 0)
                {
                    string strAssetInsValidity = dtRepossessionDetails.Tables[4].Rows[0]["Insurance_validity"].ToString();
                    if (!string.IsNullOrEmpty(strAssetInsValidity))
                    {
                        txtInsuranceValidUpto.Text = strAssetInsValidity;
                    }

                }


            }



        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FunLRNumber()
    {
        try
        {
           
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            //*******************To Bind the Legal Repossession Details*******************
           // Procparam.Add("@OPTION", "3");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", null);
            Procparam.Add("@Location_ID", null);
            Procparam.Add("@ProgramCode", "GRR");
            if (ReleaseID == 0)
            {
                Procparam.Add("@Mode", "C");
            }
            Procparam.Add("@Action ", "2");
            ddlLRN.BindDataTable("S3G_LR_GetLRNNumber", Procparam, new string[] { "LRN_ID", "LRN_No"});
            Procparam.Clear();
           
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FunClearValues()
    {


        S3GCustomerAddress1.ClearCustomerDetails();
        txtAction.Text = txtAmountFinanced.Text = txtAssetCondition.Text = txtAssetInventory.Text = txtBranch.Text = txtGarageAddress.Text = txtGarageCode.Text
            = txtInsuranceValidUpto.Text = txtLOB.Text = txtPANum.Text = txtRepoDate.Text = txtRRNo.Text = txtSANum.Text = txtTenure.Text = "";
        grvGuarantor.DataSource = "";
        PnlGuarantor.Visible = pnlAssetDetails.Visible = false;
        grvGuarantor.DataBind();
        grvRepossessionAssetDetails.DataSource = "";
        grvRepossessionAssetDetails.DataBind();
        tcRepossessionRelease.ActiveTabIndex = 0;
        ddlLRN.Focus();
        btnRepo.Visible = false;
    }
    private void FunDocketNumber(string strMode)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            string StrLRNo = ddlLRN.SelectedItem.Text;

            if (StrLRNo == "--Select--")
            {
                StrLRNo = null;
            }
           //*******************To Bind the Docket Details*******************
           
            Procparam.Add("@ProgramCode", StrProgramCode);
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LRN_No", StrLRNo);
            Procparam.Add("@Mode", strMode);
           
            ddlDocketNo.BindDataTable("S3G_LR_GetDocketNumber", Procparam, new string[] { "Repossession_ID", "Repossession_Docket_No" });
            Procparam.Clear();

        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FunPriPageLoad()
    {

        try
        {
         
            

            this.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            #region [Assign DateFormate]
 
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;

            #endregion
            // making the textbox readonly
            #region [TextboxReadOnly]
            txtRepoDate.Attributes.Add("readonly", "readonly");
            txtReleaseDate.Attributes.Add("readonly", "readonly");
            txtAction.Attributes.Add("readonly", "readonly");
            txtPANum.Attributes.Add("readonly", "readonly");
            txtSANum.Attributes.Add("readonly", "readonly");
            txtLOB.Attributes.Add("readonly", "readonly");
            txtBranch.Attributes.Add("readonly", "readonly");
            txtAmountFinanced.Attributes.Add("readonly", "readonly");
            txtTenure.Attributes.Add("readonly", "readonly");
            txtRRNo.Attributes.Add("readonly", "readonly");
            txtGarageCode.Attributes.Add("readonly", "readonly");
            txtGarageAddress.Attributes.Add("readonly", "readonly");
            txtAssetInventory.Attributes.Add("readonly", "readonly");
            txtAssetCondition.Attributes.Add("readonly", "readonly");
            txtInsuranceValidUpto.Attributes.Add("readonly", "readonly");
            #endregion
            //Level of User for Modify, View and Create
            #region [UserLevel]
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            #endregion
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                ReleaseID = Convert.ToInt32(fromTicket.Name);
            }


            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;



            if (!IsPostBack)
            {
                lblHeading.Text = strPageTitle;
                txtReleaseDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW);
                pnlAssetDetails.Visible = PnlGuarantor.Visible = false;
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                FunToolTip();
                FunLRNumber();
                tcRepossessionRelease.ActiveTabIndex = 0;
                ddlLRN.Focus();
                if (Request.QueryString["qsMode"] == "Q")
                {

                    FunPriDisableControls(-1);
                }

                if (Request.QueryString["qsMode"] == "M")
                {
                    FunPriDisableControls(1);
                }

            }
        }
        catch(Exception ex)
        {
            throw (ex);
        }
        finally
        {
            ObjS3GSession = null;
            ObjUserInfo = null;
        }
            

          
            
        
    }

    private void FunToolTip()
    {
        ddlLRN.ToolTip = lblLRN.Text;
        ddlDocketNo.ToolTip = lblRepoDocketNo.Text;
        txtAction.ToolTip = lblAction.Text;
        txtAmountFinanced.ToolTip = lblAmountFinanced.Text;
        txtAssetCondition.ToolTip = lblAssetCondition.Text;
        txtAssetInventory.ToolTip = lblAssetInventory.Text;
        txtBranch.ToolTip = lblBranch.Text;
        txtGarageAddress.ToolTip = lblGarageAddress.Text;
        txtGarageCode.ToolTip = lblGarageCode.Text;
        txtInsuranceValidUpto.ToolTip = lblInsuranceValidUpto.Text;
        txtLOB.ToolTip = lblLOB.Text;
        txtPANum.ToolTip = lblPANum.Text;
        txtReleaseDate.ToolTip = lblReleaseDate.Text;
        txtRepoDate.ToolTip = lblRepoDate.Text;
        txtRRNo.ToolTip = lblRRNo.Text;
        txtSANum.ToolTip = lblSANum.Text;
        txtTenure.ToolTip = lblTenure.Text;
      

        //throw new NotImplementedException();
        //ddlBranch.ToolTip = lblBranch.Text;
        //ddlLOB.ToolTip = lblLOB.Text;
        //ddlAction.ToolTip = lblAction.Text;
        //ddlSLA.ToolTip = lblSLA.Text;
        //ddlMLA.ToolTip = lblMLA.Text;
        //txtLRNo.ToolTip = lblLRNo.Text;
        //txtLRDate.ToolTip = lblLRDate.Text;
        //txtActionPoints.ToolTip = lblActionPoints.Text;
        //ddlFollowUPEMPID.ToolTip = lblFlUpEmpID.Text;
        //txtFlUpEmpName.ToolTip = lblFlUpEmpName.Text;
        //txtLRStatus.ToolTip = lblLRStatus.Text;

    }

    #endregion
    
    #region [Button Events]
    protected void btnSave_Click(object sender, EventArgs e)
    {
        FunSaveRepoReleaseDetails();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        FunClearValues();
        ddlLRN.SelectedValue = "0";
        ddlDocketNo.SelectedValue = "0";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    //Changed by Bhuvana.Here Redirect the correct screen name to display the correct details of Repossesseion details.
    protected void btnRepo_Click(object sender, EventArgs e)
    {
        string s = "";

        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(ddlDocketNo.SelectedValue.ToString(), false, 0);

        if (strRedirectRepossession.Contains('?'))
            strRedirectRepossession += "&";
        else
            strRedirectRepossession += "?";

       // s=strRedirectRepossession + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q";
        string strScipt = "window.open('" + strRedirectRepossession + "qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsPopup=1" + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "SOA", strScipt, true);

        //string s = "";
        //if (txtSANum.Text == "")
        //    s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=" + txtPANum.Text + "&qsSANum=" + txtPANum.Text + "/DUMMY";

        //else
        //    s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=" + txtPANum.Text + "&qsSANum=" + txtSANum.Text;

        //// s = "../LegalRepossession/S3GLRViewAccount.aspx?qsPANum=2011-2012/PRIME/10001&qsSANum=2011-2012/WCSAN/100";

        //string strScipt = "window.open('" + s + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "SOA", strScipt, true);

    }

    #endregion

    #region [DropDownList Events]
    protected void ddlLRN_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClearValues();
       // ddlDocketNo.SelectedIndex = 0;
        if (ddlLRN.SelectedIndex > 0)
        {
           
            FunDocketNumber("C");
        }
       
    }
    protected void ddlDocketNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClearValues();
        if (ddlLRN.SelectedIndex > 0 && ddlDocketNo.SelectedIndex > 0)
        {
           
            FunRespossessionDetails("C");
        }
    }
    #endregion


    

}
