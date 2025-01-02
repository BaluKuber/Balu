using AjaxControlToolkit;
//using CrystalDecisions.CrystalReports.Engine;
using S3GBusEntity;
using S3GBusEntity.FundManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Fund_Management_S3G_FundMgt_NoteCreation : ApplyThemeForProject
{

    FunderMgtServiceReference.FundMgtServiceClient objFundMgtServiceClient;
    FundMgtServices.S3G_Note_CreationDataTable objNoteDatatable;
    FundMgtServices.S3G_Note_CreationRow objNoteRow;
    int intCompanyID, intUserID = 0;
    string strMode = string.Empty;
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    long intTrancheId;
    int strNote_id;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string strDateFormat = string.Empty;
    //string strMode = string.Empty;
    static string strPageName = "Tranche Creation";
    static string strSuffix = "";
    FormsAuthenticationTicket Ticket;
    DataTable dtRentalSchedule = new DataTable();
    DataTable dtTranche;
    public static Fund_Management_S3G_FundMgt_NoteCreation obj_Page;
    DataRow dr;
    DataTable dtamort;
    DataSet dscashflow;
    DataTable dtcash;
    DataSet dsfunder;
    bool boolexists;
    DataSet dsprint;
    string strdue_date, strTenure, strFrquency;
    int d_cnt;
    DataSet dsasset;
    DataTable dttarget;
    string due_date;
    string Tenure;
    string frequency;

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Fund Management/S3G_Fund_Translander.aspx?Code=NOTE";
    string strRedirectPageAdd = "window.location.href='../Fund Management/S3G_FundMgt_NoteCreation.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../Fund Management/S3G_Fund_Translander.aspx?Code=NOTE';";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            S3GSession ObjS3GSession = null;
            //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            lblHeading.Text = "Note Creation";
            txtcheck.Style.Add("display", "none");
            //Date Format
            ObjS3GSession = new S3GSession();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtenderNotedate.Format = strDateFormat;
            CalendarExtenderNODDat.Format = strDateFormat;
            CalendarExtendertxtDisbursementdate.Format = strDateFormat;
            txtnotedate.Text = DateTime.Now.ToString(strDateFormat);
            txtnotedate.Attributes.Add("onblur", "fnDoDate(this,'" + txtnotedate.ClientID + "','" + strDateFormat + "',null,null);");
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
                strNote_id = Convert.ToInt32(fromTicket.Name);
            }

            strMode = Request.QueryString["qsMode"];


            if (!IsPostBack)
            {
                txtDisbursementdate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDisbursementdate.ClientID + "','" + strDateFormat + "',false,false);");
                FunPriLoadReportFormatTypes();

                if (strMode == "M")                     //Modify Mode
                {
                    hidsave.Value = "1";
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    ViewState["cnt"] = d_cnt;
                    FunPriLoadNoteDtls();
                    btnPrint.Enabled = false;
                    FunPriEnableDisableControls();

                }
                else if (strMode == "Q")                //Query Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    ViewState["cnt"] = d_cnt;
                    FunPriLoadNoteDtls();
                    FunPriEnableDisableControls();


                }
                else                                   //Create Mode
                {
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    d_cnt = 0;
                    ViewState["cnt"] = d_cnt;
                    btnPrint.Enabled = false;
                    FunPriEnableDisableControls();
                }

            }


        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriLoadBankDtls()
    {
        try
        {

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Funder_ID", Convert.ToString(ddlfund.SelectedValue));
            
            ddlFunderBank.BindDataTable("S3G_Get_FunderBank", Procparam, true, new string[] { "id", "name" });
           
            if (strMode == "C")
            {
                if (ddlFunderBank.Items.Count.ToString() == "2")
                    ddlFunderBank.SelectedIndex = 1;
            }

        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    //code added by vinodha.m for printing multiple format types
    private void FunPriLoadReportFormatTypes()
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        dictparam.Add("@company_ID", intCompanyID.ToString());
        DataTable dtreportformat = Utility.GetDefaultData("S3G_FM_GetNoteLookUpDtls", dictparam);
        ddlReportFormatType.FillDataTable(dtreportformat, "Value", "Name");
    }

    protected void ddlfund_OnItem_Selected(object sender, EventArgs e)
    {
        try
        {
            //FunPriClearDtl();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            cvTranche.ErrorMessage = "Due to Data Problem, Unable to Load Funder Bank Related Details";
            cvTranche.IsValid = false;
        }

    }

    private void FunPriLoadNoteDtls()
    {
        try
        {
            FunPriSetEmptyCashtbl();
            pnlcashflow.Visible = true;
            divcashflow.Style.Add("display", "block");
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Note_ID", Convert.ToString(strNote_id));
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@USER_ID", Convert.ToString(intUserID));
          //  ddlFunderState.Items.Insert(0, "--Select--");
            //ddlFunderState.BindDataTable("S3G_Rpt_Get_InvType", Procparam, new string[] { "Lookup_value", "Lookup_Name" });
            //ddlinvoice.SelectedValue = "0";
            DataSet dsFunder = Utility.GetDataset("S3G_FUNDMGT_GETNote", Procparam);
            ViewState["modify"] = dsFunder.Tables[1];
            if (dsFunder.Tables[0] != null)
            {
                ddlcust.SelectedValue = dsFunder.Tables[0].Rows[0]["Customer_ID"].ToString();
                ddlcust.SelectedText = dsFunder.Tables[0].Rows[0]["customer_name"].ToString();
                txtnotedate.Text = dsFunder.Tables[0].Rows[0]["Note_date"].ToString();
                txtDisbursementdate.Text = dsFunder.Tables[0].Rows[0]["Disbursement_date"].ToString();
                txtgraceDays.Text = dsFunder.Tables[0].Rows[0]["Grace_Days"].ToString();
                txtnote.Text = dsFunder.Tables[0].Rows[0]["Note_Number"].ToString();
                txtStatus.Text = dsFunder.Tables[0].Rows[0]["Status_id"].ToString();
                ddlfund.SelectedValue = dsFunder.Tables[0].Rows[0]["Funder_ID"].ToString();
                ddlfund.SelectedText = dsFunder.Tables[0].Rows[0]["funder_name"].ToString();
                ddlFunderState.SelectedValue = dsFunder.Tables[0].Rows[0]["Funder_StateID"].ToString();
                FunPriLoadBankDtls();
                //if(Convert.ToInt32(ddlFunderBank.SelectedValue.ToString())>0)
                ddlFunderBank.SelectedValue = dsFunder.Tables[0].Rows[0]["Funder_Bank_Id"].ToString();
               // ddlFunderBank.SelectedValue = "0";
                FunLoadRentalSchedule();
                btnSave.Enabled = true;

                if (dsFunder.Tables[0].Rows[0]["is_dsra"].ToString() == "1")
                {
                    CHKDSRA.Checked = true;
                }
                else
                {
                    CHKDSRA.Checked = false;
                }
            }

            if (dsFunder.Tables[2] != null)
            {
                if (dsFunder.Tables[2].Rows.Count > 0)
                {
                    pnlcashflow.Visible = true;
                    divcashflow.Style.Add("display", "block");
                    gvOutFlow.DataSource = dsFunder.Tables[2];
                    gvOutFlow.DataBind();
                    ViewState["dtcash"] = dsFunder.Tables[2];
                }

            }
            if (dsFunder.Tables[3] != null)
            {
                pnlFund.Visible = true;
                divfund.Style.Add("display", "block");
                grvfund.DataSource = dsFunder.Tables[3];
                grvfund.DataBind();
                ViewState["dtasset"] = dsFunder.Tables[3];
                btnCalculatePV.Visible = true;
            }
            if (dsFunder.Tables[4] != null)
            {
                divpv.Style.Add("display", "block");
                grvPV.DataSource = dsFunder.Tables[4];
                grvPV.DataBind();
                ViewState["dtamort"] = dsFunder.Tables[4];
                FunPriFindTotal();
            }
            if (dsFunder.Tables[5] != null)
            {
                ViewState["inst_date"] = dsFunder.Tables[5].Rows[0]["due_date"].ToString();
            }

            if (dsFunder.Tables[6] != null && dsFunder.Tables[6].Rows.Count > 0 && strMode == "M")
            {
                Utility.FunShowAlertMsg(this.Page, "Tranche has been changed, Please click on Process button and then Calculate PV");
                ViewState["CalculatePV"] = "0";
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void FunProGridDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataTable dtmodify;
            dtmodify = (DataTable)ViewState["modify"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (strMode != "C")
                {
                    Label lblTrancheid = (Label)e.Row.FindControl("lblTrancheid");
                    Label lblRS_Number = (Label)e.Row.FindControl("lblRS_Number");
                    CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectAccount");
                    DataRow[] dtrow = dtmodify.Select("tranche_id='" + lblTrancheid.Text + "'");
                    if (strMode == "Q" || txtStatus.Text == "Approved")
                        chkSelectAccount.Enabled = false;
                    if (dtrow.Length > 0)
                    {
                        chkSelectAccount.Checked = true;
                        dtTranche = (DataTable)ViewState["dtTranche"];
                        DataRow[] dtrows = dtTranche.Select("tranche_id='" + lblTrancheid.Text + "'");
                        DataTable dt = dtrows.CopyToDataTable();
                        dt.Columns.Add("Status");
                        if (dt.Rows.Count > 0)
                        {
                            if (ViewState["cnt"].ToString() == "0")
                            {
                                due_date = dt.Rows[0]["dayss"].ToString();
                                ViewState["due_date"] = due_date;
                                Tenure = dt.Rows[0]["Tenure"].ToString();
                                ViewState["Tenure"] = Tenure;
                                frequency = dt.Rows[0]["frequency"].ToString();
                                ViewState["frequency"] = frequency;
                            }
                            boolexists = true;
                            d_cnt = d_cnt + 1;
                            ViewState["cnt"] = d_cnt;
                            dtrows[0]["status"] = "1";
                            dtTranche.AcceptChanges();
                            ViewState["dtTranche"] = dtTranche;
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

    protected void grvtranche_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FunProGridDataBound(sender, e);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    protected void btnfetch_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlcust.SelectedValue) == 0 || Convert.ToString(ddlcust.SelectedText) == "")
            {
                cvTranche.ErrorMessage = "Select Lessee Name";
                cvTranche.IsValid = false;
                return;
            }

            if (Convert.ToInt32(ddlfund.SelectedValue) == 0 || Convert.ToString(ddlfund.SelectedText) == "")
            {
                cvTranche.ErrorMessage = "Select Funder";
                cvTranche.IsValid = false;
                return;
            }


            FunPriLoadBankDtls();
            pnlTranche.Visible = true;
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
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@customer_id", ddlcust.SelectedValue.ToString());
            Procparam.Add("@Funder_id", ddlfund.SelectedValue.ToString());
            Procparam.Add("@Note_id", strNote_id.ToString());
            dtTranche = Utility.GetDefaultData("S3G_Fund_NoteTranche", Procparam);
            ViewState["dtTranche"] = dtTranche;
            if (dtTranche.Rows.Count > 0)
            {
                pnlTranche.Visible = true;
                divacc.Style.Add("display", "block");
                grvtranche.DataSource = dtTranche;
                grvtranche.DataBind();
                btnProcess.Visible = true;
            }
            else
            {
                grvtranche.DataSource = null;
                grvtranche.DataBind();
            }
            divacc.Style.Add("display", "block");


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
            //if (strMode == "C")
            //{

            //Added on 23Mar2015 starts here

            pnlcashflow.Visible = btnSave.Enabled = btnCalculatePV.Visible = pnlFund.Visible = false;
            divfund.Style.Add("display", "block");
            grvfund.DataSource = null;
            grvfund.DataBind();
            ViewState["dtasset"] = null;
            FunPriSetEmptyCashtbl();

            //Added on 23Mar2015 ends here

            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            Label lblTrancheid = (Label)gvr.FindControl("lblTrancheid");
            string Trancheid = lblTrancheid.Text.ToString();
            dtTranche = (DataTable)ViewState["dtTranche"];
            DataRow[] dtrow = dtTranche.Select("Tranche_id='" + Trancheid + "'");
            DataTable dt = dtrow.CopyToDataTable();
            dt.Columns.Add("Status");
            if (dt.Rows.Count > 0)
            {
                if (btn.Checked)
                {
                    if (ViewState["cnt"].ToString() == "0")
                    {
                        due_date = dt.Rows[0]["dayss"].ToString();
                        ViewState["due_date"] = due_date;
                        Tenure = dt.Rows[0]["Tenure"].ToString();
                        ViewState["Tenure"] = Tenure;
                        frequency = dt.Rows[0]["frequency"].ToString();
                        ViewState["frequency"] = frequency;

                    }
                    else
                    {
                        if (ViewState["cnt"].ToString() != "-1")
                        {
                            if (dt.Rows[0]["dayss"].ToString() != ViewState["due_date"].ToString())
                            {
                                Utility.FunShowAlertMsg(this.Page, "The selected Rental Schedule should have same Due_Date");
                                btn.Checked = false;
                                return;
                            }
                            if (dt.Rows[0]["Tenure"].ToString() != ViewState["Tenure"].ToString())
                            {
                                Utility.FunShowAlertMsg(this.Page, "The selected Rental Schedule should have same Tenure");
                                btn.Checked = false;
                                return;
                            }
                            if (dt.Rows[0]["frequency"].ToString() != ViewState["frequency"].ToString())
                            {
                                Utility.FunShowAlertMsg(this.Page, "The selected Rental Schedule should have same Frequency");
                                btn.Checked = false;
                                return;
                            }
                        }
                    }
                    boolexists = true;
                    d_cnt = d_cnt + 1;
                    ViewState["cnt"] = d_cnt;
                    dtrow[0]["status"] = "1";
                    dtTranche.AcceptChanges();
                    ViewState["dtTranche"] = dtTranche;
                }
                else
                {
                    d_cnt = d_cnt - 1;
                    ViewState["cnt"] = (d_cnt == -1) ? 0 : d_cnt;
                    dtrow[0]["status"] = "0";
                    dtTranche.AcceptChanges();
                    ViewState["dtTranche"] = dtTranche;

                }
            }
            //}
            //else
            //{

            //}
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

            FunPriSetEmptyCashtbl();

            if (ViewState["dtTranche"] == null)
                return;

            dtTranche = (DataTable)ViewState["dtTranche"];

            DataRow[] drTranch = dtTranche.Select("Status = 1");
            if (drTranch.Length == 0)
            {
                pnlcashflow.Visible = btnSave.Enabled = btnCalculatePV.Visible = pnlFund.Visible = false;
                divcashflow.Style.Add("display", "none");
                grvfund.DataSource = null;
                grvfund.DataBind();
                ViewState["dtasset"] = null;
                cvTranche.ErrorMessage = "Select Tranche Details before process";
                cvTranche.IsValid = false;
                return;
            }
            foreach (GridViewRow row in grvPV.Rows)
            {
                TextBox txtdsra = (TextBox)row.FindControl("txtDsra");
                if (CHKDSRA.Checked)
                {
                    txtdsra.Enabled = true;
                }
                else
                {
                    txtdsra.Enabled = false;
                }
            }

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@Acc_XMl", dtTranche.FunPubFormXml());
            //if (strMode == "C")
            Procparam.Add("@Note_Number", "RS");
            DataTable dt = Utility.GetDefaultData("S3G_Fund_CHK_NoteSanction", Procparam);
            if (dt.Rows.Count > 0)
            {
                pnlcashflow.Visible = btnSave.Enabled = btnCalculatePV.Visible = pnlFund.Visible = false;
                divfund.Style.Add("display", "none");
                grvfund.DataSource = null;
                grvfund.DataBind();
                ViewState["dtasset"] = null;
                Utility.FunShowAlertMsg(this.Page, "The selected Tranche is not mapped to sanction");
                return;
            }

            pnlcashflow.Visible = btnSave.Enabled = true;
            divcashflow.Style.Add("display", "block");

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@Acc_XMl", dtTranche.FunPubFormXml());
            dsasset = Utility.GetDataset("S3G_Fund_Get_NoteAsset", Procparam);
            if (dsasset.Tables[0].Rows.Count > 0)
            {
                btnCalculatePV.Visible = true;
                pnlFund.Visible = true;
                divfund.Style.Add("display", "block");
                grvfund.DataSource = dsasset.Tables[0];
                grvfund.DataBind();
                ViewState["dtasset"] = dsasset.Tables[0];
            }
            if (dsasset.Tables[1].Rows.Count > 0)
            {
                dtcash = dsasset.Tables[1];

                gvOutFlow.DataSource = dsasset.Tables[1];
                gvOutFlow.DataBind();
                ViewState["dtcash"] = dsasset.Tables[1];
            }
            FunPriAddProcessingCashFlow();

            if (dsasset.Tables[2].Rows.Count > 0)
            {
                string inst_date = dsasset.Tables[2].Rows[0]["inst_date"].ToString();
                ViewState["inst_date"] = inst_date;
            }

            if (ViewState["inst_date"] != null)
            {
                if (txtDisbursementdate.Text != "")
                {
                    if (txtgraceDays.Text != "")
                        ViewState["inst_date"] = Utility.StringToDate(ViewState["inst_date"].ToString()).AddDays(Convert.ToInt32(txtgraceDays.Text)).ToString();

                    if (Utility.StringToDate(ViewState["inst_date"].ToString()) < Utility.StringToDate(txtDisbursementdate.Text.ToString()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Disbursement Date cannot be greater than First Due Date");
                        txtDisbursementdate.Text = "";
                        return;

                    }
                }
            }
            // To bind PV Grid when click process button start here 
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@Acc_XMl", dtTranche.FunPubFormXml());
            Procparam.Add("@san_XMl", grvfund.FunPubFormXml());
            Procparam.Add("@Grace_Days", (Convert.ToString(txtgraceDays.Text) == "") ? "0" : Convert.ToString(txtgraceDays.Text));
            //if (strMode == "C")
            Procparam.Add("@Note_Number", "RS");
            if (txtDisbursementdate.Text != "")
                Procparam.Add("@disb_date", Utility.StringToDate(txtDisbursementdate.Text).ToString());
            DataTable dtstruct = new DataTable();
            DataSet dsstruct = Utility.GetDataset("S3G_Fund_Get_NoteStruct", Procparam);
            pnlpv.Visible = true;
            dtstruct = dsstruct.Tables[0];
            if (dtstruct.Rows.Count > 0)
            {
                ViewState["dtamort"] = null;
                divpv.Style.Add("display", "block");
                grvPV.DataSource = dtstruct;
                grvPV.DataBind();
                ViewState["dtstruct"] = dtstruct;
                FunPriFindTotal();
            }
            //End here 

        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }
    private void FunLoadDDLControls()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
               Procparam = new Dictionary<string, string>();
          
          Procparam.Add("@Company_ID", intCompanyID.ToString());
           // Procparam.Add("@Company_ID", Convert.ToString(ddlFunderState.SelectedValue));

            DataSet ds = Utility.GetDataset("S3G_SYSAD_GetStateLookup", Procparam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)//Delivery State
                {
                    ddlFunderState.FillDataTable(ds.Tables[0], "Location_Category_ID", "LocationCat_Description", true);
                    ddlFunderState.SelectedValue = "172";
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected void btnview_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(ddlfund.SelectedValue) == "" || Convert.ToInt64(ddlfund.SelectedValue) == 0 || Convert.ToString(ddlcust.SelectedValue) == "" || Convert.ToInt64(ddlcust.SelectedValue) == 0)
            {
                return;
            }
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(Convert.ToString(strNote_id), false, 0);
            //FormsAuthenticationTicket Lessee = new FormsAuthenticationTicket(ddlcust.SelectedValue.ToString(), false, 0);
            //hdnID.Value = "../Fund Management/S3G_ORG_Funder_Master.aspx?qsFunderId=" + FormsAuthentication.Encrypt(Ticket) + "&qsCustomerId=" + FormsAuthentication.Encrypt(Lessee) + "&qsMode=Q&Popup=yes";

            hdnID.Value = "../Fund Management/S3G_ORG_Funder_Master.aspx?qsNoteId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q&Popup=yes";

            if (ddlfund.SelectedValue != "0" && hdnID.Value.Length > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + hdnID.Value + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Funder');", true);
                return;
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    private void FunPriFindTotal()
    {
        try
        {
            if (ViewState["dtamort"] != null)
            {
                dttarget = (System.Data.DataTable)ViewState["dtamort"];
            }
            else
            {
                dttarget = (System.Data.DataTable)ViewState["dtstruct"];

            }

            if (dttarget.Rows.Count > 0)
            {
                Label Totlblrental = (Label)(grvPV).FooterRow.FindControl("Totlblrental") as Label;
                Totlblrental.Text = Convert.ToDecimal(dttarget.Compute("sum(Rental1)", "")).ToString(Funsetsuffix());

                Label TotlblAMF = (Label)(grvPV).FooterRow.FindControl("TotlblAMF") as Label;
                TotlblAMF.Text = Convert.ToDecimal(dttarget.Compute("sum(AMF1)", "")).ToString(Funsetsuffix());

                Label TotlblVAT = (Label)(grvPV).FooterRow.FindControl("TotlblVAT") as Label;
                TotlblVAT.Text = Convert.ToDecimal(dttarget.Compute("sum(VAT1)", "")).ToString(Funsetsuffix());

                Label TotlblServiceTax = (Label)(grvPV).FooterRow.FindControl("TotlblServiceTax") as Label;
                TotlblServiceTax.Text = Convert.ToDecimal(dttarget.Compute("sum(ServiceTax1)", "")).ToString(Funsetsuffix());

                Label TotlblPVAmount = (Label)(grvPV).FooterRow.FindControl("TotlblPVAmount") as Label;
                TotlblPVAmount.Text = Convert.ToDecimal(dttarget.Compute("sum(PV_Amount1)", "")).ToString(Funsetsuffix());

                Label TotlblPrincipal = (Label)(grvPV).FooterRow.FindControl("TotlblPrincipal") as Label;
                TotlblPrincipal.Text = Convert.ToDecimal(dttarget.Compute("sum(Principal1)", "")).ToString(Funsetsuffix());

                Label TotlblInterest = (Label)(grvPV).FooterRow.FindControl("TotlblInterest") as Label;
                TotlblInterest.Text = Convert.ToDecimal(dttarget.Compute("sum(Interest1)", "")).ToString(Funsetsuffix());

                Label TotlblBalance = (Label)(grvPV).FooterRow.FindControl("TotlblBalance") as Label;
                TotlblBalance.Text = Convert.ToDecimal(dttarget.Compute("sum(balance1)", "")).ToString(Funsetsuffix());
                //By Siva.K 25MAY2015 For Funder Amortization  

                Label lblTotalFPrincipal = (Label)(grvPV).FooterRow.FindControl("lblTotalFPrincipal") as Label;
                lblTotalFPrincipal.Text = Convert.ToDecimal(dttarget.Compute("sum(Funder_Principal1)", "")).ToString(Funsetsuffix());

                Label lblTotalFInterest = (Label)(grvPV).FooterRow.FindControl("lblTotalFInterest") as Label;
                lblTotalFInterest.Text = Convert.ToDecimal(dttarget.Compute("sum(Funder_Interest1)", "")).ToString(Funsetsuffix());

                Label lblTotalFBalance = (Label)(grvPV).FooterRow.FindControl("lblTotalFBalance") as Label;
                lblTotalFBalance.Text = Convert.ToDecimal(dttarget.Compute("sum(Funder_balance1)", "")).ToString(Funsetsuffix());

                Label lblTOtDsra = (Label)(grvPV).FooterRow.FindControl("lblTOtDsra") as Label;
                lblTOtDsra.Text = Convert.ToDecimal(dttarget.Compute("sum(DSRA1)", "")).ToString(Funsetsuffix());

                //Label lblTotalPVFactor = (Label)(grvPV).FooterRow.FindControl("lblTotalPVFactor") as Label;
                //lblTotalPVFactor.Text = Convert.ToDecimal(dttarget.Compute("sum(Factor_Value)", "")).ToString(Funsetsuffix());
            }
        }
        catch (Exception ex)
        {
            cvTranche.ErrorMessage = "Due to Data Problem,Unable to Move Invoices.";
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

    protected void btnCalculatePV_OnClick(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gvRow in grvPV.Rows)
            {
                TextBox txtDsra = (TextBox)gvRow.FindControl("txtDsra");
                if (txtDsra.Text == string.Empty)
                {
                    txtDsra.Text = "0.00";
                }
            }





            if (grvfund.Rows.Count.ToString() == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Please Click Process Button to Proceed");
                return;
            }
            if (txtDisbursementdate.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Select Disbursement Date");
                return;
            }
            if (ViewState["inst_date"] != null)
            {
                if (txtDisbursementdate.Text != "")
                {
                    if (Utility.StringToDate(ViewState["inst_date"].ToString()) < Utility.StringToDate(txtDisbursementdate.Text.ToString()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Disbursement Date cannot be greater than First Due Date");
                        txtDisbursementdate.Text = "";
                        return;

                    }
                }
            }

            dtTranche = (DataTable)ViewState["dtTranche"];
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@user_id", intUserID.ToString());
            Procparam.Add("@Acc_XMl", dtTranche.FunPubFormXml());
            Procparam.Add("@san_XMl", grvfund.FunPubFormXml());
            Procparam.Add("@PV_XML", grvPV.FunPubFormXml());
            Procparam.Add("@Grace_Days", (Convert.ToString(txtgraceDays.Text) == "") ? "0" : Convert.ToString(txtgraceDays.Text));
            //if (strMode == "C")
            Procparam.Add("@Note_Number", "RS");
            if (txtDisbursementdate.Text != "")
                Procparam.Add("@disb_date", Utility.StringToDate(txtDisbursementdate.Text).ToString());
            DataSet dsAmort = Utility.GetDataset("S3G_Fund_Get_NoteAmort", Procparam);
            pnlpv.Visible = true;
            dtamort = dsAmort.Tables[0];
            if (dtamort.Rows.Count > 0)
            {
                divpv.Style.Add("display", "block");
                grvPV.DataSource = dtamort;
                grvPV.DataBind();
                ViewState["dtamort"] = dtamort;
                FunPriFindTotal();
            }

            if (dsAmort.Tables[1].Rows.Count > 0)
            {
                DataTable dtpv = new DataTable();
                dtpv = dsAmort.Tables[1];
                foreach (GridViewRow gvRow in grvfund.Rows)
                {
                    Label lblsanction_id = (Label)gvRow.FindControl("lblsanction_id");
                    TextBox lblProcessingFeeAmount = (TextBox)gvRow.FindControl("lblProcessingFeeAmount");
                    Label lbldiscountamount = (Label)gvRow.FindControl("lbldiscountamount");
                    Label lblPFSTPerc = (Label)gvRow.FindControl("lblPFSTPerc");
                    Label lblPFServiceTax = (Label)gvRow.FindControl("lblPFServiceTax");
                    TextBox lblforeclosurerate = (TextBox)gvRow.FindControl("lblforeclosurerate");
                    TextBox lblDiscountrate = (TextBox)gvRow.FindControl("lblDiscountrate");
                    TextBox lblChq_Rtn_Charges = (TextBox)gvRow.FindControl("lblChq_Rtn_Charges");
                    TextBox lblODI_Rate = (TextBox)gvRow.FindControl("lblODI_Rate");
                    TextBox lblMisc_Charges = (TextBox)gvRow.FindControl("lblMisc_Charges");

                    DataRow[] dr = dtpv.Select("sanction_number = '" + lblsanction_id.Text + "'");
                    if (dr.Length > 0)
                    {
                        lbldiscountamount.Text = Convert.ToString(dr[0]["PV_Amount"]);
                        lblProcessingFeeAmount.Text = Convert.ToString(Math.Round(Convert.ToDouble(dr[0]["PV_Amount"]) * (Convert.ToDouble(dr[0]["PF_Perc"]) / 100), 2));
                        lblPFServiceTax.Text = Convert.ToString(Math.Round(Convert.ToDouble(lblProcessingFeeAmount.Text) * (Convert.ToDouble(lblPFSTPerc.Text) / 100), 2));

                        DataTable dtAst = (DataTable)ViewState["dtasset"];
                        DataRow[] drAst = dtAst.Select("sanction_id ='" + lblsanction_id.Text + "'");
                        if (drAst.Length > 0)
                        {
                            drAst[0]["Foreclosure_Rate"] = (Convert.ToString(lblforeclosurerate.Text).Trim() == "") ? 0 : Convert.ToDecimal(lblforeclosurerate.Text);
                            drAst[0]["discount_rate"] = (Convert.ToString(lblDiscountrate.Text).Trim() == "") ? 0 : Convert.ToDecimal(lblDiscountrate.Text);
                            drAst[0]["Chq_Rtn_Charges"] = (Convert.ToString(lblChq_Rtn_Charges.Text).Trim() == "") ? 0 : Convert.ToDecimal(lblChq_Rtn_Charges.Text);
                            drAst[0]["ODI_Rate"] = (Convert.ToString(lblODI_Rate.Text).Trim() == "") ? 0 : Convert.ToDecimal(lblODI_Rate.Text);
                            drAst[0]["Processing_Fee_Amount"] = Convert.ToDecimal(lblProcessingFeeAmount.Text);
                            drAst[0]["Misc_Charges"] = (Convert.ToString(lblMisc_Charges.Text).Trim() == "") ? 0 : Convert.ToDecimal(lblMisc_Charges.Text);
                            drAst[0]["PF_Service_Tax"] = Convert.ToDecimal(lblPFServiceTax.Text);
                            drAst[0]["Discount_amount"] = Convert.ToDecimal(lbldiscountamount.Text);
                            dtAst.AcceptChanges();
                            ViewState["dtasset"] = dtAst;
                        }
                    }
                }

                grvfund.DataSource = (DataTable)ViewState["dtasset"];
                grvfund.DataBind();

                FunPriAddProcessingCashFlow();

                ViewState["CalculatePV"] = "1";
                hidsave.Value = "1";//Assigning value here when calculating PV for status
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = "Error in Calculate PV";
            cvTranche.IsValid = false;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hidsave.Value == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Details modified. Please click Calculate PV and then proceed");
                return;
            }
            if (grvPV.Rows.Count.ToString() == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Details modified. Please click Calculate PV and then proceed");
                return;
            }

            if (ViewState["CalculatePV"] != null && ViewState["CalculatePV"].ToString() == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Tranche has been changed, Please click on Process button and then Calculate PV");
                return;
            }

            objNoteDatatable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_Note_CreationDataTable();
            objNoteRow = objNoteDatatable.NewS3G_Note_CreationRow();
            objNoteRow.Company_id = intCompanyID.ToString();
            objNoteRow.User_id = intUserID.ToString();
            objNoteRow.Note_Header_id = strNote_id;
            objNoteRow.Funder_id = Convert.ToInt32(ddlfund.SelectedValue);
            objNoteRow.Customer_id = Convert.ToInt32(ddlcust.SelectedValue);
            objNoteRow.Location_ID = Convert.ToInt32(ddlFunderState.SelectedValue);
            objNoteRow.Grace_days = (Convert.ToString(txtgraceDays.Text) == "") ? "0" : Convert.ToString(txtgraceDays.Text);
            objNoteRow.Funder_bank_id = (ddlFunderBank.Items != null && ddlFunderBank.Items.Count > 0) ? ddlFunderBank.SelectedValue : "0";
            objNoteRow.Note_Number = txtnote.Text;
            objNoteRow.Note_date = (Utility.StringToDate(txtnotedate.Text.ToString())).ToString();
            objNoteRow.NOD_Date = (Utility.StringToDate(DateTime.Now.ToString())).ToString();
            objNoteRow.Disb_Date = (Utility.StringToDate(txtDisbursementdate.Text.ToString())).ToString();
            if (ViewState["dtcash"] != null && ((DataTable)ViewState["dtcash"]).Rows.Count > 0)
            {
                objNoteRow.XML_Cashflow = Utility.FunPubFormXml((DataTable)ViewState["dtcash"]);
            }
            else
            {
                objNoteRow.XML_Cashflow = null;
            }

            if (ViewState["dtTranche"] != null && ((DataTable)ViewState["dtTranche"]).Rows.Count > 0)
            {
                objNoteRow.XML_Tranche = Utility.FunPubFormXml((DataTable)ViewState["dtTranche"], true);
            }
            else
            {
                objNoteRow.XML_Tranche = null;
            }
            if (ViewState["dtasset"] != null && ((DataTable)ViewState["dtasset"]).Rows.Count > 0)
            {
                objNoteRow.XML_Sanction = grvfund.FunPubFormXml();
            }
            else
            {
                objNoteRow.XML_Sanction = null;
            }
            if (CHKDSRA.Checked)
            {
                objNoteRow.IS_DSRA = 1;
            }
            else
            {
                objNoteRow.IS_DSRA = 0;
            }

            objNoteDatatable.AddS3G_Note_CreationRow(objNoteRow);

            objFundMgtServiceClient = new FunderMgtServiceReference.FundMgtServiceClient();
            string strNoteCode, strErrorMsg = string.Empty;
            Int64 intNoteNo = 0;
            string StrNoteNumber = string.Empty;
            int iErrorCode = objFundMgtServiceClient.FunPubCreateOrModifyNote(out intNoteNo, out strErrorMsg, out StrNoteNumber, ObjSerMode, ClsPubSerialize.Serialize(objNoteDatatable, ObjSerMode));
            switch (iErrorCode)
            {
                case 0:
                    btnSave.Enabled = false;
                    if (strNote_id == 0)
                    {
                        strAlert = "Note Creation " + StrNoteNumber + " Created successfully";
                        strAlert += @"\n\nWould you like to Create one more Note Creation?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPage = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        lblErrorMessage.Text = string.Empty;
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Note Creation Details updated successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                        lblErrorMessage.Text = string.Empty;
                        btnSave.Enabled = false;
                    }
                    break;
                case 1:
                    Utility.FunShowAlertMsg(this.Page, "Document Number Not Defined");
                    break;
                case 2:
                    Utility.FunShowAlertMsg(this.Page, "Rental Schedule details has been changed, Re-arrive the Tranche and proceed.");
                    break;
                case 9:
                    Utility.FunShowAlertMsg(this.Page, "Selected Tranche is already mapped. Please select any other tranche.");
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
            }
            else
            {
                Response.Redirect(strRedirectPage);
            }
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
            FunPriClearDtl();
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
            ddlcust.Clear();
            ddlfund.Clear();
            txtnote.Text = txtnotedate.Text = txtDisbursementdate.Text = string.Empty;
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriClearDtl()
    {
        try
        {
            pnlcashflow.Visible = pnlFund.Visible = pnlTranche.Visible = pnlpv.Visible = btnSave.Enabled = btnCalculatePV.Visible = btnProcess.Visible = false;
            ViewState["dtamort"] = null;
            ViewState["modify"] = null;
            ViewState["dtasset"] = null;
            ViewState["dtcash"] = null;
            ViewState["dtTranche"] = null;
            ViewState["due_date"] = null;
            ViewState["cnt"] = "0";
            ViewState["Tenure"] = null;
            ViewState["frequency"] = null;
            ddlFunderBank.Items.Clear();
            ddlFunderState.SelectedValue = "0";
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlReportFormatType.SelectedIndex > 0)
            {
                dtTranche = (DataTable)ViewState["dtTranche"];
                if (Procparam != null)
                    Procparam.Clear();
                else

                    Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                Procparam.Add("@user_id", intUserID.ToString());
                Procparam.Add("@Acc_XMl", dtTranche.FunPubFormXml());
                Procparam.Add("@customer_id", ddlcust.SelectedValue);
                Procparam.Add("@funder_id", ddlfund.SelectedValue);
                Procparam.Add("@Note_date", (Utility.StringToDate(txtnotedate.Text.ToString())).ToString());
                Procparam.Add("@TypeID", ddlReportFormatType.SelectedValue);
                Procparam.Add("@Note_ID", strNote_id.ToString());
                Session["ProcParam"] = Procparam;
                Session["Format_Type"] = ddlReportFormatType.SelectedValue;//Format_Type
                string strScipt = "window.open('../Fund Management/S3G_RPT_Note_ReportPage.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Note Creation", strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Select Report Format");
                return;
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

                dtcash = (DataTable)ViewState["dtcash"];

                if (dtcash.Rows.Count == 1)
                {
                    if (dtcash.Rows[0]["CashFlow_Type_id"].ToString() == string.Empty)
                        dtcash.Rows[0].Delete();
                }

                drEmptyRow = dtcash.NewRow();

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

                dtcash.Rows.Add(drEmptyRow);
                ViewState["dtcash"] = dtcash;
                gvOutFlow.DataSource = dtcash;
                gvOutFlow.DataBind();
            }
        }
        catch (Exception)
        {
            throw;
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
                if (ViewState["dscashflow"] == null)
                {
                    if (Procparam != null)
                        Procparam.Clear();
                    else
                        Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_ID", intCompanyID.ToString());
                    Procparam.Add("@user_id", intUserID.ToString());
                    dscashflow = Utility.GetDataset("S3G_Fund_Getcashflow", Procparam);
                    ViewState["dscashflow"] = dscashflow;
                }

                dscashflow = (DataSet)ViewState["dscashflow"];
                ddlOutflowDesc.FillDataTable(dscashflow.Tables[0], "id", "Flag_Desc");
                ddloutflowtype.FillDataTable(dscashflow.Tables[1], "id", "description");
                ddlPaymentto_OutFlow.FillDataTable(dscashflow.Tables[2], "id", "description");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void FunPriSetEmptyCashtbl()
    {
        try
        {
            DataRow drEmptyRow;
            dtcash = new DataTable();
            dtcash.Columns.Add("Date");
            dtcash.Columns.Add("CashFlow_Type");
            dtcash.Columns.Add("CashFlow_Type_id");
            dtcash.Columns.Add("CashOutFlow");
            dtcash.Columns.Add("CashOutFlow_id");
            dtcash.Columns.Add("OutflowFrom");
            dtcash.Columns.Add("OutflowFrom_id");
            dtcash.Columns.Add("Entity");
            dtcash.Columns.Add("Entity_id");
            dtcash.Columns.Add("Amount");
            drEmptyRow = dtcash.NewRow();
            dtcash.Rows.Add(drEmptyRow);
            ViewState["dtcash"] = dtcash;
            FunFillgrid(gvOutFlow, dtcash);
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

    protected void gvOutFlow_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtcash = (DataTable)ViewState["dtcash"];

            dtcash.Rows.RemoveAt(e.RowIndex);

            ViewState["dtcash"] = dtcash;
            gvOutFlow.DataSource = dtcash;
            gvOutFlow.DataBind();
            if (dtcash.Rows.Count == 0)
            {
                FunPriSetEmptyCashtbl();
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void FunPriEnableDisableControls()
    {
        try
        {
            FunLoadDDLControls();
            if (strMode == "Q")
            {
               
                ddlcust.ReadOnly = ddlfund.ReadOnly = txtStatus.ReadOnly = txtnotedate.ReadOnly = txtDisbursementdate.ReadOnly =
                txtgraceDays.ReadOnly = true;
                gvOutFlow.Columns[6].Visible = false;
                btnProcess.Enabled = btnCalculatePV.Visible = btnfetch.Enabled = btnClear.Enabled = btnSave.Enabled =
                CalendarExtendertxtDisbursementdate.Enabled = false;
                foreach (GridViewRow grv in grvfund.Rows)
                {
                    TextBox lblforeclosurerate = (TextBox)grv.FindControl("lblforeclosurerate");
                    TextBox lblDiscountrate = (TextBox)grv.FindControl("lblDiscountrate");
                    TextBox lblChq_Rtn_Charges = (TextBox)grv.FindControl("lblChq_Rtn_Charges");
                    TextBox lblODI_Rate = (TextBox)grv.FindControl("lblODI_Rate");
                    TextBox lblProcessing_Fee_Amount = (TextBox)grv.FindControl("lblProcessingFeeAmount");
                    TextBox lblMisc_Charges = (TextBox)grv.FindControl("lblMisc_Charges");
                    lblforeclosurerate.ReadOnly = lblDiscountrate.ReadOnly = lblChq_Rtn_Charges.ReadOnly = lblODI_Rate.ReadOnly = lblProcessing_Fee_Amount.ReadOnly = lblMisc_Charges.ReadOnly = true;
                }
                gvOutFlow.FooterRow.Visible = false;
                ddlFunderBank.ClearDropDownList();
                ddlFunderState.ClearDropDownList();
            }
            else if (Convert.ToString(strMode) == "M")
            {
                if (txtStatus.Text == "Approved")
                {
                    ddlFunderState.ClearDropDownList();
                    ddlFunderBank.ClearDropDownList();
                    btnSave.Enabled = false;
                    txtStatus.ReadOnly = txtnotedate.ReadOnly = txtDisbursementdate.ReadOnly = txtgraceDays.ReadOnly = true;
                    gvOutFlow.Columns[6].Visible = false;
                    btnProcess.Enabled = btnCalculatePV.Visible = btnfetch.Enabled =
                    CalendarExtendertxtDisbursementdate.Enabled = false;
                    foreach (GridViewRow grv in grvfund.Rows)
                    {
                        TextBox lblforeclosurerate = (TextBox)grv.FindControl("lblforeclosurerate");
                        TextBox lblDiscountrate = (TextBox)grv.FindControl("lblDiscountrate");
                        TextBox lblChq_Rtn_Charges = (TextBox)grv.FindControl("lblChq_Rtn_Charges");
                        TextBox lblODI_Rate = (TextBox)grv.FindControl("lblODI_Rate");
                        TextBox lblProcessing_Fee_Amount = (TextBox)grv.FindControl("lblProcessingFeeAmount");
                        TextBox lblMisc_Charges = (TextBox)grv.FindControl("lblMisc_Charges");
                        lblforeclosurerate.ReadOnly = lblDiscountrate.ReadOnly = lblChq_Rtn_Charges.ReadOnly = lblODI_Rate.ReadOnly = lblProcessing_Fee_Amount.ReadOnly = lblMisc_Charges.ReadOnly = true;
                    }
                    gvOutFlow.FooterRow.Visible = false;
                }
                ddlcust.ReadOnly = ddlfund.ReadOnly = true;
                btnClear.Enabled = false;
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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            FunProExport(grvPV, "PV");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void btnRSExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Note_Id", strNote_id.ToString());

            DataTable dsAmort = Utility.GetDefaultData("S3G_FM_Get_Note_RSExport", Procparam);

            if (dsAmort.Rows.Count > 0)
            {
                string attachment = "attachment; filename=RSAmort.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView Grv = new GridView();
                Grv.ForeColor = System.Drawing.Color.DarkBlue;
                Grv.Font.Name = "calibri";
                Grv.Font.Size = 10;

                Grv.DataSource = dsAmort;
                Grv.DataBind();

                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProExport(GridView Grv, string FileName)
    {
        try
        {
            //Type ExcellType = Type.GetTypeFromProgID("Excel.Application");
            //if (ExcellType == null)
            //{
            //Utility.FunShowAlertMsg(this, "Cannot export file. MS-Excel is not istalled in this System.");
            //return;
            //}
            if (Grv.Rows.Count > 0)
            {
                Grv.Font.Size = 10;
                string attachment = "attachment; filename=" + FileName + ".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                foreach (GridViewRow row in Grv.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        List<Control> controls = new List<Control>();

                        foreach (Control control in cell.Controls)
                        {
                            controls.Add(control);
                        }

                        foreach (Control control in controls)
                        {
                            switch (control.GetType().Name)
                            {
                                case "TextBox":
                                    cell.Controls.Add(new Literal { Text = (control as TextBox).Text });
                                    break;
                                case "Label":
                                    cell.Controls.Add(new Literal { Text = (control as Label).Text });
                                    break;
                            }
                            cell.Controls.Remove(control);
                        }
                    }
                }
                
                Grv.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Export into Excel");
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void grvfund_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtForeClosureRate = (TextBox)e.Row.FindControl("lblforeclosurerate");
                TextBox txtDiscountRate = (TextBox)e.Row.FindControl("lblDiscountrate");
                TextBox txtChqRtnCharge = (TextBox)e.Row.FindControl("lblChq_Rtn_Charges");
                TextBox txtODIRate = (TextBox)e.Row.FindControl("lblODI_Rate");
                TextBox txtPFAmt = (TextBox)e.Row.FindControl("lblProcessingFeeAmount");
                TextBox txtMiscCharges = (TextBox)e.Row.FindControl("lblMisc_Charges");

                txtForeClosureRate.SetPercentagePrefixSuffix(2, 15, false, false, "Fore Closure Rate");
                txtDiscountRate.SetPercentagePrefixSuffix(2, 15, true, false, "Discounting Rate");
                txtChqRtnCharge.SetDecimalPrefixSuffix(10, 2, false, false, "Cheque Return Charges");
                txtODIRate.SetDecimalPrefixSuffix(2, 2, false, false, "ODI Rate");
                txtPFAmt.SetDecimalPrefixSuffix(13, 2, false, false, "Processing Fee Amount");
                txtMiscCharges.SetDecimalPrefixSuffix(10, 2, false, false, "Misc Charges");
            }
        }
        catch (Exception objException)
        {
        }
    }

    protected void grvpv_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDsra = (TextBox)e.Row.FindControl("txtDsra");
                txtDsra.SetDecimalPrefixSuffix(13, 2, false, false, "DSRA");
                txtDsra.Attributes.Remove("onpaste");
                //txtDsra.Attributes.Add("onblur", "funChkDecimial(this,'" + 13 + "','" + 2 + "','" + "DSRA" + "',true);");


                if (CHKDSRA.Checked)
                {
                    txtDsra.Enabled = true;
                }
                else
                {
                    txtDsra.Enabled = false;
                    txtDsra.Text = "0.00";
                    //Label lblTOtDsra = grvPV.FooterRow.FindControl("lblTOtDsra") as Label;
                    //lblTOtDsra.Text = "0.00";
                }

                if (txtStatus.Text == "Approved" || strMode == "Q")
                {
                    CHKDSRA.Enabled = false;
                    txtDsra.Enabled = false;
                }


            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (CHKDSRA.Checked == false)
                {
                    Label lblTOtDsra = (Label)e.Row.FindControl("lblTOtDsra");
                    lblTOtDsra.Text = "0.00";
                }


            }


        }
        catch (Exception objException)
        {
        }

    }
    private void FunPriAddProcessingCashFlow()
    {
        try
        {
            DataTable dtCF;
            DataRow dr;
            DataTable dtasset = (DataTable)ViewState["dtasset"];

            Double dblProcFeeAmt = Convert.ToDouble(dtasset.Compute("Sum(Processing_Fee_Amount)", "Processing_Fee_Amount>=0"));
            Double dblServiceTax = Convert.ToDouble(dtasset.Compute("Sum(PF_Service_Tax)", "PF_Service_Tax>=0"));
            if (ViewState["dtcash"] != null)
            {
                dtCF = (DataTable)ViewState["dtcash"];
            }
            else
            {
                dtCF = dtcash.Clone();
            }
            if (dtCF.Rows.Count > 0 && Convert.ToString(dtCF.Rows[0]["CashFlow_Type_id"]) == "")
            {
                dtCF.Rows[0].Delete();
            }
            if (dblProcFeeAmt > 0)
            {
                DataRow[] drCf = dtCF.Select("CashOutFlow_id = '" + 144 + "'");
                if (drCf.Length > 0)
                {
                    drCf[0]["Amount"] = dblProcFeeAmt;
                    dtCF.AcceptChanges();
                }
                else
                {
                    dr = dtCF.NewRow();
                    dr["CashFlow_Type_id"] = "2";
                    dr["CashFlow_Type"] = "Outflow";
                    dr["CashOutFlow_id"] = 144;
                    dr["Date"] = Convert.ToString(DateTime.Now.ToString(strDateFormat));
                    dr["CashOutFlow"] = "Processing Fee";
                    dr["OutflowFrom_id"] = "1";
                    dr["OutflowFrom"] = "Funder";
                    dr["Entity_id"] = Convert.ToInt32(ddlfund.SelectedValue);
                    dr["Entity"] = Convert.ToString(ddlfund.SelectedText);
                    dr["Amount"] = dblProcFeeAmt;

                    dtCF.Rows.Add(dr);
                }
            }
            else
            {
                DataRow[] drCf = dtCF.Select("CashOutFlow_id = '" + 144 + "'");
                if (drCf.Length > 0)
                {
                    drCf[0].Delete();
                    dtCF.AcceptChanges();
                }
            }

            if (dblServiceTax > 0)
            {
                DataRow[] drCf = dtCF.Select("CashOutFlow_id = '" + 83 + "'");
                if (drCf.Length > 0)
                {
                    drCf[0]["Amount"] = dblServiceTax;
                    dtCF.AcceptChanges();
                }
                else
                {
                    dr = dtCF.NewRow();
                    dr["CashFlow_Type_id"] = "2";
                    dr["CashFlow_Type"] = "Outflow";
                    dr["CashOutFlow_id"] = 83;
                    dr["Date"] = Convert.ToString(DateTime.Now.ToString(strDateFormat));
                    dr["CashOutFlow"] = "Service tax";
                    dr["OutflowFrom_id"] = "1";
                    dr["OutflowFrom"] = "Funder";
                    dr["Entity_id"] = Convert.ToInt32(ddlfund.SelectedValue);
                    dr["Entity"] = Convert.ToString(ddlfund.SelectedText);
                    dr["Amount"] = dblServiceTax;

                    dtCF.Rows.Add(dr);
                }
            }
            else
            {
                DataRow[] drCf = dtCF.Select("CashOutFlow_id = '" + 83 + "'");
                if (drCf.Length > 0)
                {
                    drCf[0].Delete();
                    dtCF.AcceptChanges();
                }
            }

            if (dtCF.Rows.Count > 0)
            {
                dtcash = dtCF;

                gvOutFlow.DataSource = dtCF;
                gvOutFlow.DataBind();
                ViewState["dtcash"] = dtCF;
            }
            else
            {
                FunPriSetEmptyCashtbl();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void lblProcessing_Fee_Amount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strSelectID = ((TextBox)sender).ClientID;
            int _iRowIdx = Utility.FunPubGetGridRowID("grvfund", strSelectID);

            Label lblsanction_id = (Label)grvfund.Rows[_iRowIdx].FindControl("lblsanction_id");
            TextBox txtPFamt = (TextBox)grvfund.Rows[_iRowIdx].FindControl("lblProcessingFeeAmount");
            TextBox lblforeclosurerate = (TextBox)grvfund.Rows[_iRowIdx].FindControl("lblforeclosurerate");
            TextBox lblDiscountrate = (TextBox)grvfund.Rows[_iRowIdx].FindControl("lblDiscountrate");
            TextBox lblChq_Rtn_Charges = (TextBox)grvfund.Rows[_iRowIdx].FindControl("lblChq_Rtn_Charges");
            TextBox lblODI_Rate = (TextBox)grvfund.Rows[_iRowIdx].FindControl("lblODI_Rate");
            TextBox lblMisc_Charges = (TextBox)grvfund.Rows[_iRowIdx].FindControl("lblMisc_Charges");

            DataTable dtAsset = (DataTable)ViewState["dtasset"];
            DataRow[] drAst = dtAsset.Select("sanction_id = " + Convert.ToString(lblsanction_id.Text));

            if (drAst.Length > 0)
            {
                drAst[0]["Processing_Fee_Amount"] = Convert.ToDouble(txtPFamt.Text);
                drAst[0]["PF_Service_Tax"] = Math.Round(Convert.ToDouble(txtPFamt.Text) * (Convert.ToDouble(drAst[0]["Serice_Tax_Perc"]) / 100), 2);
                drAst[0]["Foreclosure_Rate"] = lblforeclosurerate.Text;
                drAst[0]["discount_rate"] = lblDiscountrate.Text;
                drAst[0]["Chq_Rtn_Charges"] = lblChq_Rtn_Charges.Text;
                drAst[0]["ODI_Rate"] = lblODI_Rate.Text;
                drAst[0]["Misc_Charges"] = Convert.ToDecimal(lblMisc_Charges.Text);
                dtAsset.AcceptChanges();
                ViewState["dtasset"] = dtAsset;
            }
            FunPriAddProcessingCashFlow();
        }
        catch (Exception objException)
        {

        }
    }

    //protected void txtDisbursementdate_OnTextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txtDisbursementdate.Text != "")
    //        {
    //            grvPV.DataSource = null;
    //            grvPV.DataBind();
    //        }
    //    }
    //    catch (Exception objException)
    //    {

    //    }
    //}

    protected void Chk_DSRAChanged(object sender, EventArgs e)
    {
        hidsave.Value = "0";
        foreach (GridViewRow row in grvPV.Rows)
        {
            TextBox txtdsra = (TextBox)row.FindControl("txtDsra");
            if (CHKDSRA.Checked)
            {
                txtdsra.Enabled = true;
            }
            else
            {
                txtdsra.Enabled = false;
                txtdsra.Text = "0.00";
                Label lblTOtDsra = (Label)(grvPV).FooterRow.FindControl("lblTOtDsra") as Label;
                lblTOtDsra.Text = "0.00";
            }
        }
    }
}