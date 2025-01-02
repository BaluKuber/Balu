
#region Page Header
//Module Name      :   LoanAdmin
//Screen Name      :   S3GLoanAdIncomeRecognition.aspx
//Created By       :   Kaliraj K
//Created Date     :   22-OCT-2010
//Purpose          :   To insert and update Income Recognition Details

#endregion

using System;
using System.Web.UI;
using System.Data;
using System.Text;
using S3GBusEntity.LoanAdmin;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using Resources;
using System.ServiceModel;

public partial class S3GLoanAdIncomeRecognition : ApplyThemeForProject
{
    #region initialization

    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjIncomeRecognition;

    //LoanAdminAccMgtServices.S3G_LOANAD_IncomeRecognitionDataTable ObjS3G_CLN_IncomeRecognitionDetailsDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;
    int intIncomeRecognitionID = 0;
    int intIncomeCalGridID = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int intRevoke = 0;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    string strDate = string.Empty;
    bool bMakerChecker = false;
    int intIRRows = 0;
    //Code end

    DataSet dsIncome = new DataSet();
    Dictionary<string, string> Procparam = null;
    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=CIR";
    string strKey = "Insert";
    string strErrMsg = string.Empty;
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdIncomeRecognition.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=CIR';";

    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public int ProPageNumRW                                                     // to retain the current page size and number
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
        ProPageNumRW = intPageNum;              // To set the page Number
        ProPageSizeRW = intPageSize;            // To set the page size    
        FunPriBindGridPaging();                       // Binding the Landing grid
    }

    #endregion

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {

        #region Grid Paging Config
        ProPageNumRW = 1;                                                           // to set the default page number
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        if (txtPageSize.Text != "")
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        else
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));
        PageAssignValue obj = new PageAssignValue(this.AssignValue);
        ucCustomPaging.callback = obj;
        ucCustomPaging.ProPageNumRW = ProPageNumRW;
        ucCustomPaging.ProPageSizeRW = ProPageSizeRW;
        #endregion

        //0 -  Calculate IR
        //1 -  Save
        //2 -  Revoke
        //3 -  Schedule
        //4 -  Get Records to Calculate
        //5 - Posting

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        //txtCutoffDate.Attributes.Add("readonly", "readonly");
        txtCutoffDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCutoffDate.ClientID + "','" + strDateFormat + "',true,  false);");
        CalendarExtender1.Format = strDateFormat;
        txtScheduleDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleDate.ClientID + "','" + strDateFormat + "',false,  true);");
        //txtScheduleDate.Attributes.Add("readonly", "readonly");
        CECScheduleDate.Format = strDateFormat;

        strDate = DateTime.Now.ToString(strDateFormat);

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));

            if (fromTicket != null)
            {
                intIncomeRecognitionID = Convert.ToInt32(fromTicket.Name);
                strMode = Request.QueryString.Get("qsMode");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
        }

        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end

        if (!IsPostBack)
        {
            //Validation Msgs from Resource File
            rfvLOB.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
            rfvLOBSave.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;
            rfvFrequency.ErrorMessage = ValidationMsgs.S3G_ValMsg_Frequency;
            rfvFrequencySave.ErrorMessage = ValidationMsgs.S3G_ValMsg_Frequency;

            txtCutoffDate.Text = DateTime.Now.ToString(strDateFormat);
            txtScheduleDate.Text = DateTime.Now.ToString(strDateFormat);

            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

           

            if ((intIncomeRecognitionID > 0) && (strMode == "M"))
            {
                FunPriDisableControls(1);
                //Code commented and added by saran on 30-Dec-2011 start
                //FunGetIncomeRecognitionDetails();
                FunGetIncomeRecognitionDetailsMod(intIncomeRecognitionID);
                //Code commented and added by saran on 30-Dec-2011 end
            }
            else if ((intIncomeRecognitionID > 0) && (strMode == "Q"))
            {
                //Code commented and added by saran on 30-Dec-2011 start
                //FunGetIncomeRecognitionDetails();
                FunGetIncomeRecognitionDetailsMod(intIncomeRecognitionID);
                //Code commented and added by saran on 30-Dec-2011 end
                FunPriDisableControls(-1);
                //rbtnSchedule.SelectedIndex = -1;
                //txtScheduleDate.Text = "";
            }
            else
            {
                FunPriBindLOBBranchFrequency();
                FunPriDisableControls(0);
            }


            string strLOB = ddlLOB.SelectedItem.Text.Split('-')[0].Trim();
            if (strLOB == "WC" || strLOB == "TL" || strLOB == "TE" || strLOB == "FT")
            {
                rfvFrequency.Enabled = false;
                rfvFrequencySave.Enabled = false;
            }
        }
        if (intRevoke == 0)
        {
            btnRevoke.Enabled = false;
        }
    }


    #endregion

    #region Page Events

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    protected void btnXLPorting_Click(object sender, EventArgs e)
    {

    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        try
        {

            FunCalculate(0);

            if (intErrCode != 0)
            {
                if ((intErrCode == -1) || (intErrCode == -2) || (intErrCode == 50))
                    Utility.FunShowValidationMsg(this.Page, "", intErrCode);
                else
                    Utility.FunShowValidationMsg(this.Page, "CLN_CIR", intErrCode);
                return;
            }
            else
            {
                grvIncomeRecognition.DataSource = dsIncome;
                grvIncomeRecognition.DataBind();

                ////if (grvIncomeRecognition.Rows.Count > 0)
                ////{
                ////    btnXLPorting.Enabled = true;
                ////}
                ////else
                ////{
                ////    btnXLPorting.Enabled = false;
                ////}
            }
        }

        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

        grvIRConsolidate.DataSource = null;
        grvIRConsolidate.DataBind();

        string strLOB = ddlLOB.SelectedItem.Text.Split('-')[0].Trim();
        if (strLOB == "WC" || strLOB == "FT")
        {
            ddlFrequency.Enabled = false;
            rfvFrequency.Enabled = false;
            rfvFrequencySave.Enabled = false;
            //lblFrequency.Attributes.Add("className","styleReqFieldLabel");
            lblFrequency.CssClass = "";
            ddlFrequency.SelectedIndex = 0;

        }
        //else if (strLOB == "TL" || strLOB == "TE")
        //{
        //    ddlFrequency.Enabled = true;
        //    rfvFrequency.Enabled = false;
        //    rfvFrequencySave.Enabled = false;
        //    lblFrequency.CssClass = "";
        //}
        else
        {
            ddlFrequency.Enabled = true;
            rfvFrequency.Enabled = true;
            rfvFrequencySave.Enabled = true;
            lblFrequency.CssClass = "styleReqFieldLabel";
        }
        Panel1.Visible = false;
        pnlIRDetails.Visible = false;
        ucCustomPaging.Visible = false;
        btnSave.Enabled = false;
        btnRevoke.Enabled = false;
    }

    protected void grvIRConsolidate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            HiddenField hdnIRStatus = (HiddenField)e.Row.FindControl("hdnIRStatus");
            Button btnPosting = (Button)e.Row.FindControl("btnPosting");
            ImageButton imgbtnQuery = (ImageButton)e.Row.FindControl("imgbtnQuery");
            ImageButton imgbtnPorting = (ImageButton)e.Row.FindControl("imgbtnPorting");


            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblPostingStatus = (Label)e.Row.FindControl("lblPostingStatus");
            Label lblMonthLock = (Label)e.Row.FindControl("lblMonthLock");
            CheckBox chkIR = (CheckBox)e.Row.FindControl("chkIR");
            CheckBox chkRevoke = (CheckBox)e.Row.FindControl("chkRevoke");


            if (hdnIRStatus.Value == "1")
            {
                chkIR.Enabled = false;
                lblStatus.Text = "Processed";
                imgbtnQuery.Enabled = true;
                imgbtnPorting.Enabled = true;

                imgbtnQuery.CssClass = "styleGridQuery";
                imgbtnPorting.CssClass = "styleGridQuery";
                chkRevoke.Enabled = true;
                btnRevoke.Enabled = true;
                if (lblPostingStatus.Text == "1")
                    btnPosting.Enabled = false;
                else
                    btnPosting.Enabled = true;
            }
            else if (hdnIRStatus.Value == "2")
            {
                lblStatus.Text = "Scheduled";
                chkIR.Enabled = false;
                chkRevoke.Enabled = true;
                btnRevoke.Enabled = true;
                btnPosting.Enabled = false;
            }
            else if (hdnIRStatus.Value == "3")
            {
                lblStatus.Text = "Reversed";
                chkRevoke.Enabled = false;
                btnPosting.Enabled = false;
            }
            else
            {
                lblStatus.Text = "Yet to Process";
                chkRevoke.Enabled = false;
                btnPosting.Enabled = false;
                intIRRows++;
                //hdnRows.Value = intIRRows;
            }

            if ((strMode == "Q"))
            {
                btnPosting.Enabled = false;
                chkRevoke.Enabled = false;
                btnRevoke.Enabled = false;
                chkIR.Enabled = false;
            }
            //Code added by saran on 30-Dec-2011 start
            if ((strMode == "M"))
            {
                chkIR.Enabled = false;
                if (lblMonthLock.Text == "True")
                {
                    chkRevoke.Enabled = false;
                }
                else
                {
                    chkRevoke.Enabled = true;
                    intRevoke = 1;
                }
            }
            //Code added by saran on 30-Dec-2011 end
            if (!bModify)
            {
                btnRevoke.Enabled = false;

            }
            if (!bCreate)
            {
                btnSave.Enabled = false;
            }

        }
    }

    protected void rbtnSchedule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnSchedule.SelectedValue == "0")
        {
            txtScheduleDate.Enabled = txtScheduleTime.Enabled = CECScheduleDate.Enabled = true;
            txtScheduleDate.Text = strDate;
            txtScheduleDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleDate.ClientID + "','" + strDateFormat + "',false,  true);");
            REVScheduleTime.Enabled = rfvScheduleDate.Enabled = rfvScheduleTime.Enabled = true;
        }
        else
        {
            txtScheduleDate.Text = txtScheduleTime.Text = "";
            txtScheduleDate.Enabled = txtScheduleTime.Enabled = CECScheduleDate.Enabled = false;
            txtScheduleDate.ReadOnly = true;
            txtScheduleDate.Attributes.Remove("onblur");
            REVScheduleTime.Enabled = rfvScheduleDate.Enabled = rfvScheduleTime.Enabled = false;
        }
    }

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        try
        {
            FunCalculate(2); //2 Revoke
            //if (dsIncome.Tables.Count > 1)
            //{
            //    grvIRConsolidate.DataSource = dsIncome.Tables[1];
            //    grvIRConsolidate.DataBind();
            //}

            if (intErrCode != 0)
            {
                if ((intErrCode == -1) || (intErrCode == -2) || (intErrCode == 50))
                    Utility.FunShowValidationMsg(this.Page, "", intErrCode);
                else
                    Utility.FunShowValidationMsg(this.Page, "CLN_CIR", intErrCode);
                return;
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, ValidationMsgs.CLN_CIR_Revoke, strRedirectPage);
                return;

                //if (dsIncome.Tables[1].Rows.Count == intIRRows)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Revocation done successfully for all the branches", strRedirectPage);
                //    return;
                //}
                //else
                //{
                //    Utility.FunShowAlertMsg(this.Page, ValidationMsgs.CLN_CIR_Revoke, strRedirectPage);
                //    return;
                //}
            }
            grvIncomeRecognition.DataSource = null;
            grvIncomeRecognition.DataBind();
            pnlIRDetails.Visible = false;
            ucCustomPaging.Visible = false;
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        //finally
        //{
        //    if (ObjIncomeRecognition != null)
        //        ObjIncomeRecognition.Close();
        //}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //if (!Utility.FunPriCompareLastDate(txtCutoffDate.Text))
            //{
            //    rfvCompareCutoffDate.ValidationGroup = "grpSave";
            //    rfvCompareCutoffDate.IsValid = false;
            //    return;
            //}
            //btnXLPorting.Enabled = false;
            if (rbtnSchedule.SelectedIndex == 0)
            {
                FunCalculate(3);
            }
            else
            {
                //It Result has to come without schedule process then use FunCalculate(1);otherwise use
                //FunCalculate(1);
                FunCalculate(3);
            }


            //if (intIncomeRecognitionID == 0)
            //{
            //    grvIRConsolidate.DataSource = dsIncome.Tables[0];
            //    grvIRConsolidate.DataBind();
            //}
            //else
            //{
            //    if (dsIncome.Tables.Count > 1)
            //    {
            //        grvIRConsolidate.DataSource = dsIncome.Tables[1];
            //        grvIRConsolidate.DataBind();
            //    }
            //    else
            //    {
            //        grvIRConsolidate.DataSource = dsIncome.Tables[0];
            //        grvIRConsolidate.DataBind();
            //    }
            //}

            //dsIncome.Tables[1]

            if (intErrCode == 0)
            {
                ddlLOB.Enabled = false;
                ddlFrequency.Enabled = false;
                txtCutoffDate.Enabled = false;
                CalendarExtender1.Enabled = false;
                btnClear.Enabled = false;
                btnGO.Enabled = false;


                Utility.FunShowAlertMsg(this.Page, "Income Recognition scheduled successfully", strRedirectPage);
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                return;

                ////if (grvIncomeRecognition.Rows.Count > 0)
                ////{
                ////    btnXLPorting.Enabled = true;
                ////}
                ////else
                ////{
                ////    btnXLPorting.Enabled = false;
                ////}
                //if (rbtnSchedule.SelectedIndex == 0)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Income Recognition scheduled successfully");
                //    return;
                //}
                //else if (intIncomeRecognitionID > 0)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Income Recognition calculation " + ValidationMsgs.S3G_ValMsg_Update, strRedirectPage);
                //    return;
                //}
                ////else
                ////{
                ////    Utility.FunShowAlertMsg(this.Page, "Income Recognition done successfully");
                ////    return;
                ////    //strAlert = "Income Recognition calculation " + strErrMsg + " done successfully";
                ////    //strAlert += @"\n" + ValidationMsgs.S3G_ValMsg_Next + " Income Recognition?";
                ////    //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                ////    //strRedirectPageView = "";
                ////    //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                ////    //return;
                ////}
            }
            else
            {
                if ((intErrCode == -1) || (intErrCode == -2) || (intErrCode == 50))
                    Utility.FunShowValidationMsg(this.Page, "", intErrCode);
                else
                    Utility.FunShowValidationMsg(this.Page, "CLN_CIR", intErrCode, strErrMsg, false);
                //Utility.FunShowValidationMsg(this.Page, "CLN_CIR", intErrCode);
                return;
            }

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        //finally
        //{
        //    if (ObjIncomeRecognition != null)
        //        ObjIncomeRecognition.Close();
        //}
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }
    protected void btnGO_Click(object sender, EventArgs e)
    {
        try
        {
            //if (!Utility.FunPriCompareLastDate(txtCutoffDate.Text))
            //{
            //    rfvCompareCutoffDate.ValidationGroup = "grpGO";
            //    rfvCompareCutoffDate.IsValid = false;
            //    return;
            //}
            Panel1.Visible = false;
            FunCalculate(4);
            if (dsIncome.Tables.Count > 0)
            {
                if (dsIncome.Tables[0].Rows.Count > 0)
                {
                    grvIRConsolidate.DataSource = dsIncome;
                    grvIRConsolidate.DataBind();
                    Panel1.Visible = true;
                    btnSave.Enabled = true;
                }
                else
                {
                    grvIRConsolidate.DataSource = null;
                    grvIRConsolidate.DataBind();
                    btnSave.Enabled = false;
                }
            }
            else
            {
                grvIRConsolidate.DataSource = null;
                grvIRConsolidate.DataBind();
                btnSave.Enabled = false;

            }
            if ((intErrCode == -1) || (intErrCode == -2) || (intErrCode == 53))
            {
                Utility.FunShowValidationMsg(this.Page, "", intErrCode);
                return;
            }
            else if (intErrCode > 0)
            {
                Utility.FunShowValidationMsg(this.Page, "CLN_CIR", intErrCode);
                return;
            }

        }

        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }

    }

    protected void grvIRConsolidate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        intIncomeCalGridID = Convert.ToInt32(e.CommandArgument.ToString());
        intIncomeRecognitionID = intIncomeCalGridID;
        hdnIncomeRecgId.Value = intIncomeCalGridID.ToString();

        switch (e.CommandName.ToLower())
        {
            case "query":
                FunExcelExport(false);
                break;

            case "porting":
                FunExcelExport(true);
                break;
            case "posting":
                FunCalculate(5);
                if (intErrCode == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Sys Journal Posting done successfully");
                    grvIRConsolidate.DataSource = dsIncome.Tables[1];
                    grvIRConsolidate.DataBind();
                }
                if (intErrCode == 50)
                {
                    Utility.FunShowValidationMsg(this.Page, "", intErrCode);
                }

                break;
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClear(true);
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }


    #endregion

    #region Page Methods

    private void FunExcelExport(bool blnExport)
    {
        //Changed by Thangam M on 08/Feb/2012 to solve nullable out param probs
        pnlIRDetails.Visible = false;
        S3GDALDBType S3G_DBType;
        S3G_DBType = FunPubGetDatabaseType();
        if (S3G_DBType == S3GDALDBType.SQL)
        {
            strProcName = "S3G_LOANAD_GetIncomeRecognition";
        }
        else
        {
            strProcName = "S3G_LOANAD_GetIncomeRecognitionSpool";
        }

        // Changes end

        //strProcName = "S3G_LOANAD_GetIncomeRecognition";
        if (blnExport)
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@IncomeRecognition_ID", intIncomeCalGridID.ToString());
            //Procparam.Add("@User_ID", intUserID);
            dsIncome = Utility.GetTableValues(strProcName, Procparam);
            grvIncomeRecognition.Columns[0].Visible = true;
            grvIncomeRecognition.DataSource = dsIncome;
            grvIncomeRecognition.DataBind();
            //grvIncomeRecognition.HeaderRow.Cells[3].Text = "Prime A/c Number";
            //grvIncomeRecognition.HeaderRow.Cells[4].Text = "Sub A/c Number";
            grvIncomeRecognition.HeaderRow.Cells[5].Text = "Income From Date";
            grvIncomeRecognition.HeaderRow.Cells[6].Text = "Income To Date";
            grvIncomeRecognition.HeaderRow.Cells[7].Text = "No of Days";
            grvIncomeRecognition.HeaderRow.Cells[8].Text = "Income Amount";

            string attachment = "attachment; filename=IncomeRecognition.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grvIncomeRecognition.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

        }
        else
        {
            FunPriBindGridPaging();
            grvIncomeRecognition.Columns[0].Visible = false;
            pnlIRDetails.Visible = true;
            ucCustomPaging.Visible = true;
        }
    }

    private void FunPriBindGridPaging()
    {
        FunPriAddCommonParameters();
        int intTotalRecords = 0;
        bool bIsNewRow = false;
        strProcName = "S3G_LOANAD_GetIncomeRecognition";
        grvIncomeRecognition.BindGridView(strProcName, Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

        //This is to hide first row if grid is empty
        if (bIsNewRow)
        {
            grvIncomeRecognition.Rows[0].Visible = false;
        }
        ucCustomPaging.Visible = true;
        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucCustomPaging.setPageSize(ProPageSizeRW);
        lblErrorMessage.Text = "";
    }

    private void FunPriAddCommonParameters()
    {
        //Paging Properties set  
        int intTotalRecords = 0;
        ObjPaging.ProCompany_ID = intCompanyID;
        ObjPaging.ProUser_ID = intUserID;
        ObjPaging.ProTotalRecords = intTotalRecords;
        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProSearchValue = "";
        ObjPaging.ProOrderBy = "";
        Procparam = new Dictionary<string, string>();
        if (Procparam != null)
        {
            Procparam.Clear();
        }
        Procparam.Add("@IncomeRecognition_ID", hdnIncomeRecgId.Value);

    }

    private void FunCalculate(int intAction)
    {
        ObjIncomeRecognition = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();

        try
        {
            StringBuilder strbBranch = new StringBuilder();
            strbBranch.Append("<Root> ");
            foreach (GridViewRow grvData in grvIRConsolidate.Rows)
            {

                CheckBox chkIR = null;
                //Below code to be changed on every page
                if ((intAction == 2) || (intAction == 5))
                {
                    chkIR = ((CheckBox)grvData.FindControl("chkRevoke"));
                }
                else
                {
                    chkIR = ((CheckBox)grvData.FindControl("chkIR"));
                }

                Label lblBranchID = ((Label)grvData.FindControl("lblBranchID"));
                Label lblNoofInstallment = ((Label)grvData.FindControl("lblNoofInstallment"));
                Label lblStatus = ((Label)grvData.FindControl("lblStatus"));

                if (chkIR.Checked)
                {
                    strbBranch.Append("<Details ");
                    //strbBranch.Append("Branch_ID='" + lblBranchID.Text + "' ");
                    strbBranch.Append("Location_ID='" + lblBranchID.Text + "' ");
                    strbBranch.Append("NoOfInst='" + lblNoofInstallment.Text + "' ");
                    //lblStatus.Text = "Processing...";
                    strbBranch.Append(" /> ");
                }

            }
            strbBranch.Append(" </Root>");

            strbBranch.Append("");

            //0 -  Calculate IR
            //1 -  Save
            //2 -  Revoke
            //3 -  Schedule
            //4 -  Get Records to Calculate

            Procparam = new Dictionary<string, string>();
            if ((intIncomeRecognitionID == 0) || (intAction == 2))
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Frequency_Code", ddlFrequency.SelectedValue);
                Procparam.Add("@CutOff_Date", Utility.StringToDate(txtCutoffDate.Text).ToString());
                Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@Action_Flag", intAction.ToString());
                Procparam.Add("@IncomeCalculation_ID", intIncomeRecognitionID.ToString());
                Procparam.Add("@XMLBranch", strbBranch.ToString());

            }
            else
            {
                //This is for during page load
                Procparam.Add("@XMLBranch", strbBranch.ToString());
                Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@Action_Flag", intAction.ToString());
                Procparam.Add("@IncomeCalculation_ID", intIncomeRecognitionID.ToString());
            }
            if ((rbtnSchedule.SelectedIndex == 0) && (txtScheduleDate.Text != ""))
            {
                Procparam.Add("@Schedule_Date", Utility.StringToDate(txtScheduleDate.Text).ToString());
                Procparam.Add("@Schedule_Time", txtScheduleTime.Text);
                Procparam.Add("@Schedule_Type", "0");
            }
            else
            {
                Procparam.Add("@Schedule_Date", DateTime.Now.ToShortDateString());
                Procparam.Add("@Schedule_Time", DateTime.Now.AddMinutes(-2.0).ToShortTimeString());
                Procparam.Add("@Schedule_Type", "1");
                //Procparam.Add("@Schedule_Date", Utility.StringToDate(txtScheduleDate.Text).ToString());
                //Procparam.Add("@Schedule_Time", txtScheduleTime.Text);
            }

            //ObjIncomeRecognition = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();


            byte[] byteDataset = ObjIncomeRecognition.FunPubGetIncomeRecognition(out intErrCode, out strErrMsg, "S3G_LOANAD_InsertIncomeRecognitionBulk", Procparam);
            dsIncome = (DataSet)ClsPubSerialize.DeSerialize(byteDataset, SerializationMode.Binary, typeof(DataSet));
            //ViewState["Income"] = dsIncome;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // if (ObjIncomeRecognition != null)
            ObjIncomeRecognition.Close();
        }
    }


    /// <summary>
    /// to bind LOB and Product details
    /// </summary>

    private void FunPriBindLOBBranchFrequency()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        if (intIncomeRecognitionID == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }
        Procparam.Add("@FilterType", "0");
        Procparam.Add("@FilterCode", "'OL'");

        Procparam.Add("@User_ID", intUserID.ToString());
        Procparam.Add("@Program_ID", "114");

        ddlLOB.BindDataTable(SPNames.S3G_ORG_GetSpecificLOBList, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        //Branch
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@User_ID", intUserID.ToString());
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Branch_ID", "Branch_Code", "Branch_Name" });

        //Frequency
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@LookupType_Code", "8");
        ddlFrequency.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

    }
    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunPriBindFrequency()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@LookupType_Code", "8");
        ddlFrequency.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });

    }
    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetIncomeRecognitionDetails()
    {

        try
        {
            FunCalculate(6);//Query Mode

            if (dsIncome.Tables.Count >= 2)
            {
                if (dsIncome.Tables[0].Rows.Count > 0)
                {
                    ddlLOB.SelectedValue = dsIncome.Tables[0].Rows[0]["LOB_ID"].ToString();
                    //ddlBranch.SelectedValue = dsIncome.Tables[0].Rows[0]["Branch_ID"].ToString();
                    ddlFrequency.SelectedValue = dsIncome.Tables[0].Rows[0]["Frequency_ID"].ToString();
                    txtCutoffDate.Text = DateTime.Parse(dsIncome.Tables[0].Rows[0]["CutOff_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    //txtCutoffDate.Text = dsIncome.Tables[0].Rows[0]["CutOff_Date"].ToString();

                    txtScheduleDate.Text = dsIncome.Tables[0].Rows[0]["Schedule_Date"].ToString();
                    txtScheduleTime.Text = dsIncome.Tables[0].Rows[0]["Schedule_Time"].ToString();

                }
                if (dsIncome.Tables[1].Rows.Count > 0)
                {
                    grvIRConsolidate.DataSource = dsIncome.Tables[1];
                    grvIRConsolidate.DataBind();
                    Panel1.Visible = true;
                    //ViewState["Income"] = dsIncome;
                }
            }
            else
            {
                if ((intErrCode == 2) || (intErrCode == 9))
                {
                    Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.ResourceManager.GetString("CLN_CIR_" + intErrCode.ToString()), strRedirectPage);
                    return;
                }
            }



        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            dsIncome.Dispose();
        }
    }

    //Code commented and added by saran on 30-Dec-2011 start

    /// <summary>
    /// This method is used to display Income recognition details
    /// </summary>
    private void FunGetIncomeRecognitionDetailsMod(int intheaderId)
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@IRHeader_ID", intheaderId.ToString());
            DataSet ds = Utility.GetDataset("S3G_LoanAd_GetIncomeRecDet", Procparam);


            if (ds.Tables.Count >= 2)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlLOB.Items.Add(new ListItem(ds.Tables[0].Rows[0]["LOB_Name"].ToString(), ds.Tables[0].Rows[0]["LOB_ID"].ToString()));
                    ddlLOB.ToolTip = ds.Tables[0].Rows[0]["LOB_Name"].ToString();
                    //ddlBranch.SelectedValue = dsIncome.Tables[0].Rows[0]["Branch_ID"].ToString();
                  //  FunPriBindFrequency();
                    ddlFrequency.Items.Add(new ListItem(ds.Tables[0].Rows[0]["Frequency_Name"].ToString(), ds.Tables[0].Rows[0]["Frequency_ID"].ToString()));
                    ddlFrequency.ToolTip = ds.Tables[0].Rows[0]["Frequency_Name"].ToString();
                    //ddlFrequency.SelectedValue = ds.Tables[0].Rows[0]["Frequency_ID"].ToString();
                    txtCutoffDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["CutOff_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    //txtCutoffDate.Text = dsIncome.Tables[0].Rows[0]["CutOff_Date"].ToString();
                    if (ds.Tables[0].Rows[0]["Schedule_Type"] != null)
                        rbtnSchedule.SelectedValue = Convert.ToInt32(ds.Tables[0].Rows[0]["Schedule_Type"]).ToString();
                    txtScheduleDate.Text = ds.Tables[0].Rows[0]["Schedule_Date"].ToString();
                    txtScheduleTime.Text = ds.Tables[0].Rows[0]["Schedule_Time"].ToString();

                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    grvIRConsolidate.DataSource = ds.Tables[1];
                    grvIRConsolidate.DataBind();
                    Panel1.Visible = true;
                    //ViewState["Income"] = dsIncome;
                }
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    //Code commented and added by saran on 30-Dec-2011 end
    private void FunPriClear(bool bClearAll)
    {
        if (bClearAll)
        {
            ddlLOB.SelectedIndex = 0;
            //ddlBranch.SelectedIndex = 0;
            ddlFrequency.SelectedIndex = 0;
            txtCutoffDate.Text = "";
            txtScheduleDate.Text = "";
            txtScheduleTime.Text = "";
            grvIRConsolidate.DataSource = null;
            grvIRConsolidate.DataBind();

            Panel1.Visible = false;
            pnlIRDetails.Visible = false;
            ucCustomPaging.Visible = false;
            btnSave.Enabled = false;
            btnRevoke.Enabled = false;

            grvIncomeRecognition.DataSource = null;
            grvIncomeRecognition.DataBind();
            //btnXLPorting.Enabled = false;
            txtCutoffDate.Text = DateTime.Now.ToString(strDateFormat);
            txtScheduleDate.Text = DateTime.Now.ToString(strDateFormat);
            rbtnSchedule.SelectedIndex = 0;

            txtScheduleDate.Enabled = txtScheduleTime.Enabled = CECScheduleDate.Enabled = true;
            txtScheduleDate.Text = strDate;
            REVScheduleTime.Enabled = rfvScheduleDate.Enabled = rfvScheduleTime.Enabled = true;
        }

    }

    ////This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                if (!bCreate)
                {
                    btnGO.Enabled = false;
                    btnSave.Enabled = false;

                }

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                //if (!bModify)
                //{
                //    btnSave.Enabled = false;
                //}
                ddlLOB.Enabled = false;
                //ddlBranch.Enabled = false;
                ddlFrequency.Enabled = false;
                txtCutoffDate.Enabled = false;
                CalendarExtender1.Enabled = false;
                btnClear.Enabled = false;
                btnCalculate.Enabled = false;
                //btnXLPorting.Enabled = true;
                btnRevoke.Visible = true;
                btnSave.Enabled = false;
                btnGO.Enabled = false;
                txtScheduleDate.ReadOnly = true;
                txtScheduleTime.ReadOnly = true;
                CalendarExtender1.Enabled = false;
                rbtnSchedule.Enabled = false;
                CECScheduleDate.Enabled = false;
                txtScheduleDate.ReadOnly = txtCutoffDate.ReadOnly = true;
                txtCutoffDate.Attributes.Remove("onblur");
                txtScheduleDate.Attributes.Remove("onblur");
                break;

            case -1:// Query Mode


                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }

                if (bClearList)
                {
                    ddlLOB.ClearDropDownList();
                    //ddlBranch.ClearDropDownList();
                    ddlFrequency.ClearDropDownList();
                }
                ddlLOB.Enabled = true;
                //ddlBranch.Enabled = true;
                ddlFrequency.Enabled = true;
                txtCutoffDate.ReadOnly = true;
                CalendarExtender1.Enabled = false;
                btnClear.Enabled = false;
                btnGO.Enabled = false;
                btnSave.Enabled = false;
                btnRevoke.Enabled = false;
                txtScheduleDate.ReadOnly = true;
                txtScheduleTime.ReadOnly = true;
                rbtnSchedule.Enabled = false;
                CECScheduleDate.Enabled = false;
                txtScheduleDate.ReadOnly = txtCutoffDate.ReadOnly = true;
                txtCutoffDate.Attributes.Remove("onblur");
                txtScheduleDate.Attributes.Remove("onblur");
                break;
        }

    }

    #endregion

}



