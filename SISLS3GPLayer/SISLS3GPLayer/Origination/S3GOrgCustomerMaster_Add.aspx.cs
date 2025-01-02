#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: Customer Master
/// Created By			: Prabhu K
/// Created Date		: 22-July-2010
/// Purpose	            : To Add/Edit the Customer
/// 



/// Module Name     :   Origination
/// Screen Name     :   Customer Master
/// Modified By     :   Swarna S
/// Created Date    :   19-12-2014
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
using ORG = S3GBusEntity.Origination;
using ORGSERVICE = OrgMasterMgtServicesReference;
using System.Collections;
using System.Globalization;
using System.Configuration;
using System.Web;
using System.Security.Permissions;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
#endregion

public partial class Origination_S3GOrgCustomerMaster_Add : ApplyThemeForProject
{
    #region Initialization

    #region Variables
    //int ErrorCode;
    int intCompanyId = 0;
    int intUserId = 0;
    int intCustomerId = 0;
    int intCRMID = 0;
    string strCustomerCode;
    Dictionary<string, string> Procparam = null;
    //User Authorization
    string strMode = string.Empty;
    bool bClearList = false;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    //Code end
    public string strDateFormat;
    string strKey = "Insert";
    static string strPageName = "Customer Master";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgCustomerMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgCustomerMaster_Add.aspx';";
    public static Origination_S3GOrgCustomerMaster_Add obj_Page;
    DataTable dtContractNos = null; DataTable dtSubContractNos = null;
    string strGST;
    #endregion

    #region Objects
    S3GSession ObjS3GSession;
    UserInfo ObjUserInfo;
    StringBuilder strbBnkDetails = new StringBuilder();
    PagingValues ObjPaging = new PagingValues();
    #endregion

