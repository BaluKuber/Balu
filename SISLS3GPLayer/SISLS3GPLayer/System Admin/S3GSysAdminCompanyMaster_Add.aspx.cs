 #region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Company Creation
/// Created By			: Nataraj Y
/// Created Date		: 07-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 07-May-2010
/// Reason              : System Admin Module Developement
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 10-May-2010
/// Reason              : System Admin Company Module Developement
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using System.Data;
#endregion

public partial class System_Admin_S3GSysAdminCompanyMaster_Add : ApplyThemeForProject
{
    #region Intialization
    int intErrCode = 0;
    int intCompanyId = 0;
    int intUserId = 0;
    bool bModify = false;
    UserInfo ObjUserInfo;
    bool bQuery = false;
    bool bCreate = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    string strMode;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminCompanyMaster_View.aspx';";
    //string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminCompanyMaster_Add.aspx';";
    CompanyMgtServicesReference.CompanyMgtServicesClient objCompanyMasterClient;
    string strDateFormat;
    S3GSession ObjS3GSession = new S3GSession();
    CompanyMgtServices.S3G_SYSAD_CompanyMaster_CUDataTable ObjS3G_SYSAD_CompanyMaster_CUDataTable = new CompanyMgtServices.S3G_SYSAD_CompanyMaster_CUDataTable();
    CompanyMgtServices.S3G_SYSAD_CompanyMasterDataTable ObjS3G_SYSAD_CompanyMasterDataTable = new CompanyMgtServices.S3G_SYSAD_CompanyMasterDataTable();
    CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable ObjS3G_CompanyMaster_ViewDataTable = new CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable();
    SerializationMode SerMode = SerializationMode.Binary;
    string strGST;
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();
        //S3GSession ObjS3GSession = new S3GSession();
        //if (Request.QueryString["qsCmpnyId"] != null)
        intCompanyId = ObjUserInfo.ProCompanyIdRW;//Convert.ToInt32(Request.QueryString["qsCmpnyId"]);
        intUserId = ObjUserInfo.ProUserIdRW;
        txtRemarks.Attributes.Add("onchange", "maxlengthfortxt(60);");
        string strPassword = txtSystemAdminPwd.Text;
        string strRedirectPage;
        txtSystemAdminPwd.Attributes.Add("value", strPassword);
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        //CalendarExtender1.Format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        CalendarExtender1.Format = strDateFormat;
        cexValdityOfRegNumber.Format = strDateFormat;
        CalendarExtender2.Format = strDateFormat;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        FunSetComboBoxAttributes(txtCity, "City", "30");
        FunSetComboBoxAttributes(txtState, "State", "60");
        FunSetComboBoxAttributes(txtCountry, "Country", "60");

        FunSetComboBoxAttributes(txtOtherCity, "City", "30");
        FunSetComboBoxAttributes(txtOtherState, "State", "60");
        FunSetComboBoxAttributes(txtOtherCountry, "Country", "60");

