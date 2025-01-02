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
using System.Collections.Generic;
using S3GBusEntity.Reports;
using S3GBusEntity.FundManagement;
using System.Globalization;
using S3GBusEntity;
using System.ServiceModel;
using AjaxControlToolkit;
using System.IO;
using System.Text;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Label = System.Web.UI.WebControls.Label;

public partial class Fund_Management_S3G_FundMgt_Tranche_add : ApplyThemeForProject
{

    FunderMgtServiceReference.FundMgtServiceClient objFundMgtServiceClient;
    FundMgtServices.S3G_Tranche_CreationDataTable objFunderDatatable;
    FundMgtServices.S3G_Tranche_CreationRow objFunderRow;
    int intCompanyID, intUserID = 0;
    string strMode = string.Empty;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    long intTrancheId;
    int strtranche_id;
    int intCustEmailRows = 0;
    string XMLCommon = string.Empty;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    public string strDateFormat = string.Empty;
    //string strMode = string.Empty;
    static string strPageName = "Tranche Creation";
    static string strSuffix = "";
    FormsAuthenticationTicket Ticket;
    string _Add = "1", _Edit = "2";
    DataTable dtRentalSchedule = new DataTable();
    DataTable dtRs = new DataTable();
    public static Fund_Management_S3G_FundMgt_Tranche_add obj_Page;
    DataRow dr;
    DataTable dtcashflow;
    DataSet dscashflow;
    DataSet dsfunder;
    DataTable dtsacnction;
    DataTable dtprint;
    DataSet dsprint;
    bool boolexists;
    DataTable dtfund_modify;
    DataTable dtfundsave;
    string strdue_date, strTenure, strFrquency;
    int d_cnt;
    DataTable dtsource;
    DataTable dttarget;
    DataTable dtFinal;
    DataTable dsasset;
    DataTable dtassettotal;
    DataTable dtcheck;
    DataTable dtcheckRs;
    DataSet dsprocess;
    string due_date;
    string Tenure;
    string frequency;
    DataTable dttranche;
    DataTable dtsanction;
    System.Data.DataTable DtAlertDetails = new System.Data.DataTable();
    System.Data.DataTable DtFollowUp = new System.Data.DataTable();
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Fund Management/S3G_FUNDMGT_TrancheCreation_View.aspx";
    string strRedirectPageAdd = "window.location.href='../Fund Management/S3G_FundMgt_Tranche_add.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../Fund Management/S3G_FUNDMGT_TrancheCreation_View.aspx';";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjCustomerService;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            S3GSession ObjS3GSession = null;
            //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            lblHeading.Text = "Tranche Creation";
            txtcheck.Style.Add("display", "none");
            if (grvrswise.Rows.Count > 0)
            {

                btnProcess.OnClientClick = "return ConfirmOnDelete();";
            }
            //Date Format
            ObjS3GSession = new S3GSession();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            //txttranchedate.Text = DateTime.Now.ToString(strDateFormat);
            CalendarExtenderTranchedate.Format = strDateFormat;
            caldisb.Format = strDateFormat;

            txtdisbursementdate.Attributes.Add("onblur", "fnDoDate(this,'" + txtdisbursementdate.ClientID + "','" + strDateFormat + "',null,null);");
            txttranchedate.Attributes.Add("onblur", "fnDoDate(this,'" + txttranchedate.ClientID + "','" + strDateFormat + "',null,null);");

