#region Page Header

/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Factoring Invoice Loading
/// Created By			: Thangam M. Irsathameen K
/// Created Date		: 17-Aug-2010
/// Purpose	            : To fetch Factoring Invoice Loading Details
/// Modified By         : --
/// Modified Date       : --
///  
///

#endregion

#region Namespaces

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
using System.ServiceModel;
using Resources;

#endregion

public partial class LoanAdmin_S3GLoanAdFactoringInvoiceLoading : ApplyThemeForProject
{
    #region Declaration

    LoanAdminMgtServicesReference.LoanAdminMgtServicesClient ObjFactoringInvoiceClient;
    LoanAdminMgtServices.S3G_LOANAD_FactoringInvoiceDetailsDataTable ObjS3G_LOANAD_FactoringInvoiceDetailsDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;

    int intFILID = 0;
    int intErrCode = 0;
    int intUserID = 0;
    int intCompanyID = 0;

    int i;
    bool status;
    string s, strFILNo, strInvoiceNo, strPartyName;
    static string strPageName = "Factoring Invoice Loading";

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    DataTable dtApprovals = new DataTable();
    Dictionary<string, string> dictParam = null;

    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=FIL";
    string strKey = "Insert";
    string strXMLFILDet = string.Empty;
    decimal totalScore = 0;
    decimal totalInvoice = 0;
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdFactoringInvoiceLoading.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=FIL';";
    S3GSession ObjS3GSession;

    public static LoanAdmin_S3GLoanAdFactoringInvoiceLoading obj_Page;
    #endregion

    #region Page Loading

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CECFILDATE.Format = strDateFormat;
            ddlLOB.AddItemToolTip();
            //ddlBranch.AddItemToolTip();
            //ddlPAN.AddItemToolTip();
            ddlSAN.AddItemToolTip();
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                { intFILID = Convert.ToInt32(fromTicket.Name); }
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
            //Code end            
            lblErrorMessage.Text = "";
           
