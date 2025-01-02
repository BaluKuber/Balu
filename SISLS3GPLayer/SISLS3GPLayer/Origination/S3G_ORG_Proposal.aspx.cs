#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Proposal
/// Created By			: Saran M
/// Created Date		: 22-August-2014
/// Purpose	            : 


/// Modified By			: Swarna S
/// Created Date		: 05-Jan-2015
/// Purpose	            : Modify Fee Based on and PG,SG changes.
/// 
/// 
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Globalization;
using System.Data;
using S3GBusEntity.Origination;
using System.Web.Security;
using System.Configuration;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Globalization;
using System.Threading;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
#endregion


public partial class Origination_S3G_ORG_Proposal : ApplyThemeForProject
{

    #region Variable declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    Dictionary<string, string> Procparam;
    string _Add = "1", _Edit = "2", _Query = "3";
    int _SlNo = 0;
    // bool PaintBG = false;
    int intCompany_Id, intEnqNewCustomerId;
    int intUserId;
    int intResult;
    int intPricingId;
    string strPricingId;
    Int64 intLeadID;

    string strMode;
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    DataTable dtPrimaryGrid = new DataTable();
    DataTable dtRebateDetGrid = new DataTable();
    DataTable dtSecondaryGrid = new DataTable();
    DataTable dtEUCDetails = new DataTable();
    DataTable dtAssetCategorytbl = new DataTable();
    DataTable DtRepayGrid = new DataTable();
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bClearList = false;


    double intAssetamount = 0;
    public string strDateFormat;
    public string strCustomer_Id = string.Empty;
    public string strCustomer_Value = string.Empty;
    public string strCustomer_Name = string.Empty;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    string strRedirectPageView = "window.location.href='../Origination/S3GORGTransLander.aspx?Code=ORPRC';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_Proposal.aspx?qsMode=C';";
    string strRedirectPage = "~/Origination/SS3GORGTransLander.aspx?Code=ORPRC";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";

    string strNewWinEMIIFrm = "S3GEMICalculator.aspx";

    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    PricingMgtServicesReference.PricingMgtServicesClient ObjPricingMgtServices;
    PricingMgtServices.S3G_ORG_PricingDataTable ObjS3G_ORG_Pricing;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;
    string strPageName = "Proposal";
    public static Origination_S3G_ORG_Proposal obj_PageValue;
    #endregion

    #region "Page Events"

