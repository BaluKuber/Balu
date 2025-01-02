#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Bulk Print
/// Created By			: Sathish G
/// Created Date		: 05-May-2017
/// <Program Summary>
#endregion

#region Name Spaces

using S3GBusEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;


#endregion


public partial class LoanAdmin_S3G_LOANAD_BulkPrint : ApplyThemeForProject
{
    #region Intialization
    Dictionary<string, string> Procparam = null;
    Dictionary<string, string> dictParam = null;
    int intCompanyID = 0;
    string strDateFormat;
    int intUserId;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    static string strPageName = "Bulk Print";
    DataTable dt = new DataTable();
    PagingValues ObjPaging = new PagingValues();
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public static LoanAdmin_S3G_LOANAD_BulkPrint obj_Page;

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

    #endregion

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;
            ProPageSizeRW = intPageSize;
            if (ddldocumentType.SelectedValue == "1")
            {
                FunPriBindPOGrid();
            }
            else if (ddldocumentType.SelectedValue == "2")
            {
                FunPriBindMRAGrid();
            }
            else if (ddldocumentType.SelectedValue == "3")
            {
                FunPriBindPricingGrid();
            }
            else if (ddldocumentType.SelectedValue == "4")
            {
                FunPriBindRSGrid();
            }
            else if (ddldocumentType.SelectedValue == "5")
            {
                FunPriBindCNGrid();
            }
            else if (ddldocumentType.SelectedValue == "6" || ddldocumentType.SelectedValue == "7")
            {
                FunPriBindDCGrid();
            }
            else if (ddldocumentType.SelectedValue == "8")
            {
                FunPriBindOCPGrid();
            }
            else if (ddldocumentType.SelectedValue == "9")
            {
                FunPriBindIIGrid();
            }
            else if (ddldocumentType.SelectedValue == "10")
            {
                FunPriBindRIGrid();
            }
            else if (ddldocumentType.SelectedValue == "11")
            {
                FunPriBindNOAGrid();
            }

