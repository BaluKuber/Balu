using System;
using S3GBusEntity;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text;
using System.Web.Security;
using System.Configuration;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Collections;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

public partial class LoanAdmin_S3GLOANADAccountClosure_Add : ApplyThemeForProject
{

    #region [Intialization and Local Fields]

    int intCompanyID, intUserID = 0;
    Dictionary<string, string> Procparam = null;
    int intErrCode;
    int intSuffix;
    public string strProgramId = "73";
    static decimal dcFooterDue = 0, dcFooterReceived = 0, dcFooterOS = 0;
    static decimal dcDue = 0, dcWaived = 0, dcPayable = 0, dcClosure = 0, dcReceived = 0, decCTR = 0, decPLR = 0;
    static decimal dcWaivedOff, dcMinAmount;
    string strDateFormat = string.Empty;
    //static string strCustomerEmail = string.Empty;
    string strAccountClosure = string.Empty;
    int intClosureDetailId = 0;
    string strPANum = string.Empty;
    string strSANum = string.Empty;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strXMLAccountClosureDetails = "<Root><Details Desc='0' /></Root>";
    StringBuilder strbAccountClosureDetails = new StringBuilder();
    string strErrorMessagePrefix = @"Correct the following validation(s): </br></br>   ";

    public static LoanAdmin_S3GLOANADAccountClosure_Add obj_Page;

    ContractMgtServicesReference.ContractMgtServicesClient objAccountClosure_Client;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDataTable objS3G_LOANAD_AccClosureTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureRow objS3G_LOANAD_AccClosureDataRow = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsDataTable objS3G_LOANAD_AccDetailsTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDetailsRow objS3G_LOANAD_AccDetailsDataRow = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureDataTable objCancelDataTable = null;
    S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureRow objCancelRow = null;


    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLOANADAccountClosure_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACCO';";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACCO";
    string strPageName = "Account Closure";

    //ReportDocument rptd = new ReportDocument();

    #region [User Authorization]

    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;

    #endregion [User Authorization]

    #endregion [Intialization and Local Fields]

    #region [Event's]

    #region [Page Event's]