    /// <summary>
    /// Page Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //ifrmCRM.Attributes["src"] = "../Common/HomePage.aspx";
            FunPriLoadPage();
            //txtSecDepAdvRent.SetPercentagePrefixSuffix(3, 5, false, false, "Sec Dep/Adv Rent %");
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("hi-IN");
            //this.txtTotalFacilityAmount.TextChanged += new System.EventHandler(this.txtTotalFacilityAmount_TextChanged);

        }
        catch (Exception ex)
        {
            CVProposal.ErrorMessage = "Due to Data Problem, Unable to Load the Proposal.";
            CVProposal.IsValid = false;
        }
    }

    #region "EUC Dtls Code"

    /// <summary>
    /// This event is used to delete the End use customer records in grid level 
    /// it will call user defined function FunPriRemoveEUCDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvEUC_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveEUCDetails(e.RowIndex);
            FunPriClearEUC();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to delete End use Customer Details";
            CVProposal.IsValid = false;
        }
    }

    /// <summary>
    /// This event will fire once we need to add the entered End use details in grid using user defined function FunPriAddEUCDtls.
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddEUC_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriAddEUCDtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to Add End use Customer Details";
            CVProposal.IsValid = false;
        }

    }

    /// <summary>
    /// This event is used to modify the selected End use customer records using user defind function FunPriModifyEUCdtls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnModifyEUC_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriModifyEUCdtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to Modify End use Customer Details";
            CVProposal.IsValid = false;
        }
    }

    /// <summary>
    /// This event is used to clear the end use customer controls using user defind function FunPriClearEUC
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClearInt_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearEUC();
            btnAddEUC.Enabled = true;
            btnModifyEUC.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to Clear End use Customer Details";
            CVProposal.IsValid = false;
        }
    }

    /// <summary>
    /// This event is used to select the End use Customer list for Display using user defined function FunPriRBSelectIndexChange
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RBSelectInt_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriRBSelectIndexChange(sender);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to Select End use Customer Details";
            CVProposal.IsValid = false;
        }

    }

    #endregion

    #region "TextBox Events"

    /// <summary>
    /// This event is used to do action related to Tenure change  in primary grid
    /// using user defined function FunPriTenurePG_OnTextChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtTenurePG_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriTenurePG_OnTextChanged();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    /// <summary>
    /// This event is used to do action related to Tenure change in secondary grid
    /// using user defined function FunPriTenureSG_OnTextChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void txtTenureSG_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriTenureSG_OnTextChanged();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    #endregion

    #region "DropDownList Events"

    /// <summary>
    /// This event is used to do action related to Asset category change in primary grid
    /// using user defined function FunPriAssetCategoryPG_SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetCategoryPG_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriAssetCategoryPG_SelectedIndexChanged();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    /// <summary>
    /// This event is used to do action related to Asset category change in secondary grid
    /// using user defined function FunPriAssetCategorySG_SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAssetCategorySG_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriAssetCategorySG_SelectedIndexChanged();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    /// <summary>
    /// This event is used to do action related to Asset category change in secondary grid
    /// using user defined function FunPriAssetCategorySG_SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRentFreqPG_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            funPriSetTenure_Freq_onchange("PG");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }
    /// <summary>
    /// This event is used to do action related to Asset category change in secondary grid
    /// using user defined function FunPriAssetCategorySG_SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRentFreqSG_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            funPriSetTenure_Freq_onchange("SG");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }


    /// <summary>
    /// This event is used to do action related to Security deposit change
    /// using user defined function FunPriSecuritydeposit_SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSecuritydeposit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriSecuritydeposit_SelectedIndexChanged();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    /// <summary>
    /// This event is used to do action related to Proposal Type change
    /// using user defined function FunPriProposalType_SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProposalType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriProposalType_SelectedIndexChanged();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    #endregion

    #region "Radiobutton Events"

    /// <summary>
    /// This event is used call at the time of change in secondary term radiobutton 
    /// and it will call the user defined funcition to do secondary grid changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RBLSecondaryTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriRBLSecondaryTerm_SelectedIndexChanged();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    /// <summary>
    /// This event is used call at the time of change in secondary term radiobutton 
    /// and it will call the user defined funcition to do secondary grid changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RBLAdvanceRent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriRBLAdvanceRent_SelectedIndexChanged();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }
    /// <summary>
    /// This event is used call at the time of change in secondary term radiobutton 
    /// and it will call the user defined funcition to do secondary grid changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RBLStructuredEI_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriRBLStructuredEI_SelectedIndexChanged();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    #endregion


    #region "Grid events"

    /// <summary>
    /// This event is used call at the time of primary grid insert in grvPrimaryGrid 
    /// using user defined funcition FunPriAddgrvPrimaryGridDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvPrimaryGrid_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                FunPriAddgrvPrimaryGridDetails();
                ifrmCRM.ResolveUrl("S3GEMICalculator.aspx?IsFromProposal=Yes");

                //ifrmCRM.Attributes["src"] = "../Common/HomePage.aspx";
            }
            FunDisableGridReturnBased();
        }
        catch (Exception ex)
        {
            CVProposal.ErrorMessage = "Unable To add Due to Data Problem";
            CVProposal.IsValid = false;
        }
    }

    /// <summary>
    /// This event is used call at the time of primary grid delete in grvPrimaryGrid 
    /// using user defined funcition FunPriRemovegrvPrimaryGridDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvPrimaryGrid_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemovegrvPrimaryGridDetails(e.RowIndex);
            ifrmCRM.ResolveUrl("S3GEMICalculator.aspx?IsFromProposal=Yes");

        }
        catch (Exception ex)
        {
            CVProposal.ErrorMessage = "Unable To delete Due to Data Problem";
            CVProposal.IsValid = false;
        }
    }

    /// <summary>
    /// This event is used call at the time of Secondary grid insert in grvSecondaryGrid 
    /// using user defined funcition FunPriAddgrvSecondaryGridDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvSecondaryGrid_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                FunPriAddgrvSecondaryGridDetails();
            }
            FunDisableGridReturnBased();
        }
        catch (Exception ex)
        {
            CVProposal.ErrorMessage = "Unable To add Due to Data Problem";
            CVProposal.IsValid = false;
        }
    }


    /// <summary>
    /// This event is used call at the time of Secondary grid delete in grvSecondaryGrid 
    /// using user defined funcition FunPriRemovegrvSecondaryGridDetails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvSecondaryGrid_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemovegrvSecondaryGridDetails(e.RowIndex);

        }
        catch (Exception ex)
        {
            CVProposal.ErrorMessage = "Unable To delete Due to Data Problem";
            CVProposal.IsValid = false;
        }
    }


    #endregion

    #region "Button events"
    /// <summary>
    /// this event is used to load customer address using user defined function FunPriGetCustomerAddress by passing customer Id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {

            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null)
            {
                if (hdnCustomerId.Value != "")
                {
                    hdnCustID.Value = hdnCustomerId.Value;
                    FunPriGetCustomerAddress(Convert.ToInt32(hdnCustomerId.Value));
                    lnkViewCustomer.Enabled = true;
                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get customer details";
            CVProposal.IsValid = false;
        }
    }

    /// <summary>
    /// This button click event is used to save proposal details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveProposalDtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to save pricing details";
            CVProposal.IsValid = false;
        }
    }


    /// <summary>
    /// This button click event is used to clear proposal details using user defined function FunPriClearProposalDtls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearProposalDtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to clear pricing details";
            CVProposal.IsValid = false;
        }
    }

    /// <summary>
    /// This button click event is used to cancel proposal details using user defined function FunPriCancelProposal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnWithdraw_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriCancelProposal();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to Cancel/Revive proposal details";
            CVProposal.IsValid = false;
        }
    }

    /// <summary>
    /// This button click event is used to redirect to view page 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FunProClearCachedFiles();
        if (Request.QueryString["qsLeadId"] != null)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "window.close();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "window.parent.document.getElementById('ctl00_ContentPlaceHolder1_btnFrameCancel').click()", true);

            //FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hdnCustID.Value, false, 0);
            //Response.Redirect("S3G_ORG_CRM_ADD.aspx?qsCustomer=" + FormsAuthentication.Encrypt(Ticket));
            Response.Redirect("~/Origination/S3GOrgCRM_View.aspx?Code=CRM");
        }
        else if (Request.QueryString["Popup"] != null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Close", "window.close();", true);
        }
        else if (PageMode == PageModes.WorkFlow)
        {
            ReturnToWFHome();
        }
        else
        {
            Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=ORPRC");
        }
    }

    #endregion

    protected void lnkViewCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            if ((HiddenField)ucCustomerCodeLov.FindControl("hdnID") != null)
            {
                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(Convert.ToString(hdnCustomerId.Value), false, 0);
                string strViewPage = "../Origination/S3GOrgCustomerMaster_Add.aspx?qsCustomerId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&IsFromApplication=yes";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + strViewPage + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
                return;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    #endregion

    #region "Page Methods"

    /// <summary>
    /// This method will execute when page Loads
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            ObjUserInfo = new UserInfo();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            calExeOfferValidTill.Format = calExeTermSheetDate.Format =
                CEtxtOfferDate.Format = strDateFormat;

            obj_PageValue = this;
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket formTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                strMode = Request.QueryString.Get("qsMode");
                if (formTicket != null)
                {
                    strPricingId = formTicket.Name;
                    intPricingId = Convert.ToInt32(formTicket.Name);
                }
            }

            if (Request.QueryString["qsLeadId"] != null)
            {
                FormsAuthenticationTicket formTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsLeadId"));
                if (formTicket != null)
                {
                    intLeadID = Convert.ToInt64(formTicket.Name);
                }
            }


            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            intCompany_Id = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;

            if (ddlAmtBasedOn.Enabled == false)
            {
                rfvOneTimeBasedOn.Enabled = false;
            }
            if (ddlProcessingBasedOn.Enabled == false)
            {
                rfvProcessingBasedon.Enabled = false;
            }
            flUpload.Attributes.Add("onchange", "fnAssignPath('" + flUpload.ClientID + "','" + hdnSelectedPath.ClientID + "'); fnLoadPath('" + btnBrowse.ClientID + "');");
            btnDlg.OnClientClick = "fnLoadPath('" + flUpload.ClientID + "');";
            if (!IsPostBack)
            {
                Session.Remove("dtPrimaryGrid");
                ViewState["Document_Path"] = "";
                txtOfferDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtOfferDate.ClientID + "','" + strDateFormat + "',true,  false);");
                txtOfferValidTill.Attributes.Add("onblur", "fnDoDate(this,'" + txtOfferValidTill.ClientID + "','" + strDateFormat + "',false,  true);");
                txtTermSheetDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtTermSheetDate.ClientID + "','" + strDateFormat + "',true,  false);");
                FunLoadLOBMaster();
                FunLoadDDLControls();
                FunPriEmptyPrimaryGrid();
                FunPriEmptyRebateDetGrid();
                FunPriEmptySecondaryGrid();
                FunPriSetEmptyEUCtbl();
                if ((intPricingId > 0) && (strMode == "M"))                  // Modify
                {
                    FunPriDisableControls(1);
                }
                else if (strMode == "Q")                                        //((strPricingId != "") && (strMode == "Q")) // Query 
                {
                    FunPriDisableControls(-1);
                }
                else                                                           //Create Mode
                {
                    FunPriDisableControls(0);
                }
                //Response.Redirect(strNewWinEMIIFrm);


                FunProClearCachedFiles();
            }
            FunPriSetMaxLength();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //To disable controls based on Create/Modify/Query
    /// <summary>
    ///This method is used to Disable the controls based on Create/Modify/Query Mode. 
    ///Here argument is used as intModeID to differentiate the Modes.
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        try
        {

            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    lblRoundNo.Text = "0";//For create Mode
                    txtOfferDate.Text = DateTime.Now.Date.ToString(ObjS3GSession.ProDateFormatRW); ;
                    pnlSecondaryGrid.Enabled = false; lnkViewCustomer.Enabled = false;
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    btnWithdraw.Visible = false;
                    BtnRevive.Visible = false;

                    if (Convert.ToInt64(intLeadID) > 0)
                    {
                        FunPriLoadLeadDtls(intLeadID);
                    }

                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    lnkViewCustomer.Enabled = true;
                    FunPriGetProposalDtls(strPricingId);
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                    }
                    btnClear.Visible = false;

                    //If transaction has been approved or cancelled
                    btnWithdraw.Visible = true;
                    BtnRevive.Visible = false;

                    if (ViewState["Status_ID"] != null)
                    {
                        if (ViewState["Status_ID"].ToString() != "44")//pending
                        {
                            QueryView();
                            if (ViewState["Status_ID"].ToString() == "120")//cancelled
                            {
                                BtnRevive.Visible = true;
                                btnWithdraw.Visible = false;
                                //btnWithdraw.Text = "Revive Proposal";
                                //btnWithdraw.Attributes.Add("OnClick", "return Confirmmsg('Do you want to revive Proposal?'); ");
                            }
                        }
                    }
                    //
                    if (ddlRentalBasedOn.SelectedValue == "1")
                    {
                        pnlRebateDet.Enabled = pnlRebateStruc.Enabled = false;
                        rfvRebateDiscountApp.Enabled = rfvRebateNoofInstall.Enabled = rfvRebateStructuredEI.Enabled =
                            rfvRebateDiscountPerc.Enabled = rfvAddiRebateDiscountPerc.Enabled = false;
                    }
                    break;


                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    FunPriGetProposalDtls(strPricingId);
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }

                    QueryView();
                    lnkViewCustomer.Enabled = true;
                    flUpload.Enabled = false;
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    /// This method is used to disable/Enable the controls for query mode
    /// </summary>
    private void QueryView()
    {
        try
        {
            btnSave.Visible = btnClear.Visible = false;
            ucCustomerCodeLov.Visible = false;
            imgCalValidpDate.Visible = ImgtxtOfferDate.Visible = false;
            CEtxtOfferDate.Enabled = calExeOfferValidTill.Enabled = false;
            txtProposalNumber.ReadOnly = txtOneTimeAmount.ReadOnly = txtOnetimeRate.ReadOnly =
            txtProcessingAmount.ReadOnly = txtProcessingRate.ReadOnly =

                //txtOfferDate.Text = 
                txtProcessingFee.ReadOnly =
                txtRemarks.ReadOnly =
                txtOneTimeFee.ReadOnly =
                txtTotalFacilityAmount.ReadOnly =
                txtSecDepAdvRent.ReadOnly = txtTermSheetDate.ReadOnly =
                txtOfferValidTill.ReadOnly = true;

            txtOfferDate.ReadOnly = txtGuaranteedEOT.ReadOnly = true;

            ddlAmtBasedOn.Enabled = false;
            ddlProcessingBasedOn.Enabled = false;
            ddlBranchList.Enabled = false;
            ddlProposalType.ClearDropDownList();
            ddlSecuritydeposit.ClearDropDownList();
            RBLAdvanceRent.Enabled = RBLSecondaryTerm.Enabled =
            //RBLStructuredEI.Enabled = RBLVATRebate.Enabled = false;
            RBLStructuredEI.Enabled = false;
            ddlReturnPattern.Enabled = false;
            ddlRentalBasedOn.Enabled = ddlSecondaryRentalBasedOn.Enabled = false;
            if (grvPrimaryGrid.FooterRow != null)
                grvPrimaryGrid.FooterRow.Visible = false;
            if (grvSecondaryGrid.FooterRow != null)
                grvSecondaryGrid.FooterRow.Visible = false;

            grvPrimaryGrid.Columns[grvPrimaryGrid.Columns.Count - 1].Visible = false;
            grvSecondaryGrid.Columns[grvPrimaryGrid.Columns.Count - 1].Visible = false;
            grvEUC.Columns[grvEUC.Columns.Count - 1].Visible = false;
            btnAddEUC.Visible = btnModifyEUC.Visible = btnhClearEUC.Visible = false;
            btnWithdraw.Visible = false;
            BtnRevive.Visible = false;
            sugLeadRefNo.Enabled = false;
            ddlMRANumber.ClearDropDownList();
            //TabDANPricing.Enabled = false;

            ddlRebateStrucAlloc.ClearDropDownList();
            grvRebateDetGrid.Columns[5].Visible = false;
            grvRebateDetGrid.FooterRow.Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to load the proposal details based on the proposal id
    /// </summary>
    /// <param name="strPricingId"></param>
    private void FunPriGetProposalDtls(string strPricingId)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Pricing_ID", strPricingId);
            Procparam.Add("@COMPANY_ID", intCompany_Id.ToString());
            DataSet ds_PricingDetails = Utility.GetDataset("S3G_ORG_Get_Proposal_Dtls", Procparam);
            if (ds_PricingDetails != null)
            {
                //            Company_ID,Customer_ID,Business_Offer_Number
                //,dbo.GetUserDateFormat(Offer_Date,@Company_Id) as Offer_Date
                //,Facility_Amount
                //,dbo.GetUserDateFormat(Offer_Valid_Till,@Company_Id) as Offer_Valid_Till
                //,LOB_ID,Location_Code,Location_ID,Proposal_Type,Lead_ID,Adv_Rent_Applicability,Secu_Deposit_Type,Secu_Rent_Amount
                //,ReturnPattern,Seco_Term_Applicability,  
                //One_Time_Fee,Repayment_Mode,Processing_Fee_Per,VAT_Rebate_Applicability,Remarks,Product_ID,Constitution_ID,Accounting_IRR,  
                //Status_ID,Round_No,Created_By,Created_On

                DataTable dtProposal = ds_PricingDetails.Tables[0];

                #region "Header Part"
                if (dtProposal.Rows.Count > 0)
                {

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Customer_ID"].ToString()))
                    {
                        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

                        hdnCustID.Value = hdnCustomerId.Value = dtProposal.Rows[0]["Customer_ID"].ToString();
                        FunPriGetCustomerAddress(Convert.ToInt32(hdnCustomerId.Value));
                    }

                    ddlMRANumber.SelectedValue = dtProposal.Rows[0]["MRA_ID"].ToString();//8968

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Business_Offer_Number"].ToString()))
                        txtProposalNumber.Text = dtProposal.Rows[0]["Business_Offer_Number"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Offer_Date"].ToString()))
                        txtOfferDate.Text = dtProposal.Rows[0]["Offer_Date"].ToString();
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Facility_Amount"].ToString()))
                    {
                        txtTotalFacilityAmount.Text = dtProposal.Rows[0]["Facility_Amount"].ToString();
                        //txtTotalFacilityAmount.Text = string.Format("{0:C}", Convert.ToDouble(txtTotalFacilityAmount.Text)).Replace("रु ", "").Replace("$", "");
                    }
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Offer_Valid_Till"].ToString()))
                        txtOfferValidTill.Text = dtProposal.Rows[0]["Offer_Valid_Till"].ToString();
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Term_Sheet_Date"].ToString()))
                        txtTermSheetDate.Text = dtProposal.Rows[0]["Term_Sheet_Date"].ToString();
                    if (dtProposal.Rows[0]["Location_ID"].ToString() != "0")
                    {
                        ddlBranchList.SelectedValue = dtProposal.Rows[0]["Location_ID"].ToString();
                        ddlBranchList.SelectedText = dtProposal.Rows[0]["Location"].ToString();
                    }
                    else
                    {
                        ddlBranchList.SelectedValue = "0";
                    }
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Proposal_Type"].ToString()))
                        ddlProposalType.SelectedValue = dtProposal.Rows[0]["Proposal_Type"].ToString();
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Adv_Rent_Applicability"].ToString()))
                        RBLAdvanceRent.SelectedValue = dtProposal.Rows[0]["Adv_Rent_Applicability"].ToString();
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Seco_Term_Applicability"].ToString()))
                        RBLSecondaryTerm.SelectedValue = dtProposal.Rows[0]["Seco_Term_Applicability"].ToString();
                    ViewState["Seco_Term_Applicability"] = RBLSecondaryTerm.SelectedValue;

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Secu_Deposit_Type"].ToString()))
                        ddlSecuritydeposit.SelectedValue = dtProposal.Rows[0]["Secu_Deposit_Type"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["ReturnPattern"].ToString()))
                        ddlReturnPattern.SelectedValue = dtProposal.Rows[0]["ReturnPattern"].ToString();


                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Amt_Based_On"].ToString()))
                        ddlAmtBasedOn.SelectedValue = dtProposal.Rows[0]["Amt_Based_On"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["PROCESSING_AMT_BASED_ON"].ToString()))
                        ddlProcessingBasedOn.SelectedValue = dtProposal.Rows[0]["PROCESSING_AMT_BASED_ON"].ToString();

                    txtOnetimeRate.Text = dtProposal.Rows[0]["ONE_TIME_RATE"].ToString();
                    txtOneTimeAmount.Text = dtProposal.Rows[0]["One_Time_Fee"].ToString();

                    txtProcessingRate.Text = dtProposal.Rows[0]["PROCESSING_FEE_RATE"].ToString();
                    txtProcessingAmount.Text = dtProposal.Rows[0]["PROCESSING_FEE_PER"].ToString();

                    if (txtOnetimeRate.Text != "" && txtOneTimeAmount.Text == "")
                    {
                        ddlAmtBasedOn.Enabled = true;
                    }
                    else
                    {
                        ddlAmtBasedOn.Enabled = false;
                    }

                    if (txtProcessingRate.Text != "" && txtProcessingAmount.Text == "")
                    {
                        ddlProcessingBasedOn.Enabled = true;
                    }
                    else
                    {
                        ddlProcessingBasedOn.Enabled = false;
                    }


                    if (txtOnetimeRate.Text == "" && txtOneTimeAmount.Text == "")
                    {
                        txtOnetimeRate.ReadOnly = false;
                        txtOneTimeAmount.ReadOnly = false;
                    }
                    else if (txtOnetimeRate.Text != "" && txtOneTimeAmount.Text == "")
                    {
                        txtOnetimeRate.ReadOnly = false;
                        txtOneTimeAmount.ReadOnly = true;
                    }
                    else if (txtOnetimeRate.Text == "" && txtOneTimeAmount.Text != "")
                    {
                        txtOnetimeRate.ReadOnly = true;
                        txtOneTimeAmount.ReadOnly = false;
                    }

                    if (txtProcessingRate.Text == "" && txtProcessingAmount.Text == "")
                    {
                        txtProcessingRate.ReadOnly = false;
                        txtProcessingAmount.ReadOnly = false;
                    }
                    else if (txtProcessingRate.Text != "" && txtProcessingAmount.Text == "")
                    {
                        txtProcessingRate.ReadOnly = false;
                        txtProcessingAmount.ReadOnly = true;
                    }
                    else if (txtProcessingRate.Text == "" && txtProcessingAmount.Text != "")
                    {
                        txtProcessingRate.ReadOnly = true;
                        txtProcessingAmount.ReadOnly = false;
                    }

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Repayment_Mode"].ToString()))
                        RBLStructuredEI.SelectedValue = dtProposal.Rows[0]["Repayment_Mode"].ToString();
                    //if (!string.IsNullOrEmpty(dtProposal.Rows[0]["VAT_Rebate_Applicability"].ToString()))
                    //RBLVATRebate.SelectedValue = dtProposal.Rows[0]["VAT_Rebate_Applicability"].ToString();
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Remarks"].ToString()))
                        txtRemarks.Text = dtProposal.Rows[0]["Remarks"].ToString();
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Round_No"].ToString()))
                        lblRoundNo.Text = dtProposal.Rows[0]["Round_No"].ToString();
                    else
                        lblRoundNo.Text = "0";

                    //opc058 start
                    rblIs_Guaranteed_EOT_Appli.SelectedValue = dtProposal.Rows[0]["Is_Guaranteed_EOT_Appli"].ToString();

                    rblRebateDiscountApp.SelectedValue = dtProposal.Rows[0]["Rebate_Discount_Appl"].ToString();

                    rblAddiRebateDiscountApp.SelectedValue = dtProposal.Rows[0]["Addi_Rebate_Discount_Appl"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Rebate_Discount_Perc"].ToString()))
                        txtRebateDiscountPerc.Text = dtProposal.Rows[0]["Rebate_Discount_Perc"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["No_of_Installments_Rebate"].ToString()))
                        txtRebateNoofInstall.Text = dtProposal.Rows[0]["No_of_Installments_Rebate"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Rebate_Struc_Allocation"].ToString()))
                        ddlRebateStrucAlloc.SelectedValue = dtProposal.Rows[0]["Rebate_Struc_Allocation"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Rebate_Allowed_Method"].ToString()))
                        RBLRebateStructuredEI.SelectedValue = dtProposal.Rows[0]["Rebate_Allowed_Method"].ToString();

                    //opc058 end


                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Addi_Rebate_Discount_Perc"].ToString()))
                        txtAddiRebateDiscountPerc.Text = dtProposal.Rows[0]["Addi_Rebate_Discount_Perc"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Addi_No_of_Installments_Rebate"].ToString()))
                        txtAddiRebateNoofInstall.Text = dtProposal.Rows[0]["Addi_No_of_Installments_Rebate"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Addi_Rebate_Allowed_Method"].ToString()))
                        RBLAddiRebateStructuredEI.SelectedValue = dtProposal.Rows[0]["Addi_Rebate_Allowed_Method"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Guaranteed_EOT"].ToString()))
                        txtGuaranteedEOT.Text = dtProposal.Rows[0]["Guaranteed_EOT"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Guaranteed_EOT_App"].ToString()))
                        ddlGuaranteedEOTApp.SelectedValue = dtProposal.Rows[0]["Guaranteed_EOT_App"].ToString();

                    ddlSecDepApp.SelectedValue = dtProposal.Rows[0]["Sec_Dep_App"].ToString();
                    ddlRentalBasedOn.SelectedValue = dtProposal.Rows[0]["Rental_Based_On"].ToString();
                    //ddlSecondaryRentalBasedOn.SelectedValue = dtProposal.Rows[0]["Secondary_Rental_Based_On"].ToString();

                    ViewState["Status_ID"] = dtProposal.Rows[0]["Status_ID"].ToString();
                    ViewState["Download_Path"] = dtProposal.Rows[0]["Upload_Path"].ToString();
                    ViewState["Document_Path"] = dtProposal.Rows[0]["Upload_Path"].ToString();
                    lblCurrentPath.Text = Path.GetFileName(ViewState["Download_Path"].ToString());
                    if (lblCurrentPath.Text != "")
                    {
                        ReqIdhyplnkView.Enabled = true;
                    }
                    FunPriProposalType_SelectedIndexChanged();
                    FunPriSecuritydeposit_SelectedIndexChanged();
                    FunPriRBLSecondaryTerm_SelectedIndexChanged();
                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Secu_Rent_Amount"].ToString()))
                        txtSecDepAdvRent.Text = dtProposal.Rows[0]["Secu_Rent_Amount"].ToString();

                    if (!string.IsNullOrEmpty(dtProposal.Rows[0]["Lead_ID"].ToString()))
                    {
                        if (Convert.ToInt32(dtProposal.Rows[0]["Lead_ID"].ToString()) > 0)
                            FunPriLoadLeadDtls(Convert.ToInt32(dtProposal.Rows[0]["Lead_ID"].ToString()));
                    }

                    //ddlStampDuty.SelectedValue = dtProposal.Rows[0]["stamp_duty_app"].ToString();
                    //ddlInterimDetails.SelectedValue = dtProposal.Rows[0]["interim_applicable"].ToString();

                }
                #endregion

                if (ds_PricingDetails.Tables[1].Rows.Count > 0)
                {
                    ViewState["dtPrimaryGrid"] = dtPrimaryGrid = ds_PricingDetails.Tables[1].Copy();
                    Session["dtPrimaryGrid"] = (DataTable)ViewState["dtPrimaryGrid"];
                    FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                    FunPriSetFooterCtrlPG();
                }

                if (ds_PricingDetails.Tables[2].Rows.Count > 0)
                {
                    ViewState["dtSecondaryGrid"] = dtSecondaryGrid = ds_PricingDetails.Tables[2].Copy();
                    FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                    FunPriSetFooterCtrlSG();
                }

                if (ds_PricingDetails.Tables[3].Rows.Count > 0)
                {
                    ViewState["dtEUCDetails"] = dtEUCDetails = ds_PricingDetails.Tables[3].Copy();
                    FunFillgrid(grvEUC, dtEUCDetails);
                }
                FunPriCalcFacilityAmount();

                //opc058

                if (ds_PricingDetails.Tables[4].Rows.Count > 0)
                {
                    ViewState["dtRebateDetGrid"] = dtRebateDetGrid = ds_PricingDetails.Tables[4].Copy();
                    Session["dtRebateDetGrid"] = (DataTable)ViewState["dtRebateDetGrid"];
                    FunFillgrid(grvRebateDetGrid, dtRebateDetGrid);
                    //FunPriSetFooterCtrlPG();
                }

                if (rblIs_Guaranteed_EOT_Appli.SelectedValue == "0")
                {
                    ddlGuaranteedEOTApp.Enabled = false;
                    txtGuaranteedEOT.Enabled = false;
                }
                if (RBLRebateStructuredEI.SelectedValue == "1" && Convert.ToInt32(rblRebateDiscountApp.SelectedValue) == 1)
                {
                    ddlRebateStrucAlloc.Enabled = false;
                }

                if (RBLRebateStructuredEI.SelectedValue == "2" && Convert.ToInt32(rblRebateDiscountApp.SelectedValue) == 1)
                {
                    txtRebateNoofInstall.Enabled = false;
                }

                if (rblRebateDiscountApp.SelectedValue == "1")
                {
                    if (RBLRebateStructuredEI.SelectedValue == "1")
                    {
                        grvRebateDetGrid.Enabled = false;
                    }
                    TextBox txtRebatePerc = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtRebatePerc");
                    TextBox txtFixedRebate = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtFixedRebate");
                    if (ddlRebateStrucAlloc.SelectedValue == "1")
                    {
                        txtRebatePerc.Enabled = false;
                    }
                    if (ddlRebateStrucAlloc.SelectedValue == "2")
                    {
                        txtFixedRebate.Enabled = false;
                    }
                }

                FunPriRebateDiscountApp_SelectedIndexChanged();
                FunPriAddiRebateDiscountApp_SelectedIndexChanged();
                //end
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }




    //To Load LOB
    /// <summary>
    /// This method is used to Load the Line of Business to a dropdown from stored Procedure using BindDatatable option.
    /// </summary>
    private void FunLoadLOBMaster()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_Id", intUserId.ToString());
            Procparam.Add("@Company_ID", intCompany_Id.ToString());
            if (ViewState["ConsitutionId"] != null)
            {
                Procparam.Add("@Consitution_Id", ViewState["ConsitutionId"].ToString());
            }
            Procparam.Add("@Program_Id", "42");
            ddlLob.BindDataTable(SPNames.LOBMaster, Procparam, false, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //To Load Proposal Type,Return Pattern and Security Deposits
    /// <summary>
    /// This method is used to Load the Proposal Type,Return Pattern and Security Deposits to a dropdown from stored Procedure using BindDatatable option.
    /// </summary>
    private void FunLoadDDLControls()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompany_Id.ToString());
            Procparam.Add("@Option", "1");
            DataSet ds = Utility.GetDataset("S3G_ORG_ProposalLukup", Procparam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)//Proposal Type
                {
                    ddlProposalType.FillDataTable(ds.Tables[0], "Value", "Name");
                }
                if (ds.Tables[1].Rows.Count > 0)//SECURITY_DEPOSIT
                {
                    ddlSecuritydeposit.FillDataTable(ds.Tables[1], "Value", "Name");
                }
                if (ds.Tables[2].Rows.Count > 0)//RETURN_PATTERN
                {
                    ddlReturnPattern.FillDataTable(ds.Tables[2], "Value", "Name", false);
                    ddlReturnPattern.SelectedValue = "3";//default PTF
                }
                if (ds.Tables[3].Rows.Count > 0)//TIME_VALUE
                {
                    ViewState["TIME_VALUE"] = ds.Tables[3].Copy();
                }
                if (ds.Tables[4].Rows.Count > 0)//FREQUENCY
                {
                    ViewState["FREQUENCY"] = ds.Tables[4].Copy();
                }

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompany_Id.ToString());
                Procparam.Add("@Option", "6");
                DataSet dsDocPath = Utility.GetDataset("S3G_ORG_ProposalLukup", Procparam);
                if (dsDocPath != null && dsDocPath.Tables[0].Rows.Count > 0)
                {
                    lblActualPath.Text = dsDocPath.Tables[0].Rows[0]["DOCUMENT_PATH"].ToString();
                }
                else
                {
                    lblActualPath.Text = "";
                }


            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to set max length for the amount field
    /// </summary>
    private void FunPriSetMaxLength()
    {
        try
        {
            txtTotalFacilityAmount.CheckGPSLength(true, "Total Facility Amount");
            txtOneTimeFee.CheckGPSLength(true, "One Time Fee");
            txtProcessingFee.CheckGPSLength(true, "Processing Fee");
            //txtSecDepAdvRent.SetDecimalPrefixSuffix(10, 2, true, "Sec Dep/Adv Rent %");
            txtSecDepAdvRent.SetPercentagePrefixSuffix(3, 5, true, false, "Sec Dep/Adv Rent %");

            txtOnetimeRate.SetPercentagePrefixSuffix(3, 8, true, false, "Onetime Rate %");
            txtProcessingRate.SetPercentagePrefixSuffix(3, 8, true, false, "Processing Rate %");
            txtOneTimeAmount.SetPercentagePrefixSuffix(10, 6, true, false, "Onetime Amount");
            //txtOneTimeAmount.Attributes.Add("onblur", "funChkDecimial(this,'10','6','Amount',true);");

            txtProcessingAmount.SetPercentagePrefixSuffix(10, 6, true, false, "Processing Amount");
            //opc058 start
            txtRebateDiscountPerc.SetPercentagePrefixSuffix(3, 2, true, false, "Rebate Discount %");
            txtGuaranteedEOT.SetPercentagePrefixSuffix(3, 3, true, false, "Guaranteed EOT Sale %");
            //opc058 end

            txtAddiRebateDiscountPerc.SetPercentagePrefixSuffix(8, 5, true, false, "Additional Rebate Discount %");

            FunPriSetMaxLength_PG();
            FunPriSetMaxLength_SG();
            FunPriSetMaxLength_Rebate();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined method is used to set Asset category defined in primary grid should be 
    /// loaded in secondary grid and End use customer details.
    /// </summary>
    private void FunAddremoveAssetCategoryDDL()
    {
        try
        {
            string strTextItem = "NAME"; string strValueItem = "ID";
            dtAssetCategorytbl = new DataTable();
            if (ViewState["dtPrimaryGrid"] != null)
                dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];

            if (!dtAssetCategorytbl.Columns.Contains("AssetCategory"))
                dtAssetCategorytbl.Columns.Add("NAME");
            if (!dtAssetCategorytbl.Columns.Contains("AssetCategory_ID"))
                dtAssetCategorytbl.Columns.Add("ID");
            DataTable dtPriSecondary = new DataTable();
            if (ddlProposalType.SelectedValue == "3")//Extension
            {
                dtPriSecondary = dtSecondaryGrid.Copy();
            }
            else
            {
                dtPriSecondary = dtPrimaryGrid.Copy();
            }

            if (dtPriSecondary.Rows.Count > 0)
            {
                DataTable dtUniqRecords = dtPriSecondary.DefaultView.ToTable(true, new string[] { "AssetCategory", "AssetCategory_ID" });

                foreach (DataRow dr in dtUniqRecords.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["AssetCategory_ID"].ToString()))
                    {
                        DataRow drAssetCategory;
                        drAssetCategory = dtAssetCategorytbl.NewRow();
                        if (!string.IsNullOrEmpty(dr["AssetCategory_ID"].ToString()))
                            drAssetCategory[strValueItem] = dr["AssetCategory_ID"];
                        if (!string.IsNullOrEmpty(dr["AssetCategory"].ToString()))
                            drAssetCategory[strTextItem] = dr["AssetCategory"];
                        dtAssetCategorytbl.Rows.Add(drAssetCategory);
                    }
                }
            }

            if (dtAssetCategorytbl.Rows.Count > 0)
            {
                ViewState["dtAssetCategorytbl"] = dtAssetCategorytbl;

                //Fill dropdown End Use Customer Details
                ddlAssetCategoryEUC.DataTextField = strTextItem;
                ddlAssetCategoryEUC.DataValueField = strValueItem;
                ddlAssetCategoryEUC.DataSource = dtAssetCategorytbl;
                ddlAssetCategoryEUC.DataBind();
                System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlAssetCategoryEUC.Items.Insert(0, liSelect1);
                ddlAssetCategoryEUC.AddItemToolTipValue();


                //Secondary Details
                //if (grvSecondaryGrid != null)
                //{
                //    if (grvSecondaryGrid.FooterRow != null)
                //    {
                //        DropDownList ddlAssetCategorySG = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlAssetCategorySG");
                //        if (ddlAssetCategorySG != null)
                //        {
                //            ddlAssetCategorySG.DataTextField = strTextItem;
                //            ddlAssetCategorySG.DataValueField = strValueItem;
                //            ddlAssetCategorySG.DataSource = dtAssetCategorytbl;
                //            ddlAssetCategorySG.DataBind();
                //            ddlAssetCategorySG.Items.Insert(0, liSelect1);
                //            ddlAssetCategorySG.AddItemToolTipValue();
                //        }
                //    }
                //}
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    /// This method is used to load the customer address based on the customer control customer id
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPriGetCustomerAddress(int CustomerID)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@CustomerID", CustomerID.ToString());
            Procparam.Add("@CompanyID", intCompany_Id.ToString());

            DataSet dsCustomer = Utility.GetDataset("S3G_ORG_Get_Exist_Customer_Details", Procparam);
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Text = txtCustomerCode.Text = dsCustomer.Tables[0].Rows[0]["Customer_Code"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(dsCustomer.Tables[0].Rows[0]["Customer_Code"].ToString(),
                    dsCustomer.Tables[0].Rows[0]["comm_Address1"].ToString() + "\n" +
             dsCustomer.Tables[0].Rows[0]["comm_Address2"].ToString() + "\n" +
            dsCustomer.Tables[0].Rows[0]["comm_city"].ToString() + "\n" +
            dsCustomer.Tables[0].Rows[0]["comm_state"].ToString() + "\n" +
            dsCustomer.Tables[0].Rows[0]["comm_country"].ToString() + "\n" +
            dsCustomer.Tables[0].Rows[0]["comm_pincode"].ToString(), dsCustomer.Tables[0].Rows[0]["Customer_Name"].ToString(), dsCustomer.Tables[0].Rows[0]["Comm_Telephone"].ToString(),
            dsCustomer.Tables[0].Rows[0]["Comm_mobile"].ToString(),
            dsCustomer.Tables[0].Rows[0]["comm_email"].ToString(), dsCustomer.Tables[0].Rows[0]["comm_website"].ToString());

            if (dsCustomer.Tables[1].Rows.Count > 0)
            {
                pnlMRADetails.Visible = true;
                ddlMRANumber.FillDataTable(dsCustomer.Tables[1], "MRA_ID", "MRA_Number");//8968
                if (dsCustomer.Tables[1].Rows.Count == 1)
                {
                    ddlMRANumber.SelectedIndex = 1;
                }
            }
            else
            {
                pnlMRADetails.Visible = false;

            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    /// <summary>
    /// This method is used to do action related to tenure change events
    /// </summary>
    private void FunPriTenurePG_OnTextChanged()
    {
        try
        {
            funPriSetTenure_Freq_onchange("PG");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to do action related to tenure change events
    /// </summary>
    private void funPriSetTenure_Freq_onchange(string strType)
    {
        try
        {
            TextBox txtTenure = null;
            DropDownList ddlRentFrequency = null;

            DropDownList ddlAdvanceArrear = null;
            TextBox txtFromInstallNo = null;
            TextBox txtToInstallNo = null;
            TextBox txtFacilityAmount = null;
            //TextBox txtResidualPer = null;
            switch (strType)
            {
                case "PG":
                    ddlRentFrequency = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlRentFrequencyPG");
                    txtTenure = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtTenurePG");
                    txtFromInstallNo = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFromInstallNoPG");
                    txtToInstallNo = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtToInstallNoPG");
                    break;
                case "SG":
                    ddlRentFrequency = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlRentFrequencySG");
                    txtTenure = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtTenureSG");
                    txtFromInstallNo = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFromInstallNoSG");
                    txtToInstallNo = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtToInstallNoSG");
                    break;
            }

            int inttenure = 0;
            if (!string.IsNullOrEmpty(txtTenure.Text))
            {
                inttenure = Convert.ToInt16(txtTenure.Text);
                switch (ddlRentFrequency.SelectedValue)
                {
                    case "5"://Bi Monthly
                        inttenure = inttenure / 2;
                        break;
                    case "6"://quarterly
                        inttenure = inttenure / 3;
                        break;
                    case "7"://half yearly 
                        inttenure = inttenure / 6;
                        break;
                    case "8"://annually 
                        inttenure = inttenure / 12;
                        break;
                }
            }
            if (RBLStructuredEI.SelectedValue == "1")//Equated
            {
                if (!string.IsNullOrEmpty(txtTenure.Text))
                {
                    txtFromInstallNo.Text = "1";
                    // txtToInstallNo.Text = inttenure.ToString();
                }
            }
            FunPriSetGridValidationPGSG(strType);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to do action related to tenure change events
    /// </summary>
    private void FunPriTenureSG_OnTextChanged()
    {
        try
        {
            DropDownList ddlRentFrequencySG = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlRentFrequencySG");
            TextBox txtTenureSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtTenureSG");
            TextBox txtFromInstallNoSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFromInstallNoSG");
            TextBox txtToInstallNoSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtToInstallNoSG");
            //By Siva.k on 01JUL2015 Remove the Residual Per
            //TextBox txtResidualPerSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtToInstallNoSG");
            int inttenure = 0;
            if (!string.IsNullOrEmpty(txtTenureSG.Text))
            {
                inttenure = Convert.ToInt16(txtTenureSG.Text);
                switch (ddlRentFrequencySG.SelectedValue)
                {
                    //case "1"://daily
                    //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                    //    break;
                    //case "2"://Weekly
                    //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                    //    break;
                    //case "4"://Monthly
                    //inttenure = inttenure;
                    //break;
                    case "5"://bi monthly 
                        inttenure = inttenure / 2;
                        break;
                    case "6"://quarterly
                        inttenure = inttenure / 3;
                        break;
                    case "7"://half yearly 
                        inttenure = inttenure / 6;
                        break;
                    case "8"://annually 
                        inttenure = inttenure / 12;
                        break;

                }
            }
            if (RBLStructuredEI.SelectedValue == "1")//Equated
            {
                if (!string.IsNullOrEmpty(txtTenureSG.Text))
                {
                    txtFromInstallNoSG.Text = "1";
                    txtToInstallNoSG.Text = inttenure.ToString();
                }
            }

            FunPriSetGridValidationPGSG("SG");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    /// <summary>
    /// This method is used to do action related to Asset category change events
    /// </summary>
    private void FunPriAssetCategorySG_SelectedIndexChanged()
    {
        try
        {
            FunPriAssetCategoryChange("SG");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to do action related to Asset category change events
    /// </summary>
    private void FunPriAssetCategoryPG_SelectedIndexChanged()
    {
        try
        {
            FunPriAssetCategoryChange("PG");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to do action related to radiobutton change events
    /// </summary>
    private void FunPriRBLSecondaryTerm_SelectedIndexChanged()
    {
        try
        {
            if (strPricingId == "")
            {
                FunPriEmptySecondaryGrid();
            }
            else
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Pricing_ID", strPricingId);
                Procparam.Add("@COMPANY_ID", intCompany_Id.ToString());
                DataSet ds_PricingDetails = Utility.GetDataset("S3G_ORG_Get_Proposal_Dtls", Procparam);

                if (ds_PricingDetails.Tables[2].Rows.Count > 0)
                {
                    ViewState["dtSecondaryGrid"] = dtSecondaryGrid = ds_PricingDetails.Tables[2].Copy();
                    FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                    FunPriSetFooterCtrlSG();
                }
                FunPriCalcFacilityAmount();
            }


            if (RBLSecondaryTerm.SelectedValue == "1")//YES
            {
                pnlSecondaryGrid.Enabled = true;
            }
            else//NO
            {
                /* Added by Thalai - Secondary grid to be clear on 22-Sep-2015 - Start */
                FunPriEmptySecondaryGrid();
                /* Added by Thalai - Secondary grid to be clear on 22-Sep-2015 - End */
                pnlSecondaryGrid.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    /// <summary>
    /// This method is used to do action related to radiobutton change events
    /// </summary>
    private void FunPriRBLStructuredEI_SelectedIndexChanged()
    {
        try
        {
            FunPriEmptyPrimaryGrid();
            FunPriEmptySecondaryGrid();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to do action related to radiobutton change events
    /// </summary>
    private void FunPriRBLAdvanceRent_SelectedIndexChanged()
    {
        try
        {

            FunPriSetSecurityDeposit();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to do action related to radiobutton change events
    /// </summary>
    private void FunPriSetSecurityDeposit()
    {
        try
        {
            txtSecDepAdvRent.ReadOnly = true;
            txtSecDepAdvRent.Text = string.Empty;
            if (ddlSecuritydeposit.SelectedValue != "1" || RBLAdvanceRent.SelectedValue == "1")
                txtSecDepAdvRent.ReadOnly = false;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to do action related to radiobutton change events
    /// </summary>
    private void FunPriSecuritydeposit_SelectedIndexChanged()
    {
        try
        {
            if (ddlSecuritydeposit.SelectedValue == "1")//1	Not applicable
            {
                RBLAdvanceRent.Enabled = true;
                lblSecDepAdvRent.CssClass = "styleDisplayLabel";
                rfvSecDepAdvRent.Enabled = false;
                ddlSecDepApp.SelectedIndex = 0;
                ddlSecDepApp.Enabled = false;
            }
            else// 2	Adjustable ,3	Refundable
            {
                lblSecDepAdvRent.CssClass = "styleReqFieldLabel";
                rfvSecDepAdvRent.Enabled = true;
                RBLAdvanceRent.Enabled = false;
                RBLAdvanceRent.SelectedValue = "0";
                ddlSecDepApp.Enabled = true;
            }
            FunPriSetSecurityDeposit();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    /// <summary>
    /// This method is used to do action related to porposal type dropdowndownlist change events
    /// </summary>
    private void FunPriProposalType_SelectedIndexChanged()
    {
        try
        {
            if (ddlProposalType.SelectedValue == "3" || ddlProposalType.SelectedValue == "4")//Extension
            {
                rfvTermSheetDate.Enabled = false;
            }
            else
            {
                rfvTermSheetDate.Enabled = true;
            }

            FunPriEmptyPrimaryGrid();
            if (ddlProposalType.SelectedValue == "3")//Extension
            {
                pnlPrimaryGrid.Enabled = false;
                RBLSecondaryTerm.SelectedValue = "1";
                RBLSecondaryTerm.Enabled = false;
                FunPriRBLSecondaryTerm_SelectedIndexChanged();
            }
            else//NO
            {
                pnlPrimaryGrid.Enabled = true;
                if (strPricingId == "")
                {
                    RBLSecondaryTerm.SelectedValue = "0";
                }
                else if (Convert.ToString(ViewState["Seco_Term_Applicability"]) != "")
                {
                    RBLSecondaryTerm.SelectedValue = Convert.ToString(ViewState["Seco_Term_Applicability"]);
                }

                RBLSecondaryTerm.Enabled = true;
                FunPriRBLSecondaryTerm_SelectedIndexChanged();
            }
            txtTotalFacilityAmount.Text = "";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to do action related to Asset category change events based on the grids type primary/Secondary
    /// </summary>
    /// <param name="strType"></param>
    private void FunPriAssetCategoryChange(string strType)
    {
        try
        {
            DataTable dtGrids = new DataTable();
            UserControls_S3GAutoSuggest ddlAssetCategory = null;
            UserControls_S3GAutoSuggest ddlAssetType = null;
            //DropDownList ddlAssetCategoryD = null;
            TextBox txtTenure = null;
            DropDownList ddlRentFrequency = null;

            DropDownList ddlAdvanceArrear = null;
            TextBox txtFromInstallNo = null;
            TextBox txtToInstallNo = null;
            TextBox txtFacilityAmount = null;
            //TextBox txtResidualPer = null;

            switch (strType)
            {
                case "PG":
                    ddlAssetCategory = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetCategoryPG");
                    ddlAssetType = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetTypePG");
                    txtTenure = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtTenurePG");
                    ddlRentFrequency = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlRentFrequencyPG");
                    ddlAdvanceArrear = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlAdvanceArrearPG");
                    txtFromInstallNo = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFromInstallNoPG");
                    txtToInstallNo = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtToInstallNoPG");
                    txtFacilityAmount = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFacilityAmountPG");
                    //By Siva.k on 01JUL2015 Remove the Residual Per
                    //txtResidualPer = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtResidualPerPG");
                    hdnAssetCatPG.Value = ddlAssetCategory.SelectedValue;
                    hdnAssetTypePG.Value = ddlAssetType.SelectedValue;
                    if (ViewState["dtPrimaryGrid"] != null)
                        dtGrids = (DataTable)ViewState["dtPrimaryGrid"];

                    break;
                case "SG":
                    ddlAssetCategory = (UserControls_S3GAutoSuggest)grvSecondaryGrid.FooterRow.FindControl("ddlAssetCategorySG");
                    ddlAssetType = (UserControls_S3GAutoSuggest)grvSecondaryGrid.FooterRow.FindControl("ddlAssetTypeSG");
                    txtTenure = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtTenureSG");
                    ddlRentFrequency = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlRentFrequencySG");
                    ddlAdvanceArrear = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlAdvanceArrearSG");
                    txtFromInstallNo = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFromInstallNoSG");
                    txtToInstallNo = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtToInstallNoSG");
                    txtFacilityAmount = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFacilityAmountSG");
                    //By Siva.k on 01JUL2015 Remove the Residual Per
                    // txtResidualPer = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtResidualPerSG");
                    hdnAssetCatPG.Value = ddlAssetCategory.SelectedValue;
                    hdnAssetTypePG.Value = ddlAssetType.SelectedValue;
                    if (ViewState["dtSecondaryGrid"] != null)
                        dtGrids = (DataTable)ViewState["dtSecondaryGrid"];
                    break;
            }


            //txtFacilityAmount.ReadOnly =
            //txtTenure.ReadOnly = false;
            //ddlRentFrequency.Enabled =
            //ddlAdvanceArrear.Enabled = true;
            //txtTenure.Text = txtFacilityAmount.Text = txtResidualPer.Text = txtFromInstallNo.Text = txtToInstallNo.Text = string.Empty;
            //if (ddlRentFrequency.Items.Count > 0)
            //    ddlRentFrequency.SelectedIndex = -1;
            //if (ddlAdvanceArrear.Items.Count > 0)
            //    ddlAdvanceArrear.SelectedIndex = -1;

            //if (RBLStructuredEI.SelectedValue == "2")//Structured
            //{
            //    if (dtGrids.Rows.Count > 0)
            //    {
            //        DataRow[] drGrid1 = null;

            //        drGrid1 = dtGrids.Select(" AssetCategory_ID = " + ddlAssetCategory.SelectedValue + "");

            //        if (drGrid1.Count() > 0)
            //        {
            //            int intcount = drGrid1.Count();
            //            txtFacilityAmount.ReadOnly =
            //            txtTenure.ReadOnly = true;
            //            ddlRentFrequency.Enabled =
            //            ddlAdvanceArrear.Enabled = false;
            //            txtTenure.Text = drGrid1[intcount - 1]["Tenure"].ToString();
            //            txtFacilityAmount.Text = drGrid1[intcount - 1]["FacilityAmount"].ToString();
            //            ddlRentFrequency.SelectedValue = drGrid1[intcount - 1]["RentFrequencyID"].ToString();
            //            ddlAdvanceArrear.SelectedValue = drGrid1[intcount - 1]["AdvanceArrearID"].ToString();
            //            txtResidualPer.Text = drGrid1[intcount - 1]["ResidualPer"].ToString();
            //            if (!string.IsNullOrEmpty(drGrid1[intcount - 1]["ToInstallNo"].ToString()))
            //                txtFromInstallNo.Text = (Convert.ToInt32(drGrid1[intcount - 1]["ToInstallNo"]) + 1).ToString();
            //        }
            //    }
            //}
            FunPriSetGridValidationPGSG(strType);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }



    /// <summary>
    /// This method is used to do action related to Asset category change events
    /// </summary>
    private void FunPriSetGridValidationPGSG(string strType)
    {
        try
        {
            TextBox txtFromInstallNo = null;
            TextBox txtToInstallNo = null;
            //TextBox txtResidualPer = null;
            TextBox txtTenure = null;
            DropDownList ddlRentFrequency = null;
            TextBox txtFacilityAmount = null;
            Button btnAddRepayment = null;
            UserControls_S3GAutoSuggest ddlAssetCategory = null;
            UserControls_S3GAutoSuggest ddlAssetType = null;
            UserControls_S3GAutoSuggest ddlAssetSubType = null;
            //DropDownList ddlAssetCategoryD = null;
            switch (strType)
            {
                case "PG":
                    txtFromInstallNo = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFromInstallNoPG");
                    txtToInstallNo = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtToInstallNoPG");
                    ddlRentFrequency = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlRentFrequencyPG");
                    txtFacilityAmount = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFacilityAmountPG");
                    txtTenure = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtTenurePG");
                    //By Siva.k on 01JUL2015 Remove the Residual Per
                    // txtResidualPer = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtResidualPerPG");
                    ddlAssetCategory = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetCategoryPG");
                    ddlAssetType = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetTypePG");
                    ddlAssetSubType = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetSubTypePG");
                    break;
                case "SG":
                    txtFromInstallNo = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFromInstallNoSG");
                    txtToInstallNo = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtToInstallNoSG");
                    ddlRentFrequency = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlRentFrequencySG");
                    txtTenure = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtTenureSG");
                    txtFacilityAmount = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFacilityAmountSG");
                    //By Siva.k on 01JUL2015 Remove the Residual Per
                    //txtResidualPer = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtResidualPerSG");
                    ddlAssetCategory = (UserControls_S3GAutoSuggest)grvSecondaryGrid.FooterRow.FindControl("ddlAssetCategorySG");
                    btnAddRepayment = (Button)grvSecondaryGrid.FooterRow.FindControl("btnAddRepayment");
                    break;
            }
            int ToInstallment;
            int FromInstallment;
            int inttenure;
            int Tenure;





            if (RBLStructuredEI.SelectedValue == "2")//structured
            {
                //txtFromInstallNo.ReadOnly = false; //By Siva.K on 24MAR2015
                if (strType == "PG")
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)ViewState["dtPrimaryGrid"];
                    dtPrimaryGrid = dt;

                    DataRow[] dr = dtPrimaryGrid.Select("AssetCategory_ID = " + ddlAssetCategory.SelectedValue + " AND TENURE = " + txtTenure.Text.Trim() + " AND RENTFREQUENCYID = " + Convert.ToString(ddlRentFrequency.SelectedValue + ""));
                    if (dr.Length > 0)
                    {
                        if (dr[0]["TOinstallno"].ToString() == "")
                        {
                            ToInstallment = 0;
                        }
                        else
                        {
                            ToInstallment = Convert.ToInt32(dr[dr.Length - 1]["TOinstallno"].ToString());
                        }
                        ToInstallment = ToInstallment + 1;
                        Tenure = Convert.ToInt32(dr[0]["Tenure"].ToString());
                        inttenure = Convert.ToInt16(txtTenure.Text);
                        txtFromInstallNo.Text = Convert.ToString(ToInstallment);
                        txtFacilityAmount.Text = Convert.ToString(dr[0]["FacilityAmount"].ToString());
                        txtFacilityAmount.ReadOnly = true;  //By Siva.K on 24MAR2015

                    }
                    else
                    {
                        txtFromInstallNo.Text = "1";
                        txtFacilityAmount.Text = "";
                        txtFacilityAmount.ReadOnly = false;  //By Siva.K on 24MAR2015
                    }
                }
                if (strType == "SG")
                {
                    if (ddlProposalType.SelectedValue != "3")
                    {
                        txtFromInstallNo.ReadOnly = true;
                        DataTable dt = new DataTable();
                        dt = (DataTable)ViewState["dtSecondaryGrid"];
                        dtSecondaryGrid = dt;
                        DataRow[] dr = dtSecondaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategory.SelectedValue + " AND TENURE = " + txtTenure.Text.Trim() + " AND RENTFREQUENCYID = " + Convert.ToString(ddlRentFrequency.SelectedValue));
                        if (dr.Length > 0)
                        {
                            if (dr[dr.Length - 1]["TOinstallno"].ToString() == "")
                            {
                                ToInstallment = 0;
                            }
                            else
                            {
                                ToInstallment = Convert.ToInt32(dr[dr.Length - 1]["TOinstallno"].ToString());
                            }
                            ToInstallment = ToInstallment + 1;
                            Tenure = Convert.ToInt32(dr[0]["Tenure"].ToString());
                            inttenure = Convert.ToInt16(txtTenure.Text);
                            txtFromInstallNo.Text = Convert.ToString(ToInstallment);
                            txtFacilityAmount.Text = Convert.ToString(dr[0]["FacilityAmount"].ToString());
                            txtFacilityAmount.ReadOnly = true;  //By Siva.K on 24MAR2015
                        }
                        else
                        {
                            dt = (DataTable)ViewState["dtPrimaryGrid"];
                            dtSecondaryGrid = dt;
                            DataRow[] dr1 = dtSecondaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategory.SelectedValue + " AND TENURE = " + txtTenure.Text.Trim() + " AND RENTFREQUENCYID = " + Convert.ToString(ddlRentFrequency.SelectedValue));
                            if (dr1.Length > 0)
                            {
                                if (dr1[0]["TOinstallno"].ToString() == "")
                                {
                                    ToInstallment = 0;
                                }
                                else
                                {
                                    ToInstallment = Convert.ToInt32(dr1[dr1.Length - 1]["TOinstallno"].ToString());
                                }
                                ToInstallment = ToInstallment + 1;
                                Tenure = Convert.ToInt32(dr1[0]["Tenure"].ToString());
                                inttenure = Convert.ToInt16(txtTenure.Text);
                                txtFromInstallNo.Text = Convert.ToString(ToInstallment);
                                txtFacilityAmount.Text = Convert.ToString(dr1[0]["FacilityAmount"].ToString());
                                txtFacilityAmount.ReadOnly = true;  //By Siva.K on 24MAR2015
                            }
                        }
                    }
                    else
                    {
                        txtFromInstallNo.ReadOnly = true;
                        DataTable dt = new DataTable();
                        dt = (DataTable)ViewState["dtSecondaryGrid"];
                        dtSecondaryGrid = dt;
                        DataRow[] dr = dtSecondaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategory.SelectedValue + " AND TENURE = " + txtTenure.Text.Trim() + " AND RENTFREQUENCYID = " + Convert.ToString(ddlRentFrequency.SelectedValue));
                        if (dr.Length > 0)
                        {
                            //btnAddRepayment.Enabled = true;
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Entered Combination already exists');", true);
                            //return;
                            if (dr[0]["TOinstallno"].ToString() == "")
                            {
                                ToInstallment = 0;
                            }
                            else
                            {
                                ToInstallment = Convert.ToInt32(dr[0]["TOinstallno"].ToString());
                            }
                            ToInstallment = ToInstallment + 1;
                            txtFromInstallNo.Text = Convert.ToString(ToInstallment);
                            txtFacilityAmount.Text = dr[0]["FacilityAmount"].ToString();
                            txtFacilityAmount.ReadOnly = true;  //By Siva.K on 24MAR2015

                        }
                        else
                        {
                            btnAddRepayment.Enabled = true;
                            txtFromInstallNo.Text = "1";
                            txtToInstallNo.Text = "";
                            txtFacilityAmount.Text = dr[0]["FacilityAmount"].ToString();
                            txtFacilityAmount.ReadOnly = true;  //By Siva.K on 24MAR2015
                            //inttenure = Convert.ToInt32(txtTenure.Text);
                            //switch (ddlRentFrequency.SelectedValue)
                            //{
                            //    case "6"://quarterly
                            //        inttenure = inttenure / 3;
                            //        break;
                            //    case "7"://half yearly 
                            //        inttenure = inttenure / 6;
                            //        break;
                            //    case "8"://annually 
                            //        inttenure = inttenure / 12;
                            //        break;
                            //}
                            //txtToInstallNo.Text = Convert.ToString(inttenure);
                        }
                    }
                }
            }
            else//Equated
            {
                if (strType == "SG")
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)ViewState["dtPrimaryGrid"];
                    dtPrimaryGrid = dt;

                    //dtPrimaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategoryPG.SelectedValue + 
                    //    " AND TENURE=" + txtTenurePG.Text.Trim() + 
                    //    " AND RENTFREQUENCYID=" + Convert.ToString(ddlRentFrequencyPG.SelectedValue));
                    if (RBLStructuredEI.SelectedValue == "1")
                    {
                        DataRow[] dr = dtPrimaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategory.SelectedValue + " AND TENURE = " + txtTenure.Text.Trim() + " AND RENTFREQUENCYID = " + Convert.ToString(ddlRentFrequency.SelectedValue));
                        if (dr.Length > 0)
                        {
                            if (dr[0]["TOinstallno"].ToString() == "")
                            {
                                ToInstallment = 0;
                            }
                            else
                            {
                                ToInstallment = Convert.ToInt32(dr[0]["TOinstallno"].ToString());
                            }

                            Tenure = Convert.ToInt32(dr[0]["Tenure"].ToString());
                            inttenure = Convert.ToInt16(txtTenure.Text);
                            switch (ddlRentFrequency.SelectedValue)
                            {
                                //case "1"://daily
                                //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                                //    break;
                                //case "2"://Weekly
                                //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                                //    break;
                                //case "4"://Monthly
                                //inttenure = inttenure;
                                //break;
                                case "5"://bi monthly 
                                    inttenure = inttenure / 2;
                                    break;
                                case "6"://quarterly
                                    inttenure = inttenure / 3;
                                    break;
                                case "7"://half yearly 
                                    inttenure = inttenure / 6;
                                    break;
                                case "8"://annually 
                                    inttenure = inttenure / 12;
                                    break;
                            }

                            //if (ToInstallment == Tenure)
                            //{
                            //    //btnAddRepayment.Enabled = false;
                            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Tenure already completed');", true);
                            //    //return;
                            //}
                            //else
                            //{
                            ToInstallment = ToInstallment + 1;
                            txtFromInstallNo.Text = Convert.ToString(ToInstallment);
                            txtToInstallNo.Text = Convert.ToString(inttenure);
                            txtFacilityAmount.Text = dr[0]["FacilityAmount"].ToString();
                            btnAddRepayment.Enabled = true;
                            txtFacilityAmount.ReadOnly = true;  //By Siva.K on 24MAR2015
                            //}
                        }
                        else
                        {
                            if (txtTenure.Text != "" && ddlRentFrequency.SelectedValue != "0")
                            {
                                inttenure = Convert.ToInt16(txtTenure.Text);
                                switch (ddlRentFrequency.SelectedValue)
                                {
                                    //case "1"://daily
                                    //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                                    //    break;
                                    //case "2"://Weekly
                                    //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                                    //    break;
                                    //case "4"://Monthly
                                    //inttenure = inttenure;
                                    //break;
                                    case "5"://bi monthly 
                                        inttenure = inttenure / 2;
                                        break;
                                    case "6"://quarterly
                                        inttenure = inttenure / 3;
                                        break;
                                    case "7"://half yearly 
                                        inttenure = inttenure / 6;
                                        break;
                                    case "8"://annually 
                                        inttenure = inttenure / 12;
                                        break;
                                }
                                txtFromInstallNo.Text = "1";
                                txtToInstallNo.Text = Convert.ToString(inttenure);
                                txtFacilityAmount.Text = "";
                                txtFacilityAmount.ReadOnly = false;  //By Siva.K on 24MAR2015
                            }
                            else
                            {
                                txtFromInstallNo.Text = "";
                                txtToInstallNo.Text = "";
                                txtFacilityAmount.ReadOnly = false;  //By Siva.K on 24MAR2015
                            }
                            txtFromInstallNo.ReadOnly = txtToInstallNo.ReadOnly = true;
                            txtFacilityAmount.ReadOnly = true;  //By Siva.K on 24MAR2015
                        }
                    }
                }
            }
            if (strType == "SG")
            {
                if (ddlProposalType.SelectedValue == "3")
                {
                    if (RBLStructuredEI.SelectedValue == "2")
                    {
                        txtFacilityAmount.ReadOnly = true;
                    }
                    else
                    {
                        txtFacilityAmount.ReadOnly = false;
                    }
                }
                else
                {
                    txtFacilityAmount.ReadOnly = true;
                }
            }
            //By Siva.k on 02JUL2015 Remove the Residual Per

            //Hided by Thalai - Validation to be removed, hence the field no longer available - Start
            //string strAssetCategory = string.Empty;
            //strAssetCategory = ddlAssetCategory.SelectedValue;
            //if (!string.IsNullOrEmpty(txtTenure.Text) && !string.IsNullOrEmpty(strAssetCategory))
            //{
            //    Procparam = new Dictionary<string, string>();
            //    Procparam.Add("@Company_ID", intCompany_Id.ToString());
            //    Procparam.Add("@AssetCategory", strAssetCategory);
            //    Procparam.Add("@Tenure", txtTenure.Text);
            //    DataTable dt = Utility.GetDefaultData("S3G_GET_RVPERCENTAGE", Procparam);
            //    if (dt.Rows.Count > 0)
            //    {
            //        //if (!string.IsNullOrEmpty(dt.Rows[0]["RV_Percentage"].ToString()))
            //        //    txtResidualPer.Text = dt.Rows[0]["RV_Percentage"].ToString();
            //    }
            //    else
            //    {
            //        if (ddlAssetCategory.SelectedText != "All")
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('RV Matrix not defined.');", true);
            //            return;
            //        }
            //    }
            //}
            //Hided by Thalai - Validation to be removed, hence the field no longer available - End
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #region "Primary grid"
    /// <summary>
    /// This method is used to set max length for the amount field from primary gridS
    /// </summary>
    private void FunPriSetMaxLength_PG()
    {
        try
        {
            if (grvPrimaryGrid.FooterRow != null)
            {
                TextBox txtFacilityAmountPG = grvPrimaryGrid.FooterRow.FindControl("txtFacilityAmountPG") as TextBox;
                TextBox txtRentRatePG = grvPrimaryGrid.FooterRow.FindControl("txtRentRatePG") as TextBox;
                TextBox txtAMFRentPG = grvPrimaryGrid.FooterRow.FindControl("txtAMFRentPG") as TextBox;
                //By Siva.k on 12MAR2015 for Fixed Rate
                TextBox txtFixedRent = grvPrimaryGrid.FooterRow.FindControl("txtFixedRent") as TextBox;
                TextBox txtFixedAMF = grvPrimaryGrid.FooterRow.FindControl("txtFixedAMF") as TextBox;
                //By Siva.k on 01JUL2015 Remove the Residual Per
                // TextBox txtResidualPerPG = grvPrimaryGrid.FooterRow.FindControl("txtResidualPerPG") as TextBox;
                txtFacilityAmountPG.CheckGPSLength(true, "Facility Amount");

                if (ddlReturnPattern.SelectedValue == "7")
                {
                    txtRentRatePG.SetPercentagePrefixSuffix(10, 8, false, false, "Rent Rate %");
                }
                else
                {
                    txtRentRatePG.SetPercentagePrefixSuffix(10, 8, false, false, "Rent Rate %");
                }

                txtAMFRentPG.SetPercentagePrefixSuffix(10, 8, false, false, "AMF Rate %");
                txtFixedRent.SetPercentagePrefixSuffix(10, 6, false, false, "Fixed Rent %");
                txtFixedAMF.SetPercentagePrefixSuffix(10, 6, false, false, "Fixed AMF %");
                //By Siva.k on 01JUL2015 Remove the Residual Per
                //txtResidualPerPG.SetPercentagePrefixSuffix(3, 5, false, false, "Residual %");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //To define empty primary grid
    /// <summary>
    ///This method is used to define empty primary grid
    /// </summary>
    private void FunPriEmptyPrimaryGrid()
    {
        try
        {
            dtPrimaryGrid = new DataTable();

            dtPrimaryGrid.Columns.Add("slno");
            dtPrimaryGrid.Columns.Add("AssetCategory");
            dtPrimaryGrid.Columns.Add("AssetCategory_ID");
            dtPrimaryGrid.Columns.Add("AssetType");
            dtPrimaryGrid.Columns.Add("AssetType_ID");
            dtPrimaryGrid.Columns.Add("AssetSubType");
            dtPrimaryGrid.Columns.Add("AssetSubType_ID");
            dtPrimaryGrid.Columns.Add("Tenure");
            dtPrimaryGrid.Columns.Add("RentFrequency");
            dtPrimaryGrid.Columns.Add("RentFrequencyID");
            dtPrimaryGrid.Columns.Add("AdvanceArrear");
            dtPrimaryGrid.Columns.Add("AdvanceArrearID");
            dtPrimaryGrid.Columns.Add("FacilityAmount", typeof(decimal));
            dtPrimaryGrid.Columns.Add("RentRate", typeof(decimal));
            dtPrimaryGrid.Columns.Add("AMFRent", typeof(decimal));
            //By Siva.k on 12MAR2015 for Fixed Rate
            dtPrimaryGrid.Columns.Add("FixedRent", typeof(decimal));
            dtPrimaryGrid.Columns.Add("FixedAMF", typeof(decimal));

            /* Changed data type by Thalai - To get max To installment - Ticket - 2194 - on 22-Sep-2015 - Start */
            dtPrimaryGrid.Columns.Add("FromInstallNo", typeof(Int32));
            dtPrimaryGrid.Columns.Add("ToInstallNo", typeof(Int32));
            /* Changed data type by Thalai - To get max To installment - Ticket - 2194 - on 22-Sep-2015 - End */
            // dtPrimaryGrid.Columns.Add("ResidualPer", typeof(decimal)); //By Siva.k on 02JUL2015 Remove the Residual Per

            DataRow drPrimaryGrid = dtPrimaryGrid.NewRow();
            dtPrimaryGrid.Rows.Add(drPrimaryGrid);

            ViewState["dtPrimaryGrid"] = dtPrimaryGrid;
            Session["dtPrimaryGrid"] = (DataTable)ViewState["dtPrimaryGrid"];
            grvPrimaryGrid.DataSource = dtPrimaryGrid;
            grvPrimaryGrid.DataBind();

            grvPrimaryGrid.Rows[0].Visible = false;
            FunPriSetFooterCtrlPG();
            ViewState["dtAssetCategorytbl"] = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //To define footer controls values for primary grid
    /// <summary>
    ///This method is used to define footer controls values primary grid
    /// </summary>
    private void FunPriSetFooterCtrlPG()
    {
        try
        {
            if (grvPrimaryGrid.FooterRow != null)
            {
                //DropDownList ddlAssetCategoryPG = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlAssetCategoryPG");
                DropDownList ddlRentFrequencyPG = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlRentFrequencyPG");
                DropDownList ddlAdvanceArrearPG = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlAdvanceArrearPG");

                if (ViewState["TIME_VALUE"] != null)
                {
                    ddlAdvanceArrearPG.FillDataTable(((DataTable)ViewState["TIME_VALUE"]), "Value", "Name", false);
                }
                if (ViewState["FREQUENCY"] != null)
                {
                    ddlRentFrequencyPG.FillDataTable(((DataTable)ViewState["FREQUENCY"]), "Value", "Name");
                }


                FunAddremoveAssetCategoryDDL();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    ///This user defined function is used to add primary grid details into the grid.
    /// </summary>
    private void FunPriAddgrvPrimaryGridDetails()
    {
        try
        {
            UserControls_S3GAutoSuggest ddlAssetCategoryPG = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetCategoryPG");
            UserControls_S3GAutoSuggest ddlAssetTypePG = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetTypePG");
            UserControls_S3GAutoSuggest ddlAssetSubTypePG = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetSubTypePG");
            TextBox txtTenurePG = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtTenurePG");
            DropDownList ddlRentFrequencyPG = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlRentFrequencyPG");
            DropDownList ddlAdvanceArrearPG = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlAdvanceArrearPG");
            TextBox txtFacilityAmountPG = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFacilityAmountPG");
            TextBox txtFromInstallNoPG = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFromInstallNoPG");
            TextBox txtToInstallNoPG = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtToInstallNoPG");
            TextBox txtRentRatePG = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtRentRatePG");
            TextBox txtAMFRentPG = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtAMFRentPG");
            //By Siva.k on 01JUL2015 Remove the Residual Per
            //TextBox txtResidualPerPG = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtResidualPerPG");
            //By Siva.k on 12MAR2015 for Fixed Rate
            TextBox txtFixedRent = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFixedRent");
            TextBox txtFixedAMF = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFixedAMF");

            //UserControls_S3GAutoSuggest ddlAssetSubTypePG = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetSubTypePG");

            ddlAssetSubTypePG.Enabled = false;

            int intcount = 0;

            DataRow drRow;
            dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];
            if (dtPrimaryGrid.Rows.Count > 0)
                if (dtPrimaryGrid.Rows[0]["slno"].ToString() == "")
                {
                    dtPrimaryGrid.Rows[0].Delete();
                }
            if (txtRentRatePG.Text == "" && txtFixedRent.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Please Enter the Rent Rate or Fixed Rent !.');", true);
                return;
            }

            //tenure exceed condition
            int inttenure = 0;
            if (!string.IsNullOrEmpty(txtTenurePG.Text))
            {
                inttenure = Convert.ToInt16(txtTenurePG.Text);
                switch (ddlRentFrequencyPG.SelectedValue)
                {
                    //case "1"://daily
                    //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                    //    break;
                    //case "2"://Weekly
                    //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                    //    break;
                    //case "4"://Monthly
                    //inttenure = inttenure;
                    //break;
                    case "5"://bi monthly 
                        inttenure = inttenure / 2;
                        break;
                    case "6"://quarterly
                        inttenure = inttenure / 3;
                        break;
                    case "7"://half yearly 
                        inttenure = inttenure / 6;
                        break;
                    case "8"://annually 
                        inttenure = inttenure / 12;
                        break;
                }
            }

            if (ddlRentFrequencyPG.SelectedValue == "5")
            {
                if (txtTenurePG.Text != "")
                {
                    int ChkTenure = Convert.ToInt32(txtTenurePG.Text);
                    if (ChkTenure % 2 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Conversion not possible based on the selected frequency');", true);
                        return;
                    }
                }
            }
            else if (ddlRentFrequencyPG.SelectedValue == "6")
            {
                if (txtTenurePG.Text != "")
                {
                    int ChkTenure = Convert.ToInt32(txtTenurePG.Text);
                    if (ChkTenure % 3 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Conversion not possible based on the selected frequency');", true);
                        return;
                    }
                }
            }
            else if (ddlRentFrequencyPG.SelectedValue == "7")
            {
                if (txtTenurePG.Text != "")
                {
                    int ChkTenure = Convert.ToInt32(txtTenurePG.Text);
                    if (ChkTenure % 6 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Conversion not possible based on the selected frequency');", true);
                        return;
                    }
                }
            }
            else if (ddlRentFrequencyPG.SelectedValue == "8")
            {
                if (txtTenurePG.Text != "")
                {
                    int ChkTenure = Convert.ToInt32(txtTenurePG.Text);
                    if (ChkTenure % 12 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Conversion not possible based on the selected frequency');", true);
                        return;
                    }
                }
            }



            if (Convert.ToInt32(txtFromInstallNoPG.Text) > inttenure)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Installment should not exceed Tenure');", true);
                txtFromInstallNoPG.Focus();
                return;
            }
            if (txtToInstallNoPG.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To Installment should not be empty');", true);
                txtToInstallNoPG.Focus();
                return;
            }

            if (Convert.ToInt32(txtToInstallNoPG.Text) > inttenure)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To Installment should not exceed Tenure having " + ddlRentFrequencyPG.SelectedItem.ToString() + " as frequency');", true);
                txtToInstallNoPG.Focus();
                return;
            }
            //By Siva.K on 24MAR2015
            if (Convert.ToInt32(txtFromInstallNoPG.Text) > Convert.ToInt32(txtToInstallNoPG.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Installment should not exceed To Installment');", true);
                txtToInstallNoPG.Focus();
                return;
            }


            //All condition check

            if (ddlAssetCategoryPG.SelectedText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Asset Category.');", true);
                txtToInstallNoPG.Focus();
                return;
            }

            if (ddlAssetTypePG.SelectedText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Asset Type.');", true);
                txtToInstallNoPG.Focus();
                return;
            }

            //if (ddlAssetSubTypePG.SelectedText.Trim() == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Asset Sub Type.');", true);
            //    txtToInstallNoPG.Focus();
            //    return;
            //}

            if (dtPrimaryGrid.Rows.Count > 0)
            {
                if (ddlAssetCategoryPG.SelectedValue == "0")
                {
                    DataRow[] drPrimaryGridAll = null;
                    drPrimaryGridAll = dtPrimaryGrid.Select(" AssetCategory_ID > " + ddlAssetCategoryPG.SelectedValue + "");
                    if (drPrimaryGridAll.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Asset Category wise already defined.');", true);
                        txtToInstallNoPG.Focus();
                        return;
                    }
                }
                else
                {
                    DataRow[] drPrimaryGridAll0 = null;
                    drPrimaryGridAll0 = dtPrimaryGrid.Select(" AssetCategory_ID = 0");
                    if (drPrimaryGridAll0.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Already `ALL` Asset Category has been chosen, Unable to proceed.');", true);
                        txtToInstallNoPG.Focus();
                        return;
                    }
                }
            }


            //Duplicate validation for equated
            if (RBLStructuredEI.SelectedValue == "1")
            {
                if (dtPrimaryGrid.Rows.Count > 0)
                {
                    DataRow[] drPrimaryGrid1 = null;
                    drPrimaryGrid1 = dtPrimaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategoryPG.SelectedValue + " AND TENURE=" + txtTenurePG.Text.Trim() + " AND RENTFREQUENCYID=" + Convert.ToString(ddlRentFrequencyPG.SelectedValue) + " AND AssetType_ID =" + ddlAssetTypePG.SelectedValue + " AND AssetSubType_ID =" + ddlAssetSubTypePG.SelectedValue);
                    if (drPrimaryGrid1.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Asset Category already added for the entered tenure and frequency');", true);
                        txtToInstallNoPG.Focus();
                        return;
                    }
                }
            }

            if (RBLSecondaryTerm.SelectedValue == "1" && (Convert.ToInt32(txtToInstallNoPG.Text) == inttenure))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Secondary Term selected…! Please check….!');", true);
                txtToInstallNoPG.Focus();
                return;
            }

            //OverLap validation
            //if (dtPrimaryGrid.Rows.Count > 0)
            //{
            //    DataRow[] drPrimaryGrid = null;
            //    drPrimaryGrid = dtPrimaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategoryPG.SelectedValue +
            //        " and (( " + txtFromInstallNoPG.Text.Trim() + " >= FromInstallNo " +
            //        " and " + txtFromInstallNoPG.Text.Trim() + " <= ToInstallNo ) or " +
            //        " ( " + txtToInstallNoPG.Text.Trim() + " >= FromInstallNo and " +
            //        txtToInstallNoPG.Text.Trim() + " <= ToInstallNo) or " +
            //        " ( FromInstallNo >= " + txtFromInstallNoPG.Text.Trim() +
            //        " and FromInstallNo <= " + txtToInstallNoPG.Text.Trim() + " ))");
            //    if (drPrimaryGrid.Count() > 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlaped');", true);
            //        txtToInstallNoPG.Focus();
            //        return;
            //    }
            //}







            try
            {
                intcount = Convert.ToInt32(dtPrimaryGrid.Rows.Count);
            }
            catch (Exception ex)
            {
            }

            drRow = dtPrimaryGrid.NewRow();
            drRow["slno"] = intcount + 1;
            drRow["AssetCategory"] = ddlAssetCategoryPG.SelectedText;
            drRow["AssetCategory_ID"] = ddlAssetCategoryPG.SelectedValue;

            drRow["AssetType"] = ddlAssetTypePG.SelectedText;
            drRow["AssetType_ID"] = ddlAssetTypePG.SelectedValue;

            drRow["AssetSubType"] = ddlAssetSubTypePG.SelectedText;
            drRow["AssetSubType_ID"] = ddlAssetSubTypePG.SelectedValue;

            drRow["Tenure"] = txtTenurePG.Text;
            drRow["RentFrequency"] = ddlRentFrequencyPG.SelectedItem.Text;
            drRow["RentFrequencyID"] = ddlRentFrequencyPG.SelectedValue;
            drRow["AdvanceArrear"] = ddlAdvanceArrearPG.SelectedItem.Text;
            drRow["AdvanceArrearID"] = ddlAdvanceArrearPG.SelectedValue;
            if (!string.IsNullOrEmpty(txtFacilityAmountPG.Text))
                drRow["FacilityAmount"] = Convert.ToDecimal(txtFacilityAmountPG.Text);
            if (!string.IsNullOrEmpty(txtRentRatePG.Text))
                //drRow["RentRate"] = txtRentRatePG.Text;
                drRow["RentRate"] = Convert.ToDecimal(txtRentRatePG.Text);
            //else
            //    drRow["RentRate"] = 0;
            if (!string.IsNullOrEmpty(txtAMFRentPG.Text))
                //drRow["AMFRent"] = txtAMFRentPG.Text;
                drRow["AMFRent"] = Convert.ToDecimal(txtAMFRentPG.Text);
            //else
            //    drRow["AMFRent"] = 0;
            //By Siva.k on 12MAR2015 for Fixed Rate
            if (!string.IsNullOrEmpty(txtFixedRent.Text))
                drRow["FixedRent"] = Convert.ToDecimal(txtFixedRent.Text);
            if (!string.IsNullOrEmpty(txtFixedAMF.Text))
                drRow["FixedAMF"] = Convert.ToDecimal(txtFixedAMF.Text);

            drRow["FromInstallNo"] = txtFromInstallNoPG.Text;
            drRow["ToInstallNo"] = txtToInstallNoPG.Text;
            //By Siva.k on 01JUL2015 Remove the Residual Per
            //if (!string.IsNullOrEmpty(txtResidualPerPG.Text))
            //    drRow["ResidualPer"] = Convert.ToDecimal(txtResidualPerPG.Text);
            dtPrimaryGrid.Rows.Add(drRow);
            ViewState["dtPrimaryGrid"] = dtPrimaryGrid;
            Session["dtPrimaryGrid"] = (DataTable)ViewState["dtPrimaryGrid"];

            FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
            FunPriSetFooterCtrlPG();
            FunPriCalcFacilityAmount();


            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["dtSecondaryGrid"];
            dtSecondaryGrid = dt;
            DataRow[] dr = dtSecondaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategoryPG.SelectedValue + " AND TENURE = " + txtTenurePG.Text.Trim() + " AND RENTFREQUENCYID=" + ddlRentFrequencyPG.SelectedValue);
            if (dr.Length > 0)
            {
                foreach (DataRow drow1 in dr)
                {
                    dtSecondaryGrid.Rows.Remove(drow1);
                }
            }
            ViewState["dtSecondaryGrid"] = dtSecondaryGrid;
            DataRow[] drNew = dtSecondaryGrid.Select(" AssetCategory_ID <> ''");
            if (drNew.Length < 1)
            {
                //if (dtSecondaryGrid.Rows.Count == 0)
                //{
                FunPriEmptySecondaryGrid();
            }
            else
            {
                ViewState["dtSecondaryGrid"] = dtSecondaryGrid;
                FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                FunPriSetFooterCtrlSG();
            }
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    ////This user defined function is used to remove primary grid details into the grid.
    /// </summary>
    private void FunPriRemovegrvPrimaryGridDetails(int intRowIndex)
    {
        try
        {
            Label lblRentFrequencyIDPG = (Label)grvPrimaryGrid.Rows[intRowIndex].FindControl("lblRentFrequencyIDPG");
            Label lblTenurePG = (Label)grvPrimaryGrid.Rows[intRowIndex].FindControl("lblTenurePG");
            Label lblAssetCategoryIDPG = (Label)grvPrimaryGrid.Rows[intRowIndex].FindControl("lblAssetCategoryIDPG");
            Label lblFromInstallNoPG = (Label)grvPrimaryGrid.Rows[intRowIndex].FindControl("lblFromInstallNoPG");

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["dtSecondaryGrid"];
            dtSecondaryGrid = dt;
            DataRow[] dr = dtSecondaryGrid.Select(" AssetCategory_ID = " + lblAssetCategoryIDPG.Text + " AND TENURE = " + lblTenurePG.Text.Trim() + " AND RENTFREQUENCYID=" + Convert.ToString(lblRentFrequencyIDPG.Text));
            if (dr.Length > 0)
            {
                foreach (DataRow drow1 in dr)
                {
                    dtSecondaryGrid.Rows.Remove(drow1);
                }
            }
            ViewState["dtSecondaryGrid"] = dtSecondaryGrid;
            //DataRow[] dr1 = dtSecondaryGrid.Select("AssetCategory_ID = '' AND TENURE = '' AND RENTFREQUENCYID = ''");
            //if (dr1.Length > 0)
            //{
            //    foreach (DataRow drow1 in dr1)
            //    {
            //        dtSecondaryGrid.Rows.Remove(drow1);
            //    }
            //}

            if (dtSecondaryGrid.Rows.Count == 0)
            {
                FunPriEmptySecondaryGrid();
            }
            else
            {
                ViewState["dtSecondaryGrid"] = dtSecondaryGrid;
                FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                FunPriSetFooterCtrlSG();
            }



            //grvSecondaryGrid.DataSource = dtSecondaryGrid;
            //grvSecondaryGrid.DataBind();


            dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];
            if (RBLStructuredEI.SelectedValue == "2")
            {
                if (lblFromInstallNoPG.Text != "1")
                {
                    DataRow[] dr1 = dtPrimaryGrid.Select(" AssetCategory_ID = " + lblAssetCategoryIDPG.Text + " AND TENURE = " + lblTenurePG.Text.Trim() + " AND RENTFREQUENCYID=" + Convert.ToString(lblRentFrequencyIDPG.Text) + " AND FROMINSTALLNO<>1");
                    if (dr1.Length > 0)
                    {
                        foreach (DataRow drow1 in dr1)
                        {
                            dtPrimaryGrid.Rows.Remove(drow1);
                        }
                    }
                    if (dtPrimaryGrid.Rows.Count == 0)
                    {
                        FunPriEmptyPrimaryGrid();
                    }
                    else
                    {
                        FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                        ViewState["dtPrimaryGrid"] = dtPrimaryGrid;
                        Session["dtPrimaryGrid"] = (DataTable)ViewState["dtPrimaryGrid"];
                        FunPriSetFooterCtrlPG();
                    }
                }
                else
                {
                    dtPrimaryGrid.Rows.RemoveAt(intRowIndex);
                    if (dtPrimaryGrid.Rows.Count == 0)
                    {
                        FunPriEmptyPrimaryGrid();
                    }
                    else
                    {

                        FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                        ViewState["dtPrimaryGrid"] = dtPrimaryGrid;
                        Session["dtPrimaryGrid"] = (DataTable)ViewState["dtPrimaryGrid"];
                        FunPriSetFooterCtrlPG();
                    }
                }
            }
            else
            {
                dtPrimaryGrid.Rows.RemoveAt(intRowIndex);
                if (dtPrimaryGrid.Rows.Count == 0)
                {
                    FunPriEmptyPrimaryGrid();
                }
                else
                {

                    FunFillgrid(grvPrimaryGrid, dtPrimaryGrid);
                    ViewState["dtPrimaryGrid"] = dtPrimaryGrid;
                    Session["dtPrimaryGrid"] = (DataTable)ViewState["dtPrimaryGrid"];
                    FunPriSetFooterCtrlPG();
                }

            }
            FunPriCalcFacilityAmount();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }


    private void FunPriCalcFacilityAmount()
    {
        decimal decTotPrimaryFaciltiy = 0;
        decimal decTotSecFacility = 0;
        try
        {
            //Primary grid
            if (ViewState["dtPrimaryGrid"] != null)
                dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];
            if (ViewState["dtSecondaryGrid"] != null)
                dtSecondaryGrid = (DataTable)ViewState["dtSecondaryGrid"];

            if (dtPrimaryGrid.Rows.Count > 0)
            {
                try
                {
                    decTotPrimaryFaciltiy = (decimal)(dtPrimaryGrid.DefaultView.ToTable(true, new string[] { "AssetCategory_ID", "FacilityAmount", "Tenure", "RentFrequencyID" }).Compute("sum(FacilityAmount)", ""));
                }
                catch (Exception ex)
                {
                }
            }
            if (dtSecondaryGrid.Rows.Count > 0)
            {
                try
                {
                    decTotSecFacility = (decimal)(dtSecondaryGrid.DefaultView.ToTable(true, new string[] { "AssetCategory_ID", "FacilityAmount", "Tenure", "RentFrequencyID" }).Compute("sum(FacilityAmount)", ""));
                }
                catch (Exception ex)
                {

                }
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        if (ddlProposalType.SelectedValue == "3")//Extension
        {
            txtTotalFacilityAmount.Text = (decTotSecFacility).ToString();
        }
        else
        {
            txtTotalFacilityAmount.Text = (decTotPrimaryFaciltiy).ToString();
        }

        //txtTotalFacilityAmount.Text = string.Format("{0:C}", Convert.ToDouble(txtTotalFacilityAmount.Text)).Replace("रु ", "").Replace("$", ""); 
    }

    #endregion

    #region "Secondary grid"
    /// <summary>
    /// This method is used to set max length for the amount field from Secondary grid
    /// </summary>
    private void FunPriSetMaxLength_SG()
    {
        try
        {
            if (grvSecondaryGrid.FooterRow != null)
            {
                TextBox txtFacilityAmountSG = grvSecondaryGrid.FooterRow.FindControl("txtFacilityAmountSG") as TextBox;
                TextBox txtRentRateSG = grvSecondaryGrid.FooterRow.FindControl("txtRentRateSG") as TextBox;
                TextBox txtAMFRentSG = grvSecondaryGrid.FooterRow.FindControl("txtAMFRentSG") as TextBox;

                TextBox txtFixedRentSG = grvSecondaryGrid.FooterRow.FindControl("txtFixedRentSG") as TextBox;
                TextBox txtFixedAMFSG = grvSecondaryGrid.FooterRow.FindControl("txtFixedAMFSG") as TextBox;
                //By Siva.k on 01JUL2015 Remove the Residual Per
                //TextBox txtResidualPerSG = grvPrimaryGrid.FooterRow.FindControl("txtResidualPerSG") as TextBox;
                txtFacilityAmountSG.CheckGPSLength(true, "Facility Amount");
                //txtRentRateSG.SetPercentagePrefixSuffix(10, 8, false, false, "Rent Rate %");

                if (ddlReturnPattern.SelectedValue == "7")
                {
                    txtRentRateSG.SetPercentagePrefixSuffix(10, 2, false, false, "Rent Rate %");
                }
                else
                {
                    txtRentRateSG.SetPercentagePrefixSuffix(10, 8, false, false, "Rent Rate %");
                }

                txtAMFRentSG.SetPercentagePrefixSuffix(10, 8, false, false, "AMF Rate %");

                txtFixedRentSG.SetPercentagePrefixSuffix(10, 6, false, false, "Fixed Rent %");
                txtFixedAMFSG.SetPercentagePrefixSuffix(10, 6, false, false, "Fixed AMF %");
                //txtResidualPerSG.SetPercentagePrefixSuffix(3, 5, false, false, "Residual %");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //To define empty Secondary grid
    /// <summary>
    ///This method is used to define empty Secondary grid
    /// </summary>
    private void FunPriEmptySecondaryGrid()
    {
        try
        {
            dtSecondaryGrid = new DataTable();

            dtSecondaryGrid.Columns.Add("slno");
            dtSecondaryGrid.Columns.Add("AssetCategory");
            dtSecondaryGrid.Columns.Add("AssetCategory_ID");
            dtSecondaryGrid.Columns.Add("AssetType");
            dtSecondaryGrid.Columns.Add("AssetType_ID");
            dtSecondaryGrid.Columns.Add("AssetSubType");
            dtSecondaryGrid.Columns.Add("AssetSubType_ID");
            dtSecondaryGrid.Columns.Add("Tenure");
            dtSecondaryGrid.Columns.Add("RentFrequency");
            dtSecondaryGrid.Columns.Add("RentFrequencyID");
            dtSecondaryGrid.Columns.Add("AdvanceArrear");
            dtSecondaryGrid.Columns.Add("AdvanceArrearID");
            dtSecondaryGrid.Columns.Add("FacilityAmount", typeof(decimal));
            dtSecondaryGrid.Columns.Add("RentRate", typeof(decimal));
            dtSecondaryGrid.Columns.Add("AMFRent", typeof(decimal));
            //By Siva.k on 12MAR2015 for Fixed Rate
            dtSecondaryGrid.Columns.Add("FixedRent", typeof(decimal));
            dtSecondaryGrid.Columns.Add("FixedAMF", typeof(decimal));

            /* Changed data type by Thalai - To get max To installment - Ticket - 2194 - on 22-Sep-2015 - Start */
            dtSecondaryGrid.Columns.Add("FromInstallNo", typeof(Int32));
            dtSecondaryGrid.Columns.Add("ToInstallNo", typeof(Int32));
            /* Changed data type by Thalai - To get max To installment - Ticket - 2194 - on 22-Sep-2015 - End */
            //dtSecondaryGrid.Columns.Add("ResidualPer", typeof(decimal)); //By Siva.k on 02JUL2015 Remove the Residual Per

            DataRow drSecondaryGrid = dtSecondaryGrid.NewRow();
            dtSecondaryGrid.Rows.Add(drSecondaryGrid);

            ViewState["dtSecondaryGrid"] = dtSecondaryGrid;
            grvSecondaryGrid.DataSource = dtSecondaryGrid;
            grvSecondaryGrid.DataBind();

            grvSecondaryGrid.Rows[0].Visible = false;
            FunPriSetFooterCtrlSG();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //To define footer controls values for primary grid
    /// <summary>
    ///This method is used to define footer controls values primary grid
    /// </summary>
    private void FunPriSetFooterCtrlSG()
    {
        try
        {
            if (grvSecondaryGrid.FooterRow != null)
            {
                //DropDownList ddlAssetCategoryPG = (DropDownList)grvPrimaryGrid.FooterRow.FindControl("ddlAssetCategoryPG");
                DropDownList ddlRentFrequencySG = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlRentFrequencySG");
                DropDownList ddlAdvanceArrearSG = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlAdvanceArrearSG");

                if (ViewState["TIME_VALUE"] != null)
                {
                    ddlAdvanceArrearSG.FillDataTable(((DataTable)ViewState["TIME_VALUE"]), "Value", "Name", false);
                }
                if (ViewState["FREQUENCY"] != null)
                {
                    ddlRentFrequencySG.FillDataTable(((DataTable)ViewState["FREQUENCY"]), "Value", "Name");
                }
                FunAddremoveAssetCategoryDDL();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    ///This user defined function is used to add secondary grid details into the grid.
    /// </summary>
    private void FunPriAddgrvSecondaryGridDetails()
    {
        try
        {
            UserControls_S3GAutoSuggest ddlAssetCategorySG = (UserControls_S3GAutoSuggest)grvSecondaryGrid.FooterRow.FindControl("ddlAssetCategorySG");
            UserControls_S3GAutoSuggest ddlAssetTypeSG = (UserControls_S3GAutoSuggest)grvSecondaryGrid.FooterRow.FindControl("ddlAssetTypeSG");
            UserControls_S3GAutoSuggest ddlAssetSubTypeSG = (UserControls_S3GAutoSuggest)grvSecondaryGrid.FooterRow.FindControl("ddlAssetSubTypeSG");
            //DropDownList ddlAssetCategorySG = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlAssetCategorySG");
            TextBox txtTenureSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtTenureSG");
            DropDownList ddlRentFrequencySG = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlRentFrequencySG");
            DropDownList ddlAdvanceArrearSG = (DropDownList)grvSecondaryGrid.FooterRow.FindControl("ddlAdvanceArrearSG");
            TextBox txtFacilityAmountSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFacilityAmountSG");
            TextBox txtFromInstallNoSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFromInstallNoSG");
            TextBox txtToInstallNoSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtToInstallNoSG");
            TextBox txtRentRateSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtRentRateSG");
            TextBox txtAMFRentSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtAMFRentSG");
            //By Siva.k on 01JUL2015 Remove the Residual Per
            // TextBox txtResidualPerSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtResidualPerSG");
            //By Siva.k on 12MAR2015 for Fixed Rate
            TextBox txtFixedRentSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFixedRentSG");
            TextBox txtFixedAMFSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFixedAMFSG");

            int intcount = 0;

            DataRow drRow;
            dtSecondaryGrid = (DataTable)ViewState["dtSecondaryGrid"];
            if (dtSecondaryGrid.Rows.Count > 0)
                if (dtSecondaryGrid.Rows[0]["slno"].ToString() == "")
                {
                    dtSecondaryGrid.Rows[0].Delete();
                }
            if (txtRentRateSG.Text == "" && txtFixedRentSG.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Please Enter the Rent Rate or Fixed Rent !.');", true);
                return;
            }

            //tenure exceed condition
            int inttenure = 0;
            if (!string.IsNullOrEmpty(txtTenureSG.Text))
            {
                inttenure = Convert.ToInt16(txtTenureSG.Text);
                switch (ddlRentFrequencySG.SelectedValue)
                {
                    //case "1"://daily
                    //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                    //    break;
                    //case "2"://Weekly
                    //    inttenure = Convert.ToInt16(txtTenureSG.Text);
                    //    break;
                    //case "4"://Monthly
                    //inttenure = inttenure;
                    //break;
                    case "5"://bi monthly 
                        inttenure = inttenure / 2;
                        break;
                    case "6"://quarterly
                        inttenure = inttenure / 3;
                        break;
                    case "7"://half yearly 
                        inttenure = inttenure / 6;
                        break;
                    case "8"://annually 
                        inttenure = inttenure / 12;
                        break;
                }
            }

            if (ddlRentFrequencySG.SelectedValue == "5")
            {
                if (txtTenureSG.Text != "")
                {
                    int ChkTenure = Convert.ToInt32(txtTenureSG.Text);
                    if (ChkTenure % 2 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Conversion not possible based on the selected frequency');", true);
                        return;
                    }
                }
            }
            else if (ddlRentFrequencySG.SelectedValue == "6")
            {
                if (txtTenureSG.Text != "")
                {
                    int ChkTenure = Convert.ToInt32(txtTenureSG.Text);
                    if (ChkTenure % 3 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Conversion not possible based on the selected frequency');", true);
                        return;
                    }
                }
            }
            else if (ddlRentFrequencySG.SelectedValue == "7")
            {
                if (txtTenureSG.Text != "")
                {
                    int ChkTenure = Convert.ToInt32(txtTenureSG.Text);
                    if (ChkTenure % 6 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Conversion not possible based on the selected frequency');", true);
                        return;
                    }
                }
            }
            else if (ddlRentFrequencySG.SelectedValue == "8")
            {
                if (txtTenureSG.Text != "")
                {
                    int ChkTenure = Convert.ToInt32(txtTenureSG.Text);
                    if (ChkTenure % 12 != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Conversion not possible based on the selected frequency');", true);
                        return;
                    }
                }
            }

            if (Convert.ToInt32(txtFromInstallNoSG.Text) > inttenure)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Installment should not exceed Tenure');", true);
                txtFromInstallNoSG.Focus();
                return;
            }
            if (Convert.ToInt32(txtToInstallNoSG.Text) > inttenure)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To Installment should not exceed Tenure');", true);
                txtToInstallNoSG.Focus();
                return;
            }

            //Duplicate validation for equated
            //if (RBLStructuredEI.SelectedValue == "1")
            //{
            //    if (dtSecondaryGrid.Rows.Count > 0)
            //    {
            //        DataRow[] drSecondaryGrid1 = null;
            //        drSecondaryGrid1 = dtSecondaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategorySG.SelectedValue + "");
            //        if (drSecondaryGrid1.Count() > 0)
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Asset Category already added.');", true);
            //            txtToInstallNoSG.Focus();
            //            return;
            //        }
            //    }
            //}

            //All condition check

            if (ddlAssetCategorySG.SelectedText.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Asset Category.');", true);
                txtToInstallNoSG.Focus();
                return;
            }

            if (dtPrimaryGrid.Rows.Count > 0)
            {
                if (ddlAssetCategorySG.SelectedValue == "0")
                {
                    DataRow[] drPrimaryGridAll = null;
                    drPrimaryGridAll = dtPrimaryGrid.Select(" AssetCategory_ID > " + ddlAssetCategorySG.SelectedValue + "");
                    if (drPrimaryGridAll.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Asset Category wise already defined.');", true);
                        txtToInstallNoSG.Focus();
                        return;
                    }
                }
                else
                {
                    DataRow[] drPrimaryGridAll0 = null;
                    drPrimaryGridAll0 = dtPrimaryGrid.Select(" AssetCategory_ID = 0");
                    if (drPrimaryGridAll0.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Already `ALL` Asset Category has been chosen, Unable to proceed.');", true);
                        txtToInstallNoSG.Focus();
                        return;
                    }
                }
            }

            //OverLap validation
            if (dtSecondaryGrid.Rows.Count > 0)
            {
                DataRow[] drSecondaryGrid = null;
                drSecondaryGrid = dtSecondaryGrid.Select(" AssetCategory_ID = " + ddlAssetCategorySG.SelectedValue +
                    //opc114 start
                    " and AssetType_ID=" + ddlAssetTypeSG.SelectedValue.Trim() +
                    //opc114 end
                    //opc117 start
                    " and AssetSubType_ID=" + ddlAssetSubTypeSG.SelectedValue.Trim() +
                    //opc117 end
                    " and tenure=" + txtTenureSG.Text.Trim() + " and RentFrequencyID = " + ddlRentFrequencySG.SelectedValue + " and (( " + txtFromInstallNoSG.Text.Trim() + " >= FromInstallNo " +
                    " and " + txtFromInstallNoSG.Text.Trim() + " <= ToInstallNo ) or " +
                    " ( " + txtToInstallNoSG.Text.Trim() + " >= FromInstallNo and " +
                    txtToInstallNoSG.Text.Trim() + " <= ToInstallNo) or " +
                    " ( FromInstallNo >= " + txtFromInstallNoSG.Text.Trim() +
                    " and FromInstallNo <= " + txtToInstallNoSG.Text.Trim() + " ))");
                if (drSecondaryGrid.Count() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlaped');", true);
                    txtToInstallNoSG.Focus();
                    return;
                }
            }




            try
            {
                intcount = Convert.ToInt32(dtSecondaryGrid.Rows.Count);
            }
            catch (Exception ex)
            {
            }

            drRow = dtSecondaryGrid.NewRow();
            drRow["slno"] = intcount + 1;
            drRow["AssetCategory"] = ddlAssetCategorySG.SelectedText;
            drRow["AssetCategory_ID"] = ddlAssetCategorySG.SelectedValue;
            drRow["AssetType"] = ddlAssetTypeSG.SelectedText;
            drRow["AssetType_ID"] = ddlAssetTypeSG.SelectedValue;
            drRow["AssetSubType"] = ddlAssetSubTypeSG.SelectedText;
            drRow["AssetSubType_ID"] = ddlAssetSubTypeSG.SelectedValue;
            drRow["Tenure"] = txtTenureSG.Text;
            drRow["RentFrequency"] = ddlRentFrequencySG.SelectedItem.Text;
            drRow["RentFrequencyID"] = ddlRentFrequencySG.SelectedValue;
            drRow["AdvanceArrear"] = ddlAdvanceArrearSG.SelectedItem.Text;
            drRow["AdvanceArrearID"] = ddlAdvanceArrearSG.SelectedValue;
            if (!string.IsNullOrEmpty(txtFacilityAmountSG.Text))
                drRow["FacilityAmount"] = Convert.ToDecimal(txtFacilityAmountSG.Text);
            if (!string.IsNullOrEmpty(txtRentRateSG.Text))
                drRow["RentRate"] = Convert.ToDecimal(txtRentRateSG.Text);
            if (!string.IsNullOrEmpty(txtAMFRentSG.Text))
                drRow["AMFRent"] = Convert.ToDecimal(txtAMFRentSG.Text);
            //By Siva.k on 12MAR2015 for Fixed Rate
            if (!string.IsNullOrEmpty(txtFixedRentSG.Text))
                drRow["FixedRent"] = Convert.ToDecimal(txtFixedRentSG.Text);
            if (!string.IsNullOrEmpty(txtFixedAMFSG.Text))
                drRow["FixedAMF"] = Convert.ToDecimal(txtFixedAMFSG.Text);

            drRow["FromInstallNo"] = txtFromInstallNoSG.Text;
            drRow["ToInstallNo"] = txtToInstallNoSG.Text;
            //By Siva.k on 01JUL2015 Remove the Residual Per
            //if (!string.IsNullOrEmpty(txtResidualPerSG.Text))
            //    drRow["ResidualPer"] = Convert.ToDecimal(txtResidualPerSG.Text);
            dtSecondaryGrid.Rows.Add(drRow);
            ViewState["dtSecondaryGrid"] = dtSecondaryGrid;
            FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
            FunPriSetFooterCtrlSG();
            FunPriCalcFacilityAmount();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    ////This user defined function is used to remove Secondary grid details into the grid.
    /// </summary>
    private void FunPriRemovegrvSecondaryGridDetails(int intRowIndex)
    {
        try
        {
            dtSecondaryGrid = (DataTable)ViewState["dtSecondaryGrid"];
            dtSecondaryGrid.Rows.RemoveAt(intRowIndex);
            if (dtSecondaryGrid.Rows.Count == 0)
            {
                FunPriEmptySecondaryGrid();
            }
            else
            {

                FunFillgrid(grvSecondaryGrid, dtSecondaryGrid);
                ViewState["dtSecondaryGrid"] = dtSecondaryGrid;
                FunPriSetFooterCtrlSG();
            }
            FunPriCalcFacilityAmount();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }


    #endregion

    #region "EUC Dtls Code"

    /// <summary>
    /// This user defined function is used to add the End use Customer details in grid.
    /// </summary>
    private void FunPriAddEUCDtls()
    {
        try
        {
            DataRow drEmptyRow;
            dtEUCDetails = (DataTable)ViewState["dtEUCDetails"];

            if (dtEUCDetails.Rows.Count > 0)
            {
                if (dtEUCDetails.Rows[0]["CustomerName"].ToString() == "") //By Siva 29MAY2015 AssetCategory_ID replaced into CustomerName handling Exception
                {
                    dtEUCDetails.Rows[0].Delete();
                }
            }

            ////checking if already exist
            //foreach (DataRow dr in dtEUCDetails.Rows)
            //{
            //    if (dr["AssetCategory_ID"].ToString().Trim() == ddlAssetCategoryEUC.SelectedItem.Text.Trim())
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "Asset Category already Exists");
            //        return;
            //    }
            //}


            drEmptyRow = dtEUCDetails.NewRow();
            drEmptyRow["SlNo"] = dtEUCDetails.Rows.Count + 1;
            //if (ddlAssetCategoryEUC.SelectedIndex > 0)
            //{
            //    drEmptyRow["AssetCategory"] = ddlAssetCategoryEUC.SelectedItem.Text.Trim();
            //    drEmptyRow["AssetCategory_ID"] = ddlAssetCategoryEUC.SelectedValue.Trim();
            //}

            if (!string.IsNullOrEmpty(txtCustomerName_EUC.Text.Trim()))
                drEmptyRow["CustomerName"] = txtCustomerName_EUC.Text.Trim();
            else
                drEmptyRow["CustomerName"] = DBNull.Value;

            if (!string.IsNullOrEmpty(txtEmailId_EUC.Text.Trim()))
                drEmptyRow["EmailId"] = txtEmailId_EUC.Text.Trim();
            else
                drEmptyRow["EmailId"] = DBNull.Value;

            if (!string.IsNullOrEmpty(txtRemarks_EUC.Text.Trim()))
                drEmptyRow["Remarks"] = txtRemarks_EUC.Text.Trim();
            else
                drEmptyRow["Remarks"] = DBNull.Value;

            dtEUCDetails.Rows.Add(drEmptyRow);
            ViewState["dtEUCDetails"] = dtEUCDetails;
            FunFillgrid(grvEUC, dtEUCDetails);

            FunPriClearEUC();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to modify the End use Customer details in grid.
    /// </summary>
    private void FunPriModifyEUCdtls()
    {
        try
        {
            dtEUCDetails = (DataTable)ViewState["dtEUCDetails"];

            DataRow drow = dtEUCDetails.Rows[Convert.ToInt32(lblEUCSlNo.Text) - 1];
            drow.BeginEdit();
            drow["SlNo"] = lblEUCSlNo.Text;
            //if (ddlAssetCategoryEUC.SelectedIndex > 0)
            //{
            //    drow["AssetCategory"] = ddlAssetCategoryEUC.SelectedItem.Text.Trim();
            //    drow["AssetCategory_ID"] = ddlAssetCategoryEUC.SelectedValue;
            //}
            //else
            //{
            //    drow["AssetCategory"] = drow["AssetCategory_ID"] = DBNull.Value;
            //}

            if (!string.IsNullOrEmpty(txtCustomerName_EUC.Text))
                drow["CustomerName"] = txtCustomerName_EUC.Text;
            if (!string.IsNullOrEmpty(txtEmailId_EUC.Text))
                drow["EmailId"] = txtEmailId_EUC.Text;
            if (!string.IsNullOrEmpty(txtRemarks_EUC.Text))
                drow["Remarks"] = txtRemarks_EUC.Text;

            drow.EndEdit();
            ViewState["dtEUCDetails"] = dtEUCDetails;
            FunFillgrid(grvEUC, dtEUCDetails);
            FunPriClearEUC();
            btnAddEUC.Enabled = btnhClearEUC.Enabled = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    ///  This user defined function is used to select the End use Customer details from grid to controls.
    /// </summary>
    /// <param name="sender"></param>
    private void FunPriRBSelectIndexChange(object sender)
    {
        try
        {
            btnModifyEUC.Enabled = true;
            btnAddEUC.Enabled = btnhClearEUC.Enabled = false;

            int intRowIndex = Utility.FunPubGetGridRowID("grvEUC", ((RadioButton)sender).ClientID);
            dtEUCDetails = (DataTable)ViewState["dtEUCDetails"];

            FunPriResetRdButton(grvEUC, intRowIndex);

            DataRow drow = dtEUCDetails.Rows[intRowIndex];
            lblEUCSlNo.Text = drow["SlNo"].ToString();
            if (!string.IsNullOrEmpty(drow["AssetCategory_ID"].ToString()))
                ddlAssetCategoryEUC.SelectedValue = drow["AssetCategory_ID"].ToString();
            txtCustomerName_EUC.Text = drow["CustomerName"].ToString();
            txtEmailId_EUC.Text = drow["EmailId"].ToString();
            txtRemarks_EUC.Text = drow["Remarks"].ToString();
            if (PageMode == PageModes.Query)
            {
                btnModifyEUC.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to delete the End use Customer details from grid.
    /// </summary>
    /// <param name="intRowIndex"></param>
    private void FunPriRemoveEUCDetails(int intRowIndex)
    {
        try
        {
            dtEUCDetails = (DataTable)ViewState["dtEUCDetails"];
            dtEUCDetails.Rows.RemoveAt(intRowIndex);
            if (dtEUCDetails.Rows.Count == 0)
            {
                FunPriSetEmptyEUCtbl();
            }
            else
            {
                FunFillgrid(grvEUC, dtEUCDetails);
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to clear the End use Customer details controls .
    /// </summary>
    private void FunPriClearEUC()
    {
        try
        {
            ddlAssetCategoryEUC.SelectedIndex = -1;
            txtCustomerName_EUC.Text = txtEmailId_EUC.Text = txtRemarks_EUC.Text = string.Empty;
            lblEUCSlNo.Text = string.Empty;
            btnModifyEUC.Enabled = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to default empty datatable for the End use Customer details .
    /// </summary>
    private void FunPriSetEmptyEUCtbl()
    {
        try
        {
            DataRow drEmptyRow;
            dtEUCDetails = new DataTable();
            dtEUCDetails.Columns.Add("SlNo");
            //dtEUCDetails.Columns.Add("AssetCategory");
            //dtEUCDetails.Columns.Add("AssetCategory_ID");
            dtEUCDetails.Columns.Add("CustomerName");
            dtEUCDetails.Columns.Add("EmailId");
            dtEUCDetails.Columns.Add("Remarks");
            drEmptyRow = dtEUCDetails.NewRow();
            dtEUCDetails.Rows.Add(drEmptyRow);
            ViewState["dtEUCDetails"] = dtEUCDetails;
            FunFillgrid(grvEUC, dtEUCDetails);
            grvEUC.Rows[0].Visible = false;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This user defined function is used to reset all select option from the end use customer grid
    /// </summary>
    /// <param name="grv"></param>
    /// <param name="intRowIndex"></param>
    private void FunPriResetRdButton(GridView grv, int intRowIndex)
    {
        try
        {
            for (int i = 0; i <= grv.Rows.Count - 1; i++)
            {
                if (i != intRowIndex)
                {
                    RadioButton rdSelect = grv.Rows[i].FindControl("RBSelect") as RadioButton;
                    rdSelect.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    [System.Web.Services.WebMethod]
    public static string[] GetCustomerName_EUCList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@Customer_Id", obj_PageValue.hdnCustID.Value);
        Procparam.Add("@PrefixText", prefixText.Trim());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetCustomerName_EUCList", Procparam));
        return suggetions.ToArray();
    }



    #endregion

    /// <summary>
    /// This method is used to check page validation and user defined validation before saving proposal.
    /// </summary>
    /// <returns></returns>
    private bool FunPriCheckIsPageValid()
    {
        bool returnValue = true;
        try
        {

            decimal decTotAmount = 0;

            //primary grid empty
            if (ViewState["dtPrimaryGrid"] != null)
                dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];
            if (ddlProposalType.SelectedValue != "3")//Extension
            {
                if (string.IsNullOrEmpty(dtPrimaryGrid.Rows[0]["slno"].ToString()))
                {
                    Utility.FunShowAlertMsg(this.Page, "Add atleast one record in Primary grid.");
                    returnValue = false;
                    return false;
                }
            }

            DataTable dtSecondaryDistinct = new DataTable();
            if (ViewState["dtSecondaryGrid"] != null)
                dtSecondaryGrid = (DataTable)ViewState["dtSecondaryGrid"];

            DataTable dtRebateDetGrid = new DataTable();
            if (ViewState["dtRebateDetGrid"] != null)
                dtRebateDetGrid = (DataTable)ViewState["dtRebateDetGrid"];


            DataTable dtPrimaryDistinct = (dtPrimaryGrid.DefaultView.ToTable(true, new string[] { "AssetCategory_ID", "Tenure", "RentFrequencyID", "FacilityAmount" })).Copy();
            //Secondry grid validations
            if (RBLSecondaryTerm.SelectedValue == "1")//YES
            {
                foreach (DataRow row in dtPrimaryGrid.Rows)
                {
                    int tenure = 0;
                    if (Convert.ToString(row["RentFrequencyID"]) != "")
                    {
                        tenure = Convert.ToInt32(row["Tenure"]);
                        string Frequency = Convert.ToString(row["RentFrequencyID"]);
                        switch (Frequency)
                        {
                            case "5"://Bi monthly
                                tenure = tenure / 2;
                                break;
                            case "6"://quarterly
                                tenure = tenure / 3;
                                break;
                            case "7"://half yearly 
                                tenure = tenure / 6;
                                break;
                            case "8"://annually 
                                tenure = tenure / 12;
                                break;
                        }
                        if (tenure != Convert.ToInt32(row["ToInstallNo"]))
                        {
                            DataRow[] dSecondaryrow = dtSecondaryGrid.Select(" AssetCategory_ID = '" + row["AssetCategory_ID"] + "' and Tenure = '" + row["Tenure"] + "' and RentFrequencyID = '" + row["RentFrequencyID"] + "'");
                            if (dSecondaryrow.Length == 0)
                            {
                                Utility.FunShowAlertMsg(this.Page, "Define asset category in secondary grid for " + row["AssetCategory"] + " !.");
                                returnValue = false;
                                return false;
                            }
                            /* Added type by Thalai - Ensure total tenure entered fully - Ticket - 2194 - on 22-Sep-2015 - Start */
                            else
                            {
                                Int32 intMaxInstall = 0;
                                intMaxInstall = Convert.ToInt32(dtSecondaryGrid.DefaultView.
                                    ToTable(true, new string[] { "AssetCategory_ID", "FacilityAmount", "Tenure", "RentFrequencyID", "ToInstallNo" }).
                                    Compute("Max(ToInstallNo)", " AssetCategory_ID = '" + row["AssetCategory_ID"]
                                    + "' and Tenure = '" + row["Tenure"] + "' and RentFrequencyID = '" + row["RentFrequencyID"] + "'"));
                                if (tenure != intMaxInstall)
                                {
                                    Utility.FunShowAlertMsg(this.Page, "Total tenure " + row["Tenure"]
                                        + " not entered fully for " + row["AssetCategory"] + " !.");
                                    returnValue = false;
                                    return false;
                                }
                            }
                            /* Added type by Thalai - Ensure total tenure entered fully - Ticket - 2194 - on 22-Sep-2015 - End */
                        }
                    }
                }//END For Loop

                //if (string.IsNullOrEmpty(dtSecondaryGrid.Rows[0]["slno"].ToString()))
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Define asset category in secondary grid.");
                //    returnValue = false;
                //    return false;
                //}

                //if (dtSecondaryGrid.Rows.Count > 0)
                //{
                //    //DataRow[] drow = dtPrimaryGrid.Select(" AssetCategory_ID = '" + ddlass + "' and Tenure = '" + + "' and RenFrequencyID = '" + + "'");

                //    dtSecondaryDistinct = (dtSecondaryGrid.DefaultView.ToTable(true, new string[] { "AssetCategory_ID", "Tenure", "RentFrequencyID", "FacilityAmount" })).Copy();
                //    int PriCount = dtPrimaryDistinct.Rows.Count;
                //    int SecCount = dtSecondaryDistinct.Rows.Count;
                //    if (PriCount > SecCount)
                //    {
                //        Utility.FunShowAlertMsg(this.Page, "All asset category defined in primary grid should be defined in secondary grid.");
                //        returnValue = false;
                //        return false;
                //    }

                //}

            }
            /* Added type by Thalai - Ensure total tenure entered fully - Ticket - 2194 - on 22-Sep-2015 - Start */
            else
            {
                foreach (DataRow row in dtPrimaryGrid.Rows)
                {
                    int tenure = 0;
                    if (Convert.ToString(row["RentFrequencyID"]) != "")
                    {
                        tenure = Convert.ToInt32(row["Tenure"]);
                        string Frequency = Convert.ToString(row["RentFrequencyID"]);
                        switch (Frequency)
                        {
                            case "5"://Bi monthly
                                tenure = tenure / 2;
                                break;
                            case "6"://quarterly
                                tenure = tenure / 3;
                                break;
                            case "7"://half yearly 
                                tenure = tenure / 6;
                                break;
                            case "8"://annually 
                                tenure = tenure / 12;
                                break;
                        }

                        Int32 intMaxInstall = 0;

                        intMaxInstall = Convert.ToInt32(dtPrimaryGrid.
                            Compute("Max(ToInstallNo)", " AssetCategory_ID = '" + row["AssetCategory_ID"]
                            + "' and Tenure = '" + row["Tenure"] + "' and RentFrequencyID = '" + row["RentFrequencyID"] + "'"));

                        intMaxInstall = Convert.ToInt32(dtPrimaryGrid.DefaultView.
                            ToTable(true, new string[] { "AssetCategory_ID", "FacilityAmount", "Tenure", "RentFrequencyID", "ToInstallNo" }).
                            Compute("Max(ToInstallNo)", " AssetCategory_ID = '" + row["AssetCategory_ID"]
                            + "' and Tenure = '" + row["Tenure"] + "' and RentFrequencyID = '" + row["RentFrequencyID"] + "'"));
                        if (tenure != intMaxInstall)
                        {
                            Utility.FunShowAlertMsg(this.Page, "Total tenure " + row["Tenure"]
                                + " not entered fully for " + row["AssetCategory"] + " !.");
                            returnValue = false;
                            return false;
                        }
                    }
                }
            }
            /* Added type by Thalai - Ensure total tenure entered fully - Ticket - 2194 - on 22-Sep-2015 - End */
            if (dtSecondaryGrid.Rows.Count > 0)
            {
                dtSecondaryDistinct = (dtSecondaryGrid.DefaultView.ToTable(true, new string[] { "AssetCategory_ID", "Tenure", "RentFrequencyID", "FacilityAmount" })).Copy();
            }

            //Total Amount validation
            decimal decFacilityPG = 0, decFacilitySG = 0;

            if (!string.IsNullOrEmpty(txtTotalFacilityAmount.Text))
                decTotAmount = Convert.ToDecimal(txtTotalFacilityAmount.Text);

            if (dtPrimaryDistinct.Rows.Count > 0)
            {
                try
                {
                    decFacilityPG = (decimal)dtPrimaryDistinct.Compute("Sum(FacilityAmount)", "FacilityAmount > 0");
                }
                catch (Exception ex)
                {
                }
            }

            if (dtSecondaryDistinct != null)
                if (dtSecondaryDistinct.Rows.Count > 0)
                {
                    try
                    {
                        decFacilitySG = (decimal)dtSecondaryDistinct.Compute("Sum(FacilityAmount)", "FacilityAmount > 0");
                    }
                    catch (Exception ex)
                    {
                    }
                }



            if (ddlProposalType.SelectedValue == "3")//Extension
            {
                if (decTotAmount != (decFacilitySG))
                {
                    Utility.FunShowAlertMsg(this.Page, "Total facility amount should be equal to sum of asset category facility amount.");
                    returnValue = false;
                    return false;
                }
            }
            else
            {
                if (decTotAmount != (decFacilityPG))
                {
                    Utility.FunShowAlertMsg(this.Page, "Total facility amount should be equal to sum of asset category facility amount.");
                    returnValue = false;
                    return false;
                }
            }


            if (RBLRebateStructuredEI.SelectedValue == "2")//Structure
            {
                if (string.IsNullOrEmpty(dtRebateDetGrid.Rows[0]["FromInstallNo"].ToString()))
                {
                    Utility.FunShowAlertMsg(this.Page, "Add atleast one record in Rebate Structure grid.");
                    returnValue = false;
                    return false;
                }
            }

            int intMaxInstPriSec = 0;
            Decimal decTotRebatePerGrid = 0;
            foreach (DataRow row in dtPrimaryGrid.Rows)
            {
                if (row["ToInstallNo"].ToString() != "")
                    intMaxInstPriSec = Convert.ToInt32(row["ToInstallNo"]);
            }

            foreach (DataRow row in dtSecondaryGrid.Rows)
            {
                if (row["ToInstallNo"].ToString() != "")
                    intMaxInstPriSec = Convert.ToInt32(row["ToInstallNo"]);
            }

            foreach (DataRow row in dtRebateDetGrid.Rows)
            {
                if (row["RebatePerc"].ToString() != "")
                    decTotRebatePerGrid = decTotRebatePerGrid + Convert.ToDecimal(row["RebatePerc"]);
            }

            //opc058 start
            if (txtRebateNoofInstall.Text != "")
            {
                if (Convert.ToInt32(txtRebateNoofInstall.Text) > intMaxInstPriSec)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Maximum Installment No of Primary / Secondary is " + intMaxInstPriSec.ToString() + " . So Rebate Allowed in installments (Nos.) cannot be > " + intMaxInstPriSec.ToString() + " ');", true);
                    return false;
                }
            }

            if (txtAddiRebateNoofInstall.Text != "")
            {
                if (Convert.ToInt32(txtAddiRebateNoofInstall.Text) > intMaxInstPriSec)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Maximum Installment No of Primary / Secondary is " + intMaxInstPriSec.ToString() + " . So Additional Rebate Allowed in installments (Nos.) cannot be > " + intMaxInstPriSec.ToString() + " ');", true);
                    return false;
                }
            }

            if (ddlRebateStrucAlloc.SelectedValue == "2")
            {
                if (decTotRebatePerGrid != 100)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Total Rebate Percentage should be 100 in Rebate Structure Grid');", true);
                    return false;
                }
            }

            //opc58 end

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Due to Data Problem,Unable to Save";
            CVProposal.IsValid = false;
            returnValue = false;
        }
        return returnValue;
    }

    /// <summary>
    /// This method is used to save proposal details
    /// </summary>
    private void FunPriClearProposalDtls()
    {
        try
        {
            S3GCustomerAddress1.ClearCustomerDetails();

            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Text =
            txtProposalNumber.Text =
            //txtOfferDate.Text = 
            txtProcessingFee.Text =
            txtRemarks.Text = txtGuaranteedEOT.Text =
       txtOneTimeFee.Text =
            txtTotalFacilityAmount.Text = txtProcessingRate.Text = txtProcessingAmount.Text =
       txtSecDepAdvRent.Text = txtOnetimeRate.Text = txtOneTimeAmount.Text =
            txtOfferValidTill.Text = string.Empty;
            txtTermSheetDate.Text = string.Empty;
            lblRoundNo.Text = "0";
            ddlBranchList.SelectedValue = "0";
            ddlBranchList.Clear();
            ddlAmtBasedOn.SelectedIndex = 0;
            ddlProcessingBasedOn.SelectedIndex = 0;
            ddlProposalType.SelectedIndex = -1;
            ddlSecuritydeposit.SelectedIndex = -1;
            RBLAdvanceRent.SelectedValue = "0";
            RBLSecondaryTerm.SelectedValue = "0";
            RBLStructuredEI.SelectedValue = "1";
            //RBLVATRebate.SelectedValue = "0";
            ddlReturnPattern.SelectedIndex = -1;
            ddlGuaranteedEOTApp.SelectedValue = "0";
            ddlRentalBasedOn.SelectedValue = ddlSecondaryRentalBasedOn.SelectedValue = "0";
            FunPriEmptyPrimaryGrid();
            FunPriEmptySecondaryGrid();
            FunPriClearEUC();
            ViewState["dtEUCDetails"] = null;
            grvEUC.DataSource = null;
            grvEUC.DataBind();

            lnkViewCustomer.Enabled = false;
            ddlBranchList.ReadOnly = sugLeadRefNo.ReadOnly = false;
            ucCustomerCodeLov.Visible = true;
            rfvcmbCustomer.Enabled = rfvCustomer.Enabled = true;
            sugLeadRefNo.Clear();
            Label lblCustomerCode = (Label)S3GCustomerAddress1.FindControl("lblCustomerCode");
            Label lblCustomerName1 = (Label)S3GCustomerAddress1.FindControl("lblCustomerName");
            lblCustomerCode.Text = lblCustomerName.Text = "Customer Code";
            lblCustomerName1.Text = "Customer Name";
            pnlCustomerInformation.GroupingText = "Customer Informations";

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }



    /// <summary>
    /// This method is used to save proposal details
    /// </summary>
    private void FunPriSaveProposalDtls()
    {
        ObjPricingMgtServices = new PricingMgtServicesReference.PricingMgtServicesClient();
        string strOffer_No = string.Empty;
        try
        {
            if (FunPriCheckIsPageValid())
            {

                //if(rdbtnOneTime.Checked==false && rdtbnProcessing.Checked ==false)
                //{
                //    Utility.FunShowAlertMsg(this, "Select atleast one type - one time or processing");
                //    return;
                //}

                //if (ddlBranchList.SelectedText == "--Select--" || ddlBranchList.SelectedText == " ")
                //{
                //    Utility.FunShowAlertMsg(this, "Select Location");
                //    return;
                //}
                //FunProUploadFiles(lblActualPath.Text);
                ObjS3G_ORG_Pricing = new PricingMgtServices.S3G_ORG_PricingDataTable();
                PricingMgtServices.S3G_ORG_PricingRow ObjPricingRow;
                ObjPricingRow = ObjS3G_ORG_Pricing.NewS3G_ORG_PricingRow();

                ObjPricingRow.Company_ID = intCompany_Id;
                if (!string.IsNullOrEmpty(hdnCustID.Value))
                {
                    ObjPricingRow.Customer_ID = Convert.ToInt32(hdnCustID.Value);
                }
                ObjPricingRow.Pricing_ID = intPricingId;
                ObjPricingRow.LOB_ID = Convert.ToInt32(ddlLob.SelectedValue);
                ObjPricingRow.Branch_ID = Convert.ToInt32(ddlBranchList.SelectedValue);


                if (!string.IsNullOrEmpty(sugLeadRefNo.SelectedValue))
                    ObjPricingRow.Lead_ID = Convert.ToInt32(sugLeadRefNo.SelectedValue);
                if (!string.IsNullOrEmpty(txtOfferDate.Text))
                    ObjPricingRow.Offer_Date = Utility.StringToDate(txtOfferDate.Text);
                if (!string.IsNullOrEmpty(txtTotalFacilityAmount.Text))
                    ObjPricingRow.Facility_Amount = Convert.ToDecimal(txtTotalFacilityAmount.Text);
                if (!string.IsNullOrEmpty(txtOfferValidTill.Text))
                    ObjPricingRow.Offer_Valid_Till = Utility.StringToDate(txtOfferValidTill.Text);

                if (!string.IsNullOrEmpty(ddlProposalType.SelectedValue))
                    ObjPricingRow.Proposal_Type = Convert.ToInt32(ddlProposalType.SelectedValue);
                ObjPricingRow.Adv_Rent__Applicability = Convert.ToInt32(RBLAdvanceRent.SelectedValue);
                if (!string.IsNullOrEmpty(ddlSecuritydeposit.SelectedValue))
                    ObjPricingRow.Secu_Deposit_Type = Convert.ToInt32(ddlSecuritydeposit.SelectedValue);
                if (!string.IsNullOrEmpty(txtSecDepAdvRent.Text))
                    ObjPricingRow.Secu_Rent_Amount = Convert.ToDecimal(txtSecDepAdvRent.Text);
                if (!string.IsNullOrEmpty(ddlReturnPattern.SelectedValue))
                    ObjPricingRow.ReturnPattern = Convert.ToInt32(ddlReturnPattern.SelectedValue);

                ObjPricingRow.Seco_Term_Applicability = Convert.ToInt32(RBLSecondaryTerm.SelectedValue);

                ObjPricingRow.Repayment_Mode = Convert.ToInt32(RBLStructuredEI.SelectedValue);

                if (ddlMRANumber.SelectedValue != "")
                    ObjPricingRow.MRA_ID = Convert.ToInt32(ddlMRANumber.SelectedValue);

                if (!string.IsNullOrEmpty(txtOneTimeAmount.Text))
                    ObjPricingRow.One_Time_Fee = Convert.ToDecimal(txtOneTimeAmount.Text);
                if (!string.IsNullOrEmpty(txtOnetimeRate.Text))
                    ObjPricingRow.ONE_TIME_RATE = Convert.ToDecimal(txtOnetimeRate.Text);

                if (!string.IsNullOrEmpty(txtProcessingAmount.Text))
                    ObjPricingRow.Processing_Fee_Per = Convert.ToDecimal(txtProcessingAmount.Text);
                if (!string.IsNullOrEmpty(txtProcessingRate.Text))
                    ObjPricingRow.PROCESSING_FEE_RATE = Convert.ToDecimal(txtProcessingRate.Text);

                ObjPricingRow.Amt_Based_On = Convert.ToInt32(ddlAmtBasedOn.SelectedValue);
                ObjPricingRow.PROCESSING_AMT_BASED_ON = Convert.ToInt32(ddlProcessingBasedOn.SelectedValue);

                //ObjPricingRow.VAT_Rebate_Applicability = Convert.ToInt32(RBLVATRebate.SelectedValue);

                if (!string.IsNullOrEmpty(txtRemarks.Text))
                    ObjPricingRow.Remarks = txtRemarks.Text;

                if (!string.IsNullOrEmpty(txtGuaranteedEOT.Text))
                    ObjPricingRow.Guaranteed_EOT = txtGuaranteedEOT.Text;
                if (!string.IsNullOrEmpty(ddlGuaranteedEOTApp.SelectedValue))
                    ObjPricingRow.Guaranteed_EOT_App = ddlGuaranteedEOTApp.SelectedValue;

                ObjPricingRow.Status_ID = 44;//default pending
                if (!string.IsNullOrEmpty(lblRoundNo.Text))
                    ObjPricingRow.Round_No = Convert.ToInt32(lblRoundNo.Text);

                ObjPricingRow.Is_Guaranteed_EOT_Appli = Convert.ToInt32(rblIs_Guaranteed_EOT_Appli.SelectedValue);

                if (rblRebateDiscountApp.SelectedValue != "" && rblRebateDiscountApp.SelectedValue != "0")
                    ObjPricingRow.Rebate_Discount_Appl = Convert.ToInt32(rblRebateDiscountApp.SelectedValue);
                else
                    ObjPricingRow.Rebate_Discount_Appl = Convert.ToInt32("0");

                ObjPricingRow.Addi_Rebate_Discount_Appl = Convert.ToInt32(rblAddiRebateDiscountApp.SelectedValue);

                if (Convert.ToInt32(ddlRebateStrucAlloc.SelectedValue) > 0)
                    ObjPricingRow.Rebate_Struc_Allocation = Convert.ToInt32(ddlRebateStrucAlloc.SelectedValue);

                if (!string.IsNullOrEmpty(txtRebateDiscountPerc.Text))
                    ObjPricingRow.Rebate_Discount_Perc = Convert.ToDecimal(txtRebateDiscountPerc.Text);

                if (!string.IsNullOrEmpty(txtAddiRebateDiscountPerc.Text))
                    ObjPricingRow.Addi_Rebate_Discount_Perc = Convert.ToDecimal(txtAddiRebateDiscountPerc.Text);

                if (!string.IsNullOrEmpty(txtRebateNoofInstall.Text))
                    ObjPricingRow.No_of_Installments_Rebate = Convert.ToInt32(txtRebateNoofInstall.Text);
                if ((RBLRebateStructuredEI.SelectedValue != "") && Convert.ToInt32(RBLRebateStructuredEI.SelectedValue) > 0)
                    ObjPricingRow.Rebate_Allowed_Method = Convert.ToInt32(RBLRebateStructuredEI.SelectedValue);

                if (!string.IsNullOrEmpty(txtAddiRebateNoofInstall.Text))
                    ObjPricingRow.Addi_No_of_Installments_Rebate = Convert.ToInt32(txtAddiRebateNoofInstall.Text);
                if ((RBLAddiRebateStructuredEI.SelectedValue != "") && Convert.ToInt32(RBLAddiRebateStructuredEI.SelectedValue) > 0)
                    ObjPricingRow.Addi_Rebate_Allowed_Method_N = Convert.ToInt32(RBLAddiRebateStructuredEI.SelectedValue);

                //ObjPricingRow.Business_Offer_Number = ;
                //ObjPricingRow.Product_ID = ;
                //ObjPricingRow.Constitution_ID = ;
                //ObjPricingRow.Customer_IRR = ;
                // ObjPricingRow.Status_ID = ;
                //ObjPricingRow.Round_No = ;
                //  ObjPricingRow.Auth_ID = ;
                ObjPricingRow.XMLPrimaryGrid = "<Root></Root>";
                ObjPricingRow.XMLSecondaryGrid = "<Root></Root>";
                ObjPricingRow.XMLEUCDtls = "<Root></Root>";

                if (ViewState["dtPrimaryGrid"] != null)
                {
                    dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];
                    foreach (DataRow row in dtPrimaryGrid.Rows)
                    {
                        if (Convert.ToString(row["RentRate"]) == "")
                        {
                            row["RentRate"] = 0;
                        }
                        if (Convert.ToString(row["AMFRent"]) == "")
                            row["AMFRent"] = 0;
                        if (Convert.ToString(row["FixedRent"]) == "")
                            row["FixedRent"] = 0;
                        if (Convert.ToString(row["FixedAMF"]) == "")
                            row["FixedAMF"] = 0;

                    }
                    dtPrimaryGrid.AcceptChanges();
                    if (!string.IsNullOrEmpty(dtPrimaryGrid.Rows[0]["slno"].ToString()))
                    {
                        ObjPricingRow.XMLPrimaryGrid = dtPrimaryGrid.FunPubFormXml(true);
                    }
                }
                if (ViewState["dtSecondaryGrid"] != null)
                {
                    dtSecondaryGrid = (DataTable)ViewState["dtSecondaryGrid"];
                    foreach (DataRow row in dtSecondaryGrid.Rows)
                    {

                        if (Convert.ToString(row["RentRate"]) == "")
                        {
                            row["RentRate"] = 0;
                        }
                        if (Convert.ToString(row["AMFRent"]) == "")
                            row["AMFRent"] = 0;
                        if (Convert.ToString(row["FixedRent"]) == "")
                            row["FixedRent"] = 0;
                        if (Convert.ToString(row["FixedAMF"]) == "")
                            row["FixedAMF"] = 0;

                    }
                    dtSecondaryGrid.AcceptChanges();

                    if (!string.IsNullOrEmpty(dtSecondaryGrid.Rows[0]["slno"].ToString()))
                    {
                        ObjPricingRow.XMLSecondaryGrid = dtSecondaryGrid.FunPubFormXml(true);
                    }
                }

                ObjPricingRow.XMLRebateDetGrid = "<Root></Root>";
                if (ViewState["dtRebateDetGrid"] != null)
                {

                    dtRebateDetGrid = (DataTable)ViewState["dtRebateDetGrid"];
                    foreach (DataRow row in dtRebateDetGrid.Rows)
                    {
                        if (Convert.ToString(row["RebatePerc"]) == "")
                        {
                            row["RebatePerc"] = 0;
                        }

                    }
                    if (!string.IsNullOrEmpty(dtRebateDetGrid.Rows[0]["RebateDet_ID"].ToString()))
                    {
                        ObjPricingRow.XMLRebateDetGrid = dtRebateDetGrid.FunPubFormXml(true);
                    }
                }

                if (ViewState["dtEUCDetails"] != null)
                {
                    dtEUCDetails = (DataTable)ViewState["dtEUCDetails"];

                    if (!string.IsNullOrEmpty(dtEUCDetails.Rows[0]["CustomerName"].ToString()))
                    {
                        ObjPricingRow.XMLEUCDtls = dtEUCDetails.FunPubFormXml(true);
                    }
                }
                ObjPricingRow.Created_By = intUserId;

                ObjPricingRow.Sec_Dep_App = Convert.ToInt32(ddlSecDepApp.SelectedValue);
                ObjPricingRow.Rental_Based_On = Convert.ToInt32(ddlRentalBasedOn.SelectedValue);
                if (!string.IsNullOrEmpty(txtTermSheetDate.Text))
                    ObjPricingRow.Term_Sheet_Date = Utility.StringToDate(txtTermSheetDate.Text);
                //ObjPricingRow.Secondary_Rental_Based_On = Convert.ToInt32(ddlSecondaryRentalBasedOn.SelectedValue);
                ObjPricingRow.Upload_Path = ViewState["Document_Path"].ToString();

                //Sathya
                //ObjPricingRow.Stamp_Duty_App = Convert.ToInt32(ddlStampDuty.SelectedValue);
                //ObjPricingRow.Interim_Applicable = Convert.ToInt32(ddlInterimDetails.SelectedValue);

                ObjS3G_ORG_Pricing.AddS3G_ORG_PricingRow(ObjPricingRow);

                SerializationMode SerMode = SerializationMode.Binary;
                byte[] ObjPricingDataTable = ClsPubSerialize.Serialize(ObjS3G_ORG_Pricing, SerMode);

                intResult = ObjPricingMgtServices.FunPubCreatePricingInt(out strOffer_No, SerMode, ObjPricingDataTable);
                FunProClearCachedFiles();
                if (intResult == 0)
                {
                    if (intPricingId > 0)
                    {
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strOffer_No = txtProposalNumber.Text;
                        strAlert = strAlert.Replace("__ALERT__", "Pricing-Proposal " + strOffer_No + " updated successfully");
                    }
                    else
                    {
                        txtProposalNumber.Text = strOffer_No;
                        strAlert = "Pricing-Proposal " + strOffer_No + " created successfully";
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here

                        strAlert += @"\n\nWould you like to create one more offer?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPageView = "";
                    }
                }
                else if (intResult == -1)
                {
                    if (intPricingId == 0)
                    {
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    }
                    strRedirectPageView = "";
                }
                else if (intResult == -2)
                {
                    if (intPricingId == 0)
                    {
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    }
                    strRedirectPageView = "";
                }
                else
                {
                    if (intPricingId > 0)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Error in updating Pricing-Proposal");
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Error in creating Pricing-Proposal");
                    }
                    strRedirectPageView = "";
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjPricingMgtServices.Close();
        }
    }

    /// <summary>
    /// This method is used to Cancel the proposal.
    /// </summary>
    private void FunPriCancelProposal()
    {
        ObjPricingMgtServices = new PricingMgtServicesReference.PricingMgtServicesClient();
        try
        {
            int intErrorCode = 0;
            intErrorCode = ObjPricingMgtServices.FunPubWithDrawPricingInt(intPricingId, intCompany_Id, intUserId);
            if (intErrorCode == 0)
            {
                if (ViewState["Status_ID"].ToString() == "120")//cancelled
                    strAlert = strAlert.Replace("__ALERT__", "Pricing-Proposal " + txtProposalNumber.Text + " Revived Successfully");
                else
                    strAlert = strAlert.Replace("__ALERT__", "Pricing-Proposal " + txtProposalNumber.Text + " Cancelled Successfully");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Error in Cancel/Revive Pricing-Proposal " + txtProposalNumber.Text);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to  Cancel/Revive  the Pricing-Proposal";
            CVProposal.IsValid = false;
        }
        finally
        {
            ObjPricingMgtServices.Close();
        }
    }

    /// <summary>
    /// This method is used to load/Bind the grid for the given datatable.
    /// </summary>
    /// <param name="grv"></param>
    /// <param name="dtEntityBankdetails"></param>
    /// 
    private void FunFillgrid(GridView grv, DataTable dtbl)
    {
        try
        {
            grv.DataSource = dtbl;
            grv.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This Method is used to load lead Details
    /// </summary>
    /// <param name="intLeadID"></param>
    private void FunPriLoadLeadDtls(Int64 intLeadID)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompany_Id));
            Procparam.Add("@Lead_ID", Convert.ToString(intLeadID));

            DataSet dsLead = Utility.GetDataset("S3G_ORG_GetProposalLeadDtls", Procparam);
            if (dsLead != null)
            {
                DataTable dtLead = dsLead.Tables[0];

                if (!string.IsNullOrEmpty(dtLead.Rows[0]["Customer_ID"].ToString()))
                {
                    Label lblCustomerCode = (Label)S3GCustomerAddress1.FindControl("lblCustomerCode");
                    Label lblCustomerName1 = (Label)S3GCustomerAddress1.FindControl("lblCustomerName");
                    HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

                    if (Convert.ToInt64(dtLead.Rows[0]["Customer_ID"]) > 0)
                    {
                        hdnCustID.Value = hdnCustomerId.Value = dtLead.Rows[0]["Customer_ID"].ToString();
                        FunPriGetCustomerAddress(Convert.ToInt32(hdnCustomerId.Value));
                        lnkViewCustomer.Enabled = true;
                        lblCustomerCode.Text = lblCustomerName.Text = "Customer Code";
                        lblCustomerName1.Text = "Customer Name";
                        pnlCustomerInformation.GroupingText = "Customer Informations";
                    }
                    else
                    {
                        if (dsLead.Tables[1] != null)
                        {
                            FunPriLoadProspectDetails(dsLead.Tables[1]);
                            lnkViewCustomer.Enabled = false;
                            lblCustomerCode.Text = lblCustomerName.Text = "";
                            lblCustomerName1.Text = "Prospect Name";
                            pnlCustomerInformation.GroupingText = "Prospect Informations";
                        }
                    }
                }

                ddlBranchList.SelectedValue = dtLead.Rows[0]["Location_ID"].ToString();
                ddlBranchList.SelectedText = dtLead.Rows[0]["Location_Desc"].ToString();
                sugLeadRefNo.SelectedValue = dtLead.Rows[0]["Lead_ID"].ToString();
                if (strMode != "M" && strMode != "Q")
                    ddlProposalType.SelectedValue = Convert.ToString(dtLead.Rows[0]["Proposal_Type"]);
                sugLeadRefNo.SelectedText = dtLead.Rows[0]["Lead_No"].ToString();

                ddlBranchList.ReadOnly = sugLeadRefNo.ReadOnly = true;
                ucCustomerCodeLov.Visible = false;
                rfvcmbCustomer.Enabled = rfvCustomer.Enabled = false;
                if (Request.QueryString["qsLeadId"] != null)
                {
                    btnClear.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriLoadProspectDetails(DataTable dtPrspct)
    {
        try
        {
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Text = txtCustomerCode.Text = dtPrspct.Rows[0]["Customer_Code"].ToString();
            S3GCustomerAddress1.SetCustomerDetails(dtPrspct.Rows[0]["Customer_Code"].ToString(),
                    dtPrspct.Rows[0]["comm_Address1"].ToString() + "\n" +
             dtPrspct.Rows[0]["comm_Address2"].ToString() + "\n" +
            dtPrspct.Rows[0]["comm_city"].ToString() + "\n" +
            dtPrspct.Rows[0]["comm_state"].ToString() + "\n" +
            dtPrspct.Rows[0]["comm_country"].ToString() + "\n" +
            dtPrspct.Rows[0]["comm_pincode"].ToString(), dtPrspct.Rows[0]["Customer_Name"].ToString(), dtPrspct.Rows[0]["Comm_Telephone"].ToString(),
            dtPrspct.Rows[0]["Comm_mobile"].ToString(),
            dtPrspct.Rows[0]["comm_email"].ToString(), dtPrspct.Rows[0]["comm_website"].ToString());
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }



    void AssignNewWorkFlowValues(int SelecteDocument, int SelectedProgramId, string SelectedDocumentNo, int BranchID, int LOBId, int ProductId, string LasDocumentNo, DataTable WFSequence)
    {
        WorkFlowSession WFValues = new WorkFlowSession(SelecteDocument, SelectedProgramId, SelectedDocumentNo, BranchID, LOBId, ProductId, LasDocumentNo, 2);
        WFValues.WorkFlowScreens = WFSequence;
    }

    #region "Web Method"
    /// <summary>
    /// This is used to load Location based on the input given by user after 3 character based on user access
    /// </summary>
    /// <param name="prefixText">text entered by user</param>
    /// <param name="count">no of counts</param>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_PageValue.intUserId.ToString());
        Procparam.Add("@Program_Id", "042");
        Procparam.Add("@Lob_Id", obj_PageValue.ddlLob.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));
        return suggestions.ToArray();
    }

    /// <summary>
    /// This is used to load Asset Category based on the input given by user after 3 character based on user access
    /// </summary>
    /// <param name="prefixText">text entered by user</param>
    /// <param name="count">no of counts</param>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static string[] GetAssetCategoryList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        if (obj_PageValue.sugLeadRefNo.SelectedValue != "0")
        {
            Procparam.Add("@Option", "3");
            Procparam.Add("@Lead_Id", obj_PageValue.sugLeadRefNo.SelectedValue);
        }
        else
            Procparam.Add("@Option", "2");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_ProposalLukup", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetTypeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        //UserControls_S3GAutoSuggest ddlAssetCategory = (UserControls_S3GAutoSuggest)obj_PageValue.grvPrimaryGrid.FooterRow.FindControl("ddlAssetCategoryPG");

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@Option", "4");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Asset_Category_ID", obj_PageValue.hdnAssetCatPG.Value);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_ProposalLukup", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetTypeListSG(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        //UserControls_S3GAutoSuggest ddlAssetCategory = (UserControls_S3GAutoSuggest)obj_PageValue.grvSecondaryGrid.FooterRow.FindControl("ddlAssetCategorySG");

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@Option", "4");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Asset_Category_ID", obj_PageValue.hdnAssetCatPG.Value);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_ProposalLukup", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetSubTypeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        //UserControls_S3GAutoSuggest ddlAssetCategory = (UserControls_S3GAutoSuggest)obj_PageValue.grvPrimaryGrid.FooterRow.FindControl("ddlAssetCategoryPG");

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@Option", "5");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Asset_Type_ID", obj_PageValue.hdnAssetTypePG.Value);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_ProposalLukup", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetSubTypeListSG(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        //UserControls_S3GAutoSuggest ddlAssetCategory = (UserControls_S3GAutoSuggest)obj_PageValue.grvSecondaryGrid.FooterRow.FindControl("ddlAssetCategorySG");

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@Option", "5");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Asset_Type_ID", obj_PageValue.hdnAssetTypePG.Value);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_ProposalLukup", Procparam));
        return suggestions.ToArray();
    }

    /// <summary>
    /// This is used to load Asset Category based on the input given by user after 3 character based on user access
    /// </summary>
    /// <param name="prefixText">text entered by user</param>
    /// <param name="count">no of counts</param>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static string[] GetAssetCategoryListSG(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@Option", "2");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_ProposalLukup", Procparam));

        /*
        if (obj_PageValue.ddlProposalType.SelectedValue == "3")//Extension
        {
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
            Procparam.Add("@Option", "2");
            Procparam.Add("@PrefixText", prefixText);
            suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_ProposalLukup", Procparam));
        }
        else
        {
            if (obj_PageValue.ViewState["dtAssetCategorytbl"] != null)
                suggestions = Utility.GetSuggestions((DataTable)obj_PageValue.ViewState["dtAssetCategorytbl"]);

        }
        */
        return suggestions.ToArray();
    }

    /// <summary>
    /// This is used to load Asset Category based on the input given by user after 3 character based on user access
    /// </summary>
    /// <param name="prefixText">text entered by user</param>
    /// <param name="count">no of counts</param>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static string[] GetLeadRefNoList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_PageValue.intCompany_Id.ToString());
        Procparam.Add("@Customer_Id", obj_PageValue.hdnCustID.Value);
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_LOAD_LEADNO", Procparam));
        return suggestions.ToArray();
    }

    #endregion

    #endregion

    protected void sugLeadRefNo_Item_Selected(object Sender, EventArgs e)
    {
        try
        {
            FunPriLoadLeadDtls(Convert.ToInt64(sugLeadRefNo.SelectedValue));
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    //protected void txtTotalFacilityAmount_TextChanged(object sender, EventArgs e)
    //{
    //    txtTotalFacilityAmount.Text = string.Format("{0:C}", Convert.ToDouble(txtTotalFacilityAmount.Text)).Replace("रु ", "").Replace("$", ""); 

    //}
    protected void btnAssetDAN_Click(object sender, EventArgs e)
    {
        //ifrmCRM.ResolveClientUrl("S3GEMICalculator.aspx?IsFromProposal=Yes");
        //ifrmCRM.ResolveUrl("S3GEMICalculator.aspx?IsFromProposal=Yes");
        //ifrmCRM.Attributes["src"] = "S3GEMICalculator.aspx?IsFromProposal=Yes";


        //ifrmCRM.
        //Response.Redirect("S3GEMICalculator.aspx?IsFromProposal=Yes");
    }
    //protected void rdtbnProcessing_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (rdtbnProcessing.Checked)
    //    {
    //        rdbtnOneTime.Checked = false;
    //        txtProcessingAmount.ReadOnly = false;
    //        txtProcessingRate.ReadOnly = false;
    //        txtOneTimeAmount.ReadOnly = true;
    //        txtOnetimeRate.ReadOnly = true;
    //        txtOneTimeAmount.Text = "";
    //        txtOnetimeRate.Text = "";
    //    }
    //}
    //protected void rdbtnOneTime_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (rdbtnOneTime.Checked)
    //    {
    //        rdtbnProcessing.Checked = false;
    //        txtProcessingAmount.ReadOnly = true;
    //        txtProcessingRate.ReadOnly = true;
    //        txtOneTimeAmount.ReadOnly = false;
    //        txtOnetimeRate.ReadOnly = false;
    //        txtProcessingAmount.Text = "";
    //        txtProcessingRate.Text = "";
    //    }
    //}


    protected void txtOnetimeRate_TextChanged(object sender, EventArgs e)
    {
        if (txtOnetimeRate.Text == "")
        {
            txtOneTimeAmount.ReadOnly = false;
            txtOnetimeRate.ReadOnly = false;
            ddlAmtBasedOn.Enabled = false;
            ddlAmtBasedOn.SelectedIndex = 0;

            rfvOneTimeBasedOn.Enabled = false; //By Siva.K on 22JUN2015 Validation
        }
        else
        {
            txtOneTimeAmount.ReadOnly = true;
            txtOnetimeRate.ReadOnly = false;
            ddlAmtBasedOn.Enabled = true;
            rfvOneTimeBasedOn.Enabled = true; //By Siva.K on 22JUN2015 Validation
        }
    }
    protected void txtOneTimeAmount_TextChanged(object sender, EventArgs e)
    {
        if (txtOneTimeAmount.Text == "")
        {
            txtOneTimeAmount.ReadOnly = false;
            txtOnetimeRate.ReadOnly = false;
        }
        else
        {
            txtOneTimeAmount.ReadOnly = false;
            txtOnetimeRate.ReadOnly = true;
        }
        ddlAmtBasedOn.Enabled = false;
    }
    protected void txtProcessingAmount_TextChanged(object sender, EventArgs e)
    {
        if (txtProcessingAmount.Text == "")
        {
            txtProcessingAmount.ReadOnly = false;
            txtProcessingRate.ReadOnly = false;
        }
        else
        {
            txtProcessingAmount.ReadOnly = false;
            txtProcessingRate.ReadOnly = true;

        }
        ddlProcessingBasedOn.Enabled = false;
    }
    protected void txtProcessingRate_TextChanged(object sender, EventArgs e)
    {
        if (txtProcessingRate.Text == "")
        {
            txtProcessingAmount.ReadOnly = false;
            txtProcessingRate.ReadOnly = false;
            ddlProcessingBasedOn.Enabled = false;
            ddlProcessingBasedOn.SelectedIndex = 0;
            rfvProcessingBasedon.Enabled = false; //By Siva.K on 22JUN2015 Validation
        }
        else
        {
            txtProcessingAmount.ReadOnly = true;
            txtProcessingRate.ReadOnly = false;
            ddlProcessingBasedOn.Enabled = true;
            rfvProcessingBasedon.Enabled = true; //By Siva.K on 22JUN2015 Validation
        }
    }

    #region Rebate Changes

    protected void rblIs_Guaranteed_EOT_Appli_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rblIs_Guaranteed_EOT_Appli.SelectedValue == "1")//YES
            {
                rfvGuaranteedEOT.Enabled = true;
                rfvGuaranteedEOTApp.Enabled = true;

                txtGuaranteedEOT.Enabled = true;
                ddlGuaranteedEOTApp.Enabled = true;

            }
            else
            {
                rfvGuaranteedEOT.Enabled = false;
                rfvGuaranteedEOTApp.Enabled = false;

                txtGuaranteedEOT.Enabled = false;
                ddlGuaranteedEOTApp.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }


    protected void ddlRebateStrucAlloc_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grvRebateDetGrid.DataSource = null;
            grvRebateDetGrid.DataBind();
            FunPriEmptyRebateDetGrid();
            TextBox txtRebatePerc = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtRebatePerc");
            TextBox txtFixedRebate = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtFixedRebate");
            if (ddlRebateStrucAlloc.SelectedValue == "1")
            {
                txtRebatePerc.Enabled = false;
                txtFixedRebate.Enabled = true;
            }
            if (ddlRebateStrucAlloc.SelectedValue == "2")
            {
                txtFixedRebate.Enabled = false;
                txtRebatePerc.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix;
            CVProposal.IsValid = false;
        }
    }

    protected void rblRebateDiscountApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriRebateDiscountApp_SelectedIndexChanged();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    private void FunPriRebateDiscountApp_SelectedIndexChanged()
    {
        if (rblRebateDiscountApp.SelectedValue == "1")//YES
        {
            rfvRebateDiscountPerc.Enabled = true;
            txtRebateDiscountPerc.Enabled = true;
            ddlRebateStrucAlloc.Enabled = true;

            RBLRebateStructuredEI.Enabled = true;


            grvRebateDetGrid.Enabled = true;

            rfvRebateStructuredEI.Enabled = true;
        }
        else
        {
            rfvRebateDiscountPerc.Enabled = false;
            txtRebateDiscountPerc.Enabled = false;
            rfvRebateInput.Enabled = false;
            ddlRebateStrucAlloc.Enabled = false;
            ddlRebateStrucAlloc.SelectedValue = "0";

            RBLRebateStructuredEI.Enabled = false;

            grvRebateDetGrid.Enabled = false;

            rfvRebateStructuredEI.Enabled = false;

        }
        if (rblRebateDiscountApp.SelectedValue == "1" && RBLStructuredEI.SelectedValue == "1")
        {
            rfvRebateNoofInstall.Enabled = true;
            txtRebateNoofInstall.Enabled = true;

            ddlRebateStrucAlloc.SelectedValue = "0";
            ddlRebateStrucAlloc.Enabled = false;
            rfvRebateInput.Enabled = false;

            grvRebateDetGrid.Enabled = false;
        }
        else
        {
            rfvRebateNoofInstall.Enabled = false;
            txtRebateNoofInstall.Enabled = false;

            if (rblRebateDiscountApp.SelectedValue == "1")
            {
                ddlRebateStrucAlloc.Enabled = true;
                rfvRebateInput.Enabled = true;
                grvRebateDetGrid.Enabled = true;
            }


        }
    }

    protected void grvRebateDet_RowDataBound(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                FunPriAddgrvPrimaryGridDetails();
                ifrmCRM.ResolveUrl("S3GEMICalculator.aspx?IsFromProposal=Yes");

                //ifrmCRM.Attributes["src"] = "../Common/HomePage.aspx";
            }
        }
        catch (Exception ex)
        {
            CVProposal.ErrorMessage = "Unable To add Due to Data Problem";
            CVProposal.IsValid = false;
        }
    }

    protected void grvRebateDetGrid_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemovegrvRebateDetGridDetails(e.RowIndex);

            TextBox txtRebatePerc = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtRebatePerc");
            TextBox txtFixedRebate = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtFixedRebate");
            if (ddlRebateStrucAlloc.SelectedValue == "1")
            {
                txtRebatePerc.Enabled = false;
            }
            if (ddlRebateStrucAlloc.SelectedValue == "2")
            {
                txtFixedRebate.Enabled = false;
            }

            ifrmCRM.ResolveUrl("S3GEMICalculator.aspx?IsFromProposal=Yes");

        }
        catch (Exception ex)
        {
            CVProposal.ErrorMessage = "Unable To delete Due to Data Problem";
            CVProposal.IsValid = false;
        }
    }

    protected void grvRebateDetGrid_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                FunPriAddgrvRebateDetGridDetails();

                TextBox txtRebatePerc = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtRebatePerc");
                TextBox txtFixedRebate = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtFixedRebate");
                if (ddlRebateStrucAlloc.SelectedValue == "1")
                {
                    txtRebatePerc.Enabled = false;
                }
                if (ddlRebateStrucAlloc.SelectedValue == "2")
                {
                    txtFixedRebate.Enabled = false;
                }

                ifrmCRM.ResolveUrl("S3GEMICalculator.aspx?IsFromProposal=Yes");

                //ifrmCRM.Attributes["src"] = "../Common/HomePage.aspx";
            }
        }
        catch (Exception ex)
        {
            CVProposal.ErrorMessage = "Unable To add Due to Data Problem";
            CVProposal.IsValid = false;
        }
    }

    private void FunPriAddgrvRebateDetGridDetails()
    {
        try
        {
            TextBox txtFromInstallNoRebate = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtFromInstallNoRebate");
            TextBox txtToInstallNoRebate = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtToInstallNoRebate");
            TextBox txtRebatePerc = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtRebatePerc");
            TextBox txtFixedRebate = (TextBox)grvRebateDetGrid.FooterRow.FindControl("txtFixedRebate");

            int intcount = 0;

            DataRow drRow;
            dtRebateDetGrid = (DataTable)ViewState["dtRebateDetGrid"];
            if (dtRebateDetGrid.Rows.Count > 0)
                if (dtRebateDetGrid.Rows[0]["RebateDet_ID"].ToString() == "")
                {
                    dtRebateDetGrid.Rows[0].Delete();
                }
            if (txtRebatePerc.Text == "" && txtFixedRebate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Please Enter the Rebate % or Fixed Rebate !.');", true);
                return;
            }

            //if (Convert.ToInt32(txtFromInstallNoPG.Text) > inttenure)
            // {
            //     ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Installment should not exceed Tenure');", true);
            //     txtFromInstallNoPG.Focus();
            //     return;
            // }
            if (txtToInstallNoRebate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To Installment should not be empty');", true);
                txtToInstallNoRebate.Focus();
                return;
            }

            //By Siva.K on 24MAR2015
            if (Convert.ToInt32(txtFromInstallNoRebate.Text) > Convert.ToInt32(txtToInstallNoRebate.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Installment should not exceed To Installment');", true);
                txtToInstallNoRebate.Focus();
                return;
            }

            int intMaxInstPriSec = 0;
            int intMaxInstRebate = 0;
            if (ViewState["dtPrimaryGrid"] != null)
            {
                dtPrimaryGrid = (DataTable)ViewState["dtPrimaryGrid"];
                foreach (DataRow row in dtPrimaryGrid.Rows)
                {
                    if (row["ToInstallNo"].ToString() != "")
                        intMaxInstPriSec = Convert.ToInt32(row["ToInstallNo"]);

                }
            }
            if (ViewState["dtSecondaryGrid"] != null)
            {
                dtSecondaryGrid = (DataTable)ViewState["dtSecondaryGrid"];
                foreach (DataRow row in dtSecondaryGrid.Rows)
                {
                    if (row["ToInstallNo"].ToString() != "")
                        intMaxInstPriSec = Convert.ToInt32(row["ToInstallNo"]);
                }
            }

            intMaxInstRebate = Convert.ToInt32(txtToInstallNoRebate.Text);

            if (intMaxInstRebate > intMaxInstPriSec)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Maximum Installment No of Primary / Secondary is " + intMaxInstPriSec.ToString() + " . So Rebate To Installement No. cannot be > " + intMaxInstPriSec.ToString() + " ');", true);
                return;
            }

            //OverLap validation

            if (dtRebateDetGrid.Rows.Count > 0)
            {
                DataRow[] drRebateDetGrid = null;
                drRebateDetGrid = dtRebateDetGrid.Select(" (( " + txtFromInstallNoRebate.Text.Trim() + " >= FromInstallNo " +
                    " and " + txtFromInstallNoRebate.Text.Trim() + " <= ToInstallNo ) or " +
                    " ( " + txtToInstallNoRebate.Text.Trim() + " >= FromInstallNo and " +
                    txtToInstallNoRebate.Text.Trim() + " <= ToInstallNo) or " +
                    " ( FromInstallNo >= " + txtFromInstallNoRebate.Text.Trim() +
                    " and FromInstallNo <= " + txtToInstallNoRebate.Text.Trim() + " ))");
                if (drRebateDetGrid.Count() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To Installment should not be overlaped');", true);
                    txtToInstallNoRebate.Focus();
                    return;
                }
            }

            Decimal decTotRebatePer = 0;

            if (ddlRebateStrucAlloc.SelectedValue == "2")
            {
                foreach (DataRow row in dtRebateDetGrid.Rows)
                {
                    decTotRebatePer = decTotRebatePer + Convert.ToDecimal(row["RebatePerc"]);
                }
                decTotRebatePer = decTotRebatePer + Convert.ToDecimal(txtRebatePerc.Text);
                if (decTotRebatePer > 100)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Total Rebate % cannot be > 100');", true);
                    return;
                }
                //if (decTotRebatePer > Convert.ToDecimal(txtRebateDiscountPerc.Text))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Total Rebate % cannot be > " + txtRebateDiscountPerc.Text + "');", true);
                //    return;
                //}

            }

            try
            {
                intcount = Convert.ToInt32(dtRebateDetGrid.Rows.Count);
            }
            catch (Exception ex)
            {
            }

            drRow = dtRebateDetGrid.NewRow();
            //drRow["RebateDet_ID"] = intcount + 1;
            drRow["RebateDet_ID"] = 0;
            if (!string.IsNullOrEmpty(txtRebatePerc.Text))
                //drRow["RentRate"] = txtRentRatePG.Text;
                drRow["RebatePerc"] = Convert.ToDecimal(txtRebatePerc.Text);
            //else

            //By Siva.k on 12MAR2015 for Fixed Rate
            if (!string.IsNullOrEmpty(txtFixedRebate.Text))
                drRow["FixedRebate"] = Convert.ToDecimal(txtFixedRebate.Text);

            drRow["FromInstallNo"] = txtFromInstallNoRebate.Text;
            drRow["ToInstallNo"] = txtToInstallNoRebate.Text;
            //By Siva.k on 01JUL2015 Remove the Residual Per
            //if (!string.IsNullOrEmpty(txtResidualPerPG.Text))
            //    drRow["ResidualPer"] = Convert.ToDecimal(txtResidualPerPG.Text);
            dtRebateDetGrid.Rows.Add(drRow);
            ViewState["dtRebateDetGrid"] = dtRebateDetGrid;
            Session["dtRebateDetGrid"] = (DataTable)ViewState["dtRebateDetGrid"];

            FunFillgrid(grvRebateDetGrid, dtRebateDetGrid);
            //FunPriSetFooterCtrlRebate();

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    /// <summary>
    ////This user defined function is used to remove primary grid details into the grid.
    /// </summary>

    private void FunPriRemovegrvRebateDetGridDetails(int intRowIndex)
    {
        try
        {
            dtRebateDetGrid = (DataTable)ViewState["dtRebateDetGrid"];

            dtRebateDetGrid.Rows.RemoveAt(intRowIndex);
            if (dtRebateDetGrid.Rows.Count == 0)
            {
                FunPriEmptyRebateDetGrid();
            }
            else
            {
                FunFillgrid(grvRebateDetGrid, dtRebateDetGrid);
                ViewState["dtRebateDetGrid"] = dtRebateDetGrid;
                Session["dtRebateDetGrid"] = (DataTable)ViewState["dtRebateDetGrid"];
                //FunPriSetFooterCtrlRebate();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }


    private void FunPriSetMaxLength_Rebate()
    {
        try
        {
            if (grvRebateDetGrid.FooterRow != null)
            {

                TextBox txtRebatePerc = grvRebateDetGrid.FooterRow.FindControl("txtRebatePerc") as TextBox;
                //By Siva.k on 12MAR2015 for Fixed Rate
                TextBox txtFixedRebate = grvRebateDetGrid.FooterRow.FindControl("txtFixedRebate") as TextBox;
                //By Siva.k on 01JUL2015 Remove the Residual Per
                // TextBox txtResidualPerPG = grvRebateDetGrid.FooterRow.FindControl("txtResidualPerPG") as TextBox;
                txtRebatePerc.SetPercentagePrefixSuffix(3, 2, false, false, "Rebate %");
                txtFixedRebate.SetPercentagePrefixSuffix(10, 2, false, false, "Fixed Rebate");

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriEmptyRebateDetGrid()
    {
        try
        {
            dtRebateDetGrid = new DataTable();

            dtRebateDetGrid.Columns.Add("RebateDet_ID");

            dtRebateDetGrid.Columns.Add("RebatePerc", typeof(decimal));
            dtRebateDetGrid.Columns.Add("FixedRebate", typeof(decimal));
            /* Changed data type by Thalai - To get max To installment - Ticket - 2194 - on 22-Sep-2015 - Start */
            dtRebateDetGrid.Columns.Add("FromInstallNo", typeof(Int32));
            dtRebateDetGrid.Columns.Add("ToInstallNo", typeof(Int32));
            /* Changed data type by Thalai - To get max To installment - Ticket - 2194 - on 22-Sep-2015 - End */
            // dtRebateDetGrid.Columns.Add("ResidualPer", typeof(decimal)); //By Siva.k on 02JUL2015 Remove the Residual Per

            DataRow drRebateDetGrid = dtRebateDetGrid.NewRow();
            dtRebateDetGrid.Rows.Add(drRebateDetGrid);

            ViewState["dtRebateDetGrid"] = dtRebateDetGrid;
            Session["dtRebateDetGrid"] = (DataTable)ViewState["dtRebateDetGrid"];
            grvRebateDetGrid.DataSource = dtRebateDetGrid;
            grvRebateDetGrid.DataBind();

            grvRebateDetGrid.Rows[0].Visible = false;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void RBLRebateStructuredEI_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //opc058 start
            if (rblRebateDiscountApp.SelectedValue == "1" && RBLRebateStructuredEI.SelectedValue == "1")
            {
                rfvRebateNoofInstall.Enabled = true;
                txtRebateNoofInstall.Enabled = true;

                ddlRebateStrucAlloc.Enabled = false;
                ddlRebateStrucAlloc.SelectedValue = "0";
                rfvRebateInput.Enabled = false;

                FunPriEmptyRebateDetGrid();

                grvRebateDetGrid.Enabled = false;
            }
            else
            {
                rfvRebateNoofInstall.Enabled = false;
                txtRebateNoofInstall.Enabled = false;

                if (rblRebateDiscountApp.SelectedValue == "1")
                {
                    ddlRebateStrucAlloc.Enabled = true;
                    rfvRebateInput.Enabled = true;
                }

                grvRebateDetGrid.Enabled = true;
            }
            //opc058 end

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }


    #endregion


    protected void ddlReturnPattern_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunDisableGridReturnBased();
    }

    protected void ddlRentalBasedOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRentalBasedOn.SelectedValue == "1")
        {
            pnlRebateDet.Enabled = pnlRebateStruc.Enabled = false;
            rfvRebateDiscountApp.Enabled = rfvRebateNoofInstall.Enabled = rfvRebateStructuredEI.Enabled =
                rfvRebateDiscountPerc.Enabled = rfvAddiRebateDiscountPerc.Enabled = false;
        }
        else
        {
            pnlRebateDet.Enabled = pnlRebateStruc.Enabled = true;
            rfvRebateDiscountApp.Enabled = rfvRebateStructuredEI.Enabled = rfvRebateDiscountPerc.Enabled = true;
        }
    }

    private void FunDisableGridReturnBased()
    {
        TextBox txtRentRatePG = grvPrimaryGrid.FooterRow.FindControl("txtRentRatePG") as TextBox;
        TextBox txtRentRateSG = grvSecondaryGrid.FooterRow.FindControl("txtRentRateSG") as TextBox;

        TextBox txtAMFRentPG = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtAMFRentPG");
        TextBox txtFixedRent = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFixedRent");
        TextBox txtFixedAMF = (TextBox)grvPrimaryGrid.FooterRow.FindControl("txtFixedAMF");

        TextBox txtAMFRentSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtAMFRentSG");
        TextBox txtFixedRentSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFixedRentSG");
        TextBox txtFixedAMFSG = (TextBox)grvSecondaryGrid.FooterRow.FindControl("txtFixedAMFSG");

        UserControls_S3GAutoSuggest ddlAssetSubTypePG = (UserControls_S3GAutoSuggest)grvPrimaryGrid.FooterRow.FindControl("ddlAssetSubTypePG");



        if (ddlReturnPattern.SelectedValue == "7")
        {
            txtAMFRentPG.Enabled = false;
            //txtFixedRent.Enabled = false;
            txtFixedAMF.Enabled = false;

            txtAMFRentSG.Enabled = false;
            //txtFixedRentSG.Enabled = false;
            txtFixedAMFSG.Enabled = false;
            txtRentRatePG.SetPercentagePrefixSuffix(10, 8, false, false, "Rent Rate %");
            txtRentRateSG.SetPercentagePrefixSuffix(10, 8, false, false, "Rent Rate %");
            rfvRentalBasedOn.Enabled = false;
            if (ddlRentalBasedOn.SelectedIndex > 0)
            {
                ddlRentalBasedOn.SelectedIndex = 0;
            }
            ddlRentalBasedOn.Enabled = ddlSecondaryRentalBasedOn.Enabled = false;
            ddlAssetSubTypePG.Enabled = true;
        }
        else
        {
            txtAMFRentPG.Enabled = true;
            txtFixedRent.Enabled = true;
            txtFixedAMF.Enabled = true;

            txtAMFRentSG.Enabled = true;
            txtFixedRentSG.Enabled = true;
            txtFixedAMFSG.Enabled = true;
            txtRentRatePG.SetPercentagePrefixSuffix(10, 8, false, false, "Rent Rate %");
            txtRentRateSG.SetPercentagePrefixSuffix(10, 8, false, false, "Rent Rate %");
            ddlRentalBasedOn.Enabled = ddlSecondaryRentalBasedOn.Enabled = true;
            rfvRentalBasedOn.Enabled = true;
            ddlAssetSubTypePG.Enabled = false;
        }
    }

    /// <summary>
    /// Additional Rebate Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rblAddiRebateDiscountApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriAddiRebateDiscountApp_SelectedIndexChanged();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            CVProposal.ErrorMessage = strErrorMessagePrefix + "Unable to get details";
            CVProposal.IsValid = false;
        }

    }

    private void FunPriAddiRebateDiscountApp_SelectedIndexChanged()
    {
        if (rblAddiRebateDiscountApp.SelectedValue == "1")//YES
        {
            rfvAddiRebateDiscountPerc.Enabled = true;
            txtAddiRebateDiscountPerc.Enabled = true;
            //ddlAddiRebateStrucAlloc.Enabled = true;

            RBLAddiRebateStructuredEI.Enabled = true;


            //grvRebateDetGrid.Enabled = true;

            rfvAddiRebateStructuredEI.Enabled = true;
        }
        else
        {
            rfvAddiRebateDiscountPerc.Enabled = false;
            txtAddiRebateDiscountPerc.Enabled = false;
            //rfvAddiRebateInput.Enabled = false;
            //ddlAddiRebateStrucAlloc.Enabled = false;
            //ddlAddiRebateStrucAlloc.SelectedValue = "0";

            RBLAddiRebateStructuredEI.Enabled = false;

            //grvAddiRebateDetGrid.Enabled = false;

            rfvAddiRebateStructuredEI.Enabled = false;

        }
        if (rblAddiRebateDiscountApp.SelectedValue == "1") //&& RBLAddiStructuredEI.SelectedValue == "1")
        {
            rfvAddiRebateNoofInstall.Enabled = true;
            txtAddiRebateNoofInstall.Enabled = true;

            //ddlAddiRebateStrucAlloc.SelectedValue = "0";
            //ddlAddiRebateStrucAlloc.Enabled = false;
            //rfvAddiRebateInput.Enabled = false;

            //grvAddiRebateDetGrid.Enabled = false;
        }
        else
        {
            rfvAddiRebateNoofInstall.Enabled = false;
            txtAddiRebateNoofInstall.Enabled = false;

            //if (rblAddiRebateDiscountApp.SelectedValue == "1")
            //{
            //    ddlAddiRebateStrucAlloc.Enabled = true;
            //    rfvAddiRebateInput.Enabled = true;
            //    grvAddiRebateDetGrid.Enabled = true;
            //}

        }
    }

    protected void btnBrowse_OnClick(object sender, EventArgs e)
    {
        try
        {
            //if (ddlDocumentType.SelectedValue != "0")
            //{
            // DataTable dtDocs = (DataTable)ViewState["Docs" + ddlDocumentType.SelectedValue];

            HttpFileCollection hfc = Request.Files;

            if (string.IsNullOrEmpty(lblActualPath.Text))
            {
                Utility.FunShowAlertMsg(this, "Document path not available to Upload");
                return;
            }

            if (hfc.Count > 0)
            {
                HttpPostedFile hpf = hfc[0];
                if (hpf.ContentLength > 0)
                {
                    chkSelect.Enabled = true;
                    chkSelect.Checked = true;
                    chkSelect.ToolTip = flUpload.ToolTip = hdnSelectedPath.Value;
                    lblCurrentPath.Text = Path.GetFileName(hpf.FileName);

                    //Cache["Docs_" + ddlDocumentType.SelectedValue + "_File_" + ddlDocument.SelectedValue] = hpf;
                    Cache["Docs_Signed_Proposal"] = hpf;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Select Document type");
            }

            FunProUploadFiles(lblActualPath.Text);
        }
        catch (Exception ex)
        {
            throw ex;
            //cvFollowUp.ErrorMessage = ex.Message;
            //cvFollowUp.IsValid = false;
        }
    }


    protected void FunProUploadFiles(string strPath)
    {
        try
        {
            btnSave.Enabled = false;
            string strCacheFile = "Docs_Signed_Proposal";
            if (Cache[strCacheFile] != null)
            {
                HttpPostedFile hpf = (HttpPostedFile)Cache[strCacheFile];

                //string strFolderName = @"\COMPANY" + intCompanyID.ToString();
                //string strFilePath = strPath + strFolderName;
                string strFilePath = strPath;
                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }
                strFilePath += @"\" + System.IO.Path.GetFileName(hpf.FileName).Split('.')[0].ToString() + "_" + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + System.IO.Path.GetFileName(hpf.FileName).Split('.')[1].ToString();
                hpf.SaveAs(strFilePath);
                ViewState["Document_Path"] = strFilePath;
                ViewState["Download_Path"] = strFilePath;
            }
            btnSave.Enabled = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunProClearCachedFiles()
    {
        try
        {
            string strCacheFile = "Docs_Signed_Proposal";
            if (Cache[strCacheFile] != null)
            {
                Cache.Remove(strCacheFile);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void LnkReqIdView_Click(object sender, EventArgs e)
    {
        try
        {
            string SignedFile = "";
            string FileName = Path.GetFileName(ViewState["Download_Path"].ToString());
            SignedFile = Server.MapPath(".") + "\\PDF Files\\Proposal\\Signed\\";

            if (!System.IO.Directory.Exists(SignedFile))
            {
                System.IO.Directory.CreateDirectory(SignedFile);
            }

            File.Copy(ViewState["Download_Path"].ToString(), SignedFile + FileName);

            string filePath = Server.MapPath(".") + "\\PDF Files\\Proposal\\Signed.zip";

            //before creation of compressed folder,deleting it if exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (!File.Exists(filePath))
            {
                //creating a zip file from one folder to another folder
                ZipFile.CreateFromDirectory(SignedFile, filePath);

                //Delete The excel file which is created

                string[] str = Directory.GetFiles(SignedFile);
                foreach (string fileName in str)
                {
                    File.Delete(fileName);
                }

                if (Directory.Exists(SignedFile))
                {
                    Directory.Delete(SignedFile);
                }
            }
            //SignedFile = filePath;

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(filePath, false, 0);
            string strScipt1 = "";

            strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + filePath.Replace("\\", "/") +
                "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ZIP", strScipt1, true);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }

}
