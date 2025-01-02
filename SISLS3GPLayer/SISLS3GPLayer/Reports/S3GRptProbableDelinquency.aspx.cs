#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Reports
/// Screen Name			: Probable Delinquency
/// Created By			: Sangeetha R
/// Created Date		: 1-Feb-2013
/// Purpose	            : To get the Demand Processing details for LOB, Location and Demand Month.
/// Last Updated By		:  
/// Last Updated Date   : 
/// Purpose	            : 
/// Last Updated By		: 
/// Last Updated Date   :   
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
using System.Globalization;
using System.Data;
using S3GBusEntity.Origination;
using System.Web.Security;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Web.UI.HtmlControls;
using S3GBusEntity.Collection;
using S3GBusEntity.Reports;
using ReportOrgColMgtServicesReference;
#endregion

public partial class Reports_S3GRptProbableDelinquency : ApplyThemeForProject
{
    #region Variable declaration
    //UserInfo ObjUserInfo;
    //S3GSession ObjS3GSession = new S3GSession();
    Dictionary<string, string> Procparam;
    int intResult, intDemandProcessId;
    string strErrorMessagePrefix = @"Please correct the following validation(s): </br></br> ";
    public string strDateFormat;
    string strDemandType;
    string strProcName = string.Empty;
    string strAlert = "alert('__ALERT__');";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    decimal totarrearamount;
    decimal totcurrentdue;
    public string strProgramId = "236";
    //Code end

    ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient ObjReceivableMgtServices;
    ClnReceivableMgtServices.S3G_CLN_DemandHeaderDataTable ObjS3G_CLN_DemandHeader;
    ReportOrgColMgtServicesClient ObjSerClient;
    DataSet dsPrblDelinq = new DataSet();