            FunPriloadPage();
        }
        catch (Exception objException)
        {
        }
    }
    private void FunPriloadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strMode = Request.QueryString.Get("qsMode");
                strtranche_id = Convert.ToInt32(fromTicket.Name);
            }
            strMode = Request.QueryString["qsMode"];

            if (!IsPostBack)
            {
                funpriclearviewstate();
                txtLRdisc.SetDecimalPrefixSuffix(25, 4, false, false, "Rental Total Disc Value");
                txtTaxdisc.SetDecimalPrefixSuffix(25, 4, false, false, "VAT Total Disc Value");
                txtAMFdisc.SetDecimalPrefixSuffix(25, 4, false, false, "AMF Total Disc Value");
                txtSerDisc.SetDecimalPrefixSuffix(25, 4, false, false, "Service Tax Total Disc Value");
                txttranchedate.Text = DateTime.Now.ToString(strDateFormat);

                if (strMode == "M")                     //Modify Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    Session["cnt"] = d_cnt;
                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Tranche_ID", Convert.ToString(strtranche_id));
                    Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
                    Procparam.Add("@USER_ID", Convert.ToString(intUserID));
                    dtcheck = Utility.GetDefaultData("S3G_FUNDMGT_chktranche", Procparam);
                    if (dtcheck.Rows.Count > 0)
                    {
                        string str_text = "The Rental Schedule details has been changed";
                        str_text += "(";
                        foreach (DataRow dr in dtcheck.Rows)
                        {
                            str_text += dr["panum"].ToString() + ",";
                        }
                        str_text = str_text.Substring(0, str_text.Length - 1);
                        str_text += ")";
                        ViewState["ShowDiscValue"] = "Yes";
                        Utility.FunShowAlertMsg(this.Page, str_text);
                    }
                    FunPriLoadTrancheDtls();
                    //FunPriBindAlertDLL(_Add);
                    //FunPriBindFollowupDLL(_Add);

                    FunPriEnableDisableControls();

                    //tbgeneral.Enabled = false;
                    //TabPanel1.Enabled = false;



                }
                else if (strMode == "Q")                //Query Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    Session["cnt"] = d_cnt;
                    FunPriLoadTrancheDtls();
                    //FunPriBindAlertDLL(_Add);
                    //FunPriBindFollowupDLL(_Add);
                    FunPriEnableDisableControls();
                }
                else                                   //Create Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    d_cnt = 0;
                    Session["cnt"] = d_cnt;
                    FunPriBindAlertDLL(_Add);
                    FunPriBindFollowupDLL(_Add);
                    btnPrint.Enabled = btprintExcel.Enabled = btnprintwoa.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriSetEmptyCashtbl()
    {
        try
        {
            DataRow drEmptyRow;
            dtcashflow = new DataTable();
            dtcashflow.Columns.Add("Date");
            dtcashflow.Columns.Add("CashFlow_Type");
            dtcashflow.Columns.Add("CashFlow_Type_id");
            dtcashflow.Columns.Add("CashOutFlow");
            dtcashflow.Columns.Add("CashOutFlow_id");
            dtcashflow.Columns.Add("OutflowFrom");
            dtcashflow.Columns.Add("OutflowFrom_id");
            dtcashflow.Columns.Add("Entity");
            dtcashflow.Columns.Add("Entity_id");
            dtcashflow.Columns.Add("Amount");
            drEmptyRow = dtcashflow.NewRow();
            dtcashflow.Rows.Add(drEmptyRow);
            Session["dtcashflow"] = dtcashflow;
            FunFillgrid(gvOutFlow, dtcashflow);
            gvOutFlow.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

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

    protected void gvOutFlow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlOutflowDesc = (DropDownList)e.Row.FindControl("ddlOutflowDesc") as DropDownList;
                DropDownList ddlPaymentto_OutFlow = (DropDownList)e.Row.FindControl("ddlPaymentto_OutFlow") as DropDownList;
                DropDownList ddloutflowtype = (DropDownList)e.Row.FindControl("ddloutflowtype") as DropDownList;
                CalendarExtender CalendarExtenderSD_OutflowDate = (CalendarExtender)e.Row.FindControl("CalendarExtenderSD_OutflowDate");
                CalendarExtenderSD_OutflowDate.Format = strDateFormat;
                if (Session["dscashflow"] == null)
                {
                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@user_id", intUserID.ToString());
                    dscashflow = Utility.GetDataset("S3G_Fund_Getcashflow", Procparam);
                    Session["dscashflow"] = dscashflow;
                }
                dscashflow = (DataSet)Session["dscashflow"];
                ddlOutflowDesc.FillDataTable(dscashflow.Tables[0], "id", "Flag_Desc");
                ddloutflowtype.FillDataTable(dscashflow.Tables[1], "id", "description");
                ddlPaymentto_OutFlow.FillDataTable(dscashflow.Tables[2], "id", "description");
                ddlPaymentto_OutFlow.SelectedIndex = 1;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void tbgeneral_ActiveTabChanged(object sender, EventArgs e)
    {
        if (tcFunder.ActiveTabIndex.ToString() == "1")
        {
            if (txtchk.Text != "")
            {
                if (strMode == "C")
                {
                    pnlRswisebreakup.Visible = false;
                    divrswise.Style.Add("display", "none");
                    grvrswise.DataSource = null;
                    grvrswise.DataBind();
                }
                txtchk.Text = "";
                dtRs = (DataTable)Session["dtRs"];
                if (dtRs != null)
                {
                    if (dtRs.Rows.Count > 0)
                    {
                        DataRow[] dtrow = dtRs.Select("status=" + 1 + "");
                        if (dtrow.Length > 0)
                        {
                            dtsource = dtrow.CopyToDataTable();
                        }
                    }
                    Session["dtsource"] = dtsource;
                    if (dtsource != null)
                    {
                        if (dtsource.Rows.Count > 0)
                        {
                            //if (strMode == "C")
                            //{
                            txtLR.Text = txtLRdisc.Text = Convert.ToDecimal(dtsource.Compute("sum(rental1)", "")).ToString(Funsetsuffix());
                            txtAMF.Text = Convert.ToDecimal(dtsource.Compute("sum(AMF1)", "")).ToString(Funsetsuffix());
                            txtTax.Text = Convert.ToDecimal(dtsource.Compute("sum(vat1)", "")).ToString(Funsetsuffix());
                            txtST.Text = Convert.ToDecimal(dtsource.Compute("sum(servicetax1)", "")).ToString(Funsetsuffix());
                            lblLRper.Text = "100%";
                            hdnlrper.Value = "100";
                            if (txtAMF.Text != "0.00")
                            {
                                if (dtsource.Compute("sum(AMF1)", "AMF_Sold=1").ToString() != "")
                                {
                                    txtAMFdisc.Text = Convert.ToDecimal(dtsource.Compute("sum(AMF1)", "AMF_Sold=1")).ToString(Funsetsuffix());
                                    lblAMFPer.Text = ((Convert.ToDecimal(txtAMFdisc.Text.ToString()) / Convert.ToDecimal(txtAMF.Text.ToString())) * 100).ToString("0.00") + "%";
                                    hdnamfper.Value = ((Convert.ToDecimal(txtAMFdisc.Text.ToString()) / Convert.ToDecimal(txtAMF.Text.ToString())) * 100).ToString("0.00000000");
                                }
                                else
                                {
                                    txtAMFdisc.Text = Convert.ToDecimal("0").ToString(Funsetsuffix());
                                    lblAMFPer.Text = "0.00%";
                                    hdnamfper.Value = "0.00";
                                }
                            }
                            else
                            {
                                txtAMFdisc.Text = Convert.ToDecimal("0").ToString(Funsetsuffix());
                                lblAMFPer.Text = "0.00%";
                                hdnamfper.Value = "0.00";
                            }
                            if (txtTax.Text != "0.00")
                            {
                                if (dtsource.Compute("sum(vat1)", "VAT_Sold=1").ToString() != "")
                                {
                                    txtTaxdisc.Text = Convert.ToDecimal(dtsource.Compute("sum(vat1)", "VAT_Sold=1")).ToString(Funsetsuffix());
                                    lblTaxper.Text = ((Convert.ToDecimal(txtTaxdisc.Text.ToString()) / Convert.ToDecimal(txtTax.Text.ToString())) * 100).ToString("0.00") + "%";
                                    hdnvatper.Value = ((Convert.ToDecimal(txtTaxdisc.Text.ToString()) / Convert.ToDecimal(txtTax.Text.ToString())) * 100).ToString("0.00000000");

                                }
                                else
                                {
                                    txtTaxdisc.Text = Convert.ToDecimal("0").ToString(Funsetsuffix());
                                    lblTaxper.Text = "0.00%";
                                    hdnvatper.Value = "0.00";
                                }
                            }
                            else
                            {
                                txtTaxdisc.Text = Convert.ToDecimal("0").ToString(Funsetsuffix());
                                lblTaxper.Text = "0.00%";
                                hdnvatper.Value = "0.00";
                            }
                            if (txtST.Text != "0.00")
                            {
                                if (dtsource.Compute("sum(servicetax1)", "ServiceTax_Sold=1").ToString() != "")
                                {
                                    txtSerDisc.Text = Convert.ToDecimal(dtsource.Compute("sum(servicetax1)", "ServiceTax_Sold=1")).ToString(Funsetsuffix());
                                    lblserdisc.Text = ((Convert.ToDecimal(txtSerDisc.Text.ToString()) / Convert.ToDecimal(txtST.Text.ToString())) * 100).ToString("0.00") + "%";
                                    hdnserper.Value = ((Convert.ToDecimal(txtSerDisc.Text.ToString()) / Convert.ToDecimal(txtST.Text.ToString())) * 100).ToString("0.00000000");

                                }
                                else
                                {
                                    txtSerDisc.Text = Convert.ToDecimal("0").ToString(Funsetsuffix());
                                    lblserdisc.Text = "0.00%";
                                    hdnserper.Value = "0.00";
                                }
                            }
                            else
                            {
                                txtSerDisc.Text = Convert.ToDecimal("0").ToString(Funsetsuffix());
                                lblserdisc.Text = "0.00%";
                                hdnserper.Value = "0.00";
                            }
                            //}
                            //if (strMode != "C")
                            //{
                            //    txtLR.Text = Convert.ToDecimal(dtsource.Compute("sum(rental1)", "")).ToString(Funsetsuffix());
                            //    txtAMF.Text = Convert.ToDecimal(dtsource.Compute("sum(AMF1)", "")).ToString(Funsetsuffix());
                            //    txtTax.Text = Convert.ToDecimal(dtsource.Compute("sum(vat1)", "")).ToString(Funsetsuffix());
                            //    txtST.Text = Convert.ToDecimal(dtsource.Compute("sum(servicetax1)", "")).ToString(Funsetsuffix());
                            //    if (ViewState["ShowDiscValue"] != null)
                            //    {
                            //        if (ViewState["ShowDiscValue"].ToString() == "Yes")
                            //        {
                            //            txtLRdisc.Text = txtLR.Text;
                            //            txtTaxdisc.Text = txtTax.Text;
                            //            txtAMFdisc.Text = txtAMF.Text;
                            //            txtSerDisc.Text = txtST.Text;
                            //        }
                            //    }
                            //    //lblLRper.Text = lblAMFPer.Text = lblTaxper.Text = lblserdisc.Text = "100%";
                            //    hdnlrper.Value = hdnamfper.Value = hdnvatper.Value = hdnserper.Value = "100";
                            //}
                        }
                    }
                    else
                    {
                        txtLR.Text = txtLRdisc.Text = "0.00";
                        txtAMF.Text = txtAMFdisc.Text = "0.00";
                        txtTax.Text = txtTaxdisc.Text = "0.00";
                        txtST.Text = txtSerDisc.Text = "0.00";
                        lblLRper.Text = lblAMFPer.Text = lblTaxper.Text = lblserdisc.Text = "0%";
                        hdnlrper.Value = hdnamfper.Value = hdnvatper.Value = hdnserper.Value = "0";
                    }

                }
            }
        }
        else if (tcFunder.ActiveTabIndex.ToString() == "2")
        {
            hdnValidateSanction.Value = "1";
            btnSave.Enabled = true;
            FunPriLoadAssetCategory();
            if (strMode == "M")
            {
                FunPriLoadSanction();
                funpubmapping();
            }
            if (strMode == "Q")
            {
                FunPriLoadSanction();
                btnSave.Enabled = false;
                btnvalidate.Enabled = false;
            }
        }
        else if (tcFunder.ActiveTabIndex.ToString() == "3" || tcFunder.ActiveTabIndex.ToString() == "4")
        {
            if (strMode == "C" || strMode == "M")
                btnSave.Enabled = true;
        }
    }

    protected void txtLRdisc_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtLRdisc.Text != "")
            {
                if (Convert.ToDecimal(txtLRdisc.Text.ToString()) > Convert.ToDecimal(txtLR.Text.ToString()))
                {
                    Utility.FunShowAlertMsg(this.Page, "Discount Rental amount should not be greater than Rental Amount");
                    txtLRdisc.Text = txtLR.Text;
                    return;
                }
                else
                {
                    lblLRper.Text = ((Convert.ToDecimal(txtLRdisc.Text.ToString()) / Convert.ToDecimal(txtLR.Text.ToString())) * 100).ToString("0.00") + "%";
                    hdnlrper.Value = ((Convert.ToDecimal(txtLRdisc.Text.ToString()) / Convert.ToDecimal(txtLR.Text.ToString())) * 100).ToString("0.00000000");
                }
            }
            else
            {
                txtLRdisc.Text = "0";
                lblLRper.Text = Convert.ToDecimal("0").ToString("0.00") + "%";
                hdnlrper.Value = Convert.ToDecimal("0").ToString("0.00000000");
            }
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void txtTaxdisc_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTaxdisc.Text != "")
            {
                if (Convert.ToDecimal(txtTaxdisc.Text.ToString()) > Convert.ToDecimal(txtTax.Text.ToString()))
                {
                    Utility.FunShowAlertMsg(this.Page, "Discount Tax amount should not be greater than Rental Amount");
                    txtTaxdisc.Text = txtLR.Text;
                    return;
                }
                else
                {
                    lblTaxper.Text = ((Convert.ToDecimal(txtTaxdisc.Text.ToString()) / Convert.ToDecimal(txtTax.Text.ToString())) * 100).ToString("0.00") + "%";
                    hdnvatper.Value = ((Convert.ToDecimal(txtTaxdisc.Text.ToString()) / Convert.ToDecimal(txtTax.Text.ToString())) * 100).ToString("0.00000000");
                }
            }
            else
            {
                txtTaxdisc.Text = "0";
                lblTaxper.Text = Convert.ToDecimal("0").ToString("0.00") + "%";
                hdnvatper.Value = Convert.ToDecimal("0").ToString("0.00000000");
            }
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void txtAMFdisc_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtAMFdisc.Text != "")
            {
                if (Convert.ToDecimal(txtAMFdisc.Text.ToString()) > Convert.ToDecimal(txtAMF.Text.ToString()))
                {
                    Utility.FunShowAlertMsg(this.Page, "Discount AMF amount should not be greater than Rental Amount");
                    txtAMFdisc.Text = txtAMF.Text;
                    return;
                }
                else
                {
                    lblAMFPer.Text = ((Convert.ToDecimal(txtAMFdisc.Text.ToString()) / Convert.ToDecimal(txtAMF.Text.ToString())) * 100).ToString("0.00") + "%";
                    hdnamfper.Value = ((Convert.ToDecimal(txtAMFdisc.Text.ToString()) / Convert.ToDecimal(txtAMF.Text.ToString())) * 100).ToString("0.00000000");
                }
            }
            else
            {
                txtAMFdisc.Text = "0";
                lblAMFPer.Text = Convert.ToDecimal("0").ToString("0.00") + "%";
                hdnamfper.Value = Convert.ToDecimal("0").ToString("0.00000000");
            }
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void txttranchedate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            Session["dtrs"] = null;
            divacc.Style.Add("display", "block");
            pnlcashflow.Visible = false;
            pnlrs.Visible = false;
            div1.Style.Add("display", "block");
            btnValidateRS.Enabled = false;
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void txtfunder_OnTextChanged(object sender, EventArgs e)
    {
        string strhdnValue = hdnfunder.Value;
        if (strhdnValue == "-1" || strhdnValue == "")
        {
            txtfunder.Text = string.Empty;
            hdnfunder.Value = string.Empty;
        }
        grvfund.DataSource = null;
        grvfund.EmptyDataText = "No Details Found";
        grvfund.DataBind();
    }

    protected void txtSerDisc_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtSerDisc.Text != "")
            {
                if (Convert.ToDecimal(txtSerDisc.Text.ToString()) > Convert.ToDecimal(txtST.Text.ToString()))
                {
                    Utility.FunShowAlertMsg(this.Page, "Discount ServiceTax amount should not be greater than Rental Amount");
                    txtSerDisc.Text = txtST.Text;
                    return;
                }
                else
                {
                    lblserdisc.Text = ((Convert.ToDecimal(txtSerDisc.Text.ToString()) / Convert.ToDecimal(txtST.Text.ToString())) * 100).ToString("0.00") + "%";
                    hdnserper.Value = ((Convert.ToDecimal(txtSerDisc.Text.ToString()) / Convert.ToDecimal(txtST.Text.ToString())) * 100).ToString("0.00000000");
                }
            }
            else
            {
                txtSerDisc.Text = "0";
                lblserdisc.Text = Convert.ToDecimal("0").ToString("0.00") + "%";
                hdnserper.Value = Convert.ToDecimal("0").ToString("0.00000000");
            }
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void btnfetch_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunLoadRentalSchedule();
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    private void FunLoadRentalSchedule()
    {
        try
        {
            boolexists = false;
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", (Convert.ToString(strMode) == "Q") ? "2" : "1");
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@customer_id", ddlcust.SelectedValue.ToString());
            Procparam.Add("@tranche_header_id", strtranche_id.ToString());
            Procparam.Add("@tranche_date", (Utility.StringToDate(txttranchedate.Text.ToString())).ToString());
            dtRs = Utility.GetDefaultData("S3G_Fund_GetRS", Procparam);
            Session["dtRs"] = dtRs;
            if (dtRs.Rows.Count > 0)
            {
                divacc.Style.Add("display", "block");
                pnlcashflow.Visible = pnlrs.Visible = true;
                div1.Style.Add("display", "block");
                grvRental.DataSource = dtRs;
                grvRental.DataBind();
                if (strMode == "C")
                    btnValidateRS.Enabled = true;
            }
            else
            {
                divacc.Style.Add("display", "block");
                pnlcashflow.Visible = pnlrs.Visible = true;
                div1.Style.Add("display", "block");
                grvRental.EmptyDataText = "No Rental schedules found";
                grvRental.DataBind();
                btnValidateRS.Enabled = false;
            }
            //div1.Style.Add("display", "block");
            //divacc.Style.Add("display", "block");
            //pnlcashflow.Visible = true;
            FunPriSetEmptyCashtbl();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriLoadAssetCategory()
    {
        try
        {
            decimal temprental;
            decimal temprental_source;
            decimal tempVAT;
            decimal tempVAT_Source;
            decimal tempAMF;
            decimal tempAMF_Source;
            decimal tempServiceTax;
            decimal tempServiceTax_source;

            //dtFinal = new DataTable();
            //dtFinal.Columns.Add("RS_Number");
            //dtFinal.Columns.Add("PA_SA_REF_ID");
            //dtFinal.Columns.Add("installment_no");
            //dtFinal.Columns.Add("Rental_disc");
            //dtFinal.Columns.Add("Rental_source");
            //dtFinal.Columns.Add("RS_TYPE");
            //dtFinal.Columns.Add("AMF_source");
            //dtFinal.Columns.Add("AMF_disc");
            //dtFinal.Columns.Add("rental_tax_disc");
            //dtFinal.Columns.Add("Rental_tax_source");
            //dtFinal.Columns.Add("Service_Tax_disc");
            //dtFinal.Columns.Add("Service_Tax_source");

            //foreach (GridViewRow gvrow in grvrswise.Rows)
            //{
            //    if (gvrow.RowType == DataControlRowType.DataRow)
            //    {
            //        if (((TextBox)gvrow.Cells[0].FindControl("lblrentaldisc")).Text.ToString() != "")
            //            temprental = Convert.ToDecimal(((TextBox)gvrow.Cells[0].FindControl("lblrentaldisc")).Text.ToString());
            //        else
            //            temprental = Convert.ToDecimal("0.00");
            //        if (((TextBox)gvrow.Cells[0].FindControl("lblVATdisc")).Text.ToString() != "")
            //            tempVAT = Convert.ToDecimal(((TextBox)gvrow.Cells[0].FindControl("lblVATdisc")).Text.ToString());
            //        else
            //            tempVAT = Convert.ToDecimal("0.00");
            //        if (((TextBox)gvrow.Cells[0].FindControl("lblAMFdisc")).Text.ToString() != "")
            //            tempAMF = Convert.ToDecimal(((TextBox)gvrow.Cells[0].FindControl("lblAMFdisc")).Text.ToString());
            //        else
            //            tempAMF = Convert.ToDecimal("0.00");
            //        if (((TextBox)gvrow.Cells[0].FindControl("lblServiceTaxdisc")).Text.ToString() != "")
            //            tempServiceTax = Convert.ToDecimal(((TextBox)gvrow.Cells[0].FindControl("lblServiceTaxdisc")).Text.ToString());
            //        else
            //            tempServiceTax = Convert.ToDecimal("0.00");
            //        temprental_source = Convert.ToDecimal(((Label)gvrow.Cells[0].FindControl("lblrental")).Text.ToString());
            //        tempVAT_Source = Convert.ToDecimal(((Label)gvrow.Cells[0].FindControl("lblVAT")).Text.ToString());
            //        tempAMF_Source = Convert.ToDecimal(((Label)gvrow.Cells[0].FindControl("lblAMF")).Text.ToString());
            //        tempServiceTax_source = Convert.ToDecimal(((Label)gvrow.Cells[0].FindControl("lblServiceTax")).Text.ToString());

            //        DataRow dr = dtFinal.NewRow();
            //        dr["Rs_Number"] = ((Label)gvrow.Cells[0].FindControl("lblRS_Number")).Text.ToString();
            //        dr["PA_SA_REF_ID"] = ((Label)gvrow.Cells[0].FindControl("lblPA_SA_REF_ID")).Text.ToString();
            //        dr["RS_TYPE"] = ((Label)gvrow.Cells[0].FindControl("lblRS_Type")).Text.ToString();
            //        dr["Installment_no"] = ((Label)gvrow.Cells[0].FindControl("lblinstallmentno")).Text.ToString();
            //        dr["Rental_disc"] = ((TextBox)gvrow.Cells[0].FindControl("lblrentaldisc")).Text.ToString();
            //        dr["AMF_disc"] = ((TextBox)gvrow.Cells[0].FindControl("lblAMFdisc")).Text.ToString();
            //        dr["rental_tax_disc"] = ((TextBox)gvrow.Cells[0].FindControl("lblVATdisc")).Text.ToString();
            //        dr["Service_Tax_disc"] = ((TextBox)gvrow.Cells[0].FindControl("lblServiceTaxdisc")).Text.ToString();
            //        dr["Rental_source"] = ((Label)gvrow.Cells[0].FindControl("lblrental")).Text.ToString();
            //        dr["AMF_source"] = ((Label)gvrow.Cells[0].FindControl("lblAMF")).Text.ToString();
            //        dr["Rental_tax_source"] = ((Label)gvrow.Cells[0].FindControl("lblVAT")).Text.ToString();
            //        dr["Service_Tax_source"] = ((Label)gvrow.Cells[0].FindControl("lblServiceTax")).Text.ToString();

            //        dtFinal.Rows.Add(dr);
            //    }
            //}
            //Session["dtFinal"] = dtFinal;
            dttarget = (DataTable)Session["dttarget"];
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@Acc_XMl", dttarget.FunPubFormXml());
            dsfunder = Utility.GetDataset("S3G_Fund_GetDetails", Procparam);
            Session["dtfundsave"] = dsfunder.Tables[0];
            Session["dsasset"] = dsfunder.Tables[1];
            divfund.Style.Add("display", "block");
            if (dsfunder.Tables[2].Rows.Count > 0)
            {
                pnlasset.Visible = true;
                divasset.Style.Add("display", "block");
                grvasset.DataSource = dsfunder.Tables[2];
                Session["assettotal"] = dsfunder.Tables[2];
                grvasset.DataBind();
                FunPriFindAssetTotal();
            }
            else
            {
                pnlasset.Visible = true;
                divasset.Style.Add("display", "block");
                grvasset.EmptyDataText = "No Details Found";
                grvasset.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadSanction()
    {
        try
        {
            dsasset = (DataTable)Session["dsasset"];
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            if (txtfunder.Text != string.Empty)
                Procparam.Add("@Funder_id", hdnfunder.Value);
            Procparam.Add("@customer_id", ddlcust.SelectedValue.ToString());
            Procparam.Add("@Asset_XMl", dsasset.FunPubFormXml());
            Procparam.Add("@tranche_date", (Utility.StringToDate(txttranchedate.Text.ToString())).ToString());
            Procparam.Add("@strmode", strMode.ToString());
            dsfunder = Utility.GetDataset("S3G_Fund_GetSanction", Procparam);
            divfund.Style.Add("display", "block");
            if (dsfunder.Tables[0].Rows.Count > 0)
            {
                pnlfunder.Visible = true;
                divfund.Style.Add("display", "block");
                Session["dtsanction"] = dsfunder.Tables[0];
                grvfund.DataSource = dsfunder.Tables[0];
                grvfund.DataBind();
                btnvalidate.Enabled = true;
            }
            else
            {
                pnlfunder.Visible = true;
                divfund.Style.Add("display", "block");
                grvfund.EmptyDataText = "No Details Found";
                grvfund.DataBind();
                btnvalidate.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadSanction();
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }
    protected void btnProcess_OnClick(object sender, EventArgs e)
    {
        try
        {


            FunPriLoadGrid();
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void ddllRSnumber_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            if (txtcheck.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Update the changed values");
                return;
            }
            dttarget = (DataTable)Session["dttarget"];
            string panum = ddllRSnumber.SelectedItem.Text.ToString();
            DataRow[] dtrow = dttarget.Select("RS_Number='" + panum + "'");
            if (dtrow.Length > 0)
            {
                DataTable dt = dtrow.CopyToDataTable();
                grvrswise.DataSource = dt;
                grvrswise.DataBind();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dtRecords = new DataTable();
            dtRecords.Columns.Add("RS_Number");
            dtRecords.Columns.Add("Installment_No");
            dtRecords.Columns["Installment_No"].DataType = typeof(Int32);
            dtRecords.Columns.Add("Rental_disc");
            dtRecords.Columns.Add("Rental_Tax_disc");
            dtRecords.Columns.Add("AMF_disc");
            dtRecords.Columns.Add("Service_Tax_disc");
            foreach (GridViewRow grv in grvrswise.Rows)
            {
                DataRow dr = dtRecords.NewRow();
                Label lblrs = grv.FindControl("lblRS_Number") as Label;
                Label lblInstallment_No = grv.FindControl("lblinstallmentno") as Label;
                TextBox txtRentaldisc = grv.FindControl("lblrentaldisc") as TextBox;
                TextBox txtAMFdisc = grv.FindControl("lblAMFdisc") as TextBox;
                TextBox txtServiceTaxdisc = grv.FindControl("lblServiceTaxdisc") as TextBox;
                TextBox txtlVATdiscc = grv.FindControl("lblVATdisc") as TextBox;
                dr["RS_Number"] = lblrs.Text.ToString();
                dr["Installment_No"] = lblInstallment_No.Text.ToString();
                
                if (txtRentaldisc.Text != "")
                    dr["Rental_disc"] = txtRentaldisc.Text.ToString();
                else
                    dr["Rental_disc"] = "0";

                if (txtlVATdiscc.Text != "")
                    dr["Rental_Tax_disc"] = txtlVATdiscc.Text.ToString();
                else
                    dr["Rental_Tax_disc"] = "0";

                if (txtAMFdisc.Text != "")
                    dr["AMF_disc"] = txtAMFdisc.Text.ToString();
                else
                    dr["AMF_disc"] = "0";

                if (txtServiceTaxdisc.Text != "")
                    dr["Service_Tax_disc"] = txtServiceTaxdisc.Text.ToString();
                else
                    dr["Service_Tax_disc"] = "0";

                dtRecords.Rows.Add(dr);
                dtRecords.AcceptChanges();

            }
            Session["dtRecords"] = dtRecords;
            dttarget = (DataTable)Session["dttarget"];
            dttarget.Rows.Cast<DataRow>().Join(dtRecords.Rows.Cast<DataRow>(),
                 r1 => new { p1 = r1["RS_Number"], p2 = r1["Installment_No"] },
                 r2 => new { p1 = r2["RS_Number"], p2 = r2["Installment_No"] },
                 (r1, r2) => new { r1, r2 }).ToList()
                 .ForEach(
                 o =>
                 {
                     o.r1.SetField("Rental_disc", o.r2["Rental_disc"]);
                     o.r1.SetField("Rental_Tax_disc", o.r2["Rental_Tax_disc"]);
                     o.r1.SetField("AMF_disc", o.r2["AMF_disc"]);
                     o.r1.SetField("Service_Tax_disc", o.r2["Service_Tax_disc"]);
                     o.r1.SetField("Rental_disc1", o.r2["Rental_disc"]);
                     o.r1.SetField("Rental_Tax_disc1", o.r2["Rental_Tax_disc"]);
                     o.r1.SetField("AMF_disc1", o.r2["AMF_disc"]);
                     o.r1.SetField("Service_Tax_disc1", o.r2["Service_Tax_disc"]);
                 }

                 );
            dttarget.AcceptChanges();
            Session["dttarget"] = dttarget;
            txtcheck.Text = "0";
            FunPriFindTotal();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void FunPriLoadGrid()
    {
        try
        {
            //if (grvrswise.Rows.Count > 0)
            //{
            //   // btnProcess.Attributes.Add("onclick", "return ConfirmOnDelete('');");
            //    btnProcess.Attributes.Add("onclick", "javascript:return ConfirmOnDelete('')");
            //    //Utility.FunShowAlertMsg(this.Page, "Existing schedule gets affected");
            //}
            hdnValidateSanction.Value = "0";
            dtRs = (DataTable)Session["dtRs"];
            if (dtRs != null)
            {
                if (dtRs.Rows.Count > 0)
                {
                    DataRow[] dtrow = dtRs.Select("status=" + 1 + "");
                    if (dtrow.Length > 0)
                    {
                        dtsource = dtrow.CopyToDataTable();
                    }
                }
            }
            if (Session["dtsource"] != null)
                dtsource = (DataTable)Session["dtsource"];
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_id", intCompanyID.ToString());
            Procparam.Add("@user_id", intCompanyID.ToString());
            Procparam.Add("@RS_xml", dtsource.FunPubFormXml());
            Procparam.Add("@LR_per", hdnlrper.Value.ToString());
            Procparam.Add("@VAT_per", hdnvatper.Value.ToString());
            Procparam.Add("@AMF_per", hdnamfper.Value.ToString());
            Procparam.Add("@SER_per", hdnserper.Value.ToString());
            Procparam.Add("@tranche_date", (Utility.StringToDate(txttranchedate.Text.ToString())).ToString());
            dsprocess = Utility.GetDataset("S3G_FUNDMGT_Get_TrancheAmort", Procparam);
            dttarget = dsprocess.Tables[0];
            ddllRSnumber.BindDataTable(dsprocess.Tables[1]);
            ddllRSnumber.SelectedIndex = 1;
            pnlRswisebreakup.Visible = true;
            divrswise.Style.Add("display", "block");

            grvrswise.DataSource = dsprocess.Tables[2];
            grvrswise.DataBind();
            Session["dttarget"] = dttarget;
            //FunPriFindTotal();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void chkSelectAccount_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtchk.Text = "0";
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            Label lblRS_Number = (Label)gvr.FindControl("lblRS_Number");
            Label lblRsType = (Label)gvr.FindControl("lblRsType");
            CheckBox chkAll = (CheckBox)grvRental.HeaderRow.FindControl("chkAll");
            string panum = lblRS_Number.Text.ToString();
            string RS_Type = lblRsType.Text.ToString();
            dtRs = (DataTable)Session["dtRs"];
            DataRow[] dtrow = dtRs.Select("RS_Number='" + panum + "' and RS_Type='" + RS_Type + "'");

            DataTable dt = dtrow.CopyToDataTable();
            dt.Columns.Add("Status");
            if (dt.Rows.Count > 0)
            {
                if (btn.Checked)
                {
                    //if (Session["cnt"].ToString() == "0")
                    //{
                    //    due_date = dt.Rows[0]["dayss"].ToString();
                    //    Session["due_date"] = due_date;
                    //    Tenure = dt.Rows[0]["Tenure"].ToString();
                    //    Session["Tenure"] = Tenure;
                    //    frequency = dt.Rows[0]["frequency"].ToString();
                    //    Session["frequency"] = frequency;

                    //}
                    //else
                    //{

                    //    if (dt.Rows[0]["dayss"].ToString() != Session["due_date"].ToString())
                    //    {
                    //        Utility.FunShowAlertMsg(this.Page, "The selected Rental Schedule should have same Due_Date");
                    //        btn.Checked = false;
                    //        return;
                    //    }
                    //    if (dt.Rows[0]["Tenure"].ToString() != Session["Tenure"].ToString())
                    //    {
                    //        Utility.FunShowAlertMsg(this.Page, "The selected Rental Schedule should have same Tenure");
                    //        btn.Checked = false;
                    //        return;
                    //    }
                    //    if (dt.Rows[0]["frequency"].ToString() != Session["frequency"].ToString())
                    //    {
                    //        Utility.FunShowAlertMsg(this.Page, "The selected Rental Schedule should have same Frequency");
                    //        btn.Checked = false;
                    //        return;
                    //    }

                    //}
                    //boolexists = true;
                    //d_cnt = d_cnt + 1;
                    //Session["cnt"] = d_cnt;
                    dtrow[0]["status"] = "1";
                    dtRs.AcceptChanges();
                    Session["dtRs"] = dtRs;
                    TabPanel1.Enabled = false;
                    TabPanel2.Enabled = false;
                    TabAlerts.Enabled = false;
                    TabFollowUp.Enabled = false;
                    btnValidateRS.Enabled = true;

                    DataRow[] rw = dtRs.Select("status=" + 0 + "");

                    if (rw.Length == 0)
                        chkAll.Checked = true;
                }
                else
                {
                    d_cnt = d_cnt - 1;
                    Session["cnt"] = d_cnt;
                    dtrow[0]["status"] = "0";
                    dtRs.AcceptChanges();
                    Session["dtRs"] = dtRs;
                    TabPanel1.Enabled = false;
                    TabPanel2.Enabled = false;
                    TabAlerts.Enabled = false;
                    TabFollowUp.Enabled = false;
                    btnValidateRS.Enabled = true;
                    chkAll.Checked = false;
                }

                DataRow[] dtrow1 = dtRs.Select("status=" + 1 + "");
                if (dtrow1.Length.ToString() == "0")
                {
                    Session["due_date"] = null;
                    Session["Tenure"] = null;
                    Session["cnt"] = "0";
                    Session["frequency"] = null;
                }
            }
            else
            {
            }
            if (strMode != "C")
            {
                pnlRswisebreakup.Visible = false;
                divrswise.Style.Add("display", "none");
                grvrswise.DataSource = null;
                grvrswise.DataBind();
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    //code added by vinodha to handle the impacts during the check all and uncheck all
    protected void chkAll_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox btn = (CheckBox)sender;
            if (btn.Checked)
            {
                dtRs = (DataTable)Session["dtRs"];
                foreach (DataRow dr in dtRs.Rows)
                {
                    dr["status"] = "1";
                }
                dtRs.AcceptChanges();
                Session["dtRs"] = dtRs;
                TabPanel1.Enabled = false;
                TabPanel2.Enabled = false;
                TabAlerts.Enabled = false;
                TabFollowUp.Enabled = false;
                btnValidateRS.Enabled = true;
            }
            else
            {
                dtRs = (DataTable)Session["dtRs"];
                foreach (DataRow dr in dtRs.Rows)
                {
                    dr["status"] = "0";
                }
                dtRs.AcceptChanges();
                Session["dtRs"] = dtRs;
                TabPanel1.Enabled = false;
                TabPanel2.Enabled = false;
                TabAlerts.Enabled = false;
                TabFollowUp.Enabled = false;
                btnValidateRS.Enabled = true;

                Session["due_date"] = null;
                Session["Tenure"] = null;
                Session["cnt"] = "0";
                Session["frequency"] = null;
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void chkAccount_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            btnSave.Enabled = false;
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            Label lblassetid = (Label)gvr.FindControl("lblassetid");
            Label lblSanctionid = (Label)gvr.FindControl("lblSanctionid");
            Label lblSanctionumber = (Label)gvr.FindControl("lblSanctionumber");
            string Sanctionumber = lblSanctionumber.Text.ToString();
            string assetid = lblassetid.Text.ToString();
            string sanction_id = lblSanctionid.Text.ToString();
            dsasset = (DataTable)Session["dsasset"];
            foreach (DataRow dr1 in dsasset.Rows)
            {
                dr1["sanction_id"] = "0";
            }
            if (btn.Checked)
            {
                foreach (GridViewRow gvrow in grvfund.Rows)
                {
                    if (gvrow.RowType == DataControlRowType.DataRow)
                    {
                        if (((Label)gvrow.Cells[0].FindControl("lblSanctionumber")).Text.ToString() == Sanctionumber.ToString())
                        {
                            ((CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount")).Checked = true;
                        }
                        else
                        {
                            ((CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount")).Enabled = false;
                        }
                    }
                }
                dtsanction = (DataTable)Session["dtsanction"];
                DataRow[] dtrow = dtsanction.Select("asset_category_id='" + assetid + "' and Sanctioned_No='" + Sanctionumber + "'");
                DataTable dt = dtrow.CopyToDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    dtrow[0]["status"] = "1";
                    dtsanction.AcceptChanges();
                    Session["dtsanction"] = dtsanction;
                }
            }
            else
            {
                foreach (GridViewRow gvrow in grvfund.Rows)
                {
                    if (gvrow.RowType == DataControlRowType.DataRow)
                    {
                        ((CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount")).Enabled = true;
                        ((CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount")).Checked = false;
                    }
                }
                dtsanction = (DataTable)Session["dtsanction"];
                foreach (DataRow dr in dtsanction.Rows)
                {
                    dr["status"] = "0";
                    dtsanction.AcceptChanges();
                    Session["dtsanction"] = dtsanction;
                }
            }

            dtsanction = (DataTable)Session["dtsanction"];
        }

        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void chkconsolidated_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkconsolidated.Checked)
            {
                pnltotal.Enabled = true;
                hdnlrper.Value = hdnamfper.Value = hdnvatper.Value = hdnserper.Value = "100";
                lblTaxper.Text = lblAMFPer.Text = lblLRper.Text = lblserdisc.Text = Convert.ToDecimal("100").ToString("0.00") + "%";
                txtLRdisc.Text = txtLR.Text;
                txtAMFdisc.Text = txtAMF.Text;
                txtTaxdisc.Text = txtTax.Text;
                txtSerDisc.Text = txtST.Text;
                FunPriLoadGrid();
            }
            else
            {
                pnltotal.Enabled = false;
                hdnlrper.Value = hdnamfper.Value = hdnvatper.Value = hdnserper.Value = "100";
                lblTaxper.Text = lblAMFPer.Text = lblLRper.Text = lblserdisc.Text = Convert.ToDecimal("100").ToString("0.00") + "%";
                txtLRdisc.Text = txtLR.Text;
                txtAMFdisc.Text = txtAMF.Text;
                txtTaxdisc.Text = txtTax.Text;
                txtSerDisc.Text = txtST.Text;
                FunPriLoadGrid();
            }
        }

        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }


    protected void gvOutFlow_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataRow drEmptyRow;

            if (e.CommandName == "Add")
            {
                TextBox txtDate_GridOutflow = (TextBox)gvOutFlow.FooterRow.FindControl("txtDate_GridOutflow");
                TextBox txtAmount_Outflow = (TextBox)gvOutFlow.FooterRow.FindControl("txtAmount_Outflow");
                DropDownList ddloutflowtype = (DropDownList)gvOutFlow.FooterRow.FindControl("ddloutflowtype");
                DropDownList ddlOutflowDesc = (DropDownList)gvOutFlow.FooterRow.FindControl("ddlOutflowDesc");
                DropDownList ddlPaymentto_OutFlow = (DropDownList)gvOutFlow.FooterRow.FindControl("ddlPaymentto_OutFlow");
                UserControls_S3GAutoSuggest ddlEntityName_OutFlow = (UserControls_S3GAutoSuggest)gvOutFlow.FooterRow.FindControl("ddlEntityName_OutFlow");

                dtcashflow = (DataTable)Session["dtcashflow"];

                if (dtcashflow.Rows.Count == 1)
                {
                    if (dtcashflow.Rows[0]["CashFlow_Type_id"].ToString() == string.Empty)
                        dtcashflow.Rows[0].Delete();
                }

                drEmptyRow = dtcashflow.NewRow();

                if (ddloutflowtype.SelectedIndex > 0)
                {
                    drEmptyRow["CashFlow_Type_id"] = ddloutflowtype.SelectedValue;
                    drEmptyRow["CashFlow_Type"] = ddloutflowtype.SelectedItem.Text;
                }
                if (ddlOutflowDesc.SelectedIndex > 0)
                {
                    drEmptyRow["CashOutFlow"] = ddlOutflowDesc.SelectedItem.Text;
                    drEmptyRow["CashOutFlow_id"] = ddlOutflowDesc.SelectedValue;
                }

                if (ddlPaymentto_OutFlow.SelectedIndex > 0)
                {
                    drEmptyRow["OutflowFrom"] = ddlPaymentto_OutFlow.SelectedItem.Text;
                    drEmptyRow["OutflowFrom_id"] = ddlPaymentto_OutFlow.Text;
                }

                drEmptyRow["Entity"] = ddlEntityName_OutFlow.SelectedText;
                drEmptyRow["Entity_id"] = ddlEntityName_OutFlow.SelectedValue;

                drEmptyRow["Amount"] = txtAmount_Outflow.Text.ToString();
                drEmptyRow["Date"] = txtDate_GridOutflow.Text.ToString();

                dtcashflow.Rows.Add(drEmptyRow);
                Session["dtcashflow"] = dtcashflow;
                gvOutFlow.DataSource = dtcashflow;
                gvOutFlow.DataBind();
            }
        }

        catch (Exception)
        {
            throw;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (grvrswise.Rows.Count < 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Rental Schedules should be processed");
                return;
            }

            if (Session["dtfundsave"] != null)
            {
                DataTable dt = (DataTable)Session["dtfundsave"];
                if (hdnValidateSanction.Value == "0" && dt.Rows.Count != 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Sanction Details should be validated");
                    return;
                }
            }

            dttarget = (DataTable)Session["dttarget"];
            foreach (DataRow dr in dttarget.Rows)
            {
                if (dr["Rental_disc"].ToString() != "")
                {
                    if (Convert.ToDecimal(dr["Rental_disc"].ToString()) > Convert.ToDecimal(dr["Rental_source"].ToString()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Target Rental Amount Exceeds the Source Rental Amount");
                        return;
                    }
                }
                if (dr["AMF_disc"].ToString() != "")
                {
                    if (Convert.ToDecimal(dr["AMF_disc"].ToString()) > Convert.ToDecimal(dr["AMF_source"].ToString()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Target AMF Amount Exceeds the Source AMF Amount");
                        return;
                    }
                }
                if (dr["rental_tax_disc"].ToString() != "")
                {
                    if (Convert.ToDecimal(dr["rental_tax_disc"].ToString()) > Convert.ToDecimal(dr["Rental_tax_source"].ToString()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Target VAT Amount Exceeds the Source VAT Amount");
                        return;
                    }
                }
                if (dr["Service_Tax_disc"].ToString() != "")
                {
                    if (Convert.ToDecimal(dr["Service_Tax_disc"].ToString()) > Convert.ToDecimal(dr["Service_Tax_source"].ToString()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Target ServiceTax Amount Exceeds the Source ServiceTax Amount");
                        return;
                    }
                }
            }

            // Code added for Call Id : 6298 
            if (ddlcust.SelectedValue == "0" || ddlcust.SelectedValue == null)
            {
                Utility.FunShowAlertMsg(this.Page, "Select Lessee Name");
                return;
            }



            objFunderDatatable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_Tranche_CreationDataTable();
            objFunderRow = objFunderDatatable.NewS3G_Tranche_CreationRow();

            objFunderRow.Company_id = intCompanyID.ToString();
            objFunderRow.Created_by = intUserID.ToString();
            objFunderRow.Created_On = DateTime.Now.ToString();
            objFunderRow.Tranche_Header_id = strtranche_id;

            if (txtfunder.Text != string.Empty)
                objFunderRow.Funder_Id = hdnfunder.Value;
            else
                objFunderRow.Funder_Id = "0";

            objFunderRow.Customer_id = ddlcust.SelectedValue;

            objFunderRow.Tranche_name = txttranche.Text;
            objFunderRow.Tranche_date = (Utility.StringToDate(txttranchedate.Text.ToString())).ToString();
            objFunderRow.Status_ID = txtstatus.Text.ToString();
            objFunderRow.Disbursement_Date = (Utility.StringToDate(DateTime.Now.ToString())).ToString();
            objFunderRow.Tenure = Session["Tenure"].ToString();

            objFunderRow.Invoice_Cov_Letter = Convert.ToInt32(ddlInvCovLetter.SelectedValue);

            if (Session["dtcashflow"] != null && ((DataTable)Session["dtcashflow"]).Rows.Count > 0)
            {
                objFunderRow.xmlCashflow = Utility.FunPubFormXml((DataTable)Session["dtcashflow"]);
            }
            else
            {
                objFunderRow.xmlCashflow = null;
            }

            if (Session["dttranche"] != null && ((DataTable)Session["dttranche"]).Rows.Count > 0)
            {
                objFunderRow.xmlTrancheDetailArcheive = Utility.FunPubFormXml((DataTable)Session["dttranche"], true);
            }
            else
            {
                objFunderRow.xmlTrancheDetailArcheive = null;
            }
            if (Session["dtfundsave"] != null && ((DataTable)Session["dtfundsave"]).Rows.Count > 0)
            {
                objFunderRow.xmlTrancheDetail = Utility.FunPubFormXml((DataTable)Session["dtfundsave"], true);
            }
            else
            {
                objFunderRow.xmlTrancheDetail = null;
            }
            if (Session["dttarget"] != null && ((DataTable)Session["dttarget"]).Rows.Count > 0)
            {
                objFunderRow.xmlTrancheamort = Utility.FunPubFormXml((DataTable)Session["dttarget"], true);
            }
            else
            {
                objFunderRow.xmlTrancheamort = null;
            }
            #region ALERT
            XMLCommon = ((System.Data.DataTable)Session["DtAlertDetails"]).FunPubFormXml(true);
            objFunderRow.XmlAlertDetails = XMLCommon;
            #endregion

            #region ALERT
            objFunderRow.XmlCustEmailDetails = FunCustEmailBuilder();
            #endregion

            if(intCustEmailRows>1)
            {
                Utility.FunShowAlertMsg(this.Page, "Selected E-Mail Alert cannot be more than one");
                return;
            }
           
            #region Followup
            objFunderRow.XmlFollowupDetail = ((System.Data.DataTable)Session["DtFollowUp"]).FunPubFormXml(true);
            #endregion

            objFunderDatatable.AddS3G_Tranche_CreationRow(objFunderRow);

            objFundMgtServiceClient = new FunderMgtServiceReference.FundMgtServiceClient();
            string strFunderCode, strErrorMsg = string.Empty;
            Int64 intFunderNo = 0;


            int iErrorCode = objFundMgtServiceClient.FunPubCreateOrModifyTrancheMaster(out intTrancheId, out strErrorMsg, ObjSerMode, ClsPubSerialize.Serialize(objFunderDatatable, ObjSerMode));
            switch (iErrorCode)
            {
                case 0:
                    btnSave.Enabled = false;
                    if (strtranche_id == 0)
                    {
                        strAlert = "Tranche Creation Created successfully";
                        strAlert += @"\n\nWould you like to Create one more Tranche Creation?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPage = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        lblErrorMessage.Text = string.Empty;
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Tranche Creation Details updated successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                        btnSave.Enabled = false;
                    }
                    break;
                case 5:
                    Utility.FunShowAlertMsg(this, "Tranche Name already Exist");
                    break;
                case 50:
                    Utility.FunShowAlertMsg(this, "Error in Saving Details");
                    break;
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void btnvalidate_Click(object sender, EventArgs e)
    {
        try
        {
            funpubmapping();
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void btnValidateRS_Click(object sender, EventArgs e)
    {
        try
        {
            btnValidateRS.Enabled = false;
            dtRs = (DataTable)Session["dtRs"];
            DataRow[] dtrow = dtRs.Select("status='" + 1 + "'");
            if (dtrow.Length == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select Rental Schedule Number");
                return;
            }
            DataTable dt = dtrow.CopyToDataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@Acc_XMl", dt.FunPubFormXml());
            dtcheckRs = Utility.GetDefaultData("S3G_Fund_RSCheck", Procparam);
            if (dtcheckRs.Rows.Count > 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Seleted Rental schedules doesnot match");
                TabPanel1.Enabled = false;
                TabPanel2.Enabled = false;
                TabAlerts.Enabled = false;
                TabFollowUp.Enabled = false;
                return;
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "RS Validation Successfully Completed");
                TabPanel1.Enabled = true;
                TabPanel2.Enabled = true;
                TabAlerts.Enabled = true;
                TabFollowUp.Enabled = true;
                Session["Tenure"] = dtcheckRs.Rows[0]["Tenure"].ToString();
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void ddlcust_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcust.SelectedValue != "0")
                FunProLoadStateCombos();
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

     protected void FunProLoadStateCombos()
     {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Customer_ID", Convert.ToString(ddlcust.SelectedValue));
            DataSet dtState = Utility.GetDataset("S3G_Fund_GetInvoiceCovering", Procparam);
            Utility.FillDataTable(ddlInvCovLetter, dtState.Tables[0], "Cust_Address_ID", "State_Name", false);

            if (dtState.Tables[1].Rows[0]["Invoice_Cov_Letter"].ToString() != "0")
                ddlInvCovLetter.SelectedValue = dtState.Tables[1].Rows[0]["Invoice_Cov_Letter"].ToString();
            //opc042 start
            grvCustEmail.DataSource = dtState.Tables[2];
            grvCustEmail.DataBind();
            //opc042 end
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void funpubmapping()
    {
        try
        {
            int i;
            Int32 Sanction_ID = 0;
            string pasaref_id;
            string asset_id;
            decimal rental;
            decimal AMF;
            decimal VAT;
            decimal ServiceTax;
            decimal total;
            decimal sanction;
            string RS_No;
            DataTable dtvalidate = new DataTable();
            dtvalidate.Columns.Add("RS_Number");
            dtvalidate.Columns.Add("rental");
            dtvalidate.Columns.Add("vat");
            dtvalidate.Columns.Add("AMF");
            dtvalidate.Columns.Add("servicetax");

            string strexpression = " Rental Schedules ==> ";
            DataTable dnew = new DataTable();
            dtsanction = (DataTable)Session["dtsanction"];

            dsasset = (DataTable)Session["dsasset"];

            //sdsasset.Columns.Add("sanction_id");
            if (dtsanction != null)
            {
                foreach (DataRow dr in dsasset.Rows)
                {
                    pasaref_id = dr["id"].ToString();
                    asset_id = dr["asset_id"].ToString();
                    RS_No = dr["RS_Number"].ToString();
                    rental = Convert.ToDecimal(dr["rental"].ToString());
                    AMF = Convert.ToDecimal(dr["AMF"].ToString());
                    VAT = Convert.ToDecimal(dr["VAT"].ToString());
                    ServiceTax = Convert.ToDecimal(dr["ServiceTax"].ToString());
                    total = rental + AMF + ServiceTax + VAT;
                    DataRow[] dr1 = dtsanction.Select("asset_category_id in (" + asset_id + ",0) and status=1");
                    dnew = new DataTable();
                    if (dr1.Length > 0)
                    {
                        dnew = dr1.CopyToDataTable();
                        Sanction_ID = Convert.ToInt32(dr1[0]["sanction_id"]);
                    }
                    if (dnew.Rows.Count > 0)
                    {
                        foreach (DataRow drrow in dnew.Rows)
                        {
                            sanction = Convert.ToDecimal(drrow["available_limit"].ToString());
                            if (sanction > total)
                            {
                                dr["sanction_id"] = drrow["sanction_id"].ToString();
                                dr1[0]["available_limit"] = sanction - total;
                                dtsanction.AcceptChanges();
                                break;
                            }
                        }
                    }
                    if (dr["sanction_id"].ToString() == "0")
                    {
                        DataRow drvalidaterow;
                        drvalidaterow = dtvalidate.NewRow();
                        drvalidaterow["RS_Number"] = RS_No;
                        drvalidaterow["rental"] = rental;
                        drvalidaterow["AMF"] = AMF;
                        drvalidaterow["VAT"] = VAT;
                        drvalidaterow["servicetax"] = ServiceTax;
                        dtvalidate.Rows.Add(drvalidaterow);
                    }
                }
                if (dtvalidate.Rows.Count > 0)
                {
                    pnluntagged.Visible = true;
                    divuntagged.Style.Add("display", "block");
                    grvuntagged.DataSource = dtvalidate;
                    grvuntagged.DataBind();
                }
                else
                {
                    pnluntagged.Visible = true;
                    divuntagged.Style.Add("display", "block");
                    grvuntagged.EmptyDataText = "All Rental schedules are tagged";
                    grvuntagged.DataBind();
                }
                dtfundsave = (DataTable)Session["dtfundsave"];

                dtfundsave.Rows.Cast<DataRow>().Join(dsasset.Rows.Cast<DataRow>(),
                 r1 => new { p1 = r1["asset_id"], p2 = r1["id"] },
                 r2 => new { p1 = r2["asset_id"], p2 = r2["id"] },
                 (r1, r2) => new { r1, r2 }).ToList()
                 .ForEach(o => o.r1.SetField("sanction_id", o.r2["sanction_id"]));

                foreach (DataRow drrow in dtfundsave.Rows)
                {
                    drrow["sanction_id"] = Convert.ToString(Sanction_ID);
                    dtfundsave.AcceptChanges();
                }

                Session["dtfundsave"] = dtfundsave;
                btnSave.Enabled = true;
            }
            btnSave.Enabled = true;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }
    [System.Web.Services.WebMethod]
    public static string[] GetCustList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@OPTION", "1");
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@OPTION", "4");
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_CommonList_AGT", Procparam));

        return suggetions.ToArray();
    }

    private void FunPriLoadTrancheDtls()
    {
        try
        {
            hdnValidateSanction.Value = "1";
            FunPriSetEmptyCashtbl();
            pnlcashflow.Visible = true;
            div1.Style.Add("display", "block");
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Tranche_ID", Convert.ToString(strtranche_id));
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@USER_ID", Convert.ToString(intUserID));
            Procparam.Add("@STRMODE", strMode.ToString());

            DataSet dsFunder = Utility.GetDataset("S3G_FUNDMGT_GETTranche", Procparam);
            Session["modify"] = dsFunder.Tables[3];
            Session["dtfund_modify"] = dsFunder.Tables[2];
            if (dsFunder.Tables[0] != null)
            {
                ddlcust.SelectedValue = dsFunder.Tables[0].Rows[0]["Customer_Id"].ToString();
                ddlcust.SelectedText = dsFunder.Tables[0].Rows[0]["customer_name"].ToString();
                txttranche.Text = dsFunder.Tables[0].Rows[0]["Tranche_Name"].ToString();
                txttranchedate.Text = dsFunder.Tables[0].Rows[0]["Tranche_date"].ToString();
                txtdisbursementdate.Text = dsFunder.Tables[0].Rows[0]["Disbursement_Date"].ToString();
                txtstatus.Text = dsFunder.Tables[0].Rows[0]["Status_id"].ToString();
                hdnfunder.Value = dsFunder.Tables[0].Rows[0]["Funder_id"].ToString();
                txtfunder.Text = dsFunder.Tables[0].Rows[0]["Funder_name"].ToString();
                txtTenure.Text = dsFunder.Tables[0].Rows[0]["Tenure"].ToString();
                Session["Tenure"] = txtTenure.Text;
                FunLoadRentalSchedule();

                FunProLoadStateCombos();

                if (dsFunder.Tables[0].Rows[0]["Invoice_Cov_Letter"].ToString() != "0")
                    ddlInvCovLetter.SelectedValue = dsFunder.Tables[0].Rows[0]["Invoice_Cov_Letter"].ToString();
                
            }

            if (dsFunder.Tables[1] != null)
            {
                if (dsFunder.Tables[1].Rows.Count > 0)
                {
                    gvOutFlow.DataSource = dsFunder.Tables[1];
                    gvOutFlow.DataBind();
                    Session["dtcashflow"] = dsFunder.Tables[1];
                }
            }

            if (dsFunder.Tables[4] != null)
            {
                pnlRswisebreakup.Visible = true;
                divrswise.Style.Add("display", "block");
                grvrswise.DataSource = dsFunder.Tables[7];
                grvrswise.DataBind();

                Session["dttarget"] = dsFunder.Tables[4];
                FunPriFindTotal();
            }
            ddllRSnumber.BindDataTable(dsFunder.Tables[8]);
            ddllRSnumber.SelectedIndex = 1;

            #region SanctionDetails
            Session["dtfundsave"] = dsFunder.Tables[9];
            #endregion

            //if (dsFunder.Tables[2] != null)
            //{
            //    pnlfunder.Visible = true;
            //    divfund.Style.Add("display", "block");
            //    grvfund.DataSource = dsFunder.Tables[2];
            //    grvfund.DataBind();
            //}
            //if (dsFunder.Tables[3] != null)
            //{
            //    Session["dttarget"] = dsFunder.Tables[3];
            //    divtarget.Style.Add("display", "block");
            //    pnltarget.Visible = true;
            //    grvtarget.DataSource = dsFunder.Tables[3];
            //    grvtarget.DataBind();
            //}

            #region Alerts Tab
            DtAlertDetails = dsFunder.Tables[5].Copy();
            if (DtAlertDetails.Rows.Count == 0)
            {
                FunPriBindAlertDLL(_Add);
            }
            else
            {
                Session["DtAlertDetails"] = DtAlertDetails;
                if (strtranche_id == 0)
                {

                    FunPriBindAlertDLL(_Add);
                }
                else
                {
                    FunPriBindAlertDLL(_Edit);
                }
                gvAlert.DataSource = DtAlertDetails;
                gvAlert.DataBind();
                FunPriGenerateNewAlertRow();
                Session["DtAlertDetails"] = DtAlertDetails;
            }
            #endregion

            #region Followup Tab


            if (dsFunder.Tables[6].Rows.Count == 0)
            {
                FunPriBindFollowupDLL(_Add);
            }
            else
            {
                Session["DtFollowUp"] = dsFunder.Tables[6];
                if (strtranche_id == 0)
                {
                    FunPriBindFollowupDLL(_Add);
                }
                else
                {
                    /*ADDED BY NATARAJ Y for issues in follow up loading by 27/6/2011*/
                    if (dsFunder.Tables[6].Rows.Count > 0)
                    {
                        FunPriBindFollowupDLL(_Edit);
                    }
                    else
                    {
                        FunPriBindFollowupDLL(_Add);
                    }
                }
                gvFollowUp.DataSource = dsFunder.Tables[6];
                gvFollowUp.DataBind();
                FunPriGenerateNewFollowupRow();
                Session["DtFollowUp"] = dsFunder.Tables[6];
            }
            #endregion

            #region Customer Email Det
            
            grvCustEmail.DataSource = dsFunder.Tables[10];
            grvCustEmail.DataBind();

            #endregion
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriCheckRS()
    {
        try
        {
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void grvRental_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //code added by vinodha for UAT Obs
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvRental.ClientID + "',this,'chkSelectAccount');");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectAccount");
                //chkSelectAccount.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvRental.ClientID + "','chkAll','chkSelectAccount');");
            }
            FunProGridDataBound(sender, e);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void FunProGridDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataTable dtmodify;
            dtmodify = (DataTable)Session["modify"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (strMode != "C")
                {
                    Label lblPASA_id = (Label)e.Row.FindControl("lblPASA_id");
                    Label lblRsType = (Label)e.Row.FindControl("lblRsType");
                    Label lblRS_Number = (Label)e.Row.FindControl("lblRS_Number");
                    CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectAccount");
                    DataRow[] dtrow = dtmodify.Select("PA_SA_REF_ID='" + lblPASA_id.Text + "' and RS_Type='" + lblRsType.Text + "'");
                    if (strMode == "Q")
                        chkSelectAccount.Enabled = false;
                    if (dtrow.Length > 0)
                    {
                        //txtchk.Text = "0";
                        chkSelectAccount.Checked = true;
                        dtRs = (DataTable)Session["dtRs"];
                        DataRow[] dtrows = dtRs.Select("RS_Number='" + lblRS_Number.Text + "' and RS_Type='" + lblRsType.Text + "'");
                        DataTable dt = dtrow.CopyToDataTable();
                        dt.Columns.Add("Status");
                        if (dt.Rows.Count > 0)
                        {
                            if (Session["cnt"].ToString() == "0")
                            {
                                due_date = dt.Rows[0]["dayss"].ToString();
                                Session["due_date"] = due_date;
                                Tenure = dt.Rows[0]["Tenure"].ToString();
                                Session["Tenure"] = Tenure;
                                frequency = dt.Rows[0]["frequency"].ToString();
                                Session["frequency"] = frequency;
                            }

                            boolexists = true;
                            d_cnt = d_cnt + 1;
                            Session["cnt"] = d_cnt;
                            dtrows[0]["status"] = "1";
                            dtRs.AcceptChanges();
                            Session["dtRs"] = dtRs;
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

    protected void grvfund_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (strMode != "C")
                {
                    dtfund_modify = (DataTable)Session["dtfund_modify"];
                    Label lblSanctionid = (Label)e.Row.FindControl("lblSanctionid");
                    Label lblassetid = (Label)e.Row.FindControl("lblassetid");
                    CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectAccount");

                    DataRow[] dtrow = dtfund_modify.Select("sanction_id='" + lblSanctionid.Text + "'");

                    if (strMode == "Q")
                        chkSelectAccount.Enabled = false;
                    if (dtrow.Length > 0)
                    {
                        // txtchk.Text = "0";
                        chkSelectAccount.Checked = true;
                        dtsanction = (DataTable)Session["dtsanction"];
                        DataRow[] dtrow1 = dtsanction.Select("asset_category_id='" + lblassetid.Text + "' and Sanction_id=" + lblSanctionid.Text + "");
                        DataTable dt = dtrow.CopyToDataTable();
                        if (dt.Rows.Count > 0)
                        {
                            dtrow1[0]["status"] = "1";
                            dtsanction.AcceptChanges();
                            Session["dtsanction"] = dtsanction;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage);
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunpriClear();
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    private void FunpriClear()
    {
        try
        {
            ddlcust.SelectedValue = "0";
            ddlcust.SelectedText = "--Select--";
            //txtfunder.SelectedText = "--Select--";
            txtdisbursementdate.Text = "";
            txttranche.Text = "";
            txttranchedate.Text = DateTime.Now.ToString(strDateFormat);
            txtstatus.Text = "";
            grvRental.DataSource = null;
            grvRental.DataBind();
            divfund.Style.Add("display", "none");
            divacc.Style.Add("display", "none");
            div1.Style.Add("display", "none");
            //divsource.Style.Add("display", "none");
            //divtarget.Style.Add("display", "none");
            grvfund.DataSource = null;
            grvfund.DataBind();
            gvOutFlow.DataSource = null;
            gvOutFlow.DataBind();
            //grvsource.DataSource = null;
            //grvsource.DataBind();
            //grvtarget.DataSource = null;
            //grvtarget.DataBind();
            //txtfunder.SelectedValue = "0";
            txtfunder.Clear();
            hdnfunder.Value = "";
            funpriclearviewstate();
            pnlcashflow.Visible = false;
            pnlfunder.Visible = false;
            pnlrs.Visible = false;
            pnlasset.Visible = false;
            tcFunder.ActiveTabIndex = 0;
            TabPanel1.Enabled = false;
            TabPanel2.Enabled = false;
            TabAlerts.Enabled = false;
            TabFollowUp.Enabled = false;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }
    private void funpriclearviewstate()
    {
        try
        {
            Session["dtcashflow"] = null;
            Session["dscashflow"] = null;
            Session["dtRs"] = null;
            Session["dtsource"] = null;
            Session["dttranche"] = null;
            Session["dtfundsave"] = null;
            Session["dsfundgrid"] = null;
            Session["due_date"] = null;
            Session["Tenure"] = null;
            Session["frequency"] = null;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriEnableDisableControls()
    {
        try
        {
            if (strMode == "Q")
            {
                ddlcust.ReadOnly = txtfunder.ReadOnly = txttranche.ReadOnly = txtstatus.ReadOnly = txtdisbursementdate.ReadOnly = true;
                gvOutFlow.Columns[6].Visible = false;
                txttranchedate.ReadOnly = true;
                chkconsolidated.Enabled = false;
                CalendarExtenderTranchedate.Enabled = false;
                caldisb.Enabled = false;
                btnProcess.Enabled = btnfetch.Enabled = btnvalidate.Enabled = btnGo.Enabled = false;
                foreach (GridViewRow grv in grvrswise.Rows)
                {
                    TextBox lblrentaldisc = (TextBox)grv.FindControl("lblrentaldisc");
                    TextBox lblVATdisc = (TextBox)grv.FindControl("lblVATdisc");
                    TextBox lblAMFdisc = (TextBox)grv.FindControl("lblAMFdisc");
                    TextBox lblServiceTaxdisc = (TextBox)grv.FindControl("lblServiceTaxdisc");
                    lblrentaldisc.ReadOnly = lblVATdisc.ReadOnly = lblAMFdisc.ReadOnly = lblServiceTaxdisc.ReadOnly = true;
                }
                /*added by vinodha .m to disable update button in query mode*/
                btnSave.Enabled = btnvalidate.Enabled = btnClear.Enabled = btnUpdate.Enabled = false;
                /*added by vinodha .m to disable update button in query mode*/
                btnPrint.Enabled = true;
                btprintExcel.Enabled = btnprintwoa.Enabled = true;
                //gvOutFlow.Rows[6].Visible = false;
                txtLRdisc.ReadOnly = txtAMFdisc.ReadOnly = txtSerDisc.ReadOnly = txtTaxdisc.ReadOnly = true;
                gvOutFlow.FooterRow.Visible = gvAlert.FooterRow.Visible = gvAlert.Columns[6].Visible =
                gvFollowUp.FooterRow.Visible = gvFollowUp.Columns[9].Visible = btnValidateRS.Enabled = false;
                TabPanel1.Enabled = TabPanel2.Enabled = TabAlerts.Enabled = TabFollowUp.Enabled = true;
                //if (gvOutFlow.Rows.Count > 0)
                //{
                //    gvOutFlow.Rows[6].Visible = false;
                //}
                if (grvRental != null)
                {
                    CheckBox chkAll = (CheckBox)grvRental.HeaderRow.FindControl("chkAll");
                    chkAll.Visible = false;
                }
                ddlInvCovLetter.ClearDropDownList();
            }
            else if (Convert.ToString(strMode) == "M")
            {
                btnClear.Enabled = false;
                TabPanel1.Enabled = TabPanel2.Enabled = TabAlerts.Enabled = TabFollowUp.Enabled = btnPrint.Enabled =
                btprintExcel.Enabled = btnprintwoa.Enabled = true;

                if(txtstatus.Text=="Approved")
                {
                    btnValidateRS.Visible = false;
                    btnfetch.Visible = false;
                    btnProcess.Visible = false;
                    btnUpdate.Visible = false;
                    btnGo.Visible = false;
                    btnvalidate.Visible = false;
                }
            }
            else if (Convert.ToString(strMode) == "C")
            {
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void imgbtnShow_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataTable dtmodal;
            string strSelectID = ((ImageButton)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvRental", strSelectID);

            Label lblPASA_id = (Label)grvRental.Rows[_iRowIdx].FindControl("lblPASA_id");

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@company_id", intCompanyID.ToString());
            Procparam.Add("@PASA_Ref_Id", lblPASA_id.Text.ToString());
            dtmodal = Utility.GetDefaultData("S3G_Fund_GetRSSchedule", Procparam);
            grvpopup.DataSource = dtmodal;
            grvpopup.DataBind();
            moeRS.Show();
            if (grvpopup.FooterRow != null)
            {
                ((Label)grvpopup.FooterRow.FindControl("lbltotrental")).Text = Convert.ToDecimal(dtmodal.Compute("sum(rental1)", "")).ToString(Funsetsuffix());
                ((Label)grvpopup.FooterRow.FindControl("lbltotAMF")).Text = Convert.ToDecimal(dtmodal.Compute("sum(AMF1)", "")).ToString(Funsetsuffix());
                ((Label)grvpopup.FooterRow.FindControl("lbltotVAT")).Text = Convert.ToDecimal(dtmodal.Compute("sum(VAT1)", "")).ToString(Funsetsuffix());
                ((Label)grvpopup.FooterRow.FindControl("lbltotSTAX")).Text = Convert.ToDecimal(dtmodal.Compute("sum(ServiceTax1)", "")).ToString(Funsetsuffix());
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void btnLesseeClose_Click(object sender, EventArgs e)
    {
        try
        {
            moeRS.Hide();
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
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


    private void FunPriFindTotal()
    {
        try
        {
            if (Session["dttarget"] != null)
                dttarget = (System.Data.DataTable)Session["dttarget"];
            if (dttarget.Rows.Count > 0)
            {
                Label Totlblrental = (Label)(grvrswise).FooterRow.FindControl("Totlblrental") as Label;
                Totlblrental.Text = Convert.ToDecimal(dttarget.Compute("sum(Rental1)", "")).ToString(Funsetsuffix());

                Label Totlblrentaldisc = (Label)(grvrswise).FooterRow.FindControl("Totlblrentaldisc") as Label;
                Totlblrentaldisc.Text = Convert.ToDecimal(dttarget.Compute("sum(Rental_disc1)", "")).ToString(Funsetsuffix());

                Label TotllblVAT = (Label)(grvrswise).FooterRow.FindControl("TotllblVAT") as Label;
                TotllblVAT.Text = Convert.ToDecimal(dttarget.Compute("sum(Rental_Tax1)", "")).ToString(Funsetsuffix());

                Label TotlblVATdisc = (Label)(grvrswise).FooterRow.FindControl("TotlblVATdisc") as Label;
                TotlblVATdisc.Text = Convert.ToDecimal(dttarget.Compute("sum(Rental_Tax_disc1)", "")).ToString(Funsetsuffix());

                Label TotlblAMF = (Label)(grvrswise).FooterRow.FindControl("TotlblAMF") as Label;
                TotlblAMF.Text = Convert.ToDecimal(dttarget.Compute("sum(AMF1)", "")).ToString(Funsetsuffix());

                Label TotlblAMFdisc = (Label)(grvrswise).FooterRow.FindControl("TotlblAMFdisc") as Label;
                TotlblAMFdisc.Text = Convert.ToDecimal(dttarget.Compute("sum(AMF_disc1)", "")).ToString(Funsetsuffix());

                Label TotlblServiceTax = (Label)(grvrswise).FooterRow.FindControl("TotlblServiceTax") as Label;
                TotlblServiceTax.Text = Convert.ToDecimal(dttarget.Compute("sum(Service_Tax1)", "")).ToString(Funsetsuffix());

                Label TotlblServiceTaxdisc = (Label)(grvrswise).FooterRow.FindControl("TotlblServiceTaxdisc") as Label;
                TotlblServiceTaxdisc.Text = Convert.ToDecimal(dttarget.Compute("sum(Service_Tax_disc1)", "")).ToString(Funsetsuffix());

                //Added on 31Mar2015 starts here
                //Show Total Discounted value as per Client Request on 30Mar2015
                //if (Convert.ToString(strMode) != "C")
                //{

                txtLRdisc.Text = Totlblrentaldisc.Text;
                txtTaxdisc.Text = TotlblVATdisc.Text;
                txtAMFdisc.Text = TotlblAMFdisc.Text;
                txtSerDisc.Text = TotlblServiceTaxdisc.Text;
                //}
                //Added on 31Mar2015 ends here

                txtLR.Text = Totlblrental.Text;
                txtAMF.Text = TotlblAMF.Text;
                txtTax.Text = TotllblVAT.Text;
                txtST.Text = TotlblServiceTax.Text;

                //Added On 29Jan2016 Starts Here
                //Bug fixed with reference of mail raised on 28Jan2016
                TotlblAMF.Text = (Convert.ToDecimal(TotlblAMF.Text) == 0) ? "1" : Convert.ToString(TotlblAMF.Text);
                TotllblVAT.Text = (Convert.ToDecimal(TotllblVAT.Text) == 0) ? "1" : Convert.ToString(TotllblVAT.Text);
                TotlblServiceTax.Text = (Convert.ToDecimal(TotlblServiceTax.Text) == 0) ? "1" : Convert.ToString(TotlblServiceTax.Text);
                //Added on 29Jan2016 Ends Here

                lblLRper.Text = ((Convert.ToDecimal(Totlblrentaldisc.Text.ToString()) / Convert.ToDecimal(Totlblrental.Text.ToString())) * 100).ToString("0.00") + "%";
                hdnlrper.Value = ((Convert.ToDecimal(Totlblrentaldisc.Text.ToString()) / Convert.ToDecimal(Totlblrental.Text.ToString())) * 100).ToString("0.00000000");

                lblAMFPer.Text = ((Convert.ToDecimal(TotlblAMFdisc.Text.ToString()) / Convert.ToDecimal(TotlblAMF.Text.ToString())) * 100).ToString("0.00") + "%";
                hdnamfper.Value = ((Convert.ToDecimal(TotlblAMFdisc.Text.ToString()) / Convert.ToDecimal(TotlblAMF.Text.ToString())) * 100).ToString("0.00000000");

                lblTaxper.Text = ((Convert.ToDecimal(TotlblVATdisc.Text.ToString()) / Convert.ToDecimal(TotllblVAT.Text.ToString())) * 100).ToString("0.00") + "%";
                hdnvatper.Value = ((Convert.ToDecimal(TotlblVATdisc.Text.ToString()) / Convert.ToDecimal(TotllblVAT.Text.ToString())) * 100).ToString("0.00000000");

                lblserdisc.Text = ((Convert.ToDecimal(TotlblServiceTaxdisc.Text.ToString()) / Convert.ToDecimal(TotlblServiceTax.Text.ToString())) * 100).ToString("0.00") + "%";
                hdnserper.Value = ((Convert.ToDecimal(TotlblServiceTaxdisc.Text.ToString()) / Convert.ToDecimal(TotlblServiceTax.Text.ToString())) * 100).ToString("0.00000000");

            }
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Move Invoices.";
            cvTranche.IsValid = false;
        }
    }

    private void FunPriFindAssetTotal()
    {
        try
        {
            if (Session["assettotal"] != null)
                dtassettotal = (System.Data.DataTable)Session["assettotal"];
            if (dtassettotal.Rows.Count > 0)
            {
                Label Totlblrentalsource = (Label)(grvasset).FooterRow.FindControl("Totlblrentalsource") as Label;
                Totlblrentalsource.Text = Convert.ToDecimal(dtassettotal.Compute("sum(rental1)", "")).ToString(Funsetsuffix());

                Label TotlblAMF = (Label)(grvasset).FooterRow.FindControl("TotlblAMF") as Label;
                TotlblAMF.Text = Convert.ToDecimal(dtassettotal.Compute("sum(AMF1)", "")).ToString(Funsetsuffix());

                Label TotlblVAT = (Label)(grvasset).FooterRow.FindControl("TotlblVAT") as Label;
                TotlblVAT.Text = Convert.ToDecimal(dtassettotal.Compute("sum(VAT1)", "")).ToString(Funsetsuffix());

                Label TotlblServiceTax = (Label)(grvasset).FooterRow.FindControl("TotlblServiceTax") as Label;
                TotlblServiceTax.Text = Convert.ToDecimal(dtassettotal.Compute("sum(ServiceTax1)", "")).ToString(Funsetsuffix());
            }
        }
        catch (Exception ex)
        {

            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Move Invoices.";
            cvTranche.IsValid = false;
        }
    }

    protected void gvOutFlow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtcashflow = (DataTable)Session["dtcashflow"];
            dtcashflow.Rows.RemoveAt(e.RowIndex);
            Session["dtcashflow"] = dtcashflow;
            gvOutFlow.DataSource = dtcashflow;
            gvOutFlow.DataBind();
            if (dtcashflow.Rows.Count == 0)
            {
                FunPriSetEmptyCashtbl();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void lblrentaldisc_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal rental = 0;
            foreach (GridViewRow gvrow in grvrswise.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    if (((TextBox)gvrow.Cells[0].FindControl("lblrentaldisc")).Text.ToString() != "")
                        rental = Convert.ToDecimal(rental) + Convert.ToDecimal(((TextBox)gvrow.Cells[0].FindControl("lblrentaldisc")).Text.ToString());
                    else
                        rental = Convert.ToDecimal(rental) + Convert.ToDecimal("0.00");
                }
            }
            Label Totlblrentaldisc = (Label)(grvrswise).FooterRow.FindControl("Totlblrentaldisc") as Label;
            Totlblrentaldisc.Text = rental.ToString(Funsetsuffix());
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void lblVATdisc_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal rental = 0;
            foreach (GridViewRow gvrow in grvrswise.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    if (((TextBox)gvrow.Cells[0].FindControl("lblVATdisc")).Text.ToString() != "")
                        rental = Convert.ToDecimal(rental) + Convert.ToDecimal(((TextBox)gvrow.Cells[0].FindControl("lblVATdisc")).Text.ToString());
                    else
                        rental = Convert.ToDecimal(rental) + Convert.ToDecimal("0.00");
                }
            }
            Label TotlblVATdisc = (Label)(grvrswise).FooterRow.FindControl("TotlblVATdisc") as Label;
            TotlblVATdisc.Text = rental.ToString(Funsetsuffix());
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void lblAMFdisc_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal rental = 0;
            foreach (GridViewRow gvrow in grvrswise.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    if (((TextBox)gvrow.Cells[0].FindControl("lblAMFdisc")).Text.ToString() != "")
                        rental = Convert.ToDecimal(rental) + Convert.ToDecimal(((TextBox)gvrow.Cells[0].FindControl("lblAMFdisc")).Text.ToString());
                    else
                        rental = Convert.ToDecimal(rental) + Convert.ToDecimal("0.00");
                }
            }
            Label TotlblAMFdisc = (Label)(grvrswise).FooterRow.FindControl("TotlblAMFdisc") as Label;
            TotlblAMFdisc.Text = rental.ToString(Funsetsuffix());
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void lblServiceTaxdisc_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            decimal rental = 0;
            foreach (GridViewRow gvrow in grvrswise.Rows)
            {
                if (gvrow.RowType == DataControlRowType.DataRow)
                {
                    if (((TextBox)gvrow.Cells[0].FindControl("lblServiceTaxdisc")).Text.ToString() != "")
                        rental = Convert.ToDecimal(rental) + Convert.ToDecimal(((TextBox)gvrow.Cells[0].FindControl("lblServiceTaxdisc")).Text.ToString());
                    else
                        rental = Convert.ToDecimal(rental) + Convert.ToDecimal("0.00");
                }
            }
            Label TotlblServiceTaxdisc = (Label)(grvrswise).FooterRow.FindControl("TotlblServiceTaxdisc") as Label;
            TotlblServiceTaxdisc.Text = rental.ToString(Funsetsuffix());
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = ex.Message;
            cvTranche.IsValid = false;
        }
    }

    #region Alert Tab

    private void FunPriBindAlertDLL(string Mode)
    {
        try
        {
            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@CompanyId", intCompanyID.ToString());
            System.Data.DataSet dsAlert = Utility.GetDataset("s3g_org_loadAlertLov", objParameters);

            DataRow[] dr = dsAlert.Tables[0].Select("ID in(141,219,220,221,222,223,224,225,239)");
            System.Data.DataTable dtAlert = dr.CopyToDataTable();
            Session["AlertDDL"] = dtAlert;
            Session["AlertUser"] = dsAlert;
            //End Here
            if (Mode == _Add)
            {

                System.Data.DataTable ObjDT = new System.Data.DataTable();
                ObjDT.Columns.Add("Type");
                ObjDT.Columns.Add("TypeID");
                ObjDT.Columns.Add("UserContact");
                ObjDT.Columns.Add("UserContactID");
                ObjDT.Columns.Add("EMail");
                ObjDT.Columns["Email"].DataType = typeof(Boolean);
                ObjDT.Columns.Add("SMS");
                ObjDT.Columns["SMS"].DataType = typeof(Boolean);
                DataRow dr_Alert = ObjDT.NewRow();
                dr_Alert["Type"] = "";
                dr_Alert["TypeID"] = "";
                dr_Alert["UserContact"] = "";
                dr_Alert["UserContactID"] = "";
                dr_Alert["EMail"] = "False";
                dr_Alert["SMS"] = "False";
                ObjDT.Rows.Add(dr_Alert);

                gvAlert.DataSource = ObjDT;
                gvAlert.DataBind();

                ObjDT.Rows.Clear();
                Session["DtAlertDetails"] = ObjDT;

                gvAlert.Rows[0].Cells.Clear();
                gvAlert.Rows[0].Visible = false;
                FunPriGenerateNewAlertRow();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriGenerateNewAlertRow()
    {
        try
        {
            DropDownList ObjddlType_AlertTab = gvAlert.FooterRow.FindControl("ddlType_AlertTab") as DropDownList;
            //Removed By Shibu 18-Sep-2013
            //  DropDownList ObjddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as DropDownList;
            UserControls_S3GAutoSuggest ddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as UserControls_S3GAutoSuggest;
            Utility.FillDLL(ObjddlType_AlertTab, ((System.Data.DataTable)Session["AlertDDL"]), true);
            //Utility.FillDLL(ObjddlContact_AlertTab, ((System.Data.DataSet)Session["AlertUser"]).Tables[1], true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void Alert_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtAlertDetails = (System.Data.DataTable)Session["DtAlertDetails"];

            DropDownList ddlAlert_Type = gvAlert.FooterRow.FindControl("ddlType_AlertTab") as DropDownList;
            //Removed By Shibu 18-Sep-2013
            // DropDownList ddlAlert_ContactList = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as DropDownList;
            UserControls_S3GAutoSuggest ddlContact_AlertTab = gvAlert.FooterRow.FindControl("ddlContact_AlertTab") as UserControls_S3GAutoSuggest;
            CheckBox ChkAlertEmail = gvAlert.FooterRow.FindControl("ChkEmail") as CheckBox;
            CheckBox ChkAlertSMS = gvAlert.FooterRow.FindControl("ChkSMS") as CheckBox;

            if (ChkAlertEmail.Checked || ChkAlertSMS.Checked)
            {

                //For Duplication
                if (DtAlertDetails.Rows.Count > 0)
                {
                    DataRow[] drAlertDetails = null;
                    drAlertDetails = DtAlertDetails.Select(" TypeId = " + ddlAlert_Type.SelectedValue + " and UserContactId=" + ddlContact_AlertTab.SelectedValue + "");
                    if (drAlertDetails.Count() > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Combination already exists');", true);
                        return;
                    }
                }


                DataRow dr = DtAlertDetails.NewRow();

                dr["TypeId"] = ddlAlert_Type.SelectedValue;
                dr["Type"] = ddlAlert_Type.SelectedItem;
                dr["UserContactId"] = ddlContact_AlertTab.SelectedValue.ToString();
                dr["UserContact"] = ddlContact_AlertTab.SelectedText;
                dr["EMail"] = ChkAlertEmail.Checked;
                dr["SMS"] = ChkAlertSMS.Checked;

                DtAlertDetails.Rows.Add(dr);

                gvAlert.DataSource = DtAlertDetails;
                gvAlert.DataBind();
                //gvAlert0.DataSource = DtAlertDetails;
                //gvAlert0.DataBind();
                Session["DtAlertDetails"] = DtAlertDetails;
                FunPriGenerateNewAlertRow();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Select Email or SMS');", true);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Add Alert";
            cvTranche.IsValid = false;
        }

    }
    protected void gvAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunPriBindAlertDetails(e);
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvTranche.IsValid = false;
        }
    }

    private void FunPriBindAlertDetails(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox ChkAlertEmail = e.Row.FindControl("ChkEmail") as CheckBox;
            CheckBox ChkAlertSMS = e.Row.FindControl("ChkSMS") as CheckBox;
            ChkAlertEmail.Enabled = ChkAlertSMS.Enabled = false;
        }
    }

    protected void gvAlert_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtAlertDetails = (System.Data.DataTable)Session["DtAlertDetails"];
            if (DtAlertDetails.Rows.Count > 0)
            {
                DtAlertDetails.Rows.RemoveAt(e.RowIndex);

                if (DtAlertDetails.Rows.Count == 0)
                {
                    FunPriBindAlertDLL(_Add);
                }
                else
                {
                    gvAlert.DataSource = DtAlertDetails;
                    gvAlert.DataBind();
                    FunPriGenerateNewAlertRow();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Remove Alert";
            cvTranche.IsValid = false;

        }
    }

    #endregion

    #region Follow Up Tab

    private void FunPriBindFollowupDLL(string Mode)
    {
        ObjCustomerService = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            S3GBusEntity.S3G_Status_Parameters ObjStatus = new S3G_Status_Parameters();
            System.Data.DataTable ObjDT = new System.Data.DataTable();
            if (Mode == _Add)
            {
                ObjStatus.Option = 35;
                ObjStatus.Param1 = intCompanyID.ToString();
                Session["UserListFolloup"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                ObjStatus.Option = 47;
                ObjStatus.Param1 = null;
                ObjDT = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);
                ObjDT.Columns.Add("FromUserId");
                ObjDT.Columns.Add("ToUserId");
            }
            if (Mode == _Edit)
            {
                ObjStatus.Option = 35;
                ObjStatus.Param1 = intCompanyID.ToString();
                Session["UserListFolloup"] = ObjCustomerService.FunPub_GetS3GStatusLookUp(ObjStatus);

                if (((System.Data.DataTable)Session["DtFollowUp"]) != null)
                    ObjDT = (System.Data.DataTable)Session["DtFollowUp"];

            }
            gvFollowUp.DataSource = ObjDT;
            gvFollowUp.DataBind();

            if (Mode == _Add)
            {
                ObjDT.Rows.Clear();
                Session["DtFollowUp"] = ObjDT;

                gvFollowUp.Rows[0].Cells.Clear();
                gvFollowUp.Rows[0].Visible = false;

            }
            FunPriGenerateNewFollowupRow();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjCustomerService.Close();
        }

    }

    private void FunPriGenerateNewFollowupRow()
    {
        try
        {
            if (gvFollowUp.Rows.Count > 0)
            {  //Removed By Shibu 18-Sep-2013
                //DropDownList ddlfrom_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as DropDownList;
                UserControls_S3GAutoSuggest ddlfrom_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as UserControls_S3GAutoSuggest;
                //DropDownList ddlTo_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as DropDownList;
                UserControls_S3GAutoSuggest ddlTo_GridFollowup = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as UserControls_S3GAutoSuggest;
                //Utility.FillDLL(ddlfrom_GridFollowup, ((System.Data.DataTable)Session["UserListFolloup"]), true);
                //Utility.FillDLL(ddlTo_GridFollowup, ((System.Data.DataTable)Session["UserListFolloup"]), true);
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FollowUp_AddRow_OnClick(object sender, EventArgs e)
    {
        try
        {
            DtFollowUp = (System.Data.DataTable)Session["DtFollowUp"];

            TextBox txttxtDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtDate_GridFollowup") as TextBox;
            //Removed By Shibu 18-Sep-2013
            //DropDownList ddlfrom_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as DropDownList;
            //DropDownList ddlTo_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as DropDownList;
            UserControls_S3GAutoSuggest ddlfrom_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlfrom_GridFollowup") as UserControls_S3GAutoSuggest;
            UserControls_S3GAutoSuggest ddlTo_GridFollowup1 = gvFollowUp.FooterRow.FindControl("ddlTo_GridFollowup") as UserControls_S3GAutoSuggest;
            TextBox txtAction_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtAction_GridFollowup") as TextBox;
            TextBox txtActionDate_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtActionDate_GridFollowup") as TextBox;
            TextBox txtCustomerResponse_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtCustomerResponse_GridFollowup") as TextBox;
            TextBox txtRemarks_GridFollowup1 = gvFollowUp.FooterRow.FindControl("txtRemarks_GridFollowup") as TextBox;
            if (Utility.CompareDates(txttxtDate_GridFollowup1.Text, txtActionDate_GridFollowup1.Text) != 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('Action Date should be greater than Date in Followup');", true);
                return;
            }
            if (ddlfrom_GridFollowup1.SelectedValue == ddlTo_GridFollowup1.SelectedValue)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Followup Details", "alert('From and To UserName should be different');", true);
                return;
            }
            DataRow dr = DtFollowUp.NewRow();
            dr["Date"] = Utility.StringToDate(txttxtDate_GridFollowup1.Text);
            dr["From"] = ddlfrom_GridFollowup1.SelectedText;
            dr["FromUserId"] = ddlfrom_GridFollowup1.SelectedValue;
            dr["To"] = ddlTo_GridFollowup1.SelectedText;
            dr["ToUserId"] = ddlTo_GridFollowup1.SelectedValue;
            dr["Action"] = txtAction_GridFollowup1.Text;
            dr["ActionDate"] = Utility.StringToDate(txtActionDate_GridFollowup1.Text);
            dr["CustomerResponse"] = txtCustomerResponse_GridFollowup1.Text;
            dr["Remarks"] = txtRemarks_GridFollowup1.Text;

            DtFollowUp.Rows.Add(dr);

            gvFollowUp.DataSource = DtFollowUp;
            gvFollowUp.DataBind();

            Session["DtFollowUp"] = DtFollowUp;
            FunPriGenerateNewFollowupRow();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Add Followup";
            cvTranche.IsValid = false;
        }
    }

    protected void gvFollowUp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DtFollowUp = (System.Data.DataTable)Session["DtFollowUp"];
            if (DtFollowUp.Rows.Count > 0)
            {
                DtFollowUp.Rows.RemoveAt(e.RowIndex);

                if (DtFollowUp.Rows.Count == 0)
                {
                    FunPriBindFollowupDLL(_Add);
                }
                else
                {
                    gvFollowUp.DataSource = DtFollowUp;
                    gvFollowUp.DataBind();
                    FunPriGenerateNewFollowupRow();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Remove Followup";
            cvTranche.IsValid = false;
        }
    }

    protected void gvFollowUp_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtDate_GridFollowup = e.Row.FindControl("txtDate_GridFollowup") as TextBox;
            txtDate_GridFollowup.Attributes.Add("readonly", "readonly");
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupDate = e.Row.FindControl("CalendarExtenderSD_FollowupDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_FollowupDate.Format = ObjS3GSession.ProDateFormatRW;

            TextBox txtActionDate_GridFollowup = e.Row.FindControl("txtActionDate_GridFollowup") as TextBox;
            txtActionDate_GridFollowup.Attributes.Add("readonly", "readonly");
            AjaxControlToolkit.CalendarExtender CalendarExtenderSD_FollowupActionDate = e.Row.FindControl("CalendarExtenderSD_FollowupActionDate") as AjaxControlToolkit.CalendarExtender;
            CalendarExtenderSD_FollowupActionDate.Format = ObjS3GSession.ProDateFormatRW;
        }
    }
    #endregion

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            dtRs = (DataTable)Session["dtRs"];

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@accxml", dtRs.FunPubFormXml());
            Procparam.Add("@CustomerID", ddlcust.SelectedValue);
            Procparam.Add("@TypeID", "4");
            Procparam.Add("@Tranche_ID", strtranche_id.ToString());
            Session["Format_Type1"] = "4";
            Session["ProcParam"] = Procparam;
            // Session["Format_Type"] = ddlReportType.SelectedValue;//Format_Type
            string strScipt = "window.open('../Fund Management/S3G_RPt_TrancheReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Note Creation", strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Print the Repayment Details";
            cvTranche.IsValid = false;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSalePersonList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_Get_User_List, Procparam));

        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetCustomerName_EUCList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Customer_Id", obj_Page.ddlcust.SelectedValue);
        Procparam.Add("@PrefixText", prefixText.Trim());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetCustomerName_EUCList", Procparam));
        return suggetions.ToArray();
    }

    //print annexure with amount  
    protected void btprintExcel_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriGetTableData(true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Print the Repayment Details";
            cvTranche.IsValid = false;
        }
    }


    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtcheck.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Update the changed values");
                return;
            }
            GridView Grv = new GridView();
            dttarget = (DataTable)Session["dttarget"];
            DataTable dt = dttarget.Copy();
            dt.Columns.Remove("PA_SA_REF_ID");
            dt.Columns.Remove("RS_Type");
            dt.Columns.Remove("Rental_disc1");
            dt.Columns.Remove("AMF_disc1");
            dt.Columns.Remove("Rental_Tax_disc1");
            dt.Columns.Remove("Service_Tax_disc1");
            dt.Columns.Remove("Rental1");
            dt.Columns.Remove("AMF1");
            dt.Columns.Remove("Rental_Tax1");
            dt.Columns.Remove("Service_Tax1");
            dt.Columns["Rental_disc"].ColumnName = "Discountred Rental";
            dt.Columns["AMF_disc"].ColumnName = "Discountred AMF";
            dt.Columns["Rental_Tax_disc"].ColumnName = "Discountred VAT";
            dt.Columns["Service_Tax_disc"].ColumnName = "Discountred ServiceTax";
            dt.Columns["Rental_source"].ColumnName = "Source Rental";
            dt.Columns["AMF_source"].ColumnName = "Source AMF";
            dt.Columns["Rental_Tax_source"].ColumnName = "Source VAT";
            dt.Columns["Service_Tax_source"].ColumnName = "Source ServiceTax";
            dt.Columns["rs_number"].ColumnName = "RS Number";
            dt.Columns["Installment_No"].ColumnName = "Installment No";
            dt.AcceptChanges();
            Grv.DataSource = dt;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Tranche Amort.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Print the Tranche";
            cvTranche.IsValid = false;
        }
    }
    //print annexure without amount  
    protected void btnprintwoa_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriGetTableData(false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Print the Repayment Details";
            cvTranche.IsValid = false;
        }
    }

    protected void FunPriGetTableData(bool isamount)
    {
        try
        {
            dtRs = (DataTable)Session["dtRs"];

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@accxml", dtRs.FunPubFormXml());
            Procparam.Add("@CustomerID", ddlcust.SelectedValue);
            Procparam.Add("@TypeID", "5");
            if (isamount == true)
                Procparam.Add("@is_amount", "1");
            else
                Procparam.Add("@is_amount", "0");

            dsprint = Utility.GetDataset("S3G_LOANAD_GET_RPT_TrancheDtls", Procparam);
            GridView grv = new GridView();

            if (isamount == true)
            {
                System.Data.DataTable dt = dsprint.Tables[0];

                dt.Columns.Add("Amt", typeof(string)).SetOrdinal(7);
                dt.AcceptChanges();

                foreach (DataRow dr in dt.Rows)
                {
                    String value = dr["Amount"].ToString();
                    value = Convert.ToDecimal(value).ToString("N", new CultureInfo("hi-IN"));
                    dr["Amt"] = value;
                    dt.AcceptChanges();
                }
                dt.Columns.Remove("Amount");
                if (dt.Columns[7].ColumnName == "Amt")
                    dt.Columns[7].ColumnName = "Amount";
                dt.AcceptChanges();
                grv.DataSource = dt;
                grv.DataBind();
            }
            else
            {
                grv.DataSource = dsprint;
                grv.DataBind();
            }

            GridView grvh = new GridView();

            if (grv.Rows.Count > 0)
            {
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Col");

                DataRow row = dtHeader.NewRow();
                row["Col"] = "Annexure";
                dtHeader.Rows.Add(row);

                grvh.DataSource = dtHeader;
                grvh.DataBind();

                grvh.GridLines = GridLines.Both;

                grvh.HeaderRow.Visible = false;

                if (isamount == true)
                    grvh.Rows[0].Cells[0].ColumnSpan = 11;
                else
                    grvh.Rows[0].Cells[0].ColumnSpan = 10;

                grvh.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                grvh.Font.Bold = true;
                grvh.ForeColor = System.Drawing.Color.Black;
                grvh.Font.Name = "calibri";
                grvh.Font.Size = 10;
            }

            GridView grv1 = new GridView();
            if (grv.Rows.Count > 0)
            {
                if (isamount == true)
                {
                    DataTable dtHeader = new DataTable();
                    dtHeader.Columns.Add("Col");
                    dtHeader.Columns.Add("Col1");
                    dtHeader.Columns.Add("Col2");
                    dtHeader.Columns.Add("Col3");
                    dtHeader.Columns.Add("Col4");
                    dtHeader.Columns.Add("Col5");
                    dtHeader.Columns.Add("Total");
                    dtHeader.Columns.Add("Amount", typeof(string));
                    dtHeader.Columns.Add("Col6");
                    dtHeader.Columns.Add("Col7");
                    dtHeader.Columns.Add("Col8");

                    DataRow row = dtHeader.NewRow();
                    row["Total"] = "Total";
                    if (dsprint.Tables[1].Rows[0][0].ToString() != String.Empty)
                        row["Amount"] = Convert.ToDecimal(dsprint.Tables[1].Rows[0][0].ToString()).ToString("N", new CultureInfo("hi-IN"));
                    dtHeader.Rows.Add(row);

                    grv1.DataSource = dtHeader;
                    grv1.DataBind();

                    grv1.GridLines = GridLines.Both;

                    grv1.HeaderRow.Visible = false;

                    grv1.Rows[0].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    grv1.Rows[0].HorizontalAlign = HorizontalAlign.Right;

                    grv1.Font.Bold = true;
                    grv1.ForeColor = System.Drawing.Color.Black;
                    grv1.Font.Name = "calibri";
                    grv1.Font.Size = 10;
                }

                string attachment;
                if (isamount == true)
                    attachment = "attachment; filename=annex-1_aoo wa.xls";
                else
                    attachment = "attachment; filename=annex-1_aoo woa.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grvh.RenderControl(htw);
                grv.RenderControl(htw);
                if (isamount == true)
                    grv1.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    #region Customer Email Det
    //opc042 start
    protected void grvCustEmail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblEmailAlert = e.Row.FindControl("lblEmail_Alert") as Label;
                CheckBox ChkAlertEmail = e.Row.FindControl("chkEmailAlert") as CheckBox;
                if(lblEmailAlert.Text=="1")
                {
                    ChkAlertEmail.Checked = true;
                }
                else
                {
                    ChkAlertEmail.Checked = false;
                }
                //ChkAlertEmail.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = strErrorMessagePrefix + ex.Message;
            cvTranche.IsValid = false;
        }
    }

    private string FunCustEmailBuilder()
    {
        StringBuilder strCustEmailBuilder = new StringBuilder();
        strCustEmailBuilder.Append("<ROOT>");
      
        try
        {
            foreach (GridViewRow grvCust in grvCustEmail.Rows)
            {
                CheckBox chkEmailAlert = (CheckBox)grvCust.FindControl("chkEmailAlert");

                if (chkEmailAlert.Checked)
                {
                    intCustEmailRows = intCustEmailRows + 1;
                    strCustEmailBuilder.Append(" <DETAILS ");
                    strCustEmailBuilder.Append("Cust_Email_Det_ID='" + ((Label)grvCust.FindControl("lblCustEmailDetID")).Text + "'");
                    strCustEmailBuilder.Append(" ");
                    strCustEmailBuilder.Append("/>");
                }
            }
            strCustEmailBuilder.Append("</ROOT>");
            // return strbXml.ToString();
            return strCustEmailBuilder.ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    //opc042 end
    #endregion

}