//Module Name      :   Loan Admin
//Screen Name      :   S3GLoanAdOverDueInterest.aspx
//Created By       :   Irsathameen K
//Created Date     :   04-Sep-2010
//Purpose          :   To insert and Query Over Due Interest
// Modified By     :   Vijaya Kumar R
// Modified Date   :   29-12-2010
// Purpose         :   To Implement the Missing functionalities. 
#region Namespaces

using System;
using System.Collections;
using System.Web.UI;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Text;
using S3GBusEntity.LoanAdmin;
using S3GBusEntity;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using LoanAdminAccMgtServicesReference;
using System.ServiceProcess;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using QRCoder;
using System.Drawing;
#endregion

public partial class LoanAdmin_S3GLoanAdOverDueInterest : ApplyThemeForProject
{
    #region Variable declaration

    string intOverdueInterestID = "0";
    int intErrCode = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    string strDate = DateTime.Now.ToString();
    public string strProgramId = "72";

    //User Authorization
    public string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    Dictionary<string, string> ObjDictionary = null;

    LoanAdminAccMgtServicesClient ObjLoanAdminAccMgtServicesClient = null;
    LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable ObjS3G_LOANAD_ODICalculationsDataTable = null;
    LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsRow ObjS3G_LOANAD_ODICalculationsRow = null;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;

    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=COI";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strDateFormat = string.Empty;
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";

    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdOverDueInterest.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=COI';";
    string strServiceName = "S3gService";
    static string strPageName = "Over Due Interest";
    public static LoanAdmin_S3GLoanAdOverDueInterest obj_Page;
    DataTable dtdata = new DataTable();
    #endregion

    #region Events

    #region Page Event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            FunPriLoadPage();
            FunPubSetErrorMessage();