        txtDateOfIncorp.Attributes.Add("onblur", "fnDoDate(this,'" + txtDateOfIncorp.ClientID + "','" + strDateFormat + "',true,  false);");
        txtValdityOfRegNumber.Attributes.Add("onblur", "fnDoDate(this,'" + txtValdityOfRegNumber.ClientID + "','" + strDateFormat + "',false,  'F');");
        TxtCGSTRegDate.Attributes.Add("onblur", "fnDoDate(this,'" + TxtCGSTRegDate.ClientID + "','" + strDateFormat + "',true,  false);");
        strGST = ClsPubConfigReader.FunPubReadConfig("Is_GST");//GST
        if (!IsPostBack)
        {
            txtPanNumber.Attributes.Add("maxlength", "20");

            //txtDateOfIncorp.Attributes.Add("readonly", "readonly");
            //txtValdityOfRegNumber.Attributes.Add("readonly", "readonly");

            FunProLoadAddressCombos();
            if (intCompanyId > 0)
            {
                FunPriCompany();

                if (!bIsActive)
                {

                    FunPriDisableControls(-1);
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    btnCancel.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {

                    //if (bMakerChecker)
                    //{
                    //    if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(hdnUserId.Value), Convert.ToInt32(hdnUserLevelId.Value))))
                    //    {
                    //        strMode = "M";
                    //    }
                    //    else
                    //    {
                    //        strMode = "Q";
                    //    }
                    //}
                    //else
                    //{
                    if ((bModify) && (ObjUserInfo.ProUserLevelIdRW == 5))
                    {
                        strMode = "M";
                    }
                    else
                    {
                        strMode = "Q";
                    }
                    //}

                }
            }
            if (strMode == "M")
            {
                FunPriDisableControls(1);
            }
            else if (strMode == "Q")
            {
                FunPriDisableControls(-1);
            }
            else
            {
                //FunPriCompany();
                FunPriDisableControls(0);
            }
        }
    }
    #endregion

    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
        }
    }

    #region Page Events
    /// <summary>
    /// Next button event to move to next Tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (tcCompanyCreation.ActiveTabIndex != tcCompanyCreation.Tabs.Count - 1)
        {
            tcCompanyCreation.ActiveTabIndex = tcCompanyCreation.ActiveTabIndex + 1;
        }
    }
    /// <summary>
    /// Previous button event to move to previous tabs by getting curent tab index
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrev_Click(object sender, EventArgs e)
    {
        if (tcCompanyCreation.ActiveTabIndex != 0)
        {
            tcCompanyCreation.ActiveTabIndex = tcCompanyCreation.ActiveTabIndex - 1;
        }
    }
    /// <summary>
    /// Save button event to insert and update record inser or update Service is called based on querystring 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrorMessage.InnerText = string.Empty;
            if (CheckZeroValidate(txtCompanyCode.Text))
            {
                if (CheckZeroValidate(txtCompanyName.Text))
                {
                    if (txtSystemAdminPwd.Text.IsValidPassword())
                    {
                        if (Utility.CompareDates(txtDateOfIncorp.Text, txtValdityOfRegNumber.Text) == 1)
                        {

                            CompanyMgtServices.S3G_SYSAD_CompanyMaster_CURow ObjCompanyMasterRow;
                            ObjCompanyMasterRow = ObjS3G_SYSAD_CompanyMaster_CUDataTable.NewS3G_SYSAD_CompanyMaster_CURow();
                            ObjCompanyMasterRow.Company_ID = intCompanyId;
                            ObjCompanyMasterRow.Accounting_Currency = txtAccCurrency.Text.Trim();
                            ObjCompanyMasterRow.Address1 = txtAddress.Text.Trim();
                            ObjCompanyMasterRow.Address2 = txtAddress2.Text.Trim();
                            ObjCompanyMasterRow.City = txtCity.SelectedItem.Text.Trim(); //txtCity.Text;
                            ObjCompanyMasterRow.State = txtState.SelectedItem.Text.Trim();
                            ObjCompanyMasterRow.CD_CEO_Head_Name = txtHeadName.Text.Trim();
                            ObjCompanyMasterRow.CD_Email_ID = txtEmailId.Text.Trim();
                            ObjCompanyMasterRow.CD_Mobile_Number = txtMobileNumber.Text.Trim();
                            ObjCompanyMasterRow.CD_Sys_Admin_User_Code = txtSystemAdminUser.Text.Trim();
                            ObjCompanyMasterRow.CD_Sys_Admin_User_Password = ClsPubCommCrypto.EncryptText(txtSystemAdminPwd.Text.Trim());
                            ObjCompanyMasterRow.CD_Telephone_Number = txtTeleNumber.Text.Trim();
                            ObjCompanyMasterRow.Zip_Code = txtPinCode.Text.Trim();
                            ObjCompanyMasterRow.CD_Website = txtWebsite.Text.Trim();
                            ObjCompanyMasterRow.Company_Code = txtCompanyCode.Text.Trim();
                            ObjCompanyMasterRow.Company_Name = txtCompanyName.Text.Trim();
                            ObjCompanyMasterRow.Constitutional_Status_Id = Convert.ToInt32(ddlConstitutionalStatus.SelectedItem.Value);
                            ObjCompanyMasterRow.Country = txtCountry.SelectedItem.Text.Trim();
                            ObjCompanyMasterRow.Create_By = intUserId;
                            ObjCompanyMasterRow.is_Active = chkboxActive.Checked;
                            ObjCompanyMasterRow.Modified_By = intUserId;
                            ObjCompanyMasterRow.OD_Address1 = txtAddress1.Text.Trim();
                            ObjCompanyMasterRow.OD_City = txtOtherCity.Text.Trim();
                            ObjCompanyMasterRow.OD_Communication_Address = txtCommunicationAdd.Text.Trim();
                            ObjCompanyMasterRow.OD_Country = txtOtherCountry.Text.Trim();
                            ObjCompanyMasterRow.OD_Date_Of_Incorporation = Utility.StringToDate(txtDateOfIncorp.Text.Trim());
                            ObjCompanyMasterRow.OD_Income_Tax_PAN_Number = txtPanNumber.Text.Trim();
                            ObjCompanyMasterRow.OD_Reg_Lic_Number = txtRegNumber.Text.Trim();
                            ObjCompanyMasterRow.CST_NO = "";
                            ObjCompanyMasterRow.ST_NO = txtSTNo.Text.Trim();
                            ObjCompanyMasterRow.OD_Remarks = txtRemarks.Text.Trim();
                            ObjCompanyMasterRow.OD_State = txtOtherState.Text.Trim();
                            ObjCompanyMasterRow.OD_Validity_Of_Reg_Lic_Number = Utility.StringToDate(txtValdityOfRegNumber.Text.Trim());
                            ObjCompanyMasterRow.OD_Zip_Code = txtOtherPin.Text.Trim();
                            ObjCompanyMasterRow.SetCreate_OnNull();
                            ObjCompanyMasterRow.SetModified_OnNull();

                            //DataTable Newdt = new DataTable();
                            //Newdt = ((DataTable)ViewState["CompanyVATTIN"]);
                            //DataRow[] NewRow = Newdt.Select("VATTIN=''");
                            //if (NewRow.Length > 0)
                            //{
                            //    foreach (DataRow drow1 in NewRow)
                            //    {
                            //        Newdt.Rows.Remove(drow1);
                            //    }
                            //}
                            //ViewState["CompanyVATTIN"] = Newdt;
                            //if (((DataTable)ViewState["CompanyVATTIN"]).Rows.Count != 0)
                            //{
                            //    ObjCompanyMasterRow.XML_VATTIN = Convert.ToString(((DataTable)ViewState["CompanyVATTIN"]).FunPubFormXml());  
                            //}

                            //if (ObjCompanyMasterRow.XML_VATTIN.ToString().Contains("<Details />"))
                            //{
                            //    ObjCompanyMasterRow.XML_VATTIN = "";
                            //}
                            ObjCompanyMasterRow.XML_VATTIN = "";

                            ObjCompanyMasterRow.CGSTIN = txtCGSTin.Text.ToString();
                            if (TxtCGSTRegDate.Text != "")
                                ObjCompanyMasterRow.CGST_Reg_Date = Utility.StringToDate(TxtCGSTRegDate.Text.Trim());

                            ObjS3G_SYSAD_CompanyMaster_CUDataTable.AddS3G_SYSAD_CompanyMaster_CURow(ObjCompanyMasterRow);

                            objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

                            if (intCompanyId > 0)
                            {
                                // if (FunPriCheckValidPinCode(txtOtherPin.Text, strType) && FunPriCheckValidPinCode(txtPinCode.Text, strType))
                                // {

                                intErrCode = objCompanyMasterClient.FunPubModifyCompany(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_CompanyMaster_CUDataTable, SerMode));
                                // }
                                //else
                                //{
                                //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Enter a valid PIN code');", true);
                                //}
                            }
                            else
                            {
                                intErrCode = objCompanyMasterClient.FunPubCreateCompany(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_CompanyMaster_CUDataTable, SerMode));
                            }

                            if (intErrCode == 0)
                            {
                                if (intCompanyId > 0)
                                {
                                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here
                                    strAlert = strAlert.Replace("__ALERT__", "Company details updated successfully");
                                    //ObjUserInfo.ProCountryNameR = txtCountry.Text;
                                    //ObjUserInfo.ProCompanyNameRW = txtCompanyName.Text;
                                    Session.Abandon();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                                }
                                else
                                {
                                    //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here
                                    strAlert = strAlert.Replace("__ALERT__", @"Company details added successfully.\n\n Relogin using system admin credentials to proceed further");
                                    Session.Abandon();
                                    //strRedirectPageView = "window.location.href='../LoginPage.aspx'";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);

                                }
                            }
                            else if (intErrCode == 1)
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Company Code already exists in the system. kindly enter a new Company Code");
                                strRedirectPageView = "";
                            }
                            else if (intErrCode == 2)
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Company Name already exists in the system. kindly enter a new Company Name");
                                strRedirectPageView = "";
                            }
                            else if (intErrCode == 3)
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Income Tax Number/PAN already exists in the system. kindly enter a new Income Tax Number/PAN");
                                strRedirectPageView = "";
                            }
                            else if (intErrCode == 4)
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Registration Number/Licence Number already exists in the system. kindly enter a new Registration Number/Licence Number");
                                strRedirectPageView = "";
                            }
                            else if (intErrCode == 5)
                            {
                                strAlert = strAlert.Replace("__ALERT__", "System admin user code already exists in the system. kindly enter a new System admin");
                                strRedirectPageView = "";
                            }
                            else
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Unexpected error in processing your request!");
                                strRedirectPageView = "";
                                lblErrorMessage.InnerText = "Unexpected error in processing your request!";
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                            //Utility.FunShowAlertMsg(this.Page, strAlert + strRedirectPage);
                            lblErrorMessage.InnerText = string.Empty;

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Validity of Registration/Licence Number should be greater than date of incorporation');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Please enter a valid Password');", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Please enter a valid Company Name');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Please enter a valid Company Code');", true);
            }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            if (objCompanyMasterClient != null)
            {
                objCompanyMasterClient.Close();
            }
        }

    }
    /// <summary>
    /// Delete butoon event to delete a record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {

            CompanyMgtServices.S3G_SYSAD_CompanyMasterRow ObjCompanyMasterRow;
            ObjCompanyMasterRow = ObjS3G_SYSAD_CompanyMasterDataTable.NewS3G_SYSAD_CompanyMasterRow();
            ObjCompanyMasterRow.Company_ID = intCompanyId;
            ObjS3G_SYSAD_CompanyMasterDataTable.AddS3G_SYSAD_CompanyMasterRow(ObjCompanyMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;
            objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            intErrCode = objCompanyMasterClient.FunPubDeleteCompany(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_CompanyMasterDataTable, SerMode));
            if (intErrCode == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Error During Company deletion');", true);

            }
            else if (intErrCode == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Delete", "alert('Company details deleted successfully');window.location.href='../System Admin/S3GSysAdminCompanyMaster_View.aspx';", true);
            }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            if (objCompanyMasterClient != null)
            {
                objCompanyMasterClient.Close();
            }
        }
    }
    /// <summary>
    /// Casncel butoon event to move back to View page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminCompanyMaster_View.aspx");
    }
    /// <summary>
    /// Clears all the record on confirmation 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtAccCurrency.Text = "";
        txtAddress.Text = "";
        txtAddress2.Text = "";
        txtCity.Text = "";
        txtState.Text = "";
        txtHeadName.Text = "";
        txtEmailId.Text = "";
        txtMobileNumber.Text = "";
        txtSystemAdminUser.Text = "";
        txtSystemAdminPwd.Attributes.Add("value", "");
        txtTeleNumber.Text = "";
        txtPinCode.Text = "";
        txtWebsite.Text = "";
        txtCompanyCode.Text = "";
        txtCompanyName.Text = "";
        ddlConstitutionalStatus.SelectedIndex = 0;
        txtCountry.Text = ""; ;
        chkboxActive.Checked = false;
        txtAddress1.Text = "";
        txtOtherCity.Text = "";
        txtCommunicationAdd.Text = "";
        txtOtherCountry.Text = "";
        txtDateOfIncorp.Text = "";
        txtPanNumber.Text = "";
        txtRegNumber.Text = "";
        txtCSTNo.Text = "";
        txtSTNo.Text = "";
        txtRemarks.Text = "";
        txtOtherState.Text = "";
        txtValdityOfRegNumber.Text = "";
        txtOtherPin.Text = "";
        //foreach (GridViewRow grow in gvVATTIN.Rows)
        //{
        //    DropDownList ddlState = (DropDownList)grow.FindControl("ddlState");
        //    TextBox txtVATTIN = (TextBox)grow.FindControl("txtVATTIN");
        //    ddlState.SelectedIndex = 0;
        //    txtVATTIN.Text = "";
        //}
    }
    /// <summary>
    /// Event for Basic details Country name since we need to enable and disable 
    /// PAN Number Masked Editor for Countries other than India
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtCountry_TextChanged(object sender, EventArgs e)
    {
        if (txtCountry.Text.Trim().ToLower() != "india")
        {
            //mskexPanNumber.Enabled = false;
            revtxtPanNumber.ValidationExpression = @"^[a-zA-Z_0-9](\w|\W)*";
            revtxtPanNumber.ErrorMessage = "Enter a valid Income Tax Number";
        }
        else
        {
            //mskexPanNumber.Enabled = true;
            revtxtPanNumber.ValidationExpression = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
            revtxtPanNumber.ErrorMessage = "Income Tax/PAN Number should be of format AAAAA9999A";

        }
        txtPanNumber.Attributes.Add("maxlength", "20");
    }


    #endregion

    #region Page Methods

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
            txtCity.FillDataTable(dtSource, "Name", "Name", false);
            txtOtherCity.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 2").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 2").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtState.FillDataTable(dtSource, "Name", "Name", false);
            txtOtherState.FillDataTable(dtSource, "Name", "Name", false);

            dtSource = new DataTable();
            if (dtAddr.Select("Category = 3").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtCountry.FillDataTable(dtSource, "Name", "Name", false);
            txtOtherCountry.FillDataTable(dtSource, "Name", "Name", false);
            //txtStateName.BindDataTable("S3G_SYSAD_GetAddressLoodup", Procparam, new string[] { "ID", "Name" });
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            if (objCompanyMasterClient != null)
            {
                objCompanyMasterClient.Close();
            }
        }
    }

    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("Category");

        return dt;
    }

    /// <summary>
    /// This method is used to display product details
    /// </summary>
    private void FunPriGetCompanyDetails()
    {
        try
        {
            CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewRow ObjCompanyMasterRow;
            ObjCompanyMasterRow = ObjS3G_CompanyMaster_ViewDataTable.NewS3G_SYSAD_CompanyMaster_ViewRow();
            ObjCompanyMasterRow.Company_ID = intCompanyId;
            ObjS3G_CompanyMaster_ViewDataTable.AddS3G_SYSAD_CompanyMaster_ViewRow(ObjCompanyMasterRow);
            objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
            // FunPriGetPinCode(intCompanyId);
            byte[] byteCompanyDetails = objCompanyMasterClient.FunPubQueryCompany(SerMode, ClsPubSerialize.Serialize(ObjS3G_CompanyMaster_ViewDataTable, SerMode));
            ObjS3G_CompanyMaster_ViewDataTable = (CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteCompanyDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable));

            #region Tab1 Controls (Basic Details)
            txtCompanyCode.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Company_Code"].ToString();
            txtCompanyName.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Company_Name"].ToString();
            txtAddress.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Address1"].ToString();
            txtAddress2.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Address2"].ToString();
            txtCity.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["City"].ToString();
            txtState.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["State"].ToString();
            txtCountry.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Country"].ToString();
            if (txtCountry.Text.Trim().ToLower() != "india")
            {
                //mskexPanNumber.Enabled = false;
                revtxtPanNumber.ValidationExpression = @"^[a-zA-Z_0-9](\w|\W)*";
                revtxtPanNumber.ErrorMessage = "Enter a valid Income Tax Number";
            }
            else
            {
                revtxtPanNumber.ValidationExpression = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
                revtxtPanNumber.ErrorMessage = "Income Tax/PAN Number should be of format AAAAA9999A";

            }
            txtCountry.Attributes.Add("maxlength", "20");

            txtPinCode.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Zip_Code"].ToString();
            ddlConstitutionalStatus.SelectedValue = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Constitutional_Status_Id"].ToString();
            if (ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Active"].ToString() == "True")
                chkboxActive.Checked = true;
            else
                chkboxActive.Checked = false;

            //hdnUserId.Value = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Created_By"].ToString();
            //hdnUserLevelId.Value = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["User_Level_ID"].ToString();
            #endregion

            #region Tab2 Controls (Corporate Details)
            txtHeadName.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CD_CEO_Head_Name"].ToString();
            txtMobileNumber.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CD_Mobile_Number"].ToString();
            txtTeleNumber.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CD_Telephone_Number"].ToString();
            txtEmailId.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CD_Email_ID"].ToString();
            txtWebsite.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CD_Website"].ToString();
            txtSystemAdminUser.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CD_Sys_Admin_User_Code"].ToString();
            //Added Inorder to set text for text box for which text mode is Password
            txtSystemAdminPwd.Attributes.Add("value", ClsPubCommCrypto.DecryptText(ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CD_Sys_Admin_User_Password"].ToString()));
            #endregion

            #region Tab3 Controls (Other Details)
            txtCommunicationAdd.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Communication_Address"].ToString();
            txtAddress1.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Address1"].ToString();
            txtOtherCity.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_City"].ToString();
            txtOtherState.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_State"].ToString();
            txtOtherCountry.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Country"].ToString();

            txtOtherPin.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Zip_Code"].ToString();
            txtDateOfIncorp.Text = DateTime.Parse(ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Date_Of_Incorporation"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            if (string.IsNullOrEmpty(txtDateOfIncorp.Text) == false)
            {
                CalendarExtender1.Enabled = false;
            }
            txtRegNumber.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Reg_Lic_Number"].ToString();
            txtCSTNo.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CST_NO"].ToString();
            txtSTNo.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["ST_NO"].ToString();
            txtValdityOfRegNumber.Text = DateTime.Parse(Convert.ToString((ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Validity_Of_Reg_Lic_Number"])), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            txtPanNumber.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Income_Tax_PAN_Number"].ToString();
            txtAccCurrency.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["Accounting_Currency"].ToString();
            txtRemarks.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["OD_Remarks"].ToString();
            if (!string.IsNullOrEmpty(ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CGST_Reg_Date"].ToString()))
                TxtCGSTRegDate.Text = DateTime.Parse(Convert.ToString((ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CGST_Reg_Date"])), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            txtCGSTin.Text = ObjS3G_CompanyMaster_ViewDataTable.Rows[0]["CGSTIN"].ToString();

            //DataSet dsVATTIN = new DataSet();
            //string strProcName = "S3G_Get_Company_VATTIN";
            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", intCompanyId.ToString());
            //dsVATTIN = Utility.GetTableValues(strProcName, Procparam);
            //if (dsVATTIN.Tables[0].Rows.Count > 0)
            //{
            //    ViewState["CompanyVATTIN"] = dsVATTIN.Tables[0];
            //    gvVATTIN.DataSource = dsVATTIN.Tables[0];
            //    gvVATTIN.DataBind();
            //}
            //else
            //{
            //    DataTable dt = new DataTable();
            //    DataRow dr;

            //    dt.Columns.Add("SNo");
            //    dt.Columns.Add("State");
            //    dt.Columns.Add("STATE_ID");
            //    dt.Columns.Add("VATTIN");
            //    dr = dt.NewRow();
            //    dt.Rows.Add(dr);
            //    ViewState["CompanyVATTIN"] = dt;
            //    gvVATTIN.DataSource = dt;
            //    gvVATTIN.DataBind();
            //}
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            if (objCompanyMasterClient != null)
            {
                objCompanyMasterClient.Close();
            }
        }
    }

    //protected void gvVATTIN_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        DropDownList ddlState = (DropDownList)e.Row.FindControl("ddlState");
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        if (intCompanyId > 0)
    //        {
    //            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
    //        }
    //        DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetStateLookup", Procparam);

    //        Utility.FillDataTable(ddlState, dtAddr, "Location_Category_ID", "LocationCat_Description");
    //    }
    //}

    //protected void gvVATTIN_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Add")
    //    {
    //        DataTable dtVATTIN = (DataTable)ViewState["CompanyVATTIN"];
    //        DataRow dRow;
    //        string find = "VATTIN = ''";
    //        DataRow[] foundRows = dtVATTIN.Select(find);

    //        if (foundRows.Length > 0)
    //        {
    //            foreach (DataRow drow1 in foundRows)
    //            {
    //                dtVATTIN.Rows.Remove(drow1);
    //            }
    //        }
            
    //        DropDownList ddlState = (DropDownList)gvVATTIN.FooterRow.FindControl("ddlState");
    //        TextBox txtVATTIN = (TextBox)gvVATTIN.FooterRow.FindControl("txtVATTIN");
    //        if (ddlState.SelectedValue == "0")
    //        {
    //            Utility.FunShowAlertMsg(this, "Select State");
    //            return;
    //        }

    //        if (txtVATTIN.Text.Trim() == "")
    //        {
    //            Utility.FunShowAlertMsg(this, "Enter VAT-TIN value for the selected State");
    //            return;
    //        }

    //        if (txtVATTIN.Text.Length != 9)
    //        {
    //            Utility.FunShowAlertMsg(this, "VAT-TIN value should be of 9 characters");
    //            return;
    //        }
    //        string find1 = "State = '" + ddlState.SelectedItem.ToString() + "'";
    //        DataRow[] foundRows1 = dtVATTIN.Select(find1);

    //        if (foundRows1.Length > 0)
    //        {
    //            Utility.FunShowAlertMsg(this, "State already exists");
    //            return;
    //        }

    //        DataTable dtable = new DataTable();
    //        dtable = dtVATTIN.Clone();

    //        dRow = dtable.NewRow();
    //        dRow["STATE"] = ddlState.SelectedItem.ToString();
    //        dRow["STATE_ID"] = Convert.ToInt32(ddlState.SelectedValue);
    //        dRow["VATTIN"] = txtVATTIN.Text.Trim();
    //        //dRow["SNo"] = intSNo;
    //        dtable.Rows.Add(dRow);
    //        dtVATTIN.Merge(dtable);
    //        gvVATTIN.DataSource = dtVATTIN;
    //        gvVATTIN.DataBind();
    //        ViewState["CompanyVATTIN"] = dtVATTIN;
    //    }
    //}

    //private void FunPriGetPinCode(int intCompany_Id)
    //{
    //    S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;

    //    try
    //    {
    //        Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
    //        dictProcParam.Add("@Company_ID", intCompany_Id.ToString());
    //        ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
    //        byte[] byteGlobalParamDetails = ObjAdminService.FunPubFillDropdown(SPNames.S3G_Get_GlobalParam_Details, dictProcParam);
    //        DataTable dtGlboalDetails = (DataTable)ClsPubSerialize.DeSerialize(byteGlobalParamDetails, SerMode, typeof(DataTable));
    //        strType = dtGlboalDetails.Rows[0]["Pincode_Field_Type"].ToString();
    //        txtOtherPin.MaxLength = Convert.ToInt32(dtGlboalDetails.Rows[0]["Pincode_Digits"].ToString());
    //        txtPinCode.MaxLength = Convert.ToInt32(dtGlboalDetails.Rows[0]["Pincode_Digits"].ToString());
    //    }
    //    catch (Exception)
    //    {
    //    }
    //    finally
    //    {
    //        ObjAdminService.Close();
    //    }
    //}

    //private bool FunPriCheckValidPinCode(string strPINCode, string strType)
    //{

    //    try
    //    {
    //        if (strPINCode.Length < txtOtherPin.MaxLength || strPINCode.Length > txtOtherPin.MaxLength)
    //        {
    //            return false;
    //        }



    //        string strUpper = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    //        string strLower = @"abcdefghijklmnopqrstuvwxyz";
    //        string strNo = "0123456789";

    //        char[] chArray = strPINCode.ToCharArray();

    //        bool blnUpperResult = false;
    //        bool blnLowerResult = false;
    //        bool blnNoResult = false;

    //        foreach (char ch in chArray)
    //        {

    //            blnUpperResult = (blnUpperResult == false) ? strUpper.IndexOf(ch) > 0 : blnUpperResult;
    //            blnLowerResult = (blnLowerResult == false) ? strLower.IndexOf(ch) > 0 : blnLowerResult;
    //            blnNoResult = (blnNoResult == false) ? strNo.IndexOf(ch) > 0 : blnNoResult;

    //        }
    //        switch (strType)
    //        {
    //            case "Alpha Numeric":
    //                blnResult = blnUpperResult | blnLowerResult | blnNoResult;
    //                break;
    //            case "Numeric":
    //                if (blnNoResult)
    //                {
    //                    if (blnUpperResult & blnLowerResult)
    //                    {
    //                        blnResult = false;
    //                    }
    //                    else blnResult = true;
    //                }
    //                else blnResult = false;
    //                break;
    //            default:
    //                blnResult = true;
    //                break;
    //        }

    //    }
    //    catch (Exception)
    //    {
    //        blnResult = false;
    //    }
    //    return blnResult;
    //}

    /// <summary>
    /// Checks weather the string has all zeors by converting to integer
    /// </summary>
    /// <param name="strVal">Gets the string needs to be checked for e.g. Textbox.Text can be an input</param>
    /// <returns>return true if its an Alpha numeric on number > 0 else return false</returns>
    private bool CheckZeroValidate(string strVal)
    {
        try
        {
            int val = Convert.ToInt32(strVal);
            if (val != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return true;
        }
    }
            #endregion


    #region MCompany

    private void FunPriCompany()
    {
        if (intCompanyId > 0)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
            txtCompanyCode.ReadOnly = true;
            txtCompanyCode.Enabled = false;
            btnDelete.Enabled = true;
            btnClear.Enabled = false;
            FunPriGetCompanyDetails();
            txtPinCode.ReadOnly = false;
            txtOtherPin.ReadOnly = false;
            // chkboxActive.Enabled = true;
            rfvPIN.Enabled = true;
            revPIN.Enabled = true;
            rfvOtherPin.Enabled = true;
            revOtherPIN.Enabled = true;
            CalendarExtender1.Enabled = false;

            if (ObjS3GSession.ProPINCodeDigitsRW > 0)
            {
                txtPinCode.MaxLength = ObjS3GSession.ProPINCodeDigitsRW;
                txtOtherPin.MaxLength = ObjS3GSession.ProPINCodeDigitsRW;

                revPIN.ValidationExpression = "\\d{" + ObjS3GSession.ProPINCodeDigitsRW + "}";
                revPIN.ErrorMessage = " PIN Code/ZIP Code Should be " +
                    ObjS3GSession.ProPINCodeDigitsRW + " Digits ";

                revOtherPIN.ValidationExpression = "\\d{" + ObjS3GSession.ProPINCodeDigitsRW + "}";
                revOtherPIN.ErrorMessage = " PIN Code/ZIP Code Should be " +
                    ObjS3GSession.ProPINCodeDigitsRW + " Digits in Other Details tab ";

                if (ObjS3GSession.ProPINCodeTypeRW.ToUpper() == "ALPHA NUMERIC")
                {
                    ftePIN.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                    fteOtherPIN.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                }
                else
                {
                    ftePIN.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                    fteOtherPIN.FilterType = AjaxControlToolkit.FilterTypes.Numbers;

                }
            }
        }
        else
        {
            FunPriDisableControls(0);

        }

    }

    #endregion

    #region FieldDisable

    private void FunPriDisableControls(int intMode)
    {
        switch (intMode)
        {

            case 0: //Create

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                txtCompanyCode.ReadOnly = false;
                ddlConstitutionalStatus.SelectedIndex = 0;
                chkboxActive.Checked = true;
                btnDelete.Enabled = false;
                btnClear.Enabled = true;
                txtPinCode.ReadOnly = true;
                txtOtherPin.ReadOnly = true;
                //mskexPanNumber.Enabled = false;
                chkboxActive.Checked = true;
                chkboxActive.Enabled = false;
                rfvPIN.Enabled = false;
                revPIN.Enabled = false;
                rfvOtherPin.Enabled = false;
                revOtherPIN.Enabled = false;
                //foreach (GridViewRow grow in gvVATTIN.Rows)
                //{
                //    DropDownList ddlState = (DropDownList)grow.FindControl("ddlState");
                //    TextBox txtVATTIN = (TextBox)grow.FindControl("txtVATTIN");
                //    ddlState.Enabled = true;
                //    txtVATTIN.Enabled = true;
                //}
                //gvVATTIN.FooterRow.Visible = true;
                break;
            case 1:
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                txtDateOfIncorp.ReadOnly = true;
                break;
            case -1: //Query
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                ListItem lit;
                lit = new ListItem(ddlConstitutionalStatus.SelectedItem.Text, ddlConstitutionalStatus.SelectedItem.Value);
                ddlConstitutionalStatus.Items.Clear();
                ddlConstitutionalStatus.Items.Add(lit);
                txtCompanyCode.Enabled = true;
                txtCompanyCode.ReadOnly = true;
                txtCompanyName.ReadOnly = true;
                btnDelete.Visible = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                txtPinCode.ReadOnly = true;
                txtOtherPin.ReadOnly = true;
                txtAddress.ReadOnly = true;
                txtAddress2.ReadOnly = true;

                txtCity.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                txtCity.ClearDropDownList();
                //txtCity.ReadOnly = true; 
                txtState.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                txtState.ClearDropDownList();
                //txtState.ReadOnly = true;
                txtCountry.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                txtCountry.ClearDropDownList();
                //txtCountry.ReadOnly = true;

                txtPinCode.ReadOnly = true;
                ddlConstitutionalStatus.Enabled = true;

                txtHeadName.ReadOnly = true;
                txtMobileNumber.ReadOnly = true;
                txtTeleNumber.ReadOnly = true;
                txtEmailId.ReadOnly = true;
                txtWebsite.ReadOnly = true;
                txtSystemAdminUser.ReadOnly = true;
                txtSystemAdminPwd.ReadOnly = true;
                chkboxActive.Enabled = false;
                txtCommunicationAdd.ReadOnly = true;
                txtAddress1.ReadOnly = true;
                //txtOtherCity.ReadOnly = true;
                txtOtherCity.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                txtOtherCity.ClearDropDownList();
                //txtOtherState.ReadOnly = true;
                txtOtherState.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                txtOtherState.ClearDropDownList();
                //txtOtherCountry.ReadOnly = true;
                txtOtherCountry.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
                txtOtherCountry.ClearDropDownList();

                txtOtherPin.ReadOnly = true;
                txtDateOfIncorp.ReadOnly = true;
                txtDateOfIncorp.Attributes.Remove("onblur");
                txtRegNumber.ReadOnly = true;
                txtCSTNo.ReadOnly = true;
                txtSTNo.ReadOnly = true;
                txtValdityOfRegNumber.ReadOnly = true;
                txtValdityOfRegNumber.Attributes.Remove("onblur");
                txtPanNumber.ReadOnly = true;
                txtAccCurrency.ReadOnly = true;
                cexValdityOfRegNumber.Enabled = false;
                CalendarExtender1.Enabled = false;
                txtRemarks.ReadOnly = true;
                //foreach (GridViewRow grow in gvVATTIN.Rows)
                //{
                //    DropDownList ddlState = (DropDownList)grow.FindControl("ddlState");
                //    TextBox txtVATTIN = (TextBox)grow.FindControl("txtVATTIN");
                //    ddlState.Enabled = false;
                //    txtVATTIN.Enabled = false;
                //}
                //gvVATTIN.FooterRow.Visible = false;
                break;
        }
    }
    #endregion
    #endregion
}