            if (!IsPostBack)
            {
                bool CanModify = true;
                btnGo.Enabled = false;
              

             

                if (PageMode == PageModes.Create)
                {
                    FunProBindBranchPANSAN();
                    txtInvoiceLoadValue.SetDecimalPrefixSuffix(10, 2, true, false, "Invoice Load Value");
                }
               
                //User Authorization            
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if ((intFILID > 0) && (strMode == "M"))// Modify
                {
                    CanModify = FunProGetFactoringInvoiceDetails();
                    FunProDisableControls(1);
                    //if (CanModify)
                    //{
                    //    FunProDisableControls(1);
                    //}
                    //else
                    //{
                    //    ViewState["CanModify"] = "1";
                    //    Utility.FunShowAlertMsg(this.Page, "Cannot modify. Payment Request has been made for this Invoice.");
                    //    FunProDisableControls(1);
                    //}
                }
                else if ((intFILID > 0) && (strMode == "Q")) // Query 
                {
                    CanModify = FunProGetFactoringInvoiceDetails();
                    FunProDisableControls(-1);
                }
                else  //Create Mode
                {
                    FunProDisableControls(0);
                }
                ddlBranch.Focus();
                if (ddlLOB.Items.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "The Factoring Line of Business is not Activated.", strRedirectPage);
                }
            }
            if (GRVFIL.FooterRow != null)
            {
                AjaxControlToolkit.CalendarExtender CEXInvDt = (AjaxControlToolkit.CalendarExtender)GRVFIL.FooterRow.FindControl("CalendarExtender1F");
                if (CEXInvDt != null)
                {
                    CEXInvDt.Format = strDateFormat;
                }
                TextBox txtInvoiceAmountF = (TextBox)GRVFIL.FooterRow.FindControl("txtInvoiceAmountF");
                txtInvoiceAmountF.SetDecimalPrefixSuffix(10, 2, true, false, "Invoice Amount");
                TextBox txtInvDateF = (TextBox)GRVFIL.FooterRow.FindControl("txtInvoiceDateF");
                if (txtInvDateF != null)
                {
                    txtInvDateF.Attributes.Add("readonly", "readonly");
                }
            }

            txtTotalEligibleAmt.Attributes.Add("readonly", "readonly");
            txtTotalInvoiceAmt.Attributes.Add("readonly", "readonly");
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region Page Events

    #region Save Clear Cancel Go

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjFactoringInvoiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();

        try
        {
            strFILNo = "";
            if (Convert.ToDateTime(ViewState["AccountDate"].ToString()) >
                Utility.StringToDate(txtFILDate.Text))
            {
                cvFactoring.ErrorMessage = "FIL date should be greater than or equal to Account creation date (" + Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat) + ")";
                cvFactoring.IsValid = false;
                return;
            }
            if (GRVFIL.Rows.Count == 0 || !GRVFIL.Rows[0].Visible)
            {
                cvFactoring.ErrorMessage = "Add atleast one Invoice Details";
                cvFactoring.IsValid = false;
                return;
            }
            TextBox txtInvoiceNoFzero = (TextBox)GRVFIL.Rows[0].FindControl("txtInvoiceNo");
            foreach (GridViewRow GvRow in GRVFIL.Rows)
            {
                TextBox txtInvoiceNoF = (TextBox)GvRow.FindControl("txtInvoiceNo");
                TextBox txtInvoiceDateF = (TextBox)GvRow.FindControl("txtInvoiceDate");
                TextBox txtPartyNameF = (TextBox)GvRow.FindControl("txtPartyName");
                TextBox txtMaturityDateF = (TextBox)GvRow.FindControl("txtMaturityDate");
                TextBox txtInvoiceAmountF = (TextBox)GvRow.FindControl("txtInvoiceAmount");
                TextBox txtEligibleAmountF = (TextBox)GvRow.FindControl("txtEligibleAmount");

                if (txtInvoiceNoF.Text.Trim() == string.Empty)
                {
                    cvFactoring.ErrorMessage = "Enter the Invoice No in SI No. " + (GvRow.RowIndex + 1).ToString();
                    cvFactoring.IsValid = false;
                    txtInvoiceNoF.Focus();
                    txtInvoiceNoF.BackColor = System.Drawing.Color.LightYellow;
                    return;
                }
                else if (txtInvoiceDateF.Text.Trim() == string.Empty)
                {
                    cvFactoring.ErrorMessage = "Enter the Invoice Date in SI No. " + (GvRow.RowIndex + 1).ToString();
                    cvFactoring.IsValid = false;
                    txtInvoiceDateF.Focus();
                    txtInvoiceDateF.BackColor = System.Drawing.Color.LightYellow;
                    return;
                }
                else if (txtPartyNameF.Text.Trim() == string.Empty)
                {
                    cvFactoring.ErrorMessage = "Enter the Party Name in SI No. " + (GvRow.RowIndex + 1).ToString();
                    cvFactoring.IsValid = false;
                    txtPartyNameF.Focus();
                    txtPartyNameF.BackColor = System.Drawing.Color.LightYellow;
                    return;
                }
                else if (txtInvoiceAmountF.Text.Trim() == string.Empty)
                {
                    cvFactoring.ErrorMessage = "Enter the Invoice Amount in SI No. " + (GvRow.RowIndex + 1).ToString();
                    cvFactoring.IsValid = false;
                    txtInvoiceAmountF.BackColor = System.Drawing.Color.LightYellow;
                    txtInvoiceAmountF.Focus();
                    return;
                }
                else if (txtCreditDays.Text.Trim() == string.Empty)
                {
                    cvFactoring.ErrorMessage = "Enter the Credit Days";
                    cvFactoring.IsValid = false;
                    txtCreditDays.Focus();
                    return;
                }
                else if (txtInvoiceLoadValue.Text.Trim() == string.Empty)
                {
                    cvFactoring.ErrorMessage = "Enter the Invoice Load Value in SI No. " + (GvRow.RowIndex + 1).ToString();
                    cvFactoring.IsValid = false;
                    txtInvoiceAmountF.Focus();
                    txtInvoiceAmountF.BackColor = System.Drawing.Color.LightYellow;
                    return;
                }
                DateTime Date = Utility.StringToDate(txtInvoiceDateF.Text).AddDays(Convert.ToInt32(txtCreditDays.Text));
                if (txtFILDate.Text == "")
                {
                    cvFactoring.ErrorMessage = "Select the FIL date";
                    cvFactoring.IsValid = false;
                    txtFILDate.Focus();
                    return;
                }

                //if (Convert.ToDateTime(Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat)) >
                //Convert.ToDateTime(txtInvoiceDateF.Text))
                //{
                //    cvFactoring.ErrorMessage = "Invoice Date should be greater than or equal to Account creation date (" + Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat) + ")" + " in Record Number " + (GvRow.RowIndex + 1).ToString();
                //    cvFactoring.IsValid = false;
                //    txtInvoiceDateF.Focus();
                //    txtInvoiceDateF.BackColor = System.Drawing.Color.LightYellow;
                //    return;
                //}

                //Based on dicussion with BA Manager Mr.K.Bashyam

                //if (Utility.StringToDate(txtInvoiceDateF.Text) > Utility.StringToDate(txtFILDate.Text))
                //{
                //    cvFactoring.ErrorMessage = "Invoice Date should be less than or equal to the FIL Date in SI No. " + (GvRow.RowIndex + 1).ToString();
                //    cvFactoring.IsValid = false;
                //    txtInvoiceDateF.Focus();
                //    txtInvoiceDateF.BackColor = System.Drawing.Color.LightYellow;
                //    return;
                //}

                //End here
                txtMaturityDateF.Text = Date.ToString(strDateFormat);
                if (Date <= Utility.StringToDate(txtFILDate.Text))
                {
                    cvFactoring.ErrorMessage = "Maturity Date should be greater than the FIL Date in SI No. " + (GvRow.RowIndex + 1).ToString();
                    cvFactoring.IsValid = false;
                    txtInvoiceDateF.Focus();
                    txtInvoiceDateF.BackColor = System.Drawing.Color.LightYellow;
                    return;
                }
                //Code commented by Ganapthy for fixing the bug 6448 -Start
                //string strEligibleAmount = Convert.ToString(Convert.ToDecimal(txtInvoiceAmountF.Text) - ((Convert.ToDecimal(txtInvoiceAmountF.Text) * Convert.ToDecimal(txtMargin.Text)) / 100));

                //txtEligibleAmountF.Text = strEligibleAmount;
                //totalScore += txtEligibleAmountF.Text != "" ? Convert.ToDecimal(txtEligibleAmountF.Text) : 0;

                //Code commented by Ganapthy for fixing the bug 6448 -end
            }

            for (int intNum = 0; intNum < GRVFIL.Rows.Count - 1; intNum++)
            {
                Label txtSNo = (Label)GRVFIL.Rows[intNum].FindControl("txtsno");
                TextBox txtInvoiceNo = (TextBox)GRVFIL.Rows[intNum].FindControl("txtInvoiceNo");
                TextBox txtPartyName = (TextBox)GRVFIL.Rows[intNum].FindControl("txtPartyName");
                for (int intNum1 = 0; intNum1 <= GRVFIL.Rows.Count - 1; intNum1++)
                {
                    Label txtSNo1 = (Label)GRVFIL.Rows[intNum1].FindControl("txtsno");
                    TextBox txtInvoiceNo1 = (TextBox)GRVFIL.Rows[intNum1].FindControl("txtInvoiceNo");
                    TextBox txtPartyName1 = (TextBox)GRVFIL.Rows[intNum1].FindControl("txtPartyName");
                    if (txtSNo1.Text != txtSNo.Text)
                    {
                        if (txtInvoiceNo.Text == txtInvoiceNo1.Text && txtPartyName.Text == txtPartyName1.Text)
                        {
                            Utility.FunShowAlertMsg(this, "Invoice No " + txtInvoiceNo1.Text + " for the party " + txtPartyName1.Text + " already exists");
                            txtInvoiceNo1.Focus();
                            return;
                        }
                    }
                }
            }

            if (Convert.ToDouble(txtTotalEligibleAmt.Text) == 0)
            {
                cvFactoring.ErrorMessage = "Total Eligible Amount cannot be Zero. Add valid Invoice Details";
                cvFactoring.IsValid = false;

                return;
            }
            //txtTotalEligibleAmt.Text = totalScore.ToString();
            if (Convert.ToDecimal(txtTotalInvoiceAmt.Text) < Convert.ToDecimal(txtInvoiceLoadValue.Text))
            {
                cvFactoring.ErrorMessage = "Total Invoice amount should be greater than or Equal to the Invoice Load value";
                cvFactoring.IsValid = false;
                return;
            }

            //if (PageMode == PageModes.Create)
            //{
            //    if (Convert.ToDecimal(txtOutStandingAmount.Text) + Convert.ToDecimal(txtCreditAvailable.Text) >= Convert.ToDecimal(txtCreditLimit.Text))
            //    {
            //        cvFactoring.ErrorMessage = "Credit Limit Exceeded. Reduce the Invoice value.";
            //        cvFactoring.IsValid = false;
            //        return;
            //    }
            //}

            ObjS3G_LOANAD_FactoringInvoiceDetailsDataTable = new LoanAdminMgtServices.S3G_LOANAD_FactoringInvoiceDetailsDataTable();
            LoanAdminMgtServices.S3G_LOANAD_FactoringInvoiceDetailsRow ObjFactoringInvoiceRow;
            ObjFactoringInvoiceRow = ObjS3G_LOANAD_FactoringInvoiceDetailsDataTable.NewS3G_LOANAD_FactoringInvoiceDetailsRow();
            ObjFactoringInvoiceRow.Company_ID = intCompanyID;
            ObjFactoringInvoiceRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjFactoringInvoiceRow.Customer_ID = Convert.ToInt32(hidcuscode.Value);//hidden Customer_ID;
            ObjFactoringInvoiceRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjFactoringInvoiceRow.PANum = Convert.ToString(ddlPAN.SelectedValue);
            ObjFactoringInvoiceRow.Bill_Type = Convert.ToInt32(rbtlBillType.SelectedValue);
            if (ddlSAN.Items.Count == 0 || ddlSAN.SelectedItem.Text == "--Select--")
            {
                if (ViewState["dum"] != null)
                {
                    ObjFactoringInvoiceRow.SANum = Convert.ToString(ViewState["dum"]);
                }
                else
                {
                    ObjFactoringInvoiceRow.SANum = "";
                }
            }
            else
            {
                ObjFactoringInvoiceRow.SANum = Convert.ToString(ddlSAN.SelectedValue);
            }
            ObjFactoringInvoiceRow.Fil_Date = Utility.StringToDate(txtFILDate.Text);
            ObjFactoringInvoiceRow.Invoice_Load_Value = Convert.ToDecimal(txtInvoiceLoadValue.Text);
            ObjFactoringInvoiceRow.Credit_Days = Convert.ToInt32(txtCreditDays.Text);
            ObjFactoringInvoiceRow.Created_By = intUserID;
            ObjFactoringInvoiceRow.FIL_ID = intFILID;
            FunProGenerateFactoringXMLDet();
            ObjFactoringInvoiceRow.XMLFILDetails = strXMLFILDet;
            ObjS3G_LOANAD_FactoringInvoiceDetailsDataTable.AddS3G_LOANAD_FactoringInvoiceDetailsRow(ObjFactoringInvoiceRow);
            //ObjFactoringInvoiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
            intErrCode = ObjFactoringInvoiceClient.FunPubCreateFactoringInvoiceDetails(out strFILNo, out strInvoiceNo, out strPartyName, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_FactoringInvoiceDetailsDataTable, SerMode));
            switch (intErrCode)
            {
                case 0:
                    {
                        if (intFILID > 0)
                        {
                            strAlert = strAlert.Replace("__ALERT__", "Factoring Invoice details updated successfully");
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        else
                        {
                            txtFILNo.Text = strFILNo;

                            strAlert = "Factoring Invoice details added successfully-" + strFILNo;
                            strAlert += @"\n\nWould you like to add one more Factoring Invoice?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END

                            lblErrorMessage.Text = string.Empty;
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }
                    break;
                case 2: Utility.FunShowAlertMsg(this.Page, "Document Number Not yet Defined");
                    break;
                case 3: Utility.FunShowAlertMsg(this.Page, "FIL Date month is closed. Factoring Invoice details cannot be created or updated");
                    break;
                case 4: Utility.FunShowAlertMsg(this.Page, "Current FIL Date should be greater than the Last FIL Date for the given combination");
                    break;
                case 5: Utility.FunShowAlertMsg(this.Page, "Document Number exceeded");
                    break;
                case 6: Utility.FunShowAlertMsg(this.Page, "Invoice Details exist for the given combination");
                    break;
                case 7: Utility.FunShowAlertMsg(this.Page, "Invoice No " + strInvoiceNo + " for the party " + strPartyName + " already exists");
                    break;
                default: break;
            }
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
        finally
        {
            //if (ObjFactoringInvoiceClient != null)
            //{
            ObjFactoringInvoiceClient.Close();
            //}
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunProClearControls();
            //if (ddlBranch.Items.Count > 0)
            //{ ddlBranch.SelectedIndex = 0; }
            ddlBranch.Clear();
            //if (ddlPAN.Items.Count > 0)
            //{ ddlPAN.Items.Clear(); }
            ddlPAN.Clear();
            if (ddlSAN.Items.Count > 0)
            {
                ddlSAN.Items.Clear();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Clear the Controls";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Redirect the Page";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {

            //string s=ViewState["AccountDate"].ToString();
            //string s1 = Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat);

            //if (Convert.ToDateTime(Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat)) >
            //    Convert.ToDateTime(txtFILDate.Text))

            if (!FunPriValidateFromEndDate(ViewState["AccountDate"].ToString(), txtFILDate.Text))
            {
                cvFactoring.ErrorMessage = "FIL date should be greater than or equal to Account creation date (" + Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat) + ")";
                cvFactoring.IsValid = false;
                return;
            }

            FunProSetInitialRow();
            lbltotal.Visible = true;
            GRVFIL.Visible = true;
            txtTotalEligibleAmt.Visible = true;
            pnlInvDtl.Visible = true;
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    /// <summary>
    /// This method will validate the from and to date - entered by the user.
    /// </summary>
    private bool FunPriValidateFromEndDate(string strFromDate, string strToDate)
    {
        DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
        dtformat.ShortDatePattern = "MM/dd/yy";

        //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
        if (Utility.StringToDate(strFromDate) > Utility.StringToDate(strToDate)) // start date should be less than or equal to the enddate
        {
            return false;
        }
        return true;
    }


    #endregion

    #region Gridview Events

    protected void GRVFIL_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                dtApprovals = (DataTable)ViewState["currenttable"];
                if (dtApprovals.Rows.Count > 0)
                {
                    if (dtApprovals.Rows[0]["Invoiceno"].ToString() == string.Empty)
                    {
                        dtApprovals.Rows[0].Delete();
                    }
                }

                Label txtsnoF = (Label)GRVFIL.FooterRow.FindControl("txtsnoF");
                TextBox txtInvoiceNoF = (TextBox)GRVFIL.FooterRow.FindControl("txtInvoiceNoF");
                TextBox txtInvoiceDateF = (TextBox)GRVFIL.FooterRow.FindControl("txtInvoiceDateF");
                TextBox txtPartyNameF = (TextBox)GRVFIL.FooterRow.FindControl("txtPartyNameF");
                TextBox txtMaturityDateF = (TextBox)GRVFIL.FooterRow.FindControl("txtMaturityDateF");
                TextBox txtInvoiceAmountF = (TextBox)GRVFIL.FooterRow.FindControl("txtInvoiceAmountF");
                TextBox txtEligibleAmountF = (TextBox)GRVFIL.FooterRow.FindControl("txtEligibleAmountF");

                DataRow[] dArray = dtApprovals.Select("INVOICENO='" + txtInvoiceNoF.Text + "' and PARTYNAME='" + txtPartyNameF.Text + "'");
                if (dArray.Length > 0)
                {
                    Utility.FunShowAlertMsg(this, "Invoice No " + txtInvoiceNoF.Text + " for the party " + txtPartyNameF.Text + " already exists");
                    txtInvoiceNoF.Focus();
                    return;
                }

                if (Convert.ToDecimal(txtTotalEligibleAmt.Text) == Convert.ToDecimal(txtCreditLimit.Text))
                {
                    cvFactoring.ErrorMessage = "Eligible amount reached the Credit Limit. Cannot add more invoices.";
                    cvFactoring.IsValid = false;
                    return;
                }
                if (txtInvoiceLoadValue.Text.Trim() == string.Empty)
                {
                    cvFactoring.ErrorMessage = "Enter the InvoiceLoadValue";
                    cvFactoring.IsValid = false;
                    txtInvoiceLoadValue.BackColor = System.Drawing.Color.LightYellow;
                    txtInvoiceLoadValue.Focus();
                    return;
                }
                else if (txtCreditDays.Text.Trim() == string.Empty)
                {
                    cvFactoring.ErrorMessage = "Enter the Credit Days";
                    cvFactoring.IsValid = false;
                    txtCreditDays.Focus();
                    return;
                }

                dr = dtApprovals.NewRow();
                dr["SNo"] = dtApprovals.Rows.Count + 1;
                dr["Invoiceno"] = txtInvoiceNoF.Text.Trim();
                dr["InvoiceDate"] = Utility.StringToDate(txtInvoiceDateF.Text.Trim());
                dr["PartyName"] = txtPartyNameF.Text.Trim();
                dr["Factoring_Inv_Load_Details_ID"] = "0";
                dr["Lock"] = "0";
                //DateTime Date = Utility.StringToDate(dr["InvoiceDate"].ToString()).AddDays(Convert.ToInt32(txtCreditDays.Text));

                DateTime Date = Utility.StringToDate(txtMaturityDateF.Text);

                if (txtFILDate.Text == "")
                {
                    cvFactoring.ErrorMessage = "Please select the FIL date";
                    cvFactoring.IsValid = false;
                    txtFILDate.Focus();
                    return;
                }
                //else if (Convert.ToDateTime(Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat)) >
                //Convert.ToDateTime(txtInvoiceDateF.Text))
                //{
                //    cvFactoring.ErrorMessage = "Invoice Date should be greater than or equal to Account creation date (" + Convert.ToDateTime(ViewState["AccountDate"].ToString()).ToString(strDateFormat) + ")";
                //    cvFactoring.IsValid = false;
                //    txtInvoiceDateF.Focus();
                //    txtInvoiceDateF.BackColor = System.Drawing.Color.LightYellow;
                //    return;
                //}

                    //Based on dicussion with BA Manager Mr.K.Bashyam

                //else if (Utility.StringToDate(txtInvoiceDateF.Text) > Utility.StringToDate(txtFILDate.Text))
                //{
                //    cvFactoring.ErrorMessage = "Invoice Date should be less than or equal to the FIL Date";
                //    cvFactoring.IsValid = false;
                //    txtInvoiceDateF.Focus();
                //    txtInvoiceDateF.BackColor = System.Drawing.Color.LightYellow;
                //    return;
                //}

                    //End here

                else if (Date <= Utility.StringToDate(txtFILDate.Text))
                {
                    cvFactoring.ErrorMessage = "Maturity Date should be greater than the FIL Date";
                    cvFactoring.IsValid = false;
                    txtInvoiceDateF.Focus();
                    txtInvoiceDateF.BackColor = System.Drawing.Color.LightYellow;
                    return;
                }
                dr["MaturityDate"] = Date.ToString();//(strDateFormat);            
                dr["InvoiceAmount"] = txtInvoiceAmountF.Text.Trim();

                int intSuffix = 0;
                if (ObjS3GSession.ProGpsSuffixRW > 2)
                {
                    intSuffix = 2;
                }
                else
                {
                    intSuffix = ObjS3GSession.ProGpsSuffixRW;
                }

                decimal dcmEligibleAmount = Math.Round(Convert.ToDecimal(dr["InvoiceAmount"]) - ((Convert.ToDecimal(dr["InvoiceAmount"]) * Convert.ToDecimal(txtMargin.Text)) / 100), intSuffix);

                if (dcmEligibleAmount + Convert.ToDecimal(txtTotalInvoiceAmt.Text) > Convert.ToDecimal(txtCreditLimit.Text))
                {
                    Utility.FunShowAlertMsg(this, "Invoice Amount exceeds the Credit Limit.");
                    dcmEligibleAmount = Convert.ToDecimal(txtCreditLimit.Text) - Convert.ToDecimal(txtTotalInvoiceAmt.Text);
                }

                dr["EligibleAmount"] = dcmEligibleAmount;

                dtApprovals.Rows.Add(dr);
                GRVFIL.DataSource = dtApprovals;
                GRVFIL.DataBind();
                ViewState["currenttable"] = dtApprovals;
            }
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void GRVFIL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtEligibleAmount = ((TextBox)e.Row.FindControl("txtEligibleAmount"));
                TextBox txtInvoiceAmount = ((TextBox)e.Row.FindControl("txtInvoiceAmount"));

                totalScore += txtEligibleAmount.Text != "" ? Convert.ToDecimal(txtEligibleAmount.Text) : 0;
                totalInvoice += txtInvoiceAmount.Text != "" ? Convert.ToDecimal(txtInvoiceAmount.Text) : 0;

                TextBox txtInvoiceDate = (TextBox)e.Row.FindControl("txtInvoiceDate");
                txtInvoiceDate.Attributes.Add("readonly", "readonly");
                if (txtInvoiceDate.Text.Trim() != string.Empty)
                {
                    DateTime Date = Convert.ToDateTime(txtInvoiceDate.Text);
                    txtInvoiceDate.Text = Date.ToString(strDateFormat);
                }
                TextBox txtMaturityDate = (TextBox)e.Row.FindControl("txtMaturityDate");
                txtMaturityDate.Attributes.Add("readonly", "readonly");
                if (txtMaturityDate.Text.Trim() != string.Empty)
                {
                    DateTime Date = Utility.StringToDate(txtMaturityDate.Text);
                    txtMaturityDate.Text = Date.ToString(strDateFormat);
                }

                AjaxControlToolkit.CalendarExtender CEXInvDt = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender1");

                if (CEXInvDt != null)
                {
                    CEXInvDt.Format = strDateFormat;
                }

                //txtInvoiceAmount.SetDecimalPrefixSuffix(10, 2, true,false,"Invoice Amount");
                txtInvoiceAmount.Attributes.Add("onchange", "SumScore('" + ObjS3GSession.ProGpsSuffixRW.ToString() + "');");

                Label lblLock = (Label)e.Row.FindControl("lblLock");
                if (lblLock.Text != "0" && PageMode == PageModes.Modify)
                {
                    e.Row.Enabled = false;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Date Format Changing
                AjaxControlToolkit.CalendarExtender ceFromDate = e.Row.FindControl("CalendarExtender1F") as AjaxControlToolkit.CalendarExtender;
                ceFromDate.Format = strDateFormat;

                TextBox txtInvDateF = (TextBox)e.Row.FindControl("txtInvoiceDateF");
                TextBox txtMaturityDateF = (TextBox)e.Row.FindControl("txtMaturityDateF");
                if (txtInvDateF != null)
                {
                    txtInvDateF.Attributes.Add("readonly", "readonly");
                }
                txtMaturityDateF.Attributes.Add("readonly", "readonly");
                TextBox txtInvoiceAmount = (TextBox)e.Row.FindControl("txtInvoiceAmountF");
                txtInvoiceAmount.SetDecimalPrefixSuffix(10, 2, true, false, "Invoice Amount");

                AjaxControlToolkit.CalendarExtender CaltMaturityDateF = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CaltMaturityDateF");
                CaltMaturityDateF.Format = strDateFormat;
            }
            txtTotalEligibleAmt.Text = totalScore.ToString();
            txtTotalInvoiceAmt.Text = totalInvoice.ToString();
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void GRVFIL_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["currenttable"];
            dtDelete.Rows.RemoveAt(e.RowIndex);

            decimal dcmTotalEligibleAmount = 0;
            if (dtDelete.Rows.Count != 0)
            {
                int i = dtDelete.Rows.Count - 1;
                int intSuffix = 0;

                if (dtDelete.Rows.Count > 1)
                {
                    dcmTotalEligibleAmount = Convert.ToDecimal(dtDelete.Compute("SUM(EligibleAmount)", "SNO <> '" + dtDelete.Rows[i]["SNO"].ToString() + "'"));
                }

                if (ObjS3GSession.ProGpsSuffixRW > 2)
                {
                    intSuffix = 2;
                }
                else
                {
                    intSuffix = ObjS3GSession.ProGpsSuffixRW;
                }

                decimal dcmEligibleAmount = Math.Round(Convert.ToDecimal(dtDelete.Rows[i]["InvoiceAmount"]) - ((Convert.ToDecimal(dtDelete.Rows[i]["InvoiceAmount"]) * Convert.ToDecimal(txtMargin.Text)) / 100), intSuffix);


                if (dcmEligibleAmount + dcmTotalEligibleAmount > Convert.ToDecimal(txtCreditLimit.Text))
                {
                    dcmEligibleAmount = Convert.ToDecimal(txtCreditLimit.Text) - dcmTotalEligibleAmount;
                }

                dtDelete.Rows[i]["EligibleAmount"] = Math.Round(dcmEligibleAmount, intSuffix).ToString();
            }

            GRVFIL.DataSource = dtDelete;
            GRVFIL.DataBind();
            ViewState["currenttable"] = dtDelete;
            if (dtDelete.Rows.Count == 0)
            { FunProSetInitialRow(); }
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region DDL Events

    protected void ddlPAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearControls(true);
            FunProGetCustomerDetails();
            FunProGetFactDetails();

            FunProLoadSubAccNo();

            string IsExist = "";
            //IsExist = FunProGetColumnValue(ddlPAN, "Exist", (DataTable)ViewState["PANumDt"]);
            if (IsExist != "" && IsExist == "1")
            {
                Utility.FunShowAlertMsg(this.Page, "FIL already exists for this Account");
                btnSave.Enabled = false;
                btnGo.Enabled = false;
            }
            else
            {
                if (bCreate)
                {
                    btnSave.Enabled = true;
                }

                btnGo.Enabled = true;
            }

            ddlPAN.Focus();
            ddlPAN.ToolTip = ddlPAN.SelectedText;
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearControls(true);
            ddlSAN.Items.Clear();
            FunProLoadPrimeAccNo();
            ddlBranch.Focus();
            ddlBranch.ToolTip = ddlBranch.SelectedText;
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ddlSAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunProClearControls(false);
            FunProGetFactDetails();
            FunProGetAccountDetails(true);
            ddlSAN.Focus();
            ddlSAN.ToolTip = ddlSAN.SelectedItem.Text;
        }
        catch (Exception objException)
        {
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #endregion

    #region User Defined Functions

    protected void FunProClearControls(bool CanClearCustomer)
    {
        try
        {
            txtStatus.Text =
            txtMargin.Text =
            txtCreditLimit.Text =
            txtCreditAvailable.Text =
            txtOutStandingAmount.Text =
            txtInvoiceLoadValue.Text = "";

            pnlInvDtl.Visible = false;

            if (CanClearCustomer)
            {
                S3GCustomerAddress1.ClearCustomerDetails();
            }

            btnGo.Enabled = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to Clear the value");
        }
    }

    protected void FunProBindBranchPANSAN()
    {
        try
        {
            // LOB
            FunPriLoadLob();




            if (ddlLOB.Items.Count > 0)
            {
                ddlLOB.Items.RemoveAt(0);
                //ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
            }

            //Branch
            FunPriLoadBranch();

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to LOB and Branch");
        }
    }

    private void FunPriLoadBranch()
    {
        //dictParam = new Dictionary<string, string>();
        //dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //if (PageMode == PageModes.Create)
        //{
        //    dictParam.Add("@Is_Active", "1");
        //}
        //dictParam.Add("@User_ID", intUserID.ToString());
        //dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
        //dictParam.Add("@Program_ID", "57");
        //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
        //ddlBranch.AddItemToolTip();
        //ddlBranch.ToolTip = ddlBranch.SelectedText;
    }

    private void FunPriLoadLob()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        if (PageMode == PageModes.Create)
        {
            dictParam.Add("@Is_Active", "1");
        }
        dictParam.Add("@User_ID", intUserID.ToString());
        dictParam.Add("@Program_ID", "57");
        ddlLOB.BindDataTable(SPNames.S3G_LOANAD_GetFactoringLOB, dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        ddlLOB.AddItemToolTip();
    }

    protected void FunProGenerateFactoringXMLDet()
    {
        try
        {
            strXMLFILDet = GRVFIL.FunPubFormXml();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to generate XML.");
        }
    }

    protected void FunProDisableControls(int mode)
    {
        try
        {
            switch (mode)
            {
                case 0:

                    Dictionary<string, string> dictFacInvoice = new Dictionary<string, string>();
                    dictFacInvoice.Add("@Company_ID", Convert.ToString(intCompanyID));
                    DataSet dsFactoringInvoiceDetails = new DataSet();
                    dsFactoringInvoiceDetails = Utility.GetDataset("S3G_LOANAD_GetFactoringDetails", dictFacInvoice);
                    if (string.IsNullOrEmpty(dsFactoringInvoiceDetails.Tables[1].Rows[0][0].ToString()))
                    {
                        Utility.FunShowAlertMsg(this, "Factoring Invoice gap days is not defined in the Global Parameter Setup", strRedirectPage);
                    }

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    pnlInvDtl.Visible = false;
                    lbltotal.Visible = false;
                    txtTotalEligibleAmt.Visible = false;
                    txtFILDate.Attributes.Add("readonly", "readonly");
                    break;
                case 1:
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    //if (ddlPAN.Items.Count != 0)
                    //{
                    //    ddlPAN.ClearDropDownList();
                    //}
                    ddlPAN.ReadOnly = true;
                    txtFILDate.ReadOnly = txtMargin.ReadOnly = txtCreditAvailable.ReadOnly = txtCreditDays.ReadOnly = txtInvoiceLoadValue.ReadOnly = txtCreditLimit.ReadOnly = txtOutStandingAmount.ReadOnly = true;
                  //  ddlBranch.ClearDropDownList();
                    if (ddlSAN.Items.Count != 0)
                    {
                        ddlSAN.ClearDropDownList();
                    }
                    imgFILDate.Visible = CECFILDATE.Enabled = false;
                    btnClear.Enabled = false;
                    txtFILDate.Attributes.Add("readonly", "readonly");
                    btnGo.Enabled = false;
                    break;
                case -1:
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    ddlPAN.ReadOnly = true;
                    if (ddlSAN.Items.Count != 0)
                    {
                        ddlSAN.ClearDropDownList();
                    }
                    //ddlBranch.ClearDropDownList();
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    btnGo.Enabled = false;
                    GRVFIL.Columns[GRVFIL.Columns.Count - 1].Visible = false;
                    txtFILDate.Attributes.Add("readonly", "readonly");
                    txtMargin.ReadOnly = txtCreditAvailable.ReadOnly = txtCreditDays.ReadOnly = txtInvoiceLoadValue.ReadOnly = txtCreditLimit.ReadOnly = txtOutStandingAmount.ReadOnly = true;
                    imgFILDate.Visible = CECFILDATE.Enabled = false;
                    btnGo.Enabled = false;
                    if (GRVFIL.FooterRow != null)
                    {
                        GRVFIL.FooterRow.Visible = false;
                    }
                    foreach (GridViewRow GvRow in GRVFIL.Rows)
                    {
                        TextBox txtInvoiceNoF = (TextBox)GvRow.FindControl("txtInvoiceNo");
                        TextBox txtPartyNameF = (TextBox)GvRow.FindControl("txtPartyName");
                        TextBox txtInvoiceAmountF = (TextBox)GvRow.FindControl("txtInvoiceAmount");
                        AjaxControlToolkit.CalendarExtender CEXInvDt = (AjaxControlToolkit.CalendarExtender)GvRow.FindControl("CalendarExtender1");
                        if (CEXInvDt != null)
                        {
                            CEXInvDt.Enabled = false;
                        }
                        if (txtInvoiceNoF != null)
                        {
                            txtInvoiceNoF.ReadOnly = true;
                        }
                        if (txtInvoiceAmountF != null)
                        {
                            txtInvoiceAmountF.ReadOnly = true;
                        }
                        if (txtPartyNameF != null)
                        {
                            txtPartyNameF.ReadOnly = true;
                        }
                    }
                    break;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to disable controls.");
        }
    }

    protected void FunProSetInitialRow()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SNo");
            dt.Columns.Add("Factoring_Inv_Load_Details_ID");
            dt.Columns.Add("Lock");
            dt.Columns.Add("Invoiceno");
            dt.Columns.Add("InvoiceDate");
            dt.Columns.Add("PartyName");
            dt.Columns.Add("MaturityDate");
            dt.Columns.Add("InvoiceAmount");
            dt.Columns.Add("EligibleAmount");
            dr = dt.NewRow();
            dr["SNo"] = 1;
            dr["Invoiceno"] = string.Empty;
            dr["Factoring_Inv_Load_Details_ID"] = "0";
            dr["Lock"] = "0";
            dr["InvoiceDate"] = string.Empty;
            dr["PartyName"] = string.Empty;
            dr["MaturityDate"] = string.Empty;
            dr["InvoiceAmount"] = string.Empty;
            dr["EligibleAmount"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["currenttable"] = dt;
            GRVFIL.DataSource = dt;
            GRVFIL.DataBind();
            GRVFIL.Rows[0].Visible = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to initiate the row");
        }
    }

    protected void FunProGetCustomerDetails()
    {
        try
        {
            Dictionary<string, string> dictFacInvoice = new Dictionary<string, string>();
            dictFacInvoice.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictFacInvoice.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictFacInvoice.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            dictFacInvoice.Add("@PANum", Convert.ToString(ddlPAN.SelectedValue));
            DataTable dtFactoringInvoiceDetails = Utility.GetDefaultData("S3G_LOANAD_GetCustomerDetailsByPAN", dictFacInvoice);
            if (dtFactoringInvoiceDetails.Rows.Count >= 1)
            {
                DataRow dtRow = dtFactoringInvoiceDetails.Rows[0];
                hidcuscode.Value = S3GCustomerAddress1.CustomerId = dtRow["Customer_ID"].ToString();

                S3GCustomerAddress1.SetCustomerDetails(dtRow, true);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to get Customer details");
        }
    }

    protected bool FunProGetFactoringInvoiceDetails()
    {
        try
        {
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@FIL_ID", Convert.ToString(intFILID));
            DS = Utility.GetDataset(SPNames.S3G_LOANAD_GetFactoringInvoiceDetails, dictParam);

            if (DS.Tables[0].Rows.Count >= 1)
            {
                int intSuffix = 0;
                if (ObjS3GSession.ProGpsSuffixRW > 2)
                {
                    intSuffix = 2;
                }
                else
                {
                    intSuffix = ObjS3GSession.ProGpsSuffixRW;
                }

                txtFILNo.Text = DS.Tables[0].Rows[0]["Fil_No"].ToString();
                DateTime Date = Convert.ToDateTime(DS.Tables[0].Rows[0]["Fil_Date"]);
                txtFILDate.Text = Date.ToString(strDateFormat);

                ddlLOB.Items.Add(new ListItem(DS.Tables[0].Rows[0]["LOB_Name"].ToString(), DS.Tables[0].Rows[0]["LOB_ID"].ToString()));
                ddlLOB.SelectedValue = DS.Tables[0].Rows[0]["LOB_ID"].ToString();
                ddlLOB.ToolTip = ddlLOB.SelectedItem.Text;
                //ddlBranch.SelectedValue = DS.Tables[0].Rows[0]["Branch_ID"].ToString();
                ddlBranch.SelectedText= DS.Tables[0].Rows[0]["Location_Name"].ToString();
                ddlBranch.SelectedValue = DS.Tables[0].Rows[0]["Location_ID"].ToString();
                ddlBranch.ToolTip = DS.Tables[0].Rows[0]["Location_Name"].ToString();
                ddlBranch.ReadOnly=true;
                //FunProLoadPrimeAccNo();
                ddlPAN.SelectedValue = DS.Tables[0].Rows[0]["PANum"].ToString();
                ddlPAN.SelectedText = DS.Tables[0].Rows[0]["PANum"].ToString();
                ddlPAN.ToolTip = ddlPAN.SelectedText;
                //Removed By Shibu Performance Issue 
               //FunProLoadSubAccNo();
                //if (ddlSAN.Items.Count != 0)
                //{
                //    ddlSAN.SelectedValue = DS.Tables[0].Rows[0]["SANum"].ToString();
                //    ddlSAN.ToolTip = ddlSAN.SelectedItem.Text;
                //    FunProGetAccountDetails(true);
                //}
                //else
                //{
                //    FunProGetAccountDetails(false);
                //}
                if (DS.Tables[0].Rows[0]["SANum"].ToString() != "")
                {
                    ddlSAN.Items.Add(new ListItem(DS.Tables[0].Rows[0]["SANum"].ToString(), DS.Tables[0].Rows[0]["SANum"].ToString()));
                    ddlSAN.ToolTip = DS.Tables[0].Rows[0]["SANum"].ToString();
                    ViewState["AccountDate"] = DS.Tables[0].Rows[0]["Creation_Date"].ToString();
                }
                else
                {
                    ddlSAN.Items.Add(new ListItem("--Select--","0"));
                }

                txtCreditDays.Text = DS.Tables[0].Rows[0]["Credit_Days"].ToString();
                txtInvoiceLoadValue.Text = DS.Tables[0].Rows[0]["Invoice_Load_Value"].ToString();
                txtStatus.Text = DS.Tables[0].Rows[0]["Status"].ToString();
                txtMargin.Text = DS.Tables[0].Rows[0]["Margin"].ToString();
                txtOutStandingAmount.Text = DS.Tables[0].Rows[0]["OutStandingAmount"].ToString();
                txtCreditLimit.Text = DS.Tables[0].Rows[0]["Credit_Limit"].ToString();

                rbtlBillType.SelectedValue = DS.Tables[0].Rows[0]["Bill_Type"].ToString();

                if (Convert.ToDecimal(DS.Tables[0].Rows[0]["CreditAvailable"].ToString()) >
                    Convert.ToDecimal(DS.Tables[0].Rows[0]["Credit_Limit"].ToString()))
                {
                    txtCreditAvailable.Text = Convert.ToString(Convert.ToDecimal(DS.Tables[0].Rows[0]["Credit_Limit"].ToString()) -
                        Convert.ToDecimal(DS.Tables[0].Rows[0]["Used_Invoice"].ToString()));
                }
                else
                {
                    txtCreditAvailable.Text = Convert.ToString(Convert.ToDecimal(DS.Tables[0].Rows[0]["CreditAvailable"].ToString()) -
                        Convert.ToDecimal(DS.Tables[0].Rows[0]["Used_Invoice"].ToString()));
                }

                if (Convert.ToDecimal(txtCreditAvailable.Text) < 0)
                {
                    txtCreditAvailable.Text = "0";
                }

                txtCreditAvailable.Text = Math.Round(Convert.ToDecimal(txtCreditAvailable.Text), intSuffix).ToString();
                txtOutStandingAmount.Text = Math.Round(Convert.ToDecimal(txtOutStandingAmount.Text), intSuffix).ToString();

                //else if (Convert.ToDecimal(DS.Tables[0].Rows[0]["CreditAvailable"].ToString()) > Convert.ToDecimal(txtCreditLimit.Text))
                //{
                //    txtCreditAvailable.Text = txtCreditLimit.Text;
                //}
                //else
                //{
                //    txtCreditAvailable.Text = DS.Tables[0].Rows[0]["CreditAvailable"].ToString();
                //}
                hidcuscode.Value = S3GCustomerAddress1.CustomerId = DS.Tables[0].Rows[0]["Customer_ID"].ToString();
                S3GCustomerAddress1.SetCustomerDetails(DS.Tables[0].Rows[0], true);
            }

            if (DS.Tables[1].Rows.Count >= 1)
            {
                GRVFIL.DataSource = DS.Tables[1];
                GRVFIL.DataBind();
                ViewState["currenttable"] = DS.Tables[1];
            }

            return Convert.ToBoolean(DS.Tables[0].Rows[0]["Can_Modify"].ToString());
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to get Factoring Details");
        }
    }

    protected void FunProLoadPrimeAccNo()
    {
        //try
        //{
        //    dictParam = new Dictionary<string, string>();
        //    dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //    dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        //    dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
        //    dictParam.Add("@Type", "Type6");
        //    dictParam.Add("@Param1", "1");
        //    dictParam.Add("@Is_Active", "1");

        //    if (PageMode == PageModes.Create)
        //    {
        //        ddlPAN.BindDataTable("S3G_LOANAD_GetPLASLA_List_for_FIL", dictParam, new string[] { "PANum", "PANum" });
        //    }
        //    else
        //    {
        //        ddlPAN.BindDataTable("S3G_LOANAD_GetPLASLA_List", dictParam, new string[] { "PANum", "PANum" });
        //    }
        //    ddlPAN.AddItemToolTip();
        //    ddlPAN.ToolTip = ddlPAN.SelectedItem.Text;
        //    ViewState["PANumDt"] = (DataTable)ddlPAN.DataSource;
        //}
        //catch (Exception objException)
        //{
        //      ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        //    throw new ApplicationException("Unable to load Prime Account Number");
        //}
        ddlPAN.Clear();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        //Procparam.Add("@UserID", obj_Page.intUserID.ToString());
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@Type", "Type6");
        Procparam.Add("@Param1", "1");
        Procparam.Add("@Is_Active", "1");
        if (obj_Page.PageMode == PageModes.Create)
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPLASLA_List_for_FIL_AGT", Procparam));
        }
        else
        {
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPLASLA_List_AGT", Procparam));
        }
        
        return suggetions.ToArray();
    }

    protected void FunProGetAccountDetails(bool FromSLA)
    {
        try
        {
            DataSet dtSet = new DataSet();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", intCompanyID.ToString());
            if (FromSLA)
            {
                dictParam.Add("@SANum", ddlSAN.SelectedValue.ToString());
            }
            else
            {
                dictParam.Add("@SANum", ddlPAN.SelectedValue.ToString() + "DUMMY");
            }
            dictParam.Add("@PANum", ddlPAN.SelectedValue);

            dtSet = Utility.GetTableValues(SPNames.S3G_LOANAD_GetPASARefID, dictParam);
            if (dtSet.Tables[0].Rows.Count > 0)
            {
                ViewState["AccountDate"] = dtSet.Tables[0].Rows[0]["Creation_Date"].ToString();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to get Account details");
        }
    }

    protected void FunProLoadSubAccNo()
    {
        try
        {
            //Sub A/c Number
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
            dictParam.Add("@Location_ID", ddlBranch.SelectedValue);
            dictParam.Add("@Param2", ddlPAN.SelectedText);
            dictParam.Add("@Type", "Type6");
            dictParam.Add("@Param1", "2");

            DataSet Dset = new DataSet();

            if (PageMode == PageModes.Create)
            {
                Dset = Utility.GetDataset("S3G_LOANAD_GetPLASLA_List_for_FIL", dictParam);
                ddlSAN.BindDataTable(Dset.Tables[0], new string[] { "SANum", "SANum" });
            }
            else
            {
                Dset = Utility.GetDataset("S3G_LOANAD_GetPLASLA_List", dictParam);
                ddlSAN.BindDataTable(Dset.Tables[0], new string[] { "SANum", "SANum" });
            }
            ddlSAN.AddItemToolTip();
            string IsBaseMLA = "";
            if (PageMode == PageModes.Create && Dset != null)
            {
                IsBaseMLA = Convert.ToString(Dset.Tables[1].Rows[0]["MLA_Applicable"]);

                if (IsBaseMLA != "" && IsBaseMLA == "1")
                {
                    if (ddlSAN.Items.Count == 1)
                    {
                        FunProClearControls(true);
                        //ddlPAN.SelectedIndex = -1;
                        ddlPAN.Clear();
                        Utility.FunShowAlertMsg(this.Page, "Sub account not exists. Cannot proceed further.");
                        return;
                    }
                    lblsubAccno.CssClass = "styleReqFieldLabel";
                    RFVSLA.Enabled = RFVSLA1.Enabled = true;
                }
                else
                {
                    lblsubAccno.CssClass = "styleDisplayLabel";
                    RFVSLA.Enabled = RFVSLA1.Enabled = false;
                    FunProGetAccountDetails(false);
                }
            }
            ddlSAN.ToolTip = ddlSAN.SelectedItem.Text;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to load Sub Account Number");
        }
    }

    protected void FunProGetFactDetails()
    {
        try
        {
            s = "";
            Dictionary<string, string> dictFacInvoice = new Dictionary<string, string>();
            dictFacInvoice.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictFacInvoice.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictFacInvoice.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            dictFacInvoice.Add("@PANum", Convert.ToString(ddlPAN.SelectedValue));
            if (ddlSAN.Items.Count == 0 || ddlSAN.SelectedValue == "0")
            {
                dictFacInvoice.Add("@SANum", ddlPAN.SelectedValue + "DUMMY");
            }
            else
            {
                dictFacInvoice.Add("@SANum", Convert.ToString(ddlSAN.SelectedValue));
            }
            DataSet dsFactoringInvoiceDetails = new DataSet();
            dsFactoringInvoiceDetails = Utility.GetDataset("S3G_LOANAD_GetFactoringDetails", dictFacInvoice);

            DataTable dtFactoringInvoiceDetails = dsFactoringInvoiceDetails.Tables[0];
            if (dtFactoringInvoiceDetails.Rows.Count >= 1)
            {
                DataRow dtRow = dtFactoringInvoiceDetails.Rows[0];
                txtStatus.Text = dtRow["Status"].ToString();
                txtMargin.Text = dtRow["Margin"].ToString() != "" ? dtRow["Margin"].ToString() : "0";
                txtOutStandingAmount.Text = dtRow["OutStandingAmount"].ToString();
                txtCreditLimit.Text = dtRow["Credit_Limit"].ToString();
                if (Convert.ToDecimal(dtRow["CreditAvailable"].ToString()) < 0)
                {
                    txtCreditAvailable.Text = "0";
                }
                else if (Convert.ToDecimal(dtRow["CreditAvailable"].ToString()) > Convert.ToDecimal(txtCreditLimit.Text))
                {
                    txtCreditAvailable.Text = txtCreditLimit.Text;
                }
                else
                {
                    txtCreditAvailable.Text = dtRow["CreditAvailable"].ToString();
                }
                //txtCreditAvailable.Text = Math.Round(Convert.ToDecimal(dtRow["CreditAvailable"].ToString()), 3).ToString();
                txtCreditDays.Text = dsFactoringInvoiceDetails.Tables[1].Rows[0][0].ToString();
                btnGo.Enabled = true;
            }
            else
            {
                btnGo.Enabled = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to load get Factoring Details");
        }
    }

    protected void FunProClearControls()
    {
        try
        {
            txtFILNo.Text = txtFILDate.Text = txtInvoiceLoadValue.Text = txtMargin.Text = txtOutStandingAmount.Text = txtStatus.Text = txtCreditLimit.Text = txtCreditDays.Text = txtCreditAvailable.Text = lblErrorMessage.Text = String.Empty;
            S3GCustomerAddress1.ClearCustomerDetails();
            GRVFIL.Visible = false;
            pnlInvDtl.Visible = false;
            txtTotalEligibleAmt.Visible = false;
            lbltotal.Visible = false;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to clear the values");
        }
    }

    protected string FunProGetColumnValue(DropDownList MyDLL, string strColumnName, DataTable Dt)
    {
        try
        {
            if (Dt != null)
            {
                DataRow[] DRows = Dt.Select(Convert.ToString(MyDLL.DataValueField) + " like '" + Convert.ToString(MyDLL.SelectedValue) + "%'");

                foreach (DataRow dr in DRows)
                {
                    return Convert.ToString(dr[strColumnName]);
                }
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion


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
        Procparam.Add("@Program_Id", "057");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }
}