    string strPageName = "Probable Delinquency";

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            //txtCutoffDate.Attributes.Add("readonly", "readonly");
            txtCutoffDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCutoffDate.ClientID + "','" + strDateFormat + "',true,  false);");

            if (!IsPostBack)
            {
                FunPriLoadPage();
            }
            CalendarExtender1.Format = strDateFormat;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            CVDemandProcessing.ErrorMessage = "Due to Data Problem, Unable to Load the Demand details.";
            CVDemandProcessing.IsValid = false;

        }
    }
    #endregion 

    #region Page Methods

    /// <summary>
    /// This method will execute when page Loads
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {

            FunPriLoadLobandBranch();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable Load Demand Processing page");
        }
    }

    /// <summary>
    ///This method is used to Load the Line of Business to a dropdown from stored Procedure using BindDatatable option..
    /// </summary>
    /// 
    private void FunPriLoadLobandBranch()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_Id", UserId);
            Procparam.Add("@Company_ID", CompanyId);
            Procparam.Add("@Program_ID", strProgramId);
            ddlLOB.BindDataTable("S3G_Cln_GetDemandLOB", Procparam, true, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Branch_ID", "Branch_Code", "Branch_Name" });

            //ddlDemandMonth.FillFinancialMonth(FinancialYear);
            //ddlDemandMonth.SelectedValue = FinancialMonth;
            FunPriLoadDemandMonth();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error in loading Lob");
        }
    }

    /// <summary>
    ///This method is used to Load the Line of Business to a dropdown 
    ///from stored Procedure using BindDatatable option..
    ///For Demand Processed Month
    /// </summary>
    /// 
    private void FunPriLoadLob()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_Id", UserId);
            Procparam.Add("@Company_ID", CompanyId);
            Procparam.Add("@Program_ID", strProgramId);
            Procparam.Add("@Option", "2");
            Procparam.Add("@DemandMonth", ddlDemandMonth.SelectedValue.ToString().Trim());

            ddlLOB.BindDataTable("S3G_Cln_GetDemandLOB", Procparam, true, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error in loading Lob");
        }
    }

    /// <summary>
    ///This method is used to Load the Demand Month to a dropdown 
    ///from stored Procedure using BindDatatable option..
    /// </summary>
    /// 
    private void FunPriLoadDemandMonth()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@FinStartYear", FinancialYear.Split('-')[0].ToString().Trim());
            Procparam.Add("@FinEndYear", FinancialYear.Split('-')[1].ToString().Trim());
            ddlDemandMonth.BindDataTable("S3G_Cln_GetDemandProcessedMonth", Procparam, true, new string[] { "Demand_month", "Demand_month" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Error in loading Processed Demand Month");
        }
    }


    /// <summary>
    /// Load Account Details based on demand type selected
    /// </summary>
    /// <param name="strDemandType"></param>
    private void FunPriLoadAccountDetails(string strDemandType)
    {
        DataTable dtAccountDetails;
        btnProcess.Enabled = true;
        Procparam = new Dictionary<string, string>();
        Procparam.Clear();
        Procparam.Add("@COMPANY_ID", CompanyId);
        //Procparam.Add("@Is_Active", "1");
        //Procparam.Add("@User_Id", UserId);
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        
        //Procparam.Add("@Option", "2");
        Procparam.Add("@DEMANDMONTH", ddlDemandMonth.SelectedValue);
        dtAccountDetails = Utility.GetDefaultData(SPNames.GetProbableDelinq, Procparam);
        ViewState["dtAccountDetails"] = dtAccountDetails;
        FunPriBindDemandDetails(dtAccountDetails);

    }

    /// <summary>
    /// To Calculate branch wise Demand 
    /// </summary>
    private void FunPriProcessBranchWiseDemand()
    {
        try
        {
            DataTable dtAccountDetails = new DataTable();
            int intCheckCount = 0;
            dtAccountDetails = (DataTable)ViewState["dtAccountDetails"];

                foreach (DataRow DrTempRow in dtAccountDetails.Rows)
                {
                    DrTempRow["SelectLocation"] = Convert.ToByte(false);
                    DrTempRow["Arrears_Amount"] = 0.0000;
                    DrTempRow["Current_Due"] = 0.0000;
                    DrTempRow["No_of_Accounts"] = 0;
                }
                dtAccountDetails.AcceptChanges();

            foreach (GridViewRow grvRow in grvDemandProcess.Rows)
            {
                if (grvRow.Enabled)
                {
                    CheckBox chkSelBranch = (CheckBox)grvRow.FindControl("chkSelectBranch");
                    Label lblBranch_Id = (Label)grvRow.FindControl("lblBranch_Id");
                    if (chkSelBranch.Checked && chkSelBranch.Enabled)
                    {
                        
                        Procparam = new Dictionary<string, string>();
                        Procparam.Clear();
                        if (ddlLOB.SelectedIndex > 0)
                        {
                            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                        }
                        Procparam.Add("@Company_ID", CompanyId);
                        Procparam.Add("@Location_ID", lblBranch_Id.Text);
                        Procparam.Add("@User_Id", UserId);
                        Procparam.Add("@Option", "2");
                        Procparam.Add("@Demand_EndDate", Utility.StringToDate(txtCutoffDate.Text).ToString());
                        Procparam.Add("@Demand_Month", ddlDemandMonth.SelectedItem.Text);
                        DataTable dtBranchValue = Utility.GetDefaultData(SPNames.S3G_CLN_GetBranchLevelDemand, Procparam);

                        DataRow[] drAccountsRow = dtAccountDetails.Select("Location_ID = '" + lblBranch_Id.Text + "'");
                    
                        for (int i = 0; i < drAccountsRow.Length; i++)
                        {
                            if (dtBranchValue.Rows.Count > 0)
                            {
                                drAccountsRow[i]["SelectLocation"] = dtBranchValue.Rows[0]["SelectLocation"];
                                drAccountsRow[i]["Arrears_Amount"] = dtBranchValue.Rows[0]["Arrears_Amount"];
                                drAccountsRow[i]["Current_Due"] = dtBranchValue.Rows[0]["Current_Due"];
                                drAccountsRow[i]["No_of_Accounts"] = dtBranchValue.Rows[0]["No_of_Accounts"];
                            }
                            else
                            {
                                drAccountsRow[i]["SelectLocation"] = "True";
                            }
                        }

                        dtAccountDetails.AcceptChanges();
                        intCheckCount++;

                    }
                }

            }
            if (intCheckCount <= 0)
            {
                btnProcess.Enabled = true;
            }
            else
            {
                btnProcess.Enabled = false;
            }
            FunPriBindDemandDetails(dtAccountDetails);
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
            btnProcess.Enabled = true;
        }
    }

    /// <summary>
    /// To bind demand details in gridview and to set 
    /// </summary>
    /// <param name="dtAccountDetails"></param>
    private void FunPriBindDemandDetails(DataTable dtAccountDetails)
    {
        try
        {
            pnlDemandProcess.Visible = true;
            if (dtAccountDetails != null && dtAccountDetails.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "No records found for Demand Processing");
                return;
            }

            grvDemandProcess.DataSource = dtAccountDetails;
            grvDemandProcess.DataBind();
            if (grvDemandProcess.FooterRow != null)
            {
                Label txtCurrent_Due = (Label)grvDemandProcess.FooterRow.FindControl("txtAmount");
                Label txtArrears = (Label)grvDemandProcess.FooterRow.FindControl("txtArrearsAmount");
                Label txtNoofAcc = (Label)grvDemandProcess.FooterRow.FindControl("txtNoofAcc");
                if (dtAccountDetails != null && dtAccountDetails.Rows.Count > 0)
                {
                    //txtCurrent_Due.Text = ((decimal)dtAccountDetails.Compute("Sum(Current_Due)", "Location_ID > 0")).ToString(Utility.SetSuffix());
                    //txtArrears.Text = ((decimal)dtAccountDetails.Compute("Sum(Arrears_Amount)", "Location_ID > 0")).ToString(Utility.SetSuffix());
                    //txtNoofAcc.Text = dtAccountDetails.Compute("Sum(No_of_Accounts)", "Location_ID > 0").ToString();
                    decimal decCurrent_Due = 0, decArrears = 0, decNoofAcc=0;
                    foreach (DataRow dr in dtAccountDetails.Rows)
                    {
                        decCurrent_Due += Convert.ToDecimal(dr["Current_Due"].ToString());
                        decArrears += Convert.ToDecimal(dr["Arrears_Amount"].ToString());
                        decNoofAcc += Convert.ToDecimal(dr["No_of_Accounts"].ToString());
                    }

                    txtCurrent_Due.Text = decCurrent_Due.ToString(Utility.SetSuffix());
                    txtArrears.Text = decArrears.ToString(Utility.SetSuffix());
                    txtNoofAcc.Text = decNoofAcc.ToString();




                }
            }
            //if (dtAccountDetails != null && dtAccountDetails.Rows.Count > 0)
            //{
            //    S3GDALDBType DBType = FunPubGetDatabaseType();
            //    if (DBType == S3GDALDBType.ORACLE)
            //    {
            //        if (dtAccountDetails.Select("SelectLocation='True'").Length == dtAccountDetails.Rows.Count)
            //        {
            //            (grvDemandProcess.HeaderRow.FindControl("chkAll") as CheckBox).Checked = true;
            //        }
            //    }
            //    else
            //    {
            //        if (dtAccountDetails.Select("SelectLocation=1").Length == dtAccountDetails.Rows.Count)
            //        {
            //            (grvDemandProcess.HeaderRow.FindControl("chkAll") as CheckBox).Checked = true;
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
                throw new Exception("Error in getting Demand details");
        }
    }

    /// <summary>
    /// To get Demand processing Details
    /// </summary>
    /// <param name="strDemandProcessId"></param>
    private void FunPriGetDemandDetails()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Demand_ProceesId", PageIdValue);
            Procparam.Add("@User_Id", UserId);
            DataSet dsDemandDetails = Utility.GetDataset(SPNames.S3G_CLN_GetDemandProcessingDetails, Procparam);
            DataRow drDemandRow = dsDemandDetails.Tables[0].Rows[0];
            if (drDemandRow["Demand_Type"] == "0")
            {
                txtCutoffDate.Text = drDemandRow["Demand_Date"].ToString();
            }
            ddlDemandMonth.SelectedValue = drDemandRow["Demand_Month"].ToString();
            ddlDemandMonth.ClearDropDownList();
            ddlLOB.SelectedValue = drDemandRow["LOB_ID"].ToString();
            ddlLOB.ClearDropDownList();

            ViewState["dtAccountDetails"] = dsDemandDetails.Tables[1];
            FunPriBindDemandDetails(dsDemandDetails.Tables[1]);
            //if ((int.Parse(dsDemandDetails.Tables[2].Rows[0]["Count"].ToString()) == 0) && int.Parse(dsDemandDetails.Tables[3].Rows[0]["Count"].ToString()) > 0)
            //{
            //    btnAssignDC.Enabled = true;
            //}
            //else
            //    btnAssignDC.Enabled = false;
            //grvDemandProcess.DataSource = dsDemandDetails.Tables[1];
            //grvDemandProcess.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    /// <summary>
    /// Method To Insert Into S3G Service table for Assign DC
    /// </summary>
    private void FunPriSetAssignDCService()
    {
        try
        {
            int ProcessCnt = 0;
            foreach (GridViewRow grvRow in grvDemandProcess.Rows)
            {
                if (grvRow.Enabled)
                {
                    CheckBox chkSelBranch = (CheckBox)grvRow.FindControl("chkSelectBranch");
                    if (chkSelBranch.Checked && chkSelBranch.Enabled)
                    {
                        Label lblBranch_Id = (Label)grvRow.FindControl("lblBranch_Id");
                        DateTime dtCurrentTime = DateTime.Now;
                        Procparam = new Dictionary<string, string>();
                        Procparam.Add("@Company_ID", CompanyId);
                        Procparam.Add("@Demand_ID", PageIdValue);
                        Procparam.Add("@Created_by", UserId);
                        Procparam.Add("@Location_ID", lblBranch_Id.Text.Trim());
                        Procparam.Add("@Schedule_Date", dtCurrentTime.ToString());
                        Procparam.Add("@Schedule_Time", dtCurrentTime.AddMinutes(5.0).ToShortTimeString());
                        Utility.GetTableScalarValue(SPNames.S3G_CLN_InsertAssignDCService, Procparam);
                        ProcessCnt++;
                    }
                }
            }
            if (ProcessCnt == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast one Location for Assign DC");
            }
            else
            {
                //Utility.FunShowAlertMsg(this, "DC assigned successfully");
                strAlert = strAlert.Replace("__ALERT__", "DC Assign Process Initiated");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FunExcelExport(object sender,EventArgs e)
    {
        //Changed by Thangam M on 08/Feb/2012 to solve nullable out param probs
        S3GDALDBType S3G_DBType;
        S3G_DBType = FunPubGetDatabaseType();
        if (S3G_DBType == S3GDALDBType.SQL)
        {
            strProcName = "S3G_RPT_PROBABLEDELINQUENCY";
        }
        //else
        //{
        //    strProcName = "S3G_LOANAD_GetIncomeRecognitionSpool";
        //}

        // Changes end

        //strProcName = "S3G_LOANAD_GetIncomeRecognition";

        int intRowIndex = Utility.FunPubGetGridRowID("grvDemandProcess", ((ImageButton)sender).ClientID.ToString());

        Label intlblBranch_Id = (Label)grvDemandProcess.Rows[intRowIndex].FindControl("lblBranch_Id");
        

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@COMPANY_ID", CompanyId.ToString());
        Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        Procparam.Add("@LOCATION_ID", intlblBranch_Id.Text);
        Procparam.Add("@DEMANDMONTH", ddlDemandMonth.SelectedValue);

        dsPrblDelinq = Utility.GetTableValues(strProcName, Procparam);
        grvprbldelinq.DataSource = dsPrblDelinq;
        grvprbldelinq.DataBind();

        string attachment = "attachment; filename=ProbableDelinquency.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.xls";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        grvprbldelinq.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    private void FunResetControls(string strOption)
    {
        //ddlLOB.SelectedIndex = 0;
        grvDemandProcess.DataSource = null;
        grvDemandProcess.DataBind();
    }
    #endregion

    #region Events

    #region Buttons



    #region Cancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Common/HomePage.aspx");
        }
        catch (Exception ex)
        {
            CVDemandProcessing.ErrorMessage = "Unable To Cancel.";
            CVDemandProcessing.IsValid = false;
        }
    }
    #endregion

    #region Clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = -1;
            ddlDemandMonth.SelectedIndex = -1;
            txtCutoffDate.Text = "";
            btnProcess.Enabled = true;
            pnlDemandProcess.Visible = false;
            grvDemandProcess.DataSource = null;
            grvDemandProcess.DataBind();
            Pnlprbldelinq.Visible = false;
            grvprbldelinq.DataSource = null;
            grvprbldelinq.DataBind();
        }
        catch (Exception ex)
        {
            CVDemandProcessing.ErrorMessage = "Unable To Clear.";
            CVDemandProcessing.IsValid = false;
        }
    }
    #endregion

    #region Process
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        //int chkCnt = 0;

        //foreach (GridViewRow grvRow in grvDemandProcess.Rows)
        //{
        //    if (grvRow.Enabled)
        //    {
        //        CheckBox chkSelBranch = (CheckBox)grvRow.FindControl("chkSelectBranch");

        //        if (chkSelBranch.Checked && chkSelBranch.Enabled)
        //        {
        //            chkCnt++;
        //            break;
        //        }
        //    }
        //}
        //if (chkCnt == 0)
        //{
        //    Utility.FunShowAlertMsg(this, "Select atleast one Location for Process");
        //    return;
        //}

            if (ddlDemandMonth.SelectedIndex > 0)
            {
                string StrCutOffYearMonth = Utility.StringToDate(txtCutoffDate.Text).Year.ToString();

                if (Utility.StringToDate(txtCutoffDate.Text).Month.ToString().Length == 1)
                {
                    StrCutOffYearMonth = StrCutOffYearMonth + "0" +
                        Utility.StringToDate(txtCutoffDate.Text).Month.ToString();
                }
                else
                {
                    StrCutOffYearMonth = StrCutOffYearMonth +
                        Utility.StringToDate(txtCutoffDate.Text).Month.ToString();
                }

                //int intDemandMonth = int.Parse(ddlDemandMonth.SelectedItem.ToString().Substring(4));
                //int intCutOffMonth = Utility.StringToDate(txtCutoffDate.Text).Month;
                //int intDemandYear = int.Parse(ddlDemandMonth.SelectedItem.ToString().Substring(0, 4));
                //int intCutOffYear = Utility.StringToDate(txtCutoffDate.Text).Year;
                if (int.Parse(StrCutOffYearMonth) < int.Parse(ddlDemandMonth.SelectedItem.ToString()))
                {
                    Utility.FunShowAlertMsg(this, "Cut Off Date cannot be less than demand month");
                    return;
                }
            }
        FunPriProcessBranchWiseDemand();
    }
    #endregion

    #region Assign DC
    protected void btnAssignDC_Click(object sender, EventArgs e)
    {
        FunPriSetAssignDCService();
    }
    #endregion

    #region Go
    protected void btnGo_Click(object sender, EventArgs e)
    {
        FunPriLoadAccountDetails(strDemandType);
    }
    #endregion

    #endregion

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FunPriLoadAccountDetails(rbtlDemandType.SelectedItem.Text);
        FunResetControls("0");
    }

    protected void grvDemandProcess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgbtnPorting = (ImageButton)e.Row.FindControl("imgbtnXL");
            imgbtnPorting.Enabled = true;
            imgbtnPorting.CssClass = "styleGridQuery";

            //CheckBox chkSelBranch = (CheckBox)e.Row.FindControl("chkSelectBranch");
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            //chkSelBranch.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvDemandProcess.ClientID + "','chkAll','chkSelectBranch');");
            //if (chkSelBranch.Checked && lblStatus.Text.ToUpper() == "PROCESSED" && PageMode == PageModes.Modify)
            //{
            //    e.Row.Enabled = true;
            //}
            //else if (chkSelBranch.Checked && lblStatus.Text.ToUpper() != "PENDING")// "PROCESSED")
            //{
            //    e.Row.Enabled = false;
            //}
            //else
            //{
            //    e.Row.Enabled = true;
            //}
            //if (chkSelBranch.Checked)
            //    ((Label)e.Row.FindControl("lblProcess")).Text = "1";

            //for (int i = 0; i < e.Row.Cells.Count; i++)
            //{
            //    switch (((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[i].GetType().ToString())
            //    {
            //        case "System.Decimal":
            //            e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            //            ((Label)e.Row.Cells[i].Controls[1]).Text = Convert.ToDecimal(((Label)e.Row.Cells[i].Controls[1]).Text).ToString(Utility.SetSuffix());
            //            break;
            //        case "System.Int32":
            //            e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            //            break;
            //        case "System.String":
            //            e.Row.Cells[i].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
            //            break;
            //    }

            //}

        }
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
        //    chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvDemandProcess.ClientID + "',this,'chkSelectBranch');");
        //}

    }

    /// <summary>
    /// To Deseriliaze the service Object
    /// </summary>
    /// <param name="byteObj"></param>
    /// <returns></returns>
    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    protected void FunProShow_AccountLevel(object sender, EventArgs e)
    {
        try
        {
            DataTable dtProbdel = new DataTable();
            Pnlprbldelinq.Visible = true;
            if (grvDemandProcess.Rows.Count > 0)
            {
                
                int intRowIndex = Utility.FunPubGetGridRowID("grvDemandProcess", ((ImageButton)sender).ClientID.ToString());

                Label intlblBranch_Id = (Label)grvDemandProcess.Rows[intRowIndex].FindControl("lblBranch_Id");
                ObjSerClient = new ReportOrgColMgtServicesClient();
                ClsPubProbDelinqParam Prbldelinq = new ClsPubProbDelinqParam();
                Prbldelinq.Company_Id = Convert.ToInt32(CompanyId);
                Prbldelinq.Lob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
                Prbldelinq.Location_Id = Convert.ToInt32(intlblBranch_Id.Text);
                Prbldelinq.DemandMonth = ddlDemandMonth.SelectedValue;

                byte[] byteProbableDelinq = ObjSerClient.FunPubGetProbableDelinq(Prbldelinq);
                List<ClsPubProbableDelinq> Probdel = (List<ClsPubProbableDelinq>)DeSeriliaze(byteProbableDelinq);
                totarrearamount = Probdel.Sum(ClsPubProbableDelinq => ClsPubProbableDelinq.Arrear_Due);
                totcurrentdue = Probdel.Sum(ClsPubProbableDelinq => ClsPubProbableDelinq.Current_Due);
                grvprbldelinq.DataSource = Probdel;
                grvprbldelinq.DataBind();
                if (grvprbldelinq.Rows.Count > 0)
                {
                    ((Label)grvprbldelinq.FooterRow.FindControl("txtArrearsAmount1")).Text = totarrearamount.ToString();
                    ((Label)grvprbldelinq.FooterRow.FindControl("txtAmount1")).Text = totcurrentdue.ToString();
                }
            }
        }



        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjSerClient.Close();
        }
    }
    protected void ddlDemandMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunResetControls("1");
        FunPriLoadLob();
        txtCutoffDate.Text = "";
        ddlLOB.SelectedIndex = 0;
    }

    #endregion
}