            if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
            {
                txtCurrentODIDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCurrentODIDate.ClientID + "','" + strDateFormat + "',true,  false);");
            }
            else
            {
                txtCurrentODIDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtCurrentODIDate.ClientID + "','" + strDateFormat + "',false,  false);");
            }
            txtScheduleDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleDate.ClientID + "','" + strDateFormat + "',false,  true);");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad; // "Due to Data Problem, Unable to Load the Over Due Interest";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        txtCustomerName.Text = hidCustId.Value = "";
        try
        {
            hidCustId.Value = (ucCustomerCode.FindControl("hdnID") as HiddenField).Value;
            TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
            if (txtName.Text.Split('-').Length > 1)
                txtCustomerName.Text = txtName.Text.Split('-')[txtName.Text.Split('-').Length - 1].ToString();
            LoadTranche(hidCustId.Value);
            FunPriPopulatePANum();
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();

            objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            ObjDictionary = new Dictionary<string, string>();
            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            ObjDictionary.Add("@Customer_ID", hidCustId.Value);
            txtODIRate.Text = objS3GAdminServicesClient.FunGetScalarValue("S3G_LOANAD_GetODIRate", ObjDictionary);


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadPrimeACNum;
            cvOverDueInterest.IsValid = false;
        }
    }

    #endregion

    #region Button Events

    #region Common Control Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            #region Validation
            if (strMode != "Q")
            {
                if (!FunPriIsValid())
                {
                    return;
                }
            }
            #endregion
            FunPriSaveOverDueInterest();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_SaveODI; //"Unable to Insert the Over Due Interest";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearPage();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_ClearAll; //"Unable to Clear the data";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //PreviewPDF_Click();
        FunPrintCoveringLetter();
    }

    protected void btnPrintAnnex_Click(object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, string> dictLAS = new Dictionary<string, string>();
            dictLAS.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictLAS.Add("@ODI_Calculation_ID", Convert.ToString(intOverdueInterestID));

            DataTable dtSale = new DataTable();

            DataSet dsLAS = Utility.GetDataset("S3G_LoanAd_ODI_Annexure", dictLAS);
            dtSale = dsLAS.Tables[1];
            GridView Grv = new GridView();

            Grv.DataSource = dtSale;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Annex-ODI.xls";
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
                row["Column1"] = "Annexure for Delay Charges Invoice Reference No " + dsLAS.Tables[0].Rows[0]["ODI_Invoice_No"].ToString();
                dtHeader.Rows.Add(row);

                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 10;
                grv1.Rows[1].Cells[0].ColumnSpan = 10;

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
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #region Other Button Events

    protected void btnRevoke_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriRevoke();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_ODIRevoke; //"Unable to Revoke the data";
            cvOverDueInterest.IsValid = false;
        }
    }
    #endregion

    #endregion

    #region DropDown List Events

    protected void ddlPrimeAccountNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtLastODICalculationDate.Text = "";
            if (ddlPrimeAccountNo.SelectedIndex > 0)
                FunBindLastODIDate();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadSubACNum; //"Due to Data Problem, Unable to Load the SANum";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ddlTrancheName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtLastODICalculationDate.Text = "";
            FunPriPopulatePANum();
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadSubACNum; //"Due to Data Problem, Unable to Load the SANum";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void LoadTranche(string strCustomer_ID)
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@Customer_ID", strCustomer_ID);
            ddlTrancheName.Items.Clear();
            ddlTrancheName.BindDataTable("S3G_CLN_GetTranche_ODI", ObjDictionary, new string[] { "Tranche_Header_Id", "Tranche_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunBindLastODIDate()
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (ddlBranch.SelectedValue != "0")
                ObjDictionary.Add("@Location_ID", ddlBranch.SelectedValue);
            ObjDictionary.Add("@PA_SA_Ref_ID", ddlPrimeAccountNo.SelectedValue);
            DataTable dtLastODIDate = Utility.GetDefaultData("S3G_lOANAD_OVERDUE_LASTODI", ObjDictionary);

            if (dtLastODIDate != null && dtLastODIDate.Rows.Count > 0)
            {
                txtLastODICalculationDate.Text = dtLastODIDate.Rows[0]["LAST_ODI_DATE"].ToString();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriPopulateCustomer();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadCustomer; //"Due to Data Problem, Unable to Load the Customer";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();

        try
        {
            FunPriPopulateCustomer();
            FunPriBindLocation();

            if (ddlLOB.SelectedValue != "0")
            {
                ObjDictionary = new Dictionary<string, string>();
                ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
                ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
                txtODIRate.Text = objS3GAdminServicesClient.FunGetScalarValue("S3G_LOANAD_GetODIRate", ObjDictionary);
            }
            else
            {
                txtODIRate.Text = "";
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadCustomer; // "Due to Data Problem, Unable to Load the Customer";
            cvOverDueInterest.IsValid = false;
        }
        finally
        {
            objS3GAdminServicesClient.Close();
        }
    }
    #endregion

    #region Other Control Events

    protected void ChkHeadODI_CheckedChanged(object sender, EventArgs e)
    {
        string strLocation = "";
        try
        {
            CheckBox ChkHeadODI = (CheckBox)sender;
            foreach (GridViewRow gvRow in grvODI.Rows)
            {
                CheckBox ChkODI = gvRow.FindControl("ChkODI") as CheckBox;
                if (ChkODI.Enabled)
                {
                    if (ChkHeadODI.Checked)
                        ChkODI.Checked = true;
                    else
                        ChkODI.Checked = false;
                }
                if (ChkODI.Checked && (gvRow.FindControl("hidBillStatus") as HiddenField).Value == "0")
                {
                    if (strLocation == "")
                        strLocation = "(" + (gvRow.FindControl("txtBranchName") as Label).Text;
                    else
                        strLocation = strLocation + "," + (gvRow.FindControl("txtBranchName") as Label).Text;
                }

            }
            if (strLocation != "")
            {
                Utility.FunShowAlertMsg(this, "Billing is not done for " + strLocation + ")");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ChkODI_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkODI = (CheckBox)sender;
            CheckBox ChkHeadODI = grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox;
            if (ViewState["dtdata"] != null)
                dtdata = (System.Data.DataTable)ViewState["dtdata"];
            else
            {
                if (!dtdata.Columns.Contains("Location_Id"))
                    dtdata.Columns.Add("Location_id");
                if (!dtdata.Columns.Contains("Customer_id"))
                    dtdata.Columns.Add("Customer_Id");
                if (!dtdata.Columns.Contains("Tranche_Id"))
                    dtdata.Columns.Add("Tranche_Id");

            }
            Label txtBranchId = ((CheckBox)sender).Parent.Parent.FindControl("txtBranchId") as Label;
            Label txtCustomerId = ((CheckBox)sender).Parent.Parent.FindControl("txtCustomerId") as Label;
            Label txtTrancheId = ((CheckBox)sender).Parent.Parent.FindControl("txtTrancheId") as Label;
            if (ChkODI.Checked)
            {
                HiddenField hidBillStatus = ((CheckBox)sender).Parent.Parent.FindControl("hidBillStatus") as HiddenField;
               
                if (hidBillStatus.Value.ToUpper() == "0")
                {
                    Utility.FunShowAlertMsg(this, "Billing not done for selected Location");
                    
                }
               
              
                    DataRow drdata = dtdata.NewRow();
                    drdata["Location_Id"] = txtBranchId.Text;
                    drdata["Customer_id"] = txtCustomerId.Text;
                     drdata["Tranche_Id"] = txtTrancheId.Text;
                    dtdata.Rows.Add(drdata);


                    ViewState["dtdata"] = dtdata;

            }

            if (!ChkODI.Checked) ChkHeadODI.Checked = false;
            else
            {
                ChkHeadODI.Checked = true;
                foreach (GridViewRow gvRow in grvODI.Rows)
                {
                    CheckBox ChkODINew = gvRow.FindControl("ChkODI") as CheckBox;
                    if (!ChkODINew.Checked && ChkODINew.Enabled)
                    {
                        ChkHeadODI.Checked = false;
                        break;
                    }
                }

                //DataRow[] drdata = dtdata.Select("Location_Id=" + txtBranchId.Text + " and Customer_id=" + txtCustomerId.Text + " and Tranche_Id=" + txtTrancheId.Text + "");
                //if (drdata.Length > 0)
                //    {
                //        drdata[0].Delete();
                //    }
                //dtdata.AcceptChanges();
                ViewState["dtdata"] = dtdata;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ChkHeadRevoke_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkHeadRevoke = (CheckBox)sender;
            foreach (GridViewRow gvRow in grvODI.Rows)
            {
                CheckBox ChkRevoke = gvRow.FindControl("ChkRevoke") as CheckBox;
                if (ChkRevoke.Enabled)
                {
                    if (ChkHeadRevoke.Checked)
                        ChkRevoke.Checked = true;
                    else
                        ChkRevoke.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void ChkRevoke_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkRevoke = (CheckBox)sender;
            CheckBox ChkHeadRevoke = grvODI.HeaderRow.FindControl("ChkHeadRevoke") as CheckBox;
            if (!ChkRevoke.Checked) ChkHeadRevoke.Checked = false;
            else
            {
                ChkHeadRevoke.Checked = true;
                foreach (GridViewRow gvRow in grvODI.Rows)
                {
                    CheckBox ChkRevokeNew = gvRow.FindControl("ChkRevoke") as CheckBox;
                    if (!ChkRevokeNew.Checked && ChkRevokeNew.Enabled)
                    {
                        ChkHeadRevoke.Checked = false;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void txtCurrentODIDate_TextChanged(object sender, EventArgs e)
    {
        panSchedule.Visible = false;
        grvODI.DataSource = null;
        grvODI.DataBind();

        if (txtCurrentODIDate.Text != "")
        {

        }

    }

    protected void btnCalculateODI_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriCalcualteODI();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_CalculateODI;// "Due to Data Problem, Unable to Calulate ODI";
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void rblAllSpecific_SelectedIndexChanged(object sender, EventArgs e)
    {
        panSchedule.Visible = false;
        grvODI.DataSource = null;
        grvODI.DataBind();

        txtLastODICalculationDate.Text = "";

        try
        {
            Button btnGetLOV = ucCustomerCode.FindControl("btnGetLOV") as Button;
            TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
            txtName.Text = "";

            if (rblAllSpecific.SelectedValue.ToUpper() == "ALL")
            {
                btnSave.Enabled = true;
                ddlPrimeAccountNo.Enabled = ddlTrancheName.Enabled = false;
                txtScheduleDate.ReadOnly = txtScheduleTime.ReadOnly = false;
                cmbCustomerCode.ReadOnly = true;
                btnGetLOV.Enabled = false;
                if (ddlPrimeAccountNo.Items.Count > 0) ddlPrimeAccountNo.SelectedIndex = 0;
                if (ddlTrancheName.Items.Count > 0) ddlTrancheName.SelectedIndex = 0;
                cmbCustomerCode.Text = hidCustId.Value = "";
                RFVPrimeAccountNo.Enabled = false;
                rbtnSchedule.Enabled = RFVScheduleTime.Enabled = RFVScheduleDate.Enabled = REVScheduleTime.Enabled = CECScheduleDate.Enabled = true;
                txtCustomerName.Text = ""; //Source added by Tamilselvan.S on 27/01/2011
                ddlBranch.Enabled = rfvCALPrimeAC.Enabled = false;//= rfvCALcustomerCode.Enabled
                ddlBranch.ReadOnly = true;
            }
            else
            {
                //btnSave.Enabled = false;
                ddlPrimeAccountNo.Enabled = ddlTrancheName.Enabled = true;
                cmbCustomerCode.ReadOnly = false;
                txtScheduleDate.ReadOnly = txtScheduleTime.ReadOnly = true;
                RFVPrimeAccountNo.Enabled = false; //RFVCustomerCode.Enabled =
                rbtnSchedule.Enabled = RFVScheduleTime.Enabled = RFVScheduleDate.Enabled = REVScheduleTime.Enabled = CECScheduleDate.Enabled = false;
                txtScheduleDate.Text = txtScheduleTime.Text = "";
                rfvCALPrimeAC.Enabled = false;// = rfvCALcustomerCode.Enabled
                ddlBranch.Enabled = true;
                ddlBranch.ReadOnly = false;
                btnGetLOV.Enabled = true;
                txtCurrentODIDate.Text = DateTime.Today.ToString(strDateFormat);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadODI;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void cmbCustomerCode_TextChanged(object sender, EventArgs e)
    {
        txtCustomerName.Text = hidCustId.Value = "";
        try
        {
            //Source modified by Tamilselvan.S on 27/01/2011
            if (System.Web.HttpContext.Current.Session["CustomerDT"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["CustomerDT"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    string filterExpression = "Customer_Code = '" + cmbCustomerCode.Text + "'";
                    DataRow[] dtSuggestions = dt.Select(filterExpression);
                    if (dtSuggestions.Length > 0)
                    {
                        hidCustId.Value = dtSuggestions[0]["Customer_ID"].ToString();
                        txtCustomerName.Text = dtSuggestions[0]["Customer_Name"].ToString();
                    }
                }
                FunPriPopulatePANum();
                panSchedule.Visible = false;
                grvODI.DataSource = null;
                grvODI.DataBind();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadPrimeACNum;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void grvODI_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField ODIStatus = (HiddenField)e.Row.FindControl("ODIStatus");
                CheckBox ChkODI = (CheckBox)e.Row.FindControl("ChkODI");
                HiddenField hidRevoke = (HiddenField)e.Row.FindControl("hidRevoke");
                CheckBox ChkRevoke = (CheckBox)e.Row.FindControl("ChkRevoke");

                if (ODIStatus.Value == "1")
                {
                    ChkODI.Checked = true;
                    ChkODI.Enabled = false;
                }
                if (hidRevoke.Value == "1")
                {
                    ChkRevoke.Checked = true;
                    ChkRevoke.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    protected void rbtnSchedule_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbtnSchedule.SelectedValue == "0")
            {
                txtScheduleDate.Enabled = txtScheduleTime.Enabled = CECScheduleDate.Enabled = true;
                txtScheduleDate.Text = strDate;
                REVScheduleTime.Enabled = RFVScheduleDate.Enabled = RFVScheduleTime.Enabled = true;
            }
            else
            {
                txtScheduleDate.Text = txtScheduleTime.Text = "";
                txtScheduleDate.Enabled = txtScheduleTime.Enabled = CECScheduleDate.Enabled = false;
                REVScheduleTime.Enabled = RFVScheduleDate.Enabled = RFVScheduleTime.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = ex.Message;
            cvOverDueInterest.IsValid = false;
        }
    }

    #endregion

    #endregion

    #region Methods

    private void FunPriRevoke()
    {
        string strODIId = string.Empty;
        foreach (GridViewRow gvRow in grvODI.Rows)
        {
            CheckBox ChkRevoke = (CheckBox)gvRow.FindControl("ChkRevoke");
            HiddenField ODIID = (HiddenField)gvRow.FindControl("ODIID");
            if (ChkRevoke.Checked && ChkRevoke.Enabled)
            {
                if (strODIId == string.Empty)
                    strODIId = ODIID.Value;
                else
                    strODIId = strODIId + "," + ODIID.Value;
            }
        }

        if (String.IsNullOrEmpty(strODIId))
        {
            cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select atleast one location for Revoke ";
            cvOverDueInterest.IsValid = false;
            return;
        }

        ObjLoanAdminAccMgtServicesClient = new LoanAdminAccMgtServicesClient();
        try
        {
            string strResult = ObjLoanAdminAccMgtServicesClient.FunPubRevokeODI(intCompanyID, strODIId, intUserID);
            if (strResult == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + Resources.ValidationMsgs.S3G_SucMsg_RevokeODI + "');" + strRedirectPageView, true);
            }
            else if (strResult == "5")
            {
                Utility.FunShowAlertMsg(this.Page, "Month is locked.Unable to Revoke Over Due Interest");
                return;
            }
            else if (strResult == "6")
            {
                Utility.FunShowAlertMsg(this.Page, "Previous month is not locked.Unable to Revoke Over Due Interest");
                return;
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Error in Revoking");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_RevokeODI;
            cvOverDueInterest.IsValid = false;
        }
        finally
        {
            if (ObjLoanAdminAccMgtServicesClient != null)
                ObjLoanAdminAccMgtServicesClient.Close();
        }
    }

    private void FunPriCalcualteODI()
    {
        TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
        if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
        {
            if (ddlPrimeAccountNo.Items.Count == 0) txtName.Focus();
            if (txtName.Text.Trim() == "")
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select the Customer Code";
                cvOverDueInterest.IsValid = false;
                return;
            }
            if (ddlPrimeAccountNo.Items.Count == 0)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select the Prime Account No";
                cvOverDueInterest.IsValid = false;
                return;
            }
        }
        if (txtCurrentODIDate.Text != "")
        {
            if (Utility.CompareDates(txtCurrentODIDate.Text, strDate) == -1)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalDate_NGT_CD; //" Current ODI Calculation Date should not be greater than Current Date  ";
                cvOverDueInterest.IsValid = false;
                return;
            }
            if (txtLastODICalculationDate.Text != "")
            {
                int intDateDiff = Utility.CompareDates(txtCurrentODIDate.Text, txtLastODICalculationDate.Text);
                if (intDateDiff == 0 || intDateDiff == 1)
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalDate_GT_LastODI; //" Current ODI Calculation Date should be greater than Last ODI Calculation Date  ";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
            }
        }

        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ObjDictionary.Add("@Lob_Id", ddlLOB.SelectedValue);
            ObjDictionary.Add("@Is_Active", "1");

            DataTable dtGPSODI = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterOverdue", ObjDictionary);
            if (dtGPSODI != null && dtGPSODI.Rows.Count > 0)
            {
                if (dtGPSODI.Rows[0]["Parameter_Value"].ToString() == "0")
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Denominator Days can not be zero in Global Parameter Setup";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
                else if (dtGPSODI.Rows[0]["Parameter_Value"].ToString() == "")
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Assign the Denominator Days  in global parameter setup ";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
                else if (dtGPSODI.Rows[0]["Parameter_Value1"].ToString() == "")
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Assign the grace days  in global parameter setup ";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
                else if (dtGPSODI.Rows[0]["Parameter_Value2"].ToString() == "")
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Assign the rate in global parameter setup ";
                    cvOverDueInterest.IsValid = false;
                    return;
                }
            }
            else
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Assign the Denominator Days,Rate and Grace days in global parameter setup ";
                cvOverDueInterest.IsValid = false;
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_CalODI;
            cvOverDueInterest.IsValid = false;
        }

        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ObjDictionary.Add("@Lob_Id", ddlLOB.SelectedValue);
            if (ddlBranch.SelectedValue != "0") ObjDictionary.Add("@Location_Id", ddlBranch.SelectedValue);
            if (ddlPrimeAccountNo.SelectedIndex != 0 && ddlPrimeAccountNo.Items.Count > 0)
                ObjDictionary.Add("@PA_SA_Ref_ID", ddlPrimeAccountNo.SelectedValue);
            if (ddlTrancheName.SelectedIndex != 0 && ddlTrancheName.Items.Count > 0)
                ObjDictionary.Add("@Tranch_Header_ID", ddlTrancheName.SelectedValue);
            if (hidCustId.Value != "")
                ObjDictionary.Add("@Customer_ID", hidCustId.Value);
            ObjDictionary.Add("@ODIDATE", Utility.StringToDate(txtCurrentODIDate.Text).ToString());
            ObjDictionary.Add("@Type", rblAllSpecific.SelectedValue);
            ObjDictionary.Add("@Mode", "1");

            DataSet dsCalCulateODI = Utility.GetDataset("S3G_LOANAD_OVERDUECALCULATION", ObjDictionary);

            if (dsCalCulateODI.Tables.Count == 1)//New Code
            {//New Code
                if (Convert.ToInt32(dsCalCulateODI.Tables[0].Rows[0][0]) == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, Utility.StringToDate(txtCurrentODIDate.Text).AddMonths(-3).ToString("MMM")+ "-" + Utility.StringToDate(txtCurrentODIDate.Text).ToString("yyyy")+" should be locked");
                    //To Calculate ODI Current Month Should be in Open Status and Previous Month Should be Locked
                    return;
                }
            }
            else
            {
                panSchedule.Visible = true;
                if (dsCalCulateODI.Tables[1] != null && dsCalCulateODI.Tables[1].Rows.Count > 0)
                {
                    grvODI.DataSource = dsCalCulateODI.Tables[1];
                    grvODI.DataBind();
                    if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
                    {
                        if (dsCalCulateODI.Tables[1].Rows[0]["Last_Calculated_Date"].ToString() != "")
                            txtLastODICalculationDate.Text = DateTime.Parse(dsCalCulateODI.Tables[1].Rows[0]["Last_Calculated_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);// ; ;
                    }
                }
                else
                {
                    grvODI.DataSource = null;
                    grvODI.EmptyDataText = "No Over Due for the Current ODI Calculation Date";
                    grvODI.DataBind();

                }
                FunPriCtrlDisable();
            }//New Code
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_CalODI;
            cvOverDueInterest.IsValid = false;
            //throw new ApplicationException("Unable to calculate the Over Due Interest");
        }
        finally
        {
            ObjDictionary = null;
        }
    }

    protected void FunPriBindLocation()
    {
        //Branch
        try
        {
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadODI;
            cvOverDueInterest.IsValid = false;
            //throw new ApplicationException("Unable to Load the Over Due Interest");
        }
    }


    protected void FunPriBindBranchLOB()
    {
        //Branch
        try
        {
            //LOB
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Program_ID", strProgramId);
            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (intOverdueInterestID == "0") ObjDictionary.Add("@Is_Active", "1");
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ddlLOB.BindDataTable(SPNames.S3G_LOANAD_GetOverDueInterestLOB, ObjDictionary, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            if (Request.QueryString["qsViewId"] != "C")
            {
                if (ddlLOB.Items.Count == 2)
                {
                    ddlLOB.SelectedIndex = 1;
                    ddlLOB.ClearDropDownList();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvOverDueInterest.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoadODI;
            cvOverDueInterest.IsValid = false;
            //throw new ApplicationException("Unable to Load the Over Due Interest");
        }
    }

    private void FunPriPopulatePANum()
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@Customer_ID", hidCustId.Value);
            ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (intOverdueInterestID == "0") ObjDictionary.Add("@Is_Active", "1");
            if (ddlBranch.SelectedValue != "0") ObjDictionary.Add("@Location_ID", ddlBranch.SelectedValue);
            if (ddlTrancheName.SelectedValue != "0")
            {
                ObjDictionary.Add("@Tranch_Header_ID", ddlTrancheName.SelectedValue);
                ddlPrimeAccountNo.BindDataTable("S3G_lOANAD_OVERDUE_PANUM", ObjDictionary, false, new string[] { "PA_SA_Ref_ID", "PANum" });
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("All", "0");
                ddlPrimeAccountNo.Items.Insert(0, liSelect);
                //RFVPrimeAccountNo.Enabled = rfvCALPrimeAC.Enabled = false;
            }
            else
            {
                ddlPrimeAccountNo.BindDataTable("S3G_lOANAD_OVERDUE_PANUM", ObjDictionary, false, new string[] { "PA_SA_Ref_ID", "PANum" });
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("All", "0");
                ddlPrimeAccountNo.Items.Insert(0, liSelect);
                //RFVPrimeAccountNo.Enabled = rfvCALPrimeAC.Enabled = true;
            }
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to Load the Prime Account No");
        }
    }

    private void FunPriPopulateCustomer()
    {
        TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
        txtName.Text = "";
        hidCustId.Value = cmbCustomerCode.Text = txtLastODICalculationDate.Text = "";
        txtCustomerName.Text = ""; //Source added by Tamilselvan.S on 27/01/2011
        ddlPrimeAccountNo.Items.Clear();
        panSchedule.Visible = false;
        grvODI.DataSource = null;
        grvODI.DataBind();

        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            if (ddlBranch.SelectedValue != "0") ObjDictionary.Add("@Location_ID", ddlBranch.SelectedValue);
            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            ObjDictionary.Add("@LOB_ID", ddlLOB.SelectedValue);
            //System.Web.HttpContext.Current.Session["CustomerDT"] = Utility.GetDefaultData(SPNames.S3G_CLN_GetCustomer, ObjDictionary);// true, "--All Customers--", new string[] { "Customer_ID", "Customer_Code", "Customer_Name" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to Load the Customer");
        }
    }

    private void FunPriLoadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            ServiceController sc;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            // Change the Date Format
            //Begin
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            strDate = DateTime.Now.ToString(strDateFormat);
            CECCurODICalcDate.Format = CECScheduleDate.Format = strDateFormat;
            //End

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intOverdueInterestID = fromTicket.Name;
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            //Code end  

            if (!IsPostBack)
            {
                txtCurrentODIDate.Text = txtScheduleDate.Text = strDate;
                CECCurODICalcDate.Format = CECScheduleDate.Format = strDateFormat;

                //User Authorization            
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                if ((intOverdueInterestID != "0") && (strMode == "M")) // Query // Modify
                {
                    FunPriGetOverDueInterest();
                    FunPriDisableControls(3);
                }
                else if ((intOverdueInterestID != "0") && (strMode == "Q")) // Query // Modify
                {
                    FunPriGetOverDueInterest();
                    FunPriDisableControls(2);
                }
                else
                {//LOB Load Only Create Mode By Shibu Performance Improvement Work 
                    FunPriBindBranchLOB();
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    ddlCustomer.Enabled = false;
                    btnPrint.Enabled = false;
                    TabContainer.Tabs[1].Enabled = false;
                }

            }
            #region "User Control"

            TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
            txtName.Width = System.Web.UI.WebControls.Unit.Pixel(170);
            Button btnGetLOV = ucCustomerCode.FindControl("btnGetLOV") as Button;
            if (rblAllSpecific.SelectedValue.ToUpper() == "ALL" || strMode == "M" || strMode == "Q")
                btnGetLOV.Enabled = false;
            else
                btnGetLOV.Enabled = true;
            txtName.Attributes.Add("ReadOnly", "true");
            txtName.Attributes.Add("onfocus", "fnLoadCustomer();");

            ucCustomerCode.strLOBID = ddlLOB.SelectedValue;
            if (ddlBranch.SelectedValue != "0") ucCustomerCode.strBranchID = ddlBranch.SelectedValue;
            ucCustomerCode.strControlID = ucCustomerCode.ClientID.ToString();
            txtLastODICalculationDate.Attributes.Add("readonly", "readonly");
            txtODIRate.Attributes.Add("readonly", "readonly");
            //FunSetHeight();

            #endregion
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            // throw new ApplicationException("Unable to Load the Over Due Interest");
        }
    }

    string strnewFile = "";
    protected void PreviewPDF_Click()
    {
        try
        {
            if (ddlCustomer.SelectedValue != "0")
            {
                Guid objGuid;
                objGuid = Guid.NewGuid();

                strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + objGuid + "ODI.pdf");

                DataSet Dst = null;

                if (ObjDictionary != null)
                    ObjDictionary.Clear();
                else
                    ObjDictionary = new Dictionary<string, string>();

                ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
                ObjDictionary.Add("@Customer_ID", ddlCustomer.SelectedValue);
                ObjDictionary.Add("@ODI_Calculation_ID", Convert.ToString(intOverdueInterestID));
                ObjDictionary.Add("@User_ID", intUserID.ToString());

                DataSet dsODI = Utility.GetDataset("S3G_LOANAD_PrintOverDueInterest", ObjDictionary);
                if (dsODI != null && dsODI.Tables[0].Rows.Count > 0)
                {
                    //ReportDocument rptd = new ReportDocument();
                    //rptd.Load(Server.MapPath("ODI.rpt"));
                    //rptd.SetDataSource(dsODI.Tables[0]);
                    //rptd.Subreports["Subreport"].SetDataSource(dsODI.Tables[1]);
                    //rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);

                    string strScipt = "";
                    strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strnewFile.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('No Over Due Interest Details for Selected Customer');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select a Customer');", true);
            }
        }
        catch (IOException winOpne)
        {
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPrintCoveringLetter()
    {
        try
        { 
            string strHTML = string.Empty;
            strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyID, Convert.ToInt32(ddlLOB.SelectedValue)
                    , Convert.ToInt32(ddlBranch.SelectedValue), 14, intOverdueInterestID);

            if (strHTML == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined");
                return;
            }

            string FileName = PDFPageSetup.FunPubGetFileName(intOverdueInterestID + intUserID + DateTime.Now.ToString("ddMMMyyyyHHmmss"));

            if (ddlPrintType.SelectedValue == "P")
            {
                string FilePath = Server.MapPath(".") + "\\PDF Files\\";
                string DownFile = FilePath + FileName + ".pdf";
                SaveDocument(strHTML, intOverdueInterestID, FilePath, FileName);
                if (!File.Exists(DownFile))
                {
                    Utility.FunShowAlertMsg(this, "File not exists");
                    return;
                }

                //Response.AppendHeader("content-disposition", "attachment; filename=ODI.pdf");
                //Response.ContentType = "application/pdf";
                //Response.WriteFile(DownFile);

                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(DownFile, false, 0);
                string strScipt1 = "";
                if (DownFile.Contains("/File.pdf"))
                {
                    strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + DownFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                }
                else
                {
                    strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + DownFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);

            }
            else if (ddlPrintType.SelectedValue == "W")
            {
                string FilePath = Server.MapPath(".") + "\\PDF Files\\";
                SaveDocument(strHTML, intOverdueInterestID, FilePath, FileName);
                string DownFile = FilePath + FileName + ".doc";
                if (!File.Exists(DownFile))
                {
                    Utility.FunShowAlertMsg(this, "File not exists");
                    return;
                }
                Response.AppendHeader("content-disposition", "attachment; filename=ODI.doc");
                Response.ContentType = "application/vnd.ms-word";
                Response.WriteFile(DownFile);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void SaveDocument(string strHTML, string ReferenceNumber, string FilePath, string FileName)
    {
        try
        {
            string strQRCode = string.Empty;
            int nDigi_Flag = 0;
            DataSet dsPrintDetails = new DataSet();

            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@ODI_Calculation_ID", Convert.ToString(intOverdueInterestID));
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            dsPrintDetails = Utility.GetDataset("S3G_LoanAd_ODI_Print", ObjDictionary);

            //DataRow[] ObjIGSTDR = dsPrintDetails.Tables[1].Select("InvTbl_IGST_Amount_Dbl > 0");

            //if (ObjIGSTDR.Length > 0)
            //{
            //    if (strHTML.Contains("~InvoiceTable~"))
            //        strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

            //    if (strHTML.Contains("~InvoiceTable1~"))
            //    {
            //        strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dsPrintDetails.Tables[1]/*Invoice Breakup*/);
            //    }
            //}
            //else
            //{
            //    if (strHTML.Contains("~InvoiceTable1~"))
            //        strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

            //    if (strHTML.Contains("~InvoiceTable~"))
            //    {
            //        strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dsPrintDetails.Tables[1]/*Invoice Breakup*/);
            //    }
            //}

            if (dsPrintDetails.Tables[0].Rows[0]["Digi_Sign_Enable"].ToString() == "1")
                nDigi_Flag = 1;

            if (strHTML.Contains("~SACTable~"))
            {
                strHTML = PDFPageSetup.FunPubBindTable("~SACTable~", strHTML, dsPrintDetails.Tables[1]/*SAC*/);
            }

            strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsPrintDetails.Tables[0]/*HDR*/);

            //strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsPrintDetails.Tables[1]/*HDR*/);
            FunPubGetQrCode(Convert.ToString(dsPrintDetails.Tables[0].Rows[0]["QRCode"]));

            strQRCode = Server.MapPath(".") + "\\PDF Files\\DCQRCode.png";

            List<string> listImageName = new List<string>();
            listImageName.Add("~CompanyLogo~");
            listImageName.Add("~InvoiceSignStamp~");
            listImageName.Add("~POSignStamp~");
            listImageName.Add("~QRCode~");
            List<string> listImagePath = new List<string>();

            if (Utility.StringToDate(dsPrintDetails.Tables[0].Rows[0]["Invoice_Date"].ToString()) >= Utility.StringToDate("06/06/2023"))
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
            }
            else
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
            }

            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));
            listImagePath.Add(strQRCode);
            strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);
            //if (strHTML.Contains("~QRCode~"))
            //    strHTML = strHTML.Replace("~QRCode~", "<img src='" + strQRCode + "' alt='Image'>");
            string text = "";
            if (Utility.StringToDate(dsPrintDetails.Tables[0].Rows[0]["Invoice_Date"].ToString()) >= Utility.StringToDate("18/07/2023"))
            {
                text = "\nRegd. Office: Ground Floor, Block No B, 809, EGA Trade Centre, Poonamallee High Road, Kilpauk, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            }
            else
            {
                text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069. ";
            }

            string SignedFile = "Signed" + intUserID.ToString();
            string DownFile = FilePath + FileName + ".pdf";
            
            //FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue, text);

            if (nDigi_Flag == 1 && ddlPrintType.SelectedValue == "P")
            {
                FunPubSaveDocument(strHTML, FilePath, SignedFile, ddlPrintType.SelectedValue, text);
                S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                ObjPDFSign.DigiPDFSign(FilePath + SignedFile + ".pdf", DownFile, "RIGHT");
            }
            else
                FunPubSaveDocument(strHTML, FilePath, FileName, ddlPrintType.SelectedValue, text);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, string FooterNote)
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

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            if (Utility.StringToDate(obj_Page.txtCurrentODIDate.Text) < Utility.StringToDate("30-Jun-2020"))
            {
                string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oDoc.ActiveWindow.Selection.Font.Size = 7;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(textDisc);
            }

            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" / ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            if (FooterNote != "")
            {
                oDoc.ActiveWindow.Selection.Font.Size = 9;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(FooterNote);
            }
            else
            {
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
            }

            string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            if (DocumentType == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
            }

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

    private void FunSetHeight()
    {
        int i = grvODI.Rows.Count;
        switch (i)
        {
            case 1:
                Panel1.Height = 62;
                break;
            case 2:
                Panel1.Height = 85;
                break;
            case 3:
                Panel1.Height = 110;
                break;
            case 4:
                Panel1.Height = 132;
                break;
            case 5:
                Panel1.Height = 155;
                break;
            case 6:
                Panel1.Height = 178;
                break;
            default:
                Panel1.Height = 200;
                break;
        }
    }

    private void FunPriGetOverDueInterest()
    {
        try
        {

            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@ODI_Calculation_ID", intOverdueInterestID.ToString());
            ObjDictionary.Add("@User_ID", intUserID.ToString());

            DataSet dsODI = Utility.GetDataset("S3G_LoanAd_GetOverDueInterest", ObjDictionary);
            if (dsODI.Tables[0] != null && dsODI.Tables[0].Rows.Count > 0)
            {

                ddlLOB.Items.Add(new ListItem(dsODI.Tables[0].Rows[0]["LOB_Name"].ToString(), dsODI.Tables[0].Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = dsODI.Tables[0].Rows[0]["LOB_Name"].ToString();
                txtCurrentODIDate.Text = DateTime.Parse(dsODI.Tables[0].Rows[0]["ODI_Calculation_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);// ;
                //Added For ODI Rate//
                if (dsODI.Tables[5].Rows.Count > 0)
                {
                    txtODIRate.Text = dsODI.Tables[5].Rows[0]["ODI_Intrest_Rate"].ToString();
                }
                //Added For ODI Rate//
                if (dsODI.Tables[0].Rows[0]["ODI_Calculation_Level"].ToString().ToUpper() == "S")
                {
                    rblAllSpecific.SelectedValue = "Specific";
                    if (dsODI.Tables[1] != null && dsODI.Tables[1].Rows.Count > 0)
                    {
                        txtScheduleDate.Text = "";
                        ddlBranch.SelectedText = dsODI.Tables[1].Rows[0]["Location_Name"].ToString();
                        ddlBranch.SelectedValue = dsODI.Tables[1].Rows[0]["Location_ID"].ToString();
                        ddlBranch.ToolTip = dsODI.Tables[1].Rows[0]["Location_Name"].ToString();
                        cmbCustomerCode.Text = dsODI.Tables[1].Rows[0]["Customer_Code"].ToString();
                        TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
                        txtName.Text = dsODI.Tables[1].Rows[0]["Customer_Code"].ToString();
                        hidCustId.Value = dsODI.Tables[1].Rows[0]["Customer_ID"].ToString();
                        string[] arrCustomerCode = dsODI.Tables[1].Rows[0]["Customer_Code"].ToString().Split('-');
                        if (arrCustomerCode.Length != 0)
                        {
                            txtCustomerName.Text = arrCustomerCode[arrCustomerCode.Length - 1].Trim();
                            ddlCustomer.SelectedValue = hidCustId.Value;
                            ddlCustomer.SelectedText = arrCustomerCode[arrCustomerCode.Length - 1].Trim();
                        }
                        if (dsODI.Tables[0].Rows[0]["Tranch_Header_ID"].ToString() != "0")
                        {
                            ddlTrancheName.Items.Add(new ListItem(dsODI.Tables[0].Rows[0]["Tranche_Name"].ToString(), dsODI.Tables[0].Rows[0]["Tranch_Header_ID"].ToString()));
                            ddlTrancheName.ToolTip = dsODI.Tables[0].Rows[0]["Tranche_Name"].ToString();

                            if (dsODI.Tables[1].Rows.Count == 1)
                            {
                                ddlPrimeAccountNo.Items.Add(new ListItem(dsODI.Tables[1].Rows[0]["PANum"].ToString(), dsODI.Tables[1].Rows[0]["PA_SA_Ref_ID"].ToString()));
                                ddlPrimeAccountNo.ToolTip = dsODI.Tables[1].Rows[0]["PANum"].ToString();
                            }
                            else
                            {
                                ddlPrimeAccountNo.Items.Add(new ListItem("All", "0"));
                                ddlPrimeAccountNo.ToolTip = "All";
                            }

                        }
                        else
                        {
                            ddlPrimeAccountNo.Items.Add(new ListItem(dsODI.Tables[1].Rows[0]["PANum"].ToString(), dsODI.Tables[1].Rows[0]["PA_SA_Ref_ID"].ToString()));
                            ddlPrimeAccountNo.ToolTip = dsODI.Tables[1].Rows[0]["PANum"].ToString();
                        }

                        FunBindLastODIDate();
                        if (dsODI.Tables[1].Rows[0]["Last_Calculated_Date"].ToString() != "") txtLastODICalculationDate.Text = DateTime.Parse(dsODI.Tables[1].Rows[0]["Last_Calculated_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);// ; 
                    }
                }
                if (dsODI.Tables.Count > 2 && dsODI.Tables[2] != null)
                {
                    panSchedule.Visible = true;
                    grvODI.DataSource = dsODI.Tables[2];
                    grvODI.DataBind();
                    // FunSetHeight();
                }
                if (dsODI.Tables.Count > 3 && dsODI.Tables[3] != null && dsODI.Tables[3].Rows.Count > 0 && rblAllSpecific.SelectedValue.ToUpper() == "ALL")
                {
                    txtScheduleDate.Text = DateTime.Parse(dsODI.Tables[3].Rows[0]["Schedule_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                    txtScheduleTime.Text = Convert.ToDateTime(dsODI.Tables[3].Rows[0]["Schedule_Date"].ToString()).ToShortTimeString();
                }
                FunPriCtrlDisable();
                if (dsODI.Tables.Count > 4 && dsODI.Tables[4] != null && dsODI.Tables[4].Rows.Count > 0)
                {
                    btnRevoke.Visible = true;
                    grvODI.Columns[5].Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to View the Over Due Interest details");
        }
    }


    private void FunPriGenerateODI()
    {
        try
        {

            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@ODI_Calculation_ID", intOverdueInterestID.ToString());
            ObjDictionary.Add("@User_ID", intUserID.ToString());

            DataSet dsODI = Utility.GetDataset("S3G_LoanAd_GenerateODI", ObjDictionary);
            if (dsODI.Tables[0].Rows[0]["print_status"].ToString() == "1")
            {
                Utility.FunShowAlertMsg(this.Page, "Print generated in Document Path");
                return;

            }
            
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to View the Over Due Interest details");
        }
    }

    private void FunPriCtrlDisable()
    {
        try
        {
            int intRevokeCnt = 0, intODICnt = 0;
            foreach (GridViewRow gvRow in grvODI.Rows)
            {
                CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
                CheckBox ChkRevoke = (CheckBox)gvRow.FindControl("ChkRevoke");
                if (!ChkODI.Enabled) intODICnt++;
                if (!ChkRevoke.Enabled) intRevokeCnt++;
            }
            if (intODICnt == grvODI.Rows.Count)
            {
                if (rblAllSpecific.SelectedValue.ToUpper() == "ALL") btnSave.Enabled = false;
                CheckBox ChkHeadODI = (CheckBox)grvODI.HeaderRow.FindControl("ChkHeadODI");
                if (ChkHeadODI != null)
                {
                    ChkHeadODI.Enabled = false;
                    ChkHeadODI.Checked = true;
                }
            }
            else
            {
                btnSave.Enabled = true;
            }

            if (intRevokeCnt == grvODI.Rows.Count)
            {
                btnRevoke.Enabled = false;
                CheckBox ChkHeadRevoke = (CheckBox)grvODI.HeaderRow.FindControl("ChkHeadRevoke");
                if (ChkHeadRevoke != null)
                {
                    ChkHeadRevoke.Enabled = false;
                    ChkHeadRevoke.Checked = true;
                }
            }
            else
                btnRevoke.Enabled = true;

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriClearPage()
    {
        try
        {
            ddlBranch.Clear();
            if (ddlPrimeAccountNo.Items.Count > 0) ddlPrimeAccountNo.Items.Clear();
            if (ddlTrancheName.Items.Count > 0) ddlTrancheName.Items.Clear();
            rblAllSpecific.SelectedValue = "All";
            txtODIRate.Text = txtCustomerName.Text = txtScheduleTime.Text = hidCustId.Value = cmbCustomerCode.Text = txtLastODICalculationDate.Text = "";
            txtScheduleDate.Text = txtCurrentODIDate.Text = strDate;
            ddlPrimeAccountNo.Enabled = ddlTrancheName.Enabled = false;
            txtScheduleDate.ReadOnly = txtScheduleTime.ReadOnly = ddlBranch.Enabled = false;
            RFVScheduleTime.Enabled = RFVScheduleDate.Enabled = REVScheduleTime.Enabled = false;
            cmbCustomerCode.ReadOnly = CECScheduleDate.Enabled = rbtnSchedule.Enabled = btnSave.Enabled = true;
            rbtnSchedule.SelectedValue = "0";
            RFVPrimeAccountNo.Enabled = rfvCALPrimeAC.Enabled = false;
            panSchedule.Visible = false;
            grvODI.DataSource = null;
            grvODI.DataBind();
            TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
            txtName.Text = "";
            Button btnGetLOV = ucCustomerCode.FindControl("btnGetLOV") as Button;
            if (rblAllSpecific.SelectedValue.ToUpper() == "ALL") btnGetLOV.Enabled = false;
            else btnGetLOV.Enabled = true;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw new ApplicationException("Unable to Clear the Over Due Interest");
        }
    }

    private void FunPriSaveOverDueInterest()
    {
        string strBranchId = string.Empty;
        string strcustomerid = string.Empty;
        string strTrancheid = string.Empty;


        foreach (GridViewRow gvRow in grvODI.Rows)
        {
            CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
            Label txtBranchId = (Label)gvRow.FindControl("txtBranchId");
            Label txtCustomerId = (Label)gvRow.FindControl("txtCustomerId");
            Label txtTrancheId = (Label)gvRow.FindControl("txtTrancheId");
            if (ChkODI.Checked && ChkODI.Enabled)
            {
                if (strBranchId == string.Empty)
                    strBranchId = txtBranchId.Text;
                else
                    strBranchId = strBranchId + "," + txtBranchId.Text;
            }

            if (ChkODI.Checked && ChkODI.Enabled)
            {
                if (strcustomerid == string.Empty)
                    strcustomerid = txtCustomerId.Text;
                else
                    strcustomerid = strBranchId + "," + txtCustomerId.Text;
            }

            if (ChkODI.Checked && ChkODI.Enabled)
            {
                if (strTrancheid == string.Empty)
                    strTrancheid = txtTrancheId.Text;
                else
                    strTrancheid = txtTrancheId + "," + txtTrancheId.Text;
            }
        }

        ObjLoanAdminAccMgtServicesClient = new LoanAdminAccMgtServicesClient();

        string strErrorLog = string.Empty;
        try
        {
            dtdata = (DataTable)ViewState["dtdata"];
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@User_ID", intUserID.ToString());
            ObjDictionary.Add("@Lob_Id", ddlLOB.SelectedValue);
            ObjDictionary.Add("@ODIDATE", Utility.StringToDate(txtCurrentODIDate.Text).ToString());

            if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC")
            {
                ObjDictionary.Add("@Location_Id", strBranchId);
                ObjDictionary.Add("@Acc_Xml", dtdata.FunPubFormXml(true));
                ObjDictionary.Add("@Schedule_Date", DateTime.Now.ToShortDateString());
                ObjDictionary.Add("@Schedule_Time", DateTime.Now.AddMinutes(5.0).ToShortTimeString());
                //if (ddlPrimeAccountNo.SelectedIndex != 0 && ddlPrimeAccountNo.Items.Count > 0)
                //    ObjDictionary.Add("@PA_SA_Ref_ID", ddlPrimeAccountNo.SelectedValue);
                //if (hidCustId.Value != "") ObjDictionary.Add("@Customer_ID", hidCustId.Value);
                //if (ddlTrancheName.SelectedIndex != 0 && ddlTrancheName.Items.Count > 0)
                //    ObjDictionary.Add("@Tranch_Header_ID", ddlTrancheName.SelectedValue);
               // ObjDictionary.Add("@Type", rblAllSpecific.SelectedValue);
                //Source modified by Tamilselvan.S on 27/01/2011
                int intResult = ObjLoanAdminAccMgtServicesClient.FunPubSaveODISchedule(ObjSerMode, ClsPubSerialize.Serialize(ObjDictionary, ObjSerMode));
                if (intResult == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.S3G_ErrMsg_ODIMonthLockLOB); //"Month was locked for the selected LOB.Unable to Save Over Due Interest");
                    return;
                }
                else if (intResult == 6)
                {
                    Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs.S3G_ErrMsg_ODIPrevMonthLockLOB);
                    return;
                }
                else if (intResult == 10)
                {
                    Utility.FunShowAlertMsg(this.Page, "ODI - Memo type was not added in the Memo Master");
                    return;
                }
                else if (intResult == 0)
                {
                    strAlert = Resources.ValidationMsgs.S3G_SucMsg_SaveODI; //"Over Due Interest added successfully  ";
                    strAlert += Resources.ValidationMsgs.S3G_ValMsg_Next; //@"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END

                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            else
            {
                ObjDictionary.Add("@Acc_Xml", dtdata.FunPubFormXml(true));
                ObjDictionary.Add("@Location_Id", strBranchId);
                if (rbtnSchedule.SelectedValue == "0")
                {
                    ObjDictionary.Add("@Schedule_Date", Utility.StringToDate(txtScheduleDate.Text).ToShortDateString());
                    ObjDictionary.Add("@Schedule_Time", txtScheduleTime.Text);
                }
                else
                {
                    ObjDictionary.Add("@Schedule_Date", DateTime.Now.ToShortDateString());
                    ObjDictionary.Add("@Schedule_Time", DateTime.Now.AddMinutes(5.0).ToShortTimeString());
                }
                int intResult = ObjLoanAdminAccMgtServicesClient.FunPubSaveODISchedule(ObjSerMode, ClsPubSerialize.Serialize(ObjDictionary, ObjSerMode));
                if (intResult == 10)
                {
                    Utility.FunShowAlertMsg(this.Page, "ODI - Memo type was not added in the Memo Master");
                    return;
                }
                if (intResult == 5)
                {
                    Utility.FunShowAlertMsg(this.Page, "Month is locked");
                    return;
                }
                else if (intResult == 6)
                {
                    Utility.FunShowAlertMsg(this.Page, "Previous month is not locked ");
                    return;
                }
                else if (intResult == 11)
                {
                    Utility.FunShowAlertMsg(this.Page, "Over Due Interest should be calculated last day of the month");
                    return;
                }
                else if (intResult == 0)
                {
                    strAlert = Resources.ValidationMsgs.S3G_SucMsg_SaveODI;//"Over Due Interest added successfully  ";
                    strAlert += Resources.ValidationMsgs.S3G_ValMsg_Next; //@"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {
            ObjDictionary = null;
            ObjLoanAdminAccMgtServicesClient.Close();
        }
    }

    #region "User Authorization"
    ////This is used to implement User Authorization
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                break;

            case 3:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnSave.Enabled = btnClear.Enabled = btnCalculateODI.Visible = false;
                ddlLOB.Enabled = ddlBranch.Enabled = imgCurrentODIDate.Visible = imgScheduleDate.Visible = false;
                txtCurrentODIDate.Width = 200;
                txtCurrentODIDate.Enabled = txtScheduleTime.Enabled = txtScheduleDate.Enabled = false;
                rblAllSpecific.Enabled = rbtnSchedule.Enabled = CECScheduleDate.Enabled = CECCurODICalcDate.Enabled = false;
                Button btnGetLOV = ucCustomerCode.FindControl("btnGetLOV") as Button;
                btnGetLOV.Enabled = false;

                if ((grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox) != null)
                    (grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox).Enabled = false;

                foreach (GridViewRow gvRow in grvODI.Rows)
                {
                    CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
                    ChkODI.Enabled = false;
                }
                grvODI.Columns[3].Visible = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }
                if (!bDelete)
                    btnRevoke.Enabled = false;
                break;

            case 2:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnRevoke.Enabled = btnSave.Enabled = btnClear.Enabled = btnCalculateODI.Visible = false;
                ddlLOB.Enabled = ddlBranch.Enabled = imgCurrentODIDate.Visible = imgScheduleDate.Visible = false;
                txtCurrentODIDate.Width = 200;
                txtCurrentODIDate.Enabled = txtScheduleTime.Enabled = txtScheduleDate.Enabled = false;
                rblAllSpecific.Enabled = rbtnSchedule.Enabled = CECScheduleDate.Enabled = CECCurODICalcDate.Enabled = false;

                if ((grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox) != null)
                    (grvODI.HeaderRow.FindControl("ChkHeadODI") as CheckBox).Enabled = false;
                if ((grvODI.HeaderRow.FindControl("ChkHeadRevoke") as CheckBox) != null)
                    (grvODI.HeaderRow.FindControl("ChkHeadRevoke") as CheckBox).Enabled = false;

                foreach (GridViewRow gvRow in grvODI.Rows)
                {
                    CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
                    CheckBox ChkRevoke = (CheckBox)gvRow.FindControl("ChkRevoke");
                    ChkODI.Enabled = ChkRevoke.Enabled = false;
                }
                grvODI.Columns[3].Visible = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }
                break;
        }
    }
    ////Code end
    #endregion

    private bool FunPriIsValid()
    {
        bool blnIsValid = true;
        cvOverDueInterest.ErrorMessage = "";
        TextBox txtName = ucCustomerCode.FindControl("txtName") as TextBox;
        if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC" && txtName.Text.Trim() == "")
        {
            cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select the Customer Code";
            cvOverDueInterest.IsValid = false;
            blnIsValid = false;
            return false;
        }
        if (rblAllSpecific.SelectedValue.ToUpper() == "SPECIFIC" && ddlPrimeAccountNo.Items.Count == 0)
        {
            cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + " Select the Prime Account No";
            cvOverDueInterest.IsValid = false;
            blnIsValid = false;
            return false;
        }
        if (txtCurrentODIDate.Text != "")
        {
            if (Utility.CompareDates(txtCurrentODIDate.Text, strDate) == -1)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalDate_NGT_CD;// " Current ODI Calculation Date should not be greater than Current Date  ";
                cvOverDueInterest.IsValid = false;
                blnIsValid = false;
                return false;
            }
            if (txtLastODICalculationDate.Text != "")
            {
                int intDateDiff = Utility.CompareDates(txtCurrentODIDate.Text, txtLastODICalculationDate.Text);
                if (intDateDiff == 0 || intDateDiff == 1)
                {
                    cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalDate_GT_LastODI; // " Current ODI Calculation Date should be greater than Last ODI Calculation Date  ";
                    cvOverDueInterest.IsValid = false;
                    blnIsValid = false;
                    return false;
                }
            }
        }

        if (rblAllSpecific.SelectedValue.ToUpper() == "ALL" && rbtnSchedule.SelectedValue == "0")
        {
            if (Utility.CompareDates(txtScheduleDate.Text, strDate) == 1)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ScheduleDate_GREQl;// " Schedule Date should be greater than or equal to Current Date  ";
                cvOverDueInterest.IsValid = false;
                blnIsValid = false;
                return false;
            }
            if (txtScheduleTime.Text != "")
            {
                DateTime st = DateTime.Parse((Utility.StringToDate(txtScheduleDate.Text).ToString()));
                DateTime et = DateTime.Now;
                TimeSpan span = new TimeSpan();
                span = et.Subtract(st);
                if ((span.Days) == 0)
                {
                    DateTime tm = DateTime.Parse(txtScheduleTime.Text);
                    span = tm.Subtract(et);
                    if (span.Hours == 0 && span.Minutes <= 5)
                    {
                        cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ScheduleTime_GR_5M;// " Schduled Time should be greater than 5 minutes for current date only ";
                        cvOverDueInterest.IsValid = false;
                        blnIsValid = false;
                        return false;
                    }
                }
            }
        }

        if (grvODI.Rows.Count == 0)
        {
            cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_ODICalNotExist; //" Records not exists for Over Due Calculation ";
            cvOverDueInterest.IsValid = false;
            blnIsValid = false;
            return false;
        }
        else
        {
            bool cntChk = false;
            foreach (GridViewRow gvRow in grvODI.Rows)
            {
                CheckBox ChkODI = (CheckBox)gvRow.FindControl("ChkODI");
                if (ChkODI.Enabled && ChkODI.Checked) cntChk = true;
            }
            if (cntChk == false)
            {
                cvOverDueInterest.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ErrMsg_Atleast_1ODI;// " Please select atleast one branch in the grid for Over Due Calculation ";
                cvOverDueInterest.IsValid = false;
                blnIsValid = false;
                return false;
            }
        }
        return blnIsValid;
    }

    #region GetCustomerCode
    /// <summary>
    /// GetCompletionList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetCustomerList(String prefixText, int count)
    {
        List<String> suggetions = null;
        DataTable dtCustomer = new DataTable();
        dtCustomer = (DataTable)System.Web.HttpContext.Current.Session["CustomerDT"];
        suggetions = GetSuggestions(prefixText, count, dtCustomer);
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
            string filterExpression = "Customer_Code like '" + key + "%'";
            DataRow[] dtSuggestions = dt1.Select(filterExpression);
            foreach (DataRow dr in dtSuggestions)
            {
                string suggestion = dr["Customer_Code"].ToString();
                suggestions.Add(suggestion);
            }
        }
        catch (Exception objException)
        {
            return suggestions;
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            //   lblErrorMessage.Text = Resources.LocalizationResources.CustomerTypeChangeError;
        }
        return suggestions;
    }
    #endregion

    #endregion

    #region [Set Error message]
    /// <summary>
    /// Created by Tamilselvan.S 
    /// Created date 27/01/2011
    /// </summary>
    public void FunPubSetErrorMessage()
    {
        RFVLOB.ErrorMessage = rfvCALLOB.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LOB;
        RFVPrimeAccountNo.ErrorMessage = rfvCALPrimeAC.ErrorMessage = "Rental Schedule No.";
        RFVCurrentODIDate.ErrorMessage = rfvCALCurrentODIcalculationDate.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_ODICalculationDate;
        RFVScheduleDate.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_ODIScheduleDate;
        RFVScheduleTime.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_ODIScheduleTime;
        REVScheduleTime.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_ODIValidTime;
    }

    #endregion [Set Error message]

    // Added By Shibu 24-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "072");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomer(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3g_LoanAd_GetCustomer_AGT", Procparam));

        return suggestions.ToArray();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string strFieldAtt = ((Button)sender).ClientID;
        int gRowIndex = Utility.FunPubGetGridRowID("grvODI", strFieldAtt);
        Label txtBranchId = grvODI.Rows[gRowIndex].FindControl("txtBranchId") as Label;
        Label txtCustomerId = grvODI.Rows[gRowIndex].FindControl("txtCustomerId") as Label;
        Label txtTrancheId = grvODI.Rows[gRowIndex].FindControl("txtTrancheId") as Label;

        if (ObjDictionary != null)
            ObjDictionary.Clear();
        else
            ObjDictionary = new Dictionary<string, string>();

        ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
        ObjDictionary.Add("@User_ID", intUserID.ToString());
        ObjDictionary.Add("@Lob_Id", ddlLOB.SelectedValue);
        ObjDictionary.Add("@Location_Id", txtBranchId.Text);
        ObjDictionary.Add("@Customer_ID", txtCustomerId.Text);
        ObjDictionary.Add("@tranche_header_id", txtTrancheId.Text);
        ObjDictionary.Add("@ODIDATE", Utility.StringToDate(txtCurrentODIDate.Text).ToString());
        DataSet dsCalCulateODI = Utility.GetDataset("S3G_LOANAD_ODI_ACCOUNT", ObjDictionary);
        ExportToExcel(dsCalCulateODI.Tables[0], txtCustomerId.Text);
        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);

    }


    public void ExportToExcel(DataTable dtExportData, string CustomerId)
    {
        try
        {
            string filename = "ODI_" + CustomerId + ".xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = dtExportData;
            dgGrid.DataBind();
            dgGrid.RenderControl(hw);
            //Write the HTML back to the browser.            
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            this.EnableViewState = false;
            Response.Write(tw.ToString());
            Response.End();
        }
        catch (Exception objException)
        {
            throw objException;
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void BtnGeneratePost_Click(object sender, EventArgs e)
    {
        FunPriGenerateODI();
    }

    private void FunPubGetQrCode(string ReferenceNumber)
    {
        try
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ReferenceNumber, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);

            qrCodeImage.Save(Server.MapPath(".") + "\\PDF Files\\DCQRCode.png");
        }
        catch (Exception ex)
        {

        }
    }
}