    protected new void Page_PreInit(object sender, EventArgs e) //transaction screen page init
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (rptd != null)
        //{
        //    rptd.Close();
        //    rptd.Dispose();
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPubPageLoad();
            FunPubSetErrorMessageControl();
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad;
            cvAccountClosure.IsValid = false;
        }
    }

    #endregion [Page Event's]

    #region [Button Event's]

    #region "Saving Account Closure Details"

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //FunPriFooterValue();
        try
        {
            //if (strAccountClosure == string.Empty)
            //{
            //    if (txtAccClosureDate.Text != "" && txtMatureDate.Text != "")
            //    {
            //        int intDtCom = Utility.CompareDates(txtMatureDate.Text, txtAccClosureDate.Text);
            //        if (intDtCom != 1)
            //        {
            //            cvAccountClosure.ErrorMessage = strErrorMessagePrefix + " Account Closure Date should be greater than or equal to Maturity Date";
            //            cvAccountClosure.IsValid = false;
            //            return;
            //        }
            //    }
            //}

            if (ddlTrancheName.SelectedText == "" && ddlMLA.SelectedText == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Select either Tranche or RS Number");
                return;
            }

            if (grvAccountBalance.Rows.Count == 0 && grvCashFlow.Rows.Count == 0)
            {
                cvAccountClosure.ErrorMessage = strErrorMessagePrefix + Resources.ValidationMsgs.S3G_ValMsg_LoanAd_AcClosure_Gene;
                cvAccountClosure.IsValid = false;
                return;
            }
            if (strAccountClosure == string.Empty && !string.IsNullOrEmpty(grvCashFlow.FooterRow.Cells[4].Text.Trim()))
            {
                if (grvCashFlow.FooterRow != null && grvCashFlow.FooterRow.Cells[4].Text.Trim() != "")
                {
                    //Commented on 7-JAN-2012 for UAT observations//
                    //if (Convert.ToDecimal(grvCashFlow.FooterRow.Cells[4].Text.Trim()) > dcMinAmount || Convert.ToDecimal(grvCashFlow.FooterRow.Cells[4].Text.Trim()) < -(dcMinAmount))
                    //{
                    //    cvAccountClosure.ErrorMessage = strErrorMessagePrefix + "Closure Amount has to be with in the permissable limit defined in Global Parameter Setup";
                    //    cvAccountClosure.IsValid = false;
                    //    return;
                    //}

                    //Added on 7-JAN-2012 for UAT observations//

                    //if (Convert.ToDecimal(grvCashFlow.FooterRow.Cells[6].Text.Trim()) != 0)
                    //{
                    //    if (Convert.ToDecimal(grvCashFlow.FooterRow.Cells[6].Text.Trim()) < 0)
                    //    {
                    //        cvAccountClosure.ErrorMessage = strErrorMessagePrefix + "Closure Amount Should not be in Credit";
                    //        cvAccountClosure.IsValid = false;
                    //        return;
                    //    }
                    //    else if (Convert.ToDecimal(grvCashFlow.FooterRow.Cells[6].Text.Trim()) > 0 )
                    //    {
                    //        cvAccountClosure.ErrorMessage = strErrorMessagePrefix + "Closure Amount should be Zero";
                    //        cvAccountClosure.IsValid = false;
                    //        return;
                    //    }
                    //}
                }
            }
            
            FunPubSaveAccountClosure();
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_LoanAd_AcClosure;
            cvAccountClosure.IsValid = false;
        }
    }

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // wf cancel
        if (PageMode == PageModes.WorkFlow)
            ReturnToWFHome();
        else
            Response.Redirect(strRedirectPage,false);
    }

    #region "Email / Print Functionality"

    protected void btnEmail_Click(object sender, EventArgs e)
    {
        bool booMail = true;
        try
        {
            FunPubSentMail(booMail);
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = "Invalid EMail ID. Mail not sent.";// Resources.ValidationMsgs.S3G_ErrMsg_MailSent;
            cvAccountClosure.IsValid = false;
        }
    }

    private StringBuilder GetHTMLTextEmail()
    {
        DataTable dtCompany = ViewState["Company"] as DataTable;
        string strCompany = string.Empty;
        string strAddress = string.Empty;
        string strAccNo = string.Empty;

        if (ddlMLA.SelectedValue != "0")
        {
            strAccNo =
                  "  for Prime Account No  " + ddlMLA.SelectedValue.Trim();
            //  +
            //" and Sub Account No  " + ddlSLA.SelectedItem.Text.Trim();
        }
        else
        {
            strAccNo =
              "  for Prime Account No  " + ddlMLA.SelectedValue.Trim();

        }
        if (dtCompany != null && dtCompany.Rows.Count > 0)
        {
            strCompany = dtCompany.Rows[0]["Company_Name"].ToString();
            strAddress = dtCompany.Rows[0]["Address1"].ToString();
            if (dtCompany.Rows[0]["City"].ToString() != "") strAddress += ", " + dtCompany.Rows[0]["City"].ToString();
            if (dtCompany.Rows[0]["City"].ToString() != "") strAddress += "-" + dtCompany.Rows[0]["Zip_Code"].ToString();
        }

        StringBuilder strMailBody = new StringBuilder();

        strMailBody.Append("<font size=\"1\"  color=\"black\" face=\"Times New Roman\">" +

           " <table width=\"100%\">" +

  "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Dear Customer," + "</font> " +
            "</td>" +
       " </tr>" +


      "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " With respect of you Account Closure requested on " + txtAccClosureDate.Text.Trim() + " " + strAccNo + ", Please find the Account Closure statement in the attached file. </font> " +
            "</td>" +
       " </tr>" +


       "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Yours Truly," + "</font> " +
            "</td>" +
       " </tr>" +
        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">" + strCompany + "</font> " +
            "</td>" +
       " </tr>" +
    "</table>" + "</font>");
        return strMailBody;

    }

    private string GetHTMLText()
    {
        DataTable dtCompany = ViewState["Company"] as DataTable;
        string strCompany = string.Empty;
        string strAddress = string.Empty;
        string strAccNo = string.Empty;

        if (ddlMLA.SelectedValue != "0")
        {
            strAccNo =
                  "  for Prime Account No  " + ddlMLA.SelectedValue.Trim();
                //  +
                //" and Sub Account No  " + ddlSLA.SelectedItem.Text.Trim();
        }
        else
        {
            strAccNo =
              "  for Prime Account No  " + ddlMLA.SelectedValue.Trim();

        }
        if (dtCompany != null && dtCompany.Rows.Count > 0)
        {
            strCompany = dtCompany.Rows[0]["Company_Name"].ToString();
            strAddress = dtCompany.Rows[0]["Address1"].ToString();
            if (dtCompany.Rows[0]["City"].ToString() != "") strAddress += "," + dtCompany.Rows[0]["City"].ToString();
            if (dtCompany.Rows[0]["City"].ToString() != "") strAddress += "-" + dtCompany.Rows[0]["Zip_Code"].ToString();
        }

        return
            "<font size=\"1\"  color=\"black\" face=\"Times New Roman\">" +

           " <table width=\"100%\">" +
        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">" + strCompany + "</font> " +
            "</td>" +
       " </tr>" +

        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">" + strAddress + "</font> " +
            "</td>" +
       " </tr>" +

          "<tr >" +
            "<td  align=\"Center\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "<b>" + " Account Closure Statement" + "</font> " + "</b>" +
            "</td>" +
       " </tr>" +
        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "<b>" + "Dear Sir / Madam," + "</font> " + "</b>" +
            "</td>" +
       " </tr>" +

       // "<tr >" +
            //     "<td  align=\"Center\" >" +
            //         "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "<b><u>" + " Sub: Account Closure Statement" + "</font> " + "</b></u>" +
            //     "</td>" +
            //" </tr>" +

        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + " Account has been closed sucessfully " + strAccNo + ".</font> " +
            "</td>" +
       " </tr>" +

         "<tr >" +
            "<td  align=\"left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Account Closure No: " + hidAccClosureNo.Value.Trim() + "</font> " + "</b></u>" +
            "</td>" +
       " </tr>" +

         "<tr >" +
            "<td  align=\"left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Account Closure Date: " + txtAccClosureDate.Text.Trim() + "</font> " + "</b></u>" +
            "</td>" +
       " </tr>" +

        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + "<b><u>" + " Account Closure Statement" + "</font> " + "</b></u>" +
            "</td>" +
       " </tr>" +

         "<tr >" +
            "<td  align=\"Left\" >"
        + "<table width=\"100%\" border=\"0\" >" +
            "<tr >" +
              "<th  align=\"Center\" >" + "Cash flow description" + "</th>" +
              "<th  align=\"Center\" >" + "Due Amount" + "</th>" +
              "<th  align=\"Center\" >" + "Waived Amount" + "</th>" +
              "<th align=\"Center\" >" + "Received Amount" + "</th>" +
              "<th align=\"Center\" >" + "Account Closure Amount" + "</th>" +

           "</tr>" + FunPriGetHtmlTable() +
         "</table>" +

         "</td>" +
        " </tr>" +

       "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\" >" + " Yours Truly," + "</font> " +
            "</td>" +
       " </tr>" +
        "<tr >" +
            "<td  align=\"Left\" >" +
                "<font size=\"1\"  color=\"Black\" face=\"Times New Roman\">" + strCompany + "</font> " +
            "</td>" +
       " </tr>" +
    "</table>" + "</font>";

    }

    private string FunPriGetHtmlTable()
    {
        string strHtml = string.Empty;
        foreach (GridViewRow gvRow in grvCashFlow.Rows)
        {
            Label lblCash = gvRow.FindControl("lblCash") as Label;
            Label lblDue = gvRow.FindControl("lblDue") as Label;
            TextBox txtWaived = gvRow.FindControl("txtWaived") as TextBox;
            Label lblPayable = gvRow.FindControl("lblPayable") as Label;
            Label lblClosure = gvRow.FindControl("lblClosure") as Label;
            //TextBox txtRemarks = gvRow.FindControl("txtRemarks") as TextBox;

            strHtml += "<tr>";
            strHtml += "<td>" + lblCash.Text.Trim() + "</td>";
            strHtml += "<td align='right'>" + lblDue.Text.Trim() + "</td>";
            strHtml += "<td align='right'>" + txtWaived.Text.Trim() + "</td>";
            strHtml += "<td align='right' >" + lblPayable.Text.Trim() + "</td>";
            strHtml += "<td align='right'>" + lblClosure.Text.Trim() + "</td>";
            //strHtml += "<td>" + txtRemarks.Text.Trim() + "</td>";
            strHtml += "</tr>";
        }

        strHtml += "<tr>";
        strHtml += "<td>" + (grvCashFlow.FooterRow.FindControl("lblTotal") as Label).Text.Trim() + "</td>";
        strHtml += "<td align='right'  >" + grvCashFlow.FooterRow.Cells[1].Text.Trim() + "</td>";
        strHtml += "<td align='right' > " + grvCashFlow.FooterRow.Cells[2].Text.Trim() + "</td>";
        strHtml += "<td align='right' >" + grvCashFlow.FooterRow.Cells[3].Text.Trim() + "</td>";
        strHtml += "<td align='right' >" + grvCashFlow.FooterRow.Cells[4].Text.Trim() + "</td>";
        strHtml += "<td>" + grvCashFlow.FooterRow.Cells[6].Text.Trim() + "</td>";
        strHtml += "</tr>";
        return strHtml;
    }

    string strnewFile = "";
    protected void PreviewPDF_Click(bool blnPrint)
    {
        try
        {
            //String htmlText = GetHTMLText();
            Guid objGuid;
            objGuid = Guid.NewGuid();

            strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + objGuid + "Closure Statement.pdf");

            DataSet Dst = null;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@PANum", ddlMLA.SelectedValue);
            Procparam.Add("@SANum", ddlMLA.SelectedValue + "DUMMY");
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
           // Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@Closure_Details_ID", hidClosureDetailId.Value);

            Dst = Utility.GetDataset("[S3G_LOANAD_AccountClosureStatement]", Procparam);
                        
            //rptd.Load(Server.MapPath("AccountClosure.rpt"));
            //rptd.SetDataSource(Dst.Tables[0]);
            //rptd.Subreports["AccountDetails"].SetDataSource(Dst.Tables[1]);
            //rptd.Subreports["Assetdetails"].SetDataSource(Dst.Tables[2]);
            //if (Dst.Tables[3] != null) rptd.Subreports["Pdc details"].SetDataSource(Dst.Tables[3]);
            //if (Dst.Tables[4] != null) rptd.Subreports["Collateral"].SetDataSource(Dst.Tables[4]);
            //rptd.ExportToDisk(ExportFormatType.PortableDocFormat, strnewFile);
            //Server.MapPath(".") + "\\PDF Files\\" + ddlSLA.SelectedValue);

            string strScipt = "";
            if (blnPrint)
            {
                strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strnewFile.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }
        }
        catch (IOException winOpne)
        {
            if (blnPrint)
                System.Diagnostics.Process.Start(strnewFile);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvAccountClosure.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PreviewDoc;
            cvAccountClosure.IsValid = false;
        }
        finally
        {
            //if (rptd != null)
            //{
            //    rptd.Close();
            //    rptd.Dispose();
            //}
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            PreviewPDF_Click(true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    #endregion

    #region [Account Closure Cancellation]

    protected void btnClosure_Click(object sender, EventArgs e)
    {
        try
        {
            objAccountClosure_Client = new ContractMgtServicesReference.ContractMgtServicesClient();
            objCancelDataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_CancelClosureDataTable();
            objCancelRow = objCancelDataTable.NewS3G_LOANAD_CancelClosureRow();
            objCancelRow.PANum = strPANum;
            objCancelRow.Closure_No = txtAccClosureNo.Text.ToString();
            objCancelRow.SANum = strSANum;
            objCancelRow.Closure_Type = 1;//Closure Type
            objCancelRow.Company_ID = intCompanyID;
            objCancelRow.User_Id = intUserID;

            objCancelDataTable.AddS3G_LOANAD_CancelClosureRow(objCancelRow);
            intErrCode = objAccountClosure_Client.FunPubCancelAccountClosure(ObjSerMode, ClsPubSerialize.Serialize(objCancelDataTable, ObjSerMode), 0);

            if (intErrCode == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Account Closure cancelled sucessfully ');" + strRedirectPageView, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Account Closure  has gone through approval and cannot be cancelled');" + strRedirectPageView, true);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    #endregion [Account Closure Cancellation]

    #endregion [Button Event's]

    #region [Grid Event's]

    #region [Only in Modify Mode]

    protected void grvCashFlow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCashFlowID = e.Row.FindControl("hdnCashFlowID") as HiddenField;
                TextBox txtWaived = e.Row.FindControl("txtWaived") as TextBox;
                Label lblDue = e.Row.FindControl("lblDue") as Label;
                TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
                if (ViewState["vwClosure_Status"] != null)
                {
                    if (Convert.ToInt32(ViewState["vwClosure_Status"]) != 1)
                        txtWaived.ReadOnly = txtRemarks.ReadOnly = true;
                }
                txtWaived.SetDecimalPrefixSuffix(25, 2, false, "Waived Amount");
                if (hdnCashFlowID.Value == "24" || hdnCashFlowID.Value == "26" || hdnCashFlowID.Value == "42" || hdnCashFlowID.Value == "98") //ODI, Memo, Penalty & Writeoff Exp
                    txtWaived.Visible = true;
                else
                    txtWaived.Visible = false;

                //if (!string.IsNullOrEmpty(txtWaived.Text.Trim()))
                //    dcWaived = dcWaived + Convert.ToDecimal(txtWaived.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = dcDue.ToString();
                e.Row.Cells[4].Text = dcWaived.ToString();
                e.Row.Cells[5].Text = dcPayable.ToString();
                e.Row.Cells[6].Text = dcClosure.ToString();
                
            }
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    #endregion [Only in Modify Mode]

    #endregion [Grid Event's]

    #region [Drop Down Event's]

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            ddlMLA.Clear();
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    protected void ddlTrancheName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            ddlMLA.Clear();
            if (ddlTrancheName.SelectedValue != "0")
            {
                string strError = "";
                Procparam = new Dictionary<string, string>();
                if (ddlMLA.SelectedValue.ToString() != "0")
                    Procparam.Add("@PA_SA_REF_ID", ddlMLA.SelectedValue.ToString());
                Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@Tranche_id", ddlTrancheName.SelectedValue);
                if (txtAccClosureDate.Text.Trim() != "")
                    Procparam.Add("@PMCDate", Utility.StringToDate(txtAccClosureDate.Text).ToString());

                strError = Utility.GetTableScalarValue("S3G_LOANAD_CheckTranForClosure", Procparam);

                if (strError == "1")
                {
                    Utility.FunShowAlertMsg(this, "Transactions are available and user to do the reversal and then to proceed");
                    return;
                }
            }
                PopulatePANCustomer(ddlMLA.SelectedValue);
                FunGetAccountDetails(ddlMLA.SelectedValue);
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    protected void ddlMLA_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            if (ddlMLA.SelectedValue !="0")
            {
                string strError = "";
                Procparam = new Dictionary<string, string>();
                if (ddlMLA.SelectedValue.ToString() != "0")
                    Procparam.Add("@PA_SA_REF_ID", ddlMLA.SelectedValue.ToString());
                Procparam.Add("@User_ID", intUserID.ToString());
                Procparam.Add("@Tranche_id", ddlTrancheName.SelectedValue);
                if (txtAccClosureDate.Text.Trim() != "")
                    Procparam.Add("@PMCDate", Utility.StringToDate(txtAccClosureDate.Text).ToString());

                strError = Utility.GetTableScalarValue("S3G_LOANAD_CheckTranForClosure", Procparam);

                if (strError == "1")
                {
                    Utility.FunShowAlertMsg(this, "Transactions are available and user to do the reversal and then to proceed");
                    return;
                }
                PopulatePANCustomer(ddlMLA.SelectedValue);
                FunGetAccountDetails(ddlMLA.SelectedValue);
            }
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    protected void txtWaived_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtWaived = (sender) as TextBox;
            GridViewRow gvRow = txtWaived.Parent.Parent as GridViewRow;
            Label lblDue = gvRow.FindControl("lblDue") as Label;
            Label lblPayable = gvRow.FindControl("lblPayable") as Label;
            Label lblClosure = gvRow.FindControl("lblClosure") as Label;
            HiddenField hdnCashFlowID = gvRow.FindControl("hdnCashFlowID") as HiddenField;

            if (!String.IsNullOrEmpty(txtWaived.Text.Trim()))
            {
                if (Convert.ToDecimal(txtWaived.Text) > Convert.ToDecimal(lblDue.Text))
                {
                    Utility.FunShowAlertMsg(this, "WaivedOff amount should be less than or equal to Due Amount");
                    txtWaived.Text = "";
                    return;
                }
                lblClosure.Text = (Convert.ToDecimal(lblDue.Text) - Convert.ToDecimal(txtWaived.Text) - Convert.ToDecimal(lblPayable.Text)).ToString();
            }
            else
                lblClosure.Text = (Convert.ToDecimal(lblDue.Text) - Convert.ToDecimal(lblPayable.Text)).ToString();

            Label lblReceived = gvRow.FindControl("lblReceived") as Label;
            lblReceived.Text = txtWaived.Text;

            dcWaived = dcClosure = 0;
            foreach (GridViewRow grRow in grvCashFlow.Rows)
            {
                lblClosure = grRow.FindControl("lblClosure") as Label;
                txtWaived = grRow.FindControl("txtWaived") as TextBox;

                if (!string.IsNullOrEmpty(txtWaived.Text.Trim()))
                    dcWaived = dcWaived + Convert.ToDecimal(txtWaived.Text);
                if (!string.IsNullOrEmpty(lblClosure.Text.Trim()))
                    dcClosure = dcClosure + Convert.ToDecimal(lblClosure.Text);
            }
            grvCashFlow.FooterRow.Cells[4].Text = dcWaived.ToString();
            grvCashFlow.FooterRow.Cells[6].Text = dcClosure.ToString();
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    protected void txtRemarks_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtRemarks = (sender) as TextBox;
            GridViewRow gvRow = txtRemarks.Parent.Parent as GridViewRow;
            HiddenField hdnCashFlowID = gvRow.FindControl("hdnCashFlowID") as HiddenField;
            if (ViewState["ClosureStatement"] != null)
            {
                DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                DataRow[] drClosureStatement = dtClosureStatement.Select    (" ID = '" + hdnCashFlowID.Value + "'  ");
                drClosureStatement[0].BeginEdit();
                drClosureStatement[0]["Remarks"] = txtRemarks.Text.Trim();
                drClosureStatement[0].AcceptChanges();
                ViewState["ClosureStatement"] = dtClosureStatement;
            }
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    #endregion [Drop Down Event's]

    #endregion [Event's]

    #region [Function's]

    private void FunPriFooterValue()
    {
        try
        {
            if (grvCashFlow.FooterRow != null)
            {
                grvCashFlow.FooterRow.Cells[1].Text = dcDue.ToString();
                grvCashFlow.FooterRow.Cells[2].Text = dcWaived.ToString();
                grvCashFlow.FooterRow.Cells[3].Text = dcPayable.ToString();
                grvCashFlow.FooterRow.Cells[4].Text = dcClosure.ToString();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    public void FunPubPageLoad()
    {
        try
        {
            obj_Page = this;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (Request.QueryString["Popup"] != null)  //transaction screen page load
            {
                btnCancel.Enabled = false;
                btnEmail.Enabled = false;
                btnPrint.Enabled = false;
            }
            if (Request.QueryString["Popup"] == null)
                FunPubSetIndex(1);
            //Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            intSuffix = ObjS3GSession.ProGpsSuffixRW;
            //End
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            //CalendarNOCDate.Format = strDateFormat;
            CalendarExtender2.Format = strDateFormat;
            txtAccClosureDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtAccClosureDate.ClientID + "','" + strDateFormat + "', false, false);");

            if (Request.QueryString["qsViewId"] != null)
            {
                string strview = Request.QueryString["qsViewId"].ToString();
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(strview);
                //strAccountClosure = fromTicket.Name.Split('~')[0];
                //strPANum = fromTicket.Name.Split('~')[1];
                //strSANum = fromTicket.Name.Split('~')[2];
                //intClosureDetailId = Convert.ToInt32(fromTicket.Name.Split('~')[3]);
                intClosureDetailId = Convert.ToInt32(fromTicket.Name);
                hidClosureDetailId.Value = intClosureDetailId.ToString();
                if (strSANum == string.Empty)
                    strSANum = strPANum + "DUMMY";
            }
            //else
            //{
            //    btnSave.Click += new EventHandler(btnEmail_Click);
            //    btnSave.Click += new EventHandler(btnPrint_Click);        
            //}

            if (!IsPostBack)
            {
                if (Request.QueryString["qsMode"] == "C")
                {
                    PopulateLOBList();
                }
                
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                txtClosureBy.Text = ObjUserInfo.ProUserNameRW.Trim().ToUpper();

                if (Request.QueryString["qsMode"] == "Q")
                {
                    FunGetAccountsClosedForModify(intClosureDetailId);
                    FunGetAccountDetailsForClosure(strPANum);
                    FunPriDisableControls(-1);
                }
                else if (Request.QueryString["qsMode"] == "M")
                {
                    FunGetAccountsClosedForModify(intClosureDetailId);
                    FunGetAccountDetailsForClosure(strPANum);
                    FunPriDisableControls(1);
                }
                else
                {
                    FunPriDisableControls(0);
                    txtAccClosureDate.Text = txtAccDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                }
                // Added by Tamilselvan on 01/02/2011 Closure done By is not for user entry
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    public void FunPubSaveAccountClosure()
    {
        objAccountClosure_Client = new ContractMgtServicesReference.ContractMgtServicesClient();
        string strDocNo = "";
        try
        {
            objS3G_LOANAD_AccClosureTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountClosureDataTable();
            objS3G_LOANAD_AccClosureDataRow = objS3G_LOANAD_AccClosureTable.NewS3G_LOANAD_AccountClosureRow();

            objS3G_LOANAD_AccClosureDataRow.Company_ID = intCompanyID;
            objS3G_LOANAD_AccClosureDataRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objS3G_LOANAD_AccClosureDataRow.Branch_ID = 0;
            objS3G_LOANAD_AccClosureDataRow.Closure_No = strAccountClosure;
            objS3G_LOANAD_AccClosureDataRow.PANum = ddlMLA.SelectedValue;
            objS3G_LOANAD_AccClosureDataRow.SANum = ddlMLA.SelectedText + "DUMMY";
            objS3G_LOANAD_AccClosureDataRow.Customer_ID = Convert.ToInt32(hdnCustomerID.Value); // Convert.ToInt32(txtCustCode.Attributes["Cust_ID"]);
            objS3G_LOANAD_AccClosureDataRow.Closure_Date = Utility.StringToDate(txtAccClosureDate.Text); // DateTime.Now;
            //if (!String.IsNullOrEmpty(txtNOCDate.Text))txtAccClosureDate
              objS3G_LOANAD_AccClosureDataRow.NOC_date = Utility.StringToDate(txtAccClosureDate.Text);
            objS3G_LOANAD_AccClosureDataRow.Created_By = intUserID;
            objS3G_LOANAD_AccClosureDataRow.Created_On = DateTime.Now;
            objS3G_LOANAD_AccClosureDataRow.Closure_Amount = 0;
            objS3G_LOANAD_AccClosureDataRow.User_ID = intUserID;
            objS3G_LOANAD_AccClosureDataRow.Tranche_id = Convert.ToInt32(ddlTrancheName.SelectedValue);
            //Source added by tamilselvan.S on 28/01/2011 for Account Creation Details 
            objS3G_LOANAD_AccClosureDataRow.XMLAccountClosureDetails = FunPriGenerateAccountClosureDetailsXMLDet();
            objS3G_LOANAD_AccClosureTable.AddS3G_LOANAD_AccountClosureRow(objS3G_LOANAD_AccClosureDataRow);

            intErrCode = objAccountClosure_Client.FunPubCreateAccountClosure(out strDocNo, out intClosureDetailId, ObjSerMode, ClsPubSerialize.Serialize(objS3G_LOANAD_AccClosureTable, ObjSerMode), 0);
            bool booMail = false;
            hidAccClosureNo.Value = strDocNo;
            string strMessage = "";
            hidClosureDetailId.Value = intClosureDetailId.ToString();
            switch (intErrCode)
            {
                case 0:
                    {

                        if (strAccountClosure == string.Empty)
                        {
                            btnSave.Enabled = false;
                            //strAccountClosure = strDocNo;
                            //try
                            //{
                            //    FunPubSentMail(booMail);
                            //}
                            //catch (Exception exMail)
                            //{
                            //    strMessage = "Invalid EMail ID. Mail not sent to the customer";
                            //}

                            strAlert = "";
                            //if (txtMode.Text.ToUpper().Trim() == "PDC")
                            //{
                            //    strAlert += "All Post dated cheques are processed. No more pending Post dated cheques... ";
                            //}
                            strAlert += Resources.ValidationMsgs.S3G_SucMsg_LoanAd_AcClosed + " - " + strDocNo;
                            strAlert += @" \n\n" + strMessage;
                            strAlert += @"\n\nWould you like to generate one more Rental Schedule Closure?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            btnSave.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + " Account Closure " + Resources.ValidationMsgs.S3G_SucMsg_Update + "');" + strRedirectPageView, true);   //Details Updated succesfully
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        break;
                    }
                case 1:
                    {
                        if (strAccountClosure == string.Empty)
                        {
                            btnSave.Enabled = false;
                            //strAccountClosure = strDocNo;
                            ////try
                            ////{
                            ////    FunPubEmailSent(booMail);
                            ////}
                            //catch (Exception exMail)
                            //{
                            //    strMessage = "Invalid EMail ID. Mail not sent to the customer";
                            //}
                            strAlert = "__ALERT__";
                            //if (txtMode.Text.ToUpper().Trim() == "PDC")
                            //{
                            //    strAlert += "All Post dated cheques are processed. No more pending Post dated cheques... ";
                            //}



                            strAlert = strAlert.Replace("__ALERT__", "Rental Schedule Closure  Details updated successfully");
                            //strAlert += @"\n" + strMessage;
                            strAlert += @"\n\nWould you like to generate one more Rental Schedule Closure ?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);

                            btnSave.Enabled = false;

                            strRedirectPageView = "";
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END

                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + " Rental Schedule Closure " + Resources.ValidationMsgs.S3G_SucMsg_Update + "');" + strRedirectPageView, true);
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        break;
                    }
                case 7:
                    {
                        if (strAccountClosure == string.Empty)
                        {
                            btnSave.Enabled = false;
                            strAccountClosure = strDocNo;
                            try
                            {
                                FunPubSentMail(booMail);
                            }
                            catch (Exception exMail)
                            {
                                strMessage = "Invalid EMail ID. Mail not sent";
                            }
                            strAlert = "Post dated cheques are not disposed to the customer... ";
                            strAlert += Resources.ValidationMsgs.S3G_SucMsg_LoanAd_AcClosed + " - " + strDocNo;
                            strAlert += @" \n\n" + strMessage;
                            strAlert += @"\n\nWould you like to view the Closure Statement?";
                            strAlert = "if(confirm('" + strAlert + "')){ document.getElementById('" + Button1.ClientID + "').click();}else {if(confirm('" + (@Resources.ValidationMsgs.S3G_ValMsg_loanAd_AcClose1More).Replace("\\n", "") + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}" + "}";
                            strRedirectPageView = "";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('" + " Account Closure " + Resources.ValidationMsgs.S3G_SucMsg_Update + "');" + strRedirectPageView, true);   //Details Updated succesfully
                            //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                            btnSave.Enabled = false;
                            //END
                        }
                        break;
                    }
                case 12:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Billing should be run up to Account Closure Date ");
                        break;
                    }
                case 3:
                    {
                        Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                        break;
                    }
                case 4:
                    {
                        Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                        break;
                    }
                case 5:
                    {
                        if (strAccountClosure == string.Empty)
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Month Closure should be open for iniatiating Account closure');" + strRedirectPageAdd, true);   //Month Closure should be open for iniatiating Pre Mature closure 
                        break;
                    }
                case 17:
                    {
                        Utility.FunShowAlertMsg(this.Page, "Lien Account ‘" + ViewState["LienContracts"] + "’ for this Account not closed");
                        break;
                    }
            }
        }
        catch (FaultException<ContractMgtServicesReference.ClsPubFaultException> ex)
        {
            FunPubStoreErrorMsgAndCloseObj(true, ex, null, objAccountClosure_Client);
            throw;
        }
        catch (Exception ex)
        {
            FunPubStoreErrorMsgAndCloseObj(true, null, ex, objAccountClosure_Client);
            throw;
        }
        finally
        {
            FunPubStoreErrorMsgAndCloseObj(false, null, null, objAccountClosure_Client);
        }
    }

    public void FunPubSentMail(bool booMail)
    {
        FunPriFooterValue();
        string strCustomerEmail = (ucdCustomer.FindControl("txtEmail") as TextBox).Text.Trim();
        if (booMail && strCustomerEmail == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Customer does not have a Email Id');", true);   //Please select the Sub Account No');", true);
            return;
        }

        if (strCustomerEmail != string.Empty)
        {
            if (strnewFile == "")
                PreviewPDF_Click(false);

            Dictionary<string, string> dictMail = new Dictionary<string, string>();
            dictMail.Add("FromMail", "s3g@sundaraminfotech.in");
            dictMail.Add("ToMail", strCustomerEmail);
            dictMail.Add("Subject", "Account Closure Statement");
            ArrayList arrMailAttachement = new ArrayList();
            StringBuilder strBody = new StringBuilder();
            strBody = GetHTMLTextEmail();
            if (strnewFile != "")
            {
                arrMailAttachement.Add(strnewFile);
            }
            Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
        }
    }

    #region [Store Error Message and Close service Object]

    public void FunPubStoreErrorMsgAndCloseObj(bool bolEx, FaultException<ContractMgtServicesReference.ClsPubFaultException> Fex, Exception ex, ContractMgtServicesReference.ContractMgtServicesClient ObjAccountCreation)
    {
        if (bolEx)
              ClsPubCommErrorLogDB.CustomErrorRoutine(Fex);
        else
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        if (ObjAccountCreation != null)
            ObjAccountCreation.Close();
    }

    #endregion [Store Error Message]

    #region [LOB/Branch Dropdown list]

    private void PopulateLOBList()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_ID", Convert.ToString(intUserID));
            Procparam.Add("@Program_ID", strProgramId);
            if (strAccountClosure == string.Empty) Procparam.Add("@Is_Active", "1");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if(ddlLOB.Items.Count==2)
            {
                ddlLOB.SelectedIndex=1;
                ddlLOB.ClearDropDownList();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion [LOB/Branch Dropdown list]

    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
       // Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Prefix", prefixText);
        Procparam.Add("@Type", "1");// Closure_Type code
        if (obj_Page.ddlTrancheName.SelectedValue != "0")
            Procparam.Add("@Tranch_Header_ID", obj_Page.ddlTrancheName.SelectedValue);
        if (obj_Page.strAccountClosure == string.Empty)
            Procparam.Add("@IsModify", "0");
        else
            Procparam.Add("@IsModify", "1");
        Procparam.Add("@PMCDate", Utility.StringToDate(obj_Page.txtAccClosureDate.Text).ToString());
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPANumForAccountClosure_AGT", Procparam));

        return suggetions.ToArray();
    }

    private void FunPriClearControls()
    {
        ucdCustomer.ClearCustomerDetails();
        txtAccDate.Text = txtPrincipal.Text = txtMatureDate.Text = txtIRR.Text = "";
        txtBusinessIRR.Text = txtCompanyIRR.Text = txtFlatRate.Text = txtTenure.Text = txtMode.Text = txtFinanceCharge.Text = "";
        grvAsset.ClearGrid();
        grvAccountBalance.ClearGrid();
        btnEmail.Enabled = btnPrint.Enabled = false;
        tbAccDetails.Enabled = tbCashFlow.Enabled = tbFunderDetails.Enabled = false;
    }

    #endregion

    private void PopulatePANCustomer(string strPAN)
    {
        try
        {
            DataTable dtTable = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@PANum", ddlMLA.SelectedValue);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@program_id", strProgramId);
            if (Convert.ToInt32(ddlTrancheName.SelectedValue) > 0)
                Procparam.Add("@tranche_id", ddlTrancheName.SelectedValue);

            dtTable = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetPANumCustomer, Procparam);

            if (dtTable != null && dtTable.Rows.Count > 0)
            {
                //string strcustomerName = Convert.ToString(dtTable.Rows[0]["Title"] + " " + dtTable.Rows[0]["Customer_Name"]);
                string strcustomerName = Convert.ToString(dtTable.Rows[0]["Customer_Name"]);
                dtTable.Rows[0]["Customer_Name"] = strcustomerName;
                ucdCustomer.SetCustomerDetails(dtTable.Rows[0], true);
                //strCustomerEmail = dtTable.Rows[0]["Comm_Email"].ToString();
                hdnCustomerID.Value = dtTable.Rows[0]["Customer_ID"].ToString();
            }
            }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunGetAccountDetails(string strPAN)
    {
        try
        {
            //if (ddlMLA.SelectedValue != "0")
            //{
                tbAccDetails.Enabled = tbCashFlow.Enabled = tbFunderDetails.Enabled = true;
                //if (strAccountClosure == string.Empty)
                //{
                    FunGetAccountDetailsForClosure(strPAN);
                //}
            //}
            //else
            //{
                //tbAccDetails.Enabled = tbCashFlow.Enabled = tbFunderDetails.Enabled = false;
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunGetAccountDetailsForClosure(string strPANum)
    {
        int intTenureType = 0;
        double intTenure = 0;
        DataSet dtSet = new DataSet();
        string strTenureDesc = string.Empty;
        int Tenure = 0;

        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            if (ddlMLA.SelectedValue != "0")
                Procparam.Add("@PANum", ddlMLA.SelectedValue);
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
          //  Procparam.Add("@Location_ID", ddlBranch.SelectedValue.ToString());
            Procparam.Add("@ClosureDetailId", intClosureDetailId.ToString());
            if (ddlTrancheName.SelectedValue != "0")
                Procparam.Add("@Tranche_Id", ddlTrancheName.SelectedValue);
            Procparam.Add("@User_ID", intUserID.ToString());

            dtSet = Utility.GetTableValues("S3G_LOANAD_GetAccountDetailsACCClosure", Procparam);

            if (dtSet != null)
            {
                if (dtSet.Tables[0] != null && dtSet.Tables[0].Rows.Count > 0)
                {
                    txtAccDate.Text = dtSet.Tables[0].Rows[0]["Creation_Date"].ToString();
                    //txtPrincipal.Text = dtSet.Tables[0].Rows[0]["Finance_Amount"].ToString();
                    intTenureType = Convert.ToInt32(dtSet.Tables[0].Rows[0]["Tenure_Code"]);
                    intTenure = Convert.ToDouble(dtSet.Tables[0].Rows[0]["Tenure"]);

                    txtMatureDate.Text = dtSet.Tables[0].Rows[0]["MatureDate"].ToString();
                    txtIRR.Text = dtSet.Tables[0].Rows[0]["Accounting_IRR"].ToString();
                    txtBusinessIRR.Text = dtSet.Tables[0].Rows[0]["Business_IRR"].ToString();
                    txtCompanyIRR.Text = dtSet.Tables[0].Rows[0]["Company_IRR"].ToString();
                    strTenureDesc = dtSet.Tables[0].Rows[0]["TenureDesc"].ToString();
                    txtTenure.Text = dtSet.Tables[0].Rows[0]["Tenure"].ToString() + " " + strTenureDesc;
                    Tenure = Int32.Parse(dtSet.Tables[0].Rows[0]["Tenure"].ToString());
                    grvaccount.DataSource = dtSet.Tables[0];
                    grvaccount.DataBind();
                }

                if (dtSet.Tables[1] != null && dtSet.Tables[1].Rows.Count > 0)
                {
                    grvAccountBalance.DataSource = dtSet.Tables[1];
                    grvAccountBalance.DataBind();
                    FunPriBindAccountBalanceGrid(dtSet.Tables[1]);
                }
                if (strMode == "")
                {
                    ViewState["ClosureStatement"] = dtSet.Tables[2];

                    if (dtSet.Tables[2] != null && dtSet.Tables[2].Rows.Count > 0)
                    {
                        FunPriBindCashFlowGrid();
                    }

                    ViewState["Company"] = dtSet.Tables[3];
                    if (dtSet.Tables[4].Rows.Count > 0)
                    {
                        grvfunderdetailss.DataSource = dtSet.Tables[4];
                        grvfunderdetailss.DataBind();
                    }
                }
                ViewState["LienContracts"] = dtSet.Tables[7].Rows[0]["Lien_Contract"].ToString();
            }
        }
        catch (FaultException<ContractMgtServicesReference.ClsPubFaultException> ex)
        {
            //FunPubStoreErrorMsgAndCloseObj(true, ex, null, objAccountClosure_Client);
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        catch (Exception ex)
        {
            //FunPubStoreErrorMsgAndCloseObj(true, null, ex, objAccountClosure_Client);
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        finally
        {
            //FunPubStoreErrorMsgAndCloseObj(false, null, null, objAccountClosure_Client);
        }
    }

    private void FunPriBindAccountBalanceGrid(DataTable dtAccountBalanc)
    {
        try
        {
            dcFooterDue = Convert.ToDecimal(dtAccountBalanc.Compute("sum(outstanding1)", "Desc<>''"));
            dcFooterReceived = Convert.ToDecimal(dtAccountBalanc.Compute("sum(OPC_fut_rec1)", "Desc<>'' "));
            dcFooterOS = Convert.ToDecimal(dtAccountBalanc.Compute("sum(fund_fut_rec1)", "Desc<>'' "));
            FunPriSetABFooterValue();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriSetABFooterValue()
    {
        try
        {
            if (grvAccountBalance.FooterRow != null)
            {
                grvAccountBalance.FooterRow.Cells[3].Text = dcFooterDue.ToString();
                grvAccountBalance.FooterRow.Cells[4].Text = dcFooterReceived.ToString();
                grvAccountBalance.FooterRow.Cells[5].Text = dcFooterOS.ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriBindCashFlowGrid()
    {
        try
        {
            if (ViewState["ClosureStatement"] != null)
            {
                DataTable dtClosureStatement = ViewState["ClosureStatement"] as DataTable;
                grvCashFlow.DataSource = dtClosureStatement;
                grvCashFlow.DataBind();

                dcDue = Convert.ToDecimal(dtClosureStatement.Compute("sum(due1)", "Description<>''"));
                dcPayable = Convert.ToDecimal(dtClosureStatement.Compute("sum(Payable1)", "Description<>'' "));
                dcClosure = Convert.ToDecimal(dtClosureStatement.Compute("sum(Closure1)", "Description<>'' "));
                dcWaived = Convert.ToDecimal(dtClosureStatement.Compute("sum(Waived1)", "Description<>''  "));
                dcReceived = Convert.ToDecimal(dtClosureStatement.Compute("sum(Received1)", "Description<>''  "));
                FunPriSetFooterValue();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriSetFooterValue()
    {
        try
        {
            if (grvCashFlow.FooterRow != null)
            {
                grvCashFlow.FooterRow.Cells[3].Text = dcDue.ToString();
                grvCashFlow.FooterRow.Cells[4].Text = dcWaived.ToString();
                grvCashFlow.FooterRow.Cells[5].Text = dcPayable.ToString();
                grvCashFlow.FooterRow.Cells[6].Text = dcClosure.ToString();
                grvCashFlow.FooterRow.Cells[7].Text = dcReceived.ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #region [User Authorization]

    /// <summary>
    /// This is used to implement User Authorization
    /// </summary>
    /// <param name="intModeID"></param>
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
                //btnSave.Enabled = btnCancel.Enabled = true;
                txtStatus.Text = "Pending";
                break;

            case 1: // Modify Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                tbAccDetails.Enabled = true;
                tbCashFlow.Enabled = true;
                tbFunderDetails.Enabled = true;
                if (bDelete)
                    btnClosure.Visible = true;
                txtClosureBy.ReadOnly = true;
                //btnPrint.Enabled = true;
                break;

            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                btnSave.Enabled = false;
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                tbAccDetails.Enabled = tbCashFlow.Enabled = txtClosureBy.ReadOnly = tbFunderDetails.Enabled = true;
                ddlLOB.Enabled = ddlMLA.Enabled = btnSave.Enabled = btnEmail.Enabled = false; // btnPrint.Enabled =
                //txtNOCDate.Enabled = CalendarNOCDate.Enabled = false;
                foreach (GridViewRow gvRow in grvCashFlow.Rows)
                {
                    ((TextBox)gvRow.FindControl("txtRemarks")).ReadOnly = true;
                }
                //if (bClearList)
                //{
                //    ddlBranch.ReadOnly = true;
                //}
                foreach (GridViewRow gvRow in grvCashFlow.Rows)
                {
                    TextBox txtWaived = (TextBox)gvRow.FindControl("txtWaived");
                    TextBox txtRemarks = (TextBox)gvRow.FindControl("txtRemarks");
                    txtWaived.ReadOnly = txtRemarks.ReadOnly = true;
                }
                break;
        }
    }

    #endregion [User Authorization]

    #region [To Get details for Modification]

    private void FunGetAccountsClosedForModify(int intClosureDetailId)
    {
        DataTable dtTable = new DataTable();
        try
        {
            //objAccountClosure_Client = new ContractMgtServicesReference.ContractMgtServicesClient();

            //byte[] bytesAccount = objAccountClosure_Client.FunGetAccountsClosedForModify(strAccountClosure, strPANum, strPANum + "DUMMY", intClosureDetailId, intCompanyID);

            //dtTable = (DataTable)ClsPubSerialize.DeSerialize(bytesAccount, SerializationMode.Binary, typeof(DataTable));

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Closure_Id", intClosureDetailId.ToString());


            DataSet dtClosure = Utility.GetDataset("S3G_LOANAD_GetAccountsClosedForModify", Procparam);
            dtTable = dtClosure.Tables[0];

            if (dtTable != null && dtTable.Rows.Count > 0)
            {
                txtAccClosureNo.Text = dtTable.Rows[0]["closure_no"].ToString();
                txtAccClosureDate.Text = DateTime.Parse(dtTable.Rows[0]["Closure_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                //txtNOCDate.Text = DateTime.Parse(dtTable.Rows[0]["NOD_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dtTable.Rows[0]["LOBName"].ToString(), dtTable.Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = dtTable.Rows[0]["LOBName"].ToString();
                //ddlBranch.SelectedValue = dtTable.Rows[0]["Location_ID"].ToString();
                //ddlBranch.SelectedText = dtTable.Rows[0]["Location_Name"].ToString();
                //ddlBranch.ToolTip = dtTable.Rows[0]["Location_Name"].ToString();
                ddlMLA.SelectedValue = dtTable.Rows[0]["pa_sa_ref_id"].ToString();
                ddlMLA.SelectedText = dtTable.Rows[0]["PANum"].ToString();
                ucdCustomer.SetCustomerDetails(dtTable.Rows[0], true);
                hdnCustomerID.Value = dtTable.Rows[0]["Customer_ID"].ToString();
                txtClosureBy.Text = dtTable.Rows[0]["User_Name"].ToString();

                if (!String.IsNullOrEmpty(dtTable.Rows[0]["Tranche_Id"].ToString()))
                {
                    ddlTrancheName.SelectedValue = dtTable.Rows[0]["Tranche_Id"].ToString();
                    ddlTrancheName.SelectedText = dtTable.Rows[0]["Tranche_Name"].ToString();
                }

                btnEmail.Enabled = btnPrint.Enabled = btnClosure.Enabled = btnSave.Enabled = false;
                ViewState["vwClosure_Status"] = dtTable.Rows[0]["Closure_Status_Code"].ToString();
                if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "1")
                {
                    txtStatus.Text = "Pending";
                    btnClosure.Enabled = btnSave.Enabled = true;
                    btnPrint.Enabled = true;
                }
                else if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "2")
                {
                    txtStatus.Text = "Under Progress";
                }
                else if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "3")
                {
                    txtStatus.Text = "Approved";
                    btnEmail.Enabled = btnPrint.Enabled = true;
                }
                else if (Convert.ToString(dtTable.Rows[0]["Closure_Status_Code"]) == "4")
                {
                    txtStatus.Text = "Rejected";
                }
                ddlLOB.Enabled =ddlMLA.Enabled = ddlTrancheName.Enabled = false;
            }

            if (dtClosure.Tables[1] != null && dtClosure.Tables[1].Rows.Count > 0)
            {
                Panel2.Visible = true;
                div2.Style.Add("display", "block");
                ViewState["ClosureStatement"] = dtClosure.Tables[1];
                FunPriBindCashFlowGrid();
            }
            if (dtClosure.Tables[2].Rows.Count > 0)
            {
                grvfunderdetailss.DataSource = dtClosure.Tables[2];
                grvfunderdetailss.DataBind();
                //dcfunderdue = Convert.ToDecimal(dtclosure.Tables[2].Compute("sum(due1)", ""));
                //dcfunderfuturerec = Convert.ToDecimal(dtclosure.Tables[2].Compute("sum(Future_receivables1)", ""));
                //dcfundernpv = Convert.ToDecimal(dtclosure.Tables[2].Compute("sum(NPV_amount1)", ""));

                //grvfunderdetailss.FooterRow.Cells[3].Text = dcfunderdue.ToString(Funsetsuffix());
                //grvfunderdetailss.FooterRow.Cells[4].Text = dcfunderfuturerec.ToString(Funsetsuffix());
                //grvfunderdetailss.FooterRow.Cells[5].Text = dcfundernpv.ToString(Funsetsuffix());

                ViewState["dtfunder"] = dtClosure.Tables[2];

               // txtToTal.Text = (Convert.ToDecimal(txtbreaking.Text.ToString()) + dcfundernpv).ToString(Funsetsuffix());

            }
        }
        catch (FaultException<ContractMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            FunPubStoreErrorMsgAndCloseObj(true, objFaultExp, null, objAccountClosure_Client);
            throw;
        }
        catch (Exception ex)
        {
            FunPubStoreErrorMsgAndCloseObj(true, null, ex, objAccountClosure_Client);
            throw;
        }
        finally
        {
            FunPubStoreErrorMsgAndCloseObj(false, null, null, objAccountClosure_Client);
        }
    }

    protected void txtAccClosureDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls();
            ddlMLA.Clear();
            ddlTrancheName.Clear();
            grvaccount.DataSource = null;
            grvaccount.DataBind();
            if (!string.IsNullOrEmpty(txtAccClosureDate.Text.Trim()))
            {
                if (Convert.ToInt32(Utility.StringToDate(txtAccClosureDate.Text.ToString()).ToString("yyyyMM")) > Convert.ToInt32(DateTime.Today.ToString("yyyyMM")))
                {
                    Utility.FunShowAlertMsg(this, "Rental Schedule Closure Date should be with in the current month");
                    txtAccClosureDate.Text = DateTime.Today.ToString(strDateFormat);
                }
            }
        }
        catch (Exception ex)
        {
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

    #endregion [To Get details for Modification]

    #region [FunPriGenerateAccountClosureDetailsXMLDet]

    /// <summary>
    /// Created by Tamilselvan.S
    /// Ceated Date 28/01/2011
    /// For Grid values to XML conversion
    /// </summary>
    /// <returns></returns>
    private string FunPriGenerateAccountClosureDetailsXMLDet()
    {
        try
        {
            strbAccountClosureDetails.Append("<Root>");
            foreach (GridViewRow gvRow in grvCashFlow.Rows)
            {
                TextBox txtRemarks = gvRow.FindControl("txtRemarks") as TextBox;
                TextBox txtWaived = gvRow.FindControl("txtWaived") as TextBox;
                Label lblClosure = gvRow.FindControl("lblClosure") as Label;
                Label lblpasaid = gvRow.FindControl("lblpasaid") as Label;
                Label lblpanum = gvRow.FindControl("lblpanum") as Label;
                Label lblDue = gvRow.FindControl("lblDue") as Label;
                Label lblReceived = gvRow.FindControl("lblReceived") as Label;
                HiddenField hdnCashFlowID = gvRow.FindControl("hdnCashFlowID") as HiddenField;

                strbAccountClosureDetails.Append("<Details ");
                strbAccountClosureDetails.Append(" Closure_Type_Code = '33'");
                strbAccountClosureDetails.Append(" Closure_Type = '1'");
                strbAccountClosureDetails.Append(" Closure_Status_Code = '1'");
                strbAccountClosureDetails.Append(" Closure_Status_Type_Code = '9'");
                strbAccountClosureDetails.Append(" Cashflow_Component = '" + hdnCashFlowID.Value + "'");
                strbAccountClosureDetails.Append(" Closure_Rate = '0'");
                strbAccountClosureDetails.Append(" Payable_Amount = '0'");
                strbAccountClosureDetails.Append(" panum = '" + lblpanum.Text.Trim() + "'");
                strbAccountClosureDetails.Append(" pasa_id = '" + lblpasaid.Text.Trim() + "'");
                strbAccountClosureDetails.Append(" SANum = '" + lblpanum.Text.Trim() + "DUMMY'");

                if (!string.IsNullOrEmpty(lblReceived.Text.Trim()))
                    strbAccountClosureDetails.Append(" Received_Amount = '" + lblReceived.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Received_Amount = '0'");

                if (!string.IsNullOrEmpty(txtWaived.Text.Trim()))
                    strbAccountClosureDetails.Append(" Waived_Amount = '" + txtWaived.Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Waived_Amount = '0'");

                if (!string.IsNullOrEmpty(grvCashFlow.FooterRow.Cells[4].Text.Trim()))
                    strbAccountClosureDetails.Append(" Closure_Amount = '" + grvCashFlow.FooterRow.Cells[4].Text.Trim() + "'");
                else
                    strbAccountClosureDetails.Append(" Closure_Amount = '0'");

                strbAccountClosureDetails.Append(" Remarks = '" + txtRemarks.Text.Trim() + "'");

                if (!string.IsNullOrEmpty(lblDue.Text.Trim()))
                    strbAccountClosureDetails.Append(" Due_Amount = '" + lblDue.Text.Trim() + "' ");
                else
                    strbAccountClosureDetails.Append(" Due_Amount = '0' ");

                strbAccountClosureDetails.Append(" PreClosure_Type = '0' ");
                strbAccountClosureDetails.Append(" Closure_Details_ID = '" + intClosureDetailId.ToString() + "' ");

                if (!string.IsNullOrEmpty(lblClosure.Text.Trim()))
                    strbAccountClosureDetails.Append(" AccClosure_Amount = '" + lblClosure.Text.Trim() + "' ");
                else
                    strbAccountClosureDetails.Append(" AccClosure_Amount = '0' ");

                strbAccountClosureDetails.Append(" />");
            }
            strbAccountClosureDetails.Append("</Root>");
            strXMLAccountClosureDetails = strbAccountClosureDetails.ToString();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        return strXMLAccountClosureDetails;
    }

    #endregion [FunPriGenerateAccountClosureDetailsXMLDet]

    #region [Set ErrorMessage for control]

    public void FunPubSetErrorMessageControl()
    {
        rfvLineOfBusiness.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_LOB;
        rfvClsoureBy.ErrorMessage = Resources.ValidationMsgs.S3G_ValMsg_AcClosure_Done;
    }

    #endregion [Set ErrorMessage for control]

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            PreviewPDF_Click(true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvAccountClosure.ErrorMessage = ex.Message;
            cvAccountClosure.IsValid = false;
        }
    }

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
        Procparam.Add("@Program_Id", "073");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue.ToString());
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Program_id", obj_Page.strProgramId);

        if (obj_Page.txtAccClosureDate.Text.Trim() != "")
            Procparam.Add("@PMCDate", Utility.StringToDate(obj_Page.txtAccClosureDate.Text.Trim()).ToString());

        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTranche_AGT", Procparam));

        return suggestions.ToArray();
    }

}
