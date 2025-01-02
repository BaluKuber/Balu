using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Configuration;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using System.Web.Security;
using System.Globalization;

//Created By :Sathish R  
//Created On:28-May-2014
//Purpose:TO Insert Schedule Job For Reports
public partial class Reports_S3GReportSchedule : ApplyThemeForProject
{
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    //ReportAdminMgtServicesReference.ApprovalMgtServicesClient objReportSchedule;
    ReportAdminMgtServicesReference.ReportAdminMgtServicesClient objReportSchedule;
    S3GBusEntity.LoanAdmin.AssetMgtServices.Shedule_ReportDataTable ObjReportDataTable = null;
    S3GBusEntity.LoanAdmin.AssetMgtServices.Shedule_ReportRow ObjReportDataRow = null;
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;
    string strRedirectPageAdd = "window.location.href='../Reports/S3GReportSchedule.aspx';";
    string strRedirectPageView1 = "window.close();";
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";
    string strKey = "Insert";
    int intScheduleId = 0;
    string strDynamicRedirection = string.Empty;
    static int intScheduleIddis = 0;
    string strDateFormat = string.Empty;
    public static Reports_S3GReportSchedule obj_Page;
    int intCompanyId;
    int intUserId;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;

            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();

            strDateFormat = ObjS3GSession.ProDateFormatRW;

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            //txtScheduleat.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleat.ClientID + "','" + strDateFormat + "',false,true);");
            //txtFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleat.ClientID + "','" + strDateFormat + "',true,false);");
            // txtToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleat.ClientID + "','" + strDateFormat + "',true,false);");
            if (!IsPostBack)
            {
                FunPubLoadMultiLOB();
                FunPubLoadFileFormate();
                FunPubChckBoxListLocation();
                FunPubLoadReportName();
                FunPriLoadStatus();
                FunPubLoadInvoiceType();
                // FunPubLoadReportmonth();
                //FillFinancialMonth(ddlDemandMonth);
                //FillFinancialMonth(ddlDemandToMonth);

                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                Procparam.Add("@Option", "1");
                DataSet ds = Utility.GetDataset("S3G_ORG_ProposalLukup", Procparam);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)//Proposal Type
                    {
                        ddlProposalType.FillDataTable(ds.Tables[0], "Value", "Name", false);
                        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--All--", "0");
                        ddlProposalType.Items.Insert(0, liSelect);
                    }
                }

                if (Request.QueryString.Get("QsMode") != "Q" && Request.QueryString.Get("QsMode") != null)
                {
                    FormsAuthenticationTicket fromTicket;
                    fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                    intScheduleId = Convert.ToInt32(fromTicket.Name);
                    if (intScheduleId > 0)
                    {
                        FunPubLoadSceduleDetailsModifyMode(intScheduleId);
                        //calScheduleAt.Format = ObjS3GSession.ProDateFormatRW;
                        FunPubDisableControls(true);
                        ViewState["Shedule_Id"] = intScheduleId.ToString();
                        intScheduleIddis = intScheduleId;
                    }
                }
                else
                {
                    btncanenReq.Visible = false;
                }
                calScheduleAt.Format = ObjS3GSession.ProDateFormatRW;
                calFromDate.Format = ObjS3GSession.ProDateFormatRW;
                calToDate.Format = ObjS3GSession.ProDateFormatRW;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }

    }
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {
    }
    protected void txtProgramName_TextChanged(object sender, EventArgs e)
    {

    }
    //protected void txtToDate_TextChanged(object sender, EventArgs e) // AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"
    //{
    //    if (Convert.ToDateTime(txtFromDate.Text.Trim()) > Convert.ToDateTime(txtToDate.Text.Trim()))
    //    {
    //        Utility.FunShowAlertMsg(this, "To date cannot be less than From date");
    //        return;
    //    }
    //}
    public void FunPubLoadMultiLOB()
    {
        try
        {
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@Is_Active", "1");
            strProParm.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            strProParm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            strProParm.Add("@Program_ID", "520");
            DataSet dsLookUp = Utility.GetDataset("S3G_Cln_GetDemandLOB", strProParm);
            ChkLSTLob.DataValueField = "LOB_ID";
            ChkLSTLob.DataTextField = "LOB_NAME";
            ChkLSTLob.DataSource = dsLookUp.Tables[0];
            ChkLSTLob.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {

        }
    }
    public void FunPubLoadFileFormate()
    {
        try
        {
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            DataSet dsLookUpFile = Utility.GetDataset("S3G_RP_GET_FILEFORMATE", strProParm);
            ddlfileformate.BindDataTable("S3G_RP_GET_FILEFORMATE", strProParm, new string[] { "ID", "NAME" });
            ddlfileformate.SelectedValue = "1";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }

    protected void ddlprogramName_SelectedIndexChange(object sender, EventArgs e)
    {
        if (ddlprogramName.SelectedValue == "1")
        {
            ddlProposalType.Enabled = true;
        }
        else
        {
            ddlProposalType.Enabled = false;
        }

        if (ddlprogramName.SelectedValue == "0")
        {
            lblToMonth.CssClass = "styleReqFieldLabel";
            ddlFunderName.Enabled = ddlVendorName.Enabled = ddlAssetStatus.Enabled =
            ddlVendorInvoiceStatus.Enabled = ddlStatus.Enabled = ddlAssetCategory.Enabled =
            ddlinvoiceType.Enabled = ddlLocation.Enabled = ddlRentalGroup.Enabled = false;
        }
        else if (ddlprogramName.SelectedValue == "1")
        {
            lblDate.CssClass = lblToMonth.CssClass = "styleDisplayLabel";
            rfvDemandToMonth.Enabled = ddlVendorName.Enabled = ddlFunderName.Enabled = ddlAssetStatus.Enabled
                = ddlVendorInvoiceStatus.Enabled = ddlAssetCategory.Enabled = true;
            ddlStatus.Enabled = ddlinvoiceType.Enabled = ddlLocation.Enabled = ddlRentalGroup.Enabled = false;
            rfvFromDate.Enabled = rfvDemandToMonth.Enabled = false;
        }
        else if (ddlprogramName.SelectedValue == "2")
        {
            lblToMonth.CssClass = "styleDisplayLabel";
            rfvDemandToMonth.Enabled = ddlVendorName.Enabled = ddlAssetStatus.Enabled = ddlVendorInvoiceStatus.Enabled
                = ddlAssetCategory.Enabled = ddlStatus.Enabled = ddlinvoiceType.Enabled = ddlLocation.Enabled = false;
            ddlRentalGroup.Enabled = true;
        }
        else if (ddlprogramName.SelectedValue == "3")
        {
            lblToMonth.CssClass = "styleReqFieldLabel";
            ddlFunderName.Enabled = ddlAssetStatus.Enabled = ddlVendorInvoiceStatus.Enabled =
                ddlAssetCategory.Enabled = ddlinvoiceType.Enabled = ddlLocation.Enabled = ddlRentalGroup.Enabled = false;
            rfvDemandToMonth.Enabled = ddlVendorName.Enabled = ddlStatus.Enabled = true;
        }
        else if (ddlprogramName.SelectedValue == "4")
        {
            lblToMonth.CssClass = "styleReqFieldLabel";
            ddlFunderName.Enabled = ddlVendorName.Enabled = ddlAssetStatus.Enabled = ddlVendorInvoiceStatus.Enabled
                = ddlStatus.Enabled = ddlAssetCategory.Enabled = ddlinvoiceType.Enabled = ddlLocation.Enabled = ddlRentalGroup.Enabled = false;
        }
        else if ((ddlprogramName.SelectedValue == "5") || (ddlprogramName.SelectedValue == "6"))
        {
            lblToMonth.CssClass = "styleReqFieldLabel";
            ddlinvoiceType.Enabled = ddlLocation.Enabled = true;
            ddlFunderName.Enabled = ddlVendorName.Enabled = ddlAssetStatus.Enabled = ddlVendorInvoiceStatus.Enabled = 
                ddlStatus.Enabled = ddlAssetCategory.Enabled = ddlRentalGroup.Enabled = false;
        }
        else if (ddlprogramName.SelectedValue == "8")
        {
            lblToMonth.CssClass = "styleDisplayLabel";
            ddlVendorName.Enabled = true;
            rfvDemandToMonth.Enabled = ddlFunderName.Enabled = ddlAssetStatus.Enabled = ddlVendorInvoiceStatus.Enabled
                = ddlAssetCategory.Enabled = ddlStatus.Enabled = ddlinvoiceType.Enabled = ddlLocation.Enabled = ddlRentalGroup.Enabled = false;
        }
    }

    protected void btnGo_Schedule(object sender, EventArgs e)
    {
        try
        {
            string strAlert = string.Empty;
            string strMsg = string.Empty;
            string StrId = string.Empty;
            string strDocPath = string.Empty;
            string strSch_ID = string.Empty;
            string strLobId = string.Empty;
            string strLocation = string.Empty;
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                if (Utility.StringToDate(txtFromDate.Text) > Utility.StringToDate(txtToDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "To date cannot be less than From date");
                    txtToDate.Focus();
                    txtToDate.Text = string.Empty;
                    return;
                }
                else if (Utility.StringToDate(txtFromDate.Text) > Utility.StringToDate(txtToDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "From date cannot be less than To date");
                    txtToDate.Focus();
                    txtToDate.Text = string.Empty;
                    return;
                }

            }
            objReportSchedule = new ReportAdminMgtServicesReference.ReportAdminMgtServicesClient();
            ObjReportDataTable = new S3GBusEntity.LoanAdmin.AssetMgtServices.Shedule_ReportDataTable();
            ObjReportDataRow = ObjReportDataTable.NewShedule_ReportRow();
            foreach (ListItem item in ChkLSTLob.Items)
            {
                if (item.Selected == true)
                {
                    strLobId += item.Value + ',';
                }
            }

            foreach (ListItem item in ChckBoxListLocation.Items)
            {
                if (item.Selected)
                {
                    strLocation += item.Value + ',';
                }
            }
            //if (strLobId == string.Empty && ddlprogramName.SelectedValue != "1")
            //{
            //    Utility.FunShowAlertMsg(this, "Select Line of Business");
            //    return;
            //}
            //if (strLocation == string.Empty && ddlprogramName.SelectedValue != "1")
            //{
            //    Utility.FunShowAlertMsg(this, "Select atleast one Location");
            //    return;
            //}
            Dictionary<string, string> Proparm = new Dictionary<string, string>();
            Proparm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Proparm.Add("@Program_ID", "520");
            DataTable dtDocPath = Utility.GetDefaultData("S3G_Get_DocumentationPath", Proparm);
            if (dtDocPath.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Define the Document Path");
                return;
            }
            else
            {
                ViewState["DocPath"] = strDocPath = dtDocPath.Rows[0]["Document_Path"].ToString();
            }
            ObjReportDataRow.Company_Id = ObjUserInfo.ProCompanyIdRW.ToString();
            ObjReportDataRow.Lob_Id = strLobId.ToString();
            ObjReportDataRow.Location_Id = strLocation.ToString();
            //ObjReportDataRow.From_Month = ddlDemandMonth.SelectedItem.Text.ToString();
            //ObjReportDataRow.To_Month = ddlDemandToMonth.SelectedItem.Text.ToString();
            if (txtFromDate.Text != string.Empty)
                ObjReportDataRow.From_Date = Utility.StringToDate(txtFromDate.Text.Trim()).ToString();
            else
                ObjReportDataRow.From_Date = Utility.StringToDate("01/01/2000").ToString();

            if (txtToDate.Text != string.Empty)
                ObjReportDataRow.To_Date = Utility.StringToDate(txtToDate.Text.Trim()).ToString();
            else
                ObjReportDataRow.To_Date = DateTime.Now.ToString();

            ObjReportDataRow.Program_Id = ddlprogramName.SelectedValue.ToString().Trim();

            ObjReportDataRow.Customer_ID = ddlLesseeName.SelectedValue;
            ObjReportDataRow.Funder_Id = Convert.ToInt32(ddlFunderName.SelectedValue);
            ObjReportDataRow.Vendor_Id = Convert.ToInt32(ddlVendorName.SelectedValue);
            ObjReportDataRow.Asset_Status = Convert.ToInt32(ddlAssetStatus.SelectedValue);
            ObjReportDataRow.Vendor_Invoice_Status = Convert.ToInt32(ddlVendorInvoiceStatus.SelectedValue);
            ObjReportDataRow.Invoice_Status = Convert.ToInt32(ddlStatus.SelectedValue);
            ObjReportDataRow.Asset_Category = Convert.ToInt32(ddlAssetCategory.SelectedValue);

            ObjReportDataRow.Schedule_At = Utility.StringToDate(txtScheduleat.Text.Trim()).ToString();
            ObjReportDataRow.Formate = ddlfileformate.SelectedValue.Trim();
            ObjReportDataRow.User_Id = ObjUserInfo.ProUserIdRW.ToString();
            ObjReportDataRow.ReportPath = strDocPath;
            //opc002 start
            ObjReportDataRow.Invoice_Type = Convert.ToInt32(ddlinvoiceType.SelectedValue);
            ObjReportDataRow.Location_Id = ddlLocation.SelectedValue;
            //opc002 end

            //Invoice_Type Is using for Proposal Type
            if (ddlprogramName.SelectedValue == "1")
            {
                ObjReportDataRow.Invoice_Type = Convert.ToInt32(ddlProposalType.SelectedValue);
            }

            //Invoice_Type Is using for Rental Group
            if (ddlprogramName.SelectedValue == "2")
            {
                ObjReportDataRow.Invoice_Type = Convert.ToInt32(ddlRentalGroup.SelectedValue);
            }

            ObjReportDataTable.AddShedule_ReportRow(ObjReportDataRow);
            byte[] ByteObReportSchedule = ClsPubSerialize.Serialize(ObjReportDataTable, SerMode);
            intErrCode = objReportSchedule.FunPubCreateScheduleJobForReport(out strMsg, out StrId, SerMode, ByteObReportSchedule);
            if (intErrCode == 0)
            {
                strAlert = "Job Scheduled Successfully";
                strAlert += @"\n\nWould you like to Schedule one more Job?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView1 + "}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else if (intErrCode == 1)  // addded by Rao  - 30-Sep-14 to have only single job as Open.
            {
                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Already pending job is in Progress";
                cvApplicationProcessing.IsValid = false;
                return;
                //strAlert = strAlert.Replace("__ALERT__", "Already pending job is in Progress");
                //strRedirectPageView1 = "";
            }
            else
            {
                cvApplicationProcessing.ErrorMessage = strErrorMessagePrefix + "Already pending job is in Work in Progress";
                cvApplicationProcessing.IsValid = false;
                return;
                //strAlert = strAlert.Replace("__ALERT__", "Due to Data Problem,Unable to schedule the Job");
                //strRedirectPageView1 = "";

            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    public void FunPubChckBoxListLocation()
    {
        try
        {
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@Is_Active", "1");
            strProParm.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            strProParm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            strProParm.Add("@Program_ID", "510");
            DataSet dsLookUp = Utility.GetDataset("S3G_Get_Branch_List", strProParm);
            ChckBoxListLocation.DataValueField = "LOCATION_ID";
            ChckBoxListLocation.DataTextField = "LOCATION_NAME";
            ChckBoxListLocation.DataSource = dsLookUp.Tables[0];
            ChckBoxListLocation.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }

    }
    public void FunPubLoadReportName()
    {
        try
        {
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            strProParm.Add("@LoadOption", "5");
            strProParm.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            ddlprogramName.BindDataTable("S3G_RP_LOADMUL_LST", strProParm, new string[] { "ID", "NAME" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }

    public void FunPubLoadInvoiceType()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "1");
            ddlinvoiceType.BindMemoDataTable("S3G_Rpt_Get_InvType", Procparam, new string[] { "Lookup_value", "Lookup_Name" });
            ddlinvoiceType.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    
protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            this.Page.MasterPageFile = "~/Common/MasterPage.master";
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {

        }

    }
    public void FunPubLoadSceduleDetailsModifyMode(int intScheduleId)
    {
        try
        {
            DataSet dsSheduleJob;
            Dictionary<string, string> Proparm = new Dictionary<string, string>();
            Proparm.Add("@ScheduleiD", intScheduleId.ToString());
            Proparm.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            dsSheduleJob = Utility.GetDataset("S3G_GET_REPORTSHEDULEDET", Proparm);
            if (dsSheduleJob.Tables[0].Rows.Count > 0)
            {
                string[] strLobvals = dsSheduleJob.Tables[0].Rows[0]["Lob_Id"].ToString().Split(',');
                for (int i = 0, ii = ChkLSTLob.Items.Count; i < ii; i++)
                    if (strLobvals.Contains(ChkLSTLob.Items[i].Value))
                        ChkLSTLob.Items[i].Selected = true;
                string[] strLobLocValues = dsSheduleJob.Tables[0].Rows[0]["Location_Id"].ToString().Split(',');
                for (int i = 0, ii = ChckBoxListLocation.Items.Count; i < ii; i++)
                    if (strLobLocValues.Contains(ChckBoxListLocation.Items[i].Value))
                        ChckBoxListLocation.Items[i].Selected = true;
                if (strLobLocValues != null && strLobLocValues.Length > 0)
                {
                    if (ChckBoxListLocation.Items.Count == strLobLocValues.Length)
                    {
                        CheckAll.Checked = true;
                    }
                    else
                    {
                        CheckAll.Checked = false;
                    }
                }
                CheckAll.Enabled = false;
                txtFromDate.Text = dsSheduleJob.Tables[0].Rows[0]["FROM_DATE"].ToString();
                txtToDate.Text = dsSheduleJob.Tables[0].Rows[0]["TO_DATE"].ToString();
                ddlfileformate.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Report_Format"].ToString();
                txtScheduleat.Text = dsSheduleJob.Tables[0].Rows[0]["Scheduled_At"].ToString();
                txtScheduleat.Text = DateTime.Parse(txtScheduleat.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(ObjS3GSession.ProDateFormatRW);
                ddlprogramName.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Report_Id"].ToString();
                if (dsSheduleJob.Tables[0].Rows[0]["STATUS_ID"].ToString() == "1" || dsSheduleJob.Tables[0].Rows[0]["STATUS_ID"].ToString() == "3")
                {
                    btncanenReq.Enabled = false;
                }
                else
                    btncanenReq.Enabled = true;

                if (dsSheduleJob.Tables[0].Rows[0]["Customer_ID"].ToString() != "0")
                {
                    ddlLesseeName.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Customer_ID"].ToString();
                    ddlLesseeName.SelectedText = dsSheduleJob.Tables[0].Rows[0]["Customer_Name"].ToString();
                }

                if (dsSheduleJob.Tables[0].Rows[0]["Funder_Id"].ToString() != "0")
                {
                    ddlFunderName.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Funder_Id"].ToString();
                    ddlFunderName.SelectedText = dsSheduleJob.Tables[0].Rows[0]["Funder_Name"].ToString();
                }

                if (dsSheduleJob.Tables[0].Rows[0]["Vendor_Id"].ToString() != "0")
                {
                    ddlVendorName.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Vendor_Id"].ToString();
                    ddlVendorName.SelectedText = dsSheduleJob.Tables[0].Rows[0]["Vendor_Name"].ToString();
                }

                if (dsSheduleJob.Tables[0].Rows[0]["Asset_Category"].ToString() != "0")
                {
                    ddlAssetCategory.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Asset_Category"].ToString();
                    ddlAssetCategory.SelectedText = dsSheduleJob.Tables[0].Rows[0]["Asset_Category_Desc"].ToString();
                }
                //opc002 start
                if (dsSheduleJob.Tables[0].Rows[0]["Location_ID"].ToString() != "0")
                {
                    ddlLocation.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Location_ID"].ToString();
                    ddlLocation.SelectedText = dsSheduleJob.Tables[0].Rows[0]["Location_Name"].ToString();
                }
                //opc002 end
                ddlAssetStatus.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Asset_Status"].ToString();
                ddlAssetStatus.ClearDropDownList();
                ddlVendorInvoiceStatus.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Vendor_Invoice_Status"].ToString();
                ddlVendorInvoiceStatus.ClearDropDownList();
                ddlStatus.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Invoice_Status"].ToString();
                ddlStatus.ClearDropDownList();
                //opc002 start
                ddlinvoiceType.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Invoice_Type"].ToString();
                ddlinvoiceType.ClearDropDownList();

                if (dsSheduleJob.Tables[0].Rows[0]["Report_Id"].ToString() == "1")
                {
                    ddlProposalType.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Invoice_Type"].ToString();
                    ddlProposalType.ClearDropDownList();
                }
                if (dsSheduleJob.Tables[0].Rows[0]["Report_Id"].ToString() == "2")
                {
                    ddlRentalGroup.SelectedValue = dsSheduleJob.Tables[0].Rows[0]["Invoice_Type"].ToString();
                }
                if (dsSheduleJob.Tables[0].Rows[0]["Report_Id"].ToString() != "5")
                {
                    ddlLesseeName.Enabled = ddlFunderName.Enabled = ddlVendorName.Enabled =
                        ddlAssetCategory.Enabled = ddlinvoiceType.Enabled = ddlLocation.Enabled = 
                        ddlVendorInvoiceStatus.Enabled = ddlAssetStatus.Enabled =
                      ddlStatus.Enabled = ddlProposalType.Enabled = ddlRentalGroup.Enabled = false;
                }
                else
                {
                    ddlFunderName.Enabled = ddlVendorName.Enabled =
                      ddlAssetCategory.Enabled = ddlVendorInvoiceStatus.Enabled = ddlAssetStatus.Enabled =
                      ddlStatus.Enabled = false;
                    ddlLesseeName.Enabled = ddlinvoiceType.Enabled = ddlLocation.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }

    private void FunPriLoadStatus()
    {
        Dictionary<string, string> dictassetrgstr = new Dictionary<string, string>();
        try
        {
            dictassetrgstr.Add("@Company_ID", intCompanyId.ToString());
            dictassetrgstr.Add("@User_ID", intUserId.ToString());
            ddlAssetStatus.BindMemoDataTable("S3G_RPT_GetAssetRgstrRptStatus", dictassetrgstr, new string[] { "Value", "Name" });
        }
        catch (Exception ex)
        {
        }
    }

    public void FunPubDisableControls(bool isDiable)
    {
        try
        {
            if (isDiable == true)
            {
                ddlprogramName.ClearDropDownList();
                ddlfileformate.ClearDropDownList();
                txtScheduleat.ReadOnly = true;
                //ddlDemandMonth.SelectedValue = "0";
                //ddlDemandToMonth.SelectedValue = "0";
                txtFromDate.Enabled = txtToDate.Enabled = false;
                ChkLSTLob.Enabled = false;
                ChckBoxListLocation.Enabled = false;
                btnGo.Enabled = false;
                btnClear.Enabled = false;
                //ddlDemandMonth.ClearDropDownList();
                //ddlDemandToMonth.ClearDropDownList();
                calScheduleAt.Enabled = false;
                calFromDate.Enabled = false;
                calToDate.Enabled = false;
                txtToDate.Enabled = txtToDate.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }

    }
    protected void btn_CancelClick(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    protected void btn_CancelRequest(object sender, EventArgs e)
    {
        try
        {
            string strAlert = string.Empty;
            Dictionary<string, string> Proparm = new Dictionary<string, string>();
            Proparm.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            Proparm.Add("@Scheduleid", intScheduleIddis.ToString());
            Proparm.Add("@IsCancel", "1");
            Utility.GetDefaultData("S3G_GET_REPORTSHEDULEDET", Proparm);
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Schedule Cancelled successfully');" + strRedirectPageView1, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //ddlDemandMonth.SelectedValue = "0";
            txtFromDate.Text = txtToDate.Text = string.Empty;
            //ddlfileformate.SelectedValue = "0";
            txtScheduleat.Text = string.Empty;
            ddlprogramName.SelectedValue = ddlProposalType.SelectedValue = "0";
            ChkLSTLob.DataSource = null;
            ChkLSTLob.DataBind();
            FunPubLoadMultiLOB();
            ChckBoxListLocation.DataSource = null;
            ChckBoxListLocation.DataBind();
            FunPubChckBoxListLocation();
            ddlAssetStatus.SelectedValue = ddlVendorInvoiceStatus.SelectedValue = ddlStatus.SelectedValue = "0";
            ddlLesseeName.Clear();
            ddlVendorName.Clear();
            ddlFunderName.Clear();
            ddlAssetCategory.Clear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    //public void FunPubLoadReportmonth()
    //{
    //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //    Procparam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
    //    Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
    //    Procparam.Add("@Program_ID", "510");
    //    ddlDemandMonth.BindDataTable("CN_GET_INDICATIVE_MONTH", Procparam, new string[] { "ID", "C_DATE" });

    //}
    public void FunPubChckBoxListLocation2()
    {
        try
        {

            string strLobId = string.Empty;
            foreach (ListItem item in ChkLSTLob.Items)
            {
                if (item.Selected == true)
                {
                    strLobId += item.Value + ',';
                }
            }
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@Is_Active", "1");
            strProParm.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            strProParm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            strProParm.Add("@Lob_Id_RCH", strLobId);
            strProParm.Add("@Program_ID", "510");
            DataSet dsLookUp = Utility.GetDataset("S3G_Get_Branch_List", strProParm);
            ChckBoxListLocation.DataValueField = "LOCATION_ID";
            ChckBoxListLocation.DataTextField = "LOCATION_NAME";
            ChckBoxListLocation.DataSource = dsLookUp.Tables[0];
            ChckBoxListLocation.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    protected void ChckAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBox chkAll = (CheckBox)sender;
        foreach (ListItem item in ChckBoxListLocation.Items)
        {
            //Modified By : Anbuvel.T,Date :17-Mar-2015,Description : Bug Fixing
            if (!item.Selected)
            {
                item.Selected = chkAll.Checked;
            }
        }
    }
    protected void ChkLSTLob_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPubChckBoxListLocation2();
        CheckAll.Checked = false;
    }
    public void FillFinancialMonth(DropDownList ddlSourceControl)
    {
        try
        {
            ddlSourceControl.Items.Clear();
            int intCurrentYear = DateTime.Now.Year;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
            int intActualMonth = Convert.ToInt32(ClsPubConfigReader.FunPubReadConfig("StartMonth"));
            string strActualYear = ((DateTime.Now.Year)).ToString();
            string strActualYearNextYear = (DateTime.Now.Year + 1).ToString();
            //string strActualYear = ((DateTime.Now.Year) - 3).ToString();
            //string strActualYearNextYear = (DateTime.Now.Year - 2).ToString();
            for (int intMonthCnt = 1; intMonthCnt <= 12; intMonthCnt++)
            {
                if (intActualMonth >= 13)
                {
                    intActualMonth = 1;
                    strActualYear = strActualYearNextYear;
                }
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(strActualYear + intActualMonth.ToString("00"), strActualYear + intActualMonth.ToString("00"));
                ddlSourceControl.Items.Insert(intMonthCnt, liPSelect);
                intActualMonth = intActualMonth + 1;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLesseeNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "318");
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetFunders(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETFunders", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendorName(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("[S3G_GETVENDORS]", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAssetCategoryDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetAssetCategory", dictparam));
        return suggetions.ToArray();
    }
}