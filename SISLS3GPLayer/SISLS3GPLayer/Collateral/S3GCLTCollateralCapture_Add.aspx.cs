﻿/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name               : COLLATERAL
/// Screen Name               : COLLATERAL CAPTURE
/// Created By                : NARASIMHA RAO.P
/// Created Date              : 05-May-2011
/// Purpose                   : 
/// Last Updated By           : NARASIMHA RAO.P
/// Reason                    : 
/// Last Updated By           : Thalaiselvam N
/// Last Updated Date         : 31-Aug-2011
/// Reason                    : Password Validation
/// Last Updated By           : Saranya I
/// Last Updated Date         : 21-Mar-2012
///  Reason                    : Alignment Changes and UAT Bug fixing
/// Last Updated By           : Bhuvana.C
/// Last Updated Date         : 28-May-2012
/// Reason                    : Fully screen changing for uat bug fixing.



/// <Program Summary>

#region "Namespaces"

using System;
using System.Collections;
using System.Collections.Generic;
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
using S3GBusEntity;
using COLLATERAL = S3GBusEntity.Collateral;
using COLLATERALSERVICES = CollateralMgtServicesReference;
using System.IO;
using System.Globalization;
using System.Web.Services;
using System.ServiceModel;
using System.Text;

#endregion

public partial class Collateral_S3GCLTCollateralCapture_Add : ApplyThemeForProject
{

    #region [Common Variable declaration]
    DataTable dtSecurities;
    int intCompanyId, intUserId = 0;
    int intCollTranId = 0;
    int intTotal = 0;
    static int intCustomer = 0;

    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    string strAccount_ID = "0";
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strXMLHighLIQSecuritiesDet;
    string strXMLMediumLIQSecuritiesDet;
    string strXMLLOWLIQSecuritiesDet;
    string strXMLCommoditiesLIQSecuritiesDet;
    string strXMLFinancialLIQSecuritiesDet;

    string strRedirectPage = "~/Collateral/S3GCLTTransLander.aspx?Code=KCP";
    string strRedirectPageClear = "~/Collateral/S3GCLTCollateralCapture_Add.aspx?qsMode=C";
    string strRedirectPageAdd = "window.location.href='../Collateral/S3GCLTCollateralCapture_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Collateral/S3GCLTTransLander.aspx?Code=KCP';";
    SerializationMode SerMode = SerializationMode.Binary;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;

    public static Collateral_S3GCLTCollateralCapture_Add obj_Page;

    //bool bolHighLiqSecEditEvent = false;
    //bool bolMediumLiqSecEditEvent = false; 
    //bool bolLowLiqSecEditEvent = false; 
    //bool bolCommoditiesLiqSecEditEvent = false; 
    //bool bolFinancialLiqSecEditEvent = false;

    int RowID;
    string strCollateralType;
    //Code end

    CollateralMgtServicesReference.CollateralMgtServicesClient ObjCollateralClient;
    COLLATERAL.CollateralMgtServices.S3G_CLT_CollateralCaptureDataTable ObjS3G_CLT_CollateralCaptureDataTable = null;

    #endregion

    #region "Page Load Event"
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            //CalendarExtender2.Format = strDateFormat;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            //Date Format
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
            //added by saranya
            if (ddlType.SelectedIndex > 0)
            {
                if (Convert.ToInt32(ddlType.SelectedValue) == 2)
                {
                    ucCustomerCodeLov.strLOV_Code = "GCMD";
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == 3)
                {
                    ucCustomerCodeLov.strLOV_Code = "PCMD";
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == 4)
                {
                    ucCustomerCodeLov.strLOV_Code = "CCMD";
                }
                else
                {
                    ucCustomerCodeLov.strLOV_Code = "CCP";
                }
                ViewState["Type"] = ucCustomerCodeLov.strLOV_Code;
            }
            else
            {
                ucCustomerCodeLov.strLOV_Code = "CCP";
            }
            //end
            if (ddlConstitution.SelectedValue.ToString() != "0")
                ucCustomerCodeLov._strConstitutionID = ddlConstitution.SelectedValue;
            //if (ddlBranch.SelectedValue.ToString() != "0")
            //    ucCustomerCodeLov._strBranchID = ddlBranch.SelectedValue;
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            if (PageMode == PageModes.Create)
            {
                txt.Attributes.Add("onfocus", "fnLoadCustomer()");
            }
            txt.ToolTip = "Customer";
            if (intCompanyId == 0)
                intCompanyId = ObjUserInfo.ProCompanyIdRW;
            if (intUserId == 0)
                intUserId = ObjUserInfo.ProUserIdRW;