            else if (ddldocumentType.SelectedValue == "11")
            {
                FunPriBindNOAGrid();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ProgramCode = "518";
        obj_Page = this;
        //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        lblHeading.Text = strPageName;
        UserInfo ObjUserInfo = new UserInfo();
        intUserId = ObjUserInfo.ProUserIdRW;
        bModify = ObjUserInfo.ProModifyRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bQuery = ObjUserInfo.ProViewRW;
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        S3GSession ObjS3GSession = new S3GSession();
        ProPageNumRW = 1;
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        if (ddldocumentType.SelectedValue == "1")
        {

            TextBox txtPageSize = (TextBox)ucPOCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucPOCustomPaging.callback = obj;
            ucPOCustomPaging.ProPageNumRW = ProPageNumRW;
            ucPOCustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderPOFromDate.Format = CalendarExtenderPOToDate.Format = strDateFormat;
            txtPO_From_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtPO_From_Date.ClientID + "','" + strDateFormat + "',false,  false);");
            txtPO_To_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtPO_To_Date.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "2")
        {

            TextBox txtPageSize = (TextBox)ucMRACustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucMRACustomPaging.callback = obj;
            ucMRACustomPaging.ProPageNumRW = ProPageNumRW;
            ucMRACustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderMRAFromDate.Format = CalendarExtenderMRAToDate.Format = strDateFormat;
            txtMRAFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtMRAFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtMRAToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtMRAToDate.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "3")
        {

            TextBox txtPageSize = (TextBox)ucPricingCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucPricingCustomPaging.callback = obj;
            ucPricingCustomPaging.ProPageNumRW = ProPageNumRW;
            ucPricingCustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderPricingFromDate.Format = CalendarExtenderPricingToDate.Format = strDateFormat;
            txtPricingFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtPricingFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtPricingToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtPricingToDate.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "4")
        {

            TextBox txtPageSize = (TextBox)ucRSCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucRSCustomPaging.callback = obj;
            ucRSCustomPaging.ProPageNumRW = ProPageNumRW;
            ucRSCustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderRSFromDate.Format = CalendarExtenderRSToDate.Format = strDateFormat;
            txtRS_From_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtRS_From_Date.ClientID + "','" + strDateFormat + "',false,  false);");
            txtRS_To_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtRS_To_Date.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "5")
        {

            TextBox txtPageSize = (TextBox)ucCNCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCNCustomPaging.callback = obj;
            ucCNCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCNCustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderCNFromDate.Format = CalendarExtenderCNToDate.Format = strDateFormat;
            txtCNFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCNFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtCNToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCNToDate.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "6" || ddldocumentType.SelectedValue == "7")
        {

            TextBox txtPageSize = (TextBox)ucDCCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucDCCustomPaging.callback = obj;
            ucDCCustomPaging.ProPageNumRW = ProPageNumRW;
            ucDCCustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderDCFromDate.Format = CalendarExtenderDCToDate.Format = strDateFormat;
            txtDCFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDCFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtDCToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDCToDate.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "8")
        {
            TextBox txtPageSize = (TextBox)ucOCPCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucOCPCustomPaging.callback = obj;
            ucOCPCustomPaging.ProPageNumRW = ProPageNumRW;
            ucOCPCustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderOCPFromDate.Format = CalendarExtenderOCPToDate.Format = strDateFormat;
            txtOCPFromdate.Attributes.Add("onblur", "fnDoDate(this,'" + txtOCPFromdate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtOCPTodate.Attributes.Add("onblur", "fnDoDate(this,'" + txtOCPTodate.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "9" || ddldocumentType.SelectedValue == "12")
        {
            TextBox txtPageSize = (TextBox)ucIICustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucIICustomPaging.callback = obj;
            ucIICustomPaging.ProPageNumRW = ProPageNumRW;
            ucIICustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderIIFromDate.Format = CalendarExtenderIIToDate.Format = strDateFormat;
            txtIIFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtIIFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtIIToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtIIToDate.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "10")
        {
            TextBox txtPageSize = (TextBox)ucRICustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucRICustomPaging.callback = obj;
            ucRICustomPaging.ProPageNumRW = ProPageNumRW;
            ucRICustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderRIFromDate.Format = CalendarExtenderRIToDate.Format = strDateFormat;
            txtRIFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtRIFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtRIToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtRIToDate.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        else if (ddldocumentType.SelectedValue == "11")
        {

            TextBox txtPageSize = (TextBox)ucNOACustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucNOACustomPaging.callback = obj;
            ucNOACustomPaging.ProPageNumRW = ProPageNumRW;
            ucNOACustomPaging.ProPageSizeRW = ProPageSizeRW;
            CalendarExtenderNOAFromDate.Format = CalendarExtenderNOAToDate.Format = strDateFormat;
            txtNOAFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtNOAFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtNOAToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtNOAToDate.ClientID + "','" + strDateFormat + "',false,  false);");
        }

        if (!IsPostBack)
        {

            rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
        rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
        rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
        rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
        rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
            FunProLoadControls();
        }
    }

    protected void FunProLoadControls()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LookupType_Code", "142");
            ddldocumentType.BindDataTable(SPNames.S3G_LOANAD_GetLookupTypeDescription, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "5");
            Procparam.Add("@Param1", intCompanyID.ToString());
            Procparam.Add("@Param2", intUserId.ToString());
            Procparam.Add("@Param3", "80");
            Procparam.Add("@Param4", "1");
            //ddlLOB.BindDataTable("S3g_Org_GetCustomerLookup", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void ddldocumentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddldocumentType.SelectedValue == "0") //PO Print
            {
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();

                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
                    rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
                    rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
                    rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
                    rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
            }
            else if (ddldocumentType.SelectedValue == "1") //PO Print
            {
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                //pnlPOSearch.Visible = true;
                pnlPOSearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
                  rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
                   rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
                   rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
                   rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
            }
            else if (ddldocumentType.SelectedValue == "2")//MRA Letter
            {
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                //pnlMRASearch.Visible = true;
                pnlMRASearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
                   rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
                   rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
                   rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
                   rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
                rfvMRAFromDate.Enabled = rfvMRAToDate.Enabled = true;
            }
            else if (ddldocumentType.SelectedValue == "3")//Pricing Letter
            {
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                //pnlPricingSearch.Visible = true;
                pnlPricingSearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
                   rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
                   rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
                   rfvOCPToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
                   rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
                rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = true;
            }
            else if (ddldocumentType.SelectedValue == "4")//Rental Schedule Agreement
            {
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                //pnlRSASearch.Visible = true;
                pnlRSASearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
                   rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
                   rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
                   rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled = false;
                rfvRSFromDate.Enabled = rfvRSToDate.Enabled = true;
            }
            else if (ddldocumentType.SelectedValue == "5")//VAT Credit Note
            {
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                //pnlCNSearch.Visible = true;
                pnlCNSearch.Style["display"] = "block";
                rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
   rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
   rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
   rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
   rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = true;
            }
            else if (ddldocumentType.SelectedValue == "6" || ddldocumentType.SelectedValue == "7")//Credit Note Debit Note
            {
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                //pnlDCSearch.Visible = true;
                pnlDCSearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvIIFromdate.Enabled = rfvIITodate.Enabled =
                    rfvMRAFromDate.Enabled = rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled =
                    rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled = rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled =
                    rfvRIFromDate.Enabled = rfvRIToDate.Enabled = rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
                rfvDCFromDate.Enabled = rfvDCToDate.Enabled = true;
            }
            else if (ddldocumentType.SelectedValue == "8")//Other Cash flows
            {
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                //pnlOCPSearch.Visible = true;
                pnlOCPSearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
   rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
   rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvPricingFromDate.Enabled =
   rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
   rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
                rfvOCPFromDate.Enabled = rfvOCPToDate.Enabled = true;
            }
            else if (ddldocumentType.SelectedValue == "9" || ddldocumentType.SelectedValue == "12")//Interim Invoices
            {
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();

                //pnlIISearch.Visible = true;
                pnlIISearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
   rfvMRAFromDate.Enabled =
   rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
   rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
   rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
                rfvIIFromdate.Enabled = rfvIITodate.Enabled = true;
            }
            else if (ddldocumentType.SelectedValue == "10")//Rental Invoices
            {
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                pnlNOASearch.Style["display"] = "none";
                pnlNOA.Visible = false;
                gvNOA.DataSource = null;
                gvNOA.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                //pnlRISearch.Visible = true;
                pnlRISearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
 rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
   rfvMRAToDate.Enabled = rfvNOAFromDate.Enabled = rfvNOAReportFormat.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
   rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
                rfvRIFromDate.Enabled = rfvRIToDate.Enabled = true;
            }
            else if (ddldocumentType.SelectedValue == "11")//Notification of Assignment
            {
                pnlPOSearch.Style["display"] = "none";
                pnlPO.Visible = false;
                pnlMRASearch.Style["display"] = "none";
                pnlMRA.Visible = false;
                pnlPricingSearch.Style["display"] = "none";
                pnlPricing.Visible = false;
                pnlRSASearch.Style["display"] = "none";
                pnlRS.Visible = false;
                gvPO.DataSource = null;
                gvPO.DataBind();
                gvMRA.DataSource = null;
                gvMRA.DataBind();
                gvPricing.DataSource = null;
                gvPricing.DataBind();
                gvRS.DataSource = null;
                gvRS.DataBind();
                pnlDCSearch.Style["display"] = "none";
                pnlDC.Visible = false;
                gvDC.DataSource = null;
                gvDC.DataBind();
                pnlCNSearch.Style["display"] = "none";
                pnlCN.Visible = false;
                gvCN.DataSource = null;
                gvCN.DataBind();
                pnlOCPSearch.Style["display"] = "none";
                pnlOCP.Visible = false;
                gvOCP.DataSource = null;
                gvOCP.DataBind();
                pnlRISearch.Style["display"] = "none";
                pnlRI.Visible = false;
                gvRI.DataSource = null;
                gvRI.DataBind();
                pnlIISearch.Style["display"] = "none";
                pnlII.Visible = false;
                gvII.DataSource = null;
                gvII.DataBind();

                Dictionary<string, string> dictparam = new Dictionary<string, string>();
                dictparam.Add("@company_ID", intCompanyID.ToString());
                DataTable dtreportformat = Utility.GetDefaultData("S3G_FM_GetNoteLookUpDtls", dictparam);
                ddlReportFormatType.FillDataTable(dtreportformat, "Value", "Name");
                //pnlNOASearch.Visible = true;
                pnlNOASearch.Style["display"] = "block";
                rfvCNFromDate.Enabled = rfvCNToDate.Enabled = rfvDCFromDate.Enabled = rfvDCToDate.Enabled =
   rfvIIFromdate.Enabled = rfvIITodate.Enabled = rfvMRAFromDate.Enabled =
   rfvMRAToDate.Enabled = rfvNOAToDate.Enabled = rfvOCPFromDate.Enabled =
   rfvOCPToDate.Enabled = rfvPricingFromDate.Enabled = rfvPricingToDate.Enabled = rfvRIFromDate.Enabled = rfvRIToDate.Enabled =
   rfvRSFromDate.Enabled = rfvRSToDate.Enabled = false;
                rfvNOAFromDate.Enabled = rfvNOAToDate.Enabled = rfvNOAReportFormat.Enabled = true;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddldocumentType.SelectedValue == "1")
            FunPriBindPOGrid();
        else if (ddldocumentType.SelectedValue == "2")
            FunPriBindMRAGrid();
        else if (ddldocumentType.SelectedValue == "3")
            FunPriBindPricingGrid();
        else if (ddldocumentType.SelectedValue == "4")
            FunPriBindRSGrid();
        else if (ddldocumentType.SelectedValue == "5")
            FunPriBindCNGrid();
        else if (ddldocumentType.SelectedValue == "6" || ddldocumentType.SelectedValue == "7")
            FunPriBindDCGrid();
        else if (ddldocumentType.SelectedValue == "8")
            FunPriBindOCPGrid();
        else if (ddldocumentType.SelectedValue == "9")
            FunPriBindIIGrid();
        else if (ddldocumentType.SelectedValue == "10")
            FunPriBindRIGrid();
        else if (ddldocumentType.SelectedValue == "11")
            FunPriBindNOAGrid();
        else if (ddldocumentType.SelectedValue == "12")
            FunPriBindStockGrid();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (ddldocumentType.SelectedValue == "1")
            FunPriGeneratePOFiles(intCompanyID, 0, 0, 10);
        else if (ddldocumentType.SelectedValue == "2")
            FunPriGenerateMRAFiles(intCompanyID, 0, 0, 14);
        else if (ddldocumentType.SelectedValue == "3")
            FunPriGeneratePricingFiles(intCompanyID, 0, 0, 1);
        else if (ddldocumentType.SelectedValue == "4")
            FunPriGenerateRSFiles(intCompanyID, 0, 0, 16);
        else if (ddldocumentType.SelectedValue == "5")
            FunPriGenerateCNFiles(intCompanyID, 0, 0, 0);
        else if (ddldocumentType.SelectedValue == "6")
            FunPriGenerateDCFiles(intCompanyID, 3, 0, 20);
        else if (ddldocumentType.SelectedValue == "7")
            FunPriGenerateDCFiles(intCompanyID, 3, 0, 21);
        else if (ddldocumentType.SelectedValue == "8")
            FunPriGenerateOCPFiles(intCompanyID, 0, 0, 0);
        else if (ddldocumentType.SelectedValue == "9")
            FunPriGenerateIIFiles(intCompanyID, 0, 0, 13);
        else if (ddldocumentType.SelectedValue == "10")
            FunPriGenerateRIFiles(intCompanyID, 0, 0, 0);
        else if (ddldocumentType.SelectedValue == "11")
        {
            int TemplateType = 0;
            if (ddlReportFormatType.SelectedValue == "1")
                TemplateType = 22;
            else if (ddlReportFormatType.SelectedValue == "2")
                TemplateType = 23;
            else if (ddlReportFormatType.SelectedValue == "3")
                TemplateType = 24;
            else if (ddlReportFormatType.SelectedValue == "4")
                TemplateType = 25;
            else if (ddlReportFormatType.SelectedValue == "5")
                TemplateType = 27;
            else if (ddlReportFormatType.SelectedValue == "6")
                TemplateType = 28;
            else if (ddlReportFormatType.SelectedValue == "7")
                TemplateType = 29;
            else if (ddlReportFormatType.SelectedValue == "8")
                TemplateType = 30;
            else if (ddlReportFormatType.SelectedValue == "9")
                TemplateType = 31;
            FunPriGenerateNOAFiles(intCompanyID, 0, 0, TemplateType);
        }
        else if (ddldocumentType.SelectedValue == "12")
            FunPriGenerateStockFiles(intCompanyID, 0, 0, 13);


    }

    private void FunPriGenerateFiles(List<string> filepaths, string OutputFile, string DocumentType)
    {
        try
        {
            object fileFormat = null;
            object file = null;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string[] filesToMerge = filepaths.ToArray();

            if (DocumentType == "P")
            {
               // PDFPageSetup.MergePDFWithoutDelete(filesToMerge, OutputFile);

                //for (int i = 0; i < filesToMerge.Length; i++)
                //{
                //    if (File.Exists(filesToMerge[i]) == true)
                //    {
                //        File.Delete(filesToMerge[i]);
                //    }
                //}
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = OutputFile;
                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;
                int temp = 0;
                foreach (string file1 in filesToMerge)
                {
                    temp++;
                    Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(file1);
                    //PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
                    CurrentDocument.Range().Copy();
                    selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
                    if (temp != filesToMerge.Length)
                        selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);
                }
                wordDocument.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
                wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

                //for (int i = 0; i < filesToMerge.Length; i++)
                //{
                //    if (File.Exists(filesToMerge[i]) == true)
                //    {
                //        File.Delete(filesToMerge[i]);
                //    }
                //}
            }

        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print");
        }
    }

    private void FunPriDownloadFile(string OutputFile)
    {
        if (ddlPrintType.SelectedValue == "P")
        {
            Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
            Response.ContentType = "application/pdf";
            Response.WriteFile(OutputFile);
        }
        else if (ddlPrintType.SelectedValue == "W")
        {
            Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.doc");
            Response.ContentType = "application/vnd.ms-word";
            Response.WriteFile(OutputFile);
        }
    }

    public void CombineMultiplePDFs(string[] fileNames, string outFile)
    {
        try
        {

            Document document = new Document();
            PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
            if (writer == null)
            {
                return;
            }
            document.Open();

            foreach (string fileName in fileNames)
            {
                PdfReader reader = new PdfReader(fileName);
                reader.ConsolidateNamedDestinations();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }

                PRAcroForm form = reader.AcroForm;
                if (form != null)
                {
                    writer.CopyAcroForm(reader);
                }

                reader.Close();
            }
            writer.Close();
            document.Close();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #region Purchase Order

    protected void gvPO_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvPO.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindPOGrid()
    {
        if (txtPO_From_Date.Text == "" && txtPO_To_Date.Text == "" && (ddlPOCustomerName.SelectedValue == "0" && ddlPOCustomerName.SelectedText == String.Empty) && (ddlPOVendorName.SelectedValue == "0" && ddlPOVendorName.SelectedText == String.Empty) && (ddlPOLoadSequenceNo.SelectedValue == "0" && ddlPOLoadSequenceNo.SelectedText == String.Empty))
        {
            Utility.FunShowAlertMsg(this.Page, "Select any one of input detail");
            return;
        }

        if ((txtPO_From_Date.Text != "") && (txtPO_To_Date.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtPO_From_Date.Text)) > Convert.ToDateTime(Utility.StringToDate(txtPO_To_Date.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtPO_From_Date.Text))
            Procparam.Add("@PO_From_Date", Utility.StringToDate(txtPO_From_Date.Text).ToString());

        if (!String.IsNullOrEmpty(txtPO_To_Date.Text))
            Procparam.Add("@PO_To_Date", Utility.StringToDate(txtPO_To_Date.Text).ToString());

        if (ddlPOCustomerName.SelectedValue != "0" && ddlPOCustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_Id", ddlPOCustomerName.SelectedValue);
        else if (ddlPOCustomerName.SelectedValue == "0" && ddlPOCustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_Id", ddlPOCustomerName.SelectedValue);
        else if (ddlPOCustomerName.SelectedValue != "0" && ddlPOCustomerName.SelectedText == String.Empty)
            Procparam.Add("@Customer_Id", null);

        if (ddlPOVendorName.SelectedValue != "0" && ddlPOVendorName.SelectedText != String.Empty)
            Procparam.Add("@Entity_ID", ddlPOVendorName.SelectedValue);
        else if (ddlPOVendorName.SelectedValue == "0" && ddlPOVendorName.SelectedText != String.Empty)
            Procparam.Add("@Entity_ID", ddlPOCustomerName.SelectedValue);
        else if (ddlPOVendorName.SelectedValue != "0" && ddlPOVendorName.SelectedText == String.Empty)
            Procparam.Add("@Entity_ID", null);

        if (ddlPOLoadSequenceNo.SelectedValue != "0" && ddlPOLoadSequenceNo.SelectedText != String.Empty)
            Procparam.Add("@LoadSequenceNo", ddlPOLoadSequenceNo.SelectedValue);
        else if (ddlPOLoadSequenceNo.SelectedValue == "0" && ddlPOLoadSequenceNo.SelectedText != String.Empty)
            Procparam.Add("@LoadSequenceNo", ddlPOLoadSequenceNo.SelectedValue);
        else if (ddlPOLoadSequenceNo.SelectedValue != "0" && ddlPOLoadSequenceNo.SelectedText == String.Empty)
            Procparam.Add("@LoadSequenceNo", null);

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvPO.BindGridView("S3G_Org_Get_PO", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucPOCustomPaging.Visible = true;
        ucPOCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucPOCustomPaging.setPageSize(ProPageSizeRW);
        pnlPO.Visible = true;

        if (bIsNewRow == true)
        {
            gvPO.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

        if (!bModify)
            gvPO.Columns[8].Visible = false;
    }

    private void FunPriGeneratePOFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strPONumber = "";
            string PO_Header_ID = "";
            string LPODate = "";
            string GSTEffectiveDate = "";
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(CompanyID));
           // dictParam.Add("@PO_Number", Convert.ToString(strPONumber));
            GSTEffectiveDate = Utility.GetDefaultData("S3G_SYSAD_GSTEFFECTIVEDATE", dictParam).Rows[0]["GST_Effective_From"].ToString();

            foreach (GridViewRow grvRow in gvPO.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    strPONumber = ((Label)grvRow.FindControl("lblPO_Number")).Text;
                    PO_Header_ID = ((Label)grvRow.FindControl("lblPO_dtl_ID")).Text;
                    LPODate = ((Label)grvRow.FindControl("lblPO_Date")).Text;

                    String strHTML = String.Empty;
                    // if (Utility.StringToDate(LPODate) >= Utility.StringToDate(GSTEffectiveDate))
                    Template_Type_Code = 32;
                    //else
                    //Template_Type_Code = 10;

                    strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, Template_Type_Code, PO_Header_ID);
                    if (strHTML == "")
                    {
                        Utility.FunShowAlertMsg(this, "Template Master not defined");
                        return;
                    }

                    string FileName = PDFPageSetup.FunPubGetFileName(strPONumber + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                    string DownFile = string.Empty;
                    if (ddlPrintType.SelectedValue == "P")
                    {
                        DownFile = FilePath + FileName + ".pdf";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                    }
                    else if (ddlPrintType.SelectedValue == "W")
                    {
                        DownFile = FilePath + FileName + ".doc";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc";
                    }

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", CompanyId.ToString());
                    Procparam.Add("@PO_Number", strPONumber);
                    //dictParam = new Dictionary<string, string>();
                    //dictParam.Add("@Company_ID", intCompanyId.ToString());
                    //dictParam.Add("@PO_Number", strPONumber);
                    DataSet dsHeader = new DataSet();
                    dsHeader = Utility.GetDataset("S3G_ORG_PurchaseOrder_Print", Procparam);

                    //if (dsHeader.Tables[1].Rows.Count == 0)
                    //    if (strHTML.Contains("~PO_Header_Table~"))
                    //        strHTML = Utility.FunPubDeleteTable("~PO_Header_Table~", strHTML);

                    //if (dsHeader.Tables[2].Rows.Count == 0)
                    //    if (strHTML.Contains("~PO_Annex1_Table~"))
                    //        strHTML = Utility.FunPubDeleteTable("~PO_Annex1_Table~", strHTML);

                    //if (dsHeader.Tables[3].Rows.Count == 0)
                    //    if (strHTML.Contains("~PO_Annex2_Table~"))
                    //        strHTML = Utility.FunPubDeleteTable("~PO_Annex2_Table~", strHTML);

                    if (dsHeader.Tables[1].Rows.Count > 0)
                        if (strHTML.Contains("~PO_Header_Table~"))
                            strHTML = PDFPageSetup.FunPubBindTable("~PO_Header_Table~", strHTML, dsHeader.Tables[1]);

                    if (dsHeader.Tables[2].Rows.Count > 0)
                        if (strHTML.Contains("~PO_Annex1_Table~"))
                            strHTML = PDFPageSetup.FunPubBindTable("~PO_Annex1_Table~", strHTML, dsHeader.Tables[2]);

                    if (dsHeader.Tables[3].Rows.Count > 0)
                        if (strHTML.Contains("~PO_Annex2_Table~"))
                            strHTML = PDFPageSetup.FunPubBindTable("~PO_Annex2_Table~", strHTML, dsHeader.Tables[3]);

                    if (dsHeader.Tables[0].Rows.Count > 0)
                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    List<string> listImagePath = new List<string>();
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                    strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                    PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);
                    filepaths.Add(DownFile);
                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);
                FunPriDownloadFile(OutputFile);
            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    //private void FunPriGeneratePOFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    //{
    //    try
    //    {
    //        object oMissing = System.Reflection.Missing.Value;
    //        object readOnly = false;
    //        object oFalse = false;
    //        string strPONumber = "";
    //        string PO_Header_ID = "";
    //        string LPODate = "";
    //        string GSTEffectiveDate = "";

    //        string strhtmlFile = string.Empty;
    //        var filepaths = new List<string>();
    //        var outputfilepaths = new List<string>();

    //        string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
    //        string FilePath = Server.MapPath(".") + "\\PDF Files\\";


    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", Convert.ToString(CompanyID));
    //        GSTEffectiveDate = Utility.GetDefaultData("S3G_SYSAD_GSTEFFECTIVEDATE", Procparam).Rows[0]["GST_Effective_From"].ToString();

    //        foreach (GridViewRow grvRow in gvPO.Rows)
    //        {
    //            if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
    //            {
    //                strPONumber = ((Label)grvRow.FindControl("lblPO_Number")).Text;
    //                PO_Header_ID = ((Label)grvRow.FindControl("lblPO_dtl_ID")).Text;
    //                LPODate = ((Label)grvRow.FindControl("lblPO_Date")).Text;

    //                String strHTML = String.Empty;
    //                if (Utility.StringToDate(LPODate) >= Utility.StringToDate(GSTEffectiveDate))
    //                    Template_Type_Code = 32;
    //                else
    //                    Template_Type_Code = 10;

    //                strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, Template_Type_Code, PO_Header_ID);
    //                if (strHTML == "")
    //                {
    //                    Utility.FunShowAlertMsg(this, "Template Master not defined");
    //                    return;
    //                }

    //                string FileName = PDFPageSetup.FunPubGetFileName(strPONumber + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
    //                string DownFile = string.Empty;
    //                if (ddlPrintType.SelectedValue == "P")
    //                {
    //                    DownFile = FilePath + FileName + ".pdf";
    //                    OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
    //                }
    //                else if (ddlPrintType.SelectedValue == "W")
    //                {
    //                    DownFile = FilePath + FileName + ".doc";
    //                    OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc";
    //                }

    //                Procparam = new Dictionary<string, string>();
    //                Procparam.Add("@Company_Id", CompanyId.ToString());
    //                Procparam.Add("@PO_Number", strPONumber);
    //                DataSet dsHeader = new DataSet();
    //                dsHeader = Utility.GetDataset("S3G_ORG_PurchaseOrder_Print_New", Procparam);
    //                //dsHeader.Tables[0].Columns.Add("Grand_Total");

    //                //if (dsHeader.Tables[0].Rows.Count != 0)
    //                //    dsHeader.Tables[0].Rows[0]["Grand_Total"] = Convert.ToDecimal((dsHeader.Tables[1].Compute("sum(Total1)", ""))).ToString(Funsetsuffix());

    //                List<string> listImageName = new List<string>();
    //                listImageName.Add("~CompanyLogo~");
    //                listImageName.Add("~InvoiceSignStamp~");
    //                listImageName.Add("~POSignStamp~");
    //                List<string> listImagePath = new List<string>();
    //                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
    //                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
    //                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

    //                var startTag = "";
    //                var endTag = "";
    //                int startIndex = 0;
    //                int endIndex = 0;
    //                string strTable;
    //                string strRow;
    //                filepaths = new List<string>();

    //                startTag = "~PO_Annex1_Table~";
    //                endTag = "</TABLE>";
    //                startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
    //                endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
    //                strTable = strHTML.Substring(startIndex, endIndex - startIndex);

    //                strHTML = strHTML.Replace(strTable, "");

    //                strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);
    //                strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[3]);
    //                strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
    //                FunPubSaveDocument(strHTML, FilePath, FileName + "1", "W", "P");
    //                strhtmlFile = FilePath + "\\" + FileName + "1" + ".doc";
    //                filepaths.Add(strhtmlFile);

    //                startTag = "<TR";
    //                endTag = "</TR>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                strRow = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(strRow, "");

    //                string r1, r2, r3, r4, r5, r6, r7 = string.Empty;
    //                string strReplaceWith, strReplaceBy = string.Empty;

    //                startTag = "<TR";
    //                endTag = "</TR>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                r1 = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(r1, "");

    //                startTag = "<TR";
    //                endTag = "</TR>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                r2 = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(r2, "");

    //                startTag = "<TR";
    //                endTag = "</TR>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                r3 = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(r3, "");

    //                startTag = "<TR";
    //                endTag = "</TR>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                r4 = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(r4, "");

    //                startTag = "<TR";
    //                endTag = "</TR>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                r5 = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(r5, "");

    //                startTag = "<TR";
    //                endTag = "</TR>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                r6 = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(r6, "");

    //                startTag = "<TR";
    //                endTag = "</TR>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                r7 = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(r7, "");
    //                strReplaceWith = r1 + r2 + r3 + r4 + r5 + r6 + r7;

    //                startTag = "<TBODY>";
    //                endTag = "</TBODY>";
    //                startIndex = strTable.IndexOf(startTag);
    //                endIndex = strTable.IndexOf(endTag, startIndex) + endTag.Length;
    //                string tbody = strTable.Substring(startIndex, endIndex - startIndex);
    //                strTable = strTable.Replace(tbody, strReplaceWith);

    //                for (int i = 0; i < dsHeader.Tables[1].Rows.Count; i++)
    //                {
    //                    string replacer1, replacer2, replacer3, replacer4, replacer5, replacer6 = string.Empty;
    //                    replacer1 = r1;
    //                    replacer2 = r2;
    //                    replacer3 = r3;
    //                    replacer4 = r4;
    //                    replacer5 = r5;
    //                    replacer6 = r6;

    //                    try
    //                    {
    //                        DataRow dr = dsHeader.Tables[1].NewRow();
    //                        dr = dsHeader.Tables[1].Rows[i];

    //                        foreach (DataColumn dcol in dsHeader.Tables[1].Columns)
    //                        {
    //                            string ColName1 = string.Empty;
    //                            ColName1 = "~" + dcol.ColumnName + "~";
    //                            if (replacer1.Contains(ColName1))
    //                                replacer1 = replacer1.Replace(ColName1, dr[dcol].ToString());
    //                        }

    //                        foreach (DataColumn dcol in dsHeader.Tables[1].Columns)
    //                        {
    //                            string ColName1 = string.Empty;
    //                            ColName1 = "~" + dcol.ColumnName + "~";
    //                            if (replacer2.Contains(ColName1))
    //                                replacer2 = replacer2.Replace(ColName1, dr[dcol].ToString());
    //                        }

    //                        DataRow dr4 = dsHeader.Tables[0].NewRow();
    //                        dr4 = dsHeader.Tables[0].Rows[0];

    //                        foreach (DataColumn dcol in dsHeader.Tables[0].Columns)
    //                        {
    //                            string ColName1 = string.Empty;
    //                            ColName1 = "~" + dcol.ColumnName + "~";
    //                            if (replacer4.Contains(ColName1))
    //                                replacer4 = replacer4.Replace(ColName1, dr4[dcol].ToString());
    //                        }

    //                        DataTable dtSelectDetails = dsHeader.Tables[2].Select("Billing_Dtl_ID=" + dsHeader.Tables[1].Rows[i]["Billing_Dtl_ID"].ToString()).CopyToDataTable();

    //                        string newtr = String.Empty;

    //                        for (int j = 0; j < dtSelectDetails.Rows.Count; j++)
    //                        {
    //                            replacer5 = r5;
    //                            DataRow drdetails = dtSelectDetails.NewRow();
    //                            foreach (DataColumn dcol in dtSelectDetails.Columns)
    //                            {
    //                                drdetails = dtSelectDetails.Rows[j];
    //                                string ColName1 = string.Empty;
    //                                ColName1 = "~" + dcol.ColumnName + "~";
    //                                if (replacer5.Contains(ColName1))
    //                                    replacer5 = replacer5.Replace(ColName1, drdetails[dcol].ToString());
    //                            }
    //                            replacer6 = r6;
    //                            foreach (DataColumn dcol in dtSelectDetails.Columns)
    //                            {
    //                                string ColName1 = string.Empty;
    //                                ColName1 = "~" + dcol.ColumnName + "~";
    //                                if (replacer6.Contains(ColName1))
    //                                    replacer6 = replacer6.Replace(ColName1, drdetails[dcol].ToString());
    //                            }

    //                            newtr = newtr + " " + replacer5 + replacer6;
    //                        }
    //                        replacer5 = newtr;


    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //                        throw ex;
    //                    }
    //                    strReplaceBy = strReplaceBy + " " + replacer1 + replacer2 + replacer3 + replacer4 + replacer5;
    //                }

    //                DataRow dr7 = dsHeader.Tables[3].NewRow();
    //                dr7 = dsHeader.Tables[3].Rows[0];
    //                foreach (DataColumn dcol in dsHeader.Tables[3].Columns)
    //                {
    //                    string ColName1 = string.Empty;
    //                    ColName1 = "~" + dcol.ColumnName + "~";
    //                    if (r7.Contains(ColName1))
    //                        r7 = r7.Replace(ColName1, dr7[dcol].ToString());
    //                }

    //                strReplaceBy = strReplaceBy + " " + r7;
    //                //strTable = strTable.Replace(strReplaceWith, "");

    //                strTable = strTable.Replace(strReplaceWith, strReplaceBy);

    //                //if (strtempTable.Contains("~PO_Header_Table~"))
    //                //    strtempTable = PDFPageSetup.FunPubBindTable("~PO_Header_Table~", strtempTable, dsHeader.Tables[1]);
    //                //if (strtempTable.Contains("~PO_Annex1_Table~"))
    //                //    strtempTable = PDFPageSetup.FunPubBindTable("~PO_Annex1_Table~", strtempTable, dsHeader.Tables[2]);
    //                //if (strtempTable.Contains("~PO_Annex2_Table~"))
    //                //    strtempTable = PDFPageSetup.FunPubBindTable("~PO_Annex2_Table~", strtempTable, dsHeader.Tables[3]);

    //                strTable = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strTable);
    //                FunPubSaveDocument(strTable, FilePath, FileName + "2", "W", "L");
    //                strhtmlFile = FilePath + "\\" + FileName + "2" + ".doc";
    //                filepaths.Add(strhtmlFile);

    //                object fileFormat = null;
    //                object file = null;
    //                oMissing = System.Reflection.Missing.Value;
    //                readOnly = false;
    //                oFalse = false;
    //                string[] filesToMerge = filepaths.ToArray();

    //                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
    //                string strwordFile = FilePath + "\\" + FileName + ".doc";
    //                string strpdfFile = FilePath + "\\" + FileName + ".pdf";

    //                if (ddlPrintType.SelectedValue == "P")
    //                {
    //                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
    //                    file = strpdfFile;
    //                    outputfilepaths.Add(strpdfFile);
    //                }
    //                else if (ddlPrintType.SelectedValue == "W")
    //                {
    //                    fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
    //                    file = strwordFile;
    //                    outputfilepaths.Add(strwordFile);
    //                }
    //                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
    //                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
    //                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;
    //                int temp = 0;

    //                foreach (string file1 in filesToMerge)
    //                {
    //                    temp++;
    //                    Microsoft.Office.Interop.Word._Document CurrentDocument = wordApplication.Documents.Add(file1);
    //                    //PDFPageSetup.CopyPageSetupForWord(CurrentDocument.PageSetup, wordDocument.Sections.Last.PageSetup);
    //                    CurrentDocument.Range().Copy();
    //                    selection.PasteAndFormat(Microsoft.Office.Interop.Word.WdRecoveryType.wdFormatOriginalFormatting);
    //                    if (temp != filesToMerge.Length)
    //                        selection.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdSectionBreakNextPage);
    //                }
    //                wordDocument.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
    //                wordDocument.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
    //                    , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
    //                wordDocument.Close(ref oFalse, ref oMissing, ref oMissing);
    //                wordApplication.Quit(ref oMissing, ref oMissing, ref oMissing);

    //                foreach (string file1 in filesToMerge)
    //                {
    //                    File.Delete(file1);
    //                }
    //            }
    //        }
    //        if (outputfilepaths.Count == 0)
    //        {
    //            Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
    //            return;
    //        }
    //        else
    //        {
    //            FunPriGenerateFiles(outputfilepaths, OutputFile, ddlPrintType.SelectedValue);
    //            FunPriDownloadFile(OutputFile);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
    //    }
    //}

    private void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, string OriType)
    {
        string strhtmlFile = FilePath + "\\" + FileName + ".html";
        string strwordFile = FilePath + "\\" + FileName + ".doc";
        string strpdfFile = FilePath + "\\" + FileName + ".pdf";
        object file = strhtmlFile;
        object oMissing = System.Reflection.Missing.Value;
        object readOnly = false;
        object oFalse = false;
        object fileFormat = null;
        Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
        Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            if (File.Exists(strwordFile) == true)
            {
                File.Delete(strwordFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);

            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            oDoc.ActiveWindow.Selection.PageSetup.TopMargin = oWord.InchesToPoints(0.5f);
            oDoc.ActiveWindow.Selection.PageSetup.BottomMargin = oWord.InchesToPoints(0.5f);
            oDoc.ActiveWindow.Selection.PageSetup.RightMargin = oWord.InchesToPoints(0.5f);
            oDoc.ActiveWindow.Selection.PageSetup.LeftMargin = oWord.InchesToPoints(0.5f);

            if (OriType == "L")
            {
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            }
            else
            {
                oDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
            }

            if (DocumentType == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
            }
            else
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
            }
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        obj_Page.ddlPOVendorName.SelectedValue = String.Empty;
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));
        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerNameDetails(String prefixText)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        if (obj_Page.ddldocumentType.SelectedValue == "1")
            obj_Page.ddlPOCustomerName.SelectedValue = String.Empty;
        else if (obj_Page.ddldocumentType.SelectedValue == "3")
            obj_Page.ddlPricingCustomerName.SelectedValue = String.Empty;
        else if (obj_Page.ddldocumentType.SelectedValue == "4")
            obj_Page.ddlRSCustomerName.SelectedValue = String.Empty;

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLSQNo(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataSet Ds = new DataSet();
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        obj_Page.ddlPOLoadSequenceNo.SelectedValue = String.Empty;
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@SearchText", prefixText);
        Procparam.Add("@User_ID", Convert.ToString(obj_Page.intUserId));
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Org_GetLPONo_AGT", Procparam));
        return suggetions.ToArray();
    }

    #endregion

    #region Rental Schedule Agreement

    protected void gvRS_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvRS.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindRSGrid()
    {

        if ((txtRS_From_Date.Text != "") && (txtRS_To_Date.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtRS_From_Date.Text)) > Convert.ToDateTime(Utility.StringToDate(txtRS_To_Date.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtRS_From_Date.Text))
            Procparam.Add("@RS_From_Date", Utility.StringToDate(txtRS_From_Date.Text).ToString());

        if (!String.IsNullOrEmpty(txtRS_To_Date.Text))
            Procparam.Add("@RS_To_Date", Utility.StringToDate(txtRS_To_Date.Text).ToString());

        if (ddlRSAccountNumber.SelectedValue != "0" && ddlRSAccountNumber.SelectedText != String.Empty)
            Procparam.Add("@AccountNo", ddlRSAccountNumber.SelectedValue);
        else if (ddlRSAccountNumber.SelectedValue == "0" && ddlRSAccountNumber.SelectedText != String.Empty)
            Procparam.Add("@AccountNo", ddlRSAccountNumber.SelectedValue);
        else if (ddlRSAccountNumber.SelectedValue != "0" && ddlRSAccountNumber.SelectedText == String.Empty)
            Procparam.Add("@AccountNo", null);

        if (ddlRSCustomerName.SelectedValue != "0" && ddlRSCustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_ID", ddlRSCustomerName.SelectedValue);
        else if (ddlRSCustomerName.SelectedValue == "0" && ddlRSCustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_ID", ddlRSCustomerName.SelectedValue);
        else if (ddlRSCustomerName.SelectedValue != "0" && ddlRSCustomerName.SelectedText == String.Empty)
            Procparam.Add("@Customer_ID", null);


        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvRS.BindGridView("S3G_LOANAD_GET_RS_BULKPRINT", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucRSCustomPaging.Visible = true;
        ucRSCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucRSCustomPaging.setPageSize(ProPageSizeRW);
        pnlRS.Visible = true;

        if (bIsNewRow == true)
        {
            gvRS.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGenerateRSFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string strRS_ID, strCustomer_ID = string.Empty;

            foreach (GridViewRow grvRow in gvRS.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    DataSet dsHeader = new DataSet();
                    strRS_ID = ((Label)grvRow.FindControl("lblRS_ID")).Text;
                    strCustomer_ID = ((Label)grvRow.FindControl("lblCustomer_ID")).Text;

                    String strHTML = String.Empty;

                    System.Data.DataTable dt_Obj = new System.Data.DataTable();
                    Dictionary<string, string> Procparam;
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", CompanyID.ToString());
                    Procparam.Add("@Lob_Id", Convert.ToString(Lob_ID));
                    Procparam.Add("@Location_ID", Convert.ToString(Location_ID));
                    Procparam.Add("@Template_Type_Code", "16");
                    dt_Obj = Utility.GetDefaultData("S3G_Get_TemplateCont", Procparam);
                    if (dt_Obj.Rows.Count == 0)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Template not defined in template master");
                        return;
                    }

                    //strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, Template_Type_Code, strRS_ID);
                    //if (strHTML == "")
                    //{
                    //    Utility.FunShowAlertMsg(this, "Template Master not defined");
                    //    return;
                    //}

                    strHTML = dt_Obj.Rows[0]["Template_Content"].ToString();

                    strHTML = strHTML.Replace("~PAGESECTIONBREAK~", "<br clear=all style='page-break-before:always'>");
                    string FileName = PDFPageSetup.FunPubGetFileName(strRS_ID + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                    string DownFile = string.Empty;
                    if (ddlPrintType.SelectedValue == "P")
                    {
                        DownFile = FilePath + FileName + ".pdf";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                    }
                    else if (ddlPrintType.SelectedValue == "W")
                    {
                        DownFile = FilePath + FileName + ".doc";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc";
                    }

                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@AccountCreation_Id", strRS_ID);
                    Procparam.Add("@CustomerID", strCustomer_ID);
                    Procparam.Add("@TypeID", "1");
                    Procparam.Add("@Sec", "0");
                    dsHeader = Utility.GetDataset("S3G_LOANAD_GET_RPT_RSDtls", Procparam);
                    dsHeader.Tables[0].Columns.Add("TotalAmount");

                    if (dsHeader.Tables[0].Rows.Count != 0)
                        dsHeader.Tables[0].Rows[0]["TotalAmount"] = Convert.ToDecimal((dsHeader.Tables[1].Compute("sum(Total_Rental)", ""))).ToString(Funsetsuffix());
                    if (strHTML.Contains("~RepaymentScheduleHeaderTable~"))
                        strHTML = PDFPageSetup.FunPubBindTable("~RepaymentScheduleHeaderTable~", strHTML, dsHeader.Tables[1]);
                    if (strHTML.Contains("~ANX2_Table_Header~"))
                        strHTML = PDFPageSetup.FunPubBindTable("~ANX2_Table_Header~", strHTML, dsHeader.Tables[2]);

                    strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    List<string> listImagePath = new List<string>();
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                    strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                    PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);
                    filepaths.Add(DownFile);
                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);
                FunPriDownloadFile(OutputFile);
            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetRSNo(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataSet Ds = new DataSet();
        obj_Page.ddlRSAccountNumber.SelectedValue = String.Empty;
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@User_ID", Convert.ToString(obj_Page.intUserId));
        Procparam.Add("@SearchOption", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetAccountsCreated_AGT", Procparam));
        return suggetions.ToArray();
    }

    #endregion

    #region MRA

    protected void gvMRA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvMRA.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindMRAGrid()
    {

        if ((txtMRAFromDate.Text != "") && (txtMRAToDate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtMRAFromDate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtMRAToDate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtMRAFromDate.Text))
            Procparam.Add("@MRA_From_Date", Utility.StringToDate(txtMRAFromDate.Text).ToString());

        if (!String.IsNullOrEmpty(txtMRAToDate.Text))
            Procparam.Add("@MRA_To_Date", Utility.StringToDate(txtMRAToDate.Text).ToString());

        if (ddlMRACustomerName.SelectedValue != "0" && ddlMRACustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_ID", ddlMRACustomerName.SelectedValue);
        else if (ddlMRACustomerName.SelectedValue == "0" && ddlMRACustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_ID", ddlMRACustomerName.SelectedValue);
        else if (ddlMRACustomerName.SelectedValue != "0" && ddlMRACustomerName.SelectedText == String.Empty)
            Procparam.Add("@Customer_ID", null);

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvMRA.BindGridView("S3G_LOANAD_GET_MRA_BULKPRINT", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucMRACustomPaging.Visible = true;
        ucMRACustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucMRACustomPaging.setPageSize(ProPageSizeRW);
        pnlMRA.Visible = true;

        if (bIsNewRow == true)
        {
            gvMRA.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGenerateMRAFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strMRA_ID = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";

            foreach (GridViewRow grvRow in gvMRA.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    DataSet dsHeader = new DataSet();
                    strMRA_ID = ((Label)grvRow.FindControl("lblMRA_ID")).Text;

                    String strHTML = String.Empty;
                    strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, Template_Type_Code, strMRA_ID);
                    if (strHTML == "")
                    {
                        Utility.FunShowAlertMsg(this, "Template Master not defined");
                        return;
                    }

                    strHTML = strHTML.Replace("~PAGESECTIONBREAK~", "<br clear=all style='page-break-before:always'>");
                    string FileName = PDFPageSetup.FunPubGetFileName(strMRA_ID + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                    string DownFile = string.Empty;
                    if (ddlPrintType.SelectedValue == "P")
                    {
                        DownFile = FilePath + FileName + ".pdf";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                    }
                    else if (ddlPrintType.SelectedValue == "W")
                    {
                        DownFile = FilePath + FileName + ".doc";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc";
                    }

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", CompanyId.ToString());
                    Procparam.Add("@MRA_ID", strMRA_ID);
                    dsHeader = Utility.GetDataset("S3G_ORG_MRA_Print", Procparam);

                    strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    List<string> listImagePath = new List<string>();
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                    strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                    PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);
                    filepaths.Add(DownFile);
                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);

                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.doc");
                    Response.ContentType = "application/vnd.ms-word";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    #endregion

    #region Pricing

    protected void gvPricing_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvPricing.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindPricingGrid()
    {

        if ((txtPricingFromDate.Text != "") && (txtPricingToDate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtPricingFromDate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtPricingToDate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtPricingFromDate.Text))
            Procparam.Add("@Pricing_From_Date", Utility.StringToDate(txtPricingFromDate.Text).ToString());

        if (!String.IsNullOrEmpty(txtPricingToDate.Text))
            Procparam.Add("@Pricing_To_Date", Utility.StringToDate(txtPricingToDate.Text).ToString());

        if (ddlPricingNo.SelectedValue != "0" && ddlPricingNo.SelectedText != String.Empty)
            Procparam.Add("@PricingNo", ddlPricingNo.SelectedValue);
        else if (ddlPricingNo.SelectedValue == "0" && ddlPricingNo.SelectedText != String.Empty)
            Procparam.Add("@PricingNo", ddlPricingNo.SelectedValue);
        else if (ddlPricingNo.SelectedValue != "0" && ddlPricingNo.SelectedText == String.Empty)
            Procparam.Add("@PricingNo", null);

        if (ddlPricingCustomerName.SelectedValue != "0" && ddlPricingCustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_ID", ddlPricingCustomerName.SelectedValue);
        else if (ddlPricingCustomerName.SelectedValue == "0" && ddlPricingCustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_ID", ddlPricingCustomerName.SelectedValue);
        else if (ddlPricingCustomerName.SelectedValue != "0" && ddlPricingCustomerName.SelectedText == String.Empty)
            Procparam.Add("@Customer_ID", null);


        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvPricing.BindGridView("S3G_LOANAD_GET_PRICING_BULKPRINT", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucPricingCustomPaging.Visible = true;
        ucPricingCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucPricingCustomPaging.setPageSize(ProPageSizeRW);
        pnlPricing.Visible = true;

        if (bIsNewRow == true)
        {
            gvPricing.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGeneratePricingFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strPricingID = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";

            foreach (GridViewRow grvRow in gvPricing.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    DataSet dsHeader = new DataSet();
                    strPricingID = ((Label)grvRow.FindControl("lblPricing_ID")).Text;
                    String strHTML = String.Empty;
                    strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, Template_Type_Code, strPricingID);
                    if (strHTML == "")
                    {
                        Utility.FunShowAlertMsg(this, "Template Master not defined");
                        return;
                    }

                    strHTML = strHTML.Replace("~PAGESECTIONBREAK~", "<br clear=all style='page-break-before:always'>");

                    string FileName = PDFPageSetup.FunPubGetFileName(strPricingID + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                    string DownFile = string.Empty;
                    if (ddlPrintType.SelectedValue == "P")
                    {
                        DownFile = FilePath + FileName + ".pdf";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                    }
                    else if (ddlPrintType.SelectedValue == "W")
                    {
                        DownFile = FilePath + FileName + ".doc";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc";
                    }

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", Convert.ToString(CompanyId));
                    Procparam.Add("@Pricing_ID", strPricingID);
                    //Procparam.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
                    dsHeader = Utility.GetDataset("S3G_Sys_GetTmplPricingDetails", Procparam);

                    if (strHTML.Contains("~Asset_Table~"))
                        strHTML = PDFPageSetup.FunPubBindTable("~Asset_Table~", strHTML, dsHeader.Tables[1]);
                    if (strHTML.Contains("~Quote_Table~"))
                        strHTML = PDFPageSetup.FunPubBindTable("~Quote_Table~", strHTML, dsHeader.Tables[2]);
                    if (strHTML.Contains("~Cashflow_Table~"))
                        strHTML = PDFPageSetup.FunPubBindTable("~Cashflow_Table~", strHTML, dsHeader.Tables[3]);

                    strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsHeader.Tables[0]);

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    List<string> listImagePath = new List<string>();
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                    strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                    PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);
                    filepaths.Add(DownFile);

                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);

                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.doc");
                    Response.ContentType = "application/vnd.ms-word";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPricingNo(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataSet Ds = new DataSet();
        obj_Page.ddlRSAccountNumber.SelectedValue = String.Empty;
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@User_ID", Convert.ToString(obj_Page.intUserId));
        Procparam.Add("@Option", "15");
        Procparam.Add("@SearchOption", "1");
        Procparam.Add("@SearchText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetPricing_List_AGT", Procparam));
        return suggetions.ToArray();
    }

    #endregion

    #region NOA

    protected void gvNOA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvNOA.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindNOAGrid()
    {

        if ((txtNOAFromDate.Text != "") && (txtNOAToDate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtNOAFromDate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtNOAToDate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtNOAFromDate.Text))
            Procparam.Add("@NOA_From_Date", Utility.StringToDate(txtNOAFromDate.Text).ToString());

        if (!String.IsNullOrEmpty(txtNOAToDate.Text))
            Procparam.Add("@NOA_To_Date", Utility.StringToDate(txtNOAToDate.Text).ToString());

        if (ddlDealNo.SelectedValue != "0" && ddlDealNo.SelectedText != String.Empty)
            Procparam.Add("@DealNo", ddlDealNo.SelectedValue);
        else if (ddlDealNo.SelectedValue == "0" && ddlDealNo.SelectedText != String.Empty)
            Procparam.Add("@DealNo", ddlDealNo.SelectedValue);
        else if (ddlDealNo.SelectedValue != "0" && ddlDealNo.SelectedText == String.Empty)
            Procparam.Add("@DealNo", null);

        if (ddlNOACustomerName.SelectedValue != "0" && ddlNOACustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_ID", ddlNOACustomerName.SelectedValue);
        else if (ddlNOACustomerName.SelectedValue == "0" && ddlNOACustomerName.SelectedText != String.Empty)
            Procparam.Add("@Customer_ID", ddlNOACustomerName.SelectedValue);
        else if (ddlNOACustomerName.SelectedValue != "0" && ddlNOACustomerName.SelectedText == String.Empty)
            Procparam.Add("@Customer_ID", null);


        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvNOA.BindGridView("S3G_LOANAD_GET_NOA_BULKPRINT", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucNOACustomPaging.Visible = true;
        ucNOACustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucNOACustomPaging.setPageSize(ProPageSizeRW);
        pnlNOA.Visible = true;

        if (bIsNewRow == true)
        {
            gvNOA.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGenerateNOAFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strNote_id = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";

            foreach (GridViewRow grvRow in gvNOA.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    String strHTML = String.Empty;
                    strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, Template_Type_Code, strNote_id);
                    if (strHTML == "")
                    {
                        Utility.FunShowAlertMsg(this, "Template Master not defined");
                        return;
                    }

                    strHTML = strHTML.Replace("~PAGESECTIONBREAK~", "<br clear=all style='page-break-before:always'>");
                    DataSet dsprint = new DataSet();
                    strNote_id = ((Label)grvRow.FindControl("lblDeal_ID")).Text;
                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();

                    Procparam.Add("@Note_ID", Convert.ToString(strNote_id));
                    Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
                    Procparam.Add("@USER_ID", Convert.ToString(intUserId));
                    DataSet dsFunder = Utility.GetDataset("S3G_FUNDMGT_GETNote", Procparam);

                    DataSet ds = new DataSet();
                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@user_id", intUserId.ToString());
                    Procparam.Add("@customer_id", dsFunder.Tables[0].Rows[0]["Customer_ID"].ToString());
                    Procparam.Add("@Funder_id", dsFunder.Tables[0].Rows[0]["Funder_ID"].ToString());
                    Procparam.Add("@Note_id", strNote_id.ToString());
                    ds = Utility.GetDataset("S3G_Fund_NoteTranche", Procparam);
                    ViewState["dtTranche"] = ds.Tables[0];
                    ViewState["Accounts"] = ds.Tables[1];
                    DataTable Accounts = new DataTable();
                    Accounts = ds.Tables[1];
                    DataTable dtTranche = new DataTable();
                    dtTranche = ds.Tables[0];

                    for (int i = 0; i < Accounts.Rows.Count; i++)
                    {
                        strHTML = String.Empty;
                        strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, Template_Type_Code, strNote_id);
                    
                        if (Procparam != null)
                            Procparam.Clear();
                        else

                            Procparam = new Dictionary<string, string>();
                        Procparam.Add("@Company_ID", intCompanyID.ToString());
                        Procparam.Add("@user_id", intUserId.ToString());
                        Procparam.Add("@Acc_XMl", dtTranche.FunPubFormXml());
                        Procparam.Add("@customer_id", dsFunder.Tables[0].Rows[0]["Customer_ID"].ToString());
                        Procparam.Add("@funder_id", dsFunder.Tables[0].Rows[0]["Funder_ID"].ToString());
                        Procparam.Add("@Note_date", (Utility.StringToDate(dsFunder.Tables[0].Rows[0]["Note_date"].ToString())).ToString());
                        Procparam.Add("@TypeID", ddlReportFormatType.SelectedValue);
                        Procparam.Add("@Note_ID", strNote_id.ToString());
                        Procparam.Add("@Tranche_ID", Accounts.Rows[i]["Tranch_Header_ID"].ToString());
                        Procparam.Add("@PASA_ID", Accounts.Rows[i]["PA_SA_Ref_ID"].ToString());

                        dsprint = Utility.GetDataset("S3G_Fund_Get_NotePrint", Procparam);

                        string FileName = PDFPageSetup.FunPubGetFileName(strNote_id + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                        string DownFile = string.Empty;
                        if (ddlPrintType.SelectedValue == "P")
                        {
                            DownFile = FilePath + FileName + ".pdf";
                            OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                        }
                        else if (ddlPrintType.SelectedValue == "W")
                        {
                            DownFile = FilePath + FileName + ".doc";
                            OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc";
                        }

                        if (strHTML.Contains("~DTLTBL1~"))
                            strHTML = PDFPageSetup.FunPubBindTableWithMerge("~DTLTBL1~", strHTML, dsprint.Tables[1]);
                        if (strHTML.Contains("~DTLTBL2~"))
                            strHTML = PDFPageSetup.FunPubBindTable("~DTLTBL2~", strHTML, dsprint.Tables[2]);
                        if (strHTML.Contains("~DTLTBL3~"))
                            strHTML = PDFPageSetup.FunPubBindTable("~DTLTBL3~", strHTML, dsprint.Tables[3]);

                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsprint.Tables[0]);

                        List<string> listImageName = new List<string>();
                        listImageName.Add("~CompanyLogo~");
                        listImageName.Add("~InvoiceSignStamp~");
                        listImageName.Add("~POSignStamp~");
                        List<string> listImagePath = new List<string>();
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                        listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                        strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                        PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);
                        filepaths.Add(DownFile);

                    }
                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);

                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.doc");
                    Response.ContentType = "application/vnd.ms-word";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetDealNo(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataSet Ds = new DataSet();
        obj_Page.ddlRSAccountNumber.SelectedValue = String.Empty;
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@User_ID", Convert.ToString(obj_Page.intUserId));
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_NoteNumber_AGT", Procparam));
        return suggetions.ToArray();
    }

    #endregion

    #region Debit/Credit

    protected void gvDC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvDC.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindDCGrid()
    {

        if ((txtDCFromDate.Text != "") && (txtDCToDate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtDCFromDate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtDCToDate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtDCFromDate.Text))
            Procparam.Add("@DC_From_Date", Utility.StringToDate(txtDCFromDate.Text).ToString());

        if (!String.IsNullOrEmpty(txtDCToDate.Text))
            Procparam.Add("@DC_To_Date", Utility.StringToDate(txtDCToDate.Text).ToString());

        if (ddlDCNo.SelectedValue != "0" && ddlDCNo.SelectedText != String.Empty)
            Procparam.Add("@DCNo", ddlDCNo.SelectedValue);
        else if (ddlDCNo.SelectedValue == "0" && ddlDCNo.SelectedText != String.Empty)
            Procparam.Add("@DCNo", ddlDCNo.SelectedValue);
        else if (ddlDCNo.SelectedValue != "0" && ddlDCNo.SelectedText == String.Empty)
            Procparam.Add("@DCNo", null);

        if (ddlDCEntityName.SelectedValue != "0" && ddlDCEntityName.SelectedText != String.Empty)
            Procparam.Add("@Entity_ID", ddlDCEntityName.SelectedValue);
        else if (ddlDCEntityName.SelectedValue == "0" && ddlDCEntityName.SelectedText != String.Empty)
            Procparam.Add("@Entity_ID", ddlDCEntityName.SelectedValue);
        else if (ddlDCEntityName.SelectedValue != "0" && ddlDCEntityName.SelectedText == String.Empty)
            Procparam.Add("@Entity_ID", null);

        if (ddldocumentType.SelectedValue == "6")
        {
            Procparam.Add("@TransType", "2");
        }
        else if (ddldocumentType.SelectedValue == "7")
        {
            Procparam.Add("@TransType", "1");
        }


        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvDC.BindGridView("S3G_LOANAD_GET_DC_BULKPRINT", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucDCCustomPaging.Visible = true;
        ucDCCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucDCCustomPaging.setPageSize(ProPageSizeRW);
        pnlDC.Visible = true;

        if (bIsNewRow == true)
        {
            gvDC.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGenerateDCFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strNote_id = string.Empty;
            string strhtmlFile = string.Empty;
            string strType = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";

            foreach (GridViewRow grvRow in gvDC.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    strNote_id = ((Label)grvRow.FindControl("lblDC_ID")).Text;
                    strType = ((Label)grvRow.FindControl("lblType")).Text;

                    String strHTML = String.Empty;

                    if (strType == "Cr")
                        strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, 21, strNote_id);
                    else if (strType == "Extension Rental")
                        strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, 12, strNote_id);
                    else if (strType == "Extension AMF")
                        strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, 52, strNote_id);
                    else
                        strHTML = PDFPageSetup.FunPubGetTemplateContent(CompanyID, Lob_ID, Location_ID, 20, strNote_id);

                    if (strHTML == "")
                    {
                        Utility.FunShowAlertMsg(this, "Template Master not defined");
                        return;
                    }

                    strHTML = strHTML.Replace("~PAGESECTIONBREAK~", "<br clear=all style='page-break-before:always'>");
                    string FileName = PDFPageSetup.FunPubGetFileName(strNote_id + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));
                    string DownFile = string.Empty;
                    if (ddlPrintType.SelectedValue == "P")
                    {
                        DownFile = FilePath + FileName + ".pdf";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
                    }
                    else if (ddlPrintType.SelectedValue == "W")
                    {
                        DownFile = FilePath + FileName + ".doc";
                        OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".doc";
                    }

                    DataSet dsprint = new DataSet();

                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();

                    DataSet dsPrintDetails = new DataSet();
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@User_ID", intUserId.ToString());
                    Procparam.Add("@DCNote_Tran_ID", strNote_id);
                    dsPrintDetails = Utility.GetDataset("S3G_DCN_GET_DCNOTE", Procparam);

                    DataTable dtFinal = new DataTable();
                    dtFinal = MergeTablesByIndex(dsPrintDetails.Tables[0], dsPrintDetails.Tables[2]);
                    dtFinal = MergeTablesByIndex(dtFinal, FunPriPmtHDR(dsPrintDetails.Tables[0]));

                    strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dtFinal);
                    if (strHTML.Contains("~Details_Table~"))
                        strHTML = PDFPageSetup.FunPubBindTable("~Details_Table~", strHTML, dsPrintDetails.Tables[1]);

                    List<string> listImageName = new List<string>();
                    listImageName.Add("~CompanyLogo~");
                    listImageName.Add("~InvoiceSignStamp~");
                    listImageName.Add("~POSignStamp~");
                    List<string> listImagePath = new List<string>();
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
                    listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));

                    strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
                    PDFPageSetup.FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue);
                    filepaths.Add(DownFile);

                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);

                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.doc");
                    Response.ContentType = "application/vnd.ms-word";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    private DataTable MergeTablesByIndex(DataTable t1, DataTable t2)
    {
        if (t1 == null || t2 == null) throw new ArgumentNullException("t1 or t2", "Both tables must not be null");

        DataTable t3 = t1.Clone();  // first add columns from table1
        foreach (DataColumn col in t2.Columns)
        {
            string newColumnName = col.ColumnName;
            int colNum = 1;
            while (t3.Columns.Contains(newColumnName))
            {
                newColumnName = string.Format("{0}_{1}", col.ColumnName, ++colNum);
            }
            t3.Columns.Add(newColumnName, col.DataType);
        }
        var mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(),
            (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
        foreach (object[] rowFields in mergedRows)
            t3.Rows.Add(rowFields);

        return t3;
    }

    private DataTable FunPriPmtHDR(DataTable dts)
    {
        DataTable dt = new DataTable();
        try
        {
            UserInfo ObjUserInfo = new UserInfo();
            DataRow drEmptyRow;
            dt.Columns.Add("activity");
            dt.Columns.Add("Company_id");
            dt.Columns.Add("Company_Name");
            dt.Columns.Add("company_address");
            dt.Columns.Add("Location");
            dt.Columns.Add("Username");
            dt.Columns.Add("Document_no");
            dt.Columns.Add("Document_date");
            dt.Columns.Add("batch");
            dt.Columns.Add("heading");
            drEmptyRow = dt.NewRow();
            drEmptyRow["activity"] = 0;
            drEmptyRow["Company_id"] = intCompanyID;
            drEmptyRow["Company_Name"] = ObjUserInfo.ProCompanyNameRW.ToString();
            drEmptyRow["company_address"] = "";
            drEmptyRow["Location"] = dts.Rows[0]["Location_ID"].ToString();
            drEmptyRow["Username"] = ObjUserInfo.ProUserNameRW.ToString();
            drEmptyRow["Document_no"] = dts.Rows[0]["DOCUMENT_NO"].ToString();
            drEmptyRow["Document_date"] = dts.Rows[0]["Document_Date"].ToString();
            drEmptyRow["batch"] = dts.Rows[0]["Tran_Currency_Amount"].ToString();
            if (ddldocumentType.SelectedValue == "6")
                drEmptyRow["heading"] = "Debit Note";
            else
                drEmptyRow["heading"] = "Credit Note";
            dt.Rows.Add(drEmptyRow);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
        return dt;
    }

    [System.Web.Services.WebMethod]
    public static string[] GetDCEntityName(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataSet Ds = new DataSet();
        obj_Page.ddlRSAccountNumber.SelectedValue = String.Empty;
        Procparam.Clear();
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", Convert.ToString(obj_Page.intCompanyID));
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANADMIN_CUSTENTITY_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetDCNo(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataSet Ds = new DataSet();
        obj_Page.ddlRSAccountNumber.SelectedValue = String.Empty;
        Procparam.Clear();
        Procparam.Add("@COMPANY_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@USER_ID", Convert.ToString(obj_Page.intUserId));
        Procparam.Add("@Task_Type", "60");
        Procparam.Add("@Program_Id", "298");
        Ds = Utility.GetDataset("S3G_DCN_GET_DOCDET", Procparam);
        suggetions = Utility.GetSuggestions(Ds.Tables[0]);
        return suggetions.ToArray();
    }

    #endregion

    #region Credit Note Print

    protected void gvCN_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvCN.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindCNGrid()
    {

        if ((txtCNFromDate.Text != "") && (txtCNToDate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtCNFromDate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtCNToDate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtCNFromDate.Text))
            Procparam.Add("@CN_From_Date", Utility.StringToDate(txtCNFromDate.Text).ToString());

        if (!String.IsNullOrEmpty(txtCNToDate.Text))
            Procparam.Add("@CN_To_Date", Utility.StringToDate(txtCNToDate.Text).ToString());

        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvCN.BindGridView("S3G_LOANAD_GET_CN_BULKPRINT", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucCNCustomPaging.Visible = true;
        ucCNCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucCNCustomPaging.setPageSize(ProPageSizeRW);
        pnlCN.Visible = true;

        if (bIsNewRow == true)
        {
            gvCN.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGenerateCNFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            string AccountNo, InstallNo, Cashflow, DocumentPath = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
            Procparam.Add("@Program_ID", "508");
            dt = Utility.GetDefaultData("S3G_LAD_GETDOCPATH", Procparam);
            DocumentPath = dt.Rows[0]["Document_Path"].ToString();

            foreach (GridViewRow grvRow in gvCN.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    AccountNo = ((Label)grvRow.FindControl("lblRS_Number")).Text.Replace("/", "_");
                    InstallNo = ((Label)grvRow.FindControl("lblInstallment_No")).Text.Replace("/", "_");
                    Cashflow = "311";

                    string DownFile = DocumentPath + "/CreditNote/" + AccountNo + "_" + InstallNo + "_" + Cashflow + ".pdf";
                    filepaths.Add(DownFile);
                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                string[] filesToMerge = filepaths.ToArray();
                CombineMultiplePDFs(filesToMerge, OutputFile);
                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    # endregion

    #region CashFlow Print

    protected void gvOCP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvOCP.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindOCPGrid()
    {

        if ((txtOCPFromdate.Text != "") && (txtOCPTodate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtOCPFromdate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtOCPTodate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtOCPFromdate.Text))
            Procparam.Add("@STARTDATE", Utility.StringToDate(txtOCPFromdate.Text).ToString());

        if (!String.IsNullOrEmpty(txtOCPTodate.Text))
            Procparam.Add("@ENDDATE", Utility.StringToDate(txtOCPTodate.Text).ToString());

        Procparam.Add("@Status_ID", "0");

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvOCP.BindGridView("S3G_LOANAD_CASHFLWPRT_RSNO", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucOCPCustomPaging.Visible = true;
        ucOCPCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucOCPCustomPaging.setPageSize(ProPageSizeRW);
        pnlOCP.Visible = true;

        if (bIsNewRow == true)
        {
            gvOCP.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGenerateOCPFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            string AccountNo, Cashflow, CashflowFlag, DocumentPath = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
            Procparam.Add("@Program_ID", "335");
            dt = Utility.GetDefaultData("S3G_LAD_GETDOCPATH", Procparam);
            DocumentPath = dt.Rows[0]["Document_Path"].ToString();

            foreach (GridViewRow grvRow in gvOCP.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    AccountNo = ((Label)grvRow.FindControl("lblTranche_Name")).Text.Replace("/", "_");
                    Cashflow = ((Label)grvRow.FindControl("lblCashflow")).Text.Replace("/", "_");
                    CashflowFlag = ((Label)grvRow.FindControl("lblCashflowFlag")).Text.Replace("/", "_");
                    string DownFile = DocumentPath + "/OtherCashflow/" + AccountNo + "_" + CashflowFlag + ".pdf";
                    filepaths.Add(DownFile);
                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                string[] filesToMerge = filepaths.ToArray();
                CombineMultiplePDFs(filesToMerge, OutputFile);
                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    #endregion

    #region Rental Invoices

    protected void gvRI_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvRI.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindRIGrid()
    {

        if ((txtRIFromDate.Text != "") && (txtRIToDate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtRIFromDate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtRIToDate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtRIFromDate.Text))
            Procparam.Add("@STARTDATE", Utility.StringToDate(txtRIFromDate.Text).ToString());

        if (!String.IsNullOrEmpty(txtRIToDate.Text))
            Procparam.Add("@ENDDATE", Utility.StringToDate(txtRIToDate.Text).ToString());

        if (ddlRIBillNumber.SelectedValue != "0" && ddlRIBillNumber.SelectedText != String.Empty)
            Procparam.Add("@BillNo", ddlRIBillNumber.SelectedValue);
        else if (ddlRIBillNumber.SelectedValue == "0" && ddlRIBillNumber.SelectedText != String.Empty)
            Procparam.Add("@BillNo", ddlRIBillNumber.SelectedValue);
        else if (ddlRIBillNumber.SelectedValue != "0" && ddlRIBillNumber.SelectedText == String.Empty)
            Procparam.Add("@BillNo", null);

        if (ddlRILocation.SelectedValue != "0" && ddlRILocation.SelectedText != String.Empty)
            Procparam.Add("@Location", ddlRILocation.SelectedValue);
        else if (ddlRILocation.SelectedValue == "0" && ddlRILocation.SelectedText != String.Empty)
            Procparam.Add("@Location", ddlRILocation.SelectedValue);
        else if (ddlRILocation.SelectedValue != "0" && ddlRILocation.SelectedText == String.Empty)
            Procparam.Add("@Location", null);

        //Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvRI.BindGridView("S3G_LOANAD_GET_RI_BULKPRINT", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucRICustomPaging.Visible = true;
        ucRICustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucRICustomPaging.setPageSize(ProPageSizeRW);
        pnlRI.Visible = true;

        if (bIsNewRow == true)
        {
            gvRI.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGenerateRIFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            string AccountNo, Billno, DocumentPath = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";

            Procparam = new Dictionary<string, string>();
            Procparam = new Dictionary<string, string>();
            //Procparam.Add("@LobId", ddlLOB.SelectedValue);
            Procparam.Add("@CompanyId", intCompanyID.ToString());
            DataTable dtDocPath = Utility.GetDefaultData("s3g_LoanAd_GetBillingDocPath", Procparam);
            DocumentPath = dtDocPath.Rows[0]["Document_Path"].ToString();

            foreach (GridViewRow grvRow in gvRI.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    AccountNo = ((Label)grvRow.FindControl("lblAccNumber")).Text.Replace("/", "_");
                    Billno = "BillNo-" + ((Label)grvRow.FindControl("lblBillNumber")).Text.Replace("/", "_");
                    string DownFile = DocumentPath + "/" + Billno + "/Rental/" + AccountNo + ".pdf";
                    filepaths.Add(DownFile);
                }
            }
            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                string[] filesToMerge = filepaths.ToArray();
                CombineMultiplePDFs(filesToMerge, OutputFile);
                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBillNo(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@SearchText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_BILLNUMBER_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLocation(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@User_ID", Convert.ToString(obj_Page.intUserId));
        Procparam.Add("@Type", "GEN");
        //Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GET_BRANCHLIST_AGT", Procparam));
        return suggetions.ToArray();
    }

    #endregion

    #region Interim Invoices

    protected void gvII_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvII.ClientID + "',this,'chkSelected');");
            }
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindIIGrid()
    {

        if ((txtIIFromDate.Text != "") && (txtIIToDate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtIIFromDate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtIIToDate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtIIFromDate.Text))
            Procparam.Add("@StartDate", Utility.StringToDate(txtIIFromDate.Text).ToString());

        if (!String.IsNullOrEmpty(txtIIToDate.Text))
            Procparam.Add("@EndDate", Utility.StringToDate(txtIIToDate.Text).ToString());

        if (ddlIIAccNumber.SelectedValue != "0" && ddlIIAccNumber.SelectedText != String.Empty)
            Procparam.Add("@Panum", ddlIIAccNumber.SelectedValue);
        else if (ddlIIAccNumber.SelectedValue == "0" && ddlIIAccNumber.SelectedText != String.Empty)
            Procparam.Add("@Panum", ddlIIAccNumber.SelectedValue);
        else if (ddlIIAccNumber.SelectedValue != "0" && ddlIIAccNumber.SelectedText == String.Empty)
            Procparam.Add("@Panum", null);

        if (ddlIIDocNumber.SelectedValue != "0" && ddlIIDocNumber.SelectedText != String.Empty)
            Procparam.Add("@DocumentNumber", ddlIIDocNumber.SelectedValue);
        else if (ddlIIDocNumber.SelectedValue == "0" && ddlIIDocNumber.SelectedText != String.Empty)
            Procparam.Add("@DocumentNumber", ddlIIDocNumber.SelectedValue);
        else if (ddlIIDocNumber.SelectedValue != "0" && ddlIIDocNumber.SelectedText == String.Empty)
            Procparam.Add("@DocumentNumber", null);

        Procparam.Add("@ProgramCode", "INTBP");

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvII.BindGridView("S3G_LOANAD_TransLander", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucIICustomPaging.Visible = true;
        ucIICustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucIICustomPaging.setPageSize(ProPageSizeRW);
        pnlII.Visible = true;

        if (bIsNewRow == true)
        {
            gvII.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriBindStockGrid()
    {

        if ((txtIIFromDate.Text != "") && (txtIIToDate.Text != ""))
        {
            if (Convert.ToDateTime(Utility.StringToDate(txtIIFromDate.Text)) > Convert.ToDateTime(Utility.StringToDate(txtIIToDate.Text).ToString()))
            {
                Utility.FunShowAlertMsg(this, "From Date cannot be greater than To Date");
                return;
            }
        }

        Procparam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtIIFromDate.Text))
            Procparam.Add("@StartDate", Utility.StringToDate(txtIIFromDate.Text).ToString());

        if (!String.IsNullOrEmpty(txtIIToDate.Text))
            Procparam.Add("@EndDate", Utility.StringToDate(txtIIToDate.Text).ToString());

        if (ddlIIAccNumber.SelectedValue != "0" && ddlIIAccNumber.SelectedText != String.Empty)
            Procparam.Add("@Panum", ddlIIAccNumber.SelectedValue);
        else if (ddlIIAccNumber.SelectedValue == "0" && ddlIIAccNumber.SelectedText != String.Empty)
            Procparam.Add("@Panum", ddlIIAccNumber.SelectedValue);
        else if (ddlIIAccNumber.SelectedValue != "0" && ddlIIAccNumber.SelectedText == String.Empty)
            Procparam.Add("@Panum", null);

        if (ddlIIDocNumber.SelectedValue != "0" && ddlIIDocNumber.SelectedText != String.Empty)
            Procparam.Add("@DocumentNumber", ddlIIDocNumber.SelectedValue);
        else if (ddlIIDocNumber.SelectedValue == "0" && ddlIIDocNumber.SelectedText != String.Empty)
            Procparam.Add("@DocumentNumber", ddlIIDocNumber.SelectedValue);
        else if (ddlIIDocNumber.SelectedValue != "0" && ddlIIDocNumber.SelectedText == String.Empty)
            Procparam.Add("@DocumentNumber", null);

        Procparam.Add("@ProgramCode", "STKTP");

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyID;
        gvII.BindGridView("S3G_LOANAD_TransLander", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucIICustomPaging.Visible = true;
        ucIICustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucIICustomPaging.setPageSize(ProPageSizeRW);
        pnlII.Visible = true;

        if (bIsNewRow == true)
        {
            gvII.Rows[0].Visible = false;
            btnPrint.Enabled = false;
        }
        else
        {
            btnPrint.Enabled = true;
        }

    }

    private void FunPriGenerateIIFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strid = string.Empty;
            string strinvoiceno = string.Empty;
            string strdocumentPath = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string strnewFile = string.Empty;
            foreach (GridViewRow grvRow in gvII.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    strid = ((Label)grvRow.FindControl("lblInterimId")).Text;
                    strinvoiceno = ((Label)grvRow.FindControl("lblAccNumber")).Text;
                    strdocumentPath = ((Label)grvRow.FindControl("lblDocumentPath")).Text;
                    //string strnewfile = ViewState["strnewFile"].ToString();
                     strnewFile = strdocumentPath + "INTBNo-" + strid;

                    //strnewfile = strnewfile + strid;
                    string DownFile = string.Empty;

                    strnewFile += "/" + "Rental";

                    strnewFile = strnewFile.Replace("\\", "/");

                    // DirectoryInfo Sourcedir = new DirectoryInfo(strnewfile);

                    //strnewfile = Server.MapPath(".") + strnewfile;
                    //FileInfo[] files = Sourcedir.GetFiles();
                    // FileInfo[] files = Directory.GetFiles(strnewfile, "GANESH0004");
                    strinvoiceno = strinvoiceno.Replace("/", "_");
                    strinvoiceno = strinvoiceno.Replace(" ", "_");

                    string outfile = string.Empty;
                    string[] str = Directory.GetFiles(strnewFile, strinvoiceno + "*");

                    //if (Convert.ToInt32(str.Length.ToString()) > 0)
                    //{

                    //    //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";
                    //    CombineMultiplePDFs(str, strnewFile + "/File.pdf");
                    //}

                    //else
                    //{
                        strinvoiceno = "/" + strinvoiceno + ".pdf";
                        strnewFile = strnewFile + strinvoiceno;
                        //string[] str1 = Directory.GetFiles(strnewfile, strtranche + "*");
                        //if (Convert.ToInt32(str1.Length.ToString()) > 0)
                        //{
                        //FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(str1, false, 0);
                        //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";
                        //FileInfo fl = new FileInfo(strnewFile);
                        //if (fl.Exists == true)
                        //{
                        //    string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strnewFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
                        //}


                        ////}
                        //else
                        //{
                        //    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                        //    return;
                        //}
                    //}




                    //DownFile = FilePath + FileName + ".pdf";


                    filepaths.Add(strnewFile);

                     OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");


                       

                }
            }

            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);

                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.doc");
                    Response.ContentType = "application/vnd.ms-word";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    private void FunPriGenerateStockFiles(int CompanyID, int Lob_ID, int Location_ID, int Template_Type_Code)
    {
        try
        {
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;
            string strid = string.Empty;
            string strinvoiceno = string.Empty;
            string strdocumentPath = string.Empty;
            string strhtmlFile = string.Empty;
            var filepaths = new List<string>();
            string OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");
            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string strnewFile = string.Empty;
            foreach (GridViewRow grvRow in gvII.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelected")).Checked)
                {
                    strid = ((Label)grvRow.FindControl("lblInterimId")).Text;
                    strinvoiceno = ((Label)grvRow.FindControl("lblAccNumber")).Text;
                    strdocumentPath = ((Label)grvRow.FindControl("lblDocumentPath")).Text;
                    //string strnewfile = ViewState["strnewFile"].ToString();
                    strnewFile = strdocumentPath;

                    //strnewfile = strnewfile + strid;
                    string DownFile = string.Empty;

                    strnewFile += "/" + "StockTransfer";

                    strnewFile = strnewFile.Replace("\\", "/");

                    // DirectoryInfo Sourcedir = new DirectoryInfo(strnewfile);

                    //strnewfile = Server.MapPath(".") + strnewfile;
                    //FileInfo[] files = Sourcedir.GetFiles();
                    // FileInfo[] files = Directory.GetFiles(strnewfile, "GANESH0004");
                    strinvoiceno = strinvoiceno.Replace("/", "_");
                    strinvoiceno = strinvoiceno.Replace(" ", "_");

                    string outfile = string.Empty;
                    string[] str = Directory.GetFiles(strnewFile, strinvoiceno + "*");

                    //if (Convert.ToInt32(str.Length.ToString()) > 0)
                    //{

                    //    //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";
                    //    CombineMultiplePDFs(str, strnewFile + "/File.pdf");
                    //}

                    //else
                    //{
                    strinvoiceno = "/" + strinvoiceno + ".pdf";
                    strnewFile = strnewFile + strinvoiceno;
                    //string[] str1 = Directory.GetFiles(strnewfile, strtranche + "*");
                    //if (Convert.ToInt32(str1.Length.ToString()) > 0)
                    //{
                    //FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(str1, false, 0);
                    //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";
                    //FileInfo fl = new FileInfo(strnewFile);
                    //if (fl.Exists == true)
                    //{
                    //    string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strnewFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
                    //}


                    ////}
                    //else
                    //{
                    //    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                    //    return;
                    //}
                    //}




                    //DownFile = FilePath + FileName + ".pdf";


                    filepaths.Add(strnewFile);

                    OutputFile = Server.MapPath(".") + "\\PDF Files\\" + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss");




                }
            }

            if (filepaths.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one record");
                return;
            }
            else
            {
                FunPriGenerateFiles(filepaths, OutputFile, ddlPrintType.SelectedValue);

                if (ddlPrintType.SelectedValue == "P")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.pdf");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(OutputFile);
                }
                else if (ddlPrintType.SelectedValue == "W")
                {
                    Response.AppendHeader("content-disposition", "attachment; filename=BulkPrint.doc");
                    Response.ContentType = "application/vnd.ms-word";
                    Response.WriteFile(OutputFile);
                }

            }

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Print" + ex.ToString());
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAccountNumber(String prefixText)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Option", "4");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", Convert.ToString(obj_Page.intCompanyID));
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetDocNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Add("@Option", "5");
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", Convert.ToString(obj_Page.intCompanyID));
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    #endregion
}