    #endregion
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
        FunPriBindGrid();
    }

    #region PageLoad

    /// <summary>
    /// Event for Display the UI to Add/Edit the Customer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
          
           if (!IsPostBack)
            {
                tcCustomerMaster.ActiveTab = tbAddress;
                FunProIntializeData();
                FunProClearCaches();
               
                // FunPriLoadRelationType();
            }

            obj_Page = this;

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            ftxtBankAddress.ValidChars += "\n";
            ftxtBankBranch.ValidChars += "\n";
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end


           
            //arrSearchVal = new ArrayList(intNoofSearch);
            ProPageNumRW = 1;

            
            bIsActive = ObjUserInfo.ProIsActiveRW;
            //bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
            //Authorization Code end

            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

            CalendarExtender2.Format = strDateFormat;
            CalendarExtender3.Format = strDateFormat;
           


            string strFormat = ObjS3GSession.ProDateFormatRW;
            //txtCEOWeddingDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCEOWeddingDate.ClientID + "','" + strFormat + "',true,false);");
            //<<Performance>>
            FunSetComboBoxAttributes(txtComCity.TextBox, "City", "30");
            //FunSetComboBoxAttributes(ddlComState, "State", "60");
            FunSetComboBoxAttributes(txtComCountry, "Country", "60");

            //<<Performance>>
            FunSetComboBoxAttributes(txtPerCity.TextBox, "City", "30");
            TxtSGSTRegDate.Attributes.Add("onblur", "fnDoDategetAge(this,'" + TxtSGSTRegDate.ClientID + "','" + strDateFormat + "',true,  false);");
            TxtCGSTRegDate.Attributes.Add("onblur", "fnDoDategetAge(this,'" + TxtCGSTRegDate.ClientID + "','" + strDateFormat + "',true,  false);");
            //FunSetComboBoxAttributes(txtPerState, "State", "60");
            //FunSetComboBoxAttributes(txtPerCountry, "Country", "60");

            //Added for Customer Creation through CRM on 26Sep2014 starts here 
            strGST = ClsPubConfigReader.FunPubReadConfig("Is_GST");//GST
            if (Request.QueryString["qsCRM_ID"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCRM_ID"));
                if (fromTicket != null)
                {
                    intCRMID = Convert.ToInt32(fromTicket.Name);
                }
            }


            //Added for Customer Creation through CRM on 26Sep2014 ends here 

            if (Request.QueryString["qsCustomerId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsCustomerId"));
                if (fromTicket != null)
                {
                    intCustomerId = Convert.ToInt32(fromTicket.Name);
                    ViewState["CustomerId"] = intCustomerId;
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Customer Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
                //Added on 08May2015 Starts here
                //Testing Observation fixed - Save button is in enabled mode when user try to view customer details from MRA
                if (Request.QueryString["IsFromApplication"] != null)
                {
                    btnSave.Visible = btnClear.Visible = btnCancel.Visible = btnExportToExcel.Visible = false;
                }
                //Added on 08May2015 ends here
            }
            if (ViewState["CustomerId"] != null)
            {
                intCustomerId = Convert.ToInt32(ViewState["CustomerId"]);
            }
            // txtPercentageStake.SetDecimalPrefixSuffix(2, 2, false, false, "Promoters Stake %");
            // txtJVPartnerStake.SetDecimalPrefixSuffix(2, 2, false, false, "JV Partner Stake %");
            if (ObjUserInfo.ProCountryNameR.Trim().ToLower() == "india")
            {
                ftxtMICRCode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                txtMICRCode.MaxLength = 9;
            }
            else
            {
                ftxtMICRCode.FilterType = AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.LowercaseLetters | AjaxControlToolkit.FilterTypes.UppercaseLetters;
                txtMICRCode.MaxLength = 10;
            }
            if (ObjS3GSession.ProPINCodeDigitsRW > 0)
            {
                txtComPincode.MaxLength = txtPerPincode.MaxLength = ObjS3GSession.ProPINCodeDigitsRW;
                if (ObjS3GSession.ProPINCodeTypeRW.ToUpper() == "ALPHA NUMERIC")
                {
                    ftxtComPincode.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                    ftxtPerPincode.FilterType = AjaxControlToolkit.FilterTypes.Numbers | AjaxControlToolkit.FilterTypes.Custom | AjaxControlToolkit.FilterTypes.UppercaseLetters | AjaxControlToolkit.FilterTypes.LowercaseLetters;
                }
                else
                {
                    ftxtComPincode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                    ftxtPerPincode.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                }
            }
            if (!IsPostBack)
            {
                FunPriSetControlSettings();

                //CalendarCEOWeddingDate.Format = ObjS3GSession.ProDateFormatRW;
                //CalendarExtenderSD.Format = ObjS3GSession.ProDateFormatRW;
                CalendarValidupto.Format = ObjS3GSession.ProDateFormatRW;
                // CalendarWeddingdate.Format = ObjS3GSession.ProDateFormatRW;

                ceReferenceDate.Format = ceValidFrom.Format = ceValidTo.Format = ObjS3GSession.ProDateFormatRW;

                FunPriLoadMasterData();
                FunProLoadAddressCombos();
                if (strMode != "Q")
                {
                    
                    FunProLoadAddressCombos();

                    FunPriLoadContractNos();
                }
                //txtDOB.Attributes.Add("onblur", "fnDoDate(this,'" + txtDOB.ClientID + "','" + strDateFormat + "',true,false);");
                //txtCEOWeddingDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCEOWeddingDate.ClientID + "','" + strDateFormat + "',true,false);");
                //  txtWeddingdate.Attributes.Add("onblur", "fnDoDate(this,'" + txtWeddingdate.ClientID + "','" + strDateFormat + "',true,false);");
                //User Authorization

                txtReferenceDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtReferenceDate.ClientID + "','" + strDateFormat + "',false,false);");
                txtValidfrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtValidfrom.ClientID + "','" + strDateFormat + "',false,false);");
                txtValidTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtValidTo.ClientID + "','" + strDateFormat + "',false,false);");

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (intCustomerId > 0)
                {
                    tabPODOMappings.Visible = true;
                    FunPriLoadPODOMappings();
                    if (strMode == "M")
                    {
                        FunPriDisableControls(1);
                        if (ddlCustomerType.SelectedItem.ToString().ToLower() == "non - corporate")
                        {
                           
                            //rfvPerEmail.Enabled = true;
                        }
                        else
                        {
                            txtIndustryName.Text = "";
                            cmbIndustryCode.Text = "";
                            
                            //rfvPerEmail.Enabled = false;
                        }
                    }
                    else
                    {
                        FunPriDisableControls(-1);
                    }

                }
                else
                {
                    tabPODOMappings.Visible = false;
                    FunPriDisableControls(0);
                }

                if (Request.QueryString["IsFromEnquiry"] != null || Request.QueryString["IsFromApplication"] != null)
                {
                    chkCoApplicant.Enabled = chkGuarantor1.Enabled = chkGuarantor2.Enabled = false;

                    if (Request.QueryString["EnquiryID"] != null)
                    {
                        int intEnquiryID = Convert.ToInt32(Request.QueryString.Get("EnquiryID"));
                        FunPriLoadEnquiryDetails(intEnquiryID);
                    }
                }

            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            //cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.PageLoadError;
            //cvCustomerMaster.IsValid = false;
        }
    }

    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ExportExcel();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }


    private void ExportExcel()
    {
        try
        {
            DataTable dtGetdata = new DataTable();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@CompanyId", obj_Page.intCompanyId.ToString());
            Procparam.Add("@CustomerId", intCustomerId.ToString());
            dtGetdata = Utility.GetDefaultData("[s3g_org_LoadCustomerCreditDetails_Export]", Procparam);

            GridView Grv = new GridView();
            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            

            if (Grv.Rows.Count > 0)
            {
                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;
                string attachment = "attachment; filename=Customer_Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");
                dtHeader.Columns.Add("Column2");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Customer Credit Details";
                dtHeader.Rows.Add(row);
                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();
                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 12;
                grv1.Rows[1].Cells[0].ColumnSpan = 12;
                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;
                grv1.RenderControl(htw);
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
                
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No data to export");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }


    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
            textBox.Attributes.Add("onpaste", "return false");
        }
    }

    protected void FunSetComboBoxAttributes(TextBox textBox, string Type, string maxLength)
    {
        if (textBox != null)
        {
            textBox.Attributes.Add("onkeyup", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
            textBox.Attributes.Add("onpaste", "return false");
        }
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetCityList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Category", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup_AGT", Procparam), false);

        return suggetions.ToArray();
    }

    protected void FunProLoadStateCombos()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "506");
            Procparam.Add("@Param1", Convert.ToString(intCustomerId));
            DataTable dtState = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);
            Utility.FillDataTable(ddlInvCovLetter, dtState, "Cust_Address_ID", "State_Name", false);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = ex.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void FunProLoadAddressCombos()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetStateLookup", Procparam);



            //<<Performance>>
            //txtComCity.FillDataTable(dtSource, "Name", "Name", false);
            //txtPerCity.FillDataTable(dtSource, "Name", "Name", false);

            Utility.FillDataTable(ddlComState, dtAddr, "Location_Category_ID", "LocationCat_Description");
            Utility.FillDataTable(ddlPerState, dtAddr, "Location_Category_ID", "LocationCat_Description");
            Utility.FillDataTable(ddlStateGroup, dtAddr, "Location_Category_ID", "LocationCat_Description");


            DataTable dtSource = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);


            if (dtSource.Select("Category = 3").Length > 0)
            {
                dtSource = dtSource.Select("Category = 3").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }
            txtComCountry.FillDataTable(dtSource, "Name", "Name", false);
            // txtPerCountry.FillDataTable(dtSource, "Name", "Name", false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = ex.Message;
            cvCustomerMaster.IsValid = false;
        }
    }


    private void FunPriLoadContractNos()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Customer_Id", intCustomerId.ToString());
            DataSet dsContractNos = Utility.GetDataset("S3G_ORG_Get_PANum", Procparam);
            DataTable dtSANUM = new DataTable();
            dtContractNos = dsContractNos.Tables[0].Copy();
            dtSANUM = dsContractNos.Tables[1].Copy();
            if (dtContractNos.Rows.Count > 0)
            {
                ddlRSNum.FillDataTable(dtContractNos, "PANum", "PANum");
               
                ViewState["dsContractNos"] = dsContractNos;
            }
            Procparam.Clear();


        }
        catch (Exception ex)
        {

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
    /// Event for Pre-Initialize the Components  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }


    protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["IsFromEnquiry"] != null || Request.QueryString["IsFromApplication"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Initialize the Customer Details";
            cvCustomerMaster.IsValid = false;
        }
    }


    #endregion

    #region Button Events

    /// <summary>
    /// Event for Insert/Update the CustomerDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strPassport;
            string strNational;

            bool IsMandatory = false;
            bool IsNeedImageCopy = false;

            Procparam = new Dictionary<string, string>();

            Procparam.Add("@CustomerName", txtCustomerName.Text.ToString());

            foreach (GridViewRow grv in gvConstitutionalDocuments.Rows)
            {
                TextBox txtValues = (TextBox)grv.FindControl("txtValues");

                string strDocumentFlag = grv.Cells[1].Text;
                if (strDocumentFlag.ToUpper() == "CID-PP")
                {
                    strPassport = txtValues.Text.ToString();
                    Procparam.Add("@PassportNo", strPassport);
                }

                else if (strDocumentFlag.ToUpper() == "CID-UID")
                {
                    strNational = txtValues.Text;
                    Procparam.Add("@NationalIdentificationNo", strNational.ToString());
                }

            }

            //txtValues.Text = Convert.ToDecimal(dt.Compute("Sum(Tran_Currency_Amount1)", "")).ToString(FunSetSuffix());



            DataTable dtNeg = Utility.GetDefaultData("NegativeListComparison", Procparam);

            if (dtNeg.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtNeg.Rows[0]["ErrorCode"].ToString()) > 0)
                {
                    Utility.FunShowAlertMsg(this, "The entered Details exists in the Negative Customer List");
                    return;
                }

            }
            string strDocName = "";
            string strConstitutionAlert = "";
            string strDeDupCustomerCode = "";
            //if (FunPriValidateDeDuplicateCustomer(out strDocName, out strDeDupCustomerCode))
            //{
            //    strDocName = strDocName.Substring(0, strDocName.Length - 1);
            //    strConstitutionAlert = "alert('__ALERT__');";
            //    if (strDeDupCustomerCode != "")
            //    {
            //        strDeDupCustomerCode = strDeDupCustomerCode.Substring(0, strDeDupCustomerCode.Length - 1);
            //        strDeDupCustomerCode.Replace(",", @"\n");
            //        strConstitutionAlert = strDocName + @" already Exist for following Customer \n" + strDeDupCustomerCode;
            //    }
            //    else
            //    {
            //        strConstitutionAlert = strDocName + @" already Exist for another Customer";
            //    }
            //    strConstitutionAlert += @"\n\nWould you like to proceed?";
            //    strConstitutionAlert = "if(confirm('" + strConstitutionAlert + "')){ document.getElementById('ctl00_ContentPlaceHolder1_btnProceedSave').click();}else{;" + strRedirectPageView + "}";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cusotmer Master", strConstitutionAlert, true);
            //    return;
            //}
            strDocName = "";
            if (FunPriValidateDuplicateConstitutionDocs(out strDocName))
            {
                strDocName = strDocName.Substring(0, strDocName.Length - 1);
                strConstitutionAlert = "alert('__ALERT__');";
                strConstitutionAlert = strDocName + " already Exist for another Customer";
                strAlert = strAlert.Replace("__ALERT__", strConstitutionAlert);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Master", strAlert, true);
                //strConstitutionAlert += @"\n\nWould you like to proceed?";
                //strConstitutionAlert = "if(confirm('" + strConstitutionAlert + "')){ document.getElementById('ctl00_ContentPlaceHolder1_btnProceedSave').click();}else{;" + strRedirectPageView + "}";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Cusotmer Master", strConstitutionAlert, true);
                return;
            }

            if (chkCustomer.Checked != true && chkGuarantor1.Checked != true && chkGuarantor2.Checked != true && chkCoApplicant.Checked != true)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Select atleast one Relation Type(Customer/Guarantor1/Guarantor2/Co-Applicant)');", true);
                tcCustomerMaster.ActiveTab = tbAddress;
                return;
            }

            if (cmbGroupCode.Text.Trim() != "" && txtGroupName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter Group Name');", true);
                tcCustomerMaster.ActiveTab = tbAddress;
                txtGroupName.Focus();
                return;
            }

            if (cmbIndustryCode.Text.Trim() != "" && cmbIndustryCode.Text.Trim() != "0" && txtIndustryName.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter Industry Name');", true);
                tcCustomerMaster.ActiveTab = tbAddress;
                txtIndustryName.Focus();
                return;
            }


              
            var withoutSpecial = new string(txtComCountry.Text.Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c)).ToArray());

             if (txtComCountry.Text != withoutSpecial)
            {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Special Characters Not allowed in Country field');", true);
                    tcCustomerMaster.ActiveTab = tbAddress;
                    txtComCountry.Focus();
                    return;
            }


             //if (ObjS3GSession.ProPINCodeDigitsRW != txtComPincode.Text.Trim().Length)
             //{
             //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Corporate address PIN should be " + ObjS3GSession.ProPINCodeDigitsRW.ToString() + " digits');", true);
             //    tcCustomerMaster.ActiveTab = tbAddress;
             //    return;
             //}

           //if (txtComWebsite.Text.Trim()=="")
           //{
           //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Website is mandatory in Corporate Address');", true);
           //    tcCustomerMaster.ActiveTab = tbAddress;
           //    return;
           //}


           //string PANExpression = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
           //if (!Regex.IsMatch(txtPAN.Text.Trim(), PANExpression))
           //{
           //    Utility.FunShowAlertMsg(this, "Permanent Tax Number should be in format of AAAAA9999A");
           //    tcCustomerMaster.ActiveTab = tbAddress;
           //    txtPAN.Focus();
           //    return;
           //}

            //if (txtFamilyName.Text.Trim() == "")
            //{
            //    Utility.FunShowAlertMsg(this, "Short Name is mandatory");
            //    tcCustomerMaster.ActiveTab = tbAddress;
            //    txtFamilyName.Focus();
            //    return;
            //}

            //if (ddlPostingGroup.SelectedValue == "-1")
            //{
            //    Utility.FunShowAlertMsg(this, "Please select a posting group");
            //    tcCustomerMaster.ActiveTab = tbAddress;
            //    ddlPostingGroup.Focus();
            //    return;
            //}


            if (txtCIN.Text != "")
            {
                if (txtCIN.Text.Length != 21)
                {
                    Utility.FunShowAlertMsg(this, "CIN number should be of 21 characters in Corporate Address");
                    tcCustomerMaster.ActiveTab = tbAddress;
                    txtCIN.Focus();
                    return;
                }
            }


            //if (txtTIN.Text != "")
            //{
            //    if (txtTIN.Text.Length != 9)
            //    {
            //        Utility.FunShowAlertMsg(this, "TIN number should be of 9 characters in Corporate Address");
            //        tcCustomerMaster.ActiveTab = tbAddress;
            //        txtTIN.Focus();
            //        return;
            //    }
            //}


            //if (txtTAN.Text.Trim() == "")
            //{
            //    Utility.FunShowAlertMsg(this, "TAN number is mandatory in Corporate Address");
            //    tcCustomerMaster.ActiveTab = tbAddress;
            //    txtTAN.Focus();
            //    return;
            //}
            if (txtTAN.Text.Trim() != "")
            {
                if (txtTAN.Text.Length != 10)
                {
                    Utility.FunShowAlertMsg(this, "TAN number should be of 10 characters in Corporate Address");
                    tcCustomerMaster.ActiveTab = tbAddress;
                    txtTAN.Focus();
                    return;
                }
            }

            if (txtPercentageStake.Text.Trim() != "")
            {
                if (Convert.ToDecimal(txtPercentageStake.Text.Trim()) > Convert.ToDecimal(100.00))
                {
                    Utility.FunShowAlertMsg(this, "Stake Percentage cannot exceed 100");
                    tcCustomerMaster.ActiveTab = tpPersonal;
                    txtPercentageStake.Focus();
                    return;
                }
            }
            else
            {
                txtPercentageStake.Text = "0";
            }
            


            //if (ddlCustomerType.SelectedItem.Text.ToUpper() != "INDIVIDUAL")
            //{
            //    if (ddlPublic.SelectedIndex == 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Select the Public/Closely held');", true);
            //        tcCustomerMaster.ActiveTabIndex = 1;
            //        return;
            //    }
            //}

            //if (txtDirectors.Text.Trim() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the No of directors/Partners');", true);
            //    tcCustomerMaster.ActiveTabIndex = 1;
            //    return;
            //}


            //if (txtBusinessProfile.Text.Trim() == "")
            //{
            //    Utility.FunShowAlertMsg(this, "Enter the Business Profile");
            //    tcCustomerMaster.ActiveTabIndex = 1;
            //    txtBusinessProfile.Focus();
            //    return;
            //}

          

            if (ddlCustomerType.SelectedItem.Text.ToUpper() == "INDIVIDUAL")
            {
                //if (txtDOB.Text == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Enter the Date of Birth');", true);
                //    tcCustomerMaster.ActiveTabIndex = 1;
                //    return;
                //}


            }
            //if (ddlCustomerType.SelectedItem.Text.ToUpper() != "INDIVIDUAL" && txtResidentialAddress.Text != "" && txtResidentialAddress.Text.Length > 300)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer Details", "alert('Residential Address Length should be less than or equal to 300');", true);
            //    tcCustomerMaster.ActiveTabIndex = 1;
            //    return;

            //}
            if (ViewState["ConsDocPath"] == null)
            {
                Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Constitution Document");
                return;
            }
            if (ViewState["ConsDocPath"] != null)
            {
                if (Convert.ToString(ViewState["ConsDocPath"]) == "")
                {
                    Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Constitution Document");
                    return;
                }
            }

            foreach (GridViewRow grv in gvConstitutionalDocuments.Rows)
            {
                //AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");
                //TextBox txOD = grv.FindControl("txOD") as TextBox;
                LinkButton hlnkView = grv.FindControl("hlnkView") as LinkButton;
                CheckBox chkIsMandatory = (CheckBox)grv.FindControl("chkIsMandatory");
                CheckBox chkIsNeedImageCopy = (CheckBox)grv.FindControl("chkIsNeedImageCopy");
                CheckBox chkCollected = (CheckBox)grv.FindControl("chkCollected");
                CheckBox chkScanned = (CheckBox)grv.FindControl("chkScanned");
                TextBox txtValues = grv.FindControl("txtValues") as TextBox;
                Label lblActualPath = (Label)grv.FindControl("lblActualPath");
                int intRowindex = grv.RowIndex;
                string fileExtension = string.Empty;
                if (chkScanned.Checked && Cache["File" + intRowindex.ToString()] == null && string.IsNullOrEmpty(lblActualPath.Text))
                {
                    Utility.FunShowAlertMsg(this, "All the scanned documents file should be uploaded.");
                    tcCustomerMaster.ActiveTab = TabConstitution;
                    return;
                }
                else
                {
                    if (Cache["File" + intRowindex.ToString()] != null)
                    {
                        HttpPostedFile hpf = (HttpPostedFile)Cache["File" + intRowindex.ToString()];
                        fileExtension = hpf.FileName;
                    }
                    else if (!string.IsNullOrEmpty(lblActualPath.Text))
                    {
                        fileExtension = lblActualPath.Text;
                    }

                }

                // Modified by R. Manikandan
                //Added Two Condition to check Mandatory Scanned Doc must only validated chkScanned.Checked == true && chkCollected.Checked == true

                if (!string.IsNullOrEmpty(fileExtension) && chkScanned.Checked == true && chkCollected.Checked == true)
                {
                    fileExtension = fileExtension.Substring(fileExtension.LastIndexOf('.') + 1);
                    if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                    {
                        Utility.FunShowAlertMsg(this, "Image/PDF file only can be uploaded. Check the file format of " + fileExtension + "");
                        return;
                    }

                }


                //string fileExtension = AsyncFileUpload1.FileName.Substring(AsyncFileUpload1.FileName.LastIndexOf('.') + 1);
                //if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
                //{
                //    Utility.FunShowAlertMsg(this, "Image/PDF file only can be uploaded. Check the file format of " + AsyncFileUpload1.FileName.ToString() + "");
                //    return;
                //}

                //if (chkIsMandatory.Checked && !chkCollected.Checked)
                //{
                //    Utility.FunShowAlertMsg(this, "All the mandatory documents should be collected.");
                //    tcCustomerMaster.ActiveTab = TabConstitution;
                //    return;
                //}
                //if (chkIsNeedImageCopy.Checked && (!chkScanned.Checked || (!hlnkView.Enabled && !AsyncFileUpload1.HasFile)))
                // Bug Fixed on 07 - DEC - 2012  Based On Bashyam sir Mail.

                //if (chkIsNeedImageCopy.Checked && (!hlnkView.Enabled && !AsyncFileUpload1.HasFile) && (chkIsMandatory.Checked))
                //{
                //    Utility.FunShowAlertMsg(this, "All the mandatory documents should be Uploaded.");
                //    tcCustomerMaster.ActiveTab = TabConstitution;
                //    return;
                //}


                // Code Added by Chandru K On 19-Jun-2014 For Mandatory document validation
                if (chkIsMandatory.Checked && chkCollected.Checked)
                    IsMandatory = true;
                //else if (chkIsMandatory.Checked && !chkCollected.Checked)
                //{
                //    Utility.FunShowAlertMsg(this, "Collect all mandatory documents");
                //    tcCustomerMaster.ActiveTab = TabConstitution;
                //    return;
                //}


                if (chkIsNeedImageCopy.Checked && chkScanned.Checked)
                    IsNeedImageCopy = true;
                // End

                if (chkCollected.Checked && (grv.Cells[1].Text.ToString().StartsWith("CID")) && txtValues.Text.Trim() == string.Empty)
                {
                    Utility.FunShowAlertMsg(this, "All the Collected documents values should be entered.");
                    tcCustomerMaster.ActiveTab = TabConstitution;
                    return;
                }
                
                if (grv.Cells[1].Text.ToString().ToUpper() == "CID-PTN")
                {
                    if (ObjS3GSession.ProCompanyCountry.Trim().ToLower() == "india" && txtValues.Text.Trim() != "")
                    {
                        string strExp = @"^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}$";
                        if (!Regex.IsMatch(txtValues.Text.Trim(), strExp))
                        {
                            Utility.FunShowAlertMsg(this, "Permanent Tax Number should be in format of AAAAA9999A");
                            tcCustomerMaster.ActiveTab = TabConstitution;
                            txtValues.Focus();
                            return;
                        }
                    }
                }
            }

            if (chkBadTrack.Checked == true && chkHotListed.Checked == true)
            {
                Utility.FunShowAlertMsg(this, "Select either black listed or hot listed.");
                tcCustomerMaster.ActiveTab = TabCustomerTrack;
                return;
            }

            //FunPriUploadFiles();
            FunProUploadFilesNew();
            FunPriSaveCustomer();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            if (intCustomerId > 0)
            {
                cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.UpdateError;
            }
            else
            {
                cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.InsertError;
            }
            cvCustomerMaster.IsValid = false;

        }
    }
    protected void btnProceedSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveCustomer();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            if (intCustomerId > 0)
            {
                cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.UpdateError;
            }
            else
            {
                cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.InsertError;
            }
            cvCustomerMaster.IsValid = false;
        }
    }
    private void FunPriSaveCustomer()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            if (cmbIndustryCode.Text == "")
            {
                cmbIndustryCode.Text = (0).ToString();
            }
            FunPriSetControlSettings();
            int intErrorCode = 0;
            CustomerMasterBusEntity ObjCustomerMaster = new CustomerMasterBusEntity();
            ObjCustomerMaster.ID = Convert.ToInt32(intCustomerId);
            ObjCustomerMaster.CustomerCode = txtCustomercode.Text.Trim();
            ObjCustomerMaster.CustomerType_ID = Convert.ToInt32(ddlCustomerType.SelectedValue);
            ObjCustomerMaster.Company_Type_ID = Convert.ToInt32(ddlCompanyType.SelectedValue);
            if (!string.IsNullOrEmpty(cmbGroupCode.Text.Trim()))
                ObjCustomerMaster.GroupCode = cmbGroupCode.Text.Trim();
            ObjCustomerMaster.Groupname = txtGroupName.Text.Trim();
            if (!string.IsNullOrEmpty(cmbIndustryCode.Text.Trim()))
                ObjCustomerMaster.IndustryCode = cmbIndustryCode.Text.Trim();
            ObjCustomerMaster.IndustryName = txtIndustryName.Text.Trim();
            if (!string.IsNullOrEmpty(ddlConstitutionName.Text.Trim())) ObjCustomerMaster.Constitution_ID = Convert.ToInt32(ddlConstitutionName.SelectedValue);
            ObjCustomerMaster.Title = ddlTitle.SelectedValue;
            ObjCustomerMaster.CustomerName = txtCustomerName.Text.Trim();
            if (!string.IsNullOrEmpty(ddlPostingGroup.SelectedValue)) ObjCustomerMaster.CustomerPostingGroupCode_ID = Convert.ToInt32(ddlPostingGroup.SelectedValue);
            ObjCustomerMaster.BillingAddress = ddlBillAddress.SelectedValue;
            ObjCustomerMaster.Comm_Address1 = txtComAddress1.Text.Trim();
            ObjCustomerMaster.Comm_Address2 = txtCOmAddress2.Text.Trim();
            ObjCustomerMaster.PAN = txtPAN.Text.Trim();
            ObjCustomerMaster.TAN = txtTAN.Text.Trim();
            ObjCustomerMaster.CIN = txtCIN.Text.Trim();
            ObjCustomerMaster.TIN = txtTIN.Text.Trim();
            ObjCustomerMaster.CGSTIN = txtCGSTin.Text.Trim();
            if (TxtCGSTRegDate.Text != "")
                ObjCustomerMaster.CGST_Reg_Date = Utility.StringToDate(TxtCGSTRegDate.Text.ToString());
            //<<Performance>>
            //ObjCustomerMaster.Comm_City = txtComCity.Text.Trim();
            ObjCustomerMaster.Comm_City = txtComCity.SelectedText.Trim();
            ObjCustomerMaster.Comm_State = ddlComState.SelectedValue.ToString().Trim();
            ObjCustomerMaster.Comm_Country = txtComCountry.SelectedItem.ToString();
            if (txtComPincode.Text.Trim() == "")
            {
                ObjCustomerMaster.Comm_PINCode = "0";
            }
            else
            {
                ObjCustomerMaster.Comm_PINCode = txtComPincode.Text.Trim();
            }
            
            ObjCustomerMaster.Comm_Mobile = txtComMobile.Text.Trim();
            ObjCustomerMaster.Comm_Telephone = txtComTelephone.Text.Trim();
            ObjCustomerMaster.Comm_Email = txtComEmail.Text.Trim();
            ObjCustomerMaster.Comm_Website = txtComWebsite.Text.Trim();
            ObjCustomerMaster.Perm_Address1 = txtPerAddress1.Text.Trim();
            ObjCustomerMaster.Perm_Address2 = txtPerAddress2.Text.Trim();
            // <<Performance>>
            ObjCustomerMaster.Perm_City = txtPerCity.SelectedText.Trim();
            ObjCustomerMaster.Perm_State = ddlPerState.SelectedValue.ToString().Trim();
            // ObjCustomerMaster.Perm_Country = txtPerCountry.SelectedItem.Text.Trim();

            if (txtPerPincode.Text.Trim() == "")
            {
                ObjCustomerMaster.Perm_PINCode = "0";
            }
            else
            {
                ObjCustomerMaster.Perm_PINCode = txtPerPincode.Text.Trim();

            }
            //ObjCustomerMaster.Perm_Mobile = txtPerMobile.Text.Trim();
            //ObjCustomerMaster.Perm_Telephone = txtPerTelephone.Text.Trim();
            ObjCustomerMaster.Perm_Email = txtPerEmail.Text.Trim();
            ObjCustomerMaster.Perm_TIN = txtPerTIN.Text.Trim();
            ObjCustomerMaster.Perm_TAN = txtPerTAN.Text.Trim();


            ObjCustomerMaster.Rental_ScheduleNo = ddlRSNum.SelectedValue.Trim();
            //ObjCustomerMaster.Tranche_No = ddlSANum.SelectedValue.Trim();

            if (ddlPublic.SelectedValue == "0" || ddlPublic.SelectedValue == "-1" || ddlPublic.SelectedValue == "")
            {
                ObjCustomerMaster.PublicCloselyheld_ID = 0;
                //if (!string.IsNullOrEmpty(ddlPublic.Text.Trim())) ObjCustomerMaster.PublicCloselyheld_ID = Convert.ToInt32(ddlPublic.SelectedValue);
            }
            else
            {
                ObjCustomerMaster.PublicCloselyheld_ID = Convert.ToInt32(ddlPublic.SelectedValue);
            }


            if (txtDirectors.Text.Trim() == "")
            {
                ObjCustomerMaster.NoOfDirectors = 0;
                //if (!string.IsNullOrEmpty(txtDirectors.Text.Trim())) ObjCustomerMaster.NoOfDirectors = Convert.ToInt32(txtDirectors.Text.Trim());
            }

            // ObjCustomerMaster.Perm_Website = txtPerWebSite.Text.Trim();
            
            
            ObjCustomerMaster.ListedAtStockExchange = txtSE.Text.Trim();
            if (!string.IsNullOrEmpty(txtPaidCapital.Text.Trim())) ObjCustomerMaster.PaidupCapital = Convert.ToDecimal(txtPaidCapital.Text.Trim());
            if (!string.IsNullOrEmpty(txtfacevalue.Text.Trim())) ObjCustomerMaster.FaceValueofShares = Convert.ToDecimal(txtfacevalue.Text.Trim());
            if (!string.IsNullOrEmpty(txtbookvalue.Text.Trim())) ObjCustomerMaster.BookValueofShares = Convert.ToDecimal(txtbookvalue.Text.Trim());
            ObjCustomerMaster.BusinessProfile = txtBusinessProfile.Text.Trim();
            // ObjCustomerMaster.Geographicalcoverage = txtGeographical.Text.Trim();
            // if (!string.IsNullOrEmpty(txtnobranch.Text.Trim())) ObjCustomerMaster.NoOfBranches = Convert.ToDecimal(txtnobranch.Text.Trim());
            // if (!string.IsNullOrEmpty(ddlGovernment.Text.Trim())) ObjCustomerMaster.GovernmentInstitutionalParticipation_ID = Convert.ToInt32(ddlGovernment.SelectedValue);
            if (!string.IsNullOrEmpty(txtPercentageStake.Text.Trim())) ObjCustomerMaster.PercentageOfStake = Convert.ToDecimal(txtPercentageStake.Text.Trim());
            // ObjCustomerMaster.JVPartnerName = txtJVPartnerName.Text.Trim();
            // if (!string.IsNullOrEmpty(txtJVPartnerStake.Text.Trim())) ObjCustomerMaster.JVPartnerStake = Convert.ToDecimal(txtJVPartnerStake.Text.Trim());
            ObjCustomerMaster.CEOName = txtCEOName.Text.Trim();
            //  if (!string.IsNullOrEmpty(txtCEOAge.Text.Trim())) ObjCustomerMaster.CEOAge = Convert.ToDecimal(txtCEOAge.Text.Trim());
            //  if (!string.IsNullOrEmpty(txtCEOexperience.Text.Trim())) ObjCustomerMaster.CEOExperienceInYears = Convert.ToDecimal(txtCEOexperience.Text.Trim());
            //  if (!string.IsNullOrEmpty(txtCEOWeddingDate.Text.Trim())) ObjCustomerMaster.CEOWeddingDate = Utility.StringToDate(txtCEOWeddingDate.Text);
            // ObjCustomerMaster.ResidentialAddress = txtResidentialAddress.Text.Trim();

            if (ViewState["BillingAddress"] != null)
            {
                string strXMLBillingDetails = ((DataTable)ViewState["BillingAddress"]).FunPubFormXml(true);
                ObjCustomerMaster.XMLBillingDetails = strXMLBillingDetails;
            }

            string strXMLBankDetails = ((DataTable)ViewState["DetailsTable"]).FunPubFormXml(true);
            strXMLBankDetails = strXMLBankDetails.Replace("OWN_BRANCH_ID='-1'", "");
            strXMLBankDetails = strXMLBankDetails.Replace("BRANCH='&nbsp;'", "");
            strXMLBankDetails = strXMLBankDetails.Replace("OWN_BRANCH_ID='&nbsp;'", "");
            ObjCustomerMaster.XmlBankDetails = strXMLBankDetails;
            ObjCustomerMaster.BillEmailType = Convert.ToInt32(ddlBillEmailType.SelectedValue);
            if (Convert.ToInt32(ddlBillEmailType.SelectedValue) > 0)
            {
                //opc042 start
                string strXmlCustEmailDet = ((DataTable)ViewState["EmailDetailsTable"]).FunPubFormXml(true);
                ObjCustomerMaster.XmlCustEmailDet = strXmlCustEmailDet;
                //opc042 end
            }

            if (!string.IsNullOrEmpty(txtValidupto.Text.Trim())) ObjCustomerMaster.ValidUpto = Utility.StringToDate(txtValidupto.Text);
            ObjCustomerMaster.Created_By = intUserId;
            ObjCustomerMaster.Modified_By = intUserId;
            ObjCustomerMaster.Company_ID = intCompanyId;
            ObjCustomerMaster.Customer = chkCustomer.Checked;
            ObjCustomerMaster.Guarantor1 = chkGuarantor1.Checked;
            ObjCustomerMaster.Guarantor2 = chkGuarantor2.Checked;
            ObjCustomerMaster.CoApplicant = chkCoApplicant.Checked;
            ObjCustomerMaster.XmlConstitutionalDocuments = gvConstitutionalDocuments.FunPubFormXml(true);


            //if (!intCustomerId.Equals(0) && intCustomerId != null)
            //{
            //    DataTable DtSaveStruct = new DataTable();
            //    DataTable DtSave = new DataTable();
            //    DtSaveStruct = (DataTable)ViewState["dtFacilityGroup"];
            //    DtSave = DtSaveStruct.Clone();
            //    foreach (GridViewRow gvRow in grvFacilityGroup.Rows)
            //    {
            //        Label lblLOB = (Label)gvRow.FindControl("lblLOB");
            //        Label lblPricingID = (Label)gvRow.FindControl("lblPricingID");
            //        Label lblProduct = (Label)gvRow.FindControl("lblProduct");
            //        Label lblFacilityAmount = (Label)gvRow.FindControl("lblFacilityAmount");
            //        Label lblSanctionedAmount = (Label)gvRow.FindControl("lblSanctionedAmount");
            //        Label lblApprovedAmount = (Label)gvRow.FindControl("lblApprovedAmount");
            //        Label lblUtilizedAmount = (Label)gvRow.FindControl("lblUtilizedAmount");
            //        Label lblDeactive = (Label)gvRow.FindControl("lblDeactive");
            //        CheckBox chkDeactivate = (CheckBox)gvRow.FindControl("chkDeactivate");
            //        DataRow drow = DtSave.NewRow();
            //        drow["BUSINESS_OFFER_NUMBER"] = lblLOB.Text;
            //        drow["PRICINGID"] = lblPricingID.Text;
            //        drow["OFFERDATE"] = lblProduct.Text;
            //        if (lblFacilityAmount.Text == "")
            //        {
            //            lblFacilityAmount.Text = "0";
            //        }
            //        if (lblSanctionedAmount.Text == "")
            //        {
            //            lblSanctionedAmount.Text = "0";
            //        }
            //        if (lblApprovedAmount.Text == "")
            //        {
            //            lblApprovedAmount.Text = "0";
            //        }
            //        if ((lblApprovedAmount.Text != "" && !lblApprovedAmount.Text.StartsWith("0")) && (lblFacilityAmount.Text != "" && !lblFacilityAmount.Text.StartsWith("0")))
            //        {
            //            lblUtilizedAmount.Text = Convert.ToString(Convert.ToDecimal(lblApprovedAmount.Text) - Convert.ToDecimal(lblFacilityAmount.Text));
            //        }
            //        else
            //        {
            //            lblUtilizedAmount.Text = "0";
            //        }
            //        drow["FACILITY_AMOUNT"] = lblFacilityAmount.Text;
            //        drow["OFFERVALIDTILL"] = lblSanctionedAmount.Text;
            //        drow["SCHEDULED_AMOUNT"] = lblApprovedAmount.Text;
            //        drow["BAL_LIMIT"] = lblUtilizedAmount.Text;
            //        if (chkDeactivate.Checked == true)
            //        {
            //            drow["IS_ACTIVE"] = true;
            //        }
            //        else
            //        {
            //            drow["IS_ACTIVE"] = false;
            //        }
            //        DtSave.Rows.Add(drow);
            //        ;
            //    }

            //    ViewState["dtFacilityGroup"] = DtSave;
             


             //   if (ViewState["dtFacilityGroup"] != null)
               // {
                 //   string strXmlCreditDetails = ((DataTable)ViewState["dtFacilityGroup"]).FunPubFormXml(true);
                  //  ObjCustomerMaster.XmlCreditDetails = strXmlCreditDetails;
                //}
            //}

            ViewState["dtFacilityGroup"] = null;
            ObjCustomerMaster.XmlCreditDetails = "";
                

            //Added By Ganapathy on 13-Nov-2013 BEGINS

            if (Request.QueryString["IsFromEnquiry"] != null && Request.QueryString["EnquiryID"] != null)
            {
                ObjCustomerMaster.Enquiry_ID = Convert.ToInt32(Request.QueryString["EnquiryID"].ToString());
            }

            //Added By Ganapathy on 13-Nov-2013 ENDS


            //BCA Changes - Kuppu - Aug-17
            ObjCustomerMaster.Short_Name = txtFamilyName.Text.Trim();
            ObjCustomerMaster.PAN = txtPAN.Text.Trim();
            ObjCustomerMaster.CIN = txtCIN.Text.Trim();
            //ObjCustomerMaster.TIN = txtTIN.Text.Trim();
            //ObjCustomerMaster.TAN = txtTAN.Text.Trim();
            ObjCustomerMaster.Notes = txtNotes.Text.Trim();
            ObjCustomerMaster.IS_BlockListed = chkBadTrack.Checked;
            ObjCustomerMaster.IS_HotListed = chkHotListed.Checked;
            ObjCustomerMaster.HotList_Reason = txtReson.Text.Trim();
            ObjCustomerMaster.Customer_Rating = txtCustomerRating.Text.Trim();
            //BDO Changes - Thangam M - 03-Oct-2012
            ObjCustomerMaster.CreditType = Convert.ToInt32(rbCreditType.SelectedValue);
            //End here
            ObjCustomerMaster.IS_POBlack = chkPOBlack.Checked;
            //Added on 26Sep2014 starts here
            ObjCustomerMaster.CRM_ID = Convert.ToInt64(intCRMID);
            //End here

            ObjCustomerMaster.Invoice_Cov_Letter = Convert.ToInt32(ddlInvCovLetter.SelectedValue);

            ObjCustomerMaster.Is_Manual_Num = Convert.ToInt32(chkManualNum.Checked);

            if (intCustomerId > 0)
            {
                ObjCustomerMaster.Mode = "Update";
                if (lblNoCustomerTrack.Visible)
                {
                    ObjCustomerMaster.XmlTrackDetails = "<Root></Root>";
                }
                else
                {
                    string strXML = gvTrack.FunPubFormXml(true);
                    strXML = strXML.Replace("RELEASEDBY='&nbsp;'", " ");
                    strXML = strXML.Replace("TYPE='-1'", " ");
                    strXML = strXML.Replace("RELEASEDATE=''", " ");
                    strXML = strXML.Replace("DATE=''", " ");
                    ObjCustomerMaster.XmlTrackDetails = strXML;
                }
                ObjCustomerMaster.XmlPODOMappings = ((DataTable)ViewState["PODOMappings"]).FunPubFormXml(true);
                ObjCustomerMaster.XmlPODOMappingsDetails = ((DataTable)ViewState["PODOMappingsDetails"]).FunPubFormXml(true);
            }
            else
            {
                ObjCustomerMaster.Mode = "Insert";
                ObjCustomerMaster.XmlPODOMappings = "";
                ObjCustomerMaster.XmlPODOMappingsDetails = "";
            }
            ObjCustomerMaster.IS_StateWiseBilling = ChkstatewiseBilling.Checked;
            intErrorCode = objCustomerMasterClient.FunPubCreateCustomerInt(out strCustomerCode, ObjCustomerMaster);

            if (intErrorCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                FunProClearCaches();
                if (intCustomerId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Customer details updated successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                else
                {
                    string[] strCustomerDetails = strCustomerCode.Split('~');
                    txtCustomercode.Text = strCustomerDetails[1].ToString();
                    if (Request.QueryString["IsFromEnquiry"] != null || Request.QueryString["IsFromApplication"] != null)
                    {
                        string strCustomerAlert = "alert('Customer details added successfully');";
                        strCustomerAlert += "window.close();window.opener.location.reload();";
                        strRedirectPageView = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strCustomerAlert, true);
                        Utility.store("EnqNewCustomerId", strCustomerDetails[0].ToString());
                        return;
                    }
                    else
                    {
                        if (Request.QueryString["qsCRM_ID"] != null)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Customer Created successfully.');window.location.href='../Origination/S3GOrgCRM_View.aspx?Code=CRM  ';", true);
                        }
                        else
                        {
                            strAlert = "Customer Code is " + txtCustomercode.Text;
                            strAlert += @"\n\nCustomer details added successfully";
                            strAlert += @"\n\nWould you like to add one more Customer?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strAlert, true);
                            strRedirectPageView = "";
                        }
                    }
                }
            }
            else if (intErrorCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
            }
            else if (intErrorCode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
            }
            else if (intErrorCode == -3)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._3.ToString());
            }
            else if (intErrorCode == -4)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this, "TIN number already defined in Branch Address for other customers");
                tcCustomerMaster.ActiveTab = tbAddress;
                return;
            }
            else if (intErrorCode == -5)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this, "TAN number already defined in Branch Address for other customers");
                tcCustomerMaster.ActiveTab = tbAddress;
                return;
            }
            else if (intErrorCode == -6)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this, "Short name already defined for other customers");
                tcCustomerMaster.ActiveTab = tbAddress;
                txtFamilyName.Focus();
                return;
            }
            else if (intErrorCode == -7)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this, "PAN number already defined for other customers");
                tcCustomerMaster.ActiveTab = tbAddress;
                txtPAN.Focus();
                return;
            }
            else if (intErrorCode == -8)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this, "CIN number already defined for other customers");
                tcCustomerMaster.ActiveTab = tbAddress;
                txtCIN.Focus();
                return;
            }
            else if (intErrorCode == -9)
            {
                /*Added by Thalai Removed location mapped as billing location in RS creation. 
	              Pls. change before remove */
                Utility.FunShowAlertMsg(this, "Location can not be removed, It is mapped as Billing address in RS Creation");
                tcCustomerMaster.ActiveTab = tbAddress;
                return;
            }
            else if (intErrorCode == -15)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this, "Corporate Address GST Tin number already defined for other customers");
                tcCustomerMaster.ActiveTab = tbAddress;
                //txtCIN.Focus();
                return;
            }
            else if (intErrorCode == 17)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this, "TAN number already defined in Corporate Address for other customers");
                tcCustomerMaster.ActiveTab = tbAddress;
                txtTAN.Focus();
                return;
            }
            else if (intErrorCode == 27)
            {
                //Added by Tamilselvan.S on 2/11/2011 for Adding validation for DNC number length
                Utility.FunShowAlertMsg(this, "TIN number already defined in Corporate Address for other customers");
                tcCustomerMaster.ActiveTab = tbAddress;
                txtTIN.Focus();
                return;
            }
            else
            {
                if (intCustomerId > 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Error in updating Customer details");
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Error in adding Customer details");
                }
                strAlert = strAlert + Resources.LocalizationResources.ResourceManager.GetString(Convert.ToString(intErrorCode)).ToString();
                strRedirectPageView = "";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                //strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }
    private bool FunPriValidateDuplicateConstitutionDocs(out string strConsDocName)
    {
        bool blnIsDuplicate = false;
        strConsDocName = "";
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "309");
            Procparam.Add("@Param1", intCompanyId.ToString());
            DataSet dsConstitution = Utility.GetDataset("S3G_ORG_GetCustomerLookUp", Procparam);
            foreach (GridViewRow grDocs in gvConstitutionalDocuments.Rows)
            {
                string strID = grDocs.Cells[0].Text;
                string strDocumentFlag = grDocs.Cells[1].Text;
                string strDocName = grDocs.Cells[2].Text;
                TextBox txtValues = (TextBox)grDocs.FindControl("txtValues");
                if (txtValues != null && strDocumentFlag.ToUpper().StartsWith("CID"))
                {
                    if (txtValues.Text.Trim() != "")
                    {
                        string strCustomerCondition = "";
                        if (intCustomerId > 0)
                        {
                            strCustomerCondition = " and Customer_Id <> " + intCustomerId;
                        }
                        DataRow[] drDuplicateDoc = dsConstitution.Tables[0].Select("DocumentFlag = '" + strDocumentFlag + "' and Values = '" + txtValues.Text.ToUpper().Trim() + "'" + strCustomerCondition);
                        if (drDuplicateDoc.Length > 0)
                        {
                            strConsDocName += strDocName + ",";
                            blnIsDuplicate = true;
                        }
                    }
                }
            }
            //if (blnIsDuplicate)
            //{

            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Duplicate Documents");
        }
        return blnIsDuplicate;
    }

    private bool FunPriValidateDeDuplicateCustomer(out string strConsDocName, out string strDeDupCustomerCode)
    {
        bool blnIsDuplicate = false;
        strConsDocName = "";
        strDeDupCustomerCode = "";
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@CONSTITUTIONID", ddlConstitutionName.SelectedValue);
            Procparam.Add("@COMPANYID", intCompanyId.ToString());
            DataSet dsConstitution = Utility.GetDataset("S3G_ORG_GETDEDUPPARAMETER", Procparam);
            DataRow[] drDeDupField = dsConstitution.Tables[0].Select("DeDup_Field <> '' and IsRequired = 1");
            bool IsDuplicateCustomerName = false;
            bool IsDuplicateAddress1 = false;
            bool IsDuplicateAddress2 = false;

            if (drDeDupField.Length > 0)
            {
                foreach (DataRow drFieldName in drDeDupField)
                {
                    string strFieldName = drFieldName["DeDup_Field"].ToString();
                    dsConstitution.Tables[1].CaseSensitive = false;
                    if (strFieldName.ToUpper() == "CUSTOMER_NAME")
                    {
                        DataRow[] drDeDup = dsConstitution.Tables[1].Select("Customer_Name = '" + txtCustomerName.Text + "'");
                        if (drDeDup.Length > 0)
                        {
                            IsDuplicateCustomerName = true;
                            strConsDocName += "Customer Name,";
                        }
                    }
                    if (strFieldName.ToUpper() == "COMM_ADDRESS1")
                    {
                        DataRow[] drDeDup = dsConstitution.Tables[1].Select("Comm_Address1 = '" + txtComAddress1.Text + "'");
                        if (drDeDup.Length > 0)
                        {
                            IsDuplicateAddress1 = true;
                            strConsDocName += "Corporate Address1,";
                        }
                    }
                    if (strFieldName.ToUpper() == "COMM_ADDRESS2")
                    {
                        DataRow[] drDeDup = dsConstitution.Tables[1].Select("Comm_Address2 = '" + txtCOmAddress2.Text + "'");
                        if (drDeDup.Length > 0)
                        {
                            IsDuplicateAddress2 = true;
                            strConsDocName += "Corporate Address2,";
                        }
                    }
                }
            }
            foreach (GridViewRow grDocs in gvConstitutionalDocuments.Rows)
            {
                string strCategoryID = grDocs.Cells[9].Text;
                string strDocumentFlag = grDocs.Cells[1].Text;
                string strDocName = grDocs.Cells[2].Text;
                TextBox txtValues = (TextBox)grDocs.FindControl("txtValues");
                if (txtValues != null)
                {
                    if (txtValues.Text.Trim() != "")
                    {
                        DataRow[] drDuplicateDoc = dsConstitution.Tables[0].Select("CONSDOCUMENTCATEGORY_ID = '" + strCategoryID + "' and Document_Value = '" + txtValues.Text.ToUpper().Trim() + "'");
                        if (drDuplicateDoc.Length > 0)
                        {
                            strConsDocName += strDocName + ",";
                            blnIsDuplicate = true;
                            foreach (DataRow dr in drDuplicateDoc)
                            {
                                if (!strDeDupCustomerCode.Contains(dr["CustomerCode"].ToString()))
                                {
                                    strDeDupCustomerCode += dr["CustomerCode"].ToString() + ",";
                                }

                            }
                        }
                    }
                }
            }
            if (IsDuplicateAddress1 || IsDuplicateAddress2 || IsDuplicateCustomerName || blnIsDuplicate)
            {
                blnIsDuplicate = true;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Validate Duplicate Documents");
        }
        return blnIsDuplicate;
    }

    /// <summary>
    /// Event for Clear the Controls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
           txtGroupName.Text = txtIndustryName.Text = txtCustomerName.Text = txtTAN.Text = txtTIN.Text = txtFamilyName.Text = txtComEmail.Text = txtPerEmail.Text =
           txtComAddress1.Text = txtPerAddress1.Text = txtCOmAddress2.Text = txtPerAddress2.Text = txtComPincode.Text = txtPerPincode.Text = txtPAN.Text = txtCIN.Text =
           txtDirectors.Text = txtSE.Text = txtPaidCapital.Text = txtfacevalue.Text = txtbookvalue.Text = txtBusinessProfile.Text =txtPercentageStake.Text = txtCEOName.Text = txtComCountry.Text =
                // txtGeographical.Text = txtnobranch.Text = 
           txtAccountNumber.Text = txtBankName.Text = txtBankBranch.Text = txtNotes.Text = txtPerContName.Text = txtPerContactNo.Text = txtComTelephone.Text = txtComMobile.Text = 
           txtMICRCode.Text = txtBankAddress.Text = txtPerTIN.Text = txtPerTAN.Text = txtComWebsite.Text = txtIFSC_Code.Text=  txtReson.Text= txtCustomerRating.Text = 
           txtLOBName.Text = txtValidupto.Text = txtUtilizedAmt.Text
           = txtFacilityType.Text = txtCustomercode.Text = string.Empty;

            //<<Performance>>
            //txtComCity.Items.Insert(0, "");
            txtComCity.Clear();

            //txtPerCity.Items.Insert(0, "");
            txtPerCity.Clear();
            //ddlComState.Items.Insert(0, "");
            //ddlPerState.Items.Insert(0, "");
            txtComCountry.Items.Insert(0, "");
            // txtPerCountry.Items.Insert(0, "");

            //txtComCity.SelectedIndex = 
            //txtPerCity.SelectedIndex =
            //txtComState.SelectedIndex = txtPerState.SelectedIndex = txtComCountry.SelectedIndex = txtPerCountry.SelectedIndex = -1;

            if (ddlCustomerType.Items.Count > 0) ddlCustomerType.SelectedIndex = 0;
            ddlBillAddress.SelectedIndex = 0;
            if (ddlConstitutionName.Items.Count > 0) ddlConstitutionName.SelectedIndex = 0;
            if (ddlTitle.Items.Count > 0) ddlTitle.SelectedIndex = 0;
            if (ddlPostingGroup.Items.Count > 0) ddlPostingGroup.SelectedIndex = 0;
            if (ddlComState.Items.Count > 0) ddlComState.SelectedIndex = 0;
            if (ddlPerState.Items.Count > 0) ddlPerState.SelectedIndex = 0;
            if (ddlRSNum.Items.Count > 0) ddlRSNum.SelectedIndex = 0;
            if (ddlSANum.Items.Count > 0) ddlSANum.SelectedIndex = 0;
            if (ddlPublic.Items.Count > 0) ddlPublic.SelectedIndex = 0;
            if (ddlCompanyType.Items.Count > 0) ddlCompanyType.SelectedIndex = 0;
            // if (ddlGovernment.Items.Count > 0) ddlGovernment.SelectedIndex = 0;

            chkBadTrack.Checked = chkDefaultAccount.Checked = chkHotListed.Checked = chkPOBlack.Checked = chkCoApplicant.Checked = chkCustomer.Checked = chkGuarantor1.Checked = chkGuarantor2.Checked = false;
            
            if (ddlAccountType.Items.Count > 0) ddlAccountType.SelectedIndex = 0;

            cmbGroupCode.Text = string.Empty;
            cmbIndustryCode.Text = string.Empty;
            gvConstitutionalDocuments.Dispose();
            gvConstitutionalDocuments.DataSource = null;
            gvConstitutionalDocuments.DataBind();
            gvBAddress.Dispose();
            gvBAddress.DataSource = null;
            gvBAddress.DataBind();
            grvBankDetails.Dispose();
            grvBankDetails.DataSource = null;
            grvBankDetails.DataBind();
            txtCustomerName.Enabled = true;
            txtCGSTin.Text = "";
            TxtCGSTRegDate.Text = "";
            TxtSGSTRegDate.Text = "";
            txtSGSTin.Text = "";
            tcCustomerMaster.ActiveTab = tbAddress;
            ChkstatewiseBilling.Checked = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.ClearError;
            cvCustomerMaster.IsValid = false;

        }
    }

    /// <summary>
    /// Event for Cancel the Process
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            FunProClearCaches();
            if (Request.QueryString["qsCRM_ID"] != null)
            {
                Response.Redirect("~/Origination/S3GOrgCRM_View.aspx?Code=CRM");
            }
            else if (Request.QueryString["IsFromEnquiry"] != null)
            {
                strAlert = "window.close();";
                //window.opener.location.reload();"; for bug in Pricing Commented on 15/02/2011 by Nataraj
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strAlert, true);
            }
            else if (Request.QueryString["IsFromApplication"] != null)
            {
                strAlert = "window.close();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strAlert, true);
            }
            else
            {
                Response.Redirect("~/Origination/S3gOrgCustomerMaster_View.aspx");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.CancelError;
            cvCustomerMaster.IsValid = false;
        }
    }
    #endregion

    #region Local Methods



    #region GetCompletionList
    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetCompletionList(String prefixText, int count)
    {
        DataTable dt1 = new DataTable();

        dt1 = (DataTable)HttpContext.Current.Session["GroupDT"];

        List<String> suggetions = GetSuggestions(prefixText, count, dt1);


        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetIndustryList(String prefixText, int count)
    {
        DataTable dt1 = new DataTable();

        dt1 = (DataTable)HttpContext.Current.Session["IndustryDT"];

        List<String> suggetions = GetSuggestions(prefixText, count, dt1);


        return suggetions.ToArray();
    }

    #endregion


    #region GetSuggestions
    /// <summary>
    /// GetSuggestions
    /// </summary>
    /// <param name="key">Country Names to search</param>
    /// <returns>Country Names Similar to key</returns>
    private static List<String> GetSuggestions(string key, int count, DataTable dt1)
    {
        List<String> suggestions = new List<string>();
        try
        {

            //DataTable dt1 = new DataTable();

            //dt1 = (DataTable)HttpContext.Current.Session["GroupDT"];// objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);

            string filterExpression = "value like '" + key + "%'";
            DataRow[] dtSuggestions = dt1.Select(filterExpression);

            foreach (DataRow dr in dtSuggestions)
            {
                string suggestion = dr["value"].ToString();
                suggestions.Add(suggestion);
            }

        }
        catch (Exception objException)
        {
            return suggestions;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            //   lblErrorMessage.Text = Resources.LocalizationResources.CustomerTypeChangeError;
        }

        return suggestions;
    }
    #endregion


    /// <summary>
    /// Load the Master data in Dropdownlist
    /// </summary>
    private void FunPriLoadMasterData()
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 1;
            ObjStatus.Param1 = S3G_Statu_Lookup.CUSTOMER_TYPE.ToString();
            Utility.FillDLL(ddlCustomerType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));
            ddlCustomerType.SelectedIndex = 1;
            ddlCustomerType.RemoveDropDownList();

            ObjStatus.Param1 = S3G_Statu_Lookup.COMPANY_TYPE.ToString();
            ObjStatus.Option = 501;
            Utility.FillDLL(ddlCompanyType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            //ObjStatus.Param1 = S3G_Statu_Lookup.HOUSE_TYPE.ToString();
            //Utility.FillDLL(ddlHouseType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = S3G_Statu_Lookup.COMPANY_TYPE.ToString();
            ObjStatus.Option = 1;
            Utility.FillDLL(ddlPublic, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            //ObjStatus.Param1 = S3G_Statu_Lookup.GOVERNMENT.ToString();
            //Utility.FillDLL(ddlGovernment, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            //ObjStatus.Param1 = S3G_Statu_Lookup.MARITAL_STATUS.ToString();
            //Utility.FillDLL(ddlMaritalStatus, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = S3G_Statu_Lookup.ACCOUNT_TYPE.ToString();
            Utility.FillDLL(ddlAccountType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Param1 = "TITLE";
            ObjStatus.Option = 1;
            Utility.FillDLL(ddlTitle, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Option = 2;
            ObjStatus.Param1 = intCompanyId.ToString();
            Utility.FillDLL(ddlConstitutionName, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Option = 502;
            ObjStatus.Param1 = intCompanyId.ToString();
            ObjStatus.Param2 = intCustomerId.ToString();
            Utility.FillDLL(ddlPostingGroup, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            //ObjStatus.Option = 4;
            //ObjStatus.Param1 = intCompanyId.ToString();
            //ObjStatus.Param2 = intUserId.ToString();
            //Utility.FillDLL(ddlBranchList, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

            ObjStatus.Option = 8;
            ObjStatus.Param1 = "GROUP";
            ObjStatus.Param2 = intCompanyId.ToString();
            DataTable dt = new DataTable();
            dt = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            System.Web.HttpContext.Current.Session["GroupDT"] = dt;


            ObjStatus.Option = 8;
            ObjStatus.Param1 = "INDUSTRY";
            ObjStatus.Param2 = intCompanyId.ToString();
            DataTable Indusdt = new DataTable();
            Indusdt = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            System.Web.HttpContext.Current.Session["IndustryDT"] = Indusdt;

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }
    /*
    private void FunPriLoadRelationType()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dt = new DataTable();
            dt = Utility.GetDefaultData("S3G_ORG_GetRelationType", Procparam);
            GrvRelation.DataSource = dt;
            GrvRelation.DataBind();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
     */

    /// <summary>
    /// Toggle the Tabs for LoanAdmin Checking
    /// </summary>
    /// <param name="blnIsEnabled"></param>
    private void FunPriToggleLoanAdminControls(bool blnIsEnabled)
    {
        try
        {
            tcCustomerMaster.Tabs[4].Enabled = blnIsEnabled;
            tcCustomerMaster.Tabs[5].Enabled = blnIsEnabled;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw new ApplicationException("Unable to Toggle Customer Track/Credit Details");
        }
    }

    /// <summary>
    /// Toggle the Controls for CustomerType(Individual/NonIndividual)
    /// </summary>
    private void FunPriToggleCustomerTypeControls()
    {
        try
        {
            //if (Convert.ToInt32(ddlCustomerType.SelectedValue) > 0)
            if (ddlCustomerType.SelectedValue != "")
            {
                if (ddlCustomerType.SelectedItem.Text.ToUpper() == "INDIVIDUAL")
                {
                    //PriFunToggleCustomerTypeControls(false);
                    // ddlPublic.SelectedIndex = ddlGovernment.SelectedIndex = 0;
                    txtDirectors.Text = txtSE.Text = txtPaidCapital.Text = txtfacevalue.Text =
                    //txtbookvalue.Text = txtBusinessProfile.Text = string.Empty;

                    //lblComEmail.CssClass = lblPerEmail.CssClass = "styleDisplayLabel";
                    //lblIndustryCode.CssClass =
                        //lblIndustryName.CssClass =
                    lblPublic.CssClass = "styleDisplayLabel";
                    //lblCEO.CssClass = lblDirectors.CssClass = lblGeographical.CssClass =
                    //lblCEOAge.CssClass = lblCEOexperience.CssClass = lblResidentialAddress.CssClass = lblnobranch.CssClass =
                    //lblBusinessProfile.CssClass = 

                    // lblGender.CssClass = lblDOB.CssClass =
                    //lblQualification.CssClass = lblProfession.CssClass =
                    //lblHouseType.CssClass = lblOwn.CssClass =                    "styleReqFieldLabel";
                }
                else
                {
                    //PriFunToggleCustomerTypeControls(true);
                    //txtDOB.Text = txtAge.Text = txtQualification.Text =
                    //txtProfession.Text = txtSpouseName.Text = txtChildren.Text =
                    //txtTotalDependents.Text = txtTotNetWorth.Text = txtRemainingLoanValue.Text =
                    //txtWeddingdate.Text = string.Empty;
                    //frvCurrentMarketValue.Enabled = false;
                    //lblComEmail.CssClass = lblPerEmail.CssClass = "styleReqFieldLabel";
                    //lblIndustryCode.CssClass =
                        //lblIndustryName.CssClass =
                    //lblPublic.CssClass =
                        //lblCEO.CssClass = lblDirectors.CssClass = lblGeographical.CssClass = lblnobranch.CssClass =
                        //lblCEOAge.CssClass = lblCEOexperience.CssClass = lblResidentialAddress.CssClass =
                        //lblBusinessProfile.CssClass =
                    //"styleReqFieldLabel";
                    // lblGender.CssClass = lblDOB.CssClass =
                    //    lblQualification.CssClass = lblProfession.CssClass =
                    //lblHouseType.CssClass = lblOwn.CssClass = "styleDisplayLabel";

                    //ddlMaritalStatus.SelectedIndex = ddlHouseType.SelectedIndex = ddlOwn.SelectedIndex = 0;
                    //ddlGender.SelectedIndex = 0;

                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Toggle Controls for Customer Type");
        }
    }


    /// <summary>
    /// Toggle the Controls for CustomerType(Individual/NonIndividual)
    /// </summary>
    /// <param name="blnIsEnabled"></param>
    /// <summary>
    /// method for default settings for all controls
    /// </summary>
    private void FunPriSetControlSettings()
    {
        try
        {
            //lblCustomercode.Text = Resources.LocalizationResources.ORG_CUST_lblCustomercode;
            //lblCustomerType.Text = Resources.LocalizationResources.ORG_CUST_lblCustomerType;
            //lblgroupcode.Text = Resources.LocalizationResources.ORG_CUST_lblgroupcode;
            //lblGroupName.Text = Resources.LocalizationResources.ORG_CUST_lblGroupName;
            //lblIndustryCode.Text = Resources.LocalizationResources.ORG_CUST_lblIndustryCode;
            //lblIndustryName.Text = Resources.LocalizationResources.ORG_CUST_lblIndustryName;
            //lblConstitutionName.Text = Resources.LocalizationResources.ORG_CUST_lblConstitutionName;
            //lblTitle.Text = Resources.LocalizationResources.ORG_CUST_lblTitle;
            //lblCustomerName.Text = Resources.LocalizationResources.ORG_CUST_lblCustomerName;
            //lblCustomerPostingGroup.Text = Resources.LocalizationResources.ORG_CUST_lblCustomerPostingGroup;

            //lblComAddress1.Text = Resources.LocalizationResources.ORG_CUST_lblComAddress1;
            //lblPerAddress1.Text = Resources.LocalizationResources.ORG_CUST_lblPerAddress1;
            //lblComAddress2.Text = Resources.LocalizationResources.ORG_CUST_lblComAddress2;
            //lblPerAddress2.Text = Resources.LocalizationResources.ORG_CUST_lblPerAddress2;
            //lblComcity.Text = Resources.LocalizationResources.ORG_CUST_lblComcity;
            //lblPerCity.Text = Resources.LocalizationResources.ORG_CUST_lblPerCity;
            //lblComState.Text = Resources.LocalizationResources.ORG_CUST_lblComState;
            //lblPerState.Text = Resources.LocalizationResources.ORG_CUST_lblPerState;
            //lblComCountry.Text = Resources.LocalizationResources.ORG_CUST_lblComCountry;
            //lblPerCountry.Text = Resources.LocalizationResources.ORG_CUST_lblPerCountry;
            //lblCompincode.Text = Resources.LocalizationResources.ORG_CUST_lblCompincode;
            //lblPerpincode.Text = Resources.LocalizationResources.ORG_CUST_lblPerpincode;
            //lblComMobile.Text = Resources.LocalizationResources.ORG_CUST_lblComMobile;
            //lblPerMobile.Text = Resources.LocalizationResources.ORG_CUST_lblPerMobile;
            //lblComTelephone.Text = Resources.LocalizationResources.ORG_CUST_lblComTelephone;
            //lblPerTelephone.Text = Resources.LocalizationResources.ORG_CUST_lblPerTelephone;
            //lblComEmail.Text = Resources.LocalizationResources.ORG_CUST_lblComEmail;
            //lblPerEmail.Text = Resources.LocalizationResources.ORG_CUST_lblPerEmail;
            //lblComWebSite.Text = Resources.LocalizationResources.ORG_CUST_lblComWebSite;
            //lblPerWebSite.Text = Resources.LocalizationResources.ORG_CUST_lblPerWebSite;

            //lblGender.Text = Resources.LocalizationResources.ORG_CUST_lblGender;
            //lblDOB.Text = Resources.LocalizationResources.ORG_CUST_lblDOB;
            //lblAge.Text = Resources.LocalizationResources.ORG_CUST_lblAge;
            //lblMaritalStatus.Text = Resources.LocalizationResources.ORG_CUST_lblMaritalStatus;
            //lblQualification.Text = Resources.LocalizationResources.ORG_CUST_lblQualification;
            //lblProfession.Text = Resources.LocalizationResources.ORG_CUST_lblProfession;
            //lblSpouseName.Text = Resources.LocalizationResources.ORG_CUST_lblSpouseName;
            //lblChildren.Text = Resources.LocalizationResources.ORG_CUST_lblChildren;
            //lblTotalDependents.Text = Resources.LocalizationResources.ORG_CUST_lblTotalDependents;
            //lblWeddingdate.Text = Resources.LocalizationResources.ORG_CUST_lblWeddingdate;
            //lblHouseType.Text = Resources.LocalizationResources.ORG_CUST_lblHouseType;
            //lblOwn.Text = Resources.LocalizationResources.ORG_CUST_lblOwn;
            //lblCurrentMarketValue.Text = Resources.LocalizationResources.ORG_CUST_lblCurrentMarketValue;
            //lblRemainingLoanValue.Text = Resources.LocalizationResources.ORG_CUST_lblRemainingLoanValue;
            //lblTotNetWorth.Text = Resources.LocalizationResources.ORG_CUST_lblTotNetWorth;

            //lblPublic.Text = Resources.LocalizationResources.ORG_CUST_lblPublic;
            //lblDirectors.Text = Resources.LocalizationResources.ORG_CUST_lblDirectors;
            //lblStockExchange.Text = Resources.LocalizationResources.ORG_CUST_lblStockExchange;
            //lblPaidCapital.Text = Resources.LocalizationResources.ORG_CUST_lblPaidCapital;
            //lblfacevalue.Text = Resources.LocalizationResources.ORG_CUST_lblfacevalue;
            //lblbookvalue.Text = Resources.LocalizationResources.ORG_CUST_lblbookvalue;
            //lblBusinessProfile.Text = Resources.LocalizationResources.ORG_CUST_lblBusinessProfile;
            //lblGeographical.Text = Resources.LocalizationResources.ORG_CUST_lblGeographical;
            //lblnobranch.Text = Resources.LocalizationResources.ORG_CUST_lblnobranch;
            //lblGovernment.Text = Resources.LocalizationResources.ORG_CUST_lblGovernment;
            //lblStake.Text = Resources.LocalizationResources.ORG_CUST_lblStake;
            //lblJVPartnerName.Text = Resources.LocalizationResources.ORG_CUST_lblJVPartnerName;
            //lblJVPartnerStake.Text = Resources.LocalizationResources.ORG_CUST_lblJVPartnerStake;
            //lblCEO.Text = Resources.LocalizationResources.ORG_CUST_lblCEO;
            //lblCEOAge.Text = Resources.LocalizationResources.ORG_CUST_lblCEOAge;
            //lblCEOexperience.Text = Resources.LocalizationResources.ORG_CUST_lblCEOexperience;
            //lblResidentialAddress.Text = Resources.LocalizationResources.ORG_CUST_lblResidentialAddress;
            //lblCEOWeddingDate.Text = Resources.LocalizationResources.ORG_CUST_lblCEOWeddingDate;

            //lblAccountType.Text = Resources.LocalizationResources.ORG_CUST_lblAccountType;
            //lblAccountNumber.Text = Resources.LocalizationResources.ORG_CUST_lblAccountNumber;
            //lblBankName.Text = Resources.LocalizationResources.ORG_CUST_lblBankName;
            //lblBankBranch.Text = Resources.LocalizationResources.ORG_CUST_lblBankBranch;
            //lblMICRCode.Text = Resources.LocalizationResources.ORG_CUST_lblMICRCode;
            //lblBankAddress.Text = Resources.LocalizationResources.ORG_CUST_lblBankAddress;

            //lblLOBName.Text = Resources.LocalizationResources.ORG_CUST_lblLOBName;
            //lblProductGroup.Text = Resources.LocalizationResources.ORG_CUST_lblProductGroup;
            //lblSanctionamt.Text = Resources.LocalizationResources.ORG_CUST_lblSanctionamt;
            //lblValid.Text = Resources.LocalizationResources.ORG_CUST_lblValid;
            //lblUtilizedAmt.Text = Resources.LocalizationResources.ORG_CUST_lblUtilizedAmt;
            //lblFacilityType.Text = Resources.LocalizationResources.ORG_CUST_lblFacilityType;
            //lblHeading.Text = Resources.LocalizationResources.ORG_CUST_lblHeading;

            //btnSave.Text = Resources.LocalizationResources.ORG_CUST_btnSave;
            //btnClear.Text = Resources.LocalizationResources.ORG_CUST_btnClear;



            rfvCustomerType.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCustomerType;
            rfvConstitutionName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvConstitutionName;
            rfvCompanyType.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCompanyType;
            rfvTitle.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvTitle;
            rfvComAddress1.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComAddress1;
            rfvtxtPerAddress1.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvtxtPerAddress1;
            rfvShortName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvShortName;
            //rfvComCity.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComCity;
            txtComCity.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComCity;
            //rfvPerCity.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerCity;
            txtPerCity.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerCity;
            rfvComState.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComState;
            rfvPerState.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerState;
            rfvComCountry.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComCountry;
            rfvPostingGroup.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPostingGroup;
            //rfvPerCountry.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerCountry;
            rfvComPincode.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComPincode;
            rvfPerPincode.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rvfPerPincode;
            rfvComTelephone.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComTelephone;
            // rfvPerTelephone.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerTelephone;
            revEmailId.ErrorMessage = Resources.LocalizationResources.ORG_CUST_revEmailId;
            rfvPerEmail.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPerEmail;
            revPerEmail.ErrorMessage = Resources.LocalizationResources.ORG_CUST_revPerEmail;
            rfvComWebsite.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComWebsite;
            revComWebsite.ErrorMessage = Resources.LocalizationResources.ORG_CUST_revComWebsite;
           
            rfvComPAN.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvComPAN;
            revPAN.ErrorMessage = Resources.LocalizationResources.ORG_CUST_PAN;
            rfvComTAN.ErrorMessage = Resources.LocalizationResources.ORG_CUST_TAN;
            //rfvPerTAN .ErrorMessage = Resources.LocalizationResources.ORG_CUST_TAN;
            // revPerWebSite.ErrorMessage = Resources.LocalizationResources.ORG_CUST_revPerWebSite;
            //rfvPercentageStake.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPercentageStake;
            //rfvQualification.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvQualification;
            //rfvtxtProfession.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvtxtProfession;
            //rfvHouseType.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvHouseType;
            //rfvOwn.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvOwn;
            //rfvtotalnetworth.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvtotalnetworth;
            rfvPublic.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvPublic;
            rfvDirectors.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvDirectors;
            rfvBusinessProfile.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvBusinessProfile;
            //rfvGeographical.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvGeographical;
            //rfvnobranch.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvnobranch;
            rfvCEOName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCEOName;
            //rfvCEOAge.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCEOAge;
            //rfvCEOexperience.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvCEOexperience;
            //rfvResidentialAddress.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvResidentialAddress;

            RFVtxtIFSC_Code.ErrorMessage = Resources.LocalizationResources.ORG_CUST_RFVtxtIFSC_Code;
            rfvAccountType.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvAccountType;
            rfvAccountNumber.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvAccountNumber;
            rfvBankName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvBankName;
            rfvBankBranch.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvBankBranch;
            rfvMICRCode.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvMICRCode;
            rfvBankAddress.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rfvBankAddress;

            rvfCustomerName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rvfCustomerName;

            rvfCustomerName.ErrorMessage = Resources.LocalizationResources.ORG_CUST_rvfCustomerName;


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Initialise the Controls");
        }
    }

    /// <summary>
    /// Method for Loading the Constitutional Documents
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPriLoadCustomerConstitutionDocs(int CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 169;
            ObjStatus.Param1 = CustomerID.ToString();
            gvConstitutionalDocuments.DataSource = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            gvConstitutionalDocuments.DataBind();

            //Added By Thangam M on 12/Mar/2012 to fix bug id - 5451
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Clear", "javascript:fnClearAsyncUploader('" + gvConstitutionalDocuments.Rows.Count.ToString() + "');", true);

            ViewState["ConstitutionDocument"] = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            ObjStatus.Option = 787;
            ObjStatus.Param1 = intCompanyId.ToString();
            DataTable dtConsDocPath = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (dtConsDocPath == null || (dtConsDocPath != null && dtConsDocPath.Rows.Count == 0))
            {
                Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Consitution Document");
                return;
            }
            ViewState["ConsDocPath"] = dtConsDocPath.Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load the Constitution Documents");
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }
    protected void hlnkView_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriShowPRDD(sender);
        }
        catch (Exception ex)
        {
            cvCustomerMaster.ErrorMessage = ex.Message;
            cvCustomerMaster.IsValid = false;
        }
    }
    private void FunPriShowPRDD(object sender)
    {
        try
        {
            string strFieldAtt = ((LinkButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvConstitutionalDocuments", strFieldAtt);
            //Label lblPath = gvConstitutionalDocuments.Rows[gRowIndex].FindControl("myThrobber") as Label;
            Label lblActualPath = gvConstitutionalDocuments.Rows[gRowIndex].FindControl("lblActualPath") as Label;
            string strFileName = lblActualPath.Text.Replace("\\", "/").Trim();
            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to View the Document");
        }
    }
    private void FunPriLoadCustomerBankDetails(int CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 200;
            ObjStatus.Param1 = CustomerID.ToString();
            ViewState["DetailsTable"] = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (ViewState["DetailsTable"] != null)
            {
                grvBankDetails.DataSource = (DataTable)ViewState["DetailsTable"];
                grvBankDetails.DataBind();
                FunPriHideBankColumns();

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Bank Details");
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }

    private void FunPriLoadBillingAddressDetails(int CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            DataTable dtBillAddress = new DataTable();
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 707;
            ObjStatus.Param1 = CustomerID.ToString();
            ViewState["BillingAddress"] = dtBillAddress = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (dtBillAddress.Rows.Count > 0)
            {
                gvBAddress.DataSource = dtBillAddress;
                gvBAddress.DataBind();
                FunPriHideBankColumns();
            }

            DataTable ObjCustomerDetails = new DataTable();
            ObjStatus.Option = 808;
            ObjStatus.Param1 = CustomerID.ToString();
            ObjCustomerDetails = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);

            if (ObjCustomerDetails.Rows.Count > 0)
            {
                //txtComState.ClearDropDownList();
                //txtPerState.ClearDropDownList();
                //txtComState.ClearSelection();
                //txtPerState.ClearSelection();
                //Dictionary<string, string> Procparam = new Dictionary<string, string>();
                //if (intCompanyId > 0)
                //{
                //    Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
                //}
                //DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetStateLookup", Procparam);
                //txtComState.FillDataTable(dtAddr, "Location_Category_ID", "LocationCat_Description", false);
                //txtPerState.FillDataTable(dtAddr, "Location_Category_ID", "LocationCat_Description", false);


                txtComAddress1.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Address1"]);
                txtCOmAddress2.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Address2"]);
                txtComCity.SelectedText = Convert.ToString(ObjCustomerDetails.Rows[0]["City"]);
                ddlComState.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Location_Category_ID"]);
                ViewState["CorporateStateValue"] = ddlComState.SelectedValue;
                txtComPincode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Pincode"]);
                if (txtComPincode.Text.Trim() == "0")
                {
                    txtComPincode.Text = "";
                }
                txtComCountry.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Country"]);
                txtComTelephone.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Telephone"]);
                txtComMobile.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Mobile"]);
                txtComEmail.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["EMail"]);
                txtComWebsite.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Website"]);
                txtTIN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["TIN"]);
                txtTAN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["TAN"]);
                txtCGSTin.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CGSTIN"]);
                if(ObjCustomerDetails.Rows[0]["CGST_Reg_Date"].ToString()!="")
                TxtCGSTRegDate.Text = DateTime.Parse(Convert.ToString((ObjCustomerDetails.Rows[0]["CGST_Reg_Date"])), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            }


            DataTable ObjCustomerCorpAddress = new DataTable();
            ObjStatus.Option = 909;
            ObjStatus.Param1 = CustomerID.ToString();
            ObjCustomerCorpAddress = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (ObjCustomerDetails.Rows.Count > 0)
            {
                txtPAN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["PAN"]);
                txtCIN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CIN"]);

            }



        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Bank Details");
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }
    private void FunPriHideBankColumns()
    {
        try
        {
            //grvBankDetails.Columns[0].Visible = false;
            //grvBankDetails.Columns[1].Visible = false;
            //grvBankDetails.Columns[2].Visible = false;
            //grvBankDetails.Columns[4].Visible = false;
            //grvBankDetails.Columns[6].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// Method for Disable the Text in Constitutional Document Details tab
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private bool FunPriDisableValueField(string str)
    {
        string[] strCheck = new string[2];
        bool blnIsResult = true;
        try
        {
            strCheck = str.Split('-');
            if (strCheck.Length > 0)
            {
                if (Convert.ToString(strCheck[0]) != "CID")
                    blnIsResult = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        return blnIsResult;

    }

    /// <summary>
    /// Method for assign the Text into Coombobox
    /// </summary>
    /// <param name="strControlText"></param>
    /// <param name="cmbControl"></param>
    private void FunPriSetDropDownText(string strControlText, AjaxControlToolkit.ComboBox cmbControl)
    {
        try
        {
            for (int i = 0; i < cmbControl.Items.Count; i++)
            {
                if (cmbControl.Items[i].Text == strControlText)
                    cmbControl.SelectedItem.Text = strControlText;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    /// <summary>
    /// Method for Loading the Customer Details
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPriLoadCustomerDetails(int CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 10;
            ObjStatus.Param1 = CustomerID.ToString();
            DataTable ObjCustomerDetails = new DataTable();

            ObjCustomerDetails = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);

            if (ObjCustomerDetails.Rows.Count > 0)
            {
                txtCustomercode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Code"]);

                ListItem lst;

                if (strMode == "Q")
                {
                    lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Type"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Type_ID"]));
                    ddlCustomerType.Items.Add(lst);
                }
                ddlCustomerType.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Type_ID"]);
                cmbGroupCode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["GroupCode"]);
                // FunPriSetDropDownText(Convert.ToString(ObjCustomerDetails.Rows[0]["GroupCode"]), cmbGroupCode); commented by Prakash 
                txtGroupName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Groupname"]);
                //if (!string.IsNullOrEmpty(txtGroupName.Text))
                //{
                //    txtGroupName.ReadOnly = true;
                //}
                //  FunPriSetDropDownText(Convert.ToString(ObjCustomerDetails.Rows[0]["IndustryCode"]), cmbIndustryCode); commented by Prakash 
                cmbIndustryCode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["IndustryCode"]);
                txtIndustryName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["IndustryName"]);

                txtFamilyName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Short_Name"]);
                txtNotes.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Notes"]);
                txtPAN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["PAN"]);
                txtCIN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CIN"]);
                // txtTIN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["TIN"]);
                //txtTAN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["TAN"]);
                rbCreditType.SelectedIndex = Convert.ToInt32(ObjCustomerDetails.Rows[0]["CreditType"]);
                if (ObjCustomerDetails.Rows[0]["Black_Listed"].ToString() != null)
                {
                    if (ObjCustomerDetails.Rows[0]["Black_Listed"].ToString() != "")
                    {
                        chkBadTrack.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Black_Listed"]);
                    }
                }
                if (ObjCustomerDetails.Rows[0]["Hot_Listed"] != null)
                {

                    if (ObjCustomerDetails.Rows[0]["Hot_Listed"].ToString() != "")
                    {
                        chkHotListed.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Hot_Listed"]);
                    }
                }

                if (ObjCustomerDetails.Rows[0]["Is_PO_Black"] != null)
                {

                    if (ObjCustomerDetails.Rows[0]["Is_PO_Black"].ToString() != "")
                    {
                        chkPOBlack.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Is_PO_Black"]);
                    }
                }
                //opc042
                ddlBillEmailType.SelectedValue = ObjCustomerDetails.Rows[0]["Bill_Email_Type"].ToString();
                //opc042
                if (ObjCustomerDetails.Rows[0]["Invoice_Cov_Letter"].ToString() != "0")
                    ddlInvCovLetter.SelectedValue = ObjCustomerDetails.Rows[0]["Invoice_Cov_Letter"].ToString();

                chkManualNum.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Is_Manual_Num"]);

                txtReson.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Hot_List_Reason"]);
                txtCustomerRating.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Rating"]);

                //if (!string.IsNullOrEmpty(txtIndustryName.Text))
                //{
                //    txtIndustryName.Enabled = false;
                //}

                if (strMode == "Q")
                {
                    lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Constitution_Name"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Constitution_ID"]));
                    ddlConstitutionName.Items.Add(lst);
                }

                ddlConstitutionName.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Constitution_ID"]);


                if (strMode == "Q")
                {
                    lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Title"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Title_ID"]));
                    ddlTitle.Items.Add(lst);
                }



                ddlTitle.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Title_ID"]);
                //added Relation Type
                chkCustomer.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Customer"]);
                chkGuarantor1.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Guarantor1"]);
                chkGuarantor2.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Guarantor2"]);
                chkCoApplicant.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Co_Applicant"]);

                if (chkCustomer.Checked)
                    chkCustomer.Enabled = false;
                if (chkGuarantor1.Checked)
                    chkGuarantor1.Enabled = false;
                if (chkGuarantor2.Checked)
                    chkGuarantor2.Enabled = false;
                if (chkCoApplicant.Checked)
                    chkCoApplicant.Enabled = false;
                //end
                txtCustomerName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Name"]);

                ListItem ListPosting = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Posting_Code"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Posting_Group_Code_ID"]), true);
                ListPosting.Selected = false;
                try
                {
                    ddlPostingGroup.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Customer_Posting_Group_Code_ID"]);
                }
                catch (Exception ex)
                {
                    ddlPostingGroup.SelectedIndex = 0;
                }

                
                txtComAddress1.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Address1"]);
                txtCOmAddress2.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Address2"]);
                txtPAN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["PAN"]);
                txtCIN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CIN"]);


                //txtTIN.Text = 
                //txtTAN.Text = 

                //<<Performance>>
                //txtComCity.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_City"]);
                txtComCity.SelectedText = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_City"]);

                if (strMode == "Q")
                {
                    ddlComState.Items.Add(Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_State"]));
                    txtComCountry.Items.Add(Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Country"]));
                    //txtPerState.Items.Add(Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_State"]));
                    //txtPerCountry.Items.Add(Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Country"]));
                }
                ddlComState.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_State"]);
                ddlBillAddress.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["BillingAddress"]);
                txtComCountry.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Country"]);
                txtComPincode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_PINCode"]);
                if (txtComPincode.Text.Trim() == "0")
                {
                    txtComPincode.Text = "";
                }
                txtComMobile.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Mobile"]);
                txtComTelephone.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Telephone"]);
                txtComEmail.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Email"]);
                txtComWebsite.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Comm_Website"]);
                //txtPerAddress1.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Address1"]);
                //txtPerAddress2.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Address2"]);
                //<<Performance>>
                //txtPerCity.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_City"]);
                //txtPerCity.SelectedText = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_City"]);
                //txtPerState.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_State"]);
                // txtPerCountry.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Country"]);
                //txtPerPincode.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_PINCode"]);
                // txtPerMobile.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Mobile"]);
                // txtPerTelephone.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Telephone"]);
                //txtPerEmail.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Email"]);
                // txtPerWebSite.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Website"]);
                //txtPerTIN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Email"]);
                //txtPerTAN.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Perm_Email"]); ;
                if (ddlCustomerType.SelectedItem.Text.ToUpper() == "INDIVIDUAL")
                {

                    #region Individual Type
                    //if (ObjCustomerDetails.Rows[0]["Gender"] == DBNull.Value)
                    //{
                    //    ddlGender.SelectedIndex = 0;
                    //}
                    //else
                    //{
                    //    ddlGender.SelectedValue = Convert.ToInt32(ObjCustomerDetails.Rows[0]["Gender"]).ToString();
                    //}
                    //if (Convert.ToString(ObjCustomerDetails.Rows[0]["DateofBirth"]) != "") txtDOB.Text = DateTime.Parse(Convert.ToString(ObjCustomerDetails.Rows[0]["DateofBirth"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);

                    ////Check if not exist
                    //if (txtDOB.Text == "1/1/1900")
                    //{
                    //    txtDOB.Text = string.Empty;
                    //}
                    //if (!string.IsNullOrEmpty(txtDOB.Text))
                    //{
                    //    int intDOBYear = Utility.StringToDate(txtDOB.Text).Year;
                    //    txtAge.Text = ((DateTime.Now.Year - intDOBYear) + 1).ToString();
                    //}

                    //if (ObjCustomerDetails.Rows[0]["Marital_Status_ID"] == DBNull.Value)
                    //{
                    //    ddlMaritalStatus.SelectedIndex = -1;
                    //}
                    //else
                    //{
                    //    if (strMode == "Q")
                    //    {
                    //        lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Marital_Status"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Marital_Status_ID"]));
                    //        ddlMaritalStatus.Items.Add(lst);
                    //    }

                    //    ddlMaritalStatus.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Marital_Status_ID"]);
                    //}
                    //txtQualification.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Qualification"]);
                    //txtProfession.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Profession"]);
                    //txtSpouseName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Spouse_Name"]);
                    //txtChildren.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Children"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Children"]);
                    //txtTotalDependents.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Dependents"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Dependents"]);
                    //if (Convert.ToString(ObjCustomerDetails.Rows[0]["Wedding_Anniversary_Date"]) != "") txtWeddingdate.Text = DateTime.Parse(Convert.ToString(ObjCustomerDetails.Rows[0]["Wedding_Anniversary_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    //if (txtWeddingdate.Text == "1/1/1900")
                    //{
                    //    txtWeddingdate.Text = string.Empty;
                    //}

                    //if (ObjCustomerDetails.Rows[0]["House_Flat_ID"] == DBNull.Value)
                    //{
                    //    ddlHouseType.SelectedIndex = -1;
                    //}
                    //else
                    //{
                    //    if (strMode == "Q")
                    //    {
                    //        lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["House_Flat"]), Convert.ToString(ObjCustomerDetails.Rows[0]["House_Flat_ID"]));
                    //        ddlHouseType.Items.Add(lst);
                    //    }

                    //    ddlHouseType.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["House_Flat_ID"]);
                    //}
                    //if (ObjCustomerDetails.Rows[0]["Is_Own"] == DBNull.Value)
                    //{
                    //    //ddlOwn.SelectedIndex = -1;
                    //}
                    //else
                    //{
                    //  //  ddlOwn.SelectedValue = (Convert.ToString(ObjCustomerDetails.Rows[0]["Is_Own"]).ToLower() == "true") ? "1" : "0";
                    //}
                    //Added by ganapathy to fix the bugID 6343
                    //if (ddlOwn.SelectedValue == "0")
                    //{
                    //    txtCurrentMarketValue.Enabled = false;
                    //    txtRemainingLoanValue.Enabled = false;
                    //    txtTotNetWorth.Enabled = false;
                    //}
                    //else
                    //{
                    //    txtCurrentMarketValue.Enabled = true;
                    //    txtRemainingLoanValue.Enabled = true;
                    //    txtTotNetWorth.Enabled = true;
                    //}
                    //End here
                    //Commented by Thangam M on 09/Mar/2012 to fix bug id 5445
                    //txtCurrentMarketValue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]);
                    //End here
                    //Code Modified By Ganapathy for fixing the bug 6343
                    // txtCurrentMarketValue.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]);
                    //txtCurrentMarketValue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Current_Market_Value"]);
                    //txtRemainingLoanValue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Remaining_Loan_Value"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Remaining_Loan_Value"]);
                    // txtRemainingLoanValue.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Remaining_Loan_Value"]);
                    //txtTotNetWorth.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Net_Morth"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Net_Morth"]);
                    //txtTotNetWorth.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Total_Net_Morth"]);
                    //End here
                    #endregion
                }
                else
                {
                    #region Non-Individual

                    if (strMode == "Q")
                    {
                        ObjStatus.Param1 = S3G_Statu_Lookup.COMPANY_TYPE.ToString();
                        ObjStatus.Option = 1;
                        Utility.FillDLL(ddlPublic, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));
                        if (Convert.ToString(ObjCustomerDetails.Rows[0]["Public_Closely_held"]) == "0")
                        {
                            ddlPublic.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlPublic.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Public_Closely_held"]);
                        }
                        
                        ddlPublic.RemoveDropDownList();

                       


                       


                    }
                    else
                    {
                        if (Convert.ToString(ObjCustomerDetails.Rows[0]["Public_Closely_held"]) == "0")
                        {
                            ddlPublic.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlPublic.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Public_Closely_held"]);
                        }
                      
                    }
                    ddlCompanyType.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Company_Type_ID"]);
                    ddlCompanyType.RemoveDropDownList();
                    // ddlPublic.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Public_Closely_held"]);
                    txtDirectors.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["No_Of_Directors"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["No_Of_Directors"]);
                    txtSE.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Stock_Exchange"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Stock_Exchange"]);
                    txtPaidCapital.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Paid_up_Capital"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Paid_up_Capital"]);
                    txtfacevalue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Face_Value"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Face_Value"]);
                    txtbookvalue.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Book_Value"]) == "0") ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Book_Value"]);
                    txtBusinessProfile.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Business_Profile"]);
                    // txtGeographical.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Geographical_Coverage"]);
                    // txtnobranch.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["No_Of_Branches"]);

                    //if (strMode == "Q")
                    //{
                    //    lst = new ListItem(Convert.ToString(ObjCustomerDetails.Rows[0]["Participation"]), Convert.ToString(ObjCustomerDetails.Rows[0]["Participation_ID"]));
                    //    ddlGovernment.Items.Add(lst);
                    //}
                    //ddlGovernment.SelectedValue = Convert.ToString(ObjCustomerDetails.Rows[0]["Participation_ID"]);

                    //Changed By Thangam M on 12/Mar/2012 to fix bug id - 5447
                    txtPercentageStake.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Promoters_stake_percentage"]);
                   // txtPercentageStake.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["Percentage_Of_Stake"]).StartsWith("0")) ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["Percentage_Of_Stake"]);

                    //  txtJVPartnerName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["JV_Partner_Name"]);

                    //  txtJVPartnerStake.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["JV_Partner_Stake"]);
                    //txtJVPartnerStake.Text = (Convert.ToString(ObjCustomerDetails.Rows[0]["JV_Partner_Stake"]).StartsWith("0")) ? "" : Convert.ToString(ObjCustomerDetails.Rows[0]["JV_Partner_Stake"]);
                    //End here

                    txtCEOName.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_MD_name"]);
                    //  txtCEOAge.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Age"]);
                    //   if (Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Wedding_Date"]) != "") txtCEOWeddingDate.Text = DateTime.Parse(Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Wedding_Date"]), CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                    //if (txtCEOWeddingDate.Text == "1/1/1900")
                    //{
                    //    txtCEOWeddingDate.Text = string.Empty;
                    //}
                    FunPriLoadBillingAddressDetails(CustomerID);
                    //txtCEOexperience.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["CEO_Experience_In_Years"]);
                    //txtResidentialAddress.Text = Convert.ToString(ObjCustomerDetails.Rows[0]["Residential_Address"]);
                    #endregion
                }

                FunPriLoadCustomerBankDetails(CustomerID);
                FunPriLoadCustomerBillEmailDetails(CustomerID);
                FunPriLoadCustomerConstitutionDocs(CustomerID);
                FunPriLoadCustomerTrackDetails(CustomerID);
                FunPriToggleCustomerTypeControls();
                FunProClearUploader();
                if (ObjCustomerDetails.Rows[0]["Is_statewiseBilling"] != null)
                {

                    if (ObjCustomerDetails.Rows[0]["Is_statewiseBilling"].ToString() != "")
                    {
                        ChkstatewiseBilling.Checked = Convert.ToBoolean(ObjCustomerDetails.Rows[0]["Is_statewiseBilling"]);
                    }
                }
                if (strMode == "Q")
                    ChkstatewiseBilling.Enabled = false;

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }


   #region Alert Tab

    private void FunPriBindCustomerTrackDLL()
    {
        try
        {
            Dictionary<string, string> ProcParam = new Dictionary<string, string>();
            ProcParam.Add("@CompanyId", intCompanyId.ToString());
            ProcParam.Add("@CustomerId", intCustomerId.ToString());
            ProcParam.Add("@UserId", intUserId.ToString());
            DataSet dsCustomerTrackDDL = Utility.GetDataset("S3G_ORG_LOADCUSTOMERTRACKDDL", ProcParam);
            ViewState["TrackLOB"] = dsCustomerTrackDDL.Tables[0];
            ViewState["TrackAccountNo"] = dsCustomerTrackDDL.Tables[1];
            ViewState["TrackType"] = dsCustomerTrackDDL.Tables[2];

            DataTable objTrack = ViewState["CustomerTrackDetails"] as DataTable;
            bool blnIsFooterOnly = false;
            if (objTrack.Rows.Count == 0)
            {
                DataRow drTrack = objTrack.NewRow();
                drTrack["LOB_ID"] = 0;
                drTrack["LOB_NAME"] = "";
                drTrack["PA_SA_REF_ID"] = "0";
                drTrack["ACCOUNTNO"] = "";
                drTrack["DATE"] = "";
                drTrack["RELEASEDATE"] = "";
                drTrack["LOGINBY"] = "";
                drTrack["RELEASED_BY"] = 0;
                drTrack["RELEASEDBY"] = "";
                drTrack["REASON"] = "";
                drTrack["TRACK_TYPE"] = "";
                drTrack["TRACK_TYPE_ID"] = 0;
                objTrack.Rows.Add(drTrack);
                blnIsFooterOnly = true;
            }
            gvTrack.DataSource = objTrack;
            // gvTrack.DataBind();
            if (blnIsFooterOnly)
            {
                objTrack.Rows.Clear();
                ViewState["CustomerTrackDetails"] = objTrack;

                //gvTrack.Rows[0].Cells.Clear();
                //gvTrack.Rows[0].Visible = false;
            }
            FunPriGenerateNewAlertRow();


        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void FunPriGenerateNewAlertRow()
    {
        try
        {

            //DropDownList ddlLOBTrack = gvTrack.FooterRow.FindControl("ddlLOBTrack") as DropDownList;
            //DropDownList ddlAccountNo = gvTrack.FooterRow.FindControl("ddlAccountNo") as DropDownList;
            //DropDownList ddlType = gvTrack.FooterRow.FindControl("ddlType") as DropDownList;

            //Utility.FillDLL(ddlLOBTrack, ((DataTable)ViewState["TrackLOB"]), true);
            //Utility.FillDLL(ddlAccountNo, ((DataTable)ViewState["TrackAccountNo"]), true);
            //Utility.FillDLL(ddlType, ((DataTable)ViewState["TrackType"]), true);

            //ddlLOBTrack.Focus();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Track_AddRow_OnClick(object sender, EventArgs e)
    {

        DataTable objTrack = (DataTable)ViewState["CustomerTrackDetails"];

        //DropDownList ddlLOBTrack = gvTrack.FooterRow.FindControl("ddlLOBTrack") as DropDownList;
        //DropDownList ddlAccountNo = gvTrack.FooterRow.FindControl("ddlAccountNo") as DropDownList;
        //DropDownList ddlType = gvTrack.FooterRow.FindControl("ddlType") as DropDownList;
        //TextBox txtDate = gvTrack.FooterRow.FindControl("txtDate") as TextBox;
        //TextBox txtReleaseDate = gvTrack.FooterRow.FindControl("txtReleaseDate") as TextBox;
        //TextBox txtReason = gvTrack.FooterRow.FindControl("txtReason") as TextBox;
        //if (ddlLOBTrack.SelectedIndex <= 0)
        //{
        //    Utility.FunShowAlertMsg(this, "Select the Line of Business");
        //    return;
        //}
        //if (ddlAccountNo.SelectedIndex <= 0)
        //{
        //    Utility.FunShowAlertMsg(this, "Select the Account No.");
        //    return;
        //}
        //if (ddlType.SelectedIndex <= 0)
        //{
        //    Utility.FunShowAlertMsg(this, "Select the Account No.");
        //    return;
        //}
        //if (string.IsNullOrEmpty(txtReleaseDate.Text))
        //{
        //    Utility.FunShowAlertMsg(this, "Select the Release Date");
        //    return;
        //}
        //if (string.IsNullOrEmpty(txtDate.Text))
        //{
        //    Utility.FunShowAlertMsg(this, "Select the Date");
        //    return;
        //}
        DataRow drTrack = objTrack.NewRow();
        //drTrack["LOB_ID"] = ddlLOBTrack.SelectedValue;
        //drTrack["LOB_NAME"] = ddlLOBTrack.SelectedItem.Text;
        //drTrack["PA_SA_REF_ID"] = ddlAccountNo.SelectedValue;
        //drTrack["ACCOUNTNO"] = ddlAccountNo.SelectedItem.Text;
        //drTrack["DATE"] = txtDate.Text;
        //drTrack["RELEASEDATE"] = txtReleaseDate.Text;
        //drTrack["LOGINBY"] = ObjUserInfo.ProUserNameRW;
        //drTrack["RELEASED_BY"] = intUserId.ToString();
        //drTrack["RELEASEDBY"] = ObjUserInfo.ProUserNameRW;
        //drTrack["REASON"] = txtReason.Text;
        //drTrack["TRACK_TYPE"] = ddlType.SelectedItem.Text;
        //drTrack["TRACK_TYPE_ID"] = ddlType.SelectedValue;
        //objTrack.Rows.Add(drTrack);

        gvTrack.DataSource = objTrack;
        // gvTrack.DataBind();
        //ViewState["CustomerTrackDetails"] = objTrack;
        //FunPriGenerateNewAlertRow();
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Please select Email or SMS');", true);

        //}
    }

    protected void gvTrack_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //try
        //{
        //    if (e.Row.RowType == DataControlRowType.Footer)
        //    {
        //        TextBox txtDate = e.Row.FindControl("txtDate") as TextBox;
        //        TextBox txtReleaseDate = e.Row.FindControl("txtReleaseDate") as TextBox;
        //        DropDownList ddlLOBTrack = e.Row.FindControl("ddlLOBTrack") as DropDownList;
        //        DropDownList ddlAccountNo = e.Row.FindControl("ddlAccountNo") as DropDownList;
        //        DropDownList ddlType = e.Row.FindControl("ddlType") as DropDownList;

        //        txtDate.Attributes.Add("readonly", "readonly");
        //        AjaxControlToolkit.CalendarExtender CalendarDate = e.Row.FindControl("CalendarDate") as AjaxControlToolkit.CalendarExtender;
        //        CalendarDate.Format = strDateFormat;


        //        txtReleaseDate.Attributes.Add("readonly", "readonly");
        //        AjaxControlToolkit.CalendarExtender CalendarReleaseDate = e.Row.FindControl("CalendarReleaseDate") as AjaxControlToolkit.CalendarExtender;
        //        CalendarReleaseDate.Format = strDateFormat;

        //    }
        //}
        //catch (Exception ex)
        //{
        //    ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        //    cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to Set the DateFormat in Customer Track Details";
        //    cvCustomerMaster.IsValid = false;
        //}
    }

    protected void gvTrack_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        //try
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Label lblLOBName = (Label)e.Row.FindControl("lblgvLOBName");
        //        Label lblAccountNo = (Label)e.Row.FindControl("lblAccountNo");
        //        Label lblTypeId = (Label)e.Row.FindControl("lblTypeId");
        //        Label lblDate = (Label)e.Row.FindControl("lblDate");
        //        Label lblReason = (Label)e.Row.FindControl("lblReason");
        //        Label lblReleaseDate = (Label)e.Row.FindControl("lblReleaseDate");
        //        Label lblReleasedBy = (Label)e.Row.FindControl("lblReleasedBy");
        //        if (ViewState["CustomerTrackDetails"] != null)
        //        {
        //            DataTable dtCustomerTrack = (DataTable)ViewState["CustomerTrackDetails"];
        //            lblLOBName.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["LOB_NAME"]);
        //            lblAccountNo.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["ACCOUNTNO"]);
        //            lblReason.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["REASON"]);
        //            lblDate.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["DATE"]);
        //            lblReleaseDate.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASEDATE"]);
        //            lblTypeId.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["TRACK_TYPE"]);
        //            if (dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASEDBY"].ToString() == "")
        //            {
        //                lblReleasedBy.Text = intUserId.ToString();
        //                e.Row.Cells[8].Text =
        //                e.Row.Cells[11].Text = ObjUserInfo.ProUserNameRW;
        //            }
        //            else
        //            {
        //                lblReleasedBy.Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASED_BY"]);
        //                e.Row.Cells[8].Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["LOGINBY"]);
        //                e.Row.Cells[11].Text = Convert.ToString(dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASEDBY"]);
        //            }
        //        }

        //    }
        //}
        //catch (Exception ex)
        //{
        //    ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        //    throw ex;
        //}
    }
    #endregion
    private void FunPriLoadCustomerTrackDetails(int intCustomerId)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 303;
            ObjStatus.Param1 = intCustomerId.ToString();
            ObjStatus.Param2 = intUserId.ToString();
            ObjStatus.Param3 = intCompanyId.ToString();
            DataTable ObjCustomerTrackDetails = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            ViewState["CustomerTrackDetails"] = ObjCustomerTrackDetails;
            FunPriBindCustomerTrackDLL();
            //if (gvTrack.Rows.Count < 1 && gvTrack.FooterRow.Visible)
            //{
            //    lblNoCustomerTrack.Visible = true;
            //    // Need to comment by Manikandan. R
            //    gvTrack.Visible = false;
            //    divTrack.Visible = false;
            //}
            //else
            //{
            //    lblNoCustomerTrack.Visible = false;
            //    gvTrack.Visible = true;
            //    divTrack.Visible = true;
            //}

            FunPriBindGrid();


            //Dictionary<string, string> objProcedureParameters = new Dictionary<string, string>();
            //objProcedureParameters.Add("@CustomerId", intCustomerId.ToString());
            //objProcedureParameters.Add("@CompanyId", intCompanyId.ToString());
            //DataSet dSet = Utility.GetDataset("s3g_org_LoadCustomerCreditDetailsOPC", objProcedureParameters);

            ////gvCredit.DataSource = dSet.Tables[0];//Utility.GetDefaultData("s3g_org_LoadCustomerCreditDetails", objProcedureParameters);
            ////gvCredit.DataBind();

            //if (dSet != null && dSet.Tables.Count > 0)
            //{
            //    ViewState["dtFacilityGroup"] = dSet.Tables[0];
            //    ViewState["dtFacilityGrid"] = dSet.Tables[0];

                

            //    //grvFacilityGroup.DataSource = dSet.Tables[0];
            //    //grvFacilityGroup.DataBind();
            //    //if (grvFacilityGroup.Rows.Count > 0)
            //    //    btnExportToExcel.Enabled = true;
            //    //else
            //    //    btnExportToExcel.Enabled = false;
            //}

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load the Customer Track Details");
        }
        finally
        {
            objCustomerMasterClient.Close();
        }

    }

    private void FunPriBindGrid()
    {
        try
        {
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = "";
            ObjPaging.ProOrderBy ="";

            //FunPriGetSearchValue();

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CustomerId", intCustomerId.ToString());
            if (ViewState["LoadCredit"] == null)
            {
                Procparam.Add("@Param1", "0");
            }
            else
            {
                Procparam.Add("@Param1", "1");
            }
            grvFacilityGroup.BindGridView("s3g_org_LoadCustomerCreditDetailsOPC", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            ViewState["LoadCredit"] = "1";
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvFacilityGroup.Rows[0].Visible = false;
            }

            //FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }

    }

    /// <summary>
    /// Toggle the Controls based on User's AccessPermission
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            if (!bQuery)
            {
                Response.Redirect(strRedirectPageView);
            }


            switch (intModeID)
            {
                case 0: // Create Mode
                    try
                    {
                        txtComCountry.SelectedValue = "India";
                    }
                    catch (Exception ex)
                    {
                        txtComCountry.SelectedIndex = 0;
                    }
                    
                    chkCustomer.Checked = true;
                    FunPriToggleLoanAdminControls(false);
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    txtCustomercode.Enabled = true;
                    txtCustomerName.Enabled = true;
                    btnClear.Enabled = true;
                    //ddlGender.SelectedIndex = 0;
                    if ((!bCreate))
                    {
                        btnSave.Enabled = false;
                    }
                    btnModify.Enabled = false;
                    btnModifyAddress.Enabled = false;
                    if (Convert.ToInt64(intCRMID) > 0)
                    {
                        FunPriLoadCRMCustDtl();
                        btnClear.Enabled = false;
                    }
                    if (ddlPostingGroup.Items.Count > 0) ddlPostingGroup.SelectedIndex = 1;
                    //txtGroupName.Attributes.Add("onblur", "Calculate(" + txtGroupName.ClientID + "," + txtBusinessProfile.ClientID + ")");
                    break;
                case 1: // Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    FunProLoadStateCombos();
                    FunPriSetControlSettings();
                    FunPriLoadCustomerDetails(intCustomerId);
                    FunPriToggleCustomerTypeControls();
                    btnModify.Enabled = false;
                    btnModifyAddress.Enabled = false;
                    tcCustomerMaster.ActiveTabIndex = 0;
                    lblCustomercode.Visible = true;
                    txtCustomercode.Visible = true;
                    txtFamilyName.ReadOnly = true;
                    //Lined by Thangam M 0n 28/Feb/2012 for bug id 5453
                    //txtCustomerName.ReadOnly = true;
                    //End here

                    //txtAge.ReadOnly = true;
                    if ((!bModify))
                    {
                        btnSave.Enabled = false;
                    }
                    FunPriLoadBillEmailDet();
                    btnClear.Enabled = false;
                    Dictionary<string, string> ProcParam = new Dictionary<string, string>();
                    ProcParam.Add("@COMPANYID", intCompanyId.ToString());
                    ProcParam.Add("@CUSTOMERID", intCustomerId.ToString());
                    DataSet dsAccountCount = Utility.GetDataset("S3G_ORG_GETACCOUNTCOUNT", ProcParam);
                    if (Convert.ToInt32(dsAccountCount.Tables[0].Rows[0][0]) > 0)
                    {
                        //txtCustomerName.ReadOnly = true;
                        if (bClearList)
                        {
                            ddlTitle.RemoveDropDownList();
                            ddlCustomerType.RemoveDropDownList();
                            ddlConstitutionName.RemoveDropDownList();
                            ddlPostingGroup.RemoveDropDownList();
                            ddlCompanyType.RemoveDropDownList();

                        }
                        //CalendarExtenderSD.Enabled = false;
                        //cmbIndustryCode.ReadOnly = txtIndustryName.ReadOnly = true;
                    }
                    ddlTitle.RemoveDropDownList();
                    ddlCustomerType.RemoveDropDownList();
                    ddlConstitutionName.RemoveDropDownList();
                    ddlPostingGroup.ClearDropDownList();
                   
                    break;
                case -1:// Query Mode
                    FunPriSetControlSettings();
                    FunProLoadAddressCombos();
                    FunProLoadStateCombos();
                    FunPriLoadCustomerDetails(intCustomerId);
                    FunPriToggleCustomerTypeControls();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    tcCustomerMaster.ActiveTabIndex = 0;
                    lblCustomercode.Visible = true;
                    ddlCustomerType.RemoveDropDownList();
                    txtCustomercode.Visible = true;
                    FunPriToggleQueryModeControls();
                    FunPriLoadBillEmailDet();
                    btnAddGroup.Enabled = false;
                    btnClearEmailGroup.Enabled = false;
                    gvBAddress.Attributes.Remove("onmouseover");
                    gvBAddress.Attributes.Remove("onmouseout");
                    gvBAddress.Enabled = false;
                    foreach (GridViewRow drow in gvBAddress.Rows)
                    {
                        LinkButton lnkDelete = (LinkButton)drow.FindControl("lnkDelete");
                        lnkDelete.Enabled = false;
                    }
                    grvBankDetails.Enabled = false;
                    foreach (GridViewRow drow in grvBankDetails.Rows)
                    {
                        LinkButton lnkbtnDelete = (LinkButton)drow.FindControl("lnkbtnDelete");
                        lnkbtnDelete.Enabled = false;
                    }
                    foreach (GridViewRow gvRow in grvFacilityGroup.Rows)
                    {
                        CheckBox chkDeactivate = (CheckBox)gvRow.FindControl("chkDeactivate");
                        chkDeactivate.Enabled = false;
                    }

                    cmbGroupCode.Enabled = false;
                    cmbIndustryCode.Enabled = false;

                    //btnCopyAddress.Visible = false;
                    chkCustomer.Enabled = false;
                    chkGuarantor1.Enabled = false;
                    chkGuarantor2.Enabled = false;
                    chkCoApplicant.Enabled = false;
                    rbCreditType.Enabled = false;
                    //btnSave.Enabled = false;
                    //btnClear.Enabled = false;
                    //btnAddAddress.Enabled = false;
                    btnModifyAddress.Enabled = false;
                    btnExportToExcel.Enabled = false;
                    ddlConstitutionName.RemoveDropDownList();
                    ddlPostingGroup.ClearDropDownList();
                    ddlBillAddress.RemoveDropDownList();
                    ddlInvCovLetter.ClearDropDownList();
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriToggleQueryModeControls()
    {
        txtCustomerName.ReadOnly = true;
        tblBankDetails.Visible = false;
        if (bClearList)
        {

            ddlConstitutionName.RemoveDropDownList();
            //ddlGender.ClearDropDownList();
            //ddlGovernment.RemoveDropDownList();
            //ddlHouseType.RemoveDropDownList();

            //ddlMaritalStatus.RemoveDropDownList();
            // ddlOwn.RemoveDropDownList();
            ddlPostingGroup.RemoveDropDownList();
            ddlPublic.RemoveDropDownList();
            ddlTitle.RemoveDropDownList();
            ddlCompanyType.RemoveDropDownList();
        }
        //CalendarExtenderSD.Enabled = false;
        //<<Performance>>
        //txtComCity.DropDownStyle = 
        txtComCity.ReadOnly = true;
        //txtComState.DropDownStyle = txtComCountry.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
        //txtComCity.ClearDropDownList();
        ddlComState.ClearDropDownList();
        txtComCountry.ClearDropDownList();
        txtComCountry.Enabled = false;
        ddlPerState.Enabled = false;
        //<<Performance>>
        //txtPerCity.DropDownStyle = 
        // txtPerCountry.DropDownStyle = txtPerState.DropDownStyle = AjaxControlToolkit.ComboBoxStyle.DropDownList;
        //txtPerCity.ClearDropDownList();
        txtPerCity.ReadOnly = true;
        //if (txtPerState.SelectedItem.Text!=null)
        //    txtPerState.ClearDropDownList();

        //txtPerCountry.ClearDropDownList();
        chkBadTrack.Enabled = false;
        chkHotListed.Enabled = chkPOBlack.Enabled = false;
        //txtComCity.ReadOnly = txtComCountry.ReadOnly = txtComState.ReadOnly =
        // = txtPerCity.ReadOnly = txtPerCountry.ReadOnly = txtPerState.ReadOnly = 
        cmbGroupCode.ReadOnly = cmbIndustryCode.ReadOnly =
        txtIndustryName.ReadOnly = txtGroupName.ReadOnly =
        txtComAddress1.ReadOnly = txtCOmAddress2.ReadOnly = txtComEmail.ReadOnly = txtPAN.ReadOnly = txtCIN.ReadOnly = txtTIN.ReadOnly = txtTAN.ReadOnly =
        txtComMobile.ReadOnly = txtComPincode.ReadOnly = txtComTelephone.ReadOnly = txtComWebsite.ReadOnly =
        txtPerAddress1.ReadOnly = txtPerAddress2.ReadOnly = txtPerEmail.ReadOnly = txtPerContName.ReadOnly = txtPerContactNo.ReadOnly = txtPerTIN.ReadOnly = txtPerTAN.ReadOnly =
        txtDirectors.ReadOnly = txtSE.ReadOnly = txtPaidCapital.ReadOnly = txtfacevalue.ReadOnly = txtbookvalue.ReadOnly = txtBusinessProfile.ReadOnly = txtAccountNumber.ReadOnly = txtBankAddress.ReadOnly = txtBankBranch.ReadOnly =
        txtBankName.ReadOnly = txtMICRCode.ReadOnly = txtReson.ReadOnly = txtCustomerRating.ReadOnly =  txtPercentageStake.ReadOnly = txtCEOName.ReadOnly = txtNotes.ReadOnly =
        txtFamilyName.ReadOnly = txtCGSTin.ReadOnly = TxtCGSTRegDate.ReadOnly = txtSGSTin.ReadOnly = TxtSGSTRegDate.ReadOnly = true;
        imgCGSTRegDate.Enabled = imgSGSTRegDate.Enabled = false;

        btnAdd.Enabled = btnModify.Enabled = btnModifyAddress.Enabled = btnClear.Enabled = btnSave.Enabled = btnAddAddress.Enabled = false;
        /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
        //grvBankDetails.Columns[11].Visible = false;
        //if (gvTrack.Rows.Count > 0)
        //{
        //    if (gvTrack.Columns[12] != null) gvTrack.Columns[12].Visible = false;
        //    //gvTrack.FooterRow.Visible = false;
        //}
    }
    private void FunPriToggleModifyModeControls()
    {

    }


    #endregion

    #region Dropdown Events



    protected void ddlRSNum_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriSetSLCodeList(ddlRSNum, ddlSANum);
    }

    private void FunPriSetSLCodeList(DropDownList ddlRSNum, DropDownList ddlSANum)
    {
        try
        {
            if (ddlRSNum.SelectedValue != "0")
            {
                Dictionary<string, string> dictParam = null;
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Customer_ID", intCustomerId.ToString());
                dictParam.Add("@RSNum", ddlRSNum.SelectedValue);
                ddlSANum.BindDataTable("S3G_ORG_GETSANUM", dictParam, new string[] { "TRANCHE_HEADER_ID", "TRANCHE_NAME" });
            }
            else
            {
                ddlSANum.ClearDropDownList();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, "TDS Master");
            throw ex;
        }
    }

    protected void ddlCustomerType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriToggleCustomerTypeControls();
            ddlCustomerType.Focus();
            grvBankDetails.DataSource = null;
            grvBankDetails.DataBind();

            //Added By Thangam M on 09/mar/2012 to fix bug id - 6031
            FunProIntializeData();
            //End

            if (ddlCustomerType.SelectedItem.ToString().ToLower() == "non individual")
            {
                
                //rfvPerEmail.Enabled = true;

            }
            else
            {
                txtIndustryName.Text = "";
                cmbIndustryCode.Text = "";
              
                //rfvPerEmail.Enabled = false;
            }
            upAddress.Update();
            upPersonal.Update();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.CustomerTypeChangeError;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ddlConstitutionName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {

            if (Convert.ToInt32(ddlConstitutionName.SelectedValue) > 0)
            {
                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                ObjStatus.Option = 7;
                ObjStatus.Param1 = ddlConstitutionName.SelectedValue.ToString();
                gvConstitutionalDocuments.DataSource = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
                gvConstitutionalDocuments.DataBind();

                //Added By Thangam on 12/Mar/2012 to fix bug id - 5451
                FunProClearUploader();

                ViewState["ConstitutionDocument"] = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
                ddlConstitutionName.Focus();
                ObjStatus.Option = 787;
                ObjStatus.Param1 = intCompanyId.ToString();
                DataTable dtConsDocPath = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
                if (dtConsDocPath == null)
                {
                    Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Consitution Document");
                    return;
                }
                if (dtConsDocPath.Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Define the spooling path in Document Path Setup for Consitution Document");
                    return;
                }
                ViewState["ConsDocPath"] = dtConsDocPath.Rows[0][0].ToString();
            }
            else
            {
                tcCustomerMaster.Tabs[3].Enabled = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.ConstitutionChangeError;
            cvCustomerMaster.IsValid = false;
        }
        finally
        {
            objCustomerMasterClient.Close();
            upConstitution.Update();
        }
    }

    protected void txtIndustryName_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtBusinessProfile.Text = txtIndustryName.Text;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.GroupChangeError;
            cvCustomerMaster.IsValid = false;
        }
    }


    protected void cmbGroupCode_OnTextChanged(object sender, EventArgs e)
    {
        try
        {

            txtGroupName.Text = string.Empty;
            txtGroupName.Focus();

            DataTable dt = (DataTable)HttpContext.Current.Session["GroupDT"];

            if (dt.Rows.Count > 0)
            {
                string filterExpression = "Value = '" + cmbGroupCode.Text + "'";
                DataRow[] dtSuggestions = dt.Select(filterExpression);

                if (dtSuggestions.Length > 0)
                {
                    txtGroupName.Text = dtSuggestions[0]["Description"].ToString();
                    //txtGroupName.Attributes.Add("onblur", "Calculate(" + txtGroupName.ClientID + "," + txtBusinessProfile.ClientID + ")");
                    //txtGroupName.ReadOnly = true;
                    txtGroupName.ReadOnly = false;
                }
                else
                {
                    txtGroupName.ReadOnly = false;
                }
            }
            if (cmbGroupCode.Text == string.Empty)
            {
                //txtGroupName.ReadOnly = false;
                txtGroupName.ReadOnly = true;
                txtGroupName.Text = string.Empty;
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.GroupChangeError;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void cmbIndustryCode_OnTextChanged(object sender, EventArgs e)
    {
        txtIndustryName.Text = string.Empty;
        try
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["IndustryDT"];

            if (dt.Rows.Count > 0)
            {
                string filterExpression = "Value = '" + cmbIndustryCode.Text + "' and Company_Id = " + intCompanyId.ToString();
                DataRow[] dtSuggestions = dt.Select(filterExpression);
                if (dtSuggestions.Length > 0)
                {
                    txtIndustryName.Text = dtSuggestions[0]["Description"].ToString();
                    //txtIndustryName.ReadOnly = true;
                    txtIndustryName.ReadOnly = false;
                }
                else
                {
                    txtIndustryName.ReadOnly = false;
                }
            }
            if (cmbIndustryCode.Text == string.Empty)
            {
                //txtIndustryName.ReadOnly = false;
                txtIndustryName.ReadOnly = true;
                txtIndustryName.Text = string.Empty;
            }
            upIndustryCode.Update();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Load the Industry Name";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region Grid Events

    /// <summary>
    /// Disable the "Values" Control in Constitutional Document Tab
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvConstitutionalDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPath = e.Row.FindControl("myThrobber") as Label;
                LinkButton hlnkView = e.Row.FindControl("hlnkView") as LinkButton;
                CheckBox chkIsMandatory = e.Row.FindControl("chkIsMandatory") as CheckBox;
                CheckBox chkIsNeedImageCopy = e.Row.FindControl("chkIsNeedImageCopy") as CheckBox;
                //AjaxControlToolkit.AsyncFileUpload asyFileUpload = e.Row.FindControl("asyFileUpload") as AjaxControlToolkit.AsyncFileUpload;
                CheckBox chkCollected = e.Row.FindControl("chkCollected") as CheckBox;
                CheckBox chkScanned = e.Row.FindControl("chkScanned") as CheckBox;
                TextBox txtValues = e.Row.FindControl("txtValues") as TextBox;


                FileUpload flUpload = (FileUpload)e.Row.FindControl("flUpload");
                //Button btnDlg = (Button)e.Row.FindControl("btnDlg");
                //HiddenField hdnSelectedPath = (HiddenField)e.Row.FindControl("hdnSelectedPath");
                Label lblActualPath = (Label)e.Row.FindControl("lblActualPath");
                TextBox txtFileupld = e.Row.FindControl("txtFileupld") as TextBox;
                Button btnBrowse = (Button)e.Row.FindControl("btnBrowse");

                if (!chkIsNeedImageCopy.Checked)
                {
                    chkScanned.Enabled = false;
                }

                // AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)e.Row.FindControl("asyFileUpload");
                if (lblActualPath != null)
                {
                    if (lblActualPath.Text.Trim() != "")
                    {
                        hlnkView.Enabled = true;
                    }
                    else
                    {
                        hlnkView.Enabled = false;
                    }
                }
                else
                {
                    hlnkView.Enabled = false;
                }

                if (!chkIsNeedImageCopy.Checked)
                {
                    txtFileupld.Visible =
                    flUpload.Visible = false;
                    hlnkView.Enabled = false;
                }



                TextBox txtVal = (TextBox)e.Row.FindControl("txtValues");
                // Commented By R. Manikandan Enable value field to enter the Remarks on 07-JAN-2013

                //if (txtVal != null)
                //{
                //    txtVal.Enabled = FunPriDisableValueField(e.Row.Cells[1].Text);
                //}
                if (!string.IsNullOrEmpty(Request.QueryString["qsMode"]))
                {
                    if (Request.QueryString["qsMode"].ToString() == "Q")
                    {
                        txtFileupld.Visible =
                    flUpload.Visible =
                        chkIsMandatory.Enabled = chkIsNeedImageCopy.Enabled = chkCollected.Enabled =
                            chkScanned.Enabled = txtValues.Enabled = false;
                    }
                }



                //flUpload.Attributes.Add("onchange", "fnAssignPath('" + flUpload.ClientID + "','" + hdnSelectedPath.ClientID + "', '" + btnBrowse.ClientID + "'); ");
                flUpload.Attributes.Add("onchange", "fnLoadPath('" + btnBrowse.ClientID + "'); ");

                //fnLoadPath('" + btnBrowse.ClientID + "');

            }
            //e.Row.Cells[11].Visible = false;

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Toggle the Values in Constitution Document Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    //Added by Thangam M on 12/Mar/2012 to fix bug id - 5451
    private void FunPriClearUploader(AjaxControlToolkit.AsyncFileUpload Uploader)
    {
        if (Uploader != null)
        {
            HttpContext crntContext;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                crntContext = HttpContext.Current;
            }
            else
            {
                crntContext = null;
            }
            if (crntContext != null)
            {
                foreach (string key in crntContext.Session.Keys)
                {
                    if (key.Contains(Uploader.ClientID))
                    {
                        crntContext.Session.Remove(key);
                    }
                }
            }
        }
    }

    //Added by Thangam M on 12/Mar/2012 to fix bug id - 5451
    protected void FunProClearUploader()
    {
        try
        {
            if (gvConstitutionalDocuments.Rows.Count > 0)
            {
                for (int i = 0; i <= gvConstitutionalDocuments.Rows.Count - 1; i++)
                {
                    LinkButton hlnkView = gvConstitutionalDocuments.Rows[i].FindControl("hlnkView") as LinkButton;
                    AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)gvConstitutionalDocuments.Rows[i].FindControl("asyFileUpload");
                    FunPriClearUploader(AsyncFileUpload1);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Clear", "javascript:fnClearAsyncUploader('" + gvConstitutionalDocuments.Rows.Count.ToString() + "');", true);

            }
        }
        catch (Exception)
        {

        }
    }




    protected void gvCustomerTrack_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlType");
                OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
                ObjStatus.Option = 1;
                ObjStatus.Param1 = S3G_Statu_Lookup.CATEGORY_TYPE.ToString();
                Utility.FillDLL(ddlType, objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus));

                if (ViewState["CustomerTrackDetails"] != null)
                {
                    DataTable dtCustomerTrack = (DataTable)ViewState["CustomerTrackDetails"];
                    ddlType.SelectedValue = dtCustomerTrack.Rows[e.Row.RowIndex]["TRACK_TYPE_ID"].ToString();
                    if (dtCustomerTrack.Rows[e.Row.RowIndex]["RELEASEDBY"].ToString() == "")
                    {
                        Label lblReleasedBy = (Label)e.Row.FindControl("lblReleasedBy");
                        lblReleasedBy.Text = intUserId.ToString();
                        e.Row.Cells[10].Text = ObjUserInfo.ProUserNameRW;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected void gvCustomerTrack_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDate = e.Row.FindControl("txtDate") as TextBox;
                txtDate.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarDate = e.Row.FindControl("CalendarDate") as AjaxControlToolkit.CalendarExtender;
                CalendarDate.Format = ObjS3GSession.ProDateFormatRW;

                TextBox txtReleaseDate = e.Row.FindControl("txtReleaseDate") as TextBox;
                txtReleaseDate.Attributes.Add("readonly", "readonly");
                AjaxControlToolkit.CalendarExtender CalendarReleaseDate = e.Row.FindControl("CalendarReleaseDate") as AjaxControlToolkit.CalendarExtender;
                CalendarReleaseDate.Format = ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to Set the DateFormat in Customer Track Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region TextBox Events

    /// <summary>
    /// Assign the Age field while DOB Changing 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtDOB_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            // txtAge.Text = string.Empty;
            //if (!string.IsNullOrEmpty(txtDOB.Text))
            //{
            //    int intDOBYear = Utility.StringToDate(txtDOB.Text).Year;
            //    txtAge.Text = ((DateTime.Now.Year - intDOBYear) + 1).ToString();
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.AgeCalculateError;
            cvCustomerMaster.IsValid = false;
        }
    }
    #endregion

    #region Bank Details Tab
    protected void FunProIntializeData()
    {
        DataTable dtBankViewDetails = new DataTable("BankDetails");
        DataTable dtGroupViewDetails = new DataTable("GroupDetails");
        try
        {
            dtBankViewDetails.Columns.Add("Name");
            dtBankViewDetails.Columns.Add("ID");
            dtBankViewDetails.Columns.Add("Account_Number");
            dtBankViewDetails.Columns.Add("Bank_Name");
            dtBankViewDetails.Columns.Add("Branch_Name");
            dtBankViewDetails.Columns.Add("IFSC_Code");
            dtBankViewDetails.Columns.Add("PANUM");
            dtBankViewDetails.Columns.Add("Tranche_Header_ID");
            dtBankViewDetails.Columns.Add("Tranche_Name");
            dtBankViewDetails.Columns.Add("MICR_Code");
            dtBankViewDetails.Columns.Add("Bank_Address");
            dtBankViewDetails.Columns.Add("Customer_Bank_ID");
            dtBankViewDetails.Columns.Add("Is_Default_Account", typeof(bool));
            //dtBankViewDetails.Columns[4].Unique = true;
            ViewState["DetailsTable"] = dtBankViewDetails;

            dtGroupViewDetails.Columns.Add("Cust_Email_Det_ID");
            dtGroupViewDetails.Columns.Add("State_ID");
            dtGroupViewDetails.Columns.Add("State_Name");
            dtGroupViewDetails.Columns.Add("Group_Name");
            dtGroupViewDetails.Columns.Add("Email_ID");
            dtGroupViewDetails.Columns.Add("Email_CC");
            ViewState["EmailDetailsTable"] = dtGroupViewDetails;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Initialize the Data for BankDetails");
        }
    }

    protected void btnAddAddress_Click(object sender, EventArgs e)
    {
        //if (txtPerEmail.Text.Trim() == "")
        //{
        //    Utility.FunShowAlertMsg(this, "Email is mandatory in Billing Address");
        //    tcCustomerMaster.ActiveTab = tbAddress;
        //    txtPerEmail.Focus();
        //    return;
        //}


        //if (txtPerTAN.Text.Trim() == "")
        //{
        //    Utility.FunShowAlertMsg(this, "TAN number is mandatory in Billing Address");
        //    tcCustomerMaster.ActiveTab = tbAddress;
        //    txtPerTAN.Focus();
        //    return;
        //}

        //if (txtPerTIN.Text != "")
        //{
        //    if (txtPerTIN.Text.Length != 9)
        //    {
        //        Utility.FunShowAlertMsg(this, "TIN Number should be of 9 characters in Branch Address");
        //        tcCustomerMaster.ActiveTab = tbAddress;
        //        txtPerTIN.Focus();
        //        return;
        //    }

        //    //foreach (GridViewRow row in gvBAddress.Rows)
        //    //{
        //    //    Label lblgTIN = (Label)row.Cells[5].FindControl("lblgTIN");
        //    //    Label lblState = (Label)row.Cells[5].FindControl("lblState");
        //    //    if (txtPerTIN.Text.Trim() == lblgTIN.Text.ToString())
        //    //    {
        //    //        if (ddlPerState.SelectedItem.ToString() != lblState.Text.ToString())
        //    //        {
        //    //            Utility.FunShowAlertMsg(this, "TIN Number already defined in Billing Address for other state");
        //    //            tcCustomerMaster.ActiveTab = tbAddress;
        //    //            txtPerTIN.Focus();
        //    //            return;
        //    //        }
        //    //    }
        //    //}
        //}



        //if (txtPerTAN.Text.Length != 10)
        //{
        //    Utility.FunShowAlertMsg(this, "TAN Number should be of 10 characters in Branch Address");
        //    tcCustomerMaster.ActiveTab = tbAddress;
        //    txtPerTAN.Focus();
        //    return;
        //}

        //foreach (GridViewRow row in gvBAddress.Rows)
        //{
        //    Label lblgTAN = (Label)row.Cells[6].FindControl("lblgTAN");
        //    Label lblState = (Label)row.Cells[5].FindControl("lblState");
        //    if (txtPerTAN.Text.Trim() == lblgTAN.Text.ToString())
        //    {
        //        if (ddlPerState.SelectedItem.ToString() != lblState.Text.ToString())
        //        {
        //            Utility.FunShowAlertMsg(this, "TAN Number already defined in Billing address for other state");
        //            tcCustomerMaster.ActiveTab = tbAddress;
        //            txtPerTAN.Focus();
        //            return;
        //        }
        //    }
        //}

        foreach (GridViewRow row in gvBAddress.Rows)
        {
            Label lblSGSTIN = (Label)row.Cells[6].FindControl("lblSGSTIN");
            Label lblState = (Label)row.Cells[5].FindControl("lblState");
            if (txtSGSTin.Text.Trim() != "" && txtSGSTin.Text.Trim() == lblSGSTIN.Text.ToString())
            {
                if (ddlPerState.SelectedItem.ToString() != lblState.Text.ToString())
                {
                    Utility.FunShowAlertMsg(this, "GSTIN already defined in Billing address for other state");
                    tcCustomerMaster.ActiveTab = tbAddress;
                    txtSGSTin.Focus();
                    return;
                }
            }
        }

        //DataTable dtExist = new DataTable();
        //OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        //Procparam = new Dictionary<string, string>();
        //Procparam.Add("@Option", "1");
        //Procparam.Add("@Param1", txtPerTIN.Text.Trim());
        //DataTable dt = Utility.GetDefaultData("S3G_ORG_CheckTINTAN", Procparam);

        //string CorporateTIN = "";
        //string CorporateTAN = "";

        //if (dt.Rows.Count > 0)
        //{
        //    CorporateTIN = Convert.ToString(dt.Rows[0]["TIN"]);
        //    CorporateTAN = Convert.ToString(dt.Rows[0]["TAN"]);
        //}

        DataTable dtBankDetails = new DataTable();
        DataRow drDetails;
        if (ViewState["BillingAddress"] != null)
            dtBankDetails = (DataTable)ViewState["BillingAddress"];
        // Exist Pooling Row Based On Select

        //foreach (DataRow MyDataRow in dtBankDetails.Select("TIN='" + txtPerTIN.Text + "' And TAN ='" + txtPerTAN.Text + "'"))
        //    dtExist.ImportRow(MyDataRow);

        //DataRow[] rowsToUpdate = dtExist.Select("State <> '" + txtPerState.SelectedValue.Trim() + "'");
        //if (rowsToUpdate.Length > 0)
        //{
        //    Utility.FunShowAlertMsg(this, "Selected Combination already exists");
        //    return;
        //}


        if (ViewState["BillingAddress"] != null)
        {
            drDetails = dtBankDetails.NewRow();
            string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
            drDetails["Rowno"] = strBankMapId;
            drDetails["LocationCat_Description"] = ddlPerState.SelectedItem.ToString().Trim();
			/* Thalai - avoid customer address ID vanish - 4771 - Start */
            drDetails["CUST_ADDRESS_ID"] = 0;
			/* Thalai - avoid customer address ID vanish - 4771 - End */
            drDetails["Location_Category_ID"] = ddlPerState.SelectedValue.Trim();
            drDetails["City"] = txtPerCity.SelectedText.Trim();
            drDetails["Address1"] = txtPerAddress1.Text.Trim();
            drDetails["Address2"] = txtPerAddress2.Text.Trim();
            if (txtPerPincode.Text.Trim() == "")
            {
                drDetails["Pincode"] ="0";
            }
            else
            {
                drDetails["Pincode"] = txtPerPincode.Text.Trim();
            }
            
            drDetails["Email"] = txtPerEmail.Text.Trim();
            drDetails["Contact_Name"] = txtPerContName.Text.Trim();
            drDetails["Contact_No"] = txtPerContactNo.Text.Trim();
            drDetails["TIN"] = txtPerTIN.Text.Trim();
            drDetails["TAN"] = txtPerTAN.Text.Trim();
            drDetails["SGSTIN"] = txtSGSTin.Text.Trim();
            drDetails["SGST_Reg_Date"] = TxtSGSTRegDate.Text.Trim();
        }
        else
        {
            dtBankDetails = new DataTable();
            dtBankDetails.Columns.Add("Rowno");
            dtBankDetails.Columns.Add("LocationCat_Description");
			/* Thalai - avoid customer address ID vanish - 4771 - Start */
            dtBankDetails.Columns.Add("CUST_ADDRESS_ID");
			/* Thalai - avoid customer address ID vanish - 4771 - End */
            dtBankDetails.Columns.Add("Location_Category_ID");
            dtBankDetails.Columns.Add("City");
            dtBankDetails.Columns.Add("Address1");
            dtBankDetails.Columns.Add("Address2");
            dtBankDetails.Columns.Add("PINCODE");
            dtBankDetails.Columns.Add("Email");
            dtBankDetails.Columns.Add("Contact_Name");
            dtBankDetails.Columns.Add("Contact_No");
            dtBankDetails.Columns.Add("TIN");
            dtBankDetails.Columns.Add("TAN");
            dtBankDetails.Columns.Add("SGSTIN");
            dtBankDetails.Columns.Add("SGST_Reg_Date");

            drDetails = dtBankDetails.NewRow();
            string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
            drDetails["Rowno"] = strBankMapId;
            drDetails["LocationCat_Description"] = ddlPerState.SelectedItem.Text.Trim();
			/* Thalai - avoid customer address ID vanish - 4771 - Start */
            drDetails["CUST_ADDRESS_ID"] = 0;
			/* Thalai - avoid customer address ID vanish - 4771 - End */
            drDetails["Location_Category_ID"] = ddlPerState.SelectedValue.Trim();
            drDetails["City"] = txtPerCity.SelectedText.Trim();
            drDetails["Address1"] = txtPerAddress1.Text.Trim();
            drDetails["Address2"] = txtPerAddress2.Text.Trim();
            if (txtPerPincode.Text.Trim() == "")
            {
                drDetails["PINCODE"] = "0";
            }
            else
            {
                drDetails["PINCODE"] = txtPerPincode.Text.Trim();
            }
          
            drDetails["Email"] = txtPerEmail.Text.Trim();
            drDetails["Contact_Name"] = txtPerContName.Text.Trim();
            drDetails["Contact_No"] = txtPerContactNo.Text.Trim();
            drDetails["TIN"] = txtPerTIN.Text.Trim();
            drDetails["TAN"] = txtPerTAN.Text.Trim();
            drDetails["SGSTIN"] = txtSGSTin.Text.Trim();
            drDetails["SGST_Reg_Date"] = TxtSGSTRegDate.Text.Trim();

        }
        dtBankDetails.Rows.Add(drDetails);
        gvBAddress.DataSource = dtBankDetails;
        gvBAddress.DataBind();
        ViewState["BillingAddress"] = dtBankDetails;
        ddlPerState.SelectedIndex = 0;
        txtPerCity.Clear();
        txtPerAddress1.Text = "";
        txtPerAddress2.Text = "";
        txtPerPincode.Text = "";
        txtPerEmail.Text = "";
        txtPerContactNo.Text = "";
        txtPerContName.Text = "";
        txtPerTAN.Text = "";
        txtPerTIN.Text = "";
        txtSGSTin.Text = "";
        TxtSGSTRegDate.Text = "";
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtBankAddress.Text.Length > 300)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('Bank Address Length should be less than or equal to 300');", true);
                tcCustomerMaster.ActiveTabIndex = 2;
                return;
            }
            if (txtIFSC_Code.Text.Length !=11)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('IFSC Code Length should be 11 Characters');", true);
                tcCustomerMaster.ActiveTabIndex = 2;
                txtIFSC_Code.Focus();
                return;
            }
            DataRow drDetails;
            DataTable dtBankDetails = (DataTable)ViewState["DetailsTable"];

            DataRow[] drDefault = dtBankDetails.Select("Is_Default_Account = 'True'");
            if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            {
                Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
                return;
            }

            if (dtBankDetails.Rows.Count < 10)
            {
                drDetails = dtBankDetails.NewRow();
                string strBankMapId = Convert.ToString(dtBankDetails.Rows.Count + 1);
                drDetails["Customer_Bank_ID"] = strBankMapId;
                // drDetails["Master_ID"] = "0";
                drDetails["Name"] = ddlAccountType.SelectedItem.Text;
                drDetails["ID"] = ddlAccountType.SelectedItem.Value;
                drDetails["Account_Number"] = txtAccountNumber.Text.Trim();
                drDetails["Bank_Name"] = txtBankName.Text.Trim();
                drDetails["Branch_Name"] = txtBankBranch.Text.Trim();
                drDetails["MICR_Code"] = txtMICRCode.Text.Trim();
                drDetails["IFSC_Code"] = txtIFSC_Code.Text.Trim();
                if (ddlRSNum.SelectedValue == "" || ddlRSNum.SelectedValue == "0")
                {
                    drDetails["PANUM"] = "0";
                }
                else
                {
                    drDetails["PANUM"] = ddlRSNum.SelectedValue;
                }
                if (ddlSANum.SelectedValue == "" || ddlSANum.SelectedValue == "0")
                {
                    drDetails["Tranche_Header_ID"] = 0;
                    drDetails["Tranche_Name"] = "";
                }
                else
                {
                    drDetails["Tranche_Header_ID"] = ddlSANum.SelectedValue;
                    drDetails["Tranche_Name"] = ddlSANum.SelectedItem.ToString();
                }
                
               
                drDetails["Bank_Address"] = txtBankAddress.Text.Trim();
                /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
                if (chkDefaultAccount.Checked == true)
                {
                    drDetails["Is_Default_Account"] = 1;
                }
                else
                {
                    drDetails["Is_Default_Account"] = 0;
                }

                drDetails["IFSC_Code"] = txtIFSC_Code.Text.Trim();
                if (ddlRSNum.SelectedIndex > 0)
                    drDetails["PANUM"] = ddlRSNum.SelectedValue;

                dtBankDetails.Rows.Add(drDetails);
                grvBankDetails.DataSource = dtBankDetails;
                grvBankDetails.DataBind();
                FunPriHideBankColumns();
                ViewState["DetailsTable"] = dtBankDetails;
                ClearBankDetailsControls();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Bank Details", "alert('Customer Bank Details should be less than or equal to 9 Banks');", true);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Add Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void btnModifyAddress_Click(object sender, EventArgs e)
    {
        
        if (txtPerTAN.Text.Trim() != "")
        {
            if (txtPerTAN.Text.Length != 10)
            {
                Utility.FunShowAlertMsg(this, "TAN Number should be of 10 characters in Billing Address");
                tcCustomerMaster.ActiveTab = tbAddress;
                txtPerTAN.Focus();
                return;
            }
        }

        foreach (GridViewRow row in gvBAddress.Rows)
        {
            Label lblSGSTIN = (Label)row.Cells[6].FindControl("lblSGSTIN");
            Label lblState = (Label)row.Cells[5].FindControl("lblState");
            if (!String.IsNullOrEmpty(txtSGSTin.Text) && txtSGSTin.Text.Trim() == lblSGSTIN.Text.ToString())
            {
                if (ddlPerState.SelectedItem.ToString() != lblState.Text.ToString())
                {
                    Utility.FunShowAlertMsg(this, "GSTIN already defined in Billing address for other state");
                    tcCustomerMaster.ActiveTab = tbAddress;
                    txtSGSTin.Focus();
                    return;
                }
            }
        }

        try
        {

            DataTable dtBankDetails = (DataTable)ViewState["BillingAddress"];
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            //DataRow[] drDefault = dtBankDetails.Select("IsDefaultAccount = 'True' and BankMapping_ID <> '" + hdnBankId.Value + "'");
            //if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            //{
            //    Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
            //    return;
            //}
            DataView dvBankdetails = dtBankDetails.DefaultView;
            dvBankdetails.Sort = "Rowno";
            int rowindex = Convert.ToInt16(hdnAddID.Value);
            dvBankdetails[rowindex].Row["LocationCat_Description"] = ddlPerState.SelectedItem.Text.Trim();
            dvBankdetails[rowindex].Row["Location_Category_ID"] = ddlPerState.SelectedValue.Trim();
            dvBankdetails[rowindex].Row["City"] = txtPerCity.SelectedText.Trim();
            dvBankdetails[rowindex].Row["Address1"] = txtPerAddress1.Text.Trim();
            dvBankdetails[rowindex].Row["Address2"] = txtPerAddress2.Text.Trim();
            if (txtPerPincode.Text.Trim() == "")
            {
                dvBankdetails[rowindex].Row["Pincode"] = "0";
            }
            else
            {
                dvBankdetails[rowindex].Row["Pincode"] = txtPerPincode.Text.Trim();
            }
            
            dvBankdetails[rowindex].Row["Email"] = txtPerEmail.Text.Trim();
            dvBankdetails[rowindex].Row["Contact_Name"] = txtPerContName.Text.Trim();
            dvBankdetails[rowindex].Row["Contact_No"] = txtPerContactNo.Text.Trim();
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            dvBankdetails[rowindex].Row["TIN"] = txtPerTIN.Text.Trim();
            dvBankdetails[rowindex].Row["TAN"] = txtPerTAN.Text.Trim();
            dvBankdetails[rowindex].Row["SGSTIN"] = txtSGSTin.Text.Trim();
            if (TxtSGSTRegDate.Text != "")
                dvBankdetails[rowindex].Row["SGST_Reg_Date"] = TxtSGSTRegDate.Text.Trim();
            else
                dvBankdetails[rowindex].Row["SGST_Reg_Date"] = "";
            gvBAddress.DataSource = dvBankdetails;
            gvBAddress.DataBind();
            ViewState["BillingAddress"] = dvBankdetails.Table;
            btnAddAddress.Enabled = true;
            btnModifyAddress.Enabled = false;

            ddlPerState.SelectedIndex = 0;
            txtPerCity.Clear();
            txtPerAddress1.Text = "";
            txtPerAddress2.Text = "";
            txtPerPincode.Text = "";
            txtPerEmail.Text = "";
            txtPerContactNo.Text = "";
            txtPerContName.Text = "";
            txtPerTAN.Text = "";
            txtPerTIN.Text = "";
            txtSGSTin.Text = "";
            TxtSGSTRegDate.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Modify the Address Details";
            cvCustomerMaster.IsValid = false;

        }

    }


    protected void btnModify_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtBankAddress.Text.Length > 300)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('Bank Address Length should be less than or equal to 300');", true);
                tcCustomerMaster.ActiveTabIndex = 2;
                return;
            }
            if (txtIFSC_Code.Text.Length != 11)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('IFSC Code Length should be 11 Characters');", true);
                tcCustomerMaster.ActiveTabIndex = 2;
                txtIFSC_Code.Focus();
                return;
            }
            DataRow drDetails;
            DataTable dtBankDetails = (DataTable)ViewState["DetailsTable"];

            DataView dvBankdetails = dtBankDetails.DefaultView;
            dvBankdetails.Sort = "Account_Number";
            int rowindex = Convert.ToInt16(hdnBankId.Value);


            DataRow[] drDefault = dtBankDetails.Select("Is_Default_Account = 'True' and (CUSTOMER_BANK_ID<>'" + dvBankdetails[rowindex].Row["CUSTOMER_BANK_ID"].ToString() + "')");
            if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            {
                Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
                return;
            }

            dvBankdetails[rowindex].Row["Name"] = ddlAccountType.SelectedItem.Text;
            dvBankdetails[rowindex].Row["ID"] = ddlAccountType.SelectedItem.Value;
            dvBankdetails[rowindex].Row["Account_Number"] = txtAccountNumber.Text.Trim();
            dvBankdetails[rowindex].Row["Bank_Name"] = txtBankName.Text.Trim();
            dvBankdetails[rowindex].Row["Branch_Name"] = txtBankBranch.Text.Trim();
            dvBankdetails[rowindex].Row["IFSC_Code"] = txtIFSC_Code.Text.Trim();
            if (ddlRSNum.SelectedValue == "" || ddlRSNum.SelectedValue == "0")
            {
                dvBankdetails[rowindex].Row["PANUM"] = "0";
            }
            else
            {
                dvBankdetails[rowindex].Row["PANUM"] = ddlRSNum.SelectedValue;
            }
            if (ddlSANum.SelectedValue == "" || ddlSANum.SelectedValue == "0")
            {
                dvBankdetails[rowindex].Row["Tranche_Header_ID"] = 0;
                dvBankdetails[rowindex].Row["Tranche_Name"] = "";
            }
            else
            {
                dvBankdetails[rowindex].Row["Tranche_Header_ID"] = ddlSANum.SelectedValue;
                dvBankdetails[rowindex].Row["Tranche_Name"] = ddlSANum.SelectedItem.ToString();
            }
            dvBankdetails[rowindex].Row["MICR_Code"] = txtMICRCode.Text.Trim();

            dvBankdetails[rowindex].Row["Bank_Address"] = txtBankAddress.Text.Trim();
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            if (chkDefaultAccount.Checked == true)
            {
                dvBankdetails[rowindex].Row["Is_Default_Account"] = 1;
            }
            else
            {
                dvBankdetails[rowindex].Row["Is_Default_Account"] = 0;
            }
           
            grvBankDetails.DataSource = dvBankdetails;
            grvBankDetails.DataBind();
            FunPriHideBankColumns();
            ViewState["DetailsTable"] = dvBankdetails.Table;
            ClearBankDetailsControls();
            btnAdd.Enabled = true;
            btnModify.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Modify the Bank Details";
            cvCustomerMaster.IsValid = false;

        }
    }

    protected void ClearBankDetailsControls()
    {
        try
        {
            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = "";
            txtBankName.Text = "";
            txtBankBranch.Text = "";
            txtMICRCode.Text = "";
            txtBankAddress.Text = "";
            txtIFSC_Code.Text = "";
            if(ddlRSNum.Items.Count>0)ddlRSNum.SelectedIndex = 0;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            chkDefaultAccount.Checked = false;

            txtIFSC_Code.Text = string.Empty;
          
            ddlRSNum.SelectedIndex = -1;
            ddlSANum.SelectedIndex = -1;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

   protected void btnBnkClear_Click(object sender, EventArgs e)
    {
        try
        {

            ddlAccountType.SelectedIndex = 0;
            txtAccountNumber.Text = "";
            txtBankName.Text = "";
            txtBankBranch.Text = "";
            txtMICRCode.Text = "";
            txtIFSC_Code.Text = "";
            ddlRSNum.SelectedIndex = 0;
            txtBankAddress.Text = "";
            hdnBankId.Value = "";
            btnAdd.Enabled = true;
            btnModify.Enabled = false;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            chkDefaultAccount.Checked = false;

            txtIFSC_Code.Text = string.Empty;
            
            ddlRSNum.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Clear the Data in Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void gvBAddress_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (strMode != "Q")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblPIN = (Label)e.Row.FindControl("lblPIN");
                    if (lblPIN.Text.Trim() == "0")
                    {
                        lblPIN.Text = "";
                    }

                    /* For Deleting purpose, Restrcit to add attribute to the Cell Remove linkbutton*/
                    for (int intCellIndex = 0; intCellIndex < e.Row.Cells.Count - 2; intCellIndex++)
                    {
                        e.Row.Cells[intCellIndex].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                        (this.gvBAddress, "Select$" + e.Row.RowIndex);
                    }
                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";

                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the Bank Details");
        }
    }



    protected void grvBankDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDefAccount = (Label)e.Row.FindControl("lblDefAccount");
                CheckBox chkgvDefaultAccount = (CheckBox)e.Row.FindControl("chkgvDefaultAccount");
                if (lblDefAccount.Text == "1" || lblDefAccount.Text == "True")
                {
                    chkgvDefaultAccount.Checked = true;
                }
                else
                {
                    chkgvDefaultAccount.Checked = false;
                }
                Label lblRS = (Label)e.Row.FindControl("lblRS");
                Label lblTranche = (Label)e.Row.FindControl("lblTranche");
                if (lblRS.Text == "0")
                {
                    lblRS.Text = "";
                }
                if (lblTranche.Text == "0")
                {
                    lblTranche.Text = "";
                }

            }
          if (strMode != "Q")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    /* For Deleting purpose, Restrcit to add attribute to the Cell Remove linkbutton*/
                    for (int intCellIndex = 0; intCellIndex < e.Row.Cells.Count - 2; intCellIndex++)
                    {
                        e.Row.Cells[intCellIndex].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                        (this.grvBankDetails, "Select$" + e.Row.RowIndex);
                    }
                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
                }
            }
            if (strMode == "Q" && e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox lblBankAddress = e.Row.FindControl("lblgvBankAddress") as TextBox;
                lblBankAddress.Enabled = true;
                lblBankAddress.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the Bank Details");
        }
    }



    protected void gvBAddress_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete = (DataTable)ViewState["BillingAddress"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            gvBAddress.DataSource = dtDelete;
            gvBAddress.DataBind();
            ViewState["BillingAddress"] = dtDelete;
            if (gvBAddress.Rows.Count == 0)
            {
                btnAdd.Enabled = true;
                btnModifyAddress.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Remove Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }


    protected void grvBankDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete = (DataTable)ViewState["DetailsTable"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            grvBankDetails.DataSource = dtDelete;
            grvBankDetails.DataBind();
            FunPriHideBankColumns();
            ViewState["DetailsTable"] = dtDelete;
            ClearBankDetailsControls();
            if (grvBankDetails.Rows.Count == 0)
            {

                btnAdd.Enabled = true;
                btnModify.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Remove Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    //protected void gvBAddress_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    { 

    //        Label lblState = gvBAddress.SelectedRow.FindControl("lblState") as Label;
    //        UserControls_S3GAutoSuggest txtPerCity = gvBAddress.SelectedRow.FindControl("txtPerCity") as UserControls_S3GAutoSuggest ;
    //        Label lblAddress1 = gvBAddress.SelectedRow.FindControl("lblAddress1") as Label;
    //        Label lblAddress2 = gvBAddress.SelectedRow.FindControl("lblAddress2") as Label;
    //        Label lblPIN = gvBAddress.SelectedRow.FindControl("lblPIN") as Label;
    //        Label lblEmailID = gvBAddress.SelectedRow.FindControl("lblEmailID") as Label;
    //        Label lblContactName = gvBAddress.SelectedRow.FindControl("lblContactName") as Label;
    //        Label lblContactNo = gvBAddress.SelectedRow.FindControl("lblContactNo") as Label;
    //        Label lblgTIN = gvBAddress.SelectedRow.FindControl("lblgTIN") as Label;
    //        Label lblgTAN = gvBAddress.SelectedRow.FindControl("lblgTAN") as Label;
    //        /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/

    //        txtPerState.SelectedValue = lblState.Text;
    //        if (gvBAddress.SelectedRow.Cells[5].Text != "&nbsp;")
    //            txtPerCity.SelectedText = Convert.ToString(gvBAddress.SelectedRow.Cells[5].Text);
    //        if (gvBAddress.SelectedRow.Cells[9].Text != "&nbsp;")
    //            lblAddress1.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[9].Text);

    //        if (gvBAddress.SelectedRow.Cells[9].Text != "&nbsp;")
    //            lblAddress2.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[9].Text);
    //        if (gvBAddress.SelectedRow.Cells[9].Text != "&nbsp;")
    //            lblPIN.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[9].Text);
    //        if (gvBAddress.SelectedRow.Cells[10].Text != "&nbsp;")
    //            lblEmailID.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[10].Text);
    //        if (gvBAddress.SelectedRow.Cells[11].Text != "&nbsp;")
    //            lblContactName.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[11].Text);
    //        if (gvBAddress.SelectedRow.Cells[11].Text != "&nbsp;")
    //            lblContactNo.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[11].Text);
    //        if (gvBAddress.SelectedRow.Cells[11].Text != "&nbsp;")
    //            lblgTIN.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[11].Text);

    //        //txtBankAddress.Text = lblBankAddress.Text;
    //        //hdnBankId.Value = lblBankMappingId.Text;
    //        /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
    //        //chkDefaultAccount.Checked = chkGridDefaultAccount.Checked;
    //        if (gvBAddress.SelectedRow.Cells[6].Text != "&nbsp;")
    //            lblgTAN.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[6].Text);//IFSC Code

    //        btnAdd.Enabled = false;
    //        btnModifyAddress.Enabled = true;

    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to view the Bank Details";
    //        cvCustomerMaster.IsValid = false;
    //    }
    //}


    protected void grvBankDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Label lblBankMappingId = grvBankDetails.SelectedRow.FindControl("lblBankMappingId") as Label;
            Label lblMasterId = grvBankDetails.SelectedRow.FindControl("lblMasterId") as Label;
            Label lblAccountType = grvBankDetails.SelectedRow.FindControl("lblAccountType") as Label;
            Label lblAccountTypeId = grvBankDetails.SelectedRow.FindControl("lblAccountTypeId") as Label;
            Label lblTrancheID = grvBankDetails.SelectedRow.FindControl("lblTrancheID") as Label;
            TextBox lblgvBankAddress = grvBankDetails.SelectedRow.FindControl("lblgvBankAddress") as TextBox;

            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            CheckBox chkGridDefaultAccount = grvBankDetails.SelectedRow.FindControl("chkgvDefaultAccount") as CheckBox;

            ddlAccountType.SelectedValue = lblAccountTypeId.Text;
            if (grvBankDetails.SelectedRow.Cells[4].Text != "&nbsp;")
                txtAccountNumber.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[4].Text);
            if (grvBankDetails.SelectedRow.Cells[6].Text != "&nbsp;")
                txtMICRCode.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[6].Text);
            if (grvBankDetails.SelectedRow.Cells[7].Text != "&nbsp;")
                txtBankName.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[7].Text);
            if (grvBankDetails.SelectedRow.Cells[8].Text != "&nbsp;")
                txtBankBranch.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[8].Text);
            txtBankAddress.Text = lblgvBankAddress.Text;
            hdnBankId.Value = Convert.ToString(grvBankDetails.SelectedRow.RowIndex);
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            chkDefaultAccount.Checked = chkGridDefaultAccount.Checked;
            if (grvBankDetails.SelectedRow.Cells[5].Text != "&nbsp;")
                txtIFSC_Code.Text = Convert.ToString(grvBankDetails.SelectedRow.Cells[5].Text);//IFSC Code
            ddlRSNum.SelectedIndex = -1;
            if (!string.IsNullOrEmpty(grvBankDetails.SelectedRow.Cells[10].Text))
            {
                if (grvBankDetails.SelectedRow.Cells[10].Text != "&nbsp;")
                    ddlRSNum.SelectedValue = Convert.ToString(grvBankDetails.SelectedRow.Cells[10].Text);
            }
            if (!string.IsNullOrEmpty(grvBankDetails.SelectedRow.Cells[11].Text))
            {
                if (grvBankDetails.SelectedRow.Cells[11].Text != "&nbsp;")
                    ddlSANum.SelectedValue = lblTrancheID.Text;
            }
         
            //if (!string.IsNullOrEmpty(grvBankDetails.SelectedRow.Cells[8].Text))
            //{
            //    if (grvBankDetails.SelectedRow.Cells[8].Text != "&nbsp;")
            //    {
            //        ddlPANum_SelectedIndexChanged(null, null);
                   
            //    }
            //}
            btnAdd.Enabled = false;
            btnModify.Enabled = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to view the Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion


    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }

    protected void gvCredit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (gvCredit.DataSource != null && ((DataTable)gvCredit.DataSource).Rows.Count > 0)
        {
            if (e.Row.Cells[1] != null && e.Row.Cells[1].Text != string.Empty)
            {
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            }
            if (e.Row.Cells[2] != null && e.Row.Cells[2].Text != string.Empty)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            }
            if (e.Row.Cells[3] != null && e.Row.Cells[3].Text != string.Empty)
            {
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            }
            if (e.Row.Cells[4] != null && e.Row.Cells[4].Text != string.Empty)
            {
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            }


        }

    }


    private void FunPriLoadEnquiryDetails(int intEnquiryNumber)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@EnquiryResponse_ID", intEnquiryNumber.ToString());
        DataTable dt = Utility.GetDefaultData("S3G_ORG_GETENQUIRYDETAILSFORPRICING", Procparam);
        txtCustomerName.Text = dt.Rows[0]["Customer_Name"].ToString();
        txtComAddress1.Text = dt.Rows[0]["Address"].ToString();
        txtCOmAddress2.Text = dt.Rows[0]["Address2"].ToString();
        txtPAN.Text = dt.Rows[0]["PAN"].ToString();
        txtCIN.Text = dt.Rows[0]["CIN"].ToString();
        //txtPAN.Text = dt.Rows[0]["PAN"].ToString();
        //txtPAN.Text = dt.Rows[0]["PAN"].ToString();
        //txtComCity.SelectedItem.Text = dt.Rows[0]["City"].ToString();
        //string strCityValue = txtComCity.Items.FindByText(dt.Rows[0]["City"].ToString()).Value;
        //txtComCity.SelectedValue = strCityValue;
        //ddlComState.SelectedValue = txtComState.Items.FindByText(dt.Rows[0]["State"].ToString()).Value;
        ddlComState.SelectedValue = ddlComState.Items.FindByText(dt.Rows[0]["State"].ToString()).Value;
        //txtComCountry.SelectedItem.Text = dt.Rows[0]["Country"].ToString();
        txtComCountry.SelectedValue = txtComCountry.Items.FindByText(dt.Rows[0]["Country"].ToString()).Value;
        txtComPincode.Text = dt.Rows[0]["PINCode_ZipCode"].ToString();
        if (txtComPincode.Text.Trim() == "0")
        {
            txtComPincode.Text = "";
        }
        txtComMobile.Text = dt.Rows[0]["Mobile"].ToString();
        txtComEmail.Text = dt.Rows[0]["EMail"].ToString();
        ddlConstitutionName.SelectedValue = dt.Rows[0]["Constitution_ID"].ToString();
        chkCustomer.Checked = true;
        ddlConstitutionName_OnSelectedIndexChanged(this, new EventArgs());
    }


    #region ""

    protected void btnBrowse_OnClick(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvConstitutionalDocuments", ((Button)sender).ClientID);

        HttpFileCollection hfc = Request.Files;
        for (int i = 0; i < hfc.Count; i++)
        {
            HttpPostedFile hpf = hfc[i];
            if (hpf.ContentLength > 0)
            {
                CheckBox chkScanned = (CheckBox)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("chkScanned");
                HiddenField hdnSelectedPath = (HiddenField)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("hdnSelectedPath");
                FileUpload flUpload = (FileUpload)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("flUpload");
                //Label lblActualPath = (Label)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("lblActualPath");
                TextBox txtFileupld = (TextBox)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("txtFileupld");

                //chkSelect.Checked = true;
                //chkScanned.ToolTip = flUpload.ToolTip = hdnSelectedPath.Value;
                //lblActualPath.Text = hpf.FileName;
                txtFileupld.Text = hpf.FileName;
                string strViewst = "File" + intRowIndex.ToString();
                Cache[strViewst] = hpf;
            }
        }
    }

    protected void chkScanned_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvConstitutionalDocuments", ((CheckBox)sender).ClientID);
        CheckBox chkScanned = (CheckBox)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("chkScanned");
        Label lblActualPath = (Label)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("lblActualPath");
        TextBox txtFileupld = (TextBox)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("txtFileupld");

        if (!chkScanned.Checked)
        {
            lblActualPath.Text =
               txtFileupld.Text = string.Empty;
        }
    }

    protected void FunProUploadFilesNew()
    {
        try
        {
            for (int i = 0; i <= gvConstitutionalDocuments.Rows.Count - 1; i++)
            {
                string strViewst = "File" + i.ToString();
                CheckBox chkScanned = (CheckBox)gvConstitutionalDocuments.Rows[i].FindControl("chkScanned");
                Label lblCurrentPath = (Label)gvConstitutionalDocuments.Rows[i].FindControl("lblActualPath");

                if (chkScanned.Checked)
                {
                    HttpPostedFile hpf = (HttpPostedFile)Cache[strViewst];
                    //string strServerPath = Server.MapPath(".").Replace("\\LoanAdmin", "");
                    //string strFilePath = "\\Data\\Invoice\\" + intCompanyId.ToString() + "_" + ddlMLA.SelectedText.Replace("/", "");

                    //if (!System.IO.Directory.Exists(strServerPath + strFilePath))
                    //{
                    //    System.IO.Directory.CreateDirectory(strServerPath + strFilePath);
                    //}

                    //if (ddlSLA.SelectedValue.ToString() != "0")
                    //{
                    //    strFilePath = strFilePath + @"\" + ddlSLA.SelectedItem.ToString().Replace("/", "") + "_" + (i + 1).ToString() + "_" + System.IO.Path.GetFileName(hpf.FileName).Replace("%20", "_").Replace(" ", "_");
                    //}
                    //else
                    //{
                    //    strFilePath = strFilePath + @"\" + (i + 1).ToString() + "_" + System.IO.Path.GetFileName(hpf.FileName).Replace("%20", "_").Replace(" ", "_");
                    //}

                    string strNewFileName = @"\COMPANY" + intCompanyId.ToString();
                    string strPath = "";
                    //if (txOD.Text != "")
                    //{
                    strPath = strNewFileName;
                    strPath = Convert.ToString(ViewState["ConsDocPath"]) + strNewFileName;
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }

                    if (hpf != null)
                    {
                        strPath += @"\" + System.IO.Path.GetFileName(hpf.FileName).Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + System.IO.Path.GetFileName(hpf.FileName).Split('.')[1].ToString();

                        lblCurrentPath.Text = strPath;
                        hpf.SaveAs(strPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void FunPriUploadFiles()
    {
        try
        {


            foreach (GridViewRow grv in gvConstitutionalDocuments.Rows)
            {
                AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)grv.FindControl("asyFileUpload");
                TextBox txOD = grv.FindControl("txOD") as TextBox;

                if (AsyncFileUpload1.FileName != "")
                {
                    FileInfo fileInfo = new FileInfo(AsyncFileUpload1.FileName);
                    if (AsyncFileUpload1.HasFile)
                    {

                        string strNewFileName = @"\COMPANY" + intCompanyId.ToString();
                        string strPath = "";
                        //if (txOD.Text != "")
                        //{
                        strPath = strNewFileName;
                        strPath = Convert.ToString(ViewState["ConsDocPath"]) + strNewFileName;
                        if (!Directory.Exists(strPath))
                        {
                            Directory.CreateDirectory(strPath);
                        }

                        strPath += @"\" + AsyncFileUpload1.FileName.Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + AsyncFileUpload1.FileName.Split('.')[1].ToString();
                        //}
                        txOD.Text = strPath;

                        AsyncFileUpload1.SaveAs(strPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("PRDD Master Document Path does not exist");
        }
    }

    protected void FunProClearCaches()
    {
        for (int i = 0; i <= gvConstitutionalDocuments.Rows.Count - 1; i++)
        {
            string strViewst = "File" + i.ToString();
            if (Cache[strViewst] != null)
            {
                Cache.Remove(strViewst);
            }
        }
    }
    #endregion




    protected void grvFacilityGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblHGroupId = (Label)e.Row.FindControl("lblHGroupId");
                GridView grvFacility = (GridView)e.Row.FindControl("grvFacility");
                //FunProSetDataSource(grvFacility, lblHGroupId.Text);
                Label lblDeactive = (Label)e.Row.FindControl("lblDeactive");
                CheckBox chkDeactivate = (CheckBox)e.Row.FindControl("chkDeactivate");
                if (lblDeactive.Text == "1" || lblDeactive.Text == "True")
                {
                    chkDeactivate.Checked = true;
                }
                else
                {
                    chkDeactivate.Checked = false;
                }

                e.Row.Cells[0].Style.Add("padding", "0px");
                //grvFacilityGroup.GridLines = GridLines.None;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProSetDataSource(GridView grv, string strGroup)
    {
        try
        {
            if (strGroup == "")
            {
                strGroup = "0";
            }

            DataTable dtFacilityGrid = (DataTable)ViewState["dtFacilityGrid"];
            DataRow[] dRows = dtFacilityGrid.Select("Group_ID = '" + strGroup + "'");
            decimal dcTotal = 0, dcSanctionAmount = 0, dcApprovedAmount = 0, scUtilizedAmount = 0, dcBalanceAmount = 0;
            if (dRows.Length > 0)
            {
                DataTable dtFilered = dRows.CopyToDataTable();

                grv.DataSource = dtFilered;
                grv.DataBind();

                //To update sub-total
                dcTotal = (decimal)dtFacilityGrid.Compute("SUM(Facility_Amount)", "Group_ID = '" + strGroup + "'");
                dcSanctionAmount = (decimal)dtFacilityGrid.Compute("SUM(Sanctioned_limit)", "Group_ID = '" + strGroup + "'");
                dcApprovedAmount = (decimal)dtFacilityGrid.Compute("SUM(Final_Sanctioned_Limit)", "Group_ID = '" + strGroup + "'");
                scUtilizedAmount = (decimal)dtFacilityGrid.Compute("SUM(Utilized_Amount)", "Group_ID = '" + strGroup + "'");
                dcBalanceAmount = (decimal)dtFacilityGrid.Compute("SUM(Remaining_Amount)", "Group_ID = '" + strGroup + "'");
            }

            DataTable dtFacilityGroup = (DataTable)ViewState["dtFacilityGroup"];

            foreach (DataRow drow in dtFacilityGroup.Rows)
            {
                if (drow["Group_ID"].ToString() == strGroup && strGroup == "0")
                {
                    drow["Total_Amount"] = dcTotal.ToString();
                    drow["Sanctioned_Group_Limit"] = dcSanctionAmount.ToString();
                    drow["Final_Groupt_Sanctioned"] = dcApprovedAmount.ToString();
                    drow["Utilized_Amount"] = scUtilizedAmount.ToString();
                    drow["Remaining_Amount"] = dcBalanceAmount.ToString();
                }
            }

            if (strGroup == "0")
            {
                Label lblHFacilityAmount = (Label)grv.Parent.FindControl("lblHFacilityAmount");
                Label lblHSanctionedAmount = (Label)grv.Parent.FindControl("lblHSanctionedAmount");
                Label lblHApprovedAmount = (Label)grv.Parent.FindControl("lblHApprovedAmount");
                Label lblHUtilizedAmount = (Label)grv.Parent.FindControl("lblHUtilizedAmount");
                Label lblHRemainingAmount = (Label)grv.Parent.FindControl("lblHRemainingAmount");

                lblHFacilityAmount.Text = dcTotal.ToString();
                lblHSanctionedAmount.Text = dcSanctionAmount.ToString();
                lblHApprovedAmount.Text = dcApprovedAmount.ToString();
                lblHUtilizedAmount.Text = scUtilizedAmount.ToString();
                lblHRemainingAmount.Text = dcBalanceAmount.ToString();
            }

            if (dtFacilityGroup.Rows.Count > 0)
            {
                dcTotal = (decimal)dtFacilityGroup.Compute("SUM(Total_Amount)", "1 = 1");
                //lblTotalAmount.Text = dcTotal.ToString();
            }

            ViewState["dtFacilityGroup"] = dtFacilityGroup;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void gvBAddress_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Label lblState = gvBAddress.SelectedRow.FindControl("lblState") as Label;
            Label lblStateCode = gvBAddress.SelectedRow.FindControl("lblStateCode") as Label;
            Label lblCity = gvBAddress.SelectedRow.FindControl("lblCity") as Label;
            Label lblAddress1 = gvBAddress.SelectedRow.FindControl("lblAddress1") as Label;
            Label lblAddress2 = gvBAddress.SelectedRow.FindControl("lblAddress2") as Label;
            Label lblPIN = gvBAddress.SelectedRow.FindControl("lblPIN") as Label;
            Label lblEmailID = gvBAddress.SelectedRow.FindControl("lblEmailID") as Label;
            Label lblContactName = gvBAddress.SelectedRow.FindControl("lblContactName") as Label;
            Label lblContactNo = gvBAddress.SelectedRow.FindControl("lblContactNo") as Label;
            Label lblgTIN = gvBAddress.SelectedRow.FindControl("lblgTIN") as Label;
            Label lblgTAN = gvBAddress.SelectedRow.FindControl("lblgTAN") as Label;
            Label lblRowNo = gvBAddress.SelectedRow.FindControl("lblRowNo") as Label;
            Label lblCustID = gvBAddress.SelectedRow.FindControl("lblCustID") as Label;
            Label lblSGSTIN = gvBAddress.SelectedRow.FindControl("lblSGSTIN") as Label;
            Label lblSGSTRegDate1 = gvBAddress.SelectedRow.FindControl("lblSGSTRegDate") as Label;
            LinkButton lnkDelete = gvBAddress.SelectedRow.FindControl("lnkDelete") as LinkButton;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            for (int i = 0; i <= gvBAddress.Rows.Count - 1; i++)
            {
                if (i == gvBAddress.SelectedRow.RowIndex)
                {
                    lnkDelete.Enabled = false;

                }
                else
                {
                    LinkButton lnkDelete1 = gvBAddress.Rows[i].FindControl("lnkDelete") as LinkButton;
                    lnkDelete1.Enabled = true;
                }
            }

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyId > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetStateLookup", Procparam);

            Utility.FillDataTable(ddlComState, dtAddr, "Location_Category_ID", "LocationCat_Description");
            Utility.FillDataTable(ddlPerState, dtAddr, "Location_Category_ID", "LocationCat_Description");


            OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "4");
            Procparam.Add("@Param1", lblState.Text);
            DataTable dt = Utility.GetDefaultData("S3G_ORG_CheckTINTAN", Procparam);
            Procparam.Clear();

            if (dt.Rows.Count > 0)
            {
                ddlPerState.SelectedValue = dt.Rows[0]["Location_Category_ID"].ToString();
            }
            if (ViewState["CorporateStateValue"] != null)
            {
                ddlComState.SelectedValue = ViewState["CorporateStateValue"].ToString();
            }

            txtPerCity.SelectedText = lblCity.Text.Trim();
            txtPerAddress1.Text = lblAddress1.Text;
            txtPerAddress2.Text = lblAddress2.Text;
            txtPerPincode.Text = lblPIN.Text;
            txtPerEmail.Text = lblEmailID.Text;
            txtPerContName.Text = lblContactName.Text;
            txtPerContactNo.Text = lblContactNo.Text;
            txtPerTIN.Text = lblgTIN.Text;
            txtPerTAN.Text = lblgTAN.Text;
            hdnAddID.Value = Convert.ToString(gvBAddress.SelectedRow.RowIndex);
            txtSGSTin.Text = lblSGSTIN.Text;
            TxtSGSTRegDate.Text = lblSGSTRegDate1.Text;
            //txtBankAddress.Text = lblBankAddress.Text;
            //hdnBankId.Value = lblBankMappingId.Text;
            /*Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)*/
            ////chkDefaultAccount.Checked = chkGridDefaultAccount.Checked;
            //if (gvBAddress.SelectedRow.Cells[6].Text != "&nbsp;")
            //    lblgTAN.Text = Convert.ToString(gvBAddress.SelectedRow.Cells[6].Text);//IFSC Code

            if (strMode != "Q")
            {
                btnAddAddress.Enabled = false;
                btnModifyAddress.Enabled = true;
            }
            else
            {
                btnAddAddress.Enabled = false;
                btnModifyAddress.Enabled = false;
            }



        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to view the Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    private void FunPriLoadCRMCustDtl()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@SearchValue", Convert.ToString(intCRMID));
            Procparam.Add("@UserId", Convert.ToString(intUserId));
            Procparam.Add("@SearchType", "Account");
            DataSet dsCust = Utility.GetDataset("S3G_ORG_GETCRMPROSPECTINFO", Procparam);
            if (dsCust != null)
            {
                ddlTitle.SelectedValue = Convert.ToString(dsCust.Tables[2].Rows[0]["Prospect_Title"]);
                txtCustomerName.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Prospect_Name"]);
                if (ddlPostingGroup.Items.FindByValue(Convert.ToString(dsCust.Tables[2].Rows[0]["Posting_GL_Code"])) == null)
                {
                    //if (ddlPostingGroup.Items.FindByText(Convert.ToString(ObjCustomerDetails.Rows[0]["Posting_Code"])) != null)
                    //{
                    //    ddlPostingGroup.Items.Remove(ddlPostingGroup.Items.FindByText(Convert.ToString(ObjCustomerDetails.Rows[0]["Posting_Code"])));
                    //}
                    //ddlPostingGroup.Items.Add(ListPosting);
                    ddlPostingGroup.SelectedIndex = 0;
                }
                else
                {
                    ddlPostingGroup.SelectedValue = Convert.ToString(dsCust.Tables[2].Rows[0]["Posting_GL_Code"]);
                }
                ddlConstitutionName.SelectedValue = Convert.ToString(dsCust.Tables[2].Rows[0]["Constitution_ID"]);
                ddlCompanyType.SelectedValue = Convert.ToString(dsCust.Tables[2].Rows[0]["Company_Type"]);
                txtComAddress1.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Address1"]);
                txtCOmAddress2.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Address2"]);
                txtComCity.SelectedText = Convert.ToString(dsCust.Tables[2].Rows[0]["City"]);
                ddlComState.SelectedValue = Convert.ToString(dsCust.Tables[2].Rows[0]["State"]);
                txtComCountry.SelectedItem.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Country"]);
                txtComPincode.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Pincode"]);
                if (txtComPincode.Text.Trim() == "0")
                {
                    txtComPincode.Text = "";
                }
                txtComTelephone.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Telephone"]);
                txtComMobile.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Mobile"]);
                txtComEmail.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Email"]);
                txtComWebsite.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Website"]);
                cmbIndustryCode.Text = Convert.ToString(dsCust.Tables[2].Rows[0]["Industry_Code"]);
                cmbIndustryCode_OnTextChanged(null, null);
                ddlConstitutionName_OnSelectedIndexChanged(null, null);
            }
        }
        catch (Exception ObjException)
        {
            throw ObjException;
        }
    }


    protected void chkDeactivate_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gvRow = (GridViewRow)chk.Parent.Parent;
        Label lblPricingID = (Label)grvFacilityGroup.Rows[gvRow.RowIndex].FindControl("lblPricingID");
        Label lblAssCategory = (Label)grvFacilityGroup.Rows[gvRow.RowIndex].FindControl("lblAssCategory");
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Customer_Id", Convert.ToString(intCustomerId));
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@Pricing_ID", lblPricingID.Text.ToString());
        Procparam.Add("@AssetCategory", lblAssCategory.Text.ToString());
        if (chk.Checked == true)
        {
            Procparam.Add("@Param1", "1");
        }
        else
        {
            Procparam.Add("@Param1", "0");
        }
        DataSet dsCust = Utility.GetDataset("S3G_ORG_CUSTCREDITTEMP", Procparam);
    }

    protected void btnReferenceNumberGo_Click(object sender, EventArgs e)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Customer_ID", intCustomerId.ToString());
        if (!String.IsNullOrEmpty(txtReferenceDate.Text))
            Procparam.Add("@ReferenceDate", txtReferenceDate.Text);
        Procparam.Add("@ValidFrom", Utility.StringToDate(txtValidfrom.Text).ToString());
        Procparam.Add("@ValidTo", Utility.StringToDate(txtValidTo.Text).ToString());
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@CashFlowType", ddlCashflow.SelectedValue);

        DataTable dtCust = Utility.GetDefaultData("S3G_ORG_CUST_PO_DO_MappingAccounts", Procparam);

        if (dtCust.Columns.Contains("ErrCode"))
        {
            if(dtCust.Rows[0]["ErrCode"].ToString()=="1")
                Utility.FunShowAlertMsg(this, "Valid From Date should be lesser or equal to Valid To Date");
            if (dtCust.Rows[0]["ErrCode"].ToString() == "2")
                Utility.FunShowAlertMsg(this, "Valid From Date should be greater or equal to Reference Date");
            dtCust.Clear();
        }

        if (dtCust.Rows.Count > 0)
        {
            pnlAccounts.Visible = true;
            grvAccounts.DataSource = dtCust;
            grvAccounts.DataBind();
            btnAddAccounts.Visible = true;
        }
        else
        {
            pnlAccounts.Visible = true;
            grvAccounts.EmptyDataText = "No records Found";
            grvAccounts.DataBind();
            btnAddAccounts.Visible = false;
        }
    }

    protected void grvAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectAccount");
                chkSelectAccount.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvAccounts.ClientID + "','chkAll','chkSelectAccount');");

            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvAccounts.ClientID + "',this,'chkSelectAccount');");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnAddAccounts_Click(object sender, EventArgs e)
    {
        if (btnAddAccounts.Text == "Add")
        {
            DataTable dtAccSt = (DataTable)ViewState["PODOMappings"];
            DataRow dr = dtAccSt.NewRow();
            dr["InvoiceRef_ID"] = 0;
            dr["RefType"] = ddlReferenceType.SelectedItem.Text;
            dr["RefTypeId"] = Convert.ToInt32(ddlReferenceType.SelectedValue);
            dr["RefNumber"] = txtReferenceNumber.Text;
            dr["CashflowType"] = ddlCashflow.SelectedItem.Text;
            dr["CashflowTypeId"] = Convert.ToInt32(ddlCashflow.SelectedValue);
            if (!String.IsNullOrEmpty(txtReferenceDate.Text))
                dr["RefDate"] = Convert.ToDateTime(txtReferenceDate.Text).ToString(strDateFormat);
            dr["TransDate"] = DateTime.Now.ToString(strDateFormat);
            dr["ValidFrom"] = Utility.StringToDate(txtValidfrom.Text).ToString(strDateFormat);
            dr["ValidTo"] = Utility.StringToDate(txtValidTo.Text).ToString(strDateFormat);
            dtAccSt.Rows.Add(dr);
            dtAccSt.AcceptChanges();

            string SlNo = dr.ItemArray[0].ToString();

            DataTable dtAccStDetails = (DataTable)ViewState["PODOMappingsDetails"];
            if (!dtAccStDetails.Columns.Contains("SlNo"))
            {
                dtAccStDetails.Columns.Add("SlNo");
            }

            int nCnt = 0;

            foreach (GridViewRow gvrow in grvAccounts.Rows)
            {
                CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount");
                Label lblTranche_Name = (Label)gvrow.Cells[0].FindControl("lblTranche_Name");
                Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");

                if (chkSelectAccount.Checked)
                {
                    DataRow drdetails = dtAccStDetails.NewRow();
                    drdetails["SlNo"] = SlNo;
                    drdetails["InvoiceRef_ID"] = 0;
                    drdetails["PA_SA_REF_ID"] = grvAccounts.DataKeys[gvrow.RowIndex].Value.ToString();
                    drdetails["Tranche"] = lblTranche_Name.Text;
                    drdetails["PANum"] = lblRS_Number.Text;
                    dtAccStDetails.Rows.Add(drdetails);
                    dtAccStDetails.AcceptChanges();

                    nCnt += 1;
                }
            }
            
            if (nCnt == 0)
            {
                dtAccSt.Rows.Remove(dr);
                Utility.FunShowAlertMsg(this, "Select atleast one account to add");
                return;
            }

            if (dtAccSt.Rows.Count > 0)
            {
                grvHistory.DataSource = dtAccSt;
                grvHistory.DataBind();
            }
            else
            {
                grvHistory.EmptyDataText = "No records Found";
                grvHistory.DataBind();
            }

            ViewState["PODOMappings"] = dtAccSt;
            ViewState["PODOMappingsDetails"] = dtAccStDetails;

            ddlReferenceType.SelectedIndex = ddlCashflow.SelectedIndex = 0;
            txtReferenceDate.Text = "";
            txtReferenceNumber.Text = "";
            txtValidfrom.Text = "";
            txtValidTo.Text = "";
            ddlReferenceType.Enabled = ddlCashflow.Enabled = true;
            //txtReferenceDate.ReadOnly = txtValidfrom.ReadOnly = txtValidTo.ReadOnly = false;
            pnlAccounts.Visible = false;
            btnAddAccounts.Text = "Add";
            btnAddAccounts.Visible = false;
            btnReferenceNumberGo.Visible = true;
            pnlHistoryinDetail.Visible = false;
            hdnInvoiceRef_ID.Value = "0";
            hdnSlNo.Value = "0";
        }
        else if (btnAddAccounts.Text == "Modify")
        {
            int nCnt = 0;

            if (Utility.StringToDate(txtValidfrom.Text) > Utility.StringToDate(txtValidTo.Text))
            {
                Utility.FunShowAlertMsg(this, "Valid From Date should be lesser or equal to Valid To Date");
                return;
            }

            if (!String.IsNullOrEmpty(txtReferenceDate.Text) && Utility.StringToDate(txtReferenceDate.Text) > Utility.StringToDate(txtValidfrom.Text))
            {
                Utility.FunShowAlertMsg(this, "Valid From Date should be greater or equal to Reference Date");
                return;
            }

            foreach (GridViewRow gvrow in grvAccounts.Rows)
            {
                CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount");
                if (chkSelectAccount.Checked)
                {
                    nCnt += 1;
                }
            }
            if (nCnt == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast one account to add");
                return;
            }
            DataTable dtAccSt = (DataTable)ViewState["PODOMappings"];
            dtAccSt.Select(hdnInvoiceRef_ID.Value == "0" ? "SlNo = " + hdnSlNo.Value : "InvoiceRef_ID = " + hdnInvoiceRef_ID.Value)[0]["RefNumber"] = txtReferenceNumber.Text;
            if (!String.IsNullOrEmpty(txtReferenceDate.Text))
                dtAccSt.Select(hdnInvoiceRef_ID.Value == "0" ? "SlNo = " + hdnSlNo.Value : "InvoiceRef_ID = " + hdnInvoiceRef_ID.Value)[0]["RefDate"] = Convert.ToDateTime(txtReferenceDate.Text).ToString(strDateFormat);
            else
                dtAccSt.Select(hdnInvoiceRef_ID.Value == "0" ? "SlNo = " + hdnSlNo.Value : "InvoiceRef_ID = " + hdnInvoiceRef_ID.Value)[0]["RefDate"] = "";
            dtAccSt.Select(hdnInvoiceRef_ID.Value == "0" ? "SlNo = " + hdnSlNo.Value : "InvoiceRef_ID = " + hdnInvoiceRef_ID.Value)[0]["ValidFrom"] = Convert.ToDateTime(txtValidfrom.Text).ToString(strDateFormat);
            dtAccSt.Select(hdnInvoiceRef_ID.Value == "0" ? "SlNo = " + hdnSlNo.Value : "InvoiceRef_ID = " + hdnInvoiceRef_ID.Value)[0]["ValidTo"] = Convert.ToDateTime(txtValidTo.Text).ToString(strDateFormat);
            dtAccSt.AcceptChanges();

            if (dtAccSt.Rows.Count > 0)
            {
                grvHistory.DataSource = dtAccSt;
                grvHistory.DataBind();
            }
            else
            {
                grvHistory.EmptyDataText = "No records Found";
                grvHistory.DataBind();
            }



            DataTable dtAccStDetails = (DataTable)ViewState["PODOMappingsDetails"];
            foreach (GridViewRow gvrow in grvAccounts.Rows)
            {
                CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount");
                Label lblTranche_Name = (Label)gvrow.Cells[0].FindControl("lblTranche_Name");
                Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");

                if (!chkSelectAccount.Checked)
                {

                    dtAccStDetails.Select(hdnInvoiceRef_ID.Value == "0" ? "SlNo = " + hdnSlNo.Value : "InvoiceRef_ID = " + hdnInvoiceRef_ID.Value + " AND PANum = '" + lblRS_Number.Text + "'")[0].Delete();
                    dtAccStDetails.AcceptChanges();
                }
            }

            ViewState["PODOMappings"] = dtAccSt;
            ViewState["PODOMappingsDetails"] = dtAccStDetails;

            ddlReferenceType.SelectedIndex = ddlCashflow.SelectedIndex = 0;
            txtReferenceDate.Text = "";
            txtReferenceNumber.Text = "";
            txtValidfrom.Text = "";
            txtValidTo.Text = "";
            ddlReferenceType.Enabled = ddlCashflow.Enabled = true;
            //txtReferenceDate.ReadOnly = txtValidfrom.ReadOnly = txtValidTo.ReadOnly = false;
            pnlAccounts.Visible = false;
            btnAddAccounts.Text = "Add";
            btnAddAccounts.Visible = false;
            btnReferenceNumberGo.Visible = true;
            pnlHistoryinDetail.Visible = false;
            hdnInvoiceRef_ID.Value = "0";
            hdnSlNo.Value = "0";

        }

    }

    protected void lnkView_Click(object sender, EventArgs e)
    {
        pnlHistoryinDetail.Visible = true;
        LinkButton lnk = (LinkButton)sender;
        GridViewRow myRow = (GridViewRow)lnk.NamingContainer;

        string InvoiceRef_ID = grvHistory.DataKeys[myRow.RowIndex].Value.ToString();
        Label lblSlNo = (Label)myRow.Cells[0].FindControl("lblSlNo");

        DataTable dtAccStDetails = (DataTable)ViewState["PODOMappingsDetails"];
        DataRow[] selecteddr = dtAccStDetails.Select(InvoiceRef_ID == "0" ? "SlNo = " + lblSlNo.Text : "InvoiceRef_ID = " + InvoiceRef_ID);
        DataTable selecteddt = new DataTable();
        selecteddt = selecteddr.CopyToDataTable();

        if (selecteddt.Rows.Count > 0)
        {
            gvHistoryindetail.DataSource = selecteddt;
            gvHistoryindetail.DataBind();
        }
        else
        {
            gvHistoryindetail.EmptyDataText = "No records Found";
            gvHistoryindetail.DataBind();
        }
        ddlReferenceType.SelectedIndex = ddlCashflow.SelectedIndex = 0;
        txtReferenceDate.Text = "";
        txtReferenceNumber.Text = "";
        txtValidfrom.Text = "";
        txtValidTo.Text = "";
        ddlReferenceType.Enabled = ddlCashflow.Enabled = true;
        //txtReferenceDate.ReadOnly = txtValidfrom.ReadOnly = txtValidTo.ReadOnly = false;
        pnlAccounts.Visible = false;
        btnAddAccounts.Text = "Add";
        btnAddAccounts.Visible = false;
        btnReferenceNumberGo.Visible = true;
        hdnInvoiceRef_ID.Value = "0";
        hdnSlNo.Value = "0";
    }

    private void FunPriLoadPODOMappings()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@user_id", intUserId.ToString());
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        DataTable dtCashflow = Utility.GetDefaultData("S3G_ORG_Getcashflow", Procparam);
        ddlCashflow.FillDataTable(dtCashflow, "id", "flag_Desc");

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Customer_ID", intCustomerId.ToString());
        Procparam.Add("@Company_ID", intCompanyId.ToString());

        DataSet dtCust = Utility.GetDataset("S3G_ORG_CUST_PO_DO_MappingDetails", Procparam);

        if (dtCust.Tables[0].Columns.Contains("SlNo"))
        {
            DataColumn RowNum = dtCust.Tables[0].Columns[0];
            //RowNum.DataType = System.Type.GetType("System.Int32");
            RowNum.AutoIncrement = true;
            RowNum.AutoIncrementSeed = 1;
            RowNum.AutoIncrementStep = 1;
            RowNum.Unique = true;
        }

        ViewState["PODOMappings"] = dtCust.Tables[0];
        ViewState["PODOMappingsDetails"] = dtCust.Tables[1];
        if (dtCust.Tables[0].Rows.Count > 0)
        {
            grvHistory.DataSource = dtCust;
            grvHistory.DataBind();
        }
        else
        {
            grvHistory.EmptyDataText = "No records Found";
            grvHistory.DataBind();
        }
    }

    protected void lnkModify_Click(object sender, EventArgs e)
    {
        pnlAccounts.Visible = true;
        LinkButton lnk = (LinkButton)sender;
        GridViewRow myRow = (GridViewRow)lnk.NamingContainer;

        string InvoiceRef_ID = grvHistory.DataKeys[myRow.RowIndex].Value.ToString();
        Label lblSlNo = (Label)myRow.Cells[0].FindControl("lblSlNo");
        hdnInvoiceRef_ID.Value = InvoiceRef_ID;
        hdnSlNo.Value = lblSlNo.Text;

        DataTable dtAccStDetails = (DataTable)ViewState["PODOMappingsDetails"];
        DataRow[] selectedDetailsdr = dtAccStDetails.Select(InvoiceRef_ID == "0" ? "SlNo = " + lblSlNo.Text : "InvoiceRef_ID = " + InvoiceRef_ID);
        DataTable selectedDetailsdt = new DataTable();
        selectedDetailsdt = selectedDetailsdr.CopyToDataTable();

        DataTable dtAccSt = (DataTable)ViewState["PODOMappings"];
        DataRow[] selecteddr = dtAccSt.Select(InvoiceRef_ID == "0" ? "SlNo = " + lblSlNo.Text : "InvoiceRef_ID = " + InvoiceRef_ID);
        DataTable selecteddt = new DataTable();
        selecteddt = selecteddr.CopyToDataTable();

        ddlReferenceType.SelectedValue = selecteddt.Rows[0]["RefTypeId"].ToString();
        txtReferenceDate.Text = selecteddt.Rows[0]["RefDate"].ToString();
        txtReferenceNumber.Text = selecteddt.Rows[0]["RefNumber"].ToString();
        txtValidfrom.Text = selecteddt.Rows[0]["ValidFrom"].ToString();
        txtValidTo.Text = selecteddt.Rows[0]["ValidTo"].ToString();
        ddlCashflow.SelectedValue = selecteddt.Rows[0]["CashflowTypeId"].ToString();
        ddlReferenceType.Enabled = ddlCashflow.Enabled = false;
        //txtReferenceDate.ReadOnly = txtValidfrom.ReadOnly = txtValidTo.ReadOnly = true;

        if (selectedDetailsdt.Rows.Count > 0)
        {
            grvAccounts.DataSource = selectedDetailsdt;
            grvAccounts.DataBind();

            CheckBox chkAll = (CheckBox)grvAccounts.HeaderRow.Cells[0].FindControl("chkAll");
            chkAll.Checked = true;

            foreach (GridViewRow gvrow in grvAccounts.Rows)
            {
                CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount");
                chkSelectAccount.Checked = true;
            }
        }
        else
        {
            grvAccounts.EmptyDataText = "No records Found";
            grvAccounts.DataBind();
        }

        btnAddAccounts.Text = "Modify";
        btnAddAccounts.Visible = true;
        btnReferenceNumberGo.Visible = false;
        pnlHistoryinDetail.Visible = false;
    }


    #region Email Group
    protected void btnAddEmailGroup_Click(object sender, EventArgs e)
    {
        try
        {

            DataRow drGroupDetails;
            DataTable dtGroupDetails = (DataTable)ViewState["EmailDetailsTable"];



            foreach (GridViewRow grvRow in grvEmailGroup.Rows)
            {
                Label lblGroupName = (Label)grvRow.FindControl("lblGroupName");
                if (lblGroupName.Text == txtEmailGroupName.Text)
                {
                    Utility.FunShowAlertMsg(this, "Group name cannot be duplicated");
                    return;
                }
            }

            drGroupDetails = dtGroupDetails.NewRow();
            //string strGroupMapId = Convert.ToString(dtGroupDetails.Rows.Count + 1);

            drGroupDetails["Cust_Email_Det_ID"] = 0;

            if (ddlBillEmailType.SelectedValue == "3")
            {
                drGroupDetails["State_ID"] = ddlStateGroup.SelectedValue;
                drGroupDetails["State_Name"] = ddlStateGroup.SelectedItem.Text;
            }
            else
            {
                drGroupDetails["State_ID"] = 0;
                drGroupDetails["State_Name"] = 0;
            }

            drGroupDetails["Group_Name"] = txtEmailGroupName.Text;
            // drDetails["Master_ID"] = "0";
            drGroupDetails["Email_ID"] = txtEmailIDGroup.Text;
            drGroupDetails["Email_CC"] = txtEmailCCGroup.Text;

            dtGroupDetails.Rows.Add(drGroupDetails);

            grvEmailGroup.DataSource = dtGroupDetails;
            grvEmailGroup.DataBind();
            ViewState["EmailDetailsTable"] = dtGroupDetails;
            ClearGroupDetailsControls();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Add Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void grvEmailGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddlBillEmailType.SelectedValue == "3")
                {
                    grvEmailGroup.Columns[2].Visible = true;
                }
                else
                {
                    grvEmailGroup.Columns[2].Visible = false;
                }
            }
            if (strMode != "Q")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    /* For Deleting purpose, Restrcit to add attribute to the Cell Remove linkbutton*/
                    for (int intCellIndex = 0; intCellIndex < e.Row.Cells.Count - 1; intCellIndex++)
                    {
                        e.Row.Cells[intCellIndex].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                        (this.grvEmailGroup, "Select$" + e.Row.RowIndex);
                    }
                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load the Email Group Details");
        }
    }

    protected void grvEmailGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete = (DataTable)ViewState["EmailDetailsTable"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            grvEmailGroup.DataSource = dtDelete;
            grvEmailGroup.DataBind();
            ViewState["EmailDetailsTable"] = dtDelete;
            if (grvEmailGroup.Rows.Count == 0)
            {
                btnAddGroup.Enabled = true;
                btnModifyGroup.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Remove Email Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ClearGroupDetailsControls()
    {
        try
        {
            ddlStateGroup.SelectedValue = "0";
            txtEmailGroupName.Text = "";
            txtEmailIDGroup.Text = "";
            txtEmailCCGroup.Text = "";

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void ddlBillEmailType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadBillEmailDet();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = Resources.LocalizationResources.ConstitutionChangeError;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void FunPriLoadBillEmailDet()
    {
        try
        {
            btnModifyGroup.Enabled = false;
            if (ddlBillEmailType.SelectedValue == "0")
            {
                pnlEmailGroup.Visible = false;
            }

            if (ddlBillEmailType.SelectedValue == "1")
            {
                pnlEmailGroup.Visible = true;
                lblStateGroup.Visible = false;
                ddlStateGroup.Visible = false;
                lblGroupName.Visible = false;
                txtEmailGroupName.Visible = false;
            }

            if (ddlBillEmailType.SelectedValue == "2")
            {
                pnlEmailGroup.Visible = true;
                //lblEMailIDSingle.Visible = false;
                //txtEMailIDSingle.Visible = false;
                lblStateGroup.Visible = false;
                ddlStateGroup.Visible = false;
                lblGroupName.Visible = true;
                txtEmailGroupName.Visible = true;

            }
            if (ddlBillEmailType.SelectedValue == "3")
            {
                pnlEmailGroup.Visible = true;
                //lblEMailIDSingle.Visible = false;
                //txtEMailIDSingle.Visible = false;
                lblStateGroup.Visible = true;
                ddlStateGroup.Visible = true;
                lblGroupName.Visible = true;
                txtEmailGroupName.Visible = true;
            }
            
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
        protected void btnModifyEmailGroup_Click(object sender, EventArgs e)
    {
        try
        {


            DataTable dtEmailGroupDetails = (DataTable)ViewState["EmailDetailsTable"];

            DataView dvEmailGroupdetails = dtEmailGroupDetails.DefaultView;
            //dvEmailGroupdetails.Sort = "Account_Number";
            int rowindex = Convert.ToInt16(hdnEmailGroupId.Value);

            //DataRow[] drDefault = dtBankDetails.Select("Is_Default_Account = 'True' and (CUSTOMER_BANK_ID<>'" + dvEmailGroupdetails[rowindex].Row["CUSTOMER_BANK_ID"].ToString() + "')");
            //if (drDefault.Length > 0 && chkDefaultAccount.Checked)
            //{
            //    Utility.FunShowAlertMsg(this, "Only one Default Account is applicable to particular Customer");
            //    return;
            //}

            dvEmailGroupdetails[rowindex].Row["State_ID"] = ddlStateGroup.SelectedValue;
            dvEmailGroupdetails[rowindex].Row["State_Name"] = ddlStateGroup.SelectedItem.Text;
            dvEmailGroupdetails[rowindex].Row["Group_Name"] = txtEmailGroupName.Text.Trim();
            dvEmailGroupdetails[rowindex].Row["Email_ID"] = txtEmailIDGroup.Text.Trim();
            dvEmailGroupdetails[rowindex].Row["Email_CC"] = txtEmailCCGroup.Text.Trim();

            grvEmailGroup.DataSource = dvEmailGroupdetails;
            grvEmailGroup.DataBind();
            //FunPriHideBankColumns();
            ViewState["EmailDetailsTable"] = dvEmailGroupdetails.Table;
            ClearGroupDetailsControls();
            btnAddGroup.Enabled = true;
            btnModifyGroup.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem, Unable to Modify the Bank Details";
            cvCustomerMaster.IsValid = false;

        }
    }

    protected void grvEmailGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Label lblStateIDGroup = grvEmailGroup.SelectedRow.FindControl("lblStateIDGroup") as Label;
            Label lblGroupName = grvEmailGroup.SelectedRow.FindControl("lblGroupName") as Label;
            Label lblEmailID = grvEmailGroup.SelectedRow.FindControl("lblEmailID") as Label;
            Label lblEmailCC = grvEmailGroup.SelectedRow.FindControl("lblEmailCC") as Label;
            hdnEmailGroupId.Value = Convert.ToString(grvEmailGroup.SelectedRow.RowIndex);
            ///Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)

            ddlStateGroup.SelectedValue = lblStateIDGroup.Text;
            txtEmailGroupName.Text = lblGroupName.Text;
            txtEmailIDGroup.Text = lblEmailID.Text;
            txtEmailCCGroup.Text = lblEmailCC.Text;

            ///Modified By prabhu on 16-Nov-2011 for CR (Default Account in Bank Details)

            btnAddGroup.Enabled = false;
            btnModifyGroup.Enabled = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Due to Data Problem,Unable to view the Bank Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    private void FunPriLoadCustomerBillEmailDetails(int CustomerID)
    {
        OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objCustomerMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            ObjStatus.Option = 201;
            ObjStatus.Param1 = CustomerID.ToString();
            ViewState["EmailDetailsTable"] = objCustomerMasterClient.FunPub_GetS3GStatusLookUp(ObjStatus);
            if (ViewState["EmailDetailsTable"] != null)
            {
                grvEmailGroup.DataSource = (DataTable)ViewState["EmailDetailsTable"];
                grvEmailGroup.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem, Unable to Load Bank Details");
        }
        finally
        {
            objCustomerMasterClient.Close();
        }
    }

    protected void btnClearEmailGroup_Click(object sender, EventArgs e)
    {
        try
        {

            ddlStateGroup.SelectedIndex = -1;
            txtEmailGroupName.Text = "";
            txtEmailIDGroup.Text = "";
            txtEmailCCGroup.Text = "";
            btnAddGroup.Enabled = true;
            btnModifyGroup.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Clear the Data in Email Group Details";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

}