            //Code end
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intCollTranId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid Collateral Capture Details");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            txthMaturityDate.Attributes.Add("onblur", "fnDoDate(this,'" + txthMaturityDate.ClientID + "','" + strDateFormat + "',false, true);");
            txtCDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCDate.ClientID + "','" + strDateFormat + "',false, false);");
            txtFMaturityDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtFMaturityDate.ClientID + "','" + strDateFormat + "',false, true);");
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            if (!IsPostBack)
            {
                FunToolTip();

                FunPriBindLook();
                FunPriInitialRow(Securities.HighLiquid);
                FunPriSetAlignmentsHighLIQSecurities();
                FunPriInitialRow(Securities.MediumLiquid);
                FunPriSetAlignmentsMediumLIQSecurities();
                FunPriInitialRow(Securities.LowLiquid);
                FunPriSetAlignmentsLOWLIQSecurities();

                FunPriInitialRow(Securities.Commodities);
                FunPriSetAlignmentsCommLIQSecurities();

                FunPriInitialRow(Securities.Financial);
                FunPriSetAlignmentsFinLIQSecurities();

                txtCollTransDate.Text = DateTime.Now.ToString(_DateFormat);
                //ViewState["LogedInPassword"] = returnLoginUserPassword();

                if (intCollTranId > 0)
                {
                    FunPubProGetCollateralCaptureDetails();
                }
                if (strMode == "Q")
                {
                    FunPriDisableControls(-1);
                }
                else if (strMode == "M")
                {
                    FunPriDisableControls(1);

                }
                else
                {
                    FunPriLoadLOV();
                    FunPriDisableControls(0);
                }
            }
        }
        catch (Exception objExp)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objExp);
            throw objExp;
        }
    }
    #endregion

    #region "User Defined Functions"
    private void FunToolTip()
    {
        //throw new NotImplementedException();
        //ddlBranch.ToolTip = lblBranch.Text;
        ddlLOB.ToolTip = lblLOB.Text;
        ddlRefPoint.ToolTip = lblRefPoint.Text;
        ddlProductCode.ToolTip = lblProductCode.Text;
        //txtCollTransNo.ToolTip = lblCollTransNo.Text;
        txtCollTransDate.ToolTip = lblCollTransDate.Text;
        ddlCurrCode.ToolTip = lblCurrCode.Text;
        ddlConstitution.ToolTip = lblConstitution.Text;
        txtCustomerCode.ToolTip = lblCustorAgent.Text;
        btnSave.ToolTip = btnSave.Text;
        btnCancel.ToolTip = btnCancel.Text;
        btnClear.ToolTip = btnClear.Text;
    }
    private void FunPriLoadLOV()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            /***** CONSTITUTION MASTER *************/
            FunPriBindConstitution();
            /***** LOAD LOB *************/
            FunPriBindLOB();

            /***** LOAD BRANCH *************/
            FunPriBindBranch();
            /***** PROCUCT CODE *************/
            //FunPriBindProduct();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriBindLook()
    {
        try
        {
            if (PageMode == PageModes.Create)
            {
                /***** LOAD Type *************/
                FunPriBindType();

                /***** REF POINT *************/
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@OPTION", "11");
                Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
                Procparam.Add("@USERID", Convert.ToString(intUserId));
                ddlRefPoint.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "LOOKUP_CODE", "LOOKUP_DESCRIPTION" });
            }

            if (PageMode != PageModes.Query)
            {
                /***** CURRENCY MASTER *************/
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@OPTION", "5");
                Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
                Procparam.Add("@USERID", Convert.ToString(intUserId));
                //if (PageMode != PageModes.Create)
                //    Procparam.Add("@COLLATERAL_ID", Convert.ToString(intCollTranId));
                ddlCurrCode.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "Currency_ID", "Currency_Code", "Currency_Name" });
            }

            FunPriFillCollateralDescription("1", ddlhCollSecurities);
        }


        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriBindType()
    {
        try
        {
            Procparam = new Dictionary<string, string>();

            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));

            ddlType.BindDataTable("S3G_CLT_GetCustomerType", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriBindProduct()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            if (strMode == "C")
                Procparam.Add("@Is_Active", "1");
            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));

            ddlProductCode.BindDataTable(SPNames.SYS_ProductMaster, Procparam, new string[] { "Product_ID", "Product_Code", "Product_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriBindLOB()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Program_ID", "163");
            if (strMode == "C")
                Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_Id", Convert.ToString(intUserId));
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriBindBranch()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Program_ID", "163");
            if (strMode == "C")
                Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_Id", Convert.ToString(intUserId));
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriBindConstitution()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@OPTION", "16");
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@USERID", Convert.ToString(intUserId));
            //if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            //    Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            ddlConstitution.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "Constitution_ID", "ConstitutionName" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPriInitialRow(Securities LiqSecurities)
    {
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();
            dtSecurities = new DataTable();
            DataRow dr;
            switch (LiqSecurities)
            {
                case Securities.HighLiquid:
                    {
                        //******* Fill Initial Row For HIGH Liq Detail Grid  ******* //
                        dtSecurities.Columns.Add("Collateral_Type_ID");
                        dtSecurities.Columns.Add("SlNo");
                        dtSecurities.Columns.Add("Mode");
                        dtSecurities.Columns.Add("Issued_By");
                        dtSecurities.Columns.Add("Collateral_Detail_ID"); // Coll Detail ID
                        dtSecurities.Columns.Add("Collateral_Securities"); // Coll Securities Name...
                        dtSecurities.Columns.Add("Demat");      // Demat value (0/1)
                        dtSecurities.Columns.Add("DematDesc");  //Demat Desc (Yes/No)
                        dtSecurities.Columns.Add("DP_Name");
                        dtSecurities.Columns.Add("DP_No");
                        dtSecurities.Columns.Add("Client_ID");
                        dtSecurities.Columns.Add("Certificate_No");
                        dtSecurities.Columns.Add("Folio_No");
                        dtSecurities.Columns.Add("Unit_Face_Value");
                        dtSecurities.Columns.Add("No_Of_Units");
                        dtSecurities.Columns.Add("Interest_Percentage");
                        dtSecurities.Columns.Add("Maturity_Date");
                        dtSecurities.Columns.Add("Maturity_Value");
                        dtSecurities.Columns.Add("Market_Rate");
                        dtSecurities.Columns.Add("Market_Value");
                        dtSecurities.Columns.Add("Collateral_Ref_No");
                        dtSecurities.Columns.Add("Scan_Ref_No");
                        dtSecurities.Columns.Add("Collateral_Item_Ref_No");
                        dtSecurities.Columns.Add("Collateral_Tran_No");
                        dtSecurities.Columns.Add("Ownership");
                        dr = dtSecurities.NewRow();
                        dr["SlNo"] = "0";
                        dtSecurities.Rows.Add(dr);
                        gvhHighLiqDetails.DataSource = dtSecurities;
                        gvhHighLiqDetails.DataBind();

                        //gvHighLiqDetails.DataSource = dtSecurities;
                        //gvHighLiqDetails.DataBind();

                        gvhHighLiqDetails.Rows[0].Visible = false;
                        ViewState[Securities.HighLiquid.ToString()] = dtSecurities;
                        break;
                    }
                case Securities.MediumLiquid:
                    {
                        //******* Fill Initial Row For MEDIUM Liq Detail Grid  ******* //
                        dtSecurities.Columns.Add("Collateral_Type_ID");
                        dtSecurities.Columns.Add("SlNo");
                        dtSecurities.Columns.Add("Mode");
                        dtSecurities.Columns.Add("Collateral_Detail_ID");
                        dtSecurities.Columns.Add("Collateral_Securities");
                        dtSecurities.Columns.Add("Description");
                        dtSecurities.Columns.Add("Model");
                        dtSecurities.Columns.Add("Year");
                        dtSecurities.Columns.Add("Registration_No");
                        dtSecurities.Columns.Add("Serial_No");
                        dtSecurities.Columns.Add("Value");
                        dtSecurities.Columns.Add("Market_Value");
                        dtSecurities.Columns.Add("Collateral_Ref_No");
                        dtSecurities.Columns.Add("Scan_Ref_No");
                        dtSecurities.Columns.Add("Collateral_Item_Ref_No");
                        dtSecurities.Columns.Add("Ownership_Medium");
                        dr = dtSecurities.NewRow();
                        dr["SlNo"] = "0";
                        dtSecurities.Rows.Add(dr);
                        //gvMedLiqDetails.DataSource = dtSecurities;
                        //gvMedLiqDetails.DataBind();
                        //gvMedLiqDetails.Rows[0].Visible = false;

                        gvMMedLiqDetails.DataSource = dtSecurities;
                        gvMMedLiqDetails.DataBind();
                        gvMMedLiqDetails.Rows[0].Visible = false;
                        btnModifyM.Enabled = false;
                        ViewState[Securities.MediumLiquid.ToString()] = dtSecurities;
                        break;
                    }
                case Securities.LowLiquid:
                    {
                        //******* Fill Initial Row For LOW Liq Detail Grid  ******* //
                        dtSecurities = new DataTable();
                        dtSecurities.Columns.Add("Collateral_Type_ID");
                        dtSecurities.Columns.Add("SlNo");
                        dtSecurities.Columns.Add("Mode");
                        dtSecurities.Columns.Add("Collateral_Detail_ID");
                        dtSecurities.Columns.Add("Collateral_Securities"); // Coll Securities Name...
                        dtSecurities.Columns.Add("Location_Details");
                        dtSecurities.Columns.Add("Measurement");
                        dtSecurities.Columns.Add("Unit_Rate");
                        dtSecurities.Columns.Add("Value");
                        dtSecurities.Columns.Add("Market_Value");
                        dtSecurities.Columns.Add("Collateral_Ref_No");
                        dtSecurities.Columns.Add("Scan_Ref_No");
                        dtSecurities.Columns.Add("Legal_OpinionDesc"); //(Yes/NO)
                        dtSecurities.Columns.Add("Legal_Opinion");       // 0/1
                        dtSecurities.Columns.Add("Legal_Scan_Ref_No");
                        dtSecurities.Columns.Add("EncumbranceDesc"); // (Yes/No)
                        dtSecurities.Columns.Add("Encumbrance");    //   0/1
                        dtSecurities.Columns.Add("Encumbrance_Scan_Ref_No");
                        dtSecurities.Columns.Add("Is_Asset_DocumentDesc");  //   (Yes/NO)
                        dtSecurities.Columns.Add("Is_Asset_Document");       //   0/1
                        dtSecurities.Columns.Add("AssetDoc_Scan_Ref_No");
                        dtSecurities.Columns.Add("Is_Valuation_CertificationDesc");      //  (Yes/NO)
                        dtSecurities.Columns.Add("Is_Valuation_Certification");      //   0/1
                        dtSecurities.Columns.Add("ValCertification_Scan_Ref_No");
                        dtSecurities.Columns.Add("Collateral_Item_Ref_No");
                        dtSecurities.Columns.Add("Ownership_Low");
                        dr = dtSecurities.NewRow();
                        dr["SlNo"] = "0";
                        dtSecurities.Rows.Add(dr);
                        //gvLowLiqDetails.DataSource = dtSecurities;
                        //gvLowLiqDetails.DataBind();
                        //gvLowLiqDetails.Rows[0].Visible = false;

                        gvLLowLiqDetails.DataSource = dtSecurities;
                        gvLLowLiqDetails.DataBind();
                        gvLLowLiqDetails.Rows[0].Visible = false;

                        ViewState[Securities.LowLiquid.ToString()] = dtSecurities;
                        btnModifyL.Enabled = false;
                        break;
                    }
                case Securities.Commodities:
                    {
                        //******* Fill Initial Row For COMMODITY Liq Detail Grid  ******* //
                        dtSecurities.Columns.Add("Collateral_Type_ID");
                        dtSecurities.Columns.Add("SlNo");
                        dtSecurities.Columns.Add("Mode");
                        dtSecurities.Columns.Add("Collateral_Detail_ID");
                        dtSecurities.Columns.Add("Collateral_Securities"); // Coll Securities Name...
                        dtSecurities.Columns.Add("Description");
                        dtSecurities.Columns.Add("Unit_Of_Measure");
                        dtSecurities.Columns.Add("UOM_Text");
                        dtSecurities.Columns.Add("Unit_Quantity");
                        dtSecurities.Columns.Add("Unit_Price");
                        dtSecurities.Columns.Add("Value");
                        dtSecurities.Columns.Add("Unit_Market_Price");
                        dtSecurities.Columns.Add("Date");
                        dtSecurities.Columns.Add("Collateral_Item_Ref_No");
                        dtSecurities.Columns.Add("Ownership_Comm");
                        dr = dtSecurities.NewRow();
                        dr["SlNo"] = "0";
                        dtSecurities.Rows.Add(dr);
                        //gvCommoDetails.DataSource = dtSecurities;
                        //gvCommoDetails.DataBind();
                        //gvCommoDetails.Rows[0].Visible = false;

                        gvCCommoDetails.DataSource = dtSecurities;
                        gvCCommoDetails.DataBind();
                        gvCCommoDetails.Rows[0].Visible = false;
                        btnModifyC.Enabled = false;
                        ViewState[Securities.Commodities.ToString()] = dtSecurities;
                        break;
                    }
                case Securities.Financial:
                    {
                        //******* Fill Initial Row For FINANCIAL Liq Detail Grid  ******* //
                        dtSecurities.Columns.Add("Collateral_Type_ID");
                        dtSecurities.Columns.Add("SlNo");
                        dtSecurities.Columns.Add("Mode");
                        dtSecurities.Columns.Add("Collateral_Detail_ID");
                        dtSecurities.Columns.Add("Collateral_Securities"); // Coll Securities Name...
                        dtSecurities.Columns.Add("Insurance_Issued_By");
                        dtSecurities.Columns.Add("Policy_No");
                        dtSecurities.Columns.Add("Policy_Value");
                        dtSecurities.Columns.Add("Current_Value");
                        dtSecurities.Columns.Add("Maturity_Date");
                        dtSecurities.Columns.Add("Collateral_Ref_No");
                        dtSecurities.Columns.Add("Scan_Ref_No");
                        dtSecurities.Columns.Add("Collateral_Item_Ref_No");
                        dtSecurities.Columns.Add("Ownership_Fin");
                        dr = dtSecurities.NewRow();
                        dr["SlNo"] = "0";
                        dtSecurities.Rows.Add(dr);
                        //gvFinDetails.DataSource = dtSecurities;
                        //gvFinDetails.DataBind();
                        //gvFinDetails.Rows[0].Visible = false;

                        gvFFinDetails.DataSource = dtSecurities;
                        gvFFinDetails.DataBind();
                        gvFFinDetails.Rows[0].Visible = false;

                        btnModifyF.Enabled = false;
                        ViewState[Securities.Financial.ToString()] = dtSecurities;
                        break;
                    }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    //private DataTable getInitialRow(GridView grdSecurities, Dictionary<string, string> dictProcParam)
    //{
    //    DataTable dtTable = new DataTable();
    //    DataRow drFirstRow;
    //    try
    //    {
    //        foreach (KeyValuePair<string, string> ProcPair in dictProcParam)
    //        {
    //            dtTable.Columns.Add(ProcPair.Key);
    //        }
    //        drFirstRow = dtTable.NewRow();
    //        foreach (KeyValuePair<string, string> ProcPair in dictProcParam)
    //        {
    //            drFirstRow[ProcPair.Key] = ProcPair.Value;
    //        }
    //        dtTable.Rows.Add(drFirstRow);
    //        grdSecurities.DataSource = dtTable;
    //        grdSecurities.DataBind();
    //        grdSecurities.Rows[0].Visible = false;
    //        return dtTable;
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex);
    //        throw ex;            
    //    }
    //    return dtTable;
    //}
    protected void FunPriDisableControls(int intModeID)
    {
        try
        {
            Button btnLdCustomer = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    ddlCurrCode.SelectedValue = ObjS3GSession.ProCurrencyIdRW.ToString();
                    ddlCollectionAgent.Visible = false;
                    //lblDYNRefPoint.Enabled = false;
                    ddlDYNRefPoint.Enabled = false;
                    ucCustomerCodeLov.Visible = true;
                    //btnLoadCustomer.Visible = true;
                    ddlProductCode.Visible = true;
                    lblProductCode.Visible = true;
                    //pnlCustDetails.Visible = false;
                    rbtlCollCollected.Enabled = true;
                    lblCustorAgent.Text = "Customer";
                    tabHighLiq.Enabled = false;

                    tabMedSeq.Enabled = false;
                    tabLow.Enabled = false;
                    tabCommodities.Enabled = false;
                    tabFinancial.Enabled = false;

                    txtTotal.Visible = false;
                    lblTotal.Visible = false;
                    txtCollTransDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    FunPriVisibilityMode(true, true, false, true);
                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                    if (!bModify)
                    {
                        btnSave.Visible = false;
                    }
                    FunPriControlVisibility(Convert.ToInt32(rbtlCollCollected.SelectedValue));
                    //ddlLOB.ClearDropDownList();
                    // ddlBranch.ClearDropDownList();
                    // ddlProductCode.ClearDropDownList();
                    //ddlConstitution.ClearDropDownList();
                    if (ddlRefPoint.Items.Count > 0)
                        ddlRefPoint.ClearDropDownList();
                    if (ddlRefPoint.SelectedValue == "3" && intCustomer == 0)
                    {
                        rfvCustomerCode.Enabled = false;
                    }
                    else
                    {
                        rfvCustomerCode.Enabled = true;
                    }
                    if (ddlType.Items.Count > 0)
                    {
                        ddlType.ClearDropDownList();
                    }
                    btnLdCustomer.Visible = false;
                    ddlCollectionAgent.Enabled = false;
                    rbtlCollCollected.Enabled = false;
                    ddlBranch.ReadOnly = true;
                    //ddlLOB.Enabled = ddlBranch.Enabled = 
                    btnClear.Enabled = btnGo.Enabled = false;

                    FunPriVisibilityMode(true, true, true, true);

                    btnSave.Text = "Save";
                    btnSave.ToolTip = "Save";
                    tabHighLiq.Enabled = true;
                    tabMedSeq.Enabled = true;
                    tabLow.Enabled = true;
                    tabCommodities.Enabled = true;
                    tabFinancial.Enabled = true;
                    txtTotal.Enabled = true;
                    break;

                case -1:// Query Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = btnClear.Enabled = false;
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }

                    btnLdCustomer.Visible = false;
                    ddlCollectionAgent.Enabled = false;
                    FunPriControlVisibility(Convert.ToInt32(rbtlCollCollected.SelectedValue));
                    //ddlLOB.ClearDropDownList();
                    if (ddlType.Items.Count > 0)
                        ddlType.ClearDropDownList();                  
                   
                    // ddlBranch.ClearDropDownList();
                    // ddlProductCode.ClearDropDownList();
                    if (ddlCurrCode.Items.Count > 0)
                        ddlCurrCode.ClearDropDownList();
                                    
                    //ddlConstitution.ClearDropDownList();
                    ddlRefPoint.ClearDropDownList();
                    if (ddlDYNRefPoint.Items.Count > 0)
                        ddlDYNRefPoint.ClearDropDownList();
                    rbtlCollCollected.Enabled = false;
                    if (grvprimeaccount.DataSource != null)
                        grvprimeaccount.Enabled = false;
                    ddlBranch.ReadOnly = true;
                    FunPriVisibilityMode(false, false, false, false);
                    tabHighLiq.Enabled = true;
                    tabMedSeq.Enabled = true;
                    tabLow.Enabled = true;
                    tabCommodities.Enabled = true;
                    tabFinancial.Enabled = true;
                    btnGo.Enabled = false;
                    FunPubhHighDisableQueryControls();
                    FunPubMedDisableQueryControls();
                    FunPubLowDisableQueryControls();
                    FunPubComDisableQueryControls();
                    FunPubFinDisableQueryControls();

                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to set the controls");
        }
    }

    private void FunPubhHighDisableQueryControls()
    {
        ddlhCollSecurities.Enabled = false;
        txthIssuedBy.ReadOnly = true;
        ddlhDemat.Enabled = false;
        txthDPName.ReadOnly = true;
        txthDPNo.ReadOnly = txthFolioNo.ReadOnly = txthCertificateNo.ReadOnly = txthClientID.ReadOnly = txthUnitFaceValue.ReadOnly =
        txthNoOfUnits.ReadOnly = txthInterest.ReadOnly = txthMaturityDate.ReadOnly = txthMaturityValue.ReadOnly =
        txthMarketRate.ReadOnly = txthMarketValue.ReadOnly = txthCollateralRefNo.ReadOnly = txthScanRefNo.ReadOnly =
        txthItemRefNo.ReadOnly = txthTranRefNo.ReadOnly = txthownership.ReadOnly = true;
        txthMaturityDate.Attributes.Remove("blur");
        btnAddH.Enabled = btnModifyH.Enabled = btnhClearH.Enabled = false;
    }
    private void FunPubMedDisableQueryControls()
    {
        ddlMCollSecurities.Enabled = false;
        txtMDescription.ReadOnly = txtMModel.ReadOnly = txtMYear.ReadOnly = txtMValue.ReadOnly = txtMRegistrationNo.ReadOnly =
        txtMSerialNo.ReadOnly = txtMMarketValue.ReadOnly = txtMCollateralRefNo.ReadOnly = txtMScanRefNo.ReadOnly =
        txtMItemRefNo.ReadOnly = txtMOwnership.ReadOnly = true;
        btnAddM.Enabled = btnModifyM.Enabled = btnClearM.Enabled = false;
    }
    private void FunPubLowDisableQueryControls()
    {
        ddlLCollSecurities.Enabled = ddlLAssetDocument.Enabled = ddlLEncumbrance.Enabled = ddlLLegalOpinion.Enabled = ddlLValuationCertificate.Enabled = false;
        txtLLocationDetails.ReadOnly = txtLMeasurement.ReadOnly = txtLUnitRate.ReadOnly = txtLValue.ReadOnly = txtLMarketValue.ReadOnly =
        txtLCollateralRefNo.ReadOnly = txtLScanRefNo.ReadOnly = txtLEncumbranceScanRefNo.ReadOnly = txtLLegalScanRefNo.ReadOnly =
        txtLAssetDocScanRefNo.ReadOnly = txtLValCertificationScanRefNo.ReadOnly = txtLItemRefNo.ReadOnly = txtLOwnership.ReadOnly = true;
        btnAddL.Enabled = btnModifyL.Enabled = btnClearL.Enabled = false; ;
    }
    private void FunPubComDisableQueryControls()
    {
        ddlCCollSecurities.Enabled = ddlCUnitOfMeasure.Enabled = false;
        txtCDescription.ReadOnly = txtCUnitQty.ReadOnly = txtCUnitPrice.ReadOnly = txtCValue.ReadOnly = txtCUnitMarketPrice.ReadOnly =
        txtCDate.ReadOnly = txtCItemRefNo.ReadOnly = txtFOwnership.ReadOnly = txtCOwnership.ReadOnly = true;
        txtCDate.Attributes.Remove("blur");
        btnAddC.Enabled = btnModifyC.Enabled = btnClearC.Enabled = false;
    }
    private void FunPubFinDisableQueryControls()
    {
        ddlFCollSecurities.Enabled = false;
        txtFInsuranceIssuedBy.ReadOnly = txtFPolicyNo.ReadOnly = txtFPolicyValue.ReadOnly = txtFCurrentValue.ReadOnly =
        txtFMaturityDate.ReadOnly = txtFScanRef.ReadOnly = txtFItemRefNo.ReadOnly = txtFCollateralRef.ReadOnly = txtFOwnership.ReadOnly = false;
        txtFMaturityDate.Attributes.Remove("blur");
        btnAddF.Enabled = btnModifyF.Enabled = btnClearF.Enabled = false;
    }
    private void FunPriVisibilityMode(bool Deletevisibility, bool Editvisibility, bool Excludevisibility, bool Footervisibility)
    {
        gvHighLiqDetails.Columns[gvHighLiqDetails.Columns.Count - 1].Visible = Deletevisibility;
        gvMedLiqDetails.Columns[gvMedLiqDetails.Columns.Count - 1].Visible = Deletevisibility;
        gvLowLiqDetails.Columns[gvLowLiqDetails.Columns.Count - 1].Visible = Deletevisibility;
        gvCommoDetails.Columns[gvCommoDetails.Columns.Count - 1].Visible = Deletevisibility;
        gvFinDetails.Columns[gvFinDetails.Columns.Count - 1].Visible = Deletevisibility;

        gvHighLiqDetails.Columns[gvHighLiqDetails.Columns.Count - 2].Visible = Editvisibility;
        gvMedLiqDetails.Columns[gvMedLiqDetails.Columns.Count - 2].Visible = Editvisibility;
        gvLowLiqDetails.Columns[gvLowLiqDetails.Columns.Count - 2].Visible = Editvisibility;
        gvCommoDetails.Columns[gvCommoDetails.Columns.Count - 2].Visible = Editvisibility;
        gvFinDetails.Columns[gvFinDetails.Columns.Count - 2].Visible = Editvisibility;


        //gvHighLiqDetails.Columns[gvHighLiqDetails.Columns.Count - 3].Visible = Excludevisibility;
        //gvMedLiqDetails.Columns[gvMedLiqDetails.Columns.Count - 3].Visible = Excludevisibility;
        //gvLowLiqDetails.Columns[gvLowLiqDetails.Columns.Count - 3].Visible = Excludevisibility;
        //gvCommoDetails.Columns[gvCommoDetails.Columns.Count - 3].Visible = Excludevisibility;
        //gvFinDetails.Columns[gvFinDetails.Columns.Count - 3].Visible = Excludevisibility;

        if (gvHighLiqDetails.FooterRow != null)
        {
            gvHighLiqDetails.FooterRow.Visible = Footervisibility;
            gvMedLiqDetails.FooterRow.Visible = Footervisibility;
            gvLowLiqDetails.FooterRow.Visible = Footervisibility;
            gvCommoDetails.FooterRow.Visible = Footervisibility;
            gvFinDetails.FooterRow.Visible = Footervisibility;
        }

    }
    private void FunPriControlVisibility(int CollCollected)
    {
        switch (CollCollected)
        {
            case 1:    //Customer
                ddlRefPoint.Visible = true;
                lblRefPoint.Visible = true;
                ddlCollectionAgent.Visible = false;
                lblCustorAgent.Text = "Customer";
                ucCustomerCodeLov.Visible = true;
                break;
            case 2:  //Collection Agent
                ddlRefPoint.Visible = true;
                lblRefPoint.Visible = true;
                ddlRefPoint.Enabled = false;
                TextBox txtCustName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtCustName.Visible = false;
                txtCustomerCode.Visible = false;
                ddlCollectionAgent.Visible = true;
                lblCustorAgent.Text = "Collection Agent";
                ucCustomerCodeLov.Visible = true;
                break;
        }
    }
    private void PopulateCustomerDetails(string strCustID, string custOption)  // custOption 1=Custmoer Master Details, 2=ThirdParty Details
    {
        try
        {
            DataSet ds;
            DataTable dtTable = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Clear();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@User_ID", Convert.ToString(intUserId));
            Procparam.Add("@Option", custOption);
            Procparam.Add("@Lob_ID", Convert.ToString(ddlLOB.SelectedValue));
            Procparam.Add("@Customer_ID", strCustID);
            Procparam.Add("@chkCustomer", "1");
            ds = Utility.GetDataset("S3G_CLT_GetCustomerDetails", Procparam);
            if (ds.Tables.Count > 0)
            {
                dtTable = ds.Tables[0];
                intCustomer = custOption == "1" ? Convert.ToInt32(dtTable.Rows[0]["Customer_ID"].ToString()) : 0;
                S3GCustomerAddress1.SetCustomerDetails(dtTable.Rows[0], true);
                txtCustomerCode.Text = dtTable.Rows[0]["Customer_Name"].ToString();
                ddlConstitution.SelectedValue = dtTable.Rows[0]["CONSTITUTION_ID"].ToString();
            }
            else
            {
                intCustomer = 0;
                TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                txtName.Text = "";
                txtCustomerCode.Text = "";
                //pnlCustDetails.Visible = false;
                S3GCustomerAddress1.ClearCustomerDetails();
                ddlConstitution.SelectedValue = "0";
                Utility.FunShowAlertMsg(this.Page, "Customer already existed, Please choose another customer");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
        }
    }
    private void FunPriCollateralCollectedBy()
    {

        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        S3GCustomerAddress1.ClearCustomerDetails();
        txtCustomerCode.Text = "";
        TextBox txtCustName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        txtCustName.Text = "";
        intCustomer = 0;
        //pnlCustDetails.Visible = false;
        //lblDYNRefPoint.Enabled = false;
        ddlDYNRefPoint.Enabled = false;
        lblCustorAgent.Visible = true;
        ddlProductCode.SelectedValue = "0";
        ddlLOB.SelectedValue = "0";
        ddlConstitution.SelectedValue = "0";
        ddlRefPoint.SelectedValue = "0";
        ddlBranch.SelectedValue = "0";
        grvprimeaccount.DataSource = null;
        grvprimeaccount.DataBind();
        pnlPASADetails.Visible = false;
        int i = Convert.ToInt32(rbtlCollCollected.SelectedValue);
        switch (i)
        {
            case 1:
                FunPriEnableTab();
                ddlCollectionAgent.Visible = false;
                //btnLoadCustomer.Visible = true;
                lblCustorAgent.Text = "Customer Name";
                lblRefPoint.Visible = true;
                ddlRefPoint.Visible = true;
                ucCustomerCodeLov.Visible = true;
                ddlType.Enabled = true;
                ddlRefPoint.Enabled = true;
                pnlCustDetails.GroupingText = "Customer Details";
                rfvCustomerCode.ErrorMessage = "Select a Customer";
                rfvRefPoint.Enabled = false;
                break;
            case 2:
                FunPriEnableTab();
                Procparam.Add("@OPTION", "12");
                Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
                Procparam.Add("@USERID", Convert.ToString(intUserId));
                if (strMode == "C")
                    Procparam.Add("@Is_Active", "1");
                if (strMode == "C" && ddlLOB.SelectedValue != "0")
                    Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                if (strMode == "C" && ddlBranch.SelectedValue != "0")
                    Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));

                lblCustorAgent.Text = "Collection Agent";
                ddlCollectionAgent.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "Emp_UTPA_ID", "DebtCollector_Code" });
                ddlCollectionAgent.Visible = true;
                //lblRefPoint.Visible = false;
                ddlRefPoint.Enabled = false;
                //btnLoadCustomer.Visible = false;
                ucCustomerCodeLov.Visible = false;
                pnlCustDetails.GroupingText = "Collection Agent Details";
                rfvCustomerCode.ErrorMessage = "Select a Collection Agent";
                rfvCustomerCodeGo.ErrorMessage = "Select a Collection Agent";
                rfvRefPoint.Enabled = false;
                ddlType.ClearSelection();
                lblRefPoint.CssClass = "styleDisplayLabel";
                lblDYNRefPoint.CssClass = "styleDisplayLabel";
                lblType.CssClass = "styleDisplayLabel";
                ddlType.Enabled = false;
                rfvType.Enabled = false;
                rfvTypeGo.Enabled = false;
                break;
        }
    }
    private void FunPubFillDynamicRefPointField()
    {

        if (ddlRefPoint.SelectedValue != "0")
        {
            if (ddlRefPoint.SelectedValue != "3")
            {
                if (intCustomer == 0 && strMode == "C")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select Customer");
                    ddlRefPoint.SelectedIndex = 0;
                    return;
                }
            }
            //pnlPASADetails.Visible = false;
            //ddlDYNRefPoint.Enabled = true;
            //lblDYNRefPoint.CssClass = "styleReqFieldLabel";

            //rfvDYNRefPoint.Enabled = true;
            ddlDYNRefPoint.ClearSelection();
            grvprimeaccount.DataSource = null;
            grvprimeaccount.DataBind();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            int value = Convert.ToInt32(ddlRefPoint.SelectedValue);

            ddlDYNRefPoint.Enabled = false;
            rfvDYNRefPoint.Enabled = false;
            pnlPASADetails.Visible = true;
            grvprimeaccount.Visible = true;
            pnlPASADetails.GroupingText = ddlRefPoint.SelectedItem.Text + " Details";
            grvprimeaccount.Columns[3].Visible = false;
            if (ddlLOB.SelectedValue == "0")
            {
                ddlRefPoint.SelectedValue = "0";
                Utility.FunShowAlertMsg(this.Page, "Select Line of Business");
                return;
            }
            switch (value)
            {
                case 3:
                    Procparam.Add("@OPTION", "7");
                    Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
                    Procparam.Add("@USERID", Convert.ToString(intUserId));
                    if (intCustomer != 0)
                    {
                        Procparam.Add("@Customer_ID", Convert.ToString(intCustomer));
                    }
                    Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                    if (strMode == "M" || strMode == "Q")
                    {
                        Procparam.Add("@Collateral_Capture_ID", intCollTranId.ToString());
                    }
                    lblDYNRefPoint.Text = "Enquiry Code";
                    rfvDYNRefPoint.ErrorMessage = "Select Enquiry Code";
                    grvprimeaccount.Columns[2].HeaderText = "Enquiry Number";
                    grvprimeaccount.BindGridView("S3G_CLT_LOADLOV", Procparam);
                    //ddlDYNRefPoint.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "EnquiryUpdation_ID", "Enquiry_No" });
                    break;

                case 4:
                    Procparam.Add("@OPTION", "8");
                    Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
                    Procparam.Add("@USERID", Convert.ToString(intUserId));
                    Procparam.Add("@Customer_ID", Convert.ToString(intCustomer));
                    Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                    if (strMode == "M" || strMode == "Q")
                    {
                        Procparam.Add("@Collateral_Capture_ID", intCollTranId.ToString());
                    }
                    lblDYNRefPoint.Text = "Pricing Code";
                    rfvDYNRefPoint.ErrorMessage = "Select Pricing Code";
                    grvprimeaccount.Columns[2].HeaderText = "Pricing Number";
                    grvprimeaccount.BindGridView("S3G_CLT_LOADLOV", Procparam);
                    //ddlDYNRefPoint.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "Pricing_ID", "Business_Offer_Number" });
                    break;
                case 5:
                    Procparam.Add("@OPTION", "9");
                    Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
                    Procparam.Add("@USERID", Convert.ToString(intUserId));
                    Procparam.Add("@Customer_ID", Convert.ToString(intCustomer));
                    Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                    if (strMode == "M" || strMode == "Q")
                    {
                        Procparam.Add("@Collateral_Capture_ID", intCollTranId.ToString());
                    }
                    lblDYNRefPoint.Text = "Application Code";
                    rfvDYNRefPoint.ErrorMessage = "Select Application Code";
                    grvprimeaccount.Columns[2].HeaderText = "Application Number";
                    grvprimeaccount.BindGridView("S3G_CLT_LOADLOV", Procparam);
                    //ddlDYNRefPoint.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "Application_Process_ID", "Application_Number" });
                    break;
                case 6:

                    ddlDYNRefPoint.Enabled = false;
                    //lblDYNRefPoint.Enabled = false;
                    ddlDYNRefPoint.Enabled = false;
                    //lblDYNRefPoint.Enabled = false;
                    rfvDYNRefPoint.Enabled = false;
                    grvprimeaccount.Columns[3].Visible = true;
                    grvprimeaccount.Columns[2].HeaderText = "Prime A/c No.";
                    lblDYNRefPoint.Text = "Ref Code";
                    if (ddlLOB.SelectedValue == "0")
                    {
                        ddlRefPoint.SelectedValue = "0";
                        Utility.FunShowAlertMsg(this.Page, "Select Line of Business");
                        return;
                    }
                    FunPriFillAccountDetails();
                    //if (FunPriFillAccountDetails().Rows.Count > 0)
                    //    pnlPASADetails.Visible = true;
                    //else
                    //{
                    //    pnlPASADetails.Visible = false;
                    //    Utility.FunShowAlertMsg(this.Page, "No accounts for the selected customer");
                    //    ddlRefPoint.SelectedValue = "0";
                    //}

                    break;
            }

            if (grvprimeaccount.Rows.Count > 0)
                pnlPASADetails.Visible = true;
            else
            {
                pnlPASADetails.Visible = false;
                Utility.FunShowAlertMsg(this.Page, "No " + ddlRefPoint.SelectedItem.Text + " for the selected customer");
                ddlRefPoint.SelectedValue = "0";
            }
        }
        else
        {

            lblDYNRefPoint.CssClass = "styleDisplayLabel";
            ddlDYNRefPoint.ClearSelection();
            ddlDYNRefPoint.Enabled = false;
            lblDYNRefPoint.Text = "Ref Code";
            pnlPASADetails.Visible = false;
            grvprimeaccount.DataSource = null;
            grvprimeaccount.DataBind();
        }
    }
    private DataTable FunPriFillAccountDetails()
    {
        pnlPASADetails.Visible = true;
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@Customer_ID", Convert.ToString(intCustomer));
        if (ddlLOB.SelectedValue != "0")
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        if (ddlBranch.SelectedValue != "0")
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
        if (strMode == "M" || strMode == "Q")
        {
            Procparam.Add("@Collateral_Capture_ID", intCollTranId.ToString());
        }
        DataTable dt;
        dt = Utility.GetDefaultData("S3G_CLT_GETPASADetails", Procparam);
        grvprimeaccount.DataSource = dt;
        grvprimeaccount.DataBind();
        return dt;
    }
    private void FunPubProGetCollateralCaptureDetails()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Collateral_Capture_ID", Convert.ToString(intCollTranId));
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            DataSet ds = new DataSet();
            DataTable dtCustomerData;
            ds = Utility.GetDataset("S3G_CLT_GetCollateralCaptureByID", Procparam);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtTotal.Text = ds.Tables[0].Rows[0]["Total"].ToString();
                rbtlCollCollected.SelectedValue = ds.Tables[0].Rows[0]["Collateral_Collected_By"].ToString();
                FunPriCollateralCollectedBy();
                txtCollTransNo.Text = ds.Tables[0].Rows[0]["Collateral_Tran_No"].ToString();
                txtCollTransDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["Collateral_Tran_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

                ListItem lst = new ListItem(ds.Tables[0].Rows[0]["LOB_Name"].ToString(), ds.Tables[0].Rows[0]["LOB_ID"].ToString());
                ddlLOB.Items.Add(lst);
                ddlLOB.SelectedValue = ds.Tables[0].Rows[0]["LOB_ID"].ToString();
                if (ds.Tables[0].Rows[0]["Type_ID"].ToString() != "0")
                {
                    lst = new ListItem(ds.Tables[0].Rows[0]["Type"].ToString(), ds.Tables[0].Rows[0]["Type_ID"].ToString());
                    ddlType.Items.Add(lst);

                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["Type_ID"].ToString();
                }
                ListItem lst1 = new ListItem(ds.Tables[0].Rows[0]["Product_Name"].ToString(), ds.Tables[0].Rows[0]["Product_ID"].ToString());
                ddlProductCode.Items.Add(lst1);
                ddlProductCode.SelectedValue = ds.Tables[0].Rows[0]["Product_ID"].ToString();

                // ListItem lst2 = new ListItem(ds.Tables[0].Rows[0]["Location"].ToString(), ds.Tables[0].Rows[0]["Location_ID"].ToString());
                //ddlBranch.Items.Add(lst2);
                ddlBranch.SelectedValue = ds.Tables[0].Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = ds.Tables[0].Rows[0]["Location"].ToString();

                ListItem lst3 = new ListItem(ds.Tables[0].Rows[0]["Constitution_Name"].ToString(), ds.Tables[0].Rows[0]["Constitution_ID"].ToString());
                ddlConstitution.Items.Add(lst3);
                ddlConstitution.SelectedValue = ds.Tables[0].Rows[0]["Constitution_ID"].ToString();

                ddlCollectionAgent.SelectedValue = ds.Tables[0].Rows[0]["Collection_Agent_ID"].ToString();

                lst = new ListItem(ds.Tables[0].Rows[0]["Ref_Name"].ToString(), ds.Tables[0].Rows[0]["Ref_Point_Code"].ToString());
                ddlRefPoint.Items.Add(lst);

                ddlRefPoint.SelectedValue = ds.Tables[0].Rows[0]["Ref_Point_Code"].ToString();

                if (ds.Tables[0].Rows[0]["Customer_ID"].ToString() != "")
                {
                    intCustomer = Convert.ToInt32(ds.Tables[0].Rows[0]["Customer_ID"].ToString());
                }
                if (ddlRefPoint.SelectedValue != "0")
                {
                    FunPubFillDynamicRefPointField();
                    ddlDYNRefPoint.SelectedValue = ds.Tables[0].Rows[0]["Ref_Point_ID"].ToString();
                    //if (ddlRefPoint.SelectedValue == "6")
                    //{
                    foreach (GridViewRow grvData in grvprimeaccount.Rows)
                    {
                        CheckBox chkAccount = null;
                        chkAccount = ((CheckBox)grvData.FindControl("chkAccount"));
                        Label lblAccount_ID = ((Label)grvData.FindControl("lblAccount_ID"));

                        DataRow[] DRow = ds.Tables[0].Select("Ref_Point_ID = " + lblAccount_ID.Text);

                        //if (lblAccount_ID.Text == ds.Tables[0].Rows[0]["Ref_Point_ID"].ToString())
                        if (DRow.Length > 0)
                        {
                            chkAccount.Checked = true;
                            chkAccount.Enabled = false;
                        }
                    }
                    //}
                }
                if (PageMode == PageModes.Query)
                {
                    lst = new ListItem(ds.Tables[0].Rows[0]["Currency_Name"].ToString(), ds.Tables[0].Rows[0]["Currency_Code"].ToString());
                    ddlCurrCode.Items.Add(lst);
                }

                ddlCurrCode.SelectedValue = ds.Tables[0].Rows[0]["Currency_Code"].ToString();

                FunPriCollateralDetailsOnLOBandProductCode();
                Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
                Procparam.Add("@Option", Convert.ToString(rbtlCollCollected.SelectedValue));
                if (rbtlCollCollected.SelectedValue == "1")
                    Procparam.Add("@Customer_ID", ds.Tables[0].Rows[0]["Customer_ID"].ToString());
                else if (rbtlCollCollected.SelectedValue == "2")
                    Procparam.Add("@Customer_ID", ds.Tables[0].Rows[0]["Collection_Agent_ID"].ToString());
                dtCustomerData = Utility.GetDefaultData("[S3G_CLT_GetCustomerDetails]", Procparam);
                if (dtCustomerData.Rows.Count > 0)
                {
                    S3GCustomerAddress1.SetCustomerDetails(dtCustomerData.Rows[0], true);
                    txtCustomerCode.Text = dtCustomerData.Rows[0]["Customer_Code"].ToString();
                    TextBox txtCustName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                    txtCustName.Text = dtCustomerData.Rows[0]["Customer_Code"].ToString();
                    Label lblCustCode = (Label)S3GCustomerAddress1.FindControl("lblCustomerCode");
                    Label lblCustName = (Label)S3GCustomerAddress1.FindControl("lblCustomerName");
                    pnlCustDetails.Visible = true;
                    if (rbtlCollCollected.SelectedValue == "1")
                    {
                        lblCustCode.Text = "Customer Code";
                        lblCustName.Text = "Customer Name";
                    }
                    else if (rbtlCollCollected.SelectedValue == "2")
                    {
                        lblCustCode.Text = "Collection Agent Code";
                        lblCustName.Text = "Collection Agent Name";
                    }
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                gvhHighLiqDetails.DataSource = ds.Tables[1];
                gvhHighLiqDetails.DataBind();

                lblHighLiqDetails.Visible = false;
                ViewState[Securities.HighLiquid.ToString()] = ds.Tables[1];
            }
            else
            {
                if (strMode == "Q")
                {
                    lblHighLiqDetails.Visible = true;
                    gvhHighLiqDetails.DataSource = null;
                    gvhHighLiqDetails.DataBind();
                    //    FunPriInitialRow(Securities.HighLiquid);
                    //    gvHighLiqDetails.DataSource = dtSecurities;
                    //    gvHighLiqDetails.DataBind();
                    //    int columnCount = gvHighLiqDetails.Rows[0].Cells.Count;
                    //    gvHighLiqDetails.Rows[0].Cells.Clear();
                    //    gvHighLiqDetails.Rows[0].Cells.Add(new TableCell());
                    //    gvHighLiqDetails.Rows[0].Cells[0].ColumnSpan = columnCount;
                    //    gvHighLiqDetails.HeaderRow.Visible = false;
                    //    gvHighLiqDetails.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    //    gvHighLiqDetails.Rows[0].Style.Add("text-align", "center");
                    //    // gvLowLiqDetails.RowStyle.HorizontalAlign = HorizontalAlign.Center;
                    //    gvHighLiqDetails.Rows[0].Cells[0].Text = "No Records Found.";
                }
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                lblMedLiqDetails.Visible = false;
                gvMedLiqDetails.DataSource = ds.Tables[2];
                gvMedLiqDetails.DataBind();

                gvMMedLiqDetails.DataSource = ds.Tables[2];
                gvMMedLiqDetails.DataBind();

                ViewState[Securities.MediumLiquid.ToString()] = ds.Tables[2];
            }
            else
            {
                if (strMode == "Q")
                {
                    //FunPriInitialRow(Securities.MediumLiquid); 
                    //gvMedLiqDetails.DataBind();
                    //int columnCount = gvMedLiqDetails.Rows[0].Cells.Count;
                    //gvMedLiqDetails.Rows[0].Cells.Clear();
                    //gvMedLiqDetails.Rows[0].Cells.Add(new TableCell());
                    //gvMedLiqDetails.Rows[0].Cells[0].ColumnSpan = columnCount;
                    //gvMedLiqDetails.HeaderRow.Visible = false;
                    //gvMedLiqDetails.Rows[0].Style.Add("text-align", "center");
                    //// gvLowLiqDetails.RowStyle.HorizontalAlign = HorizontalAlign.Center;
                    //gvMedLiqDetails.Rows[0].Cells[0].Text = "No Records Found.";
                    lblMedLiqDetails.Visible = true;
                    gvMedLiqDetails.DataSource = null;
                    gvMedLiqDetails.DataBind();
                }
            }

            if (ds.Tables[3].Rows.Count > 0)
            {
                lblLowLiquidsecurites.Visible = false;
                gvLowLiqDetails.DataSource = ds.Tables[3];
                gvLowLiqDetails.DataBind();

                gvLLowLiqDetails.DataSource = ds.Tables[3];
                gvLLowLiqDetails.DataBind();

                ViewState[Securities.LowLiquid.ToString()] = ds.Tables[3];
            }
            else
            {
                if (strMode == "Q")
                {
                    //FunPriInitialRow(Securities.LowLiquid);
                    //gvLowLiqDetails.DataSource = dtSecurities;
                    //gvLowLiqDetails.DataBind();
                    //int columnCount = gvLowLiqDetails.Rows[0].Cells.Count;
                    //gvLowLiqDetails.Rows[0].Cells.Clear();
                    //gvLowLiqDetails.Rows[0].Cells.Add(new TableCell());
                    //gvLowLiqDetails.Rows[0].Cells[0].ColumnSpan = columnCount;
                    //gvLowLiqDetails.HeaderRow.Visible = false;
                    //gvLowLiqDetails.Rows[0].Cells[0].Text = "No Records Found.";
                    lblLowLiquidsecurites.Visible = true;
                    gvLowLiqDetails.DataSource = null;
                    gvLowLiqDetails.DataBind();
                }
            }
            if (ds.Tables[4].Rows.Count > 0)
            {
                lblCommSecurities.Visible = false;
                gvCommoDetails.DataSource = ds.Tables[4];
                gvCommoDetails.DataBind();

                gvCCommoDetails.DataSource = ds.Tables[4];
                gvCCommoDetails.DataBind();

                ViewState[Securities.Commodities.ToString()] = ds.Tables[4];
            }
            else
            {
                if (strMode == "Q")
                {
                    //FunPriInitialRow(Securities.Commodities);
                    //gvCommoDetails.DataSource = dtSecurities;
                    //gvCommoDetails.DataBind();
                    //int columnCount = gvCommoDetails.Rows[0].Cells.Count;
                    //gvCommoDetails.Rows[0].Cells.Clear();
                    //gvCommoDetails.Rows[0].Cells.Add(new TableCell());
                    //gvCommoDetails.Rows[0].Cells[0].ColumnSpan = columnCount;
                    //gvCommoDetails.HeaderRow.Visible = false;
                    //gvCommoDetails.Rows[0].Style.Add("text-align", "center");
                    //// gvLowLiqDetails.RowStyle.HorizontalAlign = HorizontalAlign.Center;
                    //gvCommoDetails.Rows[0].Cells[0].Text = "No Records Found.";
                    lblCommSecurities.Visible = true;
                    gvCommoDetails.DataSource = null;
                    gvCommoDetails.DataBind();
                }
            }

            if (ds.Tables[5].Rows.Count > 0)
            {
                lblFinSecurities.Visible = false;
                gvFFinDetails.DataSource = ds.Tables[5];
                gvFFinDetails.DataBind();
                ViewState[Securities.Financial.ToString()] = ds.Tables[5];
            }
            else
            {
                if (strMode == "Q")
                {
                    //FunPriInitialRow(Securities.Financial);
                    //gvFinDetails.DataSource = dtSecurities;
                    //gvFinDetails.DataBind();
                    //int columnCount = gvFinDetails.Rows[0].Cells.Count;
                    //gvFinDetails.Rows[0].Cells.Clear();
                    //gvFinDetails.Rows[0].Cells.Add(new TableCell());
                    //gvFinDetails.Rows[0].Cells[0].ColumnSpan = columnCount;
                    //gvFinDetails.HeaderRow.Visible = false;
                    //gvFinDetails.Rows[0].Style.Add("text-align", "center");
                    //// gvLowLiqDetails.RowStyle.HorizontalAlign = HorizontalAlign.Center;
                    //gvFinDetails.Rows[0].Cells[0].Text = "No Records Found.";
                    lblFinSecurities.Visible = true;
                    gvFinDetails.DataSource = null;
                    gvFinDetails.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunPriClearPage()
    {
        //ddlLOB.SelectedIndex = -1;
        //ddlBranch.SelectedIndex = -1;
        //rbtlCollCollected.SelectedValue = "1";


    }

    private void FunPriFillCollateralDescription(string collCode, DropDownList ddlDesc)
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@Collateral_Code", collCode);
        if (ddlLOB.SelectedValue != "0")
            Procparam.Add("@LOB_ID ", ddlLOB.SelectedValue);
        if (ddlProductCode.SelectedValue != "0")
            Procparam.Add("@PRODUCT_ID", ddlProductCode.SelectedValue);
        if (ddlConstitution.SelectedValue != "0")
            Procparam.Add("@Constitution_ID", ddlConstitution.SelectedValue);
        //if (strMode == "C")
        Procparam.Add("@Is_Active", "1");
        ddlDesc.BindDataTable("S3G_CLT_getCollateralDetailsID", Procparam, new string[] { "Collateral_Detail_ID", "Collateral_Securities_Name" });
    }

    private void FunPriCollateralDetailsOnLOBandProductCode()
    {
        FunPriInitialRow(Securities.HighLiquid);
        FunPriInitialRow(Securities.MediumLiquid);
        // FunPriInitialRow(Securities.LowLiquid);
        //FunPriInitialRow(Securities.Commodities);
        //FunPriInitialRow(Securities.Financial);

        //DropDownList ddlDescriptionHIGH = (DropDownList)gvHighLiqDetails.FooterRow.FindControl("ddlCollSecurities");
        //FunPriFillCollateralDescription("1", ddlDescriptionHIGH);
        FunPriFillCollateralDescription("1", ddlhCollSecurities);
        if (ddlhCollSecurities.Items.Count == 1)
        {
            tcCollateralCapture.Tabs[1].Visible = false;
        }
        else
        {
            tcCollateralCapture.Tabs[1].Visible = true;
        }

        //DropDownList ddlDescriptionMEDIUM = (DropDownList)gvMedLiqDetails.FooterRow.FindControl("ddlCollSecurities");
        //FunPriFillCollateralDescription("2", ddlDescriptionMEDIUM);
        FunPriFillCollateralDescription("2", ddlMCollSecurities);
        if (ddlMCollSecurities.Items.Count == 1)
        {
            tcCollateralCapture.Tabs[2].Visible = false;
        }
        else
        {
            tcCollateralCapture.Tabs[2].Visible = true;
        }

        //DropDownList ddlDescriptionLOW = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlCollSecurities");
        //FunPriFillCollateralDescription("3", ddlDescriptionLOW);
        FunPriFillCollateralDescription("3", ddlLCollSecurities);
        if (ddlLCollSecurities.Items.Count == 1)
        {
            tcCollateralCapture.Tabs[3].Visible = false;
        }
        else
        {
            tcCollateralCapture.Tabs[3].Visible = true;
        }

        //DropDownList ddlDescriptionCOMMODITIES = (DropDownList)gvCommoDetails.FooterRow.FindControl("ddlCollSecurities");
        //FunPriFillCollateralDescription("4", ddlDescriptionCOMMODITIES);
        FunPriFillCollateralDescription("4", ddlCCollSecurities);
        if (ddlCCollSecurities.Items.Count == 1)
        {
            tcCollateralCapture.Tabs[4].Visible = false;
        }
        else
        {
            tcCollateralCapture.Tabs[4].Visible = true;
        }

        //DropDownList ddlDescriptionFINANCIAL = (DropDownList)gvFinDetails.FooterRow.FindControl("ddlCollSecurities");
        //FunPriFillCollateralDescription("5", ddlDescriptionFINANCIAL);
        FunPriFillCollateralDescription("5", ddlFCollSecurities);
        if (ddlFCollSecurities.Items.Count == 1)
        {
            tcCollateralCapture.Tabs[5].Visible = false;
        }
        else
        {
            tcCollateralCapture.Tabs[5].Visible = true;
        }
    }
    private void FunPriGenerateXMLDetails(Securities enuSecurities)
    {

        try
        {
            switch (enuSecurities)
            {
                case Securities.HighLiquid:

                    dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
                    if (dtSecurities.Rows.Count == 1 && dtSecurities.Rows[0]["SlNo"].ToString() == "0")
                        strXMLHighLIQSecuritiesDet = "<Root> </Root>";
                    else
                        strXMLHighLIQSecuritiesDet = dtSecurities.FunPubFormXml();
                    //strXMLHighLIQSecuritiesDet = gvHighLiqDetails.FunPubFormXml();
                    break;
                case Securities.MediumLiquid:
                    dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
                    if (dtSecurities.Rows.Count == 1 && dtSecurities.Rows[0]["SlNo"].ToString() == "0")
                        strXMLMediumLIQSecuritiesDet = "<Root> </Root>";
                    else
                        //strXMLMediumLIQSecuritiesDet = gvMedLiqDetails.FunPubFormXml();
                        strXMLMediumLIQSecuritiesDet = dtSecurities.FunPubFormXml();
                    break;
                case Securities.LowLiquid:
                    dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];
                    if (dtSecurities.Rows.Count == 1 && dtSecurities.Rows[0]["SlNo"].ToString() == "0")
                        strXMLLOWLIQSecuritiesDet = "<Root> </Root>";
                    else
                        //strXMLLOWLIQSecuritiesDet = gvLowLiqDetails.FunPubFormXml();
                        strXMLLOWLIQSecuritiesDet = dtSecurities.FunPubFormXml();
                    break;
                case Securities.Commodities:
                    dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];
                    if (dtSecurities.Rows.Count == 1 && dtSecurities.Rows[0]["SlNo"].ToString() == "0")
                        strXMLCommoditiesLIQSecuritiesDet = "<Root> </Root>";
                    else
                        //strXMLCommoditiesLIQSecuritiesDet = gvCommoDetails.FunPubFormXml();
                        strXMLCommoditiesLIQSecuritiesDet = dtSecurities.FunPubFormXml();
                    break;
                case Securities.Financial:
                    dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];
                    if (dtSecurities.Rows.Count == 1 && dtSecurities.Rows[0]["SlNo"].ToString() == "0")
                        strXMLFinancialLIQSecuritiesDet = "<Root> </Root>";
                    else
                        //strXMLFinancialLIQSecuritiesDet = gvFinDetails.FunPubFormXml();
                        strXMLFinancialLIQSecuritiesDet = dtSecurities.FunPubFormXml();
                    break;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
            throw objException;
        }
    }

    private void FunPriSetAlignmentsHighLIQSecurities()
    {
        txthDPNo.Style.Add("text-align", "right");
        txthDPNo.Attributes.Add("onpaste", "return false;");

        txthClientID.Style.Add("text-align", "right");
        txthClientID.Attributes.Add("onpaste", "return false;");

        txthUnitFaceValue.Style.Add("text-align", "right");
        txthUnitFaceValue.Attributes.Add("onpaste", "return false;");

        txthNoOfUnits.Style.Add("text-align", "right");
        txthNoOfUnits.SetDecimalPrefixSuffix(8, 4, false, "No of Units");

        txthInterest.Style.Add("text-align", "right");
        txthInterest.Attributes.Add("onpaste", "return false;");
        txthInterest.SetPercentagePrefixSuffix(2, 2, false, false, "Interst Percentage");

        txthMaturityValue.Style.Add("text-align", "right");
        txthMaturityValue.SetDecimalPrefixSuffix(15, 4, false, "Maturity Value");

        txthMarketRate.Style.Add("text-align", "right");
        txthMarketRate.SetDecimalPrefixSuffix(15, 4, false, "Market Rate");

        txthownership.Style.Add("text-align", "right");
        txthownership.SetDecimalPrefixSuffix(3, 0, false, "Ownership");

        txthMarketValue.Style.Add("text-align", "right");
        //txthMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");

        CEhMaturityDate.Format = strDateFormat;

        txthNoOfUnits.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txthNoOfUnits.ClientID + "','" + txthMarketRate.ClientID + "','" + txthMarketValue.ClientID + "')");
        txthMarketRate.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txthNoOfUnits.ClientID + "','" + txthMarketRate.ClientID + "','" + txthMarketValue.ClientID + "')");
        string s = rfvhUnitFaceValue.ClientID;
        //ddlCollSecurities.Attributes.Add("onchange", "javascript:return fncheckMandatory('" + ddlCollSecurities.ClientID + "','" + rfvIssuedBy.ClientID + "','" + rfvUnitFaceValue.ClientID + "','" + rfvNoOfUnits.ClientID + "','" + rfvInterest.ClientID + "','" + rfvMaturityDate.ClientID + "','" + rfvMaturityValue.ClientID + "')");
        //ddlhDemat.Attributes.Add("onchange", "javascript:return fncheckMandatory1('" + ddlhDemat.ClientID + "','" + txthDPName.ClientID + "','" + txthDPNo.ClientID + "','" + txthClientID.ClientID + "','" + txthCertificateNo.ClientID + "','" + rfvhDPName.ClientID + "','" + rfvhDPNo.ClientID + "','" + rfvhClientID.ClientID + "','" + rfvhCertificateNo.ClientID + "')");

    }

    private void FunPriSetAlignmentsHighLIQSecurities(GridViewRowEventArgs e)
    {
        DropDownList ddlCollSecurities = e.Row.FindControl("ddlCollSecurities") as DropDownList;
        DropDownList ddlDemat = e.Row.FindControl("ddlDemat") as DropDownList;
        RequiredFieldValidator rfvIssuedBy = e.Row.FindControl("rfvIssuedBy") as RequiredFieldValidator;
        RequiredFieldValidator rfvUnitFaceValue = e.Row.FindControl("rfvUnitFaceValue") as RequiredFieldValidator;
        RequiredFieldValidator rfvNoOfUnits = e.Row.FindControl("rfvNoOfUnits") as RequiredFieldValidator;

        RequiredFieldValidator rfvDPName = e.Row.FindControl("rfvDPName") as RequiredFieldValidator;
        RequiredFieldValidator rfvDPNO = e.Row.FindControl("rfvDPNO") as RequiredFieldValidator;
        RequiredFieldValidator rfvClientID = e.Row.FindControl("rfvClientID") as RequiredFieldValidator;
        RequiredFieldValidator rfvCertificateNo = e.Row.FindControl("rfvCertificateNo") as RequiredFieldValidator;
        TextBox txtDPName = e.Row.FindControl("txtDPName") as TextBox;
        TextBox txtCertificateNo = e.Row.FindControl("txtCertificateNo") as TextBox;

        RequiredFieldValidator rfvInterest = e.Row.FindControl("rfvInterest") as RequiredFieldValidator;
        RequiredFieldValidator rfvMaturityDate = e.Row.FindControl("rfvMaturityDate") as RequiredFieldValidator;
        RequiredFieldValidator rfvMaturityValue = e.Row.FindControl("rfvMaturityValue") as RequiredFieldValidator;


        TextBox txtDPNo = e.Row.FindControl("txtDPNo") as TextBox;
        txtDPNo.Style.Add("text-align", "right");
        txtDPNo.Attributes.Add("onpaste", "return false;");

        TextBox txtClientID = e.Row.FindControl("txtClientID") as TextBox;
        txtClientID.Style.Add("text-align", "right");
        txtClientID.Attributes.Add("onpaste", "return false;");

        TextBox txtUnitFaceValue = e.Row.FindControl("txtUnitFaceValue") as TextBox;
        txtUnitFaceValue.Style.Add("text-align", "right");
        txtUnitFaceValue.Attributes.Add("onpaste", "return false;");

        TextBox txtNoOfUnits = e.Row.FindControl("txtNoOfUnits") as TextBox;
        txtNoOfUnits.Style.Add("text-align", "right");
        txtNoOfUnits.SetDecimalPrefixSuffix(8, 4, false, "No of Units");

        TextBox txtInterest = e.Row.FindControl("txtInterest") as TextBox;
        txtInterest.Style.Add("text-align", "right");
        txtInterest.Attributes.Add("onpaste", "return false;");
        txtInterest.SetPercentagePrefixSuffix(2, 2, false, false, "Interst Percentage");

        TextBox txtMaturityValue = e.Row.FindControl("txtMaturityValue") as TextBox;
        txtMaturityValue.Style.Add("text-align", "right");
        txtMaturityValue.SetDecimalPrefixSuffix(15, 4, false, "Maturity Value");

        TextBox txtMarketRate = e.Row.FindControl("txtMarketRate") as TextBox;
        txtMarketRate.Style.Add("text-align", "right");
        txtMarketRate.SetDecimalPrefixSuffix(15, 4, false, "Market Rate");

        TextBox txtMarketValue = e.Row.FindControl("txtMarketValue") as TextBox;
        //txtMarketValue.Style.Add("text-align", "right");
        txtMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");


        AjaxControlToolkit.CalendarExtender CEMaturityDate = e.Row.FindControl("CEMaturityDate") as AjaxControlToolkit.CalendarExtender;
        CEMaturityDate.Format = strDateFormat;

        txtNoOfUnits.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtNoOfUnits.ClientID + "','" + txtMarketRate.ClientID + "','" + txtMarketValue.ClientID + "')");
        txtMarketRate.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtNoOfUnits.ClientID + "','" + txtMarketRate.ClientID + "','" + txtMarketValue.ClientID + "')");
        string s = rfvUnitFaceValue.ClientID;
        //ddlCollSecurities.Attributes.Add("onchange", "javascript:return fncheckMandatory('" + ddlCollSecurities.ClientID + "','" + rfvIssuedBy.ClientID + "','" + rfvUnitFaceValue.ClientID + "','" + rfvNoOfUnits.ClientID + "','" + rfvInterest.ClientID + "','" + rfvMaturityDate.ClientID + "','" + rfvMaturityValue.ClientID + "')");
        ddlDemat.Attributes.Add("onchange", "javascript:return fncheckMandatory1('" + ddlDemat.ClientID + "','" + txtDPName.ClientID + "','" + txtDPNo.ClientID + "','" + txtClientID.ClientID + "','" + txtCertificateNo.ClientID + "','" + rfvDPName.ClientID + "','" + rfvDPNO.ClientID + "','" + rfvClientID.ClientID + "','" + rfvCertificateNo.ClientID + "')");

    }

    private void FunPriSetAlignmentsMediumLIQSecurities()
    {

        txtMYear.Style.Add("text-align", "right");
        txtMYear.Attributes.Add("onpaste", "return false;");
        //txtMYear.SetPercentagePrefixSuffix(3, 0, true, false, " Year ");


        txtMValue.Style.Add("text-align", "right");
        txtMValue.SetDecimalPrefixSuffix(8, 4, false, "Value");

        txtMOwnership.Style.Add("text-align", "right");
        txtMOwnership.SetDecimalPrefixSuffix(3, 0, false, "Ownership_Medium");

        txtMMarketValue.Style.Add("text-align", "right");
        txtMMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");
        ddlMCollSecurities.Attributes.Add("onchange", "javascript:return fncheckMandatoryInMedSec('" + ddlMCollSecurities.ClientID + "','" + rfvMRegistration.ClientID + "','" + rfvMSerialNo.ClientID + "')");
    }

    private void FunPriSetAlignmentsMediumLIQSecurities(GridViewRowEventArgs e)
    {
        DropDownList ddlCollSecurities = e.Row.FindControl("ddlCollSecurities") as DropDownList;
        RequiredFieldValidator rfvRegistration = e.Row.FindControl("rfvRegistration") as RequiredFieldValidator;
        RequiredFieldValidator rfvSerialNo = e.Row.FindControl("rfvSerialNo") as RequiredFieldValidator;

        TextBox txtYear = e.Row.FindControl("txtYear") as TextBox;
        txtYear.Style.Add("text-align", "right");
        txtYear.Attributes.Add("onpaste", "return false;");
        txtYear.SetPercentagePrefixSuffix(4, 0, true, false, " Year ");

        TextBox txtValue = e.Row.FindControl("txtValue") as TextBox;
        txtValue.Style.Add("text-align", "right");
        txtValue.SetDecimalPrefixSuffix(8, 4, false, "Value");

        TextBox txtMarketValue = e.Row.FindControl("txtMarketValue") as TextBox;
        txtMarketValue.Style.Add("text-align", "right");
        txtMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");


        ddlCollSecurities.Attributes.Add("onchange", "javascript:return fncheckMandatoryInMedSec('" + ddlCollSecurities.ClientID + "','" + rfvRegistration.ClientID + "','" + rfvSerialNo.ClientID + "')");

    }
    private void FunPriSetAlignmentsLOWLIQSecurities(GridViewRowEventArgs e)
    {
        TextBox txtMeasurement = e.Row.FindControl("txtMeasurement") as TextBox;
        txtMeasurement.Style.Add("text-align", "right");
        txtMeasurement.SetDecimalPrefixSuffix(8, 4, false, "Measurement");

        TextBox txtUnitRate = e.Row.FindControl("txtUnitRate") as TextBox;
        txtUnitRate.Style.Add("text-align", "right");
        txtUnitRate.SetDecimalPrefixSuffix(8, 4, false, "Unit Rate");

        TextBox txtValue = e.Row.FindControl("txtValue") as TextBox;
        txtValue.Style.Add("text-align", "right");
        //txtValue.SetDecimalPrefixSuffix(8, 4, false, "Value");

        TextBox txtMarketValue = e.Row.FindControl("txtMarketValue") as TextBox;
        txtMarketValue.Style.Add("text-align", "right");
        txtMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");

        txtMeasurement.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtMeasurement.ClientID + "','" + txtUnitRate.ClientID + "','" + txtValue.ClientID + "')");
        txtUnitRate.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtMeasurement.ClientID + "','" + txtUnitRate.ClientID + "','" + txtValue.ClientID + "')");
    }
    private void FunPriSetAlignmentsCommoditiesLIQSecurities(GridViewRowEventArgs e)
    {
        //TextBox txtUnitOfMeasure = e.Row.FindControl("txtUnitOfMeasure") as TextBox;
        //txtUnitOfMeasure.Style.Add("text-align", "right");

        TextBox txtUnitQty = e.Row.FindControl("txtUnitQty") as TextBox;
        txtUnitQty.Style.Add("text-align", "right");

        TextBox txtUnitPrice = e.Row.FindControl("txtUnitPrice") as TextBox;
        txtUnitPrice.Style.Add("text-align", "right");
        txtUnitPrice.SetDecimalPrefixSuffix(8, 4, false, "Unit Price");

        TextBox txtValue = e.Row.FindControl("txtValue") as TextBox;
        txtValue.Style.Add("text-align", "right");

        TextBox txtUnitMarketPrice = e.Row.FindControl("txtUnitMarketPrice") as TextBox;
        txtUnitMarketPrice.Style.Add("text-align", "right");
        txtUnitMarketPrice.SetDecimalPrefixSuffix(8, 4, false, "Unit Market Price");

        AjaxControlToolkit.CalendarExtender CEDate = e.Row.FindControl("CEDate") as AjaxControlToolkit.CalendarExtender;
        CEDate.Format = strDateFormat;

        txtUnitQty.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtUnitQty.ClientID + "','" + txtUnitPrice.ClientID + "','" + txtValue.ClientID + "')");
        txtUnitPrice.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtUnitQty.ClientID + "','" + txtUnitPrice.ClientID + "','" + txtValue.ClientID + "')");
    }
    private void FunPriSetAlignmentsFinancialLIQSecurities(GridViewRowEventArgs e)
    {
        TextBox txtPolicyValue = e.Row.FindControl("txtPolicyValue") as TextBox;
        txtPolicyValue.Style.Add("text-align", "right");
        txtPolicyValue.SetDecimalPrefixSuffix(8, 4, false, "Policy Value");

        TextBox txtCurrentValue = e.Row.FindControl("txtCurrentValue") as TextBox;
        txtCurrentValue.Style.Add("text-align", "right");
        txtCurrentValue.SetDecimalPrefixSuffix(8, 4, false, "Current Value");

        AjaxControlToolkit.CalendarExtender CEMaturityDate = e.Row.FindControl("CEMaturityDate") as AjaxControlToolkit.CalendarExtender;
        CEMaturityDate.Format = strDateFormat;
    }
    public enum Securities
    {
        HighLiquid,
        MediumLiquid,
        LowLiquid,
        Commodities,
        Financial,
        StorageData      // To check the securities are mapped with storage...
    }
    //public bool GVBoolFormat(string val)
    //{
    //    if (val != "")
    //        return Convert.ToBoolean(val);
    //    else
    //        return false;
    //}
    //******* Save Button ********//
    private void FunPriSaveCollateralCapture()
    {
        ObjCollateralClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {
            string strCCNo = "";
            DataTable dtHighLIQSecurity;
            dtHighLIQSecurity = (DataTable)ViewState[Securities.HighLiquid.ToString()];

            ObjS3G_CLT_CollateralCaptureDataTable = new S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureDataTable();
            COLLATERAL.CollateralMgtServices.S3G_CLT_CollateralCaptureRow objS3G_CLT_CollateralCaptureRow;
            objS3G_CLT_CollateralCaptureRow = ObjS3G_CLT_CollateralCaptureDataTable.NewS3G_CLT_CollateralCaptureRow();

            objS3G_CLT_CollateralCaptureRow.Company_ID = intCompanyId;
            //Added by Saranya
            objS3G_CLT_CollateralCaptureRow.Type_ID = (ddlType.SelectedValue != "" && ddlType.SelectedValue != "0") ? Convert.ToInt32(ddlType.SelectedValue) : 0;
            //End
            objS3G_CLT_CollateralCaptureRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);

            //if (ddlBranch.SelectedValue != string.Empty)
            //{
            objS3G_CLT_CollateralCaptureRow.Branch_ID = (ddlBranch.SelectedValue != "" && ddlBranch.SelectedValue != "0") ? Convert.ToInt32(ddlBranch.SelectedValue) : 0;
            //Convert.ToInt32(ddlBranch.SelectedValue);
            //}

            //if (ddlProductCode.Items.Count > 0 && (ddlProductCode.SelectedValue != "" && ddlProductCode.SelectedValue != "0"))
            //{
            objS3G_CLT_CollateralCaptureRow.Product_ID = (ddlProductCode.SelectedValue != "" && ddlProductCode.SelectedValue != "0") ? Convert.ToInt32(ddlProductCode.SelectedValue) : 0;
                    //Convert.ToInt32(ddlProductCode.SelectedValue);
            //}

            if (ddlConstitution.Items.Count > 0 && (ddlConstitution.SelectedValue != "" && ddlConstitution.SelectedValue != "0"))
            {
                objS3G_CLT_CollateralCaptureRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedValue);
            }
            objS3G_CLT_CollateralCaptureRow.Collateral_Collected_By = Convert.ToByte(rbtlCollCollected.SelectedValue);
            if (intCustomer != 0)
            {
                objS3G_CLT_CollateralCaptureRow.Customer_ID = intCustomer;
            }
            objS3G_CLT_CollateralCaptureRow.Total = intTotal.ToString();
            objS3G_CLT_CollateralCaptureRow.Collection_Agent_ID = ddlCollectionAgent.SelectedValue != "" ? Convert.ToInt32(ddlCollectionAgent.SelectedValue) : 0;
            objS3G_CLT_CollateralCaptureRow.Currency_Code = Convert.ToInt32(ddlCurrCode.SelectedValue);
            objS3G_CLT_CollateralCaptureRow.Collateral_Tran_Date = DateTime.Now;

            objS3G_CLT_CollateralCaptureRow.Ref_Point_Type_Code = 4;
            objS3G_CLT_CollateralCaptureRow.Ref_Point_Code = (ddlRefPoint.SelectedValue != "" && ddlRefPoint.SelectedValue != "") ? Convert.ToInt32(ddlRefPoint.SelectedValue) : 0;
            //objS3G_CLT_CollateralCaptureRow.Ref_Point_ID = (ddlDYNRefPoint.SelectedValue != "" && ddlDYNRefPoint.SelectedValue != "0") ? Convert.ToInt32(ddlDYNRefPoint.SelectedValue) : (ddlRefPoint.SelectedValue == "6" ? FunPriGetSelectedAccountID() : 0);


            //if (ddlRefPoint.SelectedValue == "6" && objS3G_CLT_CollateralCaptureRow.Ref_Point_ID == "0")
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Please Select Account");
            //    return;
            //}

            if (ddlRefPoint.SelectedValue != "0")
            {
                if (FunPriGetSelectedAccountID() == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select alteast one " + ddlRefPoint.SelectedItem.Text);
                    return;
                }
            }



            objS3G_CLT_CollateralCaptureRow.Ref_Point_ID = FunPriGetSelectedAccountID();

            objS3G_CLT_CollateralCaptureRow.Is_Active = true;
            objS3G_CLT_CollateralCaptureRow.Collateral_Tran_No = txtCollTransNo.Text;
            objS3G_CLT_CollateralCaptureRow.Collateral_Capture_ID = intCollTranId;

            FunPriGenerateXMLDetails(Securities.HighLiquid);
            objS3G_CLT_CollateralCaptureRow.XML_HighLiqSecDetails = strXMLHighLIQSecuritiesDet;
            FunPriGenerateXMLDetails(Securities.MediumLiquid);
            objS3G_CLT_CollateralCaptureRow.XML_MedLiqSecDetails = strXMLMediumLIQSecuritiesDet;
            FunPriGenerateXMLDetails(Securities.LowLiquid);
            objS3G_CLT_CollateralCaptureRow.XML_LowLiqSecDetails = strXMLLOWLIQSecuritiesDet;
            FunPriGenerateXMLDetails(Securities.Commodities);
            objS3G_CLT_CollateralCaptureRow.XML_CommoditiesDetails = strXMLCommoditiesLIQSecuritiesDet;
            FunPriGenerateXMLDetails(Securities.Financial);
            objS3G_CLT_CollateralCaptureRow.XML_FinancialDetails = strXMLFinancialLIQSecuritiesDet;
            objS3G_CLT_CollateralCaptureRow.Collateral_Valuation_No = "";
            objS3G_CLT_CollateralCaptureRow.Collateral_Valuation_Date = DateTime.Now;
            if (strMode == "M")
                objS3G_CLT_CollateralCaptureRow.Collateral_Capture_ID = intCollTranId;
            else
                objS3G_CLT_CollateralCaptureRow.Collateral_Capture_ID = 0;
            if (strXMLHighLIQSecuritiesDet == "<Root> </Root>" && strXMLMediumLIQSecuritiesDet == "<Root> </Root>" &&
                strXMLLOWLIQSecuritiesDet == "<Root> </Root>" && strXMLCommoditiesLIQSecuritiesDet == "<Root> </Root>" &&
                strXMLFinancialLIQSecuritiesDet == "<Root> </Root>")
            {
                Utility.FunShowAlertMsg(this.Page, "Please enter atleast one Collateral");
                return;
            }
            objS3G_CLT_CollateralCaptureRow.Created_By = intUserId;
            objS3G_CLT_CollateralCaptureRow.Created_On = DateTime.Now;
            objS3G_CLT_CollateralCaptureRow.Modified_By = intUserId;
            objS3G_CLT_CollateralCaptureRow.Modified_On = DateTime.Now;

            ObjS3G_CLT_CollateralCaptureDataTable.AddS3G_CLT_CollateralCaptureRow(objS3G_CLT_CollateralCaptureRow);

            intErrCode = ObjCollateralClient.FunPubCreateCollateralCapture(out strCCNo, SerMode, ClsPubSerialize.Serialize(ObjS3G_CLT_CollateralCaptureDataTable, SerMode));

            if (intErrCode == 0)
            {
                //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                if (intCollTranId > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Collateral Capture details updated successfully - " + txtCollTransNo.Text + "');" + strRedirectPageView, true);
                    return;
                    //Response.Redirect(strRedirectPage);
                }
                else
                {
                    txtCollTransNo.Text = strCCNo;
                    strAlert = "Collateral Capture details added successfully - " + strCCNo;
                    strAlert += @"\n\nWould you like to add one more Collateral Capture details?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    //lblErrorMessage.Text = string.Empty;
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
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to Save the  Collateral Capture Details");
                return;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(objException);
            Utility.FunShowAlertMsg(this.Page, "Unable to Save the  Collateral Capture Details");
            //cvCustomerMaster.IsValid = false;
        }
        finally
        {
            //if (ObjCollateralClient != null)
            ObjCollateralClient.Close();
        }
    }

    //*******  HIGH LIQUID SECURITIES   ***************//
    private void FunPriAddHighLIQSecurities()
    {
        try
        {
            DataRow dr;
            dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
            if (dtSecurities.Rows.Count > 0)
            {
                if (dtSecurities.Rows[0]["SlNo"].ToString() == "0" || dtSecurities.Rows[0]["Collateral_Detail_ID"].ToString() == "")
                {
                    dtSecurities.Rows[0].Delete();
                }
            }
            dr = dtSecurities.NewRow();
            dr["SlNo"] = dtSecurities.Rows.Count + 1;
            dr["Collateral_Detail_ID"] = ddlhCollSecurities.SelectedValue;
            dr["Collateral_Securities"] = ddlhCollSecurities.SelectedItem.Text.Trim();
            dr["Issued_By"] = txthIssuedBy.Text.Trim();
            dr["Demat"] = ddlhDemat.SelectedValue;// == "1" ? true : false;
            dr["DematDesc"] = ddlhDemat.SelectedItem.Text.Trim();
            dr["DP_Name"] = txthDPName.Text.Trim();
            dr["DP_No"] = txthDPNo.Text == "" ? "0" : txthDPNo.Text.Trim();//txtDPNo.Text.Trim();
            dr["Client_ID"] = txthClientID.Text == "" ? "0" : txthClientID.Text.Trim(); // txtClientID.Text.Trim();
            dr["Certificate_No"] = txthCertificateNo.Text.Trim();
            dr["Folio_No"] = txthFolioNo.Text.Trim();
            dr["Unit_Face_Value"] = txthUnitFaceValue.Text.Trim();
            dr["No_Of_Units"] = txthNoOfUnits.Text.Trim();
            dr["Interest_Percentage"] = txthInterest.Text.Trim();
            dr["Maturity_Date"] = txthMaturityDate.Text.Trim();
            dr["Maturity_Value"] = txthMaturityValue.Text.Trim() == "" ? "0" : txthMaturityValue.Text.Trim();
            dr["Market_Rate"] = txthMarketRate.Text.Trim() == "" ? "0" : txthMarketRate.Text.Trim();
            dr["Market_Value"] = txthMarketValue.Text.Trim() == "" ? "0" : txthMarketValue.Text.Trim();
            dr["Collateral_Ref_No"] = txthCollateralRefNo.Text.Trim();
            dr["Scan_Ref_No"] = txthScanRefNo.Text.Trim();
            dr["Collateral_Item_Ref_No"] = txthItemRefNo.Text.Trim();
            dr["Collateral_Tran_No"] = txthTranRefNo.Text.Trim();
            dr["Ownership"] = txthownership.Text.Trim() == "" ? "0" : txthownership.Text.Trim();

            //dr["Is_Release"] = true;
            dtSecurities.Rows.Add(dr);
            gvhHighLiqDetails.DataSource = dtSecurities;
            gvhHighLiqDetails.DataBind();
            ViewState[Securities.HighLiquid.ToString()] = dtSecurities;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunHighLIQSecuritiesGridviewRowcommand(GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {

                dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
                DropDownList ddlCollSecurities = (DropDownList)gvHighLiqDetails.FooterRow.FindControl("ddlCollSecurities");
                TextBox txtIssuedBy = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtIssuedBy");
                DropDownList ddlDemat = (DropDownList)gvHighLiqDetails.FooterRow.FindControl("ddlDemat");
                TextBox txtDPName = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtDPName");
                TextBox txtDPNo = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtDPNo");
                TextBox txtClientID = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtClientID");
                TextBox txtCertificateNo = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtCertificateNo");
                TextBox txtFolioNo = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtFolioNo");
                TextBox txtUnitFaceValue = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtUnitFaceValue");
                TextBox txtNoOfUnits = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtNoOfUnits");
                TextBox txtInterest = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtInterest");
                TextBox txtMaturityDate = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtMaturityDate");
                TextBox txtMaturityValue = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtMaturityValue");
                TextBox txtMarketRate = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtMarketRate");
                TextBox txtMarketValue = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtMarketValue");
                TextBox txtCollateralRefNo = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtCollateralRefNo");
                TextBox txtScanRefNo = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtScanRefNo");
                TextBox txtItemRefNo = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtItemRefNo");
                TextBox txtTranRefNo = (TextBox)gvHighLiqDetails.FooterRow.FindControl("txtTranRefNo");

                if (dtSecurities.Rows.Count > 0)
                {
                    if (dtSecurities.Rows[0]["SlNo"].ToString() == "0" && dtSecurities.Rows[0]["Collateral_Detail_ID"].ToString() == "")
                    {
                        dtSecurities.Rows[0].Delete();
                    }
                }
                dr = dtSecurities.NewRow();
                dr["SlNo"] = gvHighLiqDetails.Rows.Count + 1;
                dr["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
                dr["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text.Trim();
                dr["Issued_By"] = txtIssuedBy.Text.Trim();
                dr["Demat"] = ddlDemat.SelectedValue == "1" ? true : false;
                dr["DematDesc"] = ddlDemat.SelectedItem.Text.Trim();
                dr["DP_Name"] = txtDPName.Text.Trim();
                dr["DP_No"] = txtDPNo.Text == "" ? "0" : txtDPNo.Text.Trim();//txtDPNo.Text.Trim();
                dr["Client_ID"] = txtClientID.Text == "" ? "0" : txtClientID.Text.Trim(); // txtClientID.Text.Trim();
                dr["Certificate_No"] = txtCertificateNo.Text.Trim();
                dr["Folio_No"] = txtFolioNo.Text.Trim();
                dr["Unit_Face_Value"] = txtUnitFaceValue.Text.Trim();
                dr["No_Of_Units"] = txtNoOfUnits.Text.Trim();
                dr["Interest_Percentage"] = txtInterest.Text.Trim();
                dr["Maturity_Date"] = txtMaturityDate.Text.Trim();
                dr["Maturity_Value"] = txtMaturityValue.Text.Trim() == "" ? "0" : txtMaturityValue.Text.Trim();
                dr["Market_Rate"] = txtMarketRate.Text.Trim() == "" ? "0" : txtMarketRate.Text.Trim();
                dr["Market_Value"] = txtMarketValue.Text.Trim() == "" ? "0" : txtMarketValue.Text.Trim();
                dr["Collateral_Ref_No"] = txtCollateralRefNo.Text.Trim();
                dr["Scan_Ref_No"] = txtScanRefNo.Text.Trim();
                dr["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
                dr["Collateral_Tran_No"] = txtCollTransNo.Text.Trim();
                //dr["Is_Release"] = true;
                dtSecurities.Rows.Add(dr);
                gvhHighLiqDetails.DataSource = dtSecurities;
                gvhHighLiqDetails.DataBind();
                ViewState[Securities.HighLiquid.ToString()] = dtSecurities;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunHighLIQSecuritiesGridviewRowDeleting(GridViewDeleteEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
            if (FunIsCollateralSecurityIDISExists(dtSecurities.Rows[e.RowIndex]["Collateral_Item_Ref_No"].ToString()))
            {
                if (hdnValue.Value == "true")
                {
                    ViewState["RowID"] = e.RowIndex;
                    ViewState["CollateralType"] = "H";
                    txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    MPE.Show();
                }
            }
            else
            {
                ViewState["RowID"] = e.RowIndex;
                ViewState["CollateralType"] = "H";
                txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                MPE.Show();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunHighLIQSecuritiesGridViewRowUpdating(GridViewUpdateEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
            GridViewRow gvRow = gvHighLiqDetails.Rows[e.RowIndex];


            DropDownList ddlCollSecurities = (DropDownList)gvRow.FindControl("ddlCollSecurities");
            TextBox txtIssuedBy = (TextBox)gvRow.FindControl("txtIssuedBy");
            DropDownList ddlDemat = (DropDownList)gvRow.FindControl("ddlDemat");
            TextBox txtDPName = (TextBox)gvRow.FindControl("txtDPName");
            TextBox txtDPNo = (TextBox)gvRow.FindControl("txtDPNo");
            TextBox txtClientID = (TextBox)gvRow.FindControl("txtClientID");
            TextBox txtCertificateNo = (TextBox)gvRow.FindControl("txtCertificateNo");
            TextBox txtFolioNo = (TextBox)gvRow.FindControl("txtFolioNo");
            TextBox txtUnitFaceValue = (TextBox)gvRow.FindControl("txtUnitFaceValue");
            TextBox txtNoOfUnits = (TextBox)gvRow.FindControl("txtNoOfUnits");
            TextBox txtInterest = (TextBox)gvRow.FindControl("txtInterest");
            TextBox txtMaturityDate = (TextBox)gvRow.FindControl("txtMaturityDate");
            TextBox txtMaturityValue = (TextBox)gvRow.FindControl("txtMaturityValue");
            TextBox txtMarketRate = (TextBox)gvRow.FindControl("txtMarketRate");
            TextBox txtMarketValue = (TextBox)gvRow.FindControl("txtMarketValue");
            TextBox txtCollateralRefNo = (TextBox)gvRow.FindControl("txtCollateralRefNo");
            TextBox txtScanRefNo = (TextBox)gvRow.FindControl("txtScanRefNo");
            TextBox txtItemRefNo = (TextBox)gvRow.FindControl("txtItemRefNo");
            TextBox txtTranRefNo = (TextBox)gvRow.FindControl("txtTranRefNo");


            DataRow drow = dtSecurities.Rows[e.RowIndex];
            drow.BeginEdit();
            drow["SlNo"] = e.RowIndex + 1;
            drow["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
            drow["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text.Trim();
            drow["Issued_By"] = txtIssuedBy.Text.Trim();
            drow["Demat"] = ddlDemat.SelectedValue == "1" ? true : false;
            drow["DematDesc"] = ddlDemat.SelectedItem.Text.Trim();
            drow["DP_Name"] = txtDPName.Text.Trim();
            drow["DP_No"] = txtDPNo.Text == "" ? "0" : txtDPNo.Text.Trim();//txtDPNo.Text.Trim();
            drow["Client_ID"] = txtClientID.Text == "" ? "0" : txtClientID.Text.Trim(); // txtClientID.Text.Trim();
            drow["Certificate_No"] = txtCertificateNo.Text.Trim();
            drow["Folio_No"] = txtFolioNo.Text.Trim();
            drow["Unit_Face_Value"] = txtUnitFaceValue.Text.Trim() == "" ? "0" : txtUnitFaceValue.Text.Trim();
            drow["No_Of_Units"] = txtNoOfUnits.Text.Trim() == "" ? "0" : txtNoOfUnits.Text.Trim();
            drow["Interest_Percentage"] = txtInterest.Text.Trim() == "" ? "0" : txtInterest.Text.Trim();
            drow["Maturity_Date"] = txtMaturityDate.Text.Trim();
            drow["Maturity_Value"] = txtMaturityValue.Text.Trim() == "" ? "0" : txtMaturityValue.Text.Trim();
            drow["Market_Rate"] = txtMarketRate.Text.Trim() == "" ? "0" : txtMarketRate.Text.Trim();
            drow["Market_Value"] = txtMarketValue.Text.Trim() == "" ? "0" : txtMarketValue.Text.Trim();
            drow["Collateral_Ref_No"] = txtCollateralRefNo.Text.Trim();
            drow["Scan_Ref_No"] = txtScanRefNo.Text.Trim();
            drow["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
            drow["Collateral_Tran_No"] = txtCollTransNo.Text.Trim();
            //drow["Is_Release"] = true;
            drow.EndEdit();
            ViewState[Securities.HighLiquid.ToString()] = dtSecurities;
            gvHighLiqDetails.EditIndex = -1;
            gvHighLiqDetails.ShowFooter = true;
            dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
            //FunPubBindWorkflowSequence(dtWorkflow);
            gvhHighLiqDetails.DataSource = dtSecurities;
            gvhHighLiqDetails.DataBind();
            gvHighLiqDetails.Columns[gvHighLiqDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }


    private void FunHighLIQSecuritiesGridViewRowEditing(GridViewEditEventArgs e)
    {
        try
        {
            gvHighLiqDetails.ShowFooter = false;
            gvHighLiqDetails.EditIndex = e.NewEditIndex;
            dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
            gvHighLiqDetails.DataSource = dtSecurities;
            gvHighLiqDetails.DataBind();
            DropDownList ddlCollSecurities = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("ddlCollSecurities") as DropDownList;
            RequiredFieldValidator rfvIssuedBy = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("rfvIssuedBy") as RequiredFieldValidator;
            RequiredFieldValidator rfvUnitFaceValue = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("rfvUnitFaceValue") as RequiredFieldValidator;
            RequiredFieldValidator rfvNoOfUnits = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("rfvNoOfUnits") as RequiredFieldValidator;

            DropDownList ddlDemat = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("ddlDemat") as DropDownList;
            RequiredFieldValidator rfvDPName = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("rfvDPName") as RequiredFieldValidator;
            RequiredFieldValidator rfvDPNo = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("rfvDPNo") as RequiredFieldValidator;
            RequiredFieldValidator rfvClientID = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("rfvClientID") as RequiredFieldValidator;
            RequiredFieldValidator rfvCertificateNo = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("rfvCertificateNo") as RequiredFieldValidator;

            ddlDemat.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Demat"].ToString() == "True" ? "1" : "0";
            ListItem LICollSecurity = new ListItem(dtSecurities.Rows[e.NewEditIndex]["Collateral_Securities"].ToString(), dtSecurities.Rows[e.NewEditIndex]["Collateral_Detail_ID"].ToString());
            ddlCollSecurities.Items.Insert(0, LICollSecurity);

            if (ddlCollSecurities.SelectedItem.Text.Trim() == "Deposits")
            {
                rfvIssuedBy.Visible = false;
                rfvUnitFaceValue.Visible = false;
                rfvNoOfUnits.Visible = false;
            }
            else
            {
                rfvIssuedBy.Visible = true;
                rfvUnitFaceValue.Visible = true;
                rfvNoOfUnits.Visible = true;
            }
            if (ddlDemat.SelectedItem.Text == "Yes")
            {
                rfvDPName.Visible = true;
                rfvDPNo.Visible = true;
                rfvClientID.Visible = true;
                rfvCertificateNo.Visible = false;
            }
            else if (ddlDemat.SelectedItem.Text == "No")
            {
                rfvDPName.Visible = false;
                rfvDPNo.Visible = false;
                rfvClientID.Visible = false;
                rfvCertificateNo.Visible = true;
            }
            TextBox txtDPNo = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("txtDPNo") as TextBox;
            txtDPNo.Style.Add("text-align", "right");

            TextBox txtClientID = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("txtClientID") as TextBox;
            txtClientID.Style.Add("text-align", "right");

            TextBox txtUnitFaceValue = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("txtUnitFaceValue") as TextBox;
            txtUnitFaceValue.Style.Add("text-align", "right");

            TextBox txtNoOfUnits = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("txtNoOfUnits") as TextBox;
            txtNoOfUnits.Style.Add("text-align", "right");
            txtNoOfUnits.SetDecimalPrefixSuffix(8, 4, false, "No of Units");

            TextBox txtInterest = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("txtInterest") as TextBox;
            txtInterest.Style.Add("text-align", "right");
            txtInterest.SetPercentagePrefixSuffix(2, 2, false, false, "Interst Percentage");

            TextBox txtMaturityValue = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("txtMaturityValue") as TextBox;
            txtMaturityValue.Style.Add("text-align", "right");
            txtMaturityValue.SetDecimalPrefixSuffix(15, 4, false, "Maturity Value");

            TextBox txtMarketRate = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("txtMarketRate") as TextBox;
            txtMarketRate.Style.Add("text-align", "right");
            txtMarketRate.SetDecimalPrefixSuffix(15, 4, false, "Market Rate");

            TextBox txtMarketValue = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("txtMarketValue") as TextBox;
            txtMarketValue.Style.Add("text-align", "right");
            txtMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");
            //txtMarketValue.SetDecimalPrefixSuffix(8, 4, false, "Market Value");

            AjaxControlToolkit.CalendarExtender CEMaturityDate = gvHighLiqDetails.Rows[e.NewEditIndex].FindControl("CEMaturityDate") as AjaxControlToolkit.CalendarExtender;
            CEMaturityDate.Format = strDateFormat;

            txtNoOfUnits.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtNoOfUnits.ClientID + "','" + txtMarketRate.ClientID + "','" + txtMarketValue.ClientID + "')");
            txtMarketRate.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtNoOfUnits.ClientID + "','" + txtMarketRate.ClientID + "','" + txtMarketValue.ClientID + "')");
            //************
            gvHighLiqDetails.Columns[gvHighLiqDetails.Columns.Count - 1].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunHighLIQSecuritiesGridviewRowCancelingEdit(GridViewCancelEditEventArgs e)
    {
        try
        {
            gvHighLiqDetails.EditIndex = -1;
            gvHighLiqDetails.ShowFooter = true;
            gvHighLiqDetails.DataSource = (DataTable)ViewState[Securities.HighLiquid.ToString()];
            gvHighLiqDetails.DataBind();
            gvHighLiqDetails.Columns[gvHighLiqDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunHighLIQSecuritiesGridviewRowDataBound(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlCollSecurities = e.Row.FindControl("ddlCollSecurities") as DropDownList;
                if (ddlCollSecurities != null)
                {
                    FunPriFillCollateralDescription("1", ddlCollSecurities);
                    FunPriSetAlignmentsHighLIQSecurities(e);
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState != DataControlRowState.Edit) // && e.Row.RowState != DataControlRowState.Alternate)
                {
                    Label lblItemRefNo = e.Row.FindControl("lblItemRefNo") as Label;
                    Label lblMode = e.Row.FindControl("lblMode") as Label;
                    LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;
                    if (lblMode != null && !string.IsNullOrEmpty(lblMode.Text))
                    {
                        btnRemove.Enabled = false;
                    }

                    if (lblItemRefNo != null)
                    {
                        if (FunIsCollateralSecurityIDISExists(lblItemRefNo.Text.Trim()))
                        {
                            btnRemove.Attributes.Add("onclick", "javascript:return confirm_delete();");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //*******  MEDIUM LIQUID SECURITIES **************//
    private void FunMediumLIQSecuritiesGridviewRowcommand(GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {

                dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];

                DropDownList ddlCollSecurities = (DropDownList)gvMedLiqDetails.FooterRow.FindControl("ddlCollSecurities");
                TextBox txtDescription = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtDescription");
                TextBox txtModel = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtModel");
                TextBox txtYear = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtYear");
                TextBox txtRegistrationNo = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtRegistrationNo");
                TextBox txtSerialNo = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtSerialNo");
                TextBox txtValue = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtValue");
                TextBox txtMarketValue = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtMarketValue");
                TextBox txtCollateralRefNo = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtCollateralRefNo");
                TextBox txtScanRefNo = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtScanRefNo");
                TextBox txtItemRefNo = (TextBox)gvMedLiqDetails.FooterRow.FindControl("txtItemRefNo");
                for (int i = 0; i < dtSecurities.Rows.Count; i++)
                {
                    DataRow drow = dtSecurities.Rows[i];
                    if (ddlCollSecurities.SelectedItem.Text.Trim() == "Vehicle" && txtRegistrationNo.Text.Trim() == drow["Registration_No"].ToString())
                    {
                        Utility.FunShowAlertMsg(this.Page, "Registration No already existed");
                        txtRegistrationNo.Focus();
                        return;
                    }
                    if (ddlCollSecurities.SelectedItem.Text.Trim() == "Machinery" && txtSerialNo.Text.Trim() == drow["Serial_No"].ToString())
                    {
                        Utility.FunShowAlertMsg(this.Page, "Serial No already existed");
                        txtSerialNo.Focus();
                        return;
                    }
                }
                if (dtSecurities.Rows.Count > 0)
                {
                    if (dtSecurities.Rows[0]["SlNo"].ToString() == "0" && dtSecurities.Rows[0]["Collateral_Detail_ID"].ToString() == "")
                    {
                        dtSecurities.Rows[0].Delete();
                    }
                }
                if (txtRegistrationNo.Text == "" && txtSerialNo.Text == "")
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter Registration No or Serial No");
                    txtRegistrationNo.Focus();
                    return;
                }
                dr = dtSecurities.NewRow();


                dr["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text.Trim();
                dr["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
                dr["Description"] = txtDescription.Text.Trim();
                dr["Model"] = txtModel.Text.Trim();
                dr["Year"] = txtYear.Text.Trim();
                dr["Registration_No"] = txtRegistrationNo.Text.Trim();
                dr["Serial_No"] = txtSerialNo.Text.Trim();
                dr["Value"] = txtValue.Text.Trim() == "" ? "0" : txtValue.Text.Trim();
                dr["Market_Value"] = txtMarketValue.Text.Trim() == "" ? "0" : txtMarketValue.Text.Trim();
                dr["Collateral_Ref_No"] = txtCollateralRefNo.Text.Trim();
                dr["Scan_Ref_No"] = txtScanRefNo.Text.Trim();
                dr["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
                dr["Ownership"] = txtMOwnership.Text.Trim() == "" ? "0" : txtMOwnership.Text.Trim();
                //dr["Is_Release"] = false;
                dtSecurities.Rows.Add(dr);
                gvMedLiqDetails.DataSource = dtSecurities;
                gvMedLiqDetails.DataBind();
                ViewState[Securities.MediumLiquid.ToString()] = dtSecurities;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private bool FunIsCollateralSecurityIDISExists(string CollateralItemRefNo)
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@Collateral_Item_Ref_No", CollateralItemRefNo);
        DataTable dtIsExist = new DataTable();
        dtIsExist = Utility.GetDefaultData("S3G_CLT_IsCollItemRefExistInCollStorage", Procparam);
        if (dtIsExist.Rows.Count > 0)
            return true;
        else
            return false;
    }

    private void FunMediumLIQSecuritiesGridviewRowDeleting(GridViewDeleteEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
            if (FunIsCollateralSecurityIDISExists(dtSecurities.Rows[e.RowIndex]["Collateral_Item_Ref_No"].ToString()))
            {
                if (hdnValue.Value == "true")
                {
                    ViewState["RowID"] = e.RowIndex;
                    ViewState["CollateralType"] = "M";
                    txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    MPE.Show();
                }
            }
            else
            {
                ViewState["RowID"] = e.RowIndex;
                ViewState["CollateralType"] = "M";
                txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                MPE.Show();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunMediumLIQSecuritiesGridviewRowUpdating(GridViewUpdateEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
            GridViewRow gvRow = gvMedLiqDetails.Rows[e.RowIndex];

            DropDownList ddlCollSecurities = (DropDownList)gvRow.FindControl("ddlCollSecurities");
            TextBox txtDescription = (TextBox)gvRow.FindControl("txtDescription");
            TextBox txtModel = (TextBox)gvRow.FindControl("txtModel");
            TextBox txtYear = (TextBox)gvRow.FindControl("txtYear");
            TextBox txtRegistrationNo = (TextBox)gvRow.FindControl("txtRegistrationNo");
            TextBox txtSerialNo = (TextBox)gvRow.FindControl("txtSerialNo");
            TextBox txtValue = (TextBox)gvRow.FindControl("txtValue");
            TextBox txtMarketValue = (TextBox)gvRow.FindControl("txtMarketValue");
            TextBox txtCollateralRefNo = (TextBox)gvRow.FindControl("txtCollateralRefNo");
            TextBox txtScanRefNo = (TextBox)gvRow.FindControl("txtScanRefNo");
            TextBox txtItemRefNo = (TextBox)gvRow.FindControl("txtItemRefNo");
            //CheckBox chkRelease = (CheckBox)gvRow.FindControl("chkRelease");

            if (txtRegistrationNo.Text == "" && txtSerialNo.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter Registration No or Serial No");
                txtRegistrationNo.Focus();
                return;
            }
            for (int i = 0; i < dtSecurities.Rows.Count; i++)
            {
                DataRow dr = dtSecurities.Rows[i];
                if (ddlCollSecurities.SelectedItem.Text.Trim() == "Vehicle" && txtRegistrationNo.Text.Trim() == dr["Registration_No"].ToString())
                {
                    Utility.FunShowAlertMsg(this.Page, "Registration No already existed");
                    txtRegistrationNo.Focus();
                    return;
                }
                else if (ddlCollSecurities.SelectedItem.Text.Trim() == "Machinery" && txtSerialNo.Text.Trim() == dr["Serial_No"].ToString())
                {
                    Utility.FunShowAlertMsg(this.Page, "Serial No already existed");
                    txtSerialNo.Focus();
                    return;
                }
            }
            DataRow drow = dtSecurities.Rows[e.RowIndex];
            drow.BeginEdit();
            drow["SlNo"] = e.RowIndex + 1;
            drow["Description"] = txtDescription.Text.Trim();
            drow["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text.Trim();
            drow["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
            drow["Model"] = txtModel.Text.Trim();
            drow["Year"] = txtYear.Text.Trim();
            drow["Registration_No"] = txtRegistrationNo.Text.Trim();
            drow["Serial_No"] = txtSerialNo.Text.Trim();
            drow["Value"] = txtValue.Text.Trim() == "" ? "0" : txtValue.Text.Trim();
            drow["Market_Value"] = txtMarketValue.Text.Trim() == "" ? "0" : txtMarketValue.Text.Trim();
            drow["Collateral_Ref_No"] = txtCollateralRefNo.Text.Trim();
            drow["Scan_Ref_No"] = txtScanRefNo.Text.Trim();
            drow["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
            drow["Ownership_Medium"] = txtMOwnership.Text.Trim();
            //drow["Is_Release"] = chkRelease.Checked;
            drow.EndEdit();
            ViewState[Securities.MediumLiquid.ToString()] = dtSecurities;
            gvMedLiqDetails.EditIndex = -1;
            gvMedLiqDetails.ShowFooter = true;
            dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
            //FunPubBindWorkflowSequence(dtWorkflow);
            gvMedLiqDetails.DataSource = dtSecurities;
            gvMedLiqDetails.DataBind();
            gvMedLiqDetails.Columns[gvMedLiqDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunMediumLIQSecuritiesGridviewRowEditing(GridViewEditEventArgs e)
    {
        try
        {

            gvMedLiqDetails.EditIndex = e.NewEditIndex;
            gvMedLiqDetails.ShowFooter = false;
            dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
            gvMedLiqDetails.DataSource = dtSecurities;
            gvMedLiqDetails.DataBind();
            DropDownList ddlCollSecurities = gvMedLiqDetails.Rows[e.NewEditIndex].FindControl("ddlCollSecurities") as DropDownList;
            RequiredFieldValidator rfvRegistration = gvMedLiqDetails.Rows[e.NewEditIndex].FindControl("rfvRegistration") as RequiredFieldValidator;
            RequiredFieldValidator rfvSerialNo = gvMedLiqDetails.Rows[e.NewEditIndex].FindControl("rfvSerialNo") as RequiredFieldValidator;

            //FunPriFillCollateralDescription("2", ddlCollSecurities);
            //ddlCollSecurities.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Collateral_Detail_ID"].ToString();
            ListItem LICollSecurity = new ListItem(dtSecurities.Rows[e.NewEditIndex]["Collateral_Securities"].ToString(), dtSecurities.Rows[e.NewEditIndex]["Collateral_Detail_ID"].ToString());
            ddlCollSecurities.Items.Insert(0, LICollSecurity);

            TextBox txtYear = gvMedLiqDetails.Rows[e.NewEditIndex].FindControl("txtYear") as TextBox;
            txtYear.Style.Add("text-align", "right");

            TextBox txtValue = gvMedLiqDetails.Rows[e.NewEditIndex].FindControl("txtValue") as TextBox;
            txtValue.Style.Add("text-align", "right");
            txtValue.SetDecimalPrefixSuffix(8, 4, false, "Value");

            TextBox txtMarketValue = gvMedLiqDetails.Rows[e.NewEditIndex].FindControl("txtMarketValue") as TextBox;
            txtMarketValue.Style.Add("text-align", "right");
            txtMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");
            if (ddlCollSecurities.SelectedItem.Text.Trim() == "Vehicles")
            {
                rfvRegistration.Visible = true;
                rfvSerialNo.Visible = false;
            }
            else if (ddlCollSecurities.SelectedItem.Text.Trim() == "Machinery")
            {
                rfvRegistration.Visible = false;
                rfvSerialNo.Visible = true;
            }
            //ddlCollSecurities.Attributes.Add("onchange", "javascript:return fncheckMandatoryInMedSec('" + ddlCollSecurities.ClientID + "','" + rfvRegistration.ClientID + "','" + rfvSerialNo.ClientID + "')");

            gvMedLiqDetails.Columns[gvMedLiqDetails.Columns.Count - 1].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunMediumLIQSecuritiesGridviewRowCancelingEdit(GridViewCancelEditEventArgs e)
    {
        try
        {
            gvMedLiqDetails.EditIndex = -1;
            gvMedLiqDetails.ShowFooter = true;
            gvMedLiqDetails.DataSource = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
            gvMedLiqDetails.DataBind();
            gvMedLiqDetails.Columns[gvMedLiqDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunMediumLIQSecuritiesGridviewRowDataBound(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlCollSecurities = e.Row.FindControl("ddlCollSecurities") as DropDownList;
                if (ddlCollSecurities != null)
                {
                    FunPriFillCollateralDescription("2", ddlCollSecurities);
                    FunPriSetAlignmentsMediumLIQSecurities(e);
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState != DataControlRowState.Edit)
                {
                    Label lblItemRefNo = e.Row.FindControl("lblItemRefNo") as Label;
                    Label lblMode = e.Row.FindControl("lblMode") as Label;
                    LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;
                    if (lblMode != null && !string.IsNullOrEmpty(lblMode.Text))
                    {
                        btnRemove.Enabled = false;
                    }
                    if (lblItemRefNo != null)
                    {
                        if (FunIsCollateralSecurityIDISExists(lblItemRefNo.Text.Trim()))
                        {
                            btnRemove.Attributes.Add("onclick", "javascript:return confirm_delete();");
                        }
                    }
                }
                //Label lblMarketValue = e.Row.FindControl("lblMarketValue") as Label;
                //Label lblValue = e.Row.FindControl("lblValue") as Label;
                ////lblMarketValue.Text = lblMarketValue.Text.ToString(Funsetsuffix);
                //lblValue.Text = ((Convert.ToInt32(lblValue.Text)) * d1).ToString(Funsetsuffix);
                //if (lblMarketValue != null)
                //{
                //    if (!string.IsNullOrEmpty(lblMarketValue.Text))
                //    {
                //        d1 = Convert.ToDecimal(lblMarketValue.Text);
                //        lblMarketValue.Text = d1.ToString(Funsetsuffix());
                //    }
                //}
                //if (lblValue != null)
                //{
                //    if (!string.IsNullOrEmpty(lblValue.Text))
                //    {
                //        d1 = Convert.ToDecimal(lblValue.Text);
                //        lblValue.Text = d1.ToString(Funsetsuffix());
                //    }
                //}
            }
            //if (e.Row.RowType == DataControlRowType.DataRow && bolMediumLiqSecEditEvent == true)
            //{
            //    FunPriSetAlignmentsMediumLIQSecurities(e);
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //*******  LOW LIQUID SECURITIES    **************//
    private void FunLOWLIQSecuritiesGridviewRowcommand(GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {

                DataTable dt = (DataTable)ViewState[Securities.LowLiquid.ToString()];
                DropDownList ddlCollSecurities = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlCollSecurities");
                TextBox txtLocationDetails = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtLocationDetails");
                TextBox txtMeasurement = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtMeasurement");
                TextBox txtUnitRate = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtUnitRate");
                TextBox txtValue = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtValue");
                TextBox txtMarketValue = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtMarketValue");
                TextBox txtCollateralRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtCollateralRefNo");
                TextBox txtScanRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtScanRefNo");
                DropDownList ddlLegalOpinion = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlLegalOpinion");
                TextBox txtLegalScanRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtLegalScanRefNo");
                DropDownList ddlEncumbrance = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlEncumbrance");
                TextBox txtEncumbranceScanRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtEncumbranceScanRefNo");
                DropDownList ddlAssetDocument = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlAssetDocument");
                TextBox txtAssetDocScanRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtAssetDocScanRefNo");
                DropDownList ddlValuationCertificate = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlValuationCertificate");
                TextBox txtValCertificationScanRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtValCertificationScanRefNo");
                TextBox txtItemRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtItemRefNo");

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SlNo"].ToString() == "0" && dt.Rows[0]["Collateral_Detail_ID"].ToString() == "")
                    {
                        dt.Rows[0].Delete();
                    }
                }

                dr = dt.NewRow();
                dr["SlNo"] = gvLowLiqDetails.Rows.Count + 1;
                dr["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text;
                dr["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
                dr["Location_Details"] = txtLocationDetails.Text.Trim();
                dr["Measurement"] = txtMeasurement.Text.Trim() == "" ? "0" : txtMeasurement.Text.Trim();
                dr["Unit_Rate"] = txtUnitRate.Text.Trim() == "" ? "0" : txtUnitRate.Text.Trim();
                dr["Value"] = txtValue.Text.Trim() == "" ? "0" : txtValue.Text.Trim();
                dr["Market_Value"] = txtMarketValue.Text.Trim() == "" ? "0" : txtMarketValue.Text.Trim();
                dr["Collateral_Ref_No"] = txtCollateralRefNo.Text.Trim();
                dr["Scan_Ref_No"] = txtScanRefNo.Text.Trim();
                dr["Legal_Opinion"] = ddlLegalOpinion.SelectedValue == "1" ? true : false;
                dr["Legal_OpinionDesc"] = ddlLegalOpinion.SelectedItem.Text.Trim();
                dr["Legal_Scan_Ref_No"] = txtLegalScanRefNo.Text.Trim();
                dr["Encumbrance"] = ddlEncumbrance.SelectedValue == "1" ? true : false;
                dr["EncumbranceDesc"] = ddlEncumbrance.SelectedItem.Text.Trim();
                dr["Encumbrance_Scan_Ref_No"] = txtEncumbranceScanRefNo.Text.Trim();
                dr["Is_Asset_Document"] = ddlAssetDocument.SelectedValue == "1" ? true : false;
                dr["Is_Asset_DocumentDesc"] = ddlAssetDocument.SelectedItem.Text.Trim();
                dr["AssetDoc_Scan_Ref_No"] = txtAssetDocScanRefNo.Text.Trim();
                dr["Is_Valuation_Certification"] = ddlValuationCertificate.SelectedValue == "1" ? true : false;
                dr["Is_Valuation_CertificationDesc"] = ddlValuationCertificate.SelectedItem.Text.Trim();
                dr["ValCertification_Scan_Ref_No"] = txtValCertificationScanRefNo.Text.Trim();
                dr["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
                dr["Ownership_Low"] = txtLOwnership.Text.Trim() == "" ? "0" : txtLOwnership.Text.Trim();
                //dr["Is_Release"] = true;
                dt.Rows.Add(dr);
                gvLowLiqDetails.DataSource = dt;
                gvLowLiqDetails.DataBind();
                ViewState[Securities.LowLiquid.ToString()] = dt;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunLOWLIQSecuritiesGridviewRowDeleting(GridViewDeleteEventArgs e)
    {
        try
        {

            dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];
            if (FunIsCollateralSecurityIDISExists(dtSecurities.Rows[e.RowIndex]["Collateral_Item_Ref_No"].ToString()))
            {
                if (hdnValue.Value == "true")
                {
                    ViewState["RowID"] = e.RowIndex;
                    ViewState["CollateralType"] = "L";
                    txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    MPE.Show();
                }
            }
            else
            {
                ViewState["RowID"] = e.RowIndex;
                ViewState["CollateralType"] = "L";
                txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                MPE.Show();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunLOWLIQSecuritiesGridviewRowUpdating(GridViewUpdateEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];
            GridViewRow gvRow = gvLowLiqDetails.Rows[e.RowIndex];


            DropDownList ddlCollSecurities = (DropDownList)gvRow.FindControl("ddlCollSecurities");
            TextBox txtLocationDetails = (TextBox)gvRow.FindControl("txtLocationDetails");
            TextBox txtMeasurement = (TextBox)gvRow.FindControl("txtMeasurement");
            TextBox txtUnitRate = (TextBox)gvRow.FindControl("txtUnitRate");
            TextBox txtValue = (TextBox)gvRow.FindControl("txtValue");
            TextBox txtMarketValue = (TextBox)gvRow.FindControl("txtMarketValue");
            TextBox txtCollateralRefNo = (TextBox)gvRow.FindControl("txtCollateralRefNo");
            TextBox txtScanRefNo = (TextBox)gvRow.FindControl("txtScanRefNo");
            DropDownList ddlLegalOpinion = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlLegalOpinion");
            TextBox txtLegalScanRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtLegalScanRefNo");
            DropDownList ddlEncumbrance = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlEncumbrance");
            TextBox txtEncumbranceScanRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtEncumbranceScanRefNo");
            DropDownList ddlAssetDocument = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlAssetDocument");
            TextBox txtAssetDocScanRefNo = (TextBox)gvLowLiqDetails.FooterRow.FindControl("txtAssetDocScanRefNo");
            DropDownList ddlValuationCertificate = (DropDownList)gvLowLiqDetails.FooterRow.FindControl("ddlValuationCertificate");
            TextBox txtValCertificationScanRefNo = (TextBox)gvRow.FindControl("txtValCertificationScanRefNo");
            TextBox txtItemRefNo = (TextBox)gvRow.FindControl("txtItemRefNo");


            DataRow drow = dtSecurities.Rows[e.RowIndex];
            drow.BeginEdit();
            drow["SlNo"] = e.RowIndex + 1;
            drow["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text;
            drow["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
            drow["Location_Details"] = txtLocationDetails.Text.Trim();
            drow["Measurement"] = txtMeasurement.Text.Trim() == "" ? "0" : txtMeasurement.Text.Trim();
            drow["Unit_Rate"] = txtUnitRate.Text.Trim() == "" ? "0" : txtUnitRate.Text.Trim();
            drow["Value"] = txtValue.Text.Trim() == "" ? "0" : txtValue.Text.Trim();
            drow["Market_Value"] = txtMarketValue.Text.Trim() == "" ? "0" : txtMarketValue.Text.Trim();
            drow["Collateral_Ref_No"] = txtCollateralRefNo.Text.Trim();
            drow["Scan_Ref_No"] = txtScanRefNo.Text.Trim();
            drow["Legal_Opinion"] = ddlLegalOpinion.SelectedValue == "1" ? true : false;
            drow["Legal_OpinionDesc"] = ddlLegalOpinion.SelectedItem.Text.Trim();
            drow["Legal_Scan_Ref_No"] = txtLegalScanRefNo.Text.Trim();
            drow["Encumbrance"] = ddlEncumbrance.SelectedValue == "1" ? true : false;
            drow["EncumbranceDesc"] = ddlEncumbrance.SelectedItem.Text.Trim();
            drow["Encumbrance_Scan_Ref_No"] = txtEncumbranceScanRefNo.Text.Trim();
            drow["Is_Asset_Document"] = ddlAssetDocument.SelectedValue == "1" ? true : false;
            drow["Is_Asset_DocumentDesc"] = ddlAssetDocument.SelectedItem.Text.Trim();
            drow["AssetDoc_Scan_Ref_No"] = txtAssetDocScanRefNo.Text.Trim();
            drow["Is_Valuation_Certification"] = ddlValuationCertificate.SelectedValue == "1" ? true : false;
            drow["Is_Valuation_CertificationDesc"] = ddlValuationCertificate.SelectedItem.Text.Trim();
            drow["ValCertification_Scan_Ref_No"] = txtValCertificationScanRefNo.Text.Trim();
            drow["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
            drow["Ownership_Low"] = txtLOwnership.Text.Trim() == "" ? "0" : txtLOwnership.Text.Trim();
            //drow["Is_Release"] = true;
            drow.EndEdit();
            ViewState[Securities.LowLiquid.ToString()] = dtSecurities;
            gvLowLiqDetails.EditIndex = -1;
            gvLowLiqDetails.ShowFooter = true;
            dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];
            gvLowLiqDetails.DataSource = (DataTable)ViewState[Securities.LowLiquid.ToString()];
            gvLowLiqDetails.DataBind();
            gvLowLiqDetails.Columns[gvLowLiqDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunLOWLIQSecuritiesGridviewRowEditing(GridViewEditEventArgs e)
    {
        try
        {
            gvLowLiqDetails.ShowFooter = false;
            gvLowLiqDetails.EditIndex = e.NewEditIndex;
            dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];
            gvLowLiqDetails.DataSource = dtSecurities;
            gvLowLiqDetails.DataBind();
            DropDownList ddlCollSecurities = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("ddlCollSecurities") as DropDownList;
            //FunPriFillCollateralDescription("3", ddlCollSecurities);
            ListItem LICollSecurity = new ListItem(dtSecurities.Rows[e.NewEditIndex]["Collateral_Securities"].ToString(), dtSecurities.Rows[e.NewEditIndex]["Collateral_Detail_ID"].ToString());
            ddlCollSecurities.Items.Insert(0, LICollSecurity);


            DropDownList ddlLegalOpinion = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("ddlLegalOpinion") as DropDownList;

            DropDownList ddlEncumbrance = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("ddlEncumbrance") as DropDownList;

            DropDownList ddlAssetDocument = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("ddlAssetDocument") as DropDownList;

            DropDownList ddlValuationCertificate = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("ddlValuationCertificate") as DropDownList;

            ddlLegalOpinion.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Legal_Opinion"].ToString() == "True" ? "1" : "0";
            ddlAssetDocument.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Is_Asset_Document"].ToString() == "True" ? "1" : "0";
            ddlEncumbrance.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Encumbrance"].ToString() == "True" ? "1" : "0";
            ddlValuationCertificate.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Is_Valuation_Certification"].ToString() == "True" ? "1" : "0";


            TextBox txtMeasurement = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("txtMeasurement") as TextBox;
            txtMeasurement.Style.Add("text-align", "right");
            txtMeasurement.SetDecimalPrefixSuffix(8, 4, false, "Measurement");

            TextBox txtUnitRate = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("txtUnitRate") as TextBox;
            txtUnitRate.Style.Add("text-align", "right");
            txtUnitRate.SetDecimalPrefixSuffix(8, 4, false, "Unit Rate");

            TextBox txtValue = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("txtValue") as TextBox;
            txtValue.Style.Add("text-align", "right");
            //txtValue.SetDecimalPrefixSuffix(8, 4, false, "Value");

            TextBox txtMarketValue = gvLowLiqDetails.Rows[e.NewEditIndex].FindControl("txtMarketValue") as TextBox;
            txtMarketValue.Style.Add("text-align", "right");
            txtMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");

            txtMeasurement.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtMeasurement.ClientID + "','" + txtUnitRate.ClientID + "','" + txtValue.ClientID + "')");
            txtUnitRate.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtMeasurement.ClientID + "','" + txtUnitRate.ClientID + "','" + txtValue.ClientID + "')");
            gvLowLiqDetails.Columns[gvLowLiqDetails.Columns.Count - 1].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunLOWLIQSecuritiesGridviewRowCancelingEdit(GridViewCancelEditEventArgs e)
    {
        try
        {
            gvLowLiqDetails.EditIndex = -1;
            gvLowLiqDetails.ShowFooter = true;
            gvLowLiqDetails.DataSource = (DataTable)ViewState[Securities.LowLiquid.ToString()];
            gvLowLiqDetails.DataBind();
            gvLowLiqDetails.Columns[gvLowLiqDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunLOWLIQSecuritiesGridviewRowDataBound(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlCollSecurities = e.Row.FindControl("ddlCollSecurities") as DropDownList;
                if (ddlCollSecurities != null)
                {
                    FunPriFillCollateralDescription("3", ddlCollSecurities);
                    FunPriSetAlignmentsLOWLIQSecurities(e);
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState != DataControlRowState.Edit)
                {
                    Label lblItemRefNo = e.Row.FindControl("lblItemRefNo") as Label;
                    Label lblMode = e.Row.FindControl("lblMode") as Label;
                    LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;
                    if (lblMode != null && !string.IsNullOrEmpty(lblMode.Text))
                    {
                        btnRemove.Enabled = false;
                    }
                    if (lblItemRefNo != null)
                    {
                        if (FunIsCollateralSecurityIDISExists(lblItemRefNo.Text.Trim()))
                        {
                            btnRemove.Attributes.Add("onclick", "javascript:return confirm_delete();");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //*******  COMMODITIES LIQUID SECURITIES  ********//
    private void FunCommoditiesLIQSecuritiesGridviewRowcommand(GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {

                dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];

                DropDownList ddlCollSecurities = (DropDownList)gvCommoDetails.FooterRow.FindControl("ddlCollSecurities");
                TextBox txtDescription = (TextBox)gvCommoDetails.FooterRow.FindControl("txtDescription");
                DropDownList ddlUnitOfMeasure = (DropDownList)gvCommoDetails.FooterRow.FindControl("ddlUnitOfMeasure");
                TextBox txtUnitQty = (TextBox)gvCommoDetails.FooterRow.FindControl("txtUnitQty");
                TextBox txtUnitPrice = (TextBox)gvCommoDetails.FooterRow.FindControl("txtUnitPrice");
                TextBox txtValue = (TextBox)gvCommoDetails.FooterRow.FindControl("txtValue");
                TextBox txtUnitMarketPrice = (TextBox)gvCommoDetails.FooterRow.FindControl("txtUnitMarketPrice");
                TextBox txtDate = (TextBox)gvCommoDetails.FooterRow.FindControl("txtDate");
                TextBox txtItemRefNo = (TextBox)gvCommoDetails.FooterRow.FindControl("txtItemRefNo");

                if (dtSecurities.Rows.Count > 0)
                {
                    if (dtSecurities.Rows[0]["SlNo"].ToString() == "0" && dtSecurities.Rows[0]["Collateral_Detail_ID"].ToString() == "")
                    {
                        dtSecurities.Rows[0].Delete();
                    }
                }
                dr = dtSecurities.NewRow();
                dr["SlNo"] = gvCommoDetails.Rows.Count + 1;
                dr["Description"] = txtDescription.Text.Trim();
                dr["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
                dr["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text.Trim();
                dr["Unit_Of_Measure"] = ddlUnitOfMeasure.SelectedValue;
                dr["UOM_Text"] = ddlUnitOfMeasure.SelectedItem.Text.Trim();
                dr["Unit_Quantity"] = txtUnitQty.Text.Trim();
                dr["Unit_Price"] = txtUnitPrice.Text.Trim() == "" ? "0" : txtUnitPrice.Text.Trim();
                dr["Value"] = txtValue.Text.Trim() == "" ? "0" : txtValue.Text.Trim();
                dr["Unit_Market_Price"] = txtUnitMarketPrice.Text.Trim() == "" ? "0" : txtUnitMarketPrice.Text.Trim();
                dr["Date"] = txtDate.Text.Trim();
                dr["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
                dr["Ownership_Comm"] = txtCOwnership.Text.Trim();
                //dr["Is_Release"] = true;


                dtSecurities.Rows.Add(dr);
                gvCommoDetails.DataSource = dtSecurities;
                gvCommoDetails.DataBind();
                ViewState[Securities.Commodities.ToString()] = dtSecurities;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunCommoditiesLIQSecuritiesGridviewRowDeleting(GridViewDeleteEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];
            if (FunIsCollateralSecurityIDISExists(dtSecurities.Rows[e.RowIndex]["Collateral_Item_Ref_No"].ToString()))
            {
                if (hdnValue.Value == "true")
                {
                    ViewState["RowID"] = e.RowIndex;
                    ViewState["CollateralType"] = "C";
                    txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    MPE.Show();
                }
            }
            else
            {
                ViewState["RowID"] = e.RowIndex;
                ViewState["CollateralType"] = "C";
                txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                MPE.Show();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunCommoditiesLIQSecuritiesGridviewRowUpdating(GridViewUpdateEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];
            GridViewRow gvRow = gvCommoDetails.Rows[e.RowIndex];


            DropDownList ddlCollSecurities = (DropDownList)gvRow.FindControl("ddlCollSecurities");
            TextBox txtDescription = (TextBox)gvRow.FindControl("txtDescription");
            DropDownList ddlUnitOfMeasure = (DropDownList)gvRow.FindControl("ddlUnitOfMeasure");
            TextBox txtUnitQty = (TextBox)gvRow.FindControl("txtUnitQty");
            TextBox txtUnitPrice = (TextBox)gvRow.FindControl("txtUnitPrice");
            TextBox txtValue = (TextBox)gvRow.FindControl("txtValue");
            TextBox txtUnitMarketPrice = (TextBox)gvRow.FindControl("txtUnitMarketPrice");
            TextBox txtDate = (TextBox)gvRow.FindControl("txtDate");
            TextBox txtItemRefNo = (TextBox)gvRow.FindControl("txtItemRefNo");

            DataRow drow = dtSecurities.Rows[e.RowIndex];
            drow.BeginEdit();
            drow["SlNo"] = e.RowIndex + 1;
            drow["Description"] = txtDescription.Text.Trim();
            drow["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text.Trim();
            drow["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
            drow["Unit_Of_Measure"] = ddlUnitOfMeasure.SelectedValue;
            drow["UOM_Text"] = ddlUnitOfMeasure.SelectedItem.Text.Trim();
            drow["Unit_Quantity"] = txtUnitQty.Text.Trim();
            drow["Unit_Price"] = txtUnitPrice.Text.Trim() == "" ? "0" : txtUnitPrice.Text.Trim();
            drow["Value"] = txtValue.Text.Trim() == "" ? "0" : txtValue.Text.Trim();
            drow["Unit_Market_Price"] = txtUnitMarketPrice.Text.Trim() == "" ? "0" : txtUnitMarketPrice.Text.Trim();
            drow["Date"] = txtDate.Text.Trim();
            drow["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
            drow["Ownership_Comm"] = txtCOwnership.Text.Trim() == "" ? "0" : txtCOwnership.Text.Trim();
            //drow["Is_Release"] = true;
            drow.EndEdit();
            ViewState[Securities.Commodities.ToString()] = dtSecurities;
            gvCommoDetails.EditIndex = -1;
            gvCommoDetails.ShowFooter = true;
            dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];
            gvCommoDetails.DataSource = dtSecurities;
            gvCommoDetails.DataBind();
            gvCommoDetails.Columns[gvCommoDetails.Columns.Count - 1].Visible = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunCommoditiesLIQSecuritiesGridviewRowEditing(GridViewEditEventArgs e)
    {
        try
        {
            gvCommoDetails.ShowFooter = false;
            gvCommoDetails.EditIndex = e.NewEditIndex;
            dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];
            gvCommoDetails.DataSource = dtSecurities;
            gvCommoDetails.DataBind();
            DropDownList ddlCollSecurities = gvCommoDetails.Rows[e.NewEditIndex].FindControl("ddlCollSecurities") as DropDownList;
            //FunPriFillCollateralDescription("4", ddlCollSecurities);
            //ddlCollSecurities.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Collateral_Detail_ID"].ToString();
            ListItem LICollSecurity = new ListItem(dtSecurities.Rows[e.NewEditIndex]["Collateral_Securities"].ToString(), dtSecurities.Rows[e.NewEditIndex]["Collateral_Detail_ID"].ToString());
            ddlCollSecurities.Items.Insert(0, LICollSecurity);


            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            /***** LOAD Unit of Measure *************/
            Procparam.Add("@OPTION", "15");
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@UserID", Convert.ToString(intUserId));
            DropDownList ddlUnitOfMeasure = gvCommoDetails.Rows[e.NewEditIndex].FindControl("ddlUnitOfMeasure") as DropDownList;
            ddlUnitOfMeasure.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "LOOKUP_CODE", "LOOKUP_DESCRIPTION" });
            ddlUnitOfMeasure.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Unit_Of_Measure"].ToString();

            TextBox txtUnitQty = gvCommoDetails.Rows[e.NewEditIndex].FindControl("txtUnitQty") as TextBox;
            txtUnitQty.Style.Add("text-align", "right");

            TextBox txtUnitPrice = gvCommoDetails.Rows[e.NewEditIndex].FindControl("txtUnitPrice") as TextBox;
            txtUnitPrice.Style.Add("text-align", "right");
            txtUnitPrice.SetDecimalPrefixSuffix(8, 4, false, "Unit Price");

            TextBox txtValue = gvCommoDetails.Rows[e.NewEditIndex].FindControl("txtValue") as TextBox;
            txtValue.Style.Add("text-align", "right");

            TextBox txtUnitMarketPrice = gvCommoDetails.Rows[e.NewEditIndex].FindControl("txtUnitMarketPrice") as TextBox;
            txtUnitMarketPrice.Style.Add("text-align", "right");
            txtUnitMarketPrice.SetDecimalPrefixSuffix(8, 4, false, "Unit Market Price");

            AjaxControlToolkit.CalendarExtender CEDate = gvCommoDetails.Rows[e.NewEditIndex].FindControl("CEDate") as AjaxControlToolkit.CalendarExtender;
            CEDate.Format = strDateFormat;

            txtUnitQty.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtUnitQty.ClientID + "','" + txtUnitPrice.ClientID + "','" + txtValue.ClientID + "')");
            txtUnitPrice.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtUnitQty.ClientID + "','" + txtUnitPrice.ClientID + "','" + txtValue.ClientID + "')");



            gvCommoDetails.Columns[gvCommoDetails.Columns.Count - 1].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunCommoditiesLIQSecuritiesGridviewRowCancelingEdit(GridViewCancelEditEventArgs e)
    {
        try
        {
            gvCommoDetails.EditIndex = -1;
            gvCommoDetails.ShowFooter = true;
            gvCommoDetails.DataSource = (DataTable)ViewState[Securities.Commodities.ToString()];
            gvCommoDetails.DataBind();
            gvCommoDetails.Columns[gvCommoDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunCommoditiesLIQSecuritiesGridviewRowDataBound(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlCollSecurities = e.Row.FindControl("ddlCollSecurities") as DropDownList;
                if (ddlCollSecurities != null)
                {
                    FunPriFillCollateralDescription("4", ddlCollSecurities);
                    FunPriSetAlignmentsCommoditiesLIQSecurities(e);
                }

                if (Procparam != null)
                    Procparam.Clear();
                else
                    Procparam = new Dictionary<string, string>();
                /***** LOAD Unit of Measure *************/
                Procparam.Add("@OPTION", "15");
                Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
                Procparam.Add("@UserID", Convert.ToString(intUserId));
                DropDownList ddlUnitOfMeasure = e.Row.FindControl("ddlUnitOfMeasure") as DropDownList;
                if (ddlUnitOfMeasure != null)
                {
                    ddlUnitOfMeasure.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "LOOKUP_CODE", "LOOKUP_DESCRIPTION" });
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMode = e.Row.FindControl("lblMode") as Label;
                LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;
                if (lblMode != null && !string.IsNullOrEmpty(lblMode.Text))
                {
                    btnRemove.Enabled = false;
                }

                if (e.Row.RowState != DataControlRowState.Edit)
                {
                    Label lblItemRefNo = e.Row.FindControl("lblItemRefNo") as Label;
                    if (lblItemRefNo != null)
                    {
                        if (FunIsCollateralSecurityIDISExists(lblItemRefNo.Text.Trim()))
                        {
                            //btnRemove.Attributes.Add("onclick", "javascript:return confirm_delete();");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //*******  FINIANCIAL LIQUID SECURITIES  ********//
    private void FunFinancialLIQSecuritiesGridviewRowcommand(GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {

                dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];

                DropDownList ddlCollSecurities = (DropDownList)gvFinDetails.FooterRow.FindControl("ddlCollSecurities");
                TextBox txtInsuranceIssuedBy = (TextBox)gvFinDetails.FooterRow.FindControl("txtInsuranceIssuedBy");
                TextBox txtPolicyNo = (TextBox)gvFinDetails.FooterRow.FindControl("txtPolicyNo");
                TextBox txtPolicyValue = (TextBox)gvFinDetails.FooterRow.FindControl("txtPolicyValue");
                TextBox txtCurrentValue = (TextBox)gvFinDetails.FooterRow.FindControl("txtCurrentValue");
                TextBox txtMaturityDate = (TextBox)gvFinDetails.FooterRow.FindControl("txtMaturityDate");
                TextBox txtCollateralRef = (TextBox)gvFinDetails.FooterRow.FindControl("txtCollateralRef");
                TextBox txtScanRef = (TextBox)gvFinDetails.FooterRow.FindControl("txtScanRef");
                TextBox txtItemRefNo = (TextBox)gvFinDetails.FooterRow.FindControl("txtItemRefNo");

                if (dtSecurities.Rows.Count > 0)
                {
                    if (dtSecurities.Rows[0]["SlNo"].ToString() == "0" && dtSecurities.Rows[0]["Collateral_Detail_ID"].ToString() == "")
                    {
                        dtSecurities.Rows[0].Delete();
                    }
                }

                //if (txtInsuranceIssuedBy.Text == "" && txtPolicyNo.Text == "" && txtPolicyValue.Text == "" && txtCurrentValue.Text == ""
                //      && txtMaturityDate.Text == "" && txtCollateralRef.Text == "" && txtScanRef.Text == "" && txtItemRefNo.Text == "")
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Add one row in Financial Liquid Security details");
                //    return;
                //}
                dr = dtSecurities.NewRow();

                dr["SlNo"] = gvFinDetails.Rows.Count + 1;
                dr["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
                dr["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text;
                dr["Insurance_Issued_By"] = txtInsuranceIssuedBy.Text.Trim();
                dr["Policy_No"] = txtPolicyNo.Text.Trim();
                dr["Policy_Value"] = txtPolicyValue.Text.Trim();
                dr["Current_Value"] = txtCurrentValue.Text.Trim() == "" ? "0" : txtCurrentValue.Text.Trim();
                dr["Maturity_Date"] = txtMaturityDate.Text.Trim();
                dr["Collateral_Ref_No"] = txtCollateralRef.Text.Trim();
                dr["Scan_Ref_No"] = txtScanRef.Text.Trim();
                dr["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
                dr["Ownership_Fin"] = txtFOwnership.Text.Trim();
                //dr["Is_Release"] = true;
                dtSecurities.Rows.Add(dr);
                gvFinDetails.DataSource = dtSecurities;
                gvFinDetails.DataBind();
                ViewState[Securities.Financial.ToString()] = dtSecurities;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunFinancialLIQSecuritiesGridviewRowDeleting(GridViewDeleteEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];
            if (FunIsCollateralSecurityIDISExists(dtSecurities.Rows[e.RowIndex]["Collateral_Item_Ref_No"].ToString()))
            {
                if (hdnValue.Value == "true")
                {
                    ViewState["RowID"] = e.RowIndex;
                    ViewState["CollateralType"] = "F";
                    txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                    MPE.Show();
                }
            }
            else
            {
                ViewState["RowID"] = e.RowIndex;
                ViewState["CollateralType"] = "F";
                txtExcludeDate.Text = Utility.StringToDate(DateTime.Now.ToString()).ToString(strDateFormat);
                MPE.Show();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunFinancialLIQSecuritiesGridviewRowUpdating(GridViewUpdateEventArgs e)
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];
            GridViewRow gvRow = gvFinDetails.Rows[e.RowIndex];

            DropDownList ddlCollSecurities = (DropDownList)gvRow.FindControl("ddlCollSecurities");
            TextBox txtInsuranceIssuedBy = (TextBox)gvRow.FindControl("txtInsuranceIssuedBy");
            TextBox txtPolicyNo = (TextBox)gvRow.FindControl("txtPolicyNo");
            TextBox txtPolicyValue = (TextBox)gvRow.FindControl("txtPolicyValue");
            TextBox txtCurrentValue = (TextBox)gvRow.FindControl("txtCurrentValue");
            TextBox txtMaturityDate = (TextBox)gvRow.FindControl("txtMaturityDate");
            TextBox txtCollateralRef = (TextBox)gvRow.FindControl("txtCollateralRef");
            TextBox txtScanRef = (TextBox)gvRow.FindControl("txtScanRef");
            TextBox txtItemRefNo = (TextBox)gvRow.FindControl("txtItemRefNo");


            DataRow drow = dtSecurities.Rows[e.RowIndex];
            drow.BeginEdit();
            drow["SlNo"] = e.RowIndex + 1;
            drow["Collateral_Detail_ID"] = ddlCollSecurities.SelectedValue;
            drow["Collateral_Securities"] = ddlCollSecurities.SelectedItem.Text.Trim();
            drow["Insurance_Issued_By"] = txtInsuranceIssuedBy.Text.Trim();
            drow["Policy_No"] = txtPolicyNo.Text.Trim();
            drow["Policy_Value"] = txtPolicyValue.Text.Trim();
            drow["Current_Value"] = txtCurrentValue.Text.Trim() == "" ? "0" : txtCurrentValue.Text.Trim();
            drow["Maturity_Date"] = txtMaturityDate.Text.Trim();
            drow["Collateral_Ref_No"] = txtCollateralRef.Text.Trim();
            drow["Scan_Ref_No"] = txtScanRef.Text.Trim();
            drow["Collateral_Item_Ref_No"] = txtItemRefNo.Text.Trim();
            drow["Ownership_Fin"] = txtFOwnership.Text.Trim() == "" ? "0" : txtFOwnership.Text.Trim();
            //drow["Is_Release"] = true;
            drow.EndEdit();
            ViewState[Securities.Financial.ToString()] = dtSecurities;
            gvFinDetails.EditIndex = -1;
            gvFinDetails.ShowFooter = true;
            dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];
            gvFinDetails.DataSource = (DataTable)ViewState[Securities.Financial.ToString()];
            gvFinDetails.DataBind();
            gvFinDetails.Columns[gvFinDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunFinancialLIQSecuritiesGridviewRowEditing(GridViewEditEventArgs e)
    {
        try
        {
            gvFinDetails.ShowFooter = false;
            gvFinDetails.EditIndex = e.NewEditIndex;
            dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];
            gvFinDetails.DataSource = dtSecurities;
            gvFinDetails.DataBind();
            DropDownList ddlCollSecurities = gvFinDetails.Rows[e.NewEditIndex].FindControl("ddlCollSecurities") as DropDownList;
            //FunPriFillCollateralDescription("5", ddlCollSecurities);
            //ddlCollSecurities.SelectedValue = dtSecurities.Rows[e.NewEditIndex]["Collateral_Detail_ID"].ToString();
            ListItem LICollSecurity = new ListItem(dtSecurities.Rows[e.NewEditIndex]["Collateral_Securities"].ToString(), dtSecurities.Rows[e.NewEditIndex]["Collateral_Detail_ID"].ToString());
            ddlCollSecurities.Items.Insert(0, LICollSecurity);

            TextBox txtPolicyValue = gvFinDetails.Rows[e.NewEditIndex].FindControl("txtPolicyValue") as TextBox;
            txtPolicyValue.Style.Add("text-align", "right");
            txtPolicyValue.SetDecimalPrefixSuffix(8, 4, false, "Policy Value");

            TextBox txtCurrentValue = gvFinDetails.Rows[e.NewEditIndex].FindControl("txtCurrentValue") as TextBox;
            txtCurrentValue.Style.Add("text-align", "right");
            txtCurrentValue.SetDecimalPrefixSuffix(8, 4, false, "Current Value");

            AjaxControlToolkit.CalendarExtender CEMaturityDate = gvFinDetails.Rows[e.NewEditIndex].FindControl("CEMaturityDate") as AjaxControlToolkit.CalendarExtender;
            CEMaturityDate.Format = strDateFormat;
            gvFinDetails.Columns[gvFinDetails.Columns.Count - 1].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private void FunFinancialLIQSecuritiesGridviewRowCancelingEdit(GridViewCancelEditEventArgs e)
    {
        try
        {
            gvFinDetails.EditIndex = -1;
            gvFinDetails.ShowFooter = true;
            gvFinDetails.DataSource = (DataTable)ViewState[Securities.Financial.ToString()];
            gvFinDetails.DataBind();
            gvFinDetails.Columns[gvFinDetails.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunFinancialLIQSecuritiesGridviewRowDataBound(GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlCollSecurities = e.Row.FindControl("ddlCollSecurities") as DropDownList;
                if (ddlCollSecurities != null)
                {
                    FunPriFillCollateralDescription("5", ddlCollSecurities);
                    FunPriSetAlignmentsFinancialLIQSecurities(e);
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMode = e.Row.FindControl("lblMode") as Label;
                LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;
                if (lblMode != null && !string.IsNullOrEmpty(lblMode.Text))
                {
                    btnRemove.Enabled = false;
                }

                if (e.Row.RowState != DataControlRowState.Edit)
                {
                    Label lblItemRefNo = e.Row.FindControl("lblItemRefNo") as Label;
                    if (lblItemRefNo != null)
                    {
                        if (FunIsCollateralSecurityIDISExists(lblItemRefNo.Text.Trim()))
                        {
                            btnRemove.Attributes.Add("onclick", "javascript:return confirm_delete();");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion

    #region "Button Events"
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveCollateralCapture();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }



    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPageClear);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }
    protected void btnLoadCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            pnlCustDetails.Visible = true;
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            if (ddlType.SelectedIndex > 0)
            {
                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

                if (hdnCustomerId != null && hdnCustomerId.Value != string.Empty)
                {
                    hdnCustID.Value = hdnCustomerId.Value;
                    intCustomer = Convert.ToInt32(hdnCustomerId.Value);


                    PopulateCustomerDetails(hdnCustID.Value, "1");
                    ddlRefPoint.SelectedIndex = 0;
                    ddlDYNRefPoint.ClearSelection();
                    ddlDYNRefPoint.Enabled = false;
                    //lblDYNRefPoint.Enabled= false;
                    grvprimeaccount.DataSource = null;
                    grvprimeaccount.DataBind();
                    pnlPASADetails.Visible = false;


                    Label lblCustCode = (Label)S3GCustomerAddress1.FindControl("lblCustomerCode");
                    Label lblCustName = (Label)S3GCustomerAddress1.FindControl("lblCustomerName");

                    lblCustCode.Text = "Customer Code";
                    lblCustName.Text = "Customer Name";
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Select Type");
                txtName.Text = "";
                txtCustomerCode.Text = "";

                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    #region "GridView Events"
    //******* HIGH Liquid Securities ********//
    protected void gvHighLiqDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FunHighLIQSecuritiesGridviewRowcommand(e);
    }

    protected void gvHighLiqDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunHighLIQSecuritiesGridviewRowDeleting(e);
        FunClearHDetails();
    }
    protected void gvHighLiqDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        FunHighLIQSecuritiesGridViewRowUpdating(e);
    }
    protected void gvHighLiqDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        FunHighLIQSecuritiesGridViewRowEditing(e);
    }
    protected void gvHighLiqDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        FunHighLIQSecuritiesGridviewRowCancelingEdit(e);
    }


    protected void gvHighLiqDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        FunHighLIQSecuritiesGridviewRowDataBound(e);
    }

    //******* MEDIUM Liquid Securities ********//
    protected void gvMedLiqDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FunMediumLIQSecuritiesGridviewRowcommand(e);
    }
    protected void gvMedLiqDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunMediumLIQSecuritiesGridviewRowDeleting(e);
        FunClearMDetails();

    }
    protected void gvMedLiqDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        FunMediumLIQSecuritiesGridviewRowUpdating(e);
    }
    protected void gvMedLiqDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        FunMediumLIQSecuritiesGridviewRowEditing(e);
    }
    protected void gvMedLiqDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        FunMediumLIQSecuritiesGridviewRowCancelingEdit(e);
    }
    protected void gvMedLiqDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        FunMediumLIQSecuritiesGridviewRowDataBound(e);
    }
    //******* LOW Liquid Securities ********//
    protected void gvLowLiqDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FunLOWLIQSecuritiesGridviewRowcommand(e);
    }
    protected void gvLowLiqDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunLOWLIQSecuritiesGridviewRowDeleting(e);
    }
    protected void gvLowLiqDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        FunLOWLIQSecuritiesGridviewRowUpdating(e);
    }
    protected void gvLowLiqDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        FunLOWLIQSecuritiesGridviewRowEditing(e);
    }
    protected void gvLowLiqDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        FunLOWLIQSecuritiesGridviewRowCancelingEdit(e);
    }
    protected void gvLowLiqDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        FunLOWLIQSecuritiesGridviewRowDataBound(e);
    }
    //******* COMMODITIES Liquid Securities ********//
    protected void gvCommoDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FunCommoditiesLIQSecuritiesGridviewRowcommand(e);
    }

    protected void gvCommoDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunCommoditiesLIQSecuritiesGridviewRowDeleting(e);
    }
    protected void gvCommoDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        FunCommoditiesLIQSecuritiesGridviewRowUpdating(e);
    }
    protected void gvCommoDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        FunCommoditiesLIQSecuritiesGridviewRowEditing(e);
    }
    protected void gvCommoDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        FunCommoditiesLIQSecuritiesGridviewRowCancelingEdit(e);
    }
    protected void gvCommoDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        FunCommoditiesLIQSecuritiesGridviewRowDataBound(e);
    }
    //******* FINANCIAL Liquid Securities ********//
    protected void gvFinDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FunFinancialLIQSecuritiesGridviewRowcommand(e);
    }
    protected void gvFinDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        FunFinancialLIQSecuritiesGridviewRowDeleting(e);
    }
    protected void gvFinDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        FunFinancialLIQSecuritiesGridviewRowUpdating(e);
    }
    protected void gvFinDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        FunFinancialLIQSecuritiesGridviewRowEditing(e);
    }
    protected void gvFinDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        FunFinancialLIQSecuritiesGridviewRowCancelingEdit(e);
    }
    protected void gvFinDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        FunFinancialLIQSecuritiesGridviewRowDataBound(e);
    }
    #endregion

    #region "Drop Down Events"


    protected void ddlRefPoint_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRefPoint.SelectedValue == "3")
        {
            rfvTypeGo.Enabled = false;
            rfvType.Enabled = false;
            rfvCustomerCodeGo.Enabled = false;
            rfvCustomerCode.Enabled = false;
        }
        else
        {
            if (rbtlCollCollected.SelectedValue == "2")
            {
                rfvTypeGo.Enabled = 
                rfvType.Enabled = false;
            }
            else
            {
                rfvTypeGo.Enabled = true;
                rfvType.Enabled = true;
            }            
            rfvCustomerCodeGo.Enabled = true;
            rfvCustomerCode.Enabled = true;
        }
        FunPubFillDynamicRefPointField();
        ddlRefPoint.Focus();
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        S3GCustomerAddress1.ClearCustomerDetails();
        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        intCustomer = 0;
        txtCustomerCode.Text = "";
        txtName.Text = "";
        FunBriClearDDL();
        if (ddlType.SelectedIndex > 0)
        {
            if (Convert.ToInt32(ddlType.SelectedValue) == 2)
            {
                ucCustomerCodeLov.strLOV_Code = "GCMD";
            }
            else if (Convert.ToInt32(ddlType.SelectedValue) == 3)
            {
                ucCustomerCodeLov.strLOV_Code = "PCMD";
            }
            else if (Convert.ToInt32(ddlType.SelectedValue) == 4)
            {
                ucCustomerCodeLov.strLOV_Code = "CCMD";
            }
            else
            {
                ucCustomerCodeLov.strLOV_Code = "CCP";
            }
            ViewState["Type"] = ucCustomerCodeLov.strLOV_Code;
        }
        else
        {
            ucCustomerCodeLov.strLOV_Code = "CCP";
        }
    }

    private void FunBriClearDDL()
    {
        ddlConstitution.ClearSelection();
        ddlLOB.ClearSelection();
        ddlBranch.Clear(); ;
        ddlProductCode.ClearSelection();
        ddlRefPoint.ClearSelection();
        ddlDYNRefPoint.ClearSelection();
        grvprimeaccount.DataSource = null;
        grvprimeaccount.DataBind();
        pnlPASADetails.Visible = false;
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FunPriCollateralDetailsOnLOBandProductCode();
        //FunPriBindBranch();
        ddlBranch.Clear();
        FunPriEnableTab();
        FunPriBindProduct();

        //FunPriBindConstitution();
        ddlRefPoint.SelectedValue = "0";
        ddlRefPoint_SelectedIndexChanged(sender, e);
        //if (ddlRefPoint.SelectedValue == "6")
        //{
        //    if (FunPriFillAccountDetails().Rows.Count > 0)
        //        pnlPASADetails.Visible = true;
        //    else
        //    {
        //        pnlPASADetails.Visible = false;
        //        Utility.FunShowAlertMsg(this.Page, "No accounts for the selected customer");
        //        ddlRefPoint.SelectedValue = "0";
        //    }
        //}
        if (rbtlCollCollected.SelectedValue == "1")
        {
            //Saranya

            //if (ddlLOB.SelectedValue.ToString() != "0")
            //    ucCustomerCodeLov._strLobID = ddlLOB.SelectedValue;
            //TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            //txtName.Text = "";
            //S3GCustomerAddress1.ClearCustomerDetails();
            //txtCustomerCode.Text = "";
            //intCustomer = 0;

            //End
        }
        else
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OPTION", "12");
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@UserID", intUserId.ToString());
            if (strMode == "C")
                Procparam.Add("@Is_Active", "1");
            if (strMode == "C" && ddlLOB.SelectedValue != "0")
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            if (strMode == "C" && ddlBranch.SelectedValue != "0")
                Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());

            //lblCustorAgent.Text = "Collection Agent";
            ddlCollectionAgent.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "Emp_UTPA_ID", "DebtCollector_Code" });
            S3GCustomerAddress1.ClearCustomerDetails();
            intCustomer = 0;
            txtCustomerCode.Text = "";
        }
    }

    private void FunPriEnableTab()
    {
        tabHighLiq.Enabled = false;
        tabMedSeq.Enabled = false;
        tabLow.Enabled = false;
        tabCommodities.Enabled = false;
        tabFinancial.Enabled = false;
        //FunPriInitialRow(Securities.HighLiquid);
        //FunPriInitialRow(Securities.MediumLiquid);
        //FunPriInitialRow(Securities.LowLiquid);
        //FunPriInitialRow(Securities.Commodities);
        //FunPriInitialRow(Securities.Financial);

    }

    protected void ddlProductCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriEnableTab();
    }
    protected void ddlConstitution_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriEnableTab();
        if (ddlConstitution.SelectedValue.ToString() != "0")
            ucCustomerCodeLov._strConstitutionID = ddlConstitution.SelectedValue;
        int i = Convert.ToInt32(rbtlCollCollected.SelectedValue);
        if (i == 1)
        {
            S3GCustomerAddress1.ClearCustomerDetails();
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            intCustomer = 0;
            txtCustomerCode.Text = "";
            txt.Text = "";
        }

        //FunPriCollateralDetailsOnLOBandProductCode();
    }
    protected void ddlCollectionAgent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCollectionAgent.SelectedItem.Text != "--Select--" && rbtlCollCollected.SelectedValue == "2")
        {
            pnlCustDetails.Visible = true;
            PopulateCustomerDetails(ddlCollectionAgent.SelectedValue, "2");
            Label lblCustCode = (Label)S3GCustomerAddress1.FindControl("lblCustomerCode");
            Label lblCustName = (Label)S3GCustomerAddress1.FindControl("lblCustomerName");

            lblCustCode.Text = "Collection Agent Code";
            lblCustName.Text = "Collection Agent Name";
        }
        else if (ddlCollectionAgent.SelectedValue == "0")
        {
            //pnlCustDetails.Visible = false;
            S3GCustomerAddress1.ClearCustomerDetails();
            txtCustomerCode.Text = "";
        }
    }
    #endregion

    #region "Radio Button Event"
    protected void rbtlCollCollected_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriCollateralCollectedBy();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    protected void chkAccount_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriGetSelectedAccountID();
        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            throw objFaultExp;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private string FunPriGetSelectedAccountID()
    {
        strAccount_ID = "0";
        CheckBox chkAccount = null;
        foreach (GridViewRow grvData in grvprimeaccount.Rows)
        {
            chkAccount = ((CheckBox)grvData.FindControl("chkAccount"));
            if (chkAccount.Checked)
            {
                strAccount_ID = strAccount_ID + "," + ((Label)grvData.FindControl("lblAccount_ID")).Text;
                //FunPriGetInsurancePolicy(intAsset_ID);
            }
            //else
            //    chkAccount.Enabled = false;
        }

        //if (strAccount_ID == "0")
        //{
        //    foreach (GridViewRow grvData in grvprimeaccount.Rows)
        //    {
        //        chkAccount = ((CheckBox)grvData.FindControl("chkAccount"));
        //        chkAccount.Enabled = true;
        //    }
        //}
        if (strAccount_ID != "0")
        {
            strAccount_ID = strAccount_ID.Substring(2, strAccount_ID.Length - 2);
        }

        return strAccount_ID;
    }

    //public bool GVAccountBoolFormat(string val)
    //{
    //    string AccountId = Utility.GetDefaultData("", "");
    //    if (val == ViewState["AccountID"].ToString())
    //        return true;
    //    else
    //        return false;
    //}
    #endregion


    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "163");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }


    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {   //Saranya
        //if (ddlBranch.SelectedValue.ToString() != "0")
        //    ucCustomerCodeLov._strBranchID = ddlBranch.SelectedValue;
        //End
        if (ddlRefPoint.SelectedValue == "6")
        {
            if (FunPriFillAccountDetails().Rows.Count > 0)
                pnlPASADetails.Visible = true;
            else
            {
                pnlPASADetails.Visible = false;
                Utility.FunShowAlertMsg(this.Page, "No accounts for the selected customer");
                ddlRefPoint.SelectedValue = "0";
            }
        }
        if (rbtlCollCollected.SelectedValue == "1")
        {
            //Saranya

            //if (ddlBranch.SelectedValue.ToString() != "0")
            //    ucCustomerCodeLov._strBranchID = ddlBranch.SelectedValue;
            //TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            //txtName.Text = string.Empty;
            //S3GCustomerAddress1.ClearCustomerDetails();
            //txtCustomerCode.Text = string.Empty;
            //intCustomer = 0;

            //End
        }
        else
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OPTION", "12");
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@UserID", Convert.ToString(intUserId));
            if (strMode == "C")
                Procparam.Add("@Is_Active", "1");
            if (strMode == "C" && ddlLOB.SelectedValue != "0")
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            if (strMode == "C" && ddlBranch.SelectedValue != "0")
                Procparam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));

            //lblCustorAgent.Text = "Collection Agent";
            ddlCollectionAgent.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "Emp_UTPA_ID", "DebtCollector_Code" });
            S3GCustomerAddress1.ClearCustomerDetails();
            intCustomer = 0;
            txtCustomerCode.Text = string.Empty;
        }
    }


    private string Funsetsuffix()
    {

        int suffix = 1;

        // S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        // suffix = 0;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {
            if (ObjS3GAdminService.FunPubPasswordValidation(ObjUserInfo.ProUserIdRW, txtPassword.Text.Trim().Substring(1, txtPassword.Text.Trim().Length - 1)) == 0)
            {
                if (ViewState["RowID"] != null)
                    RowID = Convert.ToInt32(ViewState["RowID"]);
                if (ViewState["CollateralType"] != null)
                    strCollateralType = ViewState["CollateralType"].ToString();
                FunPriDeleteCollateralSecurity(strCollateralType, RowID);
                MPE.Hide();
            }
            else
                Utility.FunShowAlertMsg(this.Page, "Password does not match");
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (ObjS3GAdminService != null)
                ObjS3GAdminService.Close();
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        tabHighLiq.Enabled = true;
        tabMedSeq.Enabled = true;
        tabLow.Enabled = true;
        tabCommodities.Enabled = true;
        tabFinancial.Enabled = true;
        FunPriCollateralDetailsOnLOBandProductCode();
    }

    private void FunPriDeleteCollateralSecurity(string collateralType, int intRowID)
    {
        switch (collateralType)
        {
            case "H":
                {
                    dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
                    dtSecurities.Rows.RemoveAt(intRowID);
                    dtSecurities.AcceptChanges();
                    gvhHighLiqDetails.DataSource = dtSecurities;
                    gvhHighLiqDetails.DataBind();
                    ViewState[Securities.HighLiquid.ToString()] = dtSecurities;
                    if (dtSecurities.Rows.Count == 0)
                    { FunPriInitialRow(Securities.HighLiquid); }
                    break;
                }
            case "M":
                {
                    dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
                    dtSecurities.Rows.RemoveAt(intRowID);
                    dtSecurities.AcceptChanges();
                    gvMedLiqDetails.DataSource = dtSecurities;
                    gvMedLiqDetails.DataBind();
                    ViewState[Securities.MediumLiquid.ToString()] = dtSecurities;
                    if (dtSecurities.Rows.Count == 0)
                    { FunPriInitialRow(Securities.MediumLiquid); }
                    break;
                }
            case "L":
                {
                    dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];
                    dtSecurities.Rows.RemoveAt(intRowID);
                    dtSecurities.AcceptChanges();
                    gvLowLiqDetails.DataSource = dtSecurities;
                    gvLowLiqDetails.DataBind();
                    ViewState[Securities.LowLiquid.ToString()] = dtSecurities;
                    if (dtSecurities.Rows.Count == 0)
                    { FunPriInitialRow(Securities.LowLiquid); }
                    break;
                }
            case "C":
                {
                    dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];
                    dtSecurities.Rows.RemoveAt(intRowID);
                    dtSecurities.AcceptChanges();
                    gvCommoDetails.DataSource = dtSecurities;
                    gvCommoDetails.DataBind();
                    ViewState[Securities.Commodities.ToString()] = dtSecurities;
                    if (dtSecurities.Rows.Count == 0)
                    { FunPriInitialRow(Securities.Commodities); }
                    break;
                }
            case "F":
                {
                    dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];
                    dtSecurities.Rows.RemoveAt(intRowID);
                    dtSecurities.AcceptChanges();
                    gvFinDetails.DataSource = dtSecurities;
                    gvFinDetails.DataBind();
                    ViewState[Securities.Financial.ToString()] = dtSecurities;
                    if (dtSecurities.Rows.Count == 0)
                    { FunPriInitialRow(Securities.Financial); }
                    break;
                }

        }
    }
    protected void btnCan_Click(object sender, EventArgs e)
    {
        MPE.Hide();
    }
    private string returnLoginUserPassword()
    {
        DataTable dt = new DataTable();
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        Procparam.Add("@User_ID ", ObjUserInfo.ProUserIdRW.ToString());
        Procparam.Add("@Company_ID ", Convert.ToString(intCompanyId));
        dt = Utility.GetDefaultData("S3G_CLT_GetLoginUserPassword", Procparam);
        if (dt.Rows.Count > 0)
            return dt.Rows[0]["Password"].ToString();
        else
            return string.Empty;
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        MPE.Show();
    }

    protected void ddlCollSecurities_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DropDownList ddlCollSecurities = (DropDownList)sender;
        GridViewRow gvRow = (GridViewRow)ddlCollSecurities.Parent.Parent;
        RequiredFieldValidator rfvRegistration = gvRow.FindControl("rfvRegistration") as RequiredFieldValidator;
        RequiredFieldValidator rfvSerialNo = gvRow.FindControl("rfvSerialNo") as RequiredFieldValidator;

        RequiredFieldValidator rfvInterest = gvRow.FindControl("rfvInterest") as RequiredFieldValidator;
        RequiredFieldValidator rfvMaturityDate = gvRow.FindControl("rfvMaturityDate") as RequiredFieldValidator;
        RequiredFieldValidator rfvMaturityValue = gvRow.FindControl("rfvMaturityValue") as RequiredFieldValidator;

        RequiredFieldValidator rfvIssuedBy = gvRow.FindControl("rfvIssuedBy") as RequiredFieldValidator;
        RequiredFieldValidator rfvUnitFaceValue = gvRow.FindControl("rfvUnitFaceValue") as RequiredFieldValidator;
        RequiredFieldValidator rfvNoOfUnits = gvRow.FindControl("rfvNoOfUnits") as RequiredFieldValidator;

        if (ddlCollSecurities.SelectedItem.Text.Trim() == "Vehicles")
        {
            rfvRegistration.Visible = true;
            rfvSerialNo.Visible = false;
        }
        else if (ddlCollSecurities.SelectedItem.Text.Trim() == "Machinery")
        {
            rfvRegistration.Visible = false;
            rfvSerialNo.Visible = true;
        }

        if (ddlCollSecurities.SelectedItem.Text.Trim().ToLower() == "equity shares")
        {
            rfvInterest.Enabled = rfvMaturityDate.Enabled = rfvMaturityValue.Enabled = false;
        }
        else
        {
            rfvInterest.Enabled = rfvMaturityDate.Enabled = rfvMaturityValue.Enabled = true;
        }

        if (ddlCollSecurities.SelectedItem.Text.Trim().ToLower() == "deposits")
        {
            rfvIssuedBy.Enabled = rfvUnitFaceValue.Enabled = rfvNoOfUnits.Enabled = false;
        }
        else
        {
            rfvIssuedBy.Enabled = rfvUnitFaceValue.Enabled = rfvNoOfUnits.Enabled = true;
        }
    }
    protected void ddlDemat_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlDemat = (DropDownList)sender;
        GridViewRow gvRow = (GridViewRow)ddlDemat.Parent.Parent;

        TextBox txtDPName = gvRow.FindControl("txtDPName") as TextBox;
        TextBox txtDPNo = gvRow.FindControl("txtDPNo") as TextBox;
        TextBox txtClientID = gvRow.FindControl("txtClientID") as TextBox;
        TextBox txtCertificateNo = gvRow.FindControl("txtCertificateNo") as TextBox;

        RequiredFieldValidator rfvDPName = gvRow.FindControl("rfvDPName") as RequiredFieldValidator;
        RequiredFieldValidator rfvDPNo = gvRow.FindControl("rfvDPNo") as RequiredFieldValidator;
        RequiredFieldValidator rfvClientID = gvRow.FindControl("rfvClientID") as RequiredFieldValidator;
        RequiredFieldValidator rfvCertificateNo = gvRow.FindControl("rfvCertificateNo") as RequiredFieldValidator;

        txtDPName.Text = txtDPNo.Text = txtClientID.Text = txtCertificateNo.Text = string.Empty;
        if (ddlDemat.SelectedItem.Text == "Yes")
        {
            rfvDPName.Visible = true;
            rfvDPNo.Visible = true;
            rfvClientID.Visible = true;
            rfvCertificateNo.Visible = false;
        }
        else if (ddlDemat.SelectedItem.Text == "No")
        {
            rfvDPName.Visible = false;
            rfvDPNo.Visible = false;
            rfvClientID.Visible = false;
            rfvCertificateNo.Visible = true;
        }
    }
    protected void ddlCollSecurities_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlCollSecurities = (DropDownList)sender;
        GridViewRow gvRow = (GridViewRow)ddlCollSecurities.Parent.Parent;
        RequiredFieldValidator rfvIssuedBy = gvRow.FindControl("rfvIssuedBy") as RequiredFieldValidator;
        RequiredFieldValidator rfvUnitFaceValue = gvRow.FindControl("rfvUnitFaceValue") as RequiredFieldValidator;
        RequiredFieldValidator rfvNoOfUnits = gvRow.FindControl("rfvNoOfUnits") as RequiredFieldValidator;

        if (ddlCollSecurities.SelectedItem.Text.Trim() == "Deposits")
        {
            rfvIssuedBy.Visible = false;
            rfvUnitFaceValue.Visible = false;
            rfvNoOfUnits.Visible = false;
        }
        else
        {
            rfvIssuedBy.Visible = true;
            rfvUnitFaceValue.Visible = true;
            rfvNoOfUnits.Visible = true;
        }

    }

    //High
    protected void btnAddH_Click(object sender, EventArgs e)
    {
        FunPriAddHighLIQSecurities();
        FunClearHDetails();
    }

    protected void rdHSelect_CheckedChanged(object sender, EventArgs e)
    {
        btnModifyH.Enabled = true;
        btnAddH.Enabled = btnhClearH.Enabled = false;

        int intRowIndex = Utility.FunPubGetGridRowID("gvhHighLiqDetails", ((RadioButton)sender).ClientID);
        dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];

        FunPriResetRdButton(gvhHighLiqDetails, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];

        lblhSlNo.Text = drow["SlNo"].ToString();
        if (!string.IsNullOrEmpty(drow["Collateral_Detail_ID"].ToString()))
        {
            ddlhCollSecurities.SelectedValue = drow["Collateral_Detail_ID"].ToString();    
        }
        
        txthIssuedBy.Text = drow["Issued_By"].ToString();
        ddlhDemat.SelectedValue = drow["Demat"].ToString();// == "true" ? "1" : "0";
        txthDPName.Text = drow["DP_Name"].ToString();
        txthDPNo.Text = drow["DP_No"].ToString();
        txthClientID.Text = drow["Client_ID"].ToString();
        txthCertificateNo.Text = drow["Certificate_No"].ToString();
        txthFolioNo.Text = drow["Folio_No"].ToString();
        txthUnitFaceValue.Text = drow["Unit_Face_Value"].ToString();
        txthNoOfUnits.Text = drow["No_Of_Units"].ToString();
        txthInterest.Text = drow["Interest_Percentage"].ToString();
        txthMaturityDate.Text = drow["Maturity_Date"].ToString();
        txthMaturityValue.Text = drow["Maturity_Value"].ToString();
        txthMarketRate.Text = drow["Market_Rate"].ToString();
        txthMarketValue.Text = drow["Market_Value"].ToString();
        txthCollateralRefNo.Text = drow["Collateral_Ref_No"].ToString();
        txthScanRefNo.Text = drow["Scan_Ref_No"].ToString();
        txthItemRefNo.Text = drow["Collateral_Item_Ref_No"].ToString();
        txthownership.Text = drow["Ownership"].ToString();
        //txtCollTransNo.Text.Trim();

        ddlhCollSecurities_SelectedIndexChanged(null, null);

        if (PageMode == PageModes.Query)
        {
            btnModifyH.Enabled = false;
        }

        if (PageMode != PageModes.Query)
        {
            ddlhDemat_SelectedIndexChanged(null, null);
        }

    }

    protected void btnModifyH_Click(object sender, EventArgs e)
    {
        FunHighLIQSecuritiesGridViewRowUpdating();
    }

    private void FunHighLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];

            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblhSlNo.Text) - 1];
            drow.BeginEdit();
            drow["SlNo"] = lblhSlNo.Text;
            drow["Collateral_Detail_ID"] = ddlhCollSecurities.SelectedValue;
            drow["Collateral_Securities"] = ddlhCollSecurities.SelectedItem.Text.Trim();
            drow["Issued_By"] = txthIssuedBy.Text.Trim();
            drow["Demat"] = ddlhDemat.SelectedValue;// == "1" ? true : false;
            drow["DematDesc"] = ddlhDemat.SelectedItem.Text.Trim();
            drow["DP_Name"] = txthDPName.Text.Trim();
            drow["DP_No"] = txthDPNo.Text == "" ? "0" : txthDPNo.Text.Trim();//txtDPNo.Text.Trim();
            drow["Client_ID"] = txthClientID.Text == "" ? "0" : txthClientID.Text.Trim(); // txtClientID.Text.Trim();
            drow["Certificate_No"] = txthCertificateNo.Text.Trim();
            drow["Folio_No"] = txthFolioNo.Text.Trim();
            drow["Unit_Face_Value"] = txthUnitFaceValue.Text.Trim() == "" ? "0" : txthUnitFaceValue.Text.Trim();
            drow["No_Of_Units"] = txthNoOfUnits.Text.Trim() == "" ? "0" : txthNoOfUnits.Text.Trim();
            drow["Interest_Percentage"] = txthInterest.Text.Trim() == "" ? "0" : txthInterest.Text.Trim();
            drow["Maturity_Date"] = txthMaturityDate.Text.Trim();
            drow["Maturity_Value"] = txthMaturityValue.Text.Trim() == "" ? "0" : txthMaturityValue.Text.Trim();
            drow["Market_Rate"] = txthMarketRate.Text.Trim() == "" ? "0" : txthMarketRate.Text.Trim();
            drow["Market_Value"] = txthMarketValue.Text.Trim() == "" ? "0" : txthMarketValue.Text.Trim();
            drow["Collateral_Ref_No"] = txthCollateralRefNo.Text.Trim();
            drow["Scan_Ref_No"] = txthScanRefNo.Text.Trim();
            drow["Collateral_Item_Ref_No"] = txthItemRefNo.Text.Trim();
            drow["Collateral_Tran_No"] = txthTranRefNo.Text.Trim();
            drow["Ownership"] = txthownership.Text.Trim();
            drow.EndEdit();
            ViewState[Securities.HighLiquid.ToString()] = dtSecurities;

            dtSecurities = (DataTable)ViewState[Securities.HighLiquid.ToString()];
            gvhHighLiqDetails.DataSource = dtSecurities;
            gvhHighLiqDetails.DataBind();
            FunClearHDetails();

            btnAddH.Enabled = btnhClearH.Enabled = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunClearHDetails()
    {
        lblhSlNo.Text = "";
        ddlhCollSecurities.SelectedValue = "0";
        txthIssuedBy.Text = "";
        ddlhDemat.SelectedValue = "0";
        txthDPName.Text = txthDPNo.Text = txthClientID.Text =
        txthCertificateNo.Text = txthFolioNo.Text =
        txthUnitFaceValue.Text = txthNoOfUnits.Text =
        txthInterest.Text = txthMaturityDate.Text =
        txthMaturityValue.Text = txthMarketRate.Text =
        txthMarketValue.Text = txthCollateralRefNo.Text =
        txthScanRefNo.Text = txthItemRefNo.Text = txthownership.Text = "";

        btnModifyH.Enabled = false;
        btnAddH.Enabled = btnhClearH.Enabled = true;
        FunPriResetRdButton(gvhHighLiqDetails, -1);

    }

    protected void btnClearH_Click(object sender, EventArgs e)
    {
        FunClearHDetails();
        btnAddH.Enabled = true;
        btnModifyH.Enabled = false;
    }

    private void FunPriResetRdButton(GridView grv, int intRowIndex)
    {
        for (int i = 0; i <= grv.Rows.Count - 1; i++)
        {
            if (i != intRowIndex)
            {
                RadioButton rdSelect = grv.Rows[i].FindControl("rdSelect") as RadioButton;
                rdSelect.Checked = false;
            }
        }
    }

    protected void ddlhCollSecurities_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlCollSecurities.SelectedItem.Text.Trim() == "Vehicles")
        //{
        //    rfvRegistration.Visible = true;
        //    rfvSerialNo.Visible = false;
        //}
        //else if (ddlCollSecurities.SelectedItem.Text.Trim() == "Machinery")
        //{
        //    rfvRegistration.Visible = false;
        //    rfvSerialNo.Visible = true;
        //}

        if (ddlhCollSecurities.SelectedItem.Text.Trim().ToLower() == "equity shares")
        {
            rfvhInterest.Enabled = rfvhMaturityDate.Enabled = rfvhMaturityValue.Enabled = false;
            lblhInterest.CssClass = lblhMaturityDate.CssClass = lblhMaturityValue.CssClass = "";
        }
        else
        {
            rfvhInterest.Enabled = rfvhMaturityDate.Enabled = rfvhMaturityValue.Enabled = true;
            lblhInterest.CssClass = lblhMaturityDate.CssClass = lblhMaturityValue.CssClass = "styleReqFieldLabel";
        }

        if (ddlhCollSecurities.SelectedItem.Text.Trim().ToLower() == "deposits")
        {
            rfvIssuedBy.Enabled = rfvhUnitFaceValue.Enabled = rfvhNoOfUnits.Enabled = false;
            lblhIssuedBy.CssClass = lblhUnitFaceValue.CssClass = lblhNoOfUnits.CssClass = "";
        }
        else
        {
            rfvIssuedBy.Enabled = rfvhUnitFaceValue.Enabled = rfvhNoOfUnits.Enabled = true;
            lblhIssuedBy.CssClass = lblhUnitFaceValue.CssClass = lblhNoOfUnits.CssClass = "styleReqFieldLabel";
        }
    }


    //Medium
    protected void btnAddM_Click(object sender, EventArgs e)
    {
        FunProAddMediumLIQSecurities();
        FunClearMDetails();
    }

    protected void rdMSelect_CheckedChanged(object sender, EventArgs e)
    {
        btnModifyM.Enabled = true;
        btnAddM.Enabled = btnClearM.Enabled = false;

        int intRowIndex = Utility.FunPubGetGridRowID("gvMMedLiqDetails", ((RadioButton)sender).ClientID);
        dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];

        //FunPriResetRdButton(gvMMedLiqDetails, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];

        lblMSlNo.Text = drow["SlNo"].ToString();
        ddlMCollSecurities.SelectedValue = drow["Collateral_Detail_ID"].ToString();
        txtMDescription.Text = drow["Description"].ToString();
        txtMModel.Text = drow["Model"].ToString();
        txtMYear.Text = drow["Year"].ToString();
        txtMRegistrationNo.Text = drow["Registration_No"].ToString();
        txtMSerialNo.Text = drow["Serial_No"].ToString();
        txtMValue.Text = drow["Value"].ToString();
        txtMMarketValue.Text = drow["Market_Value"].ToString();
        txtMCollateralRefNo.Text = drow["Collateral_Ref_No"].ToString();
        txtMScanRefNo.Text = drow["Scan_Ref_No"].ToString();
        txtMItemRefNo.Text = drow["Collateral_Item_Ref_No"].ToString();
        txtMOwnership.Text = drow["Ownership_Medium"].ToString();

        //txtCollTransNo.Text.Trim();
        if (PageMode == PageModes.Query)
        {
            btnModifyM.Enabled = false;
        }

    }

    protected void btnModifyM_Click(object sender, EventArgs e)
    {
        FunMediumLIQSecuritiesGridViewRowUpdating();
    }

    private void FunMediumLIQSecuritiesGridViewRowUpdating()
    {

        dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
        GridViewRow gvRow = gvMMedLiqDetails.Rows[Convert.ToInt32(lblMSlNo.Text) - 1];


        if (txtMRegistrationNo.Text == "" && txtMSerialNo.Text == "")
        {
            Utility.FunShowAlertMsg(this.Page, "Enter Registration No or Serial No");
            txtMRegistrationNo.Focus();
            return;
        }
        for (int i = 0; i < dtSecurities.Rows.Count; i++)
        {
            DataRow dr = dtSecurities.Rows[i];
            if (dtSecurities.Rows[i]["SlNo"].ToString() != lblMSlNo.Text)
            {
                if (ddlMCollSecurities.SelectedItem.Text.Trim() == "Vehicle" && txtMRegistrationNo.Text.Trim() == dr["Registration_No"].ToString())
                {
                    Utility.FunShowAlertMsg(this.Page, "Registration No already existed");
                    txtMRegistrationNo.Focus();
                    return;
                }
                else if (ddlMCollSecurities.SelectedItem.Text.Trim() == "Machinery" && txtMSerialNo.Text.Trim() == dr["Serial_No"].ToString())
                {
                    Utility.FunShowAlertMsg(this.Page, "Serial No already existed");
                    txtMSerialNo.Focus();
                    return;
                }
            }
        }
        DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblMSlNo.Text) - 1];
        drow.BeginEdit();

        drow["Description"] = txtMDescription.Text.Trim();
        drow["Collateral_Securities"] = ddlMCollSecurities.SelectedItem.Text.Trim();
        drow["Collateral_Detail_ID"] = ddlMCollSecurities.SelectedValue;
        drow["Model"] = txtMModel.Text.Trim();
        drow["Year"] = txtMYear.Text.Trim();
        drow["Registration_No"] = txtMRegistrationNo.Text.Trim();
        drow["Serial_No"] = txtMSerialNo.Text.Trim();
        drow["Value"] = txtMValue.Text.Trim() == "" ? "0" : txtMValue.Text.Trim();
        drow["Market_Value"] = txtMMarketValue.Text.Trim() == "" ? "0" : txtMMarketValue.Text.Trim();
        drow["Collateral_Ref_No"] = txtMCollateralRefNo.Text.Trim();
        drow["Scan_Ref_No"] = txtMScanRefNo.Text.Trim();
        drow["Collateral_Item_Ref_No"] = txtMItemRefNo.Text.Trim();
        drow["Ownership_Medium"] = txtMOwnership.Text.Trim() == "" ? "0" : txtMOwnership.Text.Trim();
        //drow["Is_Release"] = chkRelease.Checked;
        drow.EndEdit();
        ViewState[Securities.MediumLiquid.ToString()] = dtSecurities;
        gvMMedLiqDetails.EditIndex = -1;
        gvMMedLiqDetails.ShowFooter = true;
        dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];
        //FunPubBindWorkflowSequence(dtWorkflow);
        gvMMedLiqDetails.DataSource = dtSecurities;
        gvMMedLiqDetails.DataBind();
        gvMMedLiqDetails.Columns[gvMMedLiqDetails.Columns.Count - 1].Visible = true;

        FunClearMDetails();

        btnAddM.Enabled = btnClearM.Enabled = true;
    }

    private void FunClearMDetails()
    {
        lblMSlNo.Text = "";
        ddlMCollSecurities.SelectedValue = "0";
        txtMDescription.Text = "";
        txtMModel.Text = "";
        txtMYear.Text = "";
        txtMRegistrationNo.Text = "";
        txtMSerialNo.Text = txtMValue.Text =
           txtMMarketValue.Text =
        txtMCollateralRefNo.Text =
        txtMScanRefNo.Text =
        txtMItemRefNo.Text = txtMOwnership.Text = "";

        btnAddM.Enabled = btnhClearH.Enabled = true;

        btnModifyM.Enabled = false;

        FunPriResetRdButton(gvMMedLiqDetails, -1);

    }

    protected void btnClearM_Click(object sender, EventArgs e)
    {
        FunClearMDetails();
        btnAddM.Enabled = true;
        btnModifyM.Enabled = false;
    }

    protected void ddlMCollSecurities_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMCollSecurities.SelectedItem.Text.Trim() == "Vehicles")
        {
            rfvMRegistration.Visible = true;
            rfvMSerialNo.Visible = false;
        }
        else if (ddlMCollSecurities.SelectedItem.Text.Trim() == "Machinery")
        {
            rfvMRegistration.Visible = false;
            rfvMSerialNo.Visible = true;
        }

    }

    protected void FunProAddMediumLIQSecurities()
    {

        DataRow dr;

        dtSecurities = (DataTable)ViewState[Securities.MediumLiquid.ToString()];


        for (int i = 0; i < dtSecurities.Rows.Count; i++)
        {
            DataRow drow = dtSecurities.Rows[i];
            if (ddlMCollSecurities.SelectedItem.Text.Trim() == "Vehicle" && txtMRegistrationNo.Text.Trim() == drow["Registration_No"].ToString())
            {
                Utility.FunShowAlertMsg(this.Page, "Registration No already existed");
                txtMRegistrationNo.Focus();
                return;
            }
            if (ddlMCollSecurities.SelectedItem.Text.Trim() == "Machinery" && txtMSerialNo.Text.Trim() == drow["Serial_No"].ToString())
            {
                Utility.FunShowAlertMsg(this.Page, "Serial No already existed");
                txtMSerialNo.Focus();
                return;
            }
        }
        if (dtSecurities.Rows.Count > 0)
        {
            if (dtSecurities.Rows[0]["SlNo"].ToString() == "0" || dtSecurities.Rows[0]["Collateral_Detail_ID"].ToString() == "")
            {
                dtSecurities.Rows[0].Delete();
            }
        }
        if (txtMRegistrationNo.Text == "" && txtMSerialNo.Text == "")
        {
            Utility.FunShowAlertMsg(this.Page, "Enter Registration No or Serial No");
            txtMRegistrationNo.Focus();
            return;
        }
        dr = dtSecurities.NewRow();

        dr["SlNo"] = (dtSecurities.Rows.Count + 1).ToString();
        dr["Collateral_Securities"] = ddlMCollSecurities.SelectedItem.Text.Trim();
        dr["Collateral_Detail_ID"] = ddlMCollSecurities.SelectedValue;
        dr["Description"] = txtMDescription.Text.Trim();
        dr["Model"] = txtMModel.Text.Trim();
        dr["Year"] = txtMYear.Text.Trim();
        dr["Registration_No"] = txtMRegistrationNo.Text.Trim();
        dr["Serial_No"] = txtMSerialNo.Text.Trim();
        dr["Value"] = txtMValue.Text.Trim() == "" ? "0" : txtMValue.Text.Trim();
        dr["Market_Value"] = txtMMarketValue.Text.Trim() == "" ? "0" : txtMMarketValue.Text.Trim();
        dr["Collateral_Ref_No"] = txtMCollateralRefNo.Text.Trim();
        dr["Scan_Ref_No"] = txtMScanRefNo.Text.Trim();
        dr["Collateral_Item_Ref_No"] = txtMItemRefNo.Text.Trim();
        dr["Ownership_Medium"] = txtMOwnership.Text.Trim() == "" ? "0" : txtMOwnership.Text.Trim();

        //dr["Is_Release"] = false;
        dtSecurities.Rows.Add(dr);
        gvMMedLiqDetails.DataSource = dtSecurities;
        gvMMedLiqDetails.DataBind();
        ViewState[Securities.MediumLiquid.ToString()] = dtSecurities;
    }

    //Medium

    //Low
    protected void btnAddL_Click(object sender, EventArgs e)
    {
        FunProAddLowLIQSecurities();
        FunClearLDetails();
    }

    protected void rdLSelect_CheckedChanged(object sender, EventArgs e)
    {
        btnModifyL.Enabled = true;
        btnAddL.Enabled = btnClearL.Enabled = false;

        int intRowIndex = Utility.FunPubGetGridRowID("gvLLowLiqDetails", ((RadioButton)sender).ClientID);
        dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];

        FunPriResetRdButton(gvCCommoDetails, intRowIndex);

        DataRow dr = dtSecurities.Rows[intRowIndex];

        lblLSlNo.Text = dr["SlNo"].ToString();
        ddlLCollSecurities.SelectedValue = dr["Collateral_Detail_ID"].ToString();
        txtLLocationDetails.Text = dr["Location_Details"].ToString();
        txtLMeasurement.Text = dr["Measurement"].ToString();
        txtLUnitRate.Text = dr["Unit_Rate"].ToString();
        txtLValue.Text = dr["Value"].ToString();
        txtLMarketValue.Text = dr["Market_Value"].ToString();
        txtLCollateralRefNo.Text = dr["Collateral_Ref_No"].ToString();
        txtLScanRefNo.Text = dr["Scan_Ref_No"].ToString();
        ddlLLegalOpinion.SelectedValue = dr["Legal_Opinion"].ToString() == "true" ? "1" : "0";
        ddlLLegalOpinion.SelectedValue = dr["Legal_OpinionDesc"] == "treu" ? "1" : "0";
        txtLLegalScanRefNo.Text = dr["Legal_Scan_Ref_No"].ToString();
        ddlLEncumbrance.SelectedValue = dr["Encumbrance"].ToString() == "true" ? "1" : "0";
        ddlLEncumbrance.SelectedValue = dr["EncumbranceDesc"].ToString() == "true" ? "1" : "0";
        txtLEncumbranceScanRefNo.Text = dr["Encumbrance_Scan_Ref_No"].ToString();
        ddlLAssetDocument.SelectedValue = dr["Is_Asset_Document"].ToString() == "true" ? "1" : "0";
        ddlLAssetDocument.SelectedValue = dr["Is_Asset_DocumentDesc"].ToString() == "true" ? "1" : "0";
        txtLAssetDocScanRefNo.Text = dr["AssetDoc_Scan_Ref_No"].ToString();
        ddlLValuationCertificate.SelectedValue = dr["Is_Valuation_Certification"] == "true" ? "1" : "0";
        ddlLValuationCertificate.SelectedValue = dr["Is_Valuation_CertificationDesc"] == "true" ? "1" : "0";
        txtLValCertificationScanRefNo.Text = dr["ValCertification_Scan_Ref_No"].ToString();
        txtLItemRefNo.Text = dr["Collateral_Item_Ref_No"].ToString();
        txtLOwnership.Text = dr["Ownership_Low"].ToString();

        //txtCollTransNo.Text.Trim();
        if (PageMode == PageModes.Query)
        {
            btnModifyL.Enabled = false;
        }

    }

    protected void btnModifyL_Click(object sender, EventArgs e)
    {
        FunLowLIQSecuritiesGridViewRowUpdating();
    }

    private void FunLowLIQSecuritiesGridViewRowUpdating()
    {

        dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];

        DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblLSlNo.Text) - 1];
        drow.BeginEdit();
        drow["SlNo"] = lblLSlNo.Text;
        drow["Collateral_Securities"] = ddlLCollSecurities.SelectedItem.Text;
        drow["Collateral_Detail_ID"] = ddlLCollSecurities.SelectedValue;
        drow["Location_Details"] = txtLLocationDetails.Text.Trim();
        drow["Measurement"] = txtLMeasurement.Text.Trim() == "" ? "0" : txtLMeasurement.Text.Trim();
        drow["Unit_Rate"] = txtLUnitRate.Text.Trim() == "" ? "0" : txtLUnitRate.Text.Trim();
        drow["Value"] = txtLValue.Text.Trim() == "" ? "0" : txtLValue.Text.Trim();
        drow["Market_Value"] = txtLMarketValue.Text.Trim() == "" ? "0" : txtLMarketValue.Text.Trim();
        drow["Collateral_Ref_No"] = txtLCollateralRefNo.Text.Trim();
        drow["Scan_Ref_No"] = txtLScanRefNo.Text.Trim();
        drow["Legal_Opinion"] = ddlLLegalOpinion.SelectedValue == "1" ? true : false;
        drow["Legal_OpinionDesc"] = ddlLLegalOpinion.SelectedItem.Text.Trim();
        drow["Legal_Scan_Ref_No"] = txtLLegalScanRefNo.Text.Trim();
        drow["Encumbrance"] = ddlLEncumbrance.SelectedValue == "1" ? true : false;
        drow["EncumbranceDesc"] = ddlLEncumbrance.SelectedItem.Text.Trim();
        drow["Encumbrance_Scan_Ref_No"] = txtLEncumbranceScanRefNo.Text.Trim();
        drow["Is_Asset_Document"] = ddlLAssetDocument.SelectedValue == "1" ? true : false;
        drow["Is_Asset_DocumentDesc"] = ddlLAssetDocument.SelectedItem.Text.Trim();
        drow["AssetDoc_Scan_Ref_No"] = txtLAssetDocScanRefNo.Text.Trim();
        drow["Is_Valuation_Certification"] = ddlLValuationCertificate.SelectedValue == "1" ? true : false;
        drow["Is_Valuation_CertificationDesc"] = ddlLValuationCertificate.SelectedItem.Text.Trim();
        drow["ValCertification_Scan_Ref_No"] = txtLValCertificationScanRefNo.Text.Trim();
        drow["Collateral_Item_Ref_No"] = txtLItemRefNo.Text.Trim();
        drow["Ownership_Low"] = txtLOwnership.Text.Trim() == "" ? "0" : txtLOwnership.Text.Trim();
        //drow["Is_Release"] = true;
        drow.EndEdit();

        ViewState[Securities.LowLiquid.ToString()] = dtSecurities;
        dtSecurities = (DataTable)ViewState[Securities.LowLiquid.ToString()];

        gvLLowLiqDetails.DataSource = (DataTable)ViewState[Securities.LowLiquid.ToString()];
        gvLLowLiqDetails.DataBind();

        FunClearLDetails();
        btnAddL.Enabled = btnClearL.Enabled = true;
    }

    private void FunClearLDetails()
    {
        lblLSlNo.Text = txtLLocationDetails.Text = txtLMeasurement.Text =
            txtLUnitRate.Text = txtLValue.Text = txtLMarketValue.Text = txtLCollateralRefNo.Text =
            txtLScanRefNo.Text = txtLLegalScanRefNo.Text = txtLEncumbranceScanRefNo.Text =
            txtLAssetDocScanRefNo.Text = txtLValCertificationScanRefNo.Text = txtLItemRefNo.Text = txtLOwnership.Text = "";

        ddlLCollSecurities.SelectedValue =
        ddlLLegalOpinion.SelectedValue =
        ddlLLegalOpinion.SelectedValue =
        ddlLEncumbrance.SelectedValue =
        ddlLEncumbrance.SelectedValue =
        ddlLAssetDocument.SelectedValue =
        ddlLAssetDocument.SelectedValue =
        ddlLValuationCertificate.SelectedValue =
        ddlLValuationCertificate.SelectedValue = "0";

        btnAddL.Enabled = btnClearL.Enabled = true;

        btnModifyL.Enabled = false;

        FunPriResetRdButton(gvLLowLiqDetails, -1);

    }

    protected void btnClearL_Click(object sender, EventArgs e)
    {
        FunClearLDetails();
        btnAddL.Enabled = true;
        btnModifyL.Enabled = false;
    }

    protected void FunProAddLowLIQSecurities()
    {
        DataRow dr;

        DataTable dt = (DataTable)ViewState[Securities.LowLiquid.ToString()];
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["SlNo"].ToString() == "0" || dt.Rows[0]["Collateral_Detail_ID"].ToString() == "")
            {
                dt.Rows[0].Delete();
            }
        }

        dr = dt.NewRow();
        dr["SlNo"] = dt.Rows.Count + 1;
        dr["Collateral_Securities"] = ddlLCollSecurities.SelectedItem.Text;
        dr["Collateral_Detail_ID"] = ddlLCollSecurities.SelectedValue;
        dr["Location_Details"] = txtLLocationDetails.Text.Trim();
        dr["Measurement"] = txtLMeasurement.Text.Trim() == "" ? "0" : txtLMeasurement.Text.Trim();
        dr["Unit_Rate"] = txtLUnitRate.Text.Trim() == "" ? "0" : txtLUnitRate.Text.Trim();
        dr["Value"] = txtLValue.Text.Trim() == "" ? "0" : txtLValue.Text.Trim();
        dr["Market_Value"] = txtLMarketValue.Text.Trim() == "" ? "0" : txtLMarketValue.Text.Trim();
        dr["Collateral_Ref_No"] = txtLCollateralRefNo.Text.Trim();
        dr["Scan_Ref_No"] = txtLScanRefNo.Text.Trim();
        dr["Legal_Opinion"] = ddlLLegalOpinion.SelectedValue == "1" ? true : false;
        dr["Legal_OpinionDesc"] = ddlLLegalOpinion.SelectedItem.Text.Trim();
        dr["Legal_Scan_Ref_No"] = txtLLegalScanRefNo.Text.Trim();
        dr["Encumbrance"] = ddlLEncumbrance.SelectedValue == "1" ? true : false;
        dr["EncumbranceDesc"] = ddlLEncumbrance.SelectedItem.Text.Trim();
        dr["Encumbrance_Scan_Ref_No"] = txtLEncumbranceScanRefNo.Text.Trim();
        dr["Is_Asset_Document"] = ddlLAssetDocument.SelectedValue == "1" ? true : false;
        dr["Is_Asset_DocumentDesc"] = ddlLAssetDocument.SelectedItem.Text.Trim();
        dr["AssetDoc_Scan_Ref_No"] = txtLAssetDocScanRefNo.Text.Trim();
        dr["Is_Valuation_Certification"] = ddlLValuationCertificate.SelectedValue == "1" ? true : false;
        dr["Is_Valuation_CertificationDesc"] = ddlLValuationCertificate.SelectedItem.Text.Trim();
        dr["ValCertification_Scan_Ref_No"] = txtLValCertificationScanRefNo.Text.Trim();
        dr["Collateral_Item_Ref_No"] = txtLItemRefNo.Text.Trim();
        dr["Ownership_Low"] = txtLOwnership.Text.Trim() == "" ? "0" : txtLOwnership.Text.Trim();
        //dr["Is_Release"] = true;
        dt.Rows.Add(dr);
        gvLLowLiqDetails.DataSource = dt;
        gvLLowLiqDetails.DataBind();
        ViewState[Securities.LowLiquid.ToString()] = dt;
    }

    private void FunPriSetAlignmentsLOWLIQSecurities()
    {
        txtLMeasurement.Style.Add("text-align", "right");
        txtLMeasurement.SetDecimalPrefixSuffix(8, 4, false, "Measurement");

        txtLUnitRate.Style.Add("text-align", "right");
        txtLUnitRate.SetDecimalPrefixSuffix(8, 4, false, "Unit Rate");

        txtLValue.Style.Add("text-align", "right");
        //txtValue.SetDecimalPrefixSuffix(8, 4, false, "Value");

        txtLMarketValue.Style.Add("text-align", "right");
        txtLMarketValue.SetDecimalPrefixSuffix(15, 4, false, "Market Value");

        txtLOwnership.Style.Add("text-align", "right");
        txtLOwnership.SetDecimalPrefixSuffix(3, 0, false, "Ownership_Low");

        txtLMeasurement.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtLMeasurement.ClientID + "','" + txtLUnitRate.ClientID + "','" + txtLValue.ClientID + "')");
        txtLUnitRate.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtLMeasurement.ClientID + "','" + txtLUnitRate.ClientID + "','" + txtLValue.ClientID + "')");
    }

    //Low

    //Commodities

    protected void btnAddC_Click(object sender, EventArgs e)
    {
        FunProAddCommLIQSecurities();
        FunClearCDetails();
    }

    protected void rdCSelect_CheckedChanged(object sender, EventArgs e)
    {
        btnModifyC.Enabled = true;
        btnAddC.Enabled = btnClearC.Enabled = false;

        int intRowIndex = Utility.FunPubGetGridRowID("gvCCommoDetails", ((RadioButton)sender).ClientID);
        dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];

        FunPriResetRdButton(gvCCommoDetails, intRowIndex);

        DataRow dr = dtSecurities.Rows[intRowIndex];

        lblCSlNo.Text = dr["SlNo"].ToString();
        txtCDescription.Text = dr["Description"].ToString();
        ddlCCollSecurities.SelectedValue = dr["Collateral_Detail_ID"].ToString();
        ddlCUnitOfMeasure.SelectedValue = dr["Unit_Of_Measure"].ToString();
        txtCUnitQty.Text = dr["Unit_Quantity"].ToString();
        txtCUnitPrice.Text = dr["Unit_Price"].ToString();
        txtCValue.Text = dr["Value"].ToString();
        txtCUnitMarketPrice.Text = dr["Unit_Market_Price"].ToString();
        txtCDate.Text = dr["Date"].ToString();
        txtCItemRefNo.Text = dr["Collateral_Item_Ref_No"].ToString();
        txtCOwnership.Text = dr["Ownership_Comm"].ToString();

        if (PageMode == PageModes.Query)
        {
            btnModifyC.Enabled = false;
        }

    }

    protected void btnModifyC_Click(object sender, EventArgs e)
    {
        FunCommLIQSecuritiesGridViewRowUpdating();
    }

    private void FunCommLIQSecuritiesGridViewRowUpdating()
    {

        dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];

        DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblCSlNo.Text) - 1];
        drow.BeginEdit();
        drow["Description"] = txtCDescription.Text.Trim();
        drow["Collateral_Securities"] = ddlCCollSecurities.SelectedItem.Text.Trim();
        drow["Collateral_Detail_ID"] = ddlCCollSecurities.SelectedValue;
        drow["Unit_Of_Measure"] = ddlCUnitOfMeasure.SelectedValue;
        drow["UOM_Text"] = ddlCUnitOfMeasure.SelectedItem.Text.Trim();
        drow["Unit_Quantity"] = txtCUnitQty.Text.Trim();
        drow["Unit_Price"] = txtCUnitPrice.Text.Trim() == "" ? "0" : txtCUnitPrice.Text.Trim();
        drow["Value"] = txtCValue.Text.Trim() == "" ? "0" : txtCValue.Text.Trim();
        drow["Unit_Market_Price"] = txtCUnitMarketPrice.Text.Trim() == "" ? "0" : txtCUnitMarketPrice.Text.Trim();
        drow["Date"] = txtCDate.Text.Trim();
        drow["Collateral_Item_Ref_No"] = txtCItemRefNo.Text.Trim();
        drow["Ownership_Comm"] = txtCOwnership.Text.Trim();
        //drow["Is_Release"] = true;
        drow.EndEdit();
        ViewState[Securities.Commodities.ToString()] = dtSecurities;
        dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];
        gvCCommoDetails.DataSource = dtSecurities;
        gvCCommoDetails.DataBind();

        FunClearCDetails();
        btnAddC.Enabled = btnClearC.Enabled = true;
    }

    private void FunClearCDetails()
    {
        lblCSlNo.Text =
        txtCDescription.Text =
        txtCUnitQty.Text =
        txtCUnitPrice.Text =
        txtCValue.Text =
        txtCUnitMarketPrice.Text =
        txtCDate.Text =
        txtCItemRefNo.Text = txtCOwnership.Text = "";
        ddlCCollSecurities.SelectedValue =
        ddlCUnitOfMeasure.SelectedValue = "0";

        btnAddC.Enabled = btnClearC.Enabled = true;

        btnModifyC.Enabled = false;

        FunPriResetRdButton(gvCCommoDetails, -1);

    }

    protected void btnClearC_Click(object sender, EventArgs e)
    {
        FunClearCDetails();
        btnAddC.Enabled = true;
        btnModifyC.Enabled = false;
    }

    protected void FunProAddCommLIQSecurities()
    {
        DataRow dr;
        dtSecurities = (DataTable)ViewState[Securities.Commodities.ToString()];

        if (dtSecurities.Rows.Count > 0)
        {
            if (dtSecurities.Rows[0]["SlNo"].ToString() == "0" || dtSecurities.Rows[0]["Collateral_Detail_ID"].ToString() == "")
            {
                dtSecurities.Rows[0].Delete();
            }
        }

        dr = dtSecurities.NewRow();
        dr["SlNo"] = dtSecurities.Rows.Count + 1;
        dr["Description"] = txtCDescription.Text.Trim();
        dr["Collateral_Detail_ID"] = ddlCCollSecurities.SelectedValue;
        dr["Collateral_Securities"] = ddlCCollSecurities.SelectedItem.Text.Trim();
        dr["Unit_Of_Measure"] = ddlCUnitOfMeasure.SelectedValue;
        dr["UOM_Text"] = ddlCUnitOfMeasure.SelectedItem.Text.Trim();
        dr["Unit_Quantity"] = txtCUnitQty.Text.Trim();
        dr["Unit_Price"] = txtCUnitPrice.Text.Trim() == "" ? "0" : txtCUnitPrice.Text.Trim();
        dr["Value"] = txtCValue.Text.Trim() == "" ? "0" : txtCValue.Text.Trim();
        dr["Unit_Market_Price"] = txtCUnitMarketPrice.Text.Trim() == "" ? "0" : txtCUnitMarketPrice.Text.Trim();
        dr["Date"] = txtCDate.Text.Trim();
        dr["Collateral_Item_Ref_No"] = txtCItemRefNo.Text.Trim();
        dr["Ownership_Comm"] = txtCOwnership.Text.Trim();
        //dr["Is_Release"] = true;

        dtSecurities.Rows.Add(dr);
        gvCCommoDetails.DataSource = dtSecurities;
        gvCCommoDetails.DataBind();
        ViewState[Securities.Commodities.ToString()] = dtSecurities;
    }

    private void FunPriSetAlignmentsCommLIQSecurities()
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        /***** LOAD Unit of Measure *************/
        Procparam.Add("@OPTION", "15");
        Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
        Procparam.Add("@UserID", Convert.ToString(intUserId));
        ddlCUnitOfMeasure.BindDataTable("S3G_CLT_LOADLOV", Procparam, new string[] { "LOOKUP_CODE", "LOOKUP_DESCRIPTION" });

        txtCUnitQty.Style.Add("text-align", "right");
        txtCUnitPrice.Style.Add("text-align", "right");
        txtCUnitPrice.SetDecimalPrefixSuffix(8, 4, false, "Unit Price");

        txtCOwnership.Style.Add("text-align", "right");
        txtCOwnership.SetDecimalPrefixSuffix(3, 0, false, "Ownership_Comm");

        txtCValue.Style.Add("text-align", "right");
        txtCValue.Attributes.Add("readonly", "readonly");
        txtCUnitMarketPrice.Style.Add("text-align", "right");
        txtCUnitMarketPrice.SetDecimalPrefixSuffix(8, 4, false, "Unit Market Price");

        CEDate.Format = strDateFormat;

        txtCUnitQty.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtCUnitQty.ClientID + "','" + txtCUnitPrice.ClientID + "','" + txtCValue.ClientID + "')");
        txtCUnitPrice.Attributes.Add("onkeyup", "javascript:return fnGetValue('" + txtCUnitQty.ClientID + "','" + txtCUnitPrice.ClientID + "','" + txtCValue.ClientID + "')");
    }

    //Commodities Sec

    //Finance

    protected void btnAddF_Click(object sender, EventArgs e)
    {
        FunProAddFinLIQSecurities();
        FunClearFDetails();
    }

    protected void rdFSelect_CheckedChanged(object sender, EventArgs e)
    {
        btnModifyF.Enabled = true;
        btnAddF.Enabled = btnClearF.Enabled = false;

        int intRowIndex = Utility.FunPubGetGridRowID("gvFFinDetails", ((RadioButton)sender).ClientID);
        dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];

        FunPriResetRdButton(gvFFinDetails, intRowIndex);

        DataRow dr = dtSecurities.Rows[intRowIndex];

        lblFSlNo.Text = dr["SlNo"].ToString();
        ddlFCollSecurities.SelectedValue = dr["Collateral_Detail_ID"].ToString();
        txtFInsuranceIssuedBy.Text = dr["Insurance_Issued_By"].ToString();
        txtFPolicyNo.Text = dr["Policy_No"].ToString();
        txtFPolicyValue.Text = dr["Policy_Value"].ToString();
        txtFCurrentValue.Text = dr["Current_Value"].ToString();
        txtFMaturityDate.Text = dr["Maturity_Date"].ToString();
        txtFCollateralRef.Text = dr["Collateral_Ref_No"].ToString();
        txtFItemRefNo.Text = dr["Collateral_Item_Ref_No"].ToString();
        txtFScanRef.Text = dr["Scan_Ref_No"].ToString();
        txtFOwnership.Text = dr["Ownership_Fin"].ToString();


        if (PageMode == PageModes.Query)
        {
            btnModifyF.Enabled = false;
        }

    }

    protected void btnModifyF_Click(object sender, EventArgs e)
    {
        FunFinLIQSecuritiesGridViewRowUpdating();
    }

    private void FunFinLIQSecuritiesGridViewRowUpdating()
    {

        dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];

        DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblFSlNo.Text) - 1];

        drow.BeginEdit();
        drow["Collateral_Detail_ID"] = ddlFCollSecurities.SelectedValue;
        drow["Collateral_Securities"] = ddlFCollSecurities.SelectedItem.Text.Trim();
        drow["Insurance_Issued_By"] = txtFInsuranceIssuedBy.Text.Trim();
        drow["Policy_No"] = txtFPolicyNo.Text.Trim();
        drow["Policy_Value"] = txtFPolicyValue.Text.Trim();
        drow["Current_Value"] = txtFCurrentValue.Text.Trim() == "" ? "0" : txtFCurrentValue.Text.Trim();
        drow["Maturity_Date"] = txtFMaturityDate.Text.Trim();
        drow["Collateral_Ref_No"] = txtFCollateralRef.Text.Trim();
        drow["Scan_Ref_No"] = txtFScanRef.Text.Trim();
        drow["Collateral_Item_Ref_No"] = txtFItemRefNo.Text.Trim();
        drow["Ownership_Fin"] = txtFOwnership.Text.Trim() == "" ? "0" : txtFOwnership.Text.Trim();
        //drow["Is_Release"] = true;
        drow.EndEdit();
        ViewState[Securities.Financial.ToString()] = dtSecurities;

        dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];

        gvFFinDetails.DataSource = (DataTable)ViewState[Securities.Financial.ToString()];
        gvFFinDetails.DataBind();

        FunClearFDetails();
        btnAddF.Enabled = btnClearF.Enabled = true;
    }

    private void FunClearFDetails()
    {
        lblFSlNo.Text =
        txtFInsuranceIssuedBy.Text =
        txtFPolicyNo.Text =
        txtFPolicyValue.Text =
        txtFCurrentValue.Text =
        txtFMaturityDate.Text =
        txtFCollateralRef.Text =
        txtFScanRef.Text =
        txtFItemRefNo.Text = txtFOwnership.Text = "";
        ddlFCollSecurities.SelectedValue = "0";

        btnAddF.Enabled = btnClearF.Enabled = true;

        btnModifyF.Enabled = false;

        FunPriResetRdButton(gvFFinDetails, -1);

    }

    protected void btnClearF_Click(object sender, EventArgs e)
    {
        FunClearFDetails();
        btnAddF.Enabled = true;
        btnModifyF.Enabled = false;
    }

    protected void FunProAddFinLIQSecurities()
    {
        DataRow dr;
        dtSecurities = (DataTable)ViewState[Securities.Financial.ToString()];

        if (dtSecurities.Rows.Count > 0)
        {
            if (dtSecurities.Rows[0]["SlNo"].ToString() == "0" || dtSecurities.Rows[0]["Collateral_Detail_ID"].ToString() == "")
            {
                dtSecurities.Rows[0].Delete();
            }
        }

        dr = dtSecurities.NewRow();

        dr["SlNo"] = dtSecurities.Rows.Count + 1;
        dr["Collateral_Detail_ID"] = ddlFCollSecurities.SelectedValue;
        dr["Collateral_Securities"] = ddlFCollSecurities.SelectedItem.Text;
        dr["Insurance_Issued_By"] = txtFInsuranceIssuedBy.Text.Trim();
        dr["Policy_No"] = txtFPolicyNo.Text.Trim();
        dr["Policy_Value"] = txtFPolicyValue.Text.Trim();
        dr["Current_Value"] = txtFCurrentValue.Text.Trim() == "" ? "0" : txtFCurrentValue.Text.Trim();
        dr["Maturity_Date"] = txtFMaturityDate.Text.Trim();
        dr["Collateral_Ref_No"] = txtFCollateralRef.Text.Trim();
        dr["Scan_Ref_No"] = txtFScanRef.Text.Trim();
        dr["Collateral_Item_Ref_No"] = txtFItemRefNo.Text.Trim();
        dr["Ownership_Fin"] = txtFOwnership.Text.Trim() == "" ? "0" : txtFOwnership.Text.Trim();
        //dr["Is_Release"] = true;
        dtSecurities.Rows.Add(dr);
        gvFFinDetails.DataSource = dtSecurities;
        gvFFinDetails.DataBind();
        ViewState[Securities.Financial.ToString()] = dtSecurities;
    }

    private void FunPriSetAlignmentsFinLIQSecurities()
    {
        txtFPolicyValue.Style.Add("text-align", "right");
        txtFPolicyValue.SetDecimalPrefixSuffix(8, 4, false, "Policy Value");

        txtFCurrentValue.Style.Add("text-align", "right");
        txtFCurrentValue.SetDecimalPrefixSuffix(8, 4, false, "Current Value");

        txtFOwnership.Style.Add("text-align", "right");
        txtFOwnership.SetDecimalPrefixSuffix(3, 0, false, "Ownership_Fin");




        CEFMaturityDate.Format = strDateFormat;
    }

    //Finance

    protected void ddlhDemat_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlhDemat.SelectedValue == "0")
        {
            FunProSetHDematToggle(false);
        }
        else
        {
            FunProSetHDematToggle(true);
        }
    }

    protected void FunProSetHDematToggle(bool blEnable)
    {
        if (!blEnable)
        {
            txthDPNo.Text = "";
            txthClientID.Text = "";
            txthDPName.Text = "";

            lblhCertificateNo.CssClass = "styleReqFieldLabel";
            lblhDPNo.CssClass = "";
            lblhDPName.CssClass = "";
            lblhClientID.CssClass = "";

        }
        else
        {
            txthCertificateNo.Text = "";

            lblhCertificateNo.CssClass = "";
            lblhDPNo.CssClass = "styleReqFieldLabel";
            lblhDPName.CssClass = "styleReqFieldLabel";
            lblhClientID.CssClass = "styleReqFieldLabel";
        }

        rfvhDPNo.Enabled = rfvhClientID.Enabled = rfvhDPName.Enabled = blEnable;
        rfvhCertificateNo.Enabled = !blEnable;

        txthDPNo.ReadOnly = txthClientID.ReadOnly = txthDPName.ReadOnly = !blEnable;
        txthCertificateNo.ReadOnly = blEnable;

    }

    protected void grvprimeaccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            Label lblSelect = (Label)e.Row.FindControl("lblSelect");
            lblSelect.Text = "Select " + ddlRefPoint.SelectedItem.Text;
        }
    